using System.Windows;
using System.Windows.Input;

namespace SignalVisualizer.Application
{
    public partial class WorksheetView
    {
        public WorksheetView()
        {
            InitializeComponent();
        }

        private void Items_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void RangeSlider_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            MessageBox.Show("Shit");
        }
    }
}