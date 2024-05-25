namespace Idmr.Yogeme
{
	partial class RegionSelectDialog
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
			this.label1 = new System.Windows.Forms.Label();
			this.optRegion1 = new System.Windows.Forms.RadioButton();
			this.optRegion2 = new System.Windows.Forms.RadioButton();
			this.optRegion3 = new System.Windows.Forms.RadioButton();
			this.optRegion4 = new System.Windows.Forms.RadioButton();
			this.cmdOk = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select Region to apply Backdrops";
			// 
			// optRegion1
			// 
			this.optRegion1.AutoSize = true;
			this.optRegion1.Checked = true;
			this.optRegion1.Location = new System.Drawing.Point(15, 34);
			this.optRegion1.Name = "optRegion1";
			this.optRegion1.Size = new System.Drawing.Size(68, 17);
			this.optRegion1.TabIndex = 1;
			this.optRegion1.TabStop = true;
			this.optRegion1.Text = "Region 1";
			this.optRegion1.UseVisualStyleBackColor = true;
			// 
			// optRegion2
			// 
			this.optRegion2.AutoSize = true;
			this.optRegion2.Location = new System.Drawing.Point(126, 34);
			this.optRegion2.Name = "optRegion2";
			this.optRegion2.Size = new System.Drawing.Size(68, 17);
			this.optRegion2.TabIndex = 1;
			this.optRegion2.Text = "Region 2";
			this.optRegion2.UseVisualStyleBackColor = true;
			// 
			// optRegion3
			// 
			this.optRegion3.AutoSize = true;
			this.optRegion3.Location = new System.Drawing.Point(15, 57);
			this.optRegion3.Name = "optRegion3";
			this.optRegion3.Size = new System.Drawing.Size(68, 17);
			this.optRegion3.TabIndex = 1;
			this.optRegion3.Text = "Region 3";
			this.optRegion3.UseVisualStyleBackColor = true;
			// 
			// optRegion4
			// 
			this.optRegion4.AutoSize = true;
			this.optRegion4.Location = new System.Drawing.Point(126, 57);
			this.optRegion4.Name = "optRegion4";
			this.optRegion4.Size = new System.Drawing.Size(68, 17);
			this.optRegion4.TabIndex = 1;
			this.optRegion4.Text = "Region 4";
			this.optRegion4.UseVisualStyleBackColor = true;
			// 
			// cmdOk
			// 
			this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOk.Location = new System.Drawing.Point(15, 80);
			this.cmdOk.Name = "cmdOk";
			this.cmdOk.Size = new System.Drawing.Size(75, 23);
			this.cmdOk.TabIndex = 2;
			this.cmdOk.Text = "&OK";
			this.cmdOk.UseVisualStyleBackColor = true;
			this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(119, 80);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// RegionSelectDialog
			// 
			this.AcceptButton = this.cmdOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(213, 116);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOk);
			this.Controls.Add(this.optRegion4);
			this.Controls.Add(this.optRegion3);
			this.Controls.Add(this.optRegion2);
			this.Controls.Add(this.optRegion1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RegionSelectDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Region";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton optRegion1;
		private System.Windows.Forms.RadioButton optRegion2;
		private System.Windows.Forms.RadioButton optRegion3;
		private System.Windows.Forms.RadioButton optRegion4;
		private System.Windows.Forms.Button cmdOk;
		private System.Windows.Forms.Button cmdCancel;
	}
}