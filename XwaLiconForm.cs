/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2026 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.18.2+
 *
 * CHANGELOG
 * [NEW] created
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	public partial class XwaLiconForm : Form
	{
		Bitmap _licon;
		List<CraftIconImage> _icons = new List<CraftIconImage>();
		int _x, _y, _zoom;
		bool _mouseDown;

		public XwaLiconForm()
		{
			InitializeComponent();

			string installPath = getInstallPath();
			string bitmapFile = Path.Combine(installPath, "FRONTRES\\MAPICONS\\LICON.BMP");
			string shiplistfile = Path.Combine(installPath, "SHIPLIST.TXT");
			loadBitmap(bitmapFile);
			loadShiplist(shiplistfile);
		}

		string getInstallPath()
		{
			string installPath = CraftDataManager.GetInstance().GetInstallPath();
			if (installPath == "") installPath = Settings.GetInstance().XwaPath;
			return installPath;
		}

		bool loadBitmap(string bitmapFile)
		{
			if (!File.Exists(bitmapFile)) return false;
			try
			{
				_licon = (Bitmap)Image.FromFile(bitmapFile);
				lblLicon.Text = bitmapFile;
			}
			catch { return false; }
			pctLicon.BackgroundImage = _licon;
			// TODO: replace ^ with paint
			_x = 0;
			_y = 0;
			_zoom = 1;
			return true;
		}

		bool loadShiplist(string shiplistFile)
		{
			if (!File.Exists(shiplistFile)) return false;

			try
			{
				using (StreamReader sr = new StreamReader(shiplistFile))
				{
					lstSpecies.Items.Clear();
					_icons.Clear();
					lblShiplist.Text = shiplistFile;
					while (!sr.EndOfStream)
					{
						CraftIconImage icon = new CraftIconImage();

						string s = sr.ReadLine();
						if (s.Length > 0)
						{
							s = s.Replace("\t", "");
							lstSpecies.Items.Add(s);
							int pos = s.LastIndexOf('!');
							if (pos >= 0) s = s.Substring(pos + 1);
						}

						string[] tokens = s.Split(',');
						if (tokens.Length >= 12)
						{
							icon.Hidden = (tokens[0].StartsWith("*") || string.Compare(tokens[1], "Planet/asteroid", StringComparison.OrdinalIgnoreCase) == 0);

							int.TryParse(tokens[9].Trim(), out int x1);
							int.TryParse(tokens[10].Trim(), out int y1);
							int.TryParse(tokens[11].Trim(), out int x2);
							int.TryParse(tokens[12].Trim(), out int y2);
							int width = x2 - x1;
							int height = y2 - y1;
							icon.OriginalRect = new Rectangle(x1, y1, width, height);
							if (width == 0 || height == 0) continue;

							Rectangle destRect = new Rectangle(0, 0, width, height);
							Bitmap temp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
							using (Graphics tg = Graphics.FromImage(temp))
							{
								tg.InterpolationMode = InterpolationMode.NearestNeighbor;
								tg.DrawImage(_licon, destRect, icon.OriginalRect, GraphicsUnit.Pixel);

								icon.Icon = temp;
								icon.Rect = icon.OriginalRect;
							}
						}
						_icons.Add(icon);
					}
				}
			}
			catch { return false; }
			return true;
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			//TODO: save
		}
		private void btnCancel_Click(object sender, EventArgs e) => Close();
		private void btnOK_Click(object sender, EventArgs e)
		{
			btnApply_Click("OK", new EventArgs());
			Close();
		}
		private void btnOpenLicon_Click(object sender, EventArgs e)
		{
			opnFile.FileName = "LICON.BMP";
			opnFile.Filter = "Licon Bitmap|LICON.BMP|All Bitmaps|*.bmp";
			opnFile.InitialDirectory = Path.Combine(getInstallPath(), "FRONTRES\\MAPICONS");
			var res = opnFile.ShowDialog();
			if (res != DialogResult.OK) return;

			loadBitmap(opnFile.FileName);
			lstSpecies.SelectedItem = null;
			// TODO: redraw
		}
		private void btnOpenShiplist_Click(object sender, EventArgs e)
		{
			opnFile.FileName = "SHIPLIST.TXT";
			opnFile.Filter = "Shiplist|SHIPLIST.TXT|All Text Files|*.txt";
			opnFile.InitialDirectory = getInstallPath();
			var res = opnFile.ShowDialog();
			if (res != DialogResult.OK) return;

			loadShiplist(opnFile.FileName);
			lstSpecies.SelectedItem = null;
			// TODO: redraw
		}
		private void btnReset_Click(object sender, EventArgs e)
		{
			if (lstSpecies.SelectedIndex == -1) return;

			var icon = _icons[lstSpecies.SelectedIndex];
			icon.Rect = icon.OriginalRect;
			// TODO: redraw
		}
		private void btnZoomIn_Click(object sender, EventArgs e)
		{
			_zoom *= 2;
			btnZoomOut.Enabled = true;
			if (_zoom == 4) btnZoomIn.Enabled = false;
		}
		private void btnZoomOut_Click(object sender, EventArgs e)
		{
			_zoom /= 2;
			btnZoomIn.Enabled = true;
			if (_zoom == 1) btnZoomOut.Enabled = false;
		}

		private void pctLicon_MouseDown(object sender, MouseEventArgs e) => _mouseDown = true;
		private void pctLicon_MouseEnter(object sender, EventArgs e) { if (optMove.Checked) Cursor = Cursors.Hand; }
		private void pctLicon_MouseLeave(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
			if (_mouseDown)
			{
				_mouseDown = false;
				// TODO: drop where it is.
			}
		}
		private void pctLicon_MouseMove(object sender, MouseEventArgs e)
		{
			if (optModify.Checked)
			{
				// TODO: update cursor with arrows
			}
			// TODO: move
		}

		private void pctLicon_Paint(object sender, PaintEventArgs e)
		{

		}

		private void pctLicon_MouseUp(object sender, MouseEventArgs e)
		{
			_mouseDown = false;
			// TODO: drop
		}

		// Modified version of the BriefingForm2 class
		class CraftIconImage
		{
			public Rectangle OriginalRect;   // Position and dimensions from the original bitmap image it was sourced from
			public Rectangle Rect;           // Current position and dimensions
			public Bitmap Icon;              // The cropped source icon

			public int Width;
			public int Height;
			public bool Hidden = false;      // For XWA, specifies if the shiplist species entry may be hidden.

			// XWA icons were drawing incorrectly in TFTC 1.3, and getting the best positions and sizes requires knowing the exact information in the ship list.
			public int GetWidth() => OriginalRect.Width + 1;
			public int GetHeight() => OriginalRect.Height + 1;
		}
	}
}
