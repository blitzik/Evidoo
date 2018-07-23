namespace Common.FlashMessages
{
    public class FlashMessage
    {
        private string _message;
        public string Message
        {
            get { return _message; }
        }


        private Type _type;
        public Type Type
        {
            get { return _type; }
        }


        public FlashMessage(string message, Type type)
        {
            _message = message;
            _type = type;
        }
    }
}