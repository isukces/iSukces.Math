#if !WPFFEATURES

namespace iSukces.Mathematics.Compatibility
{
    public struct Rect
    {
        public Rect(double x, double y, double width, double height)
        {
            X      = x;
            Y      = y;
            Width  = width;
            Height = height;
        }

        public Rect(Point topLeft, Point bottomRight)
        {
            X     = topLeft.X;
            Y     = topLeft.Y;
            Width = bottomRight.X - topLeft.X;
            Height = bottomRight.Y - topLeft.Y;
        }

        public static bool operator ==(Rect rect1, Rect rect2)
        {
            return rect1.X == rect2.X
                   && rect1.Y == rect2.Y
                   && rect1.Width == rect2.Width
                   && rect1.Height == rect2.Height;
        }

        public static bool operator !=(Rect rect1, Rect rect2) { return !(rect1 == rect2); }

        private static Rect CreateEmptyRect()
        {
            return new Rect(double.PositiveInfinity, double.PositiveInfinity,
                double.NegativeInfinity, double.NegativeInfinity);
        }

        private static bool Equals(Rect rect1, Rect rect2)
        {
            if (rect1.IsEmpty)
                return rect2.IsEmpty;
            if (rect1.X.Equals(rect2.X) && rect1.Y.Equals(rect2.Y) && rect1.Width.Equals(rect2.Width))
                return rect1.Height.Equals(rect2.Height);
            return false;
        }

        public override bool Equals(object o) { return o is Rect rect && Equals(this, rect); }


        public bool Equals(Rect value) { return Equals(this, value); }


        public override int GetHashCode()
        {
            if (IsEmpty)
                return 0;
            return X.GetHashCode()
                   ^ Y.GetHashCode()
                   ^ Width.GetHashCode()
                   ^ Height.GetHashCode();
        }


        public override string ToString()
        {
            if (IsEmpty)
                return "Empty";
            var numericListSeparator = Utils.GetNumericListSeparator(null);
            return $"{X}{numericListSeparator}{Y}{numericListSeparator}{Width}{numericListSeparator}{Height}";
        }

        public static Rect Empty { get; } = CreateEmptyRect();

        public bool IsEmpty => Width < 0.0;

        public double X { get; }

        public double Y { get; }

        public double Width { get; }

        public double Height { get; }

        public double Left => X;
        
        public double Top => Y;


        public double Right
        {
            get
            {
                if (IsEmpty)
                    return double.NegativeInfinity;
                return X + Width;
            }
        }


        public double Bottom
        {
            get
            {
                if (IsEmpty)
                    return double.NegativeInfinity;
                return Y + Height;
            }
        }

        public Point TopLeft => new Point(Left, Top);

        public Point TopRight => new Point(Right, Top);

        public Point BottomLeft => new Point(Left, Bottom);

        public Point BottomRight => new Point(Right, Bottom);
    }
}
#endif
