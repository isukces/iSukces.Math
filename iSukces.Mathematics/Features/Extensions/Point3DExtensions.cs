#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics;

public static class Point3DExtensions
{
    extension(Point3D p)
    {
        public Point ToPoint2D()
        {
            return new Point(p.X, p.Y);
        }
    }
}