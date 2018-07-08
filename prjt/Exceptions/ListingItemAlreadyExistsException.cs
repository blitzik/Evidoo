using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Exceptions
{
    public class ListingItemAlreadyExistsException : Exception
    {
        public ListingItemAlreadyExistsException()
        {
        }


        public ListingItemAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
