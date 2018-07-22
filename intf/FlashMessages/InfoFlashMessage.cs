using Common.FlashMessages;
using intf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.FlashMessages
{
    public class InfoFlashMessage : FlashMessage
    {
        public InfoFlashMessage(string message) : base(message)
        {
            _type = Common.FlashMessages.Type.INFO;
            _viewModel = new FlashMessageInfoViewModel(this);
        }
    }
}
