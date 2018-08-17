using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Domain
{
    public class Week : PropertyChangedBase
    {
        private int _weekNumber;
        public int WeekNumber
        {
            get { return _weekNumber; }
            private set { _weekNumber = value; }
        }


        private bool _isCurrent;
        public bool IsCurrent
        {
            get { return _isCurrent; }
        }


        private IReadOnlyCollection<DayItem> _readOnlyDayItemsCollection;
        private List<DayItem>_dayItems;
        public IReadOnlyCollection<DayItem> DayItems
        {
            get
            {
                if (_readOnlyDayItemsCollection == null) {
                    _readOnlyDayItemsCollection = new ReadOnlyCollection<DayItem>(_dayItems);
                }
                return _readOnlyDayItemsCollection;
            }
        }


        public Week(int weekNumber, bool isCurrent)
        {
            _weekNumber = weekNumber;
            _isCurrent = isCurrent;
            _dayItems = new List<DayItem>();
        }


        public void AddDayItem(DayItem dayItem)
        {
            if (dayItem.Week != WeekNumber) {
                throw new ArgumentException();
            }

            _dayItems.Add(dayItem);
        }
    }
}
