#if TYPECONVERTERS

using System;
using System.ComponentModel;
using System.Globalization;

namespace iSukces.Mathematics.TypeConverters
{
    public abstract class GenericTypeConverter<T> : MathTypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var txt = value as string;
            return txt != null
                ? ConvertFromStringInternal(context, culture, txt)
                : base.ConvertFrom(context, culture, value);
        }

        protected abstract T ConvertFromStringInternal(ITypeDescriptorContext context, CultureInfo culture, string value);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            return destinationType == typeof(string)
                ? ConvertToStringInternal(context, culture, (T)value)
                : base.ConvertTo(context, culture, value, destinationType);
        }

        protected abstract string ConvertToStringInternal(ITypeDescriptorContext context, CultureInfo culture, T value);

        protected double DeserializeDouble(string x, CultureInfo culture)
        {
            return double.Parse(x.Trim(), culture);
        }

        protected int DeserializeInt(string x, CultureInfo culture)
        {
            return int.Parse(x.Trim(), culture);
        }

        protected ArgumentException PrepareArgumentException(string value)
        {
            return new ArgumentException("Unable to convert '" + value + "' to " + typeof(T));
        }

        protected string[] Split(string x, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(x)) return new[] { x ?? string.Empty };
            var separator = GetSeparator(culture);
            return x.Split(separator);
        }
    }
}
#endif
