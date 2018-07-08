using prjt.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.EventAggregator.Messages
{
    public class ChangeViewMessage
    {
        private string _viewModelName;
        public string ViewModelName
        {
            get { return _viewModelName; }
        }


        private IViewModel _viewModel;
        public IViewModel ViewModel
        {
            get { return _viewModel; }
        }


        public ChangeViewMessage(string viewName)
        {
            _viewModelName = viewName;
        }


        public ChangeViewMessage(IViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
