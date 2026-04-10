// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.

using System;
using Xunit;

namespace iSukces.Mathematics.Test;

/// <summary>
///     Tests for Size3D struct
/// </summary>
public sealed class Size3DTests
{
    [Theory]
    [InlineData(10, 20, 30)]
    [InlineData(0, 0, 0)]
    [InlineData(100.5, 200.7, 300.9)]
    public void T01_Constructor_With_Valid_Arguments_Should_Succeed(double x, double y, double z)
    {
        // Act & Assert - should not throw
        var size = new Size3D(x, y, z);
        Assert.Equal(x, size.X, 12);
        Assert.Equal(y, size.Y, 12);
        Assert.Equal(z, size.Z, 12);
    }

    [Theory]
    [InlineData(-1, 10, 20)]
    [InlineData(10, -1, 20)]
    [InlineData(10, 20, -1)]
    [InlineData(-1, -1, -1)]
    public void T02_Constructor_With_Negative_Arguments_Should_Throw_SizeException(double x, double y, double z)
    {
        // Act & Assert
        var ex = Assert.Throws<SizeException>(() => new Size3D(x, y, z));
        Assert.Contains(ErrorMessages.NegativeSizeArgument, ex.Message);
    }

    [Fact]
    public void T03_Empty_Should_Return_Special_Instance()
    {
        // Arrange
        var empty = Size3D.Empty;

        // Act & Assert
        Assert.True(empty.IsEmpty);
        Assert.Equal(double.NegativeInfinity, empty.X);
        Assert.Equal(double.NegativeInfinity, empty.Y);
        Assert.Equal(double.NegativeInfinity, empty.Z);
    }

    [Theory]
    [InlineData(-1, 10, 20, true)]
    [InlineData(10, -1, 20, true)]
    [InlineData(10, 20, -1, true)]
    [InlineData(10, 20, 30, false)]
    public void T04_IsEmpty_Should_Be_True_When_Any_Dimension_Is_Negative(double x, double y, double z, bool expected)
    {
        // Arrange
        var size = Size3D.TryCreate(x, y, z);

        // Act & Assert
        Assert.Equal(expected, size.IsEmpty);
    }

    [Theory]
    [InlineData(10, 20, 30)]
    public void T05_X_Y_Z_Should_Return_Constructor_Values(double x, double y, double z)
    {
        // Arrange
        var size = new Size3D(x, y, z);

        // Act & Assert
        Assert.Equal(x, size.X, 12);
        Assert.Equal(y, size.Y, 12);
        Assert.Equal(z, size.Z, 12);
    }

    [Theory]
    [InlineData(10, 20, 30, 40, 40, 20, 30)]
    [InlineData(10, 20, 30, 10, 10, 20, 30)]
    public void T06_WithX_Should_Create_New_Instance(double x, double y, double z, double newX, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var size = new Size3D(x, y, z);

        // Act
        var result = size.WithX(newX);

        // Assert
        Assert.Equal(expectedX, result.X, 12);
        Assert.Equal(expectedY, result.Y, 12);
        Assert.Equal(expectedZ, result.Z, 12);
    }

    [Theory]
    [InlineData(10, 20, 30, 40, 10, 40, 30)]
    [InlineData(10, 20, 30, 20, 10, 20, 30)]
    public void T07_WithY_Should_Create_New_Instance(double x, double y, double z, double newY, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var size = new Size3D(x, y, z);

        // Act
        var result = size.WithY(newY);

        // Assert
        Assert.Equal(expectedX, result.X, 12);
        Assert.Equal(expectedY, result.Y, 12);
        Assert.Equal(expectedZ, result.Z, 12);
    }

    [Theory]
    [InlineData(10, 20, 30, 40, 10, 20, 40)]
    [InlineData(10, 20, 30, 30, 10, 20, 30)]
    public void T08_WithZ_Should_Create_New_Instance(double x, double y, double z, double newZ, double expectedX, double expectedY, double expectedZ)
    {
        // Arrange
        var size = new Size3D(x, y, z);

        // Act
        var result = size.WithZ(newZ);

        // Assert
        Assert.Equal(expectedX, result.X, 12);
        Assert.Equal(expectedY, result.Y, 12);
        Assert.Equal(expectedZ, result.Z, 12);
    }

