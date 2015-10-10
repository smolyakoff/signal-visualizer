using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using SignalVisualizer.Application.Utility;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Charting
{
    public class RawSignalChart : SliceChartBase
    {
        public RawSignalChart(SignalCache signal)
        {
            XAxis.IsPanEnabled = false;
            Series.Points.AddRange(signal.Select(x => x.ToDataPoint()));
        }

        public override void Draw(Slice slice, List<DataPoint> points)
        {
            if (slice.Length == 0)
            {
                return;
            }
            if (points.Count > 0)
            {
                XAxis.Minimum = points[slice.Position].X;
                XAxis.Maximum = points[slice.Position + slice.Length - 1].X;
            }
            Model.ResetAllAxes();
            base.Draw(slice, points);
        }
    }
}
