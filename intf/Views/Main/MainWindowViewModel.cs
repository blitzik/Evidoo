using Caliburn.Micro;
using prjt.Domain;
using intf.Messages;
using System.Reflection;
using Common.EventAggregator.Messages;
using intf.BaseViewModels;

namespace intf.Views
{
    public class MainWindowViewModel :
        BaseConductorOneActive,
        IHandle<IChangeViewMessage<BaseViewModels.IViewModel>>,
        IHandle<DisplayOverlayMessage>,
        IHandle<HideOverlayMessage>
    {
        private PageTitle _title = new PageTitle();
        public PageTitle Title
        {
            get { return _title; }
            private set
            {
                Set(ref _title, value);
            }
        }


        private string _version;
        public string AppVersion
        {
            get { return _version; }
        }


        public MainWindowViewModel()
        {
            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        // -----


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            OverlayState = OverlayState.HIDDEN;
            DisplayListingsOverview();
        }


        public override void ActivateItem(BaseViewModels.IViewModel item)
        {
            Title = item.WindowTitle;

            base.ActivateItem(item);
        }


        // -----


        public void DisplayListingsOverview()
        {
            Handle(new ChangeViewMessage<ListingsOverviewViewModel>());
        }


        public void DisplayListingCreation()
        {
            Handle(new ChangeViewMessage<ListingViewModel>());
        }


        public void DisplayEmployersList()
        {
            Handle(new ChangeViewMessage<EmployersViewModel>());
        }


        public void DisplaySettings()
        {
            Handle(new ChangeViewMessage<SettingsViewModel>());
        }


        public void DisplayEmptyListingsGeneration()
        {
            Handle(new ChangeViewMessage<EmptyListingsGenerationViewModel>());
        }


        // -----


        public void Handle(IChangeViewMessage<BaseViewModels.IViewModel> message)
        {
            IViewModel vm;
            if (message.ViewModel != null) {
                vm = message.ViewModel;
            } else {
                vm = GetViewModel(message.Type);
            }

            if (vm == ActiveItem) {
                return;
            }
            ActivateItem(vm);
            message.Apply(ActiveItem);
        }


        // -----


        private BaseViewModels.IViewModel _overlayWindowViewModel;
        public BaseViewModels.IViewModel OverlayWindowViewModel
        {
            get { return _overlayWindowViewModel; }
            set { Set(ref _overlayWindowViewModel, value); }
        }


        private OverlayState _overlayState;
        public OverlayState OverlayState
        {
            get { return _overlayState; }
            set { Set(ref _overlayState, value); }
        }


        public void Handle(DisplayOverlayMessage message)
        {
            OverlayState = OverlayState.VISIBLE;
            if (message.ViewModel != null) {
                OverlayWindowViewModel = message.ViewModel;
            } else {
                OverlayWindowViewModel = GetViewModel(message.Type);
            }
        }


        public void Handle(HideOverlayMessage message)
        {
            OverlayState = OverlayState.HIDDEN;
        }
    }
}