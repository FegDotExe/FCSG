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
        public override void Draw(bool drawMiddle=true){
            BasicDraw(this.spriteBatch,drawMiddle);
        }

        public override void BasicDraw(SpriteBatch spriteBatch, bool drawMiddle = true)
        {
            if(draw){
                drawMiddle=false;
                if(drawMiddle==true){
                    DrawMiddleTexture();
                }
                spriteBatch.Draw(texture, new Rectangle(this.x,this.y,this.width,this.height),null,color,rotation,origin,effects,(float)depth);
            }
        }
    }
}