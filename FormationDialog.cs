/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.2
 */

/* CHANGELOG
 * v1.1.1, 120814
 * - class renamed
 * v1.0, 110921
 * - Release
 */

using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>Dialog to visually choose Flightgroup Formation</summary>
	public partial class FormationDialog : Form
	{
		public int Formation { get { return cboFormation.SelectedIndex; } }
		int _index;
		ArrayList _forms = new ArrayList(34);

		public FormationDialog(int index)
		{
			InitializeComponent();
			cboFormation.Items.AddRange(Idmr.Platform.BaseStrings.Formation);
			_index = index;
			Assembly a = Assembly.GetExecutingAssembly();
			string[] resourceNames = a.GetManifestResourceNames();
			for (int i=0;i<_forms.Capacity;i++) _forms.Add("new");
			foreach (string s in resourceNames)
			{
				if (s.StartsWith("Idmr.Yogeme.images.form") && s.EndsWith(".png"))
				{
					int num = Convert.ToInt32(s.Substring(23, s.Length-27));
					System.IO.Stream img = a.GetManifestResourceStream(s);
					Bitmap bmp = Bitmap.FromStream(img) as Bitmap;
					_forms[num] = bmp;
					img.Close();
				}
			}
			cboFormation.SelectedIndex = index;
			Height = 338;
		}

		public void SetToTie95()
		{
			cboFormation.Items.Clear();
			cboFormation.Items.AddRange(Idmr.Platform.Tie.Strings.Formation);
			cboFormation.SelectedIndex = _index;
		}

		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboFormation.SelectedIndex == -1) return;
			pctFormation.BackgroundImage = (Bitmap)_forms[cboFormation.SelectedIndex];
		}

		void cmdCancel_Click(object sender, EventArgs e) { Close(); }
		void cmdOK_Click(object sender, EventArgs e) { Close(); }
	}
}