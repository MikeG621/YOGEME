/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.12+
 */

/* CHANGELOG
 * [UPD] cleanup
 * v1.12, 220103
 * - Release
 */

using Idmr.LfdReader;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class TourForm : Form
	{
		readonly LfdFile _missions;
		Text _tour;
		int _tourIndex;

		public TourForm()
		{
			InitializeComponent();
			var config = Settings.GetInstance();
			if (!config.XwingInstalled)
			{
				MessageBox.Show("X-wing installation not found, Tour function not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
				return;
			}
			else
			{
				bool remastered = Directory.Exists(config.XwingPath + "\\X-wing Data");
				_missions = new LfdFile(config.XwingPath + (remastered ? "\\X-Wing Data" : "") + "\\RESOURCE\\MISSIONS.LFD");
				opnMission.InitialDirectory = config.XwingPath + (remastered ? "\\X-Wing Data" : "") + "\\MISSION";
				_tourIndex = 1;
				loadTour();
			}
		}

		void loadTour()
		{
			_tour = (Text)_missions.Resources["TEXTtour" + _tourIndex];
			lstMiss.Items.Clear();
			lstMiss.Items.AddRange(_tour.Strings[0].Split('\0'));
			lstMiss.SelectedIndex = 0;
			lblTour.Text = "Tour " + _tourIndex;
		}

		void cmdAdd_Click(object sender, EventArgs e) => opnMission.ShowDialog();
		void cmdCancel_Click(object sender, EventArgs e) => Close();
		void cmdMoveDown_Click(object sender, EventArgs e)
		{
			int missIndex = lstMiss.SelectedIndex;
			if (missIndex == -1 || missIndex == lstMiss.Items.Count - 1) return;

			string strTemp = _tour.Strings[2 + missIndex];  //description
			_tour.Strings[2 + missIndex] = _tour.Strings[2 + missIndex + 1];
			_tour.Strings[2 + missIndex + 1] = strTemp;

			string[] arrTemp = _tour.Strings[1].Split('\0'); // title
			strTemp = arrTemp[missIndex];
			arrTemp[missIndex] = arrTemp[missIndex + 1];
			arrTemp[missIndex + 1] = strTemp;
			_tour.Strings[1] = string.Join("\0", arrTemp);

			arrTemp = _tour.Strings[0].Split('\0'); // mission
			strTemp = arrTemp[missIndex];
			arrTemp[missIndex] = arrTemp[missIndex + 1];
			arrTemp[missIndex + 1] = strTemp;
			_tour.Strings[0] = string.Join("\0", arrTemp);
			lstMiss.Items.RemoveAt(missIndex);
			lstMiss.Items.Insert(missIndex + 1, strTemp);
			lstMiss.SelectedIndex = missIndex + 1;
		}
		void cmdMoveUp_Click(object sender, EventArgs e)
		{
			int missIndex = lstMiss.SelectedIndex;
			if (missIndex < 1) return;

			string strTemp = _tour.Strings[2 + missIndex];  //description
			_tour.Strings[2 + missIndex] = _tour.Strings[2 + missIndex - 1];
			_tour.Strings[2 + missIndex - 1] = strTemp;

			string[] arrTemp = _tour.Strings[1].Split('\0'); // title
			strTemp = arrTemp[missIndex];
			arrTemp[missIndex] = arrTemp[missIndex - 1];
			arrTemp[missIndex - 1] = strTemp;
			_tour.Strings[1] = string.Join("\0", arrTemp);

			arrTemp = _tour.Strings[0].Split('\0'); // mission
			strTemp = arrTemp[missIndex];
			arrTemp[missIndex] = arrTemp[missIndex - 1];
			arrTemp[missIndex - 1] = strTemp;
			_tour.Strings[0] = string.Join("\0", arrTemp);
			lstMiss.Items.RemoveAt(missIndex);
			lstMiss.Items.Insert(missIndex - 1, strTemp);
			lstMiss.SelectedIndex = missIndex - 1;
		}
		void cmdNext_Click(object sender, EventArgs e)
		{
			if (_tourIndex < 5) _tourIndex++;   // LFD suggests there's up to 8, but there's only details for 5
			loadTour();
		}
		void cmdOK_Click(object sender, EventArgs e)
		{
			_missions.Write();
			Close();
		}
		void cmdPrev_Click(object sender, EventArgs e)
		{
			if (_tourIndex > 1) _tourIndex--;
			loadTour();
		}
		void cmdRemove_Click(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex == -1) return;

			int index = lstMiss.SelectedIndex;
			_tour.Strings[0] = _tour.Strings[0].Replace(lstMiss.SelectedItem.ToString(), "").Replace("\0\0", "\0").Trim('\0');
			_tour.Strings[1] = _tour.Strings[1].Replace(txtTitle.Text, "").Replace("\0\0", "\0").Trim('\0');
			for (int i = index + 2; i < _tour.NumberOfStrings - 1; i++) _tour.Strings[i] = _tour.Strings[i + 1];
			_tour.NumberOfStrings--;
			lstMiss.Items.RemoveAt(index);
			try { lstMiss.SelectedIndex = index; }
			catch (ArgumentOutOfRangeException) { lstMiss.SelectedIndex = index - 1; }
		}

		void lstMiss_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex == -1) return;

			int missIndex = lstMiss.SelectedIndex;
			string desc = _tour.Strings[2 + missIndex].Replace("\0", "\r\n");
			desc = desc.Replace(Convert.ToChar(1), ']');
			desc = desc.Replace(Convert.ToChar(2), '[');
			txtDesc.Text = desc;
			txtTitle.Text = _tour.Strings[1].Split('\0')[missIndex];
		}

		void opnMission_FileOk(object sender, CancelEventArgs e)
		{
			string strMission = opnMission.FileName;
			strMission = strMission.Remove(0, strMission.LastIndexOf("\\") + 1);
			strMission = strMission.Remove(strMission.Length - 4, 4);
			lstMiss.Items.Add(strMission);
			_tour.Strings[0] += "\0" + strMission;
			_tour.Strings[1] += "\0" + "<Enter Title>";
			_tour.NumberOfStrings++;
			_tour.Strings[_tour.NumberOfStrings - 1] = "<Enter Description>";
			lstMiss.SelectedIndex = lstMiss.Items.Count - 1;
		}

		void txtDesc_TextChanged(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex == -1) return;

			int missIndex = lstMiss.SelectedIndex;
			string desc = txtDesc.Text.Replace("\r\n", "\0");
			desc = desc.Replace(']', Convert.ToChar(1));
			desc = desc.Replace('[', Convert.ToChar(2));
			_tour.Strings[2 + missIndex] = desc;
		}

		void txtTitle_TextChanged(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex == -1) return;

			string[] titles = _tour.Strings[1].Split('\0');
			titles[lstMiss.SelectedIndex] = txtTitle.Text;
			_tour.Strings[1] = string.Join("\0", titles);
		}
	}
}