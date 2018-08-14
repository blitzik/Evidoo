using prjt.Domain;
using prjt.Services.Entities;
using prjt.Services.IO;
using prjt.Services.Pdf;
using prjt.Utils;
using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Caliburn.Micro;
using System.Windows.Forms;
using intf.BaseViewModels;
using intf.Subscribers.Messages;
using intf.Messages;
using Common.Overlay;

namespace intf.Views
{
    public class EmptyListingsGenerationViewModel : BaseScreen
    {
        private List<int> _years;
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


        private DelegateCommand<object> _generatePdfsCommand;
        public DelegateCommand<object> GeneratePdfsCommand
        {
            get
            {
                if (_generatePdfsCommand == null) {
                    _generatePdfsCommand = new DelegateCommand<object>(p => GeneratePdfs());
                }
                return _generatePdfsCommand;
            }
        }


        private IIODialogService _filePathDialogService;
        private IListingsReportFactory _multipleListingReportFactory;
        private IListingReportGenerator _listingReportGenerator;
        private IListingFactory _listingFactory;


        public EmptyListingsGenerationViewModel(
            IIODialogService filePathDialogService,
            IListingsReportFactory multipleListingReportFactory,
            IListingReportGenerator listingReportGenerator,
            IListingFactory listingFactory
        ) {
            _filePathDialogService = filePathDialogService;
            _multipleListingReportFactory = multipleListingReportFactory;
            _listingReportGenerator = listingReportGenerator;
            _listingFactory = listingFactory;            
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            BaseWindowTitle = "Generování prázných výčetek";
            SelectedYear = DateTime.Now.Year;

            _years = Date.GetYears(2010, "DESC");
            _years.Insert(0, _years[0] + 1);
        }


        private void GeneratePdfs()
        {
            string filePath = _filePathDialogService.GetFilePath<SaveFileDialog>(
                string.Format("Výčetky {0}", SelectedYear),
                d => { d.Filter = "PDF dokument (*.pdf)|*.pdf"; }
            );

            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            IOverlayToken ot = Overlay.DisplayOverlay(PrepareViewModel<ProgressViewModel>());
            Task.Run(() => {
                List<Listing> list = new List<Listing>();
                for (int month = 0; month < 12; month++) {
                    list.Add(_listingFactory.Create(SelectedYear, month + 1));
                }

                Document doc = _multipleListingReportFactory.Create(list, new DefaultListingPdfReportSetting());

                _listingReportGenerator.Save(filePath, doc);

                ot.HideOverlay();
                EventAggregator.PublishOnUIThread(new ListingPdfSuccessfullyGeneratedMessage());
            });
        }

    }
}
