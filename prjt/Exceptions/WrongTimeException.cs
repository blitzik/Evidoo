using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Exceptions
{
    public class WrongTimeException : Exception
    {
        public WrongTimeException()
        {
        }


        public WrongTimeException(string message) : base(message)
        {
        }
    }
}
