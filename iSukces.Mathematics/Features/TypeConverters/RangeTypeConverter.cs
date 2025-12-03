#if TYPECONVERTERS
using System;
using System.ComponentModel;
using System.Globalization;

namespace iSukces.Mathematics.TypeConverters;

public class RangeTypeConverter : GenericTypeConverter<DRange>
{
    protected override DRange ConvertFromStringInternal(ITypeDescriptorContext context, CultureInfo culture,
        string value)
    {
        if (string.Equals(value.Trim(), Empty, StringComparison.OrdinalIgnoreCase))
            return DRange.Empty;
        var a = value.Split(GetSeparator(culture));
        if (a.Length < 2) throw PrepareArgumentException(value);
        return new DRange(DeserializeDouble(a[0], culture), DeserializeDouble(a[1], culture));
    }

    protected override string ConvertToStringInternal(ITypeDescriptorContext? context, CultureInfo culture,
        DRange value)
    {
        if (value.IsEmpty)
            return Empty;
        return value.Min.ToString(culture) + GetSeparator(culture) + value.Max.ToString(culture);
    }
}
#endif
