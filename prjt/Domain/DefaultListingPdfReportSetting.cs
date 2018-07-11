using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Domain
{
    public class DefaultListingPdfReportSetting : PropertyChangedBase
    {
        private string _ownerName;
        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                _ownerName = value;
                NotifyOfPropertyChange(() => OwnerName);
                ProcessOnPropertyChanged();
            }
        }


        private bool _isOwnerNameVisible;
        public bool IsOwnerNameVisible
        {
            get { return _isOwnerNameVisible; }
            set
            {
                _isOwnerNameVisible = value;
                NotifyOfPropertyChange(() => IsOwnerNameVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _isEmployerVisible;
        public bool IsEmployerVisible
        {
            get { return _isEmployerVisible; }
            set
            {
                _isEmployerVisible = value;
                NotifyOfPropertyChange(() => IsEmployerVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _isVacationVisible;
        public bool IsVacationVisible
        {
            get { return _isVacationVisible; }
            set
            {
                _isVacationVisible = value;
                NotifyOfPropertyChange(() => IsVacationVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areOtherHoursVisible;
        public bool AreOtherHoursVisible
        {
            get { return _areOtherHoursVisible; }
            set
            {
                _areOtherHoursVisible = value;
                NotifyOfPropertyChange(() => AreOtherHoursVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areShortHalfHoursEnabled;
        public bool AreShortHalfHoursEnabled
        {
            get { return _areShortHalfHoursEnabled; }
            set
            {
                _areShortHalfHoursEnabled = value;
                NotifyOfPropertyChange(() => AreShortHalfHoursEnabled);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areWorkedHoursVisible;
        public bool AreWorkedHoursVisible
        {
            get { return _areWorkedHoursVisible; }
            set
            {
                _areWorkedHoursVisible = value;
                NotifyOfPropertyChange(() => AreWorkedHoursVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areSiknessHours;
        public bool AreSiknessHoursVisible
        {
            get { return _areSiknessHours; }
            set
            {
                _areSiknessHours = value;
                NotifyOfPropertyChange(() => AreSiknessHoursVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areHolidaysHoursVisible;
        public bool AreHolidaysHoursVisible
        {
            get { return _areHolidaysHoursVisible; }
            set
            {
                _areHolidaysHoursVisible = value;
                NotifyOfPropertyChange(() => AreHolidaysHoursVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areLunchHoursVisible;
        public bool AreLunchHoursVisible
        {
            get { return _areLunchHoursVisible; }
            set
            {
                _areLunchHoursVisible = value;
                NotifyOfPropertyChange(() => AreLunchHoursVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areTotalWorkedHoursVisible;
        public bool AreTotalWorkedHoursVisible
        {
            get { return _areTotalWorkedHoursVisible; }
            set
            {
                _areTotalWorkedHoursVisible = value;
                NotifyOfPropertyChange(() => AreTotalWorkedHoursVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _isHourlyWageVisible;
        public bool IsHourlyWageVisible
        {
            get { return _isHourlyWageVisible; }
            set
            {
                _isHourlyWageVisible = value;
                NotifyOfPropertyChange(() => IsHourlyWageVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areVacationDaysVisible;
        public bool AreVacationDaysVisible
        {
            get { return _areVacationDaysVisible; }
            set
            {
                _areVacationDaysVisible = value;
                NotifyOfPropertyChange(() => AreVacationDaysVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areDietsVisible;
        public bool AreDietsVisible
        {
            get { return _areDietsVisible; }
            set
            {
                _areDietsVisible = value;
                NotifyOfPropertyChange(() => AreDietsVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _arePaidHolidaysVisible;
        public bool ArePaidHolidaysVisible
        {
            get { return _arePaidHolidaysVisible; }
            set
            {
                _arePaidHolidaysVisible = value;
                NotifyOfPropertyChange(() => ArePaidHolidaysVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _areBonusesVisible;
        public bool AreBonusesVisible
        {
            get { return _areBonusesVisible; }
            set
            {
                _areBonusesVisible = value;
                NotifyOfPropertyChange(() => AreBonusesVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _aAreDollarsVisible;
        public bool AreDollarsVisible
        {
            get { return _aAreDollarsVisible; }
            set
            {
                _aAreDollarsVisible = value;
                NotifyOfPropertyChange(() => AreDollarsVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _isPrepaymentVisible;
        public bool IsPrepaymentVisible
        {
            get { return _isPrepaymentVisible; }
            set
            {
                _isPrepaymentVisible = value;
                NotifyOfPropertyChange(() => IsPrepaymentVisible);
                ProcessOnPropertyChanged();
            }
        }


        private bool _isSicknessVisible;
        public bool IsSicknessVisible
        {
            get { return _isSicknessVisible; }
            set
            {
                _isSicknessVisible = value;
                NotifyOfPropertyChange(() => IsSicknessVisible);
                ProcessOnPropertyChanged();
            }
        }


        public DefaultListingPdfReportSetting() { }


        public DefaultListingPdfReportSetting(DefaultListingPdfReportSetting setting)
        {
            UpdateBy(setting);
        }


        public void UpdateBy(DefaultListingPdfReportSetting setting)
        {
            OwnerName = setting.OwnerName;

            IsEmployerVisible = setting.IsEmployerVisible;
            IsOwnerNameVisible = setting.IsOwnerNameVisible;

            AreShortHalfHoursEnabled = setting.AreShortHalfHoursEnabled;
            AreWorkedHoursVisible = setting.AreWorkedHoursVisible;
            AreLunchHoursVisible = setting.AreLunchHoursVisible;
            AreOtherHoursVisible = setting.AreOtherHoursVisible;
            AreTotalWorkedHoursVisible = setting.AreTotalWorkedHoursVisible;
            IsVacationVisible = setting.IsVacationVisible;
            AreSiknessHoursVisible = setting.AreSiknessHoursVisible;
            AreHolidaysHoursVisible = setting.AreHolidaysHoursVisible;

            IsHourlyWageVisible = setting.IsHourlyWageVisible;
            AreVacationDaysVisible = setting.AreVacationDaysVisible;
            AreDietsVisible = setting.AreDietsVisible;
            ArePaidHolidaysVisible = setting.ArePaidHolidaysVisible;
            AreBonusesVisible = setting.AreBonusesVisible;
            AreDollarsVisible = setting.AreDollarsVisible;
            IsPrepaymentVisible = setting.IsPrepaymentVisible;
            IsSicknessVisible = setting.IsSicknessVisible;
        }


        public delegate void PropertyChangedHandler(object sender, EventArgs args);
        public event PropertyChangedHandler OnSettingPropertyChanged;

        private void ProcessOnPropertyChanged()
        {
            if (OnSettingPropertyChanged != null) {
                OnSettingPropertyChanged(this, EventArgs.Empty);
            }
        }


        public bool IsEqual(object obj)
        {
            if (obj == null) {
                return false;
            }

            if (object.ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            DefaultListingPdfReportSetting setting = (DefaultListingPdfReportSetting)obj;

            if (string.IsNullOrEmpty(OwnerName)) {
                if (!string.IsNullOrEmpty(setting.OwnerName)) { return false; }
            } else {
                if (!OwnerName.Equals(setting.OwnerName)) { return false; }
            }

            if (IsEmployerVisible != setting.IsEmployerVisible) { return false; }
            if (IsOwnerNameVisible != setting.IsOwnerNameVisible) { return false; }

            if (AreShortHalfHoursEnabled != setting.AreShortHalfHoursEnabled) { return false; }
            if (AreWorkedHoursVisible != setting.AreWorkedHoursVisible) { return false; }
            if (AreLunchHoursVisible != setting.AreLunchHoursVisible) { return false; }
            if (AreOtherHoursVisible != setting.AreOtherHoursVisible) { return false; }
            if (AreTotalWorkedHoursVisible != setting.AreTotalWorkedHoursVisible) { return false; }
            if (IsVacationVisible != setting.IsVacationVisible) { return false; }
            if (AreSiknessHoursVisible != setting.AreSiknessHoursVisible) { return false; }
            if (AreHolidaysHoursVisible != setting.AreHolidaysHoursVisible) { return false; }

            if (IsHourlyWageVisible != setting.IsHourlyWageVisible) { return false; }
            if (AreVacationDaysVisible != setting.AreVacationDaysVisible) { return false; }
            if (AreDietsVisible != setting.AreDietsVisible) { return false; }
            if (ArePaidHolidaysVisible != setting.ArePaidHolidaysVisible) { return false; }
            if (AreBonusesVisible != setting.AreBonusesVisible) { return false; }
            if (AreDollarsVisible != setting.AreDollarsVisible) { return false; }
            if (IsPrepaymentVisible != setting.IsPrepaymentVisible) { return false; }
            if (IsSicknessVisible != setting.IsSicknessVisible) { return false; }

            return true;
        }


        public void ResetSettings()
        {
            OwnerName = null;

            IsEmployerVisible = true;
            IsOwnerNameVisible = true;

            AreShortHalfHoursEnabled = false;
            AreWorkedHoursVisible = true;
            AreLunchHoursVisible = true;
            AreOtherHoursVisible = true;
            AreTotalWorkedHoursVisible = true;
            IsVacationVisible = true;
            AreSiknessHoursVisible = true;
            AreHolidaysHoursVisible = true;
            
            IsHourlyWageVisible = true;
            AreVacationDaysVisible = true;
            AreDietsVisible = true;
            ArePaidHolidaysVisible = true;
            AreBonusesVisible = true;
            AreDollarsVisible = true;
            IsPrepaymentVisible = true;
            IsSicknessVisible = true;
        }
    }
}
