/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2025 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.17.1
 *
 * CHANGELOG
 * v1.17.1, 250303
 * [FIX #119] Crash when skipping to the end of the briefing
 * [UPD #118] TIE text tags now use in-game font when possible
 * v1.16.0.5, 241120
 * [FIX #112] Exception adding events to blank briefing, and silently overwriting existing events.
 * v1.16.0.2, 241017
 * [FIX] Exception during XwaSetIcon and XwaMoveIcon event modifications
 * v1.16, 241013
 * [UPD] Updates due to Platform
 * [UPD] Text tag, FG tag and mask colors defined at ctor instead of inline
 * [NEW] TextTag and ShipTag structs to replace int[,] for _textTags and _fgTags
 * [UPD] _events now EventCollection type
 * v1.15.3, 231111
 * [FIX #92] Event overflow due to XWA move
 * v1.14, 230804
 * [NEW] SkipMarker for TIE/XvT
 * [UPD] Replaced Unk1 field with label
 * [FIX] Moved page increment to CaptionText instead of PageBreak
 * [FIX] Crash if a Caption/Title index was -1
 * v1.13.6, 220619
 * [NEW] Shift All checkbox on Events tab so timing can move together
 * v1.11, 210801
 * [UPD] Icons attempt to load from the platform directory to account for mods [JB]
 * v1.9.2, 210328
 * [FIX] Missing LST note in the title
 * [FIX #53] Move map for XWA
 * v1.8, 201004
 * [FIX] Timers unregister Tick to prevent calls after Dispose [JB]
 * v1.7, 200816
 * [FIX] XWA MoveIcon selecting wrong icon
 * [FIX] XvT craft icons [JB]
 * v1.6.5, 200704
 * [UPD] Icons now use BMPs instead of the DATs, importDats() renamed to importIcons()
 * [UPD] If the craft index is OutOfRange, use the first one
 * [FIX] XWA ShipInfo for X-wings work now
 * v1.6.4, 200119
 * [NEW #30] onModified callback to prevent mission from auto-dirty when opening
 * v1.5, 180910
 * [FIX] map performance [JB]
 * [UPD] XvT map size tweaked [JB]
 * [UPD] PostLoadInit tweaked [JB]
 * [UPD] _Closed and _Closing changed to _FormClosed and _FormClosing, adjust the exit routine [JB]
 * [FIX] _Load forces an update [JB]
 * [ADD] redraw timer and supporting popup functions [JB]
 * [FIX] limit to FF_Click [JB]
 * [ADD] code moved to ProcessEvent() and ResetBriefing() [JB]
 * [ADD] playback speed shown if not 1x [JB]
 * [FIX] grid paint when at extreme coords [JB]
 * [FIX] Y zoom when painting FG icons [JB]
 * [NEW] lblCaption_Click [JB]
 * [FIX] XWA tag names now use Icon # [JB]
 * [UPD] control focus in updateParameters() [JB]
 * [UPD] cmdNew now adds after current line [JB]
 * [UPD] tweaked layout size
 * v1.3, 170107
 * [NEW] Multiple briefings capability, popup [JB]
 * [NEW] BriefData.WaypointArr [JB]
 * [FIX] crash fixes [JB]
 * [NEW] support functions for event capacity and moving events [JB]
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.1.1, 120814
 * - class renamed
 * - ctors no longer ref
 * - renamed a ton of stuff
 * [NEW] txtNotes
 * v1.1, 120715
 * - using Platform
 * - removed local EventType, use BaseBriefing.EventType
 * - using BaseBriefing.EventParameterCount
 * v1.0, 110921
 * - Release
 */

using Idmr.Common;
using Idmr.LfdReader;
using Idmr.Platform;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Idmr.Yogeme
{
	/// <summary>The briefing forms for YOGEME, one form for TIE-XWA</summary>
	public partial class BriefingForm : Form
	{
		#region Vars
		BriefData[] _briefData;
		BriefData _tempBD;
		readonly Platform.Xvt.BriefingCollection _xvtBriefingCollection;
		readonly Platform.Xwa.BriefingCollection _xwaBriefingCollection;
		int _currentCollectionIndex;
		readonly Platform.Tie.Briefing _tieBriefing;
		Platform.Xvt.Briefing _xvtBriefing;
		Platform.Xwa.Briefing _xwaBriefing;
		bool _loading = false;
		readonly Color _normalColor;
		//readonly Color _highlightColor; // TODO: this is currently unused, maybe at some point work highlighting in
		readonly Color _titleColor;
		readonly BaseBriefing.EventCollection _events;   // this will contain the event listing for use, raw data is in Briefing.Events[]
		short _zoomX = 48;
		short _zoomY;
		short _mapX, _mapY; // mapX and mapY will be different, namely the grid coordinates of the center, like how TIE handles it
		Bitmap _map;
		readonly ShipTag[] _fgTags = new ShipTag[8];
		TextTag[] _textTags = new TextTag[8];
		TextTag[] _ttBackup;
		readonly Color[] _tagColors;
		readonly Color[] _iffColors;
		readonly Color[] _iffBackColors;
		readonly Color[] _maskColors;
		readonly DataTable _tableTags = new DataTable("Tags");
		readonly DataTable _tableStrings = new DataTable("Strings");
		readonly int _timerInterval;
		BaseBriefing.EventType _eventType = BaseBriefing.EventType.None;
		short _tempX, _tempY;
		readonly Settings.Platform _platform;
		string[] _tags;
		string[] _strings;
		readonly int _maxEvents;
		string _message = "";
		int _regionDelay = -1;
		int _page = 1;
		short _icon = 0;
		bool _popupPreviewActive = false;     //[JB] The popup feature allows the user to move and zoom the map without changing any events, as well as see the coordinates of the mouse cursor.  Designed to assist raw editing of the event list.
		Point _popupPreviewZoom;
		Point _popupPreviewMap;
		bool _isMiddleDrag = false;
		Point _popupMiddle;
		bool _mapPaintScheduled = false;

		int _previousTimeIndex = 0;           //Tracks the previous time index of the briefing so we can detect when the user is manually scrolling through arbitrary times.
		static int[] _xvtTagSizeCache = null;
#pragma warning disable IDE1006 // Naming Styles
		EventHandler onModified = null;
#pragma warning restore IDE1006 // Naming Styles
		readonly LfdFile _empire;
		#endregion

		static public string[] SharedTeamNames = new string[10];

		public BriefingForm(Platform.Tie.FlightGroupCollection fg, Platform.Tie.Briefing briefing, EventHandler onModifiedCallback)
		{
			_loading = true;
			_platform = Settings.Platform.TIE;
			_titleColor = Color.FromArgb(0xFC, 0xFC, 0x54);
			_normalColor = Color.FromArgb(0xFC, 0xFC, 0xFC);
			_tagColors = new Color[] {
				Color.FromArgb(0, 0xAC, 0),			// green
				Color.FromArgb(0xAC, 0, 0),			// red
				Color.FromArgb(0xAC, 0, 0xAC),		// purple
				Color.FromArgb(0, 0x2C, 0xAC),		// blue
				Color.FromArgb(0xA8, 0, 0),			// red2
				Color.FromArgb(0xFC, 0x54, 0x54),	// light red
				Color.FromArgb(0x44, 0x44, 0x44),	// gray
				Color.FromArgb(0xCC, 0xCC, 0xCC)	// white
			};
			_iffColors = new Color[] {
				Color.FromArgb(0, 0xE0, 0),		// green
				Color.FromArgb(0xE0, 0, 0),		// red
				Color.FromArgb(0, 0x78, 0xE0),	// blue
				Color.FromArgb(0xE0, 0, 0xE0),	// purple
				Color.FromArgb(0xE0, 0, 0),		// red2
				Color.FromArgb(0xE0, 0, 0xE0)	// purple2
			};
			_iffBackColors = new Color[] {
				Color.FromArgb(0, 0x78, 0),		// green
				Color.FromArgb(0x78, 0, 0),		// red
				Color.FromArgb(0, 0x10, 0x78),	// blue
				Color.FromArgb(0x78, 0, 0x78),	// purple
				Color.FromArgb(0x78, 0, 0),		// red2
				Color.FromArgb(0x78, 0, 0x78)	// purple2
			};
			//_highlightColor = Color.FromArgb(0x00, 0xA8, 0x00);
			_zoomY = _zoomX;            // in most cases, these will remain the same
			_tieBriefing = briefing;
			_maxEvents = Platform.Tie.Briefing.EventQuantityLimit;
			_events = new BaseBriefing.EventCollection(MissionFile.Platform.TIE);
			InitializeComponent();
			Text = "YOGEME Briefing Editor - TIE";
			try
			{
				_empire = new LfdFile(Settings.GetInstance().TiePath + "\\RESOURCE\\EMPIRE.LFD");
			}
			catch
			{
				_empire = null;
			}
			#region layout edit
			// final layout update, as in VS it's spread out
			Height = 426;
			Width = 760;
			tabBrief.Width = 752;
			cmdMarker.Top = 160;
			Point loc = new Point(608, 188);
			pnlShipTag.Location = loc;
			pnlTextTag.Location = loc;
			#endregion
			Import(fg); // FGs are separate so they can be updated without running the BRF as well
			importIcons(Application.StartupPath + "\\images\\TIE_BRF.bmp", 34);
			_tags = _tieBriefing.BriefingTag;
			_strings = _tieBriefing.BriefingString;
			importStrings();
			_timerInterval = Platform.Tie.Briefing.TicksPerSecond;
			txtLength.Text = Convert.ToString(Math.Round((decimal)_tieBriefing.Length / _timerInterval, 2));
			hsbTimer.Maximum = _tieBriefing.Length + 11;
			_mapX = 0;
			_mapY = 0;
			lstEvents.Items.Clear();
			importEvents(_tieBriefing.Events);
			hsbTimer.Value = 0;
			numTile.Value = _tieBriefing.Tile;
			cboText.SelectedIndex = 0;
			cboFGTag.SelectedIndex = 0;
			cboTextTag.SelectedIndex = 0;
			cboColorTag.SelectedIndex = 0;

			labBriefIndex2.Visible = false;
			cboBriefIndex2.Visible = false;
			tabTeams.Enabled = false;

			_loading = false;
			onModified = onModifiedCallback;
			postLoadInit();
		}
		public BriefingForm(Platform.Xvt.FlightGroupCollection fg, Platform.Xvt.BriefingCollection briefing, EventHandler onModifiedCallback)
		{
			_loading = true;
			_platform = Settings.Platform.XvT;
			_titleColor = Color.FromArgb(0xFC, 0xFC, 0x00);
			_normalColor = Color.FromArgb(0xF8, 0xFC, 0xF8);
			_tagColors = new Color[] {
				Color.FromArgb(0, 0xAC, 0),	// green
				Color.FromArgb(0xA8, 0, 0),	// red
				Color.FromArgb(0xA8, 0xAC, 0),	// yellow
				Color.FromArgb(0, 0x2C, 0xA8),	// blue
				Color.FromArgb(0xA8, 0, 0xA8),	// purple
				Color.Black
			};
			_iffColors = new Color[] {
				Color.FromArgb(0, 0xE0, 0),		// green
				Color.FromArgb(0xE0, 0, 0),		// red
				Color.FromArgb(0, 0, 0xE0),		// blue
				Color.FromArgb(0xE0, 0xE0, 0),	// yellow
				Color.FromArgb(0xE0, 0, 0),		// red2
				Color.FromArgb(0xE0, 0, 0xE0)	// purple2
			};
			_iffBackColors = new Color[] {
				Color.FromArgb(0, 0x78, 0),		// green
				Color.FromArgb(0x78, 0, 0),		// red
				Color.FromArgb(0, 0, 0x78),		// blue
				Color.FromArgb(0x78, 0x78, 0),	// yellow
				Color.FromArgb(0x78, 0, 0),		// red2
				Color.FromArgb(0x78, 0, 0x78)	// purple2
			};
			_maskColors = new Color[] {
				// green
				Color.FromArgb(0x38, 0xD4, 0),
				Color.FromArgb(0x18, 0xA8, 0),
				Color.FromArgb(8, 0x7C, 0),
				Color.FromArgb(0, 0x54, 0),
				Color.Black,
				Color.FromArgb(0, 1, 0),
				// red
				Color.FromArgb(0xF8, 0x24, 0),
				Color.FromArgb(0xC0, 0x10, 0),
				Color.FromArgb(0x80, 4, 0),
				Color.FromArgb(0x48, 0, 0),
				Color.Black,
				Color.FromArgb(1, 0, 0),
				// blue
				Color.FromArgb(0x58, 0xDC, 0xF8),
				Color.FromArgb(0x28, 0x84, 0xC0),
				Color.FromArgb(8, 0x3C, 0x90),
				Color.FromArgb(0, 8, 0x58),
				Color.Black,
				Color.FromArgb(0, 0, 1),
				// yellow
				Color.FromArgb(0xD8, 0xFC, 0),
				Color.FromArgb(0xD0, 0xCC, 0),
				Color.FromArgb(0xA8, 0x9C, 0),
				Color.FromArgb(0x80, 0x74, 0),
				Color.Black,
				Color.FromArgb(1, 1, 0),
				// red
				Color.FromArgb(0xF8, 0x24, 0),
				Color.FromArgb(0xC0, 0x10, 0),
				Color.FromArgb(0x80, 4, 0),
				Color.FromArgb(0x48, 0, 0),
				Color.Black,
				Color.FromArgb(1, 0, 0),
				// purple
				Color.FromArgb(0x90, 0x88, 0xF0),
				Color.FromArgb(0x70, 0x5C, 0xB0),
				Color.FromArgb(0x50, 0x30, 0x78),
				Color.FromArgb(0x30, 8, 0x40),
				Color.Black,
				Color.FromArgb(1, 0, 1)
			};
			//_highlightColor = Color.FromArgb(0x40, 0xC4, 0x40);
			_zoomY = _zoomX;
			_xvtBriefingCollection = briefing;
			_currentCollectionIndex = 0;
			_xvtBriefing = briefing[0];
			_maxEvents = Platform.Xvt.Briefing.EventQuantityLimit;
			_events = new BaseBriefing.EventCollection(MissionFile.Platform.XvT);
			InitializeComponent();
			Text = "YOGEME Briefing Editor - XvT/BoP";
			Import(fg);
			#region XvT layout change
			Height = 426;
			Width = 760;
			tabBrief.Width = 752;
			Point loc = new Point(608, 188);
			pnlShipTag.Location = loc;
			pnlTextTag.Location = loc;
			pctBrief.Size = new Size(360, 208); //[JB] //Was 214.  The actual size in game appears to be 320x210, but I trimmed it down to 208 because it seemed to be rendering some extra pixels.
			pctBrief.Left = 150;
			lblCaption.BackColor = Color.FromArgb(0, 0, 0x78);
			lblCaption.Font = new System.Drawing.Font("Times New Roman", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblCaption.Size = new Size(360, 28);
			lblCaption.Location = new Point(150, 254);
			lblTitle.BackColor = Color.FromArgb(0x10, 0x10, 0x20);
			lblTitle.Font = new System.Drawing.Font("Times New Roman", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblTitle.Size = new Size(360, 16);
			lblTitle.TextAlign = ContentAlignment.TopCenter;
			lblTitle.ForeColor = _titleColor;
			lblTitle.Text = "*Defined in .LST file*";
			lblTitle.Location = new Point(150, 24);
			cmdTitle.Enabled = false;
			cboColorTag.Items.Clear();
			cboColorTag.Items.Add("Green");
			cboColorTag.Items.Add("Red");
			cboColorTag.Items.Add("Yellow");
			cboColorTag.Items.Add("Blue");
			cboColorTag.Items.Add("Purple");
			cboColorTag.Items.Add("Black");
			cboColor.Items.Clear();
			cboColor.Items.Add("Green");
			cboColor.Items.Add("Red");
			cboColor.Items.Add("Yellow");
			cboColor.Items.Add("Blue");
			cboColor.Items.Add("Purple");
			cboColor.Items.Add("Black");
			cmdMarker.Top = 160;
			#endregion
			importIcons(Application.StartupPath + "\\images\\XvT_BRF.bmp", 22);
			_tags = _xvtBriefing.BriefingTag;
			_strings = _xvtBriefing.BriefingString;
			importStrings();
			_timerInterval = Platform.Xvt.Briefing.TicksPerSecond;
			txtLength.Text = Convert.ToString(Math.Round((decimal)_xvtBriefing.Length / _timerInterval, 2));
			hsbTimer.Maximum = _xvtBriefing.Length + 11;
			_mapX = 0;
			_mapY = 0;
			lstEvents.Items.Clear();
			importEvents(_xvtBriefing.Events);
			hsbTimer.Value = 0;
			numTile.Value = _xvtBriefing.Tile;
			cboText.SelectedIndex = 0;
			cboFGTag.SelectedIndex = 0;
			cboTextTag.SelectedIndex = 0;
			cboColorTag.SelectedIndex = 0;

			for (int i = 0; i < _xvtBriefingCollection.Count; i++)
			{
				string s = "Briefing " + (i + 1);
				cboBriefIndex1.Items.Add(s);
				cboBriefIndex2.Items.Add(s);
			}
			for (int i = 0; i < 10; i++)  //XvT has 10 teams
				lstTeams.Items.Add((i + 1) + ": " + SharedTeamNames[i]);
			cboBriefIndex1.SelectedIndex = 0;
			cboBriefIndex2.SelectedIndex = 0;
			refreshTeamList();
			updateTitle();

			_loading = false;
			onModified = onModifiedCallback;
			postLoadInit();
		}
		public BriefingForm(Platform.Xwa.BriefingCollection briefing, EventHandler onModifiedCallback)
		{
			_loading = true;
			_platform = Settings.Platform.XWA;
			_titleColor = Color.FromArgb(0x63, 0x82, 0xFF);
			_normalColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
			_tagColors = new Color[] {
				Color.FromArgb(0, 0xE3, 0),	// green
				Color.FromArgb(0xE7, 0, 0),	// red
				Color.FromArgb(0xE7, 0xE3, 0),	// yellow
				Color.FromArgb(0x63, 0x61, 0xE7),	// purple
				Color.FromArgb(0xDE, 0, 0xDE),	// pink
				Color.FromArgb(0, 4, 0xA5)	// blue
			};
			_iffColors = new Color[] {
				Color.FromArgb(0, 0xE0, 0),		// green
				Color.FromArgb(0xE0, 0, 0),		// red
				Color.FromArgb(0x60, 0x60, 0xE0),	// blue
				Color.FromArgb(0xE0, 0xE0, 0),	// yellow
				Color.FromArgb(0xE0, 0, 0),		// red2
				Color.FromArgb(0xE0, 0, 0xE0)	// purple
			};
			_iffBackColors = new Color[] {
				Color.FromArgb(0, 0x78, 0),		// green
				Color.FromArgb(0x78, 0, 0),		// red
				Color.FromArgb(0x20, 0x20, 0x78),	// blue
				Color.FromArgb(0x78, 0x78, 0),	// yellow
				Color.FromArgb(0x78, 0, 0),		// red2
				Color.FromArgb(0x78, 0, 0x78)	// purple
			};
			_maskColors = new Color[] {
				Color.FromArgb(0x40, 0xBC, 0x20),	// green
				Color.FromArgb(0xF8, 0x54, 0x50),	// red
				Color.FromArgb(0x68, 0x8C, 0xF8),	// blue
				Color.FromArgb(0xE8, 0xD0, 0x40),	// yellow
				Color.FromArgb(0xF8, 0x54, 0x50),	// red2
				Color.FromArgb(0xF8, 0x80, 0xF8)	// purple
			};
			//_highlightColor = _titleColor;
			_zoomX = 32;
			_zoomY = _zoomX;
			_xwaBriefingCollection = briefing;
			_currentCollectionIndex = 0;
			_xwaBriefing = briefing[0];
			_maxEvents = Platform.Xwa.Briefing.EventQuantityLimit;
			_events = new BaseBriefing.EventCollection(MissionFile.Platform.XWA);
			InitializeComponent();
			Text = "YOGEME Briefing Editor - XWA";
			#region XWA layout change
			// TODO: view is off by a little bit, couple pixels
			label7.Text = "Icon:";
			Height = 484;
			Width = 760;
			tabBrief.Width = 752;
			Point loc = new Point(608, 246);
			pnlShipTag.Location = loc;
			pnlTextTag.Location = loc;
			pnlShipInfo.Location = loc;
			pnlRotate.Location = loc;
			pnlMove.Location = loc;
			pnlNew.Location = loc;
			pnlRegion.Location = loc;
			cmdNewShip.Visible = true;
			cmdMoveShip.Visible = true;
			cmdRotate.Visible = true;
			cmdShipInfo.Visible = true;
			cmdRegion.Visible = true;
			pctBrief.Size = new Size(510, 294);
			pctBrief.Left += 36;
			lblTitle.BackColor = Color.FromArgb(0x18, 0x18, 0x18);
			lblTitle.Size = new Size(510, 28);
			lblTitle.Left += 36;
			lblTitle.Top -= 4;
			lblTitle.TextAlign = ContentAlignment.TopCenter;
			lblTitle.ForeColor = _titleColor;
			lblTitle.Text = "*Defined in .LST file*";
			lblTitle.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
			cmdTitle.Enabled = false;
			cmdMarker.Visible = false;
			lblCaption.BackColor = Color.FromArgb(0x20, 0x30, 0x88);
			lblCaption.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
			lblCaption.Size = new Size(510, 40);
			lblCaption.Top += 68;
			lblCaption.Left += 36;
			vsbBRF.Left -= 38;
			vsbBRF.Height = 294;
			tabBrief.Height += 58;
			hsbBRF.Top += 70;
			hsbBRF.Width = 510;
			hsbBRF.Left += 36;
			lblInstruction.Top += 58;
			pnlBottomRight.Top += 58;
			pnlBottomLeft.Top += 58;
			dataT.Height += 58;
			dataS.Height += 58;
			lstEvents.Height += 58;
			cboColorTag.Items.Clear();
			cboColorTag.Items.Add("Green");
			cboColorTag.Items.Add("Red");
			cboColorTag.Items.Add("Yellow");
			cboColorTag.Items.Add("Purple");
			cboColorTag.Items.Add("Pink");
			cboColorTag.Items.Add("Blue");
			cboColor.Items.Clear();
			cboColor.Items.Add("Green");
			cboColor.Items.Add("Red");
			cboColor.Items.Add("Yellow");
			cboColor.Items.Add("Purple");
			cboColor.Items.Add("Pink");
			cboColor.Items.Add("Blue");
			cboEvent.Items.RemoveAt(0);	// Remove the "Skip Marker" event
			cboEvent.Items.Add("New Icon");
			cboEvent.Items.Add("Show Ship Data");
			cboEvent.Items.Add("Move Icon");
			cboEvent.Items.Add("Rotate Icon");
			cboEvent.Items.Add("Switch to Region");
			cboCraft.Items.AddRange(Platform.Xwa.Strings.CraftType);
			cboNCraft.Items.AddRange(Platform.Xwa.Strings.CraftType);
			#endregion
			// Try loading directly from the installation.  If it fails, load the default image strip.
			if (!loadXwaIcons(56)) importIcons(Application.StartupPath + "\\images\\XWA_BRF.bmp", 56);
			_tags = _xwaBriefing.BriefingTag;
			_strings = _xwaBriefing.BriefingString;
			importStrings();
			_timerInterval = Platform.Xwa.Briefing.TicksPerSecond;
			txtLength.Text = Convert.ToString(Math.Round((decimal)_xwaBriefing.Length / _timerInterval, 2));
			hsbTimer.Maximum = _xwaBriefing.Length + 11;
			_mapX = 0;
			_mapY = 0;
			lstEvents.Items.Clear();
			_briefData = new BriefData[100];    // this way I don't have to deal with expanding the array
			string[] names = new string[100];
			for (int i = 0; i < _briefData.Length; i++) names[i] = "Icon #" + i;
			cboFG.Items.AddRange(names);
			cboFGTag.Items.AddRange(names);
			cboInfoCraft.Items.AddRange(names);
			cboRCraft.Items.AddRange(names);
			cboMoveIcon.Items.AddRange(names);
			cboNewIcon.Items.AddRange(names);
			importEvents(_xwaBriefing.Events);
			hsbTimer.Value = 0;
			numTile.Value = _xwaBriefing.Tile;
			txtNotes.Enabled = true;
			txtNotes.Text = _xwaBriefing.BriefingStringsNotes[0];
			cboText.SelectedIndex = 0;
			cboFGTag.SelectedIndex = 0;
			cboTextTag.SelectedIndex = 0;
			cboColorTag.SelectedIndex = 0;
			cboInfoCraft.SelectedIndex = 0;
			cboRCraft.SelectedIndex = 0;
			cboRotateAmount.SelectedIndex = 0;
			cboMoveIcon.SelectedIndex = 0;
			cboNewIcon.SelectedIndex = 0;
			cboNCraft.SelectedIndex = 0;
			cboIconIff.SelectedIndex = 0;

			for (int i = 0; i < _xwaBriefingCollection.Count; i++)
			{
				string s = "Briefing " + (i + 1);
				cboBriefIndex1.Items.Add(s);
				cboBriefIndex2.Items.Add(s);
			}
			for (int i = 0; i < 10; i++)  //XWA has 10 teams
				lstTeams.Items.Add((i + 1) + ": " + SharedTeamNames[i]);
			cboBriefIndex1.SelectedIndex = 0;
			cboBriefIndex2.SelectedIndex = 0;
			refreshTeamList();
			updateTitle();

			_loading = false;
			onModified = onModifiedCallback;
			postLoadInit();
		}

		/// <summary>Handles redundant code for each of the 3 platform modes.</summary>
		void postLoadInit()
		{
			if (lstEvents.Items.Count > 0) lstEvents.SelectedIndex = 0;
			else
			{
				cmdUp.Enabled = false;
				cmdDown.Enabled = false;
			}
		}

		void fillBriefData(int index, int craftType, BaseFlightGroup.Waypoint waypoint, BaseFlightGroup.Waypoint[] waypoints, byte iff, string name)
		{
			_briefData[index].Craft = craftType;
			_briefData[index].Waypoint = (short[])waypoint;
			_briefData[index].WaypointArr = waypoints;
			_briefData[index].IFF = iff;
			_briefData[index].Name = name;
			cboFG.Items.Add(name);
			cboFGTag.Items.Add(name);
		}

		/// <summary>Attempts to load XWA's briefing icons directly from the installation files.</summary>
		bool loadXwaIcons(int size)
		{
			try
			{
				System.Collections.Generic.List<string> shiplist = new System.Collections.Generic.List<string>(232);
				string line;
				using (StreamReader sr = new StreamReader(CraftDataManager.GetInstance().GetInstallPath() + "\\SHIPLIST.TXT"))
				{
					while (!sr.EndOfStream)
					{
						line = sr.ReadLine();
						if (line.StartsWith("!")) shiplist.Add(line);
					}
				}
				Image bmp = Image.FromFile(CraftDataManager.GetInstance().GetInstallPath() + "\\FRONTRES\\MAPICONS\\LICON.BMP");
				imgCraft.ImageSize = new Size(size, size);
				for (int i = 0; i < shiplist.Count; i++)
				{
					Bitmap icon = new Bitmap(size, size);
					using (Graphics g = Graphics.FromImage(icon))
					{
						string[] tokens = shiplist[i].Split(',');
						int x1 = 0, x2 = 0, y1 = 0, y2 = 0, width = 0, height = 0;
						if (tokens.Length >= 13) // Extraneous commas may exist.
						{
							int.TryParse(tokens[9].Trim(), out x1);
							int.TryParse(tokens[10].Trim(), out y1);
							int.TryParse(tokens[11].Trim(), out x2);
							int.TryParse(tokens[12].Trim(), out y2);
							width = x2 - x1;
							height = y2 - y1;
						}
						Rectangle src = new Rectangle(x1, y1, width, height);
						if (width > size) width = size;
						if (height > size) height = size;
						Rectangle dest = new Rectangle((size / 2) - (width / 2), (size / 2) - (height / 2), width, height);
						g.DrawImage(bmp, dest, src, GraphicsUnit.Pixel);
					}
					imgCraft.Images.Add(icon);
				}
			}
			catch { return false; }
			return true;
		}
		void importIcons(string filename, int size)
		{
			try
			{
				imgCraft.ImageSize = new Size(size, size);
				imgCraft.Images.AddStrip(Image.FromFile(filename));
			}
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
		}
		void importEvents(BaseBriefing.EventCollection rawEvents)
		{
			for (int i = 0; i < _maxEvents; i++)
			{
				try
				{
					_events.Add(rawEvents[i].Clone());
					if (_events[i].IsEndEvent) break;

					if (_platform == Settings.Platform.XWA && _events[i].Type == BaseBriefing.EventType.XwaMoveIcon && _briefData[_events[i].Variables[0]].Waypoint != null && _briefData[_events[i].Variables[0]].Waypoint[0] == 0 && _briefData[_events[i].Variables[0]].Waypoint[1] == 0)
					{   // this prevents Exception if Move instruction is before NewIcon, and only assigns initial position
						_briefData[_events[i].Variables[0]].Waypoint[0] = _events[i].Variables[1];
						_briefData[_events[i].Variables[0]].Waypoint[1] = _events[i].Variables[2];
					}
				}
				catch (ArgumentOutOfRangeException) { break; }	// if briefing is corrupted leading to an overflow, just kick out
				lstEvents.Items.Add("");
				updateList(i);
			}
		}
		void importStrings()
		{
			if (_tableTags.Columns.Count == 0)
			{
				_tableTags.Columns.Add("tag");
				_tableStrings.Columns.Add("string");
			}
			_tableTags.Clear();
			_tableStrings.Clear();
			for (int i = 0; i < _tags.Length; i++)
			{
				DataRow dr = _tableTags.NewRow();
				dr[0] = _tags[i];
				_tableTags.Rows.Add(dr);
				dr = _tableStrings.NewRow();
				dr[0] = _strings[i];
				_tableStrings.Rows.Add(dr);
			}
			dataTags.Table = _tableTags;
			dataStrings.Table = _tableStrings;
			dataT.DataSource = dataTags;
			dataS.DataSource = dataStrings;
			_tableTags.RowChanged += new DataRowChangeEventHandler(tableTags_RowChanged);
			_tableStrings.RowChanged += new DataRowChangeEventHandler(tableStrings_RowChanged);
			loadTags();
			loadStrings();
		}

		public void Import(Platform.Tie.FlightGroupCollection fg)
		{
			_briefData = new BriefData[fg.Count];
			cboFG.Items.Clear();
			cboFGTag.Items.Clear();
			for (int i = 0; i < fg.Count; i++) fillBriefData(i, fg[i].CraftType, fg[i].Waypoints[14], null, fg[i].IFF, fg[i].Name);
		}
		public void Import(Platform.Xvt.FlightGroupCollection fg)
		{
			_briefData = new BriefData[fg.Count];
			cboFG.Items.Clear();
			cboFGTag.Items.Clear();
			for (int i = 0; i < fg.Count; i++)
			{
				var wps = new BaseFlightGroup.Waypoint[8];
				for (int k = 0; k < 8; k++) wps[k] = fg[i].Waypoints[14 + k];
				fillBriefData(i, fg[i].CraftType, fg[i].Waypoints[14], wps, fg[i].IFF, fg[i].Name);
			}
		}
		public void Save()
		{
			_baseBrf.Events.Clear();
			for (int evnt = 0; evnt < _maxEvents; evnt++)
			{
				if (_events[evnt].IsEndEvent) break;

				_baseBrf.Events.Add(_events[evnt].Clone());
			}
			_baseBrf.Tile = (short)numTile.Value;
			onModified?.Invoke("Save", new EventArgs());
		}

		void tabBrief_SelectedIndexChanged(object sender, EventArgs e) => hsbTimer.Value = (tabBrief.SelectedIndex != 0 ? 1 : 0);  // force refresh, since pct doesn't want to update when hidden

		#region frmBrief
		void frmBrief_Activated(object sender, EventArgs e) => MapPaint();
		void frmBrief_FormClosed(object sender, FormClosedEventArgs e)
		{
			tabBrief.Focus();
			_map.Dispose();
		}
		void frmBrief_FormClosing(object sender, FormClosingEventArgs e)
		{
			Save();
			//Important! There's an issue where the event can trigger after the map is disposed, even after calling Stop(). The event must be unregistered.
			tmrPopup.Stop();
			tmrPopup.Tick -= tmrPopup_Tick;
			tmrMapRedraw.Stop();
			tmrMapRedraw.Tick -= tmrMapRedraw_Tick;
			onModified = null;
		}
		void frmBrief_Load(object sender, EventArgs e)
		{
			for (int i = 0; i < 8; i++) _fgTags[i].Slot = -1;
			for (int i = 0; i < 8; i++) _textTags[i].StringIndex = -1;
			_map = new Bitmap(pctBrief.Width, pctBrief.Height, PixelFormat.Format24bppRgb);
			hsbTimer.Value = 1;
			hsbTimer.Value = 0;
		}
		#endregion	frmBrief
		#region tabDisplay
		#region Timer related
		void startTimer()
		{
			cmdPlay.Enabled = false;
			cmdPlay.Visible = false;
			cmdPause.Enabled = true;
			cmdPause.Visible = true;
			cmdPause.Focus();
			tmrBrief.Start();
		}
		void stopTimer()
		{
			tmrBrief.Stop();
			cmdPause.Enabled = false;
			cmdPause.Visible = false;
			cmdPlay.Enabled = true;
			cmdPlay.Visible = true;
			cmdPlay.Focus();
			tmrBrief.Interval = 1000 / _timerInterval;
		}

		void cmdFF_Click(object sender, EventArgs e)
		{
			if (hsbTimer.Value == hsbTimer.Maximum - 11) return;

			int newSpeed = tmrBrief.Interval / 2;
			if (newSpeed < 125 / _timerInterval) newSpeed = 125 / _timerInterval;    //Limit to 8x speed.
			tmrBrief.Interval = newSpeed;
			startTimer();
		}
		void cmdNext_Click(object sender, EventArgs e)
		{
			int i;
			for (i = 0; i < _events.Count; i++)
				if (_events[i].Time > hsbTimer.Value && (_events[i].Type == BaseBriefing.EventType.SkipMarker || _events[i].Type == BaseBriefing.EventType.PageBreak)) break;
			if (i == _maxEvents || i == _events.Count) hsbTimer.Value = hsbTimer.Maximum - 11;    // tmr_Tick takes care of halting
			else hsbTimer.Value = _events[i].Time;
		}
		void cmdPause_Click(object sender, EventArgs e) => stopTimer();
		void cmdPlay_Click(object sender, EventArgs e)
		{
			if (hsbTimer.Value == hsbTimer.Maximum - 11) return;

			tmrBrief.Interval = 1000 / _timerInterval;
			startTimer();
		}
		void cmdStart_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 8; i++)
			{
				_fgTags[i].Slot = -1;
				_fgTags[i].StartTime = 0;
				_textTags[i].StringIndex = -1;
				_textTags[i].X = 0;
				_textTags[i].Y = 0;
				_textTags[i].ColorIndex = 0;
			}
			_previousTimeIndex = 0;
			hsbTimer.Value = 0;
		}
		void cmdStop_Click(object sender, EventArgs e)
		{
			stopTimer();
			for (int i = 0; i < 8; i++)
			{
				_fgTags[i].Slot = -1;
				_fgTags[i].StartTime = 0;
				_textTags[i].StringIndex = -1;
				_textTags[i].X = 0;
				_textTags[i].Y = 0;
				_textTags[i].ColorIndex = 0;
			}
			hsbTimer.Value = 0;
		}

		void hsbTimer_ValueChanged(object sender, EventArgs e)
		{
			bool paint = false;
			if (hsbTimer.Value != 0 && ((hsbTimer.Value - _previousTimeIndex >= 2) || hsbTimer.Value <= _previousTimeIndex))
			{
				// A non-incremental or reverse change (if incremental the timer should be +1 to previous), the user most likely manually moved the scrollbar.
				// Iterate through all past events and rebuild the briefing state.
				resetBriefing();
				stopTimer();
				for (int i = 0; i < _maxEvents; i++)
				{
					if (_events[i].Time > hsbTimer.Value || _events[i].IsEndEvent) break;
					paint |= processEvent(i, true);
				}
			}

			_previousTimeIndex = hsbTimer.Value;
			if (hsbTimer.Value == 0) resetBriefing();
			
			if (_regionDelay != -1)
			{
				_message = "";
				_regionDelay = -1;
				lblCaption.Visible = true;
				lblTitle.Visible = true;
			}
			for (int i = 0; i < _maxEvents; i++)
			{
				if (_events[i].Time < hsbTimer.Value) continue;

				if (_events[i].Time > hsbTimer.Value || _events[i].IsEndEvent) break;
				paint |= processEvent(i, false);
			}
			for (int h = 0; h < 8; h++) if (hsbTimer.Value - _fgTags[h].StartTime < 13) paint = true;
			lblTime.Text = string.Format("{0:Time: 0.00}", (decimal)hsbTimer.Value / _timerInterval);
			if (hsbTimer.Value == (hsbTimer.Maximum - 11) || hsbTimer.Value == 0) stopTimer();
			if (paint) MapPaint();
			if (tmrBrief.Interval != (1000 / _timerInterval)) lblTime.Text += "  (" + (1000 / _timerInterval) / tmrBrief.Interval + "x)";
		}

		void tmrBrief_Tick(object sender, EventArgs e)
		{
			if (_regionDelay == -1) hsbTimer.Value++;
			else if (_regionDelay == 0)
			{
				_message = "";
				_regionDelay--;
				lblCaption.Visible = true;
				lblTitle.Visible = true;
			}
			else _regionDelay--;
		}
		void tmrPopup_Tick(object sender, EventArgs e)
		{
			if (_popupPreviewActive) return;

			tmrPopup.Stop();
			lblPopupInfo.Visible = false;
			lblPopupInfo.Text = "";
		}
		void tmrMapRedraw_Tick(object sender, EventArgs e)
		{
			if (_mapPaintScheduled)
			{
				if (_platform == Settings.Platform.TIE) tiePaint();
				else if (_platform == Settings.Platform.XvT) xvtPaint();
				else if (_platform == Settings.Platform.XWA) xwaPaint();

				_mapPaintScheduled = false;
			}
			tmrMapRedraw.Stop();
		}
		#endregion	Timer related

		public void MapPaint()
		{
			if (_mapPaintScheduled) return;
			
			if (!tmrMapRedraw.Enabled) tmrMapRedraw.Start();
			_mapPaintScheduled = true;
		}

		void displayString(Graphics g, string fontID, string text, short left, short top, Color color)
		{
			// not doing shadows, leaving it loose for font8 or font6
			LfdReader.Font font = (LfdReader.Font)_empire.Resources[fontID];
			font.SetColor(color);
			char[] chars = text.ToCharArray();
			int offset = left;
			byte index;
			bool badChar = false;
			var oldMode = g.InterpolationMode;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			for (int i = 0; i < chars.Length; i++)
			{
				index = Convert.ToByte(chars[i] - font.StartingChar);
				if (index >= font.NumberOfGlyphs)
				{
					index = Convert.ToByte('X' - font.StartingChar);
					badChar = true;
					font.SetColor(Color.Red);
				}
				var glyph = font.Glyphs[index];
				g.DrawImage(glyph, offset, top, glyph.Width * 2, glyph.Height * 2);
				offset += (glyph.Width + 1) * 2;
				if (badChar)
				{
					badChar = false;
					font.SetColor(color);
				}
			}
			g.InterpolationMode = oldMode;
		}
		void drawGrid(int x, int y, Graphics g)
		{
			Pen pn = new Pen(Color.FromArgb(0x50, 0, 0)) { Width = 1 };
			if (_platform == Settings.Platform.TIE)
			{
				pn.Color = Color.FromArgb(0x48, 0, 0);
				pn.Width = 2;
			}
			int mod = (_platform == Settings.Platform.TIE ? 2 : 1);

			int w = pctBrief.Width;
			int h = pctBrief.Height;
			//Calculate where the viewport is, then find the longest span to know how many lines to iterate through.
			int x1 = (x - w) / (_zoomX * mod);
			int y1 = (y - h) / (_zoomX * mod);
			int x2 = (x + w) / (_zoomX * mod);
			int y2 = (y + h) / (_zoomX * mod);
			int min = x1, max = x2;
			if (y1 < min) min = y1;
			if (y2 > max) max = y2;

			if (_zoomX >= 32)
			{
				for (int i = min; i < max; i++)
				{
					if (i % 4 == 0) continue; // don't draw where there'll be maj lines

					g.DrawLine(pn, 0, _zoomY * i * mod + y - 1, w, _zoomY * i * mod + y - 1);   //min lines, every zoom pixels
					g.DrawLine(pn, 0, y - 1 - _zoomY * i * mod, w, y - 1 - _zoomY * i * mod);
					g.DrawLine(pn, _zoomX * i * mod + x, 0, _zoomX * i * mod + x, h);
					g.DrawLine(pn, x - _zoomX * i * mod, 0, x - _zoomX * i * mod, h);
				}
			}
			else if (_zoomX >= 16)
			{
				for (int i = min; i < max; i++)
				{
					if (i % 2 == 0) continue;

					g.DrawLine(pn, 0, _zoomY * 2 * i * mod + y - 1, w, _zoomY * 2 * i * mod + y - 1);   //min lines, every zoomx2 pixels
					g.DrawLine(pn, 0, y - 1 - _zoomY * 2 * i * mod, w, y - 1 - _zoomY * 2 * i * mod);
					g.DrawLine(pn, _zoomX * 2 * i * mod + x, 0, _zoomX * 2 * i * mod + x, h);
					g.DrawLine(pn, x - _zoomX * 2 * i * mod, 0, x - _zoomX * 2 * i * mod, h);
				}
			}
			// else if (zoom < 16) just don't draw them
			pn.Color = Color.FromArgb(0x90, 0, 0);
			if (_platform == Settings.Platform.TIE) pn.Color = Color.FromArgb(0x78, 0, 0);
			g.DrawLine(pn, 0, y - 1, w, y - 1); // origin lines
			g.DrawLine(pn, x, 0, x, h);
			for (int i = 0; i < 36; i++)
			{
				g.DrawLine(pn, 0, _zoomY * 4 * i * mod + y - 1, w, _zoomY * 4 * i * mod + y - 1);   //maj lines, every zoomx4 pixels
				g.DrawLine(pn, 0, y - 1 - _zoomY * 4 * i * mod, w, y - 1 - _zoomY * 4 * i * mod);
				g.DrawLine(pn, _zoomX * 4 * i * mod + x, 0, _zoomX * 4 * i * mod + x, h);
				g.DrawLine(pn, x - _zoomX * 4 * i * mod, 0, x - _zoomX * 4 * i * mod, h);
			}
		}
		void enableOkCancel(bool state)
		{
			cmdOk.Enabled = state;
			cmdCancel.Enabled = state;
			cmdClear.Enabled = !state;
			if (_platform == Settings.Platform.TIE) cmdTitle.Enabled = !state;
			cmdCaption.Enabled = !state;
			cmdFG.Enabled = !state;
			cmdText.Enabled = !state;
			cmdZoom.Enabled = !state;
			cmdMove.Enabled = !state;
			cmdBreak.Enabled = !state;
			if (!state)
			{
				pnlShipInfo.Visible = false;
				pnlShipTag.Visible = false;
				pnlTextTag.Visible = false;
				pnlRotate.Visible = false;
				pnlMove.Visible = false;
				pnlNew.Visible = false;
				pnlRegion.Visible = false;
			}
			if (_platform == Settings.Platform.XWA)
			{
				cmdMoveShip.Enabled = !state;
				cmdNewShip.Enabled = !state;
				cmdRotate.Enabled = !state;
				cmdShipInfo.Enabled = !state;
				cmdRegion.Enabled = !state;
			}
		}
		int findExisting(BaseBriefing.EventType eventType)
		{
			int i;
			for (i = 0; i < _maxEvents; i++)
			{
				if (_events[i].Time < hsbTimer.Value) continue;

				if (_events[i].Time > hsbTimer.Value) return (i + 10000); // did not find existing, return next available + marker
				if (_events[i].Type == eventType) return i;
			}
			return i + 10000; // actually somehow got through the entire loop
		}
		int findNext() => findNext(hsbTimer.Value);
		int findNext(int time)
		{
			int i;
			for (i = 0; i < _maxEvents; i++) if (_events[i].Time > time) break;
			return i;
		}
		Bitmap flatMask(Bitmap craftImage, byte iff, byte intensity)
		{
			// this is just for FG tags.  flat image, I only care about the shape.
			Bitmap bmpNew = new Bitmap(craftImage);
			BitmapData bmData = GraphicsFunctions.GetBitmapData(bmpNew, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride * bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			#region declare IFF
			byte[] rgb = new byte[3];
			switch (iff)
			{
				case 0:     // green
					rgb[1] = 1;
					break;
				case 2:     // blue
					rgb[1] = 2;
					rgb[2] = 1;
					break;
				case 3:     // purple
				case 5:     // purple2
					rgb[0] = 1;
					rgb[2] = 1;
					break;
				default:    // red
					rgb[0] = 1;
					break;
			}
			#endregion
			for (int y = 0; y < bmpNew.Height; y++)
			{
				for (int x = 0, pos = bmData.Stride * y; x < bmpNew.Width; x++)
				{
					if (pix[pos + x * 3] == 0) continue;
					pix[pos + x * 3] = (byte)(intensity * rgb[2]);
					pix[pos + x * 3 + 1] = (byte)(rgb[1] == 2 ? (intensity == 0xe0 ? 0x78 : (intensity == 0x78 ? 0x10 : (intensity == 0xfc ? 0xa0 : (intensity == 0xc8 ? 0x58 : (intensity == 0x94 ? 0x24 : 4))))) : intensity * rgb[1]);
					pix[pos + x * 3 + 2] = (byte)(intensity * rgb[0]);
				}
			}
			GraphicsFunctions.CopyBytesToImage(pix, bmData);
			bmpNew.UnlockBits(bmData);
			bmpNew.MakeTransparent(Color.Black);
			return bmpNew;
		}
		int[] getTagSize(int craft)
		{
			if (_xvtTagSizeCache == null) _xvtTagSizeCache = new int[imgCraft.Images.Count];

			int[] size = new int[2];
			if (craft >= 0 && craft < _xvtTagSizeCache.Length)
			{
				if (_xvtTagSizeCache[craft] == 0)
				{
					//Not scanned yet. Scan the bitmap for non RGB(0,0,0) pixels to detect its dimensions.
					Bitmap bmpNew = new Bitmap(imgCraft.Images[craft]);
					BitmapData bmData = GraphicsFunctions.GetBitmapData(bmpNew, PixelFormat.Format24bppRgb);
					byte[] pix = new byte[bmData.Stride * bmData.Height];
					GraphicsFunctions.CopyImageToBytes(bmData, pix);

					int left = imgCraft.Images[craft].Width, right = 0;
					int top = imgCraft.Images[craft].Height, bottom = 0;
					for (int y = 0; y < bmpNew.Height; y++)
						for (int x = 0, pos = bmData.Stride * y; x < bmpNew.Width; x++)
							if (pix[pos + x * 3] != 0 || pix[pos + x * 3 + 1] != 0 || pix[pos + x * 3 + 2] != 0)
							{
								if (x < left) left = x;
								if (x > right) right = x;
								if (y < top) top = y;
								if (y > bottom) bottom = y;
							}
					//Pack in the result. High word for width, low word for height.
					_xvtTagSizeCache[craft] = ((right - left + 1) << 16) | (bottom - top + 1);
				}
				size[0] = _xvtTagSizeCache[craft] >> 16;
				size[1] = _xvtTagSizeCache[craft] & 0xFFFF;
			}
			return size;    // size of base craft image as [width,height]
		}
		void drawImageQuad(int x, int y, int spacing, Bitmap craftImage, Graphics g)
		{
			g.DrawImageUnscaled(craftImage, x + spacing, y + spacing);
			g.DrawImageUnscaled(craftImage, x + spacing, y - spacing);
			g.DrawImageUnscaled(craftImage, x - spacing, y - spacing);
			g.DrawImageUnscaled(craftImage, x - spacing, y + spacing);
		}
		void popupUpdate(string s)
		{
			lblPopupInfo.Visible = true;
			lblPopupInfo.Text = s;
			tmrPopup.Start();
		}
		void popupPreviewStop()
		{
			if (!_popupPreviewActive) return;
			
			_mapX = (short)_popupPreviewMap.X;  //Restore prior map settings
			_mapY = (short)_popupPreviewMap.Y;
			_zoomX = (short)_popupPreviewZoom.X;
			_zoomY = (short)_popupPreviewZoom.Y;

			_popupPreviewActive = false;
			MapPaint();
		}
		void popupPreviewStart()
		{
			if (_popupPreviewActive) return;
			
			pctBrief.Focus();  //Need to force focus so it generates MouseWheel events
			_popupPreviewMap.X = _mapX;  //Backup existing map settings
			_popupPreviewMap.Y = _mapY;
			_popupPreviewZoom.X = _zoomX;
			_popupPreviewZoom.Y = _zoomY;

			_popupPreviewActive = true;
			MapPaint();
		}
		bool processEvent(int evtIndex, bool rebuild)
		{
			bool paint = false;
			int i = evtIndex;
			if (_events[i].Type == BaseBriefing.EventType.PageBreak)
			{
				if (_platform == Settings.Platform.TIE) lblTitle.Text = "";
				lblCaption.Text = "";
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.TitleText && _platform == Settings.Platform.TIE)  // XvT and XWA use .LST files
			{
				if (_strings[_events[i].Variables[0]].StartsWith(">"))
				{
					lblTitle.TextAlign = ContentAlignment.TopCenter;
					lblTitle.ForeColor = _titleColor;
					lblTitle.Text = _strings[_events[i].Variables[0]].Replace(">", "");
				}
				else
				{
					lblTitle.TextAlign = ContentAlignment.TopLeft;
					lblTitle.ForeColor = _normalColor;
					lblTitle.Text = _strings[_events[i].Variables[0]];
				}
			}
			else if (_events[i].Type == BaseBriefing.EventType.CaptionText && _events[i].Variables[0] != -1)
			{
				if (_strings[_events[i].Variables[0]].StartsWith(">"))
				{
					lblCaption.TextAlign = ContentAlignment.TopCenter;
					lblCaption.ForeColor = _titleColor;
					lblCaption.Text = _strings[_events[i].Variables[0]].Replace(">", "").Replace("$", "\r\n");
				}
				else
				{
					lblCaption.TextAlign = ContentAlignment.TopLeft;
					lblCaption.ForeColor = _normalColor;
					lblCaption.Text = _strings[_events[i].Variables[0]].Replace("$", "\r\n");
				}
				_page++;
			}
			else if (_events[i].Type == BaseBriefing.EventType.MoveMap)
			{
				_mapX = _events[i].Variables[0];
				_mapY = _events[i].Variables[1];
				if (_platform == Settings.Platform.XvT || _platform == Settings.Platform.BoP || _platform == Settings.Platform.XWA)
				{
					_mapX /= 2;
					_mapY /= 2;
				}
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.ZoomMap)
			{
				_zoomX = _events[i].Variables[0];
				_zoomY = _events[i].Variables[1];
				if (_zoomX < 1) _zoomX = 1;
				if (_zoomY < 1) _zoomY = 1;
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.ClearFGTags)
			{
				for (int h = 0; h < 8; h++)
				{
					_fgTags[h].Slot = -1;
					_fgTags[h].StartTime = 0;
				}
				paint = true;
			}
			else if (_events[i].IsFGTag)
			{
				int v = (int)_events[i].Type - (int)BaseBriefing.EventType.FGTag1;
				_fgTags[v].Slot = _events[i].Variables[0];
				_fgTags[v].StartTime = _events[i].Time;
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.ClearTextTags)
			{
				for (int h = 0; h < 8; h++)
				{
					_textTags[h].StringIndex = -1;
					_textTags[h].X = 0;
					_textTags[h].Y = 0;
					_textTags[h].ColorIndex = 0;
				}
				paint = true;
			}
			else if (_events[i].IsTextTag)
			{
				int v = (int)_events[i].Type - (int)BaseBriefing.EventType.TextTag1;
				_textTags[v].StringIndex = _events[i].Variables[0];
				_textTags[v].X = _events[i].Variables[1];
				_textTags[v].Y = _events[i].Variables[2];
				_textTags[v].ColorIndex = _events[i].Variables[3];
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaSetIcon)
			{
				_briefData[_events[i].Variables[0]].Craft = _events[i].Variables[1] - 1;
				_briefData[_events[i].Variables[0]].IFF = (byte)_events[i].Variables[2];
				_briefData[_events[i].Variables[0]].Name = "Icon #" + _events[i].Variables[0].ToString();
				_briefData[_events[i].Variables[0]].Waypoint = new short[4];
				_briefData[_events[i].Variables[0]].Waypoint[3] = (short)(_events[i].Variables[1] != 0 ? 1 : 0);
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaShipInfo)
			{
				if (_events[i].Variables[0] == 1)
				{
					_message = "Ship Info: " + (_briefData[_events[i].Variables[1]].Craft >= 0 ? Platform.Xwa.Strings.CraftType[_briefData[_events[i].Variables[1]].Craft + 1] : "<flight group not found>");
					if (!rebuild)
					{
						lblTitle.Visible = false;
						lblCaption.Visible = false;
					}
				}
				else
				{
					_message = "";
					lblTitle.Visible = true;
					lblCaption.Visible = true;
				}
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaMoveIcon && Common.IsValidArray(_briefData[_events[i].Variables[0]].Waypoint, 1))
			{
				_briefData[_events[i].Variables[0]].Waypoint[0] = _events[i].Variables[1];
				_briefData[_events[i].Variables[0]].Waypoint[1] = _events[i].Variables[2];
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaRotateIcon && Common.IsValidArray(_briefData[_events[i].Variables[0]].Waypoint, 2))
			{
				_briefData[_events[i].Variables[0]].Waypoint[2] = _events[i].Variables[1];
				paint = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaChangeRegion)
			{
				for (int h = 0; h < 8; h++)
				{
					_fgTags[h].Slot = -1;
					_fgTags[h].StartTime = 0;
					_textTags[h].StringIndex = -1;
					_textTags[h].X = 0;
					_textTags[h].Y = 0;
					_textTags[h].ColorIndex = 0;
				}
				_briefData = new BriefData[_briefData.Length];

				_message = "Region " + (_events[i].Variables[0] + 1);
				_regionDelay = _timerInterval * 3;

				if (!rebuild)
				{
					lblTitle.Visible = false;
					lblCaption.Visible = false;
				}
				paint = true;
			}
			// don't need to account for EndBriefing or SkipMarker
			return paint;
		}
		void resetBriefing()
		{
			_page = 0;
			_mapX = 0;
			_mapY = 0;
			_zoomX = 32;
			if (_platform == Settings.Platform.TIE) _zoomX = 48;
			_zoomY = _zoomX;
			for (int h = 0; h < 8; h++)
			{
				_fgTags[h].Slot = -1;
				_fgTags[h].StartTime = 0;
			}
			for (int h = 0; h < 8; h++)
			{
				_textTags[h].StringIndex = -1;
				_textTags[h].X = 0;
				_textTags[h].Y = 0;
				_textTags[h].ColorIndex = 0;
			}
			if (_platform == Settings.Platform.XWA) _briefData = new BriefData[_briefData.Length];
			_message = "";
			if (_platform == Settings.Platform.TIE) lblTitle.Text = "";
			lblCaption.Text = "";
			lblTitle.Visible = true;
			lblCaption.Visible = true;
		}
		void tieMask(Bitmap craftImage, byte iff)
		{
			// works a little different than mission map, everything guides off the B value and IFF
			// image is stored as blue due to non-standard G values that do not fit a good equation
			BitmapData bmData = GraphicsFunctions.GetBitmapData(craftImage, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride * bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			#region declare IFF
			byte[] rgb = new byte[3];
			switch (iff)
			{
				case 0:     // green
					rgb[1] = 1;
					break;
				case 2:     // blue
					rgb[1] = 2;
					rgb[2] = 1;
					break;
				case 3:     // purple
				case 5:     // purple2
					rgb[0] = 1;
					rgb[2] = 1;
					break;
				default:    // red
					rgb[0] = 1;
					break;
			}
			#endregion
			for (int y = 0; y < craftImage.Height; y++)
			{
				for (int x = 0, pos = bmData.Stride * y; x < craftImage.Width; x++)
				{
					pix[pos + x * 3 + 2] = (byte)(pix[pos + x * 3] * rgb[0]);
					pix[pos + x * 3 + 1] = (byte)(rgb[1] == 2 ? pix[pos + x * 3 + 1] : (pix[pos + x * 3] * rgb[1]));
					pix[pos + x * 3] = (byte)(pix[pos + x * 3] * rgb[2]);
				}
			}
			GraphicsFunctions.CopyBytesToImage(pix, bmData);
			craftImage.UnlockBits(bmData);
			craftImage.MakeTransparent(Color.Black);
		}
		void tiePaint()
		{
			if (_loading) return;

			int w = pctBrief.Width;
			int h = pctBrief.Height;
			int X = 2 * (-_zoomX * _mapX / 256) + w / 2;    // values are written out like this to force even numbers
			int Y = 2 * (-_zoomY * _mapY / 256) + h / 2;
			Pen pn = new Pen(Color.FromArgb(0x48, 0, 0)) { Width = 2 };
			Graphics g = Graphics.FromImage(_map);
			g.Clear(SystemColors.Control);
			SolidBrush sb = new SolidBrush(Color.Black);
			g.FillRectangle(sb, 0, 0, w, h);
			g.DrawLine(pn, 0, 1, w, 1);
			g.DrawLine(pn, 0, h - 3, w, h - 3);
			g.DrawLine(pn, 1, 0, 1, h);
			g.DrawLine(pn, w - 1, 0, w - 1, h);
			drawGrid(X, Y, g);
			Bitmap bmptemp;
			for (int i = 0; i < 8; i++)
			{
				if (_fgTags[i].Slot == -1 || _briefData[_fgTags[i].Slot].Waypoint[3] != 1) continue;

				byte iff = _briefData[_fgTags[i].Slot].IFF;
				sb.Color = _iffColors[iff < _iffColors.Length ? iff : 1]; // default to red
				SolidBrush sb2 = new SolidBrush(sb.Color = _iffBackColors[iff < _iffBackColors.Length ? iff : 1]);
				int wpX = 2 * (int)Math.Round((double)_zoomX * _briefData[_fgTags[i].Slot].Waypoint[0] / 256, 0) + X;
				int wpY = 2 * (int)Math.Round((double)_zoomY * -_briefData[_fgTags[i].Slot].Waypoint[1] / 256, 0) + Y;
				int frame = hsbTimer.Value - _fgTags[i].StartTime;
				if (_fgTags[i].StartTime == 0) frame = 12; // if tagged at t=0, just the box
				try { bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i].Slot].Craft]); }
				catch { bmptemp = new Bitmap(imgCraft.Images[0]); }
				switch (frame)
				{
					case 0:
						drawImageQuad(wpX - 16, wpY - 16, 32, flatMask(bmptemp, iff, 0xFC), g);
						break;
					case 1:
						drawImageQuad(wpX - 16, wpY - 16, 32, flatMask(bmptemp, iff, 0xC8), g);
						drawImageQuad(wpX - 16, wpY - 16, 28, flatMask(bmptemp, iff, 0xFC), g);
						break;
					case 2:
						drawImageQuad(wpX - 16, wpY - 16, 32, flatMask(bmptemp, iff, 0x94), g);
						drawImageQuad(wpX - 16, wpY - 16, 28, flatMask(bmptemp, iff, 0xC8), g);
						drawImageQuad(wpX - 16, wpY - 16, 24, flatMask(bmptemp, iff, 0xFC), g);
						break;
					case 3:
						drawImageQuad(wpX - 16, wpY - 16, 32, flatMask(bmptemp, iff, 0x60), g);
						drawImageQuad(wpX - 16, wpY - 16, 28, flatMask(bmptemp, iff, 0x94), g);
						drawImageQuad(wpX - 16, wpY - 16, 24, flatMask(bmptemp, iff, 0xC8), g);
						drawImageQuad(wpX - 16, wpY - 16, 20, flatMask(bmptemp, iff, 0xFC), g);
						break;
					case 4:
						drawImageQuad(wpX - 16, wpY - 16, 28, flatMask(bmptemp, iff, 0x60), g);
						drawImageQuad(wpX - 16, wpY - 16, 24, flatMask(bmptemp, iff, 0x94), g);
						drawImageQuad(wpX - 16, wpY - 16, 20, flatMask(bmptemp, iff, 0xC8), g);
						drawImageQuad(wpX - 16, wpY - 16, 16, flatMask(bmptemp, iff, 0xFC), g);
						break;
					case 5:
						drawImageQuad(wpX - 16, wpY - 16, 24, flatMask(bmptemp, iff, 0x60), g);
						drawImageQuad(wpX - 16, wpY - 16, 20, flatMask(bmptemp, iff, 0x94), g);
						drawImageQuad(wpX - 16, wpY - 16, 16, flatMask(bmptemp, iff, 0xC8), g);
						drawImageQuad(wpX - 16, wpY - 16, 12, flatMask(bmptemp, iff, 0xFC), g);
						break;
					case 6:
						drawImageQuad(wpX - 16, wpY - 16, 20, flatMask(bmptemp, iff, 0x60), g);
						drawImageQuad(wpX - 16, wpY - 16, 16, flatMask(bmptemp, iff, 0x94), g);
						drawImageQuad(wpX - 16, wpY - 16, 12, flatMask(bmptemp, iff, 0xC8), g);
						drawImageQuad(wpX - 16, wpY - 16, 8, flatMask(bmptemp, iff, 0xFC), g);
						break;
					case 7:
						drawImageQuad(wpX - 16, wpY - 16, 16, flatMask(bmptemp, iff, 0x60), g);
						drawImageQuad(wpX - 16, wpY - 16, 12, flatMask(bmptemp, iff, 0x94), g);
						drawImageQuad(wpX - 16, wpY - 16, 8, flatMask(bmptemp, iff, 0xC8), g);
						drawImageQuad(wpX - 16, wpY - 16, 4, flatMask(bmptemp, iff, 0xFC), g);
						break;
					case 8:
						drawImageQuad(wpX - 16, wpY - 16, 12, flatMask(bmptemp, iff, 0x60), g);
						drawImageQuad(wpX - 16, wpY - 16, 8, flatMask(bmptemp, iff, 0x94), g);
						drawImageQuad(wpX - 16, wpY - 16, 4, flatMask(bmptemp, iff, 0xC8), g);
						break;
					case 9:
						drawImageQuad(wpX - 16, wpY - 16, 8, flatMask(bmptemp, iff, 0x60), g);
						drawImageQuad(wpX - 16, wpY - 16, 4, flatMask(bmptemp, iff, 0x94), g);
						break;
					case 10:
						drawImageQuad(wpX - 16, wpY - 16, 4, flatMask(bmptemp, iff, 0x60), g);
						break;
					case 11:
						g.FillRectangle(sb, wpX - 8, wpY - 8, 18, 18);
						g.FillRectangle(sb2, wpX - 6, wpY - 6, 14, 14);
						break;
					default:
						// 12 or greater, just the box
						g.FillRectangle(sb, wpX - 12, wpY - 12, 26, 26);
						g.FillRectangle(sb2, wpX - 10, wpY - 10, 22, 22);
						break;
				}
			}
			for (int i = 0; i < 8; i++)
			{
				if (_textTags[i].StringIndex == -1) continue;

				sb.Color = _tagColors[_textTags[i].ColorIndex < _tagColors.Length ? _textTags[i].ColorIndex : 1]; // default to red
				short left = (short)(2 * (int)Math.Round((double)_zoomX * _textTags[i].X / 256, 0) + X);
				short top = (short)(2 * (int)Math.Round((double)_zoomY * _textTags[i].Y / 256, 0) + Y);
				if (_empire != null) { displayString(g, "FONTfont6", _tags[_textTags[i].StringIndex], left, top, sb.Color); }
				else g.DrawString(_tags[_textTags[i].StringIndex], new System.Drawing.Font("MS Reference Sans Serif", 10), sb, left, top);
			}
			for (int i = 0; i < _briefData.Length; i++)
			{
				if (_briefData[i].Waypoint[3] != 1) continue;

				if (_zoomX >= 32)
				{
					try { bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft]); }
					catch { bmptemp = new Bitmap(imgCraft.Images[0]); }
				}
				else
				{
					try { bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft + 88]); }    // small icon
					catch { bmptemp = new Bitmap(imgCraft.Images[88]); }
				}
				tieMask(bmptemp, _briefData[i].IFF);
				// simple base-256 grid coords * zoom to get pixel location, * 2 to enlarge, + map offset, - pic size/2 to center
				// forced to even numbers
				g.DrawImageUnscaled(bmptemp, 2 * (int)Math.Round((double)_zoomX * _briefData[i].Waypoint[0] / 256, 0) + X - 16, 2 * (int)Math.Round((double)_zoomY * -_briefData[i].Waypoint[1] / 256, 0) + Y - 16);  //[JB] Invert Y axis of waypoint, fixed Y zoom.
			}
			g.DrawString("#" + _page, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.White), w - 20, 4);
			pctBrief.Invalidate();
			g.Dispose();
		}
		Bitmap xvtMask(Bitmap craftImage, byte iff, byte frame)
		{
			Bitmap bmpNew = new Bitmap(craftImage);
			BitmapData bmData = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride * bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			#region define FG tag colors
			byte[] p = new byte[5];
			byte intensity = 255;
			if (frame == 2) intensity = 0xC5;
			else if (frame == 3) intensity = 0x8F;
			else if (frame == 4) intensity = 0x5B;
			p[0] = (byte)(0xB8 * (intensity + 1) / 256);
			p[1] = (byte)(0x98 * (intensity + 1) / 256);
			p[2] = (byte)(0x70 * (intensity + 1) / 256);
			p[3] = (byte)(0x58 * (intensity + 1) / 256);
			#endregion
			Color useMask = _maskColors[iff * 6 + 5];
			Color[] clr = new Color[5];
			for (int i = 0; i < 5; i++) clr[i] = _maskColors[iff * 6 + i];
			byte b;
			for (int y = 0; y < bmpNew.Height; y++)
			{
				for (int x = 0, pos = y * bmData.Stride; x < bmpNew.Width; x++)
				{
					// stupid thing returns BGR instead of RGB
					b = pix[pos + x * 3 + 1];
					int index = 4 - (b / 0x28);
					if (frame == 0)
					{
						pix[pos + x * 3] = clr[index].B;
						pix[pos + x * 3 + 1] = clr[index].G;
						pix[pos + x * 3 + 2] = clr[index].R;
						continue;
					}
					pix[pos + x * 3] = (byte)(p[index] * useMask.B);
					pix[pos + x * 3 + 1] = (byte)(p[index] * useMask.G);
					pix[pos + x * 3 + 2] = (byte)(p[index] * useMask.R);
				}
			}
			GraphicsFunctions.CopyBytesToImage(pix, bmData);
			bmpNew.UnlockBits(bmData);
			bmpNew.MakeTransparent(Color.Black);
			return bmpNew;
		}
		void xvtPaint()
		{
			if (_loading) return;

			int w = pctBrief.Width;
			int h = pctBrief.Height;
			int X = 2 * (-_zoomX * _mapX / 256) + w / 2;
			int Y = 2 * (-_zoomY * _mapY / 256) + h / 2;    //[JB] mapX(-320) and mapY(+200) offsets better approximate the viewport as it appears in game, although it slightly messes up the point calculations used elsewhere
			Graphics g;
			g = Graphics.FromImage(_map);
			g.Clear(SystemColors.Control);
			SolidBrush sb = new SolidBrush(Color.Black);
			g.FillRectangle(sb, 0, 0, w, h);
			drawGrid(X, Y, g);
			Bitmap bmptemp;
			#region FG tags
			int briefIndex = cboBriefIndex1.SelectedIndex;
			if (briefIndex < 0) briefIndex = 0;
			BaseFlightGroup.Waypoint wp;
			for (int i = 0; i < 8; i++)
			{
				if (_fgTags[i].Slot == -1) continue;

				wp = _briefData[_fgTags[i].Slot].WaypointArr[briefIndex];
				if (!wp.Enabled) continue;

				byte iff = _briefData[_fgTags[i].Slot].IFF;
				sb.Color = _iffColors[iff < _iffColors.Length ? iff : 1]; // default to red
				SolidBrush sb2 = new SolidBrush(sb.Color = _iffBackColors[iff < _iffBackColors.Length ? iff : 1]);
				int wpX = (int)Math.Round((double)_zoomX * wp[0] / 256, 0) + X;
				int wpY = (int)Math.Round((double)_zoomY * -wp[1] / 256, 0) + Y;
				int frame = hsbTimer.Value - _fgTags[i].StartTime;
				if (_fgTags[i].StartTime == 0) frame = 12; // if tagged at t=0, just the box
				int[] pos = getTagSize(_briefData[_fgTags[i].Slot].Craft);
				try { bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i].Slot].Craft]); }
				catch { bmptemp = new Bitmap(imgCraft.Images[0]); }
				switch (frame)
				{
					case 0:
						drawImageQuad(wpX - 11, wpY - 11, 16, xvtMask(bmptemp, iff, 1), g);
						break;
					case 1:
						drawImageQuad(wpX - 11, wpY - 11, 16, xvtMask(bmptemp, iff, 2), g);
						drawImageQuad(wpX - 11, wpY - 11, 14, xvtMask(bmptemp, iff, 1), g);
						break;
					case 2:
						drawImageQuad(wpX - 11, wpY - 11, 16, xvtMask(bmptemp, iff, 3), g);
						drawImageQuad(wpX - 11, wpY - 11, 14, xvtMask(bmptemp, iff, 2), g);
						drawImageQuad(wpX - 11, wpY - 11, 12, xvtMask(bmptemp, iff, 1), g);
						break;
					case 3:
						drawImageQuad(wpX - 11, wpY - 11, 16, xvtMask(bmptemp, iff, 4), g);
						drawImageQuad(wpX - 11, wpY - 11, 14, xvtMask(bmptemp, iff, 3), g);
						drawImageQuad(wpX - 11, wpY - 11, 12, xvtMask(bmptemp, iff, 2), g);
						drawImageQuad(wpX - 11, wpY - 11, 10, xvtMask(bmptemp, iff, 1), g);
						break;
					case 4:
						drawImageQuad(wpX - 11, wpY - 11, 14, xvtMask(bmptemp, iff, 4), g);
						drawImageQuad(wpX - 11, wpY - 11, 12, xvtMask(bmptemp, iff, 3), g);
						drawImageQuad(wpX - 11, wpY - 11, 10, xvtMask(bmptemp, iff, 2), g);
						drawImageQuad(wpX - 11, wpY - 11, 8, xvtMask(bmptemp, iff, 1), g);
						break;
					case 5:
						drawImageQuad(wpX - 11, wpY - 11, 12, xvtMask(bmptemp, iff, 4), g);
						drawImageQuad(wpX - 11, wpY - 11, 10, xvtMask(bmptemp, iff, 3), g);
						drawImageQuad(wpX - 11, wpY - 11, 8, xvtMask(bmptemp, iff, 2), g);
						drawImageQuad(wpX - 11, wpY - 11, 6, xvtMask(bmptemp, iff, 1), g);
						break;
					case 6:
						drawImageQuad(wpX - 11, wpY - 11, 10, xvtMask(bmptemp, iff, 4), g);
						drawImageQuad(wpX - 11, wpY - 11, 8, xvtMask(bmptemp, iff, 3), g);
						drawImageQuad(wpX - 11, wpY - 11, 6, xvtMask(bmptemp, iff, 2), g);
						drawImageQuad(wpX - 11, wpY - 11, 4, xvtMask(bmptemp, iff, 1), g);
						break;
					case 7:
						drawImageQuad(wpX - 11, wpY - 11, 8, xvtMask(bmptemp, iff, 4), g);
						drawImageQuad(wpX - 11, wpY - 11, 6, xvtMask(bmptemp, iff, 3), g);
						drawImageQuad(wpX - 11, wpY - 11, 4, xvtMask(bmptemp, iff, 2), g);
						drawImageQuad(wpX - 11, wpY - 11, 2, xvtMask(bmptemp, iff, 1), g);
						break;
					case 8:
						drawImageQuad(wpX - 11, wpY - 11, 6, xvtMask(bmptemp, iff, 4), g);
						drawImageQuad(wpX - 11, wpY - 11, 4, xvtMask(bmptemp, iff, 3), g);
						drawImageQuad(wpX - 11, wpY - 11, 2, xvtMask(bmptemp, iff, 2), g);
						break;
					case 9:
						drawImageQuad(wpX - 11, wpY - 11, 4, xvtMask(bmptemp, iff, 4), g);
						drawImageQuad(wpX - 11, wpY - 11, 2, xvtMask(bmptemp, iff, 3), g);
						break;
					case 10:
						drawImageQuad(wpX - 11, wpY - 11, 2, xvtMask(bmptemp, iff, 4), g);
						break;
					case 11:
						g.FillRectangle(sb, wpX - (pos[0] / 2), wpY - (pos[1] / 2), pos[0], pos[1]);
						g.FillRectangle(sb2, wpX - (pos[0] / 2 - 1), wpY - (pos[1] / 2 - 1), pos[0] - 2, pos[1] - 2);
						break;
					default:
						// 12 or greater, just the box
						g.FillRectangle(sb, wpX - (pos[0] / 2 + 2), wpY - (pos[1] / 2 + 2), pos[0] + 4, pos[1] + 4);
						g.FillRectangle(sb2, wpX - (pos[0] / 2 + 1), wpY - (pos[1] / 2 + 1), pos[0] + 2, pos[1] + 2);
						break;
				}
			}
			#endregion FG tags
			for (int i = 0; i < 8; i++)
			{
				if (_textTags[i].StringIndex == -1) continue;

				sb.Color = _tagColors[_textTags[i].ColorIndex < _tagColors.Length ? _textTags[i].ColorIndex : 1]; // default to red
				g.DrawString(_tags[_textTags[i].StringIndex], new System.Drawing.Font("MS Reference Sans Serif", 6), sb, (int)Math.Round((double)_zoomX * _textTags[i].X / 256, 0) + X, (int)Math.Round((double)_zoomY * _textTags[i].Y / 256, 0) + Y);
			}
			for (int i = 0; i < _briefData.Length; i++)
			{
				wp = _briefData[i].WaypointArr[briefIndex];
				if (!wp.Enabled) continue;

				try { bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft]); }
				catch { bmptemp = new Bitmap(imgCraft.Images[0]); }
				bmptemp = xvtMask(bmptemp, _briefData[i].IFF, 0);
				int[] pos = getTagSize(_briefData[i].Craft);
				// simple base-256 grid coords * zoom to get pixel location, + map offset, - pic size/2 to center
				g.DrawImageUnscaled(bmptemp, (int)Math.Round((double)_zoomX * wp[0] / 256, 0) + X - 11 + (pos[0] % 2), (int)Math.Round((double)_zoomY * -wp[1] / 256, 0) + Y - 11); //[JB] Invert Y axis. Also fixed Y axis zoom (was using X).
			}
			g.DrawString("#" + _page, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.White), w - 20, 4);
			pctBrief.Invalidate();
			g.Dispose();
		}
		Bitmap xwaMask(Bitmap craftImage, byte iff)
		{
			Bitmap bmpNew = new Bitmap(craftImage);
			BitmapData bmData = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride * bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			byte r = _maskColors[iff < _maskColors.Length ? iff : 1].R;	// default to red
			byte g = _maskColors[iff < _maskColors.Length ? iff : 1].G;
			byte b = _maskColors[iff < _maskColors.Length ? iff : 1].B;
			for (int y = 0; y < bmpNew.Height; y++)
			{
				for (int x = 0, pos = y * bmData.Stride; x < bmpNew.Width; x++)
				{
					pix[pos + x * 3] = (byte)(pix[pos + x * 3] * b / 256);
					pix[pos + x * 3 + 1] = (byte)(pix[pos + x * 3 + 1] * g / 256);
					pix[pos + x * 3 + 2] = (byte)(pix[pos + x * 3 + 2] * r / 256);
				}
			}
			GraphicsFunctions.CopyBytesToImage(pix, bmData);
			bmpNew.UnlockBits(bmData);
			bmpNew.MakeTransparent(Color.Black);
			return bmpNew;
		}
		void xwaPaint()
		{
			if (_loading) return;

			int w = pctBrief.Width;
			int h = pctBrief.Height;
			int X = 2 * (-_zoomX * _mapX / 256) + w / 2;
			int Y = 2 * (-_zoomY * _mapY / 256) + h / 2;
			Graphics g;
			g = Graphics.FromImage(_map);
			g.Clear(SystemColors.Control);
			SolidBrush sb = new SolidBrush(Color.FromArgb(0x18, 0x18, 0x18));
			g.FillRectangle(sb, 0, 0, w, h);
			if (_message != "")
			{
				sb.Color = Color.FromArgb(0xE7, 0xE3, 0);   // yellow
				StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
				g.DrawString(_message, new System.Drawing.Font("Arial", 12, FontStyle.Bold), sb, w / 2, h / 2, sf);
				pctBrief.Invalidate();
				g.Dispose();
				return;
			}
			drawGrid(X, Y, g);
			Bitmap bmptemp;
			#region FG tags
			for (int i = 0; i < 8; i++)
			{
				if (_fgTags[i].Slot == -1 || _briefData[_fgTags[i].Slot].Waypoint == null || _briefData[_fgTags[i].Slot].Waypoint[3] != 1) continue;

				byte iff = _briefData[_fgTags[i].Slot].IFF;
				sb.Color = _iffColors[iff < _iffColors.Length ? iff : 1]; // default to red
				SolidBrush sb2 = new SolidBrush(sb.Color = _iffBackColors[iff < _iffBackColors.Length ? iff : 1]);
				int wpX = (int)Math.Round((double)_zoomX * _briefData[_fgTags[i].Slot].Waypoint[0] / 256, 0) + X;
				int wpY = (int)Math.Round((double)_zoomY * _briefData[_fgTags[i].Slot].Waypoint[1] / 256, 0) + Y;
				int frame = hsbTimer.Value - _fgTags[i].StartTime;
				if (_fgTags[i].StartTime == 0) frame = 12; // if tagged at t=0, just the box
				byte r = sb.Color.R, b = sb.Color.B, gn = sb.Color.G;
				try { bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i].Slot].Craft]); }
				catch { bmptemp = new Bitmap(imgCraft.Images[0]); } // if it breaks, use an X-wing
				bmptemp = xwaMask(bmptemp, iff);
				if (_briefData[_fgTags[i].Slot].Waypoint[2] == 1) bmptemp.RotateFlip(RotateFlipType.Rotate270FlipNone);
				else if (_briefData[_fgTags[i].Slot].Waypoint[2] == 2) bmptemp.RotateFlip(RotateFlipType.Rotate180FlipNone);
				else if (_briefData[_fgTags[i].Slot].Waypoint[2] == 3) bmptemp.RotateFlip(RotateFlipType.Rotate90FlipNone);
				else if (_briefData[_fgTags[i].Slot].Waypoint[2] == 4) bmptemp.RotateFlip(RotateFlipType.RotateNoneFlipX);
				switch (frame)
				{
					case 0:
						drawImageQuad(wpX - 28, wpY - 28, 16, bmptemp, g);
						break;
					case 1:
						drawImageQuad(wpX - 28, wpY - 28, 16, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 14, bmptemp, g);
						break;
					case 2:
						drawImageQuad(wpX - 28, wpY - 28, 16, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 14, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 12, bmptemp, g);
						break;
					case 3:
						drawImageQuad(wpX - 28, wpY - 28, 16, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 14, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 12, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 10, bmptemp, g);
						break;
					case 4:
						drawImageQuad(wpX - 28, wpY - 28, 14, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 12, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 10, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 8, bmptemp, g);
						break;
					case 5:
						drawImageQuad(wpX - 28, wpY - 28, 12, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 10, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 8, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 6, bmptemp, g);
						break;
					case 6:
						drawImageQuad(wpX - 28, wpY - 28, 10, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 8, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 6, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 4, bmptemp, g);
						break;
					case 7:
						drawImageQuad(wpX - 28, wpY - 28, 8, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 6, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 4, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 2, bmptemp, g);
						break;
					case 8:
						g.FillRectangle(sb2, wpX - 12, wpY - 11, 25, 22);
						drawImageQuad(wpX - 28, wpY - 28, 6, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 4, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 2, bmptemp, g);
						break;
					case 9:
						if (r == 0xE0) r = 0x90;
						else if (r == 0x60) r = 0x30;
						if (gn == 0xE0) r = 0x94;
						if (b == 0xE0) b = 0x90;
						else if (b == 0x60) b = 0x30;
						sb.Color = Color.FromArgb(r, gn, b);
						// TODO: these aren't fixed sizes
						g.FillRectangle(sb, wpX - 13, wpY - 12, 27, 24);
						g.FillRectangle(sb2, wpX - 12, wpY - 11, 25, 22);
						drawImageQuad(wpX - 28, wpY - 28, 4, bmptemp, g);
						drawImageQuad(wpX - 28, wpY - 28, 2, bmptemp, g);
						break;
					case 10:
						if (r == 0xE0) r = 0xA8;
						else if (r == 0x60) r = 0x40;
						if (gn == 0xE0) r = 0xAC;
						if (b == 0xE0) b = 0xA8;
						else if (b == 0x60) b = 0x40;
						sb.Color = Color.FromArgb(r, gn, b);
						g.FillRectangle(sb, wpX - 14, wpY - 13, 29, 26);
						g.FillRectangle(sb2, wpX - 13, wpY - 12, 27, 24);
						drawImageQuad(wpX - 28, wpY - 28, 2, bmptemp, g);
						break;
					case 11:
						if (r == 0xE0) r = 0xC8;
						else if (r == 0x60) r = 0x50;
						if (gn == 0xE0) r = 0xC8;
						if (b == 0xE0) b = 0xC8;
						else if (b == 0x60) b = 0x50;
						sb.Color = Color.FromArgb(r, gn, b);
						g.FillRectangle(sb, wpX - 15, wpY - 14, 31, 28);
						g.FillRectangle(sb2, wpX - 14, wpY - 13, 29, 26);
						break;
					default:
						// 12 or greater, just the box
						g.FillRectangle(sb, wpX - 17, wpY - 16, 35, 32);
						g.FillRectangle(sb2, wpX - 16, wpY - 15, 33, 30);
						break;
				}
			}
			#endregion FG tags
			for (int i = 0; i < 8; i++)
			{
				if (_textTags[i].StringIndex == -1) continue;

				sb.Color = _tagColors[_textTags[i].ColorIndex < _tagColors.Length ? _textTags[i].ColorIndex : 1]; // default to red
				g.DrawString(_tags[_textTags[i].StringIndex], new System.Drawing.Font("Arial", 9, FontStyle.Bold), sb, (int)Math.Round((double)_zoomX * _textTags[i].X / 256, 0) + X, (int)Math.Round((double)_zoomY * _textTags[i].Y / 256, 0) + Y);
			}
			for (int i = 0; i < _briefData.Length; i++)
			{
				if (_briefData[i].Waypoint == null || _briefData[i].Waypoint[3] != 1) continue;

				try { bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft]); }
				catch { bmptemp = new Bitmap(imgCraft.Images[0]); }
				bmptemp = xwaMask(bmptemp, _briefData[i].IFF);
				if (_briefData[i].Waypoint[2] == 1) bmptemp.RotateFlip(RotateFlipType.Rotate270FlipNone);
				else if (_briefData[i].Waypoint[2] == 2) bmptemp.RotateFlip(RotateFlipType.Rotate180FlipNone);
				else if (_briefData[i].Waypoint[2] == 3) bmptemp.RotateFlip(RotateFlipType.Rotate90FlipNone);
				else if (_briefData[i].Waypoint[2] == 4) bmptemp.RotateFlip(RotateFlipType.RotateNoneFlipX);
				// simple base-256 grid coords * zoom to get pixel location, + map offset, - pic size/2 to center
				g.DrawImageUnscaled(bmptemp, (int)Math.Round((double)_zoomX * _briefData[i].Waypoint[0] / 256, 0) + X - 28, (int)Math.Round((double)_zoomY * _briefData[i].Waypoint[1] / 256, 0) + Y - 28);  //[JB] Fixed Y zoom.
			}
			g.DrawString("#" + _page, new System.Drawing.Font("Arial", 8), new SolidBrush(Color.White), w - 20, 4);
			pctBrief.Invalidate();
			g.Dispose();
		}

		void cboColorTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			_textTags[(int)numText.Value - 1].ColorIndex = cboColorTag.SelectedIndex;
			MapPaint();
		}
		void cboIconIff_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_briefData[_icon].IFF = (byte)cboIconIff.SelectedIndex;
			MapPaint();
		}
		void cboMoveIcon_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			if (_tempX != -621 && _tempY != -621)
			{
				// here, temp is the OLD location of the icon
				short selectedX = _briefData[cboMoveIcon.SelectedIndex].Waypoint[0];
				short selectedY = _briefData[cboMoveIcon.SelectedIndex].Waypoint[1];
				try
				{
					_briefData[cboMoveIcon.SelectedIndex].Waypoint[0] = _briefData[_icon].Waypoint[0];
					_briefData[cboMoveIcon.SelectedIndex].Waypoint[1] = _briefData[_icon].Waypoint[1];
				}
				catch (NullReferenceException) { /* do nothing*/ }
				finally
				{
					try
					{
						_briefData[_icon].Waypoint[0] = _tempX;
						_briefData[_icon].Waypoint[1] = _tempY;
					}
					catch (NullReferenceException) { /* do nothing*/ }
					_tempX = selectedX;
					_tempY = selectedY;
				}
			}
			_icon = (short)cboMoveIcon.SelectedIndex;
			MapPaint();
		}
		void cboNCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_briefData[_icon].Craft = cboNCraft.SelectedIndex - 1;
			if (cboNCraft.SelectedIndex == 0) _briefData[_icon].Waypoint[3] = 0;
			MapPaint();
		}
		void cboNewIcon_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_briefData[_icon] = _tempBD;
			_icon = (short)cboNewIcon.SelectedIndex;
			_tempBD = _briefData[_icon];
			_briefData[_icon].Craft = cboNCraft.SelectedIndex - 1;
			_briefData[_icon].IFF = (byte)cboIconIff.SelectedIndex;
			if (_tempX != -621 && _tempY != -621)
			{
				_briefData[_icon].Waypoint = new short[4];
				_briefData[_icon].Waypoint[0] = _tempX;
				_briefData[_icon].Waypoint[1] = _tempY;
				if (cboNCraft.SelectedIndex != 0) _briefData[_icon].Waypoint[3] = 1;
			}
			MapPaint();
		}
		void cboRCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			try { _briefData[_icon].Waypoint[2] = _tempX; }
			catch (NullReferenceException) { /* do nothing */ }
			_icon = (short)cboRCraft.SelectedIndex;
			try
			{
				_tempX = _briefData[_icon].Waypoint[2];
				_briefData[_icon].Waypoint[2] = (short)cboRotateAmount.SelectedIndex;
			}
			catch (NullReferenceException) { _tempX = 0; }
			MapPaint();
		}
		void cboRotateAmount_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			try { _briefData[_icon].Waypoint[2] = (short)cboRotateAmount.SelectedIndex; }
			catch (NullReferenceException) { /* do nothing*/ }
			MapPaint();
		}
		void cboTextTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			_textTags[(int)numText.Value - 1].StringIndex = cboTextTag.SelectedIndex;
			MapPaint();
		}

		void cmdBreak_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.PageBreak;
			enableOkCancel(true);
		}
		void cmdCancel_Click(object sender, EventArgs e)
		{
			cboText.Enabled = false;
			optFG.Enabled = false;
			optText.Enabled = false;
			lblTitle.Visible = true;
			lblCaption.Visible = true;
			hsbBRF.Visible = false;
			vsbBRF.Visible = false;
			lblInstruction.Visible = false;
			if (_eventType == BaseBriefing.EventType.CaptionText && sender.ToString() != "OK") { _page--; }
			else if (_eventType == BaseBriefing.EventType.TextTag1 && sender.ToString() != "OK") { _textTags = _ttBackup; }
			else if (_eventType == BaseBriefing.EventType.MoveMap && sender.ToString() != "OK")
			{
				_mapX = _tempX;
				_mapY = _tempY;
			}
			else if (_eventType == BaseBriefing.EventType.ZoomMap && sender.ToString() != "OK")
			{
				_zoomX = _tempX;
				_zoomY = _tempY;
			}
			else if (_eventType == BaseBriefing.EventType.XwaRotateIcon && sender.ToString() != "OK")
			{
				try { _briefData[_icon].Waypoint[2] = _tempX; }
				catch (NullReferenceException) { /* do nothing */ }
			}
			else if (_eventType == BaseBriefing.EventType.XwaMoveIcon && sender.ToString() != "OK")
			{
				try
				{
					_briefData[_icon].Waypoint[0] = _tempX;
					_briefData[_icon].Waypoint[1] = _tempY;
				}
				catch (NullReferenceException) { /* do nothing */ }
			}
			_eventType = 0;
			enableOkCancel(false);
			MapPaint();
		}
		void cmdCaption_Click(object sender, EventArgs e)
		{
			cboText.Enabled = true;
			_eventType = BaseBriefing.EventType.CaptionText;
            _page++;
            enableOkCancel(true);
		}
		void cmdClear_Click(object sender, EventArgs e)
		{
			optFG.Enabled = true;
			optText.Enabled = true;
			_eventType = BaseBriefing.EventType.ClearFGTags;
			enableOkCancel(true);
		}
		void cmdFG_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.FGTag1;
			pnlShipTag.Visible = true;
			enableOkCancel(true);
		}
		void cmdOk_Click(object sender, EventArgs e)
		{
			if (!hasAvailableEventSpace(2 + BaseBriefing.EventParameters.GetCount(_eventType)))
			{
				MessageBox.Show("Event list is full, cannot add more.", "Error");
				cmdCancel_Click(0, new EventArgs());
				return;
			}

			if (_eventType == BaseBriefing.EventType.ClearFGTags && optText.Checked) _eventType = BaseBriefing.EventType.ClearTextTags;
			int i = -1;
			switch (_eventType)
			{
				case BaseBriefing.EventType.SkipMarker:
                    i = findExisting(_eventType);
                    if (i < 10000) { break; }  // no further action, existing found

					i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					lstEvents.Items.Insert(i, "");
					break;
				case BaseBriefing.EventType.PageBreak:
					i = findExisting(_eventType);
					if (i < 10000) { break; }

					i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					lstEvents.Items.Insert(i, "");
					if (_platform == Settings.Platform.TIE) lblTitle.Text = "";
					lblCaption.Text = "";
					break;
				case BaseBriefing.EventType.TitleText:
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
					}
					_events[i].Variables[0] = (short)((cboText.SelectedIndex >= 0) ? cboText.SelectedIndex : 0);
					if (_strings[_events[i].Variables[0]].StartsWith(">"))
					{
						lblTitle.TextAlign = ContentAlignment.TopCenter;
						lblTitle.ForeColor = _titleColor;
						lblTitle.Text = _strings[_events[i].Variables[0]].Replace(">", "");
					}
					else
					{
						lblTitle.TextAlign = ContentAlignment.TopLeft;
						lblTitle.ForeColor = _normalColor;
						lblTitle.Text = _strings[_events[i].Variables[0]];
					}
					break;
				case BaseBriefing.EventType.CaptionText:
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
					}
					_events[i].Variables[0] = (short)cboText.SelectedIndex;
					if (_strings[_events[i].Variables[0]].StartsWith(">"))
					{
						lblCaption.TextAlign = ContentAlignment.TopCenter;
						lblCaption.ForeColor = _titleColor;
						lblCaption.Text = _strings[_events[i].Variables[0]].Replace(">", "");
					}
					else
					{
						lblCaption.TextAlign = ContentAlignment.TopLeft;
						lblCaption.ForeColor = _normalColor;
						lblCaption.Text = _strings[_events[i].Variables[0]];
					}
                    break;
				case BaseBriefing.EventType.MoveMap:
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
					}
					_events[i].Variables[0] = _mapX;
					_events[i].Variables[1] = _mapY;
					// don't need to repaint, done while adjusting values
					break;
				case BaseBriefing.EventType.ZoomMap:
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
					}
					_events[i].Variables[0] = _zoomX;
					_events[i].Variables[1] = _zoomY;
					// don't need to repaint, done while adjusting values
					break;
				case BaseBriefing.EventType.ClearFGTags:
					i = findExisting(_eventType);
					if (i < 10000) break;

					i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					lstEvents.Items.Insert(i, "");
					for (int n = 0; n < 8; n++)
					{
						_fgTags[n].Slot = -1;
						_fgTags[n].StartTime = 0;
					}
					break;
				case BaseBriefing.EventType.FGTag1:
					_eventType = (BaseBriefing.EventType)((int)_eventType + numFG.Value - 1);
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
					}
					_events[i].Variables[0] = (short)cboFGTag.SelectedIndex;
					_fgTags[(int)_eventType - (int)BaseBriefing.EventType.FGTag1].Slot = _events[i].Variables[0];
					_fgTags[(int)_eventType - (int)BaseBriefing.EventType.FGTag1].StartTime = _events[i].Time;
					MapPaint();
					break;
				case BaseBriefing.EventType.ClearTextTags:
					i = findExisting(_eventType);
					if (i < 10000) break;

					i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					lstEvents.Items.Insert(i, "");
					for (int n = 0; n < 8; n++)
					{
						_textTags[n].StringIndex = -1;
						_textTags[n].X = 0;
					}
					break;
				case BaseBriefing.EventType.TextTag1:
					_eventType = (BaseBriefing.EventType)((int)_eventType + numText.Value - 1);
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						if (_tempX == -621 && _tempY == -621)
						{
							MessageBox.Show("No tag location selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							i = 0;
							break;
						}
						i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
					}
					else
					{
						// found existing, just see if we change location or not
						if (_tempX == -621 && _tempY == -621)
						{
							_tempX = _events[i].Variables[1];
							_tempY = _events[i].Variables[2];
						}
					}
					_events[i].Variables[0] = (short)cboTextTag.SelectedIndex;
					_events[i].Variables[1] = _tempX;
					_events[i].Variables[2] = _tempY;
					_events[i].Variables[3] = (short)cboColorTag.SelectedIndex;
					// don't need to repaint or restore/edit from backup, as it's taken care of during placement
					break;
				case BaseBriefing.EventType.XwaSetIcon:
					if (_tempX == -621 && _tempY == -621 && cboNCraft.SelectedIndex == 0)
					{
						MessageBox.Show("No craft location selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						i = 0;
						break;
					}

					i = _events.Insert(findNext(), new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					lstEvents.Items.Insert(i, "");
					_events[i].Variables[0] = _icon;
					_events[i].Variables[1] = (short)cboNCraft.SelectedIndex;
					_events[i].Variables[2] = (short)cboIconIff.SelectedIndex;
					updateList(i);
					// add initial MoveIcon 
					if (cboNCraft.SelectedIndex != 0)
					{
						i = _events.Insert(findNext(), new BaseBriefing.Event(BaseBriefing.EventType.XwaMoveIcon) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
						_events[i].Variables[0] = _icon;
						_events[i].Variables[1] = _tempX;
						_events[i].Variables[2] = _tempY;
					}
					break;
				case BaseBriefing.EventType.XwaShipInfo:
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
					}
					_events[i].Variables[0] = (short)(optInfoOn.Checked ? 1 : 0);
					_events[i].Variables[1] = (short)cboInfoCraft.SelectedIndex;
					break;
				case BaseBriefing.EventType.XwaMoveIcon:
					if (_tempX == -621 && _tempY == -621)
					{
						MessageBox.Show("No craft location or valid icon selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						i = 0;
						break;
					}

					if (numMoveTime.Value == 0)
					{
						i = _events.Insert(findNext(), new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
						_events[i].Variables[0] = _icon;
						_events[i].Variables[1] = _briefData[_icon].Waypoint[0];
						_events[i].Variables[2] = _briefData[_icon].Waypoint[1];
					}
					else
					{
						int total = (int)Math.Round(numMoveTime.Value * _timerInterval);
                        if (!hasAvailableEventSpace((2 + BaseBriefing.EventParameters.GetCount(_eventType)) * total))
                        {
                            MessageBox.Show("Not enough room in Event list for the full Move, aborting...", "Error");
                            cmdCancel_Click(0, new EventArgs());
                            return;
                        }

                        int t0 = hsbTimer.Value, x = _briefData[_icon].Waypoint[0], y = _briefData[_icon].Waypoint[1];
                        for (int j = 0; j <= total; j++)
						{
							i = _events.Insert(findNext(j + t0), new BaseBriefing.Event(_eventType) { Time = (short)(j + t0) });
							lstEvents.Items.Insert(i, "");
							_events[i].Variables[0] = _icon;
							_events[i].Variables[1] = (short)((x - _tempX) * j / total + _tempX);
							_events[i].Variables[2] = (short)((y - _tempY) * j / total + _tempY);
							updateList(i);
						}
					}
					break;
				case BaseBriefing.EventType.XwaRotateIcon:
					i = _events.Insert(findNext(), new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					lstEvents.Items.Insert(i, "");
					_events[i].Variables[0] = _icon;
					_events[i].Variables[1] = (short)cboRotateAmount.SelectedIndex;
					break;
				case BaseBriefing.EventType.XwaChangeRegion:
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i = _events.Insert(i - 10000, new BaseBriefing.Event(_eventType) { Time = (short)hsbTimer.Value });
						lstEvents.Items.Insert(i, "");
					}
					_events[i].Variables[0] = (short)(numNewRegion.Value - 1);
					break;
				default:    // this shouldn't be possible
					break;
			}
			lstEvents.SelectedIndex = i;
			updateList(i);
			onModified?.Invoke("EventAdd", new EventArgs());
			cmdCancel_Click("OK", new EventArgs());
		}
        void cmdMarker_Click(object sender, EventArgs e)
        {
			_eventType = BaseBriefing.EventType.SkipMarker;
			enableOkCancel(true);
        }
        void cmdMove_Click(object sender, EventArgs e)
		{
			lblTitle.Visible = false;
			lblCaption.Visible = false;
			hsbBRF.Maximum = 32768;
			hsbBRF.Minimum = -32767;
			hsbBRF.Value = _mapX;
			hsbBRF.Visible = true;
			vsbBRF.Maximum = 32768;
			vsbBRF.Minimum = -32767;
			vsbBRF.Value = _mapY;
			vsbBRF.Visible = true;
			_tempX = _mapX;
			_tempY = _mapY;
			_eventType = BaseBriefing.EventType.MoveMap;
			enableOkCancel(true);
		}
		void cmdMoveShip_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.XwaMoveIcon;
			enableOkCancel(true);
			pnlMove.Visible = true;
			lblTitle.Visible = false;
			lblCaption.Visible = false;
			lblInstruction.Visible = true;
			_tempX = -621;
			_tempY = -621;
			_icon = (short)cboMoveIcon.SelectedIndex;
		}
		void cmdNewShip_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.XwaSetIcon;
			enableOkCancel(true);
			pnlNew.Visible = true;
			lblTitle.Visible = false;
			lblCaption.Visible = false;
			lblInstruction.Visible = true;
			_tempX = -621;
			_tempY = -621;
			_icon = (short)cboNewIcon.SelectedIndex;
			_tempBD = _briefData[_icon];
		}
		void cmdRegion_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.XwaChangeRegion;
			enableOkCancel(true);
			pnlRegion.Visible = true;
		}
		void cmdRotate_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.XwaRotateIcon;
			enableOkCancel(true);
			pnlRotate.Visible = true;
			_icon = (short)cboRCraft.SelectedIndex;
			try { cboRotateAmount.SelectedIndex = _briefData[_icon].Waypoint[2]; }
			catch (NullReferenceException) { cboRotateAmount.SelectedIndex = 0; }
			_tempX = (short)cboRotateAmount.SelectedIndex;
		}
		void cmdShipInfo_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.XwaShipInfo;
			enableOkCancel(true);
			pnlShipInfo.Visible = true;
		}
		void cmdText_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.TextTag1;
			pnlTextTag.Visible = true;
			lblTitle.Visible = false;
			lblCaption.Visible = false;
			lblInstruction.Visible = true;
			_ttBackup = _textTags;
			_tempX = -621;
			_tempY = -621;
			enableOkCancel(true);
		}
		void cmdTitle_Click(object sender, EventArgs e)
		{
			cboText.Enabled = true;
			_eventType = BaseBriefing.EventType.TitleText;
			enableOkCancel(true);
		}
		void cmdZoom_Click(object sender, EventArgs e)
		{
			lblTitle.Visible = false;
			lblCaption.Visible = false;
			hsbBRF.Value = _zoomX;
			hsbBRF.Minimum = 1;
			hsbBRF.Maximum = 300;
			hsbBRF.Visible = true;
			vsbBRF.Value = _zoomY;
			vsbBRF.Minimum = 1;
			vsbBRF.Maximum = 300;
			vsbBRF.Visible = true;
			_tempX = _zoomX;
			_tempY = _zoomY;
			_eventType = BaseBriefing.EventType.ZoomMap;
			enableOkCancel(true);
		}

		void hsbBRF_ValueChanged(object sender, EventArgs e)
		{
			if (_eventType == BaseBriefing.EventType.MoveMap) _mapX = (short)hsbBRF.Value;
			if (_eventType == BaseBriefing.EventType.ZoomMap) _zoomX = (short)hsbBRF.Value;
			MapPaint();
		}

		void lblCaption_Click(object sender, EventArgs e)
		{
			int time = hsbTimer.Value;
			for (int i = 0; i < _maxEvents; i++)
			{
				if (_events[i].IsEndEvent)
				{
					hsbTimer.Value = 1;
					hsbTimer.Value = 0;
					return;
				}
				if (_events[i].Time > time && _events[i].Type == BaseBriefing.EventType.CaptionText)
				{
					if (_events[i].Time < hsbTimer.Maximum)
					{
						hsbTimer.Value = _events[i].Time;
						hsbTimer.Value = _events[i].Time + 1;
					}
					return;
				}
			}
		}

		void numText_ValueChanged(object sender, EventArgs e)
		{
			_textTags = _ttBackup;  // restore and re-edit
			_textTags[(int)numText.Value - 1].StringIndex = cboTextTag.SelectedIndex;
			_textTags[(int)numText.Value - 1].X = _tempX;
			_textTags[(int)numText.Value - 1].Y = _tempY;
			_textTags[(int)numText.Value - 1].ColorIndex = cboColorTag.SelectedIndex;
			MapPaint();
		}

		void pctBrief_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) return;
			
			if (e.Button == MouseButtons.Middle) _isMiddleDrag = false;
			popupPreviewStop();
		}
		void pctBrief_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button.ToString() != "Left")
			{
				if (e.Button == MouseButtons.Middle)
				{
					_isMiddleDrag = true;
					_popupMiddle.X = e.X;
					_popupMiddle.Y = e.Y;
				}
				popupPreviewStart();
				pctBrief_MouseMove(0, e); //Simulate mouse move to refresh and display the data
				return;
			}
			int w = pctBrief.Width;
			int h = pctBrief.Height;
			if (_eventType == BaseBriefing.EventType.TextTag1)
			{
				int mod = (_platform != Settings.Platform.TIE ? 2 : 1);
				_textTags = _ttBackup;  // restore backup before messing with it again
				_tempX = (short)(128 * e.X / _zoomX * mod - 64 * w / _zoomX * mod + _mapX);
				_tempY = (short)(128 * e.Y / _zoomY * mod - 64 * h / _zoomY * mod + _mapY);
				_textTags[(int)numText.Value - 1].StringIndex = cboTextTag.SelectedIndex;
				_textTags[(int)numText.Value - 1].X = _tempX;
				_textTags[(int)numText.Value - 1].Y = _tempY;
				_textTags[(int)numText.Value - 1].ColorIndex = cboColorTag.SelectedIndex;
				MapPaint();
			}
			else if (_eventType == BaseBriefing.EventType.XwaSetIcon)
			{
				_briefData[_icon].Waypoint = new short[4];
				_tempX = (short)(256 * e.X / _zoomX - 128 * w / _zoomX + _mapX);
				_tempY = (short)(256 * e.Y / _zoomY - 128 * h / _zoomY + _mapY);
				_briefData[_icon].Waypoint[0] = _tempX;
				_briefData[_icon].Waypoint[1] = _tempY;
				if (cboNCraft.SelectedIndex != 0) _briefData[_icon].Waypoint[3] = 1;
				else _briefData[_icon].Waypoint[3] = 0;
				MapPaint();
			}
			else if (_eventType == BaseBriefing.EventType.XwaMoveIcon)
			{
				try
				{
					if (_tempX == -621 && _tempY == -621)
					{
						_tempX = _briefData[_icon].Waypoint[0];
						_tempY = _briefData[_icon].Waypoint[1];
					}
					_briefData[_icon].Waypoint[0] = (short)(256 * e.X / _zoomX - 128 * w / _zoomX + _mapX);
					_briefData[_icon].Waypoint[1] = (short)(256 * e.Y / _zoomY - 128 * h / _zoomY + _mapY);
				}
				catch (NullReferenceException) { /* do nothing*/ }
				MapPaint();
			}
		}
		void pctBrief_MouseMove(object sender, MouseEventArgs e)
		{
			if (!_popupPreviewActive) return;
			
			int w = pctBrief.Width;
			int h = pctBrief.Height;
			if (_isMiddleDrag)
			{
				double scx = (w / _zoomX) * 0.75;
				double scy = (h / _zoomY) * 0.75;

				int ox = (int)((e.X - _popupMiddle.X) * scx);
				int oy = (int)((e.Y - _popupMiddle.Y) * scy);
				_mapX += (short)ox;
				_mapY += (short)oy;
				_popupMiddle.X = e.X;
				_popupMiddle.Y = e.Y;
				MapPaint();
			}
			int mod = (_platform != Settings.Platform.TIE ? 2 : 1);
			int xu = 128 * e.X / _zoomX * mod - 64 * w / _zoomX * mod + _mapX;
			int yu = 128 * e.Y / _zoomY * mod - 64 * h / _zoomY * mod + _mapY;
			double xkm = Math.Round((xu * 0.00625), 2);
			double ykm = Math.Round((-yu * 0.00625), 2);
			string s = "PREVIEW ONLY\nZoom: " + _zoomX + " , " + _zoomY;
			s += "\nMap Offset: " + _mapX + " , " + _mapY;
			s += "\nMap Coords: " + xu + " , " + yu;
			if (_platform != Settings.Platform.XWA) s += "\nWaypoint Coords (km): " + xkm.ToString() + " , " + ykm.ToString();
			popupUpdate(s);
			if (e.Delta > 0)
			{
				_zoomX += 2;
				_zoomY += 2;
				if (_zoomX > 128) _zoomX = 128;
				if (_zoomY > 128) _zoomY = 128;
				MapPaint();
			}
			else if (e.Delta < 0)
			{
				_zoomX -= 2;
				_zoomY -= 2;
				if (_zoomX < 1) _zoomX = 1;
				if (_zoomY < 1) _zoomY = 1;
				MapPaint();
			}
		}
		void pctBrief_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.DrawImage(_map, 0, 0, _map.Width, _map.Height);
		}

		void txtLength_TextChanged(object sender, EventArgs e)
		{
			short t_Length;
			switch (_platform)
			{
				case Settings.Platform.TIE:
					try
					{
						t_Length = (short)Math.Round(Convert.ToDecimal(txtLength.Text) * _timerInterval, 0);
						_tieBriefing.Length = t_Length;
						onModified?.Invoke("LengthChange", new EventArgs());
						hsbTimer.Maximum = _tieBriefing.Length + 11;
						if (Math.Round((decimal)_tieBriefing.Length / _timerInterval, 2) != Convert.ToDecimal(txtLength.Text))
							txtLength.Text = Convert.ToString(Math.Round((decimal)_tieBriefing.Length / _timerInterval, 2));
					}
					catch { txtLength.Text = Convert.ToString(Math.Round((decimal)_tieBriefing.Length / _timerInterval, 2)); }
					break;
				case Settings.Platform.XvT:
					try
					{
						t_Length = (short)Math.Round(Convert.ToDecimal(txtLength.Text) * _timerInterval, 0);
						_xvtBriefing.Length = t_Length;
						onModified?.Invoke("LengthChange", new EventArgs());
						hsbTimer.Maximum = _xvtBriefing.Length + 11;
						if (Math.Round((decimal)_xvtBriefing.Length / _timerInterval, 2) != Convert.ToDecimal(txtLength.Text))
							txtLength.Text = Convert.ToString(Math.Round((decimal)_xvtBriefing.Length / _timerInterval, 2));
					}
					catch { txtLength.Text = Convert.ToString(Math.Round((decimal)_xvtBriefing.Length / _timerInterval, 2)); }
					break;
				case Settings.Platform.XWA:
					try
					{
						t_Length = (short)Math.Round(Convert.ToDecimal(txtLength.Text) * _timerInterval, 0);
						_xwaBriefing.Length = t_Length;
						onModified?.Invoke("LengthChange", new EventArgs());
						hsbTimer.Maximum = _xwaBriefing.Length + 11;
						if (Math.Round((decimal)_xwaBriefing.Length / _timerInterval, 2) != Convert.ToDecimal(txtLength.Text))
							txtLength.Text = Convert.ToString(Math.Round((decimal)_xwaBriefing.Length / _timerInterval, 2));
					}
					catch { txtLength.Text = Convert.ToString(Math.Round((decimal)_xwaBriefing.Length / _timerInterval, 2)); }
					break;
			}
		}

		void vsbBRF_ValueChanged(object sender, EventArgs e)
		{
			if (_eventType == BaseBriefing.EventType.MoveMap) _mapY = (short)vsbBRF.Value;
			if (_eventType == BaseBriefing.EventType.ZoomMap) _zoomY = (short)vsbBRF.Value;
			MapPaint();
		}
		#endregion	tabDisplay
		#region tabStrings
		void loadStrings()
		{
			cboString.Items.Clear();
			cboText.Items.Clear();
			for (int i = 0; i < _strings.Length; i++)
			{
				cboString.Items.Add(_strings[i]);
				cboText.Items.Add(_strings[i]);
			}
		}
		void loadTags()
		{
			cboTag.Items.Clear();
			cboTextTag.Items.Clear();
			for (int i = 0; i < _tags.Length; i++)
			{
				cboTag.Items.Add(_tags[i]);
				cboTextTag.Items.Add(_tags[i]);
			}
		}

		void dataS_CurrentCellChanged(object sender, EventArgs e) { if (_platform == Settings.Platform.XWA) txtNotes.Text = _xwaBriefing.BriefingStringsNotes[dataS.CurrentCell.RowNumber]; }

		void tableStrings_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (!_loading && onModified != null) onModified("StringsChanged", new EventArgs());
			int i = 0;
			for (int j = 0; j < _strings.Length; j++)
				if (_tableStrings.Rows[j].Equals(e.Row))
				{
					i = j;
					break;
				}
			_strings[i] = _tableStrings.Rows[i][0].ToString();
			loadStrings();
		}
		void tableTags_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (!_loading && onModified != null) onModified("TagsChanged", new EventArgs());
			int i = 0;
			for (int j = 0; j < _tags.Length; j++)
				if (_tableTags.Rows[j].Equals(e.Row))
				{
					i = j;
					break;
				}
			_tags[i] = _tableTags.Rows[i][0].ToString();
			loadTags();
		}

		void txtNotes_Leave(object sender, EventArgs e)
		{
			if (_xwaBriefing.BriefingStringsNotes[dataS.CurrentCell.RowNumber] != txtNotes.Text && onModified != null) onModified("NotesMod", new EventArgs());
			_xwaBriefing.BriefingStringsNotes[dataS.CurrentCell.RowNumber] = txtNotes.Text;
		}
		#endregion	tabStrings
		#region tabEvents
		int convertEventToCboEventIndex(short eventValue) => cboEvent.Items[0].ToString() == "Skip Marker" ? (eventValue == 1 ? 0 : eventValue - 2) : eventValue - 3;
		short convertCboEventIndexToEvent(int index) => (short)(cboEvent.Items[0].ToString() == "Skip Marker" ? (index == 0 ? 1 : index + 2) : index + 3);
		/// <summary>Tally briefing events and make sure there's enough space in the raw briefing array.</summary>
		bool hasAvailableEventSpace(int requestedParams) => _baseBrf.EventsLength + requestedParams < _maxEvents * 2;
		void newEventAtCurrent()
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1) i = 0;
			lstEvents.Items.Insert(i, "");
			i = _events.Insert(i, new BaseBriefing.Event(BaseBriefing.EventType.PageBreak));
			lstEvents.SelectedIndex = i;
			onModified?.Invoke("EventAdd", new EventArgs());
		}
		/// <summary>Swaps one briefing event index with another.</summary>
		void swapEvent(int index1, int index2)
		{
			var t = _events[index1];
			_events[index1] = _events[index2];
			_events[index2] = t;
			onModified?.Invoke("SwapEvent", new EventArgs());
		}
		/// <summary>Shifts briefing events by swapping the contents of the origin index in a linear path until it occupies the end index.</summary>
		void shiftEvents(int origin, int end)
		{
			if (end > origin) for (int i = origin; i < end; i++) swapEvent(i, i + 1); //swap downward
			else if (end < origin) for (int i = origin; i > end; i--) swapEvent(i, i - 1); //swap upward
		}
		void updateList(int index)
		{
			if (index == -1) return;

			string temp = string.Format("{0,-8:0.00}", (decimal)_events[index].Time / _timerInterval);
			temp += cboEvent.Items[convertEventToCboEventIndex((short)_events[index].Type)].ToString();
			if (_events[index].Type == BaseBriefing.EventType.TitleText || _events[index].Type == BaseBriefing.EventType.CaptionText && _events[index].Variables[0] != -1)
			{
				if (_strings[_events[index].Variables[0]].Length > 30) temp += ": \"" + _strings[_events[index].Variables[0]].Substring(0, 30) + "...\"";
				else temp += ": \"" + _strings[_events[index].Variables[0]] + '\"';
			}
			else if (_events[index].Type == BaseBriefing.EventType.MoveMap || _events[index].Type == BaseBriefing.EventType.ZoomMap) { temp += ": X:" + _events[index].Variables[0] + " Y:" + _events[index].Variables[1]; }
			else if (_events[index].IsFGTag)
			{
				//[JB] This fixed a potential crash while I working on functionality to delete FGs.  Not sure if still needed.
				int fgIndex = _events[index].Variables[0];
				if (fgIndex >= _briefData.Length)
				{
					fgIndex = 0;
					_events[index].Variables[0] = 0;
				}
				temp += ": " + ((_platform != Settings.Platform.XWA) ? _briefData[fgIndex].Name : "Icon #" + fgIndex);
			}
			else if (_events[index].IsTextTag)
			{
				if (_tags[_events[index].Variables[0]].Length > 30) temp += ": \"" + _tags[_events[index].Variables[0]].Substring(0, 30) + "...\"";
				else temp += ": \"" + _tags[_events[index].Variables[0]] + '\"';
			}
			else if (_events[index].Type == BaseBriefing.EventType.XwaSetIcon) { temp += " #" + _events[index].Variables[0] + ": Craft: " + Platform.Xwa.Strings.CraftType[_events[index].Variables[1]] + " IFF: " + cboIFF.Items[_events[index].Variables[2]].ToString(); }
			else if (_events[index].Type == BaseBriefing.EventType.XwaShipInfo)
			{
				if (_events[index].Variables[0] == 1) temp += ": Icon # " + _events[index].Variables[1] + " State: On";
				else temp += ": Icon # " + _events[index].Variables[1] + " State: Off";
			}
			else if (_events[index].Type == BaseBriefing.EventType.XwaMoveIcon) { temp += " #" + _events[index].Variables[0] + ": X:" + _events[index].Variables[1] + " Y:" + _events[index].Variables[2]; }
			else if (_events[index].Type == BaseBriefing.EventType.XwaRotateIcon) { temp += " #" + _events[index].Variables[0] + ": " + cboRotate.Items[_events[index].Variables[1]]; }
			else if (_events[index].Type == BaseBriefing.EventType.XwaChangeRegion) { temp += " #" + (_events[index].Variables[0] + 1); }
			lstEvents.Items[index] = temp;
			if (!_loading)
			{
				_loading = true;
				lstEvents.SelectedIndex = index;
				_loading = false;
				MapPaint();
			}
		}
		void updateParameters()
		{
			Control currentControl = ActiveControl;
			int i = lstEvents.SelectedIndex;
			_loading = true;
			cboIFF.Enabled = false;
			cboString.Enabled = false;
			cboTag.Enabled = false;
			cboFG.Enabled = false;
			cboColor.Enabled = false;
			numX.Enabled = false;
			numY.Enabled = false;
			optOff.Enabled = false;
			optOn.Enabled = false;
			numRegion.Enabled = false;
			cboCraft.Enabled = false;
			cboRotate.Enabled = false;
			if (_events[i].Type == BaseBriefing.EventType.TitleText || _events[i].Type == BaseBriefing.EventType.CaptionText)
			{
				try { cboString.SelectedIndex = _events[i].Variables[0]; }
				catch
				{
					cboString.SelectedIndex = 0;
					_events[i].Variables[0] = 0;
				}
				cboString.Enabled = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.MoveMap || _events[i].Type == BaseBriefing.EventType.ZoomMap)
			{
				numX.Value = _events[i].Variables[0];
				numY.Value = _events[i].Variables[1];
				numX.Enabled = true;
				numY.Enabled = true;
			}
			else if (_events[i].IsFGTag)
			{
				try { cboFG.SelectedIndex = _events[i].Variables[0]; }
				catch
				{
					cboFG.SelectedIndex = 0;
					_events[i].Variables[0] = 0;
				}
				cboFG.Enabled = true;
			}
			else if (_events[i].IsTextTag)
			{
				try
				{
					cboTag.SelectedIndex = _events[i].Variables[0];
					cboColor.SelectedIndex = _events[i].Variables[3];
				}
				catch
				{
					cboTag.SelectedIndex = 0;
					cboColor.SelectedIndex = 0;
					_events[i].Variables[0] = 0;
					_events[i].Variables[1] = 0;
					_events[i].Variables[2] = 0;
					_events[i].Variables[3] = 0;
				}
				numX.Value = _events[i].Variables[1];
				numY.Value = _events[i].Variables[2];
				cboTag.Enabled = true;
				cboColor.Enabled = true;
				numX.Enabled = true;
				numY.Enabled = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaSetIcon)
			{
				cboFG.SelectedIndex = _events[i].Variables[0];
				cboCraft.SelectedIndex = _events[i].Variables[1];
				cboIFF.SelectedIndex = _events[i].Variables[2];
				cboCraft.Enabled = true;
				cboIFF.Enabled = true;
				cboFG.Enabled = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaShipInfo)
			{
				optOn.Checked = Convert.ToBoolean(_events[i].Variables[0]);
				optOff.Checked = !optOn.Checked;
				cboFG.SelectedIndex = _events[i].Variables[1];
				cboFG.Enabled = true;
				optOff.Enabled = true;
				optOn.Enabled = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaMoveIcon)
			{
				cboFG.SelectedIndex = _events[i].Variables[0];
				numX.Value = _events[i].Variables[1];
				numY.Value = _events[i].Variables[2];
				cboFG.Enabled = true;
				numX.Enabled = true;
				numY.Enabled = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaRotateIcon)
			{
				cboFG.SelectedIndex = _events[i].Variables[0];
				cboRotate.SelectedIndex = _events[i].Variables[1];
				cboFG.Enabled = true;
				cboRotate.Enabled = true;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaChangeRegion)
			{
				numRegion.Value = _events[i].Variables[0] + 1;
				numRegion.Enabled = true;
			}
			_loading = false;
			ActiveControl = currentControl;
		}

		void cboColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].IsTextTag)
			{
				_events[i].Variables[3] = (short)cboColor.SelectedIndex;
				onModified?.Invoke("TagColor", new EventArgs());
			}
			updateList(i);
		}
		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == BaseBriefing.EventType.XwaSetIcon)
			{
				_events[i].Variables[1] = (short)cboCraft.SelectedIndex;
				onModified?.Invoke("NewIcon", new EventArgs());
			}
			updateList(i);
		}
		void cboEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (_loading || i == -1 || cboEvent.SelectedIndex == -1) return;

			int oldEventSize = 2 + BaseBriefing.EventParameters.GetCount(_events[i].Type);
			int newEventSize = 2 + BaseBriefing.EventParameters.GetCount(convertCboEventIndexToEvent(cboEvent.SelectedIndex));
			if (!hasAvailableEventSpace(newEventSize - oldEventSize))
			{
				MessageBox.Show("Cannot change Event Type because the briefing list is full and the replaced event needs more space than is available.", "Error");
				return;
			}
			onModified?.Invoke("ChangeEvent", new EventArgs());
			_events[i].Type = (BaseBriefing.EventType)convertCboEventIndexToEvent(cboEvent.SelectedIndex);
			updateParameters();
			updateList(i);
		}
		void cboFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].IsFGTag || _events[i].Type == BaseBriefing.EventType.XwaSetIcon
				|| _events[i].Type == BaseBriefing.EventType.XwaMoveIcon || _events[i].Type == BaseBriefing.EventType.XwaRotateIcon)
			{
				onModified?.Invoke("ChangeFG", new EventArgs());
				_events[i].Variables[0] = (short)cboFG.SelectedIndex;
			}
			else if (_events[i].Type == BaseBriefing.EventType.XwaShipInfo)
			{
				onModified?.Invoke("ChangeFG", new EventArgs());
				_events[i].Variables[1] = (short)cboFG.SelectedIndex;
			}
			updateList(i);
		}
		void cboIFF_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == BaseBriefing.EventType.XwaSetIcon)
			{
				onModified?.Invoke("ChangeIFF", new EventArgs());
				_events[i].Variables[2] = (short)cboIFF.SelectedIndex;
			}
			updateList(i);
		}
		void cboRotate_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == BaseBriefing.EventType.XwaRotateIcon)
			{
				onModified?.Invoke("RotateIcon", new EventArgs());
				_events[i].Variables[1] = (short)cboRotate.SelectedIndex;
			}
			updateList(i);
		}
		void cboString_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == BaseBriefing.EventType.TitleText || _events[i].Type == BaseBriefing.EventType.CaptionText)
			{
				onModified?.Invoke("ChangeString", new EventArgs());
				_events[i].Variables[0] = (short)cboString.SelectedIndex;
			}
			updateList(i);
		}
		void cboTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].IsTextTag)
			{
				onModified?.Invoke("ChangeTag", new EventArgs());
				_events[i].Variables[0] = (short)cboTag.SelectedIndex;
			}
			updateList(i);
		}

		void cmdDelete_Click(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1) return;

			lstEvents.Items.RemoveAt(i);
			_events.RemoveAt(i);
			onModified?.Invoke("EventDelete", new EventArgs());
			try { lstEvents.SelectedIndex = i; }
			catch { lstEvents.SelectedIndex = i - 1; }
		}
		void cmdDown_Click(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == lstEvents.Items.Count - 1 || i == -1) return;

			swapEvent(i + 1, i);
			string item = lstEvents.Items[i].ToString();
			lstEvents.Items[i] = lstEvents.Items[i + 1];
			lstEvents.Items[i + 1] = item;
			lstEvents.SelectedIndex = i + 1;
			onModified?.Invoke("EventDown", new EventArgs());
		}
		void cmdNew_Click(object sender, EventArgs e)
		{
			if (hasAvailableEventSpace(6) == false)
			{
				MessageBox.Show("Event list is full, cannot add more.", "Error");
				return;
			}
			newEventAtCurrent();
			if (lstEvents.SelectedIndex + 1 < lstEvents.Items.Count)
			{
				int index = lstEvents.SelectedIndex;
				swapEvent(index, index + 1);
				updateList(index + 1);   //updateList() changes lstEvents.SelectedIndex but doesn't seem to refresh the parameters controls with the selected item.  Update in reverse order so that a forced refresh will succeed.
				updateList(index);
				lstEvents.SelectedIndex = index + 1;
			}
			else updateList(lstEvents.SelectedIndex);
		}
		void cmdUp_Click(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1) return;

			swapEvent(i - 1, i);
			string item = lstEvents.Items[i].ToString();
			lstEvents.Items[i] = lstEvents.Items[i - 1];
			lstEvents.Items[i - 1] = item;
			lstEvents.SelectedIndex = i - 1;
			onModified?.Invoke("EventUp", new EventArgs());
		}

		void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			_loading = true;
			numTime.Value = _events[i].Time;
			cboEvent.SelectedIndex = convertEventToCboEventIndex((short)_events[i].Type);
			_loading = false;
			updateParameters();
			try { cmdUp.Enabled = (_events[i - 1].Time == _events[i].Time); }
			catch { cmdUp.Enabled = false; }
			try { cmdDown.Enabled = (_events[i + 1].Time == _events[i].Time); }
			catch { cmdDown.Enabled = false; }
		}

		void numRegion_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == BaseBriefing.EventType.XwaChangeRegion)
			{
				onModified?.Invoke("ChangeRegion", new EventArgs());
				_events[i].Variables[0] = (short)(numRegion.Value - 1);
			}
			updateList(i);
		}
		void numTime_ValueChanged(object sender, EventArgs e)
		{
			lblEventTime.Text = string.Format("{0:= 0.00 seconds}", numTime.Value / _timerInterval);
			int size = lstEvents.Items.Count;
			int index = lstEvents.SelectedIndex;
			if (_loading || index == -1) return;

			_loading = true;
			onModified?.Invoke("ChangeTime", new EventArgs());
			short diff = (short)(numTime.Value - _events[index].Time);
			_events[index].Time = (short)numTime.Value;

			if (chkShift.Checked)
			{
				updateList(index);
				for (int i = index + 1; i < lstEvents.Items.Count; i++)
				{
					_events[i].Time += diff;
					updateList(i);
				}
			}
			else
			{
				int p = index;
				if (diff > 0)  //Positive change, moving down in the list
				{
					if (index < size - 1 && _events[index].Time != _events[index + 1].Time)
					{
						p = index + 1;
						while (p < size && _events[p].Time< _events[index].Time) p++;  //Search until a greater time index is found
						p--;  //If found, insert before.  If not found (p=size) adjusts to last slot in array.
					}
				}
				else if (diff < 0)  //Negative change, moving up in the list
				{
					if (index > 0 && _events[index].Time != _events[index - 1].Time)
					{
						p = index - 1;
						while (p >= 0 && _events[p].Time > _events[index].Time) p--;   //Search until a lesser time index is found
						p++;  //If found, insert after.  If not found (p=-1) adjusts to first slot in array.
					}
				}
				if (diff != 0)
				{
					shiftEvents(index, p);
					lstEvents.Items.RemoveAt(index);
					lstEvents.Items.Insert(p, "");
					lstEvents.SelectedIndex = p;
					updateList(p);
				}
			}

			_loading = false;
			try { cmdUp.Enabled = (_events[index - 1].Time == _events[index].Time); }
			catch { cmdUp.Enabled = false; }
			try { cmdDown.Enabled = (_events[index + 1].Time == _events[index].Time); }
			catch { cmdDown.Enabled = false; }
		}
		void numX_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == BaseBriefing.EventType.MoveMap || _events[i].Type == BaseBriefing.EventType.ZoomMap) _events[i].Variables[0] = (short)numX.Value;
			else if (_events[i].IsTextTag || _events[i].Type == BaseBriefing.EventType.XwaMoveIcon) _events[i].Variables[1] = (short)numX.Value;
			onModified?.Invoke("ChangeX", new EventArgs());
			updateList(i);
		}
		void numY_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == BaseBriefing.EventType.MoveMap || _events[i].Type == BaseBriefing.EventType.ZoomMap) _events[i].Variables[1] = (short)numY.Value;
			else if (_events[i].IsTextTag || _events[i].Type == BaseBriefing.EventType.XwaMoveIcon) _events[i].Variables[2] = (short)numY.Value;
			onModified?.Invoke("ChangeY", new EventArgs());
			updateList(i);
		}

		void optOn_CheckedChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == BaseBriefing.EventType.XwaShipInfo)
			{
				onModified?.Invoke("ToggleInfo", new EventArgs());
				_events[i].Variables[0] = (short)(optOn.Checked ? 1 : 0);
			}
			updateList(i);
		}
		#endregion tabEvents
		#region tabTeams
		/// <summary>Performs the necessary data saving and swapping when selecting a new Briefing.</summary>
		void changeBriefingIndex(int briefIndex)
		{
			if (_loading || briefIndex == _currentCollectionIndex) return;

			if (briefIndex > cboBriefIndex1.Items.Count) briefIndex = cboBriefIndex1.Items.Count - briefIndex;
			Save();  //Save current changes before switching
			_currentCollectionIndex = briefIndex;
			if (_platform == Settings.Platform.XvT)
			{
				_xvtBriefing = _xvtBriefingCollection[briefIndex];
				//These select lines copied from the constructor to re-load and re-init particular data sets and controls.
				_events.Clear();
				_tags = _xvtBriefing.BriefingTag;
				_strings = _xvtBriefing.BriefingString;
				importStrings();
				txtLength.Text = Convert.ToString(Math.Round((decimal)_xvtBriefing.Length / _timerInterval, 2));
				lstEvents.Items.Clear();
				importEvents(_xvtBriefing.Events);
				numTile.Value = _xvtBriefing.Tile;
				cboText.SelectedIndex = 0;
				cboFGTag.SelectedIndex = 0;
				cboTextTag.SelectedIndex = 0;
				cboColorTag.SelectedIndex = 0;
			}
			else if (_platform == Settings.Platform.XWA)
			{
				_xwaBriefing = _xwaBriefingCollection[briefIndex];
				//These select lines copied from the constructor to re-load and re-init particular data sets and controls.
				_events.Clear();
				_tags = _xwaBriefing.BriefingTag;
				_strings = _xwaBriefing.BriefingString;
				importStrings();
				txtLength.Text = Convert.ToString(Math.Round((decimal)_xwaBriefing.Length / _timerInterval, 2));
				lstEvents.Items.Clear();
				importEvents(_xwaBriefing.Events);
				numTile.Value = _xwaBriefing.Tile;
				txtNotes.Enabled = true;
				txtNotes.Text = _xwaBriefing.BriefingStringsNotes[0];
				cboText.SelectedIndex = 0;
				cboFGTag.SelectedIndex = 0;
				cboTextTag.SelectedIndex = 0;
				cboColorTag.SelectedIndex = 0;
				cboInfoCraft.SelectedIndex = 0;
				cboRCraft.SelectedIndex = 0;
				cboRotateAmount.SelectedIndex = 0;
				cboMoveIcon.SelectedIndex = 0;
				cboNewIcon.SelectedIndex = 0;
				cboNCraft.SelectedIndex = 0;
				cboIconIff.SelectedIndex = 0;
			}

			if (lstEvents.Items.Count > 0) lstEvents.SelectedIndex = 0;
			if (tabBrief.SelectedIndex == 0) //Only update the map if the tab is visible.
			{
				hsbTimer.Value = 1;
				hsbTimer.Value = 0;  //Need both changes to force the map to update
			}
			updateTitle();
			refreshTeamList();
		}
		void refreshTeamList()
		{
			if (_platform == Settings.Platform.TIE) return;

			bool temp = _loading;
			_loading = true;
			lstTeams.SelectedIndex = -1;
			for (int i = 0; i < 10; i++)
			{
				bool team = (_platform == Settings.Platform.XvT) ? _xvtBriefing.Team[i] : _xwaBriefing.Team[i];
				if (team) lstTeams.SelectedIndex = i;
			}
			_loading = temp;
		}
		/// <summary>For multi-briefing platforms (XvT and XWA) this updates the BriefingForm title with the currently selected briefing and applicable teams.</summary>
		/// <remarks>The title is a convenient location to display this information regardless of which tab is selected.</remarks>
		void updateTitle()
		{
			if (_platform == Settings.Platform.TIE) return;

			int teamIndex = cboBriefIndex1.SelectedIndex;
			if (teamIndex < 0) teamIndex = 0;
			string title = Text;
			string update = "Briefing #" + (teamIndex + 1);
			string team = "";
			int pos = title.IndexOf("   [");
			if (pos >= 0) title = title.Remove(pos);

			if (_platform == Settings.Platform.XvT)
			{
				for (int i = 0; i < _xvtBriefing.Team.Length; i++)
					if (_xvtBriefing.Team[i])
					{
						if (team.Length > 0) team += ", ";
						team += SharedTeamNames[i];
					}
			}
			else if (_platform == Settings.Platform.XWA)
			{
				for (int i = 0; i < _xwaBriefing.Team.Length; i++)
					if (_xwaBriefing.Team[i])
					{
						if (team.Length > 0) team += ", ";
						team += SharedTeamNames[i];
					}
			}
			if (team == "") team = "NO TEAMS";

			title += "   [" + update + " for: " + team + "]";
			Text = title;
		}

		/// <summary>Switches currently visible team briefing.</summary>
		void cboBriefIndex1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_loading = true;
			cboBriefIndex2.SelectedIndex = cboBriefIndex1.SelectedIndex;
			_loading = false;
			changeBriefingIndex(cboBriefIndex1.SelectedIndex);
		}

        void cboBriefIndex2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			_loading = true;
			cboBriefIndex1.SelectedIndex = cboBriefIndex2.SelectedIndex;
			_loading = false;
			changeBriefingIndex(cboBriefIndex2.SelectedIndex);
		}

		void lstTeams_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			int index = cboBriefIndex1.SelectedIndex;
			if (index == -1 || _platform == Settings.Platform.TIE) return;

			bool[] teams = (_platform == Settings.Platform.XvT ? _xvtBriefing.Team : _xwaBriefing.Team);
			ListBox.SelectedIndexCollection sel = lstTeams.SelectedIndices;
			for (int i = 0; i < 10; i++)
			{
				bool found = false;
				for (int k = 0; k < sel.Count; k++)
					if (sel[k] == i)
					{
						teams[i] = true;
						found = true;

						//Each team can only see one briefing, so iterate through the other briefings and deselect them.
						if (_platform == Settings.Platform.XvT) for (int j = 0; j < _xvtBriefingCollection.Count; j++) if (j != index) _xvtBriefingCollection[j].Team[i] = false;
						if (_platform == Settings.Platform.XWA) for (int j = 0; j < _xwaBriefingCollection.Count; j++) if (j != index) _xwaBriefingCollection[j].Team[i] = false;
						break;
					}
				if (!found) teams[i] = false;
			}
			onModified?.Invoke("ChangeTeams", new EventArgs());
			updateTitle();
		}
		#endregion tabTeams

		BaseBriefing _baseBrf => (_platform == Settings.Platform.TIE ? _tieBriefing : (_platform == Settings.Platform.XvT ? _xvtBriefing : (BaseBriefing)_xwaBriefing));
	}

	/// <summary>Represents details for a single FG/Icon.</summary>
	public struct BriefData
	{
		/// <summary>Gets or sets the craft type.</summary>
		public int Craft;
		/// <summary>Gets or sets the values to make up a Waypoint.</summary>
		public short[] Waypoint;
		/// <summary>Gets or sets the waypoints.</summary>
		public BaseFlightGroup.Waypoint[] WaypointArr;
		/// <summary>Gets or sets the IFF.</summary>
		public byte IFF;
		/// <summary>Gets or sets the FG Name.</summary>
		public string Name;
	}

	/// <summary>Represents details for a single FG/Icon tag.</summary>
	struct ShipTag
	{
		/// <summary>The FlightGroup or Icon index.</summary>
		public short Slot;
		/// <summary>The event time the tag was applied.</summary>
		public short StartTime;
	}

	/// <summary>Represents details for a single Text tag.</summary>
	struct TextTag
	{
		/// <summary>The string index to display.</summary>
		public int StringIndex;
		/// <summary>Map X value.</summary>
		public short X;
		/// <summary>Map Y value.</summary>
		public short Y;
		/// <summary>Color value.</summary>
		public int ColorIndex;
	}

	/*
	 * Okay, here's how the BRF.dat files work:
	 * first SHORT is number of entries
	 * header section is SHORT offset values to craft images,
	 * lets me define multiple craft to the same image.
	 * at the image locations:
	 * BYTE width, BYTE height
	 * BITFIELD; bottom 4 are left px, top 4 are right px, always in pairs even for odd sizes
	 * reads left to right, top to bottom

	NOTE: the DAT files were deprecated to just use BMPs, but had to keep XvT since it sizes the FG tags according to size, not a fixed box
	 */
}