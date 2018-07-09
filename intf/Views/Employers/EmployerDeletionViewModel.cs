using Caliburn.Micro;
using Common.Commands;
using intf.BaseViewModels;
using intf.Subscribers.Messages;
using prjt.Domain;
using prjt.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Views
{
    public class EmployerDeletionViewModel : BaseScreen
    {
        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
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
            _employer = employer;
            _employerFacade = employerFacade;
        }


        public event Action<object, EventArgs> OnDeletedEmployer;
        private void DeleteEmployer()
        {
            _employerFacade.Delete(Employer);

            EventAggregator.PublishOnUIThread(new EmployerSuccessfullyDeletedMessage(Employer));
            OnDeletedEmployer?.Invoke(this, EventArgs.Empty);
        }


        public event Action<object, EventArgs> OnReturnBackClicked;
        private void ReturnBack()
        {
            OnReturnBackClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
