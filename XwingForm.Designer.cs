using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class XwingForm
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
		void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XwingForm));
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tabFG = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.lstFG = new System.Windows.Forms.ListBox();
			this.tabFGMinor = new System.Windows.Forms.TabControl();
			this.tabCraft = new System.Windows.Forms.TabPage();
			this.lblBRFNotice = new System.Windows.Forms.Label();
			this.cmdImportXWI = new System.Windows.Forms.Button();
			this.cmdSwitchFG = new System.Windows.Forms.Button();
			this.lblPlatformWarning = new System.Windows.Forms.Label();
			this.grpCraft5 = new System.Windows.Forms.GroupBox();
			this.chkPlatformGuns = new System.Windows.Forms.CheckBox();
			this.lblObjectValue = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.numObjectValue = new System.Windows.Forms.NumericUpDown();
			this.grpPlatformBitfield = new System.Windows.Forms.GroupBox();
			this.chkGun1 = new System.Windows.Forms.CheckBox();
			this.chkGun3 = new System.Windows.Forms.CheckBox();
			this.chkGun2 = new System.Windows.Forms.CheckBox();
			this.chkGun4 = new System.Windows.Forms.CheckBox();
			this.chkGun6 = new System.Windows.Forms.CheckBox();
			this.chkGun5 = new System.Windows.Forms.CheckBox();
			this.cmdMoveDown = new System.Windows.Forms.Button();
			this.cmdMoveUp = new System.Windows.Forms.Button();
			this.lblFG = new System.Windows.Forms.Label();
			this.grpCraft4 = new System.Windows.Forms.GroupBox();
			this.cboStatus = new System.Windows.Forms.ComboBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.grpCraft2 = new System.Windows.Forms.GroupBox();
			this.cboObject = new System.Windows.Forms.ComboBox();
			this.cboCraft = new System.Windows.Forms.ComboBox();
			this.label85 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.cboIFF = new System.Windows.Forms.ComboBox();
			this.cboAI = new System.Windows.Forms.ComboBox();
			this.cboMarkings = new System.Windows.Forms.ComboBox();
			this.cboPlayer = new System.Windows.Forms.ComboBox();
			this.cboFormation = new System.Windows.Forms.ComboBox();
			this.cmdForms = new System.Windows.Forms.Button();
			this.grpCraft3 = new System.Windows.Forms.GroupBox();
			this.numSeconds = new System.Windows.Forms.NumericUpDown();
			this.lblSeconds = new System.Windows.Forms.Label();
			this.numWaves = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.lblCraft = new System.Windows.Forms.Label();
			this.numCraft = new System.Windows.Forms.NumericUpDown();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblNotUsed = new System.Windows.Forms.Label();
			this.txtSpecCargo = new System.Windows.Forms.TextBox();
			this.numSC = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.txtCargo = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lblStarting = new System.Windows.Forms.Label();
			this.tabArrDep = new System.Windows.Forms.TabPage();
			this.lblArrDepNote = new System.Windows.Forms.Label();
			this.grpArrTrigger = new System.Windows.Forms.GroupBox();
			this.cboArrCondition = new System.Windows.Forms.ComboBox();
			this.numArrSec = new System.Windows.Forms.NumericUpDown();
			this.cboArrFG = new System.Windows.Forms.ComboBox();
			this.numArrMin = new System.Windows.Forms.NumericUpDown();
			this.label27 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label86 = new System.Windows.Forms.Label();
			this.cboMothership = new System.Windows.Forms.ComboBox();
			this.cmdCopyAD = new System.Windows.Forms.Button();
			this.imgToolbar = new System.Windows.Forms.ImageList(this.components);
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.optArrHyp = new System.Windows.Forms.RadioButton();
			this.optArrMS = new System.Windows.Forms.RadioButton();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.optDepHyp = new System.Windows.Forms.RadioButton();
			this.optDepMS = new System.Windows.Forms.RadioButton();
			this.cmdPasteAD = new System.Windows.Forms.Button();
			this.tabGoals = new System.Windows.Forms.TabPage();
			this.lblGoalNote = new System.Windows.Forms.Label();
			this.groupBox14 = new System.Windows.Forms.GroupBox();
			this.label32 = new System.Windows.Forms.Label();
			this.cboPrimGoalT = new System.Windows.Forms.ComboBox();
			this.tabWP = new System.Windows.Forms.TabPage();
			this.lblCS3 = new System.Windows.Forms.Label();
			this.lblCS2 = new System.Windows.Forms.Label();
			this.lblCS1 = new System.Windows.Forms.Label();
			this.lblCSInfo = new System.Windows.Forms.Label();
			this.cmdCopyWPSP = new System.Windows.Forms.Button();
			this.label80 = new System.Windows.Forms.Label();
			this.label76 = new System.Windows.Forms.Label();
			this.numRoll = new System.Windows.Forms.NumericUpDown();
			this.numPitch = new System.Windows.Forms.NumericUpDown();
			this.numYaw = new System.Windows.Forms.NumericUpDown();
			this.label56 = new System.Windows.Forms.Label();
			this.dataWP = new System.Windows.Forms.DataGrid();
			this.dataWP_Raw = new System.Windows.Forms.DataGrid();
			this.chkWPHyp = new System.Windows.Forms.CheckBox();
			this.chkWP2 = new System.Windows.Forms.CheckBox();
			this.chkWP1 = new System.Windows.Forms.CheckBox();
			this.chkSP3 = new System.Windows.Forms.CheckBox();
			this.chkSP2 = new System.Windows.Forms.CheckBox();
			this.chkSP1 = new System.Windows.Forms.CheckBox();
			this.chkWP3 = new System.Windows.Forms.CheckBox();
			this.label77 = new System.Windows.Forms.Label();
			this.label78 = new System.Windows.Forms.Label();
			this.tabOrders = new System.Windows.Forms.TabPage();
			this.lblOrderNote = new System.Windows.Forms.Label();
			this.cboOrderValue = new System.Windows.Forms.ComboBox();
			this.cboOrderSecondary = new System.Windows.Forms.ComboBox();
			this.cboOrderPrimary = new System.Windows.Forms.ComboBox();
			this.label35 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.cboOrder = new System.Windows.Forms.ComboBox();
			this.cmdCopyOrder = new System.Windows.Forms.Button();
			this.lblODesc = new System.Windows.Forms.Label();
			this.cmdPasteOrder = new System.Windows.Forms.Button();
			this.tabMission = new System.Windows.Forms.TabPage();
			this.grpMission = new System.Windows.Forms.GroupBox();
			this.label84 = new System.Windows.Forms.Label();
			this.cboMissionLocation = new System.Windows.Forms.ComboBox();
			this.cboEndEvent = new System.Windows.Forms.ComboBox();
			this.label83 = new System.Windows.Forms.Label();
			this.numUnknown1 = new System.Windows.Forms.NumericUpDown();
			this.label82 = new System.Windows.Forms.Label();
			this.numMissionTime = new System.Windows.Forms.NumericUpDown();
			this.label19 = new System.Windows.Forms.Label();
			this.lblMissionTimeNote = new System.Windows.Forms.Label();
			this.groupBox24 = new System.Windows.Forms.GroupBox();
			this.txtPrimComp1 = new System.Windows.Forms.TextBox();
			this.txtPrimComp3 = new System.Windows.Forms.TextBox();
			this.txtPrimComp2 = new System.Windows.Forms.TextBox();
			this.toolXwing = new System.Windows.Forms.ToolBar();
			this.toolNew = new System.Windows.Forms.ToolBarButton();
			this.toolOpen = new System.Windows.Forms.ToolBarButton();
			this.toolSave = new System.Windows.Forms.ToolBarButton();
			this.toolSaveAs = new System.Windows.Forms.ToolBarButton();
			this.toolSep1 = new System.Windows.Forms.ToolBarButton();
			this.toolNewItem = new System.Windows.Forms.ToolBarButton();
			this.toolDeleteItem = new System.Windows.Forms.ToolBarButton();
			this.toolCopy = new System.Windows.Forms.ToolBarButton();
			this.toolPaste = new System.Windows.Forms.ToolBarButton();
			this.toolSep2 = new System.Windows.Forms.ToolBarButton();
			this.toolMap = new System.Windows.Forms.ToolBarButton();
			this.toolBriefing = new System.Windows.Forms.ToolBarButton();
			this.toolVerify = new System.Windows.Forms.ToolBarButton();
			this.toolSep3 = new System.Windows.Forms.ToolBarButton();
			this.toolOptions = new System.Windows.Forms.ToolBarButton();
			this.toolHelp = new System.Windows.Forms.ToolBarButton();
			this.menuXwing = new System.Windows.Forms.MainMenu(this.components);
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuNew = new System.Windows.Forms.MenuItem();
			this.menuNewXwing = new System.Windows.Forms.MenuItem();
			this.menuNewTIE = new System.Windows.Forms.MenuItem();
			this.menuNewXvT = new System.Windows.Forms.MenuItem();
			this.menuNewBoP = new System.Windows.Forms.MenuItem();
			this.menuNewXWA = new System.Windows.Forms.MenuItem();
			this.menuOpen = new System.Windows.Forms.MenuItem();
			this.menuRecent = new System.Windows.Forms.MenuItem();
			this.menuRec1 = new System.Windows.Forms.MenuItem();
			this.menuRec2 = new System.Windows.Forms.MenuItem();
			this.menuRec3 = new System.Windows.Forms.MenuItem();
			this.menuRec4 = new System.Windows.Forms.MenuItem();
			this.menuRec5 = new System.Windows.Forms.MenuItem();
			this.menuSave = new System.Windows.Forms.MenuItem();
			this.menuSaveAs = new System.Windows.Forms.MenuItem();
			this.menuSaveAsTIE = new System.Windows.Forms.MenuItem();
			this.menuSaveAsXvT = new System.Windows.Forms.MenuItem();
			this.menuSaveAsBoP = new System.Windows.Forms.MenuItem();
			this.menuSaveAsXWA = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuExit = new System.Windows.Forms.MenuItem();
			this.menuEdit = new System.Windows.Forms.MenuItem();
			this.menuUndo = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuCut = new System.Windows.Forms.MenuItem();
			this.menuCopy = new System.Windows.Forms.MenuItem();
			this.menuPaste = new System.Windows.Forms.MenuItem();
			this.menuDelete = new System.Windows.Forms.MenuItem();
			this.menuTools = new System.Windows.Forms.MenuItem();
			this.menuVerify = new System.Windows.Forms.MenuItem();
			this.menuMap = new System.Windows.Forms.MenuItem();
			this.menuBriefing = new System.Windows.Forms.MenuItem();
			this.menuOptions = new System.Windows.Forms.MenuItem();
			this.menuGoalSummary = new System.Windows.Forms.MenuItem();
			this.menuTest = new System.Windows.Forms.MenuItem();
			this.menuHelp = new System.Windows.Forms.MenuItem();
			this.menuHelpInfo = new System.Windows.Forms.MenuItem();
			this.menuAbout = new System.Windows.Forms.MenuItem();
			this.menuIDMR = new System.Windows.Forms.MenuItem();
			this.menuER = new System.Windows.Forms.MenuItem();
			this.opnXW = new System.Windows.Forms.OpenFileDialog();
			this.savXW = new System.Windows.Forms.SaveFileDialog();
			this._dataWaypoints = new System.Data.DataView();
			this._dataWaypointsRaw = new System.Data.DataView();
			this.menuSaveAsXwing = new System.Windows.Forms.MenuItem();
			this.tabMain.SuspendLayout();
			this.tabFG.SuspendLayout();
			this.tabFGMinor.SuspendLayout();
			this.tabCraft.SuspendLayout();
			this.grpCraft5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numObjectValue)).BeginInit();
			this.grpPlatformBitfield.SuspendLayout();
			this.grpCraft4.SuspendLayout();
			this.grpCraft2.SuspendLayout();
			this.grpCraft3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numSeconds)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numWaves)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numCraft)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numSC)).BeginInit();
			this.tabArrDep.SuspendLayout();
			this.grpArrTrigger.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numArrSec)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numArrMin)).BeginInit();
			this.groupBox7.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.tabGoals.SuspendLayout();
			this.groupBox14.SuspendLayout();
			this.tabWP.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRoll)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPitch)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numYaw)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWP)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWP_Raw)).BeginInit();
			this.tabOrders.SuspendLayout();
			this.tabMission.SuspendLayout();
			this.grpMission.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnknown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMissionTime)).BeginInit();
			this.groupBox24.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._dataWaypoints)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._dataWaypointsRaw)).BeginInit();
			this.SuspendLayout();
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabFG);
			this.tabMain.Controls.Add(this.tabMission);
			this.tabMain.Location = new System.Drawing.Point(0, 32);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(793, 536);
			this.tabMain.TabIndex = 0;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tabFG
			// 
			this.tabFG.Controls.Add(this.label1);
			this.tabFG.Controls.Add(this.lstFG);
			this.tabFG.Controls.Add(this.tabFGMinor);
			this.tabFG.Location = new System.Drawing.Point(4, 22);
			this.tabFG.Name = "tabFG";
			this.tabFG.Size = new System.Drawing.Size(785, 510);
			this.tabFG.TabIndex = 0;
			this.tabFG.Text = "Flight Groups";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(208, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "IFF - waves x craft";
			// 
			// lstFG
			// 
			this.lstFG.BackColor = System.Drawing.Color.Black;
			this.lstFG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.lstFG.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstFG.ForeColor = System.Drawing.Color.Gray;
			this.lstFG.ItemHeight = 15;
			this.lstFG.Items.AddRange(new object[] {
            "3 - 12 - *1x(3) Ship name"});
			this.lstFG.Location = new System.Drawing.Point(8, 24);
			this.lstFG.Name = "lstFG";
			this.lstFG.Size = new System.Drawing.Size(216, 480);
			this.lstFG.TabIndex = 1;
			this.lstFG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstFG_DrawItem);
			this.lstFG.SelectedIndexChanged += new System.EventHandler(this.lstFG_SelectedIndexChanged);
			// 
			// tabFGMinor
			// 
			this.tabFGMinor.Controls.Add(this.tabCraft);
			this.tabFGMinor.Controls.Add(this.tabArrDep);
			this.tabFGMinor.Controls.Add(this.tabGoals);
			this.tabFGMinor.Controls.Add(this.tabWP);
			this.tabFGMinor.Controls.Add(this.tabOrders);
			this.tabFGMinor.Location = new System.Drawing.Point(232, 0);
			this.tabFGMinor.Name = "tabFGMinor";
			this.tabFGMinor.SelectedIndex = 0;
			this.tabFGMinor.Size = new System.Drawing.Size(552, 504);
			this.tabFGMinor.TabIndex = 0;
			// 
			// tabCraft
			// 
			this.tabCraft.Controls.Add(this.lblBRFNotice);
			this.tabCraft.Controls.Add(this.cmdImportXWI);
			this.tabCraft.Controls.Add(this.cmdSwitchFG);
			this.tabCraft.Controls.Add(this.lblPlatformWarning);
			this.tabCraft.Controls.Add(this.grpCraft5);
			this.tabCraft.Controls.Add(this.cmdMoveDown);
			this.tabCraft.Controls.Add(this.cmdMoveUp);
			this.tabCraft.Controls.Add(this.lblFG);
			this.tabCraft.Controls.Add(this.grpCraft4);
			this.tabCraft.Controls.Add(this.grpCraft2);
			this.tabCraft.Controls.Add(this.grpCraft3);
			this.tabCraft.Controls.Add(this.groupBox1);
			this.tabCraft.Controls.Add(this.lblStarting);
			this.tabCraft.Location = new System.Drawing.Point(4, 22);
			this.tabCraft.Name = "tabCraft";
			this.tabCraft.Size = new System.Drawing.Size(544, 478);
			this.tabCraft.TabIndex = 0;
			this.tabCraft.Text = "Craft";
			// 
			// lblBRFNotice
			// 
			this.lblBRFNotice.Location = new System.Drawing.Point(254, 406);
			this.lblBRFNotice.Name = "lblBRFNotice";
			this.lblBRFNotice.Size = new System.Drawing.Size(284, 31);
			this.lblBRFNotice.TabIndex = 18;
			this.lblBRFNotice.Text = "Editing BRF groups will not change the actual mission. Only basic properties and " +
    "waypoints may be edited.";
			// 
			// cmdImportXWI
			// 
			this.cmdImportXWI.Location = new System.Drawing.Point(291, 440);
			this.cmdImportXWI.Name = "cmdImportXWI";
			this.cmdImportXWI.Size = new System.Drawing.Size(125, 23);
			this.cmdImportXWI.TabIndex = 16;
			this.cmdImportXWI.Text = "Import FGs from XWI";
			this.cmdImportXWI.UseVisualStyleBackColor = true;
			this.cmdImportXWI.Click += new System.EventHandler(this.cmdImportXWI_Click);
			// 
			// cmdSwitchFG
			// 
			this.cmdSwitchFG.Location = new System.Drawing.Point(424, 440);
			this.cmdSwitchFG.Name = "cmdSwitchFG";
			this.cmdSwitchFG.Size = new System.Drawing.Size(92, 23);
			this.cmdSwitchFG.TabIndex = 17;
			this.cmdSwitchFG.Text = "Switch to BRF";
			this.cmdSwitchFG.UseVisualStyleBackColor = true;
			this.cmdSwitchFG.Click += new System.EventHandler(this.cmdSwitchFG_Click);
			// 
			// lblPlatformWarning
			// 
			this.lblPlatformWarning.Location = new System.Drawing.Point(280, 304);
			this.lblPlatformWarning.Name = "lblPlatformWarning";
			this.lblPlatformWarning.Size = new System.Drawing.Size(235, 26);
			this.lblPlatformWarning.TabIndex = 15;
			this.lblPlatformWarning.Text = "Warning! Platforms may crash the game if used outside the Pilot Proving Ground le" +
    "vels.";
			// 
			// grpCraft5
			// 
			this.grpCraft5.Controls.Add(this.chkPlatformGuns);
			this.grpCraft5.Controls.Add(this.lblObjectValue);
			this.grpCraft5.Controls.Add(this.label3);
			this.grpCraft5.Controls.Add(this.numObjectValue);
			this.grpCraft5.Controls.Add(this.grpPlatformBitfield);
			this.grpCraft5.Location = new System.Drawing.Point(279, 174);
			this.grpCraft5.Name = "grpCraft5";
			this.grpCraft5.Size = new System.Drawing.Size(241, 119);
			this.grpCraft5.TabIndex = 14;
			this.grpCraft5.TabStop = false;
			this.grpCraft5.Text = "Training Platform Gun Controls";
			// 
			// chkPlatformGuns
			// 
			this.chkPlatformGuns.AutoSize = true;
			this.chkPlatformGuns.Location = new System.Drawing.Point(25, 21);
			this.chkPlatformGuns.Name = "chkPlatformGuns";
			this.chkPlatformGuns.Size = new System.Drawing.Size(102, 17);
			this.chkPlatformGuns.TabIndex = 21;
			this.chkPlatformGuns.Text = "Guns are hostile";
			this.chkPlatformGuns.UseVisualStyleBackColor = true;
			this.chkPlatformGuns.CheckedChanged += new System.EventHandler(this.chkPlatformGuns_CheckedChanged);
			// 
			// lblObjectValue
			// 
			this.lblObjectValue.AutoSize = true;
			this.lblObjectValue.Location = new System.Drawing.Point(22, 46);
			this.lblObjectValue.Name = "lblObjectValue";
			this.lblObjectValue.Size = new System.Drawing.Size(62, 13);
			this.lblObjectValue.TabIndex = 20;
			this.lblObjectValue.Text = "Raw Value:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(9, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 43);
			this.label3.TabIndex = 24;
			this.label3.Text = "Same values produce different patterns depending on platform.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// numObjectValue
			// 
			this.numObjectValue.Location = new System.Drawing.Point(87, 44);
			this.numObjectValue.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
			this.numObjectValue.Name = "numObjectValue";
			this.numObjectValue.Size = new System.Drawing.Size(52, 20);
			this.numObjectValue.TabIndex = 22;
			this.numObjectValue.ValueChanged += new System.EventHandler(this.numObjectValue_ValueChanged);
			// 
			// grpPlatformBitfield
			// 
			this.grpPlatformBitfield.Controls.Add(this.chkGun1);
			this.grpPlatformBitfield.Controls.Add(this.chkGun3);
			this.grpPlatformBitfield.Controls.Add(this.chkGun2);
			this.grpPlatformBitfield.Controls.Add(this.chkGun4);
			this.grpPlatformBitfield.Controls.Add(this.chkGun6);
			this.grpPlatformBitfield.Controls.Add(this.chkGun5);
			this.grpPlatformBitfield.Location = new System.Drawing.Point(145, 19);
			this.grpPlatformBitfield.Name = "grpPlatformBitfield";
			this.grpPlatformBitfield.Size = new System.Drawing.Size(81, 81);
			this.grpPlatformBitfield.TabIndex = 22;
			this.grpPlatformBitfield.TabStop = false;
			this.grpPlatformBitfield.Text = "Placement";
			// 
			// chkGun1
			// 
			this.chkGun1.AutoSize = true;
			this.chkGun1.Location = new System.Drawing.Point(23, 58);
			this.chkGun1.Name = "chkGun1";
			this.chkGun1.Size = new System.Drawing.Size(15, 14);
			this.chkGun1.TabIndex = 4;
			this.chkGun1.UseVisualStyleBackColor = true;
			// 
			// chkGun3
			// 
			this.chkGun3.AutoSize = true;
			this.chkGun3.Location = new System.Drawing.Point(23, 38);
			this.chkGun3.Name = "chkGun3";
			this.chkGun3.Size = new System.Drawing.Size(15, 14);
			this.chkGun3.TabIndex = 2;
			this.chkGun3.UseVisualStyleBackColor = true;
			// 
			// chkGun2
			// 
			this.chkGun2.AutoSize = true;
			this.chkGun2.Location = new System.Drawing.Point(44, 58);
			this.chkGun2.Name = "chkGun2";
			this.chkGun2.Size = new System.Drawing.Size(15, 14);
			this.chkGun2.TabIndex = 5;
			this.chkGun2.UseVisualStyleBackColor = true;
			// 
			// chkGun4
			// 
			this.chkGun4.AutoSize = true;
			this.chkGun4.Location = new System.Drawing.Point(44, 38);
			this.chkGun4.Name = "chkGun4";
			this.chkGun4.Size = new System.Drawing.Size(15, 14);
			this.chkGun4.TabIndex = 3;
			this.chkGun4.UseVisualStyleBackColor = true;
			// 
			// chkGun6
			// 
			this.chkGun6.AutoSize = true;
			this.chkGun6.Location = new System.Drawing.Point(44, 18);
			this.chkGun6.Name = "chkGun6";
			this.chkGun6.Size = new System.Drawing.Size(15, 14);
			this.chkGun6.TabIndex = 1;
			this.chkGun6.UseVisualStyleBackColor = true;
			// 
			// chkGun5
			// 
			this.chkGun5.AutoSize = true;
			this.chkGun5.Location = new System.Drawing.Point(23, 18);
			this.chkGun5.Name = "chkGun5";
			this.chkGun5.Size = new System.Drawing.Size(15, 14);
			this.chkGun5.TabIndex = 0;
			this.chkGun5.UseVisualStyleBackColor = true;
			// 
			// cmdMoveDown
			// 
			this.cmdMoveDown.Location = new System.Drawing.Point(445, 374);
			this.cmdMoveDown.Name = "cmdMoveDown";
			this.cmdMoveDown.Size = new System.Drawing.Size(75, 23);
			this.cmdMoveDown.TabIndex = 13;
			this.cmdMoveDown.Text = "Move Down";
			this.cmdMoveDown.UseVisualStyleBackColor = true;
			this.cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
			// 
			// cmdMoveUp
			// 
			this.cmdMoveUp.Location = new System.Drawing.Point(445, 349);
			this.cmdMoveUp.Name = "cmdMoveUp";
			this.cmdMoveUp.Size = new System.Drawing.Size(75, 23);
			this.cmdMoveUp.TabIndex = 12;
			this.cmdMoveUp.Text = "Move Up";
			this.cmdMoveUp.UseVisualStyleBackColor = true;
			this.cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
			// 
			// lblFG
			// 
			this.lblFG.Location = new System.Drawing.Point(296, 349);
			this.lblFG.Name = "lblFG";
			this.lblFG.Size = new System.Drawing.Size(120, 16);
			this.lblFG.TabIndex = 11;
			this.lblFG.Text = "Flight Group #0 of 0";
			// 
			// grpCraft4
			// 
			this.grpCraft4.Controls.Add(this.cboStatus);
			this.grpCraft4.Controls.Add(this.lblStatus);
			this.grpCraft4.Location = new System.Drawing.Point(279, 114);
			this.grpCraft4.Name = "grpCraft4";
			this.grpCraft4.Size = new System.Drawing.Size(241, 54);
			this.grpCraft4.TabIndex = 10;
			this.grpCraft4.TabStop = false;
			// 
			// cboStatus
			// 
			this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboStatus.Location = new System.Drawing.Point(78, 15);
			this.cboStatus.Name = "cboStatus";
			this.cboStatus.Size = new System.Drawing.Size(136, 21);
			this.cboStatus.TabIndex = 23;
			this.cboStatus.Leave += new System.EventHandler(this.cboStatus_Leave);
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(17, 15);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(37, 13);
			this.lblStatus.TabIndex = 0;
			this.lblStatus.Text = "Status";
			// 
			// grpCraft2
			// 
			this.grpCraft2.Controls.Add(this.cboObject);
			this.grpCraft2.Controls.Add(this.cboCraft);
			this.grpCraft2.Controls.Add(this.label85);
			this.grpCraft2.Controls.Add(this.label18);
			this.grpCraft2.Controls.Add(this.label7);
			this.grpCraft2.Controls.Add(this.label8);
			this.grpCraft2.Controls.Add(this.label9);
			this.grpCraft2.Controls.Add(this.label13);
			this.grpCraft2.Controls.Add(this.label14);
			this.grpCraft2.Controls.Add(this.label15);
			this.grpCraft2.Controls.Add(this.cboIFF);
			this.grpCraft2.Controls.Add(this.cboAI);
			this.grpCraft2.Controls.Add(this.cboMarkings);
			this.grpCraft2.Controls.Add(this.cboPlayer);
			this.grpCraft2.Controls.Add(this.cboFormation);
			this.grpCraft2.Controls.Add(this.cmdForms);
			this.grpCraft2.Location = new System.Drawing.Point(16, 184);
			this.grpCraft2.Name = "grpCraft2";
			this.grpCraft2.Size = new System.Drawing.Size(232, 279);
			this.grpCraft2.TabIndex = 9;
			this.grpCraft2.TabStop = false;
			this.grpCraft2.Leave += new System.EventHandler(this.grpCraft2_Leave);
			// 
			// cboObject
			// 
			this.cboObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboObject.Location = new System.Drawing.Point(88, 44);
			this.cboObject.Name = "cboObject";
			this.cboObject.Size = new System.Drawing.Size(136, 21);
			this.cboObject.TabIndex = 11;
			this.cboObject.SelectedIndexChanged += new System.EventHandler(this.cboObject_SelectedIndexChanged);
			// 
			// cboCraft
			// 
			this.cboCraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCraft.Location = new System.Drawing.Point(88, 16);
			this.cboCraft.Name = "cboCraft";
			this.cboCraft.Size = new System.Drawing.Size(136, 21);
			this.cboCraft.TabIndex = 10;
			this.cboCraft.SelectedIndexChanged += new System.EventHandler(this.cboCraft_SelectedIndexChanged);
			// 
			// label85
			// 
			this.label85.Location = new System.Drawing.Point(16, 31);
			this.label85.Name = "label85";
			this.label85.Size = new System.Drawing.Size(80, 16);
			this.label85.TabIndex = 0;
			this.label85.Text = " OR ";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(8, 47);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(80, 16);
			this.label18.TabIndex = 0;
			this.label18.Text = "Object Type";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "Craft Type";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 69);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "IFF";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 93);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 16);
			this.label9.TabIndex = 0;
			this.label9.Text = "AI skill";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 141);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(72, 16);
			this.label13.TabIndex = 0;
			this.label13.Text = "Player";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 117);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(80, 16);
			this.label14.TabIndex = 0;
			this.label14.Text = "Markings";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(8, 165);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(80, 16);
			this.label15.TabIndex = 0;
			this.label15.Text = "Formation";
			// 
			// cboIFF
			// 
			this.cboIFF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboIFF.Location = new System.Drawing.Point(88, 69);
			this.cboIFF.Name = "cboIFF";
			this.cboIFF.Size = new System.Drawing.Size(136, 21);
			this.cboIFF.TabIndex = 12;
			// 
			// cboAI
			// 
			this.cboAI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAI.Location = new System.Drawing.Point(88, 93);
			this.cboAI.Name = "cboAI";
			this.cboAI.Size = new System.Drawing.Size(136, 21);
			this.cboAI.TabIndex = 13;
			// 
			// cboMarkings
			// 
			this.cboMarkings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMarkings.Location = new System.Drawing.Point(88, 117);
			this.cboMarkings.Name = "cboMarkings";
			this.cboMarkings.Size = new System.Drawing.Size(136, 21);
			this.cboMarkings.TabIndex = 14;
			// 
			// cboPlayer
			// 
			this.cboPlayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPlayer.Items.AddRange(new object[] {
            "AI",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
			this.cboPlayer.Location = new System.Drawing.Point(88, 141);
			this.cboPlayer.Name = "cboPlayer";
			this.cboPlayer.Size = new System.Drawing.Size(136, 21);
			this.cboPlayer.TabIndex = 15;
			// 
			// cboFormation
			// 
			this.cboFormation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFormation.Location = new System.Drawing.Point(88, 165);
			this.cboFormation.Name = "cboFormation";
			this.cboFormation.Size = new System.Drawing.Size(136, 21);
			this.cboFormation.TabIndex = 16;
			this.cboFormation.SelectedIndexChanged += new System.EventHandler(this.cboFormation_SelectedIndexChanged);
			// 
			// cmdForms
			// 
			this.cmdForms.Location = new System.Drawing.Point(160, 189);
			this.cmdForms.Name = "cmdForms";
			this.cmdForms.Size = new System.Drawing.Size(64, 24);
			this.cmdForms.TabIndex = 19;
			this.cmdForms.Text = "&Forms...";
			this.cmdForms.Click += new System.EventHandler(this.cmdForms_Click);
			// 
			// grpCraft3
			// 
			this.grpCraft3.Controls.Add(this.numSeconds);
			this.grpCraft3.Controls.Add(this.lblSeconds);
			this.grpCraft3.Controls.Add(this.numWaves);
			this.grpCraft3.Controls.Add(this.label10);
			this.grpCraft3.Controls.Add(this.lblCraft);
			this.grpCraft3.Controls.Add(this.numCraft);
			this.grpCraft3.Location = new System.Drawing.Point(280, 24);
			this.grpCraft3.Name = "grpCraft3";
			this.grpCraft3.Size = new System.Drawing.Size(240, 84);
			this.grpCraft3.TabIndex = 8;
			this.grpCraft3.TabStop = false;
			this.grpCraft3.Leave += new System.EventHandler(this.grpCraft3_Leave);
			// 
			// numSeconds
			// 
			this.numSeconds.Location = new System.Drawing.Point(162, 40);
			this.numSeconds.Name = "numSeconds";
			this.numSeconds.Size = new System.Drawing.Size(40, 20);
			this.numSeconds.TabIndex = 23;
			this.numSeconds.ValueChanged += new System.EventHandler(this.numSeconds_ValueChanged);
			// 
			// lblSeconds
			// 
			this.lblSeconds.AutoSize = true;
			this.lblSeconds.Location = new System.Drawing.Point(153, 16);
			this.lblSeconds.Name = "lblSeconds";
			this.lblSeconds.Size = new System.Drawing.Size(49, 13);
			this.lblSeconds.TabIndex = 22;
			this.lblSeconds.Text = "Seconds";
			// 
			// numWaves
			// 
			this.numWaves.Location = new System.Drawing.Point(24, 40);
			this.numWaves.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numWaves.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWaves.Name = "numWaves";
			this.numWaves.Size = new System.Drawing.Size(40, 20);
			this.numWaves.TabIndex = 20;
			this.numWaves.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 16);
			this.label10.TabIndex = 0;
			this.label10.Text = "# of waves";
			// 
			// lblCraft
			// 
			this.lblCraft.Location = new System.Drawing.Point(96, 16);
			this.lblCraft.Name = "lblCraft";
			this.lblCraft.Size = new System.Drawing.Size(51, 16);
			this.lblCraft.TabIndex = 0;
			this.lblCraft.Text = "# of craft";
			// 
			// numCraft
			// 
			this.numCraft.Location = new System.Drawing.Point(104, 40);
			this.numCraft.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numCraft.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numCraft.Name = "numCraft";
			this.numCraft.Size = new System.Drawing.Size(40, 20);
			this.numCraft.TabIndex = 21;
			this.numCraft.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtName);
			this.groupBox1.Controls.Add(this.lblNotUsed);
			this.groupBox1.Controls.Add(this.txtSpecCargo);
			this.groupBox1.Controls.Add(this.numSC);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtCargo);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Location = new System.Drawing.Point(16, 24);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 144);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(88, 16);
			this.txtName.MaxLength = 16;
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(128, 20);
			this.txtName.TabIndex = 4;
			this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
			// 
			// lblNotUsed
			// 
			this.lblNotUsed.Location = new System.Drawing.Point(88, 88);
			this.lblNotUsed.Name = "lblNotUsed";
			this.lblNotUsed.Size = new System.Drawing.Size(80, 16);
			this.lblNotUsed.TabIndex = 3;
			this.lblNotUsed.Text = "(not used)";
			// 
			// txtSpecCargo
			// 
			this.txtSpecCargo.Location = new System.Drawing.Point(88, 88);
			this.txtSpecCargo.MaxLength = 12;
			this.txtSpecCargo.Name = "txtSpecCargo";
			this.txtSpecCargo.Size = new System.Drawing.Size(128, 20);
			this.txtSpecCargo.TabIndex = 7;
			this.txtSpecCargo.Visible = false;
			this.txtSpecCargo.Leave += new System.EventHandler(this.txtSpecCargo_Leave);
			// 
			// numSC
			// 
			this.numSC.Location = new System.Drawing.Point(96, 112);
			this.numSC.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.numSC.Name = "numSC";
			this.numSC.Size = new System.Drawing.Size(32, 20);
			this.numSC.TabIndex = 8;
			this.numSC.ValueChanged += new System.EventHandler(this.numSC_ValueChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Cargo";
			// 
			// txtCargo
			// 
			this.txtCargo.Location = new System.Drawing.Point(88, 64);
			this.txtCargo.MaxLength = 16;
			this.txtCargo.Name = "txtCargo";
			this.txtCargo.Size = new System.Drawing.Size(128, 20);
			this.txtCargo.TabIndex = 6;
			this.txtCargo.Leave += new System.EventHandler(this.txtCargo_Leave);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Special Cargo";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Name";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 112);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 16);
			this.label6.TabIndex = 3;
			this.label6.Text = "Special Ship #";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblStarting
			// 
			this.lblStarting.Location = new System.Drawing.Point(296, 374);
			this.lblStarting.Name = "lblStarting";
			this.lblStarting.Size = new System.Drawing.Size(120, 32);
			this.lblStarting.TabIndex = 11;
			this.lblStarting.Text = "1 Craft at 30 seconds";
			// 
			// tabArrDep
			// 
			this.tabArrDep.Controls.Add(this.lblArrDepNote);
			this.tabArrDep.Controls.Add(this.grpArrTrigger);
			this.tabArrDep.Controls.Add(this.label86);
			this.tabArrDep.Controls.Add(this.cboMothership);
			this.tabArrDep.Controls.Add(this.cmdCopyAD);
			this.tabArrDep.Controls.Add(this.groupBox7);
			this.tabArrDep.Controls.Add(this.groupBox9);
			this.tabArrDep.Controls.Add(this.cmdPasteAD);
			this.tabArrDep.Location = new System.Drawing.Point(4, 22);
			this.tabArrDep.Name = "tabArrDep";
			this.tabArrDep.Size = new System.Drawing.Size(544, 478);
			this.tabArrDep.TabIndex = 1;
			this.tabArrDep.Text = "Arr/Dep";
			// 
			// lblArrDepNote
			// 
			this.lblArrDepNote.AutoSize = true;
			this.lblArrDepNote.Location = new System.Drawing.Point(102, 291);
			this.lblArrDepNote.Name = "lblArrDepNote";
			this.lblArrDepNote.Size = new System.Drawing.Size(136, 13);
			this.lblArrDepNote.TabIndex = 32;
			this.lblArrDepNote.Text = "Not available in BRF mode.";
			this.lblArrDepNote.Visible = false;
			// 
			// grpArrTrigger
			// 
			this.grpArrTrigger.Controls.Add(this.cboArrCondition);
			this.grpArrTrigger.Controls.Add(this.numArrSec);
			this.grpArrTrigger.Controls.Add(this.cboArrFG);
			this.grpArrTrigger.Controls.Add(this.numArrMin);
			this.grpArrTrigger.Controls.Add(this.label27);
			this.grpArrTrigger.Controls.Add(this.label26);
			this.grpArrTrigger.Controls.Add(this.label23);
			this.grpArrTrigger.Controls.Add(this.label25);
			this.grpArrTrigger.Controls.Add(this.label24);
			this.grpArrTrigger.Location = new System.Drawing.Point(105, 135);
			this.grpArrTrigger.Name = "grpArrTrigger";
			this.grpArrTrigger.Size = new System.Drawing.Size(256, 105);
			this.grpArrTrigger.TabIndex = 31;
			this.grpArrTrigger.TabStop = false;
			this.grpArrTrigger.Text = "Arrival Trigger:";
			// 
			// cboArrCondition
			// 
			this.cboArrCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboArrCondition.FormattingEnabled = true;
			this.cboArrCondition.Location = new System.Drawing.Point(84, 46);
			this.cboArrCondition.Name = "cboArrCondition";
			this.cboArrCondition.Size = new System.Drawing.Size(153, 21);
			this.cboArrCondition.TabIndex = 4;
			this.cboArrCondition.SelectedIndexChanged += new System.EventHandler(this.cboArrCondition_SelectedIndexChanged);
			// 
			// numArrSec
			// 
			this.numArrSec.Increment = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.numArrSec.Location = new System.Drawing.Point(155, 73);
			this.numArrSec.Maximum = new decimal(new int[] {
            54,
            0,
            0,
            0});
			this.numArrSec.Name = "numArrSec";
			this.numArrSec.Size = new System.Drawing.Size(46, 20);
			this.numArrSec.TabIndex = 6;
			this.numArrSec.ValueChanged += new System.EventHandler(this.numArrSec_ValueChanged);
			// 
			// cboArrFG
			// 
			this.cboArrFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboArrFG.FormattingEnabled = true;
			this.cboArrFG.Location = new System.Drawing.Point(84, 19);
			this.cboArrFG.Name = "cboArrFG";
			this.cboArrFG.Size = new System.Drawing.Size(153, 21);
			this.cboArrFG.TabIndex = 3;
			this.cboArrFG.SelectedIndexChanged += new System.EventHandler(this.cboArrFG_SelectedIndexChanged);
			// 
			// numArrMin
			// 
			this.numArrMin.Location = new System.Drawing.Point(65, 73);
			this.numArrMin.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numArrMin.Name = "numArrMin";
			this.numArrMin.Size = new System.Drawing.Size(46, 20);
			this.numArrMin.TabIndex = 5;
			this.numArrMin.ValueChanged += new System.EventHandler(this.numArrMin_ValueChanged);
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(24, 49);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(54, 13);
			this.label27.TabIndex = 28;
			this.label27.Text = "Condition:";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(12, 22);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(67, 13);
			this.label26.TabIndex = 27;
			this.label26.Text = "Flight Group:";
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(24, 72);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(40, 16);
			this.label23.TabIndex = 4;
			this.label23.Text = "Delay:";
			this.label23.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(207, 75);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(26, 13);
			this.label25.TabIndex = 4;
			this.label25.Text = "Sec";
			this.label25.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(117, 75);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(24, 13);
			this.label24.TabIndex = 4;
			this.label24.Text = "Min";
			this.label24.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label86
			// 
			this.label86.AutoSize = true;
			this.label86.Location = new System.Drawing.Point(102, 96);
			this.label86.Name = "label86";
			this.label86.Size = new System.Drawing.Size(62, 13);
			this.label86.TabIndex = 26;
			this.label86.Text = "Mothership:";
			// 
			// cboMothership
			// 
			this.cboMothership.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMothership.Location = new System.Drawing.Point(170, 93);
			this.cboMothership.Name = "cboMothership";
			this.cboMothership.Size = new System.Drawing.Size(136, 21);
			this.cboMothership.TabIndex = 2;
			this.cboMothership.SelectedIndexChanged += new System.EventHandler(this.cboMothership_SelectedIndexChanged);
			// 
			// cmdCopyAD
			// 
			this.cmdCopyAD.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdCopyAD.ImageIndex = 6;
			this.cmdCopyAD.ImageList = this.imgToolbar;
			this.cmdCopyAD.Location = new System.Drawing.Point(32, 15);
			this.cmdCopyAD.Name = "cmdCopyAD";
			this.cmdCopyAD.Size = new System.Drawing.Size(24, 23);
			this.cmdCopyAD.TabIndex = 24;
			this.cmdCopyAD.Click += new System.EventHandler(this.cmdCopyAD_Click);
			// 
			// imgToolbar
			// 
			this.imgToolbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolbar.ImageStream")));
			this.imgToolbar.TransparentColor = System.Drawing.Color.Transparent;
			this.imgToolbar.Images.SetKeyName(0, "");
			this.imgToolbar.Images.SetKeyName(1, "");
			this.imgToolbar.Images.SetKeyName(2, "");
			this.imgToolbar.Images.SetKeyName(3, "");
			this.imgToolbar.Images.SetKeyName(4, "");
			this.imgToolbar.Images.SetKeyName(5, "");
			this.imgToolbar.Images.SetKeyName(6, "");
			this.imgToolbar.Images.SetKeyName(7, "");
			this.imgToolbar.Images.SetKeyName(8, "");
			this.imgToolbar.Images.SetKeyName(9, "");
			this.imgToolbar.Images.SetKeyName(10, "");
			this.imgToolbar.Images.SetKeyName(11, "");
			this.imgToolbar.Images.SetKeyName(12, "");
			this.imgToolbar.Images.SetKeyName(13, "");
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.optArrHyp);
			this.groupBox7.Controls.Add(this.optArrMS);
			this.groupBox7.Location = new System.Drawing.Point(105, 15);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(125, 72);
			this.groupBox7.TabIndex = 0;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Arrive Via:";
			// 
			// optArrHyp
			// 
			this.optArrHyp.Checked = true;
			this.optArrHyp.Location = new System.Drawing.Point(16, 16);
			this.optArrHyp.Name = "optArrHyp";
			this.optArrHyp.Size = new System.Drawing.Size(104, 24);
			this.optArrHyp.TabIndex = 0;
			this.optArrHyp.TabStop = true;
			this.optArrHyp.Text = "Hyperspace";
			this.optArrHyp.CheckedChanged += new System.EventHandler(this.optArrHyp_CheckedChanged);
			// 
			// optArrMS
			// 
			this.optArrMS.Location = new System.Drawing.Point(16, 40);
			this.optArrMS.Name = "optArrMS";
			this.optArrMS.Size = new System.Drawing.Size(80, 24);
			this.optArrMS.TabIndex = 1;
			this.optArrMS.Text = "Mothership";
			this.optArrMS.CheckedChanged += new System.EventHandler(this.optArrMS_CheckedChanged);
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.optDepHyp);
			this.groupBox9.Controls.Add(this.optDepMS);
			this.groupBox9.Location = new System.Drawing.Point(236, 15);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(125, 72);
			this.groupBox9.TabIndex = 0;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Depart Via:";
			// 
			// optDepHyp
			// 
			this.optDepHyp.Checked = true;
			this.optDepHyp.Location = new System.Drawing.Point(16, 16);
			this.optDepHyp.Name = "optDepHyp";
			this.optDepHyp.Size = new System.Drawing.Size(104, 24);
			this.optDepHyp.TabIndex = 6;
			this.optDepHyp.TabStop = true;
			this.optDepHyp.Text = "Hyperspace";
			this.optDepHyp.CheckedChanged += new System.EventHandler(this.optDepHyp_CheckedChanged);
			// 
			// optDepMS
			// 
			this.optDepMS.Location = new System.Drawing.Point(16, 40);
			this.optDepMS.Name = "optDepMS";
			this.optDepMS.Size = new System.Drawing.Size(80, 24);
			this.optDepMS.TabIndex = 7;
			this.optDepMS.Text = "Mothership";
			this.optDepMS.CheckedChanged += new System.EventHandler(this.optDepMS_CheckedChanged);
			// 
			// cmdPasteAD
			// 
			this.cmdPasteAD.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdPasteAD.ImageIndex = 7;
			this.cmdPasteAD.ImageList = this.imgToolbar;
			this.cmdPasteAD.Location = new System.Drawing.Point(62, 15);
			this.cmdPasteAD.Name = "cmdPasteAD";
			this.cmdPasteAD.Size = new System.Drawing.Size(24, 23);
			this.cmdPasteAD.TabIndex = 25;
			this.cmdPasteAD.Click += new System.EventHandler(this.cmdPasteAD_Click);
			// 
			// tabGoals
			// 
			this.tabGoals.Controls.Add(this.lblGoalNote);
			this.tabGoals.Controls.Add(this.groupBox14);
			this.tabGoals.Location = new System.Drawing.Point(4, 22);
			this.tabGoals.Name = "tabGoals";
			this.tabGoals.Size = new System.Drawing.Size(544, 478);
			this.tabGoals.TabIndex = 3;
			this.tabGoals.Text = "Goals";
			// 
			// lblGoalNote
			// 
			this.lblGoalNote.AutoSize = true;
			this.lblGoalNote.Location = new System.Drawing.Point(188, 116);
			this.lblGoalNote.Name = "lblGoalNote";
			this.lblGoalNote.Size = new System.Drawing.Size(136, 13);
			this.lblGoalNote.TabIndex = 33;
			this.lblGoalNote.Text = "Not available in BRF mode.";
			this.lblGoalNote.Visible = false;
			// 
			// groupBox14
			// 
			this.groupBox14.Controls.Add(this.label32);
			this.groupBox14.Controls.Add(this.cboPrimGoalT);
			this.groupBox14.Location = new System.Drawing.Point(8, 24);
			this.groupBox14.Name = "groupBox14";
			this.groupBox14.Size = new System.Drawing.Size(528, 72);
			this.groupBox14.TabIndex = 0;
			this.groupBox14.TabStop = false;
			this.groupBox14.Text = "Mission Goal";
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(55, 31);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(120, 16);
			this.label32.TabIndex = 1;
			this.label32.Text = "The flight group must";
			this.label32.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// cboPrimGoalT
			// 
			this.cboPrimGoalT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPrimGoalT.Location = new System.Drawing.Point(183, 31);
			this.cboPrimGoalT.Name = "cboPrimGoalT";
			this.cboPrimGoalT.Size = new System.Drawing.Size(184, 21);
			this.cboPrimGoalT.TabIndex = 1;
			this.cboPrimGoalT.SelectedIndexChanged += new System.EventHandler(this.cboPrimGoalT_SelectedIndexChanged);
			// 
			// tabWP
			// 
			this.tabWP.Controls.Add(this.lblCS3);
			this.tabWP.Controls.Add(this.lblCS2);
			this.tabWP.Controls.Add(this.lblCS1);
			this.tabWP.Controls.Add(this.lblCSInfo);
			this.tabWP.Controls.Add(this.cmdCopyWPSP);
			this.tabWP.Controls.Add(this.label80);
			this.tabWP.Controls.Add(this.label76);
			this.tabWP.Controls.Add(this.numRoll);
			this.tabWP.Controls.Add(this.numPitch);
			this.tabWP.Controls.Add(this.numYaw);
			this.tabWP.Controls.Add(this.label56);
			this.tabWP.Controls.Add(this.dataWP);
			this.tabWP.Controls.Add(this.dataWP_Raw);
			this.tabWP.Controls.Add(this.chkWPHyp);
			this.tabWP.Controls.Add(this.chkWP2);
			this.tabWP.Controls.Add(this.chkWP1);
			this.tabWP.Controls.Add(this.chkSP3);
			this.tabWP.Controls.Add(this.chkSP2);
			this.tabWP.Controls.Add(this.chkSP1);
			this.tabWP.Controls.Add(this.chkWP3);
			this.tabWP.Controls.Add(this.label77);
			this.tabWP.Controls.Add(this.label78);
			this.tabWP.Location = new System.Drawing.Point(4, 22);
			this.tabWP.Name = "tabWP";
			this.tabWP.Size = new System.Drawing.Size(544, 478);
			this.tabWP.TabIndex = 4;
			this.tabWP.Text = "Waypoints";
			// 
			// lblCS3
			// 
			this.lblCS3.AutoSize = true;
			this.lblCS3.Location = new System.Drawing.Point(192, 242);
			this.lblCS3.Name = "lblCS3";
			this.lblCS3.Size = new System.Drawing.Size(70, 13);
			this.lblCS3.TabIndex = 23;
			this.lblCS3.Text = "* Coord Set 4";
			// 
			// lblCS2
			// 
			this.lblCS2.AutoSize = true;
			this.lblCS2.Location = new System.Drawing.Point(192, 222);
			this.lblCS2.Name = "lblCS2";
			this.lblCS2.Size = new System.Drawing.Size(70, 13);
			this.lblCS2.TabIndex = 23;
			this.lblCS2.Text = "* Coord Set 3";
			// 
			// lblCS1
			// 
			this.lblCS1.AutoSize = true;
			this.lblCS1.Location = new System.Drawing.Point(192, 202);
			this.lblCS1.Name = "lblCS1";
			this.lblCS1.Size = new System.Drawing.Size(70, 13);
			this.lblCS1.TabIndex = 23;
			this.lblCS1.Text = "* Coord Set 2";
			// 
			// lblCSInfo
			// 
			this.lblCSInfo.Location = new System.Drawing.Point(189, 272);
			this.lblCSInfo.Name = "lblCSInfo";
			this.lblCSInfo.Size = new System.Drawing.Size(265, 85);
			this.lblCSInfo.TabIndex = 22;
			this.lblCSInfo.Text = resources.GetString("lblCSInfo.Text");
			// 
			// cmdCopyWPSP
			// 
			this.cmdCopyWPSP.Location = new System.Drawing.Point(279, 62);
			this.cmdCopyWPSP.Name = "cmdCopyWPSP";
			this.cmdCopyWPSP.Size = new System.Drawing.Size(75, 38);
			this.cmdCopyWPSP.TabIndex = 20;
			this.cmdCopyWPSP.Text = "Copy SP1 to 2 and 3";
			this.cmdCopyWPSP.UseVisualStyleBackColor = true;
			this.cmdCopyWPSP.Click += new System.EventHandler(this.cmdCopyWPSP_Click);
			// 
			// label80
			// 
			this.label80.Location = new System.Drawing.Point(342, 408);
			this.label80.Name = "label80";
			this.label80.Size = new System.Drawing.Size(178, 28);
			this.label80.TabIndex = 19;
			this.label80.Text = "Note: interface displays corrected pitch angle";
			// 
			// label76
			// 
			this.label76.Image = ((System.Drawing.Image)(resources.GetObject("label76.Image")));
			this.label76.Location = new System.Drawing.Point(248, 376);
			this.label76.Name = "label76";
			this.label76.Size = new System.Drawing.Size(20, 20);
			this.label76.TabIndex = 6;
			// 
			// numRoll
			// 
			this.numRoll.Location = new System.Drawing.Point(288, 440);
			this.numRoll.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numRoll.Minimum = new decimal(new int[] {
            179,
            0,
            0,
            -2147483648});
			this.numRoll.Name = "numRoll";
			this.numRoll.Size = new System.Drawing.Size(48, 20);
			this.numRoll.TabIndex = 18;
			this.numRoll.Leave += new System.EventHandler(this.numRoll_Leave);
			// 
			// numPitch
			// 
			this.numPitch.Location = new System.Drawing.Point(288, 408);
			this.numPitch.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numPitch.Minimum = new decimal(new int[] {
            179,
            0,
            0,
            -2147483648});
			this.numPitch.Name = "numPitch";
			this.numPitch.Size = new System.Drawing.Size(48, 20);
			this.numPitch.TabIndex = 17;
			this.numPitch.Leave += new System.EventHandler(this.numPitch_Leave);
			// 
			// numYaw
			// 
			this.numYaw.Location = new System.Drawing.Point(288, 376);
			this.numYaw.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numYaw.Minimum = new decimal(new int[] {
            179,
            0,
            0,
            -2147483648});
			this.numYaw.Name = "numYaw";
			this.numYaw.Size = new System.Drawing.Size(48, 20);
			this.numYaw.TabIndex = 16;
			this.numYaw.Leave += new System.EventHandler(this.numYaw_Leave);
			// 
			// label56
			// 
			this.label56.Location = new System.Drawing.Point(368, 16);
			this.label56.Name = "label56";
			this.label56.Size = new System.Drawing.Size(144, 16);
			this.label56.TabIndex = 1;
			this.label56.Text = "Raw Data - position*160";
			// 
			// dataWP
			// 
			this.dataWP.AllowSorting = false;
			this.dataWP.CaptionVisible = false;
			this.dataWP.DataMember = "";
			this.dataWP.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataWP.Location = new System.Drawing.Point(24, 40);
			this.dataWP.Name = "dataWP";
			this.dataWP.PreferredColumnWidth = 52;
			this.dataWP.PreferredRowHeight = 20;
			this.dataWP.RowHeadersVisible = false;
			this.dataWP.Size = new System.Drawing.Size(160, 224);
			this.dataWP.TabIndex = 0;
			// 
			// dataWP_Raw
			// 
			this.dataWP_Raw.AllowSorting = false;
			this.dataWP_Raw.CaptionVisible = false;
			this.dataWP_Raw.DataMember = "";
			this.dataWP_Raw.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataWP_Raw.Location = new System.Drawing.Point(360, 40);
			this.dataWP_Raw.Name = "dataWP_Raw";
			this.dataWP_Raw.PreferredColumnWidth = 52;
			this.dataWP_Raw.PreferredRowHeight = 20;
			this.dataWP_Raw.RowHeadersVisible = false;
			this.dataWP_Raw.Size = new System.Drawing.Size(160, 224);
			this.dataWP_Raw.TabIndex = 0;
			// 
			// chkWPHyp
			// 
			this.chkWPHyp.Location = new System.Drawing.Point(192, 182);
			this.chkWPHyp.Name = "chkWPHyp";
			this.chkWPHyp.Size = new System.Drawing.Size(96, 16);
			this.chkWPHyp.TabIndex = 14;
			this.chkWPHyp.Text = "Hyperspace";
			// 
			// chkWP2
			// 
			this.chkWP2.Location = new System.Drawing.Point(192, 142);
			this.chkWP2.Name = "chkWP2";
			this.chkWP2.Size = new System.Drawing.Size(96, 16);
			this.chkWP2.TabIndex = 6;
			this.chkWP2.Text = "Waypoint 2";
			// 
			// chkWP1
			// 
			this.chkWP1.Location = new System.Drawing.Point(192, 122);
			this.chkWP1.Name = "chkWP1";
			this.chkWP1.Size = new System.Drawing.Size(96, 16);
			this.chkWP1.TabIndex = 5;
			this.chkWP1.Text = "Waypoint 1";
			// 
			// chkSP3
			// 
			this.chkSP3.Location = new System.Drawing.Point(192, 102);
			this.chkSP3.Name = "chkSP3";
			this.chkSP3.Size = new System.Drawing.Size(96, 16);
			this.chkSP3.TabIndex = 3;
			this.chkSP3.Text = "Start Point 3";
			// 
			// chkSP2
			// 
			this.chkSP2.Location = new System.Drawing.Point(192, 82);
			this.chkSP2.Name = "chkSP2";
			this.chkSP2.Size = new System.Drawing.Size(96, 16);
			this.chkSP2.TabIndex = 2;
			this.chkSP2.Text = "Start Point 2";
			// 
			// chkSP1
			// 
			this.chkSP1.Location = new System.Drawing.Point(192, 62);
			this.chkSP1.Name = "chkSP1";
			this.chkSP1.Size = new System.Drawing.Size(96, 16);
			this.chkSP1.TabIndex = 1;
			this.chkSP1.Text = "Start Point 1";
			// 
			// chkWP3
			// 
			this.chkWP3.Location = new System.Drawing.Point(192, 162);
			this.chkWP3.Name = "chkWP3";
			this.chkWP3.Size = new System.Drawing.Size(96, 16);
			this.chkWP3.TabIndex = 7;
			this.chkWP3.Text = "Waypoint 3";
			// 
			// label77
			// 
			this.label77.Image = ((System.Drawing.Image)(resources.GetObject("label77.Image")));
			this.label77.Location = new System.Drawing.Point(248, 408);
			this.label77.Name = "label77";
			this.label77.Size = new System.Drawing.Size(20, 20);
			this.label77.TabIndex = 6;
			// 
			// label78
			// 
			this.label78.Image = ((System.Drawing.Image)(resources.GetObject("label78.Image")));
			this.label78.Location = new System.Drawing.Point(248, 440);
			this.label78.Name = "label78";
			this.label78.Size = new System.Drawing.Size(20, 20);
			this.label78.TabIndex = 6;
			// 
			// tabOrders
			// 
			this.tabOrders.Controls.Add(this.lblOrderNote);
			this.tabOrders.Controls.Add(this.cboOrderValue);
			this.tabOrders.Controls.Add(this.cboOrderSecondary);
			this.tabOrders.Controls.Add(this.cboOrderPrimary);
			this.tabOrders.Controls.Add(this.label35);
			this.tabOrders.Controls.Add(this.label34);
			this.tabOrders.Controls.Add(this.label33);
			this.tabOrders.Controls.Add(this.label28);
			this.tabOrders.Controls.Add(this.cboOrder);
			this.tabOrders.Controls.Add(this.cmdCopyOrder);
			this.tabOrders.Controls.Add(this.lblODesc);
			this.tabOrders.Controls.Add(this.cmdPasteOrder);
			this.tabOrders.Location = new System.Drawing.Point(4, 22);
			this.tabOrders.Name = "tabOrders";
			this.tabOrders.Size = new System.Drawing.Size(544, 478);
			this.tabOrders.TabIndex = 2;
			this.tabOrders.Text = "Orders";
			// 
			// lblOrderNote
			// 
			this.lblOrderNote.AutoSize = true;
			this.lblOrderNote.Location = new System.Drawing.Point(205, 190);
			this.lblOrderNote.Name = "lblOrderNote";
			this.lblOrderNote.Size = new System.Drawing.Size(136, 13);
			this.lblOrderNote.TabIndex = 34;
			this.lblOrderNote.Text = "Not available in BRF mode.";
			this.lblOrderNote.Visible = false;
			// 
			// cboOrderValue
			// 
			this.cboOrderValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOrderValue.FormattingEnabled = true;
			this.cboOrderValue.Location = new System.Drawing.Point(250, 125);
			this.cboOrderValue.Name = "cboOrderValue";
			this.cboOrderValue.Size = new System.Drawing.Size(121, 21);
			this.cboOrderValue.TabIndex = 25;
			this.cboOrderValue.SelectedIndexChanged += new System.EventHandler(this.cboOrderValue_SelectedIndexChanged);
			// 
			// cboOrderSecondary
			// 
			this.cboOrderSecondary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOrderSecondary.FormattingEnabled = true;
			this.cboOrderSecondary.Location = new System.Drawing.Point(250, 100);
			this.cboOrderSecondary.Name = "cboOrderSecondary";
			this.cboOrderSecondary.Size = new System.Drawing.Size(121, 21);
			this.cboOrderSecondary.TabIndex = 24;
			this.cboOrderSecondary.SelectedIndexChanged += new System.EventHandler(this.cboOrderSecondary_SelectedIndexChanged);
			// 
			// cboOrderPrimary
			// 
			this.cboOrderPrimary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOrderPrimary.FormattingEnabled = true;
			this.cboOrderPrimary.Location = new System.Drawing.Point(250, 75);
			this.cboOrderPrimary.Name = "cboOrderPrimary";
			this.cboOrderPrimary.Size = new System.Drawing.Size(121, 21);
			this.cboOrderPrimary.TabIndex = 23;
			this.cboOrderPrimary.SelectedIndexChanged += new System.EventHandler(this.cboOrderPrimary_SelectedIndexChanged);
			// 
			// label35
			// 
			this.label35.AutoSize = true;
			this.label35.Location = new System.Drawing.Point(90, 128);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(154, 13);
			this.label35.TabIndex = 22;
			this.label35.Text = "Dock Time (minutes) / Throttle:";
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Location = new System.Drawing.Point(149, 103);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(95, 13);
			this.label34.TabIndex = 21;
			this.label34.Text = "Secondary Target:";
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Location = new System.Drawing.Point(166, 78);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(78, 13);
			this.label33.TabIndex = 20;
			this.label33.Text = "Primary Target:";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(166, 53);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(36, 13);
			this.label28.TabIndex = 19;
			this.label28.Text = "Order:";
			// 
			// cboOrder
			// 
			this.cboOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOrder.FormattingEnabled = true;
			this.cboOrder.Location = new System.Drawing.Point(208, 50);
			this.cboOrder.Name = "cboOrder";
			this.cboOrder.Size = new System.Drawing.Size(213, 21);
			this.cboOrder.TabIndex = 18;
			this.cboOrder.SelectedIndexChanged += new System.EventHandler(this.cboOrder_SelectedIndexChanged);
			// 
			// cmdCopyOrder
			// 
			this.cmdCopyOrder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdCopyOrder.ImageIndex = 6;
			this.cmdCopyOrder.ImageList = this.imgToolbar;
			this.cmdCopyOrder.Location = new System.Drawing.Point(48, 48);
			this.cmdCopyOrder.Name = "cmdCopyOrder";
			this.cmdCopyOrder.Size = new System.Drawing.Size(24, 23);
			this.cmdCopyOrder.TabIndex = 16;
			this.cmdCopyOrder.Click += new System.EventHandler(this.cmdCopyOrder_Click);
			// 
			// lblODesc
			// 
			this.lblODesc.Location = new System.Drawing.Point(16, 70);
			this.lblODesc.Name = "lblODesc";
			this.lblODesc.Size = new System.Drawing.Size(512, 16);
			this.lblODesc.TabIndex = 8;
			// 
			// cmdPasteOrder
			// 
			this.cmdPasteOrder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdPasteOrder.ImageIndex = 7;
			this.cmdPasteOrder.ImageList = this.imgToolbar;
			this.cmdPasteOrder.Location = new System.Drawing.Point(78, 50);
			this.cmdPasteOrder.Name = "cmdPasteOrder";
			this.cmdPasteOrder.Size = new System.Drawing.Size(24, 23);
			this.cmdPasteOrder.TabIndex = 17;
			this.cmdPasteOrder.Click += new System.EventHandler(this.cmdPasteOrder_Click);
			// 
			// tabMission
			// 
			this.tabMission.Controls.Add(this.grpMission);
			this.tabMission.Controls.Add(this.lblMissionTimeNote);
			this.tabMission.Controls.Add(this.groupBox24);
			this.tabMission.Location = new System.Drawing.Point(4, 22);
			this.tabMission.Name = "tabMission";
			this.tabMission.Size = new System.Drawing.Size(785, 510);
			this.tabMission.TabIndex = 4;
			this.tabMission.Text = "Mission";
			// 
			// grpMission
			// 
			this.grpMission.Controls.Add(this.label84);
			this.grpMission.Controls.Add(this.cboMissionLocation);
			this.grpMission.Controls.Add(this.cboEndEvent);
			this.grpMission.Controls.Add(this.label83);
			this.grpMission.Controls.Add(this.numUnknown1);
			this.grpMission.Controls.Add(this.label82);
			this.grpMission.Controls.Add(this.numMissionTime);
			this.grpMission.Controls.Add(this.label19);
			this.grpMission.Location = new System.Drawing.Point(20, 12);
			this.grpMission.Name = "grpMission";
			this.grpMission.Size = new System.Drawing.Size(384, 123);
			this.grpMission.TabIndex = 27;
			this.grpMission.TabStop = false;
			this.grpMission.Leave += new System.EventHandler(this.grpMission_Leave);
			// 
			// label84
			// 
			this.label84.AutoSize = true;
			this.label84.Location = new System.Drawing.Point(61, 36);
			this.label84.Name = "label84";
			this.label84.Size = new System.Drawing.Size(60, 13);
			this.label84.TabIndex = 25;
			this.label84.Text = "End Event:";
			// 
			// cboMissionLocation
			// 
			this.cboMissionLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMissionLocation.FormattingEnabled = true;
			this.cboMissionLocation.Location = new System.Drawing.Point(127, 86);
			this.cboMissionLocation.Name = "cboMissionLocation";
			this.cboMissionLocation.Size = new System.Drawing.Size(148, 21);
			this.cboMissionLocation.TabIndex = 23;
			// 
			// cboEndEvent
			// 
			this.cboEndEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEndEvent.FormattingEnabled = true;
			this.cboEndEvent.Location = new System.Drawing.Point(127, 33);
			this.cboEndEvent.Name = "cboEndEvent";
			this.cboEndEvent.Size = new System.Drawing.Size(148, 21);
			this.cboEndEvent.TabIndex = 21;
			// 
			// label83
			// 
			this.label83.AutoSize = true;
			this.label83.Location = new System.Drawing.Point(70, 89);
			this.label83.Name = "label83";
			this.label83.Size = new System.Drawing.Size(51, 13);
			this.label83.TabIndex = 23;
			this.label83.Text = "Location:";
			// 
			// numUnknown1
			// 
			this.numUnknown1.Location = new System.Drawing.Point(127, 60);
			this.numUnknown1.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numUnknown1.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
			this.numUnknown1.Name = "numUnknown1";
			this.numUnknown1.Size = new System.Drawing.Size(48, 20);
			this.numUnknown1.TabIndex = 22;
			// 
			// label82
			// 
			this.label82.AutoSize = true;
			this.label82.Location = new System.Drawing.Point(59, 62);
			this.label82.Name = "label82";
			this.label82.Size = new System.Drawing.Size(62, 13);
			this.label82.TabIndex = 21;
			this.label82.Text = "Unknown1:";
			// 
			// numMissionTime
			// 
			this.numMissionTime.Location = new System.Drawing.Point(127, 9);
			this.numMissionTime.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numMissionTime.Name = "numMissionTime";
			this.numMissionTime.Size = new System.Drawing.Size(48, 20);
			this.numMissionTime.TabIndex = 20;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(5, 11);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(116, 13);
			this.label19.TabIndex = 18;
			this.label19.Text = "Mission Time (minutes):";
			// 
			// lblMissionTimeNote
			// 
			this.lblMissionTimeNote.Location = new System.Drawing.Point(410, 21);
			this.lblMissionTimeNote.Name = "lblMissionTimeNote";
			this.lblMissionTimeNote.Size = new System.Drawing.Size(251, 55);
			this.lblMissionTimeNote.TabIndex = 26;
			this.lblMissionTimeNote.Text = "This mission time is not used for Pilot Proving Ground levels.  Instead it\'s stor" +
    "ed in the parameters for the Training Platform 1 object.";
			// 
			// groupBox24
			// 
			this.groupBox24.Controls.Add(this.txtPrimComp1);
			this.groupBox24.Controls.Add(this.txtPrimComp3);
			this.groupBox24.Controls.Add(this.txtPrimComp2);
			this.groupBox24.Location = new System.Drawing.Point(20, 150);
			this.groupBox24.Name = "groupBox24";
			this.groupBox24.Size = new System.Drawing.Size(384, 116);
			this.groupBox24.TabIndex = 0;
			this.groupBox24.TabStop = false;
			this.groupBox24.Text = "Mission Complete Messages";
			// 
			// txtPrimComp1
			// 
			this.txtPrimComp1.BackColor = System.Drawing.Color.Black;
			this.txtPrimComp1.ForeColor = System.Drawing.Color.Lime;
			this.txtPrimComp1.Location = new System.Drawing.Point(16, 24);
			this.txtPrimComp1.MaxLength = 64;
			this.txtPrimComp1.Name = "txtPrimComp1";
			this.txtPrimComp1.Size = new System.Drawing.Size(352, 20);
			this.txtPrimComp1.TabIndex = 10;
			// 
			// txtPrimComp3
			// 
			this.txtPrimComp3.BackColor = System.Drawing.Color.Black;
			this.txtPrimComp3.ForeColor = System.Drawing.Color.Lime;
			this.txtPrimComp3.Location = new System.Drawing.Point(16, 84);
			this.txtPrimComp3.MaxLength = 64;
			this.txtPrimComp3.Name = "txtPrimComp3";
			this.txtPrimComp3.Size = new System.Drawing.Size(352, 20);
			this.txtPrimComp3.TabIndex = 12;
			// 
			// txtPrimComp2
			// 
			this.txtPrimComp2.BackColor = System.Drawing.Color.Black;
			this.txtPrimComp2.ForeColor = System.Drawing.Color.Lime;
			this.txtPrimComp2.Location = new System.Drawing.Point(16, 54);
			this.txtPrimComp2.MaxLength = 64;
			this.txtPrimComp2.Name = "txtPrimComp2";
			this.txtPrimComp2.Size = new System.Drawing.Size(352, 20);
			this.txtPrimComp2.TabIndex = 11;
			// 
			// toolXwing
			// 
			this.toolXwing.AutoSize = false;
			this.toolXwing.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolNew,
            this.toolOpen,
            this.toolSave,
            this.toolSaveAs,
            this.toolSep1,
            this.toolNewItem,
            this.toolDeleteItem,
            this.toolCopy,
            this.toolPaste,
            this.toolSep2,
            this.toolMap,
            this.toolBriefing,
            this.toolVerify,
            this.toolSep3,
            this.toolOptions,
            this.toolHelp});
			this.toolXwing.DropDownArrows = true;
			this.toolXwing.ImageList = this.imgToolbar;
			this.toolXwing.Location = new System.Drawing.Point(0, 0);
			this.toolXwing.Name = "toolXwing";
			this.toolXwing.ShowToolTips = true;
			this.toolXwing.Size = new System.Drawing.Size(794, 30);
			this.toolXwing.TabIndex = 1;
			this.toolXwing.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolXwing_ButtonClick);
			// 
			// toolNew
			// 
			this.toolNew.ImageIndex = 0;
			this.toolNew.Name = "toolNew";
			this.toolNew.ToolTipText = "New Mission";
			// 
			// toolOpen
			// 
			this.toolOpen.ImageIndex = 1;
			this.toolOpen.Name = "toolOpen";
			this.toolOpen.ToolTipText = "Open Mission";
			// 
			// toolSave
			// 
			this.toolSave.ImageIndex = 2;
			this.toolSave.Name = "toolSave";
			this.toolSave.ToolTipText = "Save Mission";
			// 
			// toolSaveAs
			// 
			this.toolSaveAs.ImageIndex = 3;
			this.toolSaveAs.Name = "toolSaveAs";
			this.toolSaveAs.ToolTipText = "Save As...";
			// 
			// toolSep1
			// 
			this.toolSep1.Name = "toolSep1";
			this.toolSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			this.toolSep1.Text = "    ";
			// 
			// toolNewItem
			// 
			this.toolNewItem.ImageIndex = 4;
			this.toolNewItem.Name = "toolNewItem";
			this.toolNewItem.ToolTipText = "New FlightGroup";
			// 
			// toolDeleteItem
			// 
			this.toolDeleteItem.ImageIndex = 5;
			this.toolDeleteItem.Name = "toolDeleteItem";
			this.toolDeleteItem.ToolTipText = "Delete FlightGroup";
			// 
			// toolCopy
			// 
			this.toolCopy.ImageIndex = 6;
			this.toolCopy.Name = "toolCopy";
			this.toolCopy.ToolTipText = "Copy FlightGroup";
			// 
			// toolPaste
			// 
			this.toolPaste.ImageIndex = 7;
			this.toolPaste.Name = "toolPaste";
			this.toolPaste.ToolTipText = "Paste FlightGroup";
			// 
			// toolSep2
			// 
			this.toolSep2.Name = "toolSep2";
			this.toolSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolMap
			// 
			this.toolMap.ImageIndex = 8;
			this.toolMap.Name = "toolMap";
			this.toolMap.ToolTipText = "Mission Map";
			// 
			// toolBriefing
			// 
			this.toolBriefing.ImageIndex = 9;
			this.toolBriefing.Name = "toolBriefing";
			this.toolBriefing.ToolTipText = "Briefing";
			// 
			// toolVerify
			// 
			this.toolVerify.Enabled = false;
			this.toolVerify.ImageIndex = 11;
			this.toolVerify.Name = "toolVerify";
			this.toolVerify.ToolTipText = "Mission Verify";
			// 
			// toolSep3
			// 
			this.toolSep3.Name = "toolSep3";
			this.toolSep3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolOptions
			// 
			this.toolOptions.ImageIndex = 10;
			this.toolOptions.Name = "toolOptions";
			this.toolOptions.ToolTipText = "Options";
			// 
			// toolHelp
			// 
			this.toolHelp.ImageIndex = 13;
			this.toolHelp.Name = "toolHelp";
			this.toolHelp.ToolTipText = "Help";
			// 
			// menuXwing
			// 
			this.menuXwing.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuTools,
            this.menuTest,
            this.menuHelp});
			// 
			// menuFile
			// 
			this.menuFile.Index = 0;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuRecent,
            this.menuSave,
            this.menuSaveAs,
            this.menuItem7,
            this.menuExit});
			this.menuFile.Text = "&File";
			// 
			// menuNew
			// 
			this.menuNew.Index = 0;
			this.menuNew.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNewXwing,
            this.menuNewTIE,
            this.menuNewXvT,
            this.menuNewBoP,
            this.menuNewXWA});
			this.menuNew.Text = "&New...";
			// 
			// menuNewXwing
			// 
			this.menuNewXwing.Index = 0;
			this.menuNewXwing.Text = "X-&wing mission";
			this.menuNewXwing.Click += new System.EventHandler(this.menuNewXwing_Click);
			// 
			// menuNewTIE
			// 
			this.menuNewTIE.Index = 1;
			this.menuNewTIE.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.menuNewTIE.Text = "&TIE mission";
			this.menuNewTIE.Click += new System.EventHandler(this.menuNewTIE_Click);
			// 
			// menuNewXvT
			// 
			this.menuNewXvT.Index = 2;
			this.menuNewXvT.Text = "X&vT mission";
			this.menuNewXvT.Click += new System.EventHandler(this.menuNewXvT_Click);
			// 
			// menuNewBoP
			// 
			this.menuNewBoP.Index = 3;
			this.menuNewBoP.Text = "&BoP mission";
			this.menuNewBoP.Click += new System.EventHandler(this.menuNewBoP_Click);
			// 
			// menuNewXWA
			// 
			this.menuNewXWA.Index = 4;
			this.menuNewXWA.Text = "&XWA mission";
			this.menuNewXWA.Click += new System.EventHandler(this.menuNewXWA_Click);
			// 
			// menuOpen
			// 
			this.menuOpen.Index = 1;
			this.menuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuOpen.Text = "&Open...";
			this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
			// 
			// menuRecent
			// 
			this.menuRecent.Enabled = false;
			this.menuRecent.Index = 2;
			this.menuRecent.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuRec1,
            this.menuRec2,
            this.menuRec3,
            this.menuRec4,
            this.menuRec5});
			this.menuRecent.Text = "Open &Recent...";
			// 
			// menuRec1
			// 
			this.menuRec1.Index = 0;
			this.menuRec1.Text = "1.";
			this.menuRec1.Visible = false;
			// 
			// menuRec2
			// 
			this.menuRec2.Index = 1;
			this.menuRec2.Text = "2.";
			this.menuRec2.Visible = false;
			// 
			// menuRec3
			// 
			this.menuRec3.Index = 2;
			this.menuRec3.Text = "3.";
			this.menuRec3.Visible = false;
			// 
			// menuRec4
			// 
			this.menuRec4.Index = 3;
			this.menuRec4.Text = "4.";
			this.menuRec4.Visible = false;
			// 
			// menuRec5
			// 
			this.menuRec5.Index = 4;
			this.menuRec5.Text = "5.";
			this.menuRec5.Visible = false;
			// 
			// menuSave
			// 
			this.menuSave.Index = 3;
			this.menuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuSave.Text = "&Save";
			this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
			// 
			// menuSaveAs
			// 
			this.menuSaveAs.Index = 4;
			this.menuSaveAs.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSaveAsTIE,
            this.menuSaveAsXvT,
            this.menuSaveAsBoP,
            this.menuSaveAsXWA,
            this.menuSaveAsXwing});
			this.menuSaveAs.Text = "Save &As...";
			// 
			// menuSaveAsTIE
			// 
			this.menuSaveAsTIE.Index = 0;
			this.menuSaveAsTIE.Text = "&TIE mission";
			this.menuSaveAsTIE.Click += new System.EventHandler(this.menuSaveAsTIE_Click);
			// 
			// menuSaveAsXvT
			// 
			this.menuSaveAsXvT.Index = 1;
			this.menuSaveAsXvT.Text = "X&vT mission";
			this.menuSaveAsXvT.Click += new System.EventHandler(this.menuSaveAsXvT_Click);
			// 
			// menuSaveAsBoP
			// 
			this.menuSaveAsBoP.Index = 2;
			this.menuSaveAsBoP.Text = "&BoP mission";
			this.menuSaveAsBoP.Click += new System.EventHandler(this.menuSaveAsBoP_Click);
			// 
			// menuSaveAsXWA
			// 
			this.menuSaveAsXWA.Index = 3;
			this.menuSaveAsXWA.Text = "&XWA mission";
			this.menuSaveAsXWA.Click += new System.EventHandler(this.menuSaveAsXWA_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 5;
			this.menuItem7.Text = "-";
			// 
			// menuExit
			// 
			this.menuExit.Index = 6;
			this.menuExit.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftX;
			this.menuExit.Text = "E&xit";
			this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
			// 
			// menuEdit
			// 
			this.menuEdit.Index = 1;
			this.menuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuUndo,
            this.menuItem6,
            this.menuCut,
            this.menuCopy,
            this.menuPaste,
            this.menuDelete});
			this.menuEdit.Text = "&Edit";
			// 
			// menuUndo
			// 
			this.menuUndo.Enabled = false;
			this.menuUndo.Index = 0;
			this.menuUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.menuUndo.Text = "&Undo";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.Text = "-";
			// 
			// menuCut
			// 
			this.menuCut.Index = 2;
			this.menuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.menuCut.Text = "Cu&t";
			// 
			// menuCopy
			// 
			this.menuCopy.Index = 3;
			this.menuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuCopy.Text = "&Copy";
			this.menuCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// menuPaste
			// 
			this.menuPaste.Index = 4;
			this.menuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.menuPaste.Text = "&Paste";
			this.menuPaste.Click += new System.EventHandler(this.menuPaste_Click);
			// 
			// menuDelete
			// 
			this.menuDelete.Index = 5;
			this.menuDelete.Text = "&Delete";
			this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// menuTools
			// 
			this.menuTools.Index = 2;
			this.menuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuVerify,
            this.menuMap,
            this.menuBriefing,
            this.menuOptions,
            this.menuGoalSummary});
			this.menuTools.Text = "&Tools";
			// 
			// menuVerify
			// 
			this.menuVerify.Enabled = false;
			this.menuVerify.Index = 0;
			this.menuVerify.Text = "&Verify Mission";
			this.menuVerify.Click += new System.EventHandler(this.menuVerify_Click);
			// 
			// menuMap
			// 
			this.menuMap.Index = 1;
			this.menuMap.Text = "&Map";
			this.menuMap.Click += new System.EventHandler(this.menuMap_Click);
			// 
			// menuBriefing
			// 
			this.menuBriefing.Index = 2;
			this.menuBriefing.Text = "&Briefing";
			this.menuBriefing.Click += new System.EventHandler(this.menuBriefing_Click);
			// 
			// menuOptions
			// 
			this.menuOptions.Index = 3;
			this.menuOptions.Text = "&Options...";
			this.menuOptions.Click += new System.EventHandler(this.menuOptions_Click);
			// 
			// menuGoalSummary
			// 
			this.menuGoalSummary.Index = 4;
			this.menuGoalSummary.Text = "FG &Goal Summary";
			this.menuGoalSummary.Click += new System.EventHandler(this.menuGoalSummary_Click);
			// 
			// menuTest
			// 
			this.menuTest.Enabled = false;
			this.menuTest.Index = 3;
			this.menuTest.Text = "Te&st";
			this.menuTest.Click += new System.EventHandler(this.menuTest_Click);
			// 
			// menuHelp
			// 
			this.menuHelp.Index = 4;
			this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuHelpInfo,
            this.menuAbout,
            this.menuIDMR,
            this.menuER});
			this.menuHelp.Text = "&Help";
			// 
			// menuHelpInfo
			// 
			this.menuHelpInfo.Index = 0;
			this.menuHelpInfo.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.menuHelpInfo.Text = "&Help";
			this.menuHelpInfo.Click += new System.EventHandler(this.menuHelpInfo_Click);
			// 
			// menuAbout
			// 
			this.menuAbout.Index = 1;
			this.menuAbout.Text = "&About";
			this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
			// 
			// menuIDMR
			// 
			this.menuIDMR.Index = 2;
			this.menuIDMR.Text = "&IDMR.ER.net";
			this.menuIDMR.Click += new System.EventHandler(this.menuIDMR_Click);
			// 
			// menuER
			// 
			this.menuER.Index = 3;
			this.menuER.Text = "&Empirereborn.net";
			this.menuER.Click += new System.EventHandler(this.menuER_Click);
			// 
			// opnXW
			// 
			this.opnXW.DefaultExt = "xwi";
			this.opnXW.Filter = "X-wing Missions|*.xwi|Mission Files|*.tie";
			// 
			// savXW
			// 
			this.savXW.DefaultExt = "xwi";
			this.savXW.FileName = "NewMission.xwi";
			this.savXW.Filter = "X-wing Missions|*.xwi|Mission Files|*.tie";
			this.savXW.FileOk += new System.ComponentModel.CancelEventHandler(this.savXW_FileOk);
			// 
			// _dataWaypoints
			// 
			this._dataWaypoints.AllowDelete = false;
			this._dataWaypoints.AllowNew = false;
			// 
			// _dataWaypointsRaw
			// 
			this._dataWaypointsRaw.AllowDelete = false;
			this._dataWaypointsRaw.AllowNew = false;
			// 
			// menuSaveAsXwing
			// 
			this.menuSaveAsXwing.Index = 4;
			this.menuSaveAsXwing.Text = "X-&wing mission";
			this.menuSaveAsXwing.Click += new System.EventHandler(this.menuSaveAsXwing_Click);
			// 
			// XwingForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(794, 575);
			this.Controls.Add(this.toolXwing);
			this.Controls.Add(this.tabMain);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Menu = this.menuXwing;
			this.MinimumSize = new System.Drawing.Size(800, 600);
			this.Name = "XwingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ye Olde Galactic Empire Mission Editor - X-wing";
			this.Activated += new System.EventHandler(this.frmXwing_Activated);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmXwing_Closing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.XwingForm_KeyDown);
			this.tabMain.ResumeLayout(false);
			this.tabFG.ResumeLayout(false);
			this.tabFGMinor.ResumeLayout(false);
			this.tabCraft.ResumeLayout(false);
			this.grpCraft5.ResumeLayout(false);
			this.grpCraft5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numObjectValue)).EndInit();
			this.grpPlatformBitfield.ResumeLayout(false);
			this.grpPlatformBitfield.PerformLayout();
			this.grpCraft4.ResumeLayout(false);
			this.grpCraft4.PerformLayout();
			this.grpCraft2.ResumeLayout(false);
			this.grpCraft3.ResumeLayout(false);
			this.grpCraft3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numSeconds)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numWaves)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numCraft)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numSC)).EndInit();
			this.tabArrDep.ResumeLayout(false);
			this.tabArrDep.PerformLayout();
			this.grpArrTrigger.ResumeLayout(false);
			this.grpArrTrigger.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numArrSec)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numArrMin)).EndInit();
			this.groupBox7.ResumeLayout(false);
			this.groupBox9.ResumeLayout(false);
			this.tabGoals.ResumeLayout(false);
			this.tabGoals.PerformLayout();
			this.groupBox14.ResumeLayout(false);
			this.tabWP.ResumeLayout(false);
			this.tabWP.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRoll)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPitch)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numYaw)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWP)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataWP_Raw)).EndInit();
			this.tabOrders.ResumeLayout(false);
			this.tabOrders.PerformLayout();
			this.tabMission.ResumeLayout(false);
			this.grpMission.ResumeLayout(false);
			this.grpMission.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnknown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMissionTime)).EndInit();
			this.groupBox24.ResumeLayout(false);
			this.groupBox24.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._dataWaypoints)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._dataWaypointsRaw)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		Button cmdCopyOrder;
		Button cmdPasteOrder;
		Button cmdCopyAD;
		Button cmdPasteAD;
		NumericUpDown numYaw;
		NumericUpDown numPitch;
		NumericUpDown numRoll;
		Label label76;
		Label label77;
		Label label78;
		ImageList imgToolbar;
		ToolBarButton toolNew;
		ToolBarButton toolOpen;
		ToolBarButton toolSave;
		ToolBarButton toolSaveAs;
		ToolBarButton toolSep1;
		ToolBarButton toolNewItem;
		ToolBarButton toolDeleteItem;
		ToolBarButton toolCopy;
		ToolBarButton toolPaste;
		ToolBarButton toolSep2;
		ToolBarButton toolMap;
		ToolBarButton toolBriefing;
		ToolBarButton toolVerify;
		ToolBarButton toolSep3;
        ToolBarButton toolOptions;
		ToolBarButton toolHelp;
		MenuItem menuHelpInfo;
		MenuItem menuAbout;
		MenuItem menuIDMR;
		MenuItem menuER;
		MenuItem menuVerify;
		MenuItem menuMap;
		MenuItem menuBriefing;
		MenuItem menuOptions;
		MenuItem menuUndo;
		MenuItem menuCut;
		MenuItem menuCopy;
		MenuItem menuPaste;
		MenuItem menuDelete;
		MenuItem menuItem6;
		MenuItem menuItem7;
		DataGrid dataWP;
		DataGrid dataWP_Raw;
		Label label56;
		CheckBox chkWPHyp;
		CheckBox chkWP2;
		CheckBox chkWP1;
		CheckBox chkSP3;
		CheckBox chkSP2;
		CheckBox chkSP1;
        CheckBox chkWP3;
		GroupBox groupBox24;
		TextBox txtPrimComp1;
		TextBox txtPrimComp2;
		ToolBar toolXwing;
		MainMenu menuXwing;
		MenuItem menuFile;
		MenuItem menuEdit;
		MenuItem menuTools;
		MenuItem menuHelp;
		MenuItem menuNew;
		MenuItem menuOpen;
		MenuItem menuNewTIE;
		MenuItem menuNewXvT;
		MenuItem menuNewBoP;
		MenuItem menuNewXWA;
		MenuItem menuSave;
		MenuItem menuSaveAs;
		MenuItem menuSaveAsTIE;
		MenuItem menuSaveAsBoP;
		MenuItem menuSaveAsXvT;
		OpenFileDialog opnXW;
		SaveFileDialog savXW;
		MenuItem menuSaveAsXWA;
		TabPage tabFG;
		MenuItem menuExit;
		TabPage tabMission;
		ListBox lstFG;
		Label label1;
		TabPage tabCraft;
		TabPage tabArrDep;
		TabPage tabOrders;
		TabPage tabGoals;
        TabPage tabWP;
		Label label2;
		Label label4;
		Label label5;
		Label lblNotUsed;
		TextBox txtName;
		TextBox txtCargo;
		TextBox txtSpecCargo;
		Label label6;
		NumericUpDown numSC;
		GroupBox groupBox1;
		GroupBox grpCraft3;
		Label label7;
		Label label8;
		Label label9;
		Label label10;
		Label lblCraft;
		Label label13;
		NumericUpDown numWaves;
		NumericUpDown numCraft;
		GroupBox grpCraft2;
		Label label14;
		Label label15;
		ComboBox cboCraft;
		ComboBox cboIFF;
		ComboBox cboAI;
		ComboBox cboMarkings;
		ComboBox cboFormation;
		GroupBox grpCraft4;
		Label lblStatus;
		ComboBox cboStatus;
		Label lblFG;
		Label lblStarting;
		Button cmdForms;
		GroupBox groupBox7;
		RadioButton optArrHyp;
		RadioButton optArrMS;
		ComboBox cboMothership;
		GroupBox groupBox9;
		RadioButton optDepHyp;
		RadioButton optDepMS;
		Label label23;
		Label label24;
		Label label25;
		TabControl tabMain;
		TabControl tabFGMinor;
		GroupBox groupBox14;
		ComboBox cboPrimGoalT;
		Label label32;
		Label lblODesc;
		NumericUpDown numArrMin;
		NumericUpDown numArrSec;
        Label label80;
		private MenuItem menuTest;
		private MenuItem menuRecent;
		private MenuItem menuRec1;
		private MenuItem menuRec2;
		private MenuItem menuRec3;
		private MenuItem menuRec4;
		private MenuItem menuRec5;
        private MenuItem menuGoalSummary;
		private MenuItem menuNewXwing;
		private NumericUpDown numMissionTime;
		private Label label19;
		private Label label82;
		private NumericUpDown numUnknown1;
		private Label label83;
		private ComboBox cboEndEvent;
		private Label label84;
		private ComboBox cboMissionLocation;
		private TextBox txtPrimComp3;
		private ComboBox cboObject;
		private Label label85;
		private Label label18;
		private Label label86;
		private GroupBox grpArrTrigger;
		private ComboBox cboArrCondition;
		private ComboBox cboArrFG;
		private Label label27;
        private Label label26;
		private ComboBox cboOrderSecondary;
		private ComboBox cboOrderPrimary;
		private Label label35;
		private Label label34;
		private Label label33;
		private Label label28;
		private ComboBox cboOrder;
		private ComboBox cboPlayer;
        private Button cmdCopyWPSP;
        private Button cmdMoveDown;
        private Button cmdMoveUp;
        private Label lblObjectValue;
        private CheckBox chkPlatformGuns;
        private NumericUpDown numObjectValue;
        private GroupBox grpPlatformBitfield;
        private CheckBox chkGun1;
        private CheckBox chkGun3;
        private CheckBox chkGun2;
        private CheckBox chkGun4;
        private CheckBox chkGun6;
        private CheckBox chkGun5;
        private Label lblMissionTimeNote;
        private GroupBox grpCraft5;
        private Label label3;
        private NumericUpDown numSeconds;
        private Label lblSeconds;
        private Label lblPlatformWarning;
        private Label lblCS3;
        private Label lblCS2;
        private Label lblCS1;
        private Label lblCSInfo;
        private Button cmdSwitchFG;
        private Label lblBRFNotice;
        private Button cmdImportXWI;
        private ComboBox cboOrderValue;
        private Label lblArrDepNote;
        private Label lblGoalNote;
        private Label lblOrderNote;
        private GroupBox grpMission;
		private MenuItem menuSaveAsXwing;
	}
}