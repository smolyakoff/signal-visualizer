using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace SignalVisualizer.Core
{
    public class SignalSource : ISignalSource, IDisposable
    {
        private readonly SignalReader _reader;

        public SignalSource(string name, SignalReader reader)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentNullException>(reader != null);

            _reader = reader;
            Name = name;
        }

        public string Name { get; private set; }

        public SignalDescription Description => _reader.Description;

        public Task<double[]> ReadValuesAsync(int start, int count)
        {
            _reader.Position = start;
            return _reader.ReadValuesAsync(count);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _reader?.Dispose();
            }
        }
    }
}
