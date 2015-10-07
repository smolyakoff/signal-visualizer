using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace SignalVisualizer.Core
{
    public class SignalSourceCollection : ISignalSourceCollection, IDisposable
    {
        private readonly List<SignalSource> _sources;

        private SignalSourceCollection(List<SignalSource> sources)
        {
            _sources = sources;
        }

        public static SignalSourceCollection FromFile(string filePath)
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
                .Select(f => new SignalSource(Path.GetFileName(f), new SignalReader(f)))
                .ToList();
            return new SignalSourceCollection(sources);
        }

        public bool IsEmpty => _sources.Count == 0;

        public int MaxLength => _sources.Max(x => x.Description.Ticks);

        public IEnumerator<ISignalSource> GetEnumerator()
        {
            return _sources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
                _sources.ForEach(s => s?.Dispose());
                _sources.Clear();
            }
        }
    }
}
