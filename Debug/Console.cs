using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using AshTech.Core;
using FontStashSharp;

namespace AshTech.Debug
{
    public class ConsoleCommand
    {
        public string command { get; set; }
        public string description { get; set; }
        public string helpText { get; set; }
        public Action<string[]> commandAction { get; set; }
        public ConsoleCommand(string command, string description, string helpText, Action<string[]> commandAction)
        {
            this.command = command;
            this.description = description;
            this.helpText = helpText;
            this.commandAction = commandAction;
        }
    }
    public enum ConsoleLineType
    {
        normal,
        warning,
        error,
        command
    }

    public static class Console
    {
        private enum ConsoleState
        {
            open,
            closed,
            opening,
            closing,
        }

        private class ConsoleLine
        {
            public string lineText { get; set; }
            public ConsoleLineType lineType { get; set; }
            public DateTime dateTime { get; }

            public ConsoleLine()
            {
                dateTime = DateTime.Now;
            }
            public ConsoleLine(string lineText, ConsoleLineType lineType)
            {
                this.lineText = lineText;
                this.lineType = lineType;
                dateTime = DateTime.Now;
            }

            public Color lineColor
            {
                get
                {
                    switch (lineType)
                    {
                        case ConsoleLineType.normal:
                            return Color.LightGray;
                        case ConsoleLineType.warning:
                            return Color.Yellow;
                        case ConsoleLineType.error:
                            return Color.OrangeRed;
                        case ConsoleLineType.command:
                            return Color.LimeGreen;
                        default:
                            return Color.LightGray;
                    }
                }
            }
        }

        private static SpriteSheet consoleSpriteSheet;
        private static Texture2D consoleTexture;
        private static SpriteFontBase consoleFont;

        private static List<ConsoleLine> consoleLines = new List<ConsoleLine>();
        private static List<ConsoleCommand> consoleCommands = new List<ConsoleCommand>();

        private static string consoleTestLine = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
        private static int textPadding = 10;
        private static int lineHeight = 0;

        private static int animationSpeed = 18;
        private static int timeSinceCursorFlash = 0;
        private static int timeSinceCursorSpeed = 25;
        private static bool displayCursor;
        private static string cursor = "|";
        private static string commandString = "";
        private static List<string> previousCommandStrings = new List<string>();
        private static int previousCommandIndex = 0;

        private static bool startAnimating = false;
        private static ConsoleState consoleState = ConsoleState.closed;

        private static Game game;

        private static bool textInput { get { if (consoleState == ConsoleState.open) return true; else return false; } }

        public static bool displayConsole
        {
            get
            {
                if (consoleState == ConsoleState.closed || startAnimating == true && consoleState == ConsoleState.opening) //extra or rule stops console flashing incorect position
                    return false;
                else
                    return true;
            }
            set
            {
                if (value == false && consoleState == ConsoleState.open)
                {
                    startAnimating = true;
                    consoleState = ConsoleState.closing;
                }
                if (value == true && consoleState == ConsoleState.closed)
                {
                    startAnimating = true;
                    consoleState = ConsoleState.opening;
                }
            }
        }

        private static Rectangle PositionSize = new Rectangle(5, 0, 900, 350);
        private static Rectangle consoleRectangle = new Rectangle(0, 0, 0, 0);

        internal static void Setup(Game game)
        {
            Console.game = game;

            ConsoleLine consoleLine = new ConsoleLine() { lineType = ConsoleLineType.normal, lineText = "The AshTechEngine Console <(^.^)>" };
            consoleLines.Add(consoleLine);
            consoleLine = new ConsoleLine() { lineType = ConsoleLineType.normal, lineText = "== enter ? for list of avalable commands ==" };
            consoleLines.Add(consoleLine);

            //setup listener for text input
            game.Window.TextInput += Window_TextInput;
            game.Window.KeyUp += Window_KeyUp;
            //setup listener for screen resized events
            game.Window.ClientSizeChanged += ScreenResized;

            //setup default commands
            consoleCommands.Add(new ConsoleCommand("?", "This Help Text. [ ? [page number] ] to get additinal pages", "Help! I need somebody. Help! Not just anybody. Help! You know I need someone. Help!", a =>
            {

                int page = 1;
                int limitPerPage = 10;
                int maxPages = (int)Math.Ceiling((double)consoleCommands.Count / limitPerPage);

                if (a.Length > 0)
                {
                    if (int.TryParse(a[0], out page))
                    {
                        page = Math.Clamp(page, 0, maxPages);
                    }
                    else
                    {
                        WriteLine(ConsoleLineType.error, "error parsing page number argument  " + a[0]);
                    }
                }
                WriteLine(" -- Commands Page " + page + " / " + maxPages + " -- ");
                foreach (var command in consoleCommands.Skip((page - 1) * limitPerPage).Take(limitPerPage).ToList())
                {
                    WriteLine("[ " + command.command + " ]  ->  " + command.description);
                }
                WriteLine(" -- for additonal command help enter COMMAND ? -- ");
            }));

            consoleCommands.Add(new ConsoleCommand("clr", "Clear the console window", "Simply clears the console window of all previous lines", a =>
            {
                consoleLines.Clear();
            }));

        }

