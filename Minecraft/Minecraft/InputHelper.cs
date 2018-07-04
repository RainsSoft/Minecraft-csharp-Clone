using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace net.minecraft.src
{
    public class InputHelper
    {
        Game game;

        KeyboardState keyState;
        KeyboardState oldKeyState;

        public MouseHelper Mouse;

        public InputHelper(Game game)
        {
            this.game = game;
            Mouse = new MouseHelper(game);
        }
        
        public void Update(GameTime gameTime)
        {
            Mouse.Update(gameTime);

            oldKeyState = keyState;
            keyState = Keyboard.GetState();
        }

        public Keys GetPressedKey()
        {
            var keys = keyState.GetPressedKeys();
            if (keys.Length > 0)
                return keys[keys.Length - 1];
            else return Keys.None;
        }

        public bool WasKeyPressed(Keys key)
        {
            return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key) && game.IsActive;
        }

        public bool IsKeyUp(Keys key)
        {
            return keyState.IsKeyUp(key) && game.IsActive;
        }

        public bool IsKeyDown(Keys key)
        {
            return keyState.IsKeyDown(key) && game.IsActive;
        }
    }
}
