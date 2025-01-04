using System;

namespace iSukces.Mathematics;

public static class DoubleExtension
{
    public static int Ceil(this double x, int round)
    {
        return (int)Math.Ceiling(x / round) * round;
    }
        
    /// <summary>
    /// Zwraca liczbę, która nie wykracza poza zakres min-max 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double MinMax(this double x, double min, double max)
    {

        if (x < min) return min;
        if (x > max) return max;
        return x;
    }

    public static int Floor(this double x)
    {
        return (int)Math.Floor(x);
    }

    public static double Sqr(this double x)
    {
        return x * x;
    }

    public static bool BetweenExclusive(this double x, double a, double b)
    {
        return x > a && x < b;
    }

    public static bool BetweenInclusive(this double x, double a, double b)
    {
        return x >= a && x <= b;
    }

    public static bool NotEquals(this double x, double y)
    {
        return x != y;
    }

    public static bool NotEqualsZero(this double x)
    {
        return x != 0.0;
    }


    public static int Round(this double x)
    {
        return (int)Math.Round(x);
    }
        
    public static double Abs(this double x)
    {
        return Math.Abs(x);
    }

    public static int RoundUp(this int x, int round)
    {
        x =  x + round - 1;
        x /= round;
        x *= round;
        return x;
    }

    public static double Round(this double x, double round)
    {
        return Math.Round(x / round) * round;
    }

    public static int Round10(this double x)
    {
        return 10 * (int)Math.Round(x / 10);
    }

    public static int RoundUP10(this double x)
    {
        return 10 * (int)Math.Ceiling(x / 10);
    }

    public static int RoundUPxx(this double x, int xx = 1)
    {
        if (xx == 1)
            return (int)Math.Ceiling(x);
        return xx * (int)Math.Ceiling(x / xx);
    }

    public static int RoundUP5(this double x)
    {
        return 5 * (int)Math.Ceiling(x / 5);
    }

    public static int RoundDOWN10(this double x)
    {
        return 10 * (int)Math.Floor(x / 10);
    }
    public static int RoundDOWN2(this double x)
    {
        return 2 * (int)Math.Floor(x / 2);
    }
    public static int RoundDOWN5(this double x)
    {
        return 5 * (int)Math.Floor(x / 5);
    }
}