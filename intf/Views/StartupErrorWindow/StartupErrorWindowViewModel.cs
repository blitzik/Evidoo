using Common.Commands;
using intf.BaseViewModels;

namespace intf.Views
{
    public class StartupErrorWindowViewModel : BaseScreen
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                Set(ref _text, value);
            }
        }


        private DelegateCommand<object> _closeAppCommand;
        public DelegateCommand<object> CloseAppCommand
        {
            get
            {
                if (_closeAppCommand == null) {
                    _closeAppCommand = new DelegateCommand<object>(p => TryClose());
                }
                return _closeAppCommand;
            }
        }
        

        public StartupErrorWindowViewModel()
        {
        }
    }
}
