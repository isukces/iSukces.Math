using System.Linq;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class TopologyTriangleTests
{
    [Fact]
    public void T01_Area_center_and_boundings_should_be_correct()
    {
        var shape = TopologyShape.FromPoints(new Point(0, 0), new Point(10, 0), new Point(0, 10));
        var t     = shape.Triangles.Single();
        Assert.Equal(50, t.Area, 12);
        Assert.Equal(new Point(10d / 3d, 10d / 3d), t.Center);
        Assert.Equal(new Rect(0, 0, 10, 10), t.Boundings);
    }

    [Fact]
    public void T02_IsInside_should_detect_inside_and_outside_points()
    {
        var shape = TopologyShape.FromPoints(new Point(0, 0), new Point(10, 0), new Point(0, 10));
        var t     = shape.Triangles.Single();
        Assert.True(t.IsInside(new Point(1, 1)));
        Assert.False(t.IsInside(new Point(10, 10)));
    }

    [Fact]
    public void T03_GetInsidePoints_should_filter_points()
    {
        var shape = TopologyShape.FromPoints(new Point(0, 0), new Point(10, 0), new Point(0, 10));
        var t = shape.Triangles.Single();
        var got = t.GetInsidePoints([new Point(1, 1), new Point(5, 0), new Point(9, 9)]).ToArray();
        Assert.Single(got);
        Assert.Equal(new Point(1, 1), got[0]);
    }

    [Fact]
    public void T04_CommonLine_should_find_shared_edge()
    {
        var shape = (TopologyShape)new Rect(0, 0, 10, 10);
        var a     = shape.Triangles[0];
        var b     = shape.Triangles[1];
        var common = a.CommonLine(b);
        Assert.NotNull(common);
        Assert.True(a.HasThisTwoPoints(common.PointA, common.PointB));
        Assert.True(b.HasThisTwoPoints(common.PointA, common.PointB));
    }
}
