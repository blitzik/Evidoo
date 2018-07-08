using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.ViewModels.Base
{
    public abstract class BaseScreen : Common.ViewModels.BaseScreen<IViewModel>, IViewModel
    {
        protected PageTitle _windowTitle = new PageTitle();
        public PageTitle WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                Set(ref _windowTitle, value);
            }
        }


        protected string _baseWindowTitle;
        public string BaseWindowTitle
        {
            get { return _baseWindowTitle; }
            set
            {
                _baseWindowTitle = value;
                WindowTitle.Text = value;
            }
        }
    }
}
