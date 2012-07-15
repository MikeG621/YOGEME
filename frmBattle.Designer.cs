using System.Windows.Forms;

namespace Idmr.Yogeme
{
	partial class frmBattle
	{
		System.ComponentModel.IContainer components = null;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBattle));
			this.cmdPrev = new System.Windows.Forms.Button();
			this.cmdNext = new System.Windows.Forms.Button();
			this.lblBattle = new System.Windows.Forms.Label();
			this.tcBattle = new System.Windows.Forms.TabControl();
			this.tabBattle = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.txtBattle = new System.Windows.Forms.TextBox();
			this.txtBTitle1 = new System.Windows.Forms.TextBox();
			this.txtBTitle2 = new System.Windows.Forms.TextBox();
			this.txtCTitle1 = new System.Windows.Forms.TextBox();
			this.txtCTitle2 = new System.Windows.Forms.TextBox();
			this.txtSystem = new System.Windows.Forms.TextBox();
			this.txtCutscene = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tabMission = new System.Windows.Forms.TabPage();
			this.cmdMoveUp = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.cmdAdd = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.lstMiss = new System.Windows.Forms.ListBox();
			this.txtDesc = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmdRemove = new System.Windows.Forms.Button();
			this.cmdMoveDown = new System.Windows.Forms.Button();
			this.tabBitmap = new System.Windows.Forms.TabPage();
			this.picGalaxy = new System.Windows.Forms.PictureBox();
			this.grpFrame = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.numFrameTop = new System.Windows.Forms.NumericUpDown();
			this.numFrameLeft = new System.Windows.Forms.NumericUpDown();
			this.numFrameWidth = new System.Windows.Forms.NumericUpDown();
			this.numFrameHeight = new System.Windows.Forms.NumericUpDown();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.tabSystem = new System.Windows.Forms.TabPage();
			this.label14 = new System.Windows.Forms.Label();
			this.cmdExport = new System.Windows.Forms.Button();
			this.picSystem = new System.Windows.Forms.PictureBox();
			this.cmdImport = new System.Windows.Forms.Button();
			this.cmdSave = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.opnSystem = new System.Windows.Forms.OpenFileDialog();
			this.savSystem = new System.Windows.Forms.SaveFileDialog();
			this.opnMission = new System.Windows.Forms.OpenFileDialog();
			this.tcBattle.SuspendLayout();
			this.tabBattle.SuspendLayout();
			this.tabMission.SuspendLayout();
			this.tabBitmap.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picGalaxy)).BeginInit();
			this.grpFrame.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFrameTop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFrameLeft)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFrameWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFrameHeight)).BeginInit();
			this.tabSystem.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSystem)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdPrev
			// 
			this.cmdPrev.Location = new System.Drawing.Point(8, 16);
			this.cmdPrev.Name = "cmdPrev";
			this.cmdPrev.Size = new System.Drawing.Size(64, 24);
			this.cmdPrev.TabIndex = 10;
			this.cmdPrev.Text = "&Previous";
			this.cmdPrev.Click += new System.EventHandler(this.cmdPrev_Click);
			// 
			// cmdNext
			// 
			this.cmdNext.Location = new System.Drawing.Point(152, 16);
			this.cmdNext.Name = "cmdNext";
			this.cmdNext.Size = new System.Drawing.Size(64, 24);
			this.cmdNext.TabIndex = 9;
			this.cmdNext.Text = "&Next";
			this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
			// 
			// lblBattle
			// 
			this.lblBattle.Location = new System.Drawing.Point(80, 16);
			this.lblBattle.Name = "lblBattle";
			this.lblBattle.Size = new System.Drawing.Size(64, 24);
			this.lblBattle.TabIndex = 1;
			this.lblBattle.Text = "Battle1";
			this.lblBattle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tcBattle
			// 
			this.tcBattle.Controls.Add(this.tabBattle);
			this.tcBattle.Controls.Add(this.tabMission);
			this.tcBattle.Controls.Add(this.tabBitmap);
			this.tcBattle.Controls.Add(this.tabSystem);
			this.tcBattle.Location = new System.Drawing.Point(0, 48);
			this.tcBattle.Name = "tcBattle";
			this.tcBattle.SelectedIndex = 0;
			this.tcBattle.Size = new System.Drawing.Size(560, 256);
			this.tcBattle.TabIndex = 7;
			this.tcBattle.SelectedIndexChanged += new System.EventHandler(this.tcBattle_SelectedIndexChanged);
			// 
			// tabBattle
			// 
			this.tabBattle.Controls.Add(this.label1);
			this.tabBattle.Controls.Add(this.txtBattle);
			this.tabBattle.Controls.Add(this.txtBTitle1);
			this.tabBattle.Controls.Add(this.txtBTitle2);
			this.tabBattle.Controls.Add(this.txtCTitle1);
			this.tabBattle.Controls.Add(this.txtCTitle2);
			this.tabBattle.Controls.Add(this.txtSystem);
			this.tabBattle.Controls.Add(this.txtCutscene);
			this.tabBattle.Controls.Add(this.label2);
			this.tabBattle.Controls.Add(this.label3);
			this.tabBattle.Controls.Add(this.label4);
			this.tabBattle.Controls.Add(this.label5);
			this.tabBattle.Location = new System.Drawing.Point(4, 22);
			this.tabBattle.Name = "tabBattle";
			this.tabBattle.Size = new System.Drawing.Size(552, 230);
			this.tabBattle.TabIndex = 0;
			this.tabBattle.Text = "Battle";
			this.tabBattle.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Battle Title";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// txtBattle
			// 
			this.txtBattle.Location = new System.Drawing.Point(128, 24);
			this.txtBattle.Name = "txtBattle";
			this.txtBattle.Size = new System.Drawing.Size(352, 20);
			this.txtBattle.TabIndex = 0;
			// 
			// txtBTitle1
			// 
			this.txtBTitle1.Location = new System.Drawing.Point(128, 88);
			this.txtBTitle1.Name = "txtBTitle1";
			this.txtBTitle1.Size = new System.Drawing.Size(352, 20);
			this.txtBTitle1.TabIndex = 2;
			// 
			// txtBTitle2
			// 
			this.txtBTitle2.Location = new System.Drawing.Point(128, 112);
			this.txtBTitle2.Name = "txtBTitle2";
			this.txtBTitle2.Size = new System.Drawing.Size(352, 20);
			this.txtBTitle2.TabIndex = 3;
			// 
			// txtCTitle1
			// 
			this.txtCTitle1.Location = new System.Drawing.Point(128, 144);
			this.txtCTitle1.Name = "txtCTitle1";
			this.txtCTitle1.Size = new System.Drawing.Size(352, 20);
			this.txtCTitle1.TabIndex = 4;
			// 
			// txtCTitle2
			// 
			this.txtCTitle2.Location = new System.Drawing.Point(128, 168);
			this.txtCTitle2.Name = "txtCTitle2";
			this.txtCTitle2.Size = new System.Drawing.Size(352, 20);
			this.txtCTitle2.TabIndex = 5;
			// 
			// txtSystem
			// 
			this.txtSystem.Location = new System.Drawing.Point(128, 200);
			this.txtSystem.Name = "txtSystem";
			this.txtSystem.Size = new System.Drawing.Size(88, 20);
			this.txtSystem.TabIndex = 6;
			// 
			// txtCutscene
			// 
			this.txtCutscene.Location = new System.Drawing.Point(128, 56);
			this.txtCutscene.Name = "txtCutscene";
			this.txtCutscene.Size = new System.Drawing.Size(352, 20);
			this.txtCutscene.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Cutscene Title";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 1;
			this.label3.Text = "Battle Text";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 144);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Cutscene Text";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 200);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 1;
			this.label5.Text = "System";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tabMission
			// 
			this.tabMission.Controls.Add(this.cmdMoveUp);
			this.tabMission.Controls.Add(this.label8);
			this.tabMission.Controls.Add(this.cmdAdd);
			this.tabMission.Controls.Add(this.label6);
			this.tabMission.Controls.Add(this.lstMiss);
			this.tabMission.Controls.Add(this.txtDesc);
			this.tabMission.Controls.Add(this.label7);
			this.tabMission.Controls.Add(this.cmdRemove);
			this.tabMission.Controls.Add(this.cmdMoveDown);
			this.tabMission.Location = new System.Drawing.Point(4, 22);
			this.tabMission.Name = "tabMission";
			this.tabMission.Size = new System.Drawing.Size(552, 230);
			this.tabMission.TabIndex = 1;
			this.tabMission.Text = "Missions";
			this.tabMission.UseVisualStyleBackColor = true;
			// 
			// cmdMoveUp
			// 
			this.cmdMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("cmdMoveUp.Image")));
			this.cmdMoveUp.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdMoveUp.Location = new System.Drawing.Point(56, 176);
			this.cmdMoveUp.Name = "cmdMoveUp";
			this.cmdMoveUp.Size = new System.Drawing.Size(24, 24);
			this.cmdMoveUp.TabIndex = 5;
			this.cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(432, 48);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(80, 96);
			this.label8.TabIndex = 4;
			this.label8.Text = "Line breaks are required, do not rely on word wrap for this text box";
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(176, 72);
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size(64, 24);
			this.cmdAdd.TabIndex = 0;
			this.cmdAdd.Text = "&Add...";
			this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(24, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 16);
			this.label6.TabIndex = 2;
			this.label6.Text = "Missions";
			// 
			// lstMiss
			// 
			this.lstMiss.Location = new System.Drawing.Point(24, 48);
			this.lstMiss.Name = "lstMiss";
			this.lstMiss.Size = new System.Drawing.Size(136, 108);
			this.lstMiss.TabIndex = 2;
			this.lstMiss.SelectedIndexChanged += new System.EventHandler(this.lstMiss_SelectedIndexChanged);
			// 
			// txtDesc
			// 
			this.txtDesc.AcceptsReturn = true;
			this.txtDesc.Location = new System.Drawing.Point(256, 48);
			this.txtDesc.Multiline = true;
			this.txtDesc.Name = "txtDesc";
			this.txtDesc.Size = new System.Drawing.Size(176, 168);
			this.txtDesc.TabIndex = 3;
			this.txtDesc.TextChanged += new System.EventHandler(this.txtDesc_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(264, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 16);
			this.label7.TabIndex = 2;
			this.label7.Text = "Description";
			// 
			// cmdRemove
			// 
			this.cmdRemove.Location = new System.Drawing.Point(176, 120);
			this.cmdRemove.Name = "cmdRemove";
			this.cmdRemove.Size = new System.Drawing.Size(64, 24);
			this.cmdRemove.TabIndex = 1;
			this.cmdRemove.Text = "&Remove";
			this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
			// 
			// cmdMoveDown
			// 
			this.cmdMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("cmdMoveDown.Image")));
			this.cmdMoveDown.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.cmdMoveDown.Location = new System.Drawing.Point(104, 176);
			this.cmdMoveDown.Name = "cmdMoveDown";
			this.cmdMoveDown.Size = new System.Drawing.Size(24, 24);
			this.cmdMoveDown.TabIndex = 4;
			this.cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
			// 
			// tabBitmap
			// 
			this.tabBitmap.Controls.Add(this.picGalaxy);
			this.tabBitmap.Controls.Add(this.grpFrame);
			this.tabBitmap.Location = new System.Drawing.Point(4, 22);
			this.tabBitmap.Name = "tabBitmap";
			this.tabBitmap.Size = new System.Drawing.Size(552, 230);
			this.tabBitmap.TabIndex = 2;
			this.tabBitmap.Text = "Galaxy";
			this.tabBitmap.UseVisualStyleBackColor = true;
			// 
			// picGalaxy
			// 
			this.picGalaxy.Location = new System.Drawing.Point(152, 24);
			this.picGalaxy.Name = "picGalaxy";
			this.picGalaxy.Size = new System.Drawing.Size(344, 152);
			this.picGalaxy.TabIndex = 2;
			this.picGalaxy.TabStop = false;
			this.picGalaxy.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picGalaxy_MouseMove);
			this.picGalaxy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picGalaxy_MouseDown);
			this.picGalaxy.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picGalaxy_MouseUp);
			// 
			// grpFrame
			// 
			this.grpFrame.Controls.Add(this.label10);
			this.grpFrame.Controls.Add(this.numFrameTop);
			this.grpFrame.Controls.Add(this.numFrameLeft);
			this.grpFrame.Controls.Add(this.numFrameWidth);
			this.grpFrame.Controls.Add(this.numFrameHeight);
			this.grpFrame.Controls.Add(this.label11);
			this.grpFrame.Controls.Add(this.label12);
			this.grpFrame.Controls.Add(this.label13);
			this.grpFrame.Location = new System.Drawing.Point(8, 16);
			this.grpFrame.Name = "grpFrame";
			this.grpFrame.Size = new System.Drawing.Size(120, 152);
			this.grpFrame.TabIndex = 1;
			this.grpFrame.TabStop = false;
			this.grpFrame.Text = "Frame";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(40, 16);
			this.label10.TabIndex = 1;
			this.label10.Text = "Top";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// numFrameTop
			// 
			this.numFrameTop.Location = new System.Drawing.Point(56, 24);
			this.numFrameTop.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numFrameTop.Name = "numFrameTop";
			this.numFrameTop.Size = new System.Drawing.Size(48, 20);
			this.numFrameTop.TabIndex = 0;
			this.numFrameTop.ValueChanged += new System.EventHandler(this.numFrameTop_ValueChanged);
			// 
			// numFrameLeft
			// 
			this.numFrameLeft.Location = new System.Drawing.Point(56, 56);
			this.numFrameLeft.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numFrameLeft.Name = "numFrameLeft";
			this.numFrameLeft.Size = new System.Drawing.Size(48, 20);
			this.numFrameLeft.TabIndex = 1;
			this.numFrameLeft.ValueChanged += new System.EventHandler(this.numFrameLeft_ValueChanged);
			// 
			// numFrameWidth
			// 
			this.numFrameWidth.Location = new System.Drawing.Point(56, 120);
			this.numFrameWidth.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numFrameWidth.Name = "numFrameWidth";
			this.numFrameWidth.Size = new System.Drawing.Size(48, 20);
			this.numFrameWidth.TabIndex = 3;
			this.numFrameWidth.ValueChanged += new System.EventHandler(this.numFrameWidth_ValueChanged);
			// 
			// numFrameHeight
			// 
			this.numFrameHeight.Location = new System.Drawing.Point(56, 88);
			this.numFrameHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numFrameHeight.Name = "numFrameHeight";
			this.numFrameHeight.Size = new System.Drawing.Size(48, 20);
			this.numFrameHeight.TabIndex = 2;
			this.numFrameHeight.ValueChanged += new System.EventHandler(this.numFrameHeight_ValueChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 56);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(40, 16);
			this.label11.TabIndex = 1;
			this.label11.Text = "Left";
			this.label11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 88);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(40, 16);
			this.label12.TabIndex = 1;
			this.label12.Text = "Height";
			this.label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 120);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(40, 16);
			this.label13.TabIndex = 1;
			this.label13.Text = "Width";
			this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tabSystem
			// 
			this.tabSystem.Controls.Add(this.label14);
			this.tabSystem.Controls.Add(this.cmdExport);
			this.tabSystem.Controls.Add(this.picSystem);
			this.tabSystem.Controls.Add(this.cmdImport);
			this.tabSystem.Location = new System.Drawing.Point(4, 22);
			this.tabSystem.Name = "tabSystem";
			this.tabSystem.Size = new System.Drawing.Size(552, 230);
			this.tabSystem.TabIndex = 3;
			this.tabSystem.Text = "System";
			this.tabSystem.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 96);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(176, 128);
			this.label14.TabIndex = 2;
			this.label14.Text = "Images must be 256 color indexed bitmaps.  Importing images with palettes other t" +
				"han the one exported will produce... interesting results.";
			// 
			// cmdExport
			// 
			this.cmdExport.Location = new System.Drawing.Point(32, 32);
			this.cmdExport.Name = "cmdExport";
			this.cmdExport.Size = new System.Drawing.Size(75, 23);
			this.cmdExport.TabIndex = 0;
			this.cmdExport.Text = "&Export";
			this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
			// 
			// picSystem
			// 
			this.picSystem.Location = new System.Drawing.Point(208, 24);
			this.picSystem.Name = "picSystem";
			this.picSystem.Size = new System.Drawing.Size(288, 176);
			this.picSystem.TabIndex = 0;
			this.picSystem.TabStop = false;
			// 
			// cmdImport
			// 
			this.cmdImport.Location = new System.Drawing.Point(32, 64);
			this.cmdImport.Name = "cmdImport";
			this.cmdImport.Size = new System.Drawing.Size(75, 23);
			this.cmdImport.TabIndex = 1;
			this.cmdImport.Text = "&Import";
			this.cmdImport.Click += new System.EventHandler(this.cmdImport_Click);
			// 
			// cmdSave
			// 
			this.cmdSave.Location = new System.Drawing.Point(272, 16);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(64, 24);
			this.cmdSave.TabIndex = 8;
			this.cmdSave.Text = "&Save";
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(352, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(168, 32);
			this.label9.TabIndex = 4;
			this.label9.Text = "You must hit \'Save\' before switching files to retain changes";
			// 
			// opnSystem
			// 
			this.opnSystem.DefaultExt = "bmp";
			this.opnSystem.Filter = "Bitmaps (.bmp) | *.bmp";
			this.opnSystem.FileOk += new System.ComponentModel.CancelEventHandler(this.opnSystem_FileOk);
			// 
			// savSystem
			// 
			this.savSystem.Filter = "Bitmaps (.bmp) | *.bmp";
			this.savSystem.FileOk += new System.ComponentModel.CancelEventHandler(this.savSystem_FileOk);
			// 
			// opnMission
			// 
			this.opnMission.CheckFileExists = false;
			this.opnMission.CheckPathExists = false;
			this.opnMission.DefaultExt = "tie";
			this.opnMission.Filter = "Mission Files|*.tie";
			this.opnMission.FileOk += new System.ComponentModel.CancelEventHandler(this.opnMission_FileOk);
			// 
			// frmBattle
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(560, 302);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.cmdSave);
			this.Controls.Add(this.tcBattle);
			this.Controls.Add(this.lblBattle);
			this.Controls.Add(this.cmdPrev);
			this.Controls.Add(this.cmdNext);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmBattle";
			this.Text = "YOGEME TIE Battle Editor";
			this.tcBattle.ResumeLayout(false);
			this.tabBattle.ResumeLayout(false);
			this.tabBattle.PerformLayout();
			this.tabMission.ResumeLayout(false);
			this.tabMission.PerformLayout();
			this.tabBitmap.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picGalaxy)).EndInit();
			this.grpFrame.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numFrameTop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFrameLeft)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFrameWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFrameHeight)).EndInit();
			this.tabSystem.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picSystem)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		Button cmdPrev;
		Button cmdNext;
		Label lblBattle;
		TabPage tabBattle;
		TabPage tabMission;
		TabPage tabBitmap;
		TextBox txtBattle;
		TextBox txtBTitle1;
		TextBox txtBTitle2;
		TextBox txtCTitle1;
		TextBox txtCTitle2;
		TextBox txtCutscene;
		Label label1;
		Label label2;
		Label label3;
		Label label4;
		Label label5;
		TextBox txtDesc;
		ListBox lstMiss;
		Label label6;
		Label label7;
		Button cmdAdd;
		Button cmdRemove;
		TextBox txtSystem;
		Label label8;
		Button cmdSave;
		Label label9;
		NumericUpDown numFrameTop;
		NumericUpDown numFrameLeft;
		NumericUpDown numFrameHeight;
		NumericUpDown numFrameWidth;
		GroupBox grpFrame;
		Label label10;
		Label label11;
		Label label12;
		Label label13;
		TabPage tabSystem;
		PictureBox picSystem;
		PictureBox picGalaxy;
		Button cmdExport;
		Button cmdImport;
		OpenFileDialog opnSystem;
		SaveFileDialog savSystem;
		Label label14;
		TabControl tcBattle;
		OpenFileDialog opnMission;
		Button cmdMoveUp;
		Button cmdMoveDown;
	}
}