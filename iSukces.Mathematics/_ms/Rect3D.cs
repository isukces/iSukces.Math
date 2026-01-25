// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

public readonly record struct Rect3D : IFormattable
{
    public Rect3D(double x, double y, double z, double sizeX,
        double sizeY, double sizeZ)
    {
        _x     = x;
        _y     = y;
        _z     = z;
        _sizeX = sizeX;
        _sizeY = sizeY;
        _sizeZ = sizeZ;
        _isSet = true;
    }

    public Rect3D(Point3D location, Size3D size)
    {
        _x     = location.X;
        _y     = location.Y;
        _z     = location.Z;
        _sizeX = size.X;
        _sizeY = size.Y;
        _sizeZ = size.Z;
        _isSet = true;
    }


    /// <summary>
    ///     Constructor which sets the initial values to bound the two points provided.
    /// </summary>
    /// <param name="point1">First point.</param>
    /// <param name="point2">Second point.</param>
    internal Rect3D(Point3D point1, Point3D point2)
    {
        _x     = Math.Min(point1.X, point2.X);
        _y     = Math.Min(point1.Y, point2.Y);
        _z     = Math.Min(point1.Z, point2.Z);
        _sizeX = Math.Max(point1.X, point2.X) - _x;
        _sizeY = Math.Max(point1.Y, point2.Y) - _y;
        _sizeZ = Math.Max(point1.Z, point2.Z) - _z;
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
        if (IsEmpty)
        {
            return "Empty";
        }

        // Helper to get the numeric list separator for a given culture.
        var separator = MsCompatibility.GetNumericListSeparator(provider);
        return string.Format(provider,
            "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}{0}{5:" + format + "}{0}{6:" + format + "}",
            separator,
            _x,
            _y,
            _z,
            _sizeX,
            _sizeY,
            _sizeZ);
    }


    /*/// <summary>
    /// Compares two Rect3D instances for object equality.  In this equality
    /// Double.NaN is equal to itself, unlike in numeric equality.
    /// Note that double values can acquire error when operated upon, such that
    /// an exact comparison between two values which
    /// are logically equal may fail.
    /// </summary>
    /// <returns>
    /// bool - true if the two Rect3D instances are exactly equal, false otherwise
    /// </returns>
    /// <param name='rect1'>The first Rect3D to compare</param>
    /// <param name='rect2'>The second Rect3D to compare</param>
    public static bool Equals(Rect3D rect1, Rect3D rect2)
    {
        if (rect1.IsEmpty)
        {
            return rect2.IsEmpty;
        }
        else
        {
            return rect1.X.Equals(rect2.X) &&
                   rect1.Y.Equals(rect2.Y) &&
                   rect1.Z.Equals(rect2.Z) &&
                   rect1.SizeX.Equals(rect2.SizeX) &&
                   rect1.SizeY.Equals(rect2.SizeY) &&
                   rect1.SizeZ.Equals(rect2.SizeZ);
        }
    }*/


    /// <summary>
    ///     Equals - compares this Rect3D with the passed in object.  In this equality
    ///     Double.NaN is equal to itself, unlike in numeric equality.
    ///     Note that double values can acquire error when operated upon, such that
    ///     an exact comparison between two values which
    ///     are logically equal may fail.
    /// </summary>
    /// <returns>
    ///     bool - true if "value" is equal to "this".
    /// </returns>
    /// <param name='value'>The Rect3D to compare to "this"</param>
    public bool Equals(Rect3D value)
    {
        if (_isSet != value._isSet) return false;
        return _x.Equals(value._x) &&
               _y.Equals(value._y) &&
               _z.Equals(value._z) &&
               _sizeX.Equals(value._sizeX) &&
               _sizeY.Equals(value._sizeY) &&
               _sizeZ.Equals(value._sizeZ);
    }

    /// <summary>
    ///     Returns the HashCode for this Rect3D
    /// </summary>
    /// <returns>
    ///     int - the HashCode for this Rect3D
    /// </returns>
    public override int GetHashCode()
    {
        if (IsEmpty)
        {
            return 0;
        }

        // Perform field-by-field XOR of HashCodes
        return X.GetHashCode() ^
               Y.GetHashCode() ^
               Z.GetHashCode() ^
               SizeX.GetHashCode() ^
               SizeY.GetHashCode() ^
               SizeZ.GetHashCode();
    }


    /// <summary>
    ///     Update this rectangle to be the union of this and point.
    /// </summary>
    /// <param name="point">Point.</param>
    public Rect3D GetUnion(Point3D point)
    {
        var rect3D = new Rect3D(point, point);
        return GetUnion(rect3D);
    }


    /// <summary>
    ///     Update this rectangle to be the union of this and rect.
    /// </summary>
    /// <param name="rect">Rectangle.</param>
    public Rect3D GetUnion(Rect3D rect)
    {
        if (IsEmpty)
            return rect;
        if (rect.IsEmpty)
            return this;
        var x     = Math.Min(_x, rect._x);
        var y     = Math.Min(_y, rect._y);
        var z     = Math.Min(_z, rect._z);
        var sizeX = Math.Max(_x + _sizeX, rect._x + rect._sizeX) - x;
        var sizeY = Math.Max(_y + _sizeY, rect._y + rect._sizeY) - y;
        var sizeZ = Math.Max(_z + _sizeZ, rect._z + rect._sizeZ) - z;
        return new Rect3D(x, y, z, sizeX, sizeY, sizeZ);
    }

/*
    /// <summary>
    /// Parse - returns an instance converted from the provided string using
    /// the culture "en-US"
    /// <param name="source"> string with Rect3D data </param>
    /// </summary>
    public static Rect3D Parse(string source)
    {
        IFormatProvider formatProvider = System.Windows.Markup.TypeConverterHelper.InvariantEnglishUS;

        TokenizerHelper th = new TokenizerHelper(source, formatProvider);

        Rect3D value;

        String firstToken = th.NextTokenRequired();

        // The token will already have had whitespace trimmed so we can do a
        // simple string compare.
        if (firstToken == "Empty")
        {
            value = Empty;
        }
        else
        {
            value = new Rect3D(
                Convert.ToDouble(firstToken, formatProvider),
                Convert.ToDouble(th.NextTokenRequired(), formatProvider),
                Convert.ToDouble(th.NextTokenRequired(), formatProvider),
                Convert.ToDouble(th.NextTokenRequired(), formatProvider),
                Convert.ToDouble(th.NextTokenRequired(), formatProvider),
                Convert.ToDouble(th.NextTokenRequired(), formatProvider));
        }

        // There should be no more tokens in this string.
        th.LastTokenRequired();

        return value;
    }
*/

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

    public static Rect3D Empty => default;

    public Point3D Location => new(X, Y, Z);

    public double X => _isSet ? _x : double.PositiveInfinity;
    public double Y => _isSet ? _y : double.PositiveInfinity;
    public double Z => _isSet ? _z : double.PositiveInfinity;


    public double SizeX => _isSet ? _sizeX : double.NegativeInfinity;
    public double SizeY => _isSet ? _sizeY : double.NegativeInfinity;
    public double SizeZ => _isSet ? _sizeZ : double.NegativeInfinity;

    public bool IsEmpty => !_isSet;

    public Size3D Size => _isSet ? new(_sizeX, _sizeY, _sizeZ) : Size3D.Empty;

    internal readonly double _x;
    internal readonly double _y;
    internal readonly double _z;
    internal readonly double _sizeX;
    internal readonly double _sizeY;
    internal readonly double _sizeZ;

    private readonly bool _isSet;
}
