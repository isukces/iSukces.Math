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


#if WPFFEATURES

namespace iSukces.Mathematics.TypeConverters
{
    public class Coordinates3DTypeConverter : GenericTypeConverter<Coordinates3D>
    {
        private static string ConvertToString(Vector3D v, CultureInfo culture)
        {
            if (v.Equals(Coordinates3D.XVersor)) return "X";
            if (v.Equals(Coordinates3D.YVersor)) return "Y";
            if (v.Equals(Coordinates3D.ZVersor)) return "Z";

            if (v.Equals(Coordinates3D.XMinusVersor)) return "-X";
            if (v.Equals(Coordinates3D.YMinusVersor)) return "-Y";
            if (v.Equals(Coordinates3D.ZMinusVersor)) return "-Z";
            var separator = GetSeparator(culture);
            var data = v.X.ToString(culture) + separator + v.Y.ToString(culture) + separator + v.Z.ToString(culture);
            return data;
        }

        private static string ConvertToString(Point3D v, CultureInfo culture)
        {
            if (v.Equals(new Point3D())) return "";
            var separator = GetSeparator(culture);
            var data = v.X.ToString(culture) + separator + v.Y.ToString(culture) + separator + v.Z.ToString(culture);
            return data;
        }

        protected override Coordinates3D ConvertFromStringInternal(ITypeDescriptorContext context, CultureInfo culture,
            string value)
        {
            if (string.Equals(value.Trim(), "identity", StringComparison.OrdinalIgnoreCase))
                return Coordinates3D.NORMAL;
            var separatorChar = GetSeparator(culture);
            var items = value.Split(separatorChar).Select(a => a.Trim().ToLower()).ToArray();
            var doubles = new List<double>(9);
            Func<string, Vector3D?> tryAdd = x =>
            {
                if (x == "x") return Coordinates3D.XVersor;
                if (x == "y") return Coordinates3D.YVersor;
                if (x == "z") return Coordinates3D.ZVersor;
                if (x == "-x") return Coordinates3D.XMinusVersor;
                if (x == "-y") return Coordinates3D.YMinusVersor;
                if (x == "-z") return Coordinates3D.ZMinusVersor;
                return null;
            };
            for (var index = 0; index < items.Length; index++)
            {
                var item = items[index];
                if (item.StartsWith("t"))
                {
                    var dx = DeserializeDouble(item.Substring(1), culture);
                    var dy = DeserializeDouble(items[index + 1], culture);
                    var dz = DeserializeDouble(items[index + 2], culture);
                    return Coordinates3D.FromTranslate(dx, dy, dz);
                }
                var vector = tryAdd(item);
                if (vector != null)
                {
                    doubles.Add(vector.Value.X);
                    doubles.Add(vector.Value.Y);
                    doubles.Add(vector.Value.Z);
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

        protected override string ConvertToStringInternal(ITypeDescriptorContext context, CultureInfo culture,
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


        private const string Identity = "Identity";
    }
}
#endif