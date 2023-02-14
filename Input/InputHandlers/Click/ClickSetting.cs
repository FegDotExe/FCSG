using System;
using System.Collections.Generic;
using System.Text;

namespace FCSG
{
    public class ClickSetting
    {
        private ClickDelegate? downClickDelegate=null;
        private ClickDelegate? upClickDelegate = null;
        private bool active;
        private bool latestReturn;
        private bool continuous;
        /// <summary>
        /// Whether the click is pressed
        /// </summary>
        public bool isDown
        {
            get { return active; }
        }
        /// <summary>
        /// Whether the click is not pressed
        /// </summary>
        public bool isUp
        {
            get { return !active; }
        }

        public ClickSetting(ClickDelegate? downClickDelegate, ClickDelegate? upClickDelegate=null, bool continuous=false)
        {
            this.downClickDelegate = downClickDelegate;
            this.upClickDelegate = upClickDelegate;
            this.continuous = continuous;
        }

        /// <summary>
        /// Tells the click that it has been pressed down. If the click is continuous, this will keep calling the delegate; otherwise, this will only set it to active
        /// </summary>
        /// <param name="sb">The sprite base the click should happen onto</param>
        /// <param name="x">The x value of the click</param>
        /// <param name="y">The y value of the click</param>
        /// <returns>The result of the click delegate if not null and the click was effective; <c>true</c> otherwise</returns>
        public bool Down(SpriteBase sb, int x, int y)
        {
            if (downClickDelegate == null)
            {
                return true;
            }
            if ((!active) || continuous) //This activates in every case unless continuous is false and active is true
            {
                active = true;
                latestReturn = downClickDelegate(sb, x, y);
                return latestReturn;
            }else if(active && !continuous)
            {
                return latestReturn;
            }
            return true;
        }
        /// <summary>
        /// Tells the click that it has been released.
        /// </summary>
        /// <param name="sb">The sprite base the click should happen onto</param>
        /// <param name="x">The x value of the click</param>
        /// <param name="y">The y value of the click</param>
        public void Up(SpriteBase sb, int x, int y)
        {
            if(upClickDelegate == null)
            {
                active = false;
                return;
            }
            if (active)
            {
                upClickDelegate(sb, x, y);
                active = false;
            }
        }
    }
}
