/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.2.3+
 */

/* CHANGELOG
 * [UPD] updated
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.1.1, 120814
 * - class renamed
 * v1.0, 110921
 * - Release
 */

using System.Diagnostics;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>Dialog to display program information</summary>
	public partial class AboutDialog : Form
	{
		public AboutDialog()
		{
			InitializeComponent();
			lblVersion.Text += Application.ProductVersion;
			Height = 218;
		}

		void cmdClose_Click(object sender, System.EventArgs e) => Close();

		void lnkCopyright_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start("mailto:mjgaisser@gmail.com?subject=YOGEME");
		void lnkIdmr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Common.LaunchGithub();

		void pctBanner_Click(object sender, System.EventArgs e) => Common.LaunchGithub();
	}
}