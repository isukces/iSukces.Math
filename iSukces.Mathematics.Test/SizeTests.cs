// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.

using System;
using Xunit;

namespace iSukces.Mathematics.Test;

/// <summary>
///     Tests for Size struct
/// </summary>
public sealed class SizeTests
{
    [Theory]
    [InlineData(10, 20)]
    [InlineData(0, 0)]
    [InlineData(100.5, 200.7)]
    public void T01_Constructor_With_Valid_Arguments_Should_Succeed(double width, double height)
    {
        // Act & Assert - should not throw
        var size = new Size(width, height);
        Assert.Equal(width, size.Width, 12);
        Assert.Equal(height, size.Height, 12);
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(10, -1)]
    [InlineData(-1, -1)]
    public void T02_Constructor_With_Negative_Arguments_Should_Throw_SizeException(double width, double height)
    {
        // Act & Assert
        var ex = Assert.Throws<SizeException>(() => new Size(width, height));
        Assert.Contains(ErrorMessages.NegativeSizeArgument, ex.Message);
    }

    [Fact]
    public void T03_Empty_Should_Return_Default_Size()
    {
        // Act & Assert
        Assert.Equal(default(Size), Size.Empty);
    }

    [Fact]
    public void T04_IsEmpty_Should_Be_True_For_Default()
    {
        // Arrange
        var size = default(Size);

        // Act & Assert
        Assert.True(size.IsEmpty);
    }

    [Fact]
    public void T05_IsEmpty_Should_Be_False_For_Non_Default()
    {
        // Arrange
        var size = new Size(10, 20);

        // Act & Assert
        Assert.False(size.IsEmpty);
    }

    [Theory]
    [InlineData(10, 20)]
    [InlineData(0, 0)]
    [InlineData(100.5, 200.7)]
    public void T06_Width_And_Height_Should_Return_Constructor_Values(double width, double height)
    {
        // Arrange
        var size = new Size(width, height);

        // Act & Assert
        Assert.Equal(width, size.Width, 12);
        Assert.Equal(height, size.Height, 12);
    }

    [Fact]
    public void T07_Width_And_Height_Of_Empty_Should_Return_NegativeInfinity()
    {
        // Arrange
        var size = Size.Empty;

        // Act & Assert
        Assert.Equal(double.NegativeInfinity, size.Width);
        Assert.Equal(double.NegativeInfinity, size.Height);
    }

    [Theory]
    [InlineData(10, 20, 2, 20, 40)]
    [InlineData(10, 20, 0.5, 5, 10)]
    [InlineData(10, 20, 1, 10, 20)]
    [InlineData(10, 20, 0, 0, 0)]
    public void T08_Multiplication_Operator_Should_Scale_Size(double width, double height, double scale, double expectedWidth, double expectedHeight)
    {
        // Arrange
        var size = new Size(width, height);

        // Act
        var result1 = size * scale;
        var result2 = scale * size;

        // Assert
        Assert.Equal(expectedWidth, result1.Width, 12);
        Assert.Equal(expectedHeight, result1.Height, 12);
        Assert.Equal(expectedWidth, result2.Width, 12);
        Assert.Equal(expectedHeight, result2.Height, 12);
    }

    [Fact]
    public void T09_Multiplying_Empty_Should_Return_Empty()
    {
        // Arrange
        var empty = Size.Empty;

        // Act
        var result = empty * 2;

        // Assert
        Assert.True(result.IsEmpty);
    }

    [Fact]
    public void T10_Multiply_Obsolete_Method_Should_Match_Operator()
    {
        // Arrange
        var size = new Size(10, 20);

        // Act
        var result1 = size * 2;
        var result2 = size.Multiply(2);

        // Assert
        Assert.Equal(result1.Width, result2.Width, 12);
        Assert.Equal(result1.Height, result2.Height, 12);
    }

    [Theory]
    [InlineData(10, 20, 30, 30, 20)]
    [InlineData(10, 20, 10, 10, 20)]
    public void T11_WithWidth_Should_Create_New_Size(double width, double height, double newWidth, double expectedWidth, double expectedHeight)
    {
        // Arrange
        var size = new Size(width, height);

        // Act
        var result = size.WithWidth(newWidth);

        // Assert
        Assert.Equal(expectedWidth, result.Width, 12);
        Assert.Equal(expectedHeight, result.Height, 12);
    }

    [Theory]
    [InlineData(10, 20, 30, 10, 30)]
    [InlineData(10, 20, 20, 10, 20)]
    public void T12_WithHeight_Should_Create_New_Size(double width, double height, double newHeight, double expectedWidth, double expectedHeight)
    {
        // Arrange
        var size = new Size(width, height);

        // Act
        var result = size.WithHeight(newHeight);

        // Assert
        Assert.Equal(expectedWidth, result.Width, 12);
        Assert.Equal(expectedHeight, result.Height, 12);
    }

    [Fact]
    public void T13_With_Methods_Should_Not_Modify_Original()
    {
        // Arrange
        var original = new Size(10, 20);

        // Act
        var modified = original.WithWidth(30);

        // Assert
        Assert.Equal(10, original.Width, 12);
        Assert.Equal(30, modified.Width, 12);
    }

    [Fact]
    public void T14_Explicit_Conversion_To_Vector_Should_Work()
    {
        // Arrange
        var size = new Size(10, 20);

        // Act
        var vector = (Vector)size;

        // Assert
        Assert.Equal(10, vector.X, 12);
        Assert.Equal(20, vector.Y, 12);
    }

    [Fact]
    public void T15_Explicit_Conversion_To_Point_Should_Work()
    {
        // Arrange
        var size = new Size(10, 20);

        // Act
        var point = (Point)size;

        // Assert
        Assert.Equal(10, point.X, 12);
        Assert.Equal(20, point.Y, 12);
    }

    [Theory]
    [InlineData(10, 20, 10, 20, true)]
    [InlineData(10, 20, 20, 10, false)]
    [InlineData(10, 20, 10, 21, false)]
    public void T16_Equals_Should_Compare_Width_And_Height(double w1, double h1, double w2, double h2, bool expected)
    {
        // Arrange
        var size1 = new Size(w1, h1);
        var size2 = new Size(w2, h2);

        // Act & Assert
        Assert.Equal(expected, size1.Equals(size2));
    }

    [Fact]
    public void T17_Empty_Sizes_Should_Be_Equal()
    {
        // Arrange
        var empty1 = Size.Empty;
        var empty2 = default(Size);

        // Act & Assert
        Assert.True(empty1.Equals(empty2));
        Assert.True(empty1 == empty2);
    }

    [Fact]
    public void T18_GetHashCode_Should_Be_Consistent()
    {
        // Arrange
        var size1 = new Size(10, 20);
        var size2 = new Size(10, 20);

        // Act & Assert
        Assert.Equal(size1.GetHashCode(), size2.GetHashCode());
    }

    [Fact]
    public void T19_ToString_For_Empty_Should_Return_Empty()
    {
        // Arrange
        var size = Size.Empty;

        // Act
        var result = size.ToString();

        // Assert
        Assert.Equal("Empty", result);
    }

    [Fact]
    public void T20_ToString_For_Non_Empty_Should_Return_Width_And_Height()
    {
        // Arrange
        var size = new Size(10.5, 20.7);

        // Act
        var result = size.ToString();

        // Assert - format uses system locale separator (can be ; or ,)
        Assert.Contains("10", result);
        Assert.Contains("5", result);
        Assert.Contains("20", result);
        Assert.Contains("7", result);
    }
}
