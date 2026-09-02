namespace Idmr.Yogeme
{
	partial class XwaLiconForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XwaLiconForm));
			this.pctLicon = new System.Windows.Forms.PictureBox();
			this.btnOpenLicon = new System.Windows.Forms.Button();
			this.btnOpenShiplist = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblShiplist = new System.Windows.Forms.Label();
			this.lblLicon = new System.Windows.Forms.Label();
			this.vsbIcons = new System.Windows.Forms.VScrollBar();
			this.lstSpecies = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnZoomIn = new System.Windows.Forms.Button();
			this.btnZoomOut = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.optMove = new System.Windows.Forms.RadioButton();
			this.optModify = new System.Windows.Forms.RadioButton();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.lblCoords = new System.Windows.Forms.Label();
			this.opnFile = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.pctLicon)).BeginInit();
			this.SuspendLayout();
			// 
			// pctLicon
			// 
			this.pctLicon.BackColor = System.Drawing.Color.Black;
			this.pctLicon.Location = new System.Drawing.Point(12, 70);
			this.pctLicon.Name = "pctLicon";
			this.pctLicon.Size = new System.Drawing.Size(645, 318);
			this.pctLicon.TabIndex = 0;
			this.pctLicon.TabStop = false;
			this.pctLicon.Paint += new System.Windows.Forms.PaintEventHandler(this.pctLicon_Paint);
			this.pctLicon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctLicon_MouseDown);
			this.pctLicon.MouseEnter += new System.EventHandler(this.pctLicon_MouseEnter);
			this.pctLicon.MouseLeave += new System.EventHandler(this.pctLicon_MouseLeave);
			this.pctLicon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctLicon_MouseMove);
			this.pctLicon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctLicon_MouseUp);
			// 
			// btnOpenLicon
			// 
			this.btnOpenLicon.Location = new System.Drawing.Point(12, 12);
			this.btnOpenLicon.Name = "btnOpenLicon";
			this.btnOpenLicon.Size = new System.Drawing.Size(24, 23);
			this.btnOpenLicon.TabIndex = 1;
			this.btnOpenLicon.Text = "...";
			this.btnOpenLicon.UseVisualStyleBackColor = true;
			this.btnOpenLicon.Click += new System.EventHandler(this.btnOpenLicon_Click);
			// 
			// btnOpenShiplist
			// 
			this.btnOpenShiplist.Location = new System.Drawing.Point(12, 41);
			this.btnOpenShiplist.Name = "btnOpenShiplist";
			this.btnOpenShiplist.Size = new System.Drawing.Size(24, 23);
			this.btnOpenShiplist.TabIndex = 1;
			this.btnOpenShiplist.Text = "...";
			this.btnOpenShiplist.UseVisualStyleBackColor = true;
			this.btnOpenShiplist.Click += new System.EventHandler(this.btnOpenShiplist_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(42, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "LICON:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(42, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "SHIPLIST:";
			// 
			// lblShiplist
			// 
			this.lblShiplist.AutoSize = true;
			this.lblShiplist.Location = new System.Drawing.Point(106, 46);
			this.lblShiplist.Name = "lblShiplist";
			this.lblShiplist.Size = new System.Drawing.Size(56, 13);
			this.lblShiplist.TabIndex = 3;
			this.lblShiplist.Text = "(lblShiplist)";
			// 
			// lblLicon
			// 
			this.lblLicon.AutoSize = true;
			this.lblLicon.Location = new System.Drawing.Point(106, 17);
			this.lblLicon.Name = "lblLicon";
			this.lblLicon.Size = new System.Drawing.Size(49, 13);
			this.lblLicon.TabIndex = 3;
			this.lblLicon.Text = "(lblLicon)";
			// 
			// vsbIcons
			// 
			this.vsbIcons.Location = new System.Drawing.Point(660, 70);
			this.vsbIcons.Name = "vsbIcons";
			this.vsbIcons.Size = new System.Drawing.Size(20, 318);
			this.vsbIcons.TabIndex = 4;
			// 
			// lstSpecies
			// 
			this.lstSpecies.FormattingEnabled = true;
			this.lstSpecies.Location = new System.Drawing.Point(12, 425);
			this.lstSpecies.Name = "lstSpecies";
			this.lstSpecies.ScrollAlwaysVisible = true;
			this.lstSpecies.Size = new System.Drawing.Size(668, 134);
			this.lstSpecies.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 399);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(37, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Zoom:";
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.Location = new System.Drawing.Point(55, 394);
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(24, 23);
			this.btnZoomIn.TabIndex = 1;
			this.btnZoomIn.Text = "+";
			this.btnZoomIn.UseVisualStyleBackColor = true;
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.Enabled = false;
			this.btnZoomOut.Location = new System.Drawing.Point(85, 394);
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(24, 23);
			this.btnZoomOut.TabIndex = 1;
			this.btnZoomOut.Text = "-";
			this.btnZoomOut.UseVisualStyleBackColor = true;
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(166, 399);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(37, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Mode:";
			// 
			// optMove
			// 
			this.optMove.AutoSize = true;
			this.optMove.Checked = true;
			this.optMove.Location = new System.Drawing.Point(209, 397);
			this.optMove.Name = "optMove";
			this.optMove.Size = new System.Drawing.Size(52, 17);
			this.optMove.TabIndex = 8;
			this.optMove.Text = "Move";
			this.optMove.UseVisualStyleBackColor = true;
			// 
			// optModify
			// 
			this.optModify.AutoSize = true;
			this.optModify.Location = new System.Drawing.Point(267, 397);
			this.optModify.Name = "optModify";
			this.optModify.Size = new System.Drawing.Size(56, 17);
			this.optModify.TabIndex = 8;
			this.optModify.Text = "Modify";
			this.optModify.UseVisualStyleBackColor = true;
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(605, 394);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(75, 23);
			this.btnReset.TabIndex = 9;
			this.btnReset.Text = "&Reset Craft";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(524, 565);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 10;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(605, 565);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(443, 565);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 10;
			this.btnApply.Text = "&Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(440, 401);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Coords:";
			// 
			// lblCoords
			// 
			this.lblCoords.AutoSize = true;
			this.lblCoords.Location = new System.Drawing.Point(489, 401);
			this.lblCoords.Name = "lblCoords";
			this.lblCoords.Size = new System.Drawing.Size(88, 13);
			this.lblCoords.TabIndex = 12;
			this.lblCoords.Text = "000 000 000 000";
			// 
			// XwaLiconForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(688, 591);
			this.Controls.Add(this.lblCoords);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.optModify);
			this.Controls.Add(this.optMove);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lstSpecies);
			this.Controls.Add(this.vsbIcons);
			this.Controls.Add(this.lblLicon);
			this.Controls.Add(this.lblShiplist);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOpenShiplist);
			this.Controls.Add(this.btnZoomOut);
			this.Controls.Add(this.btnZoomIn);
			this.Controls.Add(this.btnOpenLicon);
			this.Controls.Add(this.pctLicon);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "XwaLiconForm";
			this.Text = "XWA LICON/SHIPLIST Editor";
			((System.ComponentModel.ISupportInitialize)(this.pctLicon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pctLicon;
		private System.Windows.Forms.Button btnOpenLicon;
		private System.Windows.Forms.Button btnOpenShiplist;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblShiplist;
		private System.Windows.Forms.Label lblLicon;
		private System.Windows.Forms.VScrollBar vsbIcons;
		private System.Windows.Forms.ListBox lstSpecies;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnZoomIn;
		private System.Windows.Forms.Button btnZoomOut;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton optMove;
		private System.Windows.Forms.RadioButton optModify;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblCoords;
		private System.Windows.Forms.OpenFileDialog opnFile;
	}
}