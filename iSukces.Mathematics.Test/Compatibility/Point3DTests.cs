using Xunit;

namespace iSukces.Mathematics.Test.Compatibility;

public sealed class Point3DTests
{
    [Fact]
    public void T01_Add_and_subtract_vector_should_work()
    {
        var point  = new Point3D(10, 20, 30);
        var vector = new Vector3D(3, -5, 7);

        Assert.Equal(new Point3D(13, 15, 37), point + vector);
        Assert.Equal(new Point3D(7, 25, 23), point - vector);
    }

    [Fact]
    public void T02_Subtract_point_should_return_vector()
    {
        var point1 = new Point3D(10, 20, 30);
        var point2 = new Point3D(3, 5, 11);

        Assert.Equal(new Vector3D(7, 15, 19), point1 - point2);
    }

    [Fact]
    public void T03_Explicit_cast_to_vector_should_work()
    {
        var point = new Point3D(2, -3, 4);
        var got   = (Vector3D)point;

        Assert.Equal(new Vector3D(2, -3, 4), got);
    }

    [Fact]
    public void T04_Matrix_multiplication_should_transform_point()
    {
        var point  = new Point3D(1, 1, 1);
        var matrix = new Matrix3D(
            2, 0, 0,
            0, 3, 0,
            0, 0, 4,
            5, 7, 11);

        Assert.Equal(new Point3D(7, 10, 15), point * matrix);
    }
}
