/*
 * YOGEME.exe, All-in-one Mission Editor for the X-wing series, TIE through XWA
 * Copyright (C) 2007-2012 Michael Gaisser (mjgaisser@gmail.com)
 * Licensed under the GPL v3.0 or later
 * 
 * VERSION: 1.2.2
 */

/* CHANGELOG
 * v1.1.1, 120814
 * - removed/renamed some methods, condensed
 * - class renamed
 * v1.1, 120715
 * - Created
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Idmr.LfdReader;
using Idmr.Platform.Tie;

namespace Idmr.Yogeme
{
	/// <summary>Form for Briefing and Debriefing officer question preview</summary>
	public partial class OfficerPreviewForm : Form
	{
		string[] _answerLines;
		int _page = 1;
		int _selectedIndex = 0;
		Questions _questions;
		RadioButton[] _opts = new RadioButton[4];
		Color _normalText = Color.FromArgb(84, 84, 252);
		Color _highlight = Color.FromArgb(0, 168, 0);
		LfdFile _empire;
		LfdFile _talk;
		Bitmap _preview = new Bitmap(320, 200);
		byte[] _indexes = new byte[5];
		string _fontID = "FONTfont8";

		public OfficerPreviewForm(Questions questions)
		{
			try
			{
				string path = new Settings().TiePath + "\\RESOURCE\\";
				_empire = new LfdFile(path + "EMPIRE.LFD");
				_talk = new LfdFile(path + "TALK.LFD");
			}
			catch (System.IO.FileNotFoundException)
			{
				MessageBox.Show("TIE resource files not found, preview unavailable", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
			_questions = questions;
			InitializeComponent();
			#region Pltt assignments
			Pltt standard = (Pltt)_empire.Resources["PLTTstandard"];
			Pltt offcr21 = (Pltt)_talk.Resources["PLTToffcr21"];
			Pltt offbak = (Pltt)_talk.Resources["PLTToffbak"];
			Pltt ssrobe9 = (Pltt)_talk.Resources["PLTTssrobe9"];
			Pltt ssbak = (Pltt)_talk.Resources["PLTTssbak"];
			Pltt doffbak = (Pltt)_talk.Resources["PLTTdoffbak"];
			Pltt dssbak = (Pltt)_talk.Resources["PLTTdssbak"];
			ColorPalette brf_off = Pltt.ConvertToPalette(new Pltt[] { standard, offcr21, offbak });
			ColorPalette brf_ss = Pltt.ConvertToPalette(new Pltt[] { standard, ssrobe9, ssbak });
			ColorPalette dbrf_off = Pltt.ConvertToPalette(new Pltt[] { standard, offcr21, doffbak });
			ColorPalette dbrf_ss = Pltt.ConvertToPalette(new Pltt[] { standard, ssrobe9, dssbak });
			((Delt)_talk.Resources["DELToffbak"]).Palette = brf_off;
			Delt temp = (Delt)_talk.Resources["DELTofftxt"];
			temp.Palette = brf_off;
			temp.Image.MakeTransparent(Color.Black);
			((Delt)_talk.Resources["DELTdoffbak"]).Palette = dbrf_off;
			temp = (Delt)_talk.Resources["DELTdofftxt"];
			temp.Palette = dbrf_off;
			temp.Image.MakeTransparent(Color.Black);
			temp = (Delt)_talk.Resources["DELToffcr21"];
			temp.Palette = brf_off;
			temp.Image.MakeTransparent(Color.Black);
			((Anim)_talk.Resources["ANIMeyes"]).SetPalette(brf_off);
			((Anim)_talk.Resources["ANIMmouth"]).SetPalette(brf_off);
			((Delt)_talk.Resources["DELTssbak"]).Palette = brf_ss;
			temp = (Delt)_talk.Resources["DELTsstxt"];
			temp.Palette = brf_ss;
			temp.Image.MakeTransparent(Color.Black);
			((Delt)_talk.Resources["DELTdssbak"]).Palette = dbrf_ss;
			temp = (Delt)_talk.Resources["DELTdsstxt"];
			temp.Palette = dbrf_ss;
			temp.Image.MakeTransparent(Color.Black);
			temp = (Delt)_talk.Resources["DELTssrobe9"];
			temp.Palette = brf_ss;
			temp.Image.MakeTransparent(Color.Black);
			((Anim)_talk.Resources["ANIMssface"]).SetPalette(brf_ss);
			// offcr21, eyes, mouth, ssrobe9 and ssface technically change between brf and dbrf, but not really
			((LfdReader.Font)_empire.Resources[_fontID]).SetColor(_normalText);
			#endregion
			#region array declarations
			_opts[0] = optPreOff;
			_opts[1] = optPreSec;
			_opts[2] = optPostOff;
			_opts[3] = optPostSec;
			for(int i = 0; i < 4; i++) _opts[i].CheckedChanged += new EventHandler(optsArr_CheckedChanged);
			_opts[0].Checked = true;
			#endregion
		}

		#region controls
		void optsArr_CheckedChanged(object sender, EventArgs e)
		{
			// really this is only to make sure that paint only fires once per event
			if (!((RadioButton)sender).Checked) return;
			loadBackAndQuestions();
		}

		void pctPreview_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			// blow image up to 640x400
			g.DrawImage(_preview, 0, 0, _preview.Width*2, _preview.Height*2);
		}

		void pctPreview_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.X > 244 && e.X < 622 && e.Y < 390)
			{
				if (e.Y < 232)	// encompasses answer and page blocks
					{ if (cmdNext.Enabled) cmdNext_Click("pctPreview_MouseUp", new EventArgs()); }
				else if (e.Y > 290)	// questions
				{
					int question = (e.Y - 290) / 20;	// the question line clicked, 0-4
					if (_indexes[question] == 255) return;	// blank Q/A set
					_selectedIndex = _indexes[question];
					_answerLines = _currentAnswer.Split(new string[] { "\r\n" }, StringSplitOptions.None);
					_page = 1;
					cmdPrevious.Enabled = false;
					cmdNext.Enabled = (_numberOfPages > 1);
					loadPage();
				}
				// else whitespace
			}
			// else whitespace
		}
		
		void cmdClose_Click(object sender, EventArgs e) { Close(); }

		void cmdPrevious_Click(object sender, EventArgs e)
		{
			_page--;
			if (_page == 1) cmdPrevious.Enabled = false;
			cmdNext.Enabled = true;
			loadPage();
		}

		void cmdNext_Click(object sender, EventArgs e)
		{
			_page++;
			if (_page == _numberOfPages) cmdNext.Enabled = false;
			cmdPrevious.Enabled = true;
			loadPage();
		}
		#endregion controls

		#region methods
		void loadPage()
		{
			loadBackAndQuestions();
			int offset = (_page - 1) * 10;
			for (int i = 0; i < 10; i++)
			{
				if (i + offset == _answerLines.Length) break;
				displayString(_answerLines[i + offset], 122, (short)(6 + i * 10));
			}
			displayString("Page " + _page + " of " + _numberOfPages, 234, 106);
			pctPreview.Invalidate();
		}

		void displayString(string text, short left, short top)
		{
			// in-game there's a shadow at (+1, +1) that I'm ignoring here
			LfdReader.Font font8 = (LfdReader.Font)_empire.Resources[_fontID];
			char[] chars = text.ToCharArray();
			int offset = left;
			byte glyph;
			Graphics g = Graphics.FromImage(_preview);
			for (int i = 0; i < chars.Length; i++)
			{
				if (chars[i] == '[')
				{
					font8.SetColor(_highlight);
					continue;
				}
				else if (chars[i] == ']')
				{
					font8.SetColor(_normalText);
					continue;
				}
				glyph = Convert.ToByte(chars[i] - font8.StartingChar);
				g.DrawImageUnscaled(font8.Glyphs[glyph], offset, top);
				offset += font8.Glyphs[glyph].Width + 1;
			}
			g.Dispose();
		}

		void loadBackAndQuestions()
		{
			Graphics g = Graphics.FromImage(_preview);
			Delt offcr21 = ((Delt)_talk.Resources["DELToffcr21"]);
			Delt ssrobe9 = ((Delt)_talk.Resources["DELTssrobe9"]);
			Anim eyes = ((Anim)_talk.Resources["ANIMeyes"]);
			Anim ssface= ((Anim)_talk.Resources["ANIMssface"]);
			Anim mouth = ((Anim)_talk.Resources["ANIMmouth"]);
			switch (_qaSet)
			{
				// original graphics are 320x200, shift values come from the appropriate FILM
				case 0:
					Delt offbak = ((Delt)_talk.Resources["DELToffbak"]);
					Delt offtxt = ((Delt)_talk.Resources["DELTofftxt"]);
					g.DrawImageUnscaled(offbak.Image, 0, 0);
					g.DrawImageUnscaled(offtxt.Image, offtxt.Left, offtxt.Top);
					g.DrawImageUnscaled(offcr21.Image, offcr21.Left, offcr21.Top + 10);
					g.DrawImageUnscaled(eyes.Frames[0].Image, eyes.Left, eyes.Top);
					g.DrawImageUnscaled(mouth.Frames[0].Image, mouth.Left, mouth.Top);
					break;
				case 1:
					Delt ssbak = ((Delt)_talk.Resources["DELTssbak"]);
					Delt sstxt = ((Delt)_talk.Resources["DELTsstxt"]);
					g.DrawImageUnscaled(ssbak.Image, 0, 0);
					g.DrawImageUnscaled(sstxt.Image, sstxt.Left , sstxt.Top);
					g.DrawImageUnscaled(ssrobe9.Image, ssrobe9.Left - 24, ssrobe9.Top + 12);
					g.DrawImageUnscaled(ssface.Frames[0].Image, ssface.Left - 24, ssface.Top + 12);
					break;
				case 2:
					Delt doffbak = ((Delt)_talk.Resources["DELTdoffbak"]);
					Delt dofftxt = ((Delt)_talk.Resources["DELTdofftxt"]);
					g.DrawImageUnscaled(doffbak.Image, 0, 0);
					g.DrawImageUnscaled(dofftxt.Image, dofftxt.Left, dofftxt.Top);
					g.DrawImageUnscaled(offcr21.Image, offcr21.Left, offcr21.Top + 10);
					g.DrawImageUnscaled(eyes.Frames[0].Image, eyes.Left, eyes.Top);
					g.DrawImageUnscaled(mouth.Frames[0].Image, mouth.Left, mouth.Top);
					break;
				case 3:
					Delt dssbak = ((Delt)_talk.Resources["DELTdssbak"]);
					Delt dsstxt = ((Delt)_talk.Resources["DELTdsstxt"]);
					g.DrawImageUnscaled(dssbak.Image, 0, 0);
					g.DrawImageUnscaled(dsstxt.Image, dsstxt.Left, dsstxt.Top);
					g.DrawImageUnscaled(ssrobe9.Image, ssrobe9.Left - 24, ssrobe9.Top + 12);
					g.DrawImageUnscaled(ssface.Frames[0].Image, ssface.Left - 24, ssface.Top + 12);
					break;
			}
			g.Dispose();
			((LfdReader.Font)_empire.Resources[_fontID]).SetColor(_normalText);
			int used = 0;
			for (_selectedIndex = 4; _selectedIndex > -1; _selectedIndex--)
			{
				_indexes[_selectedIndex] = 255;
				string q = _currentQuestion;
				if (q != "" || _currentAnswer != "")
				{
					displayString(q, 122, (short)(145 + (4 - used) * 10));
					_indexes[4 - used] = (byte)_selectedIndex;
					used++;
				}
			}
			pctPreview.Invalidate();
		}
		#endregion methods
		
		#region properties
		int _qaSet
		{
			get
			{
				for (int i = 0; i < 4; i++)
					if (_opts[i].Checked) return i;
				return -1;	// this never happen, but makes the compiler happy
			}
		}

		int _offset { get { return (_qaSet % 2) * 5; } }

		string _currentQuestion
		{
			get
			{
				if (_qaSet < 2) return _questions.PreMissQuestions[_selectedIndex + _offset];
				else return _questions.PostMissQuestions[_selectedIndex + _offset];
			}
		}

		string _currentAnswer
		{
			get
			{
				if (_qaSet < 2) return _questions.PreMissAnswers[_selectedIndex + _offset];
				else return _questions.PostMissAnswers[_selectedIndex + _offset];
			}
		}

		int _numberOfPages { get { return _answerLines.Length / 10 + 1; } }
		#endregion
	}
}
