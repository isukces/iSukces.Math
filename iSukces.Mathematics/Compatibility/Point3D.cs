#if !WPFFEATURES
using System;

namespace iSukces.Mathematics.Compatibility
{
    public struct Point3D : IEquatable<Point3D>
    {
        public Point3D(double x, double y, double z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point3D operator +(Point3D a, Vector3D b)
        {
            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }


        public static Point3D operator +(Vector3D b, Point3D a)
        {
            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static bool operator ==(Point3D a, Point3D b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static explicit operator Point3D(Vector3D x)
        {
            return new Point3D(x.X, x.Y, x.Z);
        }

        public static bool operator !=(Point3D a, Point3D b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static Point3D operator -(Point3D a, Vector3D b)
        {
            return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3D operator -(Point3D point1, Point3D point2)
        {
            return new Vector3D(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
        }

        public bool Equals(Point3D other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            return obj is Point3D other && Equals(other);
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

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
    }
}
#endif