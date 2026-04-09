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
}
