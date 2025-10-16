using AshTech.Core;
using AshTech.Debug;
using AshTech.Draw;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AshTech.UI.Widgets
{
    public class TextInput : UIWidget
    {
       
        public Alignment textAlignment;

        public SpriteFontBase font;
        public Color[] fontColor = { Color.Black };

        public int textPadding = 10;
                
        public int cursorFlashSpeed = 350;
        public char cursor = '|';
        public string preText = ">";
        public string postText = "";

        private float timeSinceCursorFlash = 0;
        private bool drawCursor = true;

        public string value { get { return _value; } set { _value = value; ValueChanaged?.Invoke(this, EventArgs.Empty); }  }
        private string _value = "";

        public event EventHandler ValueChanaged;
        public event EventHandler PressedEnter;
        public event EventHandler PressedUp;

        public TextInput(string name, Rectangle bounds, DesktopAnchor anchor, string fontAssetId, int fontSize) : base(name, bounds, anchor)
        {
            font = AssetManager.GetFontSystem(fontAssetId).GetFont(fontSize);            
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (Focus)
            {
                char character = e.Character;
                var key = e.Key;
                if (key == Keys.Back)
                {
                    if (value.Length > 0)
                    {
                        value = value.Remove(value.Length - 1);
                        ValueChanaged?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if (key == Keys.Enter)
                {
                    if (value.Length > 0)
                    {
                        PressedEnter?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if (key == Keys.Up)
                {
                    PressedUp?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    value += character;
                    ValueChanaged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        internal override void DesktopConnected()
        {
            desktop.game.Window.TextInput += Window_TextInput;
        }

        internal override void Update(GameTime gameTime)
        {
            timeSinceCursorFlash += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timeSinceCursorFlash >= cursorFlashSpeed)
            {
                timeSinceCursorFlash = 0;
                drawCursor = !drawCursor;
            }
        }

        internal override void HandleInput(GameTime gameTime, InputManager input)
        {
            if (Focus)
            {

            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Rectangle drawPos = DesktopBounds;
                spriteBatch.DrawString(font, preText + value + (drawCursor ? cursor : "") + postText, rectangle: drawPos, textAlignment, fontColor);           
            }
        }
    }
}
