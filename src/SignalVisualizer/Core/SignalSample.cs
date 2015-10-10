using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.Statistics;

namespace SignalVisualizer.Core
{
    public class SignalSample : IEnumerable<Point>
    {
        private readonly Signal _signal;
        private readonly Slice _slice;
        private readonly Lazy<double> _min;
        private readonly Lazy<double> _max;
        private readonly Lazy<double> _rms;
        private readonly Lazy<double> _peakFactor;
        private readonly Lazy<SortedList<double, object>> _sortedValues; 

        private readonly Lazy<Point[]> _spectrum;

        public SignalSample(Signal signal, Slice slice)
        {
            Contract.Requires<ArgumentNullException>(signal != null);
            Contract.Requires<ArgumentOutOfRangeException>(slice.Position + slice.Length <= signal.Length);

            _signal = signal;
            _slice = slice;
            _spectrum = new Lazy<Point[]>(CalculateAmplitudeSpectrum);
            _min = new Lazy<double>(CalculateMinimum);
            _max = new Lazy<double>(CalculateMaximum);
            _rms = new Lazy<double>(CalculateRootMeanSquare);
            _peakFactor = new Lazy<double>(CalculatePeakFactor);
            _sortedValues = new Lazy<SortedList<double, object>>(GetSortedList);
        }

        public int Length => _slice.Length;

        public Point[] AmplitudeSpectrum => _spectrum.Value;

        public double Minimum => _min.Value;

        public double Maximum => _max.Value;

        public double RootMeanSquare => _rms.Value;

        public double PeakFactor => _peakFactor.Value;

        private Point[] CalculateAmplitudeSpectrum()
        {
            var values = _signal.Skip(_slice.Position)
                .Take(_slice.Length)
                .Select(p => new Complex(p.Y, 0))
                .ToArray();
            Fourier.Forward(values, FourierOptions.Matlab);
            var spectrum = values.Select(ToSpectrumPoint)
                .Take(values.Length / 2)
                .ToArray();
            return spectrum;
        }

        private SortedList<double, object> GetSortedList()
        {
            var dictionary = new Dictionary<double, object>();
            foreach (var value in this)
            {
                dictionary[value.Y] = null;
            }
            return new SortedList<double, object>(dictionary);
        } 

        private double CalculateMinimum()
        {
            return _sortedValues.Value.Keys.First();
        }

        private double CalculateMaximum()
        {
            return _sortedValues.Value.Keys.Last();
        }

        private double CalculateRootMeanSquare()
        {
            return this.Select(p => p.Y).RootMeanSquare();
        }

        private double CalculatePeakFactor()
        {
            var max = Math.Max(Math.Abs(Minimum), Maximum);
            return max/RootMeanSquare;
        }

        private Point ToSpectrumPoint(Complex c, int i)
        {
            var x = i * (double)_signal.Header.Frequency * 2 / Length;
            var y = c.Magnitude * 2 / Length;
            return new Point(x, y);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _signal.Skip(_slice.Position).Take(_slice.Length).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
