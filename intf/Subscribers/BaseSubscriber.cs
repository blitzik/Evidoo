using Caliburn.Micro;
using Common.FlashMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Subscribers
{
    public abstract class BaseSubscriber
    {
        protected IEventAggregator _eventAggregator;
        protected IFlashMessagesManager _flashMessagesManager;

        public BaseSubscriber(
            IEventAggregator eventAggregator,
            IFlashMessagesManager flashMessagesManager
        ) {
            _eventAggregator = eventAggregator;
            _flashMessagesManager = flashMessagesManager;

            eventAggregator.Subscribe(this);
        }
    }
}
