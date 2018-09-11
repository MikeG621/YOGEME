/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2017 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.3
 */

/* CHANGELOG
 * v1.3, 170107
 * [NEW] Dialog created [JB]
 */

using System;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class GoalSummaryDialog : Form
    {
        public GoalSummaryDialog(string displayText)
        {
            InitializeComponent();
            txtSummary.Text = displayText;
            txtSummary.Select(0,0);
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
