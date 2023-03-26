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
    public class TextInput : UIWidget
    {
        private static SpriteFontBase font;

        public bool displayCursor;
        public int cursorFlashSpeed = 25;
        private int timeSinceCursorFlash = 0;
        public char cursor = '|';

        
        public string value { get { return _value; } set { _value = value; ValueChanaged?.Invoke(this, EventArgs.Empty); }  }
        private string _value = "";

        public event EventHandler ValueChanaged;


        public TextInput(Desktop desktop, Rectangle bounds, SpriteFontBase font) : base(desktop, bounds)
        {
            //setup listener for text input
            desktop.game.Window.TextInput += Window_TextInput; ;
            desktop.game.Window.KeyUp += Window_KeyUp; ;
        }

        private void Window_KeyUp(object sender, InputKeyEventArgs e)
        {

        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
           
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
                spriteBatch.DrawString(font, ">" + value + (displayCursor ? cursor : ""), new Vector2(consoleRectangle.X + textPadding, consoleRectangle.Height - (lineHeight + lineHeight / 2)), new Color[] { Color.LimeGreen });
            spriteBatch.End();
        }

    }

}
