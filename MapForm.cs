/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2018 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.5
 */

/* CHANGELOG
 * v1.5, 180910
 * [NEW] lots of variables for UI tracking [JB]
 * [NEW] Xwing capability [JB]
 * [NEW] Callbacks in ctors [JB]
 * [UPD] Import moved to after Init [JB]
 * [NEW] chkDistance, hide buttons, listboxes, help button [JB]
 * [UPD] form closing events updated [JB]
 * [UPD] paint bypass if !visible [JB]
 * [NEW] GetIFFColor, GetDrawColor, SwapSelectedItems, UpdateFlightGroup, lots of new map functions [JB]
 * [FIX] waypoint stuff [JB]
 * [NEW] selection box [JB]
 * [NEW] redraw timer [JB]
 * [NEW] MapData.FullName, .Visible, .Selected, .Difficulty [JB]
 * [UPD] keyboard/mouse controls reworked [JB]
 * [NEW] added Deselect() to several controls [JB]
 * [NEW] SelectionData [JB]
 * v1.4, 171016
 * [ADD #11] form is now resizable, can be maximized
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.2, 121006
 * - Settings passed in
 * v1.1.1, 120814
 * [FIX] MapData.Waypoints now based on BaseFlightGroup.BaseWaypoint, now updates back and forth with parent Form
 * - MapPaint() Orientation switch{} removed and condensed
 * - class renamed
 * v1.0, 110921
 * - Release
 */

