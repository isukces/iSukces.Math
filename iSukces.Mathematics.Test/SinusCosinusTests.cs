using System;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class SinusCosinusTests
{
    [Fact]
    public void T01_FromAngleDeg_should_compute_expected_values()
    {
        var sc = SinusCosinus.FromAngleDeg(90);
        Assert.Equal(1, sc.Sin, 12);
        Assert.Equal(0, sc.Cos, 12);
    }

    [Fact]
    public void T02_FromAngleRad_should_compute_expected_values()
    {
        var sc = SinusCosinus.FromAngleRad(Math.PI / 6);
        Assert.Equal(0.5, sc.Sin, 12);
        Assert.Equal(Math.Sqrt(3) / 2, sc.Cos, 12);
        Assert.Equal(Math.Tan(Math.PI / 6), sc.Tan, 12);
    }

    [Fact]
    public void T03_ToVector_methods_should_respect_component_order()
    {
        var sc = new SinusCosinus(0.6, 0.8);
        var a  = sc.ToVectorCosSin(10, 20);
        var b  = sc.ToVectorSinCos(10, 20);
        Assert.Equal(new Vector(8, 12), a);
        Assert.Equal(new Vector(6, 16), b);
    }

    [Fact]
    public void T04_ToString_should_contain_cos_and_sin()
    {
        var sc = new SinusCosinus(0.1, 0.2);
        var s  = sc.ToString();
        Assert.Contains("cos=", s);
        Assert.Contains("sin=", s);
    }

    [Theory]
    [InlineData(0, 0, 1)]
    [InlineData(30, 0.5, 0.866025403784)]
    [InlineData(45, 0.707106781187, 0.707106781187)]
    [InlineData(60, 0.866025403784, 0.5)]
    [InlineData(90, 1, 0)]
    [InlineData(180, 0, -1)]
    public void T05_SinSquaredPlusCosSquared_Should_Equal_1(double degrees, double expectedSin, double expectedCos)
    {
        // Act
        var sc = SinusCosinus.FromAngleDeg(degrees);

        // Assert
        Assert.Equal(expectedSin, sc.Sin, 12);
        Assert.Equal(expectedCos, sc.Cos, 12);

        // Assert: sin²(α) + cos²(α) = 1
        var identity = sc.Sin * sc.Sin + sc.Cos * sc.Cos;
        Assert.Equal(1.0, identity, 12);
    }

    [Fact]
    public void T06_Factory_Methods_Should_Be_Consistent()
    {
        // Arrange
        var degrees = 45;

        // Act
        var fromDeg = SinusCosinus.FromAngleDeg(degrees);
        var fromRad = SinusCosinus.FromAngleRad(degrees * MathEx.DEGTORAD);

        // Assert
        Assert.Equal(fromDeg.Sin, fromRad.Sin, 12);
        Assert.Equal(fromDeg.Cos, fromRad.Cos, 12);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(30, 0.57735026919)]
    [InlineData(45, 1)]
    [InlineData(60, 1.73205080757)]
    public void T07_Tan_Should_Equal_Sin_Divided_By_Cos(double degrees, double expectedTan)
    {
        // Arrange
        var sc = SinusCosinus.FromAngleDeg(degrees);

        // Act & Assert
        Assert.Equal(expectedTan, sc.Tan, 9);
    }

    [Fact]
    public void T08_Tan_At_90_Degrees_Should_Be_PositiveInfinity()
    {
        // Arrange
        var sc = SinusCosinus.FromAngleDeg(90);

        // Act & Assert
        Assert.Equal(double.PositiveInfinity, sc.Tan);
    }

    [Fact]
    public void T09_Constructor_Should_Set_Sin_And_Cos()
    {
        // Arrange
        var sin = 0.707;
        var cos = 0.707;

        // Act
        var sc = new SinusCosinus(sin, cos);

        // Assert
        Assert.Equal(sin, sc.Sin, 12);
        Assert.Equal(cos, sc.Cos, 12);
    }

    [Theory]
    [InlineData(-90)]
    [InlineData(-180)]
    [InlineData(-270)]
    public void T10_Negative_Angles_Should_Work_Correctly(double degrees)
    {
        // Act
        var sc = SinusCosinus.FromAngleDeg(degrees);

        // Assert: sin² + cos² = 1 should still hold
        var identity = sc.Sin * sc.Sin + sc.Cos * sc.Cos;
        Assert.Equal(1.0, identity, 12);
    }
}
