using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


#if TYPECONVERTERS

namespace iSukces.Mathematics.TypeConverters
{
    public class Coordinates3DTypeConverter : GenericTypeConverter<Coordinates3D>
    {
        protected override Coordinates3D ConvertFromStringInternal(ITypeDescriptorContext context, CultureInfo culture,
            string value)
        {
            if (string.Equals(value.Trim(), "identity", StringComparison.OrdinalIgnoreCase))
                return Coordinates3D.NORMAL;
            var separatorChar = GetSeparator(culture);
            var items = value.Split(separatorChar).Select(a => a.Trim().ToLower()).ToArray();
            var doubles = new List<double>(9);

          

            for (var index = 0; index < items.Length; index++)
            {
                var item = items[index];
                if (item.StartsWith("t", StringComparison.OrdinalIgnoreCase))
                {
                    var dx = DeserializeDouble(item.Substring(1), culture);
                    var dy = DeserializeDouble(items[index + 1], culture);
                    var dz = DeserializeDouble(items[index + 2], culture);
                    return Coordinates3D.FromTranslate(dx, dy, dz);
                }
                if (TryAdd(item, out var vector))
                {
                    doubles.Add(vector.X);
                    doubles.Add(vector.Y);
                    doubles.Add(vector.Z);
                }
                else
                    doubles.Add(DeserializeDouble(item, culture));
                if (doubles.Count > 8)
                    return Coordinates3D.FromDeserialized(
                        new Vector3D(doubles[0], doubles[1], doubles[2]),
                        new Vector3D(doubles[3], doubles[4], doubles[5]),
                        new Point3D(doubles[6], doubles[7], doubles[8]));
            }
            if (doubles.Count > 5)
                return Coordinates3D.FromDeserialized(
                    new Vector3D(doubles[0], doubles[1], doubles[2]),
                    new Vector3D(doubles[3], doubles[4], doubles[5]));
            throw PrepareArgumentException(value);
        }

        protected override string ConvertToStringInternal(ITypeDescriptorContext? context, CultureInfo? culture,
            Coordinates3D value)
        {
            var separator = GetSeparator(culture);
            var x = ConvertToString(value.X, culture);
            var y = ConvertToString(value.Y, culture);
            var translation = ConvertToString(value.Origin, culture);
            var data = x + separator + y;
            var ortogonal = "X" + separator + "Y";
            if (data == ortogonal && translation != "")
                return "T" + translation;
            if (translation != "") data += separator + translation;
            if (data == ortogonal) return Identity;
            return data;
        }
        
    }
}
#endif
