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
        ObservableCollection<IFlashMessageViewModel> Items { get; }

        void DisplayFlashMessage(IFlashMessage flashMessage);
        void ClearFlashMessage(IFlashMessageViewModel vm);
        void ClearFlashMessages();        
    }
}
