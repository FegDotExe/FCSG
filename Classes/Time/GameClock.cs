using System;

namespace FCSG{
    /// <summary>
    /// A class used to handle in-game time and <c>TimeEvent</c>s.
    /// After constructing this class, it should be updated through <c>GameClock.Update()</c> at the beginning of every <c>Game.Draw()</c> call.
    /// </summary>
    public class GameClock{
        private long startTime;
        private long lastTime;
        /// <summary>
        /// How much time passed since the object was constructed or <c>GameClock.Update()</c> was called.
        /// This is generally used in the <c>deltaTime</c> argument of the <c>TimeEvent</c> constructor.
        /// </summary>
        public long Elapsed{
            get{
                return DateTimeOffset.Now.ToUnixTimeMilliseconds()-lastTime;
            }
        }

        /// <summary>
        /// Construct a new <c>GameClock</c>, setting its <c>startTime</c> at the current time.
        /// </summary>
        public GameClock(){
            startTime=DateTimeOffset.Now.ToUnixTimeMilliseconds();
            lastTime=startTime;
        }

        /// <summary>
        /// Set the time of the clock to the current time. Should be placed at the beginning of <c>Game.Draw()</c>
        /// </summary>
        public void Update(){
            long currentTime=DateTimeOffset.Now.ToUnixTimeMilliseconds();
            lastTime=currentTime;
        }
    }
}