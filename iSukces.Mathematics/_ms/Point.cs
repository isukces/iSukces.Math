// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

namespace iSukces.Mathematics;

/// <summary>
///     Point - Defaults to 0,0
/// </summary>
public readonly record struct Point(double X, double Y)
{
    /// <summary>
    ///     Multiply: Point * Matrix
    /// </summary>
    public static Point Multiply(Point point, Matrix matrix)
    {
        return matrix.Transform(point);
    }

    /// <summary>
    ///     Operator Point + Vector
    /// </summary>
    /// <returns>
    ///     Point - The result of the addition
    /// </returns>
    /// <param name="point"> The Point to be added to the Vector </param>
    /// <param name="vector"> The Vectr to be added to the Point </param>
    public static Point operator +(Point point, Vector vector)
    {
        return new Point(point.X + vector.X, point.Y + vector.Y);
    }

    /// <summary>
    ///     Explicit conversion to Vector
    /// </summary>
    /// <returns>
    ///     Vector - A Vector equal to this Point
    /// </returns>
    /// <param name="point"> Point - the Point to convert to a Vector </param>
    public static explicit operator Vector(Point point)
    {
        return new Vector(point.X, point.Y);
    }

    /// <summary>
    ///     Operator Point * Matrix
    /// </summary>
    public static Point operator *(Point point, Matrix matrix)
    {
        return matrix.Transform(point);
    }

    /// <summary>
    ///     Operator Point - Vector
    /// </summary>
    /// <returns>
    ///     Point - The result of the subtraction
    /// </returns>
    /// <param name="point"> The Point from which the Vector is subtracted </param>
    /// <param name="vector"> The Vector which is subtracted from the Point </param>
    public static Point operator -(Point point, Vector vector)
    {
        return new Point(point.X - vector.X, point.Y - vector.Y);
    }

    /// <summary>
    ///     Operator Point - Point
    /// </summary>
    /// <returns>
    ///     Vector - The result of the subtraction
    /// </returns>
    /// <param name="point1"> The Point from which point2 is subtracted </param>
    /// <param name="point2"> The Point subtracted from point1 </param>
    public static Vector operator -(Point point1, Point point2)
    {
        return new Vector(point1.X - point2.X, point1.Y - point2.Y);
    }
}
