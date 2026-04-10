// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.

using System;
using Xunit;
using Triangle = iSukces.Helix.Triangle;

namespace iSukces.Mathematics.Test;

/// <summary>
///     Tests for Triangle class
/// </summary>
public sealed class TriangleTests
{
    [Theory]
    [InlineData(3, 4, 5)]
    [InlineData(5, 12, 13)]
    [InlineData(8, 15, 17)]
    [InlineData(7, 24, 25)]
    public void T01_ComputeHypotenuse_Should_Satisfy_Pythagorean_Theorem(double a, double b, double expectedC)
    {
        // Act
        var result = Triangle.ComputeHypotenuse(a, b);

        // Assert: c² = a² + b²
        Assert.Equal(expectedC, result, 12);
        Assert.Equal(expectedC * expectedC, a * a + b * b, 12);
    }

    [Theory]
    [InlineData(5, 3, 4)]
    [InlineData(13, 5, 12)]
    public void T02_ComputeSide_Should_Satisfy_Pythagorean_Theorem(double c, double a, double expectedB)
    {
        // Act
        var result = Triangle.ComputeSide(c, a);

        // Assert: b² = c² - a²
        Assert.Equal(expectedB, result, 12);
        Assert.Equal(expectedB * expectedB, c * c - a * a, 12);
    }

    [Theory]
    [InlineData(3, 4, 5, 6)]
    [InlineData(5, 12, 13, 30)]
    [InlineData(8, 15, 17, 60)]
    [InlineData(13, 14, 15, 84)]
    public void T03_TriangleArea_Should_Match_Heron_Formula(double a, double b, double c, double expectedArea)
    {
        // Act
        var result = Triangle.TriangleArea(a, b, c);

        // Assert: Area = √[s(s-a)(s-b)(s-c)] where s = (a+b+c)/2
        Assert.Equal(expectedArea, result, 12);
    }

    [Fact]
    public void T04_TriangleArea2_Should_Return_Squared_Area()
    {
        // Arrange
        var a = 3.0;
        var b = 4.0;
        var c = 5.0;

        // Act
        var area = Triangle.TriangleArea(a, b, c);
        var area2 = Triangle.TriangleArea2(a, b, c);

        // Assert: TriangleArea = √(TriangleArea2)
        Assert.Equal(area * area, area2, 12);
    }

    [Theory]
    [InlineData(3, 4, 5, true)]
    [InlineData(1, 1, 3, false)]
    [InlineData(5, 10, 3, false)]
    public void T05_Triangle_Inequality_Should_Hold(double a, double b, double c, bool isValid)
    {
        // Act & Assert: a + b > c, a + c > b, b + c > a
        if (isValid)
        {
            Assert.True(a + b > c);
            Assert.True(a + c > b);
            Assert.True(b + c > a);
        }
        else
        {
            // At least one inequality should fail
            var allValid = (a + b > c) && (a + c > b) && (b + c > a);
            Assert.False(allValid);
        }
    }

    [Fact]
    public void T06_Height_Should_Satisfy_Area_Formula()
    {
        // Arrange
        var t = new Triangle { A = 3, B = 4, C = 5 };

        // Act & Assert: Area = (a × ha) / 2 = (b × hb) / 2 = (c × hc) / 2
        var area = t.Area;
        var areaFromA = t.A * t.HA / 2;
        var areaFromB = t.B * t.HB / 2;
        var areaFromC = t.C * t.HC / 2;

        Assert.Equal(area, areaFromA, 12);
        Assert.Equal(area, areaFromB, 12);
        Assert.Equal(area, areaFromC, 12);
    }

