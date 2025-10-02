#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif
using System;


namespace iSukces.Mathematics;

public struct PointYZ : IEquatable<PointYZ>
{
    public PointYZ(double y, double z)
    {
        Y = y;
        Z = z;
    }

    public static PointYZ operator +(PointYZ point, VectorYZ vector)
    {
        return new PointYZ(point.Y + vector.Y, point.Y + vector.Z);
    }

    public static PointYZ operator +(VectorYZ vector, PointYZ point)
    {
        return new PointYZ(point.Y + vector.Y, point.Y + vector.Z);
    }

    public static bool operator ==(PointYZ left, PointYZ right)
    {
        return left.Equals(right);
    }

    public static explicit operator PointYZ(Point p)
    {
        return new PointYZ(p.X, p.Y);
    }

    public static bool operator !=(PointYZ left, PointYZ right)
    {
        return !left.Equals(right);
    }

    public static Point3D operator *(PointYZ p, Coordinates3D c)
    {
        return p.To3D() * c;
    }

    public static VectorYZ operator -(PointYZ a, PointYZ b)
    {
        return new VectorYZ(a.Y - b.Y, a.Z - b.Z);
    }

    public Point AsPointXy()
    {
        return new Point(Y, Z);
    }

    public double DistanceTo(PointYZ other)
    {
        return MathEx.PitagorasC(other.Y - Y, other.Z - Z);
    }

    public bool Equals(PointYZ other)
    {
        return Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is PointYZ point && Equals(point);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Y.GetHashCode() * 397) ^ Z.GetHashCode();
        }
    }

    public Point3D To3D(double x = 0)
    {
        return new Point3D(x, Y, Z);
    }

    public override string ToString()
    {
        return $"<*, {Y}, {Z}>";
    }

    public double Y { get; set; }
    public double Z { get; set; }
}

