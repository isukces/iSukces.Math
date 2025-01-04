#if !WPFFEATURES
using System;

namespace iSukces.Mathematics.Compatibility
{
    public struct Size : IEquatable<Size>
    {
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public static bool Equals(Size size1, Size size2)
        {
            if (size1.IsEmpty)
                return size2.IsEmpty;
            if (size1.Width.Equals(size2.Width))
                return size1.Height.Equals(size2.Height);
            return false;
        }

        public static bool operator ==(Size size1, Size size2)
        {
            if (size1.Width == size2.Width)
                return size1.Height == size2.Height;
            return false;
        }


        public static explicit operator Vector(Size size)
        {
            return new Vector(size.Width, size.Height);
        }


        public static explicit operator Point(Size size)
        {
            return new Point(size.Width, size.Height);
        }

        public static bool operator !=(Size size1, Size size2)
        {
            return !(size1 == size2);
        }

        private static Size CreateEmptySize()
        {
            return new Size(double.NegativeInfinity, double.NegativeInfinity);
        }

        public override bool Equals(object? o)
        {
            return o is Size size && Equals(this, size);
        }


        public bool Equals(Size value)
        {
            return Equals(this, value);
        }


        public override int GetHashCode()
        {
            if (IsEmpty)
                return 0;
            return Width.GetHashCode() ^ Height.GetHashCode();
        }


        public override string ToString()
        {
            if (IsEmpty)
                return "Empty";
            var numericListSeparator = Utils.GetNumericListSeparator(null);
            return $"{Width}{numericListSeparator}{Height}";
        }

        public static Size Empty { get; } = CreateEmptySize();

        public bool IsEmpty => Width < 0.0;


        public double Width { get; }


        public double Height { get; }
    }
}
#endif
