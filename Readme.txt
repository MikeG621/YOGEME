Ye Olde Galactic Empire Mission Editor
======================================

Author: Michael Gaisser (mjgaisser@gmail.com)
Version: 1.4.2
Date: 2018.02.24

Thank you for downloading YOGEME, the all-in-one mission editor for the X-Wing
series, TIE95 through X-wing Alliance.  This program allows easy mission
creation for all of your favorite Star Wars flight sims in a single platform,
and has abilities and mission parameters that have been discovered since the
original mainstream editors were created.

==========
Version History

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