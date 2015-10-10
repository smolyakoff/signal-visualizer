using Caliburn.Micro;
using SignalVisualizer.Application.Charting;
using SignalVisualizer.Application.Utility;

namespace SignalVisualizer.Application
{
    public class SpectrumViewModel : ISignalViewModel
    {
        private readonly int _index;

        public SpectrumViewModel(SignalCache signalCache, int index)
        {
            _index = index;
            Chart = new SpectrumChart(signalCache);
            Info = new SignalInfoViewModel(signalCache.Header);
        }

        public string Name => $"Спектр сигнала #{_index + 1}";

        public IChart Chart { get; }

        public SignalInfoViewModel Info { get; }
    }
}