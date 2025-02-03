#if !WPFFEATURES
using ThePoint=iSukces.Mathematics.Compatibility.Point;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint=System.Windows.Point;
using TheVector=System.Windows.Vector;
#endif
using System;
using System.Linq;
using iSukces.Helix;


namespace iSukces.Mathematics;

public class CircleOnPlane : Circle, ICloneable
{
    /// <summary>
    /// Tworzy instancję obiektu
    /// </summary>
    public CircleOnPlane()
    {
    }

    /// <summary>
    /// Tworzy instancję obiektu na podstawie promienia
    /// </summary>
    /// <param name="radius">promień</param>
    public CircleOnPlane(double radius)
        : base(radius)
    {
    }

    /// <summary>
    /// Tworzy instancję obiektu na podstawie promienia i współrzędnych środka
    /// </summary>
    /// <param name="radius">promień</param>
    /// <param name="center">środek</param>
    public CircleOnPlane(double radius, ThePoint center)
        : base(radius)
    {
        Center = center;
    }

    /// <summary>
    /// Tworzy instancję obiektu na podstawie środka i dowolnego punktu na obwodzie
    /// </summary>
    /// <param name="center">środek</param>
    /// <param name="anyPointOnPlane">dowolny punkt na obwodzie</param>
    public CircleOnPlane(ThePoint center, ThePoint anyPointOnPlane)
        : this((center - anyPointOnPlane).Length, center)
    {
    }


    /// <summary>
    /// Oblicza punkty przecięcia okręgu z linią (nieskończona długość) przechodzącą przez 2 punkty
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public ThePoint[] CrossLine(ThePoint a, ThePoint b)
    {
        ThePoint[]   result;
        LineEquation le = LineEquation.FromPointAndDeltas(a.X - Center.X, a.Y - Center.Y, b.X - a.X, b.Y - a.Y);
        if (Math.Abs(le.A) > Math.Abs(le.B))
        {
            // pionowa
            SquareEquation se = new SquareEquation(
                1 + MathEx.Sqr(le.B / le.A),
                -2 * le.B * le.C / le.A,
                MathEx.Sqr(le.C / le.A) - MathEx.Sqr(_radius)
            );
            var tmp = se.Solutions;
            result = new ThePoint[tmp.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = new ThePoint(le.GetX(tmp[i]) + Center.X, tmp[i] + Center.Y);

        }
        else
        {
            // pozioma
            SquareEquation se = new SquareEquation(
                1 + MathEx.Sqr(le.A / le.B),
                -2 * le.A * le.C / le.B,
                MathEx.Sqr(le.C / le.B) - MathEx.Sqr(_radius)
            );
            var tmp = se.Solutions;
            result = new ThePoint[tmp.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = new ThePoint(tmp[i] + Center.X, le.GetY(tmp[i]) + Center.Y);

        }
        return result;
    }


    /// <summary>
    /// Realizuje operator !=
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są różne</returns>
    public static bool operator !=(CircleOnPlane? left, CircleOnPlane? right)
    {
        var eq = left == right;
        return !eq;
    }

    /// <summary>
    /// Realizuje operator ==
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są równe</returns>
    public static bool operator ==(CircleOnPlane? left, CircleOnPlane? right)
    {
        if (left == (object?)null && right == (object?)null) return true;
        if (left == (object?)null || right == (object?)null) return false;
        return left.Center == right.Center && left._radius == right._radius;
    }


    /// <summary>
    /// Wykonuje kopię obiektu
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        return new CircleOnPlane(_radius, Center);
    }

    /// <summary>
    /// Sprawdza, czy wskazany obiekt jest równy bieżącemu
    /// </summary>
    /// <param name="obj">obiekt do porównania z obiektem bieżącym</param>
    /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
    public override bool Equals(object? obj)
    {
        if (obj is CircleOnPlane) return (CircleOnPlane)obj == this;
        return false;
    }

    /// <summary>
    /// Zwraca kod HASH obiektu
    /// </summary>
    /// <returns>kod HASH obiektu</returns>
    public override int GetHashCode()
    {
        return Center.GetHashCode() ^ _radius.GetHashCode();
    }

    public override string ToString()
    {
        return string.Format("Circle {0}, radius {1}", Center, Radius);
    }
        

    /// <summary>
    /// Środek koła
    /// </summary>
    public ThePoint Center { get; protected set; }
    
    
    public CircleCrossInfo GetCrossPoints(CircleOnPlane c)
    {
        if (this == c)
            return new CircleCrossInfo(CircleLocations.Full);

        var rSum = _radius + c._radius;
        var v    = c.Center - Center;
        var odl  = v.Length;

        if (odl > rSum)
            return new CircleCrossInfo(CircleLocations.NoCommonPoints);

        if (odl < Math.Abs(_radius - c._radius))
        {
            if (_radius > c._radius)
                return new CircleCrossInfo(CircleLocations.OtherCircleInside);
            return new CircleCrossInfo(CircleLocations.InsideOtherCircle);
        }

        if (odl == rSum)
        {
            var p = Center + v * (_radius / rSum);
            return new CircleCrossInfo(CircleLocations.Outside1Point, p);
        }

        if (odl == _radius - c._radius)
        {
            v.Normalize();
            var p = Center + v * _radius;
            return new CircleCrossInfo(CircleLocations.OtherCircleInside1Point, p);
        }

        if (odl == c._radius - _radius)
        {
            v.Normalize();
            var p = Center - v * _radius;
            return new CircleCrossInfo(CircleLocations.InsideOtherCircle, p);
        }

        var triangle = new Triangle
        {
            A = _radius,
            B = c._radius,
            C = odl
        };
        var hc            = triangle.HC;
        var d1_length     = Triangle.ComputeSide(_radius, hc);
        var v_d1          = v.SetLength(d1_length);
        var v_hc          = v_d1.GetPrependicular().SetLength(hc);
        var segmentVector = v_d1 + v_hc;

        var info = new CircleCrossInfo(CircleLocations.Other)
        {
            Points =
            [
                Center + v_d1 + v_hc,
                Center + v_d1 - v_hc
            ]
        };
        return info;
    }

    /// <summary>
    ///     Zwraca tablicę punktów przecięcia półprostej z okręgiem
    /// </summary>
    /// <param name="ray">promień (półprosta)</param>
    /// <returns>tablica punktów przecięcia</returns>
    public ThePoint[] GetCrossPoints(Ray2D ray)
    {
        var ray_StartPoint = ray.BeginPoint;
        var ray_Versor     = ray.Axis;
        var dx             = Center.X - ray_StartPoint.X;
        var dy             = Center.Y - ray_StartPoint.Y;

        var e = new SquareEquation
        {
            A = 1, // ray.Versor.Length ^ 2
            B = -2 * (ray_Versor.X * dx + ray_Versor.Y * dy),
            C = dx * dx + dy * dy - _radius * _radius
        };
        return
        (
            from x in e.Solutions
            where x >= 0
            select ray.GetPoint(x)
        ).ToArray();
    }
        
}

