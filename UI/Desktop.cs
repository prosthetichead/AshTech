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
        public Rectangle bounds;
        private Dictionary<string,UIWidget> widgets;
        public Game game { get { return _game; } }
        private Game _game;

        public Desktop(Rectangle bounds, Game game)
        {
            this.bounds = bounds;
            widgets = new Dictionary<string, UIWidget>();
            _game = game;
        }

        public void AddWidget(string widgetUnquieName, UIWidget widget)
        {
            widget.desktop = this;
            widgets.Add(widgetUnquieName, widget);
        }

        public void Update(GameTime gameTime)
        {
            foreach (UIWidget widget in widgets.Values)
            {
                widget.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UIWidget widget in widgets.Values)
            {
                widget.Draw(spriteBatch);
            }
        }
    }
}
