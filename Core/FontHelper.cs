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

        public static void DrawStringRectangle(SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle rectangle, Alignment alignment, Color color, float rotation = 0, float scale = 1, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0)
        {
            Vector2 stringSize = font.MeasureString(text);

            if (alignment == Alignment.TopLeft)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.X, rectangle.Y), color, rotation, new Vector2(0, 0), scale, spriteEffects, layerDepth );
            }
            if (alignment == Alignment.TopCenter)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.Width / 2 + rectangle.X, rectangle.Y), color, origin: new Vector2((int)stringSize.X / 2, 0), scale: scale, rotation: rotation, effects: spriteEffects, layerDepth: layerDepth);
            }
            if (alignment == Alignment.TopRight)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.Width + rectangle.X, rectangle.Y), color, origin: new Vector2((int)stringSize.X, 0), scale: scale, rotation: rotation, effects: spriteEffects, layerDepth: layerDepth);
            }
            if (alignment == Alignment.CenterLeft)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.X, rectangle.Height / 2 + rectangle.Y), color, origin: new Vector2(0, (int)stringSize.Y / 2), scale: scale, rotation: rotation, effects: spriteEffects, layerDepth: layerDepth);
            }
            if (alignment == Alignment.CenterCenter)
            {
                spriteBatch.DrawString(font, text, rectangle.Center.ToVector2(), color, origin: new Vector2((int)stringSize.X / 2, (int)stringSize.Y / 2), scale: scale, rotation: rotation, effects: spriteEffects, layerDepth: layerDepth);
            }
            if (alignment == Alignment.CenterRight)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.Width + rectangle.X, rectangle.Height / 2 + rectangle.Y), color, origin: new Vector2(stringSize.X, (int)stringSize.Y / 2), scale: scale, rotation: rotation, effects: spriteEffects, layerDepth: layerDepth);
            }
            if (alignment == Alignment.BottomLeft)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.X, rectangle.Height + rectangle.Y), color, origin: new Vector2(0, (int)stringSize.Y), scale: scale, rotation: rotation, effects: spriteEffects, layerDepth: layerDepth);
            }
            if (alignment == Alignment.BottomCenter)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.Width / 2 + rectangle.X, rectangle.Height + rectangle.Y), color, origin: new Vector2((int)stringSize.X / 2, stringSize.Y), scale: scale, rotation: rotation, effects: spriteEffects, layerDepth: layerDepth);
            }
            if (alignment == Alignment.BottomRight)
            {
                spriteBatch.DrawString(font, text, new Vector2(rectangle.Width + rectangle.X, rectangle.Height + rectangle.Y), color, origin: new Vector2((int)stringSize.X, stringSize.Y), scale: scale, rotation: rotation, effects: spriteEffects, layerDepth: layerDepth);
            }

        }


    }
}
