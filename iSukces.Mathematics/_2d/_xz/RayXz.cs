using System;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics;

public readonly struct RayXz : IEquatable<RayXz>
{
    public RayXz(PointXZ origin, VectorXZ direction)
    {
        Origin    = origin;
        Direction = direction.GetNormalized();
    }

    public static RayXz operator +(RayXz ray, VectorXZ vector)
    {
        return new RayXz(ray.Origin + vector, ray.Direction);
    }

    public static bool operator ==(RayXz left, RayXz right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RayXz left, RayXz right)
    {
        return !Equals(left, right);
    }

    public bool Equals(RayXz other)
    {
        return Origin.Equals(other.Origin) && Direction.Equals(other.Direction);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj.GetType() == GetType() && Equals((RayXz)obj);
    }

    /// <summary>
    ///     Rzutuje punkt przecięcia pionowej linii na linię opisaną przez ray
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public double FromVerticalLine(XVerticalLine line)
    {
        var x      = line.X;
        var z      = GetZ(line.X);
        var dx     = x - Origin.X;
        var dz     = z - Origin.Z;
        var v2     = new VectorXZ(dx, dz);
        var result = VectorXZ.DotProduct(v2, Direction);
        return result;
    }

    public Coordinates3D GetCoordinates(double y, Vector3D vector3D)
    {
        var xV = new Vector3D(Direction.X, 0, Direction.Z);
        var yV = Coordinates3D.YVersor;
        var o  = new Point3D(Origin.X, y, Origin.Z);
        return new Coordinates3D(xV, yV, o);
    }

    public override int GetHashCode()
    {
        return (Origin.GetHashCode() * 397) ^ Direction.GetHashCode();
    }

    public PointXZ GetPoint(double x)
    {
        var z = GetZ(x);
        return new PointXZ(x, z);
    }

    private double GetZ(double x)
    {
        var line2 = LineEquation.FromPointAndDeltas(Origin.X, Origin.Z, Direction.X, Direction.Z);
        var z     = line2.GetY(x);
        return z;
    }

    public XVerticalLine MapToVerticalLine(double x)
    {
        var xAbs = Origin.X + x * Direction.X;
        return new XVerticalLine(xAbs);
    }

    public VectorXZ SwapX()
    {
        return new VectorXZ(-Direction.X, Direction.Z);
    }

    #region Properties

    public PointXZ  Origin    { get; }
    public VectorXZ Direction { get; }

    #endregion
}
