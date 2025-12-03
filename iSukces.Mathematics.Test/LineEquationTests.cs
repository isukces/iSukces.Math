using Xunit;

namespace iSukces.Mathematics.Test;

public class LineEquationTests
{
    [Fact]
    public void T01A_Should_convert_to_string()
    {
        var line = LineEquation.FromPointAndDeltas(3, 1, 0, 13);
        Assert.Equal("x = 3", line.ToString());
    }

    [Fact]
    public void T01B_Should_convert_to_string()
    {
        var line = LineEquation.FromPointAndDeltas(3, 1, 0, -13);
        Assert.Equal("x = 3", line.ToString());
    }

    [Fact]
    public void T01C_Should_convert_to_string()
    {
        var line = LineEquation.FromPointAndDeltas(1, 3, 13, 0);
        Assert.Equal("y = 3", line.ToString());
    }

    [Fact]
    public void T01D_Should_convert_to_string()
    {
        var line = LineEquation.FromPointAndDeltas(1, 3, -13, 0);
        Assert.Equal("y = 3", line.ToString());
    }
    
    [Fact]
    public void T02_Should_create_horizontal()
    {
        var line = LineEquation.Horizontal(3);
        Assert.Equal("y = 3", line.ToString());

        var dist = line.DistanceNotNormalized(0, 3);
        Assert.Equal(0, dist);
        
        dist = line.DistanceNotNormalized(10, 3);
        Assert.Equal(0, dist);
    }
    
    [Fact]
    public void T03_Should_create_vertical()
    {
        var line = LineEquation.Vertical(2);
        Assert.Equal("x = 2", line.ToString());

        var dist = line.DistanceNotNormalized(2, 3);
        Assert.Equal(0, dist);
        
        dist = line.DistanceNotNormalized(2, 10);
        Assert.Equal(0, dist);
    }
}
