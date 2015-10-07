using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;

namespace SignalVisualizer.Application
{
    public class AppBootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container.Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IWindowManager, WindowManager>()
                .PerRequest<WorksheetViewModel, WorksheetViewModel>()
                .PerRequest<SignalTabViewModel, SignalTabViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return  _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<WorksheetViewModel>();
        }
    }
}
