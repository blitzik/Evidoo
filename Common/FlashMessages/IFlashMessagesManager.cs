using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.FlashMessages
{
    public enum Type
    {
        INFO,
        SUCCESS,
        WARNING,
        ERROR
    }

    public interface IFlashMessagesManager
    {
        bool IsEmpty { get; }
        ObservableCollection<FlashMessageDecorator> FlashMessages { get; }

        void DisplayFlashMessage(string message, Type type);
        void ClearFlashMessages();        
    }
}
