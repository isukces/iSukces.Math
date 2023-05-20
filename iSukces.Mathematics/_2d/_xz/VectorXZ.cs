using System;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows.Media.Media3D;
using System.Windows;
#endif


namespace iSukces.Mathematics;

public struct VectorXZ : IEquatable<VectorXZ>
{
    public VectorXZ(double x, double z)
    {
        X = x;
        Z = z;
    }

    public static double CrossProduct(VectorXZ v1, VectorXZ v2)
    {
        // bida implementacja
        return Vector.CrossProduct((Vector)v1, (Vector)v2);
    }

    public static double DotProduct(VectorXZ a, VectorXZ b)
    {
        return a.X * b.X + a.Z * b.Z;
    }

    public static VectorXZ operator +(VectorXZ left, VectorXZ right)
    {
        return new VectorXZ(left.X + right.X, left.Z + right.Z);
    }

    public static bool operator ==(VectorXZ left, VectorXZ right)
    {
        return left.Equals(right);
    }

    public static explicit operator Vector(VectorXZ source)
    {
        return new Vector(source.X, source.Z);
    }

    public static bool operator !=(VectorXZ left, VectorXZ right)
    {
        return !left.Equals(right);
    }

    public static VectorXZ operator *(VectorXZ left, double right)
    {
        return new VectorXZ(left.X * right, left.Z * right);
    }

    public static VectorXZ operator *(double left, VectorXZ right)
    {
        return new VectorXZ(left * right.X, left * right.Z);
    }

    public static VectorXZ operator -(VectorXZ left, VectorXZ right)
    {
        return new VectorXZ(left.X - right.X, left.Z - right.Z);
    }

    public static VectorXZ operator -(VectorXZ left)
    {
        return new VectorXZ(-left.X, -left.Z);
    }


    public double DotProduct(VectorXZ other)
    {
        return DotProduct(this, other);
    }

    public bool Equals(VectorXZ other)
    {
        return X.Equals(other.X) && Z.Equals(other.Z);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is VectorXZ xz && Equals(xz);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (X.GetHashCode() * 397) ^ Z.GetHashCode();
        }
    }

    public VectorXZ GetNormalized()
    {
        var l = Length;
        return new VectorXZ(X / l, Z / l);
    }

    public VectorXZ GetRot90()
    {
        return new VectorXZ(-Z, X);
    }

    public override string ToString()
    {
        return $"<*, {X}, {Z}>";
    }

    public Vector3D ToVector3D()
    {
        return new Vector3D(X, 0, Z);
    }

    public VectorXZ WithMinusX()
    {
        return new VectorXZ(-X, Z);
    }

    #region Properties

    public double X      { get; }
    public double Z      { get; }
    public double Length => Math.Sqrt(X * X + Z * Z);

    #endregion
}
