#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics;

public readonly struct Ray3D : IRay3D
{
    public Ray3D(Point3D origin, Vector3D direction)
    {
        Origin    = origin;
        Direction = direction;
    }

    public static Ray3D operator +(Ray3D a, Vector3D b)
    {
        return new Ray3D(a.Origin + b, a.Direction);
    }

    public static Ray3D operator +(Ray3D a, double move)
    {
        if (move.Equals(0d))
            return a;
        var moveVector = move * a.Direction.ToNormalized();
        return new Ray3D(a.Origin + moveVector, a.Direction);
    }

    public Point3D GetNearest(Point3D point)
    {
        return Origin
               + (Vector3D.DotProduct(point - Origin, Direction) / Direction.LengthSquared
                  * Direction);
    }

    public Ray3D ToNormalized()
    {
        return new Ray3D(Origin, Direction.ToNormalized());
    }

    public override string ToString()
    {
        return $"Origin: {Origin}, Direction: {Direction}";
    }

    public Ray3D WithDirection(Vector3D newDirection)
    {
        return new Ray3D(Origin, newDirection);
    }

    public Ray3D WithOrigin(Point3D newOrigin)
    {
        return new Ray3D(newOrigin, Direction);
    }

    #region Properties


    /// <summary>
    ///     Gets or sets the direction.
    /// </summary>
    /// <value>The direction.</value>
    public Vector3D Direction { get; }

    /// <summary>
    ///     Gets or sets the origin.
    /// </summary>
    /// <value>The origin.</value>
    public Point3D Origin { get; }

    #endregion
}
