/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2026 Michael Gaisser (mjgaisser@gmail.com)
 * This file authored by "JB" (Random Starfighter) (randomstarfighter@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.17.7+
 *
 * CHANGELOG
 * [NEW #137] created
 */

// All these require DEBUG too.
//#define SCREENSHOT    // Dump canvas screenshot to installation folder
//#define NOPATH        // Force empty installation path for testing asset failure
//#define NOIMPORT      // Testing asset failure of bundled icons
//#define XWAINTRO      // Intro animation when starting a briefing at frame zero.


/* This editor is designed to handle all four platforms.  To accomplish this, it converts briefing
 * information like flightgroups, strings, and events into an abstract format.  Many functions are
 * named in such a way to help identify how that information is accessed or converted.  Mission data
 * is loaded into the abstraction layer.  Depending on which briefing is selected, the abstracted
 * info is then loaded into the frontend GUI.  Switching the current briefing will push the frontend
 * data back into the abstraction layer.  Saving will push the frontend, then push the abstraction
 * layer back into the mission.  XWING is a special case as it contains some special properties like
 * customizable page templates that directly interface with the mission properties.
 * This chart illustrates the relationships:
 * 
 *   +-------FrontEnd
 *   |           |
 *   |    Abstraction Layer
 *   |           |
 *   |    +---Mission--------+
 *   |    |      |     |     |
 *   +--XWING   TIE   XVT   XWA
 */

// NOTE: Depending on configuration, intellisense will sometimes automatically add namespaces.
// This can cause ambiguity errors with some classes.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Media;                   // For audio
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Media3D;   // For Vector3D
using Idmr.LfdReader;
using Idmr.Platform;

namespace Idmr.Yogeme
{
	/// <summary>Briefing editor GUI for all platforms.</summary>
	public partial class BriefingForm2 : Form
	{
		#region Vars and stuff
		// Configuration, form status, and integration with main form
		BriefingConfigsCollection.BriefingConfig _config = BriefingConfigsCollection.GetConfig(Settings.Platform.None);
		FormScaler _scaler = null;
		Settings.Platform _platform = Settings.Platform.None;
		bool _loading = false;                // If true, form controls can be initialized without invoking data changes.
		bool _isDirty = false;                // If true, the briefing data will be exported and saved when the form is closed.
		bool _isFinishedLoading = false;      // If true, all fields that require initialization or assignment have finished loading.  The program state is valid.
		readonly HashSet<int> _pendingWaypointSync = new HashSet<int>();
		string _titleString = "";             // Empty for XW and TIE.  The loaded mission title for XvT or XWA, or the placeholder if not loaded.
		string _titleOverrideString = "";     // String to appear in place of the title text.  Used to convey certain information to the user.
		int _titleOverrideTime = 0;           // Time remaining (in milliseconds), to display an override string if it should automatically expire.  If zero, the override is persistent until changed.
		DataChangeCallback _dataChangeCallback = null;

		// Color resources
		Color[] _standardColors = null;       // Indexed by enum type
		Color[] _inlineTextColors = null;     // Only for XvT/XWA, allowing raw color codes (escaped like "\2")
		Color[] _textTagColors = null;        // The final color for a finished text tag
		Color[][] _textTagPalettes = null;    // For each text tag color, a range of colors for animation purposes
		Color[] _iconIffColors = null;        // For tinting if using fallback craft icons, and generally for indexing cached icons.
		int[] _iconIffPaletteIndices = null;  // Maps a craft icon IFF index to a text tag palette index.  This allows the FG tag animation to reuse existing palette colors.

		/// <summary>The core briefing container as loaded by Idmr.Platform.</summary>
		/// <remarks>For XW and TIE it will be the briefing itself.  For XvT and XWA it will the briefing collection.</remarks>
		readonly object _corePlatformBriefing = null;

		readonly List<AbstractBriefing> _briefingList;   // The abstraction layer that holds all information imported from a mission
		int _currentBriefingIndex = 0;          // Current briefing selected in the dropdown, loaded into the frontend
		int _currentTeamIndex = 0;              // Team index, only used by XvT for player numbering

		// Active information loaded from the currently selected briefing, or the mission itself.
		List<AbstractFlightgroup> _flightgroups;
		readonly List<string> _tags = new List<string>();
		readonly List<string> _strings = new List<string>();
		readonly List<string> _captionNotes = new List<string>();
		readonly List<AbstractEvent> _events = new List<AbstractEvent>();

		// Other runtime information, some are platform specific.
		readonly string[] _teamNames = new string[10];
		readonly string[] _regionNames = new string[4];
		string[] _iffNames;

		// The maximum safe limit for strings, excluding null terminators.  Default for XvT/XWA.  XWING and TIE override on init.
		readonly int _maxStringLength = 318;    // Format spec requires dual null terminators.
		readonly int _maxTagLength = 39;        // Format spec requires one null terminator.

		int _eventSpaceUsed = 0;           // Dynamically tracks current space used
		readonly int _eventSpaceMax = 0;   // Assigned on platform init, the maximum space a mission can hold
		const int MaxEventTime = 9990;     // Must be less than the end marker (which is time 9999)

		// Both of these together influence the maximum time that the scrollbar can reach.
		int _briefingDuration = 0;         // The assigned duration of the current briefing, in ticks.
		int _highestEventTime = 0;         // The highest time of all loaded events.  Does not include the end marker.

		// Platform-specific graphic and font resources loaded during init.
		BriefingFont _font = new BriefingFont(BitmapCacheType.FontCaption);
		BriefingFont _tagFont = new BriefingFont(BitmapCacheType.FontTag);
		BriefingFont _fontAltCaption = null;    // Holds high def font in XWING, larger font in XvT/XWA
		BriefingFont _fontAltTag = null;        // Holds high def font in XWING
		readonly Dictionary<long, Bitmap> _bitmapCache = new Dictionary<long, Bitmap>();    // A cache for preloaded or processed (tinted, rotated) bitmaps to avoid expensive reprocessing
		List<CraftIconImage> _craftIconImages = new List<CraftIconImage>();
		List<CraftIconImage> _craftFlatIconImages = null;    // Only used for TIE
		int _smallIconOffset = 0;               // For XWING and TIE, there's a duplicate set of scaled (zoomed out) icons.  If nonzero, this is the offset to add to the craft type to access the small icons.
		int _craftIconBracketSize = 0;
		AssetSourceType _iconAssetSource = AssetSourceType.None;

		// Selection and event manipulation
		List<SelectedObject> _selectedObjects = new List<SelectedObject>();
		AbstractEventType _tempEvent = AbstractEventType.None;
		Rectangle _tempInteract;             // If the temp event is movable, this is the interact location, in map canvas pixels.
		int _lastSelectedIconIndex = 0;      // Keeps track of the last selected XWA icon, for events that use them
		int _lastSelectedMoveIconUid = -1;   // When working with MoveIcon movement (path mode or otherwise) this is the last selected Event UID.

		// Tag and caption strings have different sources but are shared by the same edit controls in the sidebar.
		BriefingString _editStringType = BriefingString.None;
		int _editStringIndex = 0;
		int _editStringCharLimit = 0;
		int _editStringUndoUid = -1;

		// Rendering surfaces
		Bitmap _titleCanvas;
		Bitmap _mapCanvas;
		Bitmap _captionCanvas;
		Bitmap _xwingPanel3Canvas;
		Bitmap _xwingPanel4Canvas;
		Bitmap _combinedCanvas;

		// Rendering and scaling
		RefreshFlags _pendingRedraw = RefreshFlags.None;
		InterpolationMode _interpolationMode = InterpolationMode.Bicubic;
		float _mapScale = 2.0f;              // Scaling applied to the map PictureBox image for output purposes.  The input map canvas is never scaled.
		Size _lastFormSize;                  // Form scaling is expensive.  Prevents duplicate resize events from running.

		// Briefing map playback
		readonly int _ticksPerSecond = 24;   // Framerate for reporting time durations.  May override during platform init.
		float _playbackFramerate = 21.30f;   // Actual playback rate to best approximate in-game time.  May override during platform init, or during high def.
		bool _playbackEnabled = false;       // Tracks whether the briefing is currently playing or not.
		int _playbackSpeed = 1;              // Multiplier to number of frames processed per tick.
		int _playbackLastSystemTime = 0;     // The last polled system time.  The core value for tracking the passage of time.
		int _playbackPrecisionTime = 0;      // Accumulated time (milliseconds with 8-bit fractional component), including frame rollover time.  Determines when a new frame should be triggered.
		int _playbackFramesReady = 0;        // The number of briefing event time ticks that are pending.
		int _previousRenderTime = -1;        // Event time that was previously rendered.  Used for certain time jumps.

		// Map event states and visual information.
		Point _currentZoom, _destinationZoom;
		Point _currentPosition, _destinationPosition;
		int _currentPage = 0;              // Page number for information and goto purposes, based on caption.
		int _currentRegionPage = 0;        // Page number for XWA shadow icons, based on region change events.
		int _pendingRegionIndex = 0;
		int _currentRegionIndex = 0;
		int _regionPageTime = -1;          // When a region change is encountered, this is the time index to load the new icons.  Will be -1 when nothing needs to be loaded.
		bool _regionTimeAdvanced = false;  // A region transition requires advancing a single frame before the map is shown again, so that icons are where they should be.
		int _lastPageTime = -1;
		readonly bool[] _panelStringEnabled = new bool[4];
		readonly int[] _panelStringIndex = new int[4];
		readonly int[] _panelStringEventUid = new int[4];
		const int PANEL_TITLE = 0;
		const int PANEL_CAPTION = 1;
		const int PANEL_XWING3 = 2;
		const int PANEL_XWING4 = 3;
		readonly MapElement[] _mapFgTags = new MapElement[8];
		readonly MapElement[] _mapTextTags = new MapElement[8];
		readonly MapElement[] _mapIcons = new MapElement[192];
		readonly MapElement _mapShipInfoTag = new MapElement();
		List<MapElement>[] _mapIconCache = null; // Will be generated when used

		// XWA map effects and special states
		MapEffect _mapEffect = MapEffect.None;
		int _mapEffectCurrentFrame = 0;
		int _mapEffectMaxFrame = 0;
		MapEffect _queuedMapEffect = MapEffect.None;
		int _queuedMapEffectDelay = 0;
		int _queuedMapEffectDuration = 0;
		string _mapEffectString = "";
		bool _activeShipInfoBlock = false;
		int _activeShipInfoIcon = -1;   // The IconIndex of the activated ship info, or -1 for no ship info.
		int _shipInfoStringIndex = -1;  // Index of caption string to display during a ship info block.  -1 for none.
		int _shipInfoStringUid = -1;    // The event UID that activated the ship info caption string.

		SoundPlayer _activeSound = null;
		Dictionary<int, SoundPlayer> _captionSounds = null;

		// Alternate map modes
		readonly List<MapElement> _mapShadowIcons = new List<MapElement>();
		readonly List<PathNode> _pathNodes = new List<PathNode>();
		readonly List<MapElement> _mapPreviewIcons = new List<MapElement>();
		Point _firstPreviewStep = getNullPoint();
		Point _lastPreviewStep;
		bool _freeLookMode = false;
		bool _pathMode = false;          // XWA only, allows batch creation of icon movement events.
		bool _panelMode = false;         // XWING only, interactive adjustment of text and map panels.
		short[][] _panelBackup = null;   // Stores a backup state of page templates when using panel mode.
		int _panelModeEdge = 0;          // The currently selected panel edge when resizing in panel mode.
		Rectangle _freeLookRect;         // When free look is activated, this stores the original map frame (in raw units) so that a border can be drawn.
		Point _backupPosition;
		Point _backupZoom;

		// Undo and redo
		readonly List<List<UndoOperation>> _undoFrames = new List<List<UndoOperation>>();
		readonly List<List<UndoOperation>> _redoFrames = new List<List<UndoOperation>>();
		List<UndoOperation> _currentUndoFrame = new List<UndoOperation>();
		int _lastTimeShiftUndoUid = -1;

		// Interaction
		MouseDragState _mouseDragState = MouseDragState.None;
		Point _mouse1, _mouse2, _dragCarry;
		const int CLICK_TOLERANCE = 3;
		Rectangle _scaledTitleRect = new Rectangle();
		Rectangle _scaledMapRect = new Rectangle();
		Rectangle _scaledCaptionRect = new Rectangle();
		Rectangle _scaledPanel3Rect = new Rectangle();
		Rectangle _scaledPanel4Rect = new Rectangle();
		int _lastEditEventUid = -1;                   // For quick navigation to the last edited event
		readonly Rectangle[] _xwingSourceViewport = new Rectangle[5];  // Stores the last known source bounding box that was copied from when an Xwing panel was updated.
		bool _keyStateShift = false;
		bool _keyStateCtrl = false;
		static readonly int[] _bookmarkTimes = new int[4];     // Static so they persist even if the form is reopened
		readonly List<AbstractEvent> _eventCopyBuffer = new List<AbstractEvent>();
		Rectangle _mapShiftStart, _mapShiftEnd;       // Start and end locations of a selected map move or map zoom event
		Point _mapShiftEndZoom, _mapShiftEndPos;      // This serves the same purpose as the backup values in free look mode, but for shift mode, because we still need the original free look backups when we exit shift mode.
		bool _mapShiftMode = false;                   // Editing move and zoom events requires some workarounds with free look
		Point[] _freeLookBackup = null;               // The initial state when entering map shift mode

		// Timeline
		int _timelineColumnWidth = 40;                                   // Size in pixels of each column
		int _timelineRowScrollPos = 0;                                   // If there are too many events to fit in the vertical space of a timeline, this is the scroll position of the first visible item
		int _timelineHighestItemCount = 0;                               // The maximum number of event rows for any of the visible columns
		int _timelineVisibleRowCount = 0;                                // How many event rows (not including the header pip) are capable of fitting inside the timeline strip at its current panel size
		readonly List<MapElement> _timelineElements = new List<MapElement>();     // A list of all items that have been rendered on the timeline

		// Unsorted
		readonly System.Drawing.Font _statusFont = new System.Drawing.Font("Arial", 7, FontStyle.Regular);
		System.Drawing.Font _timelineFont = new System.Drawing.Font("Arial", 8f, FontStyle.Regular);
		System.Drawing.Font _regionFont = null;      // Only loaded for XWA

		Button[] _moveButtons = null;  // An array of icon movement buttons.  Indexed in order of Numpad1 through Numpad9, including 5 which will be null.  Assigned during form init.
		readonly int[] _moveOffsetX = new int[9] {-1, 0, 1,  -1, 0, 1,  -1, 0, 1};  // Indices correspond to Numpad1 through 9
		readonly int[] _moveOffsetY = new int[9] { 1, 1, 1,   0, 0, 0,  -1,-1,-1};  // Indices correspond to Numpad1 through 9

		readonly string[] _craftAbbrev;     // The craft abbreviations from the platform strings.
		readonly string[] _craftNames;      // The full craft names from the platform strings.  Only used for XWA.
		readonly string[] _iconRotations = new string[] { "None", "Left 90°", "180°", "Right 90°", "Mirror" };
		#endregion

		#region Constructors
		public BriefingForm2(Platform.Xwing.FlightGroupCollection fgCollection, Platform.Xwing.Briefing brf, DataChangeCallback callback)
		{
			InitializeComponent();
			initCommonControls();
			_dataChangeCallback = callback;

			_corePlatformBriefing = brf;
			_eventSpaceMax = Platform.Xwing.Briefing.EventQuantityLimit * 2;
			_craftAbbrev = CraftDataManager.GetInstance().GetShortNames();

			_maxStringLength = 510;  // 512 bytes max
			_maxTagLength = 63;      // 64 bytes max

			// Strings are larger in this platform, so the edit boxes on the strings tab need more space.
			pnlLargeEditString.Height += 20;
			foreach (Control c in pnlLargeEditString.Controls)
			{
				if (c == txtMainEditTag || c == txtMainEditString)
					c.Height += 20;
				else
					c.Top += 20;
			}

			string installPath = CraftDataManager.GetInstance().GetInstallPath();
			if (installPath == "")
				installPath = Settings.GetInstance().XwingPath;

			setPlatformConfig(Settings.Platform.XWING, installPath);

			_ticksPerSecond = 10;
			_playbackFramerate = (_config.HighDef ? 9.25f : 10.30f);  // Slower in XW98 than XW94

			_flightgroups = importAllFlightgroup(fgCollection, _craftAbbrev);
			_briefingList = importXwing(brf);
			_craftIconBracketSize = 10;

			loadXwingFrontend();
			initBriefing();
			applyFormSize();
			verifyBriefing(true, false);
		}

		public BriefingForm2(Platform.Tie.Mission mission, DataChangeCallback callback)
		{
			InitializeComponent();
			initCommonControls();
			_dataChangeCallback = callback;

			_corePlatformBriefing = mission.Briefing;
			_eventSpaceMax = Platform.Tie.Briefing.EventQuantityLimit * 2;
			_craftAbbrev = CraftDataManager.GetInstance().GetShortNames();

			_maxStringLength = 158;  // 160 bytes max
			_maxTagLength = 39;      // 40 bytes max

			string installPath = CraftDataManager.GetInstance().GetInstallPath();
			if (installPath == "")
				installPath = Settings.GetInstance().TiePath;

			setPlatformConfig(Settings.Platform.TIE, installPath);

			_ticksPerSecond = 12;
			_playbackFramerate = 12.25f;   // From TF98

			_flightgroups = importAllFlightgroup(mission.FlightGroups, _craftAbbrev);
			_briefingList = importTie(mission.Briefing);

			loadTieFrontend();
			initBriefing();
			applyFormSize();
			verifyBriefing(true, false);
		}

		public BriefingForm2(Platform.Xvt.Mission mission, DataChangeCallback callback)
		{
			InitializeComponent();
			initCommonControls();
			_dataChangeCallback = callback;

			_corePlatformBriefing = mission.Briefings;
			_eventSpaceMax = Platform.Xvt.Briefing.EventQuantityLimit * 2;
			_craftAbbrev = CraftDataManager.GetInstance().GetShortNames();

			// If setting the BoP mission flag on a file (or new mission), it reports the default path.  So manually check for BoP too.
			string installPath = CraftDataManager.GetInstance().GetInstallPath();
			if (installPath == "" || (mission.IsBop && installPath.IndexOf("BalanceOfPower", StringComparison.OrdinalIgnoreCase) == -1))
				installPath = mission.IsBop ? Settings.GetInstance().BopPath : Settings.GetInstance().XvtPath;
			
			Settings.Platform platform = mission.IsBop ? Settings.Platform.BoP : Settings.Platform.XvT;
			setPlatformConfig(platform, installPath);

			_ticksPerSecond = 24;
			_playbackFramerate = 21.30f;

			_flightgroups = importAllFlightgroup(mission.FlightGroups, _craftAbbrev);
			_briefingList = importXvt(mission.Briefings);
			_titleString = "*Defined in .LST file*";
			resolvePlayerNumbers(_flightgroups);

			for (int i = 0; i < 10; i++)
				_teamNames[i] = mission.Teams[i].Name;

			lblStringHint.Text += "\r\nBackslash plus number for special highlights '\\1' to '\\6'.  Double backslash '\\\\' for literal backslash.  Use the map to experiment.";

			loadXvtTitle(mission.MissionPath);

			loadXvtFrontend();
			initBriefing();
			applyFormSize();
			verifyBriefing(true, false);
		}

		public BriefingForm2(Platform.Xwa.Mission mission, DataChangeCallback callback)
		{
			InitializeComponent();
			initCommonControls();
			_dataChangeCallback = callback;

			_corePlatformBriefing = mission.Briefings;
			_eventSpaceMax = Platform.Xwa.Briefing.EventQuantityLimit * 2;
			_craftAbbrev = CraftDataManager.GetInstance().GetShortNames();
			_craftAbbrev[0] = "None";
			_craftNames = CraftDataManager.GetInstance().GetLongNames();
			_craftNames[0] = "None";

			// The main reason for this part is handling of the DSII icon name.  It will usually show up as
			// a backdrop entry, but we should have something more descriptive, while also accounting for
			// modified entries like TFTC.  The assumption here is that abbreviations marked invalid "*"
			// should fall back to their full name instead, for the purposes of the craft type dropdown.
			// But this does rely on the shiplists being properly marked.
			if (_craftAbbrev.Length == _craftNames.Length)
			{
				for (int i = 0; i < _craftNames.Length; i++)
				{
					if (_craftAbbrev[i].StartsWith("*"))
						_craftAbbrev[i] = _craftNames[i];
				}
			}

			string installPath = CraftDataManager.GetInstance().GetInstallPath();
			if (installPath == "")
				installPath = Settings.GetInstance().XwaPath;

			setPlatformConfig(Settings.Platform.XWA, installPath);

			// Auto detect if XWAU is present for playback speed adjustment.
			if (installPath != "")
				chkHighDef.Checked = isXwauInstalled(installPath);

			_ticksPerSecond = 24;
			_playbackFramerate = (chkHighDef.Checked ? 24.00f : 21.30f);  // Faster in XWAU than vanilla

			_flightgroups = new List<AbstractFlightgroup>();   // Not used, but needs to exist
			_briefingList = importXwa(mission.Briefings);

			for (int i = 0; i < 10; i++)
				_teamNames[i] = mission.Teams[i].Name;

			for (int i = 0; i < 4; i++)
				_regionNames[i] = mission.Regions[i].Name;

			lblStringHint.Text += "\r\nBackslash plus number for special highlights '\\1' to '\\6'.  Double backslash '\\\\' for literal backslash.  Use the map to experiment.";

			loadXwaSounds(mission.MissionPath);
			loadXwaTitle(mission.MissionPath);

			loadXwaFrontend();
			initBriefing();
			applyFormSize();
			verifyBriefing(true, false);
		}
		#endregion Constructors

		#region Platform initialization
		void loadXwingFrontend()
		{
			setXwingCanvas(chkHighDef.Checked);

			// This is a combined list of craft types and icons, plus some other things.
			// Icon availability depends on game.
			cboCraftType.Items.Clear();
			cboCraftType.Items.AddRange(Platform.Xwing.Strings.BriefingObjectType);
			cboCraftType.SelectedIndex = 1;

			cmdNewShip.Visible = true;

			// Moves these controls so they occupy the space used by the XWA buttons in the designer.
			cmdNewShip.Top = cmdBreak.Top + 28;
			cmdPanel3.Top = cmdNewShip.Top + 28;
			cmdPanel4.Top = cmdNewShip.Top + 28;
			alignTempPanelBottom(cmdPanel4);

			// Page types need to be populated before selecting the indices when rebuilding the list.
			refreshXwingTabPageTemplates();
			rebuildXwingBriefingList();
	
			cboCurrentBriefing.SelectedIndex = 0;
			cboXwingNewType.SelectedIndex = 0;
		}

		void loadTieFrontend()
		{
			// The official size in game is 292, 147
			// The title is a special case.  A rendering quirk in game allows vertical overflows all the way to
			// the bottom.  To emulate this, the title canvas must extend to the bottom for blitting purposes.
			_combinedCanvas = new Bitmap(292, 12 + 113 + 22);
			_titleCanvas = new Bitmap(292, 12 + 113 + 22);
			_mapCanvas = new Bitmap(292, 113);
			_captionCanvas = new Bitmap(292, 22);
			
			for (int i = 0; i < _briefingList.Count; i++)
				cboCurrentBriefing.Items.Add("Briefing #" + (i + 1));
	
			cboCurrentBriefing.SelectedIndex = 0;
			cboCurrentBriefing.Enabled = false;

			alignTempPanelBottom(cmdBreak);
		}

		void loadXvtFrontend()
		{
			// The official size in game is 360, 236
			// The title height is 12, but the title string is rendered elsewhere, not part of the map rectangle.
			// It's also a bit too small to display the actual title text here, so we'll increase it slightly.
			// The caption height is 28.  Since the title isn't used, this leaves 236 - 28 = 208 for the map.
			// However there seems to be an extra pixel for the map.  Not sure where it comes from, but it can be
			// seen by measuring the grid lines.
			_combinedCanvas = new Bitmap(360, 12 + 2 + 209 + 28);
			_titleCanvas = new Bitmap(360, 12 + 2);
			_mapCanvas = new Bitmap(360, 209);
			_captionCanvas = new Bitmap(360, 28);
			
			for (int i = 0; i < _briefingList.Count; i++)
				cboCurrentBriefing.Items.Add("Briefing #" + (i + 1));
	
			cboCurrentBriefing.SelectedIndex = 0;

			alignTempPanelBottom(cmdBreak);
		}

		void loadXwaFrontend()
		{
			// The region text is bigger than any of the loaded fonts we use.  We could load from the game
			// assets, but it's easier to use a system font.  It doesn't need to be pixel accurate.
			_regionFont = new System.Drawing.Font("Arial", 12f, FontStyle.Bold);

			// The official size in game is 510, 335
			// We'll keep the title space above the map for consistency with other platforms.
			_combinedCanvas = new Bitmap(510, 12 + 293 + 42);
			_titleCanvas = new Bitmap(510, 12);
			_mapCanvas = new Bitmap(510, 293);
			_captionCanvas = new Bitmap(510, 42);

			for (int i = 0; i < _briefingList.Count; i++)
				cboCurrentBriefing.Items.Add("Briefing #" + (i + 1));

			cboRegion.Items.AddRange(_regionNames);
			cboRegion.SelectedIndex = 0;
	
			cboCurrentBriefing.SelectedIndex = 0;

			cmdRegion.Visible = true;
			cmdNewShip.Visible = true;
			cmdMoveShip.Visible = true;
			cmdRotate.Visible = true;
			cmdShipInfo.Visible = true;
			cmdShipInfoText.Visible = true;

			cboMoveIncrement.Items.Add("MoveIcon");
			lblCaptionNotes.Enabled = true;
			txtCaptionNotes.Enabled = true;
			cmdMoveReset.Enabled = false;
			refreshIconAcceleration();
			refreshFormTitle();
		}
		
		/// <summary>Finalizes form controls based on the assigned platform and loaded mission information, and initializes the selected briefing.</summary>
		void initBriefing()
		{
			bool temp = _loading;
			_loading = true;

			for (int i = 0; i < _mapFgTags.Length; i++) _mapFgTags[i] = new MapElement();
			for (int i = 0; i < _mapTextTags.Length; i++) _mapTextTags[i] = new MapElement();
			for (int i = 0; i < _mapIcons.Length; i++) _mapIcons[i] = new MapElement();

			// Tick rate is different depending on platform.
			hsbTimer.LargeChange = _ticksPerSecond / 2;
			numDurationSec.Maximum = (decimal)Math.Ceiling((double)MaxEventTime / _ticksPerSecond);

			// ~60 FPS desired, but the actual firing rate is constrained by the resolution of the system clock.
			// It will almost always be slower than whatever it's set to.  Results seem to vary.
			tmrBrief.Interval = 1000 / 60;
			_playbackLastSystemTime = Environment.TickCount;
			tmrBrief.Start();

			string[] tagColors;
			if (isXwing) tagColors = new string[] { "Default" };
			else if (isTie) tagColors = new string[] { "Green", "Red", "Purple", "Blue", "* Red2", "* Bright Red", "* Darkest Gray", "* Light Gray", "* Tan", "* Med Gray", "* Dark Gray", "* Brown", "* Black" };
			else if (isXvt) tagColors = new string[] { "Green", "Red", "Yellow", "Blue", "Purple", "* Black" };
			else tagColors = new string[] { "Green", "Red", "Yellow", "Blue", "Purple", "* Dark Blue", "* Black" };
			
			cboTextTagColor.Items.AddRange(tagColors);
			cboTextTagColor.SelectedIndex = 0;

			// If the names in the mission file are empty, give them a placeholder.
			for (int i = 0; i < 10; i++)
			{
				if (string.IsNullOrWhiteSpace(_teamNames[i]))
					_teamNames[i] = "Team #" + (i + 1);
			}
			lstTeams.Items.AddRange(_teamNames);

			for (int i = 0; i < 4; i++)
			{
				if (string.IsNullOrWhiteSpace(_regionNames[i]))
					_regionNames[i] = "Region " + (i + 1);
			}

			pnlEditContainer.Width = pnlTempCreate.Width;
			buildEventTypeList();

			if (isXwing) _iffNames = Platform.Xwing.Strings.IFF;
			else _iffNames = new string[] {"Rebel", "Imperial", "Blue", "Yellow", "Red", "Purple" };

			cboIff.Items.AddRange(_iffNames);
			cboIff.SelectedIndex = 0;

			if (isXwa)
			{
				cboCraftType.Items.AddRange(_craftAbbrev);
				cboCraftType.SelectedIndex = 0;
			}

			selectBriefing(0, true);
			resetBriefing();
			refreshCanvas(RefreshFlags.All);

			resetSidebar();

			bool isMulti = (isXvt || isXwa);
			chkXwaEffects.Enabled = isXwa;
			chkAudioPlayback.Enabled = isXwa;
			chkIconShadows.Enabled = (!isXwing);    // Mostly for XWA, but also for disabled waypoints in TF and XvT
			if (!isXwa) chkIconShadows.Text = "Waypoint Shadows";
			cboShadowFilter.Enabled = isXwa;
			// HighDef controls XW94 vs XW98 (canvas + framerate), or XWA vs XWAU/TFTC (framerate)
			chkHighDef.Enabled = (isXwing || isXwa);
			if (isXwa) chkHighDef.Text = "XWAU Framerate";
			chkTeamPlayerLabels.Enabled = isMulti;
			rebuildSidebarConfig();

			lblTeams.Enabled = isMulti;
			lstTeams.Enabled = isMulti;

			// Default text includes player numbers too, but that's only for XvT
			if (isXwa) chkTeamPlayerLabels.Text = "Show Team Label";

			optStateOn.Checked = true;
			cboPathAction.SelectedIndex = 0;

			KeyPreview = true;
			_isFinishedLoading = true;
			_loading = temp;
		}

		/// <summary>Loads persistent configuration settings and applies them.</summary>
		void loadConfig()
		{
			_config = BriefingConfigsCollection.GetConfig(_platform);
			assignConfigValues();
		}

		/// <summary>Applies persistent configuration settings to the form.</summary>
		void assignConfigValues()
		{
			setRadio(optConfigDynamicScale, _config.DynamicScaleEnabled);
			safeSetNumeric(numConfigMapScale, (decimal)_config.FixedMapScale);
			safeSetCbo(cboConfigRenderMode, _config.InterpolationMode);
			chkConfigAutoNearest.Checked = _config.AutoNearest;
			safeSetNumeric(numTimelineFontSize, _config.TimelineFontSize);
			safeSetNumeric(numTimelineRowCount, _config.TimelineMinimumRows);
			chkConfigAutoDuration.Checked = _config.AutoExtendDuration;
			safeSetNumeric(numConfigAutoDuration, _config.AutoExtendAmount);
			chkAnimateMoveZoom.Checked = _config.AnimateMoveZoom;
			chkAnimateTags.Checked = _config.AnimateTags;
			chkLoopPlayback.Checked = _config.LoopPlayback;
			chkTeamPlayerLabels.Checked = _config.TeamPlayerLabels;
			chkTimelinePlayback.Checked = _config.TimelinePlayback;
			chkXwaEffects.Checked = _config.TransitionEffects;
			chkAudioPlayback.Checked = _config.AudioPlayback;
			chkIconShadows.Checked = _config.IconShadows;
			chkHighDef.Checked = _config.HighDef;
			chkFreeLookInfo.Checked = _config.FreeLookInfo;
			safeSetCbo(cboMoveIncrement, _config.MoveIncrement);
			chkMoveSnapGrid.Checked = _config.MoveSnapGrid;
			chkMoveSnapMouse.Checked = _config.MoveSnapMouse;

			safeSetCbo(cboTimeShift, _config.TimeShiftIncrement);
			chkAutoPageBreak.Checked = _config.AutoPageBreak;
			chkAutoTitle.Checked = _config.AutoTitle;
			chkAutoShipInfoEnd.Checked = _config.AutoShipInfoEnd;
			safeSetNumeric(numShipInfoTime, (decimal)_config.ShipInfoTime);
			chkAutoShipInfoJump.Checked = _config.AutoShipInfoJump;
			chkAdvanceTime.Checked = _config.IconAdvanceTime;

			chkEventListDifference.Checked = _config.EventListDifference;
			chkEventListTickTime.Checked = _config.EventListTickTime;
			safeSetCbo(cboEventShift, _config.EventShift);
			setRadio(optEventListShiftAll, _config.EventListShiftAll);
			chkEventListShiftJump.Checked = _config.EventListShiftJump;

			chkExportEverything.Checked = _config.ExportEverything;
			chkExportStrings.Checked = _config.ExportStrings;
			chkExportEmptyString.Checked = _config.ExportEmptyString;
			chkExportEvents.Checked = _config.ExportEvents;
			chkExportFlightgroup.Checked = _config.ExportFlightgroup;
			chkExportXwing.Checked = _config.ExportXwing;

			// Width and Height are assigned later in the load process after everything has been instantiated
		}

		/// <summary>Saves persistent configuration settings for the current platform after pulling their values from the frontend controls.</summary>
		void saveConfig()
		{
			_config.DynamicScaleEnabled = optConfigDynamicScale.Checked;
			_config.FixedMapScale = (float)numConfigMapScale.Value;
			_config.InterpolationMode = (byte)cboConfigRenderMode.SelectedIndex;
			_config.AutoNearest = chkConfigAutoNearest.Checked;
			_config.TimelineFontSize = (byte)numTimelineFontSize.Value;
			_config.TimelineMinimumRows = (byte)numTimelineRowCount.Value;
			_config.AutoExtendDuration = chkConfigAutoDuration.Checked;
			_config.AutoExtendAmount = (byte)numConfigAutoDuration.Value;
			_config.Width = (short)Width;
			_config.Height = (short)Height;
			_config.AnimateMoveZoom = chkAnimateMoveZoom.Checked;
			_config.AnimateTags = chkAnimateTags.Checked;
			_config.LoopPlayback = chkLoopPlayback.Checked;
			_config.TeamPlayerLabels = chkTeamPlayerLabels.Checked;
			_config.TimelinePlayback = chkTimelinePlayback.Checked;
			_config.TransitionEffects = chkXwaEffects.Checked;
			_config.AudioPlayback = chkAudioPlayback.Checked;
			_config.IconShadows = chkIconShadows.Checked;
			_config.HighDef = chkHighDef.Checked;

			_config.FreeLookInfo = chkFreeLookInfo.Checked;
			_config.MoveIncrement = (byte)cboMoveIncrement.SelectedIndex;
			_config.MoveSnapGrid = chkMoveSnapGrid.Checked;
			_config.MoveSnapMouse = chkMoveSnapMouse.Checked;

			_config.TimeShiftIncrement = (byte)cboTimeShift.SelectedIndex;
			_config.AutoPageBreak = chkAutoPageBreak.Checked;
			_config.AutoTitle = chkAutoTitle.Checked;
			_config.AutoShipInfoEnd = chkAutoShipInfoEnd.Checked;
			_config.ShipInfoTime = (float)numShipInfoTime.Value;
			_config.AutoShipInfoJump = chkAutoShipInfoJump.Checked;
			_config.IconAdvanceTime = chkAdvanceTime.Checked;

			_config.EventListDifference = chkEventListDifference.Checked;
			_config.EventListTickTime = chkEventListTickTime.Checked;
			_config.EventShift = (byte)cboEventShift.SelectedIndex;
			_config.EventListShiftAll = optEventListShiftAll.Checked;
			_config.EventListShiftJump = chkEventListShiftJump.Checked;

			_config.ExportEverything = chkExportEverything.Checked;
			_config.ExportStrings = chkExportStrings.Checked;
			_config.ExportEmptyString = chkExportEmptyString.Checked;
			_config.ExportEvents = chkExportEvents.Checked;
			_config.ExportFlightgroup = chkExportFlightgroup.Checked;
			_config.ExportXwing = chkExportXwing.Checked;
		}

		string getPlatformInstallPath(Settings.Platform platform)
		{
			string ret = "";
			if (platform == Settings.Platform.XWING && Settings.GetInstance().XwingInstalled) ret = Settings.GetInstance().XwingPath;
			else if (platform == Settings.Platform.TIE && Settings.GetInstance().TieInstalled) ret = Settings.GetInstance().TiePath;
			else if (platform == Settings.Platform.XvT && Settings.GetInstance().XvtInstalled) ret = Settings.GetInstance().XvtPath;
			else if (platform == Settings.Platform.BoP && Settings.GetInstance().BopInstalled) ret = Settings.GetInstance().BopPath;
			else if (platform == Settings.Platform.XWA && Settings.GetInstance().XwaInstalled) ret = Settings.GetInstance().XwaPath;
			return ret;
		}

		/// <summary>Assigns the working platform and tries to load platform-specific assets like fonts and bitmaps.</summary>
		void setPlatformConfig(Settings.Platform platform, string path)
		{
			string fontPath = path;

			// As far as the editor functionality is concerned, there aren't any special considerations for
			// BoP.  However for the sake of normal runtime logic, everything will check for XvT instead.
			// NOTE: BoP -does- matter when loading assets.
			bool isBop = false;
			if (platform == Settings.Platform.BoP)
			{
				// Fall back to XvT logic for runtime operation.
				isBop = true;
				platform = Settings.Platform.XvT;
			}

			if (platform == Settings.Platform.XvT)
			{
				// Fonts only exist in the base folder.
				int pos = path.IndexOf("BalanceOfPower", StringComparison.OrdinalIgnoreCase);
				if (pos >= 0)
				{
					// For icons, ensure it's using the correct offsets for BoP.
					isBop = true;
					fontPath = path.Remove(pos);
				}
			}
			
			// The platform must be assigned before the loading functions are run.
			_platform = platform;
			loadConfig();

			// The IFF colors need to be prepared first.  The color is used when loading assets directly
			// into the craft icon bitmap cache.
			generateColors();
			loadPlatformFonts(fontPath);
			loadPlatformIcons(path, isBop);
			generateLoadReport();
		}

		/// <summary>Initializes common controls that don't have platform-specific values or customization applied.  This occurs before the platform and configuration settings are loaded.</summary>
		void initCommonControls()
		{
			bool temp = _loading;
			_loading = true;

			// The form is wider in the designer to display all the controls.  Hide them by returning to
			// its default width.  The form will resize again after the platform is fully loaded.
			int reduce = tabBriefing.ClientRectangle.Right - (pnlEditContainer.Left + pnlTempCreate.Width + pnlEditContainer.Margin.Right);
			tabBriefing.Width -= reduce;
			pnlEditContainer.Width -= reduce;
			Width -= reduce;

			// NOTE: If a control is added here, add a corresponding dispose in the _FormClosed event.
			convertButtonImage(cmdFF);
			convertButtonImage(cmdPlay);
			convertButtonImage(cmdPause);
			convertButtonImage(cmdStop);
			convertButtonImage(cmdPrev);
			convertButtonImage(cmdNext);
			convertButtonImage(cmdMoveTagUp);
			convertButtonImage(cmdMoveTagDown);
			convertButtonImage(cmdMoveStringUp);
			convertButtonImage(cmdMoveStringDown);
			convertButtonImage(cmdEventListGroupUp);
			convertButtonImage(cmdEventListGroupDown);
			convertButtonImage(cmdEventListShiftUp);
			convertButtonImage(cmdEventListShiftDown);
			convertButtonImage(cmdXwingPageUp);
			convertButtonImage(cmdXwingPageDown);

			isolateScrollbar(hsbBRF);
			isolateScrollbar(vsbBRF);
			isolateScrollbar(vsbTimeline);

			_moveButtons = new Button[9] { cmdMoveSW, cmdMoveS, cmdMoveSE,   cmdMoveW, null, cmdMoveE,   cmdMoveNW, cmdMoveN, cmdMoveNE };
			foreach (Button b in _moveButtons)
			{
				if (b == null) continue;

				convertButtonImage(b);
				b.MouseClick += iconMoveButton_Click;
				b.MouseEnter += iconMoveButton_MouseEnter;
				b.MouseLeave += iconMoveButton_MouseLeave;
			}
			
			// MouseEnter doesn't seem to work properly depending on which part of the control is hovered.
			// MouseLeave handles mouse interaction while LostFocus handles keyboard interaction (such as tabbing to a different control)
			numIconStepX.MouseLeave += iconMoveStep_MouseLeave;
			numIconStepY.MouseLeave += iconMoveStep_MouseLeave;
			numIconStepX.LostFocus += iconMoveStep_MouseLeave;
			numIconStepY.LostFocus += iconMoveStep_MouseLeave;

			pctBrief.MouseWheel += pctBrief_MouseWheel;
			pctTimeline.MouseWheel += pctTimeline_MouseWheel;

			// This dropdown needs to be populated before the values are loaded from config.
			foreach (string e in Enum.GetNames(typeof(System.Drawing.Drawing2D.InterpolationMode)))
			{
				// Invalid is -1, so all valid enums will have the proper zero-based ordering.
				if (e != "Invalid")
					cboConfigRenderMode.Items.Add(e);
			}
			cboConfigRenderMode.SelectedIndex = 0;

			string[] shiftIncrements = getTimeShiftIncrementStrings();
			cboTimeShift.Items.AddRange(shiftIncrements);
			cboEventShift.Items.AddRange(shiftIncrements);
			cboTimeShift.SelectedIndex = 0;
			cboEventShift.SelectedIndex = 0;

			cboRotation.Items.AddRange(_iconRotations);
			cboRotation.SelectedIndex = 0;

			cboPathNodeRot.Items.AddRange(_iconRotations);
			cboPathNodeRot.SelectedIndex = 0;

			cboPathAutoType.SelectedIndex = 0;

			cboShadowFilter.Items.Add("All Icons");
			cboShadowFilter.Items.Add("All except null");
			cboShadowFilter.SelectedIndex = 0;

			cboMoveIncrement.SelectedIndex = 0;
			cboPathAutoRotate.SelectedIndex = 1;
			cboPathDefaultFacing.SelectedIndex = 0;

			// Initially nothing is selected, so make sure it's disabled if the user opens the tab.
			pnlEventListManage.Enabled = false;

			// Panel mode was a late addition.  The map tab was full, so the event tab was used instead.
			// Move it to the correct tab.
			pnlEditContainer.Controls.Add(pnlPanelMode);
			lblMapPanelMode.Visible = false;
			tabDisplay.Controls.Add(lblMapPanelMode);

			_loading = temp;

			_scaler = new FormScaler(this, BriefingForm2_ExternalResize);
		}

		/// <summary>Applies the form size and scale from its configuration.</summary>
		void applyFormSize()
		{
			// Needs to be set after the platform and canvas is initialized, but before everything else here.
			MinimumSize = getMinimumFormSize();

			if (_config.Width > 0 && _config.Height > 0)
			{
				Width = _config.Width;
				Height = _config.Height;
			}

			updateFormSize(true);
		}
		
		/// <summary>Creates a list of all briefing events used by this platform, and assigns them to the event list dropdown.</summary>
		void buildEventTypeList()
		{
			List<AbstractEventType> list = new List<AbstractEventType>
			{
				AbstractEventType.SkipMarker,
				AbstractEventType.PageBreak,
				AbstractEventType.TitleText,
				AbstractEventType.CaptionText
			};

			if (isXwing)
			{
				list.Add(AbstractEventType.PanelText3);
				list.Add(AbstractEventType.PanelText4);
			}

			list.Add(AbstractEventType.MoveMap);
			list.Add(AbstractEventType.ZoomMap);
			list.Add(AbstractEventType.ClearFgTags);
			list.Add(AbstractEventType.FgTag1);
			list.Add(AbstractEventType.FgTag2);
			list.Add(AbstractEventType.FgTag3);
			list.Add(AbstractEventType.FgTag4);

			if (!isXwing)
			{
				list.Add(AbstractEventType.FgTag5);
				list.Add(AbstractEventType.FgTag6);
				list.Add(AbstractEventType.FgTag7);
				list.Add(AbstractEventType.FgTag8);
			}
			
			list.Add(AbstractEventType.ClearTextTags);
			list.Add(AbstractEventType.TextTag1);
			list.Add(AbstractEventType.TextTag2);
			list.Add(AbstractEventType.TextTag3);
			list.Add(AbstractEventType.TextTag4);

			if (!isXwing)
			{
				list.Add(AbstractEventType.TextTag5);
				list.Add(AbstractEventType.TextTag6);
				list.Add(AbstractEventType.TextTag7);
				list.Add(AbstractEventType.TextTag8);
			}

			if (isXwa)
			{
				list.Add(AbstractEventType.XwaSetIcon);
				list.Add(AbstractEventType.XwaShipInfo);
				list.Add(AbstractEventType.XwaMoveIcon);
				list.Add(AbstractEventType.XwaRotateIcon);
				list.Add(AbstractEventType.XwaChangeRegion);
				list.Add(AbstractEventType.XwaInfoParagraph);
			}

			cboEventType.Items.Clear();
			foreach (AbstractEventType type in list)
			{
				EventDef def = EventDef.GetEventDefByType(type);
				cboEventType.Items.Add(def.Name);
			}
			cboEventType.SelectedIndex = 0;
		}

		Color[] convertColorIntArray(int[] colors)
		{
			Color[] result = new Color[colors.Length];
			for (int i = 0; i < colors.Length; i++)
				result[i] = Color.FromArgb((int)(0xFF000000 | colors[i]));

			return result;
		}

		Color[][] convertColorIntMultiArray(int[][] colors)
		{
			Color[][] result = new Color[colors.Length][];
			for (int i = 0; i < colors.Length; i++)
				result[i] = convertColorIntArray(colors[i]);
			return result;
		}

		Color[] makeColorArray(params int[] values) => convertColorIntArray(values);

		/// <summary>Loads all colors and palettes for the currently assigned platform.</summary>
		private void generateColors()
		{
			// NOTE: If the StandardColor enum changes, the order must be adjusted here for all platforms.
			// The order is: Default, Highlight, Center, Panel

			// Default text is always white.  Highlight is for [brackets] and typically green, except for XWA.
			// Centered captions (prefix ">") are always some shade of yellow.
			// Panel is the background for title and captions, always blue.
			// XWING is from 1994 screenshot.  TIE is from DOS 1995 screenshot.
			if    (isXwing) _standardColors = makeColorArray(0xF7EFEF, 0x00AA00, 0xE3FF00, 0x0000AA);
			else if (isTie) _standardColors = makeColorArray(0xF7EFEF, 0x00AA00, 0xE3FF00, 0x0000AA);
			else if (isXvt) _standardColors = makeColorArray(0xFFFFFF, 0x40C440, 0xFFFF00, 0x000080);
			else if (isXwa) _standardColors = makeColorArray(0xFFFFFF, 0x6080FF, 0xFFFF00, 0x404080);

			// Only XvT and XWA have inline color codes.
			if (isXvt)
			{
				_inlineTextColors = makeColorArray(
					0xFFFFFF,  // White
					0x40C440,  // Green
					0xFF0000,  // Red
					0xFFFF00,  // Yellow
					0x0000FF,  // Blue
					0x8080FF,  // Purple
					0x020202   // Array overflow, appears black.  Intensity 0 is transparent and 1 is reserved for shadow.
				);
			}
			else if (isXwa)
			{
				_inlineTextColors = makeColorArray(
					0xFFFFFF,  // White
					0x6080FF,  // Blue
					0xFF0000,  // Red
					0xFFFF00,  // Yellow
					0x3232FF,  // Dark blue
					0x8080FF   // Purple
				);
			}

			if (isXwing)
			{
				// From XW94 screenshot.
				_textTagColors = makeColorArray(0x59DFFF);
				_textTagPalettes = new Color[1][] { makeColorArray(0x00085D, 0x0C3C92, 0x2C86C7, 0x59DFFF, 0x59DFFF) };

				// Colors from loaded bitmaps:  Default   Green     Red       Blue
				_iconIffColors = makeColorArray(0x87DB00, 0x87DB00, 0xFF2700, 0x5BDFFF);
			}
			else if (isTie)
			{
				// TIE Colors, taken from DOS 1995.
				_textTagColors = makeColorArray(
					0x00AE00,  // Green
					0xAE0000,  // Red
					0xAE00AE,  // Purple
					0x003CAE,  // Blue
					0xAA0000,  // Overflow colors begin here.  Red 2
					0xFF5555,  // Bright Red
					0x454545,  // Darkest gray
					0xCFCFCF,  // Light gray
					0xC7BEAA,  // Tan
					0x797975,  // Medium gray
					0x595549,  // Dark gray
					0x695D30,  // Brown
					0x201810   // Black
				);

				// From DOS screenshot.
				int[][] palettes = new int[13][] {
					new int[] { 0x004900, 0x006100, 0x007900, 0x009600, 0x00AE00, 0x00CB00, 0x00E300, 0x00FF00 },  // 0 = Green
					new int[] { 0x490000, 0x610000, 0x790000, 0x960000, 0xAE0000, 0xCB0000, 0xE30000, 0xFF0000 },  // 1 = Red
					new int[] { 0x490049, 0x610061, 0x790079, 0x960096, 0xAE00AE, 0xCB00CB, 0xE300E3, 0xFF00FF },  // 2 = Purple
					new int[] { 0x000049, 0x000461, 0x001079, 0x002496, 0x003CAE, 0x0059CB, 0x0079E3, 0x00A2FF },  // 3 = Blue
					new int[] { 0x000000, 0x0000AA, 0x00AA00, 0x00AAAA, 0xAA0000, 0xAA00AA, 0xAA5500, 0xAAAAAA },  // 4 = * Red (Overflow begins, standard DOS palette, low intensity.)
					new int[] { 0x555555, 0x5555FF, 0x55FF55, 0x55FFFF, 0xFF5555, 0xFF55FF, 0xFFFF55, 0xFFFFFF },  // 5 = * Bright Red (standard DOS palette, high intensity)
					new int[] { 0x000000, 0x101010, 0x202020, 0x303030, 0x454545, 0x555555, 0x656565, 0x757575 },  // 6 = * Darkest Gray
					new int[] { 0x8A8A8A, 0x9A9A9A, 0xAAAAAA, 0xBABABA, 0xCFCFCF, 0xDFDFDF, 0xEFEFEF, 0xFFFFFF },  // 7 = * Light Gray
					new int[] { 0xEFE7CF, 0xE7DBC7, 0xDFD3BE, 0xD3CBB6, 0xC7BEAA, 0xBAB2A2, 0xAA9E8E, 0x9A9286 },  // 8 = * Tan
					new int[] { 0x9E9279, 0x968A75, 0x8A8271, 0x827D71, 0x797975, 0x757175, 0x757569, 0x716D61 },  // 9 = * Medium Gray
					new int[] { 0x756D59, 0x6D6959, 0x696151, 0x5D5951, 0x595549, 0x595141, 0x514538, 0x5D413C },  // 10 = * Dark Gray
					new int[] { 0x654D38, 0x655141, 0x695949, 0x695D41, 0x695D30, 0x554D30, 0x4D4128, 0x453820 },  // 11 = * Brown
					new int[] { 0x3C3420, 0x3C281C, 0x302814, 0x282010, 0x201810, 0x201010, 0x281010, 0x200C08 },  // 12 = * Black
				};

				_textTagPalettes = convertColorIntMultiArray(palettes);
				_iconIffPaletteIndices = new int[6] { 0, 1, 3, 2, 1, 2 };  // Green, Red, Blue, Purple, Red, Purple

				// From loaded bitmaps:         Green     Red       Blue      Purple    Red       Purple
				_iconIffColors = makeColorArray(0x00FF00, 0xFF0000, 0x00A3FF, 0xFF00FF, 0xFF0000, 0xFF00FF);
			}
			else if (isXvt)
			{
				// Colors:                      Green     Red       Yellow    Blue      Purple    * Black
				_textTagColors = makeColorArray(0x00AC00, 0xAC0000, 0xACAC00, 0x0000AC, 0xAC00AC, 0x000000);

				// From screenshot
				int[][] palettes = new int[6][] {
					new int[] { 0x004800, 0x006000, 0x007800, 0x009400, 0x00AC00, 0x00C800, 0x00E000, 0x00FC00 }, // 0 = Green
					new int[] { 0x480000, 0x600000, 0x780000, 0x900000, 0xA80000, 0xC80000, 0xE00000, 0xF80000 }, // 1 = Red
					new int[] { 0x484800, 0x606000, 0x787800, 0x909400, 0xA8AC00, 0xC8C800, 0xE0E000, 0xF8FC00 }, // 2 = Yellow
					new int[] { 0x000048, 0x000060, 0x000078, 0x000090, 0x0000A8, 0x0000C8, 0x0000E0, 0x0000F8 }, // 3 = Blue
					new int[] { 0x480048, 0x600060, 0x780078, 0x900090, 0xA800A8, 0xC800C8, 0xE000E0, 0xF800F8 }, // 4 = Purple
					new int[] { 0x000008, 0x000000, 0x000000, 0x000000, 0x000000, 0x000000, 0x000000, 0x000000 }, // 5 = * Black
				};

				_textTagPalettes = convertColorIntMultiArray(palettes);
				_iconIffPaletteIndices = new int[6] { 0, 1, 3, 2, 1, 4 };  // Green, Red, Blue, Yellow, Red, Purple

				// From screenshot:             Green     Red       Blue      Yellow    Red       Purple
				_iconIffColors = makeColorArray(0x38D400, 0xF82400, 0x58DCF8, 0xF8FC00, 0xF82400, 0x9088F0);
			}
			else if (isXwa)
			{
				// Colors:                      Green     Red       Yellow    Blue      Purple    *DarkBlu, *Black
				_textTagColors = makeColorArray(0x00E000, 0xE00000, 0xE0E000, 0x6060E0, 0xE000E0, 0x0004A0, 0x000000);

				// From screenshot
				int[][] palettes = new int[7][] {
					new int[] { 0x004800, 0x006000, 0x007800, 0x009400, 0x00AC00, 0x00C800, 0x00E000, 0x00FC00 }, // 0 = Green
					new int[] { 0x480000, 0x600000, 0x780000, 0x900000, 0xA80000, 0xC80000, 0xE00000, 0xF80000 }, // 1 = Red
					new int[] { 0x484800, 0x606000, 0x787800, 0x909400, 0xA8AC00, 0xC8C800, 0xE0E000, 0xF8FC00 }, // 2 = Yellow
					new int[] { 0x101048, 0x202060, 0x202078, 0x303090, 0x4040A8, 0x5050C8, 0x6060E0, 0x6080F8 }, // 3 = Blue
					new int[] { 0x480048, 0x600060, 0x780078, 0x900090, 0xA800A8, 0xC800C8, 0xE000E0, 0xF800F8 }, // 4 = Purple
					new int[] { 0x000078, 0x0000A8, 0x0000E0, 0x000410, 0x000448, 0x000478, 0x0004A0, 0x000000 }, // 5 = * Dark Blue
					new int[] { 0x000000, 0x000000, 0x000000, 0x000000, 0x000000, 0x000000, 0x000000, 0x000000 }, // 6 = * Black
				};
				
				_textTagPalettes = convertColorIntMultiArray(palettes);
				_iconIffPaletteIndices = new int[6] { 0, 1, 3, 2, 1, 4 };  // Green, Red, Blue, Yellow, Red, Purple

				// From screenshot:             Green     Red       Blue      Yellow    Red       Purple
				_iconIffColors = makeColorArray(0x58EC18, 0xF83028, 0x5884F8, 0xF8F078, 0xF83028, 0xE870F8);
			}
		}
		#endregion Platform initialization

		#region Import and export (frontend)
		/// <summary>Saves all data back into the mission briefing container.</summary>
		public void Save()
		{
			if (isFormClosed()) return;

			// Register any final control data changes that could mark dirty.
			Validate();

			if (!_isDirty) return;

			// Push the changes from the frontend into the abstraction layer.
			exportActiveBriefing();

			// Push the changes from the abstraction layer into the mission itself.
			if (isXwing) exportXwing();
			else if (isTie) exportTie();
			else if (isXvt) exportXvt();
			else if (isXwa) exportXwa();

			// Remove dirty mark from title.
			if (Text.EndsWith("*")) Text = Text.Substring(0, Text.Length - 1);

			_isDirty = false;
		}

		/// <summary>Exports the active selected briefing from the editor frontend into the abstraction layer.</summary>
		/// <remarks>This does not save the mission.  The selected briefing is still resident in the frontend for further editing.</remarks>
		void exportActiveBriefing()
		{
			Validate();

			AbstractBriefing brf = _briefingList[_currentBriefingIndex];

			brf.RunningTime = (short)_briefingDuration;

			for (int i = 0; i < 10; i++) brf.ViewedByTeam[i] = lstTeams.GetSelected(i);

			string[] destTags = brf.Tags;
			string[] destStrings = brf.Strings;

			// XWING strings are global to all pages, and stored in the first page.
			if (isXwing)
			{
				destTags = _briefingList[0].Tags;
				destStrings = _briefingList[0].Strings;
			}

			for (int i = 0; i < destTags.Length; i++) destTags[i] = exportString(_tags[i]);
			for (int i = 0; i < destStrings.Length; i++) destStrings[i] = exportString(_strings[i]);

			if (isXwa) for (int i = 0; i < brf.Notes.Length; i++) brf.Notes[i] = _captionNotes[i];

			brf.Events.Clear();
			for (int i = 0; i < _events.Count; i++)
			{
				AbstractEvent evt = new AbstractEvent(_events[i]);

				// Flip text tags to match the flightgroup axis.  FGs are flipped on mission load, not in the abstraction layer.
				if (evt.IsTextTag && isWaypointInvertedPlatform()) evt.Params[2] *= -1;

				brf.Events.Add(evt);
			}
		}

		/// <summary>Imports the selected briefing from the abstraction layer into the editor frontend.</summary>
		void importActiveBriefing()
		{
			var brf = _briefingList[_currentBriefingIndex];

			// For XWING, strings are global to all pages.  TIE only has one page.
			// XvT and XWA have their own set of strings for each briefing.
			string[] srcTags = brf.Tags;
			string[] srcStrings = brf.Strings;

			if (isXwing)
			{
				srcTags = _briefingList[0].Tags;
				srcStrings = _briefingList[0].Strings;
			}

			_tags.Clear();
			_strings.Clear();
			for (int i = 0; i < srcTags.Length; i++) _tags.Add(importString(srcTags[i]));
			for (int i = 0; i < srcStrings.Length; i++) _strings.Add(importString(srcStrings[i]));

			_captionNotes.Clear();
			if (isXwa) for (int i = 0; i < brf.Notes.Length; i++) _captionNotes.Add(brf.Notes[i]);

			for (int i = 0; i < 10; i++) lstTeams.SetSelected(i, brf.ViewedByTeam[i]);

			clearEvents();
			for (int i = 0; i < brf.Events.Count; i++)
			{
				AbstractEvent evt = new AbstractEvent(brf.Events[i]);

				// Flip text tags to match the flightgroup axis.  FGs are flipped on mission load, not in the abstraction layer.
				if (evt.IsTextTag && isWaypointInvertedPlatform()) evt.Params[2] *= -1;

				var def = EventDef.GetEventDefByType(evt.Event);
				_eventSpaceUsed += def.GetSize();
				_events.Add(evt);
			}

			_highestEventTime = getHighestTime();
			setDuration(brf.RunningTime, false);
			updateEventSpace(0);
			populateMainStrings(-1, -1);
			refreshEventList(-1);
		}

		/// <summary>Fully loads a briefing into the frontend, unloading the existing briefing if necessary.</summary>
		/// <param name="index">Briefing to load.  If the index doesn't change, nothing will happen unless loading is forced.</param>
		/// <param name="loading">If true, forces loading even if the index isn't changing.  If false, the existing briefing is unloaded first.</param>
		void selectBriefing(int index, bool loading)
		{
			if (_currentBriefingIndex == index && !loading) return;

			setPanelMode(false);
			setPathMode(false);
			setFreeLookMode(false);
			clearUndo();

			// Completely stop and reset anything that might be active.
			resetBriefing();

			// If we're not loading, it means the user switched the current briefing, so push all changes.
			if (!loading) exportActiveBriefing();
			
			// Load the selected briefing into its initial clean state.
			_currentBriefingIndex = index;
			importActiveBriefing();

			// Reset again to run the first frame.
			resetBriefing();
			rebuildShadowIcons();

			if (isCurrentTab(MainTabIndex.BriefingOptions)) populateBriefingOptions();

			if (isXwing) refreshXwingTempCreateButtons();

			if (isXvt) _currentTeamIndex = getVisibleTeam();

			refreshFormTitle();
			refreshCanvas(RefreshFlags.All);
		}

		/// <summary>Converts a string from raw to frontend, escaping the result.</summary>
		string importString(string s)
		{
			string ret = "";
			
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];

				// Test for broken imports that still have their null terminator characters and remove them.
				if (c == 0)
				{
					// First, last, or double terminators.
					if (i == 0 || i == s.Length - 1) break;
					if (i < s.Length - 1 && s[i + 1] == 0) break;
				}

				// Backslash will be used to represent escaped characters, so double up any existing ones.
				if (c == '\\')
				{
					ret += c;
					ret += c;
				}
				else if (c >= 0 && c <= 9)  //  Convert raw color code to escaped character.
					ret += "\\" + (char)('0' + c);
				else ret += c;
			}
			return ret;
		}

		/// <summary>Converts a string from frontend to raw, unescaping the result.</summary>
		string exportString(string s)
		{
			string ret = "";
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				char next = '\0';
				if (i + 1 < s.Length) next = s[i + 1];

				// Convert escaped characters back to raw.
				if (c == '\\')
				{
					if (next == '\\')
					{
						i++;
						ret += c;
					}
					else if (next >= '0' && next <= '9')
					{
						i++;

						// Protection, anything higher may crash the game.
						if (isXwa && next >= '7') continue;

						ret += (char)(next - '0');
					}
					else ret += c;  // Interpret as a single backslash for safety.
				}
				else ret += c;
			}
			return ret;
		}

		/// <summary>Retrieves a list of all briefing pages from the mission, imported into abstraction format.</summary>
		List<AbstractBriefing> importXwing(Platform.Xwing.Briefing srcBrf)
		{
			List<AbstractBriefing> briefingList = new List<AbstractBriefing>();

			int pgCount = Math.Max(1, srcBrf.Pages.Count);
			for (int i = 0; i < pgCount; i++) briefingList.Add(new AbstractBriefing());

			var baseBrf = briefingList[0];

			// Count is the same for all arrays.
			int strCount = srcBrf.BriefingString.Length;
			baseBrf.AllocateStrings(strCount);

			Array.Copy(srcBrf.BriefingString, baseBrf.Strings, strCount);
			Array.Copy(srcBrf.BriefingTag, baseBrf.Tags, strCount);

			for (int i = 0; i < srcBrf.Pages.Count; i++)
			{
				var srcPg = srcBrf.Pages[i];
				var destBrf = briefingList[i];
				
				destBrf.RunningTime = srcPg.Length;

				foreach (var srcEvt in srcPg.Events)
				{
					// Some pages don't have an End event.  The platform loader substitutes two events (None + EndBriefing).
					// See BONUS3BW.BRF, page #3.
					if (srcEvt.Type == Platform.Xwing.Briefing.EventType.None) break;

					var def = EventDef.GetEventDefByRawXwing((short)srcEvt.Type);
					if (def == null || def.Type == AbstractEventType.None)
					{
						popupError($"Failed to load briefing.  Unexpected event type: {srcEvt.Type}");
						break;
					}

					// We don't handle this event in the editor, it will be automatically generated when saving.
					if (def.Type == AbstractEventType.EndBriefing) break;

					AbstractEvent destEvt = new AbstractEvent
					{
						Event = def.Type,
						Time = srcEvt.Time
					};
					Array.Copy(srcEvt.Variables, destEvt.Params, srcEvt.Variables.Length);
					destBrf.Events.Add(destEvt);
				}
			}

			return briefingList;
		}

		/// <summary>Exports all briefing pages from the abstraction layer, saving them into the mission.</summary>
		void exportXwing()
		{
			var destBrf = getXwingCoreBriefing();

			AbstractBriefing srcBrf = _briefingList[0];

			Array.Copy(srcBrf.Strings, destBrf.BriefingString, srcBrf.Strings.Length);
			Array.Copy(srcBrf.Tags, destBrf.BriefingTag, srcBrf.Tags.Length);

			List<string> report = new List<string>();

			for (int i = 0; i < destBrf.Pages.Count; i++)
			{
				var destPg = destBrf.Pages[i];
				srcBrf = _briefingList[i];

				// Erase and rebuild event list.
				destPg.Events.Clear();
				destPg.Length = srcBrf.RunningTime;

				foreach (var evt in exportXwingEventList(srcBrf.Events, report, i)) destPg.Events.Add(evt);
			}

			if (report.Count > 0) displayExportReport(report);
		}

		/// <summary>Retrieves a list of all briefing pages from the mission, imported into abstraction format.</summary>
		List<AbstractBriefing> importTie(Platform.Tie.Briefing srcBrf)
		{
			List<AbstractBriefing> briefingList = new List<AbstractBriefing>();

			var destBrf = new AbstractBriefing { RunningTime = srcBrf.Length };

			// Count is the same for all arrays.
			int count = srcBrf.BriefingString.Length;
			destBrf.AllocateStrings(count);
			Array.Copy(srcBrf.BriefingString, destBrf.Strings, count);
			Array.Copy(srcBrf.BriefingTag, destBrf.Tags, count);

			foreach (BaseBriefing.Event srcEvt in srcBrf.Events)
			{
				EventDef def = EventDef.GetEventDefByRaw((short)srcEvt.Type);
				if (def == null || def.Type == AbstractEventType.None)
				{
					popupError($"Failed to load briefing.  Unexpected event type: {srcEvt.Type}");
					break;
				}

				// We don't handle this event in the editor, it will be automatically generated when saving.
				if (def.Type == AbstractEventType.EndBriefing) break;

				AbstractEvent destEvt = new AbstractEvent
				{
					Event = def.Type,
					Time = srcEvt.Time
				};
				Array.Copy(srcEvt.Variables, destEvt.Params, srcEvt.Variables.Length);
				destBrf.Events.Add(destEvt);
			}

			briefingList.Add(destBrf);
			return briefingList;
		}
		
		/// <summary>Exports all briefing pages from the abstraction layer, saving them into the mission.</summary>
		void exportTie()
		{
			Platform.Tie.Briefing destBrf = (Platform.Tie.Briefing)_corePlatformBriefing;
			var srcBrf = _briefingList[0];

			// Erase and rebuild event list.
			destBrf.Events.Clear();
			destBrf.Length = srcBrf.RunningTime;

			List<string> report = new List<string>();
			foreach (var evt in exportEventList(srcBrf.Events, report, -1)) destBrf.Events.Add(evt);

			Array.Copy(srcBrf.Strings, destBrf.BriefingString, srcBrf.Strings.Length);
			Array.Copy(srcBrf.Tags, destBrf.BriefingTag, srcBrf.Tags.Length);

			if (report.Count > 0) displayExportReport(report);
		}

		/// <summary>Retrieves a list of all briefing pages from the mission, imported into abstraction format.</summary>
		List<AbstractBriefing> importXvt(Platform.Xvt.BriefingCollection briefing)
		{
			List<AbstractBriefing> briefingList = new List<AbstractBriefing>();
			foreach (var srcBrf in briefing)
			{
				AbstractBriefing destBrf = new AbstractBriefing { RunningTime = srcBrf.Length };

				// Count is the same for all arrays.
				int count = srcBrf.BriefingString.Length;
				destBrf.AllocateStrings(count);
				Array.Copy(srcBrf.BriefingString, destBrf.Strings, count);
				Array.Copy(srcBrf.BriefingTag, destBrf.Tags, count);

				foreach (var srcEvt in srcBrf.Events)
				{
					var def = EventDef.GetEventDefByRaw((short)srcEvt.Type);
					if (def == null || def.Type == AbstractEventType.None)
					{
						popupError($"Failed to load briefing.  Unexpected event type: {srcEvt.Type}");
						break;
					}

					// We don't handle this event in the editor, it will be automatically generated when saving.
					if (def.Type == AbstractEventType.EndBriefing) break;

					AbstractEvent destEvt = new AbstractEvent
					{
						Event = def.Type,
						Time = srcEvt.Time
					};
					Array.Copy(srcEvt.Variables, destEvt.Params, srcEvt.Variables.Length);
					destBrf.Events.Add(destEvt);
				}
				Array.Copy(srcBrf.Team, destBrf.ViewedByTeam, 10);
				briefingList.Add(destBrf);
			}

			return briefingList;
		}
		
		/// <summary>Exports all briefing pages from the abstraction layer, saving them into the mission.</summary>
		void exportXvt()
		{
			Platform.Xvt.BriefingCollection briefing = (Platform.Xvt.BriefingCollection)_corePlatformBriefing;

			for (int i = 0; i < briefing.Count; i++)
			{
				var destBrf = briefing[i];
				var srcBrf = _briefingList[i];

				// Erase and rebuild event list.
				destBrf.Events.Clear();
				destBrf.Length = srcBrf.RunningTime;

				List<string> report = new List<string>();
				foreach (var evt in exportEventList(srcBrf.Events, report, i)) destBrf.Events.Add(evt);

				Array.Copy(srcBrf.Strings, destBrf.BriefingString, srcBrf.Strings.Length);
				Array.Copy(srcBrf.Tags, destBrf.BriefingTag, srcBrf.Tags.Length);
				Array.Copy(srcBrf.ViewedByTeam, destBrf.Team, 10);

				if (report.Count > 0) displayExportReport(report);
			}
		}

		/// <summary>Retrieves a list of all briefing pages from the mission, imported into abstraction format.</summary>
		List<AbstractBriefing> importXwa(Platform.Xwa.BriefingCollection briefing)
		{
			List<AbstractBriefing> briefingList = new List<AbstractBriefing>();
			foreach (var srcBrf in briefing)
			{
				AbstractBriefing destBrf = new AbstractBriefing { RunningTime = srcBrf.Length };

				// Count is the same for all arrays.
				int count = srcBrf.BriefingString.Length;
				destBrf.AllocateStrings(count);
				Array.Copy(srcBrf.BriefingString, destBrf.Strings, count);
				Array.Copy(srcBrf.BriefingTag, destBrf.Tags, count);
				for (int j = 0; j < count; j++) destBrf.Notes[j] = srcBrf.BriefingStringsNotes[j];

				foreach (var srcEvt in srcBrf.Events)
				{
					var def = EventDef.GetEventDefByRaw((short)srcEvt.Type);
					if (def == null || def.Type == AbstractEventType.None)
					{
						// NOTE: commented out to suppress an error on certain custom missions, might address later
						//popupError($"Failed to load briefing.  Unexpected event type: {srcEvt.Type}");
						//break;
						continue;
					}

					// We don't handle this event in the editor, it will be automatically generated when saving.
					if (def.Type == AbstractEventType.EndBriefing) break;

					AbstractEvent destEvt = new AbstractEvent
					{
						Event = def.Type,
						Time = srcEvt.Time
					};
					Array.Copy(srcEvt.Variables, destEvt.Params, srcEvt.Variables.Length);
					destBrf.Events.Add(destEvt);
				}
				Array.Copy(srcBrf.Team, destBrf.ViewedByTeam, 10);
				briefingList.Add(destBrf);
			}

			return briefingList;
		}
		
		/// <summary>Exports all briefing pages from the abstraction layer, saving them into the mission.</summary>
		void exportXwa()
		{
			Platform.Xwa.BriefingCollection briefing = (Platform.Xwa.BriefingCollection)_corePlatformBriefing;
			for (int i = 0; i < briefing.Count; i++)
			{
				var destBrf = briefing[i];
				var srcBrf = _briefingList[i];

				// Erase and rebuild event list.
				destBrf.Events.Clear();
				destBrf.Length = srcBrf.RunningTime;
				List<string> report = new List<string>();
				foreach (BaseBriefing.Event evt in exportEventList(srcBrf.Events, report, i)) destBrf.Events.Add(evt);

				Array.Copy(srcBrf.Strings, destBrf.BriefingString, srcBrf.Strings.Length);
				Array.Copy(srcBrf.Tags, destBrf.BriefingTag, srcBrf.Tags.Length);
				for (int j = 0; j < srcBrf.Notes.Length; j++) destBrf.BriefingStringsNotes[j] = srcBrf.Notes[j];
				Array.Copy(srcBrf.ViewedByTeam, destBrf.Team, 10);

				if (report.Count > 0) displayExportReport(report);
			}
		}

		/// <summary>Checks if the specified event type is compatible with the current platform.</summary>
		bool isCompatiblePlatformEvent(AbstractEventType type)
		{
			switch (type)
			{
				case AbstractEventType.None:
				case AbstractEventType.Unknown2:
					 return false;

				case AbstractEventType.SkipMarker:
				case AbstractEventType.PageBreak:
				case AbstractEventType.TitleText:
				case AbstractEventType.CaptionText:
				case AbstractEventType.MoveMap:
				case AbstractEventType.ZoomMap:
				case AbstractEventType.ClearFgTags:
				case AbstractEventType.FgTag1:
				case AbstractEventType.FgTag2:
				case AbstractEventType.FgTag3:
				case AbstractEventType.FgTag4:
					return true;

				case AbstractEventType.PanelText3:
				case AbstractEventType.PanelText4:
					return isXwing;

				case AbstractEventType.FgTag5:
				case AbstractEventType.FgTag6:
				case AbstractEventType.FgTag7:
				case AbstractEventType.FgTag8:
					return !isXwing;

				case AbstractEventType.ClearTextTags:
				case AbstractEventType.TextTag1:
				case AbstractEventType.TextTag2:
				case AbstractEventType.TextTag3:
				case AbstractEventType.TextTag4:
					return true;

				case AbstractEventType.TextTag5:
				case AbstractEventType.TextTag6:
				case AbstractEventType.TextTag7:
				case AbstractEventType.TextTag8:
					return !isXwing;

				case AbstractEventType.XwaSetIcon:
				case AbstractEventType.XwaShipInfo:
				case AbstractEventType.XwaMoveIcon:
				case AbstractEventType.XwaRotateIcon:
				case AbstractEventType.XwaChangeRegion:
				case AbstractEventType.XwaInfoParagraph:
					return isXwa;

				case AbstractEventType.UnknownClear:
				case AbstractEventType.UnknownEffect:
					return isXwing;

				default:
					break;
			}

			return false;
		}
		
		/// <summary>Logs a string to list of message strings.</summary>
		/// <param name="message">Message string to add.</param>
		/// <param name="report">The list to receive the added message.</param>
		/// <param name="briefIndex">Specifies a briefing index that this error was encountered in, to distinguish between briefings or pages.</param>
		/// <param name="header">Tracks whether a message has been logged for this briefing index.</param>
		void logExportError(string message, List<string> report, int briefIndex, ref bool header)
		{
			if (report == null) return;

			if (header == false && briefIndex >= 0)
			{
				if (report.Count > 0) report.Add(Environment.NewLine);

				string type = isXwing ? "PAGE" : "BRIEFING";
				report.Add(type + " #" + (briefIndex + 1));
				header = true;
			}
			report.Add(message);
		}

		/// <summary>Converts a list of mapped briefing events into a list of output events, ready for saving.  Applies the necessary conversion checks and optionally builds a report.</summary>
		/// <remarks>Only for XWING.</remarks>
		/// <param name="source">List of abstract mapped events used by the frontend.</param>
		/// <param name="report">Optional list of strings to log conversion errors.</param>
		/// <param name="pageIndex">Page index to assist in distinguishing errors in multiple pages.</param>
		/// <returns>A list of events that are directly compatible with the working platform.</returns>
		List<Platform.Xwing.Briefing.Event> exportXwingEventList(List<AbstractEvent> source, List<string> report, int pageIndex)
		{
			List<Platform.Xwing.Briefing.Event> result = new List<Platform.Xwing.Briefing.Event>();

			bool header = false;

			foreach (var srcEvt in source)
			{
				var def = EventDef.GetEventDefByType(srcEvt.Event);
				if (def == null || def.Type == AbstractEventType.None)
				{
					logExportError("ERROR: Failed to save briefing.  Unexpected event type", report, pageIndex, ref header);
					break;
				}

				if (!isCompatiblePlatformEvent(srcEvt.Event))
				{
					logExportError("WARNING: Event \"" + def.Name + "\" not saved, not compatible with current platform.", report, pageIndex, ref header);
					continue;
				}

				Platform.Xwing.Briefing.Event destEvt = new Platform.Xwing.Briefing.Event((Platform.Xwing.Briefing.EventType)def.RawEventXW) { Time = srcEvt.Time };

				// All params are compatible except for TextTag.  Xwing does not have a color parameter.  It's the last one, and can be safely dropped.
				Array.Copy(srcEvt.Params, destEvt.Variables, destEvt.Variables.Length);
				result.Add(destEvt);
			}

			// Finish with end event.
			Platform.Xwing.Briefing.Event endEvt = new Platform.Xwing.Briefing.Event(Platform.Xwing.Briefing.EventType.EndBriefing);
			result.Add(endEvt);

			return result;
		}

		/// <summary>Converts a list of mapped briefing events into a list of output events, ready for saving.  Applies the necessary conversion checks and optionally builds a report.</summary>
		/// <remarks>Only for TIE, XvT, and XWA.</remarks>
		/// <param name="source">List of abstract mapped events used by the frontend.</param>
		/// <param name="report">Optional list of strings to log conversion errors.</param>
		/// <param name="briefIndex">Briefing index to assist in distinguishing errors in multiple briefings.  Use -1 for none (TIE).</param>
		/// <returns>A list of events that are directly compatible with the working platform.</returns>
		List<BaseBriefing.Event> exportEventList(List<AbstractEvent> source, List<string> report, int briefIndex)
		{
			List<BaseBriefing.Event> result = new List<BaseBriefing.Event>();

			bool header = false;

			foreach (var srcEvt in source)
			{
				var def = EventDef.GetEventDefByType(srcEvt.Event);
				if (def == null || def.Type == AbstractEventType.None)
				{
					logExportError("ERROR: Failed to save briefing.  Unexpected event type", report, briefIndex, ref header);
					break;
				}

				if (!isCompatiblePlatformEvent(srcEvt.Event))
				{
					logExportError("WARNING: Event \"" + def.Name + "\" not saved, not compatible with current platform.", report, briefIndex, ref header);
					continue;
				}

				BaseBriefing.Event destEvt = new BaseBriefing.Event
				{
					Type = (BaseBriefing.EventType)def.RawEvent,
					Time = srcEvt.Time
				};
				Array.Copy(srcEvt.Params, destEvt.Variables, destEvt.Variables.Length);
				result.Add(destEvt);
			}

			// Finish with end event.
			result.Add(new BaseBriefing.Event(BaseBriefing.EventType.EndBriefing));
			return result;
		}

		string buildReportString(List<string> report)
		{
			string msg = "";
			foreach (string s in report)
			{
				if (msg != "") msg += Environment.NewLine;
				msg += s;
			}
			return msg;
		}

		void displayExportReport(List<string> report) => MessageBox.Show(buildReportString(report), "Export Report");

		void displayHelpReport(List<string> report) => MessageBox.Show(buildReportString(report), "Help");

		public void Import(Platform.Xwing.FlightGroupCollection brfFg)
		{
			if (isFormClosed()) return;

			if (!_loading) prepareReimport();
			_flightgroups = importAllFlightgroup(brfFg, _craftAbbrev);
			refreshMap();
		}

		public void Import(int index, Platform.Xwing.FlightGroup fg)
		{
			if (isFormClosed() || index < 0 || index >= _flightgroups.Count) return;

			if (!_loading) prepareReimport();
			importFlightgroup(fg, _flightgroups[index], _craftAbbrev);
			refreshMap();
		}

		/// <summary>Imports all flightgroups from the mission flightgroup collection, generating a list of abstract flightgroups.</summary>
		List<AbstractFlightgroup> importAllFlightgroup(Platform.Xwing.FlightGroupCollection collection, string[] craftAbbrev)
		{
			List<AbstractFlightgroup> flightgroups = new List<AbstractFlightgroup>();
			foreach (var fg in collection)
			{
				AbstractFlightgroup afg = new AbstractFlightgroup();
				importFlightgroup(fg, afg, craftAbbrev);
				flightgroups.Add(afg);
			}
			return flightgroups;
		}

		/// <summary>Imports a single flightgroup from a mission, converting the relevant information into an abstract flightgroup.</summary>
		void importFlightgroup(Platform.Xwing.FlightGroup srcFg, AbstractFlightgroup afg, string[] craftAbbrev)
		{
			afg.CraftType = (srcFg.IsFlightGroup() ? srcFg.CraftType : srcFg.ObjectType);
			afg.CraftIff = srcFg.IFF;
			afg.Abbrev = getSafeString(craftAbbrev, afg.CraftType);
			afg.Name = srcFg.Name;
			afg.DisplayName = (afg.Abbrev + " " + afg.Name).Trim();

			// Start Point 1.  It doesn't matter whether the waypoint is enabled, in XWING they're always visible.
			afg.Waypoints[0] = srcFg.Waypoints[0];
			afg.Waypoints[0].Enabled = true;

			// Waypoints 1 to 3.
			for (int ws = 0; ws < 3; ws++)
			{
				afg.Waypoints[1 + ws] = srcFg.Waypoints[7 + ws];
				afg.Waypoints[1 + ws].Enabled = true;
			}
		}

		public void Import(Platform.Tie.FlightGroupCollection flightgroups)
		{
			if (isFormClosed()) return;

			if (!_loading) prepareReimport();
			_flightgroups = importAllFlightgroup(flightgroups, _craftAbbrev);
			refreshMap();
		}

		public void Import(int index, Platform.Tie.FlightGroup fg)
		{
			if (isFormClosed() || index < 0 || index >= _flightgroups.Count) return;

			if (!_loading) prepareReimport();
			importFlightgroup(fg, _flightgroups[index], _craftAbbrev);
			refreshMap();
		}

		/// <summary>Imports all flightgroups from the mission flightgroup collection, generating a list of abstract flightgroups.</summary>
		List<AbstractFlightgroup> importAllFlightgroup(Platform.Tie.FlightGroupCollection collection, string[] craftAbbrev)
		{
			List<AbstractFlightgroup> flightgroups = new List<AbstractFlightgroup>();
			foreach (Platform.Tie.FlightGroup fg in collection)
			{
				AbstractFlightgroup afg = new AbstractFlightgroup();
				importFlightgroup(fg, afg, craftAbbrev);
				flightgroups.Add(afg);
			}
			return flightgroups;
		}

		/// <summary>Imports a single flightgroup from a mission, converting the relevant information into an abstract flightgroup.</summary>
		void importFlightgroup(Platform.Tie.FlightGroup fg, AbstractFlightgroup afg, string[] craftAbbrev)
		{
			afg.CraftType = fg.CraftType;
			afg.CraftIff = fg.IFF;
			afg.Abbrev = getSafeString(craftAbbrev, afg.CraftType);
			afg.Name = fg.Name;
			afg.DisplayName = (afg.Abbrev + " " + afg.Name).Trim();
			afg.Waypoints[0] = fg.Waypoints[14];
		}

		public void Import(Platform.Xvt.FlightGroupCollection collection)
		{
			if (isFormClosed()) return;

			if (!_loading) prepareReimport();
			_flightgroups = importAllFlightgroup(collection, _craftAbbrev);
			resolvePlayerNumbers(_flightgroups);
			refreshMap();
		}

		public void Import(int index, Platform.Xvt.FlightGroup fg)
		{
			if (isFormClosed() || index < 0 || index >= _flightgroups.Count) return;

			if (!_loading) prepareReimport();
			importFlightgroup(fg, _flightgroups[index], _craftAbbrev);
			resolvePlayerNumbers(_flightgroups);
			refreshMap();
		}

		/// <summary>Imports all flightgroups from the mission flightgroup collection, generating a list of abstract flightgroups.</summary>
		List<AbstractFlightgroup> importAllFlightgroup(Platform.Xvt.FlightGroupCollection collection, string[] craftAbbrev)
		{
			List<AbstractFlightgroup> flightgroups = new List<AbstractFlightgroup>();
			foreach (Platform.Xvt.FlightGroup fg in collection)
			{
				AbstractFlightgroup afg = new AbstractFlightgroup();
				importFlightgroup(fg, afg, craftAbbrev);
				flightgroups.Add(afg);
			}
			return flightgroups;
		}

		/// <summary>Imports a single flightgroup from a mission, converting the relevant information into an abstract flightgroup.</summary>
		void importFlightgroup(Platform.Xvt.FlightGroup fg, AbstractFlightgroup afg, string[] craftAbbrev)
		{
			afg.CraftType = fg.CraftType;
			afg.CraftIff = fg.IFF;
			afg.PlayerNumber = fg.PlayerNumber;
			afg.Team = fg.Team;
			afg.Abbrev = getSafeString(craftAbbrev, afg.CraftType);
			afg.Name = fg.Name;
			afg.DisplayName = (afg.Abbrev + " " + afg.Name).Trim();

			for (int wp = 0; wp < 8; wp++) afg.Waypoints[wp] = fg.Waypoints[14 + wp];
		}
		#endregion Import and export (frontend)

		#region Main form and control utility
		bool isFormClosed() => IsDisposed;
		
		/// <summary>Checks if a particular tab is selected in the form.</summary>
		bool isCurrentTab(MainTabIndex index) => ((MainTabIndex)tabBriefing.SelectedIndex == index);

		/// <summary>Resizes the temp panel so that the bottom edge is just below the specified button control.</summary>
		void alignTempPanelBottom(Button b) => pnlTempCreate.Height = b.Bottom + (b.Margin.Bottom * 2);

		/// <summary>Converts an image assigned to a button to use the operating system colors instead of a static bitmap.</summary>
		/// <remarks>Because a pre-made bitmap that's easily distinguishable for both light and dark system themes is impractical.</remarks>
		void convertButtonImage(Button b)
		{
			Color newFg = Color.FromArgb(0xFF, 0, 0x80, 0xFF);
			Color newBg = Color.Transparent;  //System.Drawing.SystemColors.Control;

			int foreground = Color.Black.ToArgb();

			Bitmap bmp = new Bitmap(b.Image);
			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					// NOTE: This could be faster without going through these slow functions, but the bitmaps
					// are small enough that there doesn't seem to be any detectable time loss.

					// Any pixel that isn't transparent (alpha=0) gets the text color.
					int pix = bmp.GetPixel(x, y).ToArgb();
					bmp.SetPixel(x, y, (pix == foreground ? newFg : newBg));
				}
			}
			Bitmap old = (Bitmap)b.Image;
			b.Image = bmp;
			old?.Dispose();
		}

		/// <summary>Sets the override string that will appear in place of the title text.</summary>
		/// <remarks>This is used to convey certain kinds of information to the user in a manner that's less intrusive than a popup dialog box.</remarks>
		/// <param name="str">String to display.  To clear an existing override, specify an empty string.</param>
		/// <param name="seconds">Seconds to display the message, if the message should automatically expire.  Use zero for no duration.</param>
		void setTitleOverride(string str, int seconds)
		{
			_titleOverrideString = str;
			_titleOverrideTime = seconds * 1000;
			refreshCanvas(RefreshFlags.Title);
		}

		void refreshFormTitle()
		{
			string s = "YOGEME Briefing Editor - ";
			if (isXwing) s += "X-Wing";
			else if (isTie) s += "TIE";
			else if (isXvt) s += "XvT/BoP";
			else if (isXwa) s += "XWA";

			if (isXvt || isXwa)
			{
				// Append team names, up to three.  Any more will be truncated.
				s += "  [Briefing #" + (_currentBriefingIndex + 1) + " for: ";
				string tstr = "";
				int count = 0;
				for (int i = 0; i < lstTeams.Items.Count; i++)
				{
					if (!lstTeams.GetSelected(i)) continue;

					if (tstr != "") tstr += ", ";
					if (++count == 4) { tstr += "..."; break; }

					tstr += _teamNames[i];
				}

				if (tstr == "") tstr = "No Teams Assigned!";
				s += tstr + "] ";
			}
			if (_isDirty) s += "*";
			Text = s;
		}
		
		/// <summary>Function signature for data sent from the briefing form to the parent form.</summary>
		/// <param name="tag">Identifier tag indicating what was modified.  See <see cref="DataChangeTags"/> for a list of exchanged tags and their expected data format.</param>
		/// <param name="index">Index of an array element being modified, if applicable.</param>
		/// <param name="data">Data packet containing the new information, if applicable.  The handler is responsible for interpreting the data type.</param>
		public delegate void DataChangeCallback(string tag, int index, object data);

		public abstract class DataChangeTags
		{
			/// <summary>Generic change, parameters are unused</summary>
			public const string Generic = "";
			/// <summary>A flightgroup waypoint was changed.  Index: flightgroup, Data: unused</summary>
			public const string FgWaypoint = "fgwaypoint";
			/// <summary>A flightgroup's identifying information was changed.  Only for XWING.  Index: flightgroup, Data:Tuple(CraftType, Iff, Name)</summary>
			public const string FgEntry = "fgentry";
			/// <summary>A new flightgroup was added.  Only for XWING.  Index: flightgroup, Data:<see cref="Platform.Xwing.FlightGroup"/> object</summary>
			public const string FgNew = "fgnew";
			/// <summary>A flightgroup was deleted.  Only for XWING.  Index: flightgroup, Data: unused</summary>
			public const string FgDelete = "fgdelete";
		}

		/// <summary>Notifies the parent form that something was modified via the briefing form.  Includes an optional packet for transmitting or syncing data.</summary>
		/// <remarks>The function signature should match the delegate <see cref="DataChangeCallback"/>.</remarks>
		void notifyDataChange(string tag, int index, object data)
		{
			markDirty(false);
			_dataChangeCallback?.Invoke(tag, index, data);
		}

		/// <summary>Marks dirty and notifies the parent form that a flightgroup's briefing waypoint has changed.</summary>
		void notifyWaypointChange(int fgIndex)
		{
			markDirty(false);
			// Mouse dragging can produce rapid updates which can lag the parent form.  Queue up a single item.
			_pendingWaypointSync.Add(fgIndex);
		}

		/// <summary>Marks dirty and notifies the parent form that a flightgroup's identifying information has changed</summary>
		void notifyFlightgroupChange(int fgIndex)
		{
			markDirty(false);
			AbstractFlightgroup afg = _flightgroups[fgIndex];
			_dataChangeCallback?.Invoke(DataChangeTags.FgEntry, fgIndex, (afg.CraftType, afg.CraftIff, afg.Name) );
		}

		/// <summary>Marks dirty and notifies the parent form that a generic data change has occurred.</summary>
		void markDirty(bool invoke = true)
		{
			Common.MarkDirty(this, false);
			_isDirty = true;
			txtVerify.Text = "The briefing has changed, click Verify to refresh.";
			if (invoke) _dataChangeCallback?.Invoke(DataChangeTags.Generic, 0, null);
		}

		void safeSetNumeric(NumericUpDown num, int value)
		{
			decimal v = value;
			if (v < num.Minimum) v = num.Minimum;
			if (v > num.Maximum) v = num.Maximum;
			num.Value = v;
		}

		void safeSetNumeric(NumericUpDown num, decimal value)
		{
			if (value < num.Minimum) value = num.Minimum;
			if (value > num.Maximum) value = num.Maximum;
			num.Value = value;
		}

		void safeSetScrollbar(ScrollBar scroll, int value)
		{
			if (value < scroll.Minimum) value = scroll.Minimum;
			if (value > scroll.Maximum) value = scroll.Maximum;
			scroll.Value = value;
		}

		void safeSetCbo(ComboBox cbo, int index)
		{
			if (index < 0 || index >= cbo.Items.Count) index = -1;

			cbo.SelectedIndex = index;
		}

		void loadCbo(ComboBox cbo, int index)
		{
			bool temp = _loading;
			_loading = true;
			safeSetCbo(cbo, index);
			_loading = temp;
		}

		/// <summary>Retrieves a list of all radio buttons that are in the same group as the specified control.</summary>
		List<RadioButton> enumRadioButtons(RadioButton radio)
		{
			List<RadioButton> result = new List<RadioButton>();
			foreach (Control c in radio.Parent.Controls)
				if (c is RadioButton r) result.Add(r);

			return result;
		}

		/// <summary>Sets the checked state of a radio button when it belongs to a group of exactly two radios.</summary>
		/// <param name="radio">Any radio that belongs to the requested group.</param>
		/// <param name="state">The expected state of that particular radio button.</param>
		/// <exception cref="ArgumentException">There must be exactly two radios in the specified group.</exception>
		void setRadio(RadioButton radio, bool state)
		{
			List<RadioButton> list = null;
			if (radio.Tag != null && radio.Tag is List<RadioButton> cache) list = cache;

			if (list == null) list = enumRadioButtons(radio);
			if (list.Count != 2) throw new ArgumentException("setRadio count expects two radios");

			int index = (radio == list[0] ? 0 : 1);
			if (!state) index ^= 1;
			list[index].Checked = true;
			if (radio.Tag == null) radio.Tag = list;
		}

		/// <summary>A MouseEnter event over the PictureBox will steal focus if this condition is true.</summary>
		/// <remarks>Necessary to help trap keyboard events.</remarks>
		bool allowFocusSteal()
		{
			// This determines whether the window is in the foreground and has focus.
			// Taking focus always snaps the form into the foreground, which is annoying.
			if (ActiveForm != this) return false;

			// When using a control with keyboard focus, don't steal away from it.
			Type type = ActiveControl.GetType();
			return (type != typeof(TextBox) && type != typeof(NumericUpDown));
		}
		
		/// <summary>Isolates a scrollbar inside a panel, nesting it deeper from its parent container.</summary>
		void isolateScrollbar(ScrollBar sb)
		{
			// Scrollbars will automatically catch keyboard input events before they can be processed by the
			// main form's KeyDown handler, even if the scrollbars don't have focus.  This prevents certain
			// interactive hotkeys from working correctly, like arrow keys on the map or timeline.  By
			// nesting the scrollbar inside a panel, it allows the KeyDown event to work correctly.
			Control parent = sb.Parent;

			Panel container = new Panel
			{
				Location = sb.Location,
				Size = sb.Size
			};

			parent.Controls.Remove(sb);
			container.Controls.Add(sb);
			parent.Controls.Add(container);
		}

		/// <summary>Retrieves the parent control of an isolated scrollbar, or the scrollbar itself if not isolated.</summary>
		Control getIsolatedScrollbar(ScrollBar sb)
		{
			// If a scrollbar is isolated within a panel, its location is relative to the panel, not
			// the tab control.  So if we need to access its proper location, we need the parent.
			Control parent = sb.Parent;
			if (parent.Controls.Count == 1) return parent;

			return sb;
		}

		/// <summary>Sets the location and size of a scrollbar that has been isolated, which includes the scrollbar itself and its parent panel.</summary>
		void setIsolatedScrollbarSize(ScrollBar sb, int x, int y, int width, int height)
		{
			Control parent = getIsolatedScrollbar(sb);
			parent.Location = new Point(x, y);
			if (width != 0) parent.Width = width;
			if (height != 0) parent.Height = height;

			if (parent != sb)
			{
				sb.Location = new Point(0, 0);
				if (width != 0) sb.Width = width;
				if (height != 0) sb.Height = height;
			}
		}

		bool isVisibleChecked(CheckBox c) => (c.Visible && c.Enabled && c.Checked);
		#endregion Main form and control utility

		#region Resources
		/// <summary>Tries to load an LFD file.  If it doesn't exist or fails to load for any reason, returns null.</summary>
		LfdFile loadLfdFile(string filename)
		{
			if (!File.Exists(filename)) return null;

			LfdFile lfd = null;
			try { lfd = new LfdFile(filename); }
			catch { }

			return lfd;
		}

		/// <summary>Tries to open the LFD file and load any fonts that aren't yet loaded.</summary>
		/// <returns>True when all fonts are loaded.  False if even one font remains unloaded.</returns>
		bool loadXwingFontsFromLfdArchive(string fontFile, AssetSourceType sourceType)
		{
			var lfd = loadLfdFile(fontFile);
			if (lfd != null)
			{
				_font.LoadFromLfd(lfd, "font8", true, sourceType);
				_tagFont.LoadFromLfd(lfd, "font6", false, sourceType);
				_fontAltCaption.LoadFromLfd(lfd, "font18", true, sourceType);
				_fontAltTag.LoadFromLfd(lfd, "font8", false, sourceType);  // Same as font, but no shadow
			}
			return (_font.Loaded && _tagFont.Loaded && _fontAltCaption.Loaded && _fontAltTag.Loaded);
		}

		/// <summary>Retrieves a list of relative subdirectories where a RESOURCE folder may be present.</summary>
		/// <remarks>XWING asset locations are different for every version.</remarks>
		string[] getXwingResourcePaths() => new string[] { "", "X-Wing Data", "XWINGCD", "XWINGCD\\X-WING DATA" };

		/// <summary>Attempts to detect and load XWING fonts from any available resources or alternative sources.</summary>
		void detectXwingFonts(string platformPath)
		{
			// Generate a list of potential paths, in order of priority.
			// The asset type helps debug or report exactly where the resource was loaded from.
			string appFolder = Path.GetDirectoryName(Application.ExecutablePath);
			Dictionary<string, AssetSourceType> paths = new Dictionary<string, AssetSourceType>
			{
				// Priority given to any archive placed directly into the application folder.
				{ Path.Combine(appFolder, "XWING.LFD"), AssetSourceType.Application }
			};

			// Next prioritize the load path.  It could be either the mission path, or the installation path.
			string xwingPath = getPlatformInstallPath(Settings.Platform.XWING);
			AssetSourceType sourceType = AssetSourceType.Mission;
			if (platformPath.StartsWith(xwingPath)) sourceType = AssetSourceType.Installation;

			foreach (string d in getXwingResourcePaths()) paths.Add(Path.Combine(platformPath, d, "RESOURCE", "XWING.LFD"), sourceType);

			// If the path was different from the assigned install path, check the installation paths too.
			if (sourceType == AssetSourceType.Mission)
				foreach (string d in getXwingResourcePaths()) paths.Add(Path.Combine(xwingPath, d, "RESOURCE", "XWING.LFD"), AssetSourceType.Installation);

			// Last resort if the above paths all fail.  Try TIE Fighter since the fonts are compatible.
			// Same as above, prioritize application folder, then installation path.
			paths.Add(Path.Combine(appFolder, "EMPIRE.LFD"), AssetSourceType.AlternateApplication);
			string tiePath = getPlatformInstallPath(Settings.Platform.TIE);
			if (tiePath != "") paths.Add(Path.Combine(tiePath, "RESOURCE", "EMPIRE.LFD"), AssetSourceType.AlternateInstallation);

			// Finished compiling the list of all possible resource paths.
			// Now check every path until all required fonts have been loaded.
			foreach (var pair in paths) if (loadXwingFontsFromLfdArchive(pair.Key, pair.Value)) break;
		}

		/// <summary>Tries to open the LFD file and load any fonts that aren't yet loaded.</summary>
		/// <returns>True when all fonts are loaded.  False if even one font remains unloaded.</returns>
		bool loadTieFontsFromLfdArchive(string fontFile, AssetSourceType sourceType)
		{
			var lfd = loadLfdFile(fontFile);
			if (lfd != null)
			{
				_font.LoadFromLfd(lfd, "font8", true, sourceType);
				_tagFont.LoadFromLfd(lfd, "font6", false, sourceType);
			}
			return (_font.Loaded && _tagFont.Loaded);
		}

		/// <summary>Attempts to detect and load TIE fonts from any available resources or alternative sources.</summary>
		void detectTieFonts(string platformPath)
		{
			// Generate a list of potential paths, in order of priority.
			// The asset type helps debug or report exactly where the resource was loaded from.
			string appFolder = Path.GetDirectoryName(Application.ExecutablePath);
			Dictionary<string, AssetSourceType> paths = new Dictionary<string, AssetSourceType>
			{
				// Priority given to any archive placed directly into the application folder.
				{ Path.Combine(appFolder, "EMPIRE.LFD"), AssetSourceType.Application }
			};

			// Next prioritize the load path.  It could be either the mission path, or the installation path.
			string tiePath = getPlatformInstallPath(Settings.Platform.TIE);
			AssetSourceType sourceType = AssetSourceType.Mission;
			if (platformPath.StartsWith(tiePath)) sourceType = AssetSourceType.Installation;

			paths.Add(Path.Combine(platformPath, "RESOURCE", "EMPIRE.LFD"), AssetSourceType.Mission);

			// If the path was different from the assigned install path, check the installation paths too.
			if (sourceType == AssetSourceType.Mission) paths.Add(Path.Combine(tiePath, "RESOURCE", "EMPIRE.LFD"), AssetSourceType.Installation);

			// Last resort if the above paths all fail.  Try XWING since the fonts are compatible.
			// Same as above, prioritize application folder, then installation path.
			paths.Add(Path.Combine(appFolder, "XWING.LFD"), AssetSourceType.AlternateApplication);

			string xwingPath = getPlatformInstallPath(Settings.Platform.XWING);
			if (xwingPath != "")
				foreach (string d in getXwingResourcePaths()) paths.Add(Path.Combine(xwingPath, d, "RESOURCE", "XWING.LFD"), AssetSourceType.AlternateInstallation);

			// Finished compiling the list of all possible resource paths.
			// Now check every path until all required fonts have been loaded.
			foreach (var pair in paths) if (loadTieFontsFromLfdArchive(pair.Key, pair.Value)) break;
		}

		/// <summary>Loads all fonts for the assigned platform.  Prioritizes assets from the specified installation.  If not found, defaults will be generated.</summary>
		/// <remarks>Default fonts allow basic editor functionality, but the map display will not be authentic.</remarks>
		/// <param name="path">Installation path to load assets from.</param>
		void loadPlatformFonts(string path)
		{
			// First attempt to load fonts directly from the platform installation.
			// TODO: [JB] If the game fonts are missing, might need to test on Linux or alternative OS if
			// the default generated fonts are available, and what happens if they're missing.

			if (isXwing)
			{
				// Make sure the high def fonts aren't null when we try to load them.
				_fontAltCaption = new BriefingFont(BitmapCacheType.FontCaptionHigh);
				_fontAltTag = new BriefingFont(BitmapCacheType.FontTagHigh);
				detectXwingFonts(path);

				if (_font.Loaded) _font.VerticalSpacing = 2;

				// With a custom font installed, it seems there's a specific vertical height that needs to be used.
				if (_fontAltCaption.Loaded) _fontAltCaption.VerticalSpacing = 20 - _fontAltCaption.Height;
			}
			else if (isTie)
			{
				detectTieFonts(path);
				if (_font.Loaded) _font.VerticalSpacing = 2;
			}
			else if (isXvt || isXwa)
			{
				// The font for the title and team name are larger, so load an extra font.
				_fontAltCaption = new BriefingFont(BitmapCacheType.FontCaptionHigh);

				// The detected location of the mission may be different than the actual install path.  Check both.
				// They may also be the same.
				string installPath = getPlatformInstallPath(_platform);
				string[] installations = new string[2] { path, installPath };

				foreach (string curPath in installations)
				{
					if (curPath == "") continue;

					AssetSourceType sourceType = AssetSourceType.Mission;
					if (installPath != "" && curPath.StartsWith(installPath)) sourceType = AssetSourceType.Installation;

					string fontFile = Path.Combine(curPath, "TIMES10.ABP");
					if (File.Exists(fontFile))
					{
						_font.LoadFromAbpFile(fontFile, sourceType);
						if (_font.Loaded) _font.VerticalSpacing = (isXvt ? 4 : 2);
					}

					fontFile = Path.Combine(curPath, "TIMES12.ABP");
					if (File.Exists(fontFile)) _fontAltCaption.LoadFromAbpFile(fontFile, sourceType);
				}
			}

			// Determine if any fonts failed to load.  If so, generate default fonts to use in their
			// place.  These are close approximates to the actual fonts in terms of size, but
			// readability and spacing may not be ideal.
			if (!_font.Loaded)
			{
				if (isXwing || isTie)
				{
					_font.GenerateDefaultFont(7.8f, "Arial", FontStyle.Bold, true);
					_font.Height = 7;
					_font.VerticalOffset = -3;
				}
				else if (isXvt)
				{
					_font.GenerateDefaultFont(7.6f, "Comic Sans MS", FontStyle.Regular, false);
					// It's almost perfect, but some of the glyph spacing is off.
					_font.GetChar(' ').Width -= 2;
					_font.Height = 13;
				}
				else if (isXwa)
				{
					_font.GenerateDefaultFont(9, "Arial", FontStyle.Bold, false);
					// Offset is aligned for tag purposes.
					_font.Height = 13;
					_font.VerticalOffset = -1;
				}
			}

			if (!_tagFont.Loaded)
			{
				if (isXwing || isTie)
				{
					// Terminal seems to have better legibility for how small it is.
					// Surprisingly this is the right height, but the width is off.
					_tagFont.GenerateDefaultFont(5.4f, "Terminal", FontStyle.Regular, false);
					_tagFont.Height = 6;
					_tagFont.VerticalOffset = -2;
				}
			}

			if (_fontAltCaption != null && !_fontAltCaption.Loaded)
			{
				if (isXwing)
				{
					// If we go much larger, the yellow hint page disclaimer text won't fit.
					_fontAltCaption.GenerateDefaultFont(15.6f, "Times New Roman", FontStyle.Regular, true);
					_fontAltCaption.Height = 17;
					_fontAltCaption.VerticalOffset = -5;
				}
				else if (isXvt) _fontAltCaption.GenerateDefaultFont(9.2f, "Times New Roman", FontStyle.Regular, false);
				else if (isXwa) _fontAltCaption.GenerateDefaultFont(10.8f, "Arial", FontStyle.Bold, false);
			}

			if (_fontAltTag != null && !_fontAltTag.Loaded)
			{
				if (isXwing)
				{
					_fontAltTag.GenerateDefaultFont(10, "Arial", FontStyle.Regular, false);
					_fontAltTag.Height = 10;
					_fontAltTag.VerticalOffset = -3;
				}
			}

			// All fonts have loaded, either directly from the installation, or from generated placeholders.
			// XvT and XWA use the same font for captions and tags.
			if (isXvt || isXwa) _tagFont = _font;

			// Animated text tags use a special encoding rather than bitmaps to quickly render text
			// at arbitrary colors and brightness.
			_tagFont.GenerateTagEncoding();
			_fontAltTag?.GenerateTagEncoding();
		}

		/// <summary>Loads craft map icons for the assigned platform.  Prioritizes assets from the specified installation.  If not found, defaults will be used.</summary>
		/// <remarks>Default icons are shipped with the editor.  XWING icons are approximated from TIE, and color tinted palettes will not be accurate.</remarks>
		void loadPlatformIcons(string path, bool isBop)
		{
			// The detected location of the mission may be different than the actual install path.  Check both.
			// They may also be the same.
			string installPath = getPlatformInstallPath(_platform);
			string[] installations = new string[2] { path, installPath };

			foreach (string curPath in installations)
			{
				if (_craftIconImages.Count != 0) break;   // If something loaded, we're done
				if (curPath == "") continue;

				AssetSourceType sourceType = AssetSourceType.Mission;
				if (installPath != "" && curPath.StartsWith(installPath)) sourceType = AssetSourceType.Installation;

				if (isXwing)
				{
					foreach (string d in getXwingResourcePaths())
					{
						// XWING.LFD contains standard.PLTT
						// Need to load BWING.LFD and not BRIEF.LFD, because otherwise we won't have the updated icon set with the B-Wing.
						string paletteFile = Path.Combine(curPath, d, "RESOURCE", "XWING.LFD");
						string briefFile = Path.Combine(curPath, d, "RESOURCE", "BWING.LFD");
						if (File.Exists(paletteFile) && File.Exists(briefFile))
						{
							loadXwingBitmaps(paletteFile, briefFile, sourceType);
							break;
						}
					}
				}
				else if (isTie)
				{
					string resourceFile = Path.Combine(curPath, "RESOURCE", "PLAYER.LFD");
					if (File.Exists(resourceFile)) loadTieBitmaps(resourceFile, sourceType);
				}
				else if (isXvt) loadXvtBitmaps(curPath, isBop, sourceType);
				else if (isXwa)
				{
					string bitmapFile = Path.Combine(curPath, "FRONTRES\\MAPICONS\\LICON.BMP");
					string shiplistfile = Path.Combine(curPath, "SHIPLIST.TXT");
					if (File.Exists(bitmapFile) && File.Exists(shiplistfile)) loadXwaBitmaps(bitmapFile, shiplistfile, sourceType);
					loadXwaExtraBitmaps(Path.Combine(curPath, "FRONTRES\\MAPICONS\\"));
				}
			}

			// If the platform failed to load, use the default icons.
			if (_craftIconImages.Count == 0)
			{
				if (isXwing)
				{
					importIcons(Path.Combine(Application.StartupPath, "images", "TIE_BRF.bmp"), 34, true, 2.0f);
					mapTieIconToXwing();
				}
				else if (isTie) importIcons(Path.Combine(Application.StartupPath, "images", "TIE_BRF.bmp"), 34, true, 2.0f);
				else if (isXvt) importIcons(Path.Combine(Application.StartupPath, "images", "XvT_BRF.bmp"), 22, false, 1.5f);
				else if (isXwa) importIcons(Path.Combine(Application.StartupPath, "images", "XWA_BRF.bmp"), 56, false, 0.0f);

				// Should never happen, but could if the files are missing.  Create a placeholder just so it doesn't crash.
				if (_craftIconImages.Count == 0)
				{
					CraftIconImage empty = new CraftIconImage { Icon = new Bitmap(1, 1, PixelFormat.Format24bppRgb) };
					_craftIconImages.Add(empty);
				}
			}

			if (isTie) createFlatIcons();
		}

		/// <summary>Loads an LFD anim archive and returns the frames as a list of bitmaps.</summary>
		List<Bitmap> loadAnimBitmaps(Anim anim)
		{
			List<Bitmap> frames = new List<Bitmap>();
			for (int i = 0; i < anim.NumberOfFrames; i++) frames.Add(anim.Frames[i].Image);
			return frames;
		}

		/// <summary>Loads a list of 24-bit bitmaps directly into the cache using the specified color key.</summary>
		/// <remarks></remarks>
		/// <param name="frames">List of bitmaps to load.</param>
		/// <param name="color">Color key to use when inserting into the cache.  Should correspond to an IFF color.</param>
		/// <param name="offset">Index offset to assigned craft type.  XWA craft are zero-based, other platforms one-based.</param>
		void loadBitmapsIntoCache(List<Bitmap> frames, Color color, int offset)
		{
			for (int i = 0; i < frames.Count; i++)
			{
				if (frames[i] == null) continue;

				long key = getBitmapCacheKey(BitmapCacheType.CraftIcon, i + offset, color, 0);
				if (!_bitmapCache.ContainsKey(key)) _bitmapCache.Add(key, frames[i]);
			}
		}

		/// <summary>Loads a list of 8-bit bitmaps directly into the cache using the specified palette and color key.</summary>
		/// <param name="frames">List of bitmaps to load.</param>
		/// <param name="pal">Palette to use when converting to 24-bit.</param>
		/// <param name="color">Color key to use when inserting into the cache.  Should correspond to an IFF color.</param>
		void loadBitmapsIntoCache(List<Bitmap> frames, Pltt pal, Color color)
		{
			for (int i = 0; i < frames.Count; i++)
			{
				// Convert original 8bit indexed palette to the new format.
				Bitmap src = frames[i];
				Bitmap dest = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);

				Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);

				BitmapData srcData = src.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
				int srcStride = Math.Abs(srcData.Stride);
				int srcSize = srcStride * srcData.Height;
				byte[] srcSurf = new byte[srcSize];
				Marshal.Copy(srcData.Scan0, srcSurf, 0, srcSize);

				BitmapData destData = dest.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
				int destStride = Math.Abs(destData.Stride);
				int destSize = destStride * destData.Height;
				byte[] destSurf = new byte[destSize];

				for (int y = 0; y < destData.Height; y++)
				{
					int srcOffset = y * srcStride;
					int destOffset = y * destStride;
					for (int x = 0; x < destData.Width; x++)
					{
						byte palIndex = srcSurf[srcOffset++];
						if (palIndex > 0)
						{
							Color palColor = pal.Entries[palIndex];
							destSurf[destOffset] = palColor.B;
							destSurf[destOffset+1] = palColor.G;
							destSurf[destOffset+2] = palColor.R;
						}
						destOffset += 3;
					}
				}

				// Copy the transformed surface back into the bitmap and finalize it.
				Marshal.Copy(destSurf, 0, destData.Scan0, destSize);
				dest.UnlockBits(destData);
				src.UnlockBits(srcData);
				dest.MakeTransparent(Color.Black);

				// Insert directly into cache to preserve the original colors.
				long key = getBitmapCacheKey(BitmapCacheType.CraftIcon, i + 1, color, 0);
				if (!_bitmapCache.ContainsKey(key)) _bitmapCache.Add(key, dest);
			}
		}

		/// <summary>Finishes loading a series of icon bitmaps by converting them from 8-bit palettized color to grayscale.</summary>
		/// <param name="frames">List of 8-bit palettized bitmaps to convert and finalize.</param>
		/// <param name="pal">Palette to use when converting to RGB.</param>
		/// <param name="grayscaleCount">If positive, indicates only a certain number of icons in the set should be converted to grayscale.  For XWING only.</param>
		void loadCraftIconsFromBitmaps(List<Bitmap> frames, Pltt pal, int grayscaleCount)
		{
			// XWING and TIE briefing sets include a duplicate set of small icons that follow the standard size icons.
			int setCount = frames.Count / 2;

			_craftIconImages.Clear();

			// Bulding the icon sizes at the end will replace this with an empty image.
			CraftIconImage nulldat = new CraftIconImage { Icon = null };
			_craftIconImages.Add(nulldat);

			for (int i = 0; i < frames.Count; i++)
			{
				int iconIndex = i % setCount;
				int frameWidth = frames[i].Width;
				int frameHeight = frames[i].Height;

				Rectangle lockRect = new Rectangle(0, 0, frameWidth, frameHeight);

				Bitmap src = frames[i];
				BitmapData srcData = src.LockBits(lockRect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
				int srcSize = Math.Abs(srcData.Stride) * srcData.Height;
				byte[] srcSurf = new byte[srcSize];
				Marshal.Copy(srcData.Scan0, srcSurf, 0, srcSize);

				Bitmap dest = new Bitmap(frameWidth, frameHeight, PixelFormat.Format24bppRgb);
				BitmapData destData = dest.LockBits(lockRect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
				int destSize = Math.Abs(destData.Stride) * destData.Height;
				byte[] destSurf = new byte[destSize];

				bool grayscale = (grayscaleCount <= 0 || iconIndex < grayscaleCount);

				for (int y = 0; y < frameHeight; y++)
				{
					int destOffset = (y * destData.Stride);
					for (int x = 0; x < frameWidth; x++)
					{
						byte palIndex = srcSurf[(y * srcData.Stride) + x];
						if (palIndex > 0)
						{
							Color color = pal.Entries[palIndex];
							
							if (grayscale)
							{
								byte highest = 0;
								if (color.R > highest) highest = color.R;
								if (color.G > highest) highest = color.G;
								if (color.B > highest) highest = color.B;
								destSurf[destOffset] = highest;
								destSurf[destOffset+1] = highest;
								destSurf[destOffset+2] = highest;
							}
							else
							{
								destSurf[destOffset] = color.B;
								destSurf[destOffset+1] = color.G;
								destSurf[destOffset+2] = color.R;
							}
						}
						destOffset += 3;
					}
				}

				Marshal.Copy(destSurf, 0, destData.Scan0, destSize);

				src.UnlockBits(srcData);
				dest.UnlockBits(destData);

				dest.MakeTransparent(Color.Black);

				CraftIconImage data = new CraftIconImage
				{
					Icon = dest,
					Rect = lockRect,
					OriginalRect = lockRect,
					Width = frameWidth,
					Height = frameHeight,
					SquareSize = Math.Max(frameWidth, frameHeight),
					DrawOffsetX = -frameWidth / 2,
					DrawOffsetY = -frameHeight / 2
				};
				_craftIconImages.Add(data);
			}

			extractCraftIconList(null, false);
			finalizeCraftIconList();

			// If we got here without throwing an exception, then everything loaded successfully.
			_smallIconOffset = setCount;
		}

		bool loadXwingBitmaps(string paletteFile, string briefFile, AssetSourceType sourceType)
		{
			try
			{
				// There are three palettes in XWING, "grn", "red", and "blu".
				// Green has a full set including planets and misc objects, red and blue only have craft.
				// Everything that's not a craft uses the explicit colors, and should not be converted to
				// grayscale and tinted.

				// Resource names, in order of IFF from 1 to 3.  Not using default IFF zero.
				string[] resStr = new string[] { "iconsgrn", "iconsred", "iconsblu" };

				LfdFile palArchive = new LfdFile(paletteFile);
				Pltt pal = (Pltt)palArchive.Resources["PLTTstandard"];
				/*foreach (Resource res in palArchive.Resources)
				{
					if (res.Type == Resource.ResourceType.Pltt && stringEqual(res.Name, "standard"))
					{
						pal = res as Pltt;
						break;
					}
				}*/

				LfdFile iconArchive = new LfdFile(briefFile);
				Anim[] anim = new Anim[3];
				for (int i = 0; i < resStr.Length; i++) anim[i] = (Anim)iconArchive.Resources[$"ANIM{resStr[i]}"];
				/*foreach (Resource res in iconArchive.Resources)
				{
					if (res.Type == Resource.ResourceType.Anim)
					{
						for (int i = 0; i < resStr.Length; i++)
						{
							if (stringEqual(res.Name, resStr[i])) anim[i] = res as Anim;
						}
					}
				}*/

				if (pal != null && anim[0] != null)
				{
					bool result = false;
					for (int i = 0; i < anim.Length; i++)
					{
						List<Bitmap> frames = loadAnimBitmaps(anim[i]);
						loadBitmapsIntoCache(frames, pal, getIconColor(i + 1));
						if (i != 0) continue;
						
						loadCraftIconsFromBitmaps(frames, pal, 25);
						_iconAssetSource = sourceType;
						result = true;
					}

					return result;
				}
			}
			catch { }

			return false;
		}

		bool loadTieBitmaps(string resourceFile, AssetSourceType sourceType)
		{
			try
			{
				// Resource names, in order of IFF from 0 to 3.
				string[] resStr = new string[] { "iconsgrn", "iconsred", "iconsblu", "iconspur" };

				LfdFile lfd = new LfdFile(resourceFile);
				Pltt pal = (Pltt)lfd.Resources["PLTTrange"];
				Anim[] anim = new Anim[4];
				for (int i = 0; i < resStr.Length; i++) anim[i] = (Anim)lfd.Resources[$"ANIM{resStr[i]}"];
				/*foreach (Resource res in lfd.Resources)
				{
					if (res.Type == Resource.ResourceType.Pltt && stringEqual(res.Name, "range")) pal = res as Pltt;
					else if (res.Type == Resource.ResourceType.Anim)
						for (int i = 0; i < resStr.Length; i++) if (stringEqual(res.Name, resStr[i])) anim[i] = res as Anim;
				}*/

				if (pal != null && anim[0] != null)
				{
					bool result = false;
					for (int i = 0; i < anim.Length; i++)
					{
						List<Bitmap> frames = loadAnimBitmaps(anim[i]);
						loadBitmapsIntoCache(frames, pal, getIconColor(i));
						if (i != 0) continue;
						
						loadCraftIconsFromBitmaps(frames, pal, -1);
						_iconAssetSource = sourceType;
						result = true;
					}

					return result;
				}
			}
			catch { }

			return false;
		}

		bool loadXvtBitmaps(string installPath, bool isBop, AssetSourceType sourceType)
		{
			try
			{
				int[] mapTable = new int[106];
				Rectangle[] rectTable = new Rectangle[70];
				int rectCount = isBop ? 70 : 64;

				string exeFile = Path.Combine(installPath, "z_xvt__.exe");
				using (FileStream fs = new FileStream(exeFile, FileMode.Open, FileAccess.Read))
				{
					using (BinaryReader br = new BinaryReader(fs))
					{
						// File verification, right before the data there's some strings.
						// Check the last one.
						fs.Position = isBop ? 0x12A2FC : 0x1323F4;
						if (br.ReadInt64() != 0x647261656C63)  // Hex values for the string "cleard"
							return false;

						// Read mapping array
						fs.Position = isBop ? 0x12A308 : 0x132400;
						for (int i = 0; i < 106; i++) mapTable[i] = br.ReadInt32();

						// Read rectangle array
						fs.Position = isBop ? 0x12A4B0 : 0x1325A8;
						for (int i = 0; i < rectCount; i++)
						{
							int left = br.ReadInt32();
							int top = br.ReadInt32();
							int right = br.ReadInt32();
							int bottom = br.ReadInt32();

							rectTable[i].X = left;
							rectTable[i].Y = top;
							rectTable[i].Width = (right - left) + 1;
							rectTable[i].Height = (bottom - top) + 1;
						}
					}
				}

				// The first one is the grayscale, meant for tinted.  In game it's used for FG tag animation icons.
				// NOTE: yel_4 doesn't actually exist in BOP.
				string[] resStr = new string[] { "icons.bmp", "grn_4.bmp", "red_4.bmp", "blu_4.bmp", "yel_4.bmp", "prp_4.bmp" };
				
				// Map the resources to IFF.  Iff 4 is red, so skip over it.
				int[] resIff = new int[] { -1, 0, 1, 2, 3, 5 };

				// Will add a null entry before finalizing.
				_craftIconImages.Clear();
				
				for (int i = 0; i < resStr.Length; i++)
				{
					List<Bitmap> bitmaps = new List<Bitmap>();

					string filename = Path.Combine(installPath, "frontres", resStr[i]);

					// This is really just to fall back to vanilla for the missing yellow icon set.
					// For the purposes of having an "empty" bitmap containing some garbage pixels.
					// The garbage isn't correct to the game, so something else is going on.
					if (!File.Exists(filename) && isBop)
					{
						int pos = installPath.IndexOf("BalanceOfPower", StringComparison.OrdinalIgnoreCase);
						if (pos >= 0) filename = Path.Combine(installPath.Substring(0, pos), "frontres", resStr[i]);
					}

					if (File.Exists(filename))
					{
						using (Image main = Image.FromFile(filename))
						{
							using (Graphics g = Graphics.FromImage(main))
							{
								// Skip the null craft entry to start with the X-Wing.
								for (int c = 1; c < mapTable.Length; c++)
								{
									Rectangle r = rectTable[mapTable[c]];
									Bitmap temp = new Bitmap(r.Width, r.Height, PixelFormat.Format24bppRgb);
									using (Graphics b = Graphics.FromImage(temp))
									{
										// Noticed some artifacts on the SSD, particularly IFF green, highlighted by FG tag.
										// The original bitmap was fine.  Needed the correct interpolation mode to fix.
										b.InterpolationMode = InterpolationMode.NearestNeighbor;
										b.DrawImage(main, 0, 0, r, GraphicsUnit.Pixel);
									}
									temp.MakeTransparent(Color.Black);

									// If this is the grayscale set, use these metrics to build the craft info.
									if (i == 0)
									{
										// Just assign the bitmap, the finalize function will calculate the rest from the image dimensions.
										CraftIconImage data = new CraftIconImage { Icon = temp };
										_craftIconImages.Add(data);
									}
									else bitmaps.Add(temp);
								}
							}
						}

						if (i > 0) loadBitmapsIntoCache(bitmaps, getIconColor(resIff[i]), 1);
					}
				}

				if (_craftIconImages.Count > 0)
				{
					// Add null entry before finalizing.
					_craftIconImages.Insert(0, new CraftIconImage());
					finalizeCraftIconList();
					_iconAssetSource = sourceType;
					return true;
				}
			}
			catch { }

			return false;
		}

		bool loadXwaBitmaps(string bitmapFile, string shiplistfile, AssetSourceType sourceType)
		{
			try
			{
				_craftIconImages.Clear();
				using (StreamReader sr = File.OpenText(shiplistfile))
				{
					while (!sr.EndOfStream)
					{
						CraftIconImage data = new CraftIconImage();

						string s = sr.ReadLine();
						if (s.Length > 0)
						{
							int pos = s.LastIndexOf('!');
							if (pos >= 0) s = s.Substring(pos + 1);
						}
						s = s.Replace("\t", "");

						// 0=Name, 1=Class, 2=Flyable, 3=Known, 4=Skirmish, 5..8=unused icon rect?, 9..12=icon rect
						string[] tokens = s.Split(',');
						if (tokens.Length >= 12)
						{
							data.Hidden = (tokens[0].StartsWith("*") || stringEqual(tokens[1], "Planet/asteroid"));

							string shipClass = tokens[1];
							int.TryParse(tokens[9].Trim(), out int x1);
							int.TryParse(tokens[10].Trim(), out int y1);
							int.TryParse(tokens[11].Trim(), out int x2);
							int.TryParse(tokens[12].Trim(), out int y2);
							int width = x2 - x1;
							int height = y2 - y1;
							data.OriginalRect = new Rectangle(x1, y1, width, height);
						}
						data.ShipList = true;
						_craftIconImages.Add(data);
					}
				}

				Bitmap bitmap = (Bitmap)Image.FromFile(bitmapFile);
				extractCraftIconList(bitmap, false);
				finalizeCraftIconList();
				_iconAssetSource = sourceType;

				return true;
			}
			catch { }

			return false;
		}

		/// <summary>Loads the pre-tinted icons for each IFF, adding them directly into the cache.</summary>
		/// <remarks>Builds off the icon locations already loaded from the shipinfo file.</remarks>
		void loadXwaExtraBitmaps(string bitmapPath)
		{
			try
			{
				string[] files = new string[] { "LGRN_4", "LRED_4", "LBLU_4", "LYEL_4", "LPRP_4" };
				int[] iff = new int[] { 0, 1, 2, 3, 5 };  // Skip IFF red
				for (int i = 0; i < files.Length; i++)
				{
					string path = Path.Combine(bitmapPath, files[i] + ".CBM");
					if (!File.Exists(path)) continue;

					Bitmap master = loadXwaCbm(path);
					if (master == null ) continue;

					List<Bitmap> icons = new List<Bitmap>();

					for (int j = 0; j < _craftIconImages.Count; j++)
					{
						Rectangle src = _craftIconImages[j].OriginalRect;
						if (src.Width == 0 || src.Height == 0)
						{
							icons.Add(null);
							continue;
						}

						// Seems to clip some icon bitmaps without this.
						src.Width++;
						src.Height++;

						Bitmap temp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
						using (Graphics g = Graphics.FromImage(temp))
						{
							g.InterpolationMode = InterpolationMode.NearestNeighbor;
							g.DrawImage(master, 0, 0, src, GraphicsUnit.Pixel);
						}

						// Check if the bitmap is empty.  If so, don't assign a bitmap.  This provides a
						// fallback to LICON.BMP, but this behavior is not accurate.
						var tempData = temp.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
						int[] pixData = new int[(tempData.Height * tempData.Stride) / 4];
						Marshal.Copy(tempData.Scan0, pixData, 0, pixData.Length);
						long colors = 0;
						for (int y = 0; y < tempData.Height; y++)
						{
							int baseOffset = (y * tempData.Stride) / 4;
							for (int x = 0; x < tempData.Width; x++) colors += (pixData[baseOffset++] & 0x00FFFFFF);
						}
						temp.UnlockBits(tempData);

						if (colors == 0)
						{
							temp.Dispose();
							temp = null;
						}

						icons.Add(temp);
					}

					loadBitmapsIntoCache(icons, _iconIffColors[iff[i]], 0);
					master.Dispose();
				}
			}
			catch { }
		}

		Bitmap loadXwaCbm(string filename)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader br = new BinaryReader(fs))
				{
					// This isn't the full CBM spec.  Just reads enough to load the icon files.
					// Only thing we need from the main header is the image count.
					int count = br.ReadInt32();
					if (count != 1 ) return null;

					fs.Position = 0x24;
					// Now read the data for image, the palette and pixel data.
					int width = br.ReadInt32();
					int height = br.ReadInt32();
					fs.Position += 4;
					int totalSize = br.ReadInt32();
					if (width * height != totalSize) return null;

					byte[] palette = new byte[0x400];
					byte[] sourceData = new byte[totalSize];

					fs.Position = (0x24 + 0x824) - 0x400;
					fs.Read(palette, 0, 0x400);
					fs.Read(sourceData, 0, totalSize);

					// Optimize by converting the palette to 32 bit.
					// This will be faster than writing a bitmap one byte at a time.
					int[] pal32 = new int[256];
					for (int i = 0; i < 256; i++)
					{
						int offset = i * 4;
						int color = (palette[offset] << 16 | palette[offset+1] << 8 | palette[offset+2]);
						pal32[i] = (int)(color != 0 ? color | 0xFF000000 : 0);
					}

					Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
					var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
					int[] destData = new int[(bmpData.Height * bmpData.Stride) / 4];

					// Every 8-bit pixel is converted to 32-bit result.
					for (int y = 0; y < height; y++)
						for (int x = 0, offset = y * width; x < width; x++, offset++) destData[offset] = pal32[sourceData[offset]];

					Marshal.Copy(destData, 0, bmpData.Scan0, destData.Length);
					bmp.UnlockBits(bmpData);
					return bmp;
				}
			}
		}

		void importIcons(string filename, int size, bool scaleDown, float boostIntensity)
		{
			#if DEBUG && NOIMPORT
			filename = "";
			#endif

			if (!File.Exists(filename)) return;

			_craftIconImages.Clear();
			// This selection size will be overwritten when the list is finalized.
			_craftIconBracketSize = (int)(size * 0.4);

			Image iconSource = Image.FromFile(filename);

			if (iconSource.PixelFormat != PixelFormat.Format24bppRgb || scaleDown || boostIntensity > 1.0f)
			{
				int width = iconSource.Width;
				int height = iconSource.Height;
				Rectangle srcRect = new Rectangle(0, 0, width, height);
				if (scaleDown)
				{
					width /= 2;
					height /= 2;
					size /= 2;
				}
				Rectangle destRect = new Rectangle(0, 0, width, height);

				Bitmap convert = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				using (Graphics g = Graphics.FromImage(convert))
				{
					g.InterpolationMode = InterpolationMode.NearestNeighbor;
					g.DrawImage(iconSource, destRect, srcRect, GraphicsUnit.Pixel);
				}

				if (boostIntensity > 1.0f)
				{
					// Scale up the color intensity while also converting to grayscale if not already done.
					var bmpData = convert.LockBits(destRect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
					int lockSize = bmpData.Stride * bmpData.Height;
					byte[] pixelData = new byte[lockSize];
					Marshal.Copy(bmpData.Scan0, pixelData, 0, lockSize);
					const double MaxIntensity = 441.67295593;   // The square root of a vector length for RGB(255,255,255)
					for (int y = 0; y < bmpData.Height; y++)
					{
						int offset = y * bmpData.Stride;
						for (int x = 0; x < bmpData.Width; x++)
						{
							byte c1 = pixelData[offset];
							byte c2 = pixelData[offset+1];
							byte c3 = pixelData[offset+2];
							if (c1 + c2 + c3 > 0)
							{
								double intensity = Math.Sqrt(c1*c1 + c2*c2 + c3*c3) / MaxIntensity;
								intensity *= boostIntensity;
								if (intensity > 1.0) intensity = 1.0;

								byte col = (byte)(intensity * 255);
								pixelData[offset] = col;
								pixelData[offset+1] = col;
								pixelData[offset+2] = col;
							}
							offset += 3;
						}
					}
					Marshal.Copy(pixelData, 0, bmpData.Scan0, lockSize);
					convert.UnlockBits(bmpData);
				}

				iconSource = convert;
			}

			int tileWidth = iconSource.Width / size;
			int tileHeight = iconSource.Height / size;
			for (int y = 0; y < tileHeight; y++)
				for (int x = 0; x < tileWidth; x++)
				{
					CraftIconImage icon = new CraftIconImage { OriginalRect = new Rectangle(x * size, y * size, size, size) };
					_craftIconImages.Add(icon);
				}
			extractCraftIconList((Bitmap)iconSource, true);
			finalizeCraftIconList();
		}

		/// <summary>Only for TIE Fighter.  Creates a special list of icons where all drawn pixels are the same color.</summary>
		void createFlatIcons()
		{
			_craftFlatIconImages = new List<CraftIconImage>();
			foreach (CraftIconImage icon in _craftIconImages)
			{
				CraftIconImage flat = new CraftIconImage
				{
					DrawOffsetX = icon.DrawOffsetX,
					DrawOffsetY = icon.DrawOffsetY,
					Rect = icon.Rect,
					SquareSize = icon.SquareSize,
					Width = icon.Width,
					Height = icon.Height
				};

				Bitmap srcBmp = icon.Icon;
				Bitmap dstBmp = new Bitmap(srcBmp.Width, srcBmp.Height, PixelFormat.Format24bppRgb);
				Rectangle r = new Rectangle(0, 0, srcBmp.Width, srcBmp.Height);
				var srcData = srcBmp.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
				var dstData = dstBmp.LockBits(r, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
				int srcStride = Math.Abs(srcData.Stride);
				int dstStride = Math.Abs(dstData.Stride);
				int srcSize = srcBmp.Height * srcStride;
				int dstSize = dstBmp.Height * dstStride;
				byte[] srcSurface = new byte[srcSize];
				byte[] dstSurface = new byte[dstSize];
				Marshal.Copy(srcData.Scan0, srcSurface, 0, srcSize);
				for (int y = 0; y < srcBmp.Height; y++)
				{
					int srcOffset = y * srcStride;
					int dstOffset = y * dstStride;
					for (int x = 0; x < srcBmp.Width; x++)
					{
						int col = srcSurface[srcOffset] + srcSurface[srcOffset + 1] + srcSurface[srcOffset + 2];
						if (col != 0)
						{
							dstSurface[dstOffset] = 0xFF;
							dstSurface[dstOffset + 1] = 0xFF;
							dstSurface[dstOffset + 2] = 0xFF;
						}
						srcOffset += 4;
						dstOffset += 3;
					}
				}
				Marshal.Copy(dstSurface, 0, dstData.Scan0, dstSize);
				srcBmp.UnlockBits(srcData);
				dstBmp.UnlockBits(dstData);

				dstBmp.MakeTransparent(Color.Black);
				flat.Icon = dstBmp;
				_craftFlatIconImages.Add(flat);
			}
		}

		void mapTieIconToXwing()
		{
			// Temp list to store everything we loaded from TIE.
			List<CraftIconImage> tie = new List<CraftIconImage>();
			for (int i = 0; i < _craftIconImages.Count; i++) tie.Add(_craftIconImages[i]);

			// Build new icon list for XWING, pulling specific items from the TIE array.
			_craftIconImages = new List<CraftIconImage>();

			int[] tieIndex = new int[] {
				1, 2, 3,      //X-W, Y-W, A-W
				5, 6, 7, 16,  //T/F, T/I, T/B, GUN
				21, 17,24,32, //TRN, SHU, TUG, FRT
				33, 49,42,40, //CON, CRS, FRG, CRV
				51, 6, 71,71, //ISD, T/A                T/A icon is basically the same as T/I
				75,76, 70,80, //MINE, MINE, SAT, NAV
				84, 4         //PROB, B-W
			};

			_craftIconImages.Add(tie[0]); // Null entry

			for (int set = 0; set < 2; set++)
			{
				int baseOffset = set * 88;

				// Standard craft and objects
				for (int i = 0; i < tieIndex.Length; i++) _craftIconImages.Add(tie[baseOffset + tieIndex[i]]);
				// Asteroids
				for (int i = 0; i < 8; i++) _craftIconImages.Add(tie[baseOffset + 86]);
				// 15 planet, 1 death star.  Using another planet for the DS.
				for (int i = 0; i < 16; i++) _craftIconImages.Add(tie[baseOffset + 87]);
				// Junk, just use the container graphic as placeholder.
				for (int i = 0; i < 59; i++) _craftIconImages.Add(tie[baseOffset + 26]);
			}

			// The TIE icons were already built, so no need for that here.
			_smallIconOffset = 108;
		}

		/// <summary>Scans through the list of loaded icons, assigning the draw properties and metrics from their image sizes.</summary>
		void finalizeCraftIconList()
		{
			Bitmap empty = null;
			int totalSize = 0;
			int totalCount = 0;
			foreach (CraftIconImage icon in _craftIconImages)
			{
				if (icon.Icon == null)
				{
					if (empty == null)
					{
						empty = new Bitmap(1, 1, PixelFormat.Format24bppRgb);
						empty.SetPixel(0, 0, Color.Black);
						empty.MakeTransparent(Color.Black);
					}

					icon.Icon = empty;
					icon.Rect = new Rectangle(0, 0, 1, 1);
					continue;
				}

				icon.Icon.MakeTransparent(Color.Black);
				icon.Width = icon.Icon.Width;
				icon.Height = icon.Icon.Height;
				icon.SquareSize = Math.Max(icon.Width, icon.Height);
				icon.DrawOffsetX = -(icon.Width / 2);
				icon.DrawOffsetY = -(icon.Height / 2);
				
				totalSize += icon.SquareSize;
				totalCount++;
			}

			// If further customization of the bracket size is required, assign it in the platform's constructor.
			if (totalCount > 0) _craftIconBracketSize = (totalSize / totalCount) + 3;
		}

		/// <summary>Finishes loading an icon and modifying the icon list with the relevant information.</summary>
		/// <remarks>Attempts to crop the bitmaps, removing transparent borders for faster drawing.</remarks>
		/// <param name="master">The master bitmap containing all icons aggregated into a single image.  If null, the icon image must already be provided in the element.</param>
		/// <param name="crop">Only needed if loading the stock icon strips to trim excess space.  Should not be used when loading proper assets.</param>
		void extractCraftIconList(Bitmap master, bool crop)
		{
			foreach (CraftIconImage icon in _craftIconImages)
			{
				Bitmap original = icon.Icon;

				int width = icon.OriginalRect.Width;
				int height = icon.OriginalRect.Height;
				if (width == 0 || height == 0) continue;

				Rectangle destRect = new Rectangle(0, 0, width, height);
				Bitmap temp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				using (Graphics tg = Graphics.FromImage(temp))
				{
					tg.InterpolationMode = InterpolationMode.NearestNeighbor;
					Bitmap copySource = (master ?? original);
					tg.DrawImage(copySource, destRect, icon.OriginalRect, GraphicsUnit.Pixel);

					icon.Icon = temp;
					icon.Rect = destRect;

					if (!crop) continue;

					// Attempt to crop the bitmap and use that image instead.
					BitmapData bmpData = temp.LockBits(destRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
					int surfSize = Math.Abs(bmpData.Stride) * bmpData.Height;
					byte[] surfData = new byte[surfSize];
					Marshal.Copy(bmpData.Scan0, surfData, 0, surfSize);

					int startY = -1;
					int endY = -1;
					int startX = int.MaxValue;
					int endX = int.MinValue;

					for (int y = 0; y < bmpData.Height; y++)
					{
						for (int x = 0; x < bmpData.Width; x++)
						{
							int offset = (y * Math.Abs(bmpData.Stride)) + (x * 3);
							int intensity = surfData[offset] + surfData[offset+1] + surfData[offset+2];
							if (intensity != 0)
							{
								if (startY == -1) startY = y;
								endY = y;
								if (x > endX) endX = x;
								if (x < startX) startX = x;
							}
						}
					}

					temp.UnlockBits(bmpData);

					if (startY >= 0)
					{
						int cropWidth = (endX - startX) + 1;
						int cropHeight = (endY - startY) + 1;
						if (cropWidth != bmpData.Width || cropHeight != bmpData.Height)
						{
							Rectangle destCropRect = new Rectangle(0, 0, cropWidth, cropHeight);
							Rectangle srcCropRect = new Rectangle(startX, startY, cropWidth, cropHeight);
							Bitmap cropped = new Bitmap(cropWidth, cropHeight, PixelFormat.Format24bppRgb);
							using (Graphics cg = Graphics.FromImage(cropped))
							{
								cg.InterpolationMode = InterpolationMode.NearestNeighbor;
								cg.DrawImage(temp, destCropRect, srcCropRect, GraphicsUnit.Pixel);
							}
							icon.Icon = cropped;
							icon.Rect = destCropRect;
							icon.CropOffset = new Size(startX, startY);
						}
					}
				}
			}
		}
		#endregion Resources

		#region Form scaling
		int getSidebarWidth() => pnlTempCreate.Width;

		int getFormScaledValue(int value) => (int)(value * Settings.GetInstance().FormScale);

		void resizeTabs(int clientWidth, int clientHeight)
		{
			// The main form's client size dimensions are zero if minimized.
			if (clientWidth == 0 && clientHeight == 0) return;

			// This can get called multiple times during form init.  Avoids bogging down with repeat requests.
			if (_lastFormSize.Width == clientWidth && _lastFormSize.Height == clientHeight) return;

			tabBriefing.SuspendLayout();

			// Almost everything is contained within the tab control.
			// The tab control's overall size includes the row of tabs at the top.
			tabBriefing.Width = clientWidth - (tabBriefing.Left * 2);
			tabBriefing.Height = clientHeight - (tabBriefing.Top + tabBriefing.Margin.Bottom + pnlFooter.Height);

			// The footer is at the very bottom, outside the tab.
			pnlFooter.Top = clientHeight - pnlFooter.Height;

			// This is the usable space within the tab's display area, unlike the larger ClientRectangle that includes the tab itself.
			int width = tabBriefing.DisplayRectangle.Width;
			int height = tabBriefing.DisplayRectangle.Height;

			resizeStringTab(width, height);
			resizeMapTab(width, height);
			resizeEventTab(width, height);
			resizeBriefingOptionsTab(width, height);
			resizeVerifyTab(width, height);
			_lastFormSize = new Size(clientWidth, clientHeight);

			if (isCurrentTab(MainTabIndex.EventList))
			{
				resizeStringPanel(false);
				if (_selectedObjects.Count > 0) displayEditControls();
			}

			tabBriefing.ResumeLayout();
		}

		void resizeStringTab(int interiorWidth, int interiorHeight)
		{
			int maxListHeight = (_tags.Count * lstMainStrings.ItemHeight) + lstMainStrings.Margin.Bottom + 1;
			int availListHeight = interiorHeight - (lstMainStrings.Top + lstMainStrings.Margin.Bottom + pnlLargeEditString.Height);
			if (maxListHeight < availListHeight) availListHeight = maxListHeight;

			lstMainTags.Height = availListHeight;

			// The width of the string list can extend to the right edge of the screen
			lstMainStrings.Height = availListHeight;
			lstMainStrings.Width = interiorWidth - (lstMainStrings.Left + (lstMainStrings.Margin.Right * 4));

			// Align edit panel to bottom of string list, and width to match the right margin.
			// NOTE: The XWING form init increases the size of this panel and the string textboxes.
			pnlLargeEditString.Top = lstMainStrings.Bottom + lstMainStrings.Margin.Bottom;
			pnlLargeEditString.Width = lstMainStrings.Right - lstMainTags.Left;

			txtMainEditString.Width = (pnlLargeEditString.Right - txtMainEditString.Left) - (txtMainEditString.Margin.Right * 2);
			lblStringHint.Width = (txtMainEditString.Right - lblStringHint.Left);
			txtCaptionNotes.Width = (txtMainEditString.Right - txtCaptionNotes.Left);
		}

		void resizeMapTab(int interiorWidth, int interiorHeight)
		{
			// The map tab interior is divided into two main areas.  Left and right.
			// The left pane contains the map display, with map scrollbars below and to the right.
			// Underneath the map is the timeline, and the playback/time controls under that.
			// The right pane contains a sidebar which dynamically holds controls for creating events,
			// editing items, time shift, or quick-access display options.

			// The playback controls are fixed to the bottom.
			pnlBottomLeft.Top = interiorHeight - pnlBottomLeft.Height;
			
			// Find out how much space is available for the map and timeline.
			int availWidth = interiorWidth - (getMapMarginWidth() + vsbBRF.Width + getSidebarWidth());
			int availHeight = interiorHeight - (getMapMarginHeight() + hsbBRF.Height + getMinimumTimelineHeight() + pnlBottomLeft.Height);

			int reqMapWidth = getMapCanvasWidth();
			int reqMapHeight = getMapCanvasHeight();

			// Failsafe from the old code which was horribly broken.  Just in case there's a problem somewhere.
			if (availWidth < reqMapWidth) availWidth = reqMapWidth;
			if (availHeight < reqMapHeight) availHeight = reqMapHeight;

			// Determine how to scale the map.  Default to configuration, then check for dynamic size.
			_mapScale = (float)numConfigMapScale.Value;
			if (optConfigDynamicScale.Checked)
			{
				float scaleWidth = (float)availWidth / reqMapWidth;
				float scaleHeight = (float)availHeight / reqMapHeight;
				
				// Limit by the smallest of the two scales, to maintain aspect ratio.
				float scale = Math.Min(scaleWidth, scaleHeight);
				if (scale < 1.0f) scale = 1.0f;

				_mapScale = scale;
			}

			// Use the proper rendering mode for the applied scale.
			refreshInterpolationMode();

			// Now calculate the actual map size and assign the position.
			int mapWidth = (int)(reqMapWidth * _mapScale);
			int mapHeight = (int)(reqMapHeight * _mapScale);
			
			pctBrief.Top = pctBrief.Margin.Top;
			pctBrief.Size = new Size(mapWidth, mapHeight);
			pctBrief.Left = Math.Max(pctBrief.Margin.Left, (availWidth - mapWidth) / 2);

			// Assign location and size of the map scrollbars, relative to the map.
			setIsolatedScrollbarSize(hsbBRF, pctBrief.Left, pctBrief.Bottom, mapWidth, 0);
			setIsolatedScrollbarSize(vsbBRF, pctBrief.Right, pctBrief.Top, 0, mapHeight);

			// The timeline gets whatever is left.  It uses the full horizontal space (minus the sidebar) even if the map is scaled down.
			// Add the height back that was subtracted when calculating the available map space.
			int minTimelineHeight = getMinimumTimelineHeight();
			int timelineWidth = availWidth;
			int timelineHeight = (availHeight - mapHeight) + minTimelineHeight;

			// There's a discrepancy with fixed map scales above 1.0 where sometimes the resulting map height is
			// larger than the available height, which makes the timeline shorter than expected.
			// It's only a few pixels so not breaking.  It suggests that something, somewhere, isn't calculating
			// correctly.  Maybe the form minimum size.  But it seems all variables are accounted for?
			if (timelineHeight < minTimelineHeight) timelineHeight = minTimelineHeight;

			Size oldTimelineSize = pctTimeline.Size;
			pctTimeline.Top = pctBrief.Bottom + hsbBRF.Height + pctBrief.Margin.Bottom;
			pctTimeline.Size = new Size(timelineWidth, timelineHeight);
			setIsolatedScrollbarSize(vsbTimeline, pctTimeline.Right, pctTimeline.Top, vsbTimeline.Width, timelineHeight);

			// Only for XWING, occupies the space when in Panel mode.
			lblMapPanelMode.Location = pctTimeline.Location;
			lblMapPanelMode.Size = pctTimeline.Size;

			// Playback controls below the timeline.
			pnlBottomLeft.Width = pctTimeline.Right - pnlBottomLeft.Left;
			hsbTimer.Width = (pnlBottomLeft.Right - hsbTimer.Left) - pnlBottomLeft.Margin.Right;

			// Center the time labels over the scrollbar.
			int labelPos = (hsbTimer.Width / 2) - (lblTime.Width + lblTimelineOffset.Width) / 2;
			lblTime.Left = hsbTimer.Left + Math.Max(0, labelPos);
			lblTimelineOffset.Left = lblTime.Right;

			// To reuse the edit controls, the sidebar panel is moved between tabs and may not exist here at this time.
			if (tabDisplay.Contains(pnlEditContainer))
			{
				pnlEditContainer.Left = interiorWidth - pnlEditContainer.Width;
				pnlEditContainer.Top = pctBrief.Top;
				pnlEditContainer.Height = interiorHeight - pnlEditContainer.Top;
				pnlMapOptions.Top = pnlEditContainer.Bottom - pnlMapOptions.Height;
				pnlMapOptions.Left = 0;
			}

			// All control placements finished.

			// NOTE: pctBrief used to have an image assigned but it was removed, see notes in the Paint function.
			/// The rendering has changed, <see cref="pctBrief_Paint"/>

			// Waiting on the refresh timer is slow, which can cause flicker when resizing.
			// Blit whatever is currently on the canvas.
			blitMap();

			if (pctTimeline.Image == null || oldTimelineSize != pctTimeline.Size)
			{
				// The timeline doesn't maintain its own canvas, so stretch the old image before disposing.
				Image oldTimelineImage = pctTimeline.Image;
				pctTimeline.Image = new Bitmap(pctTimeline.Width, pctTimeline.Height);

				if (oldTimelineImage != null)
				{
					using (Graphics g = Graphics.FromImage(pctTimeline.Image))
					{
						g.InterpolationMode = InterpolationMode.NearestNeighbor;
						g.DrawImage(oldTimelineImage, 0, 0);
					}
					oldTimelineImage.Dispose();
				}
			}

			// Calculate the scaled output locations on the combined canvas, based on the map scale.
			int titleHeight = (isTie ? 12 : _titleCanvas.Height);
			_scaledTitleRect = new Rectangle(0, 0, (int)(_titleCanvas.Width * _mapScale), (int)(titleHeight * _mapScale));
			int y = _scaledTitleRect.Height;

			_scaledMapRect = new Rectangle(0, y, (int)(_mapCanvas.Width * _mapScale), (int)(_mapCanvas.Height * _mapScale));
			y += _scaledMapRect.Height;
			_scaledCaptionRect = new Rectangle(0, y, (int)(_captionCanvas.Width * _mapScale), (int)(_captionCanvas.Height * _mapScale));
		}
		
		void resizeEventTab(int interiorWidth, int interiorHeight)
		{
			int width = tabBriefing.Left + pnlEventListManage.Width + ((pnlEventListManage.Margin.Left + pnlEventListManage.Margin.Right) * 2);
			
			lstEvents.Width = interiorWidth - width;
			lstEvents.Height = interiorHeight - 3;

			pnlEventListButtons.Left = lstEvents.Right + lstEvents.Margin.Right;
			pnlEventListManage.Left = lstEvents.Right + lstEvents.Margin.Right;

			// To reuse the edit controls, the panel is moved between tabs and may not exist here at this time.
			if (tabEvents.Contains(pnlEditContainer))
			{
				pnlEditContainer.Left = lstEvents.Right + lstEvents.Margin.Right;
				pnlEditContainer.Top = pnlEventListManage.Bottom + pnlEventListManage.Margin.Bottom;
				pnlEditContainer.Height = interiorHeight - pnlEditContainer.Top;
			}
		}

		void resizeBriefingOptionsTab(int interiorWidth, int interiorHeight)
		{
			txtBriefImport.Width = interiorWidth - (txtBriefImport.Left + txtBriefImport.Margin.Right);
			txtBriefImport.Height = interiorHeight - (txtBriefImport.Top + txtBriefImport.Margin.Bottom);
		}

		void resizeVerifyTab(int interiorWidth, int interiorHeight)
		{
			lblRepair.Width = interiorWidth - (lblRepair.Left + lblRepair.Margin.Right);
			txtVerify.Width = interiorWidth - (txtVerify.Left + txtVerify.Margin.Right);
			txtVerify.Height = interiorHeight - (txtVerify.Top + txtVerify.Margin.Bottom);
		}

		/// <summary>Retrieve the width on the form that a map at 1x scale will occupy.</summary>
		int getMapCanvasWidth()
		{
			// When resizing during init, use the designer size.
			if (_combinedCanvas == null) return pctBrief.Width;

			int width = _combinedCanvas.Width;  // Default to minimum of 1x scale
			if (isXwing && !isXwingHighDef()) width *= 2;
			return width;
		}
		/// <summary>Retrieve the height on the form that a map at 1x scale will occupy.</summary>
		int getMapCanvasHeight()
		{
			// When resizing during init, use the designer size.
			if (_combinedCanvas == null) return pctBrief.Height;

			int height = _combinedCanvas.Height;  // Default to minimum of 1x scale
			if (isXwing && !isXwingHighDef()) height *= 2;
			return height;
		}

		Size getMinimumMapSize()
		{
			// If the map is dynamically resizeable, the minimum map size cannot fall below 1.0 scale.
			int mapWidth = getMapCanvasWidth();
			int mapHeight = getMapCanvasHeight();
			if (!optConfigDynamicScale.Checked)
			{
				float mapScale = Math.Max(1.0f, (float)numConfigMapScale.Value);
				mapWidth = (int)(mapWidth * mapScale);
				mapHeight = (int)(mapHeight * mapScale);
			}
			return new Size(mapWidth, mapHeight);
		}

		int getMapMarginWidth() => pctBrief.Margin.Left + pctBrief.Margin.Right;
		int getMapMarginHeight() => pctBrief.Margin.Top + pctBrief.Margin.Bottom;

		Size getMinimumFormSize()
		{
			// Calculate the absolute minimum size that the entire form can be, in order to fit all elements.
			// The map tab is the primary concern, because it's the most complex tab and the configured map size
			// is what drives the main form size and thus all resizing operations.  The map tab must also be
			// tall enough to hold the sidebar, and wide enough to hold the footer.
			// There are also some panels on other tabs that need to be considered due to their fixed sizes.

			// The total form size includes the operating system title and border.  The client is the usable interior space.
			int windowBorderWidth = Width - ClientRectangle.Width;
			int windowBorderHeight = Height - ClientRectangle.Height;

			// The tab control contains its own border and "title" area with buttons.
			int tabBorderWidth = tabBriefing.Width - tabBriefing.DisplayRectangle.Width;
			int tabBorderHeight = tabBriefing.Height - tabBriefing.DisplayRectangle.Height;

			// The base amounts determine the total size of all elements and spacing outside the tab interior.
			int baseWidth = windowBorderWidth + tabBorderWidth;
			baseWidth += tabBriefing.Left * 2;  // Reserved margin space on both sides.

			int baseHeight = windowBorderHeight + tabBorderHeight;
			baseHeight += tabBriefing.Top;
			baseHeight += tabBriefing.Margin.Bottom;
			baseHeight += pnlFooter.Height;

			// Calculate the total size of the interior tab contents.
			// This makes it easier to compare and find the largest interior spans.
			// First start with everything that appears on the briefing map tab, including timeline and sidebar.
			Size map = getMinimumMapSize();
			int mapWidth = getMapMarginWidth() + map.Width;
			mapWidth += vsbBRF.Width;
			mapWidth += getSidebarWidth();

			int mapHeight = getMapMarginHeight() + map.Height;
			mapHeight += hsbBRF.Height;
			mapHeight += getMinimumTimelineHeight();
			mapHeight += pnlBottomLeft.Height;            // Contains playback controls and briefing time scroll.

			// The height required by the sidebar.
			int sidebarHeight = pnlTempCreate.Height;     // Dynamically sized depending on platform
			sidebarHeight += grpTimeShift.Top - pnlTempCreate.Bottom;  // Spacing in designer
			sidebarHeight += grpTimeShift.Height;
			sidebarHeight += grpTimeShift.Margin.Bottom;
			sidebarHeight += pnlMapOptions.Height;        // Dynamically sized

			if (isXwa)
			{
				// The tallest array of edit controls will appear if a MoveIcon is selected.
				int xwa = pnlIcon.Height + pnlIcon.Margin.Bottom;
				xwa += pnlPosition.Height + pnlPosition.Margin.Bottom;
				xwa += pnlMoveOptions.Height + pnlMoveOptions.Margin.Bottom;
				xwa += pnlRotation.Height + pnlRotation.Margin.Bottom;
				xwa += pnlIconMove.Height;
				sidebarHeight = Math.Max(sidebarHeight, xwa);
			}
			mapHeight = Math.Max(mapHeight, sidebarHeight);

			// Map and sidebar spans are finished, but there are panels in other tabs.
			// This is the largest fixed-size panel that exists within any tab.
			int otherWidth = grpXwingPageTemplates.Location.X + grpXwingPageTemplates.Width;

			// Don't want to hide the load report since it could have important asset information.
			int otherHeight = txtStatusLoadReport.Bottom;

			// Finalize the largest tab interiors.
			int tabWidth = Math.Max(mapWidth, otherWidth);
			int tabHeight = Math.Max(mapHeight, otherHeight);

			// Calculate the final size with the exterior elements included.
			// The footer is the widest fixed-size panel that exists outside the tab.
			int widthReq = baseWidth + Math.Max(tabWidth, pnlFooter.Width);
			int heightReq = baseHeight + tabHeight;
			return new Size(widthReq, heightReq);
		}

		int getMinimumTimelineHeight() => (_timelineFont.Height + 2) * ((int)numTimelineRowCount.Value + 1);

		/// <summary>Recursively calls SuspendLayout for the specified control and all child controls.</summary>
		void suspendLayouts(Control container)
		{
			container.SuspendLayout();
			foreach (Control c in container.Controls) suspendLayouts(c);
		}
		/// <summary>Recursively calls ResumeLayout for the specified control and all child controls.</summary>
		void resumeLayouts(Control container)
		{
			container.ResumeLayout(false);
			foreach (Control c in container.Controls) resumeLayouts(c);
		}

		/// <summary>Performs a full resize operation, recalculating and resizing all containers and controls based on the current form size.</summary>
		/// <param name="force">Forces an update even if the size hasn't changed.</param>
		void updateFormSize(bool force)
		{
			if (force) _lastFormSize = new Size(0, 0);

			// Setting the minimum size automatically adjusts the current size if needed.
			MinimumSize = getMinimumFormSize();

			suspendLayouts(this);
			resizeTabs(ClientRectangle.Width, ClientRectangle.Height);
			resumeLayouts(this);
			refreshTimeline();
		}

		/// <summary>Reassigns the graphics interpolation mode of the map canvas depending on the current map scale and config settings.</summary>
		void refreshInterpolationMode()
		{
			// Default to selected mode, unless fixed size and auto nearest is checked.
			_interpolationMode = (InterpolationMode)cboConfigRenderMode.SelectedIndex;

			float scale = _mapScale;
			if (isXwing && !isXwingHighDef()) scale *= 2.0f;

			// Check if fixed size is a whole integer.
			if (optConfigFixedScale.Checked && chkConfigAutoNearest.Checked && ((int)(scale * 100.0f) % 100 == 0))
				_interpolationMode = InterpolationMode.NearestNeighbor;

			refreshMap();
		}
		#endregion Form scaling

		#region Undo and redo
		/// <summary>Searches the current frame for an existing operation.</summary>
		/// <returns>The operation if found, or null if not found.</returns>
		UndoOperation getUndoOperation(int UID)
		{
			if (UID <= 0) return null;

			foreach (UndoOperation op in _currentUndoFrame) if (UID == op.UID) return op;

			return null;
		}

		/// <summary>Completely clears and resets all states related to the Undo/Redo stack.</summary>
		void clearUndo()
		{
			// This is called automatically as part of the briefing selection during init.
			// But don't replace the load status text until there's actually something to clear.
			int prevCount = _undoFrames.Count + _redoFrames.Count + _currentUndoFrame.Count;

			_undoFrames.Clear();
			_redoFrames.Clear();
			_currentUndoFrame.Clear();
			
			if (prevCount != 0) refreshRevertStatus();
		}

		/// <summary>Prepares a new frame of Undo operations, performing all necessary management.</summary>
		/// <param name="finalized">Indicates that subsequent add operations are finalized, not waiting for updates.  This implies a guaranteed change of state.</param>
		void beginUndoFrame(bool finalized)
		{
			// Avoid refreshing the load status at the bottom if nothing is actually happening here.
			if (_currentUndoFrame.Count + _undoFrames.Count + _redoFrames.Count == 0) return;

			if (finalized) _redoFrames.Clear();

			// If the list is empty, we don't need to do anything.
			// Otherwise filter the list, push the changes, and start a new frame.
			if (_currentUndoFrame.Count > 0)
			{
				// Delete any elements that were never updated
				int pos = 0;
				while (pos < _currentUndoFrame.Count)
				{
					if (_currentUndoFrame[pos].HasUpdated()) pos++;
					else _currentUndoFrame.RemoveAt(pos);
				}

				// If there are any items remaining, add the list and begin a new frame.
				if (_currentUndoFrame.Count > 0)
				{
					// Since a modification has been made, anything in the redo buffer is no longer valid.
					if (_redoFrames.Count > 0) _redoFrames.Clear();

					_undoFrames.Add(_currentUndoFrame);
					_currentUndoFrame = new List<UndoOperation>();
				}
			}
			refreshRevertStatus();
		}

		/// <summary>Adds an undo operation to the current frame.</summary>
		/// <param name="op">Operation to add.</param>
		/// <param name="newFrame">If true, begins a new empty frame before adding the operation.</param>
		void addUpdatedUndoOperation(UndoOperation op, bool newFrame)
		{
			// This operation is finalized, meaning we're not waiting on any additional updates.
			// This is a guaranteed change of states, so the redo buffer must be cleared.
			_redoFrames.Clear();

			if (newFrame) beginUndoFrame(true);

			addUndoOperation(op, true);
		}

		/// <summary>Adds an undo operation to the current frame.  Notifies the parent form that something changed.</summary>
		void addUndoOperation(UndoOperation op, bool dirty)
		{
			_currentUndoFrame.Add(op);
			if (dirty) markDirty();
			refreshRevertStatus();
		}

		/// <summary>Refreshes the status text in the main form's footer panel to display the number of undo frames in the stack.</summary>
		void refreshRevertStatus()
		{
			int count = _undoFrames.Count;
			foreach (UndoOperation op in _currentUndoFrame)
			{
				if (op.HasUpdated())
				{
					count++;
					break;
				}
			}

			string s = "Nothing to undo";
			if (count > 0) s = count.ToString() + " undo frame" + (count > 1 ? "s" : "");

			lblStatus.Text = s;
		}

		/// <summary>Checks that it's possible to undo a particular frame of operations while in path mode.</summary>
		/// <remarks>Path mode generates Move and Rotate events, and may also change briefing duration.  Those are the only kinds of events allowed to be undone here.</remarks>
		bool isPathmodeUndoable(List<UndoOperation> frame)
		{
			// Originally undo was not allowed in path mode, to convey to the user that edits to nodes are not undoable.
			// However this prevented generated icons from being quickly undone, requiring exiting path mode first.
			foreach (UndoOperation op in frame)
			{
				if (op.OpType == UndoType.Duration) continue;

				if (op.OpType != UndoType.Event && op.OpType != UndoType.NewEvent && op.OpType != UndoType.DeleteEvent) return false;

				AbstractEvent evt = (AbstractEvent)op.GetData(true);

				// If a path is replaced, it may delete existing rotate events, which is acceptable.
				if (op.OpType == UndoType.DeleteEvent && evt.Event == AbstractEventType.XwaRotateIcon) continue;

				if (evt.Event != AbstractEventType.XwaMoveIcon && evt.Event != AbstractEventType.XwaRotateIcon) return false;
			}
			return true;
		}

		/// <summary>Checks that it's possible to undo a particular frame of operations while in the string tab.</summary>
		/// <remarks>Includes direct string edits, but also swapping string positions which can include certain event changes.</remarks>
		bool isStringTabUndoable(List<UndoOperation> frame)
		{
			foreach (UndoOperation op in frame)
			{
				if (!op.HasUpdated()) continue;
				if (op.OpType == UndoType.String || op.OpType == UndoType.Tag) continue;
				if (op.OpType == UndoType.SwapString || op.OpType == UndoType.SwapTag || op.OpType == UndoType.SwapNote) continue;
				if (op.OpType == UndoType.Event)
				{
					AbstractEvent one = (AbstractEvent)op.GetData(false);
					AbstractEvent two = (AbstractEvent)op.GetData(true);
					if (one.IsAnyCaption || one.IsTextTag || one.Event == AbstractEventType.XwaInfoParagraph)
					{
						// First param of all these events is the string index.  That is allowed to be different.
						// But for tags, make sure there wasn't a position or color change.
						if (one.IsTextTag)
							for (int i = 1; i < one.Params.Length; i++)
								if (one.Params[i] != two.Params[i]) return false;

						// Everything else must match.
						if (one.Event == two.Event && one.Time == two.Time) continue;
					}
				}
				// If we get here, an unsupported undo type, unsupported event, or mismatched event type or time.
				return false;
			}
			return true;
		}

		/// <summary>Checks if there's at least one finished operation in the specified frame that can be undone.</summary>
		bool isUndoable(List<UndoOperation> frame)
		{
			foreach (UndoOperation op in frame)
				if (op.HasUpdated()) return true;
			
			// The list was empty, or the list only contains non-updated items.
			return false;
		}

		void performUndo()
		{
			if (_tempEvent != AbstractEventType.None) return;
			if (!isUndoable(_currentUndoFrame) && _undoFrames.Count == 0) return;

			List<UndoOperation> opList = (isUndoable(_currentUndoFrame) ? _currentUndoFrame : _undoFrames[_undoFrames.Count - 1]);
			if (_pathMode && !isPathmodeUndoable(opList))
			{
				setTitleOverride("Cannot undo this operation in path mode.", 3);
				return;
			}
			if (isCurrentTab(MainTabIndex.MainStrings) && !isStringTabUndoable(opList))
			{
				popupInfo("Can only undo string changes in the string tab.");
				return;
			}

			UndoType changes = UndoType.None;

			// Undo everything in the current frame.
			if (_currentUndoFrame.Count > 0)
			{
				changes = revertChanges(_currentUndoFrame, false);
				_redoFrames.Add(_currentUndoFrame);

				_currentUndoFrame = new List<UndoOperation>();
			}
			
			// If nothing was in the current frame, or nothing was actually undone, proceed to the next frame if one exists.
			if (changes == UndoType.None && _undoFrames.Count > 0)
			{
				int index = _undoFrames.Count - 1;
				changes = revertChanges(_undoFrames[index], false);
				_redoFrames.Add(_undoFrames[index]);
				_undoFrames.RemoveAt(index);

				_currentUndoFrame = new List<UndoOperation>();
			}

			revertFinished(changes);
			refreshRevertStatus();
		}

		void performRedo()
		{
			if (_tempEvent != AbstractEventType.None) return;

			// If something is in the current frame, we can't redo.
			if (_currentUndoFrame.Count > 0 || _redoFrames.Count == 0) return;

			List<UndoOperation> opList = _redoFrames[_redoFrames.Count - 1];
			if (_pathMode && !isPathmodeUndoable(opList))
			{
				setTitleOverride("Cannot redo this operation in path mode.", 3);
				return;
			}
			if (isCurrentTab(MainTabIndex.MainStrings) && !isStringTabUndoable(opList))
			{
				popupInfo("Can only redo string changes in the string tab.");
				return;
			}

			int index = _redoFrames.Count - 1;
			UndoType changes = revertChanges(_redoFrames[index], true);
			_undoFrames.Add(_redoFrames[index]);
			_redoFrames.RemoveAt(index);

			_currentUndoFrame = new List<UndoOperation>();

			revertFinished(changes);
			refreshRevertStatus();
		}

		/// <summary>Performs changes to actual data states as part of an undo or redo operation.</summary>
		UndoType revertChanges(List<UndoOperation> operations, bool redo)
		{
			UndoType changes = UndoType.None;
			if (operations.Count == 0) return changes;

			// If undoing, traverse backwards in case ordering matters.
			int pos = operations.Count - 1;
			int step = -1;

			if (redo)
			{
				// If redoing, we have to traverse forward.
				pos = 0;
				step = 1;
			}

			while (pos >= 0 && pos < operations.Count)
			{
				UndoOperation op = operations[pos];
				pos += step;

				// If the operation was never updated with new data, it can be discarded.
				if (!op.HasUpdated()) continue;

				changes |= op.OpType;

				if (op.OpType == UndoType.FlightgroupMove)
				{
					if (op.Index >= 0 && op.Index < _flightgroups.Count)
					{
						AbstractFlightgroup afg = _flightgroups[op.Index];
						AbstractFlightgroup data = (AbstractFlightgroup)op.GetData(redo);
						Point p = data.DisplayPos;
						afg.Waypoints[op.SubIndex].RawX = (short)p.X;
						afg.Waypoints[op.SubIndex].RawY = (short)getInvertedY(p.Y);
						afg.UndoUID = -1;
						notifyWaypointChange(op.Index);
					}
				}
				else if (op.OpType == UndoType.Event)
				{
					if (op.Index >= 0 && op.Index < _events.Count)
					{
						AbstractEvent evt = _events[op.Index];
						AbstractEvent data = (AbstractEvent)op.GetData(redo);
						evt.CopyFrom(data);
					}
				}
				else if (op.OpType == UndoType.SwapEvent)
				{
					moveEventIndex((int)op.NewData, (int)op.OldData, false);
				}
				else if (op.OpType == UndoType.TimeShift)
				{
					int shiftAmount = (int)op.GetData(redo);
					timeShift(op.Index, shiftAmount, false);
				}
				else if (op.OpType == UndoType.Duration)
				{
					int maxTime = (int)op.GetData(redo);
					setDuration(maxTime, false);
				}
				else if (op.OpType == UndoType.NewEvent)
				{
					if (redo)
					{
						AbstractEvent evt = (AbstractEvent)op.GetData(redo);
						insertEvent(op.Index, evt, false);
					}
					else deleteEvent(op.Index);
				}
				else if (op.OpType == UndoType.DeleteEvent)
				{
					if (redo) deleteEvent(op.Index);
					else
					{
						AbstractEvent evt = (AbstractEvent)op.GetData(redo);
						insertEvent(op.Index, evt, false);
					}
				}
				else if (op.OpType == UndoType.String)
				{
					revertString(BriefingString.String, op.Index, (string)op.GetData(redo));
				}
				else if (op.OpType == UndoType.Tag)
				{
					revertString(BriefingString.Tag, op.Index, (string)op.GetData(redo));
				}
				else if (op.OpType == UndoType.SwapString)
				{
					int index1 = (int)op.GetData(true);
					int index2 = (int)op.GetData(false);
					swapMainString(index1, index2, _strings, lstMainStrings, _maxStringLength, UndoType.None);
					swapXwingEventStrings(BriefingString.String, index1, index2);
				}
				else if (op.OpType == UndoType.SwapTag)
				{
					int index1 = (int)op.GetData(true);
					int index2 = (int)op.GetData(false);
					swapMainString(index1, index2, _tags, lstMainTags, _maxTagLength, UndoType.None);
					swapXwingEventStrings(BriefingString.Tag, index1, index2);
				}
				else if (op.OpType == UndoType.SwapNote)
				{
					swapMainString((int)op.GetData(true), (int)op.GetData(false), _captionNotes, null, -1, UndoType.None);
				}
				else if (op.OpType == UndoType.FlightgroupData)
				{
					if (op.Index >= 0 && op.Index < _flightgroups.Count)
					{
						var afg = _flightgroups[op.Index];
						AbstractFlightgroup data = (AbstractFlightgroup)op.GetData(redo);
						afg.CraftType = data.CraftType;
						afg.CraftIff = data.CraftIff;
						afg.DisplayName = data.DisplayName;
						afg.Name = data.Name;
					}
				}
			}

			return changes;
		}

		void revertFinished(UndoType changes)
		{
			bool rebuild = false, relink = false, strings = false;

			if (changes.HasFlag(UndoType.Event)) rebuild = true;
			if (changes.HasFlag(UndoType.SwapEvent)) relink = true;

			if (changes.HasFlag(UndoType.NewEvent) || changes.HasFlag(UndoType.DeleteEvent))
			{
				rebuild = true;
				relink = true;
			}

			if (changes.HasFlag(UndoType.String) || changes.HasFlag(UndoType.Tag)) strings = true;
			if (changes.HasFlag(UndoType.SwapString) || changes.HasFlag(UndoType.SwapTag)) strings = true;
			if (changes.HasFlag(UndoType.TimeShift)) rebuild = true;

			if (rebuild)
			{
				rebuildBriefing();
				rebuildShadowIcons();
			}

			if (relink) relinkSelectedEventUid();
			if (strings && isCurrentTab(MainTabIndex.MainStrings)) populateMainStrings(-1, -1);

			if ((rebuild || relink || strings) && isCurrentTab(MainTabIndex.EventList))
			{
				refreshEventList(-1);
				applySelectionToEventList();
				refreshEventListMoveButtons();
			}

			if (!_pathMode) displayEditControls();

			refreshCanvas(RefreshFlags.All);
		}

		/// <summary>Creates or modifies an Undo state that is meant to be externally tracked via UID.</summary>
		/// <param name="undoUid">Source UID to get if existing, or destination UID to set if creating.</param>
		/// <param name="data">Data packet to attach.</param>
		/// <param name="type">Operation type indicating how the data packet will be interpreted.</param>
		/// <param name="index">Array or list index for the operation type, if applicable.</param>
		/// <param name="subindex">Secondary index, only for specific types.</param>
		void updateUndoProperty(ref int undoUid, object data, UndoType type, int index, int subindex = 0)
		{
			UndoOperation op = getUndoOperation(undoUid);
			if (op == null)
			{
				op = new UndoOperation(type, data, index, subindex);
				undoUid = op.UID;
				addUndoOperation(op, false);
			}
			else
			{
				bool state = op.HasUpdated();
				op.Update(data);
				if (!state) refreshRevertStatus();
			}
		}

		/// <summary>Modifies an Undo state that is known to previously exist, applying a new data packet.</summary>
		/// <param name="undoUid">UID to retrieve</param>
		/// <param name="data">New data packet to assign.</param>
		void updateKnownUndoProperty(int undoUid, object data)
		{
			UndoOperation op = getUndoOperation(undoUid);
			if (op != null)
			{
				bool state = op.HasUpdated();
				op.Update(data);
				if (!state) refreshRevertStatus();
			}
		}
		#endregion Undo and redo

		#region Copy and paste
		void cutString(BriefingString bstr, int index)
		{
			if (index < 0) return;

			List<string> list = getStringList(bstr);
			string oldString = list[index];

			UndoType ut = (bstr == BriefingString.String ? UndoType.String : UndoType.Tag);
			UndoOperation op = new UndoOperation(ut, oldString, index);
			op.Update("");
			addUpdatedUndoOperation(op, true);

			Clipboard.SetText(oldString);
			list[index] = "";
			populateMainStrings(index, index);
		}

		void copyString(BriefingString bstr, int index)
		{
			if (index < 0) return;

			string s = getSafeString(getStringList(bstr), index);
			if (!string.IsNullOrWhiteSpace(s)) Clipboard.SetText(s);
		}

		void pasteString(BriefingString bstr, int index)
		{
			if (index < 0) return;

			List<string> list = getStringList(bstr);
			string oldString = list[index];
			string newString = Clipboard.GetText();

			newString = newString.Replace('\t', '.');
			char[] remove = new char[] {'\r', '\n' };
			foreach (char c in remove)
			{
				int pos = newString.IndexOf(c);
				if (pos >= 0) newString = newString.Remove(pos);
			}

			UndoType ut = (bstr == BriefingString.String ? UndoType.String : UndoType.Tag);
			UndoOperation op = new UndoOperation(ut, oldString, index);
			op.Update(newString);
			addUpdatedUndoOperation(op, true);

			list[index] = newString;
			populateMainStrings(index, index);
		}

		void cutEvents()
		{
			var selection = getSelectedEvents();
			if (selection.Count == 0) return;

			// Delete from highest index to lowest so that the indices are valid when deleting.
			selection.Sort(SelectedObject.CompareDescending);

			_eventCopyBuffer.Clear();
			deselectObjects();
			beginUndoFrame(true);

			foreach (var so in selection)
			{
				if (isValidEvent(so))
				{
					AbstractEvent evt = new AbstractEvent(_events[so.Index]);
					_eventCopyBuffer.Add(evt);

					var data = evt.GetDataSnapshot();
					UndoOperation op = new UndoOperation(UndoType.DeleteEvent, data, so.Index);
					op.Update(data);
					addUpdatedUndoOperation(op, false);
					deleteEvent(so.Index);
				}
			}

			// We deleted backwards, so the resulting list of events is inverted.  Reverse it to paste in the correct order.
			_eventCopyBuffer.Reverse();
			serializeEventClipboard();

			refreshCanvas(RefreshFlags.All);
			refreshEventList(-1);
		}

		void serializeEventClipboard()
		{
			Stream stream = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(stream);  // No "using" directive since it invalidates the stream when it disposes.
			bw.Write(_eventCopyBuffer.Count);
			foreach (var evt in _eventCopyBuffer)
			{
				bw.Write(evt.Time);
				bw.Write((short)evt.Event);
				for (int i = 0; i < evt.Params.Length; i++) bw.Write(evt.Params[i]);

				string s = null;
				if (evt.IsAnyCaption) s = getSafeString(_strings, evt.Params[0]);
				else if (evt.IsTextTag) s = getSafeString(_tags, evt.Params[0]);
				else if (evt.Event == AbstractEventType.XwaInfoParagraph) s = getSafeString(_strings, evt.Params[0] - 1);   // If out of range it will be empty
				if (s != null) bw.Write(s);
			}
			DataObject data = new DataObject();
			data.SetText("event_list");
			data.SetData("yogeme_brf", false, stream);
			Clipboard.SetDataObject(data, true);
		}

		void unserializeEventClipboard()
		{
			if (!(Clipboard.GetDataObject() is DataObject data)) return;

			if (data.GetText() != "event_list") return;

			if (data.GetData("yogeme_brf", false) is MemoryStream stream)
			{
				stream.Seek(0, SeekOrigin.Begin);
				using (BinaryReader br = new BinaryReader(stream))
				{
					_eventCopyBuffer.Clear();
					int count = br.ReadInt32();
					for (int i = 0; i < count; i++)
					{
						AbstractEvent evt = new AbstractEvent
						{
							Time = br.ReadInt16(),
							Event = (AbstractEventType)br.ReadInt16()
						};
						for (int j = 0; j < evt.Params.Length; j++) evt.Params[j] = br.ReadInt16();

						// Read the string even if we're not going to use it.
						string s = null;
						if (evt.IsAnyCaption || evt.IsTextTag || evt.Event == AbstractEventType.XwaInfoParagraph) s = br.ReadString();

						if (!string.IsNullOrEmpty(s))
						{
							int index = -1;
							if (evt.IsAnyCaption) index = getOrCreateString(_strings, s);
							else if (evt.IsTextTag) index = getOrCreateString(_tags, s);
							else if (evt.Event == AbstractEventType.XwaInfoParagraph) index = getOrCreateString(_strings, s) + 1;
							if (index >= 0) evt.Params[0] = (short)index;
						}

						_eventCopyBuffer.Add(evt);
					}
				}
			}
		}
		void copyEvents()
		{
			List<SelectedObject> selection = getSelectedEvents();
			if (selection.Count == 0) return;

			_eventCopyBuffer.Clear();
			foreach (var so in _selectedObjects) if (isValidEvent(so)) _eventCopyBuffer.Add(new AbstractEvent(_events[so.Index]));

			serializeEventClipboard();
		}

		/// <summary>Copies all information needed to duplicate the selected icons at their current visible frame.</summary>
		void copyIcons()
		{
			// Create a list of selected icons, with sorting to maintain order.
			SortedDictionary<int, int> selIcons = new SortedDictionary<int, int>();
			
			var selection = getSelectedEvents();
			if (selection.Count > 0)
			{
				// Copy only selected icons.
				foreach (var so in _selectedObjects)
				{
					if (!isValidEvent(so)) continue;

					var evt = _events[so.Index];
					if (evt.Event == AbstractEventType.XwaMoveIcon && !selIcons.ContainsKey(evt.Params[0])) selIcons.Add(evt.Params[0], evt.UID);
				}
			}
			else
			{
				// No items selected, copy all visible icons.
				for (int i = 0; i < _mapIcons.Length; i++)
					if (_mapIcons[i].Enabled && _mapIcons[i].LastMoveEventUid >= 0) selIcons.Add(i, _mapIcons[i].LastMoveEventUid);
			}

			if (selIcons.Count == 0) return;

			_eventCopyBuffer.Clear();
			
			// Keep them sorted by type, so the resulting event list is arranged neatly.
			List<AbstractEvent> evtIcon = new List<AbstractEvent>();
			List<AbstractEvent> evtMove = new List<AbstractEvent>();
			List<AbstractEvent> evtRotate = new List<AbstractEvent>();
			foreach (var item in selIcons)
			{
				short iconIndex = (short)item.Key;
				var elem = getShadowIconByUid(item.Value);
				if (elem == null) continue;

				evtIcon.Add(new AbstractEvent(0, AbstractEventType.XwaSetIcon, iconIndex, (short)elem.IconCraftType, (short)elem.Color));
				evtMove.Add(new AbstractEvent(0, AbstractEventType.XwaMoveIcon, iconIndex, (short)elem.X, (short)elem.Y));
				if (elem.IconRotation != 0) evtRotate.Add(new AbstractEvent(0, AbstractEventType.XwaRotateIcon, iconIndex, (short)elem.IconRotation));
			}

			// Merge up a list of all the events.
			_eventCopyBuffer.AddRange(evtIcon);
			_eventCopyBuffer.AddRange(evtMove);
			_eventCopyBuffer.AddRange(evtRotate);

			setTitleOverride($"Copied {selIcons.Count} icons.", 2);
			serializeEventClipboard();
		}

		void pasteEvents()
		{
			unserializeEventClipboard();
			if (_eventCopyBuffer.Count == 0) return;

			int baseTime = hsbTimer.Value;
			if (isCurrentTab(MainTabIndex.EventList))
			{
				if (lstEvents.SelectedIndex < 0) baseTime = 0;
				else baseTime = _events[lstEvents.SelectedIndex].Time;
			}

			beginUndoFrame(true);

			// Find the lowest time, which will be used to find the time offset when creating the pasted events.
			short lowestTime = 9999;
			foreach (var evt in _eventCopyBuffer) if (evt.Time < lowestTime) lowestTime = evt.Time;

			List<int> newUids = new List<int>();
			int refreshStart = 0;

			foreach (var evt in _eventCopyBuffer)
			{
				if (!hasAvailableSpace(evt.Event))
				{
					popupError("Not enough available event space for pasting.");
					break;
				}

				// Create a duplicate so it gets a new UID.
				AbstractEvent newEvent = new AbstractEvent(evt);
				newEvent.Time = (short)(baseTime + (newEvent.Time - lowestTime));
				int destIndex = getInsertionIndexForEvent(newEvent.Time, false);
				if (destIndex < refreshStart) refreshStart = destIndex;

				insertEvent(destIndex, newEvent, true);
				newUids.Add(newEvent.UID);
				lstEvents.Items.Add("");
			}

			if (newUids.Count > 0)
			{
				rebuildShadowIcons();
				rebuildBriefing();
				if (isCurrentTab(MainTabIndex.EventList)) refreshEventList(-1);

				// Select the newly created events.
				_selectedObjects.Clear();
				foreach (int uid in newUids) _selectedObjects.Add(new SelectedObject(SelectedType.Event, getEventIndexByUid(uid), uid));

				refreshFromNewSelection();
				refreshMap();
			}
		}
		#endregion region Copy and paste

		#region Playback and navigation
		bool isPlaybackActive => _playbackEnabled;
		
		/// <summary>Refreshes the time index label below the timeline.  Includes playback rate if fast forward is active.</summary>
		void refreshTime()
		{
			string s = "Time: " + getTimeString(hsbTimer.Value) + "  (Raw: " + hsbTimer.Value + ")";
			if (_playbackSpeed > 1) s += " (" + _playbackSpeed + "x)";

			lblTime.Text = s;
		}

		/// <summary>Begins or resumes playback of the briefing map from the currently selected time.</summary>
		void playBriefing(bool looping)
		{
			if (_tempEvent != AbstractEventType.None || _pathMode) return;

			_playbackEnabled = true;

			if (isXwa)
			{
				// For XWA, play the caption sound only if we're resuming at the exact time the caption was set.
				if (hsbTimer.Value == 0 || hsbTimer.Value == _lastPageTime)
					playCaptionSound(_panelStringIndex[PANEL_CAPTION]);

				#if DEBUG && XWAINTRO
				// There's an initial wipe effect, but only when entering the briefing for the first time.  The events
				// won't play while the effect is active, but the caption audio will, if it exists.  The end timing of
				// the audio depends on this effect.  Playing without the effect gives the "true" time for the caption.
				// NOTE: If playing briefings simultaneously and checking if they're synced, this must be enabled.
				if (hsbTimer.Value == 0 && chkXwaEffects.Checked && !looping)
					beginMapEffect(MapEffect.Initializing_MapWipeIn, 26);
				#endif
			}

			_playbackLastSystemTime = Environment.TickCount;
			_playbackPrecisionTime = 0;
			_playbackFramesReady = 0;

			cmdPlay.Visible = false;
			cmdPause.Visible = true;
		}

		void pauseBriefing()
		{
			stopCaptionSound();
			beginMapEffect(MapEffect.None, 0);
			setPlaybackSpeed(1);
			refreshTime();
			_playbackEnabled = false;
			_playbackFramesReady = 0;
			cmdPlay.Visible = true;
			cmdPause.Visible = false;

			// If the user has chosen not to animate the timeline during playback, need to resume drawing.
			if (!chkTimelinePlayback.Checked) refreshTimeline();
		}
		
		void setPlaybackSpeed(int speed) => _playbackSpeed = clamp(speed, 1, 8);

		Point getDefaultZoom()
		{
			int result = ((isTie || isXwing) ? 16 : 32);
			// NOTE: The actual zoom in XW98, for the sake of the bug, is 16.  To render properly in the map here it needs to be half.
			if (isXwing && isXwingHighDef()) result = 8;
			return new Point(result, result);
		}

		/// <summary>Logically resets the briefing to its default initial state (zoom, position, all elements empty or disabled).</summary>
		/// <remarks>This does not change the current time, or run any events.</remarks>
		void resetEventStates()
		{
			_currentZoom = getDefaultZoom();
			_destinationZoom = _currentZoom;

			_currentPosition = new Point(0, 0);
			_destinationPosition = _currentPosition;

			for (int i = 0; i < 4; i++)
			{
				_panelStringEnabled[i] = false;
				_panelStringIndex[i] = -1;
				_panelStringEventUid[i] = 0;
			}

			for (int i = 0; i < _mapFgTags.Length; i++) _mapFgTags[i].Enabled = false;

			for (int i = 0; i < _mapTextTags.Length; i++)
			{
				_mapTextTags[i].Enabled = false;
				_mapTextTags[i].ClearTextTagCache(false);
			}

			_mapShipInfoTag.Enabled = false;

			foreach (MapElement elem in _mapIcons)
			{
				elem.X = 0;
				elem.Y = 0;
				elem.Enabled = false;
				elem.IconCraftType = 0;
				elem.Color = 0;
				elem.IconRotation = 0;
				elem.LastMoveEventIndex = -1;
				elem.LastMoveEventUid = -1;
			}

			_currentPage = 0;

			// Other frontend things that directly relate to the state of playback.
			_currentRegionPage = 0;
			_regionPageTime = -1;
			_regionTimeAdvanced = false;
			_lastPageTime = -1;
			_previousRenderTime = -1;
			_currentRegionIndex = 0;
			_pendingRegionIndex = 0;
			_activeShipInfoBlock = false;
			_activeShipInfoIcon = -1;
			_shipInfoStringIndex = -1;
			_shipInfoStringUid = -1;
			changeRegionIcons(-1);
		}

		/// <summary>Completely resets the briefing and current time back to the beginning.</summary>
		void resetBriefing()
		{
			pauseBriefing();
			deselectObjects();
			resetEventStates();
			jumpToTime(0, true);
			refreshMap();
		}

		/// <summary>Runs one frame of animation for the map, incrementing all effect or animation timers.</summary>
		/// <param name="timeIndex">The time of this frame, only needed for region transitions.</param>
		void animTick(int timeIndex)
		{
			if (_queuedMapEffect != MapEffect.None && _queuedMapEffectDelay > 0)
			{
				_queuedMapEffectDelay--;
				if (_queuedMapEffectDelay == 0)
				{
					beginMapEffect(_queuedMapEffect, _queuedMapEffectDuration);
					_queuedMapEffect = MapEffect.None;
				}
			}

			if (_regionPageTime == timeIndex)
			{
				// The region has changed, update the map accordingly.
				_currentRegionPage++;
				changeRegionIcons(_pendingRegionIndex);

				// Clear all tags, otherwise they'll remain highlighted/visible when changing regions.
				foreach (MapElement elem in _mapFgTags)   elem.Enabled = false;
				foreach (MapElement elem in _mapTextTags) elem.Enabled = false;
				_mapShipInfoTag.Enabled = false;
			}

			// Increment tag times.
			foreach (MapElement elem in _mapFgTags)   { if (elem.Enabled) elem.ElapsedTime++; }
			foreach (MapElement elem in _mapTextTags) { if (elem.Enabled) elem.ElapsedTime++; }
			if (_mapShipInfoTag.Enabled) _mapShipInfoTag.ElapsedTime++;

			if (!chkAnimateMoveZoom.Checked)
			{
				_currentPosition = _destinationPosition;
				_currentZoom = _destinationZoom;
			}
		}

		/// <summary>Performs a single frame of animation for MoveMap or ZoomMap events.</summary>
		/// <remarks>This is a separate function so it can be part of the normal map anim, or a background calculation while editing these events.</remarks>
		void animMapMoveZoomTick()
		{
			Point curZoom = _currentZoom;
			Point destZoom = _destinationZoom;
			// In order for XW98 value changes to work correctly, it has to use the doubled zoom that
			// would be used in-game.  It will be converted back when done.  This is needed for
			// MAX17.BRF to work properly, which has extreme value changes for move and zoom.
			if (isXwing && isXwingHighDef())
			{
				curZoom = new Point(_currentZoom.X * 2, _currentZoom.Y * 2);
				destZoom = new Point(_destinationZoom.X * 2, _destinationZoom.Y * 2);
			}

			int zoomDiffX = Math.Abs(curZoom.X - destZoom.X);
			int zoomDiffY = Math.Abs(curZoom.Y - destZoom.Y);
			int zoomStep = 2;
			if (zoomDiffX >= 12 || zoomDiffY >= 12) zoomStep = 8;
			if (curZoom.X < 10) zoomStep = 1;
			curZoom.X = adjustValue(curZoom.X, destZoom.X, zoomStep);
			curZoom.Y = adjustValue(curZoom.Y, destZoom.Y, zoomStep);

			int zoomScale = 1;
			if (curZoom.X != 0) zoomScale = (256 / curZoom.X) + 1;

			int moveDiffX = Math.Abs(_currentPosition.X - _destinationPosition.X);
			int moveDiffY = Math.Abs(_currentPosition.Y - _destinationPosition.Y);
			int scaledMoveDiff = Math.Max(moveDiffX, moveDiffY) / zoomScale;
			int moveStep = 2 * zoomScale;
			if (scaledMoveDiff >= 16) moveStep *= 2;

			_currentPosition.X = adjustValue(_currentPosition.X, _destinationPosition.X, moveStep);
			_currentPosition.Y = adjustValue(_currentPosition.Y, _destinationPosition.Y, moveStep);

			// Scale back down and clamp minimum to 1.
			if (isXwing && isXwingHighDef()) curZoom = new Point(Math.Max(1, curZoom.X / 2), Math.Max(1, curZoom.Y / 2));

			_currentZoom = curZoom;
		}

		/// <summary>Gravitates a Zoom or Move value as it makes one step toward its target value.</summary>
		/// <returns>The new value after adjustment, or the current value if unchanged.</returns>
		int adjustValue(int current, int target, int step)
		{
			int result = current;
			if (target < current)
			{
				result = current - step;
				if (result < target) result = target;
			}
			if (target > current)
			{
				result = current + step;
				if (result > target) result = target;
			}
			return result;
		}

		/// <summary>Runs an entire briefing frame, including animations and events at that time.</summary>
		void runFrame(int timeIndex)
		{
			mapEffectTick();
			animTick(timeIndex);
			runEventsAtTime(timeIndex, false);
			// If there's a map move/zoom encountered on the first frame of a new region, we need to suppress
			// the animation until the following frame.  See page #3 in B3M1.  After switching to region #2, 
			// The rebel icons must not be visible on the first frame, even though there's a zoom event.
			if (!isXwa || _regionPageTime == -1) animMapMoveZoomTick();  // Move and Zoom activate and animate on the same frame.

			if (timeIndex == _regionPageTime) _regionPageTime = -1;

			refreshMap();
		}

		/// <summary>Rebuilds the entire briefing up to the current time.</summary>
		/// <remarks>Necessary after any modification to events that would alter its current visual state.</remarks>
		void rebuildBriefing()
		{
			Point curPos = _currentPosition;
			Point curZoom = _currentZoom;

			beginMapEffect(MapEffect.None, 0);
			resetEventStates();
			runEventsAtTime(0, true);
			for (int time = 1; time <= hsbTimer.Value; time++) runFrame(time);

			// If in free look mode, attempt to preserve the former viewing position before the reset was applied.
			if (_freeLookMode)
			{
				_backupPosition = _currentPosition;
				_backupZoom = _currentZoom;

				_currentPosition = curPos;
				_currentZoom = curZoom;
			}

			_highestEventTime = getHighestTime();
			refreshShadowIconDropdown();
			refreshMap();
		}

		/// <summary>Manages all XWA icons during a region change.</summary>
		/// <param name="newRegion">Zero-based index of region, or -1 to reset.</param>
		/// <remarks>Icons states are saved per region.  Switching to a prior region must restore those states.</remarks>
		void changeRegionIcons(int newRegion)
		{
			if (newRegion < 0)
			{
				// Reset cache.
				if (_mapIconCache != null)
					for (int i = 0; i < _mapIconCache.Length; i++) _mapIconCache[i].Clear();
				return;
			}

			if (_currentRegionIndex == newRegion || newRegion >= 4) return;

			if (_mapIconCache == null)
			{
				_mapIconCache = new List<MapElement>[4];
				for (int i = 0; i < 4; i++) _mapIconCache[i] = new List<MapElement>();
			}

			// Backup the icons in the current region, transferring the object to a different list.
			_mapIconCache[_currentRegionIndex].Clear();
			for (int i = 0; i < _mapIcons.Length; i++)
			{
				if (!_mapIcons[i].Enabled) continue;

				_mapIconCache[_currentRegionIndex].Add(_mapIcons[i]);
				_mapIcons[i] = new MapElement();
			}

			// Reset all icon properties.
			foreach (MapElement elem in _mapIcons)
			{
				elem.Enabled = false;
				elem.X = 0;
				elem.Y = 0;
				elem.IconRotation = 0;
				elem.IconCraftType = 0;
				elem.Color = 0;
			}

			// Load any icons from the new region, if they exist.
			foreach (MapElement elem in _mapIconCache[newRegion]) _mapIcons[elem.DataIndex] = elem;

			// Finished loading the state.  We don't need these cache entries anymore.
			_mapIconCache[newRegion].Clear();

			_currentRegionIndex = newRegion;
			refreshShadowIconDropdown();
		}

		/// <summary>Sets a new briefing time, and performs a full refresh of all event and visual states.</summary>
		void jumpToTime(int timeIndex, bool setSlider)
		{
			// It's possible for keyboard commands to change the briefing position when the UI
			// would otherwise prevent a change in time.  Make sure to exit any temp event.
			if (_tempEvent != AbstractEventType.None) endTempEvent(false);

			setFreeLookMode(false);
			pauseBriefing();

			timeIndex = clamp(timeIndex, 0, getMaximumScrollTime());

			if (setSlider) hsbTimer.Value = timeIndex;

			refreshTime();

			if (timeIndex < _previousRenderTime || timeIndex == 0)
			{
				rebuildBriefing();
				refreshCanvas(RefreshFlags.All);
			}
			else if (timeIndex > _previousRenderTime)
			{
				for (int time = _previousRenderTime + 1; time <= hsbTimer.Value; time++) runFrame(time);
			}

			refreshMap();
			refreshTimeline();
		}

		/// <summary>Processes all events at the specified time.  Does not include animations.</summary>
		void runEventsAtTime(int timeIndex, bool force)
		{
			for (int i = 0; i < _events.Count; i++)
				if (_events[i].Time == timeIndex) runEvent(i, force);
			_previousRenderTime = timeIndex;
		}

		void runEvent(int eventIndex, bool force)
		{
			AbstractEvent evt = _events[eventIndex];

			int index;
			switch (evt.Event)
			{
				case AbstractEventType.PageBreak:
					for (int i = 0; i < 4; i++) _panelStringEnabled[i] = false;
					refreshCanvas(RefreshFlags.TextPanels);
					break;

				case AbstractEventType.TitleText:      //  Param [0] = string index
					// XWING doesn't change strings unless it's cleared first.
					if (isXwing && _panelStringEnabled[PANEL_TITLE]) break;

					_panelStringEnabled[PANEL_TITLE] = true;
					_panelStringIndex[PANEL_TITLE] = evt.Params[0];
					_panelStringEventUid[PANEL_TITLE] = evt.UID;
					refreshCanvas(RefreshFlags.Title);
					break;
					
				case AbstractEventType.CaptionText:    //  Param [0] = string index
					if (isXwing && _panelStringEnabled[PANEL_CAPTION]) break;

					// Check if the time is different before changing page, so that simple refreshes don't increment the page.
					if (evt.Time != _lastPageTime) _lastPageTime = evt.Time;

					// Page number and audio are only triggered if the string changes.
					if (_panelStringIndex[PANEL_CAPTION] != evt.Params[0])
					{
						_currentPage++;
						if (isXwa && _playbackEnabled) playCaptionSound(evt.Params[0]);
					}
					_panelStringEnabled[PANEL_CAPTION] = true;
					_panelStringIndex[PANEL_CAPTION] = evt.Params[0];
					_panelStringEventUid[PANEL_CAPTION] = evt.UID;
					refreshCanvas(RefreshFlags.Caption);
					break;

				case AbstractEventType.PanelText3:     //  Param [0] = string index
					if (!_panelStringEnabled[PANEL_XWING3])
					{
						_panelStringEnabled[PANEL_XWING3] = true;
						_panelStringIndex[PANEL_XWING3] = evt.Params[0];
						_panelStringEventUid[PANEL_XWING3] = evt.UID;
						refreshCanvas(RefreshFlags.Panel3);
					}
					break;

				case AbstractEventType.PanelText4:     //  Param [0] = string index
					if (!_panelStringEnabled[PANEL_XWING4])
					{
						_panelStringEnabled[PANEL_XWING4] = true;
						_panelStringIndex[PANEL_XWING4] = evt.Params[0];
						_panelStringEventUid[PANEL_XWING4] = evt.UID;
						refreshCanvas(RefreshFlags.Panel4);
					}
					break;

				case AbstractEventType.MoveMap:          // Param [0] = X, [1] = Y
					_destinationPosition = new Point(evt.Params[0], evt.Params[1]);
					if (evt.Time == 0 || force || !chkAnimateMoveZoom.Checked) _currentPosition = _destinationPosition;
					break;

				case AbstractEventType.ZoomMap:          // Param [0] = X, [1] = Y
					bool instant = (evt.Time == 0 || force);
					if (!instant && isXwing && isXwingHighDef())
					{
						// Bug in XW98.  The zoom is internally doubled from the param value.  Ignores the event
						// if the destination value is the same, but doesn't consider the doubled amount.
						// See the end of X-Wing Historical 6 (KEYAN.BRF), also T1M1 (DEFECT.BRF)
						if (_destinationZoom.X * 2 == evt.Params[0] && _destinationZoom.Y * 2 == evt.Params[1]) break;
					}
					_destinationZoom = new Point(evt.Params[0], evt.Params[1]);
					if (instant || !chkAnimateMoveZoom.Checked) _currentZoom = _destinationZoom;
					break;

				case AbstractEventType.ClearFgTags:
					foreach (MapElement elem in _mapFgTags) elem.Enabled = false;
					break;

				case AbstractEventType.FgTag1:          // Param [0] = fgIndex (or icon index for XWA)
				case AbstractEventType.FgTag2:
				case AbstractEventType.FgTag3:
				case AbstractEventType.FgTag4:
				case AbstractEventType.FgTag5:
				case AbstractEventType.FgTag6:
				case AbstractEventType.FgTag7:
				case AbstractEventType.FgTag8:
					index = evt.Event - AbstractEventType.FgTag1;
					_mapFgTags[index].DataIndex = evt.Params[0];
					_mapFgTags[index].Enabled = true;
					_mapFgTags[index].ElapsedTime = (force ? 80 : 0);
					_mapFgTags[index].EventIndex = eventIndex;
					_mapFgTags[index].EventUid = evt.UID;
					break;

				case AbstractEventType.ClearTextTags:
					foreach (MapElement elem in _mapTextTags) elem.Enabled = false;
					break;

				case AbstractEventType.TextTag1:           // Param [0] = tagIndex, [1] = X, [2] = Y, [3] = Color
				case AbstractEventType.TextTag2:
				case AbstractEventType.TextTag3:
				case AbstractEventType.TextTag4:
				case AbstractEventType.TextTag5:
				case AbstractEventType.TextTag6:
				case AbstractEventType.TextTag7:
				case AbstractEventType.TextTag8:
					index = evt.Event - AbstractEventType.TextTag1;
					// XWING requires a clear text command in order to reuse a text tag.
					if (isXwing && _mapTextTags[index].Enabled) break;

					_mapTextTags[index].ClearTextTagCache(false);
					_mapTextTags[index].DataIndex = evt.Params[0];
					_mapTextTags[index].X = evt.Params[1];
					_mapTextTags[index].Y = evt.Params[2];
					_mapTextTags[index].Color = evt.Params[3];
					_mapTextTags[index].Enabled = true;
					_mapTextTags[index].ElapsedTime = ((!chkAnimateTags.Checked || force) ? 80 : 0);
					_mapTextTags[index].EventIndex = eventIndex;
					_mapTextTags[index].EventUid = evt.UID;
					break;

				case AbstractEventType.XwaSetIcon:         // Param [0] = iconIndex, [1] = craftType, [2] = iffcolor
					index = evt.Params[0];
					if (index >= 0 && index < _mapIcons.Length)
					{
						// Craft type zero disables an icon.
						// Note: this event does not reset rotation.  See B6M1 page 2 SHU icon.
						_mapIcons[index].Enabled = (evt.Params[1] != 0);
						_mapIcons[index].DataIndex = index;
						_mapIcons[index].IconCraftType = evt.Params[1];
						_mapIcons[index].Color = evt.Params[2];
						_mapIcons[index].EventIndex = eventIndex;
						_mapIcons[index].EventUid = evt.UID;
					}
					break;

				case AbstractEventType.XwaMoveIcon:
					index = evt.Params[0];
					if (index >= 0 && index < _mapIcons.Length)
					{
						_mapIcons[index].X = evt.Params[1];
						_mapIcons[index].Y = evt.Params[2];
						_mapIcons[index].LastMoveEventIndex = eventIndex;
						_mapIcons[index].LastMoveEventUid = evt.UID;
					}
					break;

				case AbstractEventType.XwaRotateIcon:
					index = evt.Params[0];
					if (index >= 0 && index < _mapIcons.Length)
					{
						_mapIcons[index].IconRotation = evt.Params[1];
						_mapIcons[index].LastRotateEventIndex = eventIndex;
					}
					break;

				case AbstractEventType.XwaShipInfo:  // Param [0] = State, Param [1] = iconIndex
					// Only play the effects if the briefing is running.
					if (evt.Params[0] == 0)
					{
						// End state.  If effects are playing, retain the active ship until the effect finishes.
						// Otherwise end it.
						_mapShipInfoTag.Enabled = false;
						_activeShipInfoBlock = false;
						// The duration was 6, but there's a slight delay in game that needs to be accounted for.
						if (isPlaybackActive && chkXwaEffects.Checked) beginMapEffect(MapEffect.ShipInfoD_IconShrink, 6);
						else _activeShipInfoIcon = -1;
					}
					else
					{
						// Begin state.
						_activeShipInfoBlock = true;
						_activeShipInfoIcon = 0;
						_mapEffectString = "Ship Info: Unknown Craft";
							
						// The tag animation needs to finish playing before the effect truly begins.  Important for timing.
						if (isPlaybackActive && chkXwaEffects.Checked) queueMapEffect(12, MapEffect.ShipInfoA_IconGrow, 5);

						index = evt.Params[1];

						// This event has the same effect as activating a flightgroup tag, but it has its own unique slot.
						_mapShipInfoTag.DataIndex = index;
						_mapShipInfoTag.Enabled = true;
						_mapShipInfoTag.ElapsedTime = 0;
						_mapShipInfoTag.EventIndex = eventIndex;
						_mapShipInfoTag.EventUid = evt.UID;

						if (index >= 0 && index < _mapIcons.Length)
						{
							_activeShipInfoIcon = index;
							int craftType = _mapIcons[index].IconCraftType;
							if (craftType == 0) _mapEffectString = "Ship Info: No craft assigned for Icon #" + (index + 1);
							else if (craftType >= 1 && craftType < _craftNames.Length) _mapEffectString = "Ship Info: " + _craftNames[craftType];
						}
					}
					break;

				case AbstractEventType.XwaChangeRegion:  // Param [0] = region index
					if (evt.Time == 0)
					{
						// Avoid displaying anything, see start of B3M1.
						_currentRegionIndex = evt.Params[0];
						break;
					}

					// The page will increment on the next time tick, which is usually when the new icons are established.
					// If the user is paused on this exact time, they shouldn't see the new shadow icons yet.
					_regionPageTime = evt.Time + 1;
					_pendingRegionIndex = evt.Params[0];
					_mapEffectString = getRegionName(evt.Params[0], false);

					// Only play the effects if the briefing is running.
					if (isPlaybackActive && chkXwaEffects.Checked)
					{
						_regionTimeAdvanced = false;
						beginMapEffect(MapEffect.RegionA_MapWipeOut, 25);
					}
					break;

				case AbstractEventType.XwaInfoParagraph:
					// One-based string number.  If zero, the string is hidden.
					index = evt.Params[0] - 1;
					if (index < 0 || index >= _strings.Count) index = -1;
					_shipInfoStringIndex = index;
					_shipInfoStringUid = evt.UID;
					break;
			}
		}

		void checkReadyFrames()
		{
			// At one point in development a multithreaded timer was used, but the accuracy wasn't any better.
			int pendingFrames = _playbackFramesReady;
			if (pendingFrames == 0) return;

			_playbackFramesReady = 0;

			if (hsbTimer.Value == _briefingDuration)
			{
				// Reached the assigned briefing duration.  Loop with current playback speed, or stop.
				if (chkLoopPlayback.Checked)
				{
					int speed = _playbackSpeed;
					jumpToTime(0, true);
					playBriefing(true);
					setPlaybackSpeed(speed);
				}
				else pauseBriefing();
				return;
			}

			int step = _playbackSpeed;
			if (step == 1)
			{
				// On 1x playback speed, we may still need to catch up to the elapsed time.
				if (step < pendingFrames) step = pendingFrames;
			}
			else
			{
				// We're on 2x playback or faster.
				step *= pendingFrames;
			}
				
			// Don't overshoot the maximum.
			int startTime = hsbTimer.Value;
			if (startTime + step > _briefingDuration) step = _briefingDuration - startTime;

			// During certain effects, the map won't tick until the effect finishes.
			// Keep track of how many map ticks have occurred, so the time can be adjusted accordingly.
			int mapTicks = 0;

			for (int i = 0; i < step; i++)
			{
				bool runEvent = true;

				// The initial map entrance, or a region transition halts the playback until the effect finishes.
				// ShipInfo transitions don't halt the map.
				if (_mapEffect == MapEffect.Initializing_MapWipeIn || (_mapEffect >= MapEffect.RegionA_MapWipeOut && _mapEffect <= MapEffect.RegionD_MapWipeIn))
				{
					if (_mapEffect == MapEffect.RegionC_TextFadeOut && !_regionTimeAdvanced && (_mapEffectCurrentFrame == _mapEffectMaxFrame - 1))
					{
						// For a single frame of a region transition, there is a special case where a map tick needs
						// to be played.  This allows the icons to be updated before the map is visible again.
						// Also updates the page so the shadow icons will match.
						_regionTimeAdvanced = true;
					}
					else
					{
						mapEffectTick();
						runEvent = false;
					}
				}

				if (runEvent)
				{
					mapTicks++;
					runFrame(startTime + mapTicks);
				}
			}
			hsbTimer.Value += mapTicks;

			if (pctTimeline.Focused) refreshTimelineOffset();
			refreshMap();
		}

		/// <summary>A queued effect allows the map to tick normally for a delay before the real effect begins.</summary>
		void queueMapEffect(int delay, MapEffect effect, int duration)
		{
			_queuedMapEffectDelay = delay;
			_queuedMapEffect = effect;
			_queuedMapEffectDuration = duration;
		}

		/// <summary>Begins a transition or visual effect for XWA.</summary>
		/// <remarks>Also can be used to clear the effect by setting back to none.</remarks>
		void beginMapEffect(MapEffect effect, int duration)
		{
			// There seems to be a slight timing issue with the effects, playing side by side they
			// don't fully sync to video footage.  The time passed to the effect duration here is
			// usually +1 over the observed frame time.
			_mapEffect = effect;
			_mapEffectCurrentFrame = 0;
			_mapEffectMaxFrame = duration;

			// Refresh the map when starting or ending the sequence.
			switch (effect)
			{
				case MapEffect.Initializing_MapWipeIn:
				case MapEffect.RegionA_MapWipeOut:
				case MapEffect.ShipInfoA_IconGrow:
				case MapEffect.ShipInfoD_IconShrink:
				case MapEffect.None:
					refreshMap();
					break;
			}
		}
		
		/// <summary>Runs a single tick of map effect logic for timing purposes.</summary>
		/// <remarks>Not responsible for drawing, but does trigger a render refresh.</remarks>
		void mapEffectTick()
		{
			if (_mapEffect == MapEffect.None) return;

			if (++_mapEffectCurrentFrame >= _mapEffectMaxFrame)
			{
				// The effect duration has expired, determine what to do next.
				switch (_mapEffect)
				{
					case MapEffect.Initializing_MapWipeIn:
						beginMapEffect(MapEffect.None, 0);
						break;

					case MapEffect.RegionA_MapWipeOut:
						beginMapEffect(MapEffect.RegionB_TextFadeIn, 26);
						break;

					case MapEffect.RegionB_TextFadeIn:
						// Total visible text time appears to be 48, possibly 50 for frames where text isn't drawn.
						beginMapEffect(MapEffect.RegionC_TextFadeOut, 25);
						break;

					case MapEffect.RegionC_TextFadeOut:
						beginMapEffect(MapEffect.RegionD_MapWipeIn, 25);
						break;

					case MapEffect.RegionD_MapWipeIn:
						beginMapEffect(MapEffect.None, 0);
						break;

					case MapEffect.ShipInfoA_IconGrow:
						beginMapEffect(MapEffect.ShipInfoB_ScreenSweep, 34); // 33
						break;

					case MapEffect.ShipInfoB_ScreenSweep:
						// This effect lasts until disabled, so begin with the maximum time.
						beginMapEffect(MapEffect.ShipInfoC_MeshRotate, hsbTimer.Maximum - hsbTimer.Value);
						break;

					case MapEffect.ShipInfoC_MeshRotate:
						// Nothing, waiting for the corresponding ShipInfo event to disable the state.
						break;

					case MapEffect.ShipInfoD_IconShrink:
						beginMapEffect(MapEffect.None, 0);
						_activeShipInfoIcon = -1;
						break;
				}
			}
			refreshCanvas(RefreshFlags.Effect);
		}
		
		void gotoSelectedEvent()
		{
			int time = getEarliestSelectedTime();
			if (time >= 0)
			{
				jumpToTime(time, true);
				verticalCenterTimeline(-1);
			}

			if (!_pathMode) return;

			// If we're in path mode, select this icon as the working icon.
			foreach (var so in _selectedObjects)
			{
				if (!isValidEvent(so)) continue;

				var evt = _events[so.Index];
				if (evt.Event != AbstractEventType.XwaMoveIcon) continue;

				_lastSelectedIconIndex = evt.Params[0];
				_lastSelectedMoveIconUid = evt.UID;
				break;
			}
		}

		bool gotoNextPage(int curTime, int direction)
		{
			int time = findNextPageTime(curTime, direction);
			if (time > getMaximumScrollTime()) time = getMaximumScrollTime();

			jumpToTime(time, true);
			return (time != curTime);
		}

		void gotoPage(int pageNumber)
		{
			int time = -1;
			int page = 0;
			foreach (AbstractEvent evt in _events)
			{
				if (evt.Event != AbstractEventType.CaptionText && evt.Event != AbstractEventType.PageBreak) continue;

				// If multiple events are on the same time, only increment the page once
				if (evt.Time == time) continue;

				time = evt.Time;
				if (++page == pageNumber)
				{
					jumpToTime(time, true);
					return;
				}
			}
		}

		/// <summary>Performs a navigation jump based on what's selected or currently active in the sidebar.</summary>
		/// <remarks>MoveIcon or RotateIcon will jump to the original icon definition.  Craft info will jump between the first and last event./remarks>
		/// <param name="fromEventUid">UID of a currently selected item.  If not specified (-1), will search for a selected item.</param>
		void gotoContextEvent(int fromEventUid)
		{
			int fromIndex = -1;
			if (fromEventUid == -1)
			{
				List<SelectedObject> evtList = getSelectedEvents();
				if (evtList.Count > 0) fromIndex = evtList[0].Index;
			}
			else fromIndex = getEventIndexByUid(fromEventUid);
			
			if (fromIndex == -1) return;

			AbstractEventType fromType = _events[fromIndex].Event;

			// Find which region the select event resides in.  ShipInfo restricts to a specific block.
			// Icons are preserved across region changes, so an old definition may not reside in the current region block.
			int fromRegion = 0;
			int regionStart = 0;
			int regionEnd = _events.Count - 1;
			bool restrictRegion = (fromType == AbstractEventType.XwaShipInfo);

			for (int i = 0; i < _events.Count; i++)
			{
				AbstractEvent evt = _events[i];
				if (evt.Event != AbstractEventType.XwaChangeRegion) continue;

				if (evt.Time > _events[fromIndex].Time)
				{
					regionEnd = i;
					break;
				}
				else
				{
					if (restrictRegion) regionStart = i;
					fromRegion = evt.Params[0];
				}
			}

			// Now search through the event range to find the correct target.
			int region = 0;
			int targetIndex = -1;
			for (int i = regionStart; i <= regionEnd; i++)
			{
				AbstractEvent evt = _events[i];
				if (evt.Event == AbstractEventType.XwaChangeRegion) region = evt.Params[0];

				if (region != fromRegion) continue;

				if (evt.Event == AbstractEventType.XwaSetIcon)
				{
					if (fromType == AbstractEventType.XwaMoveIcon || fromType == AbstractEventType.XwaRotateIcon || (fromType >= AbstractEventType.FgTag1 && fromType <= AbstractEventType.FgTag8))
						if (evt.Params[0] == _events[fromIndex].Params[0]) targetIndex = i;
				}
				else if (evt.Event == AbstractEventType.XwaShipInfo)
				{
					if (fromType == AbstractEventType.XwaShipInfo && evt.Params[1] == _events[fromIndex].Params[1])
						if (evt.Params[0] != _events[fromIndex].Params[0]) targetIndex = i;
				}
			}

			if (targetIndex >= 0)
			{
				jumpToTime(_events[targetIndex].Time, true);
				verticalCenterTimeline(targetIndex);

				_selectedObjects.Clear();
				_selectedObjects.Add(new SelectedObject(SelectedType.Event, targetIndex, _events[targetIndex].UID));
				refreshFromNewSelection();
			}
			else popupError("Could not find a corresponding event to navigate to.");
		}

		void gotoNextTimelinePage(int direction, bool large)
		{
			int step = ((pctTimeline.Width / _timelineColumnWidth) - 1) / 2;
			if (large) step = (step - 1) * 2;

			jumpToTime(hsbTimer.Value + (step * direction), true);
		}

		void gotoNextEventTime(int curTime, int direction)
		{
			int lowTime = 0;
			int highTime = 9999;
			foreach (var evt in _events)
			{
				if (evt.Time < curTime && evt.Time > lowTime) lowTime = evt.Time;
				if (evt.Time > curTime && evt.Time < highTime) highTime = evt.Time;
			}

			if (highTime == 9999) highTime = curTime;

			int targetTime = (direction > 0 ? highTime : lowTime);
			jumpToTime(targetTime, true);
		}

		int findNextPageTime(int curTime, int direction)
		{
			int prevTime = 0;
			int nextTime = 9999;
			int lastTime = 0;
			foreach (var evt in _events)
			{
				lastTime = evt.Time;
				if (evt.Event == AbstractEventType.PageBreak || evt.Event == AbstractEventType.CaptionText)
				{
					if (lastTime < curTime && lastTime > prevTime) prevTime = lastTime;
					if (lastTime > curTime && lastTime < nextTime) nextTime = lastTime;
				}
			}

			if (nextTime == 9999) nextTime = lastTime;

			return (direction >= 0 ? nextTime : prevTime);
		}

		void scrollTimeline()
		{
			int scrollWidth = (int)(_timelineColumnWidth * 0.8);
			int stepX = (_mouse1.X - _mouse2.X) / scrollWidth;
			if (stepX != 0)
			{
				int newTime = clamp(hsbTimer.Value + stepX, 0, getMaximumScrollTime());
				jumpToTime(newTime, true);
				refreshMap();
				refreshTimeline();

				// Update position for new delta.
				_mouse1.X -= stepX * scrollWidth;
			}

			int stepY = (_mouse2.Y - _mouse1.Y) / _timelineFont.Height;
			if (stepY != 0)
			{
				int newPos = _timelineRowScrollPos + stepY;
				if (newPos + _timelineVisibleRowCount > _timelineHighestItemCount) newPos = _timelineHighestItemCount - _timelineVisibleRowCount;
				if (newPos < 0) newPos = 0;
				_timelineRowScrollPos = newPos;
				refreshTimeline();

				// Update position for new delta.
				_mouse1.Y += stepY * _timelineFont.Height / 2;
			}
		}

		/// <summary>Adjusts the vertical scroll position of the timeline to center on a selected item.</summary>
		/// <param name="eventIndex">An existing event index to center on.  Specify -1 to detect the earliest selected item.</param>
		void verticalCenterTimeline(int eventIndex)
		{
			if (eventIndex < 0)
			{
				// Try to select the earliest selected item.
				var sel = getSelectedEvents();
				if (sel.Count > 0)
				{
					sel.Sort(SelectedObject.Compare);
					eventIndex = sel[0].Index;
				}
			}

			if (eventIndex < 0) return;

			// Now determine where the selection resides in the column.
			// If it's not already visible, try to center the vertical position over it, with a slight bias above the middle.
			var evtIndices = enumEventsAtTime(_events[eventIndex].Time);
			if (evtIndices.Count > 0)
			{
				int lowestIndex = evtIndices[0];
				if (eventIndex < _timelineRowScrollPos || eventIndex >= _timelineRowScrollPos + _timelineVisibleRowCount)
				{
					int pos = (eventIndex - lowestIndex) - ((_timelineVisibleRowCount - 1) / 2);
					if (pos < 0) pos = 0;
						
					_timelineRowScrollPos = pos;
					refreshTimeline();
				}
			}
		}
		#endregion Playback and navigation

		#region Misc unsorted
		bool isXwing => (_platform == Settings.Platform.XWING);
		bool isTie => (_platform == Settings.Platform.TIE);
		bool isXvt => (_platform == Settings.Platform.XvT);
		bool isXwa => (_platform == Settings.Platform.XWA);

		/// <summary>Determines whether the current platform displays the title text from an event.</summary>
		bool isTitlePlatform => (isXwing || isTie);
		
		/// <summary>Checks if the platform requires flightgroup waypoint Y-axis to be flipped.</summary>
		/// <remarks>In some platforms, the waypoint is flipped relative to the inflight map, but not for the briefing map.</remarks>
		bool isWaypointInvertedPlatform() => (isTie || isXvt);
		///<inheritdoc cref="isWaypointInvertedPlatform"/>
		bool isWaypointInvertedPlatform(Settings.Platform platform) => (platform == Settings.Platform.TIE || platform == Settings.Platform.XvT);

		/// <summary>Returns a Y-axis position with the sign flipped if the platform requires it.</summary>
		int getInvertedY(int y) => (isWaypointInvertedPlatform() ? -y : y);

		bool isTeamLabelVisible()
		{
			if (!isXvt && !isXwa) return false;

			if (_freeLookMode || _mapShiftMode || _pathMode || _tempEvent != AbstractEventType.None || _activeShipInfoBlock) return false;

			return chkTeamPlayerLabels.Checked;
		}

		/// <summary>For the XWING platform, resolves an IFF color for a craft type if the IFF is default.  For all other platforms, just returns the IFF.</summary>
		/// <param name="craftType">Craft type to check.</param>
		/// <param name="iff">Original IFF of the flightgroup.</param>
		/// <param name="platform">Platform to use for conversion.</param>
		/// <returns>A raw IFF value.  It is platform-dependent.</returns>
		int resolveIff(int craftType, int iff, Settings.Platform platform)
		{
			if (platform == Settings.Platform.XWING && iff == 0)
			{
				// Default IFF needs to resolve to a proper IFF for the correct color.
				// Adjust the IFF based on craft type before retrieving the color value.
				// Rebel: X-W,Y-W,A-W, CRS,FRG,B-W
				// Imperial: T/F,T/I,T/B,GUN, STD,T/A, mines, sat, etc.
				// Neutral:  TRN,SHU,TUG,CON,FRT, CRV.
				// NOTE: Default for TRN is Imperial in flight, but Neutral in briefing map.
				if ((craftType >= 1 && craftType <= 3) || craftType == 13 || craftType == 14 || craftType == 25) iff = 1;
				else if ((craftType >= 4 && craftType <= 7) || (craftType >= 16 && craftType <= 24)) iff = 2;
				else if ((craftType >= 8 && craftType <= 12) || craftType == 15) iff = 3;
			}
			return iff;
		}

		/// <summary>Checks if a craft type should be modified to access a different icon graphic when drawing an icon.</summary>
		/// <remarks>XWING and TIE have special considerations when choosing the icon.</remarks>
		/// <param name="craftType">The unmodified craft type.</param>
		/// <param name="iff">IFF code to use.  Only relevant for XWING.</param>
		int getAdjustedCraftType(int craftType, int iff)
		{
			if ((isXwing || isTie) && _smallIconOffset > 0)
			{
				int zoomX = _currentZoom.X;
				int zoomY = _currentZoom.Y;
				if (isXwing && isXwingHighDef())
				{
					zoomX *= 2;
					zoomY *= 2;
				}

				bool useSmallIcon = (zoomX < 32 || zoomY < 32);

				// For XWING, the default IFF must resolve into a proper IFF for color selection.
				if (isXwing)
				{
					iff = resolveIff(craftType, iff, _platform);

					// The green (IFF=1) icon set contains all objects in the game, not just craft.
					if (useSmallIcon)
					{
						if (craftType == 0 && iff >= 2)
							return craftType + 25;
					}
				}

				if (useSmallIcon) craftType += _smallIconOffset;
			}
			return craftType;
		}

		/// <summary>Determines if a shadow icon would be visible on the map, based on time, region page, and filter setting.  Assumes the element is enabled.</summary>
		bool isShadowIconVisible(MapElement element)
		{
			if (!chkIconShadows.Checked || element.IconTime == hsbTimer.Value || element.IconRegionPage != _currentRegionPage) return false;

			// Index 0 includes everything.
			// Index 1 is everything except null
			// Index 2+ only shows specific icons
			int filter = cboShadowFilter.SelectedIndex;
			if (filter == 1 && element.IconCraftType == 0) return false;
			if (filter >= 2 && ((filter - 2) != element.DataIndex)) return false;

			return true;
		}

		/// <summary>Retrieves the craft abbreviation from the specified type, or an empty string if invalid.</summary>
		string getCraftAbbrev(int craftType) => ((craftType >= 0 && craftType < _craftAbbrev.Length) ? _craftAbbrev[craftType] : "");

		void prepareReimport()
		{
			pauseBriefing();
			if (_tempEvent != AbstractEventType.None) endTempEvent(false);
			if (_selectedObjects.Count != 0) deselectObjects();
		}

		void refreshTextTags() { foreach (var elem in _mapTextTags) elem?.ClearTextTagCache(false); }

		void setFreeLookMode(bool state)
		{
			if (_freeLookMode == state) return;

			if (isXwing)
				// Don't allow free look mode if the page doesn't have a map.
				if (!getXwingPanel(Platform.Xwing.PageTemplate.Elements.Map).IsVisible) return;

			if (!state)
			{
				// Exiting mode
				_currentPosition = _backupPosition;
				_currentZoom = _backupZoom;
			}
			else
			{
				// Entering mode
				pauseBriefing();
				_freeLookRect = getRawMapRect(false);
				_backupPosition = _currentPosition;
				_backupZoom = _currentZoom;
			}
			_freeLookMode = state;
			refreshMap();
		}

		/// <summary>A wrapper around free look mode, only used when editing an existing MoveMap or ZoomMap event.</summary>
		void setShiftMode(bool state)
		{
			if (_mapShiftMode == state) return;

			if (state)
			{
				if (_freeLookMode) _freeLookBackup = new Point[] { _currentPosition, _currentZoom, _backupPosition, _backupZoom };
				else _freeLookBackup = null;

				setFreeLookMode(true);
			}
			else
			{
				if (_freeLookBackup != null)
				{
					_currentPosition = _freeLookBackup[0];
					_currentZoom = _freeLookBackup[1];
					_backupPosition = _freeLookBackup[2];
					_backupZoom = _freeLookBackup[3];
				}
				else setFreeLookMode(false);

				rebuildBriefing();
			}

			_mapShiftMode = state;
		}

		void beginMoveItems()
		{
			beginUndoFrame(false);
			_dragCarry = new Point();

			foreach (var so in _selectedObjects) so.UndoUID = 0;
		}

		/// <summary>Applies a rotation for an XWA Move icon by creating a Rotate event, or modifying the existing one.</summary>
		/// <param name="eventIndex">Must be a valid event index to a MoveIcon event.</param>
		/// <param name="exact">If true, uses a specific value.  If false, expects a directional step.</param>
		/// <param name="value">A specific value, or a directional step (-1 or 1).</param>
		void setRotationForMoveIcon(int eventIndex, bool exact, int value)
		{
			if (eventIndex < 0 || eventIndex >= _events.Count) return;

			var moveEvt = _events[eventIndex];
			if (moveEvt.Event != AbstractEventType.XwaMoveIcon) return;

			int iconIndex = moveEvt.Params[0];
			if (iconIndex < 0 || iconIndex >= _mapIcons.Length) return;

			// The shadow icon data stores its known rotation at that time.
			var shadow = getShadowIconByUid(moveEvt.UID);
			if (shadow == null) return;

			int newRotation = shadow.IconRotation;
			if (exact) newRotation = value;
			else if (newRotation < 4)
			{
				newRotation += value;
				if (newRotation >= 4) newRotation = 0;
				else if (newRotation < 0) newRotation = 3;
			}

			// Make sure the value is actually changing before proceeding.
			if (newRotation == shadow.IconRotation) return;

			var rotEvt = getIconEventAtTime(AbstractEventType.XwaRotateIcon, iconIndex, moveEvt.Time, out _);
			if (rotEvt == null)
			{
				// Not found, auto create an event.
				if (!hasAvailableSpace(AbstractEventType.XwaRotateIcon))
				{
					popupError("Not enough event space to create a new rotation event.");
					return;
				}
				rotEvt = new AbstractEvent(moveEvt.Time, AbstractEventType.XwaRotateIcon, (short)iconIndex, (short)newRotation);
				int rotIndex = getInsertionIndexForIconEvent(rotEvt.Time, AbstractEventType.XwaRotateIcon, iconIndex);
				insertEvent(rotIndex, rotEvt, true);
				relinkSelectedEventUid();
			}
			else
			{
				UndoOperation op = new UndoOperation(UndoType.Event, rotEvt.GetDataSnapshot(), 0);
				rotEvt.Params[1] = (short)newRotation;
				op.Update(rotEvt.GetDataSnapshot());
				addUpdatedUndoOperation(op, false);
			}

			rebuildShadowIcons();
			rebuildBriefing();
			refreshMap();
			refreshTimeline();
		}

		void popupError(string message) => MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		void popupWarning(string message) => MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		void popupInfo(string message) => MessageBox.Show(message, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
		bool popupConfirm(string message) => (MessageBox.Show(message, "Confirm action", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
		
		void refreshTimelineOffset()
		{
			string s = "";
			if (_mouseDragState != MouseDragState.TimelineDrag)
			{
				// Calculate the offset where the user is hovering the mouse, and update a label indicating the time.
				int relativeTicks = (int)Math.Round((double)(_mouse2.X - pctTimeline.Image.Width / 2) / _timelineColumnWidth);
				int absoluteTicks = hsbTimer.Value + relativeTicks;
				if (absoluteTicks < 0)
				{
					// Clamp to zero by adding the inverse of the overflow.
					relativeTicks += (-absoluteTicks);
					absoluteTicks = 0;
				}
				s = "Cursor: " + getTimeString(absoluteTicks);
				if (absoluteTicks <= _briefingDuration) s += $"  ({(relativeTicks >= 0 ? "+" : "-")}{getTimeString(Math.Abs(relativeTicks), true)}, {relativeTicks} raw)";
				else s += " beyond duration";
			}

			lblTimelineOffset.Text = s;
		}

		int clamp(int value, int min, int max)
		{
			if (value < min) value = min;
			else if (value > max) value = max;
			return value;
		}

		/// <summary>Retrieves the index of the currently visible flightgroup waypoint.</summary>
		int getFlightgroupWaypoint()
		{
			// For XWING, the waypoint is customizable for the current page, so retrieve the proper value.
			// For XvT and XWA, this pulls directly from the briefing index.  Same for TIE, but the index is always zero.
			int wp = _currentBriefingIndex;
			if (isXwing) wp = getXwingCoreBriefing().Pages[_currentBriefingIndex].Waypoint;
			return wp;
		}

		void displayHelp()
		{
			List<string> r = new List<string>();
			if (isCurrentTab(MainTabIndex.BriefingMap))
			{
				if (_panelMode)
				{
					r.Add("P or Esc - exit panel mode");
					r.Add("");
					r.Add("WASD - select edge of current panel");
					r.Add("Space - select interior of current panel");
					r.Add("");
					r.Add("Arrow Keys");
					r.Add("   If edge is selected, resizes panel");
					r.Add("   If interior is selected, moves panel");
					r.Add("");
					r.Add("Left click to select edge or interior, drag to resize or move");
				}
				else if (_pathMode)
				{
					r.Add("P or Esc - exit path mode");
					r.Add("Left Click - select and move nodes");
					r.Add("Shift + Left Click - create node at end");
					r.Add("Ctrl + Left Click - create node at front");
					r.Add("Delete - remove selected nodes");
					r.Add("1 to 9 - select node by number");
					r.Add("Brackets [ ] - rotation");
					r.Add("+/- adjust steps");
					r.Add("Backslash - rotate on first or second");
				}
				else
				{
					r.Add("Left Click - Select or move");
					r.Add("   Hold shift to add selection, control to toggle");
					r.Add("   Drag empty space for bounding box selection");
					r.Add("   Drag selected item to move");
					r.Add("Middle Click - Context reset");
					r.Add("Right Click - Panning");
					r.Add("Mousewheel - Zoom");
					r.Add("");
					r.Add("Ctrl+C - Copy");
					r.Add("Ctrl+X - Cut");
					r.Add("Ctrl+V - Paste");
					r.Add("Ctrl+Z - Undo");
					r.Add("Ctrl+Y - Redo");
					r.Add("");
					r.Add("Ctrl+K - Copy visible icons (XWA only)");
					r.Add("");
					r.Add("Esc - Context reset and cancel");
					r.Add("Del - Delete selected items");
					r.Add("I - Expand (I)cons");
					r.Add("G - Goto context event");
					r.Add("1 to 9 - Goto caption");
					r.Add("F5-F8 recall bookmark time (shift to set)");
					r.Add("Space - play/pause briefing");
					r.Add("F - accelerate playback");
					r.Add("PgUp/PgDn - Goto prev/next caption");
					r.Add("Home/End - Goto beginning or last event");
					r.Add("Ctrl+End - Goto exact end of briefing");
					r.Add("Enter - Center time on selected event");
					r.Add("E/R - Prev/next timeline page");
					r.Add("T/Y - Prev/next event column");
					r.Add("U - Select last edited event");
					r.Add("L - set briefing length to current time");
					r.Add("+/- Navigate time by 1 frame");
					r.Add("Backspace - Deactivate XWA icon");
					r.Add("");

					if (ActiveControl == pctTimeline)
					{
						r.Add("TIMELINE SPECIFIC");
						r.Add("Ctrl + Arrow Key - move selected events");
					}
					else
					{
						r.Add("MAP SPECIFIC");
						r.Add("Arrow keys - move selected items by increment");
						r.Add("   Hold shift for small increment, control for large");
						r.Add("");
						r.Add("1 to 9 (number row) - go to caption");
						r.Add("Numpad Arrows - Generate XWA icon movement");
						r.Add("Bracket [ ] - Rotate XWA icon");
						r.Add("D - Detect icon distance");
						r.Add("P - Toggle path mode (XWA) or panel mode (XWING)");
					}
				}
			}
			else if (isCurrentTab(MainTabIndex.MainStrings))
				r.Add("Enter, F2, or double click to edit selected string or tag");
			
			if (r.Count > 0) displayHelpReport(r);
		}
		#endregion Misc unsorted

		#region Interaction and unit conversion
		/// <summary>Converts a raw briefing coordinate to its pixel position on the map canvas.</summary>
		Point convertRawPosToPixelPos(int rawX, int rawY)
		{
			int scale = (isXwing && isXwingHighDef() ? 128 : 256);
			int x = (_currentZoom.X * (rawX - _currentPosition.X) / scale) + (_mapCanvas.Width / 2);
			int y = (_currentZoom.Y * (rawY - _currentPosition.Y) / scale) + (_mapCanvas.Height / 2);
			return new Point(x, y);
		}
		/// <inheritdoc cref="convertRawPosToPixelPos"/>
		Point convertRawPosToPixelPos(Point raw) => convertRawPosToPixelPos(raw.X, raw.Y);

		/// <summary>Converts a pixel position on the map canvas to a raw briefing position.</summary>
		Point convertPixelPosToRawPos(int pixelX, int pixelY)
		{
			float scale = (isXwing && isXwingHighDef() ? 128.0f : 256.0f);
			int x = (int)(((pixelX - (_mapCanvas.Width / 2)) * (scale / _currentZoom.X)) + _currentPosition.X);
			int y = (int)(((pixelY - (_mapCanvas.Height / 2)) * (scale / _currentZoom.Y)) + _currentPosition.Y);
			return new Point(x, y);
		}
		/// <inheritdoc cref="convertPixelPosToRawPos"/>
		Point convertPixelPosToRawPos(Point pixel) => convertPixelPosToRawPos(pixel.X, pixel.Y);

		/// <summary>Retrieves the current map scale.</summary>
		/// <remarks>XWING when running in low res mode has a built-in 2x multiplier that isn't reflected in the scale value but still requires special consideration.</remarks>
		float getMapScale()
		{
			float mapScale = _mapScale;
			if (isXwing && !isXwingHighDef()) mapScale *= 2;
			return mapScale;
		}

		/// <summary>Retrieves the pixel offset required to transform the physical mouse position on the map to its logical position.</summary>
		/// <remarks>Necessary when the map panel is configured so that the panel center is not aligned with the canvas center.</remarks>
		Point getXwingMapMouseOffset()
		{
			// At (0,0) map position, an item at (0,0) will always appear in the center of the map panel,
			// no matter how the panel's rectangle is configured.  Which could be smaller than, or moved
			// away from, the center of the canvas.
			// Determines the offset between the source (where it blits the map from) and the destination
			// where it blits to.
			var map = getXwingPanel(Platform.Xwing.PageTemplate.Elements.Map);
			var r = getXwingDestinationRect(map, isXwingHighDef() ? 2 : 1);
			return new Point(_xwingSourceViewport[4].X - r.X, _xwingSourceViewport[4].Top);
		}

		/// <summary>Converts a mouse position from the scaled map display to its original map canvas position.</summary>
		Point convertMousePosToPixelPos(Point mouse)
		{
			float mapScale = getMapScale();
			float x = mouse.X / mapScale;
			float y = (mouse.Y - _scaledMapRect.Top) / mapScale;
			Point ret = new Point((int)x, (int)y);
			if (isXwing) ret.Offset(getXwingMapMouseOffset());
			return ret;
		}

		/// <summary>Converts a mouse position from the scaled map display to a raw briefing position.</summary>
		Point convertMousePosToRawPos(Point mouse)
		{
			Point pix = convertMousePosToPixelPos(mouse);
			return convertPixelPosToRawPos(pix);
		}

		/// <summary>Converts a rectangle in raw briefing units, to a new rectangle in map canvas units.</summary>
		Rectangle convertRawRectToPixelRect(Rectangle r)
		{
			Point p1 = convertRawPosToPixelPos(r.Left, r.Top);
			Point p2 = convertRawPosToPixelPos(r.Right, r.Bottom);
			return new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
		}

		/// <summary>Converts both mouse points into a proper normalized rectangle of the stretched display.</summary>
		/// <param name="mapCanvas">If true, the result is scaled to the map canvas instead of the stretched display.</param>
		Rectangle getNormalizedMouseRectangle(bool mapCanvas)
		{
			int x1 = _mouse1.X;
			int x2 = _mouse2.X;
			int y1 = _mouse1.Y;
			int y2 = _mouse2.Y;
			if (x1 > x2)
			{
				int temp = x1;
				x1 = x2;
				x2 = temp;
			}
			if (y1 > y2)
			{
				int temp = y1;
				y1 = y2;
				y2 = temp;
			}

			if (mapCanvas)
			{
				float mapScale = getMapScale();

				y1 -= _scaledMapRect.Top;
				y2 -= _scaledMapRect.Top;

				x1 = (int)(x1 / mapScale);
				x2 = (int)(x2 / mapScale);
				y1 = (int)(y1 / mapScale);
				y2 = (int)(y2 / mapScale);

				if (isXwing)
				{
					Point offset = getXwingMapMouseOffset();
					x1 += offset.X;
					x2 += offset.X;
					y1 += offset.Y;
					y2 += offset.Y;
				}
			}

			return new Rectangle(x1, y1, x2 - x1, y2 - y1);
		}

		/// <summary>Determines if the mouse points are far enough apart to register a drag operation.</summary>
		bool isMouseDrag() => ((Math.Abs(_mouse2.X - _mouse1.X) > CLICK_TOLERANCE) || (Math.Abs(_mouse2.Y - _mouse1.Y) > CLICK_TOLERANCE));

		/// <summary>Checks if a mouse click is inside a flightgroup or XWA icon.</summary>
		/// <returns>A flightgroup index for non-XWA platforms, an icon index for XWA, or -1 if nothing found.</returns>
		int getFlightgroupIndexAtCursor()
		{
			Point pos = convertMousePosToPixelPos(_mouse1);
			if (isXwa)
			{
				for (int i = 0; i < _mapIcons.Length; i++) if (_mapIcons[i].InteractRect.Contains(pos)) return i;
			}
			else
				for (int i = 0; i < _flightgroups.Count; i++) if (_flightgroups[i].InteractRect.Contains(pos)) return i;

			return -1;
		}

		bool clickInsideSelectedMapObject()
		{
			Point pos = convertMousePosToPixelPos(_mouse1);
			foreach (var so in _selectedObjects)
			{
				// Zero width is technically valid, but for our purposes it means the interact was disabled.
				if (so.InteractRect.Width == 0) continue;

				if (isValidWaypoint(so))
				{
					if (_flightgroups[so.Index].InteractRect.Contains(pos)) return true;
				}
				else if (so.Type == SelectedType.Event || so.Type == SelectedType.PathNode)
				{
					if (so.InteractRect.Contains(pos)) return true;
				}
			}
			return false;
		}

		bool clickInsideSelectedTimelineObject()
		{
			foreach (MapElement elem in _timelineElements)
			{
				if (!elem.InteractRect.Contains(_mouse1)) continue;

				// Header pip
				if (elem.EventIndex == -1) return true;

				// Specific element
				if (hasSelectedItem(SelectedType.Event, elem.EventIndex)) return true;
			}
			return false;
		}

		/// <summary>When using click selection rather than bounding box, priority by distance determines the closest match.</summary>
		int getClickPriority(Rectangle a, Rectangle b, int coefficient)
		{
			int ax = (a.Left + a.Right) / 2;
			int ay = (a.Top + a.Bottom) / 2;
			int bx = (b.Left + b.Right) / 2;
			int by = (b.Top + b.Bottom) / 2;
			int distx = bx - ax;
			int disty = by - ay;
			return (int)Math.Sqrt(distx * distx + disty * disty) * coefficient;
		}

		/// <summary>Checks that the provided location, in map canvas pixels, does not intersect with any existing items on the map.</summary>
		bool isEmptyMapLocation(Rectangle pos)
		{
			foreach (var afg in _flightgroups)
				if (afg.InteractRect.Width > 0 && pos.IntersectsWith(afg.InteractRect)) return false;

			foreach (var elem in _mapTextTags)
				if (elem.Enabled && elem.InteractRect.Width > 0 && pos.IntersectsWith(elem.InteractRect)) return false;

			foreach (var elem in _mapIcons)
				if (elem.Enabled && elem.InteractRect.Width > 0 && pos.IntersectsWith(elem.InteractRect)) return false;

			foreach (var elem in _mapShadowIcons)
				if (elem.Enabled && elem.InteractRect.Width > 0 && pos.IntersectsWith(elem.InteractRect)) return false;

			return true;
		}
		
		/// <summary>Retrieves the visible map area in raw units.</summary>
		Rectangle getRawMapRect(bool force)
		{
			// Free look mode will have a backup of the proper unaltered viewing location.
			if (_freeLookMode && !force) return _freeLookRect;

			Point p1, p2;
			if (isXwing)
			{
				// After the map canvas was resized to 424 in high-def, the edges were pushed off screen.
				// This also revealed some problems with how the rectangle was managed.
				// The goal here is that returning to the original position in free look draws the dashed
				// border within the edge of the visible panel, not pushed out of view.
				Rectangle src = _xwingSourceViewport[4];
				p1 = convertPixelPosToRawPos(src.Left, src.Top);
				p2 = convertPixelPosToRawPos(src.Right, src.Bottom);

				// Converting back to pixels when drawing involves a loss of precision, and it may extend off the edge.
				// Try to calculate the overflow and compensate.
				int maxWidth = (isXwingHighDef() ? 420 : 212) - 1;
				Point test = convertRawPosToPixelPos(p2);
				if (test.X > maxWidth) p2.X -= (getPixelRawX() + 1) * (test.X - maxWidth);
			}
			else
			{
				p1 = convertPixelPosToRawPos(0, 0);
				p2 = convertPixelPosToRawPos(_mapCanvas.Width - 1, _mapCanvas.Height - 1);
			}

			return new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
		}

		void setMoveZoomRectangles(int time)
		{
			// Backup the current state because we need to construct what the starting and ending viewing positions are.
			Point oldCurPos = _currentPosition;
			Point oldDestPos = _destinationPosition;
			Point oldCurZoom = _currentZoom;
			Point oldDestZoom = _destinationZoom;

			Point defZoom = getDefaultZoom();
			_currentPosition = new Point();
			_destinationPosition = new Point();
			_currentZoom = defZoom;
			_destinationZoom = defZoom;

			_mapShiftStart = getRawMapRect(true);
			_mapShiftEnd = _mapShiftStart;

			bool found = false;

			int lastTime = 0;
			foreach (var evt in _events)
			{
				while (lastTime < evt.Time)
				{
					animMapMoveZoomTick();
					lastTime++;
				}

				if (evt.Event != AbstractEventType.ZoomMap && evt.Event != AbstractEventType.MoveMap) continue;

				// Allow animation to finish unless interrupted by something else.
				if (evt.Time > time) break;
				else if (evt.Time == time && !found)
				{
					found = true;
					_mapShiftStart = getRawMapRect(true);
				}
				
				if (evt.Event == AbstractEventType.ZoomMap)
				{
					// NOTE: This does not account for the XW98 zoom bug.
					_destinationZoom = new Point(evt.Params[0], evt.Params[1]);
					if (evt.Time == 0) _currentZoom = _destinationZoom;
				}
				else if (evt.Event == AbstractEventType.MoveMap)
				{
					_destinationPosition.X = evt.Params[0];
					_destinationPosition.Y = evt.Params[1];
					if (evt.Time == 0) _currentPosition = _destinationPosition;
				}

				// Make sure any events at time zero are included.
				if (evt.Time == 0) _mapShiftStart = getRawMapRect(true);
			}

			// Jump to end and get the final position.
			_currentPosition = _destinationPosition;
			_currentZoom = _destinationZoom;
			_mapShiftEnd = getRawMapRect(true);
			_mapShiftEndZoom = _currentZoom;
			_mapShiftEndPos = _currentPosition;

			// Restore the state before returning.
			_currentPosition = oldCurPos;
			_destinationPosition = oldDestPos;
			_currentZoom = oldCurZoom;
			_destinationZoom = oldDestZoom;
		}

		void restoreShiftModeView()
		{
			_currentPosition = _mapShiftEndPos;
			_currentZoom = _mapShiftEndZoom;
			refreshMap();
		}
		#endregion Interaction and unit conversion
		
		#region Selection
		bool isValidPathNode(SelectedObject so) => (so.Type == SelectedType.PathNode && so.Index >= 0 && so.Index < _pathNodes.Count);
		bool isValidWaypoint(SelectedObject so) => (so.Type == SelectedType.Waypoint && so.Index >= 0 && so.Index < _flightgroups.Count);
		bool isValidEvent(SelectedObject so) => (so.Type == SelectedType.Event && so.Index >= 0 && so.Index < _events.Count);

		bool hasSelectedItem(SelectedType type, int index)
		{
			foreach (var so in _selectedObjects) if (so.Type == type && so.Index == index) return true;

			return false;
		}

		/// <summary>Retrieves a filtered list of only selected events.  The events are guaranteed to be valid.</summary>
		/// <returns>A list of events, or an empty list if no events are selected.</returns>
		List<SelectedObject> getSelectedEvents()
		{
			List<SelectedObject> results = new List<SelectedObject>();
			foreach (var so in _selectedObjects) if (isValidEvent(so)) results.Add(so);
			return results;
		}

		SelectedObject getSelectedEventType(AbstractEventType type)
		{
			foreach (var so in _selectedObjects) if (isValidEvent(so) && _events[so.Index].Event == type) return so;
			return null;
		}

		bool isEditingEventType(AbstractEventType type)
		{
			if (_selectedObjects.Count != 1) return false;

			SelectedObject so = _selectedObjects[0];
			if (isValidEvent(so))
			{
				var evt = _events[so.Index];
				if (evt.Event == type) return true;
				if (type == AbstractEventType.FgTag1 && evt.IsFgTag) return true;
				if (type == AbstractEventType.TextTag1 && evt.IsTextTag) return true;
			}

			return false;
		}

		/// <summary>Checks if exactly two events are selected: MoveMap and ZoomMap.</summary>
		/// <returns>True if both events are selected, valid, and on the same time index.</returns>
		bool isDualEditingMoveZoom()
		{
			if (_selectedObjects.Count != 2) return false;

			SelectedObject move = getSelectedEventType(AbstractEventType.MoveMap);
			SelectedObject zoom = getSelectedEventType(AbstractEventType.ZoomMap);

			if (move != null && zoom != null ) return (_events[move.Index].Time == _events[zoom.Index].Time);

			return false;
		}

		SelectedObject getSelectedItem(SelectedType type, int index)
		{
			foreach (var so in _selectedObjects) if (so.Type == type && so.Index == index) return so;

			return null;
		}
		
		/// <summary>Checks if all selected objects belong to the same XWA icon.</summary>
		/// <returns>An icon index only if all events are icon events, and all events use the same icon.  Otherwise -1.</returns>
		int getUniformIconSelection()
		{
			int iconIndex = -1;
			foreach (var so in _selectedObjects)
			{
				if (so.Type != SelectedType.Event) return -1;

				if (so.Index >= 0 && so.Index < _events.Count)
				{
					AbstractEvent evt = _events[so.Index];
					int checkIcon = -1;
					if (evt.Event == AbstractEventType.XwaSetIcon || evt.Event == AbstractEventType.XwaMoveIcon || evt.Event == AbstractEventType.XwaRotateIcon)
						checkIcon = evt.Params[0];
					else if (evt.Event == AbstractEventType.XwaShipInfo) checkIcon = evt.Params[1];

					if (checkIcon == -1 || (iconIndex >= 0 && iconIndex != checkIcon)) return -1;

					iconIndex = checkIcon;
				}
			}
			return iconIndex;
		}

		SelectedObject getPrimarySelectedObject()
		{
			SelectedObject result = null;
			int selCount = _selectedObjects.Count;
			if (selCount == 1) result = _selectedObjects[0];
			else if (selCount > 1)
			{
				// Multiple items are selected.  Determine if all of them match the same type.
				// If so, this allows us to select a primary object, and use the same edit controls.
				// But for the sake of simplicity, only allow this for certain types.
				int path = 0, waypoint = 0;
				Dictionary<AbstractEventType, int> eventCount = new Dictionary<AbstractEventType, int>();
				AbstractEventType eventType = AbstractEventType.None;
				foreach (var so in _selectedObjects)
				{
					if (so.Type == SelectedType.PathNode) path++;
					else if (so.Type == SelectedType.Waypoint) waypoint++;
					else if (isValidEvent(so))
					{
						eventType = _events[so.Index].Event;
						if (eventCount.ContainsKey(eventType)) eventCount[eventType]++;
						else eventCount.Add(eventType, 1);
					}
				}

				if (path == selCount || waypoint == selCount || (eventType != AbstractEventType.None && eventCount[eventType] == selCount))
					result = _selectedObjects[0];
				else
				{
					// Check for allowed mixed types.
					eventCount.TryGetValue(AbstractEventType.XwaMoveIcon, out int moveIconCount);
					eventCount.TryGetValue(AbstractEventType.XwaRotateIcon, out int rotIconCount);

					int textTagCount = 0;
					for (int i = 0; i < 8; i++)
					{
						eventCount.TryGetValue(AbstractEventType.TextTag1 + i, out int tag);
						textTagCount += tag;
					}

					if (moveIconCount + rotIconCount == selCount || textTagCount == selCount) result = _selectedObjects[0];
				}
			}
			return result;
		}
		
		int getEarliestSelectedTime()
		{
			int time = -1;
			foreach (var so in _selectedObjects)
				if (isValidEvent(so))
					if (time == -1 || _events[so.Index].Time < time) time = _events[so.Index].Time;
			return time;
		}
		
		/// <summary>Determines if two selected objects are pointing to the same thing.</summary>
		bool isMatchingSelection(SelectedObject left, SelectedObject right)
		{
			if (left == null || right == null) return false;
			return (left.Type == right.Type && left.Index == right.Index);
		}
		
		/// <summary>Deselects all objects and resets the sidebar when not in path mode.</summary>
		void deselectObjects()
		{
			setShiftMode(false);
			if (!_pathMode) resetSidebar();

			_selectedObjects.Clear();
			refreshTextTags();
			refreshCanvas(RefreshFlags.All);
		}

		/// <summary>Checks the currently selected objects to see if the item exists.</summary>
		/// <returns>The index of the matching currently selected item, or -1 if not found.</returns>
		int hasSelectedObject(SelectedObject item)
		{
			for (int i = 0; i < _selectedObjects.Count; i++) if (_selectedObjects[i].IsEqual(item)) return i;
			return -1;
		}

		/// <summary>Determines whether to merge, remove, or replace the specified objects from the current selection.  Updates the sidebar.</summary>
		/// <remarks>The input list may be empty, if a click or bounding box resulted in no objects found.</remarks>
		void updateSelectionList(bool mapClick, List<SelectedObject> newObjects)
		{
			int oldCount = _selectedObjects.Count;
			var oldSel = getPrimarySelectedObject();

			if (mapClick)
			{
				// This is a mouse click rather than a bounding box.  Since multiple items may overlap,
				// sort by priority (lowest is the best match) and discard everything else.
				if (newObjects.Count > 1)
				{
					newObjects.Sort(SelectedObject.CompareClickPriority);
					newObjects.RemoveRange(1, newObjects.Count - 1);
				}
			}

			bool modified = false;
			if (_keyStateShift)
			{
				// Add the newly selected items, if they aren't already selected.
				foreach (var so in newObjects)
				{
					if (hasSelectedObject(so) != -1) continue;

					_selectedObjects.Add(so);
					modified = true;
				}
			}
			else if (_keyStateCtrl)
			{
				// Toggle selected items.
				foreach (var so in newObjects)
				{
					int index = hasSelectedObject(so);
					if (index >= 0) _selectedObjects.RemoveAt(index);
					else _selectedObjects.Add(so);

					modified = true;
				}
			}
			else
			{
				// Completely replace existing selection.  The new list may be empty.
				if (newObjects.Count != _selectedObjects.Count || newObjects.Count > 0) modified = true;

				_selectedObjects = newObjects;
			}

			// Refresh sidebar and visuals.  Path mode reserves the entire sidebar.
			if (_pathMode)
			{
				int prevIconIndex = _lastSelectedIconIndex;
				int prevMoveUid = _lastSelectedMoveIconUid;
				bool workingExist = (getWorkingMoveIcon() >= 0);

				// Selection prioritizes path nodes first.  If it's a node, sync up the ListBox selection.
				if (_selectedObjects.Count > 0)
				{
					SelectedObject so = _selectedObjects[0];
					if (isValidPathNode(so)) selectPathNode(so.Index, false);
					else if (isValidEvent(so))
					{
						var evt = _events[so.Index];
						if (evt.Event == AbstractEventType.XwaMoveIcon)
						{
							// Normally this information would be updated when refreshing the edit controls.
							// But we're not calling that, so assign manually.
							_lastSelectedIconIndex = evt.Params[0];
							_lastSelectedMoveIconUid = evt.UID;
						}
					}
				}
				else
				{
					// Disable the movement edit controls.
					selectPathNode(-1, false);
				}

				// Allows the user to select generated icons to quickly delete them without changing the working icon.
				// As long as the selection belongs to the same icon index.
				// If the working icon was deleted, will retain whatever was assigned in the search.
				if (workingExist && _lastSelectedIconIndex == prevIconIndex) _lastSelectedMoveIconUid = prevMoveUid;
			}
			else if (modified)
			{
				refreshTextTags();

				// Avoids flickering the sidebar if the resulting selection is the same thing.
				var newSel = getPrimarySelectedObject();
				if (newSel == null || !isMatchingSelection(newSel, oldSel))
				{
					setShiftMode(false);
					displayEditControls();
				}
				else refreshEditControls();
			}

			refreshCanvas(RefreshFlags.All);
		}

		void selectMapObjects()
		{
			var r = getNormalizedMouseRectangle(true);
			var objectList = enumMapSelection(r);
			bool mapClick = (r.Width <= CLICK_TOLERANCE && r.Height <= CLICK_TOLERANCE);
			updateSelectionList(mapClick, objectList);
		}

		void selectTimelineObjects()
		{
			var r = getNormalizedMouseRectangle(false);
			var objectList = enumTimelineSelection(r);
			updateSelectionList(false, objectList);
		}

		void selectEventByUid(int uid)
		{
			deselectObjects();

			int eventIndex = getEventIndexByUid(uid);
			if (eventIndex >= 0)
			{
				_selectedObjects.Add(new SelectedObject(SelectedType.Event, eventIndex, uid));
				displayEditControls();
				refreshCanvas(RefreshFlags.All);
			}
		}

		void expandIconSelectionByType()
		{
			// Determine the kinds of things that are currently selected.
			HashSet<int> selIconIndices = new HashSet<int>();
			HashSet<AbstractEventType> selEventTypes = new HashSet<AbstractEventType>();
			HashSet<int> selEventTimes = new HashSet<int>();

			foreach (var so in _selectedObjects)
			{
				if (!isValidEvent(so)) continue;

				var evt = _events[so.Index];
				if (evt.Event == AbstractEventType.XwaSetIcon || evt.Event == AbstractEventType.XwaMoveIcon || evt.Event == AbstractEventType.XwaRotateIcon)
				{
					selEventTypes.Add(evt.Event);
					selEventTimes.Add(evt.Time);
					selIconIndices.Add(evt.Params[0]);
				}
			}

			if (selIconIndices.Count == 0) return;

			// Implied types to select, if one was selected but not another.
			// NewIcon expands to everything.
			// MoveIcon implicitly selects RotateIcon, and vice versa.
			if (selEventTypes.Contains(AbstractEventType.XwaSetIcon))
			{
				selEventTypes.Add(AbstractEventType.XwaMoveIcon);
				selEventTypes.Add(AbstractEventType.XwaRotateIcon);
			}

			if (selEventTypes.Contains(AbstractEventType.XwaMoveIcon)) selEventTypes.Add(AbstractEventType.XwaRotateIcon);

			if (selEventTypes.Contains(AbstractEventType.XwaRotateIcon)) selEventTypes.Add(AbstractEventType.XwaMoveIcon);

			// We don't know what region page the items were selected on.
			// Iterate through events and note the page times.
			List<int> pageStartTimes = new List<int>();
			List<int> pageEndTimes = new List<int>();
			List<bool> pageUsed = new List<bool>();
			int startTime = 0, endTime = 0;
			for (int i = 0; i < _events.Count; i++)
			{
				endTime = _events[i].Time;
				if (_events[i].Event != AbstractEventType.XwaChangeRegion) continue;

				pageStartTimes.Add(startTime);
				pageEndTimes.Add(endTime);
				pageUsed.Add(false);
				startTime = _events[i].Time;
				endTime = _events[i].Time;
			}
			// Adds the final page from the last region break until the end of the briefing.
			// Or the entire briefing if no region breaks were encountered.
			pageStartTimes.Add(startTime);
			pageEndTimes.Add(endTime);
			pageUsed.Add(false);

			// From the selected event times, flag which pages we can select from.
			for (int p = 0; p < pageUsed.Count; p++)
			{
				foreach (int time in selEventTimes)
					if (time >= pageStartTimes[p] && time <= pageEndTimes[p]) pageUsed[p] = true;
			}

			short[] firstMoveTime = new short[_mapIcons.Length];
			for (int i = 0; i < firstMoveTime.Length; i++) firstMoveTime[i] = -1;

			// Rebuild a new selection by iterating through all events.
			// Any event that matches the selected icon index, or selected type, and exists within the correct page and time span.
			_selectedObjects.Clear();
			for (int i = 0; i < _events.Count; i++)
			{
				var evt = _events[i];

				int iconIndex = -1;
				if (evt.Event == AbstractEventType.XwaSetIcon || evt.Event == AbstractEventType.XwaMoveIcon || evt.Event == AbstractEventType.XwaRotateIcon)
					iconIndex = evt.Params[0];

				if (!selEventTypes.Contains(evt.Event) || !selIconIndices.Contains(iconIndex)) continue;

				// Determine whether the page is accepted, and whether the event is within the page times.
				bool found = false;
				for (int p = 0; p < pageStartTimes.Count; p++)
					if (pageUsed[p] && evt.Time >= pageStartTimes[p] && evt.Time <= pageEndTimes[p])
					{
						found = true;
						break;
					}

				if (found && iconIndex >= 0 && iconIndex < firstMoveTime.Length)
				{
					if (evt.Event == AbstractEventType.XwaMoveIcon && firstMoveTime[iconIndex] == -1) firstMoveTime[iconIndex] = evt.Time;

					// Don't select these event types at the first MoveIcon time unless the user has
					// explicitly selected the first icon.  Want to keep Move/Rotate together if they
					// happen on the same frame.
					if ((evt.Event == AbstractEventType.XwaMoveIcon || evt.Event == AbstractEventType.XwaRotateIcon) && firstMoveTime[iconIndex] == evt.Time && !selEventTimes.Contains(evt.Time))
						found = false;
				}

				if (found) _selectedObjects.Add(new SelectedObject(SelectedType.Event, i, evt.UID));
			}

			refreshFromNewSelection();
		}

		/// <summary>Checks all selected objects, making sure that their event indices point to the correct event via their UID.  Invalid events are removed from the selection.</summary>
		/// <remarks>This should be used after any operation on the event list that inserts, deletes, or reorders the events.</remarks>
		void relinkSelectedEventUid()
		{
			int pos = 0;
			while (pos < _selectedObjects.Count)
			{
				bool delete = false;

				var so = _selectedObjects[pos];
				if (so.Type == SelectedType.Event)
				{
					so.Index = getEventIndexByUid(so.EventUID);
					// It's possible for an event to have been deleted, or removed as an undo operation.
					if (so.Index == -1) delete = true;
				}

				if (delete) _selectedObjects.RemoveAt(pos);
				else pos++;
			}
		}

		/// <summary>Determines the raw position of the primary selected object, if it has a position that is relevant to multi-selection editing.</summary>
		/// <returns>If the object is a movable type, returns true and assigns the out parameters.  Otherwise returns false.</returns>
		bool getPrimaryPosition(SelectedObject pso, out Point result)
		{
			int resX = int.MaxValue;
			int resY = int.MaxValue;

			if (isValidPathNode(pso))
			{
				resX = _pathNodes[pso.Index].X;
				resY = _pathNodes[pso.Index].Y;
			}
			else if (isValidWaypoint(pso))
			{
				var afg = _flightgroups[pso.Index];
				resX = afg.DisplayPos.X;
				resY = afg.DisplayPos.Y;
			}
			else if (isValidEvent(pso))
			{
				var evt = _events[pso.Index];
				if (evt.Event == AbstractEventType.XwaMoveIcon)
				{
					resX = evt.Params[1];
					resY = evt.Params[2];
				}
				else if (evt.IsTextTag)
				{
					resX = evt.Params[1];
					resY = evt.Params[2];
				}
			}

			if (resX != int.MaxValue && resX != int.MaxValue)
			{
				result = new Point(resX, resY);
				return true;
			}

			result = new Point(0, 0);
			return false;
		}

		void deleteSelectedItems()
		{
			if (_selectedObjects.Count == 0) return;

			// If on the event tab, will try to preserve the selected line.
			int listIndex = lstEvents.SelectedIndex;

			// Sort order to delete highest indexes first
			_selectedObjects.Sort(SelectedObject.CompareDescending);

			beginUndoFrame(true);

			bool eventChanged = false;
			bool pathChanged = false;
			bool focusChanged = false;

			foreach (var so in _selectedObjects)
			{
				if (isValidEvent(so))
				{
					object data = _events[so.Index].GetDataSnapshot();
					UndoOperation op = new UndoOperation(UndoType.DeleteEvent, data, so.Index);
					op.Update(data);
					addUpdatedUndoOperation(op, false);
					deleteEvent(so.Index);
					eventChanged = true;
				}
				else if (isValidPathNode(so))
				{
					deletePathNode(so.Index, false);
					pathChanged = true;
				}
				else if (isValidWaypoint(so) && isXwing)
				{
					deleteXwingFlightgroup(so.Index);
					focusChanged = true;
				}
			}

			if (focusChanged) Focus();

			if (pathChanged) refreshPathList(-1);

			deselectObjects();

			if (eventChanged)
			{
				rebuildShadowIcons();
				rebuildBriefing();
			}

			if (isCurrentTab(MainTabIndex.BriefingMap)) refreshCanvas(RefreshFlags.All);
			else if (isCurrentTab(MainTabIndex.EventList))
			{
				refreshEventList(-1);

				// Try to select the next item after deletion.  Will be -1 if nothing is left.
				if (listIndex >= lstEvents.Items.Count) listIndex = lstEvents.Items.Count - 1;
				lstEvents.SelectedIndex = listIndex;
			}
		}
		
		/// <summary>Refreshes after a direct alteration of the selected object list.</summary>
		void refreshFromNewSelection()
		{
			if (!_pathMode) displayEditControls();

			refreshCanvas(RefreshFlags.All);

			if (isCurrentTab(MainTabIndex.EventList))
				// Besides updating the list selection, this also nicely changes the scroll position.
				applySelectionToEventList();
		}
		
		List<SelectedObject> enumMapSelection(Rectangle pixBound)
		{
			List<SelectedObject> results = new List<SelectedObject>();

			// Path mode prioritizes nodes and ignores other things which have no relevance.
			if (_pathMode)
			{
				for (int i = 0; i < _pathNodes.Count; i++)
				{
					var node = _pathNodes[i];
					if (node.InteractRect.Width > 0 && node.InteractRect.IntersectsWith(pixBound)) results.Add(new SelectedObject(SelectedType.PathNode, i, 0, 1));
				}
			}
			else
			{
				for (int i = 0; i < _flightgroups.Count; i++)
				{
					var afg = _flightgroups[i];

					if (!afg.Waypoints[getFlightgroupWaypoint()].Enabled && !chkIconShadows.Checked) continue;

					if (afg.InteractRect.Width > 0 && afg.InteractRect.IntersectsWith(pixBound))
					{
						int p = getClickPriority(afg.InteractRect, pixBound, 1);
						results.Add(new SelectedObject(SelectedType.Waypoint, i, 0, p));
					}
				}

				foreach (var elem in _mapTextTags)
				{
					if (elem.Enabled && elem.InteractRect.Width > 0 && elem.InteractRect.IntersectsWith(pixBound))
					{
						int p = getClickPriority(elem.InteractRect, pixBound, 2);
						results.Add(new SelectedObject(SelectedType.Event, elem.EventIndex, elem.EventUid, p));
					}
				}
			}

			// Even if we're in path mode, we still need to be able to select icons.
			// Paths can be aligned to the selected icon, or a path can be derived from selected icons.
			if (!_pathMode || (_pathMode && results.Count == 0))
			{
				HashSet<int> selIconUid = new HashSet<int>();

				foreach (var elem in _mapIcons)
				{
					if (elem.Enabled && elem.InteractRect.Width > 0 && elem.InteractRect.IntersectsWith(pixBound))
					{
						int p = getClickPriority(elem.InteractRect, pixBound, 1);

						// If there isn't any move item attached, it's a NewIcon event at the default position.
						if (elem.LastMoveEventIndex == -1)
							results.Add(new SelectedObject(SelectedType.Event, elem.EventIndex, elem.EventUid, p));
						else
							results.Add(new SelectedObject(SelectedType.Event, elem.LastMoveEventIndex, elem.LastMoveEventUid, p));

						// Keep track of any icons selected here, so that they can be excluded from the shadow icon check.
						selIconUid.Add(elem.LastMoveEventUid);
					}
				}

				if (chkIconShadows.Checked)
				{
					foreach (var elem in _mapShadowIcons)
					{
						if (elem.Enabled && isShadowIconVisible(elem) && !selIconUid.Contains(elem.EventUid))
						{
							if (elem.InteractRect.Width > 0 && elem.InteractRect.IntersectsWith(pixBound))
							{
								int p = getClickPriority(elem.InteractRect, pixBound, 5);
								results.Add(new SelectedObject(SelectedType.Event, elem.EventIndex, elem.EventUid, p));
								selIconUid.Add(elem.LastMoveEventUid);
							}
						}
					}
				}
			}
			return results;
		}

		List<SelectedObject> enumTimelineSelection(Rectangle pixBound)
		{
			List<SelectedObject> results = new List<SelectedObject>();

			List<int> selTime = new List<int>();
			HashSet<int> selUid = new HashSet<int>();

			// This collection only stores the visible items.
			foreach (var elem in _timelineElements)
			{
				if (!elem.InteractRect.IntersectsWith(pixBound)) continue;

				if (elem.EventIndex == -1)
				{
					// The header pip.  Data holds the time.
					selTime.Add(elem.DataIndex);
				}
				else if (elem.EventIndex >= 0)
				{
					results.Add(new SelectedObject(SelectedType.Event, elem.EventIndex, elem.EventUid));
					selUid.Add(elem.EventUid);
				}
			}

			// Check all header pips that may have been selected, and grab everything at the chosen times.
			foreach (int time in selTime)
			{
				for (int i = 0; i < _events.Count; i++)
				{
					var evt = _events[i];
					if (evt.Time > time) break;

					if (evt.Time == time && !selUid.Contains(evt.UID)) results.Add(new SelectedObject(SelectedType.Event, i, evt.UID));
				}
			}

			return results;
		}
		#endregion Selection

		#region Event lookup and manipulation
		/// <summary>Resolves which event index is associated with the specified event UID.</summary>
		/// <returns>A valid event list index, or -1 if not found.</returns>
		int getEventIndexByUid(int uid)
		{
			for (int i = 0; i < _events.Count; i++) if (_events[i].UID == uid) return i;
			
			return -1;
		}

		int countEventsAtTime(int time)
		{
			int result = 0;
			for (int i = 0; i < _events.Count; i++)
			{
				if (_events[i].Time == time) result++;
				else if (_events[i].Time > time) break;
			}
			return result;
		}

		/// <summary>Checks if there's a specific event anywhere at the specified time.</summary>
		/// <returns>The event object and its event list index.  Or null and -1 if not found.</returns>
		AbstractEvent getEventAtTime(AbstractEventType type, int time, out int resultEventIndex)
		{
			for (int i = 0; i < _events.Count; i++)
			{
				var evt = _events[i];
				if (evt.Time > time) break;

				if (evt.Event == type && evt.Time == time)
				{
					resultEventIndex = i;
					return _events[i];
				}
			}
		
			resultEventIndex = -1;
			return null;
		}
		
		/// <summary>Attempts to find the most recent RotateIcon event for the specified icon, on or before the specified time.</summary>
		/// <returns>A valid event list index, or -1 if not found.</returns>
		int getRotateIconEventIndex(int time, int iconIndex)
		{
			int result = -1;
			for (int i = 0; i < _events.Count; i++)
			{
				var evt = _events[i];
				if (evt.Time > time) break;

				if (evt.Event == AbstractEventType.XwaChangeRegion && evt.Time < time) result = -1;
				else if (evt.Event == AbstractEventType.XwaRotateIcon && evt.Params[0] == iconIndex) result = i;
			}
			return result;
		}
		
		/// <summary>Retrieves the highest and lowest event indices (inclusive) that occupy a specific time.  Or -1 if not found.</summary>
		void getEventIndexRangeAtTime(int time, out int lowestIndex, out int highestIndex)
		{
			lowestIndex = -1;
			highestIndex = -1;
			for (int i = 0; i < _events.Count; i++)
			{
				if (_events[i].Time == time)
				{
					highestIndex = i;
					if (lowestIndex == -1) lowestIndex = i;
				}
				else if (_events[i].Time > time) break;
			}
		}

		/// <summary>Retrieves a list of event indices for all events at the specified time.</summary>
		/// <returns>A list of valid event indices, or an empty list if nothing found.</returns>
		List<int> enumEventsAtTime(int time)
		{
			List<int> results = new List<int>();
			for (int i = 0; i < _events.Count; i++)
			{
				if (_events[i].Time == time) results.Add(i);
				else if (_events[i].Time > time) break;
			}
			return results;
		}
		
		AbstractEvent getIconEventAtTime(AbstractEventType type, int iconIndex, int time, out int resultEventIndex)
		{
			for (int i = 0; i < _events.Count; i++)
			{
				var evt = _events[i];
				if (evt.Time > time) break;

				if (evt.Event == type && evt.Time == time)
				{
					switch (evt.Event)
					{
						case AbstractEventType.XwaSetIcon:
						case AbstractEventType.XwaMoveIcon:
						case AbstractEventType.XwaRotateIcon:
							if (evt.Params[0] == iconIndex)
							{
								resultEventIndex = i;
								return evt;
							}
							break;

						case AbstractEventType.XwaShipInfo:
							if (evt.Params[1] == iconIndex)
							{
								resultEventIndex = i;
								return evt;
							}
							break;
					}
				}
			}

			resultEventIndex = -1;
			return null;
		}

		void insertEvent(int listIndex, AbstractEvent evt, bool makeUndo)
		{
			// If the index is the same as Count, it will be added to the end.
			if (listIndex >= 0 && listIndex <= _events.Count)
			{
				var def = EventDef.GetEventDefByType(evt.Event);
				updateEventSpace(def.GetSize());

				_events.Insert(listIndex, evt);
				_highestEventTime = getHighestTime();

				autoAdjustDuration(makeUndo);

				if (makeUndo)
				{
					UndoOperation op = new UndoOperation(UndoType.NewEvent, evt.GetDataSnapshot(), listIndex);
					op.Update();
					addUpdatedUndoOperation(op, false);
				}
			}
		}

		void deleteEvent(int listIndex)
		{
			var evt = _events[listIndex];
			var def = EventDef.GetEventDefByType(evt.Event);
			updateEventSpace(-1 * def.GetSize());

			_events.RemoveAt(listIndex);
			_highestEventTime = getHighestTime();
		}

		bool hasAvailableSpace(AbstractEventType type)
		{
			var def = EventDef.GetEventDefByType(type);
			if (def == null) return false;

			int reqSize = _eventSpaceUsed + def.GetSize();
			return (reqSize <= _eventSpaceMax);
		}

		void clearEvents()
		{
			_eventSpaceUsed = 2;  // Reserved space for EndBriefing
			_events.Clear();
			updateEventSpace(0);
		}
		
		void updateEventSpace(int change)
		{
			_eventSpaceUsed += change;
			if (_eventSpaceUsed <= 2)  // Reserved space
				lblEventSpace.Text = "briefing is empty";
			else lblEventSpace.Text = _eventSpaceUsed + " / " + _eventSpaceMax + " space used";
		}

		int calculateEventSpace(List<AbstractEvent> events)
		{
			int size = 2;  // Reserved space
			foreach (var evt in events) size += EventDef.GetEventDefByType(evt.Event).GetSize();

			return size;
		}

		/// <summary>Attempts to find a suitable event list index where an XWA icon event can be inserted.</summary>
		/// <remarks>Scans existing icon-related events at the time to try and maintain a reasonable order.</remarks>
		int getInsertionIndexForIconEvent(int timeIndex, AbstractEventType type, int iconIndex)
		{
			int afterIndex = 0;  // If the list is empty, this is the default insert point.
			int groupStart = -1;
			int groupEnd = -1;

			for (int i = 0; i < _events.Count; i++)
			{
				int eventTime = _events[i].Time;
				if (eventTime < timeIndex)
				{
					// If nothing is found at the target time, insert after closest event.
					afterIndex = i + 1;
				}
				else if (eventTime == timeIndex)
				{
					if (groupStart == -1) groupStart = i;

					groupEnd = i;
				}
				else if (eventTime > timeIndex) break;
			}

			if (groupStart == -1) return afterIndex;

			// Pagebreaks and Markers are always first
			if (type == AbstractEventType.PageBreak || type == AbstractEventType.SkipMarker) return groupStart;

			// If the first event is a Pagebreak or Marker, we'll never insert there, so skip over it.
			if (_events[groupStart].Event == AbstractEventType.PageBreak || _events[groupStart].Event == AbstractEventType.SkipMarker) groupStart++;

			if (type == AbstractEventType.XwaSetIcon)
			{
				// If there are existing NewIcon events, find a sequential insert position.
				int afterNewIcon = -1;
				int beforeNewIcon = -1;
				for (int i = groupStart; i <= groupEnd; i++)
				{
					if (_events[i].Event == AbstractEventType.XwaSetIcon)
					{
						if (iconIndex > _events[i].Params[0]) afterNewIcon = i + 1;
						else beforeNewIcon = i;
					}
				}

				if (afterNewIcon >= 0) return afterNewIcon;

				if (beforeNewIcon >= 0) return beforeNewIcon;

				// Aside from PageBreak, NewIcon are usually at the top, so insert there.
				return groupStart;
			}
			else if (type == AbstractEventType.XwaMoveIcon)
			{
				int afterNewIcon = -1;
				int beforeMoveIcon = -1;
				int afterMoveIcon = -1;
				int beforeRotateIcon = -1;
				for (int i = groupStart; i <= groupEnd; i++)
				{
					if (_events[i].Event == AbstractEventType.XwaSetIcon) afterNewIcon = i + 1;
					else if (_events[i].Event == AbstractEventType.XwaRotateIcon && beforeRotateIcon == -1) beforeRotateIcon = -1;
					else if (_events[i].Event == AbstractEventType.XwaMoveIcon)
					{
						if (iconIndex > _events[i].Params[0]) afterMoveIcon = i + 1;
						else beforeMoveIcon = i;
					}
				}

				if (afterMoveIcon >= 0) return afterMoveIcon;

				if (beforeMoveIcon >= 0) return beforeMoveIcon;

				// There aren't any existing MoveIcons.  They usually come after the NewIcons, so place after that if found.
				if (afterNewIcon >= 0) return afterNewIcon;

				// If for some reason there's a RotateIcon, it's standard for MoveIcons to appear before them.
				if (beforeRotateIcon >= 0) return beforeRotateIcon;
			}
			else if (type == AbstractEventType.XwaRotateIcon)
			{
				int afterNewIcon = -1;
				int afterMoveIcon = -1;
				int beforeRotateIcon = -1;
				int afterRotateIcon = -1;
				for (int i = groupStart; i <= groupEnd; i++)
				{
					if (_events[i].Event == AbstractEventType.XwaSetIcon) afterNewIcon = i + 1;
					else if (_events[i].Event == AbstractEventType.XwaMoveIcon) afterMoveIcon = i + 1;
					else if (_events[i].Event == AbstractEventType.XwaRotateIcon)
					{
						if (iconIndex > _events[i].Params[0]) afterRotateIcon = i + 1;
						else beforeRotateIcon = i;
					}
				}

				if (afterRotateIcon >= 0) return afterRotateIcon;

				if (beforeRotateIcon >= 0) return beforeRotateIcon;

				// No existing RotateIcons.  Ideally insert after MoveIcon.
				if (afterMoveIcon >= 0) return afterMoveIcon;

				// No MoveIcons either.  Insert after NewIcon.
				if (afterNewIcon >= 0) return afterNewIcon;
			}

			// If we get here, there was an icon event with no suitable insert location found.
			// Or we're inserting some other event.
			// Insert after the end of the group.
			return groupEnd + 1;
		}

		int getInsertionIndexForEvent(int timeIndex, bool firstInGroup)
		{
			int found = 0;
			for (int i = 0; i < _events.Count; i++)
			{
				int eventTime = _events[i].Time;
				if (eventTime < timeIndex) found = i + 1;
				else if (eventTime == timeIndex)
				{
					if (firstInGroup) return i;

					found = i + 1;
				}
				else if (eventTime > timeIndex) return found;
			}
			return found;
		}

		short[] getTagUsageArray(int time, AbstractEventType start, AbstractEventType end, AbstractEventType clear)
		{
			short[] result = new short[8];
			for (int i = 0; i < result.Length; i++) result[i] = -1;

			foreach (var evt in _events)
			{
				if (evt.Event >= start && evt.Event <= end)
				{
					int index = (evt.Event - start);
					result[index] = evt.Time;
				}
				else if (evt.Event == clear || evt.Event == AbstractEventType.XwaChangeRegion)
				{
					if (evt.Time <= time) for (int i = 0; i < result.Length; i++) result[i] = -1;
					else break;        // Don't check beyond the specified time
				}
			}
			return result;
		}

		/// <summary>Scans the event list to determine if an icon is used anywhere in the region page that contains the specified time.</summary>
		/// <param name="time">The time to search for, either the current briefing time, or the time of a particular event.  Used to discard regions that don't include the specified time.</param>
		/// <returns>An array indicating the icon usage states of all map icons.  Each element is a packed bitfield, information may be obtained via <see cref="unpackIconUsage"/></returns>
		int[] getIconUsageArray(int time)
		{
			int[] result = new int[_mapIcons.Length];
			int region = 0;

			foreach (var evt in _events)
			{
				if (evt.Event == AbstractEventType.XwaSetIcon)
				{
					short index = evt.Params[0];
					short craft = evt.Params[1];
					if (index >= 0 && index < result.Length) result[index] = packIconUsage(result[index], evt.Time, craft);
				}
				else if (evt.Event == AbstractEventType.XwaChangeRegion && evt.Params[0] != region)
				{
					region = evt.Params[0];
					if (evt.Time <= time) Array.Clear(result, 0, result.Length);
					else break;        // Don't check beyond the specified time
				}
			}
			return result;
		}
		int packIconUsage(int currentPackedValue, int eventTime, int craftType)
		{
			// High-word is craft type.  The highest bit indicates that the icon was cleared (craft set
			// to zero) while still preserving the last known craft type.  Low-word is the event time.
			if (craftType == 0) craftType = (currentPackedValue >> 16) | 0x8000;
			return (craftType << 16) | (ushort)eventTime;
		}
		/// <summary>Extracts relevant information from a single data element obtained from <see cref="getIconUsageArray"/></summary>
		void unpackIconUsage(int packedValue, out int eventTime, out int craftType, out bool deactivated)
		{
			eventTime = packedValue & 0xFFFF;
			craftType = packedValue >> 16;
			deactivated = ((craftType & 0x8000) != 0);
			craftType &= 0x7FFF;
		}
		#endregion Event lookup and manipulation

		#region Sidebar
		/// <summary>Adjusts all controls in the map options panel that appears in the sidebar.  Disabled items are removed, and remaining controls are restacked vertically.</summary>
		void rebuildSidebarConfig()
		{
			// The order of controls in the list does not correspond to their internal ordering in the
			// designer.  Sort based on their vertical position, and remove any controls that were disabled.
			// While here, find the starting alignment (the topmost item in the designer view).
			SortedDictionary<int, Control> controls = new SortedDictionary<int, Control>();
			List<Control> inactive = new List<Control>();
			int curY = int.MaxValue;
			foreach (Control c in pnlMapOptions.Controls)
			{
				if (c.Location.Y < curY) curY = c.Location.Y;

				if (c.Enabled) controls.Add(c.Location.Y, c);
				else inactive.Add(c);
			}

			// Remove controls flagged for removal.
			foreach (Control c in inactive) pnlMapOptions.Controls.Remove(c);

			// Vertically reposition all remaining items.
			foreach (KeyValuePair<int, Control> item in controls)
			{
				Control c = item.Value;
				c.Location = new Point(c.Location.X, curY);
				curY += c.Height + (c.Margin.Bottom - 1);
			}

			// Finalize new height of the container.
			pnlMapOptions.Height = curY + pnlMapOptions.Margin.Bottom;
		}

		/// <summary>Populates a dropdown with all tags to choose from, marking which ones are currently free or in use.</summary>
		/// <param name="cbo">The dropdown to populate.</param>
		/// <param name="curIndex">The current index selected of an existing item.  Use -1 if creating a new item.</param>
		/// <param name="curTime">Time used to determine if something is in use now or later.</param>
		/// <param name="usage">Tag usage availability information.</param>
		void populateTagSlotDropdown(ComboBox cbo, int curIndex, int curTime, short[] usage)
		{
			bool btemp = _loading;
			_loading = true;

			int prevIndex = cbo.SelectedIndex;
			cbo.BeginUpdate();

			int count = (isXwing ? 4 : 8);
			cbo.Items.Clear();
			for (int i = 0; i < count; i++)
			{
				int tagTime = usage[i];
				string s = "#" + (i + 1);
				if (i == curIndex) s += " (this)";
				else if (tagTime != -1 && tagTime <= curTime) s += " (in use)";
				else if (tagTime != -1 && tagTime > curTime) s += " (later)";
				cbo.Items.Add(s);
			}

			cbo.EndUpdate();
			safeSetCbo(cbo, prevIndex);

			_loading = btemp;
		}

		/// <summary>Checks if an icon slot's map position has been properly initialized with a corresponding Move event.</summary>
		/// <param name="time">Time of the NewIcon event.</param>
		/// <param name="iconIndex">Icon index being activated.</param>
		bool isIconInitialized(int time, int iconIndex)
		{
			bool result = false;
			int moveTime = -1;
			foreach (var evt in _events)
			{
				if (evt.Event == AbstractEventType.XwaSetIcon && evt.Params[0] == iconIndex) result = true;
				else if (evt.Event == AbstractEventType.XwaMoveIcon && moveTime == -1 && evt.Params[0] == iconIndex) moveTime = evt.Time;
				else if (evt.Event == AbstractEventType.XwaChangeRegion && evt.Time < time)
				{
					result = false;
					moveTime = -1;
				}
			}
			return (result && moveTime >= 0 && moveTime <= time);
		}

		/// <summary>Populates a dropdown with all icon slots to choose from, marking which ones are currently free or in use.</summary>
		/// <param name="cbo">The dropdown to populate.</param>
		/// <param name="curIndex">The current index selected of an existing item.  Use -1 if creating a new item.</param>
		/// <param name="curTime">Time used to determine if something is in use now or later.</param>
		/// <param name="warnType">If set to 1, marks already active slots for icon creation.  If set to 2, marks inactive slots for ShipInfo.  If set to 0, only marks later slots.</param>
		/// 
		void populateIconDropdown(ComboBox cbo, int curIndex, int curTime, int warnType)
		{
			bool btemp = _loading;
			_loading = true;

			int[] usage = getIconUsageArray(curTime);
			int prevIndex = cbo.SelectedIndex;
			cbo.BeginUpdate();

			while (cbo.Items.Count < _mapIcons.Length) cbo.Items.Add("");

			for (int i = 0; i < _mapIcons.Length; i++)
			{
				unpackIconUsage(usage[i], out int evtTime, out int craftType, out bool deactivated);

				// When creating icons, existing icons will be marked as in use.
				// When creating a ShipInfo event, the icon must exist at the current time or it can crash
				// the game.  Warnings will say "not set" and possibly "YET" if defined later.
				// Icons defined in the future are always marked "later".
				string s = "#" + (i + 1) + " ";
				if (warnType == 2 && (evtTime > curTime || craftType == 0 || deactivated)) s += " not set";
				else s += (craftType >= 1 ? getCraftTypeAbbrev(craftType) : "");

				if (i == curIndex) s += " (this)";
				else if (evtTime != 0)
				{
					if (evtTime <= curTime && warnType == 1) s += " (in use)";
					else if (evtTime > curTime) s += (warnType == 2 ? " YET" : " (later)");
				}

				cbo.Items[i] = s;
			}
	
			cbo.EndUpdate();
			safeSetCbo(cbo, prevIndex);

			_loading = btemp;
		}

		void populateFlightgroupDropdown(ComboBox cbo, bool prefix)
		{
			int prevIndex = cbo.SelectedIndex;
			cbo.BeginUpdate();
			cbo.Items.Clear();
			for (int i = 0; i < _flightgroups.Count; i++) cbo.Items.Add(prefix ? _flightgroups[i].DisplayName : _flightgroups[i].Name);
			cbo.EndUpdate();

			if (prevIndex < 0) prevIndex = 0;
			if (prevIndex >= cbo.Items.Count) prevIndex = cbo.Items.Count - 1;
			cbo.SelectedIndex = prevIndex;
		}
		
		/// <summary>Completely resets the sidebar to its default state, assuming no items are selected, nor a temporary event is being created.</summary>
		void resetSidebar()
		{
			beginSidebarBuild();

			// For the map tab.  Event list tab has an empty sidebar until something is selected.
			if (isCurrentTab(MainTabIndex.BriefingMap))
			{
				addSidebarControl(pnlTempCreate);
				addSidebarControl(grpTimeShift);
				grpTimeShift.Top += grpTimeShift.Margin.Top;
				addSidebarControl(pnlMapOptions);
				pnlMapOptions.Top = pnlEditContainer.Bottom - pnlMapOptions.Height;

				if (isXwing)
				{
					// Dynamically display the extra panel buttons depending on whether they're enabled in the current page settings.
					// Almost nobody will ever use these.  No need to clutter the sidebar if the panels are disabled.
					var panel3 = getXwingPanel(Platform.Xwing.PageTemplate.Elements.Panel3);
					var panel4 = getXwingPanel(Platform.Xwing.PageTemplate.Elements.Panel4);
					cmdPanel3.Visible = (panel3.IsVisible && panel3.Right != 0 && panel3.Bottom != 0);
					cmdPanel4.Visible = (panel4.IsVisible && panel4.Right != 0 && panel4.Bottom != 0);

					refreshXwingTempCreateButtons();
				}

				setMapScrollbarVisibility(false);

				optFG.Enabled = false;
				optText.Enabled = false;

				cmdOk.Enabled = false;
				cmdCancel.Enabled = false;
			}

			endSidebarBuild();
		}

		/// <summary>Enables or disables all child controls within a panel.</summary>
		void setChildControls(Panel panel, bool state) { foreach (Control c in panel.Controls) c.Enabled = state; }
		
		/// <summary>Rebuilds the sidebar based on the current selection, and refreshes any editable controls with the current parameters.</summary>
		void displayEditControls()
		{
			beginUndoFrame(false);

			bool btemp = _loading;
			_loading = true;

			var so = getPrimarySelectedObject();

			// Note: path nodes are handled elsewhere.
			if (so != null)
			{
				// Reset active control list to empty and hidden.
				beginSidebarBuild();

				// The scrollbars aren't part of the sidebar but still need to be hidden.
				setMapScrollbarVisibility(false);

				if (isValidWaypoint(so))
				{
					addSidebarControl(pnlPosition);
					if (_selectedObjects.Count > 1)
					{
						addSidebarControl(pnlMoveMulti);
						optMoveRelative.Checked = true;
					}
					addSidebarControl(pnlMoveOptions);

					var afg = _flightgroups[so.Index];

					setEditPositionMaxRange();
					setEditPositionIncrement(0);
					setEditPositionValues(afg.Waypoints[getFlightgroupWaypoint()]);

					if (isXwing)
					{
						addSidebarControl(pnlCraftIcon);
						addSidebarControl(pnlCraftName);

						safeSetCbo(cboCraftType, afg.CraftType);
						safeSetCbo(cboIff, afg.CraftIff);
						txtCraftName.Text = afg.Name;
						txtCraftName.ReadOnly = false;
					}
					else
					{
						addSidebarControl(pnlCraftName);
						addSidebarControl(pnlShipState);

						// This is informational only, can't be edited.
						lblCraftName.Text = "Flightgroup #" + (so.Index + 1);
						txtCraftName.Text = afg.DisplayName;
						txtCraftName.ReadOnly = true;

						// Allows the user to set the waypoint state without having to use the main form.
						setRadio(optStateOn, afg.Waypoints[getFlightgroupWaypoint()].Enabled);
					}
				}
				else if (isValidEvent(so))
				{
					var evt = _events[so.Index];
					switch (evt.Event)
					{
						case AbstractEventType.TitleText:
						case AbstractEventType.CaptionText:
						case AbstractEventType.PanelText3:
						case AbstractEventType.PanelText4:
							addSidebarControl(pnlString);
							setEditString(BriefingString.String, evt.Params[0]);

							if (evt.Event == AbstractEventType.TitleText)
							{
								addSidebarControl(lblTempInfo);
								lblTempInfo.Text = "";
								if (isTitlePlatform)
								{
									if (!txtEditString.Text.StartsWith(">")) lblTempInfo.Text = "Prefix '>' to center horizontally";
								}
								else lblTempInfo.Text = "This event has no effect on this platform.";
							}
							break;

						case AbstractEventType.MoveMap:
							setShiftMode(true);
							setMoveZoomRectangles(evt.Time);
							_currentPosition = new Point(evt.Params[0], evt.Params[1]);
							_currentZoom = _mapShiftEndZoom;
							addSidebarControl(pnlPosition);
							addSidebarControl(pnlMoveOptions);
							if (isCurrentTab(MainTabIndex.BriefingMap))
							{
								addSidebarControl(pnlPanOptions);
								optPanFreeLook.Checked = true;
							}
							disableMoveOptions(true, true);

							setEditPositionMaxRange();
							setEditPositionIncrement(0);
							setEditPositionValues(evt.Params[0], evt.Params[1]);
							setMapScrollbarVisibility(true);
							setMapScrollbarMovePosition(evt.Params[0], evt.Params[1]);
							break;

						case AbstractEventType.ZoomMap:
							setShiftMode(true);
							setMoveZoomRectangles(evt.Time);
							_currentPosition = _mapShiftEndPos;
							_currentZoom = _mapShiftEndZoom;

							addSidebarControl(pnlPosition);
							addSidebarControl(chkLinkZoom);
							if (isCurrentTab(MainTabIndex.BriefingMap))
							{
								addSidebarControl(pnlPanOptions);
								optPanFreeLook.Checked = true;
							}

							setEditPositionZoomRange();
							setEditPositionIncrement(1);
							setEditPositionValues(evt.Params[0], evt.Params[1]);
							setMapScrollbarVisibility(true);
							setMapScrollbarZoomPosition(evt.Params[0], evt.Params[1]);
							break;

						case AbstractEventType.FgTag1:
						case AbstractEventType.FgTag2:
						case AbstractEventType.FgTag3:
						case AbstractEventType.FgTag4:
						case AbstractEventType.FgTag5:
						case AbstractEventType.FgTag6:
						case AbstractEventType.FgTag7:
						case AbstractEventType.FgTag8:
							addSidebarControl(pnlShipTag);

							if (isCurrentTab(MainTabIndex.BriefingMap))
							{
								addSidebarControl(lblTempInfo);
								lblTempInfo.Text = "Shift+click an icon on the map or choose one from the list.";
							}

							if (isXwa)
							{
								addSidebarControl(pnlGoto);
								lblGoto.Text = "Go to NewIcon";
								cmdGotoEvent.Tag = evt.UID;
							}

							int fgSlot = evt.Event - AbstractEventType.FgTag1;
							short[] fgUsage = getTagUsageArray(evt.Time, AbstractEventType.FgTag1, AbstractEventType.FgTag8, AbstractEventType.ClearFgTags);
							populateTagSlotDropdown(cboFgTagSlot, fgSlot, evt.Time, fgUsage);
							safeSetCbo(cboFgTagSlot, fgSlot);

							if (isXwa) populateIconDropdown(cboFGTagItem, -1, evt.Time, 0);
							else populateFlightgroupDropdown(cboFGTagItem, true);

							safeSetCbo(cboFGTagItem, evt.Params[0]);
							break;

						case AbstractEventType.TextTag1:
						case AbstractEventType.TextTag2:
						case AbstractEventType.TextTag3:
						case AbstractEventType.TextTag4:
						case AbstractEventType.TextTag5:
						case AbstractEventType.TextTag6:
						case AbstractEventType.TextTag7:
						case AbstractEventType.TextTag8:
							addSidebarControl(pnlTextTag);
							addSidebarControl(pnlString);
							addSidebarControl(pnlPosition);
							if (_selectedObjects.Count > 1)
							{
								addSidebarControl(pnlMoveMulti);
								optMoveRelative.Checked = true;
							}
							addSidebarControl(pnlMoveOptions);

							if (isCurrentTab(MainTabIndex.EventList)) disableMoveOptions(true, false);

							int textSlot = evt.Event - AbstractEventType.TextTag1;
							short[] textUsage = getTagUsageArray(evt.Time, AbstractEventType.TextTag1, AbstractEventType.TextTag8, AbstractEventType.ClearTextTags);
							populateTagSlotDropdown(cboTextTagSlot, textSlot, evt.Time, textUsage);
							safeSetCbo(cboTextTagSlot, textSlot);
							setEditPositionMaxRange();
							setEditPositionIncrement(-1);
							setEditPositionValues(evt.Params[1], evt.Params[2]);
							setEditString(BriefingString.Tag, evt.Params[0]);
							safeSetCbo(cboTextTagColor, evt.Params[3]);

							// Only allow multiselect editing of position and color.
							if (_selectedObjects.Count > 1)
							{
								cboTextTagSlot.Enabled = false;
								pnlString.Enabled = false;
							}
							break;

						case AbstractEventType.XwaSetIcon:
							_lastSelectedIconIndex = evt.Params[0];
							addSidebarControl(pnlIcon);
							addSidebarControl(pnlCraftIcon);

							if (_selectedObjects.Count == 1 && isCurrentTab(MainTabIndex.BriefingMap))
							{
								// Typically a New icon event is paired with a Move event to give it a proper position.
								// An uninitialized icon is still visible and selectable to the map, but it won't have
								// directional controls because it's not a Move event.
								addSidebarControl(lblTempInfo);
								if (!isIconInitialized(evt.Time, evt.Params[0]))
								{
									lblTempInfo.Text = "No corresponding Move event.  Icon at default position.";
									addSidebarControl(cmdCreateMove);
								}
								else lblTempInfo.Text = "Create or select a Move event to change the icon's position.";
							}

							populateIconDropdown(cboIcon, evt.Params[0], evt.Time, 1);
							cboIcon.Enabled = (getUniformIconSelection() == evt.Params[0]);
							safeSetCbo(cboIcon, evt.Params[0]);
							safeSetCbo(cboCraftType, evt.Params[1]);
							safeSetCbo(cboIff, evt.Params[2]);
							break;

						case AbstractEventType.XwaShipInfo:
							_lastSelectedIconIndex = evt.Params[1];
							addSidebarControl(pnlIcon);
							addSidebarControl(pnlShipState);
							addSidebarControl(pnlGoto);
							populateIconDropdown(cboIcon, evt.Params[1], hsbTimer.Value, 2);
							lblGoto.Text = "Go to Info " + (evt.Params[0] != 0 ? "End" : "Start");
							cmdGotoEvent.Tag = evt.UID;
							safeSetCbo(cboIcon, evt.Params[1]);
							setRadio(optStateOn, (evt.Params[0] != 0));
							break;

						case AbstractEventType.XwaMoveIcon:
							_lastSelectedIconIndex = evt.Params[0];
							_lastSelectedMoveIconUid = evt.UID;
							addSidebarControl(pnlIcon);
							addSidebarControl(pnlPosition);
							if (_selectedObjects.Count > 1)
							{
								addSidebarControl(pnlMoveMulti);
								optMoveRelative.Checked = true;
							}
							addSidebarControl(pnlMoveOptions);

							if (isCurrentTab(MainTabIndex.EventList)) disableMoveOptions(true, false);
							else if (isCurrentTab(MainTabIndex.BriefingMap)) addSidebarControl(pnlRotation);

							// Not enough vertical space to display the full set of movement controls, so only show this on the event tab.
							if (isCurrentTab(MainTabIndex.EventList))
							{
								addSidebarControl(pnlGoto);
								lblGoto.Text = "Go to NewIcon";
								cmdGotoEvent.Tag = evt.UID;
							}

							if (isCurrentTab(MainTabIndex.BriefingMap) && _selectedObjects.Count == 1) addSidebarControl(pnlIconMove);

							setEditPositionMaxRange();
							setEditPositionIncrement(-1);
							setEditPositionValues(evt.Params[1], evt.Params[2]);
							populateIconDropdown(cboIcon, evt.Params[0], evt.Time, 0);
							safeSetCbo(cboIcon, evt.Params[0]);

							// The rotation box lets us choose a rotation for this time index without needing to manually create a RotateIcon event.
							int rotation = 0;
							var elem = getShadowIconByUid(evt.UID);
							if (elem != null) rotation = elem.IconRotation;

							safeSetCbo(cboRotation, rotation);

							if (getUniformIconSelection() == -1) pnlIcon.Enabled = false;

							if (_selectedObjects.Count > 1)
							{
								pnlRotation.Enabled = false;
								pnlGoto.Enabled = false;
							}

							refreshIconAcceleration();
							break;

						case AbstractEventType.XwaRotateIcon:
							_lastSelectedIconIndex = evt.Params[0];
							addSidebarControl(pnlIcon);
							addSidebarControl(pnlRotation);
							addSidebarControl(pnlGoto);
							populateIconDropdown(cboIcon, evt.Params[0], evt.Time, 0);
							lblGoto.Text = "Go to NewIcon";
							cmdGotoEvent.Tag = evt.UID;
							safeSetCbo(cboIcon, evt.Params[0]);
							safeSetCbo(cboRotation, evt.Params[1]);
							break;

						case AbstractEventType.XwaChangeRegion:
							addSidebarControl(pnlRegion);
							safeSetCbo(cboRegion, evt.Params[0]);
							break;

						case AbstractEventType.XwaInfoParagraph:
							addSidebarControl(pnlShipState);
							addSidebarControl(pnlString);
							addSidebarControl(lblTempInfo);

							int specIndex = evt.Params[0] - 1;
							if (specIndex < 0)
							{
								optStateOff.Checked = true;
								specIndex = 0;
							}
							else optStateOn.Checked = true;

							setEditString(BriefingString.String, specIndex);
							pnlString.Enabled = optStateOn.Checked;
							lblTempInfo.Text = "Bracket [] and center > not supported.  Color codes 3-6 only.";
							break;
					}
				}

				// Finalize and show any controls that were added.
				endSidebarBuild();
			}
			else
			{
				setMapScrollbarVisibility(false);

				if (isDualEditingMoveZoom() && isCurrentTab(MainTabIndex.BriefingMap))
				{
					// Special case, editing a MoveMap and ZoomMap at the same time index.
					// Allows both of them to be modified with Free Look, as long as the option is checked.
					setShiftMode(true);
					setFreeLookMode(true);
					beginSidebarBuild();
					addSidebarControl(lblTempInfo);
					addSidebarControl(pnlPanOptions);
					var move = _events[getSelectedEventType(AbstractEventType.MoveMap).Index];
					var zoom = _events[getSelectedEventType(AbstractEventType.ZoomMap).Index];
					_currentPosition = new Point(move.Params[0], move.Params[1]);
					_currentZoom = new Point(zoom.Params[0], zoom.Params[1]);
					lblTempInfo.Text = "Can edit both Move and Zoom if This Event is checked";
					optPanFreeLook.Checked = true;
					setMoveZoomRectangles(move.Time);
					endSidebarBuild();
				}
				else if (_selectedObjects.Count > 0)
				{
					// Nothing selected, or they're different types and not compatible with a specific edit control.
					// Check for a grouping of selected icon events.  This allows the user to bulk change an icon index to something else.
					beginSidebarBuild();
					int iconIndex = getUniformIconSelection();
					if (iconIndex >= 0)
					{
						addSidebarControl(pnlIcon);
						populateIconDropdown(cboIcon, iconIndex, hsbTimer.Value, 0);
					}
					else
					{
						addSidebarControl(lblTempInfo);
						lblTempInfo.Text = "Multiple items selected, different types.";
					}
					endSidebarBuild();
				}
				else
				{
					// Nothing to show.
					resetSidebar();
				}
			}
			_loading = btemp;
		}

		/// <summary>Refreshes the editable parameters in the sidebar based on the current selection.</summary>
		/// <remarks>This typically applies when moving things directly on the map instead of using the location controls in the sidebar.</remarks>
		void refreshEditControls()
		{
			bool temp = _loading;
			_loading = true;

			var so = getPrimarySelectedObject();

			if (so != null)
			{
				if (isValidWaypoint(so))
				{
					var afg = _flightgroups[so.Index];
					var wp = afg.Waypoints[getFlightgroupWaypoint()];
					setEditPositionValues(wp);
					setRadio(optStateOn, wp.Enabled);
				}
				else if (isValidPathNode(so) && so.Index == lstPath.SelectedIndex) setEditPositionValues(_pathNodes[so.Index].X, _pathNodes[so.Index].Y);
				else if (isValidEvent(so))
				{
					AbstractEvent evt = _events[so.Index];
					if (evt.IsTextTag) setEditPositionValues(evt.Params[1], evt.Params[2]);
					else if (evt.Event == AbstractEventType.MoveMap || evt.Event == AbstractEventType.ZoomMap)
					{
						setEditPositionValues(evt.Params[0], evt.Params[1]);
						setEditScrollbarValues(evt.Params[0], evt.Params[1]);
					}
					else if (evt.Event == AbstractEventType.XwaMoveIcon) setEditPositionValues(evt.Params[1], evt.Params[2]);
				}
			}

			_loading = temp;
		}
		
		void resizeStringPanel(bool mapTab)
		{
			int heightDiff = 0;

			if (pnlString.Tag != null)
			{
				// We're switching to the map, or attempting to dynamically recalculate the vertical space
				// while on the event tab.  Restore the original sizes.
				Size[] old = (Size[])pnlString.Tag;
				heightDiff = old[2].Height + txtEditString.Height;
				pnlString.Size = old[0];
				pnlEditContainer.Width = pnlString.Width;
				cboEditString.Size = old[1];
				txtEditString.Size = old[2];
				pnlString.Tag = null;
			}

			if (!mapTab)
			{
				// The event list tab has extra width available for the sidebar, which may be helpful for
				// editing strings.  Store a backup of the original sizes, then modify them as needed.
				pnlString.Tag = new Size[3] { pnlString.Size, cboEditString.Size, txtEditString.Size };
				pnlString.Width = pnlEventListManage.Width;
				int newWidth = pnlEventListManage.Width - txtEditString.Margin.Right;
				pnlEditContainer.Width = newWidth;
				cboEditString.Width = newWidth;
				txtEditString.Width = newWidth;

				// In the event list tab, the event manager panel consumes a lot of vertical space.
				// The height available to the sidebar is reduced.  The text tag controls consume
				// the most vertical space.  Attempt to calculate the total height it requires.
				int expectedHeight = (pnlEventListManage.Bottom + pnlEventListManage.Margin.Bottom) +
								(pnlTextTag.Height + pnlString.Height + pnlPosition.Height + pnlMoveOptions.Height) +
								(pnlTextTag.Margin.Bottom * 4);  // Space between and below all panels

				// The first attempt is to place the position and move options side by side.
				// This will be performed when the sidebar build is finalized.
				if (expectedHeight > lstEvents.Bottom)
				{
					expectedHeight -= (pnlPosition.Height + pnlMoveOptions.Height + pnlTextTag.Margin.Bottom);
					expectedHeight += Math.Max(pnlPosition.Height, pnlMoveOptions.Height);
				}

				// If it still requires more space than is available, try to shrink the textbox.  This tries to
				// optimize the space as much as possible to prevent controls from getting cut off the bottom.
				if (expectedHeight > lstEvents.Bottom)
				{
					int fontHeight = txtEditString.Font.Height;
					int lines = txtEditString.ClientRectangle.Height / fontHeight;
					int maxOverflow = lines - 2;

					int overflowLines = (expectedHeight - lstEvents.Bottom) / fontHeight;
					overflowLines = clamp(overflowLines, 1, maxOverflow);

					heightDiff = -(overflowLines * fontHeight);
					txtEditString.Height += heightDiff;
					pnlString.Height += heightDiff;
				}
			}

			// If the textbox height changed, check if any controls are currently visible in the sidebar.
			// If they're below the textbox, change their vertical position accordingly.
			if (heightDiff != 0)
				foreach (Control c in pnlEditContainer.Controls) if (c.Visible && c.Top > txtEditString.Top) c.Top += heightDiff;
		}

		void transferSidebarPanels(bool mapTab)
		{
			if (mapTab && !tabDisplay.Controls.Contains(pnlEditContainer))
			{
				tabDisplay.SuspendLayout();
				tabDisplay.Controls.Add(pnlEditContainer);
				resizeStringPanel(true);
				pnlEditContainer.Top = pctBrief.Top;
				pnlEditContainer.Left = getIsolatedScrollbar(vsbTimeline).Right + 4;
				// Resets the height since it will have changed if the user switched to the Events tab, and fixes the map options to be where it should.
				pnlEditContainer.Height = tabBriefing.DisplayRectangle.Height - pnlEditContainer.Top;
				pnlMapOptions.Top = pnlEditContainer.Bottom - pnlMapOptions.Height;
				tabDisplay.ResumeLayout();
			}
			else if (!mapTab && !tabEvents.Controls.Contains(pnlEditContainer))
			{
				tabEvents.SuspendLayout();
				tabEvents.Controls.Add(pnlEditContainer);
				resizeStringPanel(false);
				pnlEditContainer.Left = pnlEventListManage.Left;
				pnlEditContainer.Top = pnlEventListManage.Bottom + pnlEventListManage.Margin.Bottom;
				pnlEditContainer.Height = tabEvents.Bottom - pnlEditContainer.Top;
				tabEvents.ResumeLayout();
			}
		}

		void transferXwingPanels(bool mapTab)
		{
			if (mapTab && !pnlPanelMode.Controls.Contains(pnlXwingPanelRect))
			{
				pnlPanelMode.Controls.Add(pnlXwingPanelRect);
				pnlPanelMode.Controls.Add(pnlXwingPanelLines);
				pnlXwingPanelRect.Location = (Point)pnlXwingPanelRect.Tag;
				pnlXwingPanelLines.Location = (Point)pnlXwingPanelLines.Tag;
			}
			else if (!mapTab && !grpXwingPageTemplates.Controls.Contains(pnlXwingPanelRect))
			{
				pnlXwingPanelRect.Tag = pnlXwingPanelRect.Location;
				pnlXwingPanelLines.Tag = pnlXwingPanelLines.Location;
				grpXwingPageTemplates.Controls.Add(pnlXwingPanelRect);
				grpXwingPageTemplates.Controls.Add(pnlXwingPanelLines);

				// From left to right: panel listbox, rectangle values, text lines.
				int top = lstXwingPageTypes.Top;
				pnlXwingPanelRect.Location = new Point(lstXwingPagePanels.Right + lstPanelMode.Margin.Right, top);
				pnlXwingPanelLines.Location = new Point(pnlXwingPanelRect.Right, top);
			}
		}

		/// <summary>Hides all sidebar panels that are used for editing properties of selected items.</summary>
		void hideSidebarPanels() { foreach (Control c in pnlEditContainer.Controls) c.Visible = false; }

		/// <summary>Prepares the sidebar by hiding all elements and clearing the list of enabled controls.</summary>
		/// <remarks>Use <see cref="addSidebarControl"/> to add controls to the list.</remarks>
		void beginSidebarBuild()
		{
			// The sidebar is extremely slow to update.  Not sure if this speeds it up, but taking anything we can get.
			pnlEditContainer.Visible = false;
			suspendLayouts(pnlEditContainer);

			hideSidebarPanels();

			if (pnlEditContainer.Tag == null) pnlEditContainer.Tag = new List<Control>();
			else ((List<Control>)pnlEditContainer.Tag).Clear();
		}

		/// <summary>Adds a control to the sidebar list.  Controls will be stacked vertically, spaced by their margins.</summary>
		/// <remarks>Use <see cref="endSidebarBuild"/> to finalize and display the list.</remarks>
		void addSidebarControl(Control c)
		{
			List<Control> list = (List<Control>)pnlEditContainer.Tag;
			
			// Depending on the user's item selection, entire panels or child controls may have been disabled.
			// Make sure everything is restored.
			c.Enabled = true;
			foreach (Control child in c.Controls) child.Enabled = true;

			// All appended controls are stacked vertically.
			int y = 0;
			int margin = 0;
			if (list.Count > 0)
			{
				y = list[list.Count - 1].Bottom;
				margin = list[list.Count - 1].Margin.Bottom;
			}

			margin = Math.Max(margin, c.Margin.Top);
			c.Location = new Point(0, y + margin);
			list.Add(c);
		}

		/// <summary>Finalizes and displays a sidebar by enabling visibility on all controls that have been added to the list.</summary>
		void endSidebarBuild()
		{
			List<Control> list = (List<Control>)pnlEditContainer.Tag;
			int bottom = 0;
			bool hasTextTag = false;
			foreach (Control c in list)
			{
				if (c == pnlTextTag) hasTextTag = true;
				c.Visible = true;
				bottom = Math.Max(bottom, c.Bottom);
			}

			if (isCurrentTab(MainTabIndex.EventList))
			{
				bottom += pnlEventListManage.Bottom + pnlEventListManage.Margin.Bottom;

				// Text tags consume a lot of vertical space.  The string edit panel is resized when the
				// tab changes, but if the form size is too small, the remaining panels may extend off
				// the bottom of the screen.  Change the two panels to be side by side instead.
				if (bottom > lstEvents.Bottom && hasTextTag)
				{
					pnlMoveOptions.Left = pnlPosition.Right + pnlPosition.Margin.Right;
					pnlMoveOptions.Top = pnlPosition.Top;
				}
			}

			resumeLayouts(pnlEditContainer);
			pnlEditContainer.Visible = true;
		}

		void disableMoveOptions(bool mouse, bool snap)
		{
			if (mouse)
			{
				chkMoveLockX.Enabled = false;
				chkMoveLockY.Enabled = false;
				chkMoveSnapMouse.Enabled = false;
			}
			if (snap)
			{
				chkMoveSnapGrid.Enabled = false;
				chkMoveSnapMouse.Enabled = false;
			}
		}
		#endregion Sidebar

		#region Data display and editing
		/// <summary>Modifies a parameter for all currently selected items.</summary>
		/// <param name="paramType">Parameter type to modify.</param>
		/// <param name="paramValue">New value to assign.</param>
		/// <param name="rebuild">Should always be true except when updating multiple parameters from a single edit.  Specify false to suppress a rebuild from interfering with UI values before the final parameter is updated.</param>
		void modifyParam(EditParamType paramType, int paramValue, bool rebuild = true)
		{
			// Ignore any control changes during a load.
			// Temp events pull values directly from the control rather than relying on update logic.
			if (_loading || _tempEvent != AbstractEventType.None || _selectedObjects.Count == 0) return;

			bool eventDirty = false;
			bool mapDirty = false;
			bool stringDirty = false;
			bool iconDirty = false;

			foreach (var so in _selectedObjects)
			{
				if (isValidWaypoint(so))
				{
					var afg = _flightgroups[so.Index];
					int wp = getFlightgroupWaypoint();

					if (paramType == EditParamType.X || paramType == EditParamType.Y || paramType == EditParamType.CraftInfoState)
					{
						updateUndoProperty(ref so.UndoUID, afg.GetDataSnapshot(), UndoType.FlightgroupMove, so.Index, afg.DisplayWaypoint);
						if      (paramType == EditParamType.X) afg.Waypoints[wp].RawX = (short)paramValue;
						else if (paramType == EditParamType.Y) afg.Waypoints[wp].RawY = (short)paramValue;
						else if (paramType == EditParamType.CraftInfoState) afg.Waypoints[wp].Enabled = (paramValue != 0);
						updateKnownUndoProperty(so.UndoUID, afg.GetDataSnapshot());
						notifyWaypointChange(so.Index);
						mapDirty = true;
					}
					else if (isXwing)
					{
						if (paramType == EditParamType.IconCraftType || paramType == EditParamType.IconCraftIff)
						{
							UndoOperation op = new UndoOperation(UndoType.FlightgroupData, afg.GetDataSnapshot(), so.Index);
							if      (paramType == EditParamType.IconCraftType) afg.CraftType = paramValue;
							else if (paramType == EditParamType.IconCraftIff)  afg.CraftIff = paramValue;
							op.Update(afg.GetDataSnapshot());
							addUpdatedUndoOperation(op, false);
							notifyFlightgroupChange(so.Index);
							mapDirty = true;
						}
					}
				}
				else if (isValidEvent(so))
				{
					var evt = _events[so.Index];
					var before = evt.GetDataSnapshot();
					bool update = false;

					switch (evt.Event)
					{
						case AbstractEventType.TitleText:
						case AbstractEventType.CaptionText:
						case AbstractEventType.PanelText3:
						case AbstractEventType.PanelText4:
							if (paramType == EditParamType.String) { evt.Params[0] = (short)paramValue; update = true;	stringDirty = true; }
							break;

						case AbstractEventType.MoveMap:
						case AbstractEventType.ZoomMap:
							if      (paramType == EditParamType.X) { evt.Params[0] = (short)paramValue; update = true; }
							else if (paramType == EditParamType.Y) { evt.Params[1] = (short)paramValue; update = true; }
							break;

						case AbstractEventType.FgTag1:
						case AbstractEventType.FgTag2:
						case AbstractEventType.FgTag3:
						case AbstractEventType.FgTag4:
						case AbstractEventType.FgTag5:
						case AbstractEventType.FgTag6:
						case AbstractEventType.FgTag7:
						case AbstractEventType.FgTag8:
							if      (paramType == EditParamType.FgIndex)      { evt.Params[0] = (short)paramValue; update = true; }
							else if (paramType == EditParamType.TagEventType) { evt.Event = AbstractEventType.FgTag1 + paramValue; update = true; }
							break;

						case AbstractEventType.TextTag1:
						case AbstractEventType.TextTag2:
						case AbstractEventType.TextTag3:
						case AbstractEventType.TextTag4:
						case AbstractEventType.TextTag5:
						case AbstractEventType.TextTag6:
						case AbstractEventType.TextTag7:
						case AbstractEventType.TextTag8:
							if      (paramType == EditParamType.String)    { evt.Params[0] = (short)paramValue; update = true; }
							else if (paramType == EditParamType.X)         { evt.Params[1] = (short)paramValue; update = true; }
							else if (paramType == EditParamType.Y)         { evt.Params[2] = (short)paramValue; update = true; }
							else if (paramType == EditParamType.TextColor) { evt.Params[3] = (short)paramValue; update = true; }
							else if (paramType == EditParamType.TagEventType) { evt.Event = AbstractEventType.TextTag1 + paramValue; update = true; }
							break;

						case AbstractEventType.XwaSetIcon:
							if      (paramType == EditParamType.IconIndex)     { evt.Params[0] = (short)paramValue; update = true; iconDirty = true; }
							else if (paramType == EditParamType.IconCraftType) { evt.Params[1] = (short)paramValue; update = true; iconDirty = true; }
							else if (paramType == EditParamType.IconCraftIff)  { evt.Params[2] = (short)paramValue; update = true; iconDirty = true; }
							break;

						case AbstractEventType.XwaShipInfo:
							if (paramType == EditParamType.CraftInfoState) { evt.Params[0] = (short)paramValue; update = true; }
							else if (paramType == EditParamType.IconIndex) { evt.Params[1] = (short)paramValue; update = true; }
							break;

						case AbstractEventType.XwaMoveIcon:
							if (paramType == EditParamType.IconIndex) { evt.Params[0] = (short)paramValue; update = true; iconDirty = true; }
							else if (paramType == EditParamType.X)    { evt.Params[1] = (short)paramValue; update = true; iconDirty = true; }
							else if (paramType == EditParamType.Y)    { evt.Params[2] = (short)paramValue; update = true; iconDirty = true; }
							break;

						case AbstractEventType.XwaRotateIcon:
							if      (paramType == EditParamType.IconIndex)         { evt.Params[0] = (short)paramValue; update = true; iconDirty = true; }
							else if (paramType == EditParamType.IconCraftRotation) { evt.Params[1] = (short)paramValue; update = true; iconDirty = true; }
							break;

						case AbstractEventType.XwaChangeRegion:
							if (paramType == EditParamType.Region) { evt.Params[0] = (short)paramValue; update = true; }
							break;

						case AbstractEventType.XwaInfoParagraph:
							if (paramType == EditParamType.String) { evt.Params[0] = (short)paramValue; update = true; }
							break;
					}
					if (update)
					{
						eventDirty = true;
						_lastEditEventUid = evt.UID;

						UndoOperation op = new UndoOperation(UndoType.Event, before, so.Index);
						op.Update(evt.GetDataSnapshot());
						addUpdatedUndoOperation(op, false);
					}
				}
			}

			markDirty();

			if (isEditingEventType(AbstractEventType.MoveMap) || isEditingEventType(AbstractEventType.ZoomMap))
			{
				setMoveZoomRectangles(_events[_selectedObjects[0].Index].Time);
				mapDirty = true;
			}

			// Old code tried to optimize by running a single event, but there were too many edge cases where
			// a rebuild was necessary to avoid visual bugs.  It's easier to always rebuild.
			if (eventDirty && rebuild)
			{
				mapDirty = true;
				rebuildBriefing();
			}

			if (mapDirty)
			{
				refreshMap();
				refreshTimeline();
			}

			if (iconDirty)
				rebuildShadowIcons();

			if (stringDirty)
			{
				// A title and caption could be using the same string, so update both.
				refreshCanvas(RefreshFlags.Title | RefreshFlags.Caption);

				if (isXwing) refreshCanvas(RefreshFlags.Panel3 | RefreshFlags.Panel4);
			}

			if (isCurrentTab(MainTabIndex.EventList)) refreshEventListSelected();
		}

		void setEditString(BriefingString stringType, int index)
		{
			bool btemp = _loading;
			_loading = true;

			List<string> container = getStringList(stringType);

			_editStringCharLimit = (stringType == BriefingString.String) ? _maxStringLength : _maxTagLength;

			string s = "";
			if (index >= 0 && index < container.Count) s = container[index];
			else index = -1;

			// This function is called whenever a different string is selected from the list, or when the
			// string edit panel is activated.  A single undo state will be used to track all changes.
			UndoType undoType = (stringType == BriefingString.String ? UndoType.String : UndoType.Tag);
			UndoOperation op = new UndoOperation(undoType, s, index);
			addUndoOperation(op, false);
			_editStringUndoUid = op.UID;

			if (stringType != _editStringType) populateEditStrings(container);

			cboEditString.SelectedIndex = index;
			txtEditString.Text = s;

			_editStringType = stringType;
			_editStringIndex = index;

			// If a caption string is too long, some of it is cut off in the textbox and it's not immediately clear that
			// it's happening.  Ideally there should be a scrollbar, but there isn't an auto hide option for textboxes.
			// Horizontal space is limited, and a scrollbar constrains it further.  This attempts to calculate whether
			// the string is long enough to fit inside the textbox and display the scrollbar only when necessary.
			// The presence of the scrollbar reduces the reported client space.
			// Using the control size instead of the client size seems to produce more accurate wordwrapping results.
			// It's close, but maybe not perfect.
			txtEditString.ScrollBars = ScrollBars.None;
			Size sz = TextRenderer.MeasureText(s, txtEditString.Font, new Size(txtEditString.Width + 1, txtEditString.Height), TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
			if (sz.Height >= txtEditString.ClientSize.Height) txtEditString.ScrollBars = ScrollBars.Vertical;
			
			refreshEditStringVisual();
			refreshEditStringLabel(s);

			_loading = btemp;
		}

		/// <summary>Rebuilds the string/tag dropdown list with the specified container.</summary>
		void populateEditStrings(List<string> container)
		{
			bool btemp = _loading;
			_loading = true;

			int oldIndex = cboEditString.SelectedIndex;
			cboEditString.BeginUpdate();
			cboEditString.Items.Clear();
			for (int i = 0; i < container.Count; i++) cboEditString.Items.Add($"#{i + 1}: {container[i]}");
			cboEditString.EndUpdate();
			safeSetCbo(cboEditString, oldIndex);

			_loading = btemp;
		}

		void updateEditString(string s)
		{
			var container = getStringList(_editStringType);

			if (_editStringIndex >= 0 && _editStringIndex < container.Count)
			{
				container[_editStringIndex] = s;

				bool btemp = _loading;
				_loading = true;
				cboEditString.Items[_editStringIndex] = "#" + (_editStringIndex + 1) + ": " + s;
				_loading = btemp;

				updateKnownUndoProperty(_editStringUndoUid, s);
				markDirty();
			}

			refreshEditStringVisual();
			refreshEditStringLabel(s);

			if (isCurrentTab(MainTabIndex.EventList))
			{
				// Multiple events may be using the same string index, so update everything.
				for (int i = 0; i < _events.Count; i++)
				{
					var evt = _events[i];
					if ((_editStringType == BriefingString.String && evt.IsAnyCaption) || (_editStringType == BriefingString.Tag && evt.IsTextTag)) refreshEventList(i);
				}
			}
		}

		List<string> getStringList(BriefingString stringType) => (stringType == BriefingString.String ? _strings : _tags);

		void revertString(BriefingString stringType, int index, string s)
		{
			bool btemp = _loading;
			_loading = true;

			List<string> container = getStringList(stringType);
			if (index >= 0 && index < container.Count) container[index] = s;

			// If the string belongs to the current list, need to refresh.
			if (stringType == _editStringType) populateEditStrings(container);

			if (index == _editStringIndex) txtEditString.Text = s;

			refreshCanvas(RefreshFlags.Title);
			refreshCanvas(RefreshFlags.Caption);

			_loading = btemp;
		}

		void refreshEditStringLabel(string s)
		{
			int length = getUnescapedStringLength(s);
			lblEditStringType.Text = (_editStringType == BriefingString.Tag ? "Tag:" : "Caption:");
			lblEditStringInfo.Text = length + " / " + _editStringCharLimit + " Chars";
			lblEditStringInfo.ForeColor = (length <= _editStringCharLimit ? SystemColors.ControlText : Color.Red);
		}

		/// <summary>Refreshes any relevant canvases after a string or tag was changed.</summary>
		void refreshEditStringVisual()
		{
			AbstractEventType[] eventTypes = new AbstractEventType[4] { AbstractEventType.TitleText, AbstractEventType.CaptionText, AbstractEventType.PanelText3, AbstractEventType.PanelText4 };
			RefreshFlags[] flags = new RefreshFlags[4] { RefreshFlags.Title, RefreshFlags.Caption, RefreshFlags.Panel3, RefreshFlags.Panel4 };

			if (_tempEvent == AbstractEventType.None)
			{
				if (_editStringType == BriefingString.String)
				{
					for (int i = 0; i < 4; i++)
					{
						if (!_panelStringEnabled[i]) continue;

						int eventIndex = getEventIndexByUid(_panelStringEventUid[i]);
						if (eventIndex >= 0)
						{
							var evt = _events[eventIndex];
							if (evt.Event == eventTypes[i] && evt.Params[0] == _panelStringIndex[i]) refreshCanvas(flags[i]);
						}
					}

					if (_activeShipInfoBlock && _shipInfoStringIndex == _editStringIndex) refreshMap();
				}
				else refreshMap(); // Tag type, most likely visible if it's being edited.
			}
			else if ((_tempEvent >= AbstractEventType.TextTag1 && _tempEvent <= AbstractEventType.TextTag8) || _tempEvent == AbstractEventType.XwaInfoParagraph) refreshMap();
			else for (int i = 0; i < 4; i++) if (_tempEvent == eventTypes[i]) refreshCanvas(flags[i]);
		}

		/// <summary>Used for refreshing the X and Y position labels above the map position numeric controls.</summary>
		void refreshPositionLabel(string text, Label label, NumericUpDown num)
		{
			// Zoom will have a minimum set.
			// Anything else can be assumed to be a position, so include a unit conversion to kilometers.
			if (num.Minimum == 1) label.Text = text;
			else
			{
				// NOTE: For all platforms with briefing waypoints, the grid is based on miles, so 256
				// units (1.6km) = 1 cell on grid.  In XWA, nothing is holding it to any specific unit.
				// For the sake of mission conversions and legacy editors, we'll keep them the same.
				label.Text = string.Format("{0}  {1:F2}", text, (float)num.Value / 160.0f);
			}
		}

		void setEditPositionValues(int x, int y)
		{
			bool temp = _loading;
			_loading = true;
			safeSetNumeric(numX, x);
			safeSetNumeric(numY, y);
			_loading = temp;

			refreshPositionLabel("X:", lblX, numX);
			refreshPositionLabel("Y:", lblY, numY);
		}
		void setEditPositionValues(Point p) => setEditPositionValues(p.X, p.Y);
		void setEditPositionValues(BaseFlightGroup.Waypoint wp) => setEditPositionValues(wp.RawX, wp.RawY);

		void setEditScrollbarValues(int x, int y)
		{
			bool temp = _loading;
			_loading = true;

			if (_tempEvent == AbstractEventType.MoveMap || _tempEvent == AbstractEventType.ZoomMap)
			{
				// The scrollbar min/max values are initialized relative to the original position.
				// The user probably doesn't need to scroll to the very edge of the map.
				// This isn't a hard limit, so adjust the range if the user has tried to scroll beyond via mouse look or numeric control.
				if (x < hsbBRF.Minimum && x >= short.MinValue) hsbBRF.Minimum = x;
				if (x > hsbBRF.Maximum && x <= short.MaxValue) hsbBRF.Maximum = x;

				if (y < vsbBRF.Minimum && y >= short.MinValue) vsbBRF.Minimum = y;
				if (y > vsbBRF.Maximum && y <= short.MaxValue) vsbBRF.Maximum = y;
			}
			
			safeSetScrollbar(hsbBRF, x);
			safeSetScrollbar(vsbBRF, y);
			_loading = temp;
		}
		void setEditScrollbarValues(Point p) => setEditScrollbarValues(p.X, p.Y);

		void setEditPositionMaxRange()
		{
			numX.Minimum = short.MinValue;
			numX.Maximum = short.MaxValue;

			numY.Minimum = short.MinValue;
			numY.Maximum = short.MaxValue;
		}

		void setEditPositionZoomRange()
		{
			numX.Minimum = 1;
			numX.Maximum = short.MaxValue;

			numY.Minimum = 1;
			numY.Maximum = short.MaxValue;
		}

		void setMapScrollbarVisibility(bool state)
		{
			getIsolatedScrollbar(hsbBRF).Visible = state;
			getIsolatedScrollbar(vsbBRF).Visible = state;
			hsbBRF.Visible = state;
			vsbBRF.Visible = state;
		}

		void setMapScrollbarMovePosition(int x, int y)
		{
			float rawPerPixX = 256.0f / _currentZoom.X;
			float rawPerPixY = 256.0f / _currentZoom.Y;

			float gridCountX = (float)_mapCanvas.Width / _currentZoom.X;
			float gridCountY = (float)_mapCanvas.Height / _currentZoom.Y;

			int spanX = 2 * (int)(gridCountX * _currentZoom.X * rawPerPixX);
			int spanY = 2 * (int)(gridCountY * _currentZoom.Y * rawPerPixY);

			int minx = x - spanX;
			if (minx < short.MinValue) minx = short.MinValue;

			int maxx = x + spanX;
			if (maxx > short.MaxValue) maxx = short.MaxValue;

			int miny = y - spanY;
			if (miny < short.MinValue) miny = short.MinValue;

			int maxy = y + spanY;
			if (maxy > short.MaxValue) maxy = short.MaxValue;

			hsbBRF.Minimum = minx;
			hsbBRF.Maximum = maxx;
			safeSetScrollbar(hsbBRF, x);
			hsbBRF.SmallChange = (int)rawPerPixX;
			hsbBRF.LargeChange = (int)(rawPerPixX * (_currentZoom.X / 2.0f));

			vsbBRF.Minimum = miny;
			vsbBRF.Maximum = maxy;
			safeSetScrollbar(vsbBRF, y);
			vsbBRF.SmallChange = (int)rawPerPixY;
			vsbBRF.LargeChange = (int)(rawPerPixY * (_currentZoom.Y / 2.0f));
		}

		void setMapScrollbarZoomPosition(int x, int y)
		{
			int maxx = x + 128;
			if (maxx < 256) maxx = 256;

			int maxy = y + 128;
			if (maxy < 256) maxy = 256;

			hsbBRF.Minimum = 1;
			hsbBRF.Maximum = maxx;
			safeSetScrollbar(hsbBRF, x);
			hsbBRF.SmallChange = 1;
			hsbBRF.LargeChange = 2;

			vsbBRF.Minimum = 1;
			vsbBRF.Maximum = maxy;
			safeSetScrollbar(vsbBRF, y);
			vsbBRF.SmallChange = 1;
			vsbBRF.LargeChange = 2;
		}

		void setEditPositionIncrement(int value)
		{
			if (value <= 0)
			{
				// Not specified, assign from current dropdown selection.
				int index = cboMoveIncrement.SelectedIndex;
				if (index >= 0 && index <= 7)
				{
					// Pixel 1 to 8
					numX.Increment = Math.Max(1, getPixelRawX() * (index + 1));
					numY.Increment = Math.Max(1, getPixelRawY() * (index + 1));
				}
				else if (index >= 8 && index <= 16)
				{
					// [8]    [9]    [10]  [11]  [12]  [13]  [14]  [15]  [16]
					// 1/256, 1/128, 1/64, 1/32, 1/16, 1/8,  1/4,  1/2,  1/1   Grid
					// Convert to grid spacing based on fraction.  Grid is based on miles, not kilometers.
					int shift = index - 8;
					numX.Increment = 1 << shift;
					numY.Increment = 1 << shift;
				}
				else if (index == 17)
				{
					// Use the XWA MoveIcon increments instead.
					numX.Increment = Math.Max(1, numIconStepX.Value);
					numY.Increment = Math.Max(1, numIconStepY.Value);
				}
			}
			else
			{
				numX.Increment = value;
				numY.Increment = value;
			}
		}
		#endregion Data display and editing

		#region Time shift
		/// <summary>Retrieves the list of strings to populate the time shift dropdowns.</summary>
		string[] getTimeShiftIncrementStrings() => new string[] { "1 frame", "1/4 sec", "1/2 sec", "1 sec", "5 sec", "30 sec" };

		/// <summary>Retrieves a time shift increment amount, in briefing map ticks, from a dropdown containing a selection of available options.</summary>
		int getTimeShiftIncrement(ComboBox cbo)
		{
			switch (cbo.SelectedIndex)
			{
				case 0: return 1;
				case 1: return _ticksPerSecond / 4;
				case 2: return _ticksPerSecond / 2;
				case 3: return _ticksPerSecond;
				case 4: return _ticksPerSecond * 5;
				case 5: return _ticksPerSecond * 30;
			}
			return 1;
		}
		
		void shiftSelectedTimelineItems()
		{
			int stepX = (_mouse2.X - _mouse1.X) / _timelineColumnWidth;
			if (stepX != 0)
			{
				// Full time shift, moving everything on the timeline starting with the selected item.
				int lowest = _events.Count;
				foreach (var so in _selectedObjects) if (isValidEvent(so) && so.Index < lowest) lowest = so.Index;
				if (lowest != _events.Count) timeShift(lowest, stepX, true);

				_mouse1.X += stepX * _timelineColumnWidth;
			}
			_mouse1.Y = _mouse2.Y;
		}

		// Unlike the other move function, this drags an item across the timeline, only affecting the event's time.
		void moveSelectedTimelineItems()
		{
			int stepX = (_mouse2.X - _mouse1.X) / _timelineColumnWidth;
			if (stepX != 0)
			{
				// Horizontal drag.  Shift through time.
				moveEventsAcrossTime(stepX, true);
				_mouse1.X += stepX * _timelineColumnWidth;
			}

			int stepY = (_mouse2.Y - _mouse1.Y) / _timelineFont.Height;
			if (stepY != 0)
			{
				// Vertical drag.  Shift ordering relative to other events occupying the same time index.
				moveEventsInTime(stepY);
				_mouse1.Y += stepY * _timelineFont.Height;
			}
		}

		void timeShift(int startEventIndex, int tickOffset, bool makeUndo)
		{
			if (startEventIndex < 0 || startEventIndex >= _events.Count || tickOffset == 0) return;

			int startTime = _events[startEventIndex].Time;
			int lowerTime = 0;
			int upperTime = startTime;
			for (int i = 0; i < _events.Count; i++)
			{
				if (i < startEventIndex) lowerTime = _events[i].Time;
				else if (i > startEventIndex) upperTime = _events[i].Time;
			}

			// If running as revert operation, use the step exactly as specified.
			int step = tickOffset;
			if (makeUndo)
			{
				// Determine how far we can shift.  We can't shift the starting event below the next lowest event.
				// We also can't push the last event beyond the max time.
				if (tickOffset < 0 && startTime + tickOffset < lowerTime) step = -(startTime - lowerTime);
				else if (tickOffset >= 0 && upperTime + tickOffset > MaxEventTime) step = MaxEventTime - upperTime;
			}
			if (step == 0) return;

			if (makeUndo)
			{
				// If the most recent shift is still in the current frame, check if they should remain together so
				// that a revert operation can process them in bulk.  If clicking several times to shift, it can
				// be undone in a single command rather than needing to revert every click.  But if the start point
				// of the shift, or shift amount changes, generate a new frame.
				var lastOp = getUndoOperation(_lastTimeShiftUndoUid);
				if (lastOp == null || lastOp.Index != startEventIndex || step != (int)lastOp.NewData) beginUndoFrame(true);
			}

			for (int i = startEventIndex; i < _events.Count; i++) _events[i].Time += (short)step;

			if (makeUndo)
			{
				// The "old" value will be the inverted step to return to its previous time.
				UndoOperation op = new UndoOperation(UndoType.TimeShift, -step, startEventIndex);
				op.Update(step);
				addUpdatedUndoOperation(op, false);
				_lastTimeShiftUndoUid = op.UID;

				// While we're here, also check the end time.
				// If an undo operation is created, it will run its revert separately.
				autoAdjustDuration(true);
			}

			relinkSelectedEventUid();
			rebuildBriefing();
			rebuildShadowIcons();
			refreshMap();
			refreshTimeline();

			if (isCurrentTab(MainTabIndex.EventList))
			{
				refreshEventListRange(startEventIndex, _events.Count - 1);
				applySelectionToEventList();
				refreshEventListMoveButtons();
			}
		}

		void timelineShift(int tickOffset)
		{
			int currentTime = hsbTimer.Value;

			// Running from the timeline, locate the first event that would be impacted by a shift.
			for (int i = 0; i < _events.Count; i++)
			{
				if (_events[i].Time < currentTime) continue;

				// If removing time, clamp the offset so it doesn't push below the currently viewed time.
				if (tickOffset < 0 && _events[i].Time + tickOffset < currentTime) tickOffset = -(_events[i].Time - currentTime);
				timeShift(i, tickOffset, true);
				return;
			}
		}

		/// <summary>Retrieve the target event index for a move operation across time.</summary>
		int getShiftedIndexAtTime(int timeIndex, bool firstInGroup, int stepDirection)
		{
			int exact = -1;
			int lowerBound = -1;
			int upperBound = -1;
			for (int i = 0; i < _events.Count; i++)
			{
				int evtTime = _events[i].Time;
				if (evtTime < timeIndex)
				{
					// The largest index below the target time is always the best candidate if no better options are found.
					lowerBound = i;
				}
				else if (evtTime == timeIndex)
				{
					exact = i;
					if (firstInGroup) break;
				}
				else
				{
					// Time is greater, nothing else to check.
					upperBound = i;
					break;
				}
			}

			if (exact >= 0)
			{
				// Typically Page Break is the only exception, automatically placed first.  But it was off by one, so it needs this adjustment.
				if (firstInGroup) exact--;

				// Depending on which side that prospective elements are being shifted from, it needs a correction.
				if (stepDirection >= 0) return exact;
				
				return exact + 1;
			}

			if (stepDirection > 0 && lowerBound >= 0) return lowerBound;

			if (stepDirection < 0 && upperBound >= 0) return upperBound;

			// Failed to find anything
			return 0;
		}
		
		bool moveEventIndex(int curIndex, int destIndex, bool createUndo)
		{
			if (curIndex == destIndex || curIndex < 0 || destIndex < 0 || curIndex >= _events.Count || destIndex >= _events.Count) return false;

			int step = (destIndex < curIndex) ? -1 : 1;
			int pos = curIndex;
			while (pos != destIndex)
			{
				int nextPos = pos + step;
				if (createUndo)
				{
					// Undo operations use the data property, so assign both old and new indices.
					// The event data doesn't matter here.
					UndoOperation undo = new UndoOperation(UndoType.SwapEvent, pos, 0);
					undo.Update(nextPos);
					addUpdatedUndoOperation(undo, false);
				}
				_events[pos].Swap(_events[nextPos]);
				pos = nextPos;
			}
			return true;
		}
		void moveEventsInTime(int step)
		{
			if (step == 0) return;

			var results = getSelectedEvents();
			if (results.Count == 0) return;

			// If moving in a positive direction, sort highest to lowest.
			if (step > 0) results.Sort(SelectedObject.CompareDescending);
			else results.Sort(SelectedObject.Compare);

			// It greatly simplifies things if we ensure that all events occupy the same time.
			int firstTime = _events[results[0].Index].Time;
			int lowestIndex = -1;
			int highestIndex = -1;
			
			getEventIndexRangeAtTime(firstTime, out lowestIndex, out highestIndex);

			// If there's only a single event there, nothing to move.
			if (lowestIndex == highestIndex) return;

			for (int i = 0; i < results.Count; i++)
			{
				int eventIndex = results[i].Index;

				if (_events[eventIndex].Time != firstTime) 	return;

				// Clamp the step so we don't overshoot in any direction.
				if (step < 0)
				{
					if (step <= lowestIndex - eventIndex) step = lowestIndex - eventIndex;
				}
				else
				{
					if (step >= highestIndex - eventIndex) step = highestIndex - eventIndex;
				}
				
				// If the item is at the edge, there's nothing to move.
				if (step == 0) return;
			}

			for (int i = 0; i < results.Count; i++)
			{
				int eventIndex = results[i].Index;
				var evt = _events[eventIndex];

				int destIndex = eventIndex + step;
				if (destIndex < lowestIndex) destIndex = lowestIndex;
				else if (destIndex > highestIndex) destIndex = highestIndex;

				if (!moveEventIndex(eventIndex, destIndex, true)) break;
			}

			markDirty();
			relinkSelectedEventUid();
			rebuildShadowIcons();
			rebuildBriefing();
			refreshMap();
			refreshTimeline();
		}

		void moveEventsAcrossTime(int step, bool allowCross)
		{
			if (step == 0) return;

			var results = getSelectedEvents();
			if (results.Count == 0) return;

			// No matter which direction to move, move in sequence from lowest to highest.
			// This way the ordering is preserved as much as possible.
			results.Sort(SelectedObject.Compare);

			int lowBoundaryTime = 0;
			int highBoundaryTime = MaxEventTime;
			if (!allowCross)
			{
				int lowIndex = 9999;
				int highIndex = 0;
				for (int i = 0; i < results.Count; i++)
				{
					int eventIndex = results[i].Index;

					if (eventIndex < lowIndex) lowIndex = eventIndex;
					if (eventIndex > highIndex) highIndex = eventIndex;

					if (lowIndex > 0) lowBoundaryTime = _events[lowIndex - 1].Time + 1;
					if (highIndex < _events.Count - 1) highBoundaryTime = _events[highIndex + 1].Time - 1;
				}
			}

			int shift = 0;

			for (int i = 0; i < results.Count; i++)
			{
				// Shifting anything in this process will ruin all the indices, so ensure the correct event by UID.
				int eventIndex = getEventIndexByUid(results[i].EventUID);

				var evt = _events[eventIndex];

				if (shift == 0)
				{
					// If we can't cross an existing time, calculate the maximum time we can jump
					// All remaining items will use the same amount.
					shift = step;
					if (step > 0 && evt.Time + step > highBoundaryTime) shift = highBoundaryTime - evt.Time;    // positive result
					else if (step < 0 && evt.Time + step < lowBoundaryTime) shift = lowBoundaryTime - evt.Time;     // negative result

					// If we can't move at all, that's the end of it.
					if (shift == 0) break;
				}

				// Important to check the destination index before changing the time, because that
				// would interfere with the event search.
				int newTime = evt.Time + shift;
				bool first = false;
				if (evt.Event == AbstractEventType.PageBreak || evt.Event == AbstractEventType.SkipMarker) first = true;

				int newIndex = getShiftedIndexAtTime(newTime, first, step);

				// Create undo and change the time before swapping.
				UndoOperation op = new UndoOperation(UndoType.Event, evt.GetDataSnapshot(), eventIndex);
				evt.Time = (short)newTime;
				op.Update(evt.GetDataSnapshot());
				addUpdatedUndoOperation(op, false);

				if (newIndex != eventIndex)
				{
					// Try to move, halt the entire operation if a failure occurred.
					if (!moveEventIndex(eventIndex, newIndex, true)) break;
				}

			}

			autoAdjustDuration(true);

			markDirty();
			relinkSelectedEventUid();
			rebuildShadowIcons();
			rebuildBriefing();
			refreshMap();
			refreshTimeline();
		}
		#endregion Time shift

		#region Temp events
		/// <summary>Toggles the Enabled state of the temp event buttons on the map tab, based on whether the selected briefing page has a map or not.</summary>
		void refreshXwingTempCreateButtons()
		{
			bool isMap = getXwingPanel(Platform.Xwing.PageTemplate.Elements.Map).IsVisible;
			cmdClear.Enabled = isMap;
			cmdFG.Enabled = isMap;
			cmdText.Enabled = isMap;
			cmdMove.Enabled = isMap;
			cmdZoom.Enabled = isMap;
			cmdNewShip.Enabled = isMap;
		}

		void beginTempEvent(AbstractEventType eventType)
		{
			pauseBriefing();

			// Display an informational message in the title bar.
			// For XWING, make sure title panel is visible and large enough.
			bool info = eventType != AbstractEventType.TitleText;
			if (isXwing)
			{
				var title = getXwingPanel(Platform.Xwing.PageTemplate.Elements.Title);
				if (!title.IsVisible || title.Right - title.Left < 180) info = false;
			}
			if (info)
			{
				string evType = (eventType == AbstractEventType.XwingNewIcon ? "icon" : "event");
				setTitleOverride($"Finish the {evType} or click Cancel", 0);
			}

			if (eventType != AbstractEventType.MoveMap && eventType != AbstractEventType.ZoomMap && _freeLookMode)
			{
				setFreeLookMode(false);
				// Some events rely on checking the map for empty space.
				// Need to force the map and all the interacts to update after exiting free look.
				drawMap();
			}

			// Prevent the user from scrolling the time while temp creation is active.
			hsbTimer.Enabled = false;
			cmdOk.Enabled = true;
			cmdCancel.Enabled = true;

			_tempEvent = eventType;

			Point p;
			int tagIndex;

			_loading = true;
			beginSidebarBuild();

			switch (eventType)
			{
				// PageBreaks and Markers don't have parameters, not used here.
				case AbstractEventType.CaptionText:
				case AbstractEventType.TitleText:
				case AbstractEventType.PanelText3:
				case AbstractEventType.PanelText4:
					addSidebarControl(pnlString);
					setEditString(BriefingString.String, getAutoStringSlot(BriefingString.String));

					if (eventType == AbstractEventType.TitleText)
					{
						// New title events will default to the current title.
						// If there's a PageBreak, the string will be cleared, but the index will be available.
						if (_panelStringIndex[PANEL_TITLE] >= 0) setEditString(BriefingString.String, _panelStringIndex[PANEL_TITLE]);

						addSidebarControl(lblTempInfo);
						lblTempInfo.Text = "";
						if (isTitlePlatform)
						{
							if (!txtEditString.Text.StartsWith(">")) lblTempInfo.Text = "Prefix '>' to center horizontally";
						}
						else lblTempInfo.Text = "This event has no effect on this platform.";
					}

					if (isTitlePlatform || eventType == AbstractEventType.CaptionText)
					{
						addSidebarControl(chkAutoPageBreak);
						chkAutoPageBreak.Enabled = (hsbTimer.Value > 0);
					}
					if (isTitlePlatform && eventType != AbstractEventType.TitleText)
					{
						addSidebarControl(chkAutoTitle);
						chkAutoTitle.Enabled = (_panelStringIndex[PANEL_TITLE] >= 0 && getEventAtTime(AbstractEventType.TitleText, hsbTimer.Value, out _) == null);
					}
					break;

				case AbstractEventType.MoveMap:
					if (!_freeLookMode) _backupPosition = _currentPosition;
					addSidebarControl(pnlPosition);
					addSidebarControl(pnlMoveOptions);
					if (_freeLookMode && _currentZoom != _backupZoom)
					{
						addSidebarControl(lblTempInfo);
						lblTempInfo.Text = "Using free look, will combine a Zoom event at the current scale.";
					}
					disableMoveOptions(true, true);
					setEditPositionMaxRange();
					setEditPositionIncrement(0);
					setEditPositionValues(_currentPosition);
					setMapScrollbarVisibility(true);
					setMapScrollbarMovePosition(_currentPosition.X, _currentPosition.Y);
					break;

				case AbstractEventType.ZoomMap:
					if (!_freeLookMode) _backupZoom = _currentZoom;
					addSidebarControl(pnlPosition);
					addSidebarControl(chkLinkZoom);
					if (_freeLookMode && _currentPosition != _backupPosition)
					{
						addSidebarControl(lblTempInfo);
						lblTempInfo.Text = "Using free look, will combine a Move event at the current position.";
					}
					setEditPositionZoomRange();
					setEditPositionIncrement(1);
					setEditPositionValues(_currentZoom);
					setMapScrollbarVisibility(true);
					setMapScrollbarZoomPosition(_currentZoom.X, _currentZoom.Y);
					break;

				case AbstractEventType.ClearFgTags:
				case AbstractEventType.ClearTextTags:
					addSidebarControl(pnlTempCreate);
					setChildControls(pnlTempCreate, false);
					optFG.Enabled = true;
					optText.Enabled = true;
					break;

				// Only need the first event.
				case AbstractEventType.FgTag1:
					addSidebarControl(pnlShipTag);

					short[] fgUsage = getTagUsageArray(hsbTimer.Value, AbstractEventType.FgTag1, AbstractEventType.FgTag8, AbstractEventType.ClearFgTags);
					populateTagSlotDropdown(cboFgTagSlot, -1, hsbTimer.Value, fgUsage);

					if (isXwa) populateIconDropdown(cboFGTagItem, -1, hsbTimer.Value, 0);
					else populateFlightgroupDropdown(cboFGTagItem, true);

					// Make sure the index is valid.  The verification feature can report any other problems.
					if (cboFGTagItem.SelectedIndex < 0 || cboFGTagItem.SelectedIndex >= cboFGTagItem.Items.Count) safeSetCbo(cboFGTagItem, 0);

					tagIndex = getAutoTagSlot(fgUsage, hsbTimer.Value);
					if (tagIndex == -1)
					{
						popupWarning("All flightgroup tag slots are currently in use.  You must choose a specific tag slot to override, or clear the existing tags.");
						tagIndex = 0;
					}

					safeSetCbo(cboFgTagSlot, tagIndex);
					break;

				case AbstractEventType.TextTag1:
					addSidebarControl(pnlTextTag);
					addSidebarControl(pnlString);
					addSidebarControl(pnlPosition);
					addSidebarControl(pnlMoveOptions);
					disableMoveOptions(true, false);

					short[] textUsage = getTagUsageArray(hsbTimer.Value, AbstractEventType.TextTag1, AbstractEventType.TextTag8, AbstractEventType.ClearTextTags);
					populateTagSlotDropdown(cboTextTagSlot, -1, hsbTimer.Value, textUsage);
					tagIndex = getAutoTagSlot(textUsage, hsbTimer.Value);
					if (tagIndex == -1)
					{
						popupWarning("All text tag slots are currently in use.  You must choose a specific tag slot to override, or clear the existing tags.");
						tagIndex = 0;
					}

					safeSetCbo(cboTextTagSlot, tagIndex);
					setEditString(BriefingString.Tag, getAutoStringSlot(BriefingString.Tag));
					string tagString = txtEditString.Text;
					Size tagSize = measureTagString(tagString, _tagFont);
					if (tagString == "")
					{
						// For an empty string, check for arbitrary length to hold some text.
						Size minSize = measureTagString("Sample Text Tag", _tagFont);
						tagSize.Width = minSize.Width;
					}
					p = getEmptyMapPosition(tagSize.Width, tagSize.Height, 4, 0);
					setEditPositionMaxRange();
					setEditPositionIncrement(-1);
					setEditPositionValues(p);
					break;

				case AbstractEventType.XwaSetIcon:
					addSidebarControl(pnlIcon);
					addSidebarControl(pnlCraftIcon);
					addSidebarControl(chkAutoMoveIcon);
					if (!chkAutoMoveIcon.Checked)
					{
						addSidebarControl(lblTempInfo);
						lblTempInfo.Text = "Click on map to create an icon there.";
					}
					addSidebarControl(pnlPosition);
					addSidebarControl(pnlMoveOptions);
					disableMoveOptions(true, false);
					addSidebarControl(pnlRotation);

					// Some of the panels may need to be disabled.  Set initial state to whatever the checkbox is.
					chkAutoMoveIcon_CheckedChanged(0, new EventArgs());

					int iconIndex = getAutoIconSlot(hsbTimer.Value, cboIcon.SelectedIndex);
					if (iconIndex == -1)
					{
						popupWarning("All icon slots are currently in use.  You must choose a specific icon slot to override.");
						iconIndex = 0;
					}

					populateIconDropdown(cboIcon, -1, hsbTimer.Value, 1);
					safeSetCbo(cboIcon, iconIndex);
					safeSetCbo(cboCraftType, Math.Max(1, cboCraftType.SelectedIndex));
					safeSetCbo(cboRotation, 0);
					p = getEmptyMapPosition(_craftIconBracketSize, _craftIconBracketSize, 0, -_craftIconBracketSize / 2);
					setEditPositionMaxRange();
					setEditPositionIncrement(-1);
					setEditPositionValues(p);
					pnlPosition.Enabled = chkAutoMoveIcon.Checked;
					pnlRotation.Enabled = chkAutoMoveIcon.Checked;
					break;

				case AbstractEventType.XwaMoveIcon:
					addSidebarControl(pnlIcon);
					addSidebarControl(pnlPosition);
					addSidebarControl(pnlMoveOptions);
					disableMoveOptions(true, false);
					populateIconDropdown(cboIcon, -1, hsbTimer.Value, 0);
					safeSetCbo(cboIcon, _lastSelectedIconIndex);
					p = convertPixelPosToRawPos(_mapCanvas.Width / 2, _mapCanvas.Height / 2);
					setEditPositionMaxRange();
					setEditPositionIncrement(-1);
					setEditPositionValues(p);
					break;

				case AbstractEventType.XwaRotateIcon:
					addSidebarControl(pnlIcon);
					addSidebarControl(pnlRotation);
					populateIconDropdown(cboIcon, -1, hsbTimer.Value, 0);
					safeSetCbo(cboIcon, _lastSelectedIconIndex);
					int rotEventIndex = getRotateIconEventIndex(hsbTimer.Value, _lastSelectedIconIndex);
					int rotation = (rotEventIndex >= 0 ? _events[rotEventIndex].Params[1] : 0);
					safeSetCbo(cboRotation, rotation);
					break;

				case AbstractEventType.XwaShipInfo:
					addSidebarControl(pnlIcon);
					addSidebarControl(pnlShipState);
					addSidebarControl(pnlShipInfoEnd);
					populateIconDropdown(cboIcon, -1, hsbTimer.Value, 2);
					setRadio(optStateOn, !_activeShipInfoBlock);
					pnlShipInfoEnd.Enabled = optStateOn.Checked;
					break;

				case AbstractEventType.XwaChangeRegion:
					addSidebarControl(pnlRegion);
					break;

				case AbstractEventType.XwingNewIcon:
					addSidebarControl(pnlPosition);
					addSidebarControl(pnlMoveOptions);
					disableMoveOptions(true, false);
					addSidebarControl(pnlCraftIcon);
					addSidebarControl(pnlCraftName);
					p = getEmptyMapPosition(_craftIconBracketSize, _craftIconBracketSize, 2, -_craftIconBracketSize / 2);
					setEditPositionMaxRange();
					setEditPositionIncrement(-1);
					setEditPositionValues(p);
					txtCraftName.Text = "Unnamed";
					txtCraftName.ReadOnly = false;
					break;

				case AbstractEventType.XwaInfoParagraph:
					addSidebarControl(pnlShipState);
					addSidebarControl(pnlString);
					addSidebarControl(lblTempInfo);
					addSidebarControl(pnlShipInfoEnd);
					setEditString(BriefingString.String, 0);
					setRadio(optStateOn, (_shipInfoStringIndex == -1));
					pnlString.Enabled = optStateOn.Checked;
					lblTempInfo.Text = "Persists until changed or disabled.  Bracket [] and center > not supported.  Color codes 3-6 only.";
					pnlShipInfoEnd.Enabled = optStateOn.Checked;
					numShipInfoTime.Enabled = false;
					lblShipInfoTime.Enabled = false;
					break;
			}

			addSidebarControl(pnlTempEnd);
			pnlTempEnd.Top = getFormScaledValue(340);
			endSidebarBuild();

			// If something was hidden in the sidebar, it may still have focus.  Make sure it changes.
			cmdOk.Focus();
			refreshMap();
			_loading = false;
		}

		void insertTempEvent()
		{
			// Special case since this isn't a real event, it just uses the controls to set up the parameters.
			if (_tempEvent == AbstractEventType.XwingNewIcon)
			{
				AbstractFlightgroup afg = new AbstractFlightgroup
				{
					Name = txtCraftName.Text,
					Abbrev = getCraftAbbrev(cboCraftType.SelectedIndex),
					CraftType = cboCraftType.SelectedIndex,
					CraftIff = cboIff.SelectedIndex
				};
				afg.DisplayName = getCraftDisplayName(afg.Abbrev, afg.Name);
				BaseFlightGroup.Waypoint waypoint = afg.Waypoints[getFlightgroupWaypoint()];
				waypoint.RawX = (short)numX.Value;
				waypoint.RawY = (short)numY.Value;
				waypoint.Enabled = true;
				insertXwingFlightgroup(afg, -1);
				endTempEvent(true);

				return;
			}

			// Real events handled here.
			bool created = false;
			if (hasAvailableSpace(_tempEvent))
			{
				short time = (short)hsbTimer.Value;
				AbstractEvent evt = new AbstractEvent(time, _tempEvent);

				// If we're inserting multiple grouped events, they'll be assigned to the extra elements.
				AbstractEvent[] batch = new AbstractEvent[3] { evt, null, null };

				switch (_tempEvent)
				{
					case AbstractEventType.SkipMarker:
					case AbstractEventType.PageBreak:
						getEventIndexRangeAtTime(hsbTimer.Value, out int low, out int high);
						if (low < 0)
						{
							// No params to assign
							break;
						}
						for (int i = low; i <= high; i++)
						{
							if (_events[i].Event != _tempEvent) continue;

							string ts = (_tempEvent == AbstractEventType.PageBreak ? "Page Break" : "Marker");
							popupInfo($"There is already a {ts} event at this time.");
							evt.Event = AbstractEventType.None;
							break;
						}
						break;

					case AbstractEventType.TitleText:
					case AbstractEventType.CaptionText:
					case AbstractEventType.PanelText3:
					case AbstractEventType.PanelText4:
						evt.Params[0] = (short)cboEditString.SelectedIndex;

						if (time != 0 && isVisibleChecked(chkAutoPageBreak) && (isTitlePlatform || _tempEvent == AbstractEventType.CaptionText))
							batch[1] = new AbstractEvent(time, AbstractEventType.PageBreak);

						if (_tempEvent == AbstractEventType.TitleText || !isVisibleChecked(chkAutoTitle) || !isTitlePlatform) break;

						if (_panelStringIndex[PANEL_TITLE] >= 0 && getEventAtTime(AbstractEventType.TitleText, evt.Time, out _) == null)
						{
							// No title here, but a title was previously shown.  Autocreate the title,
							// and position the event so that it's created before the caption/panel.
							// PageBreak knows to insert at the top, so that can remain where it is.
							AbstractEvent title = new AbstractEvent(time, AbstractEventType.TitleText, (short)_panelStringIndex[PANEL_TITLE]);
							batch[2] = evt;
							batch[0] = title;
						}
						break;

					case AbstractEventType.MoveMap:
					case AbstractEventType.ZoomMap:
						evt.Params[0] = (short)numX.Value;
						evt.Params[1] = (short)numY.Value;

						if (!_freeLookMode) break;

						// Autocreate a move or zoom event based on whether the corresponding property was changed.
						if (_tempEvent == AbstractEventType.ZoomMap && _currentPosition != _backupPosition)
						{
							// Rearrange so that the move command is inserted first, as is default convention.
							batch[1] = evt;
							batch[0] = new AbstractEvent(time, AbstractEventType.MoveMap, (short)_currentPosition.X, (short)_currentPosition.Y);
						}
						else if (_tempEvent == AbstractEventType.MoveMap && _currentZoom != _backupZoom)
							batch[1] = new AbstractEvent(time, AbstractEventType.ZoomMap, (short)_currentZoom.X, (short)_currentZoom.Y);
						break;

					case AbstractEventType.ClearFgTags:
					case AbstractEventType.ClearTextTags:
						evt.Event = (optFG.Checked ? AbstractEventType.ClearFgTags : AbstractEventType.ClearTextTags);
						break;

					case AbstractEventType.FgTag1:
						evt.Event = AbstractEventType.FgTag1 + cboFgTagSlot.SelectedIndex;
						evt.Params[0] = (short)cboFGTagItem.SelectedIndex;
						break;

					case AbstractEventType.TextTag1:
						evt.Event = AbstractEventType.TextTag1 + cboTextTagSlot.SelectedIndex;
						evt.Params[0] = (short)cboEditString.SelectedIndex;
						evt.Params[1] = (short)numX.Value;
						evt.Params[2] = (short)numY.Value;
						evt.Params[3] = (short)cboTextTagColor.SelectedIndex;
						break;

					case AbstractEventType.XwaSetIcon:
						int iconIndex = cboIcon.SelectedIndex;
						_lastSelectedIconIndex = iconIndex;
						evt.Params[0] = (short)iconIndex;
						evt.Params[1] = (short)cboCraftType.SelectedIndex;
						evt.Params[2] = (short)cboIff.SelectedIndex;

						// Don't create extra events for null craft, since the intention is probably to disable the icon.
						if (!chkAutoMoveIcon.Checked || cboCraftType.SelectedIndex <= 0) break;

						batch[1] = new AbstractEvent(time, AbstractEventType.XwaMoveIcon, (short)iconIndex, (short)numX.Value, (short)numY.Value);
						// For convenience, set the working icon so we can immediately use path mode.
						_lastSelectedMoveIconUid = batch[1].UID;
						if (cboRotation.SelectedIndex != 0) batch[2] = new AbstractEvent(time, AbstractEventType.XwaRotateIcon, (short)iconIndex, (short)cboRotation.SelectedIndex);
						break;

					case AbstractEventType.XwaMoveIcon:
						_lastSelectedIconIndex = cboIcon.SelectedIndex;
						evt.Params[0] = (short)cboIcon.SelectedIndex;
						evt.Params[1] = (short)numX.Value;
						evt.Params[2] = (short)numY.Value;
						break;

					case AbstractEventType.XwaRotateIcon:
						_lastSelectedIconIndex = cboIcon.SelectedIndex;
						evt.Params[0] = (short)cboIcon.SelectedIndex;
						evt.Params[1] = (short)cboRotation.SelectedIndex;
						break;

					case AbstractEventType.XwaShipInfo:
						evt.Params[0] = (short)(optStateOn.Checked ? 1 : 0);
						evt.Params[1] = (short)cboIcon.SelectedIndex;
						if (optStateOn.Checked && chkAutoShipInfoEnd.Checked)
						{
							int endTime = Math.Min(MaxEventTime, evt.Time + (short)(numShipInfoTime.Value * _ticksPerSecond));
							batch[1] = new AbstractEvent((short)endTime, AbstractEventType.XwaShipInfo, 0, (short)cboIcon.SelectedIndex);
						}
						break;

					case AbstractEventType.XwaChangeRegion:
						evt.Params[0] = (short)cboRegion.SelectedIndex;
						break;

					case AbstractEventType.XwaInfoParagraph:
						evt.Params[0] = (short)(optStateOn.Checked ? cboEditString.SelectedIndex + 1 : 0);
						if (!optStateOn.Checked || !chkAutoShipInfoEnd.Checked) break;

						// Find the next shipinfo off state and create a paragraph off state there.
						foreach (AbstractEvent search in _events)
						{
							if (search.Time > evt.Time && search.Event == AbstractEventType.XwaShipInfo && search.Params[0] == 0)
							{
								batch[1] = new AbstractEvent(search.Time, AbstractEventType.XwaInfoParagraph, 0);
								break;
							}
						}
						break;
				}

				// If something failed, it will have set the event to None.
				if (evt.Event != AbstractEventType.None)
				{
					beginUndoFrame(true);

					int jumpTime = -1;
					int processCount = 0;
					for (int i = 0; i < batch.Length; i++)
					{
						var newEvt = batch[i];
						if (newEvt == null) continue;

						if (i > 0)
						{
							if (newEvt.Event == AbstractEventType.PageBreak || newEvt.Event == AbstractEventType.SkipMarker)
							{
								// If there's already a matching break, don't insert.
								var existEvt = getEventAtTime(newEvt.Event, newEvt.Time, out _);
								if (existEvt != null) continue;
							}
							else if (newEvt.Event == AbstractEventType.XwaMoveIcon || newEvt.Event == AbstractEventType.XwaRotateIcon)
							{
								// If there's an existing event for this icon, replace the existing parameters.
								var existEvt = getIconEventAtTime(newEvt.Event, newEvt.Params[0], newEvt.Time, out _);
								if (existEvt != null)
								{
									existEvt.CopyFrom(newEvt);
									processCount++;
									continue;
								}
							}
						}

						// If we get here without breaking or continuing, we need to insert the item.
						if (!hasAvailableSpace(newEvt.Event))
						{
							popupError($"Not enough space to create {newEvt.Event} event.");
							break;
						}

						int iconIndex = 0;
						if (newEvt.Event == AbstractEventType.XwaSetIcon || newEvt.Event == AbstractEventType.XwaMoveIcon || newEvt.Event == AbstractEventType.XwaRotateIcon)
							iconIndex = newEvt.Params[0];

						int destIndex = getInsertionIndexForIconEvent(newEvt.Time, newEvt.Event, iconIndex);
						insertEvent(destIndex, newEvt, true);
						processCount++;

						// If it's an info off event, or paragraph off event, assign the jump time.
						// Region skips ahead to the next frame.
						if ((newEvt.Event == AbstractEventType.XwaShipInfo || newEvt.Event == AbstractEventType.XwaInfoParagraph) && (newEvt.Params[0] == 0 && chkAutoShipInfoJump.Checked))
							jumpTime = newEvt.Time;
						else if (newEvt.Event == AbstractEventType.XwaChangeRegion) jumpTime = newEvt.Time + 1;
					}

					relinkSelectedEventUid();
					rebuildShadowIcons();
					rebuildBriefing();

					// We're done using free look for these event types.
					if (_tempEvent == AbstractEventType.MoveMap || _tempEvent == AbstractEventType.ZoomMap) setFreeLookMode(false);

					if (jumpTime >= 0) jumpToTime(jumpTime, true);

					created = true;
					_lastEditEventUid = evt.UID;
					refreshMap();
					refreshTimeline();
				}
			}
			else popupError("Not enough space to create new event.");

			endTempEvent(created);
		}

		void endTempEvent(bool created)
		{
			// Restore functionality of navigation.
			hsbTimer.Enabled = true;

			// If the user creates one of these events at time zero, the rebuilt values must take effect.
			// Otherwise revert to whatever the previous setting was.
			if (_tempEvent == AbstractEventType.MoveMap)
			{
				if (created && hsbTimer.Value == 0) rebuildBriefing();
				else if (!_freeLookMode) _currentPosition = _backupPosition;
			}
			if (_tempEvent == AbstractEventType.ZoomMap)
			{
				if (created && hsbTimer.Value == 0) rebuildBriefing();
				else if (!_freeLookMode) _currentZoom = _backupZoom;
			}

			_tempEvent = AbstractEventType.None;
			setTitleOverride("", 0);
			resetSidebar();
			refreshCanvas(RefreshFlags.All);
		}
		
		int getAutoTagSlot(short[] usage, int time)
		{
			int count = (isXwing ? 4 : 8);
			int result = -1;
			for (int i = 0; i < count; i++)
			{
				// Prioritize empty slot, otherwise overwrite a future slot.
				if (usage[i] == 0) return i;
				if (result == -1 && usage[i] > time) result = i;
			}
			return result;
		}

		/// <summary>Search the icon list for an unused slot to choose when creating a temp event.</summary>
		/// <returns>An icon index that hasn't been activated yet, or -1 if not found.</returns>
		int getAutoIconSlot(int time, int startIndex)
		{
			int[] usage = getIconUsageArray(time);

			int remain = usage.Length;
			int pos = Math.Max(0, startIndex);

			int result = -1;
			int empty = 0;
			while (--remain >= 0)
			{
				// Note: Icons that have been cleared (set back to zero) are still considered in use for auto detection.
				unpackIconUsage(usage[pos], out int evtTime, out int craftType, out _);
				if (evtTime == 0 && craftType == 0) empty++;
				if (result == -1 && usage[pos] == 0) result = pos;
				if (++pos >= usage.Length) pos = 0;
			}

			// If the entire list is empty, as it would be in a new region, always choose the first slot.
			return (empty == usage.Length ? 0 : result);
		}
		
		/// <summary>Attempts to automatically determine a string index when choosing to begin a temp event.</summary>
		/// <returns>Always returns a valid string index, although the existing string may not be empty.</returns>
		int getAutoStringSlot(BriefingString type)
		{
			List<string> strList = getStringList(type);

			// If editing the same kind of string, start there.
			int start = (type == _editStringType ? _editStringIndex : -1);

			// Search for the first empty string, based on the starting position or beginning of the list.
			int remain = strList.Count;
			int pos = Math.Max(0, start);
			while (--remain >= 0)
			{
				if (string.IsNullOrWhiteSpace(strList[pos])) return pos;
				if (++pos >= strList.Count)	pos = 0;
			}

			// No empty strings.  Choose the next one from the current selection, if possible.
			// If no starting string was assigned (-1), the result will be zero.
			if (++start >= strList.Count) start = 0;
			
			return start;
		}
		
		/// <summary>Searches the map for a suitable empty location close to the center when creating a new event.</summary>
		/// <param name="width">Width, in pixels.</param>
		/// <param name="height">Height, in pixels.</param>
		/// <param name="padding">Padding around all edges, in pixels.  Intended for text tags.  Should be zero or positive.</param>
		/// <param name="offset">Offset to apply to both dimensions, in pixels.  Intended for square icons.  Should be zero or negative.</param>
		/// <remarks>Checks the interact locations of all existing items to find a location which does not overlap.</remarks>
		/// <returns>A Point position in raw map units.  If no suitable position is found, it returns the center of the map.</returns>
		Point getEmptyMapPosition(int width, int height, int padding, int offset)
		{
			int itemWidth = (width + (padding * 2)) - 1;
			int itemHeight = (height + (padding * 2)) - 1;
			int stepX = Math.Max(4, _mapCanvas.Width / 40);
			int stepY = Math.Max(4, _mapCanvas.Height / 40);

			int midX = _mapCanvas.Width / 2;
			int midY = _mapCanvas.Height / 2;
			int curOffsetX = 0;
			int curOffsetY = 0;

			// The search begins at the center of the map.  The X and Y offsets will grow in a positive distance
			// away from the center.  At the end of each iteration, the sign is flipped so it can check the
			// mirrored position in the opposite direction.  If the resulting sign is positive, it means the
			// negative mirrored check has finished, and the offset distance increases.  A full horizontal row
			// is checked before adjusting the vertical position and checking the next row.  The scan continues
			// until the offsets exceed the edges of the map.
			while (curOffsetY <= midY)
			{
				while (curOffsetX <= midX)
				{
					int x = (midX + curOffsetX + offset) - padding;
					int y = (midY + curOffsetY + offset) - padding;
					if (x > 0 && y > 0 && x + width < _mapCanvas.Width && y + height < _mapCanvas.Height)
					{
						Rectangle check = new Rectangle(x, y, itemWidth, itemHeight);
						if (isEmptyMapLocation(check))
						{
							// Proper location without padding or offset, and converted to what the platform expects.
							Point p = convertPixelPosToRawPos(midX + curOffsetX, midY + curOffsetY);
							p.Y = getInvertedY(p.Y);
							return p;
						}
					}

					curOffsetX *= -1;
					if (curOffsetX >= 0) curOffsetX += stepX;
				}
				
				curOffsetX = 0;   // Reset to center of next row.
				curOffsetY *= -1;
				if (curOffsetY >= 0) curOffsetY += stepY;
			}

			// Default to center of the map if nothing found.
			Point ret = convertPixelPosToRawPos(midX, midY);
			ret.Y = getInvertedY(ret.Y);
			return ret;
		}
		#endregion Temp events

		#region Path mode
		void setPathMode(bool state)
		{
			if (!isXwa || _tempEvent != AbstractEventType.None || _pathMode == state) return;

			if (state == true && getWorkingMoveIcon() == -1)
			{
				popupError("Path mode requires a selected MoveIcon event to work from.");
				return;
			}

			_pathMode = state;
			if (state)
			{
				setTitleOverride("Icon path mode - press P key to exit", 0);
				beginSidebarBuild();

				addSidebarControl(pnlPosition);
				addSidebarControl(pnlMoveOptions);
				addSidebarControl(pnlIconPath);

				setEditPositionIncrement(0);
				setEditPositionMaxRange();
				refreshPathList(-1);

				endSidebarBuild();

				selectPathNode(lstPath.SelectedIndex, true);
			}
			else
			{
				setTitleOverride("", 0);
				_mapPreviewIcons.Clear();
				deselectObjects();
			}
			refreshMap();
		}

		string getPathNodeString(int index) => $"#{index + 1} S:{_pathNodes[index].Steps} T:{_pathNodes[index].Time} R:{_pathNodes[index].Rotation}";

		/// <summary>Refreshes a specific item in the path list, or -1 to refresh the entire list.</summary>
		void refreshPathList(int index)
		{
			bool btemp = _loading;
			_loading = true;

			if (index == -1)
			{
				int selIndex = lstPath.SelectedIndex;

				lstPath.Items.Clear();
				for (int i = 0; i < _pathNodes.Count; i++) lstPath.Items.Add(getPathNodeString(i));

				if (selIndex >= 0 && selIndex < lstPath.Items.Count) lstPath.SelectedIndex = selIndex;
			}
			else if (index >= 0 && index < _pathNodes.Count) lstPath.Items[index] = getPathNodeString(index);

			_loading = btemp;
		}

		/// <summary>Adds a path node to the end of the list, at the position where the user has clicked.</summary>
		/// <param name="pos">Position to create the node, in raw map units.</param>
		/// <param name="rotation">Initial rotation from an existing icon, or zero for default.</param>
		void addPathNode(Point pos, int rotation, bool append)
		{
			if (_pathNodes.Count >= 32) return;

			// Make sure the new node is far enough apart from all existing nodes.
			int xtol = getPixelRawX() * CLICK_TOLERANCE * 3;
			int ytol = getPixelRawY() * CLICK_TOLERANCE * 3;
			foreach (var exist in _pathNodes)
			{
				int xdist = Math.Abs(exist.X - pos.X);
				int ydist = Math.Abs(exist.Y - pos.Y);
				if (xdist <= xtol && ydist <= ytol) return;
			}

			PathNode node = new PathNode
			{
				X = pos.X,
				Y = pos.Y,
				Rotation = rotation
			};

			if (_pathNodes.Count > 0)
			{
				// Compare against what already exists.
				int existIndex = (append ? _pathNodes.Count - 1 : 0);
				var compare = _pathNodes[existIndex];
				if (cboPathAutoRotate.SelectedIndex > 0)
				{
					// Longest axis and direction will determine rotation.
					int offsetX = node.X - compare.X;
					int offsetY = node.Y - compare.Y;
					if (Math.Abs(offsetY) > Math.Abs(offsetX)) compare.Rotation = (offsetY >= 0 ? 2 : 0);
					else compare.Rotation = (offsetX >= 0 ? 3 : 1);
				}
				node.Rotation = compare.Rotation;
			}

			int insertIndex = (append ? _pathNodes.Count : 0);
			lstPath.Items.Add("");
			_pathNodes.Insert(insertIndex, node);
			refreshPathList(append ? lstPath.Items.Count - 1 : -1);
			selectPathNode(insertIndex, true);
		}

		void deletePathNode(int index, bool refresh)
		{
			if (index >= 0 && index < _pathNodes.Count) _pathNodes.RemoveAt(index);

			if (refresh) refreshPathList(-1);
		}

		void selectPathNode(int index, bool resetSelection)
		{
			if (index >= _pathNodes.Count) index = _pathNodes.Count - 1;

			if (index < 0)
			{
				pnlPosition.Enabled = false;
				disableMoveOptions(true, false);
				return;
			}

			bool btemp = _loading;
			_loading = true;
			lstPath.SelectedIndex = index;

			if (resetSelection)
			{
				// Sync up the node selection on the map.
				_selectedObjects.Clear();
				_selectedObjects.Add(new SelectedObject(SelectedType.PathNode, index, 0));
				refreshMap();
			}

			pnlPosition.Enabled = true;
			setChildControls(pnlMoveOptions, true);
			setEditPositionValues(_pathNodes[index].X, _pathNodes[index].Y);
			safeSetNumeric(numPathNodeStep, _pathNodes[index].Steps);
			safeSetNumeric(numPathNodeTime, _pathNodes[index].Time);
			safeSetCbo(cboPathNodeRot, _pathNodes[index].Rotation);
			_loading = btemp;
		}

		int calculateNodeLengths()
		{
			int totalRawLength = 0;
			for (int i = 0; i < _pathNodes.Count - 1; i++)
			{
				var cur = _pathNodes[i];
				var next = _pathNodes[i + 1];
				cur.LengthX = next.X - cur.X;
				cur.LengthY = next.Y - cur.Y;
				// Scale down and back up to prevent any overflows.
				int xlen = cur.LengthX / 4;
				int ylen = cur.LengthY / 4;
				cur.Length = (int)Math.Sqrt(xlen * xlen + ylen * ylen) * 4;
				totalRawLength += cur.Length;
			}
			return totalRawLength;
		}

		void applyPreviewFromNodes()
		{
			_mapPreviewIcons.Clear();
			refreshMap();

			int eventIndex = getWorkingMoveIcon();
			if (eventIndex == -1) return;

			//int startTime = _events[eventIndex].Time;
			int iconIndex = _events[eventIndex].Params[0];
			if (iconIndex < 0 || iconIndex >= _mapIcons.Length) return;

			calculateNodeLengths();

			int timeOffset = 0;
			int prevRotation = 0;
			for (int i = 0; i < _pathNodes.Count; i++)
			{
				var node = _pathNodes[i];
				float stepX = (float)node.LengthX / node.Steps;
				float stepY = (float)node.LengthY / node.Steps;
				float curX = _pathNodes[i].X;
				float curY = _pathNodes[i].Y;

				for (int j = 0; j < node.Steps; j++)
				{
					// Don't replace the starting point at the working icon.
					// But we still need the positional change below it.
					if (i != 0 || j != 0)
					{
						MapElement elem = new MapElement
						{
							Enabled = true,
							EventIndex = -1,
							DataIndex = iconIndex,
							IconCraftType = _mapIcons[iconIndex].IconCraftType,
							Color = _mapIcons[iconIndex].Color
						};

						if (i > 0 && i < _pathNodes.Count - 1 && j == 0 && !node.FirstRotation) elem.IconRotation = prevRotation;
						else elem.IconRotation = node.Rotation;

						elem.IconTime = timeOffset;
						elem.X = (int)curX;
						elem.Y = (int)curY;
						_mapPreviewIcons.Add(elem);
					}

					curX += stepX;
					curY += stepY;
					timeOffset += node.Time;
					prevRotation = node.Rotation;
				}
			}

			refreshPathStats();
			refreshMap();
		}

		/// <summary>Performs some basic verification checks on the list of generated preview icons.  Displays any errors to the user.</summary>
		/// <param name="path">Specifies whether path nodes were used to generate this preview.</param>
		/// <returns>True if it passes verification.</returns>
		bool verifyPreviewIcons(bool path)
		{
			int moveEventIndex = getWorkingMoveIcon();
			if (moveEventIndex == -1)
			{
				popupError("You must select an existing icon to generate movement from.");
				return false;
			}

			if (_mapPreviewIcons.Count == 0)
			{
				popupError("No icons to generate.");
				return false;
			}

			// Some things to check here.  This isn't strictly necessary but it's helpful to avoid some logistic
			// problems that can arise when carelessly generating a long sequence of icons.
			// The user must be viewing the same region as the working icon.  This includes returning to a prior region.
			// Overflow across a region change should be avoided, since the defined icons may be different or
			// nonexistent.  This includes a region transition into the same region.  Let the user sort that out.
			int iconRegion = -1;
			int curRegion = 0;
			int viewRegion = 0;
			int viewEndTime = -1;
			int lastEndTime = -1;
			foreach (var evt in _events)
			{
				if (evt.UID == _lastSelectedMoveIconUid) iconRegion = curRegion;
				if (evt.Event == AbstractEventType.XwaChangeRegion)
				{
					curRegion = evt.Params[0];
					lastEndTime = evt.Time - 1;

					if (evt.Time < hsbTimer.Value) viewRegion = curRegion;
					else if (viewEndTime == -1)	viewEndTime = lastEndTime;
				}
			}

			if (viewRegion != iconRegion)
			{
				popupError("You must be in the same region as the working icon.");
				return false;
			}

			// If no region change was encountered, or there were no further region changes after
			// the viewed region page, allow the full duration.
			if (viewEndTime == -1 || lastEndTime == viewEndTime) viewEndTime = MaxEventTime;

			// The time will never be less than the selected MoveIcon.
			// If the user changes time, and especially if the region is returning back to the original
			// working icon's region at a later time, then we need to use the current time instead.
			int baseTime = _events[moveEventIndex].Time;
			if (hsbTimer.Value > baseTime) baseTime = hsbTimer.Value;

			int duration = _mapPreviewIcons[_mapPreviewIcons.Count - 1].IconTime;
			if (baseTime + duration > viewEndTime)
			{
				int overflow = (baseTime + duration) - viewEndTime;
				string msg = "The duration of the previewed icon sequence is too long.\n";
				if (viewEndTime != MaxEventTime) msg += "It must not overflow into the next region change.";
				else msg += "It exceeds the maximum briefing duration.";
				msg += $"\n\nIt is {overflow} frame{(overflow != 1 ? "s" : "")} ({getTimeString(overflow)} seconds) too long.";
				if (path) msg += $"\nThe total path duration must be {getTimeString(duration - overflow + 1)} seconds or less.";

				popupError(msg);
				return false;
			}
			return true;
		}

		int getLongestNodeSegment()
		{
			int found = -1;
			float longest = float.MinValue;

			// The last node is the end position, not a segment.
			for (int i = 0; i < _pathNodes.Count - 1; i++)
			{
				if (_pathNodes[i].NodeSpacing <= longest) continue;

				longest = _pathNodes[i].NodeSpacing;
				found = i;
			}

			return found;
		}

		void generateAutoPathPreview()
		{
			int eventIndex = getWorkingMoveIcon();
			if (eventIndex == -1)
			{
				popupError("You must select a Move icon before generating a path.");
				return;
			}
			if (_pathNodes.Count < 2)
			{
				popupError("You must have at least two nodes to generate a path.");
				return;
			}

			int targetDuration = 0;
			int targetSpacing = 0;

			int curSpacingX = getPixelRawX();
			int curSpacingY = getPixelRawY();
			int minSpacing = Math.Min(curSpacingX, curSpacingY);

			// See the generated cboPathAutoType collection.
			PathAutoType type = (PathAutoType)cboPathAutoType.SelectedIndex;
			switch (type)
			{
				case PathAutoType.TimeSec:
					targetDuration = (int)(numPathAutoAmount.Value * _ticksPerSecond);
					break;

				case PathAutoType.TimeTick:
					targetDuration = (int)numPathAutoAmount.Value;
					break;

				case PathAutoType.DistPixel:  // Pixel at 1x scaling.
					targetSpacing = minSpacing * (int)numPathAutoAmount.Value;
					break;

				case PathAutoType.DistMeter:
					// Meters are user friendly, but don't match up to the grid.  Convert to grid.
					targetSpacing = (int)(((float)numPathAutoAmount.Value / 1000.0f) * 256.0f);
					break;

				case PathAutoType.DistRaw:
					targetSpacing = (int)numPathAutoAmount.Value;
					break;
			}

			if (type <= PathAutoType.TimeTick && targetDuration == 0)
			{
				popupError("You must specify a total duration for the generated path.");
				return;
			}
			else if (type > PathAutoType.TimeTick && targetSpacing == 0)
			{
				popupError("The chosen icon spacing rounds down to a distance of zero.  Choose a higher icon spacing.");
				return;
			}

			// Reset nodes.
			foreach (PathNode node in _pathNodes) node.Steps = 1;

			int totalLength = calculateNodeLengths();
			if (targetDuration > 0 )
			{
				// Distribute based on duration.  Assign steps to each segment until we run out of ticks.
				// One icon per tick.  Tick rate determined by the first time step.
				int step = _pathNodes[0].Time;
				if (step < 1) step = 1;

				int remain = targetDuration - (_pathNodes.Count * step);
				while (remain > 0)
				{
					int index = getLongestNodeSegment();
					if (index == -1) break;

					_pathNodes[index].Steps++;
					remain -= step;
				}
			}
			else
			{
				// Distribute based on spacing.
				int count = totalLength / targetSpacing;
				int eventSize = EventDef.GetEventDefByType(AbstractEventType.MoveMap).GetSize();
				int spaceReq = eventSize * count;
				if (_eventSpaceUsed + spaceReq > _eventSpaceMax)
				{
					int overflow = count - (((_eventSpaceMax - _eventSpaceUsed) + (eventSize - 1)) / eventSize);
					popupError($"There isn't enough space to produce this path!  It would generate {count} icons, consuming {spaceReq} event space, which is {overflow} icons over capacity.");
					return;
				}
				
				while (true)
				{
					int index = getLongestNodeSegment();
					if (index == -1) break;
					if (_pathNodes[index].NodeSpacing <= targetSpacing) break;

					_pathNodes[index].Steps++;
				}
			}

			// Now that the steps are finished, apply the auto rotation.
			// The first element of the rotate control disables rotation generation.
			if (cboPathAutoRotate.SelectedIndex > 0 && cboPathDefaultFacing.SelectedIndex >= 1)
			{
				// The default facing determines which way the bitmap image is facing when the craft rotation is zero.
				// Not all bitmaps face the same way, if they face a direction at all.
				// For example: X-W faces up, B-W faces left, T/F faces forward.
				// We don't know what the bitmaps are, so the user must choose.

				// Rotations:       0 = None, 1 = Left 90, 2 = 180, 3 = Right 90, 4 = Mirror
				
				// Default Facing:  0 = None, 1 = Up, 2 = Left, 3 = Right, 4 = Down
				// Indexed by default facing (after subtracting 1), which rotation do we need to apply to face the desired direction.
				int[] right = new int[4]{ 3, 4, 0, 1 };
				int[] left  = new int[4]{ 1, 0, 4, 3 };
				int[] up    = new int[4]{ 0, 3, 1, 2 };
				int[] down  = new int[4]{ 2, 1, 3, 0 };
				int defFacing = cboPathDefaultFacing.SelectedIndex - 1;
				
				for (int i = 0; i < _pathNodes.Count - 1; i++)
				{
					int xlen = _pathNodes[i + 1].X - _pathNodes[i].X;
					int ylen = _pathNodes[i + 1].Y - _pathNodes[i].Y;

					int rotation;
					if (Math.Abs(xlen) > Math.Abs(ylen))
					{
						if (xlen >= 0) rotation = right[defFacing];
						else rotation = left[defFacing];
					}
					else
					{
						if (ylen >= 0) rotation = down[defFacing];
						else rotation = up[defFacing];
					}
					_pathNodes[i].Rotation = rotation;
					_pathNodes[i].FirstRotation = (cboPathAutoRotate.SelectedIndex == 1);
				}

				// Final node uses the rotation of the previous node before it.
				_pathNodes[_pathNodes.Count - 1].Rotation = _pathNodes[_pathNodes.Count - 2].Rotation;
			}
			else
			{
				int rotation = cboPathDefaultFacing.SelectedIndex;
				if (cboPathAutoRotate.SelectedIndex == 0)
				{
					// Not using the rotation dropdown, use whatever the working icon has.
					var work = getShadowIconByUid(_lastSelectedMoveIconUid);
					if (work != null) rotation = work.IconRotation;
				}

				for (int i = 0; i < _pathNodes.Count; i++)
				{
					_pathNodes[i].Rotation = rotation;
					_pathNodes[i].FirstRotation = true;
				}
			}

			// Distribute time to all nodes.
			int timeStep = _pathNodes[0].Time;
			for (int i = 1; i < _pathNodes.Count; i++) _pathNodes[i].Time = timeStep;

			refreshPathList(-1);
			refreshPathStats();
			applyPreviewFromNodes();
		}

		/// <summary>Refreshes a label with the total duration and event space required to generate the chosen path.</summary>
		void refreshPathStats()
		{
			int time = 0;
			int moveCount = 0;
			int rotCount = 0;

			int curRot = -1;
			int eventIndex = getWorkingMoveIcon();
			if (eventIndex >= 0)
			{
				var elem = getShadowIconByUid(_events[eventIndex].UID);
				if (elem != null) curRot = elem.IconRotation;
			}

			for (int i = 0; i < _pathNodes.Count - 1; i++)
			{
				time += (_pathNodes[i].Steps * _pathNodes[i].Time);
				moveCount += _pathNodes[i].Steps;
				if (_pathNodes[i].Rotation != curRot)
				{
					rotCount++;
					curRot = _pathNodes[i].Rotation;
				}
			}

			// Last node is always a single item.
			if (_pathNodes.Count > 0)
			{
				time++;
				moveCount++;
			}

			int spaceUsed = moveCount * EventDef.GetEventDefByType(AbstractEventType.XwaMoveIcon).GetSize();
			spaceUsed += rotCount * EventDef.GetEventDefByType(AbstractEventType.XwaRotateIcon).GetSize();

			lblPathTotalDuration.Text = $"Total: {getTimeString(time, true)}\nSpace: {spaceUsed}";
		}

		void rotatePathNodes(bool clockwise)
		{
			double rotation = (Math.PI / 2) * (clockwise ? 1 : -1);
			foreach (var node in _pathNodes)
			{
				int offsetX = node.X - _pathNodes[0].X;
				int offsetY = node.Y - _pathNodes[0].Y;
				node.X = _pathNodes[0].X + (int)(Math.Cos(rotation) * offsetX - Math.Sin(rotation) * offsetY);
				node.Y = _pathNodes[0].Y + (int)(Math.Sin(rotation) * offsetX + Math.Cos(rotation) * offsetY);
			}
		}

		void derivePathFromSelection(int iconIndex)
		{
			List<SelectedObject> filter = new List<SelectedObject>();
			foreach (var so in _selectedObjects)
			{
				if (!isValidEvent(so)) continue;

				var evt = _events[so.Index];
				if (evt.Event == AbstractEventType.XwaMoveIcon && evt.Params[0] == iconIndex) filter.Add(so);
			}

			// Include the working icon if not part of the selection.
			SelectedObject work = new SelectedObject(SelectedType.Event, getWorkingMoveIcon(), _lastSelectedMoveIconUid);
			if (hasSelectedObject(work) == -1) filter.Add(work);

			if (filter.Count < 2)
			{
				popupError("You must select at least two MoveIcons on the map, belonging to the same icon, in order to derive a path from them.");
				return;
			}

			if (_pathNodes.Count > 0)
				if (!popupConfirm("Deriving a path will replace your existing path nodes.  Continue?")) return;

			// Make sure the events are in sequential order.
			filter.Sort(SelectedObject.Compare);

			// Build a list of nodes, one for each icon.  No limit on maximum.
			_pathNodes.Clear();
			int prevTime = -1;
			for (int i = 0; i < filter.Count; i++)
			{
				var elem = getShadowIconByUid(filter[i].EventUID);
				if (elem != null)
				{
					if (prevTime == -1) prevTime = elem.IconTime;

					PathNode node = new PathNode
					{
						Time = elem.IconTime - prevTime,
						X = elem.X,
						Y = elem.Y,
						Rotation = elem.IconRotation
					};
					_pathNodes.Add(node);
					prevTime = elem.IconTime;
				}
			}

			if (_pathNodes.Count != filter.Count)
			{
				popupError("Unexpected error, couldn't find event for selection.");
				return;
			}

			// Fix the time step for the first node, since it will be zero.
			_pathNodes[0].Time = Math.Max(1, _pathNodes[1].Time - _pathNodes[0].Time);

			// Optimize the list by scanning through node segments and removing them when possible.  For a
			// sequence like A-B-C, first get vector AB.  Then check subsequent segments.  If vector BC
			// is similar to AB (length and direction), increment the step count for A and remove B.
			// If BC does not match AB, B becomes the starting point.  Repeat until there's nothing left.
			int pos = 0;
			while (pos < _pathNodes.Count - 1)
			{
				var workNode = _pathNodes[pos];
				var curNode = _pathNodes[pos];
				var nextNode = _pathNodes[pos + 1];
				pos++;

				// Get the proper length before normalizing.  Clamp to epsilon to avoid NaN if lengths are zero, which
				// can happen if multiple nodes occupy the same position.
				// NOTE: Using this vector since we're already using System.Media and PresentationCore.dll for audio.
				// Saves having to add System.Numerics.dll for the sole purpose of accessing Vector2.
				Vector3D first = new Vector3D(nextNode.X - curNode.X, nextNode.Y - curNode.Y, 0.0);
				double firstLen = Math.Min(double.Epsilon, first.Length);
				first.Normalize();

				int pos2 = pos;
				while (pos2 < _pathNodes.Count - 1)
				{
					curNode = _pathNodes[pos2];
					nextNode = _pathNodes[pos2 + 1];
					Vector3D second = new Vector3D(nextNode.X - curNode.X, nextNode.Y - curNode.Y, 0.0);
					double secondLen = Math.Min(double.Epsilon, second.Length);
					double ratio = ((firstLen <= secondLen) ? (firstLen / secondLen) : (secondLen / firstLen));
					second.Normalize();

					// Offsets and lengths won't always be uniform, so check if they're reasonably close enough.
					if (ratio >= 0.95 && Vector3D.DotProduct(first, second) >= 0.995)
					{
						// Try to fix time in case it changes.  The first node of each segment won't have the correct time assigned if the times are different.
						workNode.Time = nextNode.Time;
						workNode.Steps++;
						_pathNodes.RemoveAt(pos2);
					}
					else break;
				}
			}

			refreshPathList(-1);
			refreshMap();
		}
		#endregion Path mode

		#region Xwing panel mode
		void setPanelMode(bool state)
		{
			if (!isXwing || _tempEvent != AbstractEventType.None || _panelMode == state) return;

			_panelMode = state;
			if (state)
			{
				pauseBriefing();
				deselectObjects();
				setFreeLookMode(false);
				beginSidebarBuild();
				addSidebarControl(pnlPanelMode);
				addSidebarControl(pnlMoveOptions);

				_panelModeEdge = 9;

				// Only using the lock controls from this panel.
				setChildControls(pnlMoveOptions, false);
				chkMoveLockX.Enabled = true;
				chkMoveLockY.Enabled = true;

				Platform.Xwing.Briefing core = getXwingCoreBriefing();
				int pageType = core.Pages[_currentBriefingIndex].PageType;
				int count = 0;
				foreach (var page in core.Pages) if (page.PageType == pageType) count++;
				string s = $"Page Type #{pageType + 1}\n";
				if (count == 1) s += $"In use by {count} page";
				else s += $"Shared by {count} pages\nChanges affect all pages!";

				// Init backup state, but only do it once.
				if (_panelBackup == null) _panelBackup = new short[4][];

				if (pageType < _panelBackup.Length && _panelBackup[pageType] == null) _panelBackup[pageType] = getPanelRawData(pageType);

				lblPanelModeUsage.Text = s;
				pctTimeline.Visible = false;
				vsbTimeline.Visible = false;
				lblMapPanelMode.Visible = true;
				refreshPanelModeList();
				refreshPanelTextLines(getPanelModePageTemplate(), lstPanelMode.SelectedIndex);
				endSidebarBuild();
			}
			else
			{
				if (_mouseDragState != MouseDragState.None)
				{
					Cursor = Cursors.Default;
					_mouseDragState = MouseDragState.None;
				}
				pctTimeline.Visible = true;
				vsbTimeline.Visible = true;
				lblMapPanelMode.Visible = false;
				resetSidebar();
			}
			refreshCanvas(RefreshFlags.All);
		}

		short[] getPanelRawData(int pageType)
		{
			short[] dat = new short[25];
			var panels = getXwingCoreBriefing().Templates[pageType].Items;
			for (int i = 0; i < 5; i++)
			{
				int offset = i * 5;
				dat[offset + 0] = panels[i].Top;
				dat[offset + 1] = panels[i].Left;
				dat[offset + 2] = panels[i].Bottom;
				dat[offset + 3] = panels[i].Right;
				dat[offset + 4] = (short)(panels[i].IsVisible ? 1 : 0);
			}
			return dat;
		}

		void setPanelRawData(int pageType, short[] dat)
		{
			var panels = getXwingCoreBriefing().Templates[pageType].Items;
			for (int i = 0; i < 5; i++)
			{
				int offset = i * 5;
				panels[i].Top = dat[offset + 0];
				panels[i].Left = dat[offset + 1];
				panels[i].Bottom = dat[offset + 2];
				panels[i].Right = dat[offset + 3];
				panels[i].IsVisible = Convert.ToBoolean(dat[offset + 4]);
			}
		}
		
		bool isXwingPanelEditIndexSelected(int index) => (isXwing && _panelMode && lstPanelMode.SelectedIndex == index);
	
		void selectXwingEditPanel(int index)
		{
			if (ActiveControl != lstPanelMode) lstPanelMode.SelectedIndex = index;

			_panelModeEdge = 9;
			applyPanelConfigToFrontEnd();
			refreshPanelModeList();
			refreshPanelModeVisuals();
			refreshPanelTextLines(getWorkingPageTemplate(), index);
		}

		Cursor getMapPanelDragCursor(int edgeIndex)
		{
			// Resize operation.
			Cursor[] cursors = new Cursor[] {
				Cursors.SizeNWSE, Cursors.SizeNESW, Cursors.SizeNESW, Cursors.SizeNWSE,
				Cursors.SizeNS, Cursors.SizeNS, Cursors.SizeWE, Cursors.SizeWE
			};
			return cursors[edgeIndex];
		}
		
		Rectangle[] getMapPanelScaledRects() => new Rectangle[] {_scaledTitleRect, _scaledCaptionRect, _scaledPanel3Rect, _scaledPanel4Rect, _scaledMapRect };

		int getMapPanelDragTolerance() => (int)(CLICK_TOLERANCE * _mapScale);

		int getMapPanelDragEdge(Point cursorPos, Rectangle panel)
		{
			int tolerance = getMapPanelDragTolerance();
			int tol2 = tolerance * 2;

			Rectangle full = new Rectangle(panel.Left - tolerance, panel.Top - tolerance, panel.Width + tol2, panel.Height + tol2);
			if (!full.Contains(cursorPos)) return -1;

			Rectangle[] check = new Rectangle[8];
			// Corners: Top Left, Top Right, Bottom Left, Bottom Right
			check[0] = new Rectangle(panel.Left - tolerance, panel.Top - tolerance, tol2, tol2);
			check[1] = new Rectangle(panel.Right - tolerance, panel.Top - tolerance, tol2, tol2);
			check[2] = new Rectangle(panel.Left - tolerance, panel.Bottom - tolerance, tol2, tol2);
			check[3] = new Rectangle(panel.Right - tolerance, panel.Bottom - tolerance, tol2, tol2);
			// Edges: Top, Bottom, Left, Right
			check[4] = new Rectangle(panel.Left, panel.Top - tolerance, panel.Width, tol2);
			check[5] = new Rectangle(panel.Left, panel.Bottom - tolerance, panel.Width, tol2);
			check[6] = new Rectangle(panel.Left - tolerance, panel.Top, tol2, panel.Height);
			check[7] = new Rectangle(panel.Right - tolerance, panel.Top, tol2, panel.Height);

			// Correction since the corners are a bit small.
			for (int i = 0; i < 4; i++) check[i].Inflate(tolerance, tolerance);
				
			// Check if the click was inside any of these boundaries, and find the closest match.
			int closestIndex = -1;
			int closestDistance = int.MaxValue;
			for (int i = 0; i < check.Length; i++)
			{
				if (!check[i].Contains(cursorPos)) continue;

				int x = (check[i].Left + check[i].Right) / 2;
				int y = (check[i].Top + check[i].Bottom) / 2;
				int xlen = (x - cursorPos.X);
				int ylen = (y - cursorPos.Y);
				int dist = (int)Math.Sqrt(xlen * xlen + ylen * ylen);
				if (dist < closestDistance)
				{
					closestDistance = dist;
					closestIndex = i;
				}
			}

			return closestIndex;
		}

		void resizeSelectedPanel(int rawOffsetX, int rawOffsetY, bool drag)
		{
			var item = getPanelModePagePanel();

			short x = (short)rawOffsetX;
			short y = (short)rawOffsetY;
			if (drag)
			{
				_dragCarry.Offset(x, y);
				// The panels are based on 1x scale, and there's a hidden 2x scale in lowdef mode.
				float scale = _mapScale * 2.0f;
				x = (short)(_dragCarry.X / scale);
				y = (short)(_dragCarry.Y / scale);
				_dragCarry.Offset(-x * (int)scale, -y * (int)scale);
			}

			// Corners (0 to 3): Top Left, Top Right, Bottom Left, Bottom Right
			// Edges (4 to 7): Top, Bottom, Left, Right
			bool left = false, right = false, top = false, bottom = false;
			switch (_panelModeEdge)
			{
				case 0: item.Left += x; item.Top += y; left = true; top = true; break;
				case 1: item.Right += x; item.Top += y; right = true; top = true; break;
				case 2: item.Left += x; item.Bottom += y; left = true; bottom = true; break;
				case 3: item.Right += x; item.Bottom += y; right = true; bottom = true; break;
				case 4: item.Top += y; top = true; break;
				case 5: item.Bottom += y; bottom = true; break;
				case 6: item.Left += x; left = true; break;
				case 7: item.Right += x; right = true; break;
			}

			// Clamp to prevent crossing the opposite boundary
			if (left && item.Left > item.Right) item.Left = item.Right;
			if (right && item.Right < item.Left) item.Right = item.Left;
			if (top && item.Top > item.Bottom) item.Top = item.Bottom;
			if (bottom && item.Bottom < item.Top) item.Bottom = item.Top;

			// Clamp to prevent going out of bounds
			if (item.Left < 0) item.Left = 0;
			if (item.Right > 212) item.Right = 212;
			if (item.Top < 0) item.Top = 0;
			if (item.Bottom > 138) item.Bottom = 138;

			markDirty();
			applyPanelConfigToFrontEnd();
			refreshPanelModeVisuals();
		}

		void moveSelectedPanel(int rawOffsetX, int rawOffsetY, bool drag)
		{
			var item = getPanelModePagePanel();

			int x = rawOffsetX;
			int y = rawOffsetY;
			if (drag)
			{
				_dragCarry.Offset(x, y);
				// The panels are based on 1x scale, and there's a hidden 2x scale in lowdef mode.
				float scale = _mapScale * 2.0f;
				x = (int)(_dragCarry.X / scale);
				y = (int)(_dragCarry.Y / scale);
				_dragCarry.Offset(-x * (int)scale, -y * (int)scale);
			}

			if (x < 0 && item.Left > 0)
			{
				x *= -1;
				if (item.Left < x) x = item.Left;

				item.Left -= (short)x;
				item.Right -= (short)x;
			}
			else if (x > 0 && item.Right < 212)
			{
				if (212 - item.Right < x) x = 212 - item.Right;

				item.Left += (short)x;
				item.Right += (short)x;
			}

			if (y < 0 && item.Top > 0)
			{
				y *= -1;
				if (item.Top < y) y = item.Top;

				item.Top -= (short)y;
				item.Bottom -= (short)y;
			}
			else if (y > 0 && item.Bottom < 138)
			{
				if (138 - item.Bottom < y) y = 138 - item.Bottom;

				item.Top += (short)y;
				item.Bottom += (short)y;
			}

			markDirty();
			applyPanelConfigToFrontEnd();
			refreshPanelModeVisuals();
		}

		/// <summary>Retrieves the page template that is currently visible on the map in panel mode.</summary>
		Platform.Xwing.PageTemplate getPanelModePageTemplate()
		{
			var core = getXwingCoreBriefing();
			int pageType = core.Pages[_currentBriefingIndex].PageType;
			return core.Templates[pageType];
		}

		/// <summary>Retrieves the currently selected panel in panel mode.</summary>
		Platform.Xwing.PagePanel getPanelModePagePanel()
		{
			var template = getPanelModePageTemplate();
			// Sometimes this can lose selection when swapping pages.
			int index = Math.Max(0, lstPanelMode.SelectedIndex);
			return template.Items[index];
		}

		/// <summary>Refreshes the text in the panel mode listbox.</summary>
		void refreshPanelModeList() => refreshPanelListBox(lstPanelMode, getPanelModePageTemplate(), true);

		/// <summary>Refreshes the map in panel mode to display the correct selection brackets.</summary>
		void refreshPanelModeVisuals()
		{
			// The text panels, including the selection brackets, are redrawn before the combined
			// canvas is drawn.  But the panel rectangle metrics used for the selection bracket size
			// are not recalculated until the combined canvas is redrawn.  Normally this isn't a
			// problem, but because we're changing the underlying panel size, we need to force the
			// metrics to update before refreshing the final display.  Just for the correct brackets.
			drawCombineCanvas();
			refreshCanvas(RefreshFlags.All);
		}
		#endregion Xwing panel mode

		#region Color retrieval
		Color getXwingIconColor(int iff, int craftType)
		{
			// NOTE: If the icons are loaded directly from the assets, the craft lookups will use
			// whatever bitmaps have been loaded instead of relying on any of these colors.
			if (craftType >= 0 && craftType <= 25)
			{
				iff = resolveIff(craftType, iff, _platform);
				if (craftType > 0 || (craftType == 0 && iff >= 2)) return getIconColor(iff);
			}

			// If the original icons are loaded, everything else should be IFF green.
			// This includes all icons, regardless of what color they actually are.
			if (_iconAssetSource != AssetSourceType.None) return getIconColor(1);

			// If we get here, this assumes the icons are loaded from TIE_BRF.bmp and mapped from TIE.
			// If an icon strip is generated for XW and uses the correct colors for the misc objects,
			// then this can be removed and just return white to use the unaltered colors from the bitmap.
			iff = 0;
			if (craftType >= 26 && craftType <= 33) iff = 2;        // Asteroids (red)
			else if (craftType >= 34 && craftType <= 48) iff = 1;   // Planets (green)
			else if (craftType == 49) iff = 3;                      // Death Star (blue)
			if (iff > 0) return getIconColor(iff);

			// If the assets aren't loaded, we won't have the proper icons, or anything close enough,
			// for the misc object items.  No need to tint them.
			return Color.White;
		}

		Color[] getPaletteFromIff(int iff)
		{
			if (iff < 0 || iff >= _iconIffPaletteIndices.Length) iff = 0;
			return _textTagPalettes[_iconIffPaletteIndices[iff]];
		}

		Color getIconColor(int iff)
		{
			if (iff < 0) iff = 0;
			// In XWING, everything beyond is neutral.  Otherwise default to green.
			if (iff >= _iconIffColors.Length) iff = (isXwing ? _iconIffColors.Length - 1 : 0);
			return _iconIffColors[iff];
		}

		Color getTextTagColor(int index)
		{
			if (index < 0 || index >= _textTagColors.Length) index = 0;
			return _textTagColors[index];
		}

		Color[] getTextTagColorPalette(int index)
		{
			if (index < 0 || index >= _textTagPalettes.Length) index = 0;
			return _textTagPalettes[index];
		}

		/// <summary>Retrieves the highest shade of a palette color, typically used for the caret square when drawing text tags.</summary>
		Color getTextTagCaret(int index)
		{
			if (index < 0 || index >= _textTagPalettes.Length) index = 0;
			return _textTagPalettes[index][_textTagPalettes[index].Length - 1];
		}

		Color getStandardColor(StandardColor type) => _standardColors[(int)type];

		/// <summary>Given a raw color code from an inline text color value, retrieve the color to render the subsequent characters.</summary>
		Color getInlineTextColor(char c, Color defaultColor)
		{
			// Char 0 is interpreted as newline or null terminator.  It should not be passed here.
			// Char 1 is usually the default input color for the render operation, and shouldn't be used here.
			Color ret = defaultColor;
			if (isXwing || isTie || c <= 2) ret = (c == 2 ? getStandardColor(StandardColor.Highlight) : defaultColor);
			else if (isXvt || isXwa)
			{
				int colIndex = (c - 2) + 1;
				if (colIndex < _inlineTextColors.Length) ret = _inlineTextColors[colIndex];
			}
			return ret;
		}
		#endregion Color retrieval

		#region String retrieval and utilities
		string getSafeString(List<string> data, int index) => (index >= 0 && index < data.Count ? data[index] : "");
		string getSafeString(string[] data, int index) => (index >= 0 && index < data.Length ? data[index] : "");
		
		bool stringEqual(string a, string b) => (string.Compare(a, b, StringComparison.OrdinalIgnoreCase) == 0);

		int getUnescapedStringLength(string s)
		{
			int pos = 0, count = 0;
			while (pos < s.Length)
			{
				char c = s[pos++];
				char next = (pos < s.Length ? s[pos] : '\0');
				count++;
				if (c == '\\' && (next == '\\' || (next >= '0' && next <= '9'))) pos++;
			}
			return count;
		}
		
		string getTextTag(int index)
		{
			string s = getSafeString(_tags, index);

			// Truncate if there's an escaped null terminator.
			// The string can never exceed this length no matter the time.
			int pos = s.IndexOf("\\0");
			if (pos >= 0) s = s.Remove(pos);
			return s;
		}

		char[] getUnescapedString(string s, out int length)
		{
			// The resulting string might be shorter, but never longer than the original escaped string.
			// This is really only for proper interpretation of null characters.
			char[] str = new char[s.Length];
			int pos = 0, wpos = 0;
			while (pos < s.Length)
			{
				char c = s[pos++];
				char next = (pos < s.Length ? s[pos] : '\0');
				if (c == '\\')
				{
					if (next == '\\') pos++;
					else if (next >= '0' && next <= '9')
					{
						c = (char)(next - '0');
						pos++;
					}
				}
				str[wpos++] = c;
			}
			length = wpos;
			return str;
		}
		
		/// <summary>Measures the amount of space required to display a text tag.</summary>
		/// <param name="text">String to be measured.  May contain control codes.</param>
		/// <param name="font">Font to use when retrieving glyph sizes.</param>
		/// <returns>A Size item containing the width and height required to display the text.</returns>
		Size measureTagString(string text, BriefingFont font)
		{
			int w = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				char next = (char)0;
				if (i < text.Length - 1) next = text[i + 1];

				if (c == '\\')
				{
					if (next == '0') break;
					if (next >= '1' && next <= '9') i++;
					continue;
				}

				if (!isXwing && (c == '[' || c == ']')) continue;
				
				var glyph = font.GetChar(c);
				w += glyph.Width + font.HorizontalSpacing;
			}

			return new Size(w, font.MaxHeight);
		}
		
		string getCraftTypeAbbrev(int craftType)
		{
			string s = getSafeString(_craftAbbrev, craftType);
			return (s != "" ? s : "???");
		}

		string getCraftDisplayName(string abbrev, string name) => (abbrev + " " + name).Trim();
		#endregion String retrieval and utilities

		#region Bitmap retrieval and utils
		long getBitmapCacheKey(BitmapCacheType type, int typeIndex, Color color, int rotation)
		{
			// Compose a 64 bit key.  The high 32 bits are subdivided into Type and Index.  The low 32 bits contain the RGB color (and rotation, if applicable).
			long typeId = ((int)type << 16) | typeIndex;
			long colorId = (rotation << 24) | (color.R << 16) | (color.G << 8) | color.B;
			return (typeId << 32) | colorId;
		}

		/// <summary>Retrieves a previously tinted bitmap from the cache, or generates a new tinted bitmap from the provided source and color.</summary>
		/// <remarks>Tinting a bitmap before every draw operation is expensive.  This speeds up the process by caching all tinted bitmaps, and retrieving from the cache when available.</remarks>
		/// <param name="type">For grouping purposes, to keep bitmaps distinct since they're all stored in a single cache.</param>
		/// <param name="typeIndex">For craft icons, this is the craft type index.  For fonts, this is the character code.</param>
		/// <param name="source">If nothing exists in the cache, this is the source Bitmap that will be tinted.</param>
		/// <param name="color">RGB color to tint.  Use <b>Color.White</b> for no tinting.</param>
		/// <param name="rotation">XWA rotation to use.  Zero for no rotation.</param>
		/// <returns>The tinted Bitmap, or the source Bitmap if no tinting was performed.</returns>
		Bitmap getCachedTintedBitmap(BitmapCacheType type, int typeIndex, Bitmap source, Color color, int rotation)
		{
			long key = getBitmapCacheKey(type, typeIndex, color, rotation);
			if (type != BitmapCacheType.None && _bitmapCache.ContainsKey(key)) return _bitmapCache[key];

			if (color == Color.White && type != BitmapCacheType.None)
			{
				_bitmapCache.Add(key, source);
				return source;
			}

			// Loading the XWA icon CBMs introduced a problem.  Since the icons are loaded directly
			// into the cache, attempting to rotate would pull from the default untinted sources.
			// Try to find the original icon in the cache, but without rotation.  If found, use that.
			Bitmap bmp = null;
			if (rotation != 0)
			{
				long key2 = getBitmapCacheKey(type, typeIndex, color, 0);
				if (_bitmapCache.ContainsKey(key2)) bmp = _bitmapCache[key2];
			}
			if (bmp == null) bmp = getTintedBitmap(source, color);
			if (rotation != 0) bmp = rotateBitmap(bmp, rotation);
			if (type != BitmapCacheType.None) _bitmapCache.Add(key, bmp);
			return bmp;
		}

		/// <summary>Returns a new bitmap tinted to the specified color.</summary>
		Bitmap getTintedBitmap(Bitmap bitmap, Color color)
		{
			Bitmap bmp = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.InterpolationMode = InterpolationMode.NearestNeighbor;
				g.DrawImage(bitmap, 0, 0);

				Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
				var bmpData = bmp.LockBits(lockRect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

				int stride = Math.Abs(bmpData.Stride);
				int lockSize = stride * bmpData.Height;
				byte[] pixelData = new byte[lockSize];
				Marshal.Copy(bmpData.Scan0, pixelData, 0, lockSize);

				// Convert the source pixels to grayscale, then apply the requested color.
				for (int y = 0; y < bmpData.Height; y++)
				{
					int scanline = y * stride;
					// 3 bytes per pixel.  Byte order in memory is BGR.
					for (int p = 0; p < bmpData.Width * 3; p += 3)
					{
						// Using the blue channel of the source as a grayscale intensity.
						int offset = scanline + p;
						byte b = pixelData[offset];

						// Font shadows use an intensity of 1 to simulate black without being transparent.
						if (b <= 1) continue;

						pixelData[offset + 0] = (byte)((b * color.B) / 255);
						pixelData[offset + 1] = (byte)((b * color.G) / 255);
						pixelData[offset + 2] = (byte)((b * color.R) / 255);
					}
				}

				Marshal.Copy(pixelData, 0, bmpData.Scan0, lockSize);
				bmp.UnlockBits(bmpData);
			}
			bmp.MakeTransparent(Color.Black);
			return bmp;
		}

		/// <summary>Flips or rotates a bitmap according to XWA rotations.</summary>
		/// <param name="source">Source bitmap to rotate.  It will not be altered.</param>
		/// <param name="xwaRotation">XWA-style event rotation to apply.  Use <b>zero</b> for no rotation.</param>
		/// <returns>A new bitmap with the rotation applied.</returns>
		Bitmap rotateBitmap(Bitmap source, int xwaRotation)
		{
			if (xwaRotation <= 0 || xwaRotation > 4) return source;

			Bitmap bmp = new Bitmap(source);
			if (xwaRotation == 1) bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
			else if (xwaRotation == 2) bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
			else if (xwaRotation == 3) bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
			else if (xwaRotation == 4) bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
			return bmp;
		}

		CraftIconImage getCraftIconData(int craftType)
		{
			if (craftType < 0 || craftType >= _craftIconImages.Count) craftType = 0;
			return _craftIconImages[craftType];
		}
		Bitmap getCraftIcon(int craftType, Color color, int rotation)
		{
			if (craftType < 0 || craftType >= _craftIconImages.Count) craftType = 0;
			var icon = _craftIconImages[craftType];
			return getCachedTintedBitmap(BitmapCacheType.CraftIcon, craftType, icon.Icon, color, rotation);
		}

		Bitmap getFlatCraftIcon(int craftType, Color color, int rotation)
		{
			if (_craftFlatIconImages == null) return getCraftIcon(craftType, color, rotation);

			var icon = _craftFlatIconImages[craftType];
			return getCachedTintedBitmap(BitmapCacheType.FlatCraftIcon, craftType, icon.Icon, color, rotation);
		}
		#endregion Bitmap retrieval and utils

		#region Position and movement
		/// <summary>Calculates how many raw map units are within a single pixel at the current zoom level.</summary>
		/// <remarks>Ignores custom zoom while in free look mode.</remarks>
		int getPixelRawX()
		{
			float scale = (isXwing && isXwingHighDef() ? 128.0f : 256.0f);
			int zoom = (_freeLookMode ? _backupZoom.X : _currentZoom.X);
			return Math.Max(1, (int)(scale / zoom));
		}
		///<inheritdoc cref="getPixelRawX"/>
		int getPixelRawY()
		{
			float scale = (isXwing && isXwingHighDef() ? 128.0f : 256.0f);
			int zoom = (_freeLookMode ? _backupZoom.Y : _currentZoom.Y);
			return Math.Max(1, (int)(scale / zoom));
		}

		/// <summary>Gets the movement increment, in raw units, that was applied to the positional numeric control.</summary>
		int getMoveIncrementX()
		{
			int value = (int)numX.Increment;
			if (isXwa && cboMoveIncrement.SelectedIndex == 17) value = (int)numIconStepX.Value;
			return Math.Max(1, value);
		}
		///<inheritdoc cref="getMoveIncrementX"/>
		int getMoveIncrementY()
		{
			int value = (int)numY.Increment;
			if (isXwa && cboMoveIncrement.SelectedIndex == 17) value = (int)numIconStepY.Value;
			return Math.Max(1, value);
		}

		/// <summary>Used to snap a mouse click position to grid, if applicable.</summary>
		Point getGridSnappedPosition(Point p)
		{
			// When operating on a mouse click, the nudge helps center over the appropriate snap position on the grid.
			int snapX = getMoveIncrementX();
			int nudgeX = (p.X > 0 ? 1 : -1) * (snapX / 2);
			int x = ((p.X + nudgeX) / snapX) * snapX;

			int snapY = getMoveIncrementY();
			int nudgeY = (p.Y > 0 ? 1 : -1) * (snapY / 2);
			int y = ((p.Y + nudgeY) / snapY) * snapY;

			return new Point(x, y);
		}

		short getNewAxisPosition(int value, int offset, int snapAmount)
		{
			int result = value + offset;
			if (snapAmount != 0) result = (result / snapAmount) * snapAmount;
			return (short)clamp(result, short.MinValue, short.MaxValue);
		}

		void moveSelectedItems(int rawOffsetX, int rawOffsetY, bool mouseDrag)
		{
			rawOffsetY = getInvertedY(rawOffsetY);

			int snapX = getMoveIncrementX();
			int snapY = getMoveIncrementY();

			if (mouseDrag)
			{
				if (chkMoveLockX.Checked) rawOffsetX = 0;
				if (chkMoveLockY.Checked) rawOffsetY = 0;

				if (chkMoveSnapGrid.Checked | chkMoveSnapMouse.Checked)
				{
					_dragCarry.X += rawOffsetX;
					if (_dragCarry.X / snapX != 0)
					{
						int step = _dragCarry.X / snapX;
						_dragCarry.X -= (step * snapX);
						rawOffsetX = step * snapX;
					}
					else rawOffsetX = 0;

					_dragCarry.Y += rawOffsetY;
					if (_dragCarry.Y / snapY != 0)
					{
						int step = _dragCarry.Y / snapY;
						_dragCarry.Y -= (step * snapY);
						rawOffsetY = step * snapY;
					}
					else rawOffsetY = 0;
				}
			}

			// Mousedrag snap is accounted for, if applicable.  From here on, it's only for grid snap.
			if (!chkMoveSnapGrid.Checked)
			{
				snapX = 0;
				snapY = 0;
			}

			if (rawOffsetX == 0 && rawOffsetY == 0) return;

			if (_tempEvent != AbstractEventType.None)
			{
				switch (_tempEvent)
				{
					case AbstractEventType.TextTag1:
					case AbstractEventType.XwaMoveIcon:
					case AbstractEventType.MoveMap:
					case AbstractEventType.XwaSetIcon:
					case AbstractEventType.XwingNewIcon:
						if (_tempEvent == AbstractEventType.XwaSetIcon && !chkAutoMoveIcon.Checked) break;

						// The map coordinates use the same units as everything else, but they can't be inverted.
						// For the sake of moving with the arrow keys, we need to flip back if necessary.
						if (_tempEvent == AbstractEventType.MoveMap) rawOffsetY = getInvertedY(rawOffsetY);

						// Avoids changing both if only one value is changed.
						if (rawOffsetX != 0) safeSetNumeric(numX, getNewAxisPosition((int)numX.Value, rawOffsetX, snapX));
						if (rawOffsetY != 0) safeSetNumeric(numY, getNewAxisPosition((int)numY.Value, rawOffsetY, snapY));

						break;
				}

				// Don't do anything else for temp events.
				return;
			}

			// No temp events active, apply to selected items.
			foreach (var so in _selectedObjects)
			{
				if (isValidWaypoint(so))
				{
					var afg = _flightgroups[so.Index];
					updateUndoProperty(ref so.UndoUID, afg.GetDataSnapshot(), UndoType.FlightgroupMove, so.Index, afg.DisplayWaypoint);
					afg.Waypoints[afg.DisplayWaypoint].RawX = getNewAxisPosition(afg.Waypoints[afg.DisplayWaypoint].RawX, rawOffsetX, snapX);
					afg.Waypoints[afg.DisplayWaypoint].RawY = getNewAxisPosition(afg.Waypoints[afg.DisplayWaypoint].RawY, rawOffsetY, snapY);
					updateKnownUndoProperty(so.UndoUID, afg.GetDataSnapshot());
					notifyWaypointChange(so.Index);
				}
				else if (isValidEvent(so))
				{
					var evt = _events[so.Index];

					// Note: if this changes, make sure the X/Y parameter indices match.  Conveniently text
					// tags and icons use the same parameters for position, so we can handle both here.
					if (evt.IsTextTag || evt.Event == AbstractEventType.XwaMoveIcon)
					{
						MapElement main = null, shad = null;

						// Make sure it's enabled on the map.  Something could be selected in the timeline, and we don't
						// want to move things that shouldn't be interactable.
						foreach (var elem in _mapTextTags) if (elem.Enabled && elem.Enabled && elem.EventUid == evt.UID) main = elem;

						foreach (var elem in _mapIcons) if (elem.Enabled && elem.LastMoveEventUid == evt.UID) main = elem;

						// Every visible map icon has at least one matching shadow icon entry.  If the main
						// icon is selected, we need to sync its corresponding shadow entry too.
						// Shadows still exist even when disabled in the map options, they're just not drawn.
						// The interact determines whether they're capable of being moved.
						foreach (var elem in _mapShadowIcons)
						{
							if (!elem.Enabled || elem.LastMoveEventUid != evt.UID) continue;

							if (main != null && main.LastMoveEventUid == elem.LastMoveEventUid) shad = elem;
							else if (isShadowIconVisible(elem) && elem.InteractRect.Width > 0) shad = elem;
						}

						if (main == null && shad == null) continue;

						updateUndoProperty(ref so.UndoUID, evt.GetDataSnapshot(), UndoType.Event, so.Index, -1);
						evt.Params[1] = getNewAxisPosition(evt.Params[1], rawOffsetX, snapX);
						evt.Params[2] = getNewAxisPosition(evt.Params[2], rawOffsetY, snapY);
						updateKnownUndoProperty(so.UndoUID, evt.GetDataSnapshot());

						// Synchronize the linked elements.
						if (main != null) { main.X = evt.Params[1]; main.Y = evt.Params[2]; }
						if (shad != null) { shad.X = evt.Params[1]; shad.Y = evt.Params[2]; }

						_lastEditEventUid = evt.UID;
						markDirty();
					}
				}
				else if (isValidPathNode(so))
				{
					_pathNodes[so.Index].X = getNewAxisPosition(_pathNodes[so.Index].X, rawOffsetX, snapX);
					_pathNodes[so.Index].Y = getNewAxisPosition(_pathNodes[so.Index].Y, rawOffsetY, snapY);
				}
			}
		}

		void positionChanged(bool xmodified, bool ymodified)
		{
			int x = (int)numX.Value;
			int y = (int)numY.Value;

			// The linking checkbox is only visible when editing a Zoom event.
			// When linked, both values must be synchronized to whatever was edited.
			bool linked = isVisibleChecked(chkLinkZoom);
			if (linked)
			{
				if (xmodified) y = x;
				else if (ymodified) x = y;
			}

			if (_tempEvent == AbstractEventType.None)
			{
				bool absolute = true;

				if (_selectedObjects.Count > 1)
				{
					var pso = getPrimarySelectedObject();
					// PathNodes don't follow the sidebar refresh logic, so the multi movement options aren't visible.
					// PathNodes will always have relative movement.
					if (pso != null && ((pnlMoveMulti.Visible && optMoveRelative.Checked) || pso.Type == SelectedType.PathNode))
					{
						absolute = false;

						// Get the position of the primary item.  Apply relative movement based on the delta
						// between its current position and the values retrieved from the numeric controls.
						if (getPrimaryPosition(pso, out Point pos))
						{
							if (pso.Type != SelectedType.PathNode) pos.Y = getInvertedY(pos.Y);
							moveSelectedItems(x - pos.X, y - pos.Y, false);
						}
					}
				}

				if (absolute)
				{
					// Anything else is an event parameter.
					if (linked)
					{
						modifyParam(EditParamType.X, x, false);
						modifyParam(EditParamType.Y, y, true);
					}
					else
					{
						if (xmodified) modifyParam(EditParamType.X, x);
						else modifyParam(EditParamType.Y, y);
					}
				}
			}
			else
			{
				if (_tempEvent == AbstractEventType.MoveMap) _currentPosition = new Point(x, y);
				else if (_tempEvent == AbstractEventType.ZoomMap) _currentZoom = new Point(x, y);
			}

			// Easiest way to refresh the controls to reflect their new values, without triggering more UI event updates.
			if (linked) setEditPositionValues(x, y);
			if (hsbBRF.Visible || vsbBRF.Visible) setEditScrollbarValues(x, y);
			refreshMap();
		}
		#endregion Position and movement

		#region Duration
		string getTimeString(int brfTicks, bool verbose = false)
		{
			float seconds = (float)brfTicks / _ticksPerSecond;
			string s = seconds.ToString("0.00");
			if (verbose) s += " sec";
			return s;
		}

		int getMinimumDuration() => (int)(_ticksPerSecond * numConfigAutoDuration.Value);

		/// <summary>Sets the briefing duration, in ticks.  Can be used to initialize the navigation controls, or to change the actual duration.</summary>
		/// <remarks>This is frequently set when autoadjusting duration when inserting events.  If manually adjusted by the user, a new undo frame should be created first.</remarks>
		void setDuration(int newTime, bool makeUndo)
		{
			if (newTime == _briefingDuration) return;

			newTime = clamp(newTime, getMinimumDuration(), MaxEventTime);
			if (newTime < hsbTimer.Value) jumpToTime(newTime, true);

			if (makeUndo)
			{
				UndoOperation op = new UndoOperation(UndoType.Duration, _briefingDuration, 0);
				op.Update(newTime);
				addUpdatedUndoOperation(op, false);
			}

			_briefingDuration = (short)newTime;
			// In order for the user to manually scroll to the last tick, it needs this extra padding.
			hsbTimer.Maximum = getMaximumScrollTime() + (hsbTimer.LargeChange - 1);

			// Avoid invoking a value change if triggered by directly changing the control value.
			if (ActiveControl != numDurationSec) numDurationSec.Value = (decimal)Math.Ceiling((double)newTime / _ticksPerSecond);
			refreshTimeline();
		}
		
		void autoAdjustDuration(bool makeUndo)
		{
			if (!chkConfigAutoDuration.Checked) return;

			int newDuration = getHighestTime() + (int)(_ticksPerSecond * numConfigAutoDuration.Value);
			if (newDuration > _briefingDuration) setDuration(newDuration, makeUndo);
		}

		/// <summary>Retrieves the time index of the last event, or zero if the event list is empty.</summary>
		int getHighestTime() => (_events.Count > 0 ? _events[_events.Count - 1].Time : 0);

		/// <summary>Returns the highest useful time that the timer scrollbar can reach.</summary>
		/// <remarks>The scrollbar's Maximum value is influenced by LargeChange.  In order to scroll to the end, Maximum needs to be padded.  This function returns the real maximum time.</remarks>
		int getMaximumScrollTime() => Math.Max(_briefingDuration, _highestEventTime);
		#endregion Duration

		#region Xwa icon editing
		void rebuildShadowIcons()
		{
			_mapShadowIcons.Clear();

			int regionPage = 0;
			int regionIndex = 0;
			int baseIndex = 0;
			int iconCount = _mapIcons.Length;
			const int RegionCount = 4;
			bool[] enabled = new bool[iconCount * RegionCount];
			short[] craftType = new short[iconCount * RegionCount];
			short[] craftIff = new short[iconCount * RegionCount];
			short[] rotation = new short[iconCount * RegionCount];
			short[] lastRotIndex = new short[iconCount * RegionCount];
			for (int i = 0; i < lastRotIndex.Length; i++) lastRotIndex[i] = -1;

			int index;
			int lastTime = -1;
			int groupIndex = 0;

			for (int i = 0; i < _events.Count; i++)
			{
				var evt = _events[i];
				if (evt.Time != lastTime)
				{
					lastTime = evt.Time;
					groupIndex = _mapShadowIcons.Count;
				}
				if (evt.Event != AbstractEventType.XwaChangeRegion && (regionIndex < 0 || regionIndex >= RegionCount)) continue;

				switch (evt.Event)
				{
					case AbstractEventType.XwaChangeRegion:
						regionIndex = evt.Params[0];
						baseIndex = regionIndex * iconCount;
						regionPage++;
						break;

					case AbstractEventType.XwaSetIcon:
						index = evt.Params[0];
						if (index >= 0 && index < iconCount)
						{
							enabled[baseIndex + index] = true;
							craftType[baseIndex + index] = evt.Params[1];
							craftIff[baseIndex + index] = evt.Params[2];
						}
						break;

					case AbstractEventType.XwaMoveIcon:
						index = evt.Params[0];
						if (index >= 0 && index < iconCount && craftType[baseIndex + index] > 0)
						{
							MapElement elem = new MapElement
							{
								Enabled = true,
								EventIndex = i,
								EventUid = evt.UID,
								LastMoveEventIndex = i,
								LastMoveEventUid = evt.UID,
								LastRotateEventIndex = lastRotIndex[baseIndex + index],
								X = evt.Params[1],
								Y = evt.Params[2],
								DataIndex = index,
								Color = craftIff[baseIndex + index],
								IconCraftType = craftType[baseIndex + index],
								IconRotation = rotation[baseIndex + index],
								IconTime = evt.Time,
								IconRegionPage = regionPage,
								IconRegionIndex = regionIndex
							};
							_mapShadowIcons.Add(elem);
						}
						break;

					case AbstractEventType.XwaRotateIcon:
						index = evt.Params[0];
						if (index >= 0 && index < iconCount)
						{
							rotation[baseIndex + index] = evt.Params[1];
							lastRotIndex[baseIndex + index] = (short)i;

							// A Rotation event might follow a Move event at the same time index.
							// In this case, the Rotation must retroactively apply to the Move.
							for (int j = groupIndex; j < _mapShadowIcons.Count; j++)
							{
								if (_mapShadowIcons[j].DataIndex != index || _mapShadowIcons[j].IconTime != lastTime) continue;

								_mapShadowIcons[j].IconRotation = evt.Params[1];
								_mapShadowIcons[j].LastRotateEventIndex = i;
							}
						}
						break;
				}
			}

			refreshShadowIconDropdown();
			refreshMap();
		}

		void refreshShadowIconDropdown()
		{
			bool btemp = _loading;
			_loading = true;

			const int DefaultCount = 2;
			string[] items = new string[DefaultCount + _mapIcons.Length];
			items[0] = cboShadowFilter.Items[0].ToString();
			items[1] = cboShadowFilter.Items[1].ToString();

			for (int i = 0; i < _mapIcons.Length; i++) items[DefaultCount + i] = "#" + (i + 1);

			foreach (var elem in _mapShadowIcons)
			{
				if (elem.IconRegionPage != _currentRegionPage) continue;

				if (elem.IconCraftType > 0 && elem.IconCraftType < _craftNames.Length)
					items[DefaultCount + elem.DataIndex] = "#" + (elem.DataIndex + 1) + " " + _craftNames[elem.IconCraftType];
			}

			int selection = cboShadowFilter.SelectedIndex;
			cboShadowFilter.Items.Clear();
			cboShadowFilter.Items.AddRange(items);
			cboShadowFilter.SelectedIndex = selection;

			_loading = btemp;
		}

		/// <summary>Resolves the last selected MoveIcon event via UID lookup.</summary>
		/// <returns>-1 if not found or invalid.  Otherwise a valid index into the event list.</returns>
		int getWorkingMoveIcon()
		{
			int eventIndex = getEventIndexByUid(_lastSelectedMoveIconUid);
			if (eventIndex >= 0 && _events[eventIndex].Event == AbstractEventType.XwaMoveIcon) return eventIndex;

			_lastSelectedMoveIconUid = -1;
			return -1;
		}

		/// <summary>Retrieves a shadow icon by its MoveIcon event UID.</summary>
		MapElement getShadowIconByUid(int uid)
		{
			foreach (var elem in _mapShadowIcons) if (elem.EventUid == uid) return elem;
			return null;
		}

		/// <summary>Retrieves the array index of one of the 8-way directional MoveIcon controls</summary>
		int getDirectionalMoveButtonIndex(object control)
		{
			// Numpad5 is null so need to check before testing a match.
			int result = -1;
			if (control != null)
				for (int i = 0; i < _moveButtons.Length; i++) if (_moveButtons[i] == control) result = i;

			return result;
		}

		static Point getNullPoint() => new Point(int.MaxValue, int.MaxValue);
		void backupFirstPreviewStep()
		{
			Point cur = new Point((int)numIconStepX.Value, (int)numIconStepY.Value);
			if (_firstPreviewStep == getNullPoint()) _firstPreviewStep = cur;
			cmdMoveReset.Enabled = (_firstPreviewStep != cur);
		}
		void restoreFirstPreviewStep()
		{
			if (_firstPreviewStep == getNullPoint()) return;

			safeSetNumeric(numIconStepX, _firstPreviewStep.X);
			safeSetNumeric(numIconStepY, _firstPreviewStep.Y);
			_firstPreviewStep = getNullPoint();
			cmdMoveReset.Enabled = false;
		}

		/// <summary>Creates a move event for a selected icon that doesn't have a move event.</summary>
		void createAutoMove()
		{
			if (_selectedObjects.Count != 1 || !isValidEvent(_selectedObjects[0])) return;

			var evt = _events[_selectedObjects[0].Index];

			if (evt.Event != AbstractEventType.XwaSetIcon) return;
			if (isIconInitialized(evt.Time, evt.Params[0])) return;

			if (!hasAvailableSpace(AbstractEventType.XwaMoveIcon))
			{
				popupError("Not enough space to create a Move event");
				return;
			}

			// Create at (0, 0) which is the default for icons without a modified position.
			// Try the current time position, but not if it's before the icon is created.
			int time = Math.Max(hsbTimer.Value, evt.Time);
			AbstractEvent newEvt = new AbstractEvent((short)time, AbstractEventType.XwaMoveIcon, evt.Params[0], 0, 0);
			int pos = getInsertionIndexForEvent(time, false);
			beginUndoFrame(true);
			insertEvent(pos, newEvt, true);
			rebuildShadowIcons();
			rebuildBriefing();
			selectEventByUid(newEvt.UID);
		}

		void refreshIconAcceleration()
		{
			Point cur = new Point((int)numIconStepX.Value, (int)numIconStepY.Value);
			cmdMoveReset.Enabled = (_firstPreviewStep != getNullPoint() && _firstPreviewStep != cur);

			bool state = !optIconAccelNone.Checked;
			numIconAccelX.Enabled = state;
			numIconAccelY.Enabled = state;
			chkIconLinkAccel.Enabled = state;
			chkIconStepAccel.Enabled = state;
		}

		void generateDirectionalIconMovement(object control, bool useBatchCount)
		{
			backupFirstPreviewStep();
			refreshDirectionalPreview(control, useBatchCount);
			int curMoveEventIndex = getWorkingMoveIcon();
			int curButtonIndex = getDirectionalMoveButtonIndex(ActiveControl);
			if (curMoveEventIndex == -1 || _mapPreviewIcons.Count == 0) return;

			if (!verifyPreviewIcons(false))
			{
				// If it errors out from using the numpad, make sure the preview is removed.
				_mapPreviewIcons.Clear();
				refreshMap();
				return;
			}

			int baseTime = _events[curMoveEventIndex].Time;
			if (hsbTimer.Value > baseTime) baseTime = hsbTimer.Value;
			int lastUid = generateIconsFromPreview(baseTime);
			if (lastUid < 0) return;

			if (chkIconStepAccel.Checked)
			{
				safeSetNumeric(numIconStepX, _lastPreviewStep.X);
				safeSetNumeric(numIconStepY, _lastPreviewStep.Y);
				backupFirstPreviewStep();  // It's already backed up, but this refreshes the label too
			}

			// Update our working object to the last object we added, so new edits can continue where we left off.
			_lastSelectedMoveIconUid = lastUid;
			rebuildShadowIcons();
			relinkSelectedEventUid();

			// Update our currently selected object to point to the last generated icon.
			int newMoveEventIndex = getEventIndexByUid(lastUid);
			foreach (var so in _selectedObjects)
			{
				if (so.Type != SelectedType.Event || so.Index != curMoveEventIndex) continue;

				so.Index = newMoveEventIndex;
				so.EventUID = lastUid;
			}

			if (chkAdvanceTime.Checked) jumpToTime(_events[newMoveEventIndex].Time, true);

			// If the user actively clicked a directional button, generate new previews.
			// If they didn't click, it was a hotkey.  We no longer need the generated icons.
			if (curButtonIndex >= 0) refreshDirectionalPreview(control, useBatchCount);
			else _mapPreviewIcons.Clear();

			refreshMap();
			refreshTimeline();
		}

		int generateIconsFromPreview(int baseTime)
		{
			if (_mapPreviewIcons.Count == 0) return -1;

			beginUndoFrame(true);
			int lastUid = -1;

			// Derive a starting rotation from the working icon.
			int lastRotation = -1;
			var shadow = getShadowIconByUid(_lastSelectedMoveIconUid);
			if (shadow != null) lastRotation = shadow.IconRotation;

			// Estimate the duration of the path to auto adjust the total briefing duration.  Normally inserted
			// events will extend the duration when necessary.  This step avoids invoking a duration extension
			// for every single generated icon.
			int estDuration = baseTime + _mapPreviewIcons[_mapPreviewIcons.Count - 1].IconTime + _ticksPerSecond;
			if (estDuration > _briefingDuration) setDuration(estDuration, true);

			// Generate all the necessary events to create the path.
			// Existing items that belong to this icon will be removed or replaced.
			foreach (var elem in _mapPreviewIcons)
			{
				int time = baseTime + elem.IconTime;

				// See if a MoveIcon event for this icon already exists.  If so, replace it.
				int existEvtIndex = -1;
				var existEvt = getIconEventAtTime(AbstractEventType.XwaMoveIcon, elem.DataIndex, time, out existEvtIndex);
				if (existEvt != null)
				{
					// If the position is different, update.
					if (existEvt.Params[1] != elem.X || existEvt.Params[2] != elem.Y)
					{
						UndoOperation op = new UndoOperation(UndoType.Event, existEvt.GetDataSnapshot(), existEvtIndex);
						existEvt.Params[1] = (short)elem.X;
						existEvt.Params[2] = (short)elem.Y;
						op.Update(existEvt.GetDataSnapshot());
						addUpdatedUndoOperation(op, false);
					}
					lastUid = existEvt.UID;
				}
				else
				{
					// Create new event.
					if (!hasAvailableSpace(AbstractEventType.XwaMoveIcon))
					{
						popupError("Not enough event space to create more MoveIcon events.");
						break;
					}

					AbstractEvent newEvt = new AbstractEvent((short)time, AbstractEventType.XwaMoveIcon, (short)elem.DataIndex, (short)elem.X, (short)elem.Y);
					int destIndex = getInsertionIndexForEvent(newEvt.Time, false);
					insertEvent(destIndex, newEvt, true);
					lastUid = newEvt.UID;
				}
				
				// Check for updated rotations, and place them.
				if (elem.IconRotation != lastRotation)
				{
					lastRotation = elem.IconRotation;

					existEvt = getIconEventAtTime(AbstractEventType.XwaRotateIcon, elem.DataIndex, time, out existEvtIndex);
					if (existEvt != null)
					{
						// If the rotation is different, update.
						if (existEvt.Params[1] != elem.IconRotation)
						{
							UndoOperation op = new UndoOperation(UndoType.Event, existEvt.GetDataSnapshot(), existEvtIndex);
							existEvt.Params[1] = (short)elem.IconRotation;
							op.Update(existEvt.GetDataSnapshot());
							addUpdatedUndoOperation(op, false);
						}
					}
					else
					{
						// Create new rotation event.
						if (!hasAvailableSpace(AbstractEventType.XwaRotateIcon))
						{
							popupError("Not enough event space to create more RotateIcon events.");
							break;
						}

						AbstractEvent newEvt = new AbstractEvent((short)time, AbstractEventType.XwaRotateIcon, (short)elem.DataIndex, (short)elem.IconRotation);
						int destIndex = getInsertionIndexForEvent(newEvt.Time, false);
						insertEvent(destIndex, newEvt, true);
					}
				}
				else
				{
					// No change in rotation.  Check for an existing rotate event at this time and delete it.
					existEvt = getIconEventAtTime(AbstractEventType.XwaRotateIcon, elem.DataIndex, time, out existEvtIndex);
					if (existEvt != null)
					{
						var data = existEvt.GetDataSnapshot();
						UndoOperation op = new UndoOperation(UndoType.DeleteEvent, data, existEvtIndex);
						op.Update(data);
						addUpdatedUndoOperation(op, false);
						deleteEvent(existEvtIndex);
					}
				}
			}
			return lastUid;
		}

		void refreshDirectionalPreview(object control, bool useBatchCount)
		{
			_mapPreviewIcons.Clear();

			int eventIndex = getWorkingMoveIcon();
			int buttonIndex = getDirectionalMoveButtonIndex(control);

			if (eventIndex >= 0 && buttonIndex >= 0)
			{
				int stepX = (int)numIconStepX.Value;
				int stepY = (int)numIconStepY.Value;
				int accelX = (int)numIconAccelX.Value;
				int accelY = (int)numIconAccelY.Value;
				bool useAccel = (!optIconAccelNone.Checked && accelX != 0 || accelY != 0);
				var moveEvt = _events[eventIndex];
				int iconIndex = moveEvt.Params[0];
				int curX = moveEvt.Params[1];
				int curY = moveEvt.Params[2];
				if (iconIndex >= 0 && iconIndex < _mapIcons.Length && (stepX != 0 || stepY != 0))
				{
					var source = _mapIcons[iconIndex];
					int count = (useBatchCount ? (int)numMoveStep.Value : 1);
					for (int i = 0; i < count; i++)
					{
						MapElement elem = new MapElement
						{
							Enabled = true,
							EventIndex = -1,
							DataIndex = iconIndex,
							IconCraftType = source.IconCraftType,
							Color = source.Color,
							IconRotation = source.IconRotation,
							IconTime = (i + 1)
						};

						curX += _moveOffsetX[buttonIndex] * stepX;
						curY += _moveOffsetY[buttonIndex] * stepY;
						elem.X = curX;
						elem.Y = curY;
						if (useAccel)
						{
							if (optIconAccelRaw.Checked)
							{
								stepX += accelX;
								stepY += accelY;
							}
							else if (optIconAccelPercent.Checked)
							{
								stepX += (int)(stepX * (accelX / 100.0f));
								stepY += (int)(stepY * (accelY / 100.0f));
							}

							// Positive acceleration to speed up, or negative to slow down.
							// If either step goes negative, it would flip movement direction and keep going.  Break so it stops.
							// If moving in a cardinal direction, one of the steps will be zero, so break when both are zero.
							if (stepX < 0 || stepY < 0 || (stepX == 0 && stepY == 0)) break;
						}
						_mapPreviewIcons.Add(elem);
						_lastPreviewStep = new Point(stepX, stepY);
					}
				}
			}

			refreshMap();
		}

		int getRegionPage(int eventUid)
		{
			int page = 0;
			foreach (var evt in _events)
			{
				if (evt.Event == AbstractEventType.XwaChangeRegion) page++;
				if (evt.UID == eventUid) break;
			}
			return page;
		}

		void detectIconMoveDistance()
		{
			if (!isXwa) return;

			// Auto detects the distance between two MoveIcon events.
			// If found, sets the numeric boxes that control the MoveIcon step amounts with the resulting X and Y distances.
			// If exactly two MoveIcon events are selected, use those two icons for context.
			// Otherwise if the last selected MoveIcon event is chosen, attempts to find a previous matching icon.

			// Count the number of selected Move events.
			int moveCount = 0;
			foreach (var so in _selectedObjects) if (isValidEvent(so) && _events[so.Index].Event == AbstractEventType.XwaMoveIcon) moveCount++;

			int distX = -1;
			int distY = -1;

			if (_selectedObjects.Count == 2 && moveCount == 2)
			{
				var one = _events[_selectedObjects[0].Index];
				var two = _events[_selectedObjects[1].Index];
				distX = Math.Abs(two.Params[1] - one.Params[1]);
				distY = Math.Abs(two.Params[2] - one.Params[2]);
			}
			else
			{
				int eventIndex = getWorkingMoveIcon();
				if (eventIndex >= 0)
				{
					// Using the last known MoveIcon selection.
					// Determine which region page it exists on.
					// Then search the event list for the most recent MoveIcon on the same page, before the selected one.
					var selEvt = _events[eventIndex];
					int selRegionPage = getRegionPage(selEvt.UID);

					int regionPage = 0;
					AbstractEvent foundEvt = null;
					foreach (var evt in _events)
					{
						if (evt.Event == AbstractEventType.XwaChangeRegion) regionPage++;
						if (evt.UID == selEvt.UID) break;

						if (evt.Event == AbstractEventType.XwaMoveIcon && regionPage == selRegionPage && evt.Params[0] == selEvt.Params[0]) foundEvt = evt;
					}

					if (foundEvt != null)
					{
						distX = Math.Abs(selEvt.Params[1] - foundEvt.Params[1]);
						distY = Math.Abs(selEvt.Params[2] - foundEvt.Params[2]);
					}
					else popupInfo("Could not find a prior MoveIcon step for the currently selected MoveIcon event.");
				}
				else popupInfo("You must select a MoveIcon event to calculate relative distance.");
			}

			if (distX > 0 || distY > 0)
			{
				// If either axis is zero, set both to match the nonzero axis.
				if (distX == 0) distX = distY;
				if (distY == 0) distY = distX;

				safeSetNumeric(numIconStepX, distX);
				safeSetNumeric(numIconStepY, distY);
			}
		}

		void clearIcons()
		{
			if (!isXwa) return;

			HashSet<short> selIndex = new HashSet<short>();
			foreach (var so in getSelectedEvents())
			{
				var evt = _events[so.Index];
				if (evt.Event == AbstractEventType.XwaSetIcon || evt.Event == AbstractEventType.XwaMoveIcon || evt.Event == AbstractEventType.XwaRotateIcon)
					selIndex.Add(evt.Params[0]);
			}

			if (selIndex.Count == 0)
			{
				popupError("Select an icon event to deactivate, but not delete, the icon at this time.");
				return;
			}

			foreach (short iconIndex in selIndex)
			{
				if (!hasAvailableSpace(AbstractEventType.XwaSetIcon))
				{
					popupError("Not enough available event space.");
					break;
				}

				AbstractEvent evt = new AbstractEvent((short)hsbTimer.Value, AbstractEventType.XwaSetIcon, iconIndex, 0, 0);
				int evtIndex = getInsertionIndexForEvent(hsbTimer.Value, false);
				insertEvent(evtIndex, evt, true);
			}
			relinkSelectedEventUid();
			rebuildShadowIcons();
			rebuildBriefing();
			refreshMap();
			refreshTimeline();
		}
		#endregion Xwa icon editing

		#region Rendering
		void refreshMap() => _pendingRedraw |= RefreshFlags.Map;
		void refreshTimeline() => _pendingRedraw |= RefreshFlags.Timeline;
		void refreshCanvas(RefreshFlags flag) => _pendingRedraw |= flag;

		// Compatibility with old code.
		public void MapPaint() => refreshMap();
		
		void checkRender()
		{
			// If not looking at the map tab, a redraw will effectively become queued and the map will be redrawn when the tab is changed.
			if (_pendingRedraw == RefreshFlags.None || tabBriefing.SelectedIndex != 0) return;

			if (_pendingRedraw.HasFlag(RefreshFlags.Title)) drawTitle();

			if (_pendingRedraw.HasFlag(RefreshFlags.Map)) drawMap();

			if (_pendingRedraw.HasFlag(RefreshFlags.Caption)) drawCaption();

			if (isXwing)
			{
				if (_pendingRedraw.HasFlag(RefreshFlags.Panel3)) drawXwingAuxPanel(_xwingPanel3Canvas, Platform.Xwing.PageTemplate.Elements.Panel3, AbstractEventType.PanelText3);
				if (_pendingRedraw.HasFlag(RefreshFlags.Panel4)) drawXwingAuxPanel(_xwingPanel4Canvas, Platform.Xwing.PageTemplate.Elements.Panel4, AbstractEventType.PanelText4);
			}

			// Anything in the map display needs to refresh the combined canvas.
			if (_pendingRedraw != RefreshFlags.Timeline) drawCombineCanvas();

			// Effects are rendered to the combined canvas in a final stage before blitting.
			if (_pendingRedraw.HasFlag(RefreshFlags.Effect)) drawEffects();

			if (_pendingRedraw != RefreshFlags.Timeline) blitMap();

			// The timeline renders directly to the pct, no need to blit.
			if (_pendingRedraw.HasFlag(RefreshFlags.Timeline)) drawTimeline();

			_pendingRedraw = RefreshFlags.None;
		}

		void drawMap()
		{
			using (Graphics g = Graphics.FromImage(_mapCanvas))
			{
				Color background = Color.Black;

				if (isXwa) background = Color.FromArgb(0x18, 0x18, 0x18);
				else if (isXvt) background = Color.FromArgb(0x00, 0x00, 0x18);
				else if (isXwing && getXwingCoreBriefing().MissionLocation) background = Color.FromArgb(0x2B, 0x30, 0x50);  // Death Star surface
				else if (isTie) background = Color.Transparent;    // For layering the overlapped Title/Caption overflows

				g.Clear(background);

				if (isXwa && hsbTimer.Value == _regionPageTime - 1 && (!isPlaybackActive || !chkXwaEffects.Checked))
				{
					var sz = g.MeasureString(_mapEffectString, _regionFont);
					float effectY = (_combinedCanvas.Height / 2) - (sz.Height / 2);
					float thisY = (_mapCanvas.Height / 2) - (sz.Height / 2);
					thisY += ((effectY - thisY) / 2) + 1;
					g.DrawString(_mapEffectString, _regionFont, Brushes.Yellow, (_mapCanvas.Width / 2) - (sz.Width / 2), thisY);
					return;
				}

				// The ship info is a special case because when active, it obscures the map.
				if (_activeShipInfoBlock)
				{
					if (_tempEvent == AbstractEventType.XwaInfoParagraph && optStateOn.Checked)
					{
						drawShipInfoText(g, cboEditString.SelectedIndex, 0, true, Color.SteelBlue);
						return;
					}
					else if (_activeShipInfoIcon >= 0 && _queuedMapEffect == MapEffect.None && _mapEffect == MapEffect.None)
					{
						// There is an active ship info sequence, but visual effects were disabled.
						// Print the ship info and we're done.
						Color bracket = Color.Black;
						var so = getPrimarySelectedObject();
						if (so != null && so.EventUID == _shipInfoStringUid) bracket = Color.White;
						drawShipInfoText(g, _shipInfoStringIndex, 0, true, bracket);
						return;
					}
				}

				drawMapGrid(g);
				drawMapElements(g);

				if (_tempEvent != AbstractEventType.None) drawTempEvents(g);
				
				if (_tempEvent == AbstractEventType.None && _selectedObjects.Count > 0) drawSelectedItems(g);

				// Draw selection box if the user is bounding-box selecting.
				if (_mouseDragState == MouseDragState.MapBoundClick)
				{
					var r = getNormalizedMouseRectangle(true);
					drawRectangle(g, r, -1, Color.White, false);
				}

				// Draw page number, which is usually incremented on a caption event.
				if (isXvt || (isXwa && (_mapEffect < MapEffect.ShipInfoA_IconGrow || _mapEffect > MapEffect.ShipInfoD_IconShrink)))
				{
					int y = (isXvt ? _mapCanvas.Height - 15 : -1);
					var c = getStandardColor(isXvt ? StandardColor.Default : StandardColor.Highlight);
					drawBriefingString(g, "Page # " + _currentPage, c, _font, _mapCanvas.Width - 60, y, -1, -1);
				}

				// NOTE: the XvT/XWA team visibility label, drawn in the upper left corner, is partially outside the map
				// panel.  It needs to overlap with the title pane, so it's rendered with the combined canvas instead.
				if (_freeLookMode || _mapShiftMode)
				{
					// Draw a border rectangle around the former view position so the user knows where they're looking.
					// Also include a notice and some positional stats in the top left corner.

					if (_mapShiftMode)
					{
						// Shift mode means we're editing a map Zoom or Move event.
						// Although technically it uses free look mode, the behavior is a bit different.
						// Don't draw the look position, only starting and ending rectangles.
						drawRectangle(g, convertRawRectToPixelRect(_mapShiftStart), -1, Color.SlateBlue, true);
						drawRectangle(g, convertRawRectToPixelRect(_mapShiftEnd), -1, Color.Magenta, true);
					}
					else drawRectangle(g, convertRawRectToPixelRect(_freeLookRect), -1, Color.SteelBlue, true);

					int xpos = 0, ypos = 0;
					if (isXwing)
					{
						xpos = _xwingSourceViewport[4].Left;
						ypos = _xwingSourceViewport[4].Top;
					}
					string label = (_mapShiftMode ? "Event Location" : "Free Look");
					g.DrawString(label, _statusFont, Brushes.White, xpos, ypos);

					if (!_mapShiftMode && chkFreeLookInfo.Checked)
					{
						ypos += _statusFont.Height + 1;
						if (_currentPosition != _backupPosition)
						{
							g.DrawString($"Position: {_currentPosition.X}, {_currentPosition.Y}", _statusFont, Brushes.White, xpos, ypos);
							ypos += _statusFont.Height + 1;
						}
						if (_currentZoom != _backupZoom) g.DrawString($"Zoom: {_currentZoom.X}, {_currentZoom.Y}", _statusFont, Brushes.White, xpos, ypos);
					}
				}
				
				if (_pathMode)
				{
					Point p1;
					g.DrawString("Path Mode", _statusFont, Brushes.White, 0, _mapCanvas.Height - _statusFont.Height - 2);
					for (int i = 0; i < _pathNodes.Count; i++)
					{
						var node = _pathNodes[i];
						PathNode next = null;
						if (i + 1 < _pathNodes.Count) next = _pathNodes[i + 1];

						p1 = convertRawPosToPixelPos(node.X, node.Y);

						if (next != null)
						{
							var p2 = convertRawPosToPixelPos(next.X, next.Y);
							g.DrawLine(Pens.White, p1, p2);
						}

						Rectangle r = new Rectangle(p1.X - 5, p1.Y - 5, 10, 10);
						node.InteractRect = r;
						g.FillRectangle(Brushes.Black, r);
						g.DrawRectangle(Pens.White, r);
						g.DrawString("#" + (i + 1), _statusFont, Brushes.White, p1.X + 6, p1.Y + 6);

						var sel = getSelectedItem(SelectedType.PathNode, i);
						if (sel != null)
						{
							sel.InteractRect = r;

							r.Offset(2, 2);
							r.Width -= 3;
							r.Height -= 3;
							g.FillRectangle(Brushes.White, r);
						}
					}

					// Draw a persistent bracket around the current working icon.  Selection state doesn't matter.
					int eventIndex = getWorkingMoveIcon();
					if (eventIndex >= 0)
					{
						var moveEvent = _events[eventIndex];
						p1 = convertRawPosToPixelPos(moveEvent.Params[1], moveEvent.Params[2]);
						Rectangle r = new Rectangle(p1.X - 8, p1.Y - 8, 16, 16);
						drawRectangle(g, r, 4, Color.MediumOrchid, false);
						r.Inflate(1, 1);
						drawRectangle(g, r, 5, Color.MediumOrchid, false);
					}
				}
				else if (isXwingPanelEditIndexSelected(4))
				{
					var r = getXwingSelectionRect(Platform.Xwing.PageTemplate.Elements.Map, _mapCanvas);
					drawPanelSelectionBracket(g, r);
				}
			}
		}

		/// <summary>Renders various objects to the map, typically icons and text tags.</summary>
		void drawMapElements(Graphics g)
		{
			// Needed for the icons to draw correctly.
			g.InterpolationMode = InterpolationMode.NearestNeighbor;

			// Draw the flightgroup tags first.
			foreach (var elem in _mapFgTags) drawFgTag(g, elem);

			// All non-XWING platforms draw text tags after the flightgroup tags, but before the icon itself.
			if (!isXwing)
			{
				for (int i = 0; i < _mapTextTags.Length; i++)
				{
					var elem = _mapTextTags[i];
					drawTextTag(g, elem);
				}
			}

			// Seems to improve the stretching slightly.
			if (isXwing && isXwingHighDef()) g.PixelOffsetMode = PixelOffsetMode.Half;

			// Draw the icons, overlapping with any tags that were drawn in the background.
			for (int i = 0; i < _flightgroups.Count; i++)
			{
				var afg = _flightgroups[i];

				int wp = getFlightgroupWaypoint();
				afg.DisplayWaypoint = wp;

				BaseFlightGroup.Waypoint waypoint = afg.Waypoints[wp];
				if (waypoint.Enabled || (!waypoint.Enabled && chkIconShadows.Checked))
				{
					afg.DisplayPos = new Point(waypoint.RawX, getInvertedY(waypoint.RawY));
					var pos = convertRawPosToPixelPos(afg.DisplayPos);

					Color color = Color.Gray;
					if (waypoint.Enabled)
					{
						if (isXwing) color = getXwingIconColor(afg.CraftIff, afg.CraftType);
						else color = getIconColor(afg.CraftIff);
					}
					
					int craftType = getAdjustedCraftType(afg.CraftType, afg.CraftIff);
					var iconData = getCraftIconData(craftType);
					var icon = getCraftIcon(craftType, color, 0);

					Rectangle rect = new Rectangle(pos.X - (_craftIconBracketSize / 2), pos.Y - (_craftIconBracketSize / 2 ), _craftIconBracketSize, _craftIconBracketSize);

					if (isXwing && isXwingHighDef())
					{
						// The drawing function here doesn't stretch properly to the original, so the icon pixels won't be
						// exact.  However the general outline of the icon is mostly accurate with the necessary corrections.
						// There are some exceptions, noticable at the bottom edge of the screen.  See end of max103a.
						// If this code changes, fix the corresponding temp event drawing.
						Rectangle iconRect = new Rectangle(pos.X - icon.Width, (pos.Y - icon.Height) - 2, (icon.Width * 2), (icon.Height * 2) + 4);
						if ((iconData.Height & 1) != 0)
						{
							iconRect.Y++;
							iconRect.Height--;
						}
						g.DrawImage(icon, iconRect);
						rect.Inflate(_craftIconBracketSize / 2, _craftIconBracketSize / 2);
					}
					else
					{
						pos.Offset(iconData.DrawOffsetX, iconData.DrawOffsetY);
						g.DrawImage(icon, pos);

						if (isXvt && chkTeamPlayerLabels.Checked && afg.PlayerNumber > 0)
						{
							// Determine if the FG is a playable team by testing its briefing visibility.  The primary team will
							// display the correct label color, while any alternate playable teams will have a diminished color.
							bool match = (afg.Team >= 0 && afg.Team <= 9 && _briefingList[_currentBriefingIndex].ViewedByTeam[afg.Team]);
							if (afg.Team == _currentTeamIndex || match)
							{
								Color c = (afg.Team == _currentTeamIndex ? getStandardColor(StandardColor.Default) : Color.SteelBlue);
								pos.Offset(icon.Width, icon.Height);
								drawBriefingString(g, afg.PlayerNumber.ToString(), c, _font, pos.X, pos.Y, 0, 0);
							}
						}
					}
					afg.InteractRect = rect;
				}
			}

			g.PixelOffsetMode = PixelOffsetMode.Default;

			// XWING text tags overlap flightgroup tags and their icons.
			if (isXwing)
			{
				for (int i = 0; i < _mapTextTags.Length; i++)
				{
					var elem = _mapTextTags[i];
					drawTextTag(g, elem);
				}
			}

			if (chkIconShadows.Checked) foreach (var elem in _mapShadowIcons) drawXwaIcon(g, elem, true);
			if (_mapShipInfoTag.Enabled) drawFgTag(g, _mapShipInfoTag);

			foreach (var elem in _mapIcons)
			{
				// Normally draw the icons, but we need a special case for the Rotate temp event.  It will overlap
				// with the real icon and look bad.  So in that case, avoid drawing the real icon.
				if (_tempEvent != AbstractEventType.XwaRotateIcon || cboIcon.SelectedIndex != elem.DataIndex) drawXwaIcon(g, elem, false);
			}

			foreach (var elem in _mapPreviewIcons) drawXwaIcon(g, elem, true);
		}

		void drawTitle()
		{
			using (Graphics g = Graphics.FromImage(_titleCanvas))
			{
				if (isXvt || isXwa) g.Clear(Color.FromArgb(0x18, 0x18, 0x18));
				else if (isTie)
				{
					// Most of the canvas is overflow space.  Technically this overflows down into the caption
					// too.  When in campaign mode, the blue background of the panel overwrites it (except for
					// one overflow line at the very bottom).  It's visible in the combat chamber.
					// Overflow pixels will be visible when the combined canvas is updated.
					g.Clear(Color.Black);
					Brush brush = new SolidBrush(getStandardColor(StandardColor.Panel));
					g.FillRectangle(brush, new Rectangle(0, 0, _titleCanvas.Width, 12));
					brush.Dispose();
				}
				else g.Clear(getStandardColor(StandardColor.Panel));

				string title = _titleString;
				bool custom = false;
				bool selected = false;

				if (_tempEvent == AbstractEventType.TitleText && isTitlePlatform)
				{
					title = txtEditString.Text;
					selected = true;
				}
				else
				{
					if (isTitlePlatform && _panelStringEnabled[PANEL_TITLE])
					{
						int eventIndex = getEventIndexByUid(_panelStringEventUid[PANEL_TITLE]);
						var so = getPrimarySelectedObject();
						if (so != null && so.Type == SelectedType.Event && so.Index == eventIndex) selected = true;

						int index = _panelStringIndex[PANEL_TITLE];
						if (index >= 0 && index < _strings.Count) title = _strings[index];
					}

					if (_titleOverrideString != "")
					{
						title = _titleOverrideString;
						custom = true;
					}
				}

				if (title != "")
				{
					// The draw function will take care of centering as long as the string is prefixed with '>'
					int xpos = 0, ypos = 0;
					int width = _titleCanvas.Width;
					int height = _titleCanvas.Height;
					if (isXwing)
					{
						xpos = isXwingHighDef() ? 10 : 4;
						ypos = isXwingHighDef() ? 6 : 3;
						int scale = isXwingHighDef() ? 2 : 1;
						var item = getXwingPanel(Platform.Xwing.PageTemplate.Elements.Title);
						width = (item.Right - item.Left) * scale;
						height = (item.Bottom - item.Top) * scale;
						if (isXwingHighDef())
						{
							width -= 6;
							if (height < 40) height = 40;
						}
					}
					else if (isTie)
					{
						xpos = 2;
						ypos = 2;
						width += 2;  // See comparable note in drawCaption()
					}
					else if (isXvt) ypos = -1;
					else if (isXwa) ypos = -3;

					Color c = Color.White;
					if (isXvt) c = (custom ? Color.Orchid : getStandardColor(StandardColor.Center));
					else if (isXwa) c = (custom ? Color.Yellow : getTextTagCaret(3));  // Highest shade of blue
					else if (custom) c = Color.Orchid;

					// In game, XvT and XWA both draw their titles separately from the map, so it's not actually in the title space, nor is it centered with the map.
					// They also use a larger font than the one used for tags and captions.
					var titleFont = (isXvt || isXwa ? _fontAltCaption : _font);

					// We can't use the centering control code for custom titles, because the yellow color would override this.
					// Instead calculate the center position manually.
					if (custom || isXvt || isXwa)
					{
						if (title[0] == '>') title = title.Substring(1);
						int textWidth = 0;
						for (int i = 0; i < title.Length; i++) textWidth += titleFont.GetChar(title[i]).Width + titleFont.HorizontalSpacing;
						xpos = (_titleCanvas.Width / 2) - (textWidth / 2);
						width = _titleCanvas.Width;
					}
					drawBriefingString(g, title, c, titleFont, xpos, ypos + titleFont.VerticalOffset, width, height);
				}

				bool xwingSelected = isXwingPanelEditIndexSelected(PANEL_TITLE);
				if (selected || xwingSelected)
				{
					Rectangle r = new Rectangle(0, 0, _titleCanvas.Width - 1, _titleCanvas.Height - 1);

					// The XWING canvas is maximum size to accommodate arbitrary sizes.
					// Only draw the source that was copied, which conforms to the viewport size.
					// In TIE, the title is larger for the sake of overflow text, but we don't want that height here.
					if (isXwing) r = getXwingSelectionRect(Platform.Xwing.PageTemplate.Elements.Title, _titleCanvas);
					else if (isTie) r.Height = 11;
					drawPanelSelectionBracket(g, r);
				}
			}
		}

		void drawCaption()
		{
			using (Graphics g = Graphics.FromImage(_captionCanvas))
			{
				g.Clear(getStandardColor(StandardColor.Panel));

				string text = "";
				bool selected = false;

				if (_tempEvent == AbstractEventType.CaptionText)
				{
					text = txtEditString.Text;
					selected = true;
				}
				else if (_panelStringEnabled[PANEL_CAPTION])
				{
					int eventIndex = getEventIndexByUid(_panelStringEventUid[PANEL_CAPTION]);
					var so = getPrimarySelectedObject();
					if (so != null && so.Type == SelectedType.Event && so.Index == eventIndex) selected = true;

					int index = _panelStringIndex[PANEL_CAPTION];
					if (index >= 0 && index < _strings.Count) text = _strings[index];
				}

				int xpos = 0, ypos = 0;
				int width = _captionCanvas.Width;
				int height = _captionCanvas.Height;
				if (isXwing)
				{
					var item = getXwingPanel(Platform.Xwing.PageTemplate.Elements.Caption);
					int scale = isXwingHighDef() ? 2 : 1;
					width = (item.Right - item.Left) * scale;

					if (isXwingHighDef())
					{
						width -= 6;
						if (getXwingPanel(Platform.Xwing.PageTemplate.Elements.Map).IsVisible)
						{
							// NOTE: If xpos changes, modify the string drawing function.  It has a special condition
							// needed to get centering via ">" to work properly, and it checks for this value.
							xpos = 75;
							ypos = 6;
						}
						else
						{
							xpos = 10;
							ypos = 6;
						}
					}
					else
					{
						xpos = 4;
						ypos = 3;
					}
				}
				else if (isTie)
				{
					xpos = 2;
					ypos = 2;
					// Need to adjust width too, because wordwrapping wasn't working correctly.
					// See B8M1 "Three TIE Defenders..." where in-game the word "to" rightmost pixel (not shadow) is exactly at the right
					// edge of the map.  The shadow seems to overdraw into the frame.
					width += 2;
				}
				else if (isXvt)
				{
					// In game it tries to center vertically in each row (14 pixels per row).
					// Can't set the vertical offset because it that would affect text tags too.
					// However it does use the vertical spacing override when the font loads.
					ypos = -1;
				}

				if (text != "") drawBriefingString(g, text, getStandardColor(StandardColor.Default), _font, xpos, ypos + _font.VerticalOffset, width, height);

				bool xwingSelected = isXwingPanelEditIndexSelected(PANEL_CAPTION);
				if (selected || xwingSelected)
				{
					Rectangle r = new Rectangle(0, 0, _captionCanvas.Width - 1, _captionCanvas.Height - 1);
					if (isXwing) r = getXwingSelectionRect(Platform.Xwing.PageTemplate.Elements.Caption, _titleCanvas);
					else if (isTie) r.Height--;
					drawPanelSelectionBracket(g, r);
				}
			}
		}

		void drawXwingAuxPanel(Bitmap canvas, Platform.Xwing.PageTemplate.Elements panel, AbstractEventType eventType)
		{
			var item = getXwingPanel(panel);
			if (canvas == null || !item.IsVisible) return;

			int panelIndex = (panel == Platform.Xwing.PageTemplate.Elements.Panel3 ? PANEL_XWING3 : PANEL_XWING4);
			int stringIndex = _panelStringIndex[panelIndex];
			int lastEventUid = _panelStringEventUid[panelIndex];

			using (Graphics g = Graphics.FromImage(canvas))
			{
				g.Clear(getStandardColor(StandardColor.Panel));

				string text = "";
				bool selected = false;

				if (_tempEvent == eventType)
				{
					text = txtEditString.Text;
					selected = true;
				}
				else if (_panelStringEnabled[panelIndex] && stringIndex >= 0 && stringIndex < _strings.Count)
				{
					text = _strings[stringIndex];
					int eventIndex = getEventIndexByUid(lastEventUid);
					var so = getPrimarySelectedObject();
					if (so != null && so.Type == SelectedType.Event && so.Index == eventIndex) selected = true;
				}
				
				int scale = isXwingHighDef() ? 2 : 1;
				int width = (item.Right - item.Left) * scale;
				int height = canvas.Height;

				// If the panel extends to the bottom, allow the full canvas height so that it can use the extra
				// space in high def mode.  Panel4 does not use the extra height.
				if (item.Bottom < 138 || panel == Platform.Xwing.PageTemplate.Elements.Panel4) height = (item.Bottom - item.Top) * scale;

				int xpos = 4;
				int ypos = 3;
				if (isXwingHighDef())
				{
					xpos = 10;
					ypos = 6;
					width -= 6;
				}
				if (text != "") drawBriefingString(g, text, getStandardColor(StandardColor.Default), _font, xpos, ypos + _font.VerticalOffset, width, height);

				bool xwingSelected = isXwingPanelEditIndexSelected(panelIndex);
				if (selected || xwingSelected)
				{
					Rectangle r = getXwingSelectionRect(panel, canvas);
					drawPanelSelectionBracket(g, r);
				}
			}
		}

		/// <summary>Draws a selection bracket at the specified rectangle.  Intended for panels only, because XWING has extra considerations when in panel mode.</summary>
		void drawPanelSelectionBracket(Graphics g, Rectangle r)
		{
			bool xwingPanel = (isXwing && _panelMode);
			Color c = (xwingPanel ? Color.Magenta : Color.White);
			drawRectangle(g, r, 5, c);

			if (!xwingPanel) return;

			// Edges (4 to 7): Top, Bottom, Left, Right
			if (_panelModeEdge == 4) g.DrawLine(Pens.Magenta, r.Left, r.Top, r.Right, r.Top);
			else if (_panelModeEdge == 5) g.DrawLine(Pens.Magenta, r.Left, r.Bottom, r.Right, r.Bottom);
			else if (_panelModeEdge == 6) g.DrawLine(Pens.Magenta, r.Left, r.Top, r.Left, r.Bottom);
			else if (_panelModeEdge == 7) g.DrawLine(Pens.Magenta, r.Right, r.Top, r.Right, r.Bottom);
			else if (_panelModeEdge == 9 && r.Width >= 10 && r.Height >= 10)
			{
				// Draw a cross in the center to indicate move mode is operational.
				int x = r.Left + (r.Width / 2);
				int y = r.Top + (r.Height / 2);
				g.DrawLine(Pens.Magenta, x - 5, y, x + 5, y);
				g.DrawLine(Pens.Magenta, x, y - 5, x, y + 5);
			}
		}

		/// <summary>Renders a string into a graphic context with the specified color, font, and position.  The string may contain control codes or highlight codes.</summary>
		/// <param name="g">Graphics context of the canvas to render into.</param>
		/// <param name="s">String to render.  Control codes and highlighting are allowed.</param>
		/// <param name="defaultColor">The default color to use if no highlighting is applied.</param>
		/// <param name="font">Briefing font to use.</param>
		/// <param name="x">Left starting position for each line.</param>
		/// <param name="y">Top starting position of the first line.</param>
		/// <param name="width">If positive, indicates that wordwrapping is enabled and this is the maximum pixel width a line may hold.  Zero or negative indicates no wordwrapping.</param>
		/// <param name="height">If positive, indicates the maximum pixel height of a wordwrapped viewport.  Wordwrapped rendering will break if out of bounds.  Zero or negative indicates no effect.</param>
		/// <param name="shipInfo">Specifies this is part of an XWA ShipInfo custom paragraph.  Required to properly override some highlighting rules.</param>
		/// <returns>The Point of the ending draw position.</returns>
		private Point drawBriefingString(Graphics g, string s, Color defaultColor, BriefingFont font, int x, int y, int width, int height, bool shipInfo = false)
		{
			// Unescape the string.  Necessary for sequential escaped characters to be interpreted properly.
			char[] str = getUnescapedString(s, out int strLength);

			int curX = x;
			int curY = y;
			int opos = 0;
			bool isHighlight = false;
			string word, block;
			int wordWidth, blockWidth;
			Color curColor = defaultColor;
			bool highlightEnabled = (width > 0 && !shipInfo) || (width <= 0 && !isXwing);

			// Determines the upper bound (inclusive) of what is considered a color code or highlight character.
			// We're only concerned with 0-9 since they can be escaped.  The value here doesn't necessarily mean
			// the codes are valid colors, just that they shouldn't be rendered as glyphs.
			int startingGlyph = 0x20;
			if (isXvt) startingGlyph = 8;
			else if (isXwa) startingGlyph = 7;

			// Scan through the string, building a block sequence of text to render.  Keeps track of the width metrics as it builds the string.
			// The outer loop keeps track of the starting point for each block.
			// The inner loop scans for words and terminators, and appending words to the block when appropriate.
			while (opos < strLength)
			{
				// Begin a new block.
				word = "";
				block = "";
				wordWidth = 0;
				blockWidth = 0;

				// Scan for words, beginning at the current outer position.
				int ipos = opos;
				while (ipos < strLength)
				{
					char c = str[ipos++];
					char next = (ipos < strLength ? str[ipos] : '\0');

					// For tags (no wrapping), a single null terminator ends the string.
					// For wrapped strings, single null terminator is interpreted as a newline.  Dual null terminators will end the string.
					if ((c == 0 && width <= 0) || (c == 0 && next == 0)) ipos = strLength;

					int charWidth = font.GetChar(c).Width + font.HorizontalSpacing;

					// If we reach a newline, or the end of the string, check the word we've built so far.
					if (c == '$' || (c == 0 && width > 0) || ipos == strLength)
					{
						// Text tags don't have line breaks, so they'll render the actual char.
						// Since it's not a break, continue parsing the current word.
						if (c == '$' && width <= 0)
						{
							word += c;
							wordWidth += charWidth;
							continue;
						}

						if (ipos == strLength && c != 0 && c != '$' && c != ' ')
						{
							// End of the string, include the final character.
							// NOTE: Trailing space is excluded too because it messes up formatting.  See title in TIE B3M4.
							word += c;
							wordWidth += charWidth;
						}

						// If the word fits into the available space, append the word to the outer block.
						// If the word doesn't fit, and we're not at the end of the string, we'll resume on the next outer iteration.
						if (width <= 0 || curX + blockWidth + wordWidth < width)
						{
							block += word;
							blockWidth += wordWidth;
							opos = ipos;
						}
						else if (blockWidth == 0)
						{
							// Special case, the entire word can't fit on the line.  Only draw the first character.
							// NOTE: may cause the game to freeze, especially if it happens twice.
							block = word.Substring(0, 1);
							opos++;
						}

						// Break out to render.
						break;
					}
					else
					{
						// Not a newline, and not the end of the string.
						// This will be text, or some other control code.
						if (c == ' ')
						{
							if (width <= 0)
							{
								// No wordwrapping, add the space.
								word += c;
								wordWidth += charWidth;
							}
							else
							{
								int total = curX + blockWidth + wordWidth;
								if (total < width)
								{
									// Enough space to fit the existing word.
									block += word;
									blockWidth += wordWidth;
									word = "";
									wordWidth = 0;

									// Add the space character too if possible.
									if (total + charWidth < width)
									{
										block += c;
										blockWidth += charWidth;
										opos = ipos;
									}
									else
									{
										// Render what we have.  Ignore the space character when starting the next word, so that it's
										// properly left-aligned inside the panel.
										opos = ipos;
										break;
									}
								}
								else
								{
									// Nothing fits.
									if (blockWidth == 0 && word.Length > 1)
									{
										// Special case, the entire word can't fit on the line.  Only draw the first character.
										// NOTE: may cause the game to freeze, especially if it happens twice.
										block = word.Substring(0, 1);
										opos++;
									}

									break;
								}
							}
						}
						else
						{
							// Not a space.  Any text or color control char.
							// Color codes and highlight codes do not count to word length.
							word += c;

							// If the centering effect is enabled, the prefix char itself won't contribute to word width.
							if (c == '>' && highlightEnabled && width >= 0 && word.Length == 1 && block == "") continue;

							if (c >= startingGlyph && (c != '[' && c != ']')) wordWidth += charWidth;
						}
					}
				}

				if (ipos == strLength)
				{
					// Ensure that the outer loop terminates, to avoid an infinite loop.
					// This can be triggered if the word is a single "$" in non-wordwrap (text tag) context.
					if (word != "" && block == "")
					{
						block = word;
						opos = ipos;
					}
				}

				// If there's something in the block, render it.
				if (block != "")
				{
					// The ShipInfo custom color codes must remain persistent through wordwraps.
					if (!shipInfo) curColor = defaultColor;

					// Bracket highlighting persists over newlines, so retain if necessary.
					if (isHighlight) curColor = getStandardColor(StandardColor.Highlight);

					bool isCentered = false;
					int rpos = 0;

					// An opening angle bracket indicates that the entire block should be centered horizontally on the line.
					// Generally this only happens if word wrapping is enabled.  For tags, the character will be rendered.
					if (block[0] == '>' && highlightEnabled && width >= 0)
					{
						isCentered = true;
						curColor = getStandardColor(StandardColor.Center);
						rpos = 1;
						curX = (width / 2) - (blockWidth / 2);

						// NOTE: There's something wrong with the vertical alignment, seems to be 1-2 pixels off.  Only seems to
						// happen when a line is centered.  Sometimes the non-centered lines are in the expected position, sometimes not.
						// This applies to XW94 and XW98.  Other platforms seem to be affected too.
						// Horizontal centering also seems to be off by 1-2 pixels.
						if (isXwing && x == 75) curX += 39;
					}

					// Iterate through each character in the block and render it.
					while (rpos < block.Length)
					{
						char c = block[rpos++];

						if ((c == '$' || c == 0) && width > 0)
						{
							// Single null is interpreted as a newline.
							if (c == 0 && isCentered) curColor = defaultColor;
							curX = x;
							curY += font.Height + font.VerticalSpacing;
							continue;
						}

						// Check for color codes embedded directly in the string.  Also check bracket highlights.
						if (c == 1)
						{
							// Retain center color (yellow) instead of default white.
							if (isCentered) curColor = getStandardColor(StandardColor.Center);
							else curColor = defaultColor;
							continue;
						}
						else if (c >= 2 && c <= 9)
						{
							// XWING and TIE ignore 3 or higher, and will continue using whatever color is currently assigned.
							// These higher codes are not rendered.
							if ((isXwing || isTie) && c >= 3) continue;

							// If it's a glyph, it will fall through to rendering further down.
							if (c < startingGlyph)
							{
								// Using \2 for highlight does not persist across newlines, but the proper bracket highlight '[' will.
								if (isHighlight) curColor = getInlineTextColor(c, getStandardColor(StandardColor.Highlight));
								else curColor = getInlineTextColor(c, defaultColor);
								continue;
							}
						}
						else if (c == '[' && highlightEnabled)
						{
							curColor = getStandardColor(StandardColor.Highlight);
							isHighlight = true;
							continue;
						}
						else if (c == ']' && highlightEnabled)
						{
							curColor = defaultColor;
							isHighlight = false;
							if (isCentered) curColor = getStandardColor(StandardColor.Center);
							continue;
						}

						// If we get here, it's not a control code, but visible text.  Draw the character.
						var glyph = font.GetChar(c);
						var bmp = getCachedTintedBitmap(font.CacheType, c, glyph.Image, curColor, 0);
						g.DrawImage(bmp, curX, curY);
						curX += glyph.Width + font.HorizontalSpacing;
					}
				}

				// Finished rendering the block, if anything was in it.  Now apply a newline.
				// This happens even if the block is empty, which could be triggered by a lone newline char.
				// Except if at the end of the string, so it can return the ending draw position.
				if (opos != strLength)
				{
					curX = x;
					curY += font.Height + font.VerticalSpacing;
				}

				// If we wordwrapped out of vertical bounds, there's nothing left to do.
				if (height > 0 && curY >= height) break;

				// Failsafe in case there's some kind of infinite loop by a fringe condition that hasn't been accounted for.
				// This can also be triggered if using free look to zoom in far enough to push a text tag off the bottom of the screen.
				if (curY > _combinedCanvas.Height) break;
			}
			return new Point(curX, curY);
		}

		void drawXwingPanels(Graphics g)
		{
			// XWING is complicated because there are multiple customizable panels.  To make things worse,
			// the rendering logic is different between XW94 and XW98, and not just the scaling.

			// Typically everything is drawn in order down the list of panels, and can be observed by
			// giving them overlapping bounds.  Text panels first, then finally the map.  The panel
			// coordinates are relative to the briefing canvas.  The DOS versions have a total rendering
			// size of 212x138.

			// In XW94, it's simple.  Draw everything in order.

			// In XW98, the rendering logic changes depending on whether the map is visible.  In text
			// mode, the text panels are drawn in order as expected.  But in map mode, the Title
			// (Panel #1), Map (#5), and Caption (#2) are stacked vertically in that order, regardless
			// of what the top and bottom edges of the panels are set to.  The title also has a
			// minimum height enforced.

			// The panel coordinates are scaled up by 2x.  Yet the 2x scaled area isn't 424x276.
			// Instead it's 420x330.  The width is close enough that the discrepancy seems to be ignored.
			// (Except in some cases with the map, which causes complications and adjustments elsewhere.)
			// The extra height introduces some edge cases.  Certain panels will be extended to the bottom
			// edge.  In text mode, the extension will apply to panels #1 through #3, but never #4.
			// In map mode, the Caption (panel #2) is always extended if visible.  Never Panel #3 or #4.

			// Even with the vertical stacking and bottom extension logic, the text panels are still
			// drawn in order and will obscure the previous panels if the bounds overlap.

			// In order to accommodate XW98's stacking and resizing logic when the map is visible, it's not
			// as simple as drawing everything in order.  The panel metrics need to be determined first.

			var core = getXwingCoreBriefing();
			int pageType = core.Pages[_currentBriefingIndex].PageType;
			var panelConfig = core.Templates[pageType].Items;

			int scale = isXwingHighDef() ? 2 : 1;
			Bitmap[] canvasArray = new Bitmap[] { _titleCanvas, _captionCanvas, _xwingPanel3Canvas, _xwingPanel4Canvas, _mapCanvas };
			Rectangle[] dest = new Rectangle[5];

			for (int i = 0; i < 5; i++) dest[i] = getXwingDestinationRect(panelConfig[i], scale);

			// Adjust metrics and alignment for XW98 if applicable.
			if (isXwingHighDef())
			{
				if (panelConfig[4].IsVisible)
				{
					// Map mode.  Enforce minimum height of title.
					if (dest[0].Height < 40) dest[0].Height = 40;

					// Align map to bottom of title, then caption to bottom of map.
					// The map has a single row of padding above and below.
					dest[4].Y = dest[0].Y + dest[0].Height + 1;
					dest[1].Y = dest[4].Y + dest[4].Height + 1;

					// Extend caption to bottom.
					dest[1].Height = _combinedCanvas.Height - dest[1].Y;
				}
				else
				{
					// Text only.  Find the last visible panel and extend it.
					int last = -1;
					for (int i = 0; i < 4; i++) if (panelConfig[i].IsVisible) last = i;

					// If Panel #4 is visible, don't extend anything.
					if (last >= 0 && last != 3) dest[last].Height = _combinedCanvas.Height - dest[last].Y;
				}
			}

			// Finished calculating the panel metrics.  Now draw them.
			float mapScale = getMapScale();
			for (int i = 0; i < 5; i++)
			{
				Rectangle clickViewport = new Rectangle();
				_xwingSourceViewport[i] = new Rectangle();

				if (panelConfig[i].IsVisible)
				{
					Rectangle src = new Rectangle(0, 0, dest[i].Width, dest[i].Height);

					// While we're here, determine the click viewport so that the user can select the title or caption by clicking the appropriate area.
					clickViewport = new Rectangle((int)(dest[i].Left * mapScale), (int)(dest[i].Top * mapScale), (int)(dest[i].Width * mapScale), (int)(dest[i].Height * mapScale));

					// Text panels are rendered relative to (0, 0) so that's where they need to copy from.
					// The map is not.  Without this, the map is incorrectly shifted.
					if (i == 4)
					{
						src = dest[i];
						// Center based on size.
						src.Location = getXwingMapSourceLocation(dest[4]);
					}

					g.DrawImage(canvasArray[i], dest[i], src, GraphicsUnit.Pixel);

					// For the panel selection brackets, bring in the edges so they're inside the visible rectangle.
					src.Width--;
					src.Height--;
					_xwingSourceViewport[i] = src;
				}

				// For panel mode, it disrupts behavior if the width is off the control.
				if (clickViewport.X + clickViewport.Width > pctBrief.Width)
					clickViewport.Width -= (clickViewport.X + clickViewport.Width) - pctBrief.Width;

				// Assignment is annoying because these are structs.  It's either this way, or modify the rest of the program.
				if (i == 0) _scaledTitleRect = clickViewport;
				else if (i == 1) _scaledCaptionRect = clickViewport;
				else if (i == 2) _scaledPanel3Rect = clickViewport;
				else if (i == 3) _scaledPanel4Rect = clickViewport;
				else if (i == 4) _scaledMapRect = clickViewport;
			}
		}

		void drawTeamLabelString(Graphics g)
		{
			// Multiple teams may be selected.  Find the first one.
			int team = -1;
			for (int i = 0; i < lstTeams.Items.Count; i++)
			{
				if (lstTeams.GetSelected(i))
				{
					team = i;
					break;
				}
			}

			string s = "Briefing: " + (team >= 0 ? "\\4" + _teamNames[team] : "\\3No Team!");
			drawBriefingString(g, s, Color.White, _fontAltCaption, 2, _titleCanvas.Height - 4, 0, 0);
		}

		void drawCombineCanvas()
		{
			using (Graphics g = Graphics.FromImage(_combinedCanvas))
			{
				if (isXwing)
				{
					g.Clear(Color.Black);
					drawXwingPanels(g);
				}
				else if (isTie)
				{
					// Special case to allow the Title and Caption overflow pixels to be visible.
					// Transparency allows the drawn pixels to overlap properly in sequence.
					g.Clear(Color.Black);
					g.DrawImage(_titleCanvas, 0, 0);
					g.DrawImage(_mapCanvas, 0, 12);
					g.DrawImage(_captionCanvas, 0, 12 + _mapCanvas.Height);
				}
				else
				{
					g.DrawImage(_titleCanvas, 0, 0);
					int y = _titleCanvas.Height;
					g.DrawImage(_mapCanvas, 0, y);
					y += _mapCanvas.Height;
					g.DrawImage(_captionCanvas, 0, y);

					// XvT and XWA have a label in the top left indicating which team the briefing is for.
					// It's rendered one pixel above the map, overlapping both the title and map panels.
					// During effects, it will render the string in its own context instead.
					if (isTeamLabelVisible()) drawTeamLabelString(g);
				}
			}
		}

		void drawTimeline()
		{
			_timelineHighestItemCount = 0;
			_timelineElements.Clear();

			if (!chkTimelinePlayback.Checked && isPlaybackActive)
			{
				using (Graphics g = Graphics.FromImage(pctTimeline.Image))
				{
					g.Clear(Color.Black);
				}
				pctTimeline.Invalidate();
				return;
			}

			// It's possible for events to exist beyond the assigned duration.
			int duration = _briefingDuration;
			int lastEventTime = getHighestTime();
			using (Graphics g = Graphics.FromImage(pctTimeline.Image))
			{
				g.Clear(Color.Black);

				// The current time is always rendered in the center.
				int centerX = pctTimeline.Width / 2;
				int halfColumns = centerX / _timelineColumnWidth;
				int height = pctTimeline.Height;
				int curTime = hsbTimer.Value;
				int startTime = hsbTimer.Value - halfColumns;
				int endTime = hsbTimer.Value + halfColumns;
				int lastTime = Math.Max(duration, lastEventTime);
				int startX = centerX - (halfColumns * _timelineColumnWidth);
				int lineHeight = _timelineFont.Height + 1;
				int boxSize = lineHeight - 4;
				int halfBoxSize = boxSize / 2;

				// The header pip occupies a line.  The remaining space determines how many lines are visible.
				// Because the items are "centered" on each line, the header size is larger than it visibly appears.
				int headerHeight = (lineHeight / 2) + lineHeight;
				_timelineVisibleRowCount = (pctTimeline.Height - (headerHeight - halfBoxSize)) / lineHeight;
				_timelineHighestItemCount = 0;

				// Draw the vertical column lines first.  If the column width is small, drawing over the text looks bad.
				for (int time = startTime, curX = startX;   time < endTime;   time++, curX += _timelineColumnWidth)
				{
					if (time < 0) continue;
					if (time > lastTime) break;

					// While we're going through all the events, find the tallest visible column.
					// This is necessary for scrolling and offset adjustment when drawing the event list.
					int eventCount = countEventsAtTime(time);
					if (eventCount > _timelineHighestItemCount) _timelineHighestItemCount = eventCount;

					if (time == 0)
					{
						g.DrawLine(Pens.Red, curX - 1, 0, curX - 1, height);
						g.DrawLine(Pens.Red, curX + 1, 0, curX + 1, height);
					}
					else if (time == duration)
					{
						g.DrawLine(Pens.Blue, curX - 1, 0, curX - 1, height);
						g.DrawLine(Pens.Blue, curX + 1, 0, curX + 1, height);
					}
					else if (time == curTime)
					{
						g.DrawLine(Pens.Gray, curX - 1, 0, curX - 1, height);
						g.DrawLine(Pens.Gray, curX + 1, 0, curX + 1, height);
					}
					else if (time > duration) g.DrawLine(Pens.Purple, curX, 0, curX, height);
					else g.DrawLine(Pens.DimGray, curX, 0, curX, height);
				}

				// If all columns fit in the available space, reset the vertical scroll position back to top.
				if (_timelineHighestItemCount <= _timelineVisibleRowCount) _timelineRowScrollPos = 0;

				// Now fill in the events at each column.
				for (int time = startTime, curX = startX;   time < endTime;   time++, curX += _timelineColumnWidth)
				{
					var evtList = enumEventsAtTime(time);
					if (evtList.Count == 0) continue;

					int selCount = 0;
					int firstSelected = -1;
					int lastSelected = -1;
					for (int i = 0; i < evtList.Count; i++)
					{
						if (hasSelectedItem(SelectedType.Event, evtList[i]))
						{
							selCount++;
							lastSelected = i;
							if (firstSelected == -1) firstSelected = i;
						}
					}

					int curY = lineHeight / 2;

					// Create a header pip, which allows quick selection of everything in the list.
					// It looks nicer if the vertical time bars are filled over first.  Then draw the outline.
					Rectangle pipRect = new Rectangle(curX - halfBoxSize, curY - halfBoxSize, boxSize, boxSize);
					g.FillEllipse(Brushes.Black, pipRect);
					g.DrawEllipse(Pens.White, pipRect);

					Brush fillColor = null;
					if (selCount == evtList.Count) fillColor = Brushes.White;
					else if (selCount > 0) fillColor = Brushes.Gray;

					if (fillColor != null) g.FillEllipse(fillColor, curX - halfBoxSize + 1, curY - halfBoxSize + 1, boxSize - 2, boxSize - 2);

					// Starting position for drawing the events.
					curY = headerHeight;

					MapElement pipElem = new MapElement
					{
						EventIndex = -1,
						DataIndex = time,
						InteractRect = pipRect
					};
					_timelineElements.Add(pipElem);
					
					// Draw events and their selection pip, one item per line.

					// How many items to be drawn, limited by available vertical space for rows.
					int itemCount = evtList.Count;
					int startRow = _timelineRowScrollPos;
					if (startRow + _timelineVisibleRowCount > itemCount) startRow = itemCount - _timelineVisibleRowCount;

					if (startRow < 0) startRow = 0;

					int lastItem = itemCount - 1;
					int endRow = startRow + (_timelineVisibleRowCount - 1);
					if (endRow > lastItem) endRow = lastItem;

					for (int item = startRow; item <= endRow; item++)
					{
						if (curY + (lineHeight - halfBoxSize) > height) break;

						// If there are more items offscreen, either above or below, draw a background rect to signal there are more items in that direction.
						bool offscreenItems = ((item == startRow && startRow > 0) || (item == endRow && endRow < lastItem));
						if (offscreenItems)
						{
							Rectangle bg = new Rectangle(curX, curY - halfBoxSize, _timelineColumnWidth - halfBoxSize, boxSize);
							g.FillRectangle(Brushes.Purple, bg);
						}

						int eventIndex = evtList[item];
						var evt = _events[eventIndex];
						// Moving the boxes further to the left so the overall item is a bit more centered on the line.
						Rectangle r = new Rectangle(curX - boxSize, curY - halfBoxSize, boxSize, boxSize);
						g.DrawRectangle(Pens.White, r);
						if (hasSelectedItem(SelectedType.Event, evtList[item]))
						{
							// Modifying the outer rect was making the interact size too small.
							Rectangle r2 = r;
							r2.Offset(2, 2);
							r2.Width -= 3;
							r2.Height -= 3;
							g.FillRectangle(Brushes.White, r2);
						}

						var def = EventDef.GetEventDefByType(evt.Event);
						string s = ((def != null) ? def.Abbrev : "Unk:" + (int)evt.Event);
						switch (def.Type)
						{
							// Indices for string, region, and icon are all in param[0] for these types.
							case AbstractEventType.CaptionText:
							case AbstractEventType.TitleText:
							case AbstractEventType.XwaChangeRegion:
							case AbstractEventType.XwaSetIcon:
							case AbstractEventType.XwaMoveIcon:
							case AbstractEventType.XwaRotateIcon:
								s += "." + (evt.Params[0] + 1);
								break;

							case AbstractEventType.XwaShipInfo:
								s += ":" + (evt.Params[0] != 0 ? "ON" : "OFF");
								break;
						}
						g.DrawString(s, _timelineFont, Brushes.White, curX + (halfBoxSize / 2) + 1, curY - (halfBoxSize + 2));

						var sz = g.MeasureString(s, _timelineFont);
						r.Width += (int)sz.Width;
						// The rect that passes through might be smaller than it initially was, but it may not matter.
						MapElement evtElem = new MapElement
						{
							EventIndex = eventIndex,
							EventUid = evt.UID,
							DataIndex = time,
							InteractRect = r
						};
						_timelineElements.Add(evtElem);

						// Draw arrows to indicate there are more items above and below.
						// The position corresponds to the background rectangle that was initially filled.
						if (offscreenItems)
						{
							int right = curX + (_timelineColumnWidth - halfBoxSize);
							int top = curY - halfBoxSize;
							int bottom = top + boxSize;
							int offset = lineHeight / 4;
							if (offset < 3) offset = 3;
							g.DrawLine(Pens.White, right, top, right, bottom);

							if (item == startRow)
							{
								g.DrawLine(Pens.White, right - offset, top + offset, right, top);
								g.DrawLine(Pens.White, right + offset, top + offset, right, top);
							}
							else
							{
								g.DrawLine(Pens.White, right - offset, bottom - offset, right, bottom);
								g.DrawLine(Pens.White, right + offset, bottom - offset, right, bottom);
							}
						}

						// If there's a selected item above or below, draw broader arrows to point where they are.
						if (firstSelected >= 0 && firstSelected < startRow)
						{
							int x = curX + (_timelineColumnWidth / 2);
							int width = _timelineColumnWidth / 3;
							g.DrawLine(Pens.Yellow, x - width, 1 + halfBoxSize, x, 1);
							g.DrawLine(Pens.Yellow, x + width, 1 + halfBoxSize, x, 1);
						}
						if (lastSelected >= 0 && lastSelected > endRow)
						{
							int x = curX + (_timelineColumnWidth / 2);
							int width = _timelineColumnWidth / 3;
							int bottom = pctTimeline.Height - 2;
							g.DrawLine(Pens.Yellow, x - width, bottom - halfBoxSize, x, bottom);
							g.DrawLine(Pens.Yellow, x + width, bottom - halfBoxSize, x, bottom);
						}
						curY += lineHeight;
					}
				}

				bool leftArrow = false;
				bool rightArrow = false;
				foreach (var so in _selectedObjects)
				{
					if (!isValidEvent(so)) continue;

					if (_events[so.Index].Time < startTime) leftArrow = true;
					if (_events[so.Index].Time >= endTime) rightArrow = true;
				}

				if (leftArrow || rightArrow)
				{
					int y = pctTimeline.Height / 2;
					int aheight = _timelineColumnWidth / 2;
					if (leftArrow)
					{
						g.DrawLine(Pens.Yellow, 1 + halfBoxSize, y - aheight, 1, y);
						g.DrawLine(Pens.Yellow, 1 + halfBoxSize, y + aheight, 1, y);
					}
					if (rightArrow)
					{
						int x = pctTimeline.Width - 1;
						g.DrawLine(Pens.Yellow, x - halfBoxSize, y - aheight, x, y);
						g.DrawLine(Pens.Yellow, x - halfBoxSize, y + aheight, x, y);
					}
				}

				if (_mouseDragState == MouseDragState.TimelineBoundDrag)
				{
					var r = getNormalizedMouseRectangle(false);
					drawRectangle(g, r, 0, Color.White);
				}
			}
			pctTimeline.Invalidate();

			int maximum = Math.Max(0, _timelineHighestItemCount - _timelineVisibleRowCount);
			vsbTimeline.Maximum = maximum;
			// If user changes the timeline height, the current position can be out of range.
			safeSetScrollbar(vsbTimeline, _timelineRowScrollPos);
			// If LargeChange is greater than the difference (Maximum - Value) you can't scroll to the end.
			// If changed dynamically to avoid that, it changes the physical size of the slider bar.
			// So leaving it at 1.
			vsbTimeline.LargeChange = 1;
			vsbTimeline.Enabled = (_timelineHighestItemCount > _timelineVisibleRowCount);
			vsbTimeline.Invalidate();  // Force UI refresh since changing Enabled won't automatically do it
		}

		/// <summary>Draws XWA effects on the combined canvas, based on the currently assigned effect and its frame time.</summary>
		void drawEffects()
		{
			if (_mapEffect == MapEffect.None) return;

			using (Graphics g = Graphics.FromImage(_combinedCanvas))
			{
				float ratio = 0.0f;
				if (_mapEffectMaxFrame != 0) ratio = (float)_mapEffectCurrentFrame / _mapEffectMaxFrame;

				if (ratio < 0.0f) ratio = 0.0f;
				else if (ratio > 1.0f) ratio = 1.0f;

				switch (_mapEffect)
				{
					case MapEffect.Initializing_MapWipeIn: drawEffectMapFade(g, 1.0f - ratio); break;
					case MapEffect.RegionA_MapWipeOut: drawEffectMapFade(g, ratio); break;
					case MapEffect.RegionB_TextFadeIn: drawEffectRegionText(g, ratio); break;
					case MapEffect.RegionC_TextFadeOut: drawEffectRegionText(g, 1.0f - ratio); break;
					case MapEffect.RegionD_MapWipeIn: drawEffectMapFade(g, 1.0f - ratio); break;
					case MapEffect.ShipInfoA_IconGrow: drawEffectIconGrow(g, ratio); break;
					case MapEffect.ShipInfoB_ScreenSweep: drawEffectCraftSweep(g, ratio); break;
					case MapEffect.ShipInfoC_MeshRotate: drawEffectMeshRotate(g); break;
					case MapEffect.ShipInfoD_IconShrink: drawEffectIconGrow(g, 1.0f - ratio); break;
				}

				// Normally when an effect is running, the combined canvas isn't updated.
				// But region change effects must still draw the string, even if the map is obscured.
				if (isTeamLabelVisible()) drawTeamLabelString(g);
			}
		}
		void drawEffectMapFade(Graphics g, float ratio)
		{
			// Ratio (0.0 to 1.0) determines how much of the map is obscured.
			// Wipes from the bottom to the top of the map area.  Does not affect the caption or title.
			int height = (int)(_mapCanvas.Height * ratio);

			int bottom = _combinedCanvas.Height - _captionCanvas.Height;
			int top = bottom - height;
			g.FillRectangle(Brushes.Black, new Rectangle(0, top, _combinedCanvas.Width, bottom - top));

			// Draw randomized lines from the bottom of the caption up to the hidden portion.
			Pen[] pens = new Pen[4] {
				new Pen(Color.FromArgb(50, 48, 76)),
				new Pen(Color.FromArgb(75, 72, 112)),
				new Pen(Color.FromArgb(100, 96, 148)),
				new Pen(Color.FromArgb(125, 120, 186)),
			};

			// Horizontal line indicating where the map is being hidden.
			g.DrawLine(pens[3], 0, top, _combinedCanvas.Width, top);

			// Randomized lines from the bottom center up to the horizontal line.
			Random r = new Random(Environment.TickCount);
			for (int i = 0; i < 10; i++)
			{
				int x = (int)(_combinedCanvas.Width * r.NextDouble());
				int rp = r.Next() % 4;
				g.DrawLine(pens[rp], x, top + 1, _combinedCanvas.Width / 2, _combinedCanvas.Height - 1);
			}

			foreach (Pen p in pens) p.Dispose();
		}
		void drawEffectRegionText(Graphics g, float ratio)
		{
			// Clear the entire map area.
			int top = _titleCanvas.Height;
			g.FillRectangle(Brushes.Black, new Rectangle(0, top, _mapCanvas.Width, _mapCanvas.Height));

			// Text color intensity based on frame time.
			int intensity = (int)(255.0f * ratio);
			
			// Yellow text in the center of the map.
			Brush color = new SolidBrush(Color.FromArgb(intensity, intensity, 0));
			var sz = g.MeasureString(_mapEffectString, _regionFont);
			g.DrawString(_mapEffectString, _regionFont, color, (_combinedCanvas.Width / 2) - (sz.Width / 2), (_combinedCanvas.Height / 2) - (sz.Height / 2));
			color.Dispose();
		}
		void drawEffectScaledIcon(Graphics g, Rectangle viewport, MapElement icon)
		{
			if (viewport.Width > viewport.Height)
			{
				viewport.X += (viewport.Width - viewport.Height) / 2;
				viewport.Width = viewport.Height;
			}

			var color = getIconColor(icon.Color);
			var bmp = getCraftIcon(icon.IconCraftType - 1, color, icon.IconRotation);
			if (bmp.Width > bmp.Height)
			{
				float scale = (float)bmp.Height / bmp.Width;
				viewport.Y += (int)(viewport.Height * (1.0 - scale)) / 2;
				viewport.Height = (int)(viewport.Height * scale);
			}
			else if (bmp.Height > bmp.Width)
			{
				float scale = (float)bmp.Width / bmp.Height;
				viewport.X += (int)(viewport.Width * (1.0 - scale)) / 2;
				viewport.Width = (int)(viewport.Width * scale);
			}

			var oldMode = g.InterpolationMode;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.DrawImage(bmp, viewport);
			g.InterpolationMode = oldMode;
		}
		void drawEffectIconGrow(Graphics g, float ratio)
		{
			int rectWidth = (int)(_mapCanvas.Width * ratio);
			int rectHeight = (int)(_mapCanvas.Height * ratio);
			
			int endx = _mapCanvas.Width / 2;
			int endy = _titleCanvas.Height + (_mapCanvas.Height / 2);
			int x = endx;
			int y = endy;

			MapElement elem = null;
			if (_activeShipInfoIcon >= 0 && _activeShipInfoIcon < _mapIcons.Length)
			{
				elem = _mapIcons[_activeShipInfoIcon];
				Point start = convertRawPosToPixelPos(elem.X, elem.Y);
				x = (start.X + (int)((endx - start.X) * ratio)) - (rectWidth / 2);
				y = (start.Y + (int)((endy - start.Y) * ratio)) - (rectHeight / 2);
			}

			Rectangle rect = new Rectangle(x, y, rectWidth, rectHeight);
			Pen outline = new Pen(Color.FromArgb(0x96, 0, 0));
			g.DrawRectangle(outline, rect);
			rect.Offset(1, 1);
			rect.Inflate(-1, -1);
			g.FillRectangle(Brushes.Black, rect);

			// Since we don't have fancy 3D rendering, why not use the 2D icon instead?
			if (elem != null) drawEffectScaledIcon(g, rect, elem);
			outline.Dispose();
		}
		void drawEffectCraftSweep(Graphics g, float ratio)
		{
			int mapTop = _titleCanvas.Height;

			// Fill the map area since there's no actual model to render.
			Rectangle rect = new Rectangle(0, mapTop, _mapCanvas.Width, _mapCanvas.Height);
			g.FillRectangle(Brushes.Black, rect);

			if (_activeShipInfoIcon >= 0 && _activeShipInfoIcon < _mapIcons.Length) drawEffectScaledIcon(g, rect, _mapIcons[_activeShipInfoIcon]);
			drawShipInfoText(g, _shipInfoStringIndex, mapTop, false, Color.Black);

			// Calculate where the sweep edges are.
			int x = (int)(_mapCanvas.Width * ratio);

			// Y grows at the same rate, but clamp it.  The bottom edge will be hit before the right edge.
			int maxY = (_combinedCanvas.Height - _captionCanvas.Height) - 1;
			int y = x + mapTop;
			if (y > maxY) y = maxY;

			Pen[] pens = new Pen[] {
				new Pen(Color.FromArgb(50, 48, 76)),
				new Pen(Color.FromArgb(75, 72, 112)),
				new Pen(Color.FromArgb(100, 96, 148)),
				new Pen(Color.FromArgb(125, 120, 186)),
			};

			// Draw the sweep lines on the X and Y axes.
			g.DrawLine(pens[3], 0, y, _combinedCanvas.Width, y);        // Horizontal
			g.DrawLine(pens[3], x, mapTop, x, _combinedCanvas.Height);  // Vertical

			Random r = new Random(Environment.TickCount);

			// Draw randomized lines from the bottom of the caption up to vertical line.
			for (int i = 0; i < 5; i++)
			{
				int dy = mapTop + (int)(_mapCanvas.Height * r.NextDouble());
				int rp = r.Next() % 4;
				g.DrawLine(pens[rp], x, dy, _combinedCanvas.Width / 2, _combinedCanvas.Height - 1);
			}

			// Draw randomized lines from the bottom of the caption up to horizontal line.
			// The sweep from top to bottom finishes before the sweep from left to right.
			if (y < maxY)
			{
				for (int i = 0; i < 10; i++)
				{
					int dx = (int)(_mapCanvas.Width * r.NextDouble());
					int rp = r.Next() % 4;
					g.DrawLine(pens[rp], dx, y, _combinedCanvas.Width / 2, _combinedCanvas.Height - 1);
				}
			}

			foreach (Pen p in pens) p.Dispose();
		}

		/// <summary>Draws the craft type for the XWA ShipInfo display text.  Includes custom paragraph if the string index is specified.</summary>
		/// <param name="g">Graphics context to render into.  May be either the map canvas or combined canvas.</param>
		/// <param name="stringIndex">The index of the caption string to use for the custom paragraph, or -1 for none.</param>
		/// <param name="top">The Y position of the top edge of the canvas to use as the display area.</param>
		/// <param name="center">Determines whether the text should be centered.  Only applies when no custom paragraph.</param>
		/// <param name="bracket">If not Color.Black, will draw a selection bracket around the text area with this color.</param>
		void drawShipInfoText(Graphics g, int stringIndex, int top, bool center, Color bracket)
		{
			// In game the craft stats are in the top left corner.
			// If the paragraph is enabled, draw that in the upper left corner.
			// If not enabled, just draw the ship name in the center.
			if (stringIndex >= 0 && stringIndex < _strings.Count)
			{
				// Can't use the highlight color because it's blue.  Needs to be green, so using the first text tag color.
				drawBriefingString(g, _mapEffectString, _textTagColors[0], _font, 0, top, 0, 0);
				drawBriefingString(g, "Special Characteristics:", getStandardColor(StandardColor.Center), _font, 0, top + 24, 0, 0);
				drawBriefingString(g, _strings[stringIndex], _textTagColors[0], _font, 20, top + 36, 191, _mapCanvas.Height, true);

				// The lower bracket will show the approximate limit before the craft stats are pushed off the bottom of the screen.
				if (bracket != Color.Black) drawRectangle(g, new Rectangle(17, top + 36, 174, 156), 5, bracket);
			}
			else
			{
				SizeF sz = g.MeasureString(_mapEffectString, _regionFont);
				PointF pos = new PointF(0, top);
				if (center)
				{
					pos.X += (_mapCanvas.Width / 2) - (sz.Width / 2);
					pos.Y += (_mapCanvas.Height / 2) - (sz.Height / 2);
				}
				g.DrawString(_mapEffectString, _regionFont, Brushes.Yellow, pos);
			}
		}

		void drawEffectMeshRotate(Graphics g)
		{
			int y = _combinedCanvas.Height - (_mapCanvas.Height + _captionCanvas.Height);
			Rectangle rect = new Rectangle(0, y, _mapCanvas.Width, _mapCanvas.Height);
			g.FillRectangle(Brushes.Black, rect);
			if (_activeShipInfoIcon >= 0 && _activeShipInfoIcon < _mapIcons.Length) drawEffectScaledIcon(g, rect, _mapIcons[_activeShipInfoIcon]);
			drawShipInfoText(g, _shipInfoStringIndex, y, false, Color.Black);
		}

		/// <summary>Updates the briefing map PictureBox with the most recent canvas rendering.  The output will be stretched to fill the entire control.</summary>
		/// <remarks>NOTE: Rendering has changed, <see cref="pctBrief_Paint"/></remarks>
		void blitMap() => pctBrief.Invalidate();

		private void drawMapGrid(Graphics g)
		{
			if (_currentZoom.X < 1 || _currentZoom.Y < 1) return;

			Pen light, dark;
			if (isXwing)
			{
				if (!getXwingCoreBriefing().MissionLocation)
				{
					light = new Pen(Color.FromArgb(0x10, 0x75, 0));
					dark = new Pen(Color.FromArgb(0x10, 0x30, 0));
				}
				else
				{
					// Death Star surface, from XW94.
					light = new Pen(Color.FromArgb(0xBE, 0xB6, 0xB6));
					dark = new Pen(Color.FromArgb(0x51, 0x55, 0x65));
				}
			}
			else if (isTie)
			{
				light = new Pen(Color.FromArgb(0x79, 0, 0));
				dark = new Pen(Color.FromArgb(0x49, 0, 0));
			}
			else
			{
				light = new Pen(Color.FromArgb(0x96, 0, 0));
				dark = new Pen(Color.FromArgb(0x50, 0, 0));
			}

			// XWING and TIE have borders surrounding the map panel.
			if (isXwing)
			{
				var map = getXwingPanel(Platform.Xwing.PageTemplate.Elements.Map);
				var rect = getXwingDestinationRect(map, isXwingHighDef() ? 2 : 1);
				rect.Location = getXwingMapSourceLocation(rect);
				rect.Width--;
				rect.Height--;
				g.DrawRectangle(dark, rect);
			}
			else if (isTie) g.DrawRectangle(dark, 0, 0, _mapCanvas.Width - 1, _mapCanvas.Height - 1);

			// The current map viewing position will be centered over the canvas.
			// Therefore the starting position of everything will need to be properly shifted to the top left.
			int halfWidth = _mapCanvas.Width / 2;
			int halfHeight = _mapCanvas.Height / 2;

			int scaleX = _currentZoom.X;
			int scaleY = _currentZoom.Y;
			if (isXwing && isXwingHighDef())
			{
				scaleX *= 2;
				scaleY *= 2;
			}

			// This determines how many grid cells we need to offset to get to the top left.
			int gridX = halfWidth / scaleX;
			int gridY = halfHeight / _currentZoom.Y;

			// Every 256 raw units represents a full grid cell (1 mile, not 1 kilometer) spacing.
			// The line number determines whether a primary (bright) or subdivision (dark) line is drawn.
			int lineX = (_currentPosition.X / 256) - gridX;
			int lineY = (_currentPosition.Y / 256) - gridY;
	
			// If the position is a multiple of 256, a line number correction is needed.
			if (_currentPosition.X > 0 && ((_currentPosition.X & 255) != 0)) lineX++;
			if (_currentPosition.Y > 0 && ((_currentPosition.Y & 255) != 0)) lineY++;

			// From center, offset the line drawing position to the top left based on grid size.
			int posX = halfWidth - (gridX * scaleX);
			int posY = halfHeight - (gridY * scaleY);
		
			// Adjust for the position between cells.
			// This is based off the original algorithm to rely on integer math so that the result is pixel accurate.
			posX += (scaleX * (-_currentPosition.X & 255)) / 256;
			posY += (scaleY * (-_currentPosition.Y & 255)) / 256;

			// Final correction in case the adjustment pushed wider than a full cell, which would result in missing line.
			if (posX >= scaleX)
			{
				posX -= scaleX;
				lineX--;
			}
			if (posY >= scaleY)
			{
				posY -= scaleY;
				lineY--;
			}

			// The original rendering code only considers the X axis for subdivisions.
			bool drawHalfSubdivisions = (scaleX >= 16);
			bool drawQuarterSubdivisions = (scaleX >= 32);

			int startPosX = posX;
			int startPosY = posY;
			int startLineX = lineX;
			int startLineY = lineY;

			// Have to draw the light and dark lines separately, or else they overlap incorrectly.
			// Start with the dark lines.
			while (posX < _mapCanvas.Width)
			{
				int v = (lineX & 3);
				if (drawHalfSubdivisions && (v == 2 || drawQuarterSubdivisions)) g.DrawLine(dark, posX, 0, posX, _mapCanvas.Height);
				posX += scaleX;
				lineX++;
			}
			while (posY < _mapCanvas.Height)
			{
				int v = (lineY & 3);
				if (drawHalfSubdivisions && (v == 2 || drawQuarterSubdivisions)) g.DrawLine(dark, 0, posY, _mapCanvas.Width, posY);
				posY += scaleY;
				lineY++;
			}

			// Restore starting positions and draw the light lines.
			posX = startPosX;
			posY = startPosY;
			lineX = startLineX;
			lineY = startLineY;
			while (posX < _mapCanvas.Width)
			{
				int v = (lineX & 3);
				if (v == 0) g.DrawLine(light, posX, 0, posX, _mapCanvas.Height);
				posX += scaleX;
				lineX++;
			}
			while (posY < _mapCanvas.Height)
			{
				int v = (lineY & 3);
				if (v == 0) g.DrawLine(light, 0, posY, _mapCanvas.Width, posY);
				posY += scaleY;
				lineY++;
			}

			light.Dispose();
			dark.Dispose();
		}

		void drawFgTag(Graphics g, MapElement element)
		{
			if (!element.Enabled) return;

			Point pos;
			int craftType;
			int craftIff;
			int craftRotation = 0;
			bool waypointEnabled = true;
			if (isXwa)
			{
				int iconIndex = element.DataIndex;
				if (iconIndex < 0 || iconIndex >= _mapIcons.Length) return;

				var icon = _mapIcons[iconIndex];
				pos = convertRawPosToPixelPos(icon.X, getInvertedY(icon.Y));
				craftType = icon.IconCraftType - 1;
				craftIff = icon.Color;
				craftRotation = icon.IconRotation;

				// Special case for XWA activating a tag on a null or invalid craft.
				if (craftType == -1 || !isXwaValidIcon(icon.IconCraftType - 1))
				{
					craftType = 0;
					waypointEnabled = false;
				}
			}
			else
			{
				// Invalid tags may be drawn on the map in game, but this is undefined behavior.
				// In XvT and XWING, it commonly results in a tag at position (0, 0), but not TIE.
				if (element.DataIndex < 0 || element.DataIndex >= _flightgroups.Count) return;

				var afg = _flightgroups[element.DataIndex];
				var waypoint = afg.Waypoints[getFlightgroupWaypoint()];

				waypointEnabled = waypoint.Enabled;
				pos = convertRawPosToPixelPos(waypoint.RawX, getInvertedY(waypoint.RawY));
				craftType = getAdjustedCraftType(afg.CraftType, afg.CraftIff);
				craftIff = afg.CraftIff;
			}
			if (craftType < 0 || craftType >= _craftIconImages.Count) return;

			CraftIconImage iconData = getCraftIconData(craftType);

			// If the icons are rotated, invert the offsets so they remain centered.
			int offsetX = iconData.GetDrawOffsetX();
			int offsetY = iconData.GetDrawOffsetY();
			int selWidth = iconData.GetWidth();
			int selHeight = iconData.GetHeight();
			if (craftRotation == 1 || craftRotation == 3)
			{
				int temp = offsetX;
				offsetX = offsetY;
				offsetY = temp;

				temp = selWidth;
				selWidth = selHeight;
				selHeight = temp;
			}

			element.InteractRect = new Rectangle(pos.X + offsetX, pos.Y + offsetY, selWidth, selHeight);

			int elapsedTime = element.ElapsedTime;
			if (!chkAnimateTags.Checked) elapsedTime = 12;

			if (isXwing)
			{
				Pen outer = null;
				Brush inner = null;

				bool hiDef = isXwingHighDef();
				int boxSize = 13;
				if (hiDef) boxSize = 20;

				Rectangle boxRect = new Rectangle(pos.X - boxSize / 2, pos.Y - boxSize / 2, boxSize, boxSize);
				
				if (hiDef) boxRect.Y--;
				else
				{
					// Made for high-def first, then this didn't look right.  Still seems off, but close enough.
					boxRect.Width--;
					boxRect.Height--;
				}

				if (elapsedTime <= 10)
				{
					// Animate a grayscale rectangle shrinking down to the icon.
					// It's easier to imagine the process in reverse.
					// Consider the last frame will have a single dark gray rectangle:
					//   Each additional frame will add another simultaneous rectangle, with greater starting intensity.
					//   Maximum of 4 simultaneous rectangles, and maximum of 4 intensities.
					//   Once maximum starting intensity is achieved, the rectangles get increasingly further from center.
					// When drawing multiple rectangles, the outermost rectangles are lighter in color.

					// Smallest size, and how large it grows based on elapsed time.
					int offset = (hiDef ? 10 : 6);
					if (elapsedTime <= 7)  offset += ((7 - elapsedTime) * 2);
					
					int count = (10 - elapsedTime) + 1;
					if (count > 4) count = 4;
					
					int colorIndex = count - 1;

					if (elapsedTime == 0) count = 2;
					else if (elapsedTime == 1) count = 3;

					// Colors from DOS screenshot.
					Color[] colors = new Color[4] {
						Color.FromArgb(0x28, 0x2C, 0x49),
						Color.FromArgb(0x75, 0x75, 0x75),
						Color.FromArgb(0xB6, 0xAE, 0xAE),
						Color.FromArgb(0xF7, 0xEF, 0xEF)
					};

					Pen p = new Pen(colors[colorIndex]);
					for (int i = 0; i < count; i++)
					{
						Rectangle r = new Rectangle(pos.X - offset, pos.Y - offset, offset * 2, offset * 2);
						g.DrawRectangle(p, r);

						offset += 2;

						if (colorIndex > 0) colorIndex--;
						p.Color = colors[colorIndex];
					}
					p.Dispose();
				}
				// Time = 11 doesn't have anything drawn for that frame.
				else if (elapsedTime >= 12)
				{
					if (isXwingHighDef()) boxRect.Y++;

					int frameNum = (elapsedTime - 12) + 1;
					bool border = (frameNum >= 7) || ((frameNum & 1) != 0);
					inner = new SolidBrush(Color.FromArgb(0x49, 0, 0));
					if (border) outer = new Pen(Color.FromArgb(0xC3, 0x10, 0));
				}
				
				if (inner != null)
				{
					// Apparently the fill function doesn't have the same resulting dimensions as the rectangle for the outer border.
					// Need adjustment so they're the same size, for the frames that the border isn't visible.
					Rectangle solidRect = new Rectangle(boxRect.X, boxRect.Y, boxRect.Width + 1, boxRect.Height + 1);
					g.FillRectangle(inner, solidRect);
					inner.Dispose();
				}
				if (outer != null)
				{
					g.DrawRectangle(outer, boxRect);
					outer.Dispose();
				}
			}
			else
			{
				// Any platform that isn't XWING.
				// Display icon convergence animation, or a border and filled rectangle when the animation finishes.
				// If the icon is deactivated or (null in XWA's case) then:
				//   The animation plays for TIE and XvT, but not XWA.
				//   The final border is visible for XvT and XWA, but not TIE.

				Color[] palette = getPaletteFromIff(craftIff);
				
				if (elapsedTime >= (isTie ? 11 : 12))
				{
					// Primary animation has finished.  Draw border and background where the icon will be.
					Pen outer = new Pen(palette[6]);
					Brush inner = new SolidBrush(palette[2]);

					// Defaults for TIE Fighter, which uses fixed size square.
					// XvT and XWA make further adjustments.
					int iconWidth = 12;
					int iconHeight = 12;
					if (isTie && elapsedTime == 11)
					{
						// The very first frame has a smaller box.
						iconWidth = 8;
						iconHeight = 8;
					}
					else if (isXvt)
					{
						iconWidth = iconData.Width;
						iconHeight = iconData.Height;
					}
					else if (isXwa)
					{
						// Add 1 to compensate for bounds being too small, with 2 pixel clearance on both sides (1+2+2)
						iconWidth = iconData.GetWidth();
						iconHeight = iconData.GetHeight();
						if (craftRotation == 1 || craftRotation == 3)
						{
							int temp = iconWidth;
							iconWidth = iconHeight;
							iconHeight = temp;
						}

						// If the icon is null, it displays as a very small rectangle.
						if (!waypointEnabled)
						{
							iconWidth = 4;
							iconHeight = 4;
						}
					}

					int dx = pos.X - (iconWidth / 2);
					int dy = pos.Y - (iconHeight / 2);

					// For XvT and XWA, the box is adjusted, but after the initial position is assigned.
					if (isXvt || isXwa)
					{
						dx -= 2;
						dy -= 2;
						iconWidth += 3;
						iconHeight += 3;
					}

					// Outer border
					Rectangle rect = new Rectangle(dx, dy, iconWidth, iconHeight);
					g.DrawRectangle(outer, rect);

					// Inner fill
					rect = new Rectangle(dx + 1, dy + 1, iconWidth - 1, iconHeight - 1);
					g.FillRectangle(inner, rect);

					// There's exactly one frame where a medium dark icon is shown here.
					// The frame after has just the rectangle, and then nothing.
					if (isTie && !waypointEnabled && elapsedTime == 11)
					{
						var icon = getFlatCraftIcon(craftType, palette[3], 0);
						g.DrawImage(icon, pos.X + offsetX, pos.Y + offsetY);
					}

					inner.Dispose();
					outer.Dispose();
				}
				else
				{
					// Draw animated convergence of icons.
					if (!waypointEnabled && isXwa) return;

					int drawOffset;
					int drawSteps;
					if (elapsedTime >= 4)
					{
						drawOffset = 2 * (11 - elapsedTime);
						drawSteps = (elapsedTime >= 8 ? 12 - elapsedTime : 4);
					}
					else
					{
						drawSteps = elapsedTime + 1;
						drawOffset = 16;
					}

					if (elapsedTime >= 8 && !isTie)
					{
						// Draws an animated growing box underneath.
						int iconWidth = iconData.Width;
						int iconHeight = iconData.Height;
						if (craftRotation == 1 || craftRotation == 3)
						{
							int temp = iconWidth;
							iconWidth = iconHeight;
							iconHeight = temp;
						}

						iconWidth -= (11 - elapsedTime) * 2;
						iconHeight -= (11 - elapsedTime) * 2;
						int bx = pos.X - (iconWidth / 2) - 1;
						int by = pos.Y - (iconHeight / 2) - 1;

						Rectangle rect = new Rectangle(bx, by, iconWidth, iconHeight);

						Pen outer = new Pen(palette[elapsedTime - 8]);
						Brush inner = new SolidBrush(palette[2]);
						g.DrawRectangle(outer, rect);
						g.FillRectangle(inner, rect);
						outer.Dispose();
						inner.Dispose();
					}

					int colorStep = 2;
					int drawColor = 7 - (2 * elapsedTime);
					if (drawColor < 1) drawColor = 1;

					while (drawSteps > 0)
					{
						Bitmap icon = getFlatCraftIcon(craftType, palette[drawColor], craftRotation);
						g.DrawImage(icon, pos.X - drawOffset + offsetX, pos.Y - drawOffset + offsetY);
						g.DrawImage(icon, pos.X + drawOffset + offsetX, pos.Y - drawOffset + offsetY);
						g.DrawImage(icon, pos.X - drawOffset + offsetX, pos.Y + drawOffset + offsetY);
						g.DrawImage(icon, pos.X + drawOffset + offsetX, pos.Y + drawOffset + offsetY);

						drawOffset -= 2;
						drawColor += colorStep;
						drawSteps--;
					}
				}
			}
		}

		/// <summary>Draws all visual elements of a temp event, including selection boxes when applicable.</summary>
		void drawTempEvents(Graphics g)
		{
			// Set interact to null.  Events with movable positions will reassign their interact location.
			_tempInteract.Width = 0;

			if (_tempEvent == AbstractEventType.TextTag1)
			{
				string s = txtEditString.Text;

				// If it's empty, it's hard to see on the map.  This makes the bracket larger.
				if (s == "") s = "  ";

				var pos = convertRawPosToPixelPos((int)numX.Value, getInvertedY((int)numY.Value));
				pos.Y += _tagFont.VerticalOffset;
				var color = getTextTagColor(cboTextTagColor.SelectedIndex);
				var endPos = drawBriefingString(g, s, color, _tagFont, pos.X, pos.Y, -1, -1);
				int textWidth = Math.Max(endPos.X - pos.X, _tagFont.MaxWidth);
				Rectangle rect = new Rectangle(pos.X, pos.Y, textWidth, _tagFont.MaxHeight);
				rect.Inflate(2, 2);
				drawRectangle(g, rect, 4, Color.SteelBlue);
				_tempInteract = rect;
			}
			else if (_tempEvent == AbstractEventType.FgTag1)
			{
				int index = cboFGTagItem.SelectedIndex;
				Rectangle r = new Rectangle();
				if (isXwa)
				{
					if (index >= 0 && index < _mapIcons.Length && _mapIcons[index].Enabled) r = _mapIcons[index].InteractRect;
				}
				else
				{
					if (index >= 0 && index < _flightgroups.Count)
					{
						var afg = _flightgroups[index];
						r = afg.InteractRect;

						// Tags can technically activate on disabled waypoints, but there may not be an interact assigned
						// if it was never drawn.  Display where it would be on the map.
						if (!chkIconShadows.Checked || !afg.Waypoints[getFlightgroupWaypoint()].Enabled)
						{
							var wp = afg.Waypoints[getFlightgroupWaypoint()];
							var p = convertRawPosToPixelPos(wp.RawX, getInvertedY(wp.RawY));
							p.Offset(_craftIconBracketSize / -2, _craftIconBracketSize / -2);
							r = new Rectangle(p, new Size(_craftIconBracketSize, _craftIconBracketSize));
						}
					}
				}

				if (r.Width > 0)
				{
					int pad = (isXwa ? 4 : 2);
					r.Inflate(pad, pad);
					drawRectangle(g, r, 3, Color.SteelBlue);
				}
			}
			else if (_tempEvent == AbstractEventType.XwaSetIcon && chkAutoMoveIcon.Checked)
			{
				MapElement temp = new MapElement
				{
					Enabled = true,
					X = (short)numX.Value,
					Y = (short)numY.Value,
					IconCraftType = cboCraftType.SelectedIndex,
					IconRotation = cboRotation.SelectedIndex,
					Color = cboIff.SelectedIndex,
					// The draw function checks for this to confirm drawing.
					// -1 means uninitialized and is skipped.  But we don't want to check for selected items at that index.
					LastMoveEventIndex = -2
				};
				if (temp.IconCraftType > 0) drawXwaIcon(g, temp, false);

				var pos = convertRawPosToPixelPos(temp.X, temp.Y);
				temp.X = pos.X;
				temp.Y = pos.Y;
				var iconData = getCraftIconData(temp.IconCraftType - 1);
				int size = iconData.SquareSize;
				Color c = Color.SteelBlue;
				bool empty = (size == 0 || temp.IconCraftType == 0);
				if (empty)
				{
					// Red bracket for empty image.
					size = _craftIconBracketSize / 2;
					c = Color.Red;
				}
				Rectangle rect = new Rectangle(temp.X - size / 2, temp.Y - size / 2, size, size);
				if (empty)
				{
					// Draw a red "X" to indicate nothing is here.
					g.DrawLine(Pens.Red, rect.Left, rect.Top, rect.Right, rect.Bottom);
					g.DrawLine(Pens.Red, rect.Left, rect.Bottom, rect.Right, rect.Top);
				}
				drawRectangle(g, rect, 4, c);
				_tempInteract = rect;
			}
			else if (_tempEvent == AbstractEventType.XwaMoveIcon || _tempEvent == AbstractEventType.XwaRotateIcon)
			{
				int iconIndex = cboIcon.SelectedIndex;

				// Pull relevant information to render the icon based on how it currently appears on the map.
				int craftType = 0;
				int color = 0;
				int rotation = 0;
				int x = (int)numX.Value;
				int y = (int)numY.Value;
				if (iconIndex >= 0 && _mapIcons[iconIndex].Enabled)
				{
					craftType = Math.Max(0, _mapIcons[iconIndex].IconCraftType);
					color = _mapIcons[iconIndex].Color;
					rotation = _mapIcons[iconIndex].IconRotation;
					if (_tempEvent == AbstractEventType.XwaRotateIcon)
					{
						x = _mapIcons[iconIndex].X;
						y = _mapIcons[iconIndex].Y;
					}
				}

				// Rotation event overrides whatever we pulled from the current icon state.
				if (_tempEvent == AbstractEventType.XwaRotateIcon) rotation = cboRotation.SelectedIndex;
				MapElement temp = new MapElement
				{
					Enabled = true,
					X = x,
					Y = y,
					IconCraftType = craftType,
					IconRotation = rotation,
					Color = color,
					// The draw function checks for this to confirm drawing.
					// -1 means uninitialized and is skipped.  But we don't want to check the index for selected items.
					LastMoveEventIndex = -2
				};
				drawXwaIcon(g, temp, false);

				var pos = convertRawPosToPixelPos(temp.X, temp.Y);
				temp.X = pos.X;
				temp.Y = pos.Y;
				var iconData = getCraftIconData(temp.IconCraftType);
				int size = iconData.SquareSize;
				Rectangle rect = new Rectangle(temp.X - size / 2, temp.Y - size / 2, size, size);
				drawRectangle(g, rect, 4, Color.SteelBlue);

				if (_tempEvent == AbstractEventType.XwaMoveIcon) _tempInteract = rect;
			}
			else if (_tempEvent == AbstractEventType.XwaShipInfo)
			{
				int iconIndex = cboIcon.SelectedIndex;
				if (optStateOn.Checked && iconIndex >= 0 && _mapIcons[iconIndex].Enabled)
				{
					var elem = _mapIcons[iconIndex];
					var pos = convertRawPosToPixelPos(elem.X, elem.Y);
					var iconData = getCraftIconData(elem.IconCraftType);
					int size = iconData.SquareSize;
					Rectangle rect = new Rectangle(pos.X - size / 2, pos.Y - size / 2, size, size);
					drawRectangle(g, rect, 4, Color.SteelBlue);
				}
			}
			else if (_tempEvent == AbstractEventType.XwingNewIcon)
			{
				var pos = convertRawPosToPixelPos((int)numX.Value, (int)numY.Value);

				int craftType = getAdjustedCraftType(cboCraftType.SelectedIndex, cboIff.SelectedIndex);
				var iconData = getCraftIconData(craftType);
				int size = 12;

				// Use original craft type for icon color.
				var bmp = getCraftIcon(craftType, getXwingIconColor(cboIff.SelectedIndex, cboCraftType.SelectedIndex), 0);
				if (isXwingHighDef())
				{
					size = 20;
					// If this code changes, fix the corresponding map icon drawing.
					Rectangle iconRect = new Rectangle(pos.X - iconData.Width, (pos.Y - iconData.Height) - 2, (iconData.Width * 2), (iconData.Height * 2) + 4);
					if ((iconData.Height & 1) != 0)
					{
						iconRect.Y++;
						iconRect.Height--;
					}
					g.InterpolationMode = InterpolationMode.NearestNeighbor;
					g.PixelOffsetMode = PixelOffsetMode.Half;
					g.DrawImage(bmp, iconRect);
					g.PixelOffsetMode = PixelOffsetMode.Default;
				}
				else g.DrawImage(bmp, pos.X + iconData.DrawOffsetX, pos.Y + iconData.DrawOffsetY);

				Rectangle rect = new Rectangle(pos.X - size / 2, pos.Y - size / 2, size, size);
				drawRectangle(g, rect, 4, Color.SteelBlue);
				_tempInteract = rect;
			}
		}

		/// <summary>Draws selection brackets around most currently selected types on the map.  Does not include brackets around temp events or path nodes.</summary>
		void drawSelectedItems(Graphics g)
		{
			// This will remain null unless multiple items are selected, for the purposes of double thick brackets.
			SelectedObject pso = null;
			if (_selectedObjects.Count > 1) pso = getPrimarySelectedObject();

			for (int i = 0; i < _flightgroups.Count; i++)
			{
				var afg = _flightgroups[i];
				if (!afg.Waypoints[getFlightgroupWaypoint()].Enabled && !chkIconShadows.Checked) continue;

				var sel = getSelectedItem(SelectedType.Waypoint, i);
				if (sel == null) continue;

				var r = afg.InteractRect;
				sel.InteractRect = r;
				int size = Math.Max(r.Width, r.Height) / 3;
				drawRectangle(g, r, size, Color.White);

				// Double thick bracket for primary selection.
				if (isMatchingSelection(sel, pso))
				{
					r.Inflate(1, 1);
					drawRectangle(g, r, size + 1, Color.White);
				}
			}

			// Special case for a Rotate event.  It doesn't have a direct item to display on the map.
			// Instead draw a selection bracket around all visible icons on the map that the rotate applies to.
			var rotate = getSelectedEventType(AbstractEventType.XwaRotateIcon);
			if (rotate != null)
			{
				foreach (var elem in _mapShadowIcons)
				{
					if (elem.IconTime != hsbTimer.Value && !isShadowIconVisible(elem) || elem.LastRotateEventIndex != rotate.Index) continue;

					if (elem.InteractRect.Width > 0) drawRectangle(g, elem.InteractRect, 4, Color.SteelBlue);
					else
					{
						// If the element is not visible (like the current time) it won't have an interact rect assigned.
						var iconData = getCraftIconData(elem.IconCraftType);
						var pos = convertRawPosToPixelPos(elem.X, elem.Y);
						Rectangle r = new Rectangle(pos.X - (iconData.SquareSize / 2), pos.Y - (iconData.SquareSize / 2), iconData.SquareSize, iconData.SquareSize);
						drawRectangle(g, r, 4, Color.SteelBlue);
					}
				}
			}

			
			foreach (var elem in _mapFgTags)
			{
				if (!elem.Enabled) continue;
				if (getSelectedItem(SelectedType.Event, elem.EventIndex) == null) continue;

				Rectangle r = elem.InteractRect;
				r.Inflate(5, 5);
				drawRectangle(g, r, 4, Color.SteelBlue);
			}

			foreach (var elem in _mapTextTags)
			{
				if (!elem.Enabled) continue;

				var sel = getSelectedItem(SelectedType.Event, elem.EventIndex);
				if (sel == null) continue;

				sel.InteractRect = elem.InteractRect;
				drawRectangle(g, elem.InteractRect, 4, Color.White);

				// Double thick bracket for primary selection.
				if (isMatchingSelection(sel, pso))
				{
					Rectangle r = elem.InteractRect;
					r.Inflate(1, 1);
					drawRectangle(g, r, 5, Color.White);
				}
			}

			if (chkIconShadows.Checked) foreach (var elem in _mapShadowIcons) drawXwaIconSelection(g, elem, true, pso);
			foreach (var elem in _mapIcons) drawXwaIconSelection(g, elem, false, pso);
		}

		/// <summary>Renders a text tag into the element's canvas, including all character animations and palette effects.</summary>
		bool updateTextTag(MapElement elem, string s, int time)
		{
			// Reusing the existing bitmap if nothing changes is a significant optimization.
			// Just make sure it exists and the animation has finished, if applicable.
			if (elem.TextTagBitmapCache != null && elem.TextTagStringCache == s && elem.TextTagRenderFinished) return false;

			Size sz = measureTagString(s, _tagFont);

			const int GrowSize = 20;
			int bmpWidth = GrowSize;
			if (elem.TextTagBitmapCache != null) bmpWidth = elem.TextTagBitmapCache.Width;
			if (sz.Width > bmpWidth) bmpWidth += (((sz.Width - bmpWidth) / GrowSize) + 1) * GrowSize;
			if (elem.TextTagBitmapCache == null || elem.TextTagBitmapCache.Width < bmpWidth)
			{
				elem.TextTagBitmapCache?.Dispose();

				// NOTE: The image needs to support transparency for the sake of blitting into the map.
				// We're constantly updating this image with new renderings.  LockBits() returns a memory block
				// that's compatible with the requested format, presumably adding overhead from a conversion.
				// Calling Bitmap.MakeTransparent() would transform the original bitmap to an alpha pixel format.
				// So we'll directly create and render ARGB instead, to avoid any conversions.
				elem.TextTagBitmapCache = new Bitmap(bmpWidth, sz.Height, PixelFormat.Format32bppArgb);
			}

			// Necessary for escaped characters and their length to be interpreted properly.
			char[] str = getUnescapedString(s, out int origStrLength);
			int strLength = origStrLength;
			if (chkAnimateTags.Checked && strLength > time) strLength = time;

			// This determines whether the string is fully drawn, even if the animation isn't finished yet.
			// The string grows by two chars per time tick.  Round up the length if odd.
			// Frames will be negative if the string is still drawing.  Zero if the string is drawn, but the colors are still animating.
			// In TIE Fighter, the animation plays up to the last zero frame.  Then all chars are set to the oldest
			// (usually brightest) active animated palette color, before the final inactive resting color.
			int evenLength = origStrLength + (origStrLength & 1);
			int framesOver = (time - evenLength) / 2;
			int maxFramesOver = (isXwing || evenLength != origStrLength ? 2 : 3);

			// If animation is disabled, the sequence will be marked as finished at the end.
			if (!chkAnimateTags.Checked) framesOver = maxFramesOver;

			var bmp = elem.TextTagBitmapCache;
			var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			int stride = Math.Abs(bmpData.Stride);
			int surfSize = bmpData.Height * stride;
			byte[] surfData = new byte[surfSize];

			var palette = getTextTagColorPalette(elem.Color);

			int startX = 0;
			Color highlight = new Color();
			var defaultColor = getTextTagColor(elem.Color);
			bool isHighlight = false;

			// Where to start dimming and which palette color to step down from.
			// In typical gradients, the palette indices are ordered darkest to brightest.
			int thresCharIndex = 0;

			// The last element of the palette is not used for per-character dimming.
			// It's normally a fullbright value used as the caret marker when the string is being animated.
			// Or in some cases to briefly highlight the entire string after it's finished drawing, but
			// before the animation has finished.
			int lastCharPalIndex = palette.Length - 2;
			int thresPalIndex = lastCharPalIndex;

			if (time == 2) thresPalIndex -= (isXwing ? 2 : 3);
			else thresCharIndex = (time - 1) - thresPalIndex;

			if (isXwing)
			{
				// The dimming rules change near the end of the full string.  Time is always even.
				// If the string is even and fully drawn, only the very last character is dimmed.
				// If odd, the dimmed animation is slightly out of alignment and appears to finish faster.
				if (time == origStrLength) thresCharIndex = origStrLength - 2;
				else if (time + 1 == origStrLength) thresCharIndex++;
				else if (time > origStrLength) thresCharIndex = origStrLength;
			}
			else
			{
				// This is most noticeable on TIE Fighter with the overflow colors, but the rules seem to apply
				// to XvT and XWA as well.

				// Odd-length strings at the end need a slight adjustment for when to adjust the palette.
				if (framesOver == 0 && time == origStrLength + 1) thresCharIndex--;

				// This is complicated, but it faithfully replicates the color.
				// For even-length strings, we need an extra frame over (3 instead of 2):
				// Odd-length strings: palette-1, then final color.
				// Even-length strings: palette-2, palette-1, then final
				if (framesOver > 0 && framesOver < maxFramesOver)
				{
					if (framesOver == 2) defaultColor = palette[palette.Length - 3];    // Even length, extra frame over
					else if (maxFramesOver == 2) defaultColor = palette[palette.Length - 2];    // Odd length, 1 frame over
					else defaultColor = palette[palette.Length - 1];    // Even length, 1 frame over
				}
			}

			// Color codes can change the current color.
			Color curColor = defaultColor;
			for (int i = 0; i < strLength; i++)
			{
				char c = str[i];

				if ((c == '[' || c == ']') && !isXwing)
				{
					if (c == '[')
					{
						highlight = getStandardColor(StandardColor.Highlight);
						isHighlight = true;
					}
					else
					{
						highlight = defaultColor;
						isHighlight = false;
					}

					continue;
				}

				if (c == 0) break;

				// The inline function decrements by 1 and has proper bounds checking.
				int maxCode = (isXwing || isTie ? ' ' : _inlineTextColors.Length + 1);
				if (c < maxCode)
				{
					if (c == 1) isHighlight = false;
					else
					{
						highlight = getInlineTextColor(c, curColor);
						isHighlight = true;
					}
					continue;
				}

				int colIndex = thresPalIndex;
				if (!isXwing && framesOver >= 1) colIndex = -1;
				else if (i >= thresCharIndex)
				{
					colIndex = thresPalIndex - (i - thresCharIndex);
					colIndex = clamp(colIndex, 0, lastCharPalIndex);
				}

				var glyph = _tagFont.GetChar(c);
				if (glyph.TagEncoding == null) continue;

				byte[] encData = glyph.TagEncoding;
				int height = encData[0];
				int encOffset;

				float intensity = 0;

				if (isHighlight) curColor = highlight;
				else if (colIndex == -1) curColor = defaultColor;
				else curColor = palette[colIndex];

				for (int y = 0; y < height; y++)
				{
					if (y >= bmpData.Height) break;

					int curX = startX;
					int surfOffset = (stride * y) + (curX * 4);
					encOffset = BitConverter.ToInt16(encData, 1 + (y * 2));
					int remain = 0;

					while (curX < bmp.Width)
					{
						if (remain == 0)
						{
							remain = encData[encOffset++];
							if (remain == 0) break;

							int color = encData[encOffset++];
							if (color == 0)
							{
								// The color value was empty, skip drawing anything.
								curX += remain;
								surfOffset += (remain * 4);
								remain = 0;
								continue;
							}
							else intensity = (color > 240 ? 1.0f : color / 255.0f);
						}

						surfData[surfOffset] = (byte)(curColor.B * intensity);
						surfData[surfOffset + 1] = (byte)(curColor.G * intensity);
						surfData[surfOffset + 2] = (byte)(curColor.R * intensity);
						surfData[surfOffset + 3] = 0xFF;  // Alpha
						surfOffset += 4;
						remain--;
						curX++;
					}
				}
				startX += glyph.Width + _tagFont.HorizontalSpacing;
			}

			Marshal.Copy(surfData, 0, bmpData.Scan0, surfSize);
			bmp.UnlockBits(bmpData);
			
			elem.TextTagStringCache = s;
			elem.TextTagRenderFinished = (framesOver >= maxFramesOver);
			elem.TextTagEndPos = new Point(startX, _tagFont.Height);
			return (framesOver < 0);
		}
		void drawTextTag(Graphics g, MapElement element)
		{
			if (!element.Enabled) return;

			SelectedObject sel = getSelectedItem(SelectedType.Event, element.EventIndex);
			
			// Fully expand the string if it's selected, otherwise may be hard to see and make changes to.
			// The very first frame of activation should display up to two characters (XWING only), followed
			// by the cursor box.  All platforms animate by 2 chars per tick.
			int time = (sel != null ? 100 : element.ElapsedTime);
			if (isXwing) time++;
			time *= 2;

			Point pos = convertRawPosToPixelPos(element.X, getInvertedY(element.Y));
			pos.Y += _tagFont.VerticalOffset;

			string s = getTextTag(element.DataIndex);
			bool empty = (s == "");

			// Placeholder when the map is idle, so the user knows there's something there to select.
			if (empty && !isPlaybackActive) s = "*";

			bool truncated = updateTextTag(element, s, time);
			Point endPos = element.TextTagEndPos;

			g.DrawImage(element.TextTagBitmapCache, pos);

			if (truncated && !empty)
			{
				int size = (isXvt || isXwa) ? 7 : 6;
				int offset = (isXvt || isXwa) ? 2 : 1;
				var cursor = getTextTagCaret(element.Color);
				Brush block = new SolidBrush(cursor);
				g.FillRectangle(block, new Rectangle(pos.X + endPos.X + offset, pos.Y, size, size));
				block.Dispose();
			}

			int textWidth = Math.Max(endPos.X, _tagFont.MaxWidth);
			Rectangle rect = new Rectangle(pos.X, pos.Y, textWidth, _tagFont.MaxHeight);

			// The selection area isn't centered quite right, and it can be small, especially for XWING and TIE.  So increase the size a bit.
			rect.Offset(-1, -1);
			rect.Inflate(2, 2);
			element.InteractRect = rect;
		}

		void drawXwaIcon(Graphics g, MapElement element, bool shadow)
		{
			if (!element.Enabled) return;
			if (shadow && element.EventIndex >= 0 && !isShadowIconVisible(element)) return;

			var pos = convertRawPosToPixelPos(element.X, element.Y);
			Color color = Color.Gray;
			if (!shadow) color = getIconColor(element.Color);
			else if (element.EventIndex == -1) color = Color.MediumSlateBlue;     // Shadow without event is a preview icon.

			// Display invalid icons when the briefing isn't running.  If running, FG tags will be displayed as null craft.
			if (!isXwaValidIcon(element.IconCraftType - 1))
			{
				if (!isPlaybackActive) color = Color.SlateGray;
				else return;
			}

			var bmp = getCraftIcon(element.IconCraftType - 1, color, element.IconRotation);
			var data = getCraftIconData(element.IconCraftType - 1);

			// If the icons are rotated, invert the offsets so they remain centered.
			int offsetX = data.GetDrawOffsetX();
			int offsetY = data.GetDrawOffsetY();
			int width = data.GetWidth();
			int height = data.GetHeight();
			if (element.IconRotation == 1 || element.IconRotation == 3)
			{
				// Swap.  This is important for narrow icons because their interact rect needs to correspond to the graphic orientation.
				int temp = offsetX;
				offsetX = offsetY;
				offsetY = temp;

				temp = width;
				width = height;
				height = temp;
			}

			int bracketSize = (int)(data.SquareSize * (shadow ? 0.60f : 0.75f));
			element.InteractRect = new Rectangle(pos.X - (bracketSize / 2), pos.Y - (bracketSize / 2), bracketSize, bracketSize);
			// If the image is empty, allow the minimum size for bounding box selection.
			if (data.SquareSize == 0) element.InteractRect.Size = new Size(1, 1);
			g.DrawImage(bmp, pos.X + offsetX, pos.Y + offsetY);

			// For preview icons, display an arrow if they would appear off screen.
			if (shadow && element.EventIndex == -1)
			{
				// NOTE: This isn't perfect.  Sometimes if an icon is off screen it won't detect it.
				// Even after shrinking slightly to compensate.  Sometimes if the edge is visible, it will
				// trigger an arrow anyway.
				Rectangle iconRect = new Rectangle(pos.X - (width / 2), pos.Y - (height / 2), width, height);
				iconRect.Inflate(-2, -2);
				Rectangle vismap = new Rectangle(0, 0, _mapCanvas.Width - 1, _mapCanvas.Height - 1);

				if (!vismap.IntersectsWith(iconRect))
				{
					Pen p = new Pen(Color.Yellow) { Width = 2 };

					const int ArrowLen = 6;
					// Left and right edges
					if (element.InteractRect.Right < vismap.Left)
					{
						g.DrawLine(p, 0, pos.Y, ArrowLen, pos.Y - ArrowLen);
						g.DrawLine(p, 0, pos.Y, ArrowLen, pos.Y + ArrowLen);
					}
					else if (element.InteractRect.Left > vismap.Right)
					{
						g.DrawLine(p, vismap.Right, pos.Y, vismap.Right - ArrowLen, pos.Y - ArrowLen);
						g.DrawLine(p, vismap.Right, pos.Y, vismap.Right - ArrowLen, pos.Y + ArrowLen);
					}

					// Top and bottom edges
					if (element.InteractRect.Bottom < vismap.Top)
					{
						g.DrawLine(p, pos.X, 0, pos.X - ArrowLen, ArrowLen);
						g.DrawLine(p, pos.X, 0, pos.X + ArrowLen, ArrowLen);
					}
					else if (element.InteractRect.Top > vismap.Bottom)
					{
						g.DrawLine(p, pos.X, vismap.Bottom, pos.X - ArrowLen, vismap.Bottom - ArrowLen);
						g.DrawLine(p, pos.X, vismap.Bottom, pos.X + ArrowLen, vismap.Bottom - ArrowLen);
					}

					p.Dispose();
				}
			}
		}
		void drawXwaIconSelection(Graphics g, MapElement element, bool shadow, SelectedObject primarySelection)
		{
			if (!element.Enabled) return;
			if (shadow && !isShadowIconVisible(element)) return;

			var sel = getSelectedItem(SelectedType.Event, element.LastMoveEventIndex);
			if (sel == null) return;

			// Verify if the event is actually a move event, in case something changed.
			if (!isValidEvent(sel) || _events[sel.Index].Event != AbstractEventType.XwaMoveIcon) return;

			// Need to set the interact, otherwise it can't be moved.
			sel.InteractRect = element.InteractRect;
			var r = element.InteractRect;
			Color bracketColor = (shadow ? Color.Cyan : Color.White);
			if (r.Width <= 1)
			{
				// Empty graphic, minimum size for selection.
				int sz = _craftIconBracketSize / 2;
				r.Size = new Size(sz, sz);
				r.Offset(-sz / 2, -sz / 2);
				bracketColor = Color.Red;
			}
			drawRectangle(g, r, 4, bracketColor);
			if (isMatchingSelection(sel, primarySelection))
			{
				// The double thick bracket isn't very noticeable on these, so draw triple thick.
				r.Inflate(1, 1);
				drawRectangle(g, r, 5, bracketColor);
				r.Inflate(1, 1);
				drawRectangle(g, r, 6, bracketColor);
			}
		}

		/// <summary>Draws a hollow rectangle or bracketed rectangle.</summary>
		void drawRectangle(Graphics g, Rectangle rect, int bracketSize, Color color, bool dash = false)
		{
			int x1 = rect.Left;
			int x2 = rect.Right;
			int y1 = rect.Top;
			int y2 = rect.Bottom;

			Pen p = new Pen(color);
			if (dash)
				p.DashStyle = DashStyle.Dash;

			// Draw top and bottom horizontal lines.
			if (bracketSize < 1 || (x2 - x1 <= bracketSize * 2))
			{
				g.DrawLine(p, x1, y1, x2, y1);  // Top
				g.DrawLine(p, x1, y2, x2, y2);  // Bottom
			}
			else
			{
				g.DrawLine(p, x1, y1, x1 + bracketSize, y1);  // Top left
				g.DrawLine(p, x2 - bracketSize, y1, x2, y1);  // Top right
				g.DrawLine(p, x1, y2, x1 + bracketSize, y2);  // Bottom left
				g.DrawLine(p, x2 - bracketSize, y2, x2, y2);  // Bottom right
			}

			// Draw left and right vertical lines.
			if (bracketSize < 1 || (y2 - y1 <= bracketSize * 2))
			{
				g.DrawLine(p, x1, y1, x1, y2);  // Left
				g.DrawLine(p, x2, y1, x2, y2);  // Right
			}
			else
			{
				g.DrawLine(p, x1, y1, x1, y1 + bracketSize);  // Top left
				g.DrawLine(p, x2, y1, x2, y1 + bracketSize);  // Top right
				g.DrawLine(p, x1, y2 - bracketSize, x1, y2);  // Bottom left
				g.DrawLine(p, x2, y2 - bracketSize, x2, y2);  // Bottom right
			}
			p.Dispose();
		}
		#endregion Rendering

		#region Xwing specific extras
		/// <summary>Retrieves the core XWING briefing object, allowing direct access to its pages and panels.</summary>
		/// <remarks>These values are not abstracted or subject to import/export.  Any modifications will directly alter the mission.</remarks>
		Platform.Xwing.Briefing getXwingCoreBriefing() => (Platform.Xwing.Briefing)_corePlatformBriefing;

		bool isXwingHighDef() => _font.CacheType == BitmapCacheType.FontCaptionHigh;
		void setXwingCanvas(bool highDef)
		{
			_combinedCanvas?.Dispose();
			_titleCanvas?.Dispose();
			_mapCanvas?.Dispose();
			_captionCanvas?.Dispose();
			_xwingPanel3Canvas?.Dispose();
			_xwingPanel4Canvas?.Dispose();

			bool swapFont = false;
			
			// From DOSBox screenshot cropped to briefing, it's also the typical maximum size of the briefing panels.
			int width = 212;
			int height = 138;
			int mapWidth = 212;

			// XWING is a special case since the panels are customizable.
			// The full size must be available to all items for drawing purposes.
			if (highDef)
			{
				// From screenshot size.
				width = 420;
				height = 330;

				// This caused many subtle alignment problems that were only noticeable with custom panels.
				// Initially thought to be truncated, or the excess ignored.  Yet the internal canvas really
				// does seem to be larger than the display area.
				mapWidth = 424;

				// If fonts are currently low def, swap to high if possible.
				if (_font.CacheType == BitmapCacheType.FontCaption) swapFont = true;
			}
			else
			{
				// If fonts are currently high def, swap to low.
				if (isXwingHighDef()) swapFont = true;
			}

			_combinedCanvas = new Bitmap(width, height);
			_titleCanvas = new Bitmap(width, height);
			_mapCanvas = new Bitmap(mapWidth, height);
			_captionCanvas = new Bitmap(width, height);
			_xwingPanel3Canvas = new Bitmap(width, height);
			_xwingPanel4Canvas = new Bitmap(width, height);

			if (swapFont && _fontAltCaption != null)
			{
				var temp = _font;
				_font = _fontAltCaption;
				_fontAltCaption = temp;

				temp = _tagFont;
				_tagFont = _fontAltTag;
				_fontAltTag = temp;
			}

			// If it's part of the init, the following may not be assigned yet.
			// The font and resulting canvas dimensions may be different, so they must be destroyed and rebuilt.
			foreach (var elem in _mapTextTags) elem?.ClearTextTagCache(true);

			updateFormSize(true);
		}

		string[] getXwingBriefings()
		{
			Platform.Xwing.Briefing brf = getXwingCoreBriefing();
			string[] result = new string[brf.Pages.Count];

			for (int i = 0; i < brf.Pages.Count; i++)
			{
				string s = "Page #" + (i + 1);
				string type = "";

				int pageType = brf.Pages[i].PageType;
				if (pageType >= 0 && pageType < brf.Templates.Count)
				{
					type = brf.Templates[pageType].GetPageDesc();
					if (stringEqual(type, "Text"))
					{
						// Check if it's likely a hint page.  If so, it will have more than one caption.
						// Some missions have extra captions for the same string, so don't count those.
						// During imports, the pages may not exist in the frontend, so make sure it's within
						// range.  It's not foolproof, there are some fringe cases in badly formed briefings
						// that will be be identified as hint pages when they're not.
						int captionCount = 0;
						int lastCaption = -1;
						if (i < _briefingList.Count)
						{
							foreach (var evt in _briefingList[i].Events)
							{
								if (evt.Event != AbstractEventType.CaptionText || evt.Params[0] == lastCaption) continue;

								captionCount++;
								lastCaption = evt.Params[0];
							}
						}
						if (captionCount > 1) type = "Hint";
					}
				}
				result[i] = s + ": " + type;
			}
			return result;
		}

		void rebuildXwingBriefingList()
		{
			bool temp = _loading;
			_loading = true;

			string[] items = getXwingBriefings();

			cboCurrentBriefing.Items.Clear();
			cboCurrentBriefing.Items.AddRange(items);
			cboCurrentBriefing.SelectedIndex = 0;

			lstXwingPages.Items.Clear();
			lstXwingPages.Items.AddRange(items);
			lstXwingPages.SelectedIndex = 0;

			_loading = temp;
		}

		/// <summary>Retrieves the rectangle used for a specific panel, scaled if necessary.</summary>
		/// <param name="panel">The original configuration of the panel.  Will always contain low-res coordinates.</param>
		/// <param name="scale">Scale to apply.  Should be 1 for low-def, or 2 for high-def.</param>
		Rectangle getXwingDestinationRect(Platform.Xwing.PagePanel panel, int scale)
		{
			// NOTE: This used to clamp to the actual bitmap size, but this caused text to be misaligned.
			int x1 = panel.Left * scale;
			int x2 = panel.Right * scale;
			int y1 = panel.Top * scale;
			int y2 = panel.Bottom * scale;
			return new Rectangle(x1, y1, (x2 - x1), (y2 - y1));
		}

		/// <summary>Returns a clamped rectangle that fits inside the canvas for the selection bracket.</summary>
		/// <param name="panel">Panel to retrieve.</param>
		/// <param name="canvas">Canvas bitmap used for clamping dimensions.</param>
		Rectangle getXwingSelectionRect(Platform.Xwing.PageTemplate.Elements panel, Bitmap canvas)
		{
			// The original panel destination rectangle is not clamped, which is intentional and necessary
			// for alignment to work properly in high-def mode.  For example, the right edge could be
			// 212 * 2 = 424, but the actual display canvas width is only 420.
			// We can't use the source rect either, because all text rendering is performed relative to (0, 0)
			// If the panel is moved, it might overflow the edge of the canvas and push the selection bracket
			// offscreen.  We need to compensate for the overflow by shrinking the source accordingly.

			int maxWidth = canvas.Width - 1;
			int maxHeight = canvas.Height - 1;
			
			var d = getXwingDestinationRect(getXwingPanel(panel), isXwingHighDef() ? 2 : 1);
			var r = _xwingSourceViewport[(int)panel];
			if (panel == Platform.Xwing.PageTemplate.Elements.Map)
			{
				// Needs to be adjusted slightly.  View area is constrained by the output.
				d.Location = getXwingMapSourceLocation(d);
				maxWidth = _combinedCanvas.Width;
			}
			if (r.X < 0) r.X = 0;
			if (r.Y < 0) r.Y = 0;
			// Compare the panel edges and subtract the overflows.
			if (d.Right > maxWidth) r.Width -= d.Right - maxWidth;
			if (d.Bottom > maxHeight) r.Height -= d.Bottom - maxHeight;

			return r;
		}

		/// <summary>Gets the source point of the visible canvas used for draw operations.</summary>
		/// <remarks>The full map is rendered, even if only a portion of it is visible.</remarks>
		/// <param name="rect">The rectangle of the map panel.  If running in high-def mode, it should already be scaled up by two.</param>
		/// <returns>A Point representing the top left corner of the visible map canvas.</returns>
		Point getXwingMapSourceLocation(Rectangle rect)
		{
			// Was having a lot of trouble getting the map position correct to both XW94 and XW98
			// across various panel configurations.  This seems to be mostly accurate.
			// NOTE: If the panel edges exceed their maximum ranges it's not centered correctly.
			Point ret = new Point();
			if (isXwingHighDef())
			{
				ret.X = (424 - rect.Width) / 2;
				ret.Y = (330 - rect.Height) / 2;
			}
			else
			{
				// Odd numbers will be off by one, so round them down.
				ret.X = (212 - (rect.Width & 0xFE)) / 2;
				ret.Y = (138 - (rect.Height & 0xFE)) / 2;
			}
			return ret;
		}
		
		/// <summary>Inserts a new briefing flightgroup.  Syncs to the main form.</summary>
		/// <param name="afg">Abstract flightgroup to create.</param>
		/// <param name="index">Index to insert.  Use -1 to insert at end of list.</param>
		/// <remarks>Only for the XWING platform, because the briefing contains its own flightgroups separate from the mission.</remarks>
		void insertXwingFlightgroup(AbstractFlightgroup afg, int index)
		{
			if (index == -1) index = _flightgroups.Count;

			// If it matches the count, it appends the list.
			if (index <= _flightgroups.Count)
			{
				Platform.Xwing.FlightGroup xfg = new Platform.Xwing.FlightGroup();
				
				// Need to set properly or the main form will encounter an exception trying to resolve the strings for the FG list.
				if (afg.CraftType < 18)
				{
					xfg.CraftType = (byte)afg.CraftType;
					xfg.ObjectType = 0;
				}
				else
				{
					xfg.CraftType = 0;
					xfg.ObjectType = (byte)afg.CraftType;
				}

				xfg.IFF = (byte)afg.CraftIff;
				xfg.Name = afg.Name;
				xfg.Waypoints[0] = afg.Waypoints[0];
				xfg.Waypoints[0].Enabled = true;
				for (int wp = 0; wp < 3; wp++)
				{
					xfg.Waypoints[7 + wp] = afg.Waypoints[1 + wp];
					xfg.Waypoints[7 + wp].Enabled = true;
				}
				_flightgroups.Insert(index, afg);
				notifyDataChange(DataChangeTags.FgNew, index, xfg);
			}
		}

		/// <summary>Deletes a new briefing flightgroup.  Syncs to the main form.</summary>
		/// <param name="index">Index to delete.</param>
		/// <remarks>Only for the XWING platform, because the briefing contains its own flightgroups separate from the mission.</remarks>
		void deleteXwingFlightgroup(int index)
		{
			// The main form won't allow an empty list.  If we force the issue, it will break.
			// This is needed as a preventative measure.
			if (_flightgroups.Count == 1)
			{
				// Deleting switches focus back to the main form, so bring it back to the briefing.
				Focus();
				popupError("You can't delete the last flightgroup.\n\nAt least one must remain in the briefing.");
				return;
			}
			if (index < 0 && index >= _flightgroups.Count) return;

			// Due to the complexities involved, we can't undo this operation, as it would corrupt the data state.
			clearUndo();

			// Modify any FG tags to skip this index.
			foreach (var evt in _events)
			{
				if (!evt.IsFgTag) continue;

				if (evt.Params[0] == index) evt.Params[0] = 0;
				else if (evt.Params[0] > index) evt.Params[0]--;
			}

			_flightgroups.RemoveAt(index);
			notifyDataChange(DataChangeTags.FgDelete, index, null);
		}

		/// <summary>Refreshes the lines in a listbox that contains panels and their visibility status.</summary>
		void refreshPanelListBox(ListBox listbox, Platform.Xwing.PageTemplate template, bool showPanelNum)
		{
			bool temp = _loading;
			_loading = true;

			// Build a placeholder list if empty.
			if (listbox.Items.Count == 0)
			{
				for (int i = 0; i < 5; i++) listbox.Items.Add("");
				listbox.SelectedIndex = 0;
			}

			int oldIndex = listbox.SelectedIndex;
			string[] panelNames = getXwingPanelNames();

			for (int i = 0; i < 5; i++)
			{
				string s = (showPanelNum ? $"#{i + 1}  " : "") + panelNames[i];
				if (template.Items[i].IsVisible) s += " - ON";
				listbox.Items[i] = s;
			}
			listbox.SelectedIndex = oldIndex;
			_loading = temp;
		}

		/// <summary>Refreshes the numeric control that displays the estimated number of text lines in a panel.</summary>
		/// <remarks>This control is shared by panel mode and the XWING settings tab.</remarks>
		void refreshPanelTextLines(Platform.Xwing.PageTemplate template, int panelIndex)
		{
			var thisPanel = template.Items[panelIndex];
			var map = template.GetElement(Platform.Xwing.PageTemplate.Elements.Map);
			bool visible = (thisPanel != map && map.IsVisible && thisPanel.IsVisible);

			lblPanelTextLines.Enabled = visible;
			numPanelTextLines.Enabled = visible;
			if (!visible) return;

			bool temp = _loading;
			_loading = true;
			// Round to the nearest full line, estimating.
			safeSetNumeric(numPanelTextLines, (thisPanel.Bottom - thisPanel.Top + 6) / 11);
			_loading = temp;
		}

		/// <summary>Retrieves a panel element from the currently selected briefing page that's visible on the display canvas.</summary>
		/// <remarks>This is for the main display, not necessarily panel mode.</remarks>
		Platform.Xwing.PagePanel getXwingPanel(Platform.Xwing.PageTemplate.Elements item)
		{
			var core = getXwingCoreBriefing();
			int pageType = core.Pages[_currentBriefingIndex].PageType;
			return core.Templates[pageType].GetElement(item);
		}
		/// <summary>Retrieve the page template being edited, either in panel mode or the XWING settings tab.</summary>
		Platform.Xwing.PageTemplate getWorkingPageTemplate() => (_panelMode ? getPanelModePageTemplate() : getXwingTabSelectedPageType());
		/// <summary>Retrieve the page panel being edited, either in panel mode or the XWING settings tab.</summary>
		Platform.Xwing.PagePanel getWorkingPagePanel() => (_panelMode ? getPanelModePagePanel() : getXwingTabSelectedPanel());
		int getWorkingPanelIndex() => Math.Max(0, (_panelMode ? lstPanelMode.SelectedIndex : lstXwingPagePanels.SelectedIndex));

		/// <summary>Saves the frontend panel values into the mission.</summary>
		/// <remarks>These controls are shared by panel mode and the XWING settings tab.</remarks>
		void applyPanelConfigToMission(bool refreshPanelMode)
		{
			var item = getWorkingPagePanel();
			item.IsVisible = chkPanelVisibility.Checked;
			item.Left = (short)numPanelLeft.Value;
			item.Right = (short)numPanelRight.Value;
			item.Top = (short)numPanelTop.Value;
			item.Bottom = (short)numPanelBottom.Value;

			if (refreshPanelMode) refreshPanelModeVisuals();
		}

		/// <summary>Loads the frontend panel values from the mission.</summary>
		/// <remarks>These controls are shared by panel mode and the XWING settings tab.</remarks>
		void applyPanelConfigToFrontEnd()
		{
			var item = getWorkingPagePanel();

			bool temp = _loading;
			_loading = true;
			chkPanelVisibility.Checked = item.IsVisible;
			safeSetNumeric(numPanelLeft, item.Left);
			safeSetNumeric(numPanelRight, item.Right);
			safeSetNumeric(numPanelTop, item.Top);
			safeSetNumeric(numPanelBottom, item.Bottom);
			_loading = temp;
		}
		#endregion Xwing specific extras

		#region XvT specific extras
		void loadXvtTitle(string missionFilename)
		{
			// Default string just in case the load fails.
			_titleString = "*Defined in .LST file*";

			string filename = Path.GetFileName(missionFilename);
			string folder = Path.GetDirectoryName(missionFilename);
			// Some folders have multiple .LST files.
			foreach (string lst in Directory.EnumerateFiles(folder, "*.lst"))
			{
				try
				{
					using (FileStream fs = new FileStream(lst, FileMode.Open, FileAccess.Read))
					{
						using (StreamReader sr = new StreamReader(fs))
						{
							string prevLine = "";
							while (!sr.EndOfStream)
							{
								string line = sr.ReadLine();

								// Section header or comment.
								if (line.StartsWith("[") || line.StartsWith("//"))
								{
									prevLine = "";
									continue;
								}

								if (prevLine.IndexOf(filename, StringComparison.OrdinalIgnoreCase) >= 0)
								{
									_titleString = line;
									break;
								}
								prevLine = line;
							}
						}
					}
				}
				catch { /* Any kind of error, keep the default string. */ }
			}
		}

		void resolvePlayerNumbers(List<AbstractFlightgroup> flightgroups)
		{
			// The XvT player number follows the ordering of the flightgroups on a per team basis.
			short[] count = new short[10];
			foreach (var fg in flightgroups) if (fg.PlayerNumber > 0 && fg.Team >= 0 && fg.Team < 10) fg.PlayerNumber = ++count[fg.Team];
		}

		/// <summary>Determines the first team visible in the briefing, so that player numbers can be displayed.</summary>
		/// <remarks>This uses what is currently loaded in the frontend.</remarks>
		int getVisibleTeam()
		{
			for (int i = 0; i < lstTeams.Items.Count; i++) if (lstTeams.GetSelected(i)) return i;
			return 0;
		}
		#endregion XvT specific extras

		#region XWA specific extras
		void loadXwaTitle(string missionFilename)
		{
			// Default string just in case the load fails.
			_titleString = "*Defined in .LST file*";

			string filename = Path.GetFileName(missionFilename);
			string folder = Path.GetDirectoryName(missionFilename);
			string lst = Path.Combine(folder, "MISSION.LST");
			if (!File.Exists(lst)) return;

			try
			{
				using (FileStream fs = new FileStream(lst, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader sr = new StreamReader(fs))
					{
						string prevLine = "";
						while (!sr.EndOfStream)
						{
							string line = sr.ReadLine();
							if (prevLine.IndexOf(filename, StringComparison.OrdinalIgnoreCase) >= 0)
							{
								int pos = line.IndexOf('!');
								if (pos >= 0) pos = line.IndexOf('!', pos + 1);
								if (pos >= 0) pos++;
								else pos = 0;
								_titleString = line.Substring(pos);
								break;
							}
							prevLine = line;
						}
					}
				}
			}
			catch { /* Any kind of error, keep the default string. */ }
		}

		void loadXwaSounds(string missionFilename)
		{
			string soundFolder = getXwaSoundFolder(missionFilename);
			if (soundFolder == "" || !Directory.Exists(soundFolder)) return;

			_captionSounds = new Dictionary<int, SoundPlayer>();

			try
			{
				foreach (string fullname in Directory.EnumerateFiles(soundFolder, "*.WAV"))
				{
					string filename = fullname.Substring(fullname.LastIndexOf("\\") + 1);
					if (!filename.StartsWith("B", StringComparison.OrdinalIgnoreCase)) continue;

					// B######.WAV
					// Three pairs of numbers:  Battle, mission, and one-based string number.
					// The only thing that's relevant for our purposes is the string number.
					if (filename.Length != 11) continue;
					if (!char.IsDigit(filename[5]) || !char.IsDigit(filename[6])) continue;

					int tens = filename[5] - '0';
					int ones = filename[6] - '0';
					int wavNum = (tens * 10) + ones;

					SoundPlayer sp = new SoundPlayer(fullname);
					sp.Load();
					if (!_captionSounds.ContainsKey(wavNum)) _captionSounds.Add(wavNum, sp);
				}
			}
			catch { /* Do nothing */ }
		}

		string getXwaSoundFolder(string filename)
		{
			int pos = filename.IndexOf("MISSIONS\\", StringComparison.OrdinalIgnoreCase);
			if (pos == -1) return "";

			string wavPath = filename.Remove(pos);

			// Extract the part of the string with the battle and filename
			pos = filename.LastIndexOf('\\') + 1;
			filename = filename.Substring(pos);

			int bpos = filename.IndexOf("B", StringComparison.OrdinalIgnoreCase);
			int mpos = filename.IndexOf("M", StringComparison.OrdinalIgnoreCase);
			int epos = -1;
			if (bpos >= 0 && mpos > bpos)
			{
				for (pos = mpos + 1; pos < filename.Length; pos++)
				{
					if (!char.IsDigit(filename[pos]))
					{
						epos = pos;
						break;
					}
				}
			}

			string extract = "";
			if (epos >= 0) extract = filename.Substring(bpos, epos - bpos);
			wavPath += "WAVE\\FRONTEND\\" + extract;
			return wavPath;
		}

		void playCaptionSound(int stringIndex)
		{
			if (_captionSounds == null || !chkAudioPlayback.Checked) return;

			stopCaptionSound();
			int wavNum = stringIndex + 1;
			if (_captionSounds.ContainsKey(wavNum))
			{
				_activeSound = _captionSounds[wavNum];
				_activeSound.Play();
			}
		}

		void stopCaptionSound()
		{
			if (_activeSound != null)
			{
				_activeSound.Stop();
				_activeSound = null;
			}
		}
		
		void freeCaptionSounds()
		{
			if (_captionSounds == null) return;

			stopCaptionSound();
			foreach (var item in _captionSounds) item.Value.Dispose();
			_captionSounds.Clear();
		}

		bool isXwauInstalled(string installPath)
		{
			// TotalConverter is for older XWAU/TFTC installations, superceded by Configurator.  Either one
			// of those should always be present in an XWA installation, but checking the rest to be sure. 
			string[] subDirs = new string[] { "TotalConverter", "Configurator", "DynamicCockpit", "ShadowMapping" };
			foreach (string s in subDirs) if (Directory.Exists(Path.Combine(installPath, s))) return true;
			return false;
		}

		/// <summary>Checks if the craft is any species that the game refuses to render, even if they're defined in shiplist.txt and the bitmaps.</summary>
		/// <param name="craftType">Zero-based craft type, where X-Wing is zero.</param>
		bool isXwaValidIcon(int craftType) => (craftType >= 0 && craftType < _craftIconImages.Count && !_craftIconImages[craftType].Hidden);
		#endregion XWA specific extras

		#region Status and verification
		/// <summary>Generates a summary report of the asset load process, indicating where assets were loaded or generated from.</summary>
		void generateLoadReport()
		{
			string s = getFontLoadReport(_font, "Main Font");
			s += getFontLoadReport(_tagFont, "Tag Font");
			bool fontFail = (!_font.IsAssetLoaded() || !_tagFont.IsAssetLoaded());

			if (isXwing)
			{
				s += getFontLoadReport(_fontAltCaption, "High Def Font");
				fontFail |= !_fontAltCaption.IsAssetLoaded();
			}
			else if (isXvt || isXwa)
			{
				s += getFontLoadReport(_fontAltCaption, "Title Font");
				fontFail |= !_fontAltCaption.IsAssetLoaded();
			}

			bool origIconsLoaded = (_iconAssetSource != AssetSourceType.None);
			// If it has 1, it's just the placeholder empty bitmap.
			if (_craftIconImages.Count <= 1) s += "Map Icons: CRITICAL - nothing loaded!" + Environment.NewLine;
			else s += "Map Icons: " + getAssetSourceResult(_iconAssetSource) + Environment.NewLine;

			if (fontFail || !origIconsLoaded)
			{
				s += "\r\n\r\nCheck that your installation paths are assigned correctly in settings.";
				if (isXwing)
				{
					s += "  Recommended to target X-Wing 1998 (Windows), as DOS 1993 or 1994 may be incomplete or inaccessible.";
					s += "  Alternatively, place a copy of XWING.LFD and BWING.LFD into YOGEME's application folder.";
					if (fontFail) s += "  If TIE Fighter's installation path is assigned, fonts can be loaded from there, too.";
				}
				else if (isTie) s += "  Alternatively, place a copy of EMPIRE.LFD and PLAYER.LFD into YOGEME's application folder.";
			}

			// Exact paths at the bottom, as a last resort to check if something is wrong.
			s += "\r\n\r\nExact Load Paths:\r\n";
			s += $"Main Font: {_font.SourceFile}\r\n";
			s += $"Tag Font: {_tagFont.SourceFile}\r\n";
			if (isXwing) s += $"High Def Font: {_fontAltCaption.SourceFile}\r\n";
			else if (isXvt || isXwa) s += $"Title Font: {_fontAltCaption.SourceFile}\r\n";

			txtStatusLoadReport.Text = s;

			// Generate a new text string for the quick status in the lower right corner.
			if (!fontFail && origIconsLoaded) s = "Assets Loaded OK";
			else
			{
				s = "Font " + (fontFail ? "ERR" : "OK");
				s += ", Icon " + (origIconsLoaded ? "OK" : "ERR");
			}
			lblStatus.Text = s;
		}

		/// <summary>Returns a string indicating whether the asset was successfully loaded, and from which source, for report purposes.</summary>
		string getAssetSourceResult(AssetSourceType sourceType)
		{
			switch (sourceType)
			{
				case AssetSourceType.Application: return "OK - application folder";
				case AssetSourceType.Installation: return "OK - installation folder";
				case AssetSourceType.AlternateApplication: return "OK - substituted " + (isXwing ? "TIE Fighter" : "X-Wing") + " from application folder";
				case AssetSourceType.AlternateInstallation: return "OK - substituted " + (isXwing ? "TIE Fighter" : "X-Wing") + "'s installation folder";
				case AssetSourceType.Mission: return "OK - mission path detection";
			}
			return "ERROR - not found, using placeholder";
		}
		string getFontLoadReport(BriefingFont font, string name) => name + ": " + getAssetSourceResult(font.SourceType) + Environment.NewLine;

		/// <summary>Attempts to validate whether a value exists between an acceptable range, and reports an error message if it doesn't.  Optionally returns the clamped value.</summary>
		/// <param name="value">Input value to verify.</param>
		/// <param name="min">Minimum acceptable value, inclusive.</param>
		/// <param name="max">Maximum acceptable value, inclusive.</param>
		/// <param name="repair">Whether the value should be clamped to the min-max range.</param>
		/// <param name="error">Determines whether this should be reported as an error, or a warning.</param>
		/// <param name="report">List where the logged message will be added.</param>
		/// <param name="message">String message to log.</param>
		/// <returns>If repair is false, returns the unmodified input value.  Otherwise returns the clamped value.</returns>
		int verifyRepair(int value, int min, int max, bool repair, bool error, List<string> report, string message)
		{
			if (value < min || value > max)
			{
				int old = value;
				if (repair) value = clamp(value, min, max);

				verifyAddMessage(repair, error, report, message + " value (" + old + ") out of range");
			}
			return value;
		}

		/// <summary>Adds a message to the error report log.</summary>
		/// <param name="repair">Determines whether this message was part of a repair operation.</param>
		/// <param name="error">Determines whether this should be reported as an error, or a warning.</param>
		/// <param name="report">List where the logged message will be added.</param>
		/// <param name="message">String message to log.</param>
		void verifyAddMessage(bool repair, bool error, List<string> report, string message)
		{
			// For the sake of sorting when the output is finalized, the messages are prefixed with a category tag.
			if (report == null) return;

			string prefix = (repair ? "R:" : (error ? "E:" : "W:"));
			report.Add(prefix + message);
		}

		/// <summary>Returns the number of messages in the report log that contain the specified category prefix tag.</summary>
		int getVerifyCount(List<string> report, string prefix)
		{
			int count = 0;
			foreach (string s in report) if (s.StartsWith(prefix)) count++;
			return count;
		}

		/// <summary>Modifies all strings in the report, removing their category prefix tags.</summary>
		void trimVerifyPrefixes(List<string> report)
		{
			for (int i = 0; i < report.Count; i++)
			{
				string s = report[i];
				if (s.StartsWith("R:") || s.StartsWith("E:") || s.StartsWith("W:")) report[i] = s.Substring(2);
			}
		}

		/// <summary>Returns a filtered list of strings containing only the specified category tag.</summary>
		/// <remarks>The prefix is removed and replaced with padding for indentation purposes.</remarks>
		List<string> getVerifyLines(List<string> report, string prefix, string padding)
		{
			List<string> ret = new List<string>();
			foreach (string s in report) if (s.StartsWith(prefix)) ret.Add(padding + s.Substring(prefix.Length));
			return ret;
		}

		/// <summary>Scans XWING panel and page settings for issues, adding them to the report, and optionally repairing them if possible.</summary>
		void verifyXwing(List<string> report, bool repair)
		{
			var core = getXwingCoreBriefing();

			// See t5m16wb for an example of XW94 corruption.  It can cause FG data like craft, IFF, and position to be overwritten and incorrect.
			if (_flightgroups.Count > 24) verifyAddMessage(false, false, report, "More than 24 briefing FGs, may cause corruption in X-Wing 1994");
			if (_flightgroups.Count > 40) verifyAddMessage(false, false, report, "More than 40 briefing FGs, may cause corruption in X-Wing 1998");

			for (int i = 0; i < core.Templates.Count; i++)
			{
				var pageType = core.Templates[i];
				
				bool isMap = pageType.GetElement(Platform.Xwing.PageTemplate.Elements.Map).IsVisible;
				if (i == 0 && !isMap) verifyAddMessage(false, false, report, "Page #1 must contain a map in XWVM");
				else if (i > 0 && isMap) verifyAddMessage(false, false, report, $"Page #{i + 1} must be text-only in XWVM");

				for (int j = 0; j < pageType.Items.Length; j++)
				{
					var item = pageType.Items[j];
					string s = $"Page #{i + 1}, panel #{j + 1} ";

					item.Left = (short)verifyRepair(item.Left, 0, 212, repair, true, report, s + "left edge");
					item.Right = (short)verifyRepair(item.Right, 0, 212, repair, true, report, s + "right edge" );
					item.Top = (short)verifyRepair(item.Top, 0, 138, repair, true, report, s + "top edge");
					item.Bottom = (short)verifyRepair(item.Bottom, 0, 138, repair, true, report, s + "bottom edge");

					if (item.IsVisible)
					{
						if (item.Left == 0 && item.Right == 0) verifyAddMessage(false, false, report, s + "does not have the left/right edges defined.");
						if (item.Top == 0 && item.Bottom == 0) verifyAddMessage(false, false, report, s + "does not have the top/bottom edges defined.");
					}
					if (item.Left != 0 && item.Left >= item.Right)
					{
						verifyAddMessage(repair, true, report, s + "left edge must be less than right edge");
						if (repair)
						{
							short temp = item.Left;
							item.Left = item.Right;
							item.Right = temp;
						}
					}
					if (item.Top != 0 && item.Top >= item.Bottom)
					{
						verifyAddMessage(repair, true, report, s + "top edge must be less than bottom edge");
						if (repair)
						{
							short temp = item.Top;
							item.Top = item.Bottom;
							item.Bottom = temp;
						}
					}

					// XWVM roughly supports vertical size, but the map won't be aligned correctly.
					// Due to variations in official missions, we can't test for specific values for top and
					// bottom.  Only left and right.
					if (item.IsVisible && (item.Left != 0 || item.Right != 212))
						verifyAddMessage(false, false, report, s + "custom panel horizontal alignments are not supported in XWVM");
				}
			}
			for (int i = 0; i < core.Pages.Count; i++)
			{
				var page = core.Pages[i];
				string s = "Page #" + (i + 1) + "";;

				page.PageType = (short)verifyRepair(page.PageType, 0, core.Templates.Count - 1, repair, true, report, s + "page type");
				page.Waypoint = (short)verifyRepair(page.Waypoint, 0, core.MaxWaypoints - 1, repair, true, report, s + "waypoint set");
			}
		}

		/// <summary>Verifies the data loaded into an abstract briefing object.</summary>
		/// <param name="brf">Object to scan.</param>
		/// <param name="report">List to receive error strings.  One item per error.  May be null if response information is not needed.</param>
		/// <param name="repair">If true, automatically repairs out-of-bounds errors by clamping them to their acceptable ranges.  Warnings are not affected.</param>
		void verifyAbstractBriefing(AbstractBriefing brf, List<string> report, bool repair)
		{
			int space = calculateEventSpace(brf.Events);
			if (space > _eventSpaceMax) verifyAddMessage(false, true, report, $"Event space exceeded {space} / {_eventSpaceMax}");

			for (int i = 0; i < brf.Strings.Length; i++)
			{
				string s = brf.Strings[i];
				if (s.Length > _maxStringLength)
				{
					verifyAddMessage(repair, true, report, $"String #{i + 1} length too long");
					if (repair)
					{
						s = s.Remove(_maxStringLength);
						brf.Strings[i] = s;
					}
				}
				if (isXwing && hasRawCodes(s)) verifyAddMessage(false, false, report, $"String #{i + 1} has color codes, not supported in XWVM");
			}
			for (int i = 0; i < brf.Tags.Length; i++)
			{
				string s = brf.Tags[i];
				if (s.Length > _maxTagLength)
				{
					verifyAddMessage(repair, true, report, $"Tag #{i + 1} length too long");
					if (repair)
					{
						s = s.Remove(_maxTagLength);
						brf.Tags[i] = s;
					}
				}
				if (isXwing && hasRawCodes(s)) verifyAddMessage(false, false, report, $"Tag #{i + 1} has color codes, not supported in XWVM");
			}

			bool vis = false;
			if (isXvt || isXwa)
			{
				for (int j = 0; j < brf.ViewedByTeam.Length; j++) vis |= brf.ViewedByTeam[j];
				if (!vis && brf.Events.Count > 0) verifyAddMessage(false, false, report, "Briefing is not visible to any teams.");
			}

			brf.RunningTime = (short)clamp(brf.RunningTime, 0, MaxEventTime);

			// These vars track event run-time information while we scan the events.
			bool[] panel = new bool[4];
			bool[] textTag = new bool[8];
			bool[] fgTag = new bool[8];

			const int IconCount = 192;
			short[] icon = new short[IconCount * 4];
			for (int i = 0; i < icon.Length; i++) icon[i] = -1;

			sbyte[] infoState = new sbyte[IconCount];

			short lastTime = -1;
			AbstractEventType groupEvent = AbstractEventType.None;
			int index;
			bool craftState = false;
			int region = 0;
			int pageBreak = -1;
			// For the sake of the XW98 bug, this particular check will start at the expected in-game value.
			Point lastXwingZoom = new Point(16, 16);
			bool zoomEncountered = false;

			// NOTE: This can't verify whether strings graphically fit into their respective panels.
			for (int i = 0; i < brf.Events.Count; i++)
			{
				var evt = brf.Events[i];
				var def = EventDef.GetEventDefByType(evt.Event);
				string eventStr = $"{getTimeString(evt.Time)} {def.Name}: ";
				
				evt.Time = (short)verifyRepair(evt.Time, 0, MaxEventTime, repair, true, report, eventStr + "time");
				if (evt.Time < lastTime)
				{
					if (repair) evt.Time = lastTime;
					verifyAddMessage(repair, true, report, eventStr + "time is not in ascending order");
				}
				if (evt.Time != lastTime)
				{
					pageBreak = -1;
					lastTime = evt.Time;
					groupEvent = evt.Event;
				}

				switch (evt.Event)
				{
					case AbstractEventType.PageBreak:
						pageBreak = i;
						for (int j = 0; j < 4; j++) panel[j] = false;
						if (evt.Time == 0) verifyAddMessage(false, false, report, eventStr + "not needed at time zero");
						// XWING sometimes includes markers together with page breaks at the top of an event group.
						if (!isXwa && groupEvent != AbstractEventType.PageBreak && groupEvent != AbstractEventType.SkipMarker)
							verifyAddMessage(false, false, report, eventStr + "should appear before any other events at this time");
						break;

					case AbstractEventType.ZoomMap:
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 1, short.MaxValue, repair, true, report, eventStr + "X");
						evt.Params[1] = (short)verifyRepair(evt.Params[1], 1, short.MaxValue, repair, true, report, eventStr + "Y");
						if (evt.Params[0] != evt.Params[1]) verifyAddMessage(false, false, report, eventStr + "X and Y values don't match");

						if (isXwing)
						{
							Point newZoom = new Point(evt.Params[0], evt.Params[1]);
							if (evt.Time != 0 && newZoom == lastXwingZoom)
							{
								string s = (!zoomEncountered ? "Default" : "Current");
								s += $" ({lastXwingZoom.X / 2}, {lastXwingZoom.Y / 2}) to ({newZoom.X}, {newZoom.Y})";
								verifyAddMessage(false, false, report, eventStr + $"doubled zooms are bugged, ignored in X-Wing 1998. " + s);
							}
							lastXwingZoom = new Point(newZoom.X * 2, newZoom.Y * 2);
						}
						zoomEncountered = true;
						break;

					case AbstractEventType.TitleText:
					case AbstractEventType.CaptionText:
					case AbstractEventType.PanelText3:
					case AbstractEventType.PanelText4:
						// These events are out of order, so resolve which index they are.
						int panelIndex = (evt.Event <= AbstractEventType.CaptionText ? (evt.Event - AbstractEventType.TitleText) : (evt.Event - AbstractEventType.PanelText3) + 2);
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 0, _strings.Count - 1, repair, true, report, eventStr + "string index");
						if (isXwing && panel[panelIndex])
							verifyAddMessage(false, false, report, eventStr + "in XWING, strings must be cleared with Page Break before they're assigned again");
						else if (isXwa && pageBreak == -1 && evt.Time > 0)
							verifyAddMessage(false, false, report, eventStr + "a Page Break should exist at this time, but before this event");

						int strIndex = evt.Params[0];
						if (strIndex >= 0 && strIndex < _strings.Count)
						{
							if (_strings[strIndex] == "") verifyAddMessage(false, false, report, eventStr + $"caption string #{strIndex+1} is empty");
							else if (isXwing && evt.Event == AbstractEventType.TitleText && _strings[strIndex].IndexOf("Placeholder Title") >= 0)
								verifyAddMessage(false, false, report, eventStr + $"placeholder string #{strIndex+1} hasn't been changed");
							else if (isXwing && evt.Event == AbstractEventType.CaptionText && _strings[strIndex].IndexOf("Placeholder Caption") >= 0)
								verifyAddMessage(false, false, report, eventStr + $"placeholder string #{strIndex+1} hasn't been changed");
							else if (isXvt && evt.Time < _ticksPerSecond && hasRawCodes(_strings[strIndex], 7))
								verifyAddMessage(false, true, report, eventStr + $"caption string #{strIndex+1} non-standard color code may crash during screen fade");
						}

						if (evt.Event == AbstractEventType.PanelText3 || evt.Event == AbstractEventType.PanelText4)
							verifyAddMessage(false, false, report, eventStr + $"event not supported in XWVM");
						panel[panelIndex] = true;
						break;

					case AbstractEventType.ClearFgTags:
						for (int j = 0; j < fgTag.Length; j++) fgTag[j] = false;
						break;

					case AbstractEventType.FgTag1:
					case AbstractEventType.FgTag2:
					case AbstractEventType.FgTag3:
					case AbstractEventType.FgTag4:
					case AbstractEventType.FgTag5:
					case AbstractEventType.FgTag6:
					case AbstractEventType.FgTag7:
					case AbstractEventType.FgTag8:
						int fgCount = isXwa ? _mapIcons.Length : _flightgroups.Count;
						string fgStr = isXwa ? "icon" : "flightgroup";
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 0, fgCount - 1, repair, true, report, eventStr + "tag " + fgStr + " index");
						index = evt.Event - AbstractEventType.FgTag1;
						if (fgTag[index] && isXwing)
							verifyAddMessage(false, false, report, eventStr + " XWING requires FG tags to be cleared before they're assigned again");
						if (evt.Event >= AbstractEventType.FgTag5 && isXwing)
							verifyAddMessage(false, false, report, eventStr + " XWING only allows slots 1-4, will be discarded on save");
						fgTag[index] = true;
						break;

					case AbstractEventType.ClearTextTags:
						for (int j = 0; j < fgTag.Length; j++) textTag[j] = false;
						break;

					case AbstractEventType.TextTag1:
					case AbstractEventType.TextTag2:
					case AbstractEventType.TextTag3:
					case AbstractEventType.TextTag4:
					case AbstractEventType.TextTag5:
					case AbstractEventType.TextTag6:
					case AbstractEventType.TextTag7:
					case AbstractEventType.TextTag8:
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 0, _tags.Count - 1, repair, true, report, eventStr + "tag index");
						evt.Params[3] = (short)verifyRepair(evt.Params[3], 0, _textTagColors.Length - 1, repair, true, report, eventStr + "color index");
						index = evt.Event - AbstractEventType.TextTag1;
						if (textTag[index] && isXwing)
							verifyAddMessage(false, false, report, eventStr + " XWING requires text tags to be cleared before they're assigned again");
						if (evt.Event >= AbstractEventType.TextTag5 && isXwing)
							verifyAddMessage(false, false, report, eventStr + " XWING only allows slots 1-4, will be discarded on save");
						int tagIndex = evt.Params[0];
						if (tagIndex >= 0 && tagIndex < _tags.Count)
						{
							if (_tags[tagIndex] == "") verifyAddMessage(false, false, report, eventStr + $"tag string #{tagIndex+1} is empty");
							else if (isXvt && evt.Time < _ticksPerSecond && hasRawCodes(_tags[tagIndex], 7))
								verifyAddMessage(false, true, report, eventStr + $"tag string #{tagIndex+1} nonstandard color code may crash during screen fade");
						}
						textTag[index] = true;
						break;

					case AbstractEventType.XwaSetIcon:
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 0, _mapIcons.Length - 1, repair, true, report, eventStr + "icon index");
						evt.Params[1] = (short)verifyRepair(evt.Params[1], 0, _craftAbbrev.Length - 1, repair, true, report, eventStr + "craft type");
						evt.Params[2] = (short)verifyRepair(evt.Params[2], 0, _iconIffColors.Length - 1, repair, true, report, eventStr + "color index");
						index = evt.Params[0];
						if (index >= 0 && index < _mapIcons.Length)
						{
							icon[(region * IconCount) + index] = evt.Params[1];
							int craftIndex = evt.Params[1];   // Zero for null, X-Wing starts at 1
							if (craftIndex > 0)
							{
								var data = getCraftIconData(craftIndex - 1);
								if (data.SquareSize == 0)
									verifyAddMessage(false, false, report, eventStr + $"Icon #{index + 1} {getCraftAbbrev(craftIndex)} has no graphic");
								else if (!isXwaValidIcon(craftIndex - 1))
									verifyAddMessage(false, false, report, eventStr + $"Icon #{index + 1} {getCraftAbbrev(craftIndex)} graphic may not be visible in game");
							}
						}
						break;

					case AbstractEventType.XwaShipInfo:
						evt.Params[1] = (short)verifyRepair(evt.Params[1], 0, _mapIcons.Length - 1, repair, true, report, eventStr + "icon index");
						bool newState = (evt.Params[0] != 0);
						if (newState && craftState) verifyAddMessage(false, false, report, eventStr + "Info state is already on");
						if (!newState && !craftState) verifyAddMessage(false, false, report, eventStr + "Info state is already off");
						index = evt.Params[1];
						if (index >= 0 && index < _mapIcons.Length)
						{
							infoState[index] += (sbyte)(newState ? 1 : -1);
							if (newState && icon[(region * IconCount) + index] <= 0)
								verifyAddMessage(false, true, report, eventStr + $"the craft type for icon #{index + 1} is undefined");
						}
						craftState = newState;
						break;

					case AbstractEventType.XwaMoveIcon:
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 0, _mapIcons.Length - 1, repair, true, report, eventStr + "icon index");
						// Move and Rotate events for a null craft type are technically valid, but they can indicate that something is mismatched.
						index = evt.Params[0];
						if (index >= 0 && index < _mapIcons.Length && icon[(region * IconCount) + index] < 0)
							verifyAddMessage(false, false, report, eventStr + $"icon #{index + 1} hasn't been created yet");
						break;

					case AbstractEventType.XwaRotateIcon:
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 0, _mapIcons.Length - 1, repair, true, report, eventStr + "icon index");
						evt.Params[1] = (short)verifyRepair(evt.Params[1], 0, _iconRotations.Length - 1, repair, true, report, eventStr + "rotation type");
						// Rotate events can be left over if Move events are deleted.  Issue a warning to indicate that it could be cleaned up.
						index = evt.Params[0];
						if (index >= 0 && index < _mapIcons.Length && icon[(region * IconCount) + index] < 0)
							verifyAddMessage(false, false, report, eventStr + $"icon #{index + 1} hasn't been created yet");
						break;

					case AbstractEventType.XwaChangeRegion:
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 0, _regionNames.Length - 1, repair, true, report, eventStr + "region index");
						index = evt.Params[0];
						if (index >= 0 && index < 4) region = index;
						break;

					case AbstractEventType.XwaInfoParagraph:
						evt.Params[0] = (short)verifyRepair(evt.Params[0], 0, _strings.Count - 1, repair, true, report, eventStr + "string index");
						int info = evt.Params[0] - 1;
						if (info >= 0 && info < _strings.Count && _strings[info] == "")
							verifyAddMessage(false, false, report, eventStr + $"caption string #{info+1} is empty");
						break;
				}
			}

			for (int i = 0; i < infoState.Length; i++)
			{
				if (infoState[i] == 0) continue;

				string s = " Mismatched Ship Info state for icon #" + (i + 1) + ", ";
				if (infoState[i] > 0) verifyAddMessage(false, false, report, s + "enabled without corresponding deactivation");
				else verifyAddMessage(false, false, report, s + "disabled without corresponding activation");
			}

			if (lastTime > brf.RunningTime) verifyAddMessage(false, false, report, "There are events beyond the briefing duration, they will not be activated in game.");
		}

		bool hasRawCodes(string s, int start = 0)
		{
			bool result = false;
			foreach (char c in getUnescapedString(s, out _)) result |= (c >= start && c <= 9);
			return result;
		}

		/// <summary>Runs a verification scan, repairs any issues, and displays the results in the verification tab.</summary>
		void repairBriefing()
		{
			List<string> report = new List<string>();

			if (isXwing) verifyXwing(report, true);

			// These will all be zero unless XWING reported something.
			int repCount = getVerifyCount(report, "R:");
			int errCount = getVerifyCount(report, "E:");
			int warnCount = getVerifyCount(report, "W:");

			// Filter and get ready to scan the pages/briefings.
			report = getVerifyLines(report, "R:", "");

			for (int i = 0; i < _briefingList.Count; i++)
			{
				List<string> pageReport = new List<string>();
				verifyAbstractBriefing(_briefingList[i], pageReport, true);

				int pageRep = getVerifyCount(pageReport, "R:");
				int pageErr = getVerifyCount(pageReport, "E:");
				int pageWarn = getVerifyCount(pageReport, "W:");
				repCount += pageRep;
				errCount += pageErr;
				warnCount += pageWarn;

				// Add any repaired issues to the report.
				if (pageRep > 0)
				{
					if (report.Count > 0) report.Add("");   // Newline
					string pageType = (isXwing ? "Page" : "Briefing");
					report.Add(pageType + " #" + (i + 1));
					report.AddRange(getVerifyLines(pageReport, "R:", "  "));
				}
			}

			if (repCount > 0)
			{
				report.Insert(0, "Repaired Issues:");
				markDirty();
			}

			if (errCount + warnCount > 0)
			{
				if (repCount == 0) report.Insert(0, "No repairable issues detected.");
				if (report.Count > 0) report.Add("");   // Newline
				if (errCount > 0) report.Add("Remaining Errors: " + errCount);
				if (warnCount > 0) report.Add("Remaining Warnings: " + warnCount);
				report.Add("Re-verify to get the remaining issues.");
			}

			// Compile the result string from everything in the report.
			string res = "";
			foreach (string s in report) res += (s + Environment.NewLine);
			if (res == "") res = "Nothing to repair, no issues detected.";
			txtVerify.Text = res;
		}

		void verifyBriefing(bool setStatus, bool displayMessage)
		{
			List<string> report = new List<string>();

			if (isXwing) verifyXwing(report, false);

			// These will all be zero unless XWING reported something.
			int errCount = getVerifyCount(report, "E:");
			int warnCount = getVerifyCount(report, "W:");

			trimVerifyPrefixes(report);

			for (int i = 0; i < _briefingList.Count; i++)
			{
				List<string> pageReport = new List<string>();
				verifyAbstractBriefing(_briefingList[i], pageReport, false);

				int pageErr = getVerifyCount(pageReport, "E:");
				int pageWarn = getVerifyCount(pageReport, "W:");
				errCount += pageErr;
				warnCount += pageWarn;
				
				if (pageErr + pageWarn > 0)
				{
					if (report.Count > 0) report.Add("");   // Newline

					string pageType = (isXwing ? "Page" : "Briefing");
					report.Add(pageType + " #" + (i + 1));

					if (pageErr > 0)
					{
						report.Add("  Errors:");
						report.AddRange(getVerifyLines(pageReport, "E:", "    "));
					}
					if (pageWarn > 0)
					{
						report.Add("  Warnings:");
						report.AddRange(getVerifyLines(pageReport, "W:", "    "));
					}
				}
			}

			if (errCount + warnCount > 0)
			{
				string summary = "Verify ";
				if (errCount > 0) summary += "Error: " + errCount;
				if (warnCount > 0)
				{
					if (errCount > 0) summary += ", ";
					summary += "Warn: " + warnCount;
				}

				if (setStatus) lblStatus.Text = summary;
				if (displayMessage) popupInfo($"Verification finished: {Environment.NewLine}{summary}{Environment.NewLine}View the full report on the Verify tab.");
			}
			else if (displayMessage) popupInfo("Verification complete, no issues detected.");

			// Compile the result string from everything in the report.
			string res = "";
			foreach (string s in report) res += (s + Environment.NewLine);
			if (res == "") res = "Verification complete, no issues detected.";
			txtVerify.Text = res;
		}
		#endregion Status and verification

		#region Strings/Tags tab
		/// <summary>Retrieves a tag or string formatted as it should appear in the list control.</summary>
		string getMainStringListItem(string s, int index, int maxLength)
		{
			string prefix = (getUnescapedStringLength(s) > maxLength ? " !  " : "#");
			return prefix + (index + 1) + ": " + s;
		}

		/// <summary>Refreshes both the tag and string lists.  Either a single item if a valid index, or the whole list if -1.</summary>
		/// <remarks>Tries to retain the currently selected items.</remarks>
		void populateMainStrings(int tagIndex, int stringIndex)
		{
			bool btemp = _loading;
			_loading = true;

			int ti = Math.Max(0, lstMainTags.SelectedIndex);
			int si = Math.Max(0, lstMainStrings.SelectedIndex);

			if (tagIndex < 0)
			{
				lstMainTags.BeginUpdate();
				lstMainTags.Items.Clear();
				for (int i = 0; i < _tags.Count; i++) lstMainTags.Items.Add(getMainStringListItem(_tags[i], i, _maxTagLength));
				lstMainTags.EndUpdate();
			}

			if (stringIndex < 0)
			{
				lstMainStrings.BeginUpdate();
				lstMainStrings.Items.Clear();
				for (int i = 0; i < _strings.Count; i++) lstMainStrings.Items.Add(getMainStringListItem(_strings[i], i, _maxStringLength));
				lstMainStrings.EndUpdate();
			}

			// Turn off loading before setting the indices, so the textbox and labels refresh properly.
			_loading = false;

			lstMainTags.SelectedIndex = ti;
			lstMainStrings.SelectedIndex = si;

			// For a paste operation, the indices are the same so reassigning won't trigger their selection
			// changed events.  Assigning the item directly will do it.
			if (tagIndex >= 0) lstMainTags.Items[tagIndex] = getMainStringListItem(_tags[tagIndex], tagIndex, _maxTagLength);
			if (stringIndex >= 0) lstMainStrings.Items[stringIndex] = getMainStringListItem(_strings[stringIndex], stringIndex, _maxStringLength);

			_loading = btemp;
		}

		/// <summary>Assigns a string to a textbox and updates the label with the length.</summary>
		void setMainStringTextBox(TextBox txt, List<string> container, int index, Label label, int maxLength)
		{
			bool btemp = _loading;
			_loading = true;

			string s = "";
			if (index >= 0 && index < container.Count) s = container[index];

			// Indicates that a new UndoOperation should be created the next time the string is updated.
			txt.Tag = null;
			txt.Text = s;
			refreshMainStringLength(label, s, maxLength);

			_loading = btemp;
		}

		void refreshMainStringLength(Label label, string s, int maxLength)
		{
			int length = getUnescapedStringLength(s);
			label.Text = length + " / " + maxLength + " Chars";
			label.ForeColor = (length <= maxLength ? SystemColors.ControlText : Color.Red);
		}

		/// <summary>Applies a tag/string change from the specified textbox, refreshing the container, list, and labels.</summary>
		void updateMainStringFromTextBox(TextBox txt, List<string> container, int index, ListBox lst, Label label, int maxLength, UndoType undoType)
		{
			string oldstr = "";
			string newstr = txt.Text;

			if (index >= 0 && index < container.Count)
			{
				oldstr = container[index];
				container[index] = newstr;

				bool btemp = _loading;
				_loading = true;
				lst.Items[index] = getMainStringListItem(newstr, index, maxLength);
				_loading = btemp;
			}

			refreshMainStringLength(label, newstr, maxLength);

			UndoOperation op = null;
			if (txt.Tag != null) op = getUndoOperation((int)txt.Tag);
			if (op == null)
			{
				op = new UndoOperation(undoType, oldstr, index);
				addUpdatedUndoOperation(op, true);
				txt.Tag = op.UID;
			}
			op.Update(newstr);
		}

		/// <summary>Scans all events in the specified list that use the specified strings and swaps their indices.</summary>
		/// <param name="eventList">Event list to scan.  Matching event parameters will be modified.</param>
		/// <param name="type">String type to change.</param>
		/// <param name="sourceIndex">String to search for.</param>
		/// <param name="destIndex">String to replace with.</param>
		void swapEventString(List<AbstractEvent> eventList, BriefingString type, int sourceIndex, int destIndex, bool makeUndo)
		{
			// Search all events.  If any of the tags/strings being swapped are used by an event, update the parameter accordingly.
			for (int i = 0; i < eventList.Count; i++)
			{
				var evt = eventList[i];
				int newValue = -1;

				if (evt.Event == AbstractEventType.XwaInfoParagraph && type == BriefingString.String)
				{
					// Special case because the index is one-based, with zero indicating no string.
					if (evt.Params[0] > 0)
					{
						int index = evt.Params[0] - 1;
						if (index == sourceIndex) newValue = destIndex + 1;
						else if (index == destIndex) newValue = sourceIndex + 1;
					}
				}
				else
				{
					// All other events.
					if ((type == BriefingString.String && evt.IsAnyCaption) || (type == BriefingString.Tag && evt.IsTextTag))
					{
						if (evt.Params[0] == sourceIndex) newValue = destIndex;
						else if (evt.Params[0] == destIndex) newValue = sourceIndex;
					}
				}

				if (newValue >= 0)
				{
					// Conveniently, all events have the string/tag parameter at index 0.
					if (makeUndo)
					{
						UndoOperation op = new UndoOperation(UndoType.Event, evt.GetDataSnapshot(), i);
						evt.Params[0] = (short)newValue;
						op.Update(evt.GetDataSnapshot());
						addUpdatedUndoOperation(op, false);
					}
					else evt.Params[0] = (short)newValue;
				}
			}
		}

		void swapXwingEventStrings(BriefingString type, int sourceIndex, int destIndex)
		{
			if (!isXwing) return;

			// In XWING, all pages share the same strings.
			// For all pages that aren't currently loaded in the frontend, modify these events.
			// NOTE: Don't make undo operations for these, because undo/redo is only designed to work on the current briefing.
			for (int i = 0; i < _briefingList.Count; i++) if (i != _currentBriefingIndex) swapEventString(_briefingList[i].Events, type, sourceIndex, destIndex, false);
		}

		/// <summary>Performs a move up/down operation to swap a string or tag position in the list.</summary>
		/// <remarks>All events using the string are updated accordingly.</remarks>
		void moveMainString(int sourceIndex, int step, BriefingString type)
		{
			int count = (type == BriefingString.String ? lstMainStrings.Items.Count : lstMainTags.Items.Count);
			int destIndex = sourceIndex + step;
			if (sourceIndex < 0 || sourceIndex >= count || destIndex < 0 || destIndex >= count) return;

			beginUndoFrame(true);
			swapEventString(_events, type, sourceIndex, destIndex, true);
			swapXwingEventStrings(type, sourceIndex, destIndex);

			if (type == BriefingString.String)
			{
				// The notes are paired with the caption string, so move both.
				swapMainString(sourceIndex, destIndex, _strings, lstMainStrings, _maxStringLength, UndoType.SwapString);
				if (isXwa) swapMainString(sourceIndex, destIndex, _captionNotes, null, -1, UndoType.SwapNote);
			}
			else if (type == BriefingString.Tag) swapMainString(sourceIndex, destIndex, _tags, lstMainTags, _maxTagLength, UndoType.SwapTag);

			rebuildBriefing();
			refreshMainStringMoveButtons();
		}

		/// <summary>Swaps two strings or tags, modifying the container and refreshing the list.</summary>
		void swapMainString(int sourceIndex, int destIndex, List<string> container, ListBox lst, int maxLength, UndoType undoType)
		{
			string srcstr = container[sourceIndex];
			string deststr = container[destIndex];
			container[sourceIndex] = deststr;
			container[destIndex] = srcstr;

			if (lst != null)
			{
				bool load = _loading;
				_loading = true;
				lst.Items[sourceIndex] = getMainStringListItem(deststr, sourceIndex, maxLength);
				lst.Items[destIndex] = getMainStringListItem(srcstr, destIndex, maxLength);
				lst.SelectedIndex = destIndex;
				_loading = load;
			}
			if (undoType != UndoType.None)
			{
				UndoOperation op = new UndoOperation(undoType, sourceIndex, 0);
				op.Update(destIndex);
				addUpdatedUndoOperation(op, false);
			}
		}

		void refreshMainStringMoveButtons()
		{
			cmdMoveTagUp.Enabled = (lstMainTags.SelectedIndex > 0);
			cmdMoveTagDown.Enabled = (lstMainTags.SelectedIndex < lstMainTags.Items.Count - 1);

			cmdMoveStringUp.Enabled = (lstMainStrings.SelectedIndex > 0);
			cmdMoveStringDown.Enabled = (lstMainStrings.SelectedIndex < lstMainStrings.Items.Count - 1);
		}
		#endregion Strings/Tags tab

		#region Event list tab
		/// <summary>Refreshes an item in the events ListBox.</summary>
		/// <param name="index">Specific index to refresh, or -1 for all.</param>
		void refreshEventList(int index)
		{
			bool btemp = _loading;
			_loading = true;

			if (index < 0)
			{
				lstEvents.BeginUpdate();
				lstEvents.Items.Clear();
				for (int i = 0; i < _events.Count; i++) lstEvents.Items.Add(getEventString(i));
				lstEvents.EndUpdate();
				refreshEventListMoveButtons();
			}
			else if (index < _events.Count)
			{
				bool state = lstEvents.GetSelected(index);
				lstEvents.Items[index] = getEventString(index);
				lstEvents.SetSelected(index, state);
			}

			_loading = btemp;
		}

		/// <summary>Refreshes a range (inclusive) of items in the events ListBox.</summary>
		void refreshEventListRange(int startIndex, int endIndex)
		{
			if (startIndex < 0) startIndex = 0;
			if (endIndex >= lstEvents.Items.Count) endIndex = lstEvents.Items.Count - 1;

			// Drastically speeds up the refresh operation.
			bool btemp = _loading;
			_loading = true;
			lstEvents.BeginUpdate();
			for (int i = startIndex; i <= endIndex; i++) refreshEventList(i);
			lstEvents.EndUpdate();
			_loading = btemp;
		}

		/// <summary>Refreshes only the currently selected items in the events ListBox.</summary>
		void refreshEventListSelected()
		{
			bool btemp = _loading;
			_loading = true;
			lstEvents.BeginUpdate();
			foreach (int index in lstEvents.SelectedIndices) refreshEventList(index);
			lstEvents.EndUpdate();
			_loading = btemp;
		}

		/// <summary>Synchronizes the events ListBox to contain the main selection (sidebar, map tab).</summary>
		void applySelectionToEventList()
		{
			bool btemp = _loading;
			_loading = true;

			lstEvents.ClearSelected();
			foreach (var so in _selectedObjects) if (isValidEvent(so)) lstEvents.SetSelected(so.Index, true);

			_loading = btemp;

			refreshEventListSidebar();
			refreshEventListMoveButtons();
		}

		/// <summary>Synchronizes the main selection (sidebar, map tab) to contain the events ListBox selection.</summary>
		void applyEventListToSelection()
		{
			_selectedObjects.Clear();
			foreach (int index in lstEvents.SelectedIndices)
			{
				var evt = _events[index];
				_selectedObjects.Add(new SelectedObject(SelectedType.Event, index, evt.UID));
			}
		}

		/// <summary>Retrieves a string composed of spaces that are meant to be as wide as a decimal digit.</summary>
		string getDigitSpaces(int count)
		{
			// U+2007 is a "Figure Space" which seems to be available in most standard OS fonts.
			// However it doesn't exist in every font.  Some fonts without it seem to render a space, which is fine for our purposes.
			// As long as it's not replaced with a placeholder glyph (like a rectangle) indicating a nonexistent glyph.
			return new string('\u2007', count);
		}

		string padNumericString(string str, int minimumLength)
		{
			int diff = minimumLength - str.Length;
			if (diff <= 0) return str;
				
			return getDigitSpaces(diff) + str;
		}

		/// <summary>Retrieves a string describing the event, as it appears in the events ListBox.  The index must be valid.</summary>
		string getEventString(int eventIndex)
		{
			AbstractEvent evt = _events[eventIndex];

			string s = (chkEventListTickTime.Checked ? padNumericString(evt.Time.ToString(), 4) : getTimeString(evt.Time)) + "    ";

			if (chkEventListDifference.Checked)
			{
				int diff = 0;
				if (eventIndex > 0) diff = _events[eventIndex].Time - _events[eventIndex - 1].Time;

				if (chkEventListTickTime.Checked) s += (diff == 0 ? getDigitSpaces(4) : padNumericString(diff.ToString(), 4)) + "    ";
				else s += (diff == 0 ? "       " : getTimeString(diff)) + "    ";
			}

			switch (evt.Event)
			{
				case AbstractEventType.SkipMarker: s += "Marker"; break;
				case AbstractEventType.Unknown2: s += "Unknown 2"; break;
				case AbstractEventType.PageBreak: s += "Page Break"; break;
				case AbstractEventType.TitleText: s += $"Title Text: \"{getSafeString(_strings, evt.Params[0])}\""; break;
				case AbstractEventType.CaptionText: s += $"Caption Text: \"{getSafeString(_strings, evt.Params[0])}\""; break;

				case AbstractEventType.PanelText3:
				case AbstractEventType.PanelText4:
					s += $"Panel Text {evt.Event - AbstractEventType.PanelText3 + 3}: \"{getSafeString(_strings, evt.Params[0])}\"";
					break;

				case AbstractEventType.MoveMap: s += $"Move Map: X:{evt.Params[0]} Y:{evt.Params[1]}"; break;
				case AbstractEventType.ZoomMap: s += $"Zoom Map: X:{evt.Params[0]} Y:{evt.Params[1]}"; break;

				case AbstractEventType.ClearFgTags: s += "Clear FG Tags "; break;
				case AbstractEventType.FgTag1:
				case AbstractEventType.FgTag2:
				case AbstractEventType.FgTag3:
				case AbstractEventType.FgTag4:
				case AbstractEventType.FgTag5:
				case AbstractEventType.FgTag6:
				case AbstractEventType.FgTag7:
				case AbstractEventType.FgTag8:
					s += $"FG Tag {evt.Event - AbstractEventType.FgTag1 + 1}: {getFlightgroupTagString(evt.Params[0], evt.Time)}";
					break;

				case AbstractEventType.ClearTextTags: s += "Clear Text Tags "; break;
				case AbstractEventType.TextTag1:
				case AbstractEventType.TextTag2:
				case AbstractEventType.TextTag3:
				case AbstractEventType.TextTag4:
				case AbstractEventType.TextTag5:
				case AbstractEventType.TextTag6:
				case AbstractEventType.TextTag7:
				case AbstractEventType.TextTag8:
					s += $"Text Tag {evt.Event - AbstractEventType.TextTag1 + 1}: \"{getSafeString(_tags, evt.Params[0])}\"";
					break;

				case AbstractEventType.XwaSetIcon:
					s += $"New Icon #{evt.Params[0] + 1}: Craft: {getSafeString(_craftNames, evt.Params[1])} IFF: {getSafeString(_iffNames, evt.Params[2])}";
					break;
				case AbstractEventType.XwaShipInfo:
					s += $"Show Ship Data: Icon #{evt.Params[1] + 1} State: {(evt.Params[0] != 0 ? "On" : "Off")}";
					break;
				case AbstractEventType.XwaMoveIcon:
					s += $"Move Icon #{evt.Params[0] + 1}: X:{evt.Params[1]} Y:{evt.Params[2]}";
					break;
				case AbstractEventType.XwaRotateIcon:
					s += $"Rotate Icon #{evt.Params[0] + 1}: {getSafeString(_iconRotations, evt.Params[1])}";
					break;
				case AbstractEventType.XwaChangeRegion:
					s += "Switch to " + getRegionName(evt.Params[0], true);
					break;
				case AbstractEventType.XwaInfoParagraph:
					s += "Ship Info Text";
					if (evt.Params[0] >= 1) s += $": \"{getSafeString(_strings, evt.Params[0] - 1)}\"";
					else s += " NOT DISPLAYED";
					break;
				case AbstractEventType.UnknownClear: s += "Unknown Clear"; break;
				case AbstractEventType.UnknownEffect: s += "Unknown Effect"; break;
				case AbstractEventType.EndBriefing: s += "End Briefing"; break;
			}
			return s;
		}

		/// <summary>Refreshes everything in the event tab sidebar and event management panel, based off the current selection.</summary>
		void refreshEventListSidebar()
		{
			string diffText = "No events selected";
			bool selected = (lstEvents.SelectedIndex >= 0);
			if (selected)
			{
				var evt = _events[lstEvents.SelectedIndex];

				// Select the appropriate event type in the dropdown.
				var def = EventDef.GetEventDefByType(evt.Event);
				for (int i = 0; i < cboEventType.Items.Count; i++)
				{
					if (cboEventType.Items[i].ToString() != def.Name) continue;

					loadCbo(cboEventType, i);
					break;
				}

				// Assign the time for shifting purposes.
				if (chkEventListTickTime.Checked)
				{
					numEventTime.Maximum = MaxEventTime;
					numEventTime.DecimalPlaces = 0;
					safeSetNumeric(numEventTime, evt.Time);
				}
				else
				{
					numEventTime.Maximum = (decimal)((float)MaxEventTime / _ticksPerSecond);
					numEventTime.DecimalPlaces = 2;
					safeSetNumeric(numEventTime, (decimal)evt.Time / _ticksPerSecond);
				}

				// The normal sidebar for parameter editing.
				displayEditControls();

				// Measure the difference between selected times, for informational purposes.
				diffText = "Time Diff: ";
				var sic = lstEvents.SelectedIndices;
				if (sic.Count >= 2)
				{
					int first = sic[0];
					int last = sic[sic.Count - 1];
					int diff = _events[last].Time - _events[first].Time;
					if (chkEventListTickTime.Checked) diffText += diff.ToString() + (diff == 1 ? " tick" : " ticks");
					else diffText += getTimeString(diff, true);
				}
				else diffText += "N/A";
			}

			lblEventTimeDifference.Text = diffText;
			cmdDelete.Enabled = selected;
			pnlEventListManage.Enabled = selected;

			bool single = (lstEvents.SelectedIndices.Count == 1);
			lblEventType.Enabled = single;
			cboEventType.Enabled = single;
		}

		void refreshEventListMoveButtons()
		{
			bool upGroup = false;
			bool downGroup = false;
			bool upShift = false;
			bool downShift = false;

			var sic = lstEvents.SelectedIndices;
			if (sic.Count > 0)
			{
				int first = sic[0];
				int last = sic[sic.Count - 1];
				if (_events[first].Time == _events[last].Time)
				{
					upGroup = (first > 0 && _events[first].Time == _events[first - 1].Time);
					downGroup = (last < _events.Count - 1 && _events[last].Time == _events[last + 1].Time);
				}

				if (optEventListShiftSelected.Checked)
				{
					if (chkEventListShiftJump.Checked)
					{
						upShift = (_events[first].Time > 0);
						downShift = (_events[last].Time < MaxEventTime);
					}
					else
					{
						// Restricting move by jump only checks the first or last element.  The events are allowed to push up
						// against other events, but not coincide with them.  If the first or last item is selected, it doesn't
						// matter in that direction, just constrain by time.
						upShift = (first > 0 && _events[first].Time - _events[first - 1].Time > 1) || (first == 0 && _events[first].Time > 0);
						downShift = (last < _events.Count - 1 && _events[last + 1].Time - _events[last].Time > 1) || (last == _events.Count - 1 && _events[last].Time < MaxEventTime);
					}
				}
				else
				{
					upShift = (_events[first].Time > 0);
					downShift = (_events[lstEvents.Items.Count - 1].Time < MaxEventTime);
				}
			}

			cmdEventListGroupUp.Enabled = upGroup;
			cmdEventListGroupDown.Enabled = downGroup;
			cmdEventListShiftUp.Enabled = upShift;
			cmdEventListShiftDown.Enabled = downShift;
		}

		void moveListEventsInTime(int step)
		{
			// Grab the starting indices.
			HashSet<int> indices = new HashSet<int>();
			foreach (int index in lstEvents.SelectedIndices) indices.Add(index);

			beginUndoFrame(true);
			moveEventsInTime(step);
			applySelectionToEventList();

			// Merge the new indices from after the shift.
			foreach (int index in lstEvents.SelectedIndices) indices.Add(index);

			// Refresh all the indices that changed, old and new.
			foreach (int index in indices) refreshEventList(index);
		}

		void moveListEventsAcrossTime(int step)
		{
			if (lstEvents.SelectedIndices.Count == 0 || step == 0) return;

			int startLow = lstEvents.SelectedIndices[0];
			int startHigh = lstEvents.SelectedIndices[lstEvents.SelectedIndices.Count - 1];

			// Constrain the step offset by time limits.
			if (step < 0)
			{
				int firstTime = _events[startLow].Time;
				if (firstTime + step < 0) step = -firstTime;
			}
			else
			{
				int lastIndex = startHigh;
				if (optEventListShiftAll.Checked) lastIndex = lstEvents.Items.Count - 1;

				int lastTime = _events[lastIndex].Time;
				if (lastTime + step >= MaxEventTime) step = MaxEventTime - lastTime;
			}

			// Check again since it may have been modified.
			if (step == 0) return;

			beginUndoFrame(true);
			moveEventsAcrossTime(step, chkEventListShiftJump.Checked);
			applySelectionToEventList();

			int endLow = lstEvents.SelectedIndices[0];
			int endHigh = lstEvents.SelectedIndices[lstEvents.SelectedIndices.Count - 1];

			refreshEventListRange(Math.Min(startLow, endLow), Math.Max(startHigh, endHigh));
			refreshEventListMoveButtons();
		}

		string getRegionName(int index, bool verbose)
		{
			if (index >= 0 && index < _regionNames.Length) return (verbose ? "Region: " : "") + _regionNames[index];

			return "Region #" + (index + 1);
		}

		/// <summary>Retrieves a descriptive name for a FG Tag event as it appears in the event ListBox.</summary>
		/// <param name="index">Flightgroup index, or an icon index if XWA.</param>
		/// <param name="eventTime">Only for XWA, helps deduce which icons are defined at that point in time, as icons may be redefined in different regions.</param>
		string getFlightgroupTagString(int index, int eventTime)
		{
			string s = "";
			if (isXwa)
			{
				s = "Icon #" + (index + 1);
				int[] usage = getIconUsageArray(eventTime);
				if (index >= 0 && index < usage.Length)
				{
					unpackIconUsage(usage[index], out _, out int craftType, out _);
					s += " " + getCraftTypeAbbrev(craftType);
				}
			}
			else if (index >= 0 && index < _flightgroups.Count) s = _flightgroups[index].DisplayName;

			return s;
		}
		#endregion Event list tab

		#region Briefing options tab
		/// <summary>This should match the items and ordering contained in <see cref="cboBriefActions"/></summary>
		enum BriefingManagerActions
		{
			Swap = 0,   // Uses target dropdown
			Duplicate,  // Uses target dropdown
			Erase,
			ExportToBox,
			ImportFromBox,
			ImportFromFile,
		}

		void populateBriefingOptions()
		{
			int index = cboBriefTarget.SelectedIndex;
			cboBriefTarget.Items.Clear();
			for (int i = 0; i < _briefingList.Count; i++)
			{
				string s = $"#{i + 1} ";
				int strings = 0, tags = 0, events;

				// Count the strings used.  The current strings in the frontend may have been modified
				// since they were imported from the abstraction layer.
				if (i == _currentBriefingIndex)
				{
					s += "(current)";
					foreach (string item in _strings) { if (item != "") strings++; }
					foreach (string item in _tags)    {	if (item != "") tags++;    }
					events = _events.Count;
				}
				else
				{
					var brf = _briefingList[i];
					foreach (string item in brf.Strings) { if (item != "") strings++; }
					foreach (string item in brf.Tags)    { if (item != "") tags++;    }
					events = brf.Events.Count;
				}

				if (strings + tags + events > 0)
				{
					// XWING strings are global to all pages.
					if (isXwing && i > 0) s += " events:" + events;
					else s += $" str:{strings}, tag:{tags}, events:{events}";
				}
				else s += " (empty)";
				cboBriefTarget.Items.Add(s);
			}

			if (index < 0) index = 0;
			cboBriefTarget.SelectedIndex = index;
			refreshBriefingActions();
		}

		/// <summary>Refreshes control visibility when the briefing action selection is changed.</summary>
		void refreshBriefingActions()
		{
			if (cboBriefActions.SelectedIndex == -1) cboBriefActions.SelectedIndex = 0;
			BriefingManagerActions action = (BriefingManagerActions)cboBriefActions.SelectedIndex;
			bool useTarget = false;
			bool useExport = false;

			switch (action)
			{
				case BriefingManagerActions.Swap:
				case BriefingManagerActions.Duplicate:
					useTarget = true;
					break;

				case BriefingManagerActions.ExportToBox:
				case BriefingManagerActions.ImportFromBox:
				case BriefingManagerActions.ImportFromFile:
					useExport = true;
					refreshExportOptions();
					break;
			}

			bool useExportBox = (useExport && action != BriefingManagerActions.ImportFromFile);

			lblBriefTarget.Enabled = useTarget;
			cboBriefTarget.Enabled = useTarget;
			grpExportOptions.Enabled = useExport;
			lblBriefImport.Enabled = useExportBox;
			txtBriefImport.Enabled = useExportBox;
		}

		/// <summary>Refreshes the import/export checkboxes when the briefing action selection is changed</summary>
		void refreshExportOptions()
		{
			bool fileImport = ((BriefingManagerActions)cboBriefActions.SelectedIndex == BriefingManagerActions.ImportFromFile);
			bool everything = chkExportEverything.Checked;
			chkExportStrings.Enabled = !everything;
			chkExportEvents.Enabled = !everything;
			chkExportFlightgroup.Enabled = !everything;
			chkExportEmptyString.Enabled = !fileImport;
			chkExportXwing.Enabled = (isXwing);
		}

		bool confirmBriefingTarget()
		{
			int targetIndex = cboBriefTarget.SelectedIndex;
			if (targetIndex == _currentBriefingIndex || targetIndex == -1)
			{
				popupError("You must choose a target briefing that is different from the current briefing.");
				return false;
			}
			return true;
		}

		bool confirmBriefingAction(string message)
		{
			string paraBreak = Environment.NewLine + Environment.NewLine;
			string s = "Warning!" + paraBreak + message + paraBreak + "This action cannot be undone.  Are you sure?";
			return popupConfirm(s);
		}

		/// <summary>Retrieves the IFF names as used by the text import/export feature.</summary>
		/// <remarks>IFFs are different by platform, so it uses a name lookup to maintain consistency.  This only matters when importing to XWA to create flightgroup icons.</remarks>
		string[] getIffExportNames(Settings.Platform platform)
		{
			switch (platform)
			{
				case Settings.Platform.XWING:
					string[] ret = Platform.Xwing.Strings.IFF;
					// Zero is Default and will resolve to Rebel or Imperial depending on craft type.
					ret[3] = "Blue";
					ret[4] = "Blue";
					return ret;
				case Settings.Platform.TIE: return Platform.Tie.Strings.IFF;
				case Settings.Platform.XvT: return Platform.Xvt.Strings.IFF;
				case Settings.Platform.XWA: return Platform.Xwa.Strings.IFF;
			}
			return new string[] { "Unknown" };
		}

		/// <summary>Attempts to convert an IFF code between platforms by performing a string lookup of their faction/color values.</summary>
		/// <param name="destIffNames">IFF names of the destination platform.  See <see cref="getIffExportNames(Settings.Platform)"/>.</param>
		/// <param name="srcIffNames">IFF names of the source platform.  See <see cref="getIffExportNames(Settings.Platform)</param>
		/// <param name="srcIff">Index into the source string array.</param>
		/// <returns>The index of the matching element in the destination names.  Defaults to zero if nothing matched."/></returns>
		int getIffImportValue(string[] destIffNames, string[] srcIffNames, int srcIff)
		{
			if (srcIff < 0 || srcIff >= srcIffNames.Length) return 0;

			string src = srcIffNames[srcIff];
			for (int i = 0; i < destIffNames.Length; i++) if (stringEqual(src, destIffNames[i])) return i;

			return 0;
		}

		/// <summary>Retrieves the PageType panel names as used by the text import/export feature.</summary>
		string[] getXwingPanelNames() => Platform.Xwing.Strings.PagePanels;

		string addExportLine<T>(string str, string key, T value) => (str + key + "=" + value + Environment.NewLine);

		bool hasExportXwing() => (chkExportEverything.Checked || chkExportXwing.Checked);
		bool hasExportStrings() => (chkExportEverything.Checked || chkExportStrings.Checked);
		bool hasExportEvents() => (chkExportEverything.Checked || chkExportEvents.Checked);
		bool hasExportFlightgroups() => (chkExportEverything.Checked || chkExportFlightgroup.Checked);

		string exportBriefingAsText()
		{
			string s = "[YOGEME]" + Environment.NewLine;

			s = addExportLine(s, "Duration", (short)numDurationSec.Value);

			if (isXwing && hasExportXwing())
			{
				Platform.Xwing.Briefing core = getXwingCoreBriefing();

				string[] panelNames = getXwingPanelNames();
				for (int i = 0; i < core.Templates.Count; i++)
				{
					s += $"{Environment.NewLine}[PageType.{i + 1}]{Environment.NewLine}";
					var template = core.Templates[i];
					for (int j = 0; j < 5; j++)
					{
						var item = template.Items[j];
						s += $"{panelNames[j]}={item.Left},{item.Top},{item.Right},{item.Bottom},{item.IsVisible}" + Environment.NewLine;
					}
				}
			}
			
			if (isXvt || isXwa)
			{
				s += "ViewedByTeam=";
				for (int i = 0; i < lstTeams.Items.Count; i++)
				{
					if (i > 0) s += ",";
					s += (lstTeams.GetSelected(i) ? "1" : "0");
				}
				s += Environment.NewLine;
			}

			if (hasExportStrings())
			{
				s += Environment.NewLine + "[Tags]" + Environment.NewLine;
				for (int i = 0; i < _tags.Count; i++) if (chkExportEmptyString.Checked || _tags[i] != "") s += i + "=" + _tags[i] + Environment.NewLine;

				s += Environment.NewLine + "[Strings]" + Environment.NewLine;
				for (int i = 0; i < _strings.Count; i++) if (chkExportEmptyString.Checked || _strings[i] != "") s += i + "=" + _strings[i] + Environment.NewLine;
			}

			if (hasExportEvents())
			{
				s += Environment.NewLine + "[Events]" + Environment.NewLine;
				foreach (var evt in _events)
				{
					s += getTimeString(evt.Time, false) + " " + evt.Event;
					var def = EventDef.GetEventDefByType(evt.Event);
					bool invertTextTag = (evt.IsTextTag && isWaypointInvertedPlatform());
					for (int i = 0; i < def.NumParams; i++)
					{
						short v = evt.Params[i];
						if (invertTextTag && i == 2) v *= -1;
						s += " " + v;
					}
					s += Environment.NewLine;
				}
			}

			if (hasExportFlightgroups() && !isXwa)
			{
				s += Environment.NewLine + "[Flightgroups]" + Environment.NewLine;
				string[] allCraftAbbrev = CraftDataManager.GetInstance().GetShortNames();
				int wp = getFlightgroupWaypoint();
				for (int i = 0; i < _flightgroups.Count; i++)
				{
					var afg = _flightgroups[i];
					int iff = resolveIff(afg.CraftType, afg.CraftIff, _platform);
					string[] iffNames = getIffExportNames(_platform);
					if (iff < 0) iff = 0;
					if (iff >= iffNames.Length) iff = (isXwing ? iffNames.Length - 1 : 0);

					double y = afg.Waypoints[wp].Y;
					if (isWaypointInvertedPlatform()) y = -y;

					string name = afg.Name;
					if (name == "") name = "Unnamed";

					s += $"{i}={getSafeString(allCraftAbbrev, afg.CraftType)},{iffNames[iff]},{name},{afg.Waypoints[wp].X},{y},{afg.Waypoints[wp].Z},{afg.Waypoints[wp].Enabled}" + Environment.NewLine;
				}
			}
			return s;
		}
		
		int parseInt(string s, int defaultValue = 0)
		{
			bool success = int.TryParse(s, out int ret);
			return success ? ret : defaultValue;
		}
		bool parseBool(string s, bool defaultValue = false)
		{
			bool success = bool.TryParse(s, out bool ret);
			return success ? ret : defaultValue;
		}
		double parseDouble(string s, double defaultValue = 0.0)
		{
			bool success = double.TryParse(s, out double ret);
			return success ? ret : defaultValue;
		}
		
		string[] splitTrim(string s, char delimiter)
		{
			string[] ret = s.Split(delimiter);
			for (int i = 0; i < ret.Length; i++) ret[i] = ret[i].Trim();
			return ret;
		}

		bool loadImportDataFromText(string text, bool file, ImportData dat)
		{
			dat.IsTextImport = true;
			AbstractBriefing brf = new AbstractBriefing();
			brf.AllocateStrings(_strings.Count);

			// Make sure there's an empty object in the list even if the file fails to load.
			dat.Briefings.Add(brf);

			// Parse everything in the file.
			string[] lines = splitTrim(text, '\n');
			int section;
			Platform.Xwing.PageTemplate xwingCurTemplate = null;

			// Check for signature to make sure it's not a random text file.
			if (file && lines.Length > 0 && !stringEqual(lines[0], "[YOGEME]")) return false;
			else section = 1;

			for (int i = 0; i < lines.Length; i++)
			{
				string s = lines[i];
				if (s == "" || s.StartsWith(";")) continue;

				if (stringEqual(s, "[YOGEME]")) section = 1;
				else if (stringEqual(s, "[Tags]"))
				{
					section = 2;
					dat.AddSection(ImportData.Section.Tags);
				}
				else if (stringEqual(s, "[Strings]"))
				{
					section = 3;
					dat.AddSection(ImportData.Section.Strings);
				}
				else if (stringEqual(s, "[Events]"))
				{
					section = 4;
					dat.AddSection(ImportData.Section.Events);
				}
				else if (stringEqual(s, "[Flightgroups]"))
				{
					section = 5;
					dat.AddSection(ImportData.Section.Flightgroups);
				}
				else if (s.StartsWith("[PageType.", StringComparison.OrdinalIgnoreCase))
				{
					// Set to null section just in case the parse fails.
					section = 0;
					if (s.Length == 12 && s.EndsWith("]"))
					{
						int pageNum = s[10] - '0';
						if (pageNum < 1 || pageNum > 4)
						{
							popupError($"PageType section {s} must be 1 to 4");
							return false;
						}
						else
						{
							while (pageNum > dat.XwingPageTemplates.Count) dat.XwingPageTemplates.Add(new Platform.Xwing.PageTemplate());

							section = 6;
							dat.AddSection(ImportData.Section.PageTypes);
							xwingCurTemplate = dat.XwingPageTemplates[pageNum - 1];
						}
					}
				}
				else if (s.StartsWith("["))
				{
					section = 0;
					popupError("Unhandled section type:\n\n" + s);
				}
				else if (section > 0)
				{
					if (section == 1)
					{
						string[] tok = splitTrim(s, '=');
						string key = getSafeString(tok, 0);
						string value = getSafeString(tok, 1);
						if (stringEqual(key, "Duration")) brf.RunningTime = (short)(parseInt(value) * _ticksPerSecond);
						else if (stringEqual(key, "ViewedByTeam"))
						{
							dat.TextSections |= ImportData.Section.Visibility;
							string[] teams = value.Split(',');
							for (int j = 0; j < 10; j++) brf.ViewedByTeam[j] = (parseInt(getSafeString(teams, j)) != 0);
						}
					}
					else if (section == 2)
					{
						string[] tok = splitTrim(s, '=');
						int index = parseInt(getSafeString(tok, 0));
						if (index >= 0 && index < brf.Tags.Length) brf.Tags[index] = getSafeString(tok, 1);
					}
					else if (section == 3)
					{
						string[] tok = splitTrim(s, '=');
						int index = parseInt(getSafeString(tok, 0));
						if (index >= 0 && index < brf.Strings.Length) brf.Strings[index] = getSafeString(tok, 1);
					}
					else if (section == 4)
					{
						string[] tok = s.Split(' ');

						// Helps prevent rounding errors when converting back to integer units.
						double offset = (1.0 / _ticksPerSecond) / 2;
						int time = (int)((parseDouble(getSafeString(tok, 0)) + offset) * _ticksPerSecond);
						if (time < 0 || time > MaxEventTime)
						{
							popupError("Invalid time for event:\n\n" + s + "\n\nMust be between 0 and " + getTimeString(MaxEventTime, true));
							return false;
						}

						var def = EventDef.GetEventDefByInternalName(getSafeString(tok, 1));
						if (def == null || def.Type == AbstractEventType.None)
						{
							popupError("Failed to parse event type for:\n\n" + s);
							return false;
						}

						if (def.Type == AbstractEventType.EndBriefing) continue;

						AbstractEvent evt = new AbstractEvent((short)time, def.Type);
						for (int j = 0; j < def.NumParams; j++)
						{
							short.TryParse(getSafeString(tok, 2 + j), out short param);
							evt.Params[j] = param;
						}
						brf.Events.Add(evt);
					}
					else if (section == 5)
					{
						string[] tok = splitTrim(s, '=');
						string value = getSafeString(tok, 1);

						tok = splitTrim(value, ',');
						if (tok.Length == 7)
						{
							AbstractFlightgroup afg = new AbstractFlightgroup();

							string abbrev = getSafeString(tok, 0);
							string[] allCraftAbbrev = CraftDataManager.GetInstance().GetShortNames();
							int craftType = -1;
							for (int j = 0; j < allCraftAbbrev.Length; j++)
							{
								if (!stringEqual(abbrev, allCraftAbbrev[j])) continue;

								craftType = j;
								break;
							}

							if (craftType == -1)
							{
								popupError($"Flightgroup ship type {abbrev} does not exist, replacing with X-Wing.");
								craftType = 1;
							}

							afg.Abbrev = abbrev;
							afg.CraftType = craftType;

							string craftIff = getSafeString(tok, 1);
							string[] iffNames = getIffExportNames(_platform);
							for (int j = 0; j < iffNames.Length; j++)
							{
								if (!stringEqual(craftIff, iffNames[j])) continue;

								afg.CraftIff = j;
								break;
							}
							afg.Name = getSafeString(tok, 2);
							afg.Waypoints[0].X = parseDouble(getSafeString(tok, 3));
							afg.Waypoints[0].Y = parseDouble(getSafeString(tok, 4));
							afg.Waypoints[0].Z = parseDouble(getSafeString(tok, 5));
							afg.Waypoints[0].Enabled = parseBool(getSafeString(tok, 6));
							if (isWaypointInvertedPlatform()) afg.Waypoints[0].Y = -afg.Waypoints[0].Y;
							afg.DisplayName = (afg.Abbrev + " " + afg.Name).Trim();
							dat.Flightgroups.Add(afg);
						}
					}
					else if (section == 6)
					{
						string[] tok = splitTrim(s, '=');
						string key = getSafeString(tok, 0);
						string value = getSafeString(tok, 1);
						tok = value.Split(',');

						string[] panelNames = getXwingPanelNames();

						int index = -1;
						for (int j = 0; j < panelNames.Length; j++) if (stringEqual(key, panelNames[j])) index = j;

						if (index == -1)
						{
							popupError($"PageType panel name {key} doesn't exist");
							return false;
						}
						if (tok.Length != 5)
						{
							popupError($"Not enough items in PageType panel {key}, expects 5");
							return false;
						}

						var item = xwingCurTemplate.Items[index];
						item.Left = (short)parseInt(getSafeString(tok, 0));
						item.Top = (short)parseInt(getSafeString(tok, 1));
						item.Right = (short)parseInt(getSafeString(tok, 2));
						item.Bottom = (short)parseInt(getSafeString(tok, 3));
						item.IsVisible = parseBool(getSafeString(tok, 4));
					}
				}
			}

			// If no section was encountered.
			if (section == 0) return false;

			return true;
		}

		/// <summary>Merges externally loaded import data into the editor frontend.</summary>
		/// <remarks>This is not a mission load.</remarks>
		void mergeImportData(ImportData dat)
		{
			var brf = dat.Briefings[0];
			if (isXwing && hasExportXwing() && dat.HasSection(ImportData.Section.PageTypes))
			{
				if (dat.XwingPageTemplates.Count > 0)
				{
					var core = getXwingCoreBriefing();

					core.Templates.Clear();
					core.Templates.AddRange(dat.XwingPageTemplates);

					// Don't add or remove any pages.  Just make sure the indices are valid.
					foreach (var pg in core.Pages)
					{
						pg.Waypoint = (short)clamp(pg.Waypoint, 0, core.MaxWaypoints - 1);
						pg.PageType = (short)clamp(pg.PageType, 0, core.Templates.Count - 1);
					}
				}
			}

			if ((isXvt || isXwa) && dat.HasSection(ImportData.Section.Visibility))
			{
				lstTeams.ClearSelected();
				for (int i = 0; i < brf.ViewedByTeam.Length; i++) lstTeams.SetSelected(i, brf.ViewedByTeam[i]);
			}

			int strCount = Math.Min(_strings.Count, brf.Strings.Length);
			if (hasExportStrings())
			{
				if (dat.HasSection(ImportData.Section.Strings)) for (int i = 0; i < strCount; i++) _strings[i] = brf.Strings[i];
				if (dat.HasSection(ImportData.Section.Tags)) for (int i = 0; i < strCount; i++) _tags[i] = brf.Tags[i];
			}

			if (isWaypointInvertedPlatform()) dat.InvertWaypoints(false);

			// For XWA, when importing from non-XWA platforms, generate icons on the map derived from the
			// flightgroup briefing waypoints.
			if (hasExportEvents() && dat.HasSection(ImportData.Section.Events))
			{
				clearEvents();
				if (isXwa && hasExportFlightgroups() && dat.HasFlightgroups())
				{
					int wp = 0;
					if (dat.XwingPageTemplates.Count > 0) wp = dat.XwingPageWaypoint;

					List<AbstractEvent> icon = new List<AbstractEvent>();
					List<AbstractEvent> move = new List<AbstractEvent>();
					Dictionary<int, int> fgMap = new Dictionary<int, int>();
					short iconIndex = 0;
					for (int i = 0; i < dat.Flightgroups.Count; i++)
					{
						var afg = dat.Flightgroups[i];
						if (!afg.Waypoints[wp].Enabled) continue;

						int craftType = afg.CraftType;
						if (dat.IsTextImport)
						{
							craftType = 1;
							for (int j = 0; j < _craftAbbrev.Length; j++)
							{
								if (stringEqual(_craftAbbrev[j], afg.Abbrev))
								{
									craftType = j;
									break;
								}
							}
						}

						// Since not all icons are added, need to resolve what the FG tags will point to.
						fgMap.Add(i, iconIndex);

						icon.Add(new AbstractEvent(0, AbstractEventType.XwaSetIcon, iconIndex, (short)craftType, (short)afg.CraftIff));
						move.Add(new AbstractEvent(0, AbstractEventType.XwaMoveIcon, iconIndex, afg.Waypoints[wp].RawX, afg.Waypoints[wp].RawY));
						iconIndex++;
					}

					_events.AddRange(icon);
					_events.AddRange(move);

					// Update tags to point to the icons.
					foreach (var evt in brf.Events)
					{
						if (!evt.IsFgTag) continue;

						int k = evt.Params[0];
						fgMap.TryGetValue(k, out int v);
						evt.Params[0] = (short)v;
					}
				}

				// Filter to remove incompatible events.
				int pos = 0;
				while (pos < brf.Events.Count)
				{
					if (!isCompatiblePlatformEvent(brf.Events[pos].Event)) brf.Events.RemoveAt(pos);
					else pos++;
				}

				// Add everything that's left in the list.
				_events.AddRange(brf.Events);

				int duration = brf.RunningTime;
				if (duration == 0 && _events.Count > 0) duration = _events[_events.Count - 1].Time + getMinimumDuration();
				if (duration < getMinimumDuration()) duration = getMinimumDuration();
				setDuration(duration, false);
			}

			if (isXwing)
			{
				// Load in the flightgroups.  Must have at least one loaded to proceed.
				if (hasExportFlightgroups() && dat.HasFlightgroups())
				{
					// Remove as many existing flightgroups as possible, but the container isn't allowed to have zero items.
					while (_flightgroups.Count > 1) deleteXwingFlightgroup(0);

					// Import all the new flightgroups.
					int wp = getFlightgroupWaypoint();
					foreach (var afg in dat.Flightgroups)
					{
						// Copy the waypoint we need to the correct position.
						if (wp != 0) afg.Waypoints[wp] = afg.Waypoints[0];

						if (afg.Waypoints[wp].Enabled) insertXwingFlightgroup(afg, -1);
					}

					// Now the list has multiple items, we can go back and delete the last original existing item.
					deleteXwingFlightgroup(0);
				}

				// Since the event list changed, make sure the page type information is accurate.
				// NOTE: Previously this attempted to merge entire briefings so the pages needed to be
				// fully rebuilt, but full merging was removed since it caused too many problems with pages.
				rebuildXwingBriefingList();
			}

			_eventSpaceUsed = calculateEventSpace(_events);
			updateEventSpace(0);
		}

		/// <summary>Finalizes the data loaded from a binary mission file, performing any necessary conversions.</summary>
		void finalizeImportData(ImportData dat, Settings.Platform fromPlatform, int fromTickRate)
		{
			if (_platform == fromPlatform) return;

			string[] destIff = getIffExportNames(_platform);
			string[] srcIff = getIffExportNames(fromPlatform);

			foreach (var afg in dat.Flightgroups)
			{
				int iff = resolveIff(afg.CraftType, afg.CraftIff, fromPlatform);
				afg.CraftIff = getIffImportValue(destIff, srcIff, iff);
				afg.CraftType = IconConvert.Convert(afg.CraftType, fromPlatform, _platform);
			}

			if (isWaypointInvertedPlatform(fromPlatform)) dat.InvertWaypoints(true);

			float timeScale = (float)_ticksPerSecond / fromTickRate;
			foreach (var brf in dat.Briefings)
			{
				brf.RunningTime = (short)(brf.RunningTime * timeScale);
				foreach (var evt in brf.Events) evt.Time = (short)clamp((int)(evt.Time * timeScale), 0, MaxEventTime);
			}
		}

		bool loadImportDataFromXwing(string filename, ImportData dat)
		{
			try
			{
				Platform.Xwing.Mission mission = new Platform.Xwing.Mission(filename);
				dat.XwingPageTemplates = mission.Briefing.Templates;
				dat.Flightgroups = importAllFlightgroup(mission.FlightGroupsBriefing, Platform.Xwing.Strings.CraftAbbrv);
				dat.Briefings = importXwing(mission.Briefing);
				dat.XwingPageWaypoint = mission.Briefing.Pages[0].Waypoint;
				finalizeImportData(dat, Settings.Platform.XWING, 10);
				return true;
			}
			catch { }
			return false;
		}

		bool loadImportDataFromTie(string filename, ImportData dat)
		{
			try
			{
				Platform.Tie.Mission mission = new Platform.Tie.Mission(filename);
				dat.Flightgroups = importAllFlightgroup(mission.FlightGroups, Platform.Tie.Strings.CraftAbbrv);
				dat.Briefings = importTie(mission.Briefing);
				finalizeImportData(dat, Settings.Platform.TIE, 12);
				return true;
			}
			catch { }
			return false;
		}

		bool loadImportDataFromXvt(string filename, ImportData dat)
		{
			try
			{
				Platform.Xvt.Mission mission = new Platform.Xvt.Mission(filename);
				dat.Flightgroups = importAllFlightgroup(mission.FlightGroups, Platform.Xvt.Strings.CraftAbbrv);
				dat.Briefings = importXvt(mission.Briefings);
				resolvePlayerNumbers(dat.Flightgroups);
				finalizeImportData(dat, Settings.Platform.XvT, 24);
				return true;
			}
			catch { }
			return false;
		}

		bool loadImportDataFromXwa(string filename, ImportData dat)
		{
			try
			{
				Platform.Xwa.Mission mission = new Platform.Xwa.Mission(filename);
				// No BRF flightgroups in XWA.
				dat.Briefings = importXwa(mission.Briefings);
				finalizeImportData(dat, Settings.Platform.XWA, 24);
				return true;
			}
			catch { }
			return false;
		}

		bool loadImportDataFromFile(string filename, ImportData dat)
		{
			bool result = false;
			if (filename.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
			{
				using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader sr = new StreamReader(fs))
					{
						string s = sr.ReadToEnd();
						result = loadImportDataFromText(s, true, dat);
						if (!result) popupError("File did not contain any valid import data.");
					}
				}
			}
			else if (filename.EndsWith(".xwi", StringComparison.OrdinalIgnoreCase)) result = loadImportDataFromXwing(filename, dat);
			else if (filename.EndsWith(".brf", StringComparison.OrdinalIgnoreCase))
			{
				filename = filename.Remove(filename.Length - 4) + ".xwi";
				result = loadImportDataFromXwing(filename, dat);
			}
			else
			{
				// Determine platform from file.
				short platform = 0;
				try
				{
					using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
					{
						byte[] data = new byte[2];
						fs.Read(data, 0, 2);
						platform = BitConverter.ToInt16(data, 0);
					}
				}
				catch { return false; }

				if (platform == -1) result = loadImportDataFromTie(filename, dat);
				else if (platform == 12 || platform == 14) result = loadImportDataFromXvt(filename, dat);
				else if (platform == 18) result = loadImportDataFromXwa(filename, dat);
				else popupError("File is not a valid mission.");
			}
			return result;
		}
		
		AbstractBriefing backupManagerStrings()
		{
			if (!isXwing) return null;

			return new AbstractBriefing(_briefingList[0]);
		}

		void restoreManagerStrings(AbstractBriefing backup)
		{
			if (backup == null || !isXwing) return;

			_briefingList[0].Strings = backup.Strings;
			_briefingList[0].Tags = backup.Tags;
			for (int i = 1; i < _briefingList.Count; i++) _briefingList[i].EraseStrings();
		}
		#endregion Briefing options tab
		
		#region Xwing options tab
		void refreshXwingTab()
		{
			if (!isXwing)
			{
				grpXwingPageTemplates.Enabled = false;
				grpXwingPages.Enabled = false;
				grpXwingNewPage.Enabled = false;
				grpXwing.Enabled = false;
				return;
			}

			bool temp = _loading;
			_loading = true;

			// The list of pages, same as the ones that appear in the bottom left corner of the form.
			lstXwingPages.Items.Clear();
			lstXwingPages.Items.AddRange(getXwingBriefings());

			refreshXwingTabPageTemplates();
			refreshXwingTabPagePanels();
			applyPanelConfigToFrontEnd();

			Platform.Xwing.Briefing core = getXwingCoreBriefing();
			safeSetNumeric(numXwingMaxWaypoint, core.MaxWaypoints);
			numXwingWaypointSet.Value = 1;
			numXwingWaypointSet.Maximum = (int)numXwingMaxWaypoint.Value;
			chkXwingSurfaceMission.Checked = core.MissionLocation;

			_loading = temp;

			// This is outside loading so that the cbo pagetype updates.
			lstXwingPages.SelectedIndex = 0;

			refreshXwingTabStrings();
			refreshXwingTabButtons();
			safeSetCbo(cboXwingTitle, detectXwingTitle());
		}

		void refreshXwingTabButtons()
		{
			cmdXwingPageTypeAdd.Enabled = (lstXwingPageTypes.Items.Count < 4);
			cmdXwingPageTypeDel.Enabled = (lstXwingPageTypes.Items.Count > 2);
			cmdXwingPageUp.Enabled = (lstXwingPages.SelectedIndex > 0);
			cmdXwingPageDown.Enabled = (lstXwingPages.SelectedIndex < lstXwingPages.Items.Count - 1);
			cmdXwingPageDelete.Enabled = (lstXwingPages.Items.Count > 1);
			grpXwingNewPage.Enabled = (lstXwingPages.Items.Count < 8);
		}

		/// <summary>Fully refreshes the dropdown and list of page types.  Attempts to preserve the current selection.</summary>
		void refreshXwingTabPageTemplates()
		{
			bool temp = _loading;
			_loading = true;

			int cboIndex = cboXwingPageTypes.SelectedIndex;
			int lstIndex = lstXwingPageTypes.SelectedIndex;

			cboXwingPageTypes.Items.Clear();
			lstXwingPageTypes.Items.Clear();

			var core = getXwingCoreBriefing();
			int typeCount = core.Templates.Count;
			for (int i = 0; i < typeCount; i++)
			{
				string s = "#" + (i + 1) + ": " + core.Templates[i].GetPageDesc();
				int useCount = 0;
				foreach (var pg in core.Pages) if (pg.PageType == i) useCount++;

				cboXwingPageTypes.Items.Add(s);
				if (useCount > 0) s += $"  ({useCount} page{(useCount > 1 ? "s" : "")})";
				lstXwingPageTypes.Items.Add(s);
			}

			if (cboIndex < 0 || cboIndex >= typeCount) cboIndex = 0;
			if (lstIndex < 0 || lstIndex >= typeCount) lstIndex = 0;

			cboXwingPageTypes.SelectedIndex = cboIndex;
			lstXwingPageTypes.SelectedIndex = lstIndex;

			_loading = temp;
		}

		void refreshXwingTabPagePanels()
		{
			bool temp = _loading;
			_loading = true;

			int lstIndex = Math.Max(0, lstXwingPagePanels.SelectedIndex);
			refreshPanelListBox(lstXwingPagePanels, getXwingTabSelectedPageType(), false);
			lstXwingPagePanels.SelectedIndex = lstIndex;

			_loading = temp;
		}

		Platform.Xwing.PageTemplate getXwingTabSelectedPageType()
		{
			var data = getXwingCoreBriefing();
			int pageIndex = lstXwingPageTypes.SelectedIndex;
			if (pageIndex < 0 || pageIndex >= data.Templates.Count)
			{
				pageIndex = 0;
				bool temp = _loading;
				_loading = true;
				lstXwingPageTypes.SelectedIndex = 0;
				_loading = temp;
			}
			return data.Templates[pageIndex];
		}

		Platform.Xwing.PagePanel getXwingTabSelectedPanel()
		{
			var template = getXwingTabSelectedPageType();
			int panIndex = Math.Max(0, lstXwingPagePanels.SelectedIndex);
			return template.Items[panIndex];
		}

		void refreshXwingTabStrings()
		{
			bool temp = _loading;
			_loading = true;

			int caption = Math.Max(0, cboXwingCaption.SelectedIndex);
			int title = Math.Max(0, cboXwingTitle.SelectedIndex);
			cboXwingCaption.Items.Clear();
			cboXwingTitle.Items.Clear();
			for (int i = 0; i < _strings.Count; i++)
			{
				cboXwingCaption.Items.Add(_strings[i]);
				cboXwingTitle.Items.Add(_strings[i]);
			}
			safeSetCbo(cboXwingCaption, caption);
			safeSetCbo(cboXwingTitle, title);

			_loading = temp;
		}

		void swapXwingPage(int sourceIndex, int direction)
		{
			int destIndex = sourceIndex + direction;
			var data = getXwingCoreBriefing();
			int count = data.Pages.Count;
			if (sourceIndex < 0 || sourceIndex >= count || destIndex < 0 || destIndex >= count) return;

			markDirty();

			// Before we swap, push any changes to the active page.
			exportActiveBriefing();

			// Swap the pages directly shared by the frontend.
			// Ensure the strings and tags are accessible from the first item.
			string[] strings = _briefingList[0].Strings;
			string[] tags = _briefingList[0].Tags;

			var tbrf = _briefingList[sourceIndex];
			_briefingList[sourceIndex] = _briefingList[destIndex];
			_briefingList[destIndex] = tbrf;

			_briefingList[0].Strings = strings;
			_briefingList[0].Tags = tags;

			// The XWING platform requires direct access to additional page information that isn't abstracted
			// through the frontend layers.  Swap the actual pages.
			var temp = data.Pages[sourceIndex];
			data.Pages[sourceIndex] = data.Pages[destIndex];
			data.Pages[destIndex] = temp;

			// Repopulate the briefing selector, and the list of pages.
			rebuildXwingBriefingList();
			lstXwingPages.SelectedIndex = destIndex;

			// For simplicity, reset to the first briefing.
			selectBriefing(0, true);

			refreshXwingTabButtons();
		}

		/// <summary>Searches the existing briefing caption strings to find a specific match, or generates a new string if none found.</summary>
		/// <returns>An index to the string, or -1 if unable to create.</returns>
		int getOrCreateString(List<string> container, string s)
		{
			int result = -1;
			for (int i = 0; i < container.Count; i++)
			{
				string ts = container[i];
				if (stringEqual(ts, s)) return i;
				else if (result == -1 && string.IsNullOrWhiteSpace(ts)) result = i;
			}

			// We didn't find an existing string.  If an empty slot was found, assign it.
			if (result >= 0) container[result] = s;

			return result;
		}
		
		/// <summary>Searches all XWING pages to find the first title event, and returns its caption string index.</summary>
		int detectXwingTitle()
		{
			for (int i = 0; i < _briefingList.Count; i++)
			{
				var events = (i == _currentBriefingIndex ? _events : _briefingList[i].Events);
				foreach (var evt in events) if (evt.Event == AbstractEventType.TitleText) return evt.Params[0];
			}
			return 0;
		}
		#endregion Xwing options tab

		#region Main form events
		private void BriefingForm2_Resize(object sender, EventArgs e)
		{
			// When the form inits, there's a resize to hide the designer controls, before anything has been loaded.
			if (!_isFinishedLoading) return;

			// This is called on any resize operation (minimize, restore, or manual resize).
			suspendLayouts(this);
			resizeTabs(ClientRectangle.Width, ClientRectangle.Height);
			resumeLayouts(this);

			refreshTimeline();
		}
		/// <remarks>This is called when a manual resize or window move is finished.</remarks>
		private void BriefingForm2_ResizeEnd(object sender, EventArgs e) => updateFormSize(false);
		private void BriefingForm2_ExternalResize(object sender, EventArgs e) => updateFormSize(true);
		private void BriefingForm2_FormClosed(object sender, FormClosedEventArgs e)
		{
			cmdFF.Image?.Dispose();
			cmdPlay.Image?.Dispose();
			cmdPause.Image?.Dispose();
			cmdStop.Image?.Dispose();
			cmdPrev.Image?.Dispose();
			cmdNext.Image?.Dispose();
			cmdMoveTagUp.Image?.Dispose();
			cmdMoveTagDown.Image?.Dispose();
			cmdMoveStringUp.Image?.Dispose();
			cmdMoveStringDown.Image?.Dispose();
			cmdEventListGroupUp.Image?.Dispose();
			cmdEventListGroupDown.Image?.Dispose();
			cmdEventListShiftUp.Image?.Dispose();
			cmdEventListShiftDown.Image?.Dispose();
			cmdXwingPageUp.Image?.Dispose();
			cmdXwingPageDown.Image?.Dispose();

			foreach (Button b in _moveButtons) b?.Image?.Dispose();

			pctBrief.Image?.Dispose();
			pctTimeline.Image?.Dispose();
			_titleCanvas?.Dispose();
			_mapCanvas?.Dispose();
			_captionCanvas?.Dispose();
			_combinedCanvas?.Dispose();
			_xwingPanel3Canvas?.Dispose();
			_xwingPanel4Canvas?.Dispose();

			_font?.Dispose();
			_tagFont?.Dispose();
			_fontAltCaption?.Dispose();
			_fontAltTag?.Dispose();

			_statusFont?.Dispose();
			_timelineFont?.Dispose();
			_regionFont?.Dispose();

			freeCaptionSounds();
			_scaler?.Dispose();

			foreach (var elem in _mapTextTags) elem?.ClearTextTagCache(true);

			foreach (var data in _craftIconImages) data.Icon?.Dispose();

			foreach (var item in _bitmapCache) item.Value?.Dispose();

			_craftIconImages.Clear();
			_bitmapCache.Clear();

			if (_craftFlatIconImages != null)
			{
				foreach (var data in _craftFlatIconImages) data.Icon?.Dispose();
				_craftFlatIconImages.Clear();
			}
		}
		private void BriefingForm2_FormClosing(object sender, FormClosingEventArgs e)
		{
			Save();
			saveConfig();

			_pendingRedraw = RefreshFlags.None;

			tmrBrief.Stop();
			tmrBrief.Tick -= tmrBrief_Tick;
			_dataChangeCallback = null;
		}
		private void BriefingForm2_Activated(object sender, EventArgs e)
		{
			// If form focus is changed, it could result in a frozen state, interfering with the ability to
			// select or deselect items on the map or timeline.  Clear the states when returning, just in case.
			_keyStateCtrl = false;
			_keyStateShift = false;
		}
		private void BriefingForm2_KeyDown(object sender, KeyEventArgs e)
		{
			_keyStateCtrl = e.Control;
			_keyStateShift = e.Shift;

			var type = ActiveControl.GetType();
			if (type == typeof(NumericUpDown) || type == typeof(TextBox) || type == typeof(ComboBox))
			{
				// For a TextBox, this prevents entering a newline.
				// For a NumericUpDown, this prevents the operating system beep when pressing enter.
				if (e.KeyCode == Keys.Enter)
				{
					// Allow newline to be entered in this control.
					if (ActiveControl == txtBriefImport) return;

					if (ActiveControl == txtMainEditString || (ActiveControl == txtEditString && _editStringType == BriefingString.String))
					{
						// If editing a caption string, insert the newline character used in-game.
						if (_keyStateShift) SendKeys.Send("$");
					}
					else
					{
						// Allows NumericUpDown to trigger their ValueChanged events without requiring the user to leave
						// the control or use the arrow buttons.
						ActiveControl.Focus();
						Validate();
					}
					e.Handled = true;
					e.SuppressKeyPress = true;
				}
				// All keys must be sent these controls.
				return;
			}

			#if DEBUG && SCREENSHOT
			// This dumps the unscaled canvas to the game installation folder.  Useful for comparing screenshots for accuracy.
			// Screenshots in game can saved by pressing Alt+O
			if (e.Alt && e.KeyCode == Keys.O)
			{
				try
				{
					string suffix = "";
					if (isXwing)
						suffix = isXwingHighDef() ? "_high" : "_low";

					string filename = Path.Combine(CraftDataManager.GetInstance().GetInstallPath(), "_yogeme_canvas" + suffix + ".png");
					_combinedCanvas.Save(filename);
				}
				catch { }
				return;
			}
			#endif

			if (e.KeyCode == Keys.F1)
			{
				displayHelp();
				return;
			}

			if (getDirectionalMoveButtonIndex(ActiveControl) >= 0)
			{
				// Button pressed while mouseover one of the directional controls.
				refreshDirectionalPreview(ActiveControl, _keyStateCtrl);
				return;
			}

			if (_panelMode)
			{
				// Edges (4 to 7): Top, Bottom, Left, Right
				int x = 0, y = 0, edge = _panelModeEdge;
				switch (e.KeyCode)
				{
					case Keys.Escape:
					case Keys.P:
						setPanelMode(false);
						break;

					case Keys.W: _panelModeEdge = 4; break;
					case Keys.S: _panelModeEdge = 5; break;
					case Keys.A: _panelModeEdge = 6; break;
					case Keys.D: _panelModeEdge = 7; break;
					case Keys.Space: _panelModeEdge = 9; break;  // Move

					case Keys.Left:
					case Keys.Right:
						x = (_keyStateShift ? 1 : (_keyStateCtrl ? 4 : 2) );
						if (e.KeyCode == Keys.Left) x = -x;
						break;

					case Keys.Up:
					case Keys.Down:
						y = (_keyStateShift ? 1 : (_keyStateCtrl ? 4 : 2) );
						if (e.KeyCode == Keys.Up) y = -y;
						break;
				}
				if (x != 0 || y != 0)
				{
					if (_panelModeEdge == 9) moveSelectedPanel(x, y, false);
					else resizeSelectedPanel(x, y, false);
				}
				if (edge != _panelModeEdge) refreshCanvas(RefreshFlags.XwingPanels);
				return;
			}

			if (ActiveControl == pctBrief)
			{
				if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
				{
					int x = getMoveIncrementX();
					if (_keyStateShift) x = getPixelRawX();
					else if (_keyStateCtrl) x *= 4;
					if (e.KeyCode == Keys.Left) x = -x;
					moveSelectedItems(x, 0, false);
					refreshEditControls();
					refreshMap();
				}
				else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
				{
					int y = getMoveIncrementY();
					if (_keyStateShift) y = getPixelRawY();
					else if (_keyStateCtrl) y *= 4;
					if (e.KeyCode == Keys.Up) y = -y;
					moveSelectedItems(0, y, false);
					refreshEditControls();
					refreshMap();
				}
				else if (e.KeyCode == Keys.P)
				{
					if (isXwa) setPathMode(!_pathMode);
					else if (isXwing) setPanelMode(!_panelMode);
				}
				else if (_pathMode)
				{
					if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9) selectPathNode(e.KeyCode - Keys.D1, true);
					else
					{
						int rotation = 0, count = 0;
						bool first = false;
						switch (e.KeyCode)
						{
							case Keys.OemOpenBrackets: rotation = -1; break;
							case Keys.OemCloseBrackets: rotation = 1; break;
							case Keys.Oemplus: case Keys.Add: count = 1; break;
							case Keys.OemMinus: case Keys.Subtract: count = -1; break;
							case Keys.OemBackslash: first = true; break;
						}
						if (rotation != 0 || count != 0 || first)
						{
							foreach (var so in _selectedObjects)
							{
								if (!isValidPathNode(so)) continue;

								var node = _pathNodes[so.Index];
								node.Steps = Math.Max(1, node.Steps + count);
								node.Rotation += rotation;
								if (node.Rotation < 0) node.Rotation = 3;
								else if (node.Rotation > 3) node.Rotation = 0;
								node.FirstRotation ^= first;
							}
							refreshPathList(-1);
							applyPreviewFromNodes();
						}
					}
				}
				else
				{
					if (e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad9)
					{
						if (isEditingEventType(AbstractEventType.XwaMoveIcon)) generateDirectionalIconMovement(_moveButtons[e.KeyCode - Keys.NumPad1], _keyStateCtrl);
					}
					else if (e.KeyCode == Keys.OemOpenBrackets || e.KeyCode == Keys.OemCloseBrackets)
					{
						int step = e.KeyCode == Keys.OemOpenBrackets ? 1 : -1;
						if (_tempEvent == AbstractEventType.XwaSetIcon || _tempEvent == AbstractEventType.XwaRotateIcon)
						{
							int newRot = cboRotation.SelectedIndex + step;
							if (newRot >= 4) newRot = 0;
							else if (newRot < 0) newRot = 3;
							cboRotation.SelectedIndex = newRot;
						}
						else if (isEditingEventType(AbstractEventType.XwaRotateIcon) || isEditingEventType(AbstractEventType.XwaMoveIcon))
						{
							setRotationForMoveIcon(getEventIndexByUid(_lastSelectedMoveIconUid), false, step);
							var elem = getShadowIconByUid(_lastSelectedMoveIconUid);
							if (elem != null) loadCbo(cboRotation, elem.IconRotation);
						}
					}
					else if (e.KeyCode == Keys.D) detectIconMoveDistance();
				}
			}
			else if (ActiveControl == pctTimeline)
			{
				switch (e.KeyCode)
				{
					case Keys.Up:
						if (_keyStateCtrl) moveEventsInTime(-1);
						break;
					case Keys.Down:
						if (_keyStateCtrl) moveEventsInTime(1);
						break;
					case Keys.Left:
						if (_keyStateCtrl)
						{
							moveEventsAcrossTime(-1, true);
							jumpToTime(hsbTimer.Value - 1, true);
						}
						else if (_keyStateShift) gotoNextEventTime(hsbTimer.Value, -1);
						else if (hsbTimer.Value > 0) jumpToTime(hsbTimer.Value - 1, true);
						break;
					case Keys.Right:
						if (_keyStateCtrl)
						{
							moveEventsAcrossTime(1, true);
							jumpToTime(hsbTimer.Value + 1, true);
						}
						else if (_keyStateShift) gotoNextEventTime(hsbTimer.Value, 1);
						else if (hsbTimer.Value < getMaximumScrollTime()) jumpToTime(hsbTimer.Value + 1, true);
						break;
				}
			}
			else if (ActiveControl == lstPath)
			{
				if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9) selectPathNode(e.KeyCode - Keys.D1, true);
				else if (e.KeyCode == Keys.Delete)
				{
					deletePathNode(lstPath.SelectedIndex, true);
					refreshMap();
				}
			}
			else if (ActiveControl == lstMainStrings || ActiveControl == lstMainTags)
			{
				// Set focus to edit text.
				BriefingString bstr = (ActiveControl == lstMainStrings ? BriefingString.String : BriefingString.Tag);
				int index = ((ListBox)ActiveControl).SelectedIndex;

				if (e.KeyCode == Keys.Return || e.KeyCode == Keys.F2)
				{
					if (bstr == BriefingString.String) lstMainStrings_MouseDoubleClick(null, null);
					else lstMainTags_MouseDoubleClick(null, null);
				}

				if (e.KeyCode == Keys.X) cutString(bstr, index);
				else if (e.KeyCode == Keys.C) copyString(bstr, index);
				else if (e.KeyCode == Keys.V) pasteString(bstr, index);
			}

			if (ActiveControl == pctBrief || ActiveControl == pctTimeline || ActiveControl == lstEvents)
			{
				if (_keyStateCtrl)
				{
					if (e.KeyCode == Keys.X) cutEvents();
					else if (e.KeyCode == Keys.C) copyEvents();
					else if (e.KeyCode == Keys.V) pasteEvents();
					else if (e.KeyCode == Keys.K) copyIcons();
				}

				if (e.KeyCode == Keys.Delete) deleteSelectedItems();
				else if (e.KeyCode == Keys.I) expandIconSelectionByType();
				else if (e.KeyCode == Keys.G) gotoContextEvent(-1);
				else if (e.KeyCode == Keys.Escape)
				{
					if (_panelMode) setPanelMode(false);
					else if (_tempEvent != AbstractEventType.None) endTempEvent(false);
					else if (_selectedObjects.Count > 0) deselectObjects();
					else if (_freeLookMode) setFreeLookMode(false);
				}
			}

			if ((ActiveControl == pctBrief || ActiveControl == pctTimeline) && !_pathMode)
			{
				if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9) gotoPage((e.KeyCode - Keys.D1) + 1);

				if (e.KeyCode >= Keys.F5 && e.KeyCode <= Keys.F8)
				{
					int index = e.KeyCode - Keys.F5;

					if (_keyStateShift) _bookmarkTimes[index] = hsbTimer.Value;
					else jumpToTime(_bookmarkTimes[index], true);
				}

				switch (e.KeyCode)
				{
					case Keys.Space:
						if (isPlaybackActive) pauseBriefing();
						else
						{
							int speed = 1;
							if (_keyStateShift) speed *= 2;
							if (_keyStateCtrl) speed *= 4;

							setFreeLookMode(false);
							setPlaybackSpeed(speed);
							playBriefing(false);
						}
						break;

					case Keys.F:
						setFreeLookMode(false);
						if (!isPlaybackActive)
						{
							setPlaybackSpeed(1);
							playBriefing(false);
						}
						else setPlaybackSpeed(_playbackSpeed * 2);
						break;
					case Keys.PageUp:
						gotoNextPage(hsbTimer.Value, -1);
						break;
					case Keys.PageDown:
						gotoNextPage(hsbTimer.Value, 1);
						break;
					case Keys.Home:
						jumpToTime(0, true);
						break;
					case Keys.End:
						if (e.Modifiers == Keys.Control) jumpToTime(_briefingDuration, true);
						else jumpToTime(getHighestTime(), true);
						break;
					case Keys.Enter:
						if (_tempEvent != AbstractEventType.None) insertTempEvent();
						else gotoSelectedEvent();
						break;
					case Keys.E:
						gotoNextTimelinePage(1, _keyStateShift | _keyStateCtrl);
						break;
					case Keys.R:
						gotoNextTimelinePage(-1, _keyStateShift | _keyStateCtrl);
						break;
					case Keys.T:
						if (!_keyStateCtrl) gotoNextEventTime(hsbTimer.Value, 1);
						break;
					case Keys.Y:
						if (!_keyStateCtrl) gotoNextEventTime(hsbTimer.Value, -1);
						break;
					case Keys.U:
						selectEventByUid(_lastEditEventUid);
						gotoSelectedEvent();
						break;
					case Keys.L:
						if (popupConfirm("Change briefing duration to current position?"))
						{
							beginUndoFrame(true);
							setDuration(hsbTimer.Value, true);
						}
						break;
					case Keys.OemMinus:
					case Keys.Subtract:
						jumpToTime(hsbTimer.Value - 1, true);
						break;
					case Keys.Oemplus:
					case Keys.Add:
						jumpToTime(hsbTimer.Value + 1, true);
						break;
					case Keys.Back:
						clearIcons();
						break;
				}
			}

			if (e.KeyCode == Keys.Z && _keyStateCtrl)
			{
				performUndo();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Y && _keyStateCtrl)
			{
				performRedo();
				e.Handled = true;
			}
		}
		private void BriefingForm2_KeyUp(object sender, KeyEventArgs e)
		{
			_keyStateCtrl = e.Control;
			_keyStateShift = e.Shift;

			if (getDirectionalMoveButtonIndex(ActiveControl) >= 0)
			{
				// Button pressed while mouseover one of the directional controls.
				refreshDirectionalPreview(ActiveControl, _keyStateCtrl);
				return;
			}
		}

		private void tabBriefing_SelectedIndexChanged(object sender, EventArgs e)
		{
			pauseBriefing();

			// Cancel temp if it's active.
			if (_tempEvent != AbstractEventType.None) endTempEvent(false);
	
			// These mode interfaces and sidebars are exclusive to the map.
			setPathMode(false);
			setPanelMode(false);

			switch ((MainTabIndex)tabBriefing.SelectedIndex)
			{
				case MainTabIndex.BriefingMap:
					// We don't know what the user changed, if anything.
					rebuildBriefing();
					rebuildShadowIcons();
					refreshCanvas(RefreshFlags.All);
					transferSidebarPanels(true);
					transferXwingPanels(true);

					// Reset sidebar based on selection, and repopulate strings.
					_editStringType = BriefingString.None;
					displayEditControls();

					if (isXwing) refreshXwingTempCreateButtons();
					break;

				case MainTabIndex.MainStrings:
					populateMainStrings(-1, -1);
					break;

				case MainTabIndex.EventList:
					refreshEventList(-1);
					applySelectionToEventList();
					transferSidebarPanels(false);
					
					// Reset sidebar based on selection, and repopulate strings.
					_editStringType = BriefingString.None;
					displayEditControls();
					break;

				case MainTabIndex.BriefingOptions:
					populateBriefingOptions();
					break;

				case MainTabIndex.XwingPages:
					transferXwingPanels(false);
					refreshXwingTab();
					break;
			}
		}

		private void cboCurrentBriefing_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboCurrentBriefing.Focused) selectBriefing(cboCurrentBriefing.SelectedIndex, false);
		}

		private void numDurationSec_ValueChanged(object sender, EventArgs e)
		{
			// Update immediately if the user changed the value.
			if (ActiveControl == sender) numDurationSec_Validated(sender, e);
		}
		private void numDurationSec_Validated(object sender, EventArgs e)
		{
			// The user can type decimal values if they want, they'll be reflected in the value.
			int newEndTime = (int)(numDurationSec.Value * _ticksPerSecond);
			if (_briefingDuration == newEndTime) return;

			// If the user is clicking several times, don't create a new undo state for each change.
			// Try to update the existing state instead.
			if (_currentUndoFrame.Count == 1 && _currentUndoFrame[0].OpType == UndoType.Duration)
			{
				setDuration(newEndTime, false);
				_currentUndoFrame[0].Update(newEndTime);
			}
			else
			{
				beginUndoFrame(true);
				setDuration(newEndTime, true);
			}
		}
		#endregion Main form events

		#region Map events
		private void pctBrief_MouseDown(object sender, MouseEventArgs e)
		{
			if (ActiveControl != pctBrief) pctBrief.Focus();

			_mouse1 = e.Location;
			_mouse2 = _mouse1;
			_mouseDragState = MouseDragState.None;

			if (_tempEvent != AbstractEventType.None)
			{
				// Click needs to take focus.  The user could have been editing the string, and the MouseEnter
				// mechanism avoids stealing focus in that situation.
				pctBrief.Focus();

				if (_scaledMapRect.Contains(_mouse1))
				{
					if (e.Button == MouseButtons.Left)
					{
						if (_tempEvent == AbstractEventType.FgTag1)
						{
							// If creating a FG Tag, allow the user to choose a flightgroup by clicking its icon on the map.
							int index = getFlightgroupIndexAtCursor();
							if (index >= 0 && index < cboFGTagItem.Items.Count) cboFGTagItem.SelectedIndex = index;
						}
						else if (_tempEvent == AbstractEventType.TextTag1 || _tempEvent == AbstractEventType.XwaSetIcon || _tempEvent == AbstractEventType.XwaMoveIcon || _tempEvent == AbstractEventType.XwingNewIcon)
						{
							// Since the user manually clicked on the map, automatically enable the checkbox to save them a click
							bool autoPosition = false;
							if (_tempEvent == AbstractEventType.XwaSetIcon)
							{
								if (_keyStateCtrl) cboCraftType.SelectedIndex = 0;

								// For null craft, allow selecting an icon to deactivate.  Otherwise it's a location for a move event.
								if (cboCraftType.SelectedIndex == 0)
								{
									int iconIndex = getFlightgroupIndexAtCursor();
									if (iconIndex >= 0)
									{
										cboIcon.SelectedIndex = iconIndex;
										setEditPositionValues(_mapIcons[iconIndex].X, _mapIcons[iconIndex].Y);
										// Shows the red X over the icon, but does not actually use the position.
										chkAutoMoveIcon.Checked = true;
									}
								}
								else if (!chkAutoMoveIcon.Checked)
								{
									chkAutoMoveIcon.Checked = true;
									autoPosition = true;
								}
							}

							if (_keyStateShift || autoPosition)
							{
								// Set exact position where the user has clicked.
								var pos = convertMousePosToRawPos(_mouse1);
								pos.Y = getInvertedY(pos.Y);
								if (isVisibleChecked(chkMoveSnapGrid)) pos = getGridSnappedPosition(pos);
								safeSetNumeric(numX, pos.X);
								safeSetNumeric(numY, pos.Y);
							}
							else if (_tempInteract.Width > 0)
							{
								// If clicked inside the interact location, initiate a drag state.
								var pos = convertMousePosToPixelPos(_mouse1);
								if (_tempInteract.Contains(pos)) _mouseDragState = MouseDragState.MapObjectClick;
							}
							refreshMap();
						}

						// Handle separately from above, allow clicking an existing icon to change the icon number.
						if (_mouseDragState == MouseDragState.None)
						{
							if (_tempEvent == AbstractEventType.XwaMoveIcon || _tempEvent == AbstractEventType.XwaRotateIcon || _tempEvent == AbstractEventType.XwaShipInfo)
							{
								int index = getFlightgroupIndexAtCursor();
								if (index >= 0)
								{
									// For rotations, reset the rotation to the new icon that was clicked.
									if (index != cboIcon.SelectedIndex && _tempEvent == AbstractEventType.XwaRotateIcon) safeSetCbo(cboRotation, _mapIcons[index].IconRotation);
									safeSetCbo(cboIcon, index);
								}
							}
						}
					}
					else if (e.Button == MouseButtons.Right && _tempEvent == AbstractEventType.MoveMap)
					{
						_mouseDragState = MouseDragState.MapLookClick;
						Cursor = Cursors.SizeAll;
					}
				}
			}
			else if (_panelMode)
			{
				var panels = getMapPanelScaledRects();
				int curSelection = lstPanelMode.SelectedIndex;

				// It's possible for panels to overlap.  If a panel is already selected, stay on the selected
				// panel if the mouse is inside.  The interior area includes the draggable boundary.
				// If the mouse is outside, attempt to find a new panel to select by searching the panels in
				// reverse to match their Z order.
				var current = panels[curSelection];
				int tolerance = getMapPanelDragTolerance();
				current.Inflate(tolerance, tolerance);

				if (!current.Contains(_mouse1))
				{
					for (int i = 4; i >= 0; i--)
					{
						if (!panels[i].Contains(_mouse1)) continue;

						if (curSelection != i)
						{
							selectXwingEditPanel(i);
							return;
						}
						// Break in case multiple panels overlap.
						break;
					}
				}

				// The selection didn't change.  Check how to interact with the selected panel.
				var panel = panels[curSelection];

				// Check for dragging first, by comparing the click location to the panel edges.
				int edgeIndex = getMapPanelDragEdge(_mouse1, panel);
				if (edgeIndex >= 0)
				{
					// Resize operation.
					_panelModeEdge = edgeIndex;
					Cursor = getMapPanelDragCursor(edgeIndex);
					_dragCarry = new Point();
					_mouseDragState = MouseDragState.PanelResizeClick;
				}
				else if (panel.Contains(_mouse1))
				{
					// Move operation.
					_panelModeEdge = 9;
					Cursor = Cursors.SizeAll;
					_dragCarry = new Point();
					_mouseDragState = MouseDragState.PanelMoveClick;
				}
				// Display the selected edge.
				refreshCanvas(RefreshFlags.XwingPanels);
			}
			else if (_scaledMapRect.Contains(_mouse1))
			{
				if (e.Button == MouseButtons.Left)
				{
					if (_pathMode && (_keyStateShift || _keyStateCtrl))
					{
						// In path mode, creating a node takes priority over selection.
						int eventIndex = getWorkingMoveIcon();
						bool defNode = false;
						if (_pathNodes.Count == 0 && eventIndex >= 0)
						{
							// Create an initial path node starting at the selected icon.
							// If a shadow icon was selected, the active icon properties won't be available.
							// Shadow icons do have info for that moment in time, so search for them.
							var shadow = getShadowIconByUid(_lastSelectedMoveIconUid);
							if (shadow != null)
							{
								addPathNode(new Point(shadow.X, shadow.Y), shadow.IconRotation, true);
								defNode = true;
							}
						}
						
						var p = convertMousePosToRawPos(_mouse1);
						if (chkMoveSnapGrid.Checked) p = getGridSnappedPosition(p);
						addPathNode(p, 0, defNode | _keyStateShift);
						refreshPathStats();
						refreshMap();
					}
					else
					{
						if (_activeShipInfoBlock)
						{
							// We can't select anything on the map when this block is active.
							// If the info string is visible and the user clicks on the left side of the canvas, try selecting it.
							Point pos = convertMousePosToPixelPos(_mouse1);
							if (pos.X < _mapCanvas.Width / 2 && _shipInfoStringIndex >= 0) selectEventByUid(_shipInfoStringUid);
							else deselectObjects();
						}
						else
						{
							// Standard map interaction.
							// For tags, just like temp event, allow clicking an icon on the map.
							// If not editing a tag, or nothing found, fall through to selection logic.
							if (isEditingEventType(AbstractEventType.FgTag1) && _keyStateShift)
							{
								int index = getFlightgroupIndexAtCursor();
								if (index >= 0 && index < cboFGTagItem.Items.Count) cboFGTagItem.SelectedIndex = index;
								return;
							}

							// If Ctrl clicking, the user might be trying to toggle selection.
							// In which case, use the bound selection logic rather than move item logic.
							if (!_keyStateCtrl && clickInsideSelectedMapObject()) _mouseDragState = MouseDragState.MapObjectClick;
							else _mouseDragState = MouseDragState.MapBoundClick;
						}
					}
				}
				else if (e.Button == MouseButtons.Middle)
				{
					_mouseDragState = MouseDragState.None;
					if (_pathMode)
					{
						if (_freeLookMode) setFreeLookMode(false);
					}
					else if (_mapShiftMode) restoreShiftModeView();
					else
					{
						int time = getEarliestSelectedTime();
						if (time >= 0 && time != hsbTimer.Value) gotoSelectedEvent();
						else if (_freeLookMode) setFreeLookMode(false);
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					if (_activeShipInfoBlock)
					{
						// The map isn't visible so there's nothing to pan.
						// Instead try to seek to the next event.  If the briefing is properly formed, the next event is probably deactivating the info effect.
						gotoNextEventTime(hsbTimer.Value, 1);
					}
					else
					{
						_mouseDragState = MouseDragState.MapLookClick;
						Cursor = Cursors.SizeAll;
					}
				}
			}
			else if (_scaledCaptionRect.Contains(_mouse1))
			{
				if (e.Button == MouseButtons.Left && _panelStringEnabled[PANEL_CAPTION]) selectEventByUid(_panelStringEventUid[PANEL_CAPTION]);
				else if (e.Button == MouseButtons.Right)
				{
					// Go to next page, or wrap around to beginning if not found.
					if (!gotoNextPage(hsbTimer.Value, 1)) jumpToTime(0, true);
				}
			}
			else if (_scaledTitleRect.Contains(_mouse1) && isTitlePlatform && _panelStringEnabled[PANEL_TITLE]) selectEventByUid(_panelStringEventUid[PANEL_TITLE]);
			else if (isXwing)
			{
				if (_scaledPanel3Rect.Contains(_mouse1) && _panelStringEnabled[PANEL_XWING3]) selectEventByUid(_panelStringEventUid[PANEL_XWING3]);
				else if (_scaledPanel4Rect.Contains(_mouse1) && _panelStringEnabled[PANEL_XWING4]) selectEventByUid(_panelStringEventUid[PANEL_XWING4]);
				else deselectObjects();
			}
		}
		private void pctBrief_MouseMove(object sender, MouseEventArgs e)
		{
			if (ActiveControl != pctBrief) return;

			if (_panelMode)
			{
				// If the mouse is merely hovering, check if it's a scrollable edge to click on, and update the cursor.
				if (_mouseDragState == MouseDragState.None)
				{
					Rectangle[] panels = getMapPanelScaledRects();
					int curSelection = lstPanelMode.SelectedIndex;
					int edgeIndex = getMapPanelDragEdge(e.Location, panels[curSelection]);
					if (edgeIndex >= 0) Cursor = getMapPanelDragCursor(edgeIndex);
					else if (Cursor != Cursors.Default) Cursor = Cursors.Default;
				}
			}

			if (_mouseDragState == MouseDragState.None) return;

			_mouse2 = e.Location;

			// Convert drag delta to map units, first adjusting for the visible map scaling, then the grid itself.
			// The movement won't always conform perfectly to the cursor due to scaling, but the rounding helps with that.
			float mapScale = getMapScale();
			int zoomScaleX = _currentZoom.X;
			int zoomScaleY = _currentZoom.Y;
			if (isXwing && isXwingHighDef())
			{
				zoomScaleX *= 2;
				zoomScaleY *= 2;
			}
			double moveDeltaX = Math.Round(((_mouse2.X - _mouse1.X) / mapScale) * (256.0 / zoomScaleX));
			double moveDeltaY = Math.Round(((_mouse2.Y - _mouse1.Y) / mapScale) * (256.0 / zoomScaleY));
			bool drag = isMouseDrag();

			// NOTE: Click states need to work in sequence with drag, so that one can immediately activate
			// the other and process it when necessary.  So there's no switch() or else-if chain here.

			if (_mouseDragState == MouseDragState.MapBoundClick)
			{
				// Dragging a selection box.  Redraw with the new bounding box.
				refreshMap();
			}

			if (_mouseDragState == MouseDragState.MapLookClick)
			{
				if (_tempEvent == AbstractEventType.None)
				{
					// If editing a MoveMap event, will route the changes through the event instead.
					// Otherwise don't start free look unless drag is confirmed.
					// If free look has already been started, and the user is dragging again, proceed as normal.
					if (!isEditingEventType(AbstractEventType.MoveMap) && !_freeLookMode && drag) setFreeLookMode(true);
				}
				
				if (drag) _mouseDragState = MouseDragState.MapLookDrag;
			}

			if (_mouseDragState == MouseDragState.MapLookDrag)
			{
				if (isDualEditingMoveZoom() && optPanEvent.Checked)
				{
					// Both Move and Zoom events are selected.  Can't go through the controls.
					// Apply panning directly to the event.
					var evt = _events[getSelectedEventType(AbstractEventType.MoveMap).Index];
					evt.Params[0] = (short)(evt.Params[0] + moveDeltaX);
					evt.Params[1] = (short)(evt.Params[1] + moveDeltaY);
					setMoveZoomRectangles(evt.Time);
				}
				else if (isEditingEventType(AbstractEventType.MoveMap) && optPanEvent.Checked)
				{
					// Apply panning to the event through the controls.
					modifyParam(EditParamType.X, (int)numX.Value + (int)moveDeltaX, false);
					modifyParam(EditParamType.Y, (int)numY.Value + (int)moveDeltaY, true);
					setMoveZoomRectangles(_events[_selectedObjects[0].Index].Time);
					refreshEditControls();
				}
				else
				{
					// Adjust map position for everything else, including Free Look.
					_currentPosition.X -= (int)moveDeltaX;
					_currentPosition.Y -= (int)moveDeltaY;

					if (_tempEvent == AbstractEventType.MoveMap)
					{
						setEditPositionValues(_currentPosition);
						setEditScrollbarValues(_currentPosition);
					}
				}

				// Dragging continues where it left off.
				_mouse1 = _mouse2;
				refreshMap();
			}

			if (_mouseDragState == MouseDragState.MapObjectClick && isMouseDrag())
			{
				if (_tempEvent == AbstractEventType.None) beginMoveItems();
				_mouseDragState = MouseDragState.MapObjectDrag;
			}

			if (_mouseDragState == MouseDragState.MapObjectDrag)
			{
				moveSelectedItems((int)moveDeltaX, (int)moveDeltaY, true);
				refreshEditControls();
				_mouse1 = _mouse2;
				refreshMap();
			}

			// For panel move and resize, the calculated delta is for map objects.
			// We only want the pixels.
			if (_mouseDragState == MouseDragState.PanelMoveClick && drag) _mouseDragState = MouseDragState.PanelMoveDrag;

			if (_mouseDragState == MouseDragState.PanelMoveDrag)
			{
				_panelModeEdge = 9;   // Reset the bracket
				int dx = (chkMoveLockX.Checked ? 0 : _mouse2.X - _mouse1.X);
				int dy = (chkMoveLockY.Checked ? 0 : _mouse2.Y - _mouse1.Y);
				moveSelectedPanel(dx, dy, true);
				_mouse1 = _mouse2;
			}

			if (_mouseDragState == MouseDragState.PanelResizeClick && drag) _mouseDragState = MouseDragState.PanelResizeDrag;

			if (_mouseDragState == MouseDragState.PanelResizeDrag)
			{
				resizeSelectedPanel(_mouse2.X - _mouse1.X, _mouse2.Y - _mouse1.Y, true);
				_mouse1 = _mouse2;
			}
		}
		private void pctBrief_MouseUp(object sender, MouseEventArgs e)
		{
			_mouse2 = e.Location;
			if (e.Button == MouseButtons.Left && (_mouseDragState == MouseDragState.MapBoundClick || _mouseDragState == MouseDragState.MapBoundDrag))
			{
				selectMapObjects();
				refreshMap();
			}

			// Failsafe to prevent modified cursor from getting stuck, since conditional checks on mouse state weren't always reliable.
			Cursor = Cursors.Default;
			_mouseDragState = MouseDragState.None;
		}
		private void pctBrief_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			_mouse1 = e.Location;
			if (isXwa && clickInsideSelectedMapObject())
			{
				expandIconSelectionByType();
				_mouseDragState = MouseDragState.DoubleClick;  // Indicates to the mouseup/down events that it's not a standard click.
			}
		}
		private void pctBrief_MouseWheel(object sender, MouseEventArgs e)
		{
			if (_panelMode) return;

			bool isDual = (isDualEditingMoveZoom() && optPanEvent.Checked);
			bool isEventZoom = (isEditingEventType(AbstractEventType.ZoomMap) && optPanEvent.Checked);
			bool isZoom = (_tempEvent == AbstractEventType.ZoomMap || isDual || isEventZoom);

			if (_tempEvent == AbstractEventType.None && !isZoom) setFreeLookMode(true);
			else if (!isZoom) return;

			int x = 1, y = 1;
			int delta = (e.Delta > 0 ? 1 : -1);

			// Make it faster if already zoomed in.  Clamp to even numbers so it looks nice.
			if (_currentZoom.X >= 32) x = (_currentZoom.X / 16) & 0xFFFE;
			if (_currentZoom.Y >= 32) y = (_currentZoom.Y / 16) & 0xFFFE;

			// Remain consistent with the design pattern of shift=small, control=large movements.
			if (_keyStateCtrl) { x *= 2; y *= 2; }
			if (_keyStateShift) { x = 1; y = 1; }

			if (isDual)
			{
				// Both Move and Zoom events are selected.  Can't go through the controls.
				// Apply zoom directly to the event.
				var evt = _events[getSelectedEventType(AbstractEventType.ZoomMap).Index];
				evt.Params[0] = (short)clamp(evt.Params[0] + (delta * x), 1, 256);
				evt.Params[1] = (short)clamp(evt.Params[1] + (delta * y), 1, 256);
				setMoveZoomRectangles(evt.Time);
			}
			else
			{
				int curX = (isEventZoom ? (int)numX.Value : _currentZoom.X);
				int curY = (isEventZoom ? (int)numY.Value : _currentZoom.Y);
				int newX = clamp(curX + (delta * x), 1, 256);
				int newY = clamp(curY + (delta * y), 1, 256);

				if (isEventZoom)
				{
					modifyParam(EditParamType.X, newX, false);
					modifyParam(EditParamType.Y, newY, true);
					setMoveZoomRectangles(_events[_selectedObjects[0].Index].Time);
					refreshEditControls();
				}
				else
				{
					_currentZoom = new Point(newX, newY);
					if (_tempEvent == AbstractEventType.ZoomMap)
					{
						setEditPositionValues(_currentZoom);
						setEditScrollbarValues(_currentZoom);
					}
				}
			}
			refreshMap();
		}
		private void pctBrief_MouseEnter(object sender, EventArgs e) { if (allowFocusSteal()) pctBrief.Focus(); }
		private void pctBrief_MouseLeave(object sender, EventArgs e) { if (_panelMode && _mouseDragState == MouseDragState.None && Cursor != Cursors.Default) Cursor = Cursors.Default; }
		private void pctBrief_Paint(object sender, PaintEventArgs e)
		{
			// Tried assigning the image to the pct, but there's no control over the interpolation mode.
			// It has to be drawn manually.  Also, there doesn't seem to be a way to prevent base.Paint
			// from drawing, forcing us to paint over it.  So we're not using the image and this control
			// is basically just a panel now.  On the plus side, it does seem to be slightly faster.
			e.Graphics.InterpolationMode = _interpolationMode;
			//e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
			e.Graphics.DrawImage(_combinedCanvas, pctBrief.DisplayRectangle);
		}

		private void chkAnimateTags_CheckedChanged(object sender, EventArgs e)
		{
			refreshTextTags();
			refreshMap();
		}

		private void chkFreeLookInfo_CheckedChanged(object sender, EventArgs e) => refreshMap();
		private void chkShowPlayerNumbers_CheckedChanged(object sender, EventArgs e) => refreshMap();
		private void cboShadowFilter_SelectedIndexChanged(object sender, EventArgs e) => refreshMap();
		private void chkIconShadows_CheckedChanged(object sender, EventArgs e) => refreshMap();

		private void chkHighDef_CheckedChanged(object sender, EventArgs e)
		{
			// This is triggered during load when assigning the config value, but the program state must be valid.
			if (!_isFinishedLoading) return;

			if (isXwing)
			{
				_playbackFramerate = (_config.HighDef ? 9.25f : 10.30f);  // Slower in XW98 than XW94
				setXwingCanvas(chkHighDef.Checked);
				updateFormSize(true);
				// The default zoom levels are different, so the briefing needs to be rebuilt.
				rebuildBriefing();
				// Force the panel locations to update, which also corrects the free look position if necessary.
				refreshPanelModeVisuals();
			}
			else if (isXwa) _playbackFramerate = (chkHighDef.Checked ? 24.00f : 21.30f);  // Faster in XWAU than vanilla
		}

		private void chkXwaEffects_CheckedChanged(object sender, EventArgs e)
		{
			if (!chkXwaEffects.Checked && _mapEffect != MapEffect.None)
			{
				beginMapEffect(MapEffect.None, 0);
				refreshCanvas(RefreshFlags.Effect);
			}
		}
		#endregion Map events

		#region Timeline events
		private void pctTimeline_MouseDown(object sender, MouseEventArgs e)
		{
			if (ActiveControl != pctTimeline) pctTimeline.Focus();

			_mouse1 = e.Location;
			_mouse2 = _mouse1;
			_mouseDragState = MouseDragState.None;

			if (_tempEvent != AbstractEventType.None) return;

			if (e.Button == MouseButtons.Left)
			{
				if (clickInsideSelectedTimelineObject()) _mouseDragState = MouseDragState.TimelineObjectClick;
				else _mouseDragState = MouseDragState.TimelineBoundClick;
			}
			else if (e.Button == MouseButtons.Right)
			{
				pauseBriefing();
				_mouseDragState = MouseDragState.TimelineDrag;
				Cursor = Cursors.SizeAll;
			}
			else if (e.Button == MouseButtons.Middle) gotoSelectedEvent();
		}
		private void pctTimeline_MouseMove(object sender, MouseEventArgs e)
		{
			// Normally this function is only allowed to proceed if the control was granted dedicated focus.
			// But if attempting to manually change the briefing duration, the numeric control still has focus.
			// This allows the timeline to be more interactive if there are events beyond the assigned duration.
			if (ActiveControl != pctTimeline && ActiveControl != numDurationSec) return;

			_mouse2 = e.Location;
			refreshTimelineOffset();

			if (_mouseDragState == MouseDragState.None) return;

			if (_mouseDragState == MouseDragState.TimelineBoundClick && isMouseDrag())
			{
				_mouseDragState = MouseDragState.TimelineBoundDrag;
				refreshTimeline();
			}
			else if (_mouseDragState == MouseDragState.TimelineBoundDrag) refreshTimeline(); // Dragging a selection box.  Redraw with the new bounding box.
			else if (_mouseDragState == MouseDragState.TimelineDrag) scrollTimeline();
			else if (_mouseDragState == MouseDragState.TimelineObjectClick && isMouseDrag())
			{
				beginMoveItems();
				_mouseDragState = MouseDragState.TimelineObjectDrag;
			}
			else if (_mouseDragState == MouseDragState.TimelineObjectDrag)
			{
				if (_keyStateShift) shiftSelectedTimelineItems();
				else moveSelectedTimelineItems();
			}
		}
		private void pctTimeline_MouseUp(object sender, MouseEventArgs e)
		{
			_mouse2 = e.Location;
			if (e.Button == MouseButtons.Left)
			{
				if (_mouseDragState == MouseDragState.TimelineBoundClick || _mouseDragState == MouseDragState.TimelineBoundDrag || _mouseDragState == MouseDragState.TimelineObjectClick)
				{
					selectTimelineObjects();
					refreshMap();
					refreshTimeline();
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				Cursor = Cursors.Default;
				refreshTimeline();
			}
			_mouseDragState = MouseDragState.None;
		}
		private void pctTimeline_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			_mouse1 = e.Location;
			if (isXwa && clickInsideSelectedTimelineObject())
			{
				expandIconSelectionByType();
				_mouseDragState = MouseDragState.DoubleClick;  // Indicates to the mouseup/down events that it's not a standard click.
			}
		}
		private void pctTimeline_MouseWheel(object sender, MouseEventArgs e)
		{
			int amount = 1;
			int delta = (e.Delta > 0 ? 1 : -1);

			// Make it faster if already zoomed in.
			if (_timelineColumnWidth >= 32) amount = _timelineColumnWidth / 16;

			// Remain consistent with the design pattern of shift=small, control=large movements.
			if (_keyStateCtrl) amount *= 2;
			if (_keyStateShift) amount = 1;
			_timelineColumnWidth += delta * amount;

			// Using the font height since it's the simplest metric to determine how much the user might be able to see.
			// Ideally the maximum column size should be enough to hold the majority of text that would possibly be displayed there.
			if (_timelineColumnWidth < (_font.MaxWidth / 3)) _timelineColumnWidth = (_font.MaxWidth / 3);
			else if (_timelineColumnWidth > _timelineFont.Height * 5) _timelineColumnWidth = _timelineFont.Height * 5;
			refreshTimeline();
		}
		private void pctTimeline_MouseEnter(object sender, EventArgs e)
		{
			if (allowFocusSteal()) pctTimeline.Focus();
			lblTimelineOffset.Visible = true;
		}
		private void pctTimeline_MouseLeave(object sender, EventArgs e) => lblTimelineOffset.Visible = false;

		private void vsbTimeline_ValueChanged(object sender, EventArgs e)
		{
			// Focus checking doesn't work, so check if the user changed the value.
			if (_timelineRowScrollPos == vsbTimeline.Value) return;

			_timelineRowScrollPos = vsbTimeline.Value;
			refreshTimeline();
		}
		#endregion Timeline events

		#region Xwing panel mode events
		private void lstPanelMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			selectXwingEditPanel(lstPanelMode.SelectedIndex);
		}

		private void chkPanelVisibility_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			markDirty();
			applyPanelConfigToMission(true);
			if (_panelMode)
			{
				refreshPanelModeList();
				refreshPanelModeVisuals();
			}
			else refreshXwingTabPagePanels();
			refreshPanelTextLines(getWorkingPageTemplate(), getWorkingPanelIndex());
		}

		private void numPanelLeft_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			markDirty();
			if (numPanelLeft.Value > numPanelRight.Value) numPanelRight.Value = numPanelLeft.Value;
			applyPanelConfigToMission(true);
		}
		private void numPanelTop_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			markDirty();
			if (numPanelTop.Value > numPanelBottom.Value) numPanelBottom.Value = numPanelTop.Value;
			applyPanelConfigToMission(true);
		}
		private void numPanelRight_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			markDirty();
			if (numPanelRight.Value < numPanelLeft.Value) numPanelLeft.Value = numPanelRight.Value;
			applyPanelConfigToMission(true);
		}
		private void numPanelBottom_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			markDirty();
			if (numPanelBottom.Value < numPanelTop.Value) numPanelTop.Value = numPanelBottom.Value;
			applyPanelConfigToMission(true);
		}
		private void numPanelTextLines_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			var template = getWorkingPageTemplate();
			var thisPanel = getWorkingPagePanel();
			var title = template.GetElement(Platform.Xwing.PageTemplate.Elements.Title);
			var map = template.GetElement(Platform.Xwing.PageTemplate.Elements.Map);
			var caption = template.GetElement(Platform.Xwing.PageTemplate.Elements.Caption);

			// Resize and realign caption and map according to the selected line count.
			int lines = (int)numPanelTextLines.Value;
			//int oldHeight = thisPanel.Bottom - thisPanel.Top;
			int newHeight = Math.Max(0, (lines * 12) - (lines - 1));
			if (thisPanel.Bottom == 138) thisPanel.Top = (short)(thisPanel.Bottom - newHeight);
			else if (thisPanel.Top == 0) thisPanel.Bottom = (short)newHeight;
			else if (thisPanel.Top + newHeight > 138)
			{
				thisPanel.Bottom = 138;
				thisPanel.Top = (short)(thisPanel.Bottom - newHeight);
			}
			else if (thisPanel.Bottom - newHeight < 0)
			{
				thisPanel.Top = 0;
				thisPanel.Bottom = (short)newHeight;
			}

			if (thisPanel == title) map.Top = title.Bottom;
			else if (thisPanel == caption) map.Bottom = caption.Top;
			if (map.Bottom < map.Top) map.Bottom = map.Top;

			applyPanelConfigToFrontEnd();
			if (_panelMode) refreshPanelModeVisuals();
			markDirty();
		}

		private void cmdPanelResetOrig_Click(object sender, EventArgs e)
		{
			int pageType = getXwingCoreBriefing().Pages[_currentBriefingIndex].PageType;
			if (_panelBackup == null || pageType >= _panelBackup.Length || _panelBackup[pageType] == null) return;

			setPanelRawData(pageType, _panelBackup[pageType]);
			refreshPanelModeList();
			selectXwingEditPanel(0);
			markDirty();
		}
		private void cmdPanelResetMap_Click(object sender, EventArgs e)
		{
			getPanelModePageTemplate().SetDefaultsToMapPage();
			refreshPanelModeList();
			selectXwingEditPanel(0);
			markDirty();
		}
		private void cmdPanelResetText_Click(object sender, EventArgs e)
		{
			getPanelModePageTemplate().SetDefaultsToTextPage();
			refreshPanelModeList();
			selectXwingEditPanel(0);
			markDirty();
		}
		#endregion Xwing panel mode events

		#region Editing parameter events
		private void cboMoveIncrement_SelectedIndexChanged(object sender, EventArgs e)
		{
			setEditPositionIncrement(0);
			if (_panelMode)
			{
				// We can use the move amount for pixels, but none of the grid settings.
				int value = cboMoveIncrement.SelectedIndex + 1;
				if (value > 8) value = 1;

				numPanelLeft.Increment = value;
				numPanelRight.Increment = value;
				numPanelTop.Increment = value;
				numPanelBottom.Increment = value;
			}
		}

		private void chkMoveLockX_CheckedChanged(object sender, EventArgs e) { if (chkMoveLockX.Checked) chkMoveLockY.Checked = false; }
		private void chkMoveLockY_CheckedChanged(object sender, EventArgs e) { if (chkMoveLockY.Checked) chkMoveLockX.Checked = false; }
		
		/// <summary>Assists the positional NumericUpDown controls to perform snapping and adjustments when the value is manually changed by the user.</summary>
		void validatePositionInput(NumericUpDown num, int moveIncrement)
		{
			if (ActiveControl != num) return;

			decimal value = num.Value;
			bool snap = isVisibleChecked(chkMoveSnapGrid);

			// Allow manual entry of waypoint style values via decimal values.  Note that the grid is based on miles, not kilometers.
			// Known issue: Values that are considered numerically identical despite text changes, such as "1" and "1.0", won't trigger a value change.
			// The Validate and Validating events don't trigger until after a value change, so there's no easy way to intercept the raw text.
			if (value.ToString().Contains("."))
			{
				if (float.TryParse(value.ToString(), out float fval))
				{
					value = (decimal)(fval * 160);

					// Snapping doesn't use rounding, so offset the value to compensate.
					if (snap)
					{
						int sign = (value > 0 ? 1 : -1);
						value += (moveIncrement * sign) / 2;
					}
				}
			}

			// Apply grid snap if applicable.
			if (snap) value = getNewAxisPosition((int)value, 0, moveIncrement);

			// Assign value if changed, guarding to prevent infinite recursion of events.
			if (value != num.Value)
			{
				_loading = true;
				num.Value = value;
				_loading = false;
			}
		}

		private void numX_ValueChanged(object sender, EventArgs e)
		{
			refreshPositionLabel("X:", lblX, numX);
			if (!_loading)
			{
				validatePositionInput(numX, getMoveIncrementX());
				positionChanged(true, false);
			}
		}
		private void numY_ValueChanged(object sender, EventArgs e)
		{
			refreshPositionLabel("Y:", lblY, numY);

			if (!_loading)
			{
				validatePositionInput(numY, getMoveIncrementY());
				positionChanged(false, true);
			}
		}
		
		private void hsbBRF_Scroll(object sender, ScrollEventArgs e) => safeSetNumeric(numX, hsbBRF.Value);
		private void vsbBRF_Scroll(object sender, ScrollEventArgs e) => safeSetNumeric(numY, vsbBRF.Value);
	
		private void cmdTimeShiftAdd_Click(object sender, EventArgs e) => timelineShift(getTimeShiftIncrement(cboTimeShift));
		private void cmdTimeShiftSub_Click(object sender, EventArgs e) => timelineShift(-getTimeShiftIncrement(cboTimeShift));

		private void cmdTitle_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.TitleText);
		private void cmdCaption_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.CaptionText);
		private void cmdPanel3_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.PanelText3);
		private void cmdPanel4_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.PanelText4);
		private void cmdClear_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.ClearFgTags);
		private void cmdFG_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.FgTag1);
		private void cmdText_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.TextTag1);
		private void cmdMove_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.MoveMap);
		private void cmdZoom_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.ZoomMap);
		private void cmdBreak_Click(object sender, EventArgs e)
		{
			// Can skip the UI initialization and go directly to insert, which will perform the error checking.
			_tempEvent = AbstractEventType.PageBreak;
			insertTempEvent();
		}
		private void cmdMarker_Click(object sender, EventArgs e)
		{
			_tempEvent = AbstractEventType.SkipMarker;
			insertTempEvent();
		}
		private void cmdRegion_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.XwaChangeRegion);
		private void cmdNewShip_Click(object sender, EventArgs e)
		{
			if (isXwing)
			{
				// The array limits in game appear to be 40 in XW98, and 24 in XW94.
				// The verification function will report any problems.
				beginTempEvent(AbstractEventType.XwingNewIcon);
			}
			else beginTempEvent(AbstractEventType.XwaSetIcon);
		}
		private void cmdMoveShip_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.XwaMoveIcon);
		private void cmdRotate_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.XwaRotateIcon);
		private void cmdShipInfo_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.XwaShipInfo);
		private void cmdShipInfoText_Click(object sender, EventArgs e) => beginTempEvent(AbstractEventType.XwaInfoParagraph);

		private void cmdOk_Click(object sender, EventArgs e) => insertTempEvent();
		private void cmdCancel_Click(object sender, EventArgs e) => endTempEvent(false);

		private void cboEditString_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			int baseOffset = 0;

			// If this is visible, it's for the ShipInfo text, where strings are one-based.
			if (pnlShipState.Visible && optStateOn.Checked) baseOffset = 1;
			modifyParam(EditParamType.String, baseOffset + cboEditString.SelectedIndex);

			// The dropdown is still zero-based.
			setEditString(_editStringType, cboEditString.SelectedIndex);
		}

		private void txtEditString_TextChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			updateEditString(txtEditString.Text);
			foreach (var so in _selectedObjects) if (isValidEvent(so)) _lastEditEventUid = _events[so.Index].UID;
		}

		private void cboFgTagSlot_SelectedIndexChanged(object sender, EventArgs e) => modifyParam(EditParamType.TagEventType, cboFgTagSlot.SelectedIndex);
		private void cboFGTagItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			if (_tempEvent != AbstractEventType.None) refreshMap();
			modifyParam(EditParamType.FgIndex, cboFGTagItem.SelectedIndex);
		}

		private void cboTextTagSlot_SelectedIndexChanged(object sender, EventArgs e) => modifyParam(EditParamType.TagEventType, cboTextTagSlot.SelectedIndex);
		private void cboTextTagColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			if (_tempEvent != AbstractEventType.None) refreshMap();
			modifyParam(EditParamType.TextColor, cboTextTagColor.SelectedIndex);
		}

		private void cboIcon_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			if (_tempEvent != AbstractEventType.None) refreshMap();
			else modifyParam(EditParamType.IconIndex, cboIcon.SelectedIndex);
		}

		private void cboCraftType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_tempEvent != AbstractEventType.None) refreshMap();
			else modifyParam(EditParamType.IconCraftType, cboCraftType.SelectedIndex);
		}

		private void cboIff_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_tempEvent != AbstractEventType.None) refreshMap();
			else modifyParam(EditParamType.IconCraftIff, cboIff.SelectedIndex);
		}

		private void cboRotation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			if (_tempEvent != AbstractEventType.None) refreshMap();
			else
			{
				// Special case to handle rotation if a MoveIcon is selected, since the rotation may need to be created.
				var so = getPrimarySelectedObject();
				if (so != null && isValidEvent(so) && _events[so.Index].Event == AbstractEventType.XwaMoveIcon)
					setRotationForMoveIcon(so.Index, true, cboRotation.SelectedIndex);
				else modifyParam(EditParamType.IconCraftRotation, cboRotation.SelectedIndex);
			}
		}

		private void txtCraftName_Validated(object sender, EventArgs e)
		{
			if (_loading || !isXwing || _tempEvent != AbstractEventType.None) return;

			string name = txtCraftName.Text;
			foreach (var so in _selectedObjects)
			{
				if (!isValidWaypoint(so)) continue;

				var afg = _flightgroups[so.Index];

				// OnLeave event doesn't necessarily mean it changed.
				if (stringEqual(afg.Name, name)) continue;

				UndoOperation op = new UndoOperation(UndoType.FlightgroupData, afg.GetDataSnapshot(), so.Index);
				afg.DisplayName = getCraftDisplayName(getCraftAbbrev(afg.CraftType), name);
				afg.Name = name;
				op.Update(afg.GetDataSnapshot());
				addUpdatedUndoOperation(op, false);
				notifyFlightgroupChange(so.Index);
			}
		}

		private void cmdGotoEvent_Click(object sender, EventArgs e) { if (cmdGotoEvent.Tag != null) gotoContextEvent((int)cmdGotoEvent.Tag); }

		private void cmdMoveReset_Click(object sender, EventArgs e) => restoreFirstPreviewStep();

		private void cmdCreateMove_Click(object sender, EventArgs e) => createAutoMove();

		private void cboRegion_SelectedIndexChanged(object sender, EventArgs e) => modifyParam(EditParamType.Region, cboRegion.SelectedIndex);

		private void optStateOn_CheckedChanged(object sender, EventArgs e)
		{
			if (pnlString.Visible)
			{
				// This is for the ShipInfo text.
				pnlString.Enabled = optStateOn.Checked;
				int offset = (optStateOn.Checked ? 1 : 0);
				modifyParam(EditParamType.String, offset + _editStringIndex);

				if (_tempEvent == AbstractEventType.XwaInfoParagraph) refreshMap();
			}
			else
			{
				modifyParam(EditParamType.CraftInfoState, (optStateOn.Checked ? 1 : 0));

				if (_tempEvent == AbstractEventType.XwaShipInfo) pnlShipInfoEnd.Enabled = optStateOn.Checked;

				if (lblGoto.Visible) lblGoto.Text = "Go to Info " + (optStateOn.Checked ? "End" : "Start");
			}
		}
		#endregion Editing parameter events
	
		#region Playback and navigation events
		private void tmrBrief_Tick(object sender, EventArgs e)
		{
			// This function drives everything related to time, not just map updates.
			// First check if something should be synchronized with the main form.  This helps prevent
			// flooding the main form with updates whenever a flightgroup is moved via mouse.
			if (_pendingWaypointSync.Count > 0)
			{
				foreach (int fgIndex in _pendingWaypointSync) notifyDataChange(DataChangeTags.FgWaypoint, fgIndex, null);
				_pendingWaypointSync.Clear();
			}

			int curTime = Environment.TickCount;
			int deltaTime = curTime - _playbackLastSystemTime;
			_playbackLastSystemTime = curTime;

			if (_titleOverrideTime > 0)
			{
				_titleOverrideTime -= deltaTime;
				if (_titleOverrideTime <= 0)
				{
					_titleOverrideString = "";
					refreshCanvas(RefreshFlags.Title);
				}
			}

			if (isPlaybackActive)
			{
				// Accumulate the passage of time and determine how many frames are ready.  Keeps track of
				// the fractional component for the sake of rollover time, allowing slightly more precision
				// than would be possible with only integer math and truncating the framerate to whole
				// milliseconds.  Truncating was causing noticable desync in longer briefings.
				_playbackPrecisionTime += (deltaTime << 8);
				double rate = 1000.0 / _playbackFramerate;
				int milli = (int)rate;
				int part = (int)((rate - Math.Floor(rate)) * 255.0);

				int frameTime = (milli << 8) + part;
				if (_playbackPrecisionTime >= frameTime)
				{
					int frames = _playbackPrecisionTime / frameTime;
					_playbackFramesReady += frames;
					_playbackPrecisionTime -= (frames * frameTime);
				}
				checkReadyFrames();
			}
			checkRender();
		}

		private void cmdPlay_Click(object sender, EventArgs e)
		{
			if (hsbTimer.Value == _briefingDuration) resetBriefing();
			playBriefing(false);
		}
		private void cmdPause_Click(object sender, EventArgs e) => pauseBriefing();
		private void cmdStop_Click(object sender, EventArgs e)
		{
			if (_tempEvent != AbstractEventType.None) return;

			pauseBriefing();
			resetBriefing();
		}
		private void cmdPrev_Click(object sender, EventArgs e)
		{
			if (_tempEvent != AbstractEventType.None) return;

			gotoNextPage(hsbTimer.Value, -1);
		}
		private void cmdNext_Click(object sender, EventArgs e)
		{
			if (_tempEvent != AbstractEventType.None) return;

			gotoNextPage(hsbTimer.Value, 1);
		}
		private void cmdFF_Click(object sender, EventArgs e)
		{
			if (!isPlaybackActive) playBriefing(false);
			else setPlaybackSpeed(_playbackSpeed * 2);
		}
		
		private void hsbTimer_ValueChanged(object sender, EventArgs e)
		{
			refreshTime();
			refreshTimeline();

			// The timer increments this value automatically.  If it's focused, it means the user interacted with it.
			if (hsbTimer.Focused) jumpToTime(hsbTimer.Value, false);
		}
		#endregion Playback and navigation events

		#region Xwa icon editing events
		private void iconMoveButton_Click(object sender, EventArgs e) => generateDirectionalIconMovement(sender, _keyStateCtrl);
		private void iconMoveButton_MouseEnter(object sender, EventArgs e)
		{
			// Steal focus away from any other input control.  This lets us properly trap the Control key to refresh the preview.
			((Control)sender).Focus();
			refreshDirectionalPreview(sender, _keyStateCtrl);
		}
		private void iconMoveButton_MouseLeave(object sender, EventArgs e)
		{
			if (sender != null && (sender.GetType() == typeof(Button)))
			{
				// Forcibly change the form's ActiveControl.
				pctBrief.Focus();
			}
			refreshDirectionalPreview(null, _keyStateCtrl);  // Remove preview
		}
		private void iconMoveStep_MouseLeave(object sender, EventArgs e) => refreshDirectionalPreview(null, _keyStateCtrl);

		private void numIconStepX_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveControl != sender) return;

			if (chkIconLinkStep.Checked) numIconStepY.Value = numIconStepX.Value;
			refreshDirectionalPreview(cmdMoveNE, _keyStateCtrl);
		}
		private void numIconStepY_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveControl != sender) return;

			if (chkIconLinkStep.Checked) numIconStepX.Value = numIconStepY.Value;
			refreshDirectionalPreview(cmdMoveNE, _keyStateCtrl);
		}

		private void numIconAccelX_ValueChanged(object sender, EventArgs e) { if (ActiveControl == sender && chkIconLinkAccel.Checked) numIconAccelY.Value = numIconAccelX.Value; }
		private void numIconAccelY_ValueChanged(object sender, EventArgs e) { if (ActiveControl == sender && chkIconLinkAccel.Checked) numIconAccelX.Value = numIconAccelY.Value; }

		private void cmdIconStepDist_Click(object sender, EventArgs e) => detectIconMoveDistance();

		private void cboPresetMoveAmount_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = cboPresetMoveAmount.SelectedIndex;
			if (index >= 0 && index <= 5)
			{
				// Move by fraction of a grid square.
				// Values: 2, 4, 8, 16, 32, 64
				int value = 256 / (2 << index);
				numIconStepX.Value = value;
				numIconStepY.Value = value;
			}
			else if (index >= 6 && index <= 13)
			{
				// Move by unscaled (1x) pixel on the map at the current zoom level.
				// Values: 1 to 8
				int mult = (index - 6) + 1;
				numIconStepX.Value = getPixelRawX() * mult;
				numIconStepY.Value = getPixelRawX() * mult;
			}
			else if (index >= 14 && index <= 17)
			{
				// Accelerate by preselected amount.
				int[] values = { 13, 30, -10, -25 };
				numIconAccelX.Value = values[index - 14];
				numIconAccelY.Value = values[index - 14];
				optIconAccelPercent.Checked = true;
			}
		}

		private void optIconAccelNone_CheckedChanged(object sender, EventArgs e) => refreshIconAcceleration();

		private void chkAutoMoveIcon_CheckedChanged(object sender, EventArgs e)
		{
			pnlPosition.Enabled = chkAutoMoveIcon.Checked;
			pnlMoveOptions.Enabled = chkAutoMoveIcon.Checked;
			pnlRotation.Enabled = chkAutoMoveIcon.Checked;

			if (lblTempInfo.Visible) lblTempInfo.Enabled = !chkAutoMoveIcon.Checked;

			// Update the icon visibility on the map.
			refreshMap();
		}
		#endregion Xwa icon editing events

		#region Xwa path mode events
		private void cmdPathMode_Click(object sender, EventArgs e) => setPathMode(true);

		private void lstPath_SelectedIndexChanged(object sender, EventArgs e) { if (!_loading && _pathMode) selectPathNode(lstPath.SelectedIndex, true); }

		private void numPathNodeStep_ValueChanged(object sender, EventArgs e)
		{
			int index = lstPath.SelectedIndex;
			if (_loading || index < 0) return;

			_pathNodes[index].Steps = (int)numPathNodeStep.Value;
			refreshPathList(index);
			applyPreviewFromNodes();
		}
		private void numPathNodeTime_ValueChanged(object sender, EventArgs e)
		{
			int index = lstPath.SelectedIndex;
			if (_loading || index < 0) return;

 			_pathNodes[index].Time = (int)numPathNodeTime.Value;
			refreshPathList(index);
			applyPreviewFromNodes();
		}

		private void cboPathNodeRot_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = lstPath.SelectedIndex;
			if (_loading || index < 0) return;

			_pathNodes[index].Rotation = cboPathNodeRot.SelectedIndex;
			refreshPathList(index);
			applyPreviewFromNodes();
		}
		private void cboPathAutoType_SelectedIndexChanged(object sender, EventArgs e)
		{
			PathAutoType curType = (PathAutoType)cboPathAutoType.SelectedIndex;
			PathAutoType prevType = PathAutoType.TimeSec;
			if (cboPathAutoType.Tag != null) prevType = (PathAutoType)cboPathAutoType.Tag;

			decimal min = 1;
			decimal max = 100;
			decimal increment = 1;
			decimal value = numPathAutoAmount.Value;

			// For time types, convert between their equivalent values.
			// The distance types will just set as defaults.  Trying to convert them was overly complex.
			switch (curType)
			{
				case PathAutoType.TimeSec:	min = 0.25m; max = 120.0m; increment = 0.25m;
					if (prevType == PathAutoType.TimeTick) value /= _ticksPerSecond;
					else value = 2.0m;
					break;
				case PathAutoType.TimeTick: min = 1.0m; max = 120.0m * _ticksPerSecond; increment = _ticksPerSecond / 4;
					if (prevType == PathAutoType.TimeSec) value *= _ticksPerSecond;
					else value = 2 * _ticksPerSecond;
					break;
				case PathAutoType.DistPixel: min = 1.0m; max = 50.0m; increment = 1.00m;    // Pixels at 1.0 map scale
					value = 8.0m;
					break;
				case PathAutoType.DistMeter: min = 1.0m; max = 500.0m; increment = 10.00m;
					value = 50.0m;
					break;
				case PathAutoType.DistRaw: min = 1.0m; max = 512.0m; increment = 10.00m;
					value = 30.0m;
					break;
			}
			numPathAutoAmount.DecimalPlaces = (curType == PathAutoType.TimeSec ? 2 : 0);
			numPathAutoAmount.Minimum = min;
			numPathAutoAmount.Maximum = max;
			numPathAutoAmount.Increment = increment;
			if (value < min) value = min;
			else if (value > max) value = max;
			numPathAutoAmount.Value = value;
			cboPathAutoType.Tag = curType;
		}
		private void cboPathAutoRotate_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool state = (cboPathAutoRotate.SelectedIndex > 0);
			lblPathDefaultFacing.Enabled = state;
			cboPathDefaultFacing.Enabled = state;
		}

		private void cmdPathPreview_Click(object sender, EventArgs e) => applyPreviewFromNodes();
		private void cmdPathClearList_Click(object sender, EventArgs e)
		{
			_pathNodes.Clear();
			lstPath.Items.Clear();
			_mapPreviewIcons.Clear();
			refreshMap();
		}
		private void cmdPathCreate_Click(object sender, EventArgs e)
		{
			if (_pathNodes.Count < 2)
			{
				popupError("You must place at least two nodes to create a path.\n\nShift + Left Click on map to place a node.\nSelect and press the Delete key to remove.");
				return;
			}

			int moveEventIndex = getWorkingMoveIcon();
			if (moveEventIndex == -1)
			{
				popupError("You must select an icon to generate the path from.");
				return;
			}

			applyPreviewFromNodes();
			if (!verifyPreviewIcons(true)) return;

			int baseTime = _events[moveEventIndex].Time;
			if (hsbTimer.Value > baseTime) baseTime = hsbTimer.Value;

			int lastUid = generateIconsFromPreview(baseTime);
			if (lastUid >= 0)
			{
				rebuildShadowIcons();
				relinkSelectedEventUid();
			}

			_mapPreviewIcons.Clear();
			refreshMap();
			refreshTimeline();
		}
		private void cmdPathAutoDistribute_Click(object sender, EventArgs e) => generateAutoPathPreview();
		private void cmdPathAction_Click(object sender, EventArgs e)
		{
			int eventIndex = getWorkingMoveIcon();
			int iconIndex = -1;
			if (eventIndex >= 0) iconIndex = _events[eventIndex].Params[0];

			switch (cboPathAction.SelectedIndex)
			{
				case 0:  // Align path to icon: moves entire path so that node #1 is centered on the move icon.
					if (_pathNodes.Count > 0 && iconIndex >= 0 && iconIndex < _mapIcons.Length)
					{
						int moveX = _mapIcons[iconIndex].X - _pathNodes[0].X;
						int moveY = _mapIcons[iconIndex].Y - _pathNodes[0].Y;
						foreach (var node in _pathNodes)
						{
							node.X += moveX;
							node.Y += moveY;
						}
					}
					break;
				case 1:  // Stretch #1 to icon: move only node #1, changing the length of the first segment.  Maintain position of all other nodes.
					if (_pathNodes.Count > 0 && iconIndex >= 0 && iconIndex < _mapIcons.Length)
					{
						_pathNodes[0].X = _mapIcons[iconIndex].X;
						_pathNodes[0].Y = _mapIcons[iconIndex].Y;
					}
					break;
				case 2:  // Flip horizontal
					foreach (var node in _pathNodes)
					{
						int diffX = (node.X - _pathNodes[0].X) * -1;
						node.X = _pathNodes[0].X + diffX;
					}
					break;
				case 3:  // Flip Vertical
					foreach (var node in _pathNodes)
					{
						int diffY = (node.Y - _pathNodes[0].Y) * -1;
						node.Y = _pathNodes[0].Y + diffY;
					}
					break;
				case 4:  // Rotate 90 clockwise
					rotatePathNodes(true);
					break;
				case 5:  // Rotate 90 counter-clockwise
					rotatePathNodes(false);
					break;
				case 6:  // Derive path
					derivePathFromSelection(iconIndex);
					break;
			}
			if (_mapPreviewIcons.Count > 0) applyPreviewFromNodes();
			refreshMap();
		}
		#endregion Xwa path mode events

		#region String tab events
		private void lstMainTags_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			setMainStringTextBox(txtMainEditTag, _tags, lstMainTags.SelectedIndex, lblMainTagLength, _maxTagLength);
			refreshMainStringMoveButtons();
		}
		private void lstMainStrings_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			int index = lstMainStrings.SelectedIndex;
			setMainStringTextBox(txtMainEditString, _strings, index, lblMainStringLength, _maxStringLength);
			refreshMainStringMoveButtons();
			txtCaptionNotes.Text = getSafeString(_captionNotes, index);
		}

		private void lstMainStrings_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			txtMainEditString.Focus();
			txtMainEditString.Select(txtMainEditString.Text.Length, 0);
			txtMainEditString.ScrollToCaret();
		}
		private void lstMainTags_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			txtMainEditTag.Focus();
			txtMainEditTag.Select(txtMainEditTag.Text.Length, 0);
			txtMainEditTag.ScrollToCaret();
		}

		private void txtCaptionNotes_Validated(object sender, EventArgs e)
		{
			int index = lstMainStrings.SelectedIndex;
			if (index >= 0 && index < _captionNotes.Count) _captionNotes[index] = txtCaptionNotes.Text;
		}

		private void txtMainEditTag_TextChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			updateMainStringFromTextBox(txtMainEditTag, _tags, lstMainTags.SelectedIndex, lstMainTags, lblMainTagLength, _maxTagLength, UndoType.Tag);
		}
		private void txtMainEditString_TextChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			updateMainStringFromTextBox(txtMainEditString, _strings, lstMainStrings.SelectedIndex, lstMainStrings, lblMainStringLength, _maxStringLength, UndoType.String);
		}

		private void cmdMoveTagUp_Click(object sender, EventArgs e) => moveMainString(lstMainTags.SelectedIndex, -1, BriefingString.Tag);
		private void cmdMoveTagDown_Click(object sender, EventArgs e) => moveMainString(lstMainTags.SelectedIndex, 1, BriefingString.Tag);
		private void cmdMoveStringUp_Click(object sender, EventArgs e) => moveMainString(lstMainStrings.SelectedIndex, -1, BriefingString.String);
		private void cmdMoveStringDown_Click(object sender, EventArgs e) => moveMainString(lstMainStrings.SelectedIndex, 1, BriefingString.String);
		#endregion String tab events

		#region Event tab events
		private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			deselectObjects();
			applyEventListToSelection();
			refreshEventListSidebar();
			refreshEventListMoveButtons();
		}

		private void cmdUp_Click(object sender, EventArgs e) => moveListEventsInTime(-1);
		private void cmdDown_Click(object sender, EventArgs e) => moveListEventsInTime(1);

		private void cmdNew_Click(object sender, EventArgs e)
		{
			// This is just a placeholder event.  The user will have to change it to something else.
			if (!hasAvailableSpace(AbstractEventType.PageBreak))
			{
				popupError("Not enough space to create new event.");
				return;
			}

			short time = 0;
			int index = lstEvents.SelectedIndex;

			if (index >= 0) time = _events[index].Time;

			// Insert after current selected event.  If nothing was selected (-1) then it will be the first item.
			index++;

			AbstractEvent evt = new AbstractEvent(time, AbstractEventType.PageBreak);

			beginUndoFrame(true);
			insertEvent(index, evt, true);

			// Add placeholder slot to end of list, then refresh everything from the insertion point to the end.
			// Suppressing loading greatly improves performance by ignoring several selection change events.
			_loading = true;
			lstEvents.Items.Add("");
			refreshEventListRange(index, lstEvents.Items.Count - 1);
			lstEvents.ClearSelected();
			_loading = false;

			lstEvents.SelectedIndex = index;
		}
		private void cmdDelete_Click(object sender, EventArgs e) => deleteSelectedItems();

		private void chkEventListDifference_CheckedChanged(object sender, EventArgs e)
		{
			refreshEventList(-1);
			applySelectionToEventList();
		}
		private void chkEventListTickTime_CheckedChanged(object sender, EventArgs e)
		{
			refreshEventList(-1);
			applySelectionToEventList();

			// It won't refresh the label automatically since applying the selection is masked through the loading flag.
			// It's not important, so just ask the user to refresh if they really need to see it.
			if (_selectedObjects.Count > 0) lblEventTimeDifference.Text = "Reselect to refresh";
		}

		private void cboEventType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading || _selectedObjects.Count != 1) return;

			var so = _selectedObjects[0];
			if (!isValidEvent(so)) return;

			var evt = _events[so.Index];
			var oldDef = EventDef.GetEventDefByType(evt.Event);
			var newDef = EventDef.GetEventDefByName(cboEventType.Text);
			if (oldDef == newDef) return;

			int spaceChange = newDef.GetSize() - oldDef.GetSize();
			int extraSize = Math.Max(0, spaceChange);

			if (_eventSpaceUsed + extraSize > _eventSpaceMax)
			{
				popupError("Not enough event space to hold the new event type.");
				return;
			}

			UndoOperation op = new UndoOperation(UndoType.Event, evt.GetDataSnapshot(), so.Index);
			if (newDef.ParamClass == 0 || (oldDef.ParamClass != newDef.ParamClass)) for (int i = 0; i < evt.Params.Length; i++) evt.Params[i] = 0;
			if (newDef.Type == AbstractEventType.ZoomMap)
			{
				evt.Params[0] = (short)_currentZoom.X;
				evt.Params[1] = (short)_currentZoom.Y;
			}
			evt.Event = newDef.Type;

			op.Update(evt.GetDataSnapshot());
			addUpdatedUndoOperation(op, false);
			updateEventSpace(spaceChange);
			refreshEventListSelected();
			displayEditControls();
		}

		private void cmdEventListShiftUp_Click(object sender, EventArgs e)
		{
			if (optEventListShiftAll.Checked) timeShift(lstEvents.SelectedIndex, -getTimeShiftIncrement(cboEventShift), true);
			else moveListEventsAcrossTime(-getTimeShiftIncrement(cboEventShift));
		}

		private void cmdEventListShiftDown_Click(object sender, EventArgs e)
		{
			if (optEventListShiftAll.Checked) timeShift(lstEvents.SelectedIndex, getTimeShiftIncrement(cboEventShift), true);
			else moveListEventsAcrossTime(getTimeShiftIncrement(cboEventShift));
		}

		private void numEventTime_ValueChanged(object sender, EventArgs e)
		{
			if (_loading || lstEvents.SelectedIndex < 0 || ActiveControl != numEventTime) return;

			int curTime = _events[lstEvents.SelectedIndex].Time;
			int destTime = (int)(chkEventListTickTime.Checked ? numEventTime.Value : numEventTime.Value * _ticksPerSecond);
			int step = destTime - curTime;

			if (optEventListShiftAll.Checked) timeShift(lstEvents.SelectedIndex, step, true);
			else moveListEventsAcrossTime(step);
		}

		private void optEventListShiftSelected_CheckedChanged(object sender, EventArgs e)
		{
			chkEventListShiftJump.Enabled = optEventListShiftSelected.Checked;
			refreshEventListMoveButtons();
		}

		private void chkEventListShiftJump_CheckedChanged(object sender, EventArgs e) => refreshEventListMoveButtons();
		#endregion Event tab events

		#region Editor options tab events
		private void optConfigDynamicScale_CheckedChanged(object sender, EventArgs e)
		{
			if (ActiveControl == optConfigDynamicScale || ActiveControl == optConfigFixedScale) updateFormSize(true);

			lblMapScale.Enabled = !optConfigDynamicScale.Checked;
			numConfigMapScale.Enabled = !optConfigDynamicScale.Checked;
			chkConfigAutoNearest.Enabled = !optConfigDynamicScale.Checked;
		}

		private void cboConfigRenderMode_SelectedIndexChanged(object sender, EventArgs e) => refreshInterpolationMode();
		private void chkConfigAutoNearest_CheckedChanged(object sender, EventArgs e) => refreshInterpolationMode();

		private void chkConfigAutoDuration_CheckedChanged(object sender, EventArgs e)
		{
			bool state = chkConfigAutoDuration.Checked;
			lblConfigAutoDuration.Enabled = state;
			numConfigAutoDuration.Enabled = state;
		}

		private void numTimelineFontSize_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveControl != sender) return;

			float size = (float)numTimelineFontSize.Value;
			if (size == _timelineFont.Size) return;

			_timelineFont.Dispose();
			_timelineFont = new System.Drawing.Font("Arial", size, FontStyle.Regular);

			if (getMinimumTimelineHeight() > pctTimeline.Height) updateFormSize(true);
			else refreshTimeline();
		}
		private void numTimelineRowCount_ValueChanged(object sender, EventArgs e) { if (ActiveControl == sender && getMinimumTimelineHeight() > pctTimeline.Height) updateFormSize(true); }
		private void numConfigMapScale_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveControl != sender) return;

			// Need to assign the scale so the new minimum size can be calculated.
			_mapScale = (float)numConfigMapScale.Value;
			updateFormSize(true);
		}
		#endregion Editor options tab events

		#region Briefing manager tab events
		private void chkExportEverything_CheckedChanged(object sender, EventArgs e) => refreshExportOptions();

		private void cboBriefActions_SelectedIndexChanged(object sender, EventArgs e) => refreshBriefingActions();

		private void cmdBriefAction_Click(object sender, EventArgs e)
		{
			int targetIndex = cboBriefTarget.SelectedIndex;
			switch ((BriefingManagerActions)cboBriefActions.SelectedIndex)
			{
				case BriefingManagerActions.Swap:
					if (confirmBriefingTarget() && confirmBriefingAction("The current briefing will be swapped with the chosen briefing."))
					{
						exportActiveBriefing();
						var backup = backupManagerStrings();
						var temp = _briefingList[_currentBriefingIndex];
						_briefingList[_currentBriefingIndex] = _briefingList[targetIndex];
						_briefingList[targetIndex] = temp;
						restoreManagerStrings(backup);

						// Selecting the briefing clears the undo/redo buffers as well as repopulates the list on this tab.
						selectBriefing(_currentBriefingIndex, true);
						markDirty();
					}
					break;
				case BriefingManagerActions.Duplicate:
					if (confirmBriefingTarget() && confirmBriefingAction("The target briefing will be completely overwritten by the current briefing."))
					{
						clearUndo();
						exportActiveBriefing();
						var backup = backupManagerStrings();
						_briefingList[targetIndex].CopyFrom(_briefingList[_currentBriefingIndex]);
						restoreManagerStrings(backup);
						populateBriefingOptions();
						markDirty();
					}
					break;
				case BriefingManagerActions.Erase:
					string eraseType = (isXwing ? "briefing page" : "briefing");
					if (confirmBriefingAction($"The current {eraseType} will be completely erased!"))
					{
						var backup = backupManagerStrings();
						var brf = _briefingList[_currentBriefingIndex];
						brf.Erase();
						// Restore default events.  Any other briefing or page will be left completely blank.
						// In XWING, the other pages are most likely text anyway.
						if (!isXwing || _currentBriefingIndex == 0)
						{
							brf.Events.Add(new AbstractEvent(0, AbstractEventType.MoveMap, 0, 0));
							brf.Events.Add(new AbstractEvent(0, AbstractEventType.ZoomMap, 48, 48));
						}
						// Including XW and TIE serves no purpose other than to keep the listbox from getting updated, even though it's disabled.
						int viewIndex = (isXvt || isXwa ? _currentBriefingIndex : 0);
						brf.ViewedByTeam[viewIndex] = true;
						if (isXwing && _currentBriefingIndex == 0 && !popupConfirm($"Delete all strings and tags too?")) restoreManagerStrings(backup);
						selectBriefing(_currentBriefingIndex, true);
						markDirty();
					}
					break;
				case BriefingManagerActions.ExportToBox:
					txtBriefImport.Text = exportBriefingAsText();
					break;
				case BriefingManagerActions.ImportFromBox:
					if (string.IsNullOrWhiteSpace(txtBriefImport.Text))
					{
						popupError("No text to import.");
						return;
					}
					if (confirmBriefingAction("The current briefing will be modified according to the section checkboxes, and which sections are contained in the text."))
					{
						ImportData dat = new ImportData();
						if (loadImportDataFromText(txtBriefImport.Text, false, dat))
						{
							mergeImportData(dat);
							exportActiveBriefing();  // Push back so populate can grab the updated info
							markDirty();
							verifyBriefing(true, true);
							populateBriefingOptions();
						}
					}
					break;
				case BriefingManagerActions.ImportFromFile:
					OpenFileDialog open = new OpenFileDialog
					{
						Title = "Select mission to import",
						Filter = "Mission files (*.tie)|*.tie|X-Wing mission (*.xwi)|*.xwi|Exported text (*.txt)|*.txt|All files (*.*)|*.*"
					};
					DialogResult result = open.ShowDialog();
					if (result == DialogResult.OK)
					{
						ImportData dat = new ImportData();
						if (loadImportDataFromFile(open.FileName, dat))
						{
							mergeImportData(dat);
							exportActiveBriefing();
							markDirty();
							verifyBriefing(true, true);
							populateBriefingOptions();
						}
					}
					break;
			}
		}

		private void lstTeams_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ActiveControl != sender) return;

			if (isXvt)
			{
				_currentTeamIndex = getVisibleTeam();
				refreshMap();
			}
			markDirty();
			refreshFormTitle();
		}
		#endregion Briefing manager tab events

		#region Xwing tab events
		private void lstXwingPageTypes_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			refreshXwingTabPagePanels();
			applyPanelConfigToFrontEnd();
			refreshPanelTextLines(getXwingTabSelectedPageType(), lstXwingPagePanels.SelectedIndex);
		}
		private void lstXwingPagePanels_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			applyPanelConfigToFrontEnd();
			refreshPanelTextLines(getXwingTabSelectedPageType(), lstXwingPagePanels.SelectedIndex);
		}

		private void cmdXwingDefaultMap_Click(object sender, EventArgs e)
		{
			var page = getXwingTabSelectedPageType();
			page.SetDefaultsToMapPage();
			refreshXwingTabPageTemplates();
			refreshXwingTabPagePanels();
			applyPanelConfigToFrontEnd();
			rebuildXwingBriefingList();
			markDirty();
		}
		private void cmdXwingDefaultText_Click(object sender, EventArgs e)
		{
			var page = getXwingTabSelectedPageType();
			page.SetDefaultsToTextPage();
			refreshXwingTabPageTemplates();
			refreshXwingTabPagePanels();
			applyPanelConfigToFrontEnd();
			rebuildXwingBriefingList();
			markDirty();
		}

		private void cmdXwingRestoreView_Click(object sender, EventArgs e)
		{
			var data = getXwingCoreBriefing();
			if (!popupConfirm("This will completely reset all page types to their default settings.  Any extra page types will be removed.  Are you sure?")) return;

			string s = "";
			for (int i = 0; i < data.Pages.Count; i++)
				if (data.Pages[i].PageType >= 2) s += $"Page #{i + 1} uses type #{data.Pages[i].PageType + 1}{Environment.NewLine}";
			if (s != "")
			{
				popupError("Can't remove these page types that are currently in use:" + Environment.NewLine + s);
				return;
			}

			data.Templates.Clear();
			Platform.Xwing.PageTemplate map = new Platform.Xwing.PageTemplate();
			map.SetDefaultsToMapPage();
			Platform.Xwing.PageTemplate text = new Platform.Xwing.PageTemplate();
			text.SetDefaultsToTextPage();
			data.Templates.Add(map);
			data.Templates.Add(text);
			refreshXwingTabPageTemplates();
			refreshXwingTabPagePanels();
			applyPanelConfigToFrontEnd();
			lstXwingPageTypes.SelectedIndex = 0;
			refreshXwingTabButtons();
			rebuildXwingBriefingList();
			markDirty();
		}

		private void cmdXwingPageTypeAdd_Click(object sender, EventArgs e)
		{
			if (lstXwingPageTypes.Items.Count >= 8) return;

			// Append a new text type, refresh, and select it.
			var core = getXwingCoreBriefing();
			Platform.Xwing.PageTemplate text = new Platform.Xwing.PageTemplate();
			text.SetDefaultsToTextPage();
			core.Templates.Add(text);
			lstXwingPageTypes.Items.Add("");
			int index = lstXwingPageTypes.Items.Count - 1;
			_panelBackup = null;
			refreshXwingTabPageTemplates();
			lstXwingPageTypes.SelectedIndex = index;
			refreshXwingTabButtons();
			markDirty();
		}
		private void cmdXwingPageTypeDel_Click(object sender, EventArgs e)
		{
			// Retain at least two for the sake of "defaults" even if they've been modified.
			if (lstXwingPageTypes.Items.Count <= 2 || lstXwingPageTypes.SelectedIndex < 0) return;

			int delIndex = lstXwingPageTypes.SelectedIndex;
			var core = getXwingCoreBriefing();

			string error = "";
			for (int i = 0; i < core.Pages.Count; i++)
			{
				if (core.Pages[i].PageType != delIndex) continue;

				if (error != "") error += ", ";
				error += "Page #" + (i + 1);
			}
			if (error != "")
			{
				popupError("Can't delete this page type.\n\nIt is currently in use by: " + error);
				return;
			}

			core.Templates.RemoveAt(delIndex);
			lstXwingPageTypes.Items.RemoveAt(delIndex);

			// Adjust indices to compensate for removed element.
			foreach (var page in core.Pages) if (page.PageType > delIndex) page.PageType--;

			// Try to retain index selection if possible and refresh.
			int selIndex = delIndex;
			if (delIndex >= lstXwingPageTypes.Items.Count) selIndex = lstXwingPageTypes.Items.Count - 1;

			lstXwingPages_SelectedIndexChanged(null, null);
			refreshXwingTabPageTemplates();
			lstXwingPageTypes.SelectedIndex = selIndex;
			_panelBackup = null;
			refreshXwingTabButtons();
			markDirty();
		}

		private void cmdXwingPageUp_Click(object sender, EventArgs e) => swapXwingPage(lstXwingPages.SelectedIndex, -1);
		private void cmdXwingPageDown_Click(object sender, EventArgs e) => swapXwingPage(lstXwingPages.SelectedIndex, 1);

		private void cmdXwingPageDelete_Click(object sender, EventArgs e)
		{
			int selIndex = lstXwingPages.SelectedIndex;
			if (lstXwingPages.Items.Count < 1 || selIndex < 0) return;
			if (!popupConfirm("Are you sure you want to delete this page?")) return;

			var core = getXwingCoreBriefing();

			// Back up and restore the strings if necessary.  These are stored in the first page.
			string[] strings = null;
			string[] tags = null;
			if (selIndex == 0)
			{
				strings = _briefingList[0].Strings;
				tags = _briefingList[0].Tags;
			}

			core.Pages.RemoveAt(selIndex);
			_briefingList.RemoveAt(selIndex);
			markDirty();

			// Rebuild before restoring the strings.
			rebuildXwingBriefingList();

			if (selIndex == 0)
			{
				_briefingList[0].Strings = strings;
				_briefingList[0].Tags = tags;
			}

			selectBriefing(0, true);
			cboCurrentBriefing.SelectedIndex = 0;

			// Adjust and assign new selection index.
			if (selIndex >= lstXwingPages.Items.Count) selIndex = lstXwingPages.Items.Count - 1;
			refreshXwingTabPageTemplates();
			lstXwingPages.SelectedIndex = selIndex;
		}
		private void cmdXwingPageNew_Click(object sender, EventArgs e)
		{
			if (lstXwingPages.Items.Count >= 8) return;

			var core = getXwingCoreBriefing();

			// Determine which page type best matches the new type (0 = map, 1 = text, 2 = hint)
			int newType = cboXwingNewType.SelectedIndex;
			int pageType = 0;
			for (int i = 0; i < core.Templates.Count; i++)
			{
				bool isMap = core.Templates[i].GetElement(Platform.Xwing.PageTemplate.Elements.Map).IsVisible;
				if (newType == 0 && isMap) pageType = i;
				else if (newType >= 1 && !isMap) pageType = i;
			}

			// Even if we're not making a map page, set the default waypoint.
			int waypoint = 0;
			for (int i = 0; i < core.Pages.Count; i++) if (core.Pages[i].Waypoint != 0) waypoint = core.Pages[i].Waypoint;

			// If making a hint page, resolve the strings.  We need to handle any errors before generating the page.
			int hintTitle = -1;
			int hintCaption = -1;
			if (newType == 2)
			{
				hintTitle = getOrCreateString(_strings, Platform.Xwing.Strings.BriefingPageHintTitle);
				hintCaption = getOrCreateString(_strings, Platform.Xwing.Strings.BriefingPageHintCaption);
				if (hintTitle == -1 || hintCaption == -1)
				{
					popupError("Not enough free string slots to generate the hint page text.");
					return;
				}
			}

			if (optXwingStringPlaceholder.Checked)
			{
				int genTitle = detectXwingTitle();
				if (cboXwingTitle.SelectedIndex != genTitle || _strings[genTitle] == "") genTitle = getOrCreateString(_strings, ">Placeholder Title String");

				int genCaption = getOrCreateString(_strings, "Page " + (lstXwingPages.Items.Count + 1) + " Placeholder Caption String");

				if (genTitle == -1 || genCaption == -1)
				{
					popupError("Not enough free string slots to generate the automatic title and caption.");
					return;
				}

				cboXwingTitle.SelectedIndex = genTitle;
				cboXwingCaption.SelectedIndex = genCaption;
			}
			
			// Refresh any generated strings.
			if (newType == 2 || optXwingStringPlaceholder.Checked) refreshXwingTabStrings();

			Platform.Xwing.BriefingPage page = new Platform.Xwing.BriefingPage
			{
				PageType = (short)pageType,
				Waypoint = (short)waypoint
			};
			core.Pages.Add(page);

			AbstractBriefing brf = new AbstractBriefing();
			int duration = _ticksPerSecond * 2;
			if (newType == 0)
			{
				brf.Events.Add(new AbstractEvent(0, AbstractEventType.MoveMap, 0, 0));
				brf.Events.Add(new AbstractEvent(0, AbstractEventType.ZoomMap, 48, 48));
			}
			else if (newType == 1)
			{
				brf.Events.Add(new AbstractEvent(0, AbstractEventType.TitleText, (short)cboXwingTitle.SelectedIndex));
				brf.Events.Add(new AbstractEvent(0, AbstractEventType.CaptionText, (short)cboXwingCaption.SelectedIndex));
			}
			else if (newType == 2)
			{
				brf.Events.Add(new AbstractEvent(0, AbstractEventType.TitleText, (short)hintTitle));
				brf.Events.Add(new AbstractEvent(0, AbstractEventType.CaptionText, (short)hintCaption));
				brf.Events.Add(new AbstractEvent(350, AbstractEventType.PageBreak));
				brf.Events.Add(new AbstractEvent(350, AbstractEventType.TitleText, (short)hintTitle));
				brf.Events.Add(new AbstractEvent(350, AbstractEventType.CaptionText, (short)cboXwingCaption.SelectedIndex));
				duration = (_ticksPerSecond * 71);
			}
			brf.RunningTime = (short)duration;
			_briefingList.Add(brf);
			markDirty();
			rebuildXwingBriefingList();
			refreshXwingTabPageTemplates();
			lstXwingPages.SelectedIndex = lstXwingPages.Items.Count - 1;
		}

		private void lstXwingPages_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool btemp = _loading;
			_loading = true;
			refreshXwingTabButtons();
			if (lstXwingPages.SelectedIndex >= 0)
			{
				var core = getXwingCoreBriefing();
				safeSetCbo(cboXwingPageTypes, core.Pages[lstXwingPages.SelectedIndex].PageType);
				safeSetNumeric(numXwingWaypointSet, core.Pages[lstXwingPages.SelectedIndex].Waypoint + 1);
			}
			_loading = btemp;
		}

		private void cboXwingPageType_SelectedIndexChanged(object sender, EventArgs e)
		{
			int pageIndex = lstXwingPages.SelectedIndex;
			if (_loading || pageIndex == -1) return;

			getXwingCoreBriefing().Pages[pageIndex].PageType = (short)cboXwingPageTypes.SelectedIndex;
			rebuildXwingBriefingList();
			refreshXwingTabPageTemplates();
			lstXwingPages.SelectedIndex = pageIndex;
			markDirty();
		}

		private void numXwingWaypointSet_ValueChanged(object sender, EventArgs e)
		{
			if (_loading || lstXwingPages.SelectedIndex == -1) return;

			getXwingCoreBriefing().Pages[lstXwingPages.SelectedIndex].Waypoint = (short)(numXwingWaypointSet.Value - 1);
			markDirty();
		}

		private void cboXwingNewType_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = cboXwingNewType.SelectedIndex;
			bool use = optXwingStringUse.Checked;
			cboXwingTitle.Enabled = (index == 1 && use);
			lblXwingTitle.Enabled = (index == 1 && use);
			cboXwingCaption.Enabled = (index >= 1 && use);
			lblXwingCaption.Enabled = (index >= 1 && use);
			optXwingStringUse.Enabled = (index != 0);
			optXwingStringPlaceholder.Enabled = (index != 0);
		}

		private void optXwingStringUse_CheckedChanged(object sender, EventArgs e) => cboXwingNewType_SelectedIndexChanged(sender, e);

		private void numXwingMaxWaypoint_ValueChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			var core = getXwingCoreBriefing();
			short oldCount = core.MaxWaypoints;
			short newCount = (short)numXwingMaxWaypoint.Value;

			string error = "";
			if (newCount < oldCount)
			{
				for (int i = 0; i < core.Pages.Count; i++)
				{
					if (core.Pages[i].Waypoint != oldCount - 1) continue;

					if (error != "") error += ", ";
					error += "Page #" + (i + 1);
				}
			}
			if (error != "")
			{
				popupError("Can't remove this waypoint.\n\nIt is currently in use by: " + error);

				// Revert to old value.
				_loading = true;
				numXwingMaxWaypoint.Value = oldCount;
				_loading = false;
				return;
			}

			core.MaxWaypoints = newCount;

			if (numXwingWaypointSet.Value > newCount) numXwingWaypointSet.Value = newCount;
			numXwingWaypointSet.Maximum = newCount;
			markDirty();
		}

		private void chkXwingSurfaceMission_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;

			getXwingCoreBriefing().MissionLocation = chkXwingSurfaceMission.Checked;
			markDirty();
		}
		#endregion Xwing tab events

		#region Verify tab events
		private void cmdVerify_Click(object sender, EventArgs e)
		{
			exportActiveBriefing();
			verifyBriefing(false, false);
		}

		private void cmdRepair_Click(object sender, EventArgs e)
		{
			exportActiveBriefing();
			repairBriefing();
			importActiveBriefing();
		}
		#endregion Verify tab events
		
		#region External import classes
		/// <summary>Utility class for converting a craft type of one platform to another.</summary>
		/// <remarks>This conversion is specific to briefing icons.  There isn't a proper distinction between XvT and BoP.</remarks>
		class IconConvert
		{
			#region Public functions
			/// <summary>Converts a craft type of one platform to an equivalent or similar craft type of another, regardless of upgrade or downgrade path.</summary>
			static public int Convert(int craftType, Settings.Platform startPlatform, Settings.Platform endPlatform)
			{
				if (startPlatform == endPlatform) return craftType;

				int start = _platformMap[startPlatform];
				int end = _platformMap[endPlatform];
				if (start < end) for (int i = start; i < end; i++) craftType = _upgradePath[i](craftType);
				else
				{
					// Flip the indices so XWA is first, XWING is last
					start = _downgradePath.Length - start;
					end = _downgradePath.Length - end;
					for (int i = start; i < end; i++) craftType = _downgradePath[i](craftType);
				}

				return craftType;
			}
			#endregion

			#region Private conversion data
			static readonly byte[] _xwingToTie = {
				1, 1,    // X-W
				2, 2,    // Y-W
				3, 3,    // A-W
				4, 5,    // T/F
				5, 6,    // T/I
				6, 7,    // T/B
				7, 16,   // GUN
				8, 21,   // TRN
				9, 17,   // SHU
				10, 24,  // TUG
				11, 26,  // CON
				12, 32,  // FRT
				13, 49,  // CRS
				14, 42,  // FRG
				15, 40,  // CRV
				16, 53,  // STD
				17, 8,   // T/A
				18, 75,  // Mine
				22, 70,  // Sat
				23, 83,  // Nav
				24, 80,  // Probe
				25, 4,   // B-W
				26, 86,  // Asteroid
				34, 87,  // Planet
			};

			/// <summary>XWING doesn't downgrade, so this condenses XWING exclusive icons into values that can be upgraded to TIE.</summary>
			static readonly byte[] _trimXwing = {
				18, 21, 18,   // All mines to first
				26, 33, 26,   // All asteroids
				34, 57, 34,   // All planets
				58, 108, 22,  // All other objects become Sat
			};

			/// <summary>Condenses TIE ships so it can be downgraded to XWING.</summary>
			static readonly byte[] _trimTie = {
				8, 12, 8,    // T/A, T/D, 10, 11, MIS
				13, 15, 1,   // T-W, Z-95, R-41 to X-W
				17, 20, 17,  // SHU, E/S, SPC, SCT
				21, 23, 21,  // TRN, ATR, ETR
				24, 25, 24,  // TUG, CUV
				26, 29, 26,  // CN/A through CN/D
				30, 31, 21,  // HLF and 31 to TRN
				32, 36, 32,  // FRT, CARG, CNVYR, CTRNS, 36
				37, 39, 21,  // MUTR, CORT, unused to TRN
				40, 41, 40,  // CRV, M/CRV
				42, 43, 42,  // FRG, M/FRG
				44, 44, 32,  // LINER becomes FRT
				45, 48, 42,  // CRCK, STRKC, ESC, DREAD to FRG
				49, 50, 49,  // CRS, CRL
				51, 54, 53,  // INT, VSD, ISD, *SSD to ISD
				55, 59, 26,  // CN/E through CN/I to CN/A
				60, 69, 42,  // PLT1-6, asteroid bases, FAC to FRG
				70, 74, 70,  // All sats and unused
				75, 79, 75,  // All mines and unused
				80, 82, 80,  // All probes and unused
				83, 85, 83,  // Navs
			};

			/// <summary>Condenses BoP ships so it can be downgraded to XvT.  XvT is directly compatible with TIE.</summary>
			static readonly byte[] _trimXvt = {
				78, 78, 75,  // GPLT to Mine
				87, 89, 87,  // Planet and unused
				90, 91, 69,  // SHPYD and REPYD to FAC/1
				92, 92, 46,  // M/SC to STRKC
			};

			/// <summary>Condenses XWA ships so it can be downgraded to XvT.</summary>
			static readonly byte[] _trimXwa = {
				10, 11, 1,    // IRD, TOS become X-W
				31, 31, 30,   // MINER becomes HLF
				39, 39, 38,   // FALC becomes CORT
				71, 74, 71,   // Sat/2 and unused
				79, 79, 78,   // *MN/5 to GPLT
				82, 82, 81,   // *PRB/C to PRB/B
				85, 85, 83,   // HYP to NAV
				88, 88, 83,   // RDV to NAV
				89, 89, 26,   // C/C to CN/A
				93, 95, 43,   // L/FRG, B/CRS, A/FRG to M/FRG
				96, 96, 41,   // GSP to M/CRV
				97, 98, 18,   // L/C, A/S to E/S
				99, 99, 43,   // M-CRV to M/FRG
				100, 100, 47, // STARG to ESC
				101, 101, 43, // SRS to M/FRG
				102, 109, 32, // LY3K, F/L, ActVI, MOB, Xiy/T, Frt/C/H/K to FRT
				110, 111, 38, // YT 2000/2400 to CORT
				112, 112, 32, // S/M to FRT
				113, 113, 16, // S/B to GUN
				114, 118, 5,  // T/e1-5 to T/F
				119, 123, 1,  // Clk/F, Rzr/F, Plt/F, Sup/F, Pnk/F to X-W
				124, 124, 24, // B/R to TUG
				125, 126, 1,  // Pry/F, *Xiz/F to X-W
				127, 128, 21, // FRS, PES to TRN
				129, 149, 60, // Golans, bases, etc to PLT/1
				150, 150, 24, // E-POD to tug
				151, 155, 26, // Pr/Tk, CN/J/K/L/HGR to CON/A
				156, 163, 78, // Gun platforms to GPLT
				164, 173, 32, // CF1-5, CT1-5 to FRT
				174, 182, 24, // E-POD2, ZG/S, pilots to TUG
				183, 227, 87, // B/drop to planet
				228, 228, 49, // WCRS to CRS
				229, 229, 52, // VSD2 to VSD
				230, 232, 53, // ISD2 and unused to ISD
			};
			#endregion

			#region Private functions
			/// <summary>Given an array of data with two columns, search the value on the left and return the value on the right.</summary>
			/// <returns>If value equals A, return B.  If not found, returns either the input value or zero.</returns>
			static int resolveValueRight(int value, byte[] data, bool returnZero)
			{
				int rows = data.Length / 2;
				for (int i = 0; i < rows; i++)
				{
					int baseOffset = i * 2;
					if (data[baseOffset] == value) return data[baseOffset + 1];
				}
				return (returnZero ? 0 : value);
			}
			/// <summary>Given an array of data with two columns, search the value on the right and return the value on the left.</summary>
			/// <returns>If value equals B, return A.  If not found, returns either the input value or zero.</returns>
			static int resolveValueLeft(int value, byte[] data, bool returnZero)
			{
				int rows = data.Length / 2;
				for (int i = 0; i < rows; i++)
				{
					int baseOffset = i * 2;
					if (data[baseOffset + 1] == value) return data[baseOffset];
				}
				return (returnZero ? 0 : value);
			}

			/// <summary>Given an array of data with three columns, match the value between the ranges in the first and second column (inclusive) and return the value in the third column.</summary>
			/// <returns>If value is between A and B (inclusive), return C.  If not found, returns the input value.</returns>
			static int trimValue(int value, byte[] data)
			{
				int rows = data.Length / 3;
				for (int i = 0; i < rows; i++)
				{
					int baseOffset = i * 3;
					if (value >= data[baseOffset + 0] && value <= data[baseOffset + 1]) return data[baseOffset + 2];
				}
				return value;
			}

			static int xwingToTie(int craftType)
			{
				int value = trimValue(craftType, _trimXwing);
				return resolveValueRight(value, _xwingToTie, true);
			}
			static int tieToXvt(int craftType) => craftType;
			static int xvtToXwa(int craftType) => craftType;

			static int tieToXwing(int craftType)
			{
				int value = trimValue(craftType, _trimTie);
				return resolveValueLeft(value, _xwingToTie, true);
			}
			static int xvtToTie(int craftType) => trimValue(craftType, _trimXvt);
			static int xwaToXvt(int craftType) => trimValue(craftType, _trimXwa);

			delegate int convertFunc(int craftType);
			static readonly convertFunc[] _upgradePath = new convertFunc[] { xwingToTie, tieToXvt, xvtToXwa };
			static readonly convertFunc[] _downgradePath = new convertFunc[] { xwaToXvt, xvtToTie, tieToXwing };
			static readonly Dictionary<Settings.Platform, int> _platformMap = new Dictionary<Settings.Platform, int> {
				{ Settings.Platform.XWING, 0 },
				{ Settings.Platform.TIE, 1 },
				{ Settings.Platform.XvT, 2 },
				{ Settings.Platform.BoP, 2 },  // Same as XvT
				{ Settings.Platform.XWA, 3 }
			};
			#endregion
		}

		/// <summary>Provides a compatibility layer for selective data storage between different missions, of potentially different platforms.</summary>
		/// <remarks>This data can be populated by special functions to load selective data from a briefing exported as text, or from whole mission files.</remarks>
		class ImportData
		{
			public List<AbstractBriefing> Briefings = new List<AbstractBriefing>();
			public List<AbstractFlightgroup> Flightgroups = new List<AbstractFlightgroup>();
			public List<Platform.Xwing.PageTemplate> XwingPageTemplates = new List<Platform.Xwing.PageTemplate>();
			public int XwingPageWaypoint = 0;
			public bool IsTextImport = false;
			public Section TextSections = Section.None;

			[Flags]
			public enum Section
			{
				None         =  0x0,
				Strings      =  0x1,
				Tags         =  0x2,
				Events       =  0x4,
				Flightgroups =  0x8,
				Visibility   = 0x10,
				PageTypes    = 0x20,
				All          = 0xFF,
			};

			public void AddSection(Section section) => TextSections |= section;
			public bool HasSection(Section section) => (!IsTextImport || TextSections.HasFlag(section));
			public bool HasFlightgroups() => HasSection(Section.Flightgroups) && Flightgroups.Count > 0;

			/// <summary>Inverts all vertical positions of flightgroups and text tags.</summary>
			/// <param name="raw">This should be true only when finalizing import data loaded from a binary mission file.</param>
			public void InvertWaypoints(bool raw)
			{
				foreach (var afg in Flightgroups)
					foreach (var wp in afg.Waypoints) wp.RawY *= -1;

				// If loaded from a mission, the platform loader flips the waypoints but not the text tags.
				if (raw) return;

				foreach (var brf in Briefings)
					foreach (var evt in brf.Events) if (evt.IsTextTag) evt.Params[2] *= -1;
			}
		}
		#endregion External import classes

		#region Supporting types
		/// <summary>Flags used to trigger bitmap canvas refreshes.</summary>
		[Flags]
		enum RefreshFlags
		{
			None     = 0,
			Map      = 1,
			Title    = 2,
			Caption  = 4,
			Panel3   = 8,         // Only for XWING
			Panel4   = 16,        // Only for XWING
			Timeline = 32,
			Effect   = 64,        // Any map transition effect drawn over the combined canvas

			All = Map | Title | Caption | Timeline | Panel3 | Panel4,  // Everything except effects
			TextPanels = Title | Caption | Panel3 | Panel4,
			XwingPanels = Map | TextPanels,
		}

		/// <summary>Contains a few common colors used for general text or panel.</summary>
		/// <remarks>If this order changes, or items are added, change the arrays assigned in <see cref="generateColors"/></remarks>
		enum StandardColor
		{
			Default = 0,  // Standard white text color
			Highlight,    // Highlighting with [brackets]
			Center,       // Center prefix with >
			Panel,        // Panel background
		}

		/// <summary>Mouse state behavior for click and drag operations, depending on context.</summary>
		enum MouseDragState
		{
			None = 0,
			MapBoundClick,
			MapBoundDrag,
			MapLookClick,
			MapLookDrag,
			MapObjectClick,
			MapObjectDrag,

			TimelineBoundClick,
			TimelineBoundDrag,
			TimelineObjectClick,
			TimelineObjectDrag,
			TimelineDrag,

			PanelResizeClick,
			PanelResizeDrag,
			PanelMoveClick,
			PanelMoveDrag,

			DoubleClick
		};
		
		/// <summary>These should correspond to index sequence of tab pages within <see cref="tabBriefing"/></summary>
		enum MainTabIndex
		{
			BriefingMap = 0,
			MainStrings,
			EventList,
			BriefingOptions,
			EditorOptions,
			XwingPages,
			Verify,
		}

		/// <summary>Strings and tags are edited via the same functions.  This determines the container and properties to use.</summary>
		enum BriefingString
		{
			None = 0,
			String,
			Tag,
		}

		/// <summary>This holds all briefing event types, mapped to abstracted platform-independent values.</summary>
		/// <remarks>The actual values here aren't important, only that they're distinct.  For convenience, most of them correspond to their TIE/XvT/XWA values.  XWING requires special handling.</remarks>
		enum AbstractEventType
		{
			None = 0,
			SkipMarker = 1,
			Unknown2 = 2,
			PageBreak = 3,
			TitleText = 4,      // StringIndex
			CaptionText = 5,    // StringIndex
			MoveMap = 6,        // X, Y
			ZoomMap = 7,        // X, Y
			ClearFgTags = 8,
			FgTag1 = 9,         // FGIndex
			FgTag2 = 10,
			FgTag3 = 11,
			FgTag4 = 12,
			FgTag5 = 13,        // XWING95 does not contain #5 to #8
			FgTag6 = 14,
			FgTag7 = 15,
			FgTag8 = 16,
			ClearTextTags = 17,
			TextTag1 = 18,      // TagIndex, X, Y, Color
			TextTag2 = 19,      // XWING95 does not have tag colors, format is [TagIndex, X, Y]
			TextTag3 = 20,
			TextTag4 = 21,
			TextTag5 = 22,      // XWING95 does not contain #5 to #8
			TextTag6 = 23,
			TextTag7 = 24,
			TextTag8 = 25,

			// These values are exclusive to XWA
			XwaSetIcon = 26,       // IconNum, CraftType, IFF
			XwaShipInfo = 27,      // State, IconNum
			XwaMoveIcon = 28,      // IconNum, X, Y
			XwaRotateIcon = 29,    // IconNum, Rotation
			XwaChangeRegion = 30,  // RegionNum
			XwaInfoParagraph = 31, // StringIndex

			EndBriefing = 34,      // Not available for edit, removed on import, added on export.

			// These values are exclusive to XWING95
			PanelText3 = 40,
			PanelText4 = 41,
			UnknownClear = 42,
			UnknownEffect = 43,
			XwingNewIcon = 44,  // Special case, not an event, allows creation of flightgroups.
		}

		/// <summary>All stages of visual effects used by XWA map screen transitions.</summary>
		/// <remarks>Named sequences (ABCD) help distinguish their sequential order.</remarks>
		enum MapEffect
		{
			None = 0,
			Initializing_MapWipeIn,
			RegionA_MapWipeOut,
			RegionB_TextFadeIn,
			RegionC_TextFadeOut,
			RegionD_MapWipeIn,
			ShipInfoA_IconGrow,
			ShipInfoB_ScreenSweep,
			ShipInfoC_MeshRotate,
			ShipInfoD_IconShrink,
		}

		/// <summary>All types of items that can be selected on the map or timeline.</summary>
		enum SelectedType
		{
			None = 0,
			Event = 1,      // Everything that exists in the briefing event list.
			Waypoint = 2,   // Flightgroup waypoints used in XWING, TIE, XvT.  Drawn as icons on the map, but are not events.
			PathNode = 3,   // For XWA icon generation.  Does not correspond to any briefing or mission data.
		};

		/// <summary>All parameters used by events with dedicated sidebar controls for editing purposes.</summary>
		/// <remarks>Allows value changes to be performed correctly regardless of multi-selection and event types.</remarks>
		enum EditParamType
		{
			X = 0,
			Y,
			String,
			FgIndex,
			TagEventType,   // The event type for a FGTag(1-8) or TextTag(1-8) is being changed.
			TextColor,
			IconIndex,
			IconCraftType,
			IconCraftIff,
			IconCraftRotation,
			CraftInfoState,
			Region,
		}

		/// <summary>Items and order must correspond to the dropdown collection in <see cref="cboPathAutoType"/></summary>
		enum PathAutoType
		{
			TimeSec = 0,
			TimeTick = 1,
			DistPixel = 2,
			DistMeter = 3,
			DistRaw = 4,
		}

		/// <summary>All revert operations used by the Undo/Redo system.</summary>
		[Flags]
		enum UndoType
		{
			None              =      0,
			FlightgroupMove   =    0x1,  // XWING, TIE, or XvT briefing waypoint changed.
			Event             =    0x2,  // Briefing event changed (time, event, or any of its params).
			SwapEvent         =    0x4,  // Two briefing events swapped list positions.  The events themselves were not modified.
			TimeShift         =    0x8,  // A time shift as performed on the timeline strip.  Unlike the time shift in the event list tab, it is not based on selection.
			Duration          =   0x10,  // The assigned briefing duration was changed.
			NewEvent          =   0x20,  // Briefing event inserted into list.
			DeleteEvent       =   0x40,  // Briefing event removed from list.
			String            =   0x80,  // Briefing string (caption) changed.
			Tag               =  0x100,  // Briefing tag changed.
			SwapString        =  0x200,  // Two briefing strings swapped list positions.
			SwapTag           =  0x400,  // Two briefing tags swapped list positions.
			SwapNote          =  0x800,  // Two XWA caption string notes swapped list positions.
			// NOTE: These had to be removed.  It's not enough to synchronize changes to the main form,
			// the main form also has to synchronize here.  The state must be fully and correctly tracked,
			// otherwise reverts can operate on the wrong data.
			//NewFlightgroup    = 0x1000,  // XWING flightgroup inserted to list
			//DeleteFlightgroup = 0x2000,  // XWING flightgroup removed from list
			FlightgroupData   = 0x4000,  // XWING flightgroup data changed, not including waypoints.  For name, craft type, IFF, etc.
		}

		/// <summary>Holds a copy of state changes for any arbitrary data.</summary>
		class UndoOperation
		{
			public UndoType OpType;
			public int Index;         // A string index, event index, etc.  Depends on the type.
			public int SubIndex;      // Extra value, only needed for specific types.
			public object OldData;    // Original unmodified data packet.
			public object NewData;    // The newest data packet assigned, or null if never updated.

			/// <summary>All operations will have a unique ID.</summary>
			public int UID { get; private set; }
			private static int _nextUID = 1;

			public UndoOperation(UndoType type, object data, int index, int subindex = -1)
			{
				OpType = type;
				OldData = data;
				NewData = null;
				Index = index;
				SubIndex = subindex;
				UID = _nextUID++;
			}

			/// <summary>Provides a new data packet for a value change.</summary>
			public void Update(object data) => NewData = data;

			/// <summary>Notifies a value as updated, but retains the existing data packet.  Necessary if the data packet is irrelevant.</summary>
			public void Update() => NewData = OldData;

			/// <summary>Returns whether the data packet was ever updated.</summary>
			public bool HasUpdated() => (NewData != null);

			/// <summary>Retrieves the data packet assigned to the state.</summary>
			public object GetData(bool redo) => (redo ? NewData : OldData);
		}

		class AbstractEvent
		{
			public short Time = 0;
			public AbstractEventType Event = AbstractEventType.None;
			public short[] Params = new short[4];

			/// <summary>The UID allows locating a specific event no matter where it might be in the list.</summary>
			public int UID { get; private set; }
			private static int _nextUID = 1;

			public AbstractEvent() => UID = _nextUID++;

			public AbstractEvent(short time, AbstractEventType evtType) : this()
			{
				Time = time;
				Event = evtType;
			}

			public AbstractEvent(short time, AbstractEventType evtType, params short[] evtParams) : this(time, evtType)
			{
				for (int i = 0; i < evtParams.Length; i++) Params[i] = evtParams[i];
			}

			public AbstractEvent(AbstractEvent other) : this() => CopyFrom(other);

			/// <summary>Return a copy of the data meant to be used and understood by undo operations.</summary>
			public AbstractEvent GetDataSnapshot() => new AbstractEvent(this);

			public void CopyFrom(AbstractEvent other)
			{
				Time = other.Time;
				Event = other.Event;
				for (int i = 0; i < Params.Length; i++) Params[i] = other.Params[i];
				// Don't copy the UID.
			}

			public void Swap(AbstractEvent other)
			{
				AbstractEvent temp = new AbstractEvent(this);
				CopyFrom(other);
				other.CopyFrom(temp);

				// Swap is a special operation, it's the only thing that modifies UID, since nothing is being created or destroyed.
				int uid = UID;
				UID = other.UID;
				other.UID = uid;
			}

			public bool IsAnyCaption => (Event == AbstractEventType.CaptionText || Event == AbstractEventType.TitleText || Event == AbstractEventType.PanelText3 || Event == AbstractEventType.PanelText4);
			public bool IsTextTag => (Event >= AbstractEventType.TextTag1 && Event <= AbstractEventType.TextTag8);
			public bool IsFgTag => (Event >= AbstractEventType.FgTag1 && Event <= AbstractEventType.FgTag8);

			// Not really necessary, but it's nice to have a readable string in the debugger.
			public override string ToString()
			{
				string ret = "";
				var def = EventDef.GetEventDefByType(Event);
				if (def != null) ret = Time + ": " + def.Name;
				return ret;
			}
		}

		/// <summary>Stores conversion, mapping, name, and output information for all resulting abstract event types.</summary>
		class EventDef
		{
			public readonly AbstractEventType Type;
			public readonly short RawEvent;    // Raw event type used in TIE, XvT, XWA
			public readonly int NumParams;     // Number of event parameters
			public readonly short RawEventXW;  // Raw event type used in XWING
			public readonly int NumParamsXW;   // Number of event parameters used in XWING
			public readonly int ParamClass;    // If nonzero, indicates events with similar parameters.  If changing the event type, this determines if the parameters should be wiped.
			public readonly string Name;       // Name as it appears in the event type dropdown on the event tab
			public readonly string Abbrev;     // The abbreviated string used in the timeline
			public EventDef(AbstractEventType type, short rawevent, int numparams, short raweventxw, int numparamsxw, int paramClass, string name, string abbrev)
			{
				Type = type;
				RawEvent = rawevent;
				NumParams = numparams;
				RawEventXW = raweventxw;
				NumParamsXW = numparamsxw;
				ParamClass = paramClass;
				Name = name;
				Abbrev = abbrev;
			}

			/// <summary>Retrieves how much event space is required to hold this event.</summary>
			/// <remarks>The only size difference is the text tag in XWING, which is one less parameter.</remarks>
			public int GetSize() => 2 + NumParams;

			private static readonly EventDef[] _eventList =
			{
				//                        Type.Value            Raw  Num  RawX NumX Cl  Name    Abbrev
				new EventDef(AbstractEventType.None,              0,   0,    0,  0,  0, "None", "None"),
				new EventDef(AbstractEventType.SkipMarker,        1,   0,    1,  0,  0, "Marker", "Mark"),
				new EventDef(AbstractEventType.Unknown2,          2,   0,    0,  0,  0, "Unknown", "Unk:2"),
				new EventDef(AbstractEventType.PageBreak,         3,   0,   10,  0,  0, "Page Break", "Pg"),
				new EventDef(AbstractEventType.TitleText,         4,   1,   11,  1,  1, "Title Text", "Title"),
				new EventDef(AbstractEventType.CaptionText,       5,   1,   12,  1,  1, "Caption Text", "Cap"),
				new EventDef(AbstractEventType.MoveMap,           6,   2,   15,  2,  0, "Move Map", "Mv"),
				new EventDef(AbstractEventType.ZoomMap,           7,   2,   16,  2,  0, "Zoom Map", "Zm"),
				new EventDef(AbstractEventType.ClearFgTags,       8,   0,   21,  0,  0, "Clear FG Tags", "CFG"),
				new EventDef(AbstractEventType.FgTag1,            9,   1,   22,  1,  2, "FG Tag 1", "Fg.1"),
				new EventDef(AbstractEventType.FgTag2,           10,   1,   23,  1,  2, "FG Tag 2", "Fg.2"),
				new EventDef(AbstractEventType.FgTag3,           11,   1,   24,  1,  2, "FG Tag 3", "Fg.3"),
				new EventDef(AbstractEventType.FgTag4,           12,   1,   25,  1,  2, "FG Tag 4", "Fg.4"),
				new EventDef(AbstractEventType.FgTag5,           13,   1,    0,  0,  2, "FG Tag 5", "Fg.5"),
				new EventDef(AbstractEventType.FgTag6,           14,   1,    0,  0,  2, "FG Tag 6", "Fg.6"),
				new EventDef(AbstractEventType.FgTag7,           15,   1,    0,  0,  2, "FG Tag 7", "Fg.7"),
				new EventDef(AbstractEventType.FgTag8,           16,   1,    0,  0,  2, "FG Tag 8", "Fg.8"),
																			 
				new EventDef(AbstractEventType.ClearTextTags,    17,   0,   26,  0,  0, "Clear Text Tags", "CTX"),
				new EventDef(AbstractEventType.TextTag1,         18,   4,   27,  3,  3, "Text Tag 1", "Tx.1"),
				new EventDef(AbstractEventType.TextTag2,         19,   4,   28,  3,  3, "Text Tag 2", "Tx.2"),
				new EventDef(AbstractEventType.TextTag3,         20,   4,   29,  3,  3, "Text Tag 3", "Tx.3"),
				new EventDef(AbstractEventType.TextTag4,         21,   4,   30,  3,  3, "Text Tag 4", "Tx.4"),
				new EventDef(AbstractEventType.TextTag5,         22,   4,    0,  0,  3, "Text Tag 5", "Tx.5"),
				new EventDef(AbstractEventType.TextTag6,         23,   4,    0,  0,  3, "Text Tag 6", "Tx.6"),
				new EventDef(AbstractEventType.TextTag7,         24,   4,    0,  0,  3, "Text Tag 7", "Tx.7"),
				new EventDef(AbstractEventType.TextTag8,         25,   4,    0,  0,  3, "Text Tag 8", "Tx.8"),
																			 
				new EventDef(AbstractEventType.XwaSetIcon,       26,   3,    0,  0,  0, "New Icon", "Icon"),
				new EventDef(AbstractEventType.XwaShipInfo,      27,   2,    0,  0,  0, "Ship Info", "Info"),
				new EventDef(AbstractEventType.XwaMoveIcon,      28,   3,    0,  0,  0, "Move Icon", "<>"),
				new EventDef(AbstractEventType.XwaRotateIcon,    29,   2,    0,  0,  0, "Rotate Icon", "@" ),
				new EventDef(AbstractEventType.XwaChangeRegion,  30,   1,    0,  0,  0, "Switch Region", "Rg"),
				new EventDef(AbstractEventType.XwaInfoParagraph, 31,   1,    0,  0,  0, "Ship Info Text", "InfoTxt"),
																		 
				new EventDef(AbstractEventType.PanelText3,        0,   0,   13,  1,  1, "Panel Text 3", "Pan3"),
				new EventDef(AbstractEventType.PanelText4,        0,   0,   14,  1,  1, "Panel Text 4", "Pan4"),
				new EventDef(AbstractEventType.UnknownClear,      0,   0,   31,  0,  0, "Unknown Clear", "CL?"),
				new EventDef(AbstractEventType.UnknownEffect,     0,   0,   32,  3,  0, "Unknown Effect", "???"),
																			 
				new EventDef(AbstractEventType.EndBriefing,      34,   0,   41,  0 , 0, "End Briefing", "End")
			};

			static public EventDef GetEventDefByRaw(short rawEvent)
			{
				foreach (EventDef def in _eventList) if (def.RawEvent == rawEvent) return def;

				return null;
			}

			static public EventDef GetEventDefByRawXwing(short rawEvent)
			{
				foreach (EventDef def in _eventList) if (def.RawEventXW == rawEvent) return def;

				return null;
			}

			static public EventDef GetEventDefByType(AbstractEventType type)
			{
				foreach (EventDef def in _eventList) if (def.Type == type) return def;

				return null;
			}

			static public EventDef GetEventDefByName(string name)
			{
				foreach (EventDef def in _eventList) if (def.Name == name) return def;

				return _eventList[0];  // None
			}

			/// <summary>Retrieve via lookup of the abstract event type (enum).</summary>
			static public EventDef GetEventDefByInternalName(string name)
			{
				foreach (EventDef def in _eventList) if (def.Type.ToString() == name) return def;

				return _eventList[0];  // None
			}
		}

		/// <summary>Tracks information about a selected object: what it is, where to find it, and how to interact with it.</summary>
		class SelectedObject
		{
			public SelectedType Type = SelectedType.None;
			public int Index = 0;                 // Index into the appropriate list, based on type.
			public int EventUID = 0;              // Allows tracking a specific event even if it changes its original list position.
			public Rectangle InteractRect = new Rectangle();  // Where to display selection brackets and accept mouse clicks.
			public int UndoUID = 0;               // Allows continuous updates for certain objects to maintain a single operation.
			public int MouseClickPriority = 0;    // When selecting via bounding box, priority to help resolve overlaps.

			public SelectedObject(SelectedType type, int index, int eventuid)
			{
				Type = type;
				Index = index;
				EventUID = eventuid;
				UndoUID = 0;
			}
			public SelectedObject(SelectedType type, int index, int eventuid, int clickPriority) : this(type, index, eventuid) => MouseClickPriority = clickPriority;

			/// <summary>Returns true if selected object references the same thing as another selected object.  UI properties are not compared.</summary>
			public bool IsEqual(SelectedObject other) => (Type == other.Type && Index == other.Index && EventUID == other.EventUID);
			/// <summary>Comparison to sort objects by ascending type and index.</summary>
			public static int Compare(SelectedObject left, SelectedObject right)
			{
				if (left.Type < right.Type)	return -1;
				else if (left.Type > right.Type) return 1;

				if (left.Index < right.Index) return -1;
				else if (left.Index > right.Index) return 1;

				return 0;
			}

			/// <summary>Comparison to sort objects by ascending type but descending index.</summary>
			public static int CompareDescending(SelectedObject left, SelectedObject right)
			{
				if (left.Type < right.Type) return -1;
				else if (left.Type > right.Type) return 1;

				if (left.Index > right.Index) return -1;
				else if (left.Index < right.Index) return 1;

				return 0;
			}

			/// <summary>Comparison to sort objects by ascending click priority.</summary>
			public static int CompareClickPriority(SelectedObject left, SelectedObject right)
			{
				if (left.MouseClickPriority < right.MouseClickPriority) return -1;
				else if (left.MouseClickPriority > right.MouseClickPriority) return 1;

				return 0;
			}
		}

		/// <summary>Stores an icon bitmap and related information for a single craft type.</summary>
		/// <remarks>Once loaded from application or game assets, this information doesn't change.  This is the base image for tinting or rotating, if required.</remarks>
		class CraftIconImage
		{
			public Rectangle OriginalRect;   // Position and dimensions from the original bitmap image it was sourced from
			public Rectangle Rect;           // Physical size, cropped of all transparent pixels
			public Bitmap Icon;              // The cropped source icon

			public int DrawOffsetX = 0;
			public int DrawOffsetY = 0;
			public int SquareSize = 0;       // The highest Width or Height
			public int Width;
			public int Height;
			public bool ShipList = false;    // For XWA, specifies if the icon rect was sourced from SHIPLIST.TXT
			public Size CropOffset;          // For XWA, needed to calculate correct draw offset
			public bool Hidden = false;      // For XWA, specifies if the shiplist species entry may be hidden.

			// XWA icons were drawing incorrectly in TFTC 1.3, and getting the best positions and sizes requires knowing the exact information in the ship list.
			public int GetDrawOffsetX() => (ShipList ? (((OriginalRect.Width + 1) / -2) + CropOffset.Width) : DrawOffsetX);
			public int GetDrawOffsetY() => (ShipList ? (((OriginalRect.Height + 1) / -2) + CropOffset.Height) : DrawOffsetY);
			public int GetWidth() => (ShipList ? OriginalRect.Width + 1 : Width);
			public int GetHeight() => (ShipList ? OriginalRect.Height + 1 : Height);
		}

		/// <summary>Stores important flightgroup information in a platform-independent abstract format.</summary>
		/// <remarks>Also stores interact information for icons that are currently visible on the map, for non-XWA platforms.</remarks>
		class AbstractFlightgroup
		{
			public string Name = "";
			public string DisplayName = "";         // Includes the abbreviation and name.
			public string Abbrev = "";
			public int CraftType;
			public int CraftIff;
			public int PlayerNumber = 0;            // Only for XvT.
			public int Team = 0;                    // Only for XvT.
			public BaseFlightGroup.Waypoint[] Waypoints = new BaseFlightGroup.Waypoint[8];

			public Point DisplayPos = new Point();  // Current display position, will match one of the briefing waypoints.  Required for XWING logic.
			public int DisplayWaypoint = 0;         // Index of the current waypoint, indicating which element is changed by modification.

			public Rectangle InteractRect = new Rectangle();  // Will be assigned if rendered to the map canvas.
			public int UndoUID = -1;                // Allows continuous updates to maintain a single operation.

			public AbstractFlightgroup() { for (int i = 0; i < Waypoints.Length; i++) Waypoints[i] = new BaseFlightGroup.Waypoint(); }

			public AbstractFlightgroup GetDataSnapshot()
			{
				AbstractFlightgroup afg = new AbstractFlightgroup();
				afg.CopyFrom(this);
				return afg;
			}

			public void CopyFrom(AbstractFlightgroup other)
			{
				Name = other.Name;
				DisplayName = other.DisplayName;
				Abbrev = other.Abbrev;
				CraftType = other.CraftType;
				CraftIff = other.CraftIff;
				for (int i = 0; i < Waypoints.Length; i++)
				{
					Waypoints[i].RawX = other.Waypoints[i].RawX;
					Waypoints[i].RawY = other.Waypoints[i].RawY;
					Waypoints[i].RawZ = other.Waypoints[i].RawZ;
					Waypoints[i].Enabled = other.Waypoints[i].Enabled;
				}
				// Undo operations for movement rely on the DisplayPos, so keep track of that too.
				DisplayPos = other.DisplayPos;
				DisplayWaypoint = other.DisplayWaypoint;
				// Nothing else needs to be copied.
			}
		}

		/// <summary>Assists in categorizing bitmaps when cached.</summary>
		enum BitmapCacheType
		{
			None = 0,
			CraftIcon = 1,
			FlatCraftIcon = 2,
			FontCaption = 3,
			FontTag = 4,
			FontCaptionHigh = 5,
			FontTagHigh = 6,
		}

		/// <summary>Used to track whether a particular asset was loaded from an original game resource, and where.</summary>
		enum AssetSourceType
		{
			/// <summary>Resource does not exist, failed to load, or was otherwise automatically generated.</summary>
			None = 0,

			/// <summary>Resource was loaded from a custom archive provided in the application's launch path.</summary>
			Application,

			/// <summary>Resource was loaded from a platform path defined in <see cref="Idmr.Yogeme.Settings"/></summary>
			Installation,

			/// <summary>Same as <see cref="Application"/>, but from a compatible resource intended for a different platform.</summary>
			/// <remarks>Applies to XWING and TIE which have mostly identical font sets.</remarks>
			AlternateApplication,

			/// <summary>Same as <see cref="Installation"/>, but from a compatible resource intended for a different platform.</summary>
			/// <remarks>Applies to XWING and TIE which have mostly identical font sets.</remarks>
			AlternateInstallation,

			/// <summary>Resource location was derived from the mission path, at a location that is typically different from <see cref="Idmr.Yogeme.Settings"/></summary>
			Mission
		}

		class PathNode
		{
			public int X;
			public int Y;
			public int LengthX;                 // Horizontal offset to next node
			public int LengthY;                 // Vertical offset to next node
			public int Time = 1;                // Time between steps, in raw briefing ticks
			public int Rotation = 0;            // Icon rotation to generate for this path
			public bool FirstRotation = true;   // If true, the rotation takes effect at the start of each node.  Otherwise it waits for the second icon on the path.
			public Rectangle InteractRect;      // Rendered location on the map, to determine user interaction
			public int Steps = 1;               // How many icons to place on the line segment between this node and the next.
			public int Length = 0;              // Raw distance to next node

			public float NodeSpacing => (Steps > 0 ? (float)Length / Steps : 0.0f);
		}

		/// <summary>Stores the information and bitmap for a single glyph.</summary>
		class BriefingGlyph
		{
			public Bitmap Image;
			public Rectangle Bounds;
			public int Width;
			public byte[] TagEncoding = null;
		}

		/// <summary>A specially loaded or converted font that meets the requirements for more complex text rendering.</summary>
		sealed class BriefingFont : IDisposable
		{
			/// <summary>Cache type to use.  Also identifies fonts for certain purposes, like high-def fonts in XWING.</summary>
			public readonly BitmapCacheType CacheType = BitmapCacheType.None;
			/// <summary>Pixels to add to horizontal offset when applying character spacing.  Official fonts use their own spacing.  It does not need to be adjusted.</summary>
			public int HorizontalSpacing { get; private set; } = 1;
			/// <summary>Pixels to add to vertical offset when applying a newline.  Some official fonts specify their own spacing.  Some, especially default fonts may require adjustment.</summary>
			public int VerticalSpacing = 1;
			/// <summary>Vertical line spacing as defined in the font file, or explicitly assigned for corrective purposes.</summary>
			public int Height = 0;
			/// <summary>Maximum width of a glyph bitmap.</summary>
			public int MaxWidth { get; private set; } = 0;
			/// <summary>Maximum height of a glyph bitmap.</summary>
			public int MaxHeight { get; private set; } = 0;
			/// <summary>Default system fonts typically report a much larger height than their appearance would indicate, so they need adjustment to line up where they should.</summary>
			public int VerticalOffset = 0;
			/// <summary>Whether the font has finished loading.</summary>
			public bool Loaded {get; private set; } = false;
			/// <summary>If loaded from a game asset, this is the path and filename of where it was loaded from.</summary>
			public string SourceFile { get; private set; } = string.Empty;
			/// <summary>General information of where the asset was loaded from, for reporting purposes.</summary>
			public AssetSourceType SourceType { get; private set; } = AssetSourceType.None;

			readonly Dictionary<char, BriefingGlyph> _characterSet = new Dictionary<char, BriefingGlyph>();
			readonly BriefingGlyph _glyphError = new BriefingGlyph();

			public BriefingFont(BitmapCacheType cacheType)
			{
				CacheType = cacheType;
				defaultInit();
			}

			public void Dispose()
			{
				_glyphError.Image?.Dispose();
				_glyphError.Image = null;
				foreach (var glyph in _characterSet) glyph.Value.Image?.Dispose();
				_characterSet.Clear();
			}

			/// <summary>Gets whether the font was successfully loaded from a file.</summary>
			/// <remarks>Generated defaults do not return successful.</remarks>
			public bool IsAssetLoaded() => (Loaded && SourceFile != "");

			/// <summary>Returns to an unloaded state.</summary>
			void unload()
			{
				Dispose();
				defaultInit();
			}

			void defaultInit()
			{
				generateInvalidGlyph(5, 12);
				Height = 12;
				SourceFile = string.Empty;
				SourceType = AssetSourceType.None;
				Loaded = false;
			}

			void generateInvalidGlyph(int width, int height)
			{
				// If a proper font is loaded, it needs to generate a new glyph.  Destroy the old one.
				_glyphError.Image?.Dispose();

				Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.Clear(Color.Black);
					g.DrawRectangle(Pens.White, 1, 1, bmp.Width - 2, bmp.Height - 2);
				}
				bmp.MakeTransparent(Color.Black);

				_glyphError.Image = bmp;
				_glyphError.Width = width;
				_glyphError.Bounds = new Rectangle(0, 0, width, height);
			}

			public BriefingGlyph GetChar(char c) => (_characterSet.ContainsKey(c) ? _characterSet[c] : _glyphError);

			public void LoadFromLfd(LfdFile lfd, string resource, bool shadow, AssetSourceType sourceType)
			{
				if (Loaded) return;

				try
				{
					loadFromLfdFont((LfdReader.Font)lfd.Resources[$"FONT{resource}"], shadow);
					SourceFile = lfd.FilePath;
					SourceType = sourceType;
					Loaded = true;
					/*foreach (Resource res in lfd.Resources)
					{
						if (string.Compare(res.Name, resource, StringComparison.OrdinalIgnoreCase) == 0)
						{
							loadFromLfdFont(res as LfdReader.Font, shadow);
							SourceFile = lfd.FilePath;
							SourceType = sourceType;
							Loaded = true;
							break;
						}
					}*/
				}
				catch
				{
					// If anything failed, completely reset to an unloaded state.
					unload();
				}
			}

			/// <summary>Generates a form of run-length encoding for use when rendering text tags.</summary>
			public void GenerateTagEncoding()
			{
				// Format:
				// 1 byte = number of scanlines (height)
				// foreach scanline
				//   int16 = Byte offset into encoding data array.
				//
				// For the encoding data, read in chunks of two bytes:
				// 1 byte = Length.  If zero, terminate.
				// 1 byte = Color.  If zero, transparent.  Otherwise a color intensity from 1 to 255.

				// Reserve space for data: number of scanlines, scanline offsets, and maximum character size.
				int encBufferSize = 1 + (MaxHeight * 2) + (MaxWidth * MaxHeight * 2);
				byte[] encBuffer = new byte[encBufferSize];
				
				foreach (var pair in _characterSet)
				{
					var glyph = pair.Value;
					if (glyph == null || glyph.Image == null || glyph.Image.Height == 0) continue;

					Array.Clear(encBuffer, 0, encBuffer.Length);
					int width = glyph.Image.Width;
					int height = glyph.Image.Height;

					encBuffer[0] = (byte)height;
					short encOffset = (short)(1 + (2 * height));

					Rectangle rect = new Rectangle(0, 0, width, height);
					var bmpData = glyph.Image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
					int stride = Math.Abs(bmpData.Stride);
					int surfLength = (bmpData.Height * stride) + bmpData.Width;
					byte[] surfData = new byte[surfLength];
					Marshal.Copy(bmpData.Scan0, surfData, 0, surfLength);

					for (int y = 0; y < height; y++)
					{
						// Write int16 data offset into header section.
						byte[] dataOffset = BitConverter.GetBytes(encOffset);
						encBuffer[(y * 2) + 1] = dataOffset[0];
						encBuffer[(y * 2) + 2] = dataOffset[1];

						int surfOffset = y * stride;

						byte blockLength = 0;
						byte blockColor = 0;
						for (int x = 0; x < width; x++)
						{
							byte color = surfData[surfOffset];
							surfOffset += 3;

							if (blockLength == 0)
							{
								// First pixel
								blockColor = color;
								blockLength = 1;
								continue;
							}
							else if (blockColor == color && ++blockLength < 0xFF) continue;

							// If we get here, it's a different color.
							// Or the block is max length, which should never happen, but let's handle it anyway.
							encBuffer[encOffset++] = blockLength;
							encBuffer[encOffset++] = blockColor;
							blockLength = 1;
							blockColor = color;
						}
						// Write the last block.  If there's nothing in it, it will just be zeroes, which is fine.
						encBuffer[encOffset++] = blockLength;
						encBuffer[encOffset++] = blockColor;
						// Terminate line.
						encBuffer[encOffset++] = 0;
						encBuffer[encOffset++] = 0;
					}
					glyph.Image.UnlockBits(bmpData);
					byte[] result = new byte[encOffset];
					Array.Copy(encBuffer, result, encOffset);
					glyph.TagEncoding = result;
				}
			}

			/// <summary>Converts a glyph bitmap to give it a shadow, as seen in XWING and TIE.</summary>
			/// <param name="bmp">The bitmap to modify.  Expects a grayscale 24bppRgb image.</param>
			void convertToShadow(Bitmap bmp)
			{
				Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
				var bmpData = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

				int surfSize = Math.Abs(bmpData.Stride) * bmpData.Height;
				byte[] surfData = new byte[surfSize];
				Marshal.Copy(bmpData.Scan0, surfData, 0, surfSize);

				// For all visible pixels, a shadow pixel is created at (X+1, Y+1)
				for (int y = 0; y < bmp.Height - 1; y++)
				{
					int srcOffset = y * bmpData.Stride;
					for (int x = 0; x < bmp.Width - 1; x++)
					{
						// Expects grayscale, so we only need to check one channel.
						// Since we can't use black for transparency, we'll use RGB(1,1,1) for the shadow.
						// Anything with a higher intensity is assumed to be a visible pixel.
						// Also make sure not to overwrite any other visible pixels.
						int thisIntensity = surfData[srcOffset];
						if (thisIntensity > 1)
						{
							int rightIntensity = surfData[srcOffset+3];
							if (rightIntensity == 0)
							{
								surfData[srcOffset+3] = 1;
								surfData[srcOffset+4] = 1;
								surfData[srcOffset+5] = 1;
							}

							int downOffset = srcOffset + bmpData.Stride;
							int downIntensity = surfData[downOffset];
							if (downIntensity == 0)
							{
								surfData[downOffset+0] = 1;
								surfData[downOffset+1] = 1;
								surfData[downOffset+2] = 1;
							}
						}
						srcOffset += 3;
					}
				}
				Marshal.Copy(surfData, 0, bmpData.Scan0, surfSize);
				bmp.UnlockBits(bmpData);
			}

			void loadFromLfdFont(LfdReader.Font lfdFont, bool shadow)
			{
				int defWidth = Math.Max(1, lfdFont.Height - 2);
				int defHeight = lfdFont.Height;

				if (shadow)
				{
					defWidth++;
					defHeight++;
				}

				generateInvalidGlyph(defWidth, defHeight);

				if (shadow) convertToShadow(_glyphError.Image);

				int maxWidth = 0;
				for (int i = 0; i < 256; i++)
				{
					char c = (char)i;

					BriefingGlyph glyph = new BriefingGlyph
					{
						Image = _glyphError.Image,
						Bounds = _glyphError.Bounds,
						Width = defWidth
					};

					if (i >= lfdFont.StartingChar && i < lfdFont.StartingChar + lfdFont.NumberOfGlyphs)
					{
						int index = i - lfdFont.StartingChar;
						var lfdGlyph = lfdFont.Glyphs[index];

						int width = lfdGlyph.Width;
						int height = lfdGlyph.Height;
						maxWidth = Math.Max(width, maxWidth);

						Rectangle copyRect = new Rectangle(0, 0, lfdGlyph.Width, lfdGlyph.Height);

						if (shadow)
						{
							width++;
							height++;
						}

						// Copies the image and converts to the format we need.
						Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
						using (Graphics g = Graphics.FromImage(bmp)) g.DrawImage(lfdGlyph, copyRect, copyRect, GraphicsUnit.Pixel);

						if (shadow) convertToShadow(bmp);

						bmp.MakeTransparent(Color.Black);
						glyph.Image = bmp;
						glyph.Bounds = new Rectangle(0, 0, lfdGlyph.Width, lfdGlyph.Height);
						glyph.Width = lfdGlyph.Width;
					}
					_characterSet.Add(c, glyph);
				}

				HorizontalSpacing = 1;
				Height = lfdFont.Height;
				MaxHeight = lfdFont.Height;
				MaxWidth = maxWidth;
			}

			public void LoadFromAbpFile(string filename, AssetSourceType sourceType)
			{
				if (Loaded) return;

				try
				{
					using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
					{
						using (BinaryReader br = new BinaryReader(fs))
						{
							LoadFromAbpStream(fs, br);
							SourceFile = filename;
							SourceType = sourceType;
							Loaded = true;
							br.Dispose();
						}
					}
				}
				catch { Loaded = false; }
			}

			public void GenerateDefaultFont(float size, string familyName, FontStyle style, bool shadow)
			{
				_characterSet.Clear();
				System.Drawing.Font font = new System.Drawing.Font(familyName, size, style);

				// Temporary canvas to render on.
				Bitmap render = new Bitmap(font.Height * 4, font.Height);
				Rectangle rect = new Rectangle(0, 0, render.Width, render.Height);
				int maxHeight = 0;
				int maxWidth = 0;

				using (Graphics g = Graphics.FromImage(render))
				{
					for (int i = 0; i < 256; i++)
					{
						// Render each character from the font into its own bitmap.
						g.Clear(Color.Black);
						char c = (char)i;
						string s = c.ToString();
						var sz = g.MeasureString(s, font);
						g.DrawString(s, font, Brushes.White, 0, 0);

						var bmpData = render.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
						int surfSize = Math.Abs(bmpData.Stride) * bmpData.Height;
						byte[] surfData = new byte[surfSize];
						Marshal.Copy(bmpData.Scan0, surfData, 0, surfSize);

						int x1 = -1;
						int x2 = -1;
						int y1 = 0;
						int y2 = (int)sz.Height;

						for (int y = 0; y < bmpData.Height; y++)
						{
							int scanline = y * Math.Abs(bmpData.Stride);
							for (int x = 0; x < bmpData.Width; x++)
							{
								if (surfData[scanline + (x * 3)] != 0)
								{
									if (x < x1 || x1 == -1) x1 = x;
									if (x > x2) x2 = x;
								}
							}
						}

						// If it was empty, these values won't be assigned.
						if (x1 == -1)
						{
							x1 = 0;
							x2 = (int)Math.Ceiling(sz.Width);
							if (x2 > render.Width - 1) x2 = render.Width - 1; // Tab characters will be ultra wide
							y1 = (int)Math.Floor(sz.Height * 0.4f);
							y2 = (int)Math.Ceiling(sz.Height * 0.6f);
						}

						render.UnlockBits(bmpData);

						// Transfer the rendered glyph to a new character bitmap which contains only the size it needs.
						int width = (x2 - x1) + 1;
						int height = (y2 - y1) + 1;
						int pad = (shadow ? 1 : 0);

						Bitmap cbmp = new Bitmap(width + pad, height + pad, PixelFormat.Format24bppRgb);
						using (Graphics g2 = Graphics.FromImage(cbmp)) g2.DrawImage(render, 0, 0, new Rectangle(x1, y1, width, height), GraphicsUnit.Pixel);

						if (shadow) convertToShadow(cbmp);

						cbmp.MakeTransparent(Color.Black);

						// Default space width is a bit wide, makes sentences look strange.
						if (c == ' ') width--;

						// Tabs are extra wide, so exclude it from width.
						if (i != '\t') maxWidth = Math.Max(width, maxWidth);
						maxHeight = Math.Max(height, maxHeight);

						BriefingGlyph glyph = new BriefingGlyph
						{
							Image = cbmp,
							Bounds = new Rectangle(0, 0, width, height),
							Width = width
						};
						_characterSet.Add(c, glyph);
					}
				}

				HorizontalSpacing = 1;
				MaxWidth = maxWidth;
				MaxHeight = maxHeight;
				Height = GetChar(' ').Bounds.Height;

				font.Dispose();
				render.Dispose();
				SourceType = AssetSourceType.None;
				Loaded = true;
			}

			void LoadFromAbpStream(FileStream fs, BinaryReader br)
			{
				const int BaseDataOffset = 0x60B;
				int[] charDataOffset = new int[256];
				byte[] charSizeY;
				byte[] charSizeX;

				br.ReadInt32();   // Length, total size of RLE block.  Will match the filesize minus HeaderSize.

				for (int i = 0; i < 256; i++) charDataOffset[i] = br.ReadInt32();

				// Height is first.
				charSizeY = br.ReadBytes(256);
				charSizeX = br.ReadBytes(256);
				Height = br.ReadInt32();
				br.ReadByte(); // In-game loaded flag, not needed here.
				HorizontalSpacing = br.ReadByte();

				int maxHeight = 0;
				int maxWidth = 0;

				for (int i = 0; i < 256; i++)
				{
					fs.Position = BaseDataOffset + charDataOffset[i];

					int charWidth = charSizeX[i];
					int charHeight = charSizeY[i];
					if (charWidth == 0) continue;

					maxWidth = Math.Max(charWidth, maxWidth);
					maxHeight = Math.Max(charHeight, maxHeight);

					Bitmap bmp = new Bitmap(charWidth, charHeight, PixelFormat.Format24bppRgb);
					Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
					var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

					int stride = Math.Abs(bmpData.Stride);
					int surfSize = stride * bmpData.Height;
					byte[] surfData = new byte[surfSize];

					int offset;
					int x;
					for (int y = 0; y < charHeight; y++)
					{
						// Each scanline is encoded separately, and has an Int32 length field indicating the number of RLE bytes.
						fs.Position += 4;
						x = 0;
						offset = (stride * y) + (x * 3);
						while (true)
						{
							byte opcode = br.ReadByte();
							if (opcode == 0x80) break; // End marker

							if ((opcode & 0x80) == 0)
							{
								if ((opcode & 0x40) != 0)
								{
									// Skip over position by amount in opcode.
									x += (opcode & 0x3F);
									offset += (opcode & 0x3F) * 3;
								}
								else
								{
									// Sequence of one color, by amount in the opcode, of color in the next byte.
									// XvT always has a color value of 255.  In game it's RGB(248,252,248)
									// XWA values range from 0 to 31.  The intensity values in game range from about 40 to 248.
									// The converted values here are close enough.  We won't be able to match the real palette anyway.
									// This is also true for the color sequence in the next section.
									byte b = br.ReadByte();
									if (b <= 31) b = (byte)(248 - ((31 - b) * 6));
									else b = 248;

									for (int j = 0; j < opcode; j++)
									{
										surfData[offset + 0] = b;
										surfData[offset + 1] = b;
										surfData[offset + 2] = b;
										x++;
										offset += 3;
									}
								}
							}
							else
							{
								// High bit is set, but not end marker.  A sequence of color values.
								int count = (opcode & 0x7F);
								if (count != 0)
								{
									for (int j = 0; j < count; j++)
									{
										byte b = br.ReadByte();
										if (b <= 31) b = (byte)(248 - ((31 - b) * 6));
										else b = 248;

										surfData[offset + 0] = b;
										surfData[offset + 1] = b;
										surfData[offset + 2] = b;
										offset += 3;
									}
								}
							}
						}
					}

					Marshal.Copy(surfData, 0, bmpData.Scan0, surfSize);
					bmp.UnlockBits(bmpData);
					bmp.MakeTransparent(Color.Black);

					BriefingGlyph glyph = new BriefingGlyph
					{
						Width = charSizeX[i],
						Image = bmp,
						Bounds = rect
					};
					_characterSet.Add((char)i, glyph);
				}
				MaxWidth = maxWidth;
				MaxHeight = maxHeight;
			}
		}

		/// <summary>A general purpose container that stores information for all kinds of visible objects on the map that were activated by events.</summary>
		class MapElement
		{
			public bool Enabled = false;
			public int ElapsedTime = 0;  // How much time has passed since activation, for animation purposes.
			public int DataIndex = 0;    // Will be a FG tag index, text tag index, or XWA icon index, depending on which collection this element belongs to.
			public int EventIndex = -1;  // Index of the briefing event that activated this element or shadow icon.  If a shadow icon and this is -1, it means it's a preview icon.
			public int EventUid = 0;     // UID of the event that activated this element.
			public int X = 0;
			public int Y = 0;
			public int Color = 0;        // Index into the color array to use.  Used for text tags and XWA icon IFF colors.

			// All of these are XWA only, additional parameters that track icon events.
			public int LastMoveEventIndex = -1;     // Last known move event
			public int LastMoveEventUid = 0;        // Last known move event UID
			public int LastRotateEventIndex = -1;   // Last known rotate event
			public int IconCraftType = 0;
			public int IconRotation = 0;
			public int IconTime = 0;                // For shadow icons, the time index of this icon.
			public int IconRegionPage = 0;
			public int IconRegionIndex = 0;

			// Information that's only used to render and cache text tags.
			public Bitmap TextTagBitmapCache = null;
			public string TextTagStringCache = "";
			public bool TextTagRenderFinished = false;
			public Point TextTagEndPos = new Point();

			public Rectangle InteractRect;
			public int UndoUID = -1;

			public MapElement() { }
			// NOTE: If a copy constructor is needed, copy everything InteractRect and UndoUID.

			public void ClearTextTagCache(bool dispose)
			{
				TextTagRenderFinished = false;
				TextTagStringCache = "";

				if (dispose && TextTagBitmapCache != null)
				{
					TextTagBitmapCache.Dispose();
					TextTagBitmapCache = null;
				}
			}
		}

		/// <summary>Stores a platform-independent import of a mission briefing.</summary>
		class AbstractBriefing
		{
			public short RunningTime;
			public short[] RawEvents = new short[400];
			public bool[] ViewedByTeam = new bool[10];
			public List<AbstractEvent> Events = new List<AbstractEvent>();

			public string[] Notes = null;
			public string[] Strings = null;
			public string[] Tags = null;

			public AbstractBriefing()
			{
				AllocateStrings(32);

				// Enable first team by default to assist when storing import data.
				ViewedByTeam[0] = true;
			}

			public AbstractBriefing(AbstractBriefing other)
			{
				AllocateStrings(other.Strings.Length);
				CopyFrom(other);
			}

			public void Erase()
			{
				RunningTime = 0;
				Array.Clear(RawEvents, 0, RawEvents.Length);
				Array.Clear(ViewedByTeam, 0, ViewedByTeam.Length);
				Events.Clear();
				EraseStrings();
			}

			public void EraseStrings()
			{
				for (int i = 0; i < Notes.Length; i++) Notes[i] = "";
				for (int i = 0; i < Strings.Length; i++) Strings[i] = "";
				for (int i = 0; i < Tags.Length; i++) Tags[i] = "";
			}

			public void CopyFrom(AbstractBriefing other)
			{
				RunningTime = other.RunningTime;
				Array.Copy(other.RawEvents, RawEvents, RawEvents.Length);
				Array.Copy(other.ViewedByTeam, ViewedByTeam, ViewedByTeam.Length);

				Events.Clear();
				for (int i = 0; i < other.Events.Count; i++) Events.Add(new AbstractEvent(other.Events[i]));

				for (int i = 0; i < Notes.Length; i++) Notes[i] = other.Notes[i];
				for (int i = 0; i < Strings.Length; i++) Strings[i] = other.Strings[i];
				for (int i = 0; i < Tags.Length; i++) Tags[i] = other.Tags[i];
			}

			public void AllocateStrings(int count)
			{
				Strings = new string[count];
				Tags = new string[count];
				Notes = new string[count];
				EraseStrings();
			}
		}
		#endregion Supporting types
	}

	#region Persistent configuration
	/// <summary>Stores all customizable configuration parameters used by the briefing editor.</summary>
	/// <remarks>Each platform's params are separate, except XvT and BoP which share the same object.</remarks>
	public class BriefingConfigsCollection
	{
		/*
		 * int START_TAG
		 * int Size: total byte length, including TAGs
		 * short Version
		 * BriefingConfig[4] { XW, TIE, XvT/BoP, XWA }
		 * int END_TAG
		 */
		/// <summary>Version identifying the binary format.  If the version changes, the values will revert to default.</summary>
		private const short ExpectedVersion = 1;

		private const int START_TAG = 0x53465242;  // "BRFS"
		private const int END_TAG = 0x45465242;    // "BRFE"

		/// <summary>Singleton containing the array of configurations for each platform.</summary>
		private static BriefingConfig[] _configs = null;

		/// <summary>Gets a single config for a specific platform.</summary>
		static public BriefingConfig GetConfig(Settings.Platform platform)
		{
			BriefingConfig[] collection = getCollection();

			switch (platform)
			{
				case Settings.Platform.XWING: return collection[0];
				case Settings.Platform.TIE: return collection[1];
				case Settings.Platform.XvT: case Settings.Platform.BoP: return collection[2];
				case Settings.Platform.XWA: return collection[3];
			}
			return new BriefingConfig();
		}

		static public void Save(FileStream fs, BinaryWriter bw)
		{
			try
			{
				long startPos = fs.Position;
				int size = 0;

				bw.Write(START_TAG);
				bw.Write(size);         // Placeholder until we return

				BriefingConfig[] collection = getCollection();
				bw.Write(ExpectedVersion);
				for (int i = 0; i < collection.Length; i++) collection[i].Save(bw);

				bw.Write(END_TAG);
				
				// Rewind, write size, then return to end position.
				size = (int)(fs.Position - startPos);
				fs.Position = startPos + 4;
				bw.Write(size);
				fs.Position = startPos + size;
			}
			catch { }
		}

		static public void Load(FileStream fs, BinaryReader br)
		{
			long startPos = fs.Position;
			int totalSize = 0;  // Total size of the entire config collection, including segment tags.
			bool failed = false;
			try
			{
				BriefingConfig[] collection = getCollection();

				// Check start of block
				if (br.ReadInt32() != START_TAG) throw new FileFormatException("expected start tag");

				totalSize = br.ReadInt32();

				// All briefing data
				short version = br.ReadInt16();
				if (version <= 0 || version > ExpectedVersion) throw new FileFormatException("invalid version");

				for (int i = 0; i < collection.Length; i++) collection[i].Load(version, br);

				// Check end of block
				if (br.ReadInt32() != END_TAG) throw new FileFormatException("expected end tag");
				if (fs.Position != startPos + totalSize) throw new FileFormatException("section size mismatch");
			}
			catch
			{
				failed = true;

				// Restore everything just to be safe.
				foreach (var conf in getCollection()) conf.SetDefaults();
			}

			// If failed and nothing was loaded, remains on the start position.
			// If succeeded or failed with load, exits where the segment ends.
			if (failed) fs.Position = startPos + totalSize;
		}

		/// <summary>Gets the singleton containing the array of configurations.</summary>
		static BriefingConfig[] getCollection()
		{
			if (_configs == null)
			{
				_configs = new BriefingConfig[4];
				for (int i = 0; i < 4; i++) _configs[i] = new BriefingConfig();
			}
			return _configs;
		}

		/// <summary>Stores the customizable configuration parameters used by a single platform.</summary>
		public class BriefingConfig
		{
			// Version 1
			public bool DynamicScaleEnabled;
			public float FixedMapScale;
			public byte InterpolationMode;
			public bool AutoNearest;
			public byte TimelineFontSize;
			public byte TimelineMinimumRows;
			public bool AutoExtendDuration;
			public byte AutoExtendAmount;
			public short Width;
			public short Height;
			public bool AnimateMoveZoom;
			public bool AnimateTags;
			public bool LoopPlayback;
			public bool TeamPlayerLabels;
			public bool TimelinePlayback;
			public bool TransitionEffects;
			public bool AudioPlayback;
			public bool IconShadows;
			public bool HighDef;
			public bool FreeLookInfo;
			public byte MoveIncrement;
			public bool MoveSnapGrid;
			public bool MoveSnapMouse;

			public byte TimeShiftIncrement;
			public bool AutoPageBreak;
			public bool AutoTitle;
			public bool AutoShipInfoEnd;
			public float ShipInfoTime;
			public bool AutoShipInfoJump;
			public bool IconAdvanceTime;

			public bool EventListDifference;
			public bool EventListTickTime;
			public byte EventShift;
			public bool EventListShiftAll;
			public bool EventListShiftJump;

			public bool ExportEverything;
			public bool ExportStrings;
			public bool ExportEmptyString;
			public bool ExportEvents;
			public bool ExportFlightgroup;
			public bool ExportXwing;

			byte[] _reserved = new byte[8];
			// End version 1

			public BriefingConfig() => SetDefaults();

			public void SetDefaults()
			{
				DynamicScaleEnabled = true;
				FixedMapScale = 1.0f;
				InterpolationMode = (byte)System.Drawing.Drawing2D.InterpolationMode.Bicubic;
				AutoNearest = true;
				TimelineFontSize = 8;
				TimelineMinimumRows = 4;
				AutoExtendDuration = true;
				AutoExtendAmount = 6;
				Width = -1;
				Height = -1;
				AnimateMoveZoom = true;
				AnimateTags = true;
				LoopPlayback = true;
				TeamPlayerLabels = true;
				TimelinePlayback = true;
				TransitionEffects = true;
				AudioPlayback = true;
				IconShadows = true;
				HighDef = true;
				FreeLookInfo = true;
				MoveIncrement = 0;
				MoveSnapGrid = false;
				MoveSnapMouse = false;

				TimeShiftIncrement = 0;
				AutoPageBreak = true;
				AutoTitle = true;
				AutoShipInfoEnd = true;
				ShipInfoTime = 4.0f;
				AutoShipInfoJump = true;
				IconAdvanceTime = true;

				EventListDifference = false;
				EventListTickTime = false;
				EventShift = 0;
				EventListShiftAll = true;
				EventListShiftJump = true;

				ExportEverything = true;
				ExportStrings = true;
				ExportEmptyString = false;
				ExportEvents = true;
				ExportFlightgroup = true;
				ExportXwing = true;

				Array.Clear(_reserved, 0, _reserved.Length);
			}

			public void Save(BinaryWriter bw)
			{
				bw.Write(DynamicScaleEnabled);
				bw.Write(FixedMapScale);
				bw.Write(InterpolationMode);
				bw.Write(AutoNearest);
				bw.Write(TimelineFontSize);
				bw.Write(TimelineMinimumRows);
				bw.Write(AutoExtendDuration);
				bw.Write(AutoExtendAmount);
				bw.Write(Width);
				bw.Write(Height);
				bw.Write(AnimateMoveZoom);
				bw.Write(AnimateTags);
				bw.Write(LoopPlayback);
				bw.Write(TeamPlayerLabels);
				bw.Write(TimelinePlayback);
				bw.Write(TransitionEffects);
				bw.Write(AudioPlayback);
				bw.Write(IconShadows);
				bw.Write(HighDef);
				bw.Write(FreeLookInfo);
				bw.Write(MoveIncrement);
				bw.Write(MoveSnapGrid);
				bw.Write(MoveSnapMouse);

				bw.Write(TimeShiftIncrement);
				bw.Write(AutoPageBreak);
				bw.Write(AutoTitle);
				bw.Write(AutoShipInfoEnd);
				bw.Write(ShipInfoTime);
				bw.Write(AutoShipInfoJump);
				bw.Write(IconAdvanceTime);

				bw.Write(EventListDifference);
				bw.Write(EventListTickTime);
				bw.Write(EventShift);
				bw.Write(EventListShiftAll);
				bw.Write(EventListShiftJump);

				bw.Write(ExportEverything);
				bw.Write(ExportStrings);
				bw.Write(ExportEmptyString);
				bw.Write(ExportEvents);
				bw.Write(ExportFlightgroup);
				bw.Write(ExportXwing);
				bw.Write(_reserved);
				bw.Write((byte)0xFF);  // Platform chunk end marker
			}
			public void Load(short version, BinaryReader br)
			{
				if (version >= 1)
				{
					DynamicScaleEnabled = br.ReadBoolean();
					FixedMapScale = br.ReadSingle();
					InterpolationMode = br.ReadByte();
					AutoNearest = br.ReadBoolean();
					TimelineFontSize = br.ReadByte();
					TimelineMinimumRows = br.ReadByte();
					AutoExtendDuration = br.ReadBoolean();
					AutoExtendAmount = br.ReadByte();
					Width = br.ReadInt16();
					Height = br.ReadInt16();
					AnimateMoveZoom = br.ReadBoolean();
					AnimateTags = br.ReadBoolean();
					LoopPlayback = br.ReadBoolean();
					TeamPlayerLabels = br.ReadBoolean();
					TimelinePlayback = br.ReadBoolean();
					TransitionEffects = br.ReadBoolean();
					AudioPlayback = br.ReadBoolean();
					IconShadows = br.ReadBoolean();
					HighDef = br.ReadBoolean();
					FreeLookInfo = br.ReadBoolean();
					MoveIncrement = br.ReadByte();
					MoveSnapGrid = br.ReadBoolean();
					MoveSnapMouse = br.ReadBoolean();

					TimeShiftIncrement = br.ReadByte();
					AutoPageBreak = br.ReadBoolean();
					AutoTitle = br.ReadBoolean();
					AutoShipInfoEnd = br.ReadBoolean();
					ShipInfoTime = br.ReadSingle();
					AutoShipInfoJump = br.ReadBoolean();
					IconAdvanceTime = br.ReadBoolean();

					EventListDifference = br.ReadBoolean();
					EventListTickTime = br.ReadBoolean();
					EventShift = br.ReadByte();
					EventListShiftAll = br.ReadBoolean();
					EventListShiftJump = br.ReadBoolean();

					ExportEverything = br.ReadBoolean();
					ExportStrings = br.ReadBoolean();
					ExportEmptyString = br.ReadBoolean();
					ExportEvents = br.ReadBoolean();
					ExportFlightgroup = br.ReadBoolean();
					ExportXwing = br.ReadBoolean();

					_reserved = br.ReadBytes(_reserved.Length);
				}

				// Before adding any future versions, consider using some of the reserved space in V1.

				if (br.ReadByte() != 0xFF)
				{
					SetDefaults();
					throw new FileFormatException("platform chunk end expected 0xFF");
				}
			}
		}
	}
	#endregion Persistent configuration
}
