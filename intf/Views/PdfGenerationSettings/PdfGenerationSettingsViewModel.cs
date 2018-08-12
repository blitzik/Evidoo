using intf.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.Domain;
using prjt.Facades;

namespace intf.Views
{
    public class PdfGenerationSettingsViewModel : BaseScreen
    {
        private DefaultListingPdfReportSetting _pdfSetting;
        public DefaultListingPdfReportSetting PdfSetting
        {
            get { return _pdfSetting; }
        }


        public event Action<PdfGenerationSettingsViewModel, DefaultListingPdfReportSetting> OnSettingsPropertyChanged;


        private SettingFacade _settingFacade;

        public PdfGenerationSettingsViewModel(SettingFacade settingFacade)
        {
            _settingFacade = settingFacade;
            _pdfSetting = new DefaultListingPdfReportSetting();
            _pdfSetting.OnSettingPropertyChanged += (arg) => {
                OnSettingsPropertyChanged?.Invoke(this, arg);
            };
        }



    }
}
