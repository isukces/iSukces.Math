#if TYPECONVERTERS
using System.ComponentModel;
using System.Globalization;

#if !WPFFEATURES
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;
using iSukces.Mathematics.Compatibility;

#else
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif

namespace iSukces.Mathematics.TypeConverters;

public abstract class MathTypeConverter : TypeConverter
{
    static MathTypeConverter()
    {
        var ic = CultureInfo.InvariantCulture;
        _invariantCultureSeparator = ic.TextInfo.ListSeparator[0];
        _invariantCultureLcid      = ic.LCID;
    }

    public static string ConvertToString(Vector3D v, CultureInfo culture)
    {
        if (v.Equals(Coord3D.XVersor)) return "X";
        if (v.Equals(Coord3D.YVersor)) return "Y";
        if (v.Equals(Coord3D.ZVersor)) return "Z";

        if (v.Equals(Coord3D.XMinusVersor)) return "-X";
        if (v.Equals(Coord3D.YMinusVersor)) return "-Y";
        if (v.Equals(Coord3D.ZMinusVersor)) return "-Z";
        var separator = GetSeparator(culture);
        var data      = v.X.ToString(culture) + separator + v.Y.ToString(culture) + separator + v.Z.ToString(culture);
        return data;
    }

    public static string ConvertToString(Point3D v, CultureInfo culture)
    {
        if (v.Equals(new Point3D())) return "";
        var separator = GetSeparator(culture);
        var data      = v.X.ToString(culture) + separator + v.Y.ToString(culture) + separator + v.Z.ToString(culture);
        return data;
    }

    protected static char GetSeparator(CultureInfo culture)
    {
        if (culture.LCID == _invariantCultureLcid)
            return _invariantCultureSeparator;
        var c = culture.TextInfo.ListSeparator.Trim();
        return c.Length == 1 ? c[0] : '|';
    }

    public static bool TryAdd(string x, out Vector3D v)
    {
        switch (x)
        {
            case "x":
                v = Coordinates3D.XVersor;
                return true;
            case "y":
                v = Coordinates3D.YVersor;
                return true;
            case "z":
                v = Coordinates3D.ZVersor;
                return true;
            case "-x":
                v = Coordinates3D.XMinusVersor;
                return true;
            case "-y":
                v = Coordinates3D.YMinusVersor;
                return true;
            case "-z":
                v = Coordinates3D.ZMinusVersor;
                return true;
            default:
                v = default;
                return false;
        }
    }

    #region Fields

    private static readonly char _invariantCultureSeparator;
    private static readonly int _invariantCultureLcid;

    public const string Empty = "Empty";

    public const string Identity = "Identity";

    #endregion
}
#endif

