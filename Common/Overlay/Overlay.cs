using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Common.ViewModels;

namespace Common.Overlay
{
    public enum State
    {
        VISIBLE,
        HIDDEN
    }


    public class Overlay : PropertyChangedBase, IOverlay
    {
        private IOverlayToken _token;
        public IOverlayToken Token
        {
            get { return _token; }
        }


        public bool IsActive
        {
            get
            {
                if (_token == null) {
                    return false;
                }
                return true;
            }
        }


        public IOverlayToken DisplayOverlay<VM>(VM content) where VM : IViewModel
        {
            IOverlayToken token = new OverlayToken(content);
            token.OnOverlayHide += (s) => {
                _token = null;
                NotifyOfPropertyChange(() => Token);
                NotifyOfPropertyChange(() => IsActive);
            };

            _token = token;
            NotifyOfPropertyChange(() => Token);
            NotifyOfPropertyChange(() => IsActive);

            return token;
        }
    }
}
