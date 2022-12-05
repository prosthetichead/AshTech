using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static System.TimeZoneInfo;

namespace AshTech.Core
{
    public enum SceneState
    {
        Active,
        Hidden,
    }

    public abstract class Scene
    {        
        internal AshTechEngine ashTech;

        public SpriteBatch SpriteBatch { get { return ashTech.SpriteBatch; } }
        public ContentManager Content { get { return ashTech.Game.Content;  } }
        public GraphicsDeviceManager Graphics { get { return ashTech.Graphics; } }
        public GraphicsDevice GraphicsDevice { get { return ashTech.GraphicsDevice; } }
        public InputManager Input { get { return ashTech.Input; } }


        public SceneState SceneState { get { return sceneState; } protected set { sceneState = value; } }
        private SceneState sceneState = SceneState.Active;
        
        

        public abstract void LoadContent();
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime, bool sceneHasFocus);
        public abstract void HandleInput(GameTime gameTime, bool sceneHasFocus, InputManager input);
        public abstract void Draw(GameTime gameTime, bool sceneHasFocus);

    }
}
