using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.EventArguments
{
    public class ListingArgs : EventArgs
    {
        public Listing Listing { private set; get; }


        public ListingArgs(Listing listing)
        {
            Listing = listing;
        }
    }
}
