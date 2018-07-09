using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Subscribers.Messages
{
    public abstract class BaseEmployerMessage
    {
        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
        }


        public BaseEmployerMessage(Employer employer)
        {
            _employer = employer;
        }
    }
}