        internal static void LoadContent()//ContentManager content, Game game)
        {
            //setup position and size
            PositionSize.Width = game.GraphicsDevice.Viewport.Width - 10;
            PositionSize.Height = (int)(game.GraphicsDevice.Viewport.Height * .4f);            

            //sprite sheet
            consoleTexture = AshAssetManager.LoadTexture2D("console/console.png", "ashtech.zip", "ashtech-console-texture");
            consoleSpriteSheet = new SpriteSheet(16, 16, consoleTexture);

            //font
            consoleFont = AshAssetManager.LoadFontSystem("fonts/m6x11.ttf", "ashtech.zip", assetKey: "ashtech-console-font").GetFont(12);    
        }

        private static void ScreenResized(object sender, EventArgs e)
        {
            PositionSize.Width = game.GraphicsDevice.Viewport.Width - 10;
            PositionSize.Height = (int)(game.GraphicsDevice.Viewport.Height * .4f);
        }

        private static void Window_KeyUp(object sender, InputKeyEventArgs e)
        {
            if (textInput)
            {
                var key = e.Key;
                if (key == Keys.Up)
                {
                    if (previousCommandStrings.Count > 0)
                    {
                        previousCommandIndex++;
                        previousCommandIndex = Math.Clamp(previousCommandIndex, 0, previousCommandStrings.Count - 1);
                        string newCommandString = previousCommandStrings[previousCommandIndex];
                        commandString = newCommandString;
                    }
                }
                else if (key == Keys.Down)
                {
                    if (previousCommandStrings.Count > 0)
                    {
                        previousCommandIndex--;
                        previousCommandIndex = Math.Clamp(previousCommandIndex, 0, previousCommandStrings.Count - 1);
                        string newCommandString = previousCommandStrings[previousCommandIndex];
                        commandString = newCommandString;
                    }
                }
            }
        }

        private static void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (textInput)
            {
                char character = e.Character;
                var key = e.Key;
                if (key == Keys.Back)
                {
                    if (commandString.Length > 0)
                        commandString = commandString.Remove(commandString.Length - 1);
                }
                else if (key == Keys.Enter)
                {
                    if (commandString.Length > 0)
                    {
                        previousCommandIndex = -1;
                        WriteLine(ConsoleLineType.command, ">" + commandString);
                        ExecuteCommandString();
                        previousCommandStrings.Insert(0, commandString);
                        commandString = "";

                    }
                }
                else if (key != Keys.OemTilde)
                {
                    commandString += character;
                }
            }
        }

        public static void AddConsoleCommand(ConsoleCommand consoleCommand)
        {
            consoleCommands.Add(consoleCommand);
        }

        private static void ExecuteCommandString()
        {
            var commandArray = commandString.Trim().Split(' ');
            string command = "";
            if (commandArray.Length > 0)
            {
                command = commandArray[0];
                commandArray = commandArray.Skip(1).ToArray();
                if (consoleCommands.Any(w => w.command == command))
                {
                    var consoleCommand = consoleCommands.FirstOrDefault(w => w.command == command);
                    //get the arguments if there is any check if the first one is a ? if it is display the help dont execute
                    if (commandArray.Length > 0)
                    {
                        if (commandArray[0] == "?") { WriteLine(consoleCommand.helpText); return; }
                    }
                    //Execute the Command
                    consoleCommand.commandAction(commandArray);
                }
                else
                {
                    WriteLine(ConsoleLineType.error, "Uknown Command. Enter ? for available commands.");
                }
            }
        }

        public static void WriteLine(string str)
        {
            WriteLine(ConsoleLineType.normal, str);
        }
        public static void WriteLine(ConsoleLineType lineType, string str)
        {
            foreach (string strSplit in str.Split("\n"))
            {

                ConsoleLine consoleLine = new ConsoleLine() { lineType = lineType, lineText = strSplit };
                consoleLines.Add(consoleLine);
            }
        }

