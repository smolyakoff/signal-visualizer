using OxyPlot;

namespace SignalVisualizer.Application.Charting
{
    public interface IChart
    {
        PlotModel Model { get; }
    }
}