using Caliburn.Micro;
using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Utils
{
    public class ListingCheckBoxWrapper : PropertyChangedBase
    {
        private Listing _listing;
        public Listing Listing
        {
            get { return _listing; }
            private set { Set(ref _listing, value); }
        }


        public int Month
        {
            get { return _listing.Month; }
        }


        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { Set(ref _isChecked , value); }
        }


        public ListingCheckBoxWrapper(Listing listing, bool isChecked = false)
        {
            Listing = listing;
            IsChecked = isChecked;
            NotifyOfPropertyChange(() => Month);
        }
    }
}
