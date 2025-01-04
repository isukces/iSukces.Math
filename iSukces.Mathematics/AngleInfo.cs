#if !WPFFEATURES
using TheVector = iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif
using System;

namespace iSukces.Mathematics;

/// <summary>
///     Informacja o kącie
/// </summary>
public sealed class AngleInfo : object
{
    public AngleInfo(double degrees)
    {
        _degrees = degrees;
        _radians = degrees * DEGTORAD;
        Update();
    }

    public static AngleInfo FromAngleNr(int i, int nr)
    {
        var r = new AngleInfo(360.0 * i / nr);
        if (i == 0 || i == nr)
        {
            r.Sin = 0;
            r.Cos = 1;
            r.Tan = 0;
        }

        return r;
    }


    public static AngleInfo FromTan(double y, double x)
    {
        return new AngleInfo(MathEx.Atan2Deg(y, x));
    }

    public static bool operator ==(AngleInfo? firstAngle, AngleInfo? secondAngle)
    {
        if (firstAngle == (object?)null && secondAngle == (object?)null) return true;
        if (firstAngle == (object?)null || secondAngle == (object?)null) return false;
        return firstAngle._degrees == secondAngle._degrees;
    }

    public static implicit operator AngleInfo(double x)
    {
        return new AngleInfo(x);
    }

    public static implicit operator double(AngleInfo x)
    {
        return x._degrees;
    }

    public static bool operator !=(AngleInfo? x, AngleInfo? y)
    {
        if (x == (object?)null && y == (object?)null) return false;
        if (x == (object?)null || y == (object?)null) return true;
        return x._degrees != y._degrees;
    }


    public static AngleInfo operator *(AngleInfo x, double y)
    {
        return new AngleInfo(x.Degrees * y);
    }

    public static AngleInfo operator -(double x, AngleInfo y)
    {
        return new AngleInfo(x - y._degrees);
    }

    public static AngleInfo operator -(int x, AngleInfo y)
    {
        return new AngleInfo(x - y._degrees);
    }

    public static AngleInfo Substract(double x, AngleInfo y)
    {
        return new AngleInfo(x - y._degrees);
    }

    public override bool Equals(object? obj)
    {
        var a = obj as AngleInfo;
        if (a is null) return false;
        return a == this;
    }

    public override int GetHashCode()
    {
        return _degrees.GetHashCode();
    }

    public override string ToString()
    {
        return string.Format("AngleInfo: {0}º, sin={1}, cos={2}", Degrees, Sin, Cos);
    }

    public T Transform<T>(Func<AngleInfo, T> x)
    {
        return x(this);
    }


    private void Update()
    {
        Sin = Math.Sin(_radians);
        Cos = Math.Cos(_radians);
        if (Sin == 1 || Sin == -1)
            Cos = 0;
        if (Cos == 1 || Cos == -1)
            Sin = 0;
        // tan = Math.Tan(radians);
        // ctan = 1 / tan;
        CTan = Cos / Sin;
        Tan  = Sin / Cos;
    }

    /// <summary>
    ///     Wartość cos
    /// </summary>
    public double Cos { get; private set; }

    /// <summary>
    ///     Wartość tan
    /// </summary>
    public double CTan { get; private set; }

    /// <summary>
    ///     Wartość w stopniach
    /// </summary>
    public double Degrees
    {
        get { return _degrees; }
        set
        {
            if (_degrees == value) return;
            _degrees = value;
            _radians = value * DEGTORAD;
            Update();
        }
    }

    /// <summary>
    ///     Wartość w radianach
    /// </summary>
    public double Radians
    {
        get { return _radians; }
        set
        {
            if (_radians == value) return;
            _radians = value;
            _degrees = value * RADTODEG;
            Update();
        }
    }

    /// <summary>
    ///     Wartość sin
    /// </summary>
    public double Sin { get; private set; }

    /// <summary>
    ///     Wartość tan
    /// </summary>
    public double Tan { get; private set; }

    /// <summary>
    ///     Wektro [-sin, cos]
    /// </summary>
    public TheVector V_MinusSin_Cos
    {
        get { return new TheVector(-Sin, Cos); }
    }

    /// <summary>
    ///     Wersor [ cos, sin ]
    /// </summary>
    public TheVector VectorX
    {
        get { return new TheVector(Cos, Sin); }
    }

    /// <summary>
    ///     Wersor [ -sin, cos ]
    /// </summary>
    public TheVector VectorY
    {
        get { return new TheVector(-Sin, Cos); }
    }

    private double _degrees;
    private double _radians;

    public const double DEGTORAD = 0.0174532925199432957692369076848861271344287188854172545609719;
    public const double RADTODEG = 57.2957795130823208767981548141051703324054724665643215491602;
}