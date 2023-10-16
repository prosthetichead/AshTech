using AshTech.Core;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using AshTech.Debug;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.UI.Widgets
{
    public class Button : UIWidget
    {

        public  SpriteFontBase font;
        public Alignment textAlignment = Alignment.CenterCenter;
        public Color[] textColor = { Color.Black };
        public string value;

        private bool mouseEnteredBoundsTest = false;
        
        public Button(Rectangle bounds, SpriteFontBase font) : base(bounds)
        {
            this.font = font;

        }

        public override void Update(GameTime gameTime)
        {

            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                Rectangle drawPos = ScreenBounds;
                spriteBatch.DrawString(font, value, rectangle: drawPos, textAlignment, textColor);
                
            }
        }

        public override void HandleInput(GameTime gameTime, InputManager input)
        {
            if (visible)
            {
                //mousePosition = input.MousePosition;
                if (bounds.Contains(input.MousePosition))
                    mouseInBounds = true;
                else
                    mouseInBounds = false;
            }
            else
            {
                mouseInBounds = false;
            }

            if (MouseInBounds)
            {
                if (!mouseEnteredBoundsTest)
                {
                    mouseEnteredBoundsTest = true;  
                    MouseEnteredBounds!.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                if (mouseEnteredBoundsTest)
                {
                    mouseEnteredBoundsTest = false;
                    MouseExitedBounds!.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
