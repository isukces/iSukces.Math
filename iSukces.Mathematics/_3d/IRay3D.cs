#if !WPFFEATURES
#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif


namespace iSukces.Mathematics;

public interface IRay3D
{
    Point3D Origin { get; }

    Vector3D Direction { get; }
}

