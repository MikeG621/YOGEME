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
using static Idmr.Platform.Xwa.Briefing;

namespace Idmr.Yogeme
{
	public partial class XwaLiconForm : Form
	{
		Bitmap _licon;
		Bitmap _canvas;
		readonly List<CraftIconImage> _icons = new List<CraftIconImage>();
		int _zoom;
		float _x, _y;
		bool _mouseDown, _ignoreSBs;
		Point _mousePosition;
		DragMode _dragMode;

		enum DragMode
		{
			None,
			CornerTL,
			CornerBR,
			CornerTR,
			CornerBL,
			Top,
			Left,
			Bottom,
			Right,
			Move
		}

		public XwaLiconForm()
		{
			InitializeComponent();

			string installPath = getInstallPath();
			string bitmapFile = Path.Combine(installPath, "FRONTRES\\MAPICONS\\LICON.BMP");
			string shiplistfile = Path.Combine(installPath, "SHIPLIST.TXT");
			loadBitmap(bitmapFile);
			loadShiplist(shiplistfile);
			_ignoreSBs = true;
			// reminder: hsb has RTL so working with negatives is fine. vsb doesn't have an inverse, so run positive and flip the sign
			hsbIcons.Minimum = (pctLicon.Width - _licon.Width * _zoom) * 4 / _zoom;
			vsbIcons.Maximum = (_licon.Height * _zoom - pctLicon.Height) * 4 / _zoom;
			hsbIcons.Value = 0;
			vsbIcons.Value = 0;
			_ignoreSBs = false;
		}

		bool isBetween(int value, int low, int high) => (value >= low && value <= high);

