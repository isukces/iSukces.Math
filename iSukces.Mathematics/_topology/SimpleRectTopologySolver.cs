using System;
using System.Collections.Generic;
using System.Linq;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.Mathematics;

/// <summary>
///     Klasa dzieląca prostokąt z prostokątnymi otworami na listę różnicowych prostokątów
/// </summary>
public sealed class SimpleRectTopologySolver
{
    private static Tuple<Point, Point>[] Divide(Point p1, Point p2, Rect[] cutters)
    {
        if (p1 == p2)
            return [];
        IEnumerable<Tuple<Point, Point>> o;
        if (p1.X == p2.X)
        {
            var      x   = p1.X;
            MinMax[] src = [MinMax.From2Values(p1.Y, p2.Y)];
            var cutters2 = (
                from r in cutters
                where MinMax.From2Values(r.Left, r.Right).Includes(x)
                select MinMax.From2Values(r.Top, r.Bottom)
            ).ToArray();
            var range = MinMax.Cut(src, cutters2);
            o = from yRange in range
                select new Tuple<Point, Point>(new Point(x, yRange.Min), new Point(x, yRange.Max));
        }
        else
        {
            var      y   = p1.Y;
            MinMax[] src = [MinMax.From2Values(p1.X, p2.X)];
            var cutters2 = (
                from r in cutters
                where MinMax.From2Values(r.Top, r.Bottom).Includes(y)
                select MinMax.From2Values(r.Left, r.Right)
            ).ToArray();
            var range = MinMax.Cut(src, cutters2);

            o = from xRange in range
                select new Tuple<Point, Point>(new Point(xRange.Min, y), new Point(xRange.Max, y));
        }

        return o.ToArray();
    }

    public static Tuple<Point, Point>[] GetEdges(Rect rect, Rect[]? c)
    {
        var ol = RectToLines(rect);
        if (c is null || c.Length == 0) return ol;
        // todoi: Rozwiązać problem wyznaczania krawędzi
        var A = new List<Tuple<Point, Point>>();
        foreach (var line in ol)
            A.AddRange(Divide(line.Item1, line.Item2, c));

        if (c.Length > 0)
        {
            var xRange = MinMax.From2Values(rect.Left, rect.Right);
            var yRange = MinMax.From2Values(rect.Top, rect.Bottom);
            foreach (var cc in c)
            {
                var lines = RectToLines(cc);
                A.AddRange(lines);
                // continue;
                foreach (var l in lines)
                    if (l.Item1.Y == l.Item2.Y)
                    {
                        var y = l.Item1.Y;
                        if (yRange.Includes(y))
                        {
                            var r  = MinMax.From2Values(l.Item1.X, l.Item2.X);
                            var cr = MinMax.CommonRange(r, xRange);
                            if (!cr.IsZeroOnInvalid)
                                A.Add(new Tuple<Point, Point>(new Point(cr.Min, y), new Point(cr.Max, y)));
                        }
                    }
                    else
                    {
                        var x = l.Item1.X;
                        if (xRange.Includes(x))
                        {
                            var r  = MinMax.From2Values(l.Item1.Y, l.Item2.Y);
                            var cr = MinMax.CommonRange(r, yRange);
                            if (!cr.IsZeroOnInvalid)
                                A.Add(new Tuple<Point, Point>(new Point(x, cr.Min), new Point(x, cr.Max)));
                        }
                    }
            }
        }

        return A.ToArray();
    }

    public static Tuple<Point, Point>[] RectToLines(Rect rect)
    {
        var u     = rect.GetCorners();
        var lines = new List<Tuple<Point, Point>>();
        for (var i = 0; i < u.Length; i++)
        {
            var p1 = i == 0 ? u[u.Length - 1] : u[i - 1];
            var p2 = u[i];
            if (p1 != p2)
                lines.Add(new Tuple<Point, Point>(p1, p2));
        }

        return lines.ToArray();
        ;
    } 

