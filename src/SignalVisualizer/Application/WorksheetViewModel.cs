using System.IO;
using System.Linq;
using Caliburn.Micro;
using Microsoft.Win32;
using SignalVisualizer.Application.Charting;
using SignalVisualizer.Application.Utility;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public sealed class WorksheetViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly SliceChartController _controller;
        private SignalCollection _signals;
        private string _fileName;

        public WorksheetViewModel(IEventAggregator eventAggregator, SliceChartController controller)
        {
            _eventAggregator = eventAggregator;
            _controller = controller;
            DisplayName = "Signal Visualizer";
            SampleSizes = new BindableCollection<int>(new []
            {
                1024,
                2048,
                4096,
                8192,
                16384,
                32768
            });
            Slider = new SliderViewModel(eventAggregator);
        }

        public int SignalsCount => _signals?.Count ?? 0;

        public bool IsFileOpened => _signals != null;

        public bool IsFileClosed => !IsFileOpened;

        public BindableCollection<int> SampleSizes { get; private set; }

        public SliderViewModel Slider { get; private set; }

        public bool IsDropdownVisible => (ActiveItem as TabViewModel)?.DisplayName != "Гистограмма";

        public string FileName
        {
            get { return _fileName; }
            private set
            {
                _fileName = value;
                NotifyOfPropertyChange(nameof(FileName));
                NotifyOfPropertyChange(nameof(IsFileClosed));
                NotifyOfPropertyChange(nameof(IsFileOpened));
                NotifyOfPropertyChange(nameof(SignalsCount));
            }
        }

        public void Open()
        {
            var dialog = new OpenFileDialog {Filter = "Файл данных (*.bin;*.txt)|*.bin;*.txt"};
            var chosen = dialog.ShowDialog();
            if (!chosen.GetValueOrDefault())
            {
                return;
            }
            _signals = SignalSerializer.DeserializeCollectionFromFile(dialog.FileName);
            var signals = _signals.Select(x => new SignalCache(x)).ToList();
            FileName = Path.GetFileName(dialog.FileName);
            Items.Clear();
            var first = _signals.First();
            Slider.Reset(first.Header.SampleSize, first.Length);
            Items.AddRange(new []
            {
                TabViewModel.ForRawSignal(signals, _controller, _eventAggregator),
                TabViewModel.ForSpectrum(signals, _controller),
                TabViewModel.ForHistogram(signals),  
            });
            ActivateItem(Items[0]);
        }

        protected override void ChangeActiveItem(IScreen newItem, bool closePrevious)
        {
            base.ChangeActiveItem(newItem, closePrevious);
            NotifyOfPropertyChange(nameof(IsDropdownVisible));
        }
    }
}
