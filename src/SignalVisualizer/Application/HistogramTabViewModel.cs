using Caliburn.Micro;
using OxyPlot.Series;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public class HistogramTabViewModel : Screen
    {
        public HistogramTabViewModel(IEventAggregator eventAggregator, SignalCollection signals)
        {
            DisplayName = "Гистограмма";
            var series = new BarSeries();
            series.Items.Add(new BarItem());
        }
    }
}
