/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2022 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.13.9
 */

/* CHANGELOG
 * v1.13.9, 220907
 * - Release
 */

using System.Windows.Forms;

namespace Idmr.Yogeme
{
    public partial class ErrorDialog : Form
    {
        public ErrorDialog(string message)
        {
            InitializeComponent();

            lblMessage.Text = message;
            chkIgnore.Visible = false;
        }
        public ErrorDialog(string message, bool allowIgnore)
        {
            InitializeComponent();

            lblMessage.Text = message;
            chkIgnore.Visible = allowIgnore;
        }

        public bool IgnoreErrors => chkIgnore.Checked;

        private void cmdOk_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
