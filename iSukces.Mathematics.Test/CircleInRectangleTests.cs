using System.Linq;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class CircleInRectangleTests
{
    [Fact]
    public void T01_InRect_should_detect_inside_and_outside_points()
    {
        var s = new CircleInRectangle
        {
            Rectangle = new Rect(0, 0, 10, 20)
        };
        Assert.True(s.InRect(new Point(0, 0)));
        Assert.True(s.InRect(new Point(10, 20)));
        Assert.False(s.InRect(new Point(-0.1, 10)));
        Assert.False(s.InRect(new Point(5, 21)));
    }

    [Fact]
    public void T02_Solve_should_detect_circle_inside_rectangle()
    {
        var s = new CircleInRectangle
        {
            Rectangle = new Rect(0, 0, 10, 10),
            Center    = new Point(5, 5),
            Radius    = 4
        };
        var result = s.Solve();
        Assert.Equal(CircleInRectangle.SolutionTypes.CircleInsideRectangle, result);
        Assert.Empty(s.CrossPoints);
    }

    [Fact]
    public void T03_Solve_should_detect_circle_outside_rectangle()
    {
        var s = new CircleInRectangle
        {
            Rectangle = new Rect(0, 0, 10, 10),
            Center    = new Point(100, 100),
            Radius    = 3
        };
        var result = s.Solve();
        Assert.Equal(CircleInRectangle.SolutionTypes.CircleOutsideRectangle, result);
        Assert.Empty(s.CrossPoints);
    }

    [Fact]
    public void T04_Solve_should_detect_partial_cross_and_return_points()
    {
        var s = new CircleInRectangle
        {
            Rectangle = new Rect(0, 0, 10, 10),
            Center    = new Point(5, 5),
            Radius    = 6
        };
        var result = s.Solve();
        Assert.Equal(CircleInRectangle.SolutionTypes.PartialCross, result);
        Assert.Equal(8, s.CrossPoints.Length);
        Assert.Equal(8, s.CrossPoints.Distinct().Count());
    }
}
