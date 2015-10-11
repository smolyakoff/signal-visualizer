using System.Linq;
using Caliburn.Micro;
using SignalVisualizer.Application.Events;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public sealed class SignalTabViewModel : Screen, IHandle<RangeChangedEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        public SignalTabViewModel(IEventAggregator eventAggregator, SignalCollection signals)
        {
            _eventAggregator = eventAggregator;
            DisplayName = "Сигнал";
            Signals = new BindableCollection<SignalViewModel>(signals.Select(x => new SignalViewModel(x)));
        }

        public BindableCollection<SignalViewModel> Signals { get; }

        protected override void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
        }

        public void Handle(RangeChangedEvent message)
        {
            foreach (var signal in Signals)
            {
                signal.Draw(message.LowerBound, message.UpperBound - message.LowerBound);
            }
        }
    }
}
