using Caliburn.Micro;
using Common.ViewModels;
using System;

namespace Common.ViewModelResolver
{
    public class ViewModelResolver<T> : IViewModelResolver<T>
    {
        private readonly SimpleContainer _container;


        public ViewModelResolver(SimpleContainer container)
        {
            _container = container;
        }


        public T Resolve(string viewModel)
        {
            T vm = (T)_container.GetInstance(Type.GetType(viewModel), viewModel);
            _container.BuildUp(vm);

            return vm;
        }


        public T BuildUp(T instance)
        {
            _container.BuildUp(instance);

            return instance;
        }
    }
}
