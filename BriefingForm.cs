/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2018 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.5
 */

/* CHANGELOG
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

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Idmr.Common;
using Idmr.Platform;

namespace Idmr.Yogeme
{
	/// <summary>The briefing forms for YOGEME, one form for all platforms</summary>
	public partial class BriefingForm : Form
	{
		#region Vars
		BriefData[] _briefData;
		BriefData _tempBD;
        Platform.Xvt.BriefingCollection _xvtBriefingCollection;
        Platform.Xwa.BriefingCollection _xwaBriefingCollection;
        int _currentCollectionIndex;
		Platform.Tie.Briefing _tieBriefing;
		Platform.Xvt.Briefing _xvtBriefing;
		Platform.Xwa.Briefing _xwaBriefing;
		bool _loading = false;
		Color _normalColor;
		Color _highlightColor;
		Color _titleColor;
		short[,] _events;	// this will contain the event listing for use, raw data is in Briefing.Events[]
		short _zoomX = 48;
		short _zoomY;
		int w, h;
		short _mapX, _mapY;	// mapX and mapY will be different, namely the grid coordinates of the center, like how TIE handles it
		Bitmap _map;
		int[,] _fgTags = new int[8, 2];	// [#, 0=FG/Icon 1=Time]
		int[,] _textTags = new int[8, 4];	// [#, X, Y, color]
		DataTable _tableTags = new DataTable("Tags");
		DataTable _tableStrings = new DataTable("Strings");
		int _timerInterval;
		BaseBriefing.EventType _eventType = BaseBriefing.EventType.None;
		short _tempX, _tempY;
		int[,] _tempTags;
		Settings.Platform _platform;
		string[] _tags;
		string[] _strings;
		int _maxEvents;
		string _message = "";
		int _regionDelay = -1;
		int _page = 1;
		short _icon = 0;
        bool _popupPreviewActive = false;     //[JB] The popup feature allows the user to move and zoom the map without changing any events, as well as see the coordinates of the mouse cursor.  Designed to assist raw editing of the event list.
        Point _popupPreviewZoom;
		Point _popupPreviewMap;
        bool _isMiddleDrag = false;
        Point _popupMiddle;
        bool _mapPaintScheduled = false;      //True if a paint is scheduled, that is a paint request is called while a paint is already in progress.
        static public string[] sharedTeamNames = new string[10]; //[JB] The other platforms need a way to communicate the team names to the briefing
        int _previousTimeIndex = 0;           //Tracks the previous time index of the briefing so we can detect when the user is manually scrolling through arbitrary times.
        #endregion

		public BriefingForm(Platform.Tie.FlightGroupCollection fg, Platform.Tie.Briefing briefing)
		{
			_loading = true;
			_platform = Settings.Platform.TIE;
			_titleColor = Color.FromArgb(0xFC, 0xFC, 0x54);
			_normalColor = Color.FromArgb(0xFC, 0xFC, 0xFC);
			_highlightColor = Color.FromArgb(0x00, 0xA8, 0x00);
			_zoomY = _zoomX;			// in most cases, these will remain the same
			_tieBriefing = briefing;
			_maxEvents = Platform.Tie.Briefing.EventQuantityLimit;
			_events = new short[_maxEvents,6];
			InitializeComponent();
			Text = "YOGEME Briefing Editor - TIE";
			#region layout edit
			// final layout update, as in VS it's spread out
			Height = 426;
			Width = 760;
			tabBrief.Width = 752;
			Point loc = new Point(608, 188);
			pnlShipTag.Location = loc;
			pnlTextTag.Location = loc;
			#endregion
			Import(fg);	// FGs are separate so they can be updated without running the BRF as well
			importDat(Application.StartupPath + "\\images\\TIE_BRF.dat", 34);
			_tags = _tieBriefing.BriefingTag;
			_strings = _tieBriefing.BriefingString;
			importStrings();
			_timerInterval = Platform.Tie.Briefing.TicksPerSecond;
			txtLength.Text = Convert.ToString(Math.Round((decimal)_tieBriefing.Length / _timerInterval, 2));
			hsbTimer.Maximum = _tieBriefing.Length + 11;
			w = pctBrief.Width;
			h = pctBrief.Height;
			_mapX = 0;
			_mapY = 0;
			lstEvents.Items.Clear();
			importEvents(_tieBriefing.Events);
            hsbTimer.Value = 0;
			numUnk1.Value = _tieBriefing.Unknown1;
			numUnk3.Enabled = false;
			cboText.SelectedIndex = 0;
			cboFGTag.SelectedIndex = 0;
			cboTextTag.SelectedIndex = 0;
			cboColorTag.SelectedIndex = 0;

            labBriefIndex2.Visible = false;
            cboBriefIndex2.Visible = false;
            tabTeams.Enabled = false;

            _loading = false;
            postLoadInit();
        }
		public BriefingForm(Platform.Xvt.FlightGroupCollection fg, Platform.Xvt.BriefingCollection briefing)
		{
			_loading = true;
			_platform = Settings.Platform.XvT;
			_titleColor = Color.FromArgb(0xFC, 0xFC, 0x00);
			_normalColor = Color.FromArgb(0xF8, 0xFC, 0xF8);
			_highlightColor = Color.FromArgb(0x40, 0xC4, 0x40);
			_zoomY = _zoomX;
            _xvtBriefingCollection = briefing; //[JB] Added
            _currentCollectionIndex = 0;
			_xvtBriefing = briefing[0];
			_maxEvents = Platform.Xvt.Briefing.EventQuantityLimit;
			_events = new short[_maxEvents, 6];
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
			lblCaption.Font = new Font("Times New Roman", 8F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			lblCaption.Size = new Size(360, 28);
			lblCaption.Location = new Point(150, 254);
			lblTitle.BackColor = Color.FromArgb(0x10, 0x10, 0x20);
			lblTitle.Font = new Font("Times New Roman", 8F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
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
			#endregion
			importDat(Application.StartupPath + "\\images\\XvT_BRF.dat", 22);
			_tags = _xvtBriefing.BriefingTag;
			_strings = _xvtBriefing.BriefingString;
			importStrings();
			_timerInterval = Platform.Xvt.Briefing.TicksPerSecond;
			txtLength.Text = Convert.ToString(Math.Round(((decimal)_xvtBriefing.Length / _timerInterval), 2));
			hsbTimer.Maximum = _xvtBriefing.Length + 11;
			w = pctBrief.Width;
			h = pctBrief.Height;
			_mapX = 0;
			_mapY = 0;
			lstEvents.Items.Clear();
			importEvents(_xvtBriefing.Events);
			hsbTimer.Value = 0;
			numUnk1.Value = _xvtBriefing.Unknown1;
			numUnk3.Value = _xvtBriefing.Unknown3;
			cboText.SelectedIndex = 0;
			cboFGTag.SelectedIndex = 0;
			cboTextTag.SelectedIndex = 0;
			cboColorTag.SelectedIndex = 0;

            //[JB] Set values for team selection dropdown
            for (int i = 0; i < _xvtBriefingCollection.Count; i++)  //XvT has 8 briefings
            {
                string s = "Briefing " + (i + 1);
                cboBriefIndex1.Items.Add(s);
                cboBriefIndex2.Items.Add(s);
            }
            for(int i = 0; i < 10; i++)  //XvT has 10 teams
                lstTeams.Items.Add((i + 1) + ": " + sharedTeamNames[i]);
            cboBriefIndex1.SelectedIndex = 0;
            cboBriefIndex2.SelectedIndex = 0;
            refreshTeamList();
            updateTitle();

    		_loading = false;
            postLoadInit();
        }
		public BriefingForm(Platform.Xwa.BriefingCollection briefing)
		{
			_loading = true;
			_platform = Settings.Platform.XWA;
			_titleColor = Color.FromArgb(0x63, 0x82, 0xFF);
			_normalColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
			_highlightColor = _titleColor;
			_zoomX = 32;
			_zoomY = _zoomX;
            _xwaBriefingCollection = briefing;
            _currentCollectionIndex = 0; 
            _xwaBriefing = briefing[0];
			_maxEvents = Platform.Xwa.Briefing.EventQuantityLimit;
			_events = new short[_maxEvents, 6];
			InitializeComponent();
			Text = "YOGEME Briefing Editor - XWA";
			#region XWA layout change
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
			w = pctBrief.Width;
			h = pctBrief.Height;
			lblTitle.BackColor = Color.FromArgb(0x18, 0x18, 0x18);
			lblTitle.Size = new Size(510, 28);
			lblTitle.Left += 36;
			lblTitle.Top -= 4;
			lblTitle.TextAlign = ContentAlignment.TopCenter;
			lblTitle.ForeColor = _titleColor;
			lblTitle.Text = "*Defined in .LST file*";
			lblTitle.Font = new Font("Arial", 10F, FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			cmdTitle.Enabled = false;
			lblCaption.BackColor = Color.FromArgb(0x20, 0x30, 0x88);
			lblCaption.Font = new Font("Arial", 8F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
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
			cboEvent.Items.Add("New Icon");
			cboEvent.Items.Add("Show Ship Data");
			cboEvent.Items.Add("Move Icon");
			cboEvent.Items.Add("Rotate Icon");
			cboEvent.Items.Add("Switch to Region");
			cboCraft.Items.AddRange(Platform.Xwa.Strings.CraftType);
			cboNCraft.Items.AddRange(Platform.Xwa.Strings.CraftType);
			#endregion
			importDat(Application.StartupPath + "\\images\\XWA_BRF.dat", 56);
			_tags = _xwaBriefing.BriefingTag;
			_strings = _xwaBriefing.BriefingString;
			importStrings();
			_timerInterval = Platform.Xwa.Briefing.TicksPerSecond;
			txtLength.Text = Convert.ToString(Math.Round(((decimal)_xwaBriefing.Length / _timerInterval), 2));
			hsbTimer.Maximum = _xwaBriefing.Length + 11;
			w = pctBrief.Width;
			h = pctBrief.Height;
			_mapX = 0;
			_mapY = 0;
			lstEvents.Items.Clear();
            _briefData = new BriefData[100];	// this way I don't have to deal with expanding the array
			string[] names = new string[100];
			for (int i=0;i<_briefData.Length;i++) names[i] = "Icon #" + i;
			cboFG.Items.AddRange(names);
			cboFGTag.Items.AddRange(names);
			cboInfoCraft.Items.AddRange(names);
			cboRCraft.Items.AddRange(names);
			cboMoveIcon.Items.AddRange(names);
			cboNewIcon.Items.AddRange(names);
			importEvents(_xwaBriefing.Events);
			hsbTimer.Value = 0;
			numUnk1.Value = _xwaBriefing.Unknown1;
			numUnk3.Enabled = false;
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

            //[JB] Set values for team selection dropdown
            for (int i = 0; i < _xwaBriefingCollection.Count; i++)  //XvT has 8 briefings
            {
                string s = "Briefing " + (i + 1);
                cboBriefIndex1.Items.Add(s);
                cboBriefIndex2.Items.Add(s);
            }
            for(int i = 0; i < 10; i++)  //XWA has 10 teams
                lstTeams.Items.Add((i + 1) + ": " + sharedTeamNames[i]);
            cboBriefIndex1.SelectedIndex = 0;
            cboBriefIndex2.SelectedIndex = 0;
            refreshTeamList();
            updateTitle();

            _loading = false;
            postLoadInit();
		}

        /// <summary>Handles redundant code for each of the 3 platform modes.</summary>
        void postLoadInit()
        {
            //[JB] Now that we're done loading, force refresh of the cmdUp and cmdDown buttons.
            if(lstEvents.Items.Count > 0)
            {
                lstEvents.SelectedIndex = 0;
            }
            else
            {
                cmdUp.Enabled = false;
                cmdDown.Enabled = false;
            }
        }

		void fillBriefData(int index, int craftType, BaseFlightGroup.BaseWaypoint waypoint, BaseFlightGroup.BaseWaypoint[] waypoints, byte iff, string name)
		{
			_briefData[index].Craft = craftType;
			_briefData[index].Waypoint = (short[])waypoint;
            _briefData[index].WaypointArr = waypoints;
			_briefData[index].IFF = iff;
			_briefData[index].Name = name;
			cboFG.Items.Add(name);
			cboFGTag.Items.Add(name);
		}

		void importDat(string filename, int size)
		{
			try
			{
				FileStream fs = File.OpenRead(filename);
				BinaryReader br = new BinaryReader(fs);
				int count = br.ReadInt16();
				Bitmap bm = new Bitmap(count * size, size, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				Graphics g = Graphics.FromImage(bm);
				SolidBrush sb = new SolidBrush(Color.Black);
				g.FillRectangle(sb, 0, 0, bm.Width, bm.Height);
				byte[] blue = { 0, 0x48, 0x60, 0x78, 0x94, 0xAC, 0xC8, 0xE0, 0xFC };
				byte[] green = { 0, 0, 4, 0x10, 0x24, 0x3C, 0x58, 0x78, 0xA0 };
				for (int i=0;i<count;i++)
				{
					fs.Position = i*2+2;
					fs.Position = br.ReadUInt16();
					byte b;
					w = br.ReadByte();	// using these vars just because I can
					h = br.ReadByte();
					int x;
					for (int q=0;q<h;q++)
					{
						for (int r=0;r<(w+1)/2;r++)
						{
							b = br.ReadByte();
							int p1 = b & 0xF;
							int p2 = (b & 0xF0) >> 4;
							x = (size-w)/2 + size*i + r*2;
							if (_platform == Settings.Platform.TIE)
							{
								x = size/2 - w + size*i + r*4;
								if (p1 != 0)
								{
									bm.SetPixel(x, size/2 - h + q*2, Color.FromArgb(0, green[p1], blue[p1]));
									bm.SetPixel(x + 1, size/2 - h + q*2, Color.FromArgb(0, green[p1], blue[p1]));
									bm.SetPixel(x, size/2 - h + q*2 + 1, Color.FromArgb(0, green[p1], blue[p1]));
									bm.SetPixel(x + 1, size/2 - h + q*2 + 1, Color.FromArgb(0, green[p1], blue[p1]));
								}
								if (p2 != 0)
								{
									bm.SetPixel(x + 2, size/2 - h + q*2, Color.FromArgb(0, green[p2], blue[p2]));
									bm.SetPixel(x + 3, size/2 - h + q*2, Color.FromArgb(0, green[p2], blue[p2]));
									bm.SetPixel(x + 2, size/2 - h + q*2 + 1, Color.FromArgb(0, green[p2], blue[p2]));
									bm.SetPixel(x + 3, size/2 - h + q*2 + 1, Color.FromArgb(0, green[p2], blue[p2]));
								}
							}
							else if (_platform == Settings.Platform.XvT)
							{
								p1 = (p1 != 0 ? (5 - p1) * 0x28 : 0);
								p2 = (p2 != 0 ? (5 - p2) * 0x28 : 0);
								if (p1 != 0) bm.SetPixel(x, (size-h)/2 + q, Color.FromArgb(p1, p1, p1));
								if (p2 != 0) bm.SetPixel(x + 1, (size-h)/2 + q, Color.FromArgb(p2, p2, p2));
							}
							else
							{
								p1 = (p1 != 0 ? p1 * 0x10 + 0xF : 0);
								p2 = (p2 != 0 ? p2 * 0x10 + 0xF : 0);
								if (p1 != 0) bm.SetPixel(x, (size-h)/2 + q, Color.FromArgb(p1, p1, p1));
								if (p2 != 0) bm.SetPixel(x + 1, (size-h)/2 + q, Color.FromArgb(p2, p2, p2));
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
		}
		void importEvents(short[] rawEvents)
		{
			BaseBriefing brief = (_platform == Settings.Platform.TIE ? (BaseBriefing)_tieBriefing : (_platform == Settings.Platform.XvT ? (BaseBriefing)_xvtBriefing : (BaseBriefing)_xwaBriefing));
			int offset = 0;
			for (int i=0;i<_maxEvents;i++)
			{
				_events[i, 0] = rawEvents[offset++];		// time
				_events[i, 1] = rawEvents[offset++];		// event
				if (_events[i, 1] == 0 || _events[i, 1] == (short)BaseBriefing.EventType.EndBriefing) break;
				else
				{
					for (int j = 2; j < 2 + brief.EventParameterCount(_events[i, 1]); j++, offset++) _events[i, j] = rawEvents[offset];
					if (_platform == Settings.Platform.XWA && _events[i, 1] == (short)BaseBriefing.EventType.XwaMoveIcon && _briefData[_events[i, 2]].Waypoint != null && _briefData[_events[i, 2]].Waypoint[0] == 0 && _briefData[_events[i, 2]].Waypoint[1] == 0)
					{	// this prevents Exception if Move instruction is before NewIcon, and only assigns initial position
						_briefData[_events[i, 2]].Waypoint[0] = _events[i, 3];
						_briefData[_events[i, 2]].Waypoint[1] = _events[i, 4];
					}
				}
				// okay, now that's in a usable format, list the event in lstEvents
				lstEvents.Items.Add("");
				updateList(i);
			}
		}
		void importStrings()
		{
            if(_tableTags.Columns.Count == 0)  //[JB] If switching Briefing indexes, the table columns are already initialized, so check to make sure.
            {
    			_tableTags.Columns.Add("tag");
			    _tableStrings.Columns.Add("string");
            }
            _tableTags.Clear();  //Erase existing strings in case switching Briefings.
            _tableStrings.Clear();
			for (int i=0;i<_tags.Length;i++)
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
			this._tableTags.RowChanged += new DataRowChangeEventHandler(tableTags_RowChanged);
			this._tableStrings.RowChanged += new DataRowChangeEventHandler(tableStrings_RowChanged);
			loadTags();
			loadStrings();
		}

		public void Import(Platform.Tie.FlightGroupCollection fg)
		{
			_briefData = new BriefData[fg.Count];
			cboFG.Items.Clear();
			cboFGTag.Items.Clear();
			for (int i = 0; i < fg.Count; i++)
			{
				fillBriefData(i, fg[i].CraftType, fg[i].Waypoints[14], null, fg[i].IFF, fg[i].Name);
			}
		}
		public void Import(Platform.Xvt.FlightGroupCollection fg)
		{
			_briefData = new BriefData[fg.Count];
			cboFG.Items.Clear();
			cboFGTag.Items.Clear();
			for (int i = 0; i < fg.Count; i++)
			{
				var wps = new BaseFlightGroup.BaseWaypoint[8];
				for (int k = 0; k < 8; k++)
					wps[k] = fg[i].Waypoints[14 + k];
				fillBriefData(i, fg[i].CraftType, fg[i].Waypoints[14], wps, fg[i].IFF, fg[i].Name);
			}
		}
		public void Save()
		{
			BaseBriefing brief = (_platform == Settings.Platform.TIE ? (BaseBriefing)_tieBriefing : (_platform == Settings.Platform.XvT ? (BaseBriefing)_xvtBriefing : (BaseBriefing)_xwaBriefing));
			int offset = 0;
			brief.Unknown1 = (short)numUnk1.Value;
            //[JB] I've encountered custom missions that didn't have proper end tags.
            //Modified the save routine with a sanity check if out of bounds, and attempts to detect the end of the event list.
            //If the end event is not found, inserts one at the end of the event list.
            bool endFound = false;
            int lastOffset = -1;
	        for (int evnt = 0; evnt < _maxEvents; evnt++)
			{
				for (int i = 0; i < 2; i++, offset++) brief.Events[offset] = _events[evnt, i];
                if (_events[evnt, 1] == (short)BaseBriefing.EventType.EndBriefing)
                {
                    endFound = true;
                    break;
                }
                else 
                {
                    if (_events[evnt, 1] == 0 && lastOffset == -1)
                        lastOffset = offset - 2;  //Found an empty event, possible candidate for an end marker.  Bookmark the write position to the start of this event.
                    else if (_events[evnt, 1] != 0)                 
                        lastOffset = -1;          //I'm not sure if any briefing utilizes an empty tag, but this resets our detection just in case.  Don't want to overwrite anything we're not supposed to.

                    if (offset >= brief.Events.Length - 1) break;
                    for (int i = 2; i < 2 + brief.EventParameterCount(_events[evnt, 1]) && offset < brief.Events.Length; i++, offset++)
                        brief.Events[offset] = _events[evnt, i];
                }
			}
            if (!endFound && lastOffset >= 0 && lastOffset < brief.Events.Length - 2)
            {
                brief.Events[lastOffset] = 9999;
                brief.Events[lastOffset + 1] = (short)BaseBriefing.EventType.EndBriefing;
            }
			if (_platform == Settings.Platform.XvT) _xvtBriefing.Unknown3 = (short)numUnk3.Value;
		}

		void tabBrief_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabBrief.SelectedIndex != 0) hsbTimer.Value = 1;
			else hsbTimer.Value = 0;	// force refresh, since pct doesn't want to update when hidden
		}

		#region frmBrief
		void frmBrief_Activated(object sender, EventArgs e) { MapPaint(); }
        void frmBrief_FormClosed(object sender, FormClosedEventArgs e)
        {
            tabBrief.Focus();       //[JB] Leave any focused controls so that events don't refresh the disposed map.
            _map.Dispose();
        }  
		void frmBrief_FormClosing(object sender, FormClosingEventArgs e)
		{
			Save();
            //[JB] Stop and deactivate the timers.
            //Important! There's an issue where the event can trigger after the map is disposed, even after calling Stop(). The event must be unregistered.
            tmrPopup.Stop();
            tmrPopup.Tick -= tmrPopup_Tick;
            tmrMapRedraw.Stop();
            tmrMapRedraw.Tick -= tmrMapRedraw_Tick;
            /*if (_platform==Settings.Platform.TIE) TIESave();
            else if (_platform==Settings.Platform.XvT) XvTSave();
            else XWASave();*/
		}
		void frmBrief_Load(object sender, EventArgs e)
		{
            for (int i=0;i<8;i++) _fgTags[i, 0] = -1;
			for (int i=0;i<8;i++) _textTags[i, 0] = -1;
			_map = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            hsbTimer.Value = 1;  //[JB] Need to force another update, since this function wipes the tags after the init is complete.
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
			if (hsbTimer.Value == hsbTimer.Maximum-11) return;
            int newSpeed = tmrBrief.Interval / 2;  //[JB] Clicking FF repeatedly will keep speeding it up.
            if (newSpeed < 125 / _timerInterval) newSpeed = 125 / _timerInterval;    //Limit to 8x speed.
            tmrBrief.Interval = newSpeed;  //500 / _timerInterval;
			startTimer();
		}
		void cmdNext_Click(object sender, EventArgs e)
		{
			int i;
			for (i=0;i<_maxEvents;i++)
			{
				if (_events[i, 0] <= hsbTimer.Value || _events[i, 1] != 3) continue;	// find next stop point after current position
				break;
			}
			if (i == _maxEvents) hsbTimer.Value = hsbTimer.Maximum-11;	// tmr_Tick takes care of halting
			else hsbTimer.Value = _events[i, 0];
		}
		void cmdPause_Click(object sender, EventArgs e) { stopTimer(); }
		void cmdPlay_Click(object sender, EventArgs e)
		{
			if (hsbTimer.Value == hsbTimer.Maximum-11) return;	// prevent starting if already at the end
			tmrBrief.Interval = 1000 / _timerInterval;
			startTimer();
		}
		void cmdStart_Click(object sender, EventArgs e)
		{
			for (int i=0;i<8;i++)
			{
				_fgTags[i, 0] = -1;
				_fgTags[i, 1] = 0;
			}
			for (int i=0;i<8;i++)
			{
				_textTags[i, 0] = -1;
				_textTags[i, 1] = 0;
				_textTags[i, 2] = 0;
				_textTags[i, 3] = 0;
			}
            _previousTimeIndex = 0;
			hsbTimer.Value = 0;
		}
		void cmdStop_Click(object sender, EventArgs e)
		{
			stopTimer();
			for (int i=0;i<8;i++)
			{
				_fgTags[i, 0] = -1;
				_fgTags[i, 1] = 0;
			}
			for (int i=0;i<8;i++)
			{
				_textTags[i, 0] = -1;
				_textTags[i, 1] = 0;
				_textTags[i, 2] = 0;
				_textTags[i, 3] = 0;
			}
			hsbTimer.Value = 0;
		}

		void hsbTimer_ValueChanged(object sender, EventArgs e)
		{
            bool paint = false;
            if (hsbTimer.Value != 0 && ((hsbTimer.Value - _previousTimeIndex >= 2) || hsbTimer.Value <= _previousTimeIndex))  //A non-incremental or reverse change (if incremental the timer should be +1 to previous), the user most likely manually moved the scrollbar.  Iterate through all past events and rebuild the briefing state.
            {
                resetBriefing();
                stopTimer();
                for (int i = 0; i < _maxEvents; i++)
                {
                    //Process everything up to the current time index.
                    if (_events[i, 0] > hsbTimer.Value || _events[i, 1] == (int)BaseBriefing.EventType.None || _events[i, 1] == (int)BaseBriefing.EventType.EndBriefing) break;
                    paint |= processEvent(i, true);  //paint stays enabled once enabled.
                }
            }

            _previousTimeIndex = hsbTimer.Value;
			if (hsbTimer.Value == 0)
			{
                resetBriefing();  //[JB] Moved code to function.
			}
			if (_regionDelay != -1)
			{
				_message = "";
				_regionDelay = -1;
				lblCaption.Visible = true;
				lblTitle.Visible = true;
			}
			for (int i=0;i<_maxEvents;i++)
			{
				if (_events[i,0] < hsbTimer.Value) continue;
				if (_events[i,0] > hsbTimer.Value || _events[i,1] == (int)BaseBriefing.EventType.None || _events[i,1] == (int)BaseBriefing.EventType.EndBriefing) break;
                paint |= processEvent(i, false);  //paint stays enabled once enabled.
            }
			for (int h=0;h<8;h++) if (hsbTimer.Value - _fgTags[h, 1] < 13) paint = true;
			lblTime.Text = String.Format("{0:Time: 0.00}",(decimal)hsbTimer.Value / _timerInterval);
			if (hsbTimer.Value == (hsbTimer.Maximum-11) || hsbTimer.Value == 0) stopTimer();
			if (paint) MapPaint();	// prevent MapPaint from running if no change
            if (tmrBrief.Interval != (1000 / _timerInterval))  //[JB] Show playback speed if playing fast-forward
                lblTime.Text += "  (" + (1000 / _timerInterval) / tmrBrief.Interval + "x)";
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
			//[JB] Wrapper that handles the calling of the actual map rendering.
			if (_mapPaintScheduled)
			{
				//[JB] Code formerly in the MapPaint(), performs the rendering.
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
            //[JB] I modified this function to instead serve as a wrapper that handles the map rendering.  It seems to reduce immediate performance for the sake of consistency and not clogging CPU cycles during rapid re-draw attempts. 
            if (!_mapPaintScheduled)
            {
                if (!tmrMapRedraw.Enabled)         //The timer is stopped, start it up.
                {
                    tmrMapRedraw.Start();
                    //_mapPaintRedrawTimer.Interval = 17;    //Was trying to aim for ~60 FPS, but according to MSDN, the minimum accuracy is 55 ms.
                }
                _mapPaintScheduled = true;
            }
        }

        void drawGrid(int x, int y, Graphics g)
		{
			Pen pn = new Pen(Color.FromArgb(0x50, 0, 0));
			pn.Width = 1;
			if (_platform == Settings.Platform.TIE)
			{
				pn.Color = Color.FromArgb(0x48, 0, 0);
				pn.Width = 2;
			}
			int mod = (_platform == Settings.Platform.TIE ? 2 : 1);

            //Calculate where the viewport is, then find the longest span to know how many lines to iterate through.
            int x1 = (x - w) / (_zoomX * mod);
            int y1 = (y - h) / (_zoomX * mod);
            int x2 = (x + w) / (_zoomX * mod);
            int y2 = (y + h) / (_zoomX * mod);
            int min = x1, max = x2;
            if (y1 < min)
                min = y1;
            if (y2 > max)
                max = y2;

			if (_zoomX >= 32)
			{
                for (int i = min; i < max; i++)  //[JB] Fixed line drawing if the map is moved far away from center.
				{
					if (i % 4 == 0) continue;						// don't draw where there'll be maj lines
					g.DrawLine(pn, 0, _zoomY*i*mod + y-1, w, _zoomY*i*mod + y-1);	//min lines, every zoom pixels
					g.DrawLine(pn, 0, y-1 - _zoomY*i*mod, w, y-1 - _zoomY*i*mod);
					g.DrawLine(pn, _zoomX*i*mod + x, 0, _zoomX*i*mod + x, h);
					g.DrawLine(pn, x - _zoomX*i*mod, 0, x - _zoomX*i*mod, h);
				}
			}
			else if (_zoomX >= 16)
			{
                for (int i = min; i < max; i++)
				{
					if (i % 2 == 0) continue;
					g.DrawLine(pn, 0, _zoomY*2*i*mod + y-1, w, _zoomY*2*i*mod + y-1);	//min lines, every zoomx2 pixels
					g.DrawLine(pn, 0, y-1 - _zoomY*2*i*mod, w, y-1 - _zoomY*2*i*mod);
					g.DrawLine(pn, _zoomX*2*i*mod + x, 0, _zoomX*2*i*mod + x, h);
					g.DrawLine(pn, x - _zoomX*2*i*mod, 0, x - _zoomX*2*i*mod, h);
				}
			}
			// else if (j < 16) just don't draw them
			pn.Color = Color.FromArgb(0x90, 0, 0);
			if (_platform == Settings.Platform.TIE) pn.Color = Color.FromArgb(0x78, 0, 0);
			g.DrawLine(pn, 0, y-1, w, y-1);	// origin lines
			g.DrawLine(pn, x, 0, x, h);
			for (int i=0;i<36;i++)
			{
				g.DrawLine(pn, 0, _zoomY*4*i*mod + y-1, w, _zoomY*4*i*mod + y-1);	//maj lines, every zoomx4 pixels
				g.DrawLine(pn, 0, y-1 - _zoomY*4*i*mod, w, y-1 - _zoomY*4*i*mod);
				g.DrawLine(pn, _zoomX*4*i*mod + x, 0, _zoomX*4*i*mod + x, h);
				g.DrawLine(pn, x - _zoomX*4*i*mod, 0, x - _zoomX*4*i*mod, h);
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
				if (_events[i, 0] < hsbTimer.Value) continue;
				if (_events[i, 0] > hsbTimer.Value) return (i+10000);	// did not find existing, return next available + marker
				if (_events[i, 1] == (int)eventType) return i;	// found existing
			}
			return (i+10000);	// actually somehow got through the entire loop. odds of this happening is likely zero, but some moron will do it eventually
		}
		int findNext() { return findNext(hsbTimer.Value); }
		int findNext(int time)
		{
			int i;
			for (i = 0; i < _maxEvents; i++)
			{
				if (_events[i, 0] < time) continue;
				if (_events[i, 0] > time) break;
			}
			return i;
		}
		Bitmap flatMask(Bitmap craftImage, byte iff, byte intensity)
		{
			// okay, this one is just for FG tags.  flat image, I only care about the shape.
			Bitmap bmpNew = new Bitmap(craftImage);
			BitmapData bmData = GraphicsFunctions.GetBitmapData(bmpNew, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride*bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			#region declare IFF
			byte[] rgb = new byte[3];
			switch (iff)
			{
				case 0:		// green
					rgb[1] = 1;
					break;
				case 2:		// blue
					rgb[1] = 2;
					rgb[2] = 1;
					break;
				case 3:		// purple
				case 5:		// purple2
					rgb[0] = 1;
					rgb[2] = 1;
					break;
				default:	// red
					rgb[0] = 1;
					break;
			}
			#endregion
			for (int y = 0;y < bmpNew.Height;y++)
			{
				for (int x = 0, pos = bmData.Stride*y;x < bmpNew.Width;x++)
				{
					if (pix[pos+x*3] == 0) continue;
					pix[pos+x*3] = (byte)(intensity * rgb[2]);
					pix[pos+x*3+1] = (byte)(rgb[1] == 2 ? (intensity==0xe0 ? 0x78 : (intensity==0x78 ? 0x10 : (intensity==0xfc ? 0xa0 : (intensity==0xc8 ? 0x58 : (intensity==0x94 ? 0x24 : 4))))) : intensity * rgb[1]);
					pix[pos+x*3+2] = (byte)(intensity * rgb[0]);
				}
			}
			GraphicsFunctions.CopyBytesToImage(pix, bmData);
			bmpNew.UnlockBits(bmData);
			bmpNew.MakeTransparent(Color.Black);
			return bmpNew;
		}
		int[] getTagSize(int craft)
		{
			FileStream fs = File.OpenRead(Application.StartupPath + "\\images\\XvT_BRF.dat");
			BinaryReader br = new BinaryReader(fs);
			fs.Position = craft*2+2;
			fs.Position = br.ReadInt16();
			int[] size = new int[2];
			size[0] = br.ReadByte();
			size[1] = br.ReadByte();
			fs.Close();
			return size;	// size of base craft image as [width,height]
		}
		void imageQuad(int x, int y, int spacing, Bitmap craftImage, Graphics g)
		{
			g.DrawImageUnscaled(craftImage, x+spacing, y+spacing);
			g.DrawImageUnscaled(craftImage, x+spacing, y-spacing);
			g.DrawImageUnscaled(craftImage, x-spacing, y-spacing);
			g.DrawImageUnscaled(craftImage, x-spacing, y+spacing);
		}
		void popupUpdate(string s)
		{
			lblPopupInfo.Visible = true;
			lblPopupInfo.Text = s;
			tmrPopup.Start();
		}
		void popupPreviewStop()
		{
			if (_popupPreviewActive)
			{
				_mapX = (short)_popupPreviewMap.X;  //Restore prior map settings
				_mapY = (short)_popupPreviewMap.Y;
				_zoomX = (short)_popupPreviewZoom.X;
				_zoomY = (short)_popupPreviewZoom.Y;

				_popupPreviewActive = false;
				MapPaint();
			}
		}
		void popupPreviewStart()
		{
			if (!_popupPreviewActive)
			{
				pctBrief.Focus();  //Need to force focus so it generates MouseWheel events
				_popupPreviewMap.X = _mapX;  //Backup existing map settings
				_popupPreviewMap.Y = _mapY;
				_popupPreviewZoom.X = _zoomX;
				_popupPreviewZoom.Y = _zoomY;

				_popupPreviewActive = true;
				MapPaint();
			}
		}
		bool processEvent(int evtIndex, bool rebuild)
		{
			bool paint = false;
			int i = evtIndex;  //[JB] I just moved the old code here.  No changes except to prevent hiding of text/caption panels during rebuild, since it causes heavy flickering while scrolling.
			if (_events[i, 1] == (int)BaseBriefing.EventType.PageBreak)
			{
				if (_platform == Settings.Platform.TIE) lblTitle.Text = "";
				lblCaption.Text = "";
				_page++;
				paint = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.TitleText && _platform == Settings.Platform.TIE)  // XvT and XWA use .LST files
			{
				if (_strings[_events[i, 2]].StartsWith(">"))
				{
					lblTitle.TextAlign = ContentAlignment.TopCenter;
					lblTitle.ForeColor = _titleColor;
					lblTitle.Text = _strings[_events[i, 2]].Replace(">", "");
				}
				else
				{
					lblTitle.TextAlign = ContentAlignment.TopLeft;
					lblTitle.ForeColor = _normalColor;
					lblTitle.Text = _strings[_events[i, 2]];
				}
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.CaptionText)
			{
				if (_strings[_events[i, 2]].StartsWith(">"))
				{
					lblCaption.TextAlign = ContentAlignment.TopCenter;
					lblCaption.ForeColor = _titleColor;
					lblCaption.Text = _strings[_events[i, 2]].Replace(">", "").Replace("$", "\r\n");
				}
				else
				{
					lblCaption.TextAlign = ContentAlignment.TopLeft;
					lblCaption.ForeColor = _normalColor;
					lblCaption.Text = _strings[_events[i, 2]].Replace("$", "\r\n");
				}
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.MoveMap)
			{
				_mapX = _events[i, 2];
				_mapY = _events[i, 3];
				if (_platform == Settings.Platform.XvT || _platform == Settings.Platform.BoP)
				{
					_mapX /= 2;
					_mapY /= 2;
				}
				paint = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.ZoomMap)
			{
				_zoomX = _events[i, 2];
				_zoomY = _events[i, 3];
				if (_zoomX < 1) _zoomX = 1;  //[JB] Prevent divide by zero when drawing the grid
				if (_zoomY < 1) _zoomY = 1;
				paint = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.ClearFGTags)
			{
				for (int h = 0; h < 8; h++)
				{
					_fgTags[h, 0] = -1;
					_fgTags[h, 1] = 0;
				}
				paint = true;
			}
			else if (_events[i, 1] >= (int)BaseBriefing.EventType.FGTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.FGTag8)
			{
				int v = _events[i, 1] - (int)BaseBriefing.EventType.FGTag1;
				_fgTags[v, 0] = _events[i, 2];  // FG number
				_fgTags[v, 1] = _events[i, 0];  // time started, for MapPaint
				paint = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.ClearTextTags)
			{
				for (int h = 0; h < 8; h++)
				{
					_textTags[h, 0] = -1;
					_textTags[h, 1] = 0;
					_textTags[h, 2] = 0;
					_textTags[h, 3] = 0;
				}
				paint = true;
			}
			else if (_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8)
			{
				int v = _events[i, 1] - (int)BaseBriefing.EventType.TextTag1;
				_textTags[v, 0] = _events[i, 2];    // tag#
				_textTags[v, 1] = _events[i, 3];    // X
				_textTags[v, 2] = _events[i, 4];    // Y
				_textTags[v, 3] = _events[i, 5];    // color
				paint = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaNewIcon)
			{
				_briefData[_events[i, 2]].Craft = _events[i, 3] - 1;
				_briefData[_events[i, 2]].IFF = (byte)_events[i, 4];
				_briefData[_events[i, 2]].Name = "Icon #" + _events[i, 2].ToString();
				_briefData[_events[i, 2]].Waypoint = new short[4];
				if (_events[i, 3] != 0) _briefData[_events[i, 2]].Waypoint[3] = 1;
				else _briefData[_events[i, 2]].Waypoint[3] = 0;
				paint = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaShipInfo)
			{
				if (_events[i, 2] == 1)
				{
					if (_briefData[_events[i, 3]].Craft != 0) _message = "Ship Info: " + Platform.Xwa.Strings.CraftType[_briefData[_events[i, 3]].Craft + 1];
					else _message = "Ship Info: <flight group not found>";
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
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaMoveIcon)
			{
				if (Common.IsValidArray(_briefData[_events[i, 2]].Waypoint, 1))
				{
					_briefData[_events[i, 2]].Waypoint[0] = _events[i, 3];
					_briefData[_events[i, 2]].Waypoint[1] = _events[i, 4];
					paint = true;
				}
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaRotateIcon)
			{
				if (Common.IsValidArray(_briefData[_events[i, 2]].Waypoint, 2))
				{
					_briefData[_events[i, 2]].Waypoint[2] = _events[i, 3];
					paint = true;
				}
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaChangeRegion)
			{
				for (int h = 0; h < 8; h++)
				{
					_fgTags[h, 0] = -1;
					_fgTags[h, 1] = 0;
					_textTags[h, 0] = -1;
					_textTags[h, 1] = 0;
					_textTags[h, 2] = 0;
					_textTags[h, 3] = 0;
				}
				_briefData = new BriefData[_briefData.Length];

				_message = "Region " + (_events[i, 2] + 1);
				_regionDelay = _timerInterval * 3;

				if (!rebuild)
				{
					lblTitle.Visible = false;
					lblCaption.Visible = false;
				}
				paint = true;
			}
			// don't need to account for EndBriefing
			return paint;
		}
		void resetBriefing()
		{
			_page = 1;
			_mapX = 0;
			_mapY = 0;
			_zoomX = 48;
			if (_platform == Settings.Platform.XvT || _platform == Settings.Platform.BoP || _platform == Settings.Platform.XWA) _zoomX = 32;  //[JB] Default for XvT is also 32
			_zoomY = _zoomX;
			for (int h = 0; h < 8; h++)
			{
				_fgTags[h, 0] = -1;
				_fgTags[h, 1] = 0;
			}
			for (int h = 0; h < 8; h++)
			{
				_textTags[h, 0] = -1;
				_textTags[h, 1] = 0;
				_textTags[h, 2] = 0;
				_textTags[h, 3] = 0;
			}
			if (_platform == Settings.Platform.XWA) _briefData = new BriefData[_briefData.Length];
			_message = "";
			lblTitle.Visible = true;
			lblCaption.Visible = true;
			lblTitle.Text = "";  //[JB] Clear these to force refresh, otherwise it holds old strings, even if the event list is wiped clean.
			lblCaption.Text = "";
		}
		void tieMask(Bitmap craftImage, byte iff)
		{
			// works a little different than mission map, everything guides off the B value and IFF
			// image is stored as blue due to non-standard G values that do not fit a good equation
			BitmapData bmData = GraphicsFunctions.GetBitmapData(craftImage, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride*bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			#region declare IFF
			byte[] rgb = new byte[3];
			switch (iff)
			{
				case 0:		// green
					rgb[1] = 1;
					break;
				case 2:		// blue
					rgb[1] = 2;
					rgb[2] = 1;
					break;
				case 3:		// purple
				case 5:		// purple2
					rgb[0] = 1;
					rgb[2] = 1;
					break;
				default:	// red
					rgb[0] = 1;
					break;
			}
			#endregion
			for (int y = 0;y < craftImage.Height;y++)
			{
				for (int x = 0, pos = bmData.Stride*y;x < craftImage.Width;x++)
				{
					pix[pos + x * 3 + 2] = (byte)(pix[pos + x * 3] * rgb[0]);
					pix[pos+x*3+1] = (rgb[1] == 2 ? pix[pos+x*3+1] : (byte)(pix[pos+x*3] * rgb[1]));
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
			int X = 2*(-_zoomX*_mapX/256) + w/2;	// values are written out like this to force even numbers
			int Y = 2*(-_zoomY*_mapY/256) + h/2;
			Pen pn = new Pen(Color.FromArgb(0x48, 0, 0));
			pn.Width = 2;
			Graphics g3;
			g3 = Graphics.FromImage(_map);		//graphics obj, load from the memory bitmap
			g3.Clear(SystemColors.Control);		//clear it
			SolidBrush sb = new SolidBrush(Color.Black);
			g3.FillRectangle(sb, 0, 0, w, h);
			g3.DrawLine(pn, 0, 1, w, 1);
			g3.DrawLine(pn, 0, h-3, w, h-3);
			g3.DrawLine(pn, 1, 0, 1, h);
			g3.DrawLine(pn, w-1, 0, w-1, h);
			drawGrid(X, Y, g3);
			Bitmap bmptemp;
			#region FG tags
			Bitmap bmptemp2;
			for (int i=0;i<8;i++)
			{
				if (_fgTags[i, 0] == -1 || _briefData[_fgTags[i, 0]].Waypoint[3] != 1) continue;
				sb = new SolidBrush(Color.FromArgb(0xE0, 0, 0));	// defaults to Red
				SolidBrush sb2 = new SolidBrush(Color.FromArgb(0x78, 0, 0));
				byte IFF = _briefData[_fgTags[i, 0]].IFF;
				int wpX = 2*(int)Math.Round((double)_zoomX*_briefData[_fgTags[i, 0]].Waypoint[0]/256, 0) + X;
				int wpY = 2*(int)Math.Round((double)_zoomY*-_briefData[_fgTags[i, 0]].Waypoint[1]/256, 0) + Y;  //[JB] Invert Y axis of waypoint
				int frame = hsbTimer.Value - _fgTags[i, 1];
				if (_fgTags[i, 1] == 0) frame = 12;	// if tagged at t=0, just the box
				switch (frame)
				{
					case 0:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0xFC);
						imageQuad(wpX-16, wpY-16, 32, bmptemp2, g3);
						break;
					case 1:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0xC8);
						imageQuad(wpX-16, wpY-16, 32, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xFC);
						imageQuad(wpX-16, wpY-16, 28, bmptemp2, g3);
						break;
					case 2:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x94);
						imageQuad(wpX-16, wpY-16, 32, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xC8);
						imageQuad(wpX-16, wpY-16, 28, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xFC);
						imageQuad(wpX-16, wpY-16, 24, bmptemp2, g3);
						break;
					case 3:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x60);
						imageQuad(wpX-16, wpY-16, 32, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0x94);
						imageQuad(wpX-16, wpY-16, 28, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xC8);
						imageQuad(wpX-16, wpY-16, 24, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xFC);
						imageQuad(wpX-16, wpY-16, 20, bmptemp2, g3);
						break;
					case 4:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x60);
						imageQuad(wpX-16, wpY-16, 28, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0x94);
						imageQuad(wpX-16, wpY-16, 24, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xC8);
						imageQuad(wpX-16, wpY-16, 20, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xFC);
						imageQuad(wpX-16, wpY-16, 16, bmptemp2, g3);
						break;
					case 5:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x60);
						imageQuad(wpX-16, wpY-16, 24, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0x94);
						imageQuad(wpX-16, wpY-16, 20, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xC8);
						imageQuad(wpX-16, wpY-16, 16, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xFC);
						imageQuad(wpX-16, wpY-16, 12, bmptemp2, g3);
						break;
					case 6:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x60);
						imageQuad(wpX-16, wpY-16, 20, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0x94);
						imageQuad(wpX-16, wpY-16, 16, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xC8);
						imageQuad(wpX-16, wpY-16, 12, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xFC);
						imageQuad(wpX-16, wpY-16, 8, bmptemp2, g3);
						break;
					case 7:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x60);
						imageQuad(wpX-16, wpY-16, 16, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0x94);
						imageQuad(wpX-16, wpY-16, 12, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xC8);
						imageQuad(wpX-16, wpY-16, 8, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xFC);
						imageQuad(wpX-16, wpY-16, 4, bmptemp2, g3);
						break;
					case 8:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x60);
						imageQuad(wpX-16, wpY-16, 12, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0x94);
						imageQuad(wpX-16, wpY-16, 8, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0xC8);
						imageQuad(wpX-16, wpY-16, 4, bmptemp2, g3);
						break;
					case 9:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x60);
						imageQuad(wpX-16, wpY-16, 8, bmptemp2, g3);
						bmptemp2 = flatMask(bmptemp, IFF, 0x94);
						imageQuad(wpX-16, wpY-16, 4, bmptemp2, g3);
						break;
					case 10:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = flatMask(bmptemp, IFF, 0x60);
						imageQuad(wpX-16, wpY-16, 4, bmptemp2, g3);
						break;
					case 11:
						if (IFF == 0) { sb.Color = Color.FromArgb(0, 0xE0, 0); sb2.Color = Color.FromArgb(0, 0x78, 0); }	// green
						else if (IFF == 2) { sb.Color = Color.FromArgb(0, 0x78, 0xE0); sb2.Color = Color.FromArgb(0, 0x10, 0x78); }	// blue
						else if (IFF == 3 || IFF == 5) { sb.Color = Color.FromArgb(0xE0, 0, 0xE0); sb2.Color = Color.FromArgb(0x78, 0, 0x78); }	// purple
						g3.FillRectangle(sb, wpX - 8, wpY - 8, 18, 18);
						g3.FillRectangle(sb2, wpX - 6, wpY - 6, 14, 14);
						break;
					default:
						// 12 or greater, just the box
						if (IFF == 0) { sb.Color = Color.FromArgb(0, 0xE0, 0); sb2.Color = Color.FromArgb(0, 0x78, 0); }	// green
						else if (IFF == 2) { sb.Color = Color.FromArgb(0, 0x78, 0xE0); sb2.Color = Color.FromArgb(0, 0x10, 0x78); }	// blue
						else if (IFF == 3 || IFF == 5) { sb.Color = Color.FromArgb(0xE0, 0, 0xE0); sb2.Color = Color.FromArgb(0x78, 0, 0x78); }	// purple
						g3.FillRectangle(sb, wpX - 12, wpY - 12, 26, 26);
						g3.FillRectangle(sb2, wpX - 10, wpY - 10, 22, 22);
						break;
				}
			}
			#endregion // FG tags
			#region text tags
			for (int i=0;i<8;i++)
			{
				if (_textTags[i, 0] == -1) continue;
				sb = new SolidBrush(Color.FromArgb(0xAC, 0, 0));	// default to red
				int clr = _textTags[i, 3];
				if (clr == 0) sb.Color = Color.FromArgb(0, 0xAC, 0);	// green
				// else if (clr == 1) sb.Color = Color.FramArgb(0xAC,0,0);	// red
				else if (clr == 2) sb.Color = Color.FromArgb(0xAC, 0, 0xAC);	// purple
				else if (clr == 3) sb.Color = Color.FromArgb(0, 0x2C, 0xAC);	// blue
				else if (clr == 4) sb.Color = Color.FromArgb(0xA8, 0, 0);	// red2
				else if (clr == 5) sb.Color = Color.FromArgb(0xFC, 0x54, 0x54);	// light red
				else if (clr == 6) sb.Color = Color.FromArgb(0x44, 0x44, 0x44);	// gray
				else if (clr == 7) sb.Color = Color.FromArgb(0xCC, 0xCC, 0xCC);	// white
				g3.DrawString(_tags[_textTags[i, 0]], new Font("MS Reference Sans Serif", 10), sb, 2*(int)Math.Round((double)_zoomX*_textTags[i, 1]/256, 0) + X, 2*(int)Math.Round((double)_zoomY*_textTags[i, 2]/256, 0) + Y);
			}
			#endregion	// text tags
			for (int i=0;i<_briefData.Length;i++)
			{
				if (_briefData[i].Waypoint[3] != 1) continue;
				if (_zoomX >= 32) bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft]);
				else bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft+88]);	// small icon
				tieMask(bmptemp, _briefData[i].IFF);
				// simple base-256 grid coords * zoom to get pixel location, * 2 to enlarge, + map offset, - pic size/2 to center
				// forced to even numbers
				g3.DrawImageUnscaled(bmptemp, 2*(int)Math.Round((double)_zoomX*_briefData[i].Waypoint[0]/256, 0) + X - 16, 2*(int)Math.Round((double)_zoomY*-_briefData[i].Waypoint[1]/256, 0) + Y - 16);  //[JB] Invert Y axis of waypoint, fixed Y zoom.
			}
			g3.DrawString("#" + _page, new Font("Arial", 8), new SolidBrush(Color.White), w-20, 4);
			pctBrief.Invalidate();		// since it's drawing to memory, this refreshes the pct.  Removes the flicker when zooming
			g3.Dispose();
		}
		Bitmap xvtMask(Bitmap craftImage, byte iff, byte frame)
		{
			Bitmap bmpNew = new Bitmap(craftImage);
			BitmapData bmData = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride*bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			#region define FG tag colors
			byte[] p = new byte[5];
			byte intensity = 255;
			if (frame == 2) intensity = 0xC5;
			else if (frame == 3) intensity = 0x8F;
			else if (frame == 4) intensity = 0x5B;
			p[0] = (byte)(0xB8 * (intensity+1) / 256);
			p[1] = (byte)(0x98 * (intensity+1) / 256);
			p[2] = (byte)(0x70 * (intensity+1) / 256);
			p[3] = (byte)(0x58 * (intensity+1) / 256);
			#endregion
			#region define IFF color distribution
			byte[] mask = new byte[3];
			byte[,] rgb = new byte[5, 3];
			switch (iff)
			{
				case 0:		// green
					mask[1] = 1;
					rgb[0, 0] = 0x38;
					rgb[0, 1] = 0xD4;
					rgb[1, 0] = 0x18;
					rgb[1, 1] = 0xA8;
					rgb[2, 0] = 8;
					rgb[2, 1] = 0x7C;
					rgb[3, 1] = 0x54;
					break;
				case 2:		// blue
					mask[2] = 1;
					rgb[0, 0] = 0x58;
					rgb[0, 1] = 0xDC;
					rgb[0, 2] = 0xF8;
					rgb[1, 0] = 0x28;
					rgb[1, 1] = 0x84;
					rgb[1, 2] = 0xC0;
					rgb[2, 0] = 8;
					rgb[2, 1] = 0x3C;
					rgb[2, 2] = 0x90;
					rgb[3, 1] = 8;
					rgb[3, 2] = 0x58;
					break;
				case 3:		// yellow
					mask[0] = 1;
					mask[1] = 1;
					rgb[0, 0] = 0xF8;
					rgb[0, 1] = 0xFC;
					rgb[1, 0] = 0xD0;
					rgb[1, 1] = 0xCC;
					rgb[2, 0] = 0xA8;
					rgb[2, 1] = 0x9C;
					rgb[3, 0] = 0x80;
					rgb[3, 1] = 0x74;
					break;
				case 5:		// purple
					mask[0] = 1;
					mask[2] = 1;
					rgb[0, 0] = 0x90;
					rgb[0, 1] = 0x88;
					rgb[0, 2] = 0xF0;
					rgb[1, 0] = 0x70;
					rgb[1, 1] = 0x5C;
					rgb[1, 2] = 0xB0;
					rgb[2, 0] = 0x50;
					rgb[2, 1] = 0x30;
					rgb[2, 2] = 0x78;
					rgb[3, 0] = 0x30;
					rgb[3, 1] = 8;
					rgb[3, 2] = 0x40;
					break;
				default:	// red
					mask[0] = 1;
					rgb[0, 0] = 0xF8;
					rgb[0, 1] = 0x24;
					rgb[1, 0] = 0xC0;
					rgb[1, 1] = 0x10;
					rgb[2, 0] = 0x80;
					rgb[2, 1] = 4;
					rgb[3, 0] = 0x48;
					break;
			}
			#endregion
			byte b;
			for (int y = 0;y < bmpNew.Height;y++)
			{
				for (int x = 0, pos = y*bmData.Stride;x < bmpNew.Width;x++)
				{
					// stupid thing returns BGR instead of RGB
					b = pix[pos+x*3+1];
					if (frame == 0)
					{
						pix[pos+x*3] = rgb[4 - (b/0x28), 2];
						pix[pos+x*3+1] = rgb[4 - (b/0x28), 1];
						pix[pos+x*3+2] = rgb[4 - (b/0x28), 0];
						continue;
					}
					b = p[4 - (b/0x28)];
					pix[pos+x*3] = (byte)(b * mask[2]);
					pix[pos+x*3+1] = (byte)(b * mask[1]);
					pix[pos+x*3+2] = (byte)(b * mask[0]);
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
			int X = 2*(-_zoomX*_mapX/256) + w/2;	// values are written out like this to force even numbers
			int Y = 2*(-_zoomY*_mapY/256) + h/2;    //[JB] mapX(-320) and mapY(+200) offsets better approximate the viewport as it appears in game, although it slightly messes up the point calculations used elsewhere
			Pen pn = new Pen(Color.FromArgb(0x50, 0, 0));
			pn.Width = 1;
			Graphics g3;
			g3 = Graphics.FromImage(_map);		//graphics obj, load from the memory bitmap
			g3.Clear(SystemColors.Control);		//clear it
			SolidBrush sb = new SolidBrush(Color.Black);
			g3.FillRectangle(sb, 0, 0, w, h);
			drawGrid(X, Y, g3);
			Bitmap bmptemp;
			#region FG tags
			Bitmap bmptemp2;
            int briefIndex = cboBriefIndex1.SelectedIndex;  //[JB]  Modified the XvT code to use new WaypointArr so that it can access the briefing waypoint slots above Briefing #1
            if(briefIndex < 0) briefIndex = 0;
            BaseFlightGroup.BaseWaypoint wp;
			for (int i=0;i<8;i++)
			{
                if (_fgTags[i, 0] == -1) continue;
                wp = _briefData[_fgTags[i, 0]].WaypointArr[briefIndex];  //[JB] Modified so it can use the correct briefing waypoint.  All code that accesses waypoint in this function changed to access this.
				if (wp[3] != 1) continue;
				sb = new SolidBrush(Color.FromArgb(0xE0, 0, 0));	// defaults to Red
				SolidBrush sb2 = new SolidBrush(Color.FromArgb(0x78, 0, 0));
				byte IFF = _briefData[_fgTags[i, 0]].IFF;
				int wpX = (int)Math.Round((double)_zoomX*wp[0]/256, 0) + X;
				int wpY = (int)Math.Round((double)_zoomY*-wp[1]/256, 0) + Y; //[JB] Invert Y axis
				int frame = hsbTimer.Value - _fgTags[i, 1];
				if (_fgTags[i, 1] == 0) frame = 12;	// if tagged at t=0, just the box
				int[] pos = getTagSize(_briefData[_fgTags[i, 0]].Craft);
				switch (frame)
				{
					case 0:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 1);
						imageQuad(wpX-11, wpY-11, 16, bmptemp2, g3);
						break;
					case 1:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 2);
						imageQuad(wpX-11, wpY-11, 16, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 1);
						imageQuad(wpX-11, wpY-11, 14, bmptemp2, g3);
						break;
					case 2:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 3);
						imageQuad(wpX-11, wpY-11, 16, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 2);
						imageQuad(wpX-11, wpY-11, 14, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 1);
						imageQuad(wpX-11, wpY-11, 12, bmptemp2, g3);
						break;
					case 3:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 4);
						imageQuad(wpX-11, wpY-11, 16, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 3);
						imageQuad(wpX-11, wpY-11, 14, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 2);
						imageQuad(wpX-11, wpY-11, 12, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 1);
						imageQuad(wpX-11, wpY-11, 10, bmptemp2, g3);
						break;
					case 4:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 4);
						imageQuad(wpX-11, wpY-11, 14, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 3);
						imageQuad(wpX-11, wpY-11, 12, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 2);
						imageQuad(wpX-11, wpY-11, 10, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 1);
						imageQuad(wpX-11, wpY-11, 8, bmptemp2, g3);
						break;
					case 5:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 4);
						imageQuad(wpX-11, wpY-11, 12, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 3);
						imageQuad(wpX-11, wpY-11, 10, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 2);
						imageQuad(wpX-11, wpY-11, 8, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 1);
						imageQuad(wpX-11, wpY-11, 6, bmptemp2, g3);
						break;
					case 6:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 4);
						imageQuad(wpX-11, wpY-11, 10, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 3);
						imageQuad(wpX-11, wpY-11, 8, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 2);
						imageQuad(wpX-11, wpY-11, 6, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 1);
						imageQuad(wpX-11, wpY-11, 4, bmptemp2, g3);
						break;
					case 7:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 4);
						imageQuad(wpX-11, wpY-11, 8, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 3);
						imageQuad(wpX-11, wpY-11, 6, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 2);
						imageQuad(wpX-11, wpY-11, 4, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 1);
						imageQuad(wpX-11, wpY-11, 2, bmptemp2, g3);
						break;
					case 8:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 4);
						imageQuad(wpX-11, wpY-11, 6, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 3);
						imageQuad(wpX-11, wpY-11, 4, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 2);
						imageQuad(wpX-11, wpY-11, 2, bmptemp2, g3);
						break;
					case 9:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 4);
						imageQuad(wpX-11, wpY-11, 4, bmptemp2, g3);
						bmptemp2 = xvtMask(bmptemp, IFF, 3);
						imageQuad(wpX-11, wpY-11, 2, bmptemp2, g3);
						break;
					case 10:
						bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
						bmptemp2 = xvtMask(bmptemp, IFF, 4);
						imageQuad(wpX-11, wpY-11, 2, bmptemp2, g3);
						break;
					case 11:
						if (IFF == 0) { sb.Color = Color.FromArgb(0, 0xE0, 0); sb2.Color = Color.FromArgb(0, 0x78, 0); }	// green
						else if (IFF == 2) { sb.Color = Color.FromArgb(0, 0, 0xE0); sb2.Color = Color.FromArgb(0, 0, 0x78); }	// blue
						else if (IFF == 3) { sb.Color = Color.FromArgb(0xE0, 0xE0, 0); sb2.Color = Color.FromArgb(0x78, 0x78, 0); }	// yellow
						else if (IFF == 5) { sb.Color = Color.FromArgb(0xE0, 0, 0xE0); sb2.Color = Color.FromArgb(0x78, 0, 0x78); }	// purple
						g3.FillRectangle(sb, wpX - (pos[0]/2), wpY - (pos[1]/2), pos[0], pos[1]);
						g3.FillRectangle(sb2, wpX - (pos[0]/2-1), wpY - (pos[1]/2-1), pos[0]-2, pos[1]-2);
						break;
					default:
						// 12 or greater, just the box
						if (IFF == 0) { sb.Color = Color.FromArgb(0, 0xE0, 0); sb2.Color = Color.FromArgb(0, 0x78, 0); }	// green
						else if (IFF == 2) { sb.Color = Color.FromArgb(0, 0, 0xE0); sb2.Color = Color.FromArgb(0, 0, 0x78); }	// blue
						else if (IFF == 3) { sb.Color = Color.FromArgb(0xE0, 0xE0, 0); sb2.Color = Color.FromArgb(0x78, 0x78, 0); }	// yellow
						else if (IFF == 5) { sb.Color = Color.FromArgb(0xE0, 0, 0xE0); sb2.Color = Color.FromArgb(0x78, 0, 0x78); }	// purple
						g3.FillRectangle(sb, wpX - (pos[0]/2+2), wpY - (pos[1]/2+2), pos[0]+4, pos[1]+4);
						g3.FillRectangle(sb2, wpX - (pos[0]/2+1), wpY - (pos[1]/2+1), pos[0]+2, pos[1]+2);
						break;
				}
			}
			#endregion // FG tags
			#region text tags
			for (int i=0;i<8;i++)
			{
				if (_textTags[i, 0] == -1) continue;
				sb = new SolidBrush(Color.FromArgb(0xA8, 0, 0));	// default to red
				int clr = _textTags[i, 3];
				if (clr == 0) sb.Color = Color.FromArgb(0, 0xAC, 0);	// green
				//else if (clr == 1) sb.Color = Color.FromArgb(0xA8, 0, 0);	// red
				else if (clr == 2) sb.Color = Color.FromArgb(0xA8, 0xAC, 0);	// yellow
				else if (clr == 3) sb.Color = Color.FromArgb(0, 0x2C, 0xA8);	// blue
				else if (clr == 4) sb.Color = Color.FromArgb(0xA8, 0, 0xA8);	// purple
				else if (clr == 5) sb.Color = Color.Black;	// black, although this is just retarded against a near-black BG
				g3.DrawString(_tags[_textTags[i, 0]], new Font("MS Reference Sans Serif", 6), sb, (int)Math.Round((double)_zoomX*_textTags[i, 1]/256, 0) + X, (int)Math.Round((double)_zoomY*_textTags[i, 2]/256, 0) + Y);
			}
			#endregion	// text tags
			for (int i=0;i<_briefData.Length;i++)
			{
                wp = _briefData[i].WaypointArr[briefIndex];
				if (wp[3] != 1) continue;
				bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft]);
				bmptemp = xvtMask(bmptemp, _briefData[i].IFF, 0);
				int[] pos = getTagSize(_briefData[i].Craft);
				// simple base-256 grid coords * zoom to get pixel location, + map offset, - pic size/2 to center
                g3.DrawImageUnscaled(bmptemp, (int)Math.Round((double)_zoomX*wp[0]/256, 0) + X - 11 + (pos[0]%2), (int)Math.Round((double)_zoomY*-wp[1]/256, 0) + Y - 11); //[JB] Invert Y axis. Also fixed Y axis zoom (was using X).
			}
			g3.DrawString("#" + _page, new Font("Arial", 8), new SolidBrush(Color.White), w-20, 4);
			pctBrief.Invalidate();		// since it's drawing to memory, this refreshes the pct.  Removes the flicker when zooming
			g3.Dispose();
		}
		Bitmap xwaMask(Bitmap craftImage, byte iff)
		{
			Bitmap bmpNew = new Bitmap(craftImage);
			BitmapData bmData = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride*bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			#region define IFF color distribution
			byte[] rgb = new byte[3];
			switch (iff)
			{
				case 0:		// green
					rgb[0] = 0x40;
					rgb[1] = 0xBC;
					rgb[2] = 0x20;
					break;
				case 2:		// blue
					rgb[0] = 0x68;
					rgb[1] = 0x8C;
					rgb[2] = 0xF8;
					break;
				case 3:		// yellow
					rgb[0] = 0xE8;
					rgb[1] = 0xD0;
					rgb[2] = 0x40;
					break;
				case 5:		// purple
					rgb[0] = 0xF8;
					rgb[1] = 0x80;
					rgb[2] = 0xF8;
					break;
				default:	// red
					rgb[0] = 0xF8;
					rgb[1] = 0x54;
					rgb[2] = 0x50;
					break;
			}
			#endregion
			for (int y = 0;y < bmpNew.Height;y++)
			{
				for (int x = 0, pos = y*bmData.Stride;x < bmpNew.Width;x++)
				{
					pix[pos+x*3] = (byte)((pix[pos+x*3] * rgb[2]) / 256);
					pix[pos+x*3+1] = (byte)((pix[pos+x*3+1] * rgb[1]) / 256);
					pix[pos+x*3+2] = (byte)((pix[pos+x*3+2] * rgb[0]) / 256);
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
			int X = 2*(-_zoomX*_mapX/256) + w/2;	// values are written out like this to force even numbers
			int Y = 2*(-_zoomY*_mapY/256) + h/2;
			Pen pn = new Pen(Color.FromArgb(0x50, 0, 0));
			pn.Width = 1;
			Graphics g3;
			g3 = Graphics.FromImage(_map);		//graphics obj, load from the memory bitmap
			g3.Clear(SystemColors.Control);		//clear it
			SolidBrush sb = new SolidBrush(Color.FromArgb(0x18, 0x18, 0x18));
			g3.FillRectangle(sb, 0, 0, w, h);
			if (_message != "")
			{
				sb.Color = Color.FromArgb(0xE7, 0xE3, 0);	// yellow
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				g3.DrawString(_message, new Font("Arial", 12, FontStyle.Bold), sb, w/2, h/2, sf);
				pctBrief.Invalidate();		// since it's drawing to memory, this refreshes the pct.  Removes the flicker when zooming
				g3.Dispose();
				return;
			}
			drawGrid(X, Y, g3);
			Bitmap bmptemp;
			#region FG tags
			for (int i=0;i<8;i++)
			{
				if (_fgTags[i, 0] == -1 || _briefData[_fgTags[i, 0]].Waypoint == null || _briefData[_fgTags[i, 0]].Waypoint[3] != 1) continue;  //[JB] Added null check to fix crash.
				sb = new SolidBrush(Color.FromArgb(0xE0, 0, 0));	// defaults to Red
				SolidBrush sb2 = new SolidBrush(Color.FromArgb(0x78, 0, 0));
				byte IFF = _briefData[_fgTags[i, 0]].IFF;
				if (IFF == 0) { sb.Color = Color.FromArgb(0, 0xE0, 0); sb2.Color = Color.FromArgb(0, 0x78, 0); }	// green
				else if (IFF == 2) { sb.Color = Color.FromArgb(0x60, 0x60, 0xE0); sb2.Color = Color.FromArgb(0x20, 0x20, 0x78); }	// blue
				else if (IFF == 3) { sb.Color = Color.FromArgb(0xE0, 0xE0, 0); sb2.Color = Color.FromArgb(0x78, 0x78, 0); }	// yellow
				else if (IFF == 5) { sb.Color = Color.FromArgb(0xE0, 0, 0xE0); sb2.Color = Color.FromArgb(0x78, 0, 0x78); }	// purple
				int wpX = (int)Math.Round((double)_zoomX*_briefData[_fgTags[i, 0]].Waypoint[0]/256, 0) + X;
				int wpY = (int)Math.Round((double)_zoomY*_briefData[_fgTags[i, 0]].Waypoint[1]/256, 0) + Y;
				int frame = hsbTimer.Value - _fgTags[i, 1];
				if (_fgTags[i, 1] == 0) frame = 12;	// if tagged at t=0, just the box
				byte r = sb.Color.R, b = sb.Color.B, g = sb.Color.G;
				bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i, 0]].Craft]);
				bmptemp = xwaMask(bmptemp, IFF);
				if (_briefData[_fgTags[i, 0]].Waypoint[2] == 1) bmptemp.RotateFlip(RotateFlipType.Rotate270FlipNone);
				else if (_briefData[_fgTags[i, 0]].Waypoint[2] == 2) bmptemp.RotateFlip(RotateFlipType.Rotate180FlipNone);
				else if (_briefData[_fgTags[i, 0]].Waypoint[2] == 3) bmptemp.RotateFlip(RotateFlipType.Rotate90FlipNone);
				else if (_briefData[_fgTags[i, 0]].Waypoint[2] == 4) bmptemp.RotateFlip(RotateFlipType.RotateNoneFlipX);
				switch (frame)
				{
					case 0:
						imageQuad(wpX-28, wpY-28, 16, bmptemp, g3);
						break;
					case 1:
						imageQuad(wpX-28, wpY-28, 16, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 14, bmptemp, g3);
						break;
					case 2:
						imageQuad(wpX-28, wpY-28, 16, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 14, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 12, bmptemp, g3);
						break;
					case 3:
						imageQuad(wpX-28, wpY-28, 16, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 14, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 12, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 10, bmptemp, g3);
						break;
					case 4:
						imageQuad(wpX-28, wpY-28, 14, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 12, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 10, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 8, bmptemp, g3);
						break;
					case 5:
						imageQuad(wpX-28, wpY-28, 12, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 10, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 8, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 6, bmptemp, g3);
						break;
					case 6:
						imageQuad(wpX-28, wpY-28, 10, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 8, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 6, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 4, bmptemp, g3);
						break;
					case 7:
						imageQuad(wpX-28, wpY-28, 8, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 6, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 4, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 2, bmptemp, g3);
						break;
					case 8:
						g3.FillRectangle(sb2, wpX - 12, wpY - 11, 25, 22);
						imageQuad(wpX-28, wpY-28, 6, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 4, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 2, bmptemp, g3);
						break;
					case 9:
						if (r == 0xE0) r = 0x90;
						else if (r == 0x60) r = 0x30;
						if (g == 0xE0) r = 0x94;
						if (b == 0xE0) b = 0x90;
						else if (b == 0x60) b = 0x30;
						sb.Color = Color.FromArgb(r, g, b);
						g3.FillRectangle(sb, wpX - 13, wpY - 12, 27, 24);
						g3.FillRectangle(sb2, wpX - 12, wpY - 11, 25, 22);
						imageQuad(wpX-28, wpY-28, 4, bmptemp, g3);
						imageQuad(wpX-28, wpY-28, 2, bmptemp, g3);
						break;
					case 10:
						if (r == 0xE0) r = 0xA8;
						else if (r == 0x60) r = 0x40;
						if (g == 0xE0) r = 0xAC;
						if (b == 0xE0) b = 0xA8;
						else if (b == 0x60) b = 0x40;
						sb.Color = Color.FromArgb(r, g, b);
						g3.FillRectangle(sb, wpX - 14, wpY - 13, 29, 26);
						g3.FillRectangle(sb2, wpX - 13, wpY - 12, 27, 24);
						imageQuad(wpX-28, wpY-28, 2, bmptemp, g3);
						break;
					case 11:
						if (r == 0xE0) r = 0xC8;
						else if (r == 0x60) r = 0x50;
						if (g == 0xE0) r = 0xC8;
						if (b == 0xE0) b = 0xC8;
						else if (b == 0x60) b = 0x50;
						sb.Color = Color.FromArgb(r, g, b);
						g3.FillRectangle(sb, wpX - 15, wpY - 16, 31, 28);
						g3.FillRectangle(sb2, wpX - 14, wpY - 13, 29, 26);
						break;
					default:
						// 12 or greater, just the box
						g3.FillRectangle(sb, wpX - 17, wpY - 16, 35, 32);
						g3.FillRectangle(sb2, wpX - 16, wpY - 15, 33, 30);
						break;
				}
			}
			#endregion // FG tags
			#region text tags
			for (int i=0;i<8;i++)
			{
				if (_textTags[i, 0] == -1) continue;
				sb = new SolidBrush(Color.FromArgb(0xE7, 0, 0));	// default to red
				int clr = _textTags[i, 3];
				if (clr == 0) sb.Color = Color.FromArgb(0, 0xE3, 0);	// green
				//else if (clr == 1) sb.Color = Color.FromArgb(0xE7, 0, 0);	// red
				else if (clr == 2) sb.Color = Color.FromArgb(0xE7, 0xE3, 0);	// yellow
				else if (clr == 3) sb.Color = Color.FromArgb(0x63, 0x61, 0xE7);	// purple
				else if (clr == 4) sb.Color = Color.FromArgb(0xDE, 0, 0xDE);	// pink
				else if (clr == 5) sb.Color = Color.FromArgb(0, 4, 0xA5);	// blue
				g3.DrawString(_tags[_textTags[i, 0]], new Font("Arial", 9, FontStyle.Bold), sb, (int)Math.Round((double)_zoomX*_textTags[i, 1]/256, 0) + X, (int)Math.Round((double)_zoomY*_textTags[i, 2]/256, 0) + Y);
			}
			#endregion
			for (int i=0;i<_briefData.Length;i++)
			{
				if (_briefData[i].Waypoint == null || _briefData[i].Waypoint[3] != 1) continue;
				bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft]);
				bmptemp = xwaMask(bmptemp, _briefData[i].IFF);
				if (_briefData[i].Waypoint[2] == 1) bmptemp.RotateFlip(RotateFlipType.Rotate270FlipNone);
				else if (_briefData[i].Waypoint[2] == 2) bmptemp.RotateFlip(RotateFlipType.Rotate180FlipNone);
				else if (_briefData[i].Waypoint[2] == 3) bmptemp.RotateFlip(RotateFlipType.Rotate90FlipNone);
				else if (_briefData[i].Waypoint[2] == 4) bmptemp.RotateFlip(RotateFlipType.RotateNoneFlipX);
				// simple base-256 grid coords * zoom to get pixel location, + map offset, - pic size/2 to center
				g3.DrawImageUnscaled(bmptemp, (int)Math.Round((double)_zoomX*_briefData[i].Waypoint[0]/256, 0) + X - 28, (int)Math.Round((double)_zoomY*_briefData[i].Waypoint[1]/256, 0) + Y - 28);  //[JB] Fixed Y zoom.
			}
			g3.DrawString("#" + _page, new Font("Arial", 8), new SolidBrush(Color.White), w-20, 4);
			pctBrief.Invalidate();		// since it's drawing to memory, this refreshes the pct.  Removes the flicker when zooming
			g3.Dispose();
		}

		void cboColorTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			_textTags[(int)numText.Value-1, 3] = cboColorTag.SelectedIndex;
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
					_icon = (short)cboMoveIcon.SelectedIndex;
				}
			}
			MapPaint();
		}
		void cboNCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_briefData[_icon].Craft = cboNCraft.SelectedIndex-1;
			if (cboNCraft.SelectedIndex == 0) _briefData[_icon].Waypoint[3] = 0;
			MapPaint();
		}
		void cboNewIcon_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_briefData[_icon] = _tempBD;
			_icon = (short)cboNewIcon.SelectedIndex;
			_tempBD = _briefData[_icon];
			_briefData[_icon].Craft = cboNCraft.SelectedIndex-1;
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
			_textTags[(int)numText.Value-1, 0] = cboTextTag.SelectedIndex;
			MapPaint();
		}

		void cmdBreak_Click(object sender, EventArgs e)
		{
			_eventType = BaseBriefing.EventType.PageBreak;
			_page++;
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
			if (_eventType == BaseBriefing.EventType.PageBreak && sender.ToString() != "OK") { _page--; }
			else if (_eventType == BaseBriefing.EventType.TextTag1 && sender.ToString() != "OK") { _textTags = _tempTags; }
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
            BaseBriefing brief = (_platform == Settings.Platform.TIE ? (BaseBriefing)_tieBriefing : (_platform == Settings.Platform.XvT ? (BaseBriefing)_xvtBriefing : (BaseBriefing)_xwaBriefing));
            if(hasAvailableEventSpace(2 + brief.EventParameterCount((int)_eventType)) == false) //Check space for a full event
            {
                MessageBox.Show("Event list is full, cannot add more.", "Error");
                cmdCancel_Click(0, new EventArgs());
                return;
            }

            if (_eventType == BaseBriefing.EventType.ClearFGTags) if (optText.Checked) _eventType = BaseBriefing.EventType.ClearTextTags;
			int i = -1;
			switch (_eventType)
			{
				case BaseBriefing.EventType.PageBreak:
					#region page break
					i = findExisting(_eventType);
					if (i < 10000) { _page--; break; }	// no further action, existing break found
					i -= 10000;
					try
					{
						lstEvents.SelectedIndex = i;	// this will throw for last event
						insertEvent();
					}
					catch (ArgumentOutOfRangeException)
					{
						lstEvents.Items.Add("");
						for (int n=i+2;n>i;n--)
						{
							if (_events[n-1, 1] == 0) continue;
							for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					for (int n=2;n<6;n++) _events[i, n] = 0;
					if (_platform == Settings.Platform.TIE) lblTitle.Text = "";
					lblCaption.Text = "";
					break;
					#endregion
				case BaseBriefing.EventType.TitleText:
					#region title
					i = findExisting(_eventType);
					if (i >= 10000)  //[JB] Need to change all these conditional checks to >= 10000.  If the event list is empty, none will be found, returning exactly 10000 (which isn't caught) and produces an out of bounds exception trying to insert at _events[10000,0]
					{
						i -= 10000;	// if one wasn't found, remove marker, create it.
						try
						{
							lstEvents.SelectedIndex = i;
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = (short)cboText.SelectedIndex;
					for (int n=3;n<6;n++) _events[i, n] = 0;
					if (_strings[_events[i, 2]].StartsWith(">"))
					{
						lblTitle.TextAlign = ContentAlignment.TopCenter;
						lblTitle.ForeColor = _titleColor;
						lblTitle.Text = _strings[_events[i, 2]].Replace(">", "");
					}
					else
					{
						lblTitle.TextAlign = ContentAlignment.TopLeft;
						lblTitle.ForeColor = _normalColor;
						lblTitle.Text = _strings[_events[i, 2]];
					}
					break;
					#endregion
				case BaseBriefing.EventType.CaptionText:
					#region caption
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i -= 10000;	// if one wasn't found, remove marker, create it.
						try
						{
							lstEvents.SelectedIndex = i;
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = (short)cboText.SelectedIndex;
					for (int n=3;n<6;n++) _events[i, n] = 0;
					if (_strings[_events[i, 2]].StartsWith(">"))
					{
						lblCaption.TextAlign = ContentAlignment.TopCenter;
						lblCaption.ForeColor = _titleColor;
						lblCaption.Text = _strings[_events[i, 2]].Replace(">", "");
					}
					else
					{
						lblCaption.TextAlign = ContentAlignment.TopLeft;
						lblCaption.ForeColor = _normalColor;
						lblCaption.Text = _strings[_events[i, 2]];
					}
					break;
					#endregion
				case BaseBriefing.EventType.MoveMap:
					#region move
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i -= 10000;	// if one wasn't found, remove marker, create it.
						try
						{
							lstEvents.SelectedIndex = i;
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = _mapX;
					_events[i, 3] = _mapY;
					// don't need to repaint, done while adjusting values
					break;
					#endregion
				case BaseBriefing.EventType.ZoomMap:
					#region zoom
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i -= 10000;	// if one wasn't found, remove marker, create it.
						try
						{
							lstEvents.SelectedIndex = i;
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = _zoomX;
					_events[i, 3] = _zoomY;
					// don't need to repaint, done while adjusting values
					break;
					#endregion
				case BaseBriefing.EventType.ClearFGTags:
					#region clear FG
					i = findExisting(_eventType);
					if (i < 10000) break;	// no further action, existing break found
					i -= 10000;
					try
					{
						lstEvents.SelectedIndex = i;	// this will throw for last event
						insertEvent();
					}
					catch (ArgumentOutOfRangeException)
					{
						lstEvents.Items.Add("");
						for (int n=i+2;n>i;n--)
						{
							if (_events[n-1, 1] == 0) continue;
							for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					for (int n=2;n<6;n++) _events[i, n] = 0;
					for (int n=0;n<8;n++)
					{
						_fgTags[n, 0] = -1;
						_fgTags[n, 1] = 0;
					}
					break;
					#endregion
				case BaseBriefing.EventType.FGTag1:
					#region FG
					_eventType = (BaseBriefing.EventType)((int)_eventType + numFG.Value - 1);
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i -= 10000;	// if one wasn't found, remove marker, create it.
						try
						{
							lstEvents.SelectedIndex = i;
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = (short)cboFGTag.SelectedIndex;
					for (int n=3;n<6;n++) _events[i, n] = 0;
					_fgTags[(int)_eventType-9, 0] = _events[i, 2];
					_fgTags[(int)_eventType-9, 1] = _events[i, 0];
					MapPaint();
					break;
					#endregion
				case BaseBriefing.EventType.ClearTextTags:
					#region clear text
					i = findExisting(_eventType);
					if (i < 10000) break;	// no further action, existing break found
					i -= 10000;
					try
					{
						lstEvents.SelectedIndex = i;	// this will throw for last event
						insertEvent();
					}
					catch (ArgumentOutOfRangeException)
					{
						lstEvents.Items.Add("");
						for (int n=i+2;n>i;n--)
						{
							if (_events[n-1, 1] == 0) continue;
							for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					for (int n=2;n<6;n++) _events[i, n] = 0;
					for (int n=0;n<8;n++)
					{
						_textTags[n, 0] = -1;
						_textTags[n, 1] = 0;
					}
					break;
					#endregion
				case BaseBriefing.EventType.TextTag1:
					#region text
					_eventType = (BaseBriefing.EventType)((int)_eventType + numText.Value - 1);
					// can't use FindExisting, due to extra parameter
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						if (_tempX == -621 && _tempY == -621)
						{
							MessageBox.Show("No tag location selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							i = 0;
							break;
						}
						i -= 10000;	// if one wasn't found, remove marker, create it.
						try
						{
							lstEvents.SelectedIndex = i;
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
					}
					else
					{
						// found existing, just see if we change location or not
						if (_tempX == -621 && _tempY == -621)
						{
							_tempX = _events[i, 3];
							_tempY = _events[i, 4];
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = (short)cboTextTag.SelectedIndex;
					_events[i, 3] = _tempX;
					_events[i, 4] = _tempY;
					_events[i, 5] = (short)cboColorTag.SelectedIndex;
					// don't need to repaint or restore/edit from backup, as it's taken care of during placement
					break;
					#endregion
				case BaseBriefing.EventType.XwaNewIcon:
					#region new icon
					if (_tempX == -621 && _tempY == -621 && cboNCraft.SelectedIndex == 0)
					{
						MessageBox.Show("No craft location selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						i = 0;
						break;
					}
					// start with the NewIcon command
					i = findNext();
					try
					{
						lstEvents.SelectedIndex = i;	// this will throw for last event
						insertEvent();
					}
					catch (ArgumentOutOfRangeException)
					{
						lstEvents.Items.Add("");
						for (int n=i+2;n>i;n--)
						{
							if (_events[n-1, 1] == 0) continue;
							for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = _icon;
					_events[i, 3] = (short)cboNCraft.SelectedIndex;
					_events[i, 4] = (short)cboIconIff.SelectedIndex;
					_events[i, 5] = 0;
					updateList(i);
					// and now the MoveIcon 
					if (cboNCraft.SelectedIndex != 0)
					{
						i = findNext();
						try
						{
							lstEvents.SelectedIndex = i;	// this will throw for last event
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
						_events[i, 0] = (short)hsbTimer.Value;
						_events[i, 1] = (short)BaseBriefing.EventType.XwaMoveIcon;
						_events[i, 2] = _icon;
						_events[i, 3] = _tempX;
						_events[i, 4] = _tempY;
						_events[i, 5] = 0;
					}
					break;
					#endregion
				case BaseBriefing.EventType.XwaShipInfo:
					#region info
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i -= 10000;
						try
						{
							lstEvents.SelectedIndex = i;	// this will throw for last event
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = (short)(optInfoOn.Checked ? 1 : 0);
					_events[i, 3] = (short)cboInfoCraft.SelectedIndex;
					for (int n=4;n<6;n++) _events[i, n] = 0;
					break;
					#endregion
				case BaseBriefing.EventType.XwaMoveIcon:
					#region move icon
					if (_tempX == -621 && _tempY == -621)
					{
						MessageBox.Show("No craft location or valid icon selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						i = 0;
						break;
					}
					if (numMoveTime.Value == 0)
					{
						i = findNext();		// could be lots of Moves at one time
						try
						{
							lstEvents.SelectedIndex = i;	// this will throw for last event
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
						_events[i, 0] = (short)hsbTimer.Value;
						_events[i, 1] = (short)_eventType;
						_events[i, 2] = _icon;
						_events[i, 3] = _briefData[_icon].Waypoint[0];
						_events[i, 4] = _briefData[_icon].Waypoint[1];
						_events[i, 5] = 0;
					}
					else
					{
						int t0 = hsbTimer.Value, x = _briefData[_icon].Waypoint[0], y = _briefData[_icon].Waypoint[1];
						int total = (int)Math.Round(numMoveTime.Value * _timerInterval);
						for (int j=0;j<=total;j++)
						{
							i = findNext(j + t0);
							try
							{
								lstEvents.SelectedIndex = i;	// this will throw for last event
								insertEvent();
							}
							catch (ArgumentOutOfRangeException)
							{
								lstEvents.Items.Add("");
								for (int n=i+2;n>i;n--)
								{
									if (_events[n-1, 1] == 0) continue;
									for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
								}
							}
							_events[i, 0] = (short)(j + t0);
							_events[i, 1] = (short)_eventType;
							_events[i, 2] = _icon;
							_events[i, 3] = (short)((x-_tempX) * j / total + _tempX);
							_events[i, 4] = (short)((y-_tempY) * j / total + _tempY);
							_events[i, 5] = 0;
							updateList(i);
						}
					}
					break;
					#endregion
				case BaseBriefing.EventType.XwaRotateIcon:
					#region rotate
					i = findNext();		// could be lots of Rotates at one time
					try
					{
						lstEvents.SelectedIndex = i;	// this will throw for last event
						insertEvent();
					}
					catch (ArgumentOutOfRangeException)
					{
						lstEvents.Items.Add("");
						for (int n=i+2;n>i;n--)
						{
							if (_events[n-1, 1] == 0) continue;
							for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = _icon;
					_events[i, 3] = (short)cboRotateAmount.SelectedIndex;
					for (int n=4;n<6;n++) _events[i, n] = 0;
					break;
					#endregion
				case BaseBriefing.EventType.XwaChangeRegion:
					#region region
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						i -= 10000;
						try
						{
							lstEvents.SelectedIndex = i;	// this will throw for last event
							insertEvent();
						}
						catch (ArgumentOutOfRangeException)
						{
							lstEvents.Items.Add("");
							for (int n=i+2;n>i;n--)
							{
								if (_events[n-1, 1] == 0) continue;
								for (int h=0;h<6;h++) _events[n, h] = _events[n-1, h];
							}
						}
					}
					_events[i, 0] = (short)hsbTimer.Value;
					_events[i, 1] = (short)_eventType;
					_events[i, 2] = (short)(numNewRegion.Value - 1);
					for (int n=3;n<6;n++) _events[i, n] = 0;
					break;
					#endregion
				default:	// this shouldn't be possible
					break;
			}
			lstEvents.SelectedIndex = i;
			updateList(i);
			cmdCancel_Click("OK", new System.EventArgs());
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
			_eventType = BaseBriefing.EventType.XwaNewIcon;
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
			_tempTags = _textTags;
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

		/// <summary>Feature to quickly navigate forward through tbe briefing, landing on the next caption.</summary>
		void lblCaption_Click(object sender, EventArgs e)
		{
			int time = hsbTimer.Value;
			for (int i = 0; i < _maxEvents; i++)
			{
				int etime = _events[i, 0];
				int eevt = _events[i, 1];
				if (eevt == (int)BaseBriefing.EventType.None || eevt == (int)BaseBriefing.EventType.EndBriefing)
				{
					hsbTimer.Value = 1;
					hsbTimer.Value = 0;
					return;
				}
				if (etime > time && eevt == (int)BaseBriefing.EventType.CaptionText)
				{
					if (etime < hsbTimer.Maximum)
					{
						hsbTimer.Value = etime;
						hsbTimer.Value = etime + 1;
					}
					return;
				}
			}
		}

		void numText_ValueChanged(object sender, EventArgs e)
		{
			_textTags = _tempTags;	// restore and re-edit
			_textTags[(int)numText.Value-1, 0] = cboTextTag.SelectedIndex;
			_textTags[(int)numText.Value-1, 1] = _tempX;
			_textTags[(int)numText.Value-1, 2] = _tempY;
			_textTags[(int)numText.Value-1, 3] = cboColorTag.SelectedIndex;
			MapPaint();
		}

        void pctBrief_MouseUp(object sender, MouseEventArgs e)
		{
            if(e.Button != MouseButtons.Left)
            {
                if(e.Button == MouseButtons.Middle)
                    _isMiddleDrag = false;

                popupPreviewStop();
            }
        }
		void pctBrief_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button.ToString() != "Left")
            {
                if(e.Button == MouseButtons.Middle)
                {
                    _isMiddleDrag = true;
                    _popupMiddle.X = e.X;
                    _popupMiddle.Y = e.Y;
                }
                popupPreviewStart();
                pctBrief_MouseMove(0, e); //Simulate mouse move to refresh and display the data
                return;
            }
			if (_eventType == BaseBriefing.EventType.TextTag1)
			{
				int mod = (_platform != Settings.Platform.TIE ? 2 : 1);
				_textTags = _tempTags;	// restore backup before messing with it again
				_tempX = (short)(128 * e.X / _zoomX * mod - 64 * w / _zoomX * mod + _mapX);
				_tempY = (short)(128 * e.Y / _zoomY * mod - 64 * h / _zoomY * mod + _mapY);
				_textTags[(int)numText.Value-1, 0] = cboTextTag.SelectedIndex;
				_textTags[(int)numText.Value-1, 1] = _tempX;
				_textTags[(int)numText.Value-1, 2] = _tempY;
				_textTags[(int)numText.Value-1, 3] = cboColorTag.SelectedIndex;
				MapPaint();
			}
			else if (_eventType == BaseBriefing.EventType.XwaNewIcon)
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
            if (_popupPreviewActive)
            {
                if (_isMiddleDrag)
                {
                    double scx = (w / _zoomX) * 0.75;  //[JB] zoom level is pixels per km.  Modified to be more consistent across zoom levels.
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
                if (_platform != Settings.Platform.XWA)
                    s += "\nWaypoint Coords (km): " + xkm.ToString() + " , " + ykm.ToString();
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
        }
        void pctBrief_Paint(object sender, PaintEventArgs e)
		{
			Graphics objGraphics;
			//You can't modify e.Graphics directly.
			objGraphics = e.Graphics;
			// Draw the contents of the bitmap on the form.
			objGraphics.DrawImage(_map, 0, 0, _map.Width, _map.Height);
		}

		void txtLength_TextChanged(object sender, EventArgs e)
		{
			short t_Length;
			switch (_platform)
			{
				case Settings.Platform.TIE:
					try
					{
						t_Length = (short)Math.Round(Convert.ToDecimal(txtLength.Text) * _timerInterval,0);	// this is the line that could throw
						_tieBriefing.Length = t_Length;
						hsbTimer.Maximum = _tieBriefing.Length + 11;
						if (Math.Round(((decimal)_tieBriefing.Length / _timerInterval), 2) != Convert.ToDecimal(txtLength.Text))	// so things like .51 become .5, without
							txtLength.Text = Convert.ToString(Math.Round(((decimal)_tieBriefing.Length / _timerInterval), 2));	// wiping out just a decimal
					}
					catch  { txtLength.Text = Convert.ToString(Math.Round(((decimal)_tieBriefing.Length / _timerInterval), 2)); }
					break;
				case Settings.Platform.XvT:
					try
					{
						t_Length = (short)Math.Round(Convert.ToDecimal(txtLength.Text) * _timerInterval, 0);	// this is the line that could throw
						_xvtBriefing.Length = t_Length;
						hsbTimer.Maximum = _xvtBriefing.Length + 11;
						if (Math.Round(((decimal)_xvtBriefing.Length / _timerInterval), 2) != Convert.ToDecimal(txtLength.Text))	// so things like .51 become .5, without
							txtLength.Text = Convert.ToString(Math.Round(((decimal)_xvtBriefing.Length / _timerInterval), 2));	// wiping out just a decimal
					}
					catch { txtLength.Text = Convert.ToString(Math.Round(((decimal)_xvtBriefing.Length / _timerInterval), 2)); }
					break;
				case Settings.Platform.XWA:
					try
					{
						t_Length = (short)Math.Round(Convert.ToDecimal(txtLength.Text) * _timerInterval, 0);	// this is the line that could throw
						_xwaBriefing.Length = t_Length;
						hsbTimer.Maximum = _xwaBriefing.Length + 11;
						if (Math.Round(((decimal)_xwaBriefing.Length / _timerInterval), 2) != Convert.ToDecimal(txtLength.Text))	// so things like .51 become .5, without
							txtLength.Text = Convert.ToString(Math.Round(((decimal)_xwaBriefing.Length / _timerInterval), 2));	// wiping out just a decimal
					}
					catch { txtLength.Text = Convert.ToString(Math.Round(((decimal)_xwaBriefing.Length / _timerInterval), 2)); }
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
			for (int i=0;i<_strings.Length;i++)
			{
				cboString.Items.Add(_strings[i]);
				cboText.Items.Add(_strings[i]);
			}
		}
		void loadTags()
		{
			cboTag.Items.Clear();
			cboTextTag.Items.Clear();
			for (int i=0;i<_tags.Length;i++)
			{
				cboTag.Items.Add(_tags[i]);
				cboTextTag.Items.Add(_tags[i]);
			}
		}

		void dataS_CurrentCellChanged(object sender, EventArgs e)
		{
			if (_platform == Settings.Platform.XWA) txtNotes.Text = _xwaBriefing.BriefingStringsNotes[dataS.CurrentCell.RowNumber];
		}

		void tableStrings_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i=0;
			for (int j=0;j<_strings.Length;j++)
			{
				if (_tableStrings.Rows[j].Equals(e.Row))
				{
					i = j;
					break;
				}
			}
			_strings[i] = _tableStrings.Rows[i][0].ToString();
			loadStrings();
		}
		void tableTags_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			int i=0;
			for(int j=0;j<_tags.Length;j++)
			{
				if (_tableTags.Rows[j].Equals(e.Row))
				{
					i = j;
					break;
				}
			}
			_tags[i] = _tableTags.Rows[i][0].ToString();
			loadTags();
		}

		void txtNotes_Leave(object sender, EventArgs e)
		{
			_xwaBriefing.BriefingStringsNotes[dataS.CurrentCell.RowNumber] = txtNotes.Text;
		}
		#endregion	tabStrings
		#region tabEvents
        
        /// <summary>Tally briefing events and make sure there's enough space in the raw briefing array.</summary>
        bool hasAvailableEventSpace(int requestedParams)
        {
            BaseBriefing brief = (_platform == Settings.Platform.TIE ? (BaseBriefing)_tieBriefing : (_platform == Settings.Platform.XvT ? (BaseBriefing)_xvtBriefing : (BaseBriefing)_xwaBriefing));
            int paramCount = 2;  //Reserve space for the ending command
            for(int j = 0; j < lstEvents.Items.Count; j++)
            {
                paramCount += 2 + brief.EventParameterCount(_events[j, 1]);
                if(paramCount >= brief.Events.Length)
                    return false;
            }
            if(paramCount + requestedParams >= brief.Events.Length)
                return false;
            
            return true;
        }
		void insertEvent()
		{
			// create a new item @ SelectedIndex, 
            int i = lstEvents.SelectedIndex;
			if (i == -1) i = 0;
			lstEvents.Items.Insert(i, "");
			for (int j=_maxEvents-1;j>i;j--)
			{
				if (_events[j-1, 1] == 0) continue;
				for (int h=0;h<6;h++) _events[j, h] = _events[j-1, h];
			}
			_events[i, 0] = _events[i+1, 0];
			if (_events[i, 0] == 9999) _events[i, 0] = 0;
			_events[i, 1] = 3;
			for (int j=2;j<6;j++) _events[i, j] = 0;
			lstEvents.SelectedIndex = i;
		}
		/// <summary>Swaps one briefing event index with another.</summary>
		void swapEvent(int index1, int index2)
		{
			short t;
			for (int j = 0; j < 6; j++)
			{
				t = _events[index1, j];
				_events[index1, j] = _events[index2, j];
				_events[index2, j] = t;
			}
		}
		/// <summary>Shifts briefing events by swapping the contents of the origin index in a linear path until it occupies the end index.</summary>
		void shiftEvents(int origin, int end)
		{
			if (end > origin)  //swap downward
			{
				for (int i = origin; i < end; i++)
				{
					swapEvent(i, i + 1);
				}
			}
			else if (end < origin)  //swap upward
			{
				for (int i = origin; i > end; i--)
				{
					swapEvent(i, i - 1);
				}
			}
		}
		void updateList(int index)
		{
			if (index == -1) return;
			string temp = String.Format("{0,-8:0.00}", (decimal)_events[index, 0] / _timerInterval);
			temp += cboEvent.Items[_events[index, 1]-3].ToString();
			if (_events[index, 1] == (int)BaseBriefing.EventType.TitleText || _events[index, 1] == (int)BaseBriefing.EventType.CaptionText)
			{
				if (_strings[_events[index, 2]].Length > 30) temp += ": \"" + _strings[_events[index, 2]].Substring(0, 30) + "...\"";
				else temp += ": \"" + _strings[_events[index, 2]] + '\"';
			}
			else if (_events[index, 1] == (int)BaseBriefing.EventType.MoveMap || _events[index, 1] == (int)BaseBriefing.EventType.ZoomMap) { temp += ": X:" + _events[index, 2] + " Y:" + _events[index, 3]; }
			else if (_events[index, 1] >= (int)BaseBriefing.EventType.FGTag1 && _events[index, 1] <= (int)BaseBriefing.EventType.FGTag8)
            {
                //[JB] This fixed a potential crash while I working on functionality to delete FGs.  Not sure if still needed.
                int fgIndex = _events[index, 2]; 
                if(fgIndex >= _briefData.Length)
                {
                    fgIndex = 0;
                    _events[index, 2] = 0;
                }
                temp += ": " + ((_platform != Settings.Platform.XWA) ? _briefData[fgIndex].Name : "Icon #" + fgIndex);   //[JB] Modified XWA to show Icon # since it doesn't have FG names.
            }
			else if (_events[index, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[index, 1] <= (int)BaseBriefing.EventType.TextTag8)
			{
				if (_tags[_events[index, 2]].Length > 30) temp += ": \"" + _tags[_events[index, 2]].Substring(0, 30) + "...\"";
				else temp += ": \"" + _tags[_events[index, 2]] + '\"';
			}
			else if (_events[index, 1] == (int)BaseBriefing.EventType.XwaNewIcon) { temp += " #" + _events[index, 2] + ": Craft: " + Platform.Xwa.Strings.CraftType[_events[index, 3]] + " IFF: " + cboIFF.Items[_events[index, 4]].ToString(); }
			else if (_events[index, 1] == (int)BaseBriefing.EventType.XwaShipInfo)
			{
				if (_events[index, 2] == 1) temp += ": Icon # " + _events[index, 3] + " State: On";
				else temp += ": Icon # " + _events[index, 3] + " State: Off";
			}
			else if (_events[index, 1] == (int)BaseBriefing.EventType.XwaMoveIcon) { temp += " #" + _events[index, 2] + ": X:" + _events[index, 3] + " Y:" + _events[index, 4]; }
			else if (_events[index, 1] == (int)BaseBriefing.EventType.XwaRotateIcon) { temp += " #" + _events[index, 2] + ": " + cboRotate.Items[_events[index, 3]]; }
			else if (_events[index, 1] == (int)BaseBriefing.EventType.XwaChangeRegion) { temp += " #" + (_events[index, 2]+1); }
			lstEvents.Items[index] = temp;
			if (!_loading)
			{
				_loading = true;
				lstEvents.SelectedIndex = index;
				_loading = false;
				MapPaint();		// to update things like tags, strings, etc
			}
		}
		void updateParameters()
		{
            Control currentControl = ActiveControl;  //[JB] Maintain the current control's focus.  Basically a hack to maintain tab order since refreshing other controls will steal focus.  I want it to at least maintain tab order between the X/Y num boxes.
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
			if (_events[i, 1] == (int)BaseBriefing.EventType.TitleText || _events[i, 1] == (int)BaseBriefing.EventType.CaptionText)
			{
				try { cboString.SelectedIndex = _events[i, 2]; }
				catch
				{
					cboString.SelectedIndex = 0;
					_events[i, 2] = 0;
				}
				_events[i, 3] = 0;
				_events[i, 4] = 0;
				_events[i, 5] = 0;
				cboString.Enabled = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.MoveMap || _events[i, 1] == (int)BaseBriefing.EventType.ZoomMap)
			{
				numX.Value = _events[i, 2];
				numY.Value = _events[i, 3];
				_events[i, 4] = 0;
				_events[i, 5] = 0;
				numX.Enabled = true;
				numY.Enabled = true;
			}
			else if (_events[i, 1] >= (int)BaseBriefing.EventType.FGTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.FGTag8)
			{
				try { cboFG.SelectedIndex = _events[i, 2]; }
				catch
				{
					cboFG.SelectedIndex = 0;
					_events[i, 2] = 0;
				}
				_events[i, 3] = 0;
				_events[i, 4] = 0;
				_events[i, 5] = 0;
				cboFG.Enabled = true;
			}
			else if (_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8)
			{
				try
				{
					cboTag.SelectedIndex = _events[i, 2];
					cboColor.SelectedIndex = _events[i, 5];
				}
				catch
				{
					cboTag.SelectedIndex = 0;
					cboColor.SelectedIndex = 0;
					_events[i, 2] = 0;
					_events[i, 3] = 0;
					_events[i, 4] = 0;
					_events[i, 5] = 0;
				}
				numX.Value = _events[i, 3];
				numY.Value = _events[i, 4];
				cboTag.Enabled = true;
				cboColor.Enabled = true;
				numX.Enabled = true;
				numY.Enabled = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaNewIcon)
			{
				cboFG.SelectedIndex = _events[i, 2];
				cboCraft.SelectedIndex = _events[i, 3];
				cboIFF.SelectedIndex = _events[i, 4];
				cboCraft.Enabled = true;
				cboIFF.Enabled = true;
				cboFG.Enabled = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaShipInfo)
			{
				optOn.Checked = Convert.ToBoolean(_events[i, 2]);
				optOff.Checked = !optOn.Checked;
				cboFG.SelectedIndex = _events[i, 3];
				cboFG.Enabled = true;
				optOff.Enabled = true;
				optOn.Enabled = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaMoveIcon)
			{
				cboFG.SelectedIndex = _events[i, 2];
				numX.Value = _events[i, 3];
				numY.Value = _events[i, 4];
				cboFG.Enabled = true;
				numX.Enabled = true;
				numY.Enabled = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaRotateIcon)
			{
				cboFG.SelectedIndex = _events[i, 2];
				cboRotate.SelectedIndex = _events[i, 3];
				cboFG.Enabled = true;
				cboRotate.Enabled = true;
			}
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaChangeRegion)
			{
				numRegion.Value = _events[i, 2]+1;
				numRegion.Enabled = true;
			}
			_loading = false;
            ActiveControl = currentControl;
		}

		void cboColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8) _events[i, 5] = (short)cboColor.SelectedIndex;
			updateList(i);
		}
		void cboCraft_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] == (int)BaseBriefing.EventType.XwaNewIcon) _events[i, 3] = (short)cboCraft.SelectedIndex;
			updateList(i);
		}
		void cboEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (_loading || i == -1 || cboEvent.SelectedIndex == -1) return;

            BaseBriefing brief = (_platform == Settings.Platform.TIE ? (BaseBriefing)_tieBriefing : (_platform == Settings.Platform.XvT ? (BaseBriefing)_xvtBriefing : (BaseBriefing)_xwaBriefing));
            int oldEventSize = 2 + brief.EventParameterCount(_events[i, 1]);
            int newEventSize = 2 + brief.EventParameterCount((short)(cboEvent.SelectedIndex + 3));
            if(hasAvailableEventSpace(newEventSize - oldEventSize) == false)
            {
                MessageBox.Show("Cannot change Event Type because the briefing list is full and the replaced event needs more space than is available.", "Error");
                return;
            }
			_events[i, 1] = (short)(cboEvent.SelectedIndex + 3);
			updateParameters();
			updateList(i);
		}
		void cboFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if ((_events[i, 1] >= (int)BaseBriefing.EventType.FGTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.FGTag8) || _events[i, 1] == (int)BaseBriefing.EventType.XwaNewIcon
				|| _events[i, 1] == (int)BaseBriefing.EventType.XwaMoveIcon || _events[i, 1] == (int)BaseBriefing.EventType.XwaRotateIcon) _events[i, 2] = (short)cboFG.SelectedIndex;
			else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaShipInfo) _events[i, 3] = (short)cboFG.SelectedIndex;
			updateList(i);
		}
		void cboIFF_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] == (int)BaseBriefing.EventType.XwaNewIcon) _events[i, 4] = (short)cboIFF.SelectedIndex;
			updateList(i);
		}
		void cboRotate_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] == (int)BaseBriefing.EventType.XwaRotateIcon) _events[i, 3] = (short)cboRotate.SelectedIndex;
			updateList(i);
		}
		void cboString_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] == (int)BaseBriefing.EventType.TitleText || _events[i, 1] == (int)BaseBriefing.EventType.CaptionText) _events[i, 2] = (short)cboString.SelectedIndex;
			updateList(i);
		}
		void cboTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8) _events[i, 2] = (short)cboTag.SelectedIndex;
			updateList(i);
		}

		void cmdDelete_Click(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1) return;
			lstEvents.Items.RemoveAt(i);
			for (int j=i;j<_maxEvents-1;j++)
			{
				if (_events[j, 1] == 0) break;
				for (int h=0;h<6;h++) _events[j, h] = _events[j+1, h];
			}
			try { lstEvents.SelectedIndex = i; }
			catch { lstEvents.SelectedIndex = i-1; }
		}
		void cmdDown_Click(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == lstEvents.Items.Count-1 || i == -1) return; //[JB] Fixed crash if using commands before an event is selected
			short[] t = new short[6];
			for (int j=0;j<6;j++) t[j] = _events[i+1, j];
			for (int j=0;j<6;j++) _events[i+1, j] = _events[i, j];
			for (int j=0;j<6;j++) _events[i, j] = t[j];
			string item = lstEvents.Items[i].ToString();
			lstEvents.Items[i] = lstEvents.Items[i+1];
			lstEvents.Items[i+1] = item;
			lstEvents.SelectedIndex = i+1;
		}
		void cmdNew_Click(object sender, EventArgs e)
		{
            if(hasAvailableEventSpace(6) == false)  //Check space for a full event
            {
                MessageBox.Show("Event list is full, cannot add more.", "Error");
                return;
            }
            insertEvent();
            //[JB] Changed to insert after current element.  insertEvents() inserts before, and can't be modified without breaking all the other places that use it.
            //Instead: insert before, then swap.
            if (lstEvents.SelectedIndex + 1 < lstEvents.Items.Count)
            {
                int index = lstEvents.SelectedIndex;
                swapEvent(index, index + 1);
                updateList(index + 1);   //updateList() changes lstEvents.SelectedIndex but doesn't seem to refresh the parameters controls with the selected item.  Update in reverse order so that a forced refresh will succeed.
                updateList(index);
                lstEvents.SelectedIndex = index + 1;
            }
            else
            {
                updateList(lstEvents.SelectedIndex);  //If only one item exists, refresh that.
            }
        }
		void cmdUp_Click(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1) return;
			short[] t = new short[6];
			for (int j=0;j<6;j++) t[j] = _events[i-1, j];
			for (int j=0;j<6;j++) _events[i-1, j] = _events[i, j];
			for (int j=0;j<6;j++) _events[i, j] = t[j];
			string item = lstEvents.Items[i].ToString();
			lstEvents.Items[i] = lstEvents.Items[i-1];
			lstEvents.Items[i-1] = item;
			lstEvents.SelectedIndex = i-1;
		}

		void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			_loading = true;
			numTime.Value = _events[i, 0];
			cboEvent.SelectedIndex = _events[i, 1] - 3;
			_loading = false;
			updateParameters();
			try
			{
				if (_events[i-1, 0] == _events[i, 0]) cmdUp.Enabled = true;
				else cmdUp.Enabled = false;
			}
			catch { cmdUp.Enabled = false; }
			try
			{
				if (_events[i+1, 0] == _events[i, 0]) cmdDown.Enabled = true;
				else cmdDown.Enabled = false;
			}
			catch { cmdDown.Enabled = false; }
		}

		void numRegion_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] == (int)BaseBriefing.EventType.XwaChangeRegion) _events[i, 2] = (short)(numRegion.Value - 1);
			updateList(i);
		}
        void numTime_ValueChanged(object sender, EventArgs e)
		{
            lblEventTime.Text = String.Format("{0:= 0.00 seconds}", numTime.Value / _timerInterval);
            int size = lstEvents.Items.Count;
            int i = lstEvents.SelectedIndex;
            if (_loading || i == -1) return;
            _loading = true;

            short diff = (short)(numTime.Value - _events[i, 0]);
            _events[i, 0] = (short)numTime.Value;

            int p = i;
            if (diff > 0)  //Positive change, moving down in the list
            {
                if (i < size - 1 && _events[i, 0] != _events[i + 1, 0])  //If equal time to next entry, no need to sort
                {
                    p = i + 1;  //Step over to prevent comparing to self
                    while (p < size && _events[p, 0] < _events[i, 0]) p++;  //Search until a greater time index is found
                    p--;  //If found, insert before.  If not found (p=size) adjusts to last slot in array.
                }
            }
            else if(diff < 0)  //Negative change, moving up in the list
            {
                if (i > 0 && _events[i, 0] != _events[i-1, 0])    //If equal time to previous entry, no need to sort
                {
                    p = i - 1;
                    while (p >= 0 && _events[p, 0] > _events[i, 0]) p--;   //Search until a lesser time index is found
                    p++;  //If found, insert after.  If not found (p=-1) adjusts to first slot in array.
                }
            }
            if (diff != 0)
            {
                shiftEvents(i, p);
                lstEvents.Items.RemoveAt(i);
                lstEvents.Items.Insert(p, "");
                lstEvents.SelectedIndex = p;
                updateList(p);
            }

            _loading = false;
            try
            {
                if (_events[i - 1, 0] == _events[i, 0]) cmdUp.Enabled = true;
                else cmdUp.Enabled = false;
            }
            catch { cmdUp.Enabled = false; }
            try
            {
                if (_events[i + 1, 0] == _events[i, 0]) cmdDown.Enabled = true;
                else cmdDown.Enabled = false;
            }
            catch { cmdDown.Enabled = false; }
		}
		void numX_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] == (int)BaseBriefing.EventType.MoveMap || _events[i, 1] == (int)BaseBriefing.EventType.ZoomMap) _events[i, 2] = (short)numX.Value;
			else if ((_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8)
				|| _events[i, 1] == (int)BaseBriefing.EventType.XwaMoveIcon) _events[i, 3] = (short)numX.Value;
			updateList(i);
		}
		void numY_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] == (int)BaseBriefing.EventType.MoveMap || _events[i, 1] == (int)BaseBriefing.EventType.ZoomMap) _events[i, 3] = (short)numY.Value;
			else if ((_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8)
				|| _events[i, 1] == (int)BaseBriefing.EventType.XwaMoveIcon) _events[i, 4] = (short)numY.Value;
			updateList(i);
		}

		void optOn_CheckedChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			if (_events[i, 1] == (int)BaseBriefing.EventType.XwaShipInfo) _events[i, 2] = (short)(optOn.Checked ? 1 : 0);
			updateList(i);
		}
		#endregion tabEvents
		#region tabTeams
		/// <summary>Performs the necessary data saving and swapping when selecting a new Briefing.</summary>
		void changeBriefingIndex(int briefIndex)
		{
			if (_loading) return;

			if (briefIndex == _currentCollectionIndex) return;
			if (briefIndex > cboBriefIndex1.Items.Count)
				briefIndex = cboBriefIndex1.Items.Count - briefIndex;

			Save();  //Save current changes before switching

			_currentCollectionIndex = briefIndex;
			if (_platform == Settings.Platform.XvT)
			{
				_xvtBriefing = _xvtBriefingCollection[briefIndex];
				//These select lines copied from the constructor to re-load and re-init particular data sets and controls.
				_events = new short[_maxEvents, 6];
				_tags = _xvtBriefing.BriefingTag;
				_strings = _xvtBriefing.BriefingString;
				importStrings();
				txtLength.Text = Convert.ToString(Math.Round(((decimal)_xvtBriefing.Length / _timerInterval), 2));
				lstEvents.Items.Clear();
				importEvents(_xvtBriefing.Events);
				numUnk1.Value = _xvtBriefing.Unknown1;
				numUnk3.Value = _xvtBriefing.Unknown3;
				cboText.SelectedIndex = 0;
				cboFGTag.SelectedIndex = 0;
				cboTextTag.SelectedIndex = 0;
				cboColorTag.SelectedIndex = 0;
			}
			else if (_platform == Settings.Platform.XWA)
			{
				_xwaBriefing = _xwaBriefingCollection[briefIndex];
				//These select lines copied from the constructor to re-load and re-init particular data sets and controls.
				_events = new short[_maxEvents, 6];
				_tags = _xwaBriefing.BriefingTag;
				_strings = _xwaBriefing.BriefingString;
				importStrings();
				txtLength.Text = Convert.ToString(Math.Round(((decimal)_xwaBriefing.Length / _timerInterval), 2));
				lstEvents.Items.Clear();
				importEvents(_xwaBriefing.Events);
				numUnk1.Value = _xwaBriefing.Unknown1;
				numUnk3.Enabled = false;
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

			//Refresh controls
			if (lstEvents.Items.Count > 0)
				lstEvents.SelectedIndex = 0;
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
				if (team)
					lstTeams.SelectedIndex = i;
			}
			_loading = temp;
		}
		/// <summary>For multi-briefing platforms (XvT and XWA) this updates the BriefingForm title with the currently selected briefing and applicable teams.</summary>
		/// <remarks>The title is a convenient location to display this information regardless of which tab is selected.</remarks>
		void updateTitle()
		{
			if (_platform == Settings.Platform.TIE)
				return;
			int teamIndex = cboBriefIndex1.SelectedIndex;
			if (teamIndex < 0) teamIndex = 0;
			string title = Text;
			string update = "Briefing #" + (teamIndex + 1);
			string team = "";
			int pos = title.IndexOf("   [");
			if (pos >= 0)
				title = title.Remove(pos);

			if (_platform == Settings.Platform.XvT)
			{
				for (int i = 0; i < _xvtBriefing.Team.Length; i++)
				{
					if (_xvtBriefing.Team[i])
					{
						if (team.Length > 0)
							team += ", ";
						team += sharedTeamNames[i];
					}
				}
			}
			else if (_platform == Settings.Platform.XWA)
			{
				for (int i = 0; i < _xwaBriefing.Team.Length; i++)
				{
					if (_xwaBriefing.Team[i])
					{
						if (team.Length > 0)
							team += ", ";
						team += sharedTeamNames[i];
					}
				}
			}
			if (team == "")
				team = "NO TEAMS";

			title += "   [" + update + " for: " + team + "]";
			Text = title;
		}

		/// <summary>Switches currently visible team briefing.</summary>
		void cboBriefIndex1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_loading == true) return;
            _loading = true;
           cboBriefIndex2.SelectedIndex = cboBriefIndex1.SelectedIndex;
            _loading = false;

           changeBriefingIndex(cboBriefIndex1.SelectedIndex);
        }
        void cboBriefIndex2_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(_loading == true) return;
            _loading = true;
           cboBriefIndex1.SelectedIndex = cboBriefIndex2.SelectedIndex;
            _loading = false;

           changeBriefingIndex(cboBriefIndex2.SelectedIndex);
        }

        void lstTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_loading)
                return;

            int index = cboBriefIndex1.SelectedIndex;
            if(index == -1)
                return;

            if(_platform == Settings.Platform.TIE)
                return;

            bool[] teams = (_platform == Settings.Platform.XvT ? _xvtBriefing.Team : _xwaBriefing.Team);

            ListBox.SelectedIndexCollection sel = lstTeams.SelectedIndices;
            
            for(int i = 0; i < 10; i++)
            {
                bool found = false;
                for(int k = 0; k < sel.Count; k++)
                {
                    if(sel[k] == i)
                    {
                        teams[i] = true;
                        found = true;

                        //Each team can only see one briefing, so iterate through the other briefings and deselect them.
                        if(_platform == Settings.Platform.XvT)
                        {
                            for(int j = 0; j < _xvtBriefingCollection.Count; j++)
                            {
                                if(j != index)
                                    _xvtBriefingCollection[j].Team[i] = false;
                            }
                        }
                        if(_platform == Settings.Platform.XWA)
                        {
                            for(int j = 0; j < _xwaBriefingCollection.Count; j++)
                            {
                                if(j != index)
                                    _xwaBriefingCollection[j].Team[i] = false;
                            }
                        }
                        break;
                    }
                }
                if(!found)
                    teams[i] = false;
            }
            updateTitle();
        }
		#endregion tabTeams
	}

	public struct BriefData
	{
		public int Craft;
		public short[] Waypoint;
        public BaseFlightGroup.BaseWaypoint[] WaypointArr;  //[JB] Augmented with a list of briefing waypoints for multi-briefing platforms like XvT.  But the normal short[] array is retained for compatibility and ease of use with existing code.  The augmented data is only used when specifically needed.
		public byte IFF;
		public string Name;
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
	 */
}