using System;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class QuaternionTests
{
    [Fact]
    public void T01_Identity_should_have_expected_properties()
    {
        var q = Quaternion.Identity;
        Assert.True(q.IsIdentity);
        Assert.True(q.IsNormalized);
        Assert.Equal(0, q.X, 12);
        Assert.Equal(0, q.Y, 12);
        Assert.Equal(0, q.Z, 12);
        Assert.Equal(1, q.W, 12);
        Assert.Equal(0, q.Angle, 12);
        Assert.Equal(new Vector3D(0, 1, 0), q.Axis);
    }

    [Fact]
    public void T02_Axis_angle_constructor_should_create_expected_rotation()
    {
        var q = new Quaternion(new Vector3D(0, 0, 2), 90);
        Assert.False(q.IsIdentity);
        Assert.Equal(0, q.X, 12);
        Assert.Equal(0, q.Y, 12);
        Assert.Equal(Math.Sqrt(0.5), q.Z, 12);
        Assert.Equal(Math.Sqrt(0.5), q.W, 12);
        Assert.Equal(90, q.Angle, 12);
        Assert.Equal(new Vector3D(0, 0, 1), q.Axis);
    }

    [Fact]
    public void T03_Axis_angle_constructor_should_throw_for_zero_axis()
    {
        Assert.Throws<GeometryException>(() => new Quaternion(new Vector3D(0, 0, 0), 45));
    }

    [Fact]
    public void T04_GetConjugated_should_negate_xyz_only()
    {
        var q = new Quaternion(1, 2, 3, 4);
        var c = q.GetConjugated();
        Assert.Equal(-1, c.X, 12);
        Assert.Equal(-2, c.Y, 12);
        Assert.Equal(-3, c.Z, 12);
        Assert.Equal(4, c.W, 12);
    }

    [Fact]
    public void T05_GetInverted_should_be_multiplicative_inverse()
    {
        var q   = new Quaternion(1, 2, 3, 4);
        var inv = q.GetInverted();
        var one = q * inv;
        Assert.Equal(0, one.X, 12);
        Assert.Equal(0, one.Y, 12);
        Assert.Equal(0, one.Z, 12);
        Assert.Equal(1, one.W, 12);
    }

    [Fact]
    public void T06_GetNormalized_should_return_unit_quaternion()
    {
        var q = new Quaternion(1, 2, 3, 4);
        var n = q.GetNormalized();
        var len2 = n.X * n.X + n.Y * n.Y + n.Z * n.Z + n.W * n.W;
        Assert.Equal(1, len2, 12);
        Assert.True(n.IsNormalized);
    }

    [Fact]
    public void T07_Add_and_subtract_with_identity_should_have_expected_values()
    {
        var q = new Quaternion(1, 2, 3, 4);
        var a = q + Quaternion.Identity;
        var s = q - Quaternion.Identity;
        Assert.Equal(new Quaternion(1, 2, 3, 5), a);
        Assert.Equal(new Quaternion(1, 2, 3, 3), s);
    }

    [Fact]
    public void T08_Multiply_and_static_multiply_should_match()
    {
        var q1 = new Quaternion(1, 2, 3, 4);
        var q2 = new Quaternion(2, 3, 5, 7);
        var a  = q1 * q2;
        var b  = Quaternion.Multiply(q1, q2);
        Assert.Equal(a, b);
    }
}
