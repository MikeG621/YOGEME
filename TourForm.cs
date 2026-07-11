/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2026 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.18
 *
 * CHANGELOG
 * v1.18, 260711
 * [UPD] LfdReader v3 update
 * [NEW #137] FormScaler implemented
 * v1.16, 241013
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
		readonly FormScaler _scaler;
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
			_scaler = new FormScaler(this);
		}

		void loadTour()
		{
			_tour = (Text)_missions.Resources["TEXTtour" + _tourIndex];
			lstMiss.Items.Clear();
			lstMiss.Items.AddRange(_tour.Strings[0].GetSubstringArray());
			lstMiss.SelectedIndex = 0;
			lblTour.Text = "Tour " + _tourIndex;
		}

		void cmdAdd_Click(object sender, EventArgs e) => opnMission.ShowDialog();
		void cmdCancel_Click(object sender, EventArgs e) => Close();
		void cmdMoveDown_Click(object sender, EventArgs e)
		{
			int missIndex = lstMiss.SelectedIndex;
			if (missIndex == -1 || missIndex == lstMiss.Items.Count - 1) return;

			string strTemp = _tour.Strings[2 + missIndex].Value;  //description
			_tour.Strings[2 + missIndex].Value = _tour.Strings[2 + missIndex + 1].Value;
			_tour.Strings[2 + missIndex + 1].Value = strTemp;

			string[] arrTemp = _tour.Strings[1].GetSubstringArray(); // title
			strTemp = arrTemp[missIndex];
			arrTemp[missIndex] = arrTemp[missIndex + 1];
			arrTemp[missIndex + 1] = strTemp;
			_tour.Strings[1].Value = string.Join("\0", arrTemp);

			arrTemp = _tour.Strings[0].GetSubstringArray(); // mission
			strTemp = arrTemp[missIndex];
			arrTemp[missIndex] = arrTemp[missIndex + 1];
			arrTemp[missIndex + 1] = strTemp;
			_tour.Strings[0].Value = string.Join("\0", arrTemp);
			lstMiss.Items.RemoveAt(missIndex);
			lstMiss.Items.Insert(missIndex + 1, strTemp);
			lstMiss.SelectedIndex = missIndex + 1;
		}
		void cmdMoveUp_Click(object sender, EventArgs e)
		{
			int missIndex = lstMiss.SelectedIndex;
			if (missIndex < 1) return;

			string strTemp = _tour.Strings[2 + missIndex].Value;  //description
			_tour.Strings[2 + missIndex].Value = _tour.Strings[2 + missIndex - 1].Value;
			_tour.Strings[2 + missIndex - 1].Value = strTemp;

			string[] arrTemp = _tour.Strings[1].GetSubstringArray(); // title
			strTemp = arrTemp[missIndex];
			arrTemp[missIndex] = arrTemp[missIndex - 1];
			arrTemp[missIndex - 1] = strTemp;
			_tour.Strings[1].Value = string.Join("\0", arrTemp);

			arrTemp = _tour.Strings[0].GetSubstringArray(); // mission
			strTemp = arrTemp[missIndex];
			arrTemp[missIndex] = arrTemp[missIndex - 1];
			arrTemp[missIndex - 1] = strTemp;
			_tour.Strings[0].Value = string.Join("\0", arrTemp);
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
			_tour.Strings[0].RemoveSubstringAt(index);
			_tour.Strings[1].RemoveSubstringAt(index);
			for (int i = index + 2; i < _tour.NumberOfStrings - 1; i++) _tour.Strings[i].Value = _tour.Strings[i + 1].Value;
			_tour.NumberOfStrings--;
			lstMiss.Items.RemoveAt(index);
			try { lstMiss.SelectedIndex = index; }
			catch (ArgumentOutOfRangeException) { lstMiss.SelectedIndex = index - 1; }
		}

		void lstMiss_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex == -1) return;

			int missIndex = lstMiss.SelectedIndex;
			string desc = _tour.Strings[2 + missIndex].FormattedValue;
			desc = desc.Replace(Convert.ToChar(1), ']');
			desc = desc.Replace(Convert.ToChar(2), '[');
			txtDesc.Text = desc;
			txtTitle.Text = _tour.Strings[1].GetSubstring(missIndex);
		}

		void opnMission_FileOk(object sender, CancelEventArgs e)
		{
			string strMission = opnMission.FileName;
			strMission = strMission.Remove(0, strMission.LastIndexOf("\\") + 1);
			strMission = strMission.Remove(strMission.Length - 4, 4);
			lstMiss.Items.Add(strMission);
			_tour.Strings[0].Value += "\0" + strMission;
			_tour.Strings[1].Value += "\0" + "<Enter Title>";
			_tour.NumberOfStrings++;
			_tour.Strings[_tour.NumberOfStrings - 1].Value = "<Enter Description>";
			lstMiss.SelectedIndex = lstMiss.Items.Count - 1;
		}

		void txtDesc_TextChanged(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex == -1) return;

			int missIndex = lstMiss.SelectedIndex;
			string desc = txtDesc.Text.Replace("\r\n", "\0");
			desc = desc.Replace(']', Convert.ToChar(1));
			desc = desc.Replace('[', Convert.ToChar(2));
			_tour.Strings[2 + missIndex].Value = desc;
		}

		void txtTitle_TextChanged(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex == -1) return;

			_tour.Strings[1].SetSubstring(lstMiss.SelectedIndex, txtTitle.Text);
		}
	}
}