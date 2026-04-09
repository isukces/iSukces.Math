using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class TopologyTriangleLineTests
{
    [Fact]
    public void T01_Equals_should_treat_reversed_points_as_equal()
    {
        var a = new TopologyTriangleLine(new Point(0, 0), new Point(10, 0));
        var b = new TopologyTriangleLine(new Point(10, 0), new Point(0, 0));
        Assert.True(a.Equals(b));
        Assert.False(a == b);
    }

    [Fact]
    public void T02_Distance_should_have_expected_sign()
    {
        var line = new TopologyTriangleLine(new Point(0, 0), new Point(10, 0));
        Assert.Equal(1, line.Distance(new Point(0, 1)), 12);
        Assert.Equal(-1, line.Distance(new Point(0, -1)), 12);
    }

    [Fact]
    public void T03_GetOtherVertex_and_HasThisTwoPoints_should_work()
    {
        var a = new Point(1, 2);
        var b = new Point(3, 4);
        var line = new TopologyTriangleLine(a, b);
        Assert.Equal(b, line.GetOtherVertex(a));
        Assert.True(line.HasThisTwoPoints(a, b));
        Assert.True(line.HasThisTwoPoints(b, a));
    }

    [Fact]
    public void T04_Cross_should_return_intersection_point()
    {
        var a = new TopologyTriangleLine(new Point(0, 0), new Point(10, 0));
        var b = new TopologyTriangleLine(new Point(5, -5), new Point(5, 5));
        var c = TopologyTriangleLine.Cross(a, b);
        Assert.NotNull(c);
        Assert.Equal(new Point(5, 0), c.CrossPoint);
    }

    [Fact]
    public void T05_Cross_should_return_null_for_non_intersecting_segments()
    {
        var a = new TopologyTriangleLine(new Point(0, 0), new Point(1, 0));
        var b = new TopologyTriangleLine(new Point(5, -5), new Point(5, 5));
        Assert.Null(TopologyTriangleLine.Cross(a, b));
    }

    [Fact]
    public void T06_CrossByLL_should_intersect_with_infinite_line()
    {
        var segment  = new TopologyTriangleLine(new Point(0, 0), new Point(10, 0));
        var longLine = new TopologyTriangleLine(new Point(5, -100), new Point(5, 100));
        var c = TopologyTriangleLine.CrossByLL(segment, longLine);
        Assert.NotNull(c);
        Assert.Equal(new Point(5, 0), c.CrossPoint);
    }
}
