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
using intf.Views;
using System.Threading;
using prjt.Services;
using prjt.Services.IO;
using prjt.Services.Pdf;
using prjt.Services.Backup;
using prjt.Services.Entities;
using prjt.Facades;
using prjt.Domain;
using Perst;
using intf.BaseViewModels;
using intf.Factories.Employers;

namespace Evidoo
{
    public class Bootstrapper : BootstrapperBase
    {
        static Mutex mutex = new Mutex(false, "34515d3d-cdda-4d87-aa0c-eeaab04ba20a");


        private SimpleContainer _container;

        public Bootstrapper()
        {
            Initialize();
        }


        protected override void Configure()
        {
            var config = new TypeMappingConfiguration()
            {
                DefaultSubNamespaceForViews = "intf.Views",
                DefaultSubNamespaceForViewModels = "intf.Views"
            };
            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);


            // -----


            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // Services
            _container.Singleton<PerstStorageFactory>();
            _container.Singleton<StoragePool>();
            _container.Singleton<IViewModelResolver<IViewModel>, ViewModelResolver<IViewModel>>();
            _container.Singleton<IOpeningFilePathSelector, OpenFilePathSelector>();
            _container.Singleton<ISavingFilePathSelector, SaveFilePathSelector>();
            _container.Singleton<IListingSectionFactory, ListingSectionFactory>();
            _container.Singleton<IListingPdfDocumentFactory, DefaultListingPdfReportFactory>();
            _container.Singleton<IMultipleListingReportFactory, MultipleListingReportFactory>();
            _container.Singleton<IListingReportGenerator, ListingReportGenerator>();
            _container.Singleton<IBackupImport, BackupImport>();
            _container.Singleton<IFlashMessagesManager, FlashMessagesManager>();
            _container.PerRequest<IValidationObject, ValidationObject>();
            _container.Singleton<IEmployerViewModelsFactory, EmployerViewModelsFactory>();

            _container.Singleton<IEmployerFactory, EmployerFactory>();
            _container.Singleton<IListingFactory, ListingFactory>();

            // facades
            _container.Singleton<ListingFacade>();
            _container.Singleton<SettingFacade>();
            _container.Singleton<EmployerFacade>();

            // Windows
            _container.Singleton<MainWindowViewModel>();
            _container.PerRequest<StartupErrorWindowViewModel>();

            // ViewModels
            _container.Singleton<ListingsOverviewViewModel>(nameof(ListingsOverviewViewModel));
            _container.Singleton<EmployersViewModel>(nameof(EmployersViewModel));
            _container.Singleton<ListingViewModel>(nameof(ListingViewModel));
            _container.Singleton<ListingDeletionViewModel>(nameof(ListingDeletionViewModel));
            _container.Singleton<ListingDetailViewModel>(nameof(ListingDetailViewModel));
            _container.Singleton<ListingEditingViewModel>(nameof(ListingEditingViewModel));
            _container.Singleton<ListingItemViewModel>(nameof(ListingItemViewModel));
            _container.Singleton<ListingPdfGenerationViewModel>(nameof(ListingPdfGenerationViewModel));
            _container.Singleton<SettingsViewModel>(nameof(SettingsViewModel));
            _container.Singleton<EmptyListingsGenerationViewModel>(nameof(EmptyListingsGenerationViewModel));

            //_container.Singleton<TestWindowViewModel>(nameof(TestWindowViewModel));

            _container.Instance(_container);
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            /*if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false) || AppDomain.CurrentDomain.IsDefaultAppDomain() == true) {
                System.Windows.Application.Current.Shutdown();
            }*/

            ResultObject ro = new ResultObject(true);
            try {
                Storage db = _container.GetInstance<PerstStorageFactory>().OpenConnection(PerstStorageFactory.MAIN_DATABASE_NAME);
                StoragePool sp = _container.GetInstance<StoragePool>();
                sp.Add(PerstStorageFactory.MAIN_DATABASE_NAME, db);

                var vm = _container.GetInstance<MainWindowViewModel>();
                _container.BuildUp(vm);
                _container.GetInstance<IWindowManager>().ShowWindow(vm);

            }
            catch (StorageError ex) {
                ro = new ResultObject(false);
                ro.AddMessage("Nelze načíst Vaše data.");

            }
            catch (Exception ex) {
                ro = new ResultObject(false);
                ro.AddMessage("Při spouštění aplikace došlo k neočekávané chybě");
            }

            if (!ro.Success) {
                StartupErrorWindowViewModel errw = _container.GetInstance<StartupErrorWindowViewModel>();
                errw.Text = ro.GetLastMessage();
                _container.GetInstance<IWindowManager>().ShowDialog(errw);
            }
        }


        protected override void OnExit(object sender, EventArgs e)
        {
            _container.GetInstance<StoragePool>().CloseAll();
            //mutex.ReleaseMutex();
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
