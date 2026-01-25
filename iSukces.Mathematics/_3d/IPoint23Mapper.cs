#if !WPFFEATURES

#else
using System.Windows.Media.Media3D;
using System.Windows;
#endif

namespace iSukces.Mathematics;

/// <summary>
///     Mapuje płaski punkt do punktu 3D - użyteczne dla potrzeb budowania meshy
/// </summary>
public interface IPoint23Mapper
{
    Point3D MapPoint23(Point srcPoint);
}