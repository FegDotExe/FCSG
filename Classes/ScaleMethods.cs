namespace FCSG{
    public class ScaleMethods{
        /// <summary>
        /// Returns the height of the sprite's texture after fitting it into the given rectangle of width=maxX and height=maxY while retaining proportions. If used in a LinkedVariable, the LinkedVariable should be sensitive towards changes for the given maxX and maxY.
        /// </summary>
        public static int FitHeight(SpriteBase spriteBase, int maxX, int maxY){
            int newX = (spriteBase.texture.Width*maxY)/spriteBase.texture.Height;
            if(newX <= maxX){
                return maxY;
            }
            return (spriteBase.texture.Height*maxX)/spriteBase.texture.Width;
        }

        /// <summary>
        /// Returns the width of the sprite's texture after fitting it into the given rectangle of width=maxX and height=maxY while retaining proportions. If used in a LinkedVariable, the LinkedVariable should be sensitive towards changes for the given maxX and maxY.
        /// </summary>
        public static int FitWidth(SpriteBase spriteBase, int maxX, int maxY){
            int newY = (spriteBase.texture.Height*maxX)/spriteBase.texture.Width;
            if(newY <= maxY){
                return maxX;
            }
            return (spriteBase.texture.Width*maxY)/spriteBase.texture.Height;
        }

        /// <summary>
        /// Returns the centered x position of a sprite contained in a rectangle of width=maxX. If used in a LinkedVariable, the LinkedVariable should be sensitive towards changes for the given maxX.
        /// </summary>
        public static int CenterX(SpriteBase spriteBase, int maxX){
            return (maxX - spriteBase.width)/2;
        }

        /// <summary>
        /// Get the coordinate value which centers the given base value in the given max value.
        /// </summary>
        /// <param name="baseValue">The value of the object which is to be centered</param>
        /// <param name="max">The length of the space in which the object needs to be centered</param>
        /// <returns>The coordinate value of the centered object</returns>
        public static int Center(int baseValue, int max)
        {
            return (max - baseValue)/2;
        }

        /// <summary>
        /// Returns the centered y position of a sprite contained in a rectangle of height=maxY. If used in a LinkedVariable, the LinkedVariable should be sensitive towards changes for the given maxY.
        /// </summary>
        public static int CenterY(SpriteBase spriteBase, int maxY){
            return (maxY - spriteBase.height)/2;
        }
    }
}