using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventAggregator.Messages
{
    public class ChangeViewMessage<T>
    {
        private string _viewModelName;
        public string ViewModelName
        {
            get { return _viewModelName; }
        }


        private T _viewModel;
        public T ViewModel
        {
            get { return _viewModel; }
        }


        public ChangeViewMessage(string viewName)
        {
            _viewModelName = viewName;
        }


        public ChangeViewMessage(T viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
