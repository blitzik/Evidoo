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
    public class MainWindowViewModel : BaseConductorOneActive, IHandle<ChangeViewMessage<BaseViewModels.IViewModel>>
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
            ActivateItem(nameof(ListingsOverviewViewModel));
        }


        public void DisplayListingCreation()
        {
            ActivateItem(nameof(ListingViewModel));
        }


        public void DisplayEmployersList()
        {
            ActivateItem(nameof(EmployersViewModel));
        }


        public void DisplaySettings()
        {
            ActivateItem(nameof(SettingsViewModel));
        }


        public void DisplayEmptyListingsGeneration()
        {
            ActivateItem(nameof(EmptyListingsGenerationViewModel));
        }        


        // -----


        public void Handle(ChangeViewMessage<BaseViewModels.IViewModel> message)
        {
            if (message.ViewModel != null) {
                ActivateItem(message.ViewModel);
            } else {
                ActivateItem(message.ViewModelName);
            }
        }


        // -----


        public override void ActivateItem(BaseViewModels.IViewModel item)
        {
            Title = item.WindowTitle;

            base.ActivateItem(item);
        }
    }
}