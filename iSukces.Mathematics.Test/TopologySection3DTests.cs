using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class TopologySection3DTests
{
    [Fact]
    public void T01_Length_should_be_computed_from_endpoints()
    {
        var s = new Section3D(new Point3D(0, 0, 0), new Point3D(3, 4, 12));
        Assert.Equal(13, s.Length, 12);
    }

    [Fact]
    public void T02_Length_should_be_recomputed_after_point_change()
    {
        var s = new Section3D(new Point3D(0, 0, 0), new Point3D(0, 0, 1));
        Assert.Equal(1, s.Length, 12);
        s.End = new Point3D(0, 0, 5);
        Assert.Equal(5, s.Length, 12);
    }
}
