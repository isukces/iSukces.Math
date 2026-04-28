using Xunit;
using iSukces.Mathematics;
using System;

namespace iSukces.Mathematics.Test;

public class Matrix3DTests
{
    [Fact]
    public void Identity_Should_BeCorrect()
    {
        var identity = Matrix3D.Identity;
        Assert.True(identity.IsIdentity);
        Assert.Equal(1.0, identity.M11);
        Assert.Equal(1.0, identity.M22);
        Assert.Equal(1.0, identity.M33);
        Assert.Equal(0.0, identity.OffsetX);
        Assert.Equal(0.0, identity.OffsetY);
        Assert.Equal(0.0, identity.OffsetZ);
    }

    [Fact]
    public void Constructor_TranslationOnly_Should_SetIsIdentityFalse()
    {
        var matrix = new Matrix3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 10, 20, 30);
        Assert.False(matrix.IsIdentity);
        Assert.Equal(10, matrix.OffsetX);
        Assert.Equal(20, matrix.OffsetY);
        Assert.Equal(30, matrix.OffsetZ);
    }

    [Fact]
    public void TransformPoint_Translation_Should_ShiftPoint()
    {
        var matrix = new Matrix3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 10, 20, 30);
        var point = new Point3D(1, 1, 1);

        var result = matrix.Transform(point);

        Assert.Equal(11, result.X);
        Assert.Equal(21, result.Y);
        Assert.Equal(31, result.Z);
    }

    [Fact]
    public void TransformVector_Translation_Should_NotShiftVector()
    {
        var matrix = new Matrix3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 10, 20, 30);
        var vector = new Vector3D(1, 2, 3);

        var result = matrix.Transform(vector);

        Assert.Equal(1, result.X);
        Assert.Equal(2, result.Y);
        Assert.Equal(3, result.Z);
    }

    [Fact]
    public void Transform_Scaling_Should_ScalePoint()
    {
        // Scale X by 2, Y by 3, Z by 4
        var matrix = new Matrix3D(2, 0, 0, 0, 3, 0, 0, 0, 4, 0, 0, 0);
        var point = new Point3D(1, 1, 1);

        var result = matrix.Transform(point);

        Assert.Equal(2, result.X);
        Assert.Equal(3, result.Y);
        Assert.Equal(4, result.Z);
    }

    [Fact]
    public void Multiplication_Identity_Should_ReturnSameMatrix()
    {
        var m1 = new Matrix3D(2, 0, 0, 0, 3, 0, 0, 0, 4, 10, 20, 30);
        var identity = Matrix3D.Identity;

        var res1 = m1 * identity;
        var res2 = identity * m1;

        Assert.Equal(m1, res1);
        Assert.Equal(m1, res2);
    }

    [Fact]
    public void Multiplication_TwoTranslations_Should_Combine()
    {
        var t1 = new Matrix3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 10, 0, 0);
        var t2 = new Matrix3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 20, 0);

        var result = t1 * t2;

        Assert.Equal(10, result.OffsetX);
        Assert.Equal(20, result.OffsetY);
        Assert.Equal(0, result.OffsetZ);
    }

    [Fact]
    public void Transform_RotationX_Should_RotatePoint()
    {
        // Rotation 90 deg around X: Y becomes Z, Z becomes -Y
        // Matrix: 1 0 0 | 0 0 1 | 0 -1 0 | 0 0 0
        var matrix = new Matrix3D(
            1, 0, 0,
            0, 0, 1,
            0, -1, 0,
            0, 0, 0);

        var point = new Point3D(0, 1, 0); // Point on Y axis
        var result = matrix.Transform(point);

        Assert.Equal(0, result.X);
        Assert.Equal(0, result.Y);
        Assert.Equal(1, result.Z); // Should move to Z axis
    }

    [Fact]
    public void Equals_Should_CompareValuesCorrectly()
    {
        var m1 = new Matrix3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 10, 20, 30);
        var m2 = new Matrix3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 10, 20, 30);
        var m3 = new Matrix3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0);

        Assert.True(m1.Equals(m2));
        Assert.False(m1.Equals(m3));
    }

    [Fact]
    public void WithOffset_Should_CreateNewMatrixWithUpdatedOffset()
    {
        var m1 = Matrix3D.Identity;
        var m2 = m1.WithOffset(10, 20, 30);

        Assert.Equal(10, m2.OffsetX);
        Assert.Equal(20, m2.OffsetY);
        Assert.Equal(30, m2.OffsetZ);
        Assert.True(m1.IsIdentity); // Original should remain identity
    }
}
