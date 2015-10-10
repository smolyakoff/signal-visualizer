using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using SignalVisualizer.Application.Charting;
using SignalVisualizer.Application.Utility;

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
            _container.RegisterInstance(typeof(IEventAggregator), null, new ThrottlingEventAggregator(1000 / 15));
            _container.Singleton<IWindowManager, WindowManager>()
                .PerRequest<SliceChartController, SliceChartController>()
                .PerRequest<WorksheetViewModel, WorksheetViewModel>()
                .PerRequest<TabViewModel, TabViewModel>();
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
