/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2021 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.9
 */

/* CHANGELOG:
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
		readonly Settings _config;
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
		#endregion
		#region Control Arrays
#pragma warning disable IDE1006 // Naming Styles
		readonly TextBox[] txtEoM = new TextBox[3];
		readonly CheckBox[] chkWP = new CheckBox[7];
		readonly CheckBox[] chkGunPlatform = new CheckBox[6];
		readonly MenuItem[] menuRecentMissions = new MenuItem[6];
		readonly Dictionary<Control, EventHandler> instantUpdate = new Dictionary<Control, EventHandler>();   //[JB] This system allows standard form controls to hook their normal YOGEME event handlers (typically Leave) to update immediately when the data is changed.
		readonly Dictionary<ComboBox, ComboBox> ColorizedFGList = new Dictionary<ComboBox, ComboBox>();  //[JB] Maps a control that should have a colorized FG list with a control that determines whether the list actually contains a FG list.
#pragma warning restore IDE1006 // Naming Styles
		#endregion

		public XwingForm(Settings settings)
		{
			_config = settings;
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		public XwingForm(Settings settings, string path)
		{   //this is the command line and "Open..." support
			_config = settings;
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			if (!loadMission(path)) return;
			lstFG.SelectedIndex = 0;
			_loading = false;
		}

		void closeForms()
		{
			try { _fMap.Close(); }
			catch { /* do nothing */ }
			try { _fBrief.Close(); }
			catch { /* do nothing */ }
			try { _fLibrary.Close(); }
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
			tabMain.Focus(); //[JB] Exit focus from any form controls to avoid triggering any events on missing data.
			_mission = new Mission();
			_config.LastMission = "";
			_activeFG = 0;
			_mission.FlightGroups[0].CraftType = 1;
			_mission.FlightGroups[0].ObjectType = 0;
			_mission.FlightGroups[0].IFF = 1;
			//string[] fgList = _mission.FlightGroups.GetList();
			comboRefreshFGList(cboMothership, true);
			comboRefreshFGList(cboArrFG, true);
			comboRefreshFGList(cboOrderPrimary, true);
			comboRefreshFGList(cboOrderSecondary, true);
			lstFG.Items.Clear();
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
				CraftDataManager.GetInstance().LoadPlatform(Settings.Platform.XWING, _config, Strings.CraftType, Strings.CraftAbbrv, fileMission);
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
							new TieForm(_config, fileMission).Show();
							Close();
							fs.Close(); //[JB] Files were being left open, which could cause access violations.  Need to close stream before returning.
							return false;
						case Platform.MissionFile.Platform.XvT:
							_applicationExit = false;
							new XvtForm(_config, fileMission).Show();
							Close();
							fs.Close();
							return false;
						case Platform.MissionFile.Platform.BoP:
							_applicationExit = false;
							new XvtForm(_config, fileMission).Show();
							Close();
							fs.Close();
							return false;
						case Platform.MissionFile.Platform.XWA:
							_applicationExit = false;
							new XwaForm(_config, fileMission).Show();
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
			Variable.DrawMode = DrawMode.OwnerDrawVariable;
			Variable.DrawItem += colorizedComboBox_DrawItem;
		}
		/// <summary>Applies the given <paramref name="handler"/> to the control's Changed event</summary>
		/// <param name="handler">Update event handler, usually Leave</param>
		/// <remarks>This was done to prevent having to redefine the events away from Leave. Controls will not update if loading</remarks>
		void registerInstantUpdate(Control control, EventHandler handler)
		{
			instantUpdate.Add(control, handler);
			string ct = control.GetType().ToString();
			if (ct == "System.Windows.Forms.TextBox") ((TextBox)control).TextChanged += instantUpdateHandler;
			else if (ct == "System.Windows.Forms.NumericUpDown") ((NumericUpDown)control).ValueChanged += instantUpdateHandler;
			else if (ct == "System.Windows.Forms.ComboBox") ((ComboBox)control).SelectedIndexChanged += instantUpdateHandler;
			else if (ct == "System.Windows.Forms.CheckBox") ((CheckBox)control).CheckedChanged += instantUpdateHandler;
			else if (ct == "System.Windows.Forms.RadioButton") ((RadioButton)control).CheckedChanged += instantUpdateHandler;
		}
		void saveMission(string fileMission)
		{
			switchTo(EditorMode.XWI);
			try { _fBrief.Save(); }
			catch { /* do nothing */ }
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
			cboCraft.SelectedIndex = _mission.FlightGroups[0].CraftType; // already loaded in loadCraftData
			cboObject.Items.AddRange(Strings.ObjectType); cboObject.SelectedIndex = _mission.FlightGroups[0].ObjectType;
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

			#region InstantUpdate
			//RegisterInstantUpdate(txtName, txtName_Leave);
			registerInstantUpdate(numWaves, grpCraft3_Leave);
			registerInstantUpdate(numCraft, grpCraft3_Leave);
			registerInstantUpdate(cboIFF, grpCraft2_Leave);
			registerInstantUpdate(cboPlayer, grpCraft2_Leave);

			registerColorizedFGList(cboMothership, null);
			registerColorizedFGList(cboArrFG, null);
			registerColorizedFGList(cboOrderPrimary, null);
			registerColorizedFGList(cboOrderSecondary, null);
			#endregion InstantUpdate

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

		void briefingModifiedCallback(object sender, EventArgs e)
		{
			Common.Title(this, _loading);
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
			if (_fMap != null)
			{
				lstFG.SelectedIndex = -1;
				lstFG.SelectedIndex = _activeFG;
			}
		}
		void form_Deactivate(object sender, EventArgs e)
		{
			// Exit focus from any form controls. This submits changes to the map (if it's open), and can prevent issues if coming back in.
			lstFG.Focus();
		}
		void form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			promptSave();
			if (_config.ConfirmExit && _applicationExit)
			{
				DialogResult res = MessageBox.Show("Are you sure you wish to exit?", "Confirm", MessageBoxButtons.YesNo);
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
			else if (e.KeyCode == Keys.Enter) //Allows the Enter key to submit changes in a TextBox or similar control by triggering a Leave() event.
			{
				Control c = ActiveControl;
				bool text = c.GetType().ToString() == "System.Windows.Forms.TextBox";
				int caret = 0;
				if (text)  //Focus() on a TextBox control might cause it to select all text, so preserve the caret position. 
				{
					if (((TextBox)c).Multiline == true) return;  //Multiline textboxes need to allow newlines.
					caret = ((TextBox)c).SelectionStart;
				}

				tabMain.Focus();
				c.Focus();
				if (text)
				{
					((TextBox)c).SelectionStart = caret;
					((TextBox)c).SelectionLength = 0;
				}

				e.Handled = true;
				e.SuppressKeyPress = true; //Stop the Windows UI beeping
			}
		}

		void instantUpdateHandler(object sender, EventArgs e)
		{
			if (_loading) return;
			instantUpdate[(Control)sender](sender, e);
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
					savXW.ShowDialog();
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
			Stream stream = new FileStream(Application.StartupPath + "\\YOGEME.bin", FileMode.Create, FileAccess.Write, FileShare.None);
			#region ArrDep
			if (sender.ToString() == "AD")
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG]);
				stream.Close();
				return;
			}
			#endregion
			#region Order
			if (sender.ToString() == "Order")
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG]);
				stream.Close();
				return;
			}
			#endregion
			#region generic form controls
			if (ActiveControl.GetType().ToString() == "System.Windows.Forms.TextBox")
			{
				TextBox txt_t = (TextBox)ActiveControl;
				if (txt_t.SelectedText != "")
				{
					formatter.Serialize(stream, txt_t.SelectedText);
					stream.Close();
					return;
				}
			}
			else if (ActiveControl.GetType().ToString() == "System.Windows.Forms.NumericUpDown")  //[JB] Added copy/paste for this
			{
				NumericUpDown num_t = (NumericUpDown)ActiveControl;
				formatter.Serialize(stream, num_t.Value);
				stream.Close();
				return;
			}
			else if (ActiveControl.GetType().ToString() == "System.Windows.Forms.DataGridTextBox")
			{
				stream.Close(); //[JB] I can't get it to copy/paste the current cell content, but this will prevent the entire FG from copy/paste.
				return;
			}
			#endregion
			switch (tabMain.SelectedIndex)
			{
				case 0:
					formatter.Serialize(stream, _mission.FlightGroups[_activeFG]);
					break;
			}
			stream.Close();
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
			if (_fLibrary != null)
				_fLibrary.Close();
			_fLibrary = new FlightGroupLibraryForm(Settings.Platform.XWING, _mission.FlightGroups, flightGroupLibraryCallback);
		}
		void flightGroupLibraryCallback(object sender, EventArgs e)
		{
			foreach (FlightGroup fg in (object[])sender)
			{
				if (fg == null || !newFG(fg.IsFlightGroup()))
					break;
				_mission.FlightGroups[_activeFG] = fg;
				updateFGList();
				listRefresh();
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
			Common.LaunchIdmr();
		}
		void menuMap_Click(object sender, EventArgs e)
		{
			try { _fMap.Close(); }
			catch { /* do nothing */ }
			_fMap = new MapForm(_config, _mission.FlightGroups, mapForm_DataChangedCallback);
			_fMap.Show();
		}
		void mapForm_DataChangedCallback(object sender, EventArgs e)
		{
			Common.Title(this, false);
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
			new TieForm(_config).Show();
			Close();
		}
		void menuNewXvT_Click(object sender, EventArgs e)
		{
			promptSave();
			closeForms();
			_applicationExit = false;
			new XvtForm(_config, sender.ToString() == "BoP").Show();
			Close();
		}
		void menuNewXWA_Click(object sender, EventArgs e)
		{
			promptSave();
			closeForms();
			_applicationExit = false;
			new XwaForm(_config).Show();
			Close();
		}
		void menuOpen_Click(object sender, EventArgs e)
		{
			promptSave();
			opnXW.FileName = _mission.MissionFileName;
			if (opnXW.ShowDialog() == DialogResult.OK)  //[JB] Fixes bug where dialog could be stuck open. 
			{
				opnXW_FileOk(0, new System.ComponentModel.CancelEventArgs());
				_config.SetWorkingPath(Path.GetDirectoryName(opnXW.FileName));
				opnXW.InitialDirectory = _config.GetWorkingPath(); //Update since folder may have changed
			}
		}
		void menuOptions_Click(object sender, EventArgs e)
		{
			new OptionsDialog(_config, null).ShowDialog();  //X-wing doesn't use interact labels, no callback event
		}
		void menuPaste_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream;
			try { stream = new FileStream(Application.StartupPath + "\\YOGEME.bin", FileMode.Open, FileAccess.Read, FileShare.Read); }
			catch { return; }
			#region ArrDep
			if (sender.ToString() == "AD")
			{
				try
				{
					FlightGroup fg_temp = (FlightGroup)formatter.Deserialize(stream);
					if (fg_temp == null) throw new Exception();
					FlightGroup cur = _mission.FlightGroups[_activeFG];
					cur.ArrivalHyperspace = fg_temp.ArrivalHyperspace;
					cur.DepartureHyperspace = fg_temp.DepartureHyperspace;
					cur.Mothership = fg_temp.Mothership;
					if (cur.Mothership >= _mission.FlightGroups.Count)
						cur.Mothership = -1;
					cur.ArrivalFG = fg_temp.ArrivalFG;
					if (cur.ArrivalFG >= _mission.FlightGroups.Count)
						cur.ArrivalFG = -1;
					cur.ArrivalEvent = fg_temp.ArrivalEvent;
					cur.ArrivalDelay = fg_temp.ArrivalDelay;
					lstFG_SelectedIndexChanged(0, new EventArgs());
					listRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region Order
			if (sender.ToString() == "Order")
			{
				try
				{
					FlightGroup fg_temp = (FlightGroup)formatter.Deserialize(stream);
					if (fg_temp == null) throw new Exception();
					FlightGroup cur = _mission.FlightGroups[_activeFG];
					cur.Order = fg_temp.Order;
					cur.TargetPrimary = fg_temp.TargetPrimary;
					if (cur.TargetPrimary >= _mission.FlightGroups.Count)
						cur.TargetPrimary = -1;
					cur.TargetSecondary = fg_temp.TargetSecondary;
					if (cur.TargetSecondary >= _mission.FlightGroups.Count)
						cur.TargetSecondary = -1;
					cur.DockTimeThrottle = fg_temp.DockTimeThrottle;
					lstFG_SelectedIndexChanged(0, new EventArgs());
					listRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region generic form controls
			try
			{
				if (ActiveControl.GetType().ToString() == "System.Windows.Forms.TextBox")
				{
					string s = formatter.Deserialize(stream).ToString();
					if (s.IndexOf("System.", 0) != -1) return;
					if (s.IndexOf("Idmr.", 0) != -1) return;
					TextBox t = (TextBox)ActiveControl;
					t.SelectedText = s;
					stream.Close();
					Common.Title(this, false);
					return;
				}
				else if (ActiveControl.GetType().ToString() == "System.Windows.Forms.NumericUpDown") //[JB] Added copy/paste for this
				{
					string str_t = formatter.Deserialize(stream).ToString();
					NumericUpDown control = (NumericUpDown)ActiveControl;
					decimal value = Convert.ToDecimal(str_t);
					if (value > control.Maximum) value = control.Maximum;
					else if (value < control.Minimum) value = control.Minimum;
					control.Value = value;
					Common.Title(this, false);
					stream.Close();
					return;
				}
				else if (ActiveControl.GetType().ToString() == "System.Windows.Forms.DataGridTextBox")
				{
					stream.Close(); //[JB] I can't get it to copy/paste the current cell content, but this will prevent the entire FG from copy/paste.
					return;
				}
			}
			catch { /* do nothing*/ }
			#endregion
			#region overalls by tab
			switch (tabMain.SelectedIndex)
			{
				case 0:
					try
					{
						FlightGroup fg_temp = (FlightGroup)formatter.Deserialize(stream);
						if (fg_temp == null) throw new Exception();
						if (_mode == EditorMode.BRF)  //Can't validate anything if pasting into BRF, so reset indexes.
						{
							fg_temp.Mothership = -1;
							fg_temp.ArrivalFG = -1;
							fg_temp.ArrivalEvent = 0;
							fg_temp.TargetPrimary = -1;
							fg_temp.TargetSecondary = -1;
						}
						bool isCraft = fg_temp.IsFlightGroup();
						newFG(isCraft);
						_mission.FlightGroups[_activeFG] = fg_temp;
						refreshMap(-1);
						updateFGList(); //[JB] Update all the downdown lists.
						listRefresh();
						_startingShips--;
						lstFG.SelectedIndex = _activeFG;
						lstFG_SelectedIndexChanged(0, new EventArgs()); //[JB] Need to force refresh of form controls.
						craftStart(fg_temp, true);
						lstFG.Focus();  //[JB] Return focus to list.  Lets the user delete the pasted FG without having to manually select it again.
					}
					catch { /* do nothing */ }
					break;
			}
			#endregion
			stream.Close();
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
			savXW.ShowDialog();
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
					int briefLen = br.GetEventsLength(page);
					int p = 0;
					while (p < briefLen)
					{
						if (pg.Events[p + 1] >= (int)Briefing.EventType.FGTag1 && pg.Events[p + 1] <= (int)Briefing.EventType.FGTag4)
						{
							if (pg.Events[p + 2] == fgIndex)
								count[cBrief]++;
						}
						else if (pg.Events[p + 1] == (int)Briefing.EventType.EndBriefing || pg.Events[p + 1] == (int)Briefing.EventType.None)
						{
							break;
						}
						p += 2 + br.EventParameterCount(pg.Events[p + 1]);
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
			if (_fBrief != null)  //Close (which also saves) the briefing before accessing it.  Don't call save directly since this may cause FG index corruption if multiple FGs are deleted.
				_fBrief.Close();

			//[JB] Confirm delete
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
					if (res == DialogResult.No)
						return;
				}
			}

			replaceClipboardFGReference(_activeFG, -1);
			if (_mode == EditorMode.BRF)
				_mission.Briefing.TransformFGReferences(_activeFG, -1);

			if (_mission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(_activeFG);
			craftStart(_mission.FlightGroups[_activeFG], false);
			if (_mission.FlightGroups.Count == 1)
			{
				_activeFG = _mission.FlightGroups.RemoveAt(_activeFG);  //[JB] Still need to delete to clear the indexes.  The delete function always ensures that Count==1, so it has to be inside this block, not before.
				_mission.FlightGroups.Clear();
				_activeFG = 0;
				_mission.FlightGroups[0].CraftType = 1;
				_mission.FlightGroups[0].ObjectType = 0;
				_mission.FlightGroups[0].IFF = 1;
				craftStart(_mission.FlightGroups[0], true);
			}
			else _activeFG = _mission.FlightGroups.RemoveAt(_activeFG);
			updateFGList();
			lstFG.SelectedIndex = _activeFG;
			Common.Title(this, _loading);
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

			updateMissionTabs();
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
		void listRefresh()
		{
			_noRefresh = true;  //Prevent full lstFG refresh.
			lstFG.Items[_activeFG] = ((_mode == EditorMode.XWI) ? "" : "[BRF] ") + _mission.FlightGroups[_activeFG].ToString(true);
			if (!lstFG.IsDisposed)  //[JB] Not sure if this fix is needed for this platform, but including for safety.
				lstFG.Invalidate(lstFG.GetItemRectangle(_activeFG));
			_noRefresh = false;
			if (_fMap != null) _fMap.UpdateFlightGroup(_activeFG, _mission.FlightGroups[_activeFG]);  //[JB] If the display name needs to be updated, the map most likely does too.
		}
		void listRefreshAll()
		{
			bool btemp = _loading;
			_loading = true;
			for (int i = 0; i < lstFG.Items.Count; i++)
				lstFG.Items[i] = ((_mode == EditorMode.XWI) ? "" : "[BRF] ") + _mission.FlightGroups[i].ToString(true);
			_loading = btemp;
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
			lstFG.SelectedIndex = _activeFG;
			Common.Title(this, _loading);
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
					lstFG.Items[i] = _mission.FlightGroups[i].ToString(true);
				}
			}
			_activeFG = currentFG;
		}
		/// <summary>Updates the clipboard contents from containing broken indexes.</summary>
		/// <remarks>Should be called during swap or delete (dstIndex < 0) operations.</remarks>
		void replaceClipboardFGReference(int srcIndex, int dstIndex)
		{
			//[JB] Replace any clipboard references.  Load it, check/modify type, save back to stream.  Since clipboard access is through a file on disk, I thought it would be best to avoid hammering it with changes if nothing actually changed on the clipboard.
			Stream stream = null;
			try
			{
				System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				stream = new FileStream(Application.StartupPath + "\\YOGEME.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
				object raw = formatter.Deserialize(stream);
				stream.Close();
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
						if (fg.Mothership == srcIndex) { fg.Mothership = dst; fg.ArrivalHyperspace = 1; change = true; } else if (fg.Mothership > srcIndex && dstIndex == -1) { fg.Mothership--; change = true; }
						if (fg.TargetPrimary == srcIndex) { fg.TargetPrimary = dst; change = true; } else if (fg.TargetPrimary > srcIndex && dstIndex == -1) { fg.TargetPrimary--; change = true; }
						if (fg.TargetSecondary == srcIndex) { fg.TargetSecondary = dst; change = true; } else if (fg.TargetSecondary > srcIndex && dstIndex == -1) { fg.TargetSecondary--; change = true; }
					}
					if (change)
					{
						stream = new FileStream(Application.StartupPath + "\\YOGEME.bin", FileMode.Create, FileAccess.Write, FileShare.None);
						formatter.Serialize(stream, raw);
						stream.Close();
					}
				}
			}
			catch
			{
				if (stream != null) stream.Close();  //Just in case...
			}
		}
		int translateNullableFG(ComboBox cbo)
		{
			//Return -1 for none selected, otherwise zero based FG index.
			if (cbo.SelectedIndex >= 1)
				return cbo.SelectedIndex - 1;

			return -1;
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
			listRefresh();
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
			if (lstFG.SelectedIndex == -1) return;
			if (_noRefresh == true && lstFG.SelectedIndex == _activeFG) return;   //[JB] See also listRefresh().  Replacing the item text will trigger lstFG_SelectedIndexChanged.  Improves performance by avoiding massive slowdown caused by multiple repeated refreshing.  Also avoids a stack overflow which can be caused by an endless loop of event conflicts.
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
					text = "Flight Group: " + (countGroups(true, true) + 1) + " of " + (countGroups(true, false) + 1);
					min = _mission.FlightGroups.GetFirstOfFlightGroup();
					max = _mission.FlightGroups.GetLastOfFlightGroup();
					lblFG.ForeColor = (max - min < 16) ? SystemColors.ControlText : Color.Red;  //Indexes are 0 to 15
				}
				else
				{
					text = "Object Group: " + (countGroups(false, true) + 1) + " of " + (countGroups(false, false) + 1);
					min = _mission.FlightGroups.GetFirstOfObjectGroup();
					max = _mission.FlightGroups.GetLastOfObjectGroup();
					lblFG.ForeColor = (max - min < 64) ? SystemColors.ControlText : Color.Red;
				}
			}
			else
			{
				train = false; start = false;  //Hide other stuff.
				min = 0;
				max = _mission.FlightGroups.Count - 1; //Max needs to be a valid index for the cmdMove buttons to operate.
				text = "Briefing Icon: " + (_activeFG + 1) + " of " + (max + 1);
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

			cmdMoveUp.Enabled = (_activeFG > min);
			cmdMoveDown.Enabled = (_activeFG < max);

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
			numWaves.Value = _mission.FlightGroups[_activeFG].NumberOfWaves + 1;  //[JB] Modifying the logic so the mission data reflects actual raw values
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
			optArrHyp.Checked = Convert.ToBoolean(_mission.FlightGroups[_activeFG].ArrivalHyperspace);
			optArrMS.Checked = !optArrHyp.Checked;
			optDepHyp.Checked = Convert.ToBoolean(_mission.FlightGroups[_activeFG].DepartureHyperspace);
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
			if (!lstFG.Focused) lstFG.Focus();  //[JB] Return control back to the list (helpful to maintain navigation using the arrow keys when certain tabs are open)
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
			if (_mode == EditorMode.XWI)
			{
				if (_mission.FlightGroups.MoveUp(lstFG.SelectedIndex))
				{
					replaceClipboardFGReference(_activeFG, _activeFG - 1);
					refreshMap(-1);
					updateFGList();
					_activeFG = lstFG.SelectedIndex; listRefresh();
					_activeFG = lstFG.SelectedIndex - 1; listRefresh();
					lstFG.SelectedIndex = _activeFG;
				}
			}
			else  //BRF slot movement needs to perform a raw swap without Flight Group/Object Group considerations.
			{
				int start = _activeFG;
				int end = _activeFG - 1;
				if (end >= 0)
				{
					_mission.FlightGroups.Swap(start, end);
					_mission.Briefing.TransformFGReferences(end, 32767);
					_mission.Briefing.TransformFGReferences(start, end);
					_mission.Briefing.TransformFGReferences(32767, start);
					replaceClipboardFGReference(start, end);
					refreshMap(-1);
					updateFGList();
					_activeFG = lstFG.SelectedIndex; listRefresh();
					_activeFG = lstFG.SelectedIndex - 1; listRefresh();
					lstFG.SelectedIndex = _activeFG;
				}
			}
		}
		void cmdMoveDown_Click(object sender, EventArgs e)
		{
			if (_mode == EditorMode.XWI)
			{
				if (_mission.FlightGroups.MoveDown(lstFG.SelectedIndex))
				{
					replaceClipboardFGReference(_activeFG, _activeFG + 1);
					refreshMap(-1);
					updateFGList();
					_activeFG = lstFG.SelectedIndex; listRefresh();
					_activeFG = lstFG.SelectedIndex + 1; listRefresh();
					lstFG.SelectedIndex = _activeFG;
				}
			}
			else  //BRF slot movement needs to perform a raw swap without Flight Group/Object Group considerations.
			{
				int start = _activeFG;
				int end = _activeFG + 1;
				if (end < _mission.FlightGroups.Count)
				{
					_mission.FlightGroups.Swap(start, end);
					_mission.Briefing.TransformFGReferences(end, 32767);
					_mission.Briefing.TransformFGReferences(start, end);
					_mission.Briefing.TransformFGReferences(32767, start);
					replaceClipboardFGReference(start, end);
					refreshMap(-1);
					updateFGList();
					_activeFG = lstFG.SelectedIndex; listRefresh();
					_activeFG = lstFG.SelectedIndex + 1; listRefresh();
					lstFG.SelectedIndex = _activeFG;
				}
			}
		}
		void cmdSwitchFG_Click(object sender, EventArgs e)
		{
			switchTo((_mode == EditorMode.XWI) ? EditorMode.BRF : EditorMode.XWI);
		}

		#region Craft
		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			if (_mode == EditorMode.XWI)
			{
				craftStart(_mission.FlightGroups[_activeFG], false);  //Remove just in case we're changing between craft and object
				_mission.FlightGroups[_activeFG].CraftType = Common.Update(this, _mission.FlightGroups[_activeFG].CraftType, Convert.ToByte(cboCraft.SelectedIndex));
				bool swap = !_mission.FlightGroups[_activeFG].IsFlightGroup();
				int dest = _mission.FlightGroups.GetLastOfFlightGroup(); //Need to get the destination before changing the craft type, otherwise this function will return the wrong data.
				if (_mission.FlightGroups[_activeFG].CraftType == 18)  //B-wing selected in Craft List
				{
					_mission.FlightGroups[_activeFG].CraftType = 2;  //Replace with Y-W
					_mission.FlightGroups[_activeFG].Status1 = (byte)((_mission.FlightGroups[_activeFG].Status1 % 10) + 10);
				}
				else
				{
					if (_mission.FlightGroups[_activeFG].Status1 >= 10)
						_mission.FlightGroups[_activeFG].Status1 = (byte)(_mission.FlightGroups[_activeFG].Status1 % 10);
				}
				_mission.FlightGroups[_activeFG].ObjectType = 0;
				enableRot(_mission.FlightGroups[_activeFG].ObjectType != 0);
				if (swap)
				{
					_mission.FlightGroups[_activeFG].Formation = 0;  //Swapping from Object to Craft, so reset these fields since objects often use these values for other things which can easily exceed the expected values for ordinary craft FGs.
					_mission.FlightGroups[_activeFG].NumberOfCraft = 1;
					for (int i = _activeFG; i > dest + 1; i--)
						_mission.FlightGroups.Swap(i, i - 1);
					listRefreshAll();
					_activeFG = dest + 1;
					lstFG.SelectedIndex = _activeFG;
				}
				updateFGList();
				craftStart(_mission.FlightGroups[_activeFG], true);
			}
			else
			{
				int ctype = cboCraft.SelectedIndex;
				int otype = 0;
				if (ctype == 18)  //If selecting B-wing, change to B-wing Icon instead.
				{
					ctype = 0;
					otype = 25;
				}

				_loading = true;
				cboCraft.SelectedIndex = ctype;
				cboObject.SelectedIndex = (otype == 25) ? 8 : 0;  //Select the B-wing icon index, otherwise nothing.
				_loading = false;
				_mission.FlightGroups[_activeFG].CraftType = Common.Update(this, _mission.FlightGroups[_activeFG].CraftType, Convert.ToByte(ctype));
				_mission.FlightGroups[_activeFG].ObjectType = Common.Update(this, _mission.FlightGroups[_activeFG].ObjectType, Convert.ToByte(otype));
				updateFGList();
			}
			refreshMap(-1);  //Since not just craft type but possibly index could change as well, refresh the entire thing.
		}
		void cboObject_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			if (_mode == EditorMode.XWI)
			{
				if (cboObject.SelectedIndex == 0 && _mission.FlightGroups[_activeFG].ObjectType == 0)
					return;  //Nothing actually changed, so don't update.  This skips form initializations from changing the object type.

				craftStart(_mission.FlightGroups[_activeFG], false);  //Remove just in case we're changing between craft and object
				bool swap = _mission.FlightGroups[_activeFG].IsFlightGroup();
				_mission.FlightGroups[_activeFG].ObjectType = Common.Update(this, _mission.FlightGroups[_activeFG].ObjectType, Convert.ToByte(cboObject.SelectedIndex + 17));
				_mission.FlightGroups[_activeFG].CraftType = 0;
				enableRot(_mission.FlightGroups[_activeFG].ObjectType != 0);
				if (swap)
				{
					_mission.FlightGroups.NullifyReferences(_activeFG);
					for (int i = _activeFG; i < _mission.FlightGroups.Count - 1; i++)
						_mission.FlightGroups.Swap(i, i + 1);
					listRefreshAll();
					_activeFG = lstFG.Items.Count - 1;
					lstFG.SelectedIndex = _activeFG;
				}
				updateFGList();
				craftStart(_mission.FlightGroups[_activeFG], true);
				lstFG_SelectedIndexChanged(this, new EventArgs());  //Objects have a lot of controls that need updating, depending on type.
			}
			else
			{
				_mission.FlightGroups[_activeFG].ObjectType = Common.Update(this, _mission.FlightGroups[_activeFG].ObjectType, Convert.ToByte(cboObject.SelectedIndex + 17));
				_mission.FlightGroups[_activeFG].CraftType = 0;
				listRefreshAll();
			}
			refreshMap(-1);  //Since not just craft type but possibly index could change as well, refresh the entire thing.
		}
		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				int value = cboFormation.SelectedIndex;
				if (_mission.FlightGroups[_activeFG].ObjectType > 0)
					value += (cboPrimGoalT.SelectedIndex * 4);
				_mission.FlightGroups[_activeFG].Formation = Common.Update(this, _mission.FlightGroups[_activeFG].Formation, Convert.ToByte(value));
			}
		}
		void cboStatus_Leave(object sender, EventArgs e)
		{
			byte status = Convert.ToByte((cboCraft.SelectedIndex == 18 ? 10 : 0) + Convert.ToByte(cboStatus.SelectedIndex));  //B-wing repeats codes at status 10 and higher
			_mission.FlightGroups[_activeFG].Status1 = Common.Update(this, _mission.FlightGroups[_activeFG].Status1, status);
		}
		void cmdForms_Click(object sender, EventArgs e)
		{
			try  //[JB] Added try/catch
			{
				FormationDialog dlg = new FormationDialog(_mission.FlightGroups[_activeFG].Formation);
				dlg.SetToTie95();
				if (dlg.ShowDialog() == DialogResult.OK) cboFormation.SelectedIndex = dlg.Formation;
			}
			catch
			{
				MessageBox.Show("The Formations browser could not be loaded.", "Error");
			}
		}

		void grpCraft2_Leave(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].IFF = Common.Update(this, _mission.FlightGroups[_activeFG].IFF, Convert.ToByte(cboIFF.SelectedIndex));
			_mission.FlightGroups[_activeFG].AI = Common.Update(this, _mission.FlightGroups[_activeFG].AI, Convert.ToByte(cboAI.SelectedIndex));
			_mission.FlightGroups[_activeFG].Markings = Common.Update(this, _mission.FlightGroups[_activeFG].Markings, Convert.ToByte(cboMarkings.SelectedIndex));
			_mission.FlightGroups[_activeFG].PlayerCraft = Common.Update(this, _mission.FlightGroups[_activeFG].PlayerCraft, Convert.ToByte(cboPlayer.SelectedIndex));

			if (ActiveControl == cboIFF)
				updateFGList();  //[JB] Update everything, including craft counter.
			else
				listRefresh();   //Otherwise default to simple refresh.
		}
		void grpCraft3_Leave(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].NumberOfWaves = Common.Update(this, _mission.FlightGroups[_activeFG].NumberOfWaves, Convert.ToByte(numWaves.Value - 1));  //[JB] Modified
			craftStart(_mission.FlightGroups[_activeFG], false);
			_mission.FlightGroups[_activeFG].NumberOfCraft = Common.Update(this, _mission.FlightGroups[_activeFG].NumberOfCraft, Convert.ToByte(numCraft.Value));
			craftStart(_mission.FlightGroups[_activeFG], true);
			listRefresh();
		}
		void numSC_ValueChanged(object sender, EventArgs e)
		{
			if (numSC.Value < 1 || numSC.Value > _mission.FlightGroups[_activeFG].NumberOfCraft)
			{
				if (!_loading)
					_mission.FlightGroups[_activeFG].SpecialCargoCraft = Common.Update(this, _mission.FlightGroups[_activeFG].SpecialCargoCraft, (short)-1);
				txtSpecCargo.Visible = false;
				lblNotUsed.Visible = true;
			}
			else
			{
				if (!_loading)
					_mission.FlightGroups[_activeFG].SpecialCargoCraft = Common.Update(this, _mission.FlightGroups[_activeFG].SpecialCargoCraft, Convert.ToInt16(numSC.Value - 1));   //In the XWI, a value of 0 is the first craft.  Must be outside range to have no special craft.
				txtSpecCargo.Visible = true;
				lblNotUsed.Visible = false;
			}
		}

		void txtCargo_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Cargo = Common.Update(this, _mission.FlightGroups[_activeFG].Cargo, txtCargo.Text);
		}
		void txtName_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Name = Common.Update(this, _mission.FlightGroups[_activeFG].Name, txtName.Text);
			updateFGList();
		}
		void txtSpecCargo_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].SpecialCargo = Common.Update(this, _mission.FlightGroups[_activeFG].SpecialCargo, txtSpecCargo.Text);
		}
		#endregion
		#region Objects
		void numObjectValue_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				_mission.FlightGroups[_activeFG].Formation = (byte)numObjectValue.Value;
				updateGunBitfield();
			}
		}
		void chkPlatformGuns_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				_mission.FlightGroups[_activeFG].NumberOfCraft = (byte)(chkPlatformGuns.Checked ? 2 : 1);
				numCraft.Value = _mission.FlightGroups[_activeFG].NumberOfCraft;
				listRefresh();
			}
		}
		void chkGunArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			bool[] arr = new bool[6];
			for (int i = 0; i < 6; i++)
				arr[i] = chkGunPlatform[i].Checked;

			int value = _mission.FlightGroups[_activeFG].PlatformBitfieldPack(arr);
			_mission.FlightGroups[_activeFG].Formation = (byte)value;
			numObjectValue.Value = value;
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
		void numSeconds_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].Formation = (byte)numSeconds.Value;
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

		void grpArr_Leave(object sender, EventArgs e)
		{
			craftStart(_mission.FlightGroups[_activeFG], false);
			_mission.FlightGroups[_activeFG].ArrivalDelay = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalDelay, Convert.ToInt16(_mission.FlightGroups[_activeFG].CreateArrivalDelay((int)numArrMin.Value, (int)numArrSec.Value)));
			_mission.FlightGroups[_activeFG].ArrivalCraft1 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalCraft1, Convert.ToByte(cboMothership.SelectedIndex));
		}

		void optArrMS_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrivalMethod1 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalMethod1, optArrMS.Checked);
		}
		void optDepMS_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].DepartureMethod1 = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureMethod1, optDepMS.Checked);
		}

		void optArrHyp_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrivalHyperspace = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalHyperspace, Convert.ToInt16(optArrHyp.Checked));
		}
		void optDepHyp_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].DepartureHyperspace = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureHyperspace, Convert.ToInt16(optDepHyp.Checked));
		}
		void cboMothership_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].Mothership = Common.Update(this, _mission.FlightGroups[_activeFG].Mothership, Convert.ToInt16(translateNullableFG(cboMothership)));
		}

		void cboArrFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrivalFG = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalFG, Convert.ToInt16(translateNullableFG(cboArrFG)));
		}
		void cboArrCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				if (_mission.FlightGroups[_activeFG].ArrivalEvent == 0)
					craftStart(_mission.FlightGroups[_activeFG], false);

				_mission.FlightGroups[_activeFG].ArrivalEvent = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalEvent, Convert.ToInt16(cboArrCondition.SelectedIndex));
				craftStart(_mission.FlightGroups[_activeFG], true);
			}
		}

		void numArrMin_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				FlightGroup fg = _mission.FlightGroups[_activeFG];
				craftStart(fg, false);
				fg.ArrivalDelay = Common.Update(this, fg.ArrivalDelay, Convert.ToInt16(fg.CreateArrivalDelay((int)numArrMin.Value, (int)numArrSec.Value)));
				craftStart(fg, true);
			}
		}
		void numArrSec_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				FlightGroup fg = _mission.FlightGroups[_activeFG];
				int value = (int)numArrSec.Value / 6 * 6; //Strip out all values that are not multiples of 6
				if ((int)numArrSec.Value != value) numArrSec.Value = value;

				craftStart(fg, false);
				fg.ArrivalDelay = Common.Update(this, fg.ArrivalDelay, Convert.ToInt16(fg.CreateArrivalDelay((int)numArrMin.Value, (int)numArrSec.Value)));
				craftStart(fg, true);
			}
		}
		#endregion
		#region Orders
		void cmdCopyOrder_Click(object sender, EventArgs e)
		{
			menuCopy_Click("Order", new EventArgs());
		}
		void cmdPasteOrder_Click(object sender, EventArgs e)
		{
			menuPaste_Click("Order", new EventArgs());
		}

		void cboPrimGoalT_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				_mission.FlightGroups[_activeFG].Objective = Common.Update(this, _mission.FlightGroups[_activeFG].Objective, Convert.ToInt16(cboPrimGoalT.SelectedIndex));
				if (_mission.FlightGroups[_activeFG].ObjectType > 0)
				{
					_mission.FlightGroups[_activeFG].Formation = Convert.ToByte((cboPrimGoalT.SelectedIndex * 4) + (cboFormation.SelectedIndex % 4));
				}
			}
		}
		void cboOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].Order = Common.Update(this, _mission.FlightGroups[_activeFG].Order, Convert.ToInt16(cboOrder.SelectedIndex));
		}
		void cboOrderPrimary_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].TargetPrimary = Common.Update(this, _mission.FlightGroups[_activeFG].TargetPrimary, Convert.ToInt16(translateNullableFG(cboOrderPrimary)));
		}
		void cboOrderSecondary_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].TargetSecondary = Common.Update(this, _mission.FlightGroups[_activeFG].TargetSecondary, Convert.ToInt16(translateNullableFG(cboOrderSecondary)));
		}
		void cboOrderValue_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].DockTimeThrottle = Common.Update(this, _mission.FlightGroups[_activeFG].DockTimeThrottle, Convert.ToInt16(cboOrderValue.SelectedIndex));
		}
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
			_mission.FlightGroups[_activeFG].Waypoints[wpIndex].Enabled = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[wpIndex].Enabled, c.Checked);
			refreshMap(_activeFG);
		}

		void numPitch_Leave(object sender, EventArgs e)
		{
			short Pitch = (short)Math.Round((double)((numPitch.Value >= 270) ? numPitch.Value - 270 : numPitch.Value + 90) / 360 * 256);
			_mission.FlightGroups[_activeFG].Pitch = Common.Update(this, _mission.FlightGroups[_activeFG].Pitch, Pitch);
		}
		void numRoll_Leave(object sender, EventArgs e)
		{
			short Roll = (short)Math.Round((double)numRoll.Value / 360 * 256);
			_mission.FlightGroups[_activeFG].Roll = Common.Update(this, _mission.FlightGroups[_activeFG].Roll, Roll);
		}
		void numYaw_Leave(object sender, EventArgs e)
		{
			short Yaw = (short)Math.Round((double)numYaw.Value / 360 * 256);
			_mission.FlightGroups[_activeFG].Yaw = Common.Update(this, _mission.FlightGroups[_activeFG].Yaw, Yaw);
		}

		void table_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j;
			if (_loading) return;
			_loading = true;
			for (j = 0; j < 10; j++) if (_table.Rows[j].Equals(e.Row)) break;   //find the row index that you're changing
			try
			{
				int wpIndex = _waypointMapping[j];
				for (i = 0; i < 3; i++)
				{
					short raw = (short)(Convert.ToDouble(_table.Rows[j][i]) * 160);
					_mission.FlightGroups[_activeFG].Waypoints[wpIndex][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[wpIndex][i], raw);
					_tableRaw.Rows[j][i] = raw;
				}
			}
			catch { for (i = 0; i < 3; i++) _table.Rows[j][i] = Math.Round((double)_mission.FlightGroups[_activeFG].Waypoints[j][i] / 160, 2); }
			_loading = false;
			refreshMap(_activeFG);
		}
		void tableRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j;
			if (_loading) return;
			_loading = true;
			for (j = 0; j < 10; j++) if (_tableRaw.Rows[j].Equals(e.Row)) break;    //find the row index that you're changing
			try
			{
				int wpIndex = _waypointMapping[j];
				for (i = 0; i < 3; i++)
				{
					short raw = Convert.ToInt16(_tableRaw.Rows[j][i]);
					_mission.FlightGroups[_activeFG].Waypoints[wpIndex][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[wpIndex][i], raw);
					_table.Rows[j][i] = Math.Round((double)raw / 160, 2);
				}
			}
			catch { for (i = 0; i < 3; i++) _tableRaw.Rows[j][i] = Convert.ToInt16(_mission.FlightGroups[_activeFG].Waypoints[j][i]); }
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

			for (int j = 1; j <= 2; j++)
			{
				int wpIndex = _waypointMapping[j];

				//Enable the Start Points.  Not really necessary since X-wing will always choose a randomized point, but this offers display consistency.
				_mission.FlightGroups[_activeFG].Waypoints[wpIndex][3] = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[wpIndex][3], (short)1);
				_mission.FlightGroups[_activeFG].Waypoints[wpIndex].Enabled = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[wpIndex].Enabled, true);

				for (int i = 0; i < 3; i++)
				{
					_mission.FlightGroups[_activeFG].Waypoints[wpIndex][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[wpIndex][i], row[i]);
					_tableRaw.Rows[j][i] = row[i];
					_table.Rows[j][i] = Math.Round((double)row[i] / 160, 2);
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