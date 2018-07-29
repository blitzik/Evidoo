﻿using Caliburn.Micro;
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
using intf.Subscribers;
using Common.Utils.ResultObject;

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
            _container.Instance(_container);

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // Services
            _container.Singleton<PerstStorageFactory>();
            _container.Singleton<StoragePool>();
            _container.Singleton<IViewModelResolver, ViewModelResolver>();
            _container.Singleton<IIODialogService, FilePathDialogService>();
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
            _container.Singleton<ListingsOverviewViewModel>(typeof(ListingsOverviewViewModel).FullName);
            _container.Singleton<EmployersViewModel>(typeof(EmployersViewModel).FullName);
            _container.Singleton<ListingViewModel>(typeof(ListingViewModel).FullName);
            _container.Singleton<ListingDeletionViewModel>(typeof(ListingDeletionViewModel).FullName);
            _container.Singleton<ListingDetailViewModel>(typeof(ListingDetailViewModel).FullName);
            _container.Singleton<ListingEditingViewModel>(typeof(ListingEditingViewModel).FullName);
            _container.Singleton<ListingItemViewModel>(typeof(ListingItemViewModel).FullName);
            _container.Singleton<ListingPdfGenerationViewModel>(typeof(ListingPdfGenerationViewModel).FullName);
            _container.Singleton<SettingsViewModel>(typeof(SettingsViewModel).FullName);
            _container.Singleton<EmptyListingsGenerationViewModel>(typeof(EmptyListingsGenerationViewModel).FullName);
            _container.PerRequest<WorkedTimeSettingViewModel>(typeof(WorkedTimeSettingViewModel).FullName);

            // Subscribers
            _container.Singleton<ListingSubscriber>().GetInstance<ListingSubscriber>();
            _container.Singleton<EmployerSubscriber>().GetInstance<EmployerSubscriber>();
            _container.Singleton<SettingsSubscriber>().GetInstance<SettingsSubscriber>();
        }


        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            /*if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false) || AppDomain.CurrentDomain.IsDefaultAppDomain() == true) {
                System.Windows.Application.Current.Shutdown();
            }*/

            ResultObject<object> ro = new ResultObject<object>(true);
            try {
                Storage db = _container.GetInstance<PerstStorageFactory>().OpenConnection(PerstStorageFactory.MAIN_DATABASE_NAME);
                StoragePool sp = _container.GetInstance<StoragePool>();
                sp.Add(PerstStorageFactory.MAIN_DATABASE_NAME, db);

                var vm = _container.GetInstance<MainWindowViewModel>();
                _container.BuildUp(vm);
                _container.GetInstance<IWindowManager>().ShowWindow(vm);

            }
            catch (StorageError ex) {
                ro = new ResultObject<object>(false);
                ro.AddMessage("Nelze načíst Vaše data.");

            }
            catch (Exception ex) {
                ro = new ResultObject<object>(false);
                ro.AddMessage("Při spouštění aplikace došlo k neočekávané chybě");
            }

            if (!ro.Success) {
                StartupErrorWindowViewModel errw = _container.GetInstance<StartupErrorWindowViewModel>();
                errw.Text = ro.GetLastMessage().Text;
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
