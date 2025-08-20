using System;
using System.Collections.Generic;
#if !WPFFEATURES
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;
#else
using System.ComponentModel;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif
#if TYPECONVERTERS
using iSukces.Mathematics.TypeConverters;
using System.ComponentModel;
#endif

namespace iSukces.Mathematics;
#if TYPECONVERTERS
[TypeConverter(typeof(RangeTypeConverter))]
#endif

public readonly struct DRange : IEquatable<DRange>
{
    public DRange(double min, double max)
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

    public static DRange FromCenterAndLength(double center, double length)
    {
        if (length < 0) return Empty;
        length *= 0.5;
        return new DRange(center - length, center + length);
    }
    
    public static DRange FromMinAndLength(double min, double length)
    {
        return new DRange(min, min + length);
    }

    public static DRange FromMaxAndLength(double max, double length)
    {
        return new DRange(max - length, max);
    }

    public static DRange FromList(IReadOnlyList<double>? doubles)
    {
        if (doubles is null || doubles.Count == 0)
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

        return new DRange(min, max);
    }

    public static DRange FromValues(double a, double b) { return a > b ? new DRange(b, a) : new DRange(a, b); }

    public static DRange FromValues(params double[] a) { return FromList(a); }

    public static DRange operator +(DRange a, double b) { return a.IsEmpty ? a : new DRange(a.Min + b, a.Max + b); }

    public static DRange operator +(double b, DRange a) { return a.IsEmpty ? a : new DRange(a.Min + b, a.Max + b); }

    public static DRange operator &(DRange a, DRange b)
    {
        if (a.IsEmptyOrInvalid || b.IsEmptyOrInvalid)
            return Empty;
        var tmp = new DRange(Math.Max(a.Min, b.Min), Math.Min(a.Max, b.Max));
        return tmp._kind == RangeKind.Invalid ? Empty : tmp;
    }

    public static DRange operator |(DRange a, DRange b)
    {
        if (a.IsEmptyOrInvalid)
            return b;
        if (b.IsEmptyOrInvalid)
            return a;
        return new DRange(Math.Min(a.Min, b.Min), Math.Max(a.Max, b.Max));
    }

    public static DRange operator |(DRange a, double b)
    {
        if (a.IsEmptyOrInvalid)
            return new DRange(b, b);
        return new DRange(Math.Min(a.Min, b), Math.Max(a.Max, b));
    }

    public static bool operator ==(DRange left, DRange right) { return left.Equals(right); }

    public static explicit operator MinMax(DRange r) { return r.IsEmpty ? new MinMax() : new MinMax(r.Min, r.Max); }

    public static explicit operator DRange(MinMax r) { return r.IsInvalid ? Empty : new DRange(r.Min, r.Max); }

    public static bool operator !=(DRange left, DRange right) { return !left.Equals(right); }


    public static DRange operator -(DRange a, double b) { return a.IsEmpty ? a : new DRange(a.Min - b, a.Max - b); }

    public static DRange[] operator -(DRange a, DRange b)
    {
        var notCutting = a.IsZeroOnInvalid
                         || b.IsZeroOnInvalid
                         || b.Min >= a.Max
                         || b.Max <= a.Min;
        if (notCutting) return [a];

        var has1 = b.Min > a.Min;
        var has2 = b.Max < a.Max;
        if (!has1)
            return has2 ? [new DRange(b.Max, a.Max)] : [];
        var r1 = new DRange(a.Min, b.Min);
        return has2
            ? [r1, new DRange(b.Max, a.Max)]
            : [r1];
    }

    public static DRange operator -(DRange a) { return a.IsEmpty ? a : new DRange(-a.Max, -a.Min); }

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

    public bool Equals(DRange other)
    {
        var e1 = IsEmpty;
        var e2 = other.IsEmpty;
        if (e1 || e2) return e1 && e2;
        return Max.Equals(other.Max) && Min.Equals(other.Min);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is DRange range && Equals(range);
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
    
    public DRange RoundDouble() { return new DRange(Min.Round(), Max.Round()); }

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

    public DRange WithDeltaMax(double deltaMax)
    {
        return _kind != RangeKind.Empty ? new DRange(Min, Max + deltaMax) : this;
    }

    public DRange WithDeltaMin(double deltaMin)
    {
        return _kind != RangeKind.Empty ? new DRange(Min + deltaMin, Max) : this;
    }

    public DRange WithMax(double newMax) { return _kind != RangeKind.Empty ? new DRange(Min, newMax) : this; }


    public DRange WithMin(double newMin) { return _kind != RangeKind.Empty ? new DRange(newMin, Max) : this; }

    public DRange WithValue(double x)
    {
        return IsEmpty
            ? new DRange(x, x)
            : new DRange(Math.Min(Min, x), Math.Max(Max, x));
    }

    public static DRange Empty => new DRange();

    public double Min { get; }
    public double Max { get; }

    public double Center => _kind != RangeKind.Empty ? (Max + Min) * 0.5 : double.NaN;

    public double Length => _kind != RangeKind.Empty ? Max - Min : double.NaN;

    public bool IsEmpty          => _kind == RangeKind.Empty;
    public bool IsEmptyOrInvalid => _kind is RangeKind.Empty or RangeKind.Invalid;

    public bool HasPositiveLength => _kind == RangeKind.Normal;

    public bool IsZeroOnInvalid => _kind is RangeKind.Empty or RangeKind.Zero;

    private readonly RangeKind _kind;
}
 