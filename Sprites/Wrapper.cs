using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace FCSG{
    public class Wrapper{
        private List<SpriteObject> sprites;
        private SpriteBatch spriteBatch;
        public LayerGroup leftClick;
        public LayerGroup middleClick;
        public LayerGroup rightClick;
        public LayerGroup wheelHover;
        public LayerGroup hover;

        public SpriteBatchParameters spriteBatchParams;

        public Wrapper(SpriteBatch spriteBatch, SpriteBatchParameters spriteBatchParams=null){
            this.spriteBatch = spriteBatch;
            if(this.spriteBatchParams==null){
                this.spriteBatchParams=new SpriteBatchParameters(sortMode:SpriteSortMode.FrontToBack,samplerState:SamplerState.PointClamp);
            }else{
                this.spriteBatchParams=spriteBatchParams;
            }
            sprites = new List<SpriteObject>();
            leftClick = new LayerGroup();
            middleClick = new LayerGroup();
            rightClick = new LayerGroup();
            wheelHover = new LayerGroup();
            hover = new LayerGroup();
        }

        /// <summary>
        /// Completely handles sprite addition (Adds to draw and click groups).
        /// </summary>
        public void Add(SpriteBase sprite){
            if(sprite.leftClickDelegate!=null){
                leftClick.Add(sprite);
            }
            if(sprite.middleClickDelegate!=null){
                middleClick.Add(sprite);
            }
            if(sprite.rightClickDelegate!=null){
                rightClick.Add(sprite);
            }
            if(sprite.wheelHoverDelegate!=null){
                wheelHover.Add(sprite);
            }
            if(sprite.hoverDelegate!=null){
                hover.Add(sprite);
            }
            sprite.wrapper=this;
            sprites.Add(sprite);
        }

        /// <summary>
        /// Completely handles sprite removal (Removes from draw and click groups).
        /// </summary>
        public void Remove(SpriteBase sprite){
            leftClick.Remove(sprite);
            middleClick.Remove(sprite);
            rightClick.Remove(sprite);
            wheelHover.Remove(sprite);
            hover.Remove(sprite);
            sprite.wrapper=null;
            sprites.Remove(sprite);
        }

        public void Draw(){
            //spriteBatch.Begin(sortMode:SpriteSortMode.FrontToBack,samplerState:SamplerState.PointClamp); //TODO: Should add options
            spriteBatch.Begin(this.spriteBatchParams);
            foreach(SpriteObject sprite in sprites){
                sprite.Draw();
            }
            spriteBatch.End();
        }
    
        public void Click(Clicks click,int x, int y){
            LayerGroup layers=null;
            switch(click){
                case Clicks.Left:
                    layers=leftClick;
                    break;
                case Clicks.Middle:
                    layers=middleClick;
                    break;
                case Clicks.Right:
                    layers=rightClick;
                    break;
                case Clicks.WheelHover:
                    layers=wheelHover;
                    break;
                case Clicks.Hover:
                    layers=hover;
                    break;
            }
            bool nextElement=true;
            
            layers.layerCount=0;
            while(layers.layerCount<layers.objects.Count){
                SpriteBase sprite=((List<SpriteBase>)layers)[(int)layers.layerCount];
                nextElement=sprite.Clicked(x,y,click); //Everything should be correctly handled in each sprite
                if(!nextElement){
                    break;
                }
                layers.layerCount++;
            }
            layers.layerCount=null; //Sets layerCount back to null so that it isn't considered in eventual other chunks of code
        }
    }
}