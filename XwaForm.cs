/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2023 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.13.12+
 */

/* CHANGELOG
 * [UPD] Moved around the Para2 and Para1 trigger controls so Para1 is naturally closer to the Target control
 * [UPD] Region names now appear in trigger text
 * v1.13.12, 230116
 * [NEW] RememberSelectedOrder option functionality
 * [FIX] Order Region selection resets on Open/New
 * v1.13.11, 221030
 * [FIX] Open dialog not following current directory after switching paltforms via "Open Recent"
 * v1.13.10, 221018
 * [NEW] ability to shade FG list by Region
 * v1.13.7, 220730
 * [FIX] crash when copying a WP value via Ctrl+C and no text is selected
 * [FIX] OrderWPs being linked after a multi-select paste
 * v1.13.6, 220619
 * [UPD] Confirm save now only asks if modified
 * v1.13.4, 220606
 * [NEW] FG Goal auto-toggles when setting the Trigger
 * [UPD] OneIndexedFGs implementation
 * v1.13.3, 220402
 * [FIX] ComboBox stutter for colorized cbo's due to OwnerDrawVariable
 * v1.13.2, 220319
 * [FIX] Remove focus during save to trigger any Leave events
 * v1.13.1, 220208
 * [UPD] Order Waypoint cbo replaced with a pair of numerics
 * [UPD] SP3 waypoint Text changed to RDV (Designer)
 * [NEW] menuCut [JB]
 * [FIX] multi-select refresh issues [JB]
 * [FIX] craftStart issues during Paste and arrival changes [JB]
 * [FIX] cbo loading during start/init [JB]
 * v1.13, 220130
 * [NEW] copy/paste for Order Waypoints
 * [NEW] Multi-select [JB]
 * v1.12, 220103
 * [FIX] Combos cleared during init to prevent duplicated entries [JB]
 * [UPD] Unused messages drawn in gray
 * [FIX] Listbox scrolling
 * v1.11.2, 2101005
 * [FIX] Pasting a message when at capacity now correctly does nothing
 * [UPD] Copy/paste now uses system clipboard, can more easily paste external text
 * [NEW] Copy/paste now works for Waypoints, can paste XvT/XWA Triggers/Orders
 * v1.11, 210801
 * [NEW] Hyper to Region order text shows number and name if defined [JB]
 * [FIX] ComboBoxes when using CraftType [JB]
 * [FIX] Exception on GlobalCargo during save [JB]
 * [NEW #46] Backdrop color from dialog copied for use
 * v1.10.2, 210606
 * [FIX] SBD region detection
 * v1.10.1, 210606
 * [FIX] Backdrop shadow index. Currently a locak hack, should really be fixed in Platform
 * v1.10, 210520
 * [NEW] Added explicit PlayerNumber check during save
 * [NEW] Mission Craft List menu item to display pre-briefing craft list
 * [UPD #58] Map refreshes after adjusting craft orientation [JB]
 * [UPD #56] Replaced try/catch with TryParse [JB]
 * v1.9.2, 210328
 * [FIX] Test load failure if mission isn't in platform directory
 * v1.9.1, 210130
 * [FIX] Region name refresh [JB]
 * [FIX] Region colors in parameter list [JB]
 * v1.9, 210108
 * [NEW] Wave Dialog menu and toolbar item
 * [FIX] Clipboard path
 * v1.8.2, 201219
 * [FIX] SBD default backdrop value
 * v1.8.1, 201213
 * [UPD] GlobalCargo cbo now says Shadow with the correct quantity
 * [UPD] _config passed to Hook dialog, LST form, Backdrops. RunVerify()
 * [UPD #20] Test function now attempts to detect platform from MissionPath
 * [UPD] menuTest moved under Tools, changed to &Test
 * v1.8, 201004
 * [FIX] Deactivate added to force focus fix [JB]
 * [FIX] Waypoint refresh maintains selection [JB]
 * [UPD] newFG now returns bool
 * [FIX] Special Cargo text box not showing when switching craft
 * [FIX] menuTest was really named menuText...
 * [FIX] Test launching if you cancel the intial Save
 * [UPD] saveMission now won't save/rewrite file if unmodifed
 * [NEW] FlightGroupLibrary [JB]
 * [UPD] Goal/Order/Trigger string substitutions now use GG strings for GGs 0-15 [JB]
 * [UPD] colorized cbos now work with "not FG" [JB]
 * [UPD] tweaked how colorized cbos draw [JB]
 * [FIX] SafeSetCbos added to more places [JB]
 * [FIX] refreshSkipIndicators maintains selection [JB]
 * v1.7, 200816
 * [FIX] regions in Parameter cbos drawing black-on-black
 * [FIX] crash when leaving the GG name without a selection
 * [FIX] recalculateEditorCraftNumbering() handles _activeFG now [JB]
 * [UPD] shiplist and Map calls updated for Wireframe implementation [JB]
 * [UPD] Blank messages now shown as "*" [JB]
 * [UPD] Cleanup index substitions [JB]
 * [UPD] Trigger label refresh updates [JB]
 * [FIX] Extra protections to handle custom missions using "bad" Status or Formation values [JB]
 * [UPD] Added numbers to Messages to make use in Triggers easier [JB]
 * [FIX] Craft TeamRoles reduced to 8 from 10 [JB]
 * [NEW] IFF substitutions
 * [UPD] form handlers renamed
 * [FIX] crash if selecting RecentMission of a different platform
 * v1.6.6, 200719
 * [FIX] Crash when using "Apply DTM SuperBackdrops to new missions" option
 * v1.6.5, 200704
 * [UPD] More details to ProcessCraftList error message
 * [FIX #32] bin path now explicitly uses Startup Path to prevent implicit from defaulting to sys32
 * v1.6.4, 200119
 * [NEW #30] Briefing callback
 * v1.6, 190915
 * [UPD] cmdBackdrop "Loading..." now always shown instead of just with SBD
 * [NEW] hook implementation
 * v1.5.1, 190513
 * [NEW] Changing GG or GU value will now prompt to update references throughout if it's the only FG with that designation
 * v1.5, 180910
 * [NEW] Dictionaries for Control handling [JB]
 * [UPD] blank Team names now show generic in cbo's [JB]
 * [UPD] Messages show preview in cbo's [JB]
 * [FIX] comboReset now has index check [JB]
 * [UPD] improved TargetText [JB]
 * [FIX] exception if no messages [JB]
 * [NEW] Xwing support [JB]
 * [FIX] non-XWA filestreams now closed properly [JB]
 * [UPD] order speed changed to cbo [JB]
 * [NEW] controls to apply roles to teams individually [JB]
 * [NEW] Energy beam, Cluster CM [JB]
 * [UPD] teams refreshed on startup [JB]
 * [UPD] lots of controls converted to instant-update instead of using _Leave. Done by pointing handlers to _Leave instead of redoing it [JB]
 * [NEW] colorized cbo's [JB]
 * [NEW] 'Enter' key trigger control update [JB]
 * [UPD] general performance improvements [JB]
 * [UPD] copy/paste expanded [JB]
 * [FIX] mark mission changed after HyperBuoy dialog [JB]
 * [FIX] start new mission if Recent fails load [JB]
 * [NEW] cmdMoveMessUp/Down and cmdMoveFGUp/Down [JB]
 * [UPD] lbl colors now applied via function to allow themeing [JB]
 * [UPD] delete FG reworked [JB]
 * [NEW] editor-only craft numbering [JB]
 * [UPD] better tab updates [JB]
 * [UPD] Pilot control changed to cbo [JB]
 * [NEW] Mission Craft designation button [JB]
 * [UPD] improved order information [JB]
 * [NEW] blank messages are shown [JB]
 * [NEW] Goal time limit [JB]
 * [UPD] better tab updates [JB]
 * [UPD] MessDelaySec changed to MessDelay, MessDelayMin changed to MessUnk2, MessUnk2 changed to MessUnk3 [JB]
 * [UPD #21] Allow craft markings beyond original 4
 * v1.4.3, 180509
 * [NEW] Prox trigger distances
 * [UPD] changed Trigger cbo's from Leave to SIChanged for Amount/Distance Prox handling
 * [NEW #18] label for Escort Position
 * [UPD] changed how Strings.OrderDesc gets split
 * [UPD] added MGLT multiplier for orders
 * [NEW #19] TriggerType unknowns (via JeremyAnsel)
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
using System.Collections.Generic;
using Idmr.Platform.Xwa;

namespace Idmr.Yogeme
{
	/// <summary>XWA Mission Editor GUI</summary>
	public partial class XwaForm : Form
	{
		#region vars and stuff
		readonly Settings _config;
		bool _loading;
		bool _noRefresh = false;
		MapForm _fMap;
		BriefingForm _fBrief;
		LstForm _fLST;
		FlightGroupLibraryForm _fLibrary;
		Mission _mission;
		XwaWavForm _fWav;
		bool _applicationExit;
		int _activeFG = 0;
		int _startingShips = 1;
		int _activeMessage = 0;
		readonly DataTable _tableWP = new DataTable("Waypoints");
		readonly DataTable _tableWPRaw = new DataTable("Waypoints_Raw");
		readonly DataTable _tableOrder = new DataTable("Orders");
		readonly DataTable _tableOrderRaw = new DataTable("Orders_Raw");
		byte _activeMessageTrigger = 0;
		byte _activeGlobalTrigger = 0;
		byte _activeTeam = 0;
		byte _activeArrDepTrigger = 0;
		byte _activeFGGoal = 0;
		byte _activeOrder = 0;
		byte _activeOptionCraft = 0;
		byte _activeSkipTrigger = 0;
		bool _hookBackdropInstalled;
		#endregion
		#region control arrays
#pragma warning disable IDE1006 // Naming Styles
		readonly MenuItem[] menuRecentMissions = new MenuItem[6];
		// FG AD
		readonly Label[] lblADTrig = new Label[6];
		readonly RadioButton[] optADAndOr = new RadioButton[8];
		// FG Goals
		readonly Label[] lblGoal = new Label[8];
		// FG WP
		readonly CheckBox[] chkWP = new CheckBox[22];
		// FG Order
		readonly Label[] lblOrder = new Label[4];
		// FG options
		readonly Label[] lblOptCraft = new Label[10];
		readonly CheckBox[] chkOpt = new CheckBox[18];
		// Mess
		readonly Label[] lblMessTrig = new Label[6];
		readonly RadioButton[] optMessAndOr = new RadioButton[8];
		readonly CheckBox[] chkSendTo = new CheckBox[10];
		// Glob
		readonly Label[] lblGlobTrig = new Label[12];
		readonly RadioButton[] optGlobAndOr = new RadioButton[18];
		// Team
		readonly Label[] lblTeam = new Label[10];
		readonly RadioButton[] optAllies = new RadioButton[30];
		readonly TextBox[] txtEoM = new TextBox[6];
		readonly NumericUpDown[] numTeamUnk = new NumericUpDown[6];
		// Miss2
		readonly Label[] lblGG = new Label[16];
		readonly TextBox[] txtIFFs = new TextBox[4];
		readonly TextBox[] txtRegions = new TextBox[4];
		readonly Dictionary<ComboBox, ComboBox> colorizedFGList = new Dictionary<ComboBox, ComboBox>();  //[JB] Maps a control that should have a colorized FG list with a control that determines whether the list actually contains a FG list.
#pragma warning restore IDE1006 // Naming Styles
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

		#region methods
		void closeForms()
		{
			try { _fMap.Close(); }
			catch { /* do nothing */ }
			try { _fBrief.Close(); }
			catch { /* do nothing */ }
			try { _fLST.Close(); }
			catch { /* do nothing */ }
			try { _fLibrary.Close(); }
			catch { /* do nothing */ }
			try { _fWav.Close();  }
			catch { /* do nothing */ }
		}
		void comboVarRefresh(int index, ComboBox cbo)
		{
			if (index == -1) return;
			cbo.Items.Clear();
			string[] temp;
			switch (index)      //switch (VariableType)
			{
				case 0:
					cbo.Items.Add("None");
					break;
				case 1: // Flight Group
					cbo.Items.AddRange(_mission.FlightGroups.GetList());
					break;
				case 2: // Ship Type
					cbo.Items.AddRange(Strings.CraftType);
					break;
				case 3: // Ship Class
					cbo.Items.AddRange(Strings.ShipClass);
					break;
				case 4: // Object Type
					cbo.Items.AddRange(Strings.ObjectType);
					break;
				case 5: // IFF
					cbo.Items.AddRange(getIffStrings());
					break;
				case 6: // Ship Orders
					cbo.Items.AddRange(Strings.Orders);
					break;
				case 7: // Craft when
					cbo.Items.AddRange(Strings.CraftWhen);
					break;
				case 8:  //Global Group
				case 20: //All Global Groups except
					temp = new string[256];
					for(int i = 0; i < 256; i++)
						temp[i] = ((i < 16 && _mission.GlobalGroups[i] != "") ? i.ToString() + ": " + _mission.GlobalGroups[i] : i.ToString());
					cbo.Items.AddRange(temp);
					break;
				case 9: // Rating
					cbo.Items.AddRange(Strings.Rating);
					break;
				case 10: // Craft with status
					cbo.Items.AddRange(Strings.Status);
					break;
				//case 11: // All
				case 12: // Team
					temp = _mission.Teams.GetList();
					for (int i = 0; i < temp.Length; i++)
						if (temp[i] == "") temp[i] = "Team " + (i + 1).ToString();  //[JB] Modified to replace empty strings.
					cbo.Items.AddRange(temp);
					break;
				//case 13: Player
				case 14: // After delay
					temp = new string[256];
					for (int i = 0; i < 256; i++) temp[i] = Common.GetFormattedTime(i * 5, false);
					cbo.Items.AddRange(temp);
					break;
				case 15: // All Flight Groups except
					cbo.Items.AddRange(_mission.FlightGroups.GetList());
					break;
				case 16: // All ship types except
					cbo.Items.AddRange(Strings.CraftType);
					break;
				case 17: // All ship classes except
					cbo.Items.AddRange(Strings.ShipClass);
					break;
				case 18: // All object types except
					cbo.Items.AddRange(Strings.ObjectType);
					break;
				case 19: // All IFFs except
					cbo.Items.AddRange(getIffStrings());
					break;
				//case 20: // All Global Groups except (handled above with Global Group)
				case 21: // All Teams except
					temp = _mission.Teams.GetList();
					for (int i = 0; i < temp.Length; i++)
						if (temp[i] == "") temp[i] = "Team " + (i + 1).ToString();  //[JB] Modified to replace empty strings.
					cbo.Items.AddRange(temp);
					break;
				//case 22: // All players except
				//case 23: // Global Unit
				//case 24: // All Global Units except
				//case 25: // Global Cargo
				//case 26: // All Global Cargos except
				case 27: //Message #
					temp = new string[256];  //[JB] Modified to display a one-based list of numbers, with message previews.
					for (int i = 0; i < 64; i++)
					{
						temp[i] = "#" + (i + 1).ToString();
						string s = getMessagePreview(i);
						if (s != "")
							temp[i] += "  '" + s + "'";
					}
					for (int i = 64; i < 256; i++) temp[i] = i.ToString();
					cbo.Items.AddRange(temp);
					break;
				default:
					temp = new string[256];
					for (int i = 0; i < 256; i++) temp[i] = i.ToString();
					cbo.Items.AddRange(temp);
					break;
			}
		}
		void comboReset(ComboBox cbo, string[] items, int index)
		{
			if (index < -1 || index >= items.Length) index = -1;  //[JB] Fixes rare out of bounds issues when FGs deleted or reset.
			cbo.Items.Clear();
			cbo.Items.AddRange(items);
			cbo.SelectedIndex = index;
		}
		void comboProxCheck(int trigger, ComboBox cbo)
		{
			bool btemp = _loading;
			_loading = true;
			if ((trigger == 0x31 || trigger == 0x32) && cbo.Items.Count < 30)
			{
				int dist = cbo.SelectedIndex;
				cbo.Items.Clear();
				cbo.Items.Add("0.05 km");
				for (int i = 1; i < 10; i++) cbo.Items.Add(0.1 * i + " km");
				for (int i = 10; i <= 50; i++) cbo.Items.Add(0.5 * i - 4 + " km");
				cbo.SelectedIndex = dist;
			}
			else if (trigger != 0x31 && trigger != 0x32 && cbo.Items.Count > 30)
			{
				int amount = cbo.SelectedIndex;
				cbo.Items.Clear();
				cbo.Items.AddRange(Strings.Amount);
				try { cbo.SelectedIndex = amount; }
				catch { cbo.SelectedIndex = 0; }
			}
			// else cbo is fine as-is
			_loading = btemp;
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
			tabMain.Focus();            // Exit focus from any form controls.  Fixes some issues that might arise from Leave() events trying to access modified lists.
			lstFG.Items.Clear();        // Clearing FGs here prevents issues with ComboBoxes further down.
			lstMessages.Items.Clear();  // Clearing messages to reset the move buttons.
			Common.UpdateMoveButtons(cmdMoveMessUp, cmdMoveMessDown, lstMessages);
			_mission = new Mission();
			_config.LastMission = "";
			_activeFG = 0;
			_activeMessage = 0;
			_mission.FlightGroups[0].CraftType = Convert.ToByte(_config.XwaCraft);
			_mission.FlightGroups[0].IFF = Convert.ToByte(_config.XwaIff);
			_startingShips = 0;
			craftStart(_mission.FlightGroups[0], true);
			lstFG.Items.Add(_mission.FlightGroups[0].ToString(true));
			#region required early control arrays
			// these are extracted from startup() so SBD on init works
			cboADTrig.Items.Clear();
			cboADTrig.Items.AddRange(Strings.Trigger);
			cboADTrigAmount.Items.Clear();
			cboADTrigAmount.Items.AddRange(Strings.Amount);
			cboADTrigType.Items.Clear();
			cboADTrigType.Items.AddRange(Strings.VariableType);
			cboAbort.Items.Clear();
			cboAbort.Items.AddRange(Strings.Abort); cboAbort.SelectedIndex = 0;
			cboDiff.Items.Clear();
			cboDiff.Items.AddRange(Platform.BaseStrings.Difficulty); cboDiff.SelectedIndex = 0;
			lblADTrig[0] = lblArr1;
			lblADTrig[1] = lblArr2;
			lblADTrig[2] = lblArr3;
			lblADTrig[3] = lblArr4;
			lblADTrig[4] = lblDep1;
			lblADTrig[5] = lblDep2;
			for (int i = 0; i < 6; i++)
			{
				lblADTrig[i].Click += new EventHandler(lblADTrigArr_Click);
				lblADTrig[i].MouseUp += new MouseEventHandler(lblADTrigArr_MouseUp);
				lblADTrig[i].DoubleClick += new EventHandler(lblADTrigArr_DoubleClick);
				lblADTrig[i].Tag = i;
			}
			lblOrder[0] = lblOrder1;
			lblOrder[1] = lblOrder2;
			lblOrder[2] = lblOrder3;
			lblOrder[3] = lblOrder4;
			for (int i = 0; i < 4; i++)
			{
				lblOrder[i].Click += new EventHandler(lblOrderArr_Click);
				lblOrder[i].DoubleClick += new EventHandler(lblOrderArr_DoubleClick);
				lblOrder[i].MouseUp += new MouseEventHandler(lblOrderArr_MouseUp);
				lblOrder[i].Tag = i;
			}
			if (cboOrders.Items.Count == 0) cboOrders.Items.AddRange(Strings.Orders);
			if (cboOT1Type.Items.Count == 0) cboOT1Type.Items.AddRange(Strings.VariableType);
			if (cboOT2Type.Items.Count == 0) cboOT2Type.Items.AddRange(Strings.VariableType);
			if (cboOT3Type.Items.Count == 0) cboOT3Type.Items.AddRange(Strings.VariableType);
			if (cboOT4Type.Items.Count == 0) cboOT4Type.Items.AddRange(Strings.VariableType);
			if (cboOSpeed.Items.Count == 1)
				for (int i = 1; i < 256; i++)  //The designer already starts with one item "default" for 0 MGLT.
				cboOSpeed.Items.Add(Convert.ToInt32(i * 2.2235));
			cboOSpeed.SelectedIndex = 0;
			cboSkipAmount.Items.AddRange(Strings.Amount);
			cboSkipTrig.Items.Clear();
			cboSkipTrig.Items.AddRange(Strings.Trigger);
			cboSkipType.Items.Clear();
			cboSkipType.Items.AddRange(Strings.VariableType);
			cboGlobalAmount.Items.AddRange(Strings.Amount);
			cboGlobalTrig.Items.Clear();
			cboGlobalTrig.Items.AddRange(Strings.Trigger);
			cboGlobalType.Items.Clear();
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
			for (int i = 0; i < 12; i++)
			{
				lblGlobTrig[i].Click += new EventHandler(lblGlobTrigArr_Click);
				lblGlobTrig[i].DoubleClick += new EventHandler(lblGlobTrigArr_DoubleClick);
				lblGlobTrig[i].MouseUp += new MouseEventHandler(lblGlobTrigArr_MouseUp);
				lblGlobTrig[i].Tag = i;
			}
			lblMessTrig[0] = lblMess1;
			lblMessTrig[1] = lblMess2;
			lblMessTrig[2] = lblMess3;
			lblMessTrig[3] = lblMess4;
			lblMessTrig[4] = lblMess5;
			lblMessTrig[5] = lblMess6;
			for (int i = 0; i < 6; i++)
			{
				lblMessTrig[i].Click += new EventHandler(lblMessTrigArr_Click);
				lblMessTrig[i].DoubleClick += new EventHandler(lblMessTrigArr_DoubleClick);
				lblMessTrig[i].MouseUp += new MouseEventHandler(lblMessTrigArr_MouseUp);
				lblMessTrig[i].Tag = i;
			}
			#endregion
			if (_config.SuperBackdropsInstalled && _config.InitializeUsingSuperBackdrops) menuSuperBackdrops_Click("initializeMission()", new EventArgs());
			string[] fgList = _mission.FlightGroups.GetList();
			comboReset(cboArrMS, fgList, 0);
			comboReset(cboArrMSAlt, fgList, 0);
			comboReset(cboDepMS, fgList, 0);
			comboReset(cboDepMSAlt, fgList, 0);
			numORegion.Value = 1;
			comboReset(cboTeam, _mission.Teams.GetList(), _mission.FlightGroups[0].Team);
			cboGlobalTeam.Items.Clear();
			cboGlobalTeam.Items.AddRange(_mission.Teams.GetList());
			Text = "Ye Olde Galactic Empire Mission Editor - XWA - New Mission";
			cboMessFG.Items.Clear();
			cboMessFG.Items.AddRange(fgList);
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			if (!_config.XwaInstalled) cmdBackdrop.Enabled = false;
			_applicationExit = true;    //becomes false if selecting "New Mission" from menu
		}
		void loadCraftData(string fileMission)
		{
			Strings.OverrideShipList(null, null); //Restore defaults.
			try
			{
				CraftDataManager.GetInstance().LoadPlatform(Settings.Platform.XWA, _config, Strings.CraftType, Strings.CraftAbbrv, fileMission);
				Strings.OverrideShipList(CraftDataManager.GetInstance().GetLongNames(), CraftDataManager.GetInstance().GetShortNames());
			}
			catch (Exception x) { MessageBox.Show("Error processing custom XWA ship list, using defaults.\n\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			cboCraft.Items.Clear();
			cboCraft.Items.AddRange(Strings.CraftType);
		}
		bool isMissionCraft(Mission.Trigger trigger)
		{
			return (trigger.Amount == 0 && trigger.VariableType == 1 && trigger.Variable == 0 && trigger.Condition == 0);
		}
		string getMissionCraftString(int fgIndex)
		{
			FlightGroup playerFG = null;
			foreach (FlightGroup fg in _mission.FlightGroups)
			{
				if (fg.PlayerNumber > 0)
				{
					playerFG = fg;
					break;
				}
			}

			var craft = _mission.FlightGroups[fgIndex];
			if (playerFG == null || craft.Team != playerFG.Team)
				return string.Empty;

			var trig1 = craft.ArrDepTriggers[0];
			var trig2 = craft.ArrDepTriggers[1];
			if (trig1.Amount == 0 && trig1.VariableType == 1 && trig1.Variable == 0 && trig1.Condition == 0 &&
				trig2.Amount == 0 && trig2.VariableType == 1 && trig2.Variable == 0 && trig2.Condition == 0)
			{
				if (craft.CraftType >= 0x1A && craft.CraftType <= 0x1D) return string.Empty;   // CN/A - CN/D
				if (craft.CraftType >= 0x37 && craft.CraftType <= 0x5B) return string.Empty;   // CN/E - REPYD
				if (craft.CraftType >= 0x81 && craft.CraftType <= 0x95) return string.Empty;   // Golan I - Pirate Junkyard
				if (craft.CraftType >= 0x97 && craft.CraftType <= 0xAD) return string.Empty;   // Pressure Tank - Cargo Tanker 5
				if (craft.CraftType >= 0xAF && craft.CraftType <= 0xE3) return string.Empty;   // Rebel Pilot - DS II
				if (craft.CraftType >= 0xE7) return string.Empty;  // Backdrops

				// else we're good to proceed
				string output;
				bool isStarship = false;
				if (craft.CraftType >= 0x28 && craft.CraftType <= 0x36) isStarship = true;   // CRV - SSD
				else if (craft.CraftType >= 0x5C && craft.CraftType <= 0x60) isStarship = true;   // M/SC - Gunship
				else if (craft.CraftType == 0x63 || craft.CraftType == 0x65 || craft.CraftType == 0x66) isStarship = true;  // Marauder, Imperial Research Ship, LY3000
				else if (craft.CraftType >= 0xE4) isStarship = true;    // MC80

				if (isStarship)
					output = Strings.CraftType[craft.CraftType] + " " + craft.Name;
				else
					output = (craft.NumberOfCraft * craft.NumberOfWaves) + " " + Strings.CraftType[craft.CraftType];

				return output;
			}
			return string.Empty;
		}
		Brush getFlightGroupDrawColor(int fgIndex)
		{
			Brush brText = SystemBrushes.ControlText;
			if (fgIndex < 0 || fgIndex >= _mission.FlightGroups.Count) return brText;
			switch (_mission.FlightGroups[fgIndex].IFF)
			{
				case 0:
					brText = Brushes.Lime; //LimeGreen;
					break;
				case 1:
					brText = Brushes.Red; //Crimson;
					break;
				case 2:
					brText = Brushes.DodgerBlue; //RoyalBlue;
					break;
				case 3:
					brText = Brushes.Yellow;
					break;
				case 4:
					brText = Brushes.OrangeRed; //Red;
					break;
				case 5:
					brText = Brushes.MediumOrchid; //DarkOrchid;
					break;
			}
			if (_mission.FlightGroups[fgIndex].Difficulty == 6) //[JB] For craft difficulty that never arrives.
				brText = Brushes.Gray;
			return brText;
		}
		Color getHighlightColor() { return _config.ColorInteractSelected; }
		/// <summary>Generates a string list of IFF names which provide default names instead of an empty string when no custom names are defined</summary>
		string[] getIffStrings()
		{
			string[] t = new string[_mission.IFFs.Length];
			for (int i = 0; i < t.Length; i++)
			{
				t[i] = _mission.IFFs[i];
				if (t[i] == "")
					t[i] = Strings.IFF[i];
			}
			return t;
		}
		string getMessagePreview(int index)
		{
			if (index < 0 || index >= _mission.Messages.Count)
				return "";

			string msg = _mission.Messages[index].MessageString;
			bool overLen = false;
			if (msg.Length > 12)
			{
				overLen = true;
				int pos = msg.IndexOf(' ', 12);
				if (pos >= 0)
					msg = msg.Substring(0, pos);
			}
			return msg + (overLen ? " ..." : "");
		}
		bool hasFocus(Label[] list) //[JB] Added helper function to detect focus inside control arrays when copying/pasting triggers.
		{
			foreach (Label c in list)
				if (c.Focused)
					return true;
			return false;
		}
		void labelRefresh(Mission.Trigger trigger, Label lbl)
		{
			string triggerText = trigger.ToString();
			triggerText = replaceTargetText(triggerText);

			if (trigger.VariableType == 27) //[JB] For messages, display a preview.
			{
				string p = getMessagePreview(trigger.Variable);
				if (p != "")
					triggerText += " '" + p + "'";
			}

			//[JB] Display whether it *might* appear in the Mission Craft list before the briefing.
			if (lbl == lblADTrig[0] || lbl == lblADTrig[1])
			{
				if (isMissionCraft(trigger))
					triggerText += " (In Mission Craft List)";
			}
			lbl.Text = triggerText;

		}
		bool loadMission(string fileMission)
		{
			closeForms();
			lstFG.Items.Clear();
			lstMessages.Items.Clear();
			_activeFG = 0;
			_activeMessage = 0;
			_startingShips = 0;
			try
			{
				FileStream fs = File.OpenRead(fileMission);
				try
				{
					#region determine platform
					switch (Platform.MissionFile.GetPlatform(fs))
					{
						case Platform.MissionFile.Platform.Xwing:  //[JB] Added support for Xwing
							_applicationExit = false;
							new XwingForm(_config, fileMission).Show();
							Close();
							fs.Close(); //[JB] Files were being left open, which could cause access violations.  Need to close stream before returning.
							return false;
						case Platform.MissionFile.Platform.TIE:
							_applicationExit = false;
							new TieForm(_config, fileMission).Show();
							Close();
							fs.Close();
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
					throw x;
				}
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				menuNewXWA_Click(0, new EventArgs());   // moved fix from RecentMissions_Click otherwise it would crash if the Recent was a different platform
														// Now, if Recent is a different platform this does result in a new mission initilization before the form closes
				return false;
			}
			loadCraftData(fileMission);
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
				if (_mission.FlightGroups[i].ArrivesIn30Seconds) craftStart(_mission.FlightGroups[i], true);
			}
			updateFGList();
			numORegion.Value = 1;
			if (_mission.Messages.Count == 0) enableMessages(false);
			else
			{
				enableMessages(true);
				for (int i = 0; i < _mission.Messages.Count; i++)
					lstMessages.Items.Add(getNumberedMessage(i));
			}
			bool btemp = _loading;
			_loading = true;
			updateMissionTabs();
			cboGlobalTeam.SelectedIndex = -1;   // otherwise it doesn't trigger an index change
			cboGlobalTeam.SelectedIndex = 0;
			for (_activeTeam = 0; _activeTeam < 10; _activeTeam++) teamRefresh();
			_activeTeam = 0;
			lblTeamArr_Click(lblTeam[0], new EventArgs());
			_loading = btemp;
			int c = fileMission.LastIndexOf("\\") + 1;
			Text = "Ye Olde Galactic Empire Mission Editor - XWA - " + fileMission.Substring(c);
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
			Common.SafeSetCBO(cbo, index, true);  //[JB] Set index and allow expansion in the case of undefined/unknown values.
												  //cbo.SelectedIndex = index;
		}
		void promptSave()
		{
			_config.SaveSettings();
			if (_config.ConfirmSave && (Text.IndexOf("*") != -1))
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
		void registerColorizedFGList(ComboBox variable, ComboBox variableType)
		{
			if (!_config.ColorizedDropDowns) return;
			if (variable == null)
				return;
			colorizedFGList[variable] = variableType;
			variable.DrawMode = DrawMode.OwnerDrawFixed;
			variable.DrawItem += colorizedComboBox_DrawItem;
		}
		string replaceTargetText(string text)
		{
			while (text.Contains("FG:"))
			{
				int fg = Common.ParseIntAfter(text, "FG:");
				text = text.Replace("FG:" + fg, ((fg >= 0 && fg < _mission.FlightGroups.Count) ? _mission.FlightGroups[fg].ToString() : "Undefined"));
			}
			while (text.Contains("FG2:"))
			{
				int fg = Common.ParseIntAfter(text, "FG2:");
				text = text.Replace("FG2:" + fg, ((fg >= 0 && fg < cboADPara.Items.Count) ? cboADPara.Items[fg].ToString() : "Undefined")); // this could be any Para, but they should all be the same anyway
			}
			while (text.Contains("IFF:"))
			{
				int iff = Common.ParseIntAfter(text, "IFF:");
				text = text.Replace("IFF:" + iff, "IFF " + Common.SafeString(getIffStrings(), iff, true));
			}
			while (text.Contains("TM:"))
			{
				int team = Common.ParseIntAfter(text, "TM:");
				text = text.Replace("TM:" + team, ((team >= 0 && team < 10 && _mission.Teams[team].Name != "") ? _mission.Teams[team].Name : "Team " + (team + 1).ToString()));
			}
			while (text.Contains("GG:"))
			{
				int gg = Common.ParseIntAfter(text, "GG:");
				text = text.Replace("GG:" + gg, ((gg >= 0 && gg < 16 && _mission.GlobalGroups[gg] != "") ? "GG " + gg + ": " + _mission.GlobalGroups[gg] : "Global Group " + gg));
			}
			while (text.Contains("REG:"))
			{
				int reg = Common.ParseIntAfter(text, "REG:");
				string regName = "#" + reg;
				if (reg > 0 && reg <= 4 && !_mission.Regions[reg].ToUpper().StartsWith("REGION"))
					regName += " (" +_mission.Regions[reg - 1] + ")";
				text = text.Replace("REG:" + reg, regName);
			}
			return text;
		}
		void saveMission(string fileMission)
		{
			tabMain.Focus();
			try { _fBrief.Save(); }
			catch { /* do nothing */ }
			lblTeamArr_Click(lblTeam[_activeTeam], new EventArgs());    // forces an update
			try { _mission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			Text = "Ye Olde Galactic Empire Mission Editor - XWA - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
			refreshRecent();  //[JB] Setting _config.LastMission modifies the Recent list.  Need to refresh the menu to match.
							  //Verify the mission after it's been saved
			if (_config.Verify) Common.RunVerify(_mission.MissionPath, _config);
			else
			{
				// Checking this since this is rather critical.
				bool missingPlayer = true;
				for (int f = 0; f < _mission.FlightGroups.Count; f++)
					if (_mission.FlightGroups[f].PlayerNumber > 0)
					{
						missingPlayer = false;
						break;
					}
				if (missingPlayer) MessageBox.Show("Mission is missing Player Craft! It *WILL* crash at the briefing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void setInteractiveLabelColor(Label control, bool highlight)
		{
			control.ForeColor = highlight ? _config.ColorInteractSelected : _config.ColorInteractNonSelected;
			control.BackColor = _config.ColorInteractBackground;
		}
		void startup()
		{
			loadCraftData("");
			_applicationExit = true;    //becomes false if selecting "New Mission" from menu
			_config.LastMission = "";
			_config.LastPlatform = Settings.Platform.XWA;
			opnXWA.InitialDirectory = _config.GetWorkingPath(); //[JB] Updated for MRU access.  Defaults to installation and mission folder if not enabled.
			savXWA.InitialDirectory = _config.GetWorkingPath();
			#region Menu
			menuTest.Enabled = _config.XwaInstalled;
			if (_config.RestrictPlatforms)
			{
				if (!_config.XwingInstalled) { menuNewXwing.Enabled = false; }
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
			cboCraft.SelectedIndex = _mission.FlightGroups[0].CraftType; // already loaded in loadCraftData
			cboIFF.Items.AddRange(Strings.IFF); cboIFF.SelectedIndex = _mission.FlightGroups[0].IFF;
			cboAI.Items.AddRange(Strings.Rating); cboAI.SelectedIndex = _mission.FlightGroups[0].AI;
			cboMarkings.Items.AddRange(Strings.Color);
			for (int i = cboMarkings.Items.Count; i < 256; i++) cboMarkings.Items.Add("Color #" + (i + 1));
			cboMarkings.SelectedIndex = _mission.FlightGroups[0].Markings;
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
			for (int i = 0; i < 16; i++) cboGlobCargo.Items.Add("Global Cargo " + (i + 1).ToString());
			cboGlobCargo.SelectedIndex = 0;
			cboGlobSpecCargo.Items.Add("None");
			for (int i = 0; i < 16; i++) cboGlobSpecCargo.Items.Add("Global Cargo " + (i + 1).ToString());
			cboGlobSpecCargo.SelectedIndex = 0;
			#endregion
			#region Arr/Dep
			parameterRefresh(cboADPara);
			optADAndOr[0] = optArr1OR2;
			optADAndOr[1] = optArr3OR4;
			optADAndOr[2] = optArr12OR34;
			optADAndOr[3] = optDepOR;
			optADAndOr[4] = optArr1AND2;
			optADAndOr[5] = optArr3AND4;
			optADAndOr[6] = optArr12AND34;
			optADAndOr[7] = optDepAND;
			for (int i = 0; i < 4; i++)
			{
				optADAndOr[i].CheckedChanged += new EventHandler(optADAndOrArr_CheckedChanged);
				optADAndOr[i].Tag = i;
			}
			#endregion
			#region Waypoints
			_tableWP.Columns.Add("X"); _tableWP.Columns.Add("Y"); _tableWP.Columns.Add("Z");
			_tableWPRaw.Columns.Add("X"); _tableWPRaw.Columns.Add("Y"); _tableWPRaw.Columns.Add("Z");
			for (int i = 0; i < 4; i++) //initialize WPs
			{
				DataRow dr = _tableWP.NewRow();
				int j;
				for (j = 0; j < 3; j++) dr[j] = 0;  //set X Y Z to zero
				_tableWP.Rows.Add(dr);
				dr = _tableWPRaw.NewRow();
				for (j = 0; j < 3; j++) dr[j] = 0;  //mirror in raw table
				_tableWPRaw.Rows.Add(dr);
			}
			_tableOrder.Columns.Add("X"); _tableOrder.Columns.Add("Y"); _tableOrder.Columns.Add("Z");
			_tableOrderRaw.Columns.Add("X"); _tableOrderRaw.Columns.Add("Y"); _tableOrderRaw.Columns.Add("Z");
			for (int i = 0; i < 8; i++) //initialize WPs
			{
				DataRow dr = _tableOrder.NewRow();
				int j;
				for (j = 0; j < 3; j++) dr[j] = 0;  //set X Y Z to zero
				_tableOrder.Rows.Add(dr);
				dr = _tableOrderRaw.NewRow();
				for (j = 0; j < 3; j++) dr[j] = 0;  //mirror in raw table
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
			for (int i = 0; i < 12; i++)
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
			for (int i = 0; i < 8; i++)
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
			parameterRefresh(cboSkipPara);
			cboSkipOrder.SelectedIndex = 0;
			cboRole1Teams.Items.AddRange(Strings.RoleTeams);
			cboRole2Teams.Items.AddRange(Strings.RoleTeams);
			cboRole1Teams.Items[0] = "Role 1 Disabled";
			cboRole2Teams.Items[0] = "Role 2 Disabled";
			for (int i = 0; i < 8; i++)   //Update craft role designation dropdown items.
			{
				if (_mission.Teams[i].Name != "")
				{
					cboRole1Teams.Items[i + 1] = _mission.Teams[i].Name;
					cboRole2Teams.Items[i + 1] = _mission.Teams[i].Name;
				}
			}
			cboRole1Teams.SelectedIndex = 0;
			cboRole2Teams.SelectedIndex = 0;
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
			chkOpt[13] = chkOptBEnergy;
			chkOpt[14] = chkOptCNone;
			chkOpt[15] = chkOptCChaff;
			chkOpt[16] = chkOptCFlare;
			chkOpt[17] = chkOptCCluster;
			for (int i = 0; i < 18; i++)
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
			for (int i = 0; i < 10; i++)
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
			for (int i = 0; i < 10; i++)
			{
				chkSendTo[i].Leave += new EventHandler(chkSendToArr_Leave);
				chkSendTo[i].Tag = i;
			}
			optMessAndOr[0] = optMess1OR2;
			optMessAndOr[1] = optMess3OR4;
			optMessAndOr[2] = optMess12OR34;
			optMessAndOr[3] = optMessC1OR2;
			optMessAndOr[4] = optMess1AND2;
			optMessAndOr[5] = optMess3AND4;
			optMessAndOr[6] = optMess12AND34;
			optMessAndOr[7] = optMessC1AND2;
			for (int i = 0; i < 4; i++)
			{
				optMessAndOr[i].CheckedChanged += new EventHandler(optMessAndOrArr_CheckedChanged);
				optMessAndOr[i].Tag = i;
			}
			#endregion
			#region Globals
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
				optGlobAndOr[i * 2].CheckedChanged += new EventHandler(optGlobAndOrArr_CheckedChanged); // only for evens
				optGlobAndOr[i * 2].Tag = i * 2;
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
			for (int i = 0; i < 10; i++)
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
			for (int i = 0; i < 30; i++)
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
			for (_activeTeam = 0; _activeTeam < 10; _activeTeam++) teamRefresh();  //[JB] Added so it formats the dropdowns on startup like XvT.
			_activeTeam = 0;
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
			for (int i = 0; i < 16; i++)
			{
				lblGG[i].Click += new EventHandler(lblGGArr_Click);
				lblGG[i].Tag = i;
			}
			txtIFFs[0] = txtIFF3;
			txtIFFs[1] = txtIFF4;
			txtIFFs[2] = txtIFF5;
			txtIFFs[3] = txtIFF6;
			txtRegions[0] = txtRegion1;
			txtRegions[1] = txtRegion2;
			txtRegions[2] = txtRegion3;
			txtRegions[3] = txtRegion4;
			for (int i = 0; i < 4; i++)
			{
				txtIFFs[i].Leave += new EventHandler(txtIFFsArr_Leave);
				txtIFFs[i].Tag = i + 2;
				txtRegions[i].Leave += new EventHandler(txtRegionsArr_Leave);
				txtRegions[i].Tag = i;
			}
			#endregion

			#region ControlRegistration
			registerColorizedFGList(cboADTrigVar, cboADTrigType);
			registerColorizedFGList(cboSkipVar, cboSkipType);
			registerColorizedFGList(cboMessVar, cboMessType);
			registerColorizedFGList(cboMessFG, null);
			registerColorizedFGList(cboGlobalVar, cboGlobalType);
			registerColorizedFGList(cboOT1, cboOT1Type);
			registerColorizedFGList(cboOT2, cboOT2Type);
			registerColorizedFGList(cboOT3, cboOT3Type);
			registerColorizedFGList(cboOT4, cboOT4Type);
			registerColorizedFGList(cboArrMS, null);
			registerColorizedFGList(cboArrMSAlt, null);
			registerColorizedFGList(cboDepMS, null);
			registerColorizedFGList(cboDepMSAlt, null);
			registerColorizedFGList(cboMessPara, null);
			registerColorizedFGList(cboGlobalPara, null);
			registerColorizedFGList(cboADPara, null);
			registerColorizedFGList(cboGoalPara, null);
			registerColorizedFGList(cboSkipPara, null);

			registerFgMultiEdit(txtName, "Name", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName);
			registerFgMultiEdit(numCraft, "NumberOfCraft", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(numWaves, "NumberOfWaves", MultiEditRefreshType.ItemText);
			// numGG and numGU have special logic applied, doesn't update through this
			registerFgMultiEdit(chkGU, "GlobalNumbering", 0);
			registerFgMultiEdit(txtCargo, "Cargo", 0);
			registerFgMultiEdit(txtSpecCargo, "SpecialCargo", 0);
			registerFgMultiEdit(numSC, "SpecialCargoCraft", 0);
			registerFgMultiEdit(chkRandSC, "RandSpecCargo", 0);
			registerFgMultiEdit(cboCraft, "CraftType", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName);
			registerFgMultiEdit(cboIFF, "IFF", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName);
			registerFgMultiEdit(cboTeam, "Team", MultiEditRefreshType.ItemText);
			registerFgMultiEdit(cboAI, "AI", 0);
			registerFgMultiEdit(cboMarkings, "Markings", 0);
			registerFgMultiEdit(cboPlayer, "PlayerNumber", MultiEditRefreshType.ItemText);
			registerFgMultiEdit(cboPosition, "PlayerCraft", 0);
			registerFgMultiEdit(cboRadio, "Radio", 0);
			registerFgMultiEdit(cboFormation, "Formation", 0);
			registerFgMultiEdit(numLead, "FormLeaderDist", 0);
			registerFgMultiEdit(numSpacing, "FormDistance", 0);
			registerFgMultiEdit(cboStatus, "Status1", 0);
			registerFgMultiEdit(cboStatus2, "Status2", 0);
			registerFgMultiEdit(cboWarheads, "Missile", 0);
			registerFgMultiEdit(cboBeam, "Beam", 0);
			registerFgMultiEdit(cboCounter, "Countermeasures", 0);
			registerFgMultiEdit(numExplode, "ExplosionTime", 0);
			registerFgMultiEdit(numBackdrop, "Backdrop", 0);
			registerFgMultiEdit(cboGlobCargo, "GlobalCargo", 0);
			registerFgMultiEdit(cboGlobSpecCargo, "GlobalSpecialCargo", 0);

			registerFgMultiEdit(optArrMS, "ArrivalMethod1", 0);
			registerFgMultiEdit(cboArrMS, "ArrivalCraft1", 0);
			registerFgMultiEdit(optArrMSAlt, "ArrivalMethod2", 0);
			registerFgMultiEdit(cboArrMSAlt, "ArrivalCraft2", 0);
			registerFgMultiEdit(optDepMS, "DepartureMethod1", 0);
			registerFgMultiEdit(cboDepMS, "DepartureCraft1", 0);
			registerFgMultiEdit(optDepMSAlt, "DepartureMethod2", 0);
			registerFgMultiEdit(cboDepMSAlt, "DepartureCraft2", 0);
			registerFgMultiEdit(cboADTrigAmount, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel);
			registerFgMultiEdit(cboADTrigType, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel);
			registerFgMultiEdit(cboADTrigVar, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel);
			registerFgMultiEdit(cboADTrig, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(cboADPara, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel);
			registerFgMultiEdit(numADPara, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel);
			registerFgMultiEdit(numArrMin, "ArrivalDelayMinutes", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(numArrSec, "ArrivalDelaySeconds", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(cboAbort, "AbortTrigger", 0);
			registerFgMultiEdit(numDepMin, "DepartureTimerMinutes", 0);
			registerFgMultiEdit(numDepSec, "DepartureTimerSeconds", 0);
			registerFgMultiEdit(cboDiff, "Difficulty", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(chkArrHuman, "ArriveOnlyIfHuman", MultiEditRefreshType.ItemText);
			registerFgMultiEdit(txtGoalInc, "GoalTriggerInc", 0);
			registerFgMultiEdit(txtGoalComp, "GoalTriggerComp", 0);
			registerFgMultiEdit(txtGoalFail, "GoalTriggerFail", 0);
			registerFgMultiEdit(cboGoalAmount, "GoalTriggerAmount", MultiEditRefreshType.FgGoalLabel);
			registerFgMultiEdit(cboGoalArgument, "GoalTriggerArgument", MultiEditRefreshType.FgGoalLabel);
			registerFgMultiEdit(cboGoalTrigger, "GoalTriggerCondition", MultiEditRefreshType.FgGoalLabel);
			registerFgMultiEdit(cboGoalPara, "GoalTriggerParam", MultiEditRefreshType.FgGoalLabel);
			registerFgMultiEdit(numGoalActSeq, "GoalTriggerSequence", MultiEditRefreshType.FgGoalLabel);
			registerFgMultiEdit(numGoalPoints, "GoalTriggerPoints", MultiEditRefreshType.FgGoalLabel);
			registerFgMultiEdit(numGoalTimeLimit, "GoalTriggerParam", MultiEditRefreshType.FgGoalLabel);  // shares same value as Param
			registerFgMultiEdit(numUnk42, "GoalTriggerUnk42", 0);
			registerFgMultiEdit(chkGoalEnable, "GoalTriggerEnabled", MultiEditRefreshType.FgGoalLabel);

			registerFgMultiEdit(numSP1, "WPRegion1", MultiEditRefreshType.Map);
			registerFgMultiEdit(numSP2, "WPRegion2", MultiEditRefreshType.Map);
			registerFgMultiEdit(numSP3, "WPRegion3", MultiEditRefreshType.Map);
			registerFgMultiEdit(numHYP, "WPRegion4", MultiEditRefreshType.Map);
			registerFgMultiEdit(numPitch, "Pitch", MultiEditRefreshType.Map);
			registerFgMultiEdit(numYaw, "Yaw", MultiEditRefreshType.Map);
			registerFgMultiEdit(numRoll, "Roll", MultiEditRefreshType.Map);

			registerFgMultiEdit(cboOrders, "OrderCommand", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(txtOString, "OrderCustom", 0);
			registerFgMultiEdit(cboOThrottle, "OrderThrottle", 0);
			registerFgMultiEdit(cboOSpeed, "OrderSpeed", 0);
			registerFgMultiEdit(numOVar1, "OrderVar1", 0);
			registerFgMultiEdit(numOVar2, "OrderVar2", 0);
			registerFgMultiEdit(numOVar3, "OrderVar3", 0);
			registerFgMultiEdit(cboOT1, "OrderTarget1", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(cboOT2, "OrderTarget2", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(cboOT3, "OrderTarget3", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(cboOT4, "OrderTarget4", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(cboOT1Type, "OrderTarget1Type", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(cboOT2Type, "OrderTarget2Type", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(cboOT3Type, "OrderTarget3Type", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(cboOT4Type, "OrderTarget4Type", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(optOT1T2OR, "Order12Or", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(optOT3T4OR, "Order34Or", MultiEditRefreshType.OrderLabel);

			registerFgMultiEdit(cboOptCat, "OptCategory", 0);
			registerFgMultiEdit(numOptWaves, "OptNumWaves", MultiEditRefreshType.OptCraftLabel);
			registerFgMultiEdit(numOptCraft, "OptNumCraft", MultiEditRefreshType.OptCraftLabel);
			registerFgMultiEdit(cboOptCraft, "OptCraftType", MultiEditRefreshType.OptCraftLabel);
			registerFgMultiEdit(cboRole1Teams, "Role1Team", 0);
			registerFgMultiEdit(cboRole2Teams, "Role2Team", 0);
			registerFgMultiEdit(cboRole1, "Role1", 0);
			registerFgMultiEdit(cboRole2, "Role2", 0);
			registerFgMultiEdit(txtRole, "RoleText", 0);
			registerFgMultiEdit(cboPilot, "PilotID", 0);
			registerFgMultiEdit(cboSkipAmount, "SkipTrigger", MultiEditRefreshType.SkipLabel);
			registerFgMultiEdit(cboSkipVar, "SkipTrigger", MultiEditRefreshType.SkipLabel);
			registerFgMultiEdit(cboSkipType, "SkipTrigger", MultiEditRefreshType.SkipLabel);
			registerFgMultiEdit(cboSkipTrig, "SkipTrigger", MultiEditRefreshType.SkipLabel);
			registerFgMultiEdit(cboSkipPara, "SkipTrigger", MultiEditRefreshType.SkipLabel);
			registerFgMultiEdit(numSkipPara, "SkipTrigger", MultiEditRefreshType.SkipLabel);
			registerFgMultiEdit(optSkipOR, "SkipTriggerOr", MultiEditRefreshType.SkipLabel);
			registerFgMultiEdit(chkUnk15, "Unk15", 0);  // All other unknowns are grouped together

			registerMsgMultiEdit(cboMessColor, "MessColor", MultiEditRefreshType.ItemText);
			registerMsgMultiEdit(numMessDelay, "MessDelay", 0);
			registerMsgMultiEdit(cboMessFG, "MessFG", 0);
			registerMsgMultiEdit(cboMessAmount, "MessTrigger", 0);
			registerMsgMultiEdit(cboMessType, "MessTrigger", 0);
			registerMsgMultiEdit(cboMessVar, "MessTrigger", 0);
			registerMsgMultiEdit(cboMessTrig, "MessTrigger", MultiEditRefreshType.ItemText); // Refresh whether grayed out or not.
			registerMsgMultiEdit(cboMessPara, "MessTrigger", 0);
			registerMsgMultiEdit(numMessPara, "MessTrigger", 0);
			registerMsgMultiEdit(numMessUnk1, "MessUnk1", 0);
			registerMsgMultiEdit(numMessUnk2, "MessUnk2", 0);
			registerMsgMultiEdit(chkMessUnk3, "MessUnk3", 0);
			#endregion ControlRegistration

			applySettingsHandler(0, new EventArgs());  //[JB] Configurable colors were added to options.
			updateMissionTabs();

			bool hookEnginesInstalled = false;
			bool hookHangarInstalled = false;
			bool hookMissionObjectsInstalled = false;
			bool hookMissionTieInstalled = false;
			if (_config.XwaInstalled)
			{
				_hookBackdropInstalled = File.Exists(_config.XwaPath + "\\Hook_Backdrops.dll");
				hookEnginesInstalled = File.Exists(_config.XwaPath + "\\Hook_Engine_Sound.dll");
				hookHangarInstalled = File.Exists(_config.XwaPath + "\\Hook_Hangars.dll");
				hookMissionObjectsInstalled = File.Exists(_config.XwaPath + "\\Hook_Mission_Objects.dll");
				hookMissionTieInstalled = File.Exists(_config.XwaPath + "\\Hook_Mission_Tie.dll");
			}
			menuHooks.Enabled = (_hookBackdropInstalled | hookEnginesInstalled | hookHangarInstalled | hookMissionObjectsInstalled | hookMissionTieInstalled);
		}
		void registerFgMultiEdit(Control control, string propertyName, MultiEditRefreshType refreshType)
		{
			Common.AddControlChangedHandler(control, flightgroupMultiEditHandler);
			control.Tag = new MultiEditProperty(propertyName, refreshType);
		}
		void registerMsgMultiEdit(Control control, string propertyName, MultiEditRefreshType refreshType)
		{
			Common.AddControlChangedHandler(control, messageMultiEditHandler);
			control.Tag = new MultiEditProperty(propertyName, refreshType);
		}
		Mission.Trigger getTriggerFromControls(ComboBox amount, ComboBox varType, ComboBox var, ComboBox condition, ComboBox param1, NumericUpDown param2)
		{
			Mission.Trigger ret = new Mission.Trigger
			{
				Amount = (byte)amount.SelectedIndex,
				VariableType = (byte)varType.SelectedIndex,
				Variable = (byte)var.SelectedIndex,
				Condition = (byte)condition.SelectedIndex,
				Parameter1 = (byte)param1.SelectedIndex,
				Parameter2 = (byte)param2.Value
			};
			return ret;
		}
		#endregion methods

		#region event handlers
		//[JB] Apply color changes to all interactive labels.  This is a callback event when the program settings are updated.
		void applySettingsHandler(object sender, EventArgs e)
		{
			foreach (Label lbl in lblADTrig) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeArrDepTrigger.ToString());  //Tags are set to ints, but casting objects throws an exception, so convert to string and check those instead.
			foreach (Label lbl in lblGoal) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeFGGoal.ToString());
			foreach (Label lbl in lblOrder) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeOrder.ToString());
			foreach (Label lbl in lblOptCraft) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeOptionCraft.ToString());
			foreach (Label lbl in lblMessTrig) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeMessageTrigger.ToString());
			foreach (Label lbl in lblGlobTrig) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeGlobalTrigger.ToString());
			foreach (Label lbl in lblTeam) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeTeam.ToString());
			foreach (Label lbl in lblGG) setInteractiveLabelColor(lbl, false);  //No variable tracks which one is selected, set colors but ignore highlight.
			lblGGArr_Click(lblGG[0], new EventArgs());
			setInteractiveLabelColor(lblSkipTrig1, _activeSkipTrigger == 0);
			setInteractiveLabelColor(lblSkipTrig2, _activeSkipTrigger == 1);
		}

		void briefingModifiedCallback(object sender, EventArgs e)
		{
			Common.Title(this, _loading);
		}

		void colorizedComboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			ComboBox variable = (ComboBox)sender;
			colorizedFGList.TryGetValue(variable, out ComboBox variableType);
			bool colorize = true;
			if (variableType != null)        //If a VariableType selection control is attached, check that a Flight Group type is selected.
				colorize = (variableType.SelectedIndex == 1 || variableType.SelectedIndex == 0xF);

			int paramOffset = 0;
			if (variableType == null && variable.Items.Count >= _mission.FlightGroups.Count + 5 && _mission.FlightGroups.Count >= 1)  //Detect if it's a parameter dropdown, which have 5 entries (for region #) before the FG list starts.  Parameter dropdowns are not attached to VariableType dropdowns, either.
			{
				if (variable.Items[5].ToString() == _mission.FlightGroups[0].ToString(false))  //Check to make sure it really does contain FGs by checking the first one.
					paramOffset = 5;
				else
					colorize = false;  //Should never happen, but to be safe...
			}

			if (e.Index == -1 || e.Index - paramOffset >= _mission.FlightGroups.Count) colorize = false;

			e.DrawBackground();
			// Setting the control BackColor will interrupt the drawing, so draw each item manually.
			if (colorize && !e.State.HasFlag(DrawItemState.Selected)) e.Graphics.FillRectangle(Brushes.Black, e.Bounds);
			Brush brText = SystemBrushes.ControlText;
			if (colorize) brText = getFlightGroupDrawColor(e.Index - paramOffset);
			if (colorize && brText == SystemBrushes.ControlText) brText = Brushes.LightGray;
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
		void form_Closing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.ApplicationExitCall) return;
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
				Common.Title(this, false);  // Since we're not loading, any change marks as dirty.
			}
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.ItemText)) listRefreshSelectedItems();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.CraftName)) updateFGList();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.FgGoalLabel)) goalLabelRefresh();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.OrderLabel)) orderLabelRefresh();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.OptCraftLabel)) optCraftLabelRefresh();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.Map)) refreshMap(-1);
		}
		void messageMultiEditHandler(object sender, EventArgs e)
		{
			if (_loading) return;
			MultiEditProperty prop = (MultiEditProperty)((Control)sender).Tag;
			if (prop.Name != "")
			{
				setMessageProperty(/*prop.RefreshType,*/ prop.Name, Common.GetControlValue(sender));
				Common.Title(this, false);
			}
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.ItemText)) messRefreshSelectedItems();
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
			toolXWA.Focus();    // clicking the toolbar doesn't remove focus, so last change may not be saved
			switch (toolXWA.Buttons.IndexOf(e.Button))
			{
				case 0:     //New Mission
					menuNewXWA_Click("toolbar", new EventArgs());
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
					savXWA.ShowDialog();
					break;
				case 5:     //New Item
					if (tabMain.SelectedIndex == 0) newFG();
					else if (tabMain.SelectedIndex == 1) newMess();
					break;
				case 6:     //Delete Item
					menuDelete_Click("toolbar", new EventArgs());  //[JB] Changed to call the function directly since that function now does more conditional checks.
					break;
				case 7:     //Copy Item
					menuCopy_Click("toolbar", new EventArgs());
					break;
				case 8:     //Paste Item
					menuPaste_Click("toolbar", new EventArgs());
					break;
				case 10:    //Map
					menuMap_Click("toolbar", new EventArgs());
					break;
				case 11:    //Briefing
					menuBrief_Click("toolbar", new EventArgs());
					break;
				case 12:    //Verify
					menuVerify_Click("toolbar", new EventArgs());
					break;
				case 14:    //Options
					menuOptions_Click("toolbar", new EventArgs());
					break;
				case 15:    //LST
					menuLST_Click("toolbar", new EventArgs());
					break;
				case 16:    // Wav
					menuWav_Click("toolbar", new EventArgs());
					break;
				case 17:    //Help
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
		void menuBrief_Click(object sender, EventArgs e)
		{
			Common.Title(this, false);
			try { _fBrief.Close(); }  //[JB] Prevent opening multiple dialogs. Same as XvT code.
			catch { /* do nothing */ }
			_fBrief = new BriefingForm(_mission.Briefings, briefingModifiedCallback);
			_fBrief.Show();
		}
		void menuDelete_Click(object sender, EventArgs e)
		{
			if (tabMain.SelectedIndex == 0)
			{
				if ((sender.ToString() == "toolbar") || (lstFG.Focused)) deleteFG();
			}
			else if (tabMain.SelectedIndex == 1)
			{
				if ((sender.ToString() == "toolbar") || (lstMessages.Focused)) deleteMess();
			}
		}
		void menuCopy_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream = new MemoryStream();
			DataObject data = new DataObject();

			if (sender.ToString() == "AD" || hasFocus(lblADTrig))  //[JB] Detect if triggers have focus
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger]);
				data.SetText(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].ToString());
			}
			else if (sender.ToString() == "Goal" || hasFocus(lblGoal))  //[JB] Detect if goals have focus
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal]);
				data.SetText(_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].ToString());
			}
			else if (sender.ToString() == "Order" || hasFocus(lblOrder))  //[JB] Detect if orders have focus
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Orders[(int)numORegion.Value - 1, _activeOrder % 4]);   //[JB] Fixed copy from the appropriate region.
				data.SetText(_mission.FlightGroups[_activeFG].Orders[(int)numORegion.Value - 1, _activeOrder % 4].ToString());
			}
			else if (sender.ToString() == "OrderWP")
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Orders[(int)numWPOrderRegion.Value - 1, (int)numWPOrder.Value - 1]);   // Copy the whole order
				data.SetText(_mission.FlightGroups[_activeFG].Orders[(int)numWPOrderRegion.Value - 1, (int)numWPOrder.Value - 1].ToString());
			}
			else if (sender.ToString() == "Skip" || (lblSkipTrig1.Focused || lblSkipTrig2.Focused))  //[JB] Detect if triggers have focus
			{
				int i = (lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1);
				int order = cboSkipOrder.SelectedIndex;  //[JB] Fixed copying
				if (order < 0) order = 0;
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Orders[order / 4, order % 4].SkipTriggers[i]);  //[JB] Fixed from _activeOrder
				data.SetText(_mission.FlightGroups[_activeFG].Orders[order / 4, order % 4].SkipTriggers[i].ToString());
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
			else if (sender.ToString() == "MessTrig" || hasFocus(lblMessTrig))  //[JB] Detect if triggers have focus
			{
				formatter.Serialize(stream, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger]);
				data.SetText(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].ToString());
			}
			else
			{
				switch (tabMain.SelectedIndex)
				{
					case 0:
						formatter.Serialize(stream, _mission.FlightGroups[_activeFG]);
						data.SetText(_mission.FlightGroups[_activeFG].ToString());
						break;
					case 1:
						if (_mission.Messages.Count != 0)
						{
							formatter.Serialize(stream, _mission.Messages[_activeMessage]);
							data.SetText(_mission.Messages[_activeMessage].MessageString);
						}
						break;
					case 2:
						formatter.Serialize(stream, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4]);
						data.SetText(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].ToString());
						break;
					case 3:
						formatter.Serialize(stream, _mission.Teams[_activeTeam]);
						data.SetText(_mission.Teams[_activeTeam].Name);
						break;
					default:
						break;
				}
			}
			data.SetData("yogeme", false, stream);
			Clipboard.SetDataObject(data, true);
		}
		void menuCut_Click(object sender, EventArgs e)
		{
			if(Common.Cut(ActiveControl))
				Common.Title(this, false);
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
		void menuLibrary_Click(object sender, EventArgs e)
		{
			_fLibrary?.Close();
			_fLibrary = new FlightGroupLibraryForm(Settings.Platform.XWA, _mission.FlightGroups, flightGroupLibraryCallback);
		}
		void flightGroupLibraryCallback(object sender, EventArgs e)
		{
			foreach (FlightGroup fg in (object[])sender)
			{
				if (fg == null || !newFG())
					break;
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
		void menuHooks_Click(object sender, EventArgs e)
		{
			try { new XwaHookDialog(_mission, _config).ShowDialog(); }
			catch (ObjectDisposedException) { /* do nothing */ }
		}
		void menuHyperbuoy_Click(object sender, EventArgs e)
		{
			lstFG.Focus();
			int fgCount = _mission.FlightGroups.Count;
			new HyperbuoyDialog(_mission).ShowDialog();
			if (_mission.FlightGroups.Count > fgCount)
			{
				for (int i = fgCount; i < _mission.FlightGroups.Count; i++)
				{
					if (_mission.FlightGroups[i].ArrivesIn30Seconds) craftStart(_mission.FlightGroups[i], true);
					lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
				}
				updateFGList();
				Common.Title(this, false);  //[JB] Mark changes.
			}
		}
		void menuIDMR_Click(object sender, EventArgs e)
		{
			Common.LaunchIdmr();
		}
		void menuLST_Click(object sender, EventArgs e)
		{
			_fLST = new LstForm(Settings.Platform.XWA, _config);
			_fLST.Show();
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
		void menuMissionCraft_Click(object sender, EventArgs e)
		{
			string output = "";

			for (int f = 0; f < _mission.FlightGroups.Count; f++)
			{
				var s = getMissionCraftString(f);
				if (s != "") output += s + "\r\n";
			}

			if (output == "") output = "(None)";
			else output = "NOTE: Capital ships will be listed first, non-capital ship quantities will be added when possible.\r\n\r\n" + output;
			MessageBox.Show(output, "Mission Craft List", MessageBoxButtons.OK);
		}
		void menuNewXwing_Click(object sender, EventArgs e)  //[JB] Added Xwing support.
		{
			promptSave();
			closeForms();
			_applicationExit = false;
			new XwingForm(_config).Show();
			Close();
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
			updateMissionTabs();
			lstMessages.Items.Clear();
			enableMessages(false);
			lblMessage.Text = "Message #0 of 0";
			for (_activeTeam = 0; _activeTeam < 10; _activeTeam++) teamRefresh();
			lblTeamArr_Click(lblTeam[0], new EventArgs());
			lstFG.SelectedIndex = 0;
			updateFGList(); //[JB] Need to refresh all the dropdown boxes
			_loading = false;
		}
		void menuOpen_Click(object sender, EventArgs e)
		{
			promptSave();
			opnXWA.FileName = _mission.MissionFileName;
			if (_mission.MissionFileName != "NewMission.tie") opnXWA.InitialDirectory = Directory.GetParent(_mission.MissionPath).FullName;	// follow current mission, fixes when switching platforms it wouldn't follow
			if (opnXWA.ShowDialog() == DialogResult.OK)
			{
				opnXWA_FileOk(0, new System.ComponentModel.CancelEventArgs());
				_config.SetWorkingPath(Path.GetDirectoryName(opnXWA.FileName));
				opnXWA.InitialDirectory = _config.GetWorkingPath(); //Update since folder may have changed
			}
		}
		void menuOptions_Click(object sender, EventArgs e)
		{
			new OptionsDialog(_config, applySettingsHandler).ShowDialog();
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

			Mission.Trigger trig = null;
			if (obj.GetType() == typeof(Mission.Trigger)) trig = (Mission.Trigger)obj;
			else if (obj.GetType() == typeof(Platform.Tie.Mission.Trigger))
			{
				trig = (Platform.Tie.Mission.Trigger)obj;	// upgrading is implicit
				if (trig.VariableType == 2 /* CraftType */) trig.Variable++;
			}
			else if (obj.GetType() == typeof(Platform.Xvt.Mission.Trigger))
			{
				trig = (Platform.Xvt.Mission.Trigger)obj;
				if (trig.VariableType == 2 /* CraftType */) trig.Variable++;
			}

			FlightGroup.Order ord = null;
			if (obj.GetType() == typeof(FlightGroup.Order)) ord = (FlightGroup.Order)obj;
			else if (obj.GetType() == typeof(Platform.Tie.FlightGroup.Order))
			{
				ord = (Platform.Tie.FlightGroup.Order)obj;
				if (ord.Target1Type == 2) ord.Target1++;
				if (ord.Target2Type == 2) ord.Target2++;
				if (ord.Target3Type == 2) ord.Target3++;
				if (ord.Target4Type == 2) ord.Target4++;
			}
			else if (obj.GetType() == typeof(Platform.Xvt.FlightGroup.Order))
			{
				ord = (Platform.Xvt.FlightGroup.Order)obj;
				if (ord.Target1Type == 2) ord.Target1++;
				if (ord.Target2Type == 2) ord.Target2++;
				if (ord.Target3Type == 2) ord.Target3++;
				if (ord.Target4Type == 2) ord.Target4++;
			}

			if (sender.ToString() == "AD" || hasFocus(lblADTrig))  //[JB] Detect if triggers have focus
			{
				try
				{
					foreach (FlightGroup fg in getSelectedFlightgroups())
					{
						craftStart(fg, false);
						fg.ArrDepTriggers[_activeArrDepTrigger] = new Mission.Trigger(trig);
						craftStart(fg, true);
					}
					lblADTrigArr_Click(_activeArrDepTrigger, new EventArgs());
					labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
			}
			else if (sender.ToString() == "Order" || hasFocus(lblOrder))  //[JB] Detect if orders have focus
			{
				try
				{
					foreach (FlightGroup fg in getSelectedFlightgroups())
						fg.Orders[(int)numORegion.Value - 1, _activeOrder % 4] = new FlightGroup.Order(ord);
					lblOrderArr_Click(_activeOrder, new EventArgs());
					orderLabelRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
			}
			else if (sender.ToString() == "OrderWP")
			{
				try
				{
					foreach (FlightGroup fg in getSelectedFlightgroups())
						for (int w = 0; w < ord.Waypoints.Length; w++)
							fg.Orders[(int)numWPOrderRegion.Value - 1, (int)numWPOrder.Value - 1].Waypoints[w] = Idmr.Common.ObjectCopier.DeepClone(ord.Waypoints[w]);
					numWPOrder_ValueChanged("Paste", new EventArgs());
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
			}
			else if (sender.ToString() == "Goal" || hasFocus(lblGoal))  //[JB] Detect if goals have focus
			{
				try
				{
					FlightGroup.Goal goal = (FlightGroup.Goal)obj;
					foreach (FlightGroup fg in getSelectedFlightgroups())
						fg.Goals[_activeFGGoal] = new FlightGroup.Goal(goal);
					lblGoalArr_Click(_activeFGGoal, new EventArgs());
					goalLabelRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
			}
			else if (sender.ToString() == "Skip" || (lblSkipTrig1.Focused || lblSkipTrig2.Focused))  //[JB] Detect if triggers have focus
			{
				try
				{
					int j = (lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1);
					int r = cboSkipOrder.SelectedIndex / 4;
					int o = cboSkipOrder.SelectedIndex % 4;
					foreach (FlightGroup fg in getSelectedFlightgroups())
						fg.Orders[r, o].SkipTriggers[j] = new Mission.Trigger(trig);
					lblSkipTrigArr_Click(j, new EventArgs());
					labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[j], (j == 0 ? lblSkipTrig1 : lblSkipTrig2));    // no array, hence explicit naming
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
			}
			else if (Common.Paste(ActiveControl, obj))
			{
				Common.Title(this, false);
			}
			else if (ActiveControl.GetType() == typeof(DataGridTextBox))
			{
				try
				{
					string str = obj.ToString();
					if (str.IndexOf("System.", 0) != -1) throw new FormatException();   // bypass byte[]
					if (str.IndexOf("Idmr.", 0) != -1) throw new FormatException();
					DataGrid dg = (DataGrid)ActiveControl.Parent;
					int row = dg.CurrentRowIndex;
					DataTable dt = ((DataView)dg.DataSource).Table;
					dt.Rows[row][dg.CurrentCell.ColumnNumber] = str;
					if (dt.TableName == "Waypoints") tableWP_RowChanged("paste", new DataRowChangeEventArgs(dt.Rows[row], DataRowAction.Change));
					else if (dt.TableName == "Waypoints_Raw") tableWPRaw_RowChanged("paste", new DataRowChangeEventArgs(dt.Rows[row], DataRowAction.Change));
					else if (dt.TableName == "Orders") tableOrder_RowChanged("paste", new DataRowChangeEventArgs(dt.Rows[row], DataRowAction.Change));
					else tableOrderRaw_RowChanged("paste", new DataRowChangeEventArgs(dt.Rows[row], DataRowAction.Change));
				}
				catch { /* do nothing */ }
			}
			else if (sender.ToString() == "MessTrig" || hasFocus(lblMessTrig))  //[JB] Detect if triggers have focus
			{
				try
				{
					foreach (Platform.Xwa.Message msg in getSelectedMessages())
						msg.Triggers[_activeMessageTrigger] = new Mission.Trigger(trig);
					lblMessTrigArr_Click(_activeMessageTrigger, new EventArgs());
					labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
					Common.Title(this, false);
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
							FlightGroup fg = (FlightGroup)obj;
#pragma warning disable IDE0016 // Use 'throw' expression. Can't use due to needing the null check before new()
							if (fg == null) throw new FormatException();
							if (!newFG()) break;

							_mission.FlightGroups[_activeFG] = fg;
							refreshMap(-1);
							updateFGList();
							listRefreshItem(_activeFG);
							_startingShips--;
							lstFG.SelectedIndex = _activeFG;
							lstFG_SelectedIndexChanged(0, new EventArgs()); // Force refresh of form controls.
							craftStart(fg, true);
							lstFG.Focus();
						}
						catch { /* do nothing */ }
						break;
					case 1:
						try
						{
							Platform.Xwa.Message mess = (Platform.Xwa.Message)obj;
							if (mess == null) throw new FormatException();
							if (!newMess()) break;

							_mission.Messages[_activeMessage] = mess;
							messRefreshItem(_activeMessage);
							lstMessages.SelectedIndex = _activeMessage;
							lstMessages_SelectedIndexChanged(0, new EventArgs());
#pragma warning restore IDE0016 // Use 'throw' expression
						}
						catch { /* do nothing */ }
						break;
					case 2:
						try
						{
							_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4] = trig ?? throw new FormatException();
							lblGlobTrigArr_Click(_activeGlobalTrigger, new EventArgs());
							Common.Title(this, false);
						}
						catch { /* do nothing */ }
						break;
					case 3:
						try
						{
							_mission.Teams[_activeTeam] = (Team)obj ?? throw new FormatException();
							teamRefresh();
							lblTeamArr_Click(lblTeam[_activeTeam], new EventArgs());
							Common.Title(this, false);
						}
						catch { /* do nothing */ }
						break;
				}
			}
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
				_loading = true;        //turned false in previous line
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_config.SetWorkingPath(Path.GetDirectoryName(mission)); //[JB] Update last-accessed
			opnXWA.InitialDirectory = _config.GetWorkingPath();
			savXWA.InitialDirectory = _config.GetWorkingPath();
			_loading = false;
		}
		void menuSave_Click(object sender, EventArgs e)
		{
			if (_mission.MissionPath == "\\NewMission.tie") savXWA.ShowDialog();
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
					if (_mission.FlightGroups[i].Backdrop == 55 && _mission.FlightGroups[i].Waypoints[0].Region == region)
					{
						MessageBox.Show("Mission already contains SuperBackdrops in " + _mission.Regions[region] + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}
					else if (_mission.FlightGroups[i].Waypoints[0].Region == region) requiredQty++;
				}
			}
			if (_mission.FlightGroups.Count >= Mission.FlightGroupLimit - requiredQty)
			{
				MessageBox.Show("Mission contains too many Flight Groups to add Super Backdrops (" + requiredQty + " needed).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			for (int i = 0; i < 6; i++)
			{
				int index = _mission.FlightGroups.Add();
				_mission.FlightGroups[index].CraftType = 0xB7;
				_mission.FlightGroups[index].Backdrop = 55;
				_mission.FlightGroups[index].LightRGB = "0.0 0.0 0.0";
				_mission.FlightGroups[index].Brightness = "0.0";
				if (i < 2)
				{
					_mission.FlightGroups[index].BackdropSize = "0.895";
					_mission.FlightGroups[index].Shadow = 6;	// HACK: Should be Shadow 5
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
			for (int i = 0, copied = 0; copied < requiredQty - 6; i++)
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
				_fMap.MapPaint();
			}
			catch { /* do nothing */ }
		}
		void menuTest_Click(object sender, EventArgs e)
		{
			if (_config.ConfirmTest)
			{
				DialogResult res = new TestDialog(_config).ShowDialog();
				if (res == DialogResult.Cancel) return;
			}
			// prep stuff
			menuSave_Click("menuTest_Click", new EventArgs());
			if (_mission.MissionPath == "\\NewMission.tie") return;

			string path = Directory.GetParent(_mission.MissionPath).Parent.FullName + "\\";
			if (!File.Exists(path + "XWINGALLIANCE.exe"))
			{
				System.Diagnostics.Debug.WriteLine("XWA not detected at MissionPath, default used");
				path = _config.XwaPath + "\\";
			}

			bool localMission = _mission.MissionPath.ToLower().Contains(path.ToLower());
			string fileName = (!localMission ? path + "Missions\\" + _mission.MissionFileName : _mission.MissionPath);
			

			if (_config.VerifyTest && !_config.Verify) Common.RunVerify(_mission.MissionPath, _config);
			int index = 0;
			while (File.Exists(path + "test" + index + "0.plt")) index++;
			string pilot = "test" + index + "0.plt";
			string lst = "MISSIONS\\MISSION.LST";
			string backup = "MISSIONS\\MISSION_" + index + ".bak";

			if (!localMission)
			{
				if (File.Exists(fileName))
				{
					DialogResult res = MessageBox.Show("You are not working in the platform directory and a mission with that filename exists. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (res == DialogResult.No) return;
					File.Copy(fileName, fileName + ".bak", true);
				}
				File.Copy(_mission.MissionPath, fileName, true);
			}

			// pilot file edit
			File.Copy(Application.StartupPath + "\\xwatest0.plt", path + pilot);
			FileStream pilotFile = File.OpenWrite(path + pilot);
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
			xwa.StartInfo.FileName = path + "XWINGALLIANCE.exe";
			xwa.StartInfo.Arguments = "/skipintro";
			xwa.StartInfo.UseShellExecute = false;
			xwa.StartInfo.WorkingDirectory = path;
			File.Copy(path + lst, path + backup, true);
			StreamReader sr = File.OpenText(path + "CONFIG.CFG");
			string contents = sr.ReadToEnd();
			sr.Close();
			int lastpilot = contents.IndexOf("lastpilot ") + 10;
			int nextline = contents.IndexOf("\r\n", lastpilot);
			string modified = contents.Substring(0, lastpilot) + "test" + index + contents.Substring(nextline);
			StreamWriter sw = new FileInfo(path + "CONFIG.CFG").CreateText();
			sw.Write(modified);
			sw.Close();
			sr = File.OpenText(path + lst);
			contents = sr.ReadToEnd();
			sr.Close();
			string[] expanded = contents.Replace("\r\n", "\0").Split('\0');
			expanded[3] = "7";
			expanded[4] = _mission.MissionFileName;
			expanded[5] = "!MISSION_7_DESC!YOGEME: " + expanded[4];
			modified = string.Join("\r\n", expanded);
			sw = new FileInfo(path + lst).CreateText();
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

			if (_config.DeleteTestPilots) File.Delete(path + pilot);
			File.Copy(path + backup, path + lst, true);
			File.Delete(path + backup);
			if (!localMission)
			{
				if (File.Exists(fileName + ".bak"))
				{
					File.Copy(fileName + ".bak", fileName, true);
					File.Delete(fileName + ".bak");
				}
				else
					File.Delete(fileName);
			}
			System.Diagnostics.Debug.WriteLine("Testing complete");
		}
		void menuVerify_Click(object sender, EventArgs e)
		{
			menuSave_Click("Verify", new System.EventArgs());
			if (!_config.Verify) Common.RunVerify(_mission.MissionPath, _config);    //prevents from doing this twice due to Save
		}
		void menuWav_Click(object sender, EventArgs e)
		{
			if (_mission.MissionPath == "\\NewMission.tie")
			{
				MessageBox.Show("Mission must be saved prior to WAV definition.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (!_config.XwaInstalled)
			{
				MessageBox.Show("XWA must be installed to use the WAV Manager.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (_fWav != null && !_fWav.IsDisposed)
			{
				_fWav.Reload();
				_fWav.Focus();
			}
			else
			{
				_fWav = new XwaWavForm(_config, _mission);
				if (!_fWav.IsDisposed) _fWav.Show();
			}
		}
		#endregion
		#region FlightGroups
		/// <summary>Counts all trigger and parameter references of a FG</summary>
		/// <param name="fgIndex">Index within _mission.FlightGroups</param>
		/// <remarks>Used to populate the counters in the confirm deletion dialog</remarks>
		int[] countFlightGroupReferences(int fgIndex)
		{
			int[] count = new int[9];
			const int cMothership = 1, cArrDep = 2, cOrder = 3, cSkip = 4, cGoal = 5, cMessage = 6, cFGGoal = 8; //cBrief = 7 Not needed for XWA
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (fgIndex == i)
					continue;

				FlightGroup fg = _mission.FlightGroups[i];
				if (fg.ArrivalMethod1 == true && fg.ArrivalCraft1 == fgIndex) count[cMothership]++;
				if (fg.ArrivalMethod2 == true && fg.ArrivalCraft2 == fgIndex) count[cMothership]++;
				if (fg.DepartureMethod1 == true && fg.DepartureCraft1 == fgIndex) count[cMothership]++;
				if (fg.DepartureMethod2 == true && fg.DepartureCraft2 == fgIndex) count[cMothership]++;
				foreach (Mission.Trigger adt in fg.ArrDepTriggers)
				{
					//Note: many FGs initially present in battle use the first FG for Arr/Dep condition, even though the FG isn't actually used (condition is TRUE or FALSE). In which case no need to warn.
					if ((adt.VariableType == 1 || adt.VariableType == 15) && adt.Variable == fgIndex && adt.Condition != 0 && adt.Condition != 10) count[cArrDep]++;
					if (adt.Parameter1 == 5 + fgIndex) count[cArrDep]++;
				}
				foreach (FlightGroup.Order order in fg.Orders)
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
				foreach (var goal in fg.Goals)
					if (goal.Parameter == 5 + fgIndex)
						count[cFGGoal]++;
			}

			foreach (Globals global in _mission.Globals)
				foreach (Globals.Goal goal in global.Goals)
					foreach (Mission.Trigger trig in goal.Triggers)
					{
						if ((trig.VariableType == 1 || trig.VariableType == 15) && trig.Variable == fgIndex) count[cGoal]++;
						if (trig.Parameter1 == 5 + fgIndex) count[cGoal]++;
					}

			foreach (Platform.Xwa.Message msg in _mission.Messages)
			{
				if (msg.OriginatingFG == fgIndex && fgIndex > 0) count[cMessage]++;  //suppress warning if fgIndex==0 since that's normal for a default/empty message

				foreach (Mission.Trigger trig in msg.Triggers)
				{
					if ((trig.VariableType == 1 || trig.VariableType == 15) && trig.Variable == fgIndex) count[cMessage]++;
					if (trig.Parameter1 == 5 + fgIndex) count[cMessage]++;
				}
			}

			//XWA Briefing FGTag events don't seem to reference actual FGs.  No action necessary.

			for (int i = 1; i < 8; i++)
				count[0] += count[i];
			return count;
		}
		void deleteFG()
		{
			_fBrief?.Close(); //Close (which also saves) the briefing before accessing it.  Don't call save directly since this may cause FG index corruption if multiple FGs are deleted.

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
						string[] Reasons = new string[9] { "", "Mothership reference", "Arrival/Departure trigger", "Order target reference", "'Skip to Order' trigger/param", "Global Goal trigger/param", "Message trigger/param", "Briefing FG Tag", "Goal FG Parameter" };
						string breakdown = "";
						for (int i = 1; i < 9; i++)
							if (count[i] > 0) breakdown += "    " + count[i] + " " + Reasons[i] + ((count[i] > 1) ? "s" : "") + "\n";

						string s = "This Flight Group is referenced " + count[0] + " time" + ((count[0] > 1) ? "s" : "") + " in these cases:\n" + breakdown + "\nAll references targeting this flight group will be reset to default.";
						if (count[7] > 0) s += "\nAssociated Briefing FG Tag events will be deleted.";
						s += "\n\nAre you sure you want to delete this Flight Group?";
						DialogResult res = MessageBox.Show(s, "WARNING: Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (res == DialogResult.No)
							break;  // exit the outer for() loop
					}
				}
				replaceClipboardReference(_activeFG, -1, true);
				if (_mission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(_activeFG);
				craftStart(_mission.FlightGroups[_activeFG], false);
				if (_mission.FlightGroups.Count == 1)
				{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
					_activeFG = _mission.DeleteFG(_activeFG);  // Need to perform a full delete to wipe the FG indexes (messages or briefing tags may still have them).  The delete function always ensures that Count==1, so it must be inside this block, not before.
					_mission.FlightGroups.Clear();
					_activeFG = 0;
					_mission.FlightGroups[0].CraftType = _config.XwaCraft;
					_mission.FlightGroups[0].IFF = _config.XwaIff;
					craftStart(_mission.FlightGroups[0], true);
				}
				else _activeFG = _mission.DeleteFG(_activeFG);  // Actual delete moved to platform.
#pragma warning restore IDE0059 // Unnecessary assignment of a value
			}
			// Fix bounds and make new selection.
			if (startFG >= _mission.FlightGroups.Count)
				startFG = _mission.FlightGroups.Count - 1;
			lstFG.SelectedIndex = startFG;
			updateFGList();
			refreshMap(-1);
			Common.Title(this, false);

			//[JB] Need to force these tabs to refresh, otherwise the trigger controls might display outdated information
			lstMessages_SelectedIndexChanged(0, new EventArgs());
			cboGlobalTeam_SelectedIndexChanged(0, new EventArgs());
			lstFG_SelectedIndexChanged(0, new EventArgs());
		}
		string generateGoalSummary()
		{
			//30 elements:  Primary,Prevent,Bonus, ... (repeat for each team)
			//Each element contains a list of strings for each line of text.
			//Was going to try and summarize global goals but the output was ugly, so removed it. Easy for the user to view those anyway.
			List<string>[] goalList = new List<string>[30];

			for (int i = 0; i < 30; i++)
				goalList[i] = new List<string>();

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
		/// <summary>Check mission for GlobalGroup references and update if needed.</summary>
		/// <param name="update">Whether or not to update the GG value, or just report it</param>
		/// <returns>If <paramref name="update"/> is <i>false</i>, returns <i>true</i> if GG is unique and is referenced elsewhere.<br/>
		/// If <paramref name="update"/> is <i>true</i>, always returns <i>false</i>.</returns>
		/// <remarks>The update method allows the ability to update references, even if the GG is not unique. It's not implemented in this manner, but it's possible.</remarks>
		bool updateGG(bool update)
		{
			int refCount = 0;
			int ggCount = 0;
			byte gg = _mission.FlightGroups[_activeFG].GlobalGroup;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_activeFG == i)
					continue;

				FlightGroup fg = _mission.FlightGroups[i];
				if (fg.GlobalGroup == gg)
				{
					ggCount++;
					continue;
				}

				foreach (Mission.Trigger adt in fg.ArrDepTriggers)
					if ((adt.VariableType == 8 || adt.VariableType == 20) && adt.Variable == gg)
					{
						if (update) adt.Variable = (byte)numGG.Value;
						else refCount++;
					}
				foreach (FlightGroup.Order order in fg.Orders)
				{
					if ((order.Target1Type == 8 || order.Target1Type == 20) && order.Target1 == gg)
					{
						if (update) order.Target1 = (byte)numGG.Value;
						else refCount++;
					}
					if ((order.Target2Type == 8 || order.Target2Type == 20) && order.Target2 == gg)
					{
						if (update) order.Target2 = (byte)numGG.Value;
						else refCount++;
					}
					if ((order.Target3Type == 8 || order.Target3Type == 20) && order.Target3 == gg)
					{
						if (update) order.Target3 = (byte)numGG.Value;
						else refCount++;
					}
					if ((order.Target4Type == 8 || order.Target4Type == 20) && order.Target4 == gg)
					{
						if (update) order.Target4 = (byte)numGG.Value;
						else refCount++;
					}
					foreach (Mission.Trigger sk in order.SkipTriggers)
						if ((sk.VariableType == 8 || sk.VariableType == 20) && sk.Variable == gg)
						{
							if (update) sk.Variable = (byte)numGG.Value;
							else refCount++;
						}
				}
			}
			foreach (Globals global in _mission.Globals)
				foreach (Globals.Goal goal in global.Goals)
					foreach (Mission.Trigger trig in goal.Triggers)
						if ((trig.VariableType == 8 || trig.VariableType == 20) && trig.Variable == gg)
						{
							if (update) trig.Variable = (byte)numGG.Value;
							else refCount++;
						}
			foreach (Platform.Xwa.Message msg in _mission.Messages)
				foreach (Mission.Trigger trig in msg.Triggers)
					if ((trig.VariableType == 8 || trig.VariableType == 20) && trig.Variable == gg)
					{
						if (update) trig.Variable = (byte)numGG.Value;
						else refCount++;
					}
			// since I'm using foreach and don't have the index, just redo all of the visible ones in case it's one we updated
			for (int i = 0; i < 12; i++) labelRefresh(_mission.Globals[_activeTeam].Goals[i / 4].Triggers[i % 4], lblGlobTrig[i]);
			lblGlobTrigArr_Click(_activeGlobalTrigger, new EventArgs());
			if (_mission.Messages.Count > 0)
			{
				for (int i = 0; i < 6; i++) labelRefresh(_mission.Messages[_activeMessage].Triggers[i], lblMessTrig[i]);
				lblMessTrigArr_Click(_activeMessageTrigger, new EventArgs());
			}
			return (!update && ggCount == 0 && refCount > 0);
		}
		/// <summary>Check mission for GlobalUnit references and update if needed.</summary>
		/// <param name="update">Whether or not to update the GU value, or just report it</param>
		/// <returns>If <paramref name="update"/> is <i>false</i>, returns <i>true</i> if GU is unique and is referenced elsewhere.<br/>
		/// If <paramref name="update"/> is <i>true</i>, always returns <i>false</i>.</returns>
		/// <remarks>The update method allows the ability to update references, even if the GU is not unique. It's not implemented in this manner, but it's possible.</remarks>
		bool updateGU(bool update)
		{
			int refCount = 0;
			int guCount = 0;
			byte gu = _mission.FlightGroups[_activeFG].GlobalUnit;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_activeFG == i)
					continue;

				FlightGroup fg = _mission.FlightGroups[i];
				if (fg.GlobalUnit == gu)
				{
					guCount++;
					continue;
				}

				foreach (Mission.Trigger adt in fg.ArrDepTriggers)
					if ((adt.VariableType == 23 || adt.VariableType == 24) && adt.Variable == gu)
					{
						if (update) adt.Variable = (byte)numGU.Value;
						else refCount++;
					}
				foreach (FlightGroup.Order order in fg.Orders)
				{
					if ((order.Target1Type == 23 || order.Target1Type == 24) && order.Target1 == gu)
					{
						if (update) order.Target1 = (byte)numGU.Value;
						else refCount++;
					}
					if ((order.Target2Type == 23 || order.Target2Type == 24) && order.Target2 == gu)
					{
						if (update) order.Target2 = (byte)numGU.Value;
						else refCount++;
					}
					if ((order.Target3Type == 23 || order.Target3Type == 24) && order.Target3 == gu)
					{
						if (update) order.Target3 = (byte)numGU.Value;
						else refCount++;
					}
					if ((order.Target4Type == 23 || order.Target4Type == 24) && order.Target4 == gu)
					{
						if (update) order.Target4 = (byte)numGU.Value;
						else refCount++;
					}
					foreach (Mission.Trigger sk in order.SkipTriggers)
						if ((sk.VariableType == 23 || sk.VariableType == 24) && sk.Variable == gu)
						{
							if (update) sk.Variable = (byte)numGU.Value;
							else refCount++;
						}
				}
			}
			foreach (Globals global in _mission.Globals)
				foreach (Globals.Goal goal in global.Goals)
					foreach (Mission.Trigger trig in goal.Triggers)
						if ((trig.VariableType == 23 || trig.VariableType == 24) && trig.Variable == gu)
						{
							if (update) trig.Variable = (byte)numGU.Value;
							else refCount++;
						}
			foreach (Platform.Xwa.Message msg in _mission.Messages)
				foreach (Mission.Trigger trig in msg.Triggers)
					if ((trig.VariableType == 23 || trig.VariableType == 24) && trig.Variable == gu)
					{
						if (update) trig.Variable = (byte)numGU.Value;
						else refCount++;
					}
			// since I'm using foreach and don't have the index, just redo all of the visible ones in case it's one we updated
			for (int i = 0; i < 12; i++) labelRefresh(_mission.Globals[_activeTeam].Goals[i / 4].Triggers[i % 4], lblGlobTrig[i]);
			lblGlobTrigArr_Click(_activeGlobalTrigger, new EventArgs());
			if (_mission.Messages.Count > 0)
			{
				for (int i = 0; i < 6; i++) labelRefresh(_mission.Messages[_activeMessage].Triggers[i], lblMessTrig[i]);
				lblMessTrigArr_Click(_activeMessageTrigger, new EventArgs());
			}
			return (!update && guCount == 0 && refCount > 0);
		}
		List<FlightGroup> getSelectedFlightgroups()
		{
			List<FlightGroup> fgs = new List<FlightGroup>();
			foreach (int fgIndex in lstFG.SelectedIndices)
				fgs.Add(_mission.FlightGroups[fgIndex]);
			return fgs;
		}
		void setFlightgroupProperty(MultiEditRefreshType refreshType, string name, object value)
		{
			Mission.Trigger trig = null;  // Need a trigger to refresh certain labels.  Doesn't matter which item it's from.
			int orderReg = (int)(numORegion.Value - 1);
			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				if (refreshType.HasFlag(MultiEditRefreshType.CraftCount)) craftStart(fg, false);
				switch(name)
				{
					case "Name": fg.Name = (string)value; break;
					case "NumberOfCraft":
						fg.NumberOfCraft = Convert.ToByte(value);
						if (fg.SpecialCargoCraft > fg.NumberOfCraft) fg.SpecialCargoCraft = 0;
						break;
					case "NumberOfWaves": fg.NumberOfWaves = Convert.ToByte(value); break;
					case "GlobalNumbering": fg.GlobalNumbering = Convert.ToBoolean(value); break;
					// "GlobalGroup" and "GlobalUnit" have special logic, not handled here.
					case "Cargo": fg.Cargo = (string)value; break;
					case "SpecialCargo": fg.SpecialCargo = (string)value; break;
					case "SpecialCargoCraft":
						fg.SpecialCargoCraft = Convert.ToByte((int)value);
						if (fg.SpecialCargoCraft > fg.NumberOfCraft) fg.SpecialCargoCraft = 0;
						break;
					case "RandSpecCargo": fg.RandSpecCargo = Convert.ToBoolean(value); break;
					case "CraftType": fg.CraftType = Convert.ToByte(value); break;
					case "IFF": fg.IFF = Convert.ToByte(value); break;
					case "Team": fg.Team = Convert.ToByte(value); break;
					case "AI": fg.AI = Convert.ToByte(value); break;
					case "Markings": fg.Markings = Convert.ToByte(value); break;
					case "PlayerNumber": fg.PlayerNumber = Convert.ToByte(value); break;
					case "PlayerCraft": fg.PlayerCraft = Convert.ToByte(value); break;
					case "Radio": fg.Radio = Convert.ToByte(value); break;
					case "Formation": fg.Formation = Convert.ToByte(value); break;
					case "FormDistance": fg.FormDistance = Convert.ToByte(value); break;
					case "FormLeaderDist": fg.FormLeaderDist = Convert.ToByte(value); break;
					case "Status1": fg.Status1 = Convert.ToByte(value); break;
					case "Status2": fg.Status2 = Convert.ToByte(value); break;
					case "Missile": fg.Missile = Convert.ToByte(value); break;
					case "Beam": fg.Beam = Convert.ToByte(value); break;
					case "Countermeasures": fg.Countermeasures = Convert.ToByte(value); break;
					case "ExplosionTime": fg.ExplosionTime = Convert.ToByte(value); break;
					case "Backdrop": fg.Backdrop = Convert.ToByte(value); break;
					case "GlobalCargo":
						fg.GlobalCargo = Convert.ToByte(value);
						if (fg.CraftType == 0xB7) fg.Shadow++;  // index fix
						break;
					case "GlobalSpecialCargo": fg.GlobalSpecialCargo = Convert.ToByte(value); break;
					case "ArrivalCraft1": fg.ArrivalCraft1 = Convert.ToByte(value); break;
					case "ArrivalCraft2": fg.ArrivalCraft2 = Convert.ToByte(value); break;
					case "DepartureCraft1": fg.DepartureCraft1 = Convert.ToByte(value); break;
					case "DepartureCraft2": fg.DepartureCraft2 = Convert.ToByte(value); break;
					case "ArrivalMethod1": fg.ArrivalMethod1 = Convert.ToBoolean(value); break;
					case "ArrivalMethod2": fg.ArrivalMethod2 = Convert.ToBoolean(value); break;
					case "DepartureMethod1": fg.DepartureMethod1 = Convert.ToBoolean(value); break;
					case "DepartureMethod2": fg.DepartureMethod2 = Convert.ToBoolean(value); break;
					case "ArrDepTrigger":
						trig = getTriggerFromControls(cboADTrigAmount, cboADTrigType, cboADTrigVar, cboADTrig, cboADPara, numADPara);
						fg.ArrDepTriggers[_activeArrDepTrigger] = trig;     // trig needed for refresh at end
						break;
					case "ArrivalDelayMinutes": fg.ArrivalDelayMinutes = Convert.ToByte(value); break;
					case "ArrivalDelaySeconds": fg.ArrivalDelaySeconds = Convert.ToByte(value); break;
					case "AbortTrigger": fg.AbortTrigger = Convert.ToByte(value); break;
					case "DepartureTimerMinutes": fg.DepartureTimerMinutes = Convert.ToByte(value); break;
					case "DepartureTimerSeconds": fg.DepartureTimerSeconds = Convert.ToByte(value); break;
					case "Difficulty": fg.Difficulty = Convert.ToByte(value); break;
					case "ArriveOnlyIfHuman": fg.ArriveOnlyIfHuman = Convert.ToBoolean(value); break;
					case "GoalTriggerInc": fg.Goals[_activeFGGoal].IncompleteText = (string)value; break;
					case "GoalTriggerComp": fg.Goals[_activeFGGoal].CompleteText = (string)value; break;
					case "GoalTriggerFail": fg.Goals[_activeFGGoal].FailedText = (string)value; break;
					case "GoalTriggerAmount": fg.Goals[_activeFGGoal].Amount = Convert.ToByte(value); break;
					case "GoalTriggerArgument": fg.Goals[_activeFGGoal].Argument = Convert.ToByte(value); break;
					case "GoalTriggerCondition": fg.Goals[_activeFGGoal].Condition = Convert.ToByte(value); break;
					case "GoalTriggerParam": fg.Goals[_activeFGGoal].Parameter = Convert.ToByte(value); break;  // also handles time limit
					case "GoalTriggerSequence": fg.Goals[_activeFGGoal].ActiveSequence = Convert.ToByte(value); break;
					case "GoalTriggerPoints":  fg.Goals[_activeFGGoal].Points = Convert.ToInt16(value); break;
					case "GoalTriggerUnk42": fg.Goals[_activeFGGoal].Unknown42 = Convert.ToByte(value); break;
					case "GoalTriggerEnabled": fg.Goals[_activeFGGoal].Enabled = Convert.ToBoolean(value); break;
					case "WPRegion1": fg.Waypoints[0].Region = Convert.ToByte((int)value - 1); break;
					case "WPRegion2": fg.Waypoints[1].Region = Convert.ToByte((int)value - 1); break;
					case "WPRegion3": fg.Waypoints[2].Region = Convert.ToByte((int)value - 1); break;
					case "WPRegion4": fg.Waypoints[3].Region = Convert.ToByte((int)value - 1); break;
					case "Pitch": fg.Pitch = Convert.ToInt16(value); break;
					case "Yaw": fg.Yaw = Convert.ToInt16(value); break;
					case "Roll": fg.Roll = Convert.ToInt16(value); break;
					case "OrderCommand": fg.Orders[orderReg, _activeOrder].Command = Convert.ToByte(value); break;
					case "OrderCustom": fg.Orders[orderReg, _activeOrder].CustomText = (string)value; break;
					case "OrderThrottle": fg.Orders[orderReg, _activeOrder].Throttle = Convert.ToByte(value); break;
					case "OrderSpeed": fg.Orders[orderReg, _activeOrder].Speed = Convert.ToByte(value); break;
					case "OrderVar1": fg.Orders[orderReg, _activeOrder].Variable1 = Convert.ToByte(value); break;
					case "OrderVar2": fg.Orders[orderReg, _activeOrder].Variable2 = Convert.ToByte(value); break;
					case "OrderVar3": fg.Orders[orderReg, _activeOrder].Variable3 = Convert.ToByte(value); break;
					case "OrderTarget1": fg.Orders[orderReg, _activeOrder].Target1 = Convert.ToByte(value); break;
					case "OrderTarget2": fg.Orders[orderReg, _activeOrder].Target2 = Convert.ToByte(value); break;
					case "OrderTarget3": fg.Orders[orderReg, _activeOrder].Target3 = Convert.ToByte(value); break;
					case "OrderTarget4": fg.Orders[orderReg, _activeOrder].Target4 = Convert.ToByte(value); break;
					case "OrderTarget1Type": fg.Orders[orderReg, _activeOrder].Target1Type = Convert.ToByte(value); break;
					case "OrderTarget2Type": fg.Orders[orderReg, _activeOrder].Target2Type = Convert.ToByte(value); break;
					case "OrderTarget3Type": fg.Orders[orderReg, _activeOrder].Target3Type = Convert.ToByte(value); break;
					case "OrderTarget4Type": fg.Orders[orderReg, _activeOrder].Target4Type = Convert.ToByte(value); break;
					case "Order12Or": fg.Orders[orderReg, _activeOrder].T1AndOrT2 = Convert.ToBoolean(value); break;
					case "Order34Or": fg.Orders[orderReg, _activeOrder].T3AndOrT4 = Convert.ToBoolean(value); break;
					case "OptCategory": fg.OptCraftCategory = (FlightGroup.OptionalCraftCategory)Convert.ToByte(value); break;
					case "OptNumWaves": fg.OptCraft[_activeOptionCraft].NumberOfWaves = Convert.ToByte((int)value - 1); break;
					case "OptNumCraft": fg.OptCraft[_activeOptionCraft].NumberOfCraft = Convert.ToByte(value); break;  // Take value exactly as is.
					case "OptCraftType":
						fg.OptCraft[_activeOptionCraft].CraftType = Convert.ToByte(value);
						if (fg.OptCraft[_activeOptionCraft].NumberOfCraft < 1) fg.OptCraft[_activeOptionCraft].NumberOfCraft = 1;
						break;
					case "Role1Team": fg.EnableDesignation1 = Convert.ToByte((int)value == 0 ? 255 : (int)value - 1); break;
					case "Role2Team": fg.EnableDesignation2 = Convert.ToByte((int)value == 0 ? 255 : (int)value - 1); break;
					case "Role1": fg.Designation1 = Convert.ToByte(value); break;
					case "Role2": fg.Designation2 = Convert.ToByte(value); break;
					case "RoleText": fg.Role = (string)value; break;
					case "PilotID": fg.PilotID  = (string)value; break;
					case "SkipTrigger":
						trig = getTriggerFromControls(cboSkipAmount, cboSkipType, cboSkipVar, cboSkipTrig, cboSkipPara, numSkipPara);
						fg.Orders[cboSkipOrder.SelectedIndex / 4, cboSkipOrder.SelectedIndex % 4].SkipTriggers[_activeSkipTrigger] = trig;  // trig needed for refresh at end
						break;
					case "SkipTriggerOr": fg.Orders[cboSkipOrder.SelectedIndex / 4, cboSkipOrder.SelectedIndex % 4].SkipT1AndOrT2 = Convert.ToBoolean(value); break;
					case "Unk15": fg.Goals[(int)numUnkGoal.Value - 1].Unknown15 = Convert.ToBoolean(value); break;
					default: throw new ArgumentException("Unhandled multi-edit property: " + name);
				}
				if(refreshType.HasFlag(MultiEditRefreshType.CraftCount)) craftStart(fg, true);
			}
			if (refreshType.HasFlag(MultiEditRefreshType.ArrDepLabel) && trig != null) labelRefresh(trig, lblADTrig[_activeArrDepTrigger]);
			if (refreshType.HasFlag(MultiEditRefreshType.SkipLabel))
			{
				if(trig != null) labelRefresh(trig, _activeSkipTrigger == 0 ? lblSkipTrig1 : lblSkipTrig2);
				refreshSkipIndicators();
			}
		}
		void moveFlightgroups(int direction)
		{
			List<int> selection = Common.GetSelectedIndices(lstFG);
			if(selection.Count == 0 || (direction == -1 && selection[0] == 0) || (direction == 1 && selection[selection.Count - 1] == lstFG.Items.Count - 1))
				return;

			for (int i = 0; i < selection.Count; i++)
			{
				// Traverse the selection list forward if moving up, backward if moving down.
				int accessIndex = ((direction == -1) ? i : selection.Count - 1 - i);
				int fgIndex = selection[accessIndex];
				_mission.SwapFG(fgIndex, fgIndex + direction);
				replaceClipboardReference(fgIndex, fgIndex + direction, true);
				// Updating the map after each item is slow. Reimport the entire list after we're done.
				listRefreshItem(fgIndex + direction, false);
				listRefreshItem(fgIndex, false);
				selection[accessIndex] += direction;  // Adjust indices to new positions
			}
			Common.SetSelectedIndices(lstFG, selection, ref _noRefresh);  // Apply adjusted indices

			_fBrief?.Close();
			refreshMap(-1);
			updateFGList();
			Common.Title(this, false);
			Common.UpdateMoveButtons(cmdMoveFGUp, cmdMoveFGDown, lstFG);
		}
		void listRefreshItem(int index, bool mapUpdate = true)
		{
			bool btemp = _noRefresh;
			_noRefresh = true;                      // Modifying an item will invoke SelectedIndexChanged.
			bool state = lstFG.GetSelected(index);  // It may also interfere with the current selection state.
			lstFG.Items[index] = _mission.FlightGroups[index].ToString(true);
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
		bool newFG()
		{
			if (_mission.FlightGroups.Count == Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			_activeFG = _mission.FlightGroups.Add();
			_mission.FlightGroups[_activeFG].CraftType = _config.XwaCraft;
			_mission.FlightGroups[_activeFG].IFF = _config.XwaIff;
			craftStart(_mission.FlightGroups[_activeFG], true);
			lstFG.Items.Add(_mission.FlightGroups[_activeFG].ToString(true));
			updateFGList();
			_loading = true;
			lstFG.ClearSelected();
			lstFG.SelectedIndex = _activeFG;
			_loading = false;
			Common.Title(this, _loading);
			refreshMap(-1);
			return true;
		}
		/// <summary>Scans all Flight Groups to detect duplicate names, to provide helpful craft numbering within the editor so that the user can easily tell duplicates apart in triggers.</summary>
		void recalculateEditorCraftNumber()
		{
			// Note: changing an item in lstFG will activate lstFG_SelectedIndexChanged, which changes _activeFG and potentially cause bugs elsewhere. So need to restore before exiting the function.
			int currentFG = _activeFG;

			//A-W Red and X-W Red should not be considered duplicates, so this structure maps a CraftType to a sub-dictionary of CraftName and Count.  
			//Due to the complexity and careful error checking involved (throwing exceptions is incredibly slow), two separate functions are provided to manipulate and access them.
			Dictionary<int, Dictionary<string, int>> dupeCount = new Dictionary<int, Dictionary<string, int>>();
			Dictionary<int, Dictionary<string, int>> nameCount = new Dictionary<int, Dictionary<string, int>>();
			Dictionary<int, Dictionary<string, int>> craftCount = new Dictionary<int, Dictionary<string, int>>();

			foreach (var fg in _mission.FlightGroups)     //Need to know the duplicates ahead of time, so that even the first craft encountered can be numbered accordingly.
				Common.AddFGCounter(fg.CraftType, fg.IFF, fg.Name, 1, dupeCount);

			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				FlightGroup fg = _mission.FlightGroups[i];
				//bool isDupe = (Common.GetFGCounter(fg.CraftType, fg.IFF, fg.Name, dupeCount) >= 2);

				int current;
				if (fg.GlobalNumbering)
					current = Common.AddFGCounter(fg.CraftType, fg.IFF, fg.Name, 1, nameCount);
				else
					current = Common.GetFGCounter(fg.CraftType, fg.IFF, fg.Name, craftCount) + 1;

				if (Common.GetFGCounter(fg.CraftType, fg.IFF, fg.Name, dupeCount) <= 1)
					current = 0;

				bool change = false;
				if (fg.EditorCraftNumber != current)
				{
					fg.EditorCraftNumber = current;
					change = true;
				}
				if (fg.EditorCraftExplicit != !fg.GlobalNumbering)
				{
					fg.EditorCraftExplicit = !fg.GlobalNumbering;
					change = true;
				}

				if (!fg.GlobalNumbering)
					Common.AddFGCounter(fg.CraftType, fg.IFF, fg.Name, fg.NumberOfCraft, craftCount);

				if (change)
					listRefreshItem(i);
			}
			_activeFG = currentFG;
		}
		/// <summary>Updates the clipboard contents from containing broken indexes.</summary>
		/// <remarks>Should be called during swap or delete (<paramref name="dstIndex"/> &lt; 0) operations.</remarks>
		void replaceClipboardReference(int srcIndex, int dstIndex, bool isFG)
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
					FlightGroup fg = (FlightGroup)raw;
					if (dstIndex >= 0)
					{
						if (isFG)
						{
							change |= fg.TransformFGReferences(dstIndex, 255);
							change |= fg.TransformFGReferences(srcIndex, dstIndex);
							change |= fg.TransformFGReferences(255, srcIndex);
						}
						else
						{
							change |= fg.TransformMessageReferences(dstIndex, 255);
							change |= fg.TransformMessageReferences(srcIndex, dstIndex);
							change |= fg.TransformMessageReferences(255, srcIndex);
						}
					}
					else
					{
						if (isFG)
							change |= fg.TransformFGReferences(srcIndex, -1);
						else
							change |= fg.TransformMessageReferences(srcIndex, -1);
					}
					data.SetText(fg.ToString());
				}
				else if (raw.GetType() == typeof(Platform.Xwa.Message))
				{
					Platform.Xwa.Message mess = (Platform.Xwa.Message)raw;
					foreach (var trig in mess.Triggers)
					{
						if (isFG)
						{
							if (dstIndex >= 0)
								change |= trig.SwapFGReferences(srcIndex, dstIndex);
							else
								change |= trig.TransformFGReferences(srcIndex, dstIndex, true);
						}
						else
						{
							if (dstIndex >= 0)
								change |= trig.SwapMessageReferences(srcIndex, dstIndex);
							else
								change |= trig.TransformMessageRef(srcIndex, dstIndex);
						}
					}
					data.SetText(mess.MessageString);
				}
				else if (raw.GetType() == typeof(Mission.Trigger))
				{
					Mission.Trigger trig = (Mission.Trigger)raw;
					if (isFG)
					{
						if (dstIndex >= 0)
							change |= trig.SwapFGReferences(srcIndex, dstIndex);
						else
							change |= trig.TransformFGReferences(srcIndex, dstIndex, true);
					}
					else
					{
						if (dstIndex >= 0)
							change |= trig.SwapMessageReferences(srcIndex, dstIndex);
						else
							change |= trig.TransformMessageRef(srcIndex, dstIndex);
					}
					data.SetText(trig.ToString());
				}
				else if (raw.GetType() == typeof(FlightGroup.Order))
				{
					FlightGroup.Order order = (FlightGroup.Order)raw;
					if (isFG)
					{
						if (dstIndex >= 0)
						{
							change |= order.TransformFGReferences(dstIndex, 255);
							change |= order.TransformFGReferences(srcIndex, dstIndex);
							change |= order.TransformFGReferences(255, srcIndex);
						}
						else
						{
							change |= order.TransformFGReferences(srcIndex, -1);
						}
					}
					else
					{
						if (dstIndex >= 0)
							change |= order.SwapMessageReferences(srcIndex, dstIndex);
						else
							change |= order.TransformMessageReferences(srcIndex, -1);
					}
					data.SetText(order.ToString());
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
		void updateFGList()
		{
			//[JB] Adding this here since it's a convenient way of updating the craft numbering in any situation it may be needed.  Otherwise it would need to be added to a function and called on every major FG option (move, add, delete, rename).  Since this potentially changes multiple FG names, it needs to be called before the normal updateFGList() code.
			recalculateEditorCraftNumber();

			string[] fgList = _mission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			comboReset(cboArrMS, fgList, _mission.FlightGroups[_activeFG].ArrivalCraft1);
			comboReset(cboArrMSAlt, fgList, _mission.FlightGroups[_activeFG].ArrivalCraft2);
			comboReset(cboDepMS, fgList, _mission.FlightGroups[_activeFG].DepartureCraft1);
			comboReset(cboDepMSAlt, fgList, _mission.FlightGroups[_activeFG].DepartureCraft2);
			cboMessFG.Items.Clear(); cboMessFG.Items.AddRange(fgList);
			if (_mission.Messages.Count != 0) cboMessFG.SelectedIndex = _mission.Messages[_activeMessage].OriginatingFG;
			parameterRefresh(cboSkipPara);
			parameterRefresh(cboGoalPara);
			parameterRefresh(cboADPara);
			parameterRefresh(cboMessPara);
			parameterRefresh(cboGlobalPara);
			// Refresh trigger labels
			for (int i = 0; i < 6; i++) labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[i], lblADTrig[i]);
			lblADTrigArr_Click(lblADTrig[_activeArrDepTrigger], new EventArgs());
			byte restore = _activeOrder;
			for (_activeOrder = 0; _activeOrder < 4; _activeOrder++) orderLabelRefresh();
			lblOrderArr_Click(lblOrder[restore], new EventArgs());
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			if (o == -1) o = 0;
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[0], lblSkipTrig1);
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[1], lblSkipTrig2);
			lblSkipTrigArr_Click(_activeSkipTrigger == 0 ? lblSkipTrig1 : lblSkipTrig2, new EventArgs());
			for (int i = 0; i < 12; i++) labelRefresh(_mission.Globals[_activeTeam].Goals[i / 4].Triggers[i % 4], lblGlobTrig[i]);
			lblGlobTrigArr_Click(lblGlobTrig[_activeGlobalTrigger], new EventArgs());
			if (_mission.Messages.Count > 0)
			{
				for (int i = 0; i < 6; i++) labelRefresh(_mission.Messages[_activeMessage].Triggers[i], lblMessTrig[i]);
				lblMessTrigArr_Click(lblMessTrig[_activeMessageTrigger], new EventArgs());
			}

			_loading = temp;
			listRefreshItem(_activeFG);
		}

		void lstFG_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1 || _mission.FlightGroups[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = getFlightGroupDrawColor(e.Index);  //[JB] Moved color selection to different function so that colorized dropdowns could use it too.
			if (chkRegionFilter.Checked && _mission.FlightGroups[e.Index].Waypoints[0].Region != (short)(numFilterRegion.Value - 1))
				brText = Brushes.Gray;
			e.Graphics.DrawString(lstFG.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstFG.SelectedIndex == -1 || _noRefresh) return;
			_activeFG = lstFG.SelectedIndex;
            byte order = _activeOrder;
            lblFG.Text = "Flight Group #" + (_activeFG + (_config.OneIndexedFGs ? 1 : 0)).ToString() + " of " + _mission.FlightGroups.Count.ToString();
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
			try { cboAI.SelectedIndex = _mission.FlightGroups[_activeFG].AI; }  // for some reason, some custom missions have this as -1
			catch { cboAI.SelectedIndex = 2; _mission.FlightGroups[_activeFG].AI = 2; } // default to Veteran
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
			refreshStatus();  //Handles Status1, special case for mines.
			cboStatus2.SelectedIndex = _mission.FlightGroups[_activeFG].Status2;
			cboWarheads.SelectedIndex = _mission.FlightGroups[_activeFG].Missile;
			cboBeam.SelectedIndex = _mission.FlightGroups[_activeFG].Beam;
			cboCounter.SelectedIndex = _mission.FlightGroups[_activeFG].Countermeasures;
			numExplode.Value = _mission.FlightGroups[_activeFG].ExplosionTime;
			numBackdrop.Value = _mission.FlightGroups[_activeFG].Backdrop;
			Common.SafeSetCBO(cboGlobCargo, _mission.FlightGroups[_activeFG].GlobalCargo, true);
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
			for (int i = 0; i < 4; i++)
			{
				optADAndOr[i].Checked = _mission.FlightGroups[_activeFG].ArrDepAndOr[i];
				optADAndOr[i + 4].Checked = !optADAndOr[i].Checked;
			}
			numArrMin.Value = _mission.FlightGroups[_activeFG].ArrivalDelayMinutes;
			numArrSec.Value = _mission.FlightGroups[_activeFG].ArrivalDelaySeconds;
			numDepMin.Value = _mission.FlightGroups[_activeFG].DepartureTimerMinutes;
			numDepSec.Value = _mission.FlightGroups[_activeFG].DepartureTimerSeconds;
			cboAbort.SelectedIndex = _mission.FlightGroups[_activeFG].AbortTrigger;
			cboDiff.SelectedIndex = _mission.FlightGroups[_activeFG].Difficulty;
			chkArrHuman.Checked = _mission.FlightGroups[_activeFG].ArriveOnlyIfHuman;
			for (int i = 0; i < 6; i++) labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[i], lblADTrig[i]);
			lblADTrigArr_Click(lblADTrig[0], new EventArgs());
			#endregion
			for (_activeFGGoal = 0; _activeFGGoal < 8; _activeFGGoal++) goalLabelRefresh();
			lblGoalArr_Click(lblGoal[0], new EventArgs());
			#region Waypoints
			refreshWaypointTab();  //[JB] Code moved to separate function so that the map callback can refresh it too.
			#endregion
			for (_activeOrder = 0; _activeOrder < 4; _activeOrder++) orderLabelRefresh();
			lblOrderArr_Click(lblOrder[_config.RememberSelectedOrder ? order : 0], new EventArgs());
			#region Options
			Common.SafeSetCBO(cboRole1Teams, (_mission.FlightGroups[_activeFG].EnableDesignation1 == 255) ? 0 : _mission.FlightGroups[_activeFG].EnableDesignation1 + 1, true);
			Common.SafeSetCBO(cboRole2Teams, (_mission.FlightGroups[_activeFG].EnableDesignation2 == 255) ? 0 : _mission.FlightGroups[_activeFG].EnableDesignation2 + 1, true);
			Common.SafeSetCBO(cboRole1, (_mission.FlightGroups[_activeFG].Designation1 == 255) ? 0 : _mission.FlightGroups[_activeFG].Designation1, true);
			Common.SafeSetCBO(cboRole2, (_mission.FlightGroups[_activeFG].Designation2 == 255) ? 0 : _mission.FlightGroups[_activeFG].Designation2, true);
			cboRole1.Enabled = (_mission.FlightGroups[_activeFG].EnableDesignation1 != 255); //[JB] Added
			cboRole2.Enabled = (_mission.FlightGroups[_activeFG].EnableDesignation2 != 255);
			txtRole.Text = _mission.FlightGroups[_activeFG].Role;
			cboPilot.Text = _mission.FlightGroups[_activeFG].PilotID;
			for (int i = 0; i < 18; i++) chkOpt[i].Checked = _mission.FlightGroups[_activeFG].OptLoadout[i];
			for (_activeOptionCraft = 0; _activeOptionCraft < 10; _activeOptionCraft++) optCraftLabelRefresh();
			lblOptCraftArr_Click(lblOptCraft[0], new EventArgs());
			cboOptCat.SelectedIndex = (byte)_mission.FlightGroups[_activeFG].OptCraftCategory;
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[0], lblSkipTrig1);
			labelRefresh(_mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[1], lblSkipTrig2);
			lblSkipTrigArr_Click(lblSkipTrig1, new EventArgs());
			refreshSkipIndicators();
			optSkipOR.Checked = _mission.FlightGroups[_activeFG].Orders[r, o].SkipT1AndOrT2;
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
			enableBackdrop(_mission.FlightGroups[_activeFG].CraftType == 0xB7);
			_loading = btemp;
			Common.UpdateMoveButtons(cmdMoveFGUp, cmdMoveFGDown, lstFG);
			if (!lstFG.Focused) lstFG.Focus();  //[JB] Return control back to the list (helpful to maintain navigation using the arrow keys when certain tabs are open)
        }

		void cmdMoveFGUp_Click(object sender, EventArgs e)
		{
			moveFlightgroups(-1);
		}
		void cmdMoveFGDown_Click(object sender, EventArgs e)
		{
			moveFlightgroups(1);
		}

        void chkRegionFilter_CheckedChanged(object sender, EventArgs e)
        {
			lstFG.Invalidate();
        }
        void numFilterRegion_ValueChanged(object sender, EventArgs e)
        {
			if (chkRegionFilter.Checked) lstFG.Invalidate();
        }

        #region Craft
        void enableBackdrop(bool state)
		{
			bool btemp = _loading;
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
			_loading = true;
			if (state)
			{
				//HACK: Index fix
				int shadow = _mission.FlightGroups[_activeFG].Shadow - 1;
				if (shadow == -1) shadow = 0;
				cboGlobCargo.Items.Clear();
				cboGlobCargo.Items.Add("None (0)");
				for (int i = 1; i <= 6; i++) cboGlobCargo.Items.Add("Shadow " + i);
				try { cboGlobCargo.SelectedIndex = shadow; }
				catch { cboGlobCargo.SelectedIndex = 0; }
				lblGC.Text = "Shadow";
				lblCargo.Text = "Brightness";
				lblSC.Text = "Size";
				lblName.Text = "R G B";
				lblNotUsed.Visible = !state;
				txtSpecCargo.Visible = state;
			}
			else
			{
				int cargo = _mission.FlightGroups[_activeFG].GlobalCargo;
				cboGlobCargo.Items.Clear();
				cboGlobCargo.Items.Add("None");
				for (int i = 1; i <= 16; i++) cboGlobCargo.Items.Add("Global Cargo " + i);
				try { cboGlobCargo.SelectedIndex = cargo; }
				catch { cboGlobCargo.SelectedIndex = 0; }
				lblGC.Text = "Global Cargo";
				lblCargo.Text = "Cargo";
				lblSC.Text = "Special Cargo";      //[JB] Fixed typo
				lblName.Text = "Name";
				numSC_ValueChanged("EnableBackdrop", new EventArgs());
			}
			_loading = btemp;
		}
		void refreshStatus()
		{
			cboStatus.Items.Clear();
			bool isMine = (_mission.FlightGroups[_activeFG].CraftType >= 0x4B && _mission.FlightGroups[_activeFG].CraftType <= 0x4D);
			lblStatus.Text = isMine ? "Mine Formation" : "Status";
			cboStatus.Items.AddRange(isMine ? Strings.FormationMine : Strings.Status);
			Common.SafeSetCBO(cboStatus, isMine ? _mission.FlightGroups[_activeFG].Status1 & 3 : _mission.FlightGroups[_activeFG].Status1, true);
			cboFormation.Enabled = !isMine;
		}

		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			enableBackdrop(cboCraft.SelectedIndex == 0xB7);
			refreshStatus();
		}
		void chkRandSC_CheckedChanged(object sender, EventArgs e)
		{
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
			cmdBackdrop.Text = "Loading...";
			cmdBackdrop.Enabled = false;
			try
			{
				BackdropDialog dlg = null;
				// HACK: backdrop index fix
				if (_hookBackdropInstalled) dlg = new BackdropDialog(_mission.FlightGroups[_activeFG].Backdrop, _mission.FlightGroups[_activeFG].GlobalCargo - 1, _mission.MissionFileName, _config);
				else dlg = new BackdropDialog(_mission.FlightGroups[_activeFG].Backdrop, _mission.FlightGroups[_activeFG].GlobalCargo - 1, _config);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					cboGlobCargo.SelectedIndex = dlg.Shadow;
					numBackdrop.Value = dlg.BackdropIndex;
					System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					Stream stream = new MemoryStream();
					DataObject data = new DataObject();
					formatter.Serialize(stream, dlg.Color);
					data.SetText(dlg.Color);
					data.SetData("yogeme", false, stream);
					Clipboard.SetDataObject(data, true);
				}
			}
			catch (Exception x)  //[JB] Catch all exceptions.
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			cmdBackdrop.Enabled = true;
			cmdBackdrop.Text = "Backdrops";
		}
		void cmdForms_Click(object sender, EventArgs e)
		{
			FormationDialog dlg = new FormationDialog(_mission.FlightGroups[_activeFG].Formation, _mission.FlightGroups[_activeFG].FormDistance, Settings.Platform.XWA);
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				cboFormation.SelectedIndex = dlg.Formation;
				numSpacing.Value = dlg.Spacing;
			}
		}
		void numGG_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				numGG_Leave("numGG_KeyDown", new EventArgs());
			}
		}
		void numGG_Leave(object sender, EventArgs e)
		{
			if (_mission.FlightGroups[_activeFG].GlobalGroup != Convert.ToByte(numGG.Value) && updateGG(false))
			{

				DialogResult res = MessageBox.Show("Global Group is unique and referenced. Update references to new number?", "Update Reference?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.Yes) updateGG(true);
			}
			foreach(FlightGroup fg in getSelectedFlightgroups())
				fg.GlobalGroup = Common.Update(this, fg.GlobalGroup, Convert.ToByte(numGG.Value));
			listRefreshSelectedItems();
		}
		void numGU_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				numGU_Leave("numGU_KeyDown", new EventArgs());
			}
		}
		void numGU_Leave(object sender, EventArgs e)
		{
			if (_mission.FlightGroups[_activeFG].GlobalUnit != Convert.ToByte(numGU.Value) && updateGU(false))
			{

				DialogResult res = MessageBox.Show("Global Unit is unique and referenced. Update references to new number?", "Update Reference?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.Yes) updateGU(true);
			}
			foreach(FlightGroup fg in getSelectedFlightgroups())
				fg.GlobalUnit = Common.Update(this, fg.GlobalUnit, Convert.ToByte(numGU.Value));
			listRefreshSelectedItems();
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
		#region Arr/Dep
		void lblADTrigArr_Click(object sender, EventArgs e)
		{
			//[JB] Added try/catch block from XvtForm.  This fixes a bug (failed due to exception) that prevented labels from updating after a paste operation.
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeArrDepTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeArrDepTrigger = Convert.ToByte(sender); l = lblADTrig[_activeArrDepTrigger]; }
			setInteractiveLabelColor(l, true); //[JB] Moved colors to function.
			for (int i = 0; i < 6; i++) if (i != _activeArrDepTrigger) setInteractiveLabelColor(lblADTrig[i], false);
			bool btemp = _loading;
			_loading = true;
			cboADTrig.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Condition;
			cboADTrigType.SelectedIndex = -1;
			cboADTrigType.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType;
			cboADTrigAmount.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Amount;
			//[JB] Fixes exceptions for backdrop FGs in B4M2B.TIE, B4M3FB.TIE, B4M4B.TIE which use have Parameter1 values around 78 to 80
			Common.SafeSetCBO(cboADPara, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Parameter1, true);
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
			foreach(FlightGroup fg in getSelectedFlightgroups())
				fg.ArrDepAndOr[(int)r.Tag] = Common.Update(this, fg.ArrDepAndOr[(int)r.Tag], r.Checked);
		}
		void cboADTrig_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboADTrig.SelectedIndex == -1) return;
			comboProxCheck(cboADTrig.SelectedIndex, cboADTrigAmount);
		}
		void cboADTrigType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboADTrigType.SelectedIndex == -1) return;
			comboVarRefresh(cboADTrigType.SelectedIndex, cboADTrigVar);
			try { cboADTrigVar.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable; }
			catch { cboADTrigVar.SelectedIndex = 0; }
		}

		void cmdCopyAD_Click(object sender, EventArgs e)
		{
			menuCopy_Click("AD", new EventArgs());
		}
		void cmdPasteAD_Click(object sender, EventArgs e)
		{
			menuPaste_Click("AD", new EventArgs());
		}

		void cmdMissionCraft_Click(object sender, EventArgs e)
		{
			// TODO: the error checking should be better
			FlightGroup thisFG = _mission.FlightGroups[_activeFG];

			int warning = 0;
			for (int i = 0; i < 2; i++)
			{
				Mission.Trigger trig = thisFG.ArrDepTriggers[i];
				if (trig.Amount != 0 || trig.VariableType > 1 || trig.Variable != 0 || trig.Condition != 0)
					warning++;
			}

			FlightGroup playerFG = null;
			foreach (FlightGroup fg in _mission.FlightGroups)
			{
				if (fg.PlayerNumber > 0)
				{
					playerFG = fg;
					break;
				}
			}
			if (playerFG != null)
				if (playerFG.Team != thisFG.Team)
					warning++;

			bool proceed = true;
			if (warning > 0)
			{
				string msg = "For a craft to appear in the Mission Craft List prior to the briefing, its first two arrival triggers must be repaired.\r\n" +
					"\r\n" +
					"Set to Always(TRUE)\r\n" +
					"Targeting the first Flight Group slot in the mission.\r\n" +
					"\r\n" +
					"Additionally, this craft must be on the same team as the player, and non-stationary.\r\n" +
					"\r\n" +
					"Are you sure you want to replace the arrival triggers?";
				DialogResult res = MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo);
				if (res != DialogResult.Yes)
					proceed = false;
			}
			if (proceed)
			{
				for (int i = 0; i < 2; i++)
				{
					Mission.Trigger trig = thisFG.ArrDepTriggers[i];
					trig.Amount = 0;
					trig.VariableType = 1;
					trig.Variable = 0;
					trig.Condition = 0;
					labelRefresh(trig, lblADTrig[i]);
				}
			}
		}
		void optArrMS_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMS.Enabled = optArrMS.Checked;
		}
		void optArrMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMSAlt.Enabled = optArrMSAlt.Checked;
		}
		void optDepMS_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMS.Enabled = optDepMS.Checked;
		}
		void optDepMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMSAlt.Enabled = optDepMSAlt.Checked;
		}

		#endregion
		#region Orders
		void orderLabelRefresh()
		{
			FlightGroup.Order order = _mission.FlightGroups[_activeFG].Orders[(int)numORegion.Value - 1, _activeOrder];
			string orderText = order.ToString();
			orderText = replaceTargetText(orderText);
			string indicator = "";  //[JB] Added a visual indication if the trigger is potentially being used as a skip trigger
			if (order.IsOrderUsed()) indicator = "XX ";
			else if (order.IsSkipTriggerBroken()) indicator = "?? ";
			else if (order.IsSkipTriggerActive()) indicator = ">> ";
			if (order.Command == 0x12 && order.Variable2 >= 1 && order.Variable2 <= _mission.FlightGroups.Count)  //[JB] Display flight group for Drop Off command.  Var is one-based.
				orderText += ", " + _mission.FlightGroups[order.Variable2 - 1].ToString(false);
			lblOrder[_activeOrder].Text = "Order " + (_activeOrder + 1) + ": " + indicator + orderText;
		}

		void lblOrderArr_Click(object sender, EventArgs e)
		{
            //[JB] Added catch from XvT. Fixes bug where the labels wouldn't refresh after double click paste.
            Label l;
			try { l = (Label)sender; }
			catch { _activeOrder = Convert.ToByte(sender); l = lblOrder[_activeOrder]; /*l = lblOrder[(int)sender];*/ }
            l.Focus();
			_activeOrder = Convert.ToByte(l.Tag);
            FlightGroup.Order order = _mission.FlightGroups[_activeFG].Orders[(int)numORegion.Value - 1, _activeOrder];
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 4; i++) if (i != _activeOrder) setInteractiveLabelColor(lblOrder[i], false);
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
			cboOSpeed.SelectedIndex = order.Speed;
			txtOString.Text = order.CustomText;
			_loading = btemp;
        }
		void lblOrderArr_DoubleClick(object sender, EventArgs e)
		{
			if (((MouseEventArgs)e).Button != MouseButtons.Left) return;  //[JB] Fixed to help prevent rapid right clicks from triggering paste.  Seemed to affect this frequently, despite other controls with similar events not suffering the same behavior.
			menuPaste_Click("Order", new EventArgs());
		}
		void lblOrderArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Order", new EventArgs());
		}

		void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
		{
			string[] s = Strings.OrderDesc[cboOrders.SelectedIndex].Split('|');
			lblODesc.Text = s[0];
			lblOVar1.Text = s[1];
			lblOVar2.Text = s[2];
			lblOVar3.Text = s[3];
			//lblOV1Meaning.Visible = (cboOrders.SelectedIndex == 10);
			numOVar1_ValueChanged(0, new EventArgs()); // Force refresh, since label information is provided to the user.
			numOVar2_ValueChanged(0, new EventArgs());
		}
		void cboOT1Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT1Type.SelectedIndex == -1) return;
			comboVarRefresh(cboOT1Type.SelectedIndex, cboOT1);
			try { cboOT1.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[(int)(numORegion.Value - 1), _activeOrder].Target1; }
			catch { cboOT1.SelectedIndex = 0; }
		}
		void cboOT2Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT2Type.SelectedIndex == -1) return;
			comboVarRefresh(cboOT2Type.SelectedIndex, cboOT2);
			try { cboOT2.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[(int)(numORegion.Value - 1), _activeOrder].Target2; }
			catch { cboOT2.SelectedIndex = 0; }
		}
		void cboOT3Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT3Type.SelectedIndex == -1) return;
			comboVarRefresh(cboOT3Type.SelectedIndex, cboOT3);
			try { cboOT3.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[(int)(numORegion.Value - 1), _activeOrder].Target3; }
			catch { cboOT3.SelectedIndex = 0; }
		}
		void cboOT4Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT4Type.SelectedIndex == -1) return;
			comboVarRefresh(cboOT4Type.SelectedIndex, cboOT4);
			try { cboOT4.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[(int)(numORegion.Value - 1), _activeOrder].Target4; }
			catch { cboOT4.SelectedIndex = 0; }
		}

		void cmdCopyOrder_Click(object sender, EventArgs e)
		{
			menuCopy_Click("Order", new EventArgs());
		}
		void cmdPasteOrder_Click(object sender, EventArgs e)
		{
			menuPaste_Click("Order", new EventArgs());
		}

		void numORegion_ValueChanged(object sender, EventArgs e)
		{
            byte order = _activeOrder;
            for (_activeOrder = 0; _activeOrder < 4; _activeOrder++) orderLabelRefresh();
			lblOrderArr_Click(_config.RememberSelectedOrder ? order : 0, new EventArgs());
		}
		void cboOSpeed_SelectedIndexChanged(object sender, EventArgs e)
		{
			lblOSpeedNote.Visible = (cboOSpeed.SelectedIndex > 0);
		}
		void numOVar1_ValueChanged(object sender, EventArgs e)
		{
			byte value = (byte)numOVar1.Value;
			int command = cboOrders.SelectedIndex;
			string text = "";
			bool warning = false;
			switch (command)
			{
				case 0x0C:
				case 0x0D:
				case 0x0E:
				case 0x0F:
				case 0x10:
				case 0x11: //Board and Give, Take, Exchange, Capture, Destroy Cargo, Pick Up
				case 0x13:
				case 0x14:
				case 0x1C:
				case 0x1E:
				case 0x1F:
				case 0x20: //Wait, SS Wait, SS Hold Steady, SS Wait, SS Board, Board to Repair
				case 0x24:
				case 0x2D:
				case 0x3A:
				case 0x39:
				case 0x3D: //Self-Destruct, Repair Self, Board to Defuse, Park At, Work On
					text = Common.GetFormattedTime(_mission.GetDelaySeconds(value), true);
					break;
				case 0x02:
				case 0x03:
				case 0x15:  //Circle, Circle and Evade, SS Patrol Loop.
					if (value == 0) { text = "Zero, no loops!"; warning = true; }
					break;
				case 0x32:  //Hyper to Region
					text = "Region #" + (value + 1);
					orderLabelRefresh(); // Special case, refreshes the region name directly in the order string.
					break;
				case 0xA:   //Escort
					if (value < 9) text = "Above";
					else if (value > 17) text = "Below";
					if (value % 3 == 0) text += " Left";
					else if (value % 3 == 2) text += " Right";
					if (value == 13 || value == 27) text = "Coincident";    // the only one that won't be caught otherwise
					value = (byte)(value % 9);
					if (value < 3 && numOVar1.Value != 27) text = "Leading " + text;
					else if (value > 5) text = "Trailing " + text;
					text = text.Replace("  ", " ");
					if (numOVar1.Value > 27) text = "Invalid";
					break;
			}
			lblOVar1Note.Text = text;
			lblOVar1Note.Visible = (text != "");
			lblOVar1Note.ForeColor = warning ? Color.Red : SystemColors.ControlText;
		}
		void numOVar2_ValueChanged(object sender, EventArgs e)
		{
			int command = cboOrders.SelectedIndex;
			int var = (int)numOVar2.Value;
			string text = "";
			bool warning = false;
			switch (command)
			{
				case 0x0C:
				case 0x0D:
				case 0x0E:
				case 0x0F:
				case 0x10:  //Board to Give Cargo, Take, Exchange, Capture, Destroy
				case 0x11:
				case 0x1F:
				case 0x20: //Pick Up, SS Board, Board to Repair
					warning = (var == 0);
					if (var == 0) text = "No dockings.";
					break;
				case 0x12:  //Drop Off
					if (var >= 1 && var <= _mission.FlightGroups.Count)   //Variable is FG #, one based.
					{
						text = _mission.FlightGroups[var - 1].ToString(false);
					}
					else
					{
						text = "None specified.";
						warning = true;
					}
					if (ActiveControl == numOVar2)
						orderLabelRefresh(); //Instant update the order.
					break;
			}
			lblOVar2Note.Text = text;
			lblOVar2Note.Visible = (text != "");
			lblOVar2Note.ForeColor = warning ? Color.Red : SystemColors.ControlText;
		}
		#endregion
		#region Goals
		void goalLabelRefresh()
		{
			string triggerText = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].ToString();
			triggerText = replaceTargetText(triggerText);
			lblGoal[_activeFGGoal].Text = "Goal " + (_activeFGGoal + 1) + ": " + triggerText;
		}
		void updateFGGoalParam()
		{
			int cond = cboGoalTrigger.SelectedIndex;
			if (cond == 0x2F || cond == 0x30 || cond == 0x31 || cond == 0x32) //Arrived in Region, Departed Region, nearby/proximity to, NOT nearby/proximity to
			{
				cboGoalPara.Visible = true;
				numGoalTimeLimit.Visible = false;

				lblGoalTimeLimit.Text = "Goal time limit ignored for this condition.";
				lblGoalTimeLimitSec.Visible = false;

				lblGoalTimeLimitNote.Text = "Note: Time limit shares the same variable as region. It isn't active, but the limit will appear on in-game goal text. Use the strings above to avoid this.";
				lblGoalTimeLimitNote.Visible = true;
			}
			else
			{
				cboGoalPara.Visible = false;
				numGoalTimeLimit.Visible = true;

				int sec = _mission.GetDelaySeconds((byte)numGoalTimeLimit.Value);
				lblGoalTimeLimit.Text = "Time Limit:";
				lblGoalTimeLimitSec.Visible = true;
				lblGoalTimeLimitSec.Text = (sec == 0 ? "No time limit" : "< " + Common.GetFormattedTime(sec, false));

				lblGoalTimeLimitNote.Text = "Note: The time limit displayed in-game is incorrect." + Environment.NewLine + "Consider using the above strings instead.";
				lblGoalTimeLimitNote.Visible = (sec > 0);
			}
		}

		void lblGoalArr_Click(object sender, EventArgs e)
		{
			//[JB] Fixes refresh on paste.  Copied from XvT code.
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeFGGoal = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeFGGoal = Convert.ToByte(sender); l = lblGoal[_activeFGGoal]; }
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 8; i++) if (i != _activeFGGoal) setInteractiveLabelColor(lblGoal[i], false);
			bool btemp = _loading;
			_loading = true;
			cboGoalArgument.SelectedIndex = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Argument;
			cboGoalTrigger.SelectedIndex = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Condition;
			cboGoalAmount.SelectedIndex = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Amount;
			numGoalPoints.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Points;
			chkGoalEnable.Checked = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Enabled;
			numGoalTeam.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Team + 1;
			Common.SafeSetCBO(cboGoalPara, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Parameter, false);
			numGoalTimeLimit.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Parameter;
			numGoalActSeq.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].ActiveSequence;
			txtGoalInc.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].IncompleteText;
			txtGoalComp.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].CompleteText;
			txtGoalFail.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].FailedText;
			numUnk42.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Unknown42;
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
			updateFGGoalParam();
		}
		void cboGoalTrigger_SelectedIndexChanged(object sender, EventArgs e)
		{
			comboProxCheck(cboGoalTrigger.SelectedIndex, cboGoalAmount);
			updateFGGoalParam();
			if (!_loading) chkGoalEnable.Checked = cboGoalTrigger.SelectedIndex != 10;
		}
		void numGoalTeam_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveControl == numGoalTeam)  // Only update the checkbox if manually changed.
			{
				foreach (FlightGroup fg in getSelectedFlightgroups())
					fg.Goals[_activeFGGoal].Team = Common.Update(this, fg.Goals[_activeFGGoal].Team, Convert.ToByte(numGoalTeam.Value - 1));
				int cond = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Condition;  //Always (TRUE) or None (FALSE) shouldn't Enable
				bool status = (cond != 0 && cond != 10 && numGoalTeam.Value == 1);
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Goals[_activeFGGoal].Enabled = Common.Update(this, fg.Goals[_activeFGGoal].Enabled, status);  //[JB] Automatically adjust the Enabled check
				chkGoalEnable.Checked = status;
			}
		}
		void numGoalTimeLimit_ValueChanged(object sender, EventArgs e)
		{
			updateFGGoalParam();
		}
		#endregion
		#region Waypoints
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
		void refreshWaypointTab()
		{
			bool btemp = _loading;
			_loading = true;
			int oldSelection = (int)numWPOrder.Value;
			if (oldSelection != 4) numWPOrder.Value++;
			else numWPOrder.Value--; // force change
			numWPOrder.Value = oldSelection;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 3; j++)
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
			_loading = btemp;
		}

		void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			int index = (int)c.Tag;
			if (index < 4)
			{
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Waypoints[index].Enabled = Common.Update(this, fg.Waypoints[index].Enabled, c.Checked);
			}
			else
			{
				int order = (int)numWPOrder.Value - 1;
				int region = (int)numWPOrderRegion.Value - 1;
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Orders[region, order].Waypoints[index - 4].Enabled = Common.Update(this, fg.Orders[region, order].Waypoints[index - 4].Enabled, c.Checked);
			}
			refreshMap(_activeFG);
		}

		void cmdCopyOrderWP_Click(object sender, EventArgs e)
		{
			menuCopy_Click("OrderWP", new EventArgs());
		}
		void cmdPasteOrderWP_Click(object sender, EventArgs e)
		{
			menuPaste_Click("OrderWP", new EventArgs());
		}

		/// <remarks>This handles both numWPOrder and numWPOrderRegion</remarks>
		void numWPOrder_ValueChanged(object sender, EventArgs e)
		{
			bool btemp = _loading;
			_loading = true;
			int order = (int)numWPOrder.Value - 1;
			int region = (int)numWPOrderRegion.Value - 1;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					_tableOrderRaw.Rows[i][j] = _mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[i][j];
					_tableOrder.Rows[i][j] = Math.Round((double)_mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[i][j] / 160, 2);
				}
				chkWP[i + 4].Checked = _mission.FlightGroups[_activeFG].Orders[region, order].Waypoints[i].Enabled;
			}
			_tableOrder.AcceptChanges();
			_tableOrderRaw.AcceptChanges();
			_loading = btemp;
			refreshMap(_activeFG);
		}

		void tableWP_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;
			int i, j;
			_loading = true;
			for (j = 0; j < 4; j++) if (_tableWP.Rows[j].Equals(e.Row)) break;  //find the row index that you're changing
			for (i = 0; i < 3; i++)
			{
				if (!double.TryParse(_tableWP.Rows[j][i].ToString(), out double cell))
					_tableWP.Rows[j][i] = 0;
				short raw = (short)Math.Round(cell * 160);
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Waypoints[j][i] = Common.Update(this, fg.Waypoints[j][i], raw);
				_tableWPRaw.Rows[j][i] = raw;
			}
			_loading = false;
			refreshMap(_activeFG);
		}
		void tableWPRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;
			int i, j;
			_loading = true;
			for (j = 0; j < 4; j++) if (_tableWPRaw.Rows[j].Equals(e.Row)) break;   //find the row index that you're changing
			for (i = 0; i < 3; i++)
			{
				if (!short.TryParse(_tableWPRaw.Rows[j][i].ToString(), out short raw))
					_tableWPRaw.Rows[j][i] = 0;
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Waypoints[j][i] = Common.Update(this, fg.Waypoints[j][i], raw);
				_tableWP.Rows[j][i] = Math.Round((double)raw / 160, 2);
			}
			_loading = false;
			refreshMap(_activeFG);
		}
		void tableOrder_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;
			int i, j;
			_loading = true;
			for (j = 0; j < 8; j++) if (_tableOrder.Rows[j].Equals(e.Row)) break;   //find the row index that you're changing
			int order = (int)numWPOrder.Value - 1;
			int region = (int)numWPOrderRegion.Value - 1;
			for (i = 0; i < 3; i++)
			{
				if (!double.TryParse(_tableOrder.Rows[j][i].ToString(), out double cell))
					_tableOrder.Rows[j][i] = 0;
				short raw = (short)Math.Round(cell * 160);
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Orders[region, order].Waypoints[j][i] = Common.Update(this, fg.Orders[region, order].Waypoints[j][i], raw);
				_tableOrderRaw.Rows[j][i] = raw;
			}
			_loading = false;
			refreshMap(_activeFG);
		}
		void tableOrderRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;
			int i, j;
			_loading = true;
			for (j = 0; j < 8; j++) if (_tableOrderRaw.Rows[j].Equals(e.Row)) break;    //find the row index that you're changing
			int order = (int)numWPOrder.Value - 1;
			int region = (int)numWPOrderRegion.Value - 1;
			for (i = 0; i < 3; i++)
			{
				if (!short.TryParse(_tableOrderRaw.Rows[j][i].ToString(), out short raw))
					_tableOrderRaw.Rows[j][i] = 0;
				foreach(FlightGroup fg in getSelectedFlightgroups())
					fg.Orders[region, order].Waypoints[j][i] = Common.Update(this, fg.Orders[region, order].Waypoints[j][i], raw);
				_tableOrder.Rows[j][i] = Math.Round((double)raw / 160, 2);
			}
			_loading = false;
			refreshMap(_activeFG);
		}
		#endregion
		#region Options
		void enableOptCat(bool state)
		{
			numOptCraft.Enabled = state;
			numOptWaves.Enabled = state;
			cboOptCraft.Enabled = state;
			for (int i = 0; i < 10; i++) lblOptCraft[i].Enabled = state;
		}
		void optCraftLabelRefresh()
		{
			lblOptCraft[_activeOptionCraft].Text = "Craft " + (_activeOptionCraft + 1).ToString() + ":";
			if (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType != 0)
				lblOptCraft[_activeOptionCraft].Text += " " + (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves + 1).ToString()
				+ " x (" + (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft).ToString() + ") " + Strings.CraftType[_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType];//[JB] Fixed Strings.CraftType[cboOptCraft.SelectedIndex];   //[JB] Fix so all labels properly refresh in load time, getting the craft name directly from the setting rather than the combobox
		}
		void refreshSkipIndicators()  //[JB] Maintain order indicators when a Skip Trigger is modified.
		{
            byte restore = _activeOrder;
			//Scan through all of the orders in each region.  Determine if the Skip Trigger is potentially used.  If so update the dropdown list with an indicator, and also force a refresh on the corresponding item in the Orders tab.
			for (int o = 0; o < 4; o++)
			{
				for (int r = 0; r < 4; r++)
				{
					int index = (r * 4) + o;
					FlightGroup.Order order = _mission.FlightGroups[_activeFG].Orders[r, o];

					string old = cboSkipOrder.Items[index].ToString();
					string replace = old;

					//Remove existing indicator
					int erasePos = replace.IndexOf(" XX");
					if (erasePos == -1) erasePos = replace.IndexOf(" ??");
					if (erasePos == -1) erasePos = replace.IndexOf(" <<");
					if (erasePos >= 0)
						replace = replace.Substring(0, erasePos);

					string indicator = "";
					if (order.IsOrderUsed()) indicator = " XX";
					else if (order.IsSkipTriggerBroken()) indicator = " ??";
					else if (order.IsSkipTriggerActive()) indicator = " <<";
					replace += indicator;

					if (old != replace)
					{
						cboSkipOrder.Items[index] = replace;
						if (r == (int)(numORegion.Value - 1))
							for (int listo = 0; listo < 4; listo++)
								lblOrderArr_Click(lblOrder[listo], new EventArgs());
					}
				}
			}
			lblOrderArr_Click(lblOrder[restore], new EventArgs());  //Refreshing any of the labels will have changed the order selection, so restore it.
        }

		void chkOptArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			int i = (int)c.Tag;
			foreach(FlightGroup fg in getSelectedFlightgroups())
				fg.OptLoadout[i] = Common.Update(this, fg.OptLoadout[i], c.Checked);
			bool btemp = _loading;
			_loading = true;
			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				if (c.Checked)
				{
					if (i == 0) for (int j = 1; j < 9; j++) fg.OptLoadout[j] = false; // turn off warheads
					else if (i > 0 && i < 9) fg.OptLoadout[0] = false;        // clear warhead None
					else if (i == 9) for (int j = 10; j < 14; j++) fg.OptLoadout[j] = false;  // turn off beams
					else if (i > 9 && i < 14) fg.OptLoadout[9] = false;   // clear beam None
					else if (i == 14) for (int j = 15; j < 18; j++) fg.OptLoadout[j] = false;  // turn off CMs
					else fg.OptLoadout[14] = false;   // clear CM None
				}
				else
				{
					bool b = false;
					if (i > 0 && i < 9)
					{
						for (int j = 1; j < 9; j++) b |= fg.OptLoadout[j];
						if (!b && !chkOpt[0].Checked) fg.OptLoadout[0] = true;
					}
					b = false;
					if (i > 9 && i < 14)
					{
						for (int j = 10; j < 14; j++) b |= fg.OptLoadout[j];
						if (!b && !chkOpt[9].Checked) fg.OptLoadout[9] = true;
					}
					b = false;
					if (i > 14 && i < 18)
					{
						for (int j = 15; j < 18; j++) b |= fg.OptLoadout[j];
						if (!b && !chkOpt[14].Checked) fg.OptLoadout[14] = true;
					}
				}
			}
			for (i = 0; i < 18; i++) chkOpt[i].Checked = _mission.FlightGroups[_activeFG].OptLoadout[i];
			_loading = btemp;
		}
		void lblOptCraftArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			_activeOptionCraft = Convert.ToByte(l.Tag);
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 10; i++) if (i != _activeOptionCraft) setInteractiveLabelColor(lblOptCraft[i], false);
			bool btemp = _loading;
			_loading = true;
			cboOptCraft.SelectedIndex = _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType;

			int numCraft = _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft;
			if (numCraft < 1) numCraft = 1;
			numOptCraft.Value = numCraft; //[JB] This isn't +1 like craft waves is (appears in XvT exactly as its value), but the control only accepts a minimum of 1.
			numOptWaves.Value = _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves + 1;
			_loading = btemp;
		}
		void lblSkipTrigArr_Click(object sender, EventArgs e)
		{
			//[JB] Copied XvT code to fix updating on paste.
			//Label l = (Label)sender;
			//Label ll = (l == lblSkipTrig1 ? lblSkipTrig2 : lblSkipTrig1);
			Label l; // clicked
			Label ll; // other one
			int i;
			try
			{
				l = (Label)sender;
				l.Focus();
				ll = (l == lblSkipTrig1 ? lblSkipTrig2 : lblSkipTrig1);
				i = (l == lblSkipTrig1 ? 0 : 1);    // i = clicked trigger
			}
			catch (InvalidCastException)
			{
				i = Convert.ToByte(sender);    // i = clicked trigger from code
				if (i == 0) { l = lblSkipTrig1; ; ll = lblSkipTrig2; }
				else { l = lblSkipTrig2; ll = lblSkipTrig1; }
			}
			int r, o;
			//l.Focus();
			i = (l == lblSkipTrig1 ? 0 : 1);
			r = cboSkipOrder.SelectedIndex / 4;
			o = cboSkipOrder.SelectedIndex % 4;
			if (o == -1) o = 0;
			Mission.Trigger trigger = _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i];
			setInteractiveLabelColor(l, true);
			setInteractiveLabelColor(ll, false);
			_activeSkipTrigger = (byte)i;
			bool btemp = _loading;
			_loading = true;
			cboSkipTrig.SelectedIndex = trigger.Condition;
			cboSkipType.SelectedIndex = -1;
			cboSkipType.SelectedIndex = trigger.VariableType;
			cboSkipAmount.SelectedIndex = trigger.Amount;
			Common.SafeSetCBO(cboSkipPara, trigger.Parameter1, true);
			numSkipPara.Value = trigger.Parameter2;
			_loading = btemp;
		}
		void lblSkipTrigArr_DoubleClick(object sender, EventArgs e)
		{
			if (((MouseEventArgs)e).Button != MouseButtons.Left) return;  //[JB] Avoid pasting for rapid clicks that aren't Left.
			menuPaste_Click("Skip", new EventArgs());
		}
		void lblSkipTrigArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Skip", new EventArgs());
		}

		void cboOptCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOptCat.SelectedIndex == 4) enableOptCat(true);
			else enableOptCat(false);
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
			if (!_loading && !cboSkipOrder.Focused) cboSkipOrder.Focus();  //[JB] Return focus back to self, so it can be navigated with arrow keys without giving focus to other controls.
			refreshSkipIndicators();
		}
		void cboSkipTrig_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboSkipTrig.SelectedIndex == -1) return;
			comboProxCheck(cboSkipTrig.SelectedIndex, cboSkipAmount);
		}
		void cboSkipType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboSkipType.SelectedIndex == -1) return;
			int i = (lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			if (o == -1) o = 0;
			comboVarRefresh(cboSkipType.SelectedIndex, cboSkipVar);
			try { cboSkipVar.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[r, o].SkipTriggers[i].Variable; }
			catch { cboSkipVar.SelectedIndex = 0; }
		}
		void cmdCopySkip_Click(object sender, EventArgs e)
		{
			menuCopy_Click("Skip", new EventArgs());
		}
		void cmdPasteSkip_Click(object sender, EventArgs e)
		{
			menuPaste_Click("Skip", new EventArgs());
		}
		void cboRole1Teams_SelectedIndexChanged(object sender, EventArgs e)
		{
			cboRole1.Enabled = (cboRole1Teams.SelectedIndex != 0);
		}
		void cboRole2Teams_SelectedIndexChanged(object sender, EventArgs e)
		{
			cboRole2.Enabled = (cboRole2Teams.SelectedIndex != 0);
		}

		void numExplode_ValueChanged(object sender, EventArgs e)
		{
			lblExplode.Text = numExplode.Value != 0 ? (numExplode.Value * 5) + " sec" : "default";
		}
		#endregion
		#region Unknowns
		void cboUnkOrder_Enter(object sender, EventArgs e)
		{   // use Enter to detect when user is about to change index, save what's there
			int r = cboUnkOrder.SelectedIndex / 4;
			int o = cboUnkOrder.SelectedIndex % 4;
			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				fg.Orders[r, o].Unknown9 = Common.Update(this, fg.Orders[r, o].Unknown9, Convert.ToByte(numUnk9.Value));
				fg.Orders[r, o].Unknown10 = Common.Update(this, fg.Orders[r, o].Unknown10, Convert.ToByte(numUnk10.Value));
				fg.Orders[r, o].Unknown11 = Common.Update(this, fg.Orders[r, o].Unknown11, chkUnk11.Checked);
				fg.Orders[r, o].Unknown12 = Common.Update(this, fg.Orders[r, o].Unknown12, chkUnk12.Checked);
				fg.Orders[r, o].Unknown13 = Common.Update(this, fg.Orders[r, o].Unknown13, chkUnk13.Checked);
				fg.Orders[r, o].Unknown14 = Common.Update(this, fg.Orders[r, o].Unknown14, chkUnk14.Checked);
			}
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
		void grpUnkAD_Leave(object sender, EventArgs e)
		{
			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				fg.Unknowns.Unknown5 = Common.Update(this, fg.Unknowns.Unknown5, Convert.ToByte(numUnk5.Value));
				fg.Unknowns.Unknown6 = Common.Update(this, fg.Unknowns.Unknown6, chkUnk6.Checked);
				fg.Unknowns.Unknown7 = Common.Update(this, fg.Unknowns.Unknown7, Convert.ToByte(numUnk7.Value));
				fg.Unknowns.Unknown8 = Common.Update(this, fg.Unknowns.Unknown8, Convert.ToByte(numUnk8.Value));
			}
		}
		void grpUnkCraft_Leave(object sender, EventArgs e)
		{
			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				fg.Unknowns.Unknown1 = Common.Update(this, fg.Unknowns.Unknown1, Convert.ToByte(numUnk1.Value));
				fg.Unknowns.Unknown3 = Common.Update(this, fg.Unknowns.Unknown3, Convert.ToByte(numUnk3.Value));
				fg.Unknowns.Unknown4 = Common.Update(this, fg.Unknowns.Unknown4, Convert.ToByte(numUnk4.Value));
			}
		}
		void grpUnkOther_Leave(object sender, EventArgs e)
		{   // okay, lots of stuff :P
			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				fg.Unknowns.Unknown16 = Common.Update(this, fg.Unknowns.Unknown16, Convert.ToByte(numUnk16.Value));
				fg.Unknowns.Unknown17 = Common.Update(this, fg.Unknowns.Unknown17, Convert.ToByte(numUnk17.Value));
				fg.Unknowns.Unknown18 = Common.Update(this, fg.Unknowns.Unknown18, Convert.ToByte(numUnk18.Value));
				fg.Unknowns.Unknown19 = Common.Update(this, fg.Unknowns.Unknown19, Convert.ToByte(numUnk19.Value));
				fg.Unknowns.Unknown20 = Common.Update(this, fg.Unknowns.Unknown20, Convert.ToByte(numUnk20.Value));
				fg.Unknowns.Unknown21 = Common.Update(this, fg.Unknowns.Unknown21, Convert.ToByte(numUnk21.Value));
				fg.Unknowns.Unknown22 = Common.Update(this, fg.Unknowns.Unknown22, chkUnk22.Checked);
				fg.Unknowns.Unknown23 = Common.Update(this, fg.Unknowns.Unknown23, Convert.ToByte(numUnk23.Value));
				fg.Unknowns.Unknown24 = Common.Update(this, fg.Unknowns.Unknown24, Convert.ToByte(numUnk24.Value));
				fg.Unknowns.Unknown25 = Common.Update(this, fg.Unknowns.Unknown25, Convert.ToByte(numUnk25.Value));
				fg.Unknowns.Unknown26 = Common.Update(this, fg.Unknowns.Unknown26, Convert.ToByte(numUnk26.Value));
				fg.Unknowns.Unknown27 = Common.Update(this, fg.Unknowns.Unknown27, Convert.ToByte(numUnk27.Value));
				fg.Unknowns.Unknown28 = Common.Update(this, fg.Unknowns.Unknown28, Convert.ToByte(numUnk28.Value));
				fg.Unknowns.Unknown29 = Common.Update(this, fg.Unknowns.Unknown29, chkUnk29.Checked);
				fg.Unknowns.Unknown30 = Common.Update(this, fg.Unknowns.Unknown30, chkUnk30.Checked);
				fg.Unknowns.Unknown31 = Common.Update(this, fg.Unknowns.Unknown31, chkUnk31.Checked);
				fg.Unknowns.Unknown32 = Common.Update(this, fg.Unknowns.Unknown32, Convert.ToByte(numUnk32.Value));
				fg.Unknowns.Unknown33 = Common.Update(this, fg.Unknowns.Unknown33, Convert.ToByte(numUnk33.Value));
				fg.Unknowns.Unknown34 = Common.Update(this, fg.Unknowns.Unknown34, chkUnk34.Checked);
				fg.Unknowns.Unknown35 = Common.Update(this, fg.Unknowns.Unknown35, chkUnk35.Checked);
				fg.Unknowns.Unknown36 = Common.Update(this, fg.Unknowns.Unknown36, chkUnk36.Checked);
				fg.Unknowns.Unknown37 = Common.Update(this, fg.Unknowns.Unknown37, chkUnk37.Checked);
				fg.Unknowns.Unknown38 = Common.Update(this, fg.Unknowns.Unknown38, chkUnk38.Checked);
				fg.Unknowns.Unknown39 = Common.Update(this, fg.Unknowns.Unknown39, chkUnk39.Checked);
				fg.Unknowns.Unknown40 = Common.Update(this, fg.Unknowns.Unknown40, chkUnk40.Checked);
				fg.Unknowns.Unknown41 = Common.Update(this, fg.Unknowns.Unknown41, chkUnk41.Checked);  //[JB] Fixed, was 31.
			}
		}
		void grpUnkOrder_Leave(object sender, EventArgs e)
		{
			cboUnkOrder_Enter("grpUnkOrder_Leave", new EventArgs());    // meh, no need to type it out again
		}

		void numUnkGoal_ValueChanged(object sender, EventArgs e)
		{
			chkUnk15.Checked = _mission.FlightGroups[_activeFG].Goals[(int)numUnkGoal.Value - 1].Unknown15;
		}
		#endregion
		#endregion
		#region Messages
		void deleteMess()
		{
			List<int> selection = Common.GetSelectedIndices(lstMessages);
			if (selection.Count == 0) return;
			int startMsg = _activeMessage;
			for (int si = selection.Count - 1; si >= 0; si--)
			{
				replaceClipboardReference(selection[si], -1, false);
				_mission.Messages.RemoveAt(selection[si]);
				lstMessages.Items.RemoveAt(selection[si]);
			}
			Common.Title(this, false);
			if (_mission.Messages.Count == 0)
			{
				lstMessages.Items.Clear();
				enableMessages(false);
				lblMessage.Text = "Message #0 of 0";
				return;
			}
			if (startMsg >= _mission.Messages.Count)
				startMsg = _mission.Messages.Count - 1;
			// The numbering will be wrong, so update all messages from the deletion point onwards.
			for (int i = startMsg; i < _mission.Messages.Count; i++)
				messRefreshItem(i);
			lstMessages.SelectedIndex = startMsg;
		}
		void enableMessages(bool state)
		{
			grpMessages.Enabled = state;
			grpMessCancel.Enabled = state;
			grpMessUnk.Enabled = state;
			txtMessage.Enabled = state;
			txtMessNote.Enabled = state; //[JB] Fix so edit box is disabled with others, prevents exception when leaving focus when zero messages.
			grpSend.Enabled = state;
			numMessDelay.Enabled = state;
			numMessUnk2.Enabled = state;
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
		void messRefreshItem(int index)
		{
			bool btemp = _noRefresh;
			_noRefresh = true;                            // Modifying an item will invoke SelectedIndexChanged.
			bool state = lstMessages.GetSelected(index);  // It may also interfere with the current selection state.
			lstMessages.Items[index] = getNumberedMessage(index);
			lstMessages.SetSelected(index, state);
			_noRefresh = btemp;
		}
		void messRefreshSelectedItems()
		{
			foreach (int i in Common.GetSelectedIndices(lstMessages))
				messRefreshItem(i);
		}
		List<Platform.Xwa.Message> getSelectedMessages()
		{
			List<Platform.Xwa.Message> msgs = new List<Platform.Xwa.Message>();
			foreach (int msgIndex in lstMessages.SelectedIndices)
				msgs.Add(_mission.Messages[msgIndex]);
			return msgs;
		}
		void setMessageProperty(/*MultiEditRefreshType refreshType,*/ string name, object value)
		{
			int trigRefresh = 0;
			foreach (Platform.Xwa.Message msg in getSelectedMessages())
			{
				switch (name)
				{
					case "MessTrigger":
						Mission.Trigger trig = getTriggerFromControls(cboMessAmount, cboMessType, cboMessVar, cboMessTrig, cboMessPara, numMessPara);
						msg.Triggers[_activeMessageTrigger] = trig;  // trig needed for refresh
						if (trigRefresh++ == 0) labelRefresh(trig, lblMessTrig[_activeMessageTrigger]);  // only refresh once
						break;
					case "MessColor": msg.Color = Convert.ToByte(value); break;
					case "MessDelay": msg.Delay = Convert.ToByte(value); break;
					case "MessFG": msg.OriginatingFG = Convert.ToByte(value); break;
					case "MessUnk1": msg.Unknown1 = Convert.ToByte(value); break;
					case "MessUnk2": msg.Unknown2 = Convert.ToByte(value); break;
					case "MessUnk3": msg.Unknown3 = Convert.ToBoolean(value); break;
				}
			}
		}
		void moveMessages(int direction)
		{
			List<int> selection = Common.GetSelectedIndices(lstMessages);
			if(selection.Count == 0 || (direction == -1 && selection[0] == 0) || (direction == 1 && selection[selection.Count - 1] == lstMessages.Items.Count - 1))
				return;

			for (int i = 0; i < selection.Count; i++)
			{
				// Traverse the selection list forward if moving up, backward if moving down.
				int accessIndex = ((direction == -1) ? i : selection.Count - 1 - i);
				int msgIndex = selection[accessIndex];
				Platform.Xwa.Message tmp = _mission.Messages[msgIndex];
				_mission.Messages[msgIndex] = _mission.Messages[msgIndex + direction];
				_mission.Messages[msgIndex + direction] = tmp;
				messRefreshItem(msgIndex);
				messRefreshItem(msgIndex + direction);
				selection[accessIndex] += direction;     // Adjust indices to new positions
			}
			_activeMessage += direction;

			Common.SetSelectedIndices(lstMessages, selection, ref _noRefresh);  // Apply adjusted indices
			Common.Title(this, false);
			Common.UpdateMoveButtons(cmdMoveMessUp, cmdMoveMessDown, lstMessages);
		}
		bool newMess()
		{
			if (_mission.Messages.Count == Mission.MessageLimit)
			{
				MessageBox.Show("Mission contains maximum number of Messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			_activeMessage = _mission.Messages.Add();
			if (_mission.Messages.Count == 1) enableMessages(true);
			lstMessages.Items.Add(getNumberedMessage(_activeMessage));
			lstMessages.ClearSelected();
			lstMessages.SelectedIndex = _activeMessage;
			Common.Title(this, _loading);
			return true;
		}
		string getNumberedMessage(int index)
		{
			return (index >= 0 && index < _mission.Messages.Count) ? "#" + (index + 1) + ": " + (_mission.Messages[index].MessageString != "" ? _mission.Messages[index].MessageString : " *") : "";
		}

		void lstMessages_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (_mission.Messages.Count == 0 || _mission.Messages[e.Index] == null) return;
			e.DrawBackground();
			var mess = _mission.Messages[e.Index];
			Brush brText = SystemBrushes.ControlText;
			switch (mess.Color)
			{
				case 0:
					brText = Brushes.Lime; //LimeGreen;
					break;
				case 1:
					brText = Brushes.Red; //Crimson;
					break;
				case 2:
					brText = Brushes.DodgerBlue; //RoyalBlue;
					break;
				case 3:
					brText = Brushes.Yellow;
					break;
				case 4:
					brText = Brushes.OrangeRed; //Red;
					break;
				case 5:
					brText = Brushes.MediumOrchid; //DarkOrchid;
					break;
			}

			bool used = false;
			for (int i = 0; i < mess.SentTo.Length; i++) used |= mess.SentTo[i];
			// evaluate the FALSE conditions to detect if it's locked into NEVER
			bool[] never = new bool[6];
			for (int i = 0; i < 4; i++) never[i] = (mess.Triggers[i].Condition == 10);
			never[4] = ((never[0] || never[1]) && !mess.T1AndOrT2) || (never[0] && never[1]);	// T1/2 pair
			never[5] = ((never[2] || never[3]) && !mess.T3AndOrT4) || (never[2] && never[3]);	// T3/4 pair
			if (never[4] && never[5]) used = false;
			else if ((never[4] || never[5]) && !mess.T12AndOrT34) used = false;
			if (!used) brText = Brushes.Gray;

			e.Graphics.DrawString(lstMessages.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			Common.UpdateMoveButtons(cmdMoveMessUp, cmdMoveMessDown, lstMessages);  // Running this first lets it detect if all messages have been deselected
			if (lstMessages.SelectedIndex == -1 || _noRefresh) return;
			_activeMessage = lstMessages.SelectedIndex;
			lblMessage.Text = "Message #" + (_activeMessage + 1).ToString() + " of " + _mission.Messages.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			for (int i = 0; i < 6; i++) labelRefresh(_mission.Messages[_activeMessage].Triggers[i], lblMessTrig[i]);
			txtMessage.Text = _mission.Messages[_activeMessage].MessageString;
			txtMessNote.Text = _mission.Messages[_activeMessage].Note;
			cboMessColor.SelectedIndex = _mission.Messages[_activeMessage].Color;
			for (int i = 0; i < 4; i++)
			{
				optMessAndOr[i].Checked = _mission.Messages[_activeMessage].TrigAndOr[i];
				optMessAndOr[i + 4].Checked = !optMessAndOr[i].Checked;
			}
			numMessDelay.Value = _mission.Messages[_activeMessage].Delay;
			numMessUnk2.Value = _mission.Messages[_activeMessage].Unknown2;
			for (int i = 0; i < 10; i++) chkSendTo[i].Checked = _mission.Messages[_activeMessage].SentTo[i];
			numMessUnk1.Value = _mission.Messages[_activeMessage].Unknown1;
			chkMessUnk3.Checked = _mission.Messages[_activeMessage].Unknown3;
			txtVoice.Text = _mission.Messages[_activeMessage].VoiceID;
			cboMessFG.SelectedIndex = _mission.Messages[_activeMessage].OriginatingFG;
			lblMessTrigArr_Click(0, new EventArgs());  //[JB] Was lblMessTrig[0].  Changed to XvT code of "0").
			_loading = btemp;
		}

		void chkSendToArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			foreach(Platform.Xwa.Message msg in getSelectedMessages())
				msg.SentTo[(int)c.Tag] = Common.Update(this, msg.SentTo[(int)c.Tag], c.Checked);
		}
		void lblMessTrigArr_Click(object sender, EventArgs e)
		{
			if (_mission.Messages.Count == 0) return;  //[JB] Avoid exception if no messages.
			Label l;
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

			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 6; i++) if (i != _activeMessageTrigger) setInteractiveLabelColor(lblMessTrig[i], false);
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
			foreach(Platform.Xwa.Message msg in getSelectedMessages())
				msg.TrigAndOr[(int)r.Tag] = Common.Update(this, msg.TrigAndOr[(int)r.Tag], r.Checked);
			messRefreshSelectedItems(); // Refresh whether grayed out or not.
		}
		void cboMessTrig_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMessTrig.SelectedIndex == -1) return;
			comboProxCheck(cboMessTrig.SelectedIndex, cboMessAmount);
		}
		void cboMessType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMessType.SelectedIndex == -1) return;
			comboVarRefresh(cboMessType.SelectedIndex, cboMessVar);
			try { cboMessVar.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Variable; }
			catch { cboMessVar.SelectedIndex = 0; }
		}
		void numMessDelay_ValueChanged(object sender, EventArgs e)
		{
			lblDelay.Text = Common.GetFormattedTime(_mission.GetDelaySeconds((byte)numMessDelay.Value), true);
		}
		void txtMessage_TextChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.Messages[_activeMessage].MessageString = Common.Update(this, _mission.Messages[_activeMessage].MessageString, txtMessage.Text);
			messRefreshItem(_activeMessage);
		}
		void txtVoice_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].VoiceID = Common.Update(this, _mission.Messages[_activeMessage].VoiceID, txtVoice.Text);
		}
		void txtMessNote_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Note = Common.Update(this, _mission.Messages[_activeMessage].Note, txtMessNote.Text);
		}

		void cmdMoveMessUp_Click(object sender, EventArgs e)
		{
			moveMessages(-1);
		}
		void cmdMoveMessDown_Click(object sender, EventArgs e)
		{
			moveMessages(1);
		}
		#endregion
		#region Globals
		void cboGlobalTeam_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalTeam.SelectedIndex == -1) return;
			if (!_loading) lblGlobTrigArr_Click(lblGlobTrig[_activeTeam], new EventArgs()); // re-click current lbl with save
			_activeTeam = (byte)cboGlobalTeam.SelectedIndex;
			lblTeamArr_Click(lblTeam[_activeTeam], new EventArgs());    // link the Globals and Team tabs to share GlobTeam
			bool btemp = _loading;
			_loading = true;
			for (int i = 0; i < 12; i++) labelRefresh(_mission.Globals[_activeTeam].Goals[i / 4].Triggers[i % 4], lblGlobTrig[i]);
			for (int i = 0; i < 9; i++)
			{
				optGlobAndOr[i * 2].Checked = _mission.Globals[_activeTeam].Goals[i / 3].AndOr[i % 3];  // OR
				optGlobAndOr[i * 2 + 1].Checked = !optGlobAndOr[i * 2].Checked; // AND
			}
			lblGlobTrigArr_Click(lblGlobTrig[0], new EventArgs());  // click first lbl with no save
			_loading = btemp;
		}

		void lblGlobTrigArr_Click(object sender, EventArgs e)
		{
			//[JB] Fixes bug where Goal pasting wouldn't refresh the label.  Copied try/catch code from XvT.
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeGlobalTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeGlobalTrigger = Convert.ToByte(sender); l = lblGlobTrig[_activeGlobalTrigger]; }
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 12; i++) if (i != _activeGlobalTrigger) setInteractiveLabelColor(lblGlobTrig[i], false);
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
			if (((MouseEventArgs)e).Button != MouseButtons.Left) return;  //[JB] Sometimes rapid single clicks were tricking this, like left to select, right to copy would trigger a paste instead.
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

		void cboGlobalAmount_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!_loading)
				_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Amount = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Amount, Convert.ToByte(cboGlobalAmount.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4], lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalPara_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Parameter1 = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Parameter1, Convert.ToByte(cboGlobalPara.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4], lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalTrig_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalTrig.SelectedIndex == -1) return;
			if (!_loading)
				_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Condition = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].Condition, Convert.ToByte(cboGlobalTrig.SelectedIndex));
			comboProxCheck(cboGlobalTrig.SelectedIndex, cboGlobalAmount);
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
		void cboGlobalVar_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!_loading)
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
		void numGlobActSeq_Leave(object sender, EventArgs e)
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
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Points = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Points, (short)numGlobalPoints.Value);
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
		void teamRefresh()
		{
			string team = _mission.Teams[_activeTeam].Name;
			lblTeam[_activeTeam].Text = "Team " + (_activeTeam + 1).ToString() + ": " + team;
			if (team == "") //[JB] Replace empty string with default team name, so that the dropdown lists don't have an empty line.
				team = "Team " + (_activeTeam + 1).ToString();
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
			for (int i = 0; i < 8; i++)
				if (cboType[i].SelectedIndex == 0xC || cboType[i].SelectedIndex == 0x15)
					cbo[i].Items[_activeTeam] = team;

			//[JB] Update the Briefing's list of team names.
			for (int i = 0; i < _mission.Teams.Count; i++)
				BriefingForm.SharedTeamNames[i] = _mission.Teams[i].Name;

			if (_activeTeam < 8)
			{
				cboRole1Teams.Items[1 + _activeTeam] = team;   //Each dropdown has 8 teams beginning at index[1]
				cboRole2Teams.Items[1 + _activeTeam] = team;
			}

			if (_activeTeam >= 0 && _activeTeam < 8)       //8 teams in the Radio list beginning at index[1]
				cboRadio.Items[1 + _activeTeam] = team;
		}

		void lblTeamArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			_activeTeam = Convert.ToByte(l.Tag);
			cboGlobalTeam.SelectedIndex = _activeTeam;
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 10; i++) if (i != _activeTeam) setInteractiveLabelColor(lblTeam[i], false);
			bool btemp = _loading;
			_loading = true;
			txtTeamName.Text = _mission.Teams[_activeTeam].Name;
			for (int i = 0; i < 10; i++)
			{
				if (_mission.Teams[_activeTeam].Allies[i] == Team.Allegeance.Hostile) optAllies[i * 3].Checked = true;
				else if (_mission.Teams[_activeTeam].Allies[i] == Team.Allegeance.Friendly) optAllies[i * 3 + 1].Checked = true;
				else optAllies[i * 3 + 2].Checked = true;
			}
			txtPMCVoiceID.Text = _mission.Teams[_activeTeam].VoiceIDs[0];
			txtPMFVoiceID.Text = _mission.Teams[_activeTeam].VoiceIDs[1];
			txtOMCVoiceID.Text = _mission.Teams[_activeTeam].VoiceIDs[2];
			txtPrimCompNote.Text = _mission.Teams[_activeTeam].EomNotes[0];
			txtPrimFailNote.Text = _mission.Teams[_activeTeam].EomNotes[1];
			txtSecCompNote.Text = _mission.Teams[_activeTeam].EomNotes[2];
			for (int i = 0; i < 6; i++)
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
			Team.Allegeance a = ((index % 3) == 0 ? Team.Allegeance.Hostile : ((index % 3) == 1 ? Team.Allegeance.Friendly : Team.Allegeance.Neutral));
			_mission.Teams[_activeTeam].Allies[index / 3] = Common.Update(this, _mission.Teams[_activeTeam].Allies[index / 3], a);
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
			for (int i = 0; i < 6; i++)
				_mission.Teams[_activeTeam].Unknowns[i] = Common.Update(this, _mission.Teams[_activeTeam].Unknowns[i], Convert.ToByte(numTeamUnk[i].Value));
		}

		void txtTeamName_Leave(object sender, EventArgs e)
		{
			_mission.Teams[_activeTeam].Name = Common.Update(this, _mission.Teams[_activeTeam].Name, txtTeamName.Text);
			teamRefresh();
		}
		#endregion
		#region Mission
		void updateMissionTabs()
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
			for (int i = 0; i < 4; i++)
			{
				txtIFFs[i].Text = _mission.IFFs[i + 2];
				txtRegions[i].Text = _mission.Regions[i];
			}
			numGlobCargo.Value = 2;
			numGlobCargo.Value = 1; // force the refresh
			txtNotes.Text = _mission.MissionNotes;
			ggRefresh();
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
		void ggRefresh()
		{
			for (int i = 0; i < 16; i++)
				if (_mission.GlobalGroups[i] != "") lblGG[i].Text = _mission.GlobalGroups[i];
				else lblGG[i].Text = "Global Group " + i.ToString();
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
			setInteractiveLabelColor(l, true);
			for (i = 0; i < 16; i++) if (lblGG[i] != l) setInteractiveLabelColor(lblGG[i], false);
		}
		void txtIFFsArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.IFFs[(int)t.Tag] = Common.Update(this, _mission.IFFs[(int)t.Tag], t.Text);
			comboReset(cboIFF, getIffStrings(), cboIFF.SelectedIndex);
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

		void grpRegions_Leave(object sender, EventArgs e)
		{
			parameterRefresh(cboSkipPara);
			parameterRefresh(cboGoalPara);
			parameterRefresh(cboADPara);
			parameterRefresh(cboMessPara);
			parameterRefresh(cboGlobalPara);
		}
		void txtGlobCargo_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			_mission.GlobalCargo[gc].Cargo = Common.Update(this, _mission.GlobalCargo[gc].Cargo, txtGlobCargo.Text);
		}
		void txtGlobGroup_Leave(object sender, EventArgs e)
		{
			int i;
			for (i = 0; i < 16; i++) if (lblGG[i].ForeColor == getHighlightColor()) break;
			_mission.GlobalGroups[i] = Common.Update(this, _mission.GlobalGroups[i], txtGlobGroup.Text);
			ggRefresh();
			updateFGList(); //Force refresh of any trigger labels that might contain the GG text.
		}
		void txtNotes_Leave(object sender, EventArgs e)
		{
			_mission.MissionNotes = Common.Update(this, _mission.MissionNotes, txtNotes.Text);
		}

		#endregion
	}
}