/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.16
 *
 * CHANGELOG
 * v1.16, 241013
 * [UPD] cleanup
 * v1.13.12, 230116
 * [UPD] Implemented LfdReader and removed unused code
 * v1.13.10, 221018
 * [UPD] UpdateParams now includes Roll, leaving Matrix3(,) unused
 * v1.11, 210801
 * [UPD] XWA wireframes load default profile to account for additional hook meshes [JB]
 * v1.10, 210520
 * [FIX #58, #59] XWA wireframes when using rotations instead of Waypoints [JB]
 * v1.8, 201004
 * [FIX] _dosSpeciesMap now checks for existing resource before adding [JB]
 * [UPD] XML, cleanup
 * [UPD] Vector3 and Vector3_Int16 renamed to Vertex*, since that what it is
 * [FIX] XZ yaw calculation
 * [UPD] Changed namespace to reduce visibility
 * v1.7, 200816
 * [NEW] created [JB]
 */

using Idmr.LfdReader;
using System;
using System.Collections.Generic;
using System.IO;

/* Special thanks to:
 * Jérémy Ansel for documentation of the OPT format https://github.com/JeremyAnsel/XwaOptEditor
 * Rob for documentation of the CRFT, CPLX and SHIP formats used by XW93, XW94, and TIE DOS.
 */

/* [JB] I am not familiar with 3D math so there may potentially be errors or inefficiencies in the wireframe
 * implementation here. My prototyping code was originally written in C++ and ported here. I hope the result
 * is adequate.
 * 
 * WireframeManager is the central class that's exposed to the map form. Everything is abstracted away from the
 * map, so it can request a wireframe for a craft type and flight group, update it depending on orientation and
 * zoom, and render it on screen. All without the map needing to keep track of anything. Behind the scenes, the
 * Manager utilizes many other classes that load the wireframes, manipulate the data, and transform the result:
 *   OptFile: temporary container to parse and load OPT meshes.
 *   CraftFile: temporary container to parse and load CRFT, CPLX, or SHIP meshes.
 *   WireframeDefinition: converted from a loaded OPT or Craft for a particular craft type, organized into layers.
 *     MeshLayerDefinition: contains filtered vertices and line segments for a particular mesh type.
 *   WireframeInstance: a single instance of a craft wireframe to be drawn on the map.
 *     MeshLayerInstance: vertices are transformed into screen coordinates relative to the mesh origin.
 */

namespace Idmr.Yogeme.MapWireframe
{
	/// <summary>All nodes within the tree structure have a type that determines its data format.</summary>
	public enum OptNodeType
	{
		NullNode = -1,
		NodeGroup = 0,
		FaceData = 1,
		MeshVertices = 3,
		NodeReference = 7,
		VertexNormals = 11,
		TextureCoordinates = 13,
		Texture = 20,
		FaceGrouping = 21,  // aka Mesh Data
		Hardpoint = 22,
		RotationScale = 23,
		NodeSwitch = 24,
		MeshDescriptor = 25,
		TextureAlpha = 26,
		EngineGlow = 28,
	}

	/// <summary>All possible mesh types for each component in the model.</summary>
	/// <remarks>Used in the OPT and SHIP formats. Currently unknown if they're present in the older CRFT or CPLX formats.</remarks>
	public enum MeshType
	{
		Default = 0,
		MainHull,
		Wing,
		Fuselage,
		GunTurret,
		SmallGun,
		Engine,
		Bridge,
		ShieldGenerator,
		EnergyGenerator,
		Launcher,
		CommunicationSystem,
		BeamSystem,
		CommandSystem,
		DockingPlatform,
		LandingPlatform,
		Hangar,
		CargoPod,
		MiscHull,
		Antenna,
		RotaryWing,
		RotaryGunTurret,
		RotaryLauncher,
		RotaryCommunicationSystem,
		RotaryBeamSystem,
		RotaryCommandSystem,
		Hatch,
		Custom,
		WeaponSystem1,
		WeaponSystem2,
		PowerRegenerator,
		Reactor
	}

	/// <summary>Represents a single point within a mesh</summary>
	public class Vertex
	{
		/// <summary>Initialize a point at the given coordinates</summary>
		public Vertex(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		/// <summary>Initialize a point at the given coordinates</summary>
		public Vertex(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		/// <summary>Initialize a duplicate point</summary>
		public Vertex(Vertex other)
		{
			X = other.X;
			Y = other.Y;
			Z = other.Z;
		}

		/// <summary>Apply a rotation matrix and transpose the Vertex about the origin</summary>
		/// <param name="mat">The 3d rotation matrix</param>
		public void MultTranspose(Matrix3 mat)
		{
			float vx = X;
			float vy = Y;
			float vz = Z;
			X = (float)(mat.V11 * vx + mat.V21 * vy + mat.V31 * vz);
			Y = (float)(mat.V12 * vx + mat.V22 * vy + mat.V32 * vz);
			Z = (float)(mat.V13 * vx + mat.V23 * vy + mat.V33 * vz);
		}

		/// <summary>Gets the X value</summary>
		public float X { get; internal set; }
		/// <summary>Gets the Y value</summary>
		public float Y { get; internal set; }
		/// <summary>Gets the Z value</summary>
		public float Z { get; internal set; }
	}
	/// <summary>Represents a 3x3 matrix for rotational transformations</summary>
	public class Matrix3
	{
		/// <summary>Initializes a matrix for a rotational transform, combining pitch, yaw, and roll.</summary>
		/// <remarks>In a normal coordinate system, this would be matrix multiplication for (Pitch * Roll * Yaw) in that order.</br>
		/// But X-Wing uses a different system, so we use (CraftRoll * CraftPitch * CraftYaw) to get the desired results on screen.</remarks>
		public Matrix3(double yaw, double pitch, double roll)
		{
			V11 = Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(roll) * Math.Sin(pitch) * Math.Sin(yaw);
			V12 = Math.Cos(roll) * -Math.Sin(yaw) + Math.Sin(roll) * Math.Sin(pitch) * Math.Cos(yaw);
			V13 = Math.Sin(roll) * Math.Cos(pitch);
			V21 = Math.Cos(pitch) * Math.Sin(yaw);
			V22 = Math.Cos(pitch) * Math.Cos(yaw);
			V23 = -Math.Sin(pitch);
			V31 = -Math.Sin(roll) * Math.Cos(yaw) + Math.Cos(roll) * Math.Sin(pitch) * Math.Sin(yaw);
			V32 = -Math.Sin(roll) * -Math.Sin(yaw) + Math.Cos(roll) * Math.Sin(pitch) * Math.Cos(yaw);
			V33 = Math.Cos(roll) * Math.Cos(pitch);
		}
	
		/// <summary>Applies a zoom transformation, multiplying by a scalar.</summary>
		public void Scale(double mult)
		{
			V11 *= mult;
			V12 *= mult;
			V13 *= mult;
			V21 *= mult;
			V22 *= mult;
			V23 *= mult;
			V31 *= mult;
			V32 *= mult;
			V33 *= mult;
		}

		public double V11 { get; private set; }
		public double V12 { get; private set; }
		public double V13 { get; private set; }
		public double V21 { get; private set; }
		public double V22 { get; private set; }
		public double V23 { get; private set; }
		public double V31 { get; private set; }
		public double V32 { get; private set; }
		public double V33 { get; private set; }
	}

	/// <summary>Exposes some useful defaults and functions to assist program configuration for <see cref="MeshType"/> visibility.</summary>
	public static class MeshTypeHelper
	{
		// These predefined arrays help initialize the user's configuration as well as offering quick toggles to include or exclude a selection of visible mesh types.
		public static MeshType[] DefaultMeshes { get; } = new MeshType[] { MeshType.Default, MeshType.MainHull, MeshType.Wing, MeshType.Fuselage, MeshType.Bridge, MeshType.DockingPlatform, MeshType.LandingPlatform, MeshType.Hangar, MeshType.CargoPod, MeshType.MiscHull, MeshType.Engine, MeshType.RotaryWing, MeshType.Launcher };
		public static MeshType[] HullMeshes { get; } = new MeshType[] { MeshType.Default, MeshType.MainHull, MeshType.Wing, MeshType.Fuselage, MeshType.Engine, MeshType.Bridge, MeshType.Launcher, MeshType.MiscHull, MeshType.RotaryWing };
		public static MeshType[] MiscMeshes { get; } = new MeshType[] { MeshType.ShieldGenerator, MeshType.EnergyGenerator, MeshType.CommunicationSystem, MeshType.BeamSystem, MeshType.CommandSystem, MeshType.CargoPod, MeshType.Antenna, MeshType.RotaryCommunicationSystem, MeshType.RotaryBeamSystem, MeshType.RotaryCommandSystem, MeshType.Hatch, MeshType.Custom, MeshType.PowerRegenerator, MeshType.Reactor };
		public static MeshType[] WeaponMeshes { get; } = new MeshType[] { MeshType.GunTurret, MeshType.SmallGun, MeshType.RotaryGunTurret, MeshType.RotaryLauncher, MeshType.WeaponSystem1, MeshType.WeaponSystem2 };
		public static MeshType[] HangarMeshes { get; } = new MeshType[] { MeshType.DockingPlatform, MeshType.LandingPlatform, MeshType.Hangar };

		/// <summary>Returns the default MeshTypes combined into a single value.</summary>
		public static long GetDefaultFlags() => GetFlags(DefaultMeshes);

		/// <summary>Combines an array of enum-based MeshTypes into a single value.</summary>
		public static long GetFlags(MeshType[] list)
		{
			long retval = 0;
			foreach (MeshType value in list) retval |= (long)(1 << (int)value);
			return retval;
		}

		/// <summary>Combines an int array into a single value.</summary>
		public static long GetFlags(int[] list)
		{
			long retval = 0;
			foreach (int value in list) retval |= (long)(1 << value);
			return retval;
		}
	}

	/// <summary>Container to store the vertices of a single polygon face, which may have 3 or 4 vertices.</summary>
	public struct OptFace
	{
		/// <summary>Initialize the face with the designated vertices</summary>
		/// <param name="v1">Index of first vertex</param>
		/// <param name="v2">Index of second vertex</param>
		/// <param name="v3">Index of third vertex</param>
		/// <param name="v4">Index of fourth vertex</param>
		public OptFace(int v1, int v2, int v3, int v4)
		{
			VertexIndex = new int[4];
			VertexIndex[0] = v1;
			VertexIndex[1] = v2;
			VertexIndex[2] = v3;
			VertexIndex[3] = v4;
		}

		/// <summary>Gets the Vertex array</summary>
		/// <remarks>Length is <b>4</b></remarks>
		public int[] VertexIndex { get; private set; }
	}

	/// <summary>Represents the mesh faces of a single LOD.</summary>
	public class OptLod
	{
		/// <summary>Initializes the LOD at the maximum value</summary>
		public OptLod() => Distance = float.MaxValue;
		/// <summary>Initializes the LOD with the specified distance</summary>
		/// <param name="dist">The distance assigned to the LOD</param>
		public OptLod(float dist) => Distance = dist;

		/// <summary>Gets the LOD distance</summary>
		/// <remarks>This isn't referenced or used anywhere</remarks>
		public float Distance { get; private set; }
		/// <summary>Gets the faces of the LOD</summary>
		public List<OptFace> Faces { get; } = new List<OptFace>();
	}

	/// <summary>Represents the loaded information for a single component, which is a top-level node in the OPT tree.</summary>
	public class OptComponent
	{
		/// <summary>Gets or sets the OPT node type</summary>
		public OptNodeType NodeType { get; set; } = 0;
		/// <summary>Gets or sets the mesh type</summary>
		public MeshType MeshType { get; set; } = MeshType.Default;
		/// <summary>Gets or sets the LOD index</summary>
		public int LoadingLodIndex { get; set; } = 0;
		/// <summary>Gets the Vertices of the mesh</summary>
		public List<Vertex> Vertices { get; } = new List<Vertex>();
		/// <summary>Gets the LODs</summary>
		public List<OptLod> Lods { get; } = new List<OptLod>();
	}

	/// <summary>Represents an OPT model file.</summary>
	/// <remarks>The OPT format was introduced with XvT/BoP, continued in XWA, and retrofitted into XWING and TIE for the Windows versions.<br/>
	/// The format is a tree of nodes, utilizing C-style pointers to navigate each node in the tree and access their data elements.</remarks>
	public class OptFile
	{
		int _basePosition;  // The file contents begin with some meta data that isn't part of the actual model data. This will be the stream position where the real data begins.
		int _globalOffset;  // The first piece of real data in the file is a pointer to itself. Since the entire file would be contiguous in memory, subtracting from any other pointer address gives us a relative offset into the file.

		/// <summary>Initializes and attempts to load the contents from file.</summary>
		/// <param name="filename">Path and filename of the OPT model to load.</param>
		/// <param name="checkProfile">For XWAU compatibility, indicates that we should check for any corresponding model profiles.</param>
		/// <returns>Returns <b>true</b> if the file is loaded.</returns>
		public bool LoadFromFile(string filename, bool checkProfile)
		{
			try
			{
				if (!File.Exists(filename)) return false;

				List<int> meshFilter = checkProfile ? getXwauDefaultProfileFilter(filename) : null;

				using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					using (BinaryReader br = new BinaryReader(fs))
					{
						int version = br.ReadInt32();
						if (version <= 0) fs.Position += 4;

						parseTopNodes(fs, br, meshFilter);
					}
				}
			}
			catch { return false; }
			return true;
		}

		/// <summary>Checks a model for its corresponding profile definitions, scanning for meshes that should be deactivated in the default display.</summary>
		/// <remarks>XWAU models can consolidate multiple profiles of craft meshes into a single OPT file, then deactivate meshes depending on <br/>
		/// the chosen profile.  Loading the entire OPT as a wireframe may include superfluous geometry.</remarks>
		/// <param name="filename">Path and filename of the OPT model to check.</param>
		/// <returns>Returns a list of top-level node indices that must be filtered out when loading the <b>Default</b> wireframe.  This can be null.</returns>
		List<int> getXwauDefaultProfileFilter(string filename)
		{
			// According to the hook source code and documentation on the forum (https://www.xwaupgrade.com/phpBB3/viewtopic.php?f=33&t=13090&p=176375), the search criteria is:
			//   FlightModels\[Model].opt
			// It will first look for a standalone TXT file, prefixed with the model name:
			//   FlightModels\[Model]ObjectProfiles.txt
			// If that fails, attempts to find the [ObjectProfiles] section inside a matching INI file:
			//   FlightModels\[Model].ini
			string iniFilename = "";
			int pathPos = filename.LastIndexOf('\\');
			if (pathPos >= 0)
			{
				string path = filename.Substring(0, pathPos + 1);
				string model = filename.Substring(pathPos + 1);
				int extPos = model.LastIndexOf(".");
				if (extPos >= 0) model = model.Substring(0, extPos);

				iniFilename = path + model + "ObjectProfiles.txt";
			}

			// If loading from the TXT file, we don't need the INI section header.
			bool inSection = true;
			if (!File.Exists(iniFilename))
			{
				// Load from INI instead, which is the most likely place to find this information.
				iniFilename = filename.ToUpper().Replace(".OPT", ".INI");
				inSection = false;
			}

			if (!File.Exists(iniFilename)) return null;

			List<int> meshFilter = null;
			try
			{
				// Parse any relevant text, retrieve the list of deactivated meshes for the "Default" model.
				Dictionary<string, string> objectProfiles = new Dictionary<string, string>();
				using (StreamReader sr = new StreamReader(iniFilename))
				{
					while (!sr.EndOfStream)
					{
						string line = sr.ReadLine();
						int pos = line.IndexOf(";");
						if (pos >= 0) line = line.Substring(0, pos);
						line = line.Trim().Replace("\t", "");

						if (line.StartsWith("[")) inSection = (line == "[ObjectProfiles]");
						else if (inSection && line.Length > 0)
						{
							string[] tokens = line.Split('=');
							if (tokens.Length == 2) objectProfiles.Add(tokens[0].ToUpper().Trim(), tokens[1].Trim());
						}
					}
				}
				if (objectProfiles.Count > 0)
				{
					meshFilter = new List<int>();
					foreach (var item in objectProfiles)
					{
						if (item.Key == "DEFAULT")
						{
							string[] tokens = item.Value.Split(',');
							for (int i = 0; i < tokens.Length; i++)
							{
								int result = -1;
								int.TryParse(tokens[i].Trim(), out result);
								meshFilter.Add(result);
							}
						}
					}
				}
			}
			catch { /* do nothing */ }
			return meshFilter;
		}

		/// <summary>Parses all top-level nodes.</summary>
		/// <remarks>Typically each top-level node is a single component of a particular MeshType. Its MeshType and mesh information will be defined somewhere in its child node tree.</remarks>
		void parseTopNodes(FileStream fs, BinaryReader br, List<int> meshFilter)
		{
			_basePosition = (int)fs.Position;
			_globalOffset = br.ReadInt32();
			fs.Position += 2;  // Skip Int16 heap index.
			int nodeCount = br.ReadInt32();
			int nodeTableOffset = br.ReadInt32();

			if (nodeTableOffset != 0) nodeTableOffset -= _globalOffset;

			Components = new List<OptComponent>();
			for (int i = 0; i < nodeCount; i++)
			{
				// XWAU may require us to filter out extra meshes that shouldn't appear on the default model.
				if (meshFilter != null && meshFilter.Contains(i)) continue;

				fs.Position = _basePosition + nodeTableOffset + (i * 4);
				int nodeOffset = br.ReadInt32();
				if (nodeOffset != 0)
				{
					nodeOffset -= _globalOffset;
					OptComponent comp = new OptComponent();
					fs.Position = _basePosition + nodeOffset;
					parseChildNodes(fs, br, comp);
					Components.Add(comp);
				}
			}
		}

		/// <summary>Recursively parses all child nodes of a top-level node.</summary>
		/// <remarks>Loads any relevant data into the specified component object.</remarks>
		void parseChildNodes(FileStream fs, BinaryReader br, OptComponent node)
		{
			fs.Position += 4;
			OptNodeType nodeType = (OptNodeType)br.ReadInt32();
			int childNodeCount = br.ReadInt32();
			int childNodeOffset = br.ReadInt32();
			int dataCount = br.ReadInt32();
			int dataOffset = br.ReadInt32();
			if (childNodeOffset != 0) childNodeOffset -= _globalOffset;

			node.NodeType = nodeType;
			if (dataOffset != 0) dataOffset -= _globalOffset;

			switch (nodeType)
			{
				case OptNodeType.MeshVertices:
					if (dataOffset == 0) break;
					fs.Position = _basePosition + dataOffset;
					for (int i = 0; i < dataCount; i++)
					{
						float x = br.ReadSingle();
						float y = br.ReadSingle();
						float z = br.ReadSingle();
						node.Vertices.Add(new Vertex(x, y, z));
					}
					break;
				case OptNodeType.FaceData:
					if (dataOffset == 0 || node.LoadingLodIndex >= node.Lods.Count) break;
					fs.Position = _basePosition + dataOffset + 4;  // Into the data, skipping Int32 edgeCount.
					for (int i = 0; i < dataCount; i++)
					{
						int v1 = br.ReadInt32();
						int v2 = br.ReadInt32();
						int v3 = br.ReadInt32();
						int v4 = br.ReadInt32();
						node.Lods[node.LoadingLodIndex].Faces.Add(new OptFace(v1, v2, v3, v4));
						fs.Position += 48;  // Advance to next face.
					}
					break;
				case OptNodeType.MeshDescriptor:
					if (dataOffset == 0) break;
					fs.Position = _basePosition + dataOffset;
					node.MeshType = (MeshType)br.ReadInt32();
					break;
				case OptNodeType.FaceGrouping:
					if (dataOffset == 0) break;
					fs.Position = _basePosition + dataOffset;
					for (int i = 0; i < dataCount; i++)
					{
						float distance = br.ReadSingle();
						node.Lods.Add(new OptLod(distance));
					}
					while (node.Lods.Count < childNodeCount) { node.Lods.Add(new OptLod()); }
					for (int i = 0; i < childNodeCount; i++)
					{
						node.LoadingLodIndex = i;
						fs.Position = _basePosition + childNodeOffset + (i * 4);
						int nodeOffset = br.ReadInt32();
						if (nodeOffset != 0)
						{
							nodeOffset -= _globalOffset;
							fs.Position = _basePosition + nodeOffset;
							parseChildNodes(fs, br, node);
						}
					}
					break;
			}
			// We already had a special case for FaceGrouping.
			if (nodeType != OptNodeType.FaceGrouping)
			{
				for (int i = 0; i < childNodeCount; i++)
				{
					fs.Position = _basePosition + childNodeOffset + (i * 4);
					int nodeOffset = br.ReadInt32();
					if (nodeOffset != 0)
					{
						nodeOffset -= _globalOffset;
						fs.Position = _basePosition + nodeOffset;
						parseChildNodes(fs, br, node);
					}
				}
			}
		}

		/// <summary>Gets the list of top-level nodes</summary>
		public List<OptComponent> Components { get; private set; } = new List<OptComponent>();
	}

	/// <summary>Represents a single line within a mesh</summary>
	/// <remarks>The indices point to a <see cref="Vertex"/> within a parent <see cref="MeshLayerDefinition"/></remarks>
	public class Line
	{
		/// <summary>Initialize with the indicated vertices</summary>
		/// <param name="v1">The index of the start vertex</param>
		/// <param name="v2">The index of the end vertex</param>
		public Line(int v1, int v2)
		{
			V1 = v1;
			V2 = v2;
		}

		/// <summary>Gets the index of the start vertex</summary>
		public int V1 { get; private set; }
		/// <summary>Gets the index of the end vertex</summary>
		public int V2 { get; private set; }
	}

	/// <summary>Represents a DOS craft format (CRFT, CPLX, and SHIP).</summary>
	/// <remarks>The format is similar to OPT in the sense that it contains a list of components, along with various pieces of data. These resources are typically packed into uncompressed LFD archives, which are automatically handled by the loading functions.</remarks>
	public class CraftFile
	{
		Resource.ResourceType _lfdCraftFormat = Resource.ResourceType.Undefined;

		/// <summary>Initializes the object and attempts to load relevant data from a standalone file.</summary>
		/// <param name="craftFormat">The type of the file data.</param>
		/// <param name="cftFile">The filename of the individual CFT file</param>
		/// <returns><b>true</b> if successfully loaded, otherwise <b>false</b>.</returns>
		/// <remarks>Requires the craft file format to already be known, as the context cannot be determined from the file contents.<br/>
		/// Can technically handle a <paramref name="craftFormat"/> of CRFT, CPLX or SHIP, although should only occur with processing B-WING.CFT, which is CRFT.</remarks>
		public bool LoadCftFile(string cftFile, Resource.ResourceType craftFormat)
		{
			if (!File.Exists(cftFile) || craftFormat == Resource.ResourceType.Undefined) return false;
			_lfdCraftFormat = craftFormat;

			try
			{
				if (_lfdCraftFormat == Resource.ResourceType.Crft || _lfdCraftFormat == Resource.ResourceType.Cplx) parseXwing(cftFile, 0);
				else if (_lfdCraftFormat == Resource.ResourceType.Ship) Craft = new Ship(cftFile, 0);
				else return false;  // just in case the Type is something random, shouldn't ever happen
			}
			catch { return false; }
            
			return true;
		}

		/// <summary>Initializes the object and attempts to load relevant data from within an LFD archive.</summary>
		/// <param name="archiveName">The SPECIES LFD file</param>
		/// <param name="resourceName">The resource within <paramref name="archiveName"/>.</param>
		/// <returns><b>true</b> if successful, otherwise <b>false</b>.</returns>
		public bool LoadFromArchive(string archiveName, string resourceName)
		{
			_lfdCraftFormat = Resource.ResourceType.Undefined;
            if (!File.Exists(archiveName) || Resource.GetType(archiveName, 0) != Resource.ResourceType.Rmap) return false;

			var rmap = new Rmap(archiveName);
			int index = 0;
			for (; index < rmap.NumberOfHeaders; index++) if (rmap.SubHeaders[index].Name.ToUpper() == resourceName.ToUpper()) break;
			if (index == rmap.NumberOfHeaders) return false;	// didn't find the name

			_lfdCraftFormat = rmap.SubHeaders[index].Type;
			try
			{
				if (_lfdCraftFormat == Resource.ResourceType.Crft || _lfdCraftFormat == Resource.ResourceType.Cplx) parseXwing(archiveName, rmap.SubHeaders[index].Offset);
				else if (_lfdCraftFormat == Resource.ResourceType.Ship) Craft = new Ship(archiveName, rmap.SubHeaders[index].Offset);
                else return false;  // just in case the Type is something random, shouldn't ever happen
            }
			catch { return false; }

			return true;
		}

		/// <summary>Parses the header and top-level contents of the CRFT and CPLX formats used in XWING.</summary>
		void parseXwing(string lfdName, long offset)
		{
			Ship ship;
			if (_lfdCraftFormat == Resource.ResourceType.Crft)
			{
				Crft crft = new Crft(lfdName, offset);
				ship = crft;
			}
			else
			{
				Cplx cplx = new Cplx(lfdName, offset);
				ship = cplx;
            }

			Craft = ship;
		}

		public Ship Craft { get; private set; }
	}

	/// <summary>Represents a compiled list of vertices and lines derived from the mesh and its faces.</summary>
	/// <remarks>Multiple components of the same MeshType will added into the same layer.</remarks>
	public class MeshLayerDefinition
	{
		/// <summary>Initialize an empty definition of the assigned type</summary>
		/// <param name="createMeshType">The type to assign</param>
		public MeshLayerDefinition(MeshType createMeshType) => MeshType = createMeshType;

		/// <summary>Gets the definition's type</summary>
		public MeshType MeshType { get; private set; }
		/// <summary>Gets the vertices of the mesh in raw model coordinates</summary>
		public List<Vertex> Vertices { get; } = new List<Vertex>();
		/// <summary>Gets the lines that make up the form of the mesh</summary>
		public List<Line> Lines { get; } = new List<Line>();
	}

	/// <summary>Represents a finalized wireframe definition that is ready for use in the map.</summary>
	/// <remarks>It can be generated from an OptFile or CraftFile.</remarks>
	public class WireframeDefinition
	{
		/// <summary>Creates a definition from a loaded OPT.</summary>
		/// <param name="opt">The source OPT object</param>
		/// <remarks>Performs some basic optimization to prevent shared edges, so that lines don't have to be drawn twice.</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "assignment function performs other necessary actions")]
		public WireframeDefinition(OptFile opt)
		{
			MeshLayerDefinition layer = getOrCreateMeshLayerDefinition(MeshType.MainHull);  // Create a default entry so that it's first in the list, for drawing purposes.

			foreach (OptComponent comp in opt.Components)
			{
				if (comp.Lods.Count == 0) continue;

				int[] vertUsed = new int[comp.Vertices.Count];
				HashSet<int> lineUsed = new HashSet<int>();

				layer = getOrCreateMeshLayerDefinition(comp.MeshType);
				foreach (OptFace face in comp.Lods[0].Faces)
				{
					for (int i = 0; i < 4; i++)
					{
						int vi = face.VertexIndex[i];
						// If the face is a triangle (rather than a quad), the last index will be -1.
						if (vi == -1) continue;
						if (vertUsed[vi] == 0)
						{
							layer.Vertices.Add(comp.Vertices[vi]);
							vertUsed[vi] = layer.Vertices.Count;  // One-based value.
						}
					}
					for (int i = 0; i < 3; i++)
					{
						int v1 = face.VertexIndex[i];
						int v2 = face.VertexIndex[i + 1];
						if (v1 == -1) continue;
						// For a triangle, the last vertex is missing. Link back to the first vertex in the face.
						if (v2 == -1) v2 = face.VertexIndex[0];

						// Normalize and construct a key to determine if this line already exists. Add if it doesn't.
						if (v2 < v1)
						{
							int temp = v1;
							v1 = v2;
							v2 = temp;
						}
						int key = v1 | (v2 << 16);
						if (!lineUsed.Contains(key))
						{
							v1 = vertUsed[v1] - 1; // Convert from one-based back to zero-based.
							v2 = vertUsed[v2] - 1;
							layer.Lines.Add(new Line(v1, v2));
							lineUsed.Add(key);
						}
					}
					// If the face is quadrilateral, we haven't linked the first and last vertices.
					// Perform the same thing as above.
					if (face.VertexIndex[0] >= 0 && face.VertexIndex[3] >= 0)
					{
						int v1 = face.VertexIndex[0];
						int v2 = face.VertexIndex[3];
						if (v2 < v1)
						{
							int temp = v1;
							v1 = v2;
							v2 = temp;
						}
						int key = v1 | (v2 << 16);
						if (!lineUsed.Contains(key))
						{
							v1 = vertUsed[v1] - 1; // Convert from one-based back to zero-based.
							v2 = vertUsed[v2] - 1;
							layer.Lines.Add(new Line(v1, v2));
							lineUsed.Add(key);
						}
					}
				}
			}
			calculateSize();
		}

		/// <summary>Creates a definition from a loaded DOS craft format (CRFT, CPLX, SHIP).</summary>
		/// <param name="craft">The source DOS craft object</param>
		/// <remarks>Performs some basic optimization to prevent shared edges, so that lines don't have to be drawn twice.</remarks>
		public WireframeDefinition(CraftFile craft)
		{
			if (craft == null || craft.Craft == null) return;

			// This function is conceptually similar to creating from OPT, except we already have our lines and don't need to examine the faces.
#pragma warning disable IDE0059 // Unnecessary assignment of a value: assignment function performs other necessary actions
			MeshLayerDefinition layer = getOrCreateMeshLayerDefinition(MeshType.MainHull);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
			for (int i = 0; i < craft.Craft.Components.Length; i++)
			{
				var comp = craft.Craft.Components[i];
				if (comp.Lods.Length == 0)
					continue;
				var lod = comp.Lods[0];

				int[] vertUsed = new int[lod.MeshVertices.Length];
				HashSet<int> lineUsed = new HashSet<int>();

				layer = getOrCreateMeshLayerDefinition((MeshType)comp.MeshType);
				for (int j = 0; j < lod.Shapes.Length; j++)
				{
					for (int l = 0; l < lod.Shapes[j].Lines.Length; l++)
					{
						int v1, v2;
						v1 = lod.Shapes[j].Lines[l].Vertex1;
						v2 = lod.Shapes[j].Lines[l].Vertex2;

						if (vertUsed[v1] == 0)
						{
							layer.Vertices.Add(new Vertex(lod.MeshVertices[v1].X, lod.MeshVertices[v1].Y, lod.MeshVertices[v1].Z));
							vertUsed[v1] = layer.Vertices.Count; // One-based.
						}
						if (vertUsed[v2] == 0)
						{
							layer.Vertices.Add(new Vertex(lod.MeshVertices[v2].X, lod.MeshVertices[v2].Y, lod.MeshVertices[v2].Z));
							vertUsed[v2] = layer.Vertices.Count;
						}

						// Normalize and construct a key to determine if this line already exists. Add if it doesn't.
						if (v2 < v1)
						{
							int temp = v1;
							v1 = v2;
							v2 = temp;
						}
						int key = v1 | (v2 << 16);
						if (!lineUsed.Contains(key))
						{
							v1 = vertUsed[v1] - 1;  // Convert back to zero-based index.
							v2 = vertUsed[v2] - 1;
							layer.Lines.Add(new Line(v1, v2));
							lineUsed.Add(key);
						}
					}
				}
			}
			calculateSize();
		}

		/// <summary>Applies a scale to all vertices.</summary>
		/// <param name="scale">The positive scale value</param>
		/// <remarks>Needed for DOS models. Most craft must be scaled by 0.5 to get their proper size. The largest ships need to be scaled by 2.0</remarks>
		public void Scale(float scale)
		{
			if (scale <= 0)
				return;
			LongestSpanRaw = (int)(LongestSpanRaw * scale);
			foreach (MeshLayerDefinition layer in MeshLayerDefinitions)
			{
				foreach (Vertex vert in layer.Vertices)
				{
					vert.X *= scale;
					vert.Y *= scale;
					vert.Z *= scale;
				}
			}
		}

		/// <summary>Scans all vertices in the entire wireframe to determine its largest axis span based on its bounding box.</summary>
		private void calculateSize()
		{
			int vcount = 0;
			foreach (MeshLayerDefinition layer in MeshLayerDefinitions)
			{
				vcount += layer.Vertices.Count;
			}
			if (vcount == 0) // The model wasn't loaded.
			{
				LongestSpanRaw = 0;
				return;
			}
			float minX = float.MaxValue;
			float maxX = float.MinValue;
			float minY = float.MaxValue;
			float maxY = float.MinValue;
			float minZ = float.MaxValue;
			float maxZ = float.MinValue;
			foreach (MeshLayerDefinition layer in MeshLayerDefinitions)
			{
				foreach (Vertex v in layer.Vertices)
				{
					if (v.X < minX) minX = v.X;
					if (v.X > maxX) maxX = v.X;
					if (v.Y < minY) minY = v.Y;
					if (v.Y > maxY) maxY = v.Y;
					if (v.Z < minZ) minZ = v.Z;
					if (v.Z > maxZ) maxZ = v.Z;
				}
			}
			int spanX = (int)(maxX - minX);
			int spanY = (int)(maxY - minY);
			int spanZ = (int)(maxZ - minZ);
			LongestSpanRaw = spanX;
			if (spanY > LongestSpanRaw) LongestSpanRaw = spanY;
			if (spanZ > LongestSpanRaw) LongestSpanRaw = spanZ;
		}

		/// <summary>Retrieves a layer for a particular MeshType. Creates an empty layer if it doesn't exist.</summary>
		private MeshLayerDefinition getOrCreateMeshLayerDefinition(MeshType meshType)
		{
			foreach (MeshLayerDefinition layer in MeshLayerDefinitions)
			{
				if (layer.MeshType == meshType)
					return layer;
			}
			MeshLayerDefinition entry = new MeshLayerDefinition(meshType);
			MeshLayerDefinitions.Add(entry);
			return entry;
		}

		/// <summary>Gets the span of the widest dimension, derived from bounding box, expressed in raw units (40960 units = 1 km)</summary>
		public int LongestSpanRaw { get; private set; }
		/// <summary>Gets the span of the widest dimension, derived from bounding box, expressed in meters.</summary>
		public int LongestSpanMeters { get { return (int)(LongestSpanRaw / 40.96); } }
		/// <summary>Gets the mesh source</summary>
		public List<MeshLayerDefinition> MeshLayerDefinitions { get; } = new List<MeshLayerDefinition>();
	}

	/// <summary>Represents a local instance of a <see cref="MeshLayerDefinition"/> that can be transformed without altering the definition.</summary>
	public class MeshLayerInstance
	{
		/// <summary>Creates a new instance from the specified definition</summary>
		/// <param name="def">The original mesh source</param>
		public MeshLayerInstance(MeshLayerDefinition def)
		{
			MeshLayerDefinition = def;
			if (def != null)
			{
				Vertices.Capacity = def.Vertices.Count;
				for (int i = 0; i < def.Vertices.Count; i++) Vertices.Add(new Vertex(def.Vertices[i]));
			}
		}

		/// <summary>Returns if the instance is included in the visibility flags</summary>
		/// <param name="meshVisibilityFilter">The flags determining which mesh types to display</param>
		/// <returns><b>true</b> if the <see cref="MeshLayerDefinition.MeshType"/> is included</returns>
		public bool MatchMeshFilter(long meshVisibilityFilter) => MeshLayerDefinition != null && (meshVisibilityFilter & (1 << (int)MeshLayerDefinition.MeshType)) != 0;

		/// <summary>Gets the mesh source</summary>
		public MeshLayerDefinition MeshLayerDefinition { get; private set; }
		/// <summary>Gets the vertices of the mesh in pixels</summary>
		/// <remarks>Vertices are originally loaded in raw units, but are converted to px within <see cref="WireframeInstance"/></remarks>
		public List<Vertex> Vertices { get; } = new List<Vertex>();
	}

	/// <summary>Represents a local instance of a <see cref="WireframeDefinition"/> for a single craft/flightgroup.</summary>
	public class WireframeInstance
	{
		bool _rebuildRequired = true;
		int _curX = 0;
		int _curY = 0;
		int _curZ = 0;
		int _dstX = 0;
		int _dstY = 0;
		int _dstZ = 0;
		int _curZoom = 0;
		MapForm.Orientation _curOrientation;
		long _curVisibilityFlags = 0;
		double _scaleMult;

		/// <summary>Creates a new instance from the specified definition.</summary>
		/// <param name="def">The original wireframe source</param>
		/// <param name="craftType">The craft type index</param>
		/// <param name="fgIndex">The FlightGroup index within the mission</param>
		/// <remarks>The <paramref name="craftType"/> and <paramref name="fgIndex"/> parameters are used to identify this instance so that the underlying manager can change the model when necessary.</remarks>
		public WireframeInstance(WireframeDefinition def, int craftType, int fgIndex)
		{
			AssignedCraftType = craftType;
			AssignedFGIndex = fgIndex;
			ModelDef = def;
			if (def == null) return;

			foreach (MeshLayerDefinition layer in ModelDef.MeshLayerDefinitions) LayerInstances.Add(new MeshLayerInstance(layer));
		}

		/// <summary>Determine whether the core instance has changed, and flags for rebuild if needed.</summary>
		/// <param name="craftType">The craft type index to compare against</param>
		/// <param name="fgIndex">The FlightGroup index within the mission to compare against</param>
		public void CheckAssignment(int craftType, int fgIndex)
		{
			if (craftType != AssignedCraftType || AssignedFGIndex != fgIndex)
			{
				AssignedCraftType = craftType;
				AssignedFGIndex = fgIndex;
				_rebuildRequired = true;
			}
		}

		/// <summary>Updates the transformed vertices as it should appear on screen</summary>
		/// <param name="cur">The craft origin</param>
		/// <param name="dest">A point that the craft is facing</param>
		/// <param name="zoom">The map zoom level, in px/km</param>
		/// <param name="orientation">The viewing direction of the map</param>
		/// <param name="meshTypeVisibilityFlags">The flags determining which Mesh types to display</param>
		/// <remarks>If no change is detected, the wireframe remains as is. Resulting vertex positions are relative to the model origin.</remarks>
		public void UpdateParams(Platform.BaseFlightGroup.Waypoint cur, Platform.BaseFlightGroup.Waypoint dest, int zoom, MapForm.Orientation orientation, long meshTypeVisibilityFlags, int degRoll)
		{
			if (ModelDef == null) return;
			if (!_rebuildRequired && _curX == cur.RawX && _curY == cur.RawY && _curZ == cur.RawZ && _dstX == dest.RawX && _dstY == dest.RawY && _dstZ == dest.RawZ && _curZoom == zoom && _curOrientation == orientation && _curVisibilityFlags == meshTypeVisibilityFlags)
				return;
			_rebuildRequired = false;
			_curX = cur.RawX;
			_curY = cur.RawY;
			_curZ = cur.RawZ;
			_dstX = dest.RawX;
			_dstY = dest.RawY;
			_dstZ = dest.RawZ;
			_curOrientation = orientation;
			_curZoom = zoom;
			_curVisibilityFlags = meshTypeVisibilityFlags;

			// Zoom is in pixels per KM. Model units are at a scale of 40960 units per KM.
			_scaleMult = _curZoom / 40960.0;
			int diffX = _dstX - _curX;
			int diffY = _dstY - _curY;
			int diffZ = _dstZ - _curZ;

			double yaw = 0.0;
			double pitch = 0.0;
            double roll = -degRoll * (Math.PI / 180.0f);
            if (_curX == _dstX && _curY == _dstY && _curZ == _dstZ)
			{
				if (dest.Enabled)
				{
					yaw = -Math.PI / 4;  // 45 degree turn clockwise and pitch up.
					pitch = (_curOrientation != MapForm.Orientation.XY ? -Math.PI / 4 : Math.PI / 4);
				}
			}
			else if (dest.Enabled)
			{
				if (_curOrientation == MapForm.Orientation.YZ) yaw = Math.Atan2(diffX, diffY);
				else
				{
					yaw = Math.Atan2(-diffY, -diffX);
					yaw += (Math.PI / 2);
				}
				pitch = -Math.Atan2(diffZ, Math.Sqrt(diffX * diffX + diffY * diffY));
				if (yaw > Math.PI) yaw -= Math.PI * 2;
			}

			updatePoints(_scaleMult, new Matrix3(yaw, pitch, roll));
		}

		/// <summary>Updates the transformed vertices as it should appear on screen. This applies direct rotations without needing to calculate waypoint angles.</summary>
		/// <param name="zoom">The map zoom level, in px/km</param>
		/// <param name="meshTypeVisibilityFlags">The flags determining which Mesh types to display</param>
		/// <param name="degYaw">Yaw rotation, in degrees.</param>
		/// <param name="degPitch">Pitch rotation, in degrees.</param>
		/// <param name="degRoll">Roll rotation, in degrees.</param>
		/// <remarks>If no change is detected, the wireframe remains as is. Resulting vertex positions are relative to the model origin.</remarks>
		public void UpdateSimple(int zoom, long meshTypeVisibilityFlags, int degYaw, int degPitch, int degRoll)
		{
			if (ModelDef == null) return;
			if (!_rebuildRequired && _curX == degYaw && _curY == degPitch && _curZ == degRoll &&_curZoom == zoom && _curVisibilityFlags == meshTypeVisibilityFlags)
				return;
			_rebuildRequired = false;

			// Since we're not calculating between two points, position doesn't matter. Store the rotation instead.
			_curX = degYaw;
			_curY = degPitch;
			_curZ = degRoll;
			_curZoom = zoom;
			_curVisibilityFlags = meshTypeVisibilityFlags;

			_scaleMult = _curZoom / 40960.0;
			double yaw = -degYaw * (Math.PI / 180.0f);
			// Any craft pitch outside this range seems to be rejected by XWA.
			if (degPitch >= 90 || degPitch <= -92) degPitch = 0;
			double pitch = degPitch * (Math.PI / 180.0f);
			double roll = -degRoll * (Math.PI / 180.0f);
			updatePoints(_scaleMult, new Matrix3(yaw, pitch, roll));
		}

		/// <param name="scaleMult">Scale multiplier.</param>
		/// <param name="rotation">A matrix with the necessary rotation transform.</param>
		void updatePoints(double scaleMult, Matrix3 rotation)
		{
			rotation.Scale(scaleMult);
			foreach (MeshLayerInstance cinst in LayerInstances)
			{
				if (!cinst.MatchMeshFilter(_curVisibilityFlags)) continue;
				for (int i = 0; i < cinst.Vertices.Count; i++)
				{
					Vertex v = cinst.Vertices[i];
					v.X = cinst.MeshLayerDefinition.Vertices[i].X;
					v.Y = cinst.MeshLayerDefinition.Vertices[i].Y;
					v.Z = -cinst.MeshLayerDefinition.Vertices[i].Z;  // Inverted so they appear properly. Maybe this could be handled during the load?
					v.MultTranspose(rotation);
				}
			}
		}

		/// <summary>Gets the mesh instances that make up the wireframe</summary>
		public List<MeshLayerInstance> LayerInstances { get; } = new List<MeshLayerInstance>();
		/// <summary>Gets the wireframe source</summary>
		public WireframeDefinition ModelDef { get; private set; }

		/// <summary>Gets the craft type index associated to the instance</summary>
		public int AssignedCraftType { get; private set; }
		/// <summary>Gets the FlightGroup index within the mission associated to the instance</summary>
		public int AssignedFGIndex { get; private set; }
	}

		/// <summary>Represents the central manager exposed to MapForm for accessing and managing wireframe models.</summary>
	/// <remarks>Most of the heavy-lifting is managed internally, abstracting as much work as possible away from the MapForm.</remarks>
	public class WireframeManager
	{
		Settings.Platform _curPlatform = Settings.Platform.None;   // The current loaded platform.
		string _curMissionPath = "";                               // The full path+filename of the current mission file. Used when attempting to auto-detect the game directory for available model files.
		string _curInstallPath = "";                               // The current installation path. Used to detect if the user has changed the config, and reload if necessary.
		string _modelLoadDirectory = "";                           // Contains the resolved path that all model resources are loaded from.

		Dictionary<int, WireframeDefinition> _wireframeDefinitions = null;    // Indexed by craftType, to store the loaded wireframe definition for that craft.
		List<WireframeInstance> _wireframeInstances = null;                   // Indexed by FlightGroup, so that each item in the map has its own instance.
		List<CraftData> _craftData = null;                                   // Indexed by craftType. Contains informational data loaded from an external file, most importantly the species resource names to load.

		// Only used for DOS formats.
		Dictionary<string, string> _dosSpeciesMap = null;          // Maps a list of all available species (as scanned from the SPECIES*.LFD archives) to the full path+filename of the archive it can be loaded from. (Ex: DREAD -> *path*\SPECIES2.LFD)
		Resource.ResourceType _dosCraftFormat = Resource.ResourceType.Undefined;      // Required for X-wing, specifically one file (BWING.CRF). It exists as a standalone file, not archived in SPECIES.LFD. Since the format cannot be derived from the file extension (XW93 and XW94 have the same file name), the context must be determined from the assets within SPECIES.LFD.

		/// <summary>Initializes a blank manager</summary>
		public WireframeManager()
		{
			_wireframeDefinitions = new Dictionary<int, WireframeDefinition>();
			_wireframeInstances = new List<WireframeInstance>();
		}

		/// <summary>Creates a WireframeInstance, or retrieves an existing one.</summary>
		/// <param name="craftType">The craft type index</param>
		/// <param name="fgIndex">The FlightGroup index within the mission</param>
		/// <remarks>Automatically replaces the instance if the craftType or fgIndex has changed.</remarks>
		/// <returns>If <paramref name="fgIndex"/> is negative or the <see cref="WireframeDefinition"/> for the specified <paramref name="craftType"/> does not exist, returns <b>null</b>.<br/>
		/// If the instance for the specifed <paramref name="fgIndex"/> does not exist it is created and returned. If it does exist, updates the <paramref name="craftType"/> if necessary and returns it.</returns>
		public WireframeInstance GetOrCreateWireframeInstance(int craftType, int fgIndex)
		{
			if (fgIndex < 0) return null;

			WireframeDefinition def = getWireframeDefinition(craftType);
			if (def == null) return null;

			// Pad the list so there's no out-of-bounds problems.
			while (_wireframeInstances.Count <= fgIndex) { _wireframeInstances.Add(null); }

			if (_wireframeInstances[fgIndex] == null) _wireframeInstances[fgIndex] = createWireframeInstance(def, craftType, fgIndex);
			else _wireframeInstances[fgIndex] = update(_wireframeInstances[fgIndex], craftType, fgIndex);

			return _wireframeInstances[fgIndex];
		}

		/// <summary>Prepares the manager to use a specific platform.</summary>
		/// <remarks>Handles basic tasks required when changing platforms, resetting the model cache and determining a new directory to load models from.</remarks>
		public void SetPlatform(Settings.Platform platform)
		{
			// Retrieve the current installation path. If the platform remains the same, but the user has chosen a different folder, we'll be able to reload.
			string installPath = CraftDataManager.GetInstance().GetInstallPath();

			if (_curPlatform != platform || Settings.GetInstance().LastMission != _curMissionPath || _curInstallPath != installPath)
			{
				// Prepare new cache and reset the loading context.
				_wireframeDefinitions = new Dictionary<int, WireframeDefinition>();
				_wireframeInstances = new List<WireframeInstance>();
				_dosCraftFormat = Resource.ResourceType.Undefined;
				_dosSpeciesMap = new Dictionary<string, string>();

				if (platform != Settings.Platform.None)
				{
					_modelLoadDirectory = CraftDataManager.GetInstance().GetModelPath();

					// Detect if DOS models exist and retrieve the information necessary to load them.
					string path = Path.Combine(_modelLoadDirectory, "species.lfd");
					if (File.Exists(path))
					{
						parseSpeciesFile(path);
						if (platform == Settings.Platform.TIE)
						{
							parseSpeciesFile(Path.Combine(_modelLoadDirectory, "species2.lfd"));
							parseSpeciesFile(Path.Combine(_modelLoadDirectory, "species3.lfd"));
						}
					}
				}
			}
			_curMissionPath = Settings.GetInstance().LastMission;
			_curInstallPath = installPath;
			_craftData = CraftDataManager.GetInstance().GetCraftDataList();
			_curPlatform = platform;
		}

		/// <summary>Opens a DOS SPECIES*.LFD archive, generating a list of resources it contains.</summary>
		/// <remarks>Also detects the craft file format to establish a proper loading context.</remarks>
		void parseSpeciesFile(string filename)
		{
			if (!File.Exists(filename)) return;
			if (Resource.GetType(filename, 0) == Resource.ResourceType.Rmap)
			{
				var rmap = new Rmap(filename);
				for (int h = 0; h < rmap.NumberOfHeaders; h++)
				{
					if (!_dosSpeciesMap.ContainsKey(rmap.SubHeaders[h].Name.ToLower())) _dosSpeciesMap.Add(rmap.SubHeaders[h].Name.ToLower(), filename);
					if (_dosCraftFormat == Resource.ResourceType.Undefined) _dosCraftFormat = rmap.SubHeaders[h].Type;
				}
			}
		}

		/// <summary>Loads a model definition into the cache, or retrieves an already existing cache entry.</summary>
		/// <remarks>If not already loaded, searches through the possible resource names to find a matching OPT or DOS species entry. If found, the mesh is loaded and converted to a ready format that the wireframe system can use.</remarks>
		/// <returns>Returns a model definition. If the model failed to load, the definition will be empty, but valid. Returns <b>null</b> if out of range.</returns>
		WireframeDefinition getWireframeDefinition(int craftType)
		{
			// Check if already loaded.
			if (_wireframeDefinitions.ContainsKey(craftType)) return _wireframeDefinitions[craftType];
			if (_craftData == null || craftType < 0 || craftType >= _craftData.Count) return null;

			string resourceNames = _craftData[craftType].ResourceNames.ToLower();
			WireframeDefinition def;
			if (_dosCraftFormat == Resource.ResourceType.Undefined)
			{
				OptFile opt = new OptFile();
				string[] names = resourceNames.Split('|');
				foreach (string s in names)
				{
					string s2 = s;
					if (s2.IndexOf('*') >= 0) s2 = s2.Remove(s2.IndexOf('*'));
					if (_curPlatform == Settings.Platform.BoP && opt.LoadFromFile(Path.Combine(_modelLoadDirectory, s2 + ".op1"), false)) break;
					if (opt.LoadFromFile(Path.Combine(_modelLoadDirectory, s2 + ".opt"), _curPlatform == Settings.Platform.XWA)) break;
				}
				def = new WireframeDefinition(opt);
			}
			else
			{
				CraftFile craft = new CraftFile();
				float scale = 0.5F;  // Default scale for most ships.
				string[] resNames = resourceNames.Split('|');
				foreach (string s in resNames)
				{
					string s2 = s;
					if (s2.IndexOf('*') >= 0)
					{
						float.TryParse(s2.Substring(s2.IndexOf('*') + 1), out scale);
						s2 = s2.Remove(s2.IndexOf('*'));
					}
					if (_dosSpeciesMap.ContainsKey(s2)) { if (craft.LoadFromArchive(_dosSpeciesMap[s2], s2)) break; }
					else
					{
						// This is really only required for the B-wing in the DOS versions of XWING, which exists in a standalone file.
						if (craft.LoadCftFile(Path.Combine(_modelLoadDirectory, s2 + ".cft"), _dosCraftFormat)) break;
					}
				}
				def = new WireframeDefinition(craft);
				def.Scale(scale);
			}
			_wireframeDefinitions.Add(craftType, def);
			return def;
		}

		/// <summary>Creates a new WireframeInstance from a definition.</summary>
		WireframeInstance createWireframeInstance(WireframeDefinition def, int craftType, int fgIndex) => def == null ? null : new WireframeInstance(def, craftType, fgIndex);

		/// <summary>Checks if an existing WireframeInstance needs to change its model type.</summary>
		/// <returns>Returns the current instance if nothing changed, otherwise returns a new instance.</returns>
		WireframeInstance update(WireframeInstance currentInstance, int craftType, int fgIndex)
		{
			if (currentInstance == null) return null;

			WireframeInstance ret = currentInstance;
			if (craftType != currentInstance.AssignedCraftType)
			{
				WireframeDefinition def = getWireframeDefinition(craftType);
				ret = createWireframeInstance(def, craftType, fgIndex);
			}
			else currentInstance.CheckAssignment(craftType, fgIndex);
			return ret;
		}
	}
}