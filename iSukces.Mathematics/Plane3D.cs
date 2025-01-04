using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

public sealed class Plane3D : ICloneable
{
    public Plane3D(Point3D center, Vector3D versor)
    {
        versor.Normalize();
        A = versor.X;
        B = versor.Y;
        C = versor.Z;
        D = -(center.X * A + center.Y * B + center.Z * C);
    }


    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="a">A</param>
    ///     <param name="b">B</param>
    ///     <param name="c">C</param>
    ///     <param name="d">D</param>
    /// </summary>
    public Plane3D(double a, double b, double c, double d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public static Line3D? Cross(Plane3D a, Plane3D b)
    {
        var v1 = a.Normal;
        var v2 = b.Normal;

        var n = Vector3D.CrossProduct(v1, v2);
        n.Normalize();

        var     x = Math.Abs(n.X);
        var     y = Math.Abs(n.Y);
        var     z = Math.Abs(n.Z);
        Point3D p;
        if (z >= x && z >= y)
        {
            var l1         = new LineEquation(a.A, a.B, a.D);
            var l2         = new LineEquation(b.A, b.B, b.D);
            var ppNullable = LineEquation.Cross(l1, l2);
            if (!ppNullable.HasValue)
                return null;
            var pp = ppNullable.Value;
            p = new Point3D(pp.X, pp.Y, 0);
        }
        else if (y >= x)
        {
            var l1         = new LineEquation(a.A, a.C, a.D);
            var l2         = new LineEquation(b.A, b.C, b.D);
            var ppNullable = LineEquation.Cross(l1, l2);
            if (!ppNullable.HasValue)
                return null;
            var pp = ppNullable.Value;
            p = new Point3D(pp.X, 0, pp.Y);
        }
        else
        {
            var l1         = new LineEquation(a.B, a.C, a.D);
            var l2         = new LineEquation(b.B, b.C, b.D);
            var ppNullable = LineEquation.Cross(l1, l2);
            if (!ppNullable.HasValue)
                return null;
            var pp = ppNullable.Value;
            p = new Point3D(0, pp.X, pp.Y);
        }

        p = a.Throw(p);
        p = b.Throw(p);
        return new Line3D(p, n);
    }
    public static Plane3D operator /(Plane3D srcPlane, Coordinates3D? coordinates)
    {
        return coordinates is null
            ? (Plane3D)srcPlane.MemberwiseClone()
            : srcPlane * coordinates.Reversed;
    }

    public static explicit operator Plane3D(Coordinates3D src)
    {
        return new Plane3D(src.Origin, src.Z);
    }

    public static Plane3D operator *(Plane3D srcPlane, Coordinates3D? coordinates)
    {
        if (coordinates is null)
            return (Plane3D)srcPlane.MemberwiseClone();
        var normal      = new Vector3D(srcPlane.A, srcPlane.B, srcPlane.C);
        var dNormalized = srcPlane.D / normal.Length;
        normal.Normalize();
        var newNormal = normal * coordinates;

        // var center = new Point3D(-dNormalized * normal.X, -dNormalized * normal.Y, -dNormalized * normal.Z);
        // var newCenter = center * coordinates;
        //  (-dNormalized *  normal) * coordinates =  -dNormalized * newCenter -> trzeba tylko dodać origin bo tym się
        // różni mnożenie punktu od mnożenia wektora
        // normal * coordinates
        var newCenter2 = -dNormalized * newNormal + coordinates.Origin;
        return new Plane3D(newCenter2, newNormal);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public Point3D? Cross(Line3D l)
    {
        return Cross(l.Point, l.Vector);
    }

    public Point3D? Cross(Point3D src, Vector3D v)
    {
        var a = A * v.X + B * v.Y + C * v.Z;
        var b = A * src.X + B * src.Y + C * src.Z + D;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (a == 0)
            return null;
        var l  = -b / a;
        var p2 = src + v * l;
        return p2;
    }

    public double DistanceNotNormalized(Point3D p)
    {
        return p.X * A + p.Y * B + p.Z * C + D;
    }

    public void Normalize()
    {
        var l = Math.Sqrt(A * A + B * B + C * C);
        A /= l;
        B /= l;
        C /= l;
        D /= l;
    }

    public Point3D Throw(Point3D p)
    {
        var q = -(A * p.X + B * p.Y + C * p.Z + D) / (A * A + B * B + C * C);
        return new Point3D(p.X + A * q, p.Y + B * q, p.Z + C * q);
    }

    public static Plane3D ZPlane
    {
        get { return new Plane3D(new Point3D(), new Vector3D(0, 0, 1)); }
    }

    /// <summary>
    ///     A
    /// </summary>
    public double A { get; set; }

    /// <summary>
    ///     B
    /// </summary>
    public double B { get; set; }

    /// <summary>
    ///     C
    /// </summary>
    public double C { get; set; }

    /// <summary>
    ///     D
    /// </summary>
    public double D { get; set; }

    /// <summary>
    ///     Własność jest tylko do odczytu.
    /// </summary>
    public Vector3D Normal
    {
        get { return new Vector3D(A, B, C); }
    }
}