    [Fact]
    public void T09_With_Methods_Should_Not_Modify_Original()
    {
        // Arrange
        var original = new Size3D(10, 20, 30);

        // Act
        var modified = original.WithX(40);

        // Assert
        Assert.Equal(10, original.X, 12);
        Assert.Equal(40, modified.X, 12);
        Assert.Equal(20, original.Y, 12);
        Assert.Equal(20, modified.Y, 12);
        Assert.Equal(30, original.Z, 12);
        Assert.Equal(30, modified.Z, 12);
    }

    [Theory]
    [InlineData(10, 20, 30, 10, 20, 30, true)]
    [InlineData(10, 20, 30, 20, 10, 30, false)]
    [InlineData(10, 20, 30, 10, 20, 31, false)]
    public void T10_Equality_Operators_Should_Compare_All_Dimensions(double x1, double y1, double z1, double x2, double y2, double z2, bool expected)
    {
        // Arrange
        var size1 = new Size3D(x1, y1, z1);
        var size2 = new Size3D(x2, y2, z2);

        // Act & Assert
        if (expected)
        {
            Assert.True(size1 == size2);
            Assert.False(size1 != size2);
        }
        else
        {
            Assert.False(size1 == size2);
            Assert.True(size1 != size2);
        }
    }

    [Theory]
    [InlineData(10, 20, 30, 10, 20, 30, true)]
    [InlineData(10, 20, 30, 10, 20, 31, false)]
    public void T11_Equals_Method_Should_Compare_All_Dimensions(double x1, double y1, double z1, double x2, double y2, double z2, bool expected)
    {
        // Arrange
        var size1 = new Size3D(x1, y1, z1);
        var size2 = new Size3D(x2, y2, z2);

        // Act & Assert
        Assert.Equal(expected, size1.Equals(size2));
    }

    [Fact]
    public void T12_Equals_Object_Should_Work_Correctly()
    {
        // Arrange
        var size = new Size3D(10, 20, 30);

        // Act & Assert
        Assert.True(size.Equals((object)size));
        Assert.False(size.Equals(null));
        Assert.False(size.Equals("not a Size3D"));
    }

    [Fact]
    public void T13_Equal_Size3D_Should_Have_Same_HashCode()
    {
        // Arrange
        var size1 = new Size3D(10, 20, 30);
        var size2 = new Size3D(10, 20, 30);

        // Act & Assert
        Assert.Equal(size1.GetHashCode(), size2.GetHashCode());
    }

    [Fact]
    public void T14_HashCode_Should_Use_All_Dimensions()
    {
        // Arrange
        var size1 = new Size3D(10, 20, 30);
        var size2 = new Size3D(10, 20, 31); // Different Z

        // Act & Assert
        // Different combinations should (usually) have different hash codes
        // Note: XOR can theoretically collide, but it's unlikely
        Assert.NotEqual(size1.GetHashCode(), size2.GetHashCode());
    }

    [Fact]
    public void T15_ToString_For_Empty_Should_Return_Empty()
    {
        // Arrange
        var size = Size3D.Empty;

        // Act
        var result = size.ToString();

        // Assert
        Assert.Equal("Empty", result);
    }

    [Fact]
    public void T16_ToString_For_Non_Empty_Should_Return_X_Y_Z()
    {
        // Arrange
        var size = new Size3D(10.5, 20.7, 30.9);

        // Act
        var result = size.ToString();

        // Assert - format uses system locale separator (can be ; or ,)
        Assert.Contains("10", result);
        Assert.Contains("5", result);
        Assert.Contains("20", result);
        Assert.Contains("7", result);
        Assert.Contains("30", result);
        Assert.Contains("9", result);
    }
}