    [Fact]
    public void T07_All_Heights_Should_Give_Same_Area()
    {
        // Arrange
        var t = new Triangle { A = 3, B = 4, C = 5 };

        // Act
        var areaA = t.A * t.HA / 2;
        var areaB = t.B * t.HB / 2;
        var areaC = t.C * t.HC / 2;

        // Assert: ha × a / 2 = hb × b / 2 = hc × c / 2 = Area
        Assert.Equal(areaA, areaB, 12);
        Assert.Equal(areaB, areaC, 12);
        Assert.Equal(areaC, t.Area, 12);
    }

    [Fact]
    public void T08_Setting_Sides_Should_Update_Area()
    {
        // Arrange
        var t = new Triangle();

        // Act
        t.A = 3;
        t.B = 4;
        t.C = 5;

        // Assert: Area should be 6 (3-4-5 triangle)
        Assert.Equal(6, t.Area, 12);
    }

    [Fact]
    public void T09_Setting_Sides_Should_Update_Heights()
    {
        // Arrange
        var t = new Triangle();

        // Act
        t.A = 3;
        t.B = 4;
        t.C = 5;

        // Assert: Heights should be calculated correctly
        Assert.Equal(4, t.HA, 12); // height to side a (3-4-5 triangle)
        Assert.Equal(3, t.HB, 12);
        Assert.Equal(2.4, t.HC, 12);
    }

    [Theory]
    [InlineData(3, 4, 5, 3, 4, 5, true)]
    [InlineData(3, 4, 5, 4, 3, 5, false)]
    public void T10_Equality_Should_Compare_All_Sides(double a1, double b1, double c1, double a2, double b2, double c2, bool expected)
    {
        // Arrange
        var t1 = new Triangle { A = a1, B = b1, C = c1 };
        var t2 = new Triangle { A = a2, B = b2, C = c2 };

        // Act & Assert
        if (expected)
        {
            Assert.True(t1 == t2);
            Assert.False(t1 != t2);
        }
        else
        {
            Assert.False(t1 == t2);
            Assert.True(t1 != t2);
        }
    }

    [Fact]
    public void T11_Clone_Should_Create_Deep_Copy()
    {
        // Arrange
        var t1 = new Triangle { A = 3, B = 4, C = 5 };

        // Act
        var t2 = (Triangle)t1.Clone();

        // Assert
        Assert.Equal(t1.A, t2.A, 12);
        Assert.Equal(t1.B, t2.B, 12);
        Assert.Equal(t1.C, t2.C, 12);
        Assert.Equal(t1.Area, t2.Area, 12);
        Assert.True(t1 == t2);
    }

    [Fact]
    public void T12_CopyFrom_Should_Copy_All_Properties()
    {
        // Arrange
        var t1 = new Triangle { A = 3, B = 4, C = 5 };
        var t2 = new Triangle();

        // Act
        t2.CopyFrom(t1);

        // Assert
        Assert.Equal(t1.A, t2.A, 12);
        Assert.Equal(t1.B, t2.B, 12);
        Assert.Equal(t1.C, t2.C, 12);
        Assert.Equal(t1.Area, t2.Area, 12);
    }

    [Fact]
    public void T13_CopyFrom_Null_Should_Throw_ArgumentNullException()
    {
        // Arrange
        var t = new Triangle();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => t.CopyFrom(null));
    }

    [Fact]
    public void T14_Equal_Triangles_Should_Have_Same_HashCode()
    {
        // Arrange
        var t1 = new Triangle { A = 3, B = 4, C = 5 };
        var t2 = new Triangle { A = 3, B = 4, C = 5 };

        // Act & Assert
        Assert.Equal(t1.GetHashCode(), t2.GetHashCode());
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 2, 3)]
    [InlineData(3, 4, 5)]
    public void T15_Different_Triangle_Types_Should_Work(double a, double b, double c)
    {
        // Arrange & Act
        var t = new Triangle { A = a, B = b, C = c };

        // Assert: Should create valid triangle
        Assert.Equal(a, t.A, 12);
        Assert.Equal(b, t.B, 12);
        Assert.Equal(c, t.C, 12);
        Assert.True(t.Area > 0);
    }
}
