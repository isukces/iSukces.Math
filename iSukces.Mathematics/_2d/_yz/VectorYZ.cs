using System;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows.Media.Media3D;
using System.Windows;
#endif

namespace iSukces.Mathematics;

public struct VectorYZ : IEquatable<VectorYZ>
{
    public VectorYZ(double y, double z)
    {
        Y = y;
        Z = z;
    }

    public static double CrossProduct(VectorYZ v1, VectorYZ v2)
    {
        return Vector.CrossProduct((Vector)v1, (Vector)v2);
    }

    public static double DotProduct(VectorYZ a, VectorYZ b)
    {
        return a.Y * b.Y + a.Z * b.Z;
    }

    public static bool operator ==(VectorYZ left, VectorYZ right)
    {
        return left.Equals(right);
    }

    public static explicit operator Vector(VectorYZ source)
    {
        return new Vector(source.Y, source.Z);
    }

    public static bool operator !=(VectorYZ left, VectorYZ right)
    {
        return !left.Equals(right);
    }

    public bool Equals(VectorYZ other)
    {
        return Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is VectorYZ yz && Equals(yz);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Y.GetHashCode() * 397) ^ Z.GetHashCode();
        }
    }

    public VectorYZ GetNormalized()
    {
        var l = Length;
        return new VectorYZ(Y / l, Z / l);
    }

    public override string ToString()
    {
        return $"<*, {Y}, {Z}>";
    }

    public static VectorYZ operator -(VectorYZ left, VectorYZ right)
    {
        return new VectorYZ(left.Y - right.Y, left.Z - right.Z);
    }

    public static VectorYZ operator -(VectorYZ left)
    {
        return new VectorYZ(-left.Y, -left.Z);
    }

    public static VectorYZ operator +(VectorYZ left, VectorYZ right)
    {
        return new VectorYZ(left.Y + right.Y, left.Z + right.Z);
    }

    public Vector3D ToVector3D()
    {
        return new Vector3D(0, Y, Z);
    }

    public Vector3D ToVector3D(double x)
    {
        return new Vector3D(x, Y, Z);
    }

    public double Y      { get; set; }
    public double Z      { get; set; }
    public double Length => Math.Sqrt(Y * Y + Z * Z);
}

