using System;
using System.Text;
#if !WPFFEATURES
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics;

public sealed class LineEquation : ICloneable
{
    /// <summary>
    ///     Tworzy instancję obiektu
    /// </summary>
    public LineEquation()
    {
    }

    /// <summary>
    ///     Tworzy linię przechodzącą przez punkt a i b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public LineEquation(ThePoint a, ThePoint b)
        :
        this(a.X, a.Y, b.X, b.Y)
    {
        Normalize();
    }

    /// <summary>
    ///     Tworzy linię o wskazanym kącie oraz Y0
    /// </summary>
    /// <param name="tangent"></param>
    /// <param name="y0"></param>
    public LineEquation(double tangent, double y0)
    {
        A = tangent;
        B = -1;
        C = y0;
        Normalize();
    }

    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="a">wsp. A</param>
    ///     <param name="b">wsp. B</param>
    ///     <param name="c">wsp. C</param>
    /// </summary>
    public LineEquation(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
        Normalize();
    }

    public LineEquation(double x1, double y1, double x2, double y2)
    {
        var dx = x2 - x1;
        var dy = y2 - y1;
        var l  = Math.Sqrt(dx * dx + dy * dy);
        if (l > 0)
        {
            A = -dy / l;
            B = dx / l;
        }

        if (A < 0)
        {
            A = -A;
            B = -B;
        }

        C = -(A * x1 + B * y1);
    }

    public static ThePoint? Cross(LineEquation p1, LineEquation p2)
    {
        if (p1 is null) throw new ArgumentNullException(nameof(p1));
        if (p2 is null) throw new ArgumentNullException(nameof(p2));
        // źródło http://www.math.edu.pl/punkt-przeciecia-dwoch-prostych
        if (p1.IsInvalid || p2.IsInvalid)
            return null;

        var s = new EquationSystem2
        {
            A1 = p1.A,
            B1 = p1.B,
            C1 = p1.C,
            A2 = p2.A,
            B2 = p2.B,
            C2 = p2.C
        };
        var solution = s.Solution;
        return solution;
    }

    /// <summary>
    ///     Punkt przecięcia odcinków
    /// </summary>
    public static ThePoint? CrossLineSegment(ThePoint p1Begin, ThePoint p1End, ThePoint p2Begin, ThePoint p2End)
    {
        var line1              = new LineEquation(p1Begin, p1End);
        var line2              = new LineEquation(p2Begin, p2End);
        var crossPointNullable = Cross(line1, line2);
        if (crossPointNullable is null) return null;
        var cross = crossPointNullable.Value;

        bool Test(ThePoint a, ThePoint b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (dx > 0)
                {
                    if (cross.X < a.X || cross.X > b.X) return false;
                }
                else
                {
                    if (cross.X > a.X || cross.X < b.X) return false;
                }
            }
            else
            {
                if (dy > 0)
                {
                    if (cross.Y < a.Y || cross.Y > b.Y) return false;
                }
                else
                {
                    if (cross.Y > a.Y || cross.Y < b.Y) return false;
                }
            }

            return true;
        }

        if (Test(p1Begin, p1End) && Test(p2Begin, p2End))
            return cross;
        return null;
    }

    public static LineEquation FromPointAndDeltas(double x, double y, double dx, double dy)
    {
        //double a = -dy;
        // double b = dx;
        // LineEquation r = new LineEquation(-dy, dx, -a * x - b * y);
        var r = new LineEquation(-dy, dx, dy * x - dx * y);
        return r;
    }

    public static LineEquation Make(ThePoint p1, ThePoint p2)
    {
        return FromPointAndDeltas(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
    }

    public static LineEquation Make(ThePoint p, TheVector v)
    {
        return FromPointAndDeltas(p.X, p.Y, v.X, v.Y);
    }

    public static explicit operator LineEquation(LinearFunc a)
    {
        return new LineEquation(a.A, -1, a.B);
    }


    public static LineEquation operator -(LineEquation a)
    {
        return new LineEquation(-a.A, -a.B, -a.C);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public ThePoint? Cross(LineEquation other)
    {
        return Cross(this, other);
    }

    public double DistanceNotNormalized(ThePoint p)
    {
        return A * p.X + B * p.Y + C;
    }

    public double DistanceNotNormalized(double x, double y)
    {
        return A * x + B * y + C;
    }

    /// <summary>
    ///     Zwraca współrzędną X dla podanego Y - jeśli linia pozioma (dy=a=0) to będzie NaN
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public double GetX(double y)
    {
        if (A == 0)
            return double.NaN;
        return -(B * y + C) / A;
    }

    public ThePoint GetXPoint(double y)
    {
        return new ThePoint(GetX(y), y);
    }

    /// <summary>
    ///     Zwraca współrzędną Y dla podanego X - jeśli linia pionowa (dx=b=0) to będzie NaN
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public double GetY(double x)
    {
        if (B == 0)
            return double.NaN;
        return -(A * x + C) / B;
    }

    /// <summary>
    ///     Zwraca współrzędną Y dla podanego X - jeśli linia pionowa (dx=b=0) to będzie NaN
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public ThePoint GetYPoint(double x)
    {
        return new ThePoint(x, GetY(x));
    }

    public void Normalize()
    {
        var l = Math.Sqrt(A * A + B * B);
        if (l > 0 && l != 1)
        {
            A /= l;
            B /= l;
            C /= l;
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (B == 0)
        {
            if (A == 0) return "0 = " + C;
            return "x = " + -C / A;
        }

        if (A == 0)
        {
            return "y = " + -C / B;
        }

        sb.Append($"{A} * x");
        if (B > 0)
            sb.Append(" + ");
        else
            sb.Append(" - ");
        sb.Append($"{Math.Abs(B)} * y");

        if (C != 0)
            if (sb.Length > 0)
            {
                if (C > 0)
                    sb.Append(" + ");
                else
                    sb.Append(" - ");
                sb.Append($"{Math.Abs(C)}");
            }
            else
            {
                sb.Append($"{C}");
            }

        sb.Append(" = 0");
        return "Line: " + sb;
    }

    #region Properties

    public bool MoreVertical => Math.Abs(A) > Math.Abs(B);

    /// <summary>
    ///     wsp. A
    /// </summary>
    public double A { get; set; }

    /// <summary>
    ///     Własność jest tylko do odczytu.
    /// </summary>
    public double AngleDeg => Math.Atan2(-B, A) * (180.0 / Math.PI);

    /// <summary>
    ///     wsp. B
    /// </summary>
    public double B { get; set; }

    /// <summary>
    ///     wsp. C
    /// </summary>
    public double C { get; set; }

    /// <summary>
    ///     Własność jest tylko do odczytu.
    /// </summary>
    public bool IsInvalid => A == 0 && B == 0;

    /// <summary>
    ///     Punkt zerowy - taki X dla którego Y=0
    /// </summary>
    public double ZeroPoint
    {
        get
        {
            if (A == 0)
                return double.NaN;
            // a x + c = 0
            return -C / A;
        }
    }

    #endregion
}
