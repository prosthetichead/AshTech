using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.Core
{
    public static class FontHelper
    {
        public enum Alignment
        {
            TopLeft,
            TopCenter,
            TopRight,

            CenterLeft,
            CenterCenter,
            CenterRight,

            BottomLeft,
            BottomCenter,
            BottomRight,
        }

        public static void DrawStringRectangle(SpriteBatch spriteBatch, SpriteFontBase font, string text, Rectangle rectangle, Alignment alignment, Color[] color,
                                               Vector2? scale = null, float rotation = 0, float layerDepth = 0, float characterSpacing = 0, float lineSpacing = 0,
                                               TextStyle textStyle = TextStyle.None, FontSystemEffect fontSystemEffect = FontSystemEffect.None, int effectAmount = 0)
        {
            Vector2 stringSize = font.MeasureString(text);

            if (alignment == Alignment.TopLeft)
            {
                spriteBatch.DrawString(font: font, text: text, position: new Vector2(rectangle.X, rectangle.Y), colors: color, scale: scale, rotation: rotation, origin: new Vector2(0, 0), layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
            if (alignment == Alignment.TopCenter)
            {
                spriteBatch.DrawString(font: font, text: text, position: new Vector2(rectangle.Width / 2 + rectangle.X, rectangle.Y), colors: color, scale: scale, rotation: rotation, origin: new Vector2((int)stringSize.X / 2, 0), layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
            if (alignment == Alignment.TopRight)
            {
                spriteBatch.DrawString(font: font, text: text, position: new Vector2(rectangle.Width + rectangle.X, rectangle.Y), colors: color, scale: scale, rotation: rotation, origin: new Vector2((int)stringSize.X, 0), layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
            if (alignment == Alignment.CenterLeft)
            {
                spriteBatch.DrawString(font: font, text: text, position: new Vector2(rectangle.X, rectangle.Height / 2 + rectangle.Y),  colors: color, scale: scale, rotation: rotation,  layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
            if (alignment == Alignment.CenterCenter)
            {
                spriteBatch.DrawString(font: font, text: text, position: rectangle.Center.ToVector2(), origin: new Vector2((int)stringSize.X / 2, (int)stringSize.Y / 2), colors: color, scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
            if (alignment == Alignment.CenterRight)
            {
                spriteBatch.DrawString(font: font, text: text, position: new Vector2(rectangle.Width + rectangle.X, rectangle.Height / 2 + rectangle.Y), origin: new Vector2(stringSize.X, (int)stringSize.Y / 2), colors: color, scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
            if (alignment == Alignment.BottomLeft)
            {
                spriteBatch.DrawString(font: font, text: text, position: new Vector2(rectangle.X, rectangle.Height + rectangle.Y), origin: new Vector2(0, (int)stringSize.Y), colors: color, scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
            if (alignment == Alignment.BottomCenter)
            {
                spriteBatch.DrawString(font: font, text: text, position: new Vector2(rectangle.Width / 2 + rectangle.X, rectangle.Height + rectangle.Y), origin: new Vector2((int)stringSize.X / 2, stringSize.Y), colors: color, scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
            if (alignment == Alignment.BottomRight)
            {
                spriteBatch.DrawString(font: font, text: text, position: new Vector2(rectangle.Width + rectangle.X, rectangle.Height + rectangle.Y), origin: new Vector2((int)stringSize.X, stringSize.Y), colors: color, scale: scale, rotation: rotation, layerDepth: layerDepth, characterSpacing: characterSpacing, lineSpacing: lineSpacing, textStyle: textStyle, effect: fontSystemEffect, effectAmount: effectAmount);
            }
        }


    }
}
