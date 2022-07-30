using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace FCSG
{
    /// <summary>
    /// Handles a single keyboard key press
    /// </summary>
    public class KeyInputHandler: InputHandler
    {
        private Keys key;

        public KeyInputHandler(Keys key)
        {
            this.key = key;
            lastPress = false;
        }

        /// <summary>
        /// Tests if the key is pressed
        /// </summary>
        /// <param name="lambdaArgument">Completely useless, only exists to respect inheritance</param>
        /// <returns><c>true</c> if the key is down, <c>false</c> otherwise</returns>
        public override bool Pressed(object lambdaArgument=null)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
    }
}
