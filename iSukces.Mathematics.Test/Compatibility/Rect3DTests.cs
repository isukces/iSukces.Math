using Xunit;

namespace iSukces.Mathematics.Test.Compatibility;

public sealed class Rect3DTests
{
    [Fact]
    public void T01_Union_should_be_not_empty()
    {
        var    p    = new Point3D(1, 2,3);
        var r1 = Rect3D.Empty;
        var r2 = r1.GetUnion(p);
        Assert.False(r2.IsEmpty);
    }
}
 