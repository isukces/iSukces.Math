using System;
using System.Linq;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class CircleOnPlaneTests
{
    [Fact]
    public void T01_Constructors_should_set_center_and_radius()
    {
        var c = new CircleOnPlane(new Point(1, 2), new Point(4, 6));
        Assert.Equal(new Point(1, 2), c.Center);
        Assert.Equal(5, c.Radius, 12);
    }

    [Fact]
    public void T02_CrossLine_should_return_two_points_for_horizontal_diameter()
    {
        var c   = new CircleOnPlane(5, new Point(0, 0));
        var got = c.CrossLine(new Point(-10, 0), new Point(10, 0))
            .OrderBy(a => a.X)
            .ToArray();
        Assert.Equal(2, got.Length);
        Assert.Equal(-5, got[0].X, 12);
        Assert.Equal(0, got[0].Y, 12);
        Assert.Equal(5, got[1].X, 12);
        Assert.Equal(0, got[1].Y, 12);
    }

    [Fact]
    public void T03_GetCrossPoints_should_detect_no_common_points()
    {
        var c1 = new CircleOnPlane(3, new Point(0, 0));
        var c2 = new CircleOnPlane(3, new Point(20, 0));
        var info = c1.GetCrossPoints(c2);
        Assert.Equal(CircleLocations.NoCommonPoints, info.Locations);
    }

    [Fact]
    public void T04_GetCrossPoints_should_detect_outside_tangent()
    {
        var c1 = new CircleOnPlane(3, new Point(0, 0));
        var c2 = new CircleOnPlane(2, new Point(5, 0));
        var info = c1.GetCrossPoints(c2);
        Assert.Equal(CircleLocations.Outside1Point, info.Locations);
        Assert.Single(info.Points);
        Assert.Equal(3, info.Points[0].X, 12);
        Assert.Equal(0, info.Points[0].Y, 12);
    }

    [Fact]
    public void T05_GetCrossPoints_should_return_two_intersection_points()
    {
        var c1 = new CircleOnPlane(5, new Point(0, 0));
        var c2 = new CircleOnPlane(5, new Point(8, 0));
        var got = c1.GetCrossPoints(c2);
        Assert.Equal(CircleLocations.Other, got.Locations);
        Assert.Equal(2, got.Points.Length);
        var points = got.Points.OrderBy(a => a.Y).ToArray();
        Assert.Equal(4, points[0].X, 12);
        Assert.Equal(-3, points[0].Y, 12);
        Assert.Equal(4, points[1].X, 12);
        Assert.Equal(3, points[1].Y, 12);
    }

    [Fact]
    public void T06_GetCrossPoints_with_ray_should_return_points_in_front_of_origin()
    {
        var c   = new CircleOnPlane(5, new Point(0, 0));
        var ray = new Ray2D(new Point(-10, 0), new Vector(1, 0));
        var got = c.GetCrossPoints(ray).OrderBy(a => a.X).ToArray();
        Assert.Equal(2, got.Length);
        Assert.Equal(-5, got[0].X, 12);
        Assert.Equal(5, got[1].X, 12);
    }

    [Fact]
    public void T07_Clone_and_equality_should_work()
    {
        var c1 = new CircleOnPlane(5, new Point(1, 2));
        var c2 = (CircleOnPlane)c1.Clone();
        Assert.True(c1 == c2);
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
        Assert.Contains("radius", c1.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}
