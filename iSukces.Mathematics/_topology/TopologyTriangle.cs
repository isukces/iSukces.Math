using System;
using System.Collections.Generic;
using System.Linq;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace iSukces.Mathematics;

public sealed class SimpleTriangle : object
{
    public static explicit operator SimpleTriangle(TopologyTriangle src)
    {
        return new SimpleTriangle
        {
            PA = src.Owner.Points[src.PointAIndex],
            PB = src.Owner.Points[src.PointBIndex],
            PC = src.Owner.Points[src.PointCIndex]
        };
    }

    public TopologyTriangleLine GetLine(int nr)
    {
        nr = nr % 3;
        if (nr == 0) return LineAB;
        if (nr == 1) return LineBC;
        return LineCA;
    }

    public Point GetPoint(int nr)
    {
        nr = nr % 3;
        if (nr == 0) return PA;
        if (nr == 1) return PB;
        return PC;
    }

    /// <summary>
    ///     Powierzchnia trójkąta
    /// </summary>
    public double Area
    {
        get { return Vector.CrossProduct(PB - PA, PC - PA) * 0.5; }
    }

    public Point PA { get; set; }

    public Point PB { get; set; }

    public Point PC { get; set; }

    public TopologyTriangleLine LineAB
    {
        get { return new TopologyTriangleLine(PA, PB); }
    }

    public TopologyTriangleLine LineBC
    {
        get { return new TopologyTriangleLine(PB, PC); }
    }

    public TopologyTriangleLine LineCA
    {
        get { return new TopologyTriangleLine(PC, PA); }
    }
}

public sealed class TopologyTriangle : TopologyBase
{
    public TopologyTriangle(TopologyShape owner, int pointAIndex, int pointBIndex, int pointCIndex)
    {
        Owner       = owner;
        PointAIndex = pointAIndex;
        PointBIndex = pointBIndex;
        PointCIndex = pointCIndex;

        pA = owner.Points[pointAIndex];
        pB = owner.Points[pointBIndex];
        pC = owner.Points[pointCIndex];

        LineAB = CreateLine(pointAIndex, pointBIndex, pointCIndex);
        LineAC = CreateLine(pointAIndex, pointCIndex, pointBIndex);
        LineBC = CreateLine(pointBIndex, pointCIndex, pointAIndex);
        // owner.Triangles.Add(this);
    }

    private static void GetMinMax(double a, double b, double c, out double min, out double max)
    {
        min = Math.Min(a, Math.Min(b, c));
        max = Math.Max(a, Math.Max(b, c));
    }

    public TopologyTriangleLine? CommonLine(TopologyTriangle t)
    {
        if (hasThisLine(t.LineAB)) return t.LineAB;
        if (hasThisLine(t.LineAC)) return t.LineAC;
        if (hasThisLine(t.LineBC)) return t.LineBC;
        return null;
    }

    /// <summary>
    ///     Zwraca kolekcję punktów, które są wewnątrz tego trójkąta
    /// </summary>
    /// <param name="points">kolekcja punktów, które są badane</param>
    /// <returns>kolekcja punktów</returns>
    public IEnumerable<Point> GetInsidePoints(IEnumerable<Point> points)
    {
        return points.Where(x => IsInside(x));
    }

    /// <summary>
    ///     Zwraca kolekcję punktów, które są wewnątrz tego trójkąta
    /// </summary>
    /// <param name="tc">trójkąt, którego wierzchołki są badane</param>
    /// <returns>kolekcja punktów</returns>
    public IEnumerable<Point> GetInsidePoints(TopologyTriangle tc)
    {
        return tc.Points.Where(x => IsInside(x));
    }

    public SimpleLine[] GetLines()
    {
        var l = new SimpleLine[3];
        l[0] = (SimpleLine)LineAB;
        l[1] = (SimpleLine)LineAC;
        l[2] = (SimpleLine)LineBC;
        return l;
    }

