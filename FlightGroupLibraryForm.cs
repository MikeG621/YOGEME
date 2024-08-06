/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.8+
 */

/* CHANGELOG:
 * [UPD] Mostly cleanup
 * [UPD] ArrDep renames
 * v1.8, 201004
 * [NEW] Release [JB]
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Idmr.Platform;

namespace Idmr.Yogeme
{
	public partial class FlightGroupLibraryForm : Form
	{
		const int _libraryHeaderId = 0x4C474659;      // "YFGL" header signature.
		const int _libraryVersion = 1;

		Settings.Platform _platform = Settings.Platform.None;
		readonly object _flightGroupCollection = null;
		readonly EventHandler _onAddEvent;

		List<List<object>> _groupList = null;        // A list of library groups, each group containing a list of craft objects. Each craft object may be casted to a platform-specific FlightGroup.
		List<string> _groupNames = null;             // User-defined names for each library.
		List<List<string>> _craftProblems = null;    // A list of logic issues of all items in the current library group. Each FlightGroup contains a list of strings describing any problems. Logic issues are direct FG references that may cause unintended behavior if added to a mission without scrubbing them.

		bool _isDirty = false;

		/// <summary>Initializes and prepares the FG Library for the specified platform.</summary>
		/// <remarks>The library is saved automatically when the form is closed.</remarks>
		/// <param name="platform">The platform determines which library to load, and how to access its data.</param>
		/// <param name="flightGroupCollection">The collection of craft currently in the mission. Must be a valid FlightGroupCollection object.</param>
		/// <param name="onAddCallback">Event to call when the user requests to add FlightGroups into the mission.</param>
		/// <exception cref="ArgumentNullException">The FlightGroupCollection or callback event is null.</exception>
		/// <exception cref="ArgumentException">The supplied FlightGroupCollection does not match expected type for the platform.</exception>
		public FlightGroupLibraryForm(Settings.Platform platform, object flightGroupCollection, EventHandler onAddEvent)
		{
			InitializeComponent();

			if (flightGroupCollection == null || onAddEvent == null) throw new ArgumentNullException("Invalid arguments to FG Library");

			string collectionType = flightGroupCollection.GetType().ToString();
			string requiredType = "";
			switch (platform)
			{
				case Settings.Platform.XWING: requiredType = typeof(Platform.Xwing.FlightGroupCollection).ToString(); break;
				case Settings.Platform.TIE: requiredType = typeof(Platform.Tie.FlightGroupCollection).ToString(); break;
				case Settings.Platform.XvT: case Settings.Platform.BoP: requiredType = typeof(Platform.Xvt.FlightGroupCollection).ToString(); break;
				case Settings.Platform.XWA: requiredType = typeof(Platform.Xwa.FlightGroupCollection).ToString(); break;
			}
			if (collectionType != requiredType) throw new ArgumentException("Invalid collection passed to FG Library");

			_flightGroupCollection = flightGroupCollection;
			_onAddEvent = onAddEvent;
			resetDefaultLibrary();
			loadLibrary(platform);
			refreshGroupList();
			refreshMissionCraft();
			refreshLibraryCraft();
			Show();
		}

		#region methods
		/// <summary>Deletes the selected library craft from the group.</summary>
		void deleteLibraryCraftSelection()
		{
			// [JB] I tried adding a keypress handler for delete, but this caused all sorts of weird problems.  Possibly event race conditions with drag selection.
			ListBox.SelectedIndexCollection sic = lstLibraryCraft.SelectedIndices;
			int group = lstLibraryGroup.SelectedIndex;
			if (sic.Count > 0 && group >= 0 && group < _groupList.Count)
			{
				while (sic.Count > 0)
				{
					int index = sic[sic.Count - 1];
					_groupList[group].RemoveAt(index);
					lstLibraryCraft.Items.RemoveAt(index); // Removing will modify the selected collection too.
				}
				_isDirty = true;
			}
			// There are some weird anomalies if deleting while a mouse-drag is in progress, where the underlying selection data doesn't correspond to what the user actually sees.
			// This doesn't entirely solve the problem, but it helps.  Keeping this just to be safe.
			lstLibraryCraft.Enabled = false;
			for (int i = 0; i < lstLibraryCraft.Items.Count; i++) if (lstLibraryCraft.GetSelected(i)) lstLibraryCraft.SetSelected(i, false);
			lstLibraryCraft.Enabled = true;

			refreshGroupCraftCount();
		}

		/// <summary>Retrieves the IFF color to draw an item in the craft lists.</summary>
		/// <remarks>This code is adapted from the getFlightGroupDrawColor() functions of each platform.</remarks>
		Brush getFlightGroupDrawColor(object fg)
		{
			Brush brText = SystemBrushes.ControlText;
			Brush[] xwingColors = { Brushes.DodgerBlue, Brushes.Lime, Brushes.Red, Brushes.DodgerBlue, Brushes.RoyalBlue };
			Brush[] tieColors = { Brushes.Lime, Brushes.Red, Brushes.DodgerBlue, Brushes.MediumOrchid, Brushes.OrangeRed, Brushes.DarkOrchid };
			Brush[] xvtxwaColors = { Brushes.Lime, Brushes.Red, Brushes.DodgerBlue, Brushes.Yellow, Brushes.OrangeRed, Brushes.MediumOrchid };

			int iff;
			switch (_platform)
			{
				case Settings.Platform.XWING:
					brText = Brushes.White; // Default to white for objects
					if (((Platform.Xwing.FlightGroup)fg).IsFlightGroup())
					{
						iff = ((Platform.Xwing.FlightGroup)fg).GetActualIFF();
						brText = (iff <= 4 ? xwingColors[iff] : SystemBrushes.ControlText);
					}
					break;
				case Settings.Platform.TIE:
					iff = ((Platform.Tie.FlightGroup)fg).IFF;
					if (iff <= 5) brText = tieColors[iff];
					if (((Platform.Tie.FlightGroup)fg).Difficulty == BaseFlightGroup.Difficulties.Never) brText = Brushes.Gray;
					break;
				case Settings.Platform.XvT:
				case Settings.Platform.BoP:
					iff = ((Platform.Xvt.FlightGroup)fg).IFF;
					if (iff <= 5) brText = xvtxwaColors[iff];
					if (((Platform.Xvt.FlightGroup)fg).Difficulty == BaseFlightGroup.Difficulties.Never) brText = Brushes.Gray;
					break;
				case Settings.Platform.XWA:
					iff = ((Platform.Xwa.FlightGroup)fg).IFF;
					if (iff <= 5) brText = xvtxwaColors[iff];
					if (((Platform.Xwa.FlightGroup)fg).Difficulty == BaseFlightGroup.Difficulties.Never) brText = Brushes.Gray;
					break;
			}
			return brText;
		}

		/// <summary>Retrieves a generic object from within the platform-specific FlightGroupCollection.</summary>
		object getFlightGroupObjectFromCollection(int index)
		{
			switch (_platform)
			{
				case Settings.Platform.XWING: return ((Platform.Xwing.FlightGroupCollection)_flightGroupCollection)[index];
				case Settings.Platform.TIE: return ((Platform.Tie.FlightGroupCollection)_flightGroupCollection)[index];
				case Settings.Platform.XvT: case Settings.Platform.BoP: return ((Platform.Xvt.FlightGroupCollection)_flightGroupCollection)[index];
				case Settings.Platform.XWA: return ((Platform.Xwa.FlightGroupCollection)_flightGroupCollection)[index];
			}
			return "";
		}

		/// <summary>Returns the craft string as it would appear in the FlightGroup list.</summary>
		string getFlightGroupString(object fg)
		{
			try
			{
				switch (_platform)
				{
					case Settings.Platform.XWING: return ((Platform.Xwing.FlightGroup)fg).ToString(true);
					case Settings.Platform.TIE: return ((Platform.Tie.FlightGroup)fg).ToString(true);
					case Settings.Platform.XvT: case Settings.Platform.BoP: return ((Platform.Xvt.FlightGroup)fg).ToString(true);
					case Settings.Platform.XWA: return ((Platform.Xwa.FlightGroup)fg).ToString(true);
				}
			}
			catch (Exception) { /* do nothing */ }
			return "(invalid)";
		}

		/// <summary>Returns the full path and filename of the platform-specific library data.</summary>
		string getFullPath(string libraryFileName) => Path.Combine(Directory.GetCurrentDirectory(), libraryFileName);

		/// <summary>Returns a string containing the group name and its number of items.</summary>
		string getGroupName(int group) => group < 0 || group >= _groupList.Count ? "" : _groupNames[group] + "  (" + _groupList[group].Count + ")";

		/// <summary>Returns the filename where the platform-specific library data is stored.</summary>
		string getLibraryFileName()
		{
			switch (_platform)
			{
				case Settings.Platform.XWING: return "fg_library_xw.bin";
				case Settings.Platform.TIE: return "fg_library_tie.bin";
				case Settings.Platform.XvT: case Settings.Platform.BoP: return "fg_library_xvt.bin";
				case Settings.Platform.XWA: return "fg_library_xwa.bin";
			}
			return "";
		}

		/// <summary>Resolves the target craft name of an FG index within the mission collection.</summary>
		string getProblemTargetName(int index)
		{
			if (index >= 0 && index < lstMissionCraft.Items.Count)
			{
				object targ = getFlightGroupObjectFromCollection(index);
				if (targ.GetType().ToString().Contains("FlightGroup")) return "[#" + (index + 1).ToString() + ": " + ((Platform.BaseFlightGroup)getFlightGroupObjectFromCollection(index)).ToString() + "]";
			}
			return "[#" + (index + 1).ToString() + " out of range]";
		}

		/// <summary>Activates a platform and loads its corresponding library data.</summary>
		void loadLibrary(Settings.Platform platform)
		{
			// Downgrade BoP so it can share the same library.
			if (platform == Settings.Platform.BoP) platform = Settings.Platform.XvT;

			_platform = platform;

			string label = "Tm - GG  - waves x craft (GU)";   // XvT, XWA
			if (_platform == Settings.Platform.XWING) label = "IFF - waves x craft";
			else if (_platform == Settings.Platform.TIE) label = "IFF - GG - waves x craft";
			lblMissionCraft.Text = label;
			lblLibraryCraft.Text = label;

			string filename = getLibraryFileName();
			if (filename == "") return;

			string path = getFullPath(filename);
			if (File.Exists(path))
			{
				try { using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open))) loadLibraryFromStream(br); }
				catch (Exception x) { MessageBox.Show("Error loading FG library!" + Environment.NewLine + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			}
		}

		/// <summary>Loads the contents of a library from an open file stream.</summary>
		void loadLibraryFromStream(BinaryReader br)
		{
			int fileId = br.ReadInt32();
			int fileVersion = br.ReadInt32();
			int filePlatform = br.ReadInt32();
			if (fileId != _libraryHeaderId || fileVersion != _libraryVersion || filePlatform != (int)_platform) return;

			List<object> group = _groupList[0];
			int groupCount = br.ReadInt32();
			for (int i = 0; i < groupCount; i++)
			{
				string groupName = br.ReadString();
				if (i > 0)
				{
					_groupNames.Add(groupName);
					group = new List<object>();
				}
				int craftCount = br.ReadInt32();
				br.ReadInt32(); // Reserved for future expansion data if needed.
				for (int j = 0; j < craftCount; j++)
				{
					System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					object fg = formatter.Deserialize(br.BaseStream);
					group.Add(fg);
				}
				if (i > 0) _groupList.Add(group);
			}
		}

		/// <summary>Adds a new problem string to a list of problems.</summary>
		void logProblem(string problem, List<string> output)
		{
			if (output == null) return;

			if (problem.Contains("FG:"))
			{
				int index = Common.ParseIntAfter(problem, "FG:");
				string target = getProblemTargetName(index);
				problem = problem.Replace("FG:" + index, target);
			}
			output.Add("  " + problem);
		}

		/// <summary>Initializes and resets a library to its default empty state.</summary>
		void resetDefaultLibrary()
		{
			_groupList = new List<List<object>> { new List<object>() };
			_groupNames = new List<string> { "Default" };
			_craftProblems = new List<List<string>>();
		}

		/// <summary>Refreshes the list of library groups contained within the library.</summary>
		void refreshGroupList()
		{
			if (_groupNames.Count < 1 || _groupList.Count < 1) resetDefaultLibrary();
			if (_groupNames[0] != "Default") _groupNames[0] = "Default";

			int prevSelection = lstLibraryGroup.SelectedIndex;
			if (prevSelection == -1) prevSelection = 0;

			lstLibraryGroup.Items.Clear();
			cboLibraryGroup.Items.Clear();
			for (int i = 0; i < _groupNames.Count; i++)
			{
				lstLibraryGroup.Items.Add(getGroupName(i));
				cboLibraryGroup.Items.Add(getGroupName(i));
			}
			lstLibraryGroup.SelectedIndex = prevSelection;
			if (cboLibraryGroup.SelectedIndex == -1) cboLibraryGroup.SelectedIndex = 0;
		}

		/// <summary>Refreshes the visible craft count in the group listing.</summary>
		void refreshGroupCraftCount()
		{
			int group = lstLibraryGroup.SelectedIndex;
			if (group < 0 || group >= _groupList.Count) return;

			lstLibraryGroup.Items[group] = getGroupName(group);
			cboLibraryGroup.Items[group] = getGroupName(group);
		}

		/// <summary>Refreshes the contents of the craft list of the selected library group.</summary>
		void refreshLibraryCraft()
		{
			lstLibraryCraft.Items.Clear();
			int group = lstLibraryGroup.SelectedIndex;
			if (group < 0 || group >= _groupList.Count) return;

			for (int i = 0; i < _groupList[group].Count; i++) lstLibraryCraft.Items.Add(getFlightGroupString(_groupList[group][i]));
			refreshProblems();
		}

		/// <summary>Refreshes the contents of the mission craft list.</summary>
		void refreshMissionCraft()
		{
			lstMissionCraft.Items.Clear();
			switch (_platform)
			{
				case Settings.Platform.XWING:
					foreach (Platform.Xwing.FlightGroup fg in (Platform.Xwing.FlightGroupCollection)_flightGroupCollection) lstMissionCraft.Items.Add(fg.ToString(true));
					break;
				case Settings.Platform.TIE:
					foreach (Platform.Tie.FlightGroup fg in (Platform.Tie.FlightGroupCollection)_flightGroupCollection) lstMissionCraft.Items.Add(fg.ToString(true));
					break;
				case Settings.Platform.XvT:
				case Settings.Platform.BoP:
					foreach (Platform.Xvt.FlightGroup fg in (Platform.Xvt.FlightGroupCollection)_flightGroupCollection) lstMissionCraft.Items.Add(fg.ToString(true));
					break;
				case Settings.Platform.XWA:
					foreach (Platform.Xwa.FlightGroup fg in (Platform.Xwa.FlightGroupCollection)_flightGroupCollection) lstMissionCraft.Items.Add(fg.ToString(true));
					break;
			}
		}

		/// <summary>Rebuilds the list of FG reference problems in the library craft list.</summary>
		void refreshProblems()
		{
			_craftProblems.Clear();
			int group = lstLibraryGroup.SelectedIndex;
			if (group < 0 || group >= _groupList.Count) return;

			for (int i = 0; i < _groupList[group].Count; i++) _craftProblems.Add(scanProblems(_groupList[group][i], false, true));
			lstLibraryCraft.Refresh();
		}

		/// <summary>Opens a platform-specific library file and writes the entire library into it.</summary>
		void saveLibrary()
		{
			string filename = getFullPath(getLibraryFileName());
			try
			{

				using (BinaryWriter br = new BinaryWriter(new FileStream(filename, FileMode.OpenOrCreate)))
				{
					br.Write(_libraryHeaderId);
					br.Write(_libraryVersion);
					br.Write((int)_platform);
					br.Write(_groupList.Count);
					for (int i = 0; i < _groupList.Count; i++)
					{
						br.Write(_groupNames[i]);
						br.Write(_groupList[i].Count);
						br.Write((int)0);  // Reserved for future expansion data if needed.
						for (int j = 0; j < _groupList[i].Count; j++)
						{
							System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
							formatter.Serialize(br.BaseStream, _groupList[i][j]);
						}
					}
				}
			}
			catch (Exception x) { MessageBox.Show("Unable to save FG library!" + Environment.NewLine + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}

		/// <summary>Scans a BaseFlightGroup's arrival/departure motherships for FG reference problems.</summary>
		void scanMothershipProblems(BaseFlightGroup bfg, bool scrubProblems, List<string> output)
		{
			if (bfg.ArriveViaMothership)
			{
				logProblem("Arrival mothership is FG:" + bfg.ArrivalMothership, output);
				if (scrubProblems) { bfg.ArriveViaMothership = false; bfg.ArrivalMothership = 0; }
			}
			if (bfg.AlternateMothershipUsed)
			{
				logProblem("Alternate mothership is FG:" + bfg.AlternateMothership, output);
				if (scrubProblems) { bfg.AlternateMothershipUsed = false; bfg.AlternateMothership = 0; }
			}
			if (bfg.DepartViaMothership)
			{
				logProblem("Departure mothership is FG:" + bfg.DepartureMothership, output);
				if (scrubProblems) { bfg.DepartViaMothership = false; bfg.DepartureMothership = 0; }
			}
			if (bfg.CapturedDepartViaMothership)
			{
				logProblem("Captured mothership is FG:" + bfg.CapturedDepartureMothership, output);
				if (scrubProblems) { bfg.CapturedDepartViaMothership = false; bfg.CapturedDepartureMothership = 0; }
			}
		}

		/// <summary>Checks FlightGroup for potential issues caused by explicit FlightGroup reference indices that may cause unintended behavior when added into the mission.</summary>
		/// <remarks>Checks mothership arrival/departure, all triggers and skip triggers, and orders.</remarks>
		/// <param name="fg">FlightGroup object to check.</param>
		/// <param name="scrubProblems">If true, if any FlightGroup references are detected or used in triggers, they will be replaced with null values.</param>
		/// <param name="compileProblems">If true, specifies that a list strings will be returned, detailing all FlightGroup references used by this FlightGroup.</param>
		/// <returns>Returns null if <paramref name="compileProblems"/> is false. Otherwise returns a list of strings.</returns>
		List<string> scanProblems(object fg, bool scrubProblems, bool compileProblems)
		{
			List<string> errors = (compileProblems ? new List<string>() : null);
			BaseFlightGroup bfg = (BaseFlightGroup)fg;
			byte fgType = (byte)Platform.Xwa.Mission.Trigger.TypeList.FlightGroup;
			byte notFgType = (byte)Platform.Xwa.Mission.Trigger.TypeList.NotFG;
			byte falseCond = (byte)Platform.Xwa.Mission.Trigger.ConditionList.False;
			// Begin the list with a craft name indicating the object we're checking.
			errors?.Add(bfg.ToString());
			switch (_platform)
			{
				case Settings.Platform.XWING:
					Platform.Xwing.FlightGroup xwing = (Platform.Xwing.FlightGroup)fg;
					if (xwing.Mothership >= 0)
					{
						if (!xwing.ArriveViaHyperspace)
						{
							logProblem("Arrival mothership is FG:" + xwing.Mothership, errors);
							if (scrubProblems) xwing.ArriveViaHyperspace = true;
						}
						if (!xwing.DepartViaHyperspace)
						{
							logProblem("Departure mothership is FG:" + xwing.Mothership, errors);
							if (scrubProblems) xwing.DepartViaHyperspace = true;
						}
						if (scrubProblems) xwing.Mothership = -1;
					}
					if (xwing.ArrivalFG >= 0)
					{
						logProblem("Arrival trigger is FG:" + xwing.ArrivalFG, errors);
						if (scrubProblems)
						{
							xwing.ArrivalFG = -1;
							xwing.ArrivalEvent = 0;
						}
					}
					if (xwing.TargetPrimary >= 0)
					{
						logProblem("Primary target is FG:" + xwing.TargetPrimary, errors);
						if (scrubProblems) xwing.TargetPrimary = -1;
					}
					if (xwing.TargetSecondary >= 0)
					{
						logProblem("Secondary target is FG:" + xwing.TargetSecondary, errors);
						if (scrubProblems) xwing.TargetSecondary = -1;
					}
					break;
				case Settings.Platform.TIE:
					Platform.Tie.FlightGroup tie = (Platform.Tie.FlightGroup)fg;
					scanMothershipProblems(tie, scrubProblems, errors);
					for (int i = 0; i < tie.ArrDepTriggers.Length; i++)
					{
						Platform.Tie.Mission.Trigger trig = tie.ArrDepTriggers[i];
						if (trig.Condition != 0 && trig.Condition != falseCond && trig.VariableType == fgType)
						{
							logProblem("ArrDep trigger #" + (i + 1) + " references FG:" + trig.Variable, errors);
							if (scrubProblems) { trig.Condition = 0; trig.Variable = 0; trig.VariableType = 0; }
						}
					}
					for (int i = 0; i < tie.Orders.Length; i++)
					{
						Platform.Tie.FlightGroup.Order order = tie.Orders[i];
						if (order.Target1Type == fgType) { logProblem("Order #" + (i + 1) + " Target1 references FG:" + order.Target1, errors); if (scrubProblems) { order.Target1 = 0; order.Target1Type = 0; } }
						if (order.Target2Type == fgType) { logProblem("Order #" + (i + 1) + " Target2 references FG:" + order.Target2, errors); if (scrubProblems) { order.Target2 = 0; order.Target2Type = 0; } }
						if (order.Target3Type == fgType) { logProblem("Order #" + (i + 1) + " Target3 references FG:" + order.Target3, errors); if (scrubProblems) { order.Target3 = 0; order.Target3Type = 0; } }
						if (order.Target4Type == fgType) { logProblem("Order #" + (i + 1) + " Target4 references FG:" + order.Target4, errors); if (scrubProblems) { order.Target4 = 0; order.Target4Type = 0; } }
					}
					break;
				case Settings.Platform.XvT:
				case Settings.Platform.BoP:
					Platform.Xvt.FlightGroup xvt = (Platform.Xvt.FlightGroup)fg;
					scanMothershipProblems(xvt, scrubProblems, errors);
					for (int i = 0; i < xvt.ArrDepTriggers.Length; i++)
					{
						Platform.Xvt.Mission.Trigger trig = xvt.ArrDepTriggers[i];
						if (trig.Condition != 0 && trig.Condition != falseCond && (trig.VariableType == fgType || trig.VariableType == notFgType))
						{
							logProblem("ArrDep trigger #" + (i + 1) + " references FG:" + trig.Variable, errors);
							if (scrubProblems) { trig.Condition = 0; trig.Variable = 0; trig.VariableType = 0; }
						}
					}
					for (int i = 0; i < xvt.Orders.Length; i++)
					{
						Platform.Xvt.FlightGroup.Order order = xvt.Orders[i];
						if (order.Target1Type == fgType || order.Target1Type == notFgType) { logProblem("Order #" + (i + 1) + " Target1 references FG:" + order.Target1, errors); if (scrubProblems) { order.Target1 = 0; order.Target1Type = 0; } }
						if (order.Target2Type == fgType || order.Target2Type == notFgType) { logProblem("Order #" + (i + 1) + " Target2 references FG:" + order.Target2, errors); if (scrubProblems) { order.Target2 = 0; order.Target2Type = 0; } }
						if (order.Target3Type == fgType || order.Target3Type == notFgType) { logProblem("Order #" + (i + 1) + " Target3 references FG:" + order.Target3, errors); if (scrubProblems) { order.Target3 = 0; order.Target3Type = 0; } }
						if (order.Target4Type == fgType || order.Target4Type == notFgType) { logProblem("Order #" + (i + 1) + " Target4 references FG:" + order.Target4, errors); if (scrubProblems) { order.Target4 = 0; order.Target4Type = 0; } }
					}
					for (int i = 0; i < xvt.SkipToOrder4Trigger.Length; i++)
					{
						Platform.Xvt.Mission.Trigger trig = xvt.SkipToOrder4Trigger[i];
						if (trig.Condition != 0 && trig.Condition != falseCond && (trig.VariableType == fgType || trig.VariableType == notFgType))
						{
							logProblem("Skip trigger #" + (i + 1) + " references FG:" + trig.Variable, errors);
							if (scrubProblems) { trig.Condition = 0; trig.Variable = 0; trig.VariableType = 0; }
						}
					}
					break;
				case Settings.Platform.XWA:
					Platform.Xwa.FlightGroup xwa = (Platform.Xwa.FlightGroup)fg;
					scanMothershipProblems(xwa, scrubProblems, errors);
					for (int i = 0; i < xwa.ArrDepTriggers.Length; i++)
					{
						Platform.Xwa.Mission.Trigger trig = xwa.ArrDepTriggers[i];
						if (trig.Condition != 0 && trig.Condition != falseCond && (trig.VariableType == fgType || trig.VariableType == notFgType))
						{
							logProblem("ArrDep trigger #" + (i + 1) + " references FG:" + trig.Variable, errors);
							if (scrubProblems) { trig.Condition = 0; trig.Variable = 0; trig.VariableType = 0; }
						}
						if (trig.Parameter > 4)
						{
							logProblem("ArrDep trigger #" + (i + 1) + " parameter references FG:" + (trig.Parameter - 1), errors);
							if (scrubProblems) { trig.Parameter = 0; }
						}
					}
					for (int reg = 0; reg < 4; reg++)
					{
						for (int i = 0; i < 4; i++)
						{
							Platform.Xwa.FlightGroup.Order order = xwa.Orders[reg, i];
							if (order.Target1Type == fgType || order.Target1Type == notFgType) { logProblem("Region #" + (reg + 1) + " Order #" + (i + 1) + " Target1 references FG:" + order.Target1, errors); if (scrubProblems) { order.Target1Type = 0; } }
							if (order.Target2Type == fgType || order.Target2Type == notFgType) { logProblem("Region #" + (reg + 1) + " Order #" + (i + 1) + " Target2 references FG:" + order.Target2, errors); if (scrubProblems) { order.Target2Type = 0; } }
							if (order.Target3Type == fgType || order.Target3Type == notFgType) { logProblem("Region #" + (reg + 1) + " Order #" + (i + 1) + " Target3 references FG:" + order.Target3, errors); if (scrubProblems) { order.Target3Type = 0; } }
							if (order.Target4Type == fgType || order.Target4Type == notFgType) { logProblem("Region #" + (reg + 1) + " Order #" + (i + 1) + " Target4 references FG:" + order.Target4, errors); if (scrubProblems) { order.Target4Type = 0; } }
							for (int k = 0; k < order.SkipTriggers.Length; k++)
							{
								Platform.Xwa.Mission.Trigger trig = order.SkipTriggers[k];
								if (trig.Condition != 0 && trig.Condition != falseCond && (trig.VariableType == fgType || trig.VariableType == notFgType))
								{
									logProblem("Region #" + (reg + 1) + " Order #" + (i + 1) + " Skip trigger #" + (k + 1) + " references FG:" + trig.Variable, errors);
									if (scrubProblems) { trig.Condition = 0; trig.Variable = 0; trig.VariableType = 0; }
								}
								if (trig.Parameter > 4)
								{
									logProblem("Region #" + (reg + 1) + " Order #" + (i + 1) + " Skip trigger #" + (k + 1) + " parameter references FG:" + (trig.Parameter - 1), errors);
									if (scrubProblems) { trig.Parameter = 0; }
								}
							}
						}
					}
					break;
			}

			//Remove the name if no errors were logged.
			if (errors != null && errors.Count == 1) errors.Clear();
			return errors;
		}
		#endregion

		#region controls
		/// <summary>Adds all selected mission craft into the current library group.</summary>
		void cmdAddToLibrary_Click(object sender, EventArgs e)
		{
			int group = lstLibraryGroup.SelectedIndex;
			if (group < 0 || group >= _groupList.Count) return;

			foreach (int si in lstMissionCraft.SelectedIndices)
			{
				using (MemoryStream ms = new MemoryStream(2048))
				{
					System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					formatter.Serialize(ms, getFlightGroupObjectFromCollection(si));
					ms.Position = 0;
					object fg = formatter.Deserialize(ms);

					lstLibraryCraft.Items.Add(lstMissionCraft.Items[si]);
					_groupList[group].Add(fg);
					_isDirty = true;
				}
			}
			refreshGroupCraftCount();
		}
		void cmdAddToMission_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection sic = lstLibraryCraft.SelectedIndices;
			int group = lstLibraryGroup.SelectedIndex;
			if (sic.Count == 0 || group < 0 || group >= _groupList.Count) return;

			object[] sendArray = new object[sic.Count];
			int index = 0;
			// Create an array of cloned objects to send back to the form.
			foreach (int si in sic)
			{
				using (MemoryStream ms = new MemoryStream())
				{
					System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					formatter.Serialize(ms, _groupList[group][si]);
					ms.Position = 0;
					object fg = formatter.Deserialize(ms);
					if (chkAutoscrubAddMission.Checked) scanProblems(fg, true, false);
					sendArray[index++] = fg;
				}
			}
			_onAddEvent(sendArray, new EventArgs());
		}
		void cmdDeleteCraft_Click(object sender, EventArgs e) => deleteLibraryCraftSelection();
		void cmdDeleteGroup_Click(object sender, EventArgs e)
		{
			int group = lstLibraryGroup.SelectedIndex;
			if (group < 1)
			{
				MessageBox.Show("You cannot delete the Default group. Delete its FlightGroup contents instead.", "Note");
				return;
			}
			if (group >= _groupList.Count) return;

			DialogResult res = MessageBox.Show("Are you sure you want to delete the library group \"" + _groupNames[group] + "\" ?" + Environment.NewLine + "It contains " + _groupList[group].Count + " craft.", "Confirm", MessageBoxButtons.YesNo);
			if (res != DialogResult.Yes) return;

			_groupList.RemoveAt(group);
			_groupNames.RemoveAt(group);
			if (group > _groupList.Count - 1) group = _groupList.Count - 1;
			lstLibraryGroup.SelectedIndex = group;
			cboLibraryGroup.SelectedIndex = group;
			refreshGroupList();
			refreshLibraryCraft();
			_isDirty = true;
		}
		void cmdMoveCraftDown_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection sic = lstLibraryCraft.SelectedIndices;
			int group = lstLibraryGroup.SelectedIndex;
			if (sic.Count == 0 || group < 0 || group >= _groupList.Count) return;

			int[] selection = new int[sic.Count];
			for (int i = 0; i < sic.Count; i++) selection[i] = sic[i];
			if (selection[selection.Length - 1] == lstLibraryCraft.Items.Count - 1) return;

			for (int i = selection.Length - 1; i >= 0; i--)
			{
				object temp = _groupList[group][selection[i] + 1];
				_groupList[group][selection[i] + 1] = _groupList[group][selection[i]];
				_groupList[group][selection[i]] = temp;
			}
			refreshLibraryCraft();
			for (int i = 0; i < selection.Length; i++) lstLibraryCraft.SetSelected(selection[i] + 1, true);
			_isDirty = true;
		}
		void cmdMoveCraftToGroup_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection sic = lstLibraryCraft.SelectedIndices;
			int srcGroup = lstLibraryGroup.SelectedIndex;
			int dstGroup = cboLibraryGroup.SelectedIndex;
			if (sic.Count == 0 || srcGroup < 0 || srcGroup >= _groupList.Count || dstGroup < 0 || dstGroup >= _groupList.Count) return;

			if (srcGroup == dstGroup)
			{
				MessageBox.Show("Already in this group.", "Note");
				return;
			}

			for (int i = sic.Count - 1; i >= 0; i--)
			{
				_groupList[dstGroup].Add(_groupList[srcGroup][sic[i]]);
				_groupList[srcGroup].RemoveAt(sic[i]);
			}
			refreshGroupList();
			refreshLibraryCraft();
			_isDirty = true;
		}
		void cmdMoveCraftUp_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection sic = lstLibraryCraft.SelectedIndices;
			int group = lstLibraryGroup.SelectedIndex;
			if (sic.Count == 0 || group < 0 || group >= _groupList.Count) return;

			int[] selection = new int[sic.Count];
			for (int i = 0; i < sic.Count; i++) selection[i] = sic[i];
			if (selection[0] == 0) return;

			for (int i = 0; i < selection.Length; i++)
			{
				object temp = _groupList[group][selection[i] - 1];
				_groupList[group][selection[i] - 1] = _groupList[group][selection[i]];
				_groupList[group][selection[i]] = temp;
			}
			refreshLibraryCraft();
			for (int i = 0; i < selection.Length; i++) lstLibraryCraft.SetSelected(selection[i] - 1, true);
			_isDirty = true;
		}
		void cmdNewGroup_Click(object sender, EventArgs e)
		{
			_groupList.Add(new List<object>());
			string name = txtName.Text.Trim();
			txtName.Text = "";
			if (name == "") name = "New Group";
			_groupNames.Add(name);
			refreshGroupList();
			lstLibraryGroup.SelectedIndex = lstLibraryGroup.Items.Count - 1;
			_isDirty = true;
		}
		void cmdRenameGroup_Click(object sender, EventArgs e)
		{
			int group = lstLibraryGroup.SelectedIndex;
			if (group < 0 || group >= _groupNames.Count) return;

			if (group == 0)
			{
				MessageBox.Show("You cannot rename the Default group.", "Note");
				return;
			}

			string name = txtName.Text.Trim();
			if (name == "") return;

			_groupNames[group] = name;
			refreshGroupList();
			txtName.Text = "";
			_isDirty = true;
		}
		void cmdScrubProblems_Click(object sender, EventArgs e)
		{
			int group = lstLibraryGroup.SelectedIndex;
			if (group < 0 || group >= _groupList.Count) return;

			foreach (int si in lstLibraryCraft.SelectedIndices) if (si >= 0 && si < _groupList[group].Count) scanProblems(_groupList[group][si], true, false);
			refreshProblems();
			_isDirty = true;
		}
		void cmdViewProblems_Click(object sender, EventArgs e)
		{
			string text = "";
			int count = 0;
			foreach (int si in lstLibraryCraft.SelectedIndices)
			{
				if (count > 38)
				{
					text += Environment.NewLine + "Too many problems to list, select fewer craft.";
					break;
				}
				if (text != "") text += Environment.NewLine;
				for (int i = 0; i < _craftProblems[si].Count; i++)
				{
					if (i > 0 && text != "") text += Environment.NewLine;
					text += _craftProblems[si][i];
					count++;
				}
			}
			if (text == "") text = "No problems found.";
			MessageBox.Show(text, "Possible FlightGroup reference problems");
		}

		void form_Activated(object sender, EventArgs e)
		{
			// The main editor form doesn't synchronize its changes with the FG Library.
			// This automatically performs a refresh whenever the user switches focus to the library form.
			refreshMissionCraft();
			refreshProblems();
		}
		void form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_isDirty) return;

			saveLibrary();
			_isDirty = false;
		}
		void form_SizeChanged(object sender, EventArgs e)
		{
			int height = grpCraftManager.Bottom - lstMissionCraft.Top;
			if (ClientRectangle.Bottom > lstMissionCraft.Top + height) height += ClientRectangle.Bottom - (lstMissionCraft.Top + height);
			lstMissionCraft.Size = new Size(lstMissionCraft.Width, height);
			lstLibraryCraft.Size = new Size(lstLibraryCraft.Width, height);
		}

		void lstGroupCraft_DrawItem(object sender, DrawItemEventArgs e)
		{
			Brush brText;
			int group = lstLibraryGroup.SelectedIndex;
			if (group >= 0 && group < _groupList.Count && e.Index >= 0 && e.Index < _groupList[group].Count)
			{
				object fg = _groupList[group][e.Index];
				brText = getFlightGroupDrawColor(fg);
				e.DrawBackground();
				if (e.Index < _craftProblems.Count && _craftProblems[e.Index].Count > 0)
				{
					int height = e.Bounds.Bottom - e.Bounds.Top;
					Rectangle r = new Rectangle(e.Bounds.Right - height - 10, e.Bounds.Top, height + 10, height);
					e.Graphics.DrawString((_craftProblems[e.Index].Count - 1).ToString() + " !", e.Font, Brushes.Red, r, StringFormat.GenericDefault);
				}
				e.Graphics.DrawString(lstLibraryCraft.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
			}
		}
		void lstLibraryGroup_SelectedIndexChanged(object sender, EventArgs e) => refreshLibraryCraft();
		void lstMissionCraft_DrawItem(object sender, DrawItemEventArgs e)
		{
			object fg = getFlightGroupObjectFromCollection(e.Index);
			if (e.Index == -1 || fg.ToString() == "") return;

			e.DrawBackground();
			Brush brText = getFlightGroupDrawColor(fg);
			e.Graphics.DrawString(lstMissionCraft.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		#endregion
	}
}