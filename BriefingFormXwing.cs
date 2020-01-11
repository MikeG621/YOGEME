/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2020 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.5+
 */

/* CHANGELOG
 * [UPD] some formatting stuff
 * [NEW #30] onModified callback to prevent mission from auto-dirty when opening
 * v1.5, 180910
 * [NEW] created [JB]
 * [UPD] tweaked layout size
 */


/* [JB] A note on changed functions:
 * I originally tried altering the code in such a way that it could potentially be
 * merged into the TIE/XvT/XWA briefing, but after seeing that so many core functions
 * needed to be changed, I decided to abandon that idea.  It was theoretically possible
 * but seemed to require far more work and overhead then it was worth, particularly since
 * I was constantly shuffling certain things around and wasn't sure on the final layout
 * and new functions I would need.  I stripped away most of the unused functions and form
 * controls.  Some of the altered functions kept certain code blocks or platform checks
 * intact to maintain the logic in the event that they are ever merged in the future.
 * */

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Idmr.Common;
using Idmr.Platform;
using Idmr.Platform.Xwing;

namespace Idmr.Yogeme
{
	/// <summary>The X-wing briefing form for YOGEME.  The X-wing briefing is similar enough to re-use the Briefing form of the other platforms, but different enough that we can't easily integrate it with the other form without lots of hacks to accomodate the slightly different instruction set and multi-page format.</summary>
	public partial class BriefingFormXwing : Form
	{
		#region Vars
		BriefData[] _briefData;
		BriefData _tempBD;
		Briefing _xwingBriefing;
		bool _loading = false;
		Color _normalColor;
		Color _highlightColor;
		Color _titleColor;
		short[,] _events;	// this will contain the event listing for use, raw data is in Briefing.Events[]

        //New for X-wing
        int currentPage = 0;    // The current briefing page currently being edited.
        bool resizeNeeded = false;

		short _zoomX = 48;
		short _zoomY;
		int w, h;
		short _mapX, _mapY;	// mapX and mapY will be different, namely the grid coordinates of the center, like how TIE handles it
		Bitmap _map;
		int[,] _fgTags = new int[8, 2];	// [#, 0=FG/Icon 1=Time]
		int[,] _textTags = new int[8, 4];	// [#, X, Y, color]
		DataTable _tableTags = new DataTable("Tags");
		int _timerInterval;
		Briefing.EventType _eventType;
		short _tempX, _tempY;
		int[,] _tempTags;
		Settings.Platform _platform;
		string[] _tags;
		string[] _strings;
		int _maxEvents;
		int _regionDelay = -1;
		int _page = 1;
		short _icon = 0;
        bool _popupPreviewActive = false; //[JB] New feature
        Timer _popupTimer = new Timer();
        short _popupPreviewZoomX;
        short _popupPreviewZoomY;
        short _popupPreviewMapX;
        short _popupPreviewMapY;
        bool _popupDragState = false;
        int _popupMiddleX;
        int _popupMiddleY;
        Timer _mapPaintRedrawTimer = new Timer();  //[JB] Added a timer to control map painting in an attempt to smooth re-drawing performance.
        bool _mapPaintScheduled = false;      //True if a paint is scheduled, that is a paint request is called while a paint is already in progress.
        int _previousTimeIndex = 0;           //Tracks the previous time index of the briefing so we can detect when the user is manually scrolling through arbitrary times.
		EventHandler onModified = null;
		#endregion

		public BriefingFormXwing(FlightGroupCollection fg, Briefing briefing, EventHandler onModifiedCallback)
		{
			_loading = true;
			_platform = Settings.Platform.XWING;
			_titleColor = Color.FromArgb(0xFC, 0xFC, 0x54);
			_normalColor = Color.FromArgb(0xFC, 0xFC, 0xFC);
			_highlightColor = Color.FromArgb(0x00, 0xA8, 0x00);
			_zoomY = _zoomX;			// in most cases, these will remain the same
            _xwingBriefing = briefing;
			_maxEvents = Briefing.EventQuantityLimit;
			_events = new short[_maxEvents,6];
			InitializeComponent();
			Text = "YOGEME Briefing Editor - X-wing";
			#region layout edit
			// final layout update, as in VS it's spread out
			Height = 426;
			Width = 760;
			tabBrief.Width = 752;
			Point loc = new Point(608, 188);
			pnlShipTag.Location = loc;
			pnlTextTag.Location = loc;
			#endregion

            //Need to replace the strings before the event list is imported.
            cboEvent.Items.Clear();
            cboEvent.Items.AddRange(_xwingBriefing.GetUsableEventTypeStrings());

            Import(fg);	// FGs are separate so they can be updated without running the BRF as well
			importDat(Application.StartupPath + "\\images\\TIE_BRF.dat", 34);
			_tags = _xwingBriefing.BriefingTag;
			_strings = _xwingBriefing.BriefingString;
			importStrings();
			_timerInterval = Briefing.TicksPerSecond;
			//txtLength.Text = Don't set length here, SetCurrentPage() will load the correct value.
			hsbTimer.Maximum = _xwingBriefing.Length + 11;
            int newWidth = 420;
            //Reduce the width and adjust to re-center it
            pctBrief.Location = new Point(pctBrief.Location.X + ((pctBrief.Width - newWidth) / 2), pctBrief.Location.Y);  //584 from the width of the control, 
            pctBrief.Width = newWidth;
            lblTitle.Location = new Point(lblTitle.Location.X + ((lblTitle.Width - newWidth) / 2), lblTitle.Location.Y);
            lblTitle.Width = newWidth;
            lblCaption.Location = new Point(lblCaption.Location.X + ((lblCaption.Width - newWidth) / 2), lblCaption.Location.Y);
            lblCaption.Width = newWidth;
            w = pctBrief.Width;
			h = pctBrief.Height;
            lblTitle.Font = new Font("Times New Roman", 15);
            lblCaption.Font = new Font("Times New Roman", 15);
            lblCaption.Padding = new Padding(72, 0, 0, 0);  //Approximated the padding to the left based on a similar ratio in game.
            lblCaption.Click += cmdNextCaption_Click;
			_mapX = 0;
			_mapY = 0;
            rebuildPageList();
            setCurrentPage(0);  //Set the working page to grab the event list before importing

            lstEvents.Items.Clear();
            importEvents(_xwingBriefing.Events);
			hsbTimer.Value = 0;
			cboText.SelectedIndex = 0;
			cboFGTag.SelectedIndex = 0;
			cboTextTag.SelectedIndex = 0;
			cboColorTag.SelectedIndex = 0;


            lstViewport.Items.Clear();
            lstViewport.Items.AddRange(Strings.BriefingUIElement);
            lstViewport.SelectedIndex = 0;

            refreshPageTypes();
            cboPageType.SelectedIndex = 0;
            cboPageAddType.SelectedIndex = 0;
            cboPageAddTitle.SelectedIndex = 0;
            cboPageAddCaption.SelectedIndex = 0;

            numUItop.Leave += grpUI_Leave;
            numUIleft.Leave += grpUI_Leave;
            numUIbottom.Leave += grpUI_Leave;
            numUIright.Leave += grpUI_Leave;
            chkUIvisible.CheckedChanged += grpUI_Leave;

            numPageCoordSet.Minimum = 1;
            numPageCoordSet.Maximum = briefing.MaxCoordSet;
            cboMissionLocation.Items.AddRange(Strings.MissionLocation);
            cboMissionLocation.SelectedIndex = briefing.MissionLocation;
            for (int i = 2; i <= 4; i++)
                cboMaxCoordSet.Items.Add(i.ToString());
            cboMaxCoordSet.SelectedIndex = briefing.MaxCoordSet - 2;

            lstString.SelectedIndex = 0;
            
            _loading = false;
			onModified = onModifiedCallback;
			postLoadInit();
        }

