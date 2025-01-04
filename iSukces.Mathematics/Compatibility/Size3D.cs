#if !WPFFEATURES
using System;

namespace iSukces.Mathematics.Compatibility
{
    public readonly struct Size3D : IEquatable<Size3D>
    {
        public Size3D(double x, double y, double z)
        {
            if (x < 0 || y < 0 || z < 0)
                throw new ArgumentException(ua);
            X = x;
            Y = y;
            Z = z;
        }

        public static bool operator ==(Size3D a, Size3D b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Size3D a, Size3D b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public bool Equals(Size3D other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object? obj)
        {
            return obj is Size3D other && Equals(other);
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

        public static Size3D Empty { get; set; }

        public bool IsEmpty => X < 0;

        public double X { get; }

        public double Y { get; }

        public double Z { get; }

        private const string ua = "ujemny argument";
    }
}
#endif
