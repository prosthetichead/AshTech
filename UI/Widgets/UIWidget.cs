using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.UI.Widgets
{
    public abstract class UIWidget
    {
        private Desktop desktop;
        Rectangle bounds;
        public bool focus;

        public UIWidget(Desktop desktop, Rectangle bounds)
        {
            this.desktop = desktop;
            this.bounds = bounds;
            focus = false;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }

    
}
