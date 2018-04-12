/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2017 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.4.1+
 */

/* CHANGELOG
 * [ADD #19] TriggerType unknowns (via JeremyAnsel)
 * v1.4.1, 171118
 * [UPD] added Exclamation icon to FG delete confirmation
 * [UPD] omitted Backdrops from craftStart
 * [NEW #13] SBD implementation
 * [FIX] EnableBackdrops(false) now resets Special Cargo visibility properly
 * [FIX] numExplode toggled in EnableBackdrops()
 * v1.4, 171016
 * [NEW #10] Custom ship list loading
 * v1.3, 170107
 * [UPD] added Ion Pulse [JB]
 * [NEW] MRU capability [JB]
 * [NEW] Delete menu item [JB]
 * [NEW] FG Goal Summary [JB]
 * [FIX] various crashes [JB]
 * [FIX] typos [JB]
 * [FIX] various trigger indexes [JB]
 * [FIX] redid opnXWA process [JB]
 * [FIX] Special Cargo assignment [JB]
 * [FIX] copy/paste issues [JB]
 * [FIX] added missing Active Sequence handlers [JB]
 * v1.2.8, 160606
 * [FIX] WaitForExit in Test replaced with named process check loop (Steam's fault)
 * v1.2.6, 150209
 * [FIX #6] Exit/Save confirmation was shown twice
 * [FIX] Initialized cboMessPara so it wouldn't crash on initial message
 * v1.2.5, 150110
 * [UPD #3] modified Common.Update calls for generics
 * v1.2.3, 141214
 * [UPD] change to MPL
 * [FIX] blank form when trying to use LstForm when TIE isn't installed
 * v1.2, 121006
 * - removed try{} in opnXWA_FileOk
 * - Settings passed in and out
 * [NEW] Test menu
 * [UPD] lblStarting now only applies to Normal difficulty
 * [UPD] opn/sav dialogs default to \MISSIONS
 * [NEW] Open Recent menu
 * v1.1.1, 120814
 * [FIX] Unhandled exception when switching Order Regions
 * [UPD] chkWPArr_Leave to chkWPArr_CheckedChanged
 * - renamed a ton of stuff
 * - class renamed
 * v1.0, 110921
 * - Release
 */

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Idmr.Platform.Xwa;

namespace Idmr.Yogeme
{
	/// <summary>XWA Mission Editor GUI</summary>
	public partial class XwaForm : Form
	{
		#region vars and stuff
		Settings _config;
		bool _loading;
		MapForm _fMap;
		BriefingForm _fBrief;
		LstForm _fLST;
		Mission _mission;
		bool _applicationExit;
		int _activeFG = 0;
		int _startingShips = 1;
		string[] _iffs;
		int _activeMessage = 0;
		DataTable _tableWP = new DataTable("Waypoints");
		DataTable _tableWPRaw = new DataTable("Waypoints_Raw");
		DataTable _tableOrder = new DataTable("Orders");
		DataTable _tableOrderRaw = new DataTable("Orders_Raw");
		byte _activeMessageTrigger = 0;
		byte _activeGlobalTrigger = 0;
		byte _activeTeam = 0;
		byte _activeArrDepTrigger = 0;
		byte _activeFGGoal = 0;
		byte _activeOrder = 0;
		byte _activeOptionCraft = 0;
		#endregion
		#region control arrays
		MenuItem[] menuRecentMissions = new MenuItem[6];
		// FG AD
		Label[] lblADTrig = new Label[6];
		RadioButton[] optADAndOr = new RadioButton[8];
		// FG Goals
		Label[] lblGoal = new Label[8];
		// FG WP
		CheckBox[] chkWP = new CheckBox[22];
		// FG Order
		Label[] lblOrder = new Label[4];
		// FG options
		Label[] lblOptCraft = new Label[10];
		CheckBox[] chkOpt = new CheckBox[16];  //[JB] Added Ion Pulse
		// Mess
		Label[] lblMessTrig = new Label[6];
		RadioButton[] optMessAndOr = new RadioButton[8];
		CheckBox[] chkSendTo = new CheckBox[10];
		// Glob
		Label[] lblGlobTrig = new Label[12];
		RadioButton[] optGlobAndOr = new RadioButton[18];
		// Team
		Label[] lblTeam = new Label[10];
		RadioButton[] optAllies = new RadioButton[30];
		TextBox[] txtEoM = new TextBox[6];
		NumericUpDown[] numTeamUnk = new NumericUpDown[6];
		// Miss2
		Label[] lblGG = new Label[16];
		TextBox[] txtIFFs = new TextBox[4];
		TextBox[] txtRegions = new TextBox[4];
		#endregion

