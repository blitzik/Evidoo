using Common.Commands;
using prjt.ViewModels.Base;
using prjt.Domain;
using prjt.Facades;
using System;

namespace prjt.ViewModels
{
    public class EmployerItemViewModel : BaseConductorOneActive
    {
        private Employer _employer;
        public Employer Employer
        {
            get { return _employer; }
        }


        private EmployerDetailViewModel _employerDetailViewModel;


        private IEmployerViewModelsFactory _employerVMFactory;
        private EmployerFacade _employerFacade;

        public event Action<object, EventArgs> OnDeletedEmployer;

        public EmployerItemViewModel(Employer employer, EmployerFacade employerFacade, IEmployerViewModelsFactory employerVMFactory)
        {
            _employer = employer;

            _employerVMFactory = employerVMFactory;
            _employerFacade = employerFacade;
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            _employerDetailViewModel = (EmployerDetailViewModel)_employerVMFactory.Create(_employer, EmployerViewModel.DETAIL);
            //ActivateItem(_employerDetailViewModel);
        }
    }
}
