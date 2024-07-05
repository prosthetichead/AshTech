using AshTech.Core;
using AshTech.Draw;
using FontStashSharp;
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
        //public Desktop Desktop { get {  return Desktop; } }
        protected Desktop desktop;
        protected Rectangle bounds;
        
        public SpriteFontBase Font { get {  return font; } set { font = value; } }
        private SpriteFontBase font;

        public Alignment TextAlignment { get { return textAlignment; } set { textAlignment = value; } }
        private Alignment textAlignment;        

        public bool Focus { get { return desktop.Focus ? focus : false; } set { focus = value; } } 
        private bool focus;

        public bool Visible { get { return desktop.visible ? visible : false; } set { visible = value; } }    
        private bool visible;

        public int drawOrder = 0;

        public DesktopAnchor anchor;

        public string Name { get { return name; } }
        private string name;
        
        public SpriteBox BackgroundSpriteBox { get { return backgroundSpriteBox; } }
        private SpriteBox backgroundSpriteBox;
        public bool drawBackgroundSpriteBox = false;

        public bool MouseInBounds { get { return Visible ? mouseInBounds : false; } }
        private bool mouseInBounds;
        private bool mouseInBoundsTest = false;
        private bool mouseOutBoundsTest = false;

        public event EventHandler MouseEnteredBounds;
        public event EventHandler MouseExitedBounds;
        public event EventHandler MouseLeftClick;

        public UIWidget(string name, Rectangle bounds, DesktopAnchor anchor)
        {
            font = AssetManager.LoadFontSystem("fonts/m6x11.ttf", "ashtech.zip", assetKey: "ashtech-console-font").GetFont(12);
            textAlignment = Alignment.CenterLeft;

            this.bounds = bounds;
            focus = false;
            visible = true;
            this.anchor = anchor;
            this.name = name;

            MouseEnteredBounds += (obj, args) => { Debug.Console.WriteLine("Mouse Entered Bounds of UI Widget " + name); };
            MouseExitedBounds += (obj, args) => { Debug.Console.WriteLine("Mouse Exited Bounds of UI Widget " + name); };

        }

        public void SetBackground(Texture2D backgroundTexture, int spriteSize)
        {
            backgroundSpriteBox = new SpriteBox(backgroundTexture, spriteSize, bounds);
            drawBackgroundSpriteBox = true;
        }

        internal void ConnectDesktop(Desktop desktop)
        {
            this.desktop = desktop;
            DesktopConnected();
        }

        internal virtual void DesktopConnected()
        {

        }

        internal abstract void Update(GameTime gameTime);

        internal virtual void HandleInput(GameTime gameTime, InputManager input)
        {
            //Check if mouse is In or Out bounds and fire the event
            if (Visible)
            {
                if (DesktopBounds.Contains(input.MousePosition))
                    mouseInBounds = true;
                else
                    mouseInBounds = false;

                if (MouseInBounds && !mouseInBoundsTest)
                {
                    mouseInBoundsTest = true;
                    mouseOutBoundsTest = false;
                    MouseEnteredBounds?.Invoke(this, EventArgs.Empty);
                }
                else if (!MouseInBounds && mouseInBoundsTest && !mouseOutBoundsTest)
                {
                    mouseInBoundsTest = false;
                    mouseOutBoundsTest = true;
                    MouseExitedBounds?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                mouseInBounds = false;
                mouseInBoundsTest = false;
                mouseOutBoundsTest = false;
            }
        }

        internal abstract void Draw(SpriteBatch spriteBatch);

        internal Rectangle DesktopBounds
        {
            get
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

    
}
