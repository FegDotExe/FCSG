using System.Collections.Generic;
using System.Diagnostics;

namespace FCSG{
    /// <summary>
    /// A class which stores sprites in a list, keeping them ordered by their depth or by a custom delegate, with the ones with higher values in front
    /// </summary>
    public class LayerGroup{
        public int? layerCount=null;
        public List<SpriteBase> objects;
        private DoubleSpriteBaseDelegate comparer;
        private static DoubleSpriteBaseDelegate defaultComparer = (SpriteBase sb) => (float)sb.depth;
        /// <summary>
        /// Construct a new LayerGroup which will sort its sprites by their depth
        /// </summary>
        public LayerGroup(){
            objects=new List<SpriteBase>();
            comparer=defaultComparer;
        }
        /// <summary>
        /// Construct a new LayerGroup which will sort its sprites by the given delegate
        /// </summary>
        public LayerGroup(DoubleSpriteBaseDelegate comparer){
            objects=new List<SpriteBase>();
            this.comparer=comparer;
        }
        /// <summary>
        /// Adds the given sprite to the LayerGroup, keeping it ordered by its depth
        /// </summary>
        public void Add(SpriteBase sprite){
            this.Add(sprite,this.objects);
            if(layerCount!=null && (sprite==objects[(int)layerCount]||objects.IndexOf(sprite)<(int)layerCount)){ //The "if" is placed after the addition so that objects.IndexOf(sprite) works without trouble
                layerCount++;
            }
        }
        /// <summary>
        /// Adds the given sprite to the LayerGroup, keeping it ordered by its depth using a variant of insertion sort
        /// </summary>
        private void Add(SpriteBase sprite, List<SpriteBase> objects){
            objects.Add(sprite);
            int i = objects.Count - 2;

            while(i>=0 && comparer(objects[i])<comparer(sprite)){
                objects[i+1] = objects[i];
                i--;
            }

            //Debug.WriteLine(i + "/" + (objects.Count - 1));

            objects[i + 1] = sprite;
        }

        /// <summary>
        /// Removes the given sprite from the LayerGroup
        /// </summary>
        public void Remove(SpriteBase sprite){
            if(layerCount!=null && (sprite==objects[(int)layerCount]||objects.IndexOf(sprite)<(int)layerCount)){
                layerCount--;
            }
            objects.Remove(sprite);
        }

        /// <summary>
        /// Reorders all the objects in the group
        /// </summary>
        public void Update(){
            List<SpriteBase> newObjects=new List<SpriteBase>();
            foreach(SpriteBase sprite in objects){
                this.Add(sprite,newObjects);
            }
            objects=newObjects;
        }

        public static implicit operator List<SpriteBase>(LayerGroup group){
            return group.objects;
        }
        public SpriteBase[] ToArray()
        {
            return objects.ToArray();
        }
    }
}