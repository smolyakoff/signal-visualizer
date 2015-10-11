using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using SignalVisualizer.Application.Utility;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Charting
{
    public class RawSignalChart : SliceChartBase
    {
        private readonly SignalCache _signal;

        public RawSignalChart(SignalCache signal)
        {
            _signal = signal;
            XAxis.IsPanEnabled = false;
            Series.Points.AddRange(signal.Select(x => x.ToDataPoint()));
        }

        public override List<DataPoint> Calculate(Slice slice)
        {
            return _signal.GetSample(slice).Select(x => x.ToDataPoint()).ToList();
        }

        public override void Draw(Slice slice, List<DataPoint> points)
        {
            if (slice.Length == 0)
            {
                return;
            }
            if (points.Count > 0)
            {
                XAxis.Minimum = points[0].X;
                XAxis.Maximum = points[points.Count - 1].X;
            }
            Model.ResetAllAxes();
            base.Draw(slice, points);
        }
    }
}
