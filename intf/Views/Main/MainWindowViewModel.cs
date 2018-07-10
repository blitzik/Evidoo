using Caliburn.Micro;
using prjt.Domain;
using intf.Messages;
using System.Reflection;
using Common.Commands;
using Common.EventAggregator.Messages;
using Common.ViewModels;
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


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);

            DisplayListingsOverview();
        }


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
            if (message.ViewModel != null) {
                ActivateItem(message.ViewModel);
            } else {
                ActivateItem(GetViewModel(message.Type));
            }
            message.Apply(ActiveItem);
        }


        // -----


        public override void ActivateItem(BaseViewModels.IViewModel item)
        {
            Title = item.WindowTitle;

            base.ActivateItem(item);
        }
    }
}