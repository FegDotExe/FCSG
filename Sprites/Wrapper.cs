using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;

namespace FCSG{
    public class Wrapper{
        private List<SpriteObject> sprites;
        private SpriteBatch spriteBatch;

        public ClickLayerGroup[] clickLayerGroup = new ClickLayerGroup[1+(int)Enum.GetValues(typeof(Clicks)).Cast<Clicks>().Last<Clicks>()];
        public ClickLayerGroup leftClick { get { return clickLayerGroup[(int)Clicks.Left]; } }
        public ClickLayerGroup middleClick { get { return clickLayerGroup[(int)Clicks.Middle]; } }
        public ClickLayerGroup rightClick { get { return clickLayerGroup[(int)Clicks.Right]; } }
        public ClickLayerGroup wheelHover { get { return clickLayerGroup[(int)Clicks.WheelHover]; } }
        public ClickLayerGroup hover { get { return clickLayerGroup[(int)Clicks.Hover]; } }

        public SpriteBatchParameters spriteBatchParams;

        public Wrapper(SpriteBatch spriteBatch, SpriteBatchParameters spriteBatchParams=null){
            this.spriteBatch = spriteBatch;
            if(this.spriteBatchParams==null){
                this.spriteBatchParams=new SpriteBatchParameters(sortMode:SpriteSortMode.FrontToBack,samplerState:SamplerState.PointClamp);
            }else{
                this.spriteBatchParams=spriteBatchParams;
            }
            sprites = new List<SpriteObject>();
            clickLayerGroup[(int)Clicks.Left] = new ClickLayerGroup(Clicks.Left);
            clickLayerGroup[(int)Clicks.Middle] = new ClickLayerGroup(Clicks.Middle);
            clickLayerGroup[(int)Clicks.Right] = new ClickLayerGroup(Clicks.Right);
            clickLayerGroup[(int)Clicks.WheelHover] = new ClickLayerGroup(Clicks.WheelHover);
            clickLayerGroup[(int)Clicks.Hover] = new ClickLayerGroup(Clicks.Hover);
        }

        /// <summary>
        /// Completely handles sprite addition (Adds to draw and click groups).
        /// </summary>
        public void Add(SpriteBase sprite){
            for (int i = 0; i < clickLayerGroup.Length; i++) { //Adds the sprite to the various ClickLayerGroups
                if (sprite.clickArray[i] != null)
                {
                    clickLayerGroup[i].Add(sprite);
                }
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
    
        public void Down(Clicks click,int x, int y){
            ClickLayerGroup layerGroup = clickLayerGroup[(int)click];
            layerGroup.Down(x, y);
        }

        public void Up(Clicks click, int x, int y)
        {
            ClickLayerGroup layerGroup = clickLayerGroup[(int)click];
            layerGroup.Up(x, y);
        }
    }
}