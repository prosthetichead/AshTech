using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AshTech.Core
{
    public class SpriteSheet
    {
        private Texture2D texture;
        private int singleSpriteWidth;
        private int singleSpriteHeight;
                        
        public SpriteSheet(int singleSpriteWidth, int singleSpriteHeight, Texture2D texture)
        {
            this.singleSpriteWidth = singleSpriteWidth;
            this.singleSpriteHeight = singleSpriteHeight;
            this.texture = texture;
        }
                
        private Rectangle GetSourceRectangle(int spriteNumber)
        {
            int rectangleX = spriteNumber % (texture.Width / singleSpriteWidth);
            int rectangleY = spriteNumber / (texture.Width / singleSpriteWidth);

            return new Rectangle(rectangleX * singleSpriteWidth, rectangleY * singleSpriteHeight, singleSpriteWidth, singleSpriteWidth);
        }

        public void Draw(SpriteBatch spriteBatch, int frameNumber, Vector2 position, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffect, float depth)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, singleSpriteWidth, singleSpriteHeight), GetSourceRectangle(frameNumber), color, rotation, origin, spriteEffect, depth);
        }

        public void Draw(SpriteBatch spriteBatch, int frameNumber, Rectangle rectangle,Color color, float rotation, Vector2 origin, SpriteEffects spriteEffect, float depth)
        {
            spriteBatch.Draw(texture, rectangle, GetSourceRectangle(frameNumber), color, rotation, origin, spriteEffect, depth);
        }

       // public void Draw(SpriteBatch spriteBatch, int frameNumber, Vector2 position, Vector2 size, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffect, float depth)
       // {
       //     spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), GetSourceRectangle(frameNumber), color, rotation, origin, spriteEffect, depth);
       // }
    }
}
