using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Exceptions
{
    class OtherHoursException : WrongTimeException
    {
        public OtherHoursException()
        {
        }


        public OtherHoursException(string message) : base(message)
        {

        }
    }
}
