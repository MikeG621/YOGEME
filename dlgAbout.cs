/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.1
 */

/* CHANGELOG
 * v1.0, 110921
 * - Release
 */

using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>Dialog to display program information</summary>
	public partial class dlgAbout : Form
	{
		public dlgAbout()
		{
			InitializeComponent();
			lblVersion.Text += Application.ProductVersion;
			Height = 218;
		}

		private void cmdClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void linkIdmr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Common.LaunchIdmr();
		}
		private void linkGE_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Common.LaunchER();
		}

		private void lnkMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Common.EmailJagged();
		}

		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
			Common.LaunchIdmr();
		}
	}
}
