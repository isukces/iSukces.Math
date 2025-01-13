#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics;

public static class SizeExtension
{
    /// <summary>
    ///     czy rozmiar ma pole powierzchni > 0
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static bool HasArea(this Size size)
    {
        return !size.IsEmpty && size.Width > 0 && size.Height > 0;
    }
}