using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Domain
{
    public class ResultObject
    {
        private readonly bool _success;
        public bool Success
        {
            get { return _success; }
        }


        private object _result;
        public object Result
        {
            get { return _result; }
        }


        private List<string> _messages;


        public ResultObject(bool success, object result = null)
        {
            _messages = new List<string>();
            _success = success;
            _result = result;
        }


        public void AddMessage(string message)
        {
            _messages.Add(message);
        }


        public List<string> GetMessages()
        {
            return new List<string>(_messages);
        }


        public string GetLastMessage()
        {
            return _messages.Last();
        }
    }
}
