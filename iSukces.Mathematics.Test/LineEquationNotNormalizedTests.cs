using Xunit;
using iSukces.Mathematics;
using System;

namespace iSukces.Mathematics.Test;

public class LineEquationNotNormalizedTests
{
    [Fact]
    public void Horizontal_Should_CreateCorrectLine()
    {
        var line = LineEquationNotNormalized.Horizontal(5);
        Assert.Equal(0, line.A);
        Assert.Equal(-1, line.B);
        Assert.Equal(5, line.C);
        Assert.Equal("y=5", line.ToString());
    }

    [Fact]
    public void Vertical_Should_CreateCorrectLine()
    {
        var line = LineEquationNotNormalized.Vertical(10);
        Assert.Equal(-1, line.A);
        Assert.Equal(0, line.B);
        Assert.Equal(10, line.C);
        Assert.Equal("x=10", line.ToString());
    }

    [Fact]
    public void DistanceNotNormalized_Should_ReturnZeroForPointOnLine()
    {
        // Line: -1x + 0y + 10 = 0  => x = 10
        var line = LineEquationNotNormalized.Vertical(10);
        Assert.Equal(0, line.DistanceNotNormalized(10, 5));
        Assert.Equal(0, line.DistanceNotNormalized(10, -100));
    }

    [Fact]
    public void DistanceNotNormalized_Should_ReturnCorrectValueForPointOffLine()
    {
        // Line: x = 10 => -1x + 0y + 10 = 0
        var line = LineEquationNotNormalized.Vertical(10);
        // Point (12, 5) => -1*12 + 0*5 + 10 = -2
        Assert.Equal(-2, line.DistanceNotNormalized(12, 5));
    }

    [Fact]
    public void GetX_Should_ReturnCorrectValue()
    {
        // Line: -1x + 1y + 0 = 0 => x = y
        var line = new LineEquationNotNormalized(-1, 1, 0);
        Assert.Equal(5, line.GetX(5));
        Assert.Equal(-2, line.GetX(-2));
    }

    [Fact]
    public void GetY_Should_ReturnCorrectValue()
    {
        // Line: -1x + 1y + 0 = 0 => y = x
        var line = new LineEquationNotNormalized(-1, 1, 0);
        Assert.Equal(5, line.GetY(5));
        Assert.Equal(-2, line.GetY(-2));
    }

    [Fact]
    public void GetX_HorizontalLine_Should_ReturnNaN()
    {
        var line = LineEquationNotNormalized.Horizontal(5);
        Assert.True(double.IsNaN(line.GetX(5)));
    }

    [Fact]
    public void GetY_VerticalLine_Should_ReturnNaN()
    {
        var line = LineEquationNotNormalized.Vertical(10);
        Assert.True(double.IsNaN(line.GetY(10)));
    }

    [Fact]
    public void FromPointAndDeltas_Should_CreateCorrectLine()
    {
        // Point (0,0), Delta (1,1) => Line y = x => -1x + 1y + 0 = 0
        var line = LineEquationNotNormalized.FromPointAndDeltas(0, 0, 1, 1);
        Assert.Equal(-1, line.A);
        Assert.Equal(1, line.B);
        Assert.Equal(0, line.C);
    }

    [Fact]
    public void Cross_Should_FindIntersection()
    {
        // Line 1: x = 5 => -1x + 0y + 5 = 0
        var l1 = LineEquationNotNormalized.Vertical(5);
        // Line 2: y = 10 => 0x - 1y + 10 = 0
        var l2 = LineEquationNotNormalized.Horizontal(10);

        var point = LineEquationNotNormalized.Cross(l1, l2);

        Assert.NotNull(point);
        Assert.Equal(5, point.Value.X);
        Assert.Equal(10, point.Value.Y);
    }

    [Fact]
    public void Cross_ParallelLines_Should_ReturnNull()
    {
        var l1 = LineEquationNotNormalized.Vertical(5);
        var l2 = LineEquationNotNormalized.Vertical(10);

        var point = LineEquationNotNormalized.Cross(l1, l2);

        Assert.Null(point);
    }

    [Fact]
    public void CrossLineSegment_Should_ReturnIntersection()
    {
        // Segment 1: (0,0) to (10,10)
        // Segment 2: (0,10) to (10,0)
        // Intersection at (5,5)
        var p1 = new iSukces.Mathematics.Point(0, 0);
        var p2 = new iSukces.Mathematics.Point(10, 10);
        var p3 = new iSukces.Mathematics.Point(0, 10);
        var p4 = new iSukces.Mathematics.Point(10, 0);

        var result = LineEquationNotNormalized.CrossLineSegment(p1, p2, p3, p4);

        Assert.NotNull(result);
        Assert.Equal(5, result.Value.X);
        Assert.Equal(5, result.Value.Y);
    }

    [Fact]
    public void CrossLineSegment_NoIntersection_Should_ReturnNull()
    {
        // Segment 1: (0,0) to (1,1)
        // Segment 2: (2,2) to (3,3)
        var p1 = new iSukces.Mathematics.Point(0, 0);
        var p2 = new iSukces.Mathematics.Point(1, 1);
        var p3 = new iSukces.Mathematics.Point(2, 2);
        var p4 = new iSukces.Mathematics.Point(3, 3);

        var result = LineEquationNotNormalized.CrossLineSegment(p1, p2, p3, p4);

        Assert.Null(result);
    }
}
