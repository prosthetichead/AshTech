using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using AshTech.Debug;
using FontStashSharp;

namespace AshTech.Core
{
    public class AshTechEngine : DrawableGameComponent
    {
        
        public InputManager Input { get { return _input; } }
        private InputManager _input;

        public GraphicsDeviceManager Graphics { get { return _graphics; } }
        private GraphicsDeviceManager _graphics;

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } } 
        private SpriteBatch _spriteBatch;

        private List<Scene> scenes = new List<Scene>();
        private List<Scene> scenesToDraw = new List<Scene>();
        private List<Scene> scenesToUpdate = new List<Scene>();

        private SpriteFontBase font;


        public AshTechEngine(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            _graphics = graphics;
            _input = new InputManager();

            Console.Setup(game);
            AssetManager.Setup(game);
            
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
            Console.LoadContent();//Game.Content, Game);
            Input.AddAction(new InputAction("AshTechConsoleToggle", "Debug Console", new Keys[] { Keys.OemTilde }) { hiddenAction = true });

            //load the font to write engine info using
            font = AssetManager.LoadFont("ashtech.zip", "fonts/m6x11.ttf", 24);
        }

        protected override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            //update the inputs
            Input.Update();

            //update the console
            Console.Update();
                        
            //check if the conosle button is pressed
            if (Input.IsActionTriggered("AshTechConsoleToggle"))
            {
                System.Diagnostics.Debug.WriteLine("console");
                Console.displayConsole = !Console.displayConsole;
            }
         
            //add all the scenes in our update list 
            scenesToUpdate.Clear();
            foreach (Scene scene in scenes)
            {
                scenesToUpdate.Add(scene);
            }

            //first screen has focus if the game window has focus and console isnt open
            bool sceneHasFocus = Game.IsActive == true && Console.displayConsole == false; 

            while (scenesToUpdate.Count > 0)
            {
                //pop scenes off the list
                Scene scene = scenesToUpdate[scenesToUpdate.Count - 1]; 
                scenesToUpdate.RemoveAt(scenesToUpdate.Count - 1);                

                if(scene.SceneState == SceneState.Active && sceneHasFocus)
                {
                    scene.Update(gameTime, sceneHasFocus);
                    scene.HandleInput(gameTime, sceneHasFocus, Input);
                    sceneHasFocus = false; //now no other screen can have focus and run with it as true.
                }
                else
                {
                    scene.Update(gameTime, sceneHasFocus);
                    scene.HandleInput(gameTime, sceneHasFocus, Input);
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
                bool sceneHasFocus = Game.IsActive == true && Console.displayConsole == false;

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
            else
            {
                //no scenes display a message 
                GraphicsDevice.Clear(Color.Black);
                SpriteBatch.Begin();
                SpriteBatch.DrawString(font, "Error no active scene.\nPress ~ to open the Console", new Vector2(30, 30), colors: new Color[] { Color.MonoGameOrange });
                SpriteBatch.End();  

            }

            Console.Draw(SpriteBatch);
        }

    }
}
