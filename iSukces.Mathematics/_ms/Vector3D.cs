// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

public readonly record struct Vector3D(double X, double Y, double Z) : IFormattable
{
    /// <summary>
    ///     Vector addition.
    /// </summary>
    /// <param name="vector1">First vector being added.</param>
    /// <param name="vector2">Second vector being added.</param>
    /// <returns>Result of addition.</returns>
    public static Vector3D Add(Vector3D vector1, Vector3D vector2)
    {
        return new Vector3D(vector1.X + vector2.X,
            vector1.Y + vector2.Y,
            vector1.Z + vector2.Z);
    }

    /// <summary>
    ///     Computes the angle between two vectors.
    /// </summary>
    /// <param name="vector1">First vector.</param>
    /// <param name="vector2">Second vector.</param>
    /// <returns>
    ///     Returns the angle required to rotate vector1 into vector2 in degrees.
    ///     This will return a value between [0, 180] degrees.
    ///     (Note that this is slightly different from the Vector member
    ///     function of the same name.  Signed angles do not extend to 3D.)
    /// </returns>
    public static double AngleBetween(Vector3D vector1, Vector3D vector2)
    {
        vector1 = vector1.GetNormalized();
        vector2 = vector2.GetNormalized();

        var ratio = DotProduct(vector1, vector2);

        // The "straight forward" method of acos(u.v) has large precision
        // issues when the dot product is near +/-1.  This is due to the
        // steep slope of the acos function as we approach +/- 1.  Slight
        // precision errors in the dot product calculation cause large
        // variation in the output value.
        //
        //        |                   |
        //         \__                |
        //            ---___          |
        //                  ---___    |
        //                        ---_|_
        //                            | ---___
        //                            |       ---___
        //                            |             ---__
        //                            |                  \
        //                            |                   |
        //       -|-------------------+-------------------|-
        //       -1                   0                   1
        //
        //                         acos(x)
        //
        // To avoid this we use an alternative method which finds the
        // angle bisector by (u-v)/2:
        //
        //                            _>
        //                       u  _-  \ (u-v)/2
        //                        _-  __-v
        //                      _=__--      
        //                    .=----------->
        //                            v
        //
        // Because u and v and unit vectors, (u-v)/2 forms a right angle
        // with the angle bisector.  The hypotenuse is 1, therefore
        // 2*asin(|u-v|/2) gives us the angle between u and v.
        //
        // The largest possible value of |u-v| occurs with perpendicular
        // vectors and is sqrt(2)/2 which is well away from extreme slope
        // at +/-1.
        //

        double theta;

        if (ratio < 0)
        {
            theta = Math.PI - 2.0 * Math.Asin((-vector1 - vector2).Length / 2.0);
        }
        else
        {
            theta = 2.0 * Math.Asin((vector1 - vector2).Length / 2.0);
        }

        return theta * MathEx.RADTODEG;
    }

    /// <summary>
    ///     Vector cross product.
    /// </summary>
    /// <param name="vector1">First vector.</param>
    /// <param name="vector2">Second vector.</param>
    /// <returns>Cross product of two vectors.</returns>
    public static Vector3D CrossProduct(Vector3D vector1, Vector3D vector2)
    {
        CrossProduct(ref vector1, ref vector2, out var result);
        return result;
    }

    /// <summary>
    ///     Faster internal version of CrossProduct that avoids copies
    ///     vector1 and vector2 to a passed by ref for perf and ARE NOT MODIFIED
    /// </summary>
    internal static void CrossProduct(ref Vector3D vector1, ref Vector3D vector2, out Vector3D result)
    {
        var x = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
        var y = vector1.Z * vector2.X - vector1.X * vector2.Z;
        var z = vector1.X * vector2.Y - vector1.Y * vector2.X;
        result = new Vector3D(x, y, z);
    }

    /// <summary>
    ///     Vector cross product.
    /// </summary>
    /// <param name="vector1">First vector.</param>
    /// <param name="vector2">Second vector.</param>
    /// <returns>Cross product of two vectors.</returns>
    public static Vector3D CrossProductNormalized(Vector3D vector1, Vector3D vector2)
    {
        CrossProduct(ref vector1, ref vector2, out var result);
        return result.GetNormalized();
    }

    /// <summary>
    ///     Scalar division.
    /// </summary>
    /// <param name="vector">Vector being divided.</param>
    /// <param name="scalar">Scalar value by which we divide the vector.</param>
    /// <returns>Result of division.</returns>
    public static Vector3D Divide(Vector3D vector, double scalar)
    {
        return vector * (1.0 / scalar);
    }

    /// <summary>
    ///     Vector dot product.
    /// </summary>
    /// <param name="vector1">First vector.</param>
    /// <param name="vector2">Second vector.</param>
    /// <returns>Dot product of two vectors.</returns>
    public static double DotProduct(Vector3D vector1, Vector3D vector2)
    {
        return DotProduct(ref vector1, ref vector2);
    }

    /// <summary>
    ///     Faster internal version of DotProduct that avoids copies
    ///     vector1 and vector2 to a passed by ref for perf and ARE NOT MODIFIED
    /// </summary>
    internal static double DotProduct(ref Vector3D vector1, ref Vector3D vector2)
    {
        return vector1.X * vector2.X +
               vector1.Y * vector2.Y +
               vector1.Z * vector2.Z;
    }

    /// <summary>
    ///     Compares two Vector3D instances for object equality.  In this equality
    ///     Double.NaN is equal to itself, unlike in numeric equality.
    ///     Note that double values can acquire error when operated upon, such that
    ///     an exact comparison between two values which
    ///     are logically equal may fail.
    /// </summary>
    /// <returns>
    ///     bool - true if the two Vector3D instances are exactly equal, false otherwise
    /// </returns>
    /// <param name='vector1'>The first Vector3D to compare</param>
    /// <param name='vector2'>The second Vector3D to compare</param>
    public static bool Equals(Vector3D vector1, Vector3D vector2)
    {
        return vector1.X.Equals(vector2.X) &&
               vector1.Y.Equals(vector2.Y) &&
               vector1.Z.Equals(vector2.Z);
    }

    /// <summary>
    ///     Scalar multiplication.
    /// </summary>
    /// <param name="scalar">Scalar value by which the vector is multiplied</param>
    /// <param name="vector">Vector being multiplied.</param>
    /// <returns>Result of multiplication.</returns>
    public static Vector3D Multiply(double scalar, Vector3D vector)
    {
        return new Vector3D(vector.X * scalar,
            vector.Y * scalar,
            vector.Z * scalar);
    }

    /// <summary>
    ///     Vector3D * Matrix3D multiplication
    /// </summary>
    /// <param name="vector">Vector being tranformed.</param>
    /// <param name="matrix">Transformation matrix applied to the vector.</param>
    /// <returns>Result of multiplication.</returns>
    public static Vector3D Multiply(Vector3D vector, Matrix3D matrix)
    {
        return matrix.Transform(vector);
    }

    /// <summary>
    ///     Vector addition.
    /// </summary>
    /// <param name="vector1">First vector being added.</param>
    /// <param name="vector2">Second vector being added.</param>
    /// <returns>Result of addition.</returns>
    public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
    {
        return new Vector3D(vector1.X + vector2.X,
            vector1.Y + vector2.Y,
            vector1.Z + vector2.Z);
    }

    /// <summary>
    ///     Vector3D + Point3D addition.
    /// </summary>
    /// <param name="vector">Vector by which we offset the point.</param>
    /// <param name="point">Point being offset by the given vector.</param>
    /// <returns>Result of addition.</returns>
    public static Point3D operator +(Vector3D vector, Point3D point)
    {
        return new Point3D(vector.X + point.X,
            vector.Y + point.Y,
            vector.Z + point.Z);
    }

    /// <summary>
    ///     Scalar division.
    /// </summary>
    /// <param name="vector">Vector being divided.</param>
    /// <param name="scalar">Scalar value by which we divide the vector.</param>
    /// <returns>Result of division.</returns>
    public static Vector3D operator /(Vector3D vector, double scalar)
    {
        return vector * (1.0 / scalar);
    }

    /// <summary>
    ///     Vector3D to Point3D conversion.
    /// </summary>
    /// <param name="vector">Vector being converted.</param>
    /// <returns>Point representing the given vector.</returns>
    public static explicit operator Point3D(Vector3D vector)
    {
        return new Point3D(vector.X, vector.Y, vector.Z);
    }

    /// <summary>
    ///     Scalar multiplication.
    /// </summary>
    /// <param name="vector">Vector being multiplied.</param>
    /// <param name="scalar">Scalar value by which the vector is multiplied.</param>
    /// <returns>Result of multiplication.</returns>
    public static Vector3D operator *(Vector3D vector, double scalar)
    {
        return new Vector3D(vector.X * scalar,
            vector.Y * scalar,
            vector.Z * scalar);
    }


    public static double operator *(Vector3D a, Vector3D b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    }


    /// <summary>
    ///     Scalar multiplication.
    /// </summary>
    /// <param name="scalar">Scalar value by which the vector is multiplied</param>
    /// <param name="vector">Vector being multiplied.</param>
    /// <returns>Result of multiplication.</returns>
    public static Vector3D operator *(double scalar, Vector3D vector)
    {
        return new Vector3D(vector.X * scalar,
            vector.Y * scalar,
            vector.Z * scalar);
    }

    /// <summary>
    ///     Vector3D * Matrix3D multiplication
    /// </summary>
    /// <param name="vector">Vector being tranformed.</param>
    /// <param name="matrix">Transformation matrix applied to the vector.</param>
    /// <returns>Result of multiplication.</returns>
    public static Vector3D operator *(Vector3D vector, Matrix3D matrix)
    {
        return matrix.Transform(vector);
    }

    /// <summary>
    ///     Vector subtraction.
    /// </summary>
    /// <param name="vector1">Vector that is subtracted from.</param>
    /// <param name="vector2">Vector being subtracted.</param>
    /// <returns>Result of subtraction.</returns>
    public static Vector3D operator -(Vector3D vector1, Vector3D vector2)
    {
        return new Vector3D(vector1.X - vector2.X,
            vector1.Y - vector2.Y,
            vector1.Z - vector2.Z);
    }


    /// <summary>
    ///     Vector3D - Point3D subtraction.
    /// </summary>
    /// <param name="vector">Vector by which we offset the point.</param>
    /// <param name="point">Point being offset by the given vector.</param>
    /// <returns>Result of subtraction.</returns>
    public static Point3D operator -(Vector3D vector, Point3D point)
    {
        return new Point3D(vector.X - point.X,
            vector.Y - point.Y,
            vector.Z - point.Z);
    }

    /// <summary>
    ///     Operator -Vector (unary negation).
    /// </summary>
    /// <param name="vector">Vector being negated.</param>
    /// <returns>Negation of the given vector.</returns>
    public static Vector3D operator -(Vector3D vector)
    {
        return new Vector3D(-vector.X, -vector.Y, -vector.Z);
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
    internal string ConvertToString(string? format, IFormatProvider? provider)
    {
        // Helper to get the numeric list separator for a given culture.
        var separator = MsCompatibility.GetNumericListSeparator(provider);
        var format1   = "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}";
        return string.Format(provider, format1, separator, X, Y, Z);
    }

    /// <summary>
    ///     Updates the vector to maintain its direction, but to have a length
    ///     of 1. Equivalent to dividing the vector by its Length.
    ///     Returns NaN if length is zero.
    /// </summary>
    public Vector3D GetNormalized()
    {
        // Computation of length can overflow easily because it
        // first computes squared length, so we first divide by
        // the largest coefficient.
        var m           = Math.Abs(X);
        var absy        = Math.Abs(Y);
        var absz        = Math.Abs(Z);
        if (absy > m) m = absy;
        if (absz > m) m = absz;
        var x           = X / m;
        var y           = Y / m;
        var z           = Z / m;
        var length      = Math.Sqrt(x * x + y * y + z * z);
        return new Vector3D(x / length, y / length, z / length);
    }


    /*
    /// <summary>
    /// Parse - returns an instance converted from the provided string using
    /// the culture "en-US"
    /// <param name="source"> string with Vector3D data </param>
    /// </summary>
    public static Vector3D Parse(string source)
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        TokenizerHelper th = new TokenizerHelper(source, formatProvider);

        Vector3D value;

        String firstToken = th.NextTokenRequired();

        value = new Vector3D(
            Convert.ToDouble(firstToken, formatProvider),
            Convert.ToDouble(th.NextTokenRequired(), formatProvider),
            Convert.ToDouble(th.NextTokenRequired(), formatProvider));

        // There should be no more tokens in this string.
        th.LastTokenRequired();

        return value;
    }*/


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
    string IFormattable.ToString(string? format, IFormatProvider? provider)
    {
        // Delegate to the internal method which implements all ToString calls.
        return ConvertToString(format, provider);
    }

    /// <summary>
    ///     Length of the vector.
    /// </summary>
    public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

    /// <summary>
    ///     Length of the vector squared.
    /// </summary>
    public double LengthSquared => X * X + Y * Y + Z * Z;


    public bool IsNaN => double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Z);
}
