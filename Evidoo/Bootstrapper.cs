using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using Common.Validation;
using Common.FlashMessages;
using Common.ViewModelResolver;
using prjt.ViewModels.Base;
using prjt.ViewModels;
using intf.Views;

namespace Evidoo
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;


        public Bootstrapper()
        {
            Initialize();
        }


        protected override void Configure()
        {
            var config = new TypeMappingConfiguration()
            {
                DefaultSubNamespaceForViewModels = "prjt.ViewModels",
                DefaultSubNamespaceForViews = "intf.Views"
            };
            ViewModelLocator.ConfigureTypeMappings(config);
            ViewLocator.ConfigureTypeMappings(config);


            //-----


            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IFlashMessagesManager, FlashMessagesManager>();
            _container.Singleton<IViewModelResolver<IViewModel>, ViewModelResolver<IViewModel>>();
            _container.PerRequest<IValidationObject, ValidationObject>();


            _container.Singleton<MainWindowViewModel>();


            _container.Instance(_container);
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindowViewModel vm = _container.GetInstance<MainWindowViewModel>();
            _container.BuildUp(vm);

            _container.GetInstance<IWindowManager>().ShowWindow(vm);
        }


        protected override void OnExit(object sender, EventArgs e)
        {

        }


        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {
                GetType().Assembly,
                typeof(MainWindowView).Assembly
            };
        }


        protected override IEnumerable<object> GetAllInstances(System.Type service)
        {
            return _container.GetAllInstances(service);
        }


        protected override object GetInstance(System.Type service, string key)
        {
            return _container.GetInstance(service, key);
        }


        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
