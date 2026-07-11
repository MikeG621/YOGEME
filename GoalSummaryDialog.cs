/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2026 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.18
 * 
 * CHANGELOG
 * v1.18, 260711
 * [NEW #137] FormScaler implemented
 * v1.3, 170107
 * [NEW] Dialog created [JB]
 */

using System;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
    public partial class GoalSummaryDialog : Form
    {
        readonly FormScaler _scaler;

        public GoalSummaryDialog(string displayText)
        {
            InitializeComponent();
            txtSummary.Text = displayText;
            txtSummary.Select(0, 0);
            _scaler = new FormScaler(this);
        }

        void cmdClose_Click(object sender, EventArgs e) => Close();
    }
}