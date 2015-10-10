using System.Linq;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Charting
{
    internal static class ChartingExtensions
    {
        public static DataPoint ToDataPoint(this Point point)
        {
            return new DataPoint(point.X, point.Y);
        }

        public static PlotModel ApplyDefaultTheme(this PlotModel plot)
        {
            plot.Background = OxyColor.Parse("#252525");
            plot.PlotAreaBorderColor = OxyColor.Parse("#FFFFFF");
            foreach (var axis in plot.Axes)
            {
                axis.AxislineColor = OxyColor.FromRgb(255, 255, 255);
                axis.TextColor = OxyColor.FromRgb(255, 255, 255);
                axis.TicklineColor = OxyColor.FromRgb(255, 255, 255);
                if (plot.IsLineChart())
                {
                    axis.MajorGridlineColor = OxyColor.Parse("#B5B5B5").ChangeIntensity(0.5);
                    axis.MajorGridlineStyle = LineStyle.Solid;
                    axis.MinorGridlineColor = OxyColor.Parse("#B5B5B5").ChangeIntensity(0.5);
                    axis.MinorGridlineStyle = LineStyle.Solid;
                }
            }
            foreach (var annotation in plot.Annotations.OfType<LineAnnotation>())
            {
                annotation.TextColor = OxyColor.Parse("#FFFFFF");
                annotation.Color = OxyColor.FromRgb(255, 0, 0).ChangeIntensity(0.7);
                annotation.LineStyle = LineStyle.Solid;
            }
            return plot;
        }

        private static bool IsLineChart(this PlotModel plot)
        {
            return plot.Series.OfType<LineSeries>().Any();
        }
    }
}
