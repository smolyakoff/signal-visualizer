using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using SignalVisualizer.Application.Events;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public sealed class SpectrumTabViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;

        public SpectrumTabViewModel(IEventAggregator eventAggregator, IEnumerable<Signal> sources)
        {
            _eventAggregator = eventAggregator;
            DisplayName = "Спектр";
            Signals = new BindableCollection<SpectrumViewModel>(sources.Select(x => new SpectrumViewModel(x)));
        }

        public BindableCollection<SpectrumViewModel> Signals { get; }

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
