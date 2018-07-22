using Common.FlashMessages;
using intf.FlashMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Views
{
    public class FlashMessageErrorViewModel : FlashMessageBaseViewModel
    {
        public FlashMessageErrorViewModel(IFlashMessage flashMessage) : base(flashMessage)
        {
        }
    }
}
