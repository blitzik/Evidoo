using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Subscribers.Messages
{
    public class EmployerSuccessfullyDeletedMessage : BaseEmployerMessage
    {
        public EmployerSuccessfullyDeletedMessage(Employer employer) : base(employer)
        {
        }
    }
}
