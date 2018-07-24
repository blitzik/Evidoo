﻿using Caliburn.Micro;
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


        private DispatcherTimer _dispatcherTimer;

        public FlashMessagesManager()
        {
            _dispatcherTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
            _dispatcherTimer.Tick += OnTick;

            _flashMessages = new ObservableCollection<FlashMessageDecorator>();
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
                    if (fmd.CanBeRemoved(now)) {
                        FlashMessages.Remove(fmd);
                    }
                }
            }
        }


        public void DisplayFlashMessage(string message, Type type, TimeSpan? lifespan = null)
        {
            FlashMessage fm = new FlashMessage(message, type, lifespan);
            FlashMessages.Add(new FlashMessageDecorator(fm));

            if (!_dispatcherTimer.IsEnabled) {
                _dispatcherTimer.Start();
            }
        }


        public void ClearFlashMessages()
        {
            _dispatcherTimer.Stop();

            foreach (var fmd in FlashMessages) {
                fmd.MarkFlashMessageAsDisposable();
            }

            _dispatcherTimer.Start();
        }
    }
}
