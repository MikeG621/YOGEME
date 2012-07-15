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
using Idmr.Platform.Xwa;

namespace Idmr.Yogeme
{
	/// <summary>XWA Mission Editor GUI</summary>
	public partial class frmXWA : Form
	{
		#region vars and stuff
		Settings config = new Settings();
		bool _loading = false;
		frmMap fMap;
		frmBrief fBrief;
		frmLST fLST;
		BriefingCollection XwaBriefing;
		Mission XwaMission;
		bool bAppExit = false;
		int activeFG = 0;
		int startingShips = 1;
		string[] IFFs;
		int activeMessage = 0;
		DataTable tableWP = new DataTable("Waypoints");
		DataTable tableWPRaw = new DataTable("Waypoints_Raw");
		DataTable tableOrder = new DataTable("Orders");
		DataTable tableOrderRaw = new DataTable("Orders_Raw");
		byte activeMessageTrigger = 0;
		byte activeGlobalTrigger = 0;
		byte activeTeam = 0;
		byte activeArrDepTrigger = 0;
		byte activeFGGoal = 0;
		byte activeOrder = 0;
		byte activeOptionCraft = 0;
		#endregion
		#region control arrays
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
		CheckBox[] chkOpt = new CheckBox[15];
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

		public frmXWA()
		{
			InitializeComponent();
			_loading = true;
			InitializeMission();
			Startup();
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		public frmXWA(string path)
		{
			InitializeComponent();
			_loading = true;
			InitializeMission();
			Startup();
			if (!LoadMission(path)) return;
			lstFG.SelectedIndex = 0;
			if (XwaMission.Messages.Count != 0) lstMessages.SelectedIndex = 0;
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
		{
			if (index == -1) return;
			cbo.Items.Clear();
			switch (index)		//switch (VariableType)
			{
				case 0:
					cbo.Items.Add("None");
					break;
				case 1: //FlightGroup
					cbo.Items.AddRange(XwaMission.FlightGroups.GetList());
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
				case 0xC:	// Team
					cbo.Items.AddRange(XwaMission.Teams.GetList());
					break;
				// case 0xD: Player of Global Group
				case 0x15:	// All Teams except
					cbo.Items.AddRange(XwaMission.Teams.GetList());
					break;
				// case 0x17: Global Unit
				// case 0x19: Global Cargo
				// case 0x1B: Message #
				default:
					string[] temp = new string[256];
					for (int i=0;i<256;i++) temp[i] = i.ToString();
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
			XwaMission = new Mission();
			XwaBriefing = XwaMission.Briefings;
			config.LastMission = "";
			activeFG = 0;
			activeMessage = 0;
			XwaMission.FlightGroups[0].CraftType = Convert.ToByte(config.XWACraft);
			XwaMission.FlightGroups[0].IFF = Convert.ToByte(config.XWAIFF);
			string[] fgList = XwaMission.FlightGroups.GetList();
			ComboReset(cboArrMS, fgList, 0);
			ComboReset(cboArrMSAlt, fgList, 0);
			ComboReset(cboDepMS, fgList, 0);
			ComboReset(cboDepMSAlt, fgList, 0);
			lstFG.Items.Clear();
			lstFG.Items.Add(XwaMission.FlightGroups[activeFG].ToString(true));
			ComboReset(cboTeam, XwaMission.Teams.GetList(), XwaMission.FlightGroups[0].Team);
			cboGlobalTeam.Items.Clear();
			cboGlobalTeam.Items.AddRange(XwaMission.Teams.GetList());
			this.Text = "Ye Olde Galactic Empire Mission Editor - XWA - New Mission";
			cboMessFG.Items.Clear();
			cboMessFG.Items.Add(fgList[0]);
			tabMain.SelectedIndex = 0;
			tabFGMinor.SelectedIndex = 0;
			if (!config.XwaInstalled) cmdBackdrop.Enabled = false;
			bAppExit = true;	//becomes false if selecting "New Mission" from menu
		}
		void LabelRefresh(Mission.Trigger trigger, Label lbl)
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
				text = text.Replace("FG:" + fg, XwaMission.FlightGroups[fg].ToString());
			}
			while (text.Contains("TM:"))
			{
				int index = text.IndexOf("TM:") + 3;
				int length = text.IndexOfAny(new char[] { ' ', ',', '\0' }, index) - index;
				int team;
				if (length > 0) team = Int32.Parse(text.Substring(index, length));
				else team = Int32.Parse(text.Substring(index));
				text = text.Replace("TM:" + team, (XwaMission.Teams[team].Name == "" ? "Team " + team : XwaMission.Teams[team].Name));
			}
			return text;
		}
		bool LoadMission(string fileMission)
		{
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
							bAppExit = false;
							new frmTIE(fileMission).Show();
							Close();
							return false;
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
							break;
						default:
							fs.Close();
							throw new Exception("File is not a valid mission file for any platform, please select an appropriate *.tie file.");
					}
					#endregion
					XwaMission.LoadFromStream(fs);
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
			for (int i=0;i<XwaMission.FlightGroups.Count;i++)
			{
				lstFG.Items.Add(XwaMission.FlightGroups[i].ToString(true));
				if (XwaMission.FlightGroups[i].ArrivesIn30Seconds) CraftStart(XwaMission.FlightGroups[i], true);
			}
			UpdateFGList();
			if (XwaMission.Messages.Count == 0) EnableMessages(false);
			else
			{
				EnableMessages(true);
				for (int i=0;i<XwaMission.Messages.Count;i++) lstMessages.Items.Add(XwaMission.Messages[i].MessageString);
			}
			UpdateMissionTabs();
			cboGlobalTeam.SelectedIndex = -1;	// otherwise it doesn't trigger an index change
			cboGlobalTeam.SelectedIndex = 0;
			for (activeTeam=0;activeTeam<10;activeTeam++) TeamRefresh();
			activeTeam = 0;
			lblTeamArr_Click(lblTeam[0], new EventArgs());
			int c = fileMission.LastIndexOf("\\") + 1;
			this.Text = "Ye Olde Galactic Empire Mission Editor - XWA - " + fileMission.Substring(c);
			config.LastMission = fileMission;
			return true;
		}
		void ParameterRefresh(ComboBox cbo)
		{
			int index = cbo.SelectedIndex;
			cbo.Items.Clear();
			cbo.Items.Add("");
			for (int i = 0; i < 4; i++)
				cbo.Items.Add((XwaMission.Regions[i] == "" ? "Region " + (i + 1) : XwaMission.Regions[i]));
			cbo.Items.AddRange(XwaMission.FlightGroups.GetList());
			cbo.SelectedIndex = index;
		}
		void PromptSave()
		{
			if (config.ConfirmSave && (this.Text.IndexOf("*") != -1))
			{
				DialogResult res = MessageBox.Show("Mission has been edited without saving, would you like to save?", "Confirm", MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes)
				{
					if (XwaMission.MissionPath == "\\NewMission.tie") savXWA.ShowDialog();
					else SaveMission(XwaMission.MissionPath);
				}
			}
		}
		void SaveMission(string fileMission)
		{
			try { fBrief.Save(); }
			catch { /* do nothing */ }
			lblTeamArr_Click(lblTeam[activeTeam], new EventArgs());	// forces an update
			XwaMission.Briefings = XwaBriefing;
			try { XwaMission.Save(fileMission); }
			catch (Exception x) { MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
			this.Text = "Ye Olde Galactic Empire Mission Editor - XWA - " + XwaMission.MissionFileName;
			config.LastMission = fileMission;
			//Verify the mission after it's been saved
			if (config.Verify) Common.RunVerify(XwaMission.MissionPath, config.VerifyLocation);
		}
		void Startup()
		{
			config.LastPlatform = Settings.Platform.XWA;
			bAppExit = true;	//becomes false if selecting "New Mission" from menu
			if (config.RestrictPlatforms)
			{
				if (!config.TieInstalled) { menuNewTIE.Enabled = false; }
				if (!config.XvtInstalled) { menuNewXvT.Enabled = false; }
				if (!config.BopInstalled) { menuNewBoP.Enabled = false; }
			}
			if (Directory.Exists(config.XWAPath))
			{
				opnXWA.InitialDirectory = config.XWAPath;
				savXWA.InitialDirectory = config.XWAPath;
			}
			IFFs = Strings.IFF;
			#region FlightGroups
			#region Craft
			cboCraft.Items.AddRange(Strings.CraftType); cboCraft.SelectedIndex = XwaMission.FlightGroups[0].CraftType;
			cboIFF.Items.AddRange(Strings.IFF); cboIFF.SelectedIndex = XwaMission.FlightGroups[0].IFF;
			cboAI.Items.AddRange(Strings.Rating); cboAI.SelectedIndex = XwaMission.FlightGroups[0].AI;
			cboMarkings.Items.AddRange(Strings.Color); cboMarkings.SelectedIndex = XwaMission.FlightGroups[0].Markings;
			cboPlayer.SelectedIndex = 0;
			cboPosition.SelectedIndex = 0;
			cboFormation.Items.AddRange(Strings.Formation); cboFormation.SelectedIndex = 0;
			cboRadio.Items.AddRange(Strings.Radio); cboRadio.SelectedIndex = XwaMission.FlightGroups[0].Radio;
			cboStatus.Items.AddRange(Strings.Status); cboStatus.SelectedIndex = 0;
			cboStatus2.Items.AddRange(Strings.Status); cboStatus2.SelectedIndex = 0;
			cboWarheads.Items.AddRange(Strings.Warheads); cboWarheads.SelectedIndex = 0;
			cboBeam.Items.AddRange(Strings.Beam); cboBeam.SelectedIndex = 0;
			cboCounter.SelectedIndex = 0;
			cboGlobCargo.Items.Add("None");
			for (int i=0;i<16;i++) cboGlobCargo.Items.Add("Gobal Cargo " + (i+1).ToString());
			cboGlobCargo.SelectedIndex = 0;
			cboGlobSpecCargo.Items.Add("None");
			for (int i=0;i<16;i++) cboGlobSpecCargo.Items.Add("Gobal Cargo " + (i+1).ToString());
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
			ParameterRefresh(cboADPara);
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
			tableWP.Columns.Add("X"); tableWP.Columns.Add("Y"); tableWP.Columns.Add("Z");
			tableWPRaw.Columns.Add("X"); tableWPRaw.Columns.Add("Y"); tableWPRaw.Columns.Add("Z");
			for (int i=0;i<4;i++)	//initialize WPs
			{
				DataRow dr = tableWP.NewRow();
				int j;
				for (j=0;j<3;j++) dr[j] = 0;	//set X Y Z to zero
				tableWP.Rows.Add(dr);
				dr = tableWPRaw.NewRow();
				for (j=0;j<3;j++) dr[j] = 0;	//mirror in raw table
				tableWPRaw.Rows.Add(dr);
			}
			tableOrder.Columns.Add("X"); tableOrder.Columns.Add("Y"); tableOrder.Columns.Add("Z");
			tableOrderRaw.Columns.Add("X"); tableOrderRaw.Columns.Add("Y"); tableOrderRaw.Columns.Add("Z");
			for (int i=0;i<8;i++)	//initialize WPs
			{
				DataRow dr = tableOrder.NewRow();
				int j;
				for (j=0;j<3;j++) dr[j] = 0;	//set X Y Z to zero
				tableOrder.Rows.Add(dr);
				dr = tableOrderRaw.NewRow();
				for (j=0;j<3;j++) dr[j] = 0;	//mirror in raw table
				tableOrderRaw.Rows.Add(dr);
			}
			dataWaypoints.Table = tableWP;
			dataWaypoints_Raw.Table = tableWPRaw;
			dataOrders.Table = tableOrder;
			dataOrders_Raw.Table = tableOrderRaw;
			dataWP.DataSource = dataWaypoints;
			dataWP_Raw.DataSource = dataWaypoints_Raw;
			dataO.DataSource = dataOrders;
			dataO_Raw.DataSource = dataOrders_Raw;
			this.tableWP.RowChanged += new DataRowChangeEventHandler(tableWP_RowChanged);
			this.tableWPRaw.RowChanged += new DataRowChangeEventHandler(tableWPRaw_RowChanged);
			this.tableOrder.RowChanged += new DataRowChangeEventHandler(tableOrder_RowChanged);
			this.tableOrderRaw.RowChanged += new DataRowChangeEventHandler(tableOrderRaw_RowChanged);
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
			ParameterRefresh(cboGoalPara);
			#endregion
			#region Options
			cboOptCraft.Items.AddRange(Strings.CraftType);
			cboSkipAmount.Items.AddRange(Strings.Amount);
			cboSkipTrig.Items.AddRange(Strings.Trigger);
			cboSkipType.Items.AddRange(Strings.VariableType);
			ParameterRefresh(cboSkipPara);
			cboSkipOrder.SelectedIndex = 0;
			cboRole1.Items.AddRange(Strings.Roles); cboRole1.SelectedIndex = 0;
			cboRole2.Items.AddRange(Strings.Roles); cboRole2.SelectedIndex = 0;
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
			ParameterRefresh(cboGlobalPara);
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
		}

		void frmXWA_Closing(object sender, FormClosingEventArgs e)
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

		void opnXWA_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_loading = true;
			XwaMission.MissionPath = opnXWA.FileName;
			if (LoadMission(XwaMission.MissionPath))
			{
				tabMain.SelectedIndex = 0;
				tabFGMinor.SelectedIndex = 0;
				lstFG.SelectedIndex = 0;
				try { lstMessages.SelectedIndex = 0; }
				catch { /* do nothing */}
			}
			_loading = false;
		}

