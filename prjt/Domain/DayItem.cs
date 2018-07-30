using Caliburn.Micro;
using prjt.Utils;
using System;
using System.Collections.Generic;

namespace prjt.Domain
{
    public class DayItem : PropertyChangedBase
    {
        private int _year;
        public int Year
        {
            get { return _year; }
        }


        private int _month;
        public int Month
        {
            get { return _month; }
        }


        private int _day;
        public int Day
        {
            get { return _day; }
        }


        private int _week;
        public int Week
        {
            get { return _week; }
        }


        public string ShortDayName
        {
            get
            {
                return Utils.Date.DaysOfWeek[(int)Date.DayOfWeek].Substring(0, 2);
            }
        }


        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
            private set
            {
                _locality = value;
                NotifyOfPropertyChange(() => Locality);
            }
        }


        private TimeSetting _timeSetting;
        public TimeSetting TimeSetting
        {
            get { return _timeSetting; }
            private set
            {
                _timeSetting = value;
                NotifyOfPropertyChange(() => TimeSetting);
            }
        }


        private ListingItem _listingItem;
        public ListingItem ListingItem
        {
            get { return _listingItem; }
            private set { _listingItem = value; }
        }


        public bool CanBeRemoved
        {
            get { return _listingItem != null; }
        }


        public bool CanBeCopiedDown
        {
            get { return _listingItem != null && Day < Listing.DaysInMonth; }
        }


        public bool IsWeekendDay
        {
            get { return (Date.DayOfWeek == DayOfWeek.Sunday || Date.DayOfWeek == DayOfWeek.Saturday); }
        }


        public bool IsCurrentDay
        {
            get
            {
                DateTime now = DateTime.Now;
                if (now.Day != Date.Day) {
                    return false;
                }

                if (now.Month != Date.Month) {
                    return false;
                }

                if (now.Year != Date.Year) {
                    return false;
                }

                return true;
            }
        }


        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
        }


        public HashSet<string> Localities
        {
            get { return _listing.Localities; }
        }


        public DayItem(Listing listing, int day)
        {
            _listing = listing;
            _year = listing.Year;
            _month = listing.Month;
            _day = day;
            _date = new DateTime(_year, _month, day);
            _week = PrepareWeek(_year, _month, _day);

            ListingItem item = listing.GetItemByDay(day);
            if (item != null) {
                _listingItem = item;
                Locality = item.Locality;
                _timeSetting = item.TimeSetting;
            }
        }


        public void Update(ListingItem item)
        {
            ArgumentException e = new ArgumentException();
            if (!Listing.ContainsItem(item)) {
                throw e;
            }

            if (item.Date.Day != Day) {
                throw e;
            }

            ListingItem = item;

            Locality = item.Locality;
            TimeSetting = item.TimeSetting;

            NotifyPropertiesChanged();
        }


        public bool IsEqual(DayItem dayItem)
        {
            // todo check Listing too?
            if (_listingItem == null || dayItem.ListingItem == null) {
                return false;
            }

            if (!_timeSetting.isEqual(dayItem.TimeSetting)) { return false; }
            if (_locality != dayItem.Locality) { return false; }

            return true;
        }


        public void Reset()
        {
            ListingItem = null;

            Locality = null;
            TimeSetting = null;

            NotifyPropertiesChanged();
        }


        private void NotifyPropertiesChanged()
        {
            NotifyOfPropertyChange(() => CanBeCopiedDown);
            NotifyOfPropertyChange(() => CanBeRemoved);

            NotifyOfPropertyChange(() => ShortDayName);
            NotifyOfPropertyChange(() => Locality);
            NotifyOfPropertyChange(() => ListingItem);
        }


        private int PrepareWeek(int year, int month, int day)
        {
            int weekNumber = prjt.Utils.Date.GetWeekNumber(year, month, day);
            //DateTime now = DateTime.Now;
            return weekNumber;
        }

    }
}
