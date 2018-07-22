using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Common.FlashMessages;
using intf.FlashMessages;
using intf.Subscribers.Messages;

namespace intf.Subscribers
{
    public class EmployerSubscriber : BaseSubscriber,
        IHandle<EmployerSuccessfullyDeletedMessage>,
        IHandle<EmployerSuccessfullySavedMessage>
    {
        public EmployerSubscriber(IEventAggregator eventAggregator, IFlashMessagesManager flashMessagesManager) : base(eventAggregator, flashMessagesManager)
        {
        }


        public void Handle(EmployerSuccessfullyDeletedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage(new SuccessFlashMessage("Zaměstnavatel byl úspěšně odstraněn."));
        }


        public void Handle(EmployerSuccessfullySavedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage(new SuccessFlashMessage("Zaměstnavatel byl úspěšně uložen."));
        }
    }
}
