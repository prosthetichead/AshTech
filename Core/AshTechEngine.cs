using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using AshTech.Debug;
using FontStashSharp;
using static System.Formats.Asn1.AsnWriter;

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

        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();        
        public string ActiveSceneName { get { return _activeSceneName; } }
        private string _activeSceneName = "";
        private Scene _activeScene;


        private SpriteFontBase font;


        public AshTechEngine(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            _graphics = graphics;            

            Console.Setup(game);
            GameSettings.Setup(game, graphics);            
            AssetManager.Setup(game);

            _input = new InputManager();

        }

        public void AddScene(string sceneUniqueName, Scene scene)
        {
            scene.ashTech = this;
            scenes.Add(sceneUniqueName, scene);
        }

        public void RemoveScene(string sceneUniqueName)
        {
            scenes.TryGetValue(sceneUniqueName, out Scene scene);
            if (scene != null)
            {
                scene.UnloadContent();
                scenes.Remove(sceneUniqueName);
            }
        }

        public void ActivateScene(string sceneUniqueName)
        {
            scenes.TryGetValue(sceneUniqueName, out Scene scene);
            if (scene != null)
            {
                if(_activeScene != null)
                    _activeScene.UnloadContent();

                if (_graphics != null && _graphics.GraphicsDevice != null)
                {
                    scene.LoadContent(); //if graphicsDeviceService is running then load the scenes content!
                    //this is where we could put in a threaded load and a loading scene, until its finished.
                }
                _activeScene = scene;
                _activeSceneName = sceneUniqueName;
            }
            else
            {
                Console.WriteLine(ConsoleLineType.error, "No Scene with name " + sceneUniqueName + " exists");
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //load the engine assets
            AssetManager.LoadAssetPack("ashtech.zip", "ashtech");

            //setup console assets
            Console.LoadContent();

            //setup action for console
            Input.AddAction(new InputAction("AshTechConsoleToggle", "Show Debug Console", [Keys.OemTilde]) { hiddenAction = true });
            Input.AddAction(new InputAction("AshTechConsolePreviousCmd", "Debug Console Previous Command", [Keys.Up]) { hiddenAction = true });


            //load the font to write engine info using
            font = AssetManager.GetFontSystem("ashtech/fonts/m6x11.ttf").GetFont(16);

            //setup Console Commands
            Console.AddConsoleCommand(new ConsoleCommand("scenes", "List scenes and details about each scene", "", a =>
            {
                foreach(string sceneName in scenes.Keys)
                {
                    Console.WriteLine(sceneName + (sceneName == _activeSceneName ? " - Active" : ""));
                }
                if (_activeScene == null)
                    Console.WriteLine(ConsoleLineType.error,  "No Scene is Active");
            }));
            Console.AddConsoleCommand(new ConsoleCommand("activateScene", "Activate the supplied scene name", "", a =>
            {
                ActivateScene(a[0]);
            }));
        }

        protected override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            //update the inputs
            Input.Update();

            //update the console
            Console.Update(gameTime);                        
            //check if the conosle button is pressed
            if (Input.IsActionTriggered("AshTechConsoleToggle"))
            {
                Console.displayConsole = !Console.displayConsole;
            }
         
            if(_activeScene != null)
            {
                //scene has focus if the game window has focus and console isnt open
                bool sceneHasFocus = Game.IsActive == true && Console.displayConsole == false;
                _activeScene.Update(gameTime, sceneHasFocus);
                _activeScene.HandleInput(gameTime, sceneHasFocus, Input);
            }            
        }

        public override void Draw(GameTime gameTime)
        {
            if (_activeScene != null)
            {

                //scene has focus if the game window has focus and console isnt open
                bool sceneHasFocus = Game.IsActive == true && Console.displayConsole == false;

                _activeScene.Draw(gameTime, sceneHasFocus);
                    
                
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
