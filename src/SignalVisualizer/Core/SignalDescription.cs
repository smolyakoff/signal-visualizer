// ReSharper disable FieldCanBeMadeReadOnly.Local

using System.Runtime.InteropServices;

namespace SignalVisualizer.Core
{
    [StructLayout(LayoutKind.Explicit)]
    public struct SignalDescription
    {
        [FieldOffset(0)] [MarshalAs(UnmanagedType.I4)] private int _channels;

        [FieldOffset(4)] [MarshalAs(UnmanagedType.I4)] private int _sampleSize;

        [FieldOffset(8)] [MarshalAs(UnmanagedType.I4)] private int _spectrumLines;

        [FieldOffset(12)] [MarshalAs(UnmanagedType.I4)] private int _frequency;

        [FieldOffset(16)] [MarshalAs(UnmanagedType.R4)] private float _frequencyResolution;

        [FieldOffset(20)] [MarshalAs(UnmanagedType.R4)] private float _blockTime;

        [FieldOffset(24)] [MarshalAs(UnmanagedType.I4)] private int _totalTime;

        [FieldOffset(28)] [MarshalAs(UnmanagedType.I4)] private int _requestedBlocks;

        [FieldOffset(32)] [MarshalAs(UnmanagedType.I4)] private int _ticks;

        [FieldOffset(36)] [MarshalAs(UnmanagedType.I4)] private int _receivedBlocks;

        [FieldOffset(40)] [MarshalAs(UnmanagedType.R4)] private float _minValue;

        [FieldOffset(44)] [MarshalAs(UnmanagedType.R4)] private float _maxValue;

        public int Channels => _channels;

        public int SampleSize => _sampleSize;

        public int SpectrumLines => _spectrumLines;

        public int Frequency => _frequency;

        public float FrequencyResolution => _frequencyResolution;

        public float BlockTime => _blockTime;

        public int TotalTime => _totalTime;

        public int RequestedBlocks => _requestedBlocks;

        public int ReceivedBlocks => _receivedBlocks;

        public int Ticks => _ticks;

        public float MinValue => _minValue;

        public float MaxValue => _maxValue;
    }
}