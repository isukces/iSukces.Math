using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Mathematics
{
    public static class MinMaxExtensions
    {
        public static MinMax GetMinMax(this IEnumerable<double> x)
        {
            return MinMax.FromValues(x);
        }

        public static MinMax GetMinMax<T>(this IEnumerable<double> x)
        {
            return MinMax.FromValues(x);
        }

        public static MinMax GetMinMax<T>(this IEnumerable<int> x)
        {
            return MinMax.FromValues(x);
        }

        public static MinMax GetMinMax<T>(this IEnumerable<T> x, Func<T, double> cast)
        {
            return MinMax.FromValues(x.Select(cast));
        }
    }
}