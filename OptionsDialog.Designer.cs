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
			this.tabOpt2 = new System.Windows.Forms.TabPage();
			this.cmdTie = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cboTIECraft = new System.Windows.Forms.ComboBox();
			this.cboTIEIFF = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTIE = new System.Windows.Forms.TextBox();
			this.chkTIEInstall = new System.Windows.Forms.CheckBox();
			this.tabOpt3 = new System.Windows.Forms.TabPage();
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
			this.cmdXwa = new System.Windows.Forms.Button();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cboXWACraft = new System.Windows.Forms.ComboBox();
			this.cboXWAIFF = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtXWA = new System.Windows.Forms.TextBox();
			this.chkXWAInstall = new System.Windows.Forms.CheckBox();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.opnVerify = new System.Windows.Forms.OpenFileDialog();
			this.dirPlatform = new System.Windows.Forms.FolderBrowserDialog();
			this.tabOptions.SuspendLayout();
			this.tabOpt1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabMap.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabOpt2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabOpt3.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabOpt4.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabOptions
			// 
			this.tabOptions.Controls.Add(this.tabOpt1);
			this.tabOptions.Controls.Add(this.tabMap);
			this.tabOptions.Controls.Add(this.tabOpt2);
			this.tabOptions.Controls.Add(this.tabOpt3);
			this.tabOptions.Controls.Add(this.tabOpt4);
			this.tabOptions.Location = new System.Drawing.Point(0, 0);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.SelectedIndex = 0;
			this.tabOptions.Size = new System.Drawing.Size(392, 264);
			this.tabOptions.TabIndex = 0;
			// 
			// tabOpt1
			// 
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
			this.tabOpt1.Size = new System.Drawing.Size(384, 238);
			this.tabOpt1.TabIndex = 0;
			this.tabOpt1.Text = "Overall";
			// 
			// cmdVerify
			// 
			this.cmdVerify.Location = new System.Drawing.Point(350, 211);
			this.cmdVerify.Name = "cmdVerify";
			this.cmdVerify.Size = new System.Drawing.Size(24, 24);
			this.cmdVerify.TabIndex = 10;
			this.cmdVerify.Text = "...";
			this.cmdVerify.UseVisualStyleBackColor = true;
			this.cmdVerify.Click += new System.EventHandler(this.cmdVerify_Click);
			// 
			// txtVerify
			// 
			this.txtVerify.Location = new System.Drawing.Point(16, 215);
			this.txtVerify.Name = "txtVerify";
			this.txtVerify.Size = new System.Drawing.Size(328, 20);
			this.txtVerify.TabIndex = 9;
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
			this.optStartNormal.Text = "Normal";
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
			this.chkRestrict.Size = new System.Drawing.Size(224, 24);
			this.chkRestrict.TabIndex = 5;
			this.chkRestrict.Text = "Only allow editing for installed platforms";
			// 
			// chkExit
			// 
			this.chkExit.Checked = true;
			this.chkExit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkExit.Location = new System.Drawing.Point(157, 32);
			this.chkExit.Name = "chkExit";
			this.chkExit.Size = new System.Drawing.Size(224, 24);
			this.chkExit.TabIndex = 6;
			this.chkExit.Text = "Confirm exit";
			// 
			// chkVerifyTest
			// 
			this.chkVerifyTest.Checked = true;
			this.chkVerifyTest.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVerifyTest.Location = new System.Drawing.Point(157, 152);
			this.chkVerifyTest.Name = "chkVerifyTest";
			this.chkVerifyTest.Size = new System.Drawing.Size(224, 24);
			this.chkVerifyTest.TabIndex = 8;
			this.chkVerifyTest.Text = "Verify mission before test";
			// 
			// chkDeletePilot
			// 
			this.chkDeletePilot.Checked = true;
			this.chkDeletePilot.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDeletePilot.Location = new System.Drawing.Point(157, 181);
			this.chkDeletePilot.Name = "chkDeletePilot";
			this.chkDeletePilot.Size = new System.Drawing.Size(224, 24);
			this.chkDeletePilot.TabIndex = 8;
			this.chkDeletePilot.Text = "Delete Test pilot files";
			// 
			// chkTest
			// 
			this.chkTest.Checked = true;
			this.chkTest.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTest.Location = new System.Drawing.Point(157, 122);
			this.chkTest.Name = "chkTest";
			this.chkTest.Size = new System.Drawing.Size(224, 24);
			this.chkTest.TabIndex = 8;
			this.chkTest.Text = "Confirm before Testing";
			// 
			// chkVerify
			// 
			this.chkVerify.Checked = true;
			this.chkVerify.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVerify.Location = new System.Drawing.Point(157, 92);
			this.chkVerify.Name = "chkVerify";
			this.chkVerify.Size = new System.Drawing.Size(224, 24);
			this.chkVerify.TabIndex = 8;
			this.chkVerify.Text = "Verify mission on save";
			this.chkVerify.CheckedChanged += new System.EventHandler(this.chkVerify_CheckedChanged);
			// 
			// chkSave
			// 
			this.chkSave.Checked = true;
			this.chkSave.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSave.Location = new System.Drawing.Point(157, 62);
			this.chkSave.Name = "chkSave";
			this.chkSave.Size = new System.Drawing.Size(224, 24);
			this.chkSave.TabIndex = 7;
			this.chkSave.Text = "Confirm save on closing";
			// 
			// tabMap
			// 
			this.tabMap.Controls.Add(this.groupBox2);
			this.tabMap.Controls.Add(this.chkFG);
			this.tabMap.Controls.Add(this.chkTrace);
			this.tabMap.Location = new System.Drawing.Point(4, 22);
			this.tabMap.Name = "tabMap";
			this.tabMap.Size = new System.Drawing.Size(384, 238);
			this.tabMap.TabIndex = 4;
			this.tabMap.Text = "Map";
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
			// tabOpt2
			// 
			this.tabOpt2.Controls.Add(this.cmdTie);
			this.tabOpt2.Controls.Add(this.groupBox3);
			this.tabOpt2.Controls.Add(this.txtTIE);
			this.tabOpt2.Controls.Add(this.chkTIEInstall);
			this.tabOpt2.Location = new System.Drawing.Point(4, 22);
			this.tabOpt2.Name = "tabOpt2";
			this.tabOpt2.Size = new System.Drawing.Size(384, 238);
			this.tabOpt2.TabIndex = 1;
			this.tabOpt2.Text = "TIE";
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
			this.groupBox3.TabIndex = 2;
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
			this.cboTIECraft.Size = new System.Drawing.Size(160, 21);
			this.cboTIECraft.TabIndex = 3;
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
			this.cboTIEIFF.TabIndex = 4;
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
			this.tabOpt3.Size = new System.Drawing.Size(384, 238);
			this.tabOpt3.TabIndex = 2;
			this.tabOpt3.Text = "XvT, BoP";
			// 
			// cmdBop
			// 
			this.cmdBop.Location = new System.Drawing.Point(357, 49);
			this.cmdBop.Name = "cmdBop";
			this.cmdBop.Size = new System.Drawing.Size(24, 24);
			this.cmdBop.TabIndex = 9;
			this.cmdBop.Text = "...";
			this.cmdBop.UseVisualStyleBackColor = true;
			this.cmdBop.Click += new System.EventHandler(this.cmdBop_Click);
			// 
			// cmdXvt
			// 
			this.cmdXvt.Location = new System.Drawing.Point(357, 15);
			this.cmdXvt.Name = "cmdXvt";
			this.cmdXvt.Size = new System.Drawing.Size(24, 24);
			this.cmdXvt.TabIndex = 8;
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
			this.groupBox5.TabIndex = 7;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Additional Waypoints";
			// 
			// chkBRF8
			// 
			this.chkBRF8.AutoSize = true;
			this.chkBRF8.Location = new System.Drawing.Point(6, 124);
			this.chkBRF8.Name = "chkBRF8";
			this.chkBRF8.Size = new System.Drawing.Size(53, 17);
			this.chkBRF8.TabIndex = 13;
			this.chkBRF8.Text = "BRF8";
			this.chkBRF8.UseVisualStyleBackColor = true;
			// 
			// chkBRF7
			// 
			this.chkBRF7.AutoSize = true;
			this.chkBRF7.Location = new System.Drawing.Point(6, 108);
			this.chkBRF7.Name = "chkBRF7";
			this.chkBRF7.Size = new System.Drawing.Size(53, 17);
			this.chkBRF7.TabIndex = 12;
			this.chkBRF7.Text = "BRF7";
			this.chkBRF7.UseVisualStyleBackColor = true;
			// 
			// chkBRF6
			// 
			this.chkBRF6.AutoSize = true;
			this.chkBRF6.Location = new System.Drawing.Point(6, 92);
			this.chkBRF6.Name = "chkBRF6";
			this.chkBRF6.Size = new System.Drawing.Size(53, 17);
			this.chkBRF6.TabIndex = 11;
			this.chkBRF6.Text = "BRF6";
			this.chkBRF6.UseVisualStyleBackColor = true;
			// 
			// chkBRF5
			// 
			this.chkBRF5.AutoSize = true;
			this.chkBRF5.Location = new System.Drawing.Point(6, 76);
			this.chkBRF5.Name = "chkBRF5";
			this.chkBRF5.Size = new System.Drawing.Size(53, 17);
			this.chkBRF5.TabIndex = 10;
			this.chkBRF5.Text = "BRF5";
			this.chkBRF5.UseVisualStyleBackColor = true;
			// 
			// chkBRF4
			// 
			this.chkBRF4.AutoSize = true;
			this.chkBRF4.Location = new System.Drawing.Point(6, 60);
			this.chkBRF4.Name = "chkBRF4";
			this.chkBRF4.Size = new System.Drawing.Size(53, 17);
			this.chkBRF4.TabIndex = 9;
			this.chkBRF4.Text = "BRF4";
			this.chkBRF4.UseVisualStyleBackColor = true;
			// 
			// chkBRF3
			// 
			this.chkBRF3.AutoSize = true;
			this.chkBRF3.Location = new System.Drawing.Point(6, 44);
			this.chkBRF3.Name = "chkBRF3";
			this.chkBRF3.Size = new System.Drawing.Size(53, 17);
			this.chkBRF3.TabIndex = 8;
			this.chkBRF3.Text = "BRF3";
			this.chkBRF3.UseVisualStyleBackColor = true;
			// 
			// chkBRF2
			// 
			this.chkBRF2.AutoSize = true;
			this.chkBRF2.Location = new System.Drawing.Point(6, 28);
			this.chkBRF2.Name = "chkBRF2";
			this.chkBRF2.Size = new System.Drawing.Size(53, 17);
			this.chkBRF2.TabIndex = 7;
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
			this.groupBox4.TabIndex = 6;
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
			this.cboXvTCraft.Size = new System.Drawing.Size(160, 21);
			this.cboXvTCraft.TabIndex = 5;
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
			this.cboXvTIFF.TabIndex = 6;
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
			this.txtBoP.TabIndex = 4;
			// 
			// chkBoPInstall
			// 
			this.chkBoPInstall.Location = new System.Drawing.Point(8, 48);
			this.chkBoPInstall.Name = "chkBoPInstall";
			this.chkBoPInstall.Size = new System.Drawing.Size(96, 24);
			this.chkBoPInstall.TabIndex = 3;
			this.chkBoPInstall.Text = "BoP Installed";
			this.chkBoPInstall.CheckedChanged += new System.EventHandler(this.chkBoPInstall_CheckedChanged);
			// 
			// tabOpt4
			// 
			this.tabOpt4.Controls.Add(this.cmdXwa);
			this.tabOpt4.Controls.Add(this.groupBox7);
			this.tabOpt4.Controls.Add(this.txtXWA);
			this.tabOpt4.Controls.Add(this.chkXWAInstall);
			this.tabOpt4.Location = new System.Drawing.Point(4, 22);
			this.tabOpt4.Name = "tabOpt4";
			this.tabOpt4.Size = new System.Drawing.Size(384, 238);
			this.tabOpt4.TabIndex = 3;
			this.tabOpt4.Text = "XWA";
			// 
			// cmdXwa
			// 
			this.cmdXwa.Location = new System.Drawing.Point(357, 15);
			this.cmdXwa.Name = "cmdXwa";
			this.cmdXwa.Size = new System.Drawing.Size(24, 24);
			this.cmdXwa.TabIndex = 4;
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
			this.groupBox7.TabIndex = 3;
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
			this.cboXWACraft.Size = new System.Drawing.Size(160, 21);
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
			this.ClientSize = new System.Drawing.Size(392, 305);
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
			this.groupBox2.ResumeLayout(false);
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
	}
}