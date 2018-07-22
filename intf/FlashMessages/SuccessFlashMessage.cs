using Common.FlashMessages;
using intf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.FlashMessages
{
    public class SuccessFlashMessage : FlashMessage
    {
        public SuccessFlashMessage(string message) : base(message)
        {
            _type = Common.FlashMessages.Type.SUCCESS;
            _viewModel = new FlashMessageSuccessViewModel(this);
        }
    }
}
