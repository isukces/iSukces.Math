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
}