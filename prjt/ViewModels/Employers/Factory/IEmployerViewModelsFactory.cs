using prjt.Domain;
using prjt.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.ViewModels
{
    public interface IEmployerViewModelsFactory
    {
        IViewModel Create(Employer employer, EmployerViewModel viewModel);
    }
}