using Idmr.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>graphical interface for craft waypoints</summary>
	public partial class MapForm : Form
	{
		#region vars and stuff
		int _zoom = 40;
		int w, h, mapX, mapY, mapZ;
		enum Orientation { XY, XZ, YZ };
		Orientation _displayMode = Orientation.XY;
		Bitmap _map;
		MapData[] _mapData;
        List<SelectionData> _selectionList = new List<SelectionData>();
        List<SelectionData> _selectionTmp = new List<SelectionData>();
        List<int> _selectionListFGs = new List<int>();
		int[] _dragIcon = new int[2];	// [0] = fg, [1] = wp
		bool _loading = false;
		CheckBox[] chkWP = new CheckBox[22];
		Settings.Platform _platform;
		//int _wpSetCount = 1;	// assigned, never used
		bool _isDragged;
        //string _lastButtonClicked = "";	// assigned, never used
        bool _leftButtonDown = false;   //Left mouse button currently held down.
        bool _rightButtonDown = false;
        bool _dragSelectState = false;  //True if pressed
        Point _clickPixelDown = new Point(0, 0);  //Form pixel coordinates of mouse click.
        Point _clickPixelUp = new Point(0, 0);
        Point _clickMapDown = new Point(0, 0);    //Virtual map coordinates of mouse click
        Point _clickMapUp = new Point(0, 0);
        Point _dragMapPrevious = new Point(0, 0);  //If dragging continuously, holds the last position so we know how much to update.
        bool _mapFocus = false;
        bool _shiftState = false;
        int _selectionCount = 0;
        bool _norefresh = false;
        bool _mapPaintScheduled = false;      //True if a paint is scheduled, that is a paint request is called while a paint is already in progress.
        //WireframeManager wireframes = new WireframeManager();
        EventHandler onDataModified = null;
        bool _isClosing = false;              //Need a flag during form close to check whether external MapPaint() calls should be ignored.
		#endregion vars

		#region ctors
		/// <param name="fg">XwingFlights array</param>
		public MapForm(Settings settings, Platform.Xwing.FlightGroupCollection fg, EventHandler dataModifiedCallback)
        {
            _platform = Settings.Platform.XWING;
            InitializeComponent();
            Import(fg);
            try { imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_TIE.bmp")); }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            onDataModified = dataModifiedCallback;
            startup(settings);
        }

        /// <param name="fg">TFlights array</param>
        public MapForm(Settings settings, Platform.Tie.FlightGroupCollection fg, EventHandler dataModifiedCallback)
		{
			_platform = Settings.Platform.TIE;
			InitializeComponent();
			Import(fg);
			try { imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_TIE.bmp")); }
			catch(Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
            onDataModified = dataModifiedCallback;
			startup(settings);
		}

		/// <param name="fg">XFlights array</param>
        public MapForm(Settings settings, Platform.Xvt.FlightGroupCollection fg, EventHandler dataModifiedCallback)
		{
			_platform = Settings.Platform.XvT;
			InitializeComponent();
			Import(fg);
			try { imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_XvT.bmp")); }
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
            onDataModified = dataModifiedCallback;
			startup(settings);
		}

		/// <param name="fg">WFlights array</param>
		public MapForm(Settings settings, Platform.Xwa.FlightGroupCollection fg, EventHandler dataModifiedCallback)
		{
			_platform = Settings.Platform.XWA;
			InitializeComponent();
			Import(fg);
			try { imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_XWA.bmp")); }
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
            onDataModified = dataModifiedCallback;
			startup(settings);
		}
		#endregion ctors

		/// <summary>Converts the location on the physical map to a raw craft waypoint (160 raw units = 1 km).</summary>
		void convertMousepointToWaypoint(int mouseX, int mouseY, ref Point pt)
		{
			switch (_displayMode)
			{
				case Orientation.XY:
					pt.X = Convert.ToInt32((mouseX - mapX) / Convert.ToDouble(_zoom) * 160);
					pt.Y = Convert.ToInt32((mapY - mouseY) / Convert.ToDouble(_zoom) * 160);
					break;
				case Orientation.XZ:
					pt.X = Convert.ToInt32((mouseX - mapX) / Convert.ToDouble(_zoom) * 160);
					pt.Y = Convert.ToInt32((mapZ - mouseY) / Convert.ToDouble(_zoom) * 160);
					break;
				case Orientation.YZ:
					pt.X = Convert.ToInt32((mouseX - mapY) / Convert.ToDouble(_zoom) * 160);
					pt.Y = Convert.ToInt32((mapZ - mouseY) / Convert.ToDouble(_zoom) * 160);
					break;
			}
		}

		void deselect()
		{
			_selectionList.Clear();
			_selectionTmp.Clear();
			_selectionCount = 0;
			_selectionListFGs.Clear();
			lstSelected.Items.Clear();
			lstSelected.Visible = false;
		}

		/// <summary>Get the center pixel of pctMap and the coordinates it pertains to</summary>
		/// <returns>A point with the map coordinates in klicks</returns>
		PointF getCenterCoord()
		{
			PointF coord = new PointF();
			switch (_displayMode)
			{
				case Orientation.XY:
					coord.X = (w / 2 - mapX) / Convert.ToSingle(_zoom);
					coord.Y = (mapY - h / 2) / Convert.ToSingle(_zoom);
					break;
				case Orientation.XZ:
					coord.X = (w / 2 - mapX) / Convert.ToSingle(_zoom);
					coord.Y = (mapZ - h / 2) / Convert.ToSingle(_zoom);
					break;
				case Orientation.YZ:
					coord.X = (w / 2 - mapY) / Convert.ToSingle(_zoom);
					coord.Y = (mapZ - h / 2) / Convert.ToSingle(_zoom);
					break;
			}
			return coord;
		}

		string getDistanceString(Platform.BaseFlightGroup.BaseWaypoint wp1, Platform.BaseFlightGroup.BaseWaypoint wp2)
		{
			double xlen = wp1.X - wp2.X;
			double ylen = wp1.Y - wp2.Y;
			double zlen = wp1.Z - wp2.Z;
			double dist = Math.Sqrt((xlen * xlen) + (ylen * ylen) + (zlen * zlen));
			return Math.Round(dist, 2).ToString() + " km";
		}

		Brush getDrawColor(MapData dat)
		{
			Brush brText = SystemBrushes.ControlText;
			switch (dat.IFF)
			{
				case 0:
					brText = Brushes.Lime; //LimeGreen;
					break;
				case 1:
					brText = Brushes.Red; //Crimson;
					break;
				case 2:
					brText = Brushes.DodgerBlue; //RoyalBlue;
					break;
				case 3:
					if (_platform == Settings.Platform.XWING) brText = Brushes.RoyalBlue;
					else if (_platform == Settings.Platform.TIE) brText = Brushes.MediumOrchid;     // FF9932CC
					else brText = Brushes.Yellow;
					break;
				case 4:
					brText = Brushes.OrangeRed; //Red;
					break;
				case 5:
					brText = Brushes.DarkOrchid; //DarkOrchid;
					break;
			}
			if (dat.Difficulty == 6)
				brText = Brushes.Gray;

			return brText;
		}

		Color getIFFColor(int IFF)
		{
			switch (IFF)
			{
				case 0: return Color.Lime;      // FF00FF00  //[JB] Changed colors (was LimeGreen)
				case 1: return Color.Red;       // FFFF0000 (was Crimson)
				case 2: return Color.DodgerBlue;        // FF1E90FF (was Royal Blue)
				case 3:
					if (_platform == Settings.Platform.XWING) return Color.RoyalBlue;   // FF4169E1
					else if (_platform == Settings.Platform.TIE) return Color.MediumOrchid;     // FFBA55D3 (was DarkOrchid)
					else return Color.Yellow;   // FFFFFF00
				case 4: return Color.OrangeRed;         // FFFF4500 (was Red)
				case 5:
					/*if (_platform == Settings.Platform.TIE) return Color.DarkOrchid;			// FF9932CC (was Fuchsia)
                    else*/
					return Color.DarkOrchid;    // FF9932CC
			}

			return Color.White; //Nothing should return this color.
		}

		/// <summary>Take the original image from the craft image strip and adds the RGB values from the craft IFF</summary>
		/// <param name="craftImage">The greyscale craft image</param>
		/// <param name="iff">An array containing the RGB values as per the craft IFF</param>
		/// <returns>Colorized craft image according to IFF</returns>
		Bitmap mask(Bitmap craftImage, byte[] iff)
		{
			// craftImage comes in as 32bppRGB, but we force the image into 24bppRGB with LockBits
			BitmapData bmData = GraphicsFunctions.GetBitmapData(craftImage, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride * bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			for (int y = 0; y < craftImage.Height; y++)
			{
				for (int x = 0, pos = bmData.Stride * y; x < craftImage.Width; x++)
				{
					// stupid thing returns BGR instead of RGB
					pix[pos + x * 3] = (byte)(pix[pos + x * 3] * iff[2] / 255);     // get intensity, apply to IFF mask
					pix[pos + x * 3 + 1] = (byte)(pix[pos + x * 3 + 1] * iff[1] / 255);
					pix[pos + x * 3 + 2] = (byte)(pix[pos + x * 3 + 2] * iff[0] / 255);
				}
			}
			GraphicsFunctions.CopyBytesToImage(pix, bmData);
			craftImage.UnlockBits(bmData);
			craftImage.MakeTransparent(Color.Black);
			return craftImage;
		}

		void moveMapToCursor()
		{
			int offx = (_clickPixelUp.X - _dragMapPrevious.X);
			int offy = (_clickPixelUp.Y - _dragMapPrevious.Y);
			switch (_displayMode)
			{
				case Orientation.XY:
					mapX += offx;
					mapY += offy;
					break;
				case Orientation.XZ:
					mapX += offx;
					mapZ += offy;
					break;
				case Orientation.YZ:
					mapY += offx;
					mapZ += offy;
					break;
			}
			_dragMapPrevious = _clickPixelUp;
		}

		void moveSelectionToCursor()
		{
			if (onDataModified != null) onDataModified(0, new EventArgs());
			int offx = _clickMapUp.X - _dragMapPrevious.X;
			int offy = _clickMapUp.Y - _dragMapPrevious.Y;
			foreach (SelectionData dat in _selectionList)
			{
				switch (_displayMode)
				{
					case Orientation.XY:
						dat.wpRef.RawX += (short)offx;
						dat.wpRef.RawY += (short)offy;
						break;
					case Orientation.XZ:
						dat.wpRef.RawX += (short)offx;
						dat.wpRef.RawZ += (short)offy;
						break;
					case Orientation.YZ:
						dat.wpRef.RawY += (short)offx;
						dat.wpRef.RawZ += (short)offy;
						break;
				}
			}
			_dragMapPrevious = _clickMapUp;
		}

		/// <summary>Converts points to become top-left (<paramref name="a"/>) and bottom-right (<paramref name="b"/>)</summary>
		void normalizePoint(ref Point a, ref Point b)
		{
			if (b.X < a.X)
			{
				int t = a.X;
				a.X = b.X;
				b.X = t;
			}
			if (b.Y < a.Y)
			{
				int t = a.Y;
				a.Y = b.Y;
				b.Y = t;
			}
		}

		void performSelection()
		{
			Point p1 = _clickMapDown;
			Point p2 = _clickMapUp;
			normalizePoint(ref p1, ref p2);

			bool singleSelect = false;
			Point c1 = _clickPixelDown;
			Point c2 = _clickPixelUp;
			normalizePoint(ref c1, ref c2);

			if (c2.X - c1.X <= 5 && c2.Y - c1.Y <= 5)
			{
				double msX = (c2.X - mapX) / Convert.ToDouble(_zoom) * 160;
				double msY = (mapY - c2.Y) / Convert.ToDouble(_zoom) * 160;

				p1.X = (int)msX - 8;
				p1.Y = (int)msY - 8;
				p2.X = (int)msX + 8;
				p2.Y = (int)msY + 8;
				singleSelect = true;
			}

			int ord = 0;
			if (_platform == Settings.Platform.XWA)
				ord = (int)((numRegion.Value - 1) * 4 + numOrder.Value);
			deselect();
			bool[] craftAdded = new bool[_mapData.Length];
			for (int i = 0; i < _mapData.Length; i++)
			{
				if (!_mapData[i].Visible) continue;

				for (int wp = 0; wp < _mapData[i].WPs[0].Length; wp++)
				{
					if (!chkWP[wp].Checked) continue;
					if (!_mapData[i].WPs[0][wp].Enabled) continue;
					if (_platform == Settings.Platform.XWA)
					{
						Platform.Xwa.FlightGroup.Waypoint cwp = (Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[0][wp];
						if (cwp.Region != numRegion.Value - 1) continue;
					}
					if (waypointInside(_mapData[i].WPs[0][wp], p1, p2))
					{
						_selectionList.Add(new SelectionData(i, _mapData[i], _mapData[i].WPs[0][wp]));
						_selectionCount++;
						if (!craftAdded[i])
						{
							craftAdded[i] = true;
							_selectionListFGs.Add(i);
							lstSelected.Items.Add(_mapData[i].FullName);
						}
						if (singleSelect) goto Exit;
					}
				}
				if (ord > 0)
				{
					for (int wp = 0; wp < _mapData[i].WPs[ord].Length; wp++)
					{
						if (!chkWP[4 + wp].Checked) continue;
						if (!_mapData[i].WPs[ord][wp].Enabled) continue;

						if (waypointInside(_mapData[i].WPs[ord][wp], p1, p2))
						{
							_selectionList.Add(new SelectionData(i, _mapData[i], _mapData[i].WPs[ord][wp]));
							_selectionCount++;
							if (!craftAdded[i])
							{
								craftAdded[i] = true;
								_selectionListFGs.Add(i);
								lstSelected.Items.Add(_mapData[i].FullName);
							}
							if (singleSelect) goto Exit;
						}
					}
				}
			}
		Exit:
			int height = 4 + (13 * lstSelected.Items.Count);
			if (lstSelected.Top + height > pctMap.Bottom) height = pctMap.Bottom - lstSelected.Top;

			lstSelected.Height = height;
			lstSelected.Visible = _selectionCount > 0;
			lblQuickHide.Visible = _selectionCount > 0;
		}

		/// <summary>Clears and rebuilds the selection lists.</summary>
		/// <remarks>Useful when reloading externally modified data (since the main editor and map utilize different lists).</remarks>
		void reloadSelectionControls()
		{
			lstSelected.Items.Clear();
			_selectionList.Clear();
			_selectionTmp.Clear();
			_selectionListFGs.Clear();
			lstVisible.Items.Clear();
			for (int i = 0; i < _mapData.Length; i++)
			{
				lstVisible.Items.Add(_mapData[i].FullName);
			}
			lstSelected.Visible = false;
			lblQuickHide.Visible = false;
		}

		/// <summary>Schedules a map redraw using a timer interval.</summary>
		/// <remarks>This saves CPU cycles and produces smoother performance during rapid redraw requests such as when dragging something with the mouse.</remarks>
		void scheduleMapPaint()
		{
			if (!_mapPaintScheduled)
			{
				if (!mapPaintRedrawTimer.Enabled)         //The timer is stopped, start it up.
				{
					//mapPaintRedrawTimer.Interval = 17;    //Was trying to aim for ~60 FPS, but according to MSDN, the minimum accuracy is 55 ms.
					mapPaintRedrawTimer.Start();
				}
				_mapPaintScheduled = true;
			}
		}

		/// <summary>Intialization routine, loads settings and config per platform</summary>
		void startup(Settings config)
		{
			#region checkbox array
			chkWP[0] = chkSP1;
			chkWP[1] = chkSP2;
			chkWP[2] = chkSP3;
			chkWP[3] = chkSP4;
			chkWP[4] = chkWP1;
			chkWP[5] = chkWP2;
			chkWP[6] = chkWP3;
			chkWP[7] = chkWP4;
			chkWP[8] = chkWP5;
			chkWP[9] = chkWP6;
			chkWP[10] = chkWP7;
			chkWP[11] = chkWP8;
			chkWP[12] = chkRDV;
			chkWP[13] = chkHYP;
			chkWP[14] = chkBRF;
			chkWP[15] = chkBRF2;
			chkWP[16] = chkBRF3;
			chkWP[17] = chkBRF4;
			chkWP[18] = chkBRF5;
			chkWP[19] = chkBRF6;
			chkWP[20] = chkBRF7;
			chkWP[21] = chkBRF8;
			for (int i = 0; i < 22; i++)
			{
				chkWP[i].CheckedChanged += new EventHandler(chkWPArr_CheckedChanged);
				chkWP[i].Tag = i;
			}
			#endregion
			updateLayout();
			mapX = w/2;
			mapY = h/2;
			mapZ = h/2;
			_dragIcon[0] = -1;
			_loading = true;
			chkTags.Checked = Convert.ToBoolean(config.MapOptions & Settings.MapOpts.FGTags);
			chkTrace.Checked = Convert.ToBoolean(config.MapOptions & Settings.MapOpts.Traces);
            chkDistance.Enabled = chkTrace.Checked;
			int t = config.Waypoints;
            if (_platform == Settings.Platform.XWING)
            {
                for (int i = 0; i < 3; i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
                chkWP[3].Enabled = false;
                for (int i = 4; i < 7; i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
                for (int i = 7; i < 13; i++) chkWP[i].Enabled = false;
                //[13] = hyper point
                //[14,15,16] CS points, replacing HYP points.
                for (int i = 17; i < 22; i++) chkWP[i].Enabled = false;
                chkBRF.Text = "CS2";
                chkBRF2.Text = "CS3";
                chkBRF3.Text = "CS4";
            }
			if (_platform == Settings.Platform.TIE)
			{
				for (int i = 0; i < 15; i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
				for (int i = 15; i < 22; i++) chkWP[i].Enabled = false;
			}
			else if (_platform == Settings.Platform.XvT) for (int i = 0; i < 22; i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
			else if (_platform == Settings.Platform.XWA)
			{
				for (int i = 0; i < 12; i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
				chkWP[3].Text = "HYP";
				for (int i = 12; i < 22; i++) chkWP[i].Enabled = false;
				lblRegion.Visible = true;
				numRegion.Visible = true;
				lblOrder.Visible = true;
				numOrder.Visible = true;
			}
			MouseWheel += new MouseEventHandler(frmMap_MouseWheel);	// this is here because of some stupid reason, MouseWheel isn't available in the GUI

			_loading = false;
		}

		void swapSelectedItems(int datIndex, bool newState)
		{
			List<SelectionData> rem = _selectionList, add = _selectionTmp;  //Default for True (hiding), removing from main selection
			if (!newState)  //Need to restore from backup
			{
				add = _selectionList;
				rem = _selectionTmp;
			}
			int pos = 0;
			while (pos < rem.Count)
			{
				if (rem[pos].MapDataIndex == datIndex)
				{
					add.Add(rem[pos]);
					rem.RemoveAt(pos);
				}
				else
				{
					pos++;
				}
			}
		}

		/// <summary>Adjust control size/locations</summary>
		void updateLayout()
		{
			PointF center = getCenterCoord();
			pctMap.Width = Width - 102 - 120;  //[JB] Remove left and right margins.
			pctMap.Height = Height - 155;
			w = pctMap.Width;
			h = pctMap.Height;
			if (w < 1 || h < 1) return;
			_map = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			lblCoor1.Top = Height - 59;
			lblCoor2.Top = lblCoor1.Top;
			lblZoom.Top = lblCoor1.Top;
			hscZoom.Top = lblCoor1.Top;
			hscZoom.Width = Width - 498;
			lblRegion.Left = Width - 268;
			numRegion.Left = Width - 218;
			lblOrder.Left = Width - 171;
			numOrder.Left = Width - 129;
			grpDir.Left = Width - 90;
			grpPoints.Left = grpDir.Left;
			chkTags.Left = grpDir.Left;
			chkTrace.Left = grpDir.Left;
            chkDistance.Left = grpDir.Left;
			updateMapCoord(center);
            lstVisible.Height = pctMap.Bottom - lstVisible.Top;
			MapPaint(true);
		}

		/// <summary>Update mapX and mapY</summary>
		/// <param name="coord">The coordinate in klicks to use as the new center</param>
		void updateMapCoord(PointF coord)
		{
			switch (_displayMode)
			{
				case Orientation.XY:
					mapX = Convert.ToInt32(w / 2 - coord.X * Convert.ToSingle(_zoom));
					mapY = Convert.ToInt32(h / 2 + coord.Y * Convert.ToSingle(_zoom));
					break;
				case Orientation.XZ:
					mapX = Convert.ToInt32(w / 2 - coord.X * Convert.ToSingle(_zoom));
					mapZ = Convert.ToInt32(h / 2 + coord.Y * Convert.ToSingle(_zoom));
					break;
				case Orientation.YZ:
					mapY = Convert.ToInt32(w / 2 - coord.X * Convert.ToSingle(_zoom));
					mapZ = Convert.ToInt32(h / 2 + coord.Y * Convert.ToSingle(_zoom));
					break;
			}
			if (mapX / _zoom > 150) mapX = 150 * _zoom;
			if ((mapX - w) / _zoom < -150) mapX = -150 * _zoom + w;
			if (mapY / _zoom > 150) mapY = 150 * _zoom;
			if ((mapY - h) / _zoom < -150) mapY = -150 * _zoom + h;
			if (mapZ / _zoom > 150) mapZ = 150 * _zoom;
			if ((mapZ - h) / _zoom < -150) mapZ = -150 * _zoom + h;
		}

		/// <summary>Determines if a waypoint (raw units) is within a bounding box formed by a top/left and bottom/right point.</summary>
		bool waypointInside(Platform.BaseFlightGroup.BaseWaypoint wp, Point p1, Point p2)
		{
			int tx = wp.RawX;
			int ty = wp.RawY;
			int tz = wp.RawZ;
			switch (_displayMode)
			{
				case Orientation.XY:
					//ty = -ty;
					return ((tx >= p1.X && tx <= p2.X) && (ty >= p1.Y && ty <= p2.Y));
				case Orientation.XZ: return ((tx >= p1.X && tx <= p2.X) && (tz >= p1.Y && tz <= p2.Y));
				case Orientation.YZ: return ((ty >= p1.X && ty <= p2.X) && (tz >= p1.Y && tz <= p2.Y));
			}
			return false;
		}

		#region public functions
		/// <summary>The down-and-dirty function that handles map display </summary>
		/// <param name="persistant">When <b>true</b> draws to memory, <b>false</b> draws directly to the image</param>
		public void MapPaint(bool persistant)
		{
            if(_isClosing) return;
            //_lastMapPaintTime = Environment.TickCount;
            
            if (_loading)
                return;
			#region orientation setup
			int X = mapX, Y = mapZ, coord1 = 0, coord2 = 2;
			switch (_displayMode)
			{
				case Orientation.XY:
					Y = mapY;
					coord2 = 1;
					break;
				case Orientation.YZ:
					X = mapY;
					coord1 = 1;
					break;
			}
			#endregion
			#region brush, pen and graphics setup
			// Create a new pen that we shall use for drawing the lines
			Pen pn = new Pen(Color.DarkRed);		
			SolidBrush sb = new SolidBrush(Color.Black);
			SolidBrush sbg = new SolidBrush(Color.DimGray);	// for FG tags
			Pen pnTrace = new Pen(Color.Gray);		// for WP traces
            Pen pnSel = new Pen(Color.White);  //[JB] For selection boxes
			Graphics g3;
			if (persistant) 
			{
				g3 = Graphics.FromImage(_map);		//graphics obj, load from the memory bitmap
				g3.Clear(SystemColors.Control);		//clear it
			}
			else 
			{
				g3 = pctMap.CreateGraphics();		//paint directly to pict
			}
			#endregion
			#region BG and grid
			g3.FillRectangle(sb, 0, 0, w, h);		//background
			for(int i = 0; i<200; i++)
			{
				g3.DrawLine(pn, 0, _zoom*i + Y, w, _zoom*i + Y);	//min lines, every klick
				g3.DrawLine(pn, 0, Y - _zoom*i, w, Y - _zoom*i);
				g3.DrawLine(pn, _zoom*i + X, 0, _zoom*i + X, h);
				g3.DrawLine(pn, X - _zoom*i, 0,X - _zoom*i, h);
			}
			pn.Width = 3;
			for(int i = 0; i<40; i++)
			{
				g3.DrawLine(pn, 0, _zoom*i*5 + Y, w, _zoom*i*5 + Y);	//maj lines, every 5 klicks
				g3.DrawLine(pn, 0, Y - _zoom*i*5, w, Y - _zoom*i*5);
				g3.DrawLine(pn, _zoom*i*5 + X, 0, _zoom*i*5 + X, h);
				g3.DrawLine(pn, X - _zoom*i*5, 0, X - _zoom*i*5, h);
			}
			pn.Color = Color.Red;
			pn.Width = 1;
			g3.DrawLine(pn, 0, Y, w, Y);	// origin lines
			g3.DrawLine(pn, X, 0, X, h);
			#endregion
			Bitmap bmptemp;
			byte[] bAdd = new byte[3];      // [0] = R, [1] = G, [2] = B
			for (int i = 0; i < _mapData.Length; i++)
			{
				if (!_mapData[i].Visible)
					continue;
				pn.Color = getIFFColor(_mapData[i].IFF);
				bAdd[0] = pn.Color.R;
				bAdd[1] = pn.Color.G;
				bAdd[2] = pn.Color.B;
				bmptemp = new Bitmap(imgCraft.Images[_mapData[i].Craft]);
				bmptemp = mask(bmptemp, bAdd);
				// work through each WP and determine if it needs to be displayed, then place it on the map
				// draw tags if required
				// if previous sequential WP is checked and trace is required, draw trace line according to WP type
				for (int k = 0; k < 4; k++) // Start
				{
					if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled && (_platform == Settings.Platform.XWA ? _mapData[i].WPs[0][k][4] == (short)(numRegion.Value - 1) : true))
					{
						//DrawWireframe(ref g3, _mapData[i], _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y);
						g3.DrawImageUnscaled(bmptemp, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X - 8, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y - 8);
						if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, MapForm.DefaultFont, sbg, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X + 8, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y + 8);
					}
				}
				if (_platform == Settings.Platform.XWA) // WPs     [JB] XWA's north/south is inverted compared to XvT.
				{
					int ord = (int)((numRegion.Value - 1) * 4 + (numOrder.Value - 1) + 1);
					for (int k = 0; k < 8; k++)
					{
						if (chkWP[k + 4].Checked && _mapData[i].WPs[ord][k].Enabled)
						{
							g3.DrawEllipse(pn, _zoom * _mapData[i].WPs[ord][k][coord1] / 160 + X - 1, -_zoom * _mapData[i].WPs[ord][k][coord2] / 160 + Y - 1, 3, 3);
							if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k + 4].Text, DefaultFont, sbg, _zoom * _mapData[i].WPs[ord][k][coord1] / 160 + X + 4, -_zoom * _mapData[i].WPs[ord][k][coord2] / 160 + Y + 4);
							if (chkTrace.Checked)  //[JB] Restructured drawing to expand with distance labels, and fixed index out of range error and incorrect access of hyperspace point.
							{
								Platform.BaseFlightGroup.BaseWaypoint comp = _mapData[i].WPs[0][0];
								if (k == 0 && (!chkWP[0].Checked || (comp[4] != numRegion.Value - 1)))
									continue;
								else if (k > 0)
								{
									if (!chkWP[k + 3].Checked)
										continue;
									comp = _mapData[i].WPs[ord][k - 1];
								}
								g3.DrawLine(pnTrace, _zoom * comp[coord1] / 160 + X, -_zoom * comp[coord2] / 160 + Y, _zoom * _mapData[i].WPs[ord][k][coord1] / 160 + X, -_zoom * _mapData[i].WPs[ord][k][coord2] / 160 + Y);
								if (chkDistance.Checked)
								{
									int offy = chkTags.Checked ? 14 : 0; //To render it below the FG tag
									g3.DrawString(getDistanceString(_mapData[i].WPs[ord][k], comp), DefaultFont, sbg, _zoom * _mapData[i].WPs[ord][k][coord1] / 160 + X + 4, -_zoom * _mapData[i].WPs[ord][k][coord2] / 160 + Y + 4 + offy);
								}
							}
						}
					}
					continue;
				}
				else
				{
					for (int k = 4; k < 12; k++)
					{
						if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled)
						{
							g3.DrawEllipse(pn, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X - 1, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y - 1, 3, 3);
							if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, DefaultFont, sbg, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X + 4, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y + 4);
							if (chkWP[(k == 4 ? 0 : (k - 1))].Checked && chkTrace.Checked)
							{
								int comp = (k == 4 ? 0 : (k - 1));
								g3.DrawLine(pnTrace, _zoom * _mapData[i].WPs[0][comp][coord1] / 160 + X, -_zoom * _mapData[i].WPs[0][comp][coord2] / 160 + Y, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y);
								if (chkDistance.Checked)
								{
									int offy = chkTags.Checked ? 14 : 0; //To render it below the FG tag
									g3.DrawString(getDistanceString(_mapData[i].WPs[0][k], _mapData[i].WPs[0][comp]), DefaultFont, sbg, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X + 4, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y + 4 + offy);
								}
							}
						}
					}
				}
				// remaining are not valid for XWA
				if (chkWP[12].Checked && _mapData[i].WPs[0][12].Enabled) // RND
				{
					g3.DrawEllipse(pn, _zoom * _mapData[i].WPs[0][12][coord1] / 160 + X - 1, -_zoom * _mapData[i].WPs[0][12][coord2] / 160 + Y - 1, 3, 3);
					if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[12].Text, DefaultFont, sbg, _zoom * _mapData[i].WPs[0][12][coord1] / 160 + X + 4, -_zoom * _mapData[i].WPs[0][12][coord2] / 160 + Y + 4);
				}
				if (chkWP[13].Checked && _mapData[i].WPs[0][13].Enabled)    // HYP
				{
					g3.DrawEllipse(pn, _zoom * _mapData[i].WPs[0][13][coord1] / 160 + X - 1, -_zoom * _mapData[i].WPs[0][13][coord2] / 160 + Y - 1, 3, 3);
					if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[13].Text, DefaultFont, sbg, _zoom * _mapData[i].WPs[0][13][coord1] / 160 + X + 4, -_zoom * _mapData[i].WPs[0][13][coord2] / 160 + Y + 4);
					if (chkTrace.Checked)
					{
						// in this case, make sure last visible WP is the last enabled before tracing to HYP
						pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
						for (int k = 4; k < 12; k++)
						{
							if (k != 11)
							{
								if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled && !_mapData[i].WPs[0][k + 1].Enabled)
								{
									g3.DrawLine(pnTrace, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y, _zoom * _mapData[i].WPs[0][13][coord1] / 160 + X, -_zoom * _mapData[i].WPs[0][13][coord2] / 160 + Y);
									break;
								}
							}
							else if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled) g3.DrawLine(pnTrace, _zoom * _mapData[i].WPs[0][11][coord1] / 160 + X, -_zoom * _mapData[i].WPs[0][11][coord2] / 160 + Y, _zoom * _mapData[i].WPs[0][13][coord1] / 160 + X, -_zoom * _mapData[i].WPs[0][13][coord2] / 160 + Y); ;
						}
						pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
					}
				}
				for (int k = 14; k < 22; k++)   // BRF
				{
					if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled)
					{
						g3.DrawImageUnscaled(bmptemp, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X - 8, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y - 8);
						if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, MapForm.DefaultFont, sbg, _zoom * _mapData[i].WPs[0][k][coord1] / 160 + X + 8, -_zoom * _mapData[i].WPs[0][k][coord2] / 160 + Y + 8);
					}
				}
			}
            foreach (SelectionData dat in _selectionList)
            {
                int x = _zoom * dat.wpRef[coord1] / 160 + X;
                int y = -_zoom * dat.wpRef[coord2] / 160 + Y;
                x += 1;  //Doesn't seem to line up with icon correctly, push it over.
                //[JB] Draws a four corner selection box like in-game.
                g3.DrawLine(pnSel, x-8, y-8, x-4, y-8); //Horizontal top
                g3.DrawLine(pnSel, x+4, y-8, x+8, y-8);
                g3.DrawLine(pnSel, x-8, y+8, x-4, y+8); //Horizontal bottom
                g3.DrawLine(pnSel, x+4, y+8, x+8, y+8);
                g3.DrawLine(pnSel, x-8, y-8, x-8, y-4); //Vertical Left
                g3.DrawLine(pnSel, x-8, y+4, x-8, y+8);
                g3.DrawLine(pnSel, x+8, y-8, x+8, y-4); //Vertical right
                g3.DrawLine(pnSel, x+8, y+4, x+8, y+8);
            }
            if (_dragSelectState == true)
            {
                Pen sel = new Pen(Color.White);
                Point p1 = _clickPixelDown;
                Point p2 = _clickPixelUp;
                normalizePoint(ref p1, ref p2);
                Rectangle r = new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
                g3.DrawRectangle(sel, r);
            }
			if (persistant) pctMap.Invalidate();		// since it's drawing to memory, this refreshes the pct.  Removes the flicker when zooming
			g3.Dispose();
		}

        /// <summary>Loads FG data into the MapData class the form uses</summary>
        /// <param name="fg">XwingFlights array</param>
        public void Import(Platform.Xwing.FlightGroupCollection fg)
        {
            int numCraft = fg.Count;
            _mapData = new MapData[numCraft];
            for (int i = 0; i < numCraft; i++)
            {
                //Convert Craft, IFF, and remap the Waypoints to use the TIE formats so that the rest of the map can function correctly.
                _mapData[i] = new MapData(_platform);
                _mapData[i].Craft = fg[i].GetTIECraftType();
                Platform.Xwing.FlightGroup.Waypoint[] arr = new Platform.Xwing.FlightGroup.Waypoint[17];  //TIE has 15 waypoints, need extra to virtualize the Coordinate Set points throught the XvT briefing coords
                for (int j = 0; j < 15; j++)
                    arr[j] = new Platform.Xwing.FlightGroup.Waypoint();
                arr[0] = fg[i].Waypoints[0]; //SP1
                arr[1] = fg[i].Waypoints[4]; //SP2
                arr[2] = fg[i].Waypoints[5]; //SP3
                arr[4] = fg[i].Waypoints[1]; //WP1
                arr[5] = fg[i].Waypoints[2]; //WP2
                arr[6] = fg[i].Waypoints[3]; //WP3
                arr[13] = fg[i].Waypoints[6]; //HYP
                arr[14] = fg[i].Waypoints[7]; //CS1 (BRF1)
                arr[15] = fg[i].Waypoints[8]; //CS2 (BRF2)
                arr[16] = fg[i].Waypoints[9]; //CS3 (BRF3)
                fg[i].Waypoints[7][3] = 1;  //For map purposes, we need to activate these waypoints.
                fg[i].Waypoints[8][3] = 1;
                fg[i].Waypoints[9][3] = 1;
               
                _mapData[i].WPs[0] = arr; // fg[i].Waypoints;
                _mapData[i].IFF = fg[i].GetTIEIFF();
                if (fg[i].IFF == 0 && fg[i].IsObjectGroup()) _mapData[i].IFF = 1; //None/Default objects appear as Imperial.
                _mapData[i].Name = fg[i].Name;
                _mapData[i].FullName = Platform.Tie.Strings.CraftAbbrv[_mapData[i].Craft] + " " + fg[i].Name; //We converted craft to TIE, so load TIE strings.
                if (fg[i].ObjectType >= 58 && fg[i].ObjectType <= 69)
                {
                    _mapData[i].FullName = "TPlt" + (fg[i].ObjectType - 57).ToString();
                }
                if (fg[i].ObjectType >= 26 && fg[i].ObjectType <= 33)  //Asteroids always display as red in game.
                {
                    _mapData[i].IFF = 1;
                }
            }
            reloadSelectionControls();
        }

        /// <summary>Loads FG data into the MapData class the form uses</summary>
		/// <param name="fg">TFlights array</param>
		public void Import(Platform.Tie.FlightGroupCollection fg)
		{
			int numCraft = fg.Count;
			_mapData = new MapData[numCraft];
			for (int i=0;i<numCraft;i++)
			{
				_mapData[i] = new MapData(_platform);
				_mapData[i].Craft = fg[i].CraftType;
				_mapData[i].WPs[0] = fg[i].Waypoints;
				_mapData[i].IFF = fg[i].IFF;
				_mapData[i].Name = fg[i].Name;
                _mapData[i].FullName = Platform.Tie.Strings.CraftAbbrv[_mapData[i].Craft] + " " + fg[i].Name;
            }
            reloadSelectionControls();
		}

		/// <summary>Loads FG data into the MapData class the form uses</summary>
		/// <param name="fg">XFlights array</param>
		public void Import(Platform.Xvt.FlightGroupCollection fg)
		{
			int numCraft = fg.Count;
			_mapData = new MapData[numCraft];
			for (int i=0;i<numCraft;i++)
			{
				_mapData[i] = new MapData(_platform);
				_mapData[i].Craft = fg[i].CraftType;
				_mapData[i].WPs[0] = fg[i].Waypoints;
				_mapData[i].IFF = fg[i].IFF;
				_mapData[i].Name = fg[i].Name;
                _mapData[i].FullName = Platform.Xvt.Strings.CraftAbbrv[_mapData[i].Craft] + " " + fg[i].Name;
            }
            reloadSelectionControls();
		}

		/// <summary>Loads FG data into the MapData class the form uses</summary>
		/// <param name="fg">WFlights array</param>
		public void Import(Platform.Xwa.FlightGroupCollection fg)
		{
			int numCraft = fg.Count;
			_mapData = new MapData[numCraft];
			//_wpSetCount = 17;
			for (int i = 0; i < numCraft; i++)
			{
				_mapData[i] = new MapData(_platform);
				_mapData[i].Craft = fg[i].CraftType;
				_mapData[i].WPs[0] = fg[i].Waypoints;
				for (int j = 0; j < 16; j++)
				{
					int region = j / 4;
					int order = j % 4;
					_mapData[i].WPs[j + 1] = fg[i].Orders[region, order].Waypoints;
				}
				_mapData[i].IFF = fg[i].IFF;
				_mapData[i].Name = fg[i].Name;
				_mapData[i].FullName = Platform.Xwa.Strings.CraftAbbrv[_mapData[i].Craft] + " " + fg[i].Name;
				//_mapData[i].wireframe = wireframes.GetNewActiveInstance(_platform, _mapData[i].Craft);
			}
            reloadSelectionControls();
		}

		/// <summary>Updates the mapdata properties of a FlightGroup.</summary>
		/// <remarks>Should be called by the main program form during minor data adjustments.  For major changes such as FG delete, use <see cref="Import"/> for a complete refresh.</remarks>
		/// <param name="index">Index of the FlightGroup.</param>
		/// <param name="fg">The FlightGroup object to pull information from.  Compatible with all platforms.</param>
		public void UpdateFlightGroup(int index, Platform.BaseFlightGroup fg)
		{
			if (IsDisposed || _isClosing) return;
			if (index < 0 || index >= _mapData.Length || fg == null) return;
			_mapData[index].Name = fg.Name;
			_mapData[index].IFF = fg.IFF;
			_mapData[index].Craft = fg.CraftType;
			_mapData[index].Difficulty = fg.Difficulty;
			string[] abbrev = null;
			switch (_platform)
			{
				case Settings.Platform.XWING:
					Platform.Xwing.FlightGroup xfg = (Platform.Xwing.FlightGroup)fg;
					_mapData[index].Craft = xfg.GetTIECraftType();
					_mapData[index].IFF = xfg.GetTIEIFF();
					if (xfg.IFF == 0 && xfg.IsObjectGroup()) _mapData[index].IFF = 1; //None/Default objects appear as Imperial.
					abbrev = Platform.Tie.Strings.CraftAbbrv;
					if (xfg.ObjectType >= 58 && xfg.ObjectType <= 69)
						_mapData[index].FullName = "TPlt" + (xfg.ObjectType - 57).ToString();
					if (xfg.ObjectType >= 26 && xfg.ObjectType <= 33)  //Asteroids always display as red in game.
						_mapData[index].IFF = 1;
					break;
				case Settings.Platform.TIE:
					abbrev = Platform.Tie.Strings.CraftAbbrv;
					break;
				case Settings.Platform.XvT:
				case Settings.Platform.BoP:
					abbrev = Platform.Xvt.Strings.CraftAbbrv;
					break;
				case Settings.Platform.XWA:
					abbrev = Platform.Xwa.Strings.CraftAbbrv;
					break;
			}
			if (abbrev != null)
				_mapData[index].FullName = abbrev[_mapData[index].Craft] + " " + fg.Name;

			_norefresh = true;   //Don't trigger a SelectedIndexChanged event.
			bool state = lstVisible.GetSelected(index);  //For some reason the selection still changes (probably different event?) so backup/restore state to be sure.
			lstVisible.Items[index] = _mapData[index].FullName;
			lstVisible.Refresh();  //In case IFF changed, force redraw for color change
			lstVisible.SetSelected(index, state);
			for (int i = 0; i < _selectionListFGs.Count; i++)
				if (_selectionListFGs[i] == index)
				{
					state = lstSelected.GetSelected(i);
					lstSelected.Items[i] = _mapData[index].FullName;
					lstSelected.Refresh();  //For IFF
					lstSelected.SetSelected(i, state);
				}
			_norefresh = false;

			scheduleMapPaint();
		}
		#endregion public

		#region controls
		/// <summary>Change the zoom of the map and reset local x/y/z coords as neccessary</summary>
		void hscZoom_ValueChanged(object sender, EventArgs e)
		{
			PointF center = getCenterCoord();
			_zoom = hscZoom.Value;
			updateMapCoord(center);
			scheduleMapPaint(); //MapPaint(true);
			lblZoom.Text = "Zoom: " + _zoom.ToString();
		}

		/// <summary>Rotate map to Top view</summary>
		void optXY_CheckedChanged(object sender, EventArgs e)
		{
			if (optXY.Checked)
			{
				_displayMode = Orientation.XY;
				lblCoor1.Text = "X:";
				lblCoor2.Text = "Y:";
				MapPaint(false);
			}
		}

		/// <summary>Rotate map to Front view</summary>
		void optXZ_CheckedChanged(object sender, EventArgs e)
		{
			if (optXZ.Checked)
			{
				_displayMode = Orientation.XZ;
				lblCoor1.Text = "X:";
				lblCoor2.Text = "Z:";
				MapPaint(false);
			}
		}

		/// <summary>Rotate map to Side view </summary>
		void optYZ_CheckedChanged(object sender, EventArgs e)
		{
			if (optYZ.Checked)
			{
				mapY = w/2 - mapY + h/2;
				_displayMode = Orientation.YZ;
				lblCoor1.Text = "Y:";
				lblCoor2.Text = "Z:";
				MapPaint(false);
			}
			else mapY = w/2 + h/2 - mapY;
		}

		/// <summary>Timer that handles the calling of the actual map rendering, when using TimeRestrictedMapPaint()</summary>
		void mapPaintRedrawTimer_Tick(object sender, EventArgs e)
		{
			if (_mapPaintScheduled)
			{
				MapPaint(true);
				_mapPaintScheduled = false;
				mapPaintRedrawTimer.Stop();
			}
			else  //This shouldn't trigger, but just in case nothing is scheduled, stop the timer.
			{
				mapPaintRedrawTimer.Stop();
			}
		}

		#region pctMap
		void pctMap_MouseDown(object sender, MouseEventArgs e)
		{
            //_lastButtonClicked = e.Button.ToString(); //[JB] Added this to help filter out only left clicks for mouse zooming
			// move map, center on mouse
			if (e.Button.ToString() == "Right")
			{
                _rightButtonDown = true;
                if (!_leftButtonDown)  //To begin right click dragging.
                {
                    _clickPixelDown.X = e.X;
                    _clickPixelDown.Y = e.Y;
                    _clickPixelUp = _clickPixelDown;
                    _dragMapPrevious = _clickPixelUp;
                    Cursor.Current = Cursors.SizeAll;
                }
            }
			else if (e.Button.ToString() == "Left")
			{
                _leftButtonDown = true;

                if (!_rightButtonDown)
                {
                    if (!_dragSelectState) convertMousepointToWaypoint(e.X, e.Y, ref _clickMapDown);

                    if (!_shiftState) _dragSelectState = true;

                    _clickPixelDown.X = e.X;
                    _clickPixelDown.Y = e.Y;
                    convertMousepointToWaypoint(e.X, e.Y, ref _clickMapUp);
                    _dragMapPrevious = _clickMapUp;
                }
			}
			else if (e.Button.ToString() == "Middle")
			{
				mapX = w/2;
				mapY = h/2;
				mapZ = h/2;
				hscZoom.Value = 40;
				MapPaint(false);
			}
		}
        void pctMap_MouseEnter(object sender, EventArgs e) { pctMap.Focus(); _mapFocus = true; }
        void pctMap_MouseLeave(object sender, EventArgs e) { _mapFocus = false; }
        void pctMap_MouseMove(object sender, MouseEventArgs e)
		{
            if (_leftButtonDown)
            {
                _clickPixelUp.X = e.X;
                _clickPixelUp.Y = e.Y;
                convertMousepointToWaypoint(e.X, e.Y, ref _clickMapUp);

                //Calling MapPaint() directly every "frame" of mouse movement when there's a lot of items to draw, will produce a significant amount of slowdown. 
                if (_dragSelectState)
                {
                    scheduleMapPaint(); //Repaint to draw selection box.
                }
                else if (_shiftState && _selectionCount > 0)
                {
                    moveSelectionToCursor();  //Dragging selected items, so move them.
                    scheduleMapPaint(); //Repaint with new positions.
                }
            }
            else if (_rightButtonDown)
            {
                _clickPixelUp.X = e.X;
                _clickPixelUp.Y = e.Y;
                moveMapToCursor();  //Dragging selected items, so move them.
                scheduleMapPaint(); //Repaint with new positions.
            }

			// gets the current mouse location in klicks
			double msX, msY;
			switch (_displayMode)
			{
				case Orientation.XY:
					msX = (e.X - mapX) / Convert.ToDouble(_zoom);
					msY = (mapY - e.Y) / Convert.ToDouble(_zoom);
					lblCoor1.Text = "X: " + Math.Round(msX,2).ToString();
					lblCoor2.Text = "Y: " + Math.Round(msY,2).ToString();  
					break;
				case Orientation.XZ:
					msX = (e.X - mapX) / Convert.ToDouble(_zoom);
					msY = (mapZ - e.Y) / Convert.ToDouble(_zoom);
					lblCoor1.Text = "X: " + Math.Round(msX,2).ToString();
					lblCoor2.Text = "Z: " + Math.Round(msY,2).ToString();
					break;
				case Orientation.YZ:
					msX = (e.X - mapY) / Convert.ToDouble(_zoom);
					msY = (mapZ - e.Y) / Convert.ToDouble(_zoom);
					lblCoor1.Text = "Y: " + Math.Round(msX,2).ToString();
					lblCoor2.Text = "Z: " + Math.Round(msY,2).ToString();
					break;
			}
		}
		void pctMap_MouseUp(object sender, MouseEventArgs e)
		{
            if (e.Button.ToString() == "Left")
            {
                _leftButtonDown = false;
                _clickPixelUp.X = e.X;
                _clickPixelUp.Y = e.Y;
                convertMousepointToWaypoint(e.X, e.Y, ref _clickMapUp);
                _dragSelectState = false;
                if (!_shiftState)
                {
                    performSelection();
                    MapPaint(true);
                }
            }
            else if (e.Button.ToString() == "Right")
            {
                _rightButtonDown = false;
                if(!_leftButtonDown)
                    Cursor.Current = Cursors.Default;  //Doesn't seem to be necessary, but thought it might be good practice to reset to default.
            }
		}
		void pctMap_Paint(object sender, PaintEventArgs e)
		{
			Graphics objGraphics;
			//You can't modify e.Graphics directly.
			objGraphics = e.Graphics;
			// Draw the contents of the bitmap on the form.
			objGraphics.DrawImage(_map, 0, 0, _map.Width, _map.Height);
		}
        void pctMap_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

		/// <summary>Used to determine if mouse click is near a craft waypoint</summary>
		/// <returns>True if num1==(num2 ± 6)</returns>
		bool isApprox(int num1, double num2)
		{
			// +/- 6 is a good enough size
			if (num1 <= (num2+6) && num1 >= (num2-6)) return true;
			else return false;
		}
		#endregion
		#region frmMap
		void frmMap_Activated(object sender, EventArgs e) { MapPaint(true); }
        void frmMap_FormClosed(object sender, FormClosedEventArgs e) { _map.Dispose(); }
        void frmMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            onDataModified = null;  //[JB] Remove the event handler so it doesn't get called.
            _isClosing = true;
            mapPaintRedrawTimer.Stop();
        }
        void frmMap_Load(object sender, EventArgs e)
		{
			_map = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			MapPaint(true);
		}
		void frmMap_MouseWheel(object sender, MouseEventArgs e)
		{
			if (hscZoom.Value < 25 && e.Delta < 0) hscZoom.Value = 5;
			else if (hscZoom.Value > 480 && e.Delta > 0) hscZoom.Value = 500;
			else hscZoom.Value += 10 * Math.Sign(e.Delta);
		}
		void MapForm_Resize(object sender, EventArgs e)
		{
			if (!_isDragged) updateLayout();
		}
		void MapForm_ResizeBegin(object sender, EventArgs e)
		{
			_isDragged = true;
		}
		void MapForm_ResizeEnd(object sender, EventArgs e)
		{
			_isDragged = false;
			updateLayout();
		}

        void MapForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstSelected.Focused)
            {
                if (e.KeyCode == Keys.A || e.KeyCode == Keys.N)
                {
                    bool state = (e.KeyCode == Keys.A ? true : false);
                    _norefresh = true;
                    for (int i = 0; i < lstSelected.Items.Count; i++)
                        lstSelected.SetSelected(i, state);
                    _norefresh = false;
                    lstSelected_SelectedIndexChanged(sender, new EventArgs());
                    e.Handled = true;
                    return;
                }
            }
            if (!_mapFocus) return;

            _shiftState = e.Shift;

            int xOffset = 0;
            int yOffset = 0;
            int amount = 16;             // 16 / 160 = 0.10 km
            if (e.Shift) amount = 2;     // 2 / 160 ~ 0.01 km
            if (e.Control) amount = 40;  // 40 / 160 = 0.25 km
			switch (e.KeyCode)
			{
				case Keys.Up: yOffset = amount; break;
				case Keys.Down: yOffset = -amount; break;
				case Keys.Left: xOffset = -amount; break;
				case Keys.Right: xOffset = amount; break;
			}
            foreach (SelectionData dat in _selectionList)
            {
                switch (_displayMode)
                {
                    case Orientation.XY:
                        dat.wpRef.RawX += (short)xOffset;
                        dat.wpRef.RawY += (short)yOffset;
                        break;
                    case Orientation.XZ:
                        dat.wpRef.RawX += (short)xOffset;
                        dat.wpRef.RawZ += (short)yOffset;
                        break;
                    case Orientation.YZ:
                        dat.wpRef.RawY += (short)xOffset;
                        dat.wpRef.RawZ += (short)yOffset;
                        break;
                }
            }
            if (_selectionList.Count == 1 && !_leftButtonDown && (xOffset != 0 || yOffset != 0)) //Display new coordinates if using the keyboard we're not dragging
            {
                SelectionData dat = _selectionList[0];
                switch (_displayMode)
                {
                    case Orientation.XY:
                        lblCoor1.Text = "New X: " + Math.Round(dat.wpRef.X, 2).ToString();
                        lblCoor2.Text = "New Y: " + Math.Round(dat.wpRef.Y, 2).ToString();
                        break;
                    case Orientation.XZ:
                        lblCoor1.Text = "New X: " + Math.Round(dat.wpRef.X, 2).ToString();
                        lblCoor2.Text = "New Z: " + Math.Round(dat.wpRef.Z, 2).ToString();
                        break;
                    case Orientation.YZ:
                        lblCoor1.Text = "New Y: " + Math.Round(dat.wpRef.Y, 2).ToString();
                        lblCoor2.Text = "New Z: " + Math.Round(dat.wpRef.Z, 2).ToString();
                        break;
                }
            }
            if(_selectionList.Count > 0 && (xOffset != 0 || yOffset != 0) && onDataModified != null)
            {
                onDataModified(0, new EventArgs());
                MapPaint(true);
            }
            e.Handled = true;
        }
        void MapForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (!_mapFocus) return;
            _shiftState = e.Shift;
        }
		#endregion

		#region Checkboxes
		void chkTags_CheckedChanged(object sender, EventArgs e) { if (!_loading) MapPaint(true); }
        void chkTrace_CheckedChanged(object sender, EventArgs e) { if (!_loading) { MapPaint(true); chkDistance.Enabled = chkTrace.Checked; } }
        void chkDistance_CheckedChanged(object sender, EventArgs e) { if (!_loading) MapPaint(true); }
        void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			if ((CheckBox)sender == chkWP[14] && chkWP[14].Checked) for (int i=0;i<14;i++) chkWP[i].Checked = false;
            if (((CheckBox)sender).Checked == false) //[JB] Disabled points might still be selected.
                deselect();
            MapPaint(true);
		}
		#endregion

		#region Selection and visibility
		void lstVisible_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1) return;
			e.DrawBackground();
			Brush brText = getDrawColor(_mapData[e.Index]);
			e.Graphics.DrawString(lstVisible.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstSelected_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1) return;
			e.DrawBackground();
			Brush brText = getDrawColor(_mapData[_selectionListFGs[e.Index]]);
			e.Graphics.DrawString(lstSelected.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstVisible_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_norefresh) return;
			_norefresh = true;
			for (int i = 0; i < _mapData.Length; i++)
			{
				_mapData[i].Visible = true;
				swapSelectedItems(i, false);
			}
			for (int i = 0; i < lstVisible.SelectedIndices.Count; i++)
			{
				_mapData[lstVisible.SelectedIndices[i]].Visible = false;
				swapSelectedItems(lstVisible.SelectedIndices[i], true);
			}
			MapPaint(true);
			_norefresh = false;
		}
		void lstSelected_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_norefresh) return;
			_norefresh = true;
			for (int i = 0; i < lstSelected.Items.Count; i++)
			{
				int datIndex = _selectionListFGs[i];
				bool state = lstSelected.GetSelected(i);
				lstVisible.SetSelected(datIndex, state);
			}
			_norefresh = false;
			lstVisible_SelectedIndexChanged(sender, new EventArgs());
		}
		
		void cmdHideNone_Click(object sender, EventArgs e)
		{
			lstVisible.ClearSelected();
			lstVisible_SelectedIndexChanged(sender, new EventArgs());
		}
		void cmdHideAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < lstVisible.Items.Count; i++)
				lstVisible.SetSelected(i, true);
			lstVisible_SelectedIndexChanged(sender, new EventArgs());
		}
		void cmdHelp_Click(object sender, EventArgs e)
		{
			string text = "Left click: Select points.  Or click+drag to select multiple points." + Environment.NewLine;
			text += "Middle click: Reset map position back to center." + Environment.NewLine;
			text += "Right click: Drag map to scroll." + Environment.NewLine;
			text += Environment.NewLine;
			text += "Craft and waypoints cannot be moved unless selected." + Environment.NewLine;
			text += "Hold the Shift key while Left-click dragging to move all currently selected items." + Environment.NewLine;
			text += "Alternatively, you can move items by pressing the keyboard arrow keys.  Combine with the Shift key to move slower, or Ctrl to move faster." + Environment.NewLine;
			text += Environment.NewLine;
			text += "If the map is too cluttered, toggle visibility on/off by clicking their FGs in the sidebar.  Alternatively, drag-select the area to highlight specific FGs and toggle them off in the 'Hide Selection' box." + Environment.NewLine;
			text += Environment.NewLine;
			text += "To hide all selected FGs, click the 'Hide Selection' box and press 'A' to select all.  Press 'N' to select none and restore visibility." + Environment.NewLine;
			MessageBox.Show(text, "Command Reference");
		}
		#endregion Selection and visibility

		void numOrder_ValueChanged(object sender, EventArgs e) { if (!_loading) { deselect(); MapPaint(true); } }  //[JB] Added deselect
        void numRegion_ValueChanged(object sender, EventArgs e) { if (!_loading) { deselect(); MapPaint(true); } }
		#endregion controls

		struct MapData
		{
			public MapData(Settings.Platform platform)
			{
				Craft = 0;
				IFF = 0;
				Name = "";
                WPs = null;
                FullName = "";
                Visible = true;
                Selected = false;
                Difficulty = 0;
                //wireframe = null;
				switch (platform)
				{
                    case Settings.Platform.XWING:
                        WPs = new Platform.Xwing.FlightGroup.Waypoint[1][];
                        //WPs[0] = new Platform.Xwing.FlightGroup.Waypoint[7];
                        //for(int i = 0; i < WPs[0].Length; i++) WPs[0][i] = new Platform.Xwing.FlightGroup.Waypoint();
                        break;
                    case Settings.Platform.TIE:
						WPs = new Platform.Tie.FlightGroup.Waypoint[1][];
						//WPs[0] = new Platform.Tie.FlightGroup.Waypoint[15];
						//for(int i = 0; i < WPs[0].Length; i++) WPs[0][i] = new Platform.Tie.FlightGroup.Waypoint();
						break;
					case Settings.Platform.XvT:
						WPs = new Platform.Xvt.FlightGroup.Waypoint[1][];
						//WPs[0] = new Platform.Xvt.FlightGroup.Waypoint[22];
						//for (int i = 0; i < WPs[0].Length; i++) WPs[0][i] = new Platform.Xvt.FlightGroup.Waypoint();
						break;
					case Settings.Platform.XWA:
						WPs = new Platform.Xwa.FlightGroup.Waypoint[17][];
						//for (int i = 0; i < x; i++) WPs[i] = new Platform.Xwa.FlightGroup.Waypoint();
						break;
				}
			}

			public int Craft;
			public byte IFF;
			public string Name;
            public string FullName;
            public bool Visible;
            public bool Selected;
            public int Difficulty;
            //public WireframeInstance wireframe;

			public Platform.BaseFlightGroup.BaseWaypoint[][] WPs;
		}

        [Serializable]
        struct SelectionData
        {
            public SelectionData(int index, MapData mapData, Platform.BaseFlightGroup.BaseWaypoint wp)
            {
                MapDataIndex = index;
                MapDataRef = mapData;
                wpRef = wp;
            }
            public int MapDataIndex;
            public MapData MapDataRef;
            public Platform.BaseFlightGroup.BaseWaypoint wpRef;
        }

        
        /*void drawWireframe(ref Graphics g, MapData dat, int x, int y)
        {
            if (dat.wireframe == null) return;
            if (dat.wireframe.def == null) return;

            Platform.BaseFlightGroup.BaseWaypoint dst;

            if(_platform == Settings.Platform.XWA)
            {
                int ord = (int)((numRegion.Value - 1) * 4 + (numOrder.Value - 1) + 1);
                dst = dat.WPs[ord][0];
            }
            else
            {
                dst = dat.WPs[0][4];
            }
            dat.wireframe.SetRotation(dat.WPs[0][0], dst, _zoom, _displayMode);

            Pen body = new Pen(getIFFColor(dat.IFF));
            Pen hangar = new Pen(Color.White);
            Pen dock = new Pen(Color.Yellow);
            Pen p;
            int x1, x2, y1, y2;
            for (int s = 0; s < dat.wireframe.sectionverts.Length; s++)
            {
                string name = dat.wireframe.def.section[s].Name.ToLower();
                if(name.Contains("hangar")) p = hangar;
                else if(name.Contains("dock")) p = dock;
                else p = body;

                if (_displayMode == Orientation.XY)
                {
                    foreach (Line line in dat.wireframe.def.section[s].lines)
                    {
                        x1 = x + (int)(dat.wireframe.sectionverts[s][line.v1].x);
                        x2 = x + (int)(dat.wireframe.sectionverts[s][line.v2].x);
                        y1 = y + (int)(dat.wireframe.sectionverts[s][line.v1].y);
                        y2 = y + (int)(dat.wireframe.sectionverts[s][line.v2].y);
                        g.DrawLine(p, x1, y1, x2, y2);
                    }
                }
                else if (_displayMode == Orientation.XZ)
                {
                    foreach (Line line in dat.wireframe.def.section[s].lines)
                    {
                        x1 = x + (int)(dat.wireframe.sectionverts[s][line.v1].x);
                        x2 = x + (int)(dat.wireframe.sectionverts[s][line.v2].x);
                        y1 = y + (int)(dat.wireframe.sectionverts[s][line.v1].z);
                        y2 = y + (int)(dat.wireframe.sectionverts[s][line.v2].z);
                        g.DrawLine(p, x1, y1, x2, y2);
                    }
                }
                else if (_displayMode == Orientation.YZ)
                {
                    foreach (Line line in dat.wireframe.def.section[s].lines)
                    {
                        x1 = x + (int)(-dat.wireframe.sectionverts[s][line.v1].y);
                        x2 = x + (int)(-dat.wireframe.sectionverts[s][line.v2].y);
                        y1 = y + (int)(dat.wireframe.sectionverts[s][line.v1].z);
                        y2 = y + (int)(dat.wireframe.sectionverts[s][line.v2].z);
                        g.DrawLine(p, x1, y1, x2, y2);
                    }
                }
            }
        }*/
        
        /*Bitmap getBitmap(int craftType)
        {
            if (imgCraft.Images.Count == 0)
            {
                imgCraft = new ImageList();
                switch (_platform)
                {
                    case Settings.Platform.XWING:
                    case Settings.Platform.TIE:
                        //importDat(Application.StartupPath + "\\images\\TIE_BRF.dat", 34);
						imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_TIE.bmp"));
                        break;
                    case Settings.Platform.XvT:
                    case Settings.Platform.BoP:
						//importDat(Application.StartupPath + "\\images\\XvT_BRF.dat", 22);
						imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_XvT.bmp"));
						break;
                    case Settings.Platform.XWA:
						//importDat(Application.StartupPath + "\\images\\XWA_BRF.dat", 56);
						imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_XWA.bmp"));
						break;
                }
			}
            if (craftType < 0 || craftType >= imgCraft.Images.Count)
                return new Bitmap(0, 0);

            return new Bitmap(imgCraft.Images[craftType]);
        }*/

		/*void importDat(string filename, int size)
        {
            try
            {
                FileStream fs = File.OpenRead(filename);
                BinaryReader br = new BinaryReader(fs);
                int count = br.ReadInt16();
                Bitmap bm = new Bitmap(count * size, size, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bm);
                SolidBrush sb = new SolidBrush(Color.Black);
                g.FillRectangle(sb, 0, 0, bm.Width, bm.Height);
                byte[] blue = { 0, 0x48, 0x60, 0x78, 0x94, 0xAC, 0xC8, 0xE0, 0xFC };
                byte[] green = { 0, 0, 4, 0x10, 0x24, 0x3C, 0x58, 0x78, 0xA0 };
                for (int i = 0; i < count; i++)
                {
                    fs.Position = i * 2 + 2;
                    fs.Position = br.ReadUInt16();
                    byte b;
                    w = br.ReadByte();	// using these vars just because I can
                    h = br.ReadByte();
                    int x;
                    for (int q = 0; q < h; q++)
                    {
                        for (int r = 0; r < (w + 1) / 2; r++)
                        {
                            b = br.ReadByte();
                            int p1 = b & 0xF;
                            int p2 = (b & 0xF0) >> 4;
                            x = (size - w) / 2 + size * i + r * 2;
                            if (_platform == Settings.Platform.TIE)
                            {
                                x = size / 2 - w + size * i + r * 4;
                                if (p1 != 0)
                                {
                                    bm.SetPixel(x, size / 2 - h + q * 2, Color.FromArgb(0, green[p1], blue[p1]));
                                    bm.SetPixel(x + 1, size / 2 - h + q * 2, Color.FromArgb(0, green[p1], blue[p1]));
                                    bm.SetPixel(x, size / 2 - h + q * 2 + 1, Color.FromArgb(0, green[p1], blue[p1]));
                                    bm.SetPixel(x + 1, size / 2 - h + q * 2 + 1, Color.FromArgb(0, green[p1], blue[p1]));
                                }
                                if (p2 != 0)
                                {
                                    bm.SetPixel(x + 2, size / 2 - h + q * 2, Color.FromArgb(0, green[p2], blue[p2]));
                                    bm.SetPixel(x + 3, size / 2 - h + q * 2, Color.FromArgb(0, green[p2], blue[p2]));
                                    bm.SetPixel(x + 2, size / 2 - h + q * 2 + 1, Color.FromArgb(0, green[p2], blue[p2]));
                                    bm.SetPixel(x + 3, size / 2 - h + q * 2 + 1, Color.FromArgb(0, green[p2], blue[p2]));
                                }
                            }
                            else if (_platform == Settings.Platform.XvT)
                            {
                                p1 = (p1 != 0 ? (5 - p1) * 0x28 : 0);
                                p2 = (p2 != 0 ? (5 - p2) * 0x28 : 0);
                                if (p1 != 0) bm.SetPixel(x, (size - h) / 2 + q, Color.FromArgb(p1, p1, p1));
                                if (p2 != 0) bm.SetPixel(x + 1, (size - h) / 2 + q, Color.FromArgb(p2, p2, p2));
                            }
                            else
                            {
                                p1 = (p1 != 0 ? p1 * 0x10 + 0xF : 0);
                                p2 = (p2 != 0 ? p2 * 0x10 + 0xF : 0);
                                if (p1 != 0) bm.SetPixel(x, (size - h) / 2 + q, Color.FromArgb(p1, p1, p1));
                                if (p2 != 0) bm.SetPixel(x + 1, (size - h) / 2 + q, Color.FromArgb(p2, p2, p2));
                            }
                        }
                    }
                }
                imgCraft.ImageSize = new Size(size, size);
                imgCraft.Images.AddStrip(bm);
                fs.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }*/
    }
}