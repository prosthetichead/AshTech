using AshTech.Core;
using AshTech.Debug;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.UI.Widgets
{
    public class TextInput : UIWidget
    {
        private SpriteFontBase font;
        public Alignment alignment;

        public int textPadding = 10;

        public bool displayCursor = true;
        public int cursorFlashSpeed = 350;
        public char cursor = '|';
        public string preText = ">";
        public string postText = "";

        private float _timeSinceCursorFlash = 0;
        private bool _displayCursor = false;
        


        
        public string value { get { return _value; } set { _value = value; ValueChanaged?.Invoke(this, EventArgs.Empty); }  }
        private string _value = "";

        public event EventHandler ValueChanaged;
        public event EventHandler PressedEnter;


        public TextInput(Desktop desktop, Rectangle bounds, DesktopAnchor anchor, SpriteFontBase font, Alignment alignment) : base(desktop, bounds, anchor)
        {
            //setup listener for text input
            desktop.game.Window.TextInput += Window_TextInput;
            this.alignment = alignment;
            this.font = font;
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (focus)
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
                else
                {
                    value += character;
                    ValueChanaged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            _timeSinceCursorFlash += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_timeSinceCursorFlash >= cursorFlashSpeed)
            {
                _timeSinceCursorFlash = 0;
                displayCursor = !displayCursor;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                Rectangle drawPos = DrawPosition();
                spriteBatch.DrawString(font, preText + value + (displayCursor ? cursor : "") + postText, rectangle: drawPos, alignment, new Color[] { Color.LimeGreen });           
            }
        }
    }
}