		public XwaForm(Settings settings)
		{
			_config = settings;
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		public XwaForm(Settings settings, string path)
		{
			_config = settings;
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			if (!loadMission(path)) return;
			lstFG.SelectedIndex = 0;
			if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			_loading = false;
		}

		void closeForms()
		{
			try { _fMap.Close(); }
			catch { /* do nothing */ }
			try { _fBrief.Close(); }
			catch { /* do nothing */ }
			try { _fLST.Close(); }
			catch { /* do nothing */ }
		}
		void comboVarRefresh(int index, ComboBox cbo)
		{
			if (index == -1) return;
			cbo.Items.Clear();
			switch (index)		//switch (VariableType)
			{
				case 0:
					cbo.Items.Add("None");
					break;
				case 1: // Flight Group
					cbo.Items.AddRange(_mission.FlightGroups.GetList());
					break;
				case 2: // Ship Type
					cbo.Items.AddRange(Strings.CraftType);
					cbo.Items.RemoveAt(0);
					break;
				case 3: // Ship Class
					cbo.Items.AddRange(Strings.ShipClass);
					break;
				case 4: // Object Type
					cbo.Items.AddRange(Strings.ObjectType);
					break;
				case 5: // IFF
					cbo.Items.AddRange(Strings.IFF);
					break;
				case 6: // Ship Orders
					cbo.Items.AddRange(Strings.Orders);
					break;
				case 7: // Craft when
					cbo.Items.AddRange(Strings.CraftWhen);
					break;
				//case 8: Global Group
				//since it's just numbers, same as default, left out for specifics
				case 9: // Rating
					cbo.Items.AddRange(Strings.Rating);
					break;
				case 10: // Craft with status
					cbo.Items.AddRange(Strings.Status);
					break;
				//case 11: // All
				case 12: // Team
					cbo.Items.AddRange(_mission.Teams.GetList());
					break;
				//case 13: Player
				//case 14: After delay
				case 15: // All Flight Groups except
					cbo.Items.AddRange(_mission.FlightGroups.GetList());
					break;
				case 16: // All ship types except
					cbo.Items.AddRange(Strings.CraftType);
					cbo.Items.RemoveAt(0);
					break;
				case 17: // All ship classes except
					cbo.Items.AddRange(Strings.ShipClass);
					break;
				case 18: // All object types except
					cbo.Items.AddRange(Strings.ObjectType);
					break;
				case 19: // All IFFs except
					cbo.Items.AddRange(Strings.IFF);
					break;
				//case 20: // All Global Groups except
				case 21: // All Teams except
					cbo.Items.AddRange(_mission.Teams.GetList());
					break;
				//case 22: // All players except
				//case 23: // Global Unit
				//case 24: // All Global Units except
				//case 25: // Global Cargo
				//case 26: // All Global Cargos except
				//case 27: // Message #
				default:
					string[] temp = new string[256];
					for (int i=0;i<256;i++) temp[i] = i.ToString();
					cbo.Items.AddRange(temp);
					break;
			}
		}
		void comboReset(ComboBox cbo, string[] items, int index)
		{
			cbo.Items.Clear();
			cbo.Items.AddRange(items);
			cbo.SelectedIndex = index;
		}
		void craftStart(FlightGroup fg, bool bAdd)
		{
			if (fg.Difficulty == 1 || fg.Difficulty == 3 || fg.Difficulty == 6 || !fg.ArrivesIn30Seconds || fg.CraftType == 0xB7) return;
			if (bAdd) _startingShips += fg.NumberOfCraft;
			else _startingShips -= fg.NumberOfCraft;
			lblStarting.Text = _startingShips.ToString() + " craft at 30 seconds";
			if (_startingShips > Mission.CraftLimit) lblStarting.ForeColor = Color.Red;
			else lblStarting.ForeColor = SystemColors.ControlText;
		}
		void initializeMission()
		{
			tabMain.Focus(); //[JB] Exit focus from any form controls.  Fixes some crashes when Leave() events are processed after mission data has been cleared (notably from within the Messages tab).
			_mission = new Mission();
			_config.LastMission = "";
			_activeFG = 0;
			_activeMessage = 0;
			_mission.FlightGroups[0].CraftType = Convert.ToByte(_config.XwaCraft);
			_mission.FlightGroups[0].IFF = Convert.ToByte(_config.XwaIff);
			if (_config.SuperBackdropsInstalled &&_config.InitializeUsingSuperBackdrops) menuSuperBackdrops_Click("initializeMission()", new EventArgs());
			string[] fgList = _mission.FlightGroups.GetList();
			comboReset(cboArrMS, fgList, 0);
			comboReset(cboArrMSAlt, fgList, 0);
			comboReset(cboDepMS, fgList, 0);
			comboReset(cboDepMSAlt, fgList, 0);
			lstFG.Items.Clear();
			for (int i = 0; i < fgList.Length; i++) lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
			comboReset(cboTeam, _mission.Teams.GetList(), _mission.FlightGroups[0].Team);
			cboGlobalTeam.Items.Clear();
			cboGlobalTeam.Items.AddRange(_mission.Teams.GetList());
			Text = "Ye Olde Galactic Empire Mission Editor - XWA - New Mission";
			cboMessFG.Items.Clear();
			cboMessFG.Items.AddRange(fgList);
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			if (!_config.XwaInstalled) cmdBackdrop.Enabled = false;
			_applicationExit = true;	//becomes false if selecting "New Mission" from menu
		}
		void labelRefresh(Mission.Trigger trigger, Label lbl)
		{
			string triggerText = trigger.ToString();
			triggerText = replaceTargetText(triggerText);
			lbl.Text = triggerText;
		}
		string replaceTargetText(string text)
		{
			while (text.Contains("FG:"))
			{
				int index = text.IndexOf("FG:") + 3;
				int length = text.IndexOfAny(new char[] { ' ', ',', '\0' }, index) - index;
				int fg;
				if (length > 0) fg = Int32.Parse(text.Substring(index, length));
				else fg = Int32.Parse(text.Substring(index));
				text = text.Replace("FG:" + fg, _mission.FlightGroups[fg].ToString());
			}
			while (text.Contains("TM:"))
			{
				int index = text.IndexOf("TM:") + 3;
				int length = text.IndexOfAny(new char[] { ' ', ',', '\0' }, index) - index;
				int team;
				if (length > 0) team = Int32.Parse(text.Substring(index, length));
				else team = Int32.Parse(text.Substring(index));
				text = text.Replace("TM:" + team, (_mission.Teams[team].Name == "" ? "Team " + team : _mission.Teams[team].Name));
			}
			return text;
		}
		bool loadMission(string fileMission)
		{
			closeForms();
			lstFG.SelectedIndex = 0;  //[JB] Prevents sporadic index out of range exceptions (such as when opening mission with fewer FGs than the current selected index, or opening missions of a different platform)
			lstFG.Items.Clear();
			lstMessages.Items.Clear();
			_startingShips = 0;
			try
			{
				FileStream fs = File.OpenRead(fileMission);
				try
				{
					#region determine platform
					switch (Platform.MissionFile.GetPlatform(fs))
					{
						case Platform.MissionFile.Platform.TIE:
							_applicationExit = false;
							new TieForm(_config, fileMission).Show();
							Close();
							return false;
						case Platform.MissionFile.Platform.XvT:
							_applicationExit = false;
							new XvtForm(_config, fileMission).Show();
							Close();
							return false;
						case Platform.MissionFile.Platform.BoP:
							_applicationExit = false;
							new XvtForm(_config, fileMission).Show();
							Close();
							return false;
						case Platform.MissionFile.Platform.XWA:
							break;
						default:
							fs.Close();
							throw new Exception("File is not a valid mission file for any platform, please select an appropriate *.tie file.");
					}
					#endregion
					_mission.LoadFromStream(fs);
					fs.Close();
				}
				catch (Exception x)
				{
					fs.Close();
					MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			for (int i=0;i<_mission.FlightGroups.Count;i++)
			{
				lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
				if (_mission.FlightGroups[i].ArrivesIn30Seconds) craftStart(_mission.FlightGroups[i], true);
			}
			updateFGList();
			if (_mission.Messages.Count == 0) EnableMessages(false);
			else
			{
				EnableMessages(true);
				for (int i=0;i<_mission.Messages.Count;i++) lstMessages.Items.Add(_mission.Messages[i].MessageString);
			}
			UpdateMissionTabs();
			cboGlobalTeam.SelectedIndex = -1;	// otherwise it doesn't trigger an index change
			cboGlobalTeam.SelectedIndex = 0;
			for (_activeTeam=0;_activeTeam<10;_activeTeam++) TeamRefresh();
			_activeTeam = 0;
			lblTeamArr_Click(lblTeam[0], new EventArgs());
			int c = fileMission.LastIndexOf("\\") + 1;
			this.Text = "Ye Olde Galactic Empire Mission Editor - XWA - " + fileMission.Substring(c);
			_config.LastMission = fileMission;
			refreshRecent(); //[JB] Setting _config.LastMission modifies the Recent list.  Need to refresh the menu to match.
			return true;
		}
		void parameterRefresh(ComboBox cbo)
		{
			int index = cbo.SelectedIndex;
			cbo.Items.Clear();
			cbo.Items.Add("");
			for (int i = 0; i < 4; i++)
				cbo.Items.Add((_mission.Regions[i] == "" ? "Region " + (i + 1) : _mission.Regions[i]));
			cbo.Items.AddRange(_mission.FlightGroups.GetList());
			cbo.SelectedIndex = index;
		}
		void promptSave()
		{
			_config.SaveSettings();
			if (_config.ConfirmSave && (this.Text.IndexOf("*") != -1))
			{
				DialogResult res = MessageBox.Show("Mission has been edited without saving, would you like to save?", "Confirm", MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes)
				{
					if (_mission.MissionPath == "\\NewMission.tie") savXWA.ShowDialog();
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
		void saveMission(string fileMission)
		{
			try { _fBrief.Save(); }
			catch { /* do nothing */ }
			lblTeamArr_Click(lblTeam[_activeTeam], new EventArgs());	// forces an update
			try { _mission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			this.Text = "Ye Olde Galactic Empire Mission Editor - XWA - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
   			refreshRecent();  //[JB] Setting _config.LastMission modifies the Recent list.  Need to refresh the menu to match.
			//Verify the mission after it's been saved
			if (_config.Verify) Common.RunVerify(_mission.MissionPath, _config.VerifyLocation);
		}
		void startup()
		{
			if (File.Exists(Application.StartupPath + "\\xwa_shiplist.txt"))
			{
				System.Diagnostics.Debug.WriteLine("custom XWA list found");
				string[] crafts;
				string[] abbrvs;
				try
				{
					Common.ProcessCraftList(Application.StartupPath + "\\xwa_shiplist.txt", out crafts, out abbrvs);
					Strings.OverrideShipList(crafts, abbrvs);
					initializeMission();    // have to re-init since lstFG is already populated
				}
				catch { MessageBox.Show("Error processing custom XWA ship list, using defaults.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			}
			_config.LastPlatform = Settings.Platform.XWA;
			_applicationExit = true;	//becomes false if selecting "New Mission" from menu
			opnXWA.InitialDirectory = _config.GetWorkingPath(); //[JB] Updated for MRU access.  Defaults to installation and mission folder if not enabled.
			savXWA.InitialDirectory = _config.GetWorkingPath();
			_iffs = Strings.IFF;
			#region Menu
			menuText.Enabled = _config.XwaInstalled;
			if (_config.RestrictPlatforms)
			{
				if (!_config.TieInstalled) { menuNewTIE.Enabled = false; }
				if (!_config.XvtInstalled) { menuNewXvT.Enabled = false; }
				if (!_config.BopInstalled) { menuNewBoP.Enabled = false; }
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
			menuSuperBackdrops.Enabled = _config.SuperBackdropsInstalled;
			refreshRecent();
			#endregion
			#region FlightGroups
			#region Craft
			cboCraft.Items.AddRange(Strings.CraftType); cboCraft.SelectedIndex = _mission.FlightGroups[0].CraftType;
			cboIFF.Items.AddRange(Strings.IFF); cboIFF.SelectedIndex = _mission.FlightGroups[0].IFF;
			cboAI.Items.AddRange(Strings.Rating); cboAI.SelectedIndex = _mission.FlightGroups[0].AI;
			cboMarkings.Items.AddRange(Strings.Color); cboMarkings.SelectedIndex = _mission.FlightGroups[0].Markings;
			cboPlayer.SelectedIndex = 0;
			cboPosition.SelectedIndex = 0;
			cboFormation.Items.AddRange(Strings.Formation); cboFormation.SelectedIndex = 0;
			cboRadio.Items.AddRange(Strings.Radio); cboRadio.SelectedIndex = _mission.FlightGroups[0].Radio;
			cboStatus.Items.AddRange(Strings.Status); cboStatus.SelectedIndex = 0;
			cboStatus2.Items.AddRange(Strings.Status); cboStatus2.SelectedIndex = 0;
			cboWarheads.Items.AddRange(Strings.Warheads); cboWarheads.SelectedIndex = 0;
			cboBeam.Items.AddRange(Strings.Beam); cboBeam.SelectedIndex = 0;
			cboCounter.SelectedIndex = 0;
			cboGlobCargo.Items.Add("None");
			for (int i=0;i<16;i++) cboGlobCargo.Items.Add("Global Cargo " + (i+1).ToString());  //[JB] Fixed typo "Gobal"
			cboGlobCargo.SelectedIndex = 0;
			cboGlobSpecCargo.Items.Add("None");
			for (int i=0;i<16;i++) cboGlobSpecCargo.Items.Add("Global Cargo " + (i+1).ToString());
			cboGlobSpecCargo.SelectedIndex = 0;
			#endregion
			#region Arr/Dep
			cboADTrig.Items.AddRange(Strings.Trigger);
			cboADTrigAmount.Items.AddRange(Strings.Amount);
			cboADTrigType.Items.AddRange(Strings.VariableType);
			cboAbort.Items.AddRange(Strings.Abort); cboAbort.SelectedIndex = 0;
			cboDiff.Items.AddRange(Strings.Difficulty); cboDiff.SelectedIndex = 0;
			lblADTrig[0] = lblArr1;
			lblADTrig[1] = lblArr2;
			lblADTrig[2] = lblArr3;
			lblADTrig[3] = lblArr4;
			lblADTrig[4] = lblDep1;
			lblADTrig[5] = lblDep2;
			for (int i=0;i<6;i++)
			{
				lblADTrig[i].Click += new EventHandler(lblADTrigArr_Click);
				lblADTrig[i].MouseUp += new MouseEventHandler(lblADTrigArr_MouseUp);
				lblADTrig[i].DoubleClick += new EventHandler(lblADTrigArr_DoubleClick);
				lblADTrig[i].Tag = i;
			}
			parameterRefresh(cboADPara);
			optADAndOr[0] = optArr1OR2;
			optADAndOr[1] = optArr3OR4;
			optADAndOr[2] = optArr12OR34;
			optADAndOr[3] = optDepOR;
			optADAndOr[4] = optArr1AND2;
			optADAndOr[5] = optArr3AND4;
			optADAndOr[6] = optArr12AND34;
			optADAndOr[7] = optDepAND;
			for (int i=0;i<4;i++)
			{
				optADAndOr[i].CheckedChanged += new EventHandler(optADAndOrArr_CheckedChanged);
				optADAndOr[i].Tag = i;
			}
			#endregion
			#region Orders
			cboOrders.Items.AddRange(Strings.Orders);
			cboOT1Type.Items.AddRange(Strings.VariableType);
			cboOT2Type.Items.AddRange(Strings.VariableType);
			cboOT3Type.Items.AddRange(Strings.VariableType);
			cboOT4Type.Items.AddRange(Strings.VariableType);
			lblOrder[0] = lblOrder1;
			lblOrder[1] = lblOrder2;
			lblOrder[2] = lblOrder3;
			lblOrder[3] = lblOrder4;
			for (int i=0;i<4;i++)
			{
				lblOrder[i].Click += new EventHandler(lblOrderArr_Click);
				lblOrder[i].DoubleClick += new EventHandler(lblOrderArr_DoubleClick);
				lblOrder[i].MouseUp += new MouseEventHandler(lblOrderArr_MouseUp);
				lblOrder[i].Tag = i;
			}
			#endregion
			#region Waypoints
			_tableWP.Columns.Add("X"); _tableWP.Columns.Add("Y"); _tableWP.Columns.Add("Z");
			_tableWPRaw.Columns.Add("X"); _tableWPRaw.Columns.Add("Y"); _tableWPRaw.Columns.Add("Z");
			for (int i=0;i<4;i++)	//initialize WPs
			{
				DataRow dr = _tableWP.NewRow();
				int j;
				for (j=0;j<3;j++) dr[j] = 0;	//set X Y Z to zero
				_tableWP.Rows.Add(dr);
				dr = _tableWPRaw.NewRow();
				for (j=0;j<3;j++) dr[j] = 0;	//mirror in raw table
				_tableWPRaw.Rows.Add(dr);
			}
			_tableOrder.Columns.Add("X"); _tableOrder.Columns.Add("Y"); _tableOrder.Columns.Add("Z");
			_tableOrderRaw.Columns.Add("X"); _tableOrderRaw.Columns.Add("Y"); _tableOrderRaw.Columns.Add("Z");
			for (int i=0;i<8;i++)	//initialize WPs
			{
				DataRow dr = _tableOrder.NewRow();
				int j;
				for (j=0;j<3;j++) dr[j] = 0;	//set X Y Z to zero
				_tableOrder.Rows.Add(dr);
				dr = _tableOrderRaw.NewRow();
				for (j=0;j<3;j++) dr[j] = 0;	//mirror in raw table
				_tableOrderRaw.Rows.Add(dr);
			}
			dataWaypoints.Table = _tableWP;
			dataWaypoints_Raw.Table = _tableWPRaw;
			dataOrders.Table = _tableOrder;
			dataOrders_Raw.Table = _tableOrderRaw;
			dataWP.DataSource = dataWaypoints;
			dataWP_Raw.DataSource = dataWaypoints_Raw;
			dataO.DataSource = dataOrders;
			dataO_Raw.DataSource = dataOrders_Raw;
			this._tableWP.RowChanged += new DataRowChangeEventHandler(tableWP_RowChanged);
			this._tableWPRaw.RowChanged += new DataRowChangeEventHandler(tableWPRaw_RowChanged);
			this._tableOrder.RowChanged += new DataRowChangeEventHandler(tableOrder_RowChanged);
			this._tableOrderRaw.RowChanged += new DataRowChangeEventHandler(tableOrderRaw_RowChanged);
			chkWP[0] = chkSP1;
			chkWP[1] = chkSP2;
			chkWP[2] = chkSP3;
			chkWP[3] = chkWPHyp;
			chkWP[4] = chkWP1;
			chkWP[5] = chkWP2;
			chkWP[6] = chkWP3;
			chkWP[7] = chkWP4;
			chkWP[8] = chkWP5;
			chkWP[9] = chkWP6;
			chkWP[10] = chkWP7;
			chkWP[11] = chkWP8;
			for (int i=0;i<12;i++)
			{
				chkWP[i].CheckedChanged += new EventHandler(chkWPArr_CheckedChanged);
				chkWP[i].Tag = i;
			}
			#endregion
			#region FG Goals
			cboGoalAmount.Items.AddRange(Strings.Amount);
			cboGoalTrigger.Items.AddRange(Strings.Trigger);
			lblGoal[0] = lblGoal1;
			lblGoal[1] = lblGoal2;
			lblGoal[2] = lblGoal3;
			lblGoal[3] = lblGoal4;
			lblGoal[4] = lblGoal5;
			lblGoal[5] = lblGoal6;
			lblGoal[6] = lblGoal7;
			lblGoal[7] = lblGoal8;
			for (int i=0;i<8;i++)
			{
				lblGoal[i].Click += new EventHandler(lblGoalArr_Click);
				lblGoal[i].DoubleClick += new EventHandler(lblGoalArr_DoubleClick);
				lblGoal[i].MouseUp += new MouseEventHandler(lblGoalArr_MouseUp);
				lblGoal[i].Tag = i;
			}
			parameterRefresh(cboGoalPara);
			#endregion
			#region Options
			cboOptCraft.Items.AddRange(Strings.CraftType);
			cboSkipAmount.Items.AddRange(Strings.Amount);
			cboSkipTrig.Items.AddRange(Strings.Trigger);
			cboSkipType.Items.AddRange(Strings.VariableType);
			parameterRefresh(cboSkipPara);
			cboSkipOrder.SelectedIndex = 0;
			cboRole1.Items.AddRange(Strings.Roles); cboRole1.SelectedIndex = 0;
			cboRole2.Items.AddRange(Strings.Roles); cboRole2.SelectedIndex = 0;
			chkOpt[0] = chkOptWNone;
			chkOpt[1] = chkOptWBomb;
			chkOpt[2] = chkOptWRocket;
			chkOpt[3] = chkOptWMissile;
			chkOpt[4] = chkOptWTorp;
			chkOpt[5] = chkOptWAdvMissile;
			chkOpt[6] = chkOptWAdvTorp;
			chkOpt[7] = chkOptWMagPulse;
			chkOpt[8] = chkOptWIonPulse; //[JB] Added Ion Pulse, others moved down one index.
			chkOpt[9] = chkOptBNone;
			chkOpt[10] = chkOptBTractor;
			chkOpt[11] = chkOptBJamming;
			chkOpt[12] = chkOptBDecoy;
			chkOpt[13] = chkOptCNone;
			chkOpt[14] = chkOptCChaff;
			chkOpt[15] = chkOptCFlare;
			for (int i=0;i<16;i++)
			{
				chkOpt[i].CheckedChanged += new EventHandler(chkOptArr_CheckedChanged);
				chkOpt[i].Tag = i;
			}
			lblOptCraft[0] = lblOptCraft1;
			lblOptCraft[1] = lblOptCraft2;
			lblOptCraft[2] = lblOptCraft3;
			lblOptCraft[3] = lblOptCraft4;
			lblOptCraft[4] = lblOptCraft5;
			lblOptCraft[5] = lblOptCraft6;
			lblOptCraft[6] = lblOptCraft7;
			lblOptCraft[7] = lblOptCraft8;
			lblOptCraft[8] = lblOptCraft9;
			lblOptCraft[9] = lblOptCraft10;
			for (int i=0;i<10;i++)
			{
				lblOptCraft[i].Click += new EventHandler(lblOptCraftArr_Click);
				lblOptCraft[i].Tag = i;
			}
			#endregion
			#endregion
			#region Messages
			cboMessAmount.Items.AddRange(Strings.Amount);
			cboMessTrig.Items.AddRange(Strings.Trigger);
			cboMessType.Items.AddRange(Strings.VariableType);
			parameterRefresh(cboMessPara);
			chkSendTo[0] = chkMess1;
			chkSendTo[1] = chkMess2;
			chkSendTo[2] = chkMess3;
			chkSendTo[3] = chkMess4;
			chkSendTo[4] = chkMess5;
			chkSendTo[5] = chkMess6;
			chkSendTo[6] = chkMess7;
			chkSendTo[7] = chkMess8;
			chkSendTo[8] = chkMess9;
			chkSendTo[9] = chkMess10;
			for (int i=0;i<10;i++)
			{
				chkSendTo[i].Leave += new EventHandler(chkSendToArr_Leave);
				chkSendTo[i].Tag = i;
			}
			lblMessTrig[0] = lblMess1;
			lblMessTrig[1] = lblMess2;
			lblMessTrig[2] = lblMess3;
			lblMessTrig[3] = lblMess4;
			lblMessTrig[4] = lblMess5;
			lblMessTrig[5] = lblMess6;
			for (int i=0;i<6;i++)
			{
				lblMessTrig[i].Click += new EventHandler(lblMessTrigArr_Click);
				lblMessTrig[i].DoubleClick += new EventHandler(lblMessTrigArr_DoubleClick);
				lblMessTrig[i].MouseUp += new MouseEventHandler(lblMessTrigArr_MouseUp);
				lblMessTrig[i].Tag = i;
			}
			optMessAndOr[0] = optMess1OR2;
			optMessAndOr[1] = optMess3OR4;
			optMessAndOr[2] = optMess12OR34;
			optMessAndOr[3] = optMessC1OR2;
			optMessAndOr[4] = optMess1AND2;
			optMessAndOr[5] = optMess3AND4;
			optMessAndOr[6] = optMess12AND34;
			optMessAndOr[7] = optMessC1AND2;
			for (int i=0;i<4;i++)
			{
				optMessAndOr[i].CheckedChanged += new EventHandler(optMessAndOrArr_CheckedChanged);
				optMessAndOr[i].Tag = i;
			}
			#endregion
			#region Globals
			cboGlobalAmount.Items.AddRange(Strings.Amount);
			cboGlobalTrig.Items.AddRange(Strings.Trigger);
			cboGlobalType.Items.AddRange(Strings.VariableType);
			lblGlobTrig[0] = lblPrim1;
			lblGlobTrig[1] = lblPrim2;
			lblGlobTrig[2] = lblPrim3;
			lblGlobTrig[3] = lblPrim4;
			lblGlobTrig[4] = lblPrev1;
			lblGlobTrig[5] = lblPrev2;
			lblGlobTrig[6] = lblPrev3;
			lblGlobTrig[7] = lblPrev4;
			lblGlobTrig[8] = lblSec1;
			lblGlobTrig[9] = lblSec2;
			lblGlobTrig[10] = lblSec3;
			lblGlobTrig[11] = lblSec4;
			for (int i=0;i<12;i++)
			{
				lblGlobTrig[i].Click += new EventHandler(lblGlobTrigArr_Click);
				lblGlobTrig[i].DoubleClick += new EventHandler(lblGlobTrigArr_DoubleClick);
				lblGlobTrig[i].MouseUp += new MouseEventHandler(lblGlobTrigArr_MouseUp);
				lblGlobTrig[i].Tag = i;
			}
			optGlobAndOr[0] = optPrim1OR2;
			optGlobAndOr[2] = optPrim3OR4;
			optGlobAndOr[4] = optPrim12OR34;
			optGlobAndOr[6] = optPrev1OR2;
			optGlobAndOr[8] = optPrev3OR4;
			optGlobAndOr[10] = optPrev12OR34;
			optGlobAndOr[12] = optSec1OR2;
			optGlobAndOr[14] = optSec3OR4;
			optGlobAndOr[16] = optSec12OR34;
			optGlobAndOr[1] = optPrim1AND2;
			optGlobAndOr[3] = optPrim3AND4;
			optGlobAndOr[5] = optPrim12AND34;
			optGlobAndOr[7] = optPrev1AND2;
			optGlobAndOr[9] = optPrev3AND4;
			optGlobAndOr[11] = optPrev12AND34;
			optGlobAndOr[13] = optSec1AND2;
			optGlobAndOr[15] = optSec3AND4;
			optGlobAndOr[17] = optSec12AND34;
			for (int i = 0; i < 9; i++)
			{
				optGlobAndOr[i*2].CheckedChanged += new EventHandler(optGlobAndOrArr_CheckedChanged);	// only for evens
				optGlobAndOr[i*2].Tag = i*2;
			}
			#endregion
			#region Teams
			lblTeam[0] = lblTeam1;
			lblTeam[1] = lblTeam2;
			lblTeam[2] = lblTeam3;
			lblTeam[3] = lblTeam4;
			lblTeam[4] = lblTeam5;
			lblTeam[5] = lblTeam6;
			lblTeam[6] = lblTeam7;
			lblTeam[7] = lblTeam8;
			lblTeam[8] = lblTeam9;
			lblTeam[9] = lblTeam10;
			for (int i=0;i<10;i++)
			{
				lblTeam[i].Click += new EventHandler(lblTeamArr_Click);
				lblTeam[i].Tag = i;
			}
			txtEoM[0] = txtPrimComp1;
			txtEoM[1] = txtPrimComp2;
			txtEoM[2] = txtPrimFail1;
			txtEoM[3] = txtPrimFail2;
			txtEoM[4] = txtSecComp1;
			txtEoM[5] = txtSecComp2;
			optAllies[0] = optAllies1;
			optAllies[1] = optAllies2;
			optAllies[2] = optAllies3;
			optAllies[3] = optAllies4;
			optAllies[4] = optAllies5;
			optAllies[5] = optAllies6;
			optAllies[6] = optAllies7;
			optAllies[7] = optAllies8;
			optAllies[8] = optAllies9;
			optAllies[9] = optAllies10;
			optAllies[10] = optAllies11;
			optAllies[11] = optAllies12;
			optAllies[12] = optAllies13;
			optAllies[13] = optAllies14;
			optAllies[14] = optAllies15;
			optAllies[15] = optAllies16;
			optAllies[16] = optAllies17;
			optAllies[17] = optAllies18;
			optAllies[18] = optAllies19;
			optAllies[19] = optAllies20;
			optAllies[20] = optAllies21;
			optAllies[21] = optAllies22;
			optAllies[22] = optAllies23;
			optAllies[23] = optAllies24;
			optAllies[24] = optAllies25;
			optAllies[25] = optAllies26;
			optAllies[26] = optAllies27;
			optAllies[27] = optAllies28;
			optAllies[28] = optAllies29;
			optAllies[29] = optAllies30;
			for (int i=0;i<30;i++)
			{
				optAllies[i].CheckedChanged += new EventHandler(optAlliesArr_CheckedChanged);
				optAllies[i].Tag = i;
			}
			numTeamUnk[0] = numTeamUnk1;
			numTeamUnk[1] = numTeamUnk2;
			numTeamUnk[2] = numTeamUnk3;
			numTeamUnk[3] = numTeamUnk4;
			numTeamUnk[4] = numTeamUnk5;
			numTeamUnk[5] = numTeamUnk6;
			#endregion
			parameterRefresh(cboGlobalPara);
			cboGlobalTeam.SelectedIndex = 0;
			cboHangar.Items.AddRange(Enum.GetNames(typeof(Mission.HangarEnum)));
			cboOfficer.Items.AddRange(Strings.Officer);
			cboLogo.Items.AddRange(Enum.GetNames(typeof(Mission.LogoEnum)));
			#region Mission2
			lblGG[0] = lblGG1;
			lblGG[1] = lblGG2;
			lblGG[2] = lblGG3;
			lblGG[3] = lblGG4;
			lblGG[4] = lblGG5;
			lblGG[5] = lblGG6;
			lblGG[6] = lblGG7;
			lblGG[7] = lblGG8;
			lblGG[8] = lblGG9;
			lblGG[9] = lblGG10;
			lblGG[10] = lblGG11;
			lblGG[11] = lblGG12;
			lblGG[12] = lblGG13;
			lblGG[13] = lblGG14;
			lblGG[14] = lblGG15;
			lblGG[15] = lblGG16;
			for (int i=0;i<16;i++)
			{
				lblGG[i].Click += new EventHandler(lblGGArr_Click);
				lblGG[i].Tag = i;
			}
			lblGGArr_Click(lblGG[0], new EventArgs());
			txtIFFs[0] = txtIFF3;
			txtIFFs[1] = txtIFF4;
			txtIFFs[2] = txtIFF5;
			txtIFFs[3] = txtIFF6;
			txtRegions[0] = txtRegion1;
			txtRegions[1] = txtRegion2;
			txtRegions[2] = txtRegion3;
			txtRegions[3] = txtRegion4;
			for (int i=0;i<4;i++)
			{
				txtIFFs[i].Leave += new EventHandler(txtIFFsArr_Leave);
				txtIFFs[i].Tag = i+2;
				txtRegions[i].Leave += new EventHandler(txtRegionsArr_Leave);
				txtRegions[i].Tag = i;
			}
			#endregion
			UpdateMissionTabs();
		
			//[JB] Added
			//this.KeyPreview = true;  //Property is already set in .designer
			this.KeyDown += new KeyEventHandler(XwaForm_KeyDown);
		}

		void frmXWA_Activated(object sender, EventArgs e)
		{
			if (_fMap != null)
			{
				lstFG.SelectedIndex = -1;
				lstFG.SelectedIndex = _activeFG;
			}
		}
		void frmXWA_Closing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.ApplicationExitCall) return;
			promptSave();
			if (_config.ConfirmExit && _applicationExit)
			{
				DialogResult res = MessageBox.Show("Are you sure you wish to exit?", "Confirm", MessageBoxButtons.YesNo);
				if (res == DialogResult.No) { e.Cancel = true; return; }
			}
			closeForms();
			if (_applicationExit) Application.Exit();
		}
		void XwaForm_KeyDown(object sender, KeyEventArgs e)
		{
			//Instead of assigning this in designer.cs
			//  this.menuDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
			//We trap and process the key here, with few changes to the original code
			//while allowing the delete key to remain function in other areas like text boxes.

			if (e.KeyCode == Keys.Delete)
			{
				if (lstFG.Focused || lstMessages.Focused)
				{
					menuDelete_Click(0, new EventArgs());
					e.Handled = true;
				}
			}
		}

		void opnXWA_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_loading = true;
			_mission.MissionPath = opnXWA.FileName;
			if (loadMission(_mission.MissionPath))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				lstFG.SelectedIndex = 0;
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_loading = false;
		}

		void savXWA_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_mission.MissionPath = savXWA.FileName;
			saveMission(_mission.MissionPath);
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
					toolNewItem.ToolTipText = "New Message";
					toolNewItem.Enabled = true;
					toolDeleteItem.ToolTipText = "Delete Message";
					toolDeleteItem.Enabled = true;
					toolCopy.ToolTipText = "Copy Message";
					toolPaste.ToolTipText = "Paste Message";
					break;
				case 2:
					toolCopy.ToolTipText = "Copy selected Global Goal";
					toolPaste.ToolTipText = "Paste into selected Global Goal";
					toolNewItem.Enabled = false;
					toolDeleteItem.Enabled = false;
					toolNewItem.ToolTipText = "";
					toolDeleteItem.ToolTipText = "";
					break;
				case 3:
					toolCopy.ToolTipText = "Copy selected Team";
					toolPaste.ToolTipText = "Paste into selected Team";
					toolNewItem.Enabled = false;
					toolDeleteItem.Enabled = false;
					toolNewItem.ToolTipText = "";
					toolDeleteItem.ToolTipText = "";
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

		void toolXWA_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			toolXWA.Focus();	// clicking the toolbar doesn't remove focus, so last change may not be saved
			switch (toolXWA.Buttons.IndexOf(e.Button))
			{
				case 0:		//New Mission
					menuNewXWA_Click("toolbar", new EventArgs());
					_loading = false;
					break;
				case 1:		//Open Mission
					menuOpen_Click("toolbar", new EventArgs());
					_loading = false;
					break;
				case 2:		//Save Mission
					menuSave_Click("toolbar", new EventArgs());
					break;
				case 3:		//Save As
					savXWA.ShowDialog();
					break;
				case 5:		//New Item
					if (tabMain.SelectedIndex == 0) newFG();
					else if (tabMain.SelectedIndex == 1) NewMess();
					break;
				case 6:		//Delete Item
					menuDelete_Click("toolbar", new EventArgs());  //[JB] Changed to call the function directly since that function now does more conditional checks.
					break;
				case 7:		//Copy Item
					menuCopy_Click("toolbar", new EventArgs());
					break;
				case 8:		//Paste Item
					menuPaste_Click("toolbar", new EventArgs());
					break;
				case 10:	//Map
					menuMap_Click("toolbar", new EventArgs());
					break;
				case 11:	//Briefing
					menuBrief_Click("toolbar", new EventArgs());
					break;
				case 12:	//Verify
					menuVerify_Click("toolbar", new EventArgs());
					break;
				case 14:	//Options
					menuOptions_Click("toolbar", new EventArgs());
					break;
				case 15:	//LST
					menuLST_Click("toolbar", new EventArgs());
					break;
				case 16:	//Help
					menuHelpInfo_Click("toolbar", new EventArgs());
					break;
			}
		}

		#region Menu
		void menuAbout_Click(object sender, EventArgs e)
		{
			new AboutDialog().ShowDialog();
		}
		void menuBrief_Click(object sender, EventArgs e)
		{
			Common.Title(this, false);
			try { _fBrief.Close(); }  //[JB] Prevent opening multiple dialogs. Same as XvT code.
			catch { /* do nothing */ }
			_fBrief = new BriefingForm(_mission.Briefings);
			_fBrief.Show();
		}
		//[JB] Added function for menu item and modified for extra safety checks to prevent deleting when the list controls don't have focus.
		void menuDelete_Click(object sender, EventArgs e)
		{
			if (tabMain.SelectedIndex == 0)
			{
				if((sender.ToString() == "toolbar") || (lstFG.Focused)) deleteFG();
			}
			else if (tabMain.SelectedIndex == 1)
			{
				if((sender.ToString() == "toolbar") || (lstMessages.Focused)) DeleteMess();
			}
		}
		void menuCopy_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream = new FileStream("YOGEME.bin", FileMode.Create, FileAccess.Write, FileShare.None);
			#region ArrDep
			if (sender.ToString() == "AD")
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger]);
				stream.Close();
				return;
			}
			#endregion
			#region FG Goal
			if (sender.ToString() == "Goal")
			{
				//[JB] Fixed copying of goals.  The Goal class is already serializable so don't need any special code for it.
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal]);
				stream.Close();
				return;
			}
			#endregion
			#region Orders
			if (sender.ToString() == "Order")
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Orders[(int)numORegion.Value - 1, _activeOrder % 4]);   //[JB] Fixed copy from the appropriate region.
				stream.Close();
				return;
			}
			#endregion
			#region Skip to Order
			if (sender.ToString() == "Skip")
			{
				int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
				int order = cboSkipOrder.SelectedIndex;  //[JB] Fixed copying
				if(order < 0) order = 0;
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Orders[order/4, order%4].SkipTriggers[i]);  //[JB] Fixed from _activeOrder
				stream.Close();
				return;
			}
			#endregion
			#region generic TextBox
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
			#endregion
			#region MessTrig
			if (sender.ToString() == "MessTrig")
			{
				formatter.Serialize(stream, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger]);
				stream.Close();
				return;
			}
			#endregion

			switch (tabMain.SelectedIndex)
			{
				case 0:
					formatter.Serialize(stream, _mission.FlightGroups[_activeFG]);
					break;
				case 1:
					if (_mission.Messages.Count != 0) formatter.Serialize(stream, _mission.Messages[_activeMessage]);
					break;
				case 2:
					formatter.Serialize(stream, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger/4].Triggers[_activeGlobalTrigger%4]);  //[JB] Was 3, fixed bug where the 4th trigger wouldn't copy
					break;
				case 3:
					formatter.Serialize(stream, _mission.Teams[_activeTeam]);
					break;
				default:
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
			string output = "(global goals not included):\r\n----------\r\n" + generateGoalSummary();
			new GoalSummaryDialog(output).Show();
		}
		void menuHelpInfo_Click(object sender, EventArgs e)
		{
			Common.LaunchHelp();
		}
		void menuHyperbuoy_Click(object sender, EventArgs e)
		{
			lstFG.Focus();
			int fgCount = _mission.FlightGroups.Count;
			new HyperbuoyDialog(_mission).ShowDialog();
			if (_mission.FlightGroups.Count > fgCount)
			{
				for (int i = fgCount; i < _mission.FlightGroups.Count;i++)
				{
					if (_mission.FlightGroups[i].ArrivesIn30Seconds) craftStart(_mission.FlightGroups[i], true);
					lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
				}
				updateFGList();
			}
		}
		void menuIDMR_Click(object sender, EventArgs e)
		{
			Common.LaunchIdmr();
		}
		void menuLST_Click(object sender, EventArgs e)
		{
			_fLST = new LstForm(Settings.Platform.XWA);
			_fLST.Show();
		}
		void menuMap_Click(object sender, EventArgs e)
		{
			try { _fMap.Close(); }
			catch { /* do nothing */ }
			_fMap = new MapForm(_config, _mission.FlightGroups);
			_fMap.Show();
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
			_loading = true;
			initializeMission();
			UpdateMissionTabs();
			lstMessages.Items.Clear();
			EnableMessages(false);
			lblMessage.Text = "Message #0 of 0";
			for (_activeTeam=0;_activeTeam<10;_activeTeam++) TeamRefresh();
			lblTeamArr_Click(lblTeam[0], new EventArgs());
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		void menuOpen_Click(object sender, EventArgs e)
		{
			promptSave();
			opnXWA.FileName = _mission.MissionFileName;
			if(opnXWA.ShowDialog() == DialogResult.OK)  //[JB] Wait until dialog is closed before loading.  Fixes bug where dialog could be stuck open.  Also update paths for last-opened folder. 
			{
				opnXWA_FileOk(0, new System.ComponentModel.CancelEventArgs());
				_config.SetWorkingPath(Path.GetDirectoryName(opnXWA.FileName));
				opnXWA.InitialDirectory = _config.GetWorkingPath(); //Update since folder may have changed
			}
		}
		void menuOptions_Click(object sender, EventArgs e)
		{
			new OptionsDialog(_config).ShowDialog();
		}
		void menuPaste_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream;
			try { stream = new FileStream("YOGEME.bin", FileMode.Open, FileAccess.Read, FileShare.Read); }
			catch { return; }
			#region ArrDep
			if (sender.ToString()== "AD")
			{
				try
				{
					Mission.Trigger trig_temp = (Mission.Trigger)formatter.Deserialize(stream);
					_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger] = trig_temp;
					lblADTrigArr_Click(_activeArrDepTrigger, new EventArgs());
					labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region Orders
			if (sender.ToString() == "Order")
			{
				try
				{
					FlightGroup.Order order_temp = (FlightGroup.Order)formatter.Deserialize(stream);
					// can't check against null, but maybe ^ will trip?
					_mission.FlightGroups[_activeFG].Orders[(int)numORegion.Value - 1, _activeOrder % 4] = order_temp;  //[JB] Fix pasting into regions above 1.  Formerly: _activeOrder / 4
					lblOrderArr_Click(_activeOrder, new EventArgs());
					OrderLabelRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region FG Goals
			if (sender.ToString() == "Goal")
			{
				try
				{
					FlightGroup.Goal goal_temp = (FlightGroup.Goal)formatter.Deserialize(stream);
					// can't compare to null, but maybe ^ will trip?
					_mission.FlightGroups[_activeFG].Goals[_activeFGGoal] = goal_temp;
					lblGoalArr_Click(_activeFGGoal, new EventArgs());
					GoalLabelRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region Skip to Order
			if (sender.ToString() == "Skip")
			{
				try
				{
					Mission.Trigger trig_temp = (Mission.Trigger)formatter.Deserialize(stream);
					int j = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
					int r = cboSkipOrder.SelectedIndex / 4;  //[JB] Fix pasting to orders other than Order 1 Region 1
					int o = cboSkipOrder.SelectedIndex % 4;
					_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[j] = trig_temp;
					lblSkipTrigArr_Click(j, new EventArgs());
					labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[j], (j == 0 ? lblSkipTrig1 : lblSkipTrig2));	// no array, hence explicit naming
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region generic TextBox
			try
			{
				if (ActiveControl.GetType().ToString() == "System.Windows.Forms.TextBox")
				{
					string str_t = formatter.Deserialize(stream).ToString();
					if (str_t.IndexOf(".", 0) != -1) throw new Exception();		// bypass FGs, Mess, Teams, etc
					if (str_t.IndexOf("System.", 0) != -1) throw new Exception();	// bypass byte[]
					TextBox txt_t = (TextBox)ActiveControl;
					txt_t.SelectedText = str_t;
					Common.Title(this, false);
					stream.Close();
					return;
				}
			}
			catch { /* do nothing*/ }
			#endregion
			#region MessTrig
			if (sender.ToString() == "MessTrig")
			{
				try
				{
					Mission.Trigger trig_temp = (Mission.Trigger)formatter.Deserialize(stream);
					_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger] = trig_temp;
					lblMessTrigArr_Click(_activeMessageTrigger, new EventArgs());
					labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region overalls by tab
			switch (tabMain.SelectedIndex)
			{
				case 0:
					try
					{
						FlightGroup fg_temp = (FlightGroup)formatter.Deserialize(stream);
						if (fg_temp == null) throw new Exception();
						newFG();
						_mission.FlightGroups[_activeFG] = fg_temp;
						listRefresh();
						_startingShips--;
						lstFG.SelectedIndex = _activeFG;
						if (_mission.FlightGroups[_activeFG].ArrivesIn30Seconds)
						{
							_startingShips += _mission.FlightGroups[_activeFG].NumberOfCraft;
						}
						lblStarting.Text = _startingShips.ToString() + " craft at 30 seconds";
					}
					catch { /* do nothing */ }
					break;
				case 1:
					try
					{
						Platform.Xwa.Message mess_temp = (Platform.Xwa.Message)formatter.Deserialize(stream);
						if (mess_temp == null) throw new Exception();
						NewMess();
						_mission.Messages[_activeMessage] = mess_temp;
						MessListRefresh();
						lstMessages.SelectedIndex = _activeMessage;
					}
					catch { /* do nothing */ }
					break;
				case 2:
					try
					{
						Mission.Trigger trig_temp = (Mission.Trigger)formatter.Deserialize(stream);
						_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4] = trig_temp;
						lblGlobTrigArr_Click(_activeGlobalTrigger, new EventArgs());
						Common.Title(this, false);
					}
					catch { /* do nothing */ }
					break;
				case 3:
					try
					{
						Team team_temp = (Team)formatter.Deserialize(stream);
						if (team_temp == null) throw new Exception();
						_mission.Teams[_activeTeam] = team_temp;
						TeamRefresh();
						Common.Title(this, false);
					}
					catch { /* do nothing */ }
					break;
			}
			#endregion
			stream.Close();
		}
		void menuRecentMissions_Click(object sender, EventArgs e)
		{
			string mission = _config.RecentMissions[(int)((MenuItem)sender).Tag];
			promptSave();
			initializeMission();
			if (loadMission(mission))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				_activeFG = 0;
				lstFG.SelectedIndex = 0;
				_loading = true;		//turned false in previous line
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_config.SetWorkingPath(Path.GetDirectoryName(mission)); //[JB] Update last-accessed
			opnXWA.InitialDirectory = _config.GetWorkingPath();
			savXWA.InitialDirectory = _config.GetWorkingPath();
			_loading = false;
		}
		void menuSave_Click(object sender, EventArgs e)
		{
			if (_mission.MissionPath  == "\\NewMission.tie") savXWA.ShowDialog();
			else saveMission(_mission.MissionPath);
		}
		void menuSaveAsBoP_Click(object sender, EventArgs e)
		{
			menuSaveAsXvT_Click("BoP", new EventArgs());
		}
		void menuSaveAsTIE_Click(object sender, EventArgs e)
		{
			promptSave();
			try
			{
				Platform.Tie.Mission converted = Platform.Converter.XwaToTie(_mission);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXvT_Click(object sender, EventArgs e)
		{
			promptSave();
			try
			{
				Platform.Xvt.Mission converted = Platform.Converter.XwaToXvtBop(_mission, sender.ToString() == "BoP");
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXWA_Click(object sender, EventArgs e)
		{
			savXWA.ShowDialog();
		}
		void menuSuperBackdrops_Click(object sender, EventArgs e)
		{
			tabMain.Focus();
			byte region = 0;
			if (sender.ToString() != "initializeMission()")
			{
				RegionSelectDialog regionDlg = new RegionSelectDialog(new string[] { _mission.Regions[0], _mission.Regions[1], _mission.Regions[2], _mission.Regions[3] });
				if (regionDlg.ShowDialog() == DialogResult.Cancel) return;
				region = regionDlg.SelectedRegion;
			}

			int requiredQty = 6;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_mission.FlightGroups[i].CraftType == 0xB7)
				{
					if (_mission.FlightGroups[i].Backdrop == 54)
					{
						MessageBox.Show("Mission already contains SuperBackdrops in " + _mission.Regions[region] + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}
					else if (_mission.FlightGroups[i].Waypoints[0].Region == region) requiredQty++;
				}
			}
			if (_mission.FlightGroups.Count >= Mission.FlightGroupLimit - requiredQty)
			{
				MessageBox.Show("Mission contains too many Flight Groups to add Super Backdrops (" + requiredQty + " needed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			for(int i = 0; i < 6; i++)
			{
				int index = _mission.FlightGroups.Add();
				_mission.FlightGroups[index].CraftType = 0xB7;
				_mission.FlightGroups[index].Backdrop = 54;
				_mission.FlightGroups[index].LightRGB = "0.0 0.0 0.0";
				_mission.FlightGroups[index].Brightness = "0.0";
				if (i < 2)
				{
					_mission.FlightGroups[index].BackdropSize = "0.895";
					_mission.FlightGroups[index].Shadow = 5;
				}
				else _mission.FlightGroups[index].BackdropSize = "1.055";
				_mission.FlightGroups[index].IFF = 3;   // Yellow
				_mission.FlightGroups[index].GlobalGroup = 42;
				_mission.FlightGroups[index].Waypoints[0].Region = region;
				lstFG.Items.Add(_mission.FlightGroups[index].ToString(true));
			}
			_mission.FlightGroups[_mission.FlightGroups.Count - 6].Waypoints[0].Z = 1;
			_mission.FlightGroups[_mission.FlightGroups.Count - 5].Waypoints[0].Z = -1;
			_mission.FlightGroups[_mission.FlightGroups.Count - 4].Waypoints[0].Y = -1;
			_mission.FlightGroups[_mission.FlightGroups.Count - 3].Waypoints[0].Y = 1;
			_mission.FlightGroups[_mission.FlightGroups.Count - 2].Waypoints[0].X = -1;
			_mission.FlightGroups[_mission.FlightGroups.Count - 1].Waypoints[0].X = 1;
			int tempIndex = _activeFG;
			for(int i = 0, copied = 0; copied < requiredQty - 6; i++)
			{
				if (_mission.FlightGroups[i].CraftType == 0xB7 && _mission.FlightGroups[i].Waypoints[0].Region == region)
				{
					_activeFG = i;
					menuCopy_Click("SBD", new EventArgs());
					menuPaste_Click("SBD", new EventArgs());
					_mission.FlightGroups[_mission.FlightGroups.Count - 1].Brightness = "0.0";
					copied++;
				}
			}
			if (!_loading) lstFG.SelectedIndex = tempIndex;
			updateFGList();
			Common.Title(this, _loading);
			try
			{
				_fMap.Import(_mission.FlightGroups);
				_fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
		}
		void menuText_Click(object sender, EventArgs e)
		{
			if (_config.ConfirmTest)
			{
				DialogResult res = new TestDialog(_config).ShowDialog();
				if (res == DialogResult.Cancel) return;
			}
			// prep stuff
			menuSave_Click("menuTest_Click", new EventArgs());
			if (_config.VerifyTest && !_config.Verify) Common.RunVerify(_mission.MissionPath, _config.VerifyLocation);
			int index = 0;
			while (File.Exists(_config.XwaPath + "\\test" + index + "0.plt")) index++;
			string pilot = "\\test" + index + "0.plt";
			string lst = "\\MISSIONS\\MISSION.LST";
			string backup = "\\MISSIONS\\MISSION_" + index + ".bak";

			// pilot file edit
			File.Copy(Application.StartupPath + "\\xwatest0.plt", _config.XwaPath + pilot);
			FileStream pilotFile = File.OpenWrite(_config.XwaPath + pilot);
			pilotFile.Position = 4;
			char[] indexBytes = index.ToString().ToCharArray();
			BinaryWriter bw = new BinaryWriter(pilotFile);
			bw.Write(indexBytes);
			for (int i = (int)pilotFile.Position; i < 0xC; i++) pilotFile.WriteByte(0);
			pilotFile.Position = 0x010F54;
			bw.Write(indexBytes);
			for (int i = (int)pilotFile.Position; i < 0x010F50 + 0xC; i++) pilotFile.WriteByte(0);
			pilotFile.Close();

			// configure XWA
			System.Diagnostics.Process xwa = new System.Diagnostics.Process();
			xwa.StartInfo.FileName = _config.XwaPath + "\\XWINGALLIANCE.exe";
			xwa.StartInfo.Arguments = "/skipintro";
			xwa.StartInfo.UseShellExecute = false;
			xwa.StartInfo.WorkingDirectory = _config.XwaPath;
			File.Copy(_config.XwaPath + lst, _config.XwaPath + backup, true);
			StreamReader sr = File.OpenText(_config.XwaPath + "\\CONFIG.CFG");
			string contents = sr.ReadToEnd();
			sr.Close();
			int lastpilot = contents.IndexOf("lastpilot ") + 10;
			int nextline = contents.IndexOf("\r\n", lastpilot);
			string modified = contents.Substring(0, lastpilot) + "test" + index + contents.Substring(nextline);
			StreamWriter sw = new FileInfo(_config.XwaPath + "\\CONFIG.CFG").CreateText();
			sw.Write(modified);
			sw.Close();
			sr = File.OpenText(_config.XwaPath + lst);
			contents = sr.ReadToEnd();
			sr.Close();
			string[] expanded = contents.Replace("\r\n", "\0").Split('\0');
			expanded[3] = "7";
			expanded[4] = _mission.MissionFileName;
			expanded[5] = "!MISSION_7_DESC!YOGEME: " + expanded[4];
			modified = String.Join("\r\n", expanded);
			sw = new FileInfo(_config.XwaPath + lst).CreateText();
			sw.Write(modified);
			sw.Close();

			xwa.Start();
			System.Threading.Thread.Sleep(1000);
			System.Diagnostics.Process[] runningXwas = System.Diagnostics.Process.GetProcessesByName("XWINGALLIANCE");
			while (runningXwas.Length > 0)
			{
				Application.DoEvents();
				System.Diagnostics.Debug.WriteLine("sleeping...");
				System.Threading.Thread.Sleep(1000);
				runningXwas = System.Diagnostics.Process.GetProcessesByName("XWINGALLIANCE");
			}

			if (_config.DeleteTestPilots) File.Delete(_config.XwaPath + pilot);
			File.Copy(_config.XwaPath + backup, _config.XwaPath + lst, true);
			File.Delete(_config.XwaPath + backup);
		}
		void menuVerify_Click(object sender, EventArgs e)
		{
			menuSave_Click("Verify", new System.EventArgs());
			if (!_config.Verify) Common.RunVerify(_mission.MissionPath, _config.VerifyLocation);	//prevents from doing this twice due to Save
		}
		#endregion
		#region FlightGroups
		//[JB] Counts all trigger and parameter references of a flight group.  Used to populate the counters in the confirm deletion dialog.
		int[] countFlightGroupReferences(int fgIndex)
		{
			int[] count = new int[9];
			const int cMothership = 1, cArrDep = 2, cOrder = 3, cSkip = 4, cGoal = 5, cMessage = 6, cFGGoal = 8; //cBrief = 7 Not needed for XWA
			for(int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if(fgIndex == i)
					continue;

				FlightGroup fg = _mission.FlightGroups[i];
				if(fg.ArrivalMethod1 == true && fg.ArrivalCraft1 == fgIndex) count[cMothership]++;
				if(fg.ArrivalMethod2 == true && fg.ArrivalCraft2 == fgIndex) count[cMothership]++;
				if(fg.DepartureMethod1 == true && fg.DepartureCraft1 == fgIndex) count[cMothership]++;
				if(fg.DepartureMethod2 == true && fg.DepartureCraft2 == fgIndex) count[cMothership]++;
				foreach(Mission.Trigger adt in fg.ArrDepTriggers)
				{
					//Note: many FGs initially present in battle use the first FG for Arr/Dep condition, even though the FG isn't actually used (condition is TRUE or FALSE). In which case no need to warn.
					if((adt.VariableType == 1 || adt.VariableType == 15) && adt.Variable == fgIndex && adt.Condition != 0 && adt.Condition != 10) count[cArrDep]++;
					if(adt.Parameter1 == 5 + fgIndex) count[cArrDep]++;
				}
				foreach(FlightGroup.Order order in fg.Orders)
				{
					if (order.Target1Type == 1 && order.Target1 == fgIndex) count[cOrder]++;
					if (order.Target2Type == 1 && order.Target2 == fgIndex) count[cOrder]++;
					if (order.Target3Type == 1 && order.Target3 == fgIndex) count[cOrder]++;
					if (order.Target4Type == 1 && order.Target4 == fgIndex) count[cOrder]++;
					foreach (Mission.Trigger sk in order.SkipTriggers)
					{
						if ((sk.VariableType == 1 || sk.VariableType == 15) && sk.Variable == fgIndex) count[cSkip]++;
						if (sk.Parameter1 == 5 + fgIndex) count[cSkip]++;
					}
				}
				foreach(var goal in fg.Goals)
					if(goal.Parameter == 5 + fgIndex)
						count[cFGGoal]++;
			}

			foreach (Globals global in _mission.Globals)
				foreach(Globals.Goal goal in global.Goals)
					foreach(Mission.Trigger trig in goal.Triggers)
					{
						if ((trig.VariableType == 1 || trig.VariableType == 15) && trig.Variable == fgIndex) count[cGoal]++;
						if (trig.Parameter1 == 5 + fgIndex) count[cGoal]++;
					}

			foreach (Idmr.Platform.Xwa.Message msg in _mission.Messages)
			{
				if(msg.OriginatingFG == fgIndex) count[cMessage]++;

				foreach(Mission.Trigger trig in msg.Triggers)
				{
					if ((trig.VariableType == 1 || trig.VariableType == 15) && trig.Variable == fgIndex) count[cMessage]++;
					if (trig.Parameter1 == 5 + fgIndex) count[cMessage]++;
				}
			}

			//XWA Briefing FGTag events don't seem to reference actual FGs.  No action necessary.

			for(int i = 1; i < 8; i++)
				count[0] += count[i];
			return count;
		}
		//[JB] Removes all references and triggers to a flight group and replaces with null triggers.
		void scrubFG(int fgIndex)
		{
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (fgIndex == i)
					continue;

				FlightGroup fg = _mission.FlightGroups[i];
				bool check = fg.ArrivesIn30Seconds;

				//Don't check method, always scrub if referenced
				if (fg.ArrivalCraft1 == fgIndex) { fg.ArrivalMethod1 = false; fg.ArrivalCraft1 = 0; }
				if (fg.ArrivalCraft2 == fgIndex) { fg.ArrivalMethod2 = false; fg.ArrivalCraft2 = 0; }
				if (fg.DepartureCraft1 == fgIndex) { fg.DepartureMethod1 = false; fg.DepartureCraft1 = 0; }
				if (fg.DepartureCraft2 == fgIndex) { fg.DepartureMethod2 = false; fg.DepartureCraft2 = 0; }
				for(int j = 0; j < fg.ArrDepTriggers.Length; j++)
				{
					Mission.Trigger adt = fg.ArrDepTriggers[j];
					if((adt.VariableType == 1 || adt.VariableType == 15) && adt.Variable == fgIndex)
					{
						adt.Amount = 0;
						adt.VariableType = 0;
						adt.Variable = 0;
						adt.Condition = (byte)((j < 4) ? 0 : 10); //  //First 4 are arrival.  Set those to true.  Departure set to _trigger[10] = "none (FALSE)"
					}
					if(adt.Parameter1 == 5 + fgIndex) adt.Parameter1 = 0;
				}
				foreach (FlightGroup.Order order in fg.Orders)
				{
					if (order.Target1Type == 1 && order.Target1 == fgIndex) { order.Target1Type = 0; order.Target1 = 0; }
					if (order.Target2Type == 1 && order.Target2 == fgIndex) { order.Target2Type = 0; order.Target2 = 0; }
					if (order.Target3Type == 1 && order.Target3 == fgIndex) { order.Target3Type = 0; order.Target3 = 0; }
					if (order.Target4Type == 1 && order.Target4 == fgIndex) { order.Target4Type = 0; order.Target4 = 0; }

					foreach (Mission.Trigger sk in order.SkipTriggers)
					{
						if ((sk.VariableType == 1 || sk.VariableType == 15) && sk.Variable == fgIndex)
						{
							sk.Amount = 0;
							sk.VariableType = 0;
							sk.Variable = 0;
							sk.Condition = 10;  //Set to false.
						}
						if (sk.Parameter1 == 5 + fgIndex) sk.Parameter1 = 0;
					}				
				}
				foreach(var goal in fg.Goals)
					if(goal.Parameter == 5 + fgIndex)
						goal.Parameter = 0;

				if (check != fg.ArrivesIn30Seconds)
					craftStart(fg, fg.ArrivesIn30Seconds);  //Add or delete based on change

			}
			foreach (Globals global in _mission.Globals)
			{
				foreach (Globals.Goal goal in global.Goals)
				{
					foreach (Mission.Trigger trig in goal.Triggers)
					{
						if ((trig.VariableType == 1 || trig.VariableType == 15) && trig.Variable == fgIndex)
						{
							trig.Amount = 0;
							trig.VariableType = 0;
							trig.Variable = 0;
							trig.Condition = 10; //_trigger[10] = "none (FALSE)"
						}
						if (trig.Parameter1 == 5 + fgIndex) trig.Parameter1 = 0;
					}
				}
			}
			foreach (Idmr.Platform.Xwa.Message msg in _mission.Messages)
			{
				if(msg.OriginatingFG == fgIndex) msg.OriginatingFG = 0;

				foreach (Mission.Trigger trig in msg.Triggers)
				{
					if ((trig.VariableType == 1 || trig.VariableType == 15) && trig.Variable == fgIndex)
					{
						trig.Amount = 0;
						trig.VariableType = 0;
						trig.Variable = 0;
						trig.Condition = 0; //_trigger[0] = "always (TRUE)"
					}

					if (trig.Parameter1 == 5 + fgIndex) trig.Parameter1 = 0;
				}
			}
			//XWA Briefing FGTag events don't seem to reference actual FGs.  No action necessary.
		}
		//[JB] When a FG is deleted, all further FGs drop down one index.  This function decrements all craft references to compensate.
		void scrubFGMove(int fgIndex)
		{
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (fgIndex == i)
					continue;

				FlightGroup fg = _mission.FlightGroups[i];

				if (fg.ArrivalCraft1 > fgIndex) { fg.ArrivalCraft1--; }
				if (fg.ArrivalCraft2 > fgIndex) { fg.ArrivalCraft2--; }
				if (fg.DepartureCraft1 > fgIndex) { fg.DepartureCraft1--; }
				if (fg.DepartureCraft2 > fgIndex) { fg.DepartureCraft2--; }
				foreach (Mission.Trigger adt in fg.ArrDepTriggers)
				{
					if ((adt.VariableType == 1 || adt.VariableType == 15) && adt.Variable > fgIndex) adt.Variable--;
					if(adt.Parameter1 > 5 + fgIndex) adt.Parameter1--;
				}

				foreach (FlightGroup.Order order in fg.Orders)
				{
					if (order.Target1Type == 1 && order.Target1 > fgIndex) { order.Target1--; }
					if (order.Target2Type == 1 && order.Target2 > fgIndex) { order.Target2--; }
					if (order.Target3Type == 1 && order.Target3 > fgIndex) { order.Target3--; }
					if (order.Target4Type == 1 && order.Target4 > fgIndex) { order.Target4--; }

					foreach (Mission.Trigger sk in order.SkipTriggers)
					{
						if ((sk.VariableType == 1 || sk.VariableType == 15) && sk.Variable > fgIndex) sk.Variable--;
						if (sk.Parameter1 > 5 + fgIndex) sk.Parameter1--;
					}
				}
				foreach(var goal in fg.Goals)
					if(goal.Parameter > 5 + fgIndex)
						goal.Parameter--;
			}
			foreach (Globals global in _mission.Globals)
				foreach (Globals.Goal goal in global.Goals)
					foreach (Mission.Trigger trig in goal.Triggers)
					{
						if ((trig.VariableType == 1 || trig.VariableType == 15) && trig.Variable > fgIndex) trig.Variable--;
						if (trig.Parameter1 > 5 + fgIndex) trig.Parameter1--;
					}

			foreach (Idmr.Platform.Xwa.Message msg in _mission.Messages)
			{
				if(msg.OriginatingFG > fgIndex) msg.OriginatingFG--;

				foreach (Mission.Trigger trig in msg.Triggers)
				{
					if ((trig.VariableType == 1 || trig.VariableType == 15) && trig.Variable > fgIndex) trig.Variable--;
					if (trig.Parameter1 > 5 + fgIndex) trig.Parameter1--;
				}
			}

			//XWA Briefing FGTag events don't seem to reference actual FGs.  No action necessary.
		}
		void deleteFG()
		{
			if(_fBrief != null)  //Need to be able to examine the current briefing data if open and modified
			{
				_fBrief.Save();
				_fBrief.Close();
			}
			//[JB] Confirm delete
			if(_config.ConfirmFGDelete)
			{
				int[] count = countFlightGroupReferences(_activeFG);
				if (count[0] > 0)
				{
					string[] Reasons = new string[9] { "", "Mothership reference", "Arrival/Departure trigger", "Order target reference", "'Skip to Order' trigger/param", "Global Goal trigger/param", "Message trigger/param", "Briefing FG Tag", "Goal FG Parameter" };
					string breakdown = "";
					for(int i = 1; i < 9; i++)
						if(count[i] > 0) breakdown += "    " + count[i] + " " + Reasons[i] + ((count[i]>1)?"s":"") + "\n";

					string s = "This Flight Group is referenced " + count[0] + " time" + ((count[1]>1)?"s":"") + " in these cases:\n" + breakdown + "\nAll references targeting this flight group will be reset to default.";
					if(count[7] > 0) s += "\nAssociated Briefing FG Tag events will be deleted.";
					s += "\n\nAre you sure you want to delete this Flight Group?";
					DialogResult res = MessageBox.Show(s, "WARNING: Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
					if (res == DialogResult.No)
						return;
				}
			}
			scrubFG(_activeFG);
			scrubFGMove(_activeFG);

			if (_mission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(_activeFG);
			craftStart(_mission.FlightGroups[_activeFG], false);
			if (_mission.FlightGroups.Count == 1)
			{
				_mission.FlightGroups.Clear();
				_activeFG = 0;
				_mission.FlightGroups[0].CraftType = _config.XwaCraft;
				_mission.FlightGroups[0].IFF = _config.XwaIff;
				craftStart(_mission.FlightGroups[0], true);
			}
			else _activeFG = _mission.FlightGroups.RemoveAt(_activeFG);
			updateFGList();
			lstFG.SelectedIndex = 0;
			Common.Title(this, _loading);
			try
			{
				_fMap.Import(_mission.FlightGroups);
				_fMap.MapPaint(true);
			}
			catch { /* do nothing */ }

			//[JB] Need to force these tabs to refresh, otherwise the trigger controls might display outdated information
			lstMessages_SelectedIndexChanged(0, new EventArgs());
			cboGlobalTeam_SelectedIndexChanged(0, new EventArgs());
			lstFG_SelectedIndexChanged(0, new EventArgs());
		}
		void listRefresh()
		{
			lstFG.Items[_activeFG] = _mission.FlightGroups[_activeFG].ToString(true);
		}
		void newFG()
		{
			if (_mission.FlightGroups.Count == Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			_activeFG = _mission.FlightGroups.Add();
			_mission.FlightGroups[_activeFG].CraftType = _config.XwaCraft;
			_mission.FlightGroups[_activeFG].IFF = _config.XwaIff;
			craftStart(_mission.FlightGroups[_activeFG], true);
			lstFG.Items.Add(_mission.FlightGroups[_activeFG].ToString(true));
			updateFGList();
			_loading = true;
			lstFG.SelectedIndex = _activeFG;
			_loading = false;
			Common.Title(this, _loading);
			try
			{
				_fMap.Import(_mission.FlightGroups);
				_fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
		}
		void updateFGList()
		{
			string[] fgList = _mission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			comboReset(cboArrMS, fgList, 0);
			comboReset(cboArrMSAlt, fgList, 0);
			comboReset(cboDepMS, fgList, 0);
			comboReset(cboDepMSAlt, fgList, 0);
			cboMessFG.Items.Clear(); cboMessFG.Items.AddRange(fgList);
			if (_mission.Messages.Count != 0) cboMessFG.SelectedIndex = _mission.Messages[_activeMessage].OriginatingFG;
			parameterRefresh(cboSkipPara);
			parameterRefresh(cboGoalPara);
			parameterRefresh(cboADPara);
			parameterRefresh(cboMessPara);
			parameterRefresh(cboGlobalPara);
			_loading = temp;
			listRefresh();
		}
		string generateGoalSummary()
		{
			//30 elements:  Primary,Prevent,Bonus, ... (repeat for each team)
			//Each element contains a list of strings for each line of text.
			//Was going to try and summarize global goals but the output was ugly, so removed it. Easy for the user to view those anyway.
			System.Collections.Generic.List<string>[] goalList = new System.Collections.Generic.List<string>[30];

			for (int i = 0; i < 30; i++)
				goalList[i] = new System.Collections.Generic.List<string>();

			//Iterate FGs and their goals, adding them to the proper list
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				FlightGroup fg = _mission.FlightGroups[i];
				foreach (FlightGroup.Goal goal in fg.Goals)
				{
					if (goal.Condition == 0 || goal.Condition == 10 || goal.Team > 9)  //Avoid True, False conditions and make sure team is valid.
						continue;
					string c = Strings.CraftAbbrv[fg.CraftType] + " " + fg.Name;
					string n = goal.ToString().Replace("Flight Group", c);
					int category = (goal.Argument <= 1) ? 0 : 2;  //0 = primary, 1 = prevent, 2 = bonus
					goalList[(goal.Team * 3) + category].Add(n);
				}
			}

			//Compose the output by going through the teams and writing the results from each goal category
			string output = "";
			for (int i = 0; i < 10; i++) //Each team
			{
				if (goalList[(i * 3) + 0].Count == 0 && goalList[(i * 3) + 2].Count == 0)  //No primary or bonus goals
					continue;

				if (output.Length > 0)
					output += "\r\n----------\r\n";
				output += "TEAM #" + (i + 1) + ": " + _mission.Teams[i].Name + "\r\n";
				output += "PRIMARY:\r\n";
				foreach (string s in goalList[(i * 3) + 0])
					output += s + "\r\n";

				if (goalList[(i * 3) + 1].Count > 0)
				{
					output += "\r\n";
					output += "SECONDARY:\r\n";
					foreach (string s in goalList[(i * 3) + 1])
						output += s + "\r\n";
				}

				if (goalList[(i * 3) + 2].Count > 0)
				{
					output += "\r\n";
					output += "BONUS:\r\n";
					foreach (string s in goalList[(i * 3) + 2])
						output += s + "\r\n";
				}
			}
			if (output == "") output = "Nothing here.";
			output += "\r\n";

			return output;
		}

		void lstFG_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1 || _mission.FlightGroups[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch (_mission.FlightGroups[e.Index].IFF)
			{
				case 0:
					brText = Brushes.LimeGreen;
					break;
				case 1:
					brText = Brushes.Crimson;
					break;
				case 2:
					brText = Brushes.RoyalBlue;
					break;
				case 3:
					brText = Brushes.Yellow;
					break;
				case 4:
					brText = Brushes.Red;
					break;
				case 5:
					brText = Brushes.DarkOrchid;
					break;
			}
			e.Graphics.DrawString(lstFG.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstFG.SelectedIndex == -1) return;
			_activeFG = lstFG.SelectedIndex;
			lblFG.Text = "Flight Group #" + (_activeFG+1).ToString() + " of " + _mission.FlightGroups.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			#region Craft
			txtName.Text = _mission.FlightGroups[_activeFG].Name;
			txtCargo.Text = _mission.FlightGroups[_activeFG].Cargo;
			txtSpecCargo.Text = _mission.FlightGroups[_activeFG].SpecialCargo;  //[JB] Fixed, was Cargo
			numSC.Value = _mission.FlightGroups[_activeFG].SpecialCargoCraft;
			chkRandSC.Checked = _mission.FlightGroups[_activeFG].RandSpecCargo;
			cboCraft.SelectedIndex = _mission.FlightGroups[_activeFG].CraftType;
			cboIFF.SelectedIndex = _mission.FlightGroups[_activeFG].IFF;
			cboTeam.SelectedIndex = _mission.FlightGroups[_activeFG].Team;
			try { cboAI.SelectedIndex = _mission.FlightGroups[_activeFG].AI; }	// for some reason, some custom missions have this as -1
			catch { cboAI.SelectedIndex = 2; _mission.FlightGroups[_activeFG].AI = 2; }	// default to Veteran
			cboMarkings.SelectedIndex = _mission.FlightGroups[_activeFG].Markings;
			cboPlayer.SelectedIndex = _mission.FlightGroups[_activeFG].PlayerNumber;
			cboPosition.SelectedIndex = _mission.FlightGroups[_activeFG].PlayerCraft;
			cboFormation.SelectedIndex = _mission.FlightGroups[_activeFG].Formation;
			cboRadio.SelectedIndex = _mission.FlightGroups[_activeFG].Radio;
			numLead.Value = _mission.FlightGroups[_activeFG].FormLeaderDist;
			numSpacing.Value = _mission.FlightGroups[_activeFG].FormDistance;
			numWaves.Value = _mission.FlightGroups[_activeFG].NumberOfWaves;
			numCraft.Value = _mission.FlightGroups[_activeFG].NumberOfCraft;
			numGG.Value = _mission.FlightGroups[_activeFG].GlobalGroup;
			numGU.Value = _mission.FlightGroups[_activeFG].GlobalUnit;
			chkGU.Checked = _mission.FlightGroups[_activeFG].GlobalNumbering;
			cboStatus.SelectedIndex = _mission.FlightGroups[_activeFG].Status1;
			cboStatus2.SelectedIndex = _mission.FlightGroups[_activeFG].Status2;
			cboWarheads.SelectedIndex = _mission.FlightGroups[_activeFG].Missile;
			cboBeam.SelectedIndex = _mission.FlightGroups[_activeFG].Beam;
			cboCounter.SelectedIndex = _mission.FlightGroups[_activeFG].Countermeasures;
			numExplode.Value = _mission.FlightGroups[_activeFG].ExplosionTime;
			numBackdrop.Value = _mission.FlightGroups[_activeFG].Backdrop;
			cboGlobCargo.SelectedIndex = _mission.FlightGroups[_activeFG].GlobalCargo;
			cboGlobSpecCargo.SelectedIndex = _mission.FlightGroups[_activeFG].GlobalSpecialCargo;
			#endregion
			#region Arr/Dep
			optArrMS.Checked = _mission.FlightGroups[_activeFG].ArrivalMethod1;
			optArrHyp.Checked = !optArrMS.Checked;
			try { cboArrMS.SelectedIndex = _mission.FlightGroups[_activeFG].ArrivalCraft1; }
			catch { cboArrMS.SelectedIndex = 0; _mission.FlightGroups[_activeFG].ArrivalCraft1 = 0; optArrHyp.Checked = true; }
			optArrMSAlt.Checked = _mission.FlightGroups[_activeFG].ArrivalMethod2;
			optArrHypAlt.Checked = !optArrMSAlt.Checked;
			try { cboArrMSAlt.SelectedIndex = _mission.FlightGroups[_activeFG].ArrivalCraft2; }
			catch { cboArrMSAlt.SelectedIndex = 0; _mission.FlightGroups[_activeFG].ArrivalCraft2 = 0; optArrHypAlt.Checked = true; }
			optDepMS.Checked = _mission.FlightGroups[_activeFG].DepartureMethod1;
			optDepHyp.Checked = !optDepMS.Checked;
			try { cboDepMS.SelectedIndex = _mission.FlightGroups[_activeFG].DepartureCraft1; }
			catch { cboDepMS.SelectedIndex = 0; _mission.FlightGroups[_activeFG].DepartureCraft1 = 0; optDepHyp.Checked = true; }
			optDepMSAlt.Checked = _mission.FlightGroups[_activeFG].DepartureMethod2;
			optDepHypAlt.Checked = !optDepMSAlt.Checked;
			try { cboDepMSAlt.SelectedIndex = _mission.FlightGroups[_activeFG].DepartureCraft2; }
			catch { cboDepMSAlt.SelectedIndex = 0; _mission.FlightGroups[_activeFG].DepartureCraft2 = 0; optDepHypAlt.Checked = true; }
			for (int i=0;i<4;i++)
			{
				optADAndOr[i].Checked = _mission.FlightGroups[_activeFG].ArrDepAndOr[i];
				optADAndOr[i+4].Checked = !optADAndOr[i].Checked;
			}
			numArrMin.Value = _mission.FlightGroups[_activeFG].ArrivalDelayMinutes;
			numArrSec.Value = _mission.FlightGroups[_activeFG].ArrivalDelaySeconds;
			numDepMin.Value = _mission.FlightGroups[_activeFG].DepartureTimerMinutes;
			numDepSec.Value = _mission.FlightGroups[_activeFG].DepartureTimerSeconds;
			cboAbort.SelectedIndex = _mission.FlightGroups[_activeFG].AbortTrigger;
			cboDiff.SelectedIndex = _mission.FlightGroups[_activeFG].Difficulty;
			chkArrHuman.Checked = _mission.FlightGroups[_activeFG].ArriveOnlyIfHuman;
			for (int i=0;i<6;i++) labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[i], lblADTrig[i]);
			lblADTrigArr_Click(lblADTrig[0], new EventArgs());
			#endregion
			for (_activeFGGoal=0;_activeFGGoal<8;_activeFGGoal++) GoalLabelRefresh();
			lblGoalArr_Click(lblGoal[0], new EventArgs());
			#region Waypoints
			cboWP.SelectedIndex = -1;	// force change
			cboWP.SelectedIndex = 0;
			for (int i=0;i<4;i++)
			{
				for (int j=0;j<3;j++)
				{
					_tableWPRaw.Rows[i][j] = _mission.FlightGroups[_activeFG].Waypoints[i][j];
					_tableWP.Rows[i][j] = Math.Round((double)_mission.FlightGroups[_activeFG].Waypoints[i][j] / 160, 2);
				}
				chkWP[i].Checked = _mission.FlightGroups[_activeFG].Waypoints[i].Enabled;
			}
			numSP1.Value = _mission.FlightGroups[_activeFG].Waypoints[0].Region + 1;
			numSP2.Value = _mission.FlightGroups[_activeFG].Waypoints[1].Region + 1;
			numSP3.Value = _mission.FlightGroups[_activeFG].Waypoints[2].Region + 1;
			numHYP.Value = _mission.FlightGroups[_activeFG].Waypoints[3].Region + 1;
			_tableWPRaw.AcceptChanges();
			_tableWP.AcceptChanges();
			numYaw.Value = _mission.FlightGroups[_activeFG].Yaw;
			numPitch.Value = _mission.FlightGroups[_activeFG].Pitch;
			numRoll.Value = _mission.FlightGroups[_activeFG].Roll;
			#endregion
			for (_activeOrder=0;_activeOrder<4;_activeOrder++) OrderLabelRefresh();
			lblOrderArr_Click(lblOrder[0], new EventArgs());
			#region Options
			chkRole1.Checked = _mission.FlightGroups[_activeFG].EnableDesignation1;
			chkRole2.Checked = _mission.FlightGroups[_activeFG].EnableDesignation2;
			cboRole1.SelectedIndex = _mission.FlightGroups[_activeFG].Designation1;
			cboRole2.SelectedIndex = _mission.FlightGroups[_activeFG].Designation2;
			txtRole.Text = _mission.FlightGroups[_activeFG].Role;
			txtPilot.Text = _mission.FlightGroups[_activeFG].PilotID;
			for (int i=0;i<15;i++) chkOpt[i].Checked = _mission.FlightGroups[_activeFG].OptLoadout[i];
			lblSkipTrigArr_Click(lblSkipTrig2, new EventArgs());  //[JB] Added
			lblSkipTrigArr_Click(lblSkipTrig1, new EventArgs());  //[JB] Update after the Trig2 so it also updates the region parameters
			for (_activeOptionCraft=0;_activeOptionCraft<10;_activeOptionCraft++) OptCraftLabelRefresh();
			lblOptCraftArr_Click(lblOptCraft[0], new EventArgs());
			cboOptCat.SelectedIndex = (byte)_mission.FlightGroups[_activeFG].OptCraftCategory;
			//[JB] Need to fix the Skip And/Or radio button checks.
			optSkipOR.Checked = _mission.FlightGroups[_activeFG].Orders[0, 0].SkipT1AndOrT2;
			optSkipAND.Checked = !optSkipOR.Checked;
			#endregion
			#region Unknowns
			numUnk1.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown1;
			numUnk3.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown3;
			numUnk4.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown4;
			numUnk5.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown5;
			chkUnk6.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown6;
			numUnk7.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown7;
			numUnk8.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown8;
			cboUnkOrder.SelectedIndex = 0;
			cboUnkOrder_SelectedIndexChanged(0, new EventArgs()); //[JB] Fixes bug where controls wouldn't update. Force refresh because the selection value may not have actually changed.
			numUnkGoal.Value = 1;
			numUnkGoal_ValueChanged(0, new EventArgs()); //[JB] Force refresh of the associated checkbox
			numUnk16.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown16;
			numUnk17.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown17;
			numUnk18.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown18;
			numUnk19.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown19;
			numUnk20.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown20;
			numUnk21.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown21;
			chkUnk22.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown22;
			numUnk23.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown23;
			numUnk24.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown24;
			numUnk25.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown25;
			numUnk26.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown26;
			numUnk27.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown27;
			numUnk28.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown28;
			chkUnk29.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown29;
			chkUnk30.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown30;
			chkUnk31.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown31;
			numUnk32.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown32;
			numUnk33.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown33;
			chkUnk34.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown34;
			chkUnk35.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown35;
			chkUnk36.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown36;
			chkUnk37.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown37;
			chkUnk38.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown38;
			chkUnk39.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown39;
			chkUnk40.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown40;
			chkUnk41.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown41;
			#endregion
			_loading = btemp;
			EnableBackdrop((_mission.FlightGroups[_activeFG].CraftType == 0xB7 ? true : false));

			if(!lstFG.Focused) lstFG.Focus();  //[JB] Return control back to the list (helpful to maintain navigation using the arrow keys when certain tabs are open)
		}

		#region Craft
		void EnableBackdrop(bool state)
		{
			numBackdrop.Enabled = state;
			cmdBackdrop.Enabled = state;
			cboAI.Enabled = !state;
			cboMarkings.Enabled = !state;
			cboPlayer.Enabled = !state;
			cboPosition.Enabled = !state;
			cboRadio.Enabled = !state;
			cboFormation.Enabled = !state;
			cmdForms.Enabled = !state;
			numSpacing.Enabled = !state;
			numLead.Enabled = !state;
			cboStatus.Enabled = !state;
			cboStatus2.Enabled = !state;
			cboWarheads.Enabled = !state;
			cboBeam.Enabled = !state;
			cboCounter.Enabled = !state;
			cboGlobSpecCargo.Enabled = !state;
			chkGU.Enabled = !state;
			numCraft.Enabled = !state;
			numWaves.Enabled = !state;
			numSC.Enabled = !state;
			chkRandSC.Enabled = !state;
			numExplode.Enabled = !state;
			if (state)
			{
				lblGC.Text = "Shadow";
				lblCargo.Text = "Brightness";
				lblSC.Text = "Size";
				lblName.Text = "R G B";
				lblNotUsed.Visible = !state;
				txtSpecCargo.Visible = state;
			}
			else
			{
				lblGC.Text = "Global Cargo";
				lblCargo.Text = "Cargo";
				lblSC.Text = "Special Cargo";	   //[JB] Fixed typo
				lblName.Text = "Name";
				numSC_ValueChanged("EnableBackdrop", new EventArgs());
			}
		}

		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnableBackdrop((cboCraft.SelectedIndex == 0xB7 ? true : false));
			if (_loading) return;
			_mission.FlightGroups[_activeFG].CraftType = Common.Update(this, _mission.FlightGroups[_activeFG].CraftType, Convert.ToByte(cboCraft.SelectedIndex));
			updateFGList();
		}
		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].Formation = Common.Update(this, _mission.FlightGroups[_activeFG].Formation, Convert.ToByte(cboFormation.SelectedIndex));
		}
		void cboGlobCargo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].GlobalCargo = Common.Update(this, _mission.FlightGroups[_activeFG].GlobalCargo, Convert.ToByte(cboGlobCargo.SelectedIndex));
		}

		void chkRandSC_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].RandSpecCargo = Common.Update(this, _mission.FlightGroups[_activeFG].RandSpecCargo, chkRandSC.Checked);
			if (chkRandSC.Checked)
			{
				numSC.Value = 0;
				lblNotUsed.Visible = false;
				txtSpecCargo.Visible = true;
			}
			else if (numSC.Value == 0)
			{
				lblNotUsed.Visible = true;
				txtSpecCargo.Visible = false;
			}
		}

		void cmdBackdrop_Click(object sender, EventArgs e)
		{
			if (_config.SuperBackdropsInstalled)
			{	// this is here due to inherent lag when loading that many high-res images
				cmdBackdrop.Text = "Loading...";
				cmdBackdrop.Enabled = false;
			}
			try
			{
				BackdropDialog dlg = new BackdropDialog(_mission.FlightGroups[_activeFG].Backdrop, _mission.FlightGroups[_activeFG].GlobalCargo);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					cboGlobCargo.SelectedIndex = dlg.Shadow;
					numBackdrop.Value = dlg.BackdropIndex;
				}
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			cmdBackdrop.Enabled = true;
			cmdBackdrop.Text = "Backdrops";
		}
		void cmdForms_Click(object sender, EventArgs e)
		{
			try
			{
				FormationDialog dlg = new FormationDialog(_mission.FlightGroups[_activeFG].Formation);
				if (dlg.ShowDialog() == DialogResult.OK) cboFormation.SelectedIndex = dlg.Formation;
			} catch { MessageBox.Show("The Formations browser could not be loaded." , "Error"); }
		}

		void grpCraft2_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].IFF = Common.Update(this, _mission.FlightGroups[_activeFG].IFF, Convert.ToByte(cboIFF.SelectedIndex));
			_mission.FlightGroups[_activeFG].Team = Common.Update(this, _mission.FlightGroups[_activeFG].Team, Convert.ToByte(cboTeam.SelectedIndex));
			_mission.FlightGroups[_activeFG].AI = Common.Update(this, _mission.FlightGroups[_activeFG].AI, Convert.ToByte(cboAI.SelectedIndex));
			_mission.FlightGroups[_activeFG].Markings = Common.Update(this, _mission.FlightGroups[_activeFG].Markings, Convert.ToByte(cboMarkings.SelectedIndex));
			_mission.FlightGroups[_activeFG].PlayerNumber = Common.Update(this, _mission.FlightGroups[_activeFG].PlayerNumber, Convert.ToByte(cboPlayer.SelectedIndex));
			_mission.FlightGroups[_activeFG].PlayerCraft = Common.Update(this, _mission.FlightGroups[_activeFG].PlayerCraft, Convert.ToByte(cboPosition.SelectedIndex));
			_mission.FlightGroups[_activeFG].Radio = Common.Update(this, _mission.FlightGroups[_activeFG].Radio, Convert.ToByte(cboRadio.SelectedIndex));
			_mission.FlightGroups[_activeFG].FormDistance = Common.Update(this, _mission.FlightGroups[_activeFG].FormDistance, Convert.ToByte(numSpacing.Value));
			_mission.FlightGroups[_activeFG].FormLeaderDist = Common.Update(this, _mission.FlightGroups[_activeFG].FormLeaderDist, Convert.ToByte(numLead.Value));
			listRefresh();
		}
		void grpCraft3_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].NumberOfWaves = Common.Update(this, _mission.FlightGroups[_activeFG].NumberOfWaves, Convert.ToByte(numWaves.Value));
			craftStart(_mission.FlightGroups[_activeFG], false);
			_mission.FlightGroups[_activeFG].NumberOfCraft = Common.Update(this, _mission.FlightGroups[_activeFG].NumberOfCraft, Convert.ToByte(numCraft.Value));
			craftStart(_mission.FlightGroups[_activeFG], true);
			if (_mission.FlightGroups[_activeFG].SpecialCargoCraft > _mission.FlightGroups[_activeFG].NumberOfCraft) numSC.Value = 0;
			_mission.FlightGroups[_activeFG].GlobalGroup = Common.Update(this, _mission.FlightGroups[_activeFG].GlobalGroup, Convert.ToByte(numGG.Value));
			_mission.FlightGroups[_activeFG].GlobalUnit = Common.Update(this, _mission.FlightGroups[_activeFG].GlobalUnit, Convert.ToByte(numGU.Value));
			_mission.FlightGroups[_activeFG].GlobalNumbering = Common.Update(this, _mission.FlightGroups[_activeFG].GlobalNumbering, chkGU.Checked);
			listRefresh();
		}
		void grpCraft4_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Status1 = Common.Update(this, _mission.FlightGroups[_activeFG].Status1, Convert.ToByte(cboStatus.SelectedIndex));
			_mission.FlightGroups[_activeFG].Status2 = Common.Update(this, _mission.FlightGroups[_activeFG].Status2, Convert.ToByte(cboStatus2.SelectedIndex));
			_mission.FlightGroups[_activeFG].Missile = Common.Update(this, _mission.FlightGroups[_activeFG].Missile, Convert.ToByte(cboWarheads.SelectedIndex));
			_mission.FlightGroups[_activeFG].Beam = Common.Update(this, _mission.FlightGroups[_activeFG].Beam, Convert.ToByte(cboBeam.SelectedIndex));
			_mission.FlightGroups[_activeFG].Countermeasures = Common.Update(this, _mission.FlightGroups[_activeFG].Countermeasures, Convert.ToByte(cboCounter.SelectedIndex));
			//XwaMission.FlightGroups[FG].ExplosionTime = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ExplosiongTime, numExplode.Value));
			_mission.FlightGroups[_activeFG].GlobalSpecialCargo = Common.Update(this, _mission.FlightGroups[_activeFG].GlobalSpecialCargo, Convert.ToByte(cboGlobSpecCargo.SelectedIndex));
		}

		void numBackdrop_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].Backdrop = Common.Update(this, _mission.FlightGroups[_activeFG].Backdrop, Convert.ToByte(numBackdrop.Value));
		}
		void numSC_ValueChanged(object sender, EventArgs e)
		{
			if (_mission.FlightGroups[_activeFG].RandSpecCargo)
			{
				if (numSC.Value != 0) numSC.Value = 0;
				return;
			}
			if (numSC.Value == 0 || numSC.Value > _mission.FlightGroups[_activeFG].NumberOfCraft)
			{
				_mission.FlightGroups[_activeFG].SpecialCargoCraft = Common.Update(this, _mission.FlightGroups[_activeFG].SpecialCargoCraft, Convert.ToByte(0));
				txtSpecCargo.Visible = false;
				lblNotUsed.Visible = true;
			}
			else
			{
				_mission.FlightGroups[_activeFG].SpecialCargoCraft = Common.Update(this, _mission.FlightGroups[_activeFG].SpecialCargoCraft, Convert.ToByte(numSC.Value));
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
		#region Arr/Dep
		void lblADTrigArr_Click(object sender, EventArgs e)
		{
			//[JB] Added try/catch block from XvtForm.  This fixes a bug (failed due to exception) that prevented labels from updating after a paste operation.
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeArrDepTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeArrDepTrigger = Convert.ToByte(sender); l = lblADTrig[_activeArrDepTrigger]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<6;i++) if (i != _activeArrDepTrigger) lblADTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboADTrig.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Condition;
			cboADTrigType.SelectedIndex = -1;
			cboADTrigType.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType;
			cboADTrigAmount.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Amount;
			//[JB] Fixes exceptions for backdrop FGs in B4M2B.TIE, B4M3FB.TIE, B4M4B.TIE which use have Parameter1 values around 78 to 80
			int v = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Parameter1;
			if (v >= cboADPara.Items.Count)
				v = cboADPara.Items.Count - 1;
			cboADPara.SelectedIndex = v; //_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Parameter1;
			numADPara.Value = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Parameter2;
			_loading = btemp;
		}
		void lblADTrigArr_DoubleClick(object sender, EventArgs e)
		{
			menuPaste_Click("AD", new EventArgs());
		}
		void lblADTrigArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("AD", new EventArgs());
		}
		void optADAndOrArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			RadioButton r = (RadioButton)sender;
			_mission.FlightGroups[_activeFG].ArrDepAndOr[(int)r.Tag] = Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepAndOr[(int)r.Tag], r.Checked);
		}

		void cboADPara_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Parameter1 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Parameter1, Convert.ToByte(cboADPara.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboADTrig_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Condition = Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Condition, Convert.ToByte(cboADTrig.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboADTrigAmount_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Amount = Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Amount, Convert.ToByte(cboADTrigAmount.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboADTrigType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboADTrigType.SelectedIndex == -1) return;
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType = Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType, Convert.ToByte(cboADTrigType.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType, cboADTrigVar);
			try { cboADTrigVar.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable; }
			catch { cboADTrigVar.SelectedIndex = 0; _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable = 0; }
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboADTrigVar_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable = Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable, Convert.ToByte(cboADTrigVar.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboArrMS_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrivalCraft1 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalCraft1, Convert.ToByte(cboArrMS.SelectedIndex));
		}
		void cboArrMSAlt_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrivalCraft2 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalCraft2, Convert.ToByte(cboArrMSAlt.SelectedIndex));
		}
		void cboDiff_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Difficulty = Common.Update(this, _mission.FlightGroups[_activeFG].Difficulty, Convert.ToByte(cboDiff.SelectedIndex));
		}

		void chkArrHuman_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArriveOnlyIfHuman = Common.Update(this, _mission.FlightGroups[_activeFG].ArriveOnlyIfHuman, chkArrHuman.Checked);
		}

		void cmdCopyAD_Click(object sender, EventArgs e)
		{
			menuCopy_Click("AD", new EventArgs());
		}
		void cmdPasteAD_Click(object sender, EventArgs e)
		{
			menuPaste_Click("AD", new EventArgs());
		}

		void grpDep_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].DepartureCraft1 = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureCraft1, Convert.ToByte(cboDepMS.SelectedIndex));
			_mission.FlightGroups[_activeFG].DepartureCraft2 = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureCraft2, Convert.ToByte(cboDepMSAlt.SelectedIndex));
			_mission.FlightGroups[_activeFG].AbortTrigger = Common.Update(this, _mission.FlightGroups[_activeFG].AbortTrigger, Convert.ToByte(cboAbort.SelectedIndex));
			_mission.FlightGroups[_activeFG].DepartureTimerMinutes = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureTimerMinutes, Convert.ToByte(numDepMin.Value));
			_mission.FlightGroups[_activeFG].DepartureTimerSeconds = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureTimerSeconds, Convert.ToByte(numDepSec.Value));
		}

		void numADPara_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Parameter2 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Parameter2, Convert.ToByte(numADPara.Value));
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void numArrMin_Leave(object sender, EventArgs e)
		{
			craftStart(_mission.FlightGroups[_activeFG], false);
			_mission.FlightGroups[_activeFG].ArrivalDelayMinutes = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalDelayMinutes, Convert.ToByte(numArrMin.Value));
			craftStart(_mission.FlightGroups[_activeFG], true);
		}
		void numArrSec_Leave(object sender, EventArgs e)
		{
			craftStart(_mission.FlightGroups[_activeFG], false);
			_mission.FlightGroups[_activeFG].ArrivalDelaySeconds = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalDelaySeconds, Convert.ToByte(numArrSec.Value));
			craftStart(_mission.FlightGroups[_activeFG], true);
		}

		void optArrMS_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMS.Enabled = optArrMS.Checked;
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrivalMethod1 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalMethod1, optArrMS.Checked);
		}
		void optArrMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMSAlt.Enabled = optArrMSAlt.Checked;
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrivalMethod2 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalMethod2, optArrMSAlt.Checked);
		}
		void optDepMS_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMS.Enabled = optDepMS.Checked;
			if (!_loading)
				_mission.FlightGroups[_activeFG].DepartureMethod1 = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureMethod1, optDepMS.Checked);
		}
		void optDepMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMSAlt.Enabled = optDepMSAlt.Checked;
			if (!_loading)
				_mission.FlightGroups[_activeFG].DepartureMethod2 = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureMethod2, optDepMSAlt.Checked);
		}
		#endregion
		#region Orders
		void OrderLabelRefresh()
		{
			string orderText = _mission.FlightGroups[_activeFG].Orders[(int)numORegion.Value - 1, _activeOrder].ToString();
			orderText = replaceTargetText(orderText);
			lblOrder[_activeOrder].Text = "Order " + (_activeOrder + 1) + ": " + orderText;
		}

		void lblOrderArr_Click(object sender, EventArgs e)
		{
			//[JB] Added catch from XvT. Fixes bug where the labels wouldn't refresh after double click paste.
			Label l = null;
			try { l = (Label)sender; }
			catch { _activeOrder = Convert.ToByte(sender); l = lblOrder[_activeOrder]; /*l = lblOrder[(int)sender];*/ }
			l.Focus();
			_activeOrder = Convert.ToByte(l.Tag);
			FlightGroup.Order order = _mission.FlightGroups[_activeFG].Orders[(int)numORegion.Value - 1, _activeOrder];
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<4;i++) if (i!=_activeOrder) lblOrder[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboOrders.SelectedIndex = order.Command;
			cboOThrottle.SelectedIndex = order.Throttle;
			numOVar1.Value = order.Variable1;
			numOVar2.Value = order.Variable2;
			numOVar3.Value = order.Variable3;
			cboOT3Type.SelectedIndex = -1;
			cboOT3Type.SelectedIndex = order.Target3Type;
			cboOT4Type.SelectedIndex = -1;
			cboOT4Type.SelectedIndex = order.Target4Type;
			optOT3T4OR.Checked = order.T3AndOrT4;
			optOT3T4AND.Checked = !optOT3T4OR.Checked;
			cboOT1Type.SelectedIndex = -1;
			cboOT1Type.SelectedIndex = order.Target1Type;
			cboOT2Type.SelectedIndex = -1;
			cboOT2Type.SelectedIndex = order.Target2Type;
			optOT1T2OR.Checked = order.T1AndOrT2;
			optOT1T2AND.Checked = !optOT1T2OR.Checked;
			numOSpeed.Value = order.Speed;
			txtOString.Text = order.CustomText;
			_loading = btemp;
		}
		void lblOrderArr_DoubleClick(object sender, EventArgs e)
		{
			if(((MouseEventArgs)e).Button != MouseButtons.Left) return;  //[JB] Fixed to help prevent rapid right clicks from triggering paste.  Seemed to affect this frequently, despite other controls with similar events not suffering the same behavior.
			menuPaste_Click("Order", new EventArgs());
		}
		void lblOrderArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Order", new EventArgs());
		}

		void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Command = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Command, Convert.ToByte(cboOrders.SelectedIndex));
			OrderLabelRefresh();
			int i = Strings.OrderDesc[cboOrders.SelectedIndex].IndexOf("|");
			int j = Strings.OrderDesc[cboOrders.SelectedIndex].IndexOf("|", i+1);
			int k = Strings.OrderDesc[cboOrders.SelectedIndex].LastIndexOf("|");
			lblODesc.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(0, i);
			lblOVar1.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(i+1, j-i-1);
			lblOVar2.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(j+1, k-j-1);
			lblOVar3.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(k+1);
		}
		void cboOT1_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value-1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target1 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target1, Convert.ToByte(cboOT1.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT1Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT1Type.SelectedIndex == -1) return;
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target1Type = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target1Type, Convert.ToByte(cboOT1Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target1Type, cboOT1);
			try { cboOT1.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target1; }
			catch { cboOT1.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target1 = 0; }
			OrderLabelRefresh();
		}
		void cboOT2_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target2, Convert.ToByte(cboOT2.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT2Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT2Type.SelectedIndex == -1) return;
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target2Type = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target2Type, Convert.ToByte(cboOT2Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target2Type, cboOT2);
			try { cboOT2.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target2; }
			catch { cboOT2.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target2 = 0; }
			OrderLabelRefresh();
		}
		void cboOT3_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target3 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target3, Convert.ToByte(cboOT3.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT3Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT3Type.SelectedIndex == -1) return;  //[JB] Fixed, was !=
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target3Type = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target3Type, Convert.ToByte(cboOT3Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target3Type, cboOT3);
			try { cboOT3.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target3; }
			catch { cboOT3.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target3 = 0; }
			OrderLabelRefresh();
		}
		void cboOT4_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target4 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target4, Convert.ToByte(cboOT4.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT4Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT4Type.SelectedIndex == -1) return;
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target4Type = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target4Type, Convert.ToByte(cboOT4Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target4Type, cboOT4);
			try { cboOT4.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target4; }
			catch { cboOT4.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Target4 = 0; }
			OrderLabelRefresh();
		}
		void cboOThrottle_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Throttle = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Throttle, Convert.ToByte(cboOThrottle.SelectedIndex));
		}

		void numORegion_ValueChanged(object sender, EventArgs e)
		{
			for (_activeOrder=0;_activeOrder<4;_activeOrder++) OrderLabelRefresh();
			lblOrderArr_Click(0, new EventArgs());
		}
		void numOSpeed_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Speed = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Speed, Convert.ToByte(numOSpeed.Value));
		}
		void numOVar1_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Variable1 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Variable1, Convert.ToByte(numOVar1.Value));
		}
		void numOVar2_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Variable2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Variable2, Convert.ToByte(numOVar2.Value));
		}
		void numOVar3_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Variable3 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].Variable3, Convert.ToByte(numOVar3.Value));
		}

		void optOT1T2OR_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].T1AndOrT2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].T1AndOrT2, optOT1T2OR.Checked);
			OrderLabelRefresh();
		}
		void optOT3T4OR_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].T3AndOrT4 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].T3AndOrT4, optOT3T4OR.Checked);
			OrderLabelRefresh();
		}
		void optSkipOR_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			_mission.FlightGroups[_activeFG].Orders[r, o].SkipT1AndOrT2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].SkipT1AndOrT2, optSkipOR.Checked);
		}

		void txtOString_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			_mission.FlightGroups[_activeFG].Orders[r, _activeOrder].CustomText = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, _activeOrder].CustomText, txtOString.Text);
		}
		#endregion
		#region Goals
		void GoalLabelRefresh()
		{
			lblGoal[_activeFGGoal].Text = "Goal " + (_activeFGGoal + 1).ToString() + ": " + _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].ToString();
		}

		void lblGoalArr_Click(object sender, EventArgs e)
		{
			//[JB] Fixes refresh on paste.  Copied from XvT code.
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeFGGoal = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeFGGoal = Convert.ToByte(sender); l = lblGoal[_activeFGGoal]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<8;i++) if (i!=_activeFGGoal) lblGoal[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboGoalArgument.SelectedIndex = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Argument;
			cboGoalTrigger.SelectedIndex = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Condition;
			cboGoalAmount.SelectedIndex = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Amount;
			numGoalPoints.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Points;
			chkGoalEnable.Checked = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Enabled;
			numGoalTeam.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Team + 1;
			cboGoalPara.SelectedIndex = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Parameter;
			numGoalActSeq.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].ActiveSequence;
			txtGoalInc.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].IncompleteText;
			txtGoalComp.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].CompleteText;
			txtGoalFail.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].FailedText;
			_loading = btemp;
		}
		void lblGoalArr_DoubleClick(object sender, EventArgs e)
		{
			menuPaste_Click("Goal", new EventArgs());
		}
		void lblGoalArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Goal", new EventArgs());
		}

		void cboGoalPara_SelectedIndexChanged(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Parameter = (byte)cboGoalPara.SelectedIndex;
		}

		void chkGoalEnable_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Enabled = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Enabled, chkGoalEnable.Checked);
			GoalLabelRefresh();
		}

		void grpGoal_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal][0] = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal][0], Convert.ToByte(cboGoalArgument.SelectedIndex));
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal][1] = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal][1], Convert.ToByte(cboGoalTrigger.SelectedIndex));
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal][2] = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal][2], Convert.ToByte(cboGoalAmount.SelectedIndex));
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal][6] = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal][6], Convert.ToByte(cboGoalPara.SelectedIndex));
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal][7] = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal][7], Convert.ToByte(numGoalActSeq.Value));
			GoalLabelRefresh();
		}

		void numGoalActSeq_ValueChanged(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].ActiveSequence = (byte)numGoalActSeq.Value;
		}
		void numGoalPoints_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Points = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Points, (short)numGoalPoints.Value);
			GoalLabelRefresh();
		}
		void numGoalTeam_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Team = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Team, Convert.ToByte(numGoalTeam.Value - 1));
		}

		void txtGoalComp_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].CompleteText = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].CompleteText, txtGoalComp.Text);
		}
		void txtGoalFail_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].FailedText = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].FailedText, txtGoalFail.Text);
		}
		void txtGoalInc_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].IncompleteText = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].IncompleteText, txtGoalInc.Text);
		}
		#endregion
		#region Waypoints
		void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			int index = (int)c.Tag;
			if (index < 4) _mission.FlightGroups[_activeFG].Waypoints[index].Enabled = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[index].Enabled, c.Checked);
			else
			{
				int order = cboWP.SelectedIndex % 4;
				int region = cboWP.SelectedIndex / 4;
				_mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[index - 4].Enabled = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[index - 4].Enabled, c.Checked);
			}
		}
		void numWP_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Waypoints[0].Region = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[0].Region, Convert.ToByte(numSP1.Value-1));
			_mission.FlightGroups[_activeFG].Waypoints[1].Region = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[1].Region, Convert.ToByte(numSP2.Value-1));
			_mission.FlightGroups[_activeFG].Waypoints[2].Region = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[2].Region, Convert.ToByte(numSP3.Value-1));
			_mission.FlightGroups[_activeFG].Waypoints[3].Region = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[3].Region, Convert.ToByte(numHYP.Value-1));
		}

		void cboWP_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboWP.SelectedIndex == -1) return;
			bool btemp = _loading;
			_loading = true;
			int order = cboWP.SelectedIndex % 4;
			int region = cboWP.SelectedIndex / 4;
			for (int i=0;i<8;i++)
			{
				for (int j=0;j<3;j++)
				{
					_tableOrderRaw.Rows[i][j] = _mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[i][j];
					_tableOrder.Rows[i][j] = Math.Round((double)_mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[i][j] / 160, 2);
				}
				chkWP[i + 4].Checked = _mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[i].Enabled;
			}
			_tableOrder.AcceptChanges();
			_tableOrderRaw.AcceptChanges();
			_loading = btemp;
		}

		void numPitch_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Pitch = Common.Update(this, _mission.FlightGroups[_activeFG].Pitch, (short)numPitch.Value);
		}
		void numRoll_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Roll = Common.Update(this, _mission.FlightGroups[_activeFG].Roll, (short)numRoll.Value);
		}
		void numYaw_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Yaw = Common.Update(this, _mission.FlightGroups[_activeFG].Yaw, (short)numYaw.Value);
		}

		void tableWP_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;
			int i, j=0;
			_loading = true;
			for (j=0;j<4;j++) if (_tableWP.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = (short)Math.Round(Convert.ToDouble(_tableWP.Rows[j][i]) * 160);
					_mission.FlightGroups[_activeFG].Waypoints[j][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[j][i], raw);
					_tableWPRaw.Rows[j][i] = raw;
				}
			}
			catch { for (i=0;i<3;i++) _tableWP.Rows[j][i] = Math.Round((double)(_mission.FlightGroups[_activeFG].Waypoints[j][i]) / 160, 2); }	// reset
			_loading = false;
		}
		void tableWPRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;
			int i, j=0;
			_loading = true;
			for (j=0;j<4;j++) if (_tableWPRaw.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = Convert.ToInt16(_tableWPRaw.Rows[j][i]);
					_mission.FlightGroups[_activeFG].Waypoints[j][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[j][i], raw);
					_tableWP.Rows[j][i] = Math.Round((double)raw / 160, 2);
				}
			}
			catch { for (i=0;i<3;i++) _tableWPRaw.Rows[j][i] = _mission.FlightGroups[_activeFG].Waypoints[j][i]; }
			_loading = false;
		}
		void tableOrder_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;
			int i, j=0;
			_loading = true;
			for (j=0;j<8;j++) if (_tableOrder.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			int order = cboWP.SelectedIndex % 4;
			int region = cboWP.SelectedIndex / 4;
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = (short)Math.Round(Convert.ToDouble(_tableOrder.Rows[j][i]) * 160);
					_mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[j][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[j][i], raw);
					_tableOrderRaw.Rows[j][i] = raw;
				}
			}
			catch { for (i=0;i<3;i++) _tableOrder.Rows[j][i] = Math.Round((double)(_mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[j][i]) / 160, 2); }	// reset
			_loading = false;
		}
		void tableOrderRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;
			int i, j=0;
			_loading = true;
			for (j=0;j<8;j++) if (_tableOrderRaw.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			int order = cboWP.SelectedIndex % 4;
			int region = cboWP.SelectedIndex / 4;
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = Convert.ToInt16(_tableOrderRaw.Rows[j][i]);
					_mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[j][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[j][i], raw);
					_tableOrder.Rows[j][i] = Math.Round((double)raw / 160, 2);
				}
			}
			catch { for (i = 0; i < 3; i++) _tableOrderRaw.Rows[j][i] = _mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[j][i]; }
			_loading = false;
		}
		#endregion
		#region Options
		void EnableOptCat(bool b)
		{
			numOptCraft.Enabled = b;
			numOptWaves.Enabled = b;
			cboOptCraft.Enabled = b;
			for (int i=0;i<10;i++) lblOptCraft[i].Enabled = b;
		}
		void OptCraftLabelRefresh()
		{
			lblOptCraft[_activeOptionCraft].Text = "Craft " + (_activeOptionCraft+1).ToString() + ":";
			if (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType != 0)
				lblOptCraft[_activeOptionCraft].Text += " " + (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves+1).ToString()
				+ " x (" + (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft+1).ToString() + ") " + Strings.CraftType[_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType];//[JB] Fixed Strings.CraftType[cboOptCraft.SelectedIndex];   //[JB] Fix so all labels properly refresh in load time, getting the craft name directly from the setting rather than the combobox
		}

		void chkOptArr_CheckedChanged(object sender, EventArgs e)
		{
			//[JB] Incremented all indexes that were 8 or higher to accomodate Ion Pulse addition.
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			int i = (int)c.Tag;
			_mission.FlightGroups[_activeFG].OptLoadout[i] = Common.Update(this, _mission.FlightGroups[_activeFG].OptLoadout[i], c.Checked);
			bool btemp = _loading;
			_loading = true;
			if (c.Checked)
			{
				if (i == 0) for (int j=1;j<9;j++) _mission.FlightGroups[_activeFG].OptLoadout[j] = false;	// turn off warheads
				else if (i > 0 && i < 9) _mission.FlightGroups[_activeFG].OptLoadout[0] = false;		// clear warhead None
				else if (i == 9) for (int j=10;j<13;j++) _mission.FlightGroups[_activeFG].OptLoadout[j] = false;	// turn off beams
				else if (i > 9 && i < 13) _mission.FlightGroups[_activeFG].OptLoadout[9] = false;	// clear beam None
				else if (i == 13) { _mission.FlightGroups[_activeFG].OptLoadout[14] = false; _mission.FlightGroups[_activeFG].OptLoadout[15] = false; }	// turn off CMs
				else _mission.FlightGroups[_activeFG].OptLoadout[13] = false;	// clear CM None
			}
			else
			{
				//[JB] Changed variable in for() loop from "i" to "j" to avoid overwriting
				//Fixed grouping for "b" since unchecking would erase other remaining groups.
				bool b = false;
				if (i > 0 && i < 9) {
					for (int j = 1; j < 9; j++) b |= _mission.FlightGroups[_activeFG].OptLoadout[j];
					if (!b && !chkOpt[0].Checked) _mission.FlightGroups[_activeFG].OptLoadout[0] = true; }
				b = false;
				if (i > 9 && i < 13) {
					for (int j = 10; j < 13; j++) b |= _mission.FlightGroups[_activeFG].OptLoadout[j];
					if (!b && !chkOpt[9].Checked) _mission.FlightGroups[_activeFG].OptLoadout[9] = true; }
				b = false;
				if (i > 13 && i < 16) {
					for (int j = 14; j < 16; j++) b |= _mission.FlightGroups[_activeFG].OptLoadout[j];
					if (!b && !chkOpt[13].Checked) _mission.FlightGroups[_activeFG].OptLoadout[13] = true; }
			}
			for (i=0;i<16;i++) chkOpt[i].Checked = _mission.FlightGroups[_activeFG].OptLoadout[i];
			_loading = btemp;
		}
		void lblOptCraftArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			_activeOptionCraft = Convert.ToByte(l.Tag);
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<10;i++) if (i!=_activeOptionCraft) lblOptCraft[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboOptCraft.SelectedIndex = _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType;
			numOptCraft.Value = _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft+1;
			numOptWaves.Value = _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves+1;
			_loading = btemp;
		}
		void lblSkipTrigArr_Click(object sender, EventArgs e)
		{
			//[JB] Copied XvT code to fix updating on paste.
			//Label l = (Label)sender;
			//Label ll = (l == lblSkipTrig1 ? lblSkipTrig2 : lblSkipTrig1);
			Label l = null; // clicked
			Label ll = null; // other one
			int i = 0;
			try
			{
				l = (Label)sender;
				l.Focus();
				ll = (l==lblSkipTrig1 ? lblSkipTrig2 : lblSkipTrig1);
				i = (l == lblSkipTrig1 ? 0 : 1);	// i = clicked trigger
			}
			catch (InvalidCastException)
			{
				i = (int)sender;	// i = clicked trigger from code
				if (i == 0) { l = lblSkipTrig1; ; ll = lblSkipTrig2; }
				else { l = lblSkipTrig2; ll = lblSkipTrig1; }
			}
			int r = 0, o = 0;
			//l.Focus();
			i = (l == lblSkipTrig1 ? 0 : 1);
			r = cboSkipOrder.SelectedIndex / 4;
			o = cboSkipOrder.SelectedIndex % 4;
			Mission.Trigger trigger = _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i];
			l.ForeColor = SystemColors.Highlight;
			ll.ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboSkipTrig.SelectedIndex = trigger.Condition;
			cboSkipType.SelectedIndex = -1;
			cboSkipType.SelectedIndex = trigger.VariableType;
			cboSkipAmount.SelectedIndex = trigger.Amount;
			cboSkipPara.SelectedIndex = trigger.Parameter1;
			numSkipPara.Value = trigger.Parameter2;
			_loading = btemp;
		}
		void lblSkipTrigArr_DoubleClick(object sender, EventArgs e)
		{
			if(((MouseEventArgs)e).Button != MouseButtons.Left) return;  //[JB] Avoid pasting for rapid clicks that aren't Left.
			menuPaste_Click("Skip", new EventArgs());
		}
		void lblSkipTrigArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Skip", new EventArgs());
		}

		void cboOptCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].OptCraftCategory = Common.Update(this, _mission.FlightGroups[_activeFG].OptCraftCategory, (FlightGroup.OptionalCraftCategory)Convert.ToByte(cboOptCat.SelectedIndex));
			if (cboOptCat.SelectedIndex == 4) EnableOptCat(true);
			else EnableOptCat(false);
		}
		void cboOptCraft_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType = Common.Update(this, _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType, Convert.ToByte(cboOptCraft.SelectedIndex));
			OptCraftLabelRefresh();
		}
		void cboSkipAmount_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Amount = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Amount, Convert.ToByte(cboSkipAmount.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}
		void cboSkipOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[0], lblSkipTrig1);
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[1], lblSkipTrig2);
			lblSkipTrigArr_Click(lblSkipTrig1, new EventArgs());
			optSkipOR.Checked = _mission.FlightGroups[_activeFG].Orders[r, o].SkipT1AndOrT2;
			optSkipAND.Checked = !optSkipOR.Checked;
			if(!_loading && !cboSkipOrder.Focused) cboSkipOrder.Focus();  //[JB] Return focus back to self, so it can be navigated with arrow keys without giving focus to other controls.
		}
		void cboSkipPara_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Parameter1 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Parameter1, Convert.ToByte(cboSkipPara.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}
		void cboSkipTrig_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Condition = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Condition, Convert.ToByte(cboSkipTrig.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}
		void cboSkipType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboSkipType.SelectedIndex == -1) return;
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].VariableType = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].VariableType, Convert.ToByte(cboSkipType.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].VariableType, cboSkipVar);
			try { cboSkipVar.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Variable; }
			catch { cboSkipVar.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Variable = 0; }
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i], (i == 0 ? lblSkipTrig1 : lblSkipTrig2));
		}
		void cboSkipVar_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Variable = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Variable, Convert.ToByte(cboSkipVar.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}

		void chkRole1_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].EnableDesignation1 = Common.Update(this, _mission.FlightGroups[_activeFG].EnableDesignation1, chkRole1.Checked);
			cboRole1.Enabled = chkRole1.Checked;
		}
		void chkRole2_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].EnableDesignation2 = Common.Update(this, _mission.FlightGroups[_activeFG].EnableDesignation2, chkRole2.Checked);
			cboRole2.Enabled = chkRole2.Checked;
		}

		void cmdCopySkip_Click(object sender, EventArgs e)
		{
			menuCopy_Click("Skip", new EventArgs());
		}
		void cmdPasteSkip_Click(object sender, EventArgs e)
		{
			menuPaste_Click("Skip", new EventArgs());
		}

		void grpRole_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Role = Common.Update(this, _mission.FlightGroups[_activeFG].Role, txtRole.Text);
			_mission.FlightGroups[_activeFG].Designation1 = Common.Update(this, _mission.FlightGroups[_activeFG].Designation1, Convert.ToByte(cboRole1.SelectedIndex));
			_mission.FlightGroups[_activeFG].Designation2 = Common.Update(this, _mission.FlightGroups[_activeFG].Designation2, Convert.ToByte(cboRole2.SelectedIndex));
		}

		void numExplode_ValueChanged(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ExplosionTime = (byte)numExplode.Value;
		}
		void numOptCraft_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft = Common.Update(this, _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft, Convert.ToByte(numOptCraft.Value - 1));
			OptCraftLabelRefresh();
		}
		void numOptWaves_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves = Common.Update(this, _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves, Convert.ToByte(numOptWaves.Value - 1));
			OptCraftLabelRefresh();
		}
		void numSkipPara_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Parameter2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Parameter2, Convert.ToByte(numSkipPara.Value));
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}

		void optSkipOR_Leave(object sender, EventArgs e)
		{
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			_mission.FlightGroups[_activeFG].Orders[r, o].SkipT1AndOrT2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].SkipT1AndOrT2, optSkipOR.Checked);
		}

		void txtPilot_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].PilotID = Common.Update(this, _mission.FlightGroups[_activeFG].PilotID, txtPilot.Text);
		}
		#endregion
		#region Unknowns
		void cboUnkOrder_Enter(object sender, EventArgs e)
		{	// use Enter to detect when user is about to change index, save what's there
			int r = cboUnkOrder.SelectedIndex / 4;
			int o = cboUnkOrder.SelectedIndex % 4;
			_mission.FlightGroups[_activeFG].Orders[r,o].Unknown9 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r,o].Unknown9, Convert.ToByte(numUnk9.Value));
			_mission.FlightGroups[_activeFG].Orders[r, o].Unknown10 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].Unknown10, Convert.ToByte(numUnk10.Value));
			_mission.FlightGroups[_activeFG].Orders[r, o].Unknown11 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].Unknown11, chkUnk11.Checked);
			_mission.FlightGroups[_activeFG].Orders[r, o].Unknown12 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].Unknown12, chkUnk12.Checked);
			_mission.FlightGroups[_activeFG].Orders[r, o].Unknown13 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].Unknown13, chkUnk13.Checked);
			_mission.FlightGroups[_activeFG].Orders[r, o].Unknown14 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[r, o].Unknown14, chkUnk14.Checked);
		}
		void cboUnkOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			int r = cboUnkOrder.SelectedIndex / 4;
			int o = cboUnkOrder.SelectedIndex % 4;
			numUnk9.Value = _mission.FlightGroups[_activeFG].Orders[r, o].Unknown9;
			numUnk10.Value = _mission.FlightGroups[_activeFG].Orders[r, o].Unknown10;
			chkUnk11.Checked = _mission.FlightGroups[_activeFG].Orders[r, o].Unknown11;
			chkUnk12.Checked = _mission.FlightGroups[_activeFG].Orders[r, o].Unknown12;
			chkUnk13.Checked = _mission.FlightGroups[_activeFG].Orders[r, o].Unknown13;
			chkUnk14.Checked = _mission.FlightGroups[_activeFG].Orders[r, o].Unknown14;
		}

		void chkUnk15_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[(int)numUnkGoal.Value-1].Unknown15 = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[(int)numUnkGoal.Value-1].Unknown15, chkUnk15.Checked);
		}

		void grpUnkAD_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Unknowns.Unknown5 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown5, Convert.ToByte(numUnk5.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown6 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown6, chkUnk6.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown7 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown7, Convert.ToByte(numUnk7.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown8 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown8, Convert.ToByte(numUnk8.Value));
		}
		void grpUnkCraft_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Unknowns.Unknown1 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown1, Convert.ToByte(numUnk1.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown3 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown3, Convert.ToByte(numUnk3.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown4 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown4, Convert.ToByte(numUnk4.Value));
		}
		void grpUnkOther_Leave(object sender, EventArgs e)
		{	// okay, lots of stuff :P
			_mission.FlightGroups[_activeFG].Unknowns.Unknown16 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown16, Convert.ToByte(numUnk16.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown17 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown17, Convert.ToByte(numUnk17.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown18 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown18, Convert.ToByte(numUnk18.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown19 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown19, Convert.ToByte(numUnk19.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown20 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown20, Convert.ToByte(numUnk20.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown21 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown21, Convert.ToByte(numUnk21.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown22 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown22, chkUnk22.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown23 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown23, Convert.ToByte(numUnk23.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown24 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown24, Convert.ToByte(numUnk24.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown25 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown25, Convert.ToByte(numUnk25.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown26 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown26, Convert.ToByte(numUnk26.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown27 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown27 ,Convert.ToByte( numUnk27.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown28 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown28, Convert.ToByte(numUnk28.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown29 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown29, chkUnk29.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown30 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown30, chkUnk30.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown31 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown31, chkUnk31.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown32 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown32, Convert.ToByte(numUnk32.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown33 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown33, Convert.ToByte(numUnk33.Value));
			_mission.FlightGroups[_activeFG].Unknowns.Unknown34 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown34, chkUnk34.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown35 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown35, chkUnk35.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown36 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown36, chkUnk36.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown37 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown37, chkUnk37.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown38 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown38, chkUnk38.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown39 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown39, chkUnk39.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown40 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown40, chkUnk40.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown41 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown41, chkUnk41.Checked);  //[JB] Fixed, was 31.
		}
		void grpUnkOrder_Leave(object sender, EventArgs e)
		{
			cboUnkOrder_Enter("grpUnkOrder_Leave", new EventArgs());	// meh, no need to type it out again
		}

		void numUnkGoal_ValueChanged(object sender, EventArgs e)
		{
			chkUnk15.Checked = _mission.FlightGroups[_activeFG].Goals[(int)numUnkGoal.Value-1].Unknown15;
		}
		#endregion
		#endregion
		#region Messages
		void DeleteMess()
		{
			if(_activeMessage < 0 || _activeMessage >= _mission.Messages.Count)  //[JB] Added check
				return;
			lstMessages.Items.RemoveAt(_activeMessage); //[JB] Need to delete from list before _activeMessage changes, otherwise it may remove the wrong index.
			_activeMessage = _mission.Messages.RemoveAt(_activeMessage);
			if (_mission.Messages.Count == 0)
			{
				lstMessages.Items.Clear();
				EnableMessages(false);
				lblMessage.Text = "Message #0 of 0";
				return;
			}
			lstMessages.SelectedIndex = _activeMessage;
			Common.Title(this, _loading);
		}
		void EnableMessages(bool state)
		{
			grpMessages.Enabled = state;
			grpMessCancel.Enabled = state;
			grpMessUnk.Enabled = state;
			txtMessage.Enabled = state;
			txtMessNote.Enabled = state; //[JB] Fix so edit box is disabled with others, prevents exception when leaving focus when zero messages.
			grpSend.Enabled = state;
			numMessDelaySec.Enabled = state;
			numMessDelayMin.Enabled = state;
			cboMessTrig.Enabled = state;
			cboMessType.Enabled = state;
			cboMessVar.Enabled = state;
			cboMessPara.Enabled = state;
			numMessPara.Enabled = state;
			cboMessAmount.Enabled = state;
			cboMessColor.Enabled = state;
			txtVoice.Enabled = state;
			cboMessFG.Enabled = state;
		}
		void MessListRefresh()
		{
			if (_mission.Messages.Count == 0) return;
			lstMessages.Items[_activeMessage] = _mission.Messages[_activeMessage].MessageString;
		}
		void NewMess()
		{
			if (_mission.Messages.Count == Mission.MessageLimit)
			{
				MessageBox.Show("Mission contains maximum number of Messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			_activeMessage = _mission.Messages.Add();
			if (_mission.Messages.Count == 1) EnableMessages(true);
			lstMessages.Items.Add(_mission.Messages[_activeMessage].MessageString);
			lstMessages.SelectedIndex = _activeMessage;
			Common.Title(this, _loading);
		}

		void lstMessages_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (_mission.Messages.Count == 0 || _mission.Messages[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch (_mission.Messages[e.Index].Color)
			{
				case 0:
					brText = Brushes.LimeGreen;
					break;
				case 1:
					brText = Brushes.Crimson;
					break;
				case 2:
					brText = Brushes.RoyalBlue;
					break;
				case 3:
					brText = Brushes.Yellow;
					break;
				case 4:
					brText = Brushes.Red;
					break;
				case 5:
					brText = Brushes.DarkOrchid;
					break;
			}
			e.Graphics.DrawString(lstMessages.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMessages.SelectedIndex == -1) return;
			_activeMessage = lstMessages.SelectedIndex;
			lblMessage.Text = "Message #" + (_activeMessage+1).ToString() + " of " + _mission.Messages.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			for (int i=0;i<6;i++) labelRefresh(_mission.Messages[_activeMessage].Triggers[i], lblMessTrig[i]);
			txtMessage.Text = _mission.Messages[_activeMessage].MessageString;
			txtMessNote.Text = _mission.Messages[_activeMessage].Note;
			cboMessColor.SelectedIndex = _mission.Messages[_activeMessage].Color;
			for (int i=0;i<4;i++)
			{
				optMessAndOr[i].Checked = _mission.Messages[_activeMessage].TrigAndOr[i];
				optMessAndOr[i+4].Checked = !optMessAndOr[i].Checked;
			}
			numMessDelaySec.Value = _mission.Messages[_activeMessage].DelaySeconds;
			numMessDelayMin.Value = _mission.Messages[_activeMessage].DelayMinutes;
			for (int i=0;i<10;i++) chkSendTo[i].Checked = _mission.Messages[_activeMessage].SentTo[i];
			numMessUnk1.Value = _mission.Messages[_activeMessage].Unknown1;
			chkMessUnk2.Checked = _mission.Messages[_activeMessage].Unknown2;
			txtVoice.Text = _mission.Messages[_activeMessage].VoiceID;
			cboMessFG.SelectedIndex = _mission.Messages[_activeMessage].OriginatingFG;
			lblMessTrigArr_Click(0, new EventArgs());  //[JB] Was lblMessTrig[0].  Changed to XvT code of "0").
			_loading = btemp;
		}

		void chkSendToArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			_mission.Messages[_activeMessage].SentTo[(int)c.Tag] = Common.Update(this, _mission.Messages[_activeMessage].SentTo[(int)c.Tag], c.Checked);
		}
		void lblMessTrigArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeMessageTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeMessageTrigger = Convert.ToByte(sender); l = lblMessTrig[_activeMessageTrigger]; }
			/* [JB] Copied try/catch code from XvT.
			 * txtMessage_Leave() was triggering lstMessages_SelectedIndexChanged() via MessListRefresh()
			 * lstMessages_SelectedIndexChanged() then called lblMessTrigArr_Click(lblMessTrig[0], new EventArgs());
			 * which was causing l.Focus() to trigger a duplicate txtMessage_Leave() event which was overriding a direct user click on lstMessages
			 * Copying this try/catch code from XvT fixes a bug where clicking lstMessages directly wouldn't update the actual selection because duplicate Leave event triggers were being fired.*/

			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<6;i++) if (i!=_activeMessageTrigger) lblMessTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			//parameterRefresh(cboMessPara);
			cboMessTrig.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Condition;
			cboMessType.SelectedIndex = -1;
			cboMessType.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].VariableType;
			cboMessAmount.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Amount;
			cboMessPara.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Parameter1;
			numMessPara.Value = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Parameter2;
			_loading = btemp;
		}
		void lblMessTrigArr_DoubleClick(object sender, EventArgs e)
		{
			menuPaste_Click("MessTrig", new EventArgs());
		}
		void lblMessTrigArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("MessTrig", new EventArgs());
		}
		void optMessAndOrArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			RadioButton r = (RadioButton)sender;
			_mission.Messages[_activeMessage].TrigAndOr[(int)r.Tag] = Common.Update(this, _mission.Messages[_activeMessage].TrigAndOr[(int)r.Tag], r.Checked);
		}

		void cboMessAmount_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Amount = Common.Update(this, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Amount, Convert.ToByte(cboMessAmount.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
		}
		void cboMessColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.Messages[_activeMessage].Color = Common.Update(this, _mission.Messages[_activeMessage].Color, Convert.ToByte(cboMessColor.SelectedIndex));
			MessListRefresh();
		}
		void cboMessFG_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].OriginatingFG = Common.Update(this, _mission.Messages[_activeMessage].OriginatingFG, Convert.ToByte(cboMessFG.SelectedIndex));
		}
		void cboMessPara_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Parameter1 = Common.Update(this, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Parameter1, Convert.ToByte(cboMessPara.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
		}
		void cboMessTrig_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Condition = Common.Update(this, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Condition, Convert.ToByte(cboMessTrig.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
		}
		void cboMessType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMessType.SelectedIndex == -1) return;
			if (!_loading)
				_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].VariableType = Common.Update(this, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].VariableType, Convert.ToByte(cboMessType.SelectedIndex));
			comboVarRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].VariableType, cboMessVar);
			try { cboMessVar.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Variable; }
			catch { cboMessVar.SelectedIndex = 0; _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Variable = 0; }
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
		}
		void cboMessVar_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Variable = Common.Update(this, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Variable, Convert.ToByte(cboMessVar.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
		}

		void chkMessUnk2_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Unknown2 = Common.Update(this, _mission.Messages[_activeMessage].Unknown2, chkMessUnk2.Checked);
		}

		void numMessDelayMin_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].DelayMinutes = Common.Update(this, _mission.Messages[_activeMessage].DelayMinutes, Convert.ToByte(numMessDelayMin.Value));
		}
		void numMessDelaySec_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].DelaySeconds = Common.Update(this, _mission.Messages[_activeMessage].DelaySeconds, Convert.ToByte(numMessDelaySec.Value));
		}
		void numMessPara_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Parameter2 = Common.Update(this, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Parameter2, Convert.ToByte(numMessPara.Value));  //[JB] Fixed exception when leaving on zero value. No other references seem to use such an offset. (numMessPara.Value - 1)
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);	// don't know if I need this...
		}
		void numMessUnk1_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Unknown1 = Common.Update(this, _mission.Messages[_activeMessage].Unknown1, Convert.ToByte(numMessUnk1.Value));
		}

		void txtMessage_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].MessageString = Common.Update(this, _mission.Messages[_activeMessage].MessageString, txtMessage.Text);
			MessListRefresh();
		}
		void txtVoice_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].VoiceID = Common.Update(this, _mission.Messages[_activeMessage].VoiceID, txtVoice.Text);
		}
		void txtMessNote_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Note = Common.Update(this, _mission.Messages[_activeMessage].Note, txtMessNote.Text);
		}
		#endregion
		#region Globals
		void cboGlobalTeam_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalTeam.SelectedIndex == -1) return;
			if (!_loading) lblGlobTrigArr_Click(lblGlobTrig[_activeTeam], new EventArgs());	// re-click current lbl with save
			_activeTeam = (byte)cboGlobalTeam.SelectedIndex;
			lblTeamArr_Click(lblTeam[_activeTeam], new EventArgs());	// link the Globals and Team tabs to share GlobTeam
			bool btemp = _loading;
			_loading = true;
			for (int i=0;i<12;i++) labelRefresh(_mission.Globals[_activeTeam].Goals[i/4].Triggers[i%4], lblGlobTrig[i]);
			for (int i = 0; i < 9; i++)
			{
				optGlobAndOr[i * 2].Checked = _mission.Globals[_activeTeam].Goals[i / 3].AndOr[i % 3];	// OR
				optGlobAndOr[i * 2 + 1].Checked = !optGlobAndOr[i * 2].Checked;	// AND
			}
			lblGlobTrigArr_Click(lblGlobTrig[0], new EventArgs());	// click first lbl with no save
			_loading = btemp;
		}

		void lblGlobTrigArr_Click(object sender, EventArgs e)
		{
			//[JB] Fixes bug where Goal pasting wouldn't refresh the label.  Copied try/catch code from XvT.
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeGlobalTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeGlobalTrigger = Convert.ToByte(sender); l = lblGlobTrig[_activeGlobalTrigger]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<12;i++) if (i!=_activeGlobalTrigger) lblGlobTrig[i].ForeColor = SystemColors.ControlText;
			int goal = _activeGlobalTrigger / 4;
			int trig = _activeGlobalTrigger % 4;
			bool btemp = _loading;
			_loading = true;
			cboGlobalTrig.SelectedIndex = _mission.Globals[_activeTeam].Goals[goal].Triggers[trig].Condition;
			cboGlobalType.SelectedIndex = -1;
			cboGlobalType.SelectedIndex = _mission.Globals[_activeTeam].Goals[goal].Triggers[trig].VariableType;
			cboGlobalAmount.SelectedIndex = _mission.Globals[_activeTeam].Goals[goal].Triggers[trig].Amount;
			cboGlobalPara.SelectedIndex = _mission.Globals[_activeTeam].Goals[goal].Triggers[trig].Parameter1; //[JB] Fixed, don't need  + 1;
			numGlobalPara.Value = _mission.Globals[_activeTeam].Goals[goal].Triggers[trig].Parameter2;
			numGlobalPoints.Value = _mission.Globals[_activeTeam].Goals[goal].Points;
			txtGlobalInc.Text = _mission.Globals[_activeTeam].Goals[goal].GoalStrings[trig, (int)Globals.GoalState.Incomplete];
			txtGlobalComp.Text = _mission.Globals[_activeTeam].Goals[goal].GoalStrings[trig, (int)Globals.GoalState.Complete];
			txtGlobalFail.Text = _mission.Globals[_activeTeam].Goals[goal].GoalStrings[trig, (int)Globals.GoalState.Failed];
			numGlobActSeq.Value = _mission.Globals[_activeTeam].Goals[goal].ActiveSequence;
			chkGlobUnk1.Checked = _mission.Globals[_activeTeam].Goals[goal].Unknown1;
			chkGlobUnk2.Checked = _mission.Globals[_activeTeam].Goals[goal].Unknown2;
			numGlobUnk3.Value = _mission.Globals[_activeTeam].Goals[goal].Unknown3;
			numGlobUnk4.Value = _mission.Globals[_activeTeam].Goals[goal].Unknown4;
			numGlobUnk5.Value = _mission.Globals[_activeTeam].Goals[goal].Unknown5;
			numGlobUnk6.Value = _mission.Globals[_activeTeam].Goals[goal].Unknown6;
			txtGlobalFail.Visible = (goal < 1);
			txtGlobalInc.Visible = (goal < 2);
			_loading = btemp;
		}
		void lblGlobTrigArr_DoubleClick(object sender, EventArgs e)
		{
			if(((MouseEventArgs)e).Button != MouseButtons.Left) return;  //[JB] Sometimes rapid single clicks were tricking this, like left to select, right to copy would trigger a paste instead.
			menuPaste_Click("Glob", new EventArgs());
		}
		void lblGlobTrigArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Glob", new EventArgs());
		}
		void optGlobAndOrArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			RadioButton r = (RadioButton)sender;
			int goal = (int)r.Tag / 6;
			int index = ((int)r.Tag / 2) % 3;
			_mission.Globals[_activeTeam].Goals[goal].AndOr[index] = Common.Update(this, _mission.Globals[_activeTeam].Goals[goal].AndOr[index], r.Checked);
		}

		void cboGlobalAmount_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Amount = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Amount, Convert.ToByte(cboGlobalAmount.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger/4].Triggers[_activeGlobalTrigger%4], lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalPara_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Parameter1 = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Parameter1, Convert.ToByte(cboGlobalPara.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4], lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalTrig_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Condition = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Condition, Convert.ToByte(cboGlobalTrig.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4], lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalType.SelectedIndex == -1) return;
			if (!_loading)
				_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].VariableType = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].VariableType, Convert.ToByte(cboGlobalType.SelectedIndex));
			comboVarRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].VariableType, cboGlobalVar);
			try { cboGlobalVar.SelectedIndex = _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Variable; }
			catch { cboGlobalVar.SelectedIndex = 0; _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Variable = 0; }
			labelRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4], lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalVar_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Variable = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Variable, Convert.ToByte(cboGlobalVar.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4], lblGlobTrig[_activeGlobalTrigger]);
		}

		void grpGlobUnk_Leave(object sender, EventArgs e)
		{
			int goal = _activeGlobalTrigger / 4;
			_mission.Globals[_activeTeam].Goals[goal].Unknown1 = Common.Update(this, _mission.Globals[_activeTeam].Goals[goal].Unknown1, chkGlobUnk1.Checked);
			_mission.Globals[_activeTeam].Goals[goal].Unknown2 = Common.Update(this, _mission.Globals[_activeTeam].Goals[goal].Unknown2, chkGlobUnk2.Checked);
			_mission.Globals[_activeTeam].Goals[goal].Unknown3 = Common.Update(this, _mission.Globals[_activeTeam].Goals[goal].Unknown3, Convert.ToByte(numGlobUnk3.Value));
			_mission.Globals[_activeTeam].Goals[goal].Unknown4 = Common.Update(this, _mission.Globals[_activeTeam].Goals[goal].Unknown4, Convert.ToByte(numGlobUnk4.Value));
			_mission.Globals[_activeTeam].Goals[goal].Unknown5 = Common.Update(this, _mission.Globals[_activeTeam].Goals[goal].Unknown5, Convert.ToByte(numGlobUnk5.Value));
			_mission.Globals[_activeTeam].Goals[goal].Unknown6 = Common.Update(this, _mission.Globals[_activeTeam].Goals[goal].Unknown6, Convert.ToByte(numGlobUnk6.Value));
		}

		//[JB] Added function for inactive control
		private void numGlobActSeq_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].ActiveSequence = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].ActiveSequence, Convert.ToByte(numGlobActSeq.Value));
		}	
		void numGlobalPara_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Parameter2 = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Parameter2, Convert.ToByte(numGlobalPara.Value));
			labelRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4], lblGlobTrig[_activeGlobalTrigger]);
		}
		void numGlobalPoints_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger/4].Points = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger/4].Points, (short)numGlobalPoints.Value);
		}

		void txtGlobalComp_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].GoalStrings[_activeGlobalTrigger % 4, (int)Globals.GoalState.Complete] = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].GoalStrings[_activeGlobalTrigger % 4, (int)Globals.GoalState.Complete], txtGlobalComp.Text);
		}
		void txtGlobalFail_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].GoalStrings[_activeGlobalTrigger % 4, (int)Globals.GoalState.Failed] = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].GoalStrings[_activeGlobalTrigger % 4, (int)Globals.GoalState.Failed], txtGlobalFail.Text);
		}
		void txtGlobalInc_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].GoalStrings[_activeGlobalTrigger % 4, (int)Globals.GoalState.Incomplete] = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].GoalStrings[_activeGlobalTrigger % 4, (int)Globals.GoalState.Incomplete], txtGlobalInc.Text);
		}
		#endregion
		#region Teams
		void TeamRefresh()
		{
			string team = _mission.Teams[_activeTeam].Name;
			lblTeam[_activeTeam].Text = "Team " + (_activeTeam+1).ToString() + ": " + team;
			cboGlobalTeam.Items[_activeTeam] = team;
			cboTeam.Items[_activeTeam] = team;
			ComboBox[] cboType = new ComboBox[8];
			cboType[0] = cboADTrigType;
			cboType[1] = cboOT1Type;
			cboType[2] = cboOT2Type;
			cboType[3] = cboOT3Type;
			cboType[4] = cboOT4Type;
			cboType[5] = cboSkipType;
			cboType[6] = cboMessType;
			cboType[7] = cboGlobalType;
			ComboBox[] cbo = new ComboBox[8];
			cbo[0] = cboADTrigVar;
			cbo[1] = cboOT1;
			cbo[2] = cboOT2;
			cbo[3] = cboOT3;
			cbo[4] = cboOT4;
			cbo[5] = cboSkipVar;
			cbo[6] = cboMessVar;
			cbo[7] = cboGlobalVar;
			for (int i=0;i<8;i++)
				if (cboType[i].SelectedIndex == 0xC || cboType[i].SelectedIndex == 0x15)
					cbo[i].Items[_activeTeam] = team;

			//[JB] Update the Briefing's list of team names.
			for (int i = 0; i < _mission.Teams.Count; i++)
				BriefingForm.sharedTeamNames[i] = _mission.Teams[i].Name;
		}

		void lblTeamArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			_activeTeam = Convert.ToByte(l.Tag);
			cboGlobalTeam.SelectedIndex = _activeTeam;
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<10;i++) if (i!=_activeTeam) lblTeam[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			txtTeamName.Text = _mission.Teams[_activeTeam].Name;
			for (int i=0;i<10;i++)
			{
				if (_mission.Teams[_activeTeam].Allies[i] == Team.Allegeance.Hostile) optAllies[i*3].Checked = true;
				else if (_mission.Teams[_activeTeam].Allies[i] == Team.Allegeance.Friendly) optAllies[i*3+1].Checked = true;
				else optAllies[i*3+2].Checked = true;
			}
			txtPMCVoiceID.Text = _mission.Teams[_activeTeam].VoiceIDs[0];
			txtPMFVoiceID.Text = _mission.Teams[_activeTeam].VoiceIDs[1];
			txtOMCVoiceID.Text = _mission.Teams[_activeTeam].VoiceIDs[2];
			txtPrimCompNote.Text = _mission.Teams[_activeTeam].EomNotes[0];
			txtPrimFailNote.Text = _mission.Teams[_activeTeam].EomNotes[1];
			txtSecCompNote.Text = _mission.Teams[_activeTeam].EomNotes[2];
			for (int i=0;i<6;i++)
			{
				txtEoM[i].Text = _mission.Teams[_activeTeam].EndOfMissionMessages[i];
				numTeamUnk[i].Value = _mission.Teams[_activeTeam].Unknowns[i];
			}
			_loading = btemp;
		}
		void optAlliesArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			RadioButton r = (RadioButton)sender;
			if (!r.Checked) return;
			int index = (int)r.Tag;
			Team.Allegeance a = ((index%3) == 0 ? Team.Allegeance.Hostile : ((index%3) == 1 ? Team.Allegeance.Friendly : Team.Allegeance.Neutral));
			_mission.Teams[_activeTeam].Allies[index/3] = Common.Update(this, _mission.Teams[_activeTeam].Allies[index/3], a);
		}

		void grpTeamOMC_Leave(object sender, EventArgs e)
		{
			_mission.Teams[_activeTeam].VoiceIDs[2] = Common.Update(this, _mission.Teams[_activeTeam].VoiceIDs[2], txtOMCVoiceID.Text);
			_mission.Teams[_activeTeam].EndOfMissionMessages[4] = Common.Update(this, _mission.Teams[_activeTeam].EndOfMissionMessages[4], txtEoM[4].Text);
			_mission.Teams[_activeTeam].EndOfMissionMessages[5] = Common.Update(this, _mission.Teams[_activeTeam].EndOfMissionMessages[5], txtEoM[5].Text);
			_mission.Teams[_activeTeam].EomNotes[2] = Common.Update(this, _mission.Teams[_activeTeam].EomNotes[2], txtSecCompNote.Text);
		}
		void grpTeamPMC_Leave(object sender, EventArgs e)
		{
			_mission.Teams[_activeTeam].VoiceIDs[0] = Common.Update(this, _mission.Teams[_activeTeam].VoiceIDs[0], txtPMCVoiceID.Text);
			_mission.Teams[_activeTeam].EndOfMissionMessages[0] = Common.Update(this, _mission.Teams[_activeTeam].EndOfMissionMessages[0], txtEoM[0].Text);
			_mission.Teams[_activeTeam].EndOfMissionMessages[1] = Common.Update(this, _mission.Teams[_activeTeam].EndOfMissionMessages[1], txtEoM[1].Text);
			_mission.Teams[_activeTeam].EomNotes[0] = Common.Update(this, _mission.Teams[_activeTeam].EomNotes[0], txtPrimCompNote.Text);
		}
		void grpTeamPMF_Leave(object sender, EventArgs e)
		{
			_mission.Teams[_activeTeam].VoiceIDs[1] = Common.Update(this, _mission.Teams[_activeTeam].VoiceIDs[1], txtPMFVoiceID.Text);
			_mission.Teams[_activeTeam].EndOfMissionMessages[2] = Common.Update(this, _mission.Teams[_activeTeam].EndOfMissionMessages[2], txtEoM[2].Text);
			_mission.Teams[_activeTeam].EndOfMissionMessages[3] = Common.Update(this, _mission.Teams[_activeTeam].EndOfMissionMessages[3], txtEoM[3].Text);
			_mission.Teams[_activeTeam].EomNotes[1] = Common.Update(this, _mission.Teams[_activeTeam].EomNotes[1], txtPrimFailNote.Text);
		}
		void grpTeamUnknowns_Leave(object sender, EventArgs e)
		{
			for(int i=0;i<6;i++)
				_mission.Teams[_activeTeam].Unknowns[i] = Common.Update(this, _mission.Teams[_activeTeam].Unknowns[i], Convert.ToByte(numTeamUnk[i].Value));
		}

		void txtTeamName_Leave(object sender, EventArgs e)
		{
			_mission.Teams[_activeTeam].Name = Common.Update(this, _mission.Teams[_activeTeam].Name, txtTeamName.Text);
			TeamRefresh();
		}
		#endregion
		#region Mission
		void UpdateMissionTabs()
		{
			#region M1
			txtMissDesc.Text = _mission.MissionDescription;
			txtMissSucc.Text = _mission.MissionSuccessful;
			txtMissFail.Text = _mission.MissionFailed;
			txtDescNote.Text = _mission.DescriptionNotes;
			txtFailNote.Text = _mission.FailedNotes;
			txtSuccNote.Text = _mission.SuccessfulNotes;
			cboHangar.SelectedIndex = (int)_mission.MissionType;
			cboOfficer.SelectedIndex = _mission.Officer;
			try { cboLogo.SelectedIndex = (int)_mission.Logo - 4; }
			catch { _mission.Logo = Mission.LogoEnum.None; cboLogo.SelectedIndex = 4; }
			chkMissUnk1.Checked = _mission.Unknown1;
			chkMissUnk2.Checked = _mission.Unknown2;
			numMissUnk3.Value = _mission.Unknown3;
			numMissUnk4.Value = _mission.Unknown4;
			numMissUnk5.Value = _mission.Unknown5;
			numMissTimeMin.Value = _mission.TimeLimitMin;
			chkEnd.Checked = _mission.EndWhenComplete;
			#endregion
			#region M2
			for (int i=0;i<4;i++)
			{
				_iffs[i+2] = _mission.Iffs[i+2];
				txtIFFs[i].Text = _mission.Iffs[i + 2];
				txtRegions[i].Text = _mission.Regions[i];
			}
			numGlobCargo.Value = 2;
			numGlobCargo.Value = 1;	// force the refresh
			txtNotes.Text = _mission.MissionNotes;
			GGRefresh();
			#endregion
		}

		void cboHangar_Leave(object sender, EventArgs e)
		{
			_mission.MissionType = Common.Update(this, _mission.MissionType, (Mission.HangarEnum)Convert.ToByte(cboHangar.SelectedIndex));
		}
		void cboLogo_Leave(object sender, EventArgs e)
		{
			_mission.Logo = Common.Update(this, _mission.Logo, (Mission.LogoEnum)Convert.ToByte(cboLogo.SelectedIndex + 4));
		}
		void cboOfficer_Leave(object sender, EventArgs e)
		{
			_mission.Officer = Common.Update(this, _mission.Officer, Convert.ToByte(cboOfficer.SelectedIndex));
		}

		void chkEnd_Leave(object sender, EventArgs e)
		{
			_mission.EndWhenComplete = Common.Update(this, _mission.EndWhenComplete, chkEnd.Checked);
		}
		void chkMissUnk1_Leave(object sender, EventArgs e)
		{
			_mission.Unknown1 = Common.Update(this, _mission.Unknown1, chkMissUnk1.Checked);
		}
		void chkMissUnk2_Leave(object sender, EventArgs e)
		{
			_mission.Unknown2 = Common.Update(this, _mission.Unknown2, chkMissUnk2.Checked);
		}

		void numMissTimeMin_Leave(object sender, EventArgs e)
		{
			_mission.TimeLimitMin = Common.Update(this, _mission.TimeLimitMin, Convert.ToByte(numMissTimeMin.Value));
		}
		void numMissUnk3_Leave(object sender, EventArgs e)
		{
			_mission.Unknown3 = Common.Update(this, _mission.Unknown3, Convert.ToByte(numMissUnk3.Value));
		}
		void numMissUnk4_Leave(object sender, EventArgs e)
		{
			_mission.Unknown4 = Common.Update(this, _mission.Unknown4, Convert.ToByte(numMissUnk4.Value));
		}
		void numMissUnk5_Leave(object sender, EventArgs e)
		{
			_mission.Unknown5 = Common.Update(this, _mission.Unknown5, Convert.ToByte(numMissUnk5.Value));
		}

		void txtMissDesc_Leave(object sender, EventArgs e)
		{
			_mission.MissionDescription = Common.Update(this, _mission.MissionDescription, txtMissDesc.Text);
		}
		void txtMissFail_Leave(object sender, EventArgs e)
		{
			_mission.MissionFailed = Common.Update(this, _mission.MissionFailed, txtMissFail.Text);
		}
		void txtMissSucc_Leave(object sender, EventArgs e)
		{
			_mission.MissionSuccessful = Common.Update(this, _mission.MissionSuccessful, txtMissSucc.Text);
		}
		void txtDescNote_Leave(object sender, EventArgs e)
		{
			_mission.DescriptionNotes = Common.Update(this, _mission.DescriptionNotes, txtDescNote.Text);
		}
		void txtSuccNote_Leave(object sender, EventArgs e)
		{
			_mission.SuccessfulNotes = Common.Update(this, _mission.SuccessfulNotes, txtSuccNote.Text);
		}
		void txtFailNote_Leave(object sender, EventArgs e)
		{
			_mission.FailedNotes = Common.Update(this, _mission.FailedNotes, txtFailNote.Text);
		}
		#endregion
		#region Mission2
		void GGRefresh()
		{
			for (int i=0;i<16;i++)
				if (_mission.GlobalGroups[i] != "") lblGG[i].Text = _mission.GlobalGroups[i];
				else lblGG[i].Text = "Global Group " + (i+1).ToString();
		}

		void numGlobCargo_ValueChanged(object sender, EventArgs e)
		{
			int i = (int)numGlobCargo.Value - 1;
			txtGlobCargo.Text = _mission.GlobalCargo[i].Cargo;
			chkGCUnk1.Checked = _mission.GlobalCargo[i].Unknown1;
			numGCUnk2.Value = _mission.GlobalCargo[i].Unknown2;
			numGCUnk3.Value = _mission.GlobalCargo[i].Unknown3;
			numGCUnk4.Value = _mission.GlobalCargo[i].Unknown4;
			numGCUnk5.Value = _mission.GlobalCargo[i].Unknown5;
		}

		void lblGGArr_Click(object sender, EventArgs e)
		{
			Label l;
			int i;
			try
			{
				l = (Label)sender;
				l.Focus();
				i = (int)l.Tag;
			}
			catch (InvalidCastException)
			{
				i = (int)sender;
				l = lblGG[i];
			}
			txtGlobGroup.Text = _mission.GlobalGroups[i];
			l.ForeColor = SystemColors.Highlight;
			for (i=0;i<16;i++) if (lblGG[i] != l) lblGG[i].ForeColor = SystemColors.ControlText;
		}
		void txtIFFsArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.Iffs[(int)t.Tag] = Common.Update(this, _mission.Iffs[(int)t.Tag], t.Text);
			_iffs[(int)t.Tag] = t.Text;
			comboReset(cboIFF, _iffs, cboIFF.SelectedIndex);
		}
		void txtRegionsArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.Regions[(int)t.Tag] = Common.Update(this, _mission.Regions[(int)t.Tag], t.Text);
		}

		void chkGCUnk1_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			_mission.GlobalCargo[gc].Unknown1 = Common.Update(this, _mission.GlobalCargo[gc].Unknown1, chkGCUnk1.Checked);
		}

		void numGCUnk2_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			_mission.GlobalCargo[gc].Unknown2 = Common.Update(this, _mission.GlobalCargo[gc].Unknown2, Convert.ToByte(numGCUnk2.Value));
		}
		void numGCUnk3_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			_mission.GlobalCargo[gc].Unknown3 = Common.Update(this, _mission.GlobalCargo[gc].Unknown3, Convert.ToByte(numGCUnk3.Value));
		}
		void numGCUnk4_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			_mission.GlobalCargo[gc].Unknown4 = Common.Update(this, _mission.GlobalCargo[gc].Unknown4, Convert.ToByte(numGCUnk4.Value));
		}
		void numGCUnk5_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			_mission.GlobalCargo[gc].Unknown5 = Common.Update(this, _mission.GlobalCargo[gc].Unknown5, Convert.ToByte(numGCUnk5.Value));
		}

		void txtGlobCargo_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			_mission.GlobalCargo[gc].Cargo = Common.Update(this, _mission.GlobalCargo[gc].Cargo, txtGlobCargo.Text);
		}
		void txtGlobGroup_Leave(object sender, EventArgs e)
		{
			int i;
			for (i=0;i<16;i++) if (lblGG[i].ForeColor == SystemColors.Highlight) break;
			_mission.GlobalGroups[i] = Common.Update(this, _mission.GlobalGroups[i], txtGlobGroup.Text);
			GGRefresh();
		}
		void txtNotes_Leave(object sender, EventArgs e)
		{
			_mission.MissionNotes = Common.Update(this, _mission.MissionNotes, txtNotes.Text);
		}
		#endregion
	}
}