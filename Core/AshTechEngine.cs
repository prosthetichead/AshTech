using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AshTech.Core
{
    public class AshTechEngine : DrawableGameComponent
    {
        
        public InputManager Input { get { return input; } }
        private InputManager input;

        public GraphicsDeviceManager Graphics { get { return _graphics; } }
        private GraphicsDeviceManager _graphics;

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } } 
        private SpriteBatch _spriteBatch;

        private List<Scene> scenes = new List<Scene>();
        private List<Scene> scenesToDraw = new List<Scene>();
        private List<Scene> scenesToUpdate = new List<Scene>();


        public AshTechEngine(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            _graphics = graphics;
            input = new InputManager();

            //Add the default AshTech input actions.
            input.AddAction(new InputAction("AshTechConsoleToggle", "Debug Console", new Keys[] { Keys.OemTilde }) {hiddenAction=true});            
        }

        public void AddScene(Scene scene)
        {
            scene.ashTech = this;

            if (_graphics != null && _graphics.GraphicsDevice != null)
            {
                scene.LoadContent(); //if graphicsDeviceService is running then load content!
            }
            scenes.Add(scene);
        }

        public void RemoveScene(Scene scene)
        {
            scenes.Remove(scene);
            scenesToDraw.Remove(scene);
            scenesToUpdate.Remove(scene);
            scene.UnloadContent();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //load the engine assets
            
        }

        protected override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if(scenes.Count == 0)
            {
                //we have no scenes? then setup the testPattern
                SceneTestPattern testPattern = new SceneTestPattern();
                AddScene(testPattern);
            }


            //update the inputs
            input.Update();

            //add all the scenes in our update list 
            scenesToUpdate.Clear();
            foreach (Scene scene in scenes)
            {
                scenesToUpdate.Add(scene);
            }

            //first screen has focus if the game window has focus
            bool sceneHasFocus = Game.IsActive; 

            while (scenesToUpdate.Count > 0)
            {
                //pop scenes off the list
                Scene scene = scenesToUpdate[scenesToUpdate.Count - 1]; 
                scenesToUpdate.RemoveAt(scenesToUpdate.Count - 1);                

                if(scene.SceneState == SceneState.Active && sceneHasFocus)
                {
                    scene.Update(gameTime, sceneHasFocus);
                    scene.HandleInput(gameTime, sceneHasFocus, input);
                    sceneHasFocus = false; //now no other screen can have focus and run with it as true.
                }
                else
                {
                    scene.Update(gameTime, sceneHasFocus);
                    scene.HandleInput(gameTime, sceneHasFocus, input);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (scenes.Count > 0)
            {
                scenesToDraw.Clear();
                foreach (Scene scene in scenes)
                {
                    scenesToDraw.Add(scene);
                }
                //first screen has focus if the game window has focus
                bool sceneHasFocus = Game.IsActive;
                while (scenesToDraw.Count > 0)
                {
                    //pop scenes off the list
                    Scene scene = scenesToDraw[scenesToDraw.Count - 1];
                    scenesToDraw.RemoveAt(scenesToDraw.Count - 1);

                    if (scene.SceneState == SceneState.Active && sceneHasFocus)
                    {
                        scene.Draw(gameTime, sceneHasFocus);
                        sceneHasFocus = false; //now no other screen can have focus and run with it as true.
                    }
                    else
                    {
                        scene.Draw(gameTime, sceneHasFocus);
                    }
                }
            }
            else //no scenes? so display the engine test pattern 
            {

            }
        }

    }
}
