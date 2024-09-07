# [Ye Olde Galactic Empire Mission Editor](https://github.com/MikeG621/YOGEME)

Author: [Michael Gaisser](mailto:mjgaisser@gmail.com)  
![GitHub Release](https://img.shields.io/github/v/release/MikeG621/YOGEME)
![GitHub Release Date](https://img.shields.io/github/release-date/MikeG621/YOGEME)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/MikeG621/YOGEME/total)
![GitHub License](https://img.shields.io/github/license/MikeG621/YOGEME)  
![GitHub Issues or Pull Requests by label](https://img.shields.io/github/issues-raw/MikeG621/YOGEME/bug)
![GitHub Issues or Pull Requests by label](https://img.shields.io/github/issues-raw/MikeG621/YOGEME/new%20stuff)

Contributors:
- [Random Starfighter (JB)](https://github.com/RandomStarfighter)
- [Jérémy Ansel](https://github.com/JeremyAnsel)

Thank you for downloading YOGEME, the all-in-one mission editor for the X-Wing
series, Xwing95 through X-wing Alliance.  This program allows easy mission
creation for all of your favorite Star Wars flight sims in a single platform,
and has abilities and mission parameters that have been discovered since the
original mainstream editors were created.

For program usage and help (with images), see the [help file](yogeme.chm).

## Install
YOGEME does not use an installer, simply download the latest ZIP file from the
[Releases](https://github.com/MikeG621/YOGEME/releases) page on Github and
extract to your location of choice.

YOGEME will auto-detect your installed platforms and enable those editing
capabilities (you shouldn't be making missions for a platform you can't play,
they tend to be bug-ridden), however if you absolutely need a disabled
platform, it can be activated in the Options dialog. Note however that certain
functions may not work properly for an uninstalled platform.

## Uninstall
To remove YOGEME, simply delete the directory where you placed it.
This will leave behind a single config file in AppData with your user settings.

## Latest Release
#### WIP
- (About) Minor updates to links and layout.
- (Briefing) X-wing page events now edited directly instead of copying back and forth to a separate array first.
- (All) SaveAs behavior now consistent between platforms; won't abort if mission is unmodified.
- (TIE) IFF 5 (Red) permanently checked to match game behavior.
- (TIE) Fixed a bug where the Questions wouldn't clear properly when loading a second mission.
- (TIE-XWA) Test respects the "DetectMission" setting.
- (TIE-XWA) Fixed Special Cargo control visilibity when not on first FG tab.
- (XWA) Radio and Formation drop-downs swapped positions for consistency between platforms.
- (XWA) Hooks: Added new options to WeaponRates, added WarheadTypeCount.
- (Xwing) Order Descriptions now shown.
- Settings class now a singleton and therefor shared between instances.
- Various cleanups throughout.

Extensive updates due to changes in Platform.dll, using the full file format spec.
- (Briefing) Unknown 3 now labeled "Tile", still unknown effect. Confirmed no effect in TIE.
  - Good chance this will be removed in the future.
- (Briefing) Labels for Unk1/2 StartLength/EventLength and the Unknowns group deleted.
- (Briefing) Text tag, FG tag, and masking colors now defined at launch instead of during every paint.
- (Briefing) Event handling redone.
- (Hyper dialog) Update per spec, enum use to remove magic numbers.
- (TIE) Added TimeLimit, RndSeed, Global Goal editor note.
- (TIE) Arr/Dep renames.
- (XvT) Added Flightgroup Handicap, Global Goal delay, EoM delay.
- (XWA) Mission tab: Added GoalsUnimportant, WinOfficer, FailOfficer.
- (XWA) Mission2 tab: Added the rest of the properties for GlobalCargo and GlobalGroups (which also expanded to 32 total),
details for GlobalUnits added.
  - Did not expose "ID" property of GlobalCargo or Region, pretty sure that's for internal use.
- (XWA) GlobalUnit names and GlobalCargo will now appear in trigger labels.
- (XWA) Rendzvous WP renamed to "Capture HYP".
- (XWA) Added Flightgroup Handicap, "More Arr/Dep" tab.
- (XWA) Added Messages Type, Speaker Header, Special Meaning. Don't know what they do.
- (XWA) Added Global Goals Trigger Points and Delay.
---
### Additional Information
#### Dependencies
- [Idmr.Common](https://github.com/MikeG621/Common)
- [Idmr.ImageFormat.Act](https://github.com/MikeG621/ImageFormat-Act)
- [Idmr.ImageFormat.Dat](https://github.com/MikeG621/ImageFormat-Dat)
- [Idmr.Platform](https://github.com/MikeG621/Platform)
- [Idmr.LfdReader](https://github.com/MikeG621/LfdReader)

### Version History

#### v1.15.8, 1 Jun 2024
- (XWA) Fixed a Hook dialog crash, something I broke back in v1.15. (Microsoft documentation lied to me...) [Issue [#101](https://github.com/MikeG621/YOGEME/issues/101)]

#### v1.15.7, 19 May 2024
- (XWA) Briefing logo was off by 1, not accounting for "None" properly.

#### v1.15.6, 11 Mar 2024
- Option to toggle SuperBackdrops now always enabled, even if not detected. Still doesn't do anything if not installed.
- (XWA) Fixed a crash if SuperBackdrops is enabled on a new mission and the default craft type is Backdrop [Issue [#100](https://github.com/MikeG621/YOGEME/issues/100)]
- (XWA) WeaponRate hook

#### v1.15.5, 22 Dec 2023
- External Converter utility no longer packaged due to functionality now within Platform.dll
- DSCUP shiplist no longer packaged simply due to age and obsolesence
- (All but Xwing) Global Summary dialog to list out current GlobalGroups and GlobalUnits [Issue [#97](https://github.com/MikeG621/YOGEME/issues/97)]
- (XWA) 32bpp Hook skin opacity support
- (XWA) Sound hook, Interdictor
- (XWA) SAT/1 thru RDV no longer counted in "# Craft at 30 seconds" note [Issue [#96](https://github.com/MikeG621/YOGEME/issues/97)]
- (XWA) WP1 on hyper orders being enabled even if disabled [Issue [#94](https://github.com/MikeG621/YOGEME/issues/94)]
- (XWA) Fixed bad offsets on hyper exit if order WP1 originally disabled

#### v1.15.4, 25 Nov 2023
- (XWA) "HYP from Any Region" exit buoys now processed in the map [Issue [#93](https://github.com/MikeG621/YOGEME/issues/93)]
  ##### 18 Dec 2023 re-release
  - (XWA) via Imageformat.Dat, support for BC7 compressed backdrop [Issue [#98](https://github.com/MikeG621/YOGEME/issues/98)]

#### v1.15.3, 11 Nov 2023
- (XWA) Fixed a Briefing event overflow that could occur when adding Move commands [Issue [#92](https://github.com/MikeG621/YOGEME/issues/92)]
- (XWA) Opening a corrupted briefing should now stop importing wherever it fails, instead of crashing completely
- (XWA) Fixed the Briefing tab pointing to wrong files due to not fixing the regex groups after v1.14's issue 84 fix

#### v1.15.2, 27 Oct 2023
- (XWA) Designation Unknown (0x14) renamed to "HYP from Any Region" via Platform.dll
- (XWA) Arrival method type 2, "Hyper in Region of [mothership]" added [Issue [#91](https://github.com/MikeG621/YOGEME/issues/91)]

#### v1.15.1, 14 Oct 2023
- (XWA) Fixed a crash with trigger text referencing Region 4 [Issue [#87](https://github.com/MikeG621/YOGEME/issues/87)]
- (XWA) Platform.dll updated for issues [#89](https://github.com/MikeG621/YOGEME/issues/89) and [#90](https://github.com/MikeG621/YOGEME/issues/90)

#### v1.15, 23 Sep 2023
- (XWA) Overhaul of Hook dialog, brought up to current
  - Complete GUI rework;
    - drop-down selector instead of tabs
    - no longer prevented from working on a hook that isn't installed
    - Text box to directly modify the INI contents
  - Unsupported/unrecognized sections/values are kept, as are comments
  - During Write, adds short description from the LST if possible
  - Added Concourse and HullIcon support
  - Added MissionObject weapon profiles
  - Added MissionTie craft text and stats profiles
  - Fixed FamHangarCamera to use correct key names
  - Added HangarMap and FamilyHangarMap profiles and inverted floor
  - Removed HangarObjects "FoldOutside"
  - HangarObjects "LoadShuttle" now matches hook's check for "is 1" instead of "not 0"
  - Added HangarObjects OPT replacements to the list
  - The Camera and Family Camera tabs are consolidated
  - Added ability to detect CommandShip, so Hangar files can now check for additional _[OPT] and \_[OPT]\_[IFF#] files to load.
  - Added a lot of keys to HangarObjects: individual Droid settings, object profiles, inverted player, hangar lights, etc.

#### v1.14.2, 01 Sep 2023
- (XW) Error when changing ship type to B-Wing caused a crash with the Map [Issue [#86](https://github.com/MikeG621/YOGEME/issues/86)]

#### v1.14.1, 14 Aug 2023
- (XWA) Backdrop hook now loads the Resdata.txt or INI based on mission location, not just "\Missions\" [Issue [#85](https://github.com/MikeG621/YOGEME/issues/85)]
- (XWA) Hyperspace hook support added
- (XWA) Object hook profile per model added

#### v1.14, 04 Aug 2023
- (XWA) Wav form now supports mission names that start with "8" to support the MP hook [Issue [#84](https://github.com/MikeG621/YOGEME/issues/84)]
- (XWA) Moved around the Trigger Parameter fields to be more consistent between locations and so it's a bit more natural to
  select the correct control when adjusting the Region or Proximity target [Issue [#82](https://github.com/MikeG621/YOGEME/issues/82)]
- (Backdrop) The "Color:" text now only shows for XWA.
- (Backdrop) Added a label pointing out that the selected color is on the clipboard, as it's not applied automatically [Issue [#80](https://github.com/MikeG621/YOGEME/issues/80)]
- (Briefing) Fixed a crash that would occur if the Caption/Title event was created without a string being selected.
- (Briefing) Page number correctly increments on CaptionText instead of PageBreak
- (Briefing) **NEW COMMAND**: For TIE and XvT, there is now a "Marker" button which adds a breakpoint for the Skip button in-game, without clearing the Text and Caption text.
- (Map) Moving the mouse off of the map will now clear the Shift state so it doesn't get stuck.
- (Map, XWA) Virtual hyper waypoints will check secondary Designation for buoys, and multiple related selection issues were fixed.
  Likely still needs some more work. [Issue [#83](https://github.com/MikeG621/YOGEME/issues/83)]

#### v1.13.12, 16 Jan 2023
- Various simple cleanups
- (Map) Legacy wireframes now use LfdReader instead of duplicating functionality
- (All) Added "Keep Selected Order" to the main options page so it doesn't reset to Order 1 when switching FG
- (XWA) Removed a leftover debug bypass from the 1.13.10 Map testing
- (XWA) Enabled Order Waypoints are no longer displayed if the craft never visits that Region
- (XWA) Order Region selection resets during Open/New mission
- (XWA) Disabled SP1 WPs can be selected since they're always shown

#### v1.13.11, 30 Oct 2022
- (Map) Implemented usage of LfdReader for "classic" wireframe models
- (Map) XWA, SP1 always treated as Enabled for icon display and hyperspace processing
- (All) Open dialog now follows current mission properly after switching platforms via "Open Recent"

#### v1.13.10, 18 Oct 2022
- (XWA) Added a toggle ability to shade the FG list by region
- (XWA) Fixed Wireframe Roll in the map
- (XWA) Craft now display the hyper exit points instead of SP1 in other regions
- Updates for TIE, all involving Platform.dll to some degree. Deleted items are those confirmed to have zero effect in the executable.
  - "Captured on Ejection" and "Secret Goals" removed (involves GUI update)
    [Issues [#73](https://github.com/MikeG621/YOGEME/issues/73) and [#74](https://github.com/MikeG621/YOGEME/issues/74)]
  - Trigger "Unknown (arrive?)" now "cannon subsystem disabled" [Issue [#75](https://github.com/MikeG621/YOGEME/issues/75)]
  - Trigger Type "Craft When" fixed [Issue [#76](https://github.com/MikeG621/YOGEME/issues/76)]
  - Trigger Type "Misc" now "Adjusted AI Skill", added "Status" and "All Craft" types to match XvT [Issue [#77](https://github.com/MikeG621/YOGEME/issues/77)]
  - Status "No Lasers" now "No Turrets", everything past "Hyperdrive Added" deleted [Issue [#78](https://github.com/MikeG621/YOGEME/issues/78)]
  - Orders after "Board to Repair" deleted [Issue [#79](https://github.com/MikeG621/YOGEME/issues/79)]

#### v1.13.9, 07 Sep 2022
- New error dialog with the ability to skip future errors. Currently only for Backdrop DAT loading [Issue [#71](https://github.com/MikeG621/YOGEME/issues/71)]
- (XWA) Lines in RESDATA.txt will be skipped when loading backdrops if commented out by ; or // [Issue [#72](https://github.com/MikeG621/YOGEME/issues/72)]

#### v1.13.8, 28 Aug 2022
- (XWA) Added ability to edit COMBAT.LST if Justagai's MP Hook is installed

#### v1.13.7, 30 Jul 2022
- (All) Fixed a crash if copying a single Waypoint value, but no text is selected
- (XWA) Fixed the Order WPs being linked together after a multi-select Paste

#### v1.13.6, 19 Jun 2022
- (Start) Added option to load last mission from the initial launcher.
- (All) Exit confirmation when enabled now only shows if mission isn't saved. Prevents the double confirmation if default options are checked.
- (Briefing) "Shift All" checkbox on the Events tab so all events after the selected move together.

#### v1.13.5, 08 Jun 2022
- (Map) Fixed a crash in XWA if SP1 wasn't checked when changing the Region selection

#### v1.13.4, 06 Jun 2022
- (All) Option to display FlightGroup "#X of X" as 1-indexed or 0-indexed
- (XWA) FG Goal auto-toggles when setting the Trigger
- (Map) Couple back-end things, no functional changes

#### v1.13.3, 02 Apr 2022
- (All) Fixed ComboBox stutter for sometimes-colorized cbo's
- (XWA) Tooltip for Goal Active Sequence added
- (Map) Fixed the window stealing focus when you mouse over the map if the window wasn't selected

#### v1.13.2, 19 Mar 2022
- Converter utility updated to v1.7.2 to fix XvT2XWA order problems
- (All) Save will globally trigger Leave events so Ctrl+S doesn't lose last edit
- (XWA) Added Format 25C backdrop support via v2.4 of ImageFormat.Dat.dll
- (XWA) Added ability to edit Hint/Failed briefing audio, which plays immediately after leaving the mission
- (XWA) Fixed the WAV LST staying open and preventing save.
- (Map) Checkboxes to toggle wireframes and use of icon size limit added
- (Map) Minimum width increased, selection buttons fixed to left side

#### v1.13.1, 08 Feb 2022
- (XWA) SP3 waypoint renamed to RDV
- (XWA) Order Waypoint drop-down replaced with a pair of numerics
- Various fixes from RandomStarfighter
  - (All) Fixed multi-select refresh issues
  - (All) Added Cut (Ctrl+X) menu
  - (All) Fixed control and craftStart refreshes during paste and other operations
  - (All) Fixed handling of Enter key in multi-line text boxes
  - (XvT) Added buttons for Skip trigger copy/paste
  - (XWA) Reworked some more drop-down loading to prevent duplicates

#### v1.13, 30 Jan 2022
- Formation dialog redesigned (JB)
- (All) Multi-select added to all platforms [Issue [#23](https://github.com/MikeG621/YOGEME/issues/23)] (JB)
- (Xwing) Formation list fixed (via Platform.dll) [Issue [#63](https://github.com/MikeG621/YOGEME/issues/63)] (JB)
- (XvT) Order Designation is now an editable drop-down (JB)
- (XWA) Copy/paste buttons added for Order Waypoints

#### v1.12, 03 Jan 2022
- (All) Fixed the "looping" effect when scrolling through colored list boxes (Thanks to RandomStarfighter)
- (XW) Created the Tour Editor, similar to TIE's Battle Editor
- (XvT) Multi-select in FlightGroups and Messages [Issue [#23](https://github.com/MikeG621/YOGEME/issues/23)] (JB)
     This was pretty extensive, several files were touched to get this going, including updates to Platform.dll.
     Allows for changing properties for multiple FGs/Messages, multi-delete, shifting up/down in the list.
     Properties shown in the tab will be highest selected item.
     Rest of the platforms will be coming.
- (XWA) Removed "Not Identified" from the Status list (via Platform.dll)
- (XWA) Fixed duplication issues in drop-downs (JB)
- (TIE-XWA) Unused Messages are now listed in Gray
- (Map) Fixed an issue with multi-delete (JB)

#### v1.11.2, 05 Oct 2021
- (All) Copy/paste now uses the system clipboard
- (All) Copying also includes a Text equivalant for external pasting
- (All) Can now more easily paste externally copied text
- (All) Copy/paste now works for Waypoints
- (TIE-XWA) Copy/paste for Triggers and Orders are now cross-platform
- (TIE-XWA) Pasting a Message when at capacity now fails correctly
- (XWA) Redid layout of the hook dialog to prevent cutting off the list boxes

#### v1.11.1, 14 Aug 2021
- Set InvariantCulture to prevent text read errors (JB)

#### v1.11, 01 Aug 2021
- (XWA) Color picker for XWA backdrops.
  Copied to YOGEME's clipboard to be used for the backdrop name (via Ctrl+V) [Issue [#46](https://github.com/MikeG621/YOGEME/issues/76)]
- Multiple fixes from Random Starfighter (JB)
  - (All) SS Patrol and SS Await Return order strings now show target info (via Platform.dll)
  - (XWA) Hyper to Region order text updated with token so it'll show number and name if defined (via Platform.dll)
  - (XWA) Fixed some CraftType errors in Order and Trigger strings (via Platform.dll)
  - (XWA) Also fixed some ComboBoxes when using CraftType
  - (XWA) Briefing icons attempt to load from the platform directory to account for mods
  - (XWA) FGs in map now also take into account Hyper orders for region visiblity
  - (XWA) Map wireframes load default profile to account for additional hook meshes
  - (XWA) Fixed an exception on GlobalCargo during save

#### v1.10.2, 06 Jun 2021
- (XWA) SuperBackdrop region detection.
- (XWA) Backdrop error messages with XWAUP v3 fixed [Issue [#44](https://github.com/MikeG621/YOGEME/issues/44)] (via ImageFormat.Dat.dll)

#### v1.10.1, 06 Jun 2021
- (XWA) Backdrop shadow fix

#### v1.10, 20 May 2021
- (All) Redid handling bad waypoint values [Issue [#56](https://github.com/MikeG621/YOGEME/issues/56)] (JB)
- (XW) Converting FG Goals from XW corrected [Issue [#55](https://github.com/MikeG621/YOGEME/issues/55)] (JB, via Platform.dll)
- (XWA) Hook Dialog redesigned
  - Mission : Wingman markings, HangarObjects : Droid1/2Update added
  - Fixed missing Droid1/2PositionZ read
  - S-Foils hook support added
  - Skins (32bpp) hook support added
  - Shield hook support added
  - Can now handle comments at the end of a line
- (XWA) Map wireframes correctly take into account craft rotations when not using Waypoints.
  Adjusting rotation refreshes map. [Issue [#58](https://github.com/MikeG621/YOGEME/issues/58), [#59](https://github.com/MikeG621/YOGEME/issues/59)] (JB))
- (XWA) "Tools - Mission Craft List" menu item to display the pre-briefing craft list
- (XWA) Added explicit PlayerNumber check if Verify isn't run since XWA will crash at the Briefing if missing

#### v1.9.2, 28 Mar 2021
- MissionVerify updated with "OR true" and "AND false" trigger detection for 1AO2 and 3AO4s [Issue [#48](https://github.com/MikeG621/YOGEME/issues/48)]
- (All) Craft Type dropdown now lists 20 items instead of the default 8 [Issue [#45](https://github.com/MikeG621/YOGEME/issues/45)]
- (Test) Fixed a load failure when testing a mission that isn't located in a platform directory
- (XW) Converting from XW will sort the Failed message more reliably (via Platform.dll)
- (XW) Briefing conversion accuracy improved in general (via Platform.dll) [Issues [#51](https://github.com/MikeG621/YOGEME/issues/51) and [#53](https://github.com/MikeG621/YOGEME/issues/53)]
- (XWA) And/Or values now match XWA's even/odd behavior when opening (via Platform.dll) [Issue [#48](https://github.com/MikeG621/YOGEME/issues/48)]
- (XWA) Briefing MoveMap command's accuracy has been fixed [Issue [#53](https://github.com/MikeG621/YOGEME/issues/53)]
- Number of things in the XWA Wav dialog:
  - Fixed the wrong EoM button being hidden when WAV file doesn't exist
  - Fixed a crash when using multi-digit battle or mission numbers [Issue [#49](https://github.com/MikeG621/YOGEME/issues/49)]
  - PrePost text fields now clear when changing categories
  - PrePost text fields now provide a comment about random Pre-briefing WAVS when none are defined

#### v1.9.1, 30 Jan 2021
- Couple fixes from Random Starfighter (JB)
  - (Settings) Crash during SuperBackdrops detection if the Mega Patch isn't installed
  - (XWA) Fixed Region colors in the parameter lists
  - (XWA) Fixed Region names not refreshing throughout

#### v1.9, 08 Jan 2021
- (All) Fixed the path to the clipboard BIN file, was wrong in some cases
- (XWA) WAV Manager
- (XWA) Added SuperBackdrop detection for the XWAUP Mega Patch v1.1, as well as general Planet2 detection

#### v1.8.2, 19 Dec 2020
- (XWA) Fixed the default SBD indexes
- (XWA) Default hook ObjectProfile renamed to "default"
- (XWA) Backdrops index-shifted so selection is correct and lines up with AlliED
- (XWA) Bypassed loading Planet2.dat if already loaded, resulting in faster loading
- (XWA) Added backdrops zoom out abililty for oversized XWA images, with labels to show the sizes
- (XWA) Fixed the Add Backdrop/Sound/Object/Hangar buttons that I broke in 1.8.1 by cutting off the first letter...

#### v1.8.1, 13 Dec 2020
- Various code cleanups
- (TIE) Fixed a double Verify condition
- (TIE-XWA) Test function will now detect the platform per the mission location and launch there instead of default [Issue #20]
- (All) Test menu item moved under Tools, changed to <u>T</u>est (suggestion per Issue [#20](https://github.com/MikeG621/YOGEME/issues/20))
- (All) The MissionVerify path setting will now revert to default if it's missing
- (XWA) Added [Objects] ObjectProfile_fg_# hook support
- (XWA) Backdrop dialog now saves the images in memory to make opening the dialog repeatedly much faster
- (XWA) Added SuperBackdrop detection for the XWAUP Mega Patch
- (XWA) GlobalCargo dropdown now lists Shadow when Backdrops are selected

#### v1.8, 04 Oct 2020
- (All) Fixed an exception due to incomplete CraftDataManager creation that would occur when platform not detected [Issue [#37](https://github.com/MikeG621/YOGEME/issues/37)]
- (TIE-XWA) Fixed Test function trying to launch if you cancel a new save
- (XWA) Added detection for DTM Super Backdrops v3.1
- (XWA) Fixed Special Cargo text box not showing when switching craft
- (XWA) Added [HangarObjects] HangarRoofCranePositionX, HangarRoofCraneAxis, HangarRoofCraneLowOffset, HangarRoofCraneHighOffset, HangarIff hook support
- (XWA) Added [HangarObjects] ShuttlePositionX/Y/Z, ShuttleOrientation, IsShuttleFloorInverted hook support
- (XWA) Added [HangarObjects] Droids/Droids1/Droids2PositionZ, IsDroidsFloorInverted hook support
- (XWA) Added [HangarObjects] PlayerOffsetX/Y/Z, IsPlayerFloorInverted hook support
- (TIE) Fixed SaveAs XvT and XWA
- (TIE) SaveAs BoP implemented (required Converter update)
- (All) Save no longer rewrites the file if not modified
- (All) Fixed XZ map orientation wireframes
- (All) Fixed Converter.exe call
- (All) Fixed Help file call
- (All) Replaced IDMR link with Github
- Converter.exe updated to v1.6
- More updates and fixes by Random Starfighter (JB)
  - (All) New FlightGroup Library function
  - (All) Focus forcing fixes
  - (Briefing) Fixed a possible crash due to timers
  - Lots of Map stuff
    - Traces can now show ETA, speed, throttle
    - New options to control what's shown for traces
    - Middle-click action can be customized
    - Ability to expand selection to Craft type, IFF, approximate size, and to invert selection
    - Wireframes can be faded or hidden
    - Purple IFF now consistently MediumOrchid
    - Snap ability when moving items
    - Keyboard commands for WP movement, selection
    - (XWA) Ability to change WP's region
    - Ability to show/hide WPs based on Difficulty or IFF
    - Ability to zoom map to fit selected or fit all
    - Map Options save from the Map itself, don't need the Optionals dialog to change the defaults
    - Lots of rewriting and tweaking on the backend and the GUI to get everything working
  - (XvT/XWA) Colorized cbos now work with "Not FG"
  - (XvT) Fixed double listing of IFFs on the Craft tab
  - (XvT) Implementation of uncovered Unknowns; StopArrivingWhen, RndSeed, FG Goal TeamEnabled array, RandomArrDep Min/Sec
  - (XWA) Global Group strings used for GG 0-15
  - (XWA) Tweaked how the colorized cbos draw
  - (XWA) SafeSetCbos added to more places to prevent exceptions
  - (XWA) Skip Indicators update now retains selection
  - (XWA) Added [HangarObjects] HangarRoofCranePositionY, HangarRoofCranePositionZ, PlayerAnimationElevation hook support
  - (XWA) Waypoint refresh maintains selection

#### v1.7, 16 Aug 2020
- (TIE) Fixed an issue where current FG's IFF would reset when updating IFF names
- (TIE) Message colors update immediately
- (XvT) Cleaned up the FG Unknowns tab
- (XWA) IFF subsitutions in triggers and controls
- (XWA Briefing) Fixed wrong icon being selected while setting up MoveIcon
- (XWA Mission2) Fixed a crash when leaving the Global Group text box without a selection
- (XWA) Fixed regions in Parameter drop-downs being drawn as black-on-black
- (All) Mission Map now supports ship wireframe display (JB)
  - Works for all platforms, reads resources from the install directory, mod compatable
  - Touches a lot of things, not going to spell it out here
  - "\*\_shiplist.txt" files for custom craft replaced with "craft_data_*.txt"
- Various fixes and tweaks by Random Starfighter (JB)
  - (Backdrops) Images are now foregraound instead of background
  - (Backdrops) Fixed a possible IndexOutOfRange when clicking thumbnails
  - (Common) SafeString and ParseAfterInt added
  - (Formations) Images are now foreground instead of background
  - (Map) Timer handler disabled when closing
  - (Map) Max zoom and zoom speed adjusted
  - (Map) XvT craft icon fix
  - (All) Cleanup index substitutions
  - (All) Blanks messages now show as "*"
  - (All) Trigger label refresh updates
  - (All) Extra protections to handle custom missions using "bad" Status or Formation values
  - (All) Fixed editor craft number processing that could cause issues
  - (XvT) BoP IFF names implemented (consumes Unknowns 4 and 5)
  - (XvT) More TriggerTypes added
  - (XvT) Unk6 renamed to PreventOutcome
  - (XWA) Added numbers to Messages to make use in Triggers easier
  - (XWA) Craft TeamRoles reduced to 8 from 10
  - (Xwing) Starting ships count now starts at 1
  - (Xwing) Unk1 now Randomizer (unused)
  - (Xwing) Max Craft increased to 255
  - (Xwing) Yaw/Pitch/Roll tweaks, save fixed
  - (Xwing) Briefing Icons now use BMPs instead of the DATs
 
With this release I'm also going to mark [Issue [#14](https://github.com/MikeG621/YOGEME/issues/14)] and [Issue [#12](https://github.com/MikeG621/YOGEME/issues/12)] closed,
which are "Pre-TIE95" and "OPT import". Probably could've closed out 14 a
couple years ago, but I think it's in better shape now. Thanks to Random for
the work on X-wing and the OPT overhaul.

#### v1.6.6, 19 Jul 2020
- (XWA) Fixed a crash when using "Apply DTM SuperBackdrops to new missions" option

#### v1.6.5, 04 Jul 2020
- (Map) If the craft index is too high (mods) use the default 'X' image
- (All) Added more details to custom ship list error message
- (All) Clipboard bin and help path now explicitly use Startup Path to prevent implicit from defaulting to sys32 [Issue [#32](https://github.com/MikeG621/YOGEME/issues/32)]
- (XW) Added custom shiplist capability
- (Briefing) Icons now use BMPs instead of the DATs
- (Briefing) If the craft index is too high (mods) use the default 'X-wing' image
- (XWA) Including mission map icons and shiplist template for DSUCP v2.6
- (XWA) Added missing Unused ship slot at end of the shiplist to account for use in DSUCP
- (XWA) The ShipInfo in the briefings will work for X-wings now
- Various code tweaks/cleaning

Note regarding DSUCP: templates were created before the community events unfolded. While
it may not be available going forward, I'm leaving the templates in for those who still
have installers and wish to use it.

#### v1.6.4, 19 Jan 2020
- (All) Added briefing callbacks so mission doesn't dirty until something is changed [Issue [#30](https://github.com/MikeG621/YOGEME/issues/30)]
- (TIE) Battle form Height increased to account for W10 visual style [Issue [#31](https://github.com/MikeG621/YOGEME/issues/31)]
- (TIE, XvT) Backdrop dialog now dirties mission on OK

#### v1.6.3, 01 Jan 2020
- (All) Fixed a settings write corruption due to partial platform detection [Issue [#29](https://github.com/MikeG621/YOGEME/issues/29)]
- (XWA) Added ShuttleAnimation and ShuttleAnimationStraightLine hooks

#### v1.6.2, 28 Sep 2019
- (All) Fixed a crash that'd occur if Verify was run with a bad path [Issue [#28](https://github.com/MikeG621/YOGEME/issues/28)]
- (XWA) Tweaked the hook INI backup filename during save to prevent possible clashes
- (XWA) Fixed error during hooked backdrop load
- (XWA) Idmr.ImageFormat.Dat.dll updated to handle Format 25 images correctly for backdrops

#### v1.6.1, 16 Sep 2019
- (XWA) Fixed a crash that'd occur if the mission INI file doesn't exist [Issue [#27](https://github.com/MikeG621/YOGEME/issues/27)]

#### v1.6, 15 Sep 2019
- (XWA) Mission Hook editing dialog added for the hooks by Jérémy Ansel (JeremyaFr) [Issue [#26](https://github.com/MikeG621/YOGEME/issues/26)]
- (XWA) SuperBackdrops detection changed to Readme instead of Backup since XWAUCP is different than a solo install
- (XWA) Backdrop "Loading..." button text now always appears, not just when SBD detected 
- (XWA) Backdrop dialog now always resized for XWA to accommodate larger images, not just when SBD detected
- (XWA) Changed some exceptions when trying to launch the Backdrop dialog and the platform isn't detected

#### v1.5.1, 13 May 2019
While I did add the below a couple months ago and just haven't gone through releasing it,
turns out I forgot to finish updating Converter.exe and package it. Oops. [Issue [#25](https://github.com/MikeG621/YOGEME/issues/25)]
- (All) Changing GG or GU value will now prompt to update references throughout if it's the only FG with that designation

#### v1.5, 10 Sep 2018
This most likely isn't a full list of everything that's visible to the user, but you get the idea; there's a lot.
- Lots of fixes and new features by Random Starfighter (JB)
  - X-wing 95 platform support added
  - (All) Tons of back-end stuff I'm not going to spell out here
  - (All) Theme support for shaded labels (orders, triggers, etc)
  - (All) Non-platform-matching filestreams now close properly
  - (All) Colorized drop-downs
  - (All) IFF colors tweaked a little
  - (All) Lots of controls throughout changed to instant-update instead of needing to leave the control
  - (All) Copy/Paste abilities expanded
  - (All) Blank message are now shown
  - (All) General performance improvements
  - (All) 'Enter' key commits control update
  - (All) Move FG/Message Up/Down buttons added
  - (All) Editor-only craft numbering added to FGs with identical names
  - (All) Fixed an error when there's no Messages and a Message trigger becomes selected
  - (Backdrops) Fixed some XWA-specific errors when loading
  - (Briefings) Loading forces a refresh properly
  - (Briefings) Tooltips added
  - (Briefings) Fast Foward now limited to prevent ridiculous speeds
  - (Briefings) Playback speed now shown if not 1x
  - (Briefings) Fixed/tweaked various display issues
  - (Briefings) Clicking on the caption will now advance to the next caption event
  - (Briefings) XWA Tags now use Icon number
  - (Briefings) New event button now adds after current line instead of at the end
  - (Hyperbuoy) Added Beacon orders, fixed Roles, and tweaked AD settings
  - (Map) Show/Hide functionality added
  - (Map) Selection controls to edit multiple FGs at once
  - (Map) Lots of backend work to improve performance/code handling
  - (Map) Keyboard/mouse controls reworked
  - (OfficerPreview) Now opens with a question selected
  - (OfficerPreview) Right-click navigation to previous page
  - (Options) In addition to the new stuff, added a callback to refresh the main forms after update
  - (TIE) Permadeath unknowns implemented
  - (TIE) Can now only launch one OfficerPreview window
  - (TIE) EndOfMissionMessages now display in their assigned color
  - (TIE) Officer briefing questions now have a "best fit" function to automate line breaks prior to preview
  - (XvT) Blank Team names now shown as generic instead of actually blank
  - (XvT) Order speed control changed to drop-down
  - (XvT) Extra craft options added
  - (XvT) PreventCraftNumbering, DepartureClock and GoalTimeLimit unknowns implemented
  - (XWA) Order speed control changed to drop-down
  - (XWA) more controls so Roles can be correctly applied to teams individually
  - (XWA) Extra craft options added
  - (XWA) Mission correctly flagged as modified after using Hyperbuoy wizard
  - (XWA) Pilot control changed to drop-down
  - (XWA) Message Delay changed to a single Seconds control, the previous Minutes value is now an Unk
- And I took care of a couple things too...
  - (OfficerPreview) Fixed page count and highlighting display
  - (OfficerPreview) Bad characters now display a red 'X' instead of crashing
  - (XWA) Craft markings now allow values beyond the original 4 [Issue [#21](https://github.com/MikeG621/YOGEME/issues/21)]
 
#### v1.4.3, 09 May 2018
- (All) Code change for splitting some strings
- (XWA) Added distances to Prox Trigger display and appropriate ComboBoxes
- (XWA) Added a label that appears for Escort orders to explain the selected Position [Issue [#18](https://github.com/MikeG621/YOGEME/issues/18)]
- (XWA) Order speed now displays as MGLT instead of raw value
- (XWA) TriggerType unknowns filled in (JeremyAnsel) [Issue [#19](https://github.com/MikeG621/YOGEME/issues/19)]

#### v1.4.2, 24 Feb 2018
- (XWA) Updated Platform.dll to fix Order WP inversion [Issue [#16](https://github.com/MikeG621/YOGEME/issues/16)]
- (Settings) Added explicit Steam detection for all platforms [Issue [#17](https://github.com/MikeG621/YOGEME/issues/17)]
- (MissionVerify) Updated MV to remove containers, backdrops, probes, etc from AI and Orders checks [Issue [#15](https://github.com/MikeG621/YOGEME/issues/15)]
- (MissionVerfiy) Made various other changes, covered in v1.4.1r2 release

#### v1.4.1, 18 Nov 2018
- (XWA) Added DTM's SuperBackdrops support [Issue [#13](https://github.com/MikeG621/YOGEME/issues/13)]
  - (Options) New setting on the XWA tab to initialize new missions with SBD
  - Clicking the 'Backdrops' button on the main FlightGroup tab changes the text to "Loading" due to longer load times from high-res images.
- (XWA) Special Cargo text box visibility resets properly when toggling Backdrops
- (XWA) Explosion Time numerical control toggles usability properly when toggling Backdrops
- (XWA) Removed Backdrops from "Craft within 30 sec" count
- (All) Added Exclamation icon to FlightGroup Delete confirmation dialog
- (Backdrop) Fixed loading initial image if index was zero.
- (Misc) Updated the install.log to add the shiplist templates from 1.4 and the switch to MPL back from 1.2.3

#### v1.4, 16 Oct 2017
- (XWA) Added Hyperbuoy dialog [Issue [#13](https://github.com/MikeG621/YOGEME/issues/13)]
- (XvT/XWA) Made LST vertically resizable [Issue [#11](https://github.com/MikeG621/YOGEME/issues/11)]
- (Map) Form is now resizable, can be maximized [Issue [#11](https://github.com/MikeG621/YOGEME/issues/11)]
- (All) Added ability to replace default craft list, included templates [Issue [#10](https://github.com/MikeG621/YOGEME/issues/10)]

#### v1.3, 7 Jan 2017
- Lots of fixes and new features by Random Starfighter (JB)
  - RecentMission startup improved
  - (Settings) MRU handling updated, ConfirmFGDelete added
  - (Backdrop) Fixed a crash point it if can't find the backdrop resources
  - (Briefings) Various crash fixes, better event movement handling, multiple briefings capability added, button images tweaked
  - (GoalSummary) New feature
  - (All) FG Goal Summary
  - (All) Various crash fixes
  - (All) MRU capability
  - (All) Delete menu item, delete key capture
  - (All) Redo open mission procedure
  - (All) Added craft reference adjustment when deleting FGs
  - (TIE) Fixed copy/paste trigger failures
  - (TIE) Fixed Global And/Or goal assignments
  - (TIE) Changing briefing officer now resets to first question
  - (XvT) New FlightGroup function now returns BOOL
  - (XvT) Fixed SpecialCargo assignment
  - (XvT) Fixed various Label refresh issues
  - (XWA) Added Ion Pulse warhead
  - (XWA) Various typos
  - (XWA) Fixed various trigger indexes
  - (XWA) Fixed Special Cargo assignment
  - (XWA) Fixed various copy/paste issues
  - (XWA) Added missing ActiveSequence handlers
- (OfficersPreview) Form enlarged to account for W10 window style

#### v1.2.8, 06 June 2016
- (Test) Changed how YOGEME detects the running game due to how Steam handles it

#### v1.2.7 - 05 Apr 2015
- (XvT) Fixed Team copy/paste functions.
- (XvT) Added copy/paste mouse functions to Team listing.
- (XvT) FG Goal strings were saving in the wrong order.
- (XvT) FG Goal copy/paste now gets entire goal with strings and points, not just trigger.
- (XvT) Updated Platform Global Goal implementation.
- Fixed a bug where using the Verify tool with a space in the file path would show the full GUI instead of straight to results. [Issue [#7](https://github.com/MikeG621/YOGEME/issues/7)]

#### v1.2.6 - 09 Feb 2015
- (XWA) Fixed a bug where the Save/Exit dialogs would prompt twice. [Issue [#6](https://github.com/MikeG621/YOGEME/issues/6)]
- Rebuilt with latest Idmr.Platform.dll [Issues [#5](https://github.com/MikeG621/YOGEME/issues/5) and [#8](https://github.com/MikeG621/YOGEME/issues/8)]
 
#### v1.2.5 - 10 Jan 2015
- (All) Common.Update changed to generic, appropriate changes made (JeremyAnsel) [Issue [#3](https://github.com/MikeG621/YOGEME/issues/3)]

#### v1.2.4 - 15 Dec 2014
- (Settings) Fixed another crash point for x64 installs (JeremyAnsel) [Issue [#1](https://github.com/MikeG621/YOGEME/issues/1)]
 
#### v1.2.3 - 14 Dec 2014
- Updated install.log so the uninstaller will remove the test pilot files
- Changed license to MPL
- (Settings) Fixed crash looking for installed platforms in Win 8
- (Battle) Fixed crash when TIE is not installed
- (LST) Fixed blank windows/crashes when XvT/XWA is not installed
- All included .dlls have been updated
 
#### v1.2.2 - 22 Oct 2012
- Opening a mission file from the command line or "Open With..." no longer shows the Start dialog
- Packaged the pilot files so the Test function can work
 
#### v1.2.1 - 07 Oct 2012
- Fixed a crash that occurs during a first run, had to do with initializing the Settings properly
 
#### v1.2 - 06 Oct 2012
- (All) Test menu item added, Options Dialog has Test options enabled
- (All) Label with number of starting craft now only applies to Normal difficulty
- (All) Open/Save dialogs now default to the appropriate mission directory
- (All) "Open Recent" menu added, remembers the last five missions opened
- (XvT) Global Goal text boxes now save properly
- (Battle) Fixed bug in image processing that would corrupt LFD file
- Back-end updates
 
#### v1.1.1 - 14 Aug 2012
- (All) Waypoint.Enabled now updates when changing state instead of when
   losing focus
- (Map) Changed how waypoints were handled, will now interact with platform
   GUI properly
- (Settings) BRF2-8 now save properly
- (TIE) Added controls for FlightGroup.Unknowns 19-21
- (XvT) FG list once again displays properly
- (XWA) Fixed unhandled exception when changing Regions on the Orders tab 
- Idmr.Platform.dll updated to v2.0.1
  - (BaseBriefing) Fixed bug regarding StartLength calculation
  - (*.FlightGroup) Fixed bug preventing proper SpecialCargoCraft handling during Load/Save
  - (Tie.FlightGroup) Added Unknowns 19-21
  - (Tie.Officers) Fixed bug in `Save()` causing '[' and ']' to save as characters instead of the appropriate highlighiting codes
  - (Xvt.Briefing) Fixed bug in `Save()` prevent proper Events writing
  - (Xvt.Mission) Fixed critical bug in `LoadMission()` that resulted in unhandled exception
  - (Xwa.Mission) Fixed critical bug in `Save()` causing infinite loop and filesize
- Back-end updates

#### v1.1 - 15 July 2012
- (XWA) Backdrop interface will detect and load custom backdrops for XWA only
- (TIE) Briefing and Debriefing officer question preview added, displays as it
would in-game.
- Back-end updates

#### v1.0 - 21 Sept 2011
- Release

---
#### Special Thanks

All the modders and programmers over the last 25+ years who started the grunt
work that made editors like this possible, notably Evan Sabatelli for TIE
Fighter Workshop, Troy Dangerfield for both XvtED and AlliED, and countless
others who did the initial research into the mission file formats.

#### Copyright Information
Copyright © 2007- Michael Gaisser  
This program and related files are licensed under the Mozilla Public License.
See [License.txt](License.txt) for the full text. If for some reason License.txt was not
distributed with this program, you can obtain the full text of the license at
http://mozilla.org/MPL/2.0/.

The LZMA SDK is written and placed in the public domain by [Igor Pavlov](https://www.7-zip.org/sdk.html).

The BCn library and implementation are Copyright © [2020-2021 Richard Geldreich, Jr](https://github.com/richgel999/bc7enc_rdo)
and [2024 Jérémy Ansel](https://github.com/JeremyAnsel/JeremyAnsel.BcnSharp), covered by the MIT License.
See [MIT License.txt](MIT%20License.txt) for full details.

"Star Wars" and related items are trademarks of LucasFilm Ltd and
LucasArts Entertainment Co.