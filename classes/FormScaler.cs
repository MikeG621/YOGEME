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

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Idmr.Yogeme
{
	internal class FormScaler : IDisposable
	{
		/// <summary>Allows customizing certain aspects of scaling behavior.</summary>
		[Flags]
		public enum ScaleFlags
		{
			/// <summary>No special behavior, all controls scaled.</summary>
			None = 0,
			/// <summary>If two labels are left aligned and vertically touching, the top label is stretched to meet the bottom label.</summary>
			StretchLabel = 1,
			/// <summary>Overwrites the scaling method for <see cref="PictureBox"/> controls to stretch the image to fit the control.</summary>
			StretchPicture = 2,
			/// <summary>Forces <see cref="PictureBox"/> controls to always retain their original size.</summary>
			ExcludePicture = 4,
		}

		#region Properties and vars
		/// <summary>The form attached to the scaler.</summary>
		public Form Form { get; private set; }
		/// <summary>The original size of the attached form, before any scaling was applied.</summary>
		public Size DefaultSize { get; private set; }

		ScaleFlags _flags = ScaleFlags.None;
		float _scale = 1.0f;          // Current scale applied
		Font _defaultFont;            // Primary original font used by the form
		bool _eventsActive = false;   // Tracks whether event handlers are added to the form, for cleanup purposes
		Dictionary<Control, Bitmap> _origBitmaps = new Dictionary<Control, Bitmap>();           // Original bitmaps assigned to buttons
		Dictionary<Control, ControlInfo> _origLocations = new Dictionary<Control, ControlInfo>();   // Original location and size of certain controls which need this information for special behavior
		Dictionary<Control, Font> _origFonts = new Dictionary<Control, Font>();                 // Original fonts of all controls, in case any are different
		Dictionary<Font, Font> _fontMap = new Dictionary<Font, Font>();                         // Indexed by original font, contains the current replacement font
		HashSet<Control> _exclude = null;      // List of controls excluded from scaling
		EventHandler _layoutCallback = null;   // Callback to handle layout adjustments when a new scale is applied
		#endregion Properties and vars

		#region Static vars and methods
		/// <summary>Cache of all instantiated scaler objects.</summary>
		/// <remarks>Allows a global change in scale to invoke a change in all open forms.</remarks>
		static HashSet<WeakReference> _scalerCache = new HashSet<WeakReference>();

		static float _globalScale = 1.0f;
		/// <summary>The default scale used when instantiating a new scaler.  If changed, instantly invokes the new scale in all currently registered forms.</summary>
		/// <remarks>Assigned and maintained in <see cref="Settings"/>.</remarks>
		public static float GlobalScale
		{
			get => _globalScale;
			set { _globalScale = value; applyGlobalScale(); }
		}

		/// <summary>Applies the default scale to all open forms, if they still exist.</summary>
		private static void applyGlobalScale()
		{
			if (_scalerCache == null) return;
			foreach (WeakReference fsRef in _scalerCache)
				if (fsRef.Target is FormScaler scaler)
					scaler.Scale(_globalScale);
		}
		#endregion Static properties and methods

		#region Constructors
		/// <summary>Initializes the hooks the scaler into the specified form.</summary>
		/// <remarks>Instantiation should occur after the form's controls have finished setting up.  New controls may not be affected.</remarks>
		/// <param name="form">Form to hook into.</param>
		/// <param name="scale">Initial scale to apply.</param>
		/// <param name="flags">Behavior flags to apply.</param>
		/// <param name="layoutCallback">Form event to invoke when the scale has been changed.</param>
		/// <param name="exclude">List of controls to exclude from scaling.</param>
		public FormScaler(Form form) => init(form, GlobalScale, ScaleFlags.None, null, null);

		/// <inheritdoc cref="FormScaler.FormScaler(Form)"/>
		public FormScaler(Form form, EventHandler layoutCallback) => init(form, GlobalScale, ScaleFlags.None, layoutCallback, null);

		/// <inheritdoc cref="FormScaler.FormScaler(Form)"/>
		public FormScaler(Form form, params Control[] exclude) => init(form, GlobalScale, ScaleFlags.None, null, exclude);

		/// <inheritdoc cref="FormScaler.FormScaler(Form)"/>
		public FormScaler(Form form, ScaleFlags flags) => init(form, GlobalScale, flags, null, null);

		/// <inheritdoc cref="FormScaler.FormScaler(Form)"/>
		public FormScaler(Form form, ScaleFlags flags, EventHandler layoutCallback) => init(form, GlobalScale, flags, layoutCallback, null);

		void init(Form form, float scale, ScaleFlags flags, EventHandler layoutCallback, Control[] exclude)
		{
			if (scale <= 1.0f) scale = GlobalScale;

			// The ignore flag will generate excluded controls during the scan process.
			if (exclude != null || flags.HasFlag(ScaleFlags.ExcludePicture))
			{
				_exclude = new HashSet<Control>();

				if (exclude != null)
					foreach (Control c in exclude)
						_exclude.Add(c);
			}

			Form = form;
			DefaultSize = form.Size;
			_flags = flags;
			_defaultFont = form.Font;
			_layoutCallback = layoutCallback;
			scanControls(form);
			updateEventHandlers(true);

			if (scale > 1.0f)
				setFormScale(scale);

			_scalerCache.Add(new WeakReference(this));

			// Don't need the results, but this a convenient place to clean inaccessible items from the cache.
			// Otherwise it keeps growing with new forms until a rescale operation, which could never happen.
			getActiveScalers();
		}
		#endregion Constructors and init

		#region Dispose
		~FormScaler()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (Form != null)
			{
				updateEventHandlers(false);
				disposeScaledControl(Form);
				Form = null;
			}
		}
		#endregion Dispose

		#region Public methods
		/// <summary>Adjusts the scale of this form.</summary>
		public void Scale(float scale)
		{
			if (Form != null)
			{
				setFormScale(scale);
				_layoutCallback?.Invoke("FormScaler", new EventArgs());
			}
		}

		/// <summary>Checks if any active forms might exceed the screen size if the new scaling would be applied.</summary>
		/// <remarks>Forms that exceed the screen size will be clipped and controls may become inaccessible.</remarks>
		/// <param name="scale">New scale to check.</param>
		/// <returns>True if no issues are detected.</returns>
		public bool VerifyScale(float scale)
		{
			bool failed = false;
			// Always allow smaller scales to activate.
			if (scale > _scale)
			{
				// Font scaling doesn't exactly conform to the input size, so increase the heuristic.
				scale *= 1.05f;
				foreach (FormScaler scaler in getActiveScalers())
				{
					Rectangle r = Screen.FromControl(scaler.Form).Bounds;
					Size def = scaler.DefaultSize;
					int newWidth = (int)(def.Width * scale);
					int newHeight = (int)(def.Height * scale);
					failed |= (newWidth >= r.Width || newHeight >= r.Height);
				}
			}
			
			return !failed;
		}
		#endregion Public methods

		#region Private methods

		private void event_Refresh(object sender, EventArgs e) => repaintControls(Form);
		private void event_FormClosed(object sender, EventArgs e) => Dispose();

		/// <summary>Add or remove all event subscriptions applied to the form by the scaler.</summary>
		void updateEventHandlers(bool add)
		{
			// This can be called twice if Dispose is manually invoked before the form is closed.
			// Guarding it with the flag prevents it from trying to access a null form.
			if (add && !_eventsActive)
			{
				// Moving or resizing the form can cause graphical artifacts for custom painted controls,
				// so they must be repainted.  Closing the form will perform cleanup.
				// Note: refreshing on these rapid updates is CPU intensive on control-heavy forms.  May not be worth it?
				Form.Move += event_Refresh;
				Form.Resize += event_Refresh;
				Form.ResizeEnd += event_Refresh;
				Form.FormClosed += event_FormClosed;
				updatePaintHandler(true, Form, typeof(CheckBox), checkBox_Paint);
				updatePaintHandler(true, Form, typeof(RadioButton), radioButton_Paint);
				_eventsActive = true;
			}
			else if (!add && _eventsActive)
			{
				Form.Move -= event_Refresh;
				Form.Resize -= event_Refresh;
				Form.ResizeEnd -= event_Refresh;
				Form.FormClosed -= event_FormClosed;
				updatePaintHandler(false, Form, typeof(CheckBox), checkBox_Paint);
				updatePaintHandler(false, Form, typeof(RadioButton), radioButton_Paint);
				_eventsActive = false;
			}
		}

		/// <summary>Recursively adds or removes a custom paint handler to all controls of the specified type.</summary>
		void updatePaintHandler(bool add, Control c, Type type, PaintEventHandler handler)
		{
			if (c.GetType() == type)
			{
				if (add)c.Paint += handler;
				else c.Paint -= handler;
			}

			foreach (Control child in c.Controls)
				updatePaintHandler(add, child, type, handler);
		}

		/// <summary>Recursively repaint all controls that were given a custom paint handler.</summary>
		/// <remarks>Resizing or moving a form will leave visual garbage if the custom painting is not refreshed.</remarks>
		void repaintControls(Control control)
		{
			Type t = control.GetType();
			if (t == typeof(CheckBox) || t == typeof(RadioButton))
				control.Invalidate();
			
			foreach (Control c in control.Controls)
				repaintControls(c);
		}

		/// <summary>Retrieves a list of scalers that are still accessible and haven't been garbage collected.</summary>
		/// <remarks>Also filters the cache to remove inaccessible items.</remarks>
		List<FormScaler> getActiveScalers()
		{
			List<FormScaler> result = new List<FormScaler>();
			List<WeakReference> remove = new List<WeakReference>();
			foreach (WeakReference fsRef in _scalerCache)
			{
				FormScaler fs = fsRef.Target as FormScaler;
				if (fs == null || !fs.isActive) remove.Add(fsRef);
				else result.Add(fs);
			}

			if (remove.Count > 0)
			{
				foreach (WeakReference wf in remove)
					_scalerCache.Remove(wf);
			}

			return result;
		}

		bool isActive => (Form != null && !Form.IsDisposed);

		/// <summary>Recursively scans the specified control and builds the information required for the scaler to work.</summary>
		void scanControls(Control control)
		{
			if (control.Font != null)
			{
				if (!_origLocations.ContainsKey(control))
					_origLocations.Add(control, new ControlInfo() { Location = control.Location, Size = control.Size });

				if (_flags.HasFlag(ScaleFlags.ExcludePicture) && control is PictureBox)
					_exclude.Add(control);

				if (!_origFonts.ContainsKey(control))
					_origFonts.Add(control, control.Font);

				if (!_fontMap.ContainsKey(control.Font))
					_fontMap.Add(control.Font, control.Font);
			}

			foreach (Control c in control.Controls)
				scanControls(c);
		}

		/// <summary>Recursively removes and disposes any scaled bitmaps from the specified control and all its child controls.</summary>
		void disposeScaledControl(Control control)
		{
			if (_origBitmaps == null || _origBitmaps.Count == 0) return;

			if (control.GetType() == typeof(Button))
			{
				Button button = (Button)control;
				if (button.Image != null)
				{
					// Assume the original asset was loaded directly from a resx resource, and the cache contains
					// a reference to that image.  Don't dispose the original.
					Bitmap origbmp = _origBitmaps[control];
					if (button.Image != origbmp)
					{
						button.Image.Dispose();
						button.Image = null;
					}
				}
			}

			foreach (Control c in control.Controls)
				disposeScaledControl(c);
		}

		void setFormScale(float scale)
		{
			if (scale < 1.0f) scale = 1.0f;
			else if (scale > 2.5f) scale = 2.5f;

			if (_scale == scale) return;

			// The dictionary can't be modified while iterating through itself.  Build the list first.
			List<Font> fonts = new List<Font>();
			foreach (KeyValuePair<Font, Font> item in _fontMap)
				fonts.Add(item.Key);

			// Replace the loaded fonts with their scaled variations, or restore the original if not scaling.
			foreach (Font orig in fonts)
			{
				if (scale <= 1.0f)
					_fontMap[orig] = orig;
				else
					_fontMap[orig] = new Font(orig.FontFamily, orig.SizeInPoints * scale);
			}

			// The form object is the root control.  Assigning a new font will automatically scale the window and
			// all child controls.  This does not affect any controls that use a different font.  Plus we may need
			// to exclude certain controls, or to scale button images.  Recursively apply every control too.

			_scale = scale;
			Form.AutoScaleMode = AutoScaleMode.Font;
			scaleControl(Form, scale, false);

			// There's an issue with scaling.  If a form grows beyond the physical screen size, its maximum size
			// is clamped.  It uses this clamped size for all subsequent scale operations, even if shrinking back
			// within the screen limits.  Any non-resizable forms will remain clipped until re-opened.  This
			// ensures the correct size at the original scale, but scales above 1.0 may still be clipped.
			if (_scale == 1.0f)	Form.Size = DefaultSize;

			if (_flags.HasFlag(ScaleFlags.StretchLabel))
				stretchLabels(Form);
		}

		bool isExcluded(Control control) => (_exclude != null && _exclude.Contains(control));

		/// <summary>Recursively applies the new font and any special handling to the specified control.</summary>
		void scaleControl(Control control, float scale, bool exclude)
		{
			if (exclude || isExcluded(control))
			{
				control.Font = _defaultFont;
				control.Size = _origLocations[control].Size;
				foreach (Control c in control.Controls)
					scaleControl(c, scale, true);
				return;
			}

			// Assign the new font.
			if (control.Font != null && _origFonts.ContainsKey(control))
				control.Font = _fontMap[_origFonts[control]];

			Type type = control.GetType();
			if (type == typeof(ListBox))
			{
				// For lists with custom painting, the font will change but the line height will not.
				// We must assign a new height, including space for the border above and below.
				ListBox list = (ListBox)control;
				if (list.DrawMode != DrawMode.Normal)
					list.ItemHeight = control.Font.Height + 2;
			}
			else if (type == typeof(PictureBox))
			{
				if (_flags.HasFlag(ScaleFlags.StretchPicture))
					((PictureBox)control).SizeMode = PictureBoxSizeMode.StretchImage;
			}
			else if (type == typeof(Button))
			{
				Button button = (Button)control;

				// For buttons with a foreground image, create a new scaled bitmap and assign it.
				if (!_origBitmaps.ContainsKey(control))
					_origBitmaps.Add(control, (Bitmap)button.Image);

				if (button.Image != null)
				{
					Bitmap origbmp = _origBitmaps[control];

					if (button.Image != origbmp) button.Image.Dispose();

					if (scale > 1.0f)
					{
						int width = (int)(origbmp.Width * scale);
						int height = (int)(origbmp.Height * scale);
						Bitmap bmp = new Bitmap(width, height);
						using (Graphics g = Graphics.FromImage(bmp))
						{
							g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
							g.DrawImage(origbmp, new Rectangle(0, 0, width, height));
						}
						button.Image = bmp;
					}
					else button.Image = origbmp;   // No scaling, restore original
				}
			}

			foreach (Control c in control.Controls)
				scaleControl(c, scale, false);
		}

		void stretchLabels(Control control)
		{
			if (control.Controls.Count < 2) return;

			// Recursively checks the controls against all other controls in the container.  If
			// the original alignment for the second control is along the edge of the first,
			// then the first control is expanded slightly to cover the gap.  This is for the
			// interactive labels in the main form, to prevent unsightly gaps from the uneven
			// integer scaling.
			foreach (Control one in control.Controls)
			{
				foreach (Control two in control.Controls)
				{
					if (one == two || !_origLocations.ContainsKey(one) || !_origLocations.ContainsKey(two))
						continue;

					ControlInfo c1 = _origLocations[one];
					ControlInfo c2 = _origLocations[two];

					// Make sure it's in the same column.
					if (c1.Location.X != c2.Location.X) continue;

					// If the labels were originally stacked, pad them if they're misaligned.
					if (c1.Location.Y + c1.Size.Height == c2.Location.Y)
						one.Height += (two.Top - one.Bottom);
				}

				stretchLabels(one);
			}
		}
		#endregion Private methods

		#region Custom painting
		/// <summary>Calculates the position and size of the graphical and text components of a custom painted Checkbox or Radio button.</summary>
		void setAlignment(Control c, Graphics g, Rectangle clipRect, System.Drawing.ContentAlignment align, RightToLeft rtl, out Rectangle boxRect, out Rectangle textRect)
		{
			// This is mostly accurate to original unscaled checkboxes and radio buttons with default
			// properties.  It has many simplifications, only supporting a few basic properties.
			// This will never replace the original WinForms formatting.

			// Mini rant: I just wanted scaled graphics for the boxes.  Initially it seemed easy.  It's
			// just two functions to render the box and text.  Simple, right?  Nope!  Text wasn't
			// formatted correctly, the graphics weren't in the right spot, clicking on a control sends
			// different clip bounds, left vs right alignment, multiline alignment, vertical centering,
			// wordwrapping, etc. Something, somewhere was always wrong.  This was absolutely not worth
			// the effort or time investment.  But it's mostly working now, so we might as well use it.

			bool isCheckBox = c is CheckBox;
			bool isRadio = c is RadioButton;

			bool rightAlign = (align == System.Drawing.ContentAlignment.MiddleRight || align == System.Drawing.ContentAlignment.TopRight || align == System.Drawing.ContentAlignment.BottomRight);
			bool rightToLeft =  (rtl == RightToLeft.Yes);

			// Mousedown events will pass a different clip area.  Need to calculate a position manually.
			// Force vertical centering based on control size regardless of what was passed to it.
			boxRect = clipRect;
			int fontHeight = (int)Math.Ceiling(c.Font.GetHeight(g));
			int oldY = boxRect.Y;
			boxRect.Y = Math.Max(0, (c.Height / 2) - (fontHeight / 2) - 1);

			int glyphWidth = (isCheckBox ? CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width :
										RadioButtonRenderer.GetGlyphSize(g, RadioButtonState.UncheckedNormal).Width );

			int boxWidth = (int)(glyphWidth * _scale);
			boxRect.Height = boxWidth;
			boxRect.Width = boxWidth;
			if (isCheckBox) boxWidth += 2;  // For text alignment

			textRect = clipRect;
			// If the form goes off the edge of the screen, the clip bounds will be smaller, but text
			// measurement needs full size.
			textRect.Width = c.Width;
			if (isRadio) textRect.X++;
			textRect.Width -= glyphWidth;

			// TextRenderer is more accurate for measuring and drawing GUI elements rather than going
			// through Graphics.  It can handle "&" underlining.
			Size sz = TextRenderer.MeasureText(g, c.Text, c.Font, textRect.Size, TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter);
			textRect.Y = Math.Max(0, (c.Height / 2) - (sz.Height / 2) - 1);

			if (rightAlign || rightToLeft)
			{
				boxRect.X = (c.Width - boxWidth) + (isCheckBox ? 1 : 0);
				textRect.X = 0;
				if (rightToLeft)
					textRect.X = Math.Max(0, textRect.Width - sz.Width - (isCheckBox ? 2 : 1));
			}
			else textRect.X += (boxWidth * (rightAlign || rightToLeft ? -1 : 1));

			if (isRadio) boxRect.X--;

			// Some controls may appear horizontally truncated.  This provides the full width to them,
			// but is ultimately constrained by the paint clip bounds. 
			textRect.Width = c.Width;
		}

		private void checkBox_Paint(object sender, PaintEventArgs e)
		{
			if (_scale <= 1.0f) return;   // The base painting will remain visible

			CheckBox c = sender as CheckBox;
			setAlignment(c, e.Graphics, e.ClipRectangle, c.CheckAlign, c.RightToLeft, out Rectangle boxRect, out Rectangle textRect);

			e.Graphics.FillRectangle(SystemBrushes.Control, e.ClipRectangle);
			ButtonState state = ButtonState.Normal;
			if (c.CheckState != CheckState.Unchecked) state |= ButtonState.Checked;
			if (!c.Enabled || c.CheckState == CheckState.Indeterminate) state |= ButtonState.Inactive;
			ControlPaint.DrawCheckBox(e.Graphics, boxRect, state);
			TextRenderer.DrawText(e.Graphics, c.Text, c.Font, textRect, (c.Enabled ? SystemColors.ControlText : SystemColors.ControlDark), SystemColors.Control, TextFormatFlags.WordBreak);
		}

		private void radioButton_Paint(object sender, PaintEventArgs e)
		{
			if (_scale <= 1.0f) return;   // The base painting will remain visible

			RadioButton c = sender as RadioButton;
			setAlignment(c, e.Graphics, e.ClipRectangle, c.CheckAlign, c.RightToLeft, out Rectangle boxRect, out Rectangle textRect);

			e.Graphics.FillRectangle(SystemBrushes.Control, e.ClipRectangle);
			ButtonState state = (c.Checked ? ButtonState.Checked : ButtonState.Normal);
			if (!c.Enabled) state |= ButtonState.Inactive;
			ControlPaint.DrawRadioButton(e.Graphics, boxRect, state);
			TextRenderer.DrawText(e.Graphics, c.Text, c.Font, textRect, (c.Enabled ? SystemColors.ControlText : SystemColors.ControlDark), SystemColors.Control, TextFormatFlags.WordBreak);
		}
		#endregion Custom painting

		class ControlInfo
		{
			public Point Location;
			public Size Size;
		}
	}
}
