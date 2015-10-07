using System.Threading.Tasks;

namespace SignalVisualizer.Core
{
    public interface ISignalSource
    {
        string Name { get; }

        SignalDescription Description { get; }

        Task<double[]> ReadValuesAsync(int start, int count);
    }
}
