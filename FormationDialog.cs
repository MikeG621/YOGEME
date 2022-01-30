/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, XW through XWA
 * Copyright (C) 2007-2022 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the MPL v2.0 or later
 * 
 * VERSION: 1.13
 */

/* CHANGELOG
 * v1.13, 220130
 * [UPD] Redesign [JB]
 * v1.7, 200816
 * [UPD] Images are now foreground instead of background [JB]
 * v1.2.3, 141214
 * [UPD] change to MPL
 * v1.1.1, 120814
 * - class renamed
 * v1.0, 110921
 * - Release
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Idmr.Yogeme
{
	/// <summary>Dialog to visually choose Flightgroup Formation</summary>
	public partial class FormationDialog : Form
	{
		public int Formation { get { return cboFormation.SelectedIndex; } }
		public int Spacing { get { return (int)numFormSpacing.Value; } }

		/// <summary>Raw data array to retrieve formation positions. Will be initialized based on platform.</summary>
		short[] _activeDataArray = null;
		/// <summary>Count of array elements in each axis. Used to calculate offsets for the X,Y,Z subsections of the data array. Will be initialized based on platform.</summary>
		int _activeAxisDataCount = 0;

		Settings.Platform _platform;
		readonly Bitmap _canvas;
		readonly Bitmap _frontIcon;
		readonly Bitmap _sideIcon;
		readonly Bitmap _topIcon;

		#region Raw Data
		#region Bitmap Icons
		/// <summary>10x10 icon graphic for front view.</summary>
		static readonly byte[] _frontIconRaw = new byte[100] {
			0,0,0,0,0,0,0,0,0,0,
			0,0,0,0,0,0,0,0,0,0,
			0,1,0,0,0,0,0,1,0,0,
			1,0,0,0,1,0,0,0,1,0,
			1,0,0,1,1,1,0,0,1,0,
			1,1,1,1,1,1,1,1,1,0,
			1,0,0,1,1,1,0,0,1,0,
			1,0,0,0,1,0,0,0,1,0,
			0,1,0,0,0,0,0,1,0,0,
			0,0,0,0,0,0,0,0,0,0 };

		/// <summary>10x10 icon graphic for side view.</summary>
		static readonly byte[] _sideIconRaw = new byte[100] {
			0,0,0,0,0,0,0,0,0,0,
			0,0,0,0,0,0,0,0,0,0,
			0,1,1,1,0,0,0,0,0,0,
			0,1,1,1,1,1,1,0,0,0,
			1,1,1,1,1,1,1,1,1,1,
			1,1,1,1,0,0,0,0,0,0,
			1,1,1,1,1,1,1,1,1,1,
			0,1,1,1,1,1,1,0,0,0,
			0,1,1,1,0,0,0,0,0,0,
			0,0,0,0,0,0,0,0,0,0 };

		/// <summary>10x10 icon graphic for top view.</summary>
		static readonly byte[] _topIconRaw = new byte[100] {
			1,0,0,0,0,0,0,0,1,0,
			1,0,0,0,0,0,0,0,1,0,
			1,0,0,0,0,0,0,0,1,0,
			1,1,0,0,0,0,0,1,1,0,
			1,1,0,0,1,0,0,1,1,0,
			1,1,0,1,1,1,0,1,1,0,
			1,1,1,1,1,1,1,1,1,0,
			1,1,0,1,1,1,0,1,1,0,
			1,1,0,0,1,0,0,1,1,0,
			1,0,0,0,0,0,0,0,1,0 };
		#endregion Bitmap Icons

		#region XWing Data
		/// <summary>Raw formation data for X-Wing, 10 formations (60 Int16).</summary>
		/// <remarks>File offset 0xC7780 in XWING95.EXE, memory offset 0x4C9180.</remarks>
		static readonly short[] _formationDataXwing = new short[] {
			0, 2,-2, 0, 2,-2, // 0   X offsets
			0,-2, 6, 7,-6,-7, // 1
			0, 0, 0, 0, 0, 0, // 2
			0, 1,-1, 2,-2, 3, // 3
			0, 1, 2, 3, 4, 5, // 4
			0,-1,-2,-3,-4,-5, // 5
			0, 0, 0,-2,-2,-2, // 6
			0, 1, 0,-1, 0, 0, // 7
			0, 0, 0, 0, 0, 0, // 8
			0, 1,-1, 1,-1, 0, // 9
			0,-2,-2,-4,-6,-6, // 0   Y offsets
			0,-2,-6,-7,-6,-7, // 1
			0,-1,-2,-3,-4,-5, // 2
			0, 0, 0, 0, 0, 0, // 3
			0,-1,-2,-3,-4,-5, // 4
			0,-1,-2,-3,-4,-5, // 5
			0,-1,-2, 0,-1,-2, // 6
			0,-1,-2,-1,-1,-1, // 7
			0, 0, 0, 0, 0, 0, // 8
			0, 0, 0, 0, 0,-1, // 9
			0, 1, 1, 4, 5, 5, // 0   Z offsets
			0,-1, 5, 4, 5, 4, // 1
			0, 1, 2, 3, 4, 5, // 2
			0, 0, 0, 0, 0, 0, // 3
			0, 0, 0, 0, 0, 0, // 4
			0, 0, 0, 0, 0, 0, // 5
			0, 0, 0, 0, 0, 0, // 6
			0, 0, 0, 0, 1,-1, // 7
			0, 1, 2, 3, 4, 5, // 8
			0, 1, 1,-1,-1, 0, // 9
			// Overflow garbage into horizontal spacing array
			1000, 1350, 1800, 2250, 2600, 3000,
		};
		/// <summary>Data element count for each axis</summary>
		const int _axisDataCountXwing = 60;
		/// <summary>Spacing on the horizontal plane (X, Y axes)</summary>
		static readonly short[] _formationDataXwingHorizSpacing = { 1000, 1350, 1800, 2250, 2600, 3000, 3250, 3400, 3500, 250, 0, 0 };
		/// <summary>Spacing on the vertical Z axis</summary> 
		static readonly short[] _formationDataXwingVertSpacing = { 500, 700, 1150, 1600, 2050, 2400, 2850, 2100, 3300, 75, 0, 0 };
		#endregion XWing Data

		#region Tie Data
		/// <summary>Raw formation data for TIE Fighter, 13 formations (78 Int16) plus padding up to 80 Int16.</summary>
		/// <remarks>File offset 0xE4578 in TIE95.EXE, memory offset 0x4E6578.</remarks>
		static readonly short[] _formationDataTieRaw = new short[] {
			 0, 1,-1, 2,-2, 3, // 0   X offsets
			 0,-2, 4, 5,-4,-5, // 1
			 0, 0, 0, 0, 0, 0, // 2
			 0, 1,-1, 2,-2, 3, // 3
			 0, 1, 2, 3, 4, 5, // 4
			 0,-1,-2,-3,-4,-5, // 5
			 0,-1, 0,-1, 0,-1, // 6
			 0, 1, 0,-1, 0, 0, // 7
			 0, 0, 0, 0, 0, 0, // 8
			 0, 1,-1, 1,-1, 0, // 9
			 0, 1,-1, 2,-2, 3, // 10
			 0, 0, 0, 0, 0, 0, // 11
			 0, 0, 0, 0, 0, 0, // 12
			 0, 0,             // padding
			 0,-1,-1,-2,-2,-3, // 0   Y offsets
			 0,-2,-4,-5,-4,-5, // 1
			 0,-1,-2,-3,-4,-5, // 2
			 0, 0, 0, 0, 0, 0, // 3
			 0,-1,-2,-3,-4,-5, // 4
			 0,-1,-2,-3,-4,-5, // 5
			 0, 0,-1,-1,-2,-2, // 6
			 0,-1,-2,-1,-1,-1, // 7
			 0, 0, 0, 0, 0, 0, // 8
			 0, 0, 0, 0, 0,-1, // 9
			 0, 1, 1, 2, 2, 3, // 10
			 0,-1,-1,-2,-2,-3, // 11
			 0, 1, 1, 2, 2, 3, // 12
			 0, 0,             // padding
			 0, 0, 0, 0, 0, 0, // 0   Z offsets
			 0,-1, 2, 1, 3, 2, // 1
			 0, 1, 2, 3, 4, 5, // 2
			 0, 0, 0, 0, 0, 0, // 3
			 0, 0, 0, 0, 0, 0, // 4
			 0, 0, 0, 0, 0, 0, // 5
			 0, 0, 0, 0, 0, 0, // 6
			 0, 0, 0, 0, 1,-1, // 7
			 0, 1, 2, 3, 4, 5, // 8
			 0, 1, 1,-1,-1, 0, // 9
			 0, 0, 0, 0, 0, 0, // 10
			 0, 1,-1, 2,-2, 3, // 11
			 0, 1,-1, 2,-2, 3, // 12
			 // 2 elements of padding, plus 4 elements of overflow garbage
			 0, 0, -32768, 0,-16384, 0
		};
		/// <summary>Data element count for each axis</summary>
		const int _axisDataCountTie = 80;
		#endregion Tie Data

		#region Xvt Data
		/// <summary>Raw formation data for XvT and BOP, 34 formations (204 Int16)</summary>
		/// <remarks>File offset 0x125370 in BOP Z_XVT__.EXE, memory offset 0x527970.</remarks>
		static readonly short[] _formationDataXvtRaw = new short[] {
			 0, 1,-1, 2,-2, 3, // 0   X offsets
			 0, 1,-2,-3, 2, 3, // 1
			 0, 0, 0, 0, 0, 0, // 2
			 0, 1,-1, 2,-2, 3, // 3
			 1, 2, 3, 4, 5, 6, // 4
			-1,-2,-3,-4,-5,-6, // 5
			 0,-1, 0,-1, 0,-1, // 6
			 0, 1,-1, 0, 0, 0, // 7
			 0, 0, 0, 0, 0, 0, // 8
			 0, 1,-1, 1,-1, 0, // 9
			 0, 1,-1, 2,-2, 3, // 10
			 0, 0, 0, 0, 0, 0, // 11
			 0, 0, 0, 0, 0, 0, // 12
			 0, 0, 0, 0, 0, 0, // 13
			 0, 0, 0, 0, 0, 0, // 14
			 0, 1, 2, 3, 4, 5, // 15
			-1,-2,-3,-4,-5,-6, // 16
			 0, 0, 1, 2, 3, 4, // 17
			 1, 2, 3, 4, 5, 6, // 18
			 0, 0, 0, 0, 0, 0, // 19
			 0, 0, 0, 0, 0, 0, // 20
			 0, 1,-1, 2,-2, 3, // 21
			 0, 1,-1, 2,-2, 3, // 22
			-1, 1,-1, 1,-1, 1, // 23
			 0, 0, 0, 0, 0, 0, // 24
			-1, 1,-1, 1,-1, 1, // 25
			 0, 0, 0,-1, 1, 0, // 26
			 0, 1,-1, 0, 0, 0, // 27
			 0, 2,-3, 3,-2, 0, // 28
			 0, 0, 0, 0, 0, 0, // 29
			 0, 2,-3, 3,-2, 0, // 30
			-1, 1,-2, 2,-1, 1, // 31
			 0, 0, 0, 0, 0, 0, // 32
			-1, 1,-2, 2,-1, 1, // 33
			 0,-1,-1,-2,-2,-3, // 0   Y offsets
			 3, 2, 1, 0,-1,-2, // 1
			-1,-2,-3,-4,-5,-6, // 2
			 0, 0, 0, 0, 0, 0, // 3
			 0,-1,-2,-3,-4,-5, // 4
			 0,-1,-2,-3,-4,-5, // 5
			 0, 0,-1,-1,-2,-2, // 6
			 1, 0, 0,-1, 0, 0, // 7
			 0, 0, 0, 0, 0, 0, // 8
			 0, 0, 0, 0, 0,-1, // 9
			 1, 2, 2, 3, 3, 4, // 10
			 0,-1,-1,-2,-2,-3, // 11
			 1, 2, 2, 3, 3, 4, // 12
			 0, 1, 2, 3, 4, 5, // 13
			 0, 0, 0, 0, 0, 0, // 14
			 0, 0, 0, 0, 0, 0, // 15
			 0, 0, 0, 0, 0, 0, // 16
			 0, 1, 2, 3, 4, 5, // 17
			-1,-2,-3,-4,-5,-6, // 18
			 0,-1,-2,-3,-4,-5, // 19
			 0,-1,-2,-3,-4,-5, // 20
			 0, 0, 0, 0, 0, 0, // 21
			 0, 0, 0, 0, 0, 0, // 22
			 2, 2, 0, 0,-2,-2, // 23
			 2, 2, 0, 0,-2,-2, // 24
			 0, 0, 0, 0, 0, 0, // 25
			 1, 0, 0, 0, 0,-1, // 26
			 0, 0, 0, 0, 1,-1, // 27
			 3,-3, 1, 1,-3, 0, // 28
			 3,-3, 1, 1,-3, 0, // 29
			 0, 0, 0, 0, 0, 0, // 30
			 2, 2, 0, 0,-2,-2, // 31
			 2, 2, 0, 0,-2,-2, // 32
			 0, 0, 0, 0, 0, 0, // 33
			 0, 0, 0, 0, 0, 0, // 0   Z offsets
			 0, 1,-1,-2, 2, 3, // 1
			 0, 0, 0, 0, 0, 0, // 2
			 0, 0, 0, 0, 0, 0, // 3
			 0, 1, 2, 3, 4, 5, // 4
			 0, 1, 2, 3, 4, 5, // 5
			 0, 0, 0, 0, 0, 0, // 6
			 0, 0, 0, 0, 1,-1, // 7
			 0, 1, 2, 3, 4, 5, // 8
			 0, 1, 1,-1,-1, 0, // 9
			 0, 0, 0, 0, 0, 0, // 10
			 0, 1,-1, 2,-2, 3, // 11
			 0, 1,-1, 2,-2, 3, // 12
			 0, 0, 0, 0, 0, 0, // 13
			-1,-2,-3,-4,-5,-6, // 14
			 0, 0, 0, 0, 0, 0, // 15
			 0, 0, 0, 0, 0, 0, // 16
			 0, 1, 2, 3, 4, 5, // 17
			 1, 2, 3, 4, 5, 6, // 18
			 0, 1, 2, 3, 4, 5, // 19
			-1,-2,-3,-4,-5,-6, // 20
			 0, 1, 1, 2, 2, 3, // 21
			 0,-1,-1,-2,-2,-3, // 22
			 0, 0, 0, 0, 0, 0, // 23
			 1,-1, 1,-1, 1,-1, // 24
			 2, 2, 0, 0,-2,-2, // 25
			 0, 1,-1, 0, 0, 0, // 26
			 1, 0, 0,-1, 0, 0, // 27
			 0, 0, 0, 0, 0, 0, // 28
			 0, 2,-3, 3,-2, 0, // 29
			 3,-3, 1, 1,-3, 0, // 30
			 0, 0, 0, 0, 0, 0, // 31
			 1,-1, 2,-2, 1,-1, // 32
			 2, 2, 0, 0,-2,-2, // 33
			 // 6 elements of overflow garbage (the formation divisor array)
			 1, 3, 1, 1, 1, 1, // 0
		};
		/// <summary>Data element count for each axis</summary>
		const int _axisDataCountXvt = 204;
		#endregion Xvt Data

		#region Xwa Data
		/// <summary>Raw formation data for XWA, 34 formations (204 Int16)</summary>
		/// <remarks>The data is almost binary identical to XvT, except: </br>
		/// Y data for formation index 23 and 24, Z data for formation index 25.</br>
		/// XWA reduces their values from 2 and -2, to 1 and -1.</br>
		/// File offset 0x1B5C80 in XWINGALLIANCE.EXE, memory offset 0x5B7080.</remarks>
		static readonly short[] _formationDataXwaRaw = new short[] {
			 0, 1,-1, 2,-2, 3, // 0   X offsets
			 0, 1,-2,-3, 2, 3, // 1
			 0, 0, 0, 0, 0, 0, // 2
			 0, 1,-1, 2,-2, 3, // 3
			 1, 2, 3, 4, 5, 6, // 4
			-1,-2,-3,-4,-5,-6, // 5
			 0,-1, 0,-1, 0,-1, // 6
			 0, 1,-1, 0, 0, 0, // 7
			 0, 0, 0, 0, 0, 0, // 8
			 0, 1,-1, 1,-1, 0, // 9
			 0, 1,-1, 2,-2, 3, // 10
			 0, 0, 0, 0, 0, 0, // 11
			 0, 0, 0, 0, 0, 0, // 12
			 0, 0, 0, 0, 0, 0, // 13
			 0, 0, 0, 0, 0, 0, // 14
			 0, 1, 2, 3, 4, 5, // 15
			-1,-2,-3,-4,-5,-6, // 16
			 0, 0, 1, 2, 3, 4, // 17
			 1, 2, 3, 4, 5, 6, // 18
			 0, 0, 0, 0, 0, 0, // 19
			 0, 0, 0, 0, 0, 0, // 20
			 0, 1,-1, 2,-2, 3, // 21
			 0, 1,-1, 2,-2, 3, // 22
			-1, 1,-1, 1,-1, 1, // 23
			 0, 0, 0, 0, 0, 0, // 24
			-1, 1,-1, 1,-1, 1, // 25
			 0, 0, 0,-1, 1, 0, // 26
			 0, 1,-1, 0, 0, 0, // 27
			 0, 2,-3, 3,-2, 0, // 28
			 0, 0, 0, 0, 0, 0, // 29
			 0, 2,-3, 3,-2, 0, // 30
			-1, 1,-2, 2,-1, 1, // 31
			 0, 0, 0, 0, 0, 0, // 32
			-1, 1,-2, 2,-1, 1, // 33
			 0,-1,-1,-2,-2,-3, // 0   Y offsets
			 3, 2, 1, 0,-1,-2, // 1
			-1,-2,-3,-4,-5,-6, // 2
			 0, 0, 0, 0, 0, 0, // 3
			 0,-1,-2,-3,-4,-5, // 4
			 0,-1,-2,-3,-4,-5, // 5
			 0, 0,-1,-1,-2,-2, // 6
			 1, 0, 0,-1, 0, 0, // 7
			 0, 0, 0, 0, 0, 0, // 8
			 0, 0, 0, 0, 0,-1, // 9
			 1, 2, 2, 3, 3, 4, // 10
			 0,-1,-1,-2,-2,-3, // 11
			 1, 2, 2, 3, 3, 4, // 12
			 0, 1, 2, 3, 4, 5, // 13
			 0, 0, 0, 0, 0, 0, // 14
			 0, 0, 0, 0, 0, 0, // 15
			 0, 0, 0, 0, 0, 0, // 16
			 0, 1, 2, 3, 4, 5, // 17
			-1,-2,-3,-4,-5,-6, // 18
			 0,-1,-2,-3,-4,-5, // 19
			 0,-1,-2,-3,-4,-5, // 20
			 0, 0, 0, 0, 0, 0, // 21
			 0, 0, 0, 0, 0, 0, // 22
			 1, 1, 0, 0,-1,-1, // 23
			 1, 1, 0, 0,-1,-1, // 24
			 0, 0, 0, 0, 0, 0, // 25
			 1, 0, 0, 0, 0,-1, // 26
			 0, 0, 0, 0, 1,-1, // 27
			 3,-3, 1, 1,-3, 0, // 28
			 3,-3, 1, 1,-3, 0, // 29
			 0, 0, 0, 0, 0, 0, // 30
			 2, 2, 0, 0,-2,-2, // 31
			 2, 2, 0, 0,-2,-2, // 32
			 0, 0, 0, 0, 0, 0, // 33
			 0, 0, 0, 0, 0, 0, // 0   Z offsets
			 0, 1,-1,-2, 2, 3, // 1
			 0, 0, 0, 0, 0, 0, // 2
			 0, 0, 0, 0, 0, 0, // 3
			 0, 1, 2, 3, 4, 5, // 4
			 0, 1, 2, 3, 4, 5, // 5
			 0, 0, 0, 0, 0, 0, // 6
			 0, 0, 0, 0, 1,-1, // 7
			 0, 1, 2, 3, 4, 5, // 8
			 0, 1, 1,-1,-1, 0, // 9
			 0, 0, 0, 0, 0, 0, // 10
			 0, 1,-1, 2,-2, 3, // 11
			 0, 1,-1, 2,-2, 3, // 12
			 0, 0, 0, 0, 0, 0, // 13
			-1,-2,-3,-4,-5,-6, // 14
			 0, 0, 0, 0, 0, 0, // 15
			 0, 0, 0, 0, 0, 0, // 16
			 0, 1, 2, 3, 4, 5, // 17
			 1, 2, 3, 4, 5, 6, // 18
			 0, 1, 2, 3, 4, 5, // 19
			-1,-2,-3,-4,-5,-6, // 20
			 0, 1, 1, 2, 2, 3, // 21
			 0,-1,-1,-2,-2,-3, // 22
			 0, 0, 0, 0, 0, 0, // 23
			 1,-1, 1,-1, 1,-1, // 24
			 1, 1, 0, 0,-1,-1, // 25
			 0, 1,-1, 0, 0, 0, // 26
			 1, 0, 0,-1, 0, 0, // 27
			 0, 0, 0, 0, 0, 0, // 28
			 0, 2,-3, 3,-2, 0, // 29
			 3,-3, 1, 1,-3, 0, // 30
			 0, 0, 0, 0, 0, 0, // 31
			 1,-1, 2,-2, 1,-1, // 32
			 2, 2, 0, 0,-2,-2, // 33
			 // 6 elements of overflow garbage (the formation divisor array)
			 1, 3, 1, 1, 1, 1, // 0
		};
		/// <summary>Data element count for each axis</summary>
		const int _axisDataCountXwa = 204;
		#endregion #Xwa Data

		/// <summary>The divisor array is used by XvT and XWA.  It is identical in both platforms.</summary>
		/// <remarks>34 elements.</remarks>
		static readonly short[] _formationDivisor = new short[] {
			1,  3,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,
			1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  3,  3,  3,  2,
			2,  2
		};
		#endregion Raw Data

		/// <summary>Keeps track of an icon's tag position and numbering string, so that identical positions can be managed more gracefully before drawing.</summary>
		private class IconTag
		{
			public int X;
			public int Y;
			public string Tag;
			public IconTag(int x, int y, string tag) { X = x; Y = y; Tag = tag; }
		}

		/// <summary>Stores a craft formation position, in pixels, relative to the origin. May be negative.</summary>
		private class FormPosition
		{
			public int X;
			public int Y;
			public int Z;
			public FormPosition(int x, int y, int z) { X = x; Y = y; Z = z; }
		}

		public FormationDialog(int formationIndex, int spacing, Settings.Platform platform)
		{
			InitializeComponent();

			_canvas = new Bitmap(pctFormation.Width, pctFormation.Height);
			_frontIcon = createBitmapFromRaw(_frontIconRaw, 10, 10);
			_sideIcon = createBitmapFromRaw(_sideIconRaw, 10, 10);
			_topIcon = createBitmapFromRaw(_topIconRaw, 10, 10);

			setPlatform(platform);

			if (formationIndex < 0 || formationIndex >= cboFormation.Items.Count)
				formationIndex = 0;
			cboFormation.SelectedIndex = formationIndex;

			if (spacing < 0 || spacing > numFormSpacing.Maximum)
				spacing = 2;
			numFormSpacing.Value = spacing;

			lblFormInfo.BackColor = Color.Black;
			lblFormInfo.ForeColor = Color.LightGray;
		}

		/// <summary>Initializes the formation list, data arrays, and control visibility for the current working platform.</summary>
		private void setPlatform(Settings.Platform platform)
		{
			_platform = platform;
			cboFormation.Items.Clear();

			// XW does not have customizable spacing, but it does use formations exiting hangar.
			lblSpacing.Enabled = (platform != Settings.Platform.XWING);
			numFormSpacing.Enabled = lblSpacing.Enabled;
			chkFormHangar.Visible = (platform == Settings.Platform.XWING);

			switch (platform)
			{
				case Settings.Platform.XWING:
					_activeDataArray = _formationDataXwing;
					_activeAxisDataCount = _axisDataCountXwing;
					cboFormation.Items.AddRange(Platform.Xwing.Strings.Formation);
					break;
				case Settings.Platform.TIE:
					_activeDataArray = _formationDataTieRaw;
					_activeAxisDataCount = _axisDataCountTie;
					cboFormation.Items.AddRange(Platform.Tie.Strings.Formation);
					break;
				case Settings.Platform.XvT:
				case Settings.Platform.BoP:
					_activeDataArray = _formationDataXvtRaw;
					_activeAxisDataCount = _axisDataCountXvt;
					cboFormation.Items.AddRange(Platform.Xvt.Strings.Formation);
					break;
				case Settings.Platform.XWA:
					_activeDataArray = _formationDataXwaRaw;
					_activeAxisDataCount = _axisDataCountXwa;
					cboFormation.Items.AddRange(Platform.Xwa.Strings.Formation);
					break;
			}

			// Generate the static info text displayed in the unused quadrant of the preview canvas.
			string text = "";
			if (_platform == Settings.Platform.XWING)
			{
				text += "X-Wing formations are very large with no customizable spacing. The displayed spacing is scaled down by half just to fit into the preview. Fighters exiting from hangar have minimum spacing, but will use their formation. Non-fighters have a 4x spacing multiplier.";
			}
			else
			{
				text += "Craft exiting from hangar have different behavior. Zero spacing. Always Vic formation up to 3 craft per wave, Double Astern if more.";
				if (_platform != Settings.Platform.TIE)
					text += " They will use their assigned formation and spacing in regular flight. When returning to hangar, normal formation is maintained, but with zero spacing.";
			}
			text += Environment.NewLine + Environment.NewLine;
			text += "Formations are limited to a maximum of 6 craft. Additional craft positions will overflow into the next formation type.";
			lblFormInfo.Text = text;
		}

		/// <summary>Generates a bitmap from raw inline data.</summary>
		/// <remarks>Pixels marked as zero are transparent.</remarks>
		private Bitmap createBitmapFromRaw(byte[] data, int width, int height)
		{
			Bitmap bmp = new Bitmap(width, height);
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (data[y * width + x] != 0)
						bmp.SetPixel(x, y, Color.White);
					else
						bmp.SetPixel(x, y, Color.Transparent);
				}
			}
			return bmp;
		}

		/// <summary>Retrieves a value from a raw data array. Returns zero if out of bounds.</summary>
		int getValue(short[] dataArray, int index)
		{
			int ret = 0;
			if (dataArray != null && index >= 0 && index < dataArray.Length) ret = dataArray[index];
			return ret;
		}

		/// <summary>Creates an array of positions for each craft in a formation.</summary>
		/// <remarks>XW has more significant differences than other platforms, so it has its own function.</remarks>
		FormPosition[] generatePositionsXwing(int count /*, int spacing*/)
		{
			if (count < 1) count = 1;
			if (count >= 12) count = 12;
			FormPosition[] ret = new FormPosition[count];

			// Size, in world units, of the T/I.  Other craft are different.
			int craftSize = 390;

			// Craft exiting a hangar always use a particular spacing (the smallest of all formations), but the formation remains intact.
			int spacingIndex = cboFormation.SelectedIndex;
			if (chkFormHangar.Checked)
				spacingIndex = 9;

			int horizSpacing = getValue(_formationDataXwingHorizSpacing, spacingIndex);
			int vertSpacing = getValue(_formationDataXwingVertSpacing, spacingIndex);

			if (spacingIndex != 9)
			{
				// XW formations are huge, so we need to scale them down just to fit in the preview window.
				horizSpacing /= 2;
				vertSpacing /= 2;
			}

			for (int i = 0; i < count; i++)
			{
				int craftDataIndex = cboFormation.SelectedIndex * 6 + i;
				int x = getValue(_activeDataArray, (_activeAxisDataCount * 0) + craftDataIndex) * horizSpacing;
				int y = getValue(_activeDataArray, (_activeAxisDataCount * 1) + craftDataIndex) * horizSpacing;
				int z = -getValue(_activeDataArray, (_activeAxisDataCount * 2) + craftDataIndex) * vertSpacing;

				// Scale world units to icon size in pixels
				x = (int)(((float)x / craftSize) * 10.0);
				y = (int)(((float)y / craftSize) * 10.0);
				z = (int)(((float)z / craftSize) * 10.0);

				ret[i] = new FormPosition(x, y, z);
			}
			return ret;
		}

		/// <summary>Creates an array of positions for each craft in a formation.</summary>
		/// <remarks>For all platforms that aren't XW.</remarks>
		FormPosition[] generatePositions(int count, int spacing)
		{
			if (count < 1) count = 1;
			if (count >= 12) count = 12;
			FormPosition[] ret = new FormPosition[count];

			// Size and spacing, in world units, of the T/I.  Other craft are different.
			int craftSize = 390;
			int craftSpacingX = 300;
			int craftSpacingY = 350;
			int craftSpacingZ = 300;

			int spacingMult = spacing + 1;
			for (int i = 0; i < count; i++)
			{
				int formDataIndex = cboFormation.SelectedIndex * 6 + i;
				int x = getValue(_activeDataArray, (_activeAxisDataCount * 0) + formDataIndex) * spacingMult * craftSpacingX;
				int y = getValue(_activeDataArray, (_activeAxisDataCount * 1) + formDataIndex) * spacingMult * craftSpacingY;
				int z = -getValue(_activeDataArray, (_activeAxisDataCount * 2) + formDataIndex) * spacingMult * craftSpacingZ;
				if (spacingMult == 1)
				{
					x += getValue(_activeDataArray, (_activeAxisDataCount * 0) + formDataIndex) * (craftSpacingX / 2);
					y += getValue(_activeDataArray, (_activeAxisDataCount * 1) + formDataIndex) * (craftSpacingY / 2);
					z += -getValue(_activeDataArray,(_activeAxisDataCount * 2) + formDataIndex) * (craftSpacingZ / 4);
				}
				if (_platform != Settings.Platform.TIE)
				{
					int divisor = getValue(_formationDivisor, cboFormation.SelectedIndex);
					if (divisor > 1)
					{
						x /= divisor;
						y /= divisor;
						z /= divisor;
					}
				}

				// Scale world units to icon size in pixels
				x = (int)(((float)x / craftSize) * 10.0);
				y = (int)(((float)y / craftSize) * 10.0);
				z = (int)(((float)z / craftSize) * 10.0);

				ret[i] = new FormPosition(x, y, z);
			}
			return ret;
		}

		/// <summary>Given an array of positions, calculates the required offset to center them inside the viewport panels.</summary>
		/// <remarks>Only considers positions within the estimated viewing space.</remarks>
		FormPosition generateCenteredAverage(FormPosition[] pointArray)
		{
			int x = 0, y = 0, z = 0, count = 0;
			int viewportClip = pctFormation.Height / 2;
			for (int i = 0; i < pointArray.Length; i++)
			{
				if (pointArray[i].X >= -viewportClip || pointArray[i].X <= viewportClip ||
					pointArray[i].Y >= -viewportClip || pointArray[i].Y <= viewportClip ||
					pointArray[i].Z >= -viewportClip || pointArray[i].Z <= viewportClip)
				{
					count++;
					x += pointArray[i].X;
					y += pointArray[i].Y;
					z += pointArray[i].Z;
				}
			}
			if (count > 0)
			{
				x = -x / count;
				y = -y / count;
				z = -z / count;
			}
			return new FormPosition(x, y, z);
		}

		/// <summary>Adds an icon to a list. If a position matches an existing item, the existing string will be appended.</summary>
		/// <remarks>This helps coalesce overlapping items into a single string.</remarks>
		void addIconTag(int x, int y, int number, List<IconTag> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].X == x && list[i].Y == y)
				{
					list[i].Tag += "," + number;
					return;
				}
			}
			list.Add(new IconTag(x, y, number.ToString()));
		}

		/// <summary>Modifies a screen drawing point, clipping relative to its viewport panel. Returns true if clipping occurred.</summary>
		/// <remarks>The canvas is divided into four viewport panels. Point (0, 0) is the center of each respective viewport.</remarks>
		bool clipViewport(ref int x, ref int y)
		{
			int oldX = x, oldY = y;
			int sizeX = pctFormation.Width / 4;
			int sizeY = pctFormation.Height / 4;
			int marginTL = 8;   // Top/left
			int marginBR = 16;  // Bottom/right, larger since the text tags are drawn in that direction
			if (x < -sizeX + marginTL) x = -sizeX + marginTL;
			else if (x > sizeX - marginBR) x = sizeX - marginBR;
			if (y < -sizeY + marginTL) y = -sizeY + marginTL;
			else if (y > sizeY - marginBR) y = sizeY - marginBR;
			return (oldX != x || oldY != y);
		}

		/// <summary>Draws the entire formation preview onto the canvas bitmap.</summary>
		void drawCanvas()
		{
			int count = (int)numFormCount.Value;

			Graphics g;
			g = Graphics.FromImage(_canvas);
			g.Clear(Color.Black);

			FormPosition[] positions;
			if (_platform == Settings.Platform.XWING)
				positions = generatePositionsXwing(count /*, (int)numFormSpacing.Value*/);
			else
				positions = generatePositions(count, (int)numFormSpacing.Value);

			FormPosition average = chkFormFitPanel.Checked ? generateCenteredAverage(positions) : new FormPosition(0, 0, 0);

			// Inlining was a mess, so using these for simplicity.
			int quartWidth = pctFormation.Width / 4;
			int halfWidth = pctFormation.Width / 2;
			int quartHeight = pctFormation.Height / 4;
			int halfHeight = pctFormation.Height / 2;

			const int iconSize = 10;
			const int halfIconSize = iconSize / 2;

			List<IconTag> tagList = new List<IconTag>();
			Pen p = new Pen(Color.FromArgb(55, 55, 55));
			g.DrawLine(p, halfWidth, 0, halfWidth, pctFormation.Height);
			g.DrawLine(p, 0, halfHeight, pctFormation.Width, halfHeight);
			g.DrawString("Top (X - Y)", DefaultFont, Brushes.LightGray, 30, 3); 
			g.DrawString("Side (Y - Z)", DefaultFont, Brushes.LightGray, halfWidth + 30, halfHeight + 3);
			g.DrawString("Behind (X - Z)", DefaultFont, Brushes.LightGray, 30, halfHeight + 3);

			Pen pd = new Pen(Color.FromArgb(45, 45, 45)) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
			int x, y;
			if (!chkFormFitPanel.Checked)
			{
				// Draw cross lines displaying the origin point in each panel.
				// Top view panel (horizontal, then vertical)
				g.DrawLine(pd, 0, quartHeight, halfWidth, quartHeight);
				g.DrawLine(pd, quartWidth, 0, quartWidth, halfHeight);
				// Bottom view panel
				g.DrawLine(pd, 0, quartHeight + halfHeight, halfWidth, quartHeight + halfHeight);
				g.DrawLine(pd, quartWidth, halfHeight, quartWidth, pctFormation.Height);
				// Side view panel
				g.DrawLine(pd, halfWidth, quartHeight + halfHeight, pctFormation.Width, quartHeight + halfHeight);
				g.DrawLine(pd, halfWidth + quartWidth, halfHeight, halfWidth + quartWidth, pctFormation.Height);
			}
			else
			{
				// Draw cross lines where (0, 0) is in the current panel.
				int length = pctFormation.Width / 24;

				x = average.X; y = -average.Y;  // Flip Y
				if (!clipViewport(ref x, ref y))
				{
					x += quartWidth;
					y += quartHeight;
					g.DrawLine(pd, x - length, y, x + length, y); 
					g.DrawLine(pd, x, y - length, x, y + length);
				}

				x = average.Y; y = average.Z;
				if (!clipViewport(ref x, ref y))
				{
					x += quartWidth + halfWidth;
					y += quartHeight + halfHeight;
					g.DrawLine(pd, x - length, y, x + length, y); 
					g.DrawLine(pd, x, y - length, x, y + length);
				}

				x = average.X; y = average.Z;
				if (!clipViewport(ref x, ref y))
				{
					x += quartWidth;
					y += quartHeight + halfHeight;
					g.DrawLine(pd, x - length, y, x + length, y); 
					g.DrawLine(pd, x, y - length, x, y + length);
				}
			}

			// Draw the icons, clipping against the edge of their respective panels.
			// Compiles a list of number tags, which will be drawn after.
			for (int i = 0; i < count; i++)
			{
				bool isClipped;

				// For top view, Y needs to be flipped to draw correctly.
				x = (positions[i].X + average.X);
				y = (-positions[i].Y + -average.Y);
				isClipped = clipViewport(ref x, ref y);
				x += quartWidth - halfIconSize;
				y += quartHeight - halfIconSize;
				if(isClipped)
					g.DrawString("X", DefaultFont, Brushes.Red, x, y);
				else
					g.DrawImage(_topIcon, x, y);
				addIconTag(x, y, i + 1, tagList);

				x = (positions[i].Y + average.Y);
				y = (positions[i].Z + average.Z);
				isClipped = clipViewport(ref x, ref y);
				x += quartWidth + halfWidth - halfIconSize;
				y += quartHeight + halfHeight - halfIconSize;
				if(isClipped)
					g.DrawString("X", DefaultFont, Brushes.Red, x, y);
				else
					g.DrawImage(_sideIcon, x, y);
				addIconTag(x, y, i + 1, tagList);

				x = (positions[i].X + average.X);
				y = (positions[i].Z + average.Z);
				isClipped = clipViewport(ref x, ref y);
				x += quartWidth - halfIconSize;
				y += quartHeight + halfHeight - halfIconSize;
				if (isClipped)
					g.DrawString("X", DefaultFont, Brushes.Red, x, y);
				else
					g.DrawImage(_frontIcon, x, y);
				addIconTag(x, y, i + 1, tagList);
			}

			// Draw the craft numbering tags below each icon.
			foreach (IconTag tag in tagList)
				g.DrawString(tag.Tag, DefaultFont, Brushes.LightGray, tag.X + 10, tag.Y + 10);

			pctFormation.Invalidate();
			p.Dispose();
			pd.Dispose();
			g.Dispose();
		}

		void cboFormation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboFormation.SelectedIndex >= 0)
				drawCanvas();
		}
		void numFormSpacing_ValueChanged(object sender, EventArgs e) { drawCanvas(); }
		void chkFormOrigin_CheckedChanged(object sender, EventArgs e) { drawCanvas(); }
		void chkFormHangar_CheckedChanged(object sender, EventArgs e) { drawCanvas(); }
		void numFormCount_ValueChanged(object sender, EventArgs e) { drawCanvas(); }

		void cmdCancel_Click(object sender, EventArgs e) { Close(); }
		void cmdOK_Click(object sender, EventArgs e) { Close(); }

		void pctFormation_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(_canvas, 0, 0, _canvas.Width, _canvas.Height);
		}
	}
}