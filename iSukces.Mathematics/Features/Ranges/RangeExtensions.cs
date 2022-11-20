using System;
using System.Collections.Generic;
using iSukces.Mathematics.Compatibility;
using JetBrains.Annotations;
#if !WPFFEATURES
using ThePoint=iSukces.Mathematics.Compatibility.Point;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint=System.Windows.Point;
using TheVector=System.Windows.Vector;
#endif

namespace iSukces.Mathematics
{
    public static class RangeExtensions
    {
        public static Range GetRange<T>(this IEnumerable<T> src, [NotNull] Func<T, double> map)
        {
            if (src == null)
                return Range.Empty;
            if (map == null) throw new ArgumentNullException(nameof(map));

            var min       = double.NaN;
            var max       = double.NaN;
            foreach (var current in src)
                Aggregate(map(current), ref max, ref min);
            return double.IsNaN(min) ? Range.Empty : new Range(min, max);
        }

        public static Range GetRange([CanBeNull] this IEnumerable<double> src)
        {
            switch (src)
            {
                case null: return Range.Empty;
                case IReadOnlyList<double> list: return Range.FromList(list);
            }
            var min       = double.NaN;
            var max       = double.NaN;
            foreach (var current in src)
                Aggregate(current, ref max, ref min);
            return double.IsNaN(min) ? Range.Empty : new Range(min, max);
        }

        public static Range2D GetRange(this IEnumerable<ThePoint> src)
        {
            if (src == null)
                return Range2D.Empty;

            var x = Range.Empty;
            var y = Range.Empty;
            foreach (var current in src)
            {
                Aggregate(current.X, ref x);
                Aggregate(current.Y, ref y);
            }
            return new Range2D(x, y);
        }

        public static Range3D GetRange(this IEnumerable<Point3D> src)
        {
            if (src == null)
                return Range3D.Empty;

            var x = Range.Empty;
            var y = Range.Empty;
            var z = Range.Empty;
            foreach (var current in src)
            {
                Aggregate(current.X, ref x);
                Aggregate(current.Y, ref y);
                Aggregate(current.Z, ref z);
            }
            return new Range3D(x, y, z);
        }

        public static Range2D GetRange<T>(this IEnumerable<T> src, Func<T, ThePoint> map)
        {
            if (src == null)
                return Range2D.Empty;
            var x = Range.Empty;
            var y = Range.Empty;
            foreach (var current in src)
            {
                var c = map(current);
                Aggregate(c.X, ref x);
                Aggregate(c.Y, ref y);
            }
            return new Range2D(x, y);
        }

        public static Range2D GetRange<T>(this IEnumerable<T> src, Func<T, double> mapX, Func<T, double> mapY)
        {
            if (src == null)
                return Range2D.Empty;
            var x = Range.Empty;
            var y = Range.Empty;
            foreach (var current in src)
            {
                Aggregate(mapX(current), ref x);
                Aggregate(mapY(current), ref y);
            }
            return new Range2D(x, y);
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

        private static void Aggregate(double current, ref Range range)
        {
            if (range.IsEmpty)
                range = new Range(current, current);
            else
            {
                if (current > range.Max)
                    range = new Range(range.Min, current);
                else if (current < range.Min)
                    range = new Range(current, range.Max);
            }
        }
    }
}