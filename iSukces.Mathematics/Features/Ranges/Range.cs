#if COREFX
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;
#else
using System.ComponentModel;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif

#if ALLFEATURES
using iSukces.Mathematics.TypeConverters;
#endif
using System;
using System.Collections.Generic;


namespace iSukces.Mathematics
{
#if ALLFEATURES
    [TypeConverter(typeof(RangeTypeConverter))]
#endif

    public struct Range : IEquatable<Range>
    {
        public Range(double min, double max)
            : this()
        {
            Min = min;
            Max = max;
            if (min < max)
                _kind = RangeKind.Normal;
            else if (min > max)
                _kind = RangeKind.Invalid;
            else
                _kind = RangeKind.Zero;
        }

        public static Range FromCenterAndLength(double center, double length)
        {
            if (length < 0) return Empty;
            length *= 0.5;
            return new Range(center - length, center + length);
        }

        public static Range FromList(IReadOnlyList<double> doubles)
        {
            if (doubles == null || doubles.Count == 0)
                return Empty;
            var min = doubles[0];
            var max = min;
            for (var i = 1; i < doubles.Count; i++)
            {
                var current = doubles[i];
                if (current > max)
                    max = current;
                else if (current < min)
                    min = current;
            }

            return new Range(min, max);
        }

        public static Range FromValues(double a, double b) { return a > b ? new Range(b, a) : new Range(a, b); }

        public static Range FromValues(params double[] a) { return FromList(a); }

        /*public static Range operator +(Range a, Range b)
        {
            if (a.IsZeroOnInvalid) return b;
            if (b.IsZeroOnInvalid) return a;

            var min = Math.Min(a.Min, b.Min);
            var max = Math.Max(a.Max, b.Max);
            return new Range(min, max);
        }*/

        public static Range operator +(Range a, double b) { return a.IsEmpty ? a : new Range(a.Min + b, a.Max + b); }

        public static Range operator +(double b, Range a) { return a.IsEmpty ? a : new Range(a.Min + b, a.Max + b); }

        public static Range operator &(Range a, Range b)
        {
            if (a.IsEmptyOrInvalid || b.IsEmptyOrInvalid)
                return Empty;
            var tmp = new Range(Math.Max(a.Min, b.Min), Math.Min(a.Max, b.Max));
            return tmp._kind == RangeKind.Invalid ? Empty : tmp;
        }

        public static Range operator |(Range a, Range b)
        {
            if (a.IsEmptyOrInvalid)
                return b;
            if (b.IsEmptyOrInvalid)
                return a;
            return new Range(Math.Min(a.Min, b.Min), Math.Max(a.Max, b.Max));
        }

        public static Range operator |(Range a, double b)
        {
            if (a.IsEmptyOrInvalid)
                return new Range(b, b);
            return new Range(Math.Min(a.Min, b), Math.Max(a.Max, b));
        }

        public static bool operator ==(Range left, Range right) { return left.Equals(right); }

        public static explicit operator MinMax(Range r) { return r.IsEmpty ? new MinMax() : new MinMax(r.Min, r.Max); }

        public static explicit operator Range(MinMax r) { return r.IsInvalid ? Empty : new Range(r.Min, r.Max); }

        public static bool operator !=(Range left, Range right) { return !left.Equals(right); }


        /*
        public static Range operator *(Range a, Range b)
        {
            if (a.IsZeroOnInvalid || b.IsZeroOnInvalid) return Empty;
            var min = Math.Max(a.Min, b.Min);
            var max = Math.Min(a.Max, b.Max);
            return new Range(min, max);
        }
        */

        public static Range operator -(Range a, double b) { return a.IsEmpty ? a : new Range(a.Min - b, a.Max - b); }

        public static Range[] operator -(Range a, Range b)
        {
            var notCutting = a.IsZeroOnInvalid
                             || b.IsZeroOnInvalid
                             || b.Min >= a.Max
                             || b.Max <= a.Min;
            if (notCutting) return new[] { a };

            var has1 = b.Min > a.Min;
            var has2 = b.Max < a.Max;
            if (!has1)
                return has2 ? new[] { new Range(b.Max, a.Max) } : Array.Empty<Range>();
            var r1 = new Range(a.Min, b.Min);
            return has2
                ? new[] { r1, new Range(b.Max, a.Max) }
                : new[] { r1 };
        }

        public static Range operator -(Range a) { return a.IsEmpty ? a : new Range(-a.Max, -a.Min); }

        public bool Cut(ref double x)
        {
            if (IsEmptyOrInvalid)
                return false;
            if (x < Min)
                x = Min;
            else if (x > Max)
                x = Max;
            return true;
        }

        public bool Equals(Range other)
        {
            var e1 = IsEmpty;
            var e2 = other.IsEmpty;
            if (e1 || e2) return e1 && e2;
            return Max.Equals(other.Max) && Min.Equals(other.Min);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Range range && Equals(range);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _kind == RangeKind.Empty
                    ? 0
                    : (Max.GetHashCode() * 397) ^ Min.GetHashCode();
            }
        }

        public bool Includes(double x) { return !IsEmpty && x >= Min && x <= Max; }

        public bool IncludesExclusive(double x) { return !IsEmpty && x > Min && x < Max; }

        public RangeI Round() { return new RangeI(Min.Round(), Max.Round()); }
        public Range RoundDouble() { return new Range(Min.Round(), Max.Round()); }

        public bool ShouldSerializeCenter() { return false; }

        public bool ShouldSerializeHasPositiveLength() { return false; }

        public bool ShouldSerializeIsEmpty() { return false; }

        public bool ShouldSerializeIsEmptyOrInvalid() { return false; }

        public bool ShouldSerializeIsZeroOnInvalid() { return false; }

        public bool ShouldSerializeLength() { return false; }

        public override string ToString()
        {
            return IsEmpty
                ? "Empty range"
                : Min > Max
                    ? "Invalid range"
                    : $"Range {Min}..{Max}";
        }

        public Range WithDeltaMax(double deltaMax)
        {
            return _kind != RangeKind.Empty ? new Range(Min, Max + deltaMax) : this;
        }

        public Range WithDeltaMin(double deltaMin)
        {
            return _kind != RangeKind.Empty ? new Range(Min + deltaMin, Max) : this;
        }

        public Range WithMax(double newMax) { return _kind != RangeKind.Empty ? new Range(Min, newMax) : this; }


        public Range WithMin(double newMin) { return _kind != RangeKind.Empty ? new Range(newMin, Max) : this; }

        public Range WithValue(double x)
        {
            return IsEmpty
                ? new Range(x, x)
                : new Range(Math.Min(Min, x), Math.Max(Max, x));
        }

        public static Range Empty => new Range();

        public double Min { get; }
        public double Max { get; }

        public double Center => _kind != RangeKind.Empty ? (Max + Min) * 0.5 : double.NaN;

        public double Length => _kind != RangeKind.Empty ? Max - Min : double.NaN;

        public bool IsEmpty          => _kind == RangeKind.Empty;
        public bool IsEmptyOrInvalid => _kind == RangeKind.Empty || _kind == RangeKind.Invalid;

        public bool HasPositiveLength => _kind == RangeKind.Normal;

        public bool IsZeroOnInvalid => _kind == RangeKind.Empty || _kind == RangeKind.Zero;

        private readonly RangeKind _kind;
    }

    public enum RangeKind : byte
    {
        Empty,
        Zero,
        Invalid,
        Normal
    }
}

