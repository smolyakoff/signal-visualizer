using System.Windows;

namespace SignalVisualizer.Application
{
    public partial class WorksheetView
    {
        public WorksheetView()
        {
            InitializeComponent();
        }

        private void Items_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void RangeSlider_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            MessageBox.Show("Shit");
        }
    }
}
