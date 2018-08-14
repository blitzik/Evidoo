using Caliburn.Micro;
using Common.Commands;
using intf.BaseViewModels;
using intf.Messages;
using prjt.Domain;
using prjt.Facades;
using prjt.Utils;
using intf.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using prjt.Services.IO;
using prjt.Services.Pdf;
using Common.Overlay;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using intf.Subscribers.Messages;
using System.Linq;

namespace intf.Views
{
    // todo  metoda LoadListings se spouští několikrát
    public class ListingsPdfGenerationViewModel : BaseScreen
    {
        private List<ListingCheckBoxWrapper> _listingsList;
        private ICollectionView _listings;
        public ICollectionView Listings
        {
            get { return _listings; }
            set
            {
                _listings = value;
                value.GroupDescriptions.Add(new PropertyGroupDescription("Month"));

                NotifyOfPropertyChange(() => Listings);
            }
        }


        private List<int> _years;
        public List<int> Years
        {
            get { return _years; }
        }


        private List<string> _months;
        public List<string> Months
        {
            get { return _months; }
        }


        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                Set(ref _selectedYear, value);
                LoadListings(SelectedYear, SelectedMonth);
            }
        }


        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                Set(ref _selectedMonth, value);
                LoadListings(SelectedYear, SelectedMonth);
            }
        }


        private bool _canGenerate;
        public bool CanGenerate
        {
            get { return _canGenerate; }
            private set { Set(ref _canGenerate, value); }
        }


        private PdfGenerationSettingsViewModel _pdfGenerationSettingsViewModel;
        public PdfGenerationSettingsViewModel PdfGenerationSettingsViewModel
        {
            get { return _pdfGenerationSettingsViewModel; }
            private set { Set(ref _pdfGenerationSettingsViewModel, value); }
        }


        private DelegateCommand<object> _generatePdfCommand;
        public DelegateCommand<object> GeneratePdfCommand
        {
            get
            {
                if (_generatePdfCommand == null) {
                    _generatePdfCommand = new DelegateCommand<object>(p => GeneratePdf());
                }
                return _generatePdfCommand;
            }
        }


        private DelegateCommand<object> _resetSettingsCommand;
        public DelegateCommand<object> ResetSettingsCommand
        {
            get
            {
                if (_resetSettingsCommand == null) {
                    _resetSettingsCommand = new DelegateCommand<object>(
                        p => ResetSettings(),
                        p => !_defaultSettings.Pdfsetting.IsEqual(_pdfGenerationSettingsViewModel.PdfSetting)
                    );
                }
                return _resetSettingsCommand;
            }
        }


        private DefaultSettings _defaultSettings;

        private IIODialogService _filePathDialogService;
        private IListingsReportFactory _multipleListingReportFactory;
        private IListingReportGenerator _listingReportGenerator;
        private ListingFacade _listingFacade;
        private SettingFacade _settingFacade;

        public ListingsPdfGenerationViewModel(
            ListingFacade listingFacade,
            SettingFacade settingFacade,
            IIODialogService filePathDialogService,
            IListingsReportFactory multipleListingReportFactory,
            IListingReportGenerator listingReportGenerator
        ) {
            _listingFacade = listingFacade;
            _settingFacade = settingFacade;
            _filePathDialogService = filePathDialogService;
            _multipleListingReportFactory = multipleListingReportFactory;
            _listingReportGenerator = listingReportGenerator;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            BaseWindowTitle = "Hromadné generování PDF dokumentů";

            CanGenerate = false;

            _years = Date.GetYears(2010, "DESC");
            _months = new List<string>(Date.Months);
            _months.Reverse();
            _months.Insert(0, "Celý rok");

            if (SelectedYear == 0) { // month can be 0 -> displays entire year
                SetDefaultPeriod(DateTime.Today.Year, DateTime.Today.Month);
            }

            _defaultSettings = _settingFacade.GetDefaultSettings();

            PdfGenerationSettingsViewModel = GetViewModel<PdfGenerationSettingsViewModel>();
            PdfGenerationSettingsViewModel.OnSettingsPropertyChanged += (s, arg) => {
                ResetSettingsCommand.RaiseCanExecuteChanged();
            };
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            LoadListings(SelectedYear, SelectedMonth);

            _defaultSettings = _settingFacade.GetDefaultSettings();
            ResetSettings();
        }


        public void SetDefaultPeriod(int year, int month)
        {
            _selectedYear = year;
            NotifyOfPropertyChange(() => SelectedYear);

            _selectedMonth = month;
            NotifyOfPropertyChange(() => SelectedMonth);
        }


        private void LoadListings(int year, int month)
        {
            CanGenerate = false;
            var listings = _listingFacade.FindListings(year, month);
            _listingsList = new List<ListingCheckBoxWrapper>();
            foreach (Listing l in listings) {
                ListingCheckBoxWrapper w = new ListingCheckBoxWrapper(l);
                w.OnIsCheckedChanged += (s, v) => {
                    IEnumerable<Listing> result = from ListingCheckBoxWrapper lw in _listingsList where lw.IsChecked == true select lw.Listing;
                    CanGenerate = result.Count() > 0;
                };
                _listingsList.Add(w);
            }
            Listings = CollectionViewSource.GetDefaultView(_listingsList);
        }


        private void GeneratePdf()
        {
            if (CanGenerate == false) {
                return;
            }

            string fileName = SelectedMonth == 0 ?
                              string.Format("Rok {0}", SelectedYear) :
                              string.Format("{1} {0}", SelectedYear, Date.Months[12 - SelectedMonth]);

            string filePath = _filePathDialogService.GetFilePath<SaveFileDialog>(
                fileName,
                d => { d.Filter = "PDF dokument (*.pdf)|*.pdf"; }
            );

            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            IOverlayToken ot = Overlay.DisplayOverlay(PrepareViewModel<ProgressViewModel>());
            Task.Factory.StartNew(() => {
                IEnumerable<Listing> listings = from ListingCheckBoxWrapper lw in _listingsList where lw.IsChecked == true select lw.Listing;
                Document doc = _multipleListingReportFactory.Create(listings, PdfGenerationSettingsViewModel.PdfSetting);

                _listingReportGenerator.Save(filePath, doc);

                ot.HideOverlay();
                EventAggregator.PublishOnUIThread(new ListingPdfSuccessfullyGeneratedMessage());
            });
        }


        private void ResetSettings()
        {
            _pdfGenerationSettingsViewModel.PdfSetting.UpdateBy(_defaultSettings.Pdfsetting);
        }
    }
}
