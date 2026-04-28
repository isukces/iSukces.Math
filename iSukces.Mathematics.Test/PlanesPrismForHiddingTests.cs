using Xunit;
using iSukces.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Mathematics.Test;

public class PlanesPrismForHiddingTests
{
    private PlanesPrismForHidding CreateCubePrism(double size, double zOffset = 0)
    {
        // Cube centered at (0,0,zOffset) with sides of 'size'
        // TopPlate is the top face: normal (0,0,1), center (0,0, zOffset + size/2)
        // So TopPlate equation: 0x + 0y + 1z - (zOffset + size/2) = 0  => D = -(zOffset + size/2)
        var topPlate = new Plane3D(new Point3D(0, 0, zOffset + size / 2), new Vector3D(0, 0, 1));

        // Sides are planes facing inwards (DistanceNotNormalized >= 0 is inside)
        // Side 1: x = -size/2 => 1x + 0y + 0z + size/2 = 0
        // Side 2: x = size/2  => -1x + 0y + 0z + size/2 = 0
        // Side 3: y = -size/2 => 0x + 1y + 0z + size/2 = 0
        // Side 4: y = size/2  => 0x - 1y + 0z + size/2 = 0
        var sides = new Plane3D[]
        {
            new Plane3D(1, 0, 0, size / 2),
            new Plane3D(-1, 0, 0, size / 2),
            new Plane3D(0, 1, 0, size / 2),
            new Plane3D(0, -1, 0, size / 2)
        };

        return new PlanesPrismForHidding
        {
            TopPlate = topPlate,
            Sides = sides
        };
    }

    [Fact]
    public void FindNotHidden_LineCompletelyOutside_ShouldReturnFullLine()
    {
        var prism = CreateCubePrism(10);
        var a = new Point3D(100, 100, 100);
        var b = new Point3D(110, 110, 110);

        var result = prism.FindNotHidden(a, b);

        Assert.Single(result);
        Assert.Equal(a, result[0].Begin);
        Assert.Equal(b, result[0].End);
    }

    [Fact]
    public void FindNotHidden_LineCompletelyInside_ShouldReturnEmpty()
    {
        var prism = CreateCubePrism(10);
        // Cube is from -5 to 5 on X, Y. Top plate is at Z=5.
        // a, b are below top plate and inside side planes.
        var a = new Point3D(0, 0, 0);
        var b = new Point3D(0, 0, 4);

        var result = prism.FindNotHidden(a, b);

        Assert.Empty(result);
    }

    [Fact]
    public void FindNotHidden_LineCrossingThrough_ShouldSplitLine()
    {
        var prism = CreateCubePrism(10);
        // Line from (0,0,-10) to (0,0,10).
        // It enters the "hidden" zone at Z=0 (bottom of cube - wait, the code only checks TopPlate < 0)
        // Actually, the logic in FindNotHidden:
        // 1. Finds intersections with Side planes.
        // 2. Checks if midpoint is below TopPlate AND inside all sides.

        // The cube is -5..5 X, -5..5 Y, Z < 5 (hidden area).
        // a = (0,0,0), b = (0,0,10).
        // Midpoint (0,0,5) is on TopPlate (dist=0).
        // Let's use a line that clearly passes through the volume.
        var pA = new Point3D(0, 0, 0);
        var pB = new Point3D(0, 0, 10);

        var result = prism.FindNotHidden(pA, pB);

        // Since pA(0,0,0) is inside and below topPlate (dist = 0*0 + 0*0 + 1*0 - 5 = -5 < 0),
        // and pB(0,0,10) is above topPlate (dist = 10 - 5 = 5 > 0),
        // the segment from 0 to 5 should be hidden, and 5 to 10 should be visible.

        Assert.Single(result);
        Assert.Equal(5, result[0].Begin.Z, 12);
        Assert.Equal(10, result[0].End.Z, 12);
    }

    [Fact]
    public void StaticFindNotHidden_MultiplePrisms_ShouldSieveLine()
    {
        var prism1 = CreateCubePrism(10, 0);   // Z < 5 is hidden
        var prism2 = CreateCubePrism(10, 10); // Z < 15 is hidden
        var prisms = new List<PlanesPrismForHidding> { prism1, prism2 };

        var a = new Point3D(0, 0, 0);
        var b = new Point3D(0, 0, 20);

        var result = PlanesPrismForHidding.FindNotHidden(a, b, prisms);

        // Prism 1 hides 0..5. Remaining: 5..20.
        // Prism 2 hides 10..15 (relative to its own center, but the TopPlate is Z=15).
        // Wait, if prisms are combined, only the parts NOT hidden by ANY prism survive.
        // Section 5..20 is passed to Prism 2.
        // Prism 2's TopPlate is at Z=15. Parts below 15 and inside sides are hidden.
        // So 5..15 is hidden.
        // Remaining should be 15..20.

        Assert.Single(result);
        Assert.Equal(15, result[0].Begin.Z, 12);
        Assert.Equal(20, result[0].End.Z, 12);
    }

    [Fact]
    public void FindNotHidden_NullPrisms_ShouldReturnOriginalLine()
    {
        var a = new Point3D(0, 0, 0);
        var b = new Point3D(1, 1, 1);
        var result = PlanesPrismForHidding.FindNotHidden(a, b, null);

        Assert.Single(result);
        Assert.Equal(a, result[0].Begin);
        Assert.Equal(b, result[0].End);
    }
}
