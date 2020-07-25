/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2020 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.6.2+
 */

/* CHANGELOG
 * [UPD] images now use foreground instead of background [JB]
 * [FIX] possible IndexOutOfRange when clicking thumbnails [JB]
 * v1.6.2, 190928
 * [FIX] hook read error due to not ignoring comment/blank lines
 * v1.6, 190915
 * [NEW #26] Backdrop hook implementation
 * [UPD] XWA always resized, not just for SBD
 * [UPD] changed the exception thrown when platform's not installed
 * v1.5, 180910
 * [ADD] catch block for loading XWA thumbnails [JB]
 * [FIX] shadow out of range now resets _shadow in addition to num [JB]
 * v1.4.1, 171118
 * [FIX] added numBackdrop_ValueChanged callout to ctor to ensure loading if _index is 0
 * [UPD] XWA platform check moved to after InitComp due to SBD adjustments
 * [NEW #13] SBD implementation
 * v1.3, 170107
 * [FIX] catch blocks for thumbnails [JB]
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.1.1, 120814
 * - class renamed
 * v1.1, 120715
 * [NEW] XWA custom backdrops loaded for preview
 * v1.0, 110921
 * - Release
 */
using System;
using System.IO;
using System.Windows.Forms;
using Idmr.ImageFormat.Dat;
using Idmr.Platform;

namespace Idmr.Yogeme
{
	/// <summary>Dialog to visually choose backdrop images</summary>
	public partial class BackdropDialog : Form
	{
		// TODO: look into making _planets static, so it only has to initialize the first time each session
		MissionFile.Platform _platform;
		int _index;
		int _shadow = 0;
		string _backdropDirectory = "";
		string _installDirectory = "";
		int _numBackdrops = 0;
		DatFile _planets;
		PictureBox[] thumbs = new PictureBox[103];
		bool _hookInstalled;
		string _fileName;

		/// <summary>The selected Shadow setting</summary>
		/// <remarks>XWA only</remarks>
		public byte Shadow { get { return Convert.ToByte((_shadow != -1 ? _shadow : 255)); } }
		/// <summary>The selected Backdrop value</summary>
		public byte BackdropIndex { get { return Convert.ToByte(_index); } }

