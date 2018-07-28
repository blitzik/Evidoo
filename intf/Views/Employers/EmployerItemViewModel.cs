using Common.Commands;
using prjt.Domain;
using prjt.Facades;
using System;
using intf.BaseViewModels;
using intf.Factories.Employers;

namespace intf.Views
{
    public class EmployerItemViewModel : BaseConductorOneActive
    {
        private bool _canBeRemoved;
        public bool CanBeRemoved
        {
            get { return _canBeRemoved; }
            set { Set(ref _canBeRemoved, value); }
        }


        private DelegateCommand<EmployerItemViewModel> _removeEmployerCommand;
        public DelegateCommand<EmployerItemViewModel> RemoveEmployerCommand
        {
            get
            {
                if (_removeEmployerCommand == null) {
                    _removeEmployerCommand = new DelegateCommand<EmployerItemViewModel>(p => RemoveEmployer(p));
                }
                return _removeEmployerCommand;
            }
        }



        private EmployerDetailViewModel _employerDetailViewModel;
        private EmployerDeletionViewModel _employerDeletionViewModel;


        private IEmployerViewModelsFactory _employerVMFactory;
        private EmployerFacade _employerFacade;

        public event Action<object, EventArgs> OnDeletedEmployer;

        public EmployerItemViewModel(Employer employer, EmployerFacade employerFacade, IEmployerViewModelsFactory employerVMFactory)
        {
            _employerVMFactory = employerVMFactory;
            _employerFacade = employerFacade;

            CanBeRemoved = false;

            _employerDetailViewModel = (EmployerDetailViewModel)_employerVMFactory.Create(employer, EmployerViewModel.DETAIL);
            _employerDetailViewModel.OnDeletionClicked += (object sender, EventArgs e) =>
            {
                if (_employerDeletionViewModel == null) {
                    _employerDeletionViewModel = (EmployerDeletionViewModel)_employerVMFactory.Create(employer, EmployerViewModel.DELETION);
                    _employerDeletionViewModel.OnReturnBackClicked += (object s, EventArgs ea) =>
                    {
                        ActivateItem(_employerDetailViewModel);
                    };
                    _employerDeletionViewModel.OnDeletedEmployer += (object s, EventArgs ea) =>
                    {
                        CanBeRemoved = true;
                        ActivateItem(_employerDetailViewModel);
                    };
                }
                ActivateItem(_employerDeletionViewModel);
            };
            ActivateItem(_employerDetailViewModel);
        }


        private void RemoveEmployer(EmployerItemViewModel p)
        {
            OnDeletedEmployer?.Invoke(this, EventArgs.Empty);
        }

    }
}
