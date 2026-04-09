using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class TopologySideCrossTests
{
    [Fact]
    public void T01_IsCrossVertex_flags_and_explicit_cast_should_work()
    {
        var l1 = new TopologyTriangleLine(new Point(0, 0), new Point(10, 0));
        var l2 = new TopologyTriangleLine(new Point(0, 0), new Point(0, 10));
        var c  = new TopologySideCross
        {
            First     = l1,
            Second    = l2,
            CrossPoint = new Point(0, 0)
        };
        Assert.True(c.IsCrossVertexOfFirst);
        Assert.True(c.IsCrossVertexOfSecond);
        Assert.Equal("**", c.CrossConfig);
        Assert.Equal(new Point(0, 0), (Point)c);
    }
}
