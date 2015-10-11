using System;
using System.Diagnostics.Contracts;
using SignalVisualizer.Core;

namespace SignalVisualizer.Application.Events
{
    public class RangeChangedEvent : IRange
    {
        public RangeChangedEvent(int lowerBound, int upperBound)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(lowerBound >= 0 && upperBound > lowerBound);

            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public int LowerBound { get; }

        public int UpperBound { get; }
    }
}
