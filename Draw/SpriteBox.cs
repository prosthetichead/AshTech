using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.Draw
{
    public class SpriteBox
    {
        private SpriteSheet boxSpriteSheet;
        public Rectangle bounds;
        public Color color = Color.White;

        public int topLeftSpriteIndex;
        public int topRightSpriteIndex;
        public int topCenterSpriteIndex;
        public int centerLeftSpriteIndex;
        public int centerRightSpriteIndex;
        public int centerSpriteIndex;
        public int bottomLeftSpriteIndex;
        public int bottomRightSpriteIndex;
        public int bottomCenterSpriteIndex;

        public SpriteBox(Texture2D boxTexture, int spriteSize, Rectangle bounds, int topLeftSpriteIndex = 0, int topCenterSpriteIndex = 1,
                                  int topRightSpriteIndex = 2, int centerLeftSpriteIndex = 3, int centerSpriteIndex = 4,
                                  int centerRightSpriteIndex = 5, int bottomLeftSpriteIndex = 6, int bottomCenterSpriteIndex = 7, int bottomRightSpriteIndex = 8)
        {
            this.bounds = bounds;
            boxSpriteSheet = new SpriteSheet(spriteSize, spriteSize, boxTexture);
            this.topLeftSpriteIndex = topLeftSpriteIndex;
            this.topCenterSpriteIndex = topCenterSpriteIndex;
            this.topRightSpriteIndex = topRightSpriteIndex;
            this.centerLeftSpriteIndex = centerLeftSpriteIndex;
            this.centerSpriteIndex = centerSpriteIndex;
            this.centerRightSpriteIndex = centerRightSpriteIndex;
            this.bottomLeftSpriteIndex = bottomLeftSpriteIndex;
            this.bottomCenterSpriteIndex = bottomCenterSpriteIndex;
            this.bottomRightSpriteIndex = bottomRightSpriteIndex;
        }

        public SpriteBox(SpriteSheet boxSpriteSheet, Rectangle bounds, int topLeftSpriteIndex = 0, int topCenterSpriteIndex = 1,
                                  int topRightSpriteIndex = 2, int centerLeftSpriteIndex = 3, int centerSpriteIndex = 4,
                                  int centerRightSpriteIndex = 5, int bottomLeftSpriteIndex = 6, int bottomCenterSpriteIndex = 7, int bottomRightSpriteIndex = 8)
        {
            this.bounds = bounds;
            this.boxSpriteSheet = boxSpriteSheet;
            this.topLeftSpriteIndex = topLeftSpriteIndex;
            this.topCenterSpriteIndex = topCenterSpriteIndex;
            this.topRightSpriteIndex = topRightSpriteIndex;
            this.centerLeftSpriteIndex = centerLeftSpriteIndex;
            this.centerSpriteIndex = centerSpriteIndex;
            this.centerRightSpriteIndex = centerRightSpriteIndex;
            this.bottomLeftSpriteIndex = bottomLeftSpriteIndex;
            this.bottomCenterSpriteIndex = bottomCenterSpriteIndex;
            this.bottomRightSpriteIndex = bottomRightSpriteIndex;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //top left corner
            boxSpriteSheet.Draw(spriteBatch, topLeftSpriteIndex, new Vector2(bounds.X, bounds.Y), color, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            //top
            boxSpriteSheet.Draw(spriteBatch, topCenterSpriteIndex, new Rectangle(bounds.X + boxSpriteSheet.SpriteWidth, bounds.Y, bounds.Width - (boxSpriteSheet.SpriteWidth*2), boxSpriteSheet.SpriteHeight), color, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            // top right corner                
            boxSpriteSheet.Draw(spriteBatch, topRightSpriteIndex, new Vector2(bounds.X + bounds.Width, bounds.Y), color, 0f, new Vector2(boxSpriteSheet.SpriteWidth, 0), SpriteEffects.None, 0f);

            //left 
            boxSpriteSheet.Draw(spriteBatch, centerLeftSpriteIndex, new Rectangle(bounds.X, bounds.Y + boxSpriteSheet.SpriteHeight, boxSpriteSheet.SpriteWidth, bounds.Height - (boxSpriteSheet.SpriteHeight * 2)), color, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            //Center 
            boxSpriteSheet.Draw(spriteBatch, centerSpriteIndex, new Rectangle(bounds.X + boxSpriteSheet.SpriteWidth, bounds.Y + boxSpriteSheet.SpriteHeight, bounds.Width - (boxSpriteSheet.SpriteWidth * 2), bounds.Height - (boxSpriteSheet.SpriteHeight * 2)), color, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            //right
            boxSpriteSheet.Draw(spriteBatch, centerRightSpriteIndex, new Rectangle(bounds.X + bounds.Width, bounds.Y + boxSpriteSheet.SpriteHeight, boxSpriteSheet.SpriteWidth, bounds.Height - (boxSpriteSheet.SpriteHeight * 2)), color, 0f, new Vector2(boxSpriteSheet.SpriteWidth, 0), SpriteEffects.None, 0f);

            //bottom left corner
            boxSpriteSheet.Draw(spriteBatch, bottomLeftSpriteIndex, new Vector2(bounds.X, bounds.Y + bounds.Height), color, 0f, new Vector2(0, boxSpriteSheet.SpriteHeight), SpriteEffects.None, 0f);

            //bottom
            boxSpriteSheet.Draw(spriteBatch, bottomCenterSpriteIndex, new Rectangle(bounds.X + boxSpriteSheet.SpriteWidth, bounds.Y + bounds.Height, bounds.Width - (boxSpriteSheet.SpriteWidth * 2), boxSpriteSheet.SpriteHeight), color, 0f, new Vector2(0, boxSpriteSheet.SpriteHeight), SpriteEffects.None, 0f);

            //bottom right corner
            boxSpriteSheet.Draw(spriteBatch, bottomRightSpriteIndex, new Vector2(bounds.X + bounds.Width, bounds.Y + bounds.Height), color, 0f, new Vector2(boxSpriteSheet.SpriteWidth, boxSpriteSheet.SpriteHeight), SpriteEffects.None, 0f);

        }
    }
}
