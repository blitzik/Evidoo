﻿using Caliburn.Micro;
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
using System.Threading;
using System.Threading.Tasks;

namespace intf.Subscribers
{
    public class ListingSubscriber : BaseSubscriber,
        IHandle<ListingSuccessfulySavedMessage>,
        IHandle<ListingSuccessfullyDeletedMessage>,
        IHandle<ListingPdfSuccessfullyGeneratedMessage>,
        IHandle<ListingSuccessfullyCopiedMessage>
    {
        public ListingSubscriber(IEventAggregator eventAggregator, IFlashMessagesManager flashMessagesManager) : base(eventAggregator, flashMessagesManager)
        {
        }

        public void Handle(ListingSuccessfulySavedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage("Výčetka byla úspěšně uložena.", Common.FlashMessages.Type.SUCCESS);

            _eventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDetailViewModel>(x => x.Listing = message.Listing));
        }


        public void Handle(ListingSuccessfullyDeletedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage("Výčetka byla úspěšně odstraněna.", Common.FlashMessages.Type.SUCCESS);
            _eventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingsOverviewViewModel>());
        }


        public void Handle(ListingPdfSuccessfullyGeneratedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage("Váš PDF dokument byl úspěšně uložen.", Common.FlashMessages.Type.SUCCESS);
        }


        public void Handle(ListingSuccessfullyCopiedMessage message)
        {
            _eventAggregator.PublishOnUIThread(new ChangeViewMessage<ListingDetailViewModel>(x => x.Listing = message.Listing));
            _flashMessagesManager.DisplayFlashMessage("Kopie výčetky byla úspěšně vytvořena.", Common.FlashMessages.Type.SUCCESS);
        }
    }
}