        void postLoadInit()
        {
            _popupTimer.Tick += popupTimer_Tick;
            _mapPaintRedrawTimer.Tick += mapPaintRedrawTimer_Tick;
            //_paintRedrawTimer.Elapsed += PaintRedrawTimerEvent; //new EventHandler(PaintRedrawTimerEvent);

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

		public void Import(FlightGroupCollection fg)
		{
			_briefData = new BriefData[fg.Count];
			cboFG.Items.Clear();
			cboFGTag.Items.Clear();
			for (int i = 0; i < fg.Count; i++)
            {
                int craftType = fg[i].GetTIECraftType();
                byte iff = fg[i].GetTIEIFF();
                if (fg[i].IFF == 0 && fg[i].IsObjectGroup() && fg[i].ObjectType != 25) iff = 1; //None/Default objects (except B-Wing icon, type 25) appear as Imperial.
                if (iff > 2) iff = 2;            //Higher IFF neutrals should always draw blue.
                if (craftType == 0x56) iff = 1;  //Asteroids should appear as Imperial.
                else if (craftType == 0x57) iff = 0;  //Planets should appear as Rebel.
                fillBriefData(i, craftType, fg[i].Waypoints[0], fg[i].Waypoints, iff, fg[i].Name);
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
				Bitmap bm = new Bitmap(count * size, size, PixelFormat.Format24bppRgb);
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
							if (_platform == Settings.Platform.XWING)
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
		BaseBriefing getBriefing()
		{
		    //[JB] Retained for compatibility with code structure.  The idea was that virtual functions and overrides could be used to call code that was specific to X-wing or the other platforms.
			return _xwingBriefing;
		}
		void importEvents(short[] rawEvents)
		{
            BaseBriefing brief = getBriefing();

			int offset = 0;
			for (int i=0;i<_maxEvents;i++)
			{
				_events[i, 0] = rawEvents[offset++];		// time
				_events[i, 1] = rawEvents[offset++];		// event
				if (_events[i, 1] == 0 || _events[i, 1] == (short)Briefing.EventType.EndBriefing) break;
				else
				{
					for (int j = 2; j < 2 + brief.EventParameterCount(_events[i, 1]); j++, offset++) _events[i, j] = rawEvents[offset];
                    //Removed XWA code
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
            }
            _tableTags.Clear();  //Erase existing strings in case switching Briefings.
            lstString.Items.Clear();
			for (int i=0;i<_tags.Length;i++)
			{
				DataRow dr = _tableTags.NewRow();
				dr[0] = _tags[i];
				_tableTags.Rows.Add(dr);
                lstString.Items.Add(_strings[i]);
			}
			dataTags.Table = _tableTags;
			dataT.DataSource = dataTags;
			_tableTags.RowChanged += new DataRowChangeEventHandler(tableTags_RowChanged);
			loadTags();
			loadStrings();
		}

		public void Save()
		{
            if (_platform == Settings.Platform.XWING)
            {
                int offset = 0;
                for (int evnt = 0; evnt < _maxEvents; evnt++)
                {
                    for (int i = 0; i < 2; i++, offset++) _xwingBriefing.Events[offset] = _events[evnt, i];
                    if (_events[evnt, 1] == (short)Briefing.EventType.EndBriefing) break;
                    else
                    {
                        int eid = _events[evnt, 1];
                        if (eid == (short)Briefing.EventType.None) break;
                        int pcount = _xwingBriefing.EventParameterCount(eid);
                        for (int i = 2; i < 2 + pcount; i++, offset++)
                            _xwingBriefing.Events[offset] = _events[evnt, i];
                    }
                }
                saveCurrentPage();
				if (onModified != null) onModified("XW Save", new EventArgs());
			}
		}

        void saveCurrentPage()
        {
			//Saves the event list we're currently editing back into the Briefing.
			BriefingPage pg = _xwingBriefing.GetBriefingPage(currentPage);
            Array.Copy(_xwingBriefing.Events, pg.Events, pg.Events.Length);
            pg.EventsLength = (short)_xwingBriefing.GetEventsLength(currentPage);
        }
        void setCurrentPage(int index)
        {
            //Just in case, make sure there's a default page that exists to edit
            if (_xwingBriefing.pages.Count == 0)
                _xwingBriefing.ResetPages(1);

            if (index < 0)
                return;

            if (index != currentPage)
                Save();
            currentPage = index;
			BriefingPage pg = _xwingBriefing.GetBriefingPage(currentPage);
            Array.Copy(pg.Events, _xwingBriefing.Events, pg.Events.Length);

            lstEvents.Items.Clear();
            importEvents(_xwingBriefing.Events);
            updateTitle();

            bool btemp = _loading;
            _loading = true;
            cboSelectPage1.SelectedIndex = index;
            cboSelectPage2.SelectedIndex = index;
            _loading = btemp;

            txtLength.Text = Convert.ToString(Math.Round(((decimal)pg.Length / _timerInterval), 2));

            refreshDisplayElements();
        }
        void updateTitle()
        {
            string title = Text;
            string prefix = "   (Now Editing Page ";
            string update = prefix + (currentPage + 1) + " of " + lstPages.Items.Count + ")";
            int pos = title.IndexOf(prefix);
            if (pos >= 0)
                title = title.Remove(pos);

            title += update;
			Text = title;
        }

		void tabBrief_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (tabBrief.SelectedIndex != 0) hsbTimer.Value = 1;
            else
            {
                if (resizeNeeded == true)
                {
                    refreshDisplayElements(); //If any of the page UI settings were updated, make sure they take effect when switching back.
                    resizeNeeded = false;
                }
                hsbTimer.Value = 0;	// force refresh, since pct doesn't want to update when hidden
            }
		}

		#region frmBrief
		void frmBrief_Activated(object sender, EventArgs e) { MapPaint(); }
        void frmBrief_FormClosed(object sender, FormClosedEventArgs e)
        {
            tabBrief.Focus();       //[JB] Leave any focused controls so that events don't refresh the disposed map.
            _popupTimer.Dispose();
            _mapPaintRedrawTimer.Dispose();
            _map.Dispose();
        }
        void frmBrief_FormClosing(object sender, FormClosingEventArgs e)
		{
			Save();
            _popupTimer.Stop(); //[JB] Stop and deactivate the timers.  Hopefully this fixes a rare exception (possibly a race condition?) where the event would still try to repaint the map even after everything was disposed.
            _popupTimer.Tick -= popupTimer_Tick;
            _mapPaintRedrawTimer.Stop();
            _mapPaintRedrawTimer.Tick -= mapPaintRedrawTimer_Tick;
			onModified = null;
		}
		void frmBrief_Load(object sender, EventArgs e)
		{
			for (int i=0;i<8;i++) _fgTags[i, 0] = -1;
			for (int i=0;i<8;i++) _textTags[i, 0] = -1;
            _map = new Bitmap(w, h, PixelFormat.Format24bppRgb);
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
            BaseBriefing brief = getBriefing();

            bool paint = false;
            if (hsbTimer.Value != 0 && ((hsbTimer.Value - _previousTimeIndex >= 2) || hsbTimer.Value <= _previousTimeIndex))  //A non-incremental or reverse change (if incremental the timer should be +1 to previous), the user most likely manually moved the scrollbar.  Iterate through all past events and rebuild the briefing state.
            {
                resetBriefing();
                stopTimer();
                for (int i = 0; i < _maxEvents; i++)
                {
                    //Process everything up to the current time index.
                    if (_events[i, 0] > hsbTimer.Value || _events[i, 1] == (int)BaseBriefing.EventType.None || _events[i, 1] == (int)BaseBriefing.EventType.EndBriefing) break;
                    paint |= processEvent(i);  //paint stays enabled once enabled.
                }
            }

            _previousTimeIndex = hsbTimer.Value;
			if (hsbTimer.Value == 0)
			{
                resetBriefing();
			}
			if (_regionDelay != -1)
			{
				//_message = "";
				_regionDelay = -1;
				lblCaption.Visible = true;
				lblTitle.Visible = true;
			}
			for (int i=0;i<_maxEvents;i++)
			{
				if (_events[i,0] < hsbTimer.Value) continue;
                if (_events[i, 0] > hsbTimer.Value || _events[i, 1] == (short)Briefing.EventType.None || _events[i, 1] == (short)Briefing.EventType.EndBriefing) break;
                paint |= processEvent(i);  //paint stays enabled once enabled.
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
				//_message = "";
				_regionDelay--;
				lblCaption.Visible = true;
				lblTitle.Visible = true;
			}
			else _regionDelay--;
		}
        void popupTimer_Tick(object sender, EventArgs e)
        {
            if (_popupPreviewActive == true) return;
            _popupTimer.Stop();
            lblPopupInfo.Visible = false;
            lblPopupInfo.Text = "";
        }
        void mapPaintRedrawTimer_Tick(object sender, EventArgs e)
        {
            //private void PaintRedrawTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
            //[JB] Wrapper that handles the calling of the actual map rendering.
            if (_mapPaintScheduled == true)
            {
                if (_platform == Settings.Platform.XWING) xwingPaint();  //X-wing imports the briefing in TIE format so that this editor can process it with minimal changes
                _mapPaintScheduled = false;
                _mapPaintRedrawTimer.Stop();
            }
            else  //This shouldn't trigger, but just in case nothing is scheduled, stop the timer.
            {
                _mapPaintRedrawTimer.Stop();
            }
        }
        #endregion	Timer related

		public void MapPaint()
		{
            //[JB] I modified this function to instead serve as a wrapper that handles the map rendering.  It seems to reduce immediate performance for the sake of consistency and not clogging CPU cycles during rapid re-draw attempts. 
            if(_mapPaintScheduled == false)
            {
                if (!_mapPaintRedrawTimer.Enabled)         //The timer is stopped, start it up.
                {
                    _mapPaintRedrawTimer.Start();
                    _mapPaintRedrawTimer.Interval = 17;    //Was trying to aim for ~60 FPS, but according to MSDN, the minimum accuracy is 55 ms.
                }
                _mapPaintScheduled = true;
            }
        }

		void drawGrid(int x, int y, Graphics g)
		{
            //x = ClampValue(x, 0, w);  //The Death Star missions usually have a map very far in one direction which breaks the coordinates of the lines unless we clamp them down.
            //y = ClampValue(y, 0, h);
			Pen pn = new Pen(Color.FromArgb(0x50, 0, 0));
			pn.Width = 1;
            if (_platform == Settings.Platform.XWING)
            {
                if (_xwingBriefing.MissionLocation == 0)
                    pn.Color = Color.FromArgb(0, 0x38, 0);         //Space
                else
                    pn.Color = Color.FromArgb(0x48, 0x48, 0x48);   //Death Star
                pn.Width = 1;
            }
			else if (_platform == Settings.Platform.TIE)
			{
				pn.Color = Color.FromArgb(0x48, 0, 0);
				pn.Width = 2;
			}
            int mod = (_platform == Settings.Platform.XWING || _platform == Settings.Platform.TIE ? 2 : 1);

            //Calculate where the viewport is, then find the longest span to know how many lines to iterate through.
            int x1 = (x - w) / (_zoomX * mod);
            int y1 = (y - h) / (_zoomX * mod);
            int x2 = (x + w) / (_zoomX * mod);
            int y2 = (y + h) / (_zoomX * mod);
            int min = x1, max = x2;
            if(y1 < min)
                min = y1;
            if(y2 > max)
                max = y2;
            
            if (_zoomX >= 32)
            {
                for (int i = min; i < max; i++)
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
					g.DrawLine(pn, 0, _zoomY*1*i*mod + y-1, w, _zoomY*1*i*mod + y-1);	//min lines, every zoom pixels
					g.DrawLine(pn, 0, y-1 - _zoomY*1*i*mod, w, y-1 - _zoomY*1*i*mod);
					g.DrawLine(pn, _zoomX*1*i*mod + x, 0, _zoomX*1*i*mod + x, h);
					g.DrawLine(pn, x - _zoomX*1*i*mod, 0, x - _zoomX*1*i*mod, h);
                }
			}
            else if(_zoomX >= 8)
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
            // else if (_zoomX < 8) just don't draw them
			pn.Color = Color.FromArgb(0x90, 0, 0);
            if (_platform == Settings.Platform.XWING)
            {
                if (_xwingBriefing.MissionLocation == 0)
                    pn.Color = Color.FromArgb(0, 0x78, 0);         //Space
                else
                    pn.Color = Color.FromArgb(0x78, 0x78, 0x78);   //Death Star
            }
            else if (_platform == Settings.Platform.TIE) pn.Color = Color.FromArgb(0x78, 0, 0);
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
			if (_platform == Settings.Platform.TIE || _platform == Settings.Platform.XWING) cmdTitle.Enabled = !state;
			cmdCaption.Enabled = !state;
			cmdFG.Enabled = !state;
			cmdText.Enabled = !state;
			cmdZoom.Enabled = !state;
			cmdMove.Enabled = !state;
            cmdClearText.Enabled = !state;
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
		}
		int findExisting(Briefing.EventType eventType)
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
		void xwingPaint()
		{
            if (_loading)
                return;
			int X = 2*(-_zoomX*_mapX/256) + w/2;	// values are written out like this to force even numbers
			int Y = 2*(-_zoomY*_mapY/256) + h/2;
            Pen pn = new Pen(Color.FromArgb(0, 0x48, 0));
            if (_xwingBriefing.MissionLocation == 0)
                pn.Color = Color.FromArgb(0, 0x38, 0);         //Space
            else
                pn.Color = Color.FromArgb(0x48, 0x48, 0x48);   //Death Star
			pn.Width = 1;
			Graphics g3;
			g3 = Graphics.FromImage(_map);		//graphics obj, load from the memory bitmap
			g3.Clear(SystemColors.Control);		//clear it
			SolidBrush sb = new SolidBrush(Color.Black);
			g3.FillRectangle(sb, 0, 0, w, h);
            if (_platform == Settings.Platform.XWING)
            {
                if (_xwingBriefing.MissionLocation == 1) //Death Star Surface
                {
                    sb = new SolidBrush(Color.FromArgb(0x2C, 0x30, 0x50));
                    g3.FillRectangle(sb, 0, 0, w, h);
                }
            }
			g3.DrawLine(pn, 0, 1, w, 1);
			g3.DrawLine(pn, 0, h-3, w, h-3);
			g3.DrawLine(pn, 1, 0, 1, h);
			g3.DrawLine(pn, w-1, 0, w-1, h);
			drawGrid(X, Y, g3);
			Bitmap bmptemp;
			#region FG tags
			Bitmap bmptemp2;

            int csIndex = _xwingBriefing.pages[currentPage].CoordSet;
            int wpIndex = 0; //Default to SP1
            if (csIndex >= 1 && csIndex <= 3)
                wpIndex = 7 + csIndex - 1;  //Switch to CS point.

			for (int i=0;i<8;i++)
			{
                if (_fgTags[i, 0] == -1) continue;
                sb = new SolidBrush(Color.FromArgb(0xE0, 0, 0));	// defaults to Red
                SolidBrush sb2 = new SolidBrush(Color.FromArgb(0x78, 0, 0));
                byte IFF = _briefData[_fgTags[i, 0]].IFF;
                int wpX = 2 * (int)Math.Round((double)_zoomX * _briefData[_fgTags[i, 0]].WaypointArr[wpIndex][0] / 256, 0) + X;
                int wpY = 2 * (int)Math.Round((double)_zoomY * _briefData[_fgTags[i, 0]].WaypointArr[wpIndex][1] / 256, 0) + Y;  //Y axis is not inverted
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
				sb = new SolidBrush(Color.FromArgb(0x55, 0xD1, 0xF1));	// [JB] Color code taken from screenshot.  X-wing doesn't allow colored-coded messages.
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
                g3.DrawImageUnscaled(bmptemp, 2 * (int)Math.Round((double)_zoomX * _briefData[i].WaypointArr[wpIndex][0] / 256, 0) + X - 16, 2 * (int)Math.Round((double)_zoomX * _briefData[i].WaypointArr[wpIndex][1] / 256, 0) + Y - 16);  //  //Y axis is not inverted
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

		void cmdCancel_Click(object sender, EventArgs e)
		{
            BaseBriefing brief = getBriefing();
			cboText.Enabled = false;
			optFG.Enabled = false;
			optText.Enabled = false;
			lblTitle.Visible = true;
			lblCaption.Visible = true;
			hsbBRF.Visible = false;
			vsbBRF.Visible = false;
			lblInstruction.Visible = false;
			//No PageBreak for X-wing.
            if (_eventType == Briefing.EventType.TextTag1 && sender.ToString() != "OK") { _textTags = _tempTags; }
            else if (_eventType == Briefing.EventType.MoveMap && sender.ToString() != "OK")
			{
				_mapX = _tempX;
				_mapY = _tempY;
			}
            else if (_eventType == Briefing.EventType.ZoomMap && sender.ToString() != "OK")
			{
				_zoomX = _tempX;
				_zoomY = _tempY;
			}
            //XwaRotateIcon, XwaMoveIcon removed.
			_eventType = 0;
			enableOkCancel(false);
			MapPaint();
		}
		void cmdCaption_Click(object sender, EventArgs e)
		{
			cboText.Enabled = true;
            _eventType = Briefing.EventType.CaptionText;
			enableOkCancel(true);
		}
		void cmdClear_Click(object sender, EventArgs e)
		{
			optFG.Enabled = true;
			optText.Enabled = true;
            _eventType = Briefing.EventType.ClearFGTags;
			enableOkCancel(true);
		}
		void cmdFG_Click(object sender, EventArgs e)
		{
            _eventType = Briefing.EventType.FGTag1;
			pnlShipTag.Visible = true;
			enableOkCancel(true);
            numFG.Maximum = (_platform == Settings.Platform.XWING) ? 4 : 8;
		}
		void cmdOk_Click(object sender, EventArgs e)
		{
            //[JB] Add test
            BaseBriefing brief = getBriefing();
            if(hasAvailableEventSpace(2 + brief.EventParameterCount((int)_eventType)) == false) //Check space for a full event
            {
                MessageBox.Show("Event list is full, cannot add more.", "Error");
                cmdCancel_Click(0, new EventArgs());
                return;
            }
            if (_eventType == Briefing.EventType.ClearFGTags) if (optText.Checked) _eventType = Briefing.EventType.ClearTextTags;
			int i = -1;

            // [JB] I tried changing these away from a switch() to function calls for a possible platform merge using override functions, but abandoned that idea.
			//TODO: convert these back to switch, get rid of the GOTO
            if(_eventType == Briefing.EventType.TitleText)
            {
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
				_events[i, 2] = (short)((cboText.SelectedIndex >= 0) ? cboText.SelectedIndex : 0);  //[JB] Fix exception if no string is selected in the dropdown box.
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
				#endregion
            }
            else if(_eventType == Briefing.EventType.CaptionText)
            {
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
				#endregion
            }
            else if(_eventType == Briefing.EventType.MoveMap)
            {
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
				#endregion
            }
            else if(_eventType == Briefing.EventType.ZoomMap)
            {
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
				#endregion
            }
            else if(_eventType == Briefing.EventType.ClearFGTags)
            {
				#region clear FG
				i = findExisting(_eventType);
				if (i < 10000) goto Finish;	// no further action, existing break found
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
				#endregion
            }
            else if(_eventType == Briefing.EventType.FGTag1)
            {
				#region FG
				_eventType = (Briefing.EventType)((int)_eventType + numFG.Value - 1);
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
                if (_platform != Settings.Platform.XWING)
                {
                    _fgTags[(int)_eventType - 9, 0] = _events[i, 2];
                    _fgTags[(int)_eventType - 9, 1] = _events[i, 0];
                }
                else
                {
                    _fgTags[(int)_eventType - (int)Briefing.EventType.FGTag1, 0] = _events[i, 2];
                    _fgTags[(int)_eventType - (int)Briefing.EventType.FGTag1, 1] = _events[i, 0];
                }
				MapPaint();
				#endregion
            }
            else if(_eventType == Briefing.EventType.ClearTextTags)
            {
				#region clear text
				i = findExisting(_eventType);
				if (i < 10000) goto Finish;	// no further action, existing break found
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
				#endregion
            }
            else if(_eventType == Briefing.EventType.TextTag1)
            {
				#region text
				_eventType = (Briefing.EventType)((int)_eventType + numText.Value - 1);
				// can't use FindExisting, due to extra parameter
				i = findExisting(_eventType);
				if (i >= 10000)
				{
					if (_tempX == -621 && _tempY == -621)
					{
						MessageBox.Show("No tag location selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						i = 0;
						goto Finish;
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
				_events[i, 5] = (short)cboColorTag.SelectedIndex;   //Unused for XWING but it won't be copied anyway since the event parameter count isn't large enough to copy the data.
				// don't need to repaint or restore/edit from backup, as it's taken care of during placement
				#endregion
            }
            else if (_eventType == Briefing.EventType.ClearText)
            {
                #region clear title/caption text
                i = findExisting(_eventType);
                if (i < 10000) goto Finish;	// no further action, existing break found
                i -= 10000;
                try
                {
                    lstEvents.SelectedIndex = i;	// this will throw for last event
                    insertEvent();
                }
                catch (ArgumentOutOfRangeException)
                {
                    lstEvents.Items.Add("");
                    for (int n = i + 2; n > i; n--)
                    {
                        if (_events[n - 1, 1] == 0) continue;
                        for (int h = 0; h < 6; h++) _events[n, h] = _events[n - 1, h];
                    }
                }
                _events[i, 0] = (short)hsbTimer.Value;
                _events[i, 1] = (short)_eventType;
                for (int n = 2; n < 6; n++) _events[i, n] = 0;
                #endregion
            }

            Finish:
			//[JB] Need to check for empty events. Some events fail to add if the user doesn't supply correct info (like XwaNewIcon with no location selected) and refreshing an empty list would throw an exception.
			if(lstEvents.Items.Count != 0)
			{
				lstEvents.SelectedIndex = i;
				updateList(i);
			}
			if (onModified != null) onModified("EventAdd", new EventArgs());
			cmdCancel_Click("OK", new EventArgs());
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
            _eventType = Briefing.EventType.MoveMap;
			enableOkCancel(true);
		}
		void cmdText_Click(object sender, EventArgs e)
		{
            _eventType = Briefing.EventType.TextTag1;
			pnlTextTag.Visible = true;
			lblTitle.Visible = false;
			lblCaption.Visible = false;
			lblInstruction.Visible = true;
			_tempTags = _textTags;
			_tempX = -621;
			_tempY = -621;
			enableOkCancel(true);
            numText.Maximum = (_platform != Settings.Platform.XWING) ? 8 : 4;
            cboColorTag.Visible = (_platform != Settings.Platform.XWING) ? true : false;
		}
		void cmdTitle_Click(object sender, EventArgs e)
		{
			cboText.Enabled = true;
			_eventType = Briefing.EventType.TitleText;
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
            _eventType = Briefing.EventType.ZoomMap;
			enableOkCancel(true);
		}
        void cmdClearText_Click(object sender, EventArgs e)
        {
            _eventType = Briefing.EventType.ClearText;
            enableOkCancel(true);
        }

		void hsbBRF_ValueChanged(object sender, EventArgs e)
		{
            if (_eventType == Briefing.EventType.MoveMap) _mapX = (short)hsbBRF.Value;
            if (_eventType == Briefing.EventType.ZoomMap) _zoomX = (short)hsbBRF.Value;
			MapPaint();
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
                    _popupDragState = false;

                popupPreviewStop();
            }
        }
		void pctBrief_MouseDown(object sender, MouseEventArgs e)
		{
            BaseBriefing brief = getBriefing();
			if (e.Button.ToString() != "Left")
            {
                if(e.Button == MouseButtons.Middle)
                {
                    _popupDragState = true;
                    _popupMiddleX = e.X;
                    _popupMiddleY = e.Y;
                }
                popupPreviewStart();
                pctBrief_MouseMove(0, e); //Simulate mouse move to refresh and display the data
                return;
            }
			if (_eventType == Briefing.EventType.TextTag1)
			{
				int mod = (_platform != Settings.Platform.TIE && _platform != Settings.Platform.XWING ? 2 : 1);
				_textTags = _tempTags;	// restore backup before messing with it again
				_tempX = (short)(128 * e.X / _zoomX * mod - 64 * w / _zoomX * mod + _mapX);
				_tempY = (short)(128 * e.Y / _zoomY * mod - 64 * h / _zoomY * mod + _mapY);
				_textTags[(int)numText.Value-1, 0] = cboTextTag.SelectedIndex;
				_textTags[(int)numText.Value-1, 1] = _tempX;
				_textTags[(int)numText.Value-1, 2] = _tempY;
				_textTags[(int)numText.Value-1, 3] = cboColorTag.SelectedIndex;
				MapPaint();
			}
            //Removed XwaNewIcon, XwaMoveIcon
		}
        void pctBrief_MouseMove(object sender, MouseEventArgs e)
        {
            if (_popupPreviewActive == true)
            {
                if (_popupDragState == true)
                {
                    double scx = (w / _zoomX) * 0.75;  //[JB] zoom level is pixels per km.  Modified to be more consistent across zoom levels.
                    double scy = (h / _zoomY) * 0.75;

                    int ox = (int)((e.X - _popupMiddleX) * scx);
                    int oy = (int)((e.Y - _popupMiddleY) * scy);
                    _mapX += (short)ox;
                    _mapY += (short)oy;
                    _popupMiddleX = e.X;
                    _popupMiddleY = e.Y;
                    MapPaint();
                }
                int mod = (_platform != Settings.Platform.TIE ? 2 : 1);
                int xu = (int)(128 * e.X / _zoomX * mod - 64 * w / _zoomX * mod + _mapX);
                int yu = (int)(128 * e.Y / _zoomY * mod - 64 * h / _zoomY * mod + _mapY);
                double xkm = Math.Round((double)(xu * 0.00625), 2);
                double ykm = Math.Round((double)(-yu * 0.00625), 2);
                string s = "PREVIEW ONLY\nZoom: " + _zoomX + " , " + _zoomY;
                s += "\nMap Offset: " + _mapX + " , " + _mapY;
                s += "\nMap Coords: " + xu + " , " + yu;
                if (_platform != Settings.Platform.XWA)
                    s += "\nWaypoint Coords (km): " + xkm.ToString() + " , " + ykm.ToString();
                popupUpdate(s, 0);
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
				case Settings.Platform.XWING:
					try
					{
						t_Length = (short)Math.Round(Convert.ToDecimal(txtLength.Text) * _timerInterval,0);	// this is the line that could throw
						_xwingBriefing.Length = t_Length;
						if (onModified != null) onModified("LengthChange", new EventArgs());
						BriefingPage pg = _xwingBriefing.GetBriefingPage(currentPage);
                        pg.Length = t_Length;
                        hsbTimer.Maximum = _xwingBriefing.Length + 11;
						if (Math.Round(((decimal)_xwingBriefing.Length / _timerInterval), 2) != Convert.ToDecimal(txtLength.Text))	// so things like .51 become .5, without
							txtLength.Text = Convert.ToString(Math.Round(((decimal)_xwingBriefing.Length / _timerInterval), 2));	// wiping out just a decimal
					}
					catch  { txtLength.Text = Convert.ToString(Math.Round(((decimal)_xwingBriefing.Length / _timerInterval), 2)); }
					break;
			}
		}

		void vsbBRF_ValueChanged(object sender, EventArgs e)
		{
			if (_eventType == Briefing.EventType.MoveMap) _mapY = (short)vsbBRF.Value;
			if (_eventType == Briefing.EventType.ZoomMap) _zoomY = (short)vsbBRF.Value;
			MapPaint();
		}

        void cmdNextCaption_Click(object sender, EventArgs e)
        {
            int time = hsbTimer.Value;
            for (int i = 0; i < _maxEvents; i++)
            {
                int etime = _events[i, 0];
                int eevt = _events[i, 1];
                if (eevt == (int)Briefing.EventType.None || eevt == (int)Briefing.EventType.EndBriefing)
                {
                    hsbTimer.Value = 1;
                    hsbTimer.Value = 0;
                    return;
                }
                if (etime > time && eevt == (int)Briefing.EventType.CaptionText)
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

        void popupUpdate(string s, int timeExtention)
        {
            lblPopupInfo.Visible = true;
            lblPopupInfo.Text = s;
            _popupTimer.Interval = 500 + timeExtention;
            _popupTimer.Start();
        }
        void popupPreviewStop()
        {
            if (_popupPreviewActive == true)
            {
                _mapX = _popupPreviewMapX;  //Restore prior map settings
                _mapY = _popupPreviewMapY;
                _zoomX = _popupPreviewZoomX;
                _zoomY = _popupPreviewZoomY;

                _popupPreviewActive = false;
                MapPaint();
            }
        }
        void popupPreviewStart()
        {
            if (_popupPreviewActive == false)
            {
                pctBrief.Focus();  //Need to force focus so it generates MouseWheel events
                _popupPreviewMapX = _mapX;  //Backup existing map settings
                _popupPreviewMapY = _mapY;
                _popupPreviewZoomX = _zoomX;
                _popupPreviewZoomY = _zoomY;

                _popupPreviewActive = true;
                MapPaint();
            }
        }
        bool processEvent(int evtIndex)
        {
            bool paint = false;
            int i = evtIndex;  //[JB] I just moved the old code here, didn't change anything.
            #region event processing
            if (_events[i, 1] == (int)Platform.Xwing.Briefing.EventType.ClearText)
            {
                if (_platform == Settings.Platform.XWING || _platform == Settings.Platform.TIE) lblTitle.Text = "";
                lblCaption.Text = "";
                _page++;
                paint = true;
            }
            else if (_events[i, 1] == (short)Briefing.EventType.TitleText && _platform == Settings.Platform.XWING || _platform == Settings.Platform.TIE)	// XvT and XWA use .LST files
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
            else if (_events[i, 1] == (short)Briefing.EventType.CaptionText || _events[i, 1] == (short)Briefing.EventType.CaptionText2)
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
            else if (_events[i, 1] == (short)Briefing.EventType.MoveMap)
            {
                _mapX = _events[i, 2];
                _mapY = _events[i, 3];
                paint = true;
            }
            else if (_events[i, 1] == (short)Briefing.EventType.ZoomMap)
            {
                _zoomX = _events[i, 2];
                _zoomY = _events[i, 3];
                if (_zoomX < 1) _zoomX = 1;  //[JB] Prevent divide by zero when drawing the grid
                if (_zoomY < 1) _zoomY = 1;
                paint = true;
            }
            else if (_events[i, 1] == (short)Briefing.EventType.ClearFGTags)
            {
                for (int h = 0; h < 8; h++)
                {
                    _fgTags[h, 0] = -1;
                    _fgTags[h, 1] = 0;
                }
                paint = true;
            }
            else if (_events[i, 1] >= (short)Briefing.EventType.FGTag1 && _events[i, 1] <= (short)Briefing.EventType.FGTag4)
            {
                int v = _events[i, 1] - (short)Briefing.EventType.FGTag1;
                _fgTags[v, 0] = _events[i, 2];	// FG number
                _fgTags[v, 1] = _events[i, 0];	// time started, for MapPaint
                paint = true;
            }
            else if (_events[i, 1] == (short)Briefing.EventType.ClearTextTags)
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
            else if (_events[i, 1] >= (short)Briefing.EventType.TextTag1 && _events[i, 1] <= (short)Briefing.EventType.TextTag4)
            {
                int v = _events[i, 1] - (short)Briefing.EventType.TextTag1;
                _textTags[v, 0] = _events[i, 2];	// tag#
                _textTags[v, 1] = _events[i, 3];	// X
                _textTags[v, 2] = _events[i, 4];	// Y
                _textTags[v, 3] = _events[i, 5];	// color
                paint = true;
            }
            //Removed XwaNewIcon, XwaShipInfo, XwaMoveIcon, XwaRotateIcon, XwaChangeRegion
            // don't need to account for EndBriefing
            #endregion
            return paint;
        }
        void resetBriefing()
        {
            _page = 1;
            _mapX = 0;
            _mapY = 0;
            _zoomX = 48;
            if (_platform == Settings.Platform.XWA) _zoomX = 32;
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
            //_message = "";
            lblTitle.Visible = true;
            lblCaption.Visible = true;
            lblTitle.Text = "";  //[JB] Clear these to force refresh, otherwise it holds old strings, even if the event list is wiped clean.
            lblCaption.Text = "";
        }

        int clampValue(int value, int min, int max)
        {
            if (value < min) value = min;
            else if (value > max) value = max;
            return value;
        }
        void clampViewport(ref int x, ref int y, ref int width, ref int height, int maxWidth, int maxHeight)
        {
            if (x < 0) x = 0;
            if (width < 0) width = 0;
            if (x > maxWidth) x = maxWidth;
            if (y < 0) y = 0;
            if (height < 0) height = 0;
            if (y > maxHeight) y = maxHeight;
            if (x + width > maxWidth)
                width = maxWidth - x;
            if (y + height > maxHeight)
                height = maxHeight - y;
            if (width > maxWidth)
                width = maxWidth;
            if (height > maxHeight)
                height = maxHeight;
        }
        void scaleViewport(ref int x, ref int y, ref int width, ref int height, float xMult, float yMult, int maxWidth, int maxHeight)
        {
            x = (int)(x * xMult);
            width = (int)(width * xMult);
            y = (int)(y * yMult);
            height = (int)(height * yMult);
            clampViewport(ref x, ref y, ref width, ref height, maxWidth, maxHeight);
        }
        void refreshDisplayElements()
        {
			BriefingPage pg = _xwingBriefing.GetBriefingPage(currentPage);
            BriefingUIPage uip = _xwingBriefing.windowSettings[pg.PageType];

            //int newWidth = 420;  Target size at maximum width
            const int Top = 5;   //Coordinates to begin painting the entire preview area in YOGEME.
            const int Left = 90;
            const int maxGameWidth = 212;    //These are the standard viewport maximums as per the page UI settings.  Might be pixels in the DOS version?  Unconfirmed.
            const int maxGameHeight = 165;   //This appears to be the actual height.  The typical caption has a bottom of 138, but the game always extends it to the maximum bottom.
            const int maxWidth = 399;   //These are the dimensions of the of the rendering area in YOGEME
            const int maxHeight = 314;  //In the windows version, X span is approximately 1.27 times larger than the Y span.
            const float xMult = 1.88F;  //Used to scale the game UI coordinates to YOGEME coordinates
            const float yMult = 1.90F;  //
            const int minTitleHeight = 20;  //Only used if map is visible.

            BriefingUIItem panel = null;
            int x, y, width, height;
            int mapTop = 0, mapBottom = 165;
            bool mapEnabled = false;

            //The game handles the coordinates differently if the map is visible:
            //  The title recalculates a minimum height of 12.09% of the vertical display area.
            //  Title is repositioned to align directly above the map (title.bottom = map.top)
            //  The bottom height is recalculated to fill all remaining vertical display.
            //  The bottom is repositioned to align directly below the map (bottom.top = map.bottom)


            //If the map is visble, the title.
            //Need to figure out the map first.  If the map is visible, then the title is always positioned above the map (title.bottom joins map.top) and the caption is always positioned below (caption.top = map.bottom).  Additionally, the caption's height is always recalculated to fill the entire remaining space.
            panel = uip.GetElement(BriefingUIPage.Elements.Map);
            if (panel.IsVisible())
            {
                int oldHeight = this.h;
                mapEnabled = true;
                x = panel.left;
                y = panel.top;

                BriefingUIItem title = uip.GetElement(BriefingUIPage.Elements.Title);
                int titleHeight = title.bottom - title.top;
                if (titleHeight < minTitleHeight)
                    titleHeight = minTitleHeight;
                y = title.top + titleHeight;  //The map aligns to the title, this seems be accurate (or reasonably so) to the game even if the title top/bottom are inverted.

                width = panel.right - panel.left;
                height = panel.bottom - panel.top;
                if (height < 1)
                {
                    pctBrief.Visible = false;
                    mapEnabled = false;
                }

                clampViewport(ref x, ref y, ref width, ref height, maxGameWidth, maxGameHeight);
                mapTop = y;  //Save the coords before we scale up for display purposes
                mapBottom = y + height;
                scaleViewport(ref x, ref y, ref width, ref height, xMult, yMult, maxWidth, maxHeight);
                if (height < 1)
                    height = 1;  //Must have at least one pixel, otherwise it will crash the paint() function.
                pctBrief.Location = new Point(Left + x, Top + y);
                pctBrief.Size = new Size(width, height);
				w = width;  //Updates the actual drawing area used by the paint() functions.
				h = height;
                if(height != oldHeight)
                    _map = new Bitmap(w, h, PixelFormat.Format24bppRgb);  //Fixes a map refresh bug if the map UI setting was resized to a larger height.
            }

            panel = uip.GetElement(BriefingUIPage.Elements.Title);
            x = panel.left;
            y = panel.top;
            width = panel.right - panel.left;
            height = panel.bottom - panel.top;
            if (mapEnabled == true)
            {
                if (height < minTitleHeight)
                    height = minTitleHeight;
                y = mapTop - height;
            }
            clampViewport(ref x, ref y, ref width, ref height, maxGameWidth, maxGameHeight);
            scaleViewport(ref x, ref y, ref width, ref height, xMult, yMult, maxWidth, maxHeight);
            lblTitle.Location = new Point(Left + x, Top + y);
            lblTitle.Size = new Size(width, height);

            panel = uip.GetElement(BriefingUIPage.Elements.Text);
            x = panel.left;
            y = panel.top;
            width = panel.right - panel.left;
            height = panel.bottom - panel.top;
            if (mapEnabled == true)
                y = mapBottom;
            height = maxGameHeight - y;  //Height always set to maximum bottom
            clampViewport(ref x, ref y, ref width, ref height, maxGameWidth, maxGameHeight);
            scaleViewport(ref x, ref y, ref width, ref height, xMult, yMult, maxWidth, maxHeight);
            lblCaption.Location = new Point(Left + x, Top + y);
            lblCaption.Size = new Size(width, height);

            hsbTimer.Value = 1;  //This forces it to refresh properly
            hsbTimer.Value = 0;

            lblCaption.Padding = new Padding(mapEnabled == true ? 72 : 6, 0, 0, 0);

            pctBrief.Visible = uip.GetElement(BriefingUIPage.Elements.Map).IsVisible();
            lblTitle.Visible = uip.GetElement(BriefingUIPage.Elements.Title).IsVisible();
            lblCaption.Visible = uip.GetElement(BriefingUIPage.Elements.Text).IsVisible();
            this.Refresh();
        }
        #endregion	tabDisplay
		#region tabStrings
		void loadStrings()
		{
			cboString.Items.Clear();
			cboText.Items.Clear();
            cboPageAddTitle.Items.Clear();
            cboPageAddCaption.Items.Clear();
			for (int i=0;i<_strings.Length;i++)
			{
				cboString.Items.Add(_strings[i]);
				cboText.Items.Add(_strings[i]);
                cboPageAddTitle.Items.Add(_strings[i]);
                cboPageAddCaption.Items.Add(_strings[i]);
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

		void tableTags_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (!_loading && onModified != null) onModified("TagsChanged", new EventArgs());
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
        void lstString_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtStringEdit.Focused) return;
            int index = lstString.SelectedIndex;
            if (index >= 0 && index < _strings.Length)
            {
                string s = _strings[index];
                s = s.Replace("$", Environment.NewLine);
                s = s.Replace("\0", Environment.NewLine);
                s = s.Replace((char)0x02, '[');  //X-wing support's TIE's style of highlighting codes, although these normally aren't used.
                s = s.Replace((char)0x01, ']');
                txtStringEdit.Text = s;
            }
        }
        void txtStringEdit_TextChanged(object sender, EventArgs e)
        {
            if (lstString.Focused && _loading) return;
            int index = lstString.SelectedIndex;
            if (index >= 0 && index < _strings.Length)
            {
				if (onModified != null) onModified("StringChanged", new EventArgs());
				string s = txtStringEdit.Text;
                s = s.Replace(Environment.NewLine, "$");
                s = s.Replace("\r", "");  //Not sure if needed, just in case.
                lstString.Items[index] = s;  //Update the list with the pretty string.
                _strings[index] = s;
                loadStrings();
            }
        }
        #endregion	tabStrings
		#region tabEvents
        bool hasAvailableEventSpace(int requestedParams)
        {
            //[JB] Tally briefing events and make sure there's enough space in the raw briefing array.
            var brief = _xwingBriefing;  //[JB] Modified for X-wing
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
            if (_platform == Settings.Platform.XWING)
                _events[i, 1] = (short)Platform.Xwing.Briefing.EventType.ClearText;
            else
                _events[i, 1] = 3;
			for (int j=2;j<6;j++) _events[i, j] = 0;
			lstEvents.SelectedIndex = i;
			if (onModified != null) onModified("EventAdd", new EventArgs());
		}
		void updateList(int index)
		{
			if (index == -1) return;
			string temp = String.Format("{0,-8:0.00}", (decimal)_events[index, 0] / _timerInterval);
			//temp += cboEvent.Items[_events[index, 1]-3].ToString();
            temp += _xwingBriefing.GetEventTypeAsString((Platform.Xwing.Briefing.EventType)_events[index, 1]);
            if (_events[index, 1] == (int)Platform.Xwing.Briefing.EventType.TitleText || _events[index, 1] == (int)Platform.Xwing.Briefing.EventType.CaptionText || _events[index, 1] == (int)Platform.Xwing.Briefing.EventType.CaptionText2)
			{
				if (_strings[_events[index, 2]].Length > 30) temp += ": \"" + _strings[_events[index, 2]].Substring(0, 30) + "...\"";
				else temp += ": \"" + _strings[_events[index, 2]] + '\"';
			}
            else if (_events[index, 1] == (int)Platform.Xwing.Briefing.EventType.MoveMap || _events[index, 1] == (int)Platform.Xwing.Briefing.EventType.ZoomMap) { temp += ": X:" + _events[index, 2] + " Y:" + _events[index, 3]; }
            else if (_events[index, 1] >= (int)Platform.Xwing.Briefing.EventType.FGTag1 && _events[index, 1] <= (int)Platform.Xwing.Briefing.EventType.FGTag4)
            {
                //[JB] This fixed a potential crash while I working on functionality to delete FGs.  Not sure if still needed.
                int fgIndex = _events[index, 2]; 
                if(fgIndex >= _briefData.Length)
                {
                    fgIndex = 0;
                    _events[index, 2] = 0;
                }
                temp += ": " + _briefData[fgIndex].Name;
            }
            else if (_events[index, 1] >= (int)Platform.Xwing.Briefing.EventType.TextTag1 && _events[index, 1] <= (int)Platform.Xwing.Briefing.EventType.TextTag4)
			{
				if (_tags[_events[index, 2]].Length > 30) temp += ": \"" + _tags[_events[index, 2]].Substring(0, 30) + "...\"";
				else temp += ": \"" + _tags[_events[index, 2]] + '\"';
			}
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
			//cboIFF.Enabled = false;  //[JB] Removed these form controls from the X-wing briefing
			cboString.Enabled = false;
			cboTag.Enabled = false;
			cboFG.Enabled = false;
			//cboColor.Enabled = false;
			numX.Enabled = false;
			numY.Enabled = false;
			//optOff.Enabled = false;
			//optOn.Enabled = false;
			//numRegion.Enabled = false;
			//cboCraft.Enabled = false;
			//cboRotate.Enabled = false;
            if (_events[i, 1] == (int)Platform.Xwing.Briefing.EventType.TitleText || _events[i, 1] == (int)Platform.Xwing.Briefing.EventType.CaptionText)
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
            else if (_events[i, 1] == (int)Platform.Xwing.Briefing.EventType.MoveMap || _events[i, 1] == (int)Platform.Xwing.Briefing.EventType.ZoomMap)
			{
				numX.Value = _events[i, 2];
				numY.Value = _events[i, 3];
				_events[i, 4] = 0;
				_events[i, 5] = 0;
				numX.Enabled = true;
				numY.Enabled = true;
			}
            else if (_events[i, 1] >= (int)Platform.Xwing.Briefing.EventType.FGTag1 && _events[i, 1] <= (int)Platform.Xwing.Briefing.EventType.FGTag4)
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
            else if (_events[i, 1] >= (int)Platform.Xwing.Briefing.EventType.TextTag1 && _events[i, 1] <= (int)Platform.Xwing.Briefing.EventType.TextTag4)
			{
				try
				{
					cboTag.SelectedIndex = _events[i, 2];
					//cboColor.SelectedIndex = _events[i, 5];  //X-wing doesn't support colored tags
				}
				catch
				{
					cboTag.SelectedIndex = 0;
					//cboColor.SelectedIndex = 0;
					_events[i, 2] = 0;
					_events[i, 3] = 0;
					_events[i, 4] = 0;
					_events[i, 5] = 0;
				}
				numX.Value = _events[i, 3];
				numY.Value = _events[i, 4];
				cboTag.Enabled = true;
				//cboColor.Enabled = true;
				numX.Enabled = true;
				numY.Enabled = true;
			}
			_loading = false;
            ActiveControl = currentControl;
        }
		void cboEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (_loading || i == -1 || cboEvent.SelectedIndex == -1) return;

            var brief = getBriefing();
            int oldEventSize = 2 + brief.EventParameterCount(_events[i, 1]);
            int newEventSize = 2 + brief.EventParameterCount((short)(cboEvent.SelectedIndex + 3));
            if(hasAvailableEventSpace(newEventSize - oldEventSize) == false)
            {
                MessageBox.Show("Cannot change Event Type because the briefing list is full and the replaced event needs more space than is available.", "Error");
                return;
            }
            if (_platform == Settings.Platform.XWING)
                _events[i, 1] = (short)_xwingBriefing.GetEventTypeByName(cboEvent.Items[cboEvent.SelectedIndex].ToString());
            else
			    _events[i, 1] = (short)(cboEvent.SelectedIndex + 3);
			if (onModified != null) onModified("EventChanged", new EventArgs());
			updateParameters();
			updateList(i);
		}
		void cboFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
            if (_platform == Settings.Platform.XWING)
            {
                if ((_events[i, 1] >= (int)Briefing.EventType.FGTag1 && _events[i, 1] <= (int)Briefing.EventType.FGTag4))
                    _events[i, 2] = (short)cboFG.SelectedIndex;
            }
            else
            {
                if ((_events[i, 1] >= (int)BaseBriefing.EventType.FGTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.FGTag8) || _events[i, 1] == (int)BaseBriefing.EventType.XwaNewIcon
                    || _events[i, 1] == (int)BaseBriefing.EventType.XwaMoveIcon || _events[i, 1] == (int)BaseBriefing.EventType.XwaRotateIcon) _events[i, 2] = (short)cboFG.SelectedIndex;
                else if (_events[i, 1] == (int)BaseBriefing.EventType.XwaShipInfo) _events[i, 3] = (short)cboFG.SelectedIndex;
            }
			if (onModified != null) onModified("FG Changed", new EventArgs());
			updateList(i);
		}
		void cboString_SelectedIndexChanged(object sender, EventArgs e)
		{
            int i = lstEvents.SelectedIndex;
            if (i == -1 || _loading) return;
            if (_platform != Settings.Platform.XWING)
            {
                if (_events[i, 1] == (int)BaseBriefing.EventType.TitleText || _events[i, 1] == (int)BaseBriefing.EventType.CaptionText) _events[i, 2] = (short)cboString.SelectedIndex;
            }
            else
            {
                if (_events[i, 1] == (int)Briefing.EventType.TitleText || _events[i, 1] == (int)Briefing.EventType.CaptionText) _events[i, 2] = (short)cboString.SelectedIndex;
            }
			if (onModified != null) onModified("String Changed", new EventArgs());
			updateList(i);
		}
		void cboTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
            if (_platform != Settings.Platform.XWING)
            {
                if (_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8) _events[i, 2] = (short)cboTag.SelectedIndex;
            }
            else
            {
                if (_events[i, 1] >= (int)Briefing.EventType.TextTag1 && _events[i, 1] <= (int)Briefing.EventType.TextTag4) _events[i, 2] = (short)cboTag.SelectedIndex;
            }
			if (onModified != null) onModified("Tag Changed", new EventArgs());
			updateList(i);
		}

		void cmdDelete_Click(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1) return;
			lstEvents.Items.RemoveAt(i);
            int j = 0;
			for (j=i;j<_maxEvents-1;j++)
			{
				if (_events[j, 1] == 0) break;
				for (int h=0;h<6;h++) _events[j, h] = _events[j+1, h];
			}
			if (onModified != null) onModified("EventDelete", new EventArgs());
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
			if (onModified != null) onModified("EventDown", new EventArgs());
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
                updateList(index + 1);   //updateList() changes lstEvents.SelectedIndex but doesn't seem to refresh the parameters controls with the selected item.  Update in reverse order so that a manual refresh will succeed.
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
			if (onModified != null) onModified("EventUp", new EventArgs());
		}

        int getEventListIndex(int eventType)
        {
            string name = _xwingBriefing.GetEventTypeAsString((Platform.Xwing.Briefing.EventType)eventType);
            return cboEvent.Items.IndexOf(name);
        }

		void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
			_loading = true;
			numTime.Value = _events[i, 0];
			//cboEvent.SelectedIndex = _events[i, 1] - 3;
            cboEvent.SelectedIndex = getEventListIndex(_events[i, 1]);
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

        void swapEvent(int index1, int index2)
        {
            //Swaps one briefing event index with another.
            short t;
			for (int j = 0; j < 6; j++)
            {
                t = _events[index1, j];
                _events[index1, j] = _events[index2, j];
                _events[index2, j] = t;
            }
			if (onModified != null) onModified("SwapEvent", new EventArgs());
		}
        void shiftEvents(int iorigin, int iend)
        {
			//Shifts briefing events by swapping the contents of the origin index in a linear path until it occupies the end index.
			if (onModified != null) onModified("ShiftEvent", new EventArgs());
			if (iend > iorigin)  //swap downward
            {
                for (int i = iorigin; i < iend; i++)
                {
                    swapEvent(i, i + 1);
                }
            }
            else if (iend < iorigin)  //swap upward
            {
                for (int i = iorigin; i > iend; i--)
                {
                    swapEvent(i, i - 1);
                }
            }
        }
        void numTime_ValueChanged(object sender, EventArgs e)
		{
            lblEventTime.Text = string.Format("{0:= 0.00 seconds}", numTime.Value / _timerInterval);
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
			if (onModified != null) onModified("TimeChanged", new EventArgs());
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
            if (_platform != Settings.Platform.XWING)
            {
                if (_events[i, 1] == (int)BaseBriefing.EventType.MoveMap || _events[i, 1] == (int)BaseBriefing.EventType.ZoomMap) _events[i, 2] = (short)numX.Value;
                else if ((_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8)
                    || _events[i, 1] == (int)BaseBriefing.EventType.XwaMoveIcon) _events[i, 3] = (short)numX.Value;
            }
            else
            {
                if (_events[i, 1] == (int)Briefing.EventType.MoveMap || _events[i, 1] == (int)Briefing.EventType.ZoomMap) _events[i, 2] = (short)numX.Value;
                else if (_events[i, 1] >= (int)Briefing.EventType.TextTag1 && _events[i, 1] <= (int)Briefing.EventType.TextTag4) _events[i, 3] = (short)numX.Value;

                if (_events[i, 1] == (int)Briefing.EventType.ZoomMap && _events[i, 2] < 1)
                    _events[i, 2] = 1;  //Prevent zoom factor 0 which crashes the game
            }
			if (onModified != null) onModified("ChangeX", new EventArgs());
			updateList(i);
		}
		void numY_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;
            if (_platform != Settings.Platform.XWING)
            {
                if (_events[i, 1] == (int)BaseBriefing.EventType.MoveMap || _events[i, 1] == (int)BaseBriefing.EventType.ZoomMap) _events[i, 3] = (short)numY.Value;
                else if ((_events[i, 1] >= (int)BaseBriefing.EventType.TextTag1 && _events[i, 1] <= (int)BaseBriefing.EventType.TextTag8)
                    || _events[i, 1] == (int)BaseBriefing.EventType.XwaMoveIcon) _events[i, 4] = (short)numY.Value;
            }
            else
            {
                if (_events[i, 1] == (int)Briefing.EventType.MoveMap || _events[i, 1] == (int)Briefing.EventType.ZoomMap) _events[i, 3] = (short)numY.Value;
                else if (_events[i, 1] >= (int)Briefing.EventType.TextTag1 && _events[i, 1] <= (int)Briefing.EventType.TextTag4) _events[i, 4] = (short)numY.Value;

                if (_events[i, 1] == (int)Briefing.EventType.ZoomMap && _events[i, 3] < 1)
                    _events[i, 3] = 1;  //Prevent zoom factor 0 which crashes the game
            }
			if (onModified != null) onModified("ChangeY", new EventArgs());
			updateList(i);
		}
		#endregion tabEvents
        #region tabPages
        void cmdPageSelect_Click(object sender, EventArgs e)
        {
            setCurrentPage(lstPages.SelectedIndex);
        }
        void cmdPageMoveUp_Click(object sender, EventArgs e)
        {
            int index = lstPages.SelectedIndex;
            if(index <= 0) return;
            saveCurrentPage();
			BriefingPage temp = _xwingBriefing.pages[index];
            _xwingBriefing.pages[index] = _xwingBriefing.pages[index - 1];
            _xwingBriefing.pages[index - 1] = temp;
			if (onModified != null) onModified("PageUp", new EventArgs());
			rebuildPageList();
            setCurrentPage(index - 1);
            lstPages.SelectedIndex = index - 1;
        }
        void cmdPageMoveDown_Click(object sender, EventArgs e)
        {
            int index = lstPages.SelectedIndex;
            if (index < 0 || index >= _xwingBriefing.pages.Count - 1) return;
            saveCurrentPage();
			BriefingPage temp = _xwingBriefing.pages[index];
            _xwingBriefing.pages[index] = _xwingBriefing.pages[index + 1];
            _xwingBriefing.pages[index + 1] = temp;
			if (onModified != null) onModified("PageDown", new EventArgs());
			rebuildPageList();
            setCurrentPage(index + 1);
            lstPages.SelectedIndex = index + 1;
        }
        void cmdPageDelete_Click(object sender, EventArgs e)
        {
            int index = lstPages.SelectedIndex;
            if(index < 0) return;
            if (index == 0 && _xwingBriefing.pages.Count == 1)
            {
                MessageBox.Show("You cannot delete the first page.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            saveCurrentPage();

            int newPage = clampValue(currentPage - 1, 0, _xwingBriefing.pages.Count - 1);
            currentPage = newPage;  //Directly set the current page so that SetCurrentPage() doesn't try to save a non-existent page when switching. 
            setCurrentPage(newPage);
            lstPages.SelectedIndex = newPage;

            _xwingBriefing.pages.RemoveAt(index);
			if (onModified != null) onModified("PageDelete", new EventArgs());
			rebuildPageList();
        }
        void cmdPageAdd_Click(object sender, EventArgs e)
        {
            saveCurrentPage();
 
            int textType = cboPageAddType.SelectedIndex;
            int titleText = cboPageAddTitle.SelectedIndex;
            int captionText = cboPageAddCaption.SelectedIndex;

            BriefingPage pg = new BriefingPage();
            pg.PageType = (short)Briefing.PageType.Text;
            int pos = 0;
            if (textType == 0) //Text only
            {
                pg.Events[pos++] = 0;
                pg.Events[pos++] = (short)Briefing.EventType.TitleText;
                pg.Events[pos++] = (short)titleText;
                pg.Events[pos++] = 0;
                pg.Events[pos++] = (short)Briefing.EventType.CaptionText;
                pg.Events[pos++] = (short)captionText;
            }
            else
            {
                int hintTitle = _xwingBriefing.GetOrCreateString(Strings.BriefingPageHintTitle);
                int hintCaption = _xwingBriefing.GetOrCreateString(Strings.BriefingPageHintCaption);
                if (hintTitle == -1 || hintCaption == -1)
                {
                    MessageBox.Show("Not enough string space to create the default hint page text.  If none of them already exist, it requires two empty string slots.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                pg.Events[pos++] = 0;
                pg.Events[pos++] = (short)Briefing.EventType.TitleText;
                pg.Events[pos++] = (short)hintTitle;
                pg.Events[pos++] = 0;
                pg.Events[pos++] = (short)Briefing.EventType.CaptionText;
                pg.Events[pos++] = (short)hintCaption;
                pg.Events[pos++] = 1;
                pg.Events[pos++] = (short)Briefing.EventType.ClearText;
                pg.Events[pos++] = 1;
                pg.Events[pos++] = (short)Briefing.EventType.TitleText;
                pg.Events[pos++] = (short)hintTitle;
                pg.Events[pos++] = 1;
                pg.Events[pos++] = (short)Briefing.EventType.CaptionText;
                pg.Events[pos++] = (short)captionText;

                _strings = _xwingBriefing.BriefingString;
                importStrings();  //Just in case new strings were added.
            }
            _xwingBriefing.pages.Add(pg);
			if (onModified != null) onModified("PageAdd", new EventArgs());
			int pgIndex = _xwingBriefing.pages.Count - 1;
            _xwingBriefing.pages[pgIndex].EventsLength = (short)_xwingBriefing.GetEventsLength(pgIndex);
            rebuildPageList();
            lstPages.SelectedIndex = pgIndex;
        }

        void lstPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool btemp = _loading;
            _loading = true;
            try
            {
				BriefingPage bp = _xwingBriefing.GetBriefingPage(lstPages.SelectedIndex);
                numPageCoordSet.Value = bp.CoordSet + 1;  //Base zero in data, Base 1 in editor.
                int pt = bp.PageType;
                if (pt < 0) pt = 0;
                if (pt >= cboPageType.Items.Count)
                    pt = cboPageType.Items.Count - 1;
                cboPageType.SelectedIndex = pt;
            }
            catch { }
            _loading = btemp;
        }

        void numPageCoordSet_ValueChanged(object sender, EventArgs e)
        {
			if (_loading) return;
            _xwingBriefing.pages[lstPages.SelectedIndex].CoordSet = (short)(numPageCoordSet.Value - 1);  //Base zero in data, Base 1 in editor.
			if (onModified != null) onModified("PageCoords", new EventArgs());
		}

        void cboPageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                _xwingBriefing.pages[lstPages.SelectedIndex].PageType = (short)cboPageType.SelectedIndex;
				if (onModified != null) onModified("PageType", new EventArgs());
				rebuildPageList();
            }
        }
        void cboPageAddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                int index = cboPageAddType.SelectedIndex;
                lblPageAddTitle.Enabled = (index == 0);
                cboPageAddTitle.Enabled = (index == 0);
				if (onModified != null) onModified("PageAddType", new EventArgs());
			}
        }

        void lstPageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdPageTypeDelete.Enabled = (lstPageType.SelectedIndex >= 2);
            lstViewport_SelectedIndexChanged(this, new EventArgs()); //Force refresh
        }
        void lstViewport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstViewport.SelectedIndex < 0)
                return;
            refreshViewports();
        }

