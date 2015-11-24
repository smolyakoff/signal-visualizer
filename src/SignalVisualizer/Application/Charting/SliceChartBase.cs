using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Charting
{
    public abstract class SliceChartBase : ISliceChart
    {
        protected SliceChartBase()
        {
            Model = new PlotModel();
            XAxis = new LinearAxis
            {
                Key = "X",
                AbsoluteMinimum = 0,
                IsZoomEnabled = true,
                IsPanEnabled = true,
                Position = AxisPosition.Bottom
            };
            YAxis = new LinearAxis
            {
                Key = "Y",
                IsZoomEnabled = false,
                IsPanEnabled = false
            };
            Series = new LineSeries();
            Model.Axes.Add(XAxis);
            Model.Axes.Add(YAxis);
            Model.Series.Add(Series);
            Model.ApplyDefaultTheme();
        }

        protected Axis XAxis { get; }

        protected Axis YAxis { get; }

        protected LineSeries Series { get; }

        public PlotModel Model { get; }

        public virtual void Draw(Slice slice, List<DataPoint> points)
        {
            if (Series.Points != points)
            {
                Series.Points.Clear();
                Series.Points.AddRange(points);
            }
            Model.InvalidatePlot(false);
        }

        public virtual List<DataPoint> Calculate(Slice slice)
        {
            return Series.Points;
        }
    }
}