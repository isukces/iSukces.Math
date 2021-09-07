#if !ALLFEATURES
using System;

namespace iSukces.Mathematics.Compatibility
{
    public struct Point: IEquatable<Point>
    {
        public static bool operator ==(Point point1, Point point2)
        {
            if (point1.X == point2.X)
                return point1.Y == point2.Y;
            return false;
        }


        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }


        public static bool Equals(Point point1, Point point2)
        {
            if (point1.X.Equals(point2.X))
                return point1.Y.Equals(point2.Y);
            return false;
        }


        public override bool Equals(object o)
        {
            if (!(o is Point _))
                return false;
            return Equals(this, (Point)o);
        }


        public bool Equals(Point value)
        {
            return Equals(this, value);
        }


        public override int GetHashCode()
        {
            var num = X;
            var hashCode1 = num.GetHashCode();
            num = Y;
            var hashCode2 = num.GetHashCode();
            return hashCode1 ^ hashCode2;
        }

#if ALLFEATURES
    public static Point Parse(string source)
    {
      IFormatProvider invariantEnglishUs = (IFormatProvider) TypeConverterHelper.InvariantEnglishUS;
      TokenizerHelper tokenizerHelper = new TokenizerHelper(source, invariantEnglishUs);
      Point point =
 new Point(Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs), Convert.ToDouble(tokenizerHelper.NextTokenRequired(), invariantEnglishUs));
      tokenizerHelper.LastTokenRequired();
      return point;
    }
#endif


        public double X { get; }
        
        public double Y { get; }


        public override string ToString()
        {
            var numericListSeparator = Utils.GetNumericListSeparator(null);
            return $"{X}{numericListSeparator}{Y}";
        }
 
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }


        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y);
        }


        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point.X - vector.X, point.Y - vector.Y);
        }


        public static Vector operator -(Point point1, Point point2)
        {
            return new Vector(point1.X - point2.X, point1.Y - point2.Y);
        }


        public static Point operator *(Point point, Matrix matrix)
        {
            return matrix.Transform(point);
        }


        public static explicit operator Size(Point point)
        {
            return new Size(Math.Abs(point.X), Math.Abs(point.Y));
        }


        public static explicit operator Vector(Point point)
        {
            return new Vector(point.X, point.Y);
        }
    }
}
#endif