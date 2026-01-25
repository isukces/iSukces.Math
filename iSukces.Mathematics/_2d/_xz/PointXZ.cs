#if !WPFFEATURES
#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif
using System;


namespace iSukces.Mathematics;

// ReSharper disable once InconsistentNaming
public readonly struct PointXZ : IEquatable<PointXZ>
{
    public PointXZ(double x, double z)
    {
        X = x;
        Z = z;
    }
 
    public static PointXZ operator +(PointXZ point, VectorXZ vector)
    {
        return new PointXZ(point.X + vector.X, point.Z + vector.Z);
    }

    public static PointXZ operator +(VectorXZ vector, PointXZ point)
    {
        return new PointXZ(point.X + vector.X, point.Z + vector.Z);
    }

    public static bool operator ==(PointXZ left, PointXZ right)
    {
        return left.Equals(right);
    }

    public static explicit operator PointXZ(Point p)
    {
        return new PointXZ(p.X, p.Y);
    }

    public static bool operator !=(PointXZ left, PointXZ right)
    {
        return !left.Equals(right);
    }


    public static Point3D operator *(PointXZ p, Coordinates3D c)
    {
        return p.ToPoint3D() * c;
    }

    // minus operator
    public static VectorXZ operator -(PointXZ left, PointXZ right)
    {
        return new VectorXZ(left.X - right.X, left.Z - right.Z);
    }

    // minus operator
    public static PointXZ operator -(PointXZ left, VectorXZ right)
    {
        return new PointXZ(left.X - right.X, left.Z - right.Z);
    }

    public Point AsPointXy()
    {
        return new Point(X, Z);
    }


    public double DistanceTo(PointXZ other)
    {
        return MathEx.PitagorasC(other.X - X, other.Z - Z);
    }

    public bool Equals(PointXZ other)
    {
        return X.Equals(other.X) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        return obj is PointXZ other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (X.GetHashCode() * 397) ^ Z.GetHashCode();
        }
    }

    public Point3D ToPoint3D(double y = 0)
    {
        return new Point3D(X, y, Z);
    }

    public override string ToString()
    {
        return X + "; " + Z;
    }

    public PointXZ WithZ(double z)
    {
        return new PointXZ(X, z);
    }

    public double X { get; }
    public double Z { get; }
}

