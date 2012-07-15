using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class frmBrief
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBrief));
			this.tmrBrief = new System.Windows.Forms.Timer(this.components);
			this.imgCraft = new System.Windows.Forms.ImageList(this.components);
			this.hsbTimer = new System.Windows.Forms.HScrollBar();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblCaption = new System.Windows.Forms.Label();
			this.pctBrief = new System.Windows.Forms.PictureBox();
			this.tabBrief = new System.Windows.Forms.TabControl();
			this.tabDisplay = new System.Windows.Forms.TabPage();
			this.pnlRegion = new System.Windows.Forms.Panel();
			this.numNewRegion = new System.Windows.Forms.NumericUpDown();
			this.label27 = new System.Windows.Forms.Label();
			this.pnlNew = new System.Windows.Forms.Panel();
			this.label26 = new System.Windows.Forms.Label();
			this.cboIconIff = new System.Windows.Forms.ComboBox();
			this.cboNCraft = new System.Windows.Forms.ComboBox();
			this.cboNewIcon = new System.Windows.Forms.ComboBox();
			this.pnlMove = new System.Windows.Forms.Panel();
			this.label25 = new System.Windows.Forms.Label();
			this.numMoveTime = new System.Windows.Forms.NumericUpDown();
			this.label24 = new System.Windows.Forms.Label();
			this.cboMoveIcon = new System.Windows.Forms.ComboBox();
			this.pnlRotate = new System.Windows.Forms.Panel();
			this.label23 = new System.Windows.Forms.Label();
			this.cboRotateAmount = new System.Windows.Forms.ComboBox();
			this.cboRCraft = new System.Windows.Forms.ComboBox();
			this.cmdRegion = new System.Windows.Forms.Button();
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
			this.pnlShipInfo = new System.Windows.Forms.Panel();
			this.optInfoOff = new System.Windows.Forms.RadioButton();
			this.optInfoOn = new System.Windows.Forms.RadioButton();
			this.cboInfoCraft = new System.Windows.Forms.ComboBox();
			this.label22 = new System.Windows.Forms.Label();
			this.cmdMoveShip = new System.Windows.Forms.Button();
			this.cmdShipInfo = new System.Windows.Forms.Button();
			this.cmdRotate = new System.Windows.Forms.Button();
			this.cmdNewShip = new System.Windows.Forms.Button();
			this.pnlShipTag = new System.Windows.Forms.Panel();
			this.label20 = new System.Windows.Forms.Label();
			this.cboFGTag = new System.Windows.Forms.ComboBox();
			this.numFG = new System.Windows.Forms.NumericUpDown();
			this.pnlTextTag = new System.Windows.Forms.Panel();
			this.label21 = new System.Windows.Forms.Label();
			this.numText = new System.Windows.Forms.NumericUpDown();
			this.cboTextTag = new System.Windows.Forms.ComboBox();
			this.cboColorTag = new System.Windows.Forms.ComboBox();
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
			this.cmdMove = new System.Windows.Forms.Button();
			this.cmdZoom = new System.Windows.Forms.Button();
			this.cmdBreak = new System.Windows.Forms.Button();
			this.tabStrings = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.dataT = new System.Windows.Forms.DataGrid();
			this.dataS = new System.Windows.Forms.DataGrid();
			this.label2 = new System.Windows.Forms.Label();
			this.tabEvents = new System.Windows.Forms.TabPage();
			this.cmdNew = new System.Windows.Forms.Button();
			this.grpUnknown = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.numUnk1 = new System.Windows.Forms.NumericUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.numUnk3 = new System.Windows.Forms.NumericUpDown();
			this.label14 = new System.Windows.Forms.Label();
			this.cmdUp = new System.Windows.Forms.Button();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.cboRotate = new System.Windows.Forms.ComboBox();
			this.cboCraft = new System.Windows.Forms.ComboBox();
			this.numRegion = new System.Windows.Forms.NumericUpDown();
			this.label19 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.optOff = new System.Windows.Forms.RadioButton();
			this.optOn = new System.Windows.Forms.RadioButton();
			this.label8 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cboString = new System.Windows.Forms.ComboBox();
			this.cboTag = new System.Windows.Forms.ComboBox();
			this.cboFG = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cboColor = new System.Windows.Forms.ComboBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.numX = new System.Windows.Forms.NumericUpDown();
			this.numY = new System.Windows.Forms.NumericUpDown();
			this.cboIFF = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.numTime = new System.Windows.Forms.NumericUpDown();
			this.cboEvent = new System.Windows.Forms.ComboBox();
			this.lstEvents = new System.Windows.Forms.ListBox();
			this.lblEventTime = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cmdDown = new System.Windows.Forms.Button();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.dataTags = new System.Data.DataView();
			this.dataStrings = new System.Data.DataView();
			((System.ComponentModel.ISupportInitialize)(this.pctBrief)).BeginInit();
			this.tabBrief.SuspendLayout();
			this.tabDisplay.SuspendLayout();
			this.pnlRegion.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numNewRegion)).BeginInit();
			this.pnlNew.SuspendLayout();
			this.pnlMove.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMoveTime)).BeginInit();
			this.pnlRotate.SuspendLayout();
			this.pnlBottomLeft.SuspendLayout();
			this.pnlBottomRight.SuspendLayout();
			this.pnlShipInfo.SuspendLayout();
			this.pnlShipTag.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFG)).BeginInit();
			this.pnlTextTag.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numText)).BeginInit();
			this.tabStrings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataT)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataS)).BeginInit();
			this.tabEvents.SuspendLayout();
			this.grpUnknown.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnk1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk3)).BeginInit();
			this.grpParameters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRegion)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numTime)).BeginInit();
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
			this.pctBrief.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctBrief_MouseDown);
			this.pctBrief.Paint += new System.Windows.Forms.PaintEventHandler(this.pctBrief_Paint);
			// 
			// tabBrief
			// 
			this.tabBrief.Controls.Add(this.tabDisplay);
			this.tabBrief.Controls.Add(this.tabStrings);
			this.tabBrief.Controls.Add(this.tabEvents);
			this.tabBrief.Location = new System.Drawing.Point(0, 8);
			this.tabBrief.Name = "tabBrief";
			this.tabBrief.SelectedIndex = 0;
			this.tabBrief.Size = new System.Drawing.Size(1061, 384);
			this.tabBrief.TabIndex = 4;
			this.tabBrief.SelectedIndexChanged += new System.EventHandler(this.tabBrief_SelectedIndexChanged);
			// 
			// tabDisplay
			// 
			this.tabDisplay.Controls.Add(this.pnlRegion);
			this.tabDisplay.Controls.Add(this.pnlNew);
			this.tabDisplay.Controls.Add(this.pnlMove);
			this.tabDisplay.Controls.Add(this.pnlRotate);
			this.tabDisplay.Controls.Add(this.cmdRegion);
			this.tabDisplay.Controls.Add(this.pnlBottomLeft);
			this.tabDisplay.Controls.Add(this.pnlBottomRight);
			this.tabDisplay.Controls.Add(this.pnlShipInfo);
			this.tabDisplay.Controls.Add(this.cmdMoveShip);
			this.tabDisplay.Controls.Add(this.cmdShipInfo);
			this.tabDisplay.Controls.Add(this.cmdRotate);
			this.tabDisplay.Controls.Add(this.cmdNewShip);
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
			this.tabDisplay.Controls.Add(this.cmdMove);
			this.tabDisplay.Controls.Add(this.cmdZoom);
			this.tabDisplay.Controls.Add(this.cmdBreak);
			this.tabDisplay.Location = new System.Drawing.Point(4, 22);
			this.tabDisplay.Name = "tabDisplay";
			this.tabDisplay.Size = new System.Drawing.Size(1053, 358);
			this.tabDisplay.TabIndex = 0;
			this.tabDisplay.Text = "Briefing";
			// 
			// pnlRegion
			// 
			this.pnlRegion.Controls.Add(this.numNewRegion);
			this.pnlRegion.Controls.Add(this.label27);
			this.pnlRegion.Location = new System.Drawing.Point(899, 228);
			this.pnlRegion.Name = "pnlRegion";
			this.pnlRegion.Size = new System.Drawing.Size(136, 53);
			this.pnlRegion.TabIndex = 31;
			this.pnlRegion.Visible = false;
			// 
			// numNewRegion
			// 
			this.numNewRegion.Location = new System.Drawing.Point(92, 3);
			this.numNewRegion.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.numNewRegion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numNewRegion.Name = "numNewRegion";
			this.numNewRegion.Size = new System.Drawing.Size(40, 20);
			this.numNewRegion.TabIndex = 1;
			this.numNewRegion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(3, 6);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(51, 13);
			this.label27.TabIndex = 0;
			this.label27.Text = "Region #";
			// 
			// pnlNew
			// 
			this.pnlNew.Controls.Add(this.label26);
			this.pnlNew.Controls.Add(this.cboIconIff);
			this.pnlNew.Controls.Add(this.cboNCraft);
			this.pnlNew.Controls.Add(this.cboNewIcon);
			this.pnlNew.Location = new System.Drawing.Point(899, 101);
			this.pnlNew.Name = "pnlNew";
			this.pnlNew.Size = new System.Drawing.Size(136, 111);
			this.pnlNew.TabIndex = 30;
			this.pnlNew.Visible = false;
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(3, 88);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(53, 13);
			this.label26.TabIndex = 4;
			this.label26.Text = "New Icon";
			this.label26.Visible = false;
			// 
			// cboIconIff
			// 
			this.cboIconIff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboIconIff.FormattingEnabled = true;
			this.cboIconIff.Items.AddRange(new object[] {
            "Rebel",
            "Imperial",
            "Blue",
            "Yellow",
            "Red",
            "Purple"});
			this.cboIconIff.Location = new System.Drawing.Point(3, 56);
			this.cboIconIff.Name = "cboIconIff";
			this.cboIconIff.Size = new System.Drawing.Size(128, 21);
			this.cboIconIff.TabIndex = 1;
			this.cboIconIff.SelectedIndexChanged += new System.EventHandler(this.cboIconIff_SelectedIndexChanged);
			// 
			// cboNCraft
			// 
			this.cboNCraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboNCraft.FormattingEnabled = true;
			this.cboNCraft.Location = new System.Drawing.Point(3, 29);
			this.cboNCraft.Name = "cboNCraft";
			this.cboNCraft.Size = new System.Drawing.Size(128, 21);
			this.cboNCraft.TabIndex = 1;
			this.cboNCraft.SelectedIndexChanged += new System.EventHandler(this.cboNCraft_SelectedIndexChanged);
			// 
			// cboNewIcon
			// 
			this.cboNewIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboNewIcon.FormattingEnabled = true;
			this.cboNewIcon.Location = new System.Drawing.Point(3, 2);
			this.cboNewIcon.Name = "cboNewIcon";
			this.cboNewIcon.Size = new System.Drawing.Size(128, 21);
			this.cboNewIcon.TabIndex = 1;
			this.cboNewIcon.SelectedIndexChanged += new System.EventHandler(this.cboNewIcon_SelectedIndexChanged);
			// 
			// pnlMove
			// 
			this.pnlMove.Controls.Add(this.label25);
			this.pnlMove.Controls.Add(this.numMoveTime);
			this.pnlMove.Controls.Add(this.label24);
			this.pnlMove.Controls.Add(this.cboMoveIcon);
			this.pnlMove.Location = new System.Drawing.Point(895, 7);
			this.pnlMove.Name = "pnlMove";
			this.pnlMove.Size = new System.Drawing.Size(136, 80);
			this.pnlMove.TabIndex = 29;
			this.pnlMove.Visible = false;
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(7, 58);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(58, 13);
			this.label25.TabIndex = 3;
			this.label25.Text = "Move Icon";
			this.label25.Visible = false;
			// 
			// numMoveTime
			// 
			this.numMoveTime.DecimalPlaces = 1;
			this.numMoveTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numMoveTime.Location = new System.Drawing.Point(69, 30);
			this.numMoveTime.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numMoveTime.Name = "numMoveTime";
			this.numMoveTime.Size = new System.Drawing.Size(62, 20);
			this.numMoveTime.TabIndex = 2;
			this.numMoveTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(3, 32);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(47, 13);
			this.label24.TabIndex = 1;
			this.label24.Text = "Time (s):";
			// 
			// cboMoveIcon
			// 
			this.cboMoveIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMoveIcon.FormattingEnabled = true;
			this.cboMoveIcon.Location = new System.Drawing.Point(3, 2);
			this.cboMoveIcon.Name = "cboMoveIcon";
			this.cboMoveIcon.Size = new System.Drawing.Size(128, 21);
			this.cboMoveIcon.TabIndex = 0;
			this.cboMoveIcon.SelectedIndexChanged += new System.EventHandler(this.cboMoveIcon_SelectedIndexChanged);
			// 
			// pnlRotate
			// 
			this.pnlRotate.Controls.Add(this.label23);
			this.pnlRotate.Controls.Add(this.cboRotateAmount);
			this.pnlRotate.Controls.Add(this.cboRCraft);
			this.pnlRotate.Location = new System.Drawing.Point(754, 228);
			this.pnlRotate.Name = "pnlRotate";
			this.pnlRotate.Size = new System.Drawing.Size(135, 74);
			this.pnlRotate.TabIndex = 28;
			this.pnlRotate.Visible = false;
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(3, 55);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(39, 13);
			this.label23.TabIndex = 1;
			this.label23.Text = "Rotate";
			this.label23.Visible = false;
			// 
			// cboRotateAmount
			// 
			this.cboRotateAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRotateAmount.FormattingEnabled = true;
			this.cboRotateAmount.Items.AddRange(new object[] {
            "None",
            "Left 90°",
            "180°",
            "Right 90°",
            "Mirror"});
			this.cboRotateAmount.Location = new System.Drawing.Point(2, 30);
			this.cboRotateAmount.Name = "cboRotateAmount";
			this.cboRotateAmount.Size = new System.Drawing.Size(128, 21);
			this.cboRotateAmount.TabIndex = 0;
			this.cboRotateAmount.SelectedIndexChanged += new System.EventHandler(this.cboRotateAmount_SelectedIndexChanged);
			// 
			// cboRCraft
			// 
			this.cboRCraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRCraft.FormattingEnabled = true;
			this.cboRCraft.Location = new System.Drawing.Point(2, 3);
			this.cboRCraft.Name = "cboRCraft";
			this.cboRCraft.Size = new System.Drawing.Size(128, 21);
			this.cboRCraft.TabIndex = 0;
			this.cboRCraft.SelectedIndexChanged += new System.EventHandler(this.cboRCraft_SelectedIndexChanged);
			// 
			// cmdRegion
			// 
			this.cmdRegion.Location = new System.Drawing.Point(672, 160);
			this.cmdRegion.Name = "cmdRegion";
			this.cmdRegion.Size = new System.Drawing.Size(64, 23);
			this.cmdRegion.TabIndex = 27;
			this.cmdRegion.Text = "Region #";
			this.cmdRegion.UseVisualStyleBackColor = true;
			this.cmdRegion.Visible = false;
			this.cmdRegion.Click += new System.EventHandler(this.cmdRegion_Click);
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
			this.cmdStart.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart.Image")));
			this.cmdStart.Location = new System.Drawing.Point(5, 16);
			this.cmdStart.Name = "cmdStart";
			this.cmdStart.Size = new System.Drawing.Size(16, 16);
			this.cmdStart.TabIndex = 4;
			this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
			// 
			// cmdPause
			// 
			this.cmdPause.Enabled = false;
			this.cmdPause.Image = ((System.Drawing.Image)(resources.GetObject("cmdPause.Image")));
			this.cmdPause.Location = new System.Drawing.Point(21, 16);
			this.cmdPause.Name = "cmdPause";
			this.cmdPause.Size = new System.Drawing.Size(16, 16);
			this.cmdPause.TabIndex = 7;
			this.cmdPause.Visible = false;
			this.cmdPause.Click += new System.EventHandler(this.cmdPause_Click);
			// 
			// cmdNext
			// 
			this.cmdNext.Image = ((System.Drawing.Image)(resources.GetObject("cmdNext.Image")));
			this.cmdNext.Location = new System.Drawing.Point(69, 16);
			this.cmdNext.Name = "cmdNext";
			this.cmdNext.Size = new System.Drawing.Size(16, 16);
			this.cmdNext.TabIndex = 10;
			this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
			// 
			// cmdFF
			// 
			this.cmdFF.Image = ((System.Drawing.Image)(resources.GetObject("cmdFF.Image")));
			this.cmdFF.Location = new System.Drawing.Point(53, 16);
			this.cmdFF.Name = "cmdFF";
			this.cmdFF.Size = new System.Drawing.Size(16, 16);
			this.cmdFF.TabIndex = 9;
			this.cmdFF.Click += new System.EventHandler(this.cmdFF_Click);
			// 
			// cmdPlay
			// 
			this.cmdPlay.Image = ((System.Drawing.Image)(resources.GetObject("cmdPlay.Image")));
			this.cmdPlay.Location = new System.Drawing.Point(21, 16);
			this.cmdPlay.Name = "cmdPlay";
			this.cmdPlay.Size = new System.Drawing.Size(16, 16);
			this.cmdPlay.TabIndex = 6;
			this.cmdPlay.Click += new System.EventHandler(this.cmdPlay_Click);
			// 
			// cmdStop
			// 
			this.cmdStop.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop.Image")));
			this.cmdStop.Location = new System.Drawing.Point(37, 16);
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new System.Drawing.Size(16, 16);
			this.cmdStop.TabIndex = 8;
			this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
			// 
			// lblTime
			// 
			this.lblTime.Location = new System.Drawing.Point(101, 0);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(72, 16);
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
			// pnlShipInfo
			// 
			this.pnlShipInfo.Controls.Add(this.optInfoOff);
			this.pnlShipInfo.Controls.Add(this.optInfoOn);
			this.pnlShipInfo.Controls.Add(this.cboInfoCraft);
			this.pnlShipInfo.Controls.Add(this.label22);
			this.pnlShipInfo.Location = new System.Drawing.Point(753, 147);
			this.pnlShipInfo.Name = "pnlShipInfo";
			this.pnlShipInfo.Size = new System.Drawing.Size(135, 74);
			this.pnlShipInfo.TabIndex = 24;
			this.pnlShipInfo.Visible = false;
			// 
			// optInfoOff
			// 
			this.optInfoOff.AutoSize = true;
			this.optInfoOff.Location = new System.Drawing.Point(80, 30);
			this.optInfoOff.Name = "optInfoOff";
			this.optInfoOff.Size = new System.Drawing.Size(39, 17);
			this.optInfoOff.TabIndex = 2;
			this.optInfoOff.Text = "Off";
			this.optInfoOff.UseVisualStyleBackColor = true;
			// 
			// optInfoOn
			// 
			this.optInfoOn.AutoSize = true;
			this.optInfoOn.Checked = true;
			this.optInfoOn.Location = new System.Drawing.Point(3, 30);
			this.optInfoOn.Name = "optInfoOn";
			this.optInfoOn.Size = new System.Drawing.Size(39, 17);
			this.optInfoOn.TabIndex = 2;
			this.optInfoOn.TabStop = true;
			this.optInfoOn.Text = "On";
			this.optInfoOn.UseVisualStyleBackColor = true;
			// 
			// cboInfoCraft
			// 
			this.cboInfoCraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboInfoCraft.FormattingEnabled = true;
			this.cboInfoCraft.Location = new System.Drawing.Point(3, 3);
			this.cboInfoCraft.Name = "cboInfoCraft";
			this.cboInfoCraft.Size = new System.Drawing.Size(128, 21);
			this.cboInfoCraft.TabIndex = 1;
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(3, 54);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(46, 13);
			this.label22.TabIndex = 0;
			this.label22.Text = "ShipInfo";
			this.label22.Visible = false;
			// 
			// cmdMoveShip
			// 
			this.cmdMoveShip.Location = new System.Drawing.Point(672, 189);
			this.cmdMoveShip.Name = "cmdMoveShip";
			this.cmdMoveShip.Size = new System.Drawing.Size(64, 23);
			this.cmdMoveShip.TabIndex = 23;
			this.cmdMoveShip.Text = "MoveShip";
			this.cmdMoveShip.UseVisualStyleBackColor = true;
			this.cmdMoveShip.Visible = false;
			this.cmdMoveShip.Click += new System.EventHandler(this.cmdMoveShip_Click);
			// 
			// cmdShipInfo
			// 
			this.cmdShipInfo.Location = new System.Drawing.Point(672, 218);
			this.cmdShipInfo.Name = "cmdShipInfo";
			this.cmdShipInfo.Size = new System.Drawing.Size(64, 23);
			this.cmdShipInfo.TabIndex = 23;
			this.cmdShipInfo.Text = "ShipInfo";
			this.cmdShipInfo.UseVisualStyleBackColor = true;
			this.cmdShipInfo.Visible = false;
			this.cmdShipInfo.Click += new System.EventHandler(this.cmdShipInfo_Click);
			// 
			// cmdRotate
			// 
			this.cmdRotate.Location = new System.Drawing.Point(608, 218);
			this.cmdRotate.Name = "cmdRotate";
			this.cmdRotate.Size = new System.Drawing.Size(64, 23);
			this.cmdRotate.TabIndex = 23;
			this.cmdRotate.Text = "Rotate";
			this.cmdRotate.UseVisualStyleBackColor = true;
			this.cmdRotate.Visible = false;
			this.cmdRotate.Click += new System.EventHandler(this.cmdRotate_Click);
			// 
			// cmdNewShip
			// 
			this.cmdNewShip.Location = new System.Drawing.Point(608, 189);
			this.cmdNewShip.Name = "cmdNewShip";
			this.cmdNewShip.Size = new System.Drawing.Size(64, 23);
			this.cmdNewShip.TabIndex = 23;
			this.cmdNewShip.Text = "NewShip";
			this.cmdNewShip.UseVisualStyleBackColor = true;
			this.cmdNewShip.Visible = false;
			this.cmdNewShip.Click += new System.EventHandler(this.cmdNewShip_Click);
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
            8,
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
			this.pnlTextTag.Controls.Add(this.cboColorTag);
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
            8,
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
			// cboColorTag
			// 
			this.cboColorTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboColorTag.Items.AddRange(new object[] {
            "Green",
            "Red",
            "Purple",
            "Blue",
            "Red",
            "Light Red",
            "Gray",
            "White"});
			this.cboColorTag.Location = new System.Drawing.Point(3, 56);
			this.cboColorTag.Name = "cboColorTag";
			this.cboColorTag.Size = new System.Drawing.Size(128, 21);
			this.cboColorTag.TabIndex = 16;
			this.cboColorTag.SelectedIndexChanged += new System.EventHandler(this.cboColorTag_SelectedIndexChanged);
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
			// cmdBreak
			// 
			this.cmdBreak.Location = new System.Drawing.Point(608, 160);
			this.cmdBreak.Name = "cmdBreak";
			this.cmdBreak.Size = new System.Drawing.Size(64, 23);
			this.cmdBreak.TabIndex = 13;
			this.cmdBreak.Text = "NewPage";
			this.cmdBreak.Click += new System.EventHandler(this.cmdBreak_Click);
			// 
			// tabStrings
			// 
			this.tabStrings.Controls.Add(this.label1);
			this.tabStrings.Controls.Add(this.dataT);
			this.tabStrings.Controls.Add(this.dataS);
			this.tabStrings.Controls.Add(this.label2);
			this.tabStrings.Location = new System.Drawing.Point(4, 22);
			this.tabStrings.Name = "tabStrings";
			this.tabStrings.Size = new System.Drawing.Size(1053, 358);
			this.tabStrings.TabIndex = 1;
			this.tabStrings.Text = "Tags and Strings";
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
			// dataS
			// 
			this.dataS.CaptionVisible = false;
			this.dataS.ColumnHeadersVisible = false;
			this.dataS.DataMember = "";
			this.dataS.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataS.Location = new System.Drawing.Point(184, 32);
			this.dataS.Name = "dataS";
			this.dataS.PreferredColumnWidth = 533;
			this.dataS.RowHeadersVisible = false;
			this.dataS.Size = new System.Drawing.Size(552, 312);
			this.dataS.TabIndex = 0;
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
			this.tabEvents.Controls.Add(this.cmdNew);
			this.tabEvents.Controls.Add(this.grpUnknown);
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
			this.tabEvents.Size = new System.Drawing.Size(1053, 358);
			this.tabEvents.TabIndex = 2;
			this.tabEvents.Text = "Event List";
			// 
			// cmdNew
			// 
			this.cmdNew.Location = new System.Drawing.Point(352, 8);
			this.cmdNew.Name = "cmdNew";
			this.cmdNew.Size = new System.Drawing.Size(88, 23);
			this.cmdNew.TabIndex = 7;
			this.cmdNew.Text = "&New Event";
			this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
			// 
			// grpUnknown
			// 
			this.grpUnknown.Controls.Add(this.label12);
			this.grpUnknown.Controls.Add(this.numUnk1);
			this.grpUnknown.Controls.Add(this.label13);
			this.grpUnknown.Controls.Add(this.numUnk3);
			this.grpUnknown.Controls.Add(this.label14);
			this.grpUnknown.Location = new System.Drawing.Point(624, 16);
			this.grpUnknown.Name = "grpUnknown";
			this.grpUnknown.Size = new System.Drawing.Size(104, 124);
			this.grpUnknown.TabIndex = 6;
			this.grpUnknown.TabStop = false;
			this.grpUnknown.Text = "Unknowns";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(18, 25);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(13, 13);
			this.label12.TabIndex = 1;
			this.label12.Text = "1";
			this.label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// numUnk1
			// 
			this.numUnk1.Location = new System.Drawing.Point(35, 25);
			this.numUnk1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numUnk1.Name = "numUnk1";
			this.numUnk1.Size = new System.Drawing.Size(56, 20);
			this.numUnk1.TabIndex = 0;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(17, 58);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(80, 13);
			this.label13.TabIndex = 1;
			this.label13.Text = "2 (Start Length)";
			this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// numUnk3
			// 
			this.numUnk3.Location = new System.Drawing.Point(35, 89);
			this.numUnk3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numUnk3.Name = "numUnk3";
			this.numUnk3.Size = new System.Drawing.Size(56, 20);
			this.numUnk3.TabIndex = 0;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(17, 89);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(13, 13);
			this.label14.TabIndex = 1;
			this.label14.Text = "3";
			this.label14.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// cmdUp
			// 
			this.cmdUp.Image = ((System.Drawing.Image)(resources.GetObject("cmdUp.Image")));
			this.cmdUp.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdUp.Location = new System.Drawing.Point(320, 8);
			this.cmdUp.Name = "cmdUp";
			this.cmdUp.Size = new System.Drawing.Size(24, 24);
			this.cmdUp.TabIndex = 5;
			this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
			// 
			// grpParameters
			// 
			this.grpParameters.Controls.Add(this.cboRotate);
			this.grpParameters.Controls.Add(this.cboCraft);
			this.grpParameters.Controls.Add(this.numRegion);
			this.grpParameters.Controls.Add(this.label19);
			this.grpParameters.Controls.Add(this.label17);
			this.grpParameters.Controls.Add(this.label16);
			this.grpParameters.Controls.Add(this.optOff);
			this.grpParameters.Controls.Add(this.optOn);
			this.grpParameters.Controls.Add(this.label8);
			this.grpParameters.Controls.Add(this.label18);
			this.grpParameters.Controls.Add(this.label4);
			this.grpParameters.Controls.Add(this.cboString);
			this.grpParameters.Controls.Add(this.cboTag);
			this.grpParameters.Controls.Add(this.cboFG);
			this.grpParameters.Controls.Add(this.label6);
			this.grpParameters.Controls.Add(this.label7);
			this.grpParameters.Controls.Add(this.label9);
			this.grpParameters.Controls.Add(this.cboColor);
			this.grpParameters.Controls.Add(this.label15);
			this.grpParameters.Controls.Add(this.label10);
			this.grpParameters.Controls.Add(this.numX);
			this.grpParameters.Controls.Add(this.numY);
			this.grpParameters.Controls.Add(this.cboIFF);
			this.grpParameters.Location = new System.Drawing.Point(320, 197);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(408, 149);
			this.grpParameters.TabIndex = 4;
			this.grpParameters.TabStop = false;
			this.grpParameters.Text = "Parameters";
			// 
			// cboRotate
			// 
			this.cboRotate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRotate.Enabled = false;
			this.cboRotate.FormattingEnabled = true;
			this.cboRotate.Items.AddRange(new object[] {
            "None",
            "Left 90°",
            "180°",
            "Right 90°",
            "Mirror"});
			this.cboRotate.Location = new System.Drawing.Point(50, 120);
			this.cboRotate.Name = "cboRotate";
			this.cboRotate.Size = new System.Drawing.Size(70, 21);
			this.cboRotate.TabIndex = 7;
			this.cboRotate.SelectedIndexChanged += new System.EventHandler(this.cboRotate_SelectedIndexChanged);
			// 
			// cboCraft
			// 
			this.cboCraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCraft.Enabled = false;
			this.cboCraft.FormattingEnabled = true;
			this.cboCraft.Location = new System.Drawing.Point(152, 120);
			this.cboCraft.Name = "cboCraft";
			this.cboCraft.Size = new System.Drawing.Size(142, 21);
			this.cboCraft.TabIndex = 6;
			this.cboCraft.SelectedIndexChanged += new System.EventHandler(this.cboCraft_SelectedIndexChanged);
			// 
			// numRegion
			// 
			this.numRegion.Enabled = false;
			this.numRegion.Location = new System.Drawing.Point(152, 89);
			this.numRegion.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.numRegion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRegion.Name = "numRegion";
			this.numRegion.Size = new System.Drawing.Size(33, 20);
			this.numRegion.TabIndex = 5;
			this.numRegion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRegion.ValueChanged += new System.EventHandler(this.numRegion_ValueChanged);
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(6, 123);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(42, 13);
			this.label19.TabIndex = 4;
			this.label19.Text = "Rotate:";
			this.label19.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(6, 90);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(35, 13);
			this.label17.TabIndex = 4;
			this.label17.Text = "State:";
			this.label17.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(108, 91);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(44, 13);
			this.label16.TabIndex = 4;
			this.label16.Text = "Region:";
			this.label16.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// optOff
			// 
			this.optOff.Enabled = false;
			this.optOff.Location = new System.Drawing.Point(48, 97);
			this.optOff.Name = "optOff";
			this.optOff.Size = new System.Drawing.Size(40, 16);
			this.optOff.TabIndex = 3;
			this.optOff.TabStop = true;
			this.optOff.Text = "Off";
			this.optOff.UseVisualStyleBackColor = true;
			// 
			// optOn
			// 
			this.optOn.Enabled = false;
			this.optOn.Location = new System.Drawing.Point(48, 82);
			this.optOn.Name = "optOn";
			this.optOn.Size = new System.Drawing.Size(40, 16);
			this.optOn.TabIndex = 3;
			this.optOn.TabStop = true;
			this.optOn.Text = "On";
			this.optOn.UseVisualStyleBackColor = true;
			this.optOn.CheckedChanged += new System.EventHandler(this.optOn_CheckedChanged);
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
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(116, 121);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(37, 16);
			this.label18.TabIndex = 1;
			this.label18.Text = "Craft:";
			this.label18.TextAlign = System.Drawing.ContentAlignment.BottomRight;
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
			this.cboString.TabIndex = 0;
			this.cboString.SelectedIndexChanged += new System.EventHandler(this.cboString_SelectedIndexChanged);
			// 
			// cboTag
			// 
			this.cboTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTag.Enabled = false;
			this.cboTag.Location = new System.Drawing.Point(152, 56);
			this.cboTag.Name = "cboTag";
			this.cboTag.Size = new System.Drawing.Size(121, 21);
			this.cboTag.TabIndex = 0;
			this.cboTag.SelectedIndexChanged += new System.EventHandler(this.cboTag_SelectedIndexChanged);
			// 
			// cboFG
			// 
			this.cboFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFG.Enabled = false;
			this.cboFG.Location = new System.Drawing.Point(280, 88);
			this.cboFG.Name = "cboFG";
			this.cboFG.Size = new System.Drawing.Size(121, 21);
			this.cboFG.TabIndex = 0;
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
			this.label7.Location = new System.Drawing.Point(208, 88);
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
			// cboColor
			// 
			this.cboColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboColor.Enabled = false;
			this.cboColor.Items.AddRange(new object[] {
            "Green",
            "Red",
            "Purple",
            "Blue",
            "Red",
            "Light Red",
            "Gray",
            "White"});
			this.cboColor.Location = new System.Drawing.Point(320, 56);
			this.cboColor.Name = "cboColor";
			this.cboColor.Size = new System.Drawing.Size(80, 21);
			this.cboColor.TabIndex = 0;
			this.cboColor.SelectedIndexChanged += new System.EventHandler(this.cboColor_SelectedIndexChanged);
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(280, 121);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(40, 16);
			this.label15.TabIndex = 1;
			this.label15.Text = "IFF:";
			this.label15.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(280, 56);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(40, 16);
			this.label10.TabIndex = 1;
			this.label10.Text = "Color:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomRight;
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
			this.numX.TabIndex = 2;
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
			this.numY.TabIndex = 2;
			this.numY.ValueChanged += new System.EventHandler(this.numY_ValueChanged);
			// 
			// cboIFF
			// 
			this.cboIFF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboIFF.Enabled = false;
			this.cboIFF.Items.AddRange(new object[] {
            "Rebel",
            "Imperial",
            "Blue",
            "Yellow",
            "Red",
            "Purple"});
			this.cboIFF.Location = new System.Drawing.Point(320, 120);
			this.cboIFF.Name = "cboIFF";
			this.cboIFF.Size = new System.Drawing.Size(80, 21);
			this.cboIFF.TabIndex = 0;
			this.cboIFF.SelectedIndexChanged += new System.EventHandler(this.cboIFF_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(352, 155);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Event Time:";
			// 
			// numTime
			// 
			this.numTime.Location = new System.Drawing.Point(352, 171);
			this.numTime.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.numTime.Name = "numTime";
			this.numTime.Size = new System.Drawing.Size(48, 20);
			this.numTime.TabIndex = 2;
			this.numTime.ValueChanged += new System.EventHandler(this.numTime_ValueChanged);
			// 
			// cboEvent
			// 
			this.cboEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEvent.Items.AddRange(new object[] {
            "Page Break",
            "Title Text",
            "Caption Text",
            "Move Map",
            "Zoom Map",
            "Clear FG Tags",
            "FG Tag 1",
            "FG Tag 2",
            "FG Tag 3",
            "FG Tag 4",
            "FG Tag 5",
            "FG Tag 6",
            "FG Tag 7",
            "FG Tag 8",
            "Clear Text Tags",
            "Text Tag 1",
            "Text Tag 2",
            "Text Tag 3",
            "Text Tag 4",
            "Text Tag 5",
            "Text Tag 6",
            "Text Tag 7",
            "Text Tag 8"});
			this.cboEvent.Location = new System.Drawing.Point(536, 171);
			this.cboEvent.Name = "cboEvent";
			this.cboEvent.Size = new System.Drawing.Size(160, 21);
			this.cboEvent.TabIndex = 1;
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
			this.lblEventTime.Location = new System.Drawing.Point(400, 171);
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
			this.cmdDown.Image = ((System.Drawing.Image)(resources.GetObject("cmdDown.Image")));
			this.cmdDown.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdDown.Location = new System.Drawing.Point(320, 40);
			this.cmdDown.Name = "cmdDown";
			this.cmdDown.Size = new System.Drawing.Size(24, 24);
			this.cmdDown.TabIndex = 5;
			this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Location = new System.Drawing.Point(352, 40);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(88, 23);
			this.cmdDelete.TabIndex = 7;
			this.cmdDelete.Text = "&Delete Event";
			this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			// 
			// frmBrief
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1073, 386);
			this.Controls.Add(this.tabBrief);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmBrief";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmBrief";
			this.Load += new System.EventHandler(this.frmBrief_Load);
			this.Closed += new System.EventHandler(this.frmBrief_Closed);
			this.Activated += new System.EventHandler(this.frmBrief_Activated);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmBrief_Closing);
			((System.ComponentModel.ISupportInitialize)(this.pctBrief)).EndInit();
			this.tabBrief.ResumeLayout(false);
			this.tabDisplay.ResumeLayout(false);
			this.pnlRegion.ResumeLayout(false);
			this.pnlRegion.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numNewRegion)).EndInit();
			this.pnlNew.ResumeLayout(false);
			this.pnlNew.PerformLayout();
			this.pnlMove.ResumeLayout(false);
			this.pnlMove.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMoveTime)).EndInit();
			this.pnlRotate.ResumeLayout(false);
			this.pnlRotate.PerformLayout();
			this.pnlBottomLeft.ResumeLayout(false);
			this.pnlBottomRight.ResumeLayout(false);
			this.pnlBottomRight.PerformLayout();
			this.pnlShipInfo.ResumeLayout(false);
			this.pnlShipInfo.PerformLayout();
			this.pnlShipTag.ResumeLayout(false);
			this.pnlShipTag.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFG)).EndInit();
			this.pnlTextTag.ResumeLayout(false);
			this.pnlTextTag.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numText)).EndInit();
			this.tabStrings.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataT)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataS)).EndInit();
			this.tabEvents.ResumeLayout(false);
			this.grpUnknown.ResumeLayout(false);
			this.grpUnknown.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnk1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnk3)).EndInit();
			this.grpParameters.ResumeLayout(false);
			this.grpParameters.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRegion)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numTime)).EndInit();
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
		DataGrid dataS;
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
		ComboBox cboColor;
		Label label10;
		NumericUpDown numX;
		NumericUpDown numY;
		Label label11;
		Button cmdUp;
		Button cmdDown;
		GroupBox grpParameters;
		GroupBox grpUnknown;
		NumericUpDown numUnk1;
		Label label12;
		Label label13;
		NumericUpDown numUnk3;
		Label label14;
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
		Button cmdBreak;
		VScrollBar vsbBRF;
		HScrollBar hsbBRF;
		ComboBox cboColorTag;
		Label lblInstruction;
		Label lblTime;
		RadioButton optOff;
		RadioButton optOn;
		ComboBox cboIFF;
		Label label15;
		NumericUpDown numRegion;
		Label label16;
		Label label17;
		ComboBox cboRotate;
		ComboBox cboCraft;
		Label label19;
		Label label18;
		Panel pnlShipTag;
		Panel pnlTextTag;
		Button cmdMoveShip;
		Button cmdShipInfo;
		Button cmdRotate;
		Button cmdNewShip;
		Panel pnlShipInfo;
		RadioButton optInfoOff;
		RadioButton optInfoOn;
		ComboBox cboInfoCraft;
		Label label22;
		Label label20;
		Label label21;
		Panel pnlBottomRight;
		Panel pnlBottomLeft;
		Button cmdRegion;
		Panel pnlRotate;
		Label label23;
		ComboBox cboRotateAmount;
		ComboBox cboRCraft;
		Panel pnlMove;
		Label label25;
		NumericUpDown numMoveTime;
		Label label24;
		ComboBox cboMoveIcon;
		Panel pnlNew;
		ComboBox cboIconIff;
		ComboBox cboNCraft;
		ComboBox cboNewIcon;
		Label label26;
		Panel pnlRegion;
		NumericUpDown numNewRegion;
		Label label27;
	}
}