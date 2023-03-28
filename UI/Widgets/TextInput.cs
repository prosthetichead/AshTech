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
        
        public int textPadding = 10;

        public bool displayCursor = true;
        public int cursorFlashSpeed = 25;
        public char cursor = '#';
        public string preText = ">";
        public string postText = "";
        public bool hasFocus = true;

        private int _timeSinceCursorFlash = 0;
        private bool _displayCursor = false;
        


        
        public string value { get { return _value; } set { _value = value; ValueChanaged?.Invoke(this, EventArgs.Empty); }  }
        private string _value = "";

        public event EventHandler ValueChanaged;


        public TextInput(Desktop desktop, Rectangle bounds, SpriteFontBase font) : base(desktop, bounds)
        {
            //setup listener for text input
            desktop.game.Window.TextInput += Window_TextInput;

            this.font = font;
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (hasFocus)
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
                        //fire off a pressed enter event?
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
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle desktopRectangle = desktop.bounds;

            spriteBatch.Begin();
                spriteBatch.DrawString(font, preText + value + (displayCursor ? cursor : "") + postText, new Vector2(desktopRectangle.X + bounds.X + textPadding, desktopRectangle.X + bounds.Y), new Color[] { Color.LimeGreen });
            spriteBatch.End();
        }

    }

}
