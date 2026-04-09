using System;
using Xunit;

namespace iSukces.Mathematics.Test.Compatibility;

public sealed class Matrix3DTests
{
    [Fact]
    public void T01_Identity_should_have_expected_properties()
    {
        var m = Matrix3D.Identity;

        Assert.True(m.IsIdentity);
        Assert.True(m.HasInverse);
        Assert.Equal(1, m.Determinant, 12);
        Assert.Equal(new Point3D(1, 2, 3), m.Transform(new Point3D(1, 2, 3)));
        Assert.Equal(new Vector3D(1, 2, 3), m.Transform(new Vector3D(1, 2, 3)));
    }

    [Fact]
    public void T02_Translation_should_affect_point_but_not_vector()
    {
        var m = new Matrix3D(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1,
            5, 7, 11);

        Assert.Equal(new Point3D(6, 9, 14), m.Transform(new Point3D(1, 2, 3)));
        Assert.Equal(new Vector3D(1, 2, 3), m.Transform(new Vector3D(1, 2, 3)));
    }

    [Fact]
    public void T03_Multiplication_should_compose_transforms()
    {
        var scale = new Matrix3D(
            2, 0, 0,
            0, 3, 0,
            0, 0, 4,
            0, 0, 0);
        var translation = new Matrix3D(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1,
            5, 7, 11);

        var composed = scale * translation;
        var got      = composed.Transform(new Point3D(1, 1, 1));

        Assert.Equal(new Point3D(7, 10, 15), got);
    }

    [Fact]
    public void T04_GetInverted_should_round_trip_point()
    {
        var m = new Matrix3D(
            2, 0, 0,
            0, 3, 0,
            0, 0, 4,
            5, 7, 11);
        var inverse = m.GetInverted();

        var source      = new Point3D(3, 5, 7);
        var transformed = m.Transform(source);
        var roundTrip   = inverse.Transform(transformed);

        Assert.Equal(source.X, roundTrip.X, 12);
        Assert.Equal(source.Y, roundTrip.Y, 12);
        Assert.Equal(source.Z, roundTrip.Z, 12);
    }

    [Fact]
    public void T05_GetInverted_should_throw_for_singular_matrix()
    {
        var m = new Matrix3D(
            1, 2, 3,
            2, 4, 6,
            0, 0, 0,
            0, 0, 0);

        Assert.False(m.HasInverse);
        Assert.Throws<InvalidOperationException>(() => m.GetInverted());
    }

    [Fact]
    public void T06_WithOffset_should_change_only_offset()
    {
        var m = new Matrix3D(
            2, 1, 0,
            0, 3, 1,
            4, 0, 5,
            6, 7, 8);

        var got = m.WithOffset(10, 20, 30);

        Assert.Equal(m.M11, got.M11);
        Assert.Equal(m.M12, got.M12);
        Assert.Equal(m.M13, got.M13);
        Assert.Equal(m.M21, got.M21);
        Assert.Equal(m.M22, got.M22);
        Assert.Equal(m.M23, got.M23);
        Assert.Equal(m.M31, got.M31);
        Assert.Equal(m.M32, got.M32);
        Assert.Equal(m.M33, got.M33);
        Assert.Equal(10, got.OffsetX);
        Assert.Equal(20, got.OffsetY);
        Assert.Equal(30, got.OffsetZ);
    }
}
