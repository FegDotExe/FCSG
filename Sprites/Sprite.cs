using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace FCSG{
    ///<summary>
    ///A class which rapresents a sprite
    ///</summary>
    public class Sprite : SpriteBase{
        public Sprite(
            SpriteParameters spriteParameters
        ) : base(
            spriteParameters: spriteParameters
        ){
            this.texture = spriteParameters.texture;

            if(this.texture==null){
                throw new ArgumentException(message: "The texture of the sprite is null");
            }
        }
        public override void Draw(){
            BasicDraw(this.spriteBatch);
        }

        public override void BasicDraw(SpriteBatch spriteBatch)
        {
            if(draw){
                spriteBatch.Draw(texture, new Rectangle(this.x,this.y,this.width,this.height),null,color,rotation,origin,effects,(float)depth);
            }
        }

        protected override Texture2D SingleTexture(bool actualTexture=false)
        {
            RenderTarget2D renderTarget;
            int usedWidth;
            int usedHeight;
            if (!actualTexture)
            {
                renderTarget = new RenderTarget2D(this.spriteBatch.GraphicsDevice, width, height);
                usedWidth = width;
                usedHeight = height;
            }
            else
            {
                renderTarget = new RenderTarget2D(this.spriteBatch.GraphicsDevice, texture.Width, texture.Height);
                usedWidth = texture.Width;
                usedHeight = texture.Height;
            }
            SpriteBatch spriteBatch = new SpriteBatch(this.spriteBatch.GraphicsDevice);
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);// TODO: make this optional 

            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0,0,usedWidth,usedHeight),null,color,rotation,origin,effects,(float)depth);
            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            return renderTarget;
        }
    }
}