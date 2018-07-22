using Common.FlashMessages;
using intf.FlashMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Views
{
    public class FlashMessageSuccessViewModel : FlashMessageBaseViewModel
    {
        public FlashMessageSuccessViewModel(IFlashMessage flashMessage) : base(flashMessage)
        {
        }
    }
}
