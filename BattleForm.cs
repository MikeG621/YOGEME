/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.16
 *
 * CHANGELOG
 * v1.16, 241013
 * [UPD] cleanup
 * v1.8.1, 201213
 * [UPS] Settings passed in
 * v1.6.4, 200119
 * [FIX #31] form Height increased to account for W10 visual style
 * v1.2.3, 141214
 * [UPD] change to MPL
 * [FIX] ctor continued running if TIE wasn't installed
 * v1.2, 121006
 * - Settings passed in
 * [FIX] Bug in Lfd image processing that corrupted the system image when saving
 * v1.1.1, 120814
 * - class renamed
 * - renamed some things
 * v1.0, 110921
 * - Release
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Idmr.LfdReader;

namespace Idmr.Yogeme
{
	/// <summary> Loads Battle#.LFD for editing</summary>
	public partial class BattleForm : Form
	{
		string _battlePath; // path to selected Battle#.lfd
		readonly string _installPath;   // TIE95 install directory
		int _battleIndex = 1;
		int _numMiss;
		string[] _missionFiles = new string[8];
		string[] _missionDescriptions = new string[8];
		string _deltName;
		Bitmap _systemImage;
		readonly Bitmap _galaxyImage;
		readonly ColorPalette _systemPalette;
		bool _loading = false;
		bool _dragging = false;
		LfdFile _battle;

		public BattleForm()
		{
			InitializeComponent();
			if (!Settings.GetInstance().TieInstalled)
			{
				MessageBox.Show("TIE95 installation not found, Battle function not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
				return;
			}
			else
			{
				_installPath = Settings.GetInstance().TiePath;
				Pltt standard = (Pltt)(new LfdFile(_installPath + "\\RESOURCE\\EMPIRE.LFD").Resources["PLTTstandard"]);
				LfdFile tourdesk = new LfdFile(_installPath + "\\RESOURCE\\TOURDESK.LFD");
				Pltt toddesk = (Pltt)tourdesk.Resources["PLTTtoddesk"];
				_systemPalette = Pltt.ConvertToPalette(new Pltt[] { standard, toddesk });
				Delt galaxy = (Delt)tourdesk.Resources["DELTgalaxy"];
				galaxy.Palette = _systemPalette;
				_galaxyImage = galaxy.Image;
				picGalaxy.Image = _galaxyImage;
				picGalaxy.Size = _galaxyImage.Size;
				numFrameLeft.Maximum = _galaxyImage.Width - 1;
				numFrameTop.Maximum = _galaxyImage.Height - 1;
			}
			_battlePath = _installPath + "\\RESOURCE\\Battle1.lfd";
			opnMission.InitialDirectory = _installPath + "\\MISSION";
			try { loadFile(_battlePath); }
			catch (Exception x)
			{
				MessageBox.Show(x.Message + " Battle function not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		/// <summary>Draws the red frame on the galaxy map signifiying where the system is located</summary>
		void drawFrame()
		{
			int l = (int)numFrameLeft.Value, t = (int)numFrameTop.Value, w = (int)numFrameWidth.Value, h = (int)numFrameHeight.Value;
			Pen pnFrame = new Pen(Color.Red);
			picGalaxy.Refresh();
			Graphics g = picGalaxy.CreateGraphics();
			g.DrawLine(pnFrame, l, t, l + w, t);
			g.DrawLine(pnFrame, l + w, t, l + w, t + h);
			g.DrawLine(pnFrame, l + w, t + h, l, t + h);
			g.DrawLine(pnFrame, l, t + h, l, t);
			g.Dispose();
		}

		/// <summary>Opens the LFD and reads it into the program</summary>
		/// <param name="path">The full filename of the LFD to open</param>
		void loadFile(string path)
		{
			_battle = new LfdFile(path);
			Text txt = (Text)_battle.Resources[0];
			txtBattle.Text = "";
			txtCutscene.Text = "";
			txtBTitle1.Text = "";
			txtBTitle2.Text = "";
			txtCTitle1.Text = "";
			txtCTitle2.Text = "";
			txtSystem.Text = "";
			lstMiss.Items.Clear();
			_missionFiles = new string[8];
			_missionDescriptions = new string[8];
			lblBattle.Text = txt.Name;
			_numMiss = txt.NumberOfStrings - 4;
			// Titles
			string[] strTemp = txt.Strings[0].Split('\0');
			txtBattle.Text = strTemp[0];
			txtCutscene.Text = strTemp[1];
			// Text
			strTemp = txt.Strings[1].Split('\0');
			txtBTitle1.Text = strTemp[0];
			txtBTitle2.Text = strTemp[1];
			txtCTitle1.Text = strTemp[2];
			if (strTemp.Length == 4) txtCTitle2.Text = strTemp[3];  // TFW's fault, either left off, or only appends '\0', not '\0\0'
																	// System
			strTemp = txt.Strings[2].Split('\0');
			_deltName = strTemp[0];
			txtSystem.Text = strTemp[1];
			// Image Frame
			_loading = true;
			string[] str_frame = strTemp[2].Split(' ');
			numFrameTop.Value = Convert.ToInt32(str_frame[0]);
			numFrameHeight.Value = Convert.ToInt32(str_frame[1]);
			numFrameLeft.Value = Convert.ToInt32(str_frame[2]);
			numFrameWidth.Value = Convert.ToInt32(str_frame[3]);
			_loading = false;
			drawFrame();
			// Missions
			strTemp = txt.Strings[3].Split('\0');
			for (int i = 0; i < strTemp.Length; i++) _missionFiles[i] = strTemp[i];
			for (int i = 0; i < _numMiss; i++) lstMiss.Items.Add(_missionFiles[i]);
			// Descriptions
			for (int i = 0; i < _numMiss; i++)
			{
				_missionDescriptions[i] = txt.Strings[4 + i];
				_missionDescriptions[i] = _missionDescriptions[i].TrimEnd('\0').Replace("\0", "\r\n");
			}
			txtDesc.Text = _missionDescriptions[0];
			// System image
			try
			{
				Delt delSystem = (Delt)_battle.Resources[1];
				delSystem.Palette = _systemPalette;
				_systemImage = delSystem.Image;
				picSystem.Image = _systemImage;
				picSystem.Size = _systemImage.Size;
			}
			catch (Exception x) { MessageBox.Show(x.Message + "  System image unavailable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
		}

		#region Main
		void cmdNext_Click(object sender, EventArgs e)
		{
			if (_battleIndex == 13) return;     //TIE95 has 13 battles, although it may be possible to add more, dunno

			try
			{
				_battleIndex++;
				_battlePath = _installPath + "\\RESOURCE\\Battle" + _battleIndex + ".lfd";
				loadFile(_battlePath);
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				_battleIndex--;
				_battlePath = _installPath + "\\RESOURCE\\Battle" + _battleIndex + ".lfd";
				loadFile(_battlePath);
			}
		}
		void cmdPrev_Click(object sender, EventArgs e)
		{
			if (_battleIndex == 1) return;

			try
			{
				_battleIndex--;
				_battlePath = _installPath + "\\RESOURCE\\Battle" + _battleIndex + ".lfd";
				loadFile(_battlePath);
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				_battleIndex++;
				_battlePath = _installPath + "\\RESOURCE\\Battle" + _battleIndex + ".lfd";
				loadFile(_battlePath);
			}
		}
		void cmdSave_Click(object sender, EventArgs e)
		{
			Text tex = (Text)_battle.Resources[0];
			Delt del = (Delt)_battle.Resources[1];
			del.Image = _systemImage;
			tex.NumberOfStrings = (short)(_numMiss + 4);
			tex.Strings[0] = txtBattle.Text + '\0' + txtCutscene.Text;
			tex.Strings[1] = txtBTitle1.Text + '\0' + txtBTitle2.Text + '\0' + txtCTitle1.Text + '\0' + txtCTitle2.Text;
			tex.Strings[2] = _deltName + '\0' + txtSystem.Text + '\0' + numFrameTop.Value + ' ' + numFrameHeight.Value + ' ' + numFrameLeft.Value + ' ' + numFrameWidth.Value;
			tex.Strings[3] = string.Join("\0", _missionFiles);
			for (int i = 0; i < _numMiss; i++) tex.Strings[4 + i] = _missionDescriptions[i].Replace("\r\n", "\0");
			try { _battle.Write(); }
			catch (Exception x) { System.Diagnostics.Debug.WriteLine("Battle save failure"); throw x; }
		}
		#endregion
		#region Missions tab
		void cmdAdd_Click(object sender, EventArgs e)
		{
			if (_numMiss == 8) return;
			opnMission.ShowDialog();
		}
		void cmdMoveDown_Click(object sender, EventArgs e)
		{
			int i = lstMiss.SelectedIndex + 1;
			if (i == -1 || i == _numMiss) return;

			string strTemp = _missionFiles[i - 1];
			_missionFiles[i - 1] = _missionFiles[i];
			_missionFiles[i] = strTemp;
			strTemp = _missionDescriptions[i - 1];
			_missionDescriptions[i - 1] = _missionDescriptions[i];
			_missionDescriptions[i] = strTemp;
			lstMiss.Items.Insert(i - 1, _missionFiles[i - 1]);
			lstMiss.Items.RemoveAt(i + 1);
			lstMiss.SelectedIndex = i;
		}
		void cmdMoveUp_Click(object sender, EventArgs e)
		{
			int i = lstMiss.SelectedIndex;
			if (i <= 0) return;

			string strTemp = _missionFiles[i - 1];
			_missionFiles[i - 1] = _missionFiles[i];
			_missionFiles[i] = strTemp;
			strTemp = _missionDescriptions[i - 1];
			_missionDescriptions[i - 1] = _missionDescriptions[i];
			_missionDescriptions[i] = strTemp;
			lstMiss.Items.Insert(i - 1, _missionFiles[i - 1]);
			lstMiss.Items.RemoveAt(i + 1);
			lstMiss.SelectedIndex = i - 1;
		}
		void cmdRemove_Click(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex != -1)
			{
				for (int i = lstMiss.SelectedIndex; i < 7; i++)
				{
					_missionDescriptions[i] = _missionDescriptions[i + 1];
					_missionFiles[i] = _missionFiles[i + 1];
				}
				lstMiss.Items.RemoveAt(lstMiss.SelectedIndex);
				txtDesc.Text = "";
			}
			_numMiss--;
		}

		void lstMiss_SelectedIndexChanged(object sender, EventArgs e) { if (lstMiss.SelectedIndex != -1) txtDesc.Text = _missionDescriptions[lstMiss.SelectedIndex]; }

		void opnMission_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string strMission = opnMission.FileName;
			strMission = strMission.Remove(0, strMission.LastIndexOf("\\") + 1);
			strMission = strMission.Remove(strMission.Length - 4, 4);
			_missionFiles[_numMiss] = strMission;
			_missionDescriptions[_numMiss] = "";
			lstMiss.Items.Add(strMission);
			_numMiss++;
		}

		void txtDesc_TextChanged(object sender, EventArgs e) { if (lstMiss.SelectedIndex != -1) _missionDescriptions[lstMiss.SelectedIndex] = txtDesc.Text; }
		#endregion
		#region System tab
		void cmdExport_Click(object sender, EventArgs e) => savSystem.ShowDialog();
		void cmdImport_Click(object sender, EventArgs e) => opnSystem.ShowDialog();

		void opnSystem_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_systemImage = new Bitmap(opnSystem.FileName) { Palette = _systemPalette };
			picSystem.Width = _systemImage.Width;
			picSystem.Height = _systemImage.Height;
			picSystem.Image = _systemImage;
		}

		void savSystem_FileOk(object sender, System.ComponentModel.CancelEventArgs e) => _systemImage.Save(savSystem.FileName, System.Drawing.Imaging.ImageFormat.Bmp); //if ImageFormat isn't specified, it saves as PNG with .BMP extension
		#endregion
		#region Galaxy tab
		void numFrameHeight_ValueChanged(object sender, EventArgs e) { if (!_loading) drawFrame(); }
		void numFrameLeft_ValueChanged(object sender, EventArgs e) { if (!_loading) drawFrame(); }
		void numFrameTop_ValueChanged(object sender, EventArgs e) { if (!_loading) drawFrame(); }
		void numFrameWidth_ValueChanged(object sender, EventArgs e) { if (!_loading) drawFrame(); }

		void picGalaxy_MouseDown(object sender, MouseEventArgs e)
		{
			int l = (int)numFrameLeft.Value, t = (int)numFrameTop.Value, w = (int)numFrameWidth.Value, h = (int)numFrameHeight.Value;
			if (e.X >= l && e.X <= (l + w) && e.Y >= t && e.Y <= (t + h)) _dragging = true;
		}
		void picGalaxy_MouseMove(object sender, MouseEventArgs e)
		{
			if (!_dragging) return;

			int l = e.X - (int)numFrameWidth.Value / 2;
			int t = e.Y - (int)numFrameHeight.Value / 2;
			if (l < 0) l = 0;
			if (t < 0) t = 0;
			if ((l + numFrameWidth.Value) > numFrameLeft.Maximum) l = (int)numFrameLeft.Maximum - (int)numFrameWidth.Value;
			if ((t + numFrameHeight.Value) > numFrameTop.Maximum) t = (int)numFrameTop.Maximum - (int)numFrameHeight.Value;
			numFrameLeft.Value = l;
			numFrameTop.Value = t;
		}
		void picGalaxy_MouseUp(object sender, MouseEventArgs e) => _dragging = false;

		void tcBattle_SelectedIndexChanged(object sender, EventArgs e) { if (tcBattle.SelectedIndex == 2) drawFrame(); }
		#endregion
	}
}