        void grpUI_Leave(object sender, EventArgs e)
        {
            if (_loading || lstViewport.SelectedIndex < 0)
                return;

            resizeNeeded = true;

            int page = lstPageType.SelectedIndex;
            if (page < 0) page = 0;
            int index = lstViewport.SelectedIndex % 5;
            BriefingUIItem item = _xwingBriefing.windowSettings[page].items[index];
            item.top = (short)numUItop.Value;
            item.left = (short)numUIleft.Value;
            item.bottom = (short)numUIbottom.Value;
            item.right = (short)numUIright.Value;
            int oldVis = item.visible;
            item.visible = (short)(chkUIvisible.Checked ? 1 : 0);
            if (oldVis != item.visible)
                refreshPageTypes();
        }

        void cmdUIDefault_Click(object sender, EventArgs e)
        {
            _xwingBriefing.ResetUISettings(2);
            //Iterate through briefing pages and adjust indexes
            for (int i = 0; i < _xwingBriefing.pages.Count; i++)
            {
                if (_xwingBriefing.pages[i].PageType >= _xwingBriefing.windowSettings.Count)
                {
                    _xwingBriefing.pages[i].PageType = (short)(_xwingBriefing.windowSettings.Count - 1);
                    if (i == lstPages.SelectedIndex)
                        lstPages_SelectedIndexChanged(this, new EventArgs()); //Force refresh of form control values of currently selected briefing page.
                }
            }
			if (onModified != null) onModified("ResetUI", new EventArgs());
			refreshPageTypes();
            rebuildPageList();
        }
        void cmdPageTypeMap_Click(object sender, EventArgs e)
        {
            int page = lstPageType.SelectedIndex;
            if (page < 0) page = 0;
            _xwingBriefing.windowSettings[page].SetDefaultsToMapPage();
			if (onModified != null) onModified("SetPageMap", new EventArgs());
			refreshPageTypes();
        }
        void cmdPageTypeText_Click(object sender, EventArgs e)
        {
            int page = lstPageType.SelectedIndex;
            if (page < 0) page = 0;
            _xwingBriefing.windowSettings[page].SetDefaultsToTextPage();
			if (onModified != null) onModified("SetPageText", new EventArgs());
			refreshPageTypes();
        }
        void cmdPageTypeAdd_Click(object sender, EventArgs e)
        {
            int curPage = lstPageType.SelectedIndex;
            if(curPage < 0) curPage = 0;
            BriefingUIPage newPage = new BriefingUIPage();
            newPage = _xwingBriefing.windowSettings[curPage];
            _xwingBriefing.windowSettings.Add(newPage);
			if (onModified != null) onModified("AddPage", new EventArgs());
			refreshPageTypes();
            //The dropdown list of page types might've changed and adjusted index, so force refresh.
            lstPages_SelectedIndexChanged(this, new EventArgs());
        }
        void cmdPageTypeDelete_Click(object sender, EventArgs e)
        {
            int curPage = lstPageType.SelectedIndex;
            if (curPage < 0) curPage = 0;
            if (_xwingBriefing.windowSettings.Count >= 2)  //Only delete if more than two pages.
            {
                _xwingBriefing.windowSettings.RemoveAt(curPage);
				if (onModified != null) onModified("DeletePage", new EventArgs());
				refreshPageTypes();

                //Iterate through briefing pages and adjust indexes
                for (int i = 0; i < _xwingBriefing.pages.Count; i++)
                {
                    if (_xwingBriefing.pages[i].PageType > curPage)
                    {
                        _xwingBriefing.pages[i].PageType--;
                        if (i == lstPages.SelectedIndex)
                            lstPages_SelectedIndexChanged(this, new EventArgs()); //Force refresh of form control values of currently selected briefing page.
                    }
                }
            }
            
        }

