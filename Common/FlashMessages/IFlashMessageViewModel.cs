using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.FlashMessages
{
    public interface IFlashMessageViewModel : IViewModel
    {
        IFlashMessage FlashMessage { get; }
    }
}
