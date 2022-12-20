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
using System.Runtime.CompilerServices;
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
        
        internal static void Setup(Game game)
        {
            AssetManager.game = game;
            textures = new Dictionary<string, Texture2D>();
            spriteFonts = new Dictionary<string, SpriteFontBase>();
            fontSystems = new Dictionary<string, FontSystem>();
        }


        public static Texture2D LoadTexture2D(string zipPath, string assetName)
        {
            string assetKey = zipPath + assetName;

            if (textures.ContainsKey(assetKey))
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

        public static SpriteFontBase LoadFont(string zipPath, string assetName, int size)
        {
            string assetKey = zipPath + assetName + size;  //check if the SpriteFontBase already exists
            if (spriteFonts.ContainsKey(assetKey))
                return spriteFonts[assetKey];

            //check if we already have a FontSystem
            string fontSystemAssetKey = zipPath + assetName;
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

        //public static SpriteFont LoadSpriteFont(string assetName)
        //{
        //    string assetKey = assetName;
        //    if (spriteFonts.ContainsKey(assetKey))
        //        return spriteFonts[assetKey];

        //    //Attempt to load from Content Pipeline First
        //    if (File.Exists($"{rootDir}/{assetName}.xnb"))
        //    {
        //        SpriteFont spriteFont = game.Content.Load<SpriteFont>(assetName);
        //        spriteFonts[assetKey] = spriteFont;
        //        return spriteFont;
        //    }

        //    return null;
        //}



    }
}
