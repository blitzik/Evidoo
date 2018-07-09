using intf.BaseViewModels;
using prjt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intf.Factories.Employers
{
    public interface IEmployerViewModelsFactory
    {
        IViewModel Create(Employer employer, EmployerViewModel viewModel);
    }
}
