using OxyPlot;
using OxyPlot.Series;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public class SpectrumViewModel
    {
        private readonly Signal _signal;

        private readonly LineSeries _series;

        public SpectrumViewModel(Signal signal)
        {
            _signal = signal;
            Plot = new PlotModel();
            _series = new LineSeries();
            Plot.Series.Add(_series);
        }

        public SignalDescription Description => _signal.Description;

        public PlotModel Plot { get; }

        public void Draw(int position, int count)
        {
            var sample = _signal.GetSample(position, count).Spectrum;
            _series.Points.Clear();
            _series.Points.AddRange(sample);
            Plot.InvalidatePlot(false);
        }
    }
}