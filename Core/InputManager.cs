﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using AshTech.Debug;
using Console = AshTech.Debug.Console;

namespace AshTech.Core
{
    public class InputManager
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private GamePadState currentGamePadState;
        private GamePadState previousGamePadState;
        private const float analogLimit = 0.5f;  /// The value of an analog control that reads as a "pressed button"

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private Point currentMousePosition;
        private Point previousMousePosition;

        public Point MousePosition {get{return currentMousePosition; }}


        public Dictionary<string, InputAction> inputActions;

        public InputManager()
        {
            inputActions = new Dictionary<string, InputAction>();

            Console.AddConsoleCommand(new ConsoleCommand("inputs", "display inputs and actions", "", a =>
            {

            }));
        }        

        ///<summary>
        ///Updates the keyboard and gamepad control states.
        ///Dont run this Game Screen Manager runs it for you
        ///</summary>
        internal void Update()
        {
            previousKeyboardState = currentKeyboardState;
            previousGamePadState = currentGamePadState;

            //update the keyboard state
            currentKeyboardState = Keyboard.GetState();
            //update the gamepad state
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            //update mouse position
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            previousMousePosition = currentMousePosition;
            currentMousePosition = currentMouseState.Position;
        }

        /// <summary>
        /// Designed to be called in a update loop until True is returned. 
        /// if false is returned no input has been fired yet and this function should continue to be called.
        /// if true is returned a input has been fired and the action has now been updated with the new input.
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public bool SetActionFromInput(string actionId)
        {
            var action = inputActions[actionId];

            if (currentKeyboardState.GetPressedKeyCount() > 0)
            {
                action.gamePadButtons.Clear();
                action.keyboardKeys.Clear();
                action.mouseButtons.Clear();

                action.keyboardKeys.Add(currentKeyboardState.GetPressedKeys()[0]);
                return true;
            }
            return false;
        }

        //pressed action checks
        public bool IsActionPressed(string actionId)
        {
            if (inputActions.TryGetValue(actionId, out InputAction inputAction))
            {
                return IsInputActionPressed(inputAction);
            }
            return false;
        }

