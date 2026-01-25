// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

/// <summary>
///     Quaternions.
///     Quaternions are distinctly 3D entities that represent rotation in three dimensions.
///     Their power comes in being able to interpolate (and thus animate) between
///     quaternions to achieve a smooth, reliable interpolation.
///     The default quaternion is the identity.
/// </summary>
public readonly record struct Quaternion : IFormattable
{
    //------------------------------------------------------
    //
    //  Constructors
    //
    //------------------------------------------------------

    /// <summary>
    ///     Constructor that sets quaternion's initial values.
    /// </summary>
    /// <param name="x">Value of the X coordinate of the new quaternion.</param>
    /// <param name="y">Value of the Y coordinate of the new quaternion.</param>
    /// <param name="z">Value of the Z coordinate of the new quaternion.</param>
    /// <param name="w">Value of the W coordinate of the new quaternion.</param>
    public Quaternion(double x, double y, double z, double w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
        var isIdentity = _x == 0 && _y == 0 && _z == 0 && _w == 1;
        if (isIdentity)
        {
            _type = QuaternionType.Identity;
            _w    = 0;
        }
        else
            _type = QuaternionType.NotIdentity;
    }


    /// <summary>
    ///     Constructs a quaternion via specified axis of rotation and an angle.
    ///     Throws an InvalidOperationException if given (0,0,0) as axis vector.
    /// </summary>
    /// <param name="axisOfRotation">Vector representing axis of rotation.</param>
    /// <param name="angleInDegrees">Angle to turn around the given axis (in degrees).</param>
    public Quaternion(Vector3D axisOfRotation, double angleInDegrees)
    {
        angleInDegrees %= 360.0;
        // Doing the modulo before converting to radians reduces total error
        var angleInRadians = angleInDegrees * (Math.PI / 180.0);
        var length         = axisOfRotation.Length;
        if (length == 0)
            throw new InvalidOperationException("ZeroAxisSpecified");
        var v = axisOfRotation / length * Math.Sin(0.5 * angleInRadians);
        _x = v.X;
        _y = v.Y;
        _z = v.Z;
        _w = Math.Cos(0.5 * angleInRadians);

        var isIdentity = _x == 0 && _y == 0 && _z == 0 && _w == 1;
        if (isIdentity)
        {
            _type = QuaternionType.Identity;
            _w    = 0;
        }
        else
            _type = QuaternionType.NotIdentity;
    }


    //------------------------------------------------------
    //
    //  Public Methods
    //
    //------------------------------------------------------

    /// <summary>
    ///     Identity quaternion
    /// </summary>
    public static Quaternion Identity => s_identity;

    /// <summary>
    ///     Retrieves quaternion's axis.
    /// </summary>
    public Vector3D Axis
    {
        // q = M [cos(Q/2), sin(Q /2)v]
        // axis = sin(Q/2)v
        // angle = cos(Q/2)
        // M is magnitude
        get
        {
            // Handle identity (where axis is indeterminate) by
            // returning arbitrary axis.
            if (_type == QuaternionType.Identity)
                return new Vector3D(0, 1, 0);
            var v = new Vector3D(_x, _y, _z);
            v = v.GetNormalized();
            return v;
        }
    }

    /// <summary>
    ///     Retrieves quaternion's angle.
    /// </summary>
    public double Angle
    {
        get
        {
            if (_type == QuaternionType.Identity)
            {
                return 0;
            }

            // Magnitude of quaternion times sine and cosine
            var msin = Math.Sqrt(_x * _x + _y * _y + _z * _z);
            var mcos = _w;

            if (!(msin <= double.MaxValue))
            {
                // Overflowed probably in squaring, so let's scale
                // the values.  We don't need to include _w in the
                // scale factor because we're not going to square
                // it.
                var maxcoeff = Math.Max(Math.Abs(_x), Math.Max(Math.Abs(_y), Math.Abs(_z)));
                var x        = _x / maxcoeff;
                var y        = _y / maxcoeff;
                var z        = _z / maxcoeff;
                msin = Math.Sqrt(x * x + y * y + z * z);
                // Scale mcos too.
                mcos = _w / maxcoeff;
            }

            // Atan2 is better than acos.  (More precise and more efficient.)
            return Math.Atan2(msin, mcos) * (360.0 / Math.PI);
        }
    }

    /// <summary>
    ///     Returns whether the quaternion is normalized (i.e. has a magnitude of 1).
    /// </summary>
    public bool IsNormalized
    {
        get
        {
            if (_type == QuaternionType.Identity)
                return true;

            var norm2 = _x * _x + _y * _y + _z * _z + _w * _w;
            return DoubleUtil.IsOne(norm2);
        }
    }

    /// <summary>
    ///     Tests whether or not a given quaternion is an identity quaternion.
    /// </summary>
    public bool IsIdentity => _type == QuaternionType.Identity;

    /// <summary>
    ///     Relaces quaternion with its conjugate
    /// </summary>
    public Quaternion GetConjugated()
    {
        if (IsIdentity)
            return this;

        // Conjugate([x,y,z,w]) = [-x,-y,-z,w]
        return new Quaternion(-_x, -_y, -_z, _w);
    }

    /// <summary>
    ///     Replaces quaternion with its inverse
    /// </summary>
    public Quaternion GetInverted()
    {
        if (IsIdentity)
            return this;

        // Inverse = Conjugate / Norm Squared
        var c     = GetConjugated();
        var norm2 = c._x * c._x + c._y * c._y + c._z * c._z + c._w * c._w;
        c = new Quaternion(c._x / norm2, c._y / norm2, c._z / norm2, c._w / norm2);
        return c;
    }

    /// <summary>
    ///     Normalizes this quaternion.
    /// </summary>
    public Quaternion GetNormalized()
    {
        if (IsIdentity)
            return this;

        var x     = _x;
        var y     = _y;
        var z     = _z;
        var w     = _w;
        var norm2 = x * x + y * y + z * z + w * w;
        if (norm2 > double.MaxValue)
        {
            // Handle overflow in computation of norm2
            var rmax = 1.0 / Max(Math.Abs(x),
                Math.Abs(y),
                Math.Abs(z),
                Math.Abs(w));

            x     *= rmax;
            y     *= rmax;
            z     *= rmax;
            w     *= rmax;
            norm2 =  x * x + y * y + z * z + w * w;
        }

        var normInverse = 1.0 / Math.Sqrt(norm2);
        x *= normInverse;
        y *= normInverse;
        z *= normInverse;
        w *= normInverse;
        return new Quaternion(x, y, z, w);
    }

    /// <summary>
    ///     Quaternion addition.
    /// </summary>
    /// <param name="left">First quaternion being added.</param>
    /// <param name="right">Second quaternion being added.</param>
    /// <returns>Result of addition.</returns>
    public static Quaternion operator +(Quaternion left, Quaternion right)
    {
        if (right.IsIdentity)
        {
            if (left.IsIdentity)
            {
                return new Quaternion(0, 0, 0, 2);
            }

            return new Quaternion(left._x, left._y, left._z, left._w + 1);
        }

        if (left.IsIdentity)
            return new Quaternion(right._x, right._y, right._z, right._w + 1);

        return new Quaternion(left._x + right._x,
            left._y + right._y,
            left._z + right._z,
            left._w + right._w);
    }

    /// <summary>
    ///     Quaternion addition.
    /// </summary>
    /// <param name="left">First quaternion being added.</param>
    /// <param name="right">Second quaternion being added.</param>
    /// <returns>Result of addition.</returns>
    public static Quaternion Add(Quaternion left, Quaternion right)
    {
        return left + right;
    }

    /// <summary>
    ///     Quaternion subtraction.
    /// </summary>
    /// <param name="left">Quaternion to subtract from.</param>
    /// <param name="right">Quaternion to subtract from the first quaternion.</param>
    /// <returns>Result of subtraction.</returns>
    public static Quaternion operator -(Quaternion left, Quaternion right)
    {
        if (right.IsIdentity)
        {
            if (left.IsIdentity)
            {
                return new Quaternion(0, 0, 0, 0);
            }

            return new Quaternion(left._x, left._y, left._z, left._w - 1);
        }

        if (left.IsIdentity)
            return new Quaternion(-right._x, -right._y, -right._z, 1 - right._w);

        return new Quaternion(left._x - right._x,
            left._y - right._y,
            left._z - right._z,
            left._w - right._w);
    }

    /// <summary>
    ///     Quaternion multiplication.
    /// </summary>
    /// <param name="left">First quaternion.</param>
    /// <param name="right">Second quaternion.</param>
    /// <returns>Result of multiplication.</returns>
    public static Quaternion operator *(Quaternion left, Quaternion right)
    {
        if (left.IsIdentity)
            return right;

        if (right.IsIdentity)
            return left;

        var x      = left._w * right._x + left._x * right._w + left._y * right._z - left._z * right._y;
        var y      = left._w * right._y + left._y * right._w + left._z * right._x - left._x * right._z;
        var z      = left._w * right._z + left._z * right._w + left._x * right._y - left._y * right._x;
        var w      = left._w * right._w - left._x * right._x - left._y * right._y - left._z * right._z;
        var result = new Quaternion(x, y, z, w);
        return result;
    }

    /// <summary>
    ///     Quaternion multiplication.
    /// </summary>
    /// <param name="left">First quaternion.</param>
    /// <param name="right">Second quaternion.</param>
    /// <returns>Result of multiplication.</returns>
    public static Quaternion Multiply(Quaternion left, Quaternion right)
    {
        return left * right;
    }

    /// <summary>
    ///     Scale this quaternion by a scalar.
    /// </summary>
    /// <param name="scale">Value to scale by.</param>
    private Quaternion GetScaled(double scale)
    {
        if (IsIdentity)
        {
            return new Quaternion(X, Y, Z, scale);
        }

        return new Quaternion(_x * scale, _y * scale, _z * scale, _w * scale);
    }

    /// <summary>
    ///     Return length of quaternion.
    /// </summary>
    private double Length()
    {
        if (IsIdentity)
        {
            return 1;
        }

        var norm2 = _x * _x + _y * _y + _z * _z + _w * _w;
        if (!(norm2 <= double.MaxValue))
        {
            // Do this the slow way to avoid squaring large
            // numbers since the length of many quaternions is
            // representable even if the squared length isn't.  Of
            // course some lengths aren't representable because
            // the length can be up to twice as big as the largest
            // coefficient.

            var max = Math.Max(Math.Max(Math.Abs(_x), Math.Abs(_y)),
                Math.Max(Math.Abs(_z), Math.Abs(_w)));

            var x = _x / max;
            var y = _y / max;
            var z = _z / max;
            var w = _w / max;

            var smallLength = Math.Sqrt(x * x + y * y + z * z + w * w);
            // Return length of this smaller vector times the scale we applied originally.
            return smallLength * max;
        }

        return Math.Sqrt(norm2);
    }


#if LATER
    /// <summary>
    /// Smoothly interpolate between the two given quaternions using Spherical 
    /// Linear Interpolation (SLERP).
    /// </summary>
    /// <param name="from">First quaternion for interpolation.</param>
    /// <param name="to">Second quaternion for interpolation.</param>
    /// <param name="t">Interpolation coefficient.</param>
    /// <returns>SLERP-interpolated quaternion between the two given quaternions.</returns>
    public static Quaternion Slerp(Quaternion from, Quaternion to, double t)
    {
        return Slerp(from, to, t, useShortestPath: true);
    }

    
    
    /// <summary>
    /// Smoothly interpolate between the two given quaternions using Spherical 
    /// Linear Interpolation (SLERP).
    /// </summary>
    /// <param name="from">First quaternion for interpolation.</param>
    /// <param name="to">Second quaternion for interpolation.</param>
    /// <param name="t">Interpolation coefficient.</param>
    /// <param name="useShortestPath">If true, Slerp will automatically flip the sign of
    ///     the destination Quaternion to ensure the shortest path is taken.</param>
    /// <returns>SLERP-interpolated quaternion between the two given quaternions.</returns>
    public static Quaternion Slerp(Quaternion from, Quaternion to, double t, bool useShortestPath)
    {
        if (from.IsDistinguishedIdentity)
        {
            from._w = 1;
        }

        if (to.IsDistinguishedIdentity)
        {
            to._w = 1;
        }

        double cosOmega;
        double scaleFrom, scaleTo;

        // Normalize inputs and stash their lengths
        double lengthFrom = from.Length();
        double lengthTo = to.Length();
        from.Scale(1 / lengthFrom);
        to.Scale(1 / lengthTo);

        // Calculate cos of omega.
        cosOmega = from._x * to._x + from._y * to._y + from._z * to._z + from._w * to._w;

        if (useShortestPath)
        {
            // If we are taking the shortest path we flip the signs to ensure that
            // cosOmega will be positive.
            if (cosOmega < 0.0)
            {
                cosOmega = -cosOmega;
                to._x = -to._x;
                to._y = -to._y;
                to._z = -to._z;
                to._w = -to._w;
            }
        }
        else
        {
            // If we are not taking the UseShortestPath we clamp cosOmega to
            // -1 to stay in the domain of Math.Acos below.
            if (cosOmega < -1.0)
            {
                cosOmega = -1.0;
            }
        }

        // Clamp cosOmega to [-1,1] to stay in the domain of Math.Acos below.
        // The logic above has either flipped the sign of cosOmega to ensure it
        // is positive or clamped to -1 aready.  We only need to worry about the
        // upper limit here.
        if (cosOmega > 1.0)
        {
            cosOmega = 1.0;
        }

        Debug.Assert(!(cosOmega < -1.0) && !(cosOmega > 1.0),
            "cosOmega should be clamped to [-1,1]");

        // The mainline algorithm doesn't work for extreme
        // cosine values.  For large cosine we have a better
        // fallback hence the asymmetric limits.
        const double maxCosine = 1.0 - 1e-6;
        const double minCosine = 1e-10 - 1.0;

        // Calculate scaling coefficients.
        if (cosOmega > maxCosine)
        {
            // Quaternions are too close - use linear interpolation.
            scaleFrom = 1.0 - t;
            scaleTo = t;
        }
        else if (cosOmega < minCosine)
        {
            // Quaternions are nearly opposite, so we will pretend to 
            // is exactly -from.
            // First assign arbitrary perpendicular to "to".
            to = new Quaternion(-from.Y, from.X, -from.W, from.Z);

            double theta = t * Math.PI;

            scaleFrom = Math.Cos(theta);
            scaleTo = Math.Sin(theta);
        }
        else
        {
            // Standard case - use SLERP interpolation.
            double omega = Math.Acos(cosOmega);
            double sinOmega = Math.Sqrt(1.0 - cosOmega * cosOmega);
            scaleFrom = Math.Sin((1.0 - t) * omega) / sinOmega;
            scaleTo = Math.Sin(t * omega) / sinOmega;
        }

        // We want the magnitude of the output quaternion to be
        // multiplicatively interpolated between the input
        // magnitudes, i.e. lengthOut = lengthFrom * (lengthTo/lengthFrom)^t
        //                            = lengthFrom ^ (1-t) * lengthTo ^ t

        double lengthOut = lengthFrom * Math.Pow(lengthTo / lengthFrom, t);
        scaleFrom *= lengthOut;
        scaleTo *= lengthOut;

        return new Quaternion(scaleFrom * from._x + scaleTo * to._x,
            scaleFrom * from._y + scaleTo * to._y,
            scaleFrom * from._z + scaleTo * to._z,
            scaleFrom * from._w + scaleTo * to._w);
    }
#endif

    private static double Max(double a, double b, double c, double d)
    {
        if (b > a)
            a = b;
        if (c > a)
            a = c;
        if (d > a)
            a = d;
        return a;
    }

    //------------------------------------------------------
    //
    //  Public Properties
    //
    //------------------------------------------------------

    /// <summary>
    ///     X - Default value is 0.
    /// </summary>
    public double X => _x;

    /// <summary>
    ///     Y - Default value is 0.
    /// </summary>
    public double Y => _y;

    /// <summary>
    ///     Z - Default value is 0.
    /// </summary>
    public double Z => _z;

    /// <summary>
    ///     W - Default value is 1.
    /// </summary>
    public double W => IsIdentity ? 1.0 : _w;

    internal readonly double _x;
    internal readonly double _y;
    internal readonly double _z;
    internal readonly double _w;
    private readonly QuaternionType _type;


    private static int GetIdentityHashCode()
    {
        // This code is called only once.
        double zero = 0;
        double one  = 1;
        // return zero.GetHashCode() ^ zero.GetHashCode() ^ zero.GetHashCode() ^ one.GetHashCode();
        // But this expression can be simplified because the first two hash codes cancel.
        return zero.GetHashCode() ^ one.GetHashCode();
    }

    private static Quaternion GetIdentity()
    {
        return default;
    }


    // Hash code for identity.
    private static int c_identityHashCode = GetIdentityHashCode();

    // Default identity
    private static readonly Quaternion s_identity = GetIdentity();


    /// <summary>
    ///     Creates a string representation of this object based on the current culture.
    /// </summary>
    /// <returns>
    ///     A string representation of this object.
    /// </returns>
    public override string ToString()
    {
        // Delegate to the internal method which implements all ToString calls.
        return ConvertToString(null /* format string */, null /* format provider */);
    }

    /// <summary>
    ///     Creates a string representation of this object based on the IFormatProvider
    ///     passed in.  If the provider is null, the CurrentCulture is used.
    /// </summary>
    /// <returns>
    ///     A string representation of this object.
    /// </returns>
    public string ToString(IFormatProvider provider)
    {
        // Delegate to the internal method which implements all ToString calls.
        return ConvertToString(null /* format string */, provider);
    }

    /// <summary>
    ///     Creates a string representation of this object based on the format string
    ///     and IFormatProvider passed in.
    ///     If the provider is null, the CurrentCulture is used.
    ///     See the documentation for IFormattable for more information.
    /// </summary>
    /// <returns>
    ///     A string representation of this object.
    /// </returns>
    string IFormattable.ToString(string format, IFormatProvider provider)
    {
        // Delegate to the internal method which implements all ToString calls.
        return ConvertToString(format, provider);
    }


    /// <summary>
    ///     Creates a string representation of this object based on the format string
    ///     and IFormatProvider passed in.
    ///     If the provider is null, the CurrentCulture is used.
    ///     See the documentation for IFormattable for more information.
    /// </summary>
    /// <returns>
    ///     A string representation of this object.
    /// </returns>
    private string ConvertToString(string format, IFormatProvider provider)
    {
        if (IsIdentity)
        {
            return "Identity";
        }

        // Helper to get the numeric list separator for a given culture.
        var separator = MsCompatibility.GetNumericListSeparator(provider);
        return string.Format(provider,
            "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}",
            separator, _x, _y, _z, _w);
    }

    private enum QuaternionType
    {
        Identity,
        NotIdentity
    }
}
