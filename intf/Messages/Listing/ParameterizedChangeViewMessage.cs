using Common.EventAggregator.Messages;
using intf.BaseViewModels;
using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Messages
{
    public class ParameterizedChangeViewMessage<T> : IChangeViewWithArgumentMessage<IViewModel> where T : IViewModel
    {
        protected string _viewModelName;
        public string ViewModelName
        {
            get { return _viewModelName; }
        }


        protected IViewModel _viewModel;
        public IViewModel ViewModel
        {
            get { return _viewModel; }
        }


        private Action<T> _action;


        public ParameterizedChangeViewMessage(Action<T> action)
        {
            Type t = typeof(T);
            _viewModelName = t.Name;
            _action = action;
        }


        public ParameterizedChangeViewMessage(T viewModel, Action<T> action)
        {
            _viewModel = viewModel;
            _action = action;
        }


        public void Apply(IEnumerable<IViewModel> viewModel)
        {
        }


        public void Apply(IViewModel viewModel)
        {
            _action.Invoke((T)viewModel);
        }
    }
}
