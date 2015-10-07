using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace SignalVisualizer.Core
{
    public class SignalCollection : IEnumerable<Signal>
    {
        private readonly List<Signal> _signals;

        private SignalCollection(List<Signal> signals)
        {
            _signals = signals;
        }

        public static SignalCollection FromFile(string filePath)
        {
            Contract.Requires<ArgumentException>(File.Exists(filePath), "File does not exist.");
            var collectionFileInfo = new FileInfo(filePath);
            if (collectionFileInfo.Directory == null)
            {
                throw new InvalidOperationException("Invalid file path.");
            }

            var sources = File.ReadAllLines(filePath)
                .Select(x => Path.Combine(collectionFileInfo.Directory.FullName, x))
                .Where(File.Exists)
                .Select(Signal.FromFile)
                .ToList();
            return new SignalCollection(sources);
        }

        public bool IsEmpty => _signals.Count == 0;

        public int MaxLength => _signals.Max(x => x.Description.Ticks);

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
