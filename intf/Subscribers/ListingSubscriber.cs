using Caliburn.Micro;
using Common.EventAggregator.Messages;
using Common.FlashMessages;
using intf.BaseViewModels;
using intf.Messages;
using intf.Subscribers.Messages;
using intf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Subscribers
{
    public class ListingSubscriber : 
        BaseSubscriber,
        IHandle<ListingSuccessfulySavedMessage>,
        IHandle<ListingSuccessfullyDeletedMessage>
    {
        public ListingSubscriber(IEventAggregator eventAggregator, IFlashMessagesManager flashMessagesManager) : base(eventAggregator, flashMessagesManager)
        {
        }

        public void Handle(ListingSuccessfulySavedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage("Výčetka byla úspěšně vytvořena.", Common.FlashMessages.Type.SUCCESS);

            _eventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDetailViewModel>(x => x.Listing = message.Listing));
        }


        public void Handle(ListingSuccessfullyDeletedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage("Výčetka byla úspěšně odstraněna.", Common.FlashMessages.Type.SUCCESS);
            _eventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingsOverviewViewModel>());
        }
    }
}
