/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2021 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.10+
 */

/* CHANGELOG
 * [UPD] Copy/paste now uses system clipboard, can CP Waypoints
 * v1.10, 210520
 * [UPD #56] Replaced try/catch with TryParse [JB]
 * v1.9.2, 210328
 * [FIX] Test load failure if mission isn't in platform directory
 * v1.9, 210108
 * [FIX] Clipboard path in some locations
 * v1.8.1, 201213
 * [UPD] _config passed to LST form, Backdrops, RunVerify()
 * [UPD #20] Test function now attempts to detect platform from MissionPath
 * [UPD] menuTest moved under Tools, changed to &Test
 * v1.8, 201004
 * [FIX] Deactivate added to force focus fix [JB]
 * [FIX] Test launching if you cancel the intial Save
 * [UPD] saveMission now won't save/rewrite file if unmodifed
 * [NEW] FlightGroupLibrary [JB]
 * [FIX] double listing of IFFs [JB]
 * [UPD] changes due to Unknown definitions: StopArrivingWhen, RndSeed, FG Goal TeamEnabled array, RandomArrDep Min/Sec [JB]
 * [UPD] colorized cbos now work with "not FG" [JB]
 * v1.7, 200816
 * [UPD] Unknowns tab cleanup
 * [FIX] recalculateEditorCraftNumbering() handles _activeFG now [JB]
 * [UPD] shiplist and Map calls updated for Wireframe implementation [JB]
 * [UPD] Blank messages now shown as "*" [JB]
 * [UPD] Cleanup index substitions [JB]
 * [UPD] Trigger label refresh updates [JB]
 * [FIX] Extra protections to handle custom missions using "bad" Status or Formation values [JB]
 * [NEW] BoP IFF names implemented (consumes Unknowns 4 and 5) [JB]
 * [NEW] More TriggerTypes added [JB]
 * [UPD] Unk6 renamed to PreventOutcome [JB]
 * [UPD] form handlers renamed
 * [FIX] re-init if load fails
 * v1.6.5, 200704
 * [UPD] More details to ProcessCraftList error message
 * [FIX #32] bin path now explicitly uses Startup Path to prevent implicit from defaulting to sys32
 * v1.6.4, 200119
 * [FIX] added Update to cmdBackdrop to ensure mission is dirtied
 * [NEW #30] Briefing callback
 * v1.5.1, 190513
 * [NEW] Changing GG value will now prompt to update references throughout if it's the only FG with that designation
 * v1.5, 180910
 * [NEW] Dictionaries for Control handling [JB]
 * [UPD] blank Team names now show generic in cbo's [JB]
 * [FIX] comboReset now has index check [JB]
 * [NEW] Xwing support [JB]
 * [FIX] non-XvT filestreams now closed properly [JB]
 * [UPD] lots of controls converted to instant-update instead of using _Leave. Done by pointing handlers to _Leave instead of redoing it [JB]
 * [UPD] order speed changed to cbo [JB]
 * [UPD] cboRoleImp/Reb renamed to 1/2 [JB]
 * [NEW] controls to apply roles to teams individually [JB]
 * [NEW] Ion Pulse, Energy beam, Cluster CM [JB]
 * [NEW] 'Enter' key trigger control update [JB]
 * [NEW] colorized cbo's [JB]
 * [UPD] copy/paste expanded [JB]
 * [NEW] cmdMoveMessUp/Down and cmdMoveFGUp/Down [JB]
 * [UPD] lbl colors now applied via function to allow themeing [JB]
 * [UPD] general performance improvements [JB]
 * [UPD] delete FG reworked [JB]
 * [NEW] editor-only craft numbering [JB]
 * [UPD] better tab updates [JB]
 * [NEW] PreventCraftNumbering, DepartureClock and Goal time limit unks implemented [JB]
 * [UPD] improved order information [JB]
 * [NEW] blank messages are shown [JB]
 * [FIX] exception in lblMessArr if no messages [JB]
 * v1.4.3, 180509
 * [UPD] changed how Strings.OrderDesc gets split
 * v1.4.1, 171118
 * [UPD] added Exclamation icon to FG delete confirmation
 * v1.4, 171016
 * [NEW #10] Custom ship list loading
 * v1.3, 170107
 * [FIX] various crashes [JB]
 * [NEW] MRU capability [JB]
 * [NEW] Delete menu item, delete key capture [JB]
 * [FIX] Redo opnXvT procedure [JB]
 * [NEW] Craft reference adjustment when deleting FGs [JB]
 * [UPD] newFG() now BOOL [JB]
 * [FIX] SpecialCargo assignment [JB]
 * [FIX] various label refresh issues [JB]
 * [NEW] FG Goal Summary [JB]
 * v1.2.8, 160606
 * [FIX] WaitForExit in Test replaced with named process check loop (Steam's fault)
 * v1.2.7, 150405
 * [FIX] Team copy/paste
 * [FIX] FG Goal copy/paste now gets entire goal with strings and points, not just trigger
 * [FIX] FG Goal strings were saving in the wrong order
 * [UPD] new Globals.Goal.Trigger implementation
 * [ADD] copy/paste mouse functions to Team listing
 * v1.2.5, 150110
 * [FIX] some type corrections near Update calls [JeremyAnsel]
 * [UPD] modified Common.Update calls for generics
 * v1.2.3, 141214
 * [UPD] change to MPL
 * [FIX] blank form when trying to use LstForm when TIE isn't installed
 * v1.2, 121006
 * - removed try{} in opnXvT_FileOk
 * - Settings passed in and out
 * [NEW] Test menu
 * [FIX] Global Goal text boxes saving contents of Completed for all
 * - txtGlobal_Leave now single function instead of Comp/Inc/Fail
 * [UPD] lblStarting now only applies to Normal difficulty
 * [UPD] opn/sav dialogs default to \Train
 * [NEW] Open Recent menu
 * v1.1.1, 120814
 * [UPD] chkWPArr_Leave to chkWPArr_CheckedChanged
 * [FIX] FG list display
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
using Idmr.Platform.Xvt;

namespace Idmr.Yogeme
{
	/// <summary>XvT Mission Editor GUI</summary>
	public partial class XvtForm : Form
	{
		#region vars and stuff
		readonly Settings _config;
		Mission _mission;
		bool _applicationExit;
		int _activeFG = 0;
		int _startingShips = 1;
		bool _loading;
		bool _noRefresh = false;
		int _activeMessage = 0;
		readonly DataTable _table = new DataTable("Waypoints");
		readonly DataTable _tableRaw = new DataTable("Waypoints_Raw");
		MapForm _fMap;
		BriefingForm _fBrief;
		LstForm _fLST;
		FlightGroupLibraryForm _fLibrary;
		byte _activeMessageTrigger = 0;
		byte _activeGlobalTrigger = 0;
		byte _activeTeam = 0;
		byte _activeArrDepTrigger = 0;
		byte _activeFGGoal = 0;
		byte _activeOrder = 0;
		byte _activeOptionCraft = 0;
		byte _activeSkipTrigger = 0;
		#endregion
		#region Control Arrays
#pragma warning disable IDE1006 // Naming Styles
		readonly TextBox[] txtIFF = new TextBox[4];
		readonly RadioButton[] optADAndOr = new RadioButton[8];
		readonly CheckBox[] chkSendTo = new CheckBox[10];
		readonly Label[] lblMessTrig = new Label[4];
		readonly Label[] lblGlobTrig = new Label[12];
		readonly RadioButton[] optGlobAndOr = new RadioButton[18];
		readonly Label[] lblTeam = new Label[10];
		readonly CheckBox[] chkAllies = new CheckBox[10];
		readonly Label[] lblADTrig = new Label[6];
		readonly Label[] lblGoal = new Label[8];
		readonly Label[] lblOrder = new Label[4];
		readonly CheckBox[] chkWP = new CheckBox[22];
		readonly CheckBox[] chkOpt = new CheckBox[18];
		readonly Label[] lblOptCraft = new Label[10];
		readonly ComboBox[] cboEoMColor = new ComboBox[6];
		readonly TextBox[] txtEoM = new TextBox[6];
		readonly MenuItem[] menuRecentMissions = new MenuItem[6];
		readonly Dictionary<Control, EventHandler> instantUpdate = new Dictionary<Control, EventHandler>();   //[JB] This system allows standard form controls to hook their normal YOGEME event handlers (typically Leave) to update immediately when the data is changed.
		readonly Dictionary<ComboBox, ComboBox> ColorizedFGList = new Dictionary<ComboBox, ComboBox>();  //[JB] Maps a control that should have a colorized FG list with a control that determines whether the list actually contains a FG list.
#pragma warning restore IDE1006 // Naming Styles
		#endregion

		public XvtForm(Settings settings, bool bBoP)
		{
			_config = settings;
			InitializeComponent();
			_loading = true;
			initializeMission();
			setBop(bBoP);
			startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		public XvtForm(Settings settings, string path)
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
			if (_fMap != null) _fMap.Close();
			if (_fBrief != null) _fBrief.Close();
			if (_fLST != null) _fLST.Close();
			if (_fLibrary != null) _fLibrary.Close();
		}
		void comboVarRefresh(int index, ComboBox cbo)
		{   //index is usually cboTrigType.SelectedIndex, cbo = cboTrigVar
			if (index == -1) return;
			cbo.Items.Clear();
			string[] temp;
			switch (index)      //switch (VariableType)
			{
				case 0:
					cbo.Items.Add("None");
					break;
				case 1: //FlightGroup
					cbo.Items.AddRange(_mission.FlightGroups.GetList());
					break;
				case 2:
					cbo.Items.AddRange(Strings.CraftType);
					cbo.Items.RemoveAt(0);
					break;
				case 3:
					cbo.Items.AddRange(Strings.ShipClass);
					break;
				case 4:
					cbo.Items.AddRange(Strings.ObjectType);
					break;
				case 5:
					cbo.Items.AddRange(getIffStrings());
					break;
				case 6:
					cbo.Items.AddRange(Strings.Orders);
					break;
				case 7:
					cbo.Items.AddRange(Strings.CraftWhen);
					break;
				//case 8: Global Group
				//since it's just numbers, same as default, left out for specifics
				case 9: //AI Skill
					cbo.Items.AddRange(Strings.Rating);
					break;
				case 0xA: //Status1
					cbo.Items.AddRange(Strings.Status);
					break;
				//case 0xB: Always true
				case 0xC:   // Teams
				case 0x15:  // All Teams except
					temp = _mission.Teams.GetList();
					for (int i = 0; i < temp.Length; i++)
						if (temp[i] == "") temp[i] = "Team " + (i + 1).ToString();  //[JB] Modified to replace empty strings.
					cbo.Items.AddRange(temp);
					break;
				case 0xD:  //Player slot
				case 0x16: //All Player Slot except
					temp = new string[256];
					for (int i = 0; i <= 255; i++) temp[i] = (i + 1).ToString();
					cbo.Items.AddRange(temp);
					if (index == 0xD)
						cbo.Items[0] = "1   (this slot may be buggy!)";
					break;
				case 0xE:  //Elapsed time
					temp = new string[256];
					for (int i = 0; i <= 255; i++) temp[i] = Common.GetFormattedTime(i * 5, false);
					temp[0] += " seconds";
					cbo.Items.AddRange(temp);
					break;
				case 0xF:  //All Flight Group except
					cbo.Items.AddRange(_mission.FlightGroups.GetList());
					break;
				case 0x10:  //All Craft type except
					cbo.Items.AddRange(Strings.CraftType);
					cbo.Items.RemoveAt(0);
					break;
				case 0x11:  //All CraftCategory except
					cbo.Items.AddRange(Strings.ShipClass);
					break;
				case 0x12:  //All ObjectCategory except
					cbo.Items.AddRange(Strings.ObjectType);
					break;
				case 0x13:  //All IFFs except
					cbo.Items.AddRange(getIffStrings());
					break;
				// case 0x14: All Global Group except
				// case 0x15: All Teams except  (already handled, above with case 0xC)
				// case 0x16: All Player Slot except  (already handled, above with case 0xD)
				// case 0x17: Global Unit
				// case 0x18: All Global Unit except
				default:
					temp = new string[256];
					for (int i = 0; i <= 255; i++) temp[i] = i.ToString();
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
		void craftStart(FlightGroup fg, bool bAdd)
		{
			if (fg.Difficulty == 1 || fg.Difficulty == 3 || !fg.ArrivesIn30Seconds) return;
			if (bAdd) _startingShips += fg.NumberOfCraft;
			else _startingShips -= fg.NumberOfCraft;
			lblStarting.Text = _startingShips.ToString() + " craft at 30 seconds";
			if (_startingShips > Mission.CraftLimit) lblStarting.ForeColor = Color.Red;
			else lblStarting.ForeColor = SystemColors.ControlText;
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
			if (_mission.FlightGroups[fgIndex].Difficulty == 6 || _mission.FlightGroups[fgIndex].Difficulty == 7) //[JB] For craft difficulty that never arrives.
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
		bool hasFocus(Label[] list) //[JB] Added helper function to detect focus inside control arrays when copying/pasting triggers.
		{
			foreach (Label c in list)
				if (c.Focused)
					return true;
			return false;
		}
		void initializeMission()
		{
			tabMain.Focus(); //[JB] Exit focus from any form controls.  Fixes some crashes when Leave() events are processed after mission data has been cleared (notably from within the Messages tab).
			_mission = new Mission();
			_config.LastMission = "";
			_activeFG = 0;
			_activeMessage = 0;
			_mission.FlightGroups[0].CraftType = Convert.ToByte(_config.XvtCraft);
			_mission.FlightGroups[0].IFF = Convert.ToByte(_config.XvtIff);
			string[] fgList = _mission.FlightGroups.GetList();
			comboReset(cboArrMS, fgList, 0);
			comboReset(cboArrMSAlt, fgList, 0);
			comboReset(cboDepMS, fgList, 0);
			comboReset(cboDepMSAlt, fgList, 0);
			lstFG.Items.Clear();
			lstFG.Items.Add(_mission.FlightGroups[_activeFG].ToString(true));
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			comboReset(cboIFF, getIffStrings(), 0);
			Text = "Ye Olde Galactic Empire Mission Editor - XvT - New Mission.tie";
		}
		void loadCraftData(string fileMission)
		{
			Strings.OverrideShipList(null, null); //Restore defaults.
			try
			{
				CraftDataManager.GetInstance().LoadPlatform(_mission.IsBop ? Settings.Platform.BoP : Settings.Platform.XvT, _config, Strings.CraftType, Strings.CraftAbbrv, fileMission);
				Strings.OverrideShipList(CraftDataManager.GetInstance().GetLongNames(), CraftDataManager.GetInstance().GetShortNames());
			}
			catch (Exception x) { MessageBox.Show("Error processing custom XvT ship list, using defaults.\n\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			cboCraft.Items.Clear();
			cboCraft.Items.AddRange(Strings.CraftType);
		}
		void labelRefresh(Mission.Trigger trigger, Label lbl)
		{   // lbl is the affected label
			string triggerText = trigger.ToString();
			triggerText = replaceTargetText(triggerText);
			lbl.Text = triggerText;
		}
		bool loadMission(string fileMission)
		{
			//string remove = _config.RecentMissions[0];
			closeForms();
			lstFG.SelectedIndex = 0;  //[JB] Prevents sporadic index out of range exceptions (such as when opening mission with fewer FGs than the current selected index, or opening missions of a different platform)
			lstFG.Items.Clear();
			_activeMessage = 0; //Reset in case new mission has fewer messages.
			lstMessages.Items.Clear();
			_startingShips = 0;
			//byte[] buffer = new byte[64];
			bool startBoP = _mission.IsBop;
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
							txtMissDesc.Text = "";  //[JB] Explicitly loading a vanilla mission, so erase these fields.  Otherwise strings from previously loaded missions remains resident. setBop() may ask to change platforms even though that's not happening, because it's looking at garbage strings.
							txtMissSucc.Text = "";
							txtMissFail.Text = "";
							_mission.MissionFailed = "";
							_mission.MissionSuccessful = "";
							setBop(false);
							break;
						case Platform.MissionFile.Platform.BoP:
							setBop(true);
							break;
						case Platform.MissionFile.Platform.XWA:
							_applicationExit = false;
							new XwaForm(_config, fileMission).Show();
							Close();
							fs.Close();
							return false;
						default:
							throw new Exception("File is not a valid mission file for any platform, please select an appropriate mission file.");
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
				if (startBoP) menuNewBoP_Click(0, new EventArgs());
				else menuNewXvT_Click(0, new EventArgs());
				return false;
			}
			loadCraftData(fileMission);
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
				if (_mission.FlightGroups[i].ArrivesIn30Seconds) craftStart(_mission.FlightGroups[i], true);
			}
			updateFGList();
			if (_mission.Messages.Count == 0) enableMessages(false);
			else
			{
				enableMessages(true);
				for (int i = 0; i < _mission.Messages.Count; i++)
					lstMessages.Items.Add(_mission.Messages[i].MessageString != "" ? _mission.Messages[i].MessageString : " *");
			}
			bool btemp = _loading;  //[JB] Now that InstantUpdate exists, we need to be more careful about batch updating of form information.
			_loading = true;
			comboReset(cboIFF, getIffStrings(), 0);
			updateMissionTabs();
			cboGlobalTeam.SelectedIndex = -1;   // otherwise it doesn't trigger an index change
			cboGlobalTeam.SelectedIndex = 0;
			for (_activeTeam = 0; _activeTeam < 10; _activeTeam++) teamRefresh();
			lblTeamArr_Click(lblTeam[0], new EventArgs());
			_loading = btemp;
			Text = "Ye Olde Galactic Empire Mission Editor - " + (_mission.IsBop ? "BoP" : "XvT") + " - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
			refreshRecent(); //[JB] Setting _config.LastMission modifies the Recent list.  Need to refresh the menu to match.
			return true;
		}
		void promptSave()
		{
			_config.SaveSettings();
			if (_config.ConfirmSave && (this.Text.IndexOf("*") != -1))
			{
				DialogResult res = MessageBox.Show("Mission has been edited without saving, would you like to save?", "Confirm", MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes)
				{
					if (_mission.MissionPath == "\\NewMission.tie") savXvT.ShowDialog();
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
			ColorizedFGList[variable] = variableType;
			variable.DrawMode = DrawMode.OwnerDrawVariable;
			variable.DrawItem += colorizedComboBox_DrawItem;
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
		string replaceTargetText(string text)
		{
			while (text.Contains("FG:"))
			{
				int fg = Common.ParseIntAfter(text, "FG:");
				text = text.Replace("FG:" + fg, ((fg >= 0 && fg < _mission.FlightGroups.Count) ? _mission.FlightGroups[fg].ToString() : "Undefined"));
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
			return text;
		}
		void saveMission(string fileMission)
		{
			try { _fBrief.Save(); }
			catch { /* do nothing */ }
			lblTeamArr_Click(lblTeam[_activeTeam], new EventArgs());
			if (Text.IndexOf("*") == -1) return;    // don't save if unmodifed
			try { _mission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			Text = "Ye Olde Galactic Empire Mission Editor - " + (_mission.IsBop ? "BoP" : "XvT") + " - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
			refreshRecent();  //[JB] Setting _config.LastMission modifies the Recent list.  Need to refresh the menu to match.
							  //Verify the mission after it's been saved
			if (_config.Verify) Common.RunVerify(_mission.MissionPath, _config);
		}
		void setBop(bool bop)
		{
			_mission.IsBop = bop;
			if (_mission.IsBop)
			{
				menuNewXvT.Shortcut = Shortcut.None;
				menuNewBoP.Shortcut = Shortcut.CtrlN;
				_config.LastPlatform = Settings.Platform.BoP;
				Text = Text.Substring(0, 41) + "BoP" + Text.Substring(44);
				txtMissDesc.MaxLength = 0x1000;
				menuTest.Enabled = _config.BopInstalled;
			}
			else
			{
				if (txtMissDesc.Text.Length > 0x400 || txtMissSucc.Text.Length != 0 || txtMissFail.Text.Length != 0)
				{
					DialogResult r = MessageBox.Show("Changing platforms will result in loss of Mission Description and Debriefing text", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					if (r == DialogResult.Cancel) { optBoP.Checked = true; return; }
					if (txtMissDesc.Text.Length > 0x400) txtMissDesc.Text = txtMissDesc.Text.Substring(0, 0x400);
				}
				menuTest.Enabled = _config.XvtInstalled;
				menuNewXvT.Shortcut = Shortcut.CtrlN;
				menuNewBoP.Shortcut = Shortcut.None;
				_config.LastPlatform = Settings.Platform.XvT;
				Text = Text.Substring(0, 41) + "XvT" + Text.Substring(44);
				txtMissDesc.MaxLength = 0x400;
				txtMissSucc.Text = "";
				txtMissFail.Text = "";
			}
			optBoP.Checked = _mission.IsBop;
			optXvT.Checked = !optBoP.Checked;
			txtMissSucc.Visible = _mission.IsBop;
			txtMissFail.Visible = _mission.IsBop;
			txtMissSucc.Enabled = _mission.IsBop;  //[JB] Fix to make these fields editable for BoP
			txtMissFail.Enabled = _mission.IsBop;
			menuNewXvT.ShowShortcut = !_mission.IsBop;
			menuNewBoP.ShowShortcut = _mission.IsBop;
			menuSaveAsBoP.Visible = !_mission.IsBop;
			menuSaveAsXvT.Visible = _mission.IsBop;
			numBackdrop.Maximum = (_mission.IsBop ? 16 : 7);  //[JB] Need to update backdrop maximums for BoP, otherwise it was staying at 7 and causing index
		}
		void setInteractiveLabelColor(Label control, bool highlight)
		{
			control.ForeColor = highlight ? _config.ColorInteractSelected : _config.ColorInteractNonSelected;
			control.BackColor = _config.ColorInteractBackground;
		}
		void startup()
		{
			loadCraftData("");
			Height = 600;   // since VS tends to slowly shrink the damn thing
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			_config.LastMission = "";
			_config.LastPlatform = _mission.IsBop ? Settings.Platform.BoP : Settings.Platform.XvT;
			opnXvT.InitialDirectory = _config.GetWorkingPath();  //[JB] Updated for MRU access.  Defaults to installation and mission folder if not enabled.
			savXvT.InitialDirectory = _config.GetWorkingPath();
			comboReset(cboIFF, getIffStrings(), 0);
			_applicationExit = true;    //becomes false if selecting "New Mission" from menu
			#region Menu
			// menuTest has already been taken care of
			if (_config.RestrictPlatforms)
			{
				if (!_config.TieInstalled) { menuNewTIE.Enabled = false; }
				if (!_config.BopInstalled) { menuNewBoP.Enabled = false; }
				if (!_config.XwaInstalled) { menuNewXWA.Enabled = false; }
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
			#region FlightGroups
			#region Craft
			cboCraft.SelectedIndex = _mission.FlightGroups[0].CraftType; // already loaded in loadCraftData
			cboIFF.SelectedIndex = _mission.FlightGroups[0].IFF; // already loaded in this function
			cboTeam.Items.AddRange(_mission.Teams.GetList()); cboTeam.SelectedIndex = _mission.FlightGroups[0].Team;
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
			#endregion
			#region Arr/Dep
			cboADTrig.Items.AddRange(Strings.Trigger);
			cboADTrigAmount.Items.AddRange(Strings.Amount);
			cboADTrigType.Items.AddRange(Strings.VariableType);
			cboAbort.Items.AddRange(Strings.Abort); cboAbort.SelectedIndex = 0;
			cboDiff.Items.AddRange(Strings.Difficulty); cboDiff.SelectedIndex = 0;
			string[] fgList = _mission.FlightGroups.GetList();
			cboArrMS.Items.AddRange(fgList); cboArrMS.SelectedIndex = 0;
			cboArrMSAlt.Items.AddRange(fgList); cboArrMSAlt.SelectedIndex = 0;
			cboDepMS.Items.AddRange(fgList); cboDepMS.SelectedIndex = 0;
			cboDepMSAlt.Items.AddRange(fgList); cboDepMSAlt.SelectedIndex = 0;
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
			lblADTrigArr_Click(0, new EventArgs());
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
			cboStopArrivingWhen.Items.AddRange(Strings.StopArrivingWhen);
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
			for (int i = 0; i < 4; i++)
			{
				lblOrder[i].Click += new EventHandler(lblOrderArr_Click);
				lblOrder[i].DoubleClick += new EventHandler(lblOrderArr_DoubleClick);
				lblOrder[i].MouseUp += new MouseEventHandler(lblOrderArr_MouseUp);
				lblOrder[i].Tag = i;
			}
			lblOrderArr_Click(0, new EventArgs());
			for (int i = 1; i < 256; i++)  //The designer already starts with one item "default" for 0 MGLT.
				cboOSpeed.Items.Add(Convert.ToInt32(i * 2.2235));
			cboOSpeed.SelectedIndex = 0;
			#endregion
			#region Waypoints
			_table.Columns.Add("X"); _table.Columns.Add("Y"); _table.Columns.Add("Z");
			_tableRaw.Columns.Add("X"); _tableRaw.Columns.Add("Y"); _tableRaw.Columns.Add("Z");
			for (int i = 0; i < 22; i++)    //initialize WPs
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
			dataWaypoints_Raw.Table = _tableRaw;
			dataWP.DataSource = dataWaypoints;
			dataWP_Raw.DataSource = dataWaypoints_Raw;
			this._table.RowChanged += new DataRowChangeEventHandler(table_RowChanged);
			this._tableRaw.RowChanged += new DataRowChangeEventHandler(tableRaw_RowChanged);
			chkWP[0] = chkSP1;
			chkWP[1] = chkSP2;
			chkWP[2] = chkSP3;
			chkWP[3] = chkSP4;
			chkWP[4] = chkWP1;
			chkWP[5] = chkWP2;
			chkWP[6] = chkWP3;
			chkWP[7] = chkWP4;
			chkWP[8] = chkWP5;
			chkWP[9] = chkWP6;
			chkWP[10] = chkWP7;
			chkWP[11] = chkWP8;
			chkWP[12] = chkWPRend;
			chkWP[13] = chkWPHyp;
			chkWP[14] = chkWPBrief1;
			chkWP[15] = chkWPBrief2;
			chkWP[16] = chkWPBrief3;
			chkWP[17] = chkWPBrief4;
			chkWP[18] = chkWPBrief5;
			chkWP[19] = chkWPBrief6;
			chkWP[20] = chkWPBrief7;
			chkWP[21] = chkWPBrief8;
			for (int i = 0; i < 22; i++)
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
			lstGoalTeams.Items.AddRange(_mission.Teams.GetList());
			lblGoalArr_Click(0, new EventArgs());
			#endregion
			#region Options
			cboOptCraft.Items.AddRange(Strings.CraftType);
			cboSkipAmount.Items.AddRange(Strings.Amount); cboSkipAmount.SelectedIndex = 0;
			cboSkipTrig.Items.AddRange(Strings.Trigger); cboSkipTrig.SelectedIndex = 0;
			cboSkipType.Items.AddRange(Strings.VariableType); cboSkipType.SelectedIndex = 0;
			cboRole1.Items.AddRange(Strings.Roles); cboRole1.SelectedIndex = 0;
			cboRole2.Items.AddRange(Strings.Roles); cboRole2.SelectedIndex = 0;
			cboRole3.Items.AddRange(Strings.Roles); cboRole3.SelectedIndex = 0;
			cboRole4.Items.AddRange(Strings.Roles); cboRole4.SelectedIndex = 0;
			cboRoleTeam1.Items.AddRange(Strings.RoleTeams); cboRoleTeam1.SelectedIndex = 0;
			cboRoleTeam2.Items.AddRange(Strings.RoleTeams); cboRoleTeam2.SelectedIndex = 0;
			cboRoleTeam3.Items.AddRange(Strings.RoleTeams); cboRoleTeam3.SelectedIndex = 0;
			cboRoleTeam4.Items.AddRange(Strings.RoleTeams); cboRoleTeam4.SelectedIndex = 0;
			chkOpt[0] = chkOptWNone;
			chkOpt[1] = chkOptWBomb;
			chkOpt[2] = chkOptWRocket;
			chkOpt[3] = chkOptWMissile;
			chkOpt[4] = chkOptWTorp;
			chkOpt[5] = chkOptWAdvMissile;
			chkOpt[6] = chkOptWAdvTorp;
			chkOpt[7] = chkOptWMagPulse;
			chkOpt[8] = chkOptWIonPulse;
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
			lblMessTrig[0] = lblMess1;
			lblMessTrig[1] = lblMess2;
			lblMessTrig[2] = lblMess3;
			lblMessTrig[3] = lblMess4;
			for (int i = 0; i < 4; i++)
			{
				lblMessTrig[i].Click += new EventHandler(lblMessTrigArr_Click);
				lblMessTrig[i].DoubleClick += new EventHandler(lblMessTrigArr_DoubleClick);
				lblMessTrig[i].MouseUp += new MouseEventHandler(lblMessTrigArr_MouseUp);
				lblMessTrig[i].Tag = i;
			}
			#endregion
			#region Globals
			cboGlobalAmount.Items.AddRange(Strings.Amount);
			cboGlobalTrig.Items.AddRange(Strings.Trigger);
			cboGlobalType.Items.AddRange(Strings.VariableType);
			cboGlobalTeam.Items.AddRange(_mission.Teams.GetList());
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
			optGlobAndOr[0] = optPrim1OR2;
			optGlobAndOr[1] = optPrim3OR4;
			optGlobAndOr[2] = optPrim12OR34;
			optGlobAndOr[3] = optPrev1OR2;
			optGlobAndOr[4] = optPrev3OR4;
			optGlobAndOr[5] = optPrev12OR34;
			optGlobAndOr[6] = optSec1OR2;
			optGlobAndOr[7] = optSec3OR4;
			optGlobAndOr[8] = optSec12OR34;
			optGlobAndOr[9] = optPrim1AND2;
			optGlobAndOr[10] = optPrim3AND4;
			optGlobAndOr[11] = optPrim12AND34;
			optGlobAndOr[12] = optPrev1AND2;
			optGlobAndOr[13] = optPrev3AND4;
			optGlobAndOr[14] = optPrev12AND34;
			optGlobAndOr[15] = optSec1AND2;
			optGlobAndOr[16] = optSec3AND4;
			optGlobAndOr[17] = optSec12AND34;
			for (int i = 0; i < 9; i++)
			{
				optGlobAndOr[i].CheckedChanged += new EventHandler(optGlobAndOrArr_CheckedChanged);
				optGlobAndOr[i].Tag = i;
			}
			txtGlobalComp.Tag = Globals.GoalState.Complete;
			txtGlobalFail.Tag = Globals.GoalState.Failed;
			txtGlobalInc.Tag = Globals.GoalState.Incomplete;
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
			chkAllies[0] = chkTeam1;
			chkAllies[1] = chkTeam2;
			chkAllies[2] = chkTeam3;
			chkAllies[3] = chkTeam4;
			chkAllies[4] = chkTeam5;
			chkAllies[5] = chkTeam6;
			chkAllies[6] = chkTeam7;
			chkAllies[7] = chkTeam8;
			chkAllies[8] = chkTeam9;
			chkAllies[9] = chkTeam10;
			for (int i = 0; i < 10; i++)
			{
				lblTeam[i].Click += new EventHandler(lblTeamArr_Click);
				lblTeam[i].DoubleClick += new EventHandler(lblTeamArr_DoubleClick);
				lblTeam[i].MouseUp += new MouseEventHandler(lblTeamArr_MouseUp);
				lblTeam[i].Tag = i;
				chkAllies[i].Leave += new EventHandler(chkAlliesArr_Leave);
				chkAllies[i].Tag = i;
			}
			cboEoMColor[0] = cboPC1Color;
			cboEoMColor[1] = cboPC2Color;
			cboEoMColor[2] = cboPF1Color;
			cboEoMColor[3] = cboPF2Color;
			cboEoMColor[4] = cboSC1Color;
			cboEoMColor[5] = cboSC2Color;
			txtEoM[0] = txtPrimComp1;
			txtEoM[1] = txtPrimComp2;
			txtEoM[2] = txtPrimFail1;
			txtEoM[3] = txtPrimFail2;
			txtEoM[4] = txtSecComp1;
			txtEoM[5] = txtSecComp2;
			for (int i = 0; i < 6; i++)
			{
				cboEoMColor[i].SelectedIndexChanged += new EventHandler(cboEoMColorArr_SelectedIndexChanged);
				cboEoMColor[i].Tag = i;
				txtEoM[i].Leave += new EventHandler(txtEoMArr_Leave);
				txtEoM[i].Tag = i;
			}
			for (_activeTeam = 0; _activeTeam < 10; _activeTeam++) teamRefresh();
			#endregion
			cboGlobalTeam.SelectedIndex = 0;
			cboMissType.Items.AddRange(Enum.GetNames(typeof(Mission.MissionTypeEnum)));
			cboMissType.SelectedIndex = 0;

			txtIFF[0] = txtIFF3;
			txtIFF[1] = txtIFF4;
			txtIFF[2] = txtIFF5;
			txtIFF[3] = txtIFF6;
			for (int i = 0; i < 4; i++)
			{
				txtIFF[i].Leave += new EventHandler(txtIFFArr_Leave);
				txtIFF[i].Tag = i + 2;
			}

			#region InstantUpdate
			//RegisterInstantUpdate(txtName, txtName_Leave);
			registerInstantUpdate(numWaves, grpCraft3_Leave);
			registerInstantUpdate(numCraft, grpCraft3_Leave);
			registerInstantUpdate(numGG, grpCraft3_Leave);
			registerInstantUpdate(numGU, grpCraft3_Leave);
			registerInstantUpdate(chkPreventNumbering, grpCraft3_Leave);
			registerInstantUpdate(cboIFF, grpCraft2_Leave);
			registerInstantUpdate(cboPlayer, grpCraft2_Leave);
			registerInstantUpdate(cboDiff, cboDiff_Leave);
			registerInstantUpdate(chkArrHuman, chkArrHuman_Leave);
			registerInstantUpdate(cboADTrigAmount, cboADTrigAmount_Leave);
			registerInstantUpdate(cboADTrigType, cboADTrigType_SelectedIndexChanged);
			registerInstantUpdate(cboADTrigVar, cboADTrigVar_Leave);
			registerInstantUpdate(cboADTrig, cboADTrig_Leave);

			registerInstantUpdate(cboGoalAmount, grpGoal_Leave);
			registerInstantUpdate(cboGoalArgument, grpGoal_Leave);
			registerInstantUpdate(cboGoalTrigger, grpGoal_Leave);
			registerInstantUpdate(numGoalPoints, numGoalPoints_Leave);

			registerInstantUpdate(numOptWaves, numOptWaves_Leave);
			registerInstantUpdate(numOptCraft, numOptCraft_Leave);
			registerInstantUpdate(cboOptCraft, cboOptCraft_Leave);

			registerInstantUpdate(cboSkipAmount, cboSkipAmount_Leave);
			registerInstantUpdate(cboSkipType, cboSkipType_SelectedIndexChanged);
			registerInstantUpdate(cboSkipVar, cboSkipVar_Leave);
			registerInstantUpdate(cboSkipTrig, cboSkipTrig_Leave);

			//RegisterInstantUpdate(txtMessage, txtMessage_Leave);
			registerInstantUpdate(cboMessAmount, cboMessAmount_Leave);
			registerInstantUpdate(cboMessType, cboMessType_SelectedIndexChanged);
			registerInstantUpdate(cboMessVar, cboMessVar_Leave);
			registerInstantUpdate(cboMessTrig, cboMessTrig_Leave);
			registerInstantUpdate(cboMessColor, cboMessColor_SelectedIndexChanged);

			registerInstantUpdate(cboGlobalAmount, cboGlobalAmount_Leave);
			registerInstantUpdate(cboGlobalType, cboGlobalType_SelectedIndexChanged);
			registerInstantUpdate(cboGlobalVar, cboGlobalVar_Leave);
			registerInstantUpdate(cboGlobalTrig, cboGlobalTrig_Leave);

			registerInstantUpdate(cboOT1, cboOT1_Leave);
			registerInstantUpdate(cboOT2, cboOT2_Leave);
			registerInstantUpdate(cboOT3, cboOT3_Leave);
			registerInstantUpdate(cboOT4, cboOT4_Leave);
			registerInstantUpdate(optOT1T2OR, optOT1T2OR_CheckedChanged);
			registerInstantUpdate(optOT3T4OR, optOT3T4OR_CheckedChanged);

			registerColorizedFGList(cboADTrigVar, cboADTrigType);
			registerColorizedFGList(cboSkipVar, cboSkipType);
			registerColorizedFGList(cboMessVar, cboMessType);
			registerColorizedFGList(cboGlobalVar, cboGlobalType);
			registerColorizedFGList(cboOT1, cboOT1Type);
			registerColorizedFGList(cboOT2, cboOT2Type);
			registerColorizedFGList(cboOT3, cboOT3Type);
			registerColorizedFGList(cboOT4, cboOT4Type);
			registerColorizedFGList(cboArrMS, null);
			registerColorizedFGList(cboArrMSAlt, null);
			registerColorizedFGList(cboDepMS, null);
			registerColorizedFGList(cboDepMSAlt, null);
			#endregion InstantUpdate

			applySettingsHandler(0, new EventArgs());  //[JB] Configurable colors were added to options.
		}
		void updateMissionTabs()
		{
			numMissUnk1.Value = _mission.Unknown1;
			numMissUnk2.Value = _mission.Unknown2;
			chkMissUnk3.Checked = _mission.Unknown3;
			for (int i = 0; i < 4; i++)
			{
				txtIFF[i].Text = _mission.IFFs[i + 2];
			}
			cboMissType.SelectedIndex = (int)_mission.MissionType;
			chkPreventOutcome.Checked = _mission.PreventMissionOutcome;
			numRndSeed.Value = _mission.RndSeed;
			numMissTimeMin.Value = _mission.TimeLimitMin;
			numMissTimeSec.Value = _mission.TimeLimitSec;
			txtMissDesc.Text = _mission.MissionDescription;
			txtMissSucc.Text = _mission.MissionSuccessful;
			txtMissFail.Text = _mission.MissionFailed;
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
			ColorizedFGList.TryGetValue(variable, out ComboBox variableType);
			bool colorize = true;
			if (variableType != null)        //If a VariableType selection control is attached, check that a Flight Group type is selected.
				colorize = (variableType.SelectedIndex == 1 || variableType.SelectedIndex == 0xF);

			if (e.Index == -1 || e.Index >= _mission.FlightGroups.Count) colorize = false;

			if (variable.BackColor == Color.Black || variable.BackColor == SystemColors.Window)
				variable.BackColor = (colorize == true) ? Color.Black : SystemColors.Window;

			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			if (colorize == true) brText = getFlightGroupDrawColor(e.Index);
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
			//Instead of assigning this in designer.cs
			//  this.menuDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
			//Trap and process the key here, with few changes to the original code
			//while allowing the delete key to remain functional in other areas like text boxes.

			if (e.KeyCode == Keys.Delete)
			{
				if (lstFG.Focused || lstMessages.Focused)
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

		//[JB] No longer an event.  Calling this function after Open Dialog closure fixes a bug where the Open Dialog could stick open.
		//void opnXvT_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		void opnXvT_FileOk()
		{
			_loading = true;
			if (loadMission(opnXvT.FileName))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				lstFG.SelectedIndex = 0;
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_loading = false;
		}

		void savXvT_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_mission.MissionPath = savXvT.FileName;
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

		void toolXvT_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			toolXvT.Focus();    // clicking the toolbar doesn't remove focus, so last change may not be saved
			switch (toolXvT.Buttons.IndexOf(e.Button))
			{
				case 0:     //New Mission
					menuNewXvT_Click("toolbar", new EventArgs());
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
					savXvT.ShowDialog();
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
				case 16:    //Help
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
			try { _fBrief.Close(); }
			catch { /* do nothing */ }
			_fBrief = new BriefingForm(_mission.FlightGroups, _mission.Briefings, briefingModifiedCallback);
			_fBrief.Show();
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
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Orders[_activeOrder]);
				data.SetText(_mission.FlightGroups[_activeFG].Orders[_activeOrder].ToString());
			}
			else if (sender.ToString() == "Skip" || lblSkipTrig1.Focused || lblSkipTrig2.Focused)  //[JB] Detect if triggers have focus
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[(lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1)]);
				data.SetText(_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[(lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1)].ToString());
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
				formatter.Serialize(stream, dgt.SelectedText);
				data.SetText(dgt.SelectedText);
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
						formatter.Serialize(stream, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].GoalTrigger);  //[JB] Changed to pasting Mission.Trigger rather than Globals.Goal.Trigger so it can copy/paste from other Triggers.
						data.SetText(_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].GoalTrigger.ToString());
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
			if (_fLibrary != null)
				_fLibrary.Close();
			_fLibrary = new FlightGroupLibraryForm(Settings.Platform.XvT, _mission.FlightGroups, flightGroupLibraryCallback);
		}
		void flightGroupLibraryCallback(object sender, EventArgs e)
		{
			foreach (FlightGroup fg in (object[])sender)
			{
				if (fg == null || !newFG())
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
		void menuLST_Click(object sender, EventArgs e)
		{
			try { _fLST.Close(); }
			catch { /* do nothing */ }
			if (_mission.IsBop) _fLST = new LstForm(Settings.Platform.BoP, _config);
			else _fLST = new LstForm(Settings.Platform.XvT, _config);
			_fLST.Show();
		}
		void menuMap_Click(object sender, EventArgs e)
		{
			try { _fMap.Close(); }
			catch { /* do nothing */ }
			_fMap = new MapForm(_config, _mission.IsBop, _mission.FlightGroups, mapForm_DataChangedCallback);
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
			_loading = true;
			initializeMission();
			updateMissionTabs();
			lstMessages.Items.Clear();
			enableMessages(false);
			lblMessage.Text = "Message #0 of 0";
			lstFG.SelectedIndex = 0;
			for (_activeTeam = 0; _activeTeam < 10; _activeTeam++) teamRefresh();
			lblTeamArr_Click(lblTeam[0], new EventArgs());
			setBop(sender.ToString() == "BoP");
			if (this.Text.EndsWith("*")) this.Text = this.Text.Substring(0, this.Text.Length - 1);
			_loading = false;  //[JB] Fix loading state when creating a new mission
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
			opnXvT.FileName = _mission.MissionFileName;
			if (opnXvT.ShowDialog() == DialogResult.OK)  //[JB] Wait until dialog is closed before loading.  Fixes bug where dialog could be stuck open.  Also update paths for last-opened folder. 
			{
				opnXvT_FileOk();
				_config.SetWorkingPath(Path.GetDirectoryName(opnXvT.FileName));
				opnXvT.InitialDirectory = _config.GetWorkingPath(); //Update since folder may have changed
			}
		}
		void menuOptions_Click(object sender, EventArgs e)
		{
			new OptionsDialog(_config, applySettingsHandler).ShowDialog();
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
		void menuPaste_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream;
			try { stream = new FileStream(Application.StartupPath + "\\YOGEME.bin", FileMode.Open, FileAccess.Read, FileShare.Read); }
			catch { return; }
			#region ArrDep
			if (sender.ToString() == "AD" || hasFocus(lblADTrig))  //[JB] Detect if triggers have focus
			{
				try
				{
					Mission.Trigger t = (Mission.Trigger)formatter.Deserialize(stream);
					_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger] = t;
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
			if (sender.ToString() == "Order" || hasFocus(lblOrder))  //[JB] Detect if orders have focus
			{
				try
				{
					FlightGroup.Order o = (FlightGroup.Order)formatter.Deserialize(stream);
					_mission.FlightGroups[_activeFG].Orders[_activeOrder] = o;
					lblOrderArr_Click(_activeOrder, new EventArgs());
					orderLabelRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region FG Goals
			if (sender.ToString() == "Goal" || hasFocus(lblGoal))  //[JB] Detect if goals have focus
			{
				try
				{
					FlightGroup.Goal g = (FlightGroup.Goal)formatter.Deserialize(stream);
					_mission.FlightGroups[_activeFG].Goals[_activeFGGoal] = g ?? throw new Exception();
					lblGoalArr_Click(_activeFGGoal, new EventArgs());
					goalLabelRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region Skip to O4
			if (sender.ToString() == "Skip" || (lblSkipTrig1.Focused || lblSkipTrig2.Focused))  //[JB] Detect if triggers have focus
			{
				try
				{
					Mission.Trigger t = (Mission.Trigger)formatter.Deserialize(stream);
					int j = (lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1);
					_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[j] = t;
					lblSkipTrigArr_Click(j, new EventArgs());
					labelRefresh(_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[j], (j == 0 ? lblSkipTrig1 : lblSkipTrig2));  // no array, hence explicit naming
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
					if (s.IndexOf("System.", 0) != -1) throw new Exception();   // bypass byte[]
					if (s.IndexOf("Idmr.", 0) != -1) throw new Exception(); // [JB] Prevent the class name when an entire Message is copied.
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
			#region MessTrig
			if (sender.ToString() == "MessTrig" || hasFocus(lblMessTrig))  //[JB] Detect if triggers have focus
			{
				try
				{
					Mission.Trigger t = (Mission.Trigger)formatter.Deserialize(stream);
					_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger] = t;
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
					if (ActiveControl.GetType().ToString() == "System.Windows.Forms.Label") break; //[JB] Prevent copy/paste on any clickable label.
					try
					{
						FlightGroup fg = (FlightGroup)formatter.Deserialize(stream);
#pragma warning disable IDE0016 // Use 'throw' expression
						if (fg == null) throw new Exception();
#pragma warning restore IDE0016 // Use 'throw' expression
							   //[JB] Need to test copy to make sure it worked
						if (newFG() == false)
							break;
						_mission.FlightGroups[_activeFG] = fg;
						refreshMap(-1);
						updateFGList(); //[JB] Update all the downdown lists.
						listRefresh();
						_startingShips--;
						lstFG.SelectedIndex = _activeFG;
						lstFG_SelectedIndexChanged(0, new EventArgs()); //[JB] Need to force refresh of form controls.
						craftStart(fg, true);
					}
					catch { /* do nothing */ }
					break;
				case 1:
					if (ActiveControl.GetType().ToString() == "System.Windows.Forms.Label") break; //[JB] Prevent copy/paste on any clickable label.
					try
					{
						Platform.Xvt.Message m = (Platform.Xvt.Message)formatter.Deserialize(stream);
#pragma warning disable IDE0016 // Use 'throw' expression
						if (m == null) throw new Exception();
#pragma warning restore IDE0016 // Use 'throw' expression
						newMess();
						_mission.Messages[_activeMessage] = m;
						messListRefresh();
						lstMessages.SelectedIndex = _activeMessage;
					}
					catch { /* do nothing */ }
					break;
				case 2:
					try  //[JB] Changed to pasting Mission.Trigger rather than Globals.Goal.Trigger so it can copy/paste from other Triggers.
					{
						Mission.Trigger t = (Mission.Trigger)formatter.Deserialize(stream);
						_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Triggers[_activeGlobalTrigger % 4].GoalTrigger = t;
						lblGlobTrigArr_Click(_activeGlobalTrigger, new EventArgs());
						Common.Title(this, false);
					}
					catch { /* do nothing */ }
					break;
				case 3:
					try
					{
						Team t = (Team)formatter.Deserialize(stream);
#pragma warning disable IDE0016 // Use 'throw' expression
						if (t == null) throw new Exception();
#pragma warning restore IDE0016 // Use 'throw' expression
						_mission.Teams[_activeTeam] = t;
						teamRefresh();
						lblTeamArr_Click(lblTeam[_activeTeam], new EventArgs());
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
				_loading = true;        //turned false in previous line
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_config.SetWorkingPath(Path.GetDirectoryName(mission)); //[JB] Update last-accessed
			opnXvT.InitialDirectory = _config.GetWorkingPath();
			savXvT.InitialDirectory = _config.GetWorkingPath();
			_loading = false;
		}
		void menuSave_Click(object sender, EventArgs e)
		{
			if (_mission.MissionPath == "\\NewMission.tie") savXvT.ShowDialog();
			else saveMission(_mission.MissionPath);
		}
		void menuSaveAsBoP_Click(object sender, EventArgs e)
		{
			setBop(true);
			savXvT.ShowDialog();
		}
		void menuSaveAsTIE_Click(object sender, EventArgs e)
		{
			promptSave();
			try
			{
				Platform.Tie.Mission converted = Platform.Converter.XvtBopToTie(_mission);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXvT_Click(object sender, EventArgs e)
		{
			setBop(false);
			savXvT.ShowDialog();
		}
		void menuSaveAsXWA_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsXWA", new System.EventArgs());
			Common.RunConverter(_mission.MissionPath, 2);
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
			string xvtPath;
			string bopPath;
			if (!File.Exists(path + "Z_XVT__.exe"))
			{
				System.Diagnostics.Debug.WriteLine("XvT/BoP not detected at MissionPath, default used");
				path = (_mission.IsBop ? _config.BopPath : _config.XvtPath) + "\\";
				xvtPath = _config.XvtPath + "\\";
				bopPath = _config.BopPath + "\\";
			}
			else
			{
				if (path.ToLower().Contains("balanceofpower"))
				{
					bopPath = path;
					xvtPath = Directory.GetParent(path).Parent.FullName + "\\";
				}
				else
				{
					xvtPath = path;
					bopPath = path + "BalanceOfPower\\";
				}
			}

			bool localMission = _mission.MissionPath.ToLower().Contains(path.ToLower());
			string fileName = (!localMission ? path + "Train\\" + _mission.MissionFileName : _mission.MissionPath);
			if (!localMission)
			{
				if (File.Exists(fileName))
				{
					DialogResult res = MessageBox.Show("You are not working in the platform directory and a mission with that filename exists. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (res == DialogResult.No) return;
					File.Copy(fileName, fileName + ".bak");
				}
				File.Copy(_mission.MissionPath, fileName, true);
			}

			if (_config.VerifyTest && !_config.Verify) Common.RunVerify(_mission.MissionPath, _config);
			/*Version os = Environment.OSVersion.Version;
			bool isWin7 = (os.Major == 6 && os.Minor == 1);
			System.Diagnostics.Process explorer = null;
			int restart = 1;
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);*/

			// configure XvT/BoP
			int index = 0;
			while (File.Exists(xvtPath + "test" + index + "0.plt")) index++;
			string pilot = "test" + index + "0.plt";
			string bopPilot = "test" + index + "0.pl2";
			string lst = "Train\\IMPERIAL.LST";
			string backup = "Train\\IMPERIAL_" + index + ".bak";

			File.Copy(Application.StartupPath + "\\xvttest0.plt", xvtPath + pilot);
			if (_config.BopInstalled) File.Copy(Application.StartupPath + "\\xvttest0.pl2", bopPath + bopPilot, true);
			// XvT pilot edit
			FileStream pilotFile = File.OpenWrite(xvtPath + pilot);
			pilotFile.Position = 4;
			char[] indexBytes = index.ToString().ToCharArray();
			new BinaryWriter(pilotFile).Write(indexBytes);
			for (int i = (int)pilotFile.Position; i < 0xC; i++) pilotFile.WriteByte(0);
			pilotFile.Close();
			// BoP pilot edit
			if (_config.BopInstalled)
			{
				pilotFile = File.OpenWrite(bopPath + bopPilot);
				pilotFile.Position = 4;
				indexBytes = index.ToString().ToCharArray();
				new BinaryWriter(pilotFile).Write(indexBytes);
				for (int i = (int)pilotFile.Position; i < 0xC; i++) pilotFile.WriteByte(0);
				pilotFile.Close();
			}

			// configure XvT
			System.Diagnostics.Process xvt = new System.Diagnostics.Process();
			xvt.StartInfo.FileName = path + "Z_XVT__.exe";
			xvt.StartInfo.Arguments = "/skipintro";
			xvt.StartInfo.UseShellExecute = false;
			xvt.StartInfo.WorkingDirectory = path;
			File.Copy(path + lst, path + backup, true);
			StreamReader sr = File.OpenText(xvtPath + "Config.cfg");
			string contents = sr.ReadToEnd();
			sr.Close();
			int lastpilot = contents.IndexOf("lastpilot ") + 10;
			int nextline = contents.IndexOf("\r\n", lastpilot);
			string modified = contents.Substring(0, lastpilot) + "test" + index + contents.Substring(nextline);
			StreamWriter sw = new FileInfo(xvtPath + "Config.cfg").CreateText();
			sw.Write(modified);
			sw.Close();
			if (_config.BopInstalled)
			{
				sr = File.OpenText(bopPath + "config2.cfg");
				contents = sr.ReadToEnd();
				sr.Close();
				lastpilot = contents.IndexOf("lastpilot ") + 10;
				nextline = contents.IndexOf("\r\n", lastpilot);
				modified = contents.Substring(0, lastpilot) + "test" + index + contents.Substring(nextline);
				sw = new FileInfo(bopPath + "config2.cfg").CreateText();
				sw.Write(modified);
				sw.Close();
			}
			sr = File.OpenText(path + lst);
			contents = sr.ReadToEnd();
			sr.Close();
			string[] expanded = contents.Replace("\r\n", "\0").Split('\0');
			expanded[4] = _mission.MissionFileName;
			expanded[5] = "YOGEME: " + expanded[4];
			modified = string.Join("\r\n", expanded);
			sw = new FileInfo(path + lst).CreateText();
			sw.Write(modified);
			sw.Close();

			/*if (isWin7)	// explorer kill so colors work right
			{
				restart = (int)key.GetValue("AutoRestartShell", 1);
				key.SetValue("AutoRestartShell", 0, Microsoft.Win32.RegistryValueKind.DWord);
				explorer = System.Diagnostics.Process.GetProcessesByName("explorer")[0];
				explorer.Kill();
				explorer.WaitForExit();
			}*/

			xvt.Start();
			System.Threading.Thread.Sleep(1000);
			System.Diagnostics.Process[] runningXvts = System.Diagnostics.Process.GetProcessesByName("Z_XVT__");
			while (runningXvts.Length > 0)
			{
				Application.DoEvents();
				System.Diagnostics.Debug.WriteLine("sleeping...");
				System.Threading.Thread.Sleep(1000);
				runningXvts = System.Diagnostics.Process.GetProcessesByName("Z_XVT__");
			}

			/*if (isWin7)	// restart
			{
				key.SetValue("AutoRestartShell", restart, Microsoft.Win32.RegistryValueKind.DWord);
				explorer.StartInfo.UseShellExecute = false;
				explorer.StartInfo.FileName = "explorer.exe";
				explorer.Start();
			}*/
			if (_config.DeleteTestPilots)
			{
				File.Delete(xvtPath + pilot);
				File.Delete(bopPath + bopPilot);
			}
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
		#endregion
		#region Flight Groups
		/// <summary>Counts all trigger and parameter references of a FG</summary>
		/// <param name="fgIndex">Index within _mission.FlightGroups</param>
		/// <remarks>Used to populate the counters in the confirm deletion dialog</remarks>
		int[] countFlightGroupReferences(int fgIndex)
		{
			int[] count = new int[8];
			const int cMothership = 1, cArrDep = 2, cOrder = 3, cSkip = 4, cGoal = 5, cMessage = 6, cBrief = 7;
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
					if (adt.VariableType == 1 && adt.Variable == fgIndex && adt.Condition != 0 && adt.Condition != 10) count[cArrDep]++;
				}
				foreach (FlightGroup.Order order in fg.Orders)
				{
					if (order.Target1Type == 1 && order.Target1 == fgIndex) count[cOrder]++;
					if (order.Target2Type == 1 && order.Target2 == fgIndex) count[cOrder]++;
					if (order.Target3Type == 1 && order.Target3 == fgIndex) count[cOrder]++;
					if (order.Target4Type == 1 && order.Target4 == fgIndex) count[cOrder]++;
				}
				foreach (Mission.Trigger sk in fg.SkipToOrder4Trigger)
					if (sk.VariableType == 1 && sk.Variable == fgIndex) count[cSkip]++;
			}

			foreach (Globals global in _mission.Globals)
				foreach (Globals.Goal goal in global.Goals)
					foreach (Globals.Goal.Trigger trig in goal.Triggers)
						if (trig.GoalTrigger.VariableType == 1 && trig.GoalTrigger.Variable == fgIndex)
							count[cGoal]++;

			foreach (Platform.Xvt.Message msg in _mission.Messages)
				foreach (Mission.Trigger trig in msg.Triggers)
					if (trig.VariableType == 1 && trig.Variable == fgIndex)
						count[cMessage]++;

			foreach (var br in _mission.Briefings)
			{
				int p = 0;
				while (p < br.EventsLength)
				{
					if (br.Events[p + 1] >= (int)Platform.BaseBriefing.EventType.FGTag1 && br.Events[p + 1] <= (int)Platform.BaseBriefing.EventType.FGTag8)
						if (br.Events[p + 2] == fgIndex)
							count[cBrief]++;

					p += 2 + br.EventParameterCount(br.Events[p + 1]);
				}
			}

			for (int i = 1; i < 8; i++)
				count[0] += count[i];
			return count;
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
					string[] Reasons = new string[8] { "", "Mothership reference", "Arrival/Departure trigger", "Order target reference", "'Skip to Order 4' trigger", "Global Goal trigger", "Message trigger", "Briefing FG Tag" };
					string breakdown = "";
					for (int i = 1; i < 8; i++)
						if (count[i] > 0) breakdown += "    " + count[i] + " " + Reasons[i] + ((count[i] > 1) ? "s" : "") + "\n";

					string s = "This Flight Group is referenced " + count[0] + " time" + ((count[0] > 1) ? "s" : "") + " in these cases:\n" + breakdown + "\nAll references targeting this flight group will be reset to default.";
					if (count[7] > 0) s += "\nAssociated Briefing FG Tag events will be deleted.";
					s += "\n\nAre you sure you want to delete this Flight Group?";
					DialogResult res = MessageBox.Show(s, "WARNING: Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
					if (res == DialogResult.No)
						return;
				}
			}
			replaceClipboardFGReference(_activeFG, -1);

			if (_mission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(_activeFG);
			craftStart(_mission.FlightGroups[_activeFG], false);
			if (_mission.FlightGroups.Count == 1)
			{
				_activeFG = _mission.DeleteFG(_activeFG);  //[JB] Need to perform a full delete to wipe the FG indexes (messages or briefing tags may still have them).  The delete function always ensures that Count==1, so it must be inside this block, not before.
				_mission.FlightGroups.Clear();
				_activeFG = 0;
				_mission.FlightGroups[0].CraftType = _config.XvtCraft;
				_mission.FlightGroups[0].IFF = _config.XvtIff;
				craftStart(_mission.FlightGroups[0], true);
			}
			else _activeFG = _mission.DeleteFG(_activeFG); //[JB] Actual delete moved to platform.
			updateFGList();
			lstFG.SelectedIndex = _activeFG;
			refreshMap(-1);
			try
			{
				_fBrief.Import(_mission.FlightGroups);
				_fBrief.MapPaint();
			}
			catch { /* do nothing */ }
			Common.Title(this, _loading);

			//[JB] Need to force these tabs to refresh, otherwise the trigger controls might display outdated information
			lstMessages_SelectedIndexChanged(0, new EventArgs());
			cboGlobalTeam_SelectedIndexChanged(0, new EventArgs());
			lstFG_SelectedIndexChanged(0, new EventArgs());

			if (!lstFG.Focused) lstFG.Focus();  //[JB] Return control back to the list (helpful to maintain navigation using the arrow keys when certain tabs are open)
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
					if (goal.Condition == 0 || goal.Condition == 10)  //Avoid True, False conditions.
						continue;
					for (int j = 0; j < 10; j++)
					{
						if(goal.GetEnabledForTeam(j))
						{
							string c = Strings.CraftAbbrv[fg.CraftType] + " " + fg.Name;
							string n = goal.ToString().Replace("Flight Group", c);
							int category = ((goal.Argument <= 1) ? 0 : 2);  //0 = primary, 1 = prevent, 2 = bonus
							goalList[(j * 3) + category].Add(n);
						}
					}
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
		void listRefresh()
		{
			//[JB] Replacing the text triggers lstFG_SelectedIndexChanged.  Improves debugger performance by avoiding massive slowdown of repeated refreshing.  Also avoids a stack overflow which can be caused by an endless loop of certain event conflicts.
			_noRefresh = true;  //Prevent full lstFG refresh.
			lstFG.Items[_activeFG] = _mission.FlightGroups[_activeFG].ToString(true);
			if (!lstFG.IsDisposed)  //Changing platforms will throw an exception accessing a disposed object.
				lstFG.Invalidate(lstFG.GetItemRectangle(_activeFG));  //[JB] Forces the ListBox to redraw in case the IFF color changed.
			_noRefresh = false;
			if (_fMap != null) _fMap.UpdateFlightGroup(_activeFG, _mission.FlightGroups[_activeFG]);  //[JB] If the display name needs to be updated, the map most likely does too.
		}
		bool newFG()
		{
			if (_mission.FlightGroups.Count >= Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			_activeFG = _mission.FlightGroups.Add();
			_mission.FlightGroups[_activeFG].CraftType = _config.XvtCraft;
			_mission.FlightGroups[_activeFG].IFF = _config.XvtIff;
			craftStart(_mission.FlightGroups[_activeFG], true);
			lstFG.Items.Add("");    // only need to create the item, listRefresh() will fill it in
			updateFGList();
			lstFG.SelectedIndex = _activeFG;
			_loading = false;
			refreshMap(-1);
			try
			{
				_fBrief.Import(_mission.FlightGroups);
				_fBrief.MapPaint();
			}
			catch { /* do nothing */ }
			Common.Title(this, _loading);
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
				if (fg.PreventCraftNumbering)
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
				if (fg.EditorCraftExplicit != !fg.PreventCraftNumbering)
				{
					fg.EditorCraftExplicit = !fg.PreventCraftNumbering;
					change = true;
				}

				if (!fg.PreventCraftNumbering)
					Common.AddFGCounter(fg.CraftType, fg.IFF, fg.Name, fg.NumberOfCraft, craftCount);

				if (change)
					lstFG.Items[i] = _mission.FlightGroups[i].ToString(true);
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
					FlightGroup fg_temp = (FlightGroup)raw;
					if (dstIndex >= 0)
					{
						change |= fg_temp.TransformFGReferences(dstIndex, 255);
						change |= fg_temp.TransformFGReferences(srcIndex, dstIndex);
						change |= fg_temp.TransformFGReferences(255, srcIndex);
					}
					else
					{
						change |= fg_temp.TransformFGReferences(srcIndex, -1);
					}
				}
				else if (raw.GetType() == typeof(Platform.Xvt.Message))
				{
					Platform.Xvt.Message mess_temp = (Platform.Xvt.Message)raw;
					foreach (var trig in mess_temp.Triggers)
					{
						if (dstIndex >= 0)
							change |= trig.SwapFGReferences(srcIndex, dstIndex);
						else
							change |= trig.TransformFGReferences(srcIndex, dstIndex, true);
					}
				}
				else if (raw.GetType() == typeof(Mission.Trigger))
				{
					Mission.Trigger trig_temp = (Mission.Trigger)raw;
					if (dstIndex >= 0)
						change |= trig_temp.SwapFGReferences(srcIndex, dstIndex);
					else
						change |= trig_temp.TransformFGReferences(srcIndex, dstIndex, true);

				}
				else if (raw.GetType() == typeof(FlightGroup.Order))
				{
					FlightGroup.Order order_temp = (FlightGroup.Order)raw;
					if (dstIndex >= 0)
					{
						change |= order_temp.TransformFGReferences(dstIndex, 255);
						change |= order_temp.TransformFGReferences(srcIndex, dstIndex);
						change |= order_temp.TransformFGReferences(255, srcIndex);
					}
					else
					{
						change |= order_temp.TransformFGReferences(srcIndex, -1);
					}
				}
				if (change)
				{
					stream = new FileStream(Application.StartupPath + "\\YOGEME.bin", FileMode.Create, FileAccess.Write, FileShare.None);
					formatter.Serialize(stream, raw);
					stream.Close();
				}
			}
			catch
			{
				if (stream != null) stream.Close();  //Just in case...
			}
		}
		void swapFG(int srcIndex, int dstIndex)
		{
			if (_mission.SwapFG(srcIndex, dstIndex))
			{
				replaceClipboardFGReference(srcIndex, dstIndex);
				if (_fBrief != null) _fBrief.Close();
				refreshMap(-1);
				updateFGList();
				listRefresh();  //Current FG
				_activeFG = dstIndex; listRefresh(); //Set to, and refresh destination.
				lstFG.SelectedIndex = dstIndex;
				Common.Title(this, _loading);
			}
		}
		void updateFGList()
		{
			//[JB] Adding this here since it's a convenient way of updating the craft numbering in any situation it may be needed.  Otherwise it would need to be called on every major FG operation (move, add, delete, rename).  Since this potentially changes multiple FG names, it needs to be called before the normal updateFGList() code.
			recalculateEditorCraftNumber();

			string[] fgList = _mission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			comboReset(cboArrMS, fgList, 0);
			comboReset(cboArrMSAlt, fgList, 0);
			comboReset(cboDepMS, fgList, 0);
			comboReset(cboDepMSAlt, fgList, 0);
			//[JB] Force refresh of trigger/order controls if Type==Flight Group is selected.
			if (cboADTrigType.SelectedIndex == 1) comboReset(cboADTrigVar, fgList, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable);
			if (cboSkipType.SelectedIndex == 1) comboReset(cboSkipVar, fgList, cboSkipVar.SelectedIndex);
			if (cboGlobalType.SelectedIndex == 1) comboReset(cboGlobalVar, fgList, cboGlobalVar.SelectedIndex);
			if (cboOT1Type.SelectedIndex == 1) comboReset(cboOT1, fgList, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1);
			if (cboOT2Type.SelectedIndex == 1) comboReset(cboOT2, fgList, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2);
			if (cboOT3Type.SelectedIndex == 1) comboReset(cboOT3, fgList, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3);
			if (cboOT4Type.SelectedIndex == 1) comboReset(cboOT4, fgList, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4);
			if (cboMessType.SelectedIndex == 1) comboReset(cboMessVar, fgList, cboMessVar.SelectedIndex);
			//[JB] This is the simplest way to force all labels to refresh, but not the most efficient. An annoying side effect of forcing clicks is that the current selection will change, so restore after refreshing.
			int restore = _activeArrDepTrigger;
			foreach (var lbl in lblADTrig) lblADTrigArr_Click(lbl, new EventArgs());
			lblADTrigArr_Click(lblADTrig[restore], new EventArgs());

			restore = _activeGlobalTrigger;
			foreach (var lbl in lblGlobTrig) lblGlobTrigArr_Click(lbl, new EventArgs());
			if (restore >= 0) lblGlobTrigArr_Click(lblGlobTrig[restore], new EventArgs());

			restore = _activeOrder;
			foreach (var lbl in lblOrder) lblOrderArr_Click(lbl, new EventArgs());
			lblOrderArr_Click(lblOrder[restore], new EventArgs());

			restore = _activeMessageTrigger;
			foreach (var lbl in lblMessTrig) lblMessTrigArr_Click(lbl, new EventArgs());
			lblMessTrigArr_Click(lblMessTrig[restore], new EventArgs());

			restore = _activeSkipTrigger;
			lblSkipTrigArr_Click((restore == 0 ? lblSkipTrig2 : lblSkipTrig1), new EventArgs());  //Only two, inactive one first, then active.
			lblSkipTrigArr_Click((restore == 0 ? lblSkipTrig1 : lblSkipTrig2), new EventArgs());

			_loading = temp;
			listRefresh();
		}
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
				}
				foreach (Mission.Trigger sk in fg.SkipToOrder4Trigger)
					if ((sk.VariableType == 8 || sk.VariableType == 20) && sk.Variable == gg)
					{
						if (update) sk.Variable = (byte)numGG.Value;
						else refCount++;
					}
			}
			foreach (Globals global in _mission.Globals)
				foreach (Globals.Goal goal in global.Goals)
					foreach (Globals.Goal.Trigger trig in goal.Triggers)
						if ((trig.GoalTrigger.VariableType == 8 || trig.GoalTrigger.VariableType == 20) && trig.GoalTrigger.Variable == gg)
						{
							if (update) trig.GoalTrigger.Variable = (byte)numGG.Value;
							else refCount++;
						}
			foreach (Platform.Xvt.Message msg in _mission.Messages)
				foreach (Mission.Trigger trig in msg.Triggers)
					if ((trig.VariableType == 8 || trig.VariableType == 20) && trig.Variable == gg)
					{
						if (update) trig.Variable = (byte)numGG.Value;
						else refCount++;
					}
			// since I'm using foreach and don't have the index, just redo all of the visible ones in case it's one we updated
			for (int i = 0; i < 12; i++) labelRefresh(_mission.Globals[_activeTeam].Goals[i / 4].Triggers[i % 4].GoalTrigger, lblGlobTrig[i]);
			lblGlobTrigArr_Click(_activeGlobalTrigger, new EventArgs());
			if (_mission.Messages.Count > 0)
			{
				for (int i = 0; i < 4; i++) labelRefresh(_mission.Messages[_activeMessage].Triggers[i], lblMessTrig[i]);
				lblMessTrigArr_Click(_activeMessageTrigger, new EventArgs());
			}
			return !update && ggCount == 0 && refCount > 0;
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
			if (_noRefresh == true && lstFG.SelectedIndex == _activeFG) return;   //[JB] Altered for performance, see note in listRefresh().
			_activeFG = lstFG.SelectedIndex;
			lblFG.Text = "Flight Group #" + (_activeFG + 1).ToString() + " of " + _mission.FlightGroups.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			#region Craft
			txtName.Text = _mission.FlightGroups[_activeFG].Name;
			txtCargo.Text = _mission.FlightGroups[_activeFG].Cargo;
			txtSpecCargo.Text = _mission.FlightGroups[_activeFG].SpecialCargo;  //[JB] Fixed, was setting to Cargo
			numSC.Value = _mission.FlightGroups[_activeFG].SpecialCargoCraft;
			chkRandSC.Checked = _mission.FlightGroups[_activeFG].RandSpecCargo;
			cboCraft.SelectedIndex = _mission.FlightGroups[_activeFG].CraftType;
			cboIFF.SelectedIndex = _mission.FlightGroups[_activeFG].IFF;
			cboTeam.SelectedIndex = _mission.FlightGroups[_activeFG].Team;
			try { cboAI.SelectedIndex = _mission.FlightGroups[_activeFG].AI; }  // for some reason, some custom missions have this as -1
			catch { cboAI.SelectedIndex = 2; _mission.FlightGroups[_activeFG].AI = 2; }
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
			refreshStatus();  //Handles Status1, special case for mines.
			cboStatus2.SelectedIndex = _mission.FlightGroups[_activeFG].Status2;
			cboWarheads.SelectedIndex = _mission.FlightGroups[_activeFG].Missile;
			cboBeam.SelectedIndex = _mission.FlightGroups[_activeFG].Beam;
			cboCounter.SelectedIndex = _mission.FlightGroups[_activeFG].Countermeasures;
			numExplode.Value = _mission.FlightGroups[_activeFG].ExplosionTime;
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
				optADAndOr[i].Checked = _mission.FlightGroups[_activeFG].ArrDepAO[i];
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
			lblADTrigArr_Click(lblADTrig[0], new EventArgs());  //[JB] Added sender.  Fixes massive slowdown from exception handling a null control.  Also fixed the remaining _Click() calls with the relevant senders.
			Common.SafeSetCBO(cboStopArrivingWhen, _mission.FlightGroups[_activeFG].StopArrivingWhen, true);
			numRandomArrivalDelayMinutes.Value = _mission.FlightGroups[_activeFG].RandomArrivalDelayMinutes;
			numRandomArrivalDelaySeconds.Value = _mission.FlightGroups[_activeFG].RandomArrivalDelaySeconds;
			#endregion
			for (_activeFGGoal = 0; _activeFGGoal < 8; _activeFGGoal++) goalLabelRefresh();
			lblGoalArr_Click(lblGoal[0], new EventArgs());
			#region Waypoints
			refreshWaypointTab();  //[JB] Code moved to separate function so that the map callback can refresh it too.
			#endregion
			for (_activeOrder = 0; _activeOrder < 4; _activeOrder++) orderLabelRefresh();
			lblOrderArr_Click(lblOrder[0], new EventArgs());
			#region Options
			cboRole1.SelectedIndex = 0;
			cboRole2.SelectedIndex = 0;
			cboRole3.SelectedIndex = 0;
			cboRole4.SelectedIndex = 0;
			cboRoleTeam1.SelectedIndex = 0;
			setRoleControls(_mission.FlightGroups[_activeFG]);

			for (int i = 0; i < 18; i++) chkOpt[i].Checked = _mission.FlightGroups[_activeFG].OptLoadout[i];
			lblSkipTrigArr_Click(lblSkipTrig1, new EventArgs());  //[JB] Fixed
			lblSkipTrigArr_Click(lblSkipTrig2, new EventArgs());
			for (_activeOptionCraft = 0; _activeOptionCraft < 10; _activeOptionCraft++) optCraftLabelRefresh();
			lblOptCraftArr_Click(lblOptCraft[0], new EventArgs());
			cboOptCat.SelectedIndex = (int)_mission.FlightGroups[_activeFG].OptCraftCategory;
			optSkipOR.Checked = _mission.FlightGroups[_activeFG].SkipToO4T1AndOrT2;
			optSkipAND.Checked = !optSkipOR.Checked;
			#endregion
			#region Unknowns
			numUnk1.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown1;
			numUnk5.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown5;
			numUnkOrder.Value = 1;
			numUnkOrder_ValueChanged(0, new EventArgs()); //[JB] Force refresh of associated checkboxes
			chkUnk17.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown17;
			chkUnk18.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown18;
			chkPreventNumbering.Checked = _mission.FlightGroups[_activeFG].PreventCraftNumbering;
			numDepClockMin.Value = _mission.FlightGroups[_activeFG].DepartureClockMinutes;
			numDepClockSec.Value = _mission.FlightGroups[_activeFG].DepartureClockSeconds;
			chkUnk22.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown22;
			chkUnk23.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown23;
			chkUnk24.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown24;
			chkUnk25.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown25;
			chkUnk26.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown26;
			chkUnk27.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown27;
			chkUnk28.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown28;
			chkUnk29.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown29;
			#endregion
			//[JB] Need to check special cargo to restore edit box if necessary.
			bool sc = _mission.FlightGroups[_activeFG].RandSpecCargo | (_mission.FlightGroups[_activeFG].SpecialCargoCraft > 0);
			lblNotUsed.Visible = !sc;
			txtSpecCargo.Visible = sc;
			if (numBackdrop.Enabled) //[JB] Need to update the backdrop edit box
			{
				int v = _mission.FlightGroups[_activeFG].Status1;
				if (v > numBackdrop.Maximum)
					v = (int)numBackdrop.Maximum;
				numBackdrop.Value = v;  //[JB] Need to enforce limit since some XvT missions have values out of range which throw an exception when trying to select that FG
			}
			_loading = btemp;

			cmdMoveFGUp.Enabled = (_activeFG > 0);
			cmdMoveFGDown.Enabled = (_activeFG < lstFG.Items.Count - 1);

			if (!lstFG.Focused) lstFG.Focus();  //[JB] Return control back to the list (helpful to maintain navigation using the arrow keys when certain tabs are open)
		}

		void cmdMoveFGUp_Click(object sender, EventArgs e)
		{
			swapFG(lstFG.SelectedIndex, lstFG.SelectedIndex - 1);
		}
		void cmdMoveFGDown_Click(object sender, EventArgs e)
		{
			swapFG(lstFG.SelectedIndex, lstFG.SelectedIndex + 1);
		}
		#region Craft
		void enableBackdrop(bool state)
		{
			cboStatus2.Enabled = !state;
			cboWarheads.Enabled = !state;
			cboCounter.Enabled = !state;
			cboBeam.Enabled = !state;
			numCraft.Enabled = !state;
			numWaves.Enabled = !state;
			txtCargo.Enabled = !state;
			txtSpecCargo.Enabled = !state;
			numSC.Enabled = !state;
			chkRandSC.Enabled = !state;
			cboAI.Enabled = !state;
			cboMarkings.Enabled = !state;
			cboPlayer.Enabled = !state;
			cboPosition.Enabled = !state;
			cboRadio.Enabled = !state;
			cboFormation.Enabled = !state;
			numSpacing.Enabled = !state;
			numLead.Enabled = !state;
			cmdForms.Enabled = !state;
			cmdBackdrop.Enabled = state;
			numBackdrop.Enabled = state;
			cboStatus.Enabled = !state;
			if (state) lblStatus.Text = "Backdrop";
			else lblStatus.Text = "Status";
		}
		void refreshStatus()
		{
			cboStatus.Items.Clear();
			bool isMine = (_mission.FlightGroups[_activeFG].CraftType >= 0x4B && _mission.FlightGroups[_activeFG].CraftType <= 0x4D);
			lblStatus.Text = isMine ? "Mine Formation" : "Status";
			cboStatus.Items.AddRange(isMine ? Strings.FormationMine : Strings.Status);
			Common.SafeSetCBO(cboStatus, (isMine ? _mission.FlightGroups[_activeFG].Status1 & 3 : _mission.FlightGroups[_activeFG].Status1), true);
			cboFormation.Enabled = !isMine;
		}

		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			enableBackdrop(cboCraft.SelectedIndex == 0x57);
			if (_loading) return;
			_mission.FlightGroups[_activeFG].CraftType = Common.Update(this, _mission.FlightGroups[_activeFG].CraftType, Convert.ToByte(cboCraft.SelectedIndex));
			updateFGList();
			refreshStatus();
		}
		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].Formation = (byte)cboFormation.SelectedIndex;
			Common.Title(this, false);
		}
		void cboStatus_Leave(object sender, EventArgs e)
		{
			//[JB] Added try/catch since there are more Status effects than Backdrops
			try { numBackdrop.Value = cboStatus.SelectedIndex; }
			catch { numBackdrop.Value = numBackdrop.Maximum; }
			_mission.FlightGroups[_activeFG].Status1 = Common.Update(this, _mission.FlightGroups[_activeFG].Status1, Convert.ToByte(cboStatus.SelectedIndex));
		}

		void chkRandSC_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].RandSpecCargo = chkRandSC.Checked;
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
			Common.Title(this, false);
		}

		void cmdBackdrop_Click(object sender, EventArgs e)
		{
			try
			{
				BackdropDialog dlg = new BackdropDialog((_mission.IsBop ? Platform.MissionFile.Platform.BoP : Idmr.Platform.MissionFile.Platform.XvT), _mission.FlightGroups[_activeFG].Status1, _config);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					numBackdrop.Value = dlg.BackdropIndex;  // simply GUI
					cboStatus.SelectedIndex = (int)numBackdrop.Value;   // drives stored value
					_mission.FlightGroups[_activeFG].Status1 = Common.Update(this, _mission.FlightGroups[_activeFG].Status1, Convert.ToByte(cboStatus.SelectedIndex));
				}
			}
			catch (Exception x)  //[JB] Catch all exceptions.
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void cmdForms_Click(object sender, EventArgs e)
		{
			try  //[JB] Added try/catch
			{
				FormationDialog dlg = new FormationDialog(_mission.FlightGroups[_activeFG].Formation);
				if (dlg.ShowDialog() == DialogResult.OK)
					cboFormation.SelectedIndex = Common.Update(this, cboFormation.SelectedIndex, dlg.Formation);
			}
			catch { MessageBox.Show("Could not load the Formations browser.", "Error"); }
		}

		void grpCraft2_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].IFF = Common.Update(this, _mission.FlightGroups[_activeFG].IFF, Convert.ToByte(cboIFF.SelectedIndex));
			_mission.FlightGroups[_activeFG].Team = Common.Update(this, _mission.FlightGroups[_activeFG].Team, Convert.ToByte(cboTeam.SelectedIndex));
			_mission.FlightGroups[_activeFG].AI = Common.Update(this, _mission.FlightGroups[_activeFG].AI, Convert.ToByte(cboAI.SelectedIndex));
			_mission.FlightGroups[_activeFG].Markings = Common.Update(this, _mission.FlightGroups[_activeFG].Markings, Convert.ToByte(cboMarkings.SelectedIndex));
			_mission.FlightGroups[_activeFG].PlayerNumber = Common.Update(this, _mission.FlightGroups[_activeFG].PlayerNumber, Convert.ToByte(cboPlayer.SelectedIndex));
			_mission.FlightGroups[_activeFG].PlayerCraft = Common.Update(this, _mission.FlightGroups[_activeFG].PlayerCraft, Convert.ToByte(cboPosition.SelectedIndex));
			_mission.FlightGroups[_activeFG].Formation = Common.Update(this, _mission.FlightGroups[_activeFG].Formation, Convert.ToByte(cboFormation.SelectedIndex));
			_mission.FlightGroups[_activeFG].Radio = Common.Update(this, _mission.FlightGroups[_activeFG].Radio, Convert.ToByte(cboRadio.SelectedIndex));
			_mission.FlightGroups[_activeFG].FormDistance = Common.Update(this, _mission.FlightGroups[_activeFG].FormDistance, Convert.ToByte(numSpacing.Value));
			_mission.FlightGroups[_activeFG].FormLeaderDist = Common.Update(this, _mission.FlightGroups[_activeFG].FormLeaderDist, Convert.ToByte(numLead.Value));

			if (ActiveControl == cboIFF)
				updateFGList();  //[JB] Update everything, including craft counter.
			else
				listRefresh();   //Otherwise default to simple refresh.
		}
		void grpCraft3_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].NumberOfWaves = Common.Update(this, _mission.FlightGroups[_activeFG].NumberOfWaves, Convert.ToByte(numWaves.Value));
			craftStart(_mission.FlightGroups[_activeFG], false);
			_mission.FlightGroups[_activeFG].NumberOfCraft = Common.Update(this, _mission.FlightGroups[_activeFG].NumberOfCraft, Convert.ToByte(numCraft.Value));
			craftStart(_mission.FlightGroups[_activeFG], true);
			if (_mission.FlightGroups[_activeFG].SpecialCargoCraft > _mission.FlightGroups[_activeFG].NumberOfCraft) numSC.Value = 0;
			//_mission.FlightGroups[_activeFG].GlobalGroup = Common.Update(this, _mission.FlightGroups[_activeFG].GlobalGroup, Convert.ToByte(numGG.Value));
			_mission.FlightGroups[_activeFG].GlobalUnit = Common.Update(this, _mission.FlightGroups[_activeFG].GlobalUnit, Convert.ToByte(numGU.Value));
			_mission.FlightGroups[_activeFG].PreventCraftNumbering = Common.Update(this, _mission.FlightGroups[_activeFG].PreventCraftNumbering, chkPreventNumbering.Checked);
			if (ActiveControl == numCraft || ActiveControl == chkPreventNumbering)
				updateFGList();  //[JB] Now that FG names indicate duplicates, recalculate and update if necessary.
			else
				listRefresh();   //Otherwise default to simple refresh.
		}
		void grpCraft4_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Status2 = Common.Update(this, _mission.FlightGroups[_activeFG].Status2, Convert.ToByte(cboStatus2.SelectedIndex));
			_mission.FlightGroups[_activeFG].Missile = Common.Update(this, _mission.FlightGroups[_activeFG].Missile, Convert.ToByte(cboWarheads.SelectedIndex));
			_mission.FlightGroups[_activeFG].Beam = Common.Update(this, _mission.FlightGroups[_activeFG].Beam, Convert.ToByte(cboBeam.SelectedIndex));
			_mission.FlightGroups[_activeFG].Countermeasures = Common.Update(this, _mission.FlightGroups[_activeFG].Countermeasures, Convert.ToByte(cboCounter.SelectedIndex));
			//XvtMission.FlightGroups[FG].ExplosionTime = Convert.ToByte(Common.Title(this, false, XvtMission.FlightGroups[activeFG].ExplosionTime, numExplode.Value));
		}

		void numBackdrop_Leave(object sender, EventArgs e)
		{
			cboStatus.SelectedIndex = (int)numBackdrop.Value;
			_mission.FlightGroups[_activeFG].Status1 = Common.Update(this, _mission.FlightGroups[_activeFG].Status1, Convert.ToByte(cboStatus.SelectedIndex));
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
			_mission.FlightGroups[_activeFG].GlobalGroup = Common.Update(this, _mission.FlightGroups[_activeFG].GlobalGroup, Convert.ToByte(numGG.Value));
		}
		void numSC_ValueChanged(object sender, EventArgs e)
		{
			Common.Title(this, _loading);
			if (_mission.FlightGroups[_activeFG].RandSpecCargo) { numSC.Value = 0; return; }
			if (numSC.Value == 0 || numSC.Value > _mission.FlightGroups[_activeFG].NumberOfCraft)
			{
				_mission.FlightGroups[_activeFG].SpecialCargoCraft = 0;
				txtSpecCargo.Visible = false;
				lblNotUsed.Visible = true;
			}
			else
			{
				_mission.FlightGroups[_activeFG].SpecialCargoCraft = (byte)numSC.Value;
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
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeArrDepTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeArrDepTrigger = Convert.ToByte(sender); l = lblADTrig[_activeArrDepTrigger]; }
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 6; i++) if (i != _activeArrDepTrigger) setInteractiveLabelColor(lblADTrig[i], false);
			bool btemp = _loading;
			_loading = true;
			cboADTrig.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Condition;
			cboADTrigType.SelectedIndex = -1;
			cboADTrigType.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType;
			cboADTrigAmount.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Amount;
			_loading = btemp;
		}
		void lblADTrigArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("AD", new EventArgs());
		}
		void lblADTrigArr_DoubleClick(object sender, EventArgs e)
		{
			menuPaste_Click("AD", new EventArgs());
		}
		void optADAndOrArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			RadioButton r = (RadioButton)sender;
			_mission.FlightGroups[_activeFG].ArrDepAO[(int)r.Tag] = Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepAO[(int)r.Tag], r.Checked);
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
			listRefresh(); //[JB] Refresh FG text
			foreach (var item in ColorizedFGList)  //[JB] This changes the display color, so refresh these controls too.
				item.Key.Refresh();
		}

		void chkArrHuman_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArriveOnlyIfHuman = Common.Update(this, _mission.FlightGroups[_activeFG].ArriveOnlyIfHuman, chkArrHuman.Checked);
			listRefresh(); //[JB] Refresh FG text
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
			_mission.FlightGroups[_activeFG].DepartureClockMinutes = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureClockMinutes, Convert.ToByte(numDepClockMin.Value));
			_mission.FlightGroups[_activeFG].DepartureClockSeconds = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureClockSeconds, Convert.ToByte(numDepClockSec.Value));
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
			if (!_loading) _mission.FlightGroups[_activeFG].ArrivalMethod1 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalMethod1, optArrMS.Checked);
		}
		void optArrMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMSAlt.Enabled = optArrMSAlt.Checked;
			if (!_loading) _mission.FlightGroups[_activeFG].ArrivalMethod2 = Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalMethod2, optArrMSAlt.Checked);
		}
		void optDepMS_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMS.Enabled = optDepMS.Checked;
			if (!_loading) _mission.FlightGroups[_activeFG].DepartureMethod1 = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureMethod1, optDepMS.Checked);
		}
		void optDepMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMSAlt.Enabled = optDepMSAlt.Checked;
			if (!_loading) _mission.FlightGroups[_activeFG].DepartureMethod2 = Common.Update(this, _mission.FlightGroups[_activeFG].DepartureMethod2, optDepMSAlt.Checked);
		}

		private void grpMoreArrival_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].StopArrivingWhen = Common.Update(this, _mission.FlightGroups[_activeFG].StopArrivingWhen, Convert.ToByte(cboStopArrivingWhen.SelectedIndex));
			_mission.FlightGroups[_activeFG].RandomArrivalDelayMinutes = Common.Update(this, _mission.FlightGroups[_activeFG].RandomArrivalDelayMinutes, Convert.ToByte(numRandomArrivalDelayMinutes.Value));
			_mission.FlightGroups[_activeFG].RandomArrivalDelaySeconds = Common.Update(this, _mission.FlightGroups[_activeFG].RandomArrivalDelaySeconds, Convert.ToByte(numRandomArrivalDelaySeconds.Value));
		}
		#endregion
		#region Orders
		void orderLabelRefresh()
		{
			FlightGroup.Order order = _mission.FlightGroups[_activeFG].Orders[_activeOrder];
			string orderText = order.ToString();
			orderText = replaceTargetText(orderText);
			if (_activeOrder == 3 && orderText == "None")  //[JB] Order 4 can only be activated by using the Skip trigger.
				orderText += new string(' ', 50) + "(only used by Skip trigger)";
			if (order.Command == 0x12 && order.Variable2 >= 1 && order.Variable2 <= _mission.FlightGroups.Count)  //[JB] Display flight group for Drop Off command.  Var is one-based.
				orderText += ", " + _mission.FlightGroups[order.Variable2 - 1].ToString(false);
			lblOrder[_activeOrder].Text = "Order " + (_activeOrder + 1) + ": " + orderText;
		}

		void lblOrderArr_Click(object sender, EventArgs e)
		{
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeOrder = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeOrder = Convert.ToByte(sender); l = lblOrder[_activeOrder]; }
			FlightGroup.Order order = _mission.FlightGroups[_activeFG].Orders[_activeOrder];
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 4; i++) if (i != _activeOrder) setInteractiveLabelColor(lblOrder[i], false);
			bool btemp = _loading;
			_loading = true;
			cboOrders.SelectedIndex = order.Command;
			cboOThrottle.SelectedIndex = order.Throttle;
			numOVar1.Value = order.Variable1;
			numOVar2.Value = order.Variable2;
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
			txtOString.Text = order.Designation;
			_loading = btemp;
		}
		void lblOrderArr_DoubleClick(object sender, EventArgs e)
		{
			menuPaste_Click("Order", new EventArgs());
		}
		void lblOrderArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Order", new EventArgs());
		}

		void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Command = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Command, Convert.ToByte(cboOrders.SelectedIndex));
			orderLabelRefresh();
			string[] s = Strings.OrderDesc[cboOrders.SelectedIndex].Split('|');
			lblODesc.Text = s[0];
			lblOVar1.Text = s[1];
			lblOVar2.Text = s[2];
			numOVar1_ValueChanged(0, new EventArgs()); //[JB] Force refresh, since label information is provided to the user.
			numOVar2_ValueChanged(0, new EventArgs());
		}
		void cboOT1_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1, Convert.ToByte(cboOT1.SelectedIndex));
			orderLabelRefresh();
		}
		void cboOT1Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT1Type.SelectedIndex == -1) return;
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1Type = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1Type, Convert.ToByte(cboOT1Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1Type, cboOT1);
			try { cboOT1.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1; }
			catch { cboOT1.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1 = 0; }
			orderLabelRefresh();
		}
		void cboOT2_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2, Convert.ToByte(cboOT2.SelectedIndex));
			orderLabelRefresh();
		}
		void cboOT2Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT2Type.SelectedIndex == -1) return;
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2Type = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2Type, Convert.ToByte(cboOT2Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2Type, cboOT2);
			try { cboOT2.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2; }
			catch { cboOT2.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2 = 0; }
			orderLabelRefresh();
		}
		void cboOT3_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3, Convert.ToByte(cboOT3.SelectedIndex));
			orderLabelRefresh();
		}
		void cboOT3Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT3Type.SelectedIndex == -1) return;
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3Type = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3Type, Convert.ToByte(cboOT3Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3Type, cboOT3);
			try { cboOT3.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3; }
			catch { cboOT3.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3 = 0; }
			orderLabelRefresh();
		}
		void cboOT4_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4, Convert.ToByte(cboOT4.SelectedIndex));
			orderLabelRefresh();
		}
		void cboOT4Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT4Type.SelectedIndex == -1) return;
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4Type = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4Type, Convert.ToByte(cboOT4Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4Type, cboOT4);
			try { cboOT4.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4; }
			catch { cboOT4.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4 = 0; }
			orderLabelRefresh();
		}
		void cboOThrottle_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Throttle = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Throttle, Convert.ToByte(cboOThrottle.SelectedIndex));
		}
		void cboOSpeed_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ActiveControl == cboOSpeed)
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Speed = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Speed, Convert.ToByte(cboOSpeed.SelectedIndex));
			lblOSpeedNote.Visible = (cboOSpeed.SelectedIndex > 0);
		}

		void cmdCopyOrder_Click(object sender, EventArgs e)
		{
			menuCopy_Click("Order", new EventArgs());
		}
		void cmdPasteOrder_Click(object sender, EventArgs e)
		{
			menuPaste_Click("Order", new EventArgs());
		}

		void numOVar1_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveControl == numOVar1)  //[JB] Since additional processing was added, only change the actual value if the user prompted it.
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable1 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable1, Convert.ToByte(numOVar1.Value));

			//[JB] Display additional information and warnings to the user.
			byte value = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable1;
			int command = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Command;
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
				case 0x1F:
				case 0x20:
				case 0x24: //Wait, SS Wait, SS Board, Board to Repair, Self-Destruct
					text = Common.GetFormattedTime(value * 5, true);
					break;
				case 0x02:
				case 0x03:
				case 0x15:  //Circle, Circle and Evade, SS Patrol Loop.
					if (value == 0) { text = "Zero, no loops!"; warning = true; }
					break;
				case 0x0A:  //Escort
					if (value < 9) text = "Above";
					else if (value > 17) text = "Below";
					if (value % 3 == 0) text += " Left";
					else if (value % 3 == 2) text += " Right";
					if (value == 13 || value == 27) { text = "Coincident"; warning = true; }   // the only one that won't be caught otherwise
					value = (byte)(value % 9);
					if (value < 3 && numOVar1.Value != 27) text = "Leading " + text;
					else if (value > 5) text = "Trailing " + text;
					text = text.Replace("  ", " ");
					if (numOVar1.Value > 27) { text = "Invalid"; warning = true; }
					break;
			}
			lblOVar1Note.Text = text;
			lblOVar1Note.Visible = (text != "");
			lblOVar1Note.ForeColor = warning ? Color.Red : SystemColors.ControlText;
		}
		void numOVar2_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveControl == numOVar2)  //[JB] Since additional processing was added, only change the actual value if the user prompted it.
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable2, Convert.ToByte(numOVar2.Value));

			//[JB] Display additional information and warnings to the user.
			int command = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Command;
			int value = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable2;
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
					warning = (value == 0);
					if (value == 0) text = "No dockings.";
					break;
				case 0xA:  //Escort.  Moved warning here since the first variable controls position.
					if (_mission.IsBop)
					{
						text = "Order bugged in BoP!";
						warning = true;
					}
					break;
				case 0x12:  //Drop Off
					if (value >= 1 && value <= _mission.FlightGroups.Count)   //Variable is FG #, one based.
					{
						text = _mission.FlightGroups[value - 1].ToString(false);
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

		void optOT1T2OR_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].T1AndOrT2 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].T1AndOrT2, optOT1T2OR.Checked);
			orderLabelRefresh();
		}
		void optOT3T4OR_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].T3AndOrT4 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].T3AndOrT4, optOT3T4OR.Checked);
			orderLabelRefresh();
		}
		void optSkipOR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].SkipToO4T1AndOrT2 = Common.Update(this, _mission.FlightGroups[_activeFG].SkipToO4T1AndOrT2, optSkipOR.Checked);
		}

		void txtOString_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Designation = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Designation, txtOString.Text);
		}
		#endregion
		#region Goals
		void goalLabelRefresh()
		{
			lblGoal[_activeFGGoal].Text = "Goal " + (_activeFGGoal + 1).ToString() + ": " + _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].ToString();
		}

		void lblGoalArr_Click(object sender, EventArgs e)
		{
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
			txtGoalInc.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].IncompleteText;
			txtGoalComp.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].CompleteText;
			txtGoalFail.Text = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].FailedText;
			numGoalTimeLimit.Value = _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].TimeLimit;
			for (int i = 0; i < 10; i++)
				lstGoalTeams.SetSelected(i, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].GetEnabledForTeam(i));
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

		void grpGoal_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Argument = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Argument, Convert.ToByte(cboGoalArgument.SelectedIndex));
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Condition = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Condition, Convert.ToByte(cboGoalTrigger.SelectedIndex));
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Amount = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Amount, Convert.ToByte(cboGoalAmount.SelectedIndex));
			goalLabelRefresh();
		}

		void numGoalPoints_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Points = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].Points, (short)numGoalPoints.Value);
			goalLabelRefresh();
		}

		void lstGoalTeams_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(ActiveControl != lstGoalTeams || _loading) return;
			for (int i = 0; i < lstGoalTeams.Items.Count; i++)
				_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].SetEnabledForTeam(i, lstGoalTeams.GetSelected(i));
			Common.Title(this, true);
		}

		void numGoalTimeLimit_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveControl == numGoalTimeLimit)
				_mission.FlightGroups[_activeFG].Goals[_activeFGGoal].TimeLimit = Common.Update(this, _mission.FlightGroups[_activeFG].Goals[_activeFGGoal].TimeLimit, Convert.ToByte(numGoalTimeLimit.Value));

			int sec = (int)numGoalTimeLimit.Value * 5;
			lblGoalTimeLimit.Text = (sec == 0 ? "No time limit" : "< " + Common.GetFormattedTime(sec, false));
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
		void enableRot(bool state)
		{
			numYaw.Enabled = state;
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
			for (int i = 0; i < 22; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					_tableRaw.Rows[i][j] = _mission.FlightGroups[_activeFG].Waypoints[i][j];
					_table.Rows[i][j] = Math.Round((double)_mission.FlightGroups[_activeFG].Waypoints[i][j] / 160, 2);
				}
				chkWP[i].Checked = _mission.FlightGroups[_activeFG].Waypoints[i].Enabled;
			}
			_tableRaw.AcceptChanges();
			_table.AcceptChanges();
			numYaw.Value = _mission.FlightGroups[_activeFG].Yaw;
			numPitch.Value = _mission.FlightGroups[_activeFG].Pitch;
			numRoll.Value = _mission.FlightGroups[_activeFG].Roll;
			if (_mission.FlightGroups[_activeFG].CraftType <= 0x45) enableRot(false);
			else enableRot(true);
			_loading = btemp;
		}

		void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			_mission.FlightGroups[_activeFG].Waypoints[(int)c.Tag].Enabled = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[(int)c.Tag].Enabled, c.Checked);
			refreshMap(_activeFG);
		}

		void table_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j;
			if (_loading) return;
			_loading = true;
			for (j = 0; j < 22; j++) if (_table.Rows[j].Equals(e.Row)) break;   //find the row index that you're changing
			for (i = 0; i < 3; i++)
			{
				if (!double.TryParse(_table.Rows[j][i].ToString(), out double cell))
					_table.Rows[j][i] = 0;
				short raw = (short)(cell * 160);
				_mission.FlightGroups[_activeFG].Waypoints[j][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[j][i], raw);
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
			for (j = 0; j < 22; j++) if (_tableRaw.Rows[j].Equals(e.Row)) break;    //find the row index that you're changing
			for (i = 0; i < 3; i++)
			{
				if (!short.TryParse(_tableRaw.Rows[j][i].ToString(), out short raw))
					_tableRaw.Rows[j][i] = 0;
				_mission.FlightGroups[_activeFG].Waypoints[j][i] = Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[j][i], raw);
				_table.Rows[j][i] = Math.Round((double)raw / 160, 2);
			}
			_loading = false;
			refreshMap(_activeFG);
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
		#endregion
		#region Options
		void enableOptCat(bool state)
		{
			numOptCraft.Enabled = state;
			numOptWaves.Enabled = state;
			cboOptCraft.Enabled = state;
			for (int i = 0; i < 10; i++) lblOptCraft[i].Enabled = state;
		}
		string getRoleString(ComboBox teamSel, ComboBox roleSel)
		{
			if (teamSel.SelectedIndex <= 0 || roleSel.SelectedIndex == -1)  //Check negative, or Role Disabled.
				return "";
			int team = teamSel.SelectedIndex;

			if (team >= 1 && team <= Strings.TeamPrefixes.Length)
				return Strings.TeamPrefixes.Substring(team - 1, 1) + Strings.Roles[roleSel.SelectedIndex].Substring(0, 3).ToUpper();

			return "";
		}
		void optCraftLabelRefresh()
		{
			lblOptCraft[_activeOptionCraft].Text = "Craft " + (_activeOptionCraft + 1).ToString() + ":";
			if (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType != 0) lblOptCraft[_activeOptionCraft].Text += " " + (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves + 1)
				+ " x (" + (_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft) + ") " + Strings.CraftType[_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType]; //Strings.CraftType[_activeOptionCraft];  //[JB] Fix so all labels properly refresh in load time, getting the craft name directly from the setting rather than the combobox . Also fixed, NumberOfCraft is not +1, should be exactly as is.
		}
		void setRoleControls(FlightGroup fg)
		{
			ComboBox[] teams = new ComboBox[4];
			teams[0] = cboRoleTeam1;
			teams[1] = cboRoleTeam2;
			teams[2] = cboRoleTeam3;
			teams[3] = cboRoleTeam4;
			ComboBox[] roles = new ComboBox[4];
			roles[0] = cboRole1;
			roles[1] = cboRole2;
			roles[2] = cboRole3;
			roles[3] = cboRole4;

			string[] roleList = Strings.Roles;
			for (int i = 0; i < 4; i++)
			{
				string code = fg.Roles[i];
				int tindex = 0;
				int rindex = -1;
				if (code.Length == 4)
				{
					string team = code.Substring(0, 1).ToUpper();
					string role = code.Substring(1, 3).ToUpper();

					tindex = Strings.TeamPrefixes.IndexOf(team) + 1;  //If not found, -1 becomes index 0 for "Role Disabled"
					for (int j = 0; j < roleList.Length; j++)
					{
						if (roleList[j].ToUpper().IndexOf(role) == 0)
						{
							rindex = j;
							continue;
						}
					}
				}
				if (rindex < 0) rindex = 0;
				teams[i].SelectedIndex = tindex;
				roles[i].SelectedIndex = rindex;
			}
		}
		/*void updateRole(string teamChar)
		{
			ComboBox c = null;
			switch (teamChar)
			{
				case "1":
					c = cboRole1;
					break;
				case "2":
					c = cboRole2;
					break;
				case "3":
					c = cboRole3;
					break;
				case "4":
					c = cboRole4;
					break;
				case "a":
					c = cboRoleTeam1;
					break;
				default:
					return;
			}
			string s = teamChar + c.Text.Substring(0, 3).ToUpper();
			for (int i = 0; i < 4; i++) // first, try to find existing entry and replace it
			{
				try
				{
					if (_mission.FlightGroups[_activeFG].Roles[i].StartsWith(teamChar))
					{
						_mission.FlightGroups[_activeFG].Roles[i] = Common.Update(this, _mission.FlightGroups[_activeFG].Roles[i], s);
						return;
					}
				}
				catch { /* do nothing  }  // block is to catch null strings
			}
			// no entry
			for (int i = 0; i < 4; i++)
			{
				if (_mission.FlightGroups[_activeFG].Roles[i] == "")
				{
					Common.Title(this, false);
					_mission.FlightGroups[_activeFG].Roles[i] = s;
					break;  // find the first unused
				}
			}
			// if this is the fifth role, then tough luck it's not getting saved
		}*/

		void chkOptArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			int i = (int)c.Tag;
			_mission.FlightGroups[_activeFG].OptLoadout[i] = Common.Update(this, _mission.FlightGroups[_activeFG].OptLoadout[i], c.Checked);
			bool tempLoad = _loading;
			_loading = true;
			if (c.Checked)
			{
				if (i == 0) for (int j = 1; j < 9; j++) _mission.FlightGroups[_activeFG].OptLoadout[j] = false; // turn off warheads
				else if (i > 0 && i < 9) _mission.FlightGroups[_activeFG].OptLoadout[0] = false;        // clear warhead None
				else if (i == 9) for (int j = 10; j < 14; j++) _mission.FlightGroups[_activeFG].OptLoadout[j] = false;  // turn off beams
				else if (i > 9 && i < 14) _mission.FlightGroups[_activeFG].OptLoadout[9] = false;   // clear beam None
				else if (i == 14) for (int j = 15; j < 18; j++) _mission.FlightGroups[_activeFG].OptLoadout[j] = false;  // turn off CMs
				else _mission.FlightGroups[_activeFG].OptLoadout[14] = false;   // clear CM None
			}
			else
			{
				bool b = false;
				if (i > 0 && i < 9)
				{
					for (int j = 1; j < 9; j++) b |= _mission.FlightGroups[_activeFG].OptLoadout[j];
					if (!b && !chkOpt[0].Checked) _mission.FlightGroups[_activeFG].OptLoadout[0] = true;
				}
				b = false;
				if (i > 9 && i < 14)
				{
					for (int j = 10; j < 14; j++) b |= _mission.FlightGroups[_activeFG].OptLoadout[j];
					if (!b && !chkOpt[9].Checked) _mission.FlightGroups[_activeFG].OptLoadout[9] = true;
				}
				b = false;
				if (i > 14 && i < 18)
				{
					for (int j = 15; j < 18; j++) b |= _mission.FlightGroups[_activeFG].OptLoadout[j];
					if (!b && !chkOpt[14].Checked) _mission.FlightGroups[_activeFG].OptLoadout[14] = true;
				}
			}
			for (i = 0; i < 18; i++) chkOpt[i].Checked = _mission.FlightGroups[_activeFG].OptLoadout[i];
			_loading = tempLoad;
		}
		void lblOptCraftArr_Click(object sender, EventArgs e)
		{
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeOptionCraft = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeOptionCraft = Convert.ToByte(sender); l = lblOptCraft[_activeOptionCraft]; }
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
				i = (int)sender;    // i = clicked trigger from code
				if (i == 0) { l = lblSkipTrig1; ; ll = lblSkipTrig2; }
				else { l = lblSkipTrig2; ll = lblSkipTrig1; }
			}
			setInteractiveLabelColor(l, true);
			setInteractiveLabelColor(ll, false);
			_activeSkipTrigger = (byte)i;
			bool btemp = _loading;
			_loading = true;
			cboSkipTrig.SelectedIndex = _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Condition;
			cboSkipType.SelectedIndex = -1;
			cboSkipType.SelectedIndex = _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].VariableType;
			cboSkipAmount.SelectedIndex = _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Amount;
			_loading = btemp;
		}
		void lblSkipTrigArr_DoubleClick(object sender, EventArgs e)
		{
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
			enableOptCat((cboOptCat.SelectedIndex == 4));
		}
		void cboOptCraft_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType = Common.Update(this, _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].CraftType, Convert.ToByte(cboOptCraft.SelectedIndex));
			_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft = Common.Update(this, _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft, Convert.ToByte(numOptCraft.Value));  //[JB] NumberOfCraft was staying at zero unless the user explicitly changed numOptCraft.  Added update here to make sure it gets initialized properly.
			optCraftLabelRefresh();
		}
		void cboSkipTrig_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1);
			_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Condition = Common.Update(this, _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Condition, Convert.ToByte(cboSkipTrig.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i], (i == 0 ? lblSkipTrig1 : lblSkipTrig2));
		}
		void cboSkipType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboSkipType.SelectedIndex == -1) return;
			int i = (lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1);
			if (!_loading)
				_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].VariableType = Common.Update(this, _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].VariableType, Convert.ToByte(cboSkipType.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].VariableType, cboSkipVar);
			try { cboSkipVar.SelectedIndex = _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Variable; }
			catch { cboSkipVar.SelectedIndex = 0; _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Variable = 0; }
			labelRefresh(_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i], (i == 0 ? lblSkipTrig1 : lblSkipTrig2));
		}
		void cboSkipVar_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1);
			_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Variable = Common.Update(this, _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Variable, Convert.ToByte(cboSkipVar.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i], (i == 0 ? lblSkipTrig1 : lblSkipTrig2));
		}
		void cboSkipAmount_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == getHighlightColor() ? 0 : 1);
			_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Amount = Common.Update(this, _mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i].Amount, Convert.ToByte(cboSkipAmount.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].SkipToOrder4Trigger[i], (i == 0 ? lblSkipTrig1 : lblSkipTrig2));
		}

		void grpRole_Leave(object sender, EventArgs e)
		{
			ComboBox[] teams = new ComboBox[4];
			teams[0] = cboRoleTeam1;
			teams[1] = cboRoleTeam2;
			teams[2] = cboRoleTeam3;
			teams[3] = cboRoleTeam4;
			ComboBox[] roles = new ComboBox[4];
			roles[0] = cboRole1;
			roles[1] = cboRole2;
			roles[2] = cboRole3;
			roles[3] = cboRole4;
			string s;
			for (int i = 0; i < 4; i++)
			{
				s = getRoleString(teams[i], roles[i]);
				_mission.FlightGroups[_activeFG].Roles[i] = Common.Update(this, _mission.FlightGroups[_activeFG].Roles[i], s);
			}
		}

		void numExplode_ValueChanged(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ExplosionTime = (byte)numExplode.Value;
		}
		void numOptWaves_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves = Common.Update(this, _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfWaves, Convert.ToByte(numOptWaves.Value - 1));
			optCraftLabelRefresh();
		}
		void numOptCraft_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft = Common.Update(this, _mission.FlightGroups[_activeFG].OptCraft[_activeOptionCraft].NumberOfCraft, Convert.ToByte(numOptCraft.Value));  //[JB] Fixed, no -1.  Take value exactly as is.
			optCraftLabelRefresh();
		}
		#endregion
		#region Unknowns
		void grpUnkAD_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Unknowns.Unknown5 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown5, Convert.ToByte(numUnk5.Value));
		}
		void grpUnkCraft_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Unknowns.Unknown1 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown1, Convert.ToByte(numUnk1.Value));
		}
		void grpUnkOrder_Leave(object sender, EventArgs e)
		{
			numUnkOrder_Enter("grpUnkOrder_Leave", new EventArgs());
		}
		void grpUnkOther_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Unknowns.Unknown17 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown17, chkUnk17.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown18 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown18, chkUnk18.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown22 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown22, chkUnk22.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown23 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown23, chkUnk23.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown24 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown24, chkUnk24.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown25 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown25, chkUnk25.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown26 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown26, chkUnk26.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown27 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown27, chkUnk27.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown28 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown28, chkUnk28.Checked);
			_mission.FlightGroups[_activeFG].Unknowns.Unknown29 = Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown29, chkUnk29.Checked);
		}

		void numUnkOrder_Enter(object sender, EventArgs e)
		{
			int i = (int)numUnkOrder.Value - 1;
			_mission.FlightGroups[_activeFG].Orders[i].Unknown6 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[i].Unknown6, Convert.ToByte(numUnk6.Value));
			_mission.FlightGroups[_activeFG].Orders[i].Unknown7 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[i].Unknown7, Convert.ToByte(numUnk7.Value));
			_mission.FlightGroups[_activeFG].Orders[i].Unknown8 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[i].Unknown8, Convert.ToByte(numUnk8.Value));
			_mission.FlightGroups[_activeFG].Orders[i].Unknown9 = Common.Update(this, _mission.FlightGroups[_activeFG].Orders[i].Unknown9, Convert.ToByte(numUnk9.Value));
		}
		void numUnkOrder_ValueChanged(object sender, EventArgs e)
		{
			int i = (int)numUnkOrder.Value - 1;
			numUnk6.Value = _mission.FlightGroups[_activeFG].Orders[i].Unknown6;
			numUnk7.Value = _mission.FlightGroups[_activeFG].Orders[i].Unknown7;
			numUnk8.Value = _mission.FlightGroups[_activeFG].Orders[i].Unknown8;
			numUnk9.Value = _mission.FlightGroups[_activeFG].Orders[i].Unknown9;
		}
		#endregion
		#endregion
		#region Messages
		void newMess()
		{
			if (_mission.Messages.Count == Mission.MessageLimit)
			{
				MessageBox.Show("Mission contains maximum number of Messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			_activeMessage = _mission.Messages.Add();
			if (_mission.Messages.Count == 1) enableMessages(true);
			lstMessages.Items.Add(_mission.Messages[_activeMessage].MessageString);
			lstMessages.SelectedIndex = _activeMessage;
			Common.Title(this, _loading);
		}
		void deleteMess()
		{
			if (_activeMessage < 0 || _activeMessage >= _mission.Messages.Count)  //[JB] Added check
				return;
			lstMessages.Items.RemoveAt(_activeMessage); //[JB] Need to delete from list before _activeMessage changes, otherwise it may remove the wrong index.
			_activeMessage = _mission.Messages.RemoveAt(_activeMessage);
			if (_mission.Messages.Count == 0)
			{
				lstMessages.Items.Clear();
				enableMessages(false);
				lblMessage.Text = "Message #0 of 0";
				return;
			}
			lstMessages.SelectedIndex = _activeMessage;
			Common.Title(this, _loading);
		}
		void enableMessages(bool state)
		{
			grpMessages.Enabled = state;
			txtMessage.Enabled = state;
			txtShort.Enabled = state;
			grpSend.Enabled = state;
			numMessDelay.Enabled = state;
			cboMessTrig.Enabled = state;
			cboMessType.Enabled = state;
			cboMessVar.Enabled = state;
			cboMessAmount.Enabled = state;
			cboMessColor.Enabled = state;
		}
		void messListRefresh()
		{
			if (_mission.Messages.Count == 0) return;
			lstMessages.Items[_activeMessage] = _mission.Messages[_activeMessage].MessageString != "" ? _mission.Messages[_activeMessage].MessageString : " *";
			lstMessages.Invalidate(lstMessages.GetItemRectangle(_activeMessage)); //[JB] Force refresh if color changed
		}
		void swapMessage(int srcIndex, int dstIndex)
		{
			if ((srcIndex < 0 || srcIndex >= _mission.Messages.Count) || (dstIndex < 0 || dstIndex >= _mission.Messages.Count) || srcIndex == dstIndex) return;
			Platform.Xvt.Message tmp = _mission.Messages[srcIndex];
			_mission.Messages[srcIndex] = _mission.Messages[dstIndex];
			_mission.Messages[dstIndex] = tmp;
			messListRefresh();
			_activeMessage = dstIndex;
			messListRefresh();
			lstMessages.SelectedIndex = dstIndex;
		}

		void lstMessages_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (_mission.Messages.Count == 0 || _mission.Messages[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch (_mission.Messages[e.Index].Color)
			{
				case 0:
					brText = Brushes.Red; //Crimson;
					break;
				case 1:
					brText = Brushes.Lime; //LimeGreen;
					break;
				case 2:
					brText = Brushes.DodgerBlue; //RoyalBlue;
					break;
				case 3:
					brText = Brushes.Yellow;
					break;
			}
			e.Graphics.DrawString(lstMessages.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMessages.SelectedIndex == -1) return;
			_activeMessage = lstMessages.SelectedIndex;
			lblMessage.Text = "Message #" + (_activeMessage + 1).ToString() + " of " + _mission.Messages.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			for (int i = 0; i < 4; i++) labelRefresh(_mission.Messages[_activeMessage].Triggers[i], lblMessTrig[i]);
			txtMessage.Text = _mission.Messages[_activeMessage].MessageString;
			cboMessColor.SelectedIndex = _mission.Messages[_activeMessage].Color;
			optMess1OR2.Checked = _mission.Messages[_activeMessage].T1AndOrT2;
			optMess1AND2.Checked = !optMess1OR2.Checked;
			optMess3OR4.Checked = _mission.Messages[_activeMessage].T3AndOrT4;
			optMess3AND4.Checked = !optMess3OR4.Checked;
			optMess12OR34.Checked = _mission.Messages[_activeMessage].T12AndOrT34;
			optMess12AND34.Checked = !optMess12OR34.Checked;
			txtShort.Text = _mission.Messages[_activeMessage].Note;
			numMessDelay.Value = _mission.Messages[_activeMessage].Delay * 5;
			for (int i = 0; i < 10; i++) chkSendTo[i].Checked = _mission.Messages[_activeMessage].SentToTeam[i];
			lblMessTrigArr_Click(0, new EventArgs());
			_loading = btemp;

			cmdMoveMessUp.Enabled = (_mission.Messages.Count > 1 && _activeMessage > 0);
			cmdMoveMessDown.Enabled = (_mission.Messages.Count > 1 && _activeMessage < _mission.Messages.Count - 1);
		}

		void chkSendToArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			_mission.Messages[_activeMessage].SentToTeam[(int)c.Tag] = Common.Update(this, _mission.Messages[_activeMessage].SentToTeam[(int)c.Tag], c.Checked);
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
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 4; i++) if (i != _activeMessageTrigger) setInteractiveLabelColor(lblMessTrig[i], false);
			bool btemp = _loading;
			_loading = true;
			cboMessTrig.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Condition;
			cboMessType.SelectedIndex = -1;
			cboMessType.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].VariableType;
			cboMessAmount.SelectedIndex = _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Amount;
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

		void cboMessAmount_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Amount = Common.Update(this, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Amount, Convert.ToByte(cboMessAmount.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
		}
		void cboMessColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_mission.Messages[_activeMessage].Color = Common.Update(this, _mission.Messages[_activeMessage].Color, Convert.ToByte(cboMessColor.SelectedIndex));
			messListRefresh();
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
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);  //[JB] Removed _loading check.  Refresh the labels on demand, like the other platforms.
		}
		void cboMessVar_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Variable = Common.Update(this, _mission.Messages[_activeMessage].Triggers[_activeMessageTrigger].Variable, Convert.ToByte(cboMessVar.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[_activeMessageTrigger], lblMessTrig[_activeMessageTrigger]);
		}

		void numMessDelay_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Delay = Common.Update(this, _mission.Messages[_activeMessage].Delay, Convert.ToByte(numMessDelay.Value / 5));
		}

		void optMess1OR2_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.Messages[_activeMessage].T1AndOrT2 = Common.Update(this, _mission.Messages[_activeMessage].T1AndOrT2, optMess1OR2.Checked);
		}
		void optMess12OR34_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.Messages[_activeMessage].T12AndOrT34 = Common.Update(this, _mission.Messages[_activeMessage].T12AndOrT34, optMess12OR34.Checked);
		}
		void optMess3OR4_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.Messages[_activeMessage].T3AndOrT4 = Common.Update(this, _mission.Messages[_activeMessage].T3AndOrT4, optMess3OR4.Checked);
		}

		void txtMessage_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].MessageString = Common.Update(this, _mission.Messages[_activeMessage].MessageString, txtMessage.Text);
			messListRefresh();
		}
		void txtShort_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Note = Common.Update(this, _mission.Messages[_activeMessage].Note, txtShort.Text);
		}

		void cmdMoveMessUp_Click(object sender, EventArgs e)
		{
			swapMessage(_activeMessage, _activeMessage - 1);
		}
		void cmdMoveMessDown_Click(object sender, EventArgs e)
		{
			swapMessage(_activeMessage, _activeMessage + 1);
		}
		#endregion
		#region Globals
		void cboGlobalTeam_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalTeam.SelectedIndex == -1) return;
			_activeTeam = (byte)cboGlobalTeam.SelectedIndex;
			lblTeamArr_Click(lblTeam[_activeTeam], new EventArgs());    // link the Globals and Team tabs to share GlobTeam
			bool btemp = _loading;
			_loading = true;
			for (int i = 0; i < 12; i++) labelRefresh(_mission.Globals[_activeTeam].Goals[i / 4].Triggers[i % 4].GoalTrigger, lblGlobTrig[i]);
			for (int i = 0; i < 9; i++)
			{
				optGlobAndOr[i].Checked = _mission.Globals[_activeTeam].Goals[i / 3].AndOr[i % 3];  // OR
				optGlobAndOr[i + 9].Checked = !optGlobAndOr[i].Checked; // AND
			}
			lblGlobTrigArr_Click(0, new EventArgs());
			_loading = btemp;
		}

		void lblGlobTrigArr_Click(object sender, EventArgs e)
		{
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
			bool btemp = _loading;
			_loading = true;
			int g = _activeGlobalTrigger / 4;
			int t = _activeGlobalTrigger % 4;
			cboGlobalTrig.SelectedIndex = _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Condition;
			cboGlobalType.SelectedIndex = -1;
			cboGlobalType.SelectedIndex = _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.VariableType;
			cboGlobalAmount.SelectedIndex = _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Amount;
			numGlobalPoints.Value = _mission.Globals[_activeTeam].Goals[g].Points;
			txtGlobalInc.Text = _mission.Globals[_activeTeam].Goals[g].Triggers[t][Globals.GoalState.Incomplete];
			txtGlobalComp.Text = _mission.Globals[_activeTeam].Goals[g].Triggers[t][Globals.GoalState.Complete];
			txtGlobalFail.Text = _mission.Globals[_activeTeam].Goals[g].Triggers[t][Globals.GoalState.Failed];
			txtGlobalFail.Visible = (g < (int)Globals.GoalIndex.Prevent);
			txtGlobalInc.Visible = (g < (int)Globals.GoalIndex.Secondary);
			labelRefresh(_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger, lblGlobTrig[_activeGlobalTrigger]);
			_loading = btemp;
		}
		void lblGlobTrigArr_DoubleClick(object sender, EventArgs e)
		{
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
			int g = (int)r.Tag / 3, i = (int)r.Tag % 3;
			_mission.Globals[_activeTeam].Goals[g].AndOr[i] = Common.Update(this, _mission.Globals[_activeTeam].Goals[g].AndOr[i], r.Checked);
		}

		void cboGlobalAmount_Leave(object sender, EventArgs e)
		{
			int g = _activeGlobalTrigger / 4, t = _activeGlobalTrigger % 4;
			_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Amount = Common.Update(this, _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Amount, Convert.ToByte(cboGlobalAmount.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger, lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalTrig_Leave(object sender, EventArgs e)
		{
			int g = _activeGlobalTrigger / 4, t = _activeGlobalTrigger % 4;
			_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Condition = Common.Update(this, _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Condition, Convert.ToByte(cboGlobalTrig.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger, lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalType.SelectedIndex == -1) return;
			int g = _activeGlobalTrigger / 4, t = _activeGlobalTrigger % 4;
			if (!_loading)
				_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.VariableType = Common.Update(this, _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.VariableType, Convert.ToByte(cboGlobalType.SelectedIndex));
			comboVarRefresh(_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.VariableType, cboGlobalVar);
			try { cboGlobalVar.SelectedIndex = _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Variable; }
			catch { cboGlobalVar.SelectedIndex = 0; _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Variable = 0; }
			if (!_loading) labelRefresh(_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger, lblGlobTrig[_activeGlobalTrigger]);
		}
		void cboGlobalVar_Leave(object sender, EventArgs e)
		{
			int g = _activeGlobalTrigger / 4, t = _activeGlobalTrigger % 4;
			_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Variable = Common.Update(this, _mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger.Variable, Convert.ToByte(cboGlobalVar.SelectedIndex));
			labelRefresh(_mission.Globals[_activeTeam].Goals[g].Triggers[t].GoalTrigger, lblGlobTrig[_activeGlobalTrigger]);
		}

		void numGlobalPoints_Leave(object sender, EventArgs e)
		{
			_mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Points = Common.Update(this, _mission.Globals[_activeTeam].Goals[_activeGlobalTrigger / 4].Points, (short)numGlobalPoints.Value);
		}

		void txtGlobal_Leave(object sender, EventArgs e)
		{
			int g = _activeGlobalTrigger / 4, t = _activeGlobalTrigger % 4;
			Globals.GoalState gs = (Globals.GoalState)((TextBox)sender).Tag;
			_mission.Globals[_activeTeam].Goals[g].Triggers[t][gs] = Common.Update(this, _mission.Globals[_activeTeam].Goals[g].Triggers[t][gs], ((TextBox)(sender)).Text);
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

			//[JB] Update the role team selection dropdowns with the team names
			ComboBox[] cboRole = new ComboBox[4];
			cboRole[0] = cboRoleTeam1;
			cboRole[1] = cboRoleTeam2;
			cboRole[2] = cboRoleTeam3;
			cboRole[3] = cboRoleTeam4;

			chkAllies[_activeTeam].Text = team;

			//[JB] Update the Briefing's list of team names.
			for (int i = 0; i < _mission.Teams.Count; i++)
				BriefingForm.SharedTeamNames[i] = _mission.Teams[i].Name;

			if (_activeTeam >= 0 && _activeTeam < 4)    //Each role only displays 4 teams beginning at index[1]
				for (int i = 0; i < 4; i++)
					cboRole[i].Items[1 + _activeTeam] = team;

			if (_activeTeam >= 0 && _activeTeam < 8)   //8 teams in the Radio list beginning at index[1]
				cboRadio.Items[1 + _activeTeam] = team;

			lstGoalTeams.Items[_activeTeam] = team;            //FG goal team selector.
		}

		void cboEoMColorArr_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox c = (ComboBox)sender;
			if (!_loading)
				_mission.Teams[_activeTeam].EndOfMissionMessageColor[(int)c.Tag] = Common.Update(this, _mission.Teams[_activeTeam].EndOfMissionMessageColor[(int)c.Tag], Convert.ToByte(c.SelectedIndex));
			Color clr = Color.Crimson;
			if (c.SelectedIndex == 1) clr = Color.LimeGreen;
			else if (c.SelectedIndex == 2) clr = Color.RoyalBlue;
			else if (c.SelectedIndex == 3) clr = Color.Yellow;
			txtEoM[(int)c.Tag].ForeColor = clr;
		}
		void chkAlliesArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			_mission.Teams[_activeTeam].AlliedWithTeam[(int)c.Tag] = Common.Update(this, _mission.Teams[_activeTeam].AlliedWithTeam[(int)c.Tag], c.Checked);
		}

		void lblTeamArr_Click(object sender, EventArgs e)
		{
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				//[JB] Modified this code to make it more like the XWA code.  The former code ran slow in the debugger due to exceptions, and performed redundant value assignments that were already handled elsewhere in various Leave() events.  Also modified all functions that manually call this event to provide a default Label object instead of relying on exceptions.
				//     Removed these assignments:  txtTeamName_Leave, txtEoMArr_Leave, cboEoMColorArr_SelectedIndexChanged, and chkAlliesArr_Leave already process the modifications.
				_activeTeam = Convert.ToByte(l.Tag);
			}   // fired by click
			catch (InvalidCastException) { _activeTeam = Convert.ToByte(sender); l = lblTeam[_activeTeam]; }    // fired by code
			if (l == null) return;
			cboGlobalTeam.SelectedIndex = _activeTeam;
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 10; i++) if (i != _activeTeam) setInteractiveLabelColor(lblTeam[i], false);
			bool btemp = _loading;
			_loading = true;
			txtTeamName.Text = _mission.Teams[_activeTeam].Name;
			for (int i = 0; i < 10; i++) chkAllies[i].Checked = _mission.Teams[_activeTeam].AlliedWithTeam[i];
			for (int i = 0; i < 6; i++)
			{
				txtEoM[i].Text = _mission.Teams[_activeTeam].EndOfMissionMessages[i];
				cboEoMColor[i].SelectedIndex = _mission.Teams[_activeTeam].EndOfMissionMessageColor[i];
			}
			_loading = btemp;
		}
		void lblTeamArr_DoubleClick(object sender, EventArgs e)
		{
			menuPaste_Click("Team", new EventArgs());
		}
		void lblTeamArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Team", new EventArgs());
		}

		void txtEoMArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.Teams[_activeTeam].EndOfMissionMessages[(int)t.Tag] = Common.Update(this, _mission.Teams[_activeTeam].EndOfMissionMessages[(int)t.Tag], t.Text);
		}

		void txtTeamName_Leave(object sender, EventArgs e)
		{
			_mission.Teams[_activeTeam].Name = Common.Update(this, _mission.Teams[_activeTeam].Name, txtTeamName.Text);
			teamRefresh();
		}
		#endregion
		#region Mission
		void cboMissType_Leave(object sender, EventArgs e)
		{
			_mission.MissionType = Common.Update(this, _mission.MissionType, (Mission.MissionTypeEnum)Convert.ToByte(cboMissType.SelectedIndex));
		}

		void chkMissUnk3_Leave(object sender, EventArgs e)
		{
			_mission.Unknown3 = Common.Update(this, _mission.Unknown3, chkMissUnk3.Checked);
		}
		void chkPreventOutcome_Leave(object sender, EventArgs e)
		{
			_mission.PreventMissionOutcome = Common.Update(this, _mission.PreventMissionOutcome, chkPreventOutcome.Checked);
		}
		private void numRndSeed_ValueChanged(object sender, EventArgs e)
		{
			_mission.RndSeed = Common.Update(this, _mission.RndSeed, Convert.ToByte(numRndSeed.Value));
		}
		void numMissTimeMin_Leave(object sender, EventArgs e)
		{
			_mission.TimeLimitMin = Common.Update(this, _mission.TimeLimitMin, Convert.ToByte(numMissTimeMin.Value));
		}
		void numMissTimeSec_Leave(object sender, EventArgs e)
		{
			_mission.TimeLimitSec = Common.Update(this, _mission.TimeLimitSec, Convert.ToByte(numMissTimeSec.Value));
		}
		void numMissUnk1_Leave(object sender, EventArgs e)
		{
			_mission.Unknown1 = Common.Update(this, _mission.Unknown1, Convert.ToByte(numMissUnk1.Value));
		}
		void numMissUnk2_Leave(object sender, EventArgs e)
		{
			_mission.Unknown2 = Common.Update(this, _mission.Unknown2, Convert.ToByte(numMissUnk2.Value));
		}

		void optXvT_CheckedChanged(object sender, EventArgs e)
		{
			setBop(!optXvT.Checked);
			Common.Title(this, _loading);
		}

		void txtIFFArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.IFFs[(int)t.Tag] = Common.Update(this, _mission.IFFs[(int)t.Tag], t.Text);
			comboReset(cboIFF, getIffStrings(), cboIFF.SelectedIndex);
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
		#endregion
	}
}