    private List<Range2D> ComputeRoundedHolesInside(Range2D source)
    {
        if (Source.IsEmpty) return [];
        var a = from r in RoundedHoles
            where
                r.X1() >= source.X1() && r.X2() <= source.X2() &&
                r.Y1() >= source.Y1() && r.Y2() <= source.Y2()
            select r;
        return a.ToList();
    }

    /*// public   Point Round(Point p) { return new Point(Round(p.X), Round(p.Y)); }


     /// <summary>
     /// </summary>
     /// <param name="x"></param>
     /// <returns></returns>
     public double Round(double x) { return Math.Round(x, RoundDigits); }
     */

    public List<Rect> Solve()
    {
        var source = Source;
        if (Source.IsEmpty || Holes is null || Holes.Length == 0)
            return [];
        RoundedHoles = Holes
            .Select(a => new Range2D(a).Round())
            .ToArray();

        {
            RoundedInside = ComputeRoundedHolesInside(source);
        }

        {
            if (Source.IsEmpty)
            {
                XEdges = [];
            }
            else if (Holes is null)
            {
                XEdges =
                [
                    Source.Left(),
                    Source.Right()
                ];
            }
            else
            {
                XEdges = RoundedInside.Select(rect => rect.X1())
                    .Union(RoundedInside.Select(rect => rect.X2())) // union
                    .Where(x => Source.XRange.IncludesExclusive(x))
                    //.Where(x => x > Source.Left() && x < Source.Right())
                    .ToList();
                XEdges.Add(Source.Left());
                XEdges.Add(Source.Right());
                XEdges.Sort();
            }
        }

        var rh        = RoundedInside;
        var srcXRange = Source.XRange; // new MinMax(Source.Left(), Source.Right());
        var srcYRange = Source.YRange; // new MinMax(Source.Top(), Source.Bottom());
        var output    = new List<Rect>();
        foreach (var xRange in XRanges)
        {
            var xCenter    = xRange.Center;
            var rectXRange = rh.Where(rect => xRange.HasCommonRangeWithPositiveLength(rect.XRange)).ToList();
            var edgesY = rectXRange.Select(rect => rect.Y1())
                .Union(rectXRange.Select(rect => rect.Y2())) // union
                .Where(y => srcYRange.IncludesExclusive(y))
                .ToList();
            edgesY.Add(Source.Top());
            edgesY.Add(Source.Bottom());
            var yRanges = MinMax.RangesFromEdges(edgesY);
            foreach (var yRange in yRanges)
            {
                var rectYRange = rectXRange.Where(rect => yRange.HasCommonRangeWithPositiveLength(rect.YRange)).ToArray();
                if (rectYRange.Length > 0) continue;

                Rect a;
                if (ReverseY)
                {
                    var c = new Point(xRange.Min, SwapY(yRange.Max));
                    var d = new Point(xRange.Max, SwapY(yRange.Min));
                    a = new Rect(c.X, c.Y, d.X - c.X, d.Y - c.Y);
                }
                else
                    a = new Rect(xRange.Min, yRange.Min, xRange.Length, yRange.Length);

                output.Add(a);
            }
        }

        return output;
    }

    private double SwapY(double y)
    {
        var top    = Source.Top();
        var bottom = Source.Bottom();
        if (y == top)
            return bottom;
        if (y == bottom)
            return top;
        return top + bottom - y;
    }

    public IReadOnlyList<Range2D> RoundedHoles { get; private set; }

    /// <summary>
    ///     Prostokąty, które mają znaczenie
    /// </summary>
    public List<Range2D> RoundedInside { get; private set; }

    /// <summary>
    ///     Krawędzie cięcia pionowego wg krawędzi otworów
    /// </summary>
    public List<double> XEdges { get; private set; }

    public MinMax[] XRanges => MinMax.RangesFromEdges(XEdges);


    /// <summary>
    ///     Prostokąt źródłowy
    /// </summary>
    public Range2D Source { get; set; }

    /// <summary>
    ///     Prostokąty wycinające
    /// </summary>
    public Rect[] Holes { get; set; }

    /// <summary>
    ///     ilość miejsc po przecinku do zakrąglenia
    /// </summary>
    public int RoundDigits { get; set; } = 3;

    public bool ReverseY { get; set; }
}