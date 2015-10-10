using Caliburn.Micro;
using SignalVisualizer.Application.Charting;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public class SliderViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _eventAggregator;
        private int _position;
        private int _length;
        private int _sampleLength;

        public SliderViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                NotifyOfPropertyChange(nameof(Position));
                NotifyOfPropertyChange(nameof(EndPosition));
                _eventAggregator.PublishOnBackgroundThread(new SliceChangedMessage(Slice));
            }
        }

        public int SampleLength
        {
            get { return _sampleLength; }
            set
            {
                _sampleLength = value;
                NotifyOfPropertyChange(nameof(SampleLength));
                NotifyOfPropertyChange(nameof(MaximumPosition));
            }
        }

        public int EndPosition => Position + Length;

        public int MaximumPosition => SampleLength - Length;

        public double ViewportSize => (double)MaximumPosition/SampleLength * Length;

        public int Length
        {
            get { return _length; }
            set
            {
                if (Position + value >= SampleLength)
                {
                    Position = SampleLength - value;
                }
                _length = value;
                NotifyOfPropertyChange(nameof(Length));
                NotifyOfPropertyChange(nameof(MaximumPosition));
                NotifyOfPropertyChange(nameof(EndPosition));
                NotifyOfPropertyChange(nameof(ViewportSize));
                _eventAggregator.PublishOnBackgroundThread(new SliceChangedMessage(Slice));
            }
        }

        public void Reset(int length, int sampleLength)
        {
            _sampleLength = sampleLength;
            _length = length;
            _position = 0;
            NotifyOfPropertyChange(nameof(SampleLength));
            NotifyOfPropertyChange(nameof(Length));
            NotifyOfPropertyChange(nameof(Position));
            NotifyOfPropertyChange(nameof(MaximumPosition));
            NotifyOfPropertyChange(nameof(EndPosition));
            _eventAggregator.PublishOnBackgroundThread(new SliceChangedMessage(Slice));
        }

        public Slice Slice => new Slice(Position, Length);
    }
}
