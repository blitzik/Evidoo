using Caliburn.Micro;
using Common.Commands;
using prjt.ViewModels.Base;
using prjt.Domain;
using prjt.Facades;
using prjt.Messages;
using prjt.Services.Entities;
using prjt.Utils;
using System;
using System.Collections.Generic;
using Common.EventAggregator.Messages;

namespace prjt.ViewModels
{
    public class ListingViewModel : BaseScreen
    {
        private ListingFacade _listingFacade;
        private EmployerFacade _employerFacade;


        private List<int> _years = Date.GetYears(2010, "DESC");
        public List<int> Years
        {
            get { return _years; }
        }


        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                Set(ref _selectedYear, value);
            }
        }


        private List<string> _months = new List<string>(Date.Months);
        public List<string> Months
        {
            get { return _months; }
        }


        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                Set(ref _selectedMonth, value);
            }
        }


        private List<Employer> _employers;
        public List<Employer> Employers
        {
            get
            {
                return _employers;
            }
        }

        private Employer _promptEmployer = new Employer() { Name = "Bez zaměstnavatele" };
        private Employer _selectedEmployer;
        public Employer SelectedEmployer
        {
            get { return _selectedEmployer; }
            set
            {
                Set(ref _selectedEmployer, value);
            }
        }


        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
            }
        }


        private int? _hourlyWage;
        public string HourlyWage
        {
            get { return _hourlyWage.ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value)) {
                    Set(ref _hourlyWage, null);
                    return;
                }

                if (!int.TryParse(value, out int result)) {
                    return;
                }

                if (result < 0) {
                    return;

                } else {
                    Set(ref _hourlyWage, result);
                }
            }
        }



        private DelegateCommand<object> _saveCommand;
        public DelegateCommand<object> SaveCommand
        {
            get
            {
                if (_saveCommand == null) {
                    _saveCommand = new DelegateCommand<object>(p => SaveListing());
                }

                return _saveCommand;
            }
        }


        private IListingFactory _listingFactory;


        public ListingViewModel(
            ListingFacade listingFacade,
            EmployerFacade employerFacade,
            IListingFactory listingFactory
        ) {
            _listingFacade = listingFacade;
            _employerFacade = employerFacade;
            _listingFactory = listingFactory;

            BaseWindowTitle = "Nová výčetka";

            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;

            Reset();
        }


        private void SetDefaults()
        {
            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;
            SelectedEmployer = _promptEmployer;
            HourlyWage = null;
            Name = null;
        }


        private void Reset()
        {
            _employers = _employerFacade.FindAllEmployers();
            _employers.Insert(0, _promptEmployer);
            NotifyOfPropertyChange(() => Employers);

            SetDefaults();
        }


        private void SaveListing()
        {
            Name = string.IsNullOrEmpty(Name) ? null : Name.Trim();

            Listing newListing = _listingFactory.Create(SelectedYear, SelectedMonth);
            newListing.Name = Name;
            newListing.HourlyWage = _hourlyWage;
            if (_hourlyWage != null && _hourlyWage <= 0) {
                HourlyWage = null;
            }

            if (_selectedEmployer != _promptEmployer) {
                newListing.Employer = _selectedEmployer;
            }

            _listingFacade.StoreListing(newListing);

            SetDefaults();

            EventAggregator.PublishOnUIThread(new ChangeViewMessage<IViewModel>(nameof(ListingDetailViewModel)));
            EventAggregator.PublishOnUIThread(new ListingMessage(newListing));
        }


        // -----


        protected override void OnActivate()
        {
            base.OnActivate();

            Reset();
        }

    }
}
