using System.Collections.Generic;

namespace FCSG
{
    /// <summary>
    /// Handles input of multiple InputHandlers at the same time; is considered pressed when at least a contained input handler is pressed
    /// </summary>
    public class InclusiveInputHandler : InputHandler
    {
        private readonly List<InputHandler> inputHandlers;

        public InclusiveInputHandler(List<InputHandler> inputHandlers)
        {
            this.inputHandlers = inputHandlers;
        }
        public InclusiveInputHandler()
        {
            this.inputHandlers = new List<InputHandler>();
        }

        public void Add(InputHandler inputHandler)
        {
            inputHandlers.Add(inputHandler);
        }

        /// <summary>
        /// Tests if any key is pressed
        /// </summary>
        /// <param name="lambdaArgument">The lambda argument to pass to each input hanlder in this class</param>
        /// <returns><c>true</c> if any key is down, <c>false</c> otherwise</returns>
        public override bool Pressed(object lambdaArgument = null)
        {
            foreach (InputHandler inputHandler in inputHandlers)
            {
                if (inputHandler.Pressed(lambdaArgument))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
