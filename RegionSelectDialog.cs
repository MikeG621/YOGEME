/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.16
 *
 * CHANGELOG
 * v1.16, 241013
 * [UPD] ctor uses Region[] instead of string[]
 * v1.4.1, 171118
 * [NEW] created (#13)
 */

using Idmr.Platform.Xwa;
using System;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class RegionSelectDialog : Form
	{
		/// <summary>Initializes a dialog using custom region names.</summary>
		/// <param name="regions">Array of region objects, must have a <see cref="Array.Length"/> of <b>4</b>.</param>
		/// <exception cref="ArgumentException">Length of <paramref name="regions"/> is not <b>4</b>.</exception>
		public RegionSelectDialog(Mission.Region[] regions)
		{
			if (regions.Length != 4) throw new ArgumentException("regions must have length of 4.");

			InitializeComponent();
			optRegion1.Text = regions[0].Name;
			optRegion2.Text = regions[1].Name;
			optRegion3.Text = regions[2].Name;
			optRegion4.Text = regions[3].Name;
		}

		void cmdOk_Click(object sender, EventArgs e) => Close();
		void cmdCancel_Click(object sender, EventArgs e) => Close();

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