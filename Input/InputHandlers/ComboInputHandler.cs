﻿using System.Collections.Generic;

namespace FCSG
{
    /// <summary>
    /// Handles input of multiple InputHandlers at the same time; is considered pressed when all the contained input handlers are pressed
    /// </summary>
    public class ComboInputHandler: InputHandler
    {
        private readonly List<InputHandler> inputHandlers;

        public ComboInputHandler(List<InputHandler> inputHandlers)
        {
            this.inputHandlers = inputHandlers;
        }
        public ComboInputHandler()
        {
            this.inputHandlers = new List<InputHandler>();
        }

        public void Add(InputHandler inputHandler)
        {
            inputHandlers.Add(inputHandler);
        }

        /// <summary>
        /// Tests if all the keys are pressed
        /// </summary>
        /// <param name="lambdaArgument">The lambda argument to pass to each input hanlder in this class</param>
        /// <returns><c>true</c> if all the keys are down, <c>false</c> otherwise</returns>
        public override bool Pressed(object lambdaArgument = null)
        {
            foreach(InputHandler inputHandler in inputHandlers)
            {
                if (!inputHandler.Pressed(lambdaArgument))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
