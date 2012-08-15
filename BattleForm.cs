/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.1.1
 */

/* CHANGELOG
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
		string _battlePath;	// path to selected Battle#.lfd
		string _installPath;	// TIE95 install directory
		int _battleIndex = 1;
		int _numMiss;
		string[] _missionFiles = new string[8];
		string[] _missionDescriptions = new string[8];
		string _deltName;
		Bitmap _systemImage;
		Bitmap _galaxyImage;
		ColorPalette _systemPalette;
		bool _loading = false;
		bool _dragging = false;
		LfdFile _battle;

		public BattleForm()
		{
			InitializeComponent();
			this.Height = 326;
			Settings config = new Settings();
			if (!config.TieInstalled)
			{
				MessageBox.Show("TIE95 installation not found, Battle function not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Close();
			}
			else
			{
				_installPath = config.TiePath;
				// dummy bitmap to create a 256 color palette
				/*Bitmap bm = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
				_systemPalette = bm.Palette;
				bm.Dispose();*/
				//palette definition
				// start with EMPIRE.PLTTstandard
				Pltt standard = (Pltt)(new LfdFile(_installPath + "\\RESOURCE\\EMPIRE.LFD").Resources["PLTTstandard"]);
				/*Rmap empire = new Rmap(_installPath + "\\RESOURCE\\EMPIRE.LFD");
				Pltt standard = new Pltt(_installPath + "\\RESOURCE\\EMPIRE.LFD", empire.SubHeaders[2].Offset);*/
				// then open up TOURDESK for the rest
				LfdFile tourdesk = new LfdFile(_installPath + "\\RESOURCE\\TOURDESK.LFD");
				Pltt toddesk = (Pltt)tourdesk.Resources["PLTTtoddesk"];
				_systemPalette = Pltt.ConvertToPalette(new Pltt[]{standard, toddesk});
				Delt galaxy = (Delt)tourdesk.Resources["DELTgalaxy"];
				galaxy.Palette = _systemPalette;
				/*FileStream fs = File.OpenRead(_installPath + "\\RESOURCE\\TOURDESK.LFD");
				Rmap tourdesk = new Rmap(fs);
				Pltt toddesk = new Pltt(fs, tourdesk.SubHeaders[1].Offset);	//PLTTtoddesk
				//galaxy map
				Delt galaxy = new Delt(fs, tourdesk.SubHeaders[7].Offset, new Pltt[]{standard, toddesk});
				fs.Close();*/
				_galaxyImage = galaxy.Image;
				picGalaxy.Image = _galaxyImage;
				picGalaxy.Size = _galaxyImage.Size;
				numFrameLeft.Maximum = _galaxyImage.Width-1;
				numFrameTop.Maximum = _galaxyImage.Height-1;
			}
			_battlePath = _installPath + "\\RESOURCE\\Battle1.lfd";
			opnMission.InitialDirectory = _installPath + "\\MISSION";
			try { loadFile(_battlePath); }
			catch(Exception x)
			{
				MessageBox.Show(x.Message + "  Battle function not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		/// <summary>Draws the red frame on the galaxy map signifiying where the system is located</summary>
		void drawFrame()
		{
			int L = (int)numFrameLeft.Value, T = (int)numFrameTop.Value, W = (int)numFrameWidth.Value, H = (int)numFrameHeight.Value;
			Pen pnFrame = new Pen(Color.Red);
			picGalaxy.Refresh();		//removes previous frame
			Graphics g = picGalaxy.CreateGraphics();	//lets us draw on it, but not permanently
			g.DrawLine(pnFrame, L, T, L+W, T);
			g.DrawLine(pnFrame, L+W, T, L+W, T+H);
			g.DrawLine(pnFrame, L+W, T+H, L, T+H);
			g.DrawLine(pnFrame, L, T+H, L, T);
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
			if (strTemp.Length == 4) txtCTitle2.Text = strTemp[3];	// TFW's fault, either left off, or only appends '\0', not '\0\0'
			// System
			strTemp = txt.Strings[2].Split('\0');
			_deltName = strTemp[0];
			txtSystem.Text = strTemp[1];
			#region Image Frame
			_loading = true;
			string[] str_frame = strTemp[2].Split(' ');
			numFrameTop.Value = Convert.ToInt32(str_frame[0]);
			numFrameHeight.Value = Convert.ToInt32(str_frame[1]);
			numFrameLeft.Value = Convert.ToInt32(str_frame[2]);
			numFrameWidth.Value = Convert.ToInt32(str_frame[3]);
			_loading = false;
			drawFrame();
			#endregion
			// Missions
			strTemp = txt.Strings[3].Split('\0');
			for (int i=0;i<strTemp.Length;i++) _missionFiles[i] = strTemp[i];
			for(int i=0;i<_numMiss;i++) lstMiss.Items.Add(_missionFiles[i]);
			// Descriptions
			for(int i=0;i<_numMiss;i++)
			{
				_missionDescriptions[i] = txt.Strings[4+i];
				_missionDescriptions[i] = _missionDescriptions[i].TrimEnd('\0').Replace("\0","\r\n");
			}
			txtDesc.Text = _missionDescriptions[0];
			#region System image
			try
			{
				Delt delSystem = (Delt)_battle.Resources[1];
				delSystem.Palette = _systemPalette;
				_systemImage = delSystem.Image;
				picSystem.Image = _systemImage;
				picSystem.Size = _systemImage.Size;
			}
			catch(Exception x)
			{
				MessageBox.Show(x.Message + "  System image unavailable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		#region Main
		void cmdNext_Click(object sender, EventArgs e)
		{
			if (_battleIndex == 13) return;		//TIE95 has 13 battles, although it may be possible to add more, dunno
			try
			{
				_battleIndex++;
				_battlePath = _installPath + "\\RESOURCE\\Battle" + _battleIndex + ".lfd";
				loadFile(_battlePath);
			}
			catch(Exception x)
			{	//if YOGEME can't load the next battle file, reload the previous
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
			catch(Exception x)
			{	// reload last successful battle file
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
			_battle.Resources[1] = del;
			tex.NumberOfStrings = (short)(_numMiss+4);
			tex.Strings[0] = txtBattle.Text + '\0' + txtCutscene.Text;
			tex.Strings[1] = txtBTitle1.Text + '\0' + txtBTitle2.Text + '\0' + txtCTitle1.Text + '\0' + txtCTitle2.Text;
			tex.Strings[2] = _deltName + '\0' + txtSystem.Text + '\0' + numFrameTop.Value + ' ' + numFrameHeight.Value
				+ ' ' + numFrameLeft.Value + ' ' + numFrameWidth.Value;
			tex.Strings[3] = String.Join("\0",_missionFiles);
			for(int i=0;i<_numMiss;i++) tex.Strings[4+i] = _missionDescriptions[i].Replace("\r\n", "\0");
			_battle.Resources[0] = tex;
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
			string strTemp;
			int i = lstMiss.SelectedIndex+1;
			if (i == -1 || i == _numMiss) return;
			strTemp = _missionFiles[i-1];
			_missionFiles[i-1] = _missionFiles[i];
			_missionFiles[i] = strTemp;
			strTemp = _missionDescriptions[i-1];
			_missionDescriptions[i-1] = _missionDescriptions[i];
			_missionDescriptions[i] = strTemp;
			lstMiss.Items.Insert(i-1, _missionFiles[i-1]);
			lstMiss.Items.RemoveAt(i+1);
			lstMiss.SelectedIndex = i;
		}
		void cmdMoveUp_Click(object sender, EventArgs e)
		{
			string strTemp;
			int i = lstMiss.SelectedIndex;
			if (i <= 0) return;
			strTemp = _missionFiles[i-1];
			_missionFiles[i-1] = _missionFiles[i];
			_missionFiles[i] = strTemp;
			strTemp = _missionDescriptions[i-1];
			_missionDescriptions[i-1] = _missionDescriptions[i];
			_missionDescriptions[i] = strTemp;
			lstMiss.Items.Insert(i-1, _missionFiles[i-1]);
			lstMiss.Items.RemoveAt(i+1);
			lstMiss.SelectedIndex = i-1;
		}
		void cmdRemove_Click(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex != -1)
			{
				int i;
				for (i=lstMiss.SelectedIndex;i<7;i++)
				{
					_missionDescriptions[i] = _missionDescriptions[i+1];
					_missionFiles[i] = _missionFiles[i+1];
				}
				lstMiss.Items.RemoveAt(lstMiss.SelectedIndex);
				txtDesc.Text = "";
			}
			_numMiss--;
		}

		void lstMiss_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex != -1) txtDesc.Text = _missionDescriptions[lstMiss.SelectedIndex];
		}

		void opnMission_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string strMission = opnMission.FileName;
			strMission = strMission.Remove(0, strMission.LastIndexOf("\\")+1);
			strMission = strMission.Remove(strMission.Length-4, 4);
			_missionFiles[_numMiss] = strMission;
			_missionDescriptions[_numMiss] = "";
			lstMiss.Items.Add(strMission);
			_numMiss++;
		}

		void txtDesc_TextChanged(object sender, EventArgs e)
		{
			if (lstMiss.SelectedIndex != -1) _missionDescriptions[lstMiss.SelectedIndex] = txtDesc.Text;
		}
		#endregion
		#region System tab
		void cmdExport_Click(object sender, EventArgs e)
		{
			savSystem.ShowDialog();
		}
		void cmdImport_Click(object sender, EventArgs e)
		{
			opnSystem.ShowDialog();
		}

		void opnSystem_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_systemImage = new Bitmap(opnSystem.FileName);
			_systemImage.Palette = _systemPalette;
			picSystem.Width = _systemImage.Width;
			picSystem.Height = _systemImage.Height;
			picSystem.Image = _systemImage;
		}

		void savSystem_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_systemImage.Save(savSystem.FileName,System.Drawing.Imaging.ImageFormat.Bmp);	//if ImageFormat isn't specified, it saves as PNG with .BMP extension
		}
		#endregion
		#region Galaxy tab
		void numFrameHeight_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) drawFrame();
		}
		void numFrameLeft_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) drawFrame();
		}
		void numFrameTop_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) drawFrame();
		}
		void numFrameWidth_ValueChanged(object sender, EventArgs e)
		{
			if (!_loading) drawFrame();
		}

		void picGalaxy_MouseDown(object sender, MouseEventArgs e)
		{
			int L = (int)numFrameLeft.Value, T = (int)numFrameTop.Value, W = (int)numFrameWidth.Value, H = (int)numFrameHeight.Value;
			if (e.X >= L && e.X <= (L+W) && e.Y >= T && e.Y <= (T+H)) _dragging = true;
		}
		void picGalaxy_MouseMove(object sender, MouseEventArgs e)
		{
			if (_dragging)
			{
				int L, T;
				L = e.X - (int)numFrameWidth.Value / 2;
				T = e.Y - (int)numFrameHeight.Value / 2;
				if (L < 0) L = 0;
				if (T < 0) T = 0;
				if ((L + numFrameWidth.Value) > numFrameLeft.Maximum) L = (int)numFrameLeft.Maximum - (int)numFrameWidth.Value;
				if ((T + numFrameHeight.Value) > numFrameTop.Maximum) T = (int)numFrameTop.Maximum - (int)numFrameHeight.Value;
				numFrameLeft.Value = L;
				numFrameTop.Value = T;
			}
		}
		void picGalaxy_MouseUp(object sender, MouseEventArgs e)
		{
			_dragging = false;
		}

		void tcBattle_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tcBattle.SelectedIndex == 2) drawFrame();
		}
		#endregion
	}
}