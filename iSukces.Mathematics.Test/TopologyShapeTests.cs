using System.Linq;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class TopologyShapeTests
{
    [Fact]
    public void T01_Explicit_cast_from_rect_should_create_4_points_and_2_triangles()
    {
        var shape = (TopologyShape)new Rect(0, 0, 10, 20);
        Assert.Equal(4, shape.Points.Count);
        Assert.Equal(2, shape.Triangles.Count);
    }

    [Fact]
    public void T02_AddTriangle_should_return_null_for_duplicate_indices()
    {
        var shape = new TopologyShape();
        shape.Points.Add(new Point(0, 0));
        shape.Points.Add(new Point(1, 0));
        shape.Points.Add(new Point(0, 1));
        Assert.Null(shape.AddTriangle(0, 0, 1));
    }

    [Fact]
    public void T03_AddTriangle_points_should_add_triangle_and_points()
    {
        var shape = new TopologyShape();
        var t     = shape.AddTriangle(new Point(0, 0), new Point(10, 0), new Point(0, 10));
        Assert.NotNull(t);
        Assert.Equal(3, shape.Points.Count);
        Assert.Single(shape.Triangles);
    }

    [Fact]
    public void T04_AddRectangle_should_add_two_triangles()
    {
        var shape = new TopologyShape();
        var tt = shape.AddRectangle(new Point(0, 0), new Point(0, 10), new Point(10, 10), new Point(10, 0));
        Assert.Equal(2, tt.Length);
        Assert.Equal(2, shape.Triangles.Count);
    }

    [Fact]
    public void T05_Border_should_return_closed_loop_for_rectangle_shape()
    {
        var shape  = (TopologyShape)new Rect(0, 0, 10, 10);
        var border = shape.Border;
        Assert.Single(border);
        Assert.True(border[0].Count >= 4);
    }

    [Fact]
    public void T06_Substract_with_null_or_empty_cutter_should_return_same_instance()
    {
        var shape = (TopologyShape)new Rect(0, 0, 10, 10);
        Assert.Same(shape, shape.Substract(null));
        var emptyCutter = new TopologyShape();
        Assert.Same(shape, shape.Substract(emptyCutter));
    }

    [Fact]
    public void T07_FromPoints_overloads_should_create_expected_triangle_count()
    {
        var a = TopologyShape.FromPoints(new Point(0, 0), new Point(1, 0), new Point(0, 1));
        var b = TopologyShape.FromPoints(new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, 1));
        Assert.Single(a.Triangles);
        Assert.Equal(2, b.Triangles.Count);
        Assert.True(b.Triangles[0].LineAC.IsHiddenTriangleEdge);
        Assert.True(b.Triangles[1].LineAB.IsHiddenTriangleEdge);
    }

    [Fact]
    public void T08_Optimize_should_not_increase_triangle_count()
    {
        var shape     = (TopologyShape)new Rect(0, 0, 10, 10);
        var optimized = shape.Optimize();
        Assert.True(optimized.Triangles.Count <= shape.Triangles.Count);
        Assert.True(optimized.Triangles.Count > 0);
    }
}
