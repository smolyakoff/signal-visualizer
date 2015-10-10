using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SignalVisualizer.Core
{
    public static class SignalSerializer
    {
        public static SignalCollection DeserializeCollectionFromFile(string filePath)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(filePath));
            var extension = Path.GetExtension(filePath);
            switch (extension)
            {
                case ".bin":
                    return new SignalCollection(new [] {DeserializeFromFile(filePath)});
                case ".txt":
                    var collectionFileInfo = new FileInfo(filePath);
                    if (collectionFileInfo.Directory == null)
                    {
                        throw new InvalidOperationException("Invalid file path.");
                    }

                    var sources = File.ReadAllLines(filePath)
                        .Select(x => Path.Combine(collectionFileInfo.Directory.FullName, x))
                        .Where(File.Exists)
                        .Select(DeserializeFromFile)
                        .ToList();
                    return new SignalCollection(sources);
                default:
                    throw new NotSupportedException($"File extension {extension} is not supported.");    
            }
        }

        public static Signal DeserializeFromFile(string fileName)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(fileName));

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var signal = DeserializeFromStream(fs);
                return signal;
            }
        }

        public static Signal DeserializeFromStream(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);

            using (var reader = new BinaryReader(stream))
            {
                var streamHeader = Encoding.ASCII.GetString(reader.ReadBytes(4));
                if (!streamHeader.StartsWith("TMB"))
                {
                    throw new NotSupportedException($"File is not supported. Invalid header: {streamHeader}.");
                }
                var handle = new GCHandle();
                SignalHeader header;
                try
                {
                    var data = reader.ReadBytes(Marshal.SizeOf(typeof (SignalHeader)));
                    handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    header = (SignalHeader) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof (SignalHeader));
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                }
                var buffer = new byte[stream.Length - stream.Position];
                reader.BaseStream.Read(buffer, 0, buffer.Length);
                var values = Enumerable.Range(0, buffer.Length)
                    .Where(x => x%4 == 0)
                    .Select(i => BitConverter.ToSingle(buffer, i))
                    .Select(Convert.ToDouble)
                    .ToArray();
                return new Signal(header, values);
            }
        }
    }
}
