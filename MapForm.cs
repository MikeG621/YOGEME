/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2022 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.13.11+
 */

/* CHANGELOG
 * [FIX] Can select disabled XWA SP1, since they're shown
 * [FIX] Selection corners X location
 * [FIX] Removed a leftover debug bypass from the 1.13.10 testing
 * [FIX] XWA Enabled Order Waypoints are no longer displayed if the craft never visits that Region
 * v1.13.11, 221030
 * [FIX] XWA SP1 now always treated as Enabled
 * v1.13.10, 221018
 * [NEW] Correct display of hyper exit points
 * [FIX] Wireframe roll
 * v1.13.5, 220608
 * [FIX] Removed a chunk that could've executed relating to not-yet-implemented changes
 * v1.13.4, 220606
 * [UPD] isVisibleInRegion now returns an enum to denote actually present versus other regions
 * [UPD] moved chkWP[12+] checks in MapPaint to non-XWA block
 * v1.13.3, 220402
 * [FIX] pctMap_Enter stealing focus when window wasn't active
 * v1.13.2, 220319
 * [NEW] checkboxes to toggle wireframes and icon limit
 * [UPD] increased minimum form width
 * [UPD] fixed selection buttons to left side
 * v1.13.1, 220208
 * [UPD] XWA SP3 now labeled "RDV"
 * v1.12, 220103
 * [FIX] Cleared sortedMapDataList on reload to fix multi-delete [JB]
 * [FIX] Listbox scrolling
 * v1.11, 210801
 * [UPD] FGs now also take into account Hyper orders for region visiblity [JB]
 * v1.10, 210520
 * [FIX #58] XWA wireframes when using rotations instead of Waypoints [JB]
 * v1.8, 201004
 * Lots of stuff here, may not list it all...
 * [NEW] Selection expansion [JB]
 * [NEW] New M-click actions [JB]
 * [NEW] Traces can show ETA, speed, throttle. Also, options to control what's shown [JB]
 * [NEW] Wireframes can be faded and hidden [JB]
 * [UPD] Purple IFF now consistently MediumOrchid [JB]
 * [NEW] Snap ability when moving [JB]
 * [NEW] Keyboard commands for WP movement, selection [JB]
 * [NEW] Ability to change WP's region for XWA [JB]
 * [NEW] Ability to show/hide WPs based on Difficulty or IFF [JB]
 * [NEW] Ability to zoom map to fit selected or fit all [JB]
 * [UPD] Map Options save, don't need the Options dialog to change defaults [JB]
 * v1.7, 200816
 * [UPD] MapPaint now always persistent
 * [NEW #12] Wireframe implementation [JB]
 * [UPD] Max zoom and zoom speed adjusted [JB]
 * [UPD] FgIndex added to MapData [JB]
 * [FIX] Unregister Tick handler to prevent misfires after closing [JB]
 * [UPD] form handlers renamed
 * v1.6.5, 200704
 * [NEW] if pulling from imgCraft trips an OutOfRange, default to img[0]
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
using Idmr.Platform;
using Idmr.Yogeme.MapWireframe;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>graphical interface for craft waypoints</summary>
	public partial class MapForm : Form
	{
		#region vars and stuff
		int _zoom = 40;               //Current zoom scale, in pixels per kilometer
#pragma warning disable IDE1006 // Naming Styles
		int w, h, _mapX, _mapY, _mapZ;   //The map vars store offset of the center of the game world (0,0,0) relative to the top left corner of the viewport, taking zoom into account.
#pragma warning restore IDE1006 // Naming Styles
		public enum Orientation { XY, XZ, YZ };
		Orientation _displayMode = Orientation.XY;
		Bitmap _map;
		MapData[] _mapData;
		readonly List<SelectionData> _selectionList = new List<SelectionData>();  // List of all craft and waypoints that are currently selected and itemized in the list control.
		readonly List<int> _sortedMapDataList = new List<int>();  // List of _mapData indices sorted for drawing order.
		readonly int[] _dragIcon = new int[2];   // [0] = fg, [1] = wp
		bool _isLoading = false;
#pragma warning disable IDE1006 // Naming Styles
		readonly CheckBox[] chkWP = new CheckBox[22];
#pragma warning restore IDE1006 // Naming Styles
		readonly Settings.Platform _platform;
		bool _isDragged;
		bool _leftButtonDown = false;     // Left mouse button currently held down.
		bool _rightButtonDown = false;
		bool _dragSelectActive = false;    // Indicates that a drag-select is in progress, and to draw the selection box.
		bool _dragMoveActive = false;      // A drag-move is in progress.
		bool _dragMoveSnapReady = false;  // The starting position for drag-move operation has been assigned, so that snapping can work correctly.
		Point _clickPixelDown = new Point(0, 0);  //Form pixel coordinates of mouse click.
		Point _clickPixelUp = new Point(0, 0);
		Point _clickMapDown = new Point(0, 0);    //Virtual map coordinates of mouse click
		Point _clickMapUp = new Point(0, 0);
		Point _dragMapPrevious = new Point(0, 0);   //If dragging continuously, holds the last position so we know how much to update.
		bool _mapFocus = false;
		bool _shiftState = false;
		bool _ignoreSelectionEvents = false;        // Ignore control events if something is modifying the selection lists.
		int _repeatSelectCount = 0;                 // Notifies the user if they keep trying to select a filtered item.
		int[] _previousCraftSelection = null;       // Maintains which craft items were previously selected, to detect which items were selected or unselected if selection was changed.
		readonly Color _fadeColor = Color.FromArgb(52, 52, 52);   // Icon and wireframe draw color for faded items.
		int _lastSnapUnit = 0;                      // Used to detect if the user has changed the unit of measurement for movement snapping.
		Keys _lastWaypointKeyCode = Keys.None;      // These lastWaypoint values are used to detect keyboard double-taps for the purposes of selecting or creating waypoints via keypress.
		Keys _lastWaypointKeyModifiers = Keys.None;
		int _lastWaypointKeyTime = 0;
		bool _mapPaintScheduled = false;      //True if a paint is scheduled, that is a paint request is called while a paint is already in progress.
		static WireframeManager _wireframeManager = null;
		enum ExpandSelection { Craft, Iff, Size, Invert };
		public enum MiddleClickAction { DoNothing, ResetToCenter, FitToWorld, FitToSelection, CenterOnSelection };
#pragma warning disable IDE1006 // Naming Styles
		EventHandler onDataModified = null;
#pragma warning restore IDE1006 // Naming Styles
		bool _isClosing = false;              //Need a flag during form close to check whether external MapPaint() calls should be ignored.
		Settings _settings = null;
		bool _hasFocus;
		enum WaypointVisibility { Absent, Present, OtherRegion };
		#endregion vars

		#region ctors
		/// <param name="fg">XwingFlights array</param>
		public MapForm(Settings settings, Platform.Xwing.FlightGroupCollection fg, EventHandler dataModifiedCallback)
		{
			_platform = Settings.Platform.XWING;
			InitializeComponent();
			if (settings.XwingOverrideExternal)
			{
				//Since the XW craft list is mapped to use TIE's list for the sake of the map, replace TIE's strings. Must be done prior to import.
				Platform.Tie.Strings.OverrideShipList(null, null);
				string[] xwType = Platform.Xwing.Strings.CraftType;
				string[] xwAbbrev = Platform.Xwing.Strings.CraftAbbrv;
				string[] newType = Platform.Tie.Strings.CraftType;
				string[] newAbbrev = Platform.Tie.Strings.CraftAbbrv;
				Platform.Xwing.FlightGroup temp = new Platform.Xwing.FlightGroup();
				for (int i = 0; i < xwType.Length; i++)
				{
					temp.CraftType = (byte)i;
					int remap = (i == xwType.Length - 1) ? 4 : temp.GetTIECraftType();  //B-wing is special case, last item in XW list. Replace directly.
					if (remap >= 0 && remap < newType.Length)
					{
						newType[remap] = xwType[i];
						newAbbrev[remap] = xwAbbrev[i];
					}
				}
				Platform.Tie.Strings.OverrideShipList(newType, newAbbrev);
			}
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
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
			onDataModified = dataModifiedCallback;
			startup(settings);
		}

		/// <param name="fg">XFlights array</param>
		public MapForm(Settings settings, bool isBoP, Platform.Xvt.FlightGroupCollection fg, EventHandler dataModifiedCallback)
		{
			_platform = isBoP ? Settings.Platform.BoP : Settings.Platform.XvT;
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

        #region private methods
        /// <summary>Converts the location on the physical map to a raw craft waypoint (160 raw units = 1 km).</summary>
        void convertMousepointToWaypoint(int mouseX, int mouseY, ref Point pt)
		{
			switch (_displayMode)
			{
				case Orientation.XY:
					pt.X = Convert.ToInt32((mouseX - _mapX) / Convert.ToDouble(_zoom) * 160);
					pt.Y = Convert.ToInt32((_mapY - mouseY) / Convert.ToDouble(_zoom) * 160);
					break;
				case Orientation.XZ:
					pt.X = Convert.ToInt32((mouseX - _mapX) / Convert.ToDouble(_zoom) * 160);
					pt.Y = Convert.ToInt32((_mapZ - mouseY) / Convert.ToDouble(_zoom) * 160);
					break;
				case Orientation.YZ:
					pt.X = Convert.ToInt32((mouseX - _mapY) / Convert.ToDouble(_zoom) * 160);
					pt.Y = Convert.ToInt32((_mapZ - mouseY) / Convert.ToDouble(_zoom) * 160);
					break;
			}
		}

		void drawCraft(Graphics g, Bitmap bmp, MapData dat, int x, int y)
		{
			WireframeInstance model = _wireframeManager.GetOrCreateWireframeInstance(dat.Craft, dat.FgIndex);

			if (!chkWireframe.Checked || model == null || model.ModelDef == null || (chkLimit.Checked && model.ModelDef.LongestSpanMeters < _settings.WireframeIconThresholdSize))
			{
				g.DrawImageUnscaled(bmp, x - 8, y - 8);
				return;
			}

			//Simple bounds check to determine if it's definitely off screen.
			double calcSpan = (double)model.ModelDef.LongestSpanRaw / 40960 * _zoom;
			int viewSpan = (int)calcSpan;
			if (x + viewSpan < 0 || x - viewSpan > w || y + viewSpan < 0 || y - viewSpan > h)
				return;

			BaseFlightGroup.BaseWaypoint dst;

			int region = -1;
			if (_platform == Settings.Platform.XWA)
			{
				region = (int)numRegion.Value - 1;
				int ord = (int)(region * 4 + numOrder.Value);
				dst = dat.WPs[ord][0];
				if (!dat.WPs[ord][0].Enabled)  // Default to first order if no waypoint defined, this keeps the orientation consistent if checking different orders and they're empty.
					dst = dat.WPs[1][0];
			}
			else
			{
				dst = dat.WPs[0][4];
			}

			int meshZoom = _zoom;
			if (_settings.WireframeMeshIconEnabled && _settings.WireframeMeshIconSize > 0 && calcSpan < _settings.WireframeMeshIconSize)
			{
				if (calcSpan < 0.00001)
					calcSpan = 0.00001;
				double scale = _settings.WireframeMeshIconSize / calcSpan;
				meshZoom = (int)(_zoom * scale);
			}

			if (_platform == Settings.Platform.XWA && !dst.Enabled)
				model.UpdateSimple(meshZoom, _settings.WireframeMeshTypeVisibility, dat.Yaw, dat.Pitch, dat.Roll);
			else if (_platform == Settings.Platform.XWA && dat.WPs[17][region].Enabled && ((Platform.Xwa.FlightGroup.Waypoint)dat.WPs[0][0]).Region != region)
				model.UpdateParams(dat.WPs[17][region], dat.WPs[18][region], meshZoom, _displayMode, _settings.WireframeMeshTypeVisibility, dat.Roll);
			else
				model.UpdateParams(dat.WPs[0][0], dst, meshZoom, _displayMode, _settings.WireframeMeshTypeVisibility, dat.Roll);

			Pen body = new Pen(dat.View == Visibility.Fade ? _fadeColor : getIFFColor(dat.IFF));
			Pen hangar = new Pen(dat.View == Visibility.Fade ? _fadeColor : Color.White);
			Pen dock = new Pen(dat.View == Visibility.Fade ? _fadeColor : Color.Yellow);
			Pen p;
			int x1, x2, y1, y2;
			int lineDrawCount = 0;
			foreach (MeshLayerInstance layer in model.LayerInstances)
			{
				if (layer.MatchMeshFilter(_settings.WireframeMeshTypeVisibility))
				{
					p = body;
					MeshType mt = layer.MeshLayerDefinition.MeshType;
					if (mt == MeshType.Hangar)
						p = hangar;
					else if (mt == MeshType.DockingPlatform || mt == MeshType.LandingPlatform)
						p = dock;
					lineDrawCount += layer.MeshLayerDefinition.Lines.Count;
					if (_displayMode == Orientation.XY)
					{
						foreach (Line line in layer.MeshLayerDefinition.Lines)
						{
							x1 = x + (int)layer.Vertices[line.V1].X;
							x2 = x + (int)layer.Vertices[line.V2].X;
							y1 = y + (int)layer.Vertices[line.V1].Y;
							y2 = y + (int)layer.Vertices[line.V2].Y;
							g.DrawLine(p, x1, y1, x2, y2);
						}
					}
					else if (_displayMode == Orientation.XZ)
					{
						foreach (Line line in layer.MeshLayerDefinition.Lines)
						{
							x1 = x + (int)layer.Vertices[line.V1].X;
							x2 = x + (int)layer.Vertices[line.V2].X;
							y1 = y + (int)layer.Vertices[line.V1].Z;
							y2 = y + (int)layer.Vertices[line.V2].Z;
							g.DrawLine(p, x1, y1, x2, y2);
						}
					}
					else if (_displayMode == Orientation.YZ)
					{
						foreach (Line line in layer.MeshLayerDefinition.Lines)
						{
							x1 = x + (int)-layer.Vertices[line.V1].Y;  // Hmm, they were the wrong direction.
							x2 = x + (int)-layer.Vertices[line.V2].Y;
							y1 = y + (int)layer.Vertices[line.V1].Z;
							y2 = y + (int)layer.Vertices[line.V2].Z;
							g.DrawLine(p, x1, y1, x2, y2);
						}
					}
				}
			}
			//If we didn't draw anything at all (empty model, or none matched the mesh filter) then our failsafe is a normal icon just so it's not invisible.
			//For larger models that appear large enough on screen, draw a pip at the origin so the user knows where the selection point is.
			if (lineDrawCount == 0)
				g.DrawImageUnscaled(bmp, x - 8, y - 8);
			else if (model.ModelDef.LongestSpanMeters > 30 && viewSpan > 32)
				g.DrawEllipse(hangar, x - 1, y - 1, 2, 2);
		}

		/// <summary>Get the center pixel of pctMap and the coordinates it pertains to</summary>
		/// <returns>A point with the map coordinates in klicks</returns>
		PointF getCenterCoord()
		{
			PointF coord = new PointF();
			switch (_displayMode)
			{
				case Orientation.XY:
					coord.X = (w / 2 - _mapX) / Convert.ToSingle(_zoom);
					coord.Y = (_mapY - h / 2) / Convert.ToSingle(_zoom);
					break;
				case Orientation.XZ:
					coord.X = (w / 2 - _mapX) / Convert.ToSingle(_zoom);
					coord.Y = (_mapZ - h / 2) / Convert.ToSingle(_zoom);
					break;
				case Orientation.YZ:
					coord.X = (w / 2 - _mapY) / Convert.ToSingle(_zoom);
					coord.Y = (_mapZ - h / 2) / Convert.ToSingle(_zoom);
					break;
			}
			return coord;
		}

		string getDistanceString(BaseFlightGroup.BaseWaypoint wp1, BaseFlightGroup.BaseWaypoint wp2)
		{
			double xlen = wp1.X - wp2.X;
			double ylen = wp1.Y - wp2.Y;
			double zlen = wp1.Z - wp2.Z;
			double dist = Math.Sqrt((xlen * xlen) + (ylen * ylen) + (zlen * zlen));
			return Math.Round(dist, 2).ToString() + " km";
		}

		string getTimeString(object fg, int orderIndex, BaseFlightGroup.BaseWaypoint wp1, BaseFlightGroup.BaseWaypoint wp2)
		{
			double xlen = wp1.X - wp2.X;
			double ylen = wp1.Y - wp2.Y;
			double zlen = wp1.Z - wp2.Z;
			double dist = Math.Sqrt((xlen * xlen) + (ylen * ylen) + (zlen * zlen));
			int command = 0;
			int throttle = 0;
			int speed = 0;
			switch (_platform)
			{
				case Settings.Platform.XWING:
					command = ((Platform.Xwing.FlightGroup)fg).Order;
					throttle = ((Platform.Xwing.FlightGroup)fg).DockTimeThrottle;
					if (throttle == 0) throttle = 100;
					else if (throttle <= 9) throttle = (throttle + 1) * 10;
					else throttle = 0;
					if (command == 0 || command == 0xC || (command >= 0x1D && command <= 0x20))  // Hold Steady, Disabled, SS Await (Return, Launch, Boarding, Arrival)
						throttle = 0;
					break;
				case Settings.Platform.TIE:
					if (orderIndex > 2)
						orderIndex = 2;
					command = ((Platform.Tie.FlightGroup)fg).Orders[orderIndex].Command;
					throttle = ((Platform.Tie.FlightGroup)fg).Orders[orderIndex].Throttle * 10;
					break;
				case Settings.Platform.XvT:
				case Settings.Platform.BoP:
					command = ((Platform.Xvt.FlightGroup)fg).Orders[orderIndex].Command;
					throttle = ((Platform.Xvt.FlightGroup)fg).Orders[orderIndex].Throttle * 10;
					speed = ((Platform.Xvt.FlightGroup)fg).Orders[orderIndex].Speed;
					break;
				case Settings.Platform.XWA:
					command = ((Platform.Xwa.FlightGroup)fg).Orders[(int)numRegion.Value - 1, orderIndex].Command;
					throttle = ((Platform.Xwa.FlightGroup)fg).Orders[(int)numRegion.Value - 1, orderIndex].Throttle * 10;
					speed = ((Platform.Xwa.FlightGroup)fg).Orders[(int)numRegion.Value - 1, orderIndex].Speed;
					break;
			}
			if (_platform != Settings.Platform.XWING)
			{
				switch (command)
				{
					case 0:     // Hold Steady
					case 0x5:   // Disabled
					case 0x6:   // Await Boarding
					case 0x13:  // Wait
					case 0x14:  // Wait
					case 0x16:  // SS Await Return
					case 0x17:  // SS Launch
					case 0x1C:  // various empty orders (hold steady)
					case 0x1E:
					case 0x21:
					case 0x22:
					case 0x23: 
					case 0x24:  // Self-Destruct
						speed = 0;
						throttle = 0;
						break;
				}
				if (command >= 0x25 && _platform != Settings.Platform.XWA)
				{
					speed = 0;
					throttle = 0;
				}
			}
			if (speed > 0) // Using an explicit speed value instead of throttle.
			{
				speed = (int)((speed * 5) / 2.2235);
				throttle = 0;
			}
			else
			{
				speed = CraftDataManager.GetInstance().GetCraftSpeed(((BaseFlightGroup)fg).CraftType);
				speed = (int)(((double)throttle / 100) * speed);
			}
			if (speed == 0)
				return "stationary";

			int seconds = (int)((dist * 1000) / speed);
			return (seconds / 60).ToString() + ":" + ((seconds % 60 < 10) ? "0" : "") + (seconds % 60).ToString() + " @ " + (throttle != 0 ? throttle + "%" : speed + " mglt");
		}

		Brush getDrawColor(MapData dat)
		{
			if (!passFilter(dat))
				return Brushes.DarkSlateGray;

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
					brText = Brushes.MediumOrchid; //DarkOrchid;
					break;
			}
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
					return Color.MediumOrchid;    // FFBA55D3
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
					_mapX += offx;
					_mapY += offy;
					break;
				case Orientation.XZ:
					_mapX += offx;
					_mapZ += offy;
					break;
				case Orientation.YZ:
					_mapY += offx;
					_mapZ += offy;
					break;
			}
			_dragMapPrevious = _clickPixelUp;
		}

		/// <summary>Retrieves the snap distance amount, in raw map units, as specified in the form control.</summary>
		int getRawSnapAmount()
		{
			int snapAmount;
			if (cboSnapUnit.SelectedIndex == 0)
				snapAmount = (int)(numSnapAmount.Value * 160);  // KM to Raw
			else
				snapAmount = (int)numSnapAmount.Value;          // Already Raw
			if(snapAmount < 1)
				snapAmount = 1;
			return snapAmount;
		}

		/// <summary>Attempts to move a selected object or waypoint relative to its location.</summary>
		void moveObject(SelectionData dat, int offsetX, int offsetY)
		{
			// If part of a larger move operation, it must be flagged as ready after the first frame of movement has been processed.
			// This initializes the starting point so that snapping can work properly.
			if (_dragMoveActive && !_dragMoveSnapReady)
			{
				dat.WpDragStartX = dat.WPRef.RawX;
				dat.WpDragStartY = dat.WPRef.RawY;
				dat.WpDragStartZ = dat.WPRef.RawZ;
			}
			else if (!_dragMoveActive)
			{
				_dragMoveSnapReady = false;
			}
			int currentX = 0, currentY = 0;
			switch (_displayMode)
			{
				case Orientation.XY:
					currentX = (_dragMoveActive ? dat.WpDragStartX : dat.WPRef.RawX);
					currentY = (_dragMoveActive ? dat.WpDragStartY : dat.WPRef.RawY);
					break;
				case Orientation.XZ:
					currentX = (_dragMoveActive ? dat.WpDragStartX : dat.WPRef.RawX);
					currentY = (_dragMoveActive ? dat.WpDragStartZ : dat.WPRef.RawZ);
					break;
				case Orientation.YZ:
					currentX = (_dragMoveActive ? dat.WpDragStartY : dat.WPRef.RawY);
					currentY = (_dragMoveActive ? dat.WpDragStartZ : dat.WPRef.RawZ);
					break;
			}

			// Calculate new position based on snap settings.
			// Integer division by the amount prevents any fractional values.
			int newX = 0, newY = 0;
			int snapAmount = getRawSnapAmount();
			int snapType = (_dragMoveActive ? cboSnapTo.SelectedIndex : 0);
			switch (snapType)
			{
				case 0:  // No snap.
					newX = currentX + offsetX;
					newY = currentY + offsetY;
					break;
				case 1:  // Snap to self.
					newX = currentX + ((offsetX / snapAmount) * snapAmount);
					newY = currentY + ((offsetY / snapAmount) * snapAmount);
					break;
				case 2:  // Snap to grid.
					newX = ((currentX + offsetX) / snapAmount) * snapAmount;
					newY = ((currentY + offsetY) / snapAmount) * snapAmount;
					break;
			}

			switch (_displayMode)
			{
				case Orientation.XY:
					dat.WPRef.RawX = (short)newX;
					dat.WPRef.RawY = (short)newY;
					break;
				case Orientation.XZ:
					dat.WPRef.RawX = (short)newX;
					dat.WPRef.RawZ = (short)newY;
					break;
				case Orientation.YZ:
					dat.WPRef.RawY = (short)newX;
					dat.WPRef.RawZ = (short)newY;
					break;
			}

			if (dat.WpOrder == 18)
			{
				// also apply the offset to the intial order WP
				moveObject(new SelectionData(0, dat.MapDataRef, dat.WpIndex * 4 + 1, 0), offsetX, offsetY);
            }

            if (_platform == Settings.Platform.XWA && ((Platform.Xwa.FlightGroup)dat.MapDataRef.FlightGroup).CraftType == 85)
				processHyperPoints();
        }

        void moveSelectionToCursor()
		{
			int offsetX = _clickMapUp.X - _clickMapDown.X;
			int offsetY = _clickMapUp.Y - _clickMapDown.Y;
			foreach (SelectionData dat in _selectionList)
			{
				if (dat.Active && dat.WpOrder != 17)	// do not move Exit points. Will update on next load
					moveObject(dat, offsetX, offsetY);
			}
			_dragMoveSnapReady = true;
			onDataModified?.Invoke(0, new EventArgs());
		}

		/// <summary>Processes a keyboard event to move all selected objects in a particular direction.</summary>
		/// <remarks>Amount is derived from the snap amount if snapping is enabled (otherwise uses a default value), but snapping is always relative to self.</remarks>
		void moveSelectionByKey(KeyEventArgs e, int directionX, int directionY)
		{
			if(_dragMoveActive || _selectionList.Count == 0)
				return;

			int amount;
			if (cboSnapTo.SelectedIndex > 0)
			{
				amount = getRawSnapAmount();
			}
			else
			{
				amount = (int)(4 / Convert.ToDouble(_zoom) * 160);
				if (_zoom < 1000 && amount < 2)
					amount = 2;
				else if (amount < 1)
					amount = 1;
			}
			if (e.Shift)
			{
				amount /= 4;
				if (amount < 1)
					amount = 1;
			}
			if (e.Control)
			{
				amount *= 4;
				if (amount > 40)
					amount = 40;
			}

			int offsetX = amount * directionX;
			int offsetY = amount * directionY;
			foreach (SelectionData dat in _selectionList)
				if(dat.Active)
					moveObject(dat, offsetX, offsetY);

			if (_selectionList.Count == 1)
			{
				SelectionData dat = _selectionList[0];
				switch (_displayMode)
				{
					case Orientation.XY:
						lblCoor1.Text = "New X: " + Math.Round(dat.WPRef.X, 2).ToString();
						lblCoor2.Text = "New Y: " + Math.Round(dat.WPRef.Y, 2).ToString();
						break;
					case Orientation.XZ:
						lblCoor1.Text = "New X: " + Math.Round(dat.WPRef.X, 2).ToString();
						lblCoor2.Text = "New Z: " + Math.Round(dat.WPRef.Z, 2).ToString();
						break;
					case Orientation.YZ:
						lblCoor1.Text = "New Y: " + Math.Round(dat.WPRef.Y, 2).ToString();
						lblCoor2.Text = "New Z: " + Math.Round(dat.WPRef.Z, 2).ToString();
						break;
				}
			}
			onDataModified?.Invoke(0, new EventArgs());
			MapPaint();
		}

		/// <summary>Moves all selected craft or waypoints into a different region. XWA only.</summary>
		void moveSelectionToRegion(int region)
		{
			bool regionChanged = false, waypointCreated = false;
			foreach (SelectionData dat in _selectionList)
			{
				if (dat.Active)
				{
					Platform.Xwa.FlightGroup.Waypoint curWp = (Platform.Xwa.FlightGroup.Waypoint)dat.WPRef;
					if (dat.WpOrder == 0)
					{
						curWp.Region = Convert.ToByte(region);
						if (dat.WpIndex == 0)
						{
							dat.MapDataRef.Region = region;
							regionChanged = true;
						}
					}
					else
					{
						int targetOrd = (int)(region * 4 + numOrder.Value);
						Platform.Xwa.FlightGroup.Waypoint targWp = (Platform.Xwa.FlightGroup.Waypoint)_mapData[dat.MapDataIndex].WPs[targetOrd][dat.WpIndex];
						targWp.RawX = curWp.RawX;
						targWp.RawY = curWp.RawY;
						targWp.RawZ = curWp.RawZ;
						targWp.Enabled = curWp.Enabled;
						curWp.Enabled = false;
						waypointCreated = true;
					}
				}
			}
			if (regionChanged || waypointCreated)
			{
				deselect();
				onDataModified?.Invoke(0, new EventArgs());
				if (regionChanged)
					lstCraft.Refresh();
				scheduleMapPaint();
			}
		}

		/// <summary>Disables all selected waypoints and removes them from the selection list.</summary>
		void disableSelectionWaypoints()
		{
			bool modified = false;
			if (_selectionList.Count > 0)
			{
				// Work backwards because we have to remove from selection, and this disrupts the selection order.
				for (int i = _selectionList.Count - 1; i >= 0; i--)
				{
					SelectionData dat = _selectionList[i];
					if (dat.Active && (dat.WpIndex != 0 || dat.WpOrder != 0) && dat.WPRef.Enabled)
					{
						dat.WPRef.Enabled = false;
						dat.Active = false;
						setSelectionState(dat.MapDataIndex, false, dat.WpOrder, dat.WpIndex);
						modified = true;
					}
				}
			}
			if (modified)
			{
				updateSelectionListSize();
				updatePreviousCraftSelection();
				onDataModified?.Invoke(0, new EventArgs());
				scheduleMapPaint();
			}
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

		/// <summary>Determines if the craft meets the filter settings for Difficulty and IFF.</summary>
		/// <remarks>Although this is similar to hiding, and many contexts check both, there are some distinctions in the usage.</remarks>
		bool passFilter(MapData dat)
		{
			bool passDiff = (cboViewDifficulty.SelectedIndex == 0 || _platform == Settings.Platform.XWING || Convert.ToBoolean(dat.Difficulty & (1 << cboViewDifficulty.SelectedIndex - 1)));
			bool passIff = (cboViewIff.SelectedIndex == 0 || dat.IFF == cboViewIff.SelectedIndex - 1);
			return (passDiff && passIff);
		}

		/// <summary>Clears all selection states, lists, and resets their associated controls.</summary>
		void deselect()
		{
			bool btemp = _ignoreSelectionEvents;
			_ignoreSelectionEvents = true;
			_previousCraftSelection = null;
			lstCraft.ClearSelected();
			_selectionList.Clear();
			lstSelection.Items.Clear();
			lstSelection.Visible = false;
			lblSelection.Visible = false;
			_ignoreSelectionEvents = btemp;
		}

		/// <summary>Returns true if the specified object parameters are in the current selection list, and active.</summary>
		bool getSelectionState(int datIndex, int order, int wp)
		{
			foreach (SelectionData dat in _selectionList)
				if (dat.Active && dat.MapDataIndex == datIndex && dat.WpOrder == order && dat.WpIndex == wp)
					return true;
			return false;
		}

		/// <summary>Adds or removes a selected object based on its parameters. Ensures the object is unique.</summary>
		/// <remarks>Automatically maintains the list and its associated controls.</remarks>
		void setSelectionState(int datIndex, bool state, int order, int wp)
		{
			if (datIndex < 0 || datIndex >= _mapData.Length)
				return;
			bool btemp = _ignoreSelectionEvents;
			_ignoreSelectionEvents = true;
			if (!state)
			{
				int i = 0;
				while (i < _selectionList.Count)
				{
					if (_selectionList[i].MapDataIndex == datIndex && _selectionList[i].WpOrder == order &&_selectionList[i].WpIndex == wp)
					{
						_selectionList.RemoveAt(i);
						lstSelection.Items.RemoveAt(i);
						if (order == 0 && wp == 0)
							lstCraft.SetSelected(datIndex, false);
					}
					else
						i++;
				}
			}
			else
			{
				bool found = false;
				foreach (SelectionData dat in _selectionList)
				{
					if (dat.MapDataIndex == datIndex && dat.WpOrder == order && dat.WpIndex == wp)
					{
						found = true;
						break;
					}
				}
				if (!found)
				{
					_selectionList.Add(new SelectionData(datIndex, _mapData[datIndex], order, wp));
					int chkIndex = (order > 0 ? 4 + wp : wp);
					lstSelection.Items.Add(_mapData[datIndex].FullName + "  " + chkWP[chkIndex].Text);
					lstSelection.SetSelected(lstSelection.Items.Count - 1, true);
					if (order == 0 && wp == 0)
						lstCraft.SetSelected(datIndex, true);
				}
			}
			_ignoreSelectionEvents = btemp;
		}

		/// <summary>Selects objects via mouse click or bounding-box dragging.</summary>
		void performSelection()
		{
			// Select anything inside the bounding box, or a single object if the user clicked on something.
			// If the Control key is held down, add the bounding box to the current selection. Or toggle selection on a single object.
			if(!ModifierKeys.HasFlag(Keys.Control))
				deselect();

			List<SelectionData> objects = new List<SelectionData>();
			enumSelectionObjects(ref _clickPixelDown, ref _clickPixelUp, objects);
			bool isClick = isPointClicked(ref _clickPixelDown, ref _clickPixelUp);
			if (isClick && objects.Count > 1)
				objects.Sort(SelectionData.Compare);

			foreach (SelectionData dat in objects)
			{
				setSelectionState(dat.MapDataIndex, (objects.Count != 1 || !ModifierKeys.HasFlag(Keys.Control) || !getSelectionState(dat.MapDataIndex, dat.WpOrder, dat.WpIndex)), dat.WpOrder, dat.WpIndex);
				if (isClick)
					break;
			}
			updateSelectionListSize();
			updatePreviousCraftSelection();
		}

		/// <summary>Populates a list of objects (craft or their waypoints) that can be selected inside a rectangle of two opposing points.</summary>
		/// <remarks>Points are the physical pixel coordinates on the map. The points will be interpreted as either a click (similar or identical points) or a bounding-box.</remarks>
		void enumSelectionObjects(ref Point p1, ref Point p2, List<SelectionData> output)
		{
			Point m1 = new Point();
			Point m2 = new Point();
			convertMousepointToWaypoint(p1.X, p1.Y, ref m1);
			convertMousepointToWaypoint(p2.X, p2.Y, ref m2);
			normalizePoint(ref m1, ref m2);

			if (isPointClicked(ref p1, ref p2))  // Also handles normalizing.
			{
				double msX = 0.0, msY = 0.0;
				switch (_displayMode)
				{
					case Orientation.XY:
						msX = (p2.X - _mapX) / Convert.ToDouble(_zoom) * 160;
						msY = (_mapY - p2.Y) / Convert.ToDouble(_zoom) * 160;
						break;
					case Orientation.XZ:
						msX = (p2.X - _mapX) / Convert.ToDouble(_zoom) * 160;
						msY = (_mapZ - p2.Y) / Convert.ToDouble(_zoom) * 160;
						break;
					case Orientation.YZ:
						msX = (p2.X - _mapY) / Convert.ToDouble(_zoom) * 160;
						msY = (_mapZ - p2.Y) / Convert.ToDouble(_zoom) * 160;
						break;
				}

				int tolerance = (int)(10 / Convert.ToDouble(_zoom) * 160);
				if (tolerance < 1)
					tolerance = 1; // For very high zoom levels.
				m1.X = (int)msX - tolerance;
				m1.Y = (int)msY - tolerance;
				m2.X = (int)msX + tolerance;
				m2.Y = (int)msY + tolerance;
			}

			int ord = 0;
			int reg = 0;
			if (_platform == Settings.Platform.XWA)
			{
				reg = (int)(numRegion.Value - 1);
				ord = (int)(reg * 4 + numOrder.Value);
			}
			for (int i = 0; i < _mapData.Length; i++)
			{
				if (_mapData[i].View == Visibility.Hide || !passFilter(_mapData[i])) continue;

				for (int wp = 0; wp < _mapData[i].WPs[0].Length; wp++)
				{
					if (!chkWP[wp].Checked || (!_mapData[i].WPs[0][wp].Enabled && _platform != Settings.Platform.XWA && wp != 0)) continue;
					if (_platform == Settings.Platform.XWA)
					{
						Platform.Xwa.FlightGroup.Waypoint cwp = (Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[0][wp];
						if (cwp.Region != numRegion.Value - 1) continue;
					}
					if (waypointInside(_mapData[i].WPs[0][wp], ref m1, ref m2))
					{
						output.Add(new SelectionData(i, _mapData[i], 0, wp));
					}
				}
				if (ord > 0)
				{
					for (int wp = 0; wp < _mapData[i].WPs[ord].Length; wp++)
					{
						if (!chkWP[4 + wp].Checked || !_mapData[i].WPs[ord][wp].Enabled) continue;
						if (_platform == Settings.Platform.XWA && _mapData[i].WPs[18][reg].Enabled && wp == 0) continue;
						if (waypointInside(_mapData[i].WPs[ord][wp], ref m1, ref m2))
						{
							output.Add(new SelectionData(i, _mapData[i], ord, wp));
						}
					}
				}
				if (_platform == Settings.Platform.XWA)
				{
					var hyp = (Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[17][reg];
					if (hyp.Enabled && waypointInside(hyp, ref m1, ref m2))
						output.Add(new SelectionData(i, _mapData[i], 17, reg));
					var exit = (Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[18][reg];
                    if (exit.Enabled && waypointInside(exit, ref m1, ref m2))
                        output.Add(new SelectionData(i, _mapData[i], 18, reg));
                }
			}
		}

		/// <summary>Returns true if two points are close enough to be considered a clicked point rather than a bounding box.</summary>
		bool isPointClicked(ref Point p1, ref Point p2)
		{
			normalizePoint(ref p1, ref p2);
			return (p2.X - p1.X <= 12 && p2.Y - p1.Y <= 12);
		}

		/// <summary>Determines if a waypoint (raw units) is within a bounding box formed by a top/left and bottom/right point.</summary>
		bool waypointInside(BaseFlightGroup.BaseWaypoint wp, ref Point p1, ref Point p2)
		{
			int tx = wp.RawX;
			int ty = wp.RawY;
			int tz = wp.RawZ;
			switch (_displayMode)
			{
				case Orientation.XY: return ((tx >= p1.X && tx <= p2.X) && (ty >= p1.Y && ty <= p2.Y));
				case Orientation.XZ: return ((tx >= p1.X && tx <= p2.X) && (tz >= p1.Y && tz <= p2.Y));
				case Orientation.YZ: return ((ty >= p1.X && ty <= p2.X) && (tz >= p1.Y && tz <= p2.Y));
			}
			return false;
		}

		/// <summary>Returns true if any specified objects are currently in our selection list.</summary>
		bool isListObjectSelected(List<SelectionData> objects)
		{
			foreach (SelectionData test in objects)
			{
				foreach (SelectionData cur in _selectionList)
				{
					if (test.MapDataIndex == cur.MapDataIndex && test.WpIndex == cur.WpIndex && test.WpOrder == cur.WpOrder)
						return true;
				}
			}
			return false;
		}
		/// <summary>Returns true if the specified craft or any of its waypoints are selected.</summary>
		bool isMapObjectSelected(int mapDataIndex)
		{
			for(int i = 0; i < _selectionList.Count; i++)
				if (_selectionList[i].MapDataIndex == mapDataIndex)
					return true;
			return false;
		}

		/// <summary>Returns a size rating of the map object, based off its wireframe.</summary>
		/// <remarks>Wireframes must be enabled. NOTE: This should probably be replaced with craft categories, but that would require additional changes, and more information in the craft data files.</remarks>
		int getModelSizeRating(int mapDataIndex)
		{
			if (!_settings.WireframeEnabled)
				return 0;
			if (_platform == Settings.Platform.XWA && _mapData[mapDataIndex].Craft == 0x55)  // Hyper point
				return 0;
			WireframeInstance wire = _wireframeManager.GetOrCreateWireframeInstance(_mapData[mapDataIndex].Craft, _mapData[mapDataIndex].FgIndex);
			if (wire == null || wire.ModelDef == null)
				return 0;
			int size = wire.ModelDef.LongestSpanMeters;
			int limit1 = 40, limit2 = 100, limit3 = 450;
			if (size > 1 && size < limit1)
				return 1;
			else if (size >= limit1 && size < limit2)
				return 2;
			else if (size >= limit2 && size < limit3)
				return 3;
			else if (size >= limit3)
				return 4;
			return 0;
		}

		/// <summary>Returns a value composed of a set of bit flags indicating which difficulties this FlightGroup arrives in.</summary>
		int getDifficultyFlags(byte fgDifficulty)
		{
			if (_platform == Settings.Platform.XWING)
				return 0b111;
			// All, Easy, Medium, Hard, ...
			int[] resultArray = { 0b111, 0b001, 0b010, 0b100, 0b110, 0b011, 0b000, 0b000, 0b001, 0b010, 0b100 };
			return (fgDifficulty < resultArray.Length ? resultArray[fgDifficulty] : 0b000);
		}

		/// <summary>Selects all visible objects on the map screen based on the criteria of currently selected items.</summary>
		void expandSelection(ExpandSelection mode)
		{
			HashSet<int> values = new HashSet<int>();
			Point m1 = new Point(), m2 = new Point();
			convertMousepointToWaypoint(0, 0, ref m1);
			convertMousepointToWaypoint(pctMap.Width, pctMap.Height, ref m2);
			normalizePoint(ref m1, ref m2);
			switch (mode)
			{
				case ExpandSelection.Craft:
					foreach (SelectionData dat in _selectionList)
						if(dat.Active)
							values.Add(dat.MapDataRef.Craft);
					for (int i = 0; i < _mapData.Length; i++)
					{
						bool wpEnabled = (_platform == Settings.Platform.XWA ? ((Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[0][0]).Region == numRegion.Value - 1 : _mapData[i].WPs[0][0].Enabled);
						if (_mapData[i].View != Visibility.Hide && passFilter(_mapData[i]) && values.Contains(_mapData[i].Craft) && wpEnabled && waypointInside(_mapData[i].WPs[0][0], ref m1, ref m2))
							setSelectionState(i, true, 0, 0);
					}
					break;
				case ExpandSelection.Iff:
					foreach (SelectionData dat in _selectionList)
						if(dat.Active)
							values.Add(dat.MapDataRef.IFF);
					for (int i = 0; i < _mapData.Length; i++)
					{
						bool wpEnabled = (_platform == Settings.Platform.XWA ? ((Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[0][0]).Region == numRegion.Value - 1 : _mapData[i].WPs[0][0].Enabled);
						if (_mapData[i].View != Visibility.Hide && passFilter(_mapData[i]) && values.Contains(_mapData[i].IFF) && wpEnabled && waypointInside(_mapData[i].WPs[0][0], ref m1, ref m2))
							setSelectionState(i, true, 0, 0);
					}
					break;
				case ExpandSelection.Size:
					foreach (SelectionData dat in _selectionList)
					{
						if (dat.Active)
						{
							int size = getModelSizeRating(dat.MapDataIndex);
							if (size > 0)
								values.Add(size);
						}
					}
					for (int i = 0; i < _mapData.Length; i++)
					{
						bool wpEnabled = (_platform == Settings.Platform.XWA ? ((Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[0][0]).Region == numRegion.Value - 1 : _mapData[i].WPs[0][0].Enabled);
						int size = getModelSizeRating(i);
						if (_mapData[i].View != Visibility.Hide && passFilter(_mapData[i]) && values.Contains(size) && wpEnabled && waypointInside(_mapData[i].WPs[0][0], ref m1, ref m2))
							setSelectionState(i, true, 0, 0);
					}
					break;
				case ExpandSelection.Invert:
					foreach (SelectionData dat in _selectionList)
						if (dat.Active && dat.WpIndex == 0 && dat.WpOrder == 0)
							values.Add(dat.MapDataIndex);
					deselect();
					for (int i = 0; i < _mapData.Length; i++)
					{
						bool wpEnabled = (_platform == Settings.Platform.XWA ? ((Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[0][0]).Region == numRegion.Value - 1 : _mapData[i].WPs[0][0].Enabled);
						if (_mapData[i].View != Visibility.Hide && passFilter(_mapData[i]) && !values.Contains(i) && wpEnabled && waypointInside(_mapData[i].WPs[0][0], ref m1, ref m2))
							setSelectionState(i, true, 0, 0);
					}
					break;
			}
			updateSelectionListSize();
			updatePreviousCraftSelection();
			scheduleMapPaint();
		}

		/// <summary>Centers the map over all visible objects in the world, adjusting the zoom for best fit.</summary>
		void fitMapToWorld()
		{
			List<SelectionData> objects = new List<SelectionData>();
			for (int i = 0; i < _mapData.Length; i++)
			{
				if (_mapData[i].View == Visibility.Hide || !passFilter(_mapData[i])) continue;

				// Derived from the waypoint trace code.
				for (int k = 0; k < 4; k++) // Start
					if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled && (_platform != Settings.Platform.XWA || _mapData[i].WPs[0][k][4] == (short)(numRegion.Value - 1)))
						objects.Add(new SelectionData(i, _mapData[i], 0, k));

				if (_platform == Settings.Platform.XWA) // WPs
				{
					int ord = (int)((numRegion.Value - 1) * 4 + (numOrder.Value - 1) + 1);
					for (int k = 0; k < 8; k++)
						if (chkWP[k + 4].Checked && _mapData[i].WPs[ord][k].Enabled)
							objects.Add(new SelectionData(i, _mapData[i], ord, k));
				}
				else
				{
					for (int k = 4; k < 22; k++)
						if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled)
							objects.Add(new SelectionData(i, _mapData[i], 0, k));
				}
			}
			fitMapToObjects(objects);
		}

		/// <summary>Centers the map over all specified objects, adjusting the zoom for best fit.</summary>
		/// <remarks>If no objects are specified, centers the map on (0,0) at its default zoom level.</remarks>
		void fitMapToObjects(List<SelectionData> objects)
		{
			if (objects == null || objects.Count == 0)
			{
				_mapX = w / 2;
				_mapY = h / 2;
				_mapZ = h / 2;
				hscZoom.Value = 40;
				MapPaint();
				return;
			}
			int minX = short.MaxValue;
			int minY = short.MaxValue;
			int maxX = short.MinValue;
			int maxY = short.MinValue;

			foreach (SelectionData dat in objects)
			{
				if (!dat.Active)
					continue;
				// Project a distance around each point based on half its wireframe size.
				int wireframeRawSize = 0;
				if (dat.WpIndex == 0 && dat.WpOrder == 0 && _settings.WireframeEnabled)
				{
					WireframeInstance wireframe = _wireframeManager.GetOrCreateWireframeInstance(dat.MapDataRef.Craft, dat.MapDataRef.FgIndex);
					if (wireframe != null && wireframe.ModelDef != null)
						wireframeRawSize = (int)((double)wireframe.ModelDef.LongestSpanMeters / 1000 / 2 * 160);
				}
				int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
				switch (_displayMode)
				{
					case Orientation.XY:
						x1 = dat.WPRef.RawX;
						y1 = dat.WPRef.RawY;
						break;
					case Orientation.XZ:
						x1 = dat.WPRef.RawX;
						y1 = dat.WPRef.RawZ;
						break;
					case Orientation.YZ:
						x1 = dat.WPRef.RawY;
						y1 = dat.WPRef.RawZ;
						break;
				}
				x2 = x1 + wireframeRawSize;
				y2 = y1 + wireframeRawSize;
				x1 -= wireframeRawSize;
				y1 -= wireframeRawSize;

				if (x1 < minX) minX = x1;
				if (x2 > maxX) maxX = x2;
				if (y1 < minY) minY = y1;
				if (y2 > maxY) maxY = y2;
			}
			double spanKmX = (maxX * 0.00625) - (minX * 0.00625);
			double spanKmY = (maxY * 0.00625) - (minY * 0.00625);
			if (spanKmX < 0.001) spanKmX = 0.001;
			if (spanKmY < 0.001) spanKmY = 0.001;

			// Calculate the zoom level necessary to hold the span of objects in each dimension. Choose the smaller value to ensure everything fits on screen.
			double zoomX = (double)pctMap.Width / spanKmX;
			double zoomY = (double)pctMap.Height / spanKmY;
			double zoom = (zoomX <= zoomY ? zoomX : zoomY);

			zoom *= 0.85;
			if (zoom < 5)
				zoom = 5;
			else if (zoom > 200)
				zoom = 200;

			hscZoom.Value = (int)zoom;
			updateMapCoord(new PointF((float)((minX + ((maxX - minX) / 2.0)) / 160.0), (float)((minY + ((maxY - minY) / 2.0)) / 160.0)));
			MapPaint();
		}

		/// <summary>Centers the map over the positional average of all selected objects. Does nothing if no objects are selected.</summary>
		/// <remarks>This function is also called when changing XY, XZ, and YZ views, so it redraws the screen even if the position remains the same.</remarks>
		void centerMapOnSelection()
		{
			if (_selectionList.Count > 0)
			{
				int mapX = 0, mapY = 0;
				switch (_displayMode)
				{
					case Orientation.XY: getAverageSelectionCoords(out mapX, out mapY, out _); break;
					case Orientation.XZ: getAverageSelectionCoords(out mapX, out _, out mapY); break;
					case Orientation.YZ: getAverageSelectionCoords(out _, out mapX, out mapY); break;
				}
				updateMapCoord(new PointF((float)mapX / 160, (float)mapY / 160));
			}
			MapPaint();
		}

		/// <summary>Creates a waypoint for any selected craft whos waypoint is disabled. If no waypoints were created, then the existing waypoints are selected.</summary>
		/// <remarks>The type of waypoint depends on which keys are pressed. Expects number row keys and modifiers. Resolves selected craft/waypoints to their parent craft.</remarks>
		void createOrSelectWaypoint(KeyEventArgs e)
		{
			// Mouse position contains screen coordinates, so check their bounds and convert them relative to the map area.
			Rectangle mapRect = pctMap.RectangleToScreen(pctMap.DisplayRectangle);
			if (!mapRect.Contains(MousePosition))
				return;

			bool create = (e.KeyCode == _lastWaypointKeyCode && e.Modifiers == _lastWaypointKeyModifiers && (Environment.TickCount - _lastWaypointKeyTime <= SystemInformation.DoubleClickTime));

			_lastWaypointKeyCode = e.KeyCode;
			_lastWaypointKeyModifiers = e.Modifiers;
			_lastWaypointKeyTime = Environment.TickCount;

			Point mouse = new Point();
			convertMousepointToWaypoint(MousePosition.X - mapRect.Left, MousePosition.Y - mapRect.Top, ref mouse);

			int wp = -1;
			int chk = -1; // Special case for XWA, since the checkbox array doesn't necessarily match the waypoint index.
			int ord = (_platform == Settings.Platform.XWA ? (int)((numRegion.Value - 1) * 4 + numOrder.Value) : 0);

			switch (_platform)
			{
				case Settings.Platform.XWING:
					if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D3)
					{
						if (e.Shift && !e.Control) wp = e.KeyCode - Keys.D1;  // SP1 to SP3
						else if (!e.Shift && !e.Control) wp = 4 + e.KeyCode - Keys.D1;  // WP1 to WP3
						else if (!e.Shift && e.Control) wp = 14 + e.KeyCode - Keys.D1;
					}
					else if (e.KeyCode == Keys.D0) wp = 13;  // HYP
					break;
				case Settings.Platform.TIE:
				case Settings.Platform.XvT:
				case Settings.Platform.BoP:
					if (e.Shift && !e.Control && e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D4) wp = e.KeyCode - Keys.D1;  // SP1 to SP4
					else if (!e.Shift && !e.Control && e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D8) wp = 4 + e.KeyCode - Keys.D1;  // WP1 to WP8
					else if (!e.Shift && e.Control && e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D8) wp = 14 + (_platform == Settings.Platform.TIE ? 0 : e.KeyCode - Keys.D1);  //BRF for TIE, BRF to BR8 for XvT
					else if (e.KeyCode == Keys.D9) wp = 12; // RND
					else if (e.KeyCode == Keys.D0) wp = 13; // HYP
					break;
				case Settings.Platform.XWA:
					if (e.Shift && e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D3) { wp = e.KeyCode - Keys.D1; ord = 0; }  // SP1 to SP3
					else if (!e.Shift && e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D8) { wp = e.KeyCode - Keys.D1; chk = 4 + wp; } // WP1 to WP8
					else if (e.KeyCode == Keys.D0) { wp = 3; ord = 0; } // HYP
					break;
			}
			if (wp == -1)  // Keys didn't resolve to a valid waypoint.
				return;

			bool modified = false;
			bool refreshRequired = false;
			List<SelectionData> exist = new List<SelectionData>();

			// Build a list of parent craft.
			HashSet<int> parentSet = new HashSet<int>();
			foreach (SelectionData dat in _selectionList)
				if(dat.Active)
					parentSet.Add(dat.MapDataIndex);

			foreach (int i in parentSet) 
			{
				if(wp < _mapData[i].WPs[ord].Length)
				{
					BaseFlightGroup.BaseWaypoint baseWp = _mapData[i].WPs[ord][wp];
					if (!baseWp.Enabled && create)
					{
						if (_platform == Settings.Platform.XWA)
						{
							((Platform.Xwa.FlightGroup.Waypoint)baseWp).Region = Convert.ToByte(numRegion.Value - 1);
							if (wp == 0 && ord == 0)
							{
								_mapData[i].Region = (int)numRegion.Value - 1;
								refreshRequired = true;
							}
						}

						modified = true;
						baseWp.Enabled = true;
						switch (_displayMode)
						{
							case Orientation.XY: baseWp.RawX = (short)mouse.X; baseWp.RawY = (short)mouse.Y; break;
							case Orientation.XZ: baseWp.RawX = (short)mouse.X; baseWp.RawZ = (short)mouse.Y; break;
							case Orientation.YZ: baseWp.RawY = (short)mouse.X; baseWp.RawZ = (short)mouse.Y; break;
						}
					}
					else if(baseWp.Enabled && !create)
					{
						exist.Add(new SelectionData(i, _mapData[i], ord, wp));
					}
				}
			}
			if (modified || exist.Count > 0)
			{
				if (chk == -1) chk = wp;
				if (chk < chkWP.Length && !chkWP[chk].Checked)
					chkWP[chk].Checked = true;
			}

			if (modified)
			{
				onDataModified?.Invoke(0, new EventArgs());
				if (refreshRequired) lstCraft.Refresh();
				scheduleMapPaint();
			}
			else if(exist.Count > 0)
			{
				deselect();
				foreach (SelectionData dat in exist)
					setSelectionState(dat.MapDataIndex, true, dat.WpOrder, dat.WpIndex);
				updateSelectionListSize();
				updatePreviousCraftSelection();
				scheduleMapPaint();
			}
		}

		/// <summary>Assigns which measurement units are used when snapping object movement.</summary>
		/// <remarks>If the unit is changed, converts the existing amount and updates the form controls with the new limits.</remarks>
		void setSnapUnit(int selectedIndex)
		{
			cboSnapUnit.SelectedIndex = selectedIndex;
			if (_lastSnapUnit == selectedIndex)
				return;
			decimal value = numSnapAmount.Value;
			if (selectedIndex == 0)  // KM
			{
				numSnapAmount.Minimum = (decimal)0.01;
				numSnapAmount.Maximum = (decimal)0.50;
				numSnapAmount.Increment = (decimal)0.01;
				numSnapAmount.DecimalPlaces = 2;
			}
			else  // Raw
			{
				numSnapAmount.Minimum = 1;
				numSnapAmount.Maximum = 80;
				numSnapAmount.Increment = 1;
				numSnapAmount.DecimalPlaces = 0;
			}
			if (selectedIndex == 0)
				value = (decimal)((double)value / 160);  // Raw to KM
			else
				value = (decimal)Math.Round((double)value * 160, 0);  // KM to Raw, round to whole units

			if (value < numSnapAmount.Minimum)
				value = numSnapAmount.Minimum;
			else if (value > numSnapAmount.Maximum)
				value = numSnapAmount.Maximum;
			numSnapAmount.Value = value;
			_lastSnapUnit = selectedIndex;
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
			_settings = config;
			if (_wireframeManager == null)
				_wireframeManager = new WireframeManager();
			_wireframeManager.SetPlatform(_platform, _settings);

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
			_mapX = w / 2;
			_mapY = h / 2;
			_mapZ = h / 2;
			_dragIcon[0] = -1;
			_isLoading = true;
			chkTags.Checked = Convert.ToBoolean(_settings.MapOptions & Settings.MapOpts.FGTags);
			chkTrace.Checked = Convert.ToBoolean(_settings.MapOptions & Settings.MapOpts.Traces);
			chkDistance.Checked = Convert.ToBoolean(_settings.MapOptions & Settings.MapOpts.TraceDistance);
			chkTime.Checked = Convert.ToBoolean(_settings.MapOptions & Settings.MapOpts.TraceTime);
			chkTraceHideFade.Checked = Convert.ToBoolean(_settings.MapOptions & Settings.MapOpts.TraceHideFade);
			chkTraceSelected.Checked = Convert.ToBoolean(_settings.MapOptions & Settings.MapOpts.TraceSelected);
			chkWireframe.Checked = _settings.WireframeEnabled;
			chkLimit.Checked = _settings.WireframeIconThresholdEnabled;
			chkLimit.Text = "Only above " + _settings.WireframeIconThresholdSize + "m";
			chkDistance.Enabled = chkTrace.Checked;
			chkTime.Enabled = chkTrace.Checked;
			chkTraceHideFade.Enabled = chkTrace.Checked;
			chkTraceSelected.Enabled = chkTrace.Checked;
			int t = _settings.Waypoints;
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
			else if (_platform == Settings.Platform.TIE)
			{
				for (int i = 0; i < 15; i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
				for (int i = 15; i < 22; i++) chkWP[i].Enabled = false;
			}
			else if (_platform == Settings.Platform.XvT || _platform == Settings.Platform.BoP) for (int i = 0; i < 22; i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
			else if (_platform == Settings.Platform.XWA)
			{
				for (int i = 0; i < 12; i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
				for (int i = 12; i < 22; i++) chkWP[i].Enabled = false;
				lblRegion.Visible = true;
				numRegion.Visible = true;
				chkSP3.Text = "RDV";
				chkSP4.Text = "HYP";
			}
			if (_platform != Settings.Platform.XWING)
			{
				lblOrder.Visible = true;
				numOrder.Visible = true;
				numOrder.Maximum = (_platform == Settings.Platform.TIE ? 3 : 4);
			}
			MouseWheel += new MouseEventHandler(form_MouseWheel);   // this is here because of some stupid reason, MouseWheel isn't available in the GUI

			// Calculate how wide the sidebar list should be, based on the strings it holds.
			int span = lstCraft.Width;
			int padding = SystemInformation.VerticalScrollBarWidth + 10;
			for (int i = 0; i < _mapData.Length; i++)
			{
				Size size = TextRenderer.MeasureText(_mapData[i].FullName, lstCraft.Font);
				if (size.Width + padding > span)
					span = size.Width + padding;
			}
			if (span != lstCraft.Width)
			{
				int diff = span - lstCraft.Width;
				lstCraft.Width = span;
				pctMap.Left += diff;
				pctMap.Width -= diff;
				lblSelection.Left += diff - 1;
				lstSelection.Left += diff - 1;
				span += TextRenderer.MeasureText("  SP1", lstSelection.Font).Width;
				lstSelection.Width = span - padding;
			}

			cboViewIff.Items.Clear();
			cboViewIff.Items.Add("All IFF");
			switch(_platform)
			{
				case Settings.Platform.XWING:
					cboViewDifficulty.Enabled = false;
					cboViewIff.Items.AddRange( new string[] { "Rebel", "Imperial", "Neutral", "Neutral 2"});
					break;
				case Settings.Platform.TIE: cboViewIff.Items.AddRange(Platform.Tie.Strings.IFF); break;
				case Settings.Platform.XvT:	case Settings.Platform.BoP: cboViewIff.Items.AddRange(Platform.Xvt.Strings.IFF); break;
				case Settings.Platform.XWA: cboViewIff.Items.AddRange(Platform.Xwa.Strings.IFF); break;
			}
			cboViewDifficulty.SelectedIndex = 0;
			cboViewIff.SelectedIndex = 0;

			cboSnapTo.SelectedIndex = _settings.MapSnapTo;
			setSnapUnit(_settings.MapSnapUnit);
			numSnapAmount.Value = Convert.ToDecimal(_settings.MapSnapAmount);
			performMiddleClickAction(_settings.MapMiddleClickActionNoneSelected);

			_isLoading = false;
		}

		/// <summary>Comparison function that sorts MapData objects by visibility. Brings faded items up front so that they can be drawn first. Visible items will overlap them.</summary>
		int compareDrawOrder(int leftIndex, int rightIndex)
		{
			if (_mapData[leftIndex].View > _mapData[rightIndex].View)
				return -1;
			else if (_mapData[leftIndex].View < _mapData[rightIndex].View)
				return 1;
			return 0;
		}

		/// <summary>Adjust control size/locations</summary>
		void updateLayout()
		{
			PointF center = getCenterCoord();
			pctMap.Width = Width - 102 - (lstCraft.Width + lstCraft.Margin.Right);
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
			lblRegion.Left = Width - 183; // Width - 268;
			numRegion.Left = Width - 133; // Width - 218;
			lblOrder.Left = Width - 272; // Width - 171;
			numOrder.Left = Width - 230; // Width - 129;
			grpDir.Left = Width - 90;
			grpPoints.Left = grpDir.Left;

			// Align all checkboxes to the bottom right of the map.
			chkTags.Left = grpDir.Left;
			chkTrace.Left = grpDir.Left;
			chkDistance.Left = grpDir.Left;
			chkTime.Left = grpDir.Left;
			chkTraceHideFade.Left = grpDir.Left - chkTraceHideFade.Width;
			chkTraceSelected.Left = grpDir.Left - chkTraceHideFade.Width;
			// Now align from the bottom, stacked in reverse order.
			moveControlAbove(chkTime, null, ClientRectangle.Bottom - 3);
			moveControlAbove(chkDistance, chkTime, 0);
			moveControlAbove(chkTrace, chkDistance, 0);
			moveControlAbove(chkTags, chkTrace, 0);
			moveControlAbove(chkTraceSelected, null, ClientRectangle.Bottom - 3);
			moveControlAbove(chkTraceHideFade, chkTraceSelected, 0);

			updateMapCoord(center);
			lstCraft.Height = pctMap.Bottom - lstCraft.Top;

			moveControlLeft(cboSnapUnit, grpDir, 4);
			moveControlLeft(numSnapAmount, cboSnapUnit, 0);
			moveControlLeft(cboSnapTo, numSnapAmount, 0);
			moveControlLeft(lblSnapTo, cboSnapTo, 0);

			// Align the hide/fade controls near the center top of the map.
			int right = pctMap.Left + (pctMap.Width / 2) + 16;
			moveControlLeft(cmdFadeNone, null, right);
			moveControlLeft(cmdHideNone, null, right);
			moveControlLeft(cmdFadeSubtract, cmdFadeNone, 5);
			moveControlLeft(cmdHideSubtract, cmdHideNone, 5);
			moveControlLeft(cmdFadeAdd, cmdFadeSubtract, 0);
			moveControlLeft(cmdHideAdd, cmdHideSubtract, 0);
			moveControlLeft(lblFade, cmdFadeAdd, 0);
			moveControlLeft(lblHide, cmdHideAdd, 0);

			// Align the "expand selection" controls to the left of the hide/fade controls.
			/*moveControlLeft(cmdExpandBySize, lblHide, 20);
			moveControlLeft(cmdExpandByIff, cmdExpandBySize, 0);
			moveControlLeft(cmdExpandByCraft, cmdExpandByIff, 0);
			moveControlLeft(cmdInvertSelection, cmdExpandByCraft, 0);
			moveControlAbove(lblExpandSelection, cmdExpandByCraft, 0);
			lblExpandSelection.Left = cmdExpandByCraft.Left;*/

			// Align the "fit to" buttons near the center bottom of the map, just above the zoom bar.
			cmdFitDefault.Left = (pctMap.Left + (pctMap.Width / 2)) + cmdFitDefault.Width + cmdFitDefault.Margin.Left + 10;
			moveControlLeft(cmdFitWorld, cmdFitDefault, 0);
			moveControlLeft(cmdFitSelected, cmdFitWorld, 0);
			moveControlLeft(lblFit, cmdFitSelected, 0);
			moveControlLeft(cmdCenterSelected, lblFit, 0);
			moveControlLeft(lblCenterOn, cmdCenterSelected, 0);
			cmdFitSelected.Top = hscZoom.Top - cmdFitSelected.Height - cmdFitSelected.Margin.Bottom;
			cmdFitWorld.Top = cmdFitSelected.Top;
			cmdFitDefault.Top = cmdFitSelected.Top;
			lblFit.Top = cmdFitSelected.Top + 5;
			cmdCenterSelected.Top = cmdFitSelected.Top;
			lblCenterOn.Top = cmdFitSelected.Top + 5;

			moveControlAbove(cboViewDifficulty, lblCoor1, 5);
			moveControlAbove(cboViewIff, lblCoor1, 5);

			MapPaint();
		}

		/// <summary>Aligns a control horizontally to the left of another control, or to the left of a fixed point if no control is specified.</summary>
		void moveControlLeft(Control control, Control anchor, int value)
		{
			if (anchor != null)
				control.Left = anchor.Left - (control.Width + value + (control.Margin.Right >= anchor.Margin.Left ? control.Margin.Right : anchor.Margin.Left));
			else
				control.Left = value - (control.Width + control.Margin.Right);
		}
		/// <summary>Aligns a control vertically above another control, or above a fixed point if no control is specified.</summary>
		void moveControlAbove(Control control, Control anchor, int padding)
		{
			if (anchor != null)
				control.Top = anchor.Top - (control.Height + padding + (control.Margin.Bottom >= anchor.Margin.Top ? control.Margin.Bottom : anchor.Margin.Top));
			else
				control.Top = padding - (control.Height + control.Margin.Bottom);
		}

		/// <summary>Update mapX and mapY</summary>
		/// <param name="coord">The coordinate in klicks to use as the new center</param>
		void updateMapCoord(PointF coord)
		{
			switch (_displayMode)
			{
				case Orientation.XY:
					_mapX = Convert.ToInt32(w / 2 - coord.X * Convert.ToSingle(_zoom));
					_mapY = Convert.ToInt32(h / 2 + coord.Y * Convert.ToSingle(_zoom));
					break;
				case Orientation.XZ:
					_mapX = Convert.ToInt32(w / 2 - coord.X * Convert.ToSingle(_zoom));
					_mapZ = Convert.ToInt32(h / 2 + coord.Y * Convert.ToSingle(_zoom));
					break;
				case Orientation.YZ:
					_mapY = Convert.ToInt32(w / 2 - coord.X * Convert.ToSingle(_zoom));
					_mapZ = Convert.ToInt32(h / 2 + coord.Y * Convert.ToSingle(_zoom));
					break;
			}
			if (_mapX / _zoom > 150) _mapX = 150 * _zoom;
			if ((_mapX - w) / _zoom < -150) _mapX = -150 * _zoom + w;
			if (_mapY / _zoom > 150) _mapY = 150 * _zoom;
			if ((_mapY - h) / _zoom < -150) _mapY = -150 * _zoom + h;
			if (_mapZ / _zoom > 150) _mapZ = 150 * _zoom;
			if ((_mapZ - h) / _zoom < -150) _mapZ = -150 * _zoom + h;
		}

		/// <summary>Calculates and retrieves the average coordinates (in raw map units) of all selected items.</summary>
		void getAverageSelectionCoords(out int x, out int y, out int z)
		{
			int sumX = 0, sumY = 0, sumZ = 0;
			/*int ord; // Not used
			if (_platform == Settings.Platform.XWA)
				ord = (int)((numRegion.Value - 1) * 4 + numOrder.Value - 1);*/

			foreach (SelectionData dat in _selectionList)
			{
				sumX += dat.WPRef.RawX;
				sumY += dat.WPRef.RawY;
				sumZ += dat.WPRef.RawZ;
			}
			if (_selectionList.Count > 0)
			{
				x = sumX / _selectionList.Count;
				y = sumY / _selectionList.Count;
				z = sumZ / _selectionList.Count;
				return;
			}
			x = 0; y = 0; z = 0;
		}

		/// <summary>Clears and rebuilds the selection lists.</summary>
		/// <remarks>For reloading externally modified data (since the main editor and map utilize different lists).</remarks>
		void reloadSelectionControls()
		{
			lstSelection.Items.Clear();
			_selectionList.Clear();
			_sortedMapDataList.Clear();
			lstCraft.Items.Clear();
			for (int i = 0; i < _mapData.Length; i++)
			{
				lstCraft.Items.Add(_mapData[i].FullName);
			}
			lstSelection.Visible = false;
			lblSelection.Visible = false;
			refreshVisibility();
		}

		/// <summary>Must be called whenever the selection is changed outside of the list control itself.</summary>
		void updatePreviousCraftSelection()
		{
			_previousCraftSelection = new int[lstCraft.SelectedIndices.Count];
			for (int i = 0; i < lstCraft.SelectedIndices.Count; i++)
				_previousCraftSelection[i] = lstCraft.SelectedIndices[i];
			lstCraft.Refresh();
			lstSelection.Refresh();
		}

		/// <summary>Updates the size and visibility of the list control based on the number of items it contains.</summary>
		void updateSelectionListSize()
		{
			int height = 4 + (lstSelection.ItemHeight * lstSelection.Items.Count);
			if (lstSelection.Top + height > pctMap.Bottom) height = pctMap.Bottom - lstSelection.Top;
			lstSelection.Height = height;
			lblSelection.Visible = (lstSelection.Items.Count > 0);
			lstSelection.Visible = (lstSelection.Items.Count > 0);
		}

		/// <summary>Re-counts how many objects are faded or hidden, updates controls providing this information, and re-sorts the draw order.</summary>
		void refreshVisibility()
		{
			int[] count = new int[3];
			for (int i = 0; i < _mapData.Length; i++)
				count[(int)_mapData[i].View]++;
			lblFade.Text = "Faded: " + count[1] + " craft";
			lblHide.Text = "Hidden: " + count[2] + " craft";
			lblFade.ForeColor = (count[1] > 0 ? Color.Red : SystemColors.WindowText);
			lblHide.ForeColor = (count[2] > 0 ? Color.Red : SystemColors.WindowText);
			scheduleMapPaint();
			lstCraft.Refresh(); // Refresh the tags indicating the visibility state.
			_sortedMapDataList.Sort(compareDrawOrder);
		}

		/// <summary>Performs a middle click action depending on whether something is selected or not, based on user configuration.</summary>
		void middleClick()
		{
			if (_selectionList.Count > 0)
				performMiddleClickAction(_settings.MapMiddleClickActionSelected);
			else
				performMiddleClickAction(_settings.MapMiddleClickActionNoneSelected);
		}

		/// <summary>Performs a middle click action.</summary>
		void performMiddleClickAction(MiddleClickAction action)
		{
			switch (action)
			{
				case MiddleClickAction.ResetToCenter: fitMapToObjects(null); break;
				case MiddleClickAction.DoNothing: break;
				case MiddleClickAction.FitToWorld: fitMapToWorld(); break;
				case MiddleClickAction.FitToSelection: fitMapToObjects(_selectionList); break;
				case MiddleClickAction.CenterOnSelection: centerMapOnSelection(); break;
				default: fitMapToObjects(null); break;
			}
		}

		/// <summary>Determines if the object is enabled and visible.</summary>
		/// <remarks>For most platforms, checks the start point. For XWA, also checks for any hyper jump to the current region.</remarks>
		WaypointVisibility isVisibleInRegion(int mapDataIndex, int waypoint)
		{
			if (_platform != Settings.Platform.XWA)
			{
                if (_mapData[mapDataIndex].WPs[0][waypoint].Enabled)
                    return WaypointVisibility.Present;
                return WaypointVisibility.Absent;
			}

			bool enabled = (waypoint == 0 || _mapData[mapDataIndex].WPs[0][waypoint].Enabled);
			if (!enabled) return WaypointVisibility.Absent;

			int region = (int)numRegion.Value - 1;
			if (_mapData[mapDataIndex].WPs[0][waypoint][4] == (short)region)
				return WaypointVisibility.Present;

			Platform.Xwa.FlightGroup xwaFg = (Platform.Xwa.FlightGroup)_mapData[mapDataIndex].FlightGroup;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (xwaFg.Orders[i, j].Command == 0x32 && xwaFg.Orders[i, j].Variable1 == region) // Hyper to region
						return WaypointVisibility.OtherRegion;
				}
			}
			return WaypointVisibility.Absent;
		}

		/// <summary>Gets a waypoint along the vector defined by two points with the given raw offset from the starting point.</summary>
		/// <param name="start">The origin Waypoint</param>
		/// <param name="end">The end Waypoint to define length and direction of the vector.</param>
		/// <param name="rawOffset">The distance in raw units from <paramref name="start"/>. Negative values are "behind" start.</param>
		/// <returns>A Waypoint of the location.</returns>
		BaseFlightGroup.BaseWaypoint getOffsetWaypoint(BaseFlightGroup.BaseWaypoint start, BaseFlightGroup.BaseWaypoint end, int rawOffset)
		{
			int[] vector = new int[3];
            for (int c = 0; c < 3; c++) vector[c] = end[c] - start[c];
            double vectorLength = Math.Sqrt(Math.Pow(vector[0], 2) + Math.Pow(vector[1], 2) + Math.Pow(vector[2], 2));
            if (vectorLength == 0)
            {
                vector[1] = 1;
                vectorLength = 1;
            }
			var offsetWaypoint = new Platform.Xwa.FlightGroup.Waypoint();
            for (int c = 0; c < 3; c++) offsetWaypoint[c] = (short)(start[c] + rawOffset * vector[c] / vectorLength);
			return offsetWaypoint;
        }

		/// <summary>Converts a waypoint into map coordinates for drawing.</summary>
		/// <param name="waypoint">Source waypoint</param>
		/// <returns>A map point taking into account Zoom and Display Orientation</returns>
		Point getMapPoint(BaseFlightGroup.BaseWaypoint waypoint)
		{
			Point mapPoint = new Point();
            int mX = _mapX, mY = _mapZ, coord1 = 0, coord2 = 2;
            switch (_displayMode)
            {
                case Orientation.XY:
                    mY = _mapY;
                    coord2 = 1;
                    break;
                case Orientation.YZ:
                    mX = _mapY;
                    coord1 = 1;
                    break;
            }
			mapPoint.X = _zoom * waypoint[coord1] / 160 + mX;
            mapPoint.Y = -_zoom * waypoint[coord2] / 160 + mY;
			return mapPoint;
        }

		void processHyperPoints()
		{
            int numCraft = _mapData.Length;
			for (int i = 0; i < numCraft; i++)
			{
				var fg = (Platform.Xwa.FlightGroup)_mapData[i].FlightGroup;

                for (int r = 0; r < 4; r++)
				{
					_mapData[i].WPs[17][r].Enabled = false;
					_mapData[i].WPs[18][r].Enabled = false;
					var exitBuoySP = new Platform.Xwa.FlightGroup.Waypoint();
					exitBuoySP.RawZ = 621;	// just using as a flag
					for (int o = 0; o < 16; o++)
					{
                        int reg = o / 4;
                        int ord = o % 4;
                        if (fg.Orders[reg, ord].Command == 50 && fg.Orders[reg, ord].Variable1 == r)
						{
							for (int b = 0; b < numCraft; b++)
							{
								var buoy = (Platform.Xwa.FlightGroup)_mapData[b].FlightGroup;
                                if (buoy.CraftType == 85 && buoy.Waypoints[0][4] == r && buoy.Designation1 == reg + 12)   // From are #12-15
								{
									exitBuoySP = buoy.Waypoints[0];
									System.Diagnostics.Debug.WriteLine(fg.ToString() + " enters Region " + (r + 1) + " from " + (reg + 1) + " via " + buoy.ToString());
									break;
								}
							}
						}
					}

					var o1w1 = _mapData[i].WPs[r * 4 + 1][0];
					// if returning to the starting region, grab the first order after the Hyper order
					if (exitBuoySP.RawZ != 621 && r == _mapData[i].WPs[0][0][4])
					{
						for (int o = 0; o < 3; o++)
						{
							if (fg.Orders[r, o].Command == 50)
							{
								o1w1 = _mapData[i].WPs[(r * 4 + 1) + (o + 1)][0];
								System.Diagnostics.Debug.WriteLine("return o1w1 used: " + fg.ToString());
							}
						}
					}

					if (fg.PlayerNumber != 0)
					{
						// Player
						for (int o = 0; o < 16; o++)
						{
                            int reg = o / 4;
                            int ord = o % 4;
							if (fg.Orders[reg, ord].Command == 50 && fg.Orders[reg, ord].Variable1 == r)
							{
								var exitPoint = getOffsetWaypoint(exitBuoySP, o1w1, -100);    // .625 km
								for (int c = 0; c < 3; c++) _mapData[i].WPs[17][r][c] = exitPoint[c];
								_mapData[i].WPs[17][r].Enabled = true;
								for (int c = 0; c < 3; c++) _mapData[i].WPs[18][r][c] = o1w1[c];
								_mapData[i].WPs[18][r].Enabled = true;
							}
						}
					}
					else
					{
						// AI
						var hyperEntry = new Platform.Xwa.FlightGroup.Waypoint();
						for (int o = 0; o < 16; o++)
						{
							int reg = o / 4;
							int ord = o % 4;
							if (fg.Orders[reg, ord].Command == 50 && fg.Orders[reg, ord].Variable1 == r)
							{
								for (int w = 1; w < 8; w++)
								{
									if (!fg.Orders[reg, ord].Waypoints[w].Enabled)
									{
										hyperEntry = fg.Orders[reg, ord].Waypoints[w - 1];
										hyperEntry.Region = (byte)reg;
										hyperEntry.Enabled = true;
										break;
									}
								}
							}
						}
						if (!hyperEntry.Enabled) continue;	// this is the "not found"/"doesn't hyper" check

						var enterBuoy = new Platform.Xwa.FlightGroup.Waypoint();
						for (int b = 0; b < numCraft; b++)
						{
                            var buoy = (Platform.Xwa.FlightGroup)_mapData[b].FlightGroup;
                            if (buoy.CraftType == 85 && buoy.Waypoints[0][4] == hyperEntry.Region && buoy.Designation1 == r + 16)  // To are #16-19
							{
								enterBuoy = buoy.Waypoints[0];
								System.Diagnostics.Debug.WriteLine(fg.ToString() + " leaves Region " + (hyperEntry.Region + 1) + " to " + (r + 1) + " via " + buoy.ToString());
								break;
							}
						}
						int[] offset = new int[3];
						for (int c = 0; c < 3; c++) offset[c] = hyperEntry[c] - enterBuoy[c];
						for (int c = 0; c < 3; c++) _mapData[i].WPs[17][r][c] = (short)(exitBuoySP[c] + offset[c]);
						_mapData[i].WPs[17][r].Enabled = true;
						for (int c = 0; c < 3; c++) _mapData[i].WPs[18][r][c] = (short)(o1w1[c] + offset[c]);
						_mapData[i].WPs[18][r].Enabled = true;
					}
				}
			}
        }
        #endregion private methods

        #region public functions
        /// <summary>The down-and-dirty function that handles map display </summary>
        public void MapPaint()
		{
			if (_isClosing || _isLoading) return;

			#region orientation setup
			int mX = _mapX, mY = _mapZ;
			switch (_displayMode)
			{
				case Orientation.XY:
					mY = _mapY;
					break;
				case Orientation.YZ:
					mX = _mapY;
					break;
			}
			#endregion
			#region brush, pen and graphics setup
			// Create a new pen that we shall use for drawing the lines
			Pen pn = new Pen(Color.DarkRed);
			SolidBrush sb = new SolidBrush(Color.Black);
			SolidBrush sbg = new SolidBrush(Color.DimGray); // for FG tags
			Pen pnTrace = new Pen(Color.Gray);      // for WP traces
			Pen pnSel = new Pen(Color.White);  //[JB] For selection boxes
			Pen pnDashTrace = new Pen(Color.DimGray)
			{
				DashStyle = System.Drawing.Drawing2D.DashStyle.Dash,
				DashPattern = new float[] { 4, 2 }
			};
			Graphics g3;
			g3 = Graphics.FromImage(_map);      //graphics obj, load from the memory bitmap
			g3.Clear(SystemColors.Control);     //clear it
			#endregion
			#region BG and grid
			g3.FillRectangle(sb, 0, 0, w, h);       //background
			for (int i = 0; i < 200; i++)
			{
				g3.DrawLine(pn, 0, _zoom * i + mY, w, _zoom * i + mY);    //min lines, every klick
				g3.DrawLine(pn, 0, mY - _zoom * i, w, mY - _zoom * i);
				g3.DrawLine(pn, _zoom * i + mX, 0, _zoom * i + mX, h);
				g3.DrawLine(pn, mX - _zoom * i, 0, mX - _zoom * i, h);
			}
			pn.Width = 3;
			for (int i = 0; i < 40; i++)
			{
				g3.DrawLine(pn, 0, _zoom * i * 5 + mY, w, _zoom * i * 5 + mY);    //maj lines, every 5 klicks
				g3.DrawLine(pn, 0, mY - _zoom * i * 5, w, mY - _zoom * i * 5);
				g3.DrawLine(pn, _zoom * i * 5 + mX, 0, _zoom * i * 5 + mX, h);
				g3.DrawLine(pn, mX - _zoom * i * 5, 0, mX - _zoom * i * 5, h);
			}
			pn.Color = Color.Red;
			pn.Width = 1;
			g3.DrawLine(pn, 0, mY, w, mY);    // origin lines
			g3.DrawLine(pn, mX, 0, mX, h);
			#endregion
			Bitmap bmptemp;
			byte[] bAdd = new byte[3];      // [0] = R, [1] = G, [2] = B
			int[] objects = new int[_mapData.Length];
			if (_sortedMapDataList.Count != _mapData.Length)
			{
				// List was not initialized, or it was changed externally by adding or deleting craft.
				_sortedMapDataList.Clear();
				for (int i = 0; i < _mapData.Length; i++)
					_sortedMapDataList.Add(i);
				_sortedMapDataList.Sort(compareDrawOrder);
			}
			int region = -1;
			if (_platform == Settings.Platform.XWA) region = (int)numRegion.Value - 1;

			foreach(int i in _sortedMapDataList)
			{
				if (_mapData[i].View == Visibility.Hide || !passFilter(_mapData[i]) || isVisibleInRegion(i, 0) == WaypointVisibility.Absent)
					continue;
				pn.Color = (_mapData[i].View == Visibility.Fade ? _fadeColor : getIFFColor(_mapData[i].IFF));
				pnTrace.Color = (_mapData[i].View == Visibility.Fade ? _fadeColor : Color.Gray);
				bAdd[0] = pn.Color.R;
				bAdd[1] = pn.Color.G;
				bAdd[2] = pn.Color.B;
				try { bmptemp = new Bitmap(imgCraft.Images[_mapData[i].Craft]); }
				catch { bmptemp = new Bitmap(imgCraft.Images[0]); }
				bmptemp = mask(bmptemp, bAdd);
				// work through each WP and determine if it needs to be displayed, then place it on the map
				// draw tags if required
				// if previous sequential WP is checked and trace is required, draw trace line according to WP type
				for (int k = 0; k < 4; k++) // Start
				{
					if (chkWP[k].Checked && isVisibleInRegion(i, k) == WaypointVisibility.Present)
					{
                        var startPoint = getMapPoint(_mapData[i].WPs[0][k]);
                        drawCraft(g3, bmptemp, _mapData[i], startPoint.X, startPoint.Y);
						if (chkTags.Checked && _mapData[i].View == Visibility.Show) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, DefaultFont, sbg, startPoint.X + 8, startPoint.Y + 8);
					}
					else if (isVisibleInRegion(i, k) == WaypointVisibility.OtherRegion)
                    {
						var exitWP = _mapData[i].WPs[17][region];
						var exitPoint = getMapPoint(exitWP);
						drawCraft(g3, bmptemp, _mapData[i], exitPoint.X, exitPoint.Y);
						if (chkTags.Checked && _mapData[i].View == Visibility.Show) g3.DrawString(_mapData[i].Name + " Exit", DefaultFont, sbg, exitPoint.X + 8, exitPoint.Y + 8);
						var exitDirection = getMapPoint(getOffsetWaypoint(exitWP, _mapData[i].WPs[18][region], -50));	// .31 km
						g3.DrawLine(pnDashTrace, exitPoint.X, exitPoint.Y, exitDirection.X, exitDirection.Y);	// hyper tail leading into the exit
					}
				}
				if (_platform == Settings.Platform.XWA) // WPs     [JB] XWA's north/south is inverted compared to XvT.
				{
					int ord = (int)(region * 4 + (numOrder.Value - 1) + 1);
                    bool offset = false;
					bool pointing = false;
                    for (int k = 0; k < 8; k++)
					{
						if (k == 0 && _mapData[i].WPs[18][region].Enabled)
						{
							// only trigger the offset in the first order in a new region or the first return order in the starting region
							if (_mapData[i].WPs[0][0][4] != region && numOrder.Value == 1) offset = true;
							else
							{
								var fg = (Platform.Xwa.FlightGroup)_mapData[i].FlightGroup;
								for (int o = 0; o < 3; o++)
								{
									if (fg.Orders[region, o].Command == 50 && (o + 2) == numOrder.Value)
									{
										offset = true;
										break;
									}
								}
							}
						}
						
                        if (chkWP[k + 4].Checked && _mapData[i].WPs[ord][k].Enabled)
						{
							var ordPoint = getMapPoint(_mapData[i].WPs[ord][k]);
							pointing = offset && k == 0;
							if (pointing) ordPoint = getMapPoint(_mapData[i].WPs[18][region]);
                            g3.DrawEllipse(pn, ordPoint.X - 1, ordPoint.Y - 1, 3, 3);
							if (chkTags.Checked && _mapData[i].View == Visibility.Show)
								g3.DrawString(_mapData[i].Name + " " + (pointing ? "Aim" : chkWP[k + 4].Text), DefaultFont, sbg, ordPoint.X + 4, ordPoint.Y + 4);
							if (chkTrace.Checked && !(chkTraceHideFade.Checked && _mapData[i].View == Visibility.Fade) && !(chkTraceSelected.Checked && !isMapObjectSelected(i)))
							{
								var baseWp = _mapData[i].WPs[0][0];
								if (_mapData[i].WPs[17][region].Enabled && ((Platform.Xwa.FlightGroup.Waypoint)_mapData[i].WPs[0][0]).Region != region)
									baseWp = _mapData[i].WPs[17][region];
								if (k == 0 && (!chkWP[0].Checked || isVisibleInRegion(i, 0) == WaypointVisibility.Absent))
									continue;
								else if (k > 0)
								{
									if (offset && k == 1)
										baseWp = _mapData[i].WPs[17][region];   // trace WP2 back to hyper exit
									else
									{
                                        if (!chkWP[k + 3].Checked)
                                            continue;
                                        baseWp = _mapData[i].WPs[ord][k - 1];
                                    }
								}
								var basePoint = getMapPoint(baseWp);
								if (pointing) g3.DrawLine(pnDashTrace, basePoint.X, basePoint.Y, ordPoint.X, ordPoint.Y);
                                else g3.DrawLine(pnTrace, basePoint.X, basePoint.Y, ordPoint.X, ordPoint.Y);
								if (_mapData[i].View == Visibility.Show && !pointing)
								{
									int offy = chkTags.Checked ? 14 : 0; //To render it below the FG tag
									if (chkDistance.Checked)
									{
										g3.DrawString(getDistanceString(_mapData[i].WPs[ord][k], baseWp), DefaultFont, sbg, ordPoint.X + 4, ordPoint.Y + 4 + offy);
										offy += 14;
									}
									if (chkTime.Checked)
									{
										g3.DrawString(getTimeString(_mapData[i].FlightGroup, (int)numOrder.Value - 1, _mapData[i].WPs[ord][k], baseWp), DefaultFont, sbg, ordPoint.X + 4, ordPoint.Y + 4 + offy);
									}
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
							var waypoint = getMapPoint(_mapData[i].WPs[0][k]);
							g3.DrawEllipse(pn, waypoint.X - 1, waypoint.Y - 1, 3, 3);
							if (chkTags.Checked && _mapData[i].View == Visibility.Show) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, DefaultFont, sbg, waypoint.X + 4, waypoint.Y + 4);
							if (chkWP[(k == 4 ? 0 : (k - 1))].Checked && chkTrace.Checked && !(chkTraceHideFade.Checked && _mapData[i].View == Visibility.Fade) && !(chkTraceSelected.Checked && !isMapObjectSelected(i)))
							{
								int comp = (k == 4 ? 0 : (k - 1));
								var previousPoint = getMapPoint(_mapData[i].WPs[0][comp]);
								g3.DrawLine(pnTrace, previousPoint.X, previousPoint.Y, waypoint.X, waypoint.Y);
								if (_mapData[i].View == Visibility.Show)
								{
									int offy = chkTags.Checked ? 14 : 0; //To render it below the FG tag
									if (chkDistance.Checked)
									{
										g3.DrawString(getDistanceString(_mapData[i].WPs[0][k], _mapData[i].WPs[0][comp]), DefaultFont, sbg, waypoint.X + 4, waypoint.Y + 4 + offy);
										offy += 14;
									}
									if (chkTime.Checked)
									{
										g3.DrawString(getTimeString(_mapData[i].FlightGroup, (int)(numOrder.Value - 1), _mapData[i].WPs[0][k], _mapData[i].WPs[0][comp]), DefaultFont, sbg, waypoint.X + 4, waypoint.Y + 4 + offy);
									}
								}
							}
						}
					}

					if (chkWP[12].Checked && _mapData[i].WPs[0][12].Enabled) // RND
					{
						var rndPoint = getMapPoint(_mapData[i].WPs[0][12]);
						g3.DrawEllipse(pn, rndPoint.X - 1, rndPoint.Y - 1, 3, 3);
						if (chkTags.Checked && _mapData[i].View == Visibility.Show) g3.DrawString(_mapData[i].Name + " " + chkWP[12].Text, DefaultFont, sbg, rndPoint.X + 4, rndPoint.Y + 4);
					}
					if (chkWP[13].Checked && _mapData[i].WPs[0][13].Enabled)    // HYP
					{
                        var hypPoint = getMapPoint(_mapData[i].WPs[0][13]);
                        g3.DrawEllipse(pn, hypPoint.X - 1, hypPoint.Y - 1, 3, 3);
						if (chkTags.Checked && _mapData[i].View == Visibility.Show) g3.DrawString(_mapData[i].Name + " " + chkWP[13].Text, DefaultFont, sbg, hypPoint.X + 4, hypPoint.Y + 4);
						if (chkTrace.Checked && !(chkTraceHideFade.Checked && _mapData[i].View == Visibility.Fade) && !(chkTraceSelected.Checked && !isMapObjectSelected(i)))
						{
							// in this case, make sure last visible WP is the last enabled before tracing to HYP
							pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
							for (int k = 4; k < 12; k++)
							{
								if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled && (k == 11 || !_mapData[i].WPs[0][k + 1].Enabled))
								{
									var lastPoint = getMapPoint(_mapData[i].WPs[0][k]);
									g3.DrawLine(pnTrace, lastPoint.X, lastPoint.Y, hypPoint.X, hypPoint.Y);
									break;
								}
							}
							pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
						}
					}
					for (int k = 14; k < 22; k++)   // BRF
					{
						if (chkWP[k].Checked && _mapData[i].WPs[0][k].Enabled)
						{
                            var brfPoint = getMapPoint(_mapData[i].WPs[0][k]);
                            g3.DrawImageUnscaled(bmptemp, brfPoint.X - 8, brfPoint.Y - 8);
							if (chkTags.Checked && _mapData[i].View == Visibility.Show) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, DefaultFont, sbg, brfPoint.X + 8, brfPoint.Y + 8);
						}
					}
				}
			}
			foreach (SelectionData dat in _selectionList)
			{
				if (!dat.Active)
					continue;
				var datPoint = getMapPoint(dat.WPRef);
				if (_platform == Settings.Platform.XWA && dat.MapDataRef.WPs[17][region].Enabled && ((Platform.Xwa.FlightGroup.Waypoint)dat.WPRef).Region != (numRegion.Value - 1))
				{
                    datPoint = getMapPoint(dat.MapDataRef.WPs[17][region]);
                }
				int x = datPoint.X;
				int y = datPoint.Y;
							//[JB] Draws a four corner selection box like in-game.
				g3.DrawLine(pnSel, x - 8, y - 8, x - 4, y - 8); //Horizontal top
				g3.DrawLine(pnSel, x + 4, y - 8, x + 8, y - 8);
				g3.DrawLine(pnSel, x - 8, y + 8, x - 4, y + 8); //Horizontal bottom
				g3.DrawLine(pnSel, x + 4, y + 8, x + 8, y + 8);
				g3.DrawLine(pnSel, x - 8, y - 8, x - 8, y - 4); //Vertical Left
				g3.DrawLine(pnSel, x - 8, y + 4, x - 8, y + 8);
				g3.DrawLine(pnSel, x + 8, y - 8, x + 8, y - 4); //Vertical right
				g3.DrawLine(pnSel, x + 8, y + 4, x + 8, y + 8);
			}
			if (_dragSelectActive)
			{
				Pen sel = new Pen(Color.White);
				Point p1 = _clickPixelDown;
				Point p2 = _clickPixelUp;
				normalizePoint(ref p1, ref p2);
				Rectangle r = new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
				g3.DrawRectangle(sel, r);
			}
			pctMap.Invalidate();        // since it's drawing to memory, this refreshes the pct.  Removes the flicker when zooming
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
				_mapData[i] = new MapData(_platform)
				{
					Craft = fg[i].GetTIECraftType(),
					FgIndex = i,
					IFF = fg[i].GetTIEIFF(),
					Name = fg[i].Name,
					FlightGroup = fg[i],
					Difficulty = 0b111
				};
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
				if (fg[i].IFF == 0 && fg[i].IsObjectGroup()) _mapData[i].IFF = 1; //None/Default objects appear as Imperial.
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
			for (int i = 0; i < numCraft; i++)
			{
				_mapData[i] = new MapData(_platform)
				{
					Craft = fg[i].CraftType,
					FgIndex = i,
					IFF = fg[i].IFF,
					Name = fg[i].Name,
					FlightGroup = fg[i],
					Difficulty = getDifficultyFlags(fg[i].Difficulty)
				};
				_mapData[i].WPs[0] = fg[i].Waypoints;
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
			for (int i = 0; i < numCraft; i++)
			{
				_mapData[i] = new MapData(_platform)
				{
					Craft = fg[i].CraftType,
					FgIndex = i,
					IFF = fg[i].IFF,
					Name = fg[i].Name,
					FlightGroup = fg[i],
					Difficulty = getDifficultyFlags(fg[i].Difficulty)
				};
				_mapData[i].WPs[0] = fg[i].Waypoints;
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
			for (int i = 0; i < numCraft; i++)
			{
				_mapData[i] = new MapData(_platform)
				{
					Craft = fg[i].CraftType,
					FgIndex = i,
					IFF = fg[i].IFF,
					Name = fg[i].Name,
					FlightGroup = fg[i],
					Difficulty = getDifficultyFlags(fg[i].Difficulty),
					Region = fg[i].Waypoints[0].Region,
					Pitch = fg[i].Pitch,
					Yaw = fg[i].Yaw,
					Roll = fg[i].Roll,
				};
				_mapData[i].WPs[0] = fg[i].Waypoints;
				for (int j = 0; j < 16; j++)
				{
					int region = j / 4;
					int order = j % 4;
					_mapData[i].WPs[j + 1] = fg[i].Orders[region, order].Waypoints;
				}
				_mapData[i].FullName = Platform.Xwa.Strings.CraftAbbrv[_mapData[i].Craft] + " " + fg[i].Name;
			}
            processHyperPoints();
            reloadSelectionControls();
		}

		/// <summary>Updates the mapdata properties of a FlightGroup.</summary>
		/// <remarks>Should be called by the main program form during minor data adjustments.  For major changes such as FG delete, use <see cref="Import"/> for a complete refresh.</remarks>
		/// <param name="index">Index of the FlightGroup.</param>
		/// <param name="fg">The FlightGroup object to pull information from.  Compatible with all platforms.</param>
		public void UpdateFlightGroup(int index, BaseFlightGroup fg)
		{
			if (IsDisposed || _isClosing) return;
			if (index < 0 || index >= _mapData.Length || fg == null) return;
			_mapData[index].Name = fg.Name;
			_mapData[index].IFF = fg.IFF;
			_mapData[index].Craft = fg.CraftType;
			_mapData[index].Difficulty = getDifficultyFlags(fg.Difficulty);
			//Waypoints are attached by reference, so there's no need to update waypoints or position here.
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
					_mapData[index].Region = ((Platform.Xwa.FlightGroup)fg).Waypoints[0].Region;
					_mapData[index].Yaw = ((Platform.Xwa.FlightGroup)fg).Yaw;
					_mapData[index].Pitch = ((Platform.Xwa.FlightGroup)fg).Pitch;
					_mapData[index].Roll = ((Platform.Xwa.FlightGroup)fg).Roll;
                    processHyperPoints();
                    break;
			}
			if (abbrev != null)
				_mapData[index].FullName = abbrev[_mapData[index].Craft] + " " + fg.Name;

			_ignoreSelectionEvents = true;
			bool state = lstCraft.GetSelected(index);  //For some reason the selection still changes (probably different event?) so backup/restore state to be sure.
			lstCraft.Items[index] = _mapData[index].FullName;
			lstCraft.Refresh();  //In case IFF changed, force redraw for color change
			lstCraft.SetSelected(index, state);
			for (int i = 0; i < _selectionList.Count; i++)
			{
				if (_selectionList[i].MapDataIndex == index)
				{
					state = lstSelection.GetSelected(i);
					lstSelection.Items[i] = _mapData[index].FullName + (_selectionList[i].WpIndex > 0 ? "  " + chkWP[_selectionList[i].WpIndex].Text : "");
					lstSelection.Refresh();  //For IFF
					lstSelection.SetSelected(i, state);
				}
			}
			_ignoreSelectionEvents = false;

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
				centerMapOnSelection();
				lblCoor1.Text = "X:";
				lblCoor2.Text = "Y:";
			}
		}
		/// <summary>Rotate map to Front view</summary>
		void optXZ_CheckedChanged(object sender, EventArgs e)
		{
			if (optXZ.Checked)
			{
				_displayMode = Orientation.XZ;
				centerMapOnSelection();
				lblCoor1.Text = "X:";
				lblCoor2.Text = "Z:";
			}
		}
		/// <summary>Rotate map to Side view </summary>
		void optYZ_CheckedChanged(object sender, EventArgs e)
		{
			if (optYZ.Checked)
			{
				_displayMode = Orientation.YZ;
				centerMapOnSelection();
				lblCoor1.Text = "Y:";
				lblCoor2.Text = "Z:";
			}
			else _mapY = w / 2 + h / 2 - _mapY;
		}

		/// <summary>Timer that handles the calling of the actual map rendering, when using TimeRestrictedMapPaint()</summary>
		void mapPaintRedrawTimer_Tick(object sender, EventArgs e)
		{
			if (_mapPaintScheduled)
			{
				MapPaint();
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
			// move map, center on mouse
			if (e.Button == MouseButtons.Right)
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
			else if (e.Button == MouseButtons.Left)
			{
				_leftButtonDown = true;
				_dragMoveSnapReady = false;

				if (!_rightButtonDown)
				{
					if (!_dragSelectActive) convertMousepointToWaypoint(e.X, e.Y, ref _clickMapDown);

					_clickPixelDown.X = e.X;
					_clickPixelDown.Y = e.Y;

					 // Multiple objects might be stacked in the same place, so enum all for the purposes of determining if any are selected.
					 // If nothing is here: initiate a drag-selection box while the button is pressed.
					 // If something is here, but unselected: select it.
					 // If something is here and selected: initiate a drag-move operation while the button is pressed.
					List<SelectionData> objects = new List<SelectionData>();
					enumSelectionObjects(ref _clickPixelDown, ref _clickPixelDown, objects);
					bool isObjectAtPoint = isListObjectSelected(objects);
					if (!_shiftState && !isObjectAtPoint) _dragSelectActive = true;
					else if (!_shiftState && isObjectAtPoint && !ModifierKeys.HasFlag(Keys.Control)) _dragMoveActive = true;
					else if (_shiftState && _selectionList.Count > 0) _dragMoveActive = true;

					convertMousepointToWaypoint(e.X, e.Y, ref _clickMapUp);
					_dragMapPrevious = _clickMapUp;
				}
			}
			else if (e.Button == MouseButtons.Middle)
			{
				middleClick();
			}
		}
		void pctMap_MouseEnter(object sender, EventArgs e)
		{
			if (_hasFocus)
			{
				pctMap.Focus();
				_mapFocus = true;
			}
		}
		void pctMap_MouseLeave(object sender, EventArgs e) { _mapFocus = false; }
		void pctMap_MouseMove(object sender, MouseEventArgs e)
		{
			if (_leftButtonDown)
			{
				_clickPixelUp.X = e.X;
				_clickPixelUp.Y = e.Y;
				convertMousepointToWaypoint(e.X, e.Y, ref _clickMapUp);

				//Calling MapPaint() directly every "frame" of mouse movement when there's a lot of items to draw, will produce a significant amount of slowdown. 
				if (_dragSelectActive)
				{
					scheduleMapPaint(); //Repaint to draw selection box.
				}
				else if((_shiftState ||_dragMoveActive) && _selectionList.Count > 0)
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
					msX = (e.X - _mapX) / Convert.ToDouble(_zoom);
					msY = (_mapY - e.Y) / Convert.ToDouble(_zoom);
					lblCoor1.Text = "X: " + Math.Round(msX, 2).ToString();
					lblCoor2.Text = "Y: " + Math.Round(msY, 2).ToString();
					break;
				case Orientation.XZ:
					msX = (e.X - _mapX) / Convert.ToDouble(_zoom);
					msY = (_mapZ - e.Y) / Convert.ToDouble(_zoom);
					lblCoor1.Text = "X: " + Math.Round(msX, 2).ToString();
					lblCoor2.Text = "Z: " + Math.Round(msY, 2).ToString();
					break;
				case Orientation.YZ:
					msX = (e.X - _mapY) / Convert.ToDouble(_zoom);
					msY = (_mapZ - e.Y) / Convert.ToDouble(_zoom);
					lblCoor1.Text = "Y: " + Math.Round(msX, 2).ToString();
					lblCoor2.Text = "Z: " + Math.Round(msY, 2).ToString();
					break;
			}
		}
		void pctMap_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_leftButtonDown = false;
				_clickPixelUp.X = e.X;
				_clickPixelUp.Y = e.Y;
				convertMousepointToWaypoint(e.X, e.Y, ref _clickMapUp);
				_dragSelectActive = false;  // Set to false so the selection box won't be painted.
				if (!_shiftState && !_dragMoveActive)
				{
					performSelection();
					MapPaint();
				}
				if (_dragMoveActive)
				{
					moveSelectionToCursor();
					_dragMoveActive = false;
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				_rightButtonDown = false;
				if (!_leftButtonDown)
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
#endregion
		#region frmMap
		void form_Activated(object sender, EventArgs e)
		{
			_hasFocus = true;
			chkLimit.Text = "Only above " + _settings.WireframeIconThresholdSize + "m";
			MapPaint();
		}
		void form_Deactivate(object sender, EventArgs e) { _hasFocus = false; }
		void form_FormClosed(object sender, FormClosedEventArgs e) { _map.Dispose(); }
		void form_FormClosing(object sender, FormClosingEventArgs e)
		{
			_settings.MapSnapTo = Convert.ToByte(cboSnapTo.SelectedIndex);
			_settings.MapSnapAmount = Convert.ToSingle(numSnapAmount.Value);
			_settings.MapSnapUnit = Convert.ToByte(cboSnapUnit.SelectedIndex);
			Settings.MapOpts mapOpts = Settings.MapOpts.None;
			if (chkTags.Checked) mapOpts |= Settings.MapOpts.FGTags;
			if (chkTrace.Checked) mapOpts |= Settings.MapOpts.Traces;
			if (chkDistance.Checked) mapOpts |= Settings.MapOpts.TraceDistance;
			if (chkTime.Checked) mapOpts |= Settings.MapOpts.TraceTime;
			if (chkTraceHideFade.Checked) mapOpts |= Settings.MapOpts.TraceHideFade;
			if (chkTraceSelected.Checked) mapOpts |= Settings.MapOpts.TraceSelected;
			_settings.MapOptions = mapOpts;
			_settings.WireframeEnabled = chkWireframe.Checked;
			_settings.WireframeIconThresholdEnabled = chkLimit.Checked;

			onDataModified = null;
			_isClosing = true;
			//[JB] Stop and deactivate the timer.
			//Important! There's an issue where the event can trigger after the map is disposed, even after calling Stop(). The event must be unregistered.
			mapPaintRedrawTimer.Stop();
			mapPaintRedrawTimer.Tick -= mapPaintRedrawTimer_Tick;
		}
		void form_Load(object sender, EventArgs e)
		{
			_map = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			MapPaint();
		}
		void form_MouseWheel(object sender, MouseEventArgs e)
		{
			double mult = _settings.MapMouseWheelZoomPercentage / 100.0;
			if (mult < 0.01) mult = 0.01; else if (mult > 0.5) mult = 0.5;

			int amount = (int)(hscZoom.Value * mult);
			if (amount < 1)
				amount = 1;

			int newZoom = hscZoom.Value + (amount * Math.Sign(e.Delta));
			if (newZoom < hscZoom.Minimum) newZoom = hscZoom.Minimum;
			else if (newZoom > hscZoom.Maximum) newZoom = hscZoom.Maximum;
			hscZoom.Value = newZoom;
		}
		void form_Resize(object sender, EventArgs e)
		{
			if (!_isDragged) updateLayout();
		}
		void form_ResizeBegin(object sender, EventArgs e)
		{
			_isDragged = true;
		}
		void form_ResizeEnd(object sender, EventArgs e)
		{
			_isDragged = false;
			updateLayout();
		}
		void form_KeyDown(object sender, KeyEventArgs e)
		{
			// By default, pressing a key tries to select the next item matching that character. Suppress that behavior.
			if (ActiveControl == lstCraft || ActiveControl == lstSelection)
				e.SuppressKeyPress = true;

			// Keys for the entire form.
			if (_platform == Settings.Platform.XWA && e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F4)
			{
				int region = e.KeyCode - Keys.F1;
				if (!e.Control)
					numRegion.Value = region + 1;
				else
					moveSelectionToRegion(region);
			}

			switch (e.KeyCode)
			{
				case Keys.Escape:
					deselect();
					scheduleMapPaint();
					break;
				case Keys.Space:
					centerMapOnSelection();
					break;
				case Keys.S:
					cmdHideSubtract_Click(0, new EventArgs());  // Reselects any that were previously selected.
					cmdHideNone_Click(0, new EventArgs());
					cmdFadeNone_Click(0, new EventArgs());
					break;
				case Keys.F:
					cmdFadeAdd_Click(0, new EventArgs());
					break;
				case Keys.H:
					cmdHideAdd_Click(0, new EventArgs());
					break;
				case Keys.C:
					expandSelection(ExpandSelection.Craft);
					break;
				case Keys.I:
					expandSelection(ExpandSelection.Iff);
					break;
				case Keys.Z:
					expandSelection(ExpandSelection.Size);
					break;
				case Keys.V:
					expandSelection(ExpandSelection.Invert);
					break;
				case Keys.Q:
					if(_selectionList.Count > 0)
						fitMapToObjects(_selectionList);
					break;
				case Keys.W:
					fitMapToWorld();
					break;
			}

			if(_mapFocus)
			{
				// Keys specific to pctMap.
				_shiftState = e.Shift;
				if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
					createOrSelectWaypoint(e);
				switch (e.KeyCode)
				{
					case Keys.Up: moveSelectionByKey(e, 0, 1); break;
					case Keys.Down: moveSelectionByKey(e, 0, -1); break;
					case Keys.Left: moveSelectionByKey(e, -1, 0); break;
					case Keys.Right: moveSelectionByKey(e, 1, 0); break;
					case Keys.Delete: disableSelectionWaypoints(); 	break;
				}
			}
			e.Handled = true;
		}
		void form_KeyUp(object sender, KeyEventArgs e)
		{
			if (_mapFocus)
				_shiftState = e.Shift;
		}
		#endregion

		#region Checkboxes
		void chkTags_CheckedChanged(object sender, EventArgs e) { if (!_isLoading) MapPaint(); }
		void chkTrace_CheckedChanged(object sender, EventArgs e)
		{
			if (!_isLoading)
			{
				MapPaint();
				chkDistance.Enabled = chkTrace.Checked;
				chkTime.Enabled = chkTrace.Checked;
				chkTraceHideFade.Enabled = chkTrace.Checked;
				chkTraceSelected.Enabled = chkTrace.Checked;
			}
		}
		void chkDistance_CheckedChanged(object sender, EventArgs e) { if (!_isLoading) MapPaint(); }
		void chkTime_CheckedChanged(object sender, EventArgs e) { if (!_isLoading) MapPaint(); }
		void chkTraceHideFade_CheckedChanged(object sender, EventArgs e) { if (!_isLoading) MapPaint(); }
		void chkTraceSelected_CheckedChanged(object sender, EventArgs e) { if (!_isLoading) MapPaint(); }
		void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_isLoading) return;
			if ((CheckBox)sender == chkWP[14] && chkWP[14].Checked) for (int i = 0; i < 14; i++) chkWP[i].Checked = false;
			if (((CheckBox)sender).Checked == false) //[JB] Disabled points might still be selected.
				deselect();
			MapPaint();
		}
		void chkWireframe_CheckedChanged(object sender, EventArgs e) { if (!_isLoading) MapPaint(); }
		void chkLimit_CheckedChanged(object sender, EventArgs e) { if (!_isLoading) MapPaint(); }
