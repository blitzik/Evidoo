using Caliburn.Micro;
using Common.Commands;
using intf.BaseViewModels;
using intf.Messages;
using prjt.Domain;
using prjt.Facades;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

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
                    _openListingCommand = new DelegateCommand<Listing>(
                        listing => EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDetailViewModel>(x => { x.Listing = listing; }))
                    );
                }

                return _openListingCommand;
            }
        }


        private DelegateCommand<object> _displayListingsPdfGenerationCommand;
        public DelegateCommand<object> DisplayListingsPdfGenerationCommand
        {
            get
            {
                if (_displayListingsPdfGenerationCommand == null) {
                    _displayListingsPdfGenerationCommand = new DelegateCommand<object>(
                        p => EventAggregator.PublishOnUIThread(
                            new ChangeViewMessage<ListingsPdfGenerationViewModel>(x => {
                                x.SetDefaultPeriod(SelectedYear, SelectedMonth);
                            })
                        )
                    );
                }
                return _displayListingsPdfGenerationCommand;
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


        private List<int> _years;
        public List<int> Years
        {
            get { return _years; }
        }


        private List<string> _months;
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
                LoadListings(SelectedYear, SelectedMonth);
            }
        }


        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                Set(ref _selectedMonth, value);
                LoadListings(SelectedYear, SelectedMonth);
            }
        }


        private ListingFacade _listingFacade;

        public ListingsOverviewViewModel(
            ListingFacade listingFacade
        ) {
            _listingFacade = listingFacade;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            BaseWindowTitle = "Přehled výčetek";

            _years = Date.GetYears(2010, "DESC");
            _months = new List<string>(Date.Months);
            _months.Reverse();
            _months.Insert(0, "Celý rok");

            _selectedYear = DateTime.Now.Year;
            NotifyOfPropertyChange(() => SelectedYear);
            _selectedMonth = DateTime.Now.Month;
            NotifyOfPropertyChange(() => SelectedMonth);
        }


        protected override void OnActivate()
        {
            base.OnActivate();

            LoadListings(SelectedYear, SelectedMonth);
        }


        private void LoadListings(int year, int month)
        {
            Listings = CollectionViewSource.GetDefaultView(_listingFacade.FindListings(year, month));
        }
    }
}
