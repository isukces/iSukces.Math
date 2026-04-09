using Xunit;

namespace iSukces.Mathematics.Test.Compatibility;

public sealed class RectTests
{
    [Fact]
    public void T01_Constructors_should_set_expected_values()
    {
        var byNumbers = new Rect(1, 2, 10, 20);
        Assert.Equal(1, byNumbers.X);
        Assert.Equal(2, byNumbers.Y);
        Assert.Equal(10, byNumbers.Width);
        Assert.Equal(20, byNumbers.Height);

        var byCorners = new Rect(new Point(1, 2), new Point(11, 22));
        Assert.Equal(1, byCorners.X);
        Assert.Equal(2, byCorners.Y);
        Assert.Equal(10, byCorners.Width);
        Assert.Equal(20, byCorners.Height);
    }

    [Fact]
    public void T02_Empty_should_have_expected_state()
    {
        var empty = Rect.Empty;

        Assert.True(empty.IsEmpty);
        Assert.Equal(double.NegativeInfinity, empty.Right);
        Assert.Equal(double.NegativeInfinity, empty.Bottom);
    }

    [Fact]
    public void T03_IntersectsWith_should_return_expected_values()
    {
        var r1 = new Rect(0, 0, 10, 10);
        var r2 = new Rect(5, 5, 10, 10);
        var r3 = new Rect(11, 11, 1, 1);

        Assert.True(r1.IntersectsWith(r2));
        Assert.False(r1.IntersectsWith(r3));
    }

    [Fact]
    public void T04_GetUnion_should_create_bounding_rect()
    {
        var r1 = new Rect(0, 0, 10, 10);
        var r2 = new Rect(5, 7, 20, 30);

        var got = r1.GetUnion(r2);

        Assert.Equal(new Rect(0, 0, 25, 37), got);
    }

    [Fact]
    public void T05_Corner_properties_should_be_consistent()
    {
        var r = new Rect(1, 2, 10, 20);

        Assert.Equal(new Point(1, 2), r.TopLeft);
        Assert.Equal(new Point(11, 2), r.TopRight);
        Assert.Equal(new Point(1, 22), r.BottomLeft);
        Assert.Equal(new Point(11, 22), r.BottomRight);
    }
}
