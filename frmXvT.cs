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
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Idmr.Platform.Xvt;

namespace Idmr.Yogeme
{
	/// <summary>XvT Mission Editor GUI</summary>
	public partial class frmXvT : Form
	{
		#region vars and stuff
		Settings config = new Settings();
		BriefingCollection XvtBriefing;
		Mission XvtMission;
		bool bAppExit = false;
		int activeFG = 0;
		int startingShips = 1;
		bool _loading = false;
		int activeMessage = 0;
		DataTable table = new DataTable("Waypoints");
		DataTable tableRaw = new DataTable("Waypoints_Raw");
		frmMap fMap;
		frmBrief fBrief;
		frmLST fLST;
		byte activeMessageTrigger = 0;
		byte activeGlobalTrigger = 0;
		byte activeTeam = 0;
		byte activeArrDepTrigger = 0;
		byte activeFGGoal = 0;
		byte activeOrder = 0;
		byte activeOptionCraft = 0;
		#endregion
		#region Control Arrays
		RadioButton[] optADAndOr = new RadioButton[8];
		CheckBox[] chkSendTo = new CheckBox[10];
		Label[] lblMessTrig = new Label[4];
		Label[] lblGlobTrig = new Label[12];
		RadioButton[] optGlobAndOr = new RadioButton[18];
		Label[] lblTeam = new Label[10];
		CheckBox[] chkAllies = new CheckBox[10];
		Label[] lblADTrig = new Label[6];
		Label[] lblGoal = new Label[8];
		Label[] lblOrder = new Label[4];
		CheckBox[] chkWP = new CheckBox[22];
		CheckBox[] chkOpt = new CheckBox[15];
		Label[] lblOptCraft = new Label[10];
		ComboBox[] cboEoMColor = new ComboBox[6];
		TextBox[] txtEoM = new TextBox[6];
		#endregion