        void cboMaxCoordSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                _xwingBriefing.MaxCoordSet = (short)(2 + cboMaxCoordSet.SelectedIndex);
                for (int i = 0; i < _xwingBriefing.pages.Count; i++)
                {
                    if (_xwingBriefing.pages[i].CoordSet > _xwingBriefing.MaxCoordSet)
                    {
                        _xwingBriefing.pages[i].CoordSet = (short)(_xwingBriefing.MaxCoordSet - 1);  //Base zero in data, Base 1 in editor.
                        if (i == lstPages.SelectedIndex)
                            refreshPageTypes();
                    }
                }
				if (onModified != null) onModified("ChangeCoords", new EventArgs());
			}
            numPageCoordSet.Maximum = _xwingBriefing.MaxCoordSet;
        }
        void cboMissionLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (_loading) return;
            _xwingBriefing.MissionLocation = (short)cboMissionLocation.SelectedIndex;
			if (onModified != null) onModified("LocationChanged", new EventArgs());
		}
        void cboSelectPage1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setCurrentPage(cboSelectPage1.SelectedIndex);
        }
        void cboSelectPage2_SelectedIndexChanged(object sender, EventArgs e)
        {
            setCurrentPage(cboSelectPage2.SelectedIndex);
        }

        void rebuildPageList()
        {
            int oldindex = lstPages.SelectedIndex;
            lstPages.Items.Clear();
            cboSelectPage1.Items.Clear();
            cboSelectPage2.Items.Clear();
            for (int i = 0; i < _xwingBriefing.pages.Count; i++)
            {
				BriefingPage pg = _xwingBriefing.pages[i];
                string entry = "#" + (i + 1) + " ";
                if (pg.PageType < 0 || pg.PageType >= _xwingBriefing.windowSettings.Count)
                    entry += "Unknown:" + pg.PageType;
                else
                    entry += _xwingBriefing.windowSettings[pg.PageType].GetPageDesc();
                short[] events = pg.Events;

                //int hintString = _xwingBriefing.FindStringList(Strings.BriefingPageHintTitle);
                bool isHint = false;

                //Detect hint page.  Has 5 commands in sequence: TitleText,CaptionText,ClearText,TitleText,CaptionText
                if (events[1] == (short)Briefing.EventType.TitleText && events[4] == (short)Briefing.EventType.CaptionText && events[7] == (short)Briefing.EventType.ClearText && events[9] == (short)Briefing.EventType.TitleText && events[12] == (short)Briefing.EventType.CaptionText)
                    isHint = true;

                /*
                int pos = 0;
                while (pos < events.Length)
                {
                    pos++; //skip time
                    int evt = events[pos++];
                    if (evt == (int)Platform.Xwing.Briefing.EventType.TitleText)
                    {
                        if(events[pos] == hintString)
                        {
                            isHint = true;
                            break;
                        }
                    }
                    else if (evt == (int)Platform.Xwing.Briefing.EventType.None || evt == (int)Platform.Xwing.Briefing.EventType.EndBriefing)
                        break;
                    pos += _xwingBriefing.EventParameterCount(evt);
                }*/
                if (isHint)
                    entry += " (hints)";
                lstPages.Items.Add(entry);

                entry = "Page " + (i + 1);
                cboSelectPage1.Items.Add(entry);
                cboSelectPage2.Items.Add(entry);
            }
            if (oldindex < 0) oldindex = 0;
            if (oldindex >= lstPages.Items.Count) oldindex = lstPages.Items.Count - 1;
            lstPages.SelectedIndex = oldindex;

            cmdPageDelete.Enabled = (lstPages.Items.Count > 1); //Can't delete if only a single page remains.

            bool btemp = _loading;
            _loading = true;
            cboSelectPage1.SelectedIndex = oldindex;
            cboSelectPage2.SelectedIndex = oldindex;
            _loading = btemp;
        }
        void refreshPageTypes()
        {
            int oldIndex = lstPageType.SelectedIndex;
            lstPageType.Items.Clear();
            int oldPgType = cboPageType.SelectedIndex;
            cboPageType.Items.Clear();
            for (int i = 0; i < _xwingBriefing.windowSettings.Count; i++)
            {
                BriefingUIPage pg = _xwingBriefing.windowSettings[i];
                string t = "#" + (i + 1) + " " + pg.GetPageDesc();
                lstPageType.Items.Add(t);
                cboPageType.Items.Add(t);
            }

            bool btemp = _loading;
            _loading = true;
            if(oldIndex < 0) oldIndex = 0;
            if(oldIndex >= lstPageType.Items.Count)
                oldIndex = lstPageType.Items.Count - 1;
            lstPageType.SelectedIndex = oldIndex;

            if (oldPgType < 0) oldPgType = 0;
            if (oldPgType >= lstPageType.Items.Count)
                oldPgType = lstPageType.Items.Count - 1;
            cboPageType.SelectedIndex = oldIndex;
            _loading = btemp;
        }
        void refreshViewports()
        {
            bool btemp = _loading;
            _loading = true;
            int page = lstPageType.SelectedIndex;
            if (page < 0) page = 0;
            int index = lstViewport.SelectedIndex % 5;
            BriefingUIItem item = _xwingBriefing.windowSettings[page].items[index];
            numUItop.Value = item.top;
            numUIleft.Value = item.left;
            numUIbottom.Value = item.bottom;
            numUIright.Value = item.right;
            chkUIvisible.Checked = (item.visible != 0);
            _loading = btemp;
        }
        #endregion tabPages
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