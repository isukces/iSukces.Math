#if ALLFEATURES
using System.ComponentModel;
using System.Globalization;

namespace iSukces.Mathematics.TypeConverters
{
    public class MinMaxTypeConverter : GenericTypeConverter<MinMax>
    {
        protected override MinMax ConvertFromStringInternal(ITypeDescriptorContext context, CultureInfo culture,
            string value)
        {
            var a = value.Split(GetSeparator(culture));
            if (a.Length < 2) throw PrepareArgumentException(value);
            return new MinMax(DeserializeDouble(a[0], culture), DeserializeDouble(a[1], culture));
        }

        protected override string ConvertToStringInternal(ITypeDescriptorContext context, CultureInfo culture,
            MinMax value)
        {
            return value.Min.ToString(culture) + GetSeparator(culture) + value.Max.ToString(culture);
        }
    }
}
#endif