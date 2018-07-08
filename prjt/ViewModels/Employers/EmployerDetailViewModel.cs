using Caliburn.Micro;
using Common.Commands;
using prjt.ViewModels.Base;
using prjt.Domain;
using prjt.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.ViewModels
{
    public class EmployerDetailViewModel : BaseScreen
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
                EmployerNameSaveCommand.RaiseCanExecuteChanged();
            }
        }


        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
        }


        private DelegateCommand<object> _employerDeletionCommand;
        public DelegateCommand<object> EmployerDeletionCommand
        {
            get
            {
                if (_employerDeletionCommand == null) {
                    _employerDeletionCommand = new DelegateCommand<object>(p => DisplayDeletion());
                }
                return _employerDeletionCommand;
            }
        }


        private DelegateCommand<object> _employerNameSaveCommand;
        public DelegateCommand<object> EmployerNameSaveCommand
        {
            get
            {
                if (_employerNameSaveCommand == null) {
                    _employerNameSaveCommand = new DelegateCommand<object>(
                        p => SaveEmployerChanges(),
                        p => !String.IsNullOrEmpty(_name) && _name != _employer.Name
                    );
                }
                return _employerNameSaveCommand;
            }
        }


        private EmployerFacade _employerFacade;

        public EmployerDetailViewModel(Employer employer, EmployerFacade employerFacade)
        {
            _employer = employer;
            _employerFacade = employerFacade;
        }


        private void SaveEmployerChanges()
        {
            Name = Name.Trim();

            _employer.Name = Name;
            _employerFacade.Update(_employer);

            EmployerNameSaveCommand.RaiseCanExecuteChanged();
        }


        public void ResetName()
        {
            Name = Employer.Name;
        }


        public delegate void DisplayEmployerDeletionHandler(object sender, EventArgs args);
        public event DisplayEmployerDeletionHandler OnDeletionClicked;
        private void DisplayDeletion()
        {
            DisplayEmployerDeletionHandler handler = OnDeletionClicked;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
