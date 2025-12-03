using System;

namespace iSukces.Mathematics;

public static class DoubleExtensions
{
    /// <param name="x"></param>
    extension(double x)
    {
        public double Abs()
        {
            return Math.Abs(x);
        }

        public bool BetweenExclusive(double a, double b)
        {
            return x > a && x < b;
        }

        public bool BetweenInclusive(double a, double b)
        {
            return x >= a && x <= b;
        }

        public int Ceil()
        {
            return (int)Math.Ceiling(x);
        }

        public int Ceil(int round)
        {
            return (int)Math.Ceiling(x / round) * round;
        }

        public int Floor()
        {
            return (int)Math.Floor(x);
        }

        /// <summary>
        ///     Zwraca liczbę, która nie wykracza poza zakres min-max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public double MinMax(double min, double max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }

        public bool NotEquals(double y)
        {
            return x != y;
        }

        public bool NotEqualsZero()
        {
            return x != 0.0;
        }

        public int Round()
        {
            return (int)Math.Round(x);
        }

        public double Round(double round)
        {
            return Math.Round(x / round) * round;
        }

        public int Round10()
        {
            return 10 * (int)Math.Round(x / 10);
        }

        /// <summary>
        ///     Zaokrągla w dół do pełnej wielokrotności
        /// </summary>
        /// <param name="round">zaokrąglenie</param>
        /// <returns></returns>
        public int RoundDown(int round)
        {
            return round * (int)Math.Floor(x / round);
        }

        public int RoundDOWN10()
        {
            return 10 * (int)Math.Floor(x / 10);
        }

        public int RoundDOWN2()
        {
            return 2 * (int)Math.Floor(x / 2);
        }

        public int RoundDOWN5()
        {
            return 5 * (int)Math.Floor(x / 5);
        }

        public int RoundUP10()
        {
            return 10 * (int)Math.Ceiling(x / 10);
        }

        public int RoundUP5()
        {
            return 5 * (int)Math.Ceiling(x / 5);
        }

        public int RoundUPxx(int xx = 1)
        {
            if (xx == 1)
                return (int)Math.Ceiling(x);
            return xx * (int)Math.Ceiling(x / xx);
        }

        public double Sqr()
        {
            return x * x;
        }
    }
}
