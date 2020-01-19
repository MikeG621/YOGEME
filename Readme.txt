Ye Olde Galactic Empire Mission Editor
======================================

Author: Michael Gaisser (mjgaisser@gmail.com)
Version: 1.6.4
Date: 2020.01.19

Thank you for downloading YOGEME, the all-in-one mission editor for the X-Wing
series, Xwing95 through X-wing Alliance.  This program allows easy mission
creation for all of your favorite Star Wars flight sims in a single platform,
and has abilities and mission parameters that have been discovered since the
original mainstream editors were created.

==========
Version History

v1.6.4, 19 Jan 2020
- (All) Added briefing callbacks so mission doesn't dirty until something is changed [Issue #30]
- (TIE) Battle form Height increased to account for W10 visual style [Issue #31]
- (TIE, XvT) Backdrop dialog now dirties mission on OK

v1.6.3, 01 Jan 2020
- (All) Fixed a settings write corruption due to partial platform detection [Issue #29]
- (XWA) Added ShuttleAnimation and ShuttleAnimationStraightLine hooks

v1.6.2, 28 Sep 2019
- (All) Fixed a crash that'd occur if Verify was run with a bad path [Issue #28]
- (XWA) Tweaked the hook INI backup filename during save to prevent possible clashes
- (XWA) Fixed error during hooked backdrop load
- (XWA) Idmr.ImageFormat.Dat.dll updated to handle Format 25 images correctly for backdrops

v1.6.1, 16 Sep 2019
 - (XWA) Fixed a crash that'd occur if the mission INI file doesn't exist [Issue #27]

v1.6, 15 Sep 2019
 - (XWA) Mission Hook editing dialog added for the hooks by Jérémy Ansel (JeremyaFr) [Issue #26]
 - (XWA) SuperBackdrops detection changed to Readme instead of Backup since XWAUCP is different than a solo install
 - (XWA) Backdrop "Loading..." button text now always appears, not just when SBD detected 
 - (XWA) Backdrop dialog now always resized for XWA to accommodate larger images, not just when SBD detected
 - (XWA) Changed some exceptions when trying to launch the Backdrop dialog and the platform isn't detected

v1.5.1, 13 May 2019
While I did add the below a couple months ago and just haven't gone through releasing it,
turns out I forgot to finish updating Converter.exe and package it. Oops. [Issue #25]
 - (All) Changing GG or GU value will now prompt to update references throughout if it's the only FG with that designation

v1.5, 10 Sep 2018
This most likely isn't a full list of everything that's visible to the user, but you get the idea; there's a lot.
 - Lots of fixes and new features by Random Starfighter (JB)
 -- X-wing 95 platform support added
 -- (All) Tons of back-end stuff I'm not going to spell out here
 -- (All) Theme support for shaded labels (orders, triggers, etc)
 -- (All) Non-platform-matching filestreams now close properly
 -- (All) Colorized drop-downs
 -- (All) IFF colors tweaked a little
 -- (All) Lots of controls throughout changed to instant-update instead of needing to leave the control
 -- (All) Copy/Paste abilities expanded
 -- (All) Blank message are now shown
 -- (All) General performance improvements
 -- (All) 'Enter' key commits control update
 -- (All) Move FG/Message Up/Down buttons added
 -- (All) Editor-only craft numbering added to FGs with identical names
 -- (All) Fixed an error when there's no Messages and a Message trigger becomes selected
 -- (Backdrops) Fixed some XWA-specific errors when loading
 -- (Briefings) Loading forces a refresh properly
 -- (Briefings) Tooltips added
 -- (Briefings) Fast Foward now limited to prevent ridiculous speeds
 -- (Briefings) Playback speed now shown if not 1x
 -- (Briefings) Fixed/tweaked various display issues
 -- (Briefings) Clicking on the caption will now advance to the next caption event
 -- (Briefings) XWA Tags now use Icon number
 -- (Briefings) New event button now adds after current line instead of at the end
 -- (Hyperbuoy) Added Beacon orders, fixed Roles, and tweaked AD settings
 -- (Map) Show/Hide functionality added
 -- (Map) Selection controls to edit multiple FGs at once
 -- (Map) Lots of backend work to improve performance/code handling
 -- (Map) Keyboard/mouse controls reworked
 -- (OfficerPreview) Now opens with a question selected
 -- (OfficerPreview) Right-click navigation to previous page
 -- (Options) In addition to the new stuff, added a callback to refresh the main forms after update
 -- (TIE) Permadeath unknowns implemented
 -- (TIE) Can now only launch one OfficerPreview window
 -- (TIE) EndOfMissionMessages now display in their assigned color
 -- (TIE) Officer briefing questions now have a "best fit" function to automate line breaks prior to preview
 -- (XvT) Blank Team names now shown as generic instead of actually blank
 -- (XvT) Order speed control changed to drop-down
 -- (XvT) Extra craft options added
 -- (XvT) PreventCraftNumbering, DepartureClock and GoalTimeLimit unknowns implemented
 -- (XWA) Order speed control changed to drop-down
 -- (XWA) more controls so Roles can be correctly applied to teams individually
 -- (XWA) Extra craft options added
 -- (XWA) Mission correctly flagged as modified after using Hyperbuoy wizard
 -- (XWA) Pilot control changed to drop-down
 -- (XWA) Message Delay changed to a single Seconds control, the previous Minutes value is now an Unk
 - And I took care of a couple things too...
 -- (OfficerPreview) Fixed page count and highlighting display
 -- (OfficerPreview) Bad characters now display a red 'X' instead of crashing
 -- (XWA) Craft markings now allow values beyond the original 4 [Issue #21]
 
v1.4.3, 09 May 2018
 - (All) Code change for splitting some strings
 - (XWA) Added distances to Prox Trigger display and appropriate ComboBoxes
 - (XWA) Added a label that appears for Escort orders to explain the selected Position [Issue #18]
 - (XWA) Order speed now displays as MGLT instead of raw value
 - (XWA) TriggerType unknowns filled in (JeremyAnsel) [Issue #19]

v1.4.2, 24 Feb 2018
 - (XWA) Updated Platform.dll to fix Order WP inversion [Issue #16]
 - (Settings) Added explicit Steam detection for all platforms [Issue #17]
 - (MissionVerify) Updated MV to remove containers, backdrops, probes, etc from AI and Orders checks [Issue #15]
 - (MissionVerfiy) Made various other changes, covered in v1.4.1r2 release

v1.4.1, 18 Nov 2018
 - (XWA) Added DTM's SuperBackdrops support [Issue #13]
 -- (Options) New setting on the XWA tab to initialize new missions with SBD
 -- Clicking the 'Backdrops' button on the main FlightGroup tab changes the text to "Loading" due to longer load times from high-res images.
 - (XWA) Special Cargo text box visibility resets properly when toggling Backdrops
 - (XWA) Explosion Time numerical control toggles usability properly when toggling Backdrops
 - (XWA) Removed Backdrops from "Craft within 30 sec" count
 - (All) Added Exclamation icon to FlightGroup Delete confirmation dialog
 - (Backdrop) Fixed loading initial image if index was zero.
 - (Misc) Updated the install.log to add the shiplist templates from 1.4 and the switch to MPL back from 1.2.3

v1.4, 16 Oct 2017
 - (XWA) Added Hyperbuoy dialog [Issue #13]
 - (XvT/XWA) Made LST vertically resizable [Issue #11]
 - (Map) Form is now resizable, can be maximized [Issue #11]
 - (All) Added ability to replace default craft list, included templates [Issue #10]

v1.3, 7 Jan 2017
 -Lots of fixes and new features by Random Starfighter (JB)
 -- RecentMission startup improved
 -- (Settings) MRU handling updated, ConfirmFGDelete added
 -- (Backdrop) Fixed a crash point it if can't find the backdrop resources
 -- (Briefings) Various crash fixes, better event movement handling, multiple briefings capability added, button images tweaked
 -- (GoalSummary) New feature
 -- (All) FG Goal Summary
 -- (All) Various crash fixes
 -- (All) MRU capability
 -- (All) Delete menu item, delete key capture
 -- (All) Redo open mission procedure
 -- (All) Added craft reference adjustment when deleting FGs
 -- (TIE) Fixed copy/paste trigger failures
 -- (TIE) Fixed Global And/Or goal assignments
 -- (TIE) Changing briefing officer now resets to first question
 -- (XvT) New FlightGroup function now returns BOOL
 -- (XvT) Fixed SpecialCargo assignment
 -- (XvT) Fixed various Label refresh issues
 -- (XWA) Added Ion Pulse warhead
 -- (XWA) Various typos
 -- (XWA) Fixed various trigger indexes
 -- (XWA) Fixed Special Cargo assignment
 -- (XWA) Fixed various copy/paste issues
 -- (XWA) Added missing ActiveSequence handlers
 - (OfficersPreview) Form enlarged to account for W10 window style

v1.2.8, 06 June 2016
 - (Test) Changed how YOGEME detects the running game due to how Steam handles it

v1.2.7 - 05 Apr 2015
 - (XvT) Fixed Team copy/paste functions.
 - (XvT) Added copy/paste mouse functions to Team listing.
 - (XvT) FG Goal strings were saving in the wrong order.
 - (XvT) FG Goal copy/paste now gets entire goal with strings and points, not just trigger.
 - (XvT) Updated Platform Global Goal implementation.
 - Fixed a bug where using the Verify tool with a space in the file path would show the full GUI instead of straight to results. [Issue #7]

v1.2.6 - 09 Feb 2015
 - (XWA) Fixed a bug where the Save/Exit dialogs would prompt twice. [Issue #6]
 - Rebuilt with latest Idmr.Platform.dll [Issues #5 and #8]
 
v1.2.5 - 10 Jan 2015
 - (All) Common.Update changed to generic, appropriate changes made (JeremyAnsel) [Issue #3]

v1.2.4 - 15 Dec 2014
 - (Settings) Fixed another crash point for x64 installs (JeremyAnsel) [Issue #1]
 
v1.2.3 - 14 Dec 2014
 - Updated install.log so the uninstaller will remove the test pilot files
 - Changed license to MPL
 - (Settings) Fixed crash looking for installed platforms in Win 8
 - (Battle) Fixed crash when TIE is not installed
 - (LST) Fixed blank windows/crashes when XvT/XWA is not installed
 - All included .dlls have been updated
 
v1.2.2 - 22 Oct 2012
 - Opening a mission file from the command line or "Open With..." no longer shows the Start dialog
 - Packaged the pilot files so the Test function can work
 
v1.2.1 - 07 Oct 2012
 - Fixed a crash that occurs during a first run, had to do with initializing the Settings properly
 
v1.2 - 06 Oct 2012
 - (All) Test menu item added, Options Dialog has Test options enabled
 - (All) Label with number of starting craft now only applies to Normal difficulty
 - (All) Open/Save dialogs now default to the appropriate mission directory
 - (All) "Open Recent" menu added, remembers the last five missions opened
 - (XvT) Global Goal text boxes now save properly
 - (Battle) Fixed bug in image processing that would corrupt LFD file
 - Back-end updates
 
v1.1.1 - 14 Aug 2012
 - (All) Waypoint.Enabled now updates when changing state instead of when
   losing focus
 - (Map) Changed how waypoints were handled, will now interact with platform
   GUI properly
 - (Settings) BRF2-8 now save properly
 - (TIE) Added controls for FlightGroup.Unknowns 19-21
 - (XvT) FG list once again displays properly
 - (XWA) Fixed unhandled exception when changing Regions on the Orders tab 
 - Idmr.Platform.dll updated to v2.0.1
 -- (BaseBriefing) Fixed bug regarding StartLength calculation
 -- (*.FlightGroup) Fixed bug preventing proper SpecialCargoCraft handling during Load/Save
 -- (Tie.FlightGroup) Added Unknowns 19-21
 -- (Tie.Officers) Fixed bug in Save() causing '[' and ']' to save as characters instead of the appropriate highlighiting codes
 -- (Xvt.Briefing) Fixed bug in Save() prevent proper Events writing
 -- (Xvt.Mission) Fixed critical bug in LoadMission() that resulted in unhandled exception
 -- (Xwa.Mission) Fixed critical bug in Save() causing infinite loop and filesize
 - Back-end updates

v1.1 - 15 July 2012
 - (XWA) Backdrop interface will detect and load custom backdrops for XWA only
 - (TIE) Briefing and Debriefing officer question preview added, displays as it
would in-game.
 - Back-end updates

v1.0 - 21 Sept 2011
 - Release
 
==========
Installation Notes

No special setup is needed, simply unzip and run. YOGEME will auto-detect your
installed platforms and enable those editing capabilities (you shouldn't be
making missions for a platform you can't play, they tend to be bug-ridden),
however if you absolutely need a disabled platform, it can be activated in the
Options dialog. Note however that certain functions may not work properly for
an uninstalled platform.

==========
Uninstallation Notes

Run Uninstall.exe, it will remove the settings file in your user directory and
all original YOGEME files.

==========
Special Thanks

All the modders and programmers over the last 15 years who started the grunt
work that made editors like this possible, notably Evan Sabatelli for TIE
Fighter Workshop, Troy Dangerfield for both XvtED and AlliED, and countless
others who did the initial research into the mission file formats.

==========
Copyright Information

Copyright © 2007- Michael Gaisser
This program and related files are licensed under the Mozilla Public License.
See MPL.txt for the full text. If for some reason MPL.txt was not distributed
with this program, you can obtain the full text of the license at
http://mozilla.org/MPL/2.0/.

The Galactic Empire: Empire Reborn is Copyright © 2004- Tiberius Fel

"Star Wars" and related items are trademarks of LucasFilm Ltd and
LucasArts Entertainment Co.

THESE FILES HAVE BEEN TESTED AND DECLARED FUNCTIONAL BY THE IDMR, AS SUCH THE
IDMR AND THE GALACTIC EMPIRE: EMPIRE REBORN CANNOT BE HELD RESPONSIBLE OR
LIABLE FOR UNWANTED EFFECTS DUE ITS USE OR MISUSE. THIS SOFTWARE IS OFFERED AS
IS WITHOUT WARRANTY OF ANY KIND.