#endregion

		#region Selection and visibility
		void lstCraft_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1) return;
			e.DrawBackground();
			Brush brText = getDrawColor(_mapData[e.Index]);
			if (_platform == Settings.Platform.XWA && isVisibleInRegion(e.Index, 0) == WaypointVisibility.Absent)
				brText = Brushes.Gray;
			e.Graphics.DrawString(lstCraft.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
			if (_mapData[e.Index].View != Visibility.Show)
			{
				int size = e.Bounds.Bottom - e.Bounds.Top;
				Rectangle bounds = new Rectangle(e.Bounds.Right - size, e.Bounds.Top, size, size);
				e.Graphics.FillRectangle(Brushes.Purple, bounds);
				e.Graphics.DrawString((_mapData[e.Index].View == Visibility.Fade ? "F" : "H"), e.Font, Brushes.White, bounds, StringFormat.GenericDefault);
			}
		}
		void lstSelection_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (_ignoreSelectionEvents || e.Index == -1) return;
			e.DrawBackground();
			Brush brText = getDrawColor(_mapData[_selectionList[e.Index].MapDataIndex]);
			e.Graphics.DrawString(lstSelection.Items[e.Index].ToString(), e.Font, brText, e.Bounds, StringFormat.GenericDefault);
		}
		void lstCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_ignoreSelectionEvents) return;
			_ignoreSelectionEvents = true;

			// Remove any filtered items from the selection.
			int startCount = lstCraft.SelectedIndices.Count;
			int si = 0;
			while (si < lstCraft.SelectedIndices.Count)
			{
				if (!passFilter(_mapData[lstCraft.SelectedIndices[si]]))
					lstCraft.SetSelected(lstCraft.SelectedIndices[si], false);
				else
					si++;
			}
			if (startCount > 0 && lstCraft.SelectedIndices.Count == 0)
			{
				if (++_repeatSelectCount >= 3)
				{
					MessageBox.Show("You're trying to select something that has been filtered out. Change the Difficulty and IFF filters at the bottom of this list.", "Hint", MessageBoxButtons.OK, MessageBoxIcon.Information);
					_repeatSelectCount = 0;
				}
			}
			if(lstCraft.SelectedIndices.Count > 0)
				_repeatSelectCount = 0;

			bool found;
			// Since the list is multi-select, add or remove selections based on changes from its last known state.
			foreach (int i in lstCraft.SelectedIndices)
			{
				found = false;
				if (_previousCraftSelection != null)
				{
					foreach (int k in _previousCraftSelection)
					{
						if (i == k)
						{
							found = true;
							break;
						}
					}
				}
				if (!found)
					setSelectionState(i, true, 0, 0);
			}
			if (_previousCraftSelection != null)
			{
				foreach (int i in _previousCraftSelection)
				{
					found = false;
					foreach (int k in lstCraft.SelectedIndices)
					{
						if (i == k)
						{
							found = true;
							break;
						}
					}
					if(!found)
						setSelectionState(i, false, 0, 0);
				}
			}
			_ignoreSelectionEvents = false;
			updateSelectionListSize();
			updatePreviousCraftSelection();
			MapPaint();
		}
		void lstSelection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_ignoreSelectionEvents) return;

			for (int i = 0; i < _selectionList.Count; i++)
				_selectionList[i].Active = false;
			foreach (int si in lstSelection.SelectedIndices)
				_selectionList[si].Active = true;
			MapPaint();
		}

		void cmdFadeAdd_Click(object sender, EventArgs e)
		{
			foreach (SelectionData dat in _selectionList)
				if (dat.Active)
					dat.MapDataRef.View = Visibility.Fade;
			refreshVisibility();
		}
		void cmdFadeSubtract_Click(object sender, EventArgs e)
		{
			foreach (SelectionData dat in _selectionList)
				if(dat.Active && dat.MapDataRef.View == Visibility.Fade)
					dat.MapDataRef.View = Visibility.Show;
			refreshVisibility();
		}
		void cmdFadeNone_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < _mapData.Length; i++)
				if (_mapData[i].View == Visibility.Fade)
					_mapData[i].View = Visibility.Show;
			refreshVisibility();
		}		
		void cmdHideAdd_Click(object sender, EventArgs e)
		{
			_ignoreSelectionEvents = true;
			for(int i = 0; i < _selectionList.Count; i++)
			{
				if (_selectionList[i].Active)
				{
					_selectionList[i].MapDataRef.View = Visibility.Hide;
					_selectionList[i].Active = false;
					lstSelection.SetSelected(i, false);
				}
			}
			_ignoreSelectionEvents = false;
			refreshVisibility();
		}
		void cmdHideSubtract_Click(object sender, EventArgs e)
		{
			_ignoreSelectionEvents = true;
			for(int i = 0; i < _selectionList.Count; i++)
			{
				if (_selectionList[i].MapDataRef.View == Visibility.Hide)
				{
					_selectionList[i].MapDataRef.View = Visibility.Show;
					_selectionList[i].Active = true;
					lstSelection.SetSelected(i, true);
				}
			}
			_ignoreSelectionEvents = false;
			refreshVisibility();
		}
		void cmdHideNone_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < _mapData.Length; i++)
				if (_mapData[i].View == Visibility.Hide)
					_mapData[i].View = Visibility.Show;
			refreshVisibility();
		}

		void cmdExpandByCraft_Click(object sender, EventArgs e) { expandSelection(ExpandSelection.Craft); }
		void cmdExpandByIff_Click(object sender, EventArgs e) { expandSelection(ExpandSelection.Iff); }
		void cmdExpandBySize_Click(object sender, EventArgs e) { expandSelection(ExpandSelection.Size); }
		void cmdInvertSelection_Click(object sender, EventArgs e) { expandSelection(ExpandSelection.Invert); }

		void cboViewDifficulty_SelectedIndexChanged(object sender, EventArgs e)
		{
			deselect();
			scheduleMapPaint();
			lstCraft.Invalidate();
		}
		void cboViewIff_SelectedIndexChanged(object sender, EventArgs e)
		{
			deselect();
			scheduleMapPaint();
			lstCraft.Invalidate();
		}
