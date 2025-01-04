#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif


namespace iSukces.Mathematics;

public static class Extensions3D
{
    public static Point3D ToPoint3D(this Point p, double z) { return new Point3D(p.X, p.Y, z); }

    public static Point3D ToPoint3D(this Point p) { return new Point3D(p.X, p.Y, 0); }

    public static Point3D ToPoint3D(this Point p, double z, ICuttingSurface? cs)
    {
        return cs is null
            ? new Point3D(p.X, p.Y, z)
            : new Point3D(p.X, p.Y, z + cs.CalculateZ(p.X, p.Y));
    }


    public static Vector3D ToVector3D(this Vector p) { return new Vector3D(p.X, p.Y, 0); }
}