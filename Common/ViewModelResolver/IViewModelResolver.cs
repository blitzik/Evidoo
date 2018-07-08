using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModelResolver
{
    public interface IViewModelResolver<T>
    {
        T Resolve(string viewModel);
        T BuildUp(T instance);
    }
}
