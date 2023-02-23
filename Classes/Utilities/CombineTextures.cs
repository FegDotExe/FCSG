using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FCSG{
    public partial class Utilities{
        public static Texture2D CombineTextures(List<Texture2D> textures, SpriteBatch spriteBatch){
            return CombineTextures(textures.ToArray(), spriteBatch);
        }

        /// <summary>
        /// Combine the given array of textures. The size of the first texture is used for the output result.
        /// </summary>
        /// <param name="textures">The array of textures to be combined</param>
        /// <param name="spriteBatch">The spritebatch which will be used to draw the sprites</param>
        /// <returns>The various sprites, combined.</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static Texture2D CombineTextures(Texture2D[] textures, SpriteBatch spriteBatch)
        {
            if (textures.Length == 0)
            {
                throw new System.ArgumentException("The list of textures is empty");
            }
            RenderTarget2D renderTarget = new RenderTarget2D(spriteBatch.GraphicsDevice, textures[0].Width, textures[0].Height);

            DrawOntoTarget(renderTarget, textures, spriteBatch);

            return renderTarget;
        }
    }
}