using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Mathematics.Compatibility;
#if !WPFFEATURES
#else
using System.Windows;
#endif

namespace iSukces.Mathematics;

public sealed class TopologyShape : TopologyBase
{
    public TopologyShape()
    {
        Points    = new List<Point>();
        Triangles = new List<TopologyTriangle>();
    }

    public List<List<Point>> Border
    {
        get
        {
            var tmp = from a in Triangles
                from b in a.GetLines()
                select b;
            // var tmpX = tmp.GroupBy(x => x).Select(x => new { ilosc = x.Count(), a = x.Key.A, b = x.Key.B }).ToArray();
            var tmp1 = tmp.GroupBy(x => x).Where(x => x.Count() == 1).Select(x => x.Key).ToList();
            var r    = new List<List<Point>>();
            while (tmp1.Count > 0)
            {
                var fragment = new List<Point>();
                r.Add(fragment);
                var jeden   = tmp1[0].A;
                var current = tmp1[0].B;
                fragment.Add(jeden);
                tmp1.RemoveAt(0);

                while (true)
                {
                    var tmp2 = tmp1.Where(x => x.A.Equals(current) || x.B.Equals(current)).ToArray();
                    if (tmp2.Length == 0) break;
                    var line = tmp2[0];
                    var idx  = tmp1.IndexOf(line);
                    if (line.B.Equals(current))
                    {
                        var tmp_ = line.A;
                        line.A = line.B;
                        line.B = tmp_;
                    }

                    fragment.Add(current);
                    current = line.B;
                    tmp1.RemoveAt(idx);
                    if (current == jeden) break;
                }
            }

            return r;
        }
    }

    /// <summary>
    ///     min długość boku trójkąta
    /// </summary>
    public double MinTriangleLen { get; set; } = 1e-10;

    /// <summary>
    ///     zaokrąglanie współrzędnych punktów
    /// </summary>
    public int PointRound { get; set; } = -1;

    /// <summary>
    ///     Wierzchołki
    /// </summary>
    public IList<Point> Points { get; set; }

    /// <summary>
    ///     Trójkąty
    /// </summary>
    public IList<TopologyTriangle> Triangles { get; set; }

    public static TopologyShape operator -(TopologyShape? left, TopologyShape right)
    {
        if (left is null) return null;
        return left.Substract(right);
    }

    public static explicit operator TopologyShape(Rect src)
    {
        var shape = new TopologyShape();
        shape.Points.Add(new Point(src.X, src.Y));
        shape.Points.Add(new Point(src.X, src.Y + src.Height));
        shape.Points.Add(new Point(src.X + src.Width, src.Y + src.Height));
        shape.Points.Add(new Point(src.X + src.Width, src.Y));
        shape.Triangles.Add(new TopologyTriangle(shape, 0, 1, 2));
        shape.Triangles.Add(new TopologyTriangle(shape, 0, 2, 3));
        return shape;
    }

    private TopologyTriangle AddTriangle(SimpleTriangle x)
    {
        var a = GetPointIndex(x.PA);
        var b = GetPointIndex(x.PB);
        var c = GetPointIndex(x.PC);
        var t = new TopologyTriangle(this, a, b, c);
        Triangles.Add(t);
        return t;
    }

    private int GetPointIndex(Point p)
    {
        if (PointRound >= 0) p = new Point(Math.Round(p.X, PointRound), Math.Round(p.Y, PointRound));
        var i                  = Points.IndexOf(p);
        if (i >= 0) return i;
        Points.Add(p);
        return Points.Count - 1;
    }

    public TopologyTriangle[] AddRectangle(Point pa, Point pb, Point pc, Point pd)
    {
        var a = GetPointIndex(pa);
        var b = GetPointIndex(pb);
        var c = GetPointIndex(pc);
        var d = GetPointIndex(pd);
        var t = new TopologyTriangle[2];

        t[0] = AddTriangle(a, b, c);
        t[1] = AddTriangle(a, c, d);
        // this.triangles.Add(t[0]);
        // this.triangles.Add(t[1]);
        return t;
    }

