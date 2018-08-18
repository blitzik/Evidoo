using Caliburn.Micro;
using Common.ExtensionMethods;
using Common.Commands;
using prjt.Domain;
using prjt.Facades;
using Perst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using intf.BaseViewModels;
using intf.Factories.Employers;
using intf.Subscribers.Messages;

namespace intf.Views
{
    public class EmployersViewModel : BaseScreen
    {
        private ObservableCollection<EmployerItemViewModel> _employers;
        private ReadOnlyObservableCollection<EmployerItemViewModel> _readOnlyEmployersCollection;
        public ReadOnlyObservableCollection<EmployerItemViewModel> Employers
        {
            get
            {
                if (_readOnlyEmployersCollection == null) {
                    _readOnlyEmployersCollection = new ReadOnlyObservableCollection<EmployerItemViewModel>(_employers);
                }
                return _readOnlyEmployersCollection;
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


        private void SaveNewEmployer()
        {
            Employer e = _employerFacade.CreateEmployer(NewEmployerName.Trim());

            _employers.Insert(0, CreateEmployerItemViewModel(e));

            NewEmployerName = null;

            EventAggregator.PublishOnUIThread(new EmployerSuccessfullySavedMessage(e));
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


        protected override void OnActivate()
        {
            base.OnActivate();

            _employers.Refill(_employerFacade.FindAllEmployers(), CreateEmployerItemViewModel);
        }


        private EmployerItemViewModel CreateEmployerItemViewModel(Employer employer)
        {
            EmployerItemViewModel vm = (EmployerItemViewModel)_employersVMFactory.Create(employer, EmployerViewModel.ITEM);
            vm.OnDeletedEmployer += (s, e) => {
                _employers.Remove((EmployerItemViewModel)s);
            };

            return vm;
        }
    }
}
