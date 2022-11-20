#if !WPFFEATURES

using System;

namespace iSukces.Mathematics.Compatibility
{
    public struct Vector : IEquatable<Vector>
    {
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            return Math.Atan2(vector1.X * vector2.Y - vector2.X * vector1.Y,
                vector1.X * vector2.X + vector1.Y * vector2.Y) * (180.0 / Math.PI);
        }


        public static double CrossProduct(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }


        public static double Determinant(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }


        public static Vector Divide(Vector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        public static bool Equals(Vector vector1, Vector vector2)
        {
            if (vector1.X.Equals(vector2.X))
                return vector1.Y.Equals(vector2.Y);
            return false;
        }


        public static Vector Multiply(double scalar, Vector vector)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }


        public static Vector Multiply(Vector vector, Matrix matrix)
        {
            return matrix.Transform(vector);
        }


        public static double Multiply(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }


        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }


        public static Point operator +(Vector vector, Point point)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y);
        }


        public static Vector operator /(Vector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        public static bool operator ==(Vector vector1, Vector vector2)
        {
            if (vector1.X == vector2.X)
                return vector1.Y == vector2.Y;
            return false;
        }


        public static explicit operator Size(Vector vector)
        {
            return new Size(Math.Abs(vector.X), Math.Abs(vector.Y));
        }


        public static explicit operator Point(Vector vector)
        {
            return new Point(vector.X, vector.Y);
        }

        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !(vector1 == vector2);
        }


        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }


        public static Vector operator *(double scalar, Vector vector)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }


        public static Vector operator *(Vector vector, Matrix matrix)
        {
            return matrix.Transform(vector);
        }


        public static double operator *(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }


        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }


        public static Vector operator -(Vector vector)
        {
            return new Vector(-vector.X, -vector.Y);
        }

        public override bool Equals(object o)
        {
            if (!(o is Vector _))
                return false;
            return Equals(this, (Vector)o);
        }

        public bool Equals(Vector value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }


        public void Normalize()
        {
            this = this / Math.Max(Math.Abs(X), Math.Abs(Y));
            this = this / Length;
        }

        public override string ToString()
        {
            var numericListSeparator = Utils.GetNumericListSeparator(null);
            return $"{X}{numericListSeparator}{Y}";
        }

        public double X { get; }

        public double Y { get; }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public double LengthSquared => X * X + Y * Y;
    }
}

#endif