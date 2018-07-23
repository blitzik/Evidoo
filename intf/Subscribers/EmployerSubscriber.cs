using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Common.FlashMessages;
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
            _flashMessagesManager.DisplayFlashMessage("Zaměstnavatel byl úspěšně odstraněn.", Common.FlashMessages.Type.SUCCESS);
        }


        public void Handle(EmployerSuccessfullySavedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage("Zaměstnavatel byl úspěšně uložen.", Common.FlashMessages.Type.SUCCESS);
        }
    }
}
