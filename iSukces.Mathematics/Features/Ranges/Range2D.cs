using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
#if COREFX
using iSukces.Mathematics.Compatibility;
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics
{
    public struct Range2D
    {
        public Range2D(Range xRange, Range yRange)
        {
            XRange = xRange;
            YRange = yRange;
        }
        
        public Range2D(Point p1, Point p2)
        {
            XRange = Range.FromValues(p1.X, p2.X);
            YRange = Range.FromValues(p1.Y, p2.Y);
        }

        public Range2D(Rect rect)
        {
            XRange = new Range(rect.Left, rect.Right);
            YRange = new Range(rect.Top, rect.Bottom);
        }

        public Range2D Round()
        {
            return new Range2D(XRange.RoundDouble(), YRange.RoundDouble());
        }
        
        public bool Includes(ThePoint point)
        {
            return XRange.Includes(point.X) && YRange.Includes(point.Y);
        }

        public bool IncludesExclusive(ThePoint point)
        {
            return XRange.IncludesExclusive(point.X) && YRange.IncludesExclusive(point.Y);
        }

        public Range2D WithPoint(ThePoint x)
        {
            return new Range2D(
                XRange.WithValue(x.X),
                YRange.WithValue(x.Y));
        }

        public static Range2D Empty
        {
            get { return new Range2D(); }
        }

        public Range XRange { get; private set; }
        public Range YRange { get; private set; }
        public bool IsEmpty
        {
            get { return XRange.IsEmpty || YRange.IsEmpty; }
        }

        public double Area
        {
            get { return IsEmpty ? 0 : XRange.Length * YRange.Length; }
        }


        public ThePoint Min
        {
            get { return new ThePoint(XRange.Min, YRange.Min); }
        }

        public ThePoint Max
        {
            get { return new ThePoint(XRange.Max, YRange.Max); }
        }

        public ThePoint Center
        {
            get { return new ThePoint(XRange.Center, YRange.Center); }
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
}