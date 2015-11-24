using System;
using System.Diagnostics.Contracts;

namespace SignalVisualizer.Core
{
    public struct Slice : IEquatable<Slice>
    {
        public Slice(int position, int length)
        {
            Contract.Requires<ArgumentOutOfRangeException>(position >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(length >= 0);

            Position = position;
            Length = length;
        }

        public int Position { get; }

        public int Length { get; }

        public override string ToString()
        {
            return $"POS: {Position} N: {Length}";
        }

        public bool Equals(Slice other)
        {
            return Position == other.Position && Length == other.Length;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Slice && Equals((Slice) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position*397) ^ Length;
            }
        }

        public static bool operator ==(Slice left, Slice right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Slice left, Slice right)
        {
            return !left.Equals(right);
        }
    }
}