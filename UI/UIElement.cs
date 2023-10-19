using AshTech.Core;
using AshTech.Draw;
using AshTech.UI.Widgets;
using FontStashSharp;
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
    public enum UIAnchor
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        BottomTop,
        BottomBottom,
        Center,
    }

    public class UIElement
    {
        public Rectangle Bounds { get { return bounds; } set { bounds = value; } }
        private Rectangle bounds;

        public bool Focus { 
            set {
                focus = value;
                if (!focus)
                {
                    UnfocusAllWidgets();
                }
            } 
            get { return focus; } }
        private bool focus;
        public bool Visible { set { visible = value; } get { return visible; } }
        private bool visible;

        private Dictionary<string, UIWidget> widgets;
        private Scene scene;

        public UIElement(Scene scene, Rectangle bounds)
        {
            this.bounds = bounds;
            this.scene = scene;
            
            focus = false;
            visible = false;

            widgets = new Dictionary<string, UIWidget>();
        }

        public void AddWidget(UIWidget widget)
        {
            widgets.Add(widget.Name, widget);
        }

        public UIWidget GetWidget(string widgetName)
        {
            if (widgets.ContainsKey(widgetName))
                return widgets.GetValueOrDefault(widgetName);
            else
                throw new ArgumentException("Widget not found with name " + widgetName);
        }

        public void UnfocusAllWidgets()
        {
            foreach (UIWidget widget in widgets.Values)
            {
                widget.Focus = false;
            }
        }

        public void FocusWidget(string widgetName)
        {    
            try
            {
                UIWidget widget = GetWidget(widgetName);
                FocusWidget(widget);
            }
            catch (Exception)
            {
                throw;
            }      
        }

        public void FocusWidget(UIWidget widget)
        {
            UnfocusAllWidgets();
            widget.Focus = true;
            focus = true;
        }


    }
}


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
