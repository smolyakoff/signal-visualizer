using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Caliburn.Micro;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public class SignalViewModel : Screen
    {
        private readonly Signal _signal;

        private readonly LineSeries _series;
        private LinearAxis _x;
        private LinearAxis _y;

        public SignalViewModel(Signal signal)
        {
            _signal = signal;
            Plot = new PlotModel();
            _series = new LineSeries();
            Points = new List<DataPoint>();
            Plot.Series.Add(_series);
            Plot.Background = OxyColor.Parse("#252525");
            _x = new LinearAxis();
            _x.Position = AxisPosition.Bottom;
            _x.Key = "X";
            _y = new LinearAxis();
            _y.Position = AxisPosition.Left;
            _y.Key = "Y";

            Plot.Axes.Add(_x);
            Plot.Axes.Add(_y);
            _series.Points.AddRange(signal);
            Description = new SignalDescriptionViewModel(signal.Header);
        }

        public SignalDescriptionViewModel Description { get; }

        public PlotModel Plot { get; }

        public List<DataPoint> Points { get; set; }

        public void Draw(int position, int count)
        {
            //var sample = _signal.GetSample(position, count);
            //Points.Clear();
            //Points.AddRange(sample);
            //NotifyOfPropertyChange(nameof(Points));
            //_series.Points.Clear();
            //_series.Points.AddRange(sample);
            
            if (_series.Points.Count > 0)
            {
                _x.Minimum = position;
                _x.Maximum = position + count;
                Plot.ResetAllAxes();
                Plot.InvalidatePlot(false);
            }
            
        }
    }
}
