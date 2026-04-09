using Xunit;

namespace iSukces.Mathematics.Test.Compatibility;

public sealed class VectorTests
{
    [Fact]
    public void T01_Basic_operators_should_work()
    {
        var v1 = new Vector(2, 3);
        var v2 = new Vector(-5, 7);

        Assert.Equal(new Vector(-3, 10), v1 + v2);
        Assert.Equal(new Vector(7, -4), v1 - v2);
        Assert.Equal(new Vector(-2, -3), -v1);
        Assert.Equal(new Vector(4, 6), v1 * 2);
        Assert.Equal(new Vector(4, 6), 2 * v1);
        Assert.Equal(new Vector(1, 1.5), v1 / 2);
    }

    [Fact]
    public void T02_Angle_cross_and_dot_should_be_correct()
    {
        var x = new Vector(1, 0);
        var y = new Vector(0, 1);

        Assert.Equal(90, Vector.AngleBetween(x, y), 12);
        Assert.Equal(1, Vector.CrossProduct(x, y), 12);
        Assert.Equal(0, x * y, 12);
    }

    [Fact]
    public void T03_Normalize_should_return_unit_vector()
    {
        var v          = new Vector(3, 4);
        var normalized = v.GetNormalized();

        Assert.Equal(1, normalized.Length, 12);
        Assert.Equal(0.6, normalized.X, 12);
        Assert.Equal(0.8, normalized.Y, 12);
    }

    [Fact]
    public void T04_Matrix_transform_should_ignore_translation()
    {
        var v      = new Vector(3, 4);
        var matrix = new Matrix(2, 0, 0, 3, 100, 200);

        var byOperator = v * matrix;
        var byMethod   = matrix.Transform(v);

        Assert.Equal(new Vector(6, 12), byOperator);
        Assert.Equal(byOperator, byMethod);
    }
}