    public Point GetOtherPointOfTrangle(Point a, Point b)
    {
        if (!pA.Equals(a) && !pA.Equals(b)) return pA;
        if (!pB.Equals(a) && !pB.Equals(b)) return pB;
        if (!pC.Equals(a) && !pC.Equals(b)) return pC;
        throw new ArgumentException();
    }

    public Point GetOtherPointOfTrangle(TopologyTriangleLine? l)
    {
        return GetOtherPointOfTrangle(l.PointA, l.PointB);
    }

    public bool HasThisPoint(Point a)
    {
        return a.Equals(pA) || a.Equals(pB) || a.Equals(pC);
    }

    public bool HasThisTwoPoints(Point a, Point b)
    {
        return HasThisPoint(a) && HasThisPoint(b);
    }

    public bool IsInside(Point p)
    {
        return LineAB.RelativeDistance(p) > 0 && LineAC.RelativeDistance(p) > 0 &&
               LineBC.RelativeDistance(p) > 0;
    }

    public bool IsInside(TopologyTriangle p)
    {
        return IsInside(p.pA) && IsInside(p.pB) && IsInside(p.pC);
    }

    public List<SimpleTriangle> Substract(TopologyTriangle x)
    {
        var l = new List<SimpleTriangle>();
        return l;
    }

    public override string ToString()
    {
        return string.Format("[ {0} ] [ {1} ] [ {2} ] area={3}", pA, pB, pC, Area);
    }

    private TopologyTriangleLine? CreateLine(int a, int b, int c)
    {
        var _a = Owner.Points[a];
        var _b = Owner.Points[b];

        var r = new TopologyTriangleLine(_a, _b);
        r.ReverseDistanceMeasure = r.IsOnDarkSide(Owner.Points[c]);
        return r;
    }

    private bool hasThisLine(TopologyTriangleLine? a)
    {
        return a.Equals(LineAB) || a.Equals(LineAC) || a.Equals(LineBC);
    }

    public string DeveloperString
    {
        get
        {
            return string.Join("\r\n", from Q in new[] {PA, PB, PC, PA} select string.Format("{0}\t{1}", Q.X, Q.Y));
        }
    }

    public Point Center
    {
        get { return new Point((pA.X + pB.X + pC.X) / 3.0, (pA.Y + pB.Y + pC.Y) / 3.0); }
    }

    /// <summary>
    ///     Powierzchnia trójkąta
    /// </summary>
    public double Area
    {
        get { return Vector.CrossProduct(pB - pA, pC - pA) * 0.5; }
    }

    public Rect Boundings
    {
        get
        {
            double xMin, xMax, yMin, yMax;
            GetMinMax(PA.X, PB.X, PC.X, out xMin, out xMax);
            GetMinMax(PA.Y, PB.Y, PC.Y, out yMin, out yMax);
            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }
    }

    /// <summary>
    ///     Bok AB
    /// </summary>
    public TopologyTriangleLine? LineAB { get; }

    /// <summary>
    ///     Bok AC
    /// </summary>
    public TopologyTriangleLine? LineAC { get; }

    /// <summary>
    ///     Bok BC
    /// </summary>
    public TopologyTriangleLine? LineBC { get; }

    /// <summary>
    ///     Właściciel
    /// </summary>
    public TopologyShape Owner { get; }

    public Point PA
    {
        get { return pA; }
    }

    public Point PB
    {
        get { return pB; }
    }

    public Point PC
    {
        get { return pC; }
    }

    /// <summary>
    ///     Indeks wierzchołka A
    /// </summary>
    public int PointAIndex { get; }

    /// <summary>
    ///     Indeks wierzchołka B
    /// </summary>
    public int PointBIndex { get; }

    /// <summary>
    ///     Indeks wierzchołka C
    /// </summary>
    public int PointCIndex { get; }

    public Point[] Points
    {
        get { return new[] {pA, pB, pC}; }
    }

    public string PointsStr
    {
        get { return string.Format("{0}, {1}, {2}, {3}, {4}, {5}", pA.X, pA.Y, pB.X, pB.Y, pC.X, pC.Y); }
    }

    private Point pA;

    private Point pB;

    private Point pC;
        
}