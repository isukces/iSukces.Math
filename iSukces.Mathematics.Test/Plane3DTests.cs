using Xunit;

namespace iSukces.Mathematics.test;

public class Plane3DTests
{
    [Fact]
    public void T01_Cross_ForIntersectingPlanes_ShouldReturnLineOnBothPlanes()
    {
        var a = new Plane3D(1, 0, 0, -2); // x = 2
        var b = new Plane3D(0, 1, 0, -3); // y = 3

        var line = Plane3D.Cross(a, b);

        Assert.NotNull(line);
        Assert.Equal(0d, a.DistanceNotNormalized(line!.Point), 12);
        Assert.Equal(0d, b.DistanceNotNormalized(line.Point), 12);
    }

    [Fact]
    public void T02_Cross_ForParallelPlanes_ShouldReturnNull()
    {
        var a = new Plane3D(0, 0, 1, 0);  // z = 0
        var b = new Plane3D(0, 0, 1, -5); // z = 5

        var line = Plane3D.Cross(a, b);

        Assert.Null(line);
    }

    [Fact]
    public void T03_Cross_ForIdenticalPlanes_ShouldReturnNull()
    {
        var a = new Plane3D(2, -1, 4, 7);
        var b = new Plane3D(2, -1, 4, 7);

        var line = Plane3D.Cross(a, b);

        Assert.Null(line);
    }

    [Fact]
    public void T04_Division_ForTranslationAlongNormal_ShouldUpdateD()
    {
        var src = new Plane3D(0, 0, 1, 2); // z = -2
        var c = Coordinates3D.FromTranslate(0, 0, 5);

        var got = src / c;

        Assert.Equal(0d, got.A, 12);
        Assert.Equal(0d, got.B, 12);
        Assert.Equal(1d, got.C, 12);
        Assert.Equal(7d, got.D, 12);
    }

    [Fact]
    public void T05_Division_ForTranslationOrthogonalToNormal_ShouldKeepPlane()
    {
        var src = new Plane3D(0, 0, 1, 2);
        var c = Coordinates3D.FromTranslate(0, 5, 0); // zerowe składowe X/Z

        var got = src / c;

        Assert.Equal(src.A, got.A, 12);
        Assert.Equal(src.B, got.B, 12);
        Assert.Equal(src.C, got.C, 12);
        Assert.Equal(src.D, got.D, 12);
    }

    [Fact]
    public void T06_Division_ForNullCoordinates_ShouldReturnCloneWithSameCoefficients()
    {
        var src = new Plane3D(1.5, -2.5, 3.5, -4.5);

        var got = src / null;

        Assert.NotSame(src, got);
        Assert.Equal(src.A, got.A, 12);
        Assert.Equal(src.B, got.B, 12);
        Assert.Equal(src.C, got.C, 12);
        Assert.Equal(src.D, got.D, 12);
    }

    [Fact]
    public void T07_Normalize_ForNonZeroNormal_ShouldProduceUnitLength()
    {
        var p = new Plane3D(3, 4, 12, 10);

        p.Normalize();

        Assert.Equal(1d, p.Normal.Length, 12);
    }

    [Fact]
    public void T08_Normalize_ShouldKeepGeometricallyEquivalentPlane()
    {
        var p = new Plane3D(3, 4, 12, -39);
        var pointOnPlane = new Point3D(1, 3, 2); // 3 + 12 + 24 - 39 = 0

        p.Normalize();

        Assert.Equal(0d, p.DistanceNotNormalized(pointOnPlane), 12);
    }

    [Fact]
    public void T09_Normalize_ForZeroNormal_ShouldFollowImplementationAndProduceNaN()
    {
        var p = new Plane3D(0, 0, 0, 0);

        p.Normalize();

        Assert.True(double.IsNaN(p.A));
        Assert.True(double.IsNaN(p.B));
        Assert.True(double.IsNaN(p.C));
        Assert.True(double.IsNaN(p.D));
    }
}