		public frmXvT(bool bBoP)
		{
			InitializeComponent();
			_loading = true;
			InitializeMission();
			SetBoP(bBoP);
			Startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		public frmXvT(string path)
		{
			InitializeComponent();
			_loading = true;
			InitializeMission();
			Startup();
			if(!LoadMission(path)) return;
			lstFG.SelectedIndex = 0;
			if (XvtMission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
			_loading = false;
		}

		void CloseForms()
		{
			try { fMap.Close(); }
			catch { /* do nothing */ }
			try { fBrief.Close(); }
			catch { /* do nothing */ }
			try { fLST.Close(); }
			catch { /* do nothing */ }
		}
		void ComboVarRefresh(int index, ComboBox cbo)
		{	//index is usually cboTrigType.SelectedIndex, cbo = cboTrigVar
			if (index == -1) return;
			cbo.Items.Clear();
			switch (index)		//switch (VariableType)
			{
				case 0:
					cbo.Items.Add("None");
					break;
				case 1: //FlightGroup
					cbo.Items.AddRange(XvtMission.FlightGroups.GetList());
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
					cbo.Items.AddRange(Strings.IFF);
					break;
				case 6:
					cbo.Items.AddRange(Strings.Orders);
					break;
				case 7:
					cbo.Items.AddRange(Strings.CraftWhen);
					break;
				//case 8: Global Group
				//since it's just numbers, same as default, left out for specifics
				case 12:	// Teams
					cbo.Items.AddRange(XvtMission.Teams.GetList());
					break;
				case 0x15:	// All Teams except
					cbo.Items.AddRange(XvtMission.Teams.GetList());
					break;
				// case 0x17: Global Unit
				default:
					string[] temp = new string[256];
					for (int i = 0;i <= 255;i++) temp[i] = i.ToString();
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
			lblStarting.Text = startingShips.ToString() + " craft at 30 seconds";
			if (startingShips > Mission.CraftLimit) lblStarting.ForeColor = Color.Red;
			else lblStarting.ForeColor = SystemColors.ControlText;
		}
		void InitializeMission()
		{
			XvtMission = new Mission();
			XvtBriefing = XvtMission.Briefings;
			config.LastMission = "";
			activeFG = 0;
			activeMessage = 0;
			XvtMission.FlightGroups[0].CraftType = Convert.ToByte(config.XvTCraft);
			XvtMission.FlightGroups[0].IFF = Convert.ToByte(config.XvTIFF);
			string[] fgList = XvtMission.FlightGroups.GetList();
			ComboReset(cboArrMS, fgList, 0);
			ComboReset(cboArrMSAlt, fgList, 0);
			ComboReset(cboDepMS, fgList, 0);
			ComboReset(cboDepMSAlt, fgList, 0);
			lstFG.Items.Clear();
			lstFG.Items.Add(XvtMission.FlightGroups[activeFG].ToString(true));
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			this.Text = "Ye Olde Galactic Empire Mission Editor - XvT - New Mission.tie";
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
				text = text.Replace("FG:" + fg, XvtMission.FlightGroups[fg].ToString());
			}
			while (text.Contains("TM:"))
			{
				int index = text.IndexOf("TM:") + 3;
				int length = text.IndexOfAny(new char[] { ' ', ',', '\0' }, index) - index;
				int team;
				if (length > 0) team = Int32.Parse(text.Substring(index, length));
				else team = Int32.Parse(text.Substring(index));
				text = text.Replace("TM:" + team, (XvtMission.Teams[team].Name == "" ? "Team " + team : XvtMission.Teams[team].Name));
			}
			return text;
		}
		bool LoadMission(string fileMission)
		{
			CloseForms();
			lstFG.Items.Clear();
			lstMessages.Items.Clear();
			startingShips = 0;
			byte[] buffer = new byte[64];
			try
			{
				FileStream fs = File.OpenRead(fileMission);
				try
				{
					#region determine platform
					switch (Platform.MissionFile.GetPlatform(fs))
					{
						case Platform.MissionFile.Platform.TIE:
							bAppExit = false;
							new frmTIE(fileMission).Show();
							Close();
							return false;
						case Platform.MissionFile.Platform.XvT:
							SetBoP(false);
							break;
						case Platform.MissionFile.Platform.BoP:
							SetBoP(true);
							break;
						case Platform.MissionFile.Platform.XWA:
							bAppExit = false;
							new frmXWA(fileMission).Show();
							Close();
							return false;
						default:
							throw new Exception("File is not a valid mission file for any platform, please select an appropriate *.tie file.");
					}
					#endregion
					XvtMission.LoadFromStream(fs);
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
			for (int i=0;i<XvtMission.FlightGroups.Count;i++)
			{
				lstFG.Items.Add(XvtMission.FlightGroups[activeFG].ToString(true));
				if (XvtMission.FlightGroups[i].ArrivesIn30Seconds) CraftStart(XvtMission.FlightGroups[i], true);
			}
			UpdateFGList();
			if (XvtMission.Messages.Count == 0) EnableMessages(false);
			else
			{
				EnableMessages(true);
				for (int i=0;i<XvtMission.Messages.Count;i++) lstMessages.Items.Add(XvtMission.Messages[i].MessageString);
			}
			UpdateMissionTabs();
			cboGlobalTeam.SelectedIndex = -1;	// otherwise it doesn't trigger an index change
			cboGlobalTeam.SelectedIndex = 0;
			for (activeTeam=0;activeTeam<10;activeTeam++) TeamRefresh();
			lblTeamArr_Click(0, new EventArgs());
			this.Text = "Ye Olde Galactic Empire Mission Editor - " + (XvtMission.IsBop ? "BoP" : "XvT") + " - " + XvtMission.MissionFileName;
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
					if (XvtMission.MissionPath == "\\NewMission.tie") savXvT.ShowDialog();
					else SaveMission(XvtMission.MissionPath);
				}
			}
		}
		void SaveMission(string fileMission)
		{
			try { fBrief.Save(); }
			catch { /* do nothing */ }
			lblTeamArr_Click(lblTeam[activeTeam], new EventArgs());
			XvtMission.Briefings = XvtBriefing;
			try { XvtMission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			this.Text = "Ye Olde Galactic Empire Mission Editor - " + (XvtMission.IsBop ? "BoP" : "XvT") + " - " + XvtMission.MissionFileName;
			config.LastMission = fileMission;
			//Verify the mission after it's been saved
			if (config.Verify) Common.RunVerify(XvtMission.MissionPath, config.VerifyLocation);
		}
		void SetBoP(bool bop)
		{
			XvtMission.IsBop = bop;
			if (XvtMission.IsBop)
			{
				menuNewXvT.Shortcut = Shortcut.None;
				menuNewBoP.Shortcut = Shortcut.CtrlN;
				config.LastPlatform = Settings.Platform.BoP;
				Text = Text.Substring(0, 41) + "BoP" + Text.Substring(44);
				txtMissDesc.MaxLength = 0x1000;
			}
			else
			{
				if (txtMissDesc.Text.Length > 0x400 || txtMissSucc.Text.Length != 0 || txtMissFail.Text.Length != 0)
				{
					DialogResult r = MessageBox.Show("Changing platforms will result in loss of Mission Description and Debriefing text", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					if (r == DialogResult.Cancel) { optBoP.Checked = true; return; }
					if (txtMissDesc.Text.Length > 0x400) txtMissDesc.Text = txtMissDesc.Text.Substring(0, 0x400);
				}
				menuNewXvT.Shortcut = Shortcut.CtrlN;
				menuNewBoP.Shortcut = Shortcut.None;
				config.LastPlatform = Settings.Platform.XvT;
				Text = Text.Substring(0, 41) + "XvT" + Text.Substring(44);
				txtMissDesc.MaxLength = 0x400;
				txtMissSucc.Text = "";
				txtMissFail.Text = "";
			}
			optBoP.Checked = XvtMission.IsBop;
			optXvT.Checked = !optBoP.Checked;
			txtMissSucc.Visible = XvtMission.IsBop;
			txtMissFail.Visible = XvtMission.IsBop;
			menuNewXvT.ShowShortcut = !XvtMission.IsBop;
			menuNewBoP.ShowShortcut = XvtMission.IsBop;
			menuSaveAsBoP.Visible = !XvtMission.IsBop;
			menuSaveAsXvT.Visible = XvtMission.IsBop;
		}
		void Startup()
		{
			this.Height = 600;	// since VS tends to slowly shrink the damn thing
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			config.LastMission = "";
			bAppExit = true;	//becomes false if selecting "New Mission" from menu
			if (config.RestrictPlatforms)
			{
				if (!config.TieInstalled) { menuNewTIE.Enabled = false; }
				if (!config.BopInstalled) { menuNewBoP.Enabled = false; }
				if (!config.XwaInstalled) { menuNewXWA.Enabled = false; }
			}
			if (Directory.Exists(config.XvTPath))
			{
				opnXvT.InitialDirectory = config.XvTPath;
				savXvT.InitialDirectory = config.XvTPath;
			}
			#region FlightGroups
			#region Craft
			cboCraft.Items.AddRange(Strings.CraftType); cboCraft.SelectedIndex = XvtMission.FlightGroups[0].CraftType;
			cboIFF.Items.AddRange(Strings.IFF); cboIFF.SelectedIndex = XvtMission.FlightGroups[0].IFF;
			cboTeam.Items.AddRange(XvtMission.Teams.GetList()); cboTeam.SelectedIndex = XvtMission.FlightGroups[0].Team;
			cboAI.Items.AddRange(Strings.Rating); cboAI.SelectedIndex = XvtMission.FlightGroups[0].AI;
			cboMarkings.Items.AddRange(Strings.Color); cboMarkings.SelectedIndex = XvtMission.FlightGroups[0].Markings;
			cboPlayer.SelectedIndex = 0;
			cboPosition.SelectedIndex = 0;
			cboFormation.Items.AddRange(Strings.Formation); cboFormation.SelectedIndex = 0;
			cboRadio.Items.AddRange(Strings.Radio); cboRadio.SelectedIndex = XvtMission.FlightGroups[0].Radio;
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
			string[] fgList = XvtMission.FlightGroups.GetList();
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
			for (int i=0;i<6;i++)
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
			lblOrderArr_Click(0, new EventArgs());
			#endregion
			#region Waypoints
			table.Columns.Add("X"); table.Columns.Add("Y"); table.Columns.Add("Z");
			tableRaw.Columns.Add("X"); tableRaw.Columns.Add("Y"); tableRaw.Columns.Add("Z");
			for (int i=0;i<22;i++)	//initialize WPs
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
			dataWaypoints_Raw.Table = tableRaw;
			dataWP.DataSource = dataWaypoints;
			dataWP_Raw.DataSource = dataWaypoints_Raw;
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
			chkWP[14] = chkWPBrief1;
			chkWP[15] = chkWPBrief2;
			chkWP[16] = chkWPBrief3;
			chkWP[17] = chkWPBrief4;
			chkWP[18] = chkWPBrief5;
			chkWP[19] = chkWPBrief6;
			chkWP[20] = chkWPBrief7;
			chkWP[21] = chkWPBrief8;
			for (int i=0;i<22;i++)
			{
				chkWP[i].Leave += new EventHandler(chkWPArr_Leave);
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
			lblGoalArr_Click(0, new EventArgs());
			#endregion
			#region Options
			cboOptCraft.Items.AddRange(Strings.CraftType);
			cboSkipAmount.Items.AddRange(Strings.Amount); cboSkipAmount.SelectedIndex = 0;
			cboSkipTrig.Items.AddRange(Strings.Trigger); cboSkipTrig.SelectedIndex = 0;
			cboSkipType.Items.AddRange(Strings.VariableType); cboSkipType.SelectedIndex = 0;
			cboRoleImp.Items.AddRange(Strings.Roles); cboRoleImp.SelectedIndex = 0;
			cboRoleReb.Items.AddRange(Strings.Roles); cboRoleReb.SelectedIndex = 0;
			cboRole3.Items.AddRange(Strings.Roles); cboRole3.SelectedIndex = 0;
			cboRole4.Items.AddRange(Strings.Roles); cboRole4.SelectedIndex = 0;
			cboRoleAll.Items.AddRange(Strings.Roles); cboRoleAll.SelectedIndex = 0;
			chkOpt[0] = chkOptWNone;
			chkOpt[1] = chkOptWRocket;
			chkOpt[2] = chkOptWBomb;
			chkOpt[3] = chkOptWMissile;
			chkOpt[4] = chkOptWTorp;
			chkOpt[5] = chkOptWAdvMissile;
			chkOpt[6] = chkOptWAdvTorp;
			chkOpt[7] = chkOptWMagPulse;
			chkOpt[8] = chkOptBNone;
			chkOpt[9] = chkOptBTractor;
			chkOpt[10] = chkOptBJamming;
			chkOpt[11] = chkOptBDecoy;
			chkOpt[12] = chkOptCNone;
			chkOpt[13] = chkOptCChaff;
			chkOpt[14] = chkOptCFlare;
			for (int i=0;i<15;i++)
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
			for (int i=0;i<4;i++)
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
			cboGlobalTeam.Items.AddRange(XvtMission.Teams.GetList());
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
			for (int i=0;i<9;i++)
			{
				optGlobAndOr[i].CheckedChanged += new EventHandler(optGlobAndOrArr_CheckedChanged);
				optGlobAndOr[i].Tag = i;
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
			for (int i=0;i<10;i++)
			{
				lblTeam[i].Click += new EventHandler(lblTeamArr_Click);
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
			for (int i=0;i<6;i++)
			{
				cboEoMColor[i].SelectedIndexChanged += new EventHandler(cboEoMColorArr_SelectedIndexChanged);
				cboEoMColor[i].Tag = i;
				txtEoM[i].Leave += new EventHandler(txtEoMArr_Leave);
				txtEoM[i].Tag = i;
			}
			for (activeTeam=0;activeTeam<10;activeTeam++) TeamRefresh();
			#endregion
			cboGlobalTeam.SelectedIndex = 0;
			//cboMissType.Items.AddRange(Strings.MissType);
			cboMissType.Items.AddRange(Enum.GetNames(typeof(Mission.MissionTypeEnum)));
			cboMissType.SelectedIndex = 0;
		}
		void UpdateMissionTabs()
		{
			numMissUnk1.Value = XvtMission.Unknown1;
			numMissUnk2.Value = XvtMission.Unknown2;
			chkMissUnk3.Checked = XvtMission.Unknown3;
			txtMissUnk4.Text = XvtMission.Unknown4;
			txtMissUnk5.Text = XvtMission.Unknown5;
			cboMissType.SelectedIndex = (int)XvtMission.MissionType;
			chkMissUnk6.Checked = XvtMission.Unknown6;
			numMissTimeMin.Value = XvtMission.TimeLimitMin;
			numMissTimeSec.Value = XvtMission.TimeLimitSec;
			txtMissDesc.Text = XvtMission.MissionDescription;
			txtMissSucc.Text = XvtMission.MissionSuccessful;
			txtMissFail.Text = XvtMission.MissionFailed;
		}

		void frmXvT_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

		void opnXvT_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_loading = true;
			if (LoadMission(opnXvT.FileName))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				lstFG.SelectedIndex = 0;
				try { lstMessages.SelectedIndex = 0; }
				catch { /* do nothing */ }
			}
			_loading = false;
		}

		void savXvT_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			XvtMission.MissionPath = savXvT.FileName;
			SaveMission(XvtMission.MissionPath);
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
			toolXvT.Focus();	// clicking the toolbar doesn't remove focus, so last change may not be saved
			switch (toolXvT.Buttons.IndexOf(e.Button))
			{
				case 0:		//New Mission
					menuNewXvT_Click("toolbar", new EventArgs());
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
					savXvT.ShowDialog();
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
			new dlgAbout().ShowDialog();
		}
		void menuBrief_Click(object sender, EventArgs e)
		{
			Common.Title(this, false);
			try { fBrief.Close(); }
			catch { /* do nothing */ }
			fBrief = new frmBrief(XvtMission.FlightGroups, ref XvtBriefing);
			fBrief.Show();
		}
		void menuCopy_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream = new FileStream("YOGEME.bin", FileMode.Create, FileAccess.Write, FileShare.None);
			#region ArrDep
			if (sender.ToString() == "AD")
			{
				formatter.Serialize(stream, XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger]);
				stream.Close();
				return;
			}
			#endregion
			#region FG Goal
			if (sender.ToString() == "Goal")
			{
				byte[] g = new byte[6];
				for (int i=0;i<6;i++) g[i] = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal][i];
				formatter.Serialize(stream, g);
				stream.Close();
				return;
			}
			#endregion
			#region Orders
			if (sender.ToString() == "Order")
			{
				formatter.Serialize(stream, XvtMission.FlightGroups[activeFG].Orders[activeOrder]);
				stream.Close();
				return;
			}
			#endregion
			#region Skip to O4
			if (sender.ToString() == "Skip")
			{
				formatter.Serialize(stream, XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[(lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1)]);
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
				formatter.Serialize(stream, XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger]);
				stream.Close();
				return;
			}
			#endregion

			switch (tabMain.SelectedIndex)
			{
				case 0:
					formatter.Serialize(stream, XvtMission.FlightGroups[activeFG]);
					break;
				case 1:
					if (XvtMission.Messages.Count != 0) formatter.Serialize(stream, XvtMission.Messages[activeMessage]);
					break;
				case 2:
					formatter.Serialize(stream, XvtMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4]);
					break;
				case 3:
					formatter.Serialize(stream, XvtMission.Teams[activeTeam]);
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
			try { fLST.Close(); }
			catch { /* do nothing */ }
			if (XvtMission.IsBop) fLST = new frmLST(Settings.Platform.BoP);
			else fLST = new frmLST(Settings.Platform.XvT);
			fLST.Show();
		}
		void menuMap_Click(object sender, EventArgs e)
		{
			try { fMap.Close(); }
			catch { /* do nothing */ }
			fMap = new frmMap(XvtMission.FlightGroups);
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
			bAppExit = false;
			new frmTIE().Show();
			Close();
		}
		void menuNewXvT_Click(object sender, EventArgs e)
		{
			PromptSave();
			CloseForms();
			_loading = true;
			InitializeMission();
			UpdateMissionTabs();
			lstMessages.Items.Clear();
			EnableMessages(false);
			lblMessage.Text = "Message #0 of 0";
			lstFG.SelectedIndex = 0;
			for (activeTeam=0;activeTeam<10;activeTeam++) TeamRefresh();
			lblTeamArr_Click(0, new EventArgs());
			SetBoP((sender.ToString() == "BoP"));
			if (this.Text.EndsWith("*")) this.Text = this.Text.Substring(0, this.Text.Length-1);
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
			opnXvT.ShowDialog();
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
					Mission.Trigger t = (Mission.Trigger)formatter.Deserialize(stream);
					XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger] = t;
					lblADTrigArr_Click(activeArrDepTrigger, new EventArgs());
					LabelRefresh(XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
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
					FlightGroup.Order o = (FlightGroup.Order)formatter.Deserialize(stream);
					XvtMission.FlightGroups[activeFG].Orders[activeOrder] = o;
					lblOrderArr_Click(activeOrder, new EventArgs());
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
					byte[] g = (byte[])formatter.Deserialize(stream);
					if (g.Length != 6) throw new Exception();
					for (int i=0;i<6;i++) XvtMission.FlightGroups[activeFG].Goals[activeFGGoal][i] = g[i];
					lblGoalArr_Click(activeFGGoal, new EventArgs());
					GoalLabelRefresh();
					Common.Title(this, false);
				}
				catch { /* do nothing */ }
				stream.Close();
				return;
			}
			#endregion
			#region Skip to O4
			if (sender.ToString() == "Skip")
			{
				try
				{
					Mission.Trigger t = (Mission.Trigger)formatter.Deserialize(stream);
					int j = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
					XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[j] = t;
					lblSkipTrigArr_Click(j, new EventArgs());
					LabelRefresh(XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[j], (j==0 ? lblSkipTrig1 : lblSkipTrig2));	// no array, hence explicit naming
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
					string s = formatter.Deserialize(stream).ToString();
					if (s.IndexOf("", 0) != -1) throw new Exception();		// bypass FGs, Mess, Teams, etc
					if (s.IndexOf("System.", 0) != -1) throw new Exception();	// bypass byte[]
					System.Windows.Forms.TextBox t = (System.Windows.Forms.TextBox)ActiveControl;
					t.SelectedText = s;
					stream.Close();
					Common.Title(this, false);
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
					Mission.Trigger t = (Mission.Trigger)formatter.Deserialize(stream);
					XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger]= t;
					lblMessTrigArr_Click(activeMessageTrigger, new EventArgs());
					LabelRefresh(XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
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
						FlightGroup fg = (FlightGroup)formatter.Deserialize(stream);
						if (fg == null) throw new Exception();
						NewFG();
						XvtMission.FlightGroups[activeFG] = fg;
						ListRefresh();
						startingShips--;
						lstFG.SelectedIndex = activeFG;
						CraftStart(fg, true);
					}
					catch { /* do nothing */ }
					break;
				case 1:
					try
					{
						Platform.Xvt.Message m = (Platform.Xvt.Message)formatter.Deserialize(stream);
						if (m == null) throw new Exception();
						NewMess();
						XvtMission.Messages[activeMessage] = m;
						MessListRefresh();
						lstMessages.SelectedIndex = activeMessage;
					}
					catch { /* do nothing */ }
					break;
				case 2:
					try
					{
						Mission.Trigger t = (Mission.Trigger)formatter.Deserialize(stream);
						XvtMission.Globals[activeTeam].Goals[activeGlobalTrigger/4].Triggers[activeGlobalTrigger%4] = t;
						lblGlobTrigArr_Click(activeGlobalTrigger, new EventArgs());
						Common.Title(this, false);
					}
					catch { /* do nothing */ }
					break;
				case 3:
					try
					{
						Team t = (Team)formatter.Deserialize(stream);
						if (t == null) throw new Exception();
						XvtMission.Teams[activeTeam] = t;
						TeamRefresh();
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
			if (XvtMission.MissionPath == "\\NewMission.tie") savXvT.ShowDialog();
			else SaveMission(XvtMission.MissionPath);
		}
		void menuSaveAsBoP_Click(object sender, EventArgs e)
		{
			SetBoP(true);
			savXvT.ShowDialog();
		}
		void menuSaveAsTIE_Click(object sender, EventArgs e)
		{
			PromptSave();
			try
			{
				Platform.Tie.Mission converted = Platform.Converter.XvtBopToTie(XvtMission);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXvT_Click(object sender, EventArgs e)
		{
			SetBoP(false);
			savXvT.ShowDialog();
		}
		void menuSaveAsXWA_Click(object sender, EventArgs e)
		{
			menuSave_Click("SaveAsXWA", new System.EventArgs());
			Common.RunConverter(XvtMission.MissionPath, 2);
		}
		void menuVerify_Click(object sender, EventArgs e)
		{
			menuSave_Click("Verify", new System.EventArgs());
			if (!config.Verify) Common.RunVerify(XvtMission.MissionPath, config.VerifyLocation);	//prevents from doing this twice due to Save
		}
		#endregion
		#region Flight Groups
		void DeleteFG()
		{
			if (XvtMission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(activeFG);
			CraftStart(XvtMission.FlightGroups[activeFG], false);
			if (XvtMission.FlightGroups.Count == 1)
			{
				XvtMission.FlightGroups.Clear();
				activeFG = 0;
				XvtMission.FlightGroups[0].CraftType = config.XvTCraft;
				XvtMission.FlightGroups[0].IFF = config.XvTIFF;
				CraftStart(XvtMission.FlightGroups[0], true);
			}
			else activeFG = XvtMission.FlightGroups.RemoveAt(activeFG);
			UpdateFGList();
			lstFG.SelectedIndex = activeFG;
			try
			{
				fMap.Import(XvtMission.FlightGroups);
				fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
			try
			{
				fBrief.Import(XvtMission.FlightGroups);
				fBrief.MapPaint();
			}
			catch { /* do nothing */ }
			Common.Title(this, _loading);
		}
		void ListRefresh()
		{
			lstFG.Items[activeFG] = XvtMission.FlightGroups[activeFG].ToString(true);
			lstFG.Items[activeFG] += " ";	// force refresh, otherwise solo IFF change wouldn't show
		}
		void NewFG()
		{
			if (XvtMission.FlightGroups.Count == Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			activeFG = XvtMission.FlightGroups.Add();
			XvtMission.FlightGroups[activeFG].CraftType = config.XvTCraft;
			XvtMission.FlightGroups[activeFG].IFF = config.XvTIFF;
			CraftStart(XvtMission.FlightGroups[activeFG], true);
			lstFG.Items.Add("");	// only need to create the item, ListRefresh() will fill it in
			UpdateFGList();
			lstFG.SelectedIndex = activeFG;
			_loading = false;
			try
			{
				fMap.Import(XvtMission.FlightGroups);
				fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
			try
			{
				fBrief.Import(XvtMission.FlightGroups);
				fBrief.MapPaint();
			}
			catch { /* do nothing */ }
			Common.Title(this, _loading);
		}
		void UpdateFGList()
		{
			string[] fgList = XvtMission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			ComboReset(cboArrMS, fgList, 0);
			ComboReset(cboArrMSAlt, fgList, 0);
			ComboReset(cboDepMS, fgList, 0);
			ComboReset(cboDepMSAlt, fgList, 0);
			_loading = temp;
			ListRefresh();
		}

		void lstFG_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1 || XvtMission.FlightGroups[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch(XvtMission.FlightGroups[e.Index].IFF)
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
			activeFG = lstFG.SelectedIndex;
			lblFG.Text = "Flight Group #" + (activeFG+1).ToString() + " of " + XvtMission.FlightGroups.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			#region Craft
			txtName.Text = XvtMission.FlightGroups[activeFG].Name;
			txtCargo.Text = XvtMission.FlightGroups[activeFG].Cargo;
			txtSpecCargo.Text = XvtMission.FlightGroups[activeFG].Cargo;
			numSC.Value = XvtMission.FlightGroups[activeFG].SpecialCargoCraft;
			chkRandSC.Checked = XvtMission.FlightGroups[activeFG].RandSpecCargo;
			cboCraft.SelectedIndex = XvtMission.FlightGroups[activeFG].CraftType;
			cboIFF.SelectedIndex = XvtMission.FlightGroups[activeFG].IFF;
			cboTeam.SelectedIndex = XvtMission.FlightGroups[activeFG].Team;
			try { cboAI.SelectedIndex = XvtMission.FlightGroups[activeFG].AI; }	// for some reason, some custom missions have this as -1
			catch { cboAI.SelectedIndex = 2; XvtMission.FlightGroups[activeFG].AI = 2; }
			cboMarkings.SelectedIndex = XvtMission.FlightGroups[activeFG].Markings;
			cboPlayer.SelectedIndex = XvtMission.FlightGroups[activeFG].PlayerNumber;
			cboPosition.SelectedIndex = XvtMission.FlightGroups[activeFG].PlayerCraft;
			cboFormation.SelectedIndex = XvtMission.FlightGroups[activeFG].Formation;
			cboRadio.SelectedIndex = XvtMission.FlightGroups[activeFG].Radio;
			numLead.Value = XvtMission.FlightGroups[activeFG].FormLeaderDist;
			numSpacing.Value = XvtMission.FlightGroups[activeFG].FormDistance;
			numWaves.Value = XvtMission.FlightGroups[activeFG].NumberOfWaves;
			numCraft.Value = XvtMission.FlightGroups[activeFG].NumberOfCraft;
			numGG.Value = XvtMission.FlightGroups[activeFG].GlobalGroup;
			numGU.Value = XvtMission.FlightGroups[activeFG].GlobalUnit;
			cboStatus.SelectedIndex = XvtMission.FlightGroups[activeFG].Status1;
			cboStatus2.SelectedIndex = XvtMission.FlightGroups[activeFG].Status2;
			cboWarheads.SelectedIndex = XvtMission.FlightGroups[activeFG].Missile;
			cboBeam.SelectedIndex = XvtMission.FlightGroups[activeFG].Beam;
			cboCounter.SelectedIndex = XvtMission.FlightGroups[activeFG].Countermeasures;
			numExplode.Value = XvtMission.FlightGroups[activeFG].ExplosionTime;
			#endregion
			#region Arr/Dep
			optArrMS.Checked = XvtMission.FlightGroups[activeFG].ArrivalMethod1;
			optArrHyp.Checked = !optArrMS.Checked;
			try { cboArrMS.SelectedIndex = XvtMission.FlightGroups[activeFG].ArrivalCraft1; }
			catch { cboArrMS.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].ArrivalCraft1 = 0; optArrHyp.Checked = true; }
			optArrMSAlt.Checked = XvtMission.FlightGroups[activeFG].ArrivalMethod2;
			optArrHypAlt.Checked = !optArrMSAlt.Checked;
			try { cboArrMSAlt.SelectedIndex = XvtMission.FlightGroups[activeFG].ArrivalCraft2; }
			catch { cboArrMSAlt.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].ArrivalCraft2 = 0; optArrHypAlt.Checked = true; }
			optDepMS.Checked = XvtMission.FlightGroups[activeFG].DepartureMethod1;
			optDepHyp.Checked = !optDepMS.Checked;
			try { cboDepMS.SelectedIndex = XvtMission.FlightGroups[activeFG].DepartureCraft1; }
			catch { cboDepMS.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].DepartureCraft1 = 0; optDepHyp.Checked = true; }
			optDepMSAlt.Checked = XvtMission.FlightGroups[activeFG].DepartureMethod2;
			optDepHypAlt.Checked = !optDepMSAlt.Checked;
			try { cboDepMSAlt.SelectedIndex = XvtMission.FlightGroups[activeFG].DepartureCraft2; }
			catch { cboDepMSAlt.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].DepartureCraft2 = 0; optDepHypAlt.Checked = true; }
			for (int i=0;i<4;i++)
			{
				optADAndOr[i].Checked = XvtMission.FlightGroups[activeFG].ArrDepAO[i];
				optADAndOr[i+4].Checked = !optADAndOr[i].Checked;
			}
			numArrMin.Value = XvtMission.FlightGroups[activeFG].ArrivalDelayMinutes;
			numArrSec.Value = XvtMission.FlightGroups[activeFG].ArrivalDelaySeconds;
			numDepMin.Value = XvtMission.FlightGroups[activeFG].DepartureTimerMinutes;
			numDepSec.Value = XvtMission.FlightGroups[activeFG].DepartureTimerSeconds;
			cboAbort.SelectedIndex = XvtMission.FlightGroups[activeFG].AbortTrigger;
			cboDiff.SelectedIndex = XvtMission.FlightGroups[activeFG].Difficulty;
			chkArrHuman.Checked = XvtMission.FlightGroups[activeFG].ArriveOnlyIfHuman;
			for (int i=0;i<6;i++) LabelRefresh(XvtMission.FlightGroups[activeFG].ArrDepTriggers[i], lblADTrig[i]);
			lblADTrigArr_Click(0, new EventArgs());
			#endregion
			for (activeFGGoal=0;activeFGGoal<8;activeFGGoal++) GoalLabelRefresh();
			lblGoalArr_Click(0, new EventArgs());
			#region Waypoints
			for (int i=0;i<22;i++)
			{
				for (int j=0;j<3;j++)
				{
					tableRaw.Rows[i][j] = XvtMission.FlightGroups[activeFG].Waypoints[i][j];
					table.Rows[i][j] = Math.Round((double)XvtMission.FlightGroups[activeFG].Waypoints[i][j] / 160, 2);
				}
				chkWP[i].Checked = XvtMission.FlightGroups[activeFG].Waypoints[i].Enabled;
			}
			tableRaw.AcceptChanges();
			table.AcceptChanges();
			numYaw.Value = XvtMission.FlightGroups[activeFG].Yaw;
			numPitch.Value = XvtMission.FlightGroups[activeFG].Pitch;
			numRoll.Value = XvtMission.FlightGroups[activeFG].Roll;
			if (XvtMission.FlightGroups[activeFG].CraftType <= 0x45) EnableRot(false);
			else EnableRot(true);
			#endregion
			for (activeOrder=0;activeOrder<4;activeOrder++) OrderLabelRefresh();
			lblOrderArr_Click(0, new EventArgs());
			#region Options
			cboRoleImp.SelectedIndex = 0;
			cboRoleReb.SelectedIndex = 0;
			cboRole3.SelectedIndex = 0;
			cboRole4.SelectedIndex = 0;
			cboRoleAll.SelectedIndex = 0;
			string[] str_role = new string[Strings.Roles.Length];
			for (int i=0;i<str_role.Length;i++) str_role[i] = Strings.Roles[i].Substring(0, 3).ToUpper();
			for (int i=0;i<4;i++)
			{
				try
				{
					string str_abbrv = XvtMission.FlightGroups[activeFG].Roles[i].Substring(1);
					int j;
					for (j=(str_role.Length-1);j>0;j--) if (str_abbrv == str_role[j]) break;	// reversed to default to zero
					switch (XvtMission.FlightGroups[activeFG].Roles[i].Substring(0, 1))
					{
						case "1":
							cboRoleImp.SelectedIndex = j;
							break;
						case "2":
							cboRoleReb.SelectedIndex = j;
							break;
						case "3":
							cboRole3.SelectedIndex = j;
							break;
						case "4":
							cboRole4.SelectedIndex = j;
							break;
						case "a":
							cboRoleAll.SelectedIndex = j;
							break;
						default:
							break;
					}
				}
				catch { /* do nothing */ }
			}
			for (int i=0;i<15;i++) chkOpt[i].Checked = XvtMission.FlightGroups[activeFG].OptLoadout[i];
			lblSkipTrigArr_Click(0, new EventArgs());
			for(activeOptionCraft=0;activeOptionCraft<10;activeOptionCraft++) OptCraftLabelRefresh();
			lblOptCraftArr_Click(0, new EventArgs());
			cboOptCat.SelectedIndex = (int)XvtMission.FlightGroups[activeFG].OptCraftCategory;
			#endregion
			#region Unknowns
			numUnk1.Value = XvtMission.FlightGroups[activeFG].Unknowns.Unknown1;
			chkUnk2.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown2;
			numUnk3.Value = XvtMission.FlightGroups[activeFG].Unknowns.Unknown3;
			numUnk4.Value = XvtMission.FlightGroups[activeFG].Unknowns.Unknown4;
			numUnk5.Value = XvtMission.FlightGroups[activeFG].Unknowns.Unknown5;
			numUnkOrder.Value = 1;
			numUnkGoal.Value = 1;
			chkUnk17.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown17;
			chkUnk18.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown18;
			chkUnk19.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown19;
			numUnk20.Value = XvtMission.FlightGroups[activeFG].Unknowns.Unknown20;
			numUnk21.Value = XvtMission.FlightGroups[activeFG].Unknowns.Unknown21;
			chkUnk22.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown22;
			chkUnk23.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown23;
			chkUnk24.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown24;
			chkUnk25.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown25;
			chkUnk26.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown26;
			chkUnk27.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown27;
			chkUnk28.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown28;
			chkUnk29.Checked = XvtMission.FlightGroups[activeFG].Unknowns.Unknown29;
			#endregion
			_loading = btemp;
		}

		#region Craft
		void EnableBackdrop(bool state)
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

		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnableBackdrop((cboCraft.SelectedIndex == 0x57 ? true : false));
			if (_loading) return;
			XvtMission.FlightGroups[activeFG].CraftType = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].CraftType, cboCraft.SelectedIndex));
			UpdateFGList();
		}
		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XvtMission.FlightGroups[activeFG].Formation = (byte)cboFormation.SelectedIndex;
			Common.Title(this, false);
		}
		void cboStatus_Leave(object sender, EventArgs e)
		{
			numBackdrop.Value = cboStatus.SelectedIndex;
			XvtMission.FlightGroups[activeFG].Status1 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Status1, cboStatus.SelectedIndex));
		}

		void chkRandSC_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XvtMission.FlightGroups[activeFG].RandSpecCargo = chkRandSC.Checked;
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
				dlgBackdrop dlg = new dlgBackdrop((XvtMission.IsBop ? Platform.MissionFile.Platform.BoP : Idmr.Platform.MissionFile.Platform.XvT), XvtMission.FlightGroups[activeFG].Status1);
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
			dlgFormation dlg = new dlgFormation(XvtMission.FlightGroups[activeFG].Formation);
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				Common.Update(this, cboFormation.SelectedIndex, dlg.Formation);
				cboFormation.SelectedIndex = dlg.Formation;
			}
		}

		void grpCraft2_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].IFF = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].IFF, cboIFF.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Team = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Team, cboTeam.SelectedIndex));
			XvtMission.FlightGroups[activeFG].AI = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].AI, cboAI.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Markings = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Markings, cboMarkings.SelectedIndex));
			XvtMission.FlightGroups[activeFG].PlayerNumber = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].PlayerNumber, cboPlayer.SelectedIndex));
			XvtMission.FlightGroups[activeFG].PlayerCraft = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].PlayerCraft, cboPosition.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Formation = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Formation, cboFormation.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Radio = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Radio, cboRadio.SelectedIndex));
			XvtMission.FlightGroups[activeFG].FormDistance = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].FormDistance, numSpacing.Value));
			XvtMission.FlightGroups[activeFG].FormLeaderDist = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].FormLeaderDist, numLead.Value));
			ListRefresh();
		}
		void grpCraft3_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].NumberOfWaves = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].NumberOfWaves, numWaves.Value));
			CraftStart(XvtMission.FlightGroups[activeFG], false);
			XvtMission.FlightGroups[activeFG].NumberOfCraft = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].NumberOfCraft, numCraft.Value));
			CraftStart(XvtMission.FlightGroups[activeFG], true);
			if (XvtMission.FlightGroups[activeFG].SpecialCargoCraft > XvtMission.FlightGroups[activeFG].NumberOfCraft) numSC.Value = 0;
			XvtMission.FlightGroups[activeFG].GlobalGroup = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].GlobalGroup, numGG.Value));
			XvtMission.FlightGroups[activeFG].GlobalUnit = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].GlobalUnit, numGU.Value));
			ListRefresh();
		}
		void grpCraft4_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Status2 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Status2, cboStatus2.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Missile = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Missile, cboWarheads.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Beam = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Beam, cboBeam.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Countermeasures = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Countermeasures, cboCounter.SelectedIndex));
			//XvtMission.FlightGroups[FG].ExplosionTime = Convert.ToByte(Common.Title(this, false, XvtMission.FlightGroups[activeFG].ExplosionTime, numExplode.Value));
		}

		void numBackdrop_Leave(object sender, EventArgs e)
		{
			cboStatus.SelectedIndex = (int)numBackdrop.Value;
			XvtMission.FlightGroups[activeFG].Status1 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Status1, cboStatus.SelectedIndex));
		}
		void numSC_ValueChanged(object sender, EventArgs e)
		{
			Common.Title(this, _loading);
			if (XvtMission.FlightGroups[activeFG].RandSpecCargo) { numSC.Value = 0; return;  }
			if (numSC.Value == 0 || numSC.Value > XvtMission.FlightGroups[activeFG].NumberOfCraft)
			{
				XvtMission.FlightGroups[activeFG].SpecialCargoCraft = 0;
				txtSpecCargo.Visible = false;
				lblNotUsed.Visible = true;
			}
			else
			{
				XvtMission.FlightGroups[activeFG].SpecialCargoCraft = (byte)numSC.Value;
				txtSpecCargo.Visible = true;
				lblNotUsed.Visible = false;
			}
		}

		void txtCargo_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Cargo = Common.Update(this, XvtMission.FlightGroups[activeFG].Cargo, txtCargo.Text).ToString();
		}
		void txtName_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Name = Common.Update(this, XvtMission.FlightGroups[activeFG].Name, txtName.Text).ToString();
			UpdateFGList();
		}
		void txtSpecCargo_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].SpecialCargo = Common.Update(this, XvtMission.FlightGroups[activeFG].SpecialCargo, txtSpecCargo.Text).ToString();
		}
		#endregion
		#region Arr/Dep
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
			for (int i=0;i<6;i++) if (i != activeArrDepTrigger) lblADTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboADTrig.SelectedIndex = XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition;
			cboADTrigType.SelectedIndex = -1;
			cboADTrigType.SelectedIndex = XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType;
			cboADTrigAmount.SelectedIndex = XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount;
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
			XvtMission.FlightGroups[activeFG].ArrDepAO[(int)r.Tag] = Convert.ToBoolean(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrDepAO[(int)r.Tag], r.Checked));
		}

		void cboADTrig_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition, cboADTrig.SelectedIndex));
			LabelRefresh(XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigAmount_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount, cboADTrigAmount.SelectedIndex));
			LabelRefresh(XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboADTrigType.SelectedIndex == -1) return;
			if (!_loading)
				XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType, cboADTrigType.SelectedIndex));
			ComboVarRefresh(XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType, cboADTrigVar);
			try { cboADTrigVar.SelectedIndex = XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable; }
			catch { cboADTrigVar.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable = 0; }
			LabelRefresh(XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigVar_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable, cboADTrigVar.SelectedIndex));
			LabelRefresh(XvtMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboArrMS_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].ArrivalCraft1 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrivalCraft1, cboArrMS.SelectedIndex));
		}
		void cboArrMSAlt_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].ArrivalCraft2 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrivalCraft2, cboArrMSAlt.SelectedIndex));
		}
		void cboDiff_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Difficulty = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Difficulty, cboDiff.SelectedIndex));
		}

		void chkArrHuman_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].ArriveOnlyIfHuman = Convert.ToBoolean(Common.Update(this, XvtMission.FlightGroups[activeFG].ArriveOnlyIfHuman, chkArrHuman.Checked));
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
			XvtMission.FlightGroups[activeFG].DepartureCraft1 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].DepartureCraft1, cboDepMS.SelectedIndex));
			XvtMission.FlightGroups[activeFG].DepartureCraft2 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].DepartureCraft2, cboDepMSAlt.SelectedIndex));
			XvtMission.FlightGroups[activeFG].AbortTrigger = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].AbortTrigger, cboAbort.SelectedIndex));
			XvtMission.FlightGroups[activeFG].DepartureTimerMinutes = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].DepartureTimerMinutes, numDepMin.Value));
			XvtMission.FlightGroups[activeFG].DepartureTimerSeconds = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].DepartureTimerSeconds, numDepSec.Value));
		}

		void numArrMin_Leave(object sender, EventArgs e)
		{
			CraftStart(XvtMission.FlightGroups[activeFG], false);
			XvtMission.FlightGroups[activeFG].ArrivalDelayMinutes = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrivalDelayMinutes, numArrMin.Value));
			CraftStart(XvtMission.FlightGroups[activeFG], true);
		}
		void numArrSec_Leave(object sender, EventArgs e)
		{
			CraftStart(XvtMission.FlightGroups[activeFG], false);
			XvtMission.FlightGroups[activeFG].ArrivalDelaySeconds = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrivalDelaySeconds, numArrSec.Value));
			CraftStart(XvtMission.FlightGroups[activeFG], true);
		}

		void optArrMS_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMS.Enabled = optArrMS.Checked;
			if (!_loading) XvtMission.FlightGroups[activeFG].ArrivalMethod1 = Convert.ToBoolean(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrivalMethod1, optArrMS.Checked));
		}
		void optArrMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMSAlt.Enabled = optArrMSAlt.Checked;
			if (!_loading) XvtMission.FlightGroups[activeFG].ArrivalMethod2 = Convert.ToBoolean(Common.Update(this, XvtMission.FlightGroups[activeFG].ArrivalMethod2, optArrMSAlt.Checked));
		}
		void optDepMS_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMS.Enabled = optDepMS.Checked;
			if (!_loading) XvtMission.FlightGroups[activeFG].DepartureMethod1 = Convert.ToBoolean(Common.Update(this, XvtMission.FlightGroups[activeFG].DepartureMethod1, optDepMS.Checked));
		}
		void optDepMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMSAlt.Enabled = optDepMSAlt.Checked;
			if (!_loading) XvtMission.FlightGroups[activeFG].DepartureMethod2 = Convert.ToBoolean(Common.Update(this, XvtMission.FlightGroups[activeFG].DepartureMethod2, optDepMSAlt.Checked));
		}
		#endregion
		#region Orders
		void OrderLabelRefresh()
		{
			string orderText = XvtMission.FlightGroups[activeFG].Orders[activeOrder].ToString();
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
			for (int i=0;i<4;i++) if (i!=activeOrder) lblOrder[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboOrders.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder][0];
			cboOThrottle.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Throttle;
			numOVar1.Value = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Variable1;
			numOVar2.Value = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Variable2;
			cboOT3Type.SelectedIndex = -1;
			cboOT3Type.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target3Type;
			cboOT4Type.SelectedIndex = -1;
			cboOT4Type.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target4Type;
			optOT3T4OR.Checked = Convert.ToBoolean(XvtMission.FlightGroups[activeFG].Orders[activeOrder].T3AndOrT4);
			optOT3T4AND.Checked = !optOT3T4OR.Checked;
			cboOT1Type.SelectedIndex = -1;
			cboOT1Type.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target1Type;
			cboOT2Type.SelectedIndex = -1;
			cboOT2Type.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target2Type;
			optOT1T2OR.Checked = Convert.ToBoolean(XvtMission.FlightGroups[activeFG].Orders[activeOrder].T1AndOrT2);
			optOT1T2AND.Checked = !optOT1T2OR.Checked;
			numOSpeed.Value = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Speed;
			txtOString.Text = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Designation;
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
				XvtMission.FlightGroups[activeFG].Orders[activeOrder].Command = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Command, cboOrders.SelectedIndex));
			OrderLabelRefresh();
			int i = Strings.OrderDesc[cboOrders.SelectedIndex].IndexOf("|");
			int j = Strings.OrderDesc[cboOrders.SelectedIndex].LastIndexOf("|");
			lblODesc.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(0, i);
			lblOVar1.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(i+1, j-i-1);
			lblOVar2.Text = Strings.OrderDesc[cboOrders.SelectedIndex].Substring(j+1);
		}
		void cboOT1_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target1 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target1, cboOT1.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT1Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT1Type.SelectedIndex == -1) return;
			if (!_loading)
				XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target1Type = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target1Type, cboOT1Type.SelectedIndex));
			ComboVarRefresh(XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target1Type, cboOT1);
			try { cboOT1.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target1; }
			catch { cboOT1.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target1 = 0; }
			OrderLabelRefresh();
		}
		void cboOT2_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target2 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target2, cboOT2.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT2Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT2Type.SelectedIndex == -1) return;
			if (!_loading)
				XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target2Type = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target2Type, cboOT2Type.SelectedIndex));
			ComboVarRefresh(XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target2Type, cboOT2);
			try { cboOT2.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target2; }
			catch { cboOT2.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target2 = 0; }
			OrderLabelRefresh();
		}
		void cboOT3_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target3 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target3, cboOT3.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT3Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT3Type.SelectedIndex == -1) return;
			if (!_loading)
				XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target3Type = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target3Type, cboOT3Type.SelectedIndex));
			ComboVarRefresh(XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target3Type, cboOT3);
			try { cboOT3.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target3; }
			catch { cboOT3.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target3 = 0; }
			OrderLabelRefresh();
		}
		void cboOT4_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target4 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target4, cboOT4.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT4Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT4Type.SelectedIndex == -1) return;
			if (!_loading)
				XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target4Type = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target4Type, cboOT4Type.SelectedIndex));
			ComboVarRefresh(XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target4Type, cboOT4);
			try { cboOT4.SelectedIndex = XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target4; }
			catch { cboOT4.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].Orders[activeOrder].Target4 = 0; }
			OrderLabelRefresh();
		}
		void cboOThrottle_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Throttle = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Throttle, cboOThrottle.SelectedIndex));
		}

		void cmdCopyOrder_Click(object sender, EventArgs e)
		{
			menuCopy_Click("Order", new EventArgs());
		}
		void cmdPasteOrder_Click(object sender, EventArgs e)
		{
			menuPaste_Click("Order", new EventArgs());
		}

		void numOSpeed_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Speed = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Speed, numOSpeed.Value));
		}
		void numOVar1_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Variable1 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Variable1, numOVar1.Value));
		}
		void numOVar2_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Variable2 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Variable2, numOVar2.Value));
		}

		void optOT1T2OR_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].T1AndOrT2 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].T1AndOrT2, Convert.ToByte(optOT1T2OR.Checked));
			OrderLabelRefresh();
		}
		void optOT3T4OR_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].T3AndOrT4 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].T3AndOrT4, Convert.ToByte(optOT3T4OR.Checked));
			OrderLabelRefresh();
		}

		void txtOString_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Orders[activeOrder].Designation = Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[activeOrder].Designation, txtOString.Text).ToString();
		}
		#endregion
		#region Goals
		void GoalLabelRefresh()
		{
			lblGoal[activeFGGoal].Text = "Goal " + (activeFGGoal+1).ToString() + ": " + XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].ToString();
		}
		
		void lblGoalArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				activeFGGoal = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { activeFGGoal = Convert.ToByte(sender); l = lblGoal[activeFGGoal]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<8;i++) if (i!=activeFGGoal) lblGoal[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboGoalArgument.SelectedIndex = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Argument;
			cboGoalTrigger.SelectedIndex = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Condition;
			cboGoalAmount.SelectedIndex = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Amount;
			numGoalPoints.Value = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Points;
			chkGoalEnable.Checked = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Enabled;
			numGoalTeam.Value = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Team+1;
			txtGoalInc.Text = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].IncompleteText;
			txtGoalComp.Text = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].CompleteText;
			txtGoalFail.Text = XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].FailedText;
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

		void chkGoalEnable_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Enabled = Convert.ToBoolean(Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Enabled, chkGoalEnable.Checked));
			GoalLabelRefresh();
		}

		void grpGoal_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Argument = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Argument, cboGoalArgument.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Condition = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Condition, cboGoalTrigger.SelectedIndex));
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Amount = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Amount, cboGoalAmount.SelectedIndex));
			GoalLabelRefresh();
		}

		void numGoalPoints_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Points = (short)Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Points, numGoalPoints.Value);
			GoalLabelRefresh();
		}
		void numGoalTeam_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Team = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].Team, (numGoalTeam.Value - 1)));
		}

		void txtGoalComp_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].IncompleteText = Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].IncompleteText, txtGoalComp.Text).ToString();
		}
		void txtGoalFail_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].CompleteText = Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].CompleteText, txtGoalFail.Text).ToString();
		}
		void txtGoalInc_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].FailedText = Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[activeFGGoal].FailedText, txtGoalInc.Text).ToString();
		}
		#endregion
		#region Waypoints
		void EnableRot(bool state)
		{
			numYaw.Enabled = state;
			numRoll.Enabled = state;
		}

		void chkWPArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			XvtMission.FlightGroups[activeFG].Waypoints[(int)c.Tag].Enabled = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Waypoints[(int)c.Tag].Enabled, c.Checked);
		}

		void table_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<22;j++) if (table.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = (short)(Convert.ToDouble(table.Rows[j][i]) * 160);
					XvtMission.FlightGroups[activeFG].Waypoints[j][i] = (short)Common.Update(this, XvtMission.FlightGroups[activeFG].Waypoints[j][i], raw);
					tableRaw.Rows[j][i] = raw;
				}
			}
			catch { for (i=0;i<3;i++) table.Rows[j][i] = Math.Round((double)(XvtMission.FlightGroups[activeFG].Waypoints[j][i]) / 160, 2); }	// reset
			_loading = false;
		}
		void tableRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<22;j++) if (tableRaw.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = Convert.ToInt16(tableRaw.Rows[j][i]);
					XvtMission.FlightGroups[activeFG].Waypoints[j][i] = (short)Common.Update(this, XvtMission.FlightGroups[activeFG].Waypoints[j][i], raw);
					table.Rows[j][i] = Math.Round((double)raw / 160, 2);
				}
			}
			catch { for (i=0;i<3;i++) tableRaw.Rows[j][i] = XvtMission.FlightGroups[activeFG].Waypoints[j][i]; }
			_loading = false;
		}

		void numPitch_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Pitch = (short)Common.Update(this, XvtMission.FlightGroups[activeFG].Pitch, (short)numPitch.Value);
		}
		void numRoll_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Roll = (short)Common.Update(this, XvtMission.FlightGroups[activeFG].Roll, (short)numRoll.Value);
		}
		void numYaw_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Yaw = (short)Common.Update(this, XvtMission.FlightGroups[activeFG].Yaw, (short)numYaw.Value);
		}
		#endregion
		#region Options
		void EnableOptCat(bool state)
		{
			numOptCraft.Enabled = state;
			numOptWaves.Enabled = state;
			cboOptCraft.Enabled = state;
			for (int i=0;i<10;i++) lblOptCraft[i].Enabled = state;
		}
		void OptCraftLabelRefresh()
		{
			lblOptCraft[activeOptionCraft].Text = "Craft " + (activeOptionCraft+1).ToString() + ":";
			if (XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].CraftType != 0) lblOptCraft[activeOptionCraft].Text += " " + (XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfWaves+1)
				+ " x (" + (XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfCraft+1) + ") " + Strings.CraftType[cboOptCraft.SelectedIndex];
		}
		void UpdateRole(string teamChar)
		{
			ComboBox c = null;
			switch (teamChar)
			{
				case "1":
					c = cboRoleImp;
					break;
				case "2":
					c = cboRoleReb;
					break;
				case "3":
					c = cboRole3;
					break;
				case "4":
					c = cboRole4;
					break;
				case "a":
					c = cboRoleAll;
					break;
				default:
					return;
			}
			string s = teamChar + c.Text.Substring(0, 3).ToUpper();
			for (int i=0;i<4;i++)	// first, try to find existing entry and replace it
			{
				try
				{
					if (XvtMission.FlightGroups[activeFG].Roles[i].StartsWith(teamChar))
					{
						XvtMission.FlightGroups[activeFG].Roles[i] = Common.Update(this, XvtMission.FlightGroups[activeFG].Roles[i], s).ToString();
						return;
					}
				}
				catch { /* do nothing */ }	// block is to catch null strings
			}
			// no entry
			for (int i=0;i<4;i++)
			{
				if (XvtMission.FlightGroups[activeFG].Roles[i] == "")
				{
					Common.Title(this, false);
					XvtMission.FlightGroups[activeFG].Roles[i] = s;
					break;	// find the first unused
				}
			}
			// if this is the fifth role, then tough luck it's not getting saved
		}

		void chkOptArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			int i = (int)c.Tag;
			XvtMission.FlightGroups[activeFG].OptLoadout[i] = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].OptLoadout[i], c.Checked);
			bool tempLoad = _loading;
			_loading = true;
			if (c.Checked)
			{
				if (i == 0) for (int j=1;j<8;j++) XvtMission.FlightGroups[activeFG].OptLoadout[j] = false;	// turn off warheads
				else if (i > 0 && i < 8) XvtMission.FlightGroups[activeFG].OptLoadout[0] = false;		// clear warhead None
				else if (i == 8) for (int j=9;j<12;j++) XvtMission.FlightGroups[activeFG].OptLoadout[j] = false;	// turn off beams
				else if (i > 8 && i < 12) XvtMission.FlightGroups[activeFG].OptLoadout[8] = false;	// clear beam None
				else if (i == 12) { XvtMission.FlightGroups[activeFG].OptLoadout[13] = false; XvtMission.FlightGroups[activeFG].OptLoadout[14] = false; }	// turn off CMs
				else XvtMission.FlightGroups[activeFG].OptLoadout[12] = false;	// clear CM None
			}
			else
			{
				bool b = false;
				if (i > 0 && i < 8) for (i=1;i<8;i++) b |= XvtMission.FlightGroups[activeFG].OptLoadout[i];
				if (!b && !chkOpt[0].Checked) XvtMission.FlightGroups[activeFG].OptLoadout[0] = true;
				b = false;
				if (i > 8 && i < 12) for (i=9;i<12;i++) b |= XvtMission.FlightGroups[activeFG].OptLoadout[i];
				if (!b && !chkOpt[8].Checked) XvtMission.FlightGroups[activeFG].OptLoadout[8] = true;
				b = false;
				if (i > 12 && i < 15) for (i=13;i<15;i++) b |= XvtMission.FlightGroups[activeFG].OptLoadout[i];
				if (!b && !chkOpt[12].Checked) XvtMission.FlightGroups[activeFG].OptLoadout[12] = true;
			}
			for (i=0;i<15;i++) chkOpt[i].Checked = XvtMission.FlightGroups[activeFG].OptLoadout[i];
			_loading = tempLoad;
		}
		void lblOptCraftArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				activeOptionCraft = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { activeOptionCraft = Convert.ToByte(sender); l = lblOptCraft[activeOptionCraft]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<10;i++) if (i!=activeOptionCraft) lblOptCraft[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboOptCraft.SelectedIndex = XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].CraftType;
			numOptCraft.Value = XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfCraft+1;
			numOptWaves.Value = XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfWaves+1;
			_loading = btemp;
		}
		void lblSkipTrigArr_Click(object sender, EventArgs e)
		{
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
			l.ForeColor = SystemColors.Highlight;
			ll.ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboSkipTrig.SelectedIndex = XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Condition;
			cboSkipType.SelectedIndex = -1;
			cboSkipType.SelectedIndex = XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].VariableType;
			cboSkipAmount.SelectedIndex = XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Amount;
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
				XvtMission.FlightGroups[activeFG].OptCraftCategory = (FlightGroup.OptionalCraftCategory)Common.Update(this, XvtMission.FlightGroups[activeFG].OptCraftCategory, cboOptCat.SelectedIndex);
			EnableOptCat((cboOptCat.SelectedIndex == 4));
		}
		void cboOptCraft_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].CraftType = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].CraftType, cboOptCraft.SelectedIndex));
			OptCraftLabelRefresh();
		}
		void cboSkipTrig_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Condition = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Condition, cboSkipTrig.SelectedIndex));
			LabelRefresh(XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i], (i==0 ? lblSkipTrig1 : lblSkipTrig2));
		}
		void cboSkipType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboSkipType.SelectedIndex == -1) return;
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			if (!_loading)
				XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].VariableType = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].VariableType, cboSkipType.SelectedIndex));
			ComboVarRefresh(XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].VariableType, cboSkipVar);
			try { cboSkipVar.SelectedIndex = XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Variable; }
			catch { cboSkipVar.SelectedIndex = 0; XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Variable = 0; }
			LabelRefresh(XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i], (i==0 ? lblSkipTrig1 : lblSkipTrig2));
		}
		void cboSkipVar_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Variable = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Variable, cboSkipVar.SelectedIndex));
			LabelRefresh(XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i], (i==0 ? lblSkipTrig1 : lblSkipTrig2));
		}
		void cboSkipAmount_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Amount = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i].Amount, cboSkipAmount.SelectedIndex));
			LabelRefresh(XvtMission.FlightGroups[activeFG].SkipToOrder4Trigger[i], (i==0 ? lblSkipTrig1 : lblSkipTrig2));
		}

		void grpRole_Leave(object sender, EventArgs e)
		{
			UpdateRole("1");
			UpdateRole("2");
			UpdateRole("3");
			UpdateRole("4");
			UpdateRole("a");
		}

		void numOptWaves_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfWaves = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfWaves, numOptWaves.Value - 1));
			OptCraftLabelRefresh();
		}
		void numOptCraft_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfCraft = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfCraft, numOptCraft.Value - 1));
			OptCraftLabelRefresh();
		}
		#endregion
		#region Unknowns
		void grpUnkAD_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown3 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown3, numUnk3.Value));
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown4 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown4, numUnk4.Value));
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown5 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown5, numUnk5.Value));
		}
		void grpUnkCraft_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown1 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown1, numUnk1.Value));
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown2 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown2, chkUnk2.Checked);
		}
		void grpUnkGoal_Leave(object sender, EventArgs e)
		{
			numUnkGoal_Enter("grpUnkGoal_Leave", new EventArgs());
		}
		void grpUnkOrder_Leave(object sender, EventArgs e)
		{
			numUnkOrder_Enter("grpUnkOrder_Leave", new EventArgs());
		}
		void grpUnkOther_Leave(object sender, EventArgs e)
		{
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown17 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown17, chkUnk17.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown18 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown18, chkUnk18.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown19 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown19, chkUnk19.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown20 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown20, numUnk20.Value));
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown21 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown21, numUnk21.Value));
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown22 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown22, chkUnk22.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown23 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown23, chkUnk23.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown24 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown24, chkUnk24.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown25 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown25, chkUnk25.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown26 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown26, chkUnk26.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown27 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown27, chkUnk27.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown28 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown28, chkUnk28.Checked);
			XvtMission.FlightGroups[activeFG].Unknowns.Unknown29 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Unknowns.Unknown29, chkUnk29.Checked);
		}

		void numUnkGoal_Enter(object sender, EventArgs e)
		{
			int i = (int)numUnkGoal.Value - 1;
			XvtMission.FlightGroups[activeFG].Goals[i].Unknown10 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[i].Unknown10, chkUnk10.Checked);
			XvtMission.FlightGroups[activeFG].Goals[i].Unknown11 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[i].Unknown11, chkUnk11.Checked);
			XvtMission.FlightGroups[activeFG].Goals[i].Unknown12 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[i].Unknown12, chkUnk12.Checked);
			XvtMission.FlightGroups[activeFG].Goals[i].Unknown13 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[i].Unknown13, numUnk13.Value));
			XvtMission.FlightGroups[activeFG].Goals[i].Unknown14 = (bool)Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[i].Unknown14, chkUnk14.Checked);
			XvtMission.FlightGroups[activeFG].Goals[i].Unknown16 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Goals[i].Unknown16, numUnk16.Value));
		}
		void numUnkGoal_ValueChanged(object sender, EventArgs e)
		{
			int i = (int)numUnkGoal.Value - 1;
			chkUnk10.Checked = XvtMission.FlightGroups[activeFG].Goals[i].Unknown10;
			chkUnk11.Checked = XvtMission.FlightGroups[activeFG].Goals[i].Unknown11;
			chkUnk12.Checked = XvtMission.FlightGroups[activeFG].Goals[i].Unknown12;
			numUnk13.Value = XvtMission.FlightGroups[activeFG].Goals[i].Unknown13;
			chkUnk14.Checked = XvtMission.FlightGroups[activeFG].Goals[i].Unknown14;
			numUnk16.Value = XvtMission.FlightGroups[activeFG].Goals[i].Unknown16;
		}
		void numUnkOrder_Enter(object sender, EventArgs e)
		{
			int i = (int)numUnkOrder.Value - 1;
			XvtMission.FlightGroups[activeFG].Orders[i].Unknown6 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[i].Unknown6, numUnk6.Value));
			XvtMission.FlightGroups[activeFG].Orders[i].Unknown7 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[i].Unknown7, numUnk7.Value));
			XvtMission.FlightGroups[activeFG].Orders[i].Unknown8 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[i].Unknown8, numUnk8.Value));
			XvtMission.FlightGroups[activeFG].Orders[i].Unknown9 = Convert.ToByte(Common.Update(this, XvtMission.FlightGroups[activeFG].Orders[i].Unknown9, numUnk9.Value));
		}
		void numUnkOrder_ValueChanged(object sender, EventArgs e)
		{
			int i = (int)numUnkOrder.Value - 1;
			numUnk6.Value = XvtMission.FlightGroups[activeFG].Orders[i].Unknown6;
			numUnk7.Value = XvtMission.FlightGroups[activeFG].Orders[i].Unknown7;
			numUnk8.Value = XvtMission.FlightGroups[activeFG].Orders[i].Unknown8;
			numUnk9.Value = XvtMission.FlightGroups[activeFG].Orders[i].Unknown9;
		}
		#endregion
		#endregion
		#region Messages
		void NewMess()
		{
			if (XvtMission.Messages.Count == Mission.MessageLimit)
			{
				MessageBox.Show("Mission contains maximum number of Messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			activeMessage = XvtMission.Messages.Add();
			if (XvtMission.Messages.Count == 1) EnableMessages(true);
			lstMessages.Items.Add(XvtMission.Messages[activeMessage].MessageString);
			lstMessages.SelectedIndex = activeMessage;
			Common.Title(this, _loading);
		}
		void DeleteMess()
		{
			activeMessage = XvtMission.Messages.RemoveAt(activeMessage);
			if (XvtMission.Messages.Count == 0)
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
			grpSend.Enabled = state;
			numMessDelay.Enabled = state;
			cboMessTrig.Enabled = state;
			cboMessType.Enabled = state;
			cboMessVar.Enabled = state;
			cboMessAmount.Enabled = state;
			cboMessColor.Enabled = state;
		}
		void MessListRefresh()
		{
			if (XvtMission.Messages.Count == 0) return;
			lstMessages.Items[activeMessage] = XvtMission.Messages[activeMessage].MessageString;
		}

		void lstMessages_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (XvtMission.Messages.Count == 0 || XvtMission.Messages[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch (XvtMission.Messages[e.Index].Color)
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
					brText = Brushes.Yellow;
					break;
			}
			e.Graphics.DrawString(lstMessages.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMessages.SelectedIndex == -1) return;
			activeMessage = lstMessages.SelectedIndex;
			lblMessage.Text = "Message #" + (activeMessage+1).ToString() + " of " + XvtMission.Messages.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			for (int i=0;i<4;i++) LabelRefresh(XvtMission.Messages[activeMessage].Triggers[i], lblMessTrig[i]);
			txtMessage.Text = XvtMission.Messages[activeMessage].MessageString;
			cboMessColor.SelectedIndex = XvtMission.Messages[activeMessage].Color;
			optMess1OR2.Checked = XvtMission.Messages[activeMessage].T1AndOrT2;
			optMess1AND2.Checked = !optMess1OR2.Checked;
			optMess3OR4.Checked = XvtMission.Messages[activeMessage].T3AndOrT4;
			optMess3AND4.Checked = !optMess3OR4.Checked;
			optMess12OR34.Checked = XvtMission.Messages[activeMessage].T12AndOrT34;
			optMess12AND34.Checked = !optMess12OR34.Checked;
			txtShort.Text = XvtMission.Messages[activeMessage].Note;
			numMessDelay.Value = XvtMission.Messages[activeMessage].Delay * 5;
			for (int i=0;i<10;i++) chkSendTo[i].Checked = XvtMission.Messages[activeMessage].SentToTeam[i];
			lblMessTrigArr_Click(0, new EventArgs());
			_loading = btemp;
		}

		void chkSendToArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			XvtMission.Messages[activeMessage].SentToTeam[(int)c.Tag] = (bool)Common.Update(this, XvtMission.Messages[activeMessage].SentToTeam[(int)c.Tag], c.Checked);
		}
		void lblMessTrigArr_Click(object sender, EventArgs e)
		{
			Label l=null;
			try
			{
				l = (Label)sender;
				l.Focus();
				activeMessageTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { activeMessageTrigger = Convert.ToByte(sender); l = lblMessTrig[activeMessageTrigger]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<4;i++) if (i!=activeMessageTrigger) lblMessTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboMessTrig.SelectedIndex = XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Condition;
			cboMessType.SelectedIndex = -1;
			cboMessType.SelectedIndex = XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].VariableType;
			cboMessAmount.SelectedIndex = XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Amount;
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
			XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Amount = Convert.ToByte(Common.Update(this, XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Amount, cboMessAmount.SelectedIndex));
			LabelRefresh(XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}
		void cboMessColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XvtMission.Messages[activeMessage].Color = Convert.ToByte(Common.Update(this, XvtMission.Messages[activeMessage].Color, cboMessColor.SelectedIndex));
			MessListRefresh();
		}
		void cboMessTrig_Leave(object sender, EventArgs e)
		{
			XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Condition = Convert.ToByte(Common.Update(this, XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Condition, cboMessTrig.SelectedIndex));
			LabelRefresh(XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}
		void cboMessType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMessType.SelectedIndex == -1) return;
			if (!_loading)
				XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].VariableType = Convert.ToByte(Common.Update(this, XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].VariableType, cboMessType.SelectedIndex));
			ComboVarRefresh(XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].VariableType, cboMessVar);
			try { cboMessVar.SelectedIndex = XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Variable; }
			catch { cboMessVar.SelectedIndex = 0; XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Variable = 0; }
			if (!_loading) LabelRefresh(XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}
		void cboMessVar_Leave(object sender, EventArgs e)
		{
			XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Variable = Convert.ToByte(Common.Update(this, XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger].Variable, cboMessVar.SelectedIndex));
			LabelRefresh(XvtMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}

		void numMessDelay_Leave(object sender, EventArgs e)
		{
			XvtMission.Messages[activeMessage].Delay = Convert.ToByte(Common.Update(this, XvtMission.Messages[activeMessage].Delay, numMessDelay.Value / 5));
		}

		void optMess1OR2_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				XvtMission.Messages[activeMessage].T1AndOrT2 = (bool)Common.Update(this, XvtMission.Messages[activeMessage].T1AndOrT2, optMess1OR2.Checked);
		}
		void optMess12OR34_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				XvtMission.Messages[activeMessage].T12AndOrT34 = (bool)Common.Update(this, XvtMission.Messages[activeMessage].T12AndOrT34, optMess12OR34.Checked);
		}
		void optMess3OR4_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				XvtMission.Messages[activeMessage].T3AndOrT4 = (bool)Common.Update(this, XvtMission.Messages[activeMessage].T3AndOrT4, optMess3OR4.Checked);
		}

		void txtMessage_Leave(object sender, EventArgs e)
		{
			XvtMission.Messages[activeMessage].MessageString = Common.Update(this, XvtMission.Messages[activeMessage].MessageString, txtMessage.Text).ToString();
			MessListRefresh();
		}
		void txtShort_Leave(object sender, EventArgs e)
		{
			XvtMission.Messages[activeMessage].Note = Common.Update(this, XvtMission.Messages[activeMessage].Note, txtShort.Text).ToString();
		}
		#endregion
		#region Globals
		void cboGlobalTeam_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalTeam.SelectedIndex == -1) return;
			activeTeam = (byte)cboGlobalTeam.SelectedIndex;
			lblTeamArr_Click(activeTeam, new EventArgs());	// link the Globals and Team tabs to share GlobTeam
			bool btemp = _loading;
			_loading = true;
			for (int i=0;i<12;i++) LabelRefresh(XvtMission.Globals[activeTeam].Goals[i/4].Triggers[i%4], lblGlobTrig[i]);
			for (int i=0;i<9;i++)
			{
				optGlobAndOr[i].Checked = XvtMission.Globals[activeTeam].Goals[i/3].AndOr[i%3];	// OR
				optGlobAndOr[i+9].Checked = !optGlobAndOr[i].Checked;	// AND
			}
			lblGlobTrigArr_Click(0, new EventArgs());
			_loading = btemp;
		}

		void lblGlobTrigArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				activeGlobalTrigger = Convert.ToByte(l.Tag);
			}
			catch (InvalidCastException) { activeGlobalTrigger = Convert.ToByte(sender); l = lblGlobTrig[activeGlobalTrigger]; }
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<12;i++) if (i!=activeGlobalTrigger) lblGlobTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			int g = activeGlobalTrigger / 4;
			int t = activeGlobalTrigger % 4;
			cboGlobalTrig.SelectedIndex = XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Condition;
			cboGlobalType.SelectedIndex = -1;
			cboGlobalType.SelectedIndex = XvtMission.Globals[activeTeam].Goals[g].Triggers[t].VariableType;
			cboGlobalAmount.SelectedIndex = XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Amount;
			numGlobalPoints.Value = XvtMission.Globals[activeTeam].Goals[g].Points;
			txtGlobalInc.Text = XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, (int)Globals.GoalState.Incomplete];
			txtGlobalComp.Text = XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, (int)Globals.GoalState.Complete];
			txtGlobalFail.Text = XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, (int)Globals.GoalState.Failed];
			txtGlobalFail.Visible = (g >= (int)Globals.GoalIndex.Prevent ? false : true);
			txtGlobalInc.Visible = (g >= (int)Globals.GoalIndex.Secondary ? false : true);
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
			XvtMission.Globals[activeTeam].Goals[g].AndOr[i] = (bool)Common.Update(this, XvtMission.Globals[activeTeam].Goals[g].AndOr[i], r.Checked);
		}

		void cboGlobalAmount_Leave(object sender, EventArgs e)
		{
			int g = activeGlobalTrigger / 4, t = activeGlobalTrigger % 4;
			XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Amount = Convert.ToByte(Common.Update(this, XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Amount, cboGlobalAmount.SelectedIndex));
			LabelRefresh(XvtMission.Globals[activeTeam].Goals[g].Triggers[t], lblGlobTrig[activeGlobalTrigger]);
		}
		void cboGlobalTrig_Leave(object sender, EventArgs e)
		{
			int g = activeGlobalTrigger / 4, t = activeGlobalTrigger % 4;
			XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Condition = Convert.ToByte(Common.Update(this, XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Condition, cboGlobalTrig.SelectedIndex));
			LabelRefresh(XvtMission.Globals[activeTeam].Goals[g].Triggers[t], lblGlobTrig[activeGlobalTrigger]);
		}
		void cboGlobalType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalType.SelectedIndex == -1) return;
			int g = activeGlobalTrigger / 4, t = activeGlobalTrigger % 4;
			if (!_loading)
				XvtMission.Globals[activeTeam].Goals[g].Triggers[t].VariableType = Convert.ToByte(Common.Update(this, XvtMission.Globals[activeTeam].Goals[g].Triggers[t].VariableType, cboGlobalType.SelectedIndex));
			ComboVarRefresh(XvtMission.Globals[activeTeam].Goals[g].Triggers[t].VariableType, cboGlobalVar);
			try { cboGlobalVar.SelectedIndex = XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Variable; }
			catch { cboGlobalVar.SelectedIndex = 0; XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Variable = 0; }
			if (!_loading) LabelRefresh(XvtMission.Globals[activeTeam].Goals[g].Triggers[t], lblGlobTrig[activeGlobalTrigger]);
		}
		void cboGlobalVar_Leave(object sender, EventArgs e)
		{
			int g = activeGlobalTrigger / 4, t = activeGlobalTrigger % 4;
			XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Variable = Convert.ToByte(Common.Update(this, XvtMission.Globals[activeTeam].Goals[g].Triggers[t].Variable, cboGlobalVar.SelectedIndex));
			LabelRefresh(XvtMission.Globals[activeTeam].Goals[g].Triggers[t], lblGlobTrig[activeGlobalTrigger]);
		}

		void numGlobalPoints_Leave(object sender, EventArgs e)
		{
			XvtMission.Globals[activeTeam].Goals[activeGlobalTrigger/4].Points = (short)Common.Update(this, XvtMission.Globals[activeTeam].Goals[activeGlobalTrigger/4].Points, (short)numGlobalPoints.Value);
		}
		// TODO: make this one function, store GoalState as txt.Tag
		void txtGlobalComp_Leave(object sender, EventArgs e)
		{
			int g = activeGlobalTrigger / 4, t = activeGlobalTrigger % 4;
			XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, Globals.GoalState.Complete] = Common.Update(this, XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, Globals.GoalState.Complete], txtGlobalComp.Text).ToString();
		}
		void txtGlobalFail_Leave(object sender, EventArgs e)
		{
			int g = activeGlobalTrigger / 4, t = activeGlobalTrigger % 4;
			XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, Globals.GoalState.Failed] = Common.Update(this, XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, Globals.GoalState.Failed], txtGlobalComp.Text).ToString();
		}
		void txtGlobalInc_Leave(object sender, EventArgs e)
		{
			int g = activeGlobalTrigger / 4, t = activeGlobalTrigger % 4;
			XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, Globals.GoalState.Incomplete] = Common.Update(this, XvtMission.Globals[activeTeam].Goals[g].GoalStrings[t, Globals.GoalState.Incomplete], txtGlobalComp.Text).ToString();
		}
		#endregion
		#region Teams
		void TeamRefresh()
		{
			string team = XvtMission.Teams[activeTeam].Name;
			lblTeam[activeTeam].Text = "Team " + (activeTeam+1).ToString() + ": " + team;
			cboGlobalTeam.Items[activeTeam] = team;
			cboTeam.Items[activeTeam] = team;
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
					cbo[i].Items[activeTeam] = team;
			chkAllies[activeTeam].Text = team;
		}

		void cboEoMColorArr_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox c = (ComboBox)sender;
			if (!_loading)
				XvtMission.Teams[activeTeam].EndOfMissionMessageColor[(int)c.Tag] = Convert.ToByte(Common.Update(this, XvtMission.Teams[activeTeam].EndOfMissionMessageColor[(int)c.Tag], c.SelectedIndex));
			Color clr = Color.Crimson;
			if (c.SelectedIndex == 1) clr = Color.LimeGreen;
			else if (c.SelectedIndex == 2) clr = Color.RoyalBlue;
			else if (c.SelectedIndex == 3) clr = Color.Yellow;
			txtEoM[(int)c.Tag].ForeColor = clr;
		}
		void chkAlliesArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			XvtMission.Teams[activeTeam].AlliedWithTeam[(int)c.Tag] = (bool)Common.Update(this, XvtMission.Teams[activeTeam].AlliedWithTeam[(int)c.Tag], c.Checked);
		}
		void lblTeamArr_Click(object sender, EventArgs e)
		{
			Label l = null;
			try
			{
				l = (Label)sender;
				l.Focus();
				XvtMission.Teams[activeTeam].Name = txtTeamName.Text;
				for (int i=0;i<6;i++)
				{
					XvtMission.Teams[activeTeam].EndOfMissionMessages[i] = txtEoM[i].Text;
					XvtMission.Teams[activeTeam].EndOfMissionMessageColor[i] = (byte)cboEoMColor[i].SelectedIndex;
				}
				for (int i=0;i<10;i++) XvtMission.Teams[activeTeam].AlliedWithTeam[i] = chkAllies[i].Checked;
				TeamRefresh();
				activeTeam = Convert.ToByte(l.Tag);
			}	// fired by click
			catch (InvalidCastException) { activeTeam = Convert.ToByte(sender); l = lblTeam[activeTeam]; }	// fired by code
			cboGlobalTeam.SelectedIndex = activeTeam;
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<10;i++) if (i!=activeTeam) lblTeam[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			txtTeamName.Text = XvtMission.Teams[activeTeam].Name;
			for (int i=0;i<10;i++) chkAllies[i].Checked = XvtMission.Teams[activeTeam].AlliedWithTeam[i];
			for (int i=0;i<6;i++)
			{
				txtEoM[i].Text = XvtMission.Teams[activeTeam].EndOfMissionMessages[i];
				cboEoMColor[i].SelectedIndex = XvtMission.Teams[activeTeam].EndOfMissionMessageColor[i];
			}
			_loading = btemp;
		}
		void txtEoMArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			XvtMission.Teams[activeTeam].EndOfMissionMessages[(int)t.Tag] = Common.Update(this, XvtMission.Teams[activeTeam].EndOfMissionMessages[(int)t.Tag], t.Text).ToString();
		}

		void txtTeamName_Leave(object sender, EventArgs e)
		{
			XvtMission.Teams[activeTeam].Name = Common.Update(this, XvtMission.Teams[activeTeam].Name, txtTeamName.Text).ToString();
			TeamRefresh();
		}
		#endregion
		#region Mission
		void cboMissType_Leave(object sender, EventArgs e)
		{
			XvtMission.MissionType = (Mission.MissionTypeEnum)Common.Update(this, (byte)XvtMission.MissionType, cboMissType.SelectedIndex);
		}

		void chkMissUnk3_Leave(object sender, EventArgs e)
		{
			XvtMission.Unknown3 = (bool)Common.Update(this, XvtMission.Unknown3, chkMissUnk3.Checked);
		}
		void chkMissUnk6_Leave(object sender, EventArgs e)
		{
			XvtMission.Unknown6 = (bool)Common.Update(this, XvtMission.Unknown6, chkMissUnk6.Checked);
		}

		void numMissTimeMin_Leave(object sender, EventArgs e)
		{
			XvtMission.TimeLimitMin = Convert.ToByte(Common.Update(this, XvtMission.TimeLimitMin, numMissTimeMin.Value));
		}
		void numMissTimeSec_Leave(object sender, EventArgs e)
		{
			XvtMission.TimeLimitSec = Convert.ToByte(Common.Update(this, XvtMission.TimeLimitSec, numMissTimeSec.Value));
		}
		void numMissUnk1_Leave(object sender, EventArgs e)
		{
			XvtMission.Unknown1 = Convert.ToByte(Common.Update(this, XvtMission.Unknown1, numMissUnk1.Value));
		}
		void numMissUnk2_Leave(object sender, EventArgs e)
		{
			XvtMission.Unknown2 = Convert.ToByte(Common.Update(this, XvtMission.Unknown2, numMissUnk2.Value));
		}

		void optXvT_CheckedChanged(object sender, EventArgs e)
		{
			SetBoP(!optXvT.Checked);
			Common.Title(this, _loading);
		}

		void txtMissDesc_Leave(object sender, EventArgs e)
		{
			XvtMission.MissionDescription = Common.Update(this, XvtMission.MissionDescription, txtMissDesc.Text).ToString();
		}
		void txtMissFail_Leave(object sender, EventArgs e)
		{
			XvtMission.MissionFailed = Common.Update(this, XvtMission.MissionFailed, txtMissFail.Text).ToString();
		}
		void txtMissSucc_Leave(object sender, EventArgs e)
		{
			XvtMission.MissionSuccessful = Common.Update(this, XvtMission.MissionSuccessful, txtMissSucc.Text).ToString();
		}
		void txtMissUnk4_Leave(object sender, EventArgs e)
		{
			XvtMission.Unknown4 = Common.Update(this, XvtMission.Unknown4, txtMissUnk4.Text).ToString();
		}
		void txtMissUnk5_Leave(object sender, EventArgs e)
		{
			XvtMission.Unknown5 = Common.Update(this, XvtMission.Unknown5, txtMissUnk5.Text).ToString();
		}
		#endregion
	}
}