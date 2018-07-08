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
using prjt.ViewModels.Base;
using Caliburn.Micro;
using System.Windows.Forms;

namespace prjt.ViewModels
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


        private readonly IWindowManager _windowManager;
        private readonly ISavingFilePathSelector _savingFilePathSelector;
        private readonly IMultipleListingReportFactory _multipleListingReportFactory;
        private readonly IListingReportGenerator _listingReportGenerator;
        private readonly IListingFactory _listingFactory;


        public EmptyListingsGenerationViewModel(
            IWindowManager windowManager,
            ISavingFilePathSelector savingFilePathSelector,
            IMultipleListingReportFactory multipleListingReportFactory,
            IListingReportGenerator listingReportGenerator,
            IListingFactory listingFactory
        ) {
            BaseWindowTitle = "Generování prázných výčetek";
            SelectedYear = DateTime.Now.Year;

            _windowManager = windowManager;
            _savingFilePathSelector = savingFilePathSelector;
            _multipleListingReportFactory = multipleListingReportFactory;
            _listingReportGenerator = listingReportGenerator;
            _listingFactory = listingFactory;

            _years = Date.GetYears(2010, "DESC");
            _years.Insert(0, _years[0] + 1);
        }


        private void GeneratePdfs()
        {
            string filePath = _savingFilePathSelector.GetFilePath(
                string.Format("Výčetky {0}", SelectedYear),
                obj => {
                    SaveFileDialog d = (SaveFileDialog)obj;
                    d.Filter = "PDF dokument (*.pdf)|*.pdf";
                }
            );

            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            ProgressBarWindowViewModel pb = new ProgressBarWindowViewModel();
            Task.Run(async () => {
                List<Listing> list = new List<Listing>();
                for (int month = 0; month < 12; month++) {
                    list.Add(_listingFactory.Create(SelectedYear, month + 1));
                }

                Document doc = _multipleListingReportFactory.Create(list, new DefaultListingPdfReportSetting());
                _listingReportGenerator.Save(filePath, doc);

                pb.Success = true;
                await Task.Delay(pb.ResultIconDelay);

                pb.TryClose();
            });

            _windowManager.ShowDialog(pb);
        }

    }
}
