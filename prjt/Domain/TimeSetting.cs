using prjt.Exceptions;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Domain
{
    public class TimeSetting
    {
        private readonly Time _start = new Time();
        public Time Start
        {
            get { return _start; }
        }


        private readonly Time _end = new Time();
        public Time End
        {
            get { return _end; }
        }


        private readonly Time _lunchStart = new Time();
        public Time LunchStart
        {
            get { return _lunchStart; }
        }


        private readonly Time _lunchEnd = new Time();
        public Time LunchEnd
        {
            get { return _lunchEnd; }
        }


        private readonly Time _otherHours = new Time();
        public Time OtherHours
        {
            get { return _otherHours; }
        }


        public Time LunchHours
        {
            get { return LunchEnd - LunchStart; }
        }


        public Time WorkedHoursWithLunch
        {
            get { return End - Start; }
        }


        public Time WorkedHours
        {
            get { return WorkedHoursWithLunch - LunchHours; }
        }


        public Time TotalWorkedHours
        {
            get { return WorkedHours + OtherHours; }
        }


        public bool HasNoTime
        {
            get
            {
                if (Start == 0 && End == 0 && LunchStart == 0 && LunchEnd == 0 && OtherHours == 0) {
                    return true;
                }

                return false;
            }
        }


        public TimeSetting() { }


        public TimeSetting(TimeSetting timeSetting)
        {
            _start = new Time(timeSetting.Start);
            _end = new Time(timeSetting.End);
            _lunchStart = new Time(timeSetting.LunchStart);
            _lunchEnd = new Time(timeSetting.LunchEnd);
            _otherHours = new Time(timeSetting.OtherHours);
        }


        public TimeSetting(Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            CheckTime(start, end, lunchStart, lunchEnd, otherHours);

            _start = new Time(start);
            _end = new Time(end);
            _lunchStart = new Time(lunchStart);
            _lunchEnd = new Time(lunchEnd);
            _otherHours = new Time(otherHours);
        }


        public TimeSetting(int start, int end, int lunchStart, int lunchEnd, int otherHours)
        {
            CheckTime(start, end, lunchStart, lunchEnd, otherHours);

            _start = new Time(start);
            _end = new Time(end);
            _lunchStart = new Time(lunchStart);
            _lunchEnd = new Time(lunchEnd);
            _otherHours = new Time(otherHours);
        }


        public bool isEqual(TimeSetting timeSetting)
        {
            if (Start != timeSetting.Start) { return false; }
            if (End != timeSetting.End) { return false; }
            if (LunchStart != timeSetting.LunchStart) { return false; }
            if (LunchEnd != timeSetting.LunchEnd) { return false; }
            if (OtherHours != timeSetting.OtherHours) { return false; }

            return true;
        }


        public static void CheckTime(Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            if (!(start == 0 && end == 0 && lunchStart == 0 && lunchEnd == 0 && otherHours == 0)) {
                if (start >= end) {
                    throw new WorkedHoursRangeException();
                }

                if (!(lunchStart == 0 && lunchEnd == 0)) {
                    if (lunchStart >= lunchEnd) {
                        throw new LunchHoursRangeException();
                    }

                    if (lunchStart < start || lunchEnd > end) {
                        throw new LunchHoursOutOfWorkedHoursRangeException();
                    }
                }

                if ((end - start - (lunchEnd - lunchStart) + otherHours) < 0) {
                    throw new OtherHoursException();
                }
            }
        }


        public static void CheckTime(int start, int end, int lunchStart, int lunchEnd, int otherHours)
        {
            if (!(start == 0 && end == 0 && lunchStart == 0 && lunchEnd == 0 && otherHours == 0)) {
                if (start >= end) {
                    throw new WorkedHoursRangeException();
                }

                if (!(lunchStart == 0 && lunchEnd == 0)) {
                    if (lunchStart >= lunchEnd) {
                        throw new LunchHoursRangeException();
                    }

                    if (lunchStart < start || lunchEnd > end) {
                        throw new LunchHoursOutOfWorkedHoursRangeException();
                    }
                }

                if ((end - start - (lunchEnd - lunchStart) + otherHours) < 0) {
                    throw new OtherHoursException();
                }
            }
        }

    }
}
