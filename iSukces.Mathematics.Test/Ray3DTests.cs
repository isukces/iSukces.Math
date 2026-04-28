using Xunit;
using iSukces.Mathematics;
using System;

namespace iSukces.Mathematics.Test;

public class Ray3DTests
{
    [Fact]
    public void Constructor_Should_SetOriginAndDirection()
    {
        var origin = new Point3D(1, 2, 3);
        var direction = new Vector3D(0, 0, 1);
        var ray = new Ray3D(origin, direction);

        Assert.Equal(origin, ray.Origin);
        Assert.Equal(direction, ray.Direction);
    }

    [Fact]
    public void GetNearest_Should_ReturnCorrectProjectedPoint()
    {
        // Ray along Z axis starting at origin
        var ray = new Ray3D(new Point3D(0, 0, 0), new Vector3D(0, 0, 1));
        var point = new Point3D(1, 1, 5);

        var nearest = ray.GetNearest(point);

        Assert.Equal(0, nearest.X);
        Assert.Equal(0, nearest.Y);
        Assert.Equal(5, nearest.Z);
    }

    [Fact]
    public void ToNormalized_Should_NormalizeDirection()
    {
        var ray = new Ray3D(new Point3D(0, 0, 0), new Vector3D(0, 0, 5));
        var normalized = ray.ToNormalized();

        Assert.Equal(1, normalized.Direction.Length);
        Assert.Equal(1, normalized.Direction.Z);
    }

    [Fact]
    public void OperatorPlus_Move_Should_ShiftOrigin()
    {
        var ray = new Ray3D(new Point3D(0, 0, 0), new Vector3D(0, 0, 1));
        var shifted = ray + 10.0;

        Assert.Equal(0, shifted.Origin.X);
        Assert.Equal(0, shifted.Origin.Y);
        Assert.Equal(10, shifted.Origin.Z);
    }

    [Fact]
    public void TryCross_Plane_Should_FindIntersection()
    {
        // Ray along Z axis starting at origin
        var ray = new Ray3D(new Point3D(0, 0, 0), new Vector3D(0, 0, 1));
        // Plane at Z=10 (normal 0,0,1, center 0,0,10)
        var plane = new Plane3D(new Point3D(0, 0, 10), new Vector3D(0, 0, 1));

        var result = ray.TryCross(plane);

        Assert.NotNull(result);
        Assert.Equal(0, result.Value.X);
        Assert.Equal(0, result.Value.Y);
        Assert.Equal(10, result.Value.Z);
    }

    [Fact]
    public void TryCross_PlaneParallel_Should_ReturnNull()
    {
        // Ray along X axis
        var ray = new Ray3D(new Point3D(0, 0, 0), new Vector3D(1, 0, 0));
        // Plane normal along X axis, but offset (parallel to ray)
        // Point (10,0,0) with normal (1,0,0) means the plane is x=10.
        // The ray is also on the X axis, so it MUST intersect at x=10.
        // To be truly parallel and NOT intersect, the ray must be parallel to the plane's surface.
        // A ray is parallel to a plane if its direction is perpendicular to the plane's normal.

        // Let's use a plane normal along Y axis, but ray along X axis.
        var plane = new Plane3D(new Point3D(0, 10, 0), new Vector3D(0, 1, 0)); // Plane y=10

        var result = ray.TryCross(plane);

        Assert.Null(result);
    }
}
