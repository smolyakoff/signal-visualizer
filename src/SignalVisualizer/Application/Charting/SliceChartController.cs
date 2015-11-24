using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using Caliburn.Micro;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Charting
{
    public class SliceChartController : IHandle<SliceChangedMessage>
    {
        private readonly ConcurrentDictionary<ISliceChart, ISliceChart> _charts =
            new ConcurrentDictionary<ISliceChart, ISliceChart>();

        private Slice _lastDrawnSlice;
        private Slice _lastSlice;

        public SliceChartController(IEventAggregator eventAggregator, Slice slice)
        {
            eventAggregator.Subscribe(this);
            _lastSlice = slice;
        }

        public void Handle(SliceChangedMessage message)
        {
            _lastSlice = message.Slice;
            Draw(false);
        }

        public void Register(ISliceChart chart)
        {
            Contract.Requires<ArgumentNullException>(chart != null);
            _charts[chart] = chart;
        }

        public void Unregister(ISliceChart chart)
        {
            Contract.Requires<ArgumentNullException>(chart != null);
            ISliceChart rubbish;
            _charts.TryRemove(chart, out rubbish);
        }

        public void ForceDraw()
        {
            Draw(true);
        }

        private void Draw(bool force)
        {
            if (_charts.IsEmpty)
            {
                return;
            }
            // Copy value to ensure it will be the same for all charts
            var slice = _lastSlice;
            if (slice == _lastDrawnSlice && !force)
            {
                return;
            }
            var results = _charts.Values.AsParallel().Select(x => new
            {
                Chart = x,
                Points = x.Calculate(slice),
                Slice = slice
            });
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var result in results)
                {
                    result.Chart.Draw(result.Slice, result.Points);
                }
            });
            _lastDrawnSlice = slice;
        }
    }
}