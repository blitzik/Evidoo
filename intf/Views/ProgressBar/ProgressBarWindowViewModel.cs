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
                return _progressViewModel;
            }
        }


        private SuccessViewModel _successViewModel;
        public SuccessViewModel SuccessViewModel
        {
            get
            {
                return _successViewModel;
            }
        }


        private FailureViewModel _failureViewModel;
        public FailureViewModel FailureViewModel
        {
            get
            {
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


        protected override void OnInitialize()
        {
            base.OnInitialize();

            _progressViewModel = PrepareViewModel<ProgressViewModel>();
            _successViewModel = PrepareViewModel<SuccessViewModel>();
            _failureViewModel = PrepareViewModel<FailureViewModel>();
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            Success = null;
        }
    }
}
