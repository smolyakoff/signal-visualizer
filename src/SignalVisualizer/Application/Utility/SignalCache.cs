using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;
using MathNet.Numerics.Statistics;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Utility
{
    public class SignalCache : IEnumerable<Point>
    {
        private readonly MemoryCache _cache;
        private readonly Signal _signal;

        public SignalCache(Signal signal)
        {
            _signal = signal;
            _cache = new MemoryCache(GetType().FullName + Guid.NewGuid());
        }

        public SignalHeader Header => _signal.Header;

        public double Skewness => _signal.Skewnewss;

        public double Kurtosis => _signal.Kurtosis;

        public IEnumerator<Point> GetEnumerator()
        {
            return _signal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public SignalSample GetSample(Slice slice)
        {
            var key = CacheKeyForSlice(slice);
            var sample = _cache.Get(key) as SignalSample;
            if (sample != null)
            {
                return sample;
            }
            sample = _signal.GetSample(slice);
            _cache[key] = sample;
            return sample;
        }

        public Histogram CalculateAmplitudeHistogram(double lower, double upper, int buckets)
        {
            var key = CacheKeyForHistogram(lower, upper, buckets);
            var histogram = _cache.Get(key) as Histogram;
            if (histogram != null)
            {
                return histogram;
            }
            histogram = _signal.CalculateAmplitudeHistogram(lower, upper, buckets);
            _cache[key] = histogram;
            return histogram;
        }

        private static string CacheKeyForSlice(Slice slice)
        {
            return $"S{slice.Position}-{slice.Length}";
        }

        private static string CacheKeyForHistogram(double lower, double upper, int buckets)
        {
            return $"H{lower}-{upper}-{buckets}";
        }
    }
}