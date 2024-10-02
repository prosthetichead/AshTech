using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using AshTech.Core;
using FontStashSharp;
using AshTech.UI;
using AshTech.UI.Widgets;
using System.Reflection.Metadata;
using AshTech.Draw;

namespace AshTech.Debug
{
    public class ConsoleCommand
    {
        public string command { get; set; }
        public string description { get; set; }
        public string helpText { get; set; }
        public Action<string[]> commandAction { get; set; }
        public bool secret { get; set; }

        public ConsoleCommand(string command, string description, string helpText, Action<string[]> commandAction, bool secret = false)
        {
            this.command = command;
            this.description = description;
            this.helpText = helpText;
            this.commandAction = commandAction;
            this.secret = secret;
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

        private static List<string> previousCommandStrings = new List<string>();
        private static int previousCommandIndex = 0;

        private static int animationSpeed = 15;
        private static bool startAnimating = false;
        private static ConsoleState consoleState = ConsoleState.closed;

        private static Game game;

        private static Desktop desktop;
        private static TextInput consoleInput;

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

        internal static void Setup(Game game)
        {
            Console.game = game;

            ConsoleLine consoleLine = new ConsoleLine() { lineType = ConsoleLineType.normal, lineText = "The AshTechEngine Console <(^.^)>" };
            consoleLines.Add(consoleLine);
            consoleLine = new ConsoleLine() { lineType = ConsoleLineType.normal, lineText = "== enter ? for list of avalable commands ==" };
            consoleLines.Add(consoleLine);

            //setup ui desktop
            desktop = new Desktop(PositionSize, game);
            
            GameSettings.SettingsChanaged += ScreenResized;

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
                if (page > 0)
                {
                    WriteLine(" -- Commands Page " + page + " / " + maxPages + " -- ");
                    WriteLine(" -- for additonal command help enter COMMAND ? -- ");
                    foreach (var command in consoleCommands.Where(w => w.secret == false).Skip((page - 1) * limitPerPage).Take(limitPerPage).ToList())
                    {
                        WriteLine("[ " + command.command + " ]  ->  " + command.description);
                    }
                    
                }
            }));

            consoleCommands.Add(new ConsoleCommand("clr", "Clear the console window", "Simply clears the console window of all previous lines", a =>
            {
                consoleLines.Clear();
            }));
            consoleCommands.Add(new ConsoleCommand("exit", "exit the game", "exit the game", a => { game.Exit(); }));
        }

        internal static void LoadContent()
        {
            //setup position and size
            PositionSize.Width = game.GraphicsDevice.Viewport.Width - 10;
            PositionSize.Height = (int)(game.GraphicsDevice.Viewport.Height * .4f);            

            //sprite sheet
            consoleTexture = AssetManager.LoadTexture2D("console/console.png", "ashtech.zip", "ashtech-console-texture");
            consoleSpriteSheet = new SpriteSheet(16, 16, consoleTexture);

            //font
            consoleFont = AssetManager.LoadFontSystem("fonts/m6x11.ttf", "ashtech.zip", assetKey: "ashtech-console-font").GetFont(12);

            //add widgets to desktop
            desktop.SetBackground(consoleSpriteSheet);

            desktop.Bounds = PositionSize;
            consoleInput = new TextInput("debugTextBox", new Rectangle(10, 0, 200, 18), DesktopAnchor.BottomLeft);
            desktop.AddWidget(consoleInput);
            consoleInput.PressedEnter += TextInput_PressedEnter;
        }

        private static void TextInput_PressedEnter(object sender, EventArgs e)
        {
            if (textInput)
            {
                UI.Widgets.TextInput textInput = (UI.Widgets.TextInput)sender;

                string commandString = textInput.value;
                previousCommandIndex = -1;
                WriteLine(ConsoleLineType.command, ">" + commandString);
                ExecuteCommandString(commandString);
                previousCommandStrings.Insert(0, commandString);
                consoleInput.value = "";
            }
        }

        private static void ScreenResized(object sender, EventArgs e)
        {
            PositionSize.Width = game.GraphicsDevice.Viewport.Width - 10;
            PositionSize.Height = (int)(game.GraphicsDevice.Viewport.Height * .4f);

            desktop.Bounds = PositionSize;
        }

        public static void AddConsoleCommand(ConsoleCommand consoleCommand)
        {
            consoleCommands.Add(consoleCommand);
        }

        private static void ExecuteCommandString(string commandString)
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

        internal static void Update(GameTime gameTime)
        {
            Rectangle consoleRectangle = desktop.Bounds;
            int hiddenY = 0 - (PositionSize.Height + 10);

            if (startAnimating)
            {
                consoleRectangle = PositionSize;
                desktop.Focus = false;
                consoleRectangle.Y = consoleState == ConsoleState.opening ? hiddenY : PositionSize.Y;
                startAnimating = false;
                consoleInput.value = "";
            }

            switch (consoleState)
            {
                case ConsoleState.opening:
                    consoleRectangle.Y = Math.Min(PositionSize.Y, consoleRectangle.Y + animationSpeed);
                    if (consoleRectangle.Y == PositionSize.Y)
                    {
                        consoleState = ConsoleState.open;
                        desktop.FocusWidget("debugTextBox");
                    }
                    break;

                case ConsoleState.closing:
                    consoleRectangle.Y = Math.Max(hiddenY, consoleRectangle.Y - animationSpeed);
                    if (consoleRectangle.Y == hiddenY)
                    {
                        consoleState = ConsoleState.closed;
                        desktop.Focus = false;
                    }
                    break;

                case ConsoleState.open:
                    consoleRectangle = PositionSize;
                    desktop.Update(gameTime);
                    break;
            }

            desktop.Bounds = consoleRectangle;
        }

        internal static void HandleInput(GameTime gameTime, InputManager input)
        {

        }


        internal static void Draw(SpriteBatch spriteBatch)
        {
            if (displayConsole)
            {
                desktop.visible = true;
                Rectangle consoleRectangle = desktop.Bounds;

                if (lineHeight == 0)
                {
                    var measureString = consoleFont.MeasureString(consoleTestLine);
                    lineHeight = (int)measureString.Y;

                }
                //Rectangle textArea = new Rectangle(consoleRectangle.X + textPadding, consoleRectangle.Y + textPadding, (consoleRectangle.Width - (textPadding*2)), (consoleRectangle.Height - (textPadding * 2)));
                int numberOfLines = MathHelper.Max((consoleRectangle.Height - textPadding * 2) / lineHeight - 1, 0);

                spriteBatch.Begin(samplerState: SamplerState.PointClamp);

                desktop.Draw(spriteBatch);

                int lineCount = numberOfLines;
                for (int i = consoleLines.Count - 1; i >= 0 && i >= consoleLines.Count - 1 - numberOfLines; i--)
                {  
                    var line = consoleLines[i];
                    spriteBatch.DrawString(consoleFont, line.lineText, new Rectangle(consoleRectangle.X + textPadding, consoleRectangle.Y + lineHeight * lineCount, consoleRectangle.Width - textPadding * 2, lineHeight), Alignment.CenterLeft, new Color[] { line.lineColor });
                    lineCount--;
                }                

                spriteBatch.End();
            }

            
        }

    }
}
