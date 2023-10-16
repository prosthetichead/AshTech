using AshTech.Core;
using AshTech.Draw;
using AshTech.UI.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.UI
{



    //    /// <summary>
    //    /// Desktop is a collection of UI Widgets
    //    /// </summary>
    public class UIElement
    {
        //        public Rectangle bounds = new Rectangle();
        private Dictionary<string, UIWidget> widgets;
        private Game game;

        public UIElement(Game game)
        {

        }


    }
}
//        private Scene scene;

//        private bool _focus = false;

//        private SpriteBox backgroundSpriteBox;

//        public bool drawBackgroundSpriteBox = false;

//        public bool visible = true;

//        private bool mouseInBounds;
//        public bool MouseInBounds { get { return mouseInBounds; } }

//        public bool focus {
//            get { return _focus; }
//            set {
//                _focus = value;
//                if (!_focus)
//                {
//                    foreach (UIWidget widget in widgets.Values)
//                    {
//                        widget.focus = false;
//                    }
//                }
//            }
//        }

//        public Desktop(Scene scene, Rectangle bounds)
//        {
//            this.scene = scene;
//            this.bounds = bounds;
//            widgets = new Dictionary<string, UIWidget>();            
//        }

//        public void SetBackground(Texture2D backgroundTexture, int spriteSize)
//        {
//            backgroundSpriteBox = new SpriteBox(backgroundTexture, spriteSize, bounds);
//            drawBackgroundSpriteBox = true;
//        }

//        public void SetBackground(SpriteSheet spriteSheet, int topLeftSpriteIndex = 0, int topCenterSpriteIndex = 1, int topRightSpriteIndex = 2,
//                                  int centerLeftSpriteIndex = 3, int centerSpriteIndex = 4, int centerRightSpriteIndex = 5,
//                                  int bottomLeftSpriteIndex = 6, int bottomCenterSpriteIndex = 7, int bottomRightSpriteIndex = 8)
//        {
//            backgroundSpriteBox = new SpriteBox(spriteSheet,bounds, topLeftSpriteIndex, topCenterSpriteIndex, topRightSpriteIndex, centerLeftSpriteIndex, centerSpriteIndex,
//                                                centerRightSpriteIndex, bottomLeftSpriteIndex, bottomCenterSpriteIndex, bottomRightSpriteIndex);
//            drawBackgroundSpriteBox = true;
//        }


//        public void AddWidget(string widgetName, UIWidget widget)
//        {
//            widgets.Add(widgetName, widget);
//        }        

//        public UIWidget GetWidget(string widgetName)
//        {
//            if(widgets.ContainsKey(widgetName))
//                return widgets.GetValueOrDefault(widgetName);
//            else
//                return null;
//        }

//        public void ShowWidget(string widgetName)
//        {
//            UIWidget widget = GetWidget(widgetName);
//            if (widget != null)
//            {
//                widget.visible = true;
//            }
//        }
//        public void HideWidget(string widgetName)
//        {
//            UIWidget widget = GetWidget(widgetName);
//            if (widget != null)
//            {
//                widget.visible = false;
//            }
//        }

//        public void FocusWidget(string widgetName)
//        {
//            UIWidget widget = GetWidget( widgetName);
//            if(widget != null)
//            {
//                widget.focus = true;
//            }
//        }

//        public void UnfocusWidget(string widgetName)
//        {
//            UIWidget widget = GetWidget(widgetName);
//            if (widget != null)
//            {
//                widget.focus = false;
//            }
//        }

//        public override void Update(GameTime gameTime)
//        {
//            if (visible)
//            {
//                foreach (UIWidget widget in widgets.Values)
//                {
//                    widget.Update(gameTime);
//                }                                

//                //mousePosition = input.MousePosition;
//                if (bounds.Contains(input.MousePosition))
//                    mouseInBounds = true;

//                foreach (UIWidget widget in widgets.Values)
//                {
//                    widget.HandleInput(gameTime, input);
//                }
//            }
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            if (visible)
//            {
//                if(drawBackgroundSpriteBox && backgroundSpriteBox != null)
//                {
//                    backgroundSpriteBox.bounds = bounds;
//                    backgroundSpriteBox.Draw(spriteBatch);
//                }

//                foreach (UIWidget widget in widgets.Values.OrderBy(x=>x.drawOrder) )
//                {
//                    widget.Draw(spriteBatch);
//                }
//            }
//        }
//    }
//}
