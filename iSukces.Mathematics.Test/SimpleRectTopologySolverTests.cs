using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class SimpleRectTopologySolverTests
{
    [Theory(DisplayName = "T01 should check CommonRangeWithPositiveLength")]
    [InlineData(-2, -1, false)]
    [InlineData(-2, 0, false)]
    [InlineData(-2, 1, true)]
    [InlineData(-2, 100, true)]
    [InlineData(0, 100, true)]
    [InlineData(3, 100, true)]
    [InlineData(10, 100, false)]
    [InlineData(12, 100, false)]
    public void T01_Should_check_CommonRangeWithPositiveLength(double min, double max, bool result)
    {
        MinMax r = new MinMax(0, 10);
        var    q = r.HasCommonRangeWithPositiveLength(new DRange(min, max));
        Assert.Equal(result, q);
        q = r.HasCommonRangeWithPositiveLength(min, max);
        Assert.Equal(result, q);
    }

    [Fact]
    public void T02_RectToLines_should_return_four_edges()
    {
        var rect  = new Rect(0, 0, 10, 20);
        var lines = SimpleRectTopologySolver.RectToLines(rect);
        Assert.Equal(4, lines.Length);
    }

    [Fact]
    public void T03_GetEdges_without_holes_should_return_rectangle_edges()
    {
        var rect  = new Rect(0, 0, 10, 10);
        var edges = SimpleRectTopologySolver.GetEdges(rect, []);
        Assert.Equal(4, edges.Length);
    }

    [Fact]
    public void T04_Solve_should_return_empty_when_no_holes()
    {
        var solver = new SimpleRectTopologySolver
        {
            Source = new Range2D(new Rect(0, 0, 10, 10)),
            Holes  = []
        };
        var got = solver.Solve();
        Assert.Empty(got);
    }

    [Fact]
    public void T05_Solve_should_return_non_empty_for_inner_hole()
    {
        var solver = new SimpleRectTopologySolver
        {
            Source = new Range2D(new Rect(0, 0, 10, 10)),
            Holes = [new Rect(3, 3, 4, 4)]
        };
        var got = solver.Solve();
        Assert.NotEmpty(got);
        var area = 0d;
        foreach (var r in got)
            area += r.Width * r.Height;
        Assert.Equal(84, area, 12);
    }
}
