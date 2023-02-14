namespace FCSG
{
    /// <summary>
    /// A class to handle generic input.
    /// Most of this class' is determined by its <see cref="Pressed(object)"/> method: all the other methods are automatically derived by its definition.
    /// <para>
    /// The method <see cref="Update(object)"/> should be called at each update cycle so that the value for the last press stays updated.
    /// </para>
    /// </summary>
    public class InputHandler
    {
        protected bool lastPress=false; //Value of the last pressed call
        public virtual bool Pressed(object lambdaArgument=null)
        {
            throw new System.Exception("The method InputHandler.Pressed needs to be overridden.");
        }
        /// <summary>
        /// Test if the key has just been pressed
        /// </summary>
        /// <param name="lambdaArgument">An argument to pass to a lambda function, if the handler is implemented through lambdas.</param>
        /// <returns><c>true</c> if <c>Pressed()</c> was <c>false</c> last time <c>Update()</c> was called, but now it is <c>true</c>; it returns <c>false</c> in every other case</returns>
        public bool NewPress(object lambdaArgument=null)
        {
            if (!lastPress)
            {
                return Pressed(lambdaArgument);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Test if the given handler's inputs have just been released
        /// </summary>
        /// <param name="lambdaArgument">An argument to pass to a lambda function, if the handler is implemented through lambdas.</param>
        /// <returns><c>true</c> if <c>Pressed()</c> was <c>true</c> last time <c>Update()</c> was called, but now it is <c>false</c>; it returns <c>false</c> in every other case</returns>
        public bool Released(object lambdaArgument=null)
        {
            if (lastPress)
            {
                return !Pressed(lambdaArgument);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Update the pressed value which will be used in the next calls to this class' function
        /// </summary>
        /// <param name="lambdaArgument">An argument to pass to a lambda function, if the handler is implemented through lambdas.</param>
        public void Update(object lambdaArgument=null)
        {
            lastPress = Pressed(lambdaArgument);
        }
    }
}
