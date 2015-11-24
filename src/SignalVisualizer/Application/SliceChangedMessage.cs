using SignalVisualizer.Core;

namespace SignalVisualizer.Application
{
    public class SliceChangedMessage
    {
        public SliceChangedMessage(Slice slice)
        {
            Slice = slice;
        }

        public Slice Slice { get; }
    }
}