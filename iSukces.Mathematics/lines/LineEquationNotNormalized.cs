using System;
using System.Text;
#if !WPFFEATURES
using ThePoint = iSukces.Mathematics.Point;
using TheVector = iSukces.Mathematics.Vector;

#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics;

using Self = LineEquationNotNormalized;

/// <summary>
/// Reprezentuje równanie prostej dwuwymiarowej w nieznormalizowanej postaci ogólnej (Ax + By + C = 0).
/// </summary>
public sealed class LineEquationNotNormalized : ICloneable
{
    /// <summary>
    ///     Tworzy instancję obiektu
    /// </summary>
    public LineEquationNotNormalized()
    {
    }

    /// <summary>
    ///     Tworzy linię przechodzącą przez punkt a i b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public LineEquationNotNormalized(ThePoint a, ThePoint b)
        :
        this(a.X, a.Y, b.X, b.Y)
    {
    }

    /// <summary>
    ///     Tworzy linię o wskazanym kącie oraz Y0
    /// </summary>
    /// <param name="tangent"></param>
    /// <param name="y0"></param>
    public LineEquationNotNormalized(double tangent, double y0)
    {
        A = tangent;
        B = -1;
        C = y0;
    }

    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="a">wsp. A</param>
    ///     <param name="b">wsp. B</param>
    ///     <param name="c">wsp. C</param>
    /// </summary>
    public LineEquationNotNormalized(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
    }

    public LineEquationNotNormalized(double x1, double y1, double x2, double y2)
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

    public static ThePoint? Cross(Self p1, Self p2)
    {
        // źródło http://www.math.edu.pl/punkt-przeciecia-dwoch-prostych
        if (p1 is null)
            throw new ArgumentNullException(nameof(p1));
        if (p2 is null)
            throw new ArgumentNullException(nameof(p2));
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
    public static ThePoint? CrossLineSegment(ThePoint p1, ThePoint p2, ThePoint p3, ThePoint p4)
    {
        var line1 = new Self(p1, p2);
        var line2 = new Self(p3, p4);
        var c     = Cross(line1, line2);
        if (c is null) return null;
        var cc = c.Value;
        Func<ThePoint, ThePoint, bool> test = (a, b) =>
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (dx > 0)
                {
                    if (cc.X < a.X || cc.X > b.X) return false;
                }
                else
                {
                    if (cc.X > a.X || cc.X < b.X) return false;
                }
            }
            else
            {
                if (dy > 0)
                {
                    if (cc.Y < a.Y || cc.Y > b.Y) return false;
                }
                else
                {
                    if (cc.Y > a.Y || cc.Y < b.Y) return false;
                }
            }

            return true;
        };
        if (test(p1, p2) && test(p3, p4)) return cc;
        return null;
    }

    public static Self From2Points(ThePoint p1, ThePoint p2)
    {
        return FromPointAndDeltas(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
    }

    public static Self FromPointAndDeltas(double x, double y, double dx, double dy)
    {
        //double a = -dy;
        // double b = dx;
        // LineEquation2 r = new LineEquation2(-dy, dx, -a * x - b * y);
        var r = new Self(-dy, dx, dy * x - dx * y);
        return r;
    }

    public static Self FromPointAndDeltas(ThePoint x, TheVector v)
    {
        var dx = v.X;
        var dy = v.Y;
        var r  = new Self(-dy, dx, dy * x.X - dx * x.Y);
        return r;
    }

    public static Self Horizontal(double y)
    {
        return new Self(0, -1, y);
    }

    public static explicit operator Self(LinearFunc a)
    {
        return new Self(a.A, -1, a.B);
    }

    public static implicit operator LineEquationNotNormalized(LineEquation x)
    {
        return new LineEquationNotNormalized(x.A, x.B, x.C);
    }


    public static Self operator -(Self a)
    {
        return new Self(-a.A, -a.B, -a.C);
    }

    public static Self Vertical(double x)
    {
        return new Self(-1, 0, x);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public ThePoint? CrossWith(Self line)
    {
        return Cross(this, line);
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
    ///     Zwraca współrzędną Y dla podanego X-jeśli linia pionowa (dx=b=0) to będzie NaN
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
    ///     Zwraca współrzędną Y dla podanego X-jeśli linia pionowa (dx=b=0) to będzie NaN
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public ThePoint GetYPoint(double x)
    {
        return new ThePoint(x, GetY(x));
    }

    public override string ToString()
    {
        var aSer = A.ToString();
        var bSer = B.ToString();
        if (aSer == "0")
        {
            if (bSer == "0")
                return "Invalid C=" + C;
            return "y=" + -C / B;
        }

        if (bSer == "0")
            return "x=" + -C / A;

        var sb = new StringBuilder();

        sb.AppendFormat("{0} * x", A);
        if (sb.Length > 0)
        {
            if (B > 0)
                sb.Append(" + ");
            else
                sb.Append(" - ");
            sb.AppendFormat("{0} * y", Math.Abs(B));
        }
        else
        {
            sb.AppendFormat("{0} * y", B);
        }

        var cSer = C.ToString();
        if (cSer != "0")
            if (sb.Length > 0)
            {
                sb.Append(C > 0 ? " + " : " - ");
                sb.AppendFormat("{0}", Math.Abs(C));
            }
            else
            {
                sb.AppendFormat("{0}", C);
            }

        sb.Append(" = 0");
        return "Line: " + sb;
    }

    public bool MoreVertical => Math.Abs(A) > Math.Abs(B);


    /// <summary>
    ///     Własność jest tylko do odczytu.
    /// </summary>
    public double AngleDeg => Math.Atan2(-B, A) * (180.0 / Math.PI);

    /// <summary>
    ///     wsp. A
    /// </summary>
    public double A
    {
        get => _a;
        set => _a = value;
    }

    /// <summary>
    ///     wsp. B
    /// </summary>
    public double B
    {
        get => _b;
        set => _b = value;
    }

    /// <summary>
    ///     wsp. C
    /// </summary>
    public double C { get; set; }

    /// <summary>
    ///     Własność jest tylko do odczytu.
    /// </summary>
    public bool IsInvalid => A == 0 && B == 0;

    /// <summary>
    ///     Punkt zerowy-taki X, dla którego Y=0
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

    private double _a;
    private double _b;
}
