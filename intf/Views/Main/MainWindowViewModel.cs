﻿using Caliburn.Micro;
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
            ActivateItem(typeof(ListingsOverviewViewModel).FullName);
        }


        public void DisplayListingCreation()
        {
            ActivateItem(typeof(ListingViewModel).FullName);
        }


        public void DisplayEmployersList()
        {
            ActivateItem(typeof(EmployersViewModel).FullName);
        }


        public void DisplaySettings()
        {
            ActivateItem(typeof(SettingsViewModel).FullName);
        }


        public void DisplayEmptyListingsGeneration()
        {
            ActivateItem(typeof(EmptyListingsGenerationViewModel).FullName);
        }


        // -----


        public void Handle(IChangeViewMessage<BaseViewModels.IViewModel> message)
        {
            if (message.ViewModel != null) {
                ActivateItem(message.ViewModel);
            } else {
                ActivateItem(message.ViewModelName);
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