// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.

using System;
using Xunit;

namespace iSukces.Mathematics.Test;

/// <summary>
///     Tests for AngleInfo trigonometric class
/// </summary>
public sealed class AngleInfoTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(180)]
    [InlineData(270)]
    [InlineData(360)]
    [InlineData(-45)]
    [InlineData(-90)]
    public void T01_SinSquaredPlusCosSquared_Should_Equal_1(double degrees)
    {
        // Arrange
        var angle = new AngleInfo(degrees);

        // Act
        var identity = angle.Sin * angle.Sin + angle.Cos * angle.Cos;

        // Assert: sin²(α) + cos²(α) = 1
        Assert.Equal(1.0, identity, 12);
    }

    [Fact]
    public void T02_DegreesToRadiansConversion_Should_Be_Precise()
    {
        // Test specific angles
        var tests = new[]
        {
            (degrees: 0.0, expectedRadians: 0.0),
            (degrees: 90.0, expectedRadians: Math.PI / 2),
            (degrees: 180.0, expectedRadians: Math.PI),
            (degrees: 270.0, expectedRadians: 3 * Math.PI / 2),
            (degrees: 360.0, expectedRadians: 2 * Math.PI)
        };

        foreach (var (degrees, expectedRadians) in tests)
        {
            var angle = new AngleInfo(degrees);
            Assert.Equal(expectedRadians, angle.Radians, 12);
        }
    }

    [Fact]
    public void T03_RadiansToDegreesConversion_Should_Be_Precise()
    {
        // Test radian to degree conversion
        var tests = new[]
        {
            (radians: 0.0, expectedDegrees: 0.0),
            (radians: Math.PI / 2, expectedDegrees: 90.0),
            (radians: Math.PI, expectedDegrees: 180.0),
            (radians: 3 * Math.PI / 2, expectedDegrees: 270.0),
            (radians: 2 * Math.PI, expectedDegrees: 360.0)
        };

        foreach (var (radians, expectedDegrees) in tests)
        {
            var angle = new AngleInfo(0);
            angle.Radians = radians;
            Assert.Equal(expectedDegrees, angle.Degrees, 12);
        }
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(90, 0, 1)]
    [InlineData(180, -1, 0)]
    [InlineData(270, 0, -1)]
    [InlineData(360, 1, 0)]
    public void T04_VectorX_Should_Return_UnitVector(double degrees, double expectedX, double expectedY)
    {
        // Arrange
        var angle = new AngleInfo(degrees);

        // Act
        var vector = angle.VectorX;

        // Assert: VectorX = [Cos, Sin]
        Assert.Equal(expectedX, vector.X, 12);
        Assert.Equal(expectedY, vector.Y, 12);
    }

    [Theory]
    [InlineData(0, 0, 1)]
    [InlineData(90, -1, 0)]
    [InlineData(180, 0, -1)]
    [InlineData(270, 1, 0)]
    public void T05_VectorY_Should_Be_Orthogonal_To_VectorX(double degrees, double expectedX, double expectedY)
    {
        // Arrange
        var angle = new AngleInfo(degrees);

        // Act
        var vectorX = angle.VectorX;
        var vectorY = angle.VectorY;

        // Assert: VectorX · VectorY = 0 (orthogonality)
        var dotProduct = vectorX.X * vectorY.X + vectorX.Y * vectorY.Y;
        Assert.Equal(0, dotProduct, 12);
        Assert.Equal(expectedX, vectorY.X, 12);
        Assert.Equal(expectedY, vectorY.Y, 12);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(45, 1)]
    [InlineData(30, 0.57735026919)]
    public void T06_Tan_Should_Handle_Special_Cases(double degrees, double expectedTan)
    {
        // Arrange
        var angle = new AngleInfo(degrees);

        // Act & Assert
        Assert.Equal(expectedTan, angle.Tan, 12);
    }

    [Fact]
    public void T07_Tan_At_90_Degrees_Should_Be_PositiveInfinity()
    {
        // Arrange
        var angle = new AngleInfo(90);

        // Act & Assert
        Assert.Equal(double.PositiveInfinity, angle.Tan);
    }

    [Fact]
    public void T08_CTan_At_0_Degrees_Should_Be_PositiveInfinity()
    {
        // Arrange
        var angle = new AngleInfo(0);

        // Act & Assert
        Assert.Equal(double.PositiveInfinity, angle.CTan);
    }

    [Theory]
    [InlineData(1, 4, 90)]
    [InlineData(1, 2, 180)]
    [InlineData(1, 1, 360)]
    [InlineData(0, 4, 0)]
    public void T09_FromAngleNr_Should_Create_Correct_Angle(int i, int nr, double expectedDegrees)
    {
        // Act
        var angle = AngleInfo.FromAngleNr(i, nr);

        // Assert
        Assert.Equal(expectedDegrees, angle.Degrees, 12);
    }

    [Theory]
    [InlineData(0, 4)]
    [InlineData(4, 4)]
    public void T10_FromAngleNr_Edge_Cases_Should_Set_Exact_Values(int i, int nr)
    {
        // Act
        var angle = AngleInfo.FromAngleNr(i, nr);

        // Assert: When i == 0 or i == nr, Sin=0, Cos=1, Tan=0
        Assert.Equal(0, angle.Sin, 12);
        Assert.Equal(1, angle.Cos, 12);
        Assert.Equal(0, angle.Tan, 12);
    }

    [Fact]
    public void T11_Multiplication_Operator_Should_Scale_Angle()
    {
        // Arrange
        var angle = new AngleInfo(45);

        // Act
        var result = angle * 2;

        // Assert
        Assert.Equal(90, result.Degrees, 12);
    }

    [Fact]
    public void T12_Subtraction_Operator_Double_Should_Work()
    {
        // Arrange
        var angle = new AngleInfo(30);

        // Act
        var result = 90 - angle;

        // Assert
        Assert.Equal(60, result.Degrees, 12);
    }

    [Fact]
    public void T13_Subtraction_Operator_Int_Should_Work()
    {
        // Arrange
        var angle = new AngleInfo(30);

        // Act
        var result = 180 - angle;

        // Assert
        Assert.Equal(150, result.Degrees, 12);
    }

    [Theory]
    [InlineData(45, 45, true)]
    [InlineData(45, 90, false)]
    [InlineData(0, 360, false)]
    public void T14_Equality_Operators_Should_Compare_Degrees(double a, double b, bool expected)
    {
        // Arrange
        var angle1 = new AngleInfo(a);
        var angle2 = new AngleInfo(b);

        // Act & Assert
        if (expected)
        {
            Assert.True(angle1 == angle2);
            Assert.False(angle1 != angle2);
        }
        else
        {
            Assert.False(angle1 == angle2);
            Assert.True(angle1 != angle2);
        }
    }

    [Fact]
    public void T15_DegreesSetter_Should_Update_All_Trig_Values()
    {
        // Arrange
        var angle = new AngleInfo(0);

        // Act
        angle.Degrees = 45;

        // Assert
        Assert.Equal(45, angle.Degrees, 12);
        Assert.Equal(0.707106781187, angle.Sin, 12);
        Assert.Equal(0.707106781187, angle.Cos, 12);
    }

    [Fact]
    public void T16_RadiansSetter_Should_Update_All_Trig_Values()
    {
        // Arrange
        var angle = new AngleInfo(0);

        // Act
        angle.Radians = Math.PI / 4; // 45 degrees

        // Assert
        Assert.Equal(45, angle.Degrees, 12);
        Assert.Equal(0.707106781187, angle.Sin, 12);
        Assert.Equal(0.707106781187, angle.Cos, 12);
    }

    [Fact]
    public void T17_ImplicitConversion_Should_Work_Both_Ways()
    {
        // Arrange
        AngleInfo angle = 90;

        // Act
        double degrees = angle;

        // Assert
        Assert.Equal(90, degrees, 12);
        Assert.Equal(90, angle.Degrees, 12);
    }

    [Fact]
    public void T18_ToString_Should_Return_Formatted_String()
    {
        // Arrange
        var angle = new AngleInfo(45);

        // Act
        var result = angle.ToString();

        // Assert: format "AngleInfo: {Degrees}º, sin={Sin}, cos={Cos}"
        Assert.Contains("45", result);
        Assert.Contains("sin=", result);
        Assert.Contains("cos=", result);
    }

    [Fact]
    public void T19_Transform_Should_Apply_Function()
    {
        // Arrange
        var angle = new AngleInfo(45);

        // Act
        var result = angle.Transform(a => a.Degrees * 2);

        // Assert
        Assert.Equal(90, result, 12);
    }

    [Theory]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(60)]
    [InlineData(120)]
    public void T20_VectorX_And_VectorY_Should_Be_Orthogonal(double degrees)
    {
        // Arrange
        var angle = new AngleInfo(degrees);

        // Act
        var vectorX = angle.VectorX;
        var vectorY = angle.VectorY;

        // Assert: VectorX · VectorY = 0 for any angle
        var dotProduct = vectorX.X * vectorY.X + vectorX.Y * vectorY.Y;
        Assert.Equal(0, dotProduct, 12);
    }

    [Fact]
    public void T21_V_MinusSin_Cos_Should_Return_Correct_Vector()
    {
        // Arrange
        var angle = new AngleInfo(45);

        // Act
        var vector = angle.V_MinusSin_Cos;

        // Assert: [-sin, cos]
        Assert.Equal(-0.707106781187, vector.X, 12);
        Assert.Equal(0.707106781187, vector.Y, 12);
    }

    [Fact]
    public void T22_FromTan_Should_Create_Angle_From_Y_X()
    {
        // Act
        var angle = AngleInfo.FromTan(1, 1); // 45 degrees

        // Assert
        Assert.Equal(45, angle.Degrees, 12);
    }

    [Fact]
    public void T23_Equals_Object_Should_Work_Correctly()
    {
        // Arrange
        var angle1 = new AngleInfo(45);
        var angle2 = new AngleInfo(45);

        // Act & Assert
        Assert.True(angle1.Equals((object)angle2));
        Assert.False(angle1.Equals(null));
        Assert.False(angle1.Equals("not an angle"));
    }

    [Fact]
    public void T24_GetHashCode_Should_Be_Consistent()
    {
        // Arrange
        var angle1 = new AngleInfo(45);
        var angle2 = new AngleInfo(45);

        // Act & Assert
        Assert.Equal(angle1.GetHashCode(), angle2.GetHashCode());
    }

    [Fact]
    public void T25_Subtract_Static_Method_Should_Work()
    {
        // Arrange
        var angle = new AngleInfo(30);

        // Act
        var result = AngleInfo.Substract(90, angle);

        // Assert
        Assert.Equal(60, result.Degrees, 12);
    }
}
