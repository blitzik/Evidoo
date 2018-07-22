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
    public class SettingsSubscriber : BaseSubscriber,
        IHandle<SettingsSuccessfullySavedMessage>
    {
        public SettingsSubscriber(IEventAggregator eventAggregator, IFlashMessagesManager flashMessagesManager) : base(eventAggregator, flashMessagesManager)
        {
        }


        public void Handle(SettingsSuccessfullySavedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage(new SuccessFlashMessage("Nastavení bylo uloženo."));
        }
    }
}
