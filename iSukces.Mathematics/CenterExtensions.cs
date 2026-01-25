
namespace iSukces.Mathematics;

public static class CenterExtensions
{
    extension(double a)
    {
        public double Center(double b)
        {
            return (a + b) * 0.5;
        }
    }
    
    extension(Point a)
    {
        public Point Center(Point b)
        {
            return new Point( (a.X + b.X) * 0.5, (a.Y + b.Y) * 0.5);
        }
    }    
    
    extension(Point3D a)
    {
        public Point3D Center(Point3D b)
        {
            return new Point3D((a.X + b.X) * 0.5, (a.Y + b.Y) * 0.5, (a.Z + b.Z) * 0.5);
        }
    }
}
