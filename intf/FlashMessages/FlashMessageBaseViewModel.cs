using Caliburn.Micro;
using Common.Commands;
using Common.FlashMessages;
using intf.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.FlashMessages
{
    public class FlashMessageBaseViewModel : BaseScreen, IFlashMessageViewModel
    {
        private IFlashMessage _flashMessage;
        public IFlashMessage FlashMessage
        {
            get { return _flashMessage; }
        }


        private bool _isMarkedForRemoval;
        public bool IsMarkedForRemoval
        {
            get { return _isMarkedForRemoval; }
            set
            {
                if (_isMarkedForRemoval == value) return;
                Set(ref _isMarkedForRemoval, value);
            }
        }


        private DelegateCommand<object> _removeCommand;
        public DelegateCommand<object> RemoveCommand
        {
            get
            {
                if (_removeCommand == null) {
                    _removeCommand = new DelegateCommand<object>(p => Remove());
                }
                return _removeCommand;
            }
        }


        public FlashMessageBaseViewModel(IFlashMessage flashMessage)
        {
            _flashMessage = flashMessage;
        }


        protected virtual void Remove()
        {
        }
    }
}
