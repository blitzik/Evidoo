using Caliburn.Micro;
using prjt.EventArguments;
using prjt.Exceptions;
using prjt.Utils;
using Perst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace prjt.Domain
{
    public class Listing : PropertyChangedBase
    {
        private int _indexYear;
        public int _IndexYear
        {
            get { return _indexYear; }
            private set { _indexYear = value; }
        }


        private int _indexMonth;
        public int _IndexMonth
        {
            get { return _indexMonth; }
            private set { _indexMonth = value; }
        }


        public void RefreshIndexes()
        {
            _IndexYear = Year;
            _IndexMonth = Month;
        }


        public bool AreIndexesMatching()
        {
            return _IndexYear == Year && _IndexMonth == Month;
        }


        // -----



        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value)) {
                    _name = "Bez názvu";
                } else {
                    _name = value;
                }
                NotifyOfPropertyChange(() => Name);
            }
        }


        private int _year;
        public int Year
        {
            get { return _year; }
            private set
            {
                if (value < 1) {
                    throw new OutOfRangeException("Only numbers higher than 0 can pass");
                }

                _year = value;
            }
        }


        private int _month;
        public int Month
        {
            get { return _month; }
            private set
            {
                if (value < 1 || value > 12) {
                    throw new OutOfRangeException();
                }
                _month = value;
            }
        }


        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
            set
            {
                _employer?.RemoveListing(this);
                _employer = value;
                _employer?.AddListing(this);
                NotifyOfPropertyChange(() => Employer);
            }
        }


        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            private set { _createdAt = value; }
        }


        public int DaysInMonth
        {
            get
            {
                return DateTime.DaysInMonth(Year, Month);
            }
        }


        [NonSerialized()]
        private IReadOnlyDictionary<int, ListingItem> _readOnlyItemsDictionary;

        private IPersistentMap<int, ListingItem> _items;
        public IReadOnlyDictionary<int, ListingItem> Items
        {
            get
            {
                if (_readOnlyItemsDictionary == null) {
                    _readOnlyItemsDictionary = new ReadOnlyDictionary<int, ListingItem>(_items);
                }
                return _readOnlyItemsDictionary;
            }
        }


        private int _workedDays = 0;
        public int WorkedDays
        {
            get { return _workedDays; }
            private set
            {
                _workedDays = value;
                NotifyOfPropertyChange(() => WorkedDays);
            }
        }


        public delegate void ListingSummaryTimeChangedHandler(object sender, Time changedTime);
        public event ListingSummaryTimeChangedHandler OnSummaryTimeChanged;


        private Time _workedHours;
        public Time WorkedHours
        {
            get { return _workedHours ?? new Time("00:00"); }
            private set
            {
                if (_workedHours != null) {
                    OnSummaryTimeChanged?.Invoke(this, _workedHours);
                }
                _workedHours = value;
                NotifyOfPropertyChange(() => WorkedHours);
            }
        }


        private Time _otherHours;
        public Time OtherHours
        {
            get { return _otherHours ?? new Time("00:00"); }
            set
            {
                if (_otherHours != null) {
                    OnSummaryTimeChanged?.Invoke(this, _otherHours);
                }
                _otherHours = value;
            }
        }


        private Time _lunchHours;
        public Time LunchHours
        {
            get { return _lunchHours ?? new Time("00:00"); }
            set
            {
                if (_lunchHours != null) {
                    OnSummaryTimeChanged?.Invoke(this, _lunchHours);
                }
                _lunchHours = value;
            }
        }


        private Time _totalWorkedHours;
        public Time TotalWorkedHours
        {
            get { return _totalWorkedHours ?? new Time("00:00"); }
            private set
            {
                if (_totalWorkedHours != null) {
                    OnSummaryTimeChanged?.Invoke(this, _totalWorkedHours);
                }
                _totalWorkedHours = value;
                NotifyOfPropertyChange(() => TotalWorkedHours);
            }
        }


        [NonSerialized()]
        private HashSet<string> _localities;
        public HashSet<string> Localities
        {
            get
            {
                if (_localities == null) {
                    _localities = new HashSet<string>();
                    foreach (ListingItem i in Items.Values) {
                        if (!string.IsNullOrEmpty(i.Locality)) {
                            _localities.Add(i.Locality);
                        }
                    }
                }
                return _localities;
            }
        }


        // ----- Summary hours


        private string _vacation;
        public string Vacation
        {
            get { return _vacation; }
            set { _vacation = value; }
        }


        private string _holiday;
        public string Holiday
        {
            get { return _holiday; }
            set { _holiday = value; }
        }


        private string _sicknessHours;
        public string SicknessHours
        {
            get { return _sicknessHours; }
            set { _sicknessHours = value; }
        }


        // -----


        private int? _hourlyWage;
        public int? HourlyWage
        {
            get { return _hourlyWage; }
            set
            {
                if (value != null && value <= 0) {
                    _hourlyWage = null;
                } else {
                    _hourlyWage = value;
                }
                NotifyOfPropertyChange(() => HourlyWage);
            }
        }


        private string _vacationDays;
        public string VacationDays
        {
            get { return _vacationDays; }
            set { _vacationDays = value; }
        }


        private string _diets;
        public string Diets
        {
            get { return _diets; }
            set { _diets = value; }
        }


        private string _paidHolidays;
        public string PaidHolidays
        {
            get { return _paidHolidays; }
            set { _paidHolidays = value; }
        }


        private string _bonuses;
        public string Bonuses
        {
            get { return _bonuses; }
            set { _bonuses = value; }
        }


        private string _dollars; // I need to come up with a better name :-)
        public string Dollars
        {
            get { return _dollars; }
            set { _dollars = value; }
        }


        private string _prepayment;
        public string Prepayment
        {
            get { return _prepayment; }
            set { _prepayment = value; }
        }


        private string _sickness;
        public string Sickness
        {
            get { return _sickness; }
            set { _sickness = value; }
        }


        // -----


        private Listing() { }


        public Listing(Storage storage, int year, int month)
        {
            Year = year;
            Month = month;
            CreatedAt = DateTime.Now;

            _items = storage.CreateMap<int, ListingItem>();
            RefreshIndexes();
        }


        public ListingItem AddItem(int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            if (_items.ContainsKey(day)) {
                throw new ListingItemAlreadyExistsException();
            }

            ListingItem newItem = new ListingItem(this, day, locality, start, end, lunchStart, lunchEnd, otherHours);
            _items.Add(day, newItem);
            WorkedDays++;
            WorkedHours += newItem.TimeSetting.WorkedHours;
            LunchHours += newItem.TimeSetting.LunchHours;
            OtherHours += newItem.TimeSetting.OtherHours;
            TotalWorkedHours += newItem.TimeSetting.TotalWorkedHours;

            Localities.Add(locality);

            return newItem;
        }


        public ListingItem AddItem(int day, string locality, TimeSetting timeSetting)
        {
            return AddItem(day, locality, timeSetting.Start, timeSetting.End, timeSetting.LunchStart, timeSetting.LunchEnd, timeSetting.OtherHours);
        }


        public ListingItem ReplaceItem(int day, string locality, Time start, Time end, Time lunchStart, Time lunchEnd, Time otherHours)
        {
            if (!_items.ContainsKey(day)) {
                return AddItem(day, locality, start, end, lunchStart, lunchEnd, otherHours);
            }

            ListingItem oldItem = GetItemByDay(day);

            ListingItem newItem = new ListingItem(this, day, locality, start, end, lunchStart, lunchEnd, otherHours);

            WorkedHours += newItem.TimeSetting.WorkedHours - oldItem.TimeSetting.WorkedHours;
            LunchHours += newItem.TimeSetting.LunchHours - oldItem.TimeSetting.LunchHours;
            OtherHours += newItem.TimeSetting.OtherHours - oldItem.TimeSetting.OtherHours;
            TotalWorkedHours += newItem.TimeSetting.TotalWorkedHours - oldItem.TimeSetting.TotalWorkedHours;

            _items[day] = newItem;

            Localities.Add(locality);

            return newItem;
        }


        public ListingItem ReplaceItem(int day, string locality, TimeSetting timeSetting)
        {
            return ReplaceItem(day, locality, timeSetting.Start, timeSetting.End, timeSetting.LunchStart, timeSetting.LunchEnd, timeSetting.OtherHours);
        }


        public void RemoveItemByDay(int day)
        {
            if (!_items.ContainsKey(day)) {
                return;
            }

            ListingItem item = _items[day];
            WorkedDays--;
            WorkedHours -= item.TimeSetting.WorkedHours;
            LunchHours -= item.TimeSetting.LunchHours;
            OtherHours -= item.TimeSetting.OtherHours;
            TotalWorkedHours -= item.TimeSetting.TotalWorkedHours;

            _items.Remove(day);
        }


        public ListingItem GetItemByDay(int day)
        {
            if (_items.ContainsKey(day)) {
                return _items[day];
            }

            return null;
        }


        public bool ContainsItem(ListingItem item)
        {
            return _items.Contains(new KeyValuePair<int, ListingItem>(item.Day, item));
        }
        
    }
}
