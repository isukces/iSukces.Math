using Xunit;

namespace iSukces.Mathematics.Test.Compatibility;

public sealed class PointTests
{
    [Fact]
    public void T01_Add_and_subtract_vector_should_work()
    {
        var point  = new Point(10, 20);
        var vector = new Vector(3, -5);

        Assert.Equal(new Point(13, 15), point + vector);
        Assert.Equal(new Point(7, 25), point - vector);
    }

    [Fact]
    public void T02_Subtract_point_should_return_vector()
    {
        var point1 = new Point(10, 20);
        var point2 = new Point(3, 5);

        Assert.Equal(new Vector(7, 15), point1 - point2);
    }

    [Fact]
    public void T03_Multiply_should_transform_point()
    {
        var point  = new Point(2, 3);
        var matrix = new Matrix(2, 0, 0, 3, 5, 7);

        var byMethod   = Point.Multiply(point, matrix);
        var byOperator = point * matrix;

        Assert.Equal(new Point(9, 16), byMethod);
        Assert.Equal(byMethod, byOperator);
    }

    [Fact]
    public void T04_Explicit_cast_to_vector_should_work()
    {
        var point = new Point(2, -3);
        var got   = (Vector)point;

        Assert.Equal(new Vector(2, -3), got);
    }
}
