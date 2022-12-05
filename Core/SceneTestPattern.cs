using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshTech.Core
{
    internal class SceneTestPattern : Scene
    {
        private SpriteFont font;
        private float timeSincePlayerMoved = 0f;
        private float timeSinceNewPos = 0f;
        private Vector2 fontPosition;
        Random random = new Random();

        public override void LoadContent()
        {
            font = Content.Load<SpriteFont>("ashTech/pixellocale");
            Input.AddAction(new InputAction("testUp", "Test Pattern Move Up", Keys.Up, Keys.W ));
            Input.AddAction(new InputAction("testDown", "Test Pattern Move Down", Keys.Down, Keys.S ));
            Input.AddAction(new InputAction("testLeft", "Test Pattern Move Left", Keys.Left, Keys.A ));
            Input.AddAction(new InputAction("testRight", "Test Pattern Move Right", Keys.Right, Keys.D ));

            fontPosition = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Height / 2);
        }

        public override void Update(GameTime gameTime, bool sceneHasFocus)
        {            
            if (timeSincePlayerMoved > 5000)
            {
                timeSinceNewPos += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeSinceNewPos >= 1000)
                {
                    timeSinceNewPos = 0;                        
                    int randomX = random.Next(0, GraphicsDevice.Viewport.Width);
                    int randomY = random.Next(0, GraphicsDevice.Viewport.Height);
                    fontPosition.X = randomX;
                    fontPosition.Y = randomY;
                }
            }            
        }

        public override void HandleInput(GameTime gameTime, bool sceneHasFocus, InputManager input)
        {
            if (sceneHasFocus)
            {
                timeSincePlayerMoved += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (input.IsActionPressed("testUp"))
                {
                    timeSincePlayerMoved = 0;
                    fontPosition.Y += -10;
                }
                if (input.IsActionPressed("testDown"))
                {
                    timeSincePlayerMoved = 0;
                    fontPosition.Y += 10;
                }
                if (input.IsActionPressed("testLeft"))
                {
                    timeSincePlayerMoved = 0;
                    fontPosition.X += -10;
                }
                if (input.IsActionPressed("testRight"))
                {
                    timeSincePlayerMoved = 0;
                    fontPosition.X += 10;
                }
            }
        }

        public override void Draw(GameTime gameTime, bool sceneHasFocus)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();
            if (sceneHasFocus)
            {
                Vector2 stringSize = font.MeasureString("AshTech Engine");
                Vector2 stringOrigin = new Vector2(stringSize.X / 2, stringSize.Y / 2);
                SpriteBatch.DrawString(font, "AshTech Engine!", fontPosition, Color.HotPink, 0, origin: stringOrigin, 1, SpriteEffects.None, 1);
            }
            else
            {
                Vector2 stringSize = font.MeasureString("Scene Lost Focus");
                Vector2 stringOrigin = new Vector2(stringSize.X / 2, stringSize.Y / 2);
                SpriteBatch.DrawString(font, "Scene Lost Focus", fontPosition, Color.OrangeRed, 0, stringOrigin, 1, SpriteEffects.None, 1);
            }
            SpriteBatch.End();
        }

        public override void UnloadContent()
        {
            
        }


    }
}
