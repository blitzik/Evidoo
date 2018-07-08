using Common.ViewModelResolver;
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
    public enum EmployerViewModel
    {
        ITEM,
        DETAIL,
        DELETION
    }


    public class EmployerViewModelsFactory : IEmployerViewModelsFactory
    {
        private IViewModelResolver<IViewModel> _viewModelResolver;
        private EmployerFacade _employerFacade;


        public EmployerViewModelsFactory(
            IViewModelResolver<IViewModel> viewModelResolver,
            EmployerFacade employerFacade
        ) {
            _viewModelResolver = viewModelResolver;
            _employerFacade = employerFacade;
        }


        public IViewModel Create(Employer employer, EmployerViewModel viewModel)
        {
            IViewModel vm;
            switch (viewModel) {
                case EmployerViewModel.DETAIL:
                    vm = new EmployerDetailViewModel(employer, _employerFacade);
                    break;

                case EmployerViewModel.DELETION:
                    vm = new EmployerDeletionViewModel(employer, _employerFacade);
                    break;

                default:
                    vm = new EmployerItemViewModel(employer, _employerFacade, this);
                    break;
            }            
            _viewModelResolver.BuildUp(vm);

            return vm;
        }
    }
}
