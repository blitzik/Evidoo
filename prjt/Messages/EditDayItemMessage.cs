using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Messages
{
    public class EditDayItemMessage : IEventAggregatorMessage
    {
        private readonly DayItem _dayItem;
        public DayItem DayItem
        {
            get { return _dayItem; }
        }


        public EditDayItemMessage(DayItem dayItem)
        {
            _dayItem = dayItem;
        }
    }
}
