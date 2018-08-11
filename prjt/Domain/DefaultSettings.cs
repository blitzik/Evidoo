using prjt.Utils;

namespace prjt.Domain
{
    public class DefaultSettings
    {
        public const string MAIN_SETTINGS_ID = "main";


        private readonly string _id;
        public string ID
        {
            get { return _id; }
        }


        private TimeSetting _time;
        public TimeSetting Time
        {
            get { return _time; }
            set
            {
                _time = value;
            }
        }


        private int _timeTickInMinutes;
        public int TimeTickInMinutes
        {
            get { return _timeTickInMinutes; }
            set { _timeTickInMinutes = value; }
        }


        private DefaultListingPdfReportSetting _pdfSetting;
        public DefaultListingPdfReportSetting Pdfsetting
        {
            get { return _pdfSetting; }
            set
            {
                _pdfSetting = value;
            }
        }


        private DefaultSettings() { }


        public DefaultSettings(string identifier)
        {
            _id = identifier;

            Time = new TimeSetting(
                new Time("06:00"),
                new Time("14:30"),
                new Time("10:30"),
                new Time("11:00"),
                new Time("00:00")
            );

            TimeTickInMinutes = 5;

            Pdfsetting = new DefaultListingPdfReportSetting();
            Pdfsetting.ResetSettings();
        }


        public DefaultSettings(string identifier, TimeSetting timeSetting, int timeTickInMinutes)
        {
            _id = identifier;

            _time = timeSetting;
            _timeTickInMinutes = timeTickInMinutes;
        }
 
    }
}
