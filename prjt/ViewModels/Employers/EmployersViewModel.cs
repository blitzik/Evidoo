using Caliburn.Micro;
using Common.Commands;
using prjt.ViewModels.Base;
using prjt.Domain;
using prjt.Facades;
using Perst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.ViewModels
{
    public class EmployersViewModel : BaseScreen
    {
        private ObservableCollection<EmployerItemViewModel> _employers;
        public ObservableCollection<EmployerItemViewModel> Employers
        {
            get
            {
                return _employers;
            }
        }


        // -----


        private string _newEmployerName;
        public string NewEmployerName
        {
            get { return _newEmployerName; }
            set
            {
                Set(ref _newEmployerName, value);
                _saveNewEmployerCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand<object> _saveNewEmployerCommand;
        public DelegateCommand<object> SaveNewEmployerCommand
        {
            get
            {
                if (_saveNewEmployerCommand == null) {
                    _saveNewEmployerCommand = new DelegateCommand<object>(p => SaveNewEmployer(), p => !string.IsNullOrEmpty(NewEmployerName));
                }
                return _saveNewEmployerCommand;
            }
        }


        private IEmployerViewModelsFactory _employersVMFactory;
        private EmployerFacade _employerFacade;

        public EmployersViewModel(EmployerFacade employerFacade, IEmployerViewModelsFactory employersVMFactory)
        {
            _employersVMFactory = employersVMFactory;
            _employerFacade = employerFacade;

            BaseWindowTitle = "Správa zaměstnavatelů";

            _employers = new ObservableCollection<EmployerItemViewModel>();
        }


        private void SaveNewEmployer()
        {
            Employer e = _employerFacade.CreateEmployer(NewEmployerName.Trim());

            Employers.Insert(0, CreateEmployerItemViewModel(e));

            NewEmployerName = null;
        }


        private EmployerItemViewModel CreateEmployerItemViewModel(Employer employer)
        {
            EmployerItemViewModel vm = (EmployerItemViewModel)_employersVMFactory.Create(employer, EmployerViewModel.ITEM);
            vm.OnDeletedEmployer += OnEmployerDeletion;

            return vm;
        }


        private void OnEmployerDeletion(object sender, EventArgs args)
        {
            _employers.Remove((EmployerItemViewModel)sender);
        }


        // -----


        protected override void OnActivate()
        {
            base.OnActivate();

            _employers.Clear();
            List<Employer> foundEmployers = _employerFacade.FindAllEmployers();
            foreach (Employer e in foundEmployers) {
                _employers.Add(CreateEmployerItemViewModel(e));
            }
        }
    }
}
