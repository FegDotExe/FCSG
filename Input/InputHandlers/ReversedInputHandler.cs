namespace FCSG
{
    /// <summary>
    /// A wrapper for input handlers which inverts the presses of the wrapper handler: when the wrapped handler is not pressed, this handler will be pressed, and vice versa
    /// </summary>
    public class ReversedInputHandler:InputHandler
    {
        private readonly InputHandler inputHandler;

        public ReversedInputHandler(InputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
        }

        public override bool Pressed(object lambdaArgument = null)
        {
            return !inputHandler.Pressed(lambdaArgument);
        }
    }
}
