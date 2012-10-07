/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.2.1
 */

/* CHANGELOG
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
using Idmr.Platform.Tie;

namespace Idmr.Yogeme
{
	/// <summary>TIE95 Mission Editor GUI</summary>
	public partial class TieForm : Form
	{
		#region vars and stuff
		Settings _config;
		Mission _mission;
		bool _applicationExit;				//for frmTIE_Closing, confirms application exit vs switching platforms
		int _activeFG = 0;			//counter to keep track of current FG being displayed
		int _startingShips = 1;		//counter for craft in play at start <30s, warning above 28
		bool _loading;		//alerts certain functions to disable during the loading process
		int _activeMessage = 0;
		DataView _dataWaypoints;
		DataTable _table = new DataTable("Waypoints");
		DataTable _tableRaw = new DataTable("Waypoints_Raw");
		DataView _dataWaypointsRaw;
		MapForm _fMap;
		BriefingForm _fBrief;
		BattleForm _fBattle;
		OfficerPreviewForm _fOfficers;
		byte _activeGlobalGoal;
		byte _activeArrDepTrigger;
		byte _activeOrder;
		#endregion
		#region Control Arrays
		Label[] lblGlob = new Label[6];
		TextBox[] txtEoM = new TextBox[6];
		CheckBox[] chkIFF = new CheckBox[6];
		TextBox[] txtIFF = new TextBox[6];
		Label[] lblADTrig = new Label[3];
		ComboBox[] cboGoal = new ComboBox[8];
		Label[] lblOrder = new Label[3];
		CheckBox[] chkWP = new CheckBox[15];
		NumericUpDown[] numUnk = new NumericUpDown[9];
		RadioButton[] optOfficers = new RadioButton[4];
		MenuItem[] menuRecentMissions = new MenuItem[6];
		#endregion

		public TieForm(Settings settings)
		{
			_config = settings;
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		public TieForm(Settings settings, string path)
		{	//this is the command line and "Open..." support
			_config = settings;
			InitializeComponent();
			_loading = true;
			initializeMission();
			startup();
			if(!loadMission(path)) return;
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
			try { _fBattle.Close(); }
			catch { /* do nothing */ }
			try { _fOfficers.Close(); }
			catch { /* do nothing */ }
		}
		void comboVarRefresh(int index, ComboBox cbo)
		{	//index is usually cboType.SelectedIndex, cbo = cboVar
			if (index == -1) return;
			cbo.Items.Clear();
			switch (index)		//switch (VariableType)
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
					string[] t = new string[_mission.IFFs.Length];
					for (int i = 0; i < t.Length; i++) t[i] = _mission.IFFs[i];
					cbo.Items.AddRange(t);
					break;
				case 6:
					cbo.Items.AddRange(Strings.Orders);
					break;
				case 7:
					cbo.Items.AddRange(Strings.CraftWhen);
					break;
				//case 8: Global Group
				//since it's just numbers, same as default, left out for specifics
				case 9:
					cbo.Items.AddRange(Strings.Misc);
					break;
				default:
					string[] temp = new string[256];
					for (int i=0;i<=255;i++) temp[i] = i.ToString();
					cbo.Items.AddRange(temp);
					break;
			}
		}
		static void comboReset(ComboBox cbo, string[] items, int index)
		{
			cbo.Items.Clear();
			cbo.Items.AddRange(items);
			cbo.SelectedIndex = index;
		}
		void craftStart(FlightGroup fg, bool bAdd)
		{
			if (fg.Difficulty == 1 || fg.Difficulty == 3 || !fg.ArrivesIn30Seconds) return;
			if (bAdd) _startingShips += fg.NumberOfCraft;
			else _startingShips -= fg.NumberOfCraft;
			lblStarting.Text = _startingShips.ToString() + " Craft at 30 seconds";
			if (_startingShips > Mission.CraftLimit) lblStarting.ForeColor = Color.Red;
			else lblStarting.ForeColor = SystemColors.ControlText;
		}
		void initializeMission()
		{
			_mission = new Mission();
			_config.LastMission = "";
			_activeFG = 0;
			_activeMessage = 0;
			_mission.FlightGroups[0].CraftType = Convert.ToByte(_config.TieCraft);
			_mission.FlightGroups[0].IFF = Convert.ToByte(_config.TieIff);
			string[] fgList = _mission.FlightGroups.GetList();
			comboReset(cboArrMS, fgList, 0);
			comboReset(cboArrMSAlt, fgList, 0);
			comboReset(cboDepMS, fgList, 0);
			comboReset(cboDepMSAlt, fgList, 0);
			lstFG.Items.Clear();
			lstFG.Items.Add(_mission.FlightGroups[_activeFG].ToString(true));
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			string[] t = new string[_mission.IFFs.Length];
			for (int i = 0; i < t.Length; i++) t[i] = _mission.IFFs[i];
			comboReset(cboIFF, t, 0);
			this.Text = "Ye Olde Galactic Empire Mission Editor - TIE - New Mission.tie";
		}
		void labelRefresh(Mission.Trigger trigger, Label lbl)
		{	// lbl is the affected label
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
			return text;
		}
		bool loadMission(string fileMission)
		{
			/* return true if successful, returns false if aborted or failed
			 * code is fairly straight-forward. read the crap and save it */
			closeForms();
			try
			{
				FileStream fs = File.OpenRead(fileMission);
				try
				{
					#region determine platform
					switch (Platform.MissionFile.GetPlatform(fs))
					{
						case Platform.MissionFile.Platform.TIE:
							initializeMission();
							break;
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
							_applicationExit = false;
							new XwaForm(_config, fileMission).Show();
							Close();
							return false;
						default:
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
			lstFG.Items.Clear();
			lstMessages.Items.Clear();
			_startingShips = 0;
			for (int i=0;i<_mission.FlightGroups.Count;i++)
			{
				lstFG.Items.Add(_mission.FlightGroups[i].ToString(true));
				if (_mission.FlightGroups[i].ArrivesIn30Seconds) craftStart(_mission.FlightGroups[i], true);
			}
			updateFGList();
			if (_mission.Messages.Count == 0) enableMessage(false);
			else
			{
				enableMessage(true);
				for (int i=0;i<_mission.Messages.Count;i++) lstMessages.Items.Add(_mission.Messages[i].MessageString);
			}
			string[] t = new string[_mission.IFFs.Length];
			for (int i = 0; i < t.Length; i++) t[i] = _mission.IFFs[i];
			comboReset(cboIFF, t, 0);
			updateMissionTabs();
			this.Text = "Ye Olde Galactic Empire Mission Editor - TIE - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
			refreshRecent();
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
		void saveMission(string fileMission)
		{
			try { _fBrief.Save(); }
			catch { /* do nothing */ }
			try { _mission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			this.Text = "Ye Olde Galactic Empire Mission Editor - TIE - " + _mission.MissionFileName;
			_config.LastMission = fileMission;
			//Verify the mission after it's been saved
			if (_config.Verify) Common.RunVerify(_mission.MissionPath, _config.VerifyLocation);
		}
		void startup()
		{
			//initializes cbo's, IFFs, resets bAppExit
			string[] t = new string[_mission.IFFs.Length];
			for (int i = 0; i < t.Length; i++) t[i] = _mission.IFFs[i];
			comboReset(cboIFF, t, 0);
			_config.LastMission = "";
			_config.LastPlatform = Settings.Platform.TIE;
			if (Directory.Exists(_config.TiePath))
			{
				opnTIE.InitialDirectory = _config.TiePath + "\\MISSION";
				savTIE.InitialDirectory = _config.TiePath + "\\MISSION";
			}
			_applicationExit = true;	//becomes false if selecting "New Mission" from menu
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
			cboCraft.Items.AddRange(Strings.CraftType); cboCraft.SelectedIndex = _mission.FlightGroups[0].CraftType;
			cboIFF.SelectedIndex = _mission.FlightGroups[0].IFF;	// already loaded default IFFs at start of function through txtIFF#.Text
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
			for (int i=0;i<3;i++) lblADTrig[i].Tag = i;
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
			for (int i=0;i<3;i++)
			{
				lblOrder[i].Click += new EventHandler(lblOrderArr_Click);
				lblOrder[i].DoubleClick += new EventHandler(lblOrderArr_DoubleClick);
				lblOrder[i].MouseUp += new MouseEventHandler(lblOrderArr_MouseUp);
				lblOrder[i].Tag = i;
			}
			_activeOrder = 0;
			cboOrders.Items.AddRange(Strings.Orders); cboOrders.SelectedIndex = 0;
			cboOT1Type.Items.AddRange(Strings.VariableType); cboOT1Type.SelectedIndex = 0;
			cboOT2Type.Items.AddRange(Strings.VariableType); cboOT2Type.SelectedIndex = 0;
			cboOT3Type.Items.AddRange(Strings.VariableType); cboOT3Type.SelectedIndex = 0;
			cboOT4Type.Items.AddRange(Strings.VariableType); cboOT4Type.SelectedIndex = 0;
			#endregion
			#region Waypoints
			_table.Columns.Add("X"); _table.Columns.Add("Y"); _table.Columns.Add("Z");
			_tableRaw.Columns.Add("X"); _tableRaw.Columns.Add("Y"); _tableRaw.Columns.Add("Z");
			for (int i=0;i<15;i++)	//initialize WPs
			{
				DataRow dr = _table.NewRow();
				int j;
				for(j=0;j<3;j++) dr[j] = 0;	//set X Y Z to zero
				_table.Rows.Add(dr);
				dr = _tableRaw.NewRow();
				for(j=0;j<3;j++) dr[j] = 0;	//mirror in raw table
				_tableRaw.Rows.Add(dr);
			}
			_dataWaypoints.Table = _table;
			_dataWaypointsRaw.Table = _tableRaw;
			dataWP.DataSource = _dataWaypoints;
			dataWP_Raw.DataSource = _dataWaypointsRaw;
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
			chkWP[14] = chkWPBrief;
			for (int i=0;i<15;i++)
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
			for (int i=1;i<4;i++)
			{
				optOfficers[i].Leave += new EventHandler(optOfficers_Leave);
				optOfficers[i].Tag = i;
			}
			#endregion
			#region Unknown
			numUnk[0] = numUnk1;
			numUnk[1] = numUnk5;
			numUnk[2] = numUnk9;
			numUnk[3] = numUnk10;
			numUnk[4] = numUnk11;
			numUnk[5] = numUnk12;
			numUnk[6] = numUnk15;
			numUnk[7] = numUnk16;
			numUnk[8] = numUnk17;
			for (int i=0;i<9;i++)
			{
				numUnk[i].Leave += new EventHandler(numUnkArr_Leave);
				numUnk[i].Tag = i;
			}
			numUnk20.Leave += new EventHandler(numUnkArr_Leave);
			numUnk20.Tag = 10;
			#endregion
			#region FG Goals
			cboGoal[0] = cboPrimGoalT;
			cboGoal[1] = cboPrimGoalA;
			cboGoal[2] = cboSecGoalT;
			cboGoal[3] = cboSecGoalA;
			cboGoal[4] = cboSecretGoalT;
			cboGoal[5] = cboSecretGoalA;
			cboGoal[6] = cboBonGoalT;
			cboGoal[7] = cboBonGoalA;
			for (int i=0;i<4;i++)
			{
				cboGoal[i*2].Items.AddRange(Strings.Trigger); cboGoal[i*2].SelectedIndex = 10;
				cboGoal[i*2+1].Items.AddRange(Strings.GoalAmount); cboGoal[i*2+1].SelectedIndex = 0;
			}
			for (int i=0;i<8;i++)
			{
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
			for (int i=0;i<6;i++)
			{
				lblGlob[i].Click += new EventHandler(lblGlobArr_Click);
				lblGlob[i].DoubleClick += new EventHandler(lblGlobArr_DoubleClick);
				lblGlob[i].MouseUp += new MouseEventHandler(lblGlobArr_MouseUp);
				lblGlob[i].Tag = i;
			}
			_activeGlobalGoal = 0;
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
			txtEoM[0] = txtPrimComp1;
			txtEoM[1] = txtPrimComp2;
			txtEoM[2] = txtSecComp1;
			txtEoM[3] = txtSecComp2;
			txtEoM[4] = txtPrimFail1;
			txtEoM[5] = txtPrimFail2;
			for (int i=0;i<6;i++)
			{
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
			for (int i=0;i<4;i++)
			{
				chkIFF[i].Leave += new EventHandler(chkIFFArr_Leave);
				chkIFF[i].Tag = i+2;
				txtIFF[i].Leave += new EventHandler(txtIFFArr_Leave);
				txtIFF[i].Tag = i+2;
			}
			#endregion
			updateMissionTabs();
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
			for (int i=0;i<6;i++) labelRefresh(_mission.GlobalGoals.Goals[i/2].Triggers[i%2], lblGlob[i]);
			lblGlobArr_Click(0, new System.EventArgs());
			#endregion
			#region Mission tab
			optCapture.Checked = _mission.CapturedOnEjection;
			optRescue.Checked = !optCapture.Checked;
			for (int i=0;i<6;i++) txtEoM[i].Text = _mission.EndOfMissionMessages[i];
			for (int i=0;i<4;i++)
			{
				txtIFF[i].Text = _mission.IFFs[i+2];
				chkIFF[i].Checked = _mission.IffHostile[i+2];
			}
			#endregion
			#region Questions tab
			if (_mission.OfficersPresent == Mission.BriefingOfficers.Both) optBoth.Checked = true;
			else if (_mission.OfficersPresent == Mission.BriefingOfficers.FlightOfficer) optFO.Checked = true;
			else optSO.Checked = true;
			txtQuestion.Text = _mission.BriefingQuestions.PreMissQuestions[0];
			txtAnswer.Text = _mission.BriefingQuestions.PreMissAnswers[0];
			#endregion
		}

		void frmTIE_Activated(object sender, EventArgs e)
		{
			if (_fMap != null)
			{
				lstFG.SelectedIndex = -1;
				lstFG.SelectedIndex = _activeFG;
			}
		}
		void frmTIE_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

		void opnTIE_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_loading = true;
			if (loadMission(opnTIE.FileName))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				_activeFG = 0;
				lstFG.SelectedIndex = 0;
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_loading = false;
		}

		void savTIE_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			saveMission(savTIE.FileName);
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
				case 0:		//New Mission
					menuNewTIE_Click("toolbar", new System.EventArgs());
					_loading = false;
					break;
				case 1:		//Open Mission
					menuOpen_Click("toolbar", new System.EventArgs());
					_loading = false;
					break;
				case 2:		//Save Mission
					menuSave_Click("toolbar", new System.EventArgs());
					break;
				case 3:		//Save As
					savTIE.ShowDialog();
					break;
				case 5:		//New Item
					if (tabMain.SelectedIndex == 0) newFG();
					else if (tabMain.SelectedIndex == 1) newMess();
					break;
				case 6:		//Delete Item
					if (tabMain.SelectedIndex == 0) deleteFG();
					else if (tabMain.SelectedIndex == 1) deleteMess();
					break;
				case 7:		//Copy Item
					menuCopy_Click("toolbar", new EventArgs());
					break;
				case 8:		//Paste Item
					menuPaste_Click("toolbar", new EventArgs());
					break;
				case 10:	//Map
					menuMap_Click("toolbat", new EventArgs());
					break;
				case 11:	//Briefing
					menuBriefing_Click("toolbar", new EventArgs());
					break;
				case 12:	//Verify
					menuVerify_Click("toolbar", new EventArgs());
					break;
				case 14:	//Options
					menuOptions_Click("toolbar", new EventArgs());
					break;
				case 15:	//Battle
					menuBattle_Click("toolbar", new EventArgs());
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
		void menuBattle_Click(object sender, EventArgs e)
		{
			_fBattle = new BattleForm(_config);
			_fBattle.Show();
		}
		void menuBriefing_Click(object sender, EventArgs e)
		{
			Common.Title(this, _loading);
			_fBrief = new BriefingForm(_mission.FlightGroups, _mission.Briefing);
			_fBrief.Show();
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
			#region Orders
			if (sender.ToString() == "Order")
			{
				formatter.Serialize(stream, _mission.FlightGroups[_activeFG].Orders[_activeOrder]);
				stream.Close();
				return;
			}
			#endregion
			#region generic TextBox
			if (this.ActiveControl.GetType().ToString() == "System.Windows.Forms.TextBox")
			{
				System.Windows.Forms.TextBox txt_t = (System.Windows.Forms.TextBox)ActiveControl;
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
				if (lblMess1.ForeColor == SystemColors.Highlight) formatter.Serialize(stream, _mission.Messages[_activeMessage].Triggers[0]);
				else formatter.Serialize(stream, _mission.Messages[_activeMessage].Triggers[1]);
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
					formatter.Serialize(stream, _mission.GlobalGoals.Goals[_activeGlobalGoal/2].Triggers[_activeGlobalGoal%2]);
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
			if (this.Text.EndsWith("*")) this.Text = this.Text.Substring(0, this.Text.Length-1);
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
			opnTIE.FileName = _mission.MissionFileName;
			opnTIE.ShowDialog();
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
					_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger] = (Mission.Trigger)formatter.Deserialize(stream);
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
			if (sender.ToString()== "Order")
			{
				try
				{
					_mission.FlightGroups[_activeFG].Orders[_activeOrder] = (FlightGroup.Order)formatter.Deserialize(stream);
					lblOrderArr_Click(_activeOrder, new EventArgs());
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
					//FG.length = 17, Mess.L = 14, Orders/Goals/Trigger = 13, str = str.L
					string str_t = formatter.Deserialize(stream).ToString();
					if (str_t.Length == 13 || str_t.Length == 14 || str_t.Length == 17)
					{
						FlightGroup fg_t = (FlightGroup)formatter.Deserialize(stream);
						if (fg_t != null) throw new Exception();
						Platform.Tie.Message mess_t = (Platform.Tie.Message)formatter.Deserialize(stream);
						if (mess_t != null) throw new Exception();
						byte[] b_t = (byte[])formatter.Deserialize(stream);
						if (b_t.Length == 4 || b_t.Length == 18) throw new Exception();
					}
					System.Windows.Forms.TextBox txt_t = (System.Windows.Forms.TextBox)ActiveControl;
					txt_t.SelectedText = str_t;
					Common.Title(this, false);
					stream.Close();
					return;
				}
			}
			catch { /* do nothing */ }
			#endregion
			#region MessTrig
			if (sender.ToString()== "MessTrig")
			{
				try
				{
					//byte[] b_temp = (byte[])formatter.Deserialize(stream);
					//if (b_temp.Length != 4) throw new Exception();
					if (lblMess1.ForeColor == SystemColors.Highlight)
					{
						_mission.Messages[_activeMessage].Triggers[0] = (Mission.Trigger)formatter.Deserialize(stream);
						//for (int i=0;i<4;i++) TieMission.Messages[activeMessage].Triggers[0][i] = b_temp[i];
						lblMessArr_Click(0, new EventArgs());
					}
					else
					{
						_mission.Messages[_activeMessage].Triggers[1] = (Mission.Trigger)formatter.Deserialize(stream);
						//for (int i=0;i<4;i++) TieMission.Messages[activeMessage].Triggers[1][i] = b_temp[i];
						lblMessArr_Click(1, new EventArgs());
					}
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
						craftStart(fg_temp, true);
					}
					catch { /* do nothing */ }
					break;
				case 1:
					try
					{
						Platform.Tie.Message mess_temp = (Platform.Tie.Message)formatter.Deserialize(stream);
						if (mess_temp == null) throw new Exception();
						newMess();
						_mission.Messages[_activeMessage] = mess_temp;
						messlistRefresh();
						lstMessages.SelectedIndex = _activeMessage;
					}
					catch { /* do nothing */ }
					break;
				case 2:
					try
					{
						_mission.GlobalGoals.Goals[_activeGlobalGoal/2].Triggers[_activeGlobalGoal%3] = (Mission.Trigger)formatter.Deserialize(stream);
						lblGlobArr_Click(_activeGlobalGoal, new EventArgs());
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
				if (_mission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_loading = false;
		}
		void menuSave_Click(object sender, EventArgs e)
		{
			if (_mission.MissionPath == "\\NewMission.tie") savTIE.ShowDialog();
			else saveMission(_mission.MissionPath);
		}
		void menuSaveAsBoP_Click(object sender, EventArgs e)
		{
			menuSaveAsXvT_Click("SaveAsBoP", new EventArgs());
		}
		void menuSaveAsTIE_Click(object sender, EventArgs e)
		{
			savTIE.ShowDialog();
		}
		void menuSaveAsXvT_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsXvT", new System.EventArgs());
			Common.RunConverter(_mission.MissionPath, 0);
		}
		void menuSaveAsXWA_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsXWA", new System.EventArgs());
			Common.RunConverter(_mission.MissionPath, 1);
		}
		void menuVerify_Click(object sender, EventArgs e)
		{
			menuSave_Click("Verify", new EventArgs());
			Common.RunVerify(_mission.MissionPath, _config.VerifyLocation);
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
			if (_config.VerifyTest && !_config.Verify) Common.RunVerify(_mission.MissionPath, _config.VerifyLocation);
			Version os = Environment.OSVersion.Version;
			bool isWin7 = (os.Major == 6 && os.Minor == 1);
			System.Diagnostics.Process explorer = null;
			int restart = 1;
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);

			// configure TIE
			int index = 0;
			while (File.Exists(_config.TiePath + "\\TEST" + index + ".tfr")) index++;
			string pilot = "\\TEST" + index + ".tfr";
			string battle = "\\RESOURCE\\BATTLE1.LFD";
			string backup = "\\RESOURCE\\BATTLE1_" + index + ".bak";
			File.Copy(Application.StartupPath + "\\TEST.tfr", _config.TiePath + pilot);
			System.Diagnostics.Process tie = new System.Diagnostics.Process();
			tie.StartInfo.FileName = _config.TiePath + "\\TIE95.exe";
			tie.StartInfo.UseShellExecute = false;
			tie.StartInfo.WorkingDirectory = _config.TiePath;
			File.Copy(_config.TiePath + battle, _config.TiePath + backup, true);
			Idmr.LfdReader.LfdFile battleLfd = new Idmr.LfdReader.LfdFile(_config.TiePath + battle);
			Idmr.LfdReader.Text txt = (Idmr.LfdReader.Text)battleLfd.Resources[0];
			string[] missions = txt.Strings[3].Split('\0');
			missions[0] = _mission.MissionFileName.Replace(".tie", "");
			txt.Strings[3] = String.Join("\0", missions);
			battleLfd.Write();

			if (isWin7)	// explorer kill so colors work right
			{
				restart = (int)key.GetValue("AutoRestartShell", 1);
				key.SetValue("AutoRestartShell", 0, Microsoft.Win32.RegistryValueKind.DWord);
				explorer = System.Diagnostics.Process.GetProcessesByName("explorer")[0];
				explorer.Kill();
				explorer.WaitForExit();
			}

			tie.Start();
			tie.WaitForExit();

			if (isWin7)	// restart
			{
				key.SetValue("AutoRestartShell", restart, Microsoft.Win32.RegistryValueKind.DWord);
				explorer.StartInfo.UseShellExecute = false;
				explorer.StartInfo.FileName = "explorer.exe";
				explorer.Start();
			}
			if (_config.DeleteTestPilots) File.Delete(_config.TiePath + pilot);
			File.Copy(_config.TiePath + backup, _config.TiePath + battle, true);
			File.Delete(_config.TiePath + backup);
		}
		#endregion
		#region FlightGroups
		void deleteFG()
		{
			if (_mission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(_activeFG);
			craftStart(_mission.FlightGroups[_activeFG], false);
			if (_mission.FlightGroups.Count == 1)
			{
				_mission.FlightGroups.Clear();
				_activeFG = 0;
				_mission.FlightGroups[0].CraftType = _config.TieCraft;
				_mission.FlightGroups[0].IFF = _config.TieIff;
				craftStart(_mission.FlightGroups[0], true);
			}
			else _activeFG = _mission.FlightGroups.RemoveAt(_activeFG);
			updateFGList();
			lstFG.SelectedIndex = _activeFG;
			Common.Title(this, _loading);
			try
			{
				_fMap.Import(_mission.FlightGroups);
				_fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
			try
			{
				_fBrief.Import(_mission.FlightGroups);
				_fBrief.MapPaint();
			}
			catch { /* do nothing */ }
		}
		void listRefresh()
		{
			lstFG.Items[_activeFG] = _mission.FlightGroups[_activeFG].ToString(true);
		}
		void newFG()
		{
			if (_mission.FlightGroups.Count == Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			_activeFG = _mission.FlightGroups.Add();
			_mission.FlightGroups[_activeFG].CraftType = _config.TieCraft;
			_mission.FlightGroups[_activeFG].IFF = _config.TieIff;
			craftStart(_mission.FlightGroups[_activeFG], true);
			lstFG.Items.Add(_mission.FlightGroups[_activeFG].ToString(true));
			updateFGList();
			lstFG.SelectedIndex = _activeFG;
			Common.Title(this, _loading);
			try
			{
				_fMap.Import(_mission.FlightGroups);
				_fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
			try
			{
				_fBrief.Import(_mission.FlightGroups);
				_fBrief.MapPaint();
			}
			catch { /* do nothing */ }
		}
		void updateFGList()
		{
			string[] fgList = _mission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			comboReset(cboArrMS, fgList, _mission.FlightGroups[_activeFG].ArrivalCraft1);
			comboReset(cboArrMSAlt, fgList, _mission.FlightGroups[_activeFG].ArrivalCraft2);
			comboReset(cboDepMS, fgList, _mission.FlightGroups[_activeFG].DepartureCraft1);
			comboReset(cboDepMSAlt, fgList, _mission.FlightGroups[_activeFG].DepartureCraft2);
			_loading = temp;
			listRefresh();
		}

		void lstFG_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1 || _mission.FlightGroups[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch(_mission.FlightGroups[e.Index].IFF)
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
					brText = Brushes.DarkOrchid;
					break;
				case 4:
					brText = Brushes.Red;
					break;
				case 5:
					brText = Brushes.Fuchsia;
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
			txtPilot.Text = _mission.FlightGroups[_activeFG].Pilot;
			txtCargo.Text = _mission.FlightGroups[_activeFG].Cargo;
			txtSpecCargo.Text = _mission.FlightGroups[_activeFG].SpecialCargo;
			numSC.Value = _mission.FlightGroups[_activeFG].SpecialCargoCraft;
			chkRandSC.Checked = _mission.FlightGroups[_activeFG].RandSpecCargo;
			numCraft.Value = _mission.FlightGroups[_activeFG].NumberOfCraft;
			numWaves.Value = _mission.FlightGroups[_activeFG].NumberOfWaves;
			numGlobal.Value = _mission.FlightGroups[_activeFG].GlobalGroup;
			cboCraft.SelectedIndex = _mission.FlightGroups[_activeFG].CraftType;
			cboIFF.SelectedIndex = _mission.FlightGroups[_activeFG].IFF;
			cboAI.SelectedIndex = _mission.FlightGroups[_activeFG].AI;
			cboMarkings.SelectedIndex = _mission.FlightGroups[_activeFG].Markings;
			cboPlayer.SelectedIndex = _mission.FlightGroups[_activeFG].PlayerCraft;
			cboFormation.SelectedIndex = _mission.FlightGroups[_activeFG].Formation;
			chkRadio.Checked = Convert.ToBoolean(_mission.FlightGroups[_activeFG].FollowsOrders);
			numLead.Value = _mission.FlightGroups[_activeFG].FormLeaderDist;
			numSpacing.Value = _mission.FlightGroups[_activeFG].FormDistance;
			cboStatus.SelectedIndex = _mission.FlightGroups[_activeFG].Status1;
			cboWarheads.SelectedIndex = _mission.FlightGroups[_activeFG].Missile;
			cboBeam.SelectedIndex = _mission.FlightGroups[_activeFG].Beam;
			#endregion
			#region Arr/Dep
			optArrMS.Checked = Convert.ToBoolean(_mission.FlightGroups[_activeFG].ArrivalMethod1);
			optArrHyp.Checked = !optArrMS.Checked;
			optArrMSAlt.Checked = Convert.ToBoolean(_mission.FlightGroups[_activeFG].ArrivalMethod2);
			optArrHypAlt.Checked = !optArrMSAlt.Checked;
			optDepMS.Checked = Convert.ToBoolean(_mission.FlightGroups[_activeFG].DepartureMethod1);
			optDepHyp.Checked = !optDepMS.Checked;
			optDepMSAlt.Checked = Convert.ToBoolean(_mission.FlightGroups[_activeFG].DepartureMethod2);
			optDepHypAlt.Checked = !optDepMSAlt.Checked;
			try { cboArrMS.SelectedIndex = _mission.FlightGroups[_activeFG].ArrivalCraft1; }
			catch { cboArrMS.SelectedIndex = 0; _mission.FlightGroups[_activeFG].ArrivalCraft1 = 0; optArrHyp.Checked = true; }
			try { cboArrMSAlt.SelectedIndex = _mission.FlightGroups[_activeFG].ArrivalCraft2; }
			catch { cboArrMSAlt.SelectedIndex = 0; _mission.FlightGroups[_activeFG].ArrivalCraft2 = 0; optArrHypAlt.Checked = true; }
			try { cboDepMS.SelectedIndex = _mission.FlightGroups[_activeFG].DepartureCraft1; }
			catch { cboDepMS.SelectedIndex = 0; _mission.FlightGroups[_activeFG].DepartureCraft1 = 0; optDepHyp.Checked = true; }
			try { cboDepMSAlt.SelectedIndex = _mission.FlightGroups[_activeFG].DepartureCraft2; }
			catch { cboDepMSAlt.SelectedIndex = 0; _mission.FlightGroups[_activeFG].DepartureCraft2 = 0; optDepHypAlt.Checked = true; }
			optArrOR.Checked = _mission.FlightGroups[_activeFG].AT1AndOrAT2;
			optArrAND.Checked = !optArrOR.Checked;
			for (int i = 0; i < 3; i++) labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[i], lblADTrig[i]);
			numArrMin.Value = _mission.FlightGroups[_activeFG].ArrivalDelayMinutes;
			numArrSec.Value = _mission.FlightGroups[_activeFG].ArrivalDelaySeconds;
			numDepMin.Value = _mission.FlightGroups[_activeFG].DepartureTimerMinutes;
			numDepSec.Value = _mission.FlightGroups[_activeFG].DepartureTimerSeconds;
			cboAbort.SelectedIndex = _mission.FlightGroups[_activeFG].AbortTrigger;
			cboDiff.SelectedIndex = _mission.FlightGroups[_activeFG].Difficulty;
			lblADTrigArr_Click(0, new EventArgs());
			#endregion
			for (int i=0;i<8;i++) cboGoal[i].SelectedIndex = _mission.FlightGroups[_activeFG].Goals[i];
			numBonGoalP.Value = _mission.FlightGroups[_activeFG].Goals.BonusPoints;
			#region Waypoints
			for (int i=0;i<15;i++)
			{
				for (int j=0;j<3;j++)
				{
					_tableRaw.Rows[i][j] = _mission.FlightGroups[_activeFG].Waypoints[i][j];
					_table.Rows[i][j] = Math.Round((double)_mission.FlightGroups[_activeFG].Waypoints[i][j] / 160, 2);
				}
				chkWP[i].Checked = _mission.FlightGroups[_activeFG].Waypoints[i].Enabled;
			}
			_table.AcceptChanges();
			_tableRaw.AcceptChanges();
			numYaw.Value = _mission.FlightGroups[_activeFG].Yaw;
			numPitch.Value = _mission.FlightGroups[_activeFG].Pitch;
			numRoll.Value = _mission.FlightGroups[_activeFG].Roll;
			if (_mission.FlightGroups[_activeFG].CraftType <= 0x45) enableRot(false);
			else enableRot(true);
			#endregion
			for (_activeOrder=0;_activeOrder<3;_activeOrder++) orderLabelRefresh();
			lblOrderArr_Click(0, new EventArgs());
			for (int i=0;i<9;i++) numUnk[i].Value = _mission.FlightGroups[_activeFG].Unknowns[i];
			chkUnk19.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown19;
			numUnk20.Value = _mission.FlightGroups[_activeFG].Unknowns.Unknown20;
			chkUnk21.Checked = _mission.FlightGroups[_activeFG].Unknowns.Unknown21;
			_loading = btemp;
			enableBackdrop((_mission.FlightGroups[_activeFG].CraftType == 0x57 ? true : false));
		}

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
			numLead.Enabled = !state;
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
		}

		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			enableBackdrop((cboCraft.SelectedIndex == 0x57 ? true : false));
			if (_loading) return;
			_mission.FlightGroups[_activeFG].CraftType = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].CraftType, cboCraft.SelectedIndex));
			enableRot((_mission.FlightGroups[_activeFG].CraftType <= 0x45 ? false : true));
			updateFGList();
		}
		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].Formation = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Formation, cboFormation.SelectedIndex));
		}
		void cboStatus_Leave(object sender, EventArgs e)
		{
			numBackdrop.Value = cboStatus.SelectedIndex;
			_mission.FlightGroups[_activeFG].Status1 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Status1, cboStatus.SelectedIndex));
		}

		void chkRandSC_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].RandSpecCargo = (bool)Common.Update(this, _mission.FlightGroups[_activeFG].RandSpecCargo, chkRandSC.Checked);
			if (chkRandSC.Checked || numSC.Value != 0)
			{
				numSC.Value = 0;
				lblNotUsed.Visible = false;
				txtSpecCargo.Visible = true;
			}
			else
			{
				lblNotUsed.Visible = true;
				txtSpecCargo.Visible = false;
			}
		}

		void cmdBackdrop_Click(object sender, EventArgs e)
		{
			try
			{
				BackdropDialog dlg = new BackdropDialog(Platform.MissionFile.Platform.TIE, _mission.FlightGroups[_activeFG].Status1);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					numBackdrop.Value = dlg.BackdropIndex;	// simply GUI
					cboStatus.SelectedIndex = (int)numBackdrop.Value;	// drives stored value
				}
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void cmdForms_Click(object sender, EventArgs e)
		{
			FormationDialog dlg = new FormationDialog(_mission.FlightGroups[_activeFG].Formation);
			dlg.SetToTie95();
			if (dlg.ShowDialog() == DialogResult.OK) cboFormation.SelectedIndex = dlg.Formation;
		}

		void grpCraft2_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].IFF = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].IFF, cboIFF.SelectedIndex));
			_mission.FlightGroups[_activeFG].AI = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].AI, cboAI.SelectedIndex));
			_mission.FlightGroups[_activeFG].Markings = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Markings, cboMarkings.SelectedIndex));
			_mission.FlightGroups[_activeFG].PlayerCraft = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].PlayerCraft, cboPlayer.SelectedIndex));
			_mission.FlightGroups[_activeFG].FollowsOrders = (bool)Common.Update(this, _mission.FlightGroups[_activeFG].FollowsOrders, chkRadio.Checked);
			_mission.FlightGroups[_activeFG].FormDistance = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].FormDistance, numSpacing.Value));
			_mission.FlightGroups[_activeFG].FormLeaderDist = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].FormLeaderDist, numLead.Value));
			listRefresh();
		}
		void grpCraft3_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].NumberOfWaves = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].NumberOfWaves, numWaves.Value));
			craftStart(_mission.FlightGroups[_activeFG], false);
			_mission.FlightGroups[_activeFG].NumberOfCraft = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].NumberOfCraft, numCraft.Value));
			craftStart(_mission.FlightGroups[_activeFG], true);
			if (_mission.FlightGroups[_activeFG].SpecialCargoCraft > _mission.FlightGroups[_activeFG].NumberOfCraft) numSC.Value = 0;
			_mission.FlightGroups[_activeFG].GlobalGroup = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].GlobalGroup, numGlobal.Value));
			listRefresh();
		}
		void grpCraft4_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Missile = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Missile, cboWarheads.SelectedIndex));
			_mission.FlightGroups[_activeFG].Beam = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Beam, cboBeam.SelectedIndex));
		}

		void numBackdrop_Leave(object sender, EventArgs e)
		{
			cboStatus.SelectedIndex = (int)numBackdrop.Value;
			_mission.FlightGroups[_activeFG].Status1 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Status1, cboStatus.SelectedIndex));
		}
		void numSC_ValueChanged(object sender, EventArgs e)
		{
			if (_mission.FlightGroups[_activeFG].RandSpecCargo) { numSC.Value = 0; return; }
			if (numSC.Value == 0 || numSC.Value > _mission.FlightGroups[_activeFG].NumberOfCraft)
			{
				if (!_loading)
					_mission.FlightGroups[_activeFG].SpecialCargoCraft = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].SpecialCargoCraft, 0));
				txtSpecCargo.Visible = false;
				lblNotUsed.Visible = true;
			}
			else
			{
				if (!_loading)
					_mission.FlightGroups[_activeFG].SpecialCargoCraft = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].SpecialCargoCraft, numSC.Value));
				txtSpecCargo.Visible = true;
				lblNotUsed.Visible = false;
			}
		}

		void txtCargo_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Cargo = Common.Update(this, _mission.FlightGroups[_activeFG].Cargo, txtCargo.Text).ToString();
		}
		void txtName_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Name = Common.Update(this, _mission.FlightGroups[_activeFG].Name, txtName.Text).ToString();
			updateFGList();
		}
		void txtPilot_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Pilot = Common.Update(this, _mission.FlightGroups[_activeFG].Pilot, txtPilot.Text).ToString();
		}
		void txtSpecCargo_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].SpecialCargo = Common.Update(this, _mission.FlightGroups[_activeFG].SpecialCargo, txtSpecCargo.Text).ToString();
		}
		#endregion
		#region ArrDep
		void lblADTrigArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeArrDepTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeArrDepTrigger = Convert.ToByte(sender); l = lblADTrig[_activeArrDepTrigger]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<3;i++) if (i != _activeArrDepTrigger) lblADTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboADTrig.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Condition;
			cboADTrigType.SelectedIndex = -1;
			cboADTrigType.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType;
			cboADTrigAmount.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Amount;
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

		void cboADTrig_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Condition = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Condition, cboADTrig.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboADTrigAmount_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Amount = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Amount, cboADTrigAmount.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboADTrigType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboADTrigType.SelectedIndex == -1) return;
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType, cboADTrigType.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].VariableType, cboADTrigVar);
			try { cboADTrigVar.SelectedIndex = _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable; }
			catch { cboADTrigVar.SelectedIndex = 0; _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable = 0; }
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboADTrigVar_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger].Variable, cboADTrigVar.SelectedIndex));
			labelRefresh(_mission.FlightGroups[_activeFG].ArrDepTriggers[_activeArrDepTrigger], lblADTrig[_activeArrDepTrigger]);
		}
		void cboDiff_Leave(object sender, EventArgs e)
		{
			craftStart(_mission.FlightGroups[_activeFG], false);
			_mission.FlightGroups[_activeFG].Difficulty = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Difficulty, cboDiff.SelectedIndex));
			craftStart(_mission.FlightGroups[_activeFG], true);
		}

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
			_mission.FlightGroups[_activeFG].ArrivalDelayMinutes = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalDelayMinutes, numArrMin.Value));
			_mission.FlightGroups[_activeFG].ArrivalDelaySeconds = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalDelaySeconds, numArrSec.Value));
			craftStart(_mission.FlightGroups[_activeFG], true);
			_mission.FlightGroups[_activeFG].ArrivalCraft1 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalCraft1, cboArrMS.SelectedIndex));
			_mission.FlightGroups[_activeFG].ArrivalCraft2 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalCraft2, cboArrMSAlt.SelectedIndex));
		}
		void grpDep_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].DepartureCraft1 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].DepartureCraft1, cboDepMS.SelectedIndex));
			_mission.FlightGroups[_activeFG].DepartureCraft2 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].DepartureCraft2, cboDepMSAlt.SelectedIndex));
			_mission.FlightGroups[_activeFG].AbortTrigger = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].AbortTrigger, cboAbort.SelectedIndex));
			_mission.FlightGroups[_activeFG].DepartureTimerMinutes = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].DepartureTimerMinutes, numDepMin.Value));
			_mission.FlightGroups[_activeFG].DepartureTimerSeconds = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].DepartureTimerSeconds, numDepSec.Value));
		}

		void optArrMS_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrivalMethod1 = (bool)Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalMethod1, optArrMS.Checked);
			cboArrMS.Enabled = optArrMS.Checked;
		}
		void optArrMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].ArrivalMethod2 = (bool)Common.Update(this, _mission.FlightGroups[_activeFG].ArrivalMethod2, optArrMSAlt.Checked);
			cboArrMSAlt.Enabled = optArrMSAlt.Checked;
		}
		void optDepMS_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].DepartureMethod1 = (bool)Common.Update(this, _mission.FlightGroups[_activeFG].DepartureMethod1, optDepMS.Checked);
			cboDepMS.Enabled = optDepMS.Checked;
		}
		void optDepMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
			_mission.FlightGroups[_activeFG].DepartureMethod2 = (bool)Common.Update(this, _mission.FlightGroups[_activeFG].DepartureMethod2, optDepMSAlt.Checked);
			cboDepMSAlt.Enabled = optDepMSAlt.Checked;
		}
		void optArrOR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].AT1AndOrAT2 = (bool)Common.Update(this, _mission.FlightGroups[_activeFG].AT1AndOrAT2, optArrOR.Checked);
		}
		#endregion
		#region Orders
		void orderLabelRefresh()
		{
			string orderText = _mission.FlightGroups[_activeFG].Orders[_activeOrder].ToString();
			orderText = replaceTargetText(orderText);
			lblOrder[_activeOrder].Text = "Order " + (_activeOrder + 1) + ": " + orderText;
		}

		void lblOrderArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeOrder = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeOrder = Convert.ToByte(sender); l = lblOrder[_activeOrder]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<3;i++) if (i!=_activeOrder) lblOrder[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboOrders.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Command;
			cboOThrottle.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Throttle;
			numOVar1.Value = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable1;
			numOVar2.Value = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable2;
			cboOT3Type.SelectedIndex = -1;
			cboOT3Type.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3Type;
			cboOT4Type.SelectedIndex = -1;
			cboOT4Type.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4Type;
			optOT3T4OR.Checked = _mission.FlightGroups[_activeFG].Orders[_activeOrder].T3AndOrT4;
			optOT3T4AND.Checked = !optOT3T4OR.Checked;
			cboOT1Type.SelectedIndex = -1;
			cboOT1Type.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1Type;
			cboOT2Type.SelectedIndex = -1;
			cboOT2Type.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2Type;
			optOT1T2OR.Checked = _mission.FlightGroups[_activeFG].Orders[_activeOrder].T1AndOrT2;
			optOT1T2AND.Checked = !optOT1T2OR.Checked;
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
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Command = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Command, cboOrders.SelectedIndex));
			orderLabelRefresh();
			int i = Strings.OrderDesc[cboOrders.SelectedIndex].IndexOf("|");
			int j = Strings.OrderDesc[cboOrders.SelectedIndex].LastIndexOf("|");
			lblODesc.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(0, i);
			lblOVar1.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(i+1, j-i-1);
			lblOVar2.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(j+1);
		}

		void cboOT1_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1, cboOT1.SelectedIndex));
		}
		void cboOT1Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT1Type.SelectedIndex == -1) return;
			if (!_loading) 
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1Type = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1Type, cboOT1Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1Type, cboOT1);
			try { cboOT1.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1; }
			catch { cboOT1.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target1 = 0; }
			orderLabelRefresh();
		}
		void cboOT2_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2, cboOT2.SelectedIndex));
		}
		void cboOT2Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT2Type.SelectedIndex == -1) return;
			if (!_loading) 
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2Type = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2Type, cboOT2Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2Type, cboOT2);
			try { cboOT2.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2; }
			catch { cboOT2.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target2 = 0; }
			orderLabelRefresh();
		}
		void cboOT3_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3, cboOT3.SelectedIndex));
		}
		void cboOT3Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT3Type.SelectedIndex == -1) return;
			if (!_loading) 
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3Type = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3Type, cboOT3Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3Type, cboOT3);
			try { cboOT3.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3; }
			catch { cboOT3.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target3 = 0; }
			orderLabelRefresh();
		}
		void cboOT4_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4, cboOT4.SelectedIndex));
		}
		void cboOT4Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT4Type.SelectedIndex == -1) return;
			if (!_loading) 
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4Type = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4Type, cboOT4Type.SelectedIndex));
			comboVarRefresh(_mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4Type, cboOT4);
			try { cboOT4.SelectedIndex = _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4; }
			catch { cboOT4.SelectedIndex = 0; _mission.FlightGroups[_activeFG].Orders[_activeOrder].Target4 = 0; }
			orderLabelRefresh();
		}
		void cboOThrottle_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Throttle = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Throttle, cboOThrottle.SelectedIndex));
		}

		void cmdCopyOrder_Click(object sender, EventArgs e)
		{
			menuCopy_Click("Order", new System.EventArgs());
		}
		void cmdPasteOrder_Click(object sender, EventArgs e)
		{
			menuPaste_Click("Order", new System.EventArgs());
		}

		void numOVar1_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable1 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable1, numOVar1.Value));
		}
		void numOVar2_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable2 = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].Variable2, numOVar2.Value));
		}

		void optOT1T2OR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].T1AndOrT2 = Convert.ToBoolean(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].T1AndOrT2, optOT1T2OR.Checked));
		}
		void optOT3T4OR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.FlightGroups[_activeFG].Orders[_activeOrder].T3AndOrT4 = Convert.ToBoolean(Common.Update(this, _mission.FlightGroups[_activeFG].Orders[_activeOrder].T3AndOrT4, optOT3T4OR.Checked));
		}
		#endregion
		#region Goals
		void cboGoalArr_Leave(object sender, EventArgs e)
		{
			ComboBox c = (ComboBox)sender;
			_mission.FlightGroups[_activeFG].Goals[(int)c.Tag] = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Goals[(int)c.Tag], cboGoal[(int)c.Tag].SelectedIndex));
		}

		void numBonGoalP_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Goals.BonusPoints = (short)Common.Update(this, _mission.FlightGroups[_activeFG].Goals.BonusPoints, numBonGoalP.Value);
		}
		#endregion
		#region Waypoints
		void enableRot(bool state)
		{
			numYaw.Enabled = state;
			numPitch.Enabled = state;
			numRoll.Enabled = state;
		}

		void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			_mission.FlightGroups[_activeFG].Waypoints[(int)c.Tag].Enabled = (bool)Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[(int)c.Tag].Enabled, c.Checked);
		}

		void numPitch_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Pitch = (short)Common.Update(this, _mission.FlightGroups[_activeFG].Pitch, numPitch.Value);
		}
		void numRoll_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Roll = (short)Common.Update(this, _mission.FlightGroups[_activeFG].Roll, numRoll.Value);
		}
		void numYaw_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Yaw = (short)Common.Update(this, _mission.FlightGroups[_activeFG].Yaw, numYaw.Value);
		}

		void table_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i,j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<15;j++) if (_table.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = (short)(Convert.ToDouble(_table.Rows[j][i]) * 160);
					_mission.FlightGroups[_activeFG].Waypoints[j][i] = (short)Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[j][i], raw);
					_tableRaw.Rows[j][i] = raw;
				}
			}
			catch { for (i=0;i<3;i++) _table.Rows[j][i] = Math.Round((double)_mission.FlightGroups[_activeFG].Waypoints[j][i] / 160, 2); }
			_loading = false;
		}
		void tableRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i,j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<15;j++) if (_tableRaw.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++) 
				{
					short raw = (short)_tableRaw.Rows[j][i];
					_mission.FlightGroups[_activeFG].Waypoints[j][i] = (short)Common.Update(this, _mission.FlightGroups[_activeFG].Waypoints[j][i], raw);
					_table.Rows[j][i] = Math.Round((double)raw / 160,2);
				}
			}
			catch { for (i=0;i<3;i++) _tableRaw.Rows[j][i] = Convert.ToInt16(_mission.FlightGroups[_activeFG].Waypoints[j][i]); }
			_loading = false;
		}
		#endregion
		#region Unknowns
		void chkUnk19_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Unknowns.Unknown19 = Convert.ToBoolean(Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown19, chkUnk19.Checked));
		}
		void chkUnk21_Leave(object sender, EventArgs e)
		{
			_mission.FlightGroups[_activeFG].Unknowns.Unknown21 = Convert.ToBoolean(Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns.Unknown21, chkUnk21.Checked));
		}
		void numUnkArr_Leave(object sender, EventArgs e)
		{
			NumericUpDown n = (NumericUpDown)sender;
			_mission.FlightGroups[_activeFG].Unknowns[(int)n.Tag] = Convert.ToByte(Common.Update(this, _mission.FlightGroups[_activeFG].Unknowns[(int)n.Tag], n.Value));
		}
		#endregion
		#endregion
		#region Messages
		void deleteMess()
		{
			_activeMessage = _mission.Messages.RemoveAt(_activeMessage);
			if (_mission.Messages.Count == 0)
			{
				lstMessages.Items.Clear();
				enableMessage(false);
				lblMessage.Text = "Message #0 of 0";
				return;
			}
			lstMessages.Items.RemoveAt(_activeMessage);
			lstMessages.SelectedIndex = _activeMessage;
			Common.Title(this, _loading);
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
		void messlistRefresh()
		{
			if (_mission.Messages.Count == 0) return;
			string temp = _mission.Messages[_activeMessage].MessageString;
			lstMessages.Items.Insert(_activeMessage, temp);
			lstMessages.Items.RemoveAt(_activeMessage+1);
		}
		void newMess()
		{
			if (_mission.Messages.Count == Mission.MessageLimit)
			{
				MessageBox.Show("Mission contains maximum number of Messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			_activeMessage = _mission.Messages.Add();
			if (_mission.Messages.Count == 1) enableMessage(true);
			lstMessages.Items.Add(_mission.Messages[_activeMessage].MessageString);
			lstMessages.SelectedIndex = _activeMessage;
			Common.Title(this, _loading);
		}

		void lblMessArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			int m;
			try
			{
				l = (Label)sender;
				l.Focus();
				m = (lblMess1 == l ? 0 : 1);	// selected
			}
			catch (InvalidCastException) { m = (int)sender; l = (m==0 ? lblMess1 : lblMess2); }
			l.ForeColor = SystemColors.Highlight;
			(m==0 ? lblMess2 : lblMess1).ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboMessTrig.SelectedIndex = _mission.Messages[_activeMessage].Triggers[m].Condition;
			cboMessType.SelectedIndex = -1;
			cboMessType.SelectedIndex = _mission.Messages[_activeMessage].Triggers[m].VariableType;
			cboMessAmount.SelectedIndex = _mission.Messages[_activeMessage].Triggers[m].Amount;
			_loading = btemp;
		}
		void lblMessArr_DoubleClick(object sender, EventArgs e)
		{
			menuPaste_Click("MessTrig", new EventArgs());
		}
		void lblMessArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("MessTrig", new EventArgs());
		}

		void cboMessAmount_Leave(object sender, EventArgs e)
		{
			int m = (lblMess1.ForeColor == SystemColors.Highlight ? 0 : 1);
			_mission.Messages[_activeMessage].Triggers[m].Amount = Convert.ToByte(Common.Update(this, _mission.Messages[_activeMessage].Triggers[m].Amount, cboMessAmount.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[m], (m==0 ? lblMess1 : lblMess2));
		}
		void cboMessColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading) 
				_mission.Messages[_activeMessage].Color = Convert.ToByte(Common.Update(this, _mission.Messages[_activeMessage].Color, cboMessColor.SelectedIndex));
			messlistRefresh();
		}
		void cboMessTrig_Leave(object sender, EventArgs e)
		{
			int m = (lblMess1.ForeColor == SystemColors.Highlight ? 0 : 1);
			_mission.Messages[_activeMessage].Triggers[m].Condition = Convert.ToByte(Common.Update(this, _mission.Messages[_activeMessage].Triggers[m].Condition, cboMessTrig.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[m], (m==0 ? lblMess1 : lblMess2));
		}
		void cboMessType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMessType.SelectedIndex == -1) return;
			int m = (lblMess1.ForeColor == SystemColors.Highlight ? 0 : 1);
			if (!_loading) 
				_mission.Messages[_activeMessage].Triggers[m].VariableType = Convert.ToByte(Common.Update(this, _mission.Messages[_activeMessage].Triggers[m].VariableType, cboMessType.SelectedIndex));
			comboVarRefresh(_mission.Messages[_activeMessage].Triggers[m].VariableType, cboMessVar);
			try { cboMessVar.SelectedIndex = _mission.Messages[_activeMessage].Triggers[m].Variable; }
			catch { cboMessVar.SelectedIndex = 0; _mission.Messages[_activeMessage].Triggers[m].Variable = 0; }
			labelRefresh(_mission.Messages[_activeMessage].Triggers[m], (m==0 ? lblMess1 : lblMess2));
		}
		void cboMessVar_Leave(object sender, EventArgs e)
		{
			int m = (lblMess1.ForeColor == SystemColors.Highlight ? 0 : 1);
			_mission.Messages[_activeMessage].Triggers[m].Variable = Convert.ToByte(Common.Update(this, _mission.Messages[_activeMessage].Triggers[m].Variable, cboMessVar.SelectedIndex));
			labelRefresh(_mission.Messages[_activeMessage].Triggers[m], (m==0 ? lblMess1 : lblMess2));
		}

		void lstMessages_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (_mission.Messages.Count == 0) return;
			if (_mission.Messages[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch(_mission.Messages[e.Index].Color)
			{
				case 0:
					brText = Brushes.Crimson;
					break;
				case 1:
					brText = Brushes.LimeGreen;
					break;
				case 2:
					brText = Brushes.RoyalBlue;
					break;
				case 3:
					brText = Brushes.DarkOrchid;
					break;
			}
			e.Graphics.DrawString(lstMessages.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMessages.SelectedIndex == -1) return;
			_activeMessage = lstMessages.SelectedIndex;
			lblMessage.Text = "Message #" + (_activeMessage+1) + " of " + _mission.Messages.Count;
			bool btemp = _loading;
			_loading = true;
			labelRefresh(_mission.Messages[_activeMessage].Triggers[0], lblMess1);
			labelRefresh(_mission.Messages[_activeMessage].Triggers[1], lblMess2);
			txtMessage.Text = _mission.Messages[_activeMessage].MessageString;
			txtShort.Text = _mission.Messages[_activeMessage].Short;
			cboMessColor.SelectedIndex = _mission.Messages[_activeMessage].Color;
			numMessDelay.Value = _mission.Messages[_activeMessage].Delay * 5;
			optMessOR.Checked = _mission.Messages[_activeMessage].Trig1AndOrTrig2;
			optMessAND.Checked = !optMessOR.Checked;
			lblMessArr_Click(0, new System.EventArgs());
			_loading = btemp;
		}

		void numMessDelay_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Delay = Convert.ToByte(Common.Update(this, _mission.Messages[_activeMessage].Delay, numMessDelay.Value / 5));
		}

		void optMessOR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				_mission.Messages[_activeMessage].Trig1AndOrTrig2 = (bool)Common.Update(this, _mission.Messages[_activeMessage].Trig1AndOrTrig2, optMessOR.Checked);
		}

		void txtMessage_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].MessageString = Common.Update(this, _mission.Messages[_activeMessage].MessageString, txtMessage.Text).ToString();
			messlistRefresh();
		}
		void txtShort_Leave(object sender, EventArgs e)
		{
			_mission.Messages[_activeMessage].Short = Common.Update(this, _mission.Messages[_activeMessage].Short, txtShort.Text).ToString();
		}
		#endregion
		#region Globals
		void lblGlobArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				_activeGlobalGoal = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { _activeGlobalGoal = Convert.ToByte(sender); l = lblGlob[_activeGlobalGoal]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<6;i++) if (i!=_activeGlobalGoal) lblGlob[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboGlobalTrig.SelectedIndex = _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Condition;
			cboGlobalType.SelectedIndex = -1;
			cboGlobalType.SelectedIndex = _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].VariableType;
			cboGlobalAmount.SelectedIndex = _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Amount;
			_loading = btemp;
		}
		void lblGlobArr_DoubleClick(object sender, EventArgs e)
		{
			menuPaste_Click("Glob", new EventArgs());
		}
		void lblGlobArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Glob", new EventArgs());
		}

		void cboGlobalAmount_Leave(object sender, EventArgs e)
		{
			_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Amount = Convert.ToByte(Common.Update(this, _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Amount, cboGlobalAmount.SelectedIndex));
			labelRefresh(_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2], lblGlob[_activeGlobalGoal]);
		}
		void cboGlobalTrig_Leave(object sender, EventArgs e)
		{
			_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Condition = Convert.ToByte(Common.Update(this, _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Condition, cboGlobalTrig.SelectedIndex));
			labelRefresh(_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2], lblGlob[_activeGlobalGoal]);
		}
		void cboGlobalType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cboGlobalType.SelectedIndex == -1) return;
			if (!_loading)
				_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].VariableType = Convert.ToByte(Common.Update(this, _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].VariableType, cboGlobalType.SelectedIndex));
			comboVarRefresh(_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].VariableType, cboGlobalVar);
			try { cboGlobalVar.SelectedIndex = _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Variable; }
			catch { cboGlobalVar.SelectedIndex = 0; _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Variable = 0; }
			labelRefresh(_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2], lblGlob[_activeGlobalGoal]);
		}
		void cboGlobalVar_Leave(object sender, EventArgs e)
		{
			_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Variable = Convert.ToByte(Common.Update(this, _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2].Variable, cboGlobalAmount.SelectedIndex));
			labelRefresh(_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].Triggers[_activeGlobalGoal % 2], lblGlob[_activeGlobalGoal]);
		}

		void optBonOR_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
				_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].T1AndOrT2 = (bool)Common.Update(this, _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].T1AndOrT2, optBonOR.Checked);
		}
		void optPrimOR_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
				_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].T1AndOrT2 = (bool)Common.Update(this, _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].T1AndOrT2, optPrimOR.Checked);
		}
		void optSecOR_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
				_mission.GlobalGoals.Goals[_activeGlobalGoal / 2].T1AndOrT2 = (bool)Common.Update(this, _mission.GlobalGoals.Goals[_activeGlobalGoal / 2].T1AndOrT2, optSecOR.Checked);
		}
		#endregion
		#region Officers
		void cboOfficer_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cboQuestion.SelectedIndex == -1) cboQuestion.SelectedIndex = 0;
			if (cboOfficer.SelectedIndex <= 1)
			{
				cboQTrigType.Enabled = false;
				cboQTrig.Enabled = false;
			}
			else
			{
				cboQTrig.Enabled = true;
				cboQTrigType.Enabled = true;
			}
			cboQuestion_SelectedIndexChanged("cboOfficer", new EventArgs());
		}
		void cboQTrig_Leave(object sender, EventArgs e)
		{
			_mission.BriefingQuestions.PostTrigger[cboQuestion.SelectedIndex] = Convert.ToByte(Common.Update(this, _mission.BriefingQuestions.PostTrigger[cboQuestion.SelectedIndex], cboQTrig.SelectedIndex));
		}
		void cboQTrigType_Leave(object sender, EventArgs e)
		{
			_mission.BriefingQuestions.PostTrigType[cboQuestion.SelectedIndex] = Convert.ToByte(Common.Update(this, _mission.BriefingQuestions.PostTrigType[cboQuestion.SelectedIndex], cboQTrigType.SelectedIndex));
		}
		void cboQuestion_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool bTemp = _loading;
			_loading = true;
			int a = 0;	//place holder
			if (cboOfficer.SelectedIndex <= 1)	//if pre-miss
			{
				if (cboOfficer.SelectedIndex == 1) a = 5;	//if Secret Order, set place holder
				txtQuestion.Text = _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + a];
				txtAnswer.Text = _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + a];
			}
			else	//post-miss
			{
				cboQTrigType.SelectedIndex = _mission.BriefingQuestions.PostTrigType[cboQuestion.SelectedIndex];
				cboQTrig.SelectedIndex = _mission.BriefingQuestions.PostTrigger[cboQuestion.SelectedIndex];
				if (cboOfficer.SelectedIndex == 3) a = 5;
				txtQuestion.Text = _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + a];
				txtAnswer.Text = _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + a];
			}
			_loading = bTemp;
		}

		void cmdPreview_Click(object sender, EventArgs e)
		{
			txtAnswer_Leave("cmdPreview", new EventArgs());
			txtQuestion_Leave("cmdPreview", new EventArgs());
			_fOfficers = new OfficerPreviewForm(_mission.BriefingQuestions);
			_fOfficers.Show();
		}

		void optOfficers_Leave(object sender, EventArgs e)
		{
			RadioButton o = (RadioButton)sender;
			_mission.OfficersPresent = (Mission.BriefingOfficers)Common.Update(this, _mission.OfficersPresent, (Mission.BriefingOfficers)o.Tag);
		}

		void txtAnswer_Leave(object sender, EventArgs e)
		{
			string t = null;
			if (cboOfficer.SelectedIndex == 0) t = _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex];
			else if (cboOfficer.SelectedIndex == 1) t = _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + 5];
			else if (cboOfficer.SelectedIndex == 2) t = _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex];
			else t = _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + 5];
			t = Common.Update(this, t, txtAnswer.Text).ToString();
			if (cboOfficer.SelectedIndex == 0) _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex] = t;
			else if (cboOfficer.SelectedIndex == 1) _mission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + 5] = t;
			else if (cboOfficer.SelectedIndex == 2) _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex] = t;
			else _mission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + 5] = t;
		}
		void txtQuestion_Leave(object sender, EventArgs e)
		{
			string t = null;
			if (cboOfficer.SelectedIndex == 0) t = _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex];
			else if (cboOfficer.SelectedIndex == 1) t = _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + 5];
			else if (cboOfficer.SelectedIndex == 2) t = _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex];
			else t = _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + 5];
			t = Common.Update(this, t, txtQuestion.Text).ToString();
			if (cboOfficer.SelectedIndex == 0) _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex] = t;
			else if (cboOfficer.SelectedIndex == 1) _mission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + 5] = t;
			else if (cboOfficer.SelectedIndex == 2) _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex] = t;
			else _mission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + 5] = t;
		}
		#endregion
		#region Mission
		void chkIFFArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			_mission.IffHostile[(int)c.Tag] = (bool)Common.Update(this, _mission.IffHostile[(int)c.Tag], c.Checked);
		}
		void txtEoMArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.EndOfMissionMessages[(int)t.Tag] = Common.Update(this, _mission.EndOfMissionMessages[(int)t.Tag], t.Text).ToString();
		}
		void txtIFFArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			_mission.IFFs[(int)t.Tag] = Common.Update(this, _mission.IFFs[(int)t.Tag], t.Text).ToString();
			cboIFF.Items[(int)t.Tag] = t.Text;
		}

		void optCapture_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
				_mission.CapturedOnEjection = (bool)Common.Update(this, _mission.CapturedOnEjection, optCapture.Checked);
		}
		#endregion
	}
}