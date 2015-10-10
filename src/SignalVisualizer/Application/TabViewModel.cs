using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Caliburn.Micro;
using SignalVisualizer.Application.Charting;
using SignalVisualizer.Application.Utility;

namespace SignalVisualizer.Application
{
    public class TabViewModel : Screen
    {
        private readonly SliceChartController _controller;
        private readonly IEventAggregator _eventAggregator;

        public TabViewModel(SliceChartController controller, IEventAggregator eventAggregator)
        {
            _controller = controller;
            _eventAggregator = eventAggregator;
            Items = new BindableCollection<ISignalViewModel>();
        }

        public BindableCollection<ISignalViewModel> Items { get; }

        public bool IsSliderVisible => Items.Select(x => x.Chart).OfType<ISliceChart>().Any();

        public SliderViewModel Slider => (Parent as WorksheetViewModel)?.Slider;

        public int Columns => Items.Count >= 4 ? 2 : 1;

        protected override void OnActivate()
        {
            if (_controller != null)
            {
                foreach (var chart in Items.Select(x => x.Chart).OfType<ISliceChart>())
                {
                    _controller.Register(chart);
                }
                _controller.ForceDraw();
            }
            if (_eventAggregator != null)
            {
                foreach (var handle in Items.OfType<IHandle>())
                {
                    _eventAggregator.Subscribe(handle);
                }
            }
            base.OnActivate();
        }

        protected override void OnDeactivate(bool close)
        {
            if (_controller != null)
            {
                foreach (var chart in Items.Select(x => x.Chart).OfType<ISliceChart>())
                {
                    _controller.Unregister(chart);
                }
            }
            if (_eventAggregator != null)
            {
                foreach (var handle in Items.OfType<IHandle>())
                {
                    _eventAggregator.Unsubscribe(handle);
                }
            }
            base.OnDeactivate(close);
        }

        public static TabViewModel ForRawSignal(IEnumerable<SignalCache> signals , SliceChartController controller, IEventAggregator eventAggregator)
        {
            Contract.Requires(signals != null);
            Contract.Requires(controller != null);

            var tab = new TabViewModel(controller, eventAggregator) {DisplayName = "Сигнал"};
            var vms = signals.Select((x, i) => new RawSignalViewModel(x, i)).ToList();
            tab.Items.AddRange(vms);
            return tab;
        }

        public static TabViewModel ForSpectrum(IEnumerable<SignalCache> signals, SliceChartController controller)
        {
            Contract.Requires(signals != null);
            Contract.Requires(controller != null);

            var tab = new TabViewModel(controller, null) {DisplayName = "Cпектр"};
            tab.Items.AddRange(signals.Select((x, i) => new SpectrumViewModel(x, i)));
            return tab;
        }

        public static TabViewModel ForHistogram(IEnumerable<SignalCache> signals)
        {
            Contract.Requires(signals != null);

            var tab = new TabViewModel(null, null) {DisplayName = "Гистограмма"};
            tab.Items.AddRange(signals.Select((x, i) => new HistogramViewModel(x, i)));
            return tab;
        }
    }
}
