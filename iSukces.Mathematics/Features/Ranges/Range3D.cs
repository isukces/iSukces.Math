using iSukces.Mathematics.Compatibility;
#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics
{
    public struct Range3D
    {
        public Range3D(Range xRange, Range yRange, Range zRange)
        {
            XRange = xRange;
            YRange = yRange;
            ZRange = zRange;
        }

        public Range3D WithPoint(Point3D x)
        {
            return new Range3D(
                XRange.WithValue(x.X),
                YRange.WithValue(x.Y),
                ZRange.WithValue(x.Z));
        }


        public bool Includes(Point3D point)
        {
            return XRange.Includes(point.X) && YRange.Includes(point.Y) && ZRange.Includes(point.Z);
        }

        public bool IncludesExclusive(Point3D point)
        {
            return XRange.IncludesExclusive(point.X) && YRange.IncludesExclusive(point.Y) &&
                   ZRange.IncludesExclusive(point.Z);
        }


        public Range XRange { get; private set; }
        public Range YRange { get; private set; }
        public Range ZRange { get; private set; }
        public bool IsEmpty
        {
            get { return XRange.IsEmpty || YRange.IsEmpty || ZRange.IsEmpty; }
        }

        public Point3D Min
        {
            get { return new Point3D(XRange.Min, YRange.Min, ZRange.Min); }
        }

        public Point3D Max
        {
            get { return new Point3D(XRange.Max, YRange.Max, ZRange.Max); }
        }

        public Point3D Center
        {
            get { return new Point3D(XRange.Center, YRange.Center, ZRange.Center); }
        }

        public Size3D Size
        {
            get { return IsEmpty ? Size3D.Empty : new Size3D(XRange.Length, YRange.Length, ZRange.Length); }
        }

        public static Range3D Empty
        {
            get { return new Range3D(); }
        }
    }
}
