using Caliburn.Micro;
using Common.Commands;
using prjt.Domain;
using prjt.Facades;
using intf.Messages;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EventAggregator.Messages;
using intf.BaseViewModels;
using intf.Subscribers.Messages;

namespace intf.Views
{
    public class ListingDeletionViewModel : BaseScreen
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            set
            {
                _listing = value;
                if (value == null) {
                    WindowTitle.Text = BaseWindowTitle;

                } else {
                    WindowTitle.Text = string.Format("{0} [{1} {2} {3}]", BaseWindowTitle, Date.Months[12 - value.Month], value.Year, string.Format("- {0}", value.Name));
                }
            }
        }


        private string _confirmationText;
        public string ConfirmationText
        {
            get { return _confirmationText; }
            set {
                Set(ref _confirmationText, value?.ToLower());
                DeleteListingCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand<object> _deleteListingCommand;
        public DelegateCommand<object> DeleteListingCommand
        {
            get
            {
                if (_deleteListingCommand == null) {
                    _deleteListingCommand = new DelegateCommand<object>(
                        p => DeleteListing(),
                        p => _confirmationText == "odstranit"
                    );
                }

                return _deleteListingCommand;
            }
        }


        private DelegateCommand<object> _returnBackCommand;
        public DelegateCommand<object> ReturnBackCommand
        {
            get
            {
                if (_returnBackCommand == null) {
                    _returnBackCommand = new DelegateCommand<object>(p => ReturnBack());
                }

                return _returnBackCommand;
            }
        }


        private ListingFacade _listingFacade;

        public ListingDeletionViewModel(ListingFacade listingFacade)
        {
            BaseWindowTitle = "Odstranění výčetky";

            _listingFacade = listingFacade;            
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventAggregator.Subscribe(this);
        }


        private void DeleteListing()
        {
            _listingFacade.DeleteListing(Listing);
            Listing = null;
            ConfirmationText = null;

            EventAggregator.PublishOnUIThread(new ListingSuccessfullyDeletedMessage(Listing));
        }


        private void ReturnBack()
        {
            EventAggregator.PublishOnUIThread(new ChangeViewMessage<IViewModel>(nameof(ListingDetailViewModel)));
        }
    }
}
