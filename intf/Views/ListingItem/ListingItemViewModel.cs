using Caliburn.Micro;
using Common.Commands;
using prjt.Domain;
using prjt.EventArguments;
using prjt.Exceptions;
using prjt.Facades;
using prjt.Services;
using prjt.Utils;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using intf.Messages;
using System.Collections.ObjectModel;
using Common.EventAggregator.Messages;
using Common.Validation;
using intf.BaseViewModels;

namespace intf.Views
{
    public class ListingItemViewModel : BaseScreen
    {
        private string _header;
        public string Header
        {
            get { return _header; }
            private set
            {
                Set(ref _header, value);
            }
        }


        private WorkedTimeSettingViewModel _workedTimeViewModel;
        public WorkedTimeSettingViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
            private set
            {
                Set(ref _workedTimeViewModel, value);
            }
        }


        private string _locality;
        public string Locality
        {
            get { return _locality; }
            set
            {
                Set(ref _locality, value);
            }
        }


        private DelegateCommand<object> _saveListingItemCommand;
        public DelegateCommand<object> SaveListingItemCommand
        {
            get
            {
                if (_saveListingItemCommand == null) {
                    _saveListingItemCommand = new DelegateCommand<object>(p => SaveListingItem());
                }
                return _saveListingItemCommand;
            }
        }


        private DayItem _dayItem;
        public DayItem DayItem
        {
            get { return _dayItem; }
            set
            {
                _dayItem = value;
                Reset(value);
                NotifyOfPropertyChange(() => DayItem);
            }
        }


        private ObservableCollection<string> _localities;
        public ObservableCollection<string> Localities
        {
            get { return _localities; }
            set
            {
                _localities = value;
                NotifyOfPropertyChange(() => Localities);
            }
        }


        private string _selectedLocality;
        public string SelectedLocality
        {
            get { return _selectedLocality; }
            set
            {
                _selectedLocality = value;
                _locality = value;
                NotifyOfPropertyChange(() => SelectedLocality);
            }
        }


        private ListingFacade _listingFacade;
        private SettingFacade _settingFacade;
        
        private DefaultSettings _defaultSettings;


        public ListingItemViewModel(ListingFacade listingFacade, SettingFacade settingFacade)
        {
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;

            Localities = new ObservableCollection<string>();
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
        }


        private void Reset(DayItem dayItem)
        {
            string date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase((new DateTime(dayItem.Year, dayItem.Month, dayItem.Day)).ToLongDateString().ToLower());
            WindowTitle.Text = String.Format("{0} - {1}", date, dayItem.Listing.Name);
            _defaultSettings = _settingFacade.GetDefaultSettings();
            Locality = null;

            if (dayItem.ListingItem != null) {
                ListingItem l = dayItem.ListingItem;
                Locality = l.Locality;

                WorkedTimeViewModel = PrepareViewModel(() => { return new WorkedTimeSettingViewModel(_defaultSettings.Time, l.TimeSetting, _defaultSettings.TimeTickInMinutes); });

            } else {
                WorkedTimeViewModel = PrepareViewModel(() => { return new WorkedTimeSettingViewModel(_defaultSettings.Time, _defaultSettings.Time, _defaultSettings.TimeTickInMinutes); });
            }

            Localities = new ObservableCollection<string>(dayItem.Localities);
        }


        private void SaveListingItem()
        {
            ListingItem newItem = _dayItem.Listing.ReplaceItem(
                _dayItem.Day,
                string.IsNullOrEmpty(_locality) ? null : _locality.Trim(),
                new Time(WorkedTimeViewModel.StartTime),
                new Time(WorkedTimeViewModel.EndTime),
                new Time(WorkedTimeViewModel.LunchStart),
                new Time(WorkedTimeViewModel.LunchEnd),
                new Time(WorkedTimeViewModel.OtherHours)
            );

            _listingFacade.Update(_dayItem.Listing);

            DayItem.Update(newItem);

            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDetailViewModel>());
        }
    }
}