		Bitmap getIcon(int height, int width, Rectangle iconRect)
		{
			if (height <= 0 || width <= 0) return null;

			Rectangle destRect = new Rectangle(0, 0, width, height);
			Bitmap temp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			using (Graphics tg = Graphics.FromImage(temp))
			{
				tg.InterpolationMode = InterpolationMode.NearestNeighbor;
				tg.DrawImage(_licon, destRect, iconRect, GraphicsUnit.Pixel);
			}
			return temp;
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
			_x = 0;
			_y = 0;
			_zoom = 1;
			_canvas = new Bitmap(_licon);
			updatePct();
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
							icon.Name = tokens[0];

							// Large icon coords. Small icon coords are tokens[5-8], but get replaced by "[9-12] / 2" per xemb
							int.TryParse(tokens[9].Trim(), out int x1);
							int.TryParse(tokens[10].Trim(), out int y1);
							int.TryParse(tokens[11].Trim(), out int x2);
							int.TryParse(tokens[12].Trim(), out int y2);
							int width = x2 - x1 + 1; // per xemb to include R px
							int height = y2 - y1 + 1;
							icon.OriginalRect = new Rectangle(x1, y1, width, height);
							if (width == 0 || height == 0)
							{
								icon.Rect = icon.OriginalRect;
								icon.Icon = null;
								_icons.Add(icon);
								continue;
							}

							icon.Icon = getIcon(height, width, icon.OriginalRect);
							icon.Rect = icon.OriginalRect;
						}
						_icons.Add(icon);
					}
				}
			}
			catch { return false; }
			return true;
		}

		Point mouseToPixel(Point mouse)
		{
			Point px = new Point
			{
				X = (mouse.X - (int)(_x * _zoom / 4)) / _zoom,
				Y = (mouse.Y - (int)(_y * _zoom / 4)) / _zoom
			};
			return px;
		}

		void panToView()
		{
			var icon = _curIcon;
			if (icon == null) return;

			if (_x < -icon.Rect.Left * 4)
				hsbIcons.Value = ((icon.Rect.Left == 0 ? 0 : 1) - icon.Rect.Left) * 4;
			else if (_x > (pctLicon.Width - icon.Rect.Right * _zoom) * 4 / _zoom)
				hsbIcons.Value = (pctLicon.Width - icon.Rect.Right * _zoom - (icon.Rect.Right == _licon.Width ? 0 : 1)) * 4 / _zoom;
			if (_y < -icon.Rect.Top * 4)
				vsbIcons.Value = (icon.Rect.Top - (icon.Rect.Top == 0 ? 0 : 1)) * 4;
			else if (_y > (pctLicon.Height - icon.Rect.Bottom * _zoom) * 4 / _zoom)
				vsbIcons.Value = (icon.Rect.Bottom * _zoom - pctLicon.Height + (icon.Rect.Bottom == _licon.Height ? 0 : 1)) * 4 / _zoom;
		}

		void setSizeIcon(Point px, CraftIconImage icon)
		{
			int tol = 3;
			if (isBetween(px.X, icon.Rect.Left - tol, icon.Rect.Left + tol) && isBetween(px.Y, icon.Rect.Top - tol, icon.Rect.Top + tol))
			{
				Cursor = Cursors.SizeNWSE;
				_dragMode = DragMode.CornerTL;
			}
			else if (isBetween(px.X, icon.Rect.Left + tol, icon.Rect.Right - tol) && isBetween(px.Y, icon.Rect.Top - tol, icon.Rect.Top + tol))
			{
				Cursor = Cursors.SizeNS;
				_dragMode = DragMode.Top;
			}
			else if (isBetween(px.X, icon.Rect.Right - tol, icon.Rect.Right + tol) && isBetween(px.Y, icon.Rect.Top - tol, icon.Rect.Top + tol))
			{
				Cursor = Cursors.SizeNESW;
				_dragMode = DragMode.CornerTR;
			}
			else if (isBetween(px.X, icon.Rect.Right - tol, icon.Rect.Right + tol) && isBetween(px.Y, icon.Rect.Top + tol, icon.Rect.Bottom - tol))
			{
				Cursor = Cursors.SizeWE;
				_dragMode = DragMode.Right;
			}
			else if (isBetween(px.X, icon.Rect.Right - tol, icon.Rect.Right + tol) && isBetween(px.Y, icon.Rect.Bottom - tol, icon.Rect.Bottom + tol))
			{
				Cursor = Cursors.SizeNWSE;
				_dragMode = DragMode.CornerBR;
			}
			else if (isBetween(px.X, icon.Rect.Left + tol, icon.Rect.Right - tol) && isBetween(px.Y, icon.Rect.Bottom - tol, icon.Rect.Bottom + tol))
			{
				Cursor = Cursors.SizeNS;
				_dragMode = DragMode.Bottom;
			}
			else if (isBetween(px.X, icon.Rect.Left - tol, icon.Rect.Left + tol) && isBetween(px.Y, icon.Rect.Bottom - tol, icon.Rect.Bottom + tol))
			{
				Cursor = Cursors.SizeNESW;
				_dragMode = DragMode.CornerBL;
			}
			else if (isBetween(px.X, icon.Rect.Left - tol, icon.Rect.Left + tol) && isBetween(px.Y, icon.Rect.Top + tol, icon.Rect.Bottom - tol))
			{
				Cursor = Cursors.SizeWE;
				_dragMode = DragMode.Left;
			}
			else if (isBetween(px.X, icon.Rect.Left + tol, icon.Rect.Right - tol) && isBetween(px.Y, icon.Rect.Top + tol, icon.Rect.Bottom - tol))
			{
				Cursor = Cursors.SizeAll;
				_dragMode = DragMode.Move;
			}
			else
			{
				Cursor = Cursors.Default;
				_dragMode = DragMode.None;
			}
		}

		void updatePct()
		{
			var g = Graphics.FromImage(_canvas);
			if (lstSpecies.SelectedIndex != -1 && !_icons[lstSpecies.SelectedIndex].Hidden)
			{
				var icon = _curIcon;
				pctIcon.BackgroundImage = icon.Icon;
				Pen outline = new Pen(Color.Yellow);
				int len = 4;
				g.DrawLine(outline, icon.Rect.Left - 1, icon.Rect.Top - 1, icon.Rect.Left - 1 + len, icon.Rect.Top - 1);
				g.DrawLine(outline, icon.Rect.Right, icon.Rect.Top - 1, icon.Rect.Right - len, icon.Rect.Top - 1);
				g.DrawLine(outline, icon.Rect.Left - 1, icon.Rect.Bottom, icon.Rect.Left - 1 + len, icon.Rect.Bottom);
				g.DrawLine(outline, icon.Rect.Right, icon.Rect.Bottom, icon.Rect.Right - len, icon.Rect.Bottom);
				g.DrawLine(outline, icon.Rect.Left - 1, icon.Rect.Top - 1, icon.Rect.Left - 1, icon.Rect.Top - 1 + len);
				g.DrawLine(outline, icon.Rect.Right, icon.Rect.Top - 1, icon.Rect.Right, icon.Rect.Top - 1 + len);
				g.DrawLine(outline, icon.Rect.Left - 1, icon.Rect.Bottom, icon.Rect.Left - 1, icon.Rect.Bottom - len);
				g.DrawLine(outline, icon.Rect.Right, icon.Rect.Bottom, icon.Rect.Right, icon.Rect.Bottom - len);
				// DaTech MapIcon displays L, T, B-1, R-1. This causes overlap all around. Sticking to L-1, T-1, B, R puts the bracket on the outside
			}
			else pctIcon.BackgroundImage = null;
			pctLicon.Invalidate();
		}

		CraftIconImage _curIcon => lstSpecies.SelectedIndex == -1 ? null : _icons[lstSpecies.SelectedIndex];

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
			updatePct();
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
			updatePct();
		}
		private void btnReset_Click(object sender, EventArgs e)
		{
			if (lstSpecies.SelectedIndex == -1) return;

			_curIcon.Rect = _curIcon.OriginalRect;
			_curIcon.Icon = getIcon(_curIcon.Rect.Height, _curIcon.Rect.Width, _curIcon.Rect);
			_canvas = new Bitmap(_licon);
			updatePct();
		}
		private void btnZoomIn_Click(object sender, EventArgs e)
		{
			_zoom *= 2;
			btnZoomOut.Enabled = true;
			if (_zoom == 4) btnZoomIn.Enabled = false;
			_ignoreSBs = true;
			hsbIcons.Minimum = (pctLicon.Width - _licon.Width * _zoom) * 4 / _zoom;
			vsbIcons.Maximum = (_licon.Height * _zoom - pctLicon.Height) * 4 / _zoom;
			hsbIcons.Value = (int)_x;
			vsbIcons.Value = -(int)_y;
			_ignoreSBs = false;
			panToView();
			updatePct();
		}
		private void btnZoomOut_Click(object sender, EventArgs e)
		{
			_zoom /= 2;
			btnZoomIn.Enabled = true;
			if (_zoom == 1) btnZoomOut.Enabled = false;
			_ignoreSBs = true;
			hsbIcons.Minimum = (pctLicon.Width - _licon.Width * _zoom) * 4 / _zoom;
			vsbIcons.Maximum = (_licon.Height * _zoom - pctLicon.Height) * 4 / _zoom;
			if (_x < hsbIcons.Minimum) _x = hsbIcons.Minimum;
			if (_y < -vsbIcons.Maximum) _y = -vsbIcons.Maximum;
			hsbIcons.Value = (int)_x;
			vsbIcons.Value = -(int)_y;
			_ignoreSBs = false;
			updatePct();
		}

		private void hsbIcons_ValueChanged(object sender, EventArgs e)
		{
			if (_ignoreSBs) return;

			_x = hsbIcons.Value;
			updatePct();
		}

		private void lstSpecies_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstSpecies.SelectedIndex == -1)
			{
				lblCoords.Text = "L:000 T:000 R:000 B:000";
				return;
			}
			var icon = _curIcon;
			lblCoords.Text = $"L:{icon.Rect.Left} T:{icon.Rect.Top} R:{icon.Rect.Right} B:{icon.Rect.Bottom}";
			panToView();
			_canvas = new Bitmap(_licon);
			updatePct();
		}

		private void pctLicon_MouseDown(object sender, MouseEventArgs e)
		{
			_mouseDown = true;
			_mousePosition = e.Location;
		}
		private void pctLicon_MouseEnter(object sender, EventArgs e) { if (optMove.Checked) Cursor = Cursors.SizeAll; }
		private void pctLicon_MouseLeave(object sender, EventArgs e) => Cursor = Cursors.Default;
		private void pctLicon_MouseMove(object sender, MouseEventArgs e)
		{
			if (optModify.Checked)
			{
				var icon = _curIcon;
				if (icon == null) return;

				var px = mouseToPixel(e.Location);
				lblDebug.Text = px.ToString();
				if (!_mouseDown)
				{
					setSizeIcon(px, icon);
					return;
				}

				var r = icon.Rect.Right;
				var b = icon.Rect.Bottom;
				var preMod = icon.Rect;
				if (_dragMode == DragMode.CornerTL)
				{
					icon.Rect.X = px.X;
					icon.Rect.Width = r - px.X;
					icon.Rect.Y = px.Y;
					icon.Rect.Height = b - px.Y;
				}
				else if (_dragMode == DragMode.CornerBR)
				{
					icon.Rect.Width = px.X - icon.Rect.Left - 1;
					icon.Rect.Height = px.Y - icon.Rect.Top - 1;
				}
				else if (_dragMode == DragMode.CornerTR)
				{
					icon.Rect.Width = px.X - icon.Rect.Left - 1;
					icon.Rect.Y = px.Y;
					icon.Rect.Height = b - px.Y;
				}
				else if (_dragMode == DragMode.CornerBL)
				{
					icon.Rect.X = px.X;
					icon.Rect.Width = r - px.X;
					icon.Rect.Height = px.Y - icon.Rect.Top - 1;
				}
				else if (_dragMode == DragMode.Left)
				{
					icon.Rect.X = px.X;
					icon.Rect.Width = r - px.X;
				}
				else if (_dragMode == DragMode.Top)
				{
					icon.Rect.Y = px.Y;
					icon.Rect.Height = b - px.Y;
				}
				else if (_dragMode == DragMode.Right) icon.Rect.Width = px.X - icon.Rect.Left - 1;
				else if (_dragMode == DragMode.Bottom) icon.Rect.Height = px.Y - icon.Rect.Top - 1;
				else if (_dragMode == DragMode.Move)
				{
					var oldPx = mouseToPixel(_mousePosition);
					icon.Rect.X += (px.X - oldPx.X);
					icon.Rect.Y += (px.Y - oldPx.Y);
				}
				_mousePosition = e.Location;
				_canvas = new Bitmap(_licon);
				icon.Icon = getIcon(icon.Rect.Height, icon.Rect.Width, icon.Rect);
				if (icon.Icon == null)
				{
					icon.Rect = preMod;
					icon.Icon = getIcon(icon.Rect.Height, icon.Rect.Width, icon.Rect);
				}
				lblCoords.Text = $"L:{icon.Rect.Left} T:{icon.Rect.Top} R:{icon.Rect.Right} B:{icon.Rect.Bottom}";
				updatePct();
				if (!pctLicon.Bounds.Contains(e.Location.X + pctLicon.Left, e.Location.Y + pctLicon.Top))
				{
					_mouseDown = false;
					Cursor = Cursors.Default;
				}
				return;
			}

			if (!_mouseDown) return;

			_x += (e.X - _mousePosition.X) * 4 / _zoom;
			if (_x < hsbIcons.Minimum) _x = hsbIcons.Minimum;
			if (_x > 0) _x = 0;
			_y += (e.Y - _mousePosition.Y) * 4 / _zoom;
			if (_y < -vsbIcons.Maximum) _y = -vsbIcons.Maximum;
			if (_y > 0) _y = 0;
			_mousePosition = e.Location;
			_ignoreSBs = true;
			hsbIcons.Value = (int)_x;
			vsbIcons.Value =  -(int)_y;
			_ignoreSBs = false;
			updatePct();
			if (!pctLicon.Bounds.Contains(e.Location.X + pctLicon.Left, e.Location.Y + pctLicon.Top))
			{
				_mouseDown = false;
				Cursor = Cursors.Default;
			}
		}
		private void pctLicon_MouseUp(object sender, MouseEventArgs e)
		{
			_mouseDown = false;
			updatePct();
		}
		private void pctLicon_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			e.Graphics.ScaleTransform(_zoom, _zoom);
			e.Graphics.DrawImage(_canvas, _x / 4, _y / 4);
		}

		private void vsbIcons_ValueChanged(object sender, EventArgs e)
		{
			if (_ignoreSBs) return;

			_y = -vsbIcons.Value;
			updatePct();
		}

		// Modified version of the BriefingForm2 class
		class CraftIconImage
		{
			public Rectangle OriginalRect;   // Position and dimensions from the original bitmap image it was sourced from
			public Rectangle Rect;           // Current position and dimensions
			public Bitmap Icon;              // The cropped source icon
			public string Name;

			public int Width;
			public int Height;
			public bool Hidden = false;      // For XWA, specifies if the shiplist species entry may be hidden.

			// XWA icons were drawing incorrectly in TFTC 1.3, and getting the best positions and sizes requires knowing the exact information in the ship list.
			public int GetWidth() => OriginalRect.Width + 1;
			public int GetHeight() => OriginalRect.Height + 1;
		}
	}
}
