using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FCSG
{
    /// <summary>
    /// Handles custom input defined through a delegate
    /// </summary>
    public class LambdaInputHandler: InputHandler
    {
        private readonly InputDelegate inputDelegate;

        /// <summary>
        /// Construct a new input hanlder which works with lambdas
        /// </summary>
        /// <param name="inputDelegate">A normal delegate</param>
        /// <param name="controllerDelegate">A special delegate used for controller inputs. When using this parameter, checks for the lambdaArgument to be a PlayerIndex are implicitly added. Usually, a lambda of this kind is <c>(PlayerIndex playerIndex)=>GamePad.GetState(playerIndex).Buttons.A == ButtonState.Pressed</c></param>
        /// <exception cref="System.Exception"></exception>
        public LambdaInputHandler(InputDelegate inputDelegate=null, ControllerDelegate controllerDelegate=null)
        {
            if (controllerDelegate != null)
            {
                this.inputDelegate = (object playerIndex) =>
                {
                    if (!typeof(PlayerIndex).IsInstanceOfType(playerIndex))
                    {
                        throw new System.Exception("The given type was not PlayerIndex.");
                    }
                    PlayerIndex index = (PlayerIndex)playerIndex;

                    return controllerDelegate(index);
                };
                return;
            }
            else
            {
                this.inputDelegate = inputDelegate;
            }
        }

        /// <summary>
        /// Tests if the key is pressed
        /// </summary>
        /// <param name="lambdaArgument">An argument to pass to a lambda function; for controller lambdas this usually is the player index</param>
        /// <returns><c>true</c> if the lambda is <c>true</c>, <c>false</c> otherwise</returns>
        public override bool Pressed(object lambdaArgument = null)
        {
            return inputDelegate(lambdaArgument);
        }
    }
}
