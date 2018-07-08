using Caliburn.Micro;
using Common.Commands;
using prjt.Domain;
using prjt.Facades;
using prjt.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.ViewModels
{
    public class EmployerDeletionViewModel : BaseScreen
    {
        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
            set { Set(ref _employer, value); }
        }


        private DelegateCommand<object> _returnBackCommand;
        public DelegateCommand<object> ReturnBackCommand
        {
            get
            {
                if (_returnBackCommand == null) {
                    _returnBackCommand = new DelegateCommand<object>(p => ReturnBack());
                }
                return _returnBackCommand;
            }
        }


        private DelegateCommand<object> _deleteCommand;
        public DelegateCommand<object> DeleteCommand
        {
            get
            {
                if (_deleteCommand == null) {
                    _deleteCommand = new DelegateCommand<object>(p => DeleteEmployer());
                }
                return _deleteCommand;
            }
        }


        private EmployerFacade _employerFacade;


        public EmployerDeletionViewModel(Employer employer, EmployerFacade employerFacade)
        {
            Employer = employer;
            _employerFacade = employerFacade;
        }


        public delegate void EmployerDeletionHandler(object sender, EventArgs args);
        public event EmployerDeletionHandler OnDeletedEmployer;
        private void DeleteEmployer()
        {
            _employerFacade.Delete(Employer);

            EmployerDeletionHandler handler = OnDeletedEmployer;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }


        public delegate void ReturnbackHandler(object sender, EventArgs args);
        public event ReturnbackHandler OnReturnBackClicked;
        private void ReturnBack()
        {
            ReturnbackHandler handler = OnReturnBackClicked;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
