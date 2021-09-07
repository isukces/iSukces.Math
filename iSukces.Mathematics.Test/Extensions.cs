using System.Globalization;

namespace iSukces.Mathematics.test
{
    public static class Extensions
    {
        public static string ToInv(this double x) { return x.ToString(CultureInfo.InvariantCulture); }
    }
}
