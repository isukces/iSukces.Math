namespace iSukces.Mathematics;

public struct Range2D
{
    public Range2D(DRange xRange, DRange yRange)
    {
        XRange = xRange;
        YRange = yRange;
    }
        
    public Range2D(Point p1, Point p2)
    {
        XRange = DRange.FromValues(p1.X, p2.X);
        YRange = DRange.FromValues(p1.Y, p2.Y);
    }

    public Range2D(Rect rect)
    {
        XRange = new DRange(rect.Left, rect.Right);
        YRange = new DRange(rect.Top, rect.Bottom);
    }

    public Range2D Round()
    {
        return new Range2D(XRange.RoundDouble(), YRange.RoundDouble());
    }
        
    public bool Includes(Point point)
    {
        return XRange.Includes(point.X) && YRange.Includes(point.Y);
    }

    public bool IncludesExclusive(Point point)
    {
        return XRange.IncludesExclusive(point.X) && YRange.IncludesExclusive(point.Y);
    }

    public Range2D WithPoint(Point x)
    {
        return new Range2D(
            XRange.WithValue(x.X),
            YRange.WithValue(x.Y));
    }

    public static Range2D Empty
    {
        get { return new Range2D(); }
    }

    public DRange XRange { get; private set; }
    public DRange YRange { get; private set; }
    public bool IsEmpty
    {
        get { return XRange.IsEmpty || YRange.IsEmpty; }
    }

    public double Area
    {
        get { return IsEmpty ? 0 : XRange.Length * YRange.Length; }
    }


    public Point Min
    {
        get { return new Point(XRange.Min, YRange.Min); }
    }

    public Point Max
    {
        get { return new Point(XRange.Max, YRange.Max); }
    }

    public Point Center
    {
        get { return new Point(XRange.Center, YRange.Center); }
    }

    public Size Size
    {
        get { return IsEmpty ? Size.Empty : new Size(XRange.Length, YRange.Length); }
    }

    public double Left() => XRange.Min;
    public double Right() => XRange.Max;
    public double Top() => YRange.Min;
    public double Bottom() => YRange.Max;

    public double X1() => XRange.Min;
    public double X2() => XRange.Max;
    public double Y1() => YRange.Min;
    public double Y2() => YRange.Max;
}