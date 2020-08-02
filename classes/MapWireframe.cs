/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2020 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.6.6+
 */

/* CHANGELOG
* v1.7, XXXXXX
* [NEW] created [JB]
*/

using System;
using System.IO;
using System.Collections.Generic;

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

namespace Idmr.Yogeme
{
	public class Vector3
	{
		public float x;
		public float y;
		public float z;
		public Vector3()
		{
			x = 0.0f;
			y = 0.0f;
			z = 0.0f;
		}
		public Vector3(int _x, int _y, int _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}
		public Vector3(float _x, float _y, float _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}
		public Vector3(Vector3 other)
		{
			x = other.x;
			y = other.y;
			z = other.z;
		}
		public void MultTranspose(Matrix3 mat)
		{
			float vx = x;
			float vy = y;
			float vz = z;
			x = (float)(mat.v11 * vx + mat.v21 * vy + mat.v31 * vz);
			y = (float)(mat.v12 * vx + mat.v22 * vy + mat.v32 * vz);
			z = (float)(mat.v13 * vx + mat.v23 * vy + mat.v33 * vz);
		}
	}
	public class Matrix3
	{
		public double v11;
		public double v12;
		public double v13;
		public double v21;
		public double v22;
		public double v23;
		public double v31;
		public double v32;
		public double v33;

		/// <summary>Initializes a matrix ready for the needed rotational transform.</summary>
		/// <remarks>This is the matrix multiplication for the equations Roll * Yaw, in that order. When applying "Roll" to the vertices,<br/>
		/// the visible effect is pitch relative to the body. Yaw works as expected. Perhaps this is because the Z axis in-game is<br/>
		/// elevation, not depth?</remarks>
		public Matrix3(double yaw, double pitch)
		{
			v11 = Math.Cos(yaw);
			v12 = -Math.Sin(yaw);
			v13 = 0;
			v21 = (Math.Cos(pitch) * Math.Sin(yaw));
			v22 = (Math.Cos(pitch) * Math.Cos(yaw));
			v23 = -Math.Sin(pitch);
			v31 = (Math.Sin(pitch) * Math.Sin(yaw));
			v32 = (Math.Sin(pitch) * Math.Cos(yaw));
			v33 = Math.Cos(pitch);
		}
	}

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
		FaceGrouping = 21,
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

	/// <summary>Exposes some useful defaults and functions to assist program configuration for <see cref="MeshType"/> visibility.</summary>
	public static class MeshTypeHelper
	{
		// These predefined arrays help initialize the user's configuration as well as offering quick toggles to include or exclude a selection of visible mesh types.
		public static MeshType[] DefaultMeshes = new MeshType[] { MeshType.Default, MeshType.MainHull, MeshType.Wing, MeshType.Fuselage, MeshType.Bridge, MeshType.DockingPlatform, MeshType.LandingPlatform, MeshType.Hangar, MeshType.CargoPod, MeshType.MiscHull, MeshType.Engine, MeshType.RotaryWing, MeshType.Launcher };
		public static MeshType[] HullMeshes = new MeshType[] { MeshType.Default, MeshType.MainHull, MeshType.Wing, MeshType.Fuselage, MeshType.Engine, MeshType.Bridge, MeshType.Launcher, MeshType.MiscHull, MeshType.RotaryWing };
		public static MeshType[] MiscMeshes = new MeshType[] { MeshType.ShieldGenerator, MeshType.EnergyGenerator, MeshType.CommunicationSystem, MeshType.BeamSystem, MeshType.CommandSystem, MeshType.CargoPod, MeshType.Antenna, MeshType.RotaryCommunicationSystem, MeshType.RotaryBeamSystem, MeshType.RotaryCommandSystem, MeshType.Hatch, MeshType.Custom, MeshType.PowerRegenerator, MeshType.Reactor };
		public static MeshType[] WeaponMeshes = new MeshType[] { MeshType.GunTurret, MeshType.SmallGun, MeshType.RotaryGunTurret, MeshType.RotaryLauncher, MeshType.WeaponSystem1, MeshType.WeaponSystem2 };
		public static MeshType[] HangarMeshes = new MeshType[] { MeshType.DockingPlatform, MeshType.LandingPlatform, MeshType.Hangar };

		/// <summary>Returns the default MeshTypes combined into a single value.</summary>
		public static long GetDefaultFlags()
		{
			return GetFlags(DefaultMeshes);
		}

		/// <summary>Combines an array of enum-based MeshTypes into a single value.</summary>
		public static long GetFlags(MeshType[] list)
		{
			long retval = 0;
			foreach (MeshType value in list)
				retval |= (long)(1 << (int)value);
			return retval;
		}

		/// <summary>Combines an int array into a single value.</summary>
		public static long GetFlags(int[] list)
		{
			long retval = 0;
			foreach (int value in list)
				retval |= (long)(1 << value);
			return retval;
		}
	}

	/// <summary>Container to store the vertices of a single polygon face, which may have 3 or 4 vertices.</summary>
	public struct OptFace
	{
		public int[] vertexIndex;
		public OptFace(int v1, int v2, int v3, int v4)
		{
			vertexIndex = new int[4];
			vertexIndex[0] = v1;
			vertexIndex[1] = v2;
			vertexIndex[2] = v3;
			vertexIndex[3] = v4;
		}
	}

	/// <summary>Container to store all mesh faces of a single LOD.</summary>
	/// <remarks>Discarded prototyping code attempted to select a lower detail mesh to improve drawing performance, but it wasn't very helpful for normal models.</remarks>
	public class OptLod
	{
		public float distance;
		public List<OptFace> faces = new List<OptFace>();
		public OptLod()
		{
			distance = float.MaxValue;
		}
		public OptLod(float dist)
		{
			distance = dist;
		}
	}

