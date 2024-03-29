﻿using System;
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
using System.Drawing;
using System.Xml.Linq;

namespace AshTech.Core
{
    public static class AssetManager
    {
        
        private static Dictionary<string, Texture2D> textures;
        private static Dictionary<string, FontSystem> fonts;
        private static Dictionary<string, string> strings;

        private static Game game;

        //public static List<string> SpriteFontBaseAssetKeys { get { return spriteFonts.Keys.ToList(); } }
        //public static List<string> Texture2DAssetKeys { get { return textures.Keys.ToList(); } }

        internal static void Setup(Game game)
        {
            AssetManager.game = game;
            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, FontSystem>();
            strings = new Dictionary<string, string>();

            Console.AddConsoleCommand(new ConsoleCommand("assets",
                "List asset keys for the provided asset type (textures, fonts, strings)",
                "Displays a list of all keys for assets currently in memmery. [assets [textures|fonts|strings]]", a =>
            {                
                string assetType = "";
                if (a!=null && a.Count() > 0)
                {
                    assetType = a[0].ToLower();
                }
                List<string> assetKeys = new List<string>();

                switch (assetType)
                {
                    case "textures":
                        assetKeys = textures.Keys.ToList();
                        break;
                    case "fonts":
                        assetKeys = fonts.Keys.ToList();
                        break;
                    case "strings":
                        assetKeys = strings.Keys.ToList();
                        break;
                    default:
                        assetKeys = new string[]{ "Please Provide a Asset Type Parameter", "assets textures", "assets fonts", "assets strings"}.ToList();
                        break;
                }

                foreach(string key in assetKeys)
                {
                    Console.WriteLine(key);
                }         
            }));
        }

        private static Stream LoadStream(string zipPath, string assetName)
        {
            if (zipPath != null)
            {
                //load stream from the zip file.
                if (File.Exists(zipPath))
                {
                    ZipArchive zipArchive = ZipFile.OpenRead(zipPath);
                    if(zipArchive.Entries.Any(w => w.FullName.Equals(assetName)))
                    {
                        var entry = zipArchive.Entries.FirstOrDefault(w=>w.FullName.Equals(assetName));
                        return entry.Open();
                    }
                }
            }
            else
            {
                return File.OpenRead(assetName);
            }
            return null;
        }

        public static string LoadString(string assetName, string zipPath = null, string assetKey = null, bool storeAsset = true, bool overwrite = false)
        {
            if (assetKey == null)
            {
                assetKey = zipPath == null ? assetName : zipPath + "/" + assetName;
            }
            //do we already have a asset loaded with this name?
            if (!overwrite && textures.ContainsKey(assetKey))
                return strings[assetKey];

            var stream = LoadStream(zipPath, assetName);
            StreamReader sr = new StreamReader(stream);
            var data = sr.ReadToEnd();
            if(storeAsset)
            {
                strings[assetKey] = data;
            }
            return data;
        }

        public static Texture2D LoadTexture2D(string assetName, string zipPath = null, string assetKey = null, bool storeAsset = true, bool overwrite = false)
        {
            if (assetKey == null)
            {
                assetKey = zipPath == null ? assetName : zipPath + "/" + assetName;
            }
            //do we already have a asset loaded with this name?
            if (!overwrite && textures.ContainsKey(assetKey))
                return textures[assetKey];

            var stream = LoadStream(zipPath, assetName);
            Texture2D texture = Texture2D.FromStream(game.GraphicsDevice, stream);
            if (storeAsset)
            {
                textures[assetKey] = texture;
            }
            return texture;
        }

        public static Texture2D GetTexture2D(string assetKey)
        {
            if (textures.ContainsKey(assetKey))            
                return textures[assetKey];            
            else
                return null;    
        }
        public static SpriteFontBase GetSpriteFontBase(string assetKey, float size)
        {
            if (fonts.ContainsKey(assetKey))            
                return fonts[assetKey].GetFont(size);            
            else
                return null;
        }
        public static FontSystem GetFontSystem(string assetKey)
        {
            if (fonts.ContainsKey(assetKey))
                return fonts[assetKey];
            else
                return null;
        }

        public static FontSystem LoadFontSystem(string assetName, string zipPath = null, string assetKey = null, FontSystemSettings fontSystemSettings = null, bool storeAsset = true, bool overwrite = false)
        {
            if (assetKey == null)
            {
                assetKey = zipPath == null ? assetName : zipPath + "/" + assetName;
            }

            //do we already have a asset loaded with this name?
            if (!overwrite && fonts.ContainsKey(assetKey))
                return fonts[assetKey];

            if (fontSystemSettings == null)
                fontSystemSettings = new FontSystemSettings();

            FontSystem fontSystem = new FontSystem(fontSystemSettings);
            fontSystem.AddFont(LoadStream(zipPath, assetName));
            if (storeAsset) { 
                    fonts.Add(assetKey, fontSystem);    
            }
            return fontSystem;
        }
    }
}
