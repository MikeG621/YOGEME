/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.1
 */

/* CHANGELOG
 * v1.0, 110921
 * - Release
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Idmr.Common;

namespace Idmr.Yogeme
{
	/// <summary>graphical interface for craft waypoints</summary>
	public partial class frmMap : Form
	{
		int j = 40;
		int w, h, mapX, mapY, mapZ;
		enum Orientation { XY, XZ, YZ };
		Orientation _displayMode = Orientation.XY;
		Bitmap _map;
		MapData[] _mapData;
		int[] _dragIcon = new int[2];	// [0] = fg, [1] = wp
		bool _loading = false;
		CheckBox[] chkWP = new CheckBox[22];
		Settings.Platform _platform;

		/// <param name="fg">TFlights array</param>
		public frmMap(Platform.Tie.FlightGroupCollection fg)
		{
			_platform = Settings.Platform.TIE;
			Import(fg);
			InitializeComponent();
			try { imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_TIE.bmp")); }
			catch(Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
			Startup();
		}

		/// <param name="fg">XFlights array</param>
		public frmMap(Platform.Xvt.FlightGroupCollection fg)
		{
			_platform = Settings.Platform.XvT;
			Import(fg);
			InitializeComponent();
			try { imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_XvT.bmp")); }
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
			Startup();
		}

		/// <param name="fg">WFlights array</param>
		public frmMap(Platform.Xwa.FlightGroupCollection fg)
		{
			_platform = Settings.Platform.XWA;
			Import(fg);
			InitializeComponent();
			try { imgCraft.Images.AddStrip(Image.FromFile(Application.StartupPath + "\\images\\craft_XWA.bmp")); }
			catch (Exception x)
			{
				MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
			Startup();
		}

		/// <summary>Intialization routine, loads settings and config per platform</summary>
		void Startup()
		{
			CheckArray();
			w = pctMap.Width;
			h = pctMap.Height;
			mapX = w/2;
			mapY = h/2;
			mapZ = h/2;
			_dragIcon[0] = -1;
			Settings config = new Settings();
			_loading = true;
			chkTags.Checked = Convert.ToBoolean(config.MapOptions & Settings.MapOpts.FGTags);
			chkTrace.Checked = Convert.ToBoolean(config.MapOptions & Settings.MapOpts.Traces);
			int t = config.Waypoints;
			if (_platform==Settings.Platform.TIE)
			{
				for (int i=0;i<15;i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
				for (int i=15;i<22;i++) chkWP[i].Enabled = false;
			}
			else if (_platform==Settings.Platform.XvT) for (int i=0;i<22;i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
			else if (_platform==Settings.Platform.XWA)
			{
				for (int i=0;i<12;i++) chkWP[i].Checked = Convert.ToBoolean(t & (1 << i));
				chkWP[3].Text = "HYP";
				for (int i=12;i<22;i++) chkWP[i].Enabled = false;
				lblRegion.Visible = true;
				numRegion.Visible = true;
				lblOrder.Visible = true;
				numOrder.Visible = true;
			}
			this.MouseWheel += new MouseEventHandler(frmMap_MouseWheel);
			_loading = false;
		}

		void CheckArray()
		{
			chkWP[0] = chkSP1;
			chkWP[1] = chkSP2;
			chkWP[2] = chkSP3;
			chkWP[3] = chkSP4;
			chkWP[4] = chkWP1;
			chkWP[5] = chkWP2;
			chkWP[6] = chkWP3;
			chkWP[7] = chkWP4;
			chkWP[8] = chkWP5;
			chkWP[9] = chkWP6;
			chkWP[10] = chkWP7;
			chkWP[11] = chkWP8;
			chkWP[12] = chkRDV;
			chkWP[13] = chkHYP;
			chkWP[14] = chkBRF;
			chkWP[15] = chkBRF2;
			chkWP[16] = chkBRF3;
			chkWP[17] = chkBRF4;
			chkWP[18] = chkBRF5;
			chkWP[19] = chkBRF6;
			chkWP[20] = chkBRF7;
			chkWP[21] = chkBRF8;
			for (int i=0;i<22;i++)
			{
				chkWP[i].CheckedChanged += new EventHandler(chkWPArr_CheckedChanged);
				chkWP[i].Tag = i;
			}
		}

		/// <summary> The down-and-dirty function that handles map display </summary>
		/// <param name="Persistant">When true draws to memory, false is temp drawing to picture</param>
		public void MapPaint(bool persistant)
		{
			if (_loading) return;
			#region orientation setup
			int X = 0, Y = 0;
			switch (_displayMode)
			{
				case Orientation.XY:
					X = mapX;
					Y = mapY;
					break;
				case Orientation.XZ:
					X = mapX;
					Y = mapZ;
					break;
				case Orientation.YZ:
					X = mapY;
					Y = mapZ;
					break;
			}
			#endregion
			#region brush, pen and graphics setup
			// Create a new pen that we shall use for drawing the lines
			Pen pn = new Pen(Color.DarkRed);		
			SolidBrush sb = new SolidBrush(Color.Black);
			SolidBrush sbg = new SolidBrush(Color.DimGray);	// for FG tags
			Pen pnTrace = new Pen(Color.Gray);		// for WP traces
			Graphics g3;
			if (persistant) 
			{
				g3 = Graphics.FromImage(_map);		//graphics obj, load from the memory bitmap
				g3.Clear(SystemColors.Control);		//clear it
			}
			else 
			{
				g3 = pctMap.CreateGraphics();		//paint directly to pict
			}
			#endregion
			#region BG and grid
			g3.FillRectangle(sb, 0, 0, w, h);		//background
			for(int i = 0; i<200; i++)
			{
				g3.DrawLine(pn, 0, j*i + Y, w, j*i + Y);	//min lines, every klick
				g3.DrawLine(pn, 0, Y - j*i, w, Y - j*i);
				g3.DrawLine(pn, j*i + X, 0, j*i + X, h);
				g3.DrawLine(pn, X - j*i, 0,X - j*i, h);
			}
			pn.Width = 3;
			for(int i = 0; i<40; i++)
			{
				g3.DrawLine(pn, 0, j*i*5 + Y, w, j*i*5 + Y);	//maj lines, every 5 klicks
				g3.DrawLine(pn, 0, Y - j*i*5, w, Y - j*i*5);
				g3.DrawLine(pn, j*i*5 + X, 0, j*i*5 + X, h);
				g3.DrawLine(pn, X - j*i*5, 0, X - j*i*5, h);
			}
			pn.Color = Color.Red;
			pn.Width = 1;
			g3.DrawLine(pn, 0, Y, w, Y);	// origin lines
			g3.DrawLine(pn, X, 0, X, h);
			#endregion
			Bitmap bmptemp;
			byte[] bAdd = new byte[3];		// [0] = R, [1] = G, [2] = B
			for (int i = 0; i<_mapData.Length; i++)
			{
				#region IFF colors
				switch(_mapData[i].IFF)
				{
					case 0:
						pn.Color = Color.LimeGreen;		// FF32CD32
						break;
					case 1:
						pn.Color = Color.Crimson;		// FFDC143C
						break;
					case 2:
						pn.Color = Color.RoyalBlue;		// FF4169E1
						break;
					case 3:
						if (_platform == Settings.Platform.TIE) pn.Color = Color.DarkOrchid;		// FF9932CC
						else pn.Color = Color.Yellow;	// FFFFFF00
						break;
					case 4:
						pn.Color = Color.Red;			// FFFF0000
						break;
					case 5:
						if (_platform == Settings.Platform.TIE) pn.Color = Color.Fuchsia;			// FFFF00FF
						else pn.Color = Color.DarkOrchid;	// FF9932CC
						break;
				}
				bAdd[0] = pn.Color.R;
				bAdd[1] = pn.Color.G;
				bAdd[2] = pn.Color.B;
				#endregion
				bmptemp = new Bitmap(imgCraft.Images[_mapData[i].Craft]);
				bmptemp = Mask(bmptemp, bAdd);
				// work through each WP and determine if it needs to be displayed, then place it on the map
				// draw tags if required
				// if previous sequential WP is checked and trace is required, draw trace line according to WP type
				switch (_displayMode)
				{
					case Orientation.XY:
						#region X/Y
						for (int k=0;k<4;k++)
						{
							if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1 && (_platform == Settings.Platform.XWA ? _mapData[i].Waypoints[k,4] == (short)(numRegion.Value-1) : true))
							{
								g3.DrawImageUnscaled(bmptemp, j*_mapData[i].Waypoints[k, 0]/160 + X-8, -j*_mapData[i].Waypoints[k, 1]/160 + Y-8);
								if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 0]/160 + X+8, -j*_mapData[i].Waypoints[k, 1]/160 + Y+8);
							}
						}
						if (_platform == Settings.Platform.XWA)
						{
							int o = (int)((numRegion.Value-1)*4+(numOrder.Value-1))*8;
							for (int k=4;k<12;k++)
							{
								if (chkWP[k].Checked && _mapData[i].Waypoints[o+k, 3] == 1)
								{
									g3.DrawEllipse(pn, j*_mapData[i].Waypoints[o+k, 0]/160 + X-1, -j*_mapData[i].Waypoints[o+k, 1]/160 + Y-1, 3, 3);
									if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[o+k, 0]/160 + X+4, -j*_mapData[i].Waypoints[o+k, 1]/160 + Y+4);
									if (chkTrace.Checked && k==4 && _mapData[i].Waypoints[0, 4] == (numRegion.Value-1) && chkWP[0].Checked) g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[0, 0]/160 + X, -j*_mapData[i].Waypoints[0, 1]/160 + Y, j*_mapData[i].Waypoints[o+k, 0]/160 + X, -j*_mapData[i].Waypoints[o+k, 1]/160 + Y);
									else if (chkTrace.Checked && chkWP[k-1].Checked) g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[o+k-1, 0]/160 + X, -j*_mapData[i].Waypoints[o+k-1, 1]/160 + Y, j*_mapData[i].Waypoints[o+k, 0]/160 + X, -j*_mapData[i].Waypoints[o+k, 1]/160 + Y);
								}
							}
							continue;
						}
						else
						{
							for (int k=4;k<12;k++)
							{
								if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1)
								{
									g3.DrawEllipse(pn, j*_mapData[i].Waypoints[k, 0]/160 + X-1, -j*_mapData[i].Waypoints[k, 1]/160 + Y-1, 3, 3);
									if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 0]/160 + X+4, -j*_mapData[i].Waypoints[k, 1]/160 + Y+4);
									if (chkWP[(k==4 ? 0 : (k-1))].Checked && chkTrace.Checked) g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[(k==4 ? 0 : (k-1)), 0]/160 + X, -j*_mapData[i].Waypoints[(k==4 ? 0 : (k-1)), 1]/160 + Y, j*_mapData[i].Waypoints[k, 0]/160 + X, -j*_mapData[i].Waypoints[k, 1]/160 + Y);
								}
							}
						}
						// remaining are not valid for XWA
						if (chkWP[12].Checked && _mapData[i].Waypoints[12,3] == 1) 
						{
							g3.DrawEllipse(pn, j*_mapData[i].Waypoints[12,0]/160 + X-1, -j*_mapData[i].Waypoints[12,1]/160 + Y-1, 3, 3);
							if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[12].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[12, 0]/160 + X+4, -j*_mapData[i].Waypoints[12, 1]/160 + Y+4);
						}
						if (chkWP[13].Checked && _mapData[i].Waypoints[13,3] == 1) 
						{
							g3.DrawEllipse(pn, j*_mapData[i].Waypoints[13,0]/160 + X-1, -j*_mapData[i].Waypoints[13,1]/160 + Y-1, 3, 3);
							if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[13].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[13, 0]/160 + X+4, -j*_mapData[i].Waypoints[13, 1]/160 + Y+4);
							if (chkTrace.Checked) 
							{
								// in this case, make sure last visible WP is the last enabled before tracing to HYP
								pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
								for (int k=4;k<12;k++)
								{
									if (k != 11)
									{
										if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1 && _mapData[i].Waypoints[k+1, 3] == 0)
										{
											g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[k, 0]/160 + X, -j*_mapData[i].Waypoints[k, 1]/160 + Y, j*_mapData[i].Waypoints[13, 0]/160 + X, -j*_mapData[i].Waypoints[13, 1]/160 + Y);
											break;
										}
									}
									else if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1) g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[11, 0]/160 + X, -j*_mapData[i].Waypoints[11, 1]/160 + Y, j*_mapData[i].Waypoints[13, 0]/160 + X, -j*_mapData[i].Waypoints[13, 1]/160 + Y); ;
								}
								pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
							}
						}
						for (int k=14;k<22;k++)
						{
							if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1)
							{
								g3.DrawImageUnscaled(bmptemp, j*_mapData[i].Waypoints[k, 0]/160 + X-8, -j*_mapData[i].Waypoints[k, 1]/160 + Y-8);
								if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 0]/160 + X+8, -j*_mapData[i].Waypoints[k, 1]/160 + Y+8);
							}
						}
						#endregion
						break;
					case Orientation.XZ:
						#region X/Z
						for (int k=0;k<4;k++)
						{
							if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1 && (_platform == Settings.Platform.XWA ? _mapData[i].Waypoints[k, 4] == (short)(numRegion.Value-1) : true))
							{
								g3.DrawImageUnscaled(bmptemp, j*_mapData[i].Waypoints[k, 0]/160 + X-8, -j*_mapData[i].Waypoints[k, 2]/160 + Y-8);
								if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 0]/160 + X+8, -j*_mapData[i].Waypoints[k, 2]/160 + Y+8);
							}
						}
						for (int k=4;k<12;k++)
						{
							if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1)
							{
								g3.DrawEllipse(pn, j*_mapData[i].Waypoints[k, 0]/160 + X-1, -j*_mapData[i].Waypoints[k, 2]/160 + Y-1, 3, 3);
								if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 0]/160 + X+4, -j*_mapData[i].Waypoints[k, 2]/160 + Y+4);
								if (chkWP[(k==4 ? 0 : (k-1))].Checked && chkTrace.Checked) g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[(k==4 ? 0 : (k-1)), 0]/160 + X, -j*_mapData[i].Waypoints[(k==4 ? 0 : (k-1)), 2]/160 + Y, j*_mapData[i].Waypoints[k, 0]/160 + X, -j*_mapData[i].Waypoints[k, 2]/160 + Y);
							}
						}
						if (chkWP[12].Checked && _mapData[i].Waypoints[12, 3] == 1)
						{
							g3.DrawEllipse(pn, j*_mapData[i].Waypoints[12, 0]/160 + X-1, -j*_mapData[i].Waypoints[12, 2]/160 + Y-1, 3, 3);
							if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[12].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[12, 0]/160 + X+4, -j*_mapData[i].Waypoints[12, 2]/160 + Y+4);
						}
						if (chkHYP.Checked && _mapData[i].Waypoints[13,3] == 1) 
						{
							g3.DrawEllipse(pn, j*_mapData[i].Waypoints[13,0]/160 + X-1, -j*_mapData[i].Waypoints[13,2]/160 + Y-1, 3, 3);
							if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " HYP", frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[13,0]/160 + X+4, -j*_mapData[i].Waypoints[13,2]/160 + Y+4);
							if (chkTrace.Checked) 
							{
								pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
								for (int k=4;k<12;k++)
								{
									if (k != 11)
									{
										if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1 && _mapData[i].Waypoints[k+1, 3] == 0)
										{
											g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[k, 0]/160 + X, -j*_mapData[i].Waypoints[k, 2]/160 + Y, j*_mapData[i].Waypoints[13, 0]/160 + X, -j*_mapData[i].Waypoints[13, 2]/160 + Y);
											break;
										}
									}
									else if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1) g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[11, 0]/160 + X, -j*_mapData[i].Waypoints[11, 2]/160 + Y, j*_mapData[i].Waypoints[13, 0]/160 + X, -j*_mapData[i].Waypoints[13, 2]/160 + Y); ;
								} pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
							}
						}
						for (int k=14;k<22;k++)
						{
							if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1)
							{
								g3.DrawImageUnscaled(bmptemp, j*_mapData[i].Waypoints[k, 0]/160 + X-8, -j*_mapData[i].Waypoints[k, 2]/160 + Y-8);
								if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 0]/160 + X+8, -j*_mapData[i].Waypoints[k, 2]/160 + Y+8);
							}
						}
						#endregion
						break;
					case Orientation.YZ:
						#region Y/Z
						for (int k=0;k<4;k++)
						{
							if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1 && (_platform == Settings.Platform.XWA ? _mapData[i].Waypoints[k, 4] == (short)(numRegion.Value-1) : true))
							{
								g3.DrawImageUnscaled(bmptemp, j*_mapData[i].Waypoints[k, 1]/160 + X-8, -j*_mapData[i].Waypoints[k, 2]/160 + Y-8);
								if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 1]/160 + X+8, -j*_mapData[i].Waypoints[k, 2]/160 + Y+8);
							}
						}
						for (int k=4;k<12;k++)
						{
							if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1)
							{
								g3.DrawEllipse(pn, j*_mapData[i].Waypoints[k, 1]/160 + X-1, -j*_mapData[i].Waypoints[k, 2]/160 + Y-1, 3, 3);
								if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 1]/160 + X+4, -j*_mapData[i].Waypoints[k, 2]/160 + Y+4);
								if (chkWP[(k==4 ? 0 : (k-1))].Checked && chkTrace.Checked) g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[(k==4 ? 0 : (k-1)), 1]/160 + X, -j*_mapData[i].Waypoints[(k==4 ? 0 : (k-1)), 2]/160 + Y, j*_mapData[i].Waypoints[k, 1]/160 + X, -j*_mapData[i].Waypoints[k, 2]/160 + Y);
							}
						}
						if (chkWP[12].Checked && _mapData[i].Waypoints[12, 3] == 1)
						{
							g3.DrawEllipse(pn, j*_mapData[i].Waypoints[12, 1]/160 + X-1, -j*_mapData[i].Waypoints[12, 2]/160 + Y-1, 3, 3);
							if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[12].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[12, 1]/160 + X+4, -j*_mapData[i].Waypoints[12, 2]/160 + Y+4);
						}
						if (chkHYP.Checked && _mapData[i].Waypoints[13,3] == 1) 
						{
							g3.DrawEllipse(pn, j*_mapData[i].Waypoints[13,1]/160 + X-1, -j*_mapData[i].Waypoints[13,2]/160 + Y-1, 3, 3);
							if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " HYP", frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[13,1]/160 + X+4, -j*_mapData[i].Waypoints[13,2]/160 + Y+4);
							if (chkTrace.Checked) 
							{
								pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
								for (int k=4;k<12;k++)
								{
									if (k != 11)
									{
										if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1 && _mapData[i].Waypoints[k+1, 3] == 0)
										{
											g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[k, 1]/160 + X, -j*_mapData[i].Waypoints[k, 2]/160 + Y, j*_mapData[i].Waypoints[13, 1]/160 + X, -j*_mapData[i].Waypoints[13, 2]/160 + Y);
											break;
										}
									}
									else if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1) g3.DrawLine(pnTrace, j*_mapData[i].Waypoints[11, 1]/160 + X, -j*_mapData[i].Waypoints[11, 2]/160 + Y, j*_mapData[i].Waypoints[13, 1]/160 + X, -j*_mapData[i].Waypoints[13, 2]/160 + Y); ;
								} pnTrace.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
							}
						}
						for (int k=14;k<22;k++)
						{
							if (chkWP[k].Checked && _mapData[i].Waypoints[k, 3] == 1)
							{
								g3.DrawImageUnscaled(bmptemp, j*_mapData[i].Waypoints[k, 1]/160 + X-8, -j*_mapData[i].Waypoints[k, 2]/160 + Y-8);
								if (chkTags.Checked) g3.DrawString(_mapData[i].Name + " " + chkWP[k].Text, frmMap.DefaultFont, sbg, j*_mapData[i].Waypoints[k, 1]/160 + X+8, -j*_mapData[i].Waypoints[k, 2]/160 + Y+8);
							}
						}
						#endregion
						break;
				}
			}
			if (persistant) pctMap.Invalidate();		// since it's drawing to memory, this refreshes the pct.  Removes the flicker when zooming
			g3.Dispose();
		}

		/// <summary>Loads FG data into the MapData class the form uses</summary>
		/// <param name="fg">TFlights array</param>
		public void Import(Platform.Tie.FlightGroupCollection fg)
		{
			int numCraft = fg.Count;
			_mapData = new MapData[numCraft];
			for (int i=0;i<numCraft;i++)
			{
				_mapData[i] = new MapData(_platform);
				_mapData[i].Craft = fg[i].CraftType;
				for (int j = 0; j < 15; j++)
					for (int k = 0; k < 4; k++) _mapData[i].Waypoints[j, k] = fg[i].Waypoints[j][k]; 
				_mapData[i].IFF = fg[i].IFF;
				_mapData[i].Name = fg[i].Name;
			}
		}

		/// <summary>Loads FG data into the MapData class the form uses</summary>
		/// <param name="fg">XFlights array</param>
		public void Import(Platform.Xvt.FlightGroupCollection fg)
		{
			int numCraft = fg.Count;
			_mapData = new MapData[numCraft];
			for (int i=0;i<numCraft;i++)
			{
				_mapData[i] = new MapData(_platform);
				_mapData[i].Craft = fg[i].CraftType;
				for (int j = 0; j < 15; j++)
					for (int k = 0; k < 4; k++) _mapData[i].Waypoints[j, k] = fg[i].Waypoints[j][k]; 
				_mapData[i].IFF = fg[i].IFF;
				_mapData[i].Name = fg[i].Name;
			}
		}

		/// <summary>Loads FG data into the MapData class the form uses</summary>
		/// <param name="fg">WFlights array</param>
		public void Import(Platform.Xwa.FlightGroupCollection fg)
		{
			int numCraft = fg.Count;
			_mapData = new MapData[numCraft];
			for (int i=0;i<numCraft;i++)
			{
				_mapData[i] = new MapData(_platform);
				_mapData[i].Craft = fg[i].CraftType;
				for (int j = 0; j < 4; j++)
					for (int k = 0; k < 5; k++) _mapData[i].Waypoints[j, k] = fg[i].Waypoints[j][k]; 
				for (int j = 4; j < 132; j++)
				{
					int region = (j - 4) / 32;
					int order = ((j - 4) % 32) / 8;
					int waypoint = (j - 4) % 8;
					for (int k = 0; k < 5; k++) _mapData[i].Waypoints[j, k] = fg[i].Orders[region, order].Waypoints[waypoint][k]; 
				}
				_mapData[i].IFF = fg[i].IFF;
				_mapData[i].Name = fg[i].Name;
			}
		}

		/// <summary>Change the zoom of the map and reset local x/y/z coords as neccessary</summary>
		void hscZoom_ValueChanged(object sender, EventArgs e)
		{
			// emulate a right-click to the center of the map and store the cords in klicks
			double msX = 0, msY = 0;
			switch(_displayMode)
			{
				case Orientation.XY:
					msX = (w/2 - mapX) / Convert.ToDouble(j);
					msY = (mapY - h/2) / Convert.ToDouble(j);
					break;
				case Orientation.XZ:
					msX = (w/2 - mapX) / Convert.ToDouble(j);
					msY = (mapZ - h/2) / Convert.ToDouble(j);
					break;
				case Orientation.YZ:
					msX = (w/2 - mapY) / Convert.ToDouble(j);
					msY = (mapZ - h/2) / Convert.ToDouble(j);
					break;
			}
			j = hscZoom.Value;
			// using the coords determine the correct map location with the new zoom
			switch(_displayMode)
			{
				case Orientation.XY:
					mapX = Convert.ToInt32(w/2 - msX * Convert.ToDouble(j));
					mapY = Convert.ToInt32(h/2 + msY * Convert.ToDouble(j));
					break;
				case Orientation.XZ:
					mapX = Convert.ToInt32(w/2 - msX * Convert.ToDouble(j));
					mapZ = Convert.ToInt32(h/2 + msY * Convert.ToDouble(j));
					break;
				case Orientation.YZ:
					mapY = Convert.ToInt32(w/2 - msX * Convert.ToDouble(j));
					mapZ = Convert.ToInt32(h/2 + msY * Convert.ToDouble(j));
					break;
			}
			if (mapX/j > 150) mapX = 150*j;
			if ((mapX-w)/j < -150) mapX = -150*j + w;
			if (mapY/j > 150) mapY = 150*j;
			if ((mapY-h)/j < -150) mapY = -150*j + h;
			if (mapZ/j > 150) mapZ = 150*j;
			if ((mapZ-h)/j < -150) mapZ = -150*j + h;
			MapPaint(true);
			lblZoom.Text = "Zoom: " + j.ToString();
		}

		/// <summary>Rotate map to Top view</summary>
		void optXY_CheckedChanged(object sender, EventArgs e)
		{
			if (optXY.Checked)
			{
				_displayMode = Orientation.XY;
				lblCoor1.Text = "X:";
				lblCoor2.Text = "Y:";
				MapPaint(false);
			}
		}

		/// <summary>Rotate map to Front view</summary>
		void optXZ_CheckedChanged(object sender, EventArgs e)
		{
			if (optXZ.Checked)
			{
				_displayMode = Orientation.XZ;
				lblCoor1.Text = "X:";
				lblCoor2.Text = "Z:";
				MapPaint(false);
			}
		}

		/// <summary>Rotate map to Side view </summary>
		void optYZ_CheckedChanged(object sender, EventArgs e)
		{
			if (optYZ.Checked)
			{
				mapY = w/2 - mapY + h/2;
				_displayMode = Orientation.YZ;
				lblCoor1.Text = "Y:";
				lblCoor2.Text = "Z:";
				MapPaint(false);
			}
			else mapY = w/2 + h/2 - mapY;
		}

		#region pctMap
		void pctMap_DoubleClick(object sender, EventArgs e)
		{
			// zoom in
			if (hscZoom.Value == 500) return;
			mapX += (mapX - w/2);
			mapY += (mapY - h/2);
			mapZ += (mapZ - w/2);
			if (mapX/j > 150) mapX = 150*j;
			if ((mapX-w)/j < -150) mapX = -150*j + w;
			if (mapY/j > 150) mapY = 150*j;
			if ((mapY-h)/j < -150) mapY = -150*j + h;
			if (mapZ/j > 150) mapZ = 150*j;
			if ((mapZ-h)/j < -150) mapZ = -150*j + h;
			hscZoom.Value = (j < 250 ? j * 2 : 500);
		}
		void pctMap_MouseDown(object sender, MouseEventArgs e)
		{
			// move map, center on mouse
			if (e.Button.ToString() == "Right")
			{
				switch (_displayMode)
				{
					#region Mode check
					case Orientation.XY:
						mapX += w / 2 - e.X;
						mapY += h / 2 - e.Y;
						if (mapX / j > 150) mapX = 150 * j;
						if ((mapX - w) / j < -150) mapX = -150 * j + w;
						if (mapY / j > 150) mapY = 150 * j;
						if ((mapY - h) / j < -150) mapY = -150 * j + h;
						break;
					case Orientation.XZ:
						mapX += w / 2 - e.X;
						mapZ += h / 2 - e.Y;
						if (mapX / j > 150) mapX = 150 * j;
						if ((mapX - w) / j < -150) mapX = -150 * j + w;
						if (mapZ / j > 150) mapZ = 150 * j;
						if ((mapZ - h) / j < -150) mapZ = -150 * j + h;
						break;
					case Orientation.YZ:
						mapY += w / 2 - e.X;
						mapZ += h / 2 - e.Y;
						if (mapY / j > 150) mapY = 150 * j;
						if ((mapY - w) / j < -150) mapY = -150 * j + w;
						if (mapZ / j > 150) mapZ = 150 * j;
						if ((mapZ - h) / j < -150) mapZ = -150 * j + h;
						break;
					#endregion
				}
				MapPaint(false);
			}
			else if (e.Button.ToString() == "Left")
			{
				// Okay, for every flightgroup, check every waypoint
				// if it's enabled, check to see if the center is about where the mouse is
				// store FG and WP indexes
				for (int fg = 0; fg < _mapData.Length; fg++)
				{
					for (int wp = 0; wp < (_platform == Settings.Platform.TIE ? 15 : 22); wp++)
					{
						if (!chkWP[wp].Checked) continue;
						if ((_displayMode == Orientation.XY && IsApprox(j * _mapData[fg].Waypoints[wp, 0] / 160 + mapX, e.X) && IsApprox(-j * _mapData[fg].Waypoints[wp, 1] / 160 + mapY, e.Y)) || (_displayMode == Orientation.XZ && IsApprox(j * _mapData[fg].Waypoints[wp, 0] / 160 + mapX, e.X) && IsApprox(-j * _mapData[fg].Waypoints[wp, 2] / 160 + mapZ, e.Y)) || (_displayMode == Orientation.YZ && IsApprox(j * _mapData[fg].Waypoints[wp, 1] / 160 + mapY, e.X) && IsApprox(-j * _mapData[fg].Waypoints[wp, 2] / 160 + mapZ, e.Y)))
						{
							_dragIcon[0] = fg;
							_dragIcon[1] = wp;
							break;
						}
					}
				}
			}
			else if (e.Button.ToString() == "Middle")
			{
				mapX = w/2;
				mapY = h/2;
				mapZ = h/2;
				hscZoom.Value = 40;
				MapPaint(false);
			}
		}
		void pctMap_MouseEnter(object sender, EventArgs e) { pctMap.Focus(); }
		void pctMap_MouseMove(object sender, MouseEventArgs e)
		{
			// gets the current mouse location in klicks
			double msX, msY;
			switch (_displayMode)
			{
				case Orientation.XY:
					msX = (e.X - mapX) / Convert.ToDouble(j);
					msY = (mapY - e.Y) / Convert.ToDouble(j);
					lblCoor1.Text = "X: " + Math.Round(msX,2).ToString();
					lblCoor2.Text = "Y: " + Math.Round(msY,2).ToString();
					break;
				case Orientation.XZ:
					msX = (e.X - mapX) / Convert.ToDouble(j);
					msY = (mapZ - e.Y) / Convert.ToDouble(j);
					lblCoor1.Text = "X: " + Math.Round(msX,2).ToString();
					lblCoor2.Text = "Z: " + Math.Round(msY,2).ToString();
					break;
				case Orientation.YZ:
					msX = (e.X - mapY) / Convert.ToDouble(j);
					msY = (mapZ - e.Y) / Convert.ToDouble(j);
					lblCoor1.Text = "Y: " + Math.Round(msX,2).ToString();
					lblCoor2.Text = "Z: " + Math.Round(msY,2).ToString();
					break;
			}
		}
		void pctMap_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button.ToString() == "Left")
			{
				if (_dragIcon[0] != -1)
				{
					// WP was dragged, reassign to the new location and repaint
					switch(_displayMode)
					{
						case Orientation.XY:
							_mapData[_dragIcon[0]].Waypoints[_dragIcon[1],0] = (short)((e.X - mapX) / Convert.ToDouble(j) * 160);
							_mapData[_dragIcon[0]].Waypoints[_dragIcon[1], 1] = (short)((mapY - e.Y) / Convert.ToDouble(j) * 160);
							break;
						case Orientation.XZ:
							_mapData[_dragIcon[0]].Waypoints[_dragIcon[1], 0] = (short)((e.X - mapX) / Convert.ToDouble(j) * 160);
							_mapData[_dragIcon[0]].Waypoints[_dragIcon[1], 2] = (short)((mapZ - e.Y) / Convert.ToDouble(j) * 160);
							break;
						case Orientation.YZ:
							_mapData[_dragIcon[0]].Waypoints[_dragIcon[1], 1] = (short)((e.X - mapY) / Convert.ToDouble(j) * 160);
							_mapData[_dragIcon[0]].Waypoints[_dragIcon[1], 2] = (short)((mapZ - e.Y) / Convert.ToDouble(j) * 160);
							break;
					}
					MapPaint(true);
				}
				_dragIcon[0] = -1;
			}
		}
		void pctMap_Paint(object sender, PaintEventArgs e)
		{
			Graphics objGraphics;
			//You can't modify e.Graphics directly.
			objGraphics = e.Graphics;
			// Draw the contents of the bitmap on the form.
			objGraphics.DrawImage(_map, 0, 0, _map.Width, _map.Height);
		}

		/// <summary>Used to determine if mouse click is near a craft waypoint</summary>
		/// <returns>True if num1==(num2 ± 6)</returns>
		bool IsApprox(int num1, double num2)
		{
			// +/- 6 is a good enough size
			if (num1 <= (num2+6) && num1 >= (num2-6)) return true;
			else return false;
		}
		#endregion
		#region frmMap
		void frmMap_Activated(object sender, EventArgs e) { MapPaint(true); }
		void frmMap_Closed(object sender, EventArgs e) { _map.Dispose(); }
		void frmMap_Load(object sender, EventArgs e)
		{
			_map = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			MapPaint(true);
		}
		void frmMap_MouseWheel(object sender, MouseEventArgs e)
		{
			if (hscZoom.Value < 25 && e.Delta < 0) hscZoom.Value = 5;
			else if (hscZoom.Value > 480 && e.Delta > 0) hscZoom.Value = 500;
			else hscZoom.Value += 20 * Math.Sign(e.Delta);
		}
		#endregion

		/// <summary>Take the original image from the craft image strip and adds the RGB values from the craft IFF</summary>
		/// <param name="craftImage">The greyscale craft image</param>
		/// <param name="iff">An array containing the RGB values as per the craft IFF</param>
		/// <returns>Colorized craft image according to IFF</returns>
		Bitmap Mask(Bitmap craftImage, byte[] iff)
		{
			// craftImage comes in as 32bppRGB, but we force the image into 24bppRGB with LockBits
			BitmapData bmData = GraphicsFunctions.GetBitmapData(craftImage, PixelFormat.Format24bppRgb);
			byte[] pix = new byte[bmData.Stride*bmData.Height];
			GraphicsFunctions.CopyImageToBytes(bmData, pix);
			for(int y = 0;y < craftImage.Height; y++)
			{
				for(int x = 0, pos = bmData.Stride*y;x < craftImage.Width; x++)
				{
					// stupid thing returns BGR instead of RGB
					pix[pos+x*3] = (byte)(pix[pos+x*3] * iff[2] / 255);		// get intensity, apply to IFF mask
					pix[pos+x*3+1] = (byte)(pix[pos+x*3+1] * iff[1] / 255);
					pix[pos+x*3+2] = (byte)(pix[pos+x*3+2] * iff[0] / 255);
				}
			}
			GraphicsFunctions.CopyBytesToImage(pix, bmData);
			craftImage.UnlockBits(bmData);
			craftImage.MakeTransparent(Color.Black);
			return craftImage;
		}

		#region Checkboxes
		void chkTags_CheckedChanged(object sender, EventArgs e) { if (!_loading) MapPaint(true); }
		void chkTrace_CheckedChanged(object sender, EventArgs e) { if (!_loading) MapPaint(true); }
		void chkWPArr_CheckedChanged(object sender, EventArgs e)
		{
			if (_loading) return;
			if ((CheckBox)sender == chkWP[14] && chkWP[14].Checked) for (int i=0;i<14;i++) chkWP[i].Checked = false;
			MapPaint(true);
		}
		#endregion

		void numOrder_ValueChanged(object sender, EventArgs e) { if (!_loading) MapPaint(true); }
		void numRegion_ValueChanged(object sender, EventArgs e) { if (!_loading) MapPaint(true); }
	}

	public struct MapData
	{
		public MapData(Settings.Platform platform)
		{
			Craft = 0;
			IFF = 0;
			Name = "";
			int x=0, y=4;
			switch (platform)
			{
				case Settings.Platform.TIE:
					x = 15;
					break;
				case Settings.Platform.XvT:
					x = 22;
					break;
				case Settings.Platform.XWA:
					x = 132;
					y = 5;
					break;
			}
			Waypoints = new short[x, y];
		}

		public int Craft;
		public short[,] Waypoints;
		public byte IFF;
		public string Name;
	}
}