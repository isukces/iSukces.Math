// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

internal static class DoubleUtil
{
    /// <summary>
    ///     IsOne - Returns whether or not the double is "close" to 1.  Same as AreClose(double, 1),
    ///     but this is faster.
    /// </summary>
    /// <returns>
    ///     bool - the result of the AreClose comparision.
    /// </returns>
    /// <param name="value"> The double to compare to 1. </param>
    public static bool IsOne(double value)
    {
        return Math.Abs(value - 1.0) < 10.0 * DBL_EPSILON;
    }


    /// <summary>
    ///     IsZero - Returns whether or not the double is "close" to 0.  Same as AreClose(double, 0),
    ///     but this is faster.
    /// </summary>
    /// <returns>
    ///     bool - the result of the AreClose comparision.
    /// </returns>
    /// <param name="value"> The double to compare to 0. </param>
    public static bool IsZero(double value)
    {
        return Math.Abs(value) < 10.0 * DBL_EPSILON;
    }

    internal const double DBL_EPSILON = 2.2204460492503131e-016; /* smallest such that 1.0+DBL_EPSILON != 1.0 */
    internal const float FLT_MIN = 1.175494351e-38F; /* Number close to zero, where float.MinValue is -float.MaxValue */
}
