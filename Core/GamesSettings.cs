using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using AshTech.Debug;
using Console = AshTech.Debug.Console;

namespace AshTech.Core
{
    public class Config
    {
        public int horizontalResolution { get; set; }
        public int verticalResolution { get; set; }
        public bool fullScreen { get; set; }
        public bool allowResize { get; set; }

        public Config()
        {
            horizontalResolution = 1280;
            verticalResolution = 720;
            fullScreen = false;
            allowResize = false;
        }

        public string prettyJSON()
        {
            var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });
            return jsonString;
        }
    }

    public static class GameSettings
    {
        public static Config config;
        public static Config previousConfig;

        private static Game game;
        private static GraphicsDeviceManager graphics;

        public static event EventHandler SettingsChanaged;

        internal static void Setup(Game game, GraphicsDeviceManager graphics)
        {
            GameSettings.game = game;
            GameSettings.graphics = graphics;

            //setup Console commands
            Console.AddConsoleCommand(new ConsoleCommand("config", "show and set the current Game Settings", "displays info about the current configuration saved for the game. \n set config using config [settingName] [value] \n save config using config save", a =>
            {
                if (a.Length == 2)
                {
                    Console.WriteLine(SetConfigFromStrings(a[0], a[1]));
                }
                else if (a.Length == 1)
                {
                    switch (a[0])
                    {
                        case "save":
                            SaveConfig();
                            Console.WriteLine(ConsoleLineType.warning, "Config saved to ashtech.config");
                            break;
                        case "reset":
                            ClearConfig();
                            Console.WriteLine(ConsoleLineType.warning, "Config cleared and defaults saved to ashtech.config");
                            break;
                        default:
                            Console.WriteLine(ConsoleLineType.error, "Unknow Config Option");
                            break;
                    }
                    
                }
                else { Console.WriteLine(config.prettyJSON());}
            }));

            LoadConfig();
        }

        public static void LoadConfig()
        {
            //try load the config file
            if (System.IO.File.Exists("ashtech.config"))
            {
                //file exits so read it and deserialize into the config object
                string configJSON = System.IO.File.ReadAllText("ashtech.config");
                config = JsonConvert.DeserializeObject<Config>(configJSON);
            }
            else
            {
                //no file exists yet.
                config = new Config();
                SaveConfig();
            }
            previousConfig = config;

            ApplyConfig();
        }

        public static void SaveConfig()
        {
            string configJSON = JsonConvert.SerializeObject(config, Formatting.Indented);
            System.IO.File.WriteAllText("ashtech.config", configJSON); //Need If Android handeling
        }

        public static void ClearConfig()
        {
            System.IO.File.Delete("ashtech.config");
            LoadConfig();
        }

        /// <summary>
        /// Apply any settings to the Game 
        /// </summary>
        public static void ApplyConfig()
        {
            graphics.ApplyChanges();
            game.Window.AllowUserResizing = config.allowResize;
            graphics.PreferredBackBufferWidth = config.horizontalResolution;
            graphics.PreferredBackBufferHeight = config.verticalResolution;
            graphics.ApplyChanges();
            graphics.IsFullScreen = config.fullScreen;
            graphics.ApplyChanges();
            previousConfig = config;

            SettingsChanaged?.Invoke(null, EventArgs.Empty);
        }

        public static string SetConfigFromStrings(string name, string value)
        {
            switch (name)
            {
                case "horizontalResolution":
                    if (int.TryParse(value, out int result))
                    {
                        config.horizontalResolution = result;
                        ApplyConfig();
                        return "success";
                    }
                    else
                    {
                        return "error cant convert value to int";
                    }
                case "verticalResolution":
                    if (int.TryParse(value, out int verticalResult))
                    {
                        config.verticalResolution = verticalResult;
                        ApplyConfig();
                        return "success";
                    }
                    else
                    {
                        return "error cant convert value to int";
                    }
                case "fullScreen":
                    if (bool.TryParse(value, out bool fullscreenResult))
                    {
                        config.fullScreen = fullscreenResult;
                        ApplyConfig();
                        return "success";
                    }
                    else
                    {
                        return "error cant convert value to bool";
                    }
                case "allowResize":
                    if (bool.TryParse(value, out bool allowResizeResult))
                    {
                        config.allowResize = allowResizeResult;
                        ApplyConfig();
                        return "success";
                    }
                    else
                    {
                        return "error cant convert value to bool";
                    }
                default:
                    return "config name not found";
            }
        }

    }
}

