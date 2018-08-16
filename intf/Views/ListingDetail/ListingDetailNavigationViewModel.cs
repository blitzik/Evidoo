using intf.BaseViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using intf.Messages;
using prjt.Domain;

namespace intf.Views
{
    public class ListingDetailNavigationViewModel : BaseScreen, ISecondNavigationViewModel
    {
        private IViewModel _currentlyActivatedItem;
        public IViewModel CurrentlyActivatedItem
        {
            get { return _currentlyActivatedItem; }
            set { Set(ref _currentlyActivatedItem, value); }
        }


        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set { Set(ref _listing, value); }
        }


        public ListingDetailNavigationViewModel(Listing listing)
        {
            _listing = listing;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
        }


        public void DisplayListingDetail()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDetailViewModel>(x => {
                x.SecondNavigation = this;
            }));
        }


        public void DisplayPDFGeneration()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingPdfGenerationViewModel>(x => {
                x.Listing = _listing;
                x.SecondNavigation = this;
            }));
        }


        public void DisplayListingEditing()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingEditingViewModel>(x => {
                x.Listing = _listing;
                x.SecondNavigation = this;
            }));
        }


        public void DisplayListingDeletion()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDeletionViewModel>(x => {
                x.Listing = _listing;
                x.SecondNavigation = this;
            }));
        }


        public void DisplayCopyListing()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<CopyListingViewModel>(x => {
                x.Listing = _listing;
                x.SecondNavigation = this;
            }));
        }
    }
}
