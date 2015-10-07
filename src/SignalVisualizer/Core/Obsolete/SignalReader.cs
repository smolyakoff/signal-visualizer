using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SignalVisualizer.Core
{
    public class SignalReader : IDisposable
    {
        private const int ValueSize = 4;

        private static readonly int HeaderSize = Marshal.SizeOf(typeof(SignalDescription)) + 4;

        private readonly BinaryReader _reader;

        private readonly Lazy<SignalDescription> _description; 

        public SignalReader(string fileName) : this(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fileName));
        }

        public SignalReader(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);
            Contract.Requires<ArgumentException>(stream.CanRead, "Stream should be readable.");
            Contract.Requires<ArgumentException>(stream.CanSeek, "Stream should be seekable.");

            _reader = new BinaryReader(stream);
            _description = new Lazy<SignalDescription>(ReadDescription);
            Validate();
        }

        public SignalDescription Description => _description.Value;

        public int Position
        {
            get { return GetPosition(); }
            set { SetPosition(value); }
        }

        public int Length => Description.Ticks;

        public bool HasNextValue => Position < Length;

        public double ReadNextValue()
        {
            Contract.Requires<InvalidOperationException>(HasNextValue, "No more signal values to read.");
            return _reader.ReadSingle();
        }

        public async Task<double[]> ReadValuesAsync(int count)
        {
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0 && Position + count < Length);

            var buffer = new byte[count * ValueSize];
            await _reader.BaseStream.ReadAsync(buffer, 0, buffer.Length);
            return Enumerable.Range(0, buffer.Length)
                .Where(x => x % ValueSize == 0)
                .Select(i => BitConverter.ToSingle(buffer, i))
                .Select(Convert.ToDouble)
                .ToArray();
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

        private int GetPosition()
        {
            // When header has not been read
            if (_reader.BaseStream.Position == 0)
            {
                return 0;
            }
            var valuePosition = Convert.ToInt32(_reader.BaseStream.Position - HeaderSize) / ValueSize;
            return valuePosition;
        }

        private void SetPosition(int position)
        {
            Contract.Requires<ArgumentOutOfRangeException>(position < Description.Ticks && position >= 0);

            _reader.BaseStream.Position = HeaderSize + position * ValueSize;
        }

        private void Validate()
        {
            var initialPosition = _reader.BaseStream.Position;
            try
            {
                var header = Encoding.ASCII.GetString(_reader.ReadBytes(4));
                if (!header.StartsWith("TMB"))
                {
                    throw new InvalidOperationException($"Invalid header: {header}.");
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Stream format is not supported.", ex);
            }
            finally
            {
                if (_reader.BaseStream.CanSeek)
                {
                    _reader.BaseStream.Position = initialPosition;
                }
            }
        }

        private SignalDescription ReadDescription()
        {
            var handle = new GCHandle();
            try
            {
                var data = _reader.ReadBytes(Marshal.SizeOf(typeof(SignalDescription)));
                handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                var description = (SignalDescription) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(SignalDescription));
                return description;
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
        }
    }
}
