#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows.Media.Media3D;
#endif
using System.Collections.Generic;
using System.Linq;


namespace iSukces.Mathematics
{
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
                for (var index1 = 0; index1 < src.Count; index1++)
                {
                    var line = src[index1];
                    var h    = i.FindNotHidden(line.Begin, line.End);
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
            var sec   = new Section3D(a, b);
            var l     = new Line3D(a, b);
            var cross = new List<Point3D>();
            for (int i = 0, max = Sides.Length; i < max; i++)
            {
                var plane = Sides[i];
                var pc    = plane.Cross(l);
                if (!pc.HasValue) continue;
                var dist = TopPlate.DistanceNotNormalized(pc.Value);
                if (dist >= 0) continue;
                plane = Sides[i == 0 ? max - 1 : i - 1];
                dist  = plane.DistanceNotNormalized(pc.Value);
                if (dist < 0) continue;
                plane = Sides[(i + 1) % max];
                dist  = plane.DistanceNotNormalized(pc.Value);
                if (dist < 0) continue;
                cross.Add(pc.Value);
            }

            var result = new List<Section3D>();
            if (cross.Count == 0)
            {
                result.Add(new Section3D(a, b));
            }
            else
            {
                var dlugoscOdcinka = sec.Length;
                var vLine          = l.Vector;
                var cross1 = (
                    from o in cross
                    let dist = Vector3D.DotProduct(o - a, vLine)
                    where dist > 0 && dist < dlugoscOdcinka
                    select new PointAndDistance(o, dist)
                ).ToList();
                cross1.Add(new PointAndDistance(a, 0));
                cross1.Add(new PointAndDistance(b, dlugoscOdcinka));
                cross1.Sort(PointAndDistance.Comare);
                {
                    var cnt = cross1.Count;
                    for (var i = 1; i < cnt; i++)
                    {
                        var p1 = cross1[i - 1];
                        var p2 = cross1[i];
                        var c  = p1.Point - p2.Point;
                        if (c.Length > 1e-6) continue;
                        cross1.RemoveAt(i == cnt - 1 ? i - 1 : i);
                        i--;
                        cnt--;
                    }

                    for (var i = 1; i < cnt; i++)
                    {
                        var p1      = cross1[i - 1].Point;
                        var p2      = cross1[i].Point;
                        var pc      = new Point3D((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                        var topDist = TopPlate.DistanceNotNormalized(pc);
                        if (topDist < 0 && InsideAllSides(pc))
                            continue;
                        result.Add(new Section3D(p1, p2));
                    }
                }
            }

            return result;
        }

        private bool InsideAllSides(Point3D a)
        {
            foreach (var s in Sides)
                if (s.DistanceNotNormalized(a) < 0)
                    return false;
            return true;
        }

        /// <summary>
        ///     płaszczyzna górna
        /// </summary>
        public Plane3D TopPlate { get; set; }

        /// <summary>
        ///     płaszczyzny ograniczające boczne
        /// </summary>
        public Plane3D[] Sides { get; set; }

        /// <summary>
        /// </summary>
        public object UserData { get; set; }

        private sealed class PointAndDistance
        {
            public PointAndDistance(Point3D point, double dist)
            {
                 Point = point;
                Dist  = dist;
            }

            public static int Comare(PointAndDistance a, PointAndDistance b)
            {
                return a.Dist.CompareTo(b.Dist);
            }

            public readonly Point3D Point;
            public readonly double Dist;
        }
    }
}