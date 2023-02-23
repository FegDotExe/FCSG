using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Linq;

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

        public static Texture2D GetOutline(this Texture2D texture, int minGaps, Color? outlineColor=null, bool keepBase=false)
        {
            Color[] basePixels = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(basePixels);

            Color[,] sqPixels = new Color[texture.Width, texture.Height];
            for (int i = 0; i < sqPixels.GetLength(0); i++)
            {
                for (int j = 0; j < sqPixels.GetLength(1); j++)
                {
                    sqPixels[i, j] = basePixels[i + (sqPixels.GetLength(0) * j)];
                }
            }

            //Color[,] outPixels = new Color[texture.Width, texture.Height];

            for (int i=0; i<sqPixels.GetLength(0); i++)
            {
                for(int j=0; j<sqPixels.GetLength(1); j++)
                {
                    //Debug.WriteLine("("+i+"x"+j+")-"+sqPixels[i, j]);
                    int gaps = 0;
                    if (i - 1 < 0 || sqPixels[i - 1, j].A == 0)
                    {
                        gaps++;
                    }
                    if (j - 1 < 0 || sqPixels[i, j - 1].A == 0)
                    {
                        gaps++;
                    }
                    if (i + 1 >= sqPixels.GetLength(0) || sqPixels[i + 1, j].A == 0)
                    {
                        gaps++;
                    }
                    if (j + 1 >= sqPixels.GetLength(1) || sqPixels[i, j + 1].A == 0)
                    {
                        gaps++;
                    }

                    if (sqPixels[i,j].A!=0 && gaps>=minGaps)
                    {
                        //Debug.WriteLine("(" + i + "x" + j + ") is an outline");
                        basePixels[i + (sqPixels.GetLength(0) * j)] = new Color(255, 255, 255, 255);

                        if (keepBase)
                        {
                            basePixels[i + (sqPixels.GetLength(0) * j)] = sqPixels[i, j];
                        }
                        else if (outlineColor == null)
                        {
                            throw new ArgumentNullException("The outline color cannot be null if keepBase is false.");
                        }
                        else
                        {
                            basePixels[i + (sqPixels.GetLength(0) * j)] = (Color)outlineColor;
                        }
                    }
                    else
                    {
                        basePixels[i + (sqPixels.GetLength(0) * j)] = new Color(0, 0, 0, 0);
                    }
                }
            }

            Texture2D output=new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
            
            output.SetData<Color>(basePixels);
            return output;
        }
    }
}