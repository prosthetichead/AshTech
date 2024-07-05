using AshTech.Core;
using AshTech.Draw;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.UI.Widgets
{
    public class Button : UIWidget
    {
        public Color[] textColor = { Color.Black };
        public string value;


        //public bool MouseInBounds { get { return ScreenBounds.Contains(desktop.mousePosition); } }

        public Button(string name, Rectangle bounds, DesktopAnchor anchor) : base(name, bounds, anchor)
        {
            TextAlignment = Alignment.CenterCenter;

            Texture2D defaultButtonTexture = AssetManager.LoadTexture2D("ui/button.png", "ashtech.zip", "ui-default-button-texture");
            drawBackgroundSpriteBox = true;
            SetBackground(defaultButtonTexture, 16);
        }

        internal override void Update(GameTime gameTime)
        {
            
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                BackgroundSpriteBox.Draw(spriteBatch);

                Rectangle drawPos = DesktopBounds;
                spriteBatch.DrawString(Font, value, rectangle: drawPos, TextAlignment, textColor);
                
            }
        }
    }
}
