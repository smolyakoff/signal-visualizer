using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using SignalVisualizer.Application.Utility;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Charting
{
    public class SpectrumChart : SliceChartBase
    {
        private readonly SignalCache _signalCache;

        public SpectrumChart(SignalCache signalCache)
        {
            _signalCache = signalCache;
            XAxis.Minimum = 0;
            XAxis.Maximum = (double)_signalCache.Header.Frequency / 2;
            XAxis.AbsoluteMaximum = XAxis.Maximum;
        }

        public override void Draw(Slice slice, List<DataPoint> points)
        {
            YAxis.Maximum = points.Select(x => x.Y).Max();
            Model.ResetAllAxes();
            base.Draw(slice, points);
        }

        public override List<DataPoint> Calculate(Slice slice)
        {
            var sample = _signalCache.GetSample(slice);
            var points = sample.AmplitudeSpectrum.Select(x => x.ToDataPoint()).ToList();
            return points;
        }
    }
}
