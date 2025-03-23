using System;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif


namespace iSukces.Mathematics;

public static class CenterExtensions
{
    public static double Center(this double a, double b)
    {
        return (a + b) * 0.5;
    }
    
    public static Point Center(this Point a, Point b)
    {
        return new Point( (a.X + b.X) * 0.5, (a.Y + b.Y) * 0.5);
    }    
    public static Point3D Center(this Point3D a, Point3D b)
    {
        return new Point3D((a.X + b.X) * 0.5, (a.Y + b.Y) * 0.5, (a.Z + b.Z) * 0.5);
    }
}
