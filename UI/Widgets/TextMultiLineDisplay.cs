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
    public class TextMultiLineDisplay : UIWidget
    {
        public TextMultiLineDisplay(string name, Rectangle bounds, DesktopAnchor anchor, Alignment alignment) : base(name, bounds, anchor)
        {
        }
        
        internal override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        internal override void HandleInput(GameTime gameTime, InputManager input)
        {
            base.HandleInput(gameTime, input);
        }

        internal override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
