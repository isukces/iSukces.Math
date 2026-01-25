// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

public struct Rect : IEquatable<Rect>
{
    public Rect(double x, double y, double width, double height)
    {
        X      = x;
        Y      = y;
        Width  = width;
        Height = height;
    }

    public Rect(Point point, Size size)
    {
        X      = point.X;
        Y      = point.Y;
        Width  = size.Width;
        Height = size.Height;
    }

    public Rect(Point topLeft, Point bottomRight)
    {
        X      = topLeft.X;
        Y      = topLeft.Y;
        Width  = bottomRight.X - topLeft.X;
        Height = bottomRight.Y - topLeft.Y;
    }

    private static Rect CreateEmptyRect()
    {
        return new Rect(double.PositiveInfinity, double.PositiveInfinity,
            double.NegativeInfinity, double.NegativeInfinity);
    }

    private static bool Equals(Rect rect1, Rect rect2)
    {
        if (rect1.IsEmpty)
            return rect2.IsEmpty;
        if (rect1.X.Equals(rect2.X) && rect1.Y.Equals(rect2.Y) && rect1.Width.Equals(rect2.Width))
            return rect1.Height.Equals(rect2.Height);
        return false;
    }

    public static Rect Make(MinMax? x, MinMax? y)
    {
        return x is null || x.IsInvalid || y is null || y.IsInvalid
            ? Empty
            : new Rect(x.Min, y.Min, x.Length, y.Length);
    }

    public static Rect Make(DRange x, DRange y)
    {
        return x.IsEmptyOrInvalid || y.IsEmptyOrInvalid
            ? Empty
            : new Rect(x.Min, y.Min, x.Length, y.Length);
    }

    public static bool operator ==(Rect rect1, Rect rect2)
    {
        return rect1.X == rect2.X
               && rect1.Y == rect2.Y
               && rect1.Width == rect2.Width
               && rect1.Height == rect2.Height;
    }

    public static bool operator !=(Rect rect1, Rect rect2)
    {
        return !(rect1 == rect2);
    }

    public override bool Equals(object? o)
    {
        return o is Rect rect && Equals(this, rect);
    }


    public bool Equals(Rect value)
    {
        return Equals(this, value);
    }


    public override int GetHashCode()
    {
        if (IsEmpty)
            return 0;
        return X.GetHashCode()
               ^ Y.GetHashCode()
               ^ Width.GetHashCode()
               ^ Height.GetHashCode();
    }


    /// <summary>
    ///     Union - Update this rectangle to be the union of this and rect.
    /// </summary>
    public Rect GetUnion(Rect rect)
    {
        if (IsEmpty)
        {
            return rect;
        }

        if (rect.IsEmpty)
            return Empty;

        var left = Math.Min(Left, rect.Left);
        var top  = Math.Min(Top, rect.Top);

        double width;
        double height;
        // We need this check so that the math does not result in NaN
        // ReSharper disable CompareOfFloatsByEqualityOperator
        if (rect.Width == double.PositiveInfinity || Width == double.PositiveInfinity)
        {
            width = double.PositiveInfinity;
        }
        else
        {
            //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)                    
            var maxRight = Math.Max(Right, rect.Right);
            width = Math.Max(maxRight - left, 0);
        }

        // We need this check so that the math does not result in NaN
        if (rect.Height == double.PositiveInfinity || Height == double.PositiveInfinity)
        {
            height = double.PositiveInfinity;
        }
        else
        {
            //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
            var maxBottom = Math.Max(Bottom, rect.Bottom);
            height = Math.Max(maxBottom - top, 0);
        }
        // ReSharper restore CompareOfFloatsByEqualityOperator

        return new Rect(left, top, width, height);
    }


    /// <summary>
    ///     IntersectsWith - Returns true if the Rect intersects with this rectangle
    ///     Returns false otherwise.
    ///     Note that if one edge is coincident, this is considered an intersection.
    /// </summary>
    /// <returns>
    ///     Returns true if the Rect intersects with this rectangle
    ///     Returns false otherwise.
    ///     or Height
    /// </returns>
    /// <param name="rect"> Rect </param>
    public bool IntersectsWith(Rect rect)
    {
        if (IsEmpty || rect.IsEmpty)
        {
            return false;
        }

        return rect.Left <= Right &&
               rect.Right >= Left &&
               rect.Top <= Bottom &&
               rect.Bottom >= Top;
    }


    public override string ToString()
    {
        if (IsEmpty)
            return "Empty";
        var numericListSeparator = MsCompatibility.GetNumericListSeparator(null);
        return $"{X}{numericListSeparator}{Y}{numericListSeparator}{Width}{numericListSeparator}{Height}";
    }

    public static Rect Empty { get; } = CreateEmptyRect();

    public bool IsEmpty => Width < 0.0;

    public double X { get; }

    public double Y { get; }

    public double Width { get; }

    public double Height { get; }

    public double Left => X;

    public double Top => Y;


    public double Right
    {
        get
        {
            if (IsEmpty)
                return double.NegativeInfinity;
            return X + Width;
        }
    }


    public double Bottom
    {
        get
        {
            if (IsEmpty)
                return double.NegativeInfinity;
            return Y + Height;
        }
    }

    public Point TopLeft => new Point(Left, Top);

    public Point TopRight => new Point(Right, Top);

    public Point BottomLeft => new Point(Left, Bottom);

    public Point BottomRight => new Point(Right, Bottom);

    public Size Size => new Size(Width, Height);

    public Point Location => new Point(Left, Right);
}
