#if WPFFEATURES
using System.Windows;
#else
using iSukces.Mathematics.Compatibility;
#endif


namespace iSukces.Mathematics;

public sealed class CircleCrossInfo
{
    public CircleCrossInfo() { }

    public CircleCrossInfo(CircleLocations locations) { Locations = locations; }

    public CircleCrossInfo(CircleLocations locations, Point p)
    {
        Locations = locations;
        Points    = [p];
    }

    public CircleLocations Locations { get; set; }

    public Point[] Points { get; set; }
}
