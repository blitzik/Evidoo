using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.FlashMessages
{
    public class FlashMessagesCollection
    {
        public bool IsEmpty
        {
            get { return _notifications.Count == 0; }
        }


        private List<FlashMessage> _notifications;

        public FlashMessagesCollection()
        {
            _notifications = new List<FlashMessage>();
        }


        public FlashMessagesCollection Add(string message, Type type)
        {
            _notifications.Add(new FlashMessage(message, type));

            return this;
        }


        public FlashMessage CutFirst()
        {
            FlashMessage n = _notifications.FirstOrDefault();
            if (n != null) {
                _notifications.Remove(n);
                return n;
            }

            return null;
        }


        public void Clear()
        {
            _notifications.Clear();
        }
    }
}