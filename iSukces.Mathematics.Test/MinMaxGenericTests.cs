// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.

using System;
using Xunit;

namespace iSukces.Mathematics.Test;

/// <summary>
///     Tests for MinMaxGeneric<T> class
/// </summary>
public sealed class MinMaxGenericTests
{
    [Theory]
    [InlineData(1, 10)]
    [InlineData(-5, 5)]
    [InlineData(0, 0)]
    public void T01_Constructor_With_Int_Should_Set_Min_And_Max(int min, int max)
    {
        // Act
        var mm = new MinMaxGeneric<int>(min, max);

        // Assert
        Assert.Equal(min, mm.Min);
        Assert.Equal(max, mm.Max);
    }

    [Theory]
    [InlineData(1.5, 10.7)]
    [InlineData(-5.3, 5.3)]
    [InlineData(0.0, 0.0)]
    public void T02_Constructor_With_Double_Should_Set_Min_And_Max(double min, double max)
    {
        // Act
        var mm = new MinMaxGeneric<double>(min, max);

        // Assert
        Assert.Equal(min, mm.Min, 12);
        Assert.Equal(max, mm.Max, 12);
    }

    [Fact]
    public void T03_Constructor_With_DateTime_Should_Work()
    {
        // Arrange
        var min = new DateTime(2020, 1, 1);
        var max = new DateTime(2025, 12, 31);

        // Act
        var mm = new MinMaxGeneric<DateTime>(min, max);

        // Assert
        Assert.Equal(min, mm.Min);
        Assert.Equal(max, mm.Max);
    }

    [Fact]
    public void T04_Constructor_With_String_Should_Work()
    {
        // Arrange
        var min = "Apple";
        var max = "Zebra";

        // Act
        var mm = new MinMaxGeneric<string>(min, max);

        // Assert
        Assert.Equal(min, mm.Min);
        Assert.Equal(max, mm.Max);
    }

    [Fact]
    public void T05_Property_Setters_Should_Update_Values_For_Int()
    {
        // Arrange
        var mm = new MinMaxGeneric<int>(1, 10);

        // Act
        mm.Min = 2;
        mm.Max = 20;

        // Assert
        Assert.Equal(2, mm.Min);
        Assert.Equal(20, mm.Max);
    }

    [Fact]
    public void T05b_Property_Setters_Should_Update_Values_For_Double()
    {
        // Arrange
        var mm = new MinMaxGeneric<double>(1.5, 10.5);

        // Act
        mm.Min = 2.5;
        mm.Max = 20.5;

        // Assert
        Assert.Equal(2.5, mm.Min, 12);
        Assert.Equal(20.5, mm.Max, 12);
    }

    [Fact]
    public void T06_Min_Property_Should_Return_Constructor_Value()
    {
        // Arrange
        var mm = new MinMaxGeneric<int>(1, 10);

        // Act & Assert
        Assert.Equal(1, mm.Min);
    }

    [Fact]
    public void T06b_Max_Property_Should_Return_Constructor_Value()
    {
        // Arrange
        var mm = new MinMaxGeneric<int>(1, 10);

        // Act & Assert
        Assert.Equal(10, mm.Max);
    }

    [Fact]
    public void T07_Constructor_Should_Accept_Min_LessThan_Max()
    {
        // Act
        var mm = new MinMaxGeneric<int>(1, 10);

        // Assert: Constructor doesn't validate min < max (as designed)
        Assert.Equal(1, mm.Min);
        Assert.Equal(10, mm.Max);
    }

    [Fact]
    public void T07b_Constructor_Should_Accept_Max_LessThan_Min()
    {
        // Act
        var mm = new MinMaxGeneric<int>(10, 1);

        // Assert: Constructor doesn't validate min < max (as designed)
        Assert.Equal(10, mm.Min);
        Assert.Equal(1, mm.Max);
    }

    [Fact]
    public void T08_Generic_Type_Must_Implement_IComparable()
    {
        // This is verified at compile time
        // The constraint `where T : IComparable<T>` ensures only IComparable<T> types are allowed

        // Arrange & Act - these should compile and work
        var intMinMax = new MinMaxGeneric<int>(1, 10);
        var doubleMinMax = new MinMaxGeneric<double>(1.5, 10.5);
        var dateTimeMinMax = new MinMaxGeneric<DateTime>(DateTime.Now, DateTime.Now.AddDays(1));

        // Assert
        Assert.NotNull(intMinMax);
        Assert.NotNull(doubleMinMax);
        Assert.NotNull(dateTimeMinMax);
    }

    [Fact]
    public void T09_Multiple_MinMaxGeneric_Can_Have_Different_Types()
    {
        // Arrange & Act
        var intMinMax = new MinMaxGeneric<int>(1, 10);
        var doubleMinMax = new MinMaxGeneric<double>(1.5, 10.5);
        var stringMinMax = new MinMaxGeneric<string>("A", "Z");

        // Assert
        Assert.Equal(1, intMinMax.Min);
        Assert.Equal(1.5, doubleMinMax.Min, 12);
        Assert.Equal("A", stringMinMax.Min);
    }

    [Fact]
    public void T10_Extreme_Int_Values_Should_Work()
    {
        // Arrange & Act
        var mm = new MinMaxGeneric<int>(int.MinValue, int.MaxValue);

        // Assert
        Assert.Equal(int.MinValue, mm.Min);
        Assert.Equal(int.MaxValue, mm.Max);
    }

    [Fact]
    public void T10b_Extreme_Double_Values_Should_Work()
    {
        // Arrange & Act
        var mm = new MinMaxGeneric<double>(double.MinValue, double.MaxValue);

        // Assert
        Assert.Equal(double.MinValue, mm.Min, 12);
        Assert.Equal(double.MaxValue, mm.Max, 12);
    }
}