	/// <summary>Holds all loaded information for a single component, which is a top-level node in the OPT tree.</summary>
	public class OptComponent
	{
		public OptNodeType nodeType = 0;
		public MeshType meshType = MeshType.Default;
		public int loadingLodIndex = 0;
		public List<Vector3> vertices = new List<Vector3>();
		public List<OptLod> lods = new List<OptLod>();
	}

	/// <summary>The OPT format was introduced with XvT/BoP, continued in XWA, and retrofitted into XWING and TIE for the Windows versions.</summary>
	/// <remarks>The format is a tree of nodes, utilizing C-style pointers to navigate each node in the tree and access their data elements.</remarks>
	public class OptFile
	{
		private int basePosition;  // The file contents begin with some meta data that isn't part of the actual model data. This will be the stream position where the real data begins.
		private int globalOffset;  // The first piece of real data in the file is a pointer to itself. Since the entire file would be contiguous in memory, subtracting from any other pointer address gives us a relative offset into the file.
		public List<OptComponent> components = new List<OptComponent>();

		/// <summary>Initializes and attempts to load the contents from file.</summary>
		/// <returns>Returns true if the file is loaded.</returns>
		public bool LoadFromFile(string filename)
		{
			try
			{
				if (!File.Exists(filename))
					return false;
				using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					using (BinaryReader br = new BinaryReader(fs))
					{
						fs.Seek(0, SeekOrigin.End);
						long fileSize = fs.Position;
						fs.Seek(0, SeekOrigin.Begin);

						int version = br.ReadInt32();
						if (version > 0)
						{
							fileSize = version;
							version = 0;
						}
						else
						{
							version = -version;
							fileSize = br.ReadInt32();
						}

						ParseTopNodes(fs, br);
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>Parses all top-level nodes.</summary>
		/// <remarks>Typically each top-level node is a single component of a particular MeshType. Its MeshType and mesh information will be defined somewhere in its child node tree.</remarks>
		private void ParseTopNodes(FileStream fs, BinaryReader br)
		{
			basePosition = (int)fs.Position;
			globalOffset = br.ReadInt32();
			fs.Position += 2;  // Skip Int16 heap index.
			int nodeCount = br.ReadInt32();
			int nodeTableOffset = br.ReadInt32();

			if (nodeTableOffset != 0)
				nodeTableOffset -= globalOffset;

			components = new List<OptComponent>();
			for (int i = 0; i < nodeCount; i++)
			{
				fs.Position = basePosition + nodeTableOffset + (i * 4);
				int nodeOffset = br.ReadInt32();
				if (nodeOffset != 0)
				{
					nodeOffset -= globalOffset;
					OptComponent comp = new OptComponent();
					fs.Position = basePosition + nodeOffset;
					ParseChildNodes(fs, br, comp);
					components.Add(comp);
				}
			}
		}

		/// <summary>Recursively parses all child nodes of a top-level node.</summary>
		/// <remarks>Loads any relevant data into the specified component object.</remarks>
		private void ParseChildNodes(FileStream fs, BinaryReader br, OptComponent node)
		{
			int nameOffset = br.ReadInt32();
			OptNodeType nodeType = (OptNodeType)br.ReadInt32();
			int childNodeCount = br.ReadInt32();
			int childNodeOffset = br.ReadInt32();
			int dataCount = br.ReadInt32();
			int dataOffset = br.ReadInt32();
			if (childNodeOffset != 0)
				childNodeOffset -= globalOffset;

			node.nodeType = nodeType;
			if (dataOffset != 0)
				dataOffset -= globalOffset;

			switch (nodeType)
			{
				case OptNodeType.MeshVertices:
					if (dataOffset == 0)
						break;
					fs.Position = basePosition + dataOffset;
					for (int i = 0; i < dataCount; i++)
					{
						float x = br.ReadSingle();
						float y = br.ReadSingle();
						float z = br.ReadSingle();
						node.vertices.Add(new Vector3(x, y, z));
					}
					break;
				case OptNodeType.FaceData:
					if (dataOffset == 0)
						break;
					if (node.loadingLodIndex >= node.lods.Count)
						break;
					fs.Position = basePosition + dataOffset + 4;  // Into the data, skipping Int32 edgeCount.
					for (int i = 0; i < dataCount; i++)
					{
						int v1 = br.ReadInt32();
						int v2 = br.ReadInt32();
						int v3 = br.ReadInt32();
						int v4 = br.ReadInt32();
						node.lods[node.loadingLodIndex].faces.Add(new OptFace(v1, v2, v3, v4));
						fs.Position += 48;  // Advance to next face.
					}
					break;
				case OptNodeType.MeshDescriptor:
					if (dataOffset == 0)
						break;
					fs.Position = basePosition + dataOffset;
					node.meshType = (MeshType)br.ReadInt32();
					break;
				case OptNodeType.FaceGrouping:
					if (dataOffset == 0)
						break;
					fs.Position = basePosition + dataOffset;
					for (int i = 0; i < dataCount; i++)
					{
						float distance = br.ReadSingle();
						node.lods.Add(new OptLod(distance));
					}
					while (node.lods.Count < childNodeCount)
					{
						node.lods.Add(new OptLod());
					}
					for (int i = 0; i < childNodeCount; i++)
					{
						node.loadingLodIndex = i;
						fs.Position = basePosition + childNodeOffset + (i * 4);
						int nodeOffset = br.ReadInt32();
						if (nodeOffset != 0)
						{
							nodeOffset -= globalOffset;
							fs.Position = basePosition + nodeOffset;
							ParseChildNodes(fs, br, node);
						}
					}
					break;
			}
			// We already had a special case for FaceGrouping.
			if (nodeType != OptNodeType.FaceGrouping)
			{
				for (int i = 0; i < childNodeCount; i++)
				{
					fs.Position = basePosition + childNodeOffset + (i * 4);
					int nodeOffset = br.ReadInt32();
					if (nodeOffset != 0)
					{
						nodeOffset -= globalOffset;
						fs.Position = basePosition + nodeOffset;
						ParseChildNodes(fs, br, node);
					}
				}
			}
		}
	}

	/// <summary>This Vector3 is needed for the DOS craft formats.</summary>
	/// <remarks>Presented as an array to make it easier to access its members.</remarks>
	public class Vector3_Int16
	{
		public short[] data = new short[3];
		public Vector3_Int16()
		{
			data[0] = 0;
			data[1] = 0;
			data[2] = 0;
		}
	}

	/// <summary>Two vertex indices that define a line.</summary>
	public class Line
	{
		public int v1;
		public int v2;
		public Line(int _v1, int _v2)
		{
			v1 = _v1;
			v2 = _v2;
		}
	}

	/// <summary>Container to store all mesh faces of a single LOD.</summary>
	public class CraftLod
	{
		public int distance;
		public short fileOffset;
		public List<Vector3_Int16> vertices = new List<Vector3_Int16>();
		public List<Line> lines = new List<Line>();
		public CraftLod(int _distance, short _fileOffset)
		{
			distance = _distance;
			fileOffset = _fileOffset;
		}
	}

	/// <summary>Holds all loaded information for a single component.</summary>
	public class CraftComponent
	{
		public MeshType meshType = MeshType.Default;
		public List<CraftLod> lods = new List<CraftLod>();
	}

	/// <summary>This facilitates loading of all DOS craft formats (CRFT, CPLX, and SHIP).</summary>
	/// <remarks>The format is similar to OPT in the sense that it contains a list of components, along with various pieces of data. These resources are typically packed into uncompressed LFD archives, which are automatically handled by the loading functions.</remarks>
	public class CraftFile
	{
		public List<CraftComponent> components = new List<CraftComponent>();
		private LfdCraftFormat lfdCraftFormat = LfdCraftFormat.None;

		/// <summary>Initializes the object and attempts to load relevant data from a standalone file.</summary>
		/// <remarks>Requires the craft file format to already be known, as the context cannot be determined from the file contents.</remarks>
		public bool LoadFromFile(string speciesName, LfdCraftFormat craftFormat)
		{
			if (!File.Exists(speciesName) || craftFormat == LfdCraftFormat.None)
				return false;
			lfdCraftFormat = craftFormat;
			try
			{
				using (FileStream fs = new FileStream(speciesName, FileMode.Open, FileAccess.Read))
				{
					using (BinaryReader br = new BinaryReader(fs))
					{
						short fileSize = br.ReadInt16();
						if (lfdCraftFormat == LfdCraftFormat.CRFT || lfdCraftFormat == LfdCraftFormat.CPLX)
							ParseXwing(fs, br);
						else if (lfdCraftFormat == LfdCraftFormat.SHIP)
							ParseTie(fs, br);
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>Initializes the object and attempts to load relevant data from within an LFD archive.</summary>
		public bool LoadFromArchive(string archiveName, string speciesName)
		{
			lfdCraftFormat = LfdCraftFormat.None;
			if (!File.Exists(archiveName))
				return false;
			try
			{
				using (FileStream fs = new FileStream(archiveName, FileMode.Open, FileAccess.Read))
				{
					using (BinaryReader br = new BinaryReader(fs, System.Text.Encoding.GetEncoding(437)))
					{
						List<LfdResourceInfo> resourceList = LoadLfdResourceTableFromStream(fs, br);
						int offset = 0;

						LfdResourceInfo entry = GetResourceInfo(resourceList, speciesName, out offset);
						if (entry != null)
						{
							lfdCraftFormat = entry.GetCraftFormat();
							fs.Position = offset;

							LfdResourceInfo current = new LfdResourceInfo();
							current.ReadFromStream(br);
							if (current.length != entry.length)
								return false;

							short fileSize = br.ReadInt16();
							if (fileSize != current.length - 2)
								return false;

							if (lfdCraftFormat == LfdCraftFormat.CRFT || lfdCraftFormat == LfdCraftFormat.CPLX)
								ParseXwing(fs, br);
							else if (lfdCraftFormat == LfdCraftFormat.SHIP)
								ParseTie(fs, br);
						}
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>Returns a list of resources contained in an LFD archive.</summary>
		private List<LfdResourceInfo> LoadLfdResourceTableFromStream(FileStream fs, BinaryReader br)
		{
			List<LfdResourceInfo> result = new List<LfdResourceInfo>();
			LfdResourceInfo header = new LfdResourceInfo();
			header.ReadFromStream(br);
			if (header.type != "RMAP")
				return result;

			result.Add(header);
			for (int i = 0; i < header.length / 16; i++)
			{
				LfdResourceInfo resource = new LfdResourceInfo();
				resource.ReadFromStream(br);
				result.Add(resource);
			}
			return result;
		}

		/// <summary>Retrieves a resource entry from the specified list and calculates its file position.</summary>
		/// <returns>Returns the matching resource entry, or null if not found. If found, its file offset is placed in the output parameter.</returns>
		private LfdResourceInfo GetResourceInfo(List<LfdResourceInfo> resourceList, string resourceName, out int fileOffset)
		{
			int totalSize = 0;
			for (int i = 0; i < resourceList.Count; i++)
			{
				if (resourceList[i].GetCraftFormat() != LfdCraftFormat.None && string.Compare(resourceList[i].name, resourceName, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					fileOffset = totalSize;
					return resourceList[i];
				}
				totalSize += 16 + resourceList[i].length;
			}
			fileOffset = 0;
			return null;
		}

		/// <summary>Parses the header and top-level contents of the CRFT and CPLX formats used in XWING.</summary>
		private void ParseXwing(FileStream fs, BinaryReader br)
		{
			byte componentCount = br.ReadByte();
			byte shadingRecordCount = br.ReadByte();

			fs.Position += 16 * shadingRecordCount;

			for (int i = 0; i < componentCount; i++)
			{
				long recStart = fs.Position;
				short nodeOffset = br.ReadInt16();
				if (nodeOffset != 0)
				{
					fs.Position = recStart + nodeOffset;
					CraftComponent comp = new CraftComponent();
					ParseNode(fs, br, comp);
					components.Add(comp);
					fs.Position = recStart + 2;
				}
			}
		}

		/// <summary>Parses the header and top-level contents of the SHIP format used in TIE.</summary>
		private void ParseTie(FileStream fs, BinaryReader br)
		{
			fs.Position += 30;   // Skip unknown.

			byte componentCount = br.ReadByte();
			byte shadingSetCount = br.ReadByte();
			// Skip int16 unknown, then 6 bytes per shading set.
			fs.Position += 2 + (6 * shadingSetCount);

			for (int i = 0; i < componentCount; i++)
			{
				long recStart = fs.Position;
				short meshType = br.ReadInt16();
				fs.Position += 42;
				short lodOffset = br.ReadInt16();

				fs.Position = recStart + lodOffset;
				CraftComponent comp = new CraftComponent();
				comp.meshType = (MeshType)meshType;
				ParseNode(fs, br, comp);
				components.Add(comp);
				fs.Position = recStart + 64;
			}
		}

		/// <summary>Parses the node mesh information for all DOS formats.</summary>
		private void ParseNode(FileStream fs, BinaryReader br, CraftComponent node)
		{
			long nodeBasePosition = fs.Position;

			int distance;
			short offset;
			do
			{
				distance = br.ReadInt32();
				offset = br.ReadInt16();
				node.lods.Add(new CraftLod(distance, offset));
			} while (distance != 0x7FFFFFFF);

			for (int i = 0; i < node.lods.Count; i++)
			{
				fs.Position = nodeBasePosition + (i * 6) + node.lods[i].fileOffset;

				short header = br.ReadInt16();
				byte vertexCount = br.ReadByte();
				byte unknown = br.ReadByte();
				byte shapeCount = br.ReadByte();
				fs.Position += shapeCount; // Skip over the shape colors.
				fs.Position += 12;         // Skip boundMin and boundMax (each are 6 bytes, Vector3_16bit).

				for (int j = 0; j < vertexCount; j++)
				{
					short test;
					Vector3_Int16 v = new Vector3_Int16();
					for (int k = 0; k < 3; k++)
					{
						v.data[k] = br.ReadInt16();
						test = (short)((v.data[k] & 0xFF00) >> 8);
						if (test == 0x7F)
						{
							test = (short)((v.data[k] & 0xFF) >> 1);
							if (j - test >= 0 && j - test < node.lods[i].vertices.Count)
							{
								v.data[k] = node.lods[i].vertices[j - test].data[k];
							}
						}
					}
					node.lods[i].vertices.Add(v);
				}

				if (lfdCraftFormat == LfdCraftFormat.CPLX || lfdCraftFormat == LfdCraftFormat.SHIP)
					fs.Position += 6 * vertexCount;  // Skip vertex normals.

				long shapeBasePosition = fs.Position;
				short[] shapeOffsets = new short[shapeCount];
				// Read shape headers.
				for (int j = 0; j < shapeCount; j++)
				{
					fs.Position += 6;  // Skip face normal.
					shapeOffsets[j] = br.ReadInt16();
				}

				for (int j = 0; j < shapeCount; j++)
				{
					fs.Position = shapeBasePosition + (j * 8) + shapeOffsets[j];
					byte type = br.ReadByte();
					byte vertices = (byte)(type & 0x0F);
					if (vertices == 2)
					{
						byte[] data = br.ReadBytes(7);
						node.lods[i].lines.Add(new Line(data[2], data[3]));
					}
					else
					{
						int length = 3 + (vertices * 2);
						byte[] data = br.ReadBytes(length);
						for (int k = 0; k < vertices; k++)
						{
							int v1 = data[k * 2];
							int v2 = data[(k + 1) * 2];
							node.lods[i].lines.Add(new Line(v1, v2));
						}
					}
				}
			}
		}
	}

	/// <summary>Stores a compiled list of vertices and lines derived from the mesh and its faces.</summary>
	/// <remarks>Multiple components of the same MeshType will added into the same layer.</remarks>
	public class MeshLayerDefinition
	{
		public MeshType meshType;
		public List<Vector3> vertices;
		public List<Line> lines;
		public MeshLayerDefinition(MeshType createMeshType)
		{
			meshType = createMeshType;
			vertices = new List<Vector3>();
			lines = new List<Line>();
		}
	}

	/// <summary>A finalized wireframe definition that is ready for use in the map.</summary>
	/// <remarks>It can be generated from an OptFile or CraftFile.</remarks>
	public class WireframeDefinition
	{
		/// <summary>Span of the widest dimension, derived from bounding box, expressed in raw units (40960 units = 1 km)</summary>
		public int longestSpanRaw = 0;
		/// <summary>Span of the widest dimension, derived from bounding box, expressed in meters.</summary>
		public int longestSpanMeters = 0;
		public List<MeshLayerDefinition> meshLayerDefinitions = new List<MeshLayerDefinition>();

		/// <summary>Creates a definition from a loaded OPT.</summary>
		/// <remarks>Performs some basic optimization to prevent shared edges, so that lines don't have to be drawn twice.</remarks>
		public WireframeDefinition(OptFile opt)
		{
			MeshLayerDefinition layer = GetOrCreateMeshLayerDefinition(MeshType.MainHull);  // Create a default entry so that it's first in the list, for drawing purposes.

			foreach (OptComponent comp in opt.components)
			{
				if (comp.lods.Count == 0)
					continue;

				int[] vertUsed = new int[comp.vertices.Count];
				HashSet<int> lineUsed = new HashSet<int>();

				layer = GetOrCreateMeshLayerDefinition(comp.meshType);
				foreach (OptFace face in comp.lods[0].faces)
				{
					for (int i = 0; i < 4; i++)
					{
						int vi = face.vertexIndex[i];
						// If the face is a triangle (rather than a quad), the last index will be -1.
						if (vi == -1)
							continue;
						if (vertUsed[vi] == 0)
						{
							layer.vertices.Add(comp.vertices[vi]);
							vertUsed[vi] = layer.vertices.Count;  // One-based value.
						}
					}
					for (int i = 0; i < 3; i++)
					{
						int v1 = face.vertexIndex[i];
						int v2 = face.vertexIndex[i + 1];
						if (v1 == -1)
							continue;
						// For a triangle, the last vertex is missing. Link back to the first vertex in the face.
						if (v2 == -1)
							v2 = face.vertexIndex[0];

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
							layer.lines.Add(new Line(v1, v2));
							lineUsed.Add(key);
						}
					}
					// If the face is quadrilateral, we haven't linked the first and last vertices.
					// Perform the same thing as above.
					if (face.vertexIndex[0] >= 0 && face.vertexIndex[3] >= 0)
					{
						int v1 = face.vertexIndex[0];
						int v2 = face.vertexIndex[3];
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
							layer.lines.Add(new Line(v1, v2));
							lineUsed.Add(key);
						}
					}
				}
			}
			CalculateSize();
		}

		/// <summary>Creates a definition from a loaded DOS craft format (CRFT, CPLX, SHIP).</summary>
		/// <remarks>Performs some basic optimization to prevent shared edges, so that lines don't have to be drawn twice.</remarks>
		public WireframeDefinition(CraftFile craft)
		{
			// This function is conceptually similar to creating from OPT, except we already have our lines and don't need to examine the faces.
			MeshLayerDefinition layer = GetOrCreateMeshLayerDefinition(MeshType.MainHull);
			for (int i = 0; i < craft.components.Count; i++)
			{
				CraftComponent comp = craft.components[i];
				if (comp.lods.Count == 0)
					continue;
				CraftLod lod = comp.lods[0];

				int[] vertUsed = new int[lod.vertices.Count];
				HashSet<int> lineUsed = new HashSet<int>();

				layer = GetOrCreateMeshLayerDefinition(comp.meshType);
				for (int j = 0; j < lod.lines.Count; j++)
				{
					int v1 = lod.lines[j].v1;
					int v2 = lod.lines[j].v2;

					if (vertUsed[v1] == 0)
					{
						int x = lod.vertices[v1].data[0];
						int y = lod.vertices[v1].data[1];
						int z = lod.vertices[v1].data[2];
						layer.vertices.Add(new Vector3(x, y, z));
						vertUsed[v1] = layer.vertices.Count; // One-based.
					}
					if (vertUsed[v2] == 0)
					{
						int x = lod.vertices[v2].data[0];
						int y = lod.vertices[v2].data[1];
						int z = lod.vertices[v2].data[2];
						layer.vertices.Add(new Vector3(x, y, z));
						vertUsed[v2] = layer.vertices.Count;
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
						layer.lines.Add(new Line(v1, v2));
						lineUsed.Add(key);
					}
				}
			}
			CalculateSize();
		}

		/// <summary>Applies a scale to all vertices.</summary>
		/// <remarks>Needed for DOS models. Most craft must be scaled by 0.5 to get their proper size. The largest ships need to be scaled by 2.0</remarks>
		public void Scale(float scale)
		{
			if (scale <= 0)
				return;
			longestSpanRaw = (int)((float)longestSpanRaw * scale);
			longestSpanMeters = (int)((float)longestSpanMeters * scale);
			foreach (MeshLayerDefinition layer in meshLayerDefinitions)
			{
				foreach (Vector3 vert in layer.vertices)
				{
					vert.x *= scale;
					vert.y *= scale;
					vert.z *= scale;
				}
			}
		}

		/// <summary>Scans all vertices in the entire wireframe to determine its largest axis span based on its bounding box.</summary>
		private void CalculateSize()
		{
			int vcount = 0;
			foreach (MeshLayerDefinition layer in meshLayerDefinitions)
			{
				vcount += layer.vertices.Count;
			}
			if (vcount == 0) // The model wasn't loaded.
			{
				longestSpanRaw = 0;
				longestSpanMeters = 0;
				return;
			}
			float minX = float.MaxValue;
			float maxX = float.MinValue;
			float minY = float.MaxValue;
			float maxY = float.MinValue;
			float minZ = float.MaxValue;
			float maxZ = float.MinValue;
			foreach (MeshLayerDefinition layer in meshLayerDefinitions)
			{
				foreach (Vector3 v in layer.vertices)
				{
					if (v.x < minX) minX = v.x;
					if (v.x > maxX) maxX = v.x;
					if (v.y < minY) minY = v.y;
					if (v.y > maxY) maxY = v.y;
					if (v.z < minZ) minZ = v.z;
					if (v.z > maxZ) maxZ = v.z;
				}
			}
			int spanX = (int)(maxX - minX);
			int spanY = (int)(maxY - minY);
			int spanZ = (int)(maxZ - minZ);
			longestSpanRaw = spanX;
			if (spanY > longestSpanRaw) longestSpanRaw = spanY;
			if (spanZ > longestSpanRaw) longestSpanRaw = spanZ;
			longestSpanMeters = (int)((double)longestSpanRaw / 40.96);
		}

		/// <summary>Retrieves a layer for a particular MeshType. Creates an empty layer if it doesn't exist.</summary>
		private MeshLayerDefinition GetOrCreateMeshLayerDefinition(MeshType meshType)
		{
			foreach (MeshLayerDefinition layer in meshLayerDefinitions)
			{
				if (layer.meshType == meshType)
					return layer;
			}
			MeshLayerDefinition entry = new MeshLayerDefinition(meshType);
			meshLayerDefinitions.Add(entry);
			return entry;
		}
	}

	/// <summary>Built from a layer definition, this stores a cloned copy of the vertices that can be transformed without altering the definition.</summary>
	public class MeshLayerInstance
	{
		public MeshLayerDefinition meshLayerDefinition = null;
		public List<Vector3> vertices = null;
		public MeshLayerInstance(MeshLayerDefinition def)
		{
			meshLayerDefinition = def;
			vertices = new List<Vector3>();
			if (def != null)
			{
				vertices.Capacity = def.vertices.Count;
				for (int i = 0; i < def.vertices.Count; i++)
					vertices.Add(new Vector3(def.vertices[i]));
			}
		}
		public bool MatchMeshFilter(long meshVisibilityFilter)
		{
			return (meshLayerDefinition != null && (meshVisibilityFilter & (1 << (int)meshLayerDefinition.meshType)) != 0);
		}
	}

	/// <summary>Stores a local instance of a wireframe for a single craft/flightgroup.</summary>
	public class WireframeInstance
	{
		public List<MeshLayerInstance> layerInstances = new List<MeshLayerInstance>();
		public WireframeDefinition modelDef = null;

		// These two variables are used for cache purposes to determine if the model needs to be reloaded.
		public int assignedCraftType;
		public int assignedFgIndex;

		private bool rebuildRequired = true;
		private int curX = 0;
		private int curY = 0;
		private int curZ = 0;
		private int dstX = 0;
		private int dstY = 0;
		private int dstZ = 0;
		private int curZoom = 0;
		private MapForm.Orientation curOrientation;
		private long curVisibilityFlags = 0;
		private double scaleMult;

		/// <summary>Creates a new instance from the specified definition.</summary>
		/// <remarks>The craftType and fgIndex parameters are used to identify this instance so that the underlying manager can change the model when necessary.</remarks>
		public WireframeInstance(WireframeDefinition def, int craftType, int fgIndex)
		{
			assignedCraftType = craftType;
			assignedFgIndex = fgIndex;
			modelDef = def;
			if (def == null)
				return;

			layerInstances = new List<MeshLayerInstance>();
			foreach (MeshLayerDefinition layer in modelDef.meshLayerDefinitions)
			{
				layerInstances.Add(new MeshLayerInstance(layer));
			}
			// Trigger a refresh the next time it's updated.
			rebuildRequired = true;
		}

		/// <summary>Determine whether the core instance has changed, and rebuild if needed.</summary>
		public void CheckAssignment(int craftType, int fgIndex)
		{
			if (craftType != assignedCraftType || assignedFgIndex != fgIndex)
			{
				assignedCraftType = craftType;
				assignedFgIndex = fgIndex;
				rebuildRequired = true;
			}
		}

		/// <summary>Updates the transformed vertices as it should appear on screen, according to several parameters.</summary>
		/// <remarks>If no change is detected, the wireframe remains as is. Resulting vertex positions are relative to the model origin.</remarks>
		public void UpdateParams(Platform.BaseFlightGroup.BaseWaypoint cur, Platform.BaseFlightGroup.BaseWaypoint dest, int zoom, MapForm.Orientation orientation, long meshTypeVisibilityFlags)
		{
			if (modelDef == null)
				return;
			if (!rebuildRequired && curX == cur.RawX && curY == cur.RawY && curZ == cur.RawZ && dstX == dest.RawX && dstY == dest.RawY && dstZ == dest.RawZ && curZoom == zoom && curOrientation == orientation && curVisibilityFlags == meshTypeVisibilityFlags)
				return;
			rebuildRequired = false;
			curX = cur.RawX;
			curY = cur.RawY;
			curZ = cur.RawZ;
			dstX = dest.RawX;
			dstY = dest.RawY;
			dstZ = dest.RawZ;
			curOrientation = orientation;
			curZoom = zoom;
			curVisibilityFlags = meshTypeVisibilityFlags;

			// Zoom is in pixels per KM. Model units are at a scale of 40960 units per KM.
			scaleMult = (double)curZoom / 40960.0;
			int diffX = dstX - curX;
			int diffY = dstY - curY;
			int diffZ = dstZ - curZ;

			double yaw = 0.0;
			double pitch = 0.0;
			if (curX == dstX && curY == dstY && curZ == dstZ)
			{
				if (dest.Enabled)
				{
					yaw = -Math.PI / 4;  // 45 degree turn clockwise and pitch up.
					pitch = curOrientation != MapForm.Orientation.XY ? -Math.PI / 4 : Math.PI / 4;
				}
			}
			else if (dest.Enabled)
			{
				if (curOrientation == MapForm.Orientation.XY)
				{
					yaw = Math.Atan2(curY - dstY, curX - dstX);
					yaw += (Math.PI / 2);
					pitch = -Math.Atan2(diffZ, Math.Sqrt(diffX * diffX + diffY * diffY));
				}
				else if (curOrientation == MapForm.Orientation.XZ)
				{
					yaw = Math.Atan2(dstZ - curZ, curX - dstX);
					yaw += (Math.PI / 2);
					pitch = -Math.Atan2(diffZ, Math.Sqrt(diffX * diffX + diffY * diffY));
				}
				else if (curOrientation == MapForm.Orientation.YZ)
				{
					yaw = -Math.Atan2(dstX - curX, curY - dstY);
					pitch = -Math.Atan2(diffZ, Math.Sqrt(diffX * diffX + diffY * diffY));
				}
				if (yaw > Math.PI)
					yaw -= Math.PI * 2;
			}

			UpdatePoints(scaleMult, yaw, pitch);
		}

		/// <summary>Recalculates the positions of all vertices according to zoom and rotation.</summary>
		private void UpdatePoints(double scaleMult, double yaw, double pitch)
		{
			Matrix3 mat = new Matrix3(yaw, pitch);
			foreach (MeshLayerInstance cinst in layerInstances)
			{
				if (!cinst.MatchMeshFilter(curVisibilityFlags))
					continue;
				for (int i = 0; i < cinst.vertices.Count; i++)
				{
					Vector3 v = cinst.vertices[i];
					v.x = (float)(cinst.meshLayerDefinition.vertices[i].x * scaleMult);
					v.y = (float)(cinst.meshLayerDefinition.vertices[i].y * scaleMult);
					v.z = (float)(-cinst.meshLayerDefinition.vertices[i].z * scaleMult);  // Inverted so they appear properly. Maybe this could be handled during the load?
					v.MultTranspose(mat);
				}
			}
		}
	}

	/// <summary>Provides context to anything that needs to load a DOS model.</summary>
	public enum LfdCraftFormat
	{
		None = 0,
		CRFT = 1,
		CPLX = 2,
		SHIP = 3,
	}

	/// <summary>Stores the header information of a single resource entry within an LFD archive.</summary>
	public class LfdResourceInfo
	{
		public string type;
		public string name;
		public int length;
		public LfdResourceInfo()
		{
			type = "";
			name = "";
			length = 0;
		}
		public void ReadFromStream(BinaryReader br)
		{
			type = new string(br.ReadChars(4)).Trim();
			name = new string(br.ReadChars(8)).Trim();
			if (type.IndexOf('\0') >= 0)
				type = type.Remove(type.IndexOf('\0'));
			if (name.IndexOf('\0') >= 0)
				name = name.Remove(name.IndexOf('\0'));
			length = br.ReadInt32();
		}
		public LfdCraftFormat GetCraftFormat()
		{
			if (type == "CRFT") return LfdCraftFormat.CRFT;
			else if (type == "CPLX") return LfdCraftFormat.CPLX;
			else if (type == "SHIP") return LfdCraftFormat.SHIP;
			return LfdCraftFormat.None;
		}
	}

	/// <summary>The central class exposed to MapForm for accessing and managing wireframe models.</summary>
	/// <remarks>Most of the heavy-lifting is managed internally, abstracting as much work as possible away from the MapForm.</remarks>
	public class WireframeManager
	{
		Settings.Platform _curPlatform = Settings.Platform.None;   // The current loaded platform.
		string _curMissionPath = "";                               // The full path+filename of the current mission file. Used when attempting to auto-detect the game directory for available model files.
		string _curInstallPath = "";                               // The current installation path. Used to detect if the user has changed the config, and reload if necessary.
		string _modelLoadDirectory = "";                           // Contains the resolved path that all model resources are loaded from.

		Dictionary<int, WireframeDefinition> wireframeDefinitions = null;    // Indexed by craftType, to store the loaded wireframe definition for that craft.
		List<WireframeInstance> wireframeInstances = null;                   // Indexed by FlightGroup, so that each item in the map has its own instance.
		List<CraftData> _craftData = null;                                   // Indexed by craftType. Contains informational data loaded from an external file, most importantly the species resource names to load.

		// Only used for DOS formats.
		Dictionary<string, string> _dosSpeciesMap = null;          // Maps a list of all available species (as scanned from the SPECIES*.LFD archives) to the full path+filename of the archive it can be loaded from. (Ex: DREAD -> *path*\SPECIES2.LFD)
		LfdCraftFormat _dosCraftFormat = LfdCraftFormat.None;      // Required for X-wing, specifically one file (BWING.CRF). It exists as a standalone file, not archived in SPECIES.LFD. Since the format cannot be derived from the file extension (XW93 and XW94 have the same file name), the context must be determined from the assets within SPECIES.LFD.

		public WireframeManager()
		{
			wireframeDefinitions = new Dictionary<int, WireframeDefinition>();
			wireframeInstances = new List<WireframeInstance>();
		}

		/// <summary>Creates a WireframeInstance, or retrieves an existing one.</summary>
		/// <remarks>Automatically replaces the instance if the craftType or fgIndex has changed.</remarks>
		public WireframeInstance GetOrCreateWireframeInstance(int craftType, int fgIndex)
		{
			if (fgIndex < 0)
				return null;

			WireframeDefinition def = GetWireframeDefinition(craftType);
			if (def == null)
				return null;

			// Pad the list so there's no out-of-bounds problems.
			while (wireframeInstances.Count <= fgIndex)
			{
				wireframeInstances.Add(null);
			}

			if (wireframeInstances[fgIndex] == null)
				wireframeInstances[fgIndex] = CreateWireframeInstance(def, craftType, fgIndex);
			else
				wireframeInstances[fgIndex] = Update(wireframeInstances[fgIndex], craftType, fgIndex);

			return wireframeInstances[fgIndex];
		}

		/// <summary>Prepares the manager to use a specific platform.</summary>
		/// <remarks>Handles basic tasks required when changing platforms, resetting the model cache and determining a new directory to load models from.</remarks>
		public void SetPlatform(Settings.Platform platform, Settings config)
		{
			// Retrieve the current installation path. If the platform remains the same, but the user has chosen a different folder, we'll be able to reload.
			string installPath = CraftDataManager.GetInstance().GetInstallPath();

			if (_curPlatform != platform || config.LastMission != _curMissionPath || _curInstallPath != installPath)
			{
				// Prepare new cache and reset the loading context.
				wireframeDefinitions = new Dictionary<int, WireframeDefinition>();
				wireframeInstances = new List<WireframeInstance>();
				_dosCraftFormat = LfdCraftFormat.None;
				_dosSpeciesMap = new Dictionary<string, string>();

				if (platform != Settings.Platform.None)
				{
					_modelLoadDirectory = CraftDataManager.GetInstance().GetModelPath();

					// Detect if DOS models exist and retrieve the information necessary to load them.
					string path = Path.Combine(_modelLoadDirectory, "species.lfd");
					if (File.Exists(path))
					{
						ParseSpeciesFile(path);
						if (platform == Settings.Platform.TIE)
						{
							ParseSpeciesFile(Path.Combine(_modelLoadDirectory, "species2.lfd"));
							ParseSpeciesFile(Path.Combine(_modelLoadDirectory, "species3.lfd"));
						}
					}
				}
			}
			_curMissionPath = config.LastMission;
			_curInstallPath = installPath;
			_craftData = CraftDataManager.GetInstance().GetCraftDataList();
			_curPlatform = platform;
		}

		/// <summary>Opens a DOS SPECIES*.LFD archive, generating a list of resources it contains.</summary>
		/// <remarks>Also detects the craft file format to establish a proper loading context.</remarks>
		private void ParseSpeciesFile(string filename)
		{
			if (!File.Exists(filename))
				return;
			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					using (BinaryReader br = new BinaryReader(fs, System.Text.Encoding.GetEncoding(437)))
					{
						LfdResourceInfo resource = new LfdResourceInfo();
						resource.ReadFromStream(br);
						if (resource.type == "RMAP")
						{
							int count = resource.length / 16;
							for (int i = 0; i < count; i++)
							{
								resource.ReadFromStream(br);

								_dosSpeciesMap.Add(resource.name.ToLower(), filename);
								if (_dosCraftFormat == LfdCraftFormat.None)
									_dosCraftFormat = resource.GetCraftFormat();
							}
						}
					}
				}
			}
			catch (Exception) { }
		}

		/// <summary>Loads a model definition into the cache, or retrieves an already existing cache entry.</summary>
		/// <remarks>If not already loaded, searches through the possible resource names to find a matching OPT or DOS species entry. If found, the mesh is loaded and converted to a ready format that the wireframe system can use.</remarks>
		/// <returns>Returns a model definition. If the model failed to load, the definition will be empty, but valid. Returns null if out of range.</returns>
		private WireframeDefinition GetWireframeDefinition(int craftType)
		{
			// Check if already loaded.
			if (wireframeDefinitions.ContainsKey(craftType))
				return wireframeDefinitions[craftType];
			if (_craftData == null || craftType < 0 || craftType >= _craftData.Count)
				return null;

			string resourceNames = _craftData[craftType].resourceNames.ToLower();
			WireframeDefinition def = null;
			if (_dosCraftFormat == LfdCraftFormat.None)
			{
				OptFile opt = new OptFile();
				string[] names = resourceNames.Split('|');
				foreach (string s in names)
				{
					string s2 = s;
					if (s2.IndexOf('*') >= 0)
						s2 = s2.Remove(s2.IndexOf('*'));
					if (_curPlatform == Settings.Platform.BoP)
					{
						if (opt.LoadFromFile(Path.Combine(_modelLoadDirectory, s2 + ".op1")))
							break;
					}
					if (opt.LoadFromFile(Path.Combine(_modelLoadDirectory, s2 + ".opt")))
						break;
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
					if (_dosSpeciesMap.ContainsKey(s2))
					{
						if (craft.LoadFromArchive(_dosSpeciesMap[s2], s2))
							break;
					}
					else
					{
						// This is really only required for the B-wing in the DOS versions of XWING, which exists in a standalone file.
						if (craft.LoadFromFile(Path.Combine(_modelLoadDirectory, s2 + ".cft"), _dosCraftFormat))
							break;
					}
				}
				def = new WireframeDefinition(craft);
				def.Scale(scale);
			}
			wireframeDefinitions.Add(craftType, def);
			return def;
		}

		/// <summary>Creates a new WireframeInstance from a definition.</summary>
		private WireframeInstance CreateWireframeInstance(WireframeDefinition def, int craftType, int fgIndex)
		{
			if (def == null)
				return null;
			return new WireframeInstance(def, craftType, fgIndex);
		}

		/// <summary>Checks if an existing WireframeInstance needs to change its model type.</summary>
		/// <returns>Returns the current instance if nothing changed, otherwise returns a new instance.</returns>
		private WireframeInstance Update(WireframeInstance currentInstance, int craftType, int fgIndex)
		{
			if (currentInstance == null)
				return null;

			WireframeInstance ret = currentInstance;
			if (craftType != currentInstance.assignedCraftType)
			{
				WireframeDefinition def = GetWireframeDefinition(craftType);
				ret = CreateWireframeInstance(def, craftType, fgIndex);
			}
			else
			{
				currentInstance.CheckAssignment(craftType, fgIndex);
			}
			return ret;
		}
	}
}