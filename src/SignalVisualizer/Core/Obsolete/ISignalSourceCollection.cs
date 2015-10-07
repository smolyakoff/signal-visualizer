using System.Collections.Generic;

namespace SignalVisualizer.Core
{
    public interface ISignalSourceCollection : IEnumerable<ISignalSource>
    {
        int MaxLength { get; }
    }
}
