using System;
using System.Collections.Generic;

namespace FCSG{
    /// <summary>
    /// A function which performs actions depending of the given time. The time ranges from 0 to 1, where 0 is "start" and 1 is "end".
    /// </summary>
    public delegate void TimeFunction(double time);
    public class TimeEvent{
        private long totalTime;
        private TimeFunction normalFunction;
        private TimeFunction finalFunction;
        private long startTime;
        private int deltaTime;// Initial delta time used to make the first call not be 0 (which would always happen with deltaTime=0)
        public bool isFinished{get; private set;}

        private string name; //The name used for the dictionaries
        private Dictionary<string,TimeEvent> timeEvents; //A list in which this time event is stored
        public static Dictionary<string,TimeEvent> defaultTimeEvents; //Default list of time events

        /// <param name="totalSeconds">The time in seconds this event will last</param>
        /// <param name="normalFunction">The function to call when the event is not finished</param>
        /// <param name="finalFunction">The function to call when the event is finished</param>
        /// <param name="deltaTime">The initial elapsed time. It is usualy determined using <c>GameClock.Elapsed</c></param>
        /// <param name="name">The name to be assigned to this time event inside of a timeEvents dictionary</param>
        /// <param name="timeEvents">A dictionary in which to insert this time event. It is required if the time event should not be replaced</param>
        /// <param name="replace">If the event should be replaced inside of a timeEvents dictionary</param>
        /// <summary>
        /// Construct a new TimeEvent.
        /// </summary>
        public TimeEvent(double totalSeconds, TimeFunction normalFunction, TimeFunction finalFunction=null, int deltaTime=0, Dictionary<string,TimeEvent> timeEvents=null, string name=null, bool replace=false){
            this.totalTime=(long)(totalSeconds*1000);
            this.normalFunction=normalFunction;
            this.finalFunction=finalFunction;
            startTime=DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.deltaTime=deltaTime;
            if(name!=null && timeEvents!=null){
                this.timeEvents=timeEvents;
                this.name=name;
                if(replace || !timeEvents.ContainsKey(name)){ //Add to the dictionary accordingly to presence and replace
                    timeEvents.Add(name,this);
                }
            }else if(name!=null && TimeEvent.defaultTimeEvents!=null){
                this.timeEvents=TimeEvent.defaultTimeEvents;
                this.name=name;
                if(replace || !timeEvents.ContainsKey(name)){ //Add to the dictionary accordingly to presence and replace
                    timeEvents.Add(name,this);
                }
            }
        }

        public void RunFunction(){
            long currentTime=DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long timeElapsed=currentTime-startTime+deltaTime;
            if(timeElapsed>=totalTime){
                if(finalFunction!=null){
                    finalFunction(1);
                }
                else{
                    normalFunction(((double)timeElapsed)/(double)totalTime);
                }
                isFinished=true;
            }else{
                normalFunction(((double)timeElapsed)/(double)totalTime);
            }
        }

        /// <summary>
        /// Correctly run all the time events in this dictionary
        /// </summary>
        /// <param name="timeEvents">The dictionary of time events</param>
        public static void RunAll(Dictionary<string,TimeEvent> timeEvents)
        {
            foreach(KeyValuePair<string,TimeEvent> kvp in timeEvents)
            {
                kvp.Value.RunFunction();
                if (kvp.Value.isFinished)
                {
                    timeEvents.Remove(kvp.Key);
                }
            }
        }
    }
}