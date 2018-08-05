using Caliburn.Micro;
using Common.Commands;
using prjt.Domain;
using prjt.EventArguments;
using prjt.Exceptions;
using prjt.Facades;
using intf.Messages;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.EventAggregator.Messages;
using intf.BaseViewModels;

namespace intf.Views
{
    public class ListingDetailViewModel : BaseScreen
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                Set(ref _listing, value);
                Reset(value);
            }
        }


        private Week _selectedWeek;
        public Week SelectedWeek
        {
            get { return _selectedWeek; }
            set {
                Set(ref _selectedWeek, value);
                if (value == null) {
                    DisplayableItems = new ObservableCollection<DayItem>(_dayItems);
                } else {
                    DisplayableItems = new ObservableCollection<DayItem>(value.DayItems);
                }
                ExpandItemsCommand.RaiseCanExecuteChanged();
            }
        }


        private List<Week> _weeksInMonth;
        public List<Week> WeeksInMonth
        {
            get { return _weeksInMonth; }
        }


        private ObservableCollection<DayItem> _displayableItems;
        public ObservableCollection<DayItem> DisplayableItems
        {
            get { return _displayableItems; }
            set
            {
                Set(ref _displayableItems, value);
            }
        }


        private DelegateCommand<int> _openListingItemDetailCommand;
        public DelegateCommand<int> OpenListingItemDetailCommand
        {
            get
            {
                if (_openListingItemDetailCommand == null) {
                    _openListingItemDetailCommand = new DelegateCommand<int>(p => OpenListingItemDetail(p));
                }
                return _openListingItemDetailCommand;
            }
        }


        private DelegateCommand<int> _removeItemCommand;
        public DelegateCommand<int> RemoveItemCommand
        {
            get
            {
                if (_removeItemCommand == null) {
                    _removeItemCommand = new DelegateCommand<int>(p => RemoveItemByDay(p));
                }
                return _removeItemCommand;
            }
        }


        private DelegateCommand<int> _copyItemDownCommand;
        public DelegateCommand<int> CopyItemDownCommand
        {
            get
            {
                if (_copyItemDownCommand == null) {
                    _copyItemDownCommand = new DelegateCommand<int>(p => CopyItemDown(p));
                }
                return _copyItemDownCommand;
            }
        }


        private DelegateCommand<object> _expandItemsCommand;
        public DelegateCommand<object> ExpandItemsCommand
        {
            get
            {
                if (_expandItemsCommand == null) {
                    _expandItemsCommand = new DelegateCommand<object>(
                        p => ExpandItems(),
                        p => SelectedWeek != null
                    );
                }
                return _expandItemsCommand;
            }
        }


        private List<DayItem> _dayItems;

        private readonly ListingFacade _listingFacade;


        public ListingDetailViewModel(
            ListingFacade listingFacade
        ) {
            _listingFacade = listingFacade;
            _dayItems = new List<DayItem>();
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            _secondNavigation = PrepareViewModel(() => { return new ListingDetailNavigationViewModel(Listing); });
        }


        private void OpenListingItemDetail(int day)
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingItemViewModel>(x => {
                x.DayItem = _dayItems[day - 1];
                x.SecondNavigation = _secondNavigation;
            }));
        }


        private void RemoveItemByDay(int day)
        {
            if (_dayItems[day - 1].Listing == null) {
                return;
            }

            Listing.RemoveItemByDay(day);
            _dayItems[day - 1].Reset();

            _listingFacade.Update(Listing);
        }


        private void CopyItemDown(int day)
        {
            DayItem dayItem = _dayItems[day - 1];
            if (dayItem.ListingItem == null || _dayItems[day].IsEqual(dayItem)) {
                return;
            }
            ListingItem newItem = Listing.ReplaceItem(day + 1, dayItem.Locality, dayItem.ListingItem.TimeSetting);

            _dayItems[day].Update(newItem);

            _listingFacade.Update(Listing);
        }


        private void ExpandItems()
        {
            SelectedWeek = null;
        }


        private void Reset(Listing listing)
        {
            WindowTitle.Text = GenerateWindowTitle(listing);
            NotifyOfPropertyChange(() => WindowTitle);

            _dayItems = PrepareDayItems(listing);
            _weeksInMonth = new List<Week>(PrepareWeeks(_dayItems).Values);
            NotifyOfPropertyChange(() => WeeksInMonth);

            DateTime now = DateTime.Now;
            int currentWeekNumber = Date.GetWeekNumber(now.Year, now.Month, now.Day);
            SelectedWeek = _weeksInMonth.Find(w => w.WeekNumber == currentWeekNumber);
        }


        private string GenerateWindowTitle(Listing listing)
        {
            return string.Format("{0} {1} {2}", Date.Months[12 - listing.Month], listing.Year, string.Format("- {0}", listing.Name));
        }


        private List<DayItem> PrepareDayItems(Listing listing)
        {
            List<DayItem> dayItems = new List<DayItem>();

            DateTime now = DateTime.Now;
            DayItem dayItem;
            for (int day = 0; day < listing.DaysInMonth; day++) {
                dayItem = new DayItem(listing, day + 1);
                dayItems.Add(dayItem);
            }

            return dayItems;
        }


        private Dictionary<int, Week> PrepareWeeks(List<DayItem> items)
        {
            DateTime now = DateTime.Now;
            int currentWeekNumber = Date.GetWeekNumber(now.Year, now.Month, now.Day);

            Dictionary<int, Week> weeks = new Dictionary<int, Week>();
            foreach (DayItem dayItem in items) {
                if (!weeks.ContainsKey(dayItem.Week)) {
                    weeks.Add(dayItem.Week, new Week(dayItem.Week, dayItem.Week == currentWeekNumber));
                }

                Week week = weeks[dayItem.Week];
                week.AddDayItem(dayItem);
            }

            return weeks;
        }

    }
}
