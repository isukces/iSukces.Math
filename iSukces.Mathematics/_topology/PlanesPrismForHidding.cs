#if !WPFFEATURES
#else
using System.Windows.Media.Media3D;
#endif
using System.Collections.Generic;
using System.Linq;
using System;

namespace iSukces.Mathematics;

/// <summary>
///     Przechowuje płaszczyzny pomocne przy określaniu czy jakiś kszatłt przesłania inny kształt
/// </summary>
public sealed class PlanesPrismForHidding
{
    public static List<Section3D> FindNotHidden(Point3D a, Point3D b, IEnumerable<PlanesPrismForHidding> prisms)
    {
        var src = new List<Section3D>();
        var dst = new List<Section3D>();
        src.Add(new Section3D(a, b));
        if (src[0].Length == 0) return dst;
        if (prisms is null)
            return src;
        var prismsList = prisms as IReadOnlyList<PlanesPrismForHidding> ?? prisms.ToList();
        if (!prismsList.Any()) return src;
        for (var index = 0; index < prismsList.Count; index++)
        {
            var i = prismsList[index];
            foreach (var section in src)
            {
                var h = i.FindNotHidden(section.Begin, section.End);
                dst.AddRange(h);
            }

            if (dst.Count == 0)
                return dst;
            src = dst;
            dst = new List<Section3D>();
        }

        return src;
    }

    public List<Section3D> FindNotHidden(Point3D a, Point3D b)
    {
        var vLine = b - a;
        var length = vLine.Length;
        if (length < 1e-9)
        {
            return (TopPlate.DistanceNotNormalized(a) < 0 && InsideAllSides(a))
                ? new List<Section3D>()
                : new List<Section3D> { new Section3D(a, b) };
        }

        var tValues = new List<double> { 0.0, 1.0 };

        // Intersect TopPlate
        double tTop = CalculateIntersectionT(a, vLine, TopPlate);
        if (tTop > 0 && tTop < 1) tValues.Add(tTop);

        // Intersect Sides
        foreach (var side in Sides)
        {
            double tSide = CalculateIntersectionT(a, vLine, side);
            if (tSide > 0 && tSide < 1) tValues.Add(tSide);
        }

        tValues.Sort();

        var result = new List<Section3D>();
        for (int i = 0; i < tValues.Count - 1; i++)
        {
            double tStart = tValues[i];
            double tEnd = tValues[i + 1];
            if (tEnd - tStart < 1e-9) continue;

            double tMid = (tStart + tEnd) / 2.0;
            Point3D midPoint = a + vLine * tMid;

            // Point is HIDDEN if it's below TopPlate AND inside all Side planes
            if (!(TopPlate.DistanceNotNormalized(midPoint) < 0 && InsideAllSides(midPoint)))
            {
                result.Add(new Section3D(a + vLine * tStart, a + vLine * tEnd));
            }
        }

        return result;
    }

    private double CalculateIntersectionT(Point3D origin, Vector3D direction, Plane3D plane)
    {
        var normal = plane.Normal;
        var dotNormalDir = normal.X * direction.X + normal.Y * direction.Y + normal.Z * direction.Z;

        if (Math.Abs(dotNormalDir) < 1e-9) return -1.0; // Parallel

        var distOrigin = plane.DistanceNotNormalized(origin);
        return -distOrigin / dotNormalDir;
    }

    private bool InsideAllSides(Point3D a)
    {
        foreach (var s in Sides)
        {
            if (s.DistanceNotNormalized(a) < 0)
                return false;
        }
        return true;
    }

    public Plane3D TopPlate { get; set; }
    public Plane3D[] Sides { get; set; }
    public object UserData { get; set; }
}
