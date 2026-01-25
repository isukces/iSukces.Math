// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

public readonly record struct Size : IEquatable<Size>
{
    public Size(double width, double height)
    {
        if (width < 0)
            throw new ArgumentException("Width and height must be non-negative", nameof(width));
        if (height < 0)
            throw new ArgumentException("Width and height must be non-negative", nameof(height));
        _width  = width;
        _height = height;
        _isSet  = true;
    }

    public static explicit operator Vector(Size size)
    {
        return new Vector(size.Width, size.Height);
    }

    public static explicit operator Point(Size size)
    {
        return new Point(size.Width, size.Height);
    }

    public bool Equals(Size value)
    {
        if (_isSet != value._isSet)
            return false;
        return !_isSet
               ||
               _width.Equals(value._width) && _height.Equals(value._height);
    }

    public override int GetHashCode()
    {
        if (IsEmpty)
            return 0;
        return Width.GetHashCode() ^ Height.GetHashCode();
    }


    public override string ToString()
    {
        if (IsEmpty)
            return "Empty";
        var numericListSeparator = MsCompatibility.GetNumericListSeparator(null);
        return $"{Width}{numericListSeparator}{Height}";
    }

    public Size WithHeight(double height)
    {
        return new Size(Width, height);
    }

    public Size WithWidth(double width)
    {
        return new Size(width, Height);
    }
    
    [Obsolete("Use * operator instead")]
    public Size Multiply(double scale)
    {
        if (IsEmpty || scale == 1d)
            return this;
        return new Size(Width * scale, Height * scale);
    }
    
    
    public static Size operator *(Size size, double scale)
    {
        if (size.IsEmpty || scale == 1d)
            return size;
        return new Size(size.Width * scale, size.Height * scale);
    }

    public static Size operator *(double scale, Size size)
    {
        if (size.IsEmpty || scale == 1d)
            return size;
        return new Size(size.Width * scale, size.Height * scale);
    }


    public static Size Empty => default;

    public bool IsEmpty => !_isSet;

    public double Width => _isSet ? _width : double.NegativeInfinity;

    public double Height => _isSet ? _height : double.NegativeInfinity;

    private readonly double _width;
    private readonly double _height;
    private readonly bool _isSet;
}
