using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AshTech.Debug;
using System.IO;
using Console = AshTech.Debug.Console;
using FontStashSharp;

namespace AshTech.Core
{
    public static class AssetManager
    {

        private static Dictionary<string, Texture2D> textures;
        private static Dictionary<string, SpriteFontBase> spriteFonts;
        private static Dictionary<string, FontSystem> fontSystems;
        private static Dictionary<string, string> strings;
                
        private static Game game;
        
        public static List<string> SpriteFontBaseAssetKeys { get { return spriteFonts.Keys.ToList(); } }

        internal static void Setup(Game game)
        {
            AssetManager.game = game;
            textures = new Dictionary<string, Texture2D>();
            spriteFonts = new Dictionary<string, SpriteFontBase>();
            fontSystems = new Dictionary<string, FontSystem>();


            Console.AddConsoleCommand(new ConsoleCommand("assets", "List asset keys for the provided asset type", "", a =>
            {
                if (a!=null && a.Count() > 0)
                {
                    string assetType = a[0].ToLower();
                    List<string> assetKeys = new List<string>();
                    switch (assetType)
                    {
                        case "texture":
                            assetKeys = textures.Keys.ToList();
                            break;
                        case "spritefont":
                            assetKeys = spriteFonts.Keys.ToList();
                            break;
                        case "fontsystem":
                            assetKeys = fontSystems.Keys.ToList();
                            break;
                        default:
                            assetKeys = new string[]{ "Please Provide a Asset Type Parameter", "texture", "spritefont", "fontsystem"}.ToList();
                            break;
                    }

                    foreach(string key in assetKeys)
                    {
                        Console.WriteLine(key);
                    }
                }
            
            }));
        }


        public static Texture2D LoadTexture2D(string zipPath, string assetName, string assetKey = null, bool overwrite = false)
        {
            if(assetKey == null)
                assetKey = zipPath + "/" + assetName;

            if (!overwrite && textures.ContainsKey(assetKey))
                return textures[assetKey];

            if (File.Exists(zipPath))
            {
                ZipArchive zipArchive = ZipFile.OpenRead(zipPath);
                foreach(var entry in zipArchive.Entries)
                {
                    if (entry.FullName.Equals(assetName))
                    {
                        Texture2D texture = Texture2D.FromStream(game.GraphicsDevice, entry.Open());
                        textures[assetKey] = texture;
                        return texture;
                    }
                }
            }

            return null;
        }

        public static SpriteFontBase GetSpriteFontBase(string assetKey)
        {
            if (spriteFonts.ContainsKey(assetKey))
            {
                return spriteFonts[assetKey];
            }
            else
            {
                return null;
            }
        }

        public static SpriteFontBase LoadSpriteFontBase(string zipPath, string assetName, int size, string fontSystemAssetKey = null, string assetKey = null, bool overwrite = false)
        {
            if (assetKey == null)
                assetKey = zipPath + "/" + assetName + "-" + size; 

            if (!overwrite && spriteFonts.ContainsKey(assetKey))
                return spriteFonts[assetKey];

            if(fontSystemAssetKey == null)
                fontSystemAssetKey = zipPath + assetName;
            FontSystem fontSystem = new FontSystem();
            if (fontSystems.ContainsKey(fontSystemAssetKey))
            {
                fontSystem = fontSystems[fontSystemAssetKey];
            }
            else
            {
                if (File.Exists(zipPath))
                {
                    ZipArchive zipArchive = ZipFile.OpenRead(zipPath);
                    foreach (var entry in zipArchive.Entries)
                    {
                        if (entry.FullName.Equals(assetName))
                        {
                            
                            fontSystem.AddFont(entry.Open());
                            fontSystems.Add(fontSystemAssetKey, fontSystem);
                            break;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }

            SpriteFontBase spriteFont = fontSystem.GetFont(size);
            spriteFonts.Add(assetKey, spriteFont);
            return spriteFont;
        }
    }
}
