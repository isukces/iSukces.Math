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
}
