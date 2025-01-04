using System;
#if !WPFFEATURES
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint=System.Windows.Point;
using TheVector=System.Windows.Vector;
#endif


namespace iSukces.Mathematics;

public static class Transformator2DExtension
{
    public static ThePoint TransformEx(this IPointTransformator2D transform, double x, double y)
    {
        if (transform is null) throw new ArgumentNullException(nameof(transform));
        return transform.Transform(new ThePoint(x, y));
    }

    public static ThePoint TransformEx(this IMatrixTransform2D transform, double x, double y)
    {
        if (transform is null) throw new ArgumentNullException(nameof(transform));
        return new ThePoint(x, y) * transform.GetTransformMatrix();
    }

    public static ThePoint TransformEx(this IMatrixTransform2D transform, ThePoint point)
    {
        if (transform is null) throw new ArgumentNullException(nameof(transform));
        return point * transform.GetTransformMatrix();
    }

    public static ThePoint ReverseTransformEx(this IInvertibleMatrixTransform2D transform, double x, double y)
    {
        if (transform is null) throw new ArgumentNullException(nameof(transform));
        return new ThePoint(x, y) * transform.GetInverseMatrix();
    }

    public static ThePoint ReverseTransformEx(this IInvertibleMatrixTransform2D transform, ThePoint point)
    {
        if (transform is null) throw new ArgumentNullException(nameof(transform));
        return point * transform.GetInverseMatrix();
    }


    public static TheVector TransformEx(this IMatrixTransform2D transform, TheVector vector)
    {
        if (transform is null) throw new ArgumentNullException(nameof(transform));
        return vector * transform.GetTransformMatrix();
    }

    public static TheVector ReverseTransformEx(this IInvertibleMatrixTransform2D transform, TheVector vector)
    {
        if (transform is null) throw new ArgumentNullException(nameof(transform));
        return vector * transform.GetInverseMatrix();
    }
}