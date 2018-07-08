using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Exceptions
{
    public class LunchHoursOutOfWorkedHoursRangeException : WrongTimeException
    {
        public LunchHoursOutOfWorkedHoursRangeException()
        {
        }


        public LunchHoursOutOfWorkedHoursRangeException(string message) : base(message)
        {
        }
    }
}
