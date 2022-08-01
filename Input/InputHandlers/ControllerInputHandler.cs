using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FCSG
{
    public class ControllerInputHandler: InputHandler
    {
        private Buttons button;

        public ControllerInputHandler(Buttons button)
        {
            this.button = button;
        }

        public override bool Pressed(object playerIndex)
        {
            if (!typeof(PlayerIndex).IsInstanceOfType(playerIndex))
            {
                throw new System.Exception("The given type was not PlayerIndex.");
            }
            PlayerIndex index=(PlayerIndex)playerIndex;
            return GamePad.GetState(index).IsButtonDown(button);
        }
    }
}
