using Common.Utils.ResultObject;
using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Subscribers.Messages
{
    public class BackupImportedMessage
    {
        private ResultObject<object> _resultObject;
        public ResultObject<object> ResultObject
        {
            get { return _resultObject; }
        }


        public BackupImportedMessage(ResultObject<object> ro)
        {
            _resultObject = ro;
        }
    }
}
