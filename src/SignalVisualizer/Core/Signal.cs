using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using MathNet.Numerics.IntegralTransforms;
using OxyPlot;

namespace SignalVisualizer.Core
{
    public class Signal
    {
        private readonly SignalDescription _description;
        private readonly double[] _values;

        public static Signal FromFile(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(fs))
            {
                var header = Encoding.ASCII.GetString(reader.ReadBytes(4));
                if (!header.StartsWith("TMB"))
                {
                    throw new NotSupportedException($"File is not supported. Invalid header: {header}.");
                }
                var handle = new GCHandle();
                SignalDescription description;
                try
                {
                    var data = reader.ReadBytes(Marshal.SizeOf(typeof(SignalDescription)));
                    handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    description = (SignalDescription)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(SignalDescription));
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                }
                var buffer = new byte[fs.Length - fs.Position];
                reader.BaseStream.Read(buffer, 0, buffer.Length);
                var values = Enumerable.Range(0, buffer.Length)
                    .Where(x => x % 4 == 0)
                    .Select(i => BitConverter.ToSingle(buffer, i))
                    .Select(Convert.ToDouble)
                    .ToArray();
                return new Signal(description, values);
            }
        }

        private Signal(SignalDescription description, double[] values)
        {
            _description = description;
            _values = values;
        }

        public SignalDescription Description => _description;

        public SignalSample GetSample(int position, int count)
        {
            return new SignalSample(_description, position, count, _values);
        }
    }

    public class SignalSample : IEnumerable<DataPoint>
    {
        private readonly SignalDescription _description;
        private readonly int _position;
        private readonly int _count;
        private readonly double[] _values;
        private readonly Lazy<DataPoint[]> _spectrum;

        public SignalSample(SignalDescription description, int position, int count, double[] values)
        {
            _description = description;
            _position = position;
            _count = count;
            _values = values;
            _spectrum = new Lazy<DataPoint[]>(CalculateSpectrum);
        }

        public IEnumerable<DataPoint> Spectrum => _spectrum.Value;

        private DataPoint[] CalculateSpectrum()
        {
            var complexValues = _values.Skip(_position)
                .Take(_count)
                .Select(x => new Complex(x, 0))
                .ToArray();
            Fourier.Forward(complexValues, FourierOptions.Matlab);
            var spectrum = complexValues.Select((v, i) =>
            {
                var x = i*(double) _description.Frequency/complexValues.Length;
                var y = v.Magnitude*2/complexValues.Length;
                return new DataPoint(x, y);
            }).Take(complexValues.Length / 2).ToArray();
            return spectrum;
        }

        public IEnumerator<DataPoint> GetEnumerator()
        {
            for (var i = _position; i < _position + _count; i++)
            {
                yield return new DataPoint(i, _values[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
