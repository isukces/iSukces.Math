#if !WPFFEATURES
using System;

namespace iSukces.Mathematics.Compatibility
{
    public struct Vector3D : IEquatable<Vector3D>
    {
        public Vector3D(double x, double y, double z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3D CrossProduct(Vector3D a, Vector3D b)
        {
            return new Vector3D(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        public static Vector3D operator +(Vector3D a, Vector3D f)
        {
            return new Vector3D(a.X + f.X, a.Y + f.Y, a.Z + f.Z);
        }

        public static bool operator ==(Vector3D a, Vector3D b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static explicit operator Vector3D(Point3D x)
        {
            return new Vector3D(x.X, x.Y, x.Z);
        }

        public static bool operator !=(Vector3D a, Vector3D b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static Vector3D operator *(Vector3D a, double f)
        {
            return new Vector3D(a.X * f, a.Y * f, a.Z * f);
        }


        public static Vector3D operator *(double f, Vector3D a)
        {
            return new Vector3D(a.X * f, a.Y * f, a.Z * f);
        }
        
        public static Vector3D operator /(Vector3D a, double f)
        {
            return new Vector3D(a.X / f, a.Y / f, a.Z / f);
        }


        public static Vector3D operator -(Vector3D a, Vector3D f)
        {
            return new Vector3D(a.X - f.X, a.Y - f.Y, a.Z - f.Z);
        }

        public static Vector3D operator -(Vector3D a)
        {
            return new Vector3D(-a.X, -a.Y, -a.Z);
        }

        public bool Equals(Vector3D other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3D other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        public void Normalize()
        {
            var l = Length;
            X /= l;
            Y /= l;
            Z /= l;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        public static double DotProduct(Vector3D a, Vector3D b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
    }
}
#endif