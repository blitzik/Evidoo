using Common.FlashMessages;
using intf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.FlashMessages
{
    public class ErrorFlashMessage : FlashMessage
    {
        public ErrorFlashMessage(string message) : base(message)
        {
            _type = Common.FlashMessages.Type.ERROR;
            _viewModel = new FlashMessageErrorViewModel(this);
        }
    }
}
