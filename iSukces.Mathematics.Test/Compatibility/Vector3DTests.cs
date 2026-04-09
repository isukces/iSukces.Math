using Xunit;

namespace iSukces.Mathematics.Test.Compatibility;

public sealed class Vector3DTests
{
    [Fact]
    public void T01_Cross_and_dot_product_should_work()
    {
        var x = new Vector3D(1, 0, 0);
        var y = new Vector3D(0, 1, 0);

        Assert.Equal(new Vector3D(0, 0, 1), Vector3D.CrossProduct(x, y));
        Assert.Equal(0, Vector3D.DotProduct(x, y), 12);
        Assert.Equal(0, x * y, 12);
    }

    [Fact]
    public void T02_AngleBetween_should_be_90_for_perpendicular_vectors()
    {
        var x = new Vector3D(1, 0, 0);
        var y = new Vector3D(0, 1, 0);

        Assert.Equal(90, Vector3D.AngleBetween(x, y), 12);
    }

    [Fact]
    public void T03_Normalize_should_return_unit_vector()
    {
        var v          = new Vector3D(3, 4, 12);
        var normalized = v.GetNormalized();

        Assert.Equal(1, normalized.Length, 12);
        Assert.Equal(3d / 13d, normalized.X, 12);
        Assert.Equal(4d / 13d, normalized.Y, 12);
        Assert.Equal(12d / 13d, normalized.Z, 12);
    }

    [Fact]
    public void T04_Matrix_multiplication_should_ignore_translation()
    {
        var v      = new Vector3D(2, 3, 4);
        var matrix = new Matrix3D(
            5, 0, 0,
            0, 7, 0,
            0, 0, 11,
            100, 200, 300);

        var byOperator = v * matrix;
        var byMethod   = matrix.Transform(v);

        Assert.Equal(new Vector3D(10, 21, 44), byOperator);
        Assert.Equal(byOperator, byMethod);
    }

    [Fact]
    public void T05_Vector_point_operators_should_work()
    {
        var vector = new Vector3D(1, 2, 3);
        var point  = new Point3D(10, 20, 30);

        Assert.Equal(new Point3D(11, 22, 33), vector + point);
        Assert.Equal(new Point3D(-9, -18, -27), vector - point);
    }
}
