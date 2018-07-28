using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Common.FlashMessages;
using Common.Utils.ResultObject;
using intf.Subscribers.Messages;

namespace intf.Subscribers
{
    public class SettingsSubscriber : BaseSubscriber,
        IHandle<SettingsSuccessfullySavedMessage>,
        IHandle<BackupSuccessfullyCreatedMessage>,
        IHandle<BackupImportedMessage>
    {
        public SettingsSubscriber(IEventAggregator eventAggregator, IFlashMessagesManager flashMessagesManager) : base(eventAggregator, flashMessagesManager)
        {
        }


        public void Handle(SettingsSuccessfullySavedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage("Nastavení bylo uloženo.", Common.FlashMessages.Type.SUCCESS);
        }


        public void Handle(BackupSuccessfullyCreatedMessage message)
        {
            _flashMessagesManager.DisplayFlashMessage("Záloha byla úspěšně vytvořena.", Common.FlashMessages.Type.SUCCESS);
        }


        public void Handle(BackupImportedMessage message)
        {
            foreach (ResultMessage m in message.ResultObject.GetMessages()) {
                _flashMessagesManager.DisplayFlashMessage(
                    m.Text,
                    GetMessageTypeByResultMessageSeverity(m.Severity),
                    m.Severity == ResultObjectMessageSeverity.WARNING ||
                    m.Severity == ResultObjectMessageSeverity.ERROR ? TimeSpan.FromSeconds(6) : TimeSpan.FromSeconds(3)
                );
            }
        }        
    }
}
