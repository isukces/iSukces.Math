// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;
using System.Diagnostics;
using System.Globalization;

namespace iSukces.Mathematics;

internal sealed class MsCompatibility
{
    // Helper to get the numeric list separator for a given IFormatProvider.
    // Separator is a comma [,] if the decimal separator is not a comma, or a semicolon [;] otherwise.
    internal static char GetNumericListSeparator(IFormatProvider? provider)
    {
        var numericSeparator = ',';

        // Get the NumberFormatInfo out of the provider, if possible
        // If the IFormatProvider doesn't contain a NumberFormatInfo, then
        // this method returns the current culture's NumberFormatInfo.
        var numberFormat = NumberFormatInfo.GetInstance(provider);

        Debug.Assert(null != numberFormat);

        // Is the decimal separator is the same as the list separator?
        // If so, we use the ";".
        if (numberFormat.NumberDecimalSeparator.Length > 0 && numericSeparator == numberFormat.NumberDecimalSeparator[0])
        {
            numericSeparator = ';';
        }

        return numericSeparator;
    }
}
