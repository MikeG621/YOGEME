using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class BriefingFormXwing
	{
		System.ComponentModel.IContainer components;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BriefingFormXwing));
			this.tmrBrief = new System.Windows.Forms.Timer(this.components);
			this.imgCraft = new System.Windows.Forms.ImageList(this.components);
			this.hsbTimer = new System.Windows.Forms.HScrollBar();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblCaption = new System.Windows.Forms.Label();
			this.pctBrief = new System.Windows.Forms.PictureBox();
			this.tabBrief = new System.Windows.Forms.TabControl();
			this.tabDisplay = new System.Windows.Forms.TabPage();
			this.cmdNextCaption = new System.Windows.Forms.Button();
			this.cboSelectPage1 = new System.Windows.Forms.ComboBox();
			this.label38 = new System.Windows.Forms.Label();
			this.lblPopupInfo = new System.Windows.Forms.Label();
			this.pnlBottomLeft = new System.Windows.Forms.Panel();
			this.cmdStart = new System.Windows.Forms.Button();
			this.cmdPause = new System.Windows.Forms.Button();
			this.cmdNext = new System.Windows.Forms.Button();
			this.cmdFF = new System.Windows.Forms.Button();
			this.cmdPlay = new System.Windows.Forms.Button();
			this.cmdStop = new System.Windows.Forms.Button();
			this.lblTime = new System.Windows.Forms.Label();
			this.pnlBottomRight = new System.Windows.Forms.Panel();
			this.cmdOk = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.txtLength = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.pnlShipTag = new System.Windows.Forms.Panel();
			this.label20 = new System.Windows.Forms.Label();
			this.cboFGTag = new System.Windows.Forms.ComboBox();
			this.numFG = new System.Windows.Forms.NumericUpDown();
			this.pnlTextTag = new System.Windows.Forms.Panel();
			this.label21 = new System.Windows.Forms.Label();
			this.numText = new System.Windows.Forms.NumericUpDown();
			this.cboTextTag = new System.Windows.Forms.ComboBox();
			this.lblInstruction = new System.Windows.Forms.Label();
			this.hsbBRF = new System.Windows.Forms.HScrollBar();
			this.vsbBRF = new System.Windows.Forms.VScrollBar();
			this.optFG = new System.Windows.Forms.RadioButton();
			this.cboText = new System.Windows.Forms.ComboBox();
			this.cmdTitle = new System.Windows.Forms.Button();
			this.cmdCaption = new System.Windows.Forms.Button();
			this.cmdClear = new System.Windows.Forms.Button();
			this.optText = new System.Windows.Forms.RadioButton();
			this.cmdFG = new System.Windows.Forms.Button();
			this.cmdText = new System.Windows.Forms.Button();
			this.cmdClearText = new System.Windows.Forms.Button();
			this.cmdMove = new System.Windows.Forms.Button();
			this.cmdZoom = new System.Windows.Forms.Button();
			this.tabStrings = new System.Windows.Forms.TabPage();
			this.label10 = new System.Windows.Forms.Label();
			this.lstString = new System.Windows.Forms.ListBox();
			this.txtStringEdit = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.dataT = new System.Windows.Forms.DataGrid();
			this.label2 = new System.Windows.Forms.Label();
			this.tabEvents = new System.Windows.Forms.TabPage();
			this.chkShift = new System.Windows.Forms.CheckBox();
			this.cboSelectPage2 = new System.Windows.Forms.ComboBox();
			this.label39 = new System.Windows.Forms.Label();
			this.cmdNew = new System.Windows.Forms.Button();
			this.cmdUp = new System.Windows.Forms.Button();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cboString = new System.Windows.Forms.ComboBox();
			this.cboTag = new System.Windows.Forms.ComboBox();
			this.cboFG = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.numX = new System.Windows.Forms.NumericUpDown();
			this.numY = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.numTime = new System.Windows.Forms.NumericUpDown();
			this.cboEvent = new System.Windows.Forms.ComboBox();
			this.lstEvents = new System.Windows.Forms.ListBox();
			this.lblEventTime = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cmdDown = new System.Windows.Forms.Button();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.tabPages = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.cboMissionLocation = new System.Windows.Forms.ComboBox();
			this.label41 = new System.Windows.Forms.Label();
			this.cboMaxCoordSet = new System.Windows.Forms.ComboBox();
			this.label40 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cboPageAddCaption = new System.Windows.Forms.ComboBox();
			this.cboPageAddTitle = new System.Windows.Forms.ComboBox();
			this.lblPageAddCaption = new System.Windows.Forms.Label();
			this.lblPageAddTitle = new System.Windows.Forms.Label();
			this.cmdPageAdd = new System.Windows.Forms.Button();
			this.cboPageAddType = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cboPageType = new System.Windows.Forms.ComboBox();
			this.numPageCoordSet = new System.Windows.Forms.NumericUpDown();
			this.label29 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.grpUI = new System.Windows.Forms.GroupBox();
			this.cmdPageTypeText = new System.Windows.Forms.Button();
			this.cmdPageTypeMap = new System.Windows.Forms.Button();
			this.chkUIvisible = new System.Windows.Forms.CheckBox();
			this.cmdPageTypeDelete = new System.Windows.Forms.Button();
			this.cmdPageTypeAdd = new System.Windows.Forms.Button();
			this.label37 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.lstPageType = new System.Windows.Forms.ListBox();
			this.lstViewport = new System.Windows.Forms.ListBox();
			this.label35 = new System.Windows.Forms.Label();
			this.cmdUIDefault = new System.Windows.Forms.Button();
			this.label33 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.numUIright = new System.Windows.Forms.NumericUpDown();
			this.numUIbottom = new System.Windows.Forms.NumericUpDown();
			this.numUIleft = new System.Windows.Forms.NumericUpDown();
			this.numUItop = new System.Windows.Forms.NumericUpDown();
			this.cmdPageMoveDown = new System.Windows.Forms.Button();
			this.cmdPageMoveUp = new System.Windows.Forms.Button();
			this.cmdPageDelete = new System.Windows.Forms.Button();
			this.cmdPageSelect = new System.Windows.Forms.Button();
			this.lstPages = new System.Windows.Forms.ListBox();
			this.dataTags = new System.Data.DataView();
			this.dataStrings = new System.Data.DataView();
			this.tmrPopup = new System.Windows.Forms.Timer(this.components);
			this.tmrMapRedraw = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pctBrief)).BeginInit();
			this.tabBrief.SuspendLayout();
			this.tabDisplay.SuspendLayout();
			this.pnlBottomLeft.SuspendLayout();
			this.pnlBottomRight.SuspendLayout();
			this.pnlShipTag.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFG)).BeginInit();
			this.pnlTextTag.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numText)).BeginInit();
			this.tabStrings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataT)).BeginInit();
			this.tabEvents.SuspendLayout();
			this.grpParameters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numTime)).BeginInit();
			this.tabPages.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPageCoordSet)).BeginInit();
			this.grpUI.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUIright)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUIbottom)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUIleft)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUItop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTags)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataStrings)).BeginInit();
			this.SuspendLayout();
			// 
			// tmrBrief
			// 
			this.tmrBrief.Tick += new System.EventHandler(this.tmrBrief_Tick);
			// 
			// imgCraft
			// 
			this.imgCraft.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgCraft.ImageSize = new System.Drawing.Size(34, 34);
			this.imgCraft.TransparentColor = System.Drawing.Color.Black;
			// 
			// hsbTimer
			// 
			this.hsbTimer.LargeChange = 12;
			this.hsbTimer.Location = new System.Drawing.Point(101, 16);
			this.hsbTimer.Name = "hsbTimer";
			this.hsbTimer.Size = new System.Drawing.Size(488, 16);
			this.hsbTimer.TabIndex = 0;
			this.hsbTimer.TabStop = true;
			this.hsbTimer.Value = 1;
			this.hsbTimer.ValueChanged += new System.EventHandler(this.hsbTimer_ValueChanged);
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(168)))));
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(84)))));
			this.lblTitle.Location = new System.Drawing.Point(8, 16);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(584, 24);
			this.lblTitle.TabIndex = 1;
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblCaption
			// 
			this.lblCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(168)))));
			this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
			this.lblCaption.Location = new System.Drawing.Point(8, 266);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(584, 50);
			this.lblCaption.TabIndex = 2;
			// 
			// pctBrief
			// 
			this.pctBrief.BackColor = System.Drawing.Color.Black;
			this.pctBrief.Location = new System.Drawing.Point(8, 40);
			this.pctBrief.Name = "pctBrief";
			this.pctBrief.Size = new System.Drawing.Size(584, 228);
			this.pctBrief.TabIndex = 3;
			this.pctBrief.TabStop = false;
			this.pctBrief.Paint += new System.Windows.Forms.PaintEventHandler(this.pctBrief_Paint);
			this.pctBrief.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctBrief_MouseDown);
			this.pctBrief.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctBrief_MouseMove);
			this.pctBrief.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctBrief_MouseUp);
			this.pctBrief.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pctBrief_MouseMove);
			// 
			// tabBrief
			// 
			this.tabBrief.Controls.Add(this.tabDisplay);
			this.tabBrief.Controls.Add(this.tabStrings);
			this.tabBrief.Controls.Add(this.tabEvents);
			this.tabBrief.Controls.Add(this.tabPages);
			this.tabBrief.Location = new System.Drawing.Point(0, 8);
			this.tabBrief.Name = "tabBrief";
			this.tabBrief.SelectedIndex = 0;
			this.tabBrief.Size = new System.Drawing.Size(903, 384);
			this.tabBrief.TabIndex = 4;
			this.tabBrief.SelectedIndexChanged += new System.EventHandler(this.tabBrief_SelectedIndexChanged);
			// 
			// tabDisplay
			// 
			this.tabDisplay.BackColor = System.Drawing.SystemColors.Control;
			this.tabDisplay.Controls.Add(this.cmdNextCaption);
			this.tabDisplay.Controls.Add(this.cboSelectPage1);
			this.tabDisplay.Controls.Add(this.label38);
			this.tabDisplay.Controls.Add(this.lblPopupInfo);
			this.tabDisplay.Controls.Add(this.pnlBottomLeft);
			this.tabDisplay.Controls.Add(this.pnlBottomRight);
			this.tabDisplay.Controls.Add(this.pnlShipTag);
			this.tabDisplay.Controls.Add(this.pnlTextTag);
			this.tabDisplay.Controls.Add(this.lblInstruction);
			this.tabDisplay.Controls.Add(this.hsbBRF);
			this.tabDisplay.Controls.Add(this.vsbBRF);
			this.tabDisplay.Controls.Add(this.optFG);
			this.tabDisplay.Controls.Add(this.cboText);
			this.tabDisplay.Controls.Add(this.cmdTitle);
			this.tabDisplay.Controls.Add(this.lblCaption);
			this.tabDisplay.Controls.Add(this.lblTitle);
			this.tabDisplay.Controls.Add(this.pctBrief);
			this.tabDisplay.Controls.Add(this.cmdCaption);
			this.tabDisplay.Controls.Add(this.cmdClear);
			this.tabDisplay.Controls.Add(this.optText);
			this.tabDisplay.Controls.Add(this.cmdFG);
			this.tabDisplay.Controls.Add(this.cmdText);
			this.tabDisplay.Controls.Add(this.cmdClearText);
			this.tabDisplay.Controls.Add(this.cmdMove);
			this.tabDisplay.Controls.Add(this.cmdZoom);
			this.tabDisplay.Location = new System.Drawing.Point(4, 22);
			this.tabDisplay.Name = "tabDisplay";
			this.tabDisplay.Size = new System.Drawing.Size(895, 358);
			this.tabDisplay.TabIndex = 0;
			this.tabDisplay.Text = "Briefing";
			// 
			// cmdNextCaption
			// 
			this.cmdNextCaption.Location = new System.Drawing.Point(11, 87);
			this.cmdNextCaption.Name = "cmdNextCaption";
			this.cmdNextCaption.Size = new System.Drawing.Size(59, 40);
			this.cmdNextCaption.TabIndex = 37;
			this.cmdNextCaption.Text = "Next Caption";
			this.cmdNextCaption.UseVisualStyleBackColor = true;
			this.cmdNextCaption.Click += new System.EventHandler(this.cmdNextCaption_Click);
			// 
			// cboSelectPage1
			// 
			this.cboSelectPage1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSelectPage1.FormattingEnabled = true;
			this.cboSelectPage1.Location = new System.Drawing.Point(8, 60);
			this.cboSelectPage1.Name = "cboSelectPage1";
			this.cboSelectPage1.Size = new System.Drawing.Size(75, 21);
			this.cboSelectPage1.TabIndex = 36;
			this.cboSelectPage1.SelectedIndexChanged += new System.EventHandler(this.cboSelectPage1_SelectedIndexChanged);
			// 
			// label38
			// 
			this.label38.AutoSize = true;
			this.label38.Location = new System.Drawing.Point(5, 44);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(68, 13);
			this.label38.TabIndex = 35;
			this.label38.Text = "Select Page:";
			// 
			// lblPopupInfo
			// 
			this.lblPopupInfo.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.lblPopupInfo.Location = new System.Drawing.Point(5, 251);
			this.lblPopupInfo.Name = "lblPopupInfo";
			this.lblPopupInfo.Size = new System.Drawing.Size(192, 65);
			this.lblPopupInfo.TabIndex = 34;
			this.lblPopupInfo.Text = "lblPopupInfo";
			this.lblPopupInfo.Visible = false;
			// 
			// pnlBottomLeft
			// 
			this.pnlBottomLeft.Controls.Add(this.cmdStart);
			this.pnlBottomLeft.Controls.Add(this.cmdPause);
			this.pnlBottomLeft.Controls.Add(this.cmdNext);
			this.pnlBottomLeft.Controls.Add(this.cmdFF);
			this.pnlBottomLeft.Controls.Add(this.hsbTimer);
			this.pnlBottomLeft.Controls.Add(this.cmdPlay);
			this.pnlBottomLeft.Controls.Add(this.cmdStop);
			this.pnlBottomLeft.Controls.Add(this.lblTime);
			this.pnlBottomLeft.Location = new System.Drawing.Point(3, 319);
			this.pnlBottomLeft.Name = "pnlBottomLeft";
			this.pnlBottomLeft.Size = new System.Drawing.Size(602, 36);
			this.pnlBottomLeft.TabIndex = 26;
			// 
			// cmdStart
			// 
			this.cmdStart.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cmdStart.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart.Image")));
			this.cmdStart.Location = new System.Drawing.Point(5, 16);
			this.cmdStart.Name = "cmdStart";
			this.cmdStart.Size = new System.Drawing.Size(16, 16);
			this.cmdStart.TabIndex = 4;
			this.cmdStart.UseVisualStyleBackColor = false;
			this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
			// 
			// cmdPause
			// 
			this.cmdPause.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cmdPause.Enabled = false;
			this.cmdPause.Image = ((System.Drawing.Image)(resources.GetObject("cmdPause.Image")));
			this.cmdPause.Location = new System.Drawing.Point(21, 16);
			this.cmdPause.Name = "cmdPause";
			this.cmdPause.Size = new System.Drawing.Size(16, 16);
			this.cmdPause.TabIndex = 7;
			this.cmdPause.UseVisualStyleBackColor = false;
			this.cmdPause.Visible = false;
			this.cmdPause.Click += new System.EventHandler(this.cmdPause_Click);
			// 
			// cmdNext
			// 
			this.cmdNext.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cmdNext.Image = ((System.Drawing.Image)(resources.GetObject("cmdNext.Image")));
			this.cmdNext.Location = new System.Drawing.Point(69, 16);
			this.cmdNext.Name = "cmdNext";
			this.cmdNext.Size = new System.Drawing.Size(16, 16);
			this.cmdNext.TabIndex = 10;
			this.cmdNext.UseVisualStyleBackColor = false;
			this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
			// 
			// cmdFF
			// 
			this.cmdFF.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cmdFF.Image = ((System.Drawing.Image)(resources.GetObject("cmdFF.Image")));
			this.cmdFF.Location = new System.Drawing.Point(53, 16);
			this.cmdFF.Name = "cmdFF";
			this.cmdFF.Size = new System.Drawing.Size(16, 16);
			this.cmdFF.TabIndex = 9;
			this.cmdFF.UseVisualStyleBackColor = false;
			this.cmdFF.Click += new System.EventHandler(this.cmdFF_Click);
			// 
			// cmdPlay
			// 
			this.cmdPlay.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cmdPlay.Image = ((System.Drawing.Image)(resources.GetObject("cmdPlay.Image")));
			this.cmdPlay.Location = new System.Drawing.Point(21, 16);
			this.cmdPlay.Name = "cmdPlay";
			this.cmdPlay.Size = new System.Drawing.Size(16, 16);
			this.cmdPlay.TabIndex = 6;
			this.cmdPlay.UseVisualStyleBackColor = false;
			this.cmdPlay.Click += new System.EventHandler(this.cmdPlay_Click);
			// 
			// cmdStop
			// 
			this.cmdStop.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cmdStop.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop.Image")));
			this.cmdStop.Location = new System.Drawing.Point(37, 16);
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new System.Drawing.Size(16, 16);
			this.cmdStop.TabIndex = 8;
			this.cmdStop.UseVisualStyleBackColor = false;
			this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
			// 
			// lblTime
			// 
			this.lblTime.Location = new System.Drawing.Point(101, 0);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(112, 16);
			this.lblTime.TabIndex = 8;
			this.lblTime.Text = "Time: 0.00";
			// 
			// pnlBottomRight
			// 
			this.pnlBottomRight.Controls.Add(this.cmdOk);
			this.pnlBottomRight.Controls.Add(this.cmdCancel);
			this.pnlBottomRight.Controls.Add(this.txtLength);
			this.pnlBottomRight.Controls.Add(this.label11);
			this.pnlBottomRight.Location = new System.Drawing.Point(608, 302);
			this.pnlBottomRight.Name = "pnlBottomRight";
			this.pnlBottomRight.Size = new System.Drawing.Size(138, 58);
			this.pnlBottomRight.TabIndex = 25;
			// 
			// cmdOk
			// 
			this.cmdOk.Enabled = false;
			this.cmdOk.Location = new System.Drawing.Point(3, 3);
			this.cmdOk.Name = "cmdOk";
			this.cmdOk.Size = new System.Drawing.Size(56, 23);
			this.cmdOk.TabIndex = 13;
			this.cmdOk.Text = "OK";
			this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Enabled = false;
			this.cmdCancel.Location = new System.Drawing.Point(77, 3);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(56, 23);
			this.cmdCancel.TabIndex = 13;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// txtLength
			// 
			this.txtLength.Location = new System.Drawing.Point(83, 32);
			this.txtLength.MaxLength = 8;
			this.txtLength.Name = "txtLength";
			this.txtLength.Size = new System.Drawing.Size(48, 20);
			this.txtLength.TabIndex = 11;
			this.txtLength.Text = "45";
			this.txtLength.TextChanged += new System.EventHandler(this.txtLength_TextChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(-13, 33);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(91, 16);
			this.label11.TabIndex = 12;
			this.label11.Text = "Run time (sec):";
			this.label11.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// pnlShipTag
			// 
			this.pnlShipTag.Controls.Add(this.label20);
			this.pnlShipTag.Controls.Add(this.cboFGTag);
			this.pnlShipTag.Controls.Add(this.numFG);
			this.pnlShipTag.Location = new System.Drawing.Point(753, 7);
			this.pnlShipTag.Name = "pnlShipTag";
			this.pnlShipTag.Size = new System.Drawing.Size(136, 54);
			this.pnlShipTag.TabIndex = 22;
			this.pnlShipTag.Visible = false;
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(3, 5);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(53, 13);
			this.label20.TabIndex = 18;
			this.label20.Text = "FG Tag #";
			// 
			// cboFGTag
			// 
			this.cboFGTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFGTag.Location = new System.Drawing.Point(3, 29);
			this.cboFGTag.Name = "cboFGTag";
			this.cboFGTag.Size = new System.Drawing.Size(128, 21);
			this.cboFGTag.TabIndex = 16;
			// 
			// numFG
			// 
			this.numFG.Location = new System.Drawing.Point(83, 3);
			this.numFG.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.numFG.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numFG.Name = "numFG";
			this.numFG.Size = new System.Drawing.Size(48, 20);
			this.numFG.TabIndex = 17;
			this.numFG.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// pnlTextTag
			// 
			this.pnlTextTag.Controls.Add(this.label21);
			this.pnlTextTag.Controls.Add(this.numText);
			this.pnlTextTag.Controls.Add(this.cboTextTag);
			this.pnlTextTag.Location = new System.Drawing.Point(754, 64);
			this.pnlTextTag.Name = "pnlTextTag";
			this.pnlTextTag.Size = new System.Drawing.Size(135, 82);
			this.pnlTextTag.TabIndex = 21;
			this.pnlTextTag.Visible = false;
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(3, 5);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(60, 13);
			this.label21.TabIndex = 18;
			this.label21.Text = "Text Tag #";
			// 
			// numText
			// 
			this.numText.Location = new System.Drawing.Point(83, 3);
			this.numText.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.numText.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numText.Name = "numText";
			this.numText.Size = new System.Drawing.Size(48, 20);
			this.numText.TabIndex = 17;
			this.numText.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numText.ValueChanged += new System.EventHandler(this.numText_ValueChanged);
			// 
			// cboTextTag
			// 
			this.cboTextTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTextTag.Location = new System.Drawing.Point(3, 29);
			this.cboTextTag.Name = "cboTextTag";
			this.cboTextTag.Size = new System.Drawing.Size(128, 21);
			this.cboTextTag.TabIndex = 16;
			this.cboTextTag.SelectedIndexChanged += new System.EventHandler(this.cboTextTag_SelectedIndexChanged);
			// 
			// lblInstruction
			// 
			this.lblInstruction.Location = new System.Drawing.Point(8, 280);
			this.lblInstruction.Name = "lblInstruction";
			this.lblInstruction.Size = new System.Drawing.Size(584, 16);
			this.lblInstruction.TabIndex = 20;
			this.lblInstruction.Text = "Click on the map where you would like to place the item";
			this.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.lblInstruction.Visible = false;
			// 
			// hsbBRF
			// 
			this.hsbBRF.Location = new System.Drawing.Point(8, 264);
			this.hsbBRF.Maximum = 32768;
			this.hsbBRF.Minimum = -32767;
			this.hsbBRF.Name = "hsbBRF";
			this.hsbBRF.Size = new System.Drawing.Size(584, 16);
			this.hsbBRF.TabIndex = 19;
			this.hsbBRF.Visible = false;
			this.hsbBRF.ValueChanged += new System.EventHandler(this.hsbBRF_ValueChanged);
			// 
			// vsbBRF
			// 
			this.vsbBRF.Location = new System.Drawing.Point(592, 40);
			this.vsbBRF.Maximum = 32768;
			this.vsbBRF.Minimum = -32767;
			this.vsbBRF.Name = "vsbBRF";
			this.vsbBRF.Size = new System.Drawing.Size(16, 224);
			this.vsbBRF.TabIndex = 18;
			this.vsbBRF.Visible = false;
			this.vsbBRF.ValueChanged += new System.EventHandler(this.vsbBRF_ValueChanged);
			// 
			// optFG
			// 
			this.optFG.Checked = true;
			this.optFG.Enabled = false;
			this.optFG.Location = new System.Drawing.Point(666, 64);
			this.optFG.Name = "optFG";
			this.optFG.Size = new System.Drawing.Size(72, 17);
			this.optFG.TabIndex = 15;
			this.optFG.TabStop = true;
			this.optFG.Text = "FG Tags";
			// 
			// cboText
			// 
			this.cboText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboText.Enabled = false;
			this.cboText.Location = new System.Drawing.Point(608, 40);
			this.cboText.Name = "cboText";
			this.cboText.Size = new System.Drawing.Size(128, 21);
			this.cboText.TabIndex = 14;
			// 
			// cmdTitle
			// 
			this.cmdTitle.Location = new System.Drawing.Point(608, 16);
			this.cmdTitle.Name = "cmdTitle";
			this.cmdTitle.Size = new System.Drawing.Size(48, 23);
			this.cmdTitle.TabIndex = 13;
			this.cmdTitle.Text = "Title";
			this.cmdTitle.Click += new System.EventHandler(this.cmdTitle_Click);
			// 
			// cmdCaption
			// 
			this.cmdCaption.Location = new System.Drawing.Point(680, 16);
			this.cmdCaption.Name = "cmdCaption";
			this.cmdCaption.Size = new System.Drawing.Size(56, 23);
			this.cmdCaption.TabIndex = 13;
			this.cmdCaption.Text = "Caption";
			this.cmdCaption.Click += new System.EventHandler(this.cmdCaption_Click);
			// 
			// cmdClear
			// 
			this.cmdClear.Location = new System.Drawing.Point(608, 64);
			this.cmdClear.Name = "cmdClear";
			this.cmdClear.Size = new System.Drawing.Size(56, 32);
			this.cmdClear.TabIndex = 13;
			this.cmdClear.Text = "Clear...";
			this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
			// 
			// optText
			// 
			this.optText.Enabled = false;
			this.optText.Location = new System.Drawing.Point(666, 79);
			this.optText.Name = "optText";
			this.optText.Size = new System.Drawing.Size(82, 17);
			this.optText.TabIndex = 15;
			this.optText.Text = "Text Tags";
			// 
			// cmdFG
			// 
			this.cmdFG.Location = new System.Drawing.Point(608, 102);
			this.cmdFG.Name = "cmdFG";
			this.cmdFG.Size = new System.Drawing.Size(64, 23);
			this.cmdFG.TabIndex = 13;
			this.cmdFG.Text = "FG Tag";
			this.cmdFG.Click += new System.EventHandler(this.cmdFG_Click);
			// 
			// cmdText
			// 
			this.cmdText.Location = new System.Drawing.Point(672, 102);
			this.cmdText.Name = "cmdText";
			this.cmdText.Size = new System.Drawing.Size(64, 23);
			this.cmdText.TabIndex = 13;
			this.cmdText.Text = "Text Tag";
			this.cmdText.Click += new System.EventHandler(this.cmdText_Click);
			// 
			// cmdClearText
			// 
			this.cmdClearText.Location = new System.Drawing.Point(608, 160);
			this.cmdClearText.Name = "cmdClearText";
			this.cmdClearText.Size = new System.Drawing.Size(64, 23);
			this.cmdClearText.TabIndex = 13;
			this.cmdClearText.Text = "ClearText";
			this.cmdClearText.Click += new System.EventHandler(this.cmdClearText_Click);
			// 
			// cmdMove
			// 
			this.cmdMove.Location = new System.Drawing.Point(608, 131);
			this.cmdMove.Name = "cmdMove";
			this.cmdMove.Size = new System.Drawing.Size(64, 23);
			this.cmdMove.TabIndex = 13;
			this.cmdMove.Text = "MoveMap";
			this.cmdMove.Click += new System.EventHandler(this.cmdMove_Click);
			// 
			// cmdZoom
			// 
			this.cmdZoom.Location = new System.Drawing.Point(672, 131);
			this.cmdZoom.Name = "cmdZoom";
			this.cmdZoom.Size = new System.Drawing.Size(64, 23);
			this.cmdZoom.TabIndex = 13;
			this.cmdZoom.Text = "ZoomMap";
			this.cmdZoom.Click += new System.EventHandler(this.cmdZoom_Click);
			// 
			// tabStrings
			// 
			this.tabStrings.BackColor = System.Drawing.SystemColors.Control;
			this.tabStrings.Controls.Add(this.label10);
			this.tabStrings.Controls.Add(this.lstString);
			this.tabStrings.Controls.Add(this.txtStringEdit);
			this.tabStrings.Controls.Add(this.label1);
			this.tabStrings.Controls.Add(this.dataT);
			this.tabStrings.Controls.Add(this.label2);
			this.tabStrings.Location = new System.Drawing.Point(4, 22);
			this.tabStrings.Name = "tabStrings";
			this.tabStrings.Size = new System.Drawing.Size(895, 358);
			this.tabStrings.TabIndex = 1;
			this.tabStrings.Text = "Tags and Strings";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(454, 19);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(282, 55);
			this.label10.TabIndex = 4;
			this.label10.Text = resources.GetString("label10.Text");
			// 
			// lstString
			// 
			this.lstString.FormattingEnabled = true;
			this.lstString.Location = new System.Drawing.Point(187, 32);
			this.lstString.Name = "lstString";
			this.lstString.Size = new System.Drawing.Size(253, 316);
			this.lstString.TabIndex = 3;
			this.lstString.SelectedIndexChanged += new System.EventHandler(this.lstString_SelectedIndexChanged);
			// 
			// txtStringEdit
			// 
			this.txtStringEdit.Location = new System.Drawing.Point(457, 77);
			this.txtStringEdit.MaxLength = 1022;
			this.txtStringEdit.Multiline = true;
			this.txtStringEdit.Name = "txtStringEdit";
			this.txtStringEdit.Size = new System.Drawing.Size(279, 267);
			this.txtStringEdit.TabIndex = 2;
			this.txtStringEdit.TextChanged += new System.EventHandler(this.txtStringEdit_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Text Tags: Drawn on map";
			// 
			// dataT
			// 
			this.dataT.CaptionVisible = false;
			this.dataT.ColumnHeadersVisible = false;
			this.dataT.DataMember = "";
			this.dataT.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataT.Location = new System.Drawing.Point(16, 32);
			this.dataT.Name = "dataT";
			this.dataT.PreferredColumnWidth = 125;
			this.dataT.RowHeadersVisible = false;
			this.dataT.Size = new System.Drawing.Size(144, 312);
			this.dataT.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(184, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(256, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Briefing Strings: Used for Title and Caption";
			// 
			// tabEvents
			// 
			this.tabEvents.BackColor = System.Drawing.SystemColors.Control;
			this.tabEvents.Controls.Add(this.chkShift);
			this.tabEvents.Controls.Add(this.cboSelectPage2);
			this.tabEvents.Controls.Add(this.label39);
			this.tabEvents.Controls.Add(this.cmdNew);
			this.tabEvents.Controls.Add(this.cmdUp);
			this.tabEvents.Controls.Add(this.grpParameters);
			this.tabEvents.Controls.Add(this.label3);
			this.tabEvents.Controls.Add(this.numTime);
			this.tabEvents.Controls.Add(this.cboEvent);
			this.tabEvents.Controls.Add(this.lstEvents);
			this.tabEvents.Controls.Add(this.lblEventTime);
			this.tabEvents.Controls.Add(this.label5);
			this.tabEvents.Controls.Add(this.cmdDown);
			this.tabEvents.Controls.Add(this.cmdDelete);
			this.tabEvents.Location = new System.Drawing.Point(4, 22);
			this.tabEvents.Name = "tabEvents";
			this.tabEvents.Size = new System.Drawing.Size(895, 358);
			this.tabEvents.TabIndex = 2;
			this.tabEvents.Text = "Event List";
			// 
			// chkShift
			// 
			this.chkShift.AutoSize = true;
			this.chkShift.Location = new System.Drawing.Point(318, 173);
			this.chkShift.Name = "chkShift";
			this.chkShift.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkShift.Size = new System.Drawing.Size(61, 17);
			this.chkShift.TabIndex = 38;
			this.chkShift.Text = "Shift All";
			this.chkShift.UseVisualStyleBackColor = true;
			// 
			// cboSelectPage2
			// 
			this.cboSelectPage2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSelectPage2.FormattingEnabled = true;
			this.cboSelectPage2.Location = new System.Drawing.Point(472, 30);
			this.cboSelectPage2.Name = "cboSelectPage2";
			this.cboSelectPage2.Size = new System.Drawing.Size(75, 21);
			this.cboSelectPage2.TabIndex = 4;
			this.cboSelectPage2.SelectedIndexChanged += new System.EventHandler(this.cboSelectPage2_SelectedIndexChanged);
			// 
			// label39
			// 
			this.label39.AutoSize = true;
			this.label39.Location = new System.Drawing.Point(469, 14);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(68, 13);
			this.label39.TabIndex = 37;
			this.label39.Text = "Select Page:";
			// 
			// cmdNew
			// 
			this.cmdNew.Location = new System.Drawing.Point(352, 8);
			this.cmdNew.Name = "cmdNew";
			this.cmdNew.Size = new System.Drawing.Size(88, 23);
			this.cmdNew.TabIndex = 2;
			this.cmdNew.Text = "&New Event";
			this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
			// 
			// cmdUp
			// 
			this.cmdUp.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cmdUp.Image = ((System.Drawing.Image)(resources.GetObject("cmdUp.Image")));
			this.cmdUp.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdUp.Location = new System.Drawing.Point(320, 8);
			this.cmdUp.Name = "cmdUp";
			this.cmdUp.Size = new System.Drawing.Size(24, 24);
			this.cmdUp.TabIndex = 0;
			this.cmdUp.UseVisualStyleBackColor = false;
			this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
			// 
			// grpParameters
			// 
			this.grpParameters.Controls.Add(this.label8);
			this.grpParameters.Controls.Add(this.label4);
			this.grpParameters.Controls.Add(this.cboString);
			this.grpParameters.Controls.Add(this.cboTag);
			this.grpParameters.Controls.Add(this.cboFG);
			this.grpParameters.Controls.Add(this.label6);
			this.grpParameters.Controls.Add(this.label7);
			this.grpParameters.Controls.Add(this.label9);
			this.grpParameters.Controls.Add(this.numX);
			this.grpParameters.Controls.Add(this.numY);
			this.grpParameters.Location = new System.Drawing.Point(320, 197);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(408, 149);
			this.grpParameters.TabIndex = 7;
			this.grpParameters.TabStop = false;
			this.grpParameters.Text = "Parameters";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 24);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(16, 16);
			this.label8.TabIndex = 2;
			this.label8.Text = "X:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(112, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "String:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// cboString
			// 
			this.cboString.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboString.Enabled = false;
			this.cboString.Location = new System.Drawing.Point(152, 24);
			this.cboString.Name = "cboString";
			this.cboString.Size = new System.Drawing.Size(248, 21);
			this.cboString.TabIndex = 2;
			this.cboString.SelectedIndexChanged += new System.EventHandler(this.cboString_SelectedIndexChanged);
			// 
			// cboTag
			// 
			this.cboTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTag.Enabled = false;
			this.cboTag.Location = new System.Drawing.Point(152, 56);
			this.cboTag.Name = "cboTag";
			this.cboTag.Size = new System.Drawing.Size(121, 21);
			this.cboTag.TabIndex = 3;
			this.cboTag.SelectedIndexChanged += new System.EventHandler(this.cboTag_SelectedIndexChanged);
			// 
			// cboFG
			// 
			this.cboFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFG.Enabled = false;
			this.cboFG.Location = new System.Drawing.Point(152, 90);
			this.cboFG.Name = "cboFG";
			this.cboFG.Size = new System.Drawing.Size(121, 21);
			this.cboFG.TabIndex = 4;
			this.cboFG.SelectedIndexChanged += new System.EventHandler(this.cboFG_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(112, 56);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 16);
			this.label6.TabIndex = 1;
			this.label6.Text = "Tag:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(80, 90);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Flight Group:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 56);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(16, 16);
			this.label9.TabIndex = 2;
			this.label9.Text = "Y:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// numX
			// 
			this.numX.Enabled = false;
			this.numX.Location = new System.Drawing.Point(32, 24);
			this.numX.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numX.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
			this.numX.Name = "numX";
			this.numX.Size = new System.Drawing.Size(56, 20);
			this.numX.TabIndex = 0;
			this.numX.ValueChanged += new System.EventHandler(this.numX_ValueChanged);
			// 
			// numY
			// 
			this.numY.Enabled = false;
			this.numY.Location = new System.Drawing.Point(32, 56);
			this.numY.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numY.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
			this.numY.Name = "numY";
			this.numY.Size = new System.Drawing.Size(56, 20);
			this.numY.TabIndex = 1;
			this.numY.ValueChanged += new System.EventHandler(this.numY_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(385, 155);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Event Time:";
			// 
			// numTime
			// 
			this.numTime.Location = new System.Drawing.Point(385, 171);
			this.numTime.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.numTime.Name = "numTime";
			this.numTime.Size = new System.Drawing.Size(48, 20);
			this.numTime.TabIndex = 5;
			this.numTime.ValueChanged += new System.EventHandler(this.numTime_ValueChanged);
			// 
			// cboEvent
			// 
			this.cboEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEvent.Location = new System.Drawing.Point(536, 171);
			this.cboEvent.Name = "cboEvent";
			this.cboEvent.Size = new System.Drawing.Size(160, 21);
			this.cboEvent.TabIndex = 6;
			this.cboEvent.SelectedIndexChanged += new System.EventHandler(this.cboEvent_SelectedIndexChanged);
			// 
			// lstEvents
			// 
			this.lstEvents.Location = new System.Drawing.Point(8, 8);
			this.lstEvents.Name = "lstEvents";
			this.lstEvents.Size = new System.Drawing.Size(304, 342);
			this.lstEvents.TabIndex = 0;
			this.lstEvents.SelectedIndexChanged += new System.EventHandler(this.lstEvents_SelectedIndexChanged);
			// 
			// lblEventTime
			// 
			this.lblEventTime.Location = new System.Drawing.Point(433, 171);
			this.lblEventTime.Name = "lblEventTime";
			this.lblEventTime.Size = new System.Drawing.Size(96, 16);
			this.lblEventTime.TabIndex = 3;
			this.lblEventTime.Text = "= 0.00 seconds";
			this.lblEventTime.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(536, 155);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Event:";
			// 
			// cmdDown
			// 
			this.cmdDown.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.cmdDown.Image = ((System.Drawing.Image)(resources.GetObject("cmdDown.Image")));
			this.cmdDown.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdDown.Location = new System.Drawing.Point(320, 40);
			this.cmdDown.Name = "cmdDown";
			this.cmdDown.Size = new System.Drawing.Size(24, 24);
			this.cmdDown.TabIndex = 1;
			this.cmdDown.UseVisualStyleBackColor = false;
			this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Location = new System.Drawing.Point(352, 40);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(88, 23);
			this.cmdDelete.TabIndex = 3;
			this.cmdDelete.Text = "&Delete Event";
			this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			// 
			// tabPages
			// 
			this.tabPages.BackColor = System.Drawing.SystemColors.Control;
			this.tabPages.Controls.Add(this.groupBox3);
			this.tabPages.Controls.Add(this.groupBox2);
			this.tabPages.Controls.Add(this.groupBox1);
			this.tabPages.Controls.Add(this.label34);
			this.tabPages.Controls.Add(this.grpUI);
			this.tabPages.Controls.Add(this.cmdPageMoveDown);
			this.tabPages.Controls.Add(this.cmdPageMoveUp);
			this.tabPages.Controls.Add(this.cmdPageDelete);
			this.tabPages.Controls.Add(this.cmdPageSelect);
			this.tabPages.Controls.Add(this.lstPages);
			this.tabPages.Location = new System.Drawing.Point(4, 22);
			this.tabPages.Name = "tabPages";
			this.tabPages.Size = new System.Drawing.Size(895, 358);
			this.tabPages.TabIndex = 3;
			this.tabPages.Text = "Page/Briefing Settings";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.cboMissionLocation);
			this.groupBox3.Controls.Add(this.label41);
			this.groupBox3.Controls.Add(this.cboMaxCoordSet);
			this.groupBox3.Controls.Add(this.label40);
			this.groupBox3.Location = new System.Drawing.Point(457, 10);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(224, 77);
			this.groupBox3.TabIndex = 23;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Briefing Settings:";
			// 
			// cboMissionLocation
			// 
			this.cboMissionLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMissionLocation.FormattingEnabled = true;
			this.cboMissionLocation.Location = new System.Drawing.Point(101, 43);
			this.cboMissionLocation.Name = "cboMissionLocation";
			this.cboMissionLocation.Size = new System.Drawing.Size(117, 21);
			this.cboMissionLocation.TabIndex = 12;
			this.cboMissionLocation.SelectedIndexChanged += new System.EventHandler(this.cboMissionLocation_SelectedIndexChanged);
			// 
			// label41
			// 
			this.label41.AutoSize = true;
			this.label41.Location = new System.Drawing.Point(6, 46);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(89, 13);
			this.label41.TabIndex = 2;
			this.label41.Text = "Mission Location:";
			// 
			// cboMaxCoordSet
			// 
			this.cboMaxCoordSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMaxCoordSet.FormattingEnabled = true;
			this.cboMaxCoordSet.Location = new System.Drawing.Point(101, 22);
			this.cboMaxCoordSet.Name = "cboMaxCoordSet";
			this.cboMaxCoordSet.Size = new System.Drawing.Size(58, 21);
			this.cboMaxCoordSet.TabIndex = 11;
			this.cboMaxCoordSet.SelectedIndexChanged += new System.EventHandler(this.cboMaxCoordSet_SelectedIndexChanged);
			// 
			// label40
			// 
			this.label40.AutoSize = true;
			this.label40.Location = new System.Drawing.Point(10, 25);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(85, 13);
			this.label40.TabIndex = 0;
			this.label40.Text = "Max Coord Sets:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cboPageAddCaption);
			this.groupBox2.Controls.Add(this.cboPageAddTitle);
			this.groupBox2.Controls.Add(this.lblPageAddCaption);
			this.groupBox2.Controls.Add(this.lblPageAddTitle);
			this.groupBox2.Controls.Add(this.cmdPageAdd);
			this.groupBox2.Controls.Add(this.cboPageAddType);
			this.groupBox2.Location = new System.Drawing.Point(149, 93);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(302, 69);
			this.groupBox2.TabIndex = 22;
			this.groupBox2.TabStop = false;
			// 
			// cboPageAddCaption
			// 
			this.cboPageAddCaption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPageAddCaption.FormattingEnabled = true;
			this.cboPageAddCaption.Location = new System.Drawing.Point(177, 36);
			this.cboPageAddCaption.Name = "cboPageAddCaption";
			this.cboPageAddCaption.Size = new System.Drawing.Size(112, 21);
			this.cboPageAddCaption.TabIndex = 7;
			// 
			// cboPageAddTitle
			// 
			this.cboPageAddTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPageAddTitle.FormattingEnabled = true;
			this.cboPageAddTitle.Location = new System.Drawing.Point(177, 14);
			this.cboPageAddTitle.Name = "cboPageAddTitle";
			this.cboPageAddTitle.Size = new System.Drawing.Size(112, 21);
			this.cboPageAddTitle.TabIndex = 6;
			// 
			// lblPageAddCaption
			// 
			this.lblPageAddCaption.AutoSize = true;
			this.lblPageAddCaption.Location = new System.Drawing.Point(101, 41);
			this.lblPageAddCaption.Name = "lblPageAddCaption";
			this.lblPageAddCaption.Size = new System.Drawing.Size(70, 13);
			this.lblPageAddCaption.TabIndex = 18;
			this.lblPageAddCaption.Text = "Caption Text:";
			// 
			// lblPageAddTitle
			// 
			this.lblPageAddTitle.AutoSize = true;
			this.lblPageAddTitle.Location = new System.Drawing.Point(117, 17);
			this.lblPageAddTitle.Name = "lblPageAddTitle";
			this.lblPageAddTitle.Size = new System.Drawing.Size(54, 13);
			this.lblPageAddTitle.TabIndex = 17;
			this.lblPageAddTitle.Text = "Title Text:";
			// 
			// cmdPageAdd
			// 
			this.cmdPageAdd.Location = new System.Drawing.Point(14, 14);
			this.cmdPageAdd.Name = "cmdPageAdd";
			this.cmdPageAdd.Size = new System.Drawing.Size(80, 23);
			this.cmdPageAdd.TabIndex = 4;
			this.cmdPageAdd.Text = "Add Page";
			this.cmdPageAdd.UseVisualStyleBackColor = true;
			this.cmdPageAdd.Click += new System.EventHandler(this.cmdPageAdd_Click);
			// 
			// cboPageAddType
			// 
			this.cboPageAddType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPageAddType.FormattingEnabled = true;
			this.cboPageAddType.Items.AddRange(new object[] {
            "Text Page",
            "Hints Page"});
			this.cboPageAddType.Location = new System.Drawing.Point(14, 40);
			this.cboPageAddType.Name = "cboPageAddType";
			this.cboPageAddType.Size = new System.Drawing.Size(81, 21);
			this.cboPageAddType.TabIndex = 5;
			this.cboPageAddType.SelectedIndexChanged += new System.EventHandler(this.cboPageAddType_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cboPageType);
			this.groupBox1.Controls.Add(this.numPageCoordSet);
			this.groupBox1.Controls.Add(this.label29);
			this.groupBox1.Controls.Add(this.label28);
			this.groupBox1.Location = new System.Drawing.Point(294, 10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(157, 77);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Page Settings:";
			// 
			// cboPageType
			// 
			this.cboPageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPageType.FormattingEnabled = true;
			this.cboPageType.Location = new System.Drawing.Point(73, 46);
			this.cboPageType.Name = "cboPageType";
			this.cboPageType.Size = new System.Drawing.Size(67, 21);
			this.cboPageType.TabIndex = 10;
			this.cboPageType.SelectedIndexChanged += new System.EventHandler(this.cboPageType_SelectedIndexChanged);
			// 
			// numPageCoordSet
			// 
			this.numPageCoordSet.Location = new System.Drawing.Point(73, 20);
			this.numPageCoordSet.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.numPageCoordSet.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numPageCoordSet.Name = "numPageCoordSet";
			this.numPageCoordSet.Size = new System.Drawing.Size(67, 20);
			this.numPageCoordSet.TabIndex = 9;
			this.numPageCoordSet.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numPageCoordSet.ValueChanged += new System.EventHandler(this.numPageCoordSet_ValueChanged);
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Location = new System.Drawing.Point(10, 49);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(62, 13);
			this.label29.TabIndex = 7;
			this.label29.Text = "Page Type:";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(15, 22);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(57, 13);
			this.label28.TabIndex = 6;
			this.label28.Text = "Coord Set:";
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Location = new System.Drawing.Point(8, 12);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(78, 13);
			this.label34.TabIndex = 20;
			this.label34.Text = "Briefing Pages:";
			// 
			// grpUI
			// 
			this.grpUI.Controls.Add(this.cmdPageTypeText);
			this.grpUI.Controls.Add(this.cmdPageTypeMap);
			this.grpUI.Controls.Add(this.chkUIvisible);
			this.grpUI.Controls.Add(this.cmdPageTypeDelete);
			this.grpUI.Controls.Add(this.cmdPageTypeAdd);
			this.grpUI.Controls.Add(this.label37);
			this.grpUI.Controls.Add(this.label36);
			this.grpUI.Controls.Add(this.lstPageType);
			this.grpUI.Controls.Add(this.lstViewport);
			this.grpUI.Controls.Add(this.label35);
			this.grpUI.Controls.Add(this.cmdUIDefault);
			this.grpUI.Controls.Add(this.label33);
			this.grpUI.Controls.Add(this.label32);
			this.grpUI.Controls.Add(this.label31);
			this.grpUI.Controls.Add(this.label30);
			this.grpUI.Controls.Add(this.numUIright);
			this.grpUI.Controls.Add(this.numUIbottom);
			this.grpUI.Controls.Add(this.numUIleft);
			this.grpUI.Controls.Add(this.numUItop);
			this.grpUI.Location = new System.Drawing.Point(8, 178);
			this.grpUI.Name = "grpUI";
			this.grpUI.Size = new System.Drawing.Size(673, 166);
			this.grpUI.TabIndex = 15;
			this.grpUI.TabStop = false;
			this.grpUI.Text = "Page UI Settings";
			this.grpUI.Leave += new System.EventHandler(this.grpUI_Leave);
			// 
			// cmdPageTypeText
			// 
			this.cmdPageTypeText.Location = new System.Drawing.Point(321, 95);
			this.cmdPageTypeText.Name = "cmdPageTypeText";
			this.cmdPageTypeText.Size = new System.Drawing.Size(86, 23);
			this.cmdPageTypeText.TabIndex = 24;
			this.cmdPageTypeText.Text = "Reset as Text";
			this.cmdPageTypeText.UseVisualStyleBackColor = true;
			this.cmdPageTypeText.Click += new System.EventHandler(this.cmdPageTypeText_Click);
			// 
			// cmdPageTypeMap
			// 
			this.cmdPageTypeMap.Location = new System.Drawing.Point(321, 69);
			this.cmdPageTypeMap.Name = "cmdPageTypeMap";
			this.cmdPageTypeMap.Size = new System.Drawing.Size(86, 23);
			this.cmdPageTypeMap.TabIndex = 23;
			this.cmdPageTypeMap.Text = "Reset as Map";
			this.cmdPageTypeMap.UseVisualStyleBackColor = true;
			this.cmdPageTypeMap.Click += new System.EventHandler(this.cmdPageTypeMap_Click);
			// 
			// chkUIvisible
			// 
			this.chkUIvisible.AutoSize = true;
			this.chkUIvisible.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkUIvisible.Location = new System.Drawing.Point(223, 121);
			this.chkUIvisible.Name = "chkUIvisible";
			this.chkUIvisible.Size = new System.Drawing.Size(59, 17);
			this.chkUIvisible.TabIndex = 22;
			this.chkUIvisible.Text = "Visible:";
			this.chkUIvisible.UseVisualStyleBackColor = true;
			// 
			// cmdPageTypeDelete
			// 
			this.cmdPageTypeDelete.Location = new System.Drawing.Point(61, 136);
			this.cmdPageTypeDelete.Name = "cmdPageTypeDelete";
			this.cmdPageTypeDelete.Size = new System.Drawing.Size(46, 23);
			this.cmdPageTypeDelete.TabIndex = 16;
			this.cmdPageTypeDelete.Text = "Delete";
			this.cmdPageTypeDelete.UseVisualStyleBackColor = true;
			this.cmdPageTypeDelete.Click += new System.EventHandler(this.cmdPageTypeDelete_Click);
			// 
			// cmdPageTypeAdd
			// 
			this.cmdPageTypeAdd.Location = new System.Drawing.Point(9, 136);
			this.cmdPageTypeAdd.Name = "cmdPageTypeAdd";
			this.cmdPageTypeAdd.Size = new System.Drawing.Size(46, 23);
			this.cmdPageTypeAdd.TabIndex = 15;
			this.cmdPageTypeAdd.Text = "Add";
			this.cmdPageTypeAdd.UseVisualStyleBackColor = true;
			this.cmdPageTypeAdd.Click += new System.EventHandler(this.cmdPageTypeAdd_Click);
			// 
			// label37
			// 
			this.label37.AutoSize = true;
			this.label37.Location = new System.Drawing.Point(101, 19);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(93, 13);
			this.label37.TabIndex = 16;
			this.label37.Text = "Display Viewports:";
			// 
			// label36
			// 
			this.label36.AutoSize = true;
			this.label36.Location = new System.Drawing.Point(6, 19);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(67, 13);
			this.label36.TabIndex = 15;
			this.label36.Text = "Page Types:";
			// 
			// lstPageType
			// 
			this.lstPageType.FormattingEnabled = true;
			this.lstPageType.Location = new System.Drawing.Point(6, 35);
			this.lstPageType.Name = "lstPageType";
			this.lstPageType.Size = new System.Drawing.Size(92, 95);
			this.lstPageType.TabIndex = 14;
			this.lstPageType.SelectedIndexChanged += new System.EventHandler(this.lstPageType_SelectedIndexChanged);
			// 
			// lstViewport
			// 
			this.lstViewport.FormattingEnabled = true;
			this.lstViewport.Location = new System.Drawing.Point(104, 35);
			this.lstViewport.Name = "lstViewport";
			this.lstViewport.Size = new System.Drawing.Size(92, 95);
			this.lstViewport.TabIndex = 17;
			this.lstViewport.SelectedIndexChanged += new System.EventHandler(this.lstViewport_SelectedIndexChanged);
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(463, 16);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(204, 114);
			this.label35.TabIndex = 12;
			this.label35.Text = resources.GetString("label35.Text");
			// 
			// cmdUIDefault
			// 
			this.cmdUIDefault.Location = new System.Drawing.Point(506, 137);
			this.cmdUIDefault.Name = "cmdUIDefault";
			this.cmdUIDefault.Size = new System.Drawing.Size(114, 23);
			this.cmdUIDefault.TabIndex = 25;
			this.cmdUIDefault.Text = "Restore to Default";
			this.cmdUIDefault.UseVisualStyleBackColor = true;
			this.cmdUIDefault.Click += new System.EventHandler(this.cmdUIDefault_Click);
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Location = new System.Drawing.Point(214, 97);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(35, 13);
			this.label33.TabIndex = 12;
			this.label33.Text = "Right:";
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Location = new System.Drawing.Point(206, 45);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(43, 13);
			this.label32.TabIndex = 12;
			this.label32.Text = "Bottom:";
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.Location = new System.Drawing.Point(221, 71);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(28, 13);
			this.label31.TabIndex = 12;
			this.label31.Text = "Left:";
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Location = new System.Drawing.Point(220, 19);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(29, 13);
			this.label30.TabIndex = 12;
			this.label30.Text = "Top:";
			// 
			// numUIright
			// 
			this.numUIright.Location = new System.Drawing.Point(255, 95);
			this.numUIright.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numUIright.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
			this.numUIright.Name = "numUIright";
			this.numUIright.Size = new System.Drawing.Size(60, 20);
			this.numUIright.TabIndex = 21;
			// 
			// numUIbottom
			// 
			this.numUIbottom.Location = new System.Drawing.Point(255, 43);
			this.numUIbottom.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numUIbottom.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
			this.numUIbottom.Name = "numUIbottom";
			this.numUIbottom.Size = new System.Drawing.Size(60, 20);
			this.numUIbottom.TabIndex = 19;
			// 
			// numUIleft
			// 
			this.numUIleft.Location = new System.Drawing.Point(255, 69);
			this.numUIleft.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numUIleft.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
			this.numUIleft.Name = "numUIleft";
			this.numUIleft.Size = new System.Drawing.Size(60, 20);
			this.numUIleft.TabIndex = 20;
			// 
			// numUItop
			// 
			this.numUItop.Location = new System.Drawing.Point(255, 17);
			this.numUItop.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
			this.numUItop.Minimum = new decimal(new int[] {
            32767,
            0,
            0,
            -2147483648});
			this.numUItop.Name = "numUItop";
			this.numUItop.Size = new System.Drawing.Size(60, 20);
			this.numUItop.TabIndex = 18;
			// 
			// cmdPageMoveDown
			// 
			this.cmdPageMoveDown.Location = new System.Drawing.Point(149, 70);
			this.cmdPageMoveDown.Name = "cmdPageMoveDown";
			this.cmdPageMoveDown.Size = new System.Drawing.Size(75, 23);
			this.cmdPageMoveDown.TabIndex = 3;
			this.cmdPageMoveDown.Text = "Move Down";
			this.cmdPageMoveDown.UseVisualStyleBackColor = true;
			this.cmdPageMoveDown.Click += new System.EventHandler(this.cmdPageMoveDown_Click);
			// 
			// cmdPageMoveUp
			// 
			this.cmdPageMoveUp.Location = new System.Drawing.Point(149, 41);
			this.cmdPageMoveUp.Name = "cmdPageMoveUp";
			this.cmdPageMoveUp.Size = new System.Drawing.Size(75, 23);
			this.cmdPageMoveUp.TabIndex = 2;
			this.cmdPageMoveUp.Text = "Move Up";
			this.cmdPageMoveUp.UseVisualStyleBackColor = true;
			this.cmdPageMoveUp.Click += new System.EventHandler(this.cmdPageMoveUp_Click);
			// 
			// cmdPageDelete
			// 
			this.cmdPageDelete.Location = new System.Drawing.Point(457, 100);
			this.cmdPageDelete.Name = "cmdPageDelete";
			this.cmdPageDelete.Size = new System.Drawing.Size(75, 23);
			this.cmdPageDelete.TabIndex = 8;
			this.cmdPageDelete.Text = "Delete Page";
			this.cmdPageDelete.UseVisualStyleBackColor = true;
			this.cmdPageDelete.Click += new System.EventHandler(this.cmdPageDelete_Click);
			// 
			// cmdPageSelect
			// 
			this.cmdPageSelect.Location = new System.Drawing.Point(149, 12);
			this.cmdPageSelect.Name = "cmdPageSelect";
			this.cmdPageSelect.Size = new System.Drawing.Size(108, 23);
			this.cmdPageSelect.TabIndex = 1;
			this.cmdPageSelect.Text = "Set as Editor Page";
			this.cmdPageSelect.UseVisualStyleBackColor = true;
			this.cmdPageSelect.Click += new System.EventHandler(this.cmdPageSelect_Click);
			// 
			// lstPages
			// 
			this.lstPages.FormattingEnabled = true;
			this.lstPages.Location = new System.Drawing.Point(8, 28);
			this.lstPages.Name = "lstPages";
			this.lstPages.Size = new System.Drawing.Size(125, 134);
			this.lstPages.TabIndex = 0;
			this.lstPages.SelectedIndexChanged += new System.EventHandler(this.lstPages_SelectedIndexChanged);
			// 
			// tmrPopup
			// 
			this.tmrPopup.Interval = 500;
			this.tmrPopup.Tick += new System.EventHandler(this.tmrPopup_Tick);
			// 
			// tmrMapRedraw
			// 
			this.tmrMapRedraw.Interval = 17;
			this.tmrMapRedraw.Tick += new System.EventHandler(this.tmrMapRedraw_Tick);
			// 
			// BriefingFormXwing
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(904, 386);
			this.Controls.Add(this.tabBrief);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "BriefingFormXwing";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "YOGEME Briefing Editor - X-wing";
			this.Activated += new System.EventHandler(this.frmBrief_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBrief_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBrief_FormClosed);
			this.Load += new System.EventHandler(this.frmBrief_Load);
			((System.ComponentModel.ISupportInitialize)(this.pctBrief)).EndInit();
			this.tabBrief.ResumeLayout(false);
			this.tabDisplay.ResumeLayout(false);
			this.tabDisplay.PerformLayout();
			this.pnlBottomLeft.ResumeLayout(false);
			this.pnlBottomRight.ResumeLayout(false);
			this.pnlBottomRight.PerformLayout();
			this.pnlShipTag.ResumeLayout(false);
			this.pnlShipTag.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFG)).EndInit();
			this.pnlTextTag.ResumeLayout(false);
			this.pnlTextTag.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numText)).EndInit();
			this.tabStrings.ResumeLayout(false);
			this.tabStrings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataT)).EndInit();
			this.tabEvents.ResumeLayout(false);
			this.tabEvents.PerformLayout();
			this.grpParameters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numTime)).EndInit();
			this.tabPages.ResumeLayout(false);
			this.tabPages.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPageCoordSet)).EndInit();
			this.grpUI.ResumeLayout(false);
			this.grpUI.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUIright)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUIbottom)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUIleft)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUItop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTags)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataStrings)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion	// Windows Form Designer generated code

		Timer tmrBrief;
		ImageList imgCraft;
		HScrollBar hsbTimer;
		Label lblTitle;
		Label lblCaption;
		PictureBox pctBrief;
		TabControl tabBrief;
		TabPage tabDisplay;
		TabPage tabStrings;
		TabPage tabEvents;
		Button cmdStop;
		Button cmdStart;
		Button cmdFF;
		Button cmdPlay;
		Button cmdPause;
		TextBox txtLength;
		Button cmdNext;
		Label label1;
		Label label2;
		System.Data.DataView dataTags;
		System.Data.DataView dataStrings;
        DataGrid dataT;
		ListBox lstEvents;
		ComboBox cboEvent;
		NumericUpDown numTime;
		Label label3;
		Label lblEventTime;
		Label label5;
		ComboBox cboString;
		ComboBox cboTag;
		ComboBox cboFG;
		Label label4;
		Label label6;
		Label label7;
		Label label8;
        Label label9;
		NumericUpDown numX;
		NumericUpDown numY;
		Label label11;
		Button cmdUp;
		Button cmdDown;
        GroupBox grpParameters;
		Button cmdNew;
		Button cmdDelete;
		Button cmdTitle;
		Button cmdCaption;
		ComboBox cboText;
		Button cmdOk;
		Button cmdCancel;
		Button cmdClear;
		RadioButton optFG;
		RadioButton optText;
		Button cmdFG;
		ComboBox cboFGTag;
		NumericUpDown numFG;
		NumericUpDown numText;
		ComboBox cboTextTag;
		Button cmdText;
		Button cmdMove;
		Button cmdZoom;
		VScrollBar vsbBRF;
		HScrollBar hsbBRF;
		Label lblInstruction;
        Label lblTime;
		Panel pnlShipTag;
		Panel pnlTextTag;
		Label label20;
		Label label21;
		Panel pnlBottomRight;
		Panel pnlBottomLeft;
        private Label lblPopupInfo;
		private TabPage tabPages;
        private Button cmdPageAdd;
        private Button cmdPageMoveDown;
        private Button cmdPageMoveUp;
        private Button cmdPageDelete;
        private Button cmdPageSelect;
        private ListBox lstPages;
        private ComboBox cboPageType;
        private NumericUpDown numPageCoordSet;
        private Label label29;
        private Label label28;
        private NumericUpDown numUIleft;
        private NumericUpDown numUItop;
        private Button cmdUIDefault;
        private Label label35;
        private Label label33;
        private Label label32;
        private Label label31;
        private Label label30;
        private NumericUpDown numUIright;
        private NumericUpDown numUIbottom;
        private ListBox lstViewport;
        private GroupBox grpUI;
        private ComboBox cboPageAddCaption;
        private ComboBox cboPageAddTitle;
        private Label lblPageAddCaption;
        private Label lblPageAddTitle;
        private ComboBox cboPageAddType;
        private ListBox lstPageType;
        private CheckBox chkUIvisible;
        private Button cmdPageTypeDelete;
        private Button cmdPageTypeAdd;
        private Label label37;
        private Label label36;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private Label label34;
        private ComboBox cboSelectPage1;
        private Label label38;
        private ComboBox cboSelectPage2;
        private Label label39;
        private Button cmdPageTypeText;
        private Button cmdPageTypeMap;
        private GroupBox groupBox3;
        private ComboBox cboMissionLocation;
        private Label label41;
        private ComboBox cboMaxCoordSet;
        private Label label40;
        private Button cmdNextCaption;
        private Button cmdClearText;
        private ListBox lstString;
        private TextBox txtStringEdit;
        private Label label10;
		private Timer tmrPopup;
		private Timer tmrMapRedraw;
        private CheckBox chkShift;
    }
}