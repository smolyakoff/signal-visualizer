using System;
using System.Linq;
using Caliburn.Micro;
using MathNet.Numerics.Statistics;
using SignalVisualizer.Application.Charting;
using SignalVisualizer.Application.Utility;

namespace SignalVisualizer.Application
{
    public class HistogramViewModel : PropertyChangedBase, ISignalViewModel
    {
        private readonly HistogramChart _chart;
        private readonly int _index;
        private readonly SignalCache _signal;
        private int _buckets;
        private double _lowerBound;
        private double _upperBound;

        public HistogramViewModel(SignalCache signal, int index)
        {
            MinValue = signal.Min(p => p.Y) - double.Epsilon;
            MaxValue = signal.Max(p => p.Y) + double.Epsilon;
            _lowerBound = MinValue;
            _upperBound = MaxValue;
            _buckets = 10;
            _signal = signal;
            _index = index;
            _chart = new HistogramChart(signal);
            Info = new SignalInfoViewModel(signal.Header);
            Columns = new BindableCollection<Tuple<int, Bucket>>();
            Update();
        }

        public double MinValue { get; }

        public double MaxValue { get; }

        public BindableCollection<Tuple<int, Bucket>> Columns { get; }

        public double Skewness => _signal.Skewness;

        public double Kurtosis => _signal.Kurtosis;

        public double LowerBound
        {
            get { return _lowerBound; }
            set
            {
                _lowerBound = value;
                NotifyOfPropertyChange(nameof(LowerBound));
                Update();
            }
        }

        public double UpperBound
        {
            get { return _upperBound; }
            set
            {
                _upperBound = value;
                NotifyOfPropertyChange(nameof(UpperBound));
                Update();
            }
        }

        public int Buckets
        {
            get { return _buckets; }
            set
            {
                _buckets = value;
                NotifyOfPropertyChange(nameof(Buckets));
                Update();
            }
        }

        public string Name => $"Гистограмма аплитуд (сигнал #{_index + 1})";

        public IChart Chart => _chart;

        public SignalInfoViewModel Info { get; }

        private void Update()
        {
            if (LowerBound >= UpperBound)
            {
                return;
            }
            Columns.Clear();
            var histogram = _signal.CalculateAmplitudeHistogram(LowerBound, UpperBound, Buckets);
            for (var i = 0; i < histogram.BucketCount; i++)
            {
                Columns.Add(Tuple.Create(i, histogram[i]));
            }
            _chart.Update(LowerBound, UpperBound, Buckets);
        }
    }
}