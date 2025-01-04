using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif

#if TYPECONVERTERS
using iSukces.Mathematics.TypeConverters;
#endif


namespace iSukces.Mathematics;
#if TYPECONVERTERS
[TypeConverter(typeof(RangeITypeConverter))]
#endif
public struct RangeI : IEquatable<RangeI>
{
    public RangeI(int min, int max) : this()
    {
        Min            = min;
        Max            = max;
        _isInitialized = true;
    }

    public static IReadOnlyList<RangeI> Cut(IEnumerable<RangeI> src, RangeI cutter)
    {
        var result = new List<RangeI>();
        foreach (var rr in src)
            result.AddRange(rr.Cut(cutter));
        return result;
    }

    public static IReadOnlyList<RangeI> Cut(IEnumerable<RangeI> src, IEnumerable<RangeI> cutters)
    {
        if (src is null)
            return Array.Empty<RangeI>();
        if (cutters != null)
            foreach (var c in cutters)
            {
                var a = Cut(src, c);
                src = a;
            }
        return src as IReadOnlyList<RangeI> ?? src.ToList();
    }

    public static RangeI FromCenterAndLength(int center, int length)
    {
        if (length < 0) return Empty;
        var min = center - length / 2;
        return new RangeI(min, min + length);
    }

    public static RangeI FromList(IList<int>? a)
    {
        if (a is null || a.Count == 0)
            return Empty;
        var min = a[0];
        var max = min;
        for (int i = 1, m = a.Count; i < m; i++)
        {
            var current = a[i];
            if (current > max)
                max = current;
            else if (current < min)
                min = current;
        }
        return new RangeI(min, max);
    }

    public static RangeI FromValues(int a, int b)
    {
        return a > b ? new RangeI(b, a) : new RangeI(a, b);
    }

    public static RangeI FromValues(params int[] a)
    {
        return FromList(a);
    }

    public static bool IsInAnyRange(IEnumerable<RangeI>? ranges, int x)
    {
        return ranges != null && ranges.Any(r => r.Includes(x));
    }


    public static IReadOnlyList<RangeI> Merge(IEnumerable<RangeI> src, RangeI append)
    {
        src = src.Where(x => !x.IsZeroOnInvalid);
        if (append.IsZeroOnInvalid)
            return src.ToList();
        var r = new List<RangeI>(src) {append};
        while (true)
        {
            var l = r.Count;
            if (l < 2) return r;
            var wasChange = false;
            for (var i = 0; i < l; i++)
            {
                var a = r[i];
                for (var j = i + 1; j < l; j++)
                {
                    var    b = r[j];
                    RangeI c;
                    if (!a.TryMergeValidRanges(b, out c)) continue;
                    r[i] = c;
                    r.RemoveAt(l - 1);
                    wasChange = true;
                    break;
                }
                if (wasChange) break;
            }
            if (!wasChange) break;
        }
        return r;
    }


    public static List<RangeI> Merge(IEnumerable<RangeI> src, IEnumerable<RangeI>? append)
    {
        if (append != null)
            foreach (var a in append)
                src = Merge(src, a);
        return src.ToList();
    }

    public static RangeI operator +(RangeI a, RangeI b)
    {
        if (a.IsZeroOnInvalid) return b;
        if (b.IsZeroOnInvalid) return a;

        var min = Math.Min(a.Min, b.Min);
        var max = Math.Max(a.Max, b.Max);
        return new RangeI(min, max);
    }

    public static RangeI operator +(RangeI a, int b)
    {
        return a.IsEmpty ? a : new RangeI(a.Min + b, a.Max + b);
    }

    public static RangeI operator +(int b, RangeI a)
    {
        return a.IsEmpty ? a : new RangeI(a.Min + b, a.Max + b);
    }

    public static bool operator ==(RangeI left, RangeI right)
    {
        return left.Equals(right);
    }

    public static explicit operator MinMax(RangeI r)
    {
        return r.IsEmpty ? new MinMax() : new MinMax(r.Min, r.Max);
    }

    public static explicit operator MinMaxI(RangeI r)
    {
        return r.IsEmpty ? new MinMaxI() : new MinMaxI(r.Min, r.Max);
    }

    public static explicit operator RangeI(MinMaxI r)
    {
        return r.IsInvalid ? Empty : new RangeI(r.Min, r.Max);
    }

    public static bool operator !=(RangeI left, RangeI right)
    {
        return !left.Equals(right);
    }


    public static RangeI operator *(RangeI a, RangeI b)
    {
        if (a.IsEmpty || b.IsEmpty) return Empty;
        var min = Math.Max(a.Min, b.Min);
        var max = Math.Min(a.Max, b.Max);
        return new RangeI(min, max);
    }

    public static RangeI operator -(RangeI a, int b)
    {
        return a.IsEmpty ? a : new RangeI(a.Min - b, a.Max - b);
    }


    public static RangeI[] operator -(RangeI a, RangeI b)
    {
        var notCutting = a.IsZeroOnInvalid
                         || b.IsZeroOnInvalid
                         || b.Min >= a.Max
                         || b.Max <= a.Min;
        if (notCutting) return new[] {a};

        var has1 = b.Min > a.Min;
        var has2 = b.Max < a.Max;
        if (!has1)
            return has2 ? new[] {new RangeI(b.Max, a.Max)} : Array.Empty<RangeI>();
        var r1 = new RangeI(a.Min, b.Min);
        return has2
            ? new[] {r1, new RangeI(b.Max, a.Max)}
            : new[] {r1};
    }


    public static RangeI operator -(RangeI a)
    {
        return a.IsEmpty ? a : new RangeI(-a.Max, -a.Min);
    }


