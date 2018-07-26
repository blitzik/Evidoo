using intf.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Messages
{
    public enum OverlayState
    {
        VISIBLE,
        HIDDEN
    }


    public class DisplayOverlayMessage
    {
        private Type _type;
        public Type Type
        {
            get { return _type; }
        }


        private IViewModel _viewModel;
        public IViewModel ViewModel
        {
            get { return _viewModel; }
        }


        public DisplayOverlayMessage(IViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
