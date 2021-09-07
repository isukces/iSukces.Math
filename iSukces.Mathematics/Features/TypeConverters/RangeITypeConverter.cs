#if ALLFEATURES

using System;
using System.ComponentModel;
using System.Globalization;

namespace iSukces.Mathematics.TypeConverters
{
    public class RangeITypeConverter : GenericTypeConverter<RangeI>
    {
        protected override RangeI ConvertFromStringInternal(ITypeDescriptorContext context, CultureInfo culture,
            string value)
        {
            if (string.Equals(value.Trim(), Empty, StringComparison.OrdinalIgnoreCase))
                return RangeI.Empty;
            var a = value.Split(GetSeparator(culture));
            if (a.Length < 2) throw PrepareArgumentException(value);
            return new RangeI(DeserializeInt(a[0], culture), DeserializeInt(a[1], culture));
        }

        protected override string ConvertToStringInternal(ITypeDescriptorContext context, CultureInfo culture,
            RangeI value)
        {
            if (value.IsEmpty) return Empty;
            return value.Min.ToString(culture) + GetSeparator(culture) + value.Max.ToString(culture);
        }
    }
}
#endif