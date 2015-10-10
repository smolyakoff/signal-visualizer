using SignalVisualizer.Application.Charting;

namespace SignalVisualizer.Application
{
    public interface ISignalViewModel
    {
        string Name { get; }

        IChart Chart { get; }

        SignalInfoViewModel Info { get; }
    }
}
