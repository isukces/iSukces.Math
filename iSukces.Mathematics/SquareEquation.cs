using System;
using System.Text;
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


namespace iSukces.Mathematics;

/// <summary>
/// Rozwiązuje równanie kwadratowe
/// </summary>
public sealed class SquareEquation
{
    public SquareEquation()
    {
    }
    public SquareEquation(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
    }

    public SquareEquation(ThePoint p1, ThePoint p2, ThePoint p3)
    {
        /*
             p1_x·(p2_y - p3_y) + p1_y·(p3_x - p2_x) + p2_x·p3_y - p2_y·p3_x
a = ⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯
           2
      (p1_x  - p1_x·(p2_x + p3_x) + p2_x·p3_x)·(p3_x - p2_x)

     2                                                        2                 2
 p1_x ·(p2_y - p3_y) + p1_y·(p2_x + p3_x)·(p3_x - p2_x) + p2_x ·p3_y - p2_y·p3_x
b = ⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯
                   2
              (p1_x  - p1_x·(p2_x + p3_x) + p2_x·p3_x)·(p2_x - p3_x)

     2                                          2       2
 p1_x ·(p2_x·p3_y - p2_y·p3_x) + p1_x·(p2_y·p3_x  - p2_x ·p3_y) + p1_y·p2_x·p3_x·(p2_x - p3_x)
c = ⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯
                          2
                     (p1_x  - p1_x·(p2_x + p3_x) + p2_x·p3_x)·(p2_x - p3_x)

        a=(p1_x*(p2_y-p3_y)+p1_y*(p3_x-p2_x)+p2_x*p3_y-p2_y*p3_x)/((SQR(p1_x)-p1_x*(p2_x+p3_x)+p2_x*p3_x)*(p3_x-p2_x))
        b=(SQR(p1_x)*(p2_y-p3_y)+p1_y*(p2_x+p3_x)*(p3_x-p2_x)+SQR(p2_x)*p3_y-p2_y*SQR(p3_x))/((SQR(p1_x)-p1_x*(p2_x+p3_x)+p2_x*p3_x)*(p2_x-p3_x))
        c=(SQR(p1_x)*(p2_x*p3_y-p2_y*p3_x)+p1_x*(p2_y*SQR(p3_x)-SQR(p2_x)*p3_y)+p1_y*p2_x*p3_x*(p2_x-p3_x))/((SQR(p1_x)-p1_x*(p2_x+p3_x)+p2_x*p3_x)*(p2_x-p3_x))
    */
        double aaa = MathEx.Sqr(p1.X);
        double bbb = MathEx.Sqr(p2.X);
        double ccc = MathEx.Sqr(p3.X);
        double ma  = (aaa - p1.X * (p2.X + p3.X) + p2.X * p3.X) * (p3.X - p2.X);
        // double mb = (aaa - p1.X * (p2.X + p3.X) + p2.X * p3.X) * (p2.X - p3.X);
        double mb = -ma;
        A = (p1.X * (p2.Y - p3.Y) + p1.Y * (p3.X - p2.X) + p2.X * p3.Y - p2.Y * p3.X) / ma;
        B = (aaa * (p2.Y - p3.Y) + p1.Y * (p2.X + p3.X) * (p3.X - p2.X) + bbb * p3.Y - p2.Y * ccc) / mb;
        C = (aaa * (p2.X * p3.Y - p2.Y * p3.X) + p1.X * (p2.Y * ccc - bbb * p3.Y) + p1.Y * p2.X * p3.X * (p2.X - p3.X)) / mb;
    }

    /// <summary>
    /// współczynnik A
    /// </summary>
    public double A { get; set; }

    /// <summary>
    /// współczynnik B
    /// </summary>
    public double B { get; set; }

    /// <summary>
    /// współczynnik C
    /// </summary>
    public double C { get; set; }

    /// <summary>
    /// Wyznacznik Delta
    /// </summary>
    public double Delta
    {
        get { return B * B - 4 * A * C; }
    }

    /// <summary>
    /// Pochodna funkcji
    /// </summary>
    public LinearFunc Diff
    {
        get { return new LinearFunc(2 * A, B); }
    }

    /// <summary>
    /// Punkt przegięcia paraboli
    /// </summary>
    public ThePoint TopPoint
    {
        get
        {
            double x = -B / (2 * A);
            return new ThePoint(x, Value(x));
        }
    }
    /// <summary>
    /// Rozwiązania - miejsca zerowe
    /// </summary>
    public double[] Solutions
    {
        get
        {
            double d = Delta;
            if (d < 0)
                return [];
            double m = -2 * A;
            if (d == 0)
                return [B / m];
            d = Math.Sqrt(d);
            if (m > 0)
                return [(B - d) / m, (B + d) / m];
            return [(B + d) / m, (B - d) / m];
        }
    }

    /// <summary>
    /// Ilość rozwiązań
    /// </summary>
    public int SolutionCount
    {
        get
        {
            double d = Delta;
            if (d < 0) return 0;
            if (d > 0) return 2;
            return 1;
        }
    }

    /// <summary>
    /// Wartość funkcji kwadratowej
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public double Value(double x)
    {
        return A * x * x + B * x + C;
    }

    public double Integrate(double x1, double x2)
    {
        double y1 = A * x1 * x1 * x1 / 3 + B * x1 * x1 / 2 + C * x1;
        double y2 = A * x2 * x2 * x2 / 3 + B * x2 * x2 / 2 + C * x2;
        return y2 - y1;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (A != 0)
        {
            if (A == 1)
                sb.Append("x^2");
            else if (A == -1)
                sb.Append("-x^2");
            else
                sb.AppendFormat("{0} x^2", A);
        }
        if (B != 0)
        {
            if (B == 1)
            {
                if (sb.Length != 0) sb.Append(" + ");
                sb.Append("x");
            }
            else if (B == -1)
            {
                if (sb.Length != 0)
                    sb.Append(" - ");
                else
                    sb.Append("-");
                sb.Append("x");
            }
            else if (B < 0)
            {
                if (sb.Length != 0)
                    sb.Append(" - ");
                else
                    sb.Append("-");
                sb.AppendFormat("{0} x", -B);
            }
            else
            {
                if (sb.Length != 0)
                    sb.Append(" + ");
                sb.AppendFormat("{0} x", B);
            }
        }
        if (C != 0)
        {
            if (C < 0)
            {
                if (sb.Length != 0)
                    sb.AppendFormat(" - {0}", -C);
                else
                    sb.AppendFormat("{0}", C);
            }
            else
            {
                if (sb.Length != 0)
                    sb.AppendFormat(" + {0}", -C);
                else
                    sb.AppendFormat("{0}", C);
            }
        }
        if (sb.Length == 0)
            sb.Append("0");
        sb.Append(" = 0");
        return sb.ToString();
    }

    public static double[] Solve(double a, double b, double c)
    {
        return new SquareEquation(a, b, c).Solutions;
    }
}