        internal static void Update()
        {
            if (startAnimating)
            {
                consoleRectangle = PositionSize;
                if (consoleState == ConsoleState.opening)
                {
                    consoleRectangle.Height = 0;
                }
                startAnimating = false;
            }

            if (consoleState == ConsoleState.opening)
            {
                //grow the console till its the same size as PositionSize
                consoleRectangle.Height += animationSpeed;
                if (consoleRectangle.Height > PositionSize.Height)
                {
                    consoleState = ConsoleState.open;
                    consoleRectangle.Height = PositionSize.Height;
                }

            }
            else if (consoleState == ConsoleState.closing)
            {
                consoleRectangle.Height -= animationSpeed;
                if (consoleRectangle.Height <= 0)
                {
                    consoleState = ConsoleState.closed;
                    consoleRectangle.Height = 0;
                }
            }
            else if(consoleState == ConsoleState.open)
            {
                consoleRectangle = PositionSize;
            }
        }

        internal static void Draw(SpriteBatch spriteBatch)
        {
            

            if (displayConsole)
            {
                timeSinceCursorFlash++;
                if (timeSinceCursorFlash >= timeSinceCursorSpeed)
                {
                    timeSinceCursorFlash = 0;
                    displayCursor = !displayCursor;
                }

                if (lineHeight == 0)
                {
                    var measureString = consoleFont.MeasureString(consoleTestLine);
                    lineHeight = (int)measureString.Y;

                }
                //Rectangle textArea = new Rectangle(consoleRectangle.X + textPadding, consoleRectangle.Y + textPadding, (consoleRectangle.Width - (textPadding*2)), (consoleRectangle.Height - (textPadding * 2)));
                int numberOfLines = MathHelper.Max((consoleRectangle.Height - textPadding * 2) / lineHeight - 1, 0);

                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                //top left corner
                consoleSpriteSheet.Draw(spriteBatch, 0, new Vector2(consoleRectangle.X, consoleRectangle.Y), Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

                //top
                consoleSpriteSheet.Draw(spriteBatch, 1, new Rectangle(consoleRectangle.X + 16, consoleRectangle.Y, consoleRectangle.Width - 32, 16), Color.White, 0f, new Vector2(0, 0),SpriteEffects.None,0f);

                // top right corner                
                consoleSpriteSheet.Draw(spriteBatch, 2, new Vector2(consoleRectangle.X + consoleRectangle.Width, consoleRectangle.Y), Color.White, 0f, new Vector2(16, 0), SpriteEffects.None, 0f);

                //left 
                consoleSpriteSheet.Draw(spriteBatch, 3, new Rectangle(consoleRectangle.X, consoleRectangle.Y + 16, 16, consoleRectangle.Height - 32), Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

                //Center 
                consoleSpriteSheet.Draw(spriteBatch, 4, new Rectangle(consoleRectangle.X + 16, consoleRectangle.Y + 16, consoleRectangle.Width - 32, consoleRectangle.Height - 32), Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

                //right
                consoleSpriteSheet.Draw(spriteBatch, 5, new Rectangle(consoleRectangle.X + consoleRectangle.Width, consoleRectangle.Y + 16, 16, consoleRectangle.Height - 32), Color.White, 0f, new Vector2(16, 0), SpriteEffects.None, 0f);

                //bottom left corner
                consoleSpriteSheet.Draw(spriteBatch, 6, new Vector2(consoleRectangle.X, consoleRectangle.Y + consoleRectangle.Height), Color.White, 0f, new Vector2(0, 16), SpriteEffects.None, 0f);

                //bottom
                consoleSpriteSheet.Draw(spriteBatch, 7, new Rectangle(consoleRectangle.X + 16, consoleRectangle.Y + consoleRectangle.Height, consoleRectangle.Width - 32, 16), Color.White, 0f, new Vector2(0, 16), SpriteEffects.None, 0f);

                //bottom right corner
                consoleSpriteSheet.Draw(spriteBatch, 8, new Vector2(consoleRectangle.X + consoleRectangle.Width, consoleRectangle.Y + consoleRectangle.Height), Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);

                int lineCount = numberOfLines;
                for (int i = consoleLines.Count - 1; i >= 0 && i >= consoleLines.Count - 1 - numberOfLines; i--)
                {  
                    var line = consoleLines[i];

                    spriteBatch.DrawString(consoleFont, line.lineText, new Rectangle(consoleRectangle.X + textPadding, consoleRectangle.Y + lineHeight * lineCount, consoleRectangle.Width - textPadding * 2, lineHeight), Alignment.CenterLeft, new Color[] { line.lineColor });
                    //spriteBatch.DrawString(consoleFont, consoleLines[i].lineText, new Rectangle(consoleRectangle.X + textPadding, consoleRectangle.Y + lineHeight * lineCount, consoleRectangle.Width - textPadding * 2, lineHeight), , line.lineColor);
                    lineCount--;
                }

                spriteBatch.DrawString(consoleFont, ">" + commandString + (displayCursor ? cursor : ""), new Vector2(consoleRectangle.X + textPadding, consoleRectangle.Height - (lineHeight + lineHeight / 2)), new Color[] { Color.LimeGreen });

                spriteBatch.End();
            }
        }

    }
}