    /// <summary>
    ///     Zwraca tablicę przedziałów utworzoną na podstawie listy punktów krańcowych
    /// </summary>
    /// <param name="edges">lista punktów krańcowych</param>
    /// <returns>tablica przedziałów</returns>
    public static RangeI[] RangesFromEdges(IEnumerable<int> edges)
    {
        var edges1 = edges.Distinct().OrderBy(a => a).ToArray();
        var cnt    = edges1.Length - 1;
        if (cnt < 1) return Array.Empty<RangeI>();
        var result = new RangeI[cnt];
        for (var i = 0; i < cnt; i++)
            result[i] = new RangeI(edges1[i], edges1[i + 1]);
        return result;
    }

    public RangeI[] ArrayOfValid()
    {
        return IsZeroOnInvalid
            ? Array.Empty<RangeI>()
            : new[] {this};
    }

    public IReadOnlyList<RangeI> Cut(IEnumerable<RangeI> cutters)
    {
        return Cut(new[] {this}, cutters);
    }

    public IReadOnlyList<RangeI> Cut(RangeI cutter)
    {
        if (IsEmpty)
            return Array.Empty<RangeI>();
        var common = cutter * this;
        if (common.IsZeroOnInvalid)
            return new[] {this};
        if (common.Equals(this))
            return Array.Empty<RangeI>();

        if (cutter.IsZeroOnInvalid || cutter.Max <= Min || cutter.Min >= Max)
            return new[] {this};
        if (cutter.Min > Min && cutter.Max < Max)
            // wycina środek
            return new[] {new RangeI(Min, cutter.Min), new RangeI(cutter.Max, Max)};
        if (cutter.Min <= Min && cutter.Max >= Max)
            // całkowicie wycina
            return Array.Empty<RangeI>();
        var result = new List<RangeI>();
        if (cutter.Min > Min)
            result.Add(new RangeI(Min, Math.Min(Max, cutter.Min)));
        if (cutter.Max < Max)
            result.Add(new RangeI(Math.Max(Min, cutter.Max), Max));
        return result.ToArray();
    }

    public bool Equals(RangeI other)
    {
        var e1 = IsEmpty;
        var e2 = other.IsEmpty;
        if (e1 || e2) return e1 && e2;
        return Max.Equals(other.Max) && Min.Equals(other.Min);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is RangeI && Equals((RangeI)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return _isInitialized
                ? (Max.GetHashCode() * 397)
                  ^ Min.GetHashCode()
                : 0;
        }
    }


    /// <summary>
    ///     Czy ten i inny zakres mają kawałek wspólny
    /// </summary>
    /// <param name="o">inny zakres</param>
    /// <param name="acceptPointOnly">akceptuje także stykanie się końcami</param>
    /// <returns></returns>
    public bool HasCommonAreaWith(RangeI o, bool acceptPointOnly)
    {
        var common = this * o;
        if (common.IsEmpty) return false;
        if (acceptPointOnly)
            return common.Length >= 0;
        return common.Length > 0;
    }

    public bool Includes(int x)
    {
        return !IsEmpty && x >= Min && x <= Max;
    }

    public bool IncludesExclusive(int x)
    {
        return !IsEmpty && x > Min && x < Max;
    }

    public bool ShouldSerializeCenter()
    {
        return false;
    }

    public bool ShouldSerializeHasPositiveLength()
    {
        return false;
    }

    public bool ShouldSerializeIsEmpty()
    {
        return false;
    }

    public bool ShouldSerializeIsZeroOnInvalid()
    {
        return false;
    }

    public bool ShouldSerializeLength()
    {
        return false;
    }

    public override string ToString()
    {
        return IsEmpty
            ? "Empty intrange"
            : Min > Max ? "Invalid intrange" : $"intRange {Min}..{Max}";
    }

    /// <summary>
    ///     Próbuje wykonać sumę zakresów; zakłada, że oba są prawidłowe i niezerowe
    /// </summary>
    /// <param name="other"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryMergeValidRanges(RangeI other, out RangeI result)
    {
        // obydwa są prawidłowe
        if (Min > other.Max || Max < other.Min)
        {
            result = this;
            return false;
        }
        result = new RangeI(Math.Min(Min, other.Min), Math.Max(Max, other.Max));
        return true;
    }

    public RangeI WithDeltaMax(int deltaMax)
    {
        return _isInitialized ? new RangeI(Min, Max + deltaMax) : this;
    }

    public RangeI WithDeltaMin(int deltaMin)
    {
        return _isInitialized ? new RangeI(Min + deltaMin, Max) : this;
    }

    public RangeI WithMax(int newMax)
    {
        return _isInitialized ? new RangeI(Min, newMax) : this;
    }

    public RangeI WithMin(int newMin)
    {
        return _isInitialized ? new RangeI(newMin, Max) : this;
    }

    public RangeI WithValue(int x)
    {
        return IsEmpty
            ? new RangeI(x, x)
            : new RangeI(Math.Min(Min, x), Math.Max(Max, x));
    }

    public static RangeI Empty
    {
        get { return new RangeI(); }
    }

    public int Min { get; private set; }
    public int Max { get; private set; }
    public int Center
    {
        get { return _isInitialized ? (Max + Min) / 2 : int.MinValue; }
    }

    public int Length
    {
        get { return _isInitialized ? Max - Min : int.MinValue; }
    }

    public bool IsEmpty
    {
        get { return !_isInitialized; }
    }

    public bool HasPositiveLength
    {
        get { return _isInitialized && Max > Min; }
    }

    public bool IsZeroOnInvalid
    {
        get { return IsEmpty || Max <= Min; }
    }

    private readonly bool _isInitialized;
}