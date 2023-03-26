using AshTech.UI.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.UI
{
    /// <summary>
    /// Desktop is a collection of UI Widgets
    /// </summary>
    public class Desktop
    {
        private Rectangle bounds;
        private List<UIWidget> widgets;
        internal Game game;

        public Desktop(Rectangle bounds, Game game)
        {
            this.bounds = bounds;
            widgets = new List<UIWidget>();
            this.game = game;
        }

        public void Update(GameTime gameTime)
        {
            foreach (UIWidget widget in widgets)
            {
                widget.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UIWidget widget in widgets)
            {
                widget.Draw(spriteBatch);
            }
        }
    }
}
