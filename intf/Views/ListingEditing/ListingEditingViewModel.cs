using Caliburn.Micro;
using Common.Commands;
using prjt.Domain;
using prjt.EventArguments;
using prjt.Facades;
using intf.Messages;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EventAggregator.Messages;
using intf.BaseViewModels;

namespace intf.Views
{
    public class ListingEditingViewModel : BaseScreen
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                Set(ref _listing, value);
                Reset(Listing);
            }
        }


        private ObservableCollection<int> _years = new ObservableCollection<int>();
        public ObservableCollection<int> Years
        {
            get { return _years; }
        }


        private ObservableCollection<string> _months = new ObservableCollection<string>();
        public ObservableCollection<string> Months
        {
            get { return _months; }
        }


        private List<Employer> _employers;
        public List<Employer> Employers
        {
            get
            {
                return _employers;
            }
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


        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                Set(ref _selectedMonth, value);
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


        private string _vacation;
        public string Vacation
        {
            get { return _vacation; }
            set { Set(ref _vacation, value); }
        }


        private string _holiday;
        public string Holiday
        {
            get { return _holiday; }
            set { Set(ref _holiday, value); }
        }


        private string _sicknessHours;
        public string SicknessHours
        {
            get { return _sicknessHours; }
            set { Set(ref _sicknessHours, value); }
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


        private string _vacationDays;
        public string VacationDays
        {
            get { return _vacationDays; }
            set { Set(ref _vacationDays, value); }
        }


        private string _diets;
        public string Diets
        {
            get { return _diets; }
            set { Set(ref _diets, value); }
        }


        private string _paidHolidays;
        public string PaidHolidays
        {
            get { return _paidHolidays; }
            set { Set(ref _paidHolidays, value); }
        }


        private string _bonuses;
        public string Bonuses
        {
            get { return _bonuses; }
            set { Set(ref _bonuses, value); }
        }


        private string _dollars; // better name? :D
        public string Dollars
        {
            get { return _dollars; }
            set { Set(ref _dollars, value); }
        }


        private string _prepayment;
        public string Prepayment
        {
            get { return _prepayment; }
            set { Set(ref _prepayment, value); }
        }


        private string _sickness;
        public string Sickness
        {
            get { return _sickness; }
            set { Set(ref _sickness, value); }
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


        private DelegateCommand<object> _returnBackCommand;
        public DelegateCommand<object> ReturnBackCommand
        {
            get
            {
                if (_returnBackCommand == null) {
                    _returnBackCommand = new DelegateCommand<object>(p => ReturnBack());
                }
                return _returnBackCommand;
            }
        }


        private ListingFacade _listingFacade;
        private EmployerFacade _employerFacade;

        
        public ListingEditingViewModel(ListingFacade listingFacade, EmployerFacade employerFacade)
        {
            BaseWindowTitle = "Úprava výčetky";

            _listingFacade = listingFacade;
            _employerFacade = employerFacade;

            _selectedEmployer = _promptEmployer;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
        }


        // -----


        private void RefreshEmployers()
        {
            _employers = _employerFacade.FindAllEmployers();
            _employers.Insert(0, _promptEmployer);

            NotifyOfPropertyChange(() => Employers);
        }


        private void SaveListing()
        {
            if (Listing == null) {
                throw new Exception("No Listing is set!");
            }

            Listing.Name = string.IsNullOrEmpty(Name) ? null : Name.Trim();
            Listing.HourlyWage = _hourlyWage;
            Listing.Employer = _selectedEmployer == _promptEmployer ? null : _selectedEmployer;
            Listing.Vacation = Vacation;
            Listing.Holiday = Holiday;
            Listing.SicknessHours = SicknessHours;
            Listing.VacationDays = VacationDays;
            Listing.Diets = Diets;
            Listing.PaidHolidays = PaidHolidays;
            Listing.Bonuses = Bonuses;
            Listing.Dollars = Dollars;
            Listing.Prepayment = Prepayment;
            Listing.Sickness = Sickness;

            _listingFacade.Update(Listing);

            if (_hourlyWage != null && _hourlyWage <= 0) {
                HourlyWage = null;
            }

            EventAggregator.PublishOnUIThread(new ChangeViewMessage<IViewModel>(nameof(ListingDetailViewModel)));
        }


        private void ReturnBack()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<IViewModel>(nameof(ListingDetailViewModel)));
        }


        private void Reset(Listing listing)
        {
            WindowTitle.Text = string.Format("{0} [{1} {2} {3}]", BaseWindowTitle, Date.Months[12 - listing.Month], listing.Year, string.Format("- {0}", listing.Name));

            RefreshEmployers();

            _years.Clear();
            _years.Add(listing.Year);
            _months.Clear();
            _months.Add(Date.Months[12 - listing.Month]);

            SelectedYear = 0;
            SelectedMonth = 0;


            if (listing.Employer == null || !_employers.Exists(e => e == listing.Employer)) {
                SelectedEmployer = _promptEmployer;
            } else {
                SelectedEmployer = listing.Employer;
            }

            Name = listing.Name;
            HourlyWage = listing.HourlyWage == null ? null : listing.HourlyWage.ToString();

            Vacation = Listing.Vacation;
            Holiday = Listing.Holiday;
            SicknessHours = Listing.SicknessHours;
            VacationDays = Listing.VacationDays;
            Diets = Listing.Diets;
            PaidHolidays = Listing.PaidHolidays;
            Bonuses = Listing.Bonuses;
            Dollars = Listing.Dollars;
            Prepayment = Listing.Prepayment;
            Sickness = Listing.Sickness;
        }

    }
}
