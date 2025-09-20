/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2025 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.17.1+
 *
 * CHANGELOG
 * [NEW] Officer facial expressions
 * v1.17.1, 250303
 * [FIX] Stripping extension to write into Battle for Testing was case-sensitive
 * v1.17, 250215
 * [FIX] Test now marks the battle:TEXT as Dirty.
 * [FIX] Increased sleep after launching TIE during test before initial run check to preven premature cleanup.
 * v1.16.0.3, 241027
 * [FIX #110] FG library callback cast exception
 * v1.16, 241013
 * [FIX] Special Cargo control visibility when not on first tab
 * [FIX] SaveAs behavior
 * [UPD] Spec updates (TimeLimit, RndSeed, Vars, WinBonus, Rescue, EomDelay, GlobalDelay, WaveDelay, ArrDep renames)
 * [UPD] IFF5 (Red) Hostile permanently checked
 * [FIX] Questions not clearing properly when loading a second mission
 * [FIX] Test now respects TieDetectMission setting
 * v1.15.5, 231222
 * [NEW #97] GlobalSummary dialog
 * [UPD] SaveAs upgrades now use Platform
 * v1.13.12, 230116
 * [NEW] RememberSelectedOrder option functionality
 * v1.13.11, 221030
 * [FIX] Open dialog not following current directory after switching paltforms via "Open Recent"
 * v1.13.10, 221018
 * [DEL #73] Captured on Ejection options
 * [DEL #74] Secret Goals
 * [UPD #77] comboVarRefresh updated to Misc to Rating, Status, and All
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
 * [UPD] Unused messages drawn in gray
 * [FIX] Listbox scrolling
 * v1.11.2, 2101005
 * [FIX] Pasting a message when at capacity now correctly does nothing
 * [UPD] Copy/paste now uses system clipboard, can more easily paste external text
 * [NEW] Copy/paste now works for Waypoints, can paste XvT/XWA Triggers/Orders
 * v1.10, 210520
 * [UPD #56] Replaced try/catch with TryParse [JB]
 * v1.9.2, 210328
 * [FIX] Test load failure if mission isn't in platform directory
 * v1.9, 210108
 * [FIX] Clipboard path in some locations
 * v1.8.1, 201213
 * [FIX] Double Verify
 * [UPD] _config passed to Officer form, Backdrops, RunVerify()
 * [UPD #20] Test function now attempts to detect platform from MissionPath
 * [UPD] menuTest moved under Tools, changed to &Test
 * v1.8, 201004
 * [FIX] Deactivate added to force focus fix [JB]
 * [FIX] Test launching if you cancel the intial Save
 * [FIX] SaveAs for XvT and XWA fixed
 * [NEW] SaveAs BoP
 * [UPD] saveMission now won't save/rewrite file if unmodifed
 * [NEW] FlightGroupLibrary [JB]
 * v1.7, 200816
 * [UPD] newFG() is now bool return
 * [FIX] recalculateEditorCraftNumbering() handles _activeFG now [JB]
 * [UPD] shiplist and Map calls updated for Wireframe implementation [JB]
 * [UPD] Blank messages now shown as "*" [JB]
 * [UPD] Cleanup index substitions [JB]
 * [UPD] Trigger label refresh updates [JB]
 * [FIX] Extra protections to handle custom missions using "bad" Status or Formation values [JB]
 * [FIX] Current FG's IFF would reset when changing IFF names
 * [FIX] Message color updates right away
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
 * [NEW] cbo's for messages [JB]
 * [NEW] Dictionaries for Control handling [JB]
 * [FIX] comboReset now has index check [JB]
 * [NEW] Xwing support [JB]
 * [FIX] non-TIE filestreams now closed properly [JB]
 * [NEW] colorized cbo's [JB]
 * [UPD] lots of controls converted to instant-update instead of using _Leave. Done by pointing handlers to _Leave instead of redoing it [JB]
 * [UPD] IFF listing in cbo's use defaults if blank via getIffStrings() [JB]
 * [UPD] better tab updates [JB]
 * [NEW] 'Enter' key trigger control update [JB]
 * [UPD] copy/paste expanded [JB]
 * [UPD] lbl colors now applied via function to allow themeing [JB]
 * [UPD] delete FG reworked [JB]
 * [UPD] general performance improvements [JB]
 * [NEW] Permadeath unknowns implemented [JB]
 * [NEW] blank messages are shown [JB]
 * [FIX] exception in lblMessArr if no messages [JB]
 * [NEW] cmdMoveMessUp/Down and cmdMoveFGUp/Down [JB]
 * [FIX] Officer Preview can only have one window now [JB]
 * [NEW] Best Fit function for Officer questions and length check [JB]
 * [NEW] EoM text boxes now display per color setting [JB]
 * [NEW] note lbl's for Orders, officer questions [JB]
 * [NEW] editor-only craft numbering [JB]
 * [UPD] improved order information [JB]
 * v1.4.3, 180509
 * [UPD] changed how Strings.OrderDesc gets split
 * v1.4.1, 171118
 * [UPD] added Exclamation icon to FG delete confirmation
 * v1.4, 171016
 * [NEW #10] Custom ship list loading
 * v1.3, 170107
 * [NEW] FG Goal Summary [JB]
 * [FIX] crash fixes [JB]
 * [NEW] MRU capability [JB]
 * [NEW] Delete menu item, delete key capture [JB]
 * [FIX] Redo opnTIE procedure [JB]
 * [FIX] copy/paste trigger failures [JB]
 * [NEW] Craft reference adjustment when deleting FGs [JB]
 * [FIX] catch blocks added to prevent crashes [JB]
 * [FIX] Global And/Or goal assignments [JB]
 * [UPD] Changing briefing officer reset to first question [JB]
 * v1.2.8, 160606
 * [FIX] Test now initially opens key RO (UAC's fault)
 * [FIX] WaitForExit in Test replaced with named process check loop (Steam's fault)
 * [UPD] Test explorer kill in Win7 now omits Steam version
 * v1.2.5, 150110
 * [UPD] modified Common.Update calls for generics
 * v1.2.3, 141214
 * [UPD] change to MPL
 * [FIX] crash trying to use BattleForm when TIE isn't installed
 * v1.2, 121006
 * - Settings passed in and out
 * [NEW] Test menu
 * - comboReset() to static
 * [UPD] lblStarting now only applies to Normal difficulty
 * [UPD] opn/sav dialogs default to \MISSION
 * [NEW] Open Recent menu
 * v1.1.1, 120814
 * [UPD] chkWPArr_Leave to chkWPArr_CheckedChanged
 * [NEW] FG.Unknowns 19-21
 * - renamed a ton of stuff
 * - class renamed
 * v1.0, 110921
 * - Release
 */

using System;
using System.Data;	// DataView and DataTable
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Idmr.Platform.Tie;
using Idmr.LfdReader;

namespace Idmr.Yogeme
{
	/// <summary>TIE95 Mission Editor GUI</summary>
	public partial class TieForm : Form
	{
		#region vars and stuff
		readonly Settings _config = Settings.GetInstance();
		Mission _mission;
		bool _applicationExit;              //for frmTIE_Closing, confirms application exit vs switching platforms
		int _activeFGIndex = 0;          //counter to keep track of current FG being displayed
		int _startingShips = 1;     //counter for craft in play at start <30s, warning above 28
		bool _loading;      //alerts certain functions to disable during the loading process
		bool _noRefresh = false;
		int _activeMessageIndex = 0;
		readonly DataTable _table = new DataTable("Waypoints");
		readonly DataTable _tableRaw = new DataTable("Waypoints_Raw");
		MapForm _fMap;
		BriefingForm _fBrief;
		FlightGroupLibraryForm _fLibrary;
		BattleForm _fBattle;
		OfficerPreviewForm _fOfficers;
		byte _activeGlobalGoalIndex;
		byte _activeArrDepTriggerIndex;
		byte _activeOrderIndex;
		byte _activeMessageTrigIndex;
		#endregion
		#region Control Arrays
#pragma warning disable IDE1006 // Naming Styles
		readonly Label[] lblGlob = new Label[6];
		readonly ComboBox[] cboEoMColor = new ComboBox[6];  //[JB] Added color dropdowns for TIE.
		readonly TextBox[] txtEoM = new TextBox[6];
		readonly CheckBox[] chkIFF = new CheckBox[6];
		readonly TextBox[] txtIFF = new TextBox[6];
		readonly Label[] lblADTrig = new Label[3];
		readonly ComboBox[] cboGoal = new ComboBox[8];
		readonly Label[] lblOrder = new Label[3];
		readonly CheckBox[] chkWP = new CheckBox[15];
		readonly RadioButton[] optOfficers = new RadioButton[4];
		readonly MenuItem[] menuRecentMissions = new MenuItem[6];
		readonly Dictionary<ComboBox, ComboBox> colorizedFGList = new Dictionary<ComboBox, ComboBox>();  //[JB] Maps a control that should have a colorized FG list with a control that determines whether the list actually contains a FG list.
#pragma warning restore IDE1006 // Naming Styles
		#endregion

		/// <summary>Initialize with a blank mission.</summary>
		/// <param name="settings"></param>
		public TieForm()
		{
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		/// <summary>Initialize with an existing mission.</summary>
		/// <param name="settings"></param>
		/// <param name="path"></param>
		/// <remarks>This is for command line and "Open..." support.</remarks>>
		public TieForm(string path)
		{
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
			try { _fBattle.Close(); }
			catch { /* do nothing */ }
			try { _fOfficers.Close(); }
			catch { /* do nothing */ }
			try { _fLibrary.Close(); }
			catch { /* do nothing */ }
		}
		void comboVarRefresh(int varType, ComboBox cbo)
		{
			if (varType == -1) return;
			cbo.Items.Clear();
			switch ((Mission.Trigger.TypeList)varType)
			{
				case Mission.Trigger.TypeList.None:
				case Mission.Trigger.TypeList.AllCraft:
					cbo.Items.Add("None");
					break;
				case Mission.Trigger.TypeList.FlightGroup:
					cbo.Items.AddRange(_mission.FlightGroups.GetList());
					break;
				case Mission.Trigger.TypeList.ShipType:
					cbo.Items.AddRange(Strings.CraftType);
					cbo.Items.RemoveAt(0);
					break;
				case Mission.Trigger.TypeList.ShipClass:
					cbo.Items.AddRange(Strings.ShipClass);
					break;
				case Mission.Trigger.TypeList.ObjectType:
					cbo.Items.AddRange(Strings.ObjectType);
					break;
				case Mission.Trigger.TypeList.IFF:
					cbo.Items.AddRange(getIffStrings());
					break;
				case Mission.Trigger.TypeList.ShipOrders:
					cbo.Items.AddRange(Strings.Orders);
					break;
				case Mission.Trigger.TypeList.CraftWhen:
					cbo.Items.AddRange(Strings.CraftWhen);
					break;
				case Mission.Trigger.TypeList.AILevel:
					cbo.Items.AddRange(Strings.Rating);
					break;
				case Mission.Trigger.TypeList.Status:
					cbo.Items.AddRange(Strings.Status);
					break;
				//case 8: Global Group
				default:
					string[] temp = new string[256];
					for (int i = 0; i <= 255; i++) temp[i] = i.ToString();
					cbo.Items.AddRange(temp);
					break;
			}
		}
		static void comboReset(ComboBox cbo, string[] items, int index)
		{
			if (index < -1 || index >= items.Length) index = -1;
			cbo.Items.Clear();
			cbo.Items.AddRange(items);
			cbo.SelectedIndex = index;
		}
		string composeGoalString(string fgName, int amount, int condition)
		{
			if (condition <= 0 || condition == 10 || condition >= Strings.Trigger.Length) return "";
			if (amount < 0 || amount >= Strings.GoalAmount.Length) return "";  //Don't know if/why these would be invalid, but just to be safe.

			return Strings.GoalAmount[amount] + " " + fgName + " must " + Strings.Trigger[condition];
		}
		void craftStart(FlightGroup fg, bool bAdd)
		{
			if (fg.Difficulty == Platform.BaseFlightGroup.Difficulties.Easy || fg.Difficulty == Platform.BaseFlightGroup.Difficulties.Hard || !fg.ArrivesIn30Seconds) return;

			if (bAdd) _startingShips += fg.NumberOfCraft;
			else _startingShips -= fg.NumberOfCraft;
			lblStarting.Text = _startingShips.ToString() + " Craft at 30 seconds";
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
					brText = Brushes.Lime;  //LimeGreen;  //[JB] I changed the colors by request to make them look closer to the colors used in game.
					break;
				case 1:
					brText = Brushes.Red;  //Crimson;
					break;
				case 2:
					brText = Brushes.DodgerBlue;  //RoyalBlue;
					break;
				case 3:
					brText = Brushes.MediumOrchid; //DarkOrchid;
					break;
				case 4:
					brText = Brushes.OrangeRed; //Red;
					break;
				case 5:
					brText = Brushes.DarkOrchid; //Fuchsia;
					break;
			}
			if (_mission.FlightGroups[fgIndex].Difficulty == Platform.BaseFlightGroup.Difficulties.Never)
				brText = Brushes.Gray;

			return brText;
		}
		Color getHighlightColor() => _config.ColorInteractSelected;
		/// <summary>Generates a string list of IFF names which provide default names instead of an empty string when no custom names are defined</summary>
		string[] getIffStrings()
		{
			string[] t = new string[_mission.IFFs.Length];
			for (int i = 0; i < t.Length; i++)
			{
				t[i] = _mission.IFFs[i];
				if (t[i] == "") t[i] = Strings.IFF[i];
			}
			return t;
		}
		/// <summary>Helper function to detect focus inside control arrays</summary>
		/// <returns><b>true</b> if any Control is Focused</returns>
		bool hasFocus(Label[] list)
		{
			foreach (Label c in list) if (c.Focused) return true;
			return false;
		}
		void initializeMission()
		{
			tabMain.Focus();            // Exit focus from any form controls.  Fixes some issues that might arise from Leave() events trying to access modified lists.
			lstFG.Items.Clear();        // Clearing FGs here prevents issues with ComboBoxes further down.
			lstMessages.Items.Clear();  // Clearing messages to reset the move buttons.
			Common.UpdateMoveButtons(cmdMoveMessUp, cmdMoveMessDown, lstMessages);
			_mission = new Mission();
			_config.LastMission = "";
			_activeFGIndex = 0;
			_activeMessageIndex = 0;
			_mission.FlightGroups[0].CraftType = Convert.ToByte(_config.TieCraft);
			_mission.FlightGroups[0].IFF = Convert.ToByte(_config.TieIff);
			_startingShips = 0;
			craftStart(_mission.FlightGroups[0], true);
			string[] fgList = _mission.FlightGroups.GetList();
			comboReset(cboArrMS, fgList, 0);
			comboReset(cboArrMSAlt, fgList, 0);
			comboReset(cboDepMS, fgList, 0);
			comboReset(cboDepMSAlt, fgList, 0);
			lstFG.Items.Add(_activeFG.ToString(true));
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			comboReset(cboIFF, getIffStrings(), 0);
			Text = "Ye Olde Galactic Empire Mission Editor - TIE - New Mission.tie";
		}
		void loadCraftData(string fileMission)
		{
			Strings.OverrideShipList(null, null); //Restore defaults.
			try
			{
				CraftDataManager.GetInstance().LoadPlatform(Settings.Platform.TIE, Strings.CraftType, Strings.CraftAbbrv, fileMission);
				Strings.OverrideShipList(CraftDataManager.GetInstance().GetLongNames(), CraftDataManager.GetInstance().GetShortNames());
			}
			catch (Exception x) { MessageBox.Show("Error processing custom TIE ship list, using defaults.\n\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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
			closeForms();
			lstFG.Items.Clear();
			lstMessages.Items.Clear();
			_activeFGIndex = 0;
			_activeMessageIndex = 0;
			_startingShips = 0;
			try
			{
				FileStream fs = File.OpenRead(fileMission);
				try
				{
					#region determine platform
					switch (Platform.MissionFile.GetPlatform(fs))
					{
						case Platform.MissionFile.Platform.Xwing:
							_applicationExit = false;
							new XwingForm(fileMission).Show();
							Close();
							fs.Close();
							return false;
						case Platform.MissionFile.Platform.TIE:
							break;
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
							fs.Close();
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
				menuNewTIE_Click(0, new EventArgs());
				return false;
			}
			loadCraftData(fileMission);
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
				if (_mission.FlightGroups[i].ArrivesIn30Seconds) craftStart(_mission.FlightGroups[i], true);
			}
			updateFGList();
			if (_mission.Messages.Count == 0) enableMessage(false);
			else
			{
				enableMessage(true);
				for (int i = 0; i < _mission.Messages.Count; i++)
					lstMessages.Items.Add(_mission.Messages[i].MessageString != "" ? _mission.Messages[i].MessageString : " *");
			}
			bool btemp = _loading;
			_loading = true;
			comboReset(cboIFF, getIffStrings(), 0);
			updateMissionTabs();
			_loading = btemp;
			Text = "Ye Olde Galactic Empire Mission Editor - TIE - " + _mission.MissionFileName;
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
					if (_mission.MissionPath == "\\NewMission.tie") savTIE.ShowDialog();
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
			if (!_config.ColorizedDropDowns || variable == null) return;

			colorizedFGList[variable] = variableType;
			variable.DrawMode = DrawMode.OwnerDrawFixed;
			variable.DrawItem += colorizedComboBox_DrawItem;
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
		Mission.Trigger getTriggerFromControls(ComboBox amount, ComboBox varType, ComboBox var, ComboBox condition)
		{
			Mission.Trigger ret = new Mission.Trigger()
			{
				Amount = (byte)amount.SelectedIndex,
				VariableType = (byte)varType.SelectedIndex,
				Variable = (byte)var.SelectedIndex,
				Condition = (byte)condition.SelectedIndex
			};
			return ret;
		}
		string replaceTargetText(string text)
		{
			while (text.Contains("FG:"))
			{
				int fg = Common.ParseIntAfter(text, "FG:");
				text = text.Replace("FG:" + fg, (fg >= 0 && fg < _mission.FlightGroups.Count) ? _mission.FlightGroups[fg].ToString() : "");
			}
			while (text.Contains("IFF:"))
			{
				int iff = Common.ParseIntAfter(text, "IFF:");
				text = text.Replace("IFF:" + iff, "IFF " + Common.SafeString(getIffStrings(), iff, true));
			}
			return text;
		}
		void saveMission(string fileMission)
		{
			tabMain.Focus();
			try { _fBrief.Save(); }
			catch { /* do nothing */ }
			if (Text.IndexOf("*") == -1) return;	// don't save if unmodified

			try { _mission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			Text = "Ye Olde Galactic Empire Mission Editor - TIE - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
			refreshRecent();
			if (_config.Verify) Common.RunVerify(_mission.MissionPath);
		}
		void setInteractiveLabelColor(Label control, bool highlight)
		{
			control.ForeColor = highlight ? _config.ColorInteractSelected : _config.ColorInteractNonSelected;
			control.BackColor = _config.ColorInteractBackground;
		}
		void startup()
		{
			loadCraftData("");
			comboReset(cboIFF, getIffStrings(), 0);
			_config.LastMission = "";
			_config.LastPlatform = Settings.Platform.TIE;
			opnTIE.InitialDirectory = _config.GetWorkingPath();
			savTIE.InitialDirectory = _config.GetWorkingPath();
			_applicationExit = true;    //becomes false if selecting "New Mission" from menu
			#region Menu
			menuTest.Enabled = _config.TieInstalled;
			if (_config.RestrictPlatforms)
			{
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
			cboIFF.SelectedIndex = _mission.FlightGroups[0].IFF;    // already loaded default IFFs at start of function through txtIFF#.Text
			cboAI.Items.AddRange(Strings.Rating); cboAI.SelectedIndex = 3;
			cboMarkings.Items.AddRange(Strings.Color); cboMarkings.SelectedIndex = 0;
			cboPlayer.SelectedIndex = 0;
			cboFormation.Items.AddRange(Strings.Formation); cboFormation.SelectedIndex = 0;
			cboStatus.Items.AddRange(Strings.Status); cboStatus.SelectedIndex = 0;
			cboWarheads.Items.AddRange(Strings.Warheads); cboWarheads.SelectedIndex = 0;
			cboBeam.Items.AddRange(Strings.Beam); cboBeam.SelectedIndex = 0;
			#endregion
			#region Arr/Dep
			lblADTrig[0] = lblArr1;
			lblADTrig[1] = lblArr2;
			lblADTrig[2] = lblDep;
			for (int i = 0; i < 3; i++) lblADTrig[i].Tag = i;
			cboADTrig.Items.AddRange(Strings.Trigger); cboADTrig.SelectedIndex = 0;
			cboADTrigAmount.Items.AddRange(Strings.Amount); cboADTrigAmount.SelectedIndex = 0;
			cboADTrigType.Items.AddRange(Strings.VariableType); cboADTrigType.SelectedIndex = 0;
			cboAbort.Items.AddRange(Strings.Abort); cboAbort.SelectedIndex = 0;
			cboDiff.Items.AddRange(Strings.Difficulty); cboDiff.SelectedIndex = 0;
			#endregion
			#region Orders
			lblOrder[0] = lblOrder1;
			lblOrder[1] = lblOrder2;
			lblOrder[2] = lblOrder3;
			for (int i = 0; i < 3; i++)
			{
				lblOrder[i].Click += new EventHandler(lblOrderArr_Click);
				lblOrder[i].DoubleClick += new EventHandler(lblOrderArr_DoubleClick);
				lblOrder[i].MouseUp += new MouseEventHandler(lblOrderArr_MouseUp);
				lblOrder[i].Tag = i;
			}
			_activeOrderIndex = 0;
			cboOrders.Items.AddRange(Strings.Orders); cboOrders.SelectedIndex = 0;
			cboOT1Type.Items.AddRange(Strings.VariableType); cboOT1Type.SelectedIndex = 0;
			cboOT2Type.Items.AddRange(Strings.VariableType); cboOT2Type.SelectedIndex = 0;
			cboOT3Type.Items.AddRange(Strings.VariableType); cboOT3Type.SelectedIndex = 0;
			cboOT4Type.Items.AddRange(Strings.VariableType); cboOT4Type.SelectedIndex = 0;
			#endregion
			#region Waypoints
			_table.Columns.Add("X"); _table.Columns.Add("Y"); _table.Columns.Add("Z");
			_tableRaw.Columns.Add("X"); _tableRaw.Columns.Add("Y"); _tableRaw.Columns.Add("Z");
			for (int i = 0; i < 15; i++)    //initialize WPs
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
			_table.RowChanged += new DataRowChangeEventHandler(table_RowChanged);
			_tableRaw.RowChanged += new DataRowChangeEventHandler(tableRaw_RowChanged);
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
			chkWP[14] = chkWPBrief;
			for (int i = 0; i < 15; i++)
			{
				chkWP[i].CheckedChanged += new EventHandler(chkWPArr_CheckedChanged);
				chkWP[i].Tag = i;
			}
			#endregion
			#region Officers
			optOfficers[0] = optBoth;
			optOfficers[1] = optBoth;
			optOfficers[2] = optFO;
			optOfficers[3] = optSO;
			for (int i = 1; i < 4; i++)
			{
				optOfficers[i].Leave += new EventHandler(optOfficers_Leave);
				optOfficers[i].Tag = i;
			}
			#endregion
			#region FG Goals
			cboGoal[0] = cboPrimGoalT;
			cboGoal[1] = cboPrimGoalA;
			cboGoal[2] = cboSecGoalT;
			cboGoal[3] = cboSecGoalA;
			cboGoal[6] = cboBonGoalT;
			cboGoal[7] = cboBonGoalA;
			for (int i = 0; i < 4; i++)
			{
				if (i == 2) continue;
				cboGoal[i * 2].Items.AddRange(Strings.Trigger); cboGoal[i * 2].SelectedIndex = 10;
				cboGoal[i * 2 + 1].Items.AddRange(Strings.GoalAmount); cboGoal[i * 2 + 1].SelectedIndex = 0;
			}
			for (int i = 0; i < 8; i++)
			{
				if (i == 4 || i == 5) continue;
				cboGoal[i].Leave += new EventHandler(cboGoalArr_Leave);
				cboGoal[i].Tag = i;
			}
			#endregion
			#region Global
			lblGlob[0] = lblPrim1;
			lblGlob[1] = lblPrim2;
			lblGlob[2] = lblSec1;
			lblGlob[3] = lblSec2;
			lblGlob[4] = lblBon1;
			lblGlob[5] = lblBon2;
			for (int i = 0; i < 6; i++)
			{
				lblGlob[i].Click += new EventHandler(lblGlobArr_Click);
				lblGlob[i].DoubleClick += new EventHandler(lblGlobArr_DoubleClick);
				lblGlob[i].MouseUp += new MouseEventHandler(lblGlobArr_MouseUp);
				lblGlob[i].Tag = i;
			}
			_activeGlobalGoalIndex = 0;
			cboGlobalAmount.Items.AddRange(Strings.Amount);
			cboGlobalType.Items.AddRange(Strings.VariableType);
			cboGlobalTrig.Items.AddRange(Strings.Trigger);
			#endregion
			cboMessTrig.Items.AddRange(Strings.Trigger);
			cboMessAmount.Items.AddRange(Strings.Amount);
			cboMessType.Items.AddRange(Strings.VariableType);
			#region Officers
			cboOfficer.SelectedIndex = 0;
			cboQTrig.Enabled = false; cboQTrig.SelectedIndex = 0;
			cboQTrigType.Enabled = false; cboQTrigType.SelectedIndex = 0;
			#endregion
			#region Mission
			cboEoMColor[0] = cboPC1Color;
			cboEoMColor[1] = cboPC2Color;
			cboEoMColor[2] = cboSC1Color;
			cboEoMColor[3] = cboSC2Color;
			cboEoMColor[4] = cboPF1Color;
			cboEoMColor[5] = cboPF2Color;
			txtEoM[0] = txtPrimComp1;
			txtEoM[1] = txtPrimComp2;
			txtEoM[2] = txtSecComp1;
			txtEoM[3] = txtSecComp2;
			txtEoM[4] = txtPrimFail1;
			txtEoM[5] = txtPrimFail2;
			for (int i = 0; i < 6; i++)
			{
				cboEoMColor[i].SelectedIndexChanged += new EventHandler(cboEoMColorArr_SelectedIndexChanged);
				cboEoMColor[i].Tag = i;
				txtEoM[i].Leave += new EventHandler(txtEoMArr_Leave);
				txtEoM[i].Tag = i;
			}
			chkIFF[0] = chkIFF3;
			chkIFF[1] = chkIFF4;
			chkIFF[2] = chkIFF5;
			chkIFF[3] = chkIFF6;
			txtIFF[0] = txtIFF3;
			txtIFF[1] = txtIFF4;
			txtIFF[2] = txtIFF5;
			txtIFF[3] = txtIFF6;
			for (int i = 0; i < 4; i++)
			{
				if (i != 2) chkIFF[i].Leave += new EventHandler(chkIFFArr_Leave);
				chkIFF[i].Tag = i + 2;
				txtIFF[i].Leave += new EventHandler(txtIFFArr_Leave);
				txtIFF[i].Tag = i + 2;
			}
			#endregion
			updateMissionTabs();

			#region ControlRegistration
			Common.AddControlChangedHandler(cboOfficer, txtAnswer_TextChanged);
			Common.AddControlChangedHandler(cboQuestion, txtAnswer_TextChanged);
			Common.AddControlChangedHandler(txtQuestion, txtAnswer_TextChanged);

			registerColorizedFGList(cboADTrigVar, cboADTrigType);
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

			registerFgMultiEdit(txtName, "Name", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName);
			registerFgMultiEdit(txtPilot, "Pilot", 0);
			registerFgMultiEdit(numCraft, "NumberOfCraft", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(numWaves, "NumberOfWaves", MultiEditRefreshType.ItemText);
			// numGlobal has special logic, not handled here.
			registerFgMultiEdit(txtCargo, "Cargo", 0);
			registerFgMultiEdit(txtSpecCargo, "SpecialCargo", 0);
			registerFgMultiEdit(numSC, "SpecialCargoCraft", 0);
			registerFgMultiEdit(chkRandSC, "RandSpecCargo", 0);

			registerFgMultiEdit(cboCraft, "CraftType", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName);
			registerFgMultiEdit(cboIFF, "IFF", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftName);
			registerFgMultiEdit(cboAI, "AI", 0);
			registerFgMultiEdit(cboMarkings, "Markings", 0);
			registerFgMultiEdit(cboPlayer, "PlayerCraft", MultiEditRefreshType.ItemText);
			registerFgMultiEdit(cboFormation, "Formation", 0);
			registerFgMultiEdit(numSpacing, "FormDistance", 0);
			registerFgMultiEdit(chkRadio, "FollowsOrders", 0);
			registerFgMultiEdit(cboStatus, "Status1", 0);
			registerFgMultiEdit(cboWarheads, "Missile", 0);
			registerFgMultiEdit(cboBeam, "Beam", 0);

			registerFgMultiEdit(optArrMS, "ArriveViaMothership", 0);
			registerFgMultiEdit(cboArrMS, "ArrivalMothership", 0);
			registerFgMultiEdit(optArrMSAlt, "AlternateMothershipUsed", 0);
			registerFgMultiEdit(cboArrMSAlt, "AlternateMothership", 0);
			registerFgMultiEdit(optDepMS, "DepartViaMothership", 0);
			registerFgMultiEdit(cboDepMS, "DepartureMothership", 0);
			registerFgMultiEdit(optDepMSAlt, "CapturedDepartViaMothership", 0);
			registerFgMultiEdit(cboDepMSAlt, "CapturedDepartureMothership", 0);
			registerFgMultiEdit(cboADTrigAmount, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel);
			registerFgMultiEdit(cboADTrigType, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel);
			registerFgMultiEdit(cboADTrigVar, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel);
			registerFgMultiEdit(cboADTrig, "ArrDepTrigger", MultiEditRefreshType.ArrDepLabel | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(optArrOR, "ArrDepTriggerOr", 0);
			registerFgMultiEdit(numArrMin, "ArrivalDelayMinutes", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(numArrSec, "ArrivalDelaySeconds", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftCount);
			registerFgMultiEdit(cboAbort, "AbortTrigger", 0);
			registerFgMultiEdit(numDepMin, "DepartureClockMinutes", 0);
			registerFgMultiEdit(numDepSec, "DepartureClockSeconds", 0);
			registerFgMultiEdit(cboDiff, "Difficulty", MultiEditRefreshType.ItemText | MultiEditRefreshType.CraftCount);

			registerFgMultiEdit(numBonGoalP, "BonusPoints", 0);
			registerFgMultiEdit(numPitch, "Pitch", 0);
			registerFgMultiEdit(numYaw, "Yaw", 0);
			registerFgMultiEdit(numRoll, "Roll", 0);

			registerFgMultiEdit(cboOrders, "OrderCommand", MultiEditRefreshType.OrderLabel);
			registerFgMultiEdit(cboOThrottle, "OrderThrottle", 0);
			registerFgMultiEdit(numOVar1, "OrderVar1", 0);
			registerFgMultiEdit(numOVar2, "OrderVar2", 0);
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

			registerFgMultiEdit(chkPermaDeath, "PermaDeath", 0);
			registerFgMultiEdit(numPermaDeathID, "PermaDeathID", 0);

			registerMsgMultiEdit(cboMessAmount, "MessTrigger", 0);
			registerMsgMultiEdit(cboMessType, "MessTrigger", 0);
			registerMsgMultiEdit(cboMessVar, "MessTrigger", 0);
			registerMsgMultiEdit(cboMessTrig, "MessTrigger", 0);
			registerMsgMultiEdit(cboMessColor, "MessColor", MultiEditRefreshType.ItemText);
			registerMsgMultiEdit(numMessDelay, "MessDelay", 0);
			registerMsgMultiEdit(optMessOR, "Mess1OR2", 0);
			#endregion ControlRegistration

			applySettingsHandler(0, new EventArgs());
		}

		void updateMissionTabs()
		{
			#region Globals tab
			optPrimOR.Checked = _mission.GlobalGoals.Goals[0].T1AndOrT2;
			optPrimAND.Checked = !optPrimOR.Checked;
			optSecOR.Checked = _mission.GlobalGoals.Goals[1].T1AndOrT2;
			optSecAND.Checked = !optSecOR.Checked;
			optBonOR.Checked = _mission.GlobalGoals.Goals[2].T1AndOrT2;
			optBonAND.Checked = !optBonOR.Checked;
			for (int i = 0; i < 6; i++) labelRefresh(_mission.GlobalGoals.Goals[i / 2].Triggers[i % 2], lblGlob[i]);
			lblGlobArr_Click(lblGlob[_activeGlobalGoalIndex], new EventArgs());
			#endregion
			#region Mission tab
			for (int i = 0; i < 6; i++)
			{
				txtEoM[i].Text = _mission.EndOfMissionMessages[i];
				cboEoMColor[i].SelectedIndex = _mission.EndOfMissionMessageColor[i];
			}
			for (int i = 0; i < 4; i++)
			{
				txtIFF[i].Text = _mission.IFFs[i + 2];
				if (i != 2) chkIFF[i].Checked = _mission.IffHostile[i + 2];
			}
			numRndSeed.Value = _mission.RandomSeed;
			numTimeLimitMin.Value = _mission.TimeLimitMin;
			numTimeLimitSec.Value = _mission.TimeLimitSec;
			#endregion
			#region Messages tab
			int msgIndex = lstMessages.SelectedIndex;
			if (msgIndex >= 0 && msgIndex < lstMessages.Items.Count)
			{
				labelRefresh(_mission.Messages[msgIndex].Triggers[0], lblMess1);
				labelRefresh(_mission.Messages[msgIndex].Triggers[1], lblMess2);
			}
			#endregion Messages tab
			#region Questions tab
			if (_mission.OfficersPresent == Mission.BriefingOfficers.Both) optBoth.Checked = true;
			else if (_mission.OfficersPresent == Mission.BriefingOfficers.FlightOfficer) optFO.Checked = true;
			else optSO.Checked = true;
			cboQuestion.SelectedIndex = 0;
			cboOfficer.SelectedIndex = 0;
			txtQuestion.Text = _mission.BriefingQuestions.PreMissQuestions[0];
			txtAnswer.Text = _mission.BriefingQuestions.PreMissAnswers[0];
			checkQuestionLength();
			#endregion
		}
		#endregion methods

		#region event handlers
		/// <summary>Apply color changes to all interactive labels.</summary>
		/// <remarks>This is a callback event when the program settings are updated.</remarks>
		void applySettingsHandler(object sender, EventArgs e)
		{
			foreach (Label lbl in lblADTrig) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeArrDepTriggerIndex.ToString());  //Tags are set to ints, but casting objects throws an exception, so convert to string and check those instead.
			foreach (Label lbl in lblGlob) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeGlobalGoalIndex.ToString());
			foreach (Label lbl in lblOrder) setInteractiveLabelColor(lbl, lbl.Tag.ToString() == _activeOrderIndex.ToString());
			setInteractiveLabelColor(lblMess1, _activeMessageTrigIndex == 0);
			setInteractiveLabelColor(lblMess2, _activeMessageTrigIndex == 1);
		}

		void briefingModifiedCallback(object sender, EventArgs e) => Common.MarkDirty(this, _loading);

		void colorizedComboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			ComboBox variable = (ComboBox)sender;
			colorizedFGList.TryGetValue(variable, out ComboBox variableType);
			bool colorize = true;
			if (variableType != null)        //If a VariableType selection control is attached, check that Flight Group is selected.
				colorize = (variableType.SelectedIndex == 1);

			if (e.Index == -1 || e.Index >= _mission.FlightGroups.Count) colorize = false;

			if (variable.BackColor == Color.Black || variable.BackColor == SystemColors.Window)
				variable.BackColor = (colorize == true) ? Color.Black : SystemColors.Window;

			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			if (colorize == true) brText = getFlightGroupDrawColor(e.Index);
			e.Graphics.DrawString(e.Index >= 0 ? variable.Items[e.Index].ToString() : "", e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}

		void form_Activated(object sender, EventArgs e) => lstFG_SelectedIndexChanged(0, new EventArgs());
		void form_Deactivate(object sender, EventArgs e) => lstFG.Focus();
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
				Common.MarkDirty(this);  // Since we're not loading, any change marks as dirty.
			}
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.ItemText)) listRefreshSelectedItems();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.CraftName)) updateFGList();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.OrderLabel)) orderLabelRefresh();
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.Map)) refreshMap(-1);
		}
		void messageMultiEditHandler(object sender, EventArgs e)
		{
			if (_loading) return;

			MultiEditProperty prop = (MultiEditProperty)((Control)sender).Tag;
			if (prop.Name != "")
			{
				setMessageProperty(prop.Name, Common.GetControlValue(sender));
				Common.MarkDirty(this);
			}
			if (prop.RefreshType.HasFlag(MultiEditRefreshType.ItemText)) messRefreshSelectedItems();
		}

		void opnTIE_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_loading = true;
			initializeMission();
			if (loadMission(opnTIE.FileName))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				_activeFGIndex = 0;
				lstFG.SelectedIndex = 0;
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_loading = false;
		}

		void savTIE_FileOk(object sender, System.ComponentModel.CancelEventArgs e) => saveMission(savTIE.FileName);

		void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabMain.SelectedIndex != 0) updateMissionTabs();

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

		void toolTIE_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			toolTIE.Focus();
			switch (toolTIE.Buttons.IndexOf(e.Button))
			{
				case 0:     //New Mission
					menuNewTIE_Click("toolbar", new EventArgs());
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
					menuSaveAsTIE_Click("toolbar", new EventArgs());
					break;
				case 5:     //New Item
					if (tabMain.SelectedIndex == 0) newFG();
					else if (tabMain.SelectedIndex == 1) newMess();
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
				case 15:    //Battle
					menuBattle_Click("toolbar", new EventArgs());
					break;
				case 16:    //Help
					menuHelpInfo_Click("toolbar", new EventArgs());
					break;
			}
		}
		#endregion event handlers

		#region Menu
		void menuAbout_Click(object sender, EventArgs e) => new AboutDialog().ShowDialog();
		void menuBattle_Click(object sender, EventArgs e)
		{
			_fBattle = new BattleForm();
			try { _fBattle.Show(); }
			catch (ObjectDisposedException) { _fBattle = null; }
		}
		void menuBriefing_Click(object sender, EventArgs e)
		{
			try { _fBrief.Close(); }
			catch { /* do nothing */ }
			_fBrief = new BriefingForm(_mission.FlightGroups, _mission.Briefing, briefingModifiedCallback);
			_fBrief.Show();
		}
		void menuCopy_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream = new MemoryStream();
			DataObject data = new DataObject();
			if (sender.ToString() == "AD" || hasFocus(lblADTrig))
			{
				formatter.Serialize(stream, _activeArrDepTrigger);
				data.SetText(_activeArrDepTrigger.ToString());
			}
			else if (sender.ToString() == "Order" || hasFocus(lblOrder))
			{
				formatter.Serialize(stream, _activeOrder);
				data.SetText(_activeOrder.ToString());
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
			else if (ActiveControl.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
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
			else if (sender.ToString() == "MessTrig" || lblMess1.Focused || lblMess2.Focused)
			{
				formatter.Serialize(stream, _activeMessage.Triggers[lblMess1.ForeColor == getHighlightColor() ? 0 : 1]);
				data.SetText(_activeMessage.Triggers[lblMess1.ForeColor == getHighlightColor() ? 0 : 1].ToString());
			}
			else
			{
				switch (tabMain.SelectedIndex)
				{
					case 0:
						formatter.Serialize(stream, _activeFG);
						data.SetText(_activeFG.ToString());
						break;
					case 1:
						if (_mission.Messages.Count != 0)
						{
							formatter.Serialize(stream, _activeMessage);
							data.SetText(_activeMessage.MessageString);
						}
						break;
					case 2:
						formatter.Serialize(stream, _activeGlobalTrigger);
						data.SetText(_activeGlobalTrigger.ToString());
						break;
				}
			}
			data.SetData("yogeme", false, stream);
			Clipboard.SetDataObject(data, true);
		}
		void menuCut_Click(object sender, EventArgs e) { if (Common.Cut(ActiveControl)) Common.MarkDirty(this); }
		void menuDelete_Click(object sender, EventArgs e)
		{
			if (tabMain.SelectedIndex == 0 && (sender.ToString() == "toolbar" || lstFG.Focused)) deleteFG();
			else if (tabMain.SelectedIndex == 1 && (sender.ToString() == "toolbar" || lstMessages.Focused)) deleteMess();
		}
		void menuER_Click(object sender, EventArgs e) => Common.LaunchER();
		void menuExit_Click(object sender, EventArgs e) => Close();
		void menuGlobalSummary_Click(object sender, EventArgs e) => new GlobalSummaryDialog(_mission.FlightGroups).Show();
		void menuGoalSummary_Click(object sender, EventArgs e) => new GoalSummaryDialog("(global goals not included)\r\n\r\n" + generateGoalSummary()).Show();
		void menuLibrary_Click(object sender, EventArgs e)
		{
			_fLibrary?.Close();
			_fLibrary = new FlightGroupLibraryForm(Settings.Platform.TIE, _mission.FlightGroups, flightGroupLibraryCallback);
		}
		void flightGroupLibraryCallback(object sender, EventArgs e)
		{
			foreach (object obj in (object[])sender)
			{
				FlightGroup fg = (FlightGroup)obj;
				if (fg == null || !newFG()) break;

				_mission.FlightGroups[_activeFGIndex] = fg;
				updateFGList();
				listRefreshItem(_activeFGIndex);
				_startingShips--;
				craftStart(fg, true);
			}
		}
		void menuHelpInfo_Click(object sender, EventArgs e) => Common.LaunchHelp();
		void menuIDMR_Click(object sender, EventArgs e) => Common.LaunchGithub();
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
			if (tabFGMinor.SelectedIndex == 3) refreshWaypointTab();
		}
		void menuNewXwing_Click(object sender, EventArgs e)
		{
			promptSave();
			closeForms();
			_applicationExit = false;
			new XwingForm().Show();
			Close();
		}
		void menuNewBoP_Click(object sender, EventArgs e) => menuNewXvT_Click("BoP", new EventArgs());
		void menuNewTIE_Click(object sender, EventArgs e)
		{
			promptSave();
			closeForms();
			_loading = true;
			initializeMission();
			lstMessages.Items.Clear();
			enableMessage(false);
			lblMessage.Text = "Message #0 of 0";
			lstFG.SelectedIndex = 0;
			_startingShips = 1;
			lblStarting.Text = "1 Craft at 30 seconds";
			updateMissionTabs();
			_loading = false;
			if (Text.EndsWith("*")) Text = Text.Substring(0, Text.Length - 1);
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
			opnTIE.FileName = _mission.MissionFileName;
			if (_mission.MissionFileName != "NewMission.tie") opnTIE.InitialDirectory = Directory.GetParent(_mission.MissionPath).FullName;
			if (opnTIE.ShowDialog() == DialogResult.Cancel) return;
			
			opnTIE_FileOk(0, new System.ComponentModel.CancelEventArgs());
			_config.SetWorkingPath(Path.GetDirectoryName(opnTIE.FileName));
			opnTIE.InitialDirectory = _config.GetWorkingPath();
		}
		void menuOptions_Click(object sender, EventArgs e) => new OptionsDialog(applySettingsHandler).ShowDialog();
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

			byte typeCraft = (byte)Mission.Trigger.TypeList.ShipType;
			Mission.Trigger trig = null;
			if (obj.GetType() == typeof(Mission.Trigger)) trig = (Mission.Trigger)obj;
			else if (obj.GetType() == typeof(Platform.Xvt.Mission.Trigger)) trig = (Mission.Trigger)(Platform.Xvt.Mission.Trigger)obj;
			else if (obj.GetType() == typeof(Platform.Xwa.Mission.Trigger))
			{
				trig = (Mission.Trigger)(Platform.Xwa.Mission.Trigger)obj;
				if (trig.VariableType == typeCraft) trig.Variable--;
			}

			FlightGroup.Order ord = null;
			if (obj.GetType() == typeof(FlightGroup.Order)) ord = (FlightGroup.Order)obj;
			else if (obj.GetType() == typeof(Platform.Xvt.FlightGroup.Order)) ord = (FlightGroup.Order)(Platform.Xvt.FlightGroup.Order)obj;
			else if (obj.GetType() == typeof(Platform.Xwa.FlightGroup.Order))
			{
				ord = (FlightGroup.Order)(Platform.Xwa.FlightGroup.Order)obj;
				if (ord.Target1Type == typeCraft) ord.Target1--;
				if (ord.Target2Type == typeCraft) ord.Target2--;
				if (ord.Target3Type == typeCraft) ord.Target3--;
				if (ord.Target4Type == typeCraft) ord.Target4--;
			}

			if (sender.ToString() == "AD" || hasFocus(lblADTrig))
			{
				try
				{
					foreach (FlightGroup fg in getSelectedFlightgroups())
					{
						craftStart(fg, false);
						fg.ArrDepTriggers[_activeArrDepTriggerIndex] = new Mission.Trigger(trig);
						craftStart(fg, true);
					}
					lblADTrigArr_Click(_activeArrDepTriggerIndex, new EventArgs());
					labelRefresh(_activeArrDepTrigger, lblADTrig[_activeArrDepTriggerIndex]);
					Common.MarkDirty(this);
				}
				catch { /* do nothing */ }
			}
			else if (sender.ToString() == "Order" || hasFocus(lblOrder))
			{
				try
				{
					foreach (FlightGroup fg in getSelectedFlightgroups()) fg.Orders[_activeOrderIndex] = new FlightGroup.Order(ord);
					lblOrderArr_Click(_activeOrderIndex, new EventArgs());
					orderLabelRefresh();
					Common.MarkDirty(this);
				}
				catch { /* do nothing */ }
			}
			else if (Common.Paste(ActiveControl, obj)) { Common.MarkDirty(this); }
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
			else if (sender.ToString() == "MessTrig" || lblMess1.Focused || lblMess2.Focused)
			{
				try
				{
					foreach (Platform.Tie.Message msg in getSelectedMessages()) msg.Triggers[_activeMessageTrigIndex] = new Mission.Trigger(trig);
					lblMessArr_Click(_activeMessageTrigIndex, new EventArgs());
					labelRefresh(trig, _activeMessageTrigIndex == 0 ? lblMess1 : lblMess2);
					Common.MarkDirty(this);
				}
				catch { /* do nothing */ }
			}
			else {
				switch (tabMain.SelectedIndex)
				{
					case 0:
						try
						{
							FlightGroup fg = (FlightGroup)obj ?? throw new FormatException();
							if (!newFG()) break;

							_mission.FlightGroups[_activeFGIndex] = fg;
							refreshMap(-1);
							updateFGList();
							listRefreshItem(_activeFGIndex);
							_startingShips--;
							lstFG.SelectedIndex = _activeFGIndex;
							lstFG_SelectedIndexChanged(0, new EventArgs());
							craftStart(fg, true);
							lstFG.Focus();
						}
						catch { /* do nothing */ }
						break;
					case 1:
						try
						{
							Platform.Tie.Message mess = (Platform.Tie.Message)obj ?? throw new FormatException();
							if (!newMess()) break;

							_mission.Messages[_activeMessageIndex] = mess;
							messRefreshItem(_activeMessageIndex);
							lstMessages.SelectedIndex = _activeMessageIndex;
							lstMessages_SelectedIndexChanged(0, new EventArgs());
						}
						catch { /* do nothing */ }
						break;
					case 2:
						try
						{
							_activeGlobalGoal.Triggers[_activeGlobalGoalIndex % 2] = trig ?? throw new FormatException();

							lblGlobArr_Click(_activeGlobalGoalIndex, new EventArgs());
							Common.MarkDirty(this);
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
				_activeFGIndex = 0;
				lstFG.SelectedIndex = 0;
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_config.SetWorkingPath(Path.GetDirectoryName(mission));
			opnTIE.InitialDirectory = _config.GetWorkingPath();
			savTIE.InitialDirectory = _config.GetWorkingPath();
			_loading = false;
		}
		void menuSave_Click(object sender, EventArgs e)
		{
			if (_mission.MissionPath == "\\NewMission.tie") savTIE.ShowDialog();
			else saveMission(_mission.MissionPath);
		}
		void menuSaveAsBoP_Click(object sender, EventArgs e)
		{
			promptSave();
			try
			{
				var converted = Platform.Converter.TieToBop(_mission);
				converted.Save();
			}
			catch (Exception x)	// Platform doesn't throw anything, but leave this here just in case
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsTIE_Click(object sender, EventArgs e)
		{
			Common.MarkDirty(this);	// this is to avoid the "unmodified" cancel
			savTIE.ShowDialog();
		}
		void menuSaveAsXvT_Click(object sender, EventArgs e)
		{
			promptSave();
			try
			{
				var converted = Platform.Converter.TieToXvt(_mission);
				converted.Save();
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXWA_Click(object sender, EventArgs e)
		{
			promptSave();
			try
			{
				var converted = Platform.Converter.TieToXwa(_mission);
				converted.Save();
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuVerify_Click(object sender, EventArgs e)
		{
			menuSave_Click("Verify", new EventArgs());
			if (!_config.Verify) Common.RunVerify(_mission.MissionPath);
		}
		void menuTest_Click(object sender, EventArgs e)
		{
			if (_config.ConfirmTest)
			{
				DialogResult res = new TestDialog().ShowDialog();
				if (res == DialogResult.Cancel) return;
			}
			menuSave_Click("menuTest_Click", new EventArgs());
			if (_mission.MissionPath == "\\NewMission.tie") return;

			string path = (_config.TieDetectMission ? Directory.GetParent(_mission.MissionPath).Parent.FullName + "\\" : _config.TiePath);
			if (!File.Exists(path + "TIE95.exe") && _config.TieDetectMission)
			{
				System.Diagnostics.Debug.WriteLine("TIE not detected at MissionPath, default used");
				path = _config.TiePath + "\\";
			}

			bool localMission = _mission.MissionPath.ToLower().Contains(path.ToLower());
			string fileName = (!localMission ? path + "MISSION\\" + _mission.MissionFileName : _mission.MissionPath);
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

			if (_config.VerifyTest && !_config.Verify) Common.RunVerify(_mission.MissionPath);
			Version os = Environment.OSVersion.Version;
			bool isWin7 = (os.Major == 6 && os.Minor == 1);
			System.Diagnostics.Process explorer = null;
			int restart = 1;
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", false);

			int index = 0;
			while (File.Exists(path + "TEST" + index + ".tfr")) index++;
			System.Diagnostics.Debug.WriteLine("pilot index: " + index);
			string pilot = "TEST" + index + ".tfr";
			string battle = "RESOURCE\\BATTLE1.LFD";
			string backup = "RESOURCE\\BATTLE1_" + index + ".bak";
			File.Copy(Application.StartupPath + "\\TEST.tfr", path + pilot);
			System.Diagnostics.Process tie = new System.Diagnostics.Process();
			tie.StartInfo.FileName = path + "TIE95.exe";
			tie.StartInfo.UseShellExecute = false;
			tie.StartInfo.WorkingDirectory = path;
			File.Copy(path + battle, path + backup, true);
			LfdFile battleLfd = new LfdFile(path + battle);
			Text txt = (Text)battleLfd.Resources[0];
			string[] missions = txt.Strings[3].Split('\0');
			missions[0] = _mission.MissionFileName.ToLower().Replace(".tie", "");
			txt.Strings[3] = string.Join("\0", missions);
			txt.Dirty();
			battleLfd.Write();

			if (isWin7 && !path.ToUpper().Contains("STEAM")) // explorer kill so colors work right
			{
				key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);
				restart = (int)key.GetValue("AutoRestartShell", 1);
				key.SetValue("AutoRestartShell", 0, Microsoft.Win32.RegistryValueKind.DWord);
				explorer = System.Diagnostics.Process.GetProcessesByName("explorer")[0];
				explorer.Kill();
				explorer.WaitForExit();
			}

			tie.Start();
			System.Threading.Thread.Sleep(3000);
			System.Diagnostics.Process[] runningTies = System.Diagnostics.Process.GetProcessesByName("tie95");
			while (runningTies.Length > 0)
			{
				Application.DoEvents();
				System.Diagnostics.Debug.WriteLine("sleeping...");
				System.Threading.Thread.Sleep(1000);
				runningTies = System.Diagnostics.Process.GetProcessesByName("tie95");
			}

			if (isWin7 && !path.ToUpper().Contains("STEAM")) // restart
			{
				key.SetValue("AutoRestartShell", restart, Microsoft.Win32.RegistryValueKind.DWord);
				explorer.StartInfo.UseShellExecute = false;
				explorer.StartInfo.FileName = "explorer.exe";
				explorer.Start();
			}
			if (_config.DeleteTestPilots) File.Delete(path + pilot);
			File.Copy(path + backup, path + battle, true);
			File.Delete(path + backup);
			if (!localMission)
			{
				if (File.Exists(fileName + ".bak"))
				{
					File.Copy(fileName + ".bak", fileName, true);
					File.Delete(fileName + ".bak");
				}
				else File.Delete(fileName);
			}
			System.Diagnostics.Debug.WriteLine("Testing complete");
		}
		#endregion
		#region FlightGroups
		FlightGroup _activeFG => _mission.FlightGroups[_activeFGIndex];
		/// <summary>Counts all trigger and parameter references of a FG</summary>
		/// <param name="fgIndex">Index within _mission.FlightGroups</param>
		/// <remarks>Used to populate the counters in the confirm deletion dialog</remarks>
		int[] countFlightGroupReferences(int fgIndex)
		{
			int[] count = new int[7];
			const int cMothership = 1, cArrDep = 2, cOrder = 3, cGoal = 4, cMessage = 5, cBrief = 6;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (fgIndex == i) continue;

				FlightGroup fg = _mission.FlightGroups[i];
				if (fg.ArriveViaMothership && fg.ArrivalMothership == fgIndex) count[cMothership]++;
				if (fg.AlternateMothershipUsed && fg.AlternateMothership == fgIndex) count[cMothership]++;
				if (fg.DepartViaMothership && fg.DepartureMothership == fgIndex) count[cMothership]++;
				if (fg.CapturedDepartViaMothership && fg.CapturedDepartureMothership == fgIndex) count[cMothership]++;
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
			}

			foreach (Globals.Goal goal in _mission.GlobalGoals.Goals)
				foreach (Mission.Trigger trig in goal.Triggers)
					if (trig.VariableType == 1 && trig.Variable == fgIndex)
						count[cGoal]++;

			foreach (Platform.Tie.Message msg in _mission.Messages)
				foreach (Mission.Trigger trig in msg.Triggers)
					if (trig.VariableType == (byte)Mission.Trigger.TypeList.FlightGroup && trig.Variable == fgIndex)
						count[cMessage]++;

			Briefing br = _mission.Briefing;
			for (int p = 0; p < br.Events.Count; p++)
				if (br.Events[p].IsFGTag && br.Events[p].Variables[0] == fgIndex) count[cBrief]++;

			for (int i = 1; i < 7; i++) count[0] += count[i];
			return count;
		}
		void deleteFG()
		{
			_fBrief?.Close(); //Close (which also saves) the briefing before accessing it.  Don't call save directly since this may cause FG index corruption if multiple FGs are deleted.

            List<int> selection = Common.GetSelectedIndices(lstFG);
			int startFG = _activeFGIndex;
			for (int si = selection.Count - 1; si >= 0; si--)  // Delete from end so prior indices remain intact.
			{
				int fgIndex = selection[si];
				if (_config.ConfirmFGDelete)
				{
					int[] count = countFlightGroupReferences(fgIndex);
					if (count[0] > 0)
					{
						string[] Reasons = new string[7] { "", "Mothership reference", "Arrival/Departure trigger", "Order target reference", "Global Goal trigger", "Message trigger", "Briefing FG Tag" };
						string breakdown = "";
						for (int i = 1; i < 7; i++)
							if (count[i] > 0) breakdown += "    " + count[i] + " " + Reasons[i] + ((count[i] > 1) ? "s" : "") + "\n";

						string s = "This Flight Group is referenced " + count[0] + " time" + ((count[0] > 1) ? "s" : "") + " in these cases:\n" + breakdown + "\nAll references targeting this flight group will be reset to default.";
						if (count[6] > 0) s += "\nAssociated Briefing FG Tag events will be deleted.";
						s += "\n\nAre you sure you want to delete this Flight Group?";
						DialogResult res = MessageBox.Show(s, "WARNING: Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (res == DialogResult.No) break;  // exit the outer for() loop
					}
				}
				replaceClipboardFGReference(fgIndex, -1);
				if (_mission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(fgIndex);
				craftStart(_mission.FlightGroups[fgIndex], false);
				if (_mission.FlightGroups.Count == 1)
				{
					_mission.DeleteFG(fgIndex);  // Need to perform a full delete to wipe the FG indexes (messages or briefing tags may still have them).  The delete function always ensures that Count==1, so it must be inside this block, not before.
					_mission.FlightGroups.Clear();
					_mission.FlightGroups[0].CraftType = _config.TieCraft;
					_mission.FlightGroups[0].IFF = _config.TieIff;
					craftStart(_mission.FlightGroups[0], true);
					break;
				}
				else _mission.DeleteFG(fgIndex);
			}
			
			if (startFG >= _mission.FlightGroups.Count)
				startFG = _mission.FlightGroups.Count - 1;
			lstFG.SelectedIndex = startFG;

			updateFGList();
			refreshMap(-1);
			try
			{
				_fBrief.Import(_mission.FlightGroups);
				_fBrief.MapPaint();
			}
			catch { /* do nothing */ }
			Common.MarkDirty(this);

			lstMessages_SelectedIndexChanged(0, new EventArgs());
			lstFG_SelectedIndexChanged(0, new EventArgs());
			updateMissionTabs();
			if (!lstFG.Focused) lstFG.Focus();
		}
		string generateGoalSummary()
		{
			//4 elements:  Primary,Secondary,Secret,Bonus
			//Each element contains a list of strings for each goal.
			List<string>[] goalList = new List<string>[4];

			for (int i = 0; i < 4; i++) goalList[i] = new List<string>();

			//Iterate FGs and their goals, adding them to the proper list
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				FlightGroup fg = _mission.FlightGroups[i];
				string c = Strings.CraftAbbrv[fg.CraftType] + " " + fg.Name;
				string n = composeGoalString(c, fg.Goals.PrimaryAmount, fg.Goals.PrimaryCondition);
				if (n != "") goalList[0].Add(n);

				n = composeGoalString(c, fg.Goals.SecondaryAmount, fg.Goals.SecondaryCondition);
				if (n != "") goalList[1].Add(n);

				n = composeGoalString(c, fg.Goals.BonusAmount, fg.Goals.BonusCondition);
				if (n != "")
				{
					n += " (" + fg.Goals.BonusPoints + " points)";
					goalList[3].Add(n);
				}
			}

			//Compose the output by going through the goal categories
			string output = "";
			for (int i = 0; i < 4; i++)
			{
				if (goalList[i].Count == 0) continue;

				if (output.Length > 0) output += "\r\n";
				switch (i)
				{
					case 0: output += "PRIMARY:\r\n"; break;
					case 1: output += "SECONDARY:\r\n"; break;
					case 3: output += "BONUS:\r\n"; break;
				}
				foreach (string s in goalList[i]) output += s + "\r\n";
			}
			if (output == "") output = "Nothing here.";
			output += "\r\n";

			return output;
		}
		List<FlightGroup> getSelectedFlightgroups()
		{
			List<FlightGroup> fgs = new List<FlightGroup>();
			foreach (int fgIndex in lstFG.SelectedIndices) fgs.Add(_mission.FlightGroups[fgIndex]);
			return fgs;
		}
		void setFlightgroupProperty(MultiEditRefreshType refreshType, string name, object value)
		{
			int trigRefresh = 0;
			foreach (FlightGroup fg in getSelectedFlightgroups())
			{
				if (refreshType.HasFlag(MultiEditRefreshType.CraftCount)) craftStart(fg, false);
				switch (name)
				{
					case "Name": fg.Name = (string)value; break;
					case "Pilot": fg.Pilot = (string)value; break;
					case "NumberOfCraft":
						fg.NumberOfCraft = Convert.ToByte(value);
						if (fg.SpecialCargoCraft > fg.NumberOfCraft) fg.SpecialCargoCraft = 0;
						break;
					case "NumberOfWaves": fg.NumberOfWaves = Convert.ToByte(value); break;
					// "GlobalGroup" has special logic, not handled here.
					case "Cargo": fg.Cargo = (string)value; break;
					case "SpecialCargo": fg.SpecialCargo = (string)value; break;
					case "SpecialCargoCraft":
						fg.SpecialCargoCraft = Convert.ToByte((int)value);
						if (fg.SpecialCargoCraft > fg.NumberOfCraft) fg.SpecialCargoCraft = 0;
						break;
					case "RandSpecCargo": fg.RandSpecCargo = Convert.ToBoolean(value); break;
					case "CraftType": fg.CraftType = Convert.ToByte(value); break;
					case "IFF": fg.IFF = Convert.ToByte(value); break;
					case "AI": fg.AI = Convert.ToByte(value); break;
					case "Markings": fg.Markings = Convert.ToByte(value); break;
					case "PlayerCraft": fg.PlayerCraft = Convert.ToByte(value); break;
					case "Formation": fg.Formation = Convert.ToByte(value); break;
					case "FormDistance": fg.FormDistance = Convert.ToByte(value); break;
					case "FollowsOrders": fg.FollowsOrders = Convert.ToBoolean(value); break;
					case "Status1": fg.Status1 = Convert.ToByte(value); break;
					case "Missile": fg.Missile = Convert.ToByte(value); break;
					case "Beam": fg.Beam = Convert.ToByte(value); break;
					case "ArrivalMothership": fg.ArrivalMothership = Convert.ToByte(value); break;
					case "AlternateMothership": fg.AlternateMothership = Convert.ToByte(value); break;
					case "DepartureMothership": fg.DepartureMothership = Convert.ToByte(value); break;
					case "CapturedDepartureMothership": fg.CapturedDepartureMothership = Convert.ToByte(value); break;
					case "ArriveViaMothership": fg.ArriveViaMothership = Convert.ToBoolean(value); break;
					case "AlternateMothershipUsed": fg.AlternateMothershipUsed = Convert.ToBoolean(value); break;
					case "DepartViaMothership": fg.DepartViaMothership = Convert.ToBoolean(value); break;
					case "CapturedDepartViaMothership": fg.CapturedDepartViaMothership = Convert.ToBoolean(value); break;
					case "ArrDepTrigger":
						fg.ArrDepTriggers[_activeArrDepTriggerIndex] = getTriggerFromControls(cboADTrigAmount, cboADTrigType, cboADTrigVar, cboADTrig);
						if (trigRefresh++ == 0) labelRefresh(fg.ArrDepTriggers[_activeArrDepTriggerIndex], lblADTrig[_activeArrDepTriggerIndex]);  // only refresh once
						break;
					case "ArrDepTriggerOr": fg.AT1OrAT2 = Convert.ToBoolean(value); break;
					case "ArrivalDelayMinutes": fg.ArrivalDelayMinutes = Convert.ToByte(value); break;
					case "ArrivalDelaySeconds": fg.ArrivalDelaySeconds = Convert.ToByte(value); break;
					case "AbortTrigger": fg.AbortTrigger = Convert.ToByte(value); break;
					case "DepartureClockMinutes": fg.DepartureTimerMinutes = Convert.ToByte(value); break;
					case "DepartureClockSeconds": fg.DepartureTimerSeconds = Convert.ToByte(value); break;
					case "Difficulty": fg.Difficulty = (Platform.BaseFlightGroup.Difficulties)value; break;
					case "BonusPoints": fg.Goals.BonusPoints = Convert.ToInt16(value); break;
					case "Pitch": fg.Pitch = Convert.ToInt16(value); break;
					case "Yaw": fg.Yaw = Convert.ToInt16(value); break;
					case "Roll": fg.Roll = Convert.ToInt16(value); break;
					case "OrderCommand": fg.Orders[_activeOrderIndex].Command = Convert.ToByte(value); break;
					case "OrderThrottle": fg.Orders[_activeOrderIndex].Throttle = Convert.ToByte(value); break;
					case "OrderVar1": fg.Orders[_activeOrderIndex].Variable1 = Convert.ToByte(value); break;
					case "OrderVar2": fg.Orders[_activeOrderIndex].Variable2 = Convert.ToByte(value); break;
					case "OrderTarget1": fg.Orders[_activeOrderIndex].Target1 = Convert.ToByte(value); break;
					case "OrderTarget2": fg.Orders[_activeOrderIndex].Target2 = Convert.ToByte(value); break;
					case "OrderTarget3": fg.Orders[_activeOrderIndex].Target3 = Convert.ToByte(value); break;
					case "OrderTarget4": fg.Orders[_activeOrderIndex].Target4 = Convert.ToByte(value); break;
					case "OrderTarget1Type": fg.Orders[_activeOrderIndex].Target1Type = Convert.ToByte(value); break;
					case "OrderTarget2Type": fg.Orders[_activeOrderIndex].Target2Type = Convert.ToByte(value); break;
					case "OrderTarget3Type": fg.Orders[_activeOrderIndex].Target3Type = Convert.ToByte(value); break;
					case "OrderTarget4Type": fg.Orders[_activeOrderIndex].Target4Type = Convert.ToByte(value); break;
					case "Order12Or": fg.Orders[_activeOrderIndex].T1OrT2 = Convert.ToBoolean(value); break;
					case "Order34Or": fg.Orders[_activeOrderIndex].T3OrT4 = Convert.ToBoolean(value); break;
					case "PermaDeath": fg.PermaDeathEnabled = Convert.ToBoolean(value); break;
					case "PermaDeathID": fg.PermaDeathID = Convert.ToByte(value); break;
					default: throw new ArgumentException("Unhandled multi-edit property: " + name);
				}
				if (refreshType.HasFlag(MultiEditRefreshType.CraftCount)) craftStart(fg, true);
			}
		}
		void moveFlightgroups(int direction)
		{
			List<int> selection = Common.GetSelectedIndices(lstFG);
			if (selection.Count == 0 || (direction == -1 && selection[0] == 0) || (direction == 1 && selection[selection.Count - 1] == lstFG.Items.Count - 1)) return;

			for (int i = 0; i < selection.Count; i++)
			{
				// Traverse the selection list forward if moving up, backward if moving down.
				int accessIndex = ((direction == -1) ? i : selection.Count - 1 - i);
				int fgIndex = selection[accessIndex];
				_mission.SwapFG(fgIndex, fgIndex + direction);
				replaceClipboardFGReference(fgIndex, fgIndex + direction);
				// Updating the map after each item is slow. Reimport the entire list after we're done.
				listRefreshItem(fgIndex + direction, false);
				listRefreshItem(fgIndex, false);
				selection[accessIndex] += direction;
			}
			Common.SetSelectedIndices(lstFG, selection, ref _noRefresh);

			_fBrief?.Close();
			refreshMap(-1);
			updateFGList();
			Common.MarkDirty(this);
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
		void listRefreshSelectedItems() { foreach (int i in Common.GetSelectedIndices(lstFG)) listRefreshItem(i); }
		bool newFG()
		{
			if (_mission.FlightGroups.Count == Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			_activeFGIndex = _mission.FlightGroups.Add();
			_activeFG.CraftType = _config.TieCraft;
			_activeFG.IFF = _config.TieIff;
			craftStart(_activeFG, true);
			lstFG.Items.Add(_activeFG.ToString(true));
			updateFGList();
			lstFG.ClearSelected();
			lstFG.SelectedIndex = _activeFGIndex;
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
		/// <summary>Updates the clipboard contents from containing broken indexes.</summary>
		/// <remarks>Should be called during swap or delete (dstIndex &lt; 0) operations.</remarks>
		void replaceClipboardFGReference(int srcIndex, int dstIndex)
		{
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
						change |= fg.TransformFGReferences(dstIndex, 255);
						change |= fg.TransformFGReferences(srcIndex, dstIndex);
						change |= fg.TransformFGReferences(255, srcIndex);
					}
					else change |= fg.TransformFGReferences(srcIndex, -1);
					data.SetText(fg.ToString());
				}
				else if (raw.GetType() == typeof(Platform.Tie.Message))
				{
					Platform.Tie.Message mess = (Platform.Tie.Message)raw;
					foreach (var trig in mess.Triggers)
					{
						if (dstIndex >= 0) change |= trig.SwapFGReferences(srcIndex, dstIndex);
						else change |= trig.TransformFGReferences(srcIndex, dstIndex, true);
					}
					data.SetText(mess.MessageString);
				}
				else if (raw.GetType() == typeof(Mission.Trigger))
				{
					Mission.Trigger trig = (Mission.Trigger)raw;
					if (dstIndex >= 0) change |= trig.SwapFGReferences(srcIndex, dstIndex);
					else change |= trig.TransformFGReferences(srcIndex, dstIndex, true);
					data.SetText(trig.ToString());

				}
				else if (raw.GetType() == typeof(FlightGroup.Order))
				{
					FlightGroup.Order order = (FlightGroup.Order)raw;
					if (dstIndex >= 0)
					{
						change |= order.TransformFGReferences(dstIndex, 255);
						change |= order.TransformFGReferences(srcIndex, dstIndex);
						change |= order.TransformFGReferences(255, srcIndex);
					}
					else change |= order.TransformFGReferences(srcIndex, -1);
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
		/// <summary>Scans all Flight Groups to detect duplicate names, to provide helpful craft numbering within the editor so that the user can easily tell duplicates apart in triggers.</summary>
		void recalculateEditorCraftNumbering()
		{
			// Note: changing an item in lstFG will activate lstFG_SelectedIndexChanged, which changes _activeFG and potentially cause bugs elsewhere. So need to restore before exiting the function.
			int currentFG = _activeFGIndex;
			//A-W Red and X-W Red should not be considered duplicates, so this structure maps a CraftType to a sub-dictionary of CraftName and Count.  
			//Due to the complexity and careful error checking involved (throwing exceptions is incredibly slow), two separate functions are provided to manipulate and access them.
			Dictionary<int, Dictionary<string, int>> dupeCount = new Dictionary<int, Dictionary<string, int>>();
			Dictionary<int, Dictionary<string, int>> nameCount = new Dictionary<int, Dictionary<string, int>>();

			foreach (var fg in _mission.FlightGroups)     //Need to know the duplicates ahead of time, so that even the first craft encountered can be numbered accordingly.
				Common.AddFGCounter(fg.CraftType, fg.IFF, fg.Name, 1, dupeCount);

			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				FlightGroup fg = _mission.FlightGroups[i];
				bool isDupe = (Common.GetFGCounter(fg.CraftType, fg.IFF, fg.Name, dupeCount) >= 2);

				int count = Common.AddFGCounter(fg.CraftType, fg.IFF, fg.Name, 1, nameCount);
				if (!isDupe) count = 0;

				if (fg.EditorCraftNumber != count)
				{
					fg.EditorCraftNumber = count;
					fg.EditorCraftExplicit = false;  //TIE does not have Flight Group numbering beyond single waves.
					listRefreshItem(i);
				}
			}
			_activeFGIndex = currentFG;
		}
		void updateFGList()
		{
			recalculateEditorCraftNumbering();

			string[] fgList = _mission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			comboReset(cboArrMS, fgList, _activeFG.ArrivalMothership);
			comboReset(cboArrMSAlt, fgList, _activeFG.AlternateMothership);
			comboReset(cboDepMS, fgList, _activeFG.DepartureMothership);
			comboReset(cboDepMSAlt, fgList, _activeFG.CapturedDepartureMothership);
			if (cboMessType.SelectedIndex == 1) comboReset(cboMessVar, fgList, cboMessVar.SelectedIndex);
			// Refresh trigger labels
			for (int i = 0; i < 3; i++) labelRefresh(_activeFG.ArrDepTriggers[i], lblADTrig[i]);
			lblADTrigArr_Click(lblADTrig[_activeArrDepTriggerIndex], new EventArgs());
			byte restore = _activeOrderIndex;
			for (_activeOrderIndex = 0; _activeOrderIndex < 3; _activeOrderIndex++) orderLabelRefresh();
			lblOrderArr_Click(lblOrder[restore], new EventArgs());
			if (_mission.Messages.Count > 0)
			{
				var lbl = _activeMessageTrigIndex == 0 ? lblMess1 : lblMess2;
				labelRefresh(_activeMessTrig, lbl);
				lblMessArr_Click(lbl, new EventArgs());
			}

			_loading = temp;
			listRefreshItem(_activeFGIndex);
		}
		bool updateGG(bool update)
		{
			int refCount = 0;
			int ggCount = 0;
			byte gg = _activeFG.GlobalGroup;
			byte typeGG = (byte)Mission.Trigger.TypeList.GlobalGroup;
			for (int i = 0; i < _mission.FlightGroups.Count; i++)
			{
				if (_activeFGIndex == i) continue;

				FlightGroup fg = _mission.FlightGroups[i];
				if (fg.GlobalGroup == gg)
				{
					ggCount++;
					continue;
				}

				foreach (Mission.Trigger adt in fg.ArrDepTriggers)
					if (adt.VariableType == typeGG && adt.Variable == gg)
					{
						if (update) adt.Variable = (byte)numGlobal.Value;
						else refCount++;
					}
				foreach (FlightGroup.Order order in fg.Orders)
				{
					if (order.Target1Type == typeGG && order.Target1 == gg)
					{
						if (update) order.Target1 = (byte)numGlobal.Value;
						else refCount++;
					}
					if (order.Target2Type == typeGG && order.Target2 == gg)
					{
						if (update) order.Target2 = (byte)numGlobal.Value;
						else refCount++;
					}
					if (order.Target3Type == typeGG && order.Target3 == gg)
					{
						if (update) order.Target3 = (byte)numGlobal.Value;
						else refCount++;
					}
					if (order.Target4Type == typeGG && order.Target4 == gg)
					{
						if (update) order.Target4 = (byte)numGlobal.Value;
						else refCount++;
					}
				}
			}
			foreach (Globals.Goal goal in _mission.GlobalGoals.Goals)
				foreach (Mission.Trigger trig in goal.Triggers)
					if (trig.VariableType == typeGG && trig.Variable == gg)
					{
						if (update) trig.Variable = (byte)numGlobal.Value;
						else refCount++;
					}
			foreach (Platform.Tie.Message msg in _mission.Messages)
				foreach (Mission.Trigger trig in msg.Triggers)
					if (trig.VariableType == typeGG && trig.Variable == gg)
					{
						if (update) trig.Variable = (byte)numGlobal.Value;
						else refCount++;
					}
			// since I'm using foreach and don't have the index, just redo all of them in case it's one we updated
			for (int i = 0; i < 6; i++) labelRefresh(_mission.GlobalGoals.Goals[i / 2].Triggers[i % 2], lblGlob[i]);
			lblGlobArr_Click(_activeGlobalGoalIndex, new EventArgs());
			if (_mission.Messages.Count > 0)
			{
				labelRefresh(_activeMessage.Triggers[0], lblMess1);
				labelRefresh(_activeMessage.Triggers[1], lblMess2);
				lblMessArr_Click(_activeMessageIndex, new EventArgs());
			}
			return !update && ggCount == 0 && refCount > 0;
		}

		void lstFG_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1 || _mission.FlightGroups[e.Index] == null) return;

			e.DrawBackground();
			Brush brText = getFlightGroupDrawColor(e.Index);
			e.Graphics.DrawString(lstFG.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstFG.SelectedIndex == -1 || _noRefresh) return;

			_activeFGIndex = lstFG.SelectedIndex;
			byte order = _activeOrderIndex;
			lblFG.Text = "Flight Group #" + (_activeFGIndex + (_config.OneIndexedFGs ? 1 : 0)).ToString() + " of " + _mission.FlightGroups.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			#region Craft
			txtName.Text = _activeFG.Name;
			txtPilot.Text = _activeFG.Pilot;
			txtCargo.Text = _activeFG.Cargo;
			txtSpecCargo.Text = _activeFG.SpecialCargo;
			numSC.Value = _activeFG.SpecialCargoCraft;
			chkRandSC.Checked = _activeFG.RandSpecCargo;
			numCraft.Value = _activeFG.NumberOfCraft;
			numWaves.Value = _activeFG.NumberOfWaves;
			numGlobal.Value = _activeFG.GlobalGroup;
			cboCraft.SelectedIndex = _activeFG.CraftType;
			cboIFF.SelectedIndex = _activeFG.IFF;
			cboAI.SelectedIndex = _activeFG.AI;
			cboMarkings.SelectedIndex = _activeFG.Markings;
			cboPlayer.SelectedIndex = _activeFG.PlayerCraft;
			Common.SafeSetCBO(cboFormation, _activeFG.Formation, true);  //[JB] Sigh... custom missions.
			chkRadio.Checked = Convert.ToBoolean(_activeFG.FollowsOrders);
			numSpacing.Value = _activeFG.FormDistance;
			refreshStatus();
			cboWarheads.SelectedIndex = _activeFG.Missile;
			cboBeam.SelectedIndex = _activeFG.Beam;
			#endregion
			#region Arr/Dep
			optArrMS.Checked = Convert.ToBoolean(_activeFG.ArriveViaMothership);
			optArrHyp.Checked = !optArrMS.Checked;
			optArrMSAlt.Checked = Convert.ToBoolean(_activeFG.AlternateMothershipUsed);
			optArrHypAlt.Checked = !optArrMSAlt.Checked;
			optDepMS.Checked = Convert.ToBoolean(_activeFG.DepartViaMothership);
			optDepHyp.Checked = !optDepMS.Checked;
			optDepMSAlt.Checked = Convert.ToBoolean(_activeFG.CapturedDepartViaMothership);
			optDepHypAlt.Checked = !optDepMSAlt.Checked;
			try { cboArrMS.SelectedIndex = _activeFG.ArrivalMothership; }
			catch { cboArrMS.SelectedIndex = 0; _activeFG.ArrivalMothership = 0; optArrHyp.Checked = true; }
			try { cboArrMSAlt.SelectedIndex = _activeFG.AlternateMothership; }
			catch { cboArrMSAlt.SelectedIndex = 0; _activeFG.AlternateMothership = 0; optArrHypAlt.Checked = true; }
			try { cboDepMS.SelectedIndex = _activeFG.DepartureMothership; }
			catch { cboDepMS.SelectedIndex = 0; _activeFG.DepartureMothership = 0; optDepHyp.Checked = true; }
			try { cboDepMSAlt.SelectedIndex = _activeFG.CapturedDepartureMothership; }
			catch { cboDepMSAlt.SelectedIndex = 0; _activeFG.CapturedDepartureMothership = 0; optDepHypAlt.Checked = true; }
			optArrOR.Checked = _activeFG.AT1OrAT2;
			optArrAND.Checked = !optArrOR.Checked;
			for (int i = 0; i < 3; i++) labelRefresh(_activeFG.ArrDepTriggers[i], lblADTrig[i]);
			numArrMin.Value = _activeFG.ArrivalDelayMinutes;
			numArrSec.Value = _activeFG.ArrivalDelaySeconds;
			numDepMin.Value = _activeFG.DepartureTimerMinutes;
			numDepSec.Value = _activeFG.DepartureTimerSeconds;
			cboAbort.SelectedIndex = _activeFG.AbortTrigger;
			cboDiff.SelectedIndex = (byte)_activeFG.Difficulty;
			lblADTrigArr_Click(lblADTrig[0], new EventArgs());
			#endregion
			for (int i = 0; i < 8; i++)
			{
				if (i == 4 || i == 5) continue;
				cboGoal[i].SelectedIndex = _activeFG.Goals[i];
			}
			numBonGoalP.Value = _activeFG.Goals.BonusPoints;
			refreshWaypointTab();
			for (_activeOrderIndex = 0; _activeOrderIndex < 3; _activeOrderIndex++) orderLabelRefresh();
			lblOrderArr_Click(lblOrder[_config.RememberSelectedOrder ? order : 0], new EventArgs());
			chkPermaDeath.Checked = _activeFG.PermaDeathEnabled;
			numPermaDeathID.Value = _activeFG.PermaDeathID;
			_loading = btemp;
			enableBackdrop(_activeFG.CraftType == 0x57);
			if (numBackdrop.Visible)  numBackdrop.Value = _activeFG.Status1;

			Common.UpdateMoveButtons(cmdMoveFGUp, cmdMoveFGDown, lstFG);
			if (!lstFG.Focused) lstFG.Focus();
		}

		void cmdMoveFGUp_Click(object sender, EventArgs e) => moveFlightgroups(-1);
		void cmdMoveFGDown_Click(object sender, EventArgs e) => moveFlightgroups(1);

		void tabFGMinor_SelectedIndexChanged(object sender, EventArgs e) { if (tabFGMinor.SelectedIndex == 0) setSpecialVisibility(); }
		#region Craft
		void enableBackdrop(bool state)
		{
			numBackdrop.Visible = state;
			cmdBackdrop.Visible = state;
			cboAI.Enabled = !state;
			cboMarkings.Enabled = !state;
			cboPlayer.Enabled = !state;
			cboFormation.Enabled = !state;
			cmdForms.Enabled = !state;
			numSpacing.Enabled = !state;
			cboStatus.Enabled = !state;
			cboWarheads.Enabled = !state;
			cboBeam.Enabled = !state;
			numCraft.Enabled = !state;
			numWaves.Enabled = !state;
			numSC.Enabled = !state;
			chkRandSC.Enabled = !state;
			lblStatus.Visible = !state;
			cboStatus.Visible = !state;
			lblBackdrop.Visible = state;
			chkRadio.Enabled = !state;
			txtCargo.Enabled = !state;
		}
		void refreshStatus()
		{
			cboStatus.Items.Clear();
			bool isMine = (_activeFG.CraftType >= 0x4B && _activeFG.CraftType <= 0x4D);
			lblStatus.Text = isMine ? "Mine Formation" : "Status";
			cboStatus.Items.AddRange(isMine ? Strings.FormationMine : Strings.Status);
			Common.SafeSetCBO(cboStatus, isMine ? _activeFG.Status1 & 3 : _activeFG.Status1, true);
			cboFormation.Enabled = !isMine;
		}
		void setSpecialVisibility()
		{
			lblNotUsed.Visible = ((numSC.Value == 0 || numSC.Value > _activeFG.NumberOfCraft) && !chkRandSC.Checked);
			txtSpecCargo.Visible = !lblNotUsed.Visible;
		}

		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			enableBackdrop((cboCraft.SelectedIndex == 0x57));
			enableRot(cboCraft.SelectedIndex > 0x45);
			refreshStatus();
		}
		void cboStatus_Leave(object sender, EventArgs e)
		{
			try { numBackdrop.Value = cboStatus.SelectedIndex; }
			catch { numBackdrop.Value = numBackdrop.Maximum; }
		}

		void chkRandSC_CheckedChanged(object sender, EventArgs e) => setSpecialVisibility();

		void cmdBackdrop_Click(object sender, EventArgs e)
		{
			try
			{
				BackdropDialog dlg = new BackdropDialog(Platform.MissionFile.Platform.TIE, _activeFG.Status1);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					numBackdrop.Value = dlg.BackdropIndex;
					// Multi-edit handles the Status value change.
					cboStatus.SelectedIndex = (int)numBackdrop.Value;
				}
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void cmdForms_Click(object sender, EventArgs e)
		{
			FormationDialog dlg = new FormationDialog(_activeFG.Formation, _activeFG.FormDistance, Settings.Platform.TIE);
			if (dlg.ShowDialog() == DialogResult.Cancel) return;
			
			cboFormation.SelectedIndex = dlg.Formation;
			numSpacing.Value = dlg.Spacing;
		}

		void numBackdrop_Leave(object sender, EventArgs e) => cboStatus.SelectedIndex = (int)numBackdrop.Value;
		void numGlobal_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) numGlobal_Leave("numGlobal_KeyDown", new EventArgs()); }
		void numGlobal_Leave(object sender, EventArgs e)
		{
			if (_activeFG.GlobalGroup != Convert.ToByte(numGlobal.Value) && updateGG(false))
			{
				DialogResult res = MessageBox.Show("Global Group is unique and referenced. Update references to new number?", "Update Reference?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.Yes) updateGG(true);
			}
			foreach(FlightGroup fg in getSelectedFlightgroups()) fg.GlobalGroup = Common.Update(this, fg.GlobalGroup, Convert.ToByte(numGlobal.Value));
			listRefreshSelectedItems();
		}
		void numSC_ValueChanged(object sender, EventArgs e) => setSpecialVisibility();
		#endregion
		#region ArrDep
		Mission.Trigger _activeArrDepTrigger => _activeFG.ArrDepTriggers[_activeArrDepTriggerIndex];
		void lblADTrigArr_Click(object sender, EventArgs e)
		{
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeArrDepTriggerIndex = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeArrDepTriggerIndex = Convert.ToByte(sender); l = lblADTrig[_activeArrDepTriggerIndex]; }
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 3; i++) if (i != _activeArrDepTriggerIndex) setInteractiveLabelColor(lblADTrig[i], false);
			bool btemp = _loading;
			_loading = true;
			cboADTrig.SelectedIndex = _activeArrDepTrigger.Condition;
			cboADTrigType.SelectedIndex = -1;
			cboADTrigType.SelectedIndex = _activeArrDepTrigger.VariableType;
			cboADTrigAmount.SelectedIndex = _activeArrDepTrigger.Amount;
			_loading = btemp;
		}
		void lblADTrigArr_DoubleClick(object sender, EventArgs e) => menuPaste_Click("AD", new EventArgs());
		void lblADTrigArr_MouseUp(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Right) menuCopy_Click("AD", new EventArgs()); }
		void cboADTrigType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboADTrigType.SelectedIndex == -1) return;

			comboVarRefresh(cboADTrigType.SelectedIndex, cboADTrigVar);
			try { cboADTrigVar.SelectedIndex = _activeArrDepTrigger.Variable; }
			catch { cboADTrigVar.SelectedIndex = 0; }
		}
		void cmdCopyAD_Click(object sender, EventArgs e) => menuCopy_Click("AD", new EventArgs());
		void cmdPasteAD_Click(object sender, EventArgs e) => menuPaste_Click("AD", new EventArgs());
		void optArrMS_CheckedChanged(object sender, EventArgs e) => cboArrMS.Enabled = optArrMS.Checked;
		void optArrMSAlt_CheckedChanged(object sender, EventArgs e) => cboArrMSAlt.Enabled = optArrMSAlt.Checked;
		void optDepMS_CheckedChanged(object sender, EventArgs e) => cboDepMS.Enabled = optDepMS.Checked;
		void optDepMSAlt_CheckedChanged(object sender, EventArgs e) => cboDepMSAlt.Enabled = optDepMSAlt.Checked;
		#endregion
		#region Orders
		FlightGroup.Order _activeOrder => _activeFG.Orders[_activeOrderIndex];
		void orderLabelRefresh()
		{
			string orderText = _activeOrder.ToString();
			orderText = replaceTargetText(orderText);
			if (_activeOrder.Command == (byte)FlightGroup.Order.CommandList.DropOff && _activeOrder.Variable2 >= 1 && _activeOrder.Variable2 <= _mission.FlightGroups.Count)
				orderText += ", " + _mission.FlightGroups[_activeOrder.Variable2 - 1].ToString(false);
			lblOrder[_activeOrderIndex].Text = "Order " + (_activeOrderIndex + 1) + ": " + orderText;
		}

		void lblOrderArr_Click(object sender, EventArgs e)
		{
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeOrderIndex = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeOrderIndex = Convert.ToByte(sender); l = lblOrder[_activeOrderIndex]; }
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 3; i++) if (i != _activeOrderIndex) setInteractiveLabelColor(lblOrder[i], false);
			bool btemp = _loading;
			_loading = true;
			cboOrders.SelectedIndex = _activeOrder.Command;
			cboOThrottle.SelectedIndex = _activeOrder.Throttle;
			numOVar1.Value = _activeOrder.Variable1;
			numOVar2.Value = _activeOrder.Variable2;
			cboOT3Type.SelectedIndex = -1;
			cboOT3Type.SelectedIndex = _activeOrder.Target3Type;
			cboOT4Type.SelectedIndex = -1;
			cboOT4Type.SelectedIndex = _activeOrder.Target4Type;
			optOT3T4OR.Checked = _activeOrder.T3OrT4;
			optOT3T4AND.Checked = !optOT3T4OR.Checked;
			cboOT1Type.SelectedIndex = -1;
			cboOT1Type.SelectedIndex = _activeOrder.Target1Type;
			cboOT2Type.SelectedIndex = -1;
			cboOT2Type.SelectedIndex = _activeOrder.Target2Type;
			optOT1T2OR.Checked = _activeOrder.T1OrT2;
			optOT1T2AND.Checked = !optOT1T2OR.Checked;
			_loading = btemp;
		}
		void lblOrderArr_DoubleClick(object sender, EventArgs e) => menuPaste_Click("Order", new EventArgs());
		void lblOrderArr_MouseUp(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Right) menuCopy_Click("Order", new EventArgs()); }

		void cboOrders_SelectedIndexChanged(object sender, EventArgs e)
		{
			string[] s = Strings.OrderDesc[cboOrders.SelectedIndex].Split('|');
			lblODesc.Text = s[0];
			lblOVar1.Text = s[1];
			lblOVar2.Text = s[2];
			numOVar1_ValueChanged(0, new EventArgs());
			numOVar2_ValueChanged(0, new EventArgs());
		}
		void cboOT1Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT1Type.SelectedIndex == -1) return;

			comboVarRefresh(cboOT1Type.SelectedIndex, cboOT1);
			try { cboOT1.SelectedIndex = _activeOrder.Target1; }
			catch { cboOT1.SelectedIndex = 0; }
		}
		void cboOT2Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT2Type.SelectedIndex == -1) return;

			comboVarRefresh(cboOT2Type.SelectedIndex, cboOT2);
			try { cboOT2.SelectedIndex = _activeOrder.Target2; }
			catch { cboOT2.SelectedIndex = 0; }
		}
		void cboOT3Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT3Type.SelectedIndex == -1) return;

			comboVarRefresh(cboOT3Type.SelectedIndex, cboOT3);
			try { cboOT3.SelectedIndex = _activeOrder.Target3; }
			catch { cboOT3.SelectedIndex = 0; }
		}
		void cboOT4Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT4Type.SelectedIndex == -1) return;

			comboVarRefresh(cboOT4Type.SelectedIndex, cboOT4);
			try { cboOT4.SelectedIndex = _activeOrder.Target4; }
			catch { cboOT4.SelectedIndex = 0; }
		}
		void cmdCopyOrder_Click(object sender, EventArgs e) => menuCopy_Click("Order", new EventArgs());
		void cmdPasteOrder_Click(object sender, EventArgs e) => menuPaste_Click("Order", new EventArgs());

		void numOVar1_ValueChanged(object sender, EventArgs e)
		{
			byte variable = _activeOrder.Variable1;
			var command = (FlightGroup.Order.CommandList)_activeOrder.Command;
			string text = "";
			bool warning = false;
			switch (command)
			{
				case FlightGroup.Order.CommandList.BoardGiveCargo:
				case FlightGroup.Order.CommandList.BoardTakeCargo:
				case FlightGroup.Order.CommandList.BoardExchangeCargo:
				case FlightGroup.Order.CommandList.BoardToCapture:
				case FlightGroup.Order.CommandList.BoardDestroy:
				case FlightGroup.Order.CommandList.PickUp:
				case FlightGroup.Order.CommandList.Wait:
				case FlightGroup.Order.CommandList.SSWait:
				case FlightGroup.Order.CommandList.SSHold:
				case FlightGroup.Order.CommandList.SSWait2:
				case FlightGroup.Order.CommandList.SSBoard:
				case FlightGroup.Order.CommandList.BoardRepair:
					text = Common.GetFormattedTime(variable * 5, true);
					break;
				case FlightGroup.Order.CommandList.Circle:
				case FlightGroup.Order.CommandList.CircleEvade:
				case FlightGroup.Order.CommandList.SSPatrol:
					if (variable == 0) { text = "Zero, no loops!"; warning = true; }
					else if (variable >= 128) { text = ((sbyte)variable).ToString() + ", no loops"; warning = true; }
					break;
				case FlightGroup.Order.CommandList.Escort:
					if (variable < 9) text = "Above";
					else if (variable > 17) text = "Below";
					if (variable % 3 == 0) text += " Left";
					else if (variable % 3 == 2) text += " Right";
					if (variable == 13 || variable == 27) text = "Coincident";    // the only one that won't be caught otherwise
					variable = (byte)(variable % 9);
					if (variable < 3 && numOVar1.Value != 27) text = "Leading " + text;
					else if (variable > 5) text = "Trailing " + text;
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
			var command = (FlightGroup.Order.CommandList)_activeOrder.Command;
			int variable = _activeOrder.Variable2;
			string text = "";
			bool warning = false;
			switch (command)
			{
				case FlightGroup.Order.CommandList.BoardGiveCargo:
				case FlightGroup.Order.CommandList.BoardTakeCargo:
				case FlightGroup.Order.CommandList.BoardExchangeCargo:
				case FlightGroup.Order.CommandList.BoardToCapture:
				case FlightGroup.Order.CommandList.BoardDestroy:
				case FlightGroup.Order.CommandList.PickUp:
				case FlightGroup.Order.CommandList.SSBoard:
				case FlightGroup.Order.CommandList.BoardRepair:
					warning = (variable == 0);
					if (variable == 0) text = "No dockings.";
					break;
				case FlightGroup.Order.CommandList.DropOff:
					if (variable >= 1 && variable <= _mission.FlightGroups.Count)   //Variable is FG #, one based.
						text = _mission.FlightGroups[variable - 1].ToString(false);
					else
					{
						text = "None specified.";
						warning = true;
					}
					if (ActiveControl == numOVar2) orderLabelRefresh(); //Instant update the order.
					break;
			}
			lblOVar2Note.Text = text;
			lblOVar2Note.Visible = (text != "");
			lblOVar2Note.ForeColor = warning ? Color.Red : SystemColors.ControlText;
		}
		#endregion
		#region Goals
		void cboGoalArr_Leave(object sender, EventArgs e)
		{
			ComboBox c = (ComboBox)sender;
			foreach(FlightGroup fg in getSelectedFlightgroups())
				fg.Goals[(int)c.Tag] = Common.Update(this, fg.Goals[(int)c.Tag], Convert.ToByte(cboGoal[(int)c.Tag].SelectedIndex));
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
			if (_fMap == null || _fMap.IsDisposed) return;
			
			if (fgIndex < 0) _fMap.Import(_mission.FlightGroups);
			else if (fgIndex < _mission.FlightGroups.Count) _fMap.UpdateFlightGroup(fgIndex, _mission.FlightGroups[fgIndex]);
			_fMap.MapPaint();
		}
		void refreshWaypointTab()
		{
			bool btemp = _loading;
			_loading = true;
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					_tableRaw.Rows[i][j] = _activeFG.Waypoints[i][j];
					_table.Rows[i][j] = Math.Round((double)_activeFG.Waypoints[i][j] / 160, 2);
				}
				chkWP[i].Checked = _activeFG.Waypoints[i].Enabled;
			}
			_table.AcceptChanges();
			_tableRaw.AcceptChanges();
			numYaw.Value = _activeFG.Yaw;
			numPitch.Value = _activeFG.Pitch;
			numRoll.Value = _activeFG.Roll;
			enableRot(_activeFG.CraftType > 0x45);
			_loading = btemp;
		}

		void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			CheckBox c = (CheckBox)sender;
			foreach(FlightGroup fg in getSelectedFlightgroups()) fg.Waypoints[(int)c.Tag].Enabled = Common.Update(this, fg.Waypoints[(int)c.Tag].Enabled, c.Checked);
			refreshMap(_activeFGIndex);
		}
		void table_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;

			_loading = true;
			int j;
			for (j = 0; j < 15; j++) if (_table.Rows[j].Equals(e.Row)) break;   //find the row index that you're changing
			for (int i = 0; i < 3; i++)
			{
				if (!double.TryParse(_table.Rows[j][i].ToString(), out double cell)) _table.Rows[j][i] = 0;
				short raw = (short)(cell * 160);
				foreach(FlightGroup fg in getSelectedFlightgroups()) fg.Waypoints[j][i] = Common.Update(this, fg.Waypoints[j][i], raw);
				_tableRaw.Rows[j][i] = raw;
			}
			_loading = false;
			refreshMap(_activeFGIndex);
		}
		void tableRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (_loading) return;

			_loading = true;
			int j;
			for (j = 0; j < 15; j++) if (_tableRaw.Rows[j].Equals(e.Row)) break;    //find the row index that you're changing
			for (int i = 0; i < 3; i++)
			{
				if (!short.TryParse(_tableRaw.Rows[j][i].ToString(), out short raw)) _tableRaw.Rows[j][i] = 0;
				foreach (FlightGroup fg in getSelectedFlightgroups()) fg.Waypoints[j][i] = Common.Update(this, fg.Waypoints[j][i], raw);
				_table.Rows[j][i] = Math.Round((double)raw / 160, 2);
			}
			_loading = false;
			refreshMap(_activeFGIndex);
		}
		#endregion
		#endregion
		#region Messages
		Platform.Tie.Message _activeMessage => _mission.Messages[_activeMessageIndex];
		Mission.Trigger _activeMessTrig => _activeMessage.Triggers[_activeMessageTrigIndex];
		void deleteMess()
		{
			List<int> selection = Common.GetSelectedIndices(lstMessages);
			if (selection.Count == 0) return;

			int startMsg = _activeMessageIndex;
			for (int si = selection.Count - 1; si >= 0; si--)
			{
				_mission.Messages.RemoveAt(selection[si]);
				lstMessages.Items.RemoveAt(selection[si]);
			}
			Common.MarkDirty(this);
			if (_mission.Messages.Count == 0)
			{
				lstMessages.Items.Clear();
				enableMessage(false);
				lblMessage.Text = "Message #0 of 0";
				return;
			}
			if (startMsg >= _mission.Messages.Count) startMsg = _mission.Messages.Count - 1;
			lstMessages.SelectedIndex = startMsg;
		}
		void enableMessage(bool state)
		{
			grpMessages.Enabled = state;
			txtMessage.Enabled = state;
			txtShort.Enabled = state;
			numMessDelay.Enabled = state;
			cboMessTrig.Enabled = state;
			cboMessType.Enabled = state;
			cboMessVar.Enabled = state;
			cboMessAmount.Enabled = state;
			cboMessColor.Enabled = state;
		}
		void messRefreshItem(int index)
		{
			bool btemp = _noRefresh;
			_noRefresh = true;                            // Modifying an item will invoke SelectedIndexChanged.
			bool state = lstMessages.GetSelected(index);  // It may also interfere with the current selection state.
			lstMessages.Items[index] = _mission.Messages[index].MessageString != "" ? _mission.Messages[index].MessageString : " *";
			lstMessages.SetSelected(index, state);
			_noRefresh = btemp;
		}
		void messRefreshSelectedItems() { foreach (int i in Common.GetSelectedIndices(lstMessages)) messRefreshItem(i); }
		List<Platform.Tie.Message> getSelectedMessages()
		{
			List<Platform.Tie.Message> msgs = new List<Platform.Tie.Message>();
			foreach (int msgIndex in lstMessages.SelectedIndices) msgs.Add(_mission.Messages[msgIndex]);
			return msgs;
		}
		void setMessageProperty(string name, object value)
		{
			int trigRefresh = 0;
			foreach (Platform.Tie.Message msg in getSelectedMessages())
			{
				switch (name)
				{
					case "MessTrigger":
						Mission.Trigger trig = getTriggerFromControls(cboMessAmount, cboMessType, cboMessVar, cboMessTrig);
						msg.Triggers[_activeMessageTrigIndex] = trig;
						if (trigRefresh++ == 0) labelRefresh(trig, _activeMessageTrigIndex == 0 ? lblMess1 : lblMess2);  // only refresh once
						break;
					case "MessColor": msg.Color = Convert.ToByte(value); break;
					case "MessDelay": msg.RawDelay = Convert.ToByte(value); break;
					case "Mess1OR2": msg.Trig1OrTrig2 = Convert.ToBoolean(value); break;
				}
			}
		}
		void moveMessages(int direction)
		{
			List<int> selection = Common.GetSelectedIndices(lstMessages);
			if (selection.Count == 0 || (direction == -1 && selection[0] == 0) || (direction == 1 && selection[selection.Count - 1] == lstMessages.Items.Count - 1)) return;

			for (int i = 0; i < selection.Count; i++)
			{
				// Traverse the selection list forward if moving up, backward if moving down.
				int accessIndex = ((direction == -1) ? i : selection.Count - 1 - i);
				int msgIndex = selection[accessIndex];
				Platform.Tie.Message tmp = _mission.Messages[msgIndex];
				_mission.Messages[msgIndex] = _mission.Messages[msgIndex + direction];
				_mission.Messages[msgIndex + direction] = tmp;
				messRefreshItem(msgIndex);
				messRefreshItem(msgIndex + direction);
				selection[accessIndex] += direction;     // Adjust indices to new positions
			}
			_activeMessageIndex += direction;

			Common.SetSelectedIndices(lstMessages, selection, ref _noRefresh);  // Apply adjusted indices
			Common.MarkDirty(this);
			Common.UpdateMoveButtons(cmdMoveMessUp, cmdMoveMessDown, lstMessages);
		}
		bool newMess()
		{
			if (_mission.Messages.Count == Mission.MessageLimit)
			{
				MessageBox.Show("Mission contains maximum number of Messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			_activeMessageIndex = _mission.Messages.Add();
			if (_mission.Messages.Count == 1) enableMessage(true);
			lstMessages.Items.Add(_activeMessage.MessageString);
			lstMessages.ClearSelected();
			lstMessages.SelectedIndex = _activeMessageIndex;
			Common.MarkDirty(this, _loading);
			return true;
		}
		void cmdMoveMessUp_Click(object sender, EventArgs e) => moveMessages(-1);
		void cmdMoveMessDown_Click(object sender, EventArgs e) => moveMessages(1);
		void lblMessArr_Click(object sender, EventArgs e)
		{
			if (_mission.Messages.Count == 0) return;

			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeMessageTrigIndex = Convert.ToByte(lblMess1 == l ? 0 : 1);    // selected
			}
			catch (InvalidCastException) { _activeMessageTrigIndex = Convert.ToByte(sender); l = (_activeMessageTrigIndex == 0 ? lblMess1 : lblMess2); }
			setInteractiveLabelColor(l, true);
			setInteractiveLabelColor((_activeMessageTrigIndex == 0 ? lblMess2 : lblMess1), false);
			bool btemp = _loading;
			_loading = true;
			cboMessTrig.SelectedIndex = _activeMessTrig.Condition;
			cboMessType.SelectedIndex = -1;
			cboMessType.SelectedIndex = _activeMessTrig.VariableType;
			cboMessAmount.SelectedIndex = _activeMessTrig.Amount;
			_loading = btemp;
		}
		void lblMessArr_DoubleClick(object sender, EventArgs e) => menuPaste_Click("MessTrig", new EventArgs());
		void lblMessArr_MouseUp(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Right) menuCopy_Click("MessTrig", new EventArgs()); }
		void cboMessType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMessType.SelectedIndex == -1) return;

			comboVarRefresh(cboMessType.SelectedIndex, cboMessVar);
			try { cboMessVar.SelectedIndex = _activeMessTrig.Variable; }
			catch { cboMessVar.SelectedIndex = 0; }
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
					brText = Brushes.Red; //Crimson;
					break;
				case 1:
					brText = Brushes.Lime; //LimeGreen;
					break;
				case 2:
					brText = Brushes.DodgerBlue; //RoyalBlue;
					break;
				case 3:
					brText = Brushes.MediumOrchid; //DarkOrchid;
					break;
			}

			bool used = true;
			// evaluate the FALSE conditions to detect if it's locked into NEVER
			bool[] never = new bool[3];
			for (int i = 0; i < 2; i++) never[i] = (mess.Triggers[i].Condition == 10);
			never[2] = ((never[0] || never[1]) && !mess.Trig1OrTrig2) || (never[0] && never[1]);   // T1/2 pair
			if (never[2]) used = false;
			if (!used) brText = Brushes.Gray;

			e.Graphics.DrawString(lstMessages.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			Common.UpdateMoveButtons(cmdMoveMessUp, cmdMoveMessDown, lstMessages);  // Running this first lets it detect if all messages have been deselected
			if (lstMessages.SelectedIndex == -1 || _noRefresh) return;

			_activeMessageIndex = lstMessages.SelectedIndex;
			lblMessage.Text = "Message #" + (_activeMessageIndex + 1) + " of " + _mission.Messages.Count;
			bool btemp = _loading;
			_loading = true;
			labelRefresh(_activeMessage.Triggers[0], lblMess1);
			labelRefresh(_activeMessage.Triggers[1], lblMess2);
			txtMessage.Text = _activeMessage.MessageString;
			txtShort.Text = _activeMessage.Short;
			cboMessColor.SelectedIndex = _activeMessage.Color;
			numMessDelay.Value = _activeMessage.RawDelay;
			optMessOR.Checked = _activeMessage.Trig1OrTrig2;
			optMessAND.Checked = !optMessOR.Checked;
			lblMessArr_Click(0, new EventArgs());
			_loading = btemp;
		}

		void numMessDelay_ValueChanged(object sender, EventArgs e)
		{
			_activeMessage.RawDelay = Common.Update(this, _activeMessage.RawDelay, (byte)numMessDelay.Value);
			lblMessDelay.Text = "= " + (int)(numMessDelay.Value / 12) + ":" + (numMessDelay.Value * 5 % 60).ToString("00");
		}

		void txtMessage_TextChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_activeMessage.MessageString = Common.Update(this, _activeMessage.MessageString, txtMessage.Text);
			messRefreshItem(_activeMessageIndex);
		}
		void txtShort_Leave(object sender, EventArgs e) => _activeMessage.Short = Common.Update(this, _activeMessage.Short, txtShort.Text);
		#endregion
		#region Globals
		Globals.Goal _activeGlobalGoal => _mission.GlobalGoals.Goals[_activeGlobalGoalIndex / 2];
		Mission.Trigger _activeGlobalTrigger => _activeGlobalGoal.Triggers[_activeGlobalGoalIndex % 2];
		void lblGlobArr_Click(object sender, EventArgs e)
		{
			Label l;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeGlobalGoalIndex = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeGlobalGoalIndex = Convert.ToByte(sender); l = lblGlob[_activeGlobalGoalIndex]; }
			setInteractiveLabelColor(l, true);
			for (int i = 0; i < 6; i++) if (i != _activeGlobalGoalIndex) setInteractiveLabelColor(lblGlob[i], false);
			bool btemp = _loading;
			_loading = true;
			cboGlobalTrig.SelectedIndex = _activeGlobalTrigger.Condition;
			cboGlobalType.SelectedIndex = -1;
			cboGlobalType.SelectedIndex = _activeGlobalTrigger.VariableType;
			cboGlobalAmount.SelectedIndex = _activeGlobalTrigger.Amount;
			txtGlobalNote.Text = _activeGlobalGoal.Name;
			_loading = btemp;
		}
		void lblGlobArr_DoubleClick(object sender, EventArgs e) => menuPaste_Click("Glob", new EventArgs());
		void lblGlobArr_MouseUp(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Right) menuCopy_Click("Glob", new EventArgs()); }

		void cboGlobalAmount_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading) _activeGlobalTrigger.Amount = Common.Update(this, _activeGlobalTrigger.Amount, Convert.ToByte(cboGlobalAmount.SelectedIndex));
			labelRefresh(_activeGlobalTrigger, lblGlob[_activeGlobalGoalIndex]);
		}
		void cboGlobalTrig_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading) _activeGlobalTrigger.Condition = Common.Update(this, _activeGlobalTrigger.Condition, Convert.ToByte(cboGlobalTrig.SelectedIndex));
			labelRefresh(_activeGlobalTrigger, lblGlob[_activeGlobalGoalIndex]);
		}
		void cboGlobalType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalType.SelectedIndex == -1) return;

			if (!_loading) _activeGlobalTrigger.VariableType = Common.Update(this, _activeGlobalTrigger.VariableType, Convert.ToByte(cboGlobalType.SelectedIndex));
			comboVarRefresh(_activeGlobalTrigger.VariableType, cboGlobalVar);
			try { cboGlobalVar.SelectedIndex = _activeGlobalTrigger.Variable; }
			catch { cboGlobalVar.SelectedIndex = 0; _activeGlobalTrigger.Variable = 0; }
			labelRefresh(_activeGlobalTrigger, lblGlob[_activeGlobalGoalIndex]);
		}
		void cboGlobalVar_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading) _activeGlobalTrigger.Variable = Common.Update(this, _activeGlobalTrigger.Variable, Convert.ToByte(cboGlobalVar.SelectedIndex));
			labelRefresh(_activeGlobalTrigger, lblGlob[_activeGlobalGoalIndex]);
		}

		void optBonOR_CheckedChanged(object sender, EventArgs e) { if (!_loading) _mission.GlobalGoals.Goals[2].T1AndOrT2 = Common.Update(this, _mission.GlobalGoals.Goals[2].T1AndOrT2, optBonOR.Checked); }
		void optPrimOR_CheckedChanged(object sender, EventArgs e) { if (!_loading) _mission.GlobalGoals.Goals[0].T1AndOrT2 = Common.Update(this, _mission.GlobalGoals.Goals[0].T1AndOrT2, optPrimOR.Checked); }
		void optSecOR_CheckedChanged(object sender, EventArgs e) { if (!_loading) _mission.GlobalGoals.Goals[1].T1AndOrT2 = Common.Update(this, _mission.GlobalGoals.Goals[1].T1AndOrT2, optSecOR.Checked); }

		void txtGlobalNote_TextChanged(object sender, EventArgs e) { if (!_loading) _activeGlobalGoal.Name = Common.Update(this, _activeGlobalGoal.Name, txtGlobalNote.Text); }
		#endregion
		#region Officers
		/// <summary>Verify length and inform the user if it might cause memory problems within the game.</summary>
		void checkQuestionLength()
		{
			int qi = (cboOfficer.SelectedIndex * 5) + cboQuestion.SelectedIndex;
			int size = txtQuestion.Text.Length + txtAnswer.Text.Length + 2;  //Strings + 2 bytes (Q/A separator, reserved null terminator in game)
			if (qi >= 10) size += 2;  //Condition byte, Type byte also consume space

			string note;
			if (size > 0x400) note = "Warning! Game text limit exceeded!" + Environment.NewLine + (size - 0x400) + " character" + ((size - 0x400 > 1) ? "s" : "") + " over limit!";
			else note = "Text space remaining:" + Environment.NewLine + (0x400 - size) + " character" + (0x400 - size != 1 ? "s" : "");

			lblQuestionNote.Text = note;
			lblQuestionNote.Visible = (note != "");
		}

		void cboFacial_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading || cboFacial.SelectedIndex == -1) return;

			int face = cboFacial.SelectedIndex + 1;
			if (cboOfficer.SelectedIndex == 0) _mission.BriefingQuestions.OfficerFacialExpressions[cboQuestion.SelectedIndex] = (Questions.OfficerFacialExpression)Common.Update(this, (int)_mission.BriefingQuestions.OfficerFacialExpressions[cboQuestion.SelectedIndex], face);
			else if (cboOfficer.SelectedIndex == 1) _mission.BriefingQuestions.SecretOrderExpressions[cboQuestion.SelectedIndex] = (Questions.SecretOrderExpression)Common.Update(this, (int)_mission.BriefingQuestions.SecretOrderExpressions[cboQuestion.SelectedIndex], face);
			else if (cboOfficer.SelectedIndex == 2) _mission.BriefingQuestions.OfficerFacialExpressions[cboQuestion.SelectedIndex + 5] = (Questions.OfficerFacialExpression)Common.Update(this, (int)_mission.BriefingQuestions.OfficerFacialExpressions[cboQuestion.SelectedIndex + 5], face);
			else _mission.BriefingQuestions.SecretOrderExpressions[cboQuestion.SelectedIndex + 5] = (Questions.SecretOrderExpression)Common.Update(this, (int)_mission.BriefingQuestions.SecretOrderExpressions[cboQuestion.SelectedIndex + 5], face);
		}
		void cboOfficer_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool btemp = _loading;
			_loading = true;
			cboFacial.Items.Clear();
			if (cboOfficer.SelectedIndex % 2 == 0) cboFacial.Items.AddRange(Enum.GetNames(typeof(Questions.OfficerFacialExpression)));
			else cboFacial.Items.AddRange(Enum.GetNames(typeof(Questions.SecretOrderExpression)));
			_loading = btemp;
			cboQuestion.SelectedIndex = 0;
			cboQTrig.Enabled = (cboOfficer.SelectedIndex > 1);
			cboQTrigType.Enabled = cboQTrig.Enabled;
			cboQuestion_SelectedIndexChanged("cboOfficer", new EventArgs());
		}
		void cboQTrig_Leave(object sender, EventArgs e)
		{
			if (cboOfficer.SelectedIndex < 2) return;

			int index = ((cboOfficer.SelectedIndex == 2) ? 0 : 5) + cboQuestion.SelectedIndex;
			byte trig = Convert.ToByte(cboQTrig.SelectedIndex);
			if (trig != 0) trig += 3;
			_mission.BriefingQuestions.PostTrigger[index] = (Questions.QuestionCondition)Common.Update(this, (byte)_mission.BriefingQuestions.PostTrigger[index], trig);
		}
		void cboQTrigType_Leave(object sender, EventArgs e)
		{
			if (cboOfficer.SelectedIndex < 2) return;

			int index = ((cboOfficer.SelectedIndex == 2) ? 0 : 5) + cboQuestion.SelectedIndex;
			_mission.BriefingQuestions.PostTrigType[index] = (Questions.QuestionType)Common.Update(this, (byte)_mission.BriefingQuestions.PostTrigType[index], Convert.ToByte(cboQTrigType.SelectedIndex + 1));
		}
		void cboQuestion_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool bTemp = _loading;
			_loading = true;
			int a = 0;  //place holder
			if (cboOfficer.SelectedIndex <= 1)  //if pre-miss
			{
				if (cboOfficer.SelectedIndex == 1) a = 5;   //if Secret Order, set place holder
				txtQuestion.Text = _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + a];
				txtAnswer.Text = _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + a];
				if (cboOfficer.SelectedIndex == 0) cboFacial.SelectedIndex = (int)_mission.BriefingQuestions.OfficerFacialExpressions[cboQuestion.SelectedIndex] - 1;
				else cboFacial.SelectedIndex = (int)_mission.BriefingQuestions.SecretOrderExpressions[cboQuestion.SelectedIndex] - 1;
			}
			else    //post-miss
			{
				if (cboOfficer.SelectedIndex == 3) a = 5;
				cboQTrigType.SelectedIndex = (int)_mission.BriefingQuestions.PostTrigType[cboQuestion.SelectedIndex + a] - 1;
				int trig = (int)_mission.BriefingQuestions.PostTrigger[cboQuestion.SelectedIndex + a];
				if (trig == 0) cboQTrig.SelectedIndex = 0;
				else cboQTrig.SelectedIndex = trig - 3;
				txtQuestion.Text = _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + a];
				txtAnswer.Text = _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + a];
				if (cboOfficer.SelectedIndex == 2) cboFacial.SelectedIndex = (int)_mission.BriefingQuestions.OfficerFacialExpressions[cboQuestion.SelectedIndex + 5] - 1;
				else cboFacial.SelectedIndex = (int)_mission.BriefingQuestions.SecretOrderExpressions[cboQuestion.SelectedIndex + 5] - 1;
			}
			_loading = bTemp;
		}

		void cmdBestFit_Click(object sender, EventArgs e)
		{
			txtAnswer_Leave("", new EventArgs());
			string output = txtAnswer.Text;
			output = output.Replace("\r", "");
			output = output.Replace("\n\n", "$$");  //Double newlines should stay.
			output = output.Replace("   ", "@@@");  //Triple spaces should stay.
			output = output.Replace("  ", "@@");    //Double ...
			output = output.Replace("\n ", "$@");   //Newlines with at least once space is an indent and should stay.
			output = output.Replace("\n@", "$@");   //Newlines with double spaces (which have already been converted) should stay.
			output = output.Replace("\n", " ");     //Replace all other newlines with spaces.
			output = output.Replace("  ", " ");     //Collapse double spaces caused by replaced newlines.
			output = output.Replace("$", "\n");     //Restore the preserved newlines.
			output = output.Replace("@", " ");      //Restore the preserved spaces.

			try
			{
				string path = _config.TiePath + "\\RESOURCE\\";
				LfdFile _empire = new LfdFile(path + "EMPIRE.LFD");

				Pltt standard = (Pltt)_empire.Resources["PLTTstandard"];
				LfdReader.Font font8 = (LfdReader.Font)_empire.Resources["FONTfont8"];

				System.Text.StringBuilder st = new System.Text.StringBuilder();
				System.Text.StringBuilder word = new System.Text.StringBuilder();
				int offset = 0;
				byte glyph = 0;
				int pos = 0;
				char c;
				int length = output.Length;
				while (pos < length)
				{
					c = output[pos];
					if (c == '\n')
					{
						offset = 0;
						st.Append(c);
					}
					else if (c == '[' || c == ']') { st.Append(c); }
					else //It's a character or space, calculate the word length to determine if a newline should be entered before the word.
					{
						word.Clear();
						int pixLen = 0;
						bool newLine = false;
						int startPos = pos;
						for (pos = startPos; pos < length; pos++)
						{
							char d = output[pos];
							if (d == '\n' || (d == ' ' && pos != startPos))
							{
								pos--; //step back to check it again at the start of the next word
								break;
							}
							word.Append(d);

							if (d != '[' && d != ']')  //Used to represent highlighting and not rendered.
							{
								glyph = Convert.ToByte(d - font8.StartingChar);
								int cwidth = font8.Glyphs[glyph].Width + 1;
								if (offset + pixLen + cwidth > 198) newLine = true;

								if (pos - startPos > 30) break;  //Forcefully break up extremely long "words" although the user will probably need to manually adjust these.  30 is about as many of the widest characters that can appear in one row.

								pixLen += cwidth;
							}
						}
						if (newLine == true)
						{
							st.Append('\n');
							if (word.Length >= 2 && word[0] == ' ' && word[1] != ' ')
							{
								string temp = word.ToString().Substring(1);
								word.Clear();
								word.Append(temp);
							}
							offset = 0;
							newLine = false;
						}
						st.Append(word);
						offset += pixLen;
					}
					pos++;
				}
				output = st.ToString();
				output = output.Replace("\n", Environment.NewLine);
				if (txtAnswer.Text != output)
				{
					txtAnswer.Text = output;
					Common.MarkDirty(this);
				}
			}
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}
		void cmdPreview_Click(object sender, EventArgs e)
		{
			txtAnswer_Leave("cmdPreview", new EventArgs());
			txtQuestion_Leave("cmdPreview", new EventArgs());
			_fOfficers?.Close();

            _fOfficers = new OfficerPreviewForm(_mission.BriefingQuestions, cboOfficer.SelectedIndex, cboQuestion.SelectedIndex);
			_fOfficers.Show();
		}

		void optOfficers_Leave(object sender, EventArgs e)
		{
			RadioButton o = (RadioButton)sender;
			_mission.OfficersPresent = Common.Update(this, _mission.OfficersPresent, (Mission.BriefingOfficers)Convert.ToByte(o.Tag));
		}

		void txtAnswer_Leave(object sender, EventArgs e)
		{
			string t;
			if (cboOfficer.SelectedIndex == 0) t = _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex];
			else if (cboOfficer.SelectedIndex == 1) t = _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + 5];
			else if (cboOfficer.SelectedIndex == 2) t = _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex];
			else t = _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + 5];
			t = Common.Update(this, t, txtAnswer.Text);
			if (cboOfficer.SelectedIndex == 0) _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex] = t;
			else if (cboOfficer.SelectedIndex == 1) _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + 5] = t;
			else if (cboOfficer.SelectedIndex == 2) _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex] = t;
			else _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + 5] = t;
			checkQuestionLength();
		}
		void txtAnswer_TextChanged(object sender, EventArgs e) => checkQuestionLength();
		void txtQuestion_Leave(object sender, EventArgs e)
		{
			string t;
			if (cboOfficer.SelectedIndex == 0) t = _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex];
			else if (cboOfficer.SelectedIndex == 1) t = _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + 5];
			else if (cboOfficer.SelectedIndex == 2) t = _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex];
			else t = _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + 5];
			t = Common.Update(this, t, txtQuestion.Text);
			if (cboOfficer.SelectedIndex == 0) _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex] = t;
			else if (cboOfficer.SelectedIndex == 1) _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + 5] = t;
			else if (cboOfficer.SelectedIndex == 2) _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex] = t;
			else _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + 5] = t;
			checkQuestionLength();
		}
		#endregion
		#region Mission
		void cboEoMColorArr_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox c = (ComboBox)sender;
			if (!_loading) _mission.EndOfMissionMessageColor[(int)c.Tag] = Common.Update(this, _mission.EndOfMissionMessageColor[(int)c.Tag], Convert.ToByte(c.SelectedIndex));
			Color clr = Color.Red;
			if (c.SelectedIndex == 1) clr = Color.Lime;
			else if (c.SelectedIndex == 2) clr = Color.DodgerBlue;
			else if (c.SelectedIndex == 3) clr = Color.MediumOrchid;
			txtEoM[(int)c.Tag].ForeColor = clr;
		}

		void chkIFFArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			_mission.IffHostile[(int)c.Tag] = Common.Update(this, _mission.IffHostile[(int)c.Tag], c.Checked);
		}
		void chkIFF5_CheckedChanged(object sender, EventArgs e) => chkIFF5.Checked = true;

		void numTimeLimitMin_Leave(object sender, EventArgs e) => _mission.TimeLimitMin = Common.Update(this, _mission.TimeLimitMin, (byte)numTimeLimitMin.Value);
		void numTimeLimitSec_Leave(object sender, EventArgs e) => _mission.TimeLimitSec = Common.Update(this, _mission.TimeLimitSec, (byte)numTimeLimitSec.Value);
		void numRndSeed_Leave(object sender, EventArgs e) => _mission.RandomSeed = Common.Update(this, _mission.RandomSeed, (byte)numRndSeed.Value);

		void txtEoMArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.EndOfMissionMessages[(int)t.Tag] = Common.Update(this, _mission.EndOfMissionMessages[(int)t.Tag], t.Text);
		}
		void txtIFFArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.IFFs[(int)t.Tag] = Common.Update(this, _mission.IFFs[(int)t.Tag], t.Text);
			comboReset(cboIFF, getIffStrings(), cboIFF.SelectedIndex);
		}
		#endregion 
	}
}