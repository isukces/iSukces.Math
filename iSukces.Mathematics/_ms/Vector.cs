// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

/// <summary>
///     Vector - A value type which defined a vector in terms of X and Y
/// </summary>
public readonly record struct Vector(double X, double Y)
{
    /// <summary>
    ///     AngleBetween - the angle between 2 vectors
    /// </summary>
    /// <returns>
    ///     Returns the the angle in degrees between vector1 and vector2
    /// </returns>
    /// <param name="vector1"> The first Vector </param>
    /// <param name="vector2"> The second Vector </param>
    public static double AngleBetween(Vector vector1, Vector vector2)
    {
        var sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
        var cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

        return Math.Atan2(sin, cos) * (180 / Math.PI);
    }

    /// <summary>
    ///     CrossProduct - Returns the cross product: vector1.X*vector2.Y - vector1.Y*vector2.X
    /// </summary>
    /// <returns>
    ///     Returns the cross product: vector1.X*vector2.Y - vector1.Y*vector2.X
    /// </returns>
    /// <param name="vector1"> The first Vector </param>
    /// <param name="vector2"> The second Vector </param>
    public static double CrossProduct(Vector vector1, Vector vector2)
    {
        return vector1.X * vector2.Y - vector1.Y * vector2.X;
    }

    /// <summary>
    ///     Multiply - Returns the dot product: vector1.X*vector2.X + vector1.Y*vector2.Y
    /// </summary>
    /// <returns>
    ///     Returns the dot product: vector1.X*vector2.X + vector1.Y*vector2.Y
    /// </returns>
    /// <param name="vector1"> The first Vector </param>
    /// <param name="vector2"> The second Vector </param>
    [Obsolete("Use * operator instead", false)]
    public static double Multiply(Vector vector1, Vector vector2)
    {
        return vector1.X * vector2.X + vector1.Y * vector2.Y;
    }

    /*
    /// <summary>
    /// Negates the values of X and Y on this Vector
    /// </summary>
    public void Negate()
    {
        _x = -_x;
        _y = -_y;
    }
    */

    /// <summary>
    ///     Operator Vector + Vector
    /// </summary>
    public static Vector operator +(Vector vector1, Vector vector2)
    {
        return new Vector(vector1.X + vector2.X,
            vector1.Y + vector2.Y);
    }

    /// <summary>
    ///     Operator Vector + Point
    /// </summary>
    public static Point operator +(Vector vector, Point point)
    {
        return new Point(point.X + vector.X, point.Y + vector.Y);
    }


    /// <summary>
    ///     Operator Vector / double
    /// </summary>
    public static Vector operator /(Vector vector, double scalar)
    {
        return vector * (1.0 / scalar);
    }

    /// <summary>
    ///     Explicit conversion to Point
    /// </summary>
    /// <returns>
    ///     Point - A Point equal to this Vector
    /// </returns>
    /// <param name="vector"> Vector - the Vector to convert to a Point </param>
    public static explicit operator Point(Vector vector)
    {
        return new Point(vector.X, vector.Y);
    }

    /// <summary>
    ///     Operator Vector * double
    /// </summary>
    public static Vector operator *(Vector vector, double scalar)
    {
        return new Vector(vector.X * scalar,
            vector.Y * scalar);
    }

    /// <summary>
    ///     Operator double * Vector
    /// </summary>
    public static Vector operator *(double scalar, Vector vector)
    {
        return new Vector(vector.X * scalar,
            vector.Y * scalar);
    }


    /// <summary>
    ///     Operator Vector * Matrix
    /// </summary>
    public static Vector operator *(Vector vector, Matrix matrix)
    {
        return matrix.Transform(vector);
    }

    /// <summary>
    ///     Operator Vector * Vector, interpreted as their dot product
    /// </summary>
    public static double operator *(Vector vector1, Vector vector2)
    {
        return vector1.X * vector2.X + vector1.Y * vector2.Y;
    }


    /// <summary>
    ///     Operator Vector - Vector
    /// </summary>
    public static Vector operator -(Vector vector1, Vector vector2)
    {
        return new Vector(vector1.X - vector2.X,
            vector1.Y - vector2.Y);
    }

    /// <summary>
    ///     Operator -Vector (unary negation)
    /// </summary>
    public static Vector operator -(Vector vector)
    {
        return new Vector(-vector.X, -vector.Y);
    }

    /// <summary>
    ///     Normalize - Updates this Vector to maintain its direction, but to have a length
    ///     of 1.  This is equivalent to dividing this Vector by Length
    /// </summary>
    public Vector GetNormalized()
    {
        // Avoid overflow
        var a = this / Math.Max(Math.Abs(X), Math.Abs(Y));
        var b = this / Length;
        return b;
    }

    /// <summary>
    ///     Length Property - the length of this Vector
    /// </summary>
    public double Length => Math.Sqrt(X * X + Y * Y);

    /// <summary>
    ///     LengthSquared Property - the squared length of this Vector
    /// </summary>
    public double LengthSquared => X * X + Y * Y;
}
