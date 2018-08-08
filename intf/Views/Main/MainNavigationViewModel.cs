using Caliburn.Micro;
using Common.Commands;
using intf.BaseViewModels;
using intf.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Views
{
    public class MainNavigationViewModel : BaseScreen
    {       

        private IViewModel _activeItem;
        public IViewModel ActiveItem
        {
            get { return _activeItem; }
            set
            {
                Set(ref _activeItem, value);
                
                bool wasActive = SecondNavigation != null;
                if (wasActive == true && value.SecondNavigation == null) { // todo
                    CanHide = true;
                    IsSecondNavigationActive = false;
                    Task.Factory.StartNew(async () => {
                        await Task.Delay(125);
                        SecondNavigation = null;
                    });

                } else {
                    if (value.SecondNavigation == SecondNavigation) {
                        return;
                    }
                    SecondNavigation = value.SecondNavigation;
                    if (SecondNavigation != null) {
                        IsSecondNavigationActive = true;
                        SecondNavigation.CurrentlyActivatedItem = ActiveItem;
                    }
                }
                
            }
        }


        private bool _canHide;
        public bool CanHide
        {
            get { return _canHide; }
            set { Set(ref _canHide, value); }
        }


        private DelegateCommand<MainNavigationViewModel> _hideNavCommand;
        public DelegateCommand<MainNavigationViewModel> HideNavCommand
        {
            get
            {
                if (_hideNavCommand == null) {
                    _hideNavCommand = new DelegateCommand<MainNavigationViewModel>(p => {
                        SecondNavigation = null;
                        CanHide = false;
                    });
                }
                return _hideNavCommand;
            }
        }


        public MainNavigationViewModel()
        {

        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            DisplayListingsOverview();
        }


        protected override void OnActivate()
        {
            base.OnActivate();


        }


        public void DisplayListingsOverview()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingsOverviewViewModel>());
        }


        public void DisplayListingCreation()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingViewModel>());
        }


        public void DisplayEmployersList()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<EmployersViewModel>());
        }


        public void DisplaySettings()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<SettingsViewModel>());
        }


        public void DisplayEmptyListingsGeneration()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<EmptyListingsGenerationViewModel>());
        }
    }
}
