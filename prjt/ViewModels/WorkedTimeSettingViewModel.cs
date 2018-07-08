using Caliburn.Micro;
using Common.Commands;
using prjt.ViewModels.Base;
using prjt.Domain;
using prjt.EventArguments;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.ViewModels
{
    public class WorkedTimeSettingViewModel : BaseScreen
    {
        private int _startTime;
        public int StartTime
        {
            get { return _startTime; }
            set
            {
                TimeSetting.CheckTime(value, EndTime, LunchStart, LunchEnd, OtherHours);

                Set(ref _startTime, value);
                NotifyOfPropertyChange(() => WorkedHours);
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private int _endTime;
        public int EndTime
        {
            get { return _endTime; }
            set
            {
                TimeSetting.CheckTime(StartTime, value, LunchStart, LunchEnd, OtherHours);

                Set(ref _endTime, value);
                NotifyOfPropertyChange(() => WorkedHours);
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private int _lunchStart;
        public int LunchStart
        {
            get { return _lunchStart; }
            set
            {
                TimeSetting.CheckTime(StartTime, EndTime, value, LunchEnd, OtherHours);

                Set(ref _lunchStart, value);
                NotifyOfPropertyChange(() => WorkedHours);
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private int _lunchEnd;
        public int LunchEnd
        {
            get { return _lunchEnd; }
            set
            {
                TimeSetting.CheckTime(StartTime, EndTime, LunchStart, value, OtherHours);

                Set(ref _lunchEnd, value);
                NotifyOfPropertyChange(() => WorkedHours);
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private int _otherHours;
        public int OtherHours
        {
            get { return _otherHours; }
            set
            {
                TimeSetting.CheckTime(StartTime, EndTime, LunchStart, LunchEnd, value);

                Set(ref _otherHours, value);
                NotifyOfPropertyChange(() => WorkedHours);
                SetFlags();
                UpdateCommandsCanExecute();
                ProcessEventOnTimeChanged();
            }
        }


        private bool _noLunch;
        public bool NoLunch
        {
            get { return _noLunch; }
            set
            {
                Set(ref _noLunch, value);

                if (value == true) {
                    _lunchStart = 0;
                    _lunchEnd = 0;

                } else {
                    _lunchStart = StartTime;
                    _lunchEnd = EndTime;
                }

                NotifyOfPropertyChange(() => LunchStart);
                NotifyOfPropertyChange(() => LunchEnd);
                NotifyOfPropertyChange(() => WorkedHours);

                ProcessEventOnTimeChanged();
                UpdateCommandsCanExecute();
            }
        }


        private bool _noTime;
        public bool NoTime
        {
            get { return _noTime; }
            set
            {
                Set(ref _noTime, value);
                if (value == true) {
                    _lastSetTime = new TimeSetting(_startTime, _endTime, _lunchStart, _lunchEnd, _otherHours);
                    SetTime(new TimeSetting());
                    _noLunch = false;

                    NotifyTimePropertiesChanged();
                    UpdateCommandsCanExecute();

                } else {
                    if (_lastSetTime.HasNoTime) {
                        SetDefaultTimes();
                    } else {
                        SetTime(_lastSetTime);
                    }
                }

                ProcessEventOnTimeChanged();
            }
        }


        public Time WorkedHours
        {
            get { return new Time(_endTime - _startTime - (_lunchEnd - _lunchStart) + _otherHours); }
        }


        private List<int> _possibleTimeTicks;
        public List<int> PossibleTimeTicks
        {
            get { return _possibleTimeTicks; }
        }


        private int _timeTickInSeconds;
        public int TimeTickInSeconds
        {
            get { return _timeTickInSeconds; }
        }


        private int _selectedTimeTickInMinutes;
        public int SelectedTimeTickInMinutes
        {
            get { return _selectedTimeTickInMinutes; }
            set
            {
                Set(ref _selectedTimeTickInMinutes, value);
                _timeTickInSeconds = value * 60;
                UpdateCommandsCanExecute();

                if (OnTimeTickChanged != null) {
                    OnTimeTickChanged(this, EventArgs.Empty);
                }
            }
        }


        private DelegateCommand<object> _shiftStartAddCommand;
        public DelegateCommand<object> ShiftStartAddCommand
        {
            get
            {
                if (_shiftStartAddCommand == null) {
                    _shiftStartAddCommand = new DelegateCommand<object>(
                        p => StartTime += TimeTickInSeconds,
                        p => NoTime == false && ((NoLunch == false && (StartTime + TimeTickInSeconds) <= LunchStart) || (NoLunch == true && (StartTime + TimeTickInSeconds) < EndTime))
                    );
                }
                return _shiftStartAddCommand;
            }
        }


        private DelegateCommand<object> _shiftStartSubCommand;
        public DelegateCommand<object> ShiftStartSubCommand
        {
            get
            {
                if (_shiftStartSubCommand == null) {
                    _shiftStartSubCommand = new DelegateCommand<object>(
                        p => StartTime -= TimeTickInSeconds,
                        p => NoTime == false && (StartTime - TimeTickInSeconds) >= 0
                    );
                }
                return _shiftStartSubCommand;
            }
        }


        private DelegateCommand<object> _shiftEndAddCommand;
        public DelegateCommand<object> ShiftEndAddCommand
        {
            get
            {
                if (_shiftEndAddCommand == null) {
                    _shiftEndAddCommand = new DelegateCommand<object>(
                        p => EndTime += TimeTickInSeconds,
                        p => NoTime == false && (EndTime + TimeTickInSeconds) <= 86400 // EndTime < 24:00
                    );
                }
                return _shiftEndAddCommand;
            }
        }


        private DelegateCommand<object> _shiftEndSubCommand;
        public DelegateCommand<object> ShiftEndSubCommand
        {
            get
            {
                if (_shiftEndSubCommand == null) {
                    _shiftEndSubCommand = new DelegateCommand<object>(
                        p => EndTime -= TimeTickInSeconds,
                        p => NoTime == false && ((NoLunch == false && (EndTime - TimeTickInSeconds) >= LunchEnd) || (NoLunch == true && (EndTime - TimeTickInSeconds) > StartTime))
                    );
                }
                return _shiftEndSubCommand;
            }
        }


        private DelegateCommand<object> _lunchStartAddCommand;
        public DelegateCommand<object> LunchStartAddCommand
        {
            get
            {
                if (_lunchStartAddCommand == null) {
                    _lunchStartAddCommand = new DelegateCommand<object>(
                        p => LunchStart += TimeTickInSeconds,
                        p => NoTime == false && NoLunch == false && (LunchStart + TimeTickInSeconds) < LunchEnd
                    );
                }
                return _lunchStartAddCommand;
            }
        }


        private DelegateCommand<object> _lunchStartSubCommand;
        public DelegateCommand<object> LunchStartSubCommand
        {
            get
            {
                if (_lunchStartSubCommand == null) {
                    _lunchStartSubCommand = new DelegateCommand<object>(
                        p => LunchStart -= TimeTickInSeconds,
                        p => NoTime == false && NoLunch == false && (LunchStart - TimeTickInSeconds) >= StartTime
                    );
                }
                return _lunchStartSubCommand;
            }
        }


        private DelegateCommand<object> _lunchEndAddCommand;
        public DelegateCommand<object> LunchEndAddCommand
        {
            get
            {
                if (_lunchEndAddCommand == null) {
                    _lunchEndAddCommand = new DelegateCommand<object>(
                        p => LunchEnd += TimeTickInSeconds,
                        p => NoTime == false && NoLunch == false && (LunchEnd + TimeTickInSeconds) <= EndTime
                    );
                }
                return _lunchEndAddCommand;
            }
        }


        private DelegateCommand<object> _lunchEndSubCommand;
        public DelegateCommand<object> LunchEndSubCommand
        {
            get
            {
                if (_lunchEndSubCommand == null) {
                    _lunchEndSubCommand = new DelegateCommand<object>(
                        p => LunchEnd -= TimeTickInSeconds,
                        p => NoTime == false && NoLunch == false && (LunchEnd - TimeTickInSeconds) > LunchStart
                    );
                }
                return _lunchEndSubCommand;
            }
        }


        private DelegateCommand<object> _otherHoursAddCommand;
        public DelegateCommand<object> OtherHoursAddCommand
        {
            get
            {
                if (_otherHoursAddCommand == null) {
                    _otherHoursAddCommand = new DelegateCommand<object>(
                        p => OtherHours += TimeTickInSeconds,
                        p => NoTime == false && WorkedHours >= 0
                    );
                }
                return _otherHoursAddCommand;
            }
        }


        private DelegateCommand<object> _otherHoursSubCommand;
        public DelegateCommand<object> OtherHoursSubCommand
        {
            get
            {
                if (_otherHoursSubCommand == null) {
                    _otherHoursSubCommand = new DelegateCommand<object>(
                        p => OtherHours -= TimeTickInSeconds,
                        p => NoTime == false && OtherHours > 0
                    );
                }
                return _otherHoursSubCommand;
            }
        }


        public delegate void TimeChangedHandler(object sender, WorkedTimeEventArgs args);
        public event TimeChangedHandler OnTimeChanged;

        public delegate void TimeTickChangedHandler(object sender, EventArgs args);
        public event TimeTickChangedHandler OnTimeTickChanged;
        

        private TimeSetting _defaultTimeSettings;
        private TimeSetting _lastSetTime;


        public WorkedTimeSettingViewModel(TimeSetting defaultTimeSettings, TimeSetting timeSetting, int timeTickInMinutes)
        {
            _defaultTimeSettings = defaultTimeSettings;
            _lastSetTime = timeSetting;

            _startTime = timeSetting.Start.TotalSeconds;
            _endTime = timeSetting.End.TotalSeconds;
            _lunchStart = timeSetting.LunchStart.TotalSeconds;
            _lunchEnd = timeSetting.LunchEnd.TotalSeconds;
            _otherHours = timeSetting.OtherHours.TotalSeconds;

            _possibleTimeTicks = new List<int>();
            _possibleTimeTicks.Add(5);
            _possibleTimeTicks.Add(10);
            _possibleTimeTicks.Add(15);
            _possibleTimeTicks.Add(30);

            SelectedTimeTickInMinutes = timeTickInMinutes;

            SetFlags();
        }


        public void SetTime(TimeSetting timeSetting)
        {
            _startTime = timeSetting.Start.TotalSeconds;
            _endTime = timeSetting.End.TotalSeconds;
            _lunchStart = timeSetting.LunchStart.TotalSeconds;
            _lunchEnd = timeSetting.LunchEnd.TotalSeconds;
            _otherHours = timeSetting.OtherHours.TotalSeconds;

            NotifyTimePropertiesChanged();
            UpdateCommandsCanExecute();
            ProcessEventOnTimeChanged();
        }


        public bool IsTimeEqual(TimeSetting time)
        {
            if (time.Start != StartTime) {
                return false;
            }

            if (time.End != EndTime) {
                return false;
            }

            if (time.LunchStart != LunchStart) {
                return false;
            }

            if (time.LunchEnd != LunchEnd) {
                return false;
            }

            if (time.OtherHours != OtherHours) {
                return false;
            }

            return true;
        }


        private void SetDefaultTimes()
        {
            TimeSetting setting;
            if (_defaultTimeSettings.HasNoTime) {
                // if default time has no time, we have to set some time otherwise there is no way of setting different time from default "HasNoTime"
                setting = new TimeSetting(
                    new Time("06:00"),
                    new Time("14:30"),
                    new Time("10:30"),
                    new Time("11:00"),
                    new Time("00:00")
                );

            } else {
                setting = _defaultTimeSettings;
            }

            _startTime = setting.Start.TotalSeconds;
            _endTime = setting.End.TotalSeconds;
            _lunchStart = setting.LunchStart.TotalSeconds;
            _lunchEnd = setting.LunchEnd.TotalSeconds;
            _otherHours = setting.OtherHours.TotalSeconds;
            
            NotifyTimePropertiesChanged();
            UpdateCommandsCanExecute();
        }


        private void SetFlags()
        {
            if (_startTime == 0 && _endTime == 0 && _lunchStart == 0 && _lunchEnd == 0 && _otherHours == 0) {
                _noTime = true;
                _noLunch = false;

            } else {
                _noTime = false;
                if (_lunchStart == 0 && _lunchEnd == 0) {
                    _noLunch = true;
                } else {
                    _noLunch = false;
                }
            }

            NotifyOfPropertyChange(() => NoTime);
            NotifyOfPropertyChange(() => NoLunch);
        }


        private void NotifyTimePropertiesChanged()
        {
            NotifyOfPropertyChange(() => StartTime);
            NotifyOfPropertyChange(() => EndTime);
            NotifyOfPropertyChange(() => LunchStart);
            NotifyOfPropertyChange(() => LunchEnd);
            NotifyOfPropertyChange(() => OtherHours);
            NotifyOfPropertyChange(() => WorkedHours);

            SetFlags();
        }


        private void UpdateCommandsCanExecute()
        {
            ShiftStartAddCommand.RaiseCanExecuteChanged();
            ShiftStartSubCommand.RaiseCanExecuteChanged();
            ShiftEndAddCommand.RaiseCanExecuteChanged();
            ShiftEndSubCommand.RaiseCanExecuteChanged();

            LunchStartAddCommand.RaiseCanExecuteChanged();
            LunchStartSubCommand.RaiseCanExecuteChanged();
            LunchEndAddCommand.RaiseCanExecuteChanged();
            LunchEndSubCommand.RaiseCanExecuteChanged();

            OtherHoursAddCommand.RaiseCanExecuteChanged();
            OtherHoursSubCommand.RaiseCanExecuteChanged();
        }


        private void ProcessEventOnTimeChanged()
        {
            if (OnTimeChanged != null) {
                OnTimeChanged(this, new WorkedTimeEventArgs(new Time(StartTime), new Time(EndTime), new Time(LunchStart), new Time(LunchEnd), new Time(OtherHours)));
            }
        }
    }

}
