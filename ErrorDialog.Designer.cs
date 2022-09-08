
namespace Idmr.Yogeme
{
    partial class ErrorDialog
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.cmdOk = new System.Windows.Forms.Button();
            this.chkIgnore = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(6, 5);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(396, 57);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Error message string\r\nline2\r\nline3\r\nline4";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdOk
            // 
            this.cmdOk.Location = new System.Drawing.Point(182, 84);
            this.cmdOk.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(38, 19);
            this.cmdOk.TabIndex = 1;
            this.cmdOk.Text = "&Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // chkIgnore
            // 
            this.chkIgnore.AutoSize = true;
            this.chkIgnore.Location = new System.Drawing.Point(139, 63);
            this.chkIgnore.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkIgnore.Name = "chkIgnore";
            this.chkIgnore.Size = new System.Drawing.Size(130, 17);
            this.chkIgnore.TabIndex = 2;
            this.chkIgnore.Text = "Ignore repeated errors";
            this.chkIgnore.UseVisualStyleBackColor = true;
            // 
            // ErrorDialog
            // 
            this.AcceptButton = this.cmdOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 107);
            this.Controls.Add(this.chkIgnore);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.CheckBox chkIgnore;
    }
}