using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;

namespace FCSG{
    /// <summary>
    /// The base sprite class, from which other classes inherit.
    /// </summary>
    public class SpriteBase : SpriteObject{
        #region Fields
        protected SpriteBatch spriteBatch;
        public string name="";
        
        //Position values
        public int x{
            get{return xVariable.Get();}
            set{xVariable.Set(value);}
        }
        public int y{
            get{return yVariable.Get();}
            set{yVariable.Set(value);}
        }
        public LinkedVariable<int> xVariable;
        public LinkedVariable<int> yVariable;

        //Size values
        public LinkedVariable<int> widthVariable;
        public LinkedVariable<int> heightVariable;
        public int width{
            get{return widthVariable.Get();}
            set{widthVariable.Set(value);}
        }
        protected int midWidth{get;set;} //Used to know wether the middle texture should be redrawn or not
        public int height{
            get{return heightVariable.Get();}
            set{heightVariable.Set(value);}
        }
        protected int midHeight{get;set;} //Used to know wether the middle texture should be redrawn or not

        public Texture2D texture{get;set;}
        protected Texture2D middleTexture;

        public Color color; //Used when the sprite is drawn
        public float rotation{get;set;}
        public Vector2 origin{get;set;}

        public float? depth{
            get { return depthVariable.Get(); }   
            set { depthVariable.Set((float)value);}
        }
        public LinkedVariable<float> depthVariable;

        public SpriteEffects effects=SpriteEffects.None;

        public bool draw{get;set;} //Wether the sprite will be drawn or not

        public Wrapper wrapper; //The wrapper which contains the sprite

        protected List<ObjectGroup<SpriteObject>> groups{get;set;}
        //Click delegates
        #region ClickDelegates
        public ClickSetting leftClickDelegate{
            get{
                return clickArray[(int)Clicks.Left];
            }
            set{
                clickArray[(int)Clicks.Left] = value;
                if(value!=null){
                    wrapper.leftClick.Add(this);
                    UpdateCollisionRectangle();
                }else{
                    wrapper.leftClick.Remove(this);
                }
            }
        }
        public ClickSetting middleClickDelegate{
            get{
                return clickArray[(int)Clicks.Middle];
            }
            set{
                clickArray[(int)Clicks.Middle] = value;
                if(value!=null){
                    wrapper.middleClick.Add(this);
                    UpdateCollisionRectangle();
                }
            else{
                    wrapper.middleClick.Remove(this);
                }
            }
        }
        public ClickSetting rightClickDelegate{
            get{
                return clickArray[(int)Clicks.Right];
            }
            set{
                clickArray[(int)Clicks.Right] = value;
                if(value!=null){
                    wrapper.rightClick.Add(this);
                    UpdateCollisionRectangle();
                }
                else{
                    wrapper.rightClick.Remove(this);
                }
            }
        }
        public ClickSetting wheelHoverDelegate{
            get{
                return clickArray[(int)Clicks.WheelHover];
            }
            set{
                clickArray[(int)Clicks.WheelHover] = value;
                if(value!=null){
                    wrapper.wheelHover.Add(this);
                    UpdateCollisionRectangle();
                }
                else{
                    wrapper.wheelHover.Remove(this);
                }
            }
        }
        public ClickSetting hoverDelegate{
            get{
                return clickArray[(int)Clicks.Hover];
            }
            set{
                clickArray[(int)Clicks.Hover] = value;
                if(value!=null){
                    wrapper.hover.Add(this);
                    UpdateCollisionRectangle();
                }
                else{
                    wrapper.hover.Remove(this);
                }
            }
        }
        public ClickSetting?[] clickArray = new ClickSetting?[1+(int)Enum.GetValues(typeof(Clicks)).Cast<Clicks>().Last<Clicks>()];
        #endregion ClickDelegates

        protected PrecisionSetting precision; //Whether precise clicking is on or off

        public Dictionary<string,object> variables{get; set;}

        /// <summary>
        /// A rectangle used for collision detection. its coordinates are relative to the sprite's position, and so is the size.
        /// </summary>
        protected CollisionRectangle collisionRectangle;

