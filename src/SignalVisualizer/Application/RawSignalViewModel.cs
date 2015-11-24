using Caliburn.Micro;
using SignalVisualizer.Application.Charting;
using SignalVisualizer.Application.Utility;

namespace SignalVisualizer.Application
{
    public class RawSignalViewModel : PropertyChangedBase, ISignalViewModel, IHandle<SliceChangedMessage>
    {
        private readonly int _order;
        private readonly SignalCache _signalCache;


        public RawSignalViewModel(SignalCache signalCache, int order)
        {
            _signalCache = signalCache;
            _order = order;
            Chart = new RawSignalChart(signalCache);
            Info = new SignalInfoViewModel(signalCache.Header);
            Properties = new BindableCollection<PropertyViewModel>();
        }

        public BindableCollection<PropertyViewModel> Properties { get; }

        public void Handle(SliceChangedMessage message)
        {
            var sample = _signalCache.GetSample(message.Slice);
            var properties = new[]
            {
                new PropertyViewModel {Label = "Минимум", Value = sample.Minimum},
                new PropertyViewModel {Label = "Максимум", Value = sample.Maximum},
                new PropertyViewModel {Label = "СКЗ", Value = sample.RootMeanSquare},
                new PropertyViewModel {Label = "Пик-фактор", Value = sample.PeakFactor}
            };
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Properties.Clear();
                Properties.AddRange(properties);
            });
        }

        public string Name => $"Чистый сигнал {_order + 1}";

        public IChart Chart { get; }

        public SignalInfoViewModel Info { get; }
    }
}