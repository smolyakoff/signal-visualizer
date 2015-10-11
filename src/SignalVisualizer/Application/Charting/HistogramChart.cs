using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using MathNet.Numerics.Statistics;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SignalVisualizer.Application.Utility;

namespace SignalVisualizer.Application.Charting
{
    public class HistogramChart : PropertyChangedBase, IChart
    {
        private readonly SignalCache _signal;
        private ColumnSeries _series;
        private CategoryAxis _xAxis;

        public HistogramChart(SignalCache signal)
        {
            _signal = signal;

        }

        public PlotModel Model { get; set; }

        public void Update(double lower, double upper, int nbuckets)
        {
            Contract.Requires(upper >= lower);
            Contract.Requires(nbuckets > 0);

            RefreshModel();
            var histogram = _signal.CalculateAmplitudeHistogram(lower, upper, nbuckets);
            _series.Items.Clear();
            foreach (var bucket in Enumerate(histogram))
            {
                _series.Items.Add(new ColumnItem(bucket.Count));
            }
            _xAxis.LabelFormatter = l => l.ToString();
            _xAxis.Maximum = _series.Items.Count - 0.5;
            NotifyOfPropertyChange(nameof(Model));
        }

        private void RefreshModel()
        {
            Model = new PlotModel();
            _xAxis = new CategoryAxis
            {
                Key = "X",
                IsPanEnabled = true,
                IsZoomEnabled = false,
                Position = AxisPosition.Bottom,
                GapWidth = 0.1
            };
            Model.Axes.Add(_xAxis);
            Model.Axes.Add(new LinearAxis
            {
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Key = "Y",
                Position = AxisPosition.Left
            });
            _series = new ColumnSeries();
            Model.Series.Add(_series);
            Model.ApplyDefaultTheme();
        }

        private static IEnumerable<Bucket> Enumerate(Histogram histogram)
        {
            for (var i = 0; i < histogram.BucketCount; i++)
            {
                var bucket = histogram[i];
                yield return bucket;
            }
        }
    }
}
