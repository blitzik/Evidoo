using Caliburn.Micro;
using Common.Commands;
using prjt.Domain;
using prjt.Facades;
using intf.Messages;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Common.EventAggregator.Messages;
using intf.BaseViewModels;

namespace intf.Views
{
    public class ListingsOverviewViewModel : BaseScreen
    {
        private DelegateCommand<Listing> _openListingCommand;
        public DelegateCommand<Listing> OpenListingCommand
        {
            get
            {
                if (_openListingCommand == null) {
                    _openListingCommand = new DelegateCommand<Listing>(p => OpenListing(p));
                }

                return _openListingCommand;
            }
        }


        private ICollectionView _listings;
        public ICollectionView Listings
        {
            get { return _listings; }
            set {
                _listings = value;
                value.GroupDescriptions.Add(new PropertyGroupDescription("Month"));

                NotifyOfPropertyChange(() => Listings);
            }
        }


        private List<int> _years = Date.GetYears(2010, "DESC");
        public List<int> Years
        {
            get { return _years; }
        }


        private List<string> _months = new List<string>(Date.Months);
        public List<string> Months
        {
            get { return _months; }
        }


        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                Set(ref _selectedYear, value);

                LoadListings(SelectedYear);
            }
        }


        private readonly ListingFacade _listingFacade;


        public ListingsOverviewViewModel(
            ListingFacade listingFacade
        ) {
            BaseWindowTitle = "Přehled výčetek";
            _listingFacade = listingFacade;
            Listings = CollectionViewSource.GetDefaultView(listingFacade.FindListings(DateTime.Now.Year));

            _selectedYear = DateTime.Now.Year;
        }


        private void LoadListings(int year)
        {
            Listings = CollectionViewSource.GetDefaultView(_listingFacade.FindListings(year));
        }


        private void OpenListing(Listing listing)
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<IViewModel>(nameof(ListingDetailViewModel)));
            EventAggregator.PublishOnUIThread(new ListingMessage(listing));
        }


        // -----


        protected override void OnActivate()
        {
            base.OnActivate();

            LoadListings(SelectedYear);
        }
    }
}
