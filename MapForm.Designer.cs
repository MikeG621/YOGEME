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
            this.label1 = new System.Windows.Forms.Label();
            this.lstVisible = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdHideNone = new System.Windows.Forms.Button();
            this.cmdHideAll = new System.Windows.Forms.Button();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.lblQuickHide = new System.Windows.Forms.Label();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.chkDistance = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pctMap)).BeginInit();
            this.grpDir.SuspendLayout();
            this.grpPoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRegion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // pctMap
            // 
            this.pctMap.BackColor = System.Drawing.Color.Black;
            this.pctMap.Location = new System.Drawing.Point(102, 64);
            this.pctMap.Name = "pctMap";
            this.pctMap.Size = new System.Drawing.Size(642, 408);
            this.pctMap.TabIndex = 28;
            this.pctMap.TabStop = false;
            this.pctMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pctMap_Paint);
            this.pctMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctMap_MouseDown);
            this.pctMap.MouseEnter += new System.EventHandler(this.pctMap_MouseEnter);
            this.pctMap.MouseLeave += new System.EventHandler(this.pctMap_MouseLeave);
            this.pctMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctMap_MouseMove);
            this.pctMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctMap_MouseUp);
            // 
            // hscZoom
            // 
            this.hscZoom.Location = new System.Drawing.Point(402, 504);
            this.hscZoom.Maximum = 500;
            this.hscZoom.Minimum = 5;
            this.hscZoom.Name = "hscZoom";
            this.hscZoom.Size = new System.Drawing.Size(342, 16);
            this.hscZoom.TabIndex = 30;
            this.hscZoom.Value = 40;
            this.hscZoom.ValueChanged += new System.EventHandler(this.hscZoom_ValueChanged);
            // 
            // lblCoor1
            // 
            this.lblCoor1.Location = new System.Drawing.Point(24, 504);
            this.lblCoor1.Name = "lblCoor1";
            this.lblCoor1.Size = new System.Drawing.Size(72, 16);
            this.lblCoor1.TabIndex = 27;
            this.lblCoor1.Text = "X:";
            // 
            // lblCoor2
            // 
            this.lblCoor2.Location = new System.Drawing.Point(120, 504);
            this.lblCoor2.Name = "lblCoor2";
            this.lblCoor2.Size = new System.Drawing.Size(96, 16);
            this.lblCoor2.TabIndex = 26;
            this.lblCoor2.Text = "Y:";
            // 
            // lblZoom
            // 
            this.lblZoom.Location = new System.Drawing.Point(322, 504);
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
            this.grpDir.Location = new System.Drawing.Point(750, 12);
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
            this.grpPoints.Location = new System.Drawing.Point(750, 100);
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
            this.chkTags.Location = new System.Drawing.Point(750, 468);
            this.chkTags.Name = "chkTags";
            this.chkTags.Size = new System.Drawing.Size(72, 24);
            this.chkTags.TabIndex = 28;
            this.chkTags.Text = "FG Tags";
            this.chkTags.CheckedChanged += new System.EventHandler(this.chkTags_CheckedChanged);
            // 
            // chkTrace
            // 
            this.chkTrace.Checked = true;
            this.chkTrace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrace.Location = new System.Drawing.Point(750, 487);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(72, 24);
            this.chkTrace.TabIndex = 29;
            this.chkTrace.Text = "Traces";
            this.chkTrace.CheckedChanged += new System.EventHandler(this.chkTrace_CheckedChanged);
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(572, 28);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(44, 13);
            this.lblRegion.TabIndex = 31;
            this.lblRegion.Text = "Region:";
            this.lblRegion.Visible = false;
            // 
            // numRegion
            // 
            this.numRegion.Location = new System.Drawing.Point(622, 26);
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
            this.numRegion.TabIndex = 32;
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
            this.lblOrder.Location = new System.Drawing.Point(669, 28);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(36, 13);
            this.lblOrder.TabIndex = 31;
            this.lblOrder.Text = "Order:";
            this.lblOrder.Visible = false;
            // 
            // numOrder
            // 
            this.numOrder.Location = new System.Drawing.Point(711, 26);
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
            this.numOrder.TabIndex = 32;
            this.numOrder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numOrder.Visible = false;
            this.numOrder.ValueChanged += new System.EventHandler(this.numOrder_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(47, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 27);
            this.label1.TabIndex = 33;
            this.label1.Text = "Click to view mouse and keyboard commands to edit and navigate the map.";
            // 
            // lstVisible
            // 
            this.lstVisible.BackColor = System.Drawing.Color.Black;
            this.lstVisible.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstVisible.ForeColor = System.Drawing.Color.Gray;
            this.lstVisible.FormattingEnabled = true;
            this.lstVisible.Location = new System.Drawing.Point(2, 116);
            this.lstVisible.Name = "lstVisible";
            this.lstVisible.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstVisible.Size = new System.Drawing.Size(94, 355);
            this.lstVisible.TabIndex = 34;
            this.lstVisible.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstVisible_DrawItem);
            this.lstVisible.SelectedIndexChanged += new System.EventHandler(this.lstVisible_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Hide:";
            // 
            // cmdHideNone
            // 
            this.cmdHideNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdHideNone.Location = new System.Drawing.Point(2, 92);
            this.cmdHideNone.Name = "cmdHideNone";
            this.cmdHideNone.Size = new System.Drawing.Size(42, 23);
            this.cmdHideNone.TabIndex = 37;
            this.cmdHideNone.Text = "None";
            this.cmdHideNone.UseVisualStyleBackColor = true;
            this.cmdHideNone.Click += new System.EventHandler(this.cmdHideNone_Click);
            // 
            // cmdHideAll
            // 
            this.cmdHideAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdHideAll.Location = new System.Drawing.Point(50, 92);
            this.cmdHideAll.Name = "cmdHideAll";
            this.cmdHideAll.Size = new System.Drawing.Size(42, 23);
            this.cmdHideAll.TabIndex = 37;
            this.cmdHideAll.Text = "All";
            this.cmdHideAll.UseVisualStyleBackColor = true;
            this.cmdHideAll.Click += new System.EventHandler(this.cmdHideAll_Click);
            // 
            // lstSelected
            // 
            this.lstSelected.BackColor = System.Drawing.Color.Black;
            this.lstSelected.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstSelected.ForeColor = System.Drawing.Color.Gray;
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.Location = new System.Drawing.Point(102, 76);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstSelected.Size = new System.Drawing.Size(94, 17);
            this.lstSelected.TabIndex = 38;
            this.lstSelected.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstSelected_DrawItem);
            this.lstSelected.SelectedIndexChanged += new System.EventHandler(this.lstSelected_SelectedIndexChanged);
            // 
            // lblQuickHide
            // 
            this.lblQuickHide.AutoSize = true;
            this.lblQuickHide.Location = new System.Drawing.Point(99, 64);
            this.lblQuickHide.Name = "lblQuickHide";
            this.lblQuickHide.Size = new System.Drawing.Size(79, 13);
            this.lblQuickHide.TabIndex = 39;
            this.lblQuickHide.Text = "Hide Selection:";
            // 
            // cmdHelp
            // 
            this.cmdHelp.Location = new System.Drawing.Point(2, 9);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(42, 23);
            this.cmdHelp.TabIndex = 40;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // chkDistance
            // 
            this.chkDistance.AutoSize = true;
            this.chkDistance.Location = new System.Drawing.Point(750, 506);
            this.chkDistance.Name = "chkDistance";
            this.chkDistance.Size = new System.Drawing.Size(68, 17);
            this.chkDistance.TabIndex = 41;
            this.chkDistance.Text = "Distance";
            this.chkDistance.UseVisualStyleBackColor = true;
            this.chkDistance.CheckedChanged += new System.EventHandler(this.chkDistance_CheckedChanged);
            // 
            // MapForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(824, 535);
            this.Controls.Add(this.chkDistance);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.lblQuickHide);
            this.Controls.Add(this.lstSelected);
            this.Controls.Add(this.cmdHideAll);
            this.Controls.Add(this.cmdHideNone);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstVisible);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numOrder);
            this.Controls.Add(this.numRegion);
            this.Controls.Add(this.lblOrder);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.chkTags);
            this.Controls.Add(this.grpPoints);
            this.Controls.Add(this.grpDir);
            this.Controls.Add(this.lblZoom);
            this.Controls.Add(this.lblCoor2);
            this.Controls.Add(this.lblCoor1);
            this.Controls.Add(this.hscZoom);
            this.Controls.Add(this.pctMap);
            this.Controls.Add(this.chkTrace);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 563);
            this.Name = "MapForm";
            this.Text = "YOGEME Map Interface";
            this.Activated += new System.EventHandler(this.frmMap_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMap_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMap_FormClosed);
            this.Load += new System.EventHandler(this.frmMap_Load);
            this.ResizeBegin += new System.EventHandler(this.MapForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MapForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MapForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pctMap)).EndInit();
            this.grpDir.ResumeLayout(false);
            this.grpPoints.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRegion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
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
		private Label label1;
        private ListBox lstVisible;
        private Label label3;
        private Button cmdHideNone;
        private Button cmdHideAll;
        private ListBox lstSelected;
        private Label lblQuickHide;
        private Button cmdHelp;
        private CheckBox chkDistance;
	}
}