/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2017 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.4+
 */

/* CHANGELOG
 * [NEW] created
 */

using System;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class RegionSelectDialog : Form
	{
		/// <summary>Initalizes a new dialog.</summary>
		public RegionSelectDialog()
		{
			InitializeComponent();
		}

		/// <summary>Initializes a dialog using custom region names.</summary>
		/// <param name="regionNames">Array of region names, must have a <see cref="Array.Length"/> of <b>4</b>.</param>
		/// <exception cref="ArgumentException">Length of <i>regionNames</i> is not <b>4</b>.</exception>
		public RegionSelectDialog(string[] regionNames)
		{
			if (regionNames.Length != 4) throw new ArgumentException("regionNames must have length of 4.");
			InitializeComponent();
			optRegion1.Text = regionNames[0];
			optRegion2.Text = regionNames[1];
			optRegion3.Text = regionNames[2];
			optRegion4.Text = regionNames[3];
		}

		private void cmdOk_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		public byte SelectedRegion
		{
			get
			{
				if (optRegion1.Checked) return 0;
				else if (optRegion2.Checked) return 1;
				else if (optRegion3.Checked) return 2;
				else return 3;
			}
		}
	}
}
