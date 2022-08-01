using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

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

        private delegate Vector2 ThumbStickDelegate(object lambdaArgument);
        /// <summary>
        /// Build a lambda input handler which is pressed when the given stick is in the given direction, but beyond the given deadzone
        /// </summary>
        /// <param name="thumbStick">Which thumbstick should be considered</param>
        /// <param name="direction">Which direction should be considered</param>
        /// <param name="deadZone">The deadzone of the thumbstick. It must be a value between 0 (no deadzone) and 1 (all deadzone)</param>
        /// <exception cref="System.Exception"></exception>
        public LambdaInputHandler(ControllerThumbSticks thumbStick, ControllerThumbSticksDirections8 direction, double deadZone)
        {
            ThumbStickDelegate thumbStickDelegate=null;
            if (thumbStick == ControllerThumbSticks.Left)
            {
                thumbStickDelegate = (object playerIndex) =>
                {
                    if (!typeof(PlayerIndex).IsInstanceOfType(playerIndex))
                    {
                        throw new System.Exception("The given type was not PlayerIndex.");
                    }
                    PlayerIndex index = (PlayerIndex)playerIndex;

                    return GamePad.GetState(index).ThumbSticks.Left;
                };
            }
            else
            {
                thumbStickDelegate = (object playerIndex) =>
                {
                    if (!typeof(PlayerIndex).IsInstanceOfType(playerIndex))
                    {
                        throw new System.Exception("The given type was not PlayerIndex.");
                    }
                    PlayerIndex index = (PlayerIndex)playerIndex;

                    return GamePad.GetState(index).ThumbSticks.Right;
                };
            }

            switch (direction)
            {
                case ControllerThumbSticksDirections8.Up:
                    inputDelegate = (object lambdaArgument) =>
                    {
                        Vector2 position = thumbStickDelegate(lambdaArgument);

                        return (
                            (Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) >= Math.Pow(deadZone,2)) &&
                            (position.Y >= (position.X*Cotangent(22.5))) &&
                            (position.Y >= (position.X * Cotangent(-22.5)))
                        );
                    };
                    break;
                case ControllerThumbSticksDirections8.UpRight:
                    inputDelegate = (object lambdaArgument) =>
                    {
                        Vector2 position = thumbStickDelegate(lambdaArgument);

                        return (
                            (Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) >= Math.Pow(deadZone, 2)) &&
                            (position.Y <= (position.X * Cotangent(22.5))) &&
                            (position.Y >= (position.X * Cotangent(67.5)))
                        );
                    };
                    break;
                case ControllerThumbSticksDirections8.Right:
                    inputDelegate = (object lambdaArgument) =>
                    {
                        Vector2 position = thumbStickDelegate(lambdaArgument);

                        return (
                            (Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) >= Math.Pow(deadZone, 2)) &&
                            (position.Y <= (position.X * Cotangent(67.5))) &&
                            (position.Y >= (position.X * Cotangent(112.5)))
                        );
                    };
                    break;
                case ControllerThumbSticksDirections8.DownRight:
                    inputDelegate = (object lambdaArgument) =>
                    {
                        Vector2 position = thumbStickDelegate(lambdaArgument);

                        return (
                            (Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) >= Math.Pow(deadZone, 2)) &&
                            (position.Y <= (position.X * Cotangent(112.5))) &&
                            (position.Y >= (position.X * Cotangent(-22.5)))
                        );
                    };
                    break;
                case ControllerThumbSticksDirections8.Down:
                    inputDelegate = (object lambdaArgument) =>
                    {
                        Vector2 position = thumbStickDelegate(lambdaArgument);

                        return (
                            (Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) >= Math.Pow(deadZone, 2)) &&
                            (position.Y <= (position.X * Cotangent(22.5))) &&
                            (position.Y <= (position.X * Cotangent(-22.5)))
                        );
                    };
                    break;
                case ControllerThumbSticksDirections8.DownLeft:
                    inputDelegate = (object lambdaArgument) =>
                    {
                        Vector2 position = thumbStickDelegate(lambdaArgument);

                        return (
                            (Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) >= Math.Pow(deadZone, 2)) &&
                            (position.Y <= (position.X * Cotangent(67.5))) &&
                            (position.Y >= (position.X * Cotangent(22.5)))
                        );
                    };
                    break;
                case ControllerThumbSticksDirections8.Left:
                    inputDelegate = (object lambdaArgument) =>
                    {
                        Vector2 position = thumbStickDelegate(lambdaArgument);

                        return (
                            (Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) >= Math.Pow(deadZone, 2)) &&
                            (position.Y <= (position.X * Cotangent(112.5))) &&
                            (position.Y >= (position.X * Cotangent(67.5)))
                        );
                    };
                    break;
                case ControllerThumbSticksDirections8.UpLeft:
                    inputDelegate = (object lambdaArgument) =>
                    {
                        Vector2 position = thumbStickDelegate(lambdaArgument);

                        return (
                            (Math.Pow(position.X, 2) + Math.Pow(position.Y, 2) >= Math.Pow(deadZone, 2)) &&
                            (position.Y <= (position.X * Cotangent(-22.5))) &&
                            (position.Y >= (position.X * Cotangent(112.5)))
                        );
                    };
                    break;
            }
        }

        private double Cotangent(double angle)
        {
            double radians= (angle/180)*Math.PI;

            return Math.Cos(radians)/Math.Sin(radians);
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

    public enum ControllerThumbSticks
    {
        Left,
        Right
    }
    public enum ControllerThumbSticksDirections8
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }
}