    public TopologyTriangle? AddTriangle(int a, int b, int c)
    {
        if (a == b || a == c || b == c)
            return null;
        var t = new TopologyTriangle(this, a, b, c);
        if (t.LineAB.Length < MinTriangleLen || t.LineAC.Length < MinTriangleLen ||
            t.LineBC.Length < MinTriangleLen)
            return null;
        // DeveloperLog.Log("Add triangle " + t.ToString());
        Triangles.Add(t);
        return t;
    }

    public TopologyTriangle? AddTriangle(Point pa, Point pb, Point pc)
    {
        var a = GetPointIndex(pa);
        var b = GetPointIndex(pb);
        var c = GetPointIndex(pc);
        return AddTriangle(a, b, c);
    }

    public TopologyShape Optimize()
    {
        var r = new TopologyShape();
        r.PointRound     = PointRound;
        r.MinTriangleLen = MinTriangleLen;
        var cnt = Triangles.Count;
        for (var i = 0; i < cnt; i++)
        {
            var t1 = Triangles[i];
            for (var j = i + 1; j < cnt; j++)
            {
                var t2   = Triangles[j];
                var line = t1.CommonLine(t2);
                if (line == (object?)null) continue;
                var c1 = t1.GetOtherPointOfTrangle(line.PointA, line.PointB);
                var c2 = t2.GetOtherPointOfTrangle(line.PointA, line.PointB);
                var v  = c2 - c1;
                v = new Vector(v.Y, -v.X);
                var    cc = -c1.X * v.X - c1.Y * v.Y;
                double d2;

                d2 = line.PointA.X * v.X + line.PointA.Y * v.Y + cc;
                if (d2 == 0)
                {
                    r.AddTriangle(c1, c2, line.PointB);
                    for (var k = i + 1; k < cnt; k++)
                        if (k != j)
                        {
                            t2 = Triangles[k];
                            r.AddTriangle(t2.PA, t2.PB, t2.PC);
                        }

                    return r.Optimize();
                }

                d2 = line.PointB.X * v.X + line.PointB.Y * v.Y + cc;
                if (d2 == 0)
                {
                    r.AddTriangle(c1, c2, line.PointA);
                    for (var k = i + 1; k < cnt; k++)
                        if (k != j)
                        {
                            t2 = Triangles[k];
                            r.AddTriangle(t2.PA, t2.PB, t2.PC);
                        }

                    return r.Optimize();
                }
            }

            r.AddTriangle(t1.PA, t1.PB, t1.PC);
        }

        return r;
    }

    public TopologyShape Substract(TopologyShape? cutter)
    {
        if (cutter is null || cutter.Triangles is null || cutter.Triangles.Count == 0) return this;
        var from = this;
        var s    = new TopologyShape();
        s.PointRound = PointRound;

        // int i = 0;
        foreach (var ct in cutter.Triangles)
        {
            //if (i++ == 0) continue;
            s            = new TopologyShape();
            s.PointRound = PointRound;
            var cutterMachine = new TopologyShapeCutter();
            cutterMachine.Output = s;

            foreach (var t in from.Triangles)
                try
                {
                    cutterMachine.CutTriangleByTriangle(t, ct);
                }
                catch
                {
                    var x = string.Format("g.DrawLine(new Pen(Brushes.Red, 1), {0});", ct.PointsStr) + "\r\n" +
                            string.Format("g.DrawLine(new Pen(Brushes.Black, 1), {0});", t.PointsStr);
                    throw;
                }

            from = s;
            //  return s;
        }

        return s;
    }

    public static TopologyShape FromPoints(Point a, Point b, Point c)
    {
        var r = new TopologyShape();
        r.Points.Add(a);
        r.Points.Add(b);
        r.Points.Add(c);
        r.Triangles.Add(new TopologyTriangle(r, 0, 1, 2));
        return r;
    }


    public static TopologyShape FromPoints(Point a, Point b, Point c, Point d)
    {
        var r = new TopologyShape();
        r.Points.Add(a);
        r.Points.Add(b);
        r.Points.Add(c);
        r.Points.Add(d);
        r.Triangles.Add(new TopologyTriangle(r, 0, 1, 2));
        r.Triangles.Add(new TopologyTriangle(r, 0, 2, 3));
        r.Triangles[0].LineAC.IsHiddenTriangleEdge = true;
        r.Triangles[1].LineAB.IsHiddenTriangleEdge = true;
        return r;
    }
}