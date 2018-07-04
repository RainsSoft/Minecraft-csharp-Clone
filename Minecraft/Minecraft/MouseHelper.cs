using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace net.minecraft.src
{
    public enum MouseButtons { Left, Right, Middle, X1, X2 };

    public class MouseHelper
    {
        Game game;

        MouseState mouseState;
        MouseState oldMouseState;

        public int X;

        public int Y;

        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                Mouse.SetPosition((int)value.X, (int)value.Y);
            }
        }

        public Vector2 Delta;

        public int WheelDelta
        {
            get { return mouseState.ScrollWheelValue; }
        }

        public MouseHelper(Game game)
        {
            this.game = game;
        }

        public void Update(GameTime gameTime)
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            X = mouseState.X;
            Y = mouseState.Y;

            position.X = X;
            position.Y = Y;

            Delta.X = mouseState.X - oldMouseState.X;
            Delta.Y = mouseState.Y - oldMouseState.Y;
        }

        public bool WasButtonPressed()
        {
            return WasButtonPressed(MouseButtons.Left) || WasButtonPressed(MouseButtons.Middle) || WasButtonPressed(MouseButtons.Right);
        }

        public bool WasButtonPressed(MouseButtons button)
        {
            return GetButtonState(button, mouseState) == ButtonState.Pressed && game.IsActive;
        }
        
        public bool WasButtonJustPressed()
        {
            return WasButtonJustPressed(MouseButtons.Left) || WasButtonJustPressed(MouseButtons.Middle) || WasButtonJustPressed(MouseButtons.Right);
        }

        public bool WasButtonJustPressed(MouseButtons button)
        {
            return GetButtonState(button, mouseState) == ButtonState.Pressed &&
                GetButtonState(button, oldMouseState) == ButtonState.Released && game.IsActive;
        }

        public bool WasButtonHeld(MouseButtons button)
        {
            return GetButtonState(button, mouseState) == ButtonState.Pressed &&
                GetButtonState(button, oldMouseState) == ButtonState.Pressed && game.IsActive;
        }

        public bool WasButtonReleased()
        {
            return WasButtonReleased(MouseButtons.Left) || WasButtonReleased(MouseButtons.Middle) || WasButtonReleased(MouseButtons.Right);
        }

        public bool WasButtonReleased(MouseButtons button)
        {
            return GetButtonState(button, mouseState) == ButtonState.Released &&
                GetButtonState(button, oldMouseState) == ButtonState.Pressed && game.IsActive;
        }

        ButtonState GetButtonState(MouseButtons button, MouseState state)
        {
            switch (button)
            {
                case MouseButtons.Left: return state.LeftButton;
                case MouseButtons.Middle: return state.MiddleButton;
                case MouseButtons.Right: return state.RightButton;
                case MouseButtons.X1: return state.XButton1;
                case MouseButtons.X2: return state.XButton2;
                default: return ButtonState.Released;
            }
        }
    }
}
