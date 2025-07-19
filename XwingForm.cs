/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2025 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.17.3
 *
 * CHANGELOG
 * v1.17.3, 250713
 * [FIX #125] initializing briefing events [JB]
 * [FIX #124] NumWave index fix [JB]
 * v1.17.2, 250606
 * [FIX #123] Bad multi-edit properties for hyper/mothership
 * v1.16.0.3, 241027
 * [FIX #110] FG library callback cast exception
 * v1.16, 241013
 * [FIX] SaveAs behavior
 * [UPD] Events changes
 * [UPD] lblODesc actually used now
 * v1.14.2, 230901
 * [FIX #86] FG.CraftType when changing to B-wing
 * v1.13.11, 221030
 * [FIX] Open dialog not following current directory after switching paltforms via "Open Recent"
 * v1.13.7, 220730
 * [FIX] crash when copying a WP value via Ctrl+C and no text is selected
 * v1.13.6, 220619
 * [UPD] Confirm save now only asks if modified
 * v1.13.4, 220606
 * [UPD] OneIndexedFGs implementation
 * v1.13.3, 220402
 * [FIX] ComboBox stutter for colorized cbo's due to OwnerDrawVariable
 * v1.13.2, 220319
 * [FIX] Remove focus during save to trigger any Leave events
 * v1.13.1, 220208
 * [NEW] menuCut [JB]
 * [FIX] multi-select refresh issues [JB]
 * [FIX] craftStart issues during Paste and arrival changes [JB]
 * v1.13, 220130
 * [NEW] Multi-select [JB]
 * v1.12, 220103
 * [NEW] Tour Editor
 * [FIX] Listbox scrolling
 * v1.11.2, 2101005
 * [UPD] Copy/paste now uses system clipboard, can more easily paste external text
 * [NEW] Copy/paste now works for Waypoints
 * v1.10, 210520
 * [UPD #56] Replaced try/catch with TyrParse [JB]
 * v1.9, 210108
 * [FIX] Clipboard path in some locations
 * v1.8.1, 201213
 * [FIX] Removed RunVerify() call
 * [UPD] menuTest moved under Tools, changed to &Test
 * v1.8, 201004
 * [FIX] Deactivate added to force focus fix [JB]
 * [UPD] newFG now returns bool
 * [NEW] FlightGroupLibrary [JB]
 * v1.7, 200816
 * [UPD #14] Nothing specific, but closing that issue
 * [FIX] recalculateEditorCraftNumbering() handles _activeFG now [JB]
 * [UPD] shiplist and Map calls updated for Wireframe implementation [JB]
 * [FIX] Starting ships set to 1 [JB]
 * [UPD] Unk1 now RandomSeed [JB]
 * [UPD] MaxCraft increased to 255 [JB]
 * [UPD] Yaw/Pitch/Roll tweaks, save fixed [JB]
 * [UPD] form handlers renamed
 * [FIX] re-init if load fails
 * v1.6.5, 200704
 * [NEW] Custom shiplist
 * [FIX #32] bin path now explicitly uses Startup Path to prevent implicit from defaulting to sys32
 * v1.6.4, 200119
 * [NEW #30] Briefing callback
 * v1.5, 180910
 * [NEW] Release [JB]
 * [NEW] Added SaveAsXwing
 * [FIX] colorized cbo uses black BG, "None" is black with white text
 */

using System;
using System.Data;	// DataView and DataTable
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Idmr.Platform.Xwing;

namespace Idmr.Yogeme
{
	/// <summary>XWING95 Mission Editor GUI</summary>
	public partial class XwingForm : Form
	{
		#region vars and stuff
		readonly Settings _config = Settings.GetInstance();
		Mission _mission;
		bool _applicationExit;              //for frmTIE_Closing, confirms application exit vs switching platforms
		int _activeFG = 0;          //counter to keep track of current FG being displayed
		int _startingShips = 1;     //counter for craft in play at start <30s, warning above 28
		int _startingObjects = 0;
		bool _loading;      //alerts certain functions to disable during the loading process
		bool _noRefresh = false;
		enum EditorMode
		{
			XWI,
			BRF
		}
		EditorMode _mode = EditorMode.XWI;      //Which FG selection we're currently viewing and editing (0=XWI, 1=BRF)
		Color _original_lstFG_BackColor;

		private readonly int[] _waypointMapping = new int[10] {   //The raw waypoint data is not in a convenient order (Start1,Waypt1,Waypt2,Waypt3,Start2,Start3,Hyper) so assists in providing an intuitive datagrid, mapping the display row (array index) to the actual FG waypoint index (element value).
			0, 4, 5,    //Start1, Start2, Start3
		    1, 2, 3,    //Waypt1, Waypt2, Waypt3
		    6,          //Hyper
            7, 8, 9     //BRF coordinate sets, listed here as virtual waypoints for ease of editing.
		};
		readonly DataTable _table = new DataTable("Waypoints");
		readonly DataTable _tableRaw = new DataTable("Waypoints_Raw");
		
		MapForm _fMap;
		BriefingFormXwing _fBrief;
		FlightGroupLibraryForm _fLibrary;
		TourForm _fTour;
		#endregion
		#region Control Arrays
#pragma warning disable IDE1006 // Naming Styles
		readonly TextBox[] txtEoM = new TextBox[3];
		readonly CheckBox[] chkWP = new CheckBox[7];
		readonly CheckBox[] chkGunPlatform = new CheckBox[6];
		readonly MenuItem[] menuRecentMissions = new MenuItem[6];
		readonly Dictionary<ComboBox, ComboBox> ColorizedFGList = new Dictionary<ComboBox, ComboBox>();  //[JB] Maps a control that should have a colorized FG list with a control that determines whether the list actually contains a FG list.
#pragma warning restore IDE1006 // Naming Styles
		#endregion

		public XwingForm()
		{
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		public XwingForm(string path)
		{   //this is the command line and "Open..." support
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			if (!loadMission(path)) return;
			lstFG.SelectedIndex = 0;
			_loading = false;
		}

		#region methods
		void closeForms()
		{
			try { _fMap.Close(); }
			catch { /* do nothing */ }
			try { _fBrief.Close(); }
			catch { /* do nothing */ }
			try { _fLibrary.Close(); }
			catch { /* do nothing */ }
			try { _fTour.Close(); }
			catch { /* do nothing */ }
		}
		void comboLoadIndex(ComboBox cbo, int index, bool nullable)
		{
			bool btemp = _loading;
			_loading = true;
			try
			{
				if (nullable) index++;
				cbo.SelectedIndex = index;
			}
			catch { cbo.SelectedIndex = 0; }
			_loading = btemp;
		}
		void comboRefreshFGList(ComboBox cbo, bool nullable)
		{
			_loading = true;
			int index = cbo.SelectedIndex;
			cbo.Items.Clear();
			if (nullable == false)
				cbo.Items.AddRange(_mission.FlightGroups.GetList());
			else
				cbo.Items.AddRange(getNullableFGList());
			if (index >= cbo.Items.Count)  //Changing missions could cause existing selected indexes to become invalid
				index = 0;
			cbo.SelectedIndex = index;
			_loading = false;
		}
		static void comboReset(ComboBox cbo, string[] items, int index)
		{
			cbo.Items.Clear();
			cbo.Items.AddRange(items);
			cbo.SelectedIndex = index;
		}
		void recalculateStart()
		{
			_startingShips = 0;
			_startingObjects = 0;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
				craftStart(_mission.FlightGroups[i], true);
		}
		void craftStart(FlightGroup fg, bool add)
		{
			if (fg.IsFlightGroup() && fg.ArrivalEvent != 0) return;
			if (!fg.ArrivesIn30Seconds) return;

			if (fg.IsFlightGroup())
			{
				if (add) _startingShips += fg.NumberOfCraft;
				else _startingShips -= fg.NumberOfCraft;
			}
			else
			{
				int count = fg.NumberOfCraft;
				if (fg.ObjectType >= 18 && fg.ObjectType <= 21)
					count *= fg.NumberOfCraft; //Square minefield.
				if (fg.IsTrainingPlatform())  //Training Platform 1 uses the NumberOfCraft field for minutes on the clock.  Other platforms use it to activate guns.
					count = 1;

				if (add) _startingObjects += count;
				else _startingObjects -= count;
			}
			lblStarting.Text = _startingShips.ToString() + " Craft at 30 seconds" + Environment.NewLine + _startingObjects.ToString() + " Object" + (_startingObjects != 1 ? "s" : "");
			if (_startingShips > Mission.CraftLimit) lblStarting.ForeColor = Color.Red;
			else lblStarting.ForeColor = SystemColors.ControlText;
		}
		Brush getFlightGroupDrawColor(int fgIndex)
		{
			Brush brText = Brushes.White;
			if (fgIndex < 0 || fgIndex >= _mission.FlightGroups.Count) return brText;
			if (_mission.FlightGroups[fgIndex].IsFlightGroup())
			{
				switch (_mission.FlightGroups[fgIndex].GetActualIFF())
				{
					case 0:  //Default IFF will be translated in GetActualIFF().
						brText = Brushes.DodgerBlue;
						break;
					case 1:
						brText = Brushes.Lime; //Brushes.LimeGreen;
						break;
					case 2:
						brText = Brushes.Red; //Brushes.Crimson;
						break;
					case 3:
						brText = Brushes.DodgerBlue;
						break;
					case 4:
						brText = Brushes.RoyalBlue;
						break;
				}
			}
			return brText;
		}
		void initializeMission()
		{
			tabMain.Focus();            // Exit focus from any form controls.  Fixes some issues that might arise from Leave() events trying to access modified lists.
			lstFG.Items.Clear();        // Clearing FGs here prevents issues with ComboBoxes further down.
			_mission = new Mission();
			_config.LastMission = "";
			_activeFG = 0;
			_mission.FlightGroups[0].CraftType = 1;
			_mission.FlightGroups[0].ObjectType = 0;
			_mission.FlightGroups[0].IFF = 1;
			_startingShips = 0;
			_startingObjects = 0;
			craftStart(_mission.FlightGroups[0], true);
			//string[] fgList = _mission.FlightGroups.GetList();
			comboRefreshFGList(cboMothership, true);
			comboRefreshFGList(cboArrFG, true);
			comboRefreshFGList(cboOrderPrimary, true);
			comboRefreshFGList(cboOrderSecondary, true);
			lstFG.Items.Add(_mission.FlightGroups[_activeFG].ToString(true));
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			cboIFF.Items.Clear();
			cboIFF.Items.AddRange(Strings.IFF);
			_mission.MissionPath = "\\NewMission.xwi";
			Text = "Ye Olde Galactic Empire Mission Editor - X-wing - NewMission.xwi";
		}
		void loadCraftData(string fileMission)
		{
			Strings.OverrideShipList(null, null); //Restore defaults.
			try
			{
				CraftDataManager.GetInstance().LoadPlatform(Settings.Platform.XWING, Strings.CraftType, Strings.CraftAbbrv, fileMission);
				Strings.OverrideShipList(CraftDataManager.GetInstance().GetLongNames(), CraftDataManager.GetInstance().GetShortNames());
			}
			catch (Exception x) { MessageBox.Show("Error processing custom XW ship list, using defaults.\n\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			cboCraft.Items.Clear();
			cboCraft.Items.AddRange(Strings.CraftType);
		}
		/*string replaceTargetText(string text)
		{
			while (text.Contains("FG:"))
			{
				int index = text.IndexOf("FG:") + 3;
				int length = text.IndexOfAny(new char[] { ' ', ',', '\0' }, index) - index;
				int fg;
				if (length > 0) fg = int.Parse(text.Substring(index, length));
				else fg = int.Parse(text.Substring(index));
				text = text.Replace("FG:" + fg, _mission.FlightGroups[fg].ToString());
			}
			return text;
		}*/
		bool loadMission(string fileMission)
		{
			/* return true if successful, returns false if aborted or failed
			 * code is fairly straight-forward. read the crap and save it */
			switchTo(EditorMode.XWI);
			closeForms();
			try
			{
				FileStream fs = File.OpenRead(fileMission);
				try
				{
					#region determine platform
					switch (Platform.MissionFile.GetPlatform(fs))
					{
						case Platform.MissionFile.Platform.Xwing:  //[JB] Added support for X-wing
							initializeMission();
							break;
						case Platform.MissionFile.Platform.TIE:
							_applicationExit = false;
							new TieForm(fileMission).Show();
							Close();
							fs.Close(); //[JB] Files were being left open, which could cause access violations.  Need to close stream before returning.
							return false;
						case Platform.MissionFile.Platform.XvT:
							_applicationExit = false;
							new XvtForm(fileMission).Show();
							Close();
							fs.Close();
							return false;
						case Platform.MissionFile.Platform.BoP:
							_applicationExit = false;
							new XvtForm(fileMission).Show();
							Close();
							fs.Close();
							return false;
						case Platform.MissionFile.Platform.XWA:
							_applicationExit = false;
							new XwaForm(fileMission).Show();
							Close();
							fs.Close();
							return false;
						default:
							throw new Exception("File is not a valid mission file for any platform, please select an appropriate *.tie or *.xwi file.");
					}
					#endregion
					_mission.LoadFromStream(fs);
					fs.Close();

					//Unlike other platforms, need to load a second file to get the briefing.
					string brf = fileMission.ToLower().Replace(".xwi", ".brf");
					if (File.Exists(brf))
					{
						fs = File.OpenRead(brf);
						_mission.LoadBriefingFromStream(fs);
						fs.Close();
					}
				}
				catch (Exception x)
				{
					fs.Close();
					throw x;
				}
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				menuNewXwing_Click(0, new EventArgs());
				return false;
			}
			loadCraftData(fileMission);
			lstFG.Items.Clear();
			_startingShips = 0;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
				if (_mission.FlightGroups[i].ArrivesIn30Seconds) craftStart(_mission.FlightGroups[i], true);
			}
			updateFGList();
			updateMissionTabs();
			Text = "Ye Olde Galactic Empire Mission Editor - X-wing - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
			refreshRecent();
			return true;
		}
		void promptSave()
		{
			_config.SaveSettings();
			if (_config.ConfirmSave && (Text.IndexOf("*") != -1))
			{
				DialogResult res = MessageBox.Show("Mission has been edited without saving, would you like to save?", "Confirm", MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes)
				{
					if (_mission.MissionPath == "\\NewMission.xwi") savXW.ShowDialog();
					else saveMission(_mission.MissionPath);
				}
			}
		}
		void refreshRecent()
		{
			for (int i = 1; i < 6; i++)
			{
				menuRecentMissions[i].Text = "&" + i + ": " + _config.RecentMissions[i] + " (" + _config.RecentPlatforms[i].ToString() + ")";
				menuRecentMissions[i].Visible = (_config.RecentMissions[i] != "");
			}
			menuRecentMissions[0].Enabled = menuRecentMissions[1].Visible;
		}
		void registerColorizedFGList(ComboBox Variable, ComboBox VariableType)
		{
			if (!_config.ColorizedDropDowns) return;
			if (Variable == null)
				return;
			ColorizedFGList[Variable] = VariableType;
			Variable.DrawMode = DrawMode.OwnerDrawFixed;
			Variable.DrawItem += colorizedComboBox_DrawItem;
		}
		void registerFgMultiEdit(Control control, string propertyName, MultiEditRefreshType refreshType)
		{
			Common.AddControlChangedHandler(control, flightgroupMultiEditHandler);
			control.Tag = new MultiEditProperty(propertyName, refreshType);
		}
		void saveMission(string fileMission)
		{
			tabMain.Focus();
			switchTo(EditorMode.XWI);
			try { _fBrief.Save(); }
			catch { /* do nothing */ }
			if (Text.IndexOf("*") == -1) return;    // don't save if unmodified

			try { _mission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			Text = "Ye Olde Galactic Empire Mission Editor - X-wing - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
			refreshRecent();  //[JB] Setting _config.LastMission modifies the Recent list.  Need to refresh the menu to match.
			//Verify the mission after it's been saved
			//if (_config.Verify) Common.RunVerify(_mission.MissionPath, _config);
		}
		void startup()
		{
			loadCraftData("");
			switchTo(EditorMode.XWI);
			//initializes cbo's, IFFs, resets bAppExit
			_config.LastMission = "";
			_config.LastPlatform = Settings.Platform.XWING;
			opnXW.InitialDirectory = _config.GetWorkingPath(); //[JB] Updated for MRU access.  Defaults to installation and mission folder if not enabled.
			savXW.InitialDirectory = _config.GetWorkingPath();
			_applicationExit = true;    //becomes false if selecting "New Mission" from menu
			#region Menu
			menuTest.Enabled = _config.XwingInstalled;
			if (_config.RestrictPlatforms)
			{
				menuNewXwing.Enabled = _config.XwingInstalled;
				menuNewXvT.Enabled = _config.XvtInstalled;
				menuNewBoP.Enabled = _config.BopInstalled;
				menuNewXWA.Enabled = _config.XwaInstalled;
			}
			menuRecentMissions[0] = menuRecent;
			menuRecentMissions[1] = menuRec1;
			menuRecentMissions[2] = menuRec2;
			menuRecentMissions[3] = menuRec3;
			menuRecentMissions[4] = menuRec4;
			menuRecentMissions[5] = menuRec5;
			for (int i = 1; i < 6; i++)
			{
				menuRecentMissions[i].Click += new EventHandler(menuRecentMissions_Click);
				menuRecentMissions[i].Tag = i;
			}
			refreshRecent();
			#endregion
			#region Craft
			bool btemp = _loading;
			_loading = true;
			cboCraft.SelectedIndex = _mission.FlightGroups[0].CraftType; // already loaded in loadCraftData
			cboObject.Items.AddRange(Strings.ObjectType); cboObject.SelectedIndex = _mission.FlightGroups[0].ObjectType;
			_loading = btemp;
			cboIFF.SelectedIndex = _mission.FlightGroups[0].IFF;    // already loaded default IFFs at start of function through txtIFF#.Text
			cboAI.Items.AddRange(Strings.Rating); cboAI.SelectedIndex = 3;
			cboMarkings.Items.AddRange(Strings.Color); cboMarkings.SelectedIndex = 0;
			cboPlayer.SelectedIndex = 0;
			cboFormation.Items.AddRange(Strings.Formation); cboFormation.SelectedIndex = 0;
			cboStatus.Items.AddRange(Strings.Status); cboStatus.SelectedIndex = 0;
			#endregion
			#region Arr/Dep
			comboReset(cboArrCondition, Strings.Trigger, 0);
			comboReset(cboOrder, Strings.Orders, 0);
			cboOrderValue.Items.AddRange(Strings.OrderValue);
			for (int i = cboOrderValue.Items.Count; i <= 20; i++)
				cboOrderValue.Items.Add(i.ToString());
			#endregion
			#region Waypoints
			_table.Columns.Add("X"); _table.Columns.Add("Y"); _table.Columns.Add("Z");
			_tableRaw.Columns.Add("X"); _tableRaw.Columns.Add("Y"); _tableRaw.Columns.Add("Z");
			for (int i = 0; i < 10; i++)    //initialize WPs
			{
				DataRow dr = _table.NewRow();
				int j;
				for (j = 0; j < 3; j++) dr[j] = 0;  //set X Y Z to zero
				_table.Rows.Add(dr);
				dr = _tableRaw.NewRow();
				for (j = 0; j < 3; j++) dr[j] = 0;  //mirror in raw table
				_tableRaw.Rows.Add(dr);
			}
			dataWaypoints.Table = _table;
			dataWaypointsRaw.Table = _tableRaw;
			dataWP.DataSource = dataWaypoints;
			dataWP_Raw.DataSource = dataWaypointsRaw;
			this._table.RowChanged += new DataRowChangeEventHandler(table_RowChanged);
			this._tableRaw.RowChanged += new DataRowChangeEventHandler(tableRaw_RowChanged);
			chkWP[0] = chkSP1;
			chkWP[1] = chkSP2;
			chkWP[2] = chkSP3;
			chkWP[3] = chkWP1;
			chkWP[4] = chkWP2;
			chkWP[5] = chkWP3;
			chkWP[6] = chkWPHyp;
			for (int i = 0; i < 7; i++)
			{
				chkWP[i].CheckedChanged += new EventHandler(chkWPArr_CheckedChanged);
				chkWP[i].Tag = i;
			}
			#endregion
			#region FG Goals
			cboPrimGoalT.Items.AddRange(Strings.Objective);
			cboPrimGoalT.SelectedIndex = 0;
			#endregion
			#region Mission
			cboEndEvent.Items.AddRange(Strings.EndEvents);
			cboMissionLocation.Items.AddRange(Strings.MissionLocation);
			txtEoM[0] = txtPrimComp1;
			txtEoM[1] = txtPrimComp2;
			txtEoM[2] = txtPrimComp3;
			for (int i = 0; i < 3; i++)
			{
				txtEoM[i].Leave += new EventHandler(txtEoMArr_Leave);
				txtEoM[i].Tag = i;
			}
			#endregion
			chkGunPlatform[0] = chkGun1;
			chkGunPlatform[1] = chkGun2;
			chkGunPlatform[2] = chkGun3;
			chkGunPlatform[3] = chkGun4;
			chkGunPlatform[4] = chkGun5;
			chkGunPlatform[5] = chkGun6;
			for (int i = 0; i < 6; i++)
				chkGunPlatform[i].CheckedChanged += new EventHandler(chkGunArr_CheckedChanged);
			updateMissionTabs();

			#region ControlRegistration
			registerColorizedFGList(cboMothership, null);
			registerColorizedFGList(cboArrFG, null);
			registerColorizedFGList(cboOrderPrimary, null);
			registerColorizedFGList(cboOrderSecondary, null);

			registerFgMultiEdit(txtName, "Name", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName);
			registerFgMultiEdit(numCraft, "NumberOfCraft", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(numWaves, "NumberOfWaves", MultiEditRefreshType.ItemText);
			registerFgMultiEdit(txtCargo, "Cargo", 0);
			registerFgMultiEdit(txtSpecCargo, "SpecialCargo", 0);
			registerFgMultiEdit(numSC, "SpecialCargoCraft", 0);
			// Craft and object types have special logic, not handled here.
			registerFgMultiEdit(cboIFF, "IFF", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName);
			registerFgMultiEdit(cboAI, "AI", 0);
			registerFgMultiEdit(cboMarkings, "Markings", 0);
			registerFgMultiEdit(cboPlayer, "PlayerCraft", MultiEditRefreshType.ItemText);
			registerFgMultiEdit(cboFormation, "Formation", 0);
			registerFgMultiEdit(numObjectValue, "PlatformValue", 0);
			registerFgMultiEdit(numSeconds, "PlatformSeconds", 0);
			registerFgMultiEdit(cboStatus, "Status", 0);
			registerFgMultiEdit(optArrHyp, "ArriveViaHyper", 0);
			registerFgMultiEdit(optDepHyp, "DepartViaHyper", 0);
			registerFgMultiEdit(cboMothership, "Mothership", 0);
			registerFgMultiEdit(cboArrFG, "ArrivalTrigFlightgroup", 0);
			registerFgMultiEdit(cboArrCondition, "ArrivalTrigCondition", MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(numArrMin, "ArrivalDelayMin", MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(numArrSec, "ArrivalDelaySec", MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(cboPrimGoalT, "Goal", 0);
			registerFgMultiEdit(numPitch, "Pitch", 0);
			registerFgMultiEdit(numYaw, "Yaw", 0);
			registerFgMultiEdit(numRoll, "Roll", 0);
			registerFgMultiEdit(cboOrder, "OrderCommand", 0);
			registerFgMultiEdit(cboOrderPrimary, "OrderPrimary", 0);
			registerFgMultiEdit(cboOrderSecondary, "OrderSecondary", 0);
			registerFgMultiEdit(cboOrderValue, "OrderValue", 0);
			#endregion ControlRegistration

			_original_lstFG_BackColor = lstFG.BackColor;
			lblBRFNotice.Visible = (_mode == EditorMode.BRF);
			cmdImportXWI.Visible = (_mode == EditorMode.BRF);
		}
		void switchTo(EditorMode mode)
		{
			if (mode == _mode)
				return;

			closeForms();

			bool btemp = _loading;
			_loading = true;
			FlightGroupCollection temp = _mission.FlightGroups;
			_mission.FlightGroups = _mission.FlightGroupsBriefing;
			_mission.FlightGroupsBriefing = temp;

			_activeFG = 0;   //Prevent index bounds if selected index is higher in one mode than available FGs in another.
			_mode = mode;
			lstFG.Items.Clear();
			lstFG.Items.AddRange(_mission.FlightGroups.GetList());
			listRefreshAll();
			updateFGList();  //Need to refresh all the comboboxes since the list may change.
			lstFG.SelectedIndex = 0;
			tabMain.SelectedIndex = 0;
			tabArrDep.Enabled = (_mode == EditorMode.XWI);
			tabGoals.Enabled = (_mode == EditorMode.XWI);
			tabOrders.Enabled = (_mode == EditorMode.XWI);
			lblArrDepNote.Visible = (_mode == EditorMode.BRF);
			lblGoalNote.Visible = (_mode == EditorMode.BRF);
			lblOrderNote.Visible = (_mode == EditorMode.BRF);
			lblArrDepNote.Enabled = (_mode == EditorMode.BRF);
			lblGoalNote.Enabled = (_mode == EditorMode.BRF);
			lblOrderNote.Enabled = (_mode == EditorMode.BRF);

			lstFG.BackColor = (_mode == EditorMode.XWI) ? _original_lstFG_BackColor : Color.MidnightBlue; ;
			cmdSwitchFG.Text = "Switch to " + ((_mode == EditorMode.XWI) ? "BRF" : "XWI");
			lblBRFNotice.Visible = (_mode == EditorMode.BRF);
			cmdImportXWI.Visible = (_mode == EditorMode.BRF);
			lblStarting.Visible = (_mode == EditorMode.XWI);
			recalculateStart();
			_loading = btemp;
		}
		void updateMissionTabs()
		{
			for (int i = 0; i < 3; i++) txtEoM[i].Text = _mission.EndOfMissionMessages[i];
			numMissionTime.Value = _mission.TimeLimitMinutes;
			cboEndEvent.SelectedIndex = _mission.EndEvent;
			numRndSeed.Value = _mission.RndSeed;
			cboMissionLocation.SelectedIndex = _mission.Location;

			//Check if this is a training course.
			lblMissionTimeNote.Visible = false;
			foreach (var fg in _mission.FlightGroups)
			{
				if (fg.IsTrainingPlatform())
				{
					lblMissionTimeNote.Visible = true;
					break;
				}
			}
		}
		#endregion methods

		#region event handlers
		void briefingModifiedCallback(object sender, EventArgs e)
		{
			Common.MarkDirty(this, _loading);
		}

		void colorizedComboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			//X-wing does not use VariableType like the other platforms do
			ComboBox variable = (ComboBox)sender;
			bool colorize = true;

			if (e.Index == -1 || e.Index >= _mission.FlightGroups.Count + 1) colorize = false;  //+1 because of None
																								//else if(e.Index == 0) colorize = false; //The first entry is always "None"

			if (variable.BackColor == Color.Black || variable.BackColor == SystemColors.Window)
				variable.BackColor = (colorize == true) ? Color.Black : SystemColors.Window;

			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			if (colorize == true) brText = getFlightGroupDrawColor(e.Index - 1);
			e.Graphics.DrawString(e.Index >= 0 ? variable.Items[e.Index].ToString() : "", e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}

		void form_Activated(object sender, EventArgs e)
		{
			// Force control refresh for current FG without losing multi-select.
			lstFG_SelectedIndexChanged(0, new EventArgs());
		}
		void form_Deactivate(object sender, EventArgs e)
		{
			// Exit focus from any form controls. This submits changes to the map (if it's open), and can prevent issues if coming back in.
			lstFG.Focus();
		}
		void form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			promptSave();
			if (_config.ConfirmExit && _applicationExit && Text.IndexOf("*") != -1)
			{
				DialogResult res = MessageBox.Show("There are unsaved changes, are you sure you wish to exit?", "Confirm", MessageBoxButtons.YesNo);
				if (res == DialogResult.No) { e.Cancel = true; return; }
			}
			closeForms();
			if (_applicationExit) Application.Exit();
		}
		void form_KeyDown(object sender, KeyEventArgs e)
		{
			//Instead of using a global shortcut for Delete in designer.cs
			//   this.menuDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
			//We trap and process the key here.  This allows the Delete key to remain function when editing other controls like text boxes.
			if (e.KeyCode == Keys.Delete)
			{
				if (lstFG.Focused)
				{
					menuDelete_Click(0, new EventArgs());
					e.Handled = true;
				}
			}
			else if (Common.KeyDown(ActiveControl, e))
			{
				e.Handled = true;
				e.SuppressKeyPress = true; // Stop the Windows UI beeping
			}
		}

		void flightgroupMultiEditHandler(object sender, EventArgs e)
		{
			if (_loading) return;
			MultiEditProperty prop = (MultiEditProperty)((Control)sender).Tag;
			if (prop.Name != "")
			{
				setFlightgroupProperty(prop.RefreshType, prop.Name, Common.GetControlValue(sender));
				Common.MarkDirty(this);  // Since we're not loading, any change marks as dirty.
			}
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.ItemText)) listRefreshSelectedItems();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.CraftName)) updateFGList();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.Map)) refreshMap(-1);
		}

		void opnXW_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_loading = true;
			if (loadMission(opnXW.FileName))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				_activeFG = 0;
				lstFG.SelectedIndex = 0;
			}
			_loading = false;
		}

		void savXW_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			saveMission(savXW.FileName);
		}

		void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (tabMain.SelectedIndex)
			{
				case 0:
					toolNewItem.ToolTipText = "New FlightGroup";
					toolNewItem.Enabled = true;
					toolDeleteItem.ToolTipText = "Delete FlightGroup";
					toolDeleteItem.Enabled = true;
					toolCopy.ToolTipText = "Copy FlightGroup";
					toolPaste.ToolTipText = "Paste FlightGroup";
					break;
				case 1:
					updateMissionTabs();
					break;
				default:
					toolNewItem.Enabled = false;
					toolDeleteItem.Enabled = false;
					toolNewItem.ToolTipText = "";
					toolDeleteItem.ToolTipText = "";
					toolCopy.ToolTipText = "Copy Item";
					toolPaste.ToolTipText = "Paste Item";
					break;
			}
		}

		void toolXwing_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			toolXwing.Focus();
			switch (toolXwing.Buttons.IndexOf(e.Button))
			{
				case 0:     //New Mission
					menuNewXwing_Click("toolbar", new EventArgs());
					_loading = false;
					break;
				case 1:     //Open Mission
					menuOpen_Click("toolbar", new EventArgs());
					_loading = false;
					break;
				case 2:     //Save Mission
					menuSave_Click("toolbar", new EventArgs());
					break;
				case 3:     //Save As
					menuSaveAsXwing_Click("toolbar", new EventArgs());
					break;
				case 5:     //New Item
					if (tabMain.SelectedIndex == 0) newFG(true);
					break;
				case 6:     //Delete Item
					menuDelete_Click("toolbar", new EventArgs());
					break;
				case 7:     //Copy Item
					menuCopy_Click("toolbar", new EventArgs());
					break;
				case 8:     //Paste Item
					menuPaste_Click("toolbar", new EventArgs());
					break;
				case 10:    //Map
					menuMap_Click("toolbat", new EventArgs());
					break;
				case 11:    //Briefing
					menuBriefing_Click("toolbar", new EventArgs());
					break;
				case 12:    //Verify
					menuVerify_Click("toolbar", new EventArgs());
					break;
				case 14:    //Options
					menuOptions_Click("toolbar", new EventArgs());
					break;
				case 15:    //Help
					menuHelpInfo_Click("toolbar", new EventArgs());
					break;
			}
		}
		#endregion event handlers

		#region Menu
		void menuAbout_Click(object sender, EventArgs e)
		{
			new AboutDialog().ShowDialog();
		}
		void menuBriefing_Click(object sender, EventArgs e)
		{
			try { _fBrief.Close(); }
			catch { /* do nothing */ }
			if (_mode == EditorMode.XWI)
				_fBrief = new BriefingFormXwing(_mission.FlightGroupsBriefing, _mission.Briefing, briefingModifiedCallback);
			else
				_fBrief = new BriefingFormXwing(_mission.FlightGroups, _mission.Briefing, briefingModifiedCallback);
			_fBrief.Show();
		}
		//[JB] Added function for menu item and modified for extra safety checks to prevent deleting when the list controls don't have focus.
		void menuDelete_Click(object sender, EventArgs e)
		{
			//Ensure controls have focus, otherwise editing various text controls will trigger deletions.
			if (tabMain.SelectedIndex == 0)
			{
				if ((sender.ToString() == "toolbar") || (lstFG.Focused)) deleteFG();
			}
		}
		void menuCopy_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream = new MemoryStream();
			DataObject data = new DataObject();

			if (sender.ToString() == "AD")
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG]);
				data.SetText(Strings.Trigger[_mission.FlightGroups[_activeFG].ArrivalEvent]);
			}
			else if (sender.ToString() == "Order")
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG]);
				data.SetText(Strings.Orders[_mission.FlightGroups[_activeFG].Order]);
			}
			else if (ActiveControl.GetType().ToString() == "System.Windows.Forms.TextBox")
			{
				TextBox txt = (TextBox)ActiveControl;
				if (txt.SelectedText != "")
				{
					formatter.Serialize(stream, txt.SelectedText);
					data.SetText(txt.SelectedText);
				}
			}
			else if (ActiveControl.GetType().ToString() == "System.Windows.Forms.NumericUpDown")  //[JB] Added copy/paste for this
			{
				NumericUpDown num = (NumericUpDown)ActiveControl;
				formatter.Serialize(stream, num.Value);
				data.SetText(num.Value.ToString());
			}
			else if (ActiveControl.GetType().ToString() == "System.Windows.Forms.DataGridTextBox")
			{
				DataGridTextBox dgt = (DataGridTextBox)ActiveControl;
				if (dgt.Text != "")
				{
					formatter.Serialize(stream, dgt.Text);
					data.SetText(dgt.Text);
				}
			}
			switch (tabMain.SelectedIndex)
			{
				case 0:
					formatter.Serialize(stream, _mission.FlightGroups[_activeFG]);
					data.SetText(_mission.FlightGroups[_activeFG].ToString());
					break;
			}
			data.SetData("yogeme", false, stream);
			Clipboard.SetDataObject(data, true);
		}
		void menuCut_Click(object sender, EventArgs e)
		{
			if(Common.Cut(ActiveControl))
				Common.MarkDirty(this);
		}
		void menuER_Click(object sender, EventArgs e)
		{
			Common.LaunchER();
		}
		void menuExit_Click(object sender, EventArgs e)
		{
			Close();
		}
		void menuGoalSummary_Click(object sender, EventArgs e)
		{
			string dsGoal = "";
			if (_mission.Location == 1) //Death Star surface
			{
				if (_mission.EndEvent == 1) dsGoal = "Death Star Surface: Clear Laser Towers";
				else if (_mission.EndEvent == 5) dsGoal = "Death Star Surface: Hit Exhaust Port";
				if (dsGoal.Length > 0) dsGoal += "\r\n\r\n";
			}
			new GoalSummaryDialog(dsGoal + generateGoalSummary()).Show();
		}
		void menuLibrary_Click(object sender, EventArgs e)
		{
			_fLibrary?.Close();
			_fLibrary = new FlightGroupLibraryForm(Settings.Platform.XWING, _mission.FlightGroups, flightGroupLibraryCallback);
		}
		void flightGroupLibraryCallback(object sender, EventArgs e)
		{
			foreach (object obj in (object[])sender)
			{
				FlightGroup fg = (FlightGroup)obj;
				if (fg == null || !newFG(fg.IsFlightGroup())) break;

				_mission.FlightGroups[_activeFG] = fg;
				updateFGList();
				listRefreshItem(_activeFG);
				_startingShips--;
				craftStart(fg, true);
			}
		}
		void menuHelpInfo_Click(object sender, EventArgs e)
		{
			Common.LaunchHelp();
		}
		void menuIDMR_Click(object sender, EventArgs e)
		{
			Common.LaunchGithub();
		}
		void menuMap_Click(object sender, EventArgs e)
		{
			try { _fMap.Close(); }
			catch { /* do nothing */ }
			_fMap = new MapForm(_mission.FlightGroups, mapForm_DataChangedCallback);
			_fMap.Show();
		}
		void mapForm_DataChangedCallback(object sender, EventArgs e)
		{
			Common.MarkDirty(this);
			if (tabFGMinor.SelectedIndex == 3)  //Update waypoints tab.
				refreshWaypointTab();
		}
		void menuNewXwing_Click(object sender, EventArgs e)
		{
			switchTo(EditorMode.XWI);
			promptSave();
			closeForms();
			_loading = true;
			initializeMission();
			lstFG.SelectedIndex = 0;
			_startingShips = 1;
			lblStarting.Text = "1 Craft at 30 seconds";
			updateMissionTabs();
			_loading = false;
			if (Text.EndsWith("*")) Text = Text.Substring(0, Text.Length - 1);
		}
		void menuNewBoP_Click(object sender, EventArgs e)
		{
			menuNewXvT_Click("BoP", new EventArgs());
		}
		void menuNewTIE_Click(object sender, EventArgs e)
		{
			promptSave();
			closeForms();
			_applicationExit = false;
			new TieForm().Show();
			Close();
		}
		void menuNewXvT_Click(object sender, EventArgs e)
		{
			promptSave();
			closeForms();
			_applicationExit = false;
			new XvtForm(sender.ToString() == "BoP").Show();
			Close();
		}
		void menuNewXWA_Click(object sender, EventArgs e)
		{
			promptSave();
			closeForms();
			_applicationExit = false;
			new XwaForm().Show();
			Close();
		}
		void menuOpen_Click(object sender, EventArgs e)
		{
			promptSave();
			opnXW.FileName = _mission.MissionFileName;
            if (_mission.MissionFileName != "NewMission.xwi") opnXW.InitialDirectory = Directory.GetParent(_mission.MissionPath).FullName; // follow current mission, fixes when switching platforms it wouldn't follow
            if (opnXW.ShowDialog() == DialogResult.OK)
			{
				opnXW_FileOk(0, new System.ComponentModel.CancelEventArgs());
				_config.SetWorkingPath(Path.GetDirectoryName(opnXW.FileName));
				opnXW.InitialDirectory = _config.GetWorkingPath(); //Update since folder may have changed
			}
		}
		void menuOptions_Click(object sender, EventArgs e)
		{
			new OptionsDialog(null).ShowDialog();  //X-wing doesn't use interact labels, no callback event
		}
		void menuPaste_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			if (!(Clipboard.GetDataObject() is DataObject data)) return;

			object obj;
			if (data.GetData("yogeme", false) is MemoryStream stream)
			{
				obj = formatter.Deserialize(stream);
				stream.Close();
			}
			else obj = data.GetData("Text");
			if (obj == null) return;

			if (sender.ToString() == "AD")
			{
				try
				{
					FlightGroup fg = (FlightGroup)obj ?? throw new FormatException();

					foreach (FlightGroup cur in getSelectedFlightgroups())
					{
						craftStart(cur, false);
						cur.ArriveViaHyperspace = fg.ArriveViaHyperspace;
						cur.DepartViaHyperspace = fg.DepartViaHyperspace;
						cur.Mothership = fg.Mothership;
						if (cur.Mothership >= _mission.FlightGroups.Count) cur.Mothership = -1;
						cur.ArrivalFG = fg.ArrivalFG;
						if (cur.ArrivalFG >= _mission.FlightGroups.Count) cur.ArrivalFG = -1;
						cur.ArrivalEvent = fg.ArrivalEvent;
						cur.ArrivalDelay = fg.ArrivalDelay;
						craftStart(cur, true);
					}
					lstFG_SelectedIndexChanged(0, new EventArgs());
					listRefreshSelectedItems();
					Common.MarkDirty(this);
				}
				catch { /* do nothing */ }
			}
			else if (sender.ToString() == "Order")
			{
				try
				{
					FlightGroup fg = (FlightGroup)obj ?? throw new FormatException();

					foreach (FlightGroup cur in getSelectedFlightgroups())
					{
						cur.Order = fg.Order;
						cur.TargetPrimary = fg.TargetPrimary;
						if (cur.TargetPrimary >= _mission.FlightGroups.Count) cur.TargetPrimary = -1;
						cur.TargetSecondary = fg.TargetSecondary;
						if (cur.TargetSecondary >= _mission.FlightGroups.Count) 	cur.TargetSecondary = -1;
						cur.DockTimeThrottle = fg.DockTimeThrottle;
					}
					lstFG_SelectedIndexChanged(0, new EventArgs());
					listRefreshSelectedItems();
					Common.MarkDirty(this);
				}
				catch { /* do nothing */ }
			}
			else if (Common.Paste(ActiveControl, obj)) Common.MarkDirty(this);
			else if (ActiveControl.GetType() == typeof(DataGridTextBox))
			{
				try
				{
					string str = obj.ToString();
					if (str.IndexOf("Idmr.", 0) != -1) throw new FormatException();

					DataGrid dg = (DataGrid)ActiveControl.Parent;
					int row = dg.CurrentRowIndex;
					DataTable dt = ((DataView)dg.DataSource).Table;
					dt.Rows[row][dg.CurrentCell.ColumnNumber] = str;
					if (dt.TableName == "Waypoints") table_RowChanged("paste", new DataRowChangeEventArgs(dt.Rows[row], DataRowAction.Change));
					else tableRaw_RowChanged("paste", new DataRowChangeEventArgs(dt.Rows[row], DataRowAction.Change));
				}
				catch { /* do nothing */ }
			}
			else
			{
				switch (tabMain.SelectedIndex)
				{
					case 0:
						try
						{
							FlightGroup fg = (FlightGroup)obj ?? throw new FormatException();

							if (_mode == EditorMode.BRF)  //Can't validate anything if pasting into BRF, so reset indexes.
							{
								fg.Mothership = -1;
								fg.ArrivalFG = -1;
								fg.ArrivalEvent = 0;
								fg.TargetPrimary = -1;
								fg.TargetSecondary = -1;
							}
							newFG(fg.IsFlightGroup());
							_mission.FlightGroups[_activeFG] = fg;
							refreshMap(-1);
							updateFGList(); //[JB] Update all the downdown lists.
							listRefreshItem(_activeFG);
							_startingShips--;
							lstFG.SelectedIndex = _activeFG;
							lstFG_SelectedIndexChanged(0, new EventArgs()); //[JB] Need to force refresh of form controls.
							craftStart(fg, true);
							lstFG.Focus();  //[JB] Return focus to list.  Lets the user delete the pasted FG without having to manually select it again.
						}
						catch { /* do nothing */ }
						break;
				}
			}
		}
		void menuRecentMissions_Click(object sender, EventArgs e)
		{
			switchTo(EditorMode.XWI);
			string mission = _config.RecentMissions[(int)((MenuItem)sender).Tag];
			promptSave();
			initializeMission();
			if (loadMission(mission))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				_activeFG = 0;
				lstFG.SelectedIndex = 0;
			}
			_config.SetWorkingPath(Path.GetDirectoryName(mission)); //[JB] Update last-accessed
			opnXW.InitialDirectory = _config.GetWorkingPath();
			savXW.InitialDirectory = _config.GetWorkingPath();
			_loading = false;
		}
		void menuSave_Click(object sender, EventArgs e)
		{
			switchTo(EditorMode.XWI);
			if (_mission.MissionPath == "\\NewMission.xwi") savXW.ShowDialog();
			else saveMission(_mission.MissionPath);
		}
		void menuSaveAsTIE_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsTIE", new EventArgs());
			try
			{
				Platform.Tie.Mission converted = Platform.Converter.XwingToTie(_mission);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXvT_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsXvT", new EventArgs());
			try
			{
				Platform.Xvt.Mission converted = Platform.Converter.XwingToXvtBop(_mission, false);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsBoP_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsBoP", new EventArgs());
			try
			{
				Platform.Xvt.Mission converted = Platform.Converter.XwingToXvtBop(_mission, true);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXWA_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsXWA", new EventArgs());
			try
			{
				Platform.Xwa.Mission converted = Platform.Converter.XwingToXwa(_mission);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXwing_Click(object sender, EventArgs e)
		{
			switchTo(EditorMode.XWI);
			Common.MarkDirty(this); // this is to avoid the "unmodified" cancel
			savXW.ShowDialog();
		}
		void menuTour_Click(object sender, EventArgs e)
		{
			_fTour = new TourForm();
			try { _fTour.Show(); }
			catch (ObjectDisposedException) { _fTour = null; }
		}
		void menuVerify_Click(object sender, EventArgs e)
		{
			switchTo(EditorMode.XWI);
			MessageBox.Show("This feature is not supported for X-wing.", "Notice");
		}
		void menuTest_Click(object sender, EventArgs e)
		{
			switchTo(EditorMode.XWI);
			MessageBox.Show("This feature is not supported for X-wing.", "Notice");
		}

		#endregion
		#region FlightGroups
		/// <summary>Counts all trigger and parameter references of a FG</summary>
		/// <param name="fgIndex">Index within _mission.FlightGroups</param>
		/// <remarks>Used to populate the counters in the confirm deletion dialog</remarks>
		int[] countFlightGroupReferences(int fgIndex)
		{
			int[] count = new int[7];
			const int cMothership = 1, cArrDep = 2, cOrder = 3, cBrief = 6;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (fgIndex == i)
					continue;

				FlightGroup fg = _mission.FlightGroups[i];
				if (fg.Mothership == fgIndex) count[cMothership]++;
				if (fg.ArrivalFG == fgIndex) count[cArrDep]++;
				if (fg.TargetPrimary == fgIndex) count[cOrder]++;
				if (fg.TargetSecondary == fgIndex) count[cOrder]++;
			}

			if (_mode == EditorMode.BRF)
			{
				Briefing br = _mission.Briefing;
				for (int page = 0; page < br.Pages.Count; page++)
				{
					BriefingPage pg = br.GetBriefingPage(page);
					for (int p = 0; p < pg.Events.Count; p++)
					{
						if (pg.Events[p].IsEndEvent) break;

						if (pg.Events[p].IsFGTag && pg.Events[p].Variables[0] == fgIndex) count[cBrief]++;
					}
				}
			}
			for (int i = 1; i < 7; i++)
				count[0] += count[i];
			return count;
		}
		/// <summary>Get the FG or Object index</summary>
		/// <param name="fg">If <i>true</i> count FlightGroups, otherwise Objects</param>
		/// <param name="selected">If <i>true</i> return selected item's index, otherwise total count</param>
		int countGroups(bool fg, bool selected)
		{
			int fgCount = -1;
			int objCount = -1;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_mission.FlightGroups[i].ObjectType > 0)
					objCount++;
				else
					fgCount++;

				if (i == _activeFG && selected)
				{
					if (!fg && _mission.FlightGroups[i].ObjectType > 0)
						return objCount;
					else
						return fgCount;
				}
			}
			return (fg ? fgCount : objCount);
		}
		void deleteFG()
		{
			_fBrief?.Close();   //Close (which also saves) the briefing before accessing it.  Don't call save directly since this may cause FG index corruption if multiple FGs are deleted.

			restrictSelection();
			List<int> selection = Common.GetSelectedIndices(lstFG);
			int startFG = _activeFG;
			for (int si = selection.Count - 1; si >= 0; si--)  // Delete from end so prior indices remain intact.
			{
				int _activeFG = selection[si];

				if (_config.ConfirmFGDelete)
				{
					int[] count = countFlightGroupReferences(_activeFG);
					if (count[0] > 0)
					{
						string[] Reasons = new string[7] { "", "Mothership reference", "Arrival/Departure trigger", "Order target reference", "Global Goal trigger", "Message trigger", "Briefing FG Tag" };
						string breakdown = "";
						for (int i = 1; i < 7; i++)
							if (count[i] > 0) breakdown += "    " + count[i] + " " + Reasons[i] + ((count[i] > 1) ? "s" : "") + "\n";

						string s = "This Flight Group is referenced " + count[0] + " time" + ((count[0] > 1) ? "s" : "") + " in these cases:\n" + breakdown + "\nAll references targeting this flight group will be reset to default.";
						if (count[6] > 0) s += "\nAssociated Briefing FG Tag events will be deleted.";
						s += "\n\nAre you sure you want to delete this Flight Group?";
						DialogResult res = MessageBox.Show(s, "WARNING: Confirm Delete", MessageBoxButtons.YesNo);
						if (res == DialogResult.No) break;  // exit the outer for() loop
					}
				}

				replaceClipboardFGReference(_activeFG, -1);
				if (_mode == EditorMode.BRF) _mission.Briefing.TransformFGReferences(_activeFG, -1);

				if (_mission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(_activeFG);
				craftStart(_mission.FlightGroups[_activeFG], false);
				if (_mission.FlightGroups.Count == 1)
				{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
					_activeFG = _mission.FlightGroups.RemoveAt(_activeFG);  // Still need to delete to clear the indexes.  The delete function always ensures that Count==1, so it has to be inside this block, not before.
					_mission.FlightGroups.Clear();
					_activeFG = 0;
					_mission.FlightGroups[0].CraftType = 1;
					_mission.FlightGroups[0].ObjectType = 0;
					_mission.FlightGroups[0].IFF = 1;
					craftStart(_mission.FlightGroups[0], true);
					break;
				}
				else _activeFG = _mission.FlightGroups.RemoveAt(_activeFG);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
			}
			// Fix bounds and make new selection.
			if (startFG >= _mission.FlightGroups.Count) startFG = _mission.FlightGroups.Count - 1;
			lstFG.SelectedIndex = startFG;

			updateFGList();
			lstFG.SelectedIndex = _activeFG;
			refreshMap(-1);
			try
			{
				if (_mode == EditorMode.BRF)
				{
					_fBrief.Import(_mission.FlightGroups);
					_fBrief.MapPaint();
				}
			}
			catch { /* do nothing */ }

			Common.MarkDirty(this);
			updateMissionTabs();
			lstFG_SelectedIndexChanged(0, new EventArgs());
			if (!lstFG.Focused) lstFG.Focus();
		}
		string generateGoalSummary()
		{
			//4 elements:  Primary,Secondary,Secret,Bonus
			//Each element contains a list of strings for each goal.
			List<string> goalList = new List<string>();

			//Iterate FGs and their goals, adding them to the proper list
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				FlightGroup fg = _mission.FlightGroups[i];
				if (fg.Objective == 0)
					continue;

				string c = Strings.CraftAbbrv[fg.CraftType] + " " + fg.Name;
				c += " " + Strings.Objective[fg.Objective];

				goalList.Add(c);
			}

			//Compose the output by going through the goal categories
			string output = "";
			foreach (string s in goalList)
				output += s + "\r\n";

			if (output == "") output = "There are no Flight Group goals.";
			output += "\r\n";

			return output;
		}
		string[] getNullableFGList()
		{
			int count = 0;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_mission.FlightGroups[i].IsFlightGroup())
					count++;
			}
			string[] retval = new string[count + 1];
			retval[0] = "None";
			count = 1;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_mission.FlightGroups[i].IsFlightGroup())
					retval[count++] = _mission.FlightGroups[i].ToString(false);
			}
			return retval;
		}
		List<FlightGroup> getSelectedFlightgroups()
		{
			List<FlightGroup> fgs = new List<FlightGroup>();
			if (lstFG.SelectedIndices.Count > 0)
			{
				bool firstType = _mission.FlightGroups[lstFG.SelectedIndex].IsFlightGroup();
				foreach (int fgIndex in lstFG.SelectedIndices)
					if (_mode == EditorMode.BRF || _mission.FlightGroups[fgIndex].IsFlightGroup() == firstType)
						fgs.Add(_mission.FlightGroups[fgIndex]);
			}
			return fgs;
		}
		void setFlightgroupProperty(MultiEditRefreshType refreshType, string name, object value)
		{
			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				if (refreshType.HasFlag(MultiEditRefreshType.CraftCount)) craftStart(fg, false);

				switch (name)
				{
					case "Name": fg.Name = (string)value; break;
					case "NumberOfCraft":
						fg.NumberOfCraft = Convert.ToByte(value);
						if (fg.SpecialCargoCraft > fg.NumberOfCraft) fg.SpecialCargoCraft = 0;
						break;
					case "NumberOfWaves": fg.NumberOfWaves = Convert.ToByte((int)value - 1); break;
					// "GlobalGroup" has special logic, not handled here.
					case "Cargo": fg.Cargo = (string)value; break;
					case "SpecialCargo": fg.SpecialCargo = (string)value; break;
					case "SpecialCargoCraft":
						fg.SpecialCargoCraft = Convert.ToInt16((int)value - 1);
						if (fg.SpecialCargoCraft < 0 || fg.SpecialCargoCraft >= fg.NumberOfCraft) fg.SpecialCargoCraft = -1;
						break;
					// Craft and object types have special logic, not handled here.
					case "IFF": fg.IFF = Convert.ToByte(value); break;
					case "AI": fg.AI = Convert.ToByte(value); break;
					case "Markings": fg.Markings = Convert.ToByte(value); break;
					case "PlayerCraft": fg.PlayerCraft = Convert.ToByte(value); break;
					case "Formation":
						fg.Formation = Convert.ToByte(value);
						if (fg.ObjectType > 0) fg.Formation += (byte)(cboPrimGoalT.SelectedIndex * 4);
						break;
					case "PlatformValue": fg.Formation = Convert.ToByte(value); break;    // Multipurpose property
					case "PlatformSeconds": fg.Formation = Convert.ToByte(value); break;
					case "Status": fg.Status1 = Convert.ToByte((fg.Status1 >= 10 ? 10 : 0) + (int)value); break;  // B-wing repeats codes at status 10 and higher
					case "ArriveViaHyper": fg.ArriveViaHyperspace = Convert.ToBoolean(value); break;
					case "DepartViaHyper": fg.DepartViaHyperspace = Convert.ToBoolean(value); break;
					case "Mothership": fg.Mothership = Convert.ToInt16(translateNullableFG((int)value)); break;
					case "ArrivalTrigFlightgroup": fg.ArrivalFG = Convert.ToInt16(translateNullableFG((int)value)); break;
					case "ArrivalTrigCondition": fg.ArrivalEvent = Convert.ToInt16(value); break;
					case "ArrivalDelayMin": fg.ArrivalDelay = Convert.ToInt16(fg.CreateArrivalDelay((int)value, fg.GetArrivalSeconds())); break;
					case "ArrivalDelaySec": fg.ArrivalDelay = Convert.ToInt16(fg.CreateArrivalDelay(fg.GetArrivalMinutes(), (int)value / 6 * 6)); break;
					case "Goal":
						fg.Objective = Convert.ToInt16(value);
						if (fg.ObjectType > 0) fg.Formation =  Convert.ToByte(((int)value * 4) + (cboFormation.SelectedIndex % 4));
						break;
					case "Pitch": fg.Pitch = (short)Math.Round((double)(((int)value >= 270) ? (int)value - 270 : (int)value + 90) / 360 * 256); break;
					case "Yaw": fg.Yaw = (short)Math.Round((double)(int)value / 360 * 256); break;
					case "Roll": fg.Roll = (short)Math.Round((double)(int)value / 360 * 256); break;
					case "OrderCommand": fg.Order = Convert.ToInt16(value); break;
					case "OrderPrimary": fg.TargetPrimary = Convert.ToInt16(translateNullableFG((int)value)); break;
					case "OrderSecondary": fg.TargetSecondary = Convert.ToInt16(translateNullableFG((int)value)); break;
					case "OrderValue": fg.DockTimeThrottle = Convert.ToInt16(value); break;
					default: throw new ArgumentException("Unhandled multi-edit property: " + name);
				}
				if(refreshType.HasFlag(MultiEditRefreshType.CraftCount)) craftStart(fg, true);
			}
		}
		/// <summary>Determines if selected objects can be moved within their respective FlightGroup or ObjectGroup sections. Also refreshes the Move buttons.</summary>
		bool checkMove(int direction)
		{
			restrictSelection();
			ListBox.SelectedIndexCollection sel = lstFG.SelectedIndices;
			bool up = false;
			bool down = false;
			if (sel.Count > 0)
			{
				int min, max;
				if (_mode == EditorMode.XWI)
				{
					bool fgType = _mission.FlightGroups[sel[0]].IsFlightGroup();
					min = fgType ? _mission.FlightGroups.GetFirstOfFlightGroup() : _mission.FlightGroups.GetFirstOfObjectGroup();
					max = fgType ? _mission.FlightGroups.GetLastOfFlightGroup() : _mission.FlightGroups.GetLastOfObjectGroup();
				}
				else
				{
					min = 0;
					max = _mission.FlightGroups.Count - 1;
				}
				up = min != -1 && sel[0] > min;
				down = max != -1 && sel[sel.Count - 1] < max;
			}
			cmdMoveUp.Enabled = up;
			cmdMoveDown.Enabled = down;
			return ((direction == -1 && up) || (direction == 1 && down));
		}
		/// <summary>Changes the FlightGroup or ObjectGroup craft type, and allows switching between those categories.</summary>
		/// <remarks>The types are expected to be a ComboBox SelectedIndex for the respective category being changed, or -1 for the category not being changed.</remarks>
		/// <returns>The list must remain sorted.  If changing category between FlightGroup and ObjectGroup, returns the new destination list index.  If not changing, returns -1.</returns>
		int changeType(FlightGroup fg, int craftType, int objectType)
		{
			if (objectType >= 0) objectType += 17;  // Expand from cbo SelectedIndex into proper item.
			bool isFlightgroup = fg.IsFlightGroup();
			// If changing from B-Wing to Y-Wing (both are craftType 2) we still need to apply the change below.
			if ((isFlightgroup && craftType == fg.CraftType && craftType != 2) || (!isFlightgroup && objectType == fg.ObjectType))
				return -1;
			// Don't change if "none" is selected for the opposite type.
			if (isFlightgroup && objectType == 17 || !isFlightgroup && craftType == 0)
				return -1;
			int ret = -1;
			bool isSwap = ((isFlightgroup && objectType >= 0) || (!isFlightgroup && craftType >= 0));
			// Have to poll these before changing the type, otherwise it interferes with search
			int lastFlightgroup = _mission.FlightGroups.GetLastOfFlightGroup();
			int lastObjectgroup = _mission.FlightGroups.GetLastOfObjectGroup();
			int oldCraftType = fg.CraftType;
			int oldObjectType = fg.ObjectType;
			int oldStatus = fg.Status1;
			if (_mode == EditorMode.XWI)
			{
				if (craftType >= 0)
				{
					if (craftType == 18) // B-Wing selected in Craft List
					{
						fg.CraftType = 2;  // Replace with Y-W
						fg.Status1 = (byte)((fg.Status1 % 10) + 10);
					}
					else
					{
						if (fg.Status1 >= 10)
							fg.Status1 = (byte)(fg.Status1 % 10);
                        fg.CraftType = (byte)craftType;
                    }
					fg.ObjectType = 0;
					if (isSwap)
					{
						// Swapping from Object to Craft, so reset these fields since objects often use these values for other things which can easily exceed the expected values for ordinary craft FGs.
						fg.Formation = 0;
						fg.NumberOfCraft = 1;
						ret = lastFlightgroup + 1;  // Whether the last index is -1 or valid, this resolves correctly.
					}
				}
				else
				{
					fg.CraftType = 0;
					fg.ObjectType = (short)objectType;
					if (isSwap)
					{
						// Swapping from Craft to Object
						ret = lastObjectgroup;
						if (ret < 0)  // May not be any objects
							ret = lastFlightgroup;
					}
				}
			}
			else
			{
				if (craftType >= 0)
				{
					if (craftType == 18)  // If selecting B-wing, change to B-wing Icon instead.
					{
						fg.CraftType = 0;
						fg.ObjectType = 25;
					}
					else
					{
						fg.CraftType = (byte)craftType;
						fg.ObjectType = 0;
					}
				}
				else
				{
					fg.CraftType = 0;
					fg.ObjectType = (short)objectType;
				}
			}

			if(fg.CraftType != oldCraftType || fg.ObjectType != oldObjectType || fg.Status1 != oldStatus)
				Common.MarkDirty(this);

			return ret;
		}
		/// <summary>Moves a flightgroup slot to another, continuously swapping until it reaches its destination.</summary>
		void moveFlightgroupToSlot(int sourceIndex, int destIndex)
		{
			if (destIndex == -1 || sourceIndex == destIndex)
				return;
			if (sourceIndex > destIndex)
			{
				for (int i = sourceIndex; i > destIndex; i--)
					_mission.FlightGroups.Swap(i, i - 1);
			}
			else if (sourceIndex < destIndex)
			{
				for (int i = sourceIndex; i < destIndex; i++)
					_mission.FlightGroups.Swap(i, i + 1);
			}
		}
		/// <summary>Retrieves the list index of a FlightGroup</summary>
		/// <remarks>If the craft or object type changes, the list is resorted and prior indices become meaningless.</remarks>
		int getFlightgroupIndexOf(FlightGroup fg)
		{
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
				if (_mission.FlightGroups[i] == fg)
					return i;
			return -1;
		}
		void changeSelectedFlightgroups(int craftType, int objectType)
		{
			List<FlightGroup> fgList = getSelectedFlightgroups();
			if (fgList.Count < 0) return;
			bool fullRefresh = false;
			foreach (FlightGroup fg in fgList)
			{
				craftStart(fg, false);
				bool isFlightgroup = fg.IsFlightGroup();
				int sourceIndex = getFlightgroupIndexOf(fg);
				int destIndex = changeType(fg, craftType, objectType);
				if (destIndex != -1)
				{
					// If craft changes to object, delete references.
					if(isFlightgroup && sourceIndex != destIndex)
						replaceClipboardFGReference(sourceIndex, -1);
					moveFlightgroupToSlot(sourceIndex, destIndex);
					fullRefresh = true;
				}
				craftStart(fg, true);
			}
			// Reselect the flightgroups, which may have been moved different slots.
			_noRefresh = true;
			lstFG.ClearSelected();
			foreach (FlightGroup fg in fgList)
				lstFG.SetSelected(getFlightgroupIndexOf(fg), true);
			_noRefresh = false;
			lstFG_SelectedIndexChanged(0, new EventArgs());
			
			// Refresh the list names and craft numbering.
			updateFGList();
			if (fullRefresh)
				listRefreshAll();
			else
				listRefreshSelectedItems();
		}
		void moveFlightgroups(int direction)
		{
			if (!checkMove(direction)) return;

			List<int> selection = Common.GetSelectedIndices(lstFG);
			for (int i = 0; i < selection.Count; i++)
			{
				// Traverse the selection list forward if moving up, backward if moving down.
				int accessIndex = ((direction == -1) ? i : selection.Count - 1 - i);
				int fgIndex = selection[accessIndex];
				replaceClipboardFGReference(fgIndex, fgIndex + direction);
				_mission.FlightGroups.Swap(fgIndex, fgIndex + direction);
				if (_mode == EditorMode.BRF)
				{
					_mission.Briefing.TransformFGReferences(fgIndex + direction, 32767);
					_mission.Briefing.TransformFGReferences(fgIndex, fgIndex + direction);
					_mission.Briefing.TransformFGReferences(32767, fgIndex);
				}
				listRefreshItem(fgIndex + direction, false);
				listRefreshItem(fgIndex, false);
				selection[accessIndex] += direction;  // Adjust indices to new positions
			}
			Common.SetSelectedIndices(lstFG, selection, ref _noRefresh);  // Apply adjusted indices

			_fBrief?.Close();
			refreshMap(-1);
			updateFGList();
			Common.MarkDirty(this);
			checkMove(0); // Refresh buttons
		}

		void listRefreshItem(int index, bool mapUpdate = true)
		{
			bool btemp = _noRefresh;
			_noRefresh = true;                      // Modifying an item will invoke SelectedIndexChanged.
			bool state = lstFG.GetSelected(index);  // It may also interfere with the current selection state.
			lstFG.Items[index] = ((_mode == EditorMode.XWI) ? "" : "[BRF] ") + _mission.FlightGroups[index].ToString(true);
			lstFG.SetSelected(index, state);
			if (!lstFG.IsDisposed) lstFG.Invalidate(lstFG.GetItemRectangle(index));
			if (_fMap != null && mapUpdate) _fMap.UpdateFlightGroup(index, _mission.FlightGroups[index]);
			_noRefresh = btemp;
		}
		void listRefreshSelectedItems()
		{
			foreach (int i in Common.GetSelectedIndices(lstFG))
				listRefreshItem(i);
		}
		void listRefreshAll()
		{
			for (int i = 0; i < lstFG.Items.Count; i++)
				listRefreshItem(i);
		}
		bool newFG(bool isCraft)  //[JB] We have to explicitly determine whether the new FG should be created in the craft section or object section.
		{
			if (_mission.FlightGroups.Count == Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			int slot = isCraft ? _mission.FlightGroups.GetLastOfFlightGroup() + 1 : _mission.FlightGroups.Count;
			_activeFG = _mission.FlightGroups.Insert(slot);
			_mission.FlightGroups[_activeFG].CraftType = _config.XwingCraft;
			_mission.FlightGroups[_activeFG].ObjectType = 0;
			_mission.FlightGroups[_activeFG].IFF = _config.XwingIff;
			craftStart(_mission.FlightGroups[_activeFG], true);
			lstFG.Items.Insert(slot, _mission.FlightGroups[_activeFG].ToString(true));
			updateFGList();
			listRefreshAll(); //[JB] Since Flight Groups are inserted before Object Groups, need to refresh the entire list.
			_activeFG = slot; //listRefreshAll() seems to change _activeFG somehow, so reset to proper index.
			lstFG.ClearSelected();
			lstFG.SelectedIndex = _activeFG;
			Common.MarkDirty(this, _loading);
			refreshMap(-1);
			try
			{
				_fBrief.Import(_mission.FlightGroups);
				_fBrief.MapPaint();
			}
			catch { /* do nothing */ }
			return true;
		}
		/// <summary>Scans all Flight Groups to detect duplicate names, to provide helpful craft numbering within the editor so that the user can easily tell duplicates apart in triggers.</summary>
		void recalculateEditorCraftNumbering()
		{
			// Note: changing an item in lstFG will activate lstFG_SelectedIndexChanged, which changes _activeFG and potentially cause bugs elsewhere. So need to restore before exiting the function.
			int currentFG = _activeFG;
			//A-W Red and X-W Red should not be considered duplicates, so this structure maps a CraftType to a sub-dictionary of CraftName and Count.  
			//Due to the complexity and careful error checking involved (throwing exceptions is incredibly slow), two separate functions are provided to manipulate and access them.
			Dictionary<int, Dictionary<string, int>> dupeCount = new Dictionary<int, Dictionary<string, int>>();
			Dictionary<int, Dictionary<string, int>> nameCount = new Dictionary<int, Dictionary<string, int>>();

			foreach (var fg in _mission.FlightGroups)     //Need to know the duplicates ahead of time, so that even the first craft encountered can be numbered accordingly.
			{
				int iff = fg.GetActualIFF();
				Common.AddFGCounter(fg.CraftType, iff, fg.Name, 1, dupeCount);
			}

			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				FlightGroup fg = _mission.FlightGroups[i];
				int iff = fg.GetActualIFF();
				bool isDupe = (Common.GetFGCounter(fg.CraftType, iff, fg.Name, dupeCount) >= 2);

				int count = Common.AddFGCounter(fg.CraftType, iff, fg.Name, 1, nameCount);
				if (!isDupe)
					count = 0;

				if (fg.EditorCraftNumber != count)
				{
					fg.EditorCraftNumber = count;
					fg.EditorCraftExplicit = false;  //X-wing does not have Flight Group numbering beyond single waves.
					listRefreshItem(i);
				}
			}
			_activeFG = currentFG;
		}
		/// <summary>Updates the clipboard contents from containing broken indexes.</summary>
		/// <remarks>Should be called during swap or delete (dstIndex < 0) operations.</remarks>
		void replaceClipboardFGReference(int srcIndex, int dstIndex)
		{
			//[JB] Replace any clipboard references.  Load it, check/modify type, save back to stream.
			try
			{
				System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				if (!(Clipboard.GetDataObject() is DataObject data) || !data.GetDataPresent("yogeme", false)) return;
				if (!(data.GetData("yogeme", false) is MemoryStream stream)) return;

				object raw = formatter.Deserialize(stream);
				stream.Close();

				data = new DataObject();
				bool change = false;
				if (raw.GetType() == typeof(FlightGroup))
				{
					//[JB] So I realized the code that manages references in X-wing is a gigantic hacky mess.  It was my original code before I cleaned it up for TIE Fighter and the rest.  Unfortunately X-wing is still a mess.  And since modifying any of the existing functions would break functionality elsewhere (due to how objects are treated separately, but also XWI vs BRF FGs in separate collections), I'm going to edit the variables manually.  Luckily there aren't many.
					FlightGroup fg = (FlightGroup)raw;
					short dst = (short)dstIndex;
					if (dstIndex >= 0)
					{
						change |= fg.ReplaceFGReference(dstIndex, -2);
						change |= fg.ReplaceFGReference(srcIndex, dstIndex);
						change |= fg.ReplaceFGReference(-2, srcIndex);
					}
					else
					{
						if (fg.ArrivalFG == srcIndex) { fg.ArrivalFG = dst; fg.ArrivalEvent = 0; change = true; } else if (fg.ArrivalFG > srcIndex && dstIndex == -1) { fg.ArrivalFG--; change = true; }
						if (fg.Mothership == srcIndex) { fg.Mothership = dst; fg.ArriveViaHyperspace = true; change = true; } else if (fg.Mothership > srcIndex && dstIndex == -1) { fg.Mothership--; change = true; }
						if (fg.TargetPrimary == srcIndex) { fg.TargetPrimary = dst; change = true; } else if (fg.TargetPrimary > srcIndex && dstIndex == -1) { fg.TargetPrimary--; change = true; }
						if (fg.TargetSecondary == srcIndex) { fg.TargetSecondary = dst; change = true; } else if (fg.TargetSecondary > srcIndex && dstIndex == -1) { fg.TargetSecondary--; change = true; }
					}
					data.SetText(fg.ToString());
				}
				if (change)
				{
					stream = new MemoryStream();
					formatter.Serialize(stream, raw);
					data.SetData("yogeme", false, stream);
					Clipboard.SetDataObject(data, true);
				}
			}
			catch { /* do nothing*/ }
		}
		int translateNullableFG(int value)
		{
			return (value >= 1) ? value - 1 : -1;
		}
		/// <summary>Don't allow mixing FlightGroups and ObjectGroups in the current selection.</summary>
		void restrictSelection()
		{
			if (_mode == EditorMode.BRF || lstFG.SelectedIndex == -1) return;  // Order doesn't matter in briefing groups.
			bool firstType = _mission.FlightGroups[lstFG.SelectedIndex].IsFlightGroup();
			bool btemp = _noRefresh;
			_noRefresh = true;
			foreach (int index in Common.GetSelectedIndices(lstFG))
			{
				if (_mission.FlightGroups[index].IsFlightGroup() != firstType)
					lstFG.SetSelected(index, false);
			}
			_noRefresh = btemp;
		}
		void updateFGList()
		{
			//[JB] Adding this here since it's a convenient way of updating the craft numbering in any situation it may be needed.  Otherwise it would need to be added to a function and called on every major FG option (move, add, delete, rename).  Since this potentially changes multiple FG names, it needs to be called before the normal updateFGList() code.
			if (_mode == EditorMode.XWI)
				recalculateEditorCraftNumbering();

			//string[] fgList = _mission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			comboRefreshFGList(cboMothership, true);
			comboRefreshFGList(cboArrFG, true);
			comboRefreshFGList(cboOrderPrimary, true);
			comboRefreshFGList(cboOrderSecondary, true);
			_loading = temp;
			listRefreshItem(_activeFG);
		}

		void lstFG_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1 || _mission.FlightGroups[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = getFlightGroupDrawColor(e.Index);  //[JB] Moved color selection to different function so that colorized dropdowns could use it too.
			e.Graphics.DrawString(lstFG.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstFG.SelectedIndex == -1 || _noRefresh) return;
			restrictSelection();  // Don't allow multiselect to mix flightgroups and objectgroups. Must be guarded by _noRefresh to prevent recursive overflow.
			_activeFG = lstFG.SelectedIndex;
			string text;
			int min, max;
			bool isFG = _mission.FlightGroups[_activeFG].IsFlightGroup();
			bool train = _mission.FlightGroups[_activeFG].IsTrainingPlatform();
			bool start = _mission.FlightGroups[_activeFG].IsStartingGate();
			if (_mode == EditorMode.XWI)
			{
				if (isFG)
				{
					text = "Flight Group: " + (countGroups(true, true) + (_config.OneIndexedFGs ? 1 : 0)) + " of " + (countGroups(true, false) + 1);
					min = _mission.FlightGroups.GetFirstOfFlightGroup();
					max = _mission.FlightGroups.GetLastOfFlightGroup();
					lblFG.ForeColor = (max - min < 16) ? SystemColors.ControlText : Color.Red;  //Indexes are 0 to 15
				}
				else
				{
					text = "Object Group: " + (countGroups(false, true) + (_config.OneIndexedFGs ? 1 : 0)) + " of " + (countGroups(false, false) + 1);
					min = _mission.FlightGroups.GetFirstOfObjectGroup();
					max = _mission.FlightGroups.GetLastOfObjectGroup();
					lblFG.ForeColor = (max - min < 64) ? SystemColors.ControlText : Color.Red;
				}
			}
			else
			{
				train = false; start = false;  //Hide other stuff.
				max = _mission.FlightGroups.Count - 1; //Max needs to be a valid index for the cmdMove buttons to operate.
				text = "Briefing Icon: " + (_activeFG + (_config.OneIndexedFGs ? 1 : 0)) + " of " + (max + 1);
			}
			lblCraft.Visible = !train || start;
			lblCraft.Text = !start ? "# of craft" : "Minutes";
			numCraft.Minimum = !start ? 1 : 0;
			numCraft.Maximum = !start ? 255 : 32727;  //For starting platforms, it's the minutes on the clock.
			numCraft.Visible = !train || start;
			lblSeconds.Visible = start;
			numSeconds.Visible = start;
			grpCraft5.Visible = train;
			grpCraft5.Enabled = !start;

			numObjectValue.Visible = train;
			cboFormation.Visible = !train;
			cmdForms.Visible = !train;
			label15.Visible = !train;  //Formation label
			grpPlatformBitfield.Visible = !isFG;
			lblPlatformWarning.Visible = train;
			if (!isFG)
			{
				lblObjectValue.Text = start ? "Seconds:" : "Raw value:";

				grpPlatformBitfield.Enabled = (train && !start);
				updateGunBitfield();
			}

			label9.Enabled = isFG;
			cboAI.Enabled = isFG;

			label14.Enabled = isFG;
			cboMarkings.Enabled = isFG;

			label13.Enabled = isFG;
			cboPlayer.Enabled = isFG;

			label10.Enabled = isFG;
			numWaves.Enabled = isFG;

			grpCraft4.Enabled = isFG;
			//cboStatus.Enabled = isFG;

			//label2.Enabled = isFG;  //Name
			label4.Enabled = isFG;
			label5.Enabled = isFG;
			label6.Enabled = isFG;

			lblFG.Text = text;
			bool btemp = _loading;
			_loading = true;
			#region Craft
			txtName.Text = _mission.FlightGroups[_activeFG].Name;
			txtCargo.Text = _mission.FlightGroups[_activeFG].Cargo;
			txtSpecCargo.Text = _mission.FlightGroups[_activeFG].SpecialCargo;
			if (_mission.FlightGroups[_activeFG].SpecialCargoCraft < -1 || _mission.FlightGroups[_activeFG].SpecialCargoCraft >= 255)
				_mission.FlightGroups[_activeFG].SpecialCargoCraft = -1;
			numSC.Value = _mission.FlightGroups[_activeFG].SpecialCargoCraft + 1;
			numCraft.Value = _mission.FlightGroups[_activeFG].NumberOfCraft;
			numWaves.Value = _mission.FlightGroups[_activeFG].NumberOfWaves + 1;  // Mission data contains raw value, zero based number of extra waves.
			if (train)
			{
				chkPlatformGuns.Checked = (_mission.FlightGroups[_activeFG].NumberOfCraft > 1);
				numObjectValue.Value = _mission.FlightGroups[_activeFG].Formation & 0x3F; //Extract the usable bitfield to prevent overflow if data is somehow out of range.
				numSeconds.Value = _mission.FlightGroups[_activeFG].Formation;  //Uses the same value, different control for UI grouping purposes
			}
			if (_mission.FlightGroups[_activeFG].CraftType == 2 && _mission.FlightGroups[_activeFG].Status1 >= 10)
				cboCraft.SelectedIndex = 18;  //Adjust for B-wing
			else if (_mode == EditorMode.BRF && _mission.FlightGroups[_activeFG].CraftType == 25)
				cboCraft.SelectedIndex = 18;  //Adjust for the object mode's B-wing used in briefings.
			else
				cboCraft.SelectedIndex = _mission.FlightGroups[_activeFG].CraftType;
			int objindex = _mission.FlightGroups[_activeFG].ObjectType - 17;
			if (objindex < 0) objindex = 0;
			cboObject.SelectedIndex = objindex;
			cboIFF.SelectedIndex = _mission.FlightGroups[_activeFG].IFF;
			cboAI.SelectedIndex = _mission.FlightGroups[_activeFG].AI;
			cboMarkings.SelectedIndex = _mission.FlightGroups[_activeFG].Markings;
			cboPlayer.SelectedIndex = _mission.FlightGroups[_activeFG].PlayerCraft;
			cboStatus.SelectedIndex = _mission.FlightGroups[_activeFG].Status1 % 10;  //Status >= 10 is for the B-wing, status codes repeat in the same order
			#endregion
			#region Arr/Dep
			optArrHyp.Checked = _mission.FlightGroups[_activeFG].ArriveViaHyperspace;
			optArrMS.Checked = !optArrHyp.Checked;
			optDepHyp.Checked = _mission.FlightGroups[_activeFG].DepartViaHyperspace;
			optDepMS.Checked = !optDepHyp.Checked;
			comboLoadIndex(cboMothership, _mission.FlightGroups[_activeFG].Mothership, true);
			comboLoadIndex(cboArrFG, _mission.FlightGroups[_activeFG].ArrivalFG, true);
			comboLoadIndex(cboArrCondition, _mission.FlightGroups[_activeFG].ArrivalEvent, false);

			numArrMin.Value = _mission.FlightGroups[_activeFG].GetArrivalMinutes();
			numArrSec.Value = _mission.FlightGroups[_activeFG].GetArrivalSeconds();

			cboOrder.SelectedIndex = _mission.FlightGroups[_activeFG].Order;
			comboLoadIndex(cboOrderPrimary, _mission.FlightGroups[_activeFG].TargetPrimary, true);
			comboLoadIndex(cboOrderSecondary, _mission.FlightGroups[_activeFG].TargetSecondary, true);
			cboOrderValue.SelectedIndex = _mission.FlightGroups[_activeFG].DockTimeThrottle;
			cboFormation.Items.Clear();
			cboPrimGoalT.Items.Clear();
			if (objindex > 0)
			{
				cboFormation.Items.AddRange(Strings.FormationObject);
				cboPrimGoalT.Items.AddRange(Strings.ObjectObjective);

				cboFormation.SelectedIndex = _mission.FlightGroups[_activeFG].Formation % 4;
				int goal = _mission.FlightGroups[_activeFG].Formation / 4;
				if (goal > 2)
					goal = 2;
				cboPrimGoalT.SelectedIndex = goal;
			}
			else
			{
				cboFormation.Items.AddRange(Strings.Formation);
				cboPrimGoalT.Items.AddRange(Strings.Objective);
				cboFormation.SelectedIndex = _mission.FlightGroups[_activeFG].Formation;
				cboPrimGoalT.SelectedIndex = _mission.FlightGroups[_activeFG].Objective;
			}
			#endregion
			#region Waypoints
			refreshWaypointTab();  //[JB] Code moved to separate function so that the map callback can refresh it too.
			#endregion
			_loading = btemp;
			checkMove(0);
			if (!lstFG.Focused) lstFG.Focus();  // Return control back to the list (helpful to maintain navigation using the arrow keys when certain tabs are open)
		}

		void cmdImportXWI_Click(object sender, EventArgs e)
		{
			if (_mode == EditorMode.BRF)
			{
				DialogResult res = MessageBox.Show("Overwrite all briefing (BRF) flightgroups with mission (XWI) flightgroups?" + Environment.NewLine + Environment.NewLine + "Existing briefing groups will be deleted!", "Confirm Replacement", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
				if (res == DialogResult.Yes)
				{
					//The current working set is already the briefing mode, so copy from the "other" FG list.
					FlightGroupCollection fglist = new FlightGroupCollection();
					foreach (FlightGroup fg in _mission.FlightGroupsBriefing)
					{
						FlightGroup temp = new FlightGroup
						{
							CraftType = fg.CraftType,
							ObjectType = fg.ObjectType,
							IFF = fg.IFF,
							NumberOfCraft = fg.NumberOfCraft,
							NumberOfWaves = fg.NumberOfWaves,
							Name = string.Copy(fg.Name),
							Cargo = string.Copy(fg.Cargo),
							SpecialCargo = string.Copy(fg.SpecialCargo),
							SpecialCargoCraft = fg.SpecialCargoCraft,
							Pitch = fg.Pitch,
							Yaw = fg.Yaw,
							Roll = fg.Roll,
							Status1 = fg.Status1 //Can't forget this, for B-wing.
						};
						for (int i = 0; i < temp.Waypoints.Length; i++)
						{
							FlightGroup.Waypoint wp = new FlightGroup.Waypoint
							{
								RawX = fg.Waypoints[i].RawX,
								RawY = fg.Waypoints[i].RawY,
								RawZ = fg.Waypoints[i].RawZ,
								Enabled = fg.Waypoints[i].Enabled
							};
							temp.Waypoints[i] = wp;
						}

						//Perform alterations to convert a B-wing into a B-wing icon object, since the BRF doesn't look at status to determine if a Y-wing is really a B-wing.
						if (temp.CraftType == 2 && temp.Status1 >= 10)
						{
							temp.CraftType = 0;
							temp.ObjectType = 25;
						}
						fglist.Add(temp);
					}
					fglist.RemoveAt(0);  //Remove default starting craft from the newly instantiated collection.
					_mission.FlightGroups = fglist;

					lstFG.Items.Clear();
					lstFG.Items.AddRange(_mission.FlightGroups.GetList());
					_activeFG = 0;
					lstFG.SelectedIndex = 0;
					listRefreshAll();
				}
			}
		}
		void cmdMoveUp_Click(object sender, EventArgs e)
		{
			moveFlightgroups(-1);
		}
		void cmdMoveDown_Click(object sender, EventArgs e)
		{
			moveFlightgroups(1);
		}
		void cmdSwitchFG_Click(object sender, EventArgs e)
		{
			switchTo((_mode == EditorMode.XWI) ? EditorMode.BRF : EditorMode.XWI);
		}

		#region Craft
		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboCraft.SelectedIndex == 0 && cboObject.SelectedIndex > 0) return;   // Don't switch to Craft "None" if already an Object.
			enableRot(cboCraft.SelectedIndex == 0 && cboObject.SelectedIndex > 0);
			if (_loading || cboCraft.SelectedIndex == -1) return;
			changeSelectedFlightgroups(cboCraft.SelectedIndex, -1);
			_loading = true;
			cboObject.SelectedIndex = 0;
			_loading = false;
		}
		void cboObject_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboCraft.SelectedIndex > 0 && cboObject.SelectedIndex == 0) return;   // Don't switch to Object "None" if already a Craft.
			enableRot(cboCraft.SelectedIndex == 0 && cboObject.SelectedIndex > 0);
			if (_loading || cboCraft.SelectedIndex == -1) return;
			changeSelectedFlightgroups(-1, cboObject.SelectedIndex);
			_loading = true;
			cboCraft.SelectedIndex = 0;
			_loading = false;
		}
		void cmdForms_Click(object sender, EventArgs e)
		{
			FormationDialog dlg = new FormationDialog(_mission.FlightGroups[_activeFG].Formation, 0, Settings.Platform.XWING);
			if (dlg.ShowDialog() == DialogResult.OK)
				cboFormation.SelectedIndex = dlg.Formation;
		}
		void numSC_ValueChanged(object sender, EventArgs e)
		{
			if (numSC.Value < 1 || numSC.Value > _mission.FlightGroups[_activeFG].NumberOfCraft)
			{
				txtSpecCargo.Visible = false;
				lblNotUsed.Visible = true;
			}
			else
			{
				txtSpecCargo.Visible = true;
				lblNotUsed.Visible = false;
			}
		}
		#endregion
		#region Objects
		void numObjectValue_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				foreach(FlightGroup fg in getSelectedFlightgroups())  // Data is tightly linked, won't work through the multiedit handler.
					fg.Formation = (byte)numObjectValue.Value;
				updateGunBitfield();
			}
		}
		void chkPlatformGuns_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				numCraft.Value = (byte)(chkPlatformGuns.Checked ? 2 : 1);  // Multiedit handler applies value and refreshes
		}
		void chkGunArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			bool[] arr = new bool[6];
			for (int i = 0; i < 6; i++)
				arr[i] = chkGunPlatform[i].Checked;
			// This applies changes via ValueChanged
			numObjectValue.Value = _mission.FlightGroups[_activeFG].PlatformBitfieldPack(arr);
		}
		void updateGunBitfield()
		{
			bool btemp = _loading;
			_loading = true;
			bool[] arr = _mission.FlightGroups[_activeFG].PlatformBitfieldUnpack();
			for (int i = 0; i < 6; i++)
				chkGunPlatform[i].Checked = arr[i];
			_loading = btemp;
		}
		#endregion Objects
		#region ArrDep
		void cmdCopyAD_Click(object sender, EventArgs e)
		{
			menuCopy_Click("AD", new System.EventArgs());
		}
		void cmdPasteAD_Click(object sender, EventArgs e)
		{
			menuPaste_Click("AD", new System.EventArgs());
		}
		#endregion
		#region Orders
		void cboOrder_SelectedIndexChanged(object sender, EventArgs e) => lblODesc.Text = Strings.OrderDesc[cboOrder.SelectedIndex];

		void cmdCopyOrder_Click(object sender, EventArgs e) => menuCopy_Click("Order", new EventArgs());
		void cmdPasteOrder_Click(object sender, EventArgs e) => menuPaste_Click("Order", new EventArgs());
		#endregion
		#region Waypoints
		void enableRot(bool state)
		{
			numYaw.Enabled = state;
			numPitch.Enabled = state;
			numRoll.Enabled = state;
		}
		/// <summary>Checks if the map exists and requests a paint operation</summary>
		/// <remarks>Useful to keep the map synced to the main form's waypoint tab.</remarks>
		void refreshMap(int fgIndex)
		{
			if (_fMap != null && !_fMap.IsDisposed)
			{
				if (fgIndex < 0)
					_fMap.Import(_mission.FlightGroups);
				else if (fgIndex < _mission.FlightGroups.Count)
					_fMap.UpdateFlightGroup(fgIndex, _mission.FlightGroups[fgIndex]);
				_fMap.MapPaint();
			}
		}
		void refreshWaypointTab()  //[JB] New function to refresh the contents the waypoint tab, since we want to call this from more than one place.
		{
			bool btemp = _loading;
			_loading = true;
			bool isCraftFG = (_mission.FlightGroups[_activeFG].ObjectType == 0);
			chkWP1.Enabled = isCraftFG && _mode == EditorMode.XWI;
			chkWP2.Enabled = isCraftFG && _mode == EditorMode.XWI;
			chkWP3.Enabled = isCraftFG && _mode == EditorMode.XWI;
			chkSP2.Enabled = isCraftFG && _mode == EditorMode.XWI;
			chkSP3.Enabled = isCraftFG && _mode == EditorMode.XWI;
			chkWPHyp.Enabled = isCraftFG && _mode == EditorMode.XWI;
			cmdCopyWPSP.Enabled = isCraftFG && _mode == EditorMode.XWI;
			lblCS1.Enabled = (_mode == EditorMode.BRF);
			lblCS2.Enabled = (_mode == EditorMode.BRF);
			lblCS3.Enabled = (_mode == EditorMode.BRF);
			int wpSkipMin = -1, wpSkipMax = -1;
			if (isCraftFG && _mode == EditorMode.XWI) { wpSkipMin = 7; wpSkipMax = 10; }  //XWI FGs skip the CS points
			else if (!isCraftFG && _mode == EditorMode.XWI) { wpSkipMin = 1; wpSkipMax = 10; }  //XWI Objects skip everything that isn't SP1.
			else if (_mode == EditorMode.BRF) { wpSkipMin = 1; wpSkipMax = 7; }  //XWI and BRF objects skip SP2, SP3, WP1, WP2, WP3, and HYP
			chkSP1.Text = (_mode == EditorMode.XWI) ? "Start Point 1" : "SP1 / CS1";
			for (int i = 0; i < 10; i++)
			{
				int wpIndex = _waypointMapping[i]; // 'i' is the display order, wpIndex is the actual index in the FG waypoint list
				for (int j = 0; j < 3; j++)
				{
					if ((wpSkipMin >= 0 && i >= wpSkipMin) && (wpSkipMax >= 0 && i < wpSkipMax))
					{
						_tableRaw.Rows[i][j] = "";
						_table.Rows[i][j] = "";
					}
					else
					{
						_tableRaw.Rows[i][j] = _mission.FlightGroups[_activeFG].Waypoints[wpIndex][j];
						_table.Rows[i][j] = Math.Round((double)_mission.FlightGroups[_activeFG].Waypoints[wpIndex][j] / 160, 2);
					}
				}
				if (i < 7)  //Only the first 7 waypoints are real and have checkboxes.  The remaining waypoints are virtualized as an abstraction for easier BRF coord-set editing behind the scenes.
					chkWP[i].Checked = _mission.FlightGroups[_activeFG].Waypoints[wpIndex].Enabled;
			}
			if (!isCraftFG) chkSP1.Checked = true; //Always check for display purposes, but value is not saved with mission.
			_table.AcceptChanges();
			_tableRaw.AcceptChanges();
			numYaw.Value = (int)Math.Round((double)_mission.FlightGroups[_activeFG].Yaw / 256 * 360);
			numPitch.Value = (int)Math.Round((double)_mission.FlightGroups[_activeFG].Pitch / 256 * 360) - 90;
			numRoll.Value = (int)Math.Round((double)_mission.FlightGroups[_activeFG].Roll / 256 * 360);
			enableRot(_mission.FlightGroups[_activeFG].ObjectType != 0);
			_loading = btemp;
		}

		void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			int wpIndex = _waypointMapping[(int)c.Tag];
			foreach(FlightGroup fg in getSelectedFlightgroups())
				fg.Waypoints[wpIndex].Enabled = Common.Update(this, fg.Waypoints[wpIndex].Enabled, c.Checked);
			refreshMap(_activeFG);
		}
		void table_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j;
			if (_loading) return;
			_loading = true;
			for (j = 0; j < 10; j++) if (_table.Rows[j].Equals(e.Row)) break;   //find the row index that you're changing
			int wpIndex = _waypointMapping[j];
			for (i = 0; i < 3; i++)
			{
				if (!double.TryParse(_table.Rows[j][i].ToString(), out double cell))
					_table.Rows[j][i] = 0;
				short raw = (short)(cell * 160);
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Waypoints[wpIndex][i] = Common.Update(this, fg.Waypoints[wpIndex][i], raw);
				_tableRaw.Rows[j][i] = raw;
			}
			_loading = false;
			refreshMap(_activeFG);
		}
		void tableRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j;
			if (_loading) return;
			_loading = true;
			for (j = 0; j < 10; j++) if (_tableRaw.Rows[j].Equals(e.Row)) break;    //find the row index that you're changing
			int wpIndex = _waypointMapping[j];
			for (i = 0; i < 3; i++)
			{
				if (!short.TryParse(_tableRaw.Rows[j][i].ToString(), out short raw))
					_tableRaw.Rows[j][i] = 0;
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Waypoints[wpIndex][i] = Common.Update(this, fg.Waypoints[wpIndex][i], raw);
				_table.Rows[j][i] = Math.Round((double)raw / 160, 2);
			}
			_loading = false;
			refreshMap(_activeFG);
		}
		void cmdCopyWPSP_Click(object sender, EventArgs e)
		{
			//Setting _table.Rows[] will overwrite the waypoints unless _loading is enabled.  Want to make sure we copy the raw data to avoid any bugs with rounding.
			bool btemp = _loading;
			_loading = true;
			short[] row = new short[3];
			for (int i = 0; i < 3; i++)
				row[i] = _mission.FlightGroups[_activeFG].Waypoints[0][i];  //Get SP1

			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				for (int j = 1; j <= 2; j++)
				{
					int wpIndex = _waypointMapping[j];

					//Enable the Start Points.  Not really necessary since X-wing will always choose a randomized point, but this offers display consistency.
					fg.Waypoints[wpIndex][3] = Common.Update(this, fg.Waypoints[wpIndex][3], (short)1);
					fg.Waypoints[wpIndex].Enabled = Common.Update(this, fg.Waypoints[wpIndex].Enabled, true);

					for (int i = 0; i < 3; i++)
					{
						fg.Waypoints[wpIndex][i] = Common.Update(this, fg.Waypoints[wpIndex][i], row[i]);
						_tableRaw.Rows[j][i] = row[i];
						_table.Rows[j][i] = Math.Round((double)row[i] / 160, 2);
					}
				}
			}
			chkSP2.Checked = true;  //Have to refresh the display manually, even though the data is all set.
			chkSP3.Checked = true;
			_loading = btemp;
			refreshMap(-1);
		}
		#endregion
		#endregion
		#region Mission
		void txtEoMArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.EndOfMissionMessages[(int)t.Tag] = Common.Update(this, _mission.EndOfMissionMessages[(int)t.Tag], t.Text);
		}
		void grpMission_Leave(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.TimeLimitMinutes = Common.Update(this, _mission.TimeLimitMinutes, Convert.ToInt16(numMissionTime.Value));
			_mission.EndEvent = Common.Update(this, _mission.EndEvent, Convert.ToInt16(cboEndEvent.SelectedIndex));
			_mission.RndSeed = Common.Update(this, _mission.RndSeed, Convert.ToInt16(numRndSeed.Value));
			_mission.Location = Common.Update(this, _mission.Location, Convert.ToInt16(cboMissionLocation.SelectedIndex));
		}
		#endregion
	}
}