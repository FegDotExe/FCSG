using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FCSG
{
    public class ClickLayerGroup
    {
        private LayerGroup upLayerGroup;
        private List<SpriteBase> downList;
        private int clickType;

        public ClickLayerGroup(Clicks clickType)
        {
            upLayerGroup = new LayerGroup();
            downList = new List<SpriteBase>();
            this.clickType = (int)clickType;
        }

        public void Add(SpriteBase sb)
        {
            upLayerGroup.Add(sb);
        }
        public void Remove(SpriteBase sb)
        {
            upLayerGroup.Remove(sb);
            downList.Remove(sb);
        }

        /// <summary>
        /// Presses down all the elements in this ClickLayerGroup. This also presses up all the elements which were previously pressed down and are not such anymore.
        /// </summary>
        /// <param name="x">The x position of the click</param>
        /// <param name="y">The y position of the click</param>
        public void Down(int x, int y)
        {
            //Stopwatch sw = Stopwatch.StartNew();

            bool nextElement = true;
            List<SpriteBase> newlyClicked = new List<SpriteBase>();
            SpriteBase[] upList = upLayerGroup.ToArray();
            foreach (SpriteBase sb in upList)
            {
                ClickSetting clickSetting = sb.clickArray[clickType];

                if ((clickSetting != null) && (sb.Clicked(x,y)))
                {
                    bool alreadyDown = clickSetting.isDown;
                    nextElement = clickSetting.Down(sb, x, y); //There should be checks for down clicking.
                    newlyClicked.Add(sb);
                    if (!nextElement)
                    {
                        //Debug.WriteLine("Stopped");
                        break;
                    }
                }
            }

            foreach(SpriteBase sb in newlyClicked)
            {
                downList.Remove(sb);
            }
            foreach(SpriteBase sb in downList.ToArray())
            {
                ClickSetting clickSetting = sb.clickArray[clickType];

                clickSetting.Up(sb, x, y);
                downList.Remove(sb);
            }
            foreach (SpriteBase sb in newlyClicked)
            {
                downList.Add(sb);
            }

            //sw.Stop();
            //Debug.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);
        }

        public void Up(int x, int y)
        {
            foreach (SpriteBase sb in downList.ToArray())
            {
                ClickSetting clickSetting = sb.clickArray[clickType];

                clickSetting.Up(sb, x, y);
                downList.Remove(sb);
            }
        }
    }
}
