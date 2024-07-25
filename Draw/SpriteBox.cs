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
        public Rectangle box;
        public Color color = Color.Red;

        public int topLeftSpriteIndex;
        public int topRightSpriteIndex;
        public int topCenterSpriteIndex;
        public int centerLeftSpriteIndex;
        public int centerRightSpriteIndex;
        public int centerSpriteIndex;
        public int bottomLeftSpriteIndex;
        public int bottomRightSpriteIndex;
        public int bottomCenterSpriteIndex;

        public SpriteBox(Texture2D boxTexture, int spriteSize, Rectangle box)
        {
            this.box = box;

            boxSpriteSheet = new SpriteSheet(spriteSize, spriteSize, boxTexture);

            topLeftSpriteIndex = 0;
            topCenterSpriteIndex = 1;
            topRightSpriteIndex = 2;
            centerLeftSpriteIndex = 3;
            centerSpriteIndex = 4;
            centerRightSpriteIndex = 5;
            bottomLeftSpriteIndex = 6;
            bottomCenterSpriteIndex = 7;
            bottomRightSpriteIndex = 8;
        }

        public SpriteBox(SpriteSheet boxSpriteSheet, int topLeftSpriteIndex = 0, int topCenterSpriteIndex = 1,
                                  int topRightSpriteIndex = 2, int centerLeftSpriteIndex = 3, int centerSpriteIndex = 4,
                                  int centerRightSpriteIndex = 5, int bottomLeftSpriteIndex = 6, int bottomCenterSpriteIndex = 7, int bottomRightSpriteIndex = 8)
        {
            this.box = new Rectangle(0,0,100,100);
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
            boxSpriteSheet.Draw(spriteBatch, topLeftSpriteIndex, new Vector2(box.X, box.Y), color, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            //top
            boxSpriteSheet.Draw(spriteBatch, topCenterSpriteIndex, new Rectangle(box.X + boxSpriteSheet.SpriteWidth, box.Y, box.Width - (boxSpriteSheet.SpriteWidth*2), boxSpriteSheet.SpriteHeight), color, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            // top right corner                
            boxSpriteSheet.Draw(spriteBatch, topRightSpriteIndex, new Vector2(box.X + box.Width, box.Y), color, 0f, new Vector2(boxSpriteSheet.SpriteWidth, 0), SpriteEffects.None, 0f);

            //left 
            boxSpriteSheet.Draw(spriteBatch, centerLeftSpriteIndex, new Rectangle(box.X, box.Y + boxSpriteSheet.SpriteHeight, boxSpriteSheet.SpriteWidth, box.Height - (boxSpriteSheet.SpriteHeight * 2)), color, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            //Center 
            boxSpriteSheet.Draw(spriteBatch, centerSpriteIndex, new Rectangle(box.X + boxSpriteSheet.SpriteWidth, box.Y + boxSpriteSheet.SpriteHeight, box.Width - (boxSpriteSheet.SpriteWidth * 2), box.Height - (boxSpriteSheet.SpriteHeight * 2)), color, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            //right
            boxSpriteSheet.Draw(spriteBatch, centerRightSpriteIndex, new Rectangle(box.X + box.Width, box.Y + boxSpriteSheet.SpriteHeight, boxSpriteSheet.SpriteWidth, box.Height - (boxSpriteSheet.SpriteHeight * 2)), color, 0f, new Vector2(boxSpriteSheet.SpriteWidth, 0), SpriteEffects.None, 0f);

            //bottom left corner
            boxSpriteSheet.Draw(spriteBatch, bottomLeftSpriteIndex, new Vector2(box.X, box.Y + box.Height), color, 0f, new Vector2(0, boxSpriteSheet.SpriteHeight), SpriteEffects.None, 0f);

            //bottom
            boxSpriteSheet.Draw(spriteBatch, bottomCenterSpriteIndex, new Rectangle(box.X + boxSpriteSheet.SpriteWidth, box.Y + box.Height, box.Width - (boxSpriteSheet.SpriteWidth * 2), boxSpriteSheet.SpriteHeight), color, 0f, new Vector2(0, boxSpriteSheet.SpriteHeight), SpriteEffects.None, 0f);

            //bottom right corner
            boxSpriteSheet.Draw(spriteBatch, bottomRightSpriteIndex, new Vector2(box.X + box.Width, box.Y + box.Height), color, 0f, new Vector2(boxSpriteSheet.SpriteWidth, boxSpriteSheet.SpriteHeight), SpriteEffects.None, 0f);

        }
    }
}
