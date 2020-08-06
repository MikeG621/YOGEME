using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class OptionsDialog
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
			this.tabOptions = new System.Windows.Forms.TabControl();
			this.tabOpt1 = new System.Windows.Forms.TabPage();
			this.chkConfirmFGDelete = new System.Windows.Forms.CheckBox();
			this.chkRememberPlatformFolder = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmdVerify = new System.Windows.Forms.Button();
			this.txtVerify = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.optStartNormal = new System.Windows.Forms.RadioButton();
			this.optStartPlat = new System.Windows.Forms.RadioButton();
			this.optStartMiss = new System.Windows.Forms.RadioButton();
			this.chkRestrict = new System.Windows.Forms.CheckBox();
			this.chkExit = new System.Windows.Forms.CheckBox();
			this.chkVerifyTest = new System.Windows.Forms.CheckBox();
			this.chkDeletePilot = new System.Windows.Forms.CheckBox();
			this.chkTest = new System.Windows.Forms.CheckBox();
			this.chkVerify = new System.Windows.Forms.CheckBox();
			this.chkSave = new System.Windows.Forms.CheckBox();
			this.tabMap = new System.Windows.Forms.TabPage();
			this.lblMouseWheelZoom = new System.Windows.Forms.Label();
			this.numMousewheelZoom = new System.Windows.Forms.NumericUpDown();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkSP1 = new System.Windows.Forms.CheckBox();
			this.chkSP2 = new System.Windows.Forms.CheckBox();
			this.chkSP3 = new System.Windows.Forms.CheckBox();
			this.chkSP4 = new System.Windows.Forms.CheckBox();
			this.chkRND = new System.Windows.Forms.CheckBox();
			this.chkHYP = new System.Windows.Forms.CheckBox();
			this.chkBRF = new System.Windows.Forms.CheckBox();
			this.chkWP6 = new System.Windows.Forms.CheckBox();
			this.chkWP2 = new System.Windows.Forms.CheckBox();
			this.chkWP1 = new System.Windows.Forms.CheckBox();
			this.chkWP3 = new System.Windows.Forms.CheckBox();
			this.chkWP4 = new System.Windows.Forms.CheckBox();
			this.chkWP5 = new System.Windows.Forms.CheckBox();
			this.chkWP7 = new System.Windows.Forms.CheckBox();
			this.chkWP8 = new System.Windows.Forms.CheckBox();
			this.chkFG = new System.Windows.Forms.CheckBox();
			this.chkTrace = new System.Windows.Forms.CheckBox();
			this.tabWireframe = new System.Windows.Forms.TabPage();
			this.cmdWireMeshDefault = new System.Windows.Forms.Button();
			this.lblQuickToggle = new System.Windows.Forms.Label();
			this.lblDrawMeshes = new System.Windows.Forms.Label();
			this.chkWireToggleHangar = new System.Windows.Forms.CheckBox();
			this.chkWireToggleWeapon = new System.Windows.Forms.CheckBox();
			this.chkWireToggleMisc = new System.Windows.Forms.CheckBox();
			this.chkWireToggleHull = new System.Windows.Forms.CheckBox();
			this.numWireMeshIcon = new System.Windows.Forms.NumericUpDown();
			this.numWireIconThreshold = new System.Windows.Forms.NumericUpDown();
			this.chkWireMeshIcon = new System.Windows.Forms.CheckBox();
			this.chkWireIconThreshold = new System.Windows.Forms.CheckBox();
			this.lstWireMeshTypes = new System.Windows.Forms.ListBox();
			this.chkWireEnabled = new System.Windows.Forms.CheckBox();
			this.tabXW = new System.Windows.Forms.TabPage();
			this.chkXwingDetectMission = new System.Windows.Forms.CheckBox();
			this.chkXwingOverrideExternal = new System.Windows.Forms.CheckBox();
			this.cmdXW = new System.Windows.Forms.Button();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.cboXWCraft = new System.Windows.Forms.ComboBox();
			this.cboXWIFF = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txtXW = new System.Windows.Forms.TextBox();
			this.chkXWInstall = new System.Windows.Forms.CheckBox();
			this.tabOpt2 = new System.Windows.Forms.TabPage();
			this.chkTieDetectMission = new System.Windows.Forms.CheckBox();
			this.chkTieOverrideExternal = new System.Windows.Forms.CheckBox();
			this.cmdTie = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cboTIECraft = new System.Windows.Forms.ComboBox();
			this.cboTIEIFF = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTIE = new System.Windows.Forms.TextBox();
			this.chkTIEInstall = new System.Windows.Forms.CheckBox();
			this.tabOpt3 = new System.Windows.Forms.TabPage();
			this.chkXvtDetectMission = new System.Windows.Forms.CheckBox();
			this.chkXvtOverrideExternal = new System.Windows.Forms.CheckBox();
			this.cmdBop = new System.Windows.Forms.Button();
			this.cmdXvt = new System.Windows.Forms.Button();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.chkBRF8 = new System.Windows.Forms.CheckBox();
			this.chkBRF7 = new System.Windows.Forms.CheckBox();
			this.chkBRF6 = new System.Windows.Forms.CheckBox();
			this.chkBRF5 = new System.Windows.Forms.CheckBox();
			this.chkBRF4 = new System.Windows.Forms.CheckBox();
			this.chkBRF3 = new System.Windows.Forms.CheckBox();
			this.chkBRF2 = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cboXvTCraft = new System.Windows.Forms.ComboBox();
			this.cboXvTIFF = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtXvT = new System.Windows.Forms.TextBox();
			this.chkXvTInstall = new System.Windows.Forms.CheckBox();
			this.txtBoP = new System.Windows.Forms.TextBox();
			this.chkBoPInstall = new System.Windows.Forms.CheckBox();
			this.tabOpt4 = new System.Windows.Forms.TabPage();
			this.lblExportWarning = new System.Windows.Forms.Label();
			this.chkXwaDetectMission = new System.Windows.Forms.CheckBox();
			this.chkXwaFlagRemappedCraft = new System.Windows.Forms.CheckBox();
			this.cmdExport = new System.Windows.Forms.Button();
			this.chkXwaOverrideScan = new System.Windows.Forms.CheckBox();
			this.chkXwaOverrideExternal = new System.Windows.Forms.CheckBox();
			this.chkBackdrops = new System.Windows.Forms.CheckBox();
			this.cmdXwa = new System.Windows.Forms.Button();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cboXWACraft = new System.Windows.Forms.ComboBox();
			this.cboXWAIFF = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtXWA = new System.Windows.Forms.TextBox();
			this.chkXWAInstall = new System.Windows.Forms.CheckBox();
			this.tabColors = new System.Windows.Forms.TabPage();
			this.label14 = new System.Windows.Forms.Label();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.cboInteractiveTheme = new System.Windows.Forms.ComboBox();
			this.label15 = new System.Windows.Forms.Label();
			this.cmdInteractBackground = new System.Windows.Forms.Button();
			this.cmdInteractNonSelect = new System.Windows.Forms.Button();
			this.cmdInteractSelect = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.lblInteractNonSelected = new System.Windows.Forms.Label();
			this.lblInteractSelected = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.txtColorBackground = new System.Windows.Forms.TextBox();
			this.txtColorNonSelected = new System.Windows.Forms.TextBox();
			this.txtColorSelected = new System.Windows.Forms.TextBox();
			this.chkColorizeFG = new System.Windows.Forms.CheckBox();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.opnVerify = new System.Windows.Forms.OpenFileDialog();
			this.dirPlatform = new System.Windows.Forms.FolderBrowserDialog();
			this.colorSelector = new System.Windows.Forms.ColorDialog();
			this.tabOptions.SuspendLayout();
			this.tabOpt1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabMap.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMousewheelZoom)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.tabWireframe.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numWireMeshIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numWireIconThreshold)).BeginInit();
			this.tabXW.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabOpt2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabOpt3.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabOpt4.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.tabColors.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabOptions
			// 
			this.tabOptions.Controls.Add(this.tabOpt1);
			this.tabOptions.Controls.Add(this.tabMap);
			this.tabOptions.Controls.Add(this.tabWireframe);
			this.tabOptions.Controls.Add(this.tabXW);
			this.tabOptions.Controls.Add(this.tabOpt2);
			this.tabOptions.Controls.Add(this.tabOpt3);
			this.tabOptions.Controls.Add(this.tabOpt4);
			this.tabOptions.Controls.Add(this.tabColors);
			this.tabOptions.Location = new System.Drawing.Point(0, 0);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.SelectedIndex = 0;
			this.tabOptions.Size = new System.Drawing.Size(405, 264);
			this.tabOptions.TabIndex = 0;
			// 
			// tabOpt1
			// 
			this.tabOpt1.Controls.Add(this.chkConfirmFGDelete);
			this.tabOpt1.Controls.Add(this.chkRememberPlatformFolder);
			this.tabOpt1.Controls.Add(this.label7);
			this.tabOpt1.Controls.Add(this.cmdVerify);
			this.tabOpt1.Controls.Add(this.txtVerify);
			this.tabOpt1.Controls.Add(this.groupBox1);
			this.tabOpt1.Controls.Add(this.chkRestrict);
			this.tabOpt1.Controls.Add(this.chkExit);
			this.tabOpt1.Controls.Add(this.chkVerifyTest);
			this.tabOpt1.Controls.Add(this.chkDeletePilot);
			this.tabOpt1.Controls.Add(this.chkTest);
			this.tabOpt1.Controls.Add(this.chkVerify);
			this.tabOpt1.Controls.Add(this.chkSave);
			this.tabOpt1.Location = new System.Drawing.Point(4, 22);
			this.tabOpt1.Name = "tabOpt1";
			this.tabOpt1.Size = new System.Drawing.Size(397, 238);
			this.tabOpt1.TabIndex = 0;
			this.tabOpt1.Text = "Overall";
			// 
			// chkConfirmFGDelete
			// 
			this.chkConfirmFGDelete.Checked = true;
			this.chkConfirmFGDelete.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkConfirmFGDelete.Location = new System.Drawing.Point(157, 179);
			this.chkConfirmFGDelete.Name = "chkConfirmFGDelete";
			this.chkConfirmFGDelete.Size = new System.Drawing.Size(224, 16);
			this.chkConfirmFGDelete.TabIndex = 12;
			this.chkConfirmFGDelete.Text = "Confirm deleting FG dependencies";
			// 
			// chkRememberPlatformFolder
			// 
			this.chkRememberPlatformFolder.Checked = true;
			this.chkRememberPlatformFolder.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRememberPlatformFolder.Location = new System.Drawing.Point(157, 157);
			this.chkRememberPlatformFolder.Name = "chkRememberPlatformFolder";
			this.chkRememberPlatformFolder.Size = new System.Drawing.Size(224, 16);
			this.chkRememberPlatformFolder.TabIndex = 11;
			this.chkRememberPlatformFolder.Text = "Remember folder when opening files";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(13, 198);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(91, 13);
			this.label7.TabIndex = 11;
			this.label7.Text = "Verify executable:";
			// 
			// cmdVerify
			// 
			this.cmdVerify.Location = new System.Drawing.Point(350, 211);
			this.cmdVerify.Name = "cmdVerify";
			this.cmdVerify.Size = new System.Drawing.Size(24, 24);
			this.cmdVerify.TabIndex = 14;
			this.cmdVerify.Text = "...";
			this.cmdVerify.UseVisualStyleBackColor = true;
			this.cmdVerify.Click += new System.EventHandler(this.cmdVerify_Click);
			// 
			// txtVerify
			// 
			this.txtVerify.Location = new System.Drawing.Point(16, 215);
			this.txtVerify.Name = "txtVerify";
			this.txtVerify.Size = new System.Drawing.Size(328, 20);
			this.txtVerify.TabIndex = 13;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.optStartNormal);
			this.groupBox1.Controls.Add(this.optStartPlat);
			this.groupBox1.Controls.Add(this.optStartMiss);
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(128, 96);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Startup";
			// 
			// optStartNormal
			// 
			this.optStartNormal.Checked = true;
			this.optStartNormal.Location = new System.Drawing.Point(8, 16);
			this.optStartNormal.Name = "optStartNormal";
			this.optStartNormal.Size = new System.Drawing.Size(112, 24);
			this.optStartNormal.TabIndex = 1;
			this.optStartNormal.TabStop = true;
			this.optStartNormal.Text = "Ask for platform";
			// 
			// optStartPlat
			// 
			this.optStartPlat.Location = new System.Drawing.Point(8, 40);
			this.optStartPlat.Name = "optStartPlat";
			this.optStartPlat.Size = new System.Drawing.Size(112, 24);
			this.optStartPlat.TabIndex = 2;
			this.optStartPlat.Text = "Load last platform";
			// 
			// optStartMiss
			// 
			this.optStartMiss.Location = new System.Drawing.Point(8, 64);
			this.optStartMiss.Name = "optStartMiss";
			this.optStartMiss.Size = new System.Drawing.Size(112, 24);
			this.optStartMiss.TabIndex = 3;
			this.optStartMiss.Text = "Load last mission";
			// 
			// chkRestrict
			// 
			this.chkRestrict.Checked = true;
			this.chkRestrict.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRestrict.Location = new System.Drawing.Point(157, 3);
			this.chkRestrict.Name = "chkRestrict";
			this.chkRestrict.Size = new System.Drawing.Size(224, 16);
			this.chkRestrict.TabIndex = 4;
			this.chkRestrict.Text = "Only allow editing for installed platforms";
			// 
			// chkExit
			// 
			this.chkExit.Checked = true;
			this.chkExit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkExit.Location = new System.Drawing.Point(157, 25);
			this.chkExit.Name = "chkExit";
			this.chkExit.Size = new System.Drawing.Size(224, 16);
			this.chkExit.TabIndex = 5;
			this.chkExit.Text = "Confirm exit";
			// 
			// chkVerifyTest
			// 
			this.chkVerifyTest.Checked = true;
			this.chkVerifyTest.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVerifyTest.Location = new System.Drawing.Point(157, 113);
			this.chkVerifyTest.Name = "chkVerifyTest";
			this.chkVerifyTest.Size = new System.Drawing.Size(224, 16);
			this.chkVerifyTest.TabIndex = 9;
			this.chkVerifyTest.Text = "Verify mission before test";
			// 
			// chkDeletePilot
			// 
			this.chkDeletePilot.Checked = true;
			this.chkDeletePilot.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDeletePilot.Location = new System.Drawing.Point(157, 135);
			this.chkDeletePilot.Name = "chkDeletePilot";
			this.chkDeletePilot.Size = new System.Drawing.Size(224, 16);
			this.chkDeletePilot.TabIndex = 10;
			this.chkDeletePilot.Text = "Delete Test pilot files";
			// 
			// chkTest
			// 
			this.chkTest.Checked = true;
			this.chkTest.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTest.Location = new System.Drawing.Point(157, 91);
			this.chkTest.Name = "chkTest";
			this.chkTest.Size = new System.Drawing.Size(224, 16);
			this.chkTest.TabIndex = 8;
			this.chkTest.Text = "Confirm before Testing";
			// 
			// chkVerify
			// 
			this.chkVerify.Checked = true;
			this.chkVerify.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVerify.Location = new System.Drawing.Point(157, 69);
			this.chkVerify.Name = "chkVerify";
			this.chkVerify.Size = new System.Drawing.Size(224, 16);
			this.chkVerify.TabIndex = 7;
			this.chkVerify.Text = "Verify mission on save";
			this.chkVerify.CheckedChanged += new System.EventHandler(this.chkVerify_CheckedChanged);
			// 
			// chkSave
			// 
			this.chkSave.Checked = true;
			this.chkSave.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSave.Location = new System.Drawing.Point(157, 47);
			this.chkSave.Name = "chkSave";
			this.chkSave.Size = new System.Drawing.Size(224, 16);
			this.chkSave.TabIndex = 6;
			this.chkSave.Text = "Confirm save on closing";
			// 
			// tabMap
			// 
			this.tabMap.Controls.Add(this.lblMouseWheelZoom);
			this.tabMap.Controls.Add(this.numMousewheelZoom);
			this.tabMap.Controls.Add(this.groupBox2);
			this.tabMap.Controls.Add(this.chkFG);
			this.tabMap.Controls.Add(this.chkTrace);
			this.tabMap.Location = new System.Drawing.Point(4, 22);
			this.tabMap.Name = "tabMap";
			this.tabMap.Size = new System.Drawing.Size(397, 238);
			this.tabMap.TabIndex = 4;
			this.tabMap.Text = "Map";
			// 
			// lblMouseWheelZoom
			// 
			this.lblMouseWheelZoom.AutoSize = true;
			this.lblMouseWheelZoom.Location = new System.Drawing.Point(13, 83);
			this.lblMouseWheelZoom.Name = "lblMouseWheelZoom";
			this.lblMouseWheelZoom.Size = new System.Drawing.Size(108, 13);
			this.lblMouseWheelZoom.TabIndex = 4;
			this.lblMouseWheelZoom.Text = "Mousewheel Zoom %";
			// 
			// numMousewheelZoom
			// 
			this.numMousewheelZoom.DecimalPlaces = 1;
			this.numMousewheelZoom.Location = new System.Drawing.Point(16, 99);
			this.numMousewheelZoom.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numMousewheelZoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numMousewheelZoom.Name = "numMousewheelZoom";
			this.numMousewheelZoom.Size = new System.Drawing.Size(51, 20);
			this.numMousewheelZoom.TabIndex = 3;
			this.numMousewheelZoom.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.chkSP1);
			this.groupBox2.Controls.Add(this.chkSP2);
			this.groupBox2.Controls.Add(this.chkSP3);
			this.groupBox2.Controls.Add(this.chkSP4);
			this.groupBox2.Controls.Add(this.chkRND);
			this.groupBox2.Controls.Add(this.chkHYP);
			this.groupBox2.Controls.Add(this.chkBRF);
			this.groupBox2.Controls.Add(this.chkWP6);
			this.groupBox2.Controls.Add(this.chkWP2);
			this.groupBox2.Controls.Add(this.chkWP1);
			this.groupBox2.Controls.Add(this.chkWP3);
			this.groupBox2.Controls.Add(this.chkWP4);
			this.groupBox2.Controls.Add(this.chkWP5);
			this.groupBox2.Controls.Add(this.chkWP7);
			this.groupBox2.Controls.Add(this.chkWP8);
			this.groupBox2.Location = new System.Drawing.Point(144, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(232, 216);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Active Waypoints";
			// 
			// chkSP1
			// 
			this.chkSP1.Location = new System.Drawing.Point(16, 24);
			this.chkSP1.Name = "chkSP1";
			this.chkSP1.Size = new System.Drawing.Size(88, 16);
			this.chkSP1.TabIndex = 3;
			this.chkSP1.Text = "SP1";
			// 
			// chkSP2
			// 
			this.chkSP2.Location = new System.Drawing.Point(16, 48);
			this.chkSP2.Name = "chkSP2";
			this.chkSP2.Size = new System.Drawing.Size(88, 16);
			this.chkSP2.TabIndex = 4;
			this.chkSP2.Text = "SP2";
			// 
			// chkSP3
			// 
			this.chkSP3.Location = new System.Drawing.Point(16, 72);
			this.chkSP3.Name = "chkSP3";
			this.chkSP3.Size = new System.Drawing.Size(88, 16);
			this.chkSP3.TabIndex = 5;
			this.chkSP3.Text = "SP3";
			// 
			// chkSP4
			// 
			this.chkSP4.Location = new System.Drawing.Point(16, 96);
			this.chkSP4.Name = "chkSP4";
			this.chkSP4.Size = new System.Drawing.Size(88, 16);
			this.chkSP4.TabIndex = 6;
			this.chkSP4.Text = "SP4";
			// 
			// chkRND
			// 
			this.chkRND.Location = new System.Drawing.Point(16, 120);
			this.chkRND.Name = "chkRND";
			this.chkRND.Size = new System.Drawing.Size(88, 16);
			this.chkRND.TabIndex = 7;
			this.chkRND.Text = "RND";
			// 
			// chkHYP
			// 
			this.chkHYP.Location = new System.Drawing.Point(16, 144);
			this.chkHYP.Name = "chkHYP";
			this.chkHYP.Size = new System.Drawing.Size(88, 16);
			this.chkHYP.TabIndex = 8;
			this.chkHYP.Text = "HYP";
			// 
			// chkBRF
			// 
			this.chkBRF.Location = new System.Drawing.Point(16, 163);
			this.chkBRF.Name = "chkBRF";
			this.chkBRF.Size = new System.Drawing.Size(88, 27);
			this.chkBRF.TabIndex = 9;
			this.chkBRF.Text = "BRF (1)";
			// 
			// chkWP6
			// 
			this.chkWP6.Location = new System.Drawing.Point(128, 144);
			this.chkWP6.Name = "chkWP6";
			this.chkWP6.Size = new System.Drawing.Size(88, 16);
			this.chkWP6.TabIndex = 15;
			this.chkWP6.Text = "WP6";
			// 
			// chkWP2
			// 
			this.chkWP2.Location = new System.Drawing.Point(128, 48);
			this.chkWP2.Name = "chkWP2";
			this.chkWP2.Size = new System.Drawing.Size(88, 16);
			this.chkWP2.TabIndex = 11;
			this.chkWP2.Text = "WP2";
			// 
			// chkWP1
			// 
			this.chkWP1.Location = new System.Drawing.Point(128, 24);
			this.chkWP1.Name = "chkWP1";
			this.chkWP1.Size = new System.Drawing.Size(88, 16);
			this.chkWP1.TabIndex = 10;
			this.chkWP1.Text = "WP1";
			// 
			// chkWP3
			// 
			this.chkWP3.Location = new System.Drawing.Point(128, 72);
			this.chkWP3.Name = "chkWP3";
			this.chkWP3.Size = new System.Drawing.Size(88, 16);
			this.chkWP3.TabIndex = 12;
			this.chkWP3.Text = "WP3";
			// 
			// chkWP4
			// 
			this.chkWP4.Location = new System.Drawing.Point(128, 96);
			this.chkWP4.Name = "chkWP4";
			this.chkWP4.Size = new System.Drawing.Size(88, 16);
			this.chkWP4.TabIndex = 13;
			this.chkWP4.Text = "WP4";
			// 
			// chkWP5
			// 
			this.chkWP5.Location = new System.Drawing.Point(128, 120);
			this.chkWP5.Name = "chkWP5";
			this.chkWP5.Size = new System.Drawing.Size(88, 16);
			this.chkWP5.TabIndex = 14;
			this.chkWP5.Text = "WP5";
			// 
			// chkWP7
			// 
			this.chkWP7.Location = new System.Drawing.Point(128, 168);
			this.chkWP7.Name = "chkWP7";
			this.chkWP7.Size = new System.Drawing.Size(88, 16);
			this.chkWP7.TabIndex = 16;
			this.chkWP7.Text = "WP7";
			// 
			// chkWP8
			// 
			this.chkWP8.Location = new System.Drawing.Point(128, 192);
			this.chkWP8.Name = "chkWP8";
			this.chkWP8.Size = new System.Drawing.Size(88, 16);
			this.chkWP8.TabIndex = 17;
			this.chkWP8.Text = "WP8";
			// 
			// chkFG
			// 
			this.chkFG.Location = new System.Drawing.Point(16, 24);
			this.chkFG.Name = "chkFG";
			this.chkFG.Size = new System.Drawing.Size(104, 16);
			this.chkFG.TabIndex = 1;
			this.chkFG.Text = "FG Tags";
			// 
			// chkTrace
			// 
			this.chkTrace.Location = new System.Drawing.Point(16, 56);
			this.chkTrace.Name = "chkTrace";
			this.chkTrace.Size = new System.Drawing.Size(104, 16);
			this.chkTrace.TabIndex = 2;
			this.chkTrace.Text = "WP Traces";
			// 
			// tabWireframe
			// 
			this.tabWireframe.Controls.Add(this.cmdWireMeshDefault);
			this.tabWireframe.Controls.Add(this.lblQuickToggle);
			this.tabWireframe.Controls.Add(this.lblDrawMeshes);
			this.tabWireframe.Controls.Add(this.chkWireToggleHangar);
			this.tabWireframe.Controls.Add(this.chkWireToggleWeapon);
			this.tabWireframe.Controls.Add(this.chkWireToggleMisc);
			this.tabWireframe.Controls.Add(this.chkWireToggleHull);
			this.tabWireframe.Controls.Add(this.numWireMeshIcon);
			this.tabWireframe.Controls.Add(this.numWireIconThreshold);
			this.tabWireframe.Controls.Add(this.chkWireMeshIcon);
			this.tabWireframe.Controls.Add(this.chkWireIconThreshold);
			this.tabWireframe.Controls.Add(this.lstWireMeshTypes);
			this.tabWireframe.Controls.Add(this.chkWireEnabled);
			this.tabWireframe.Location = new System.Drawing.Point(4, 22);
			this.tabWireframe.Name = "tabWireframe";
			this.tabWireframe.Size = new System.Drawing.Size(397, 238);
			this.tabWireframe.TabIndex = 7;
			this.tabWireframe.Text = "Wireframe";
			// 
			// cmdWireMeshDefault
			// 
			this.cmdWireMeshDefault.Location = new System.Drawing.Point(70, 210);
			this.cmdWireMeshDefault.Name = "cmdWireMeshDefault";
			this.cmdWireMeshDefault.Size = new System.Drawing.Size(93, 23);
			this.cmdWireMeshDefault.TabIndex = 10;
			this.cmdWireMeshDefault.Text = "Default Meshes";
			this.cmdWireMeshDefault.UseVisualStyleBackColor = true;
			this.cmdWireMeshDefault.Click += new System.EventHandler(this.cmdWireMeshDefault_Click);
			// 
			// lblQuickToggle
			// 
			this.lblQuickToggle.AutoSize = true;
			this.lblQuickToggle.Location = new System.Drawing.Point(191, 144);
			this.lblQuickToggle.Name = "lblQuickToggle";
			this.lblQuickToggle.Size = new System.Drawing.Size(70, 13);
			this.lblQuickToggle.TabIndex = 14;
			this.lblQuickToggle.Text = "Quick toggle:";
			// 
			// lblDrawMeshes
			// 
			this.lblDrawMeshes.AutoSize = true;
			this.lblDrawMeshes.Location = new System.Drawing.Point(264, 0);
			this.lblDrawMeshes.Name = "lblDrawMeshes";
			this.lblDrawMeshes.Size = new System.Drawing.Size(103, 13);
			this.lblDrawMeshes.TabIndex = 13;
			this.lblDrawMeshes.Text = "Draw these meshes:";
			// 
			// chkWireToggleHangar
			// 
			this.chkWireToggleHangar.AutoSize = true;
			this.chkWireToggleHangar.Location = new System.Drawing.Point(169, 214);
			this.chkWireToggleHangar.Name = "chkWireToggleHangar";
			this.chkWireToggleHangar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkWireToggleHangar.Size = new System.Drawing.Size(92, 17);
			this.chkWireToggleHangar.TabIndex = 9;
			this.chkWireToggleHangar.Text = "Hangar/Dock";
			this.chkWireToggleHangar.UseVisualStyleBackColor = true;
			this.chkWireToggleHangar.CheckedChanged += new System.EventHandler(this.chkWireToggleHangar_CheckedChanged);
			// 
			// chkWireToggleWeapon
			// 
			this.chkWireToggleWeapon.AutoSize = true;
			this.chkWireToggleWeapon.Location = new System.Drawing.Point(180, 196);
			this.chkWireToggleWeapon.Name = "chkWireToggleWeapon";
			this.chkWireToggleWeapon.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkWireToggleWeapon.Size = new System.Drawing.Size(81, 17);
			this.chkWireToggleWeapon.TabIndex = 8;
			this.chkWireToggleWeapon.Text = "All Weapon";
			this.chkWireToggleWeapon.UseVisualStyleBackColor = true;
			this.chkWireToggleWeapon.CheckedChanged += new System.EventHandler(this.chkWireToggleWeapon_CheckedChanged);
			// 
			// chkWireToggleMisc
			// 
			this.chkWireToggleMisc.AutoSize = true;
			this.chkWireToggleMisc.Location = new System.Drawing.Point(199, 178);
			this.chkWireToggleMisc.Name = "chkWireToggleMisc";
			this.chkWireToggleMisc.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkWireToggleMisc.Size = new System.Drawing.Size(62, 17);
			this.chkWireToggleMisc.TabIndex = 7;
			this.chkWireToggleMisc.Text = "All Misc";
			this.chkWireToggleMisc.UseVisualStyleBackColor = true;
			this.chkWireToggleMisc.CheckedChanged += new System.EventHandler(this.chkWireToggleMisc_CheckedChanged);
			// 
			// chkWireToggleHull
			// 
			this.chkWireToggleHull.AutoSize = true;
			this.chkWireToggleHull.Location = new System.Drawing.Point(203, 160);
			this.chkWireToggleHull.Name = "chkWireToggleHull";
			this.chkWireToggleHull.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkWireToggleHull.Size = new System.Drawing.Size(58, 17);
			this.chkWireToggleHull.TabIndex = 6;
			this.chkWireToggleHull.Text = "All Hull";
			this.chkWireToggleHull.UseVisualStyleBackColor = true;
			this.chkWireToggleHull.CheckedChanged += new System.EventHandler(this.chkWireToggleHull_CheckedChanged);
			// 
			// numWireMeshIcon
			// 
			this.numWireMeshIcon.Location = new System.Drawing.Point(217, 63);
			this.numWireMeshIcon.Name = "numWireMeshIcon";
			this.numWireMeshIcon.Size = new System.Drawing.Size(44, 20);
			this.numWireMeshIcon.TabIndex = 4;
			// 
			// numWireIconThreshold
			// 
			this.numWireIconThreshold.Location = new System.Drawing.Point(217, 40);
			this.numWireIconThreshold.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.numWireIconThreshold.Name = "numWireIconThreshold";
			this.numWireIconThreshold.Size = new System.Drawing.Size(44, 20);
			this.numWireIconThreshold.TabIndex = 2;
			// 
			// chkWireMeshIcon
			// 
			this.chkWireMeshIcon.AutoSize = true;
			this.chkWireMeshIcon.Location = new System.Drawing.Point(8, 64);
			this.chkWireMeshIcon.Name = "chkWireMeshIcon";
			this.chkWireMeshIcon.Size = new System.Drawing.Size(197, 17);
			this.chkWireMeshIcon.TabIndex = 3;
			this.chkWireMeshIcon.Text = "Use mesh as 3D icon (size in pixels):";
			this.chkWireMeshIcon.UseVisualStyleBackColor = true;
			// 
			// chkWireIconThreshold
			// 
			this.chkWireIconThreshold.AutoSize = true;
			this.chkWireIconThreshold.Location = new System.Drawing.Point(8, 41);
			this.chkWireIconThreshold.Name = "chkWireIconThreshold";
			this.chkWireIconThreshold.Size = new System.Drawing.Size(204, 17);
			this.chkWireIconThreshold.TabIndex = 1;
			this.chkWireIconThreshold.Text = "Use icons when smaller than (meters):";
			this.chkWireIconThreshold.UseVisualStyleBackColor = true;
			// 
			// lstWireMeshTypes
			// 
			this.lstWireMeshTypes.FormattingEnabled = true;
			this.lstWireMeshTypes.Items.AddRange(new object[] {
            "Default",
            "MainHull",
            "Wing",
            "Fuselage",
            "GunTurret",
            "SmallGun",
            "Engine",
            "Bridge",
            "ShieldGenerator",
            "EnergyGenerator",
            "Launcher",
            "CommunicationSystem",
            "BeamSystem",
            "CommandSystem",
            "DockingPlatform",
            "LandingPlatform",
            "Hangar",
            "CargoPod",
            "MiscHull",
            "Antenna",
            "RotaryWing",
            "RotaryGunTurret",
            "RotaryLauncher",
            "RotaryCommunicationSystem",
            "RotaryBeamSystem",
            "RotaryCommandSystem",
            "Hatch",
            "Custom",
            "WeaponSystem1",
            "WeaponSystem2",
            "PowerRegenerator",
            "Reactor"});
			this.lstWireMeshTypes.Location = new System.Drawing.Point(267, 16);
			this.lstWireMeshTypes.Name = "lstWireMeshTypes";
			this.lstWireMeshTypes.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.lstWireMeshTypes.Size = new System.Drawing.Size(114, 212);
			this.lstWireMeshTypes.TabIndex = 5;
			this.lstWireMeshTypes.SelectedIndexChanged += new System.EventHandler(this.lstWireMeshTypes_SelectedIndexChanged);
			// 
			// chkWireEnabled
			// 
			this.chkWireEnabled.AutoSize = true;
			this.chkWireEnabled.Location = new System.Drawing.Point(8, 18);
			this.chkWireEnabled.Name = "chkWireEnabled";
			this.chkWireEnabled.Size = new System.Drawing.Size(121, 17);
			this.chkWireEnabled.TabIndex = 0;
			this.chkWireEnabled.Text = "Wireframes Enabled";
			this.chkWireEnabled.UseVisualStyleBackColor = true;
			// 
			// tabXW
			// 
			this.tabXW.Controls.Add(this.chkXwingDetectMission);
			this.tabXW.Controls.Add(this.chkXwingOverrideExternal);
			this.tabXW.Controls.Add(this.cmdXW);
			this.tabXW.Controls.Add(this.groupBox6);
			this.tabXW.Controls.Add(this.txtXW);
			this.tabXW.Controls.Add(this.chkXWInstall);
			this.tabXW.Location = new System.Drawing.Point(4, 22);
			this.tabXW.Name = "tabXW";
			this.tabXW.Padding = new System.Windows.Forms.Padding(3);
			this.tabXW.Size = new System.Drawing.Size(397, 238);
			this.tabXW.TabIndex = 5;
			this.tabXW.Text = "X-wing";
			// 
			// chkXwingDetectMission
			// 
			this.chkXwingDetectMission.AutoSize = true;
			this.chkXwingDetectMission.Location = new System.Drawing.Point(182, 56);
			this.chkXwingDetectMission.Name = "chkXwingDetectMission";
			this.chkXwingDetectMission.Size = new System.Drawing.Size(194, 17);
			this.chkXwingDetectMission.TabIndex = 7;
			this.chkXwingDetectMission.Text = "Detect installation from mission path";
			this.chkXwingDetectMission.UseVisualStyleBackColor = true;
			// 
			// chkXwingOverrideExternal
			// 
			this.chkXwingOverrideExternal.AutoSize = true;
			this.chkXwingOverrideExternal.Location = new System.Drawing.Point(182, 76);
			this.chkXwingOverrideExternal.Name = "chkXwingOverrideExternal";
			this.chkXwingOverrideExternal.Size = new System.Drawing.Size(203, 17);
			this.chkXwingOverrideExternal.TabIndex = 8;
			this.chkXwingOverrideExternal.Text = "Override craft names from external file";
			this.chkXwingOverrideExternal.UseVisualStyleBackColor = true;
			// 
			// cmdXW
			// 
			this.cmdXW.Location = new System.Drawing.Point(357, 15);
			this.cmdXW.Name = "cmdXW";
			this.cmdXW.Size = new System.Drawing.Size(24, 24);
			this.cmdXW.TabIndex = 3;
			this.cmdXW.Text = "...";
			this.cmdXW.UseVisualStyleBackColor = true;
			this.cmdXW.Click += new System.EventHandler(this.cmdXW_Click);
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.label8);
			this.groupBox6.Controls.Add(this.cboXWCraft);
			this.groupBox6.Controls.Add(this.cboXWIFF);
			this.groupBox6.Controls.Add(this.label9);
			this.groupBox6.Location = new System.Drawing.Point(8, 56);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(168, 128);
			this.groupBox6.TabIndex = 4;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Default Craft";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 24);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(72, 16);
			this.label8.TabIndex = 1;
			this.label8.Text = "Craft type";
			// 
			// cboXWCraft
			// 
			this.cboXWCraft.Location = new System.Drawing.Point(8, 40);
			this.cboXWCraft.Name = "cboXWCraft";
			this.cboXWCraft.Size = new System.Drawing.Size(154, 21);
			this.cboXWCraft.TabIndex = 5;
			// 
			// cboXWIFF
			// 
			this.cboXWIFF.Items.AddRange(new object[] {
            "Default",
            "Rebel",
            "Imperial",
            "Neutral",
            "Neutral2"});
			this.cboXWIFF.Location = new System.Drawing.Point(8, 96);
			this.cboXWIFF.Name = "cboXWIFF";
			this.cboXWIFF.Size = new System.Drawing.Size(96, 21);
			this.cboXWIFF.TabIndex = 6;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 80);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(72, 16);
			this.label9.TabIndex = 1;
			this.label9.Text = "IFF";
			// 
			// txtXW
			// 
			this.txtXW.Enabled = false;
			this.txtXW.Location = new System.Drawing.Point(86, 16);
			this.txtXW.Name = "txtXW";
			this.txtXW.Size = new System.Drawing.Size(266, 20);
			this.txtXW.TabIndex = 2;
			// 
			// chkXWInstall
			// 
			this.chkXWInstall.Location = new System.Drawing.Point(8, 16);
			this.chkXWInstall.Name = "chkXWInstall";
			this.chkXWInstall.Size = new System.Drawing.Size(72, 24);
			this.chkXWInstall.TabIndex = 1;
			this.chkXWInstall.Text = "Installed";
			this.chkXWInstall.CheckedChanged += new System.EventHandler(this.chkXWInstall_CheckedChanged);
			// 
			// tabOpt2
			// 
			this.tabOpt2.Controls.Add(this.chkTieDetectMission);
			this.tabOpt2.Controls.Add(this.chkTieOverrideExternal);
			this.tabOpt2.Controls.Add(this.cmdTie);
			this.tabOpt2.Controls.Add(this.groupBox3);
			this.tabOpt2.Controls.Add(this.txtTIE);
			this.tabOpt2.Controls.Add(this.chkTIEInstall);
			this.tabOpt2.Location = new System.Drawing.Point(4, 22);
			this.tabOpt2.Name = "tabOpt2";
			this.tabOpt2.Size = new System.Drawing.Size(397, 238);
			this.tabOpt2.TabIndex = 1;
			this.tabOpt2.Text = "TIE";
			// 
			// chkTieDetectMission
			// 
			this.chkTieDetectMission.AutoSize = true;
			this.chkTieDetectMission.Location = new System.Drawing.Point(182, 56);
			this.chkTieDetectMission.Name = "chkTieDetectMission";
			this.chkTieDetectMission.Size = new System.Drawing.Size(194, 17);
			this.chkTieDetectMission.TabIndex = 7;
			this.chkTieDetectMission.Text = "Detect installation from mission path";
			this.chkTieDetectMission.UseVisualStyleBackColor = true;
			// 
			// chkTieOverrideExternal
			// 
			this.chkTieOverrideExternal.AutoSize = true;
			this.chkTieOverrideExternal.Location = new System.Drawing.Point(182, 76);
			this.chkTieOverrideExternal.Name = "chkTieOverrideExternal";
			this.chkTieOverrideExternal.Size = new System.Drawing.Size(203, 17);
			this.chkTieOverrideExternal.TabIndex = 8;
			this.chkTieOverrideExternal.Text = "Override craft names from external file";
			this.chkTieOverrideExternal.UseVisualStyleBackColor = true;
			// 
			// cmdTie
			// 
			this.cmdTie.Location = new System.Drawing.Point(357, 15);
			this.cmdTie.Name = "cmdTie";
			this.cmdTie.Size = new System.Drawing.Size(24, 24);
			this.cmdTie.TabIndex = 3;
			this.cmdTie.Text = "...";
			this.cmdTie.UseVisualStyleBackColor = true;
			this.cmdTie.Click += new System.EventHandler(this.cmdTie_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.cboTIECraft);
			this.groupBox3.Controls.Add(this.cboTIEIFF);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Location = new System.Drawing.Point(8, 56);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(168, 128);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Default Craft";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Craft type";
			// 
			// cboTIECraft
			// 
			this.cboTIECraft.Location = new System.Drawing.Point(8, 40);
			this.cboTIECraft.Name = "cboTIECraft";
			this.cboTIECraft.Size = new System.Drawing.Size(154, 21);
			this.cboTIECraft.TabIndex = 5;
			// 
			// cboTIEIFF
			// 
			this.cboTIEIFF.Items.AddRange(new object[] {
            "Rebel",
            "Imperial",
            "IFF3-Blue",
            "IFF4-Purple",
            "IFF5-Red",
            "IFF6-Purple"});
			this.cboTIEIFF.Location = new System.Drawing.Point(8, 96);
			this.cboTIEIFF.Name = "cboTIEIFF";
			this.cboTIEIFF.Size = new System.Drawing.Size(96, 21);
			this.cboTIEIFF.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "IFF";
			// 
			// txtTIE
			// 
			this.txtTIE.Enabled = false;
			this.txtTIE.Location = new System.Drawing.Point(86, 16);
			this.txtTIE.Name = "txtTIE";
			this.txtTIE.Size = new System.Drawing.Size(266, 20);
			this.txtTIE.TabIndex = 2;
			// 
			// chkTIEInstall
			// 
			this.chkTIEInstall.Location = new System.Drawing.Point(8, 16);
			this.chkTIEInstall.Name = "chkTIEInstall";
			this.chkTIEInstall.Size = new System.Drawing.Size(72, 24);
			this.chkTIEInstall.TabIndex = 1;
			this.chkTIEInstall.Text = "Installed";
			this.chkTIEInstall.CheckedChanged += new System.EventHandler(this.chkTIEInstall_CheckedChanged);
			// 
			// tabOpt3
			// 
			this.tabOpt3.Controls.Add(this.chkXvtDetectMission);
			this.tabOpt3.Controls.Add(this.chkXvtOverrideExternal);
			this.tabOpt3.Controls.Add(this.cmdBop);
			this.tabOpt3.Controls.Add(this.cmdXvt);
			this.tabOpt3.Controls.Add(this.groupBox5);
			this.tabOpt3.Controls.Add(this.groupBox4);
			this.tabOpt3.Controls.Add(this.txtXvT);
			this.tabOpt3.Controls.Add(this.chkXvTInstall);
			this.tabOpt3.Controls.Add(this.txtBoP);
			this.tabOpt3.Controls.Add(this.chkBoPInstall);
			this.tabOpt3.Location = new System.Drawing.Point(4, 22);
			this.tabOpt3.Name = "tabOpt3";
			this.tabOpt3.Size = new System.Drawing.Size(397, 238);
			this.tabOpt3.TabIndex = 2;
			this.tabOpt3.Text = "XvT, BoP";
			// 
			// chkXvtDetectMission
			// 
			this.chkXvtDetectMission.Location = new System.Drawing.Point(270, 79);
			this.chkXvtDetectMission.Name = "chkXvtDetectMission";
			this.chkXvtDetectMission.Size = new System.Drawing.Size(104, 44);
			this.chkXvtDetectMission.TabIndex = 19;
			this.chkXvtDetectMission.Text = "Detect installation from mission path";
			this.chkXvtDetectMission.UseVisualStyleBackColor = true;
			// 
			// chkXvtOverrideExternal
			// 
			this.chkXvtOverrideExternal.Location = new System.Drawing.Point(270, 122);
			this.chkXvtOverrideExternal.Name = "chkXvtOverrideExternal";
			this.chkXvtOverrideExternal.Size = new System.Drawing.Size(104, 60);
			this.chkXvtOverrideExternal.TabIndex = 20;
			this.chkXvtOverrideExternal.Text = "Override craft names from external file";
			this.chkXvtOverrideExternal.UseVisualStyleBackColor = true;
			// 
			// cmdBop
			// 
			this.cmdBop.Location = new System.Drawing.Point(357, 49);
			this.cmdBop.Name = "cmdBop";
			this.cmdBop.Size = new System.Drawing.Size(24, 24);
			this.cmdBop.TabIndex = 6;
			this.cmdBop.Text = "...";
			this.cmdBop.UseVisualStyleBackColor = true;
			this.cmdBop.Click += new System.EventHandler(this.cmdBop_Click);
			// 
			// cmdXvt
			// 
			this.cmdXvt.Location = new System.Drawing.Point(357, 15);
			this.cmdXvt.Name = "cmdXvt";
			this.cmdXvt.Size = new System.Drawing.Size(24, 24);
			this.cmdXvt.TabIndex = 3;
			this.cmdXvt.Text = "...";
			this.cmdXvt.UseVisualStyleBackColor = true;
			this.cmdXvt.Click += new System.EventHandler(this.cmdXvt_Click);
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.chkBRF8);
			this.groupBox5.Controls.Add(this.chkBRF7);
			this.groupBox5.Controls.Add(this.chkBRF6);
			this.groupBox5.Controls.Add(this.chkBRF5);
			this.groupBox5.Controls.Add(this.chkBRF4);
			this.groupBox5.Controls.Add(this.chkBRF3);
			this.groupBox5.Controls.Add(this.chkBRF2);
			this.groupBox5.Location = new System.Drawing.Point(191, 78);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(73, 144);
			this.groupBox5.TabIndex = 10;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Additional Waypoints";
			// 
			// chkBRF8
			// 
			this.chkBRF8.AutoSize = true;
			this.chkBRF8.Location = new System.Drawing.Point(6, 124);
			this.chkBRF8.Name = "chkBRF8";
			this.chkBRF8.Size = new System.Drawing.Size(53, 17);
			this.chkBRF8.TabIndex = 18;
			this.chkBRF8.Text = "BRF8";
			this.chkBRF8.UseVisualStyleBackColor = true;
			// 
			// chkBRF7
			// 
			this.chkBRF7.AutoSize = true;
			this.chkBRF7.Location = new System.Drawing.Point(6, 108);
			this.chkBRF7.Name = "chkBRF7";
			this.chkBRF7.Size = new System.Drawing.Size(53, 17);
			this.chkBRF7.TabIndex = 17;
			this.chkBRF7.Text = "BRF7";
			this.chkBRF7.UseVisualStyleBackColor = true;
			// 
			// chkBRF6
			// 
			this.chkBRF6.AutoSize = true;
			this.chkBRF6.Location = new System.Drawing.Point(6, 92);
			this.chkBRF6.Name = "chkBRF6";
			this.chkBRF6.Size = new System.Drawing.Size(53, 17);
			this.chkBRF6.TabIndex = 16;
			this.chkBRF6.Text = "BRF6";
			this.chkBRF6.UseVisualStyleBackColor = true;
			// 
			// chkBRF5
			// 
			this.chkBRF5.AutoSize = true;
			this.chkBRF5.Location = new System.Drawing.Point(6, 76);
			this.chkBRF5.Name = "chkBRF5";
			this.chkBRF5.Size = new System.Drawing.Size(53, 17);
			this.chkBRF5.TabIndex = 15;
			this.chkBRF5.Text = "BRF5";
			this.chkBRF5.UseVisualStyleBackColor = true;
			// 
			// chkBRF4
			// 
			this.chkBRF4.AutoSize = true;
			this.chkBRF4.Location = new System.Drawing.Point(6, 60);
			this.chkBRF4.Name = "chkBRF4";
			this.chkBRF4.Size = new System.Drawing.Size(53, 17);
			this.chkBRF4.TabIndex = 14;
			this.chkBRF4.Text = "BRF4";
			this.chkBRF4.UseVisualStyleBackColor = true;
			// 
			// chkBRF3
			// 
			this.chkBRF3.AutoSize = true;
			this.chkBRF3.Location = new System.Drawing.Point(6, 44);
			this.chkBRF3.Name = "chkBRF3";
			this.chkBRF3.Size = new System.Drawing.Size(53, 17);
			this.chkBRF3.TabIndex = 13;
			this.chkBRF3.Text = "BRF3";
			this.chkBRF3.UseVisualStyleBackColor = true;
			// 
			// chkBRF2
			// 
			this.chkBRF2.AutoSize = true;
			this.chkBRF2.Location = new System.Drawing.Point(6, 28);
			this.chkBRF2.Name = "chkBRF2";
			this.chkBRF2.Size = new System.Drawing.Size(53, 17);
			this.chkBRF2.TabIndex = 11;
			this.chkBRF2.Text = "BRF2";
			this.chkBRF2.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.cboXvTCraft);
			this.groupBox4.Controls.Add(this.cboXvTIFF);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Location = new System.Drawing.Point(8, 78);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(168, 128);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Default Craft";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 1;
			this.label3.Text = "Craft type";
			// 
			// cboXvTCraft
			// 
			this.cboXvTCraft.Location = new System.Drawing.Point(8, 40);
			this.cboXvTCraft.Name = "cboXvTCraft";
			this.cboXvTCraft.Size = new System.Drawing.Size(154, 21);
			this.cboXvTCraft.TabIndex = 8;
			// 
			// cboXvTIFF
			// 
			this.cboXvTIFF.Items.AddRange(new object[] {
            "Rebel",
            "Imperial",
            "Blue",
            "Yellow",
            "Red",
            "Purple"});
			this.cboXvTIFF.Location = new System.Drawing.Point(8, 96);
			this.cboXvTIFF.Name = "cboXvTIFF";
			this.cboXvTIFF.Size = new System.Drawing.Size(96, 21);
			this.cboXvTIFF.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "IFF";
			// 
			// txtXvT
			// 
			this.txtXvT.Enabled = false;
			this.txtXvT.Location = new System.Drawing.Point(110, 16);
			this.txtXvT.Name = "txtXvT";
			this.txtXvT.Size = new System.Drawing.Size(242, 20);
			this.txtXvT.TabIndex = 2;
			// 
			// chkXvTInstall
			// 
			this.chkXvTInstall.Location = new System.Drawing.Point(8, 16);
			this.chkXvTInstall.Name = "chkXvTInstall";
			this.chkXvTInstall.Size = new System.Drawing.Size(96, 24);
			this.chkXvTInstall.TabIndex = 1;
			this.chkXvTInstall.Text = "XvT Installed";
			this.chkXvTInstall.CheckedChanged += new System.EventHandler(this.chkXvTInstall_CheckedChanged);
			// 
			// txtBoP
			// 
			this.txtBoP.Enabled = false;
			this.txtBoP.Location = new System.Drawing.Point(110, 50);
			this.txtBoP.Name = "txtBoP";
			this.txtBoP.Size = new System.Drawing.Size(242, 20);
			this.txtBoP.TabIndex = 5;
			// 
			// chkBoPInstall
			// 
			this.chkBoPInstall.Location = new System.Drawing.Point(8, 48);
			this.chkBoPInstall.Name = "chkBoPInstall";
			this.chkBoPInstall.Size = new System.Drawing.Size(96, 24);
			this.chkBoPInstall.TabIndex = 4;
			this.chkBoPInstall.Text = "BoP Installed";
			this.chkBoPInstall.CheckedChanged += new System.EventHandler(this.chkBoPInstall_CheckedChanged);
			// 
			// tabOpt4
			// 
			this.tabOpt4.Controls.Add(this.lblExportWarning);
			this.tabOpt4.Controls.Add(this.chkXwaDetectMission);
			this.tabOpt4.Controls.Add(this.chkXwaFlagRemappedCraft);
			this.tabOpt4.Controls.Add(this.cmdExport);
			this.tabOpt4.Controls.Add(this.chkXwaOverrideScan);
			this.tabOpt4.Controls.Add(this.chkXwaOverrideExternal);
			this.tabOpt4.Controls.Add(this.chkBackdrops);
			this.tabOpt4.Controls.Add(this.cmdXwa);
			this.tabOpt4.Controls.Add(this.groupBox7);
			this.tabOpt4.Controls.Add(this.txtXWA);
			this.tabOpt4.Controls.Add(this.chkXWAInstall);
			this.tabOpt4.Location = new System.Drawing.Point(4, 22);
			this.tabOpt4.Name = "tabOpt4";
			this.tabOpt4.Size = new System.Drawing.Size(397, 238);
			this.tabOpt4.TabIndex = 3;
			this.tabOpt4.Text = "XWA";
			// 
			// lblExportWarning
			// 
			this.lblExportWarning.AutoSize = true;
			this.lblExportWarning.Location = new System.Drawing.Point(186, 162);
			this.lblExportWarning.Name = "lblExportWarning";
			this.lblExportWarning.Size = new System.Drawing.Size(193, 13);
			this.lblExportWarning.TabIndex = 11;
			this.lblExportWarning.Text = "Exported override in use in game folder!";
			// 
			// chkXwaDetectMission
			// 
			this.chkXwaDetectMission.AutoSize = true;
			this.chkXwaDetectMission.Location = new System.Drawing.Point(182, 56);
			this.chkXwaDetectMission.Name = "chkXwaDetectMission";
			this.chkXwaDetectMission.Size = new System.Drawing.Size(194, 17);
			this.chkXwaDetectMission.TabIndex = 5;
			this.chkXwaDetectMission.Text = "Detect installation from mission path";
			this.chkXwaDetectMission.UseVisualStyleBackColor = true;
			// 
			// chkXwaFlagRemappedCraft
			// 
			this.chkXwaFlagRemappedCraft.AutoSize = true;
			this.chkXwaFlagRemappedCraft.Location = new System.Drawing.Point(182, 116);
			this.chkXwaFlagRemappedCraft.Name = "chkXwaFlagRemappedCraft";
			this.chkXwaFlagRemappedCraft.Size = new System.Drawing.Size(202, 17);
			this.chkXwaFlagRemappedCraft.TabIndex = 8;
			this.chkXwaFlagRemappedCraft.Text = "Indicate remapped craft from scan (+)";
			this.chkXwaFlagRemappedCraft.UseVisualStyleBackColor = true;
			// 
			// cmdExport
			// 
			this.cmdExport.Location = new System.Drawing.Point(241, 136);
			this.cmdExport.Name = "cmdExport";
			this.cmdExport.Size = new System.Drawing.Size(134, 23);
			this.cmdExport.TabIndex = 9;
			this.cmdExport.Text = "Export Loaded Craft List";
			this.cmdExport.UseVisualStyleBackColor = true;
			this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
			// 
			// chkXwaOverrideScan
			// 
			this.chkXwaOverrideScan.AutoSize = true;
			this.chkXwaOverrideScan.Location = new System.Drawing.Point(182, 96);
			this.chkXwaOverrideScan.Name = "chkXwaOverrideScan";
			this.chkXwaOverrideScan.Size = new System.Drawing.Size(201, 17);
			this.chkXwaOverrideScan.TabIndex = 7;
			this.chkXwaOverrideScan.Text = "Scan craft list directly from installation";
			this.chkXwaOverrideScan.UseVisualStyleBackColor = true;
			// 
			// chkXwaOverrideExternal
			// 
			this.chkXwaOverrideExternal.AutoSize = true;
			this.chkXwaOverrideExternal.Location = new System.Drawing.Point(182, 76);
			this.chkXwaOverrideExternal.Name = "chkXwaOverrideExternal";
			this.chkXwaOverrideExternal.Size = new System.Drawing.Size(203, 17);
			this.chkXwaOverrideExternal.TabIndex = 6;
			this.chkXwaOverrideExternal.Text = "Override craft names from external file";
			this.chkXwaOverrideExternal.UseVisualStyleBackColor = true;
			// 
			// chkBackdrops
			// 
			this.chkBackdrops.Enabled = false;
			this.chkBackdrops.Location = new System.Drawing.Point(182, 193);
			this.chkBackdrops.Name = "chkBackdrops";
			this.chkBackdrops.Size = new System.Drawing.Size(185, 40);
			this.chkBackdrops.TabIndex = 10;
			this.chkBackdrops.Text = "Apply DTM Super Backdrops to new missions";
			this.chkBackdrops.UseVisualStyleBackColor = true;
			// 
			// cmdXwa
			// 
			this.cmdXwa.Location = new System.Drawing.Point(357, 15);
			this.cmdXwa.Name = "cmdXwa";
			this.cmdXwa.Size = new System.Drawing.Size(24, 24);
			this.cmdXwa.TabIndex = 3;
			this.cmdXwa.Text = "...";
			this.cmdXwa.UseVisualStyleBackColor = true;
			this.cmdXwa.Click += new System.EventHandler(this.cmdXwa_Click);
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.label5);
			this.groupBox7.Controls.Add(this.cboXWACraft);
			this.groupBox7.Controls.Add(this.cboXWAIFF);
			this.groupBox7.Controls.Add(this.label6);
			this.groupBox7.Location = new System.Drawing.Point(8, 56);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(168, 128);
			this.groupBox7.TabIndex = 4;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Default Craft";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 16);
			this.label5.TabIndex = 1;
			this.label5.Text = "Craft type";
			// 
			// cboXWACraft
			// 
			this.cboXWACraft.Location = new System.Drawing.Point(8, 40);
			this.cboXWACraft.Name = "cboXWACraft";
			this.cboXWACraft.Size = new System.Drawing.Size(154, 21);
			this.cboXWACraft.TabIndex = 3;
			// 
			// cboXWAIFF
			// 
			this.cboXWAIFF.Items.AddRange(new object[] {
            "Rebel",
            "Imperial",
            "IFF3-Blue",
            "IFF4-Yellow",
            "IFF5-Red",
            "IFF6-Purple"});
			this.cboXWAIFF.Location = new System.Drawing.Point(8, 96);
			this.cboXWAIFF.Name = "cboXWAIFF";
			this.cboXWAIFF.Size = new System.Drawing.Size(96, 21);
			this.cboXWAIFF.TabIndex = 4;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 16);
			this.label6.TabIndex = 1;
			this.label6.Text = "IFF";
			// 
			// txtXWA
			// 
			this.txtXWA.Enabled = false;
			this.txtXWA.Location = new System.Drawing.Point(86, 16);
			this.txtXWA.Name = "txtXWA";
			this.txtXWA.Size = new System.Drawing.Size(266, 20);
			this.txtXWA.TabIndex = 2;
			// 
			// chkXWAInstall
			// 
			this.chkXWAInstall.Location = new System.Drawing.Point(8, 16);
			this.chkXWAInstall.Name = "chkXWAInstall";
			this.chkXWAInstall.Size = new System.Drawing.Size(72, 24);
			this.chkXWAInstall.TabIndex = 1;
			this.chkXWAInstall.Text = "Installed";
			this.chkXWAInstall.CheckedChanged += new System.EventHandler(this.chkXWAInstall_CheckedChanged);
			// 
			// tabColors
			// 
			this.tabColors.Controls.Add(this.label14);
			this.tabColors.Controls.Add(this.groupBox8);
			this.tabColors.Controls.Add(this.chkColorizeFG);
			this.tabColors.Location = new System.Drawing.Point(4, 22);
			this.tabColors.Name = "tabColors";
			this.tabColors.Padding = new System.Windows.Forms.Padding(3);
			this.tabColors.Size = new System.Drawing.Size(397, 238);
			this.tabColors.TabIndex = 6;
			this.tabColors.Text = "Colors";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(85, 33);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(213, 13);
			this.label14.TabIndex = 13;
			this.label14.Text = "May require a program restart to take effect.";
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.cboInteractiveTheme);
			this.groupBox8.Controls.Add(this.label15);
			this.groupBox8.Controls.Add(this.cmdInteractBackground);
			this.groupBox8.Controls.Add(this.cmdInteractNonSelect);
			this.groupBox8.Controls.Add(this.cmdInteractSelect);
			this.groupBox8.Controls.Add(this.label13);
			this.groupBox8.Controls.Add(this.lblInteractNonSelected);
			this.groupBox8.Controls.Add(this.lblInteractSelected);
			this.groupBox8.Controls.Add(this.label12);
			this.groupBox8.Controls.Add(this.label11);
			this.groupBox8.Controls.Add(this.label10);
			this.groupBox8.Controls.Add(this.txtColorBackground);
			this.groupBox8.Controls.Add(this.txtColorNonSelected);
			this.groupBox8.Controls.Add(this.txtColorSelected);
			this.groupBox8.Location = new System.Drawing.Point(8, 68);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(357, 152);
			this.groupBox8.TabIndex = 12;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Interactive Label Colors";
			// 
			// cboInteractiveTheme
			// 
			this.cboInteractiveTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboInteractiveTheme.FormattingEnabled = true;
			this.cboInteractiveTheme.Items.AddRange(new object[] {
            "YOGEME",
            "XvTED / AlliED"});
			this.cboInteractiveTheme.Location = new System.Drawing.Point(226, 124);
			this.cboInteractiveTheme.Name = "cboInteractiveTheme";
			this.cboInteractiveTheme.Size = new System.Drawing.Size(109, 21);
			this.cboInteractiveTheme.TabIndex = 8;
			this.cboInteractiveTheme.SelectedIndexChanged += new System.EventHandler(this.cboInteractiveScheme_SelectedIndexChanged);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(25, 127);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(195, 13);
			this.label15.TabIndex = 18;
			this.label15.Text = "Select or restore from predefined theme:";
			// 
			// cmdInteractBackground
			// 
			this.cmdInteractBackground.Location = new System.Drawing.Point(160, 85);
			this.cmdInteractBackground.Name = "cmdInteractBackground";
			this.cmdInteractBackground.Size = new System.Drawing.Size(24, 20);
			this.cmdInteractBackground.TabIndex = 7;
			this.cmdInteractBackground.Text = "...";
			this.cmdInteractBackground.UseVisualStyleBackColor = true;
			this.cmdInteractBackground.Click += new System.EventHandler(this.cmdInteractBackground_Click);
			// 
			// cmdInteractNonSelect
			// 
			this.cmdInteractNonSelect.Location = new System.Drawing.Point(160, 59);
			this.cmdInteractNonSelect.Name = "cmdInteractNonSelect";
			this.cmdInteractNonSelect.Size = new System.Drawing.Size(24, 20);
			this.cmdInteractNonSelect.TabIndex = 5;
			this.cmdInteractNonSelect.Text = "...";
			this.cmdInteractNonSelect.UseVisualStyleBackColor = true;
			this.cmdInteractNonSelect.Click += new System.EventHandler(this.cmdInteractNonSelect_Click);
			// 
			// cmdInteractSelect
			// 
			this.cmdInteractSelect.Location = new System.Drawing.Point(160, 33);
			this.cmdInteractSelect.Name = "cmdInteractSelect";
			this.cmdInteractSelect.Size = new System.Drawing.Size(24, 20);
			this.cmdInteractSelect.TabIndex = 3;
			this.cmdInteractSelect.Text = "...";
			this.cmdInteractSelect.UseVisualStyleBackColor = true;
			this.cmdInteractSelect.Click += new System.EventHandler(this.cmdInteractSelect_Click);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(238, 24);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(90, 13);
			this.label13.TabIndex = 14;
			this.label13.Text = "Preview";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblInteractNonSelected
			// 
			this.lblInteractNonSelected.BackColor = System.Drawing.Color.RosyBrown;
			this.lblInteractNonSelected.ForeColor = System.Drawing.Color.Black;
			this.lblInteractNonSelected.Location = new System.Drawing.Point(238, 73);
			this.lblInteractNonSelected.Name = "lblInteractNonSelected";
			this.lblInteractNonSelected.Size = new System.Drawing.Size(90, 32);
			this.lblInteractNonSelected.TabIndex = 13;
			this.lblInteractNonSelected.Text = "Non-Selected";
			this.lblInteractNonSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblInteractSelected
			// 
			this.lblInteractSelected.BackColor = System.Drawing.Color.RosyBrown;
			this.lblInteractSelected.ForeColor = System.Drawing.Color.Blue;
			this.lblInteractSelected.Location = new System.Drawing.Point(238, 40);
			this.lblInteractSelected.Name = "lblInteractSelected";
			this.lblInteractSelected.Size = new System.Drawing.Size(90, 32);
			this.lblInteractSelected.TabIndex = 12;
			this.lblInteractSelected.Text = "Selected";
			this.lblInteractSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(32, 88);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(68, 13);
			this.label12.TabIndex = 11;
			this.label12.Text = "Background:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(25, 62);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(75, 13);
			this.label11.TabIndex = 10;
			this.label11.Text = "Non-Selected:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(48, 36);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(52, 13);
			this.label10.TabIndex = 9;
			this.label10.Text = "Selected:";
			// 
			// txtColorBackground
			// 
			this.txtColorBackground.Location = new System.Drawing.Point(106, 85);
			this.txtColorBackground.MaxLength = 6;
			this.txtColorBackground.Name = "txtColorBackground";
			this.txtColorBackground.Size = new System.Drawing.Size(48, 20);
			this.txtColorBackground.TabIndex = 6;
			this.txtColorBackground.Text = "BC8F8F";
			this.txtColorBackground.TextChanged += new System.EventHandler(this.txtColorBackground_TextChanged);
			// 
			// txtColorNonSelected
			// 
			this.txtColorNonSelected.Location = new System.Drawing.Point(106, 59);
			this.txtColorNonSelected.MaxLength = 6;
			this.txtColorNonSelected.Name = "txtColorNonSelected";
			this.txtColorNonSelected.Size = new System.Drawing.Size(48, 20);
			this.txtColorNonSelected.TabIndex = 4;
			this.txtColorNonSelected.Text = "000000";
			this.txtColorNonSelected.TextChanged += new System.EventHandler(this.txtColorNonSelected_TextChanged);
			// 
			// txtColorSelected
			// 
			this.txtColorSelected.Location = new System.Drawing.Point(106, 33);
			this.txtColorSelected.MaxLength = 6;
			this.txtColorSelected.Name = "txtColorSelected";
			this.txtColorSelected.Size = new System.Drawing.Size(48, 20);
			this.txtColorSelected.TabIndex = 2;
			this.txtColorSelected.Text = "0000FF";
			this.txtColorSelected.TextChanged += new System.EventHandler(this.txtColorSelected_TextChanged);
			// 
			// chkColorizeFG
			// 
			this.chkColorizeFG.AutoSize = true;
			this.chkColorizeFG.Checked = true;
			this.chkColorizeFG.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkColorizeFG.Location = new System.Drawing.Point(68, 13);
			this.chkColorizeFG.Name = "chkColorizeFG";
			this.chkColorizeFG.Size = new System.Drawing.Size(242, 17);
			this.chkColorizeFG.TabIndex = 1;
			this.chkColorizeFG.Text = "Colorize Flight Group dropdown entries by IFF.";
			this.chkColorizeFG.CheckedChanged += new System.EventHandler(this.chkColorizeFG_CheckedChanged);
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(56, 272);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(88, 32);
			this.cmdOK.TabIndex = 20;
			this.cmdOK.Text = "&OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(232, 272);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(88, 32);
			this.cmdCancel.TabIndex = 21;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// opnVerify
			// 
			this.opnVerify.FileName = "MissionVerify.exe";
			this.opnVerify.Filter = "MissionVerify|MissionVerify.exe|All programs|*.exe";
			// 
			// dirPlatform
			// 
			this.dirPlatform.Description = "Select the root install directory of the platform";
			// 
			// OptionsDialog
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(404, 305);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.tabOptions);
			this.Controls.Add(this.cmdCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "YOGEME - Options";
			this.tabOptions.ResumeLayout(false);
			this.tabOpt1.ResumeLayout(false);
			this.tabOpt1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.tabMap.ResumeLayout(false);
			this.tabMap.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMousewheelZoom)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.tabWireframe.ResumeLayout(false);
			this.tabWireframe.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numWireMeshIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numWireIconThreshold)).EndInit();
			this.tabXW.ResumeLayout(false);
			this.tabXW.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.tabOpt2.ResumeLayout(false);
			this.tabOpt2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.tabOpt3.ResumeLayout(false);
			this.tabOpt3.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.tabOpt4.ResumeLayout(false);
			this.tabOpt4.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.tabColors.ResumeLayout(false);
			this.tabColors.PerformLayout();
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		TabControl tabOptions;
		TabPage tabOpt1;
		TabPage tabOpt2;
		TabPage tabOpt3;
		TabPage tabOpt4;
		GroupBox groupBox1;
		RadioButton optStartNormal;
		RadioButton optStartPlat;
		RadioButton optStartMiss;
		CheckBox chkRestrict;
		CheckBox chkExit;
		CheckBox chkTIEInstall;
		TextBox txtTIE;
		TextBox txtBoP;
		CheckBox chkBoPInstall;
		TextBox txtXvT;
		CheckBox chkXvTInstall;
		TextBox txtXWA;
		CheckBox chkXWAInstall;
		CheckBox chkVerify;
		Button cmdOK;
		CheckBox chkSave;
		TabPage tabMap;
		CheckBox chkFG;
		CheckBox chkTrace;
		GroupBox groupBox2;
		CheckBox chkSP1;
		CheckBox chkSP2;
		CheckBox chkSP3;
		CheckBox chkSP4;
		CheckBox chkRND;
		CheckBox chkHYP;
		CheckBox chkBRF;
		CheckBox chkWP6;
		CheckBox chkWP2;
		CheckBox chkWP1;
		CheckBox chkWP3;
		CheckBox chkWP4;
		CheckBox chkWP5;
		CheckBox chkWP7;
		CheckBox chkWP8;
		GroupBox groupBox3;
		ComboBox cboTIECraft;
		ComboBox cboTIEIFF;
		Label label1;
		Label label2;
		GroupBox groupBox4;
		Label label3;
		ComboBox cboXvTCraft;
		ComboBox cboXvTIFF;
		Label label4;
		GroupBox groupBox5;
		CheckBox chkBRF8;
		CheckBox chkBRF7;
		CheckBox chkBRF6;
		CheckBox chkBRF5;
		CheckBox chkBRF4;
		CheckBox chkBRF3;
		CheckBox chkBRF2;
		private GroupBox groupBox7;
		private Label label5;
		private ComboBox cboXWACraft;
		private ComboBox cboXWAIFF;
		private Label label6;
		Button cmdCancel;
		private TextBox txtVerify;
		private Button cmdVerify;
		private OpenFileDialog opnVerify;
		private Button cmdTie;
		private Button cmdXwa;
		private FolderBrowserDialog dirPlatform;
		private Button cmdBop;
		private Button cmdXvt;
		private CheckBox chkDeletePilot;
		private CheckBox chkTest;
		private CheckBox chkVerifyTest;
		private Label label7;
        private CheckBox chkRememberPlatformFolder;
        private CheckBox chkConfirmFGDelete;
		private CheckBox chkBackdrops;
        private TabPage tabXW;
        private Button cmdXW;
        private GroupBox groupBox6;
        private Label label8;
        private ComboBox cboXWCraft;
        private ComboBox cboXWIFF;
        private Label label9;
        private TextBox txtXW;
        private CheckBox chkXWInstall;
        private TabPage tabColors;
        private GroupBox groupBox8;
        private Label lblInteractSelected;
        private Label label12;
        private Label label11;
        private Label label10;
        private TextBox txtColorBackground;
        private TextBox txtColorNonSelected;
        private TextBox txtColorSelected;
        private CheckBox chkColorizeFG;
        private Label lblInteractNonSelected;
        private Button cmdInteractSelect;
        private Label label13;
        private Button cmdInteractBackground;
        private Button cmdInteractNonSelect;
        private ColorDialog colorSelector;
        private Label label14;
        private Label label15;
        private ComboBox cboInteractiveTheme;
		private TabPage tabWireframe;
		private ListBox lstWireMeshTypes;
		private CheckBox chkWireEnabled;
		private Label lblQuickToggle;
		private Label lblDrawMeshes;
		private CheckBox chkWireToggleWeapon;
		private CheckBox chkWireToggleMisc;
		private CheckBox chkWireToggleHull;
		private NumericUpDown numWireMeshIcon;
		private NumericUpDown numWireIconThreshold;
		private Label lblMouseWheelZoom;
		private NumericUpDown numMousewheelZoom;
		private CheckBox chkWireMeshIcon;
		private CheckBox chkWireIconThreshold;
		private CheckBox chkWireToggleHangar;
		private Button cmdWireMeshDefault;
		private CheckBox chkXwaOverrideScan;
		private CheckBox chkXwaOverrideExternal;
		private Button cmdExport;
		private CheckBox chkXwaFlagRemappedCraft;
		private CheckBox chkXvtOverrideExternal;
		private CheckBox chkTieOverrideExternal;
		private CheckBox chkXwingOverrideExternal;
		private CheckBox chkXwingDetectMission;
		private CheckBox chkTieDetectMission;
		private CheckBox chkXvtDetectMission;
		private CheckBox chkXwaDetectMission;
		private Label lblExportWarning;
	}
}