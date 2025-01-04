#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics;

public static class Point3DExtension
{
    public static Point ToPoint2D(this Point3D p)
    {
        return new Point(p.X, p.Y);
    }
}