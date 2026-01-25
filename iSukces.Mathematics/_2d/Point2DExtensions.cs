
using System;

namespace iSukces.Mathematics;

public static class Point2DExtensions
{
    extension(Point p)
    {
        public Point3D ToPoint3D(double z) { return new Point3D(p.X, p.Y, z); }
        public Point3D ToPoint3D() { return new Point3D(p.X, p.Y, 0); }

        public Point3D ToPoint3D(double z, ICuttingSurface? cs)
        {
            return cs is null
                ? new Point3D(p.X, p.Y, z)
                : new Point3D(p.X, p.Y, z + cs.CalculateZ(p.X, p.Y));
        }
        
        public Point Round(int decimals)
        {
            return new Point(
                Math.Round(p.X, decimals),
                Math.Round(p.Y, decimals));
        }
    }
    
    extension(Point[]? p)
    {
        public double GetArea()
        {
            if (p is null || p.Length < 3) return 0;
            if (p.Length == 3)
            {
                var v1 = p[1] - p[0];
                var v2 = p[2] - p[1];
                return 0.5 * Vector.CrossProduct(v1, v2);
            }
            var sum = 0.0;
            for (var i = 0; i < p.Length; i++)
            {
                var vector1 = i == 0 ? p[^1] : p[i - 1];
                var vector2 = p[i];
                sum += vector1.X * vector2.Y - vector1.Y * vector2.X;
            }
            return 0.5 * sum;
        }
    }
 
}