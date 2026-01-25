/*using System;
using System.Globalization;


namespace iSukces.Mathematics;

internal static class Utils
{
    private const char Comma = ',';

    internal static char GetNumericListSeparator(IFormatProvider? provider)
    {
        var instance = NumberFormatInfo.GetInstance(provider);
        if (instance.NumberDecimalSeparator.Length > 0
            && Comma == instance.NumberDecimalSeparator[0])
            return ';';
        return Comma;
    }
    
}*/