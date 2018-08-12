using Caliburn.Micro;
using Common.Commands;
using Common.Overlay;
using intf.BaseViewModels;
using intf.Messages;
using intf.Subscribers.Messages;
using MigraDoc.DocumentObjectModel;
using prjt.Domain;
using prjt.Facades;
using prjt.Services.IO;
using prjt.Services.Pdf;
using prjt.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace intf.Views
{
    public class ListingPdfGenerationViewModel : BaseScreen
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
            }
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


        private IIODialogService _filePathDialogService;
        private SettingFacade _settingFacade;
        private IListingPdfDocumentFactory _listingPdfDocumentFactory;
        private IListingReportGenerator _listingReportGenerator;

        private DefaultSettings _defaultSettings;

        public ListingPdfGenerationViewModel(
            SettingFacade settingFacade,
            IIODialogService filePathDialogService,
            IListingPdfDocumentFactory listingPdfDocumentFactory,
            IListingReportGenerator listingReportGenerator
        ) {
            _settingFacade = settingFacade;
            _filePathDialogService = filePathDialogService;
            _listingPdfDocumentFactory = listingPdfDocumentFactory;
            _listingReportGenerator = listingReportGenerator;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            BaseWindowTitle = "Generování PDF dokumentu";

            _defaultSettings = _settingFacade.GetDefaultSettings();

            PdfGenerationSettingsViewModel = GetViewModel<PdfGenerationSettingsViewModel>();
            PdfGenerationSettingsViewModel.OnSettingsPropertyChanged += (s, arg) => {
                ResetSettingsCommand.RaiseCanExecuteChanged();
            };
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            _defaultSettings = _settingFacade.GetDefaultSettings();
            ResetSettings();
        }


        private void GeneratePdf()
        {
            string filePath = _filePathDialogService.GetFilePath<SaveFileDialog>(
                string.Format("{0} {1} - {2}", Date.Months[12 - Listing.Month], Listing.Year, Listing.Name),
                d => { d.Filter = "PDF dokument (*.pdf)|*.pdf"; }
            );
            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            IOverlayToken ot = Overlay.DisplayOverlay(PrepareViewModel<ProgressViewModel>());
            Task.Run(() => {
                Document doc = _listingPdfDocumentFactory.Create(Listing, _pdfGenerationSettingsViewModel.PdfSetting);
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
