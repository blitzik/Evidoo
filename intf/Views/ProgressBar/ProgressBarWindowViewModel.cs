using Caliburn.Micro;
using Common.Commands;
using intf.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Views
{
    public class ProgressBarWindowViewModel : BaseConductorOneActive
    {
        public string Text
        {
            get { return ProgressViewModel.Text; }
            set
            {
                ProgressViewModel.Text = value;
            }
        }


        private ProgressViewModel _progressViewModel;
        public ProgressViewModel ProgressViewModel
        {
            get
            {
                if (_progressViewModel == null) {
                    _progressViewModel = new ProgressViewModel();
                }
                return _progressViewModel;
            }
        }


        private SuccessViewModel _successViewModel;
        public SuccessViewModel SuccessViewModel
        {
            get
            {
                if (_successViewModel == null) {
                    _successViewModel = new SuccessViewModel();
                }
                return _successViewModel;
            }
        }


        private FailureViewModel _failureViewModel;
        public FailureViewModel FailureViewModel
        {
            get
            {
                if (_failureViewModel == null) {
                    _failureViewModel = new FailureViewModel();
                }
                return _failureViewModel;
            }
        }


        private bool? _success;
        public bool? Success
        {
            get { return _success; }
            set
            {
                _success = value;
                if (value == null) {
                    ActivateItem(ProgressViewModel);

                } else if (value == true) {
                    ActivateItem(SuccessViewModel);

                } else {
                    ActivateItem(FailureViewModel);
                }
            }
        }


        private int _resultIconDelay;
        public int ResultIconDelay
        {
            get { return _resultIconDelay; }
            set { _resultIconDelay = value; }
        }


        public ProgressBarWindowViewModel()
        {
            Success = null;
            ResultIconDelay = 750;
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            Success = null;
        }
    }
}
