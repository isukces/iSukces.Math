#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint=System.Windows.Point;
using TheVector=System.Windows.Vector;
#endif

namespace iSukces.Mathematics;

public sealed class Line3D
{

    public static Line3D operator *(Line3D l, Coordinates3D? c)
    {
        if (c is null)
            return l;
        return new Line3D(l.Point * c, l.Vector * c);
    }

    /// <summary>
    /// Tworzy instancję obiektu z 2 punktów
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public Line3D(Point3D p1, Point3D p2)
    {
        Point  = p1;
        Vector = p2 - p1;
        Vector.Normalize();
    }
 

    /// <summary>
    /// Tworzy instancję obiektu
    /// <param name="point">punkt startowy</param>
    /// <param name="vector">vektor kierunku</param>
    /// </summary>
    public Line3D(Point3D point, Vector3D vector)
    {
        Point  = point;
        Vector = vector;
    }
 
    /// <summary>
    /// punkt startowy
    /// </summary>
    public Point3D Point { get; }

    /// <summary>
    /// vektor kierunku
    /// </summary>
    public Vector3D Vector { get; }
}