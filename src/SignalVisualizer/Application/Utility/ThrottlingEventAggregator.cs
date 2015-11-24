using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using Action = System.Action;

namespace SignalVisualizer.Application.Utility
{
    public class ThrottlingEventAggregator : IEventAggregator, IDisposable
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly ConcurrentQueue<Tuple<object, Action<Action>>> _queue;

        private readonly Timer _timer;

        public ThrottlingEventAggregator(int period)
        {
            Contract.Requires<ArgumentOutOfRangeException>(period > 0);

            _eventAggregator = new EventAggregator();
            _queue = new ConcurrentQueue<Tuple<object, Action<Action>>>();
            _timer = new Timer(OnTimer, false, 0, period);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool HandlerExistsFor(Type messageType)
        {
            return _eventAggregator.HandlerExistsFor(messageType);
        }

        public void Subscribe(object subscriber)
        {
            _eventAggregator.Subscribe(subscriber);
        }

        public void Unsubscribe(object subscriber)
        {
            _eventAggregator.Unsubscribe(subscriber);
        }

        public void Publish(object message, Action<Action> marshal)
        {
            if (message == null)
            {
                return;
            }
            _queue.Enqueue(Tuple.Create(message, marshal));
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Dispose();
            }
        }

        private void OnTimer(object state)
        {
            foreach (var item in _queue.GroupBy(x => x.Item1.GetType()).Select(x => x.Last()))
            {
                _eventAggregator.Publish(item.Item1, item.Item2);
            }
        }
    }
}