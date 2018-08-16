using Caliburn.Micro;
using intf.BaseViewModels;
using intf.Views;
using intf.Utils;
using prjt.Domain;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.Facades;
using Common.Commands;
using prjt.Services.Entities;
using intf.Subscribers.Messages;

namespace intf.Views
{
    public class CopyListingViewModel : BaseScreen
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set { Set(ref _listing, value); }
        }


        private string _listingName;
        public string ListingName
        {
            get { return _listingName; }
            set { Set(ref _listingName, value); }
        }


        private WorkedTimeSettingViewModel _workedTimeSettingViewModel;
        public WorkedTimeSettingViewModel WorkedTimeSettingViewModel
        {
            get { return _workedTimeSettingViewModel; }
        }


        private bool _changeItemsTimes;
        public bool ChangeItemsTimes
        {
            get { return _changeItemsTimes; }
            set { Set(ref _changeItemsTimes, value); }
        }


        private DelegateCommand<object> _copyListingCommand;
        public DelegateCommand<object> CopyListingCommand
        {
            get
            {
                if (_copyListingCommand == null) {
                    _copyListingCommand = new DelegateCommand<object>(p => CopyListing());
                }
                return _copyListingCommand;
            }
        }


        private DefaultSettings _defaultSettings;


        private SettingFacade _settingFacade;
        private ListingFacade _listingFacade;
        private IListingFactory _listingFactory;

        public CopyListingViewModel(
            SettingFacade settingFacade,
            ListingFacade listingFacade,
            IListingFactory listingFactory
        ) {
            _settingFacade = settingFacade;
            _listingFacade = listingFacade;
            _listingFactory = listingFactory;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            BaseWindowTitle = "Vytvoření kopie výčetky";
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            _defaultSettings = _settingFacade.GetDefaultSettings();

            Reset(Listing);
        }


        private void CopyListing() // todo redirect to overview / listing detail + flashMessage etc...
        {
            Listing newListing = _listingFactory.Create(Listing.Year, Listing.Month);

            newListing.Name = ListingName;
            newListing.Employer = Listing.Employer;
            newListing.Vacation = Listing.Vacation;
            newListing.Holiday = Listing.Holiday;
            newListing.SicknessHours = Listing.SicknessHours;
            newListing.HourlyWage = Listing.HourlyWage;
            newListing.VacationDays = Listing.VacationDays;
            newListing.Diets = Listing.Diets;
            newListing.PaidHolidays = Listing.PaidHolidays;
            newListing.Bonuses = Listing.Bonuses;
            newListing.Dollars = Listing.Dollars;
            newListing.Prepayment = Listing.Prepayment;
            newListing.Sickness = Listing.Sickness;

            TimeSetting ts = null;
            if (ChangeItemsTimes == true) {
                ts = WorkedTimeSettingViewModel.TimeSetting;
            }

            foreach (KeyValuePair<int, ListingItem> item in Listing.Items) {
                newListing.AddItem(item.Value.Day, item.Value.Locality, ts ?? item.Value.TimeSetting);
            }

            _listingFacade.StoreListing(newListing);

            EventAggregator.PublishOnUIThread(new ListingSuccessfullyCopiedMessage(newListing));
        }


        private void Reset(Listing listing)
        {
            WindowTitle.Text = string.Format("{0} [{1} {2} {3}]", BaseWindowTitle, Date.Months[12 - listing.Month], listing.Year, string.Format("- {0}", listing.Name));
            NotifyOfPropertyChange(() => WindowTitle);

            ChangeItemsTimes = false;
            string listingName = string.Format("Kopie - {0}", Listing.Name);
            if (listingName.Length > 50) {
                listingName = listingName.Substring(0, 50);
            }
            ListingName = listingName;

            _workedTimeSettingViewModel = PrepareViewModel(() => {
                return new WorkedTimeSettingViewModel(_defaultSettings.Time, _defaultSettings.Time, _defaultSettings.TimeTickInMinutes);
            });
            NotifyOfPropertyChange(() => WorkedTimeSettingViewModel);
        }
    }
}
