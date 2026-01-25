using System;
using System.Collections.Generic;

namespace iSukces.Mathematics;

public static class RangeExtensions
{
    public static DRange GetRange<T>(this IEnumerable<T>? src, Func<T, double> map)
    {
        if (src is null)
            return DRange.Empty;
        if (map is null) throw new ArgumentNullException(nameof(map));

        var min = double.NaN;
        var max = double.NaN;
        foreach (var current in src)
            Aggregate(map(current), ref max, ref min);
        return double.IsNaN(min) ? DRange.Empty : new DRange(min, max);
    }

    public static DRange GetRange(this IEnumerable<double>? src)
    {
        switch (src)
        {
            case null: return DRange.Empty;
            case IReadOnlyList<double> list: return DRange.FromList(list);
        }
        var min = double.NaN;
        var max = double.NaN;
        foreach (var current in src)
            Aggregate(current, ref max, ref min);
        return double.IsNaN(min) ? DRange.Empty : new DRange(min, max);
    }

    public static Range2D GetRange(this IEnumerable<Point>? src)
    {
        if (src is null)
            return Range2D.Empty;

        var x = DRange.Empty;
        var y = DRange.Empty;
        foreach (var current in src)
        {
            Aggregate(current.X, ref x);
            Aggregate(current.Y, ref y);
        }
        return new Range2D(x, y);
    }

    public static Range3D GetRange(this IEnumerable<Point3D>? src)
    {
        if (src is null)
            return Range3D.Empty;

        var x = DRange.Empty;
        var y = DRange.Empty;
        var z = DRange.Empty;
        foreach (var current in src)
        {
            Aggregate(current.X, ref x);
            Aggregate(current.Y, ref y);
            Aggregate(current.Z, ref z);
        }
        return new Range3D(x, y, z);
    }

    extension<T>(IEnumerable<T>? src)
    {
        public Range2D GetRange(Func<T, Point> map)
        {
            if (src is null)
                return Range2D.Empty;
            var x = DRange.Empty;
            var y = DRange.Empty;
            foreach (var current in src)
            {
                var c = map(current);
                Aggregate(c.X, ref x);
                Aggregate(c.Y, ref y);
            }
            return new Range2D(x, y);
        }

        public Range2D GetRange(Func<T, double> mapX, Func<T, double> mapY)
        {
            if (src is null)
                return Range2D.Empty;
            var x = DRange.Empty;
            var y = DRange.Empty;
            foreach (var current in src)
            {
                Aggregate(mapX(current), ref x);
                Aggregate(mapY(current), ref y);
            }
            return new Range2D(x, y);
        }
    }

    private static void Aggregate(double current, ref double max, ref double min)
    {
        if (double.IsNaN(min))
        {
            min       = max = current;
            return;
        }
        if (current > max)
            max = current;
        else if (current < min)
            min = current;
    }

    private static void Aggregate(double current, ref DRange range)
    {
        if (range.IsEmpty)
            range = new DRange(current, current);
        else
        {
            if (current > range.Max)
                range = new DRange(range.Min, current);
            else if (current < range.Min)
                range = new DRange(current, range.Max);
        }
    }
}