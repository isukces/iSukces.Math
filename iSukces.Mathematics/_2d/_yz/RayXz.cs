using System;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif


namespace iSukces.Mathematics;

public readonly struct RayYZ : IEquatable<RayYZ>
{
    public RayYZ(PointYZ origin, VectorYZ direction)
    {
        Origin    = origin;
        Direction = direction.GetNormalized();
    }

    public static RayYZ operator +(RayYZ ray, VectorYZ vector)
    {
        return new RayYZ(ray.Origin + vector, ray.Direction);
    }

    public static bool operator ==(RayYZ left, RayYZ right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RayYZ left, RayYZ right)
    {
        return !Equals(left, right);
    }

    public bool Equals(RayYZ other)
    {
        return Origin.Equals(other.Origin) && Direction.Equals(other.Direction);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj.GetType() == GetType() && Equals((RayYZ)obj);
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
        var dx     = x - Origin.Y;
        var dz     = z - Origin.Z;
        var v2     = new VectorYZ(dx, dz);
        var result = VectorYZ.DotProduct(v2, Direction);
        return result;
    }

    public Coordinates3D GetCoordinates(double y, Vector3D vector3D)
    {
        var xV = new Vector3D(0, Direction.Y, Direction.Z);
        var yV = Coordinates3D.YVersor;
        var o  = Origin.To3D();
        return new Coordinates3D(xV, yV, o);
    }

    public override int GetHashCode()
    {
        return Origin.GetHashCode() ^ Direction.GetHashCode();
    }

    public PointYZ GetPoint(double x)
    {
        var z = GetZ(x);
        return new PointYZ(x, z);
    }

    private double GetZ(double x)
    {
        var line2 = LineEquation.FromPointAndDeltas(Origin.Y, Origin.Z, Direction.Y, Direction.Z);
        var z     = line2.GetY(x);
        return z;
    }


    public VectorYZ SwapY()
    {
        return new VectorYZ(-Direction.Y, Direction.Z);
    }

    #region Properties

    public PointYZ  Origin    { get; }
    public VectorYZ Direction { get; }

    #endregion
}
