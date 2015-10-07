using System.IO;
using System.Linq;
using Caliburn.Micro;
using Microsoft.Win32;
using SignalVisualizer.Application.Events;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public sealed class WorksheetViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IEventAggregator _eventAggregator;
        private SignalCollection _signals;

        private string _fileName;
        private SignalTabViewModel _signalTab;
        private SpectrumTabViewModel _spectrumTab;
        private HistogramTabViewModel _histogramTab;
        private int _position;
        private int _sampleSize;
        

        public WorksheetViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

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
        }

        public bool IsFileOpened => _signals != null;

        public bool IsFileClosed => !IsFileOpened;

        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                NotifyOfPropertyChange(nameof(Position));
                ChangeRange();
            }
        }

        public int MaxPosition => (_signals?.MaxLength - SampleSize) ?? 0;

        public double ViewportSize
        {
            get
            {
                const int scaleFactor = 4096 * 2048;
                if (MaxPosition == 0)
                {
                    return 0;
                }
                return (double)SampleSize * scaleFactor/ _signals.MaxLength ;
            }
        }

        public int SampleSize
        {
            get { return _sampleSize; }
            set
            {
                _sampleSize = value;
                if (Position + SampleSize >= _signals.MaxLength)
                {
                    Position = _signals.MaxLength - value;
                }
                NotifyOfPropertyChange(nameof(SampleSize));
                NotifyOfPropertyChange(nameof(MaxPosition));
                NotifyOfPropertyChange(nameof(ViewportSize));
                ChangeRange();
            }
        }

        public BindableCollection<int> SampleSizes { get; private set; }

        public string FileName
        {
            get { return _fileName; }
            private set
            {
                _fileName = value;
                NotifyOfPropertyChange(nameof(FileName));
                NotifyOfPropertyChange(nameof(IsFileClosed));
                NotifyOfPropertyChange(nameof(IsFileOpened));
            }
        }

        public void Open()
        {
            var dialog = new OpenFileDialog();
            var chosen = dialog.ShowDialog();
            if (chosen.HasValue && chosen.Value)
            {
                _signals = SignalCollection.FromFile(dialog.FileName);
                FileName = Path.GetFileName(dialog.FileName);
                LoadSignals();
            }
        }

        private void LoadSignals()
        {
            _signalTab = new SignalTabViewModel(_eventAggregator, _signals);
            _spectrumTab = new SpectrumTabViewModel(_eventAggregator, _signals);
            _histogramTab = new HistogramTabViewModel(_eventAggregator, _signals);
            Items.Clear();
            Items.AddRange(new IScreen[]
            {
                _signalTab,
                _spectrumTab,
                _histogramTab
            });
            ActivateItem(_signalTab);
            Position = 0;
            SampleSize = _signals.First().Description.SampleSize;
        }

        private void ChangeRange()
        {
            _eventAggregator.PublishOnBackgroundThread(new RangeChangedEvent(Position, Position + SampleSize));
        }
    }
}
