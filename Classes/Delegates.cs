using Microsoft.Xna.Framework;
namespace FCSG{
    public delegate void TypeAction<Type>(Type type);

    public delegate int IntSpriteBaseDelegate(SpriteBase sprite);
    public delegate int IntSpriteDelegate(Sprite sprite);
    public delegate int IntSpriteObjDelegate(SpriteObject sprite);
    public delegate object ObjectSpriteBaseDelegate(SpriteBase sprite);

    public delegate double DoubleSpriteBaseDelegate(SpriteBase sprite);

    ///<summary>
    ///A delegate used for click events. It returns true if the click should go on and look for the other clicks.
    ///</summary>
    public delegate bool ClickDelegate(SpriteBase sprite, int x, int y);

    /// <summary>
    /// A delegate which rapresents a function run while a certain async task is run; the progress should go from 0 to 1.
    /// </summary>
    /// <param name="progress">The progress in the action, from 0 to 1.</param>
    public delegate void PercentageFunction(double progress);

    public delegate object LoadDelegate(string name, Microsoft.Xna.Framework.Content.ContentManager content);

    public delegate bool InputDelegate(object lambdaArgument);
    public delegate bool ControllerDelegate(PlayerIndex playerIndex);

    /// <summary>
    /// A simple delegate mostly used to run other functions.
    /// </summary>
    public delegate void SimpleDelegate();
}