		void savXWA_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			XwaMission.MissionPath = savXWA.FileName;
			SaveMission(XwaMission.MissionPath);
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
			fBrief = new frmBrief(ref XwaBriefing);
			fBrief.Show();
		}
		void menuCopy_Click(object sender, EventArgs e)
		{
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Stream stream = new FileStream("YOGEME.bin", FileMode.Create, FileAccess.Write, FileShare.None);
			#region ArrDep
			if (sender.ToString() == "AD")
			{
				formatter.Serialize(stream, XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger]);
				stream.Close();
				return;
			}
			#endregion
			#region FG Goal
			if (sender.ToString() == "Goal")
			{
				byte[] g = new byte[9];
				for (int i=0;i<9;i++) g[i] = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][i];
				formatter.Serialize(stream, g);
				stream.Close();
				return;
			}
			#endregion
			#region Orders
			if (sender.ToString() == "Order")
			{
				formatter.Serialize(stream, XwaMission.FlightGroups[activeFG].Orders[activeOrder / 4, activeOrder % 4]);
				stream.Close();
				return;
			}
			#endregion
			#region Skip to Order
			if (sender.ToString() == "Skip")
			{
				int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
				formatter.Serialize(stream, XwaMission.FlightGroups[activeFG].Orders[activeOrder/4, activeOrder%4].SkipTriggers[i]);
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
				formatter.Serialize(stream, XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger]);
				stream.Close();
				return;
			}
			#endregion

			switch (tabMain.SelectedIndex)
			{
				case 0:
					formatter.Serialize(stream, XwaMission.FlightGroups[activeFG]);
					break;
				case 1:
					if (XwaMission.Messages.Count != 0) formatter.Serialize(stream, XwaMission.Messages[activeMessage]);
					break;
				case 2:
					formatter.Serialize(stream, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger/3].Triggers[activeGlobalTrigger%3]);
					break;
				case 3:
					formatter.Serialize(stream, XwaMission.Teams[activeTeam]);
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
			fLST = new frmLST(Settings.Platform.XWA);
			fLST.Show();
		}
		void menuMap_Click(object sender, EventArgs e)
		{
			fMap = new frmMap(XwaMission.FlightGroups);
			fMap.Show();
		}
		void menuNewBoP_Click(object sender, EventArgs e)
		{
			PromptSave();
			CloseForms();
			bAppExit = false;
			new frmXvT(true).Show();
			Close();
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
			bAppExit = false;
			new frmXvT(false).Show();
			Close();
		}
		void menuNewXWA_Click(object sender, EventArgs e)
		{
			PromptSave();
			CloseForms();
			_loading = true;
			InitializeMission();
			UpdateMissionTabs();
			lstMessages.Items.Clear();
			EnableMessages(false);
			lblMessage.Text = "Message #0 of 0";
			for (activeTeam=0;activeTeam<10;activeTeam++) TeamRefresh();
			lblTeamArr_Click(lblTeam[0], new EventArgs());
			lstFG.SelectedIndex = 0;
			_loading = false;
		}
		void menuOpen_Click(object sender, EventArgs e)
		{
			PromptSave();
			opnXWA.ShowDialog();
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
					Mission.Trigger trig_temp = (Mission.Trigger)formatter.Deserialize(stream);
					XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger] = trig_temp;
					lblADTrigArr_Click(activeArrDepTrigger, new EventArgs());
					LabelRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
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
					XwaMission.FlightGroups[activeFG].Orders[activeOrder / 4, activeOrder % 4] = order_temp;
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
					FlightGroup.Goal goal_temp = (FlightGroup.Goal)formatter.Deserialize(stream);
					// can't compare to null, but maybe ^ will trip?
					XwaMission.FlightGroups[activeFG].Goals[activeFGGoal] = goal_temp;
					lblGoalArr_Click(activeFGGoal, new EventArgs());
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
					XwaMission.FlightGroups[activeFG].Orders[activeOrder / 4, activeOrder % 4].SkipTriggers[j] = trig_temp;
					lblSkipTrigArr_Click(j, new EventArgs());
					LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[activeOrder / 4, activeOrder % 4].SkipTriggers[j], (j == 0 ? lblSkipTrig1 : lblSkipTrig2));	// no array, hence explicit naming
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
					XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger] = trig_temp;
					lblMessTrigArr_Click(activeMessageTrigger, new EventArgs());
					LabelRefresh(XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
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
						XwaMission.FlightGroups[activeFG] = fg_temp;
						ListRefresh();
						startingShips--;
						lstFG.SelectedIndex = activeFG;
						if (XwaMission.FlightGroups[activeFG].ArrivesIn30Seconds)
						{
							startingShips += XwaMission.FlightGroups[activeFG].NumberOfCraft;
						}
						lblStarting.Text = startingShips.ToString() + " craft at 30 seconds";
					}
					catch { /* do nothing */ }
					break;
				case 1:
					try
					{
						Platform.Xwa.Message mess_temp = (Platform.Xwa.Message)formatter.Deserialize(stream);
						if (mess_temp == null) throw new Exception();
						NewMess();
						XwaMission.Messages[activeMessage] = mess_temp;
						MessListRefresh();
						lstMessages.SelectedIndex = activeMessage;
					}
					catch { /* do nothing */ }
					break;
				case 2:
					try
					{
						Mission.Trigger trig_temp = (Mission.Trigger)formatter.Deserialize(stream);
						XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4] = trig_temp;
						lblGlobTrigArr_Click(activeGlobalTrigger, new EventArgs());
						Common.Title(this, false);
					}
					catch { /* do nothing */ }
					break;
				case 3:
					try
					{
						Team team_temp = (Team)formatter.Deserialize(stream);
						if (team_temp == null) throw new Exception();
						XwaMission.Teams[activeTeam] = team_temp;
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
			if (XwaMission.MissionPath  == "\\NewMission.tie") savXWA.ShowDialog();
			else SaveMission(XwaMission.MissionPath);
		}
		void menuSaveAsBoP_Click(object sender, EventArgs e)
		{
			PromptSave();
			try
			{
				Platform.Xvt.Mission converted = Platform.Converter.XwaToXvtBop(XwaMission, true);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsTIE_Click(object sender, EventArgs e)
		{
			PromptSave();
			try
			{
				Platform.Tie.Mission converted = Platform.Converter.XwaToTie(XwaMission);
				converted.Save();
			}
			catch (ArgumentException x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void menuSaveAsXvT_Click(object sender, EventArgs e)
		{
			PromptSave();
			try
			{
				Platform.Xvt.Mission converted = Platform.Converter.XwaToXvtBop(XwaMission, false);
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
		void menuVerify_Click(object sender, EventArgs e)
		{
			menuSave_Click("Verify", new System.EventArgs());
			if (!config.Verify) Common.RunVerify(XwaMission.MissionPath, config.VerifyLocation);	//prevents from doing this twice due to Save
		}
		#endregion
		#region FlightGroups
		void DeleteFG()
		{
			if (XwaMission.FlightGroups.Count != 1) lstFG.Items.RemoveAt(activeFG);
			CraftStart(XwaMission.FlightGroups[activeFG], false);
			if (XwaMission.FlightGroups.Count == 1)
			{
				XwaMission.FlightGroups.Clear();
				activeFG = 0;
				XwaMission.FlightGroups[0].CraftType = config.XWACraft;
				XwaMission.FlightGroups[0].IFF = config.XWAIFF;
				CraftStart(XwaMission.FlightGroups[0], true);
			}
			else activeFG = XwaMission.FlightGroups.RemoveAt(activeFG);
			UpdateFGList();
			lstFG.SelectedIndex = 0;
			Common.Title(this, _loading);
			try
			{
				fMap.Import(XwaMission.FlightGroups);
				fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
		}
		void ListRefresh()
		{
			lstFG.Items[activeFG] = XwaMission.FlightGroups[activeFG].ToString(true);
		}
		void NewFG()
		{
			if (XwaMission.FlightGroups.Count == Mission.FlightGroupLimit)
			{
				MessageBox.Show("Mission contains maximum number of Flight Groups", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			activeFG = XwaMission.FlightGroups.Add();
			XwaMission.FlightGroups[activeFG].CraftType = config.XWACraft;
			XwaMission.FlightGroups[activeFG].IFF = config.XWAIFF;
			CraftStart(XwaMission.FlightGroups[activeFG], true);
			lstFG.Items.Add(XwaMission.FlightGroups[activeFG].ToString(true));
			UpdateFGList();
			_loading = true;
			lstFG.SelectedIndex = activeFG;
			_loading = false;
			Common.Title(this, _loading);
			try
			{
				fMap.Import(XwaMission.FlightGroups);
				fMap.MapPaint(true);
			}
			catch { /* do nothing */ }
		}
		void UpdateFGList()
		{
			string[] fgList = XwaMission.FlightGroups.GetList();
			bool temp = _loading;
			_loading = true;
			ComboReset(cboArrMS, fgList, 0);
			ComboReset(cboArrMSAlt, fgList, 0);
			ComboReset(cboDepMS, fgList, 0);
			ComboReset(cboDepMSAlt, fgList, 0);
			cboMessFG.Items.Clear(); cboMessFG.Items.AddRange(fgList);
			if (XwaMission.Messages.Count != 0) cboMessFG.SelectedIndex = XwaMission.Messages[activeMessage].OriginatingFG;
			ParameterRefresh(cboSkipPara);
			ParameterRefresh(cboGoalPara);
			ParameterRefresh(cboADPara);
			ParameterRefresh(cboMessPara);
			ParameterRefresh(cboGlobalPara);
			_loading = temp;
			ListRefresh();
		}

		void lstFG_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1 || XwaMission.FlightGroups[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch (XwaMission.FlightGroups[e.Index].IFF)
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
			lblFG.Text = "Flight Group #" + (activeFG+1).ToString() + " of " + XwaMission.FlightGroups.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			#region Craft
			txtName.Text = XwaMission.FlightGroups[activeFG].Name;
			txtCargo.Text = XwaMission.FlightGroups[activeFG].Cargo;
			txtSpecCargo.Text = XwaMission.FlightGroups[activeFG].Cargo;
			numSC.Value = XwaMission.FlightGroups[activeFG].SpecialCargoCraft;
			chkRandSC.Checked = XwaMission.FlightGroups[activeFG].RandSpecCargo;
			cboCraft.SelectedIndex = XwaMission.FlightGroups[activeFG].CraftType;
			cboIFF.SelectedIndex = XwaMission.FlightGroups[activeFG].IFF;
			cboTeam.SelectedIndex = XwaMission.FlightGroups[activeFG].Team;
			try { cboAI.SelectedIndex = XwaMission.FlightGroups[activeFG].AI; }	// for some reason, some custom missions have this as -1
			catch { cboAI.SelectedIndex = 2; XwaMission.FlightGroups[activeFG].AI = 2; }	// default to Veteran
			cboMarkings.SelectedIndex = XwaMission.FlightGroups[activeFG].Markings;
			cboPlayer.SelectedIndex = XwaMission.FlightGroups[activeFG].PlayerNumber;
			cboPosition.SelectedIndex = XwaMission.FlightGroups[activeFG].PlayerCraft;
			cboFormation.SelectedIndex = XwaMission.FlightGroups[activeFG].Formation;
			cboRadio.SelectedIndex = XwaMission.FlightGroups[activeFG].Radio;
			numLead.Value = XwaMission.FlightGroups[activeFG].FormLeaderDist;
			numSpacing.Value = XwaMission.FlightGroups[activeFG].FormDistance;
			numWaves.Value = XwaMission.FlightGroups[activeFG].NumberOfWaves;
			numCraft.Value = XwaMission.FlightGroups[activeFG].NumberOfCraft;
			numGG.Value = XwaMission.FlightGroups[activeFG].GlobalGroup;
			numGU.Value = XwaMission.FlightGroups[activeFG].GlobalUnit;
			chkGU.Checked = XwaMission.FlightGroups[activeFG].GlobalNumbering;
			cboStatus.SelectedIndex = XwaMission.FlightGroups[activeFG].Status1;
			cboStatus2.SelectedIndex = XwaMission.FlightGroups[activeFG].Status2;
			cboWarheads.SelectedIndex = XwaMission.FlightGroups[activeFG].Missile;
			cboBeam.SelectedIndex = XwaMission.FlightGroups[activeFG].Beam;
			cboCounter.SelectedIndex = XwaMission.FlightGroups[activeFG].Countermeasures;
			numExplode.Value = XwaMission.FlightGroups[activeFG].ExplosionTime;
			numBackdrop.Value = XwaMission.FlightGroups[activeFG].Backdrop;
			cboGlobCargo.SelectedIndex = XwaMission.FlightGroups[activeFG].GlobalCargo;
			cboGlobSpecCargo.SelectedIndex = XwaMission.FlightGroups[activeFG].GlobalSpecialCargo;
			#endregion
			#region Arr/Dep
			optArrMS.Checked = XwaMission.FlightGroups[activeFG].ArrivalMethod1;
			optArrHyp.Checked = !optArrMS.Checked;
			try { cboArrMS.SelectedIndex = XwaMission.FlightGroups[activeFG].ArrivalCraft1; }
			catch { cboArrMS.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].ArrivalCraft1 = 0; optArrHyp.Checked = true; }
			optArrMSAlt.Checked = XwaMission.FlightGroups[activeFG].ArrivalMethod2;
			optArrHypAlt.Checked = !optArrMSAlt.Checked;
			try { cboArrMSAlt.SelectedIndex = XwaMission.FlightGroups[activeFG].ArrivalCraft2; }
			catch { cboArrMSAlt.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].ArrivalCraft2 = 0; optArrHypAlt.Checked = true; }
			optDepMS.Checked = XwaMission.FlightGroups[activeFG].DepartureMethod1;
			optDepHyp.Checked = !optDepMS.Checked;
			try { cboDepMS.SelectedIndex = XwaMission.FlightGroups[activeFG].DepartureCraft1; }
			catch { cboDepMS.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].DepartureCraft1 = 0; optDepHyp.Checked = true; }
			optDepMSAlt.Checked = XwaMission.FlightGroups[activeFG].DepartureMethod2;
			optDepHypAlt.Checked = !optDepMSAlt.Checked;
			try { cboDepMSAlt.SelectedIndex = XwaMission.FlightGroups[activeFG].DepartureCraft2; }
			catch { cboDepMSAlt.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].DepartureCraft2 = 0; optDepHypAlt.Checked = true; }
			for (int i=0;i<4;i++)
			{
				optADAndOr[i].Checked = XwaMission.FlightGroups[activeFG].ArrDepAndOr[i];
				optADAndOr[i+4].Checked = !optADAndOr[i].Checked;
			}
			numArrMin.Value = XwaMission.FlightGroups[activeFG].ArrivalDelayMinutes;
			numArrSec.Value = XwaMission.FlightGroups[activeFG].ArrivalDelaySeconds;
			numDepMin.Value = XwaMission.FlightGroups[activeFG].DepartureTimerMinutes;
			numDepSec.Value = XwaMission.FlightGroups[activeFG].DepartureTimerSeconds;
			cboAbort.SelectedIndex = XwaMission.FlightGroups[activeFG].AbortTrigger;
			cboDiff.SelectedIndex = XwaMission.FlightGroups[activeFG].Difficulty;
			chkArrHuman.Checked = XwaMission.FlightGroups[activeFG].ArriveOnlyIfHuman;
			for (int i=0;i<6;i++) LabelRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[i], lblADTrig[i]);
			lblADTrigArr_Click(lblADTrig[0], new EventArgs());
			#endregion
			for (activeFGGoal=0;activeFGGoal<8;activeFGGoal++) GoalLabelRefresh();
			lblGoalArr_Click(lblGoal[0], new EventArgs());
			#region Waypoints
			cboWP.SelectedIndex = -1;	// force change
			cboWP.SelectedIndex = 0;
			for (int i=0;i<4;i++)
			{
				for (int j=0;j<3;j++)
				{
					tableWPRaw.Rows[i][j] = XwaMission.FlightGroups[activeFG].Waypoints[i][j];
					tableWP.Rows[i][j] = Math.Round((double)XwaMission.FlightGroups[activeFG].Waypoints[i][j] / 160, 2);
				}
				chkWP[i].Checked = XwaMission.FlightGroups[activeFG].Waypoints[i].Enabled;
			}
			numSP1.Value = XwaMission.FlightGroups[activeFG].Waypoints[0].Region + 1;
			numSP2.Value = XwaMission.FlightGroups[activeFG].Waypoints[1].Region + 1;
			numSP3.Value = XwaMission.FlightGroups[activeFG].Waypoints[2].Region + 1;
			numHYP.Value = XwaMission.FlightGroups[activeFG].Waypoints[3].Region + 1;
			tableWPRaw.AcceptChanges();
			tableWP.AcceptChanges();
			numYaw.Value = XwaMission.FlightGroups[activeFG].Yaw;
			numPitch.Value = XwaMission.FlightGroups[activeFG].Pitch;
			numRoll.Value = XwaMission.FlightGroups[activeFG].Roll;
			#endregion
			for (activeOrder=0;activeOrder<4;activeOrder++) OrderLabelRefresh();
			lblOrderArr_Click(lblOrder[0], new EventArgs());
			#region Options
			chkRole1.Checked = XwaMission.FlightGroups[activeFG].EnableDesignation1;
			chkRole2.Checked = XwaMission.FlightGroups[activeFG].EnableDesignation2;
			cboRole1.SelectedIndex = XwaMission.FlightGroups[activeFG].Designation1;
			cboRole2.SelectedIndex = XwaMission.FlightGroups[activeFG].Designation2;
			txtRole.Text = XwaMission.FlightGroups[activeFG].Role;
			txtPilot.Text = XwaMission.FlightGroups[activeFG].PilotID;
			for (int i=0;i<15;i++) chkOpt[i].Checked = XwaMission.FlightGroups[activeFG].OptLoadout[i];
			lblSkipTrigArr_Click(lblSkipTrig1, new EventArgs());
			for (activeOptionCraft=0;activeOptionCraft<10;activeOptionCraft++) OptCraftLabelRefresh();
			lblOptCraftArr_Click(lblOptCraft[0], new EventArgs());
			cboOptCat.SelectedIndex = (byte)XwaMission.FlightGroups[activeFG].OptCraftCategory;
			#endregion
			#region Unknowns
			numUnk1.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown1;
			numUnk3.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown3;
			numUnk4.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown4;
			numUnk5.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown5;
			chkUnk6.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown6;
			numUnk7.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown7;
			numUnk8.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown8;
			cboUnkOrder.SelectedIndex = 0;
			numUnkGoal.Value = 1;
			numUnk16.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown16;
			numUnk17.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown17;
			numUnk18.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown18;
			numUnk19.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown19;
			numUnk20.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown20;
			numUnk21.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown21;
			chkUnk22.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown22;
			numUnk23.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown23;
			numUnk24.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown24;
			numUnk25.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown25;
			numUnk26.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown26;
			numUnk27.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown27;
			numUnk28.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown28;
			chkUnk29.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown29;
			chkUnk30.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown30;
			chkUnk31.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown31;
			numUnk32.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown32;
			numUnk33.Value = XwaMission.FlightGroups[activeFG].Unknowns.Unknown33;
			chkUnk34.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown34;
			chkUnk35.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown35;
			chkUnk36.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown36;
			chkUnk37.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown37;
			chkUnk38.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown38;
			chkUnk39.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown39;
			chkUnk40.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown40;
			chkUnk41.Checked = XwaMission.FlightGroups[activeFG].Unknowns.Unknown41;
			#endregion
			_loading = btemp;
			EnableBackdrop((XwaMission.FlightGroups[activeFG].CraftType == 0xB7 ? true : false));
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
				lblSC.Text = "Specal Cargo";
				lblName.Text = "Name";
			}
		}

		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnableBackdrop((cboCraft.SelectedIndex == 0xB7 ? true : false));
			if (_loading) return;
			XwaMission.FlightGroups[activeFG].CraftType = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].CraftType, cboCraft.SelectedIndex));
			UpdateFGList();
		}
		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XwaMission.FlightGroups[activeFG].Formation = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Formation, cboFormation.SelectedIndex));
		}
		void cboGlobCargo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XwaMission.FlightGroups[activeFG].GlobalCargo = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].GlobalCargo, cboGlobCargo.SelectedIndex));
		}

		void chkRandSC_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XwaMission.FlightGroups[activeFG].RandSpecCargo = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].RandSpecCargo, chkRandSC.Checked);
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
			try
			{
				dlgBackdrop dlg = new dlgBackdrop(XwaMission.FlightGroups[activeFG].Backdrop, XwaMission.FlightGroups[activeFG].GlobalCargo);
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
		}
		void cmdForms_Click(object sender, EventArgs e)
		{
			dlgFormation dlg = new dlgFormation(XwaMission.FlightGroups[activeFG].Formation);
			if (dlg.ShowDialog() == DialogResult.OK) cboFormation.SelectedIndex = dlg.Formation;
		}

		void grpCraft2_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].IFF = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].IFF, cboIFF.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Team = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Team, cboTeam.SelectedIndex));
			XwaMission.FlightGroups[activeFG].AI = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].AI, cboAI.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Markings = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Markings, cboMarkings.SelectedIndex));
			XwaMission.FlightGroups[activeFG].PlayerNumber = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].PlayerNumber, cboPlayer.SelectedIndex));
			XwaMission.FlightGroups[activeFG].PlayerCraft = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].PlayerCraft, cboPosition.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Radio = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Radio, cboRadio.SelectedIndex));
			XwaMission.FlightGroups[activeFG].FormDistance = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].FormDistance, numSpacing.Value));
			XwaMission.FlightGroups[activeFG].FormLeaderDist = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].FormLeaderDist, numLead.Value));
			ListRefresh();
		}
		void grpCraft3_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].NumberOfWaves = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].NumberOfWaves, numWaves.Value));
			CraftStart(XwaMission.FlightGroups[activeFG], false);
			XwaMission.FlightGroups[activeFG].NumberOfCraft = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].NumberOfCraft, numCraft.Value));
			CraftStart(XwaMission.FlightGroups[activeFG], true);
			if (XwaMission.FlightGroups[activeFG].SpecialCargoCraft > XwaMission.FlightGroups[activeFG].NumberOfCraft) numSC.Value = 0;
			XwaMission.FlightGroups[activeFG].GlobalGroup = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].GlobalGroup, numGG.Value));
			XwaMission.FlightGroups[activeFG].GlobalUnit = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].GlobalUnit, numGU.Value));
			XwaMission.FlightGroups[activeFG].GlobalNumbering = Convert.ToBoolean(Common.Update(this, XwaMission.FlightGroups[activeFG].GlobalNumbering, chkGU.Checked));
			ListRefresh();
		}
		void grpCraft4_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Status1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Status1, cboStatus.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Status2 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Status2, cboStatus2.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Missile = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Missile, cboWarheads.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Beam = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Beam, cboBeam.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Countermeasures = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Countermeasures, cboCounter.SelectedIndex));
			//XwaMission.FlightGroups[FG].ExplosionTime = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ExplosiongTime, numExplode.Value));
			XwaMission.FlightGroups[activeFG].GlobalSpecialCargo = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].GlobalSpecialCargo, cboGlobSpecCargo.SelectedIndex));
		}

		void numBackdrop_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XwaMission.FlightGroups[activeFG].Backdrop = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Backdrop, numBackdrop.Value));
		}
		void numSC_ValueChanged(object sender, EventArgs e)
		{
			if (XwaMission.FlightGroups[activeFG].RandSpecCargo)
			{
				if (numSC.Value != 0) numSC.Value = 0;
				return;
			}
			if (numSC.Value == 0 || numSC.Value > XwaMission.FlightGroups[activeFG].NumberOfCraft)
			{
				XwaMission.FlightGroups[activeFG].SpecialCargoCraft = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].SpecialCargoCraft, 0));
				txtSpecCargo.Visible = false;
				lblNotUsed.Visible = true;
			}
			else
			{
				XwaMission.FlightGroups[activeFG].SpecialCargoCraft = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].SpecialCargoCraft, numSC.Value));
				txtSpecCargo.Visible = true;
				lblNotUsed.Visible = false;
			}
		}

		void txtCargo_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Cargo = Common.Update(this, XwaMission.FlightGroups[activeFG].Cargo, txtCargo.Text).ToString();
		}
		void txtName_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Name = Common.Update(this, XwaMission.FlightGroups[activeFG].Name, txtName.Text).ToString();
			UpdateFGList();
		}
		void txtSpecCargo_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].SpecialCargo = Common.Update(this, XwaMission.FlightGroups[activeFG].SpecialCargo, txtSpecCargo.Text).ToString();
		}
		#endregion
		#region Arr/Dep
		void lblADTrigArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			activeArrDepTrigger = Convert.ToByte(l.Tag);
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<6;i++) if (i != activeArrDepTrigger) lblADTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboADTrig.SelectedIndex = XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition;
			cboADTrigType.SelectedIndex = -1;
			cboADTrigType.SelectedIndex = XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType;
			cboADTrigAmount.SelectedIndex = XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount;
			cboADPara.SelectedIndex = XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Parameter1;
			numADPara.Value = XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Parameter2;
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
			XwaMission.FlightGroups[activeFG].ArrDepAndOr[(int)r.Tag] = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].ArrDepAndOr[(int)r.Tag], r.Checked);
		}

		void cboADPara_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Parameter1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Parameter1, cboADPara.SelectedIndex));
			LabelRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrig_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Condition, cboADTrig.SelectedIndex));
			LabelRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigAmount_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Amount, cboADTrigAmount.SelectedIndex));
			LabelRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboADTrigType.SelectedIndex == -1) return;
			if (!_loading)
				XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType, cboADTrigType.SelectedIndex));
			ComboVarRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].VariableType, cboADTrigVar);
			try { cboADTrigVar.SelectedIndex = XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable; }
			catch { cboADTrigVar.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable = 0; }
			LabelRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboADTrigVar_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Variable, cboADTrigVar.SelectedIndex));
			LabelRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void cboArrMS_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].ArrivalCraft1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrivalCraft1, cboArrMS.SelectedIndex));
		}
		void cboArrMSAlt_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].ArrivalCraft2 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrivalCraft2, cboArrMSAlt.SelectedIndex));
		}
		void cboDiff_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Difficulty = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Difficulty, cboDiff.SelectedIndex));
		}

		void chkArrHuman_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].ArriveOnlyIfHuman = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].ArriveOnlyIfHuman, chkArrHuman.Checked);
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
			XwaMission.FlightGroups[activeFG].DepartureCraft1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].DepartureCraft1, cboDepMS.SelectedIndex));
			XwaMission.FlightGroups[activeFG].DepartureCraft2 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].DepartureCraft2, cboDepMSAlt.SelectedIndex));
			XwaMission.FlightGroups[activeFG].AbortTrigger = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].AbortTrigger, cboAbort.SelectedIndex));
			XwaMission.FlightGroups[activeFG].DepartureTimerMinutes = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].DepartureTimerMinutes, numDepMin.Value));
			XwaMission.FlightGroups[activeFG].DepartureTimerSeconds = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].DepartureTimerSeconds, numDepSec.Value));
		}

		void numADPara_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Parameter2 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger].Parameter2, numADPara.Value));
			LabelRefresh(XwaMission.FlightGroups[activeFG].ArrDepTriggers[activeArrDepTrigger], lblADTrig[activeArrDepTrigger]);
		}
		void numArrMin_Leave(object sender, EventArgs e)
		{
			CraftStart(XwaMission.FlightGroups[activeFG], false);
			XwaMission.FlightGroups[activeFG].ArrivalDelayMinutes = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrivalDelayMinutes, numArrMin.Value));
			CraftStart(XwaMission.FlightGroups[activeFG], true);
		}
		void numArrSec_Leave(object sender, EventArgs e)
		{
			CraftStart(XwaMission.FlightGroups[activeFG], false);
			XwaMission.FlightGroups[activeFG].ArrivalDelaySeconds = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].ArrivalDelaySeconds, numArrSec.Value));
			CraftStart(XwaMission.FlightGroups[activeFG], true);
		}

		void optArrMS_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMS.Enabled = optArrMS.Checked;
			if (!_loading)
				XwaMission.FlightGroups[activeFG].ArrivalMethod1 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].ArrivalMethod1, optArrMS.Checked);
		}
		void optArrMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboArrMSAlt.Enabled = optArrMSAlt.Checked;
			if (!_loading)
				XwaMission.FlightGroups[activeFG].ArrivalMethod2 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].ArrivalMethod2, optArrMSAlt.Checked);
		}
		void optDepMS_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMS.Enabled = optDepMS.Checked;
			if (!_loading)
				XwaMission.FlightGroups[activeFG].DepartureMethod1 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].DepartureMethod1, optDepMS.Checked);
		}
		void optDepMSAlt_CheckedChanged(object sender, EventArgs e)
		{
			cboDepMSAlt.Enabled = optDepMSAlt.Checked;
			if (!_loading)
				XwaMission.FlightGroups[activeFG].DepartureMethod2 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].DepartureMethod2, optDepMSAlt.Checked);
		}
		#endregion
		#region Orders
		void OrderLabelRefresh()
		{
			string orderText = XwaMission.FlightGroups[activeFG].Orders[(int)numORegion.Value - 1, activeOrder].ToString();
			orderText = replaceTargetText(orderText);
			lblOrder[activeOrder].Text = "Order " + (activeOrder + 1) + ": " + orderText;
		}

		void lblOrderArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			activeOrder = Convert.ToByte(l.Tag);
			FlightGroup.Order order = XwaMission.FlightGroups[activeFG].Orders[(int)numORegion.Value - 1, activeOrder];
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<4;i++) if (i!=activeOrder) lblOrder[i].ForeColor = SystemColors.ControlText;
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
				XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Command = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Command, cboOrders.SelectedIndex));
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
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target1, cboOT1.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT1Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT1Type.SelectedIndex == -1) return;
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target1Type = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target1Type, cboOT1Type.SelectedIndex));
			ComboVarRefresh(XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target1Type, cboOT1);
			try { cboOT1.SelectedIndex = XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target1; }
			catch { cboOT1.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target1 = 0; }
			OrderLabelRefresh();
		}
		void cboOT2_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target2 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target2, cboOT2.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT2Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT2Type.SelectedIndex == -1) return;
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target2Type = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target2Type, cboOT2Type.SelectedIndex));
			ComboVarRefresh(XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target2Type, cboOT2);
			try { cboOT2.SelectedIndex = XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target2; }
			catch { cboOT2.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target2 = 0; }
			OrderLabelRefresh();
		}
		void cboOT3_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target3 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target3, cboOT3.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT3Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT3Type.SelectedIndex != -1) return;
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target3Type = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target3Type, cboOT3Type.SelectedIndex));
			ComboVarRefresh(XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target3Type, cboOT3);
			try { cboOT3.SelectedIndex = XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target3; }
			catch { cboOT3.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target3 = 0; }
			OrderLabelRefresh();
		}
		void cboOT4_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target4 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target4, cboOT4.SelectedIndex));
			OrderLabelRefresh();
		}
		void cboOT4Type_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboOT4Type.SelectedIndex == -1) return;
			int r = (int)(numORegion.Value - 1);
			if (!_loading)
				XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target4Type = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target4Type, cboOT4Type.SelectedIndex));
			ComboVarRefresh(XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target4Type, cboOT4);
			try { cboOT4.SelectedIndex = XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target4; }
			catch { cboOT4.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Target4 = 0; }
			OrderLabelRefresh();
		}
		void cboOThrottle_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Throttle = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Throttle, cboOThrottle.SelectedIndex));
		}

		void numORegion_ValueChanged(object sender, EventArgs e)
		{
			for (activeOrder=0;activeOrder<4;activeOrder++) OrderLabelRefresh();
			lblOrderArr_Click(0, new EventArgs());
		}
		void numOSpeed_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Speed = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Speed, numOSpeed.Value));
		}
		void numOVar1_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Variable1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Variable1, numOVar1.Value));
		}
		void numOVar2_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Variable2 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Variable2, numOVar2.Value));
		}
		void numOVar3_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Variable3 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].Variable3, numOVar3.Value));
		}

		void optOT1T2OR_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].T1AndOrT2 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].T1AndOrT2, optOT1T2OR.Checked);
			OrderLabelRefresh();
		}
		void optOT3T4OR_CheckedCHanged(object sender, EventArgs e)
		{
			if (_loading) return;
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].T3AndOrT4 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].T3AndOrT4, optOT3T4OR.Checked);
			OrderLabelRefresh();
		}

		void txtOString_Leave(object sender, EventArgs e)
		{
			int r = (int)(numORegion.Value - 1);
			XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].CustomText = Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, activeOrder].CustomText, txtOString.Text).ToString();
		}
		#endregion
		#region Goals
		void GoalLabelRefresh()
		{
			lblGoal[activeFGGoal].Text = "Goal " + (activeFGGoal + 1).ToString() + ": " + XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].ToString();
		}

		void lblGoalArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			activeFGGoal = Convert.ToByte(l.Tag);
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<8;i++) if (i!=activeFGGoal) lblGoal[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboGoalArgument.SelectedIndex = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Argument;
			cboGoalTrigger.SelectedIndex = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Condition;
			cboGoalAmount.SelectedIndex = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Amount;
			numGoalPoints.Value = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Points;
			chkGoalEnable.Checked = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Enabled;
			numGoalTeam.Value = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Team + 1;
			cboGoalPara.SelectedIndex = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Parameter;
			numGoalActSeq.Value = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].ActiveSequence;
			txtGoalInc.Text = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].IncompleteText;
			txtGoalComp.Text = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].CompleteText;
			txtGoalFail.Text = XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].FailedText;
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
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Enabled = Convert.ToBoolean(Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Enabled, chkGoalEnable.Checked));
			GoalLabelRefresh();
		}

		void grpGoal_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][0] = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][0], cboGoalArgument.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][1] = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][1], cboGoalTrigger.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][2] = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][2], cboGoalAmount.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][6] = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][6], cboGoalPara.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][7] = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal][7], numGoalActSeq.Value));
			GoalLabelRefresh();
		}

		void numGoalPoints_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Points = (short)Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Points, numGoalPoints.Value);
			GoalLabelRefresh();
		}
		void numGoalTeam_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Team = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].Team, numGoalTeam.Value - 1));
		}

		void txtGoalComp_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].CompleteText = Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].CompleteText, txtGoalComp.Text).ToString();
		}
		void txtGoalFail_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].FailedText = Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].FailedText, txtGoalFail.Text).ToString();
		}
		void txtGoalInc_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].IncompleteText = Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[activeFGGoal].IncompleteText, txtGoalInc.Text).ToString();
		}
		#endregion
		#region Waypoints
		void chkWPArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			int index = (int)c.Tag;
			if (index < 4) XwaMission.FlightGroups[activeFG].Waypoints[index].Enabled = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Waypoints[index].Enabled, c.Checked);
			else
			{
				int order = cboWP.SelectedIndex % 4;
				int region = cboWP.SelectedIndex / 4;
				XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[index - 4].Enabled = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[index - 4].Enabled, c.Checked);
			}
		}
		void numWP_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Waypoints[0].Region = (byte)Common.Update(this, XwaMission.FlightGroups[activeFG].Waypoints[0].Region, numSP1.Value-1);
			XwaMission.FlightGroups[activeFG].Waypoints[1].Region = (byte)Common.Update(this, XwaMission.FlightGroups[activeFG].Waypoints[1].Region, numSP2.Value-1);
			XwaMission.FlightGroups[activeFG].Waypoints[2].Region = (byte)Common.Update(this, XwaMission.FlightGroups[activeFG].Waypoints[2].Region, numSP3.Value-1);
			XwaMission.FlightGroups[activeFG].Waypoints[3].Region = (byte)Common.Update(this, XwaMission.FlightGroups[activeFG].Waypoints[3].Region, numHYP.Value-1);
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
					tableOrderRaw.Rows[i][j] = XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[i][j];
					tableOrder.Rows[i][j] = Math.Round((double)XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[i][j] / 160, 2);
				}
				chkWP[i + 4].Checked = XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[i].Enabled;
			}
			tableOrder.AcceptChanges();
			tableOrderRaw.AcceptChanges();
			_loading = btemp;
		}

		void numPitch_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Pitch = (short)Common.Update(this, XwaMission.FlightGroups[activeFG].Pitch, numPitch.Value);
		}
		void numRoll_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Roll = (short)Common.Update(this, XwaMission.FlightGroups[activeFG].Roll, numRoll.Value);
		}
		void numYaw_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Yaw = (short)Common.Update(this, XwaMission.FlightGroups[activeFG].Yaw, numYaw.Value);
		}

		void tableWP_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<4;j++) if (tableWP.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = (short)Math.Round(Convert.ToDouble(tableWP.Rows[j][i]) * 160);
					XwaMission.FlightGroups[activeFG].Waypoints[j][i] = (short)Common.Update(this, XwaMission.FlightGroups[activeFG].Waypoints[j][i], raw);
					tableWPRaw.Rows[j][i] = raw;
				}
			}
			catch { for (i=0;i<3;i++) tableWP.Rows[j][i] = Math.Round((double)(XwaMission.FlightGroups[activeFG].Waypoints[j][i]) / 160, 2); }	// reset
			_loading = false;
		}
		void tableWPRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<4;j++) if (tableWPRaw.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = Convert.ToInt16(tableWPRaw.Rows[j][i]);
					XwaMission.FlightGroups[activeFG].Waypoints[j][i] = (short)Common.Update(this, XwaMission.FlightGroups[activeFG].Waypoints[j][i], raw);
					tableWP.Rows[j][i] = Math.Round((double)raw / 160, 2);
				}
			}
			catch { for (i=0;i<3;i++) tableWPRaw.Rows[j][i] = XwaMission.FlightGroups[activeFG].Waypoints[j][i]; }
			_loading = false;
		}
		void tableOrder_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<8;j++) if (tableOrder.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			int order = cboWP.SelectedIndex % 4;
			int region = cboWP.SelectedIndex / 4;
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = (short)Math.Round(Convert.ToDouble(tableOrder.Rows[j][i]) * 160);
					XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[j][i] = (short)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[j][i], raw);
					tableOrderRaw.Rows[j][i] = raw;
				}
			}
			catch { for (i=0;i<3;i++) tableOrder.Rows[j][i] = Math.Round((double)(XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[j][i]) / 160, 2); }	// reset
			_loading = false;
		}
		void tableOrderRaw_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i, j=0;
			if (_loading) return;
			_loading = true;
			for (j=0;j<8;j++) if (tableOrderRaw.Rows[j].Equals(e.Row)) break;	//find the row index that you're changing
			int order = cboWP.SelectedIndex % 4;
			int region = cboWP.SelectedIndex / 4;
			try
			{
				for (i=0;i<3;i++)
				{
					short raw = Convert.ToInt16(tableOrderRaw.Rows[j][i]);
					XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[j][i] = (short)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[j][i], raw);
					tableOrder.Rows[j][i] = Math.Round((double)raw / 160, 2);
				}
			}
			catch { for (i = 0; i < 3; i++) tableOrderRaw.Rows[j][i] = XwaMission.FlightGroups[activeFG].Orders[region, order].Waypoints[j][i]; }
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
			lblOptCraft[activeOptionCraft].Text = "Craft " + (activeOptionCraft+1).ToString() + ":";
			if (XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].CraftType != 0)
				lblOptCraft[activeOptionCraft].Text += " " + (XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfWaves+1).ToString()
				+ " x (" + (XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfCraft+1).ToString() + ") " + Strings.CraftType[cboOptCraft.SelectedIndex];
		}

		void chkOptArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			CheckBox c = (CheckBox)sender;
			int i = (int)c.Tag;
			XwaMission.FlightGroups[activeFG].OptLoadout[i] = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].OptLoadout[i], c.Checked);
			bool btemp = _loading;
			_loading = true;
			if (c.Checked)
			{
				if (i == 0) for (int j=1;j<8;j++) XwaMission.FlightGroups[activeFG].OptLoadout[j] = false;	// turn off warheads
				else if (i > 0 && i < 8) XwaMission.FlightGroups[activeFG].OptLoadout[0] = false;		// clear warhead None
				else if (i == 8) for (int j=9;j<12;j++) XwaMission.FlightGroups[activeFG].OptLoadout[j] = false;	// turn off beams
				else if (i > 8 && i < 12) XwaMission.FlightGroups[activeFG].OptLoadout[8] = false;	// clear beam None
				else if (i == 12) { XwaMission.FlightGroups[activeFG].OptLoadout[13] = false; XwaMission.FlightGroups[activeFG].OptLoadout[14] = false; }	// turn off CMs
				else XwaMission.FlightGroups[activeFG].OptLoadout[12] = false;	// clear CM None
			}
			else
			{
				bool b = false;
				if (i > 0 && i < 8) for (i=1;i<8;i++) b |= XwaMission.FlightGroups[activeFG].OptLoadout[i];
				if (!b && !chkOpt[0].Checked) XwaMission.FlightGroups[activeFG].OptLoadout[0] = true;
				b = false;
				if (i > 8 && i < 12) for (i=9;i<12;i++) b |= XwaMission.FlightGroups[activeFG].OptLoadout[i];
				if (!b && !chkOpt[8].Checked) XwaMission.FlightGroups[activeFG].OptLoadout[8] = true;
				b = false;
				if (i > 12 && i < 15) for (i=13;i<15;i++) b |= XwaMission.FlightGroups[activeFG].OptLoadout[i];
				if (!b && !chkOpt[12].Checked) XwaMission.FlightGroups[activeFG].OptLoadout[12] = true;
			}
			for (i=0;i<15;i++) chkOpt[i].Checked = XwaMission.FlightGroups[activeFG].OptLoadout[i];
			_loading = btemp;
		}
		void lblOptCraftArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			activeOptionCraft = Convert.ToByte(l.Tag);
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<10;i++) if (i!=activeOptionCraft) lblOptCraft[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboOptCraft.SelectedIndex = XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].CraftType;
			numOptCraft.Value = XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfCraft+1;
			numOptWaves.Value = XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfWaves+1;
			_loading = btemp;
		}
		void lblSkipTrigArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			Label ll = (l == lblSkipTrig1 ? lblSkipTrig2 : lblSkipTrig1);
			int i = 0, r = 0, o = 0;
			l.Focus();
			i = (l == lblSkipTrig1 ? 0 : 1);
			r = cboSkipOrder.SelectedIndex / 4;
			o = cboSkipOrder.SelectedIndex % 4;
			Mission.Trigger trigger = XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i];
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
			menuPaste_Click("Skip", new EventArgs());
		}
		void lblSkipTrigArr_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) menuCopy_Click("Skip", new EventArgs());
		}

		void cboOptCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
				XwaMission.FlightGroups[activeFG].OptCraftCategory = (FlightGroup.OptionalCraftCategory)Common.Update(this, (byte)XwaMission.FlightGroups[activeFG].OptCraftCategory, cboOptCat.SelectedIndex);
			if (cboOptCat.SelectedIndex == 4) EnableOptCat(true);
			else EnableOptCat(false);
		}
		void cboOptCraft_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].CraftType = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].CraftType, cboOptCraft.SelectedIndex));
			OptCraftLabelRefresh();
		}
		void cboSkipAmount_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Amount = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Amount, cboSkipAmount.SelectedIndex));
			LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}
		void cboSkipOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[0], lblSkipTrig1);
			LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[1], lblSkipTrig2);
			lblSkipTrigArr_Click(lblSkipTrig1, new EventArgs());
			optSkipOR.Checked = XwaMission.FlightGroups[activeFG].Orders[r, o].SkipT1AndOrT2;
			optSkipAND.Checked = !optSkipOR.Checked;
		}
		void cboSkipPara_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Parameter1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Parameter1, cboSkipPara.SelectedIndex));
			LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}
		void cboSkipTrig_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Condition = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Condition, cboSkipTrig.SelectedIndex));
			LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}
		void cboSkipType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboSkipType.SelectedIndex == -1) return;
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			if (!_loading)
				XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].VariableType = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].VariableType, cboSkipType.SelectedIndex));
			ComboVarRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].VariableType, cboSkipVar);
			try { cboSkipVar.SelectedIndex = XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Variable; }
			catch { cboSkipVar.SelectedIndex = 0; XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Variable = 0; }
			LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i], (i == 0 ? lblSkipTrig1 : lblSkipTrig2));
		}
		void cboSkipVar_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Variable = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Variable, cboSkipVar.SelectedIndex));
			LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}

		void chkRole1_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				XwaMission.FlightGroups[activeFG].EnableDesignation1 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].EnableDesignation1, chkRole1.Checked);
			cboRole1.Enabled = chkRole1.Checked;
		}
		void chkRole2_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loading)
				XwaMission.FlightGroups[activeFG].EnableDesignation2 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].EnableDesignation2, chkRole2.Checked);
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
			XwaMission.FlightGroups[activeFG].Role = Common.Update(this, XwaMission.FlightGroups[activeFG].Role, txtRole.Text).ToString();
			XwaMission.FlightGroups[activeFG].Designation1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Designation1, cboRole1.SelectedIndex));
			XwaMission.FlightGroups[activeFG].Designation2 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Designation2, cboRole2.SelectedIndex));
		}

		void numOptCraft_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfCraft = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfCraft, numOptCraft.Value-1));
			OptCraftLabelRefresh();
		}
		void numOptWaves_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfWaves = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].OptCraft[activeOptionCraft].NumberOfWaves, numOptWaves.Value-1));
			OptCraftLabelRefresh();
		}
		void numSkipPara_Leave(object sender, EventArgs e)
		{
			int i = (lblSkipTrig1.ForeColor == SystemColors.Highlight ? 0 : 1);
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Parameter2 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i].Parameter2, numSkipPara.Value));
			LabelRefresh(XwaMission.FlightGroups[activeFG].Orders[r, o].SkipTriggers[i], (i == 1 ? lblSkipTrig2 : lblSkipTrig1));
		}

		void optSkipOR_Leave(object sender, EventArgs e)
		{
			int r = cboSkipOrder.SelectedIndex / 4;
			int o = cboSkipOrder.SelectedIndex % 4;
			XwaMission.FlightGroups[activeFG].Orders[r, o].SkipT1AndOrT2 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].SkipT1AndOrT2, optSkipOR.Checked);
		}

		void txtPilot_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].PilotID = Common.Update(this, XwaMission.FlightGroups[activeFG].PilotID, txtPilot.Text).ToString();
		}
		#endregion
		#region Unknowns
		void cboUnkOrder_Enter(object sender, EventArgs e)
		{	// use Enter to detect when user is about to change index, save what's there
			int r = cboUnkOrder.SelectedIndex / 4;
			int o = cboUnkOrder.SelectedIndex % 4;
			XwaMission.FlightGroups[activeFG].Orders[r,o].Unknown9 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r,o].Unknown9, numUnk9.Value));
			XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown10 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown10, numUnk10.Value));
			XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown11 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown11, chkUnk11.Checked);
			XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown12 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown12, chkUnk12.Checked);
			XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown13 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown13, chkUnk13.Checked);
			XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown14 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown14, chkUnk14.Checked);
		}
		void cboUnkOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			int r = cboUnkOrder.SelectedIndex / 4;
			int o = cboUnkOrder.SelectedIndex % 4;
			numUnk9.Value = XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown9;
			numUnk10.Value = XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown10;
			chkUnk11.Checked = XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown11;
			chkUnk12.Checked = XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown12;
			chkUnk13.Checked = XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown13;
			chkUnk14.Checked = XwaMission.FlightGroups[activeFG].Orders[r, o].Unknown14;
		}

		void chkUnk15_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Goals[(int)numUnkGoal.Value-1].Unknown15 = Convert.ToBoolean(Common.Update(this, XwaMission.FlightGroups[activeFG].Goals[(int)numUnkGoal.Value-1].Unknown15, chkUnk15.Checked));
		}

		void grpUnkAD_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown5 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown5, numUnk5.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown6 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown6, chkUnk6.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown7 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown7, numUnk7.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown8 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown8, numUnk8.Value));
		}
		void grpUnkCraft_Leave(object sender, EventArgs e)
		{
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown1 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown1, numUnk1.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown3 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown3, numUnk3.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown4 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown4, numUnk4.Value));
		}
		void grpUnkOther_Leave(object sender, EventArgs e)
		{	// okay, lots of stuff :P
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown16 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown16, numUnk16.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown17 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown17, numUnk17.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown18 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown18, numUnk18.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown19 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown19, numUnk19.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown20 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown20, numUnk20.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown21 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown21, numUnk21.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown22 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown22, chkUnk22.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown23 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown23, numUnk23.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown24 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown24, numUnk24.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown25 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown25, numUnk25.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown26 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown26, numUnk26.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown27 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown27, numUnk27.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown28 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown28, numUnk28.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown29 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown29, chkUnk29.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown30 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown30, chkUnk30.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown31 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown31, chkUnk31.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown32 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown32, numUnk32.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown33 = Convert.ToByte(Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown33, numUnk33.Value));
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown34 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown34, chkUnk34.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown35 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown35, chkUnk35.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown36 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown36, chkUnk36.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown37 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown37, chkUnk37.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown38 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown38, chkUnk38.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown39 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown39, chkUnk39.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown40 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown40, chkUnk40.Checked);
			XwaMission.FlightGroups[activeFG].Unknowns.Unknown41 = (bool)Common.Update(this, XwaMission.FlightGroups[activeFG].Unknowns.Unknown41, chkUnk31.Checked);
		}
		void grpUnkOrder_Leave(object sender, EventArgs e)
		{
			cboUnkOrder_Enter("grpUnkOrder_Leave", new EventArgs());	// meh, no need to type it out again
		}

		void numUnkGoal_ValueChanged(object sender, EventArgs e)
		{
			chkUnk15.Checked = XwaMission.FlightGroups[activeFG].Goals[(int)numUnkGoal.Value-1].Unknown15;
		}
		#endregion
		#endregion
		#region Messages
		void DeleteMess()
		{
			activeMessage = XwaMission.Messages.RemoveAt(activeMessage);
			if (XwaMission.Messages.Count == 0)
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
			grpMessCancel.Enabled = state;
			grpMessUnk.Enabled = state;
			txtMessage.Enabled = state;
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
			if (XwaMission.Messages.Count == 0) return;
			lstMessages.Items[activeMessage] = XwaMission.Messages[activeMessage].MessageString;
		}
		void NewMess()
		{
			if (XwaMission.Messages.Count == Mission.MessageLimit)
			{
				MessageBox.Show("Mission contains maximum number of Messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			activeMessage = XwaMission.Messages.Add();
			if (XwaMission.Messages.Count == 1) EnableMessages(true);
			lstMessages.Items.Add(XwaMission.Messages[activeMessage].MessageString);
			lstMessages.SelectedIndex = activeMessage;
			Common.Title(this, _loading);
		}

		void lstMessages_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (XwaMission.Messages.Count == 0 || XwaMission.Messages[e.Index] == null) return;
			e.DrawBackground();
			Brush brText = SystemBrushes.ControlText;
			switch (XwaMission.Messages[e.Index].Color)
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
			activeMessage = lstMessages.SelectedIndex;
			lblMessage.Text = "Message #" + (activeMessage+1).ToString() + " of " + XwaMission.Messages.Count.ToString();
			bool btemp = _loading;
			_loading = true;
			for (int i=0;i<6;i++) LabelRefresh(XwaMission.Messages[activeMessage].Triggers[i], lblMessTrig[i]);
			txtMessage.Text = XwaMission.Messages[activeMessage].MessageString;
			txtMessNote.Text = XwaMission.Messages[activeMessage].Note;
			cboMessColor.SelectedIndex = XwaMission.Messages[activeMessage].Color;
			for (int i=0;i<4;i++)
			{
				optMessAndOr[i].Checked = XwaMission.Messages[activeMessage].TrigAndOr[i];
				optMessAndOr[i+4].Checked = !optMessAndOr[i].Checked;
			}
			numMessDelaySec.Value = XwaMission.Messages[activeMessage].DelaySeconds;
			numMessDelayMin.Value = XwaMission.Messages[activeMessage].DelayMinutes;
			for (int i=0;i<10;i++) chkSendTo[i].Checked = XwaMission.Messages[activeMessage].SentTo[i];
			numMessUnk1.Value = XwaMission.Messages[activeMessage].Unknown1;
			chkMessUnk2.Checked = XwaMission.Messages[activeMessage].Unknown2;
			txtVoice.Text = XwaMission.Messages[activeMessage].VoiceID;
			cboMessFG.SelectedIndex = XwaMission.Messages[activeMessage].OriginatingFG;
			lblMessTrigArr_Click(lblMessTrig[0], new EventArgs());
			_loading = btemp;
		}

		void chkSendToArr_Leave(object sender, EventArgs e)
		{
			CheckBox c = (CheckBox)sender;
			XwaMission.Messages[activeMessage].SentTo[(int)c.Tag] = (bool)Common.Update(this, XwaMission.Messages[activeMessage].SentTo[(int)c.Tag], c.Checked);
		}
		void lblMessTrigArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			activeMessageTrigger = Convert.ToByte(l.Tag);
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<6;i++) if (i!=activeMessageTrigger) lblMessTrig[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			cboMessTrig.SelectedIndex = XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Condition;
			cboMessType.SelectedIndex = -1;
			cboMessType.SelectedIndex = XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].VariableType;
			cboMessAmount.SelectedIndex = XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Amount;
			cboMessPara.SelectedIndex = XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Parameter1;
			numMessPara.Value = XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Parameter2;
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
			XwaMission.Messages[activeMessage].TrigAndOr[(int)r.Tag] = (bool)Common.Update(this, XwaMission.Messages[activeMessage].TrigAndOr[(int)r.Tag], r.Checked);
		}

		void cboMessAmount_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Amount = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Amount, cboMessAmount.SelectedIndex));
			LabelRefresh(XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}
		void cboMessColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			XwaMission.Messages[activeMessage].Color = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].Color, cboMessColor.SelectedIndex));
			MessListRefresh();
		}
		void cboMessFG_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].OriginatingFG = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].OriginatingFG, cboMessFG.SelectedIndex));
		}
		void cboMessPara_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Parameter1 = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Parameter1, cboMessPara.SelectedIndex));
			LabelRefresh(XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}
		void cboMessTrig_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Condition = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Condition, cboMessTrig.SelectedIndex));
			LabelRefresh(XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}
		void cboMessType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMessType.SelectedIndex == -1) return;
			if (!_loading)
				XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].VariableType = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].VariableType, cboMessType.SelectedIndex));
			ComboVarRefresh(XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].VariableType, cboMessVar);
			try { cboMessVar.SelectedIndex = XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Variable; }
			catch { cboMessVar.SelectedIndex = 0; XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Variable = 0; }
			LabelRefresh(XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}
		void cboMessVar_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Variable = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Variable, cboMessVar.SelectedIndex));
			LabelRefresh(XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);
		}

		void chkMessUnk2_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].Unknown2 = (bool)Common.Update(this, XwaMission.Messages[activeMessage].Unknown2, chkMessUnk2.Checked);
		}

		void numMessDelayMin_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].DelayMinutes = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].DelayMinutes, numMessDelayMin.Value));
		}
		void numMessDelaySec_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].DelaySeconds = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].DelaySeconds, numMessDelaySec.Value));
		}
		void numMessPara_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Parameter2 = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger].Parameter2, numMessPara.Value-1));
			LabelRefresh(XwaMission.Messages[activeMessage].Triggers[activeMessageTrigger], lblMessTrig[activeMessageTrigger]);	// don't know if I need this...
		}
		void numMessUnk1_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].Unknown1 = Convert.ToByte(Common.Update(this, XwaMission.Messages[activeMessage].Unknown1, numMessUnk1.Value));
		}

		void txtMessage_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].MessageString = Common.Update(this, XwaMission.Messages[activeMessage].MessageString, txtMessage.Text).ToString();
			MessListRefresh();
		}
		void txtVoice_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].VoiceID = Common.Update(this, XwaMission.Messages[activeMessage].VoiceID, txtVoice.Text).ToString();
		}
		void txtMessNote_Leave(object sender, EventArgs e)
		{
			XwaMission.Messages[activeMessage].Note = Common.Update(this, XwaMission.Messages[activeMessage].Note, txtMessNote.Text).ToString();
		}
		#endregion
		#region Globals
		void cboGlobalTeam_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalTeam.SelectedIndex == -1) return;
			if (!_loading) lblGlobTrigArr_Click(lblGlobTrig[activeTeam], new EventArgs());	// re-click current lbl with save
			activeTeam = (byte)cboGlobalTeam.SelectedIndex;
			lblTeamArr_Click(lblTeam[activeTeam], new EventArgs());	// link the Globals and Team tabs to share GlobTeam
			bool btemp = _loading;
			_loading = true;
			for (int i=0;i<12;i++) LabelRefresh(XwaMission.Globals[activeTeam].Goals[i/4].Triggers[i%4], lblGlobTrig[i]);
			for (int i = 0; i < 9; i++)
			{
				optGlobAndOr[i * 2].Checked = XwaMission.Globals[activeTeam].Goals[i / 3].AndOr[i % 3];	// OR
				optGlobAndOr[i * 2 + 1].Checked = !optGlobAndOr[i * 2].Checked;	// AND
			}
			lblGlobTrigArr_Click(lblGlobTrig[0], new EventArgs());	// click first lbl with no save
			_loading = btemp;
		}

		void lblGlobTrigArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			activeGlobalTrigger = Convert.ToByte(l.Tag);
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<12;i++) if (i!=activeGlobalTrigger) lblGlobTrig[i].ForeColor = SystemColors.ControlText;
			int goal = activeGlobalTrigger / 4;
			int trig = activeGlobalTrigger % 4;
			bool btemp = _loading;
			_loading = true;
			cboGlobalTrig.SelectedIndex = XwaMission.Globals[activeTeam].Goals[goal].Triggers[trig].Condition;
			cboGlobalType.SelectedIndex = -1;
			cboGlobalType.SelectedIndex = XwaMission.Globals[activeTeam].Goals[goal].Triggers[trig].VariableType;
			cboGlobalAmount.SelectedIndex = XwaMission.Globals[activeTeam].Goals[goal].Triggers[trig].Amount;
			cboGlobalPara.SelectedIndex = XwaMission.Globals[activeTeam].Goals[goal].Triggers[trig].Parameter1 + 1;
			numGlobalPara.Value = XwaMission.Globals[activeTeam].Goals[goal].Triggers[trig].Parameter2;
			numGlobalPoints.Value = XwaMission.Globals[activeTeam].Goals[goal].Points;
			txtGlobalInc.Text = XwaMission.Globals[activeTeam].Goals[goal].GoalStrings[trig, (int)Globals.GoalState.Incomplete];
			txtGlobalComp.Text = XwaMission.Globals[activeTeam].Goals[goal].GoalStrings[trig, (int)Globals.GoalState.Complete];
			txtGlobalFail.Text = XwaMission.Globals[activeTeam].Goals[goal].GoalStrings[trig, (int)Globals.GoalState.Failed];
			numGlobActSeq.Value = XwaMission.Globals[activeTeam].Goals[goal].ActiveSequence;
			chkGlobUnk1.Checked = XwaMission.Globals[activeTeam].Goals[goal].Unknown1;
			chkGlobUnk2.Checked = XwaMission.Globals[activeTeam].Goals[goal].Unknown2;
			numGlobUnk3.Value = XwaMission.Globals[activeTeam].Goals[goal].Unknown3;
			numGlobUnk4.Value = XwaMission.Globals[activeTeam].Goals[goal].Unknown4;
			numGlobUnk5.Value = XwaMission.Globals[activeTeam].Goals[goal].Unknown5;
			numGlobUnk6.Value = XwaMission.Globals[activeTeam].Goals[goal].Unknown6;
			txtGlobalFail.Visible = (goal < 1);
			txtGlobalInc.Visible = (goal < 2);
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
			int goal = (int)r.Tag / 6;
			int index = ((int)r.Tag / 2) % 3;
			XwaMission.Globals[activeTeam].Goals[goal].AndOr[index] = (bool)Common.Update(this, XwaMission.Globals[activeTeam].Goals[goal].AndOr[index], r.Checked);
		}

		void cboGlobalAmount_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger/4].Triggers[activeGlobalTrigger%4].Amount = Convert.ToByte(Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger/4].Triggers[activeGlobalTrigger%4].Amount, cboGlobalAmount.SelectedIndex));
			LabelRefresh(XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger/4].Triggers[activeGlobalTrigger%4], lblGlobTrig[activeGlobalTrigger]);
		}
		void cboGlobalPara_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Parameter1 = Convert.ToByte(Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Parameter1, cboGlobalPara.SelectedIndex));
			LabelRefresh(XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4], lblGlobTrig[activeGlobalTrigger]);
		}
		void cboGlobalTrig_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Condition = Convert.ToByte(Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Condition, cboGlobalTrig.SelectedIndex));
			LabelRefresh(XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4], lblGlobTrig[activeGlobalTrigger]);
		}
		void cboGlobalType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboGlobalType.SelectedIndex == -1) return;
			if (!_loading)
				XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].VariableType = Convert.ToByte(Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].VariableType, cboGlobalType.SelectedIndex));
			ComboVarRefresh(XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].VariableType, cboGlobalVar);
			try { cboGlobalVar.SelectedIndex = XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Variable; }
			catch { cboGlobalVar.SelectedIndex = 0; XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Variable = 0; }
			LabelRefresh(XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4], lblGlobTrig[activeGlobalTrigger]);
		}
		void cboGlobalVar_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Variable = Convert.ToByte(Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Variable, cboGlobalVar.SelectedIndex));
			LabelRefresh(XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4], lblGlobTrig[activeGlobalTrigger]);
		}

		void numGlobalPara_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Parameter2 = Convert.ToByte(Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4].Parameter2, numGlobalPara.Value));
			LabelRefresh(XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].Triggers[activeGlobalTrigger % 4], lblGlobTrig[activeGlobalTrigger]);
		}
		void numGlobalPoints_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger/4].Points = (short)Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger/4].Points, (short)numGlobalPoints.Value);
		}

		void txtGlobalComp_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].GoalStrings[activeGlobalTrigger % 4, (int)Globals.GoalState.Complete] = Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].GoalStrings[activeGlobalTrigger % 4, (int)Globals.GoalState.Complete], txtGlobalComp.Text).ToString();
		}
		void txtGlobalFail_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].GoalStrings[activeGlobalTrigger % 4, (int)Globals.GoalState.Failed] = Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].GoalStrings[activeGlobalTrigger % 4, (int)Globals.GoalState.Failed], txtGlobalFail.Text).ToString();
		}
		void txtGlobalInc_Leave(object sender, EventArgs e)
		{
			XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].GoalStrings[activeGlobalTrigger % 4, (int)Globals.GoalState.Incomplete] = Common.Update(this, XwaMission.Globals[activeTeam].Goals[activeGlobalTrigger / 4].GoalStrings[activeGlobalTrigger % 4, (int)Globals.GoalState.Incomplete], txtGlobalInc.Text).ToString();
		}
		#endregion
		#region Teams
		void TeamRefresh()
		{
			string team = XwaMission.Teams[activeTeam].Name;
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
		}

		void lblTeamArr_Click(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			l.Focus();
			activeTeam = Convert.ToByte(l.Tag);
			cboGlobalTeam.SelectedIndex = activeTeam;
			l.ForeColor = SystemColors.Highlight;
			for (int i=0;i<10;i++) if (i!=activeTeam) lblTeam[i].ForeColor = SystemColors.ControlText;
			bool btemp = _loading;
			_loading = true;
			txtTeamName.Text = XwaMission.Teams[activeTeam].Name;
			for (int i=0;i<10;i++)
			{
				if (XwaMission.Teams[activeTeam].Allies[i] == Team.Allegeance.Hostile) optAllies[i*3].Checked = true;
				else if (XwaMission.Teams[activeTeam].Allies[i] == Team.Allegeance.Friendly) optAllies[i*3+1].Checked = true;
				else optAllies[i*3+2].Checked = true;
			}
			txtPMCVoiceID.Text = XwaMission.Teams[activeTeam].VoiceIDs[0];
			txtPMFVoiceID.Text = XwaMission.Teams[activeTeam].VoiceIDs[1];
			txtOMCVoiceID.Text = XwaMission.Teams[activeTeam].VoiceIDs[2];
			txtPrimCompNote.Text = XwaMission.Teams[activeTeam].EomNotes[0];
			txtPrimFailNote.Text = XwaMission.Teams[activeTeam].EomNotes[1];
			txtSecCompNote.Text = XwaMission.Teams[activeTeam].EomNotes[2];
			for (int i=0;i<6;i++)
			{
				txtEoM[i].Text = XwaMission.Teams[activeTeam].EndOfMissionMessages[i];
				numTeamUnk[i].Value = XwaMission.Teams[activeTeam].Unknowns[i];
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
			XwaMission.Teams[activeTeam].Allies[index/3] = (Team.Allegeance)Common.Update(this, XwaMission.Teams[activeTeam].Allies[index/3], a);
		}

		void grpTeamOMC_Leave(object sender, EventArgs e)
		{
			XwaMission.Teams[activeTeam].VoiceIDs[2] = Common.Update(this, XwaMission.Teams[activeTeam].VoiceIDs[2], txtOMCVoiceID.Text).ToString();
			XwaMission.Teams[activeTeam].EndOfMissionMessages[4] = Common.Update(this, XwaMission.Teams[activeTeam].EndOfMissionMessages[4], txtEoM[4].Text).ToString();
			XwaMission.Teams[activeTeam].EndOfMissionMessages[5] = Common.Update(this, XwaMission.Teams[activeTeam].EndOfMissionMessages[5], txtEoM[5].Text).ToString();
			XwaMission.Teams[activeTeam].EomNotes[2] = Common.Update(this, XwaMission.Teams[activeTeam].EomNotes[2], txtSecCompNote.Text).ToString();
		}
		void grpTeamPMC_Leave(object sender, EventArgs e)
		{
			XwaMission.Teams[activeTeam].VoiceIDs[0] = Common.Update(this, XwaMission.Teams[activeTeam].VoiceIDs[0], txtPMCVoiceID.Text).ToString();
			XwaMission.Teams[activeTeam].EndOfMissionMessages[0] = Common.Update(this, XwaMission.Teams[activeTeam].EndOfMissionMessages[0], txtEoM[0].Text).ToString();
			XwaMission.Teams[activeTeam].EndOfMissionMessages[1] = Common.Update(this, XwaMission.Teams[activeTeam].EndOfMissionMessages[1], txtEoM[1].Text).ToString();
			XwaMission.Teams[activeTeam].EomNotes[0] = Common.Update(this, XwaMission.Teams[activeTeam].EomNotes[0], txtPrimCompNote.Text).ToString();
		}
		void grpTeamPMF_Leave(object sender, EventArgs e)
		{
			XwaMission.Teams[activeTeam].VoiceIDs[1] = Common.Update(this, XwaMission.Teams[activeTeam].VoiceIDs[1], txtPMFVoiceID.Text).ToString();
			XwaMission.Teams[activeTeam].EndOfMissionMessages[2] = Common.Update(this, XwaMission.Teams[activeTeam].EndOfMissionMessages[2], txtEoM[2].Text).ToString();
			XwaMission.Teams[activeTeam].EndOfMissionMessages[3] = Common.Update(this, XwaMission.Teams[activeTeam].EndOfMissionMessages[3], txtEoM[3].Text).ToString();
			XwaMission.Teams[activeTeam].EomNotes[1] = Common.Update(this, XwaMission.Teams[activeTeam].EomNotes[1], txtPrimFailNote.Text).ToString();
		}
		void grpTeamUnknowns_Leave(object sender, EventArgs e)
		{
			for(int i=0;i<6;i++)
				XwaMission.Teams[activeTeam].Unknowns[i] = Convert.ToByte(Common.Update(this, XwaMission.Teams[activeTeam].Unknowns[i], numTeamUnk[i].Value));
		}

		void txtTeamName_Leave(object sender, EventArgs e)
		{
			XwaMission.Teams[activeTeam].Name = Common.Update(this, XwaMission.Teams[activeTeam].Name, txtTeamName.Text).ToString();
			TeamRefresh();
		}
		#endregion
		#region Mission
		void UpdateMissionTabs()
		{
			#region M1
			txtMissDesc.Text = XwaMission.MissionDescription;
			txtMissSucc.Text = XwaMission.MissionSuccessful;
			txtMissFail.Text = XwaMission.MissionFailed;
			txtDescNote.Text = XwaMission.DescriptionNotes;
			txtFailNote.Text = XwaMission.FailedNotes;
			txtSuccNote.Text = XwaMission.SuccessfulNotes;
			cboHangar.SelectedIndex = (int)XwaMission.MissionType;
			cboOfficer.SelectedIndex = XwaMission.Officer;
			try { cboLogo.SelectedIndex = (int)XwaMission.Logo - 4; }
			catch { XwaMission.Logo = Mission.LogoEnum.None; cboLogo.SelectedIndex = 4; }
			chkMissUnk1.Checked = XwaMission.Unknown1;
			chkMissUnk2.Checked = XwaMission.Unknown2;
			numMissUnk3.Value = XwaMission.Unknown3;
			numMissUnk4.Value = XwaMission.Unknown4;
			numMissUnk5.Value = XwaMission.Unknown5;
			numMissTimeMin.Value = XwaMission.TimeLimitMin;
			chkEnd.Checked = XwaMission.EndWhenComplete;
			#endregion
			#region M2
			for (int i=0;i<4;i++)
			{
				IFFs[i+2] = XwaMission.Iffs[i+2];
				txtIFFs[i].Text = XwaMission.Iffs[i + 2];
				txtRegions[i].Text = XwaMission.Regions[i];
			}
			numGlobCargo.Value = 2;
			numGlobCargo.Value = 1;	// force the refresh
			txtNotes.Text = XwaMission.MissionNotes;
			GGRefresh();
			#endregion
		}

		void cboHangar_Leave(object sender, EventArgs e)
		{
			XwaMission.MissionType = (Mission.HangarEnum)(int)Common.Update(this, (int)XwaMission.MissionType, cboHangar.SelectedIndex);
		}
		void cboLogo_Leave(object sender, EventArgs e)
		{
			XwaMission.Logo = (Mission.LogoEnum)(int)Common.Update(this, (int)XwaMission.Logo, cboLogo.SelectedIndex + 4);
		}
		void cboOfficer_Leave(object sender, EventArgs e)
		{
			XwaMission.Officer = Convert.ToByte(Common.Update(this, XwaMission.Officer, cboOfficer.SelectedIndex));
		}

		void chkEnd_Leave(object sender, EventArgs e)
		{
			XwaMission.EndWhenComplete = (bool)Common.Update(this, XwaMission.EndWhenComplete, chkEnd.Checked);
		}
		void chkMissUnk1_Leave(object sender, EventArgs e)
		{
			XwaMission.Unknown1 = (bool)Common.Update(this, XwaMission.Unknown1, chkMissUnk1.Checked);
		}
		void chkMissUnk2_Leave(object sender, EventArgs e)
		{
			XwaMission.Unknown2 = (bool)Common.Update(this, XwaMission.Unknown2, chkMissUnk2.Checked);
		}

		void numMissTimeMin_Leave(object sender, EventArgs e)
		{
			XwaMission.TimeLimitMin = Convert.ToByte(Common.Update(this, XwaMission.TimeLimitMin, numMissTimeMin.Value));
		}
		void numMissUnk3_Leave(object sender, EventArgs e)
		{
			XwaMission.Unknown3 = Convert.ToByte(Common.Update(this, XwaMission.Unknown3, numMissUnk3.Value));
		}
		void numMissUnk4_Leave(object sender, EventArgs e)
		{
			XwaMission.Unknown4 = Convert.ToByte(Common.Update(this, XwaMission.Unknown4, numMissUnk4.Value));
		}
		void numMissUnk5_Leave(object sender, EventArgs e)
		{
			XwaMission.Unknown5 = Convert.ToByte(Common.Update(this, XwaMission.Unknown5, numMissUnk5.Value));
		}

		void txtMissDesc_Leave(object sender, EventArgs e)
		{
			XwaMission.MissionDescription = Common.Update(this, XwaMission.MissionDescription, txtMissDesc.Text).ToString();
		}
		void txtMissFail_Leave(object sender, EventArgs e)
		{
			XwaMission.MissionFailed = Common.Update(this, XwaMission.MissionFailed, txtMissFail.Text).ToString();
		}
		void txtMissSucc_Leave(object sender, EventArgs e)
		{
			XwaMission.MissionSuccessful = Common.Update(this, XwaMission.MissionSuccessful, txtMissSucc.Text).ToString();
		}
		void txtDescNote_Leave(object sender, EventArgs e)
		{
			XwaMission.DescriptionNotes = Common.Update(this, XwaMission.DescriptionNotes, txtDescNote.Text).ToString();
		}
		void txtSuccNote_Leave(object sender, EventArgs e)
		{
			XwaMission.SuccessfulNotes = Common.Update(this, XwaMission.SuccessfulNotes, txtSuccNote.Text).ToString();
		}
		void txtFailNote_Leave(object sender, EventArgs e)
		{
			XwaMission.FailedNotes = Common.Update(this, XwaMission.FailedNotes, txtFailNote.Text).ToString();
		}
		#endregion
		#region Mission2
		void GGRefresh()
		{
			for (int i=0;i<16;i++)
				if (XwaMission.GlobalGroups[i] != "") lblGG[i].Text = XwaMission.GlobalGroups[i];
				else lblGG[i].Text = "Global Group " + (i+1).ToString();
		}

		void numGlobCargo_ValueChanged(object sender, EventArgs e)
		{
			int i = (int)numGlobCargo.Value - 1;
			txtGlobCargo.Text = XwaMission.GlobalCargo[i].Cargo;
			chkGCUnk1.Checked = XwaMission.GlobalCargo[i].Unknown1;
			numGCUnk2.Value = XwaMission.GlobalCargo[i].Unknown2;
			numGCUnk3.Value = XwaMission.GlobalCargo[i].Unknown3;
			numGCUnk4.Value = XwaMission.GlobalCargo[i].Unknown4;
			numGCUnk5.Value = XwaMission.GlobalCargo[i].Unknown5;
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
				System.Diagnostics.Debug.WriteLine("InvalidCastException - lblGGArr_Click");
				i = (int)sender;
				l = lblGG[i];
			}
			txtGlobGroup.Text = XwaMission.GlobalGroups[i];
			l.ForeColor = SystemColors.Highlight;
			for (i=0;i<16;i++) if (lblGG[i] != l) lblGG[i].ForeColor = SystemColors.ControlText;
		}
		void txtIFFsArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			XwaMission.Iffs[(int)t.Tag] = Common.Update(this, XwaMission.Iffs[(int)t.Tag], t.Text).ToString();
			IFFs[(int)t.Tag] = t.Text;
			ComboReset(cboIFF, IFFs, cboIFF.SelectedIndex);
		}
		void txtRegionsArr_Leave(object sender, EventArgs e)
		{
			TextBox t = (TextBox)sender;
			XwaMission.Regions[(int)t.Tag] = Common.Update(this, XwaMission.Regions[(int)t.Tag], t.Text).ToString();
		}

		void chkGCUnk1_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			XwaMission.GlobalCargo[gc].Unknown1 = (bool)Common.Update(this, XwaMission.GlobalCargo[gc].Unknown1, chkGCUnk1.Checked);
		}

		void numGCUnk2_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			XwaMission.GlobalCargo[gc].Unknown2 = Convert.ToByte(Common.Update(this, XwaMission.GlobalCargo[gc].Unknown2, numGCUnk2.Value));
		}
		void numGCUnk3_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			XwaMission.GlobalCargo[gc].Unknown3 = Convert.ToByte(Common.Update(this, XwaMission.GlobalCargo[gc].Unknown3, numGCUnk3.Value));
		}
		void numGCUnk4_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			XwaMission.GlobalCargo[gc].Unknown4 = Convert.ToByte(Common.Update(this, XwaMission.GlobalCargo[gc].Unknown4, numGCUnk4.Value));
		}
		void numGCUnk5_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			XwaMission.GlobalCargo[gc].Unknown5 = Convert.ToByte(Common.Update(this, XwaMission.GlobalCargo[gc].Unknown5, numGCUnk5.Value));
		}

		void txtGlobCargo_Leave(object sender, EventArgs e)
		{
			int gc = (int)numGlobCargo.Value - 1;
			XwaMission.GlobalCargo[gc].Cargo = Common.Update(this, XwaMission.GlobalCargo[gc].Cargo, txtGlobCargo.Text).ToString();
		}
		void txtGlobGroup_Leave(object sender, EventArgs e)
		{
			int i;
			for (i=0;i<16;i++) if (lblGG[i].ForeColor == SystemColors.Highlight) break;
			XwaMission.GlobalGroups[i] = Common.Update(this, XwaMission.GlobalGroups[i], txtGlobGroup.Text).ToString();
			GGRefresh();
		}
		void txtNotes_Leave(object sender, EventArgs e)
		{
			XwaMission.MissionNotes = Common.Update(this, XwaMission.MissionNotes, txtNotes.Text).ToString();
		}
		#endregion	
	}
}