using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SignalVisualizer.Core
{
    public class SignalCollection : IEnumerable<Signal>
    {
        private readonly List<Signal> _signals;

        public SignalCollection(IEnumerable<Signal> signals)
        {
            _signals = new List<Signal>(signals);
        }

        public int Count => _signals.Count;

        public bool IsEmpty => _signals.Count == 0;

        public int MaxLength => _signals.Max(x => x.Length);

        public IEnumerator<Signal> GetEnumerator()
        {
            return _signals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