        #endregion Fields
        #region Constructors
        public SpriteBase(
            SpriteParameters spriteParameters
        ){
            if (spriteParameters.spriteBatch == null) {
                throw new ArgumentNullException();
            }
            this.spriteBatch = spriteParameters.spriteBatch;

            this.wrapper = spriteParameters.wrapper;
            //Add to wrapper
            if(this.wrapper!=null){
                this.wrapper.Add(this);
            }

            if (spriteParameters.variables != null)
            {
                this.variables = spriteParameters.variables;
            }
            else
            {
                this.variables = new Dictionary<string, object>();
            }

            //Adding to groups
            this.groups=new List<ObjectGroup<SpriteObject>>();
            if(spriteParameters.group!=null){ //Adds the sprite to the group
                this.groups.Add(spriteParameters.group);
                spriteParameters.group.Add(this);
            }
            if (spriteParameters.groups!=null){
                foreach(ObjectGroup<SpriteObject> spriteGroup in spriteParameters.groups){
                    this.groups.Add(spriteGroup);
                    spriteGroup.Add(this);
                }
            }
            if(spriteParameters.spritesDict!=null){
                if(spriteParameters.dictKey!=null){
                    spriteParameters.spritesDict.Add(spriteParameters.dictKey,this);
                    this.name=spriteParameters.dictKey; //FIXME: remove this along with names
                }else{
                    Console.WriteLine("Warning: SpriteBase constructor: dictKey is null, thus the object was not added to the dictionary.");
                }
            }

            //Position variables
                if(spriteParameters.x!=null){
                    this.xVariable = new LinkedVariable<int>(this,(SpriteBase sb)=>(int)spriteParameters.x);
                }
                else if(spriteParameters.xVariable != null){
                    this.xVariable = new LinkedVariable<int>(this, spriteParameters.xVariable);
                    // xVariable.SetSprite(this);
                }
                else
                {
                    this.xVariable = new LinkedVariable<int>(this, (SpriteBase sb) => 0);
                }
                if(spriteParameters.y!=null){
                    this.yVariable = new LinkedVariable<int>(this,(SpriteBase sb)=>(int)spriteParameters.y);
                }
                else if(spriteParameters.yVariable != null){
                    this.yVariable = new LinkedVariable<int>(this, spriteParameters.yVariable);
                    // yVariable.SetSprite(this);
                }
                else
                {
                    this.yVariable = new LinkedVariable<int>(this, (SpriteBase sb) => 0);
                }

            //Size delegates
                if(spriteParameters.width!=null){
                    this.widthVariable=new LinkedVariable<int>(this,(SpriteBase sprite)=>(int)spriteParameters.width);
                }
                else if(spriteParameters.widthVariable!=null)
                    this.widthVariable = new LinkedVariable<int>(this, spriteParameters.widthVariable);
                else
                    this.widthVariable = new LinkedVariable<int>(this,(SpriteBase sprite) => 100);
                midWidth = -1;
                
                if(spriteParameters.height!=null){
                    this.heightVariable=new LinkedVariable<int>(this,(SpriteBase sprite)=>(int)spriteParameters.height);
                }
                else if(spriteParameters.heightVariable!=null)
                    this.heightVariable = new LinkedVariable<int>(this, spriteParameters.heightVariable);
                else
                    this.heightVariable = new LinkedVariable<int>(this,(SpriteBase sprite) => 100);
                midHeight = -1;

            //Depth variable
            if (spriteParameters.depth != null) {
                this.depthVariable = new LinkedVariable<float>(this, (SpriteBase sb) => (float)spriteParameters.depth);
            }
            else if(spriteParameters.depthVariable!=null)
            {
                this.depthVariable = new LinkedVariable<float>(this, spriteParameters.depthVariable);
            }
            else
            {
                this.depthVariable = new LinkedVariable<float>(this,(SpriteBase sb) => 0);
            }


            this.rotation=spriteParameters.rotation;

            this.origin=spriteParameters.origin;

            this.color=spriteParameters.color;

            //Click delegates
                clickArray[(int)Clicks.Left] = spriteParameters.leftClickDelegate;
                clickArray[(int)Clicks.Middle] = spriteParameters.middleClickDelegate;
                clickArray[(int)Clicks.Right] = spriteParameters.rightClickDelegate;
                clickArray[(int)Clicks.WheelHover] = spriteParameters.wheelHoverDelegate;
                clickArray[(int)Clicks.Hover] = spriteParameters.hoverDelegate;
                
                this.precision=spriteParameters.precision;

                UpdateCollisionRectangle();

            //Collision rectangle
            if(collisionRectangle!=null && spriteParameters.collisionRectangle!=null){
                collisionRectangle=spriteParameters.collisionRectangle; //Use the collision rectangle provided by the parameters
            }

            this.xVariable.Activate();
            this.yVariable.Activate();
            this.widthVariable.Activate();
            this.heightVariable.Activate();
            this.depthVariable.Activate();
            if(this.collisionRectangle!=null && this.collisionRectangle.sprite==null){
                this.collisionRectangle.Activate(this);
            }

            this.draw=true;
        }
        #endregion Constructors
        public virtual void Draw(){
            BasicDraw(this.spriteBatch);
        }

        /// <summary>
        /// The bare skeleton of the draw method. Should be used in every context in which a custom spriteBatch is used (== everywhere except in Wrapper)
        /// </summary>
        public virtual void BasicDraw(SpriteBatch spriteBatch){}

