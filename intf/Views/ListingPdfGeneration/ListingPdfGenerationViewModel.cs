using Caliburn.Micro;
using Common.Commands;
using intf.BaseViewModels;
using intf.Messages;
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
                        p => !_defaultSettings.Pdfsetting.IsEqual(_pdfSetting)
                    );
                }
                return _resetSettingsCommand;
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


        private DefaultListingPdfReportSetting _pdfSetting;
        public DefaultListingPdfReportSetting PdfSetting
        {
            get { return _pdfSetting; }
            set
            {
                _pdfSetting = value;
                NotifyOfPropertyChange(() => PdfSetting);
            }
        }


        private readonly IIODialogService _filePathDialogService;
        private readonly SettingFacade _settingFacade;
        private readonly IWindowManager _windowManager;
        private readonly IListingPdfDocumentFactory _listingPdfDocumentFactory;
        private readonly IListingReportGenerator _listingReportGenerator;

        private DefaultSettings _defaultSettings;

        public ListingPdfGenerationViewModel(
            SettingFacade settingFacade,
            IWindowManager windowManager,
            IIODialogService filePathDialogService,
            IListingPdfDocumentFactory listingPdfDocumentFactory,
            IListingReportGenerator listingReportGenerator
        ) {
            BaseWindowTitle = "Generování PDF dokumentu";

            _settingFacade = settingFacade;
            _windowManager = windowManager;
            _filePathDialogService = filePathDialogService;
            _listingPdfDocumentFactory = listingPdfDocumentFactory;
            _listingReportGenerator = listingReportGenerator;

            _defaultSettings = settingFacade.GetDefaultSettings();

            PdfSetting = new DefaultListingPdfReportSetting(_defaultSettings.Pdfsetting);
            PdfSetting.OnSettingPropertyChanged += (object sender, EventArgs args) => { ResetSettingsCommand.RaiseCanExecuteChanged(); };
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
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

            ProgressBarWindowViewModel pb = PrepareViewModel<ProgressBarWindowViewModel>();
            Task.Run(async () => {
                Document doc = _listingPdfDocumentFactory.Create(Listing, _pdfSetting);
                _listingReportGenerator.Save(filePath, doc);

                pb.Success = true;
                await Task.Delay(pb.ResultIconDelay);

                pb.TryClose();
            });
            
            _windowManager.ShowDialog(pb);
        }


        private void ResetSettings()
        {
            PdfSetting.UpdateBy(_defaultSettings.Pdfsetting);
        }


        private void ReturnBack()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDetailViewModel>());
        }
    }
}
