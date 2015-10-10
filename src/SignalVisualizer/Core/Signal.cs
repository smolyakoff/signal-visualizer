using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace SignalVisualizer.Core
{
    public class Signal : IEnumerable<Point>
    {
        private readonly double[] _values;

        private readonly Lazy<List<Point>> _points;

        private readonly Lazy<DescriptiveStatistics> _stats; 

        private readonly double _tickTime;

        public Signal(SignalHeader header, double[] values)
        {
            Header = header;
            _values = values;
            _tickTime = (double)header.TotalTime/header.Ticks;
            _points = new Lazy<List<Point>>(() => _values.Select(ToPoint).ToList());
            _stats = new Lazy<DescriptiveStatistics>(() => new DescriptiveStatistics(_values));
        }

        public SignalHeader Header { get; }

        public int Length => _values.Length;

        public SignalSample this[Slice slice] => GetSample(slice);

        public double Kurtosis => _stats.Value.Kurtosis;

        public double Skewnewss => _stats.Value.Skewness;

        public SignalSample GetSample(Slice slice)
        {
            Contract.Requires<ArgumentOutOfRangeException>(slice.Position + slice.Length <= Length);

            return new SignalSample(this, slice);
        }

        public Histogram CalculateAmplitudeHistogram(double lower, double upper, int buckets)
        {
            Contract.Requires<ArgumentException>(upper > lower, "Upper value should be greater than lower value.");

            return new Histogram(_values.Where(x => x > lower && x < upper), buckets, lower, upper);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _points.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Point ToPoint(double value, int position)
        {
            return new Point(_tickTime * position, value);
        }
    }
}
