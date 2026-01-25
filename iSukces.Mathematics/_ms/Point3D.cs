// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

public readonly record struct Point3D(double X, double Y, double Z) : IFormattable
{
    /*/// <summary>
    ///     Point3D + Vector3D addition.
    /// </summary>
    /// <param name="point">Point being added.</param>
    /// <param name="vector">Vector being added.</param>
    /// <returns>Result of addition.</returns>
    public static Point3D Add(Point3D point, Vector3D vector)
    {
        return new Point3D(point.X + vector.X,
            point.Y + vector.Y,
            point.Z + vector.Z);
    }

    /// <summary>
    ///     Point3D * Matrix3D multiplication.
    /// </summary>
    /// <param name="point">Point being transformed.</param>
    /// <param name="matrix">Transformation matrix applied to the point.</param>
    /// <returns>Result of the transformation matrix applied to the point.</returns>
    public static Point3D Multiply(Point3D point, Matrix3D matrix)
    {
        return matrix.Transform(point);
    }*/


    /// <summary>
    ///     Point3D + Vector3D addition.
    /// </summary>
    /// <param name="point">Point being added.</param>
    /// <param name="vector">Vector being added.</param>
    /// <returns>Result of addition.</returns>
    public static Point3D operator +(Point3D point, Vector3D vector)
    {
        return new Point3D(point.X + vector.X,
            point.Y + vector.Y,
            point.Z + vector.Z);
    }

    /// <summary>
    ///     Explicit conversion to Vector3D.
    /// </summary>
    /// <param name="point">Given point.</param>
    /// <returns>Vector representing the point.</returns>
    public static explicit operator Vector3D(Point3D point)
    {
        return new Vector3D(point.X, point.Y, point.Z);
    }


    /// <summary>
    ///     Point3D * Matrix3D multiplication.
    /// </summary>
    /// <param name="point">Point being transformed.</param>
    /// <param name="matrix">Transformation matrix applied to the point.</param>
    /// <returns>Result of the transformation matrix applied to the point.</returns>
    public static Point3D operator *(Point3D point, Matrix3D matrix)
    {
        return matrix.Transform(point);
    }

    /// <summary>
    ///     Point3D - Vector3D subtraction.
    /// </summary>
    /// <param name="point">Point from which vector is being subtracted.</param>
    /// <param name="vector">Vector being subtracted from the point.</param>
    /// <returns>Result of subtraction.</returns>
    public static Point3D operator -(Point3D point, Vector3D vector)
    {
        return new Point3D(point.X - vector.X,
            point.Y - vector.Y,
            point.Z - vector.Z);
    }

    /// <summary>
    ///     Subtraction.
    /// </summary>
    /// <param name="point1">Point from which we are subtracting the second point.</param>
    /// <param name="point2">Point being subtracted.</param>
    /// <returns>Vector between the two points.</returns>
    public static Vector3D operator -(Point3D point1, Point3D point2)
    {
        return new Vector3D(point1.X - point2.X,
            point1.Y - point2.Y,
            point1.Z - point2.Z);
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
    internal string ConvertToString(string? format, IFormatProvider provider)
    {
        // Helper to get the numeric list separator for a given culture.
        var separator = MsCompatibility.GetNumericListSeparator(provider);
        var format1   = "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}";
        return string.Format(provider, format1, separator, X, Y, Z);
    }


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


    /*/// <summary>
    /// Parse - returns an instance converted from the provided string using
    /// the culture "en-US"
    /// <param name="source"> string with Point3D data </param>
    /// </summary>
    public static Point3D Parse(string source)
    {
        IFormatProvider formatProvider = System.Windows.Markup.TypeConverterHelper.InvariantEnglishUS;

        TokenizerHelper th = new TokenizerHelper(source, formatProvider);

        Point3D value;

        String firstToken = th.NextTokenRequired();

        value = new Point3D(
            Convert.ToDouble(firstToken, formatProvider),
            Convert.ToDouble(th.NextTokenRequired(), formatProvider),
            Convert.ToDouble(th.NextTokenRequired(), formatProvider));

        // There should be no more tokens in this string.
        th.LastTokenRequired();

        return value;
    }*/


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
}
