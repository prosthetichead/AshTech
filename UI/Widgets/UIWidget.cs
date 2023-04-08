using AshTech.Draw;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.UI.Widgets
{
    public enum DesktopAnchor
    {
        TopLeft,
        TopRight,  
        BottomLeft,
        BottomRight,
        BottomTop,  
        BottomBottom,
        Center,
    }

    public abstract class UIWidget
    {
        internal Desktop desktop;
        internal Rectangle bounds;
        public bool focus;
        public bool visible;
        public DesktopAnchor anchor;
        public float drawOrder = 0;
        
        private SpriteBox backgroundSpriteBox;
        public SpriteBox BackgroundSpriteBox { get { return backgroundSpriteBox; } set { backgroundSpriteBox = value; } }
         

        public UIWidget(Desktop desktop, Rectangle bounds, DesktopAnchor anchor)
        {
            this.desktop = desktop;
            this.bounds = bounds;
            focus = false;
            visible = true;
            this.anchor = anchor;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public Rectangle DrawPosition()
        {
            switch (anchor)
            {
                case DesktopAnchor.TopLeft:
                    return new Rectangle(desktop.bounds.X + bounds.X, desktop.bounds.Y + bounds.Y, bounds.Width, bounds.Height);                    
                case DesktopAnchor.TopRight:
                    return new Rectangle((desktop.bounds.X + desktop.bounds.Width) - bounds.Width - bounds.X, desktop.bounds.Y + bounds.Y, bounds.Width, bounds.Height);
                case DesktopAnchor.BottomLeft:
                    return new Rectangle(desktop.bounds.X + bounds.X, (desktop.bounds.Y + desktop.bounds.Height) - bounds.Height - bounds.Y, bounds.Width, bounds.Height);
                case DesktopAnchor.BottomRight:
                    break;
                case DesktopAnchor.BottomTop:
                    break;
                case DesktopAnchor.BottomBottom:
                    break;
                case DesktopAnchor.Center:
                    break;
                default:
                    break;
            }

            return bounds;
        }
    }

    
}
