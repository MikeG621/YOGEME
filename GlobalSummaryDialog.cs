/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2023 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.15.5
 */

/* CHANGELOG
 * v1.15.5, 231222
 * [NEW #97] Dialog created
 */

using System;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class GlobalSummaryDialog : Form
	{
		readonly string[] _groups = new string[256];
		readonly string[] _units = new string[256];

		public GlobalSummaryDialog(Platform.Tie.FlightGroupCollection fgs)
		{
			InitializeComponent();
			init();

			for (int i = 0; i < fgs.Count; i++) if (fgs[i].GlobalGroup != 0) _groups[fgs[i].GlobalGroup] += "\t" + fgs[i].ToString(true) + "\r\n";

			update();
		}
		public GlobalSummaryDialog(Platform.Xvt.FlightGroupCollection fgs)
		{
			InitializeComponent();
			init();

			for (int i = 0; i < fgs.Count; i++)
			{
				if (fgs[i].GlobalGroup != 0) _groups[fgs[i].GlobalGroup] += "\t" + fgs[i].ToString(true) + "\r\n";
				if (fgs[i].GlobalUnit != 0) _units[fgs[i].GlobalUnit] += "\t" + fgs[i].ToString(true) + "\r\n";
			}

			update();
		}
		public GlobalSummaryDialog(Platform.Xwa.FlightGroupCollection fgs)
		{
			InitializeComponent();
			init();

			for (int i = 0; i < fgs.Count; i++)
			{
				if (fgs[i].GlobalGroup != 0) _groups[fgs[i].GlobalGroup] += "\t" + fgs[i].ToString(true) + "\r\n";
				if (fgs[i].GlobalUnit != 0) _units[fgs[i].GlobalUnit] += "\t" + fgs[i].ToString(true) + "\r\n";
			}

			update();
		}

		void init()
		{
			for (int i = 0; i < 256; i++) { _groups[i] = ""; _units[i] = ""; }
		}

		void update()
		{
			for (int i = 1; i < 256; i++) if (_groups[i] != "") txtSummary.Text += "Global Group " + i + "\r\n" + _groups[i] + "\r\n";
			if (txtSummary.Text.Length > 0) txtSummary.Text += "--------------------\r\n";
			for (int i = 1; i < 256; i++) if (_units[i] != "") txtSummary.Text += "Global Unit " + i + "\r\n" + _units[i] + "\r\n";
			txtSummary.Select(0, 0);
		}

		void cmdClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
