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
        IHandle<IChangeViewMessage<BaseViewModels.IViewModel>>
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

            bool b = Overlay.IsActive;

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
            message.Apply(vm);
            ActivateItem(vm);
        }
    }
}