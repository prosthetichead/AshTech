﻿using AshTech.Core;
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
    public enum UIWidgetAnchor
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
        public event EventHandler MouseEnteredBounds;
        public event EventHandler MouseExitedBounds;
        public event EventHandler MouseLeftClick;

        internal Rectangle bounds;
        public bool focus;
        public bool visible;
        
        public SpriteBox backgroundSpriteBox;
       
        internal bool mouseInBounds;
        public bool MouseInBounds { get { return mouseInBounds; } }

        public UIWidget(string name, UIElement element, Rectangle bounds, UIWidgetAnchor anchor)
        {
            this.bounds = bounds;
            focus = false;
            visible = true;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void HandleInput(GameTime gameTime, InputManager input);

        public abstract void Draw(SpriteBatch spriteBatch);

        //public Rectangle ScreenBounds
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
