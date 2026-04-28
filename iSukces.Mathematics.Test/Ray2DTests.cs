using Xunit;
using iSukces.Mathematics;
using System;

namespace iSukces.Mathematics.Test;

public class Ray2DTests
{
    [Fact]
    public void Constructor_Should_NormalizeAxis()
    {
        var begin = new Point(0, 0);
        var end = new Point(10, 0);
        var ray = new Ray2D(begin, end);

        Assert.Equal(1, ray.Axis.Length);
        Assert.Equal(1, ray.Axis.X);
        Assert.Equal(0, ray.Axis.Y);
    }

    [Fact]
    public void GetPoint_Should_ReturnCorrectPoint()
    {
        var ray = new Ray2D(new Point(0, 0), new Vector(1, 0));
        var point = ray.GetPoint(5);

        Assert.Equal(5, point.X);
        Assert.Equal(0, point.Y);
    }

    [Fact]
    public void Map_Should_ReturnCorrectDistance()
    {
        var ray = new Ray2D(new Point(0, 0), new Vector(1, 0));
        var dist = ray.Map(new Point(5, 0));

        Assert.Equal(5, dist);
    }

    [Fact]
    public void CrossLines_Should_FindIntersection()
    {
        // Ray 1: Starts at (0,0), axis (1,0) -> X axis
        var ray1 = new Ray2D(new Point(0, 0), new Vector(1, 0));
        // Ray 2: Starts at (5,-5), axis (0,1) -> Vertical line at x=5
        var ray2 = new Ray2D(new Point(5, -5), new Vector(0, 1));

        var intersect = Ray2D.CrossLines(ray1, ray2);

        Assert.Equal(5, intersect.X);
        Assert.Equal(0, intersect.Y);
    }

    [Fact]
    public void Cross_Should_ReturnCorrectIntersectionParam()
    {
        // Ray 1: X axis
        var ray1 = new Ray2D(new Point(0, 0), new Vector(1, 0));
        // Ray 2: Starts at (5,5), axis (0,-1) -> Vertical line at x=5, going down
        var ray2 = new Ray2D(new Point(5, 5), new Vector(0, -1));

        var result = ray1.Cross(ray2);

        Assert.NotNull(result);
        Assert.Equal(5, result);
    }
}
