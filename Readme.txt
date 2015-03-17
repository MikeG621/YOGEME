Ye Olde Galactic Empire Mission Editor
======================================

Author: Michael Gaisser (mjgaisser@gmail.com)
Version: 1.2.6
Date: 2015.02.09

Thank you for downloading YOGEME, the all-in-one mission editor for the X-Wing
series, TIE95 through X-wing Alliance.  This program allows easy mission
creation for all of your favorite Star Wars flight sims in a single platform,
and has abilities and mission parameters that have been discovered since the
original mainstream editors were created.

==========
Version History

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