namespace Common.FlashMessages
{
    public abstract class FlashMessage : IFlashMessage
    {
        protected string _message;
        public string Message
        {
            get { return _message; }
        }


        protected Type _type;
        public Type Type
        {
            get { return _type; }
        }


        protected IFlashMessageViewModel _viewModel;
        public IFlashMessageViewModel ViewModel
        {
            get { return _viewModel; }
        }


        public FlashMessage(string message)
        {
            _message = message;
        }
    }
}