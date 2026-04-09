using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class Line3DTests
{
    [Fact]
    public void T01_Constructor_from_two_points_should_normalize_direction()
    {
        var line = new Line3D(new Point3D(1, 2, 3), new Point3D(1, 2, 8));
        Assert.Equal(new Point3D(1, 2, 3), line.Point);
        Assert.Equal(0, line.Vector.X, 12);
        Assert.Equal(0, line.Vector.Y, 12);
        Assert.Equal(1, line.Vector.Z, 12);
        Assert.Equal(1, line.Vector.Length, 12);
    }

    [Fact]
    public void T02_Constructor_from_point_and_vector_should_keep_vector()
    {
        var v    = new Vector3D(2, 3, 6);
        var line = new Line3D(new Point3D(1, 2, 3), v);
        Assert.Equal(v, line.Vector);
    }

    [Fact]
    public void T03_Multiply_with_null_coordinates_should_return_same_values()
    {
        var line   = new Line3D(new Point3D(1, 2, 3), new Vector3D(4, 5, 6));
        var result = line * null;
        Assert.Equal(line.Point, result.Point);
        Assert.Equal(line.Vector, result.Vector);
    }
}