        private bool IsInputActionPressed(InputAction inputAction)
        {
            //check keyboard
            for (int i = 0; i < inputAction.keyboardKeys.Count; i++)
            {
                if (IsKeyPressed(inputAction.keyboardKeys[i]))
                    return true;
            }

            //check mouse
            if (currentMouseState != null && previousMouseState != null)
            {
                for (int i = 0; i < inputAction.mouseButtons.Count; i++)
                {
                    if (IsMouseButtonPressed(inputAction.mouseButtons[i]))
                    {
                        return true;
                    }
                }
            }

            //Is a Gamepad pugged in?
            if (currentGamePadState.IsConnected)
            {
                for (int i = 0; i < inputAction.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonPressed(inputAction.gamePadButtons[i]))
                        return true;
                }
            }
            return false;
        }

        public bool IsKeyPressed(Keys key)
        {
            if (currentKeyboardState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        public bool IsGamePadButtonPressed(GamePadButtons gamePadKey)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Up:
                    return currentGamePadState.DPad.Up == ButtonState.Pressed;
                case GamePadButtons.Down:
                    return currentGamePadState.DPad.Down == ButtonState.Pressed;
                case GamePadButtons.Left:
                    return currentGamePadState.DPad.Left == ButtonState.Pressed;
                case GamePadButtons.Right:
                    return currentGamePadState.DPad.Right == ButtonState.Pressed;
                case GamePadButtons.LeftStickLeft:
                    return currentGamePadState.ThumbSticks.Left.X < 0 - analogLimit;
            }
            return false;
        }

        public bool IsMouseButtonPressed(MouseButtons mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButtons.LeftButton:
                    return currentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.RightButton:
                    return currentMouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.MiddleButton:
                    return currentMouseState.MiddleButton == ButtonState.Pressed;
                case MouseButtons.ScrollWheelUp:
                    return currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue;
                case MouseButtons.ScrollWheelDown:
                    return currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue;
            }

            return false;
        }

        //triggered Action checks
        public bool IsActionTriggered(string actionId)
        {
            if (inputActions.TryGetValue(actionId, out InputAction inputAction))
            {
                return IsInputActionTriggered(inputAction);
            }
            return false;
        }

        /// <summary>
        /// Input Action tiggered checking
        /// </summary>
        /// <param name="inputAction"></param>
        /// <returns></returns>
        private bool IsInputActionTriggered(InputAction inputAction)
        {
            for (int i = 0; i < inputAction.keyboardKeys.Count; i++)
            {
                if (IsKeyTriggered(inputAction.keyboardKeys[i]))
                {
                    return true;
                }
            }
            if (currentMouseState != null && previousMouseState != null)
            {
                for (int i = 0; i < inputAction.mouseButtons.Count; i++)
                {
                    if (IsMouseButtonTriggered(inputAction.mouseButtons[i]))
                    {
                        return true;
                    }
                }
            }
            if (currentGamePadState.IsConnected)
            {
                for (int i = 0; i < inputAction.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonTriggered(inputAction.gamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsKeyTriggered(Keys key)
        {
            if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        public bool IsMouseButtonTriggered(MouseButtons mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButtons.LeftButton:
                    return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
                case MouseButtons.RightButton:
                    return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
                case MouseButtons.MiddleButton:
                    return currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
                case MouseButtons.ScrollWheelUp:
                    return currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue;
                case MouseButtons.ScrollWheelDown:
                    return currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue;
            }

            return false;
        }

        public bool IsGamePadButtonTriggered(GamePadButtons gamePadKey)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Up:
                    return currentGamePadState.DPad.Up == ButtonState.Pressed && previousGamePadState.DPad.Up == ButtonState.Released;
                case GamePadButtons.Down:
                    return currentGamePadState.DPad.Down == ButtonState.Pressed && previousGamePadState.DPad.Down == ButtonState.Released;
                case GamePadButtons.Left:
                    return currentGamePadState.DPad.Left == ButtonState.Pressed && previousGamePadState.DPad.Left == ButtonState.Released;
                case GamePadButtons.Right:
                    return currentGamePadState.DPad.Right == ButtonState.Pressed && previousGamePadState.DPad.Right == ButtonState.Released;

                case GamePadButtons.LeftStickUp:
                    return currentGamePadState.ThumbSticks.Left.Y >= analogLimit && previousGamePadState.ThumbSticks.Left.Y < analogLimit;
                case GamePadButtons.LeftStickDown:
                    return currentGamePadState.ThumbSticks.Left.Y <= 0 - analogLimit && previousGamePadState.ThumbSticks.Left.Y > 0 - analogLimit;

                case GamePadButtons.ButtonA:
                    return currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A == ButtonState.Released;
            }
            return false;
        }


       // public Vector2 GetMousePosition()
       // {
       //     return currentMouseScreenPos.ToVector2();
       // }



        public void AddAction(InputAction action)
        {
            if (inputActions.ContainsKey(action.actionId))
                inputActions[action.actionId] = action;
            else
                inputActions.Add(action.actionId, action);
        }

        public void SaveInputActions(string fileName)
        {
            //write out the file for reading next time
            string json = JsonConvert.SerializeObject(inputActions, Formatting.Indented);
            System.IO.File.WriteAllText(fileName, json);
        }

        public bool LoadInputActions(string fileName)
        {
            //try load the actions from JSON.
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    //file exits so read it and deserialize into the inputActions List
                    string actions = System.IO.File.ReadAllText(fileName);
                    Dictionary<string, InputAction> inputActionsLoad = JsonConvert.DeserializeObject<Dictionary<string, InputAction>>(actions);
                    foreach(KeyValuePair<string, InputAction> action in inputActionsLoad)
                    {
                        inputActions.Add(action.Key, action.Value);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
