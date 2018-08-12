using Caliburn.Micro;
using Common.Commands;
using prjt.Domain;
using prjt.Facades;
using intf.Messages;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Common.EventAggregator.Messages;
using intf.BaseViewModels;
using prjt.Services.IO;
using prjt.Services.Pdf;
using prjt.Services.Entities;
using System.Windows.Forms;
using Common.Overlay;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;

namespace intf.Views
{
    public class ListingsOverviewViewModel : BaseScreen
    {
        private DelegateCommand<Listing> _openListingCommand;
        public DelegateCommand<Listing> OpenListingCommand
        {
            get
            {
                if (_openListingCommand == null) {
                    _openListingCommand = new DelegateCommand<Listing>(p => OpenListing(p));
                }

                return _openListingCommand;
            }
        }


        private DelegateCommand<object> _generatePDFCommand;
        public DelegateCommand<object> GeneratePDFCommand
        {
            get
            {
                if (_generatePDFCommand == null) {
                    _generatePDFCommand = new DelegateCommand<object>(p => GeneratePDF());
                }
                return _generatePDFCommand;
            }
        }


        private List<Listing> _listingsList;
        private ICollectionView _listings;
        public ICollectionView Listings
        {
            get { return _listings; }
            set {
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


        /*private IIODialogService _filePathDialogService;
        private IMultipleListingReportFactory _multipleListingReportFactory;
        private IListingReportGenerator _listingReportGenerator;*/

        private ListingFacade _listingFacade;

        public ListingsOverviewViewModel(
            ListingFacade listingFacade/*,
            IIODialogService filePathDialogService,
            IMultipleListingReportFactory multipleListingReportFactory,
            IListingReportGenerator listingReportGenerator*/
        ) {
            _listingFacade = listingFacade;
            /*_filePathDialogService = filePathDialogService;
            _multipleListingReportFactory = multipleListingReportFactory;
            _listingReportGenerator = listingReportGenerator;*/
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            BaseWindowTitle = "Přehled výčetek";

            _years = Date.GetYears(2010, "DESC");
            _months = new List<string>(Date.Months);
            _months.Reverse();
            _months.Insert(0, "Celý rok");

            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            LoadListings(SelectedYear, SelectedMonth);
        }


        private void LoadListings(int year, int month)
        {
            _listingsList = _listingFacade.FindListings(year, month);
            Listings = CollectionViewSource.GetDefaultView(_listingsList);
        }


        private void OpenListing(Listing listing)
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDetailViewModel>(x => { x.Listing = listing; }));
        }


        private void GeneratePDF()
        {
            /*string fileName = SelectedMonth == 0 ?
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
                Document doc = _multipleListingReportFactory.Create(_listingsList, new DefaultListingPdfReportSetting());

                _listingReportGenerator.Save(filePath, doc);

                ot.HideOverlay();
                EventAggregator.PublishOnUIThread(new ListingPdfSuccessfullyGeneratedMessage());
            });*/
        }
    }
}
