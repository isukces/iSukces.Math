using System;


namespace iSukces.Mathematics
{
    public class Iterator
    {
#if NOTREADY
        public static double FindMax(Func<double, double> func, double min, double max, int iterations)
        {
            var    step  = 4;
            var    y     = new double[step + 1];
            double bestX = 0;
            for (var iteration = 0; iteration < iterations; iteration++)
            {
                var    dx   = (max - min) / step;
                double maxY = int.MinValue;
                var    idx  = -1;
                for (var i = 0; i <= step; i++)
                {
                    var x = min + i * dx;
                    var d = func(x);
                    y[i] = d;
                    if (d > maxY)
                    {
                        maxY  = d;
                        idx   = i;
                        bestX = x;
                    }
                }

                if (idx == 0)
                {
                    max = min + dx;
                }
                else if (idx == step - 1)
                {
                    min = max - dx;
                }
                else
                {
                    max = bestX + dx;
                    min = bestX - dx;
                }
            }

            return bestX;
        }
#endif
        public static double? Solve(Func<double, double> func, double min, double max, Func<int, double, bool> end)
        {
            if (max < min)
            {
                var tmp = max;
                max = min;
                min = tmp;
            }

            if (max == min) return min;
            var y1 = func(min);
            var s1 = Math.Sign(y1);
            if (s1 == 0)
                return min;

            var y2 = func(max);
            var s2 = Math.Sign(y2);
            if (s2 == 0)
                return max;
            if (s1 == s2)
                return null;

            var iteration = 0;
            while (true)
            {
                iteration++;

                var dx = max - min;
                var dy = y2 - y1;
                var m  = dx / dy;

                var x = min + m * (0 - y1);
                var y = func(x);
                var s = Math.Sign(y);
                if (s == 0)
                    return x;
                if (s == s1)
                {
                    min = x;
                    y1  = y;
                }
                else
                {
                    max = x;
                    y2  = y;
                }

                if (end(iteration, y))
                    return x;
            }
        }
    }
}