#endregion Selection and visibility

		void numOrder_ValueChanged(object sender, EventArgs e) { if (!_isLoading) { deselect(); MapPaint(); } }  //[JB] Added deselect
		void numRegion_ValueChanged(object sender, EventArgs e)
		{
			if (!_isLoading)
			{
				deselect();
				MapPaint();
				if(_platform == Settings.Platform.XWA)
					lstCraft.Refresh();  // Refresh drawing colors for region.
			}
		}

		void cboSnapUnit_SelectedIndexChanged(object sender, EventArgs e)
		{
			setSnapUnit(cboSnapUnit.SelectedIndex);
		}
		void cboSnapTo_SelectedIndexChanged(object sender, EventArgs e)
		{
			cboSnapUnit.Enabled = (cboSnapTo.SelectedIndex > 0);
			numSnapAmount.Enabled = (cboSnapTo.SelectedIndex > 0);
		}

		void cmdFitSelected_Click(object sender, EventArgs e)
		{
			if(_selectionList.Count > 0)
				fitMapToObjects(_selectionList);
		}
		void cmdFitWorld_Click(object sender, EventArgs e)
		{
			fitMapToWorld();
		}
		void cmdFitDefault_Click(object sender, EventArgs e)
		{
			fitMapToObjects(null);
		}
		void cmdCenterSelected_Click(object sender, EventArgs e)
		{
			centerMapOnSelection();
		}
		void cmdHelp_Click(object sender, EventArgs e)
		{
			string text = "Left-click: select at cursor, or drag to select multiple objects." + Environment.NewLine;
			text += "Ctrl left-click a single object to toggle selection." + Environment.NewLine;
			text += "Ctrl left-drag to add multiple objects to selection." + Environment.NewLine;
			text += "Middle-click: fit map to world. Can customize in Options." + Environment.NewLine;
			text += "Right-click: drag map to scroll." + Environment.NewLine;
			text += "Escape: deselect all.  Spacebar: center on selection." + Environment.NewLine;
			text += "Q: fit map to selection.  W: fit map to world." + Environment.NewLine;
			text += Environment.NewLine;
			text += "Objects must be selected before they can be moved." + Environment.NewLine;
			text += "Left-click drag any selected object to move them all." + Environment.NewLine;
			text += "Or hold Shift key while dragging." + Environment.NewLine;
			text += "Or use arrow keys. Hold Shift to move slower, Ctrl to move faster. You can specify a distance with the snap amount, but arrow keys always snap to self." + Environment.NewLine;
			text += Environment.NewLine;
			text += "If the map is too cluttered, you can Fade or Hide selected objects." + Environment.NewLine;
			text += "Fade (F key) will darken their appearance to make surrounding objects easier to see." + Environment.NewLine;
			text += "Hide (H key) will make them invisible." + Environment.NewLine;
			text += "Show All (S key) will unfade and unhide all objects." + Environment.NewLine;
			text += "There are quick filters for difficulty and IFF below the craft list." + Environment.NewLine;
			text += "Waypoint trace lines have more options. See the checkboxes in the bottom right." + Environment.NewLine;
			text += Environment.NewLine;
			text += "Press C to expand the current selection by matching craft types." + Environment.NewLine;
			text += "Press I to expand by matching IFF." + Environment.NewLine;
			text += "Press Z to expand by similar size (wireframes must be enabled)." + Environment.NewLine;
			text += "Press V to invert the current selection. Useful to select the objects you want to keep, then hide the rest." + Environment.NewLine;
			text += "Expanding the selection only considers objects in the visible map area. Position the map and zoom level to get a better frame." + Environment.NewLine;
			text += Environment.NewLine;
			text += "Use the number keys (in the number row) to select or create waypoints." + Environment.NewLine;
			text += "Press once to select (if it exists). Double-tap to create at cursor." + Environment.NewLine;
			text += "1 to 8: WP1 to WP8" + Environment.NewLine;
			text += "Shift 1 to 4: SP1 to SP4" + Environment.NewLine;
			text += "Ctrl 1 to 8: BRF to BR8" + Environment.NewLine;
			text += "9: Rendezvous, 0: Hyperspace" + Environment.NewLine;
			text += Environment.NewLine;
			text += "F1 to F4: Change current region (XWA only)" + Environment.NewLine;
			text += "Ctrl+F1 to F4: Change selected waypoints to region." + Environment.NewLine;
			MessageBox.Show(text, "Command Reference");
		}
