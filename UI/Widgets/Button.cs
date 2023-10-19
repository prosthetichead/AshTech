using AshTech.Core;
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

        public  SpriteFontBase font;
        public Alignment textAlignment = Alignment.CenterCenter;
        public Color[] textColor = { Color.Black };
        public string value;

        
       

        //public bool MouseInBounds { get { return ScreenBounds.Contains(desktop.mousePosition); } }

        public Button(string name, Rectangle bounds, DesktopAnchor anchor, SpriteFontBase font) : base(name, bounds, anchor)
        {
            this.font = font;
        }

        internal override void Update(GameTime gameTime)
        {

            
        }
        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Rectangle drawPos = DesktopBounds;
                spriteBatch.DrawString(font, value, rectangle: drawPos, textAlignment, textColor);
                
            }
        }
    }
}
