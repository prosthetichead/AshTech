using AshTech.Core;
using AshTech.Draw;
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
        private SortedDictionary<string,UIWidget> widgets;
        public Game game { get { return _game; } }
        private Game _game;
        private bool _focus = false;

        private SpriteBox backgroundSpriteBox;
        //public SpriteBox BackgroundSpriteBox { get { return backgroundSpriteBox; } set { backgroundSpriteBox = value; } }
        public bool drawBackgroundSpriteBox = false;

        public bool focus { 
            get { return _focus; } 
            set {
                _focus = value;
                if (!_focus)
                {
                    foreach (UIWidget widget in widgets.Values)
                    {
                        widget.focus = false;
                    }
                }
            } 
        }
        public bool visible = true;
        

        public Desktop(Rectangle bounds, Game game)
        {
            this.bounds = bounds;
            widgets = new SortedDictionary<string, UIWidget>();
            _game = game;
        }

        public void SetBackground(Texture2D backgroundTexture, int spriteSize)
        {
            backgroundSpriteBox = new SpriteBox(backgroundTexture, spriteSize, bounds);
            drawBackgroundSpriteBox = true;
        }

        public void SetBackground(SpriteSheet spriteSheet, int topLeftSpriteIndex = 0, int topCenterSpriteIndex = 1, int topRightSpriteIndex = 2,
                                  int centerLeftSpriteIndex = 3, int centerSpriteIndex = 4, int centerRightSpriteIndex = 5,
                                  int bottomLeftSpriteIndex = 6, int bottomCenterSpriteIndex = 7, int bottomRightSpriteIndex = 8)
        {
            backgroundSpriteBox = new SpriteBox(spriteSheet,bounds, topLeftSpriteIndex, topCenterSpriteIndex, topRightSpriteIndex, centerLeftSpriteIndex, centerSpriteIndex,
                                                centerRightSpriteIndex, bottomLeftSpriteIndex, bottomCenterSpriteIndex, bottomRightSpriteIndex);
            drawBackgroundSpriteBox = true;
        }


        public void AddWidget(string widgetName, UIWidget widget)
        {
            widget.desktop = this;
            widgets.Add(widgetName, widget);
            
        }        

        public UIWidget GetWidget(string widgetName)
        {
            if(widgets.ContainsKey(widgetName))
                return widgets.GetValueOrDefault(widgetName);
            else
                return null;
        }

        public void ShowWidget(string widgetName)
        {
            UIWidget widget = GetWidget(widgetName);
            if (widget != null)
            {
                widget.visible = true;
            }
        }
        public void HideWidget(string widgetName)
        {
            UIWidget widget = GetWidget(widgetName);
            if (widget != null)
            {
                widget.visible = false;
            }
        }

        public void FocusWidget(string widgetName)
        {
            UIWidget widget = GetWidget( widgetName);
            if(widget != null)
            {
                widget.focus = true;
            }
        }

        public void UnfocusWidget(string widgetName)
        {
            UIWidget widget = GetWidget(widgetName);
            if (widget != null)
            {
                widget.focus = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (visible)
            { 
                foreach (UIWidget widget in widgets.Values)
                {
                    widget.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                if(drawBackgroundSpriteBox && backgroundSpriteBox != null)
                {
                    backgroundSpriteBox.bounds = bounds;
                    backgroundSpriteBox.Draw(spriteBatch);
                }

                foreach (UIWidget widget in widgets.Values.OrderBy(x=>x.drawOrder) )
                {
                    widget.Draw(spriteBatch);
                }
            }
        }
    }
}
