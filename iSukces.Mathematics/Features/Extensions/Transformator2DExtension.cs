using System;
#if !WPFFEATURES
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;

#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics;

public static class Transformator2DExtension
{
    extension(IPointTransformator2D transform)
    {
        public ThePoint TransformEx(double x, double y)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return transform.Transform(new ThePoint(x, y));
        }
    }

    extension(IMatrixTransform2D transform)
    {
        public ThePoint TransformEx(double x, double y)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return new ThePoint(x, y) * transform.GetTransformMatrix();
        }

        public ThePoint TransformEx(ThePoint point)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return point * transform.GetTransformMatrix();
        }

        public TheVector TransformEx(TheVector vector)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return vector * transform.GetTransformMatrix();
        }
    }

    extension(IInvertibleMatrixTransform2D transform)
    {
        public ThePoint ReverseTransformEx(double x, double y)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return new ThePoint(x, y) * transform.GetInverseMatrix();
        }

        public ThePoint ReverseTransformEx(ThePoint point)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return point * transform.GetInverseMatrix();
        }

        public TheVector ReverseTransformEx(TheVector vector)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return vector * transform.GetInverseMatrix();
        }
    }
}