        ///<summary>
        ///Resizes the texture to the size of the sprite, so that there is no need to resize it every frame; should improve performance.
        ///</summary>
        public virtual void DrawMiddleTexture(){
            if(midWidth!=width || midHeight!=height){
                // Console.WriteLine("Called a sprite resize: w="+midWidth+"/"+width+" h="+midHeight+"/"+height);

                midWidth=width;
                midHeight=height;

                // Console.WriteLine("After var update: w="+midWidth+"/"+width+" h="+midHeight+"/"+height);

                RenderTarget2D renderTarget = new RenderTarget2D(spriteBatch.GraphicsDevice,width,height);
                Utilities.DrawOntoTarget(renderTarget,this,spriteBatch);
                if (middleTexture != null) 
                {
                    middleTexture.Dispose();
                }
                middleTexture = renderTarget;
            }
        }
    
        /// <summary>
        /// Completely handles removal of sprite from the wrapper it is linked to, by using <c>Wrapper.Remove(SpriteBase)</c>
        /// </summary>
        public void Remove(){
            if(wrapper!=null){
                wrapper.Remove(this);
            }
        }

        /// <summary>
        /// Unlink all the links to and from all the <c>LinkedVariable</c>s in this sprite. This calls <c>LinkedVariable.Unlink()</c> for each linked variable.
        /// </summary>
        public virtual void Unlink()
        {
            xVariable.Unlink();
            yVariable.Unlink();
            widthVariable.Unlink();
            heightVariable.Unlink();
            depthVariable.Unlink();
        }

        //TODO: make a method which takes a rectangle class to check collisions, so that it is more linear and compatible with actual game objects
        public bool CollidesWith(int x, int y){
            if(collisionRectangle==null){
                return false;
            }
            return collisionRectangle.CollidesWith(x,y); //This uses the collision rectangle to check collisions, so that delegates can be used.
        }

        ///<summary>
        ///Checks if the sprite would be clicked at the given coordinates. Similar to <see cref="CollidesWith(int, int)"/> but also checks precisely in case <see cref="precise"/> is <c>true</c>.
        ///</summary>
        public bool Clicked(int x, int y){
            return ((precision==PrecisionSetting.Collision) && this.CollidesWith(x, y)) || ((precision==PrecisionSetting.ActualTexture) && CollidesWith(x, y) && GetPixelNotRelative(x,y).A != 0) || ((precision==PrecisionSetting.ApproxTexture) && CollidesWith(x,y) && GetPixelNotRelative(x, y).A != 0);
        }

        ///<summary>
        ///Draws this and only this texture to a new texture of the size of the screen.
        ///</summary>
        private Texture2D IsolateTexture(){
            RenderTarget2D renderTarget=new RenderTarget2D(spriteBatch.GraphicsDevice,spriteBatch.GraphicsDevice.Viewport.Width,spriteBatch.GraphicsDevice.Viewport.Width);
            Utilities.DrawOntoTarget(renderTarget,this,spriteBatch);
            return renderTarget;
        }
    
        /// <summary>
        /// Draws this and only this texture to a new texture of the size of this sprite (width,height).
        /// <para>!!The given texture should then be disposed of using <c>Texture2D.Dispose()</c>!!</para>
        /// </summary>
        /// <param name="realTexture">If true, the texture is not resized to its present (width,height) and instead keeps its original dimensions.</param>
        /// <returns>The drawn texture on a trasparent background if implemented. Otherwise, it returns a trasparent texture.</returns>
        protected virtual Texture2D SingleTexture(bool realTexture=false)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(spriteBatch.GraphicsDevice, width, height);
            return renderTarget;
        }

        /// <summary>
        /// Get the color of a pixel at the given coordinates, relatively to the whole screen.
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>The color of the given pixel if the coordinates are in range; a black color with full opacity otherwise.</returns>
        public Color GetPixelNotRelative(int x, int y, bool approximate=false)
        {
            int relativeX = x - this.x;
            int relativeY = y - this.y;

            if (approximate)
            {
                relativeX = (relativeX * texture.Width) / width;
                relativeY = (relativeY * texture.Height) / height;
            }
            Texture2D singleTexture = SingleTexture(approximate);
            Color pixel = singleTexture.GetPixel(relativeX, relativeY);
            singleTexture.Dispose();
            return pixel;
        }

        private void UpdateCollisionRectangle()
        {
            if (this.leftClickDelegate == null && this.middleClickDelegate == null && this.rightClickDelegate == null && this.wheelHoverDelegate == null && this.hoverDelegate == null)
            {
                return;
            }
            else if(collisionRectangle==null)
            {
                collisionRectangle = new CollisionRectangle(this);
            }
        }
    }

    public enum PrecisionSetting
    {
        Collision,
        ApproxTexture,
        ActualTexture
    }
}