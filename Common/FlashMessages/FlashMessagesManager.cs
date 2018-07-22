using Caliburn.Micro;
using Common.ViewModelResolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Common.FlashMessages
{
    public class FlashMessagesManager : PropertyChangedBase, IFlashMessagesManager
    {
        private ObservableCollection<IFlashMessageViewModel> _items;
        public ObservableCollection<IFlashMessageViewModel> Items
        {
            get { return _items; }
        }


        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                _isEmpty = value;
                NotifyOfPropertyChange(() => IsEmpty);
            }
        }


        private DispatcherTimer _dispatcherTimer;

        public FlashMessagesManager()
        {
            _dispatcherTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(300) };
            
            _items = new ObservableCollection<IFlashMessageViewModel>();
            IsEmpty = true;
        }


        public void DisplayFlashMessage(IFlashMessage flashMessage)
        {
            //_viewModelResolver.BuildUp(flashMessage.ViewModel);
            Items.Add(flashMessage.ViewModel);
        }


        public void ClearFlashMessage(IFlashMessageViewModel vm)
        {
            //Items.Remove(vm);
        }


        public void ClearFlashMessages()
        {
            
        }
    }
}
