/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2025 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.17.3
 *
 * CHANGELOG
 * v1.17.3, 250713
 * [UPD] thumbs is now dynamically sized
 * [UPD] added ability ot use scroll wheel over thumbs
 * [NEW #127] XWA hook backdrops support up to index 255
 * [FIX] Planet2 flag when loaded from RESDATA
 * v1.16, 241013
 * [UPD] cleanup
 * v1.14.1, 230814
 * [FIX #85] Now looks for hook INI or TXT at mission location
 * v1.14, 230804
 * [FIX] Color labels now only show in XWA
 * [NEW #80] Extra Color label pointing out that the color is on clipboard
 * v1.13.9, 220907
 * [NEW #71] Ability to skip repeated DAT load failures
 * [NEW #72] Resdata read will ignore lines commented by ; or //
 * v1.11, 210801
 * [NEW #46] Color picker for XWA
 * v1.8.2, 201219
 * [NEW] Added zoom out abililty for oversized XWA images, with labels to show the sizes
 * [FIX] XWA values are really 1-indexed, so added an empty Group to shift properly
 * [FIX] Added a hook bypass if Planet2 has previously been loaded
 * v1.8.1, 201213
 * [UPD] Settings passed in instead of re-init
 * [UPD] _planets to static so it only inits once
 * [UPD] Replaced StringFunctions.GetFilename() with Path
 * v1.7, 200816
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
using Idmr.ImageFormat.Dat;
using Idmr.Platform;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>Dialog to visually choose backdrop images</summary>
	public partial class BackdropDialog : Form
	{
		readonly MissionFile.Platform _platform;
		int _index;
		int _shadow = 0;
		string _backdropDirectory = "";
		string _installDirectory = "";
		int _numBackdrops = 0;
		static DatFile _planets;
#pragma warning disable IDE1006 // Naming Styles
		readonly PictureBox[] thumbs;
#pragma warning restore IDE1006 // Naming Styles
		readonly bool _hookInstalled;
		readonly string _fileName;
		static bool _planet2Loaded;
		int _mouseX, _mouseY;

		/// <summary>The selected Shadow setting</summary>
		/// <remarks>XWA only</remarks>
		public byte Shadow => Convert.ToByte(_shadow != -1 ? _shadow : 255);
		/// <summary>The selected Backdrop value</summary>
		public byte BackdropIndex => Convert.ToByte(_index);
		/// <summary>The selected Color from the image</summary>
		/// <remarks>XWA only, is copied as rounded "#.## #.## #.##" for use as the backdrop name.</remarks>
		public string Color
		{
			get
			{
				string[] clr = lblColor.Text.Split(' ');
				return Math.Round(decimal.Parse(clr[0]) / 256, 2) + " " + Math.Round(decimal.Parse(clr[1]) / 256, 2) + " " + Math.Round(decimal.Parse(clr[2]) / 256, 2);
			}
		}

		/// <summary>Constructor for TIE and XvT</summary>
		/// <param name="platform">Must be TIE, XvT or BoP</param>
		/// <param name="index">Backdrop index, set to 0 if out of range</param>
		/// <exception cref="ArgumentException">Invalid value for <paramref name="platform"/>.</exception>
		/// <exception cref="ApplicationException">The designated <paramref name="platform"/> installation is not detected.</exception>
		public BackdropDialog(MissionFile.Platform platform, int index)
		{
			_platform = platform;
			if (_platform == MissionFile.Platform.Invalid || _platform == MissionFile.Platform.XWA) throw new ArgumentException("Invalid platform, must be TIE, XvT or BoP");
			if (!platformInstalled()) throw new ApplicationException("Platform installation not found, feature unavailable.");

			_index = index;
			if ((_platform == MissionFile.Platform.TIE || _platform == MissionFile.Platform.XvT) && (_index < 0 || _index > 7)) _index = 0;
			if (_platform == MissionFile.Platform.BoP && (_index < 0 || _index > 16)) _index = 0;
			InitializeComponent();
			if (_planets != null) _planets = null;  // allow GC if you've changed platforms, although that'll only work if you open this again
			thumbs = new PictureBox[_numBackdrops];
			try { createThumbnails(); }
			catch { MessageBox.Show("Failed to load backdrop graphics. The game's files could not be found. Check whether the game is installed and its path has been added to the platform settings.", "Error"); }
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
			if (_index <= 0 || _index > 103 || _index == 25) _index = 1;
			_shadow = shadow;
			if (_shadow < 0 || _shadow > 6) _shadow = 0;
			InitializeComponent();
			if (!platformInstalled()) throw new ApplicationException("Platform installation not found, feature unavailable.");

			thumbs = new PictureBox[_numBackdrops];
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
		/// <param name="filePath">Full path of mission for hook implementation</param>
		/// <exception cref="ApplicationException">Platform installation not found.</exception>
		public BackdropDialog(int index, int shadow, string filePath)
		{
			_platform = MissionFile.Platform.XWA;
			_hookInstalled = true;
			_index = index;
			if (_index <= 0 || _index > 255 || _index == 25) _index = 1;
			_shadow = shadow;
			if (_shadow < 0 || _shadow > 6) _shadow = 0;
			InitializeComponent();
			if (!platformInstalled()) throw new ApplicationException("Platform installation not found, feature unavailable.");

			if (File.Exists(Path.GetFileNameWithoutExtension(filePath) + "_Resdata.txt")) _fileName = Path.GetFileNameWithoutExtension(filePath) + "_Resdata.txt";
			else _fileName = Path.GetFileNameWithoutExtension(filePath) + ".ini";
			thumbs = new PictureBox[_numBackdrops];
			createThumbnails();
			vsbThumbs.Enabled = true;
			vsbThumbs.Maximum = 39;
			numBackdrop.Maximum = _numBackdrops - 1;
			numBackdrop.Value = _index;
			numShadow.Enabled = true;
			numShadow.Value = _shadow;
			numBackdrop_ValueChanged("init", new EventArgs());
		}

		void createThumbnails()
		{
			#region array assignment
			var sz = new Size(48, 48);
			pnlThumbs.SuspendLayout();
			for (int i = 0; i < thumbs.Length; i++)
			{
				var thmb = new PictureBox
				{
					BackColor = System.Drawing.Color.Blue,
					BackgroundImageLayout = ImageLayout.Stretch,
					Location = new Point(i % 6 * 48, i / 6 * 48),
					Size = sz,
					SizeMode = PictureBoxSizeMode.Zoom,
					Tag = i,
				};
				thmb.Click += new EventHandler(thmbArr_Click);
				thmb.MouseWheel += new MouseEventHandler(thmbArr_MouseWheel);
				pnlThumbs.Controls.Add(thmb);
				thumbs[i] = thmb;
			}
			pnlThumbs.ResumeLayout(false);
			#endregion
			// TODO: also look for customs in TIE-BoP
			if (_platform == MissionFile.Platform.TIE || _platform == MissionFile.Platform.XvT)
				for (int i = 0; i < 8; i++) setThumbnail(_backdropDirectory + $"PLANET{i + 1}.ACT", i);
			else if (_platform == MissionFile.Platform.BoP)
			{
				for (int i = 0; i < 8; i++) setThumbnail(_backdropDirectory.Remove(_backdropDirectory.Length - 24, 15) + $"PLANET{i + 1}.ACT", i);
				for (int i = 8; i < 12; i++) setThumbnail(_backdropDirectory + $"PLANET{i + 1}.ACT", i);
				setThumbnail(_backdropDirectory + "SUN1B.ACT", 12);
				setThumbnail(_backdropDirectory + "SUN1C.ACT", 13);
				setThumbnail(_backdropDirectory + "SUN2B.ACT", 14);
				setThumbnail(_backdropDirectory + "SUN3A.ACT", 15);
				setThumbnail(_backdropDirectory + "SUN4A.ACT", 16);
			}
			else
			{
				thumbs[25].Enabled = false;
				if (_planets == null)
				{
					_planets = new DatFile();
					_planets.Groups.AutoSort = false;
					_planets.Groups.Add(-1);
					setThumbnailXwa("planet.dat", 0, 24);
					_planets.Groups.Add(-2);
					setThumbnailXwa("planet.dat", 25, 35);
					setThumbnailXwa("wrapback.dat", 60, 2);
					setThumbnailXwa("dsfire.dat", 62, 1);
					setThumbnailXwa("nebula.dat", 63, 10);
					setThumbnailXwa("galaxy.dat", 73, 10);
					setThumbnailXwa("backdrop.dat", 83, 11);
					setThumbnailXwa("wrapback.dat", 94, 4);
					setThumbnailXwa("nebula.dat", 98, 5);
				}
				for (int i = 1; i < _planets.NumberOfGroups; i++)
				{
					if (i == 25) continue;
					thumbs[i].Image = _planets.Groups[i].Subs[0].Image;
					thumbs[i].BackColor = System.Drawing.Color.Black;
				}
				StreamReader sr;
				System.Collections.Generic.List<string> resdata = new System.Collections.Generic.List<string>(50);
				DatFile temp;
				string line;
				bool ignoreError = false;
				try
				{
					sr = new StreamReader(_installDirectory + "\\RESDATA.TXT");
					while ((line = sr.ReadLine()) != null) if (line != "" && !line.StartsWith(";") && !line.StartsWith("//")) resdata.Add(line);
					sr.Close();
				}
				catch { MessageBox.Show("Could not open resource file:\n" + _installDirectory + "\\RESDATA.TXT", "Error"); }
				for (int i = 0; i < resdata.Count - 38; i++)    // 38 original entries, customs must be at top
				{
					try
					{
						temp = new DatFile(_installDirectory + "\\" + resdata[i]);
						System.Diagnostics.Debug.WriteLine("Loading " + temp.FileName);
						int index = 0;
						for (int g = 0; g < temp.NumberOfGroups; g++)
						{
							short id = temp.Groups[g].ID;
							index = _planets.Groups.GetIndex(id);
							if (index != -1)
							{
								System.Diagnostics.Debug.WriteLine("Overriding Group #" + index);
								_planets.Groups[index] = temp.Groups[g];
								thumbs[index].Image = temp.Groups[g].Subs[0].Image;
							}
							else if (_hookInstalled && id >= 6304 && id <= 6455)
							{
								System.Diagnostics.Debug.WriteLine("Adding hook Group #" + id);
								_planets.Groups.Add(temp.Groups[g]);
								thumbs[id - 6304].Image = temp.Groups[g].Subs[0].Image;
								thumbs[id - 6304].BackColor = System.Drawing.Color.Black;
							}
						}
						if (temp.FileName.ToLower() == "planet2.dat") _planet2Loaded = true;
					}
					catch (Exception x)
					{
						if (!ignoreError)
						{
							string message = "Error reading DAT file from RESDATA.TXT:\n";
							if (x.InnerException != null && x.InnerException.GetType() == typeof(FileNotFoundException)) message = "DAT File not found:\n";
							var dlgError = new ErrorDialog(message + _installDirectory + "\\" + resdata[i] + "\nFile skipped.", true);
							dlgError.ShowDialog();
							ignoreError |= dlgError.IgnoreErrors;
						}
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
						if (resdata.Count == 1 && resdata[0].ToLower() == "resdata\\planet2.dat" && _planet2Loaded)
						{
							System.Diagnostics.Debug.WriteLine("SBD loaded, skipping");
							continue;
						}
						try
						{
							temp = new DatFile(_installDirectory + "\\" + resdata[i]);
							System.Diagnostics.Debug.WriteLine("Loading " + temp.FileName);
							int index = 0;
							for (int g = 0; g < temp.NumberOfGroups; g++)
							{
								short id = temp.Groups[g].ID;
								index = _planets.Groups.GetIndex(id);
								if (index != -1)
								{
									System.Diagnostics.Debug.WriteLine("Overriding Group #" + index);
									_planets.Groups[index] = temp.Groups[g];
									thumbs[index].Image = temp.Groups[g].Subs[0].Image;
								}
								else if (id >= 6304 && id <= 6455)
								{
									System.Diagnostics.Debug.WriteLine("Adding hook Group #" + id);
									_planets.Groups.Add(temp.Groups[g]);
									thumbs[id - 6304].Image = temp.Groups[g].Subs[0].Image;
									thumbs[id - 6304].BackColor = System.Drawing.Color.Black;
								}
							}
							if (temp.FileName.ToLower() == "planet2.dat") _planet2Loaded = true;
						}
						catch { MessageBox.Show("Error reading DAT file from hook:\n" + _installDirectory + "\\" + resdata[i] + "\nFile skipped.", "Error"); }
					}
				}
			}
		}

		/// <summary>TIE and XvT/BoP</summary>
		/// <param name="actFile">ACT File</param>
		/// <param name="index">Thumbs index</param>
		void setThumbnail(string actFile, int index)
		{
			ImageFormat.Act.ActImage act = new ImageFormat.Act.ActImage(actFile);
			thumbs[index].Image = act.Frames[0].Image;
			thumbs[index].BackColor = System.Drawing.Color.Black;
		}
		/// <summary>XWA</summary>
		/// <param name="datFile">DAT file</param>
		/// <param name="index">Starting thumbs index</param>
		/// <param name="count">Number of groups to read</param>
		void setThumbnailXwa(string datFile, int index, int count)
		{
			try
			{
				DatFile temp = new DatFile(_backdropDirectory + datFile);
				int offset = 0;
				if (index == 25) offset = 24;
				else if (index == 94) offset = 2;
				else if (index == 98) offset = 10;
				for (int i = index; i < count + index; i++) _planets.Groups.Add(temp.Groups[i - index + offset]);
			}
			catch { throw new ArgumentException("Cannot open resource file:\n" + _backdropDirectory + datFile + "\n\nCheck your platform installation path."); }
		}
		bool platformInstalled()
		{
			string dir = "\\IVFILES\\";
			var config = Settings.GetInstance();
			//string file = "SPEC640.LST";
			// TODO: guide install off of mission path?
			switch (_platform)
			{
				case MissionFile.Platform.BoP:
					if (!config.BopInstalled) return false;
					_installDirectory = config.BopPath;
					_backdropDirectory = _installDirectory + dir;
					_numBackdrops = 17; // 8, then 38 lines, then 9
					break;
				case MissionFile.Platform.TIE:
					if (!config.TieInstalled) return false;
					_installDirectory = config.TiePath;
					_backdropDirectory = _installDirectory + dir;
					_numBackdrops = 8;
					break;
				case MissionFile.Platform.XvT:
					if (!config.XvtInstalled) return false;
					_installDirectory = config.XvtPath;
					_backdropDirectory = _installDirectory + dir;
					_numBackdrops = 8;
					break;
				case MissionFile.Platform.XWA:
					if (!config.XwaInstalled) return false;
					_installDirectory = config.XwaPath;
					_backdropDirectory = config.XwaPath + "\\RESDATA\\";
					_numBackdrops = (_hookInstalled ? 256 : 104);
					// permanently increasing this to 512. Oversized SBD images now scaled to fit
					int size = 256;
					Height += size;
					Width += size;
					pctBackdrop.Height += size;
					pctBackdrop.Width += size;
					label1.Left += size;
					label2.Left += size;
					label3.Left += size;
					label4.Left += size;
					label5.Left += size;
					label5.Visible = true;
					label6.Left += size;
					label6.Visible = true;
					lblColor.Left += size;
					lblColor.Visible = true;
					pctSample.Left += size;
					pctSample.Visible = true;
					numBackdrop.Left += size;
					numShadow.Left += size;
					cmdOK.Left += size;
					cmdOK.Top += size;
					cmdCancel.Left += size;
					cmdCancel.Top += size;
					lblWindow.Text = "512x512";
					lblWindow.Left += size;
					lblImage.Left += size;
					break;
				default:
					return false;
			}
			return true;
		}

		void cmdCancel_Click(object sender, EventArgs e) => Close();
		void cmdOK_Click(object sender, EventArgs e) => Close();

		void numShadow_ValueChanged(object sender, EventArgs e)
		{
			pctBackdrop.Image = _planets.Groups[_index].Subs[(int)numShadow.Value].Image;
			_shadow = (int)numShadow.Value;
			lblImage.Text = pctBackdrop.Image.Width + "x" + pctBackdrop.Image.Height;
			pctBackdrop.SizeMode = (pctBackdrop.Image.Width > 512 || pctBackdrop.Image.Height > 512) ? PictureBoxSizeMode.Zoom : PictureBoxSizeMode.Normal;
		}

		void numBackdrop_ValueChanged(object sender, EventArgs e)
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
				lblImage.Text = pctBackdrop.Image.Width + "x" + pctBackdrop.Image.Height;
				pctBackdrop.SizeMode = (pctBackdrop.Image.Width > 512 || pctBackdrop.Image.Height > 512) ? PictureBoxSizeMode.Zoom : PictureBoxSizeMode.Normal;
			}
			else
			{
				try
				{
					pctBackdrop.Image = thumbs[(int)numBackdrop.Value].Image;
					_index = (int)numBackdrop.Value;
				}
				catch
				{
					pctBackdrop.Image = thumbs[_index].Image;
					numBackdrop.Value = _index;
				}
				lblImage.Text = thumbs[_index].Image.Width + "x" + thumbs[_index].Image.Height;
			}
		}

		void thmbArr_Click(object sender, EventArgs e)
		{
			PictureBox p = (PictureBox)sender;
			int index = (int)p.Tag;
			if (index >= _numBackdrops) return;
			numBackdrop.Value = index;
		}
		void thmbArr_MouseWheel(object sender, MouseEventArgs e)
		{
			int diff = e.Delta / 120;

			if ((vsbThumbs.Value - diff <= vsbThumbs.Maximum) && (vsbThumbs.Value - diff >= 0))
				vsbThumbs.Value -= diff;
		}

		void vsbThumbs_ValueChanged(object sender, EventArgs e) { for (int i = 0; i < thumbs.Length; i++) thumbs[i].Top = (i / 6 - vsbThumbs.Value) * 48; }

		void pctBackdrop_MouseMove(object sender, MouseEventArgs e)
		{
			if (_platform != MissionFile.Platform.XWA) return;

			int mouseX, mouseY;
			mouseX = e.X;
			mouseY = e.Y;
			if (mouseX >= pctBackdrop.Image.Width || mouseY >= pctBackdrop.Image.Height) return;
			Bitmap img = _planets.Groups[_index].Subs[_shadow].Image;
			System.Diagnostics.Debug.WriteLine("mouse: " + mouseX + " " + mouseY);
			if (img.Width > pctBackdrop.Width || img.Height > pctBackdrop.Height)
			{
				if (img.Width > img.Height)
				{
					// Width is driving the scale, shift the Y
					mouseX = mouseX * img.Width / pctBackdrop.Width;
					mouseY = mouseY * img.Width / pctBackdrop.Width - (img.Width - img.Height) / 2;
				}
				else if (img.Height > img.Width)
				{
					// Height is driving the scale, shift the X
					mouseX = mouseX * img.Height / pctBackdrop.Height - (img.Height - img.Width) / 2;
					mouseY = mouseY * img.Height / pctBackdrop.Height;
				}
				else
				{
					// Even scaling
					mouseX = mouseX * img.Width / pctBackdrop.Width;
					mouseY = mouseY * img.Height / pctBackdrop.Height;
				}
				System.Diagnostics.Debug.WriteLine("new mouse: " + mouseX + " " + mouseY);
				if (mouseX < 0 || mouseX >= img.Width || mouseY < 0 || mouseY >= img.Height) return;
			}
			_mouseX = mouseX;
			_mouseY = mouseY;
			Color color = img.GetPixel(mouseX, mouseY);
			pctSample.BackColor = color;
		}

		void pctBackdrop_Click(object sender, EventArgs e)
		{
			if (_platform != MissionFile.Platform.XWA) return;

			try
			{
				Color color = _planets.Groups[_index].Subs[_shadow].Image.GetPixel(_mouseX, _mouseY);
				lblColor.Text = color.R + " " + color.G + " " + color.B;
			}
			catch { /* do nothing */ }
		}

		/* according to Allied:
		 * 1: planet - 6010 (158, 9822370, 39961)
		 * 25:
		 * 26: planet - 6060 (241, 8142085, 61129)
		 * 61: wrapback - 18000 (40, 122346, 4538)
		 * 63: dsfire - 6250 (1, 48436, 256)
		 * 64: nebula - 7001 (10, 371287, 2552)
		 * 74: galaxy - 8001 (10, 232962, 2555)
		 * 84: backdrop - 9001 (11, 303385, 2775)
		 * 95: wrapback - 19100 (36, 351737, 5713)
		 * 99: nebula - 7011 (5, 226285, 1276)
		 * 104:
		 */

		/* according to XWA:
		 * 1: planet 6010, MonCal planet is planet
		 * 6: planet 6020, MonCal planet is #-5 no shadow
		 * 25:
		 * 26: planet 6060, MonCal planet is blank
		 * 55: (ISDs, planet 6104, shad0 is blank)
		 * 61: wrapback 18000...
		 */

		/* with XWA hooks:
		 * 104-255: groups 6304-6455
		 */
	}
}