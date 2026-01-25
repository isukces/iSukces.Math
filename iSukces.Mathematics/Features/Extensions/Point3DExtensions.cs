
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