		/// <summary>Constructor for TIE and XvT</summary>
		/// <param name="platform">Must be TIE, XvT or BoP</param>
		/// <param name="index">Backdrop index, set to 0 if out of range</param>
		/// <exception cref="ArgumentException">Invalid value for <paramref name="platform"/>.</exception>
		/// <exception cref="ApplicationException">The designated <paramref name="platform"/> installation is not detected.</exception>
		public BackdropDialog(MissionFile.Platform platform, int index)
		{
			_platform = platform;
			if (_platform == MissionFile.Platform.Invalid || _platform == MissionFile.Platform.XWA)
				throw new ArgumentException("Invalid platform, must be TIE, XvT or BoP");
			if (!platformInstalled()) throw new ApplicationException("Platform installation not found, feature unavailable.");
			_index = index;
			if ((_platform == MissionFile.Platform.TIE || _platform == MissionFile.Platform.XvT) && (_index < 0 || _index > 7)) _index = 0;
			if (_platform == MissionFile.Platform.BoP && (_index < 0 || _index > 16)) _index = 0;
			InitializeComponent();
            try  //[JB] Added catch block
            {
				createThumbnails();
            }
            catch
            {
                MessageBox.Show("Failed to load backdrop graphics. The game's files could not be found. Check whether the game is installed and its path has been added to the platform settings.", "Error");
            }
			numBackdrop.Maximum = _numBackdrops - 1;
            numBackdrop.Value = _index;
			numBackdrop_ValueChanged("init", new EventArgs());
		}
		/// <summary>Constructor for XWA</summary>
		/// <param name="index">Backdrop index, set to 0 if out of range</param>
		/// <param name="shadow">Shadow or backdrop variant, set to 0 if out of range</param>
		/// <exception cref="ApplicationException">Platform installation not found.</exception>
		public BackdropDialog(int index, int shadow)
		{
			_platform = MissionFile.Platform.XWA;
			_index = index;
			if (_index < 0 || _index > 103) _index = 0;
			_shadow = shadow;
			if (_shadow < 0 || _shadow > 6) _shadow = 0;
			InitializeComponent();
			if (!platformInstalled()) throw new ApplicationException("Platform installation not found, feature unavailable.");
			createThumbnails();
			vsbThumbs.Enabled = true;
			numBackdrop.Maximum = _numBackdrops - 1;
			numBackdrop.Value = _index;
			numShadow.Enabled = true;
			numShadow.Value = _shadow;
			numBackdrop_ValueChanged("init", new EventArgs());
		}
		/// <summary>Constructor for XWA, Backdrop hook enabled</summary>
		/// <param name="index">Backdrop index, set to 0 if out of range</param>
		/// <param name="shadow">Shadow or backdrop variant, set to 0 if out of range</param>
		/// <param name="fileName">Name of mission for hook implementation</param>
		/// <exception cref="ApplicationException">Platform installation not found.</exception>
		public BackdropDialog(int index, int shadow, string fileName)
		{
			_platform = MissionFile.Platform.XWA;
			_hookInstalled = true;
			_index = index;
			if (_index < 0 || _index > 103) _index = 0;
			_shadow = shadow;
			if (_shadow < 0 || _shadow > 6) _shadow = 0;
			InitializeComponent();
			if (!platformInstalled()) throw new ApplicationException("Platform installation not found, feature unavailable.");
			_fileName = Idmr.Common.StringFunctions.GetFileName(fileName, false);
			if (File.Exists(_installDirectory + "\\Missions\\" + _fileName + "_Resdata.txt")) _fileName = _installDirectory + "\\Missions\\" + _fileName + "_Resdata.txt";
			else _fileName = _installDirectory + "\\Missions\\" + _fileName + ".ini";
			createThumbnails();
			vsbThumbs.Enabled = true;
			numBackdrop.Maximum = _numBackdrops - 1;
			numBackdrop.Value = _index;
			numShadow.Enabled = true;
			numShadow.Value = _shadow;
			numBackdrop_ValueChanged("init", new EventArgs());
		}