#endregion controls

		enum Visibility { Show, Fade, Hide };
		class MapData
		{
			public MapData(Settings.Platform platform)
			{
				Craft = 0;
				FgIndex = 0;
				IFF = 0;
				Name = "";
				WPs = null;
				FullName = "";
				View = Visibility.Show;
				Difficulty = 0;
				FlightGroup = null;
				Region = 0;
				Yaw = 0;
				Pitch = 0;
				Roll = 0;
				
				switch (platform)
				{
					case Settings.Platform.XWING:
						WPs = new Platform.Xwing.FlightGroup.Waypoint[1][];
						break;
					case Settings.Platform.TIE:
						WPs = new Platform.Tie.FlightGroup.Waypoint[1][];
						break;
					case Settings.Platform.XvT:
					case Settings.Platform.BoP:
						WPs = new Platform.Xvt.FlightGroup.Waypoint[1][];
						break;
					case Settings.Platform.XWA:
						WPs = new Platform.Xwa.FlightGroup.Waypoint[19][];  // 0 is Starts, 1-16 are orders, 17 is HyperExits, 18 is HyperExitVectorTarget
						WPs[17] = new Platform.Xwa.FlightGroup.Waypoint[4];
						WPs[18] = new Platform.Xwa.FlightGroup.Waypoint[4];
						for (int i = 0; i < 4; i++)
                        {
                            WPs[17][i] = new Platform.Xwa.FlightGroup.Waypoint();
							WPs[18][i] = new Platform.Xwa.FlightGroup.Waypoint();
						}
						break;
				}
			}
			// TODO: might be able to remove some of these since FG is there?
			public int Craft;
			public int FgIndex;
			public byte IFF;
			public string Name;
			public string FullName;
			public Visibility View;
			public int Difficulty;
			public object FlightGroup;
			public int Region;
			public short Yaw;
			public short Pitch;
			public short Roll;
			public BaseFlightGroup.BaseWaypoint[][] WPs;
		}

		class SelectionData
		{
			public SelectionData(int index, MapData mapData, int wpOrder, int wpIndex)
			{
				MapDataIndex = index;
				MapDataRef = mapData;
				WPRef = mapData.WPs[wpOrder][wpIndex];
				WpOrder = wpOrder;
				WpIndex = wpIndex;
				Active = true;
			}

			/// <summary>Compares two selected objects for sorting, giving priority to visible objects over faded or hidden.</summary>
			static public int Compare(SelectionData left, SelectionData right)
			{
				if (left.MapDataRef.View < right.MapDataRef.View)
					return -1;
				else if (left.MapDataRef.View > right.MapDataRef.View)
					return 1;
				if (left.MapDataIndex < right.MapDataIndex)  // Equal visibility, sort by index in craft list.
					return -1;
				else if (left.MapDataIndex > right.MapDataIndex)
					return 1;
				return 0;
			}

			public int MapDataIndex { get; private set; }
			public MapData MapDataRef { get; private set; }
			public BaseFlightGroup.BaseWaypoint WPRef { get; private set; }
			public int WpOrder { get; private set; }
			public int WpIndex { get; private set; }
			public bool Active { get; set; }          // This operates as a quick toggle in the selection list.
			public short WpDragStartX { get; set; }   // Origin for drag-move purposes, so that snapping can work.
			public short WpDragStartY { get; set; }
			public short WpDragStartZ { get; set; }
		}
	}
}