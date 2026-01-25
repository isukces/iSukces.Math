using System;
#if !WPFFEATURES

#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif


namespace iSukces.Mathematics;

public static class VectorExtensions
{


    extension(Vector v)
    {
        public Vector SetLength(double newLength)
        {
            return v * (newLength / v.Length);
        }

        public Vector3D ToVector3D()
        {
            return new Vector3D(v.X, v.Y, 0);
        }
    }

}