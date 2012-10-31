/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.2.2
 */

/* CHANGELOG
 * v1.1.1, 120814
 * - class renamed
 * v1.0, 110921
 * - Release
 */

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

		void cmdClose_Click(object sender, System.EventArgs e) { Close(); }

		void lnkGE_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { Common.LaunchER(); }
		void lnkIdmr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { Common.LaunchIdmr(); }
		void lnkMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { Common.EmailJagged(); }

		void pctBanner_Click(object sender, System.EventArgs e) { Common.LaunchIdmr(); }
	}
}
