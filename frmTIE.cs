/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.1
 */

/* CHANGELOG
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
	public partial class frmTIE : Form
	{
		#region vars and stuff
		Settings config = new Settings();
		Briefing TieBriefing;
		Mission TieMission;
		bool bAppExit;				//for frmTIE_Closing, confirms application exit vs switching platforms
		int activeFG = 0;			//counter to keep track of current FG being displayed
		int startingShips = 1;		//counter for craft in play at start <30s, warning above 28
		bool _loading = false;		//alerts certain functions to disable during the loading process
		int activeMessage = 0;
		DataView dataWaypoints;
		DataTable table = new DataTable("Waypoints");
		DataTable tableRaw = new DataTable("Waypoints_Raw");
		DataView dataWaypointsRaw;
		frmMap fMap;
		frmBrief fBrief;
		frmBattle fBattle;
		frmOfficerPreview fOfficers;
		byte activeGlobalGoal;
		byte activeArrDepTrigger;
		byte activeOrder;
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
		#endregion

		public frmTIE()
		{
			InitializeComponent();
			_loading = true;
			InitializeMission();
			Startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		public frmTIE(string path)
		{	//this is the command line and "Open..." support
			InitializeComponent();
			_loading = true;
			InitializeMission();
			Startup();
			if(!LoadMission(path)) return;
			lstFG.SelectedIndex = 0;
			if (TieMission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			_loading = false;
		}

		void CloseForms()
		{
			try { fMap.Close(); }
			catch { /* do nothing */ }
			try { fBrief.Close(); }
			catch { /* do nothing */ }
			try { fBattle.Close(); }
			catch { /* do nothing */ }
			try { fOfficers.Close(); }
			catch { /* do nothing */ }
		}
		void ComboVarRefresh(int index, ComboBox cbo)
		{	//index is usually cboType.SelectedIndex, cbo = cboVar
			if (index == -1) return;
			cbo.Items.Clear();
			switch (index)		//switch (VariableType)
			{
				case 0:
					cbo.Items.Add("None");
					break;
				case 1: //FlightGroup
					cbo.Items.AddRange(TieMission.FlightGroups.GetList());
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
					string[] t = new string[TieMission.IFFs.Length];
					for (int i = 0; i < t.Length; i++) t[i] = TieMission.IFFs[i];
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
		void ComboReset(ComboBox cbo, string[] items, int index)
		{
			cbo.Items.Clear();
			cbo.Items.AddRange(items);
			cbo.SelectedIndex = index;
		}
		void CraftStart(FlightGroup fg, bool bAdd)
		{
			if (fg.ArrivesIn30Seconds)
			{
				if (bAdd) startingShips += fg.NumberOfCraft;
				else startingShips -= fg.NumberOfCraft;
			}
			lblStarting.Text = startingShips.ToString() + " Craft at 30 seconds";
			if (startingShips > Mission.CraftLimit) lblStarting.ForeColor = Color.Red;
			else lblStarting.ForeColor = SystemColors.ControlText;
		}
		void InitializeMission()
		{
			TieMission = new Mission();
			TieBriefing = TieMission.Briefing;
			config.LastMission = "";
			activeFG = 0;
			activeMessage = 0;
			TieMission.FlightGroups[0].CraftType = Convert.ToByte(config.TIECraft);
			TieMission.FlightGroups[0].IFF = Convert.ToByte(config.TIEIFF);
			string[] fgList = TieMission.FlightGroups.GetList();
			ComboReset(cboArrMS, fgList, 0);
			ComboReset(cboArrMSAlt, fgList, 0);
			ComboReset(cboDepMS, fgList, 0);
			ComboReset(cboDepMSAlt, fgList, 0);
			lstFG.Items.Clear();
			lstFG.Items.Add(TieMission.FlightGroups[activeFG].ToString(true));
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			string[] t = new string[TieMission.IFFs.Length];
			for (int i = 0; i < t.Length; i++) t[i] = TieMission.IFFs[i];
			ComboReset(cboIFF, t, 0);
			this.Text = "Ye Olde Galactic Empire Mission Editor - TIE - New Mission.tie";
		}
		void LabelRefresh(Mission.Trigger trigger, Label lbl)
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
				text = text.Replace("FG:" + fg, TieMission.FlightGroups[fg].ToString());
			}
			return text;
		}
		bool LoadMission(string fileMission)
		{
			/* return true if successful, returns false if aborted or failed
			 * code is fairly straight-forward. read the crap and save it */
			CloseForms();
			lstFG.Items.Clear();
			lstMessages.Items.Clear();
			startingShips = 0;
			try
			{
				FileStream fs = File.OpenRead(fileMission);
				try
				{
					#region determine platform
					switch (Platform.MissionFile.GetPlatform(fs))
					{
						case Platform.MissionFile.Platform.TIE:
							break;
						case Platform.MissionFile.Platform.XvT:
							bAppExit = false;
							new frmXvT(fileMission).Show();
							Close();
							return false;
						case Platform.MissionFile.Platform.BoP:
							bAppExit = false;
							new frmXvT(fileMission).Show();
							Close();
							return false;
						case Platform.MissionFile.Platform.XWA:
							bAppExit = false;
							new frmXWA(fileMission).Show();
							Close();
							return false;
						default:
							throw new Exception("File is not a valid mission file for any platform, please select an appropriate *.tie file.");
					}
					#endregion
					TieMission.LoadFromStream(fs);
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
			for (int i=0;i<TieMission.FlightGroups.Count;i++)
			{
				lstFG.Items.Add(TieMission.FlightGroups[i].ToString(true));
				if (TieMission.FlightGroups[i].ArrivesIn30Seconds) CraftStart(TieMission.FlightGroups[i], true);
			}
			UpdateFGList();
			if (TieMission.Messages.Count == 0) EnableMessages(false);
			else
			{
				EnableMessages(true);
				for (int i=0;i<TieMission.Messages.Count;i++) lstMessages.Items.Add(TieMission.Messages[i].MessageString);
			}
			string[] t = new string[TieMission.IFFs.Length];
			for (int i = 0; i < t.Length; i++) t[i] = TieMission.IFFs[i];
			ComboReset(cboIFF, t, 0);
			UpdateMissionTabs();
			TieMission.MissionPath = fileMission;
			this.Text = "Ye Olde Galactic Empire Mission Editor - TIE - " + TieMission.MissionFileName;
			config.LastMission = fileMission;
			return true;
		}
		void PromptSave()
		{
			if (config.ConfirmSave && (this.Text.IndexOf("*") != -1))
			{
				DialogResult res = MessageBox.Show("Mission has been edited without saving, would you like to save?", "Confirm", MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes)
				{
					if (TieMission.MissionPath == "\\NewMission.tie") savTIE.ShowDialog();
					else SaveMission(TieMission.MissionPath);
				}
			}
		}
		void SaveMission(string fileMission)
		{
			try { fBrief.Save(); }
			catch { /* do nothing */ }
			TieMission.Briefing = TieBriefing;
			try { TieMission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			this.Text = "Ye Olde Galactic Empire Mission Editor - TIE - " + TieMission.MissionFileName;
			config.LastMission = fileMission;
			//Verify the mission after it's been saved
			if (config.Verify) Common.RunVerify(TieMission.MissionPath, config.VerifyLocation);
		}
		void Startup()
		{
			//initializes cbo's, IFFs, resets bAppExit
			string[] t = new string[TieMission.IFFs.Length];
			for (int i = 0; i < t.Length; i++) t[i] = TieMission.IFFs[i];
			ComboReset(cboIFF, t, 0);
			config.LastMission = "";
			config.LastPlatform = Settings.Platform.TIE;
			if (Directory.Exists(config.TIEPath))
			{
				opnTIE.InitialDirectory = config.TIEPath;
				savTIE.InitialDirectory = config.TIEPath;
			}
			bAppExit = true;	//becomes false if selecting "New Mission" from menu
			if (config.RestrictPlatforms)
			{
				if (!config.XvtInstalled) { menuNewXvT.Enabled = false; }
				if (!config.BopInstalled) { menuNewBoP.Enabled = false; }
				if (!config.XwaInstalled) { menuNewXWA.Enabled = false; }
			}
			#region Craft
			cboCraft.Items.AddRange(Strings.CraftType); cboCraft.SelectedIndex = TieMission.FlightGroups[0].CraftType;
			cboIFF.SelectedIndex = TieMission.FlightGroups[0].IFF;	// already loaded default IFFs at start of function through txtIFF#.Text
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
			activeOrder = 0;
			cboOrders.Items.AddRange(Strings.Orders); cboOrders.SelectedIndex = 0;
			cboOT1Type.Items.AddRange(Strings.VariableType); cboOT1Type.SelectedIndex = 0;
			cboOT2Type.Items.AddRange(Strings.VariableType); cboOT2Type.SelectedIndex = 0;
			cboOT3Type.Items.AddRange(Strings.VariableType); cboOT3Type.SelectedIndex = 0;
			cboOT4Type.Items.AddRange(Strings.VariableType); cboOT4Type.SelectedIndex = 0;
			#endregion
			#region Waypoints
			table.Columns.Add("X"); table.Columns.Add("Y"); table.Columns.Add("Z");
			tableRaw.Columns.Add("X"); tableRaw.Columns.Add("Y"); tableRaw.Columns.Add("Z");
			for (int i=0;i<15;i++)	//initialize WPs
			{
				DataRow dr = table.NewRow();
				int j;
				for(j=0;j<3;j++) dr[j] = 0;	//set X Y Z to zero
				table.Rows.Add(dr);
				dr = tableRaw.NewRow();
				for(j=0;j<3;j++) dr[j] = 0;	//mirror in raw table
				tableRaw.Rows.Add(dr);
			}
			dataWaypoints.Table = table;
			dataWaypointsRaw.Table = tableRaw;
			dataWP.DataSource = dataWaypoints;
			dataWP_Raw.DataSource = dataWaypointsRaw;
			this.table.RowChanged += new DataRowChangeEventHandler(table_RowChanged);
			this.tableRaw.RowChanged += new DataRowChangeEventHandler(tableRaw_RowChanged);
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
				chkWP[i].Leave += new EventHandler(chkWPArr_Leave);
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
			activeGlobalGoal = 0;
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
			UpdateMissionTabs();
		}
		void UpdateMissionTabs()
		{
			#region Globals tab
			optPrimOR.Checked = TieMission.GlobalGoals.Goals[0].T1AndOrT2;
			optPrimAND.Checked = !optPrimOR.Checked;
			optSecOR.Checked = TieMission.GlobalGoals.Goals[1].T1AndOrT2;
			optSecAND.Checked = !optSecOR.Checked;
			optBonOR.Checked = TieMission.GlobalGoals.Goals[2].T1AndOrT2;
			optBonAND.Checked = !optBonOR.Checked;
			for (int i=0;i<6;i++) LabelRefresh(TieMission.GlobalGoals.Goals[i/2].Triggers[i%2], lblGlob[i]);
			lblGlobArr_Click(0, new System.EventArgs());
			#endregion
			#region Mission tab
			optCapture.Checked = TieMission.CapturedOnEjection;
			optRescue.Checked = !optCapture.Checked;
			for (int i=0;i<6;i++) txtEoM[i].Text = TieMission.EndOfMissionMessages[i];
			for (int i=0;i<4;i++)
			{
				txtIFF[i].Text = TieMission.IFFs[i+2];
				chkIFF[i].Checked = TieMission.IffHostile[i+2];
			}
			#endregion
			#region Questions tab
			if (TieMission.OfficersPresent == Mission.BriefingOfficers.Both) optBoth.Checked = true;
			else if (TieMission.OfficersPresent == Mission.BriefingOfficers.FlightOfficer) optFO.Checked = true;
			else optSO.Checked = true;
			txtQuestion.Text = TieMission.BriefingQuestions.PreMissQuestions[0];
			txtAnswer.Text = TieMission.BriefingQuestions.PreMissAnswers[0];
			#endregion
		}

		void frmTIE_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			PromptSave();
			if (config.ConfirmExit && bAppExit)
			{
				DialogResult res = MessageBox.Show("Are you sure you wish to exit?", "Confirm", MessageBoxButtons.YesNo);
				if (res == DialogResult.No) { e.Cancel = true; return; }
			}
			CloseForms();
			config.SaveSettings();
			if (bAppExit) Application.Exit();
		}

		void opnTIE_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_loading = true;
			InitializeMission();
			if (LoadMission(opnTIE.FileName))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				activeFG = 0;
				lstFG.SelectedIndex = 0;
				_loading = true;		//turned false in previous line
				if (TieMission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			}
			_loading = false;
		}

		void savTIE_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveMission(savTIE.FileName);
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
					if (tabMain.SelectedIndex == 0) NewFG();
					else if (tabMain.SelectedIndex == 1) NewMess();
					break;
				case 6:		//Delete Item
					if (tabMain.SelectedIndex == 0) DeleteFG();
					else if (tabMain.SelectedIndex == 1) DeleteMess();
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
			new dlgAbout().ShowDialog();
		}
		void menuBattle_Click(object sender, EventArgs e)
		{
			fBattle = new frmBattle();
			fBattle.Show();
		}
		void menuBriefing_Click(object sender, EventArgs e)
		{
			Common.Title(this, _loading);
			fBrief = new frmBrief(TieMission.FlightGroups, ref TieBriefing);
			// TODO: see if I can pass TieMission.Briefing by itself without ref
			fBrief.Show();
		}
		void menuCopy_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream = new FileStream("YOGEME.bin", FileMode.Create, FileAccess.Write, FileShare.None);
			#region ArrDep
			if (sender.ToString() == "AD")
			{
				formatter.Serialize(stream, TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger]);
				stream.Close();
				return;
			}
			#endregion
			#region Orders
			if (sender.ToString() == "Order")
			{
				formatter.Serialize(stream, TieMission.FlightGroups[activeFG].Orders[activeOrder]);
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
				if (lblMess1.ForeColor == SystemColors.Highlight) formatter.Serialize(stream, TieMission.Messages[activeMessage].Triggers[0]);
				else formatter.Serialize(stream, TieMission.Messages[activeMessage].Triggers[1]);
				stream.Close();
				return;
			}
			#endregion
			switch (tabMain.SelectedIndex)
			{
				case 0:
					formatter.Serialize(stream, TieMission.FlightGroups[activeFG]);
					break;
				case 1:
					if (TieMission.Messages.Count != 0) formatter.Serialize(stream, TieMission.Messages[activeMessage]);
					break;
				case 2:
					formatter.Serialize(stream, TieMission.GlobalGoals.Goals[activeGlobalGoal/2].Triggers[activeGlobalGoal%2]);
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
			fMap = new frmMap(TieMission.FlightGroups);
			fMap.Show();
		}
		void menuNewBoP_Click(object sender, EventArgs e)
		{
			menuNewXvT_Click("BoP", new EventArgs());
		}
		void menuNewTIE_Click(object sender, EventArgs e)
		{
			PromptSave();
			CloseForms();
			_loading = true;
			InitializeMission();
			lstMessages.Items.Clear();
			EnableMessages(false);
			lblMessage.Text = "Message #0 of 0";
			lstFG.SelectedIndex = 0;
			startingShips = 1;
			lblStarting.Text = "1 Craft at 30 seconds";
			UpdateMissionTabs();
			_loading = false;
			if (this.Text.EndsWith("*")) this.Text = this.Text.Substring(0, this.Text.Length-1);
		}
		void menuNewXvT_Click(object sender, EventArgs e)
		{
			PromptSave();
			CloseForms();
			bool BoP = (sender.ToString() == "BoP" ? true : false);
			bAppExit = false;
			new frmXvT(BoP).Show();
			Close();
		}
		void menuNewXWA_Click(object sender, EventArgs e)
		{
			PromptSave();
			CloseForms();
			bAppExit = false;
			new frmXWA().Show();
			Close();
		}
		void menuOpen_Click(object sender, EventArgs e)
		{
			PromptSave();
			opnTIE.ShowDialog();
		}
		void menuOptions_Click(object sender, EventArgs e)
		{
			new dlgOptions(config).ShowDialog();
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
					TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger] = (Mission.Trigger)formatter.Deserialize(stream);
					lblADTrigArr_Click(activeArrDepTrigger, new EventArgs());
					LabelRefresh(TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
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
					TieMission.FlightGroups[activeFG].Orders[activeOrder] = (FlightGroup.Order)formatter.Deserialize(stream);
					lblOrderArr_Click(activeOrder, new EventArgs());
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
						TieMission.Messages[activeMessage].Triggers[0] = (Mission.Trigger)formatter.Deserialize(stream);
						//for (int i=0;i<4;i++) TieMission.Messages[activeMessage].Triggers[0][i] = b_temp[i];
						lblMessArr_Click(0, new EventArgs());
					}
					else
					{
						TieMission.Messages[activeMessage].Triggers[1] = (Mission.Trigger)formatter.Deserialize(stream);
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
						NewFG();
						TieMission.FlightGroups[activeFG] = fg_temp;
						ListRefresh();
						startingShips--;
						lstFG.SelectedIndex = activeFG;
						CraftStart(fg_temp, true);
					}
					catch { /* do nothing */ }
					break;
				case 1:
					try
					{
						Platform.Tie.Message mess_temp = (Platform.Tie.Message)formatter.Deserialize(stream);
						if (mess_temp == null) throw new Exception();
						NewMess();
						TieMission.Messages[activeMessage] = mess_temp;
						MessListRefresh();
						lstMessages.SelectedIndex = activeMessage;
					}
					catch { /* do nothing */ }
					break;
				case 2:
					try
					{
						TieMission.GlobalGoals.Goals[activeGlobalGoal/2].Triggers[activeGlobalGoal%3] = (Mission.Trigger)formatter.Deserialize(stream);
						lblGlobArr_Click(activeGlobalGoal, new EventArgs());
						Common.Title(this, false);
					}
					catch { /* do nothing */ }
					break;
			}
			#endregion
			stream.Close();
		}
		void menuSave_Click(object sender, EventArgs e)
		{
			if (TieMission.MissionPath == "\\NewMission.tie") savTIE.ShowDialog();
			else SaveMission(TieMission.MissionPath);
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
			Common.RunConverter(TieMission.MissionPath, 0);
		}
		void menuSaveAsXWA_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsXWA", new System.EventArgs());
			Common.RunConverter(TieMission.MissionPath, 1);
		}
		void menuVerify_Click(object sender, EventArgs e)
		{
			menuSave_Click("Verify", new EventArgs());
			Common.RunVerify(TieMission.MissionPath, config.VerifyLocation);
		}
		void menuTest_Click(object sender, EventArgs e)
		{
			if (config.ConfirmTest)
			{
				DialogResult res = new dlgTest(config).ShowDialog();
				if (res == DialogResult.Cancel) return;
			}
			menuSave_Click("menuTest_Click", new EventArgs());
			if (config.VerifyTest && !config.Verify) Common.RunVerify(TieMission.MissionPath, config.VerifyLocation);
			// TODO: test code
		}
		#endregion
		#region FlightGroups
		void DeleteFG()
		{
			if (TieMission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(activeFG);
			CraftStart(TieMission.FlightGroups[activeFG], false);
			if (TieMission.FlightGroups.Count == 1)
			{
				TieMission.FlightGroups.Clear();
				activeFG = 0;
				TieMission.FlightGroups[0].CraftType = config.TIECraft;
				TieMission.FlightGroups[0].IFF = config.TIEIFF;
				CraftStart(TieMission.FlightGroups[0], true);
			}
			else activeFG = TieMission.FlightGroups.RemoveAt(activeFG);
			UpdateFGList();
			lstFG.SelectedIndex = activeFG;
			Common.Title(this, _loading);
			try
			{
				fMap.Import(TieMission.FlightGroups);
				fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
			try
			{
				fBrief.Import(TieMission.FlightGroups);
				fBrief.MapPaint();
			}
			catch { /* do nothing */ }
		}
		void ListRefresh()
		{
			lstFG.Items[activeFG] = TieMission.FlightGroups[activeFG].ToString(true);
		}
		void NewFG()
		{
			if (TieMission.FlightGroups.Count == Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			activeFG = TieMission.FlightGroups.Add();
			TieMission.FlightGroups[activeFG].CraftType = config.TIECraft;
			TieMission.FlightGroups[activeFG].IFF = config.TIEIFF;
			CraftStart(TieMission.FlightGroups[activeFG], true);
			lstFG.Items.Add(TieMission.FlightGroups[activeFG].ToString(true));
			UpdateFGList();
			lstFG.SelectedIndex = activeFG;
			Common.Title(this, _loading);
			try
			{
				fMap.Import(TieMission.FlightGroups);
				fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
			try
			{
				fBrief.Import(TieMission.FlightGroups);
				fBrief.MapPaint();
			}
			catch { /* do nothing */ }
		}
		void UpdateFGList()
		{
			string[] fgList = TieMission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			ComboReset(cboArrMS, fgList, TieMission.FlightGroups[activeFG].ArrivalCraft1);
			ComboReset(cboArrMSAlt, fgList, TieMission.FlightGroups[activeFG].ArrivalCraft2);
			ComboReset(cboDepMS, fgList, TieMission.FlightGroups[activeFG].DepartureCraft1);
			ComboReset(cboDepMSAlt, fgList, TieMission.FlightGroups[activeFG].DepartureCraft2);
			_loading = temp;
			ListRefresh();
		}

		void lstFG_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1 || TieMission.FlightGroups[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch(TieMission.FlightGroups[e.Index].IFF)
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
			activeFG = lstFG.SelectedIndex;
			lblFG.Text = "Flight Group #" + (activeFG+1).ToString() + " of " + TieMission.FlightGroups.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			#region Craft
			txtName.Text = TieMission.FlightGroups[activeFG].Name;
			txtPilot.Text = TieMission.FlightGroups[activeFG].Pilot;
			txtCargo.Text = TieMission.FlightGroups[activeFG].Cargo;
			txtSpecCargo.Text = TieMission.FlightGroups[activeFG].SpecialCargo;
			numSC.Value = TieMission.FlightGroups[activeFG].SpecialCargoCraft;
			chkRandSC.Checked = TieMission.FlightGroups[activeFG].RandSpecCargo;
			numCraft.Value = TieMission.FlightGroups[activeFG].NumberOfCraft;
			numWaves.Value = TieMission.FlightGroups[activeFG].NumberOfWaves;
			numGlobal.Value = TieMission.FlightGroups[activeFG].GlobalGroup;
			cboCraft.SelectedIndex = TieMission.FlightGroups[activeFG].CraftType;
			cboIFF.SelectedIndex = TieMission.FlightGroups[activeFG].IFF;
			cboAI.SelectedIndex = TieMission.FlightGroups[activeFG].AI;
			cboMarkings.SelectedIndex = TieMission.FlightGroups[activeFG].Markings;
			cboPlayer.SelectedIndex = TieMission.FlightGroups[activeFG].PlayerCraft;
			cboFormation.SelectedIndex = TieMission.FlightGroups[activeFG].Formation;
			chkRadio.Checked = Convert.ToBoolean(TieMission.FlightGroups[activeFG].FollowsOrders);
			numLead.Value = TieMission.FlightGroups[activeFG].FormLeaderDist;
			numSpacing.Value = TieMission.FlightGroups[activeFG].FormDistance;
			cboStatus.SelectedIndex = TieMission.FlightGroups[activeFG].Status1;
			cboWarheads.SelectedIndex = TieMission.FlightGroups[activeFG].Missile;
			cboBeam.SelectedIndex = TieMission.FlightGroups[activeFG].Beam;
			#endregion
			#region Arr/Dep
			optArrMS.Checked = Convert.ToBoolean(TieMission.FlightGroups[activeFG].ArrivalMethod1);
			optArrHyp.Checked = !optArrMS.Checked;
			optArrMSAlt.Checked = Convert.ToBoolean(TieMission.FlightGroups[activeFG].ArrivalMethod2);
			optArrHypAlt.Checked = !optArrMSAlt.Checked;
			optDepMS.Checked = Convert.ToBoolean(TieMission.FlightGroups[activeFG].DepartureMethod1);
			optDepHyp.Checked = !optDepMS.Checked;
			optDepMSAlt.Checked = Convert.ToBoolean(TieMission.FlightGroups[activeFG].DepartureMethod2);
			optDepHypAlt.Checked = !optDepMSAlt.Checked;
			try { cboArrMS.SelectedIndex = TieMission.FlightGroups[activeFG].ArrivalCraft1; }
			catch { cboArrMS.SelectedIndex = 0; TieMission.FlightGroups[activeFG].ArrivalCraft1 = 0; optArrHyp.Checked = true; }
			try { cboArrMSAlt.SelectedIndex = TieMission.FlightGroups[activeFG].ArrivalCraft2; }
			catch { cboArrMSAlt.SelectedIndex = 0; TieMission.FlightGroups[activeFG].ArrivalCraft2 = 0; optArrHypAlt.Checked = true; }
			try { cboDepMS.SelectedIndex = TieMission.FlightGroups[activeFG].DepartureCraft1; }
			catch { cboDepMS.SelectedIndex = 0; TieMission.FlightGroups[activeFG].DepartureCraft1 = 0; optDepHyp.Checked = true; }
			try { cboDepMSAlt.SelectedIndex = TieMission.FlightGroups[activeFG].DepartureCraft2; }
			catch { cboDepMSAlt.SelectedIndex = 0; TieMission.FlightGroups[activeFG].DepartureCraft2 = 0; optDepHypAlt.Checked = true; }
			optArrOR.Checked = TieMission.FlightGroups[activeFG].AT1AndOrAT2;
			optArrAND.Checked = !optArrOR.Checked;
			for (int i = 0; i < 3; i++) LabelRefresh(TieMission.FlightGroups[activeFG].ArrDepTriggers[i], lblADTrig[i]);
			numArrMin.Value = TieMission.FlightGroups[activeFG].ArrivalDelayMinutes;
			numArrSec.Value = TieMission.FlightGroups[activeFG].ArrivalDelaySeconds;
			numDepMin.Value = TieMission.FlightGroups[activeFG].DepartureTimerMinutes;
			numDepSec.Value = TieMission.FlightGroups[activeFG].DepartureTimerSeconds;
			cboAbort.SelectedIndex = TieMission.FlightGroups[activeFG].AbortTrigger;
			cboDiff.SelectedIndex = TieMission.FlightGroups[activeFG].Difficulty;
			lblADTrigArr_Click(0, new EventArgs());
			#endregion
			for (int i=0;i<8;i++) cboGoal[i].SelectedIndex = TieMission.FlightGroups[activeFG].Goals[i];
			numBonGoalP.Value = TieMission.FlightGroups[activeFG].Goals.BonusPoints;
			#region Waypoints
			for (int i=0;i<15;i++)
			{
				for (int j=0;j<3;j++)
				{
					tableRaw.Rows[i][j] = TieMission.FlightGroups[activeFG].Waypoints[i][j];
					table.Rows[i][j] = Math.Round((double)TieMission.FlightGroups[activeFG].Waypoints[i][j] / 160, 2);
				}
				chkWP[i].Checked = TieMission.FlightGroups[activeFG].Waypoints[i].Enabled;
			}
			table.AcceptChanges();
			tableRaw.AcceptChanges();
			numYaw.Value = TieMission.FlightGroups[activeFG].Yaw;
			numPitch.Value = TieMission.FlightGroups[activeFG].Pitch;
			numRoll.Value = TieMission.FlightGroups[activeFG].Roll;
			if (TieMission.FlightGroups[activeFG].CraftType <= 0x45) EnableRot(false);
			else EnableRot(true);
			#endregion
			for (activeOrder=0;activeOrder<3;activeOrder++) OrderLabelRefresh();
			lblOrderArr_Click(0, new EventArgs());
			for (int i=0;i<9;i++) numUnk[i].Value = TieMission.FlightGroups[activeFG].Unknowns[i];
			_loading = btemp; ;
			EnableBackdrop((TieMission.FlightGroups[activeFG].CraftType == 0x57 ? true : false));
		}

		#region Craft
		void EnableBackdrop(bool state)
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
			EnableBackdrop((cboCraft.SelectedIndex == 0x57 ? true : false));
			if (_loading) return;
			TieMission.FlightGroups[activeFG].CraftType = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].CraftType, cboCraft.SelectedIndex));
			EnableRot((TieMission.FlightGroups[activeFG].CraftType <= 0x45 ? false : true));
			UpdateFGList();
		}
		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.FlightGroups[activeFG].Formation = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Formation, cboFormation.SelectedIndex));
		}
		void cboStatus_Leave(object sender, EventArgs e)
		{
			numBackdrop.Value = cboStatus.SelectedIndex;
			TieMission.FlightGroups[activeFG].Status1 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Status1, cboStatus.SelectedIndex));
		}

		void chkRandSC_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.FlightGroups[activeFG].RandSpecCargo = (bool)Common.Update(this, TieMission.FlightGroups[activeFG].RandSpecCargo, chkRandSC.Checked);
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
				dlgBackdrop dlg = new dlgBackdrop(Platform.MissionFile.Platform.TIE, TieMission.FlightGroups[activeFG].Status1);
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
			dlgFormation dlg = new dlgFormation(TieMission.FlightGroups[activeFG].Formation);
			dlg.SetToTie95();
			if (dlg.ShowDialog() == DialogResult.OK) cboFormation.SelectedIndex = dlg.Formation;
		}

		void grpCraft2_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].IFF = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].IFF, cboIFF.SelectedIndex));
			TieMission.FlightGroups[activeFG].AI = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].AI, cboAI.SelectedIndex));
			TieMission.FlightGroups[activeFG].Markings = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Markings, cboMarkings.SelectedIndex));
			TieMission.FlightGroups[activeFG].PlayerCraft = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].PlayerCraft, cboPlayer.SelectedIndex));
			TieMission.FlightGroups[activeFG].FollowsOrders = (bool)Common.Update(this, TieMission.FlightGroups[activeFG].FollowsOrders, chkRadio.Checked);
			TieMission.FlightGroups[activeFG].FormDistance = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].FormDistance, numSpacing.Value));
			TieMission.FlightGroups[activeFG].FormLeaderDist = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].FormLeaderDist, numLead.Value));
			ListRefresh();
		}
		void grpCraft3_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].NumberOfWaves = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].NumberOfWaves, numWaves.Value));
			CraftStart(TieMission.FlightGroups[activeFG], false);
			TieMission.FlightGroups[activeFG].NumberOfCraft = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].NumberOfCraft, numCraft.Value));
			CraftStart(TieMission.FlightGroups[activeFG], true);
			if (TieMission.FlightGroups[activeFG].SpecialCargoCraft > TieMission.FlightGroups[activeFG].NumberOfCraft) numSC.Value = 0;
			TieMission.FlightGroups[activeFG].GlobalGroup = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].GlobalGroup, numGlobal.Value));
			ListRefresh();
		}
		void grpCraft4_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Missile = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Missile, cboWarheads.SelectedIndex));
			TieMission.FlightGroups[activeFG].Beam = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Beam, cboBeam.SelectedIndex));
		}

		void numBackdrop_Leave(object sender, EventArgs e)
		{
			cboStatus.SelectedIndex = (int)numBackdrop.Value;
			TieMission.FlightGroups[activeFG].Status1 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Status1, cboStatus.SelectedIndex));
		}
		void numSC_ValueChanged(object sender, EventArgs e)
		{
			if (TieMission.FlightGroups[activeFG].RandSpecCargo) { numSC.Value = 0; return; }
			if (numSC.Value == 0 || numSC.Value > TieMission.FlightGroups[activeFG].NumberOfCraft)
			{
				if (!_loading)
					TieMission.FlightGroups[activeFG].SpecialCargoCraft = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].SpecialCargoCraft, 0));
				txtSpecCargo.Visible = false;
				lblNotUsed.Visible = true;
			}
			else
			{
				if (!_loading)
					TieMission.FlightGroups[activeFG].SpecialCargoCraft = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].SpecialCargoCraft, numSC.Value));
				txtSpecCargo.Visible = true;
				lblNotUsed.Visible = false;
			}
		}

		void txtCargo_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Cargo = Common.Update(this, TieMission.FlightGroups[activeFG].Cargo, txtCargo.Text).ToString();
		}
		void txtName_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Name = Common.Update(this, TieMission.FlightGroups[activeFG].Name, txtName.Text).ToString();
			UpdateFGList();
		}
		void txtPilot_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Pilot = Common.Update(this, TieMission.FlightGroups[activeFG].Pilot, txtPilot.Text).ToString();
		}
		void txtSpecCargo_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].SpecialCargo = Common.Update(this, TieMission.FlightGroups[activeFG].SpecialCargo, txtSpecCargo.Text).ToString();
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
				activeArrDepTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { activeArrDepTrigger = Convert.ToByte(sender); l = lblADTrig[activeArrDepTrigger]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<3;i++) if (i != activeArrDepTrigger) lblADTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboADTrig.SelectedIndex = TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition;
			cboADTrigType.SelectedIndex = -1;
			cboADTrigType.SelectedIndex = TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType;
			cboADTrigAmount.SelectedIndex = TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount;
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
			TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition, cboADTrig.SelectedIndex));
			LabelRefresh(TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigAmount_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount, cboADTrigAmount.SelectedIndex));
			LabelRefresh(TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboADTrigType.SelectedIndex == -1) return;
			if (!_loading)
				TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType, cboADTrigType.SelectedIndex));
			ComboVarRefresh(TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType, cboADTrigVar);
			try { cboADTrigVar.SelectedIndex = TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable; }
			catch { cboADTrigVar.SelectedIndex = 0; TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable = 0; }
			LabelRefresh(TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigVar_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable, cboADTrigVar.SelectedIndex));
			LabelRefresh(TieMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboDiff_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Difficulty = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Difficulty, cboDiff.SelectedIndex));
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
			CraftStart(TieMission.FlightGroups[activeFG], false);
			TieMission.FlightGroups[activeFG].ArrivalDelayMinutes = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].ArrivalDelayMinutes, numArrMin.Value));
			TieMission.FlightGroups[activeFG].ArrivalDelaySeconds = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].ArrivalDelaySeconds, numArrSec.Value));
			CraftStart(TieMission.FlightGroups[activeFG], true);
			TieMission.FlightGroups[activeFG].ArrivalCraft1 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].ArrivalCraft1, cboArrMS.SelectedIndex));
			TieMission.FlightGroups[activeFG].ArrivalCraft2 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].ArrivalCraft2, cboArrMSAlt.SelectedIndex));
		}
		void grpDep_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].DepartureCraft1 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].DepartureCraft1, cboDepMS.SelectedIndex));
			TieMission.FlightGroups[activeFG].DepartureCraft2 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].DepartureCraft2, cboDepMSAlt.SelectedIndex));
			TieMission.FlightGroups[activeFG].AbortTrigger = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].AbortTrigger, cboAbort.SelectedIndex));
			TieMission.FlightGroups[activeFG].DepartureTimerMinutes = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].DepartureTimerMinutes, numDepMin.Value));
			TieMission.FlightGroups[activeFG].DepartureTimerSeconds = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].DepartureTimerSeconds, numDepSec.Value));
		}

		void optArrMS_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.FlightGroups[activeFG].ArrivalMethod1 = (bool)Common.Update(this, TieMission.FlightGroups[activeFG].ArrivalMethod1, optArrMS.Checked);
			cboArrMS.Enabled = optArrMS.Checked;
		}
		void optArrMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.FlightGroups[activeFG].ArrivalMethod2 = (bool)Common.Update(this, TieMission.FlightGroups[activeFG].ArrivalMethod2, optArrMSAlt.Checked);
			cboArrMSAlt.Enabled = optArrMSAlt.Checked;
		}
		void optDepMS_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.FlightGroups[activeFG].DepartureMethod1 = (bool)Common.Update(this, TieMission.FlightGroups[activeFG].DepartureMethod1, optDepMS.Checked);
			cboDepMS.Enabled = optDepMS.Checked;
		}
		void optDepMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
			TieMission.FlightGroups[activeFG].DepartureMethod2 = (bool)Common.Update(this, TieMission.FlightGroups[activeFG].DepartureMethod2, optDepMSAlt.Checked);
			cboDepMSAlt.Enabled = optDepMSAlt.Checked;
		}
		void optArrOR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.FlightGroups[activeFG].AT1AndOrAT2 = (bool)Common.Update(this, TieMission.FlightGroups[activeFG].AT1AndOrAT2, optArrOR.Checked);
		}
		#endregion
		#region Orders
		void OrderLabelRefresh()
		{
			string orderText = TieMission.FlightGroups[activeFG].Orders[activeOrder].ToString();
			orderText = replaceTargetText(orderText);
			lblOrder[activeOrder].Text = "Order " + (activeOrder + 1) + ": " + orderText;
		}

		void lblOrderArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				activeOrder = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { activeOrder = Convert.ToByte(sender); l = lblOrder[activeOrder]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<3;i++) if (i!=activeOrder) lblOrder[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboOrders.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Command;
			cboOThrottle.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Throttle;
			numOVar1.Value = TieMission.FlightGroups[activeFG].Orders[activeOrder].Variable1;
			numOVar2.Value = TieMission.FlightGroups[activeFG].Orders[activeOrder].Variable2;
			cboOT3Type.SelectedIndex = -1;
			cboOT3Type.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Target3Type;
			cboOT4Type.SelectedIndex = -1;
			cboOT4Type.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Target4Type;
			optOT3T4OR.Checked = TieMission.FlightGroups[activeFG].Orders[activeOrder].T3AndOrT4;
			optOT3T4AND.Checked = !optOT3T4OR.Checked;
			cboOT1Type.SelectedIndex = -1;
			cboOT1Type.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Target1Type;
			cboOT2Type.SelectedIndex = -1;
			cboOT2Type.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Target2Type;
			optOT1T2OR.Checked = TieMission.FlightGroups[activeFG].Orders[activeOrder].T1AndOrT2;
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
				TieMission.FlightGroups[activeFG].Orders[activeOrder].Command = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Command, cboOrders.SelectedIndex));
			OrderLabelRefresh();
			int i = Strings.OrderDesc[cboOrders.SelectedIndex].IndexOf("|");
			int j = Strings.OrderDesc[cboOrders.SelectedIndex].LastIndexOf("|");
			lblODesc.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(0, i);
			lblOVar1.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(i+1, j-i-1);
			lblOVar2.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(j+1);
		}

		void cboOT1_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Orders[activeOrder].Target1 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Target1, cboOT1.SelectedIndex));
		}
		void cboOT1Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT1Type.SelectedIndex == -1) return;
			if (!_loading) 
				TieMission.FlightGroups[activeFG].Orders[activeOrder].Target1Type = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Target1Type, cboOT1Type.SelectedIndex));
			ComboVarRefresh(TieMission.FlightGroups[activeFG].Orders[activeOrder].Target1Type, cboOT1);
			try { cboOT1.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Target1; }
			catch { cboOT1.SelectedIndex = 0; TieMission.FlightGroups[activeFG].Orders[activeOrder].Target1 = 0; }
			OrderLabelRefresh();
		}
		void cboOT2_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Orders[activeOrder].Target2 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Target2, cboOT2.SelectedIndex));
		}
		void cboOT2Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT2Type.SelectedIndex == -1) return;
			if (!_loading) 
				TieMission.FlightGroups[activeFG].Orders[activeOrder].Target2Type = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Target2Type, cboOT2Type.SelectedIndex));
			ComboVarRefresh(TieMission.FlightGroups[activeFG].Orders[activeOrder].Target2Type, cboOT2);
			try { cboOT2.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Target2; }
			catch { cboOT2.SelectedIndex = 0; TieMission.FlightGroups[activeFG].Orders[activeOrder].Target2 = 0; }
			OrderLabelRefresh();
		}
		void cboOT3_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Orders[activeOrder].Target3 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Target3, cboOT3.SelectedIndex));
		}
		void cboOT3Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT3Type.SelectedIndex == -1) return;
			if (!_loading) 
				TieMission.FlightGroups[activeFG].Orders[activeOrder].Target3Type = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Target3Type, cboOT3Type.SelectedIndex));
			ComboVarRefresh(TieMission.FlightGroups[activeFG].Orders[activeOrder].Target3Type, cboOT3);
			try { cboOT3.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Target3; }
			catch { cboOT3.SelectedIndex = 0; TieMission.FlightGroups[activeFG].Orders[activeOrder].Target3 = 0; }
			OrderLabelRefresh();
		}
		void cboOT4_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Orders[activeOrder].Target4 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Target4, cboOT4.SelectedIndex));
		}
		void cboOT4Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT4Type.SelectedIndex == -1) return;
			if (!_loading) 
				TieMission.FlightGroups[activeFG].Orders[activeOrder].Target4Type = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Target4Type, cboOT4Type.SelectedIndex));
			ComboVarRefresh(TieMission.FlightGroups[activeFG].Orders[activeOrder].Target4Type, cboOT4);
			try { cboOT4.SelectedIndex = TieMission.FlightGroups[activeFG].Orders[activeOrder].Target4; }
			catch { cboOT4.SelectedIndex = 0; TieMission.FlightGroups[activeFG].Orders[activeOrder].Target4 = 0; }
			OrderLabelRefresh();
		}
		void cboOThrottle_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Orders[activeOrder].Throttle = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Throttle, cboOThrottle.SelectedIndex));
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
			TieMission.FlightGroups[activeFG].Orders[activeOrder].Variable1 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Variable1, numOVar1.Value));
		}
		void numOVar2_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Orders[activeOrder].Variable2 = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].Variable2, numOVar2.Value));
		}

		void optOT1T2OR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.FlightGroups[activeFG].Orders[activeOrder].T1AndOrT2 = Convert.ToBoolean(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].T1AndOrT2, optOT1T2OR.Checked));
		}
		void optOT3T4OR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.FlightGroups[activeFG].Orders[activeOrder].T3AndOrT4 = Convert.ToBoolean(Common.Update(this, TieMission.FlightGroups[activeFG].Orders[activeOrder].T3AndOrT4, optOT3T4OR.Checked));
		}
		#endregion
		#region Goals
		void cboGoalArr_Leave(object sender, EventArgs e)
		{
			ComboBox c = (ComboBox)sender;
			TieMission.FlightGroups[activeFG].Goals[(int)c.Tag] = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Goals[(int)c.Tag], cboGoal[(int)c.Tag].SelectedIndex));
		}

		void numBonGoalP_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Goals.BonusPoints = (short)Common.Update(this, TieMission.FlightGroups[activeFG].Goals.BonusPoints, numBonGoalP.Value);
		}
		#endregion
		#region Waypoints
		void EnableRot(bool state)
		{
			numYaw.Enabled = state;
			numPitch.Enabled = state;
			numRoll.Enabled = state;
		}

		void chkWPArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			TieMission.FlightGroups[activeFG].Waypoints[(int)c.Tag].Enabled = (bool)Common.Update(this, TieMission.FlightGroups[activeFG].Waypoints[(int)c.Tag].Enabled, c.Checked);
		}

		void numPitch_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Pitch = (short)Common.Update(this, TieMission.FlightGroups[activeFG].Pitch, numPitch.Value);
		}
		void numRoll_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Roll = (short)Common.Update(this, TieMission.FlightGroups[activeFG].Roll, numRoll.Value);
		}
		void numYaw_Leave(object sender, EventArgs e)
		{
			TieMission.FlightGroups[activeFG].Yaw = (short)Common.Update(this, TieMission.FlightGroups[activeFG].Yaw, numYaw.Value);
		}

		void table_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i,j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<15;j++) if (table.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = (short)(Convert.ToDouble(table.Rows[j][i]) * 160);
					TieMission.FlightGroups[activeFG].Waypoints[j][i] = (short)Common.Update(this, TieMission.FlightGroups[activeFG].Waypoints[j][i], raw);
					tableRaw.Rows[j][i] = raw;
				}
			}
			catch { for (i=0;i<3;i++) table.Rows[j][i] = Math.Round((double)TieMission.FlightGroups[activeFG].Waypoints[j][i] / 160, 2); }
			_loading = false;
		}
		void tableRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i,j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<15;j++) if (tableRaw.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++) 
				{
					short raw = (short)tableRaw.Rows[j][i];
					TieMission.FlightGroups[activeFG].Waypoints[j][i] = (short)Common.Update(this, TieMission.FlightGroups[activeFG].Waypoints[j][i], raw);
					table.Rows[j][i] = Math.Round((double)raw / 160,2);
				}
			}
			catch { for (i=0;i<3;i++) tableRaw.Rows[j][i] = Convert.ToInt16(TieMission.FlightGroups[activeFG].Waypoints[j][i]); }
			_loading = false;
		}
		#endregion
		void numUnkArr_Leave(object sender, EventArgs e)
		{
			NumericUpDown n = (NumericUpDown)sender;
			TieMission.FlightGroups[activeFG].Unknowns[(int)n.Tag] = Convert.ToByte(Common.Update(this, TieMission.FlightGroups[activeFG].Unknowns[(int)n.Tag], n.Value));
		}
		#endregion
		#region Messages
		void DeleteMess()
		{
			activeMessage = TieMission.Messages.RemoveAt(activeMessage);
			if (TieMission.Messages.Count == 0)
			{
				lstMessages.Items.Clear();
				EnableMessages(false);
				lblMessage.Text = "Message #0 of 0";
				return;
			}
			lstMessages.Items.RemoveAt(activeMessage);
			lstMessages.SelectedIndex = activeMessage;
			Common.Title(this, _loading);
		}
		void EnableMessages(bool state)
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
		void MessListRefresh()
		{
			if (TieMission.Messages.Count == 0) return;
			string temp = TieMission.Messages[activeMessage].MessageString;
			lstMessages.Items.Insert(activeMessage, temp);
			lstMessages.Items.RemoveAt(activeMessage+1);
		}
		void NewMess()
		{
			if (TieMission.Messages.Count == Mission.MessageLimit)
			{
				MessageBox.Show("Mission contains maximum number of Messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			activeMessage = TieMission.Messages.Add();
			if (TieMission.Messages.Count == 1) EnableMessages(true);
			lstMessages.Items.Add(TieMission.Messages[activeMessage].MessageString);
			lstMessages.SelectedIndex = activeMessage;
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
			cboMessTrig.SelectedIndex = TieMission.Messages[activeMessage].Triggers[m].Condition;
			cboMessType.SelectedIndex = -1;
			cboMessType.SelectedIndex = TieMission.Messages[activeMessage].Triggers[m].VariableType;
			cboMessAmount.SelectedIndex = TieMission.Messages[activeMessage].Triggers[m].Amount;
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
			TieMission.Messages[activeMessage].Triggers[m].Amount = Convert.ToByte(Common.Update(this, TieMission.Messages[activeMessage].Triggers[m].Amount, cboMessAmount.SelectedIndex));
			LabelRefresh(TieMission.Messages[activeMessage].Triggers[m], (m==0 ? lblMess1 : lblMess2));
		}
		void cboMessColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading) 
				TieMission.Messages[activeMessage].Color = Convert.ToByte(Common.Update(this, TieMission.Messages[activeMessage].Color, cboMessColor.SelectedIndex));
			MessListRefresh();
		}
		void cboMessTrig_Leave(object sender, EventArgs e)
		{
			int m = (lblMess1.ForeColor == SystemColors.Highlight ? 0 : 1);
			TieMission.Messages[activeMessage].Triggers[m].Condition = Convert.ToByte(Common.Update(this, TieMission.Messages[activeMessage].Triggers[m].Condition, cboMessTrig.SelectedIndex));
			LabelRefresh(TieMission.Messages[activeMessage].Triggers[m], (m==0 ? lblMess1 : lblMess2));
		}
		void cboMessType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMessType.SelectedIndex == -1) return;
			int m = (lblMess1.ForeColor == SystemColors.Highlight ? 0 : 1);
			if (!_loading) 
				TieMission.Messages[activeMessage].Triggers[m].VariableType = Convert.ToByte(Common.Update(this, TieMission.Messages[activeMessage].Triggers[m].VariableType, cboMessType.SelectedIndex));
			ComboVarRefresh(TieMission.Messages[activeMessage].Triggers[m].VariableType, cboMessVar);
			try { cboMessVar.SelectedIndex = TieMission.Messages[activeMessage].Triggers[m].Variable; }
			catch { cboMessVar.SelectedIndex = 0; TieMission.Messages[activeMessage].Triggers[m].Variable = 0; }
			LabelRefresh(TieMission.Messages[activeMessage].Triggers[m], (m==0 ? lblMess1 : lblMess2));
		}
		void cboMessVar_Leave(object sender, EventArgs e)
		{
			int m = (lblMess1.ForeColor == SystemColors.Highlight ? 0 : 1);
			TieMission.Messages[activeMessage].Triggers[m].Variable = Convert.ToByte(Common.Update(this, TieMission.Messages[activeMessage].Triggers[m].Variable, cboMessVar.SelectedIndex));
			LabelRefresh(TieMission.Messages[activeMessage].Triggers[m], (m==0 ? lblMess1 : lblMess2));
		}

		void lstMessages_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (TieMission.Messages.Count == 0) return;
			if (TieMission.Messages[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch(TieMission.Messages[e.Index].Color)
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
			activeMessage = lstMessages.SelectedIndex;
			lblMessage.Text = "Message #" + (activeMessage+1) + " of " + TieMission.Messages.Count;
			bool btemp = _loading;
			_loading = true;
			LabelRefresh(TieMission.Messages[activeMessage].Triggers[0], lblMess1);
			LabelRefresh(TieMission.Messages[activeMessage].Triggers[1], lblMess2);
			txtMessage.Text = TieMission.Messages[activeMessage].MessageString;
			txtShort.Text = TieMission.Messages[activeMessage].Short;
			cboMessColor.SelectedIndex = TieMission.Messages[activeMessage].Color;
			numMessDelay.Value = TieMission.Messages[activeMessage].Delay * 5;
			optMessOR.Checked = TieMission.Messages[activeMessage].Trig1AndOrTrig2;
			optMessAND.Checked = !optMessOR.Checked;
			lblMessArr_Click(0, new System.EventArgs());
			_loading = btemp;
		}

		void numMessDelay_Leave(object sender, EventArgs e)
		{
			TieMission.Messages[activeMessage].Delay = Convert.ToByte(Common.Update(this, TieMission.Messages[activeMessage].Delay, numMessDelay.Value / 5));
		}

		void optMessOR_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				TieMission.Messages[activeMessage].Trig1AndOrTrig2 = (bool)Common.Update(this, TieMission.Messages[activeMessage].Trig1AndOrTrig2, optMessOR.Checked);
		}

		void txtMessage_Leave(object sender, EventArgs e)
		{
			TieMission.Messages[activeMessage].MessageString = Common.Update(this, TieMission.Messages[activeMessage].MessageString, txtMessage.Text).ToString();
			MessListRefresh();
		}
		void txtShort_Leave(object sender, EventArgs e)
		{
			TieMission.Messages[activeMessage].Short = Common.Update(this, TieMission.Messages[activeMessage].Short, txtShort.Text).ToString();
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
				activeGlobalGoal = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { activeGlobalGoal = Convert.ToByte(sender); l = lblGlob[activeGlobalGoal]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<6;i++) if (i!=activeGlobalGoal) lblGlob[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboGlobalTrig.SelectedIndex = TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Condition;
			cboGlobalType.SelectedIndex = -1;
			cboGlobalType.SelectedIndex = TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].VariableType;
			cboGlobalAmount.SelectedIndex = TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Amount;
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
			TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Amount = Convert.ToByte(Common.Update(this, TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Amount, cboGlobalAmount.SelectedIndex));
			LabelRefresh(TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2], lblGlob[activeGlobalGoal]);
		}
		void cboGlobalTrig_Leave(object sender, EventArgs e)
		{
			TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Condition = Convert.ToByte(Common.Update(this, TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Condition, cboGlobalTrig.SelectedIndex));
			LabelRefresh(TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2], lblGlob[activeGlobalGoal]);
		}
		void cboGlobalType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cboGlobalType.SelectedIndex == -1) return;
			if (!_loading)
				TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].VariableType = Convert.ToByte(Common.Update(this, TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].VariableType, cboGlobalType.SelectedIndex));
			ComboVarRefresh(TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].VariableType, cboGlobalVar);
			try { cboGlobalVar.SelectedIndex = TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Variable; }
			catch { cboGlobalVar.SelectedIndex = 0; TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Variable = 0; }
			LabelRefresh(TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2], lblGlob[activeGlobalGoal]);
		}
		void cboGlobalVar_Leave(object sender, EventArgs e)
		{
			TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Variable = Convert.ToByte(Common.Update(this, TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2].Variable, cboGlobalAmount.SelectedIndex));
			LabelRefresh(TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].Triggers[activeGlobalGoal % 2], lblGlob[activeGlobalGoal]);
		}

		void optBonOR_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
				TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].T1AndOrT2 = (bool)Common.Update(this, TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].T1AndOrT2, optBonOR.Checked);
		}
		void optPrimOR_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
				TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].T1AndOrT2 = (bool)Common.Update(this, TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].T1AndOrT2, optPrimOR.Checked);
		}
		void optSecOR_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
				TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].T1AndOrT2 = (bool)Common.Update(this, TieMission.GlobalGoals.Goals[activeGlobalGoal / 2].T1AndOrT2, optSecOR.Checked);
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
			TieMission.BriefingQuestions.PostTrigger[cboQuestion.SelectedIndex] = Convert.ToByte(Common.Update(this, TieMission.BriefingQuestions.PostTrigger[cboQuestion.SelectedIndex], cboQTrig.SelectedIndex));
		}
		void cboQTrigType_Leave(object sender, EventArgs e)
		{
			TieMission.BriefingQuestions.PostTrigType[cboQuestion.SelectedIndex] = Convert.ToByte(Common.Update(this, TieMission.BriefingQuestions.PostTrigType[cboQuestion.SelectedIndex], cboQTrigType.SelectedIndex));
		}
		void cboQuestion_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bool bTemp = _loading;
			_loading = true;
			int a = 0;	//place holder
			if (cboOfficer.SelectedIndex <= 1)	//if pre-miss
			{
				if (cboOfficer.SelectedIndex == 1) a = 5;	//if Secret Order, set place holder
				txtQuestion.Text = TieMission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + a];
				txtAnswer.Text = TieMission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + a];
			}
			else	//post-miss
			{
				cboQTrigType.SelectedIndex = TieMission.BriefingQuestions.PostTrigType[cboQuestion.SelectedIndex];
				cboQTrig.SelectedIndex = TieMission.BriefingQuestions.PostTrigger[cboQuestion.SelectedIndex];
				if (cboOfficer.SelectedIndex == 3) a = 5;
				txtQuestion.Text = TieMission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + a];
				txtAnswer.Text = TieMission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + a];
			}
			_loading = bTemp;
		}

		void optOfficers_Leave(object sender, EventArgs e)
		{
			RadioButton o = (RadioButton)sender;
			TieMission.OfficersPresent = (Mission.BriefingOfficers)Common.Update(this, TieMission.OfficersPresent, (Mission.BriefingOfficers)o.Tag);
		}

		void txtAnswer_Leave(object sender, EventArgs e)
		{
			string t = null;
			if (cboOfficer.SelectedIndex == 0) t = TieMission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex];
			else if (cboOfficer.SelectedIndex == 1) t = TieMission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + 5];
			else if (cboOfficer.SelectedIndex == 2) t = TieMission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex];
			else t = TieMission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + 5];
			t = Common.Update(this, t, txtAnswer.Text).ToString();
			if (cboOfficer.SelectedIndex == 0) TieMission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex] = t;
			else if (cboOfficer.SelectedIndex == 1) TieMission.BriefingQuestions.PreMissAnswers[cboQuestion.SelectedIndex + 5] = t;
			else if (cboOfficer.SelectedIndex == 2) TieMission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex] = t;
			else TieMission.BriefingQuestions.PostMissAnswers[cboQuestion.SelectedIndex + 5] = t;
		}
		void txtQuestion_Leave(object sender, EventArgs e)
		{
			string t = null;
			if (cboOfficer.SelectedIndex == 0) t = TieMission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex];
			else if (cboOfficer.SelectedIndex == 1) t = TieMission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + 5];
			else if (cboOfficer.SelectedIndex == 2) t = TieMission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex];
			else t = TieMission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + 5];
			t = Common.Update(this, t, txtQuestion.Text).ToString();
			if (cboOfficer.SelectedIndex == 0) TieMission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex] = t;
			else if (cboOfficer.SelectedIndex == 1) TieMission.BriefingQuestions.PreMissQuestions[cboQuestion.SelectedIndex + 5] = t;
			else if (cboOfficer.SelectedIndex == 2) TieMission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex] = t;
			else TieMission.BriefingQuestions.PostMissQuestions[cboQuestion.SelectedIndex + 5] = t;
		}
		#endregion
		#region Mission
		void chkIFFArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			TieMission.IffHostile[(int)c.Tag] = (bool)Common.Update(this, TieMission.IffHostile[(int)c.Tag], c.Checked);
		}
		void txtEoMArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			TieMission.EndOfMissionMessages[(int)t.Tag] = Common.Update(this, TieMission.EndOfMissionMessages[(int)t.Tag], t.Text).ToString();
		}
		void txtIFFArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			TieMission.IFFs[(int)t.Tag] = Common.Update(this, TieMission.IFFs[(int)t.Tag], t.Text).ToString();
			cboIFF.Items[(int)t.Tag] = t.Text;
		}

		void optCapture_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
				TieMission.CapturedOnEjection = (bool)Common.Update(this, TieMission.CapturedOnEjection, optCapture.Checked);
		}
		#endregion

		private void cmdPreview_Click(object sender, EventArgs e)
		{
			txtAnswer_Leave("cmdPreview", new EventArgs());
			txtQuestion_Leave("cmdPreview", new EventArgs());
			fOfficers = new frmOfficerPreview(TieMission.BriefingQuestions);
			fOfficers.Show();
		}
	}
}