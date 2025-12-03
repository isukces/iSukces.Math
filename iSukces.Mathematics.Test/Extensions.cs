using System.Globalization;

namespace iSukces.Mathematics.test;

public static class Extensions
{
    extension(double x)
    {
        public string ToInv() { return x.ToString(CultureInfo.InvariantCulture); }
    }
}