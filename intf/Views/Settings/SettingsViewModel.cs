using Caliburn.Micro;
using Common.Commands;
using Common.Overlay;
using Common.Utils.ResultObject;
using intf.BaseViewModels;
using intf.Messages;
using intf.Subscribers.Messages;
using prjt.Domain;
using prjt.EventArguments;
using prjt.Facades;
using prjt.Services;
using prjt.Services.IO;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace intf.Views
{
    public class SettingsViewModel : BaseScreen
    {
        private WorkedTimeSettingViewModel _workedTimeViewModel;
        public WorkedTimeSettingViewModel WorkedTimeViewModel
        {
            get { return _workedTimeViewModel; }
        }


        private DelegateCommand<object> _saveSettingsCommand;
        public DelegateCommand<object> SaveSettingsCommand
        {
            get
            {
                if (_saveSettingsCommand == null) {
                    _saveSettingsCommand = new DelegateCommand<object>(
                        p => SaveSettings(),
                        p => HasSettingsChanged()
                    );
                }
                return _saveSettingsCommand;
            }
        }


        private DelegateCommand<object> _cancelChangesCommand;
        public DelegateCommand<object> CancelChangesCommand
        {
            get
            {
                if (_cancelChangesCommand == null) {
                    _cancelChangesCommand = new DelegateCommand<object>(
                        p => CancelChanges(),
                        p => HasSettingsChanged()
                    );
                }
                return _cancelChangesCommand;
            }
        }


        private DelegateCommand<object> _backupDataCommand;
        public DelegateCommand<object> BackupDataCommand
        {
            get
            {
                if (_backupDataCommand == null) {
                    _backupDataCommand = new DelegateCommand<object>(p => CreateBackup());
                }
                return _backupDataCommand;
            }
        }


        private string _backupFilePath;
        public string BackupFilePath
        {
            get { return _backupFilePath; }
            set
            {
                _backupFilePath = value;
                NotifyOfPropertyChange(() => BackupFilePath);
                ImportDataCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand<object> _browseCommand;
        public DelegateCommand<object> BrowseCommand
        {
            get
            {
                if (_browseCommand == null) {
                    _browseCommand = new DelegateCommand<object>(p => Browse());
                }
                return _browseCommand;
            }
        }


        private DelegateCommand<object> _importDataCommand;
        public DelegateCommand<object> ImportDataCommand
        {
            get
            {
                if (_importDataCommand == null) {
                    _importDataCommand = new DelegateCommand<object>(
                        p => ImportBackup(),
                        p => !string.IsNullOrEmpty(BackupFilePath) && File.Exists(BackupFilePath)
                    );
                }
                return _importDataCommand;
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
                CancelChangesCommand.RaiseCanExecuteChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }


        // -----


        private DefaultSettings _defaultSetting;

        private IWindowManager _windowManager;
        private IIODialogService _filePathDialogService;
        private SettingFacade _settingFacade;

        public SettingsViewModel(
            IWindowManager windowManager,
            SettingFacade settingFacade,
            IIODialogService filePathDialogService
        ) {
            _windowManager = windowManager;
            _settingFacade = settingFacade;
            _filePathDialogService = filePathDialogService;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            BaseWindowTitle = "Nastavení";
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            Reset();
        }


        public void Reset()
        {
            _defaultSetting = _settingFacade.GetDefaultSettings();

            PdfSetting = CreateNewPdfSetting(_defaultSetting.Pdfsetting);

            if (_workedTimeViewModel == null) {
                _workedTimeViewModel = PrepareViewModel(() => { return new WorkedTimeSettingViewModel(_defaultSetting.Time, _defaultSetting.Time, _defaultSetting.TimeTickInMinutes); });
                _workedTimeViewModel.OnTimeChanged += (object sender, WorkedTimeEventArgs args) =>
                {
                    CancelChangesCommand.RaiseCanExecuteChanged();
                    SaveSettingsCommand.RaiseCanExecuteChanged();
                };
                _workedTimeViewModel.OnTimeTickChanged += (object sender, EventArgs args) =>
                {
                    CancelChangesCommand.RaiseCanExecuteChanged();
                    SaveSettingsCommand.RaiseCanExecuteChanged();
                };
            }
            _workedTimeViewModel.SetTime(_defaultSetting.Time);
            _workedTimeViewModel.SelectedTimeTickInMinutes = _defaultSetting.TimeTickInMinutes;
        }


        private bool HasSettingsChanged()
        {
            if (!_workedTimeViewModel.IsTimeEqual(_defaultSetting.Time)) {
                return true;
            }

            if (_workedTimeViewModel.SelectedTimeTickInMinutes != _defaultSetting.TimeTickInMinutes) {
                return true;
            }

            if (!_pdfSetting.IsEqual(_defaultSetting.Pdfsetting)) {
                return true;
            }

            return false;
        }


        private void SaveSettings()
        {
            _defaultSetting.Time = new TimeSetting(
                new Time(_workedTimeViewModel.StartTime),
                new Time(_workedTimeViewModel.EndTime),
                new Time(_workedTimeViewModel.LunchStart),
                new Time(_workedTimeViewModel.LunchEnd),
                new Time(_workedTimeViewModel.OtherHours)
            );
            _defaultSetting.TimeTickInMinutes = _workedTimeViewModel.SelectedTimeTickInMinutes;
            _defaultSetting.Pdfsetting = new DefaultListingPdfReportSetting(PdfSetting);

            _settingFacade.UpdateDefaultSettings(_defaultSetting);

            CancelChangesCommand.RaiseCanExecuteChanged();
            SaveSettingsCommand.RaiseCanExecuteChanged();

            EventAggregator.PublishOnUIThread(new SettingsSuccessfullySavedMessage());
        }


        private void CancelChanges()
        {
            WorkedTimeViewModel.SetTime(_defaultSetting.Time);
            WorkedTimeViewModel.SelectedTimeTickInMinutes = _defaultSetting.TimeTickInMinutes;

            PdfSetting = CreateNewPdfSetting(_defaultSetting.Pdfsetting);
        }


        private void Browse()
        {
            string filePath = _filePathDialogService.GetFilePath<OpenFileDialog>(null, d => {
                d.DefaultExt = "." + PerstStorageFactory.DATABASE_EXTENSION;
                d.Filter = "Evidoo data (*.evdo)|*.evdo";
            });
            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            BackupFilePath = filePath;
        }


        private void CreateBackup()
        {
            DateTime now = DateTime.Now;
            string filePath = _filePathDialogService.GetFilePath<SaveFileDialog>(
                string.Format("Záloha dat - {0}-{1}-{2}", now.Day, now.Month, now.Year),
                d => { d.Filter = "Evidoo data (*.evdo)|*.evdo"; }
            );
            if (string.IsNullOrEmpty(filePath)) {
                return;
            }

            IOverlayToken ot = Overlay.DisplayOverlay(PrepareViewModel<ProgressViewModel>());
            Task.Run(() => {
                ResultObject<object> ro = _settingFacade.BackupData(filePath);

                ot.HideOverlay();
                EventAggregator.PublishOnUIThread(new BackupSuccessfullyCreatedMessage());
            });
        }


        private async void ImportBackup()
        {
            IOverlayToken ot = Overlay.DisplayOverlay(PrepareViewModel<ProgressViewModel>());
            Task<ResultObject<object>> t = Task.Run(() => {
                ResultObject<object> r = _settingFacade.ImportBackup(BackupFilePath);

                ot.HideOverlay();
                return r;
            });

            ResultObject<object> ro = await t;
            EventAggregator.PublishOnUIThread(new BackupImportedMessage(ro));

            _defaultSetting = _settingFacade.GetDefaultSettings();
            Reset();

            BackupFilePath = null;            
        }


        private DefaultListingPdfReportSetting CreateNewPdfSetting(DefaultListingPdfReportSetting oldSetting)
        {
            DefaultListingPdfReportSetting setting = new DefaultListingPdfReportSetting(oldSetting);
            setting.OnSettingPropertyChanged += (object sender, EventArgs args) => {
                CancelChangesCommand.RaiseCanExecuteChanged();
                SaveSettingsCommand.RaiseCanExecuteChanged();
            };

            return setting;
        }
    }
}