		void createThumbnails()
		{
			#region array assignment
			thumbs[0] = thmb0;
			thumbs[1] = thmb1;
			thumbs[2] = thmb2;
			thumbs[3] = thmb3;
			thumbs[4] = thmb4;
			thumbs[5] = thmb5;
			thumbs[6] = thmb6;
			thumbs[7] = thmb7;
			thumbs[8] = thmb8;
			thumbs[9] = thmb9;
			thumbs[10] = thmb10;
			thumbs[11] = thmb11;
			thumbs[12] = thmb12;
			thumbs[13] = thmb13;
			thumbs[14] = thmb14;
			thumbs[15] = thmb15;
			thumbs[16] = thmb16;
			thumbs[17] = thmb17;
			thumbs[18] = thmb18;
			thumbs[19] = thmb19;
			thumbs[20] = thmb20;
			thumbs[21] = thmb21;
			thumbs[22] = thmb22;
			thumbs[23] = thmb23;
			thumbs[24] = thmb24;
			thumbs[25] = thmb25;
			thumbs[26] = thmb26;
			thumbs[27] = thmb27;
			thumbs[28] = thmb28;
			thumbs[29] = thmb29;
			thumbs[30] = thmb30;
			thumbs[31] = thmb31;
			thumbs[32] = thmb32;
			thumbs[33] = thmb33;
			thumbs[34] = thmb34;
			thumbs[35] = thmb35;
			thumbs[36] = thmb36;
			thumbs[37] = thmb37;
			thumbs[38] = thmb38;
			thumbs[39] = thmb39;
			thumbs[40] = thmb40;
			thumbs[41] = thmb41;
			thumbs[42] = thmb42;
			thumbs[43] = thmb43;
			thumbs[44] = thmb44;
			thumbs[45] = thmb45;
			thumbs[46] = thmb46;
			thumbs[47] = thmb47;
			thumbs[48] = thmb48;
			thumbs[49] = thmb49;
			thumbs[50] = thmb50;
			thumbs[51] = thmb51;
			thumbs[52] = thmb52;
			thumbs[53] = thmb53;
			thumbs[54] = thmb54;
			thumbs[55] = thmb55;
			thumbs[56] = thmb56;
			thumbs[57] = thmb57;
			thumbs[58] = thmb58;
			thumbs[59] = thmb59;
			thumbs[60] = thmb60;
			thumbs[61] = thmb61;
			thumbs[62] = thmb62;
			thumbs[63] = thmb63;
			thumbs[64] = thmb64;
			thumbs[65] = thmb65;
			thumbs[66] = thmb66;
			thumbs[67] = thmb67;
			thumbs[68] = thmb68;
			thumbs[69] = thmb69;
			thumbs[70] = thmb70;
			thumbs[71] = thmb71;
			thumbs[72] = thmb72;
			thumbs[73] = thmb73;
			thumbs[74] = thmb74;
			thumbs[75] = thmb75;
			thumbs[76] = thmb76;
			thumbs[77] = thmb77;
			thumbs[78] = thmb78;
			thumbs[79] = thmb79;
			thumbs[80] = thmb80;
			thumbs[81] = thmb81;
			thumbs[82] = thmb82;
			thumbs[83] = thmb83;
			thumbs[84] = thmb84;
			thumbs[85] = thmb85;
			thumbs[86] = thmb86;
			thumbs[87] = thmb87;
			thumbs[88] = thmb88;
			thumbs[89] = thmb89;
			thumbs[90] = thmb90;
			thumbs[91] = thmb91;
			thumbs[92] = thmb92;
			thumbs[93] = thmb93;
			thumbs[94] = thmb94;
			thumbs[95] = thmb95;
			thumbs[96] = thmb96;
			thumbs[97] = thmb97;
			thumbs[98] = thmb98;
			thumbs[99] = thmb99;
			thumbs[100] = thmb100;
			thumbs[101] = thmb101;
			thumbs[102] = thmb102;
			#endregion
			for (int i = 0; i < 103; i++)
			{
				thumbs[i].Tag = i;
				thumbs[i].SizeMode = PictureBoxSizeMode.Zoom;
			}
			// TODO: also look for customs in TIE-BoP
			if (_platform == MissionFile.Platform.TIE || _platform == MissionFile.Platform.XvT)
				for (int i = 1; i <= 8; i++) setThumbnail(_backdropDirectory + "PLANET" + i.ToString() + ".ACT", i - 1);
			else if (_platform == MissionFile.Platform.BoP)
			{
				for (int i = 1; i <= 8; i++) setThumbnail(_backdropDirectory.Remove(_backdropDirectory.Length - 24, 15) + "PLANET" + i.ToString() + ".ACT", i - 1);
				for (int i = 9; i <= 12; i++) setThumbnail(_backdropDirectory + "PLANET" + i.ToString() + ".ACT", i - 1);
				setThumbnail(_backdropDirectory + "SUN1B.ACT", 12);
				setThumbnail(_backdropDirectory + "SUN1C.ACT", 13);
				setThumbnail(_backdropDirectory + "SUN2B.ACT", 14);
				setThumbnail(_backdropDirectory + "SUN3A.ACT", 15);
				setThumbnail(_backdropDirectory + "SUN4A.ACT", 16);
			}
			else
			{
				thumbs[24].Enabled = false;
				_planets = new DatFile();
				_planets.Groups.AutoSort = false;
				setThumbnail("planet.dat", 0, 24);
				_planets.Groups.Add(-1);
				setThumbnail("planet.dat", 25, 35);
				setThumbnail("wrapback.dat", 60, 2);
				setThumbnail("dsfire.dat", 62, 1);
				setThumbnail("nebula.dat", 63, 10);
				setThumbnail("galaxy.dat", 73, 10);
				setThumbnail("backdrop.dat", 83, 11);
				setThumbnail("wrapback.dat", 94, 4);
				setThumbnail("nebula.dat", 98, 5);
				for (int i = 0; i < _planets.NumberOfGroups; i++)
				{
					if (i == 24) continue;
					thumbs[i].Image = _planets.Groups[i].Subs[0].Image;
					thumbs[i].BackColor = System.Drawing.Color.Black;
				}
                StreamReader sr;
				System.Collections.Generic.List<string> resdata = new System.Collections.Generic.List<string>(50);
				string line = "";
				DatFile temp;
				try
                {
					sr = new StreamReader(_installDirectory + "\\RESDATA.TXT");
					while ((line = sr.ReadLine()) != null) if (line != "") resdata.Add(line);
					sr.Close();
				}
                catch
                {
                    MessageBox.Show("Could not open resource file:\n" + _installDirectory + "\\RESDATA.TXT", "Error");
                }
				for (int i = 0; i < resdata.Count - 38; i++)	// 38 original entries, customs must be at top
				{
					try
					{
						temp = new DatFile(_installDirectory + "\\" + resdata[i]);
						int index = 0;
						for (int g = 0; g < temp.NumberOfGroups; g++)
						{
							index = _planets.Groups.GetIndex(temp.Groups[g].ID);
							if (index != -1)
							{
								_planets.Groups[index] = temp.Groups[g];
								thumbs[index].Image = temp.Groups[g].Subs[0].Image;
							}
						}
					}
					catch
					{
						MessageBox.Show("Error reading DAT file from RESDATA.TXT:\n" + _installDirectory + "\\" + resdata[i] + "\nFile skipped.", "Error");
					}
				}
				if (_hookInstalled && File.Exists(_fileName))
				{
					resdata.Clear();
					try
					{
						sr = new StreamReader(_fileName);
						bool readLine = _fileName.EndsWith(".txt");
						while ((line = sr.ReadLine()) != null)
						{
							if (line.StartsWith("#") || line.StartsWith(";") || line.StartsWith("////") || line == "") continue;
							if (line.StartsWith("[")) readLine = false;
							if (readLine) resdata.Add(line);
							else if (line.ToLower() == "[resdata]") readLine = true;
						}
						sr.Close();
					}
					catch
					{
						MessageBox.Show("Could not open hook file:\n" + _fileName, "Error");
					}
					for (int i = 0; i < resdata.Count; i++)
					{
						try
						{
							temp = new DatFile(_installDirectory + "\\" + resdata[i]);
							int index = 0;
							for (int g = 0; g < temp.NumberOfGroups; g++)
							{
								index = _planets.Groups.GetIndex(temp.Groups[g].ID);
								if (index != -1)
								{
									_planets.Groups[index] = temp.Groups[g];
									thumbs[index].Image = temp.Groups[g].Subs[0].Image;
								}
							}
						}
						catch
						{
							MessageBox.Show("Error reading DAT file from hook:\n" + _installDirectory + "\\" + resdata[i] + "\nFile skipped.", "Error");
						}
					}
				}
			}
		}
		/// <summary>TIE and XvT/BoP</summary>
		/// <param name="file">ACT File</param>
		/// <param name="index">Thumbs index</param>
		void setThumbnail(string file, int index)
		{
			ImageFormat.Act.ActImage act = new ImageFormat.Act.ActImage(file);
			thumbs[index].Image = act.Frames[0].Image;
			thumbs[index].BackColor = System.Drawing.Color.Black;
		}
		/// <summary>XWA</summary>
		/// <param name="file">DAT file</param>
		/// <param name="index">Starting thumbs index</param>
		/// <param name="count">Number of groups to read</param>
		void setThumbnail(string file, int index, int count)
		{
            try  //[JB] Added try/catch to generate a more user-friendly message.
            {
			    DatFile temp = new DatFile(_backdropDirectory + file);
			    int offset = 0;
			    if (index == 25) offset = 24;
			    else if (index == 94) offset = 2;
			    else if (index == 98) offset = 10;
			    for (int i = index; i < count+index; i++) _planets.Groups.Add(temp.Groups[i - index + offset]);
            }
            catch
            {
                throw new ArgumentException("Cannot open resource file:\n" + _backdropDirectory + file + "\n\nCheck your platform installation path.");
            }
		}
		bool platformInstalled()
		{
			Settings s = new Settings();
			string dir = "\\IVFILES\\";
			//string file = "SPEC640.LST";
			switch (_platform)
			{
				case MissionFile.Platform.BoP:
					if (!s.BopInstalled) return false;
					_installDirectory = s.BopPath;
					_backdropDirectory = _installDirectory + dir;
					_numBackdrops = 17;	// 8, then 38 lines, then 9
					break;
				case MissionFile.Platform.TIE:
					if (!s.TieInstalled) return false;
					_installDirectory = s.TiePath;
					_backdropDirectory = _installDirectory + dir;
					_numBackdrops = 8;
					break;
				case MissionFile.Platform.XvT:
					if (!s.XvtInstalled) return false;
					_installDirectory = s.XvtPath;
					_backdropDirectory = _installDirectory + dir;
					_numBackdrops = 8;
					break;
				case MissionFile.Platform.XWA:
					if (!s.XwaInstalled) return false;
					_installDirectory = s.XwaPath;
					_backdropDirectory = s.XwaPath + "\\RESDATA\\";
					_numBackdrops = 103;
					// permanently increasing this to 512. Note however that SBD starfield is 2812px, so it'll be cut off
					int size = 256;
					Height += size;
					Width += size;
					pctBackdrop.Height += size;
					pctBackdrop.Width += size;
					label1.Left += size;
					label2.Left += size;
					numBackdrop.Left += size;
					numShadow.Left += size;
					cmdOK.Left += size;
					cmdOK.Top += size;
					cmdCancel.Left += size;
					cmdCancel.Top += size;
					break;
				default:
					return false;
			}
			return true;
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void cmdOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void numShadow_ValueChanged(object sender, EventArgs e)
		{
			pctBackdrop.Image = _planets.Groups[_index].Subs[(int)numShadow.Value].Image;
			_shadow = (int)numShadow.Value;
		}

		private void numBackdrop_ValueChanged(object sender, EventArgs e)
		{
			if (_platform == MissionFile.Platform.XWA)
			{
				try
				{
					if (_shadow >= _planets.Groups[(int)numBackdrop.Value].NumberOfSubs)
                    {
                        _shadow = 0;  //[JB] Need to set shadow or else the catch() will throw an exception since it's out of range.
                        numShadow.Value = 0;
                    }
					pctBackdrop.Image = _planets.Groups[(int)numBackdrop.Value].Subs[_shadow].Image;
					_index = (int)numBackdrop.Value;
					numShadow.Maximum = _planets.Groups[_index].NumberOfSubs - 1;
				}
				catch
				{
					pctBackdrop.Image = _planets.Groups[_index].Subs[_shadow].Image;
					numBackdrop.Value = _index;
				}
			}
			else
			{
				try
				{
					//pctBackdrop.Image = thumbs[(int)numBackdrop.Value].BackgroundImage;
					pctBackdrop.Image = thumbs[(int)numBackdrop.Value].Image;
					_index = (int)numBackdrop.Value;
				}
				catch
				{
					//pctBackdrop.Image = thumbs[_index].BackgroundImage;
					pctBackdrop.Image = thumbs[_index].Image;
					numBackdrop.Value = _index;
				}
			}
		}

		private void thmbArr_Click(object sender, EventArgs e)
		{
			PictureBox p = (PictureBox)sender;
			int index = 0;
			index = (int)p.Tag;
			if (index >= _numBackdrops) return;
			numBackdrop.Value = index;
		}

		private void vsbThumbs_ValueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < 103; i++) thumbs[i].Top = (i / 6 - vsbThumbs.Value) * 48;
		}

		/* according to Allied:
		 * 0: planet - 6010 (158, 9822370, 39961)
		 * 24:
		 * 25: planet - 6060 (241, 8142085, 61129)
		 * 60: wrapback - 18000 (40, 122346, 4538)
		 * 62: dsfire - 6250 (1, 48436, 256)
		 * 63: nebula - 7001 (10, 371287, 2552)
		 * 73: galaxy - 8001 (10, 232962, 2555)
		 * 83: backdrop - 9001 (11, 303385, 2775)
		 * 94: wrapback - 19100 (36, 351737, 5713)
		 * 98: nebula - 7011 (5, 226285, 1276)
		 * 103:
		 */

		/* according to XWA:
		 * 0: planet 6010, MonCal planet is planet
		 * 5: planet 6020, MonCal planet is #-5 no shadow
		 * 24:
		 * 25: planet 6060, MonCal planet is blank
		 * 54: (ISDs, planet 6104, shad0 is blank)
		 * 60: wrapback 18000...
		 */
	}
}
