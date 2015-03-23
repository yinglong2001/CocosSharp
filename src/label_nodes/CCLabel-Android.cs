﻿using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Android.App;
using Android.Graphics;
using Android.Text;
using Android.Util;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public partial class CCLabel
    {

        internal CCTexture2D CreateTextSprite(string text, CCFontDefinition textDefinition)
        {
            if (string.IsNullOrEmpty(text))
                return new CCTexture2D();

            int imageWidth;
            int imageHeight;
            var textDef = textDefinition;
            var contentScaleFactorWidth = CCLabel.DefaultTexelToContentSizeRatios.Width;
            var contentScaleFactorHeight = CCLabel.DefaultTexelToContentSizeRatios.Height;

            textDef.FontSize *= (int)contentScaleFactorWidth;
            textDef.Dimensions.Width *= contentScaleFactorWidth;
            textDef.Dimensions.Height *= contentScaleFactorHeight;

            bool hasPremultipliedAlpha;

            var display = Game.Activity.WindowManager.DefaultDisplay;
            var metrics = new DisplayMetrics();
            display.GetMetrics(metrics);

            // Do not take into account ScaleDensity for now.
//            var fontScaleFactor = metrics.ScaledDensity;
//            textDef.FontSize = (int)(textDef.FontSize * fontScaleFactor);


            // out paint object to hold our drawn text
            var textPaint = new TextPaint(PaintFlags.AntiAlias);
            textPaint.Color = Android.Graphics.Color.White;
            textPaint.TextAlign = Paint.Align.Left;

            textPaint.TextSize = textDef.FontSize;

            var fontName = textDef.FontName;
            var ext = System.IO.Path.GetExtension(fontName);
            if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
            {

              var path = System.IO.Path.Combine(CCContentManager.SharedContentManager.RootDirectory, fontName);
                var activity = Game.Activity;

                try
                {
                    var typeface = Typeface.CreateFromAsset(activity.Assets, path);
                    textPaint.SetTypeface(typeface);
                }
                catch (Exception)
                {
                    textPaint.SetTypeface(Typeface.Create(fontName, TypefaceStyle.Normal));
                }
            }
            else
            {
                textPaint.SetTypeface(Typeface.Create(fontName, TypefaceStyle.Normal));
            }

            // color
            var fontColor = textDef.FontFillColor;
            var fontAlpha = textDef.FontAlpha;
            var foregroundColor = new Android.Graphics.Color(fontColor.R,
                fontColor.G,
                fontColor.B,
                fontAlpha);

            textPaint.Color = foregroundColor;

            // alignment
            var horizontalAlignment = textDef.Alignment;
            var verticleAlignement = textDef.LineAlignment;

            var textAlign = (CCTextAlignment.Right == horizontalAlignment) ? Layout.Alignment.AlignOpposite
                : (CCTextAlignment.Center == horizontalAlignment) ? Layout.Alignment.AlignCenter
                : Layout.Alignment.AlignNormal;

            // LineBreak
            // TODO: Find a way to specify the type of line breaking if possible.

            var dimensions = new CCSize(textDef.Dimensions.Width, textDef.Dimensions.Height);

            var layoutAvailable = true;
            if (dimensions.Width <= 0)
            {
                dimensions.Width = 8388608;
                layoutAvailable = false;
            }

            if (dimensions.Height <= 0)
            {
                dimensions.Height = 8388608;
                layoutAvailable = false;
            }


            // Get bounding rectangle - we need its attribute and method values
            var layout = new StaticLayout(text, textPaint, 
                (int)dimensions.Width, textAlign, 1.0f, 0.0f, false);

            var boundingRect = new Rect();
            var lineBounds = new Rect();

            // Loop through all the lines so we can find our drawing offsets
            var lineCount = layout.LineCount;

            // early out if something went wrong somewhere and nothing is to be drawn
            if (lineCount == 0)
                return new CCTexture2D();

            for (int lc = 0; lc < lineCount; lc++)
            {
                layout.GetLineBounds(lc, lineBounds);
                var max = layout.GetLineMax(lc);

                if (boundingRect.Right < max)
                    boundingRect.Right = (int)max;

                boundingRect.Bottom = lineBounds.Bottom;
            }

            if (!layoutAvailable)
            {
                if (dimensions.Width == 8388608)
                {
                    dimensions.Width = boundingRect.Right;
                }
                if (dimensions.Height == 8388608)
                {
                    dimensions.Height = boundingRect.Bottom;
                }
            }

            imageWidth = (int)dimensions.Width;
            imageHeight = (int)dimensions.Height;

            // Recreate our layout based on calculated dimensions so that we can draw the text correctly
            // in our image when Alignment is not Left.
            if (textAlign != Layout.Alignment.AlignNormal)
            {
                layout = new StaticLayout(text, textPaint, 
                    (int)dimensions.Width, textAlign, 1.0f, 0.0f, false);
            }


            // Line alignment
            var yOffset = (CCVerticalTextAlignment.Bottom == verticleAlignement 
                || boundingRect.Bottom >= dimensions.Height) ? dimensions.Height - boundingRect.Bottom  // align to bottom
                : (CCVerticalTextAlignment.Top == verticleAlignement) ? 0                    // align to top
                : (imageHeight - boundingRect.Bottom) * 0.5f;                                   // align to center

            // Create our platform dependant image to be drawn to.
            var textureBitmap = Bitmap.CreateBitmap(imageWidth, imageHeight, Bitmap.Config.Argb8888);
            var drawingCanvas = new Canvas(textureBitmap);

            // Set our vertical alignment
            drawingCanvas.Translate(0, yOffset);

            // Now draw the text using our layout
            layout.Draw(drawingCanvas);

            // We will use Texture2D from stream here instead of CCTexture2D stream.
            var tex = Texture2D.FromStream(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, textureBitmap);

            // Create our texture of the label string.
            var texture = new CCTexture2D(tex);

            return texture;
        }
    }
}