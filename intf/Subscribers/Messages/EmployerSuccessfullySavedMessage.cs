using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjt.Domain;

namespace intf.Subscribers.Messages
{
    public class EmployerSuccessfullySavedMessage : BaseEmployerMessage
    {
        public EmployerSuccessfullySavedMessage(Employer employer) : base(employer)
        {
        }
    }
}
