using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FCSG{
    public static class Texture2DExtension{
        /// <summary>
        /// Gets the color of the pixel at the specified position.
        /// </summary>
        /// <param name="texture">The texture which pixel is to get</param>
        /// <param name="x">X value of the pixel</param>
        /// <param name="y">Y value of the pixcel</param>
        /// <returns>The color of the pixel at the specified coordinate if (x,y) is in range; A black color with 255 alpha otherwise.</returns>
        public static Color GetPixel(this Texture2D texture, int x, int y){
            if (x < 0 || y < 0 || x >= texture.Width || y >= texture.Height) {
                return new Color(0, 0, 0, 255);
            }

            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(pixels);
            //Console.WriteLine("GetPixel; x: " + x + " y: " + y+"; color:"+pixels[x + (y * texture.Width)]);
            return pixels[x + (y * texture.Width)];
        }
    }
}