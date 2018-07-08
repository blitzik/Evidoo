using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.EventArguments
{
    public class WorkedTimeEventArgs : EventArgs
    {
        public Time Start { get; }
        public Time End { get; }
        public Time LunchStar { get; }
        public Time LunchEnd { get; }
        public Time OtherHours { get; }


        public WorkedTimeEventArgs(Time start, Time end, Time lunchStar, Time lunchEnd, Time otherHours)
        {
            Start = start;
            End = end;
            LunchStar = lunchStar;
            LunchEnd = lunchEnd;
            OtherHours = otherHours;
        }
    }
}
