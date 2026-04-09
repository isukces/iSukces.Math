using System;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class CircleTests
{
    [Fact]
    public void T01_Radius_should_update_all_dependent_properties()
    {
        var c = new Circle(5);
        Assert.Equal(5, c.Radius, 12);
        Assert.Equal(10, c.Diameter, 12);
        Assert.Equal(Math.PI * 25, c.Area, 12);
        Assert.Equal(MathEx.DoublePI * 5, c.Perimeter, 12);
    }

    [Fact]
    public void T02_Diameter_setter_should_update_radius()
    {
        var c = new Circle();
        c.Diameter = 10;
        Assert.Equal(5, c.Radius, 12);
        Assert.Equal(10, c.Diameter, 12);
    }

    [Fact]
    public void T03_Area_setter_should_update_radius()
    {
        var c = new Circle();
        c.Area = Math.PI * 9;
        Assert.Equal(3, c.Radius, 12);
        Assert.Equal(6, c.Diameter, 12);
    }

    [Fact]
    public void T04_Perimeter_setter_should_update_radius()
    {
        var c = new Circle();
        c.Perimeter = MathEx.DoublePI * 4;
        Assert.Equal(4, c.Radius, 12);
        Assert.Equal(8, c.Diameter, 12);
    }

    [Fact]
    public void T05_Equality_should_depend_on_radius()
    {
        var a = new Circle(5);
        var b = new Circle(5);
        var c = new Circle(6);
        Assert.True(a == b);
        Assert.True(a != c);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void T06_Static_compute_methods_should_return_expected_values()
    {
        Assert.Equal(2 * Math.PI * 3, Circle.ComputePerimeter(3), 12);
        Assert.Equal(Math.PI * 9, Circle.ComputeAreaFromDiameter(6), 12);
    }
}

public sealed class CircleCrossInfoTests
{
    [Fact]
    public void T01_Constructors_should_set_locations_and_points()
    {
        var p = new Point(1, 2);
        var a = new CircleCrossInfo(CircleLocations.OtherCircleInside1Point, p);
        Assert.Equal(CircleLocations.OtherCircleInside1Point, a.Locations);
        Assert.Single(a.Points);
        Assert.Equal(p, a.Points[0]);
    }
}
