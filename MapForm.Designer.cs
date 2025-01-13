using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class MapForm
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
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapForm));
			this.pctMap = new System.Windows.Forms.PictureBox();
			this.hscZoom = new System.Windows.Forms.HScrollBar();
			this.lblCoor1 = new System.Windows.Forms.Label();
			this.lblCoor2 = new System.Windows.Forms.Label();
			this.lblZoom = new System.Windows.Forms.Label();
			this.optYZ = new System.Windows.Forms.RadioButton();
			this.grpDir = new System.Windows.Forms.GroupBox();
			this.optXY = new System.Windows.Forms.RadioButton();
			this.optXZ = new System.Windows.Forms.RadioButton();
			this.imgCraft = new System.Windows.Forms.ImageList(this.components);
			this.grpPoints = new System.Windows.Forms.GroupBox();
			this.chkSP1 = new System.Windows.Forms.CheckBox();
			this.chkSP2 = new System.Windows.Forms.CheckBox();
			this.chkSP3 = new System.Windows.Forms.CheckBox();
			this.chkSP4 = new System.Windows.Forms.CheckBox();
			this.chkWP1 = new System.Windows.Forms.CheckBox();
			this.chkWP2 = new System.Windows.Forms.CheckBox();
			this.chkWP3 = new System.Windows.Forms.CheckBox();
			this.chkWP4 = new System.Windows.Forms.CheckBox();
			this.chkWP5 = new System.Windows.Forms.CheckBox();
			this.chkWP6 = new System.Windows.Forms.CheckBox();
			this.chkWP7 = new System.Windows.Forms.CheckBox();
			this.chkWP8 = new System.Windows.Forms.CheckBox();
			this.chkRDV = new System.Windows.Forms.CheckBox();
			this.chkHYP = new System.Windows.Forms.CheckBox();
			this.chkBRF8 = new System.Windows.Forms.CheckBox();
			this.chkBRF7 = new System.Windows.Forms.CheckBox();
			this.chkBRF6 = new System.Windows.Forms.CheckBox();
			this.chkBRF5 = new System.Windows.Forms.CheckBox();
			this.chkBRF4 = new System.Windows.Forms.CheckBox();
			this.chkBRF3 = new System.Windows.Forms.CheckBox();
			this.chkBRF2 = new System.Windows.Forms.CheckBox();
			this.chkBRF = new System.Windows.Forms.CheckBox();
			this.chkTags = new System.Windows.Forms.CheckBox();
			this.chkTrace = new System.Windows.Forms.CheckBox();
			this.lblRegion = new System.Windows.Forms.Label();
			this.numRegion = new System.Windows.Forms.NumericUpDown();
			this.lblOrder = new System.Windows.Forms.Label();
			this.numOrder = new System.Windows.Forms.NumericUpDown();
			this.lstCraft = new System.Windows.Forms.ListBox();
			this.cmdHideNone = new System.Windows.Forms.Button();
			this.lstSelection = new System.Windows.Forms.ListBox();
			this.lblSelection = new System.Windows.Forms.Label();
			this.cmdHelp = new System.Windows.Forms.Button();
			this.chkDistance = new System.Windows.Forms.CheckBox();
			this.mapPaintRedrawTimer = new System.Windows.Forms.Timer(this.components);
			this.lblFade = new System.Windows.Forms.Label();
			this.cmdFadeAdd = new System.Windows.Forms.Button();
			this.cmdFadeNone = new System.Windows.Forms.Button();
			this.cmdFadeSubtract = new System.Windows.Forms.Button();
			this.lblHide = new System.Windows.Forms.Label();
			this.cmdHideAdd = new System.Windows.Forms.Button();
			this.cmdHideSubtract = new System.Windows.Forms.Button();
			this.cboSnapUnit = new System.Windows.Forms.ComboBox();
			this.lblSnapTo = new System.Windows.Forms.Label();
			this.cboSnapTo = new System.Windows.Forms.ComboBox();
			this.numSnapAmount = new System.Windows.Forms.NumericUpDown();
			this.lblExpandSelection = new System.Windows.Forms.Label();
			this.cmdExpandByCraft = new System.Windows.Forms.Button();
			this.cmdExpandByIff = new System.Windows.Forms.Button();
			this.cmdExpandBySize = new System.Windows.Forms.Button();
			this.cmdInvertSelection = new System.Windows.Forms.Button();
			this.chkTime = new System.Windows.Forms.CheckBox();
			this.chkTraceHideFade = new System.Windows.Forms.CheckBox();
			this.chkTraceSelected = new System.Windows.Forms.CheckBox();
			this.cmdFitSelected = new System.Windows.Forms.Button();
			this.lblFit = new System.Windows.Forms.Label();
			this.cmdFitWorld = new System.Windows.Forms.Button();
			this.cmdFitDefault = new System.Windows.Forms.Button();
			this.lblCenterOn = new System.Windows.Forms.Label();
			this.cmdCenterSelected = new System.Windows.Forms.Button();
			this.cboViewDifficulty = new System.Windows.Forms.ComboBox();
			this.cboViewIff = new System.Windows.Forms.ComboBox();
			this.chkWireframe = new System.Windows.Forms.CheckBox();
			this.chkLimit = new System.Windows.Forms.CheckBox();
			this.chkCumulative = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pctMap)).BeginInit();
			this.grpDir.SuspendLayout();
			this.grpPoints.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRegion)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numSnapAmount)).BeginInit();
			this.SuspendLayout();
			// 
			// pctMap
			// 
			this.pctMap.BackColor = System.Drawing.Color.Black;
			this.pctMap.Location = new System.Drawing.Point(122, 64);
			this.pctMap.Name = "pctMap";
			this.pctMap.Size = new System.Drawing.Size(622, 408);
			this.pctMap.TabIndex = 28;
			this.pctMap.TabStop = false;
			this.pctMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pctMap_Paint);
			this.pctMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctMap_MouseDown);
			this.pctMap.MouseEnter += new System.EventHandler(this.pctMap_MouseEnter);
			this.pctMap.MouseLeave += new System.EventHandler(this.pctMap_MouseLeave);
			this.pctMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctMap_MouseMove);
			this.pctMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctMap_MouseUp);
			this.pctMap.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.pctMap_PreviewKeyDown);
			// 
			// hscZoom
			// 
			this.hscZoom.Location = new System.Drawing.Point(290, 504);
			this.hscZoom.Maximum = 2000;
			this.hscZoom.Minimum = 5;
			this.hscZoom.Name = "hscZoom";
			this.hscZoom.Size = new System.Drawing.Size(342, 16);
			this.hscZoom.TabIndex = 58;
			this.hscZoom.Value = 40;
			this.hscZoom.ValueChanged += new System.EventHandler(this.hscZoom_ValueChanged);
			// 
			// lblCoor1
			// 
			this.lblCoor1.Location = new System.Drawing.Point(15, 504);
			this.lblCoor1.Name = "lblCoor1";
			this.lblCoor1.Size = new System.Drawing.Size(96, 16);
			this.lblCoor1.TabIndex = 27;
			this.lblCoor1.Text = "X:";
			// 
			// lblCoor2
			// 
			this.lblCoor2.Location = new System.Drawing.Point(110, 504);
			this.lblCoor2.Name = "lblCoor2";
			this.lblCoor2.Size = new System.Drawing.Size(96, 16);
			this.lblCoor2.TabIndex = 26;
			this.lblCoor2.Text = "Y:";
			// 
			// lblZoom
			// 
			this.lblZoom.Location = new System.Drawing.Point(212, 504);
			this.lblZoom.Name = "lblZoom";
			this.lblZoom.Size = new System.Drawing.Size(64, 16);
			this.lblZoom.TabIndex = 25;
			this.lblZoom.Text = "Zoom: 40";
			// 
			// optYZ
			// 
			this.optYZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.optYZ.Location = new System.Drawing.Point(8, 58);
			this.optYZ.Name = "optYZ";
			this.optYZ.Size = new System.Drawing.Size(40, 24);
			this.optYZ.TabIndex = 3;
			this.optYZ.TabStop = true;
			this.optYZ.Text = "Y-Z";
			this.optYZ.CheckedChanged += new System.EventHandler(this.optYZ_CheckedChanged);
			// 
			// grpDir
			// 
			this.grpDir.Controls.Add(this.optXY);
			this.grpDir.Controls.Add(this.optXZ);
			this.grpDir.Controls.Add(this.optYZ);
			this.grpDir.Location = new System.Drawing.Point(750, 3);
			this.grpDir.Name = "grpDir";
			this.grpDir.Size = new System.Drawing.Size(64, 84);
			this.grpDir.TabIndex = 0;
			this.grpDir.TabStop = false;
			// 
			// optXY
			// 
			this.optXY.Checked = true;
			this.optXY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.optXY.Location = new System.Drawing.Point(8, 10);
			this.optXY.Name = "optXY";
			this.optXY.Size = new System.Drawing.Size(40, 24);
			this.optXY.TabIndex = 1;
			this.optXY.TabStop = true;
			this.optXY.Text = "X-Y";
			this.optXY.CheckedChanged += new System.EventHandler(this.optXY_CheckedChanged);
			// 
			// optXZ
			// 
			this.optXZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.optXZ.Location = new System.Drawing.Point(8, 34);
			this.optXZ.Name = "optXZ";
			this.optXZ.Size = new System.Drawing.Size(40, 24);
			this.optXZ.TabIndex = 2;
			this.optXZ.TabStop = true;
			this.optXZ.Text = "X-Z";
			this.optXZ.CheckedChanged += new System.EventHandler(this.optXZ_CheckedChanged);
			// 
			// imgCraft
			// 
			this.imgCraft.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgCraft.ImageSize = new System.Drawing.Size(16, 16);
			this.imgCraft.TransparentColor = System.Drawing.Color.Black;
			// 
			// grpPoints
			// 
			this.grpPoints.Controls.Add(this.chkSP1);
			this.grpPoints.Controls.Add(this.chkSP2);
			this.grpPoints.Controls.Add(this.chkSP3);
			this.grpPoints.Controls.Add(this.chkSP4);
			this.grpPoints.Controls.Add(this.chkWP1);
			this.grpPoints.Controls.Add(this.chkWP2);
			this.grpPoints.Controls.Add(this.chkWP3);
			this.grpPoints.Controls.Add(this.chkWP4);
			this.grpPoints.Controls.Add(this.chkWP5);
			this.grpPoints.Controls.Add(this.chkWP6);
			this.grpPoints.Controls.Add(this.chkWP7);
			this.grpPoints.Controls.Add(this.chkWP8);
			this.grpPoints.Controls.Add(this.chkRDV);
			this.grpPoints.Controls.Add(this.chkHYP);
			this.grpPoints.Controls.Add(this.chkBRF8);
			this.grpPoints.Controls.Add(this.chkBRF7);
			this.grpPoints.Controls.Add(this.chkBRF6);
			this.grpPoints.Controls.Add(this.chkBRF5);
			this.grpPoints.Controls.Add(this.chkBRF4);
			this.grpPoints.Controls.Add(this.chkBRF3);
			this.grpPoints.Controls.Add(this.chkBRF2);
			this.grpPoints.Controls.Add(this.chkBRF);
			this.grpPoints.Location = new System.Drawing.Point(750, 97);
			this.grpPoints.Name = "grpPoints";
			this.grpPoints.Size = new System.Drawing.Size(64, 362);
			this.grpPoints.TabIndex = 5;
			this.grpPoints.TabStop = false;
			// 
			// chkSP1
			// 
			this.chkSP1.Checked = true;
			this.chkSP1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSP1.Location = new System.Drawing.Point(8, 8);
			this.chkSP1.Name = "chkSP1";
			this.chkSP1.Size = new System.Drawing.Size(48, 16);
			this.chkSP1.TabIndex = 6;
			this.chkSP1.Text = "SP1";
			// 
			// chkSP2
			// 
			this.chkSP2.Location = new System.Drawing.Point(8, 24);
			this.chkSP2.Name = "chkSP2";
			this.chkSP2.Size = new System.Drawing.Size(48, 16);
			this.chkSP2.TabIndex = 7;
			this.chkSP2.Text = "SP2";
			// 
			// chkSP3
			// 
			this.chkSP3.Location = new System.Drawing.Point(8, 40);
			this.chkSP3.Name = "chkSP3";
			this.chkSP3.Size = new System.Drawing.Size(48, 16);
			this.chkSP3.TabIndex = 8;
			this.chkSP3.Text = "SP3";
			// 
			// chkSP4
			// 
			this.chkSP4.Location = new System.Drawing.Point(8, 56);
			this.chkSP4.Name = "chkSP4";
			this.chkSP4.Size = new System.Drawing.Size(48, 16);
			this.chkSP4.TabIndex = 9;
			this.chkSP4.Text = "SP4";
			// 
			// chkWP1
			// 
			this.chkWP1.Location = new System.Drawing.Point(8, 72);
			this.chkWP1.Name = "chkWP1";
			this.chkWP1.Size = new System.Drawing.Size(48, 16);
			this.chkWP1.TabIndex = 10;
			this.chkWP1.Text = "WP1";
			// 
			// chkWP2
			// 
			this.chkWP2.Location = new System.Drawing.Point(8, 88);
			this.chkWP2.Name = "chkWP2";
			this.chkWP2.Size = new System.Drawing.Size(48, 16);
			this.chkWP2.TabIndex = 11;
			this.chkWP2.Text = "WP2";
			// 
			// chkWP3
			// 
			this.chkWP3.Location = new System.Drawing.Point(8, 104);
			this.chkWP3.Name = "chkWP3";
			this.chkWP3.Size = new System.Drawing.Size(48, 16);
			this.chkWP3.TabIndex = 12;
			this.chkWP3.Text = "WP3";
			// 
			// chkWP4
			// 
			this.chkWP4.Location = new System.Drawing.Point(8, 120);
			this.chkWP4.Name = "chkWP4";
			this.chkWP4.Size = new System.Drawing.Size(48, 16);
			this.chkWP4.TabIndex = 13;
			this.chkWP4.Text = "WP4";
			// 
			// chkWP5
			// 
			this.chkWP5.Location = new System.Drawing.Point(8, 136);
			this.chkWP5.Name = "chkWP5";
			this.chkWP5.Size = new System.Drawing.Size(48, 16);
			this.chkWP5.TabIndex = 14;
			this.chkWP5.Text = "WP5";
			// 
			// chkWP6
			// 
			this.chkWP6.Location = new System.Drawing.Point(8, 152);
			this.chkWP6.Name = "chkWP6";
			this.chkWP6.Size = new System.Drawing.Size(48, 16);
			this.chkWP6.TabIndex = 15;
			this.chkWP6.Text = "WP6";
			// 
			// chkWP7
			// 
			this.chkWP7.Location = new System.Drawing.Point(8, 168);
			this.chkWP7.Name = "chkWP7";
			this.chkWP7.Size = new System.Drawing.Size(48, 16);
			this.chkWP7.TabIndex = 16;
			this.chkWP7.Text = "WP7";
			// 
			// chkWP8
			// 
			this.chkWP8.Location = new System.Drawing.Point(8, 184);
			this.chkWP8.Name = "chkWP8";
			this.chkWP8.Size = new System.Drawing.Size(48, 16);
			this.chkWP8.TabIndex = 17;
			this.chkWP8.Text = "WP8";
			// 
			// chkRDV
			// 
			this.chkRDV.Location = new System.Drawing.Point(8, 200);
			this.chkRDV.Name = "chkRDV";
			this.chkRDV.Size = new System.Drawing.Size(48, 16);
			this.chkRDV.TabIndex = 18;
			this.chkRDV.Text = "RDV";
			// 
			// chkHYP
			// 
			this.chkHYP.Location = new System.Drawing.Point(8, 216);
			this.chkHYP.Name = "chkHYP";
			this.chkHYP.Size = new System.Drawing.Size(48, 16);
			this.chkHYP.TabIndex = 19;
			this.chkHYP.Text = "HYP";
			// 
			// chkBRF8
			// 
			this.chkBRF8.Location = new System.Drawing.Point(8, 344);
			this.chkBRF8.Name = "chkBRF8";
			this.chkBRF8.Size = new System.Drawing.Size(48, 16);
			this.chkBRF8.TabIndex = 27;
			this.chkBRF8.Text = "BF8";
			// 
			// chkBRF7
			// 
			this.chkBRF7.Location = new System.Drawing.Point(8, 328);
			this.chkBRF7.Name = "chkBRF7";
			this.chkBRF7.Size = new System.Drawing.Size(48, 16);
			this.chkBRF7.TabIndex = 26;
			this.chkBRF7.Text = "BF7";
			// 
			// chkBRF6
			// 
			this.chkBRF6.Location = new System.Drawing.Point(8, 312);
			this.chkBRF6.Name = "chkBRF6";
			this.chkBRF6.Size = new System.Drawing.Size(48, 16);
			this.chkBRF6.TabIndex = 25;
			this.chkBRF6.Text = "BF6";
			// 
			// chkBRF5
			// 
			this.chkBRF5.Location = new System.Drawing.Point(8, 296);
			this.chkBRF5.Name = "chkBRF5";
			this.chkBRF5.Size = new System.Drawing.Size(48, 16);
			this.chkBRF5.TabIndex = 24;
			this.chkBRF5.Text = "BF5";
			// 
			// chkBRF4
			// 
			this.chkBRF4.Location = new System.Drawing.Point(8, 280);
			this.chkBRF4.Name = "chkBRF4";
			this.chkBRF4.Size = new System.Drawing.Size(48, 16);
			this.chkBRF4.TabIndex = 23;
			this.chkBRF4.Text = "BF4";
			// 
			// chkBRF3
			// 
			this.chkBRF3.Location = new System.Drawing.Point(8, 264);
			this.chkBRF3.Name = "chkBRF3";
			this.chkBRF3.Size = new System.Drawing.Size(48, 16);
			this.chkBRF3.TabIndex = 22;
			this.chkBRF3.Text = "BF3";
			// 
			// chkBRF2
			// 
			this.chkBRF2.Location = new System.Drawing.Point(8, 248);
			this.chkBRF2.Name = "chkBRF2";
			this.chkBRF2.Size = new System.Drawing.Size(48, 16);
			this.chkBRF2.TabIndex = 21;
			this.chkBRF2.Text = "BF2";
			// 
			// chkBRF
			// 
			this.chkBRF.Location = new System.Drawing.Point(8, 232);
			this.chkBRF.Name = "chkBRF";
			this.chkBRF.Size = new System.Drawing.Size(48, 16);
			this.chkBRF.TabIndex = 20;
			this.chkBRF.Text = "BRF";
			// 
			// chkTags
			// 
			this.chkTags.Checked = true;
			this.chkTags.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTags.Location = new System.Drawing.Point(750, 465);
			this.chkTags.Margin = new System.Windows.Forms.Padding(0);
			this.chkTags.Name = "chkTags";
			this.chkTags.Size = new System.Drawing.Size(72, 17);
			this.chkTags.TabIndex = 28;
			this.chkTags.Text = "FG Tags";
			this.chkTags.CheckedChanged += new System.EventHandler(this.chkTags_CheckedChanged);
			// 
			// chkTrace
			// 
			this.chkTrace.Checked = true;
			this.chkTrace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTrace.Location = new System.Drawing.Point(750, 482);
			this.chkTrace.Margin = new System.Windows.Forms.Padding(0);
			this.chkTrace.Name = "chkTrace";
			this.chkTrace.Size = new System.Drawing.Size(92, 17);
			this.chkTrace.TabIndex = 29;
			this.chkTrace.Text = "Traces";
			this.chkTrace.CheckedChanged += new System.EventHandler(this.chkTrace_CheckedChanged);
			// 
			// lblRegion
			// 
			this.lblRegion.AutoSize = true;
			this.lblRegion.Location = new System.Drawing.Point(657, 11);
			this.lblRegion.Name = "lblRegion";
			this.lblRegion.Size = new System.Drawing.Size(44, 13);
			this.lblRegion.TabIndex = 31;
			this.lblRegion.Text = "Region:";
			this.lblRegion.Visible = false;
			// 
			// numRegion
			// 
			this.numRegion.Location = new System.Drawing.Point(707, 9);
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
			this.numRegion.TabIndex = 35;
			this.numRegion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRegion.Visible = false;
			this.numRegion.ValueChanged += new System.EventHandler(this.numRegion_ValueChanged);
			// 
			// lblOrder
			// 
			this.lblOrder.AutoSize = true;
			this.lblOrder.Location = new System.Drawing.Point(568, 11);
			this.lblOrder.Name = "lblOrder";
			this.lblOrder.Size = new System.Drawing.Size(36, 13);
			this.lblOrder.TabIndex = 31;
			this.lblOrder.Text = "Order:";
			this.lblOrder.Visible = false;
			// 
			// numOrder
			// 
			this.numOrder.Location = new System.Drawing.Point(610, 9);
			this.numOrder.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.numOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOrder.Name = "numOrder";
			this.numOrder.Size = new System.Drawing.Size(33, 20);
			this.numOrder.TabIndex = 34;
			this.numOrder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numOrder.Visible = false;
			this.numOrder.ValueChanged += new System.EventHandler(this.numOrder_ValueChanged);
			// 
			// lstCraft
			// 
			this.lstCraft.BackColor = System.Drawing.Color.Black;
			this.lstCraft.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstCraft.ForeColor = System.Drawing.Color.Gray;
			this.lstCraft.FormattingEnabled = true;
			this.lstCraft.Location = new System.Drawing.Point(2, 64);
			this.lstCraft.Name = "lstCraft";
			this.lstCraft.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstCraft.Size = new System.Drawing.Size(114, 407);
			this.lstCraft.TabIndex = 50;
			this.lstCraft.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstCraft_DrawItem);
			this.lstCraft.SelectedIndexChanged += new System.EventHandler(this.lstCraft_SelectedIndexChanged);
			// 
			// cmdHideNone
			// 
			this.cmdHideNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdHideNone.Location = new System.Drawing.Point(413, 35);
			this.cmdHideNone.Name = "cmdHideNone";
			this.cmdHideNone.Size = new System.Drawing.Size(42, 22);
			this.cmdHideNone.TabIndex = 44;
			this.cmdHideNone.Text = "None";
			this.cmdHideNone.UseVisualStyleBackColor = true;
			this.cmdHideNone.Click += new System.EventHandler(this.cmdHideNone_Click);
			// 
			// lstSelection
			// 
			this.lstSelection.BackColor = System.Drawing.Color.Black;
			this.lstSelection.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.lstSelection.ForeColor = System.Drawing.Color.Gray;
			this.lstSelection.FormattingEnabled = true;
			this.lstSelection.Location = new System.Drawing.Point(122, 81);
			this.lstSelection.Name = "lstSelection";
			this.lstSelection.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstSelection.Size = new System.Drawing.Size(134, 17);
			this.lstSelection.TabIndex = 51;
			this.lstSelection.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstSelection_DrawItem);
			this.lstSelection.SelectedIndexChanged += new System.EventHandler(this.lstSelection_SelectedIndexChanged);
			// 
			// lblSelection
			// 
			this.lblSelection.AutoSize = true;
			this.lblSelection.Location = new System.Drawing.Point(120, 64);
			this.lblSelection.Name = "lblSelection";
			this.lblSelection.Size = new System.Drawing.Size(54, 13);
			this.lblSelection.TabIndex = 39;
			this.lblSelection.Text = "Selection:";
			// 
			// cmdHelp
			// 
			this.cmdHelp.Location = new System.Drawing.Point(2, 3);
			this.cmdHelp.Name = "cmdHelp";
			this.cmdHelp.Size = new System.Drawing.Size(38, 21);
			this.cmdHelp.TabIndex = 49;
			this.cmdHelp.Text = "Help";
			this.cmdHelp.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.cmdHelp.UseVisualStyleBackColor = true;
			this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
			// 
			// chkDistance
			// 
			this.chkDistance.Location = new System.Drawing.Point(750, 499);
			this.chkDistance.Margin = new System.Windows.Forms.Padding(0);
			this.chkDistance.Name = "chkDistance";
			this.chkDistance.Size = new System.Drawing.Size(68, 17);
			this.chkDistance.TabIndex = 30;
			this.chkDistance.Text = "Distance";
			this.chkDistance.UseVisualStyleBackColor = true;
			this.chkDistance.CheckedChanged += new System.EventHandler(this.chkDistance_CheckedChanged);
			// 
			// mapPaintRedrawTimer
			// 
			this.mapPaintRedrawTimer.Interval = 17;
			this.mapPaintRedrawTimer.Tick += new System.EventHandler(this.mapPaintRedrawTimer_Tick);
			// 
			// lblFade
			// 
			this.lblFade.AutoSize = true;
			this.lblFade.Location = new System.Drawing.Point(282, 17);
			this.lblFade.Name = "lblFade";
			this.lblFade.Size = new System.Drawing.Size(73, 13);
			this.lblFade.TabIndex = 42;
			this.lblFade.Text = "Faded: 0 craft";
			// 
			// cmdFadeAdd
			// 
			this.cmdFadeAdd.Location = new System.Drawing.Point(361, 12);
			this.cmdFadeAdd.Name = "cmdFadeAdd";
			this.cmdFadeAdd.Size = new System.Drawing.Size(20, 22);
			this.cmdFadeAdd.TabIndex = 39;
			this.cmdFadeAdd.Text = "+";
			this.cmdFadeAdd.UseVisualStyleBackColor = true;
			this.cmdFadeAdd.Click += new System.EventHandler(this.cmdFadeAdd_Click);
			// 
			// cmdFadeNone
			// 
			this.cmdFadeNone.Location = new System.Drawing.Point(413, 12);
			this.cmdFadeNone.Name = "cmdFadeNone";
			this.cmdFadeNone.Size = new System.Drawing.Size(42, 22);
			this.cmdFadeNone.TabIndex = 41;
			this.cmdFadeNone.Text = "None";
			this.cmdFadeNone.UseVisualStyleBackColor = true;
			this.cmdFadeNone.Click += new System.EventHandler(this.cmdFadeNone_Click);
			// 
			// cmdFadeSubtract
			// 
			this.cmdFadeSubtract.Location = new System.Drawing.Point(387, 12);
			this.cmdFadeSubtract.Name = "cmdFadeSubtract";
			this.cmdFadeSubtract.Size = new System.Drawing.Size(20, 22);
			this.cmdFadeSubtract.TabIndex = 40;
			this.cmdFadeSubtract.Text = "-";
			this.cmdFadeSubtract.UseVisualStyleBackColor = true;
			this.cmdFadeSubtract.Click += new System.EventHandler(this.cmdFadeSubtract_Click);
			// 
			// lblHide
			// 
			this.lblHide.AutoSize = true;
			this.lblHide.Location = new System.Drawing.Point(278, 40);
			this.lblHide.Name = "lblHide";
			this.lblHide.Size = new System.Drawing.Size(77, 13);
			this.lblHide.TabIndex = 42;
			this.lblHide.Text = "Hidden: 0 craft";
			// 
			// cmdHideAdd
			// 
			this.cmdHideAdd.Location = new System.Drawing.Point(361, 35);
			this.cmdHideAdd.Name = "cmdHideAdd";
			this.cmdHideAdd.Size = new System.Drawing.Size(20, 22);
			this.cmdHideAdd.TabIndex = 42;
			this.cmdHideAdd.Text = "+";
			this.cmdHideAdd.UseVisualStyleBackColor = true;
			this.cmdHideAdd.Click += new System.EventHandler(this.cmdHideAdd_Click);
			// 
			// cmdHideSubtract
			// 
			this.cmdHideSubtract.Location = new System.Drawing.Point(387, 35);
			this.cmdHideSubtract.Name = "cmdHideSubtract";
			this.cmdHideSubtract.Size = new System.Drawing.Size(20, 22);
			this.cmdHideSubtract.TabIndex = 43;
			this.cmdHideSubtract.Text = "-";
			this.cmdHideSubtract.UseVisualStyleBackColor = true;
			this.cmdHideSubtract.Click += new System.EventHandler(this.cmdHideSubtract_Click);
			// 
			// cboSnapUnit
			// 
			this.cboSnapUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSnapUnit.FormattingEnabled = true;
			this.cboSnapUnit.Items.AddRange(new object[] {
            "KM",
            "Raw"});
			this.cboSnapUnit.Location = new System.Drawing.Point(692, 37);
			this.cboSnapUnit.Name = "cboSnapUnit";
			this.cboSnapUnit.Size = new System.Drawing.Size(52, 21);
			this.cboSnapUnit.TabIndex = 38;
			this.cboSnapUnit.SelectedIndexChanged += new System.EventHandler(this.cboSnapUnit_SelectedIndexChanged);
			// 
			// lblSnapTo
			// 
			this.lblSnapTo.AutoSize = true;
			this.lblSnapTo.Location = new System.Drawing.Point(521, 40);
			this.lblSnapTo.Name = "lblSnapTo";
			this.lblSnapTo.Size = new System.Drawing.Size(47, 13);
			this.lblSnapTo.TabIndex = 47;
			this.lblSnapTo.Text = "Snap to:";
			// 
			// cboSnapTo
			// 
			this.cboSnapTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSnapTo.FormattingEnabled = true;
			this.cboSnapTo.Items.AddRange(new object[] {
            "None",
            "Self",
            "Grid"});
			this.cboSnapTo.Location = new System.Drawing.Point(574, 37);
			this.cboSnapTo.Name = "cboSnapTo";
			this.cboSnapTo.Size = new System.Drawing.Size(56, 21);
			this.cboSnapTo.TabIndex = 36;
			this.cboSnapTo.SelectedIndexChanged += new System.EventHandler(this.cboSnapTo_SelectedIndexChanged);
			// 
			// numSnapAmount
			// 
			this.numSnapAmount.DecimalPlaces = 2;
			this.numSnapAmount.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.numSnapAmount.Location = new System.Drawing.Point(636, 38);
			this.numSnapAmount.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            131072});
			this.numSnapAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.numSnapAmount.Name = "numSnapAmount";
			this.numSnapAmount.Size = new System.Drawing.Size(50, 20);
			this.numSnapAmount.TabIndex = 37;
			this.numSnapAmount.Value = new decimal(new int[] {
            10,
            0,
            0,
            131072});
			// 
			// lblExpandSelection
			// 
			this.lblExpandSelection.AutoSize = true;
			this.lblExpandSelection.Location = new System.Drawing.Point(51, 40);
			this.lblExpandSelection.Name = "lblExpandSelection";
			this.lblExpandSelection.Size = new System.Drawing.Size(60, 13);
			this.lblExpandSelection.TabIndex = 49;
			this.lblExpandSelection.Text = "Expand by:";
			// 
			// cmdExpandByCraft
			// 
			this.cmdExpandByCraft.Location = new System.Drawing.Point(113, 35);
			this.cmdExpandByCraft.Margin = new System.Windows.Forms.Padding(0);
			this.cmdExpandByCraft.Name = "cmdExpandByCraft";
			this.cmdExpandByCraft.Size = new System.Drawing.Size(39, 23);
			this.cmdExpandByCraft.TabIndex = 46;
			this.cmdExpandByCraft.Text = "Craft";
			this.cmdExpandByCraft.UseVisualStyleBackColor = true;
			this.cmdExpandByCraft.Click += new System.EventHandler(this.cmdExpandByCraft_Click);
			// 
			// cmdExpandByIff
			// 
			this.cmdExpandByIff.Location = new System.Drawing.Point(152, 35);
			this.cmdExpandByIff.Margin = new System.Windows.Forms.Padding(0);
			this.cmdExpandByIff.Name = "cmdExpandByIff";
			this.cmdExpandByIff.Size = new System.Drawing.Size(39, 23);
			this.cmdExpandByIff.TabIndex = 47;
			this.cmdExpandByIff.Text = "IFF";
			this.cmdExpandByIff.UseVisualStyleBackColor = true;
			this.cmdExpandByIff.Click += new System.EventHandler(this.cmdExpandByIff_Click);
			// 
			// cmdExpandBySize
			// 
			this.cmdExpandBySize.Location = new System.Drawing.Point(191, 35);
			this.cmdExpandBySize.Margin = new System.Windows.Forms.Padding(0);
			this.cmdExpandBySize.Name = "cmdExpandBySize";
			this.cmdExpandBySize.Size = new System.Drawing.Size(39, 23);
			this.cmdExpandBySize.TabIndex = 48;
			this.cmdExpandBySize.Text = "Size";
			this.cmdExpandBySize.UseVisualStyleBackColor = true;
			this.cmdExpandBySize.Click += new System.EventHandler(this.cmdExpandBySize_Click);
			// 
			// cmdInvertSelection
			// 
			this.cmdInvertSelection.Location = new System.Drawing.Point(2, 35);
			this.cmdInvertSelection.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
			this.cmdInvertSelection.Name = "cmdInvertSelection";
			this.cmdInvertSelection.Size = new System.Drawing.Size(48, 23);
			this.cmdInvertSelection.TabIndex = 45;
			this.cmdInvertSelection.Text = "Invert";
			this.cmdInvertSelection.UseVisualStyleBackColor = true;
			this.cmdInvertSelection.Click += new System.EventHandler(this.cmdInvertSelection_Click);
			// 
			// chkTime
			// 
			this.chkTime.Location = new System.Drawing.Point(750, 516);
			this.chkTime.Margin = new System.Windows.Forms.Padding(0);
			this.chkTime.Name = "chkTime";
			this.chkTime.Size = new System.Drawing.Size(68, 17);
			this.chkTime.TabIndex = 31;
			this.chkTime.Text = "Time";
			this.chkTime.UseVisualStyleBackColor = true;
			this.chkTime.CheckedChanged += new System.EventHandler(this.chkTime_CheckedChanged);
			// 
			// chkTraceHideFade
			// 
			this.chkTraceHideFade.Location = new System.Drawing.Point(658, 482);
			this.chkTraceHideFade.Margin = new System.Windows.Forms.Padding(0);
			this.chkTraceHideFade.Name = "chkTraceHideFade";
			this.chkTraceHideFade.Size = new System.Drawing.Size(92, 17);
			this.chkTraceHideFade.TabIndex = 32;
			this.chkTraceHideFade.Text = "Hide on fade";
			this.chkTraceHideFade.UseVisualStyleBackColor = true;
			this.chkTraceHideFade.CheckedChanged += new System.EventHandler(this.chkTraceHideFade_CheckedChanged);
			// 
			// chkTraceSelected
			// 
			this.chkTraceSelected.Location = new System.Drawing.Point(658, 499);
			this.chkTraceSelected.Margin = new System.Windows.Forms.Padding(0);
			this.chkTraceSelected.Name = "chkTraceSelected";
			this.chkTraceSelected.Size = new System.Drawing.Size(92, 17);
			this.chkTraceSelected.TabIndex = 33;
			this.chkTraceSelected.Text = "Selected only";
			this.chkTraceSelected.UseVisualStyleBackColor = true;
			this.chkTraceSelected.CheckedChanged += new System.EventHandler(this.chkTraceSelected_CheckedChanged);
			// 
			// cmdFitSelected
			// 
			this.cmdFitSelected.Location = new System.Drawing.Point(303, 478);
			this.cmdFitSelected.Name = "cmdFitSelected";
			this.cmdFitSelected.Size = new System.Drawing.Size(60, 23);
			this.cmdFitSelected.TabIndex = 55;
			this.cmdFitSelected.Text = "Selected";
			this.cmdFitSelected.UseVisualStyleBackColor = true;
			this.cmdFitSelected.Click += new System.EventHandler(this.cmdFitSelected_Click);
			// 
			// lblFit
			// 
			this.lblFit.AutoSize = true;
			this.lblFit.Location = new System.Drawing.Point(267, 483);
			this.lblFit.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
			this.lblFit.Name = "lblFit";
			this.lblFit.Size = new System.Drawing.Size(33, 13);
			this.lblFit.TabIndex = 54;
			this.lblFit.Text = "Fit to:";
			// 
			// cmdFitWorld
			// 
			this.cmdFitWorld.Location = new System.Drawing.Point(369, 478);
			this.cmdFitWorld.Name = "cmdFitWorld";
			this.cmdFitWorld.Size = new System.Drawing.Size(60, 23);
			this.cmdFitWorld.TabIndex = 56;
			this.cmdFitWorld.Text = "World";
			this.cmdFitWorld.UseVisualStyleBackColor = true;
			this.cmdFitWorld.Click += new System.EventHandler(this.cmdFitWorld_Click);
			// 
			// cmdFitDefault
			// 
			this.cmdFitDefault.Location = new System.Drawing.Point(435, 478);
			this.cmdFitDefault.Name = "cmdFitDefault";
			this.cmdFitDefault.Size = new System.Drawing.Size(60, 23);
			this.cmdFitDefault.TabIndex = 57;
			this.cmdFitDefault.Text = "Default";
			this.cmdFitDefault.UseVisualStyleBackColor = true;
			this.cmdFitDefault.Click += new System.EventHandler(this.cmdFitDefault_Click);
			// 
			// lblCenterOn
			// 
			this.lblCenterOn.AutoSize = true;
			this.lblCenterOn.Location = new System.Drawing.Point(137, 483);
			this.lblCenterOn.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblCenterOn.Name = "lblCenterOn";
			this.lblCenterOn.Size = new System.Drawing.Size(56, 13);
			this.lblCenterOn.TabIndex = 56;
			this.lblCenterOn.Text = "Center on:";
			// 
			// cmdCenterSelected
			// 
			this.cmdCenterSelected.Location = new System.Drawing.Point(196, 478);
			this.cmdCenterSelected.Name = "cmdCenterSelected";
			this.cmdCenterSelected.Size = new System.Drawing.Size(60, 23);
			this.cmdCenterSelected.TabIndex = 54;
			this.cmdCenterSelected.Text = "Selected";
			this.cmdCenterSelected.UseVisualStyleBackColor = true;
			this.cmdCenterSelected.Click += new System.EventHandler(this.cmdCenterSelected_Click);
			// 
			// cboViewDifficulty
			// 
			this.cboViewDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboViewDifficulty.FormattingEnabled = true;
			this.cboViewDifficulty.Items.AddRange(new object[] {
            "All Diff",
            "Easy",
            "Medium",
            "Hard"});
			this.cboViewDifficulty.Location = new System.Drawing.Point(2, 480);
			this.cboViewDifficulty.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
			this.cboViewDifficulty.Name = "cboViewDifficulty";
			this.cboViewDifficulty.Size = new System.Drawing.Size(61, 21);
			this.cboViewDifficulty.TabIndex = 52;
			this.cboViewDifficulty.SelectedIndexChanged += new System.EventHandler(this.cboViewDifficulty_SelectedIndexChanged);
			// 
			// cboViewIff
			// 
			this.cboViewIff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboViewIff.FormattingEnabled = true;
			this.cboViewIff.Items.AddRange(new object[] {
            "All IFF"});
			this.cboViewIff.Location = new System.Drawing.Point(65, 480);
			this.cboViewIff.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
			this.cboViewIff.Name = "cboViewIff";
			this.cboViewIff.Size = new System.Drawing.Size(61, 21);
			this.cboViewIff.TabIndex = 53;
			this.cboViewIff.SelectedIndexChanged += new System.EventHandler(this.cboViewIff_SelectedIndexChanged);
			// 
			// chkWireframe
			// 
			this.chkWireframe.AutoSize = true;
			this.chkWireframe.Checked = true;
			this.chkWireframe.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWireframe.Location = new System.Drawing.Point(54, 6);
			this.chkWireframe.Name = "chkWireframe";
			this.chkWireframe.Size = new System.Drawing.Size(79, 17);
			this.chkWireframe.TabIndex = 59;
			this.chkWireframe.Text = "Wireframes";
			this.chkWireframe.UseVisualStyleBackColor = true;
			this.chkWireframe.CheckedChanged += new System.EventHandler(this.chkWireframe_CheckedChanged);
			// 
			// chkLimit
			// 
			this.chkLimit.AutoSize = true;
			this.chkLimit.Location = new System.Drawing.Point(139, 6);
			this.chkLimit.Name = "chkLimit";
			this.chkLimit.Size = new System.Drawing.Size(105, 17);
			this.chkLimit.TabIndex = 60;
			this.chkLimit.Text = "Only above XXm";
			this.chkLimit.UseVisualStyleBackColor = true;
			this.chkLimit.CheckedChanged += new System.EventHandler(this.chkLimit_CheckedChanged);
			// 
			// chkCumulative
			// 
			this.chkCumulative.Enabled = false;
			this.chkCumulative.Location = new System.Drawing.Point(658, 516);
			this.chkCumulative.Margin = new System.Windows.Forms.Padding(0);
			this.chkCumulative.Name = "chkCumulative";
			this.chkCumulative.Size = new System.Drawing.Size(92, 17);
			this.chkCumulative.TabIndex = 31;
			this.chkCumulative.Text = "Cumulative";
			this.chkCumulative.UseVisualStyleBackColor = true;
			this.chkCumulative.CheckedChanged += new System.EventHandler(this.chkCumulative_CheckedChanged);
			// 
			// MapForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(824, 535);
			this.Controls.Add(this.chkLimit);
			this.Controls.Add(this.cboViewIff);
			this.Controls.Add(this.cboViewDifficulty);
			this.Controls.Add(this.cmdCenterSelected);
			this.Controls.Add(this.lblCenterOn);
			this.Controls.Add(this.cmdFitDefault);
			this.Controls.Add(this.lblFit);
			this.Controls.Add(this.cmdFitWorld);
			this.Controls.Add(this.cmdFitSelected);
			this.Controls.Add(this.chkTraceSelected);
			this.Controls.Add(this.chkTraceHideFade);
			this.Controls.Add(this.chkCumulative);
			this.Controls.Add(this.chkTime);
			this.Controls.Add(this.cmdInvertSelection);
			this.Controls.Add(this.cmdExpandBySize);
			this.Controls.Add(this.grpPoints);
			this.Controls.Add(this.cmdExpandByIff);
			this.Controls.Add(this.cmdExpandByCraft);
			this.Controls.Add(this.lblExpandSelection);
			this.Controls.Add(this.numSnapAmount);
			this.Controls.Add(this.lblSnapTo);
			this.Controls.Add(this.cboSnapTo);
			this.Controls.Add(this.cboSnapUnit);
			this.Controls.Add(this.cmdFadeNone);
			this.Controls.Add(this.cmdHideSubtract);
			this.Controls.Add(this.cmdFadeSubtract);
			this.Controls.Add(this.cmdHideAdd);
			this.Controls.Add(this.cmdFadeAdd);
			this.Controls.Add(this.lblHide);
			this.Controls.Add(this.lblFade);
			this.Controls.Add(this.chkDistance);
			this.Controls.Add(this.cmdHelp);
			this.Controls.Add(this.lblSelection);
			this.Controls.Add(this.lstSelection);
			this.Controls.Add(this.cmdHideNone);
			this.Controls.Add(this.lstCraft);
			this.Controls.Add(this.numOrder);
			this.Controls.Add(this.numRegion);
			this.Controls.Add(this.lblOrder);
			this.Controls.Add(this.lblRegion);
			this.Controls.Add(this.chkTags);
			this.Controls.Add(this.grpDir);
			this.Controls.Add(this.lblZoom);
			this.Controls.Add(this.lblCoor2);
			this.Controls.Add(this.lblCoor1);
			this.Controls.Add(this.hscZoom);
			this.Controls.Add(this.pctMap);
			this.Controls.Add(this.chkTrace);
			this.Controls.Add(this.chkWireframe);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(750, 574);
			this.Name = "MapForm";
			this.Text = "YOGEME Map Interface";
			this.Activated += new System.EventHandler(this.form_Activated);
			this.Deactivate += new System.EventHandler(this.form_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.form_FormClosed);
			this.Load += new System.EventHandler(this.form_Load);
			this.ResizeBegin += new System.EventHandler(this.form_ResizeBegin);
			this.ResizeEnd += new System.EventHandler(this.form_ResizeEnd);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.form_KeyUp);
			this.Resize += new System.EventHandler(this.form_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pctMap)).EndInit();
			this.grpDir.ResumeLayout(false);
			this.grpPoints.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numRegion)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numSnapAmount)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		PictureBox pctMap;
		HScrollBar hscZoom;
		Label lblCoor1;
		Label lblCoor2;
		Label lblZoom;
		RadioButton optYZ;
		GroupBox grpDir;
		RadioButton optXZ;
		RadioButton optXY;
		ImageList imgCraft;
		GroupBox grpPoints;
		CheckBox chkSP1;
		CheckBox chkSP2;
		CheckBox chkSP3;
		CheckBox chkSP4;
		CheckBox chkWP1;
		CheckBox chkWP2;
		CheckBox chkWP3;
		CheckBox chkWP4;
		CheckBox chkWP5;
		CheckBox chkWP6;
		CheckBox chkWP7;
		CheckBox chkWP8;
		CheckBox chkRDV;
		CheckBox chkHYP;
		CheckBox chkBRF;
		CheckBox chkBRF5;
		CheckBox chkBRF4;
		CheckBox chkBRF3;
		CheckBox chkBRF2;
		CheckBox chkBRF8;
		CheckBox chkBRF7;
		CheckBox chkBRF6;
		CheckBox chkTags;
		CheckBox chkTrace;
		private Label lblRegion;
		private NumericUpDown numRegion;
		private Label lblOrder;
		private NumericUpDown numOrder;
        private ListBox lstCraft;
        private Button cmdHideNone;
        private ListBox lstSelection;
        private Label lblSelection;
        private Button cmdHelp;
        private CheckBox chkDistance;
		private Timer mapPaintRedrawTimer;
		private Label lblFade;
		private Button cmdFadeAdd;
		private Button cmdFadeNone;
		private Button cmdFadeSubtract;
		private Label lblHide;
		private Button cmdHideAdd;
		private Button cmdHideSubtract;
		private ComboBox cboSnapUnit;
		private Label lblSnapTo;
		private ComboBox cboSnapTo;
		private NumericUpDown numSnapAmount;
		private Label lblExpandSelection;
		private Button cmdExpandByCraft;
		private Button cmdExpandByIff;
		private Button cmdExpandBySize;
		private Button cmdInvertSelection;
		private CheckBox chkTime;
		private CheckBox chkTraceHideFade;
		private CheckBox chkTraceSelected;
		private Button cmdFitSelected;
		private Label lblFit;
		private Button cmdFitWorld;
		private Button cmdFitDefault;
		private Label lblCenterOn;
		private Button cmdCenterSelected;
		private ComboBox cboViewDifficulty;
		private ComboBox cboViewIff;
		private CheckBox chkWireframe;
		private CheckBox chkLimit;
		private CheckBox chkCumulative;
	}
}