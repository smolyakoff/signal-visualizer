using System;
using System.Diagnostics.Contracts;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Caliburn.Micro;
using Action = System.Action;

namespace SignalVisualizer.Application.Utility
{
    public class ThrottlingEventAggregator : IEventAggregator, IDisposable
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly IDisposable _eventsSubscription;

        private readonly Subject<Tuple<object, Action<Action>>> _eventsSubject;

        public ThrottlingEventAggregator(int period)
        {
            Contract.Requires<ArgumentOutOfRangeException>(period > 0);

            _eventsSubject = new Subject<Tuple<object, Action<Action>>>();
            _eventsSubscription = _eventsSubject
                .GroupByUntil(x => x.Item1.GetType(), group => group.Throttle(TimeSpan.FromTicks(period)))
                .SelectMany(x => x.TakeLast(1))
                .Subscribe(Dispatch);

            _eventAggregator = new EventAggregator();
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
            _eventsSubject.OnNext(Tuple.Create(message, marshal));
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eventsSubscription?.Dispose();
                _eventsSubject?.Dispose();
            }
        }

        private void Dispatch(Tuple<object, Action<Action>> @event)
        {
            _eventAggregator.Publish(@event.Item1, @event.Item2);
        }
    }
}