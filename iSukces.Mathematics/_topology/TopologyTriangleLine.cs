using System;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.Mathematics;

public struct SimpleLine
{
    public Point A;
    public Point B;

    public override string ToString()
    {
        return string.Format("{0} {1}", A, B);
    }

    public static explicit operator SimpleLine(TopologyTriangleLine src)
    {
        var i = src.PointA.X.CompareTo(src.PointB.X);
        if (i == 0)
            i = src.PointA.Y.CompareTo(src.PointB.Y);
        if (i < 0)
            return new SimpleLine {A = src.PointA, B = src.PointB};
        return new SimpleLine {B = src.PointA, A = src.PointB};
    }
}

public sealed class TopologyTriangleLine : TopologyBase
{
    public TopologyTriangleLine(Point pa, Point pb) : this(pa, pb, true)
    {
    }

    public TopologyTriangleLine(Point pa, Point pb, bool doOrganize)
    {
        _pointA   = pa;
        _pointB   = pb;
        VectorAB = _pointB - _pointA;
        Length   = VectorAB.Length;
        if (doOrganize)
        {
            var i = pa.X.CompareTo(pb.X);
            if (i == 0)
                i = pa.Y.CompareTo(pb.Y);
            if (i <= 0)
                prepVector = new Vector(-VectorAB.Y, VectorAB.X);
            else
                prepVector = new Vector(VectorAB.Y, -VectorAB.X);
        }
        else
        {
            prepVector = new Vector(-VectorAB.Y, VectorAB.X);
        }

        c = -RelativeDistance(_pointA);
    }

    /// <summary>
    ///     Czy jest to ukryta krawędź trójkąta (np. wynikająca z podziału wieloboku na trójkąty)
    /// </summary>
    public bool IsHiddenTriangleEdge { get; set; }

    public double Length { get; }

    public Point PointA => _pointA;

    private Point _pointA;

    public Point PointB => _pointB;

    private Point _pointB;

    /// <summary>
    ///     czy odwracać znak pomiaru odległości
    /// </summary>
    public bool ReverseDistanceMeasure { get; set; }

    public Vector VectorAB { get; private set; }

    /// <summary>
    ///     Realizuje operator !=
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są różne</returns>
    public static bool operator !=(TopologyTriangleLine? left, TopologyTriangleLine? right)
    {
        var eq = left == right;
        return !eq;
    }

    /// <summary>
    ///     Realizuje operator ==
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są równe</returns>
    public static bool operator ==(TopologyTriangleLine? left, TopologyTriangleLine? right)
    {
        if (left == (object?)null && right == (object?)null) return true;
        if (left == (object?)null || right == (object?)null) return false;
        return left.PointA == right.PointA && left.PointB == right.PointB;
    }

    public double Distance(Point p)
    {
        return RelativeDistance(p) / Length;
    }

    /// <summary>
    ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
    /// </summary>
    /// <param name="obj">obiekt do porównania z obiektem bieżącym</param>
    /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not TopologyTriangleLine line) return false;
        if (_pointA.Equals(line._pointA) && _pointB.Equals(line._pointB)) return true;
        if (_pointA.Equals(line._pointB) && _pointB.Equals(line._pointA)) return true;
        return false;
    }

    /// <summary>
    ///     Zwraca kod HASH obiektu
    /// </summary>
    /// <returns>kod HASH obiektu</returns>
    public override int GetHashCode()
    {
        return _pointA.GetHashCode() ^ _pointB.GetHashCode();
    }

    public Point GetOtherVertex(Point a)
    {
        if (a.Equals(_pointA)) return _pointB;
        if (a.Equals(_pointB)) return _pointA;
        throw new ArgumentException("Błędny punkt " + a);
    }

    public bool HasThisTwoPoints(Point a, Point b)
    {
        return a.Equals(_pointA) && b.Equals(_pointB) || b.Equals(_pointA) && a.Equals(_pointB);
    }

    public bool IsOnDarkSide(Point p)
    {
        return RelativeDistance(p) < 0;
    }

    /// <summary>
    ///     Czy wskazany punkt jest końcem odcinka
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public bool IsVertex(Point p)
    {
        return p.Equals(_pointA) || p.Equals(_pointB);
    }

    public double RelativeDistance(Point p)
    {
        var tmp = p.X * prepVector.X + p.Y * prepVector.Y + c;
        return ReverseDistanceMeasure ? -tmp : tmp;
    }

    public void Reverse()
    {
        VectorAB = -VectorAB;
        c        = -c;
    }

    public override string ToString()
    {
        if (_pointA.Equals(_pointB))
            return string.Format("{0} ?????", _pointA);
        return string.Format("{0} -> {1}", _pointA, _pointB);
    }

    public bool TriangleOnMinusOrZeroSide(TopologyTriangle t)
    {
        if (RelativeDistance(t.PA) > 0) return false;
        if (RelativeDistance(t.PB) > 0) return false;
        if (RelativeDistance(t.PC) > 0) return false;
        return true;
    }

    private double c;
    private readonly Vector prepVector;

    public static TopologySideCross? Cross(TopologyTriangleLine? a, TopologyTriangleLine? b)
    {
        var d1 = a.RelativeDistance(b._pointA);
        // if (d1 == 0) return new TopologySideCross() { First = a, Second = b, CrossPoint = b.pointA };
        var d2 = a.RelativeDistance(b._pointB);
        // if (d2 == 0) return new TopologySideCross() { First = a, Second = b, CrossPoint = b.pointB };
        if (d1 < 0 && d2 < 0 || d1 > 0 && d2 > 0) return null;

        d1 = b.RelativeDistance(a._pointA);
        if (d1 == 0) return new TopologySideCross {First = a, Second = b, CrossPoint = a._pointA};
        d2 = b.RelativeDistance(a._pointB);
        if (d2 == 0) return new TopologySideCross {First = a, Second = b, CrossPoint = a._pointB};
        if (d1 < 0 && d2 < 0 || d1 > 0 && d2 > 0) return null;

        var fragment = (0 - d1) / (d2 - d1);
        var realAB   = a._pointB - a._pointA;
        return new TopologySideCross {First = a, Second = b, CrossPoint = a._pointA + realAB * fragment};
    }

    /// <summary>
    ///     znajduje punkt przecięcia odcinka a z linią nieskończoną opartą na odcinku ll
    /// </summary>
    /// <param name="segment"></param>
    /// <param name="longLine"></param>
    /// <returns></returns>
    public static TopologySideCross? CrossByLL(TopologyTriangleLine? segment, TopologyTriangleLine longLine)
    {
        if (segment._pointA == segment._pointB || longLine._pointA == longLine._pointB) return null;
        var d1 = longLine.RelativeDistance(segment.PointA);
        var d2 = longLine.RelativeDistance(segment.PointB);
        if (d1 < 0 && d2 < 0 || d1 > 0 && d2 > 0) return null;
        if (d1 == 0)
            return new TopologySideCross {First = segment, Second = longLine, CrossPoint = segment.PointA};
        if (d2 == 0)
            return new TopologySideCross {First = segment, Second = longLine, CrossPoint = segment.PointB};
        var fragment = (0 - d1) / (d2 - d1);
        var vAB      = segment._pointB - segment._pointA;
        var cp       = segment._pointA + vAB * fragment;
        return new TopologySideCross {First = segment, Second = longLine, CrossPoint = cp};
    }
}