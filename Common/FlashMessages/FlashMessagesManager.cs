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
        private ObservableCollection<FlashMessageDecorator> _flashMessages;
        public ObservableCollection<FlashMessageDecorator> FlashMessages
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

        public FlashMessagesManager()
        {
            _dispatcherTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
            _dispatcherTimer.Tick += OnTick;

            _flashMessages = new ObservableCollection<FlashMessageDecorator>();
            IsEmpty = true;
        }


        private void OnTick(object sender, EventArgs e)
        {
            if (FlashMessages.Count < 1) {
                _dispatcherTimer.Stop();
                return;
            }

            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var disposableFlashMessages = FlashMessages.Where(x => x.DisposeAt <= now).ToList();
            foreach (var fmd in disposableFlashMessages) {
                if (fmd.CanBeDisposed == false) {
                    fmd.MarkFlashMessageAsDisposable();
                } else {
                    FlashMessages.Remove(fmd);
                }
            }
        }


        public void DisplayFlashMessage(string message, Type type)
        {
            FlashMessage fm = new FlashMessage(message, type);
            FlashMessages.Add(new FlashMessageDecorator(fm));

            if (!_dispatcherTimer.IsEnabled) {
                _dispatcherTimer.Start();
            }
        }


        public void ClearFlashMessages()
        {
        }
    }
}
