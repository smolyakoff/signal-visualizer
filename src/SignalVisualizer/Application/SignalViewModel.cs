using System.Collections;
using System.Collections.Generic;
using Caliburn.Micro;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public class SignalViewModel : PropertyChangedBase
    {
        private readonly Signal _signal;

        private readonly LineSeries _series;

        public SignalViewModel(Signal signal)
        {
            _signal = signal;
            Plot = new PlotModel();
            _series = new LineSeries();
            Points = new List<DataPoint>();
            Plot.Series.Add(_series);
            Plot.Background = OxyColor.Parse("#252525");
            Plot.Axes.Add(new LinearAxis() {AxislineColor = OxyColor.Parse("#F5F5F5")});
        }

        public SignalDescription Description => _signal.Description;

        public PlotModel Plot { get; }

        public List<DataPoint> Points { get; set; }

        public void Draw(int position, int count)
        {
            var sample = _signal.GetSample(position, count);
            Points.Clear();
            Points.AddRange(sample);
            NotifyOfPropertyChange(nameof(Points));
            //_series.Points.Clear();
            //_series.Points.AddRange(sample);
            //Plot.InvalidatePlot(false);
        }
    }
}
