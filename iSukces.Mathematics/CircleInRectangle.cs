using System;
using System.Collections.Generic;
using System.Linq;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.Mathematics;

public sealed class CircleInRectangle
{
    void CrossH(double yy, bool sortAscending)
    {
        double y = Math.Abs(yy - Center.Y);
        if (y >= Radius || y <= -Radius) return;
        double dx = MathEx.PitagorasA(Radius, y);
        if (sortAscending)
            dx = -dx;

        double x = Center.X + dx;
        if (x > Rectangle.Left && x < Rectangle.Right)
            points.Add(new Point(x, yy));
        x = Center.X - dx;
        if (x > Rectangle.Left && x < Rectangle.Right)
            points.Add(new Point(x, yy));
    }

    void CrossV(double xx, bool sortAscending)
    {
        double x = Math.Abs(xx - Center.X);
        if (x >= Radius || x <= -Radius) return;
        double dy = MathEx.PitagorasA(Radius, x);
        if (sortAscending)
            dy = -dy;
        double y = Center.Y + dy;
        if (y > Rectangle.Top && y < Rectangle.Bottom)
            points.Add(new Point(xx, y));

        y = Center.Y - dy;
        if (y > Rectangle.Top && y < Rectangle.Bottom)
            points.Add(new Point(xx, y));
    }

    public bool InRect(Point p)
    {
        if (p.X < Rectangle.Left || p.X > Rectangle.Right) return false;
        if (p.Y < Rectangle.Top || p.Y > Rectangle.Bottom) return false;
        return true;
    }

    public SolutionTypes Solve()
    {
        CrossPoints = Array.Empty<Point>();
        if (Rectangle.IsEmpty || Radius <= 0)
            return SolutionType = SolutionTypes.PartialCross;
        if (Center.X + Radius <= Rectangle.Right && Center.X - Radius >= Rectangle.Left
                                                 && Center.Y - Radius >= Rectangle.Top &&
                                                 Center.Y + Radius <= Rectangle.Bottom)
            return SolutionType = SolutionTypes.CircleInsideRectangle;

        points = new List<Point>();
        CrossV(Rectangle.Right, true);
        CrossH(Rectangle.Bottom, false);
        CrossV(Rectangle.Left, false);
        CrossH(Rectangle.Top, true);
        CrossPoints  = points.Distinct().ToArray();
        SolutionType = CrossPoints.Length > 0 ? SolutionTypes.PartialCross : SolutionTypes.CircleOutsideRectangle;
        return SolutionType;
    }


    /// <summary>
    ///     Prostokąt opisujący koło
    /// </summary>
    public Rect Rectangle { get; set; }

    /// <summary>
    ///     środek
    /// </summary>
    public Point Center { get; set; }

    /// <summary>
    ///     promień
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    ///     typ rozwiązania; własność jest tylko do odczytu.
    /// </summary>
    public SolutionTypes SolutionType { get; private set; }

    /// <summary>
    ///     Informacja o łukach (tylko dla PartialCross); własność jest tylko do odczytu.
    /// </summary>
    public Point[] CrossPoints { get; private set; }

    List<Point> points;

    public enum SolutionTypes
    {
        CircleInsideRectangle,
        CircleOutsideRectangle,
        PartialCross
    }
}