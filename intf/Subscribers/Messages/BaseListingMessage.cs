using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Subscribers.Messages
{
    public abstract class BaseListingMessage
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
        }


        public BaseListingMessage(Listing listing)
        {
            _listing = listing;
        }
    }
}
