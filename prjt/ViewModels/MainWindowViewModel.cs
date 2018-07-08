using Caliburn.Micro;
using prjt.Domain;
using prjt.Messages;
using System.Reflection;
using Common.Commands;
using prjt.ViewModels.Base;
using Common.EventAggregator.Messages;
using Common.ViewModels;

namespace prjt.ViewModels
{
    public class MainWindowViewModel : BaseConductorOneActive, IHandle<ChangeViewMessage<Base.IViewModel>>
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


        public void Handle(ChangeViewMessage<Base.IViewModel> message)
        {
            if (message.ViewModel != null) {
                ActivateItem(message.ViewModel);
            } else {
                ActivateItem(message.ViewModelName);
            }
        }


        // -----


        public override void ActivateItem(Base.IViewModel item)
        {
            Title = item.WindowTitle;

            base.ActivateItem(item);
        }
    }
}