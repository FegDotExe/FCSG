using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FCSG{
    public class SpriteParameters{
        #region Fields
        public SpriteBatch spriteBatch;
        public Texture2D texture;
        public SpriteFont font;
        public string text;
        public TextSprite.WrapMode wrapMode;
        public TextSprite.LayoutMode layoutMode;
        public TextSprite.LayoutHeight layoutHeight;
        public int offsetX; //Only for TextSprite
        public int offsetY; //Only for TextSprite
        public LinkedVariableParams<int> originalWidthVariable; //Only for TextSprite
        public LinkedVariableParams<int> originalHeightVariable; //Only for TextSprite
        public Wrapper wrapper;
        public float? depth;
        public LinkedVariableParams<float> depthVariable;
        public LinkedVariableParams<int> xVariable;
        public int? x;
        public LinkedVariableParams<int> yVariable;
        public int? y;
        public LinkedVariableParams<int> widthVariable;
        public int? width;
        public LinkedVariableParams<int> heightVariable;
        public int? height;
        public float rotation; 
        public Vector2 origin; 
        public Color color;
        public ObjectGroup<SpriteObject> group;
        public List<ObjectGroup<SpriteObject>> groups;
        public ClickSetting leftClickDelegate;
        public ClickSetting middleClickDelegate;
        public ClickSetting rightClickDelegate;
        public ClickSetting wheelHoverDelegate;
        public ClickSetting hoverDelegate;
        public Dictionary<string, SpriteBase> spritesDict;
        public string dictKey;
        public CollisionRectangle collisionRectangle;
        public PrecisionSetting precision; //Wether clicks should be pixel-precise or not.
        public Dictionary<string,object> variables;
        public SpriteBatchParameters textBatchParameters; //Used to draw the text sub-texture in TextSprite.

        #endregion Fields
        #region Constructor


        /// <summary>
        /// Constructor for SpriteParameters.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw sprites.</param>
        /// <param name="texture">The texture which will be drawn. In FCSG, this is only used in <see cref="Sprite"/>.</param>
        /// <param name="font">The font which will be used to display text. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="text">The default displayed text. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="wrapMode">How the text is wrapped. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="layoutMode">How the text is displayed in the x axis. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="layoutHeight">How the text is displayed in the y axis. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="offsetX">The x offset used to draw the base text texture. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="offsetY">The y offset used to draw the base text texture. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="originalWidthVariable">The linked variable used to determine the width of the base text texture; it is retrieved at <see cref="TextSprite.ElaborateTexture(bool,bool)"/> when <c>reloadDimension</c> is true. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="originalHeightVariable">The linked variable used to determine the height of the base text texture; it is retrieved at <see cref="TextSprite.ElaborateTexture(bool,bool)"/> when <c>reloadDimension</c> is true. In FCSG, this is only used in <see cref="TextSprite"/>.</param>
        /// <param name="wrapper">The wrapper this sprite will be added to. This parameter is extremely useful for child classes which make use of the wrapper in their constructor or generally before the sprite can actually be added to a wrapper.</param>
        /// <param name="depth">The depth of the sprite. The higher the number, the closer to the camera. The value can vary between 1 and 0. This will give a set value to <see cref="SpriteBase.depthVariable"/>.</param>
        /// <param name="depthVariable">The parameters of the depth variable for this sprite. The higher the number, the closer to the camera. The value can vary between 1 and 0.</param>
        /// <param name="xVariable">The parameters of the x variable for this sprite. This determines the x position of the sprite and is retreived at every <see cref="SpriteBase.Draw(bool)"/>.</param>
        /// <param name="x">This determines the x position of the sprite; it will give a set value to <see cref="SpriteBase.xVariable"/>.</param>
        /// <param name="yDelegate">The delegate which returns the y position of the sprite.</param>
        /// <param name="widthDelegate">The delegate which returns the width of the sprite.</param>
        /// <param name="heightDelegate">The delegate which returns the height of the sprite.</param>
        /// <param name="rotation">The rotation of the sprite. (I should probably not touch this)</param>
        /// <param name="origin">The origin of the sprite. (I should probably not touch this)</param>
        /// <param name="color">The color of the sprite.</param>
        /// <param name="group">A group to which the sprite will be added when constructed.</param>
        /// <param name="groups">A list of groups to which the sprite will be added when constructed.</param>
        /// <param name="leftClickDelegate">The delegate which will be called when the sprite is clicked with the left mouse button.</param>
        /// <param name="middleClickDelegate">The delegate which will be called when the sprite is clicked with the middle mouse button.</param>
        /// <param name="rightClickDelegate">The delegate which will be called when the sprite is clicked with the right mouse button.</param>
        /// <param name="wheelHoverDelegate">The delegate which will be called when the scrolls.</param>
        /// <param name="hoverDelegate">The delegate which will be called when the mouse is over the sprite.</param>
        /// <param name="spritesDict">A dictionary to which the sprite will be added once constructed.</param>
        /// <param name="dictKey">The key the dictionary will use when inserted in the <c>spritesDict</c></param>
        public SpriteParameters(
            SpriteBatch spriteBatch = null,
            Texture2D texture = null, //Only for Sprite
            SpriteFont font = null, //Only for TextSprite
            string text = null, //Only for TextSprite
            TextSprite.WrapMode wrapMode = TextSprite.WrapMode.Word, //Only for TextSprite
            TextSprite.LayoutMode layoutMode = TextSprite.LayoutMode.Left, //Only for TextSprite
            TextSprite.LayoutHeight layoutHeight = TextSprite.LayoutHeight.Top,
            int offsetX=0, //Only for TextSprite
            int offsetY=0, //Only for TextSprite
            LinkedVariableParams<int> originalWidthVariable=null, //Only for TextSprite
            LinkedVariableParams<int> originalHeightVariable=null, //Only for TextSprite
            Wrapper wrapper=null,
            float? depth=null, 
            LinkedVariableParams<float> depthVariable=null,
            LinkedVariableParams<int> xVariable=null,
            int? x=null,
            LinkedVariableParams<int> yVariable=null,
            int? y=null,
            LinkedVariableParams<int> widthVariable=null, 
            int? width=null,
            LinkedVariableParams<int> heightVariable=null,
            int? height=null,
            float rotation=0, 
            Vector2? origin=null, 
            Color? color=null,
            ObjectGroup<SpriteObject> group=null,
            List<ObjectGroup<SpriteObject>> groups=null,
            ClickSetting leftClickDelegate=null,
            ClickSetting middleClickDelegate=null,
            ClickSetting rightClickDelegate=null,
            ClickSetting wheelHoverDelegate=null,
            ClickSetting hoverDelegate=null,
            PrecisionSetting precision=PrecisionSetting.Collision,
            Dictionary<string, SpriteBase> spritesDict=null,
            string dictKey=null,
            CollisionRectangle collisionRectangle=null,
            Dictionary<string,object> variables=null,
            SpriteBatchParameters textBatchParameters=null
        ){
            //Variables which are not modified by default
            this.spriteBatch=spriteBatch;
            this.texture=texture;
            this.font=font;
            this.text=text;
            this.wrapMode=wrapMode;
            this.layoutMode=layoutMode;
            this.layoutHeight = layoutHeight;
            this.offsetX=offsetX;
            this.offsetY=offsetY;
            this.originalWidthVariable=originalWidthVariable;
            this.originalHeightVariable=originalHeightVariable;
            this.wrapper=wrapper;
            this.depth=depth;
            this.width=width;
            this.height=height;
            this.rotation=rotation;
            this.group=group;
            this.groups=groups;
            this.spritesDict=spritesDict;
            this.dictKey=dictKey;
            this.collisionRectangle=collisionRectangle;
            this.variables=variables;
            this.textBatchParameters=textBatchParameters;

            // Position variables
                this.xVariable=xVariable;
                this.x=x;
                this.yVariable=yVariable;
                this.y=y;

            //Size variables
                this.widthVariable=widthVariable;
                this.heightVariable=heightVariable;

            //Depth variables
                this.depthVariable=depthVariable;

            //Click delegates
                this.leftClickDelegate = leftClickDelegate;
                this.middleClickDelegate = middleClickDelegate;
                this.rightClickDelegate = rightClickDelegate;
                this.wheelHoverDelegate = wheelHoverDelegate;
                this.hoverDelegate = hoverDelegate;
            this.precision=precision;

            //Origin
            if(origin!=null){
                this.origin = (Vector2)origin;
            }else{
                this.origin = new Vector2(0,0);
            }

            //Color
            if(color!=null){
                this.color = (Color)color;
            }else{
                this.color = Color.White;
            }
        }
        #endregion Constructor
    
        #region Operators
        /// <summary>
        /// Sums two spriteparameters. If the second operator has a property, than that property will be the one considered in the end.
        /// </summary>
        /// <param name="sp1">The first sprite parameter</param>
        /// <param name="sp2">the second sprite parameter</param>
        /// <returns>A sprite parameter result of the sum of the two given sprite parameters</returns>
        public static SpriteParameters operator +(SpriteParameters sp1, SpriteParameters sp2){
            if (sp2 == null)
            {
                return sp1;
            }
            SpriteParameters output=new SpriteParameters();
            output.spriteBatch=Utilities.Choose<SpriteBatch>(sp1.spriteBatch,sp2.spriteBatch);
            output.texture=Utilities.Choose<Texture2D>(sp1.texture,sp2.texture);
            output.font=Utilities.Choose<SpriteFont>(sp1.font,sp2.font);
            output.text=Utilities.Choose<string>(sp1.text,sp2.text);
            output.wrapMode=Utilities.Choose<TextSprite.WrapMode>(sp1.wrapMode,sp2.wrapMode);
            output.layoutMode=Utilities.Choose<TextSprite.LayoutMode>(sp1.layoutMode,sp2.layoutMode);
            output.layoutHeight = Utilities.Choose(sp1.layoutHeight, sp2.layoutHeight);
            output.offsetX=Utilities.Choose<int>(sp1.offsetX,sp2.offsetX);
            output.offsetY=Utilities.Choose<int>(sp1.offsetY,sp2.offsetY);
            output.originalWidthVariable=Utilities.Choose(sp1.originalWidthVariable,sp2.originalWidthVariable);
            output.originalHeightVariable=Utilities.Choose(sp1.originalHeightVariable,sp2.originalHeightVariable);
            output.wrapper=Utilities.Choose(sp1.wrapper,sp2.wrapper);
            output.depth=Utilities.Choose(sp1.depth,sp2.depth);
            output.depthVariable = Utilities.Choose(sp1.depthVariable, sp2.depthVariable);
            output.xVariable=Utilities.Choose(sp1.xVariable,sp2.xVariable);
            output.x=Utilities.Choose(sp1.x,sp2.x);
            output.yVariable=Utilities.Choose(sp1.yVariable,sp2.yVariable);
            output.y=Utilities.Choose(sp1.y,sp2.y);
            output.widthVariable=Utilities.Choose(sp1.widthVariable,sp2.widthVariable);
            output.width=Utilities.Choose(sp1.width,sp2.width);
            output.heightVariable=Utilities.Choose(sp1.heightVariable,sp2.heightVariable);
            output.height=Utilities.Choose(sp1.height,sp2.height);
            output.rotation=Utilities.Choose(sp1.rotation,sp2.rotation);
            output.origin=Utilities.Choose(sp1.origin,sp2.origin);
            output.color=Utilities.Choose(sp1.color,sp2.color);
            output.group=Utilities.Choose(sp1.group,sp2.group);
            output.groups=Utilities.Choose(sp1.groups,sp2.groups);
            output.leftClickDelegate=Utilities.Choose(sp1.leftClickDelegate,sp2.leftClickDelegate);
            output.middleClickDelegate=Utilities.Choose(sp1.middleClickDelegate,sp2.middleClickDelegate);
            output.rightClickDelegate=Utilities.Choose(sp1.rightClickDelegate,sp2.rightClickDelegate);
            output.wheelHoverDelegate=Utilities.Choose(sp1.wheelHoverDelegate,sp2.wheelHoverDelegate);
            output.hoverDelegate=Utilities.Choose(sp1.hoverDelegate,sp2.hoverDelegate);
            output.spritesDict=Utilities.Choose(sp1.spritesDict,sp2.spritesDict);
            output.dictKey=Utilities.Choose(sp1.dictKey,sp2.dictKey);
            output.collisionRectangle=Utilities.Choose(sp1.collisionRectangle,sp2.collisionRectangle);
            output.precision = Utilities.Choose(sp1.precision, sp2.precision);
            output.variables = Utilities.Choose(sp1.variables, sp2.variables);
            output.textBatchParameters = Utilities.Choose(sp1.textBatchParameters, sp2.textBatchParameters);

            return output;
        }
        #endregion Operators
    }
}