#if TYPECONVERTERS
using System;
using System.ComponentModel;
using System.Globalization;

namespace iSukces.Mathematics.TypeConverters;

public class RangeTypeConverter : GenericTypeConverter<Range>
{
    protected override Range ConvertFromStringInternal(ITypeDescriptorContext context, CultureInfo culture,
        string value)
    {
        if (string.Equals(value.Trim(), Empty, StringComparison.OrdinalIgnoreCase))
            return Range.Empty;
        var a = value.Split(GetSeparator(culture));
        if (a.Length < 2) throw PrepareArgumentException(value);
        return new Range(DeserializeDouble(a[0], culture), DeserializeDouble(a[1], culture));
    }

    protected override string ConvertToStringInternal(ITypeDescriptorContext context, CultureInfo culture,
        Range value)
    {
        if (value.IsEmpty)
            return Empty;
        return value.Min.ToString(culture) + GetSeparator(culture) + value.Max.ToString(culture);
    }
}
#endif
