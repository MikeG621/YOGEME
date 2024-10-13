/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2024 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.16
 *
 * CHANGELOG
 * v1.16, 241013
 * [UPD] Updates per Platform
 * [NEW] TextTag and ShipTag structs (def'd in BriefingForm.cs) to replace int[,] for _textTags and _fgTags
 * [UPD] _events now EventCollection type
 * v1.13.6, 220619
 * [NEW] Shift All checkbox on Events tab so timing can move together
 * v1.8, 201004
 * [UPD] renamed the redraw and popup timers, added to front-end instead of back-end
 * v1.7, 200816
 * [UPD #14] Nothing specific, but closing that issue
 * [UPD] Icons now use BMPs instead of the DATs, importDats() renamed to importIcons() [JB]
 * v1.6.5, 200704
 * [UPD] various code tweaks/cleaning that don't affect functionality
 * v1.6.4, 200119
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
		readonly Briefing _briefing;
		bool _loading = false;
		readonly Color _normalColor;
		//readonly Color _highlightColor;	// TODO: currently unused
		readonly Color _titleColor;
		readonly Briefing.EventCollection _events;

		//New for X-wing
		int _currentPage = 0;    // The current briefing page currently being edited.
		bool _resizeNeeded = false;

		short _zoomX = 48;
		short _zoomY;
#pragma warning disable IDE1006 // Naming Styles
		int w, h;
		EventHandler onModified = null;
#pragma warning restore IDE1006 // Naming Styles
		short _mapX, _mapY; // mapX and mapY will be different, namely the grid coordinates of the center, like how TIE handles it
		Bitmap _map;
		readonly ShipTag[] _fgTags = new ShipTag[4]; // [#, 0=FG/Icon 1=Time]
		TextTag[] _textTags = new TextTag[4];   // [#, X, Y, color]
		readonly DataTable _tableTags = new DataTable("Tags");
		readonly int _timerInterval;
		Briefing.EventType _eventType;
		short _tempX, _tempY;
		TextTag[] _tempTags;
		readonly string[] _tags;
		string[] _strings;
		readonly int _maxEvents;
		int _page = 1;
		bool _popupPreviewActive = false; //[JB] New feature
		short _popupPreviewZoomX;
		short _popupPreviewZoomY;
		short _popupPreviewMapX;
		short _popupPreviewMapY;
		bool _popupIsDragging = false;
		int _popupMiddleX;
		int _popupMiddleY;
		bool _mapPaintScheduled = false;      //True if a paint is scheduled, that is a paint request is called while a paint is already in progress.
		int _previousTimeIndex = 0;           //Tracks the previous time index of the briefing so we can detect when the user is manually scrolling through arbitrary times.
		#endregion

		public BriefingFormXwing(FlightGroupCollection fg, Briefing briefing, EventHandler onModifiedCallback)
		{
			_loading = true;
			_titleColor = Color.FromArgb(0xFC, 0xFC, 0x54);
			_normalColor = Color.FromArgb(0xFC, 0xFC, 0xFC);
			//_highlightColor = Color.FromArgb(0x00, 0xA8, 0x00);
			_zoomY = _zoomX;            // in most cases, these will remain the same
			_briefing = briefing;
			_maxEvents = Briefing.EventQuantityLimit;
			_events = new Briefing.EventCollection();
			InitializeComponent();
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
			cboEvent.Items.AddRange(_briefing.GetUsableEventTypeStrings());

			Import(fg); // FGs are separate so they can be updated without running the BRF as well
			importIcons(Application.StartupPath + "\\images\\TIE_BRF.bmp", 34);
			_tags = _briefing.BriefingTag;
			_strings = _briefing.BriefingString;
			importStrings();
			_timerInterval = Briefing.TicksPerSecond;
			hsbTimer.Maximum = _briefing.Length + 11;
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
			importEvents(_briefing.Pages[0].Events);
			hsbTimer.Value = 0;
			cboText.SelectedIndex = 0;
			cboFGTag.SelectedIndex = 0;
			cboTextTag.SelectedIndex = 0;

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

			numPageCoordSet.Maximum = briefing.MaxCoordSet;
			cboMissionLocation.Items.AddRange(Strings.MissionLocation);
			cboMissionLocation.SelectedIndex = briefing.MissionLocation;
			for (int i = 2; i <= 4; i++) cboMaxCoordSet.Items.Add(i.ToString());
			cboMaxCoordSet.SelectedIndex = briefing.MaxCoordSet - 2;

			lstString.SelectedIndex = 0;

			_loading = false;
			onModified = onModifiedCallback;
			if (lstEvents.Items.Count > 0) lstEvents.SelectedIndex = 0;
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
		BaseBriefing getBriefing()
		{
			//[JB] Retained for compatibility with code structure.  The idea was that virtual functions and overrides could be used to call code that was specific to X-wing or the other platforms.
			return _briefing;
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
		void importEvents(Briefing.EventCollection rawEvents)
		{
			for (int i = 0; i < _maxEvents; i++)
			{
				try
				{
					_events.Add(rawEvents[i].Clone());
					if (_events[i].IsEndEvent) break;
				}
				catch (ArgumentOutOfRangeException) { break; }  // if briefing is corrupted leading to an overflow, just kick out
				catch (NullReferenceException) { break; }
				lstEvents.Items.Add("");
				updateList(i);
			}
		}
		void importStrings()
		{
			if (_tableTags.Columns.Count == 0)  //[JB] If switching Briefing indexes, the table columns are already initialized, so check to make sure.
			{
				_tableTags.Columns.Add("tag");
			}
			_tableTags.Clear();  //Erase existing strings in case switching Briefings.
			lstString.Items.Clear();
			for (int i = 0; i < _tags.Length; i++)
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
			//Saves the event list we're currently editing back into the Briefing.
			var events = _briefing.Pages[_currentPage].Events;
			events.Clear();
			for (int evnt = 0; evnt < _maxEvents; evnt++)
			{
				if (_events[evnt].IsEndEvent) break;

				events.Add(_events[evnt].Clone());
			}
			onModified?.Invoke("XW Save", new EventArgs());
		}

		void setCurrentPage(int index)
		{
			//Just in case, make sure there's a default page that exists to edit
			if (_briefing.Pages.Count == 0)
				_briefing.ResetPages(1);

			if (index < 0)
				return;

			if (index != _currentPage)
				Save();
			_currentPage = index;
			BriefingPage pg = _briefing.GetBriefingPage(_currentPage);

			lstEvents.Items.Clear();
			importEvents(_briefing.Pages[_currentPage].Events);
			updateTitle();

			bool btemp = _loading;
			_loading = true;
			cboSelectPage1.SelectedIndex = index;
			cboSelectPage2.SelectedIndex = index;
			_loading = btemp;

			txtLength.Text = Convert.ToString(Math.Round((decimal)pg.Length / _timerInterval, 2));

			refreshDisplayElements();
		}
		void updateTitle()
		{
			string title = Text;
			string prefix = "   (Now Editing Page ";
			string update = prefix + (_currentPage + 1) + " of " + lstPages.Items.Count + ")";
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
				if (_resizeNeeded == true)
				{
					refreshDisplayElements(); //If any of the page UI settings were updated, make sure they take effect when switching back.
					_resizeNeeded = false;
				}
				hsbTimer.Value = 0; // force refresh, since pct doesn't want to update when hidden
			}
		}

		#region frmBrief
		void frmBrief_Activated(object sender, EventArgs e) { MapPaint(); }
		void frmBrief_FormClosed(object sender, FormClosedEventArgs e)
		{
			tabBrief.Focus();       //[JB] Leave any focused controls so that events don't refresh the disposed map.
			tmrPopup.Dispose();
			tmrMapRedraw.Dispose();
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
			onModified = null;
		}
		void frmBrief_Load(object sender, EventArgs e)
		{
			for (int i = 0; i < 4; i++) _fgTags[i].Slot = -1;
			for (int i = 0; i < 4; i++) _textTags[i].StringIndex = -1;
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
			if (hsbTimer.Value == hsbTimer.Maximum - 11) return;
			int newSpeed = tmrBrief.Interval / 2;  //[JB] Clicking FF repeatedly will keep speeding it up.
			if (newSpeed < 125 / _timerInterval) newSpeed = 125 / _timerInterval;    //Limit to 8x speed.
			tmrBrief.Interval = newSpeed;  //500 / _timerInterval;
			startTimer();
		}
		void cmdNext_Click(object sender, EventArgs e)
		{
			int i;
			for (i = 0; i < _maxEvents; i++)
			{
				if (_events[i].Time <= hsbTimer.Value || _events[i].Type == Briefing.EventType.WaitForClick) continue;    // find next stop point after current position
				break;
			}
			if (i == _maxEvents) hsbTimer.Value = hsbTimer.Maximum - 11;    // tmr_Tick takes care of halting
			else hsbTimer.Value = _events[i].Time;
		}
		void cmdPause_Click(object sender, EventArgs e) { stopTimer(); }
		void cmdPlay_Click(object sender, EventArgs e)
		{
			if (hsbTimer.Value == hsbTimer.Maximum - 11) return;    // prevent starting if already at the end
			tmrBrief.Interval = 1000 / _timerInterval;
			startTimer();
		}
		void cmdStart_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 4; i++)
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
			for (int i = 0; i < 4; i++)
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
			if (hsbTimer.Value != 0 && ((hsbTimer.Value - _previousTimeIndex >= 2) || hsbTimer.Value <= _previousTimeIndex))  //A non-incremental or reverse change (if incremental the timer should be +1 to previous), the user most likely manually moved the scrollbar.  Iterate through all past events and rebuild the briefing state.
			{
				resetBriefing();
				stopTimer();
				for (int i = 0; i < _maxEvents; i++)
				{
					//Process everything up to the current time index.
					if (_events[i].Time > hsbTimer.Value || _events[i].IsEndEvent) break;
					paint |= processEvent(i);  //paint stays enabled once enabled.
				}
			}

			_previousTimeIndex = hsbTimer.Value;
			if (hsbTimer.Value == 0)
			{
				resetBriefing();
			}
			for (int i = 0; i < _maxEvents; i++)
			{
				if (_events[i].Time < hsbTimer.Value) continue;

				if (_events[i].Time > hsbTimer.Value || _events[i].IsEndEvent) break;
				paint |= processEvent(i);  //paint stays enabled once enabled.
			}
			for (int h = 0; h < 4; h++) if (hsbTimer.Value - _fgTags[h].StartTime < 13) paint = true;
			lblTime.Text = string.Format("{0:Time: 0.00}", (decimal)hsbTimer.Value / _timerInterval);
			if (hsbTimer.Value == (hsbTimer.Maximum - 11) || hsbTimer.Value == 0) stopTimer();
			if (paint) MapPaint();  // prevent MapPaint from running if no change
			if (tmrBrief.Interval != (1000 / _timerInterval))  //[JB] Show playback speed if playing fast-forward
				lblTime.Text += "  (" + (1000 / _timerInterval) / tmrBrief.Interval + "x)";
		}

		void tmrBrief_Tick(object sender, EventArgs e) => hsbTimer.Value++;
		void tmrPopup_Tick(object sender, EventArgs e)
		{
			if (_popupPreviewActive == true) return;

			tmrPopup.Stop();
			lblPopupInfo.Visible = false;
			lblPopupInfo.Text = "";
		}
		void tmrMapRedraw_Tick(object sender, EventArgs e)
		{
			if (_mapPaintScheduled)
			{
				xwingPaint();
				_mapPaintScheduled = false;
			}
			tmrMapRedraw.Stop();
		}
		#endregion	Timer related

		public void MapPaint()
		{
			//[JB] I modified this function to instead serve as a wrapper that handles the map rendering.  It seems to reduce immediate performance for the sake of consistency and not clogging CPU cycles during rapid re-draw attempts. 
			if (_mapPaintScheduled == false)
			{
				if (!tmrMapRedraw.Enabled)         //The timer is stopped, start it up.
				{
					tmrMapRedraw.Start();
					tmrMapRedraw.Interval = 17;    //Was trying to aim for ~60 FPS, but according to MSDN, the minimum accuracy is 55 ms.
				}
				_mapPaintScheduled = true;
			}
		}

		void drawGrid(int x, int y, Graphics g)
		{
			//x = ClampValue(x, 0, w);  //The Death Star missions usually have a map very far in one direction which breaks the coordinates of the lines unless we clamp them down.
			//y = ClampValue(y, 0, h);
			Pen pn = new Pen(Color.FromArgb(0x50, 0, 0))
			{
				Width = 1
			};
			if (_briefing.MissionLocation == 0)
				pn.Color = Color.FromArgb(0, 0x38, 0);         //Space
			else
				pn.Color = Color.FromArgb(0x48, 0x48, 0x48);   //Death Star
			pn.Width = 1;
			int mod = 2;

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
				for (int i = min; i < max; i++)
				{
					if (i % 4 == 0) continue;                       // don't draw where there'll be maj lines
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
					g.DrawLine(pn, 0, _zoomY * 1 * i * mod + y - 1, w, _zoomY * 1 * i * mod + y - 1);   //min lines, every zoom pixels
					g.DrawLine(pn, 0, y - 1 - _zoomY * 1 * i * mod, w, y - 1 - _zoomY * 1 * i * mod);
					g.DrawLine(pn, _zoomX * 1 * i * mod + x, 0, _zoomX * 1 * i * mod + x, h);
					g.DrawLine(pn, x - _zoomX * 1 * i * mod, 0, x - _zoomX * 1 * i * mod, h);
				}
			}
			else if (_zoomX >= 8)
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
			// else if (_zoomX < 8) just don't draw them
			pn.Color = Color.FromArgb(0x90, 0, 0);
			if (_briefing.MissionLocation == 0)
				pn.Color = Color.FromArgb(0, 0x78, 0);         //Space
			else
				pn.Color = Color.FromArgb(0x78, 0x78, 0x78);   //Death Star
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
			cmdTitle.Enabled = !state;
			cmdCaption.Enabled = !state;
			cmdFG.Enabled = !state;
			cmdText.Enabled = !state;
			cmdZoom.Enabled = !state;
			cmdMove.Enabled = !state;
			cmdClearText.Enabled = !state;
			if (!state)
			{
				pnlShipTag.Visible = false;
				pnlTextTag.Visible = false;
			}
		}
		int findExisting(Briefing.EventType eventType)
		{
			int i;
			for (i = 0; i < _maxEvents; i++)
			{
				if (_events[i].Time < hsbTimer.Value) continue;
				if (_events[i].Time > hsbTimer.Value) return (i + 10000); // did not find existing, return next available + marker
				if (_events[i].Type == eventType) return i;  // found existing
			}
			return i + 10000; // actually somehow got through the entire loop
		}
		/*int findNext() { return findNext(hsbTimer.Value); }
		int findNext(int time)
		{
			int i;
			for (i = 0; i < _maxEvents; i++)
			{
				if (_events[i, 0] < time) continue;
				if (_events[i, 0] > time) break;
			}
			return i;
		}*/
		Bitmap flatMask(Bitmap craftImage, byte iff, byte intensity)
		{
			// okay, this one is just for FG tags.  flat image, I only care about the shape.
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
		/*int[] getTagSize(int craft)
		{
			FileStream fs = File.OpenRead(Application.StartupPath + "\\images\\XvT_BRF.dat");
			BinaryReader br = new BinaryReader(fs);
			fs.Position = craft * 2 + 2;
			fs.Position = br.ReadInt16();
			int[] size = new int[2];
			size[0] = br.ReadByte();
			size[1] = br.ReadByte();
			fs.Close();
			return size;    // size of base craft image as [width,height]
		}*/
		void imageQuad(int x, int y, int spacing, Bitmap craftImage, Graphics g)
		{
			g.DrawImageUnscaled(craftImage, x + spacing, y + spacing);
			g.DrawImageUnscaled(craftImage, x + spacing, y - spacing);
			g.DrawImageUnscaled(craftImage, x - spacing, y - spacing);
			g.DrawImageUnscaled(craftImage, x - spacing, y + spacing);
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
					pix[pos + x * 3 + 1] = (rgb[1] == 2 ? pix[pos + x * 3 + 1] : (byte)(pix[pos + x * 3] * rgb[1]));
					pix[pos + x * 3] = (byte)(pix[pos + x * 3] * rgb[2]);
				}
			}
			GraphicsFunctions.CopyBytesToImage(pix, bmData);
			craftImage.UnlockBits(bmData);
			craftImage.MakeTransparent(Color.Black);
		}
		void xwingPaint()
		{
			if (_loading) return;

			int X = 2 * (-_zoomX * _mapX / 256) + w / 2;    // values are written out like this to force even numbers
			int Y = 2 * (-_zoomY * _mapY / 256) + h / 2;
			Graphics g = Graphics.FromImage(_map);
			g.Clear(SystemColors.Control);
			SolidBrush sb;
			Pen pn = new Pen(Color.FromArgb(0, 0x48, 0)) { Width = 1 };
			if (_briefing.MissionLocation == 0) //Space
			{
				pn.Color = Color.FromArgb(0, 0x38, 0);
				sb = new SolidBrush(Color.Black);
			}
			else //Death Star
			{
				pn.Color = Color.FromArgb(0x48, 0x48, 0x48);
				sb = new SolidBrush(Color.FromArgb(0x2C, 0x30, 0x50));
			}
			g.FillRectangle(sb, 0, 0, w, h);
			g.DrawLine(pn, 0, 1, w, 1);
			g.DrawLine(pn, 0, h - 3, w, h - 3);
			g.DrawLine(pn, 1, 0, 1, h);
			g.DrawLine(pn, w - 1, 0, w - 1, h);
			drawGrid(X, Y, g);
			Bitmap bmptemp;

			int csIndex = _briefing.Pages[_currentPage].CoordSet;
			int wpIndex = 0; //Default to SP1
			if (csIndex >= 1 && csIndex <= 3) wpIndex = 7 + csIndex - 1;  //Switch to CS point.

			for (int i = 0; i < 4; i++)
			{
				if (_fgTags[i].Slot == -1) continue;

				sb = new SolidBrush(Color.FromArgb(0xE0, 0, 0));    // defaults to Red
				SolidBrush sb2 = new SolidBrush(Color.FromArgb(0x78, 0, 0));
				byte IFF = _briefData[_fgTags[i].Slot].IFF;
				int wpX = 2 * (int)Math.Round((double)_zoomX * _briefData[_fgTags[i].Slot].WaypointArr[wpIndex][0] / 256, 0) + X;
				int wpY = 2 * (int)Math.Round((double)_zoomY * _briefData[_fgTags[i].Slot].WaypointArr[wpIndex][1] / 256, 0) + Y;  //Y axis is not inverted
				int frame = hsbTimer.Value - _fgTags[i].StartTime;
				if (_fgTags[i].StartTime == 0) frame = 12; // if tagged at t=0, just the box
				try { bmptemp = new Bitmap(imgCraft.Images[_briefData[_fgTags[i].Slot].Craft]); }
				catch { bmptemp = new Bitmap(imgCraft.Images[0]); }

				switch (frame)
				{
					case 0:
						imageQuad(wpX - 16, wpY - 16, 32, flatMask(bmptemp, IFF, 0xFC), g);
						break;
					case 1:
						imageQuad(wpX - 16, wpY - 16, 32, flatMask(bmptemp, IFF, 0xC8), g);
						imageQuad(wpX - 16, wpY - 16, 28, flatMask(bmptemp, IFF, 0xFC), g);
						break;
					case 2:
						imageQuad(wpX - 16, wpY - 16, 32, flatMask(bmptemp, IFF, 0x94), g);
						imageQuad(wpX - 16, wpY - 16, 28, flatMask(bmptemp, IFF, 0xC8), g);
						imageQuad(wpX - 16, wpY - 16, 24, flatMask(bmptemp, IFF, 0xFC), g);
						break;
					case 3:
						imageQuad(wpX - 16, wpY - 16, 32, flatMask(bmptemp, IFF, 0x60), g);
						imageQuad(wpX - 16, wpY - 16, 28, flatMask(bmptemp, IFF, 0x94), g);
						imageQuad(wpX - 16, wpY - 16, 24, flatMask(bmptemp, IFF, 0xC8), g);
						imageQuad(wpX - 16, wpY - 16, 20, flatMask(bmptemp, IFF, 0xFC), g);
						break;
					case 4:
						imageQuad(wpX - 16, wpY - 16, 28, flatMask(bmptemp, IFF, 0x60), g);
						imageQuad(wpX - 16, wpY - 16, 24, flatMask(bmptemp, IFF, 0x94), g);
						imageQuad(wpX - 16, wpY - 16, 20, flatMask(bmptemp, IFF, 0xC8), g);
						imageQuad(wpX - 16, wpY - 16, 16, flatMask(bmptemp, IFF, 0xFC), g);
						break;
					case 5:
						imageQuad(wpX - 16, wpY - 16, 24, flatMask(bmptemp, IFF, 0x60), g);
						imageQuad(wpX - 16, wpY - 16, 20, flatMask(bmptemp, IFF, 0x94), g);
						imageQuad(wpX - 16, wpY - 16, 16, flatMask(bmptemp, IFF, 0xC8), g);
						imageQuad(wpX - 16, wpY - 16, 12, flatMask(bmptemp, IFF, 0xFC), g);
						break;
					case 6:
						imageQuad(wpX - 16, wpY - 16, 20, flatMask(bmptemp, IFF, 0x60), g);
						imageQuad(wpX - 16, wpY - 16, 16, flatMask(bmptemp, IFF, 0x94), g);
						imageQuad(wpX - 16, wpY - 16, 12, flatMask(bmptemp, IFF, 0xC8), g);
						imageQuad(wpX - 16, wpY - 16, 8, flatMask(bmptemp, IFF, 0xFC), g);
						break;
					case 7:
						imageQuad(wpX - 16, wpY - 16, 16, flatMask(bmptemp, IFF, 0x60), g);
						imageQuad(wpX - 16, wpY - 16, 12, flatMask(bmptemp, IFF, 0x94), g);
						imageQuad(wpX - 16, wpY - 16, 8, flatMask(bmptemp, IFF, 0xC8), g);
						imageQuad(wpX - 16, wpY - 16, 4, flatMask(bmptemp, IFF, 0xFC), g);
						break;
					case 8:
						imageQuad(wpX - 16, wpY - 16, 12, flatMask(bmptemp, IFF, 0x60), g);
						imageQuad(wpX - 16, wpY - 16, 8, flatMask(bmptemp, IFF, 0x94), g);
						imageQuad(wpX - 16, wpY - 16, 4, flatMask(bmptemp, IFF, 0xC8), g);
						break;
					case 9:
						imageQuad(wpX - 16, wpY - 16, 8, flatMask(bmptemp, IFF, 0x60), g);
						imageQuad(wpX - 16, wpY - 16, 4, flatMask(bmptemp, IFF, 0x94), g);
						break;
					case 10:
						imageQuad(wpX - 16, wpY - 16, 4, flatMask(bmptemp, IFF, 0x60), g);
						break;
					case 11:
						if (IFF == 0) { sb.Color = Color.FromArgb(0, 0xE0, 0); sb2.Color = Color.FromArgb(0, 0x78, 0); }    // green
						else if (IFF == 2) { sb.Color = Color.FromArgb(0, 0x78, 0xE0); sb2.Color = Color.FromArgb(0, 0x10, 0x78); } // blue
						else if (IFF == 3 || IFF == 5) { sb.Color = Color.FromArgb(0xE0, 0, 0xE0); sb2.Color = Color.FromArgb(0x78, 0, 0x78); } // purple
						g.FillRectangle(sb, wpX - 8, wpY - 8, 18, 18);
						g.FillRectangle(sb2, wpX - 6, wpY - 6, 14, 14);
						break;
					default:
						// 12 or greater, just the box
						if (IFF == 0) { sb.Color = Color.FromArgb(0, 0xE0, 0); sb2.Color = Color.FromArgb(0, 0x78, 0); }    // green
						else if (IFF == 2) { sb.Color = Color.FromArgb(0, 0x78, 0xE0); sb2.Color = Color.FromArgb(0, 0x10, 0x78); } // blue
						else if (IFF == 3 || IFF == 5) { sb.Color = Color.FromArgb(0xE0, 0, 0xE0); sb2.Color = Color.FromArgb(0x78, 0, 0x78); } // purple
						g.FillRectangle(sb, wpX - 12, wpY - 12, 26, 26);
						g.FillRectangle(sb2, wpX - 10, wpY - 10, 22, 22);
						break;
				}
			}
			for (int i = 0; i < 4; i++)
			{
				if (_textTags[i].StringIndex == -1) continue;
				sb = new SolidBrush(Color.FromArgb(0x55, 0xD1, 0xF1));  // [JB] Color code taken from screenshot.  X-wing doesn't allow colored-coded messages.
				g.DrawString(_tags[_textTags[i].StringIndex], new Font("MS Reference Sans Serif", 10), sb, 2 * (int)Math.Round((double)_zoomX * _textTags[i].X / 256, 0) + X, 2 * (int)Math.Round((double)_zoomY * _textTags[i].Y / 256, 0) + Y);
			}
			for (int i = 0; i < _briefData.Length; i++)
			{
				if (_briefData[i].Waypoint[3] != 1) continue;
				if (_zoomX >= 32) bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft]);
				else bmptemp = new Bitmap(imgCraft.Images[_briefData[i].Craft + 88]);   // small icon
				tieMask(bmptemp, _briefData[i].IFF);
				// simple base-256 grid coords * zoom to get pixel location, * 2 to enlarge, + map offset, - pic size/2 to center
				// forced to even numbers
				g.DrawImageUnscaled(bmptemp, 2 * (int)Math.Round((double)_zoomX * _briefData[i].WaypointArr[wpIndex][0] / 256, 0) + X - 16, 2 * (int)Math.Round((double)_zoomX * _briefData[i].WaypointArr[wpIndex][1] / 256, 0) + Y - 16);  //  //Y axis is not inverted
			}
			g.DrawString("#" + _page, new Font("Arial", 8), new SolidBrush(Color.White), w - 20, 4);
			pctBrief.Invalidate();      // since it's drawing to memory, this refreshes the pct.  Removes the flicker when zooming
			g.Dispose();
		}

		void cboTextTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			_textTags[(int)numText.Value - 1].StringIndex = cboTextTag.SelectedIndex;
			MapPaint();
		}

		void cmdCancel_Click(object sender, EventArgs e)
		{
			//BaseBriefing brief = getBriefing();
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
		}
		void cmdOk_Click(object sender, EventArgs e)
		{
			BaseBriefing brief = getBriefing();
			if (!_briefing.Pages[_currentPage].Events.HasSpaceForEvent(_eventType)) //Check space for a full event
			{
				MessageBox.Show("Event list is full, cannot add more.", "Error");
				cmdCancel_Click(0, new EventArgs());
				return;
			}
			if (_eventType == Briefing.EventType.ClearFGTags) if (optText.Checked) _eventType = Briefing.EventType.ClearTextTags;
			int i = -1;

			switch (_eventType)
			{

				case Briefing.EventType.TitleText:
					i = findExisting(_eventType);
					if (i >= 10000) i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					_events[i].Variables[0] = (short)((cboText.SelectedIndex >= 0) ? cboText.SelectedIndex : 0);  //[JB] Fix exception if no string is selected in the dropdown box.
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
				case Briefing.EventType.CaptionText:
					i = findExisting(_eventType);
					if (i >= 10000) i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
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
				case Briefing.EventType.MoveMap:
					i = findExisting(_eventType);
					if (i >= 10000) i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					_events[i].Variables[0] = _mapX;
					_events[i].Variables[1] = _mapY;
					// don't need to repaint, done while adjusting values
					break;
				case Briefing.EventType.ZoomMap:
					i = findExisting(_eventType);
					if (i >= 10000) i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					_events[i].Variables[0] = _zoomX;
					_events[i].Variables[1] = _zoomY;
					// don't need to repaint, done while adjusting values
					break;
				case Briefing.EventType.ClearFGTags:
					i = findExisting(_eventType);
					if (i < 10000) break; // no further action, existing break found

					i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					for (int n = 0; n < 8; n++)
					{
						_fgTags[n].Slot = -1;
						_fgTags[n].StartTime = 0;
					}
					break;
				case Briefing.EventType.FGTag1:
					_eventType = (Briefing.EventType)((int)_eventType + numFG.Value - 1);
					i = findExisting(_eventType);
					if (i >= 10000) i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					_events[i].Variables[0] = (short)cboFGTag.SelectedIndex;
					_fgTags[(int)_eventType - (int)Briefing.EventType.FGTag1].Slot = _events[i].Variables[0];
					_fgTags[(int)_eventType - (int)Briefing.EventType.FGTag1].StartTime = _events[i].Time;
					MapPaint();
					break;
				case Briefing.EventType.ClearTextTags:
					i = findExisting(_eventType);
					if (i < 10000) break; // no further action, existing break found

					i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					for (int n = 0; n < 4; n++)
					{
						_textTags[n].StringIndex = -1;
						_textTags[n].X = 0;
					}
					break;
				case Briefing.EventType.TextTag1:
					_eventType = (Briefing.EventType)((int)_eventType + numText.Value - 1);
					i = findExisting(_eventType);
					if (i >= 10000)
					{
						if (_tempX == -621 && _tempY == -621)
						{
							MessageBox.Show("No tag location selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							i = 0;
							break;
						}
						i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
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
					break; // don't need to repaint or restore/edit from backup, as it's taken care of during placement
				case Briefing.EventType.ClearText:
					i = findExisting(_eventType);
					if (i < 10000) break;

					i = _events.Insert(i - 10000, new Briefing.Event(_eventType) { Time = (short)hsbTimer.Value });
					break;
			}

			//[JB] Need to check for empty events. Some events fail to add if the user doesn't supply correct info (like XwaNewIcon with no location selected) and refreshing an empty list would throw an exception.
			if (lstEvents.Items.Count != 0)
			{
				lstEvents.SelectedIndex = i;
				updateList(i);
			}
			onModified?.Invoke("EventAdd", new EventArgs());
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
			_textTags = _tempTags;  // restore and re-edit
			_textTags[(int)numText.Value - 1].StringIndex = cboTextTag.SelectedIndex;
			_textTags[(int)numText.Value - 1].X = _tempX;
			_textTags[(int)numText.Value - 1].Y = _tempY;
			MapPaint();
		}

		void pctBrief_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
			{
				if (e.Button == MouseButtons.Middle)
					_popupIsDragging = false;

				popupPreviewStop();
			}
		}
		void pctBrief_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button.ToString() != "Left")
			{
				if (e.Button == MouseButtons.Middle)
				{
					_popupIsDragging = true;
					_popupMiddleX = e.X;
					_popupMiddleY = e.Y;
				}
				popupPreviewStart();
				pctBrief_MouseMove(0, e); //Simulate mouse move to refresh and display the data
				return;
			}
			if (_eventType == Briefing.EventType.TextTag1)
			{
				_textTags = _tempTags;  // restore backup before messing with it again
				_tempX = (short)(128 * e.X / _zoomX - 64 * w / _zoomX + _mapX);
				_tempY = (short)(128 * e.Y / _zoomY - 64 * h / _zoomY + _mapY);
				_textTags[(int)numText.Value - 1].StringIndex = cboTextTag.SelectedIndex;
				_textTags[(int)numText.Value - 1].X = _tempX;
				_textTags[(int)numText.Value - 1].Y = _tempY;
				MapPaint();
			}
		}
		void pctBrief_MouseMove(object sender, MouseEventArgs e)
		{
			if (_popupPreviewActive)
			{
				if (_popupIsDragging)
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
				int xu = 128 * e.X / _zoomX - 64 * w / _zoomX + _mapX;
				int yu = 128 * e.Y / _zoomY - 64 * h / _zoomY + _mapY;
				double xkm = Math.Round(xu * 0.00625, 2);
				double ykm = Math.Round(-yu * 0.00625, 2);
				string s = "PREVIEW ONLY\nZoom: " + _zoomX + " , " + _zoomY;
				s += "\nMap Offset: " + _mapX + " , " + _mapY;
				s += "\nMap Coords: " + xu + " , " + yu;
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
			try
			{
				t_Length = (short)Math.Round(Convert.ToDecimal(txtLength.Text) * _timerInterval, 0);    // this is the line that could throw
				_briefing.Length = t_Length;
				onModified?.Invoke("LengthChange", new EventArgs());
				BriefingPage pg = _briefing.GetBriefingPage(_currentPage);
				pg.Length = t_Length;
				hsbTimer.Maximum = _briefing.Length + 11;
				if (Math.Round(((decimal)_briefing.Length / _timerInterval), 2) != Convert.ToDecimal(txtLength.Text))  // so things like .51 become .5, without
					txtLength.Text = Convert.ToString(Math.Round((decimal)_briefing.Length / _timerInterval, 2));    // wiping out just a decimal
			}
			catch { txtLength.Text = Convert.ToString(Math.Round((decimal)_briefing.Length / _timerInterval, 2)); }
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
				int etime = _events[i].Time;
				if (_events[i].IsEndEvent)
				{
					hsbTimer.Value = 1;
					hsbTimer.Value = 0;
					return;
				}
				if (etime > time && _events[i].Type == Briefing.EventType.CaptionText)
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
			if (!_popupPreviewActive)
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
			int i = evtIndex;
			if (_events[i].Type == Briefing.EventType.ClearText)
			{
				lblTitle.Text = "";
				lblCaption.Text = "";
				_page++;
				paint = true;
			}
			else if (_events[i].Type == Briefing.EventType.TitleText)
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
			else if (_events[i].Type == Briefing.EventType.CaptionText || _events[i].Type == Briefing.EventType.CaptionText2)
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
			}
			else if (_events[i].Type == Briefing.EventType.MoveMap)
			{
				_mapX = _events[i].Variables[0];
				_mapY = _events[i].Variables[1];
				paint = true;
			}
			else if (_events[i].Type == Briefing.EventType.ZoomMap)
			{
				_zoomX = _events[i].Variables[0];
				_zoomY = _events[i].Variables[1];
				if (_zoomX < 1) _zoomX = 1;
				if (_zoomY < 1) _zoomY = 1;
				paint = true;
			}
			else if (_events[i].Type == Briefing.EventType.ClearFGTags)
			{
				for (int h = 0; h < _fgTags.Length; h++)
				{
					_fgTags[h].Slot = -1;
					_fgTags[h].StartTime = 0;
				}
				paint = true;
			}
			else if (_events[i].IsFGTag)
			{
				int v = (int)_events[i].Type - (int)Briefing.EventType.FGTag1;
				_fgTags[v].Slot = _events[i].Variables[0];
				_fgTags[v].StartTime = _events[i].Time;
				paint = true;
			}
			else if (_events[i].Type == Briefing.EventType.ClearTextTags)
			{
				for (int h = 0; h < _textTags.Length; h++)
				{
					_textTags[h].StringIndex = -1;
					_textTags[h].X = 0;
					_textTags[h].Y = 0;
				}
				paint = true;
			}
			else if (_events[i].IsTextTag)
			{
				int v = (int)_events[i].Type - (int)Briefing.EventType.TextTag1;
				_textTags[v].StringIndex = _events[i].Variables[0];
				_textTags[v].X = _events[i].Variables[1];
				_textTags[v].Y = _events[i].Variables[2];
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
			_zoomY = _zoomX;
			for (int h = 0; h < 4; h++)
			{
				_fgTags[h].Slot = -1;
				_fgTags[h].StartTime = 0;
				_textTags[h].StringIndex = -1;
				_textTags[h].X = 0;
				_textTags[h].Y = 0;
			}
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
			BriefingPage pg = _briefing.GetBriefingPage(_currentPage);
			BriefingUIPage uip = _briefing.WindowSettings[(int)pg.PageType];

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

			BriefingUIItem panel;
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
			if (panel.IsVisible)
			{
				int oldHeight = h;
				mapEnabled = true;
				x = panel.Left;

				BriefingUIItem title = uip.GetElement(BriefingUIPage.Elements.Title);
				int titleHeight = title.Bottom - title.Top;
				if (titleHeight < minTitleHeight)
					titleHeight = minTitleHeight;
				y = title.Top + titleHeight;  //The map aligns to the title, this seems be accurate (or reasonably so) to the game even if the title top/bottom are inverted.

				width = panel.Right - panel.Left;
				height = panel.Bottom - panel.Top;
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
				if (height != oldHeight)
					_map = new Bitmap(w, h, PixelFormat.Format24bppRgb);  //Fixes a map refresh bug if the map UI setting was resized to a larger height.
			}

			panel = uip.GetElement(BriefingUIPage.Elements.Title);
			x = panel.Left;
			y = panel.Top;
			width = panel.Right - panel.Left;
			height = panel.Bottom - panel.Top;
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
			x = panel.Left;
			y = panel.Top;
			width = panel.Right - panel.Left;
			height = panel.Bottom - panel.Top;
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

			pctBrief.Visible = uip.GetElement(BriefingUIPage.Elements.Map).IsVisible;
			lblTitle.Visible = uip.GetElement(BriefingUIPage.Elements.Title).IsVisible;
			lblCaption.Visible = uip.GetElement(BriefingUIPage.Elements.Text).IsVisible;
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
			for (int i = 0; i < _strings.Length; i++)
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
			for (int i = 0; i < _tags.Length; i++)
			{
				cboTag.Items.Add(_tags[i]);
				cboTextTag.Items.Add(_tags[i]);
			}
		}

		void tableTags_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (!_loading && onModified != null) onModified("TagsChanged", new EventArgs());
			int i = 0;
			for (int j = 0; j < _tags.Length; j++)
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
				onModified?.Invoke("StringChanged", new EventArgs());
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
		bool hasAvailableEventSpace(int requestedParams) => _briefing.Pages[_currentPage].EventsLength + requestedParams < _maxEvents * 2;
		void newEventAtCurrent()
		{
			// create a new item @ SelectedIndex, 
			int i = lstEvents.SelectedIndex;
			if (i == -1) i = 0;
			lstEvents.Items.Insert(i, "");
			i = _events.Insert(i, new Briefing.Event(Briefing.EventType.ClearText));
			lstEvents.SelectedIndex = i;
			onModified?.Invoke("EventAdd", new EventArgs());
		}
		void updateList(int index)
		{
			if (index == -1) return;
			string temp = string.Format("{0,-8:0.00}", (decimal)_events[index].Time / _timerInterval);
			//temp += cboEvent.Items[_events[index].Type-3].ToString();
			temp += _briefing.GetEventTypeAsString(_events[index].Type);
			if (_events[index].Type == Briefing.EventType.TitleText || _events[index].Type == Briefing.EventType.CaptionText || _events[index].Type == Briefing.EventType.CaptionText2)
			{
				if (_strings[_events[index].Variables[0]].Length > 30) temp += ": \"" + _strings[_events[index].Variables[0]].Substring(0, 30) + "...\"";
				else temp += ": \"" + _strings[_events[index].Variables[0]] + '\"';
			}
			else if (_events[index].Type == Briefing.EventType.MoveMap || _events[index].Type == Briefing.EventType.ZoomMap) { temp += ": X:" + _events[index].Variables[0] + " Y:" + _events[index].Variables[1]; }
			else if (_events[index].IsFGTag)
			{
				//[JB] This fixed a potential crash while I working on functionality to delete FGs.  Not sure if still needed.
				int fgIndex = _events[index].Variables[0];
				if (fgIndex >= _briefData.Length)
				{
					fgIndex = 0;
					_events[index].Variables[0] = 0;
				}
				temp += ": " + _briefData[fgIndex].Name;
			}
			else if (_events[index].IsTextTag)
			{
				if (_tags[_events[index].Variables[0]].Length > 30) temp += ": \"" + _tags[_events[index].Variables[0]].Substring(0, 30) + "...\"";
				else temp += ": \"" + _tags[_events[index].Variables[0]] + '\"';
			}
			lstEvents.Items[index] = temp;
			if (!_loading)
			{
				_loading = true;
				lstEvents.SelectedIndex = index;
				_loading = false;
				MapPaint();     // to update things like tags, strings, etc
			}
		}
		void updateParameters()
		{
			Control currentControl = ActiveControl;  //[JB] Maintain the current control's focus.  Basically a hack to maintain tab order since refreshing other controls will steal focus.  I want it to at least maintain tab order between the X/Y num boxes.
			int i = lstEvents.SelectedIndex;
			_loading = true;
			cboString.Enabled = false;
			cboTag.Enabled = false;
			cboFG.Enabled = false;
			numX.Enabled = false;
			numY.Enabled = false;
			if (_events[i].Type == Briefing.EventType.TitleText || _events[i].Type == Briefing.EventType.CaptionText)
			{
				try { cboString.SelectedIndex = _events[i].Variables[0]; }
				catch
				{
					cboString.SelectedIndex = 0;
					_events[i].Variables[0] = 0;
				}
				cboString.Enabled = true;
			}
			else if (_events[i].Type == Briefing.EventType.MoveMap || _events[i].Type == Briefing.EventType.ZoomMap)
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
				}
				catch
				{
					cboTag.SelectedIndex = 0;
					_events[i].Variables[0] = 0;
					_events[i].Variables[1] = 0;
					_events[i].Variables[2] = 0;
				}
				numX.Value = _events[i].Variables[1];
				numY.Value = _events[i].Variables[2];
				cboTag.Enabled = true;
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

			int oldEventSize = 2 + Briefing.EventParameters.GetCount(_events[i].Type);
			int newEventSize = 2 + Briefing.EventParameters.GetCount((short)(cboEvent.SelectedIndex + 3));
			if (!hasAvailableEventSpace(newEventSize - oldEventSize))
			{
				MessageBox.Show("Cannot change Event Type because the briefing list is full and the replaced event needs more space than is available.", "Error");
				return;
			}
			_events[i].Type = (Briefing.EventType)_briefing.GetEventTypeByName(cboEvent.Items[cboEvent.SelectedIndex].ToString());
			onModified?.Invoke("EventChanged", new EventArgs());
			updateParameters();
			updateList(i);
		}
		void cboFG_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].IsFGTag) _events[i].Variables[0] = (short)cboFG.SelectedIndex;
			onModified?.Invoke("FG Changed", new EventArgs());
			updateList(i);
		}
		void cboString_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == Briefing.EventType.TitleText || _events[i].Type == Briefing.EventType.CaptionText) _events[i].Variables[0] = (short)cboString.SelectedIndex;
			onModified?.Invoke("String Changed", new EventArgs());
			updateList(i);
		}
		void cboTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].IsTextTag) _events[i].Variables[0] = (short)cboTag.SelectedIndex;
			onModified?.Invoke("Tag Changed", new EventArgs());
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
			if (!hasAvailableEventSpace(6))
			{
				MessageBox.Show("Event list is full, cannot add more.", "Error");
				return;
			}
			newEventAtCurrent();
			if (lstEvents.SelectedIndex + 1 < lstEvents.Items.Count)
			{
				int index = lstEvents.SelectedIndex;
				swapEvent(index, index + 1);
				updateList(index + 1);   //updateList() changes lstEvents.SelectedIndex but doesn't seem to refresh the parameters controls with the selected item.  Update in reverse order so that a manual refresh will succeed.
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

		int getEventListIndex(int eventType)
		{
			string name = _briefing.GetEventTypeAsString((Briefing.EventType)eventType);
			return cboEvent.Items.IndexOf(name);
		}

		void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			_loading = true;
			numTime.Value = _events[i].Time;
			cboEvent.SelectedIndex = getEventListIndex((int)_events[i].Type);
			_loading = false;
			updateParameters();
			try { cmdUp.Enabled = (_events[i - 1].Time == _events[i].Time); }
			catch { cmdUp.Enabled = false; }
			try { cmdDown.Enabled = (_events[i + 1].Time == _events[i].Time); }
			catch { cmdDown.Enabled = false; }
		}

		void swapEvent(int index1, int index2)
		{
			var t = _events[index1];
			_events[index1] = _events[index2];
			_events[index2] = t;
			onModified?.Invoke("SwapEvent", new EventArgs());
		}
		void shiftEvents(int origin, int end)
		{
			//Shifts briefing events by swapping the contents of the origin index in a linear path until it occupies the end index.
			onModified?.Invoke("ShiftEvent", new EventArgs());
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
		void numTime_ValueChanged(object sender, EventArgs e)
		{
			lblEventTime.Text = string.Format("{0:= 0.00 seconds}", numTime.Value / _timerInterval);
			int size = lstEvents.Items.Count;
			int index = lstEvents.SelectedIndex;
			if (_loading || index == -1) return;

			_loading = true;
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
						while (p < size && _events[p].Time < _events[index].Time) p++;  //Search until a greater time index is found
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
			onModified?.Invoke("TimeChanged", new EventArgs());
			try { cmdUp.Enabled = (_events[index - 1].Time == _events[index].Time); }
			catch { cmdUp.Enabled = false; }
			try { cmdDown.Enabled = (_events[index + 1].Time == _events[index].Time); }
			catch { cmdDown.Enabled = false; }
		}
		void numX_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == Briefing.EventType.MoveMap || _events[i].Type == Briefing.EventType.ZoomMap) _events[i].Variables[0] = (short)numX.Value;
			else if (_events[i].IsTextTag) _events[i].Variables[1] = (short)numX.Value;

			if (_events[i].Type == Briefing.EventType.ZoomMap && _events[i].Variables[0] < 1)
				_events[i].Variables[0] = 1;  //Prevent zoom factor 0 which crashes the game
			onModified?.Invoke("ChangeX", new EventArgs());
			updateList(i);
		}
		void numY_ValueChanged(object sender, EventArgs e)
		{
			int i = lstEvents.SelectedIndex;
			if (i == -1 || _loading) return;

			if (_events[i].Type == Briefing.EventType.MoveMap || _events[i].Type == Briefing.EventType.ZoomMap) _events[i].Variables[1] = (short)numY.Value;
			else if (_events[i].IsTextTag) _events[i].Variables[2] = (short)numY.Value;

			if (_events[i].Type == Briefing.EventType.ZoomMap && _events[i].Variables[1] < 1) _events[i].Variables[1] = 1;  //Prevent zoom factor 0 which crashes the game
			onModified?.Invoke("ChangeY", new EventArgs());
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
			if (index <= 0) return;
			Save();
			BriefingPage temp = _briefing.Pages[index];
			_briefing.Pages[index] = _briefing.Pages[index - 1];
			_briefing.Pages[index - 1] = temp;
			onModified?.Invoke("PageUp", new EventArgs());
			rebuildPageList();
			setCurrentPage(index - 1);
			lstPages.SelectedIndex = index - 1;
		}
		void cmdPageMoveDown_Click(object sender, EventArgs e)
		{
			int index = lstPages.SelectedIndex;
			if (index < 0 || index >= _briefing.Pages.Count - 1) return;
			Save();
			BriefingPage temp = _briefing.Pages[index];
			_briefing.Pages[index] = _briefing.Pages[index + 1];
			_briefing.Pages[index + 1] = temp;
			onModified?.Invoke("PageDown", new EventArgs());
			rebuildPageList();
			setCurrentPage(index + 1);
			lstPages.SelectedIndex = index + 1;
		}
		void cmdPageDelete_Click(object sender, EventArgs e)
		{
			int index = lstPages.SelectedIndex;
			if (index < 0) return;
			if (index == 0 && _briefing.Pages.Count == 1)
			{
				MessageBox.Show("You cannot delete the first page.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Save();

			int newPage = clampValue(_currentPage - 1, 0, _briefing.Pages.Count - 1);
			_currentPage = newPage;  //Directly set the current page so that SetCurrentPage() doesn't try to save a non-existent page when switching. 
			setCurrentPage(newPage);
			lstPages.SelectedIndex = newPage;

			_briefing.Pages.RemoveAt(index);
			onModified?.Invoke("PageDelete", new EventArgs());
			rebuildPageList();
		}
		void cmdPageAdd_Click(object sender, EventArgs e)
		{
			Save();

			int textType = cboPageAddType.SelectedIndex;
			short captionText = (short)cboPageAddCaption.SelectedIndex;

			BriefingPage pg = new BriefingPage { PageType = (short)Briefing.PageType.Text };
			if (textType == 0) //Text only
			{
				pg.Events.Add(new Briefing.Event(Briefing.EventType.TitleText));
				pg.Events[0].Variables[0] = (short)cboPageAddTitle.SelectedIndex;
				pg.Events.Add(new Briefing.Event(Briefing.EventType.CaptionText));
				pg.Events[1].Variables[0] = captionText;
			}
			else
			{
				int hintTitle = _briefing.GetOrCreateString(Strings.BriefingPageHintTitle);
				int hintCaption = _briefing.GetOrCreateString(Strings.BriefingPageHintCaption);
				if (hintTitle == -1 || hintCaption == -1)
				{
					MessageBox.Show("Not enough string space to create the default hint page text.  If none of them already exist, it requires two empty string slots.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				pg.Events.Add(new Briefing.Event(Briefing.EventType.TitleText));
				pg.Events[0].Variables[0] = (short)hintTitle;
				pg.Events.Add(new Briefing.Event(Briefing.EventType.CaptionText));
				pg.Events[1].Variables[0] = (short)hintCaption;
				pg.Events.Add(new Briefing.Event(Briefing.EventType.ClearText) { Time = 1 });
				pg.Events.Add(new Briefing.Event(Briefing.EventType.TitleText) { Time = 1 });
				pg.Events[3].Variables[0] = (short)hintTitle;
				pg.Events.Add(new Briefing.Event(Briefing.EventType.CaptionText) {  Time = 1 });
				pg.Events[4].Variables[0] = captionText;

				_strings = _briefing.BriefingString;
				importStrings();  //Just in case new strings were added.
			}
			_briefing.Pages.Add(pg);
			onModified?.Invoke("PageAdd", new EventArgs());
			int pgIndex = _briefing.Pages.Count - 1;
			rebuildPageList();
			lstPages.SelectedIndex = pgIndex;
		}

		void lstPages_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool btemp = _loading;
			_loading = true;
			try
			{
				BriefingPage bp = _briefing.GetBriefingPage(lstPages.SelectedIndex);
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
			_briefing.Pages[lstPages.SelectedIndex].CoordSet = (short)(numPageCoordSet.Value - 1);  //Base zero in data, Base 1 in editor.
			onModified?.Invoke("PageCoords", new EventArgs());
		}

		void cboPageType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loading)
			{
				_briefing.Pages[lstPages.SelectedIndex].PageType = (short)cboPageType.SelectedIndex;
				onModified?.Invoke("PageType", new EventArgs());
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
				onModified?.Invoke("PageAddType", new EventArgs());
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

			_resizeNeeded = true;

			int page = lstPageType.SelectedIndex;
			if (page < 0) page = 0;
			int index = lstViewport.SelectedIndex % 5;
			BriefingUIItem item = _briefing.WindowSettings[page].Items[index];
			item.Top = (short)numUItop.Value;
			item.Left = (short)numUIleft.Value;
			item.Bottom = (short)numUIbottom.Value;
			item.Right = (short)numUIright.Value;
			bool oldVis = item.IsVisible;
			item.IsVisible = chkUIvisible.Checked;
			if (oldVis != item.IsVisible)
				refreshPageTypes();
		}

		void cmdUIDefault_Click(object sender, EventArgs e)
		{
			_briefing.ResetUISettings(2);
			//Iterate through briefing pages and adjust indexes
			for (int i = 0; i < _briefing.Pages.Count; i++)
			{
				if (_briefing.Pages[i].PageType >= _briefing.WindowSettings.Count)
				{
					_briefing.Pages[i].PageType = (short)(_briefing.WindowSettings.Count - 1);
					if (i == lstPages.SelectedIndex)
						lstPages_SelectedIndexChanged(this, new EventArgs()); //Force refresh of form control values of currently selected briefing page.
				}
			}
			onModified?.Invoke("ResetUI", new EventArgs());
			refreshPageTypes();
			rebuildPageList();
		}
		void cmdPageTypeMap_Click(object sender, EventArgs e)
		{
			int page = lstPageType.SelectedIndex;
			if (page < 0) page = 0;
			_briefing.WindowSettings[page].SetDefaultsToMapPage();
			onModified?.Invoke("SetPageMap", new EventArgs());
			refreshPageTypes();
		}
		void cmdPageTypeText_Click(object sender, EventArgs e)
		{
			int page = lstPageType.SelectedIndex;
			if (page < 0) page = 0;
			_briefing.WindowSettings[page].SetDefaultsToTextPage();
			onModified?.Invoke("SetPageText", new EventArgs());
			refreshPageTypes();
		}
		void cmdPageTypeAdd_Click(object sender, EventArgs e)
		{
			int curPage = lstPageType.SelectedIndex;
			if (curPage < 0) curPage = 0;
			BriefingUIPage newPage = _briefing.WindowSettings[curPage];
			_briefing.WindowSettings.Add(newPage);
			onModified?.Invoke("AddPage", new EventArgs());
			refreshPageTypes();
			//The dropdown list of page types might've changed and adjusted index, so force refresh.
			lstPages_SelectedIndexChanged(this, new EventArgs());
		}
		void cmdPageTypeDelete_Click(object sender, EventArgs e)
		{
			int curPage = lstPageType.SelectedIndex;
			if (curPage < 0) curPage = 0;
			if (_briefing.WindowSettings.Count >= 2)  //Only delete if more than two pages.
			{
				_briefing.WindowSettings.RemoveAt(curPage);
				onModified?.Invoke("DeletePage", new EventArgs());
				refreshPageTypes();

				//Iterate through briefing pages and adjust indexes
				for (int i = 0; i < _briefing.Pages.Count; i++)
				{
					if (_briefing.Pages[i].PageType > curPage)
					{
						_briefing.Pages[i].PageType--;
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
				_briefing.MaxCoordSet = (short)(2 + cboMaxCoordSet.SelectedIndex);
				for (int i = 0; i < _briefing.Pages.Count; i++)
				{
					if (_briefing.Pages[i].CoordSet > _briefing.MaxCoordSet)
					{
						_briefing.Pages[i].CoordSet = (short)(_briefing.MaxCoordSet - 1);  //Base zero in data, Base 1 in editor.
						if (i == lstPages.SelectedIndex)
							refreshPageTypes();
					}
				}
				onModified?.Invoke("ChangeCoords", new EventArgs());
			}
			numPageCoordSet.Maximum = _briefing.MaxCoordSet;
		}
		void cboMissionLocation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			_briefing.MissionLocation = (short)cboMissionLocation.SelectedIndex;
			onModified?.Invoke("LocationChanged", new EventArgs());
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
			for (int i = 0; i < _briefing.Pages.Count; i++)
			{
				BriefingPage pg = _briefing.Pages[i];
				string entry = "#" + (i + 1) + " ";
				if (pg.PageType < 0 || pg.PageType >= _briefing.WindowSettings.Count)
					entry += "Unknown:" + pg.PageType;
				else
					entry += _briefing.WindowSettings[pg.PageType].GetPageDesc();

				bool isHint = false;
				//Detect hint page.  Has 5 commands in sequence: TitleText,CaptionText,ClearText,TitleText,CaptionText
				if (pg.Events[0].Type == Briefing.EventType.TitleText && pg.Events[1].Type == Briefing.EventType.CaptionText
					&& pg.Events[2].Type == Briefing.EventType.ClearText && pg.Events[3].Type == Briefing.EventType.TitleText
					&& pg.Events[4].Type == Briefing.EventType.CaptionText)
					isHint = true;

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
			for (int i = 0; i < _briefing.WindowSettings.Count; i++)
			{
				BriefingUIPage pg = _briefing.WindowSettings[i];
				string t = "#" + (i + 1) + " " + pg.GetPageDesc();
				lstPageType.Items.Add(t);
				cboPageType.Items.Add(t);
			}

			bool btemp = _loading;
			_loading = true;
			if (oldIndex < 0) oldIndex = 0;
			if (oldIndex >= lstPageType.Items.Count)
				oldIndex = lstPageType.Items.Count - 1;
			lstPageType.SelectedIndex = oldIndex;

			if (oldPgType < 0) oldPgType = 0;
			if (oldPgType >= lstPageType.Items.Count)
				oldPgType = lstPageType.Items.Count - 1;
			cboPageType.SelectedIndex = oldPgType;
			_loading = btemp;
		}
		void refreshViewports()
		{
			bool btemp = _loading;
			_loading = true;
			int page = lstPageType.SelectedIndex;
			if (page < 0) page = 0;
			int index = lstViewport.SelectedIndex % 5;
			BriefingUIItem item = _briefing.WindowSettings[page].Items[index];
			numUItop.Value = item.Top;
			numUIleft.Value = item.Left;
			numUIbottom.Value = item.Bottom;
			numUIright.Value = item.Right;
			chkUIvisible.Checked = item.IsVisible;
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