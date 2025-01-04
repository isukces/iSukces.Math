#if TYPECONVERTERS
using System.ComponentModel;
using System.Globalization;

namespace iSukces.Mathematics.TypeConverters;

public class MinMaxITypeConverter : GenericTypeConverter<MinMaxI>
{
    protected override MinMaxI ConvertFromStringInternal(ITypeDescriptorContext context, CultureInfo culture,
        string value)
    {
        var a = Split(value, culture);
        if (a.Length < 2) throw PrepareArgumentException(value);
        return new MinMaxI(DeserializeInt(a[0], culture), DeserializeInt(a[1], culture));
    }

    protected override string ConvertToStringInternal(ITypeDescriptorContext context, CultureInfo culture,
        MinMaxI value)
    {
        return value.Min.ToString(culture) + GetSeparator(culture) + value.Max.ToString(culture);
    }
}
#endif
