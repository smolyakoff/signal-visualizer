using System.Collections.Generic;
using OxyPlot;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Charting
{
    public interface ISliceChart : IChart
    {
        void Draw(Slice slice, List<DataPoint> points);

        List<DataPoint> Calculate(Slice slice);
    }
}