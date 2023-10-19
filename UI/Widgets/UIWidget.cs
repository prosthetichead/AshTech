using AshTech.Core;
using AshTech.Draw;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.UI.Widgets
{


    public abstract class UIWidget
    {           
        public Rectangle Bounds { get { return bounds; } set { bounds = value; } }
        private Rectangle bounds;
                
        public bool Focus { set { focus = value; } get { return focus; } }
        private bool focus;

        public bool Visible { set { visible = value; } get { return visible; } }
        private bool visible;

        public int DrawOrder { get { return drawOrder; } set { drawOrder = value; } }
        private int drawOrder;

        public int SelectOrder { get { return selectOrder; } set { selectOrder = value; } }
        private int selectOrder;


        public string Name { set { name = value; } get { return name; } }
        private string name;

        public UIWidget(string name, Rectangle bounds, int drawOrder = 0, int selectOrder = 0)
        {
            this.bounds = bounds;
            this.name = name;
            this.drawOrder = drawOrder;
            this.selectOrder = selectOrder;
            focus = false;
            visible = false;
        }

        public abstract void LoadContent();
        
        public abstract void UnloadContent();

        internal virtual void Update(GameTime gameTime)
        {
            //check if mouse is in bounds
            if(visible)
            {

            }
        }

        internal abstract void HandleInput(GameTime gameTime, InputManager input);

        internal abstract void Draw(SpriteBatch spriteBatch);

        //{
        //    get
        //    {
        //        switch (anchor)
        //        {
        //            case DesktopAnchor.TopLeft:
        //                return new Rectangle(desktop.bounds.X + bounds.X, desktop.bounds.Y + bounds.Y, bounds.Width, bounds.Height);
        //            case DesktopAnchor.TopRight:
        //                return new Rectangle((desktop.bounds.X + desktop.bounds.Width) - bounds.Width - bounds.X, desktop.bounds.Y + bounds.Y, bounds.Width, bounds.Height);
        //            case DesktopAnchor.BottomLeft:
        //                return new Rectangle(desktop.bounds.X + bounds.X, (desktop.bounds.Y + desktop.bounds.Height) - bounds.Height - bounds.Y, bounds.Width, bounds.Height);
        //            case DesktopAnchor.BottomRight:
        //                break;
        //            case DesktopAnchor.BottomTop:
        //                break;
        //            case DesktopAnchor.BottomBottom:
        //                break;
        //            case DesktopAnchor.Center:
        //                break;
        //            default:
        //                break;
        //        }

        //        return bounds;
        //    }
        //}
    }

    
}
