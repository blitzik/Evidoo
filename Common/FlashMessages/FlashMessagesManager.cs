using Caliburn.Micro;
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
        private ObservableCollection<FlashMessage> _flashMessages;
        public ObservableCollection<FlashMessage> FlashMessages
        {
            get { return _flashMessages; }
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
        private FlashMessagesCollection _sourceCollection;

        public FlashMessagesManager()
        {
            _sourceCollection = new FlashMessagesCollection();
            _dispatcherTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(300) };
            _dispatcherTimer.Tick += (o, e) => {
                if (_sourceCollection.IsEmpty) {
                    _dispatcherTimer.Stop();
                    return;
                }
                _flashMessages.Add(_sourceCollection.CutFirst());
            };
            _flashMessages = new ObservableCollection<FlashMessage>();
            IsEmpty = true;
        }


        public void DisplayFlashMessages()
        {
            _dispatcherTimer.Start();
        }


        public IFlashMessagesManager AddFlashMessage(string message, Type type)
        {
            if (_dispatcherTimer.IsEnabled) {
                _dispatcherTimer.Stop();
                ClearFlashMessages();
            }
            _sourceCollection.Add(message, type);
            IsEmpty = false;

            return this;
        }


        public void DisplayFlashMessage(string message, Type type)
        {
            ClearFlashMessages();
            IsEmpty = false;
            _flashMessages.Add(new FlashMessage(message, type));
        }


        public void ClearFlashMessages()
        {
            _sourceCollection.Clear();
            _flashMessages.Clear();
            IsEmpty = true;
        }
    }
}
