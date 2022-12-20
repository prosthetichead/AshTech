using AshTech.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = AshTech.Debug.Console;

namespace AshTech.Core
{
    public class AnimatedSprite
    {
        public class SpriteState
        {
            public string name;
            public List<SpriteFrame> frames = new List<SpriteFrame>();
            public int currentFrameIndex = 0;
            public SpriteFrame currentFrame;
            public float millisecondsOnFrame;

            public SpriteState(string name, float milliseconds, params int[] frameNumbers)
            {
                this.name = name;
                frames = new List<SpriteFrame>();
                foreach(int frameNumber in frameNumbers)
                {
                    SpriteFrame spriteFrame = new SpriteFrame(frameNumber, milliseconds);
                    frames.Add(spriteFrame);
                }

                currentFrameIndex = 0;
                currentFrame = frames[0];
            }
            public SpriteState(string name, List<SpriteFrame> frames)
            {
                this.name = name;
                this.frames = frames;

                currentFrameIndex = 0;
                currentFrame = frames[0];
            }

            public void NextFrame()
            {
                currentFrameIndex++;
                if(currentFrameIndex >= frames.Count)
                {
                    currentFrameIndex = 0;
                }
                currentFrame = frames[currentFrameIndex];
                millisecondsOnFrame = 0;
            }

            public void ResetState()
            {
                currentFrameIndex = 0;
                currentFrame = frames[0];
                millisecondsOnFrame = 0;
            }
        }

        public class SpriteFrame
        {
            public int frame;
            public float milliseconds;

            public SpriteFrame(int frame, float milliseconds = 100f)
            {
                this.frame = frame;
                this.milliseconds = milliseconds;
            }
        }

        private SpriteSheet spriteSheet;
        private Dictionary<string, SpriteState> states;
        private SpriteState currentSpriteState;

        public string CurrentState { get { return currentSpriteState != null ? currentSpriteState.name : "null"; } }
        
        public AnimatedSprite(SpriteSheet spriteSheet)
        {
            this.spriteSheet = spriteSheet;
            states = new Dictionary<string, SpriteState>();
        }

        public AnimatedSprite(int singleSpriteWidth, int singleSpriteHeight, Texture2D texture)
        {
            spriteSheet = new SpriteSheet(singleSpriteWidth, singleSpriteHeight, texture);
            states = new Dictionary<string, SpriteState>();
        }

        public void AddState(SpriteState spriteState)
        {
            states.Add(spriteState.name, spriteState); 
        }

        public void AddState(string name, float milliseconds, params int[] frameNumbers)
        {
            SpriteState spriteState = new SpriteState(name, milliseconds, frameNumbers);
            AddState(spriteState);
        }

        public void SetState(string name, bool resetState = true)
        {
            if (states.ContainsKey(name))
            {
                currentSpriteState = states[name];

                if (resetState)
                {
                    currentSpriteState.ResetState();
                }
            }
            else
            {
                Console.WriteLine(ConsoleLineType.error, "Sprite Animation State Not Found: Name == " + name);
            }
        }

        public void Update(GameTime gameTime)
        {
            if(currentSpriteState != null)
            {
                currentSpriteState.millisecondsOnFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(currentSpriteState.millisecondsOnFrame >= currentSpriteState.currentFrame.milliseconds)
                {
                    currentSpriteState.NextFrame();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffect, float depth)
        {            
            if(currentSpriteState != null)
                spriteSheet.Draw(spriteBatch, currentSpriteState.currentFrame.frame,  position, color, rotation, origin, spriteEffect, depth);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rectangle, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffect, float depth)
        {
            if (currentSpriteState != null)
                spriteSheet.Draw(spriteBatch, currentSpriteState.currentFrame.frame, rectangle, color, rotation, origin, spriteEffect, depth);
        }

    }
}
