using System;

namespace iSukces.Mathematics;

public static class Transformator2DExtension
{
    extension(IPointTransformator2D transform)
    {
        public Point TransformEx(double x, double y)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return transform.Transform(new Point(x, y));
        }
    }

    extension(IMatrixTransform2D transform)
    {
        public Point TransformEx(double x, double y)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return new Point(x, y) * transform.GetTransformMatrix();
        }

        public Point TransformEx(Point point)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return point * transform.GetTransformMatrix();
        }

        public Vector TransformEx(Vector vector)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return vector * transform.GetTransformMatrix();
        }
    }

    extension(IInvertibleMatrixTransform2D transform)
    {
        public Point ReverseTransformEx(double x, double y)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return new Point(x, y) * transform.GetInverseMatrix();
        }

        public Point ReverseTransformEx(Point point)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return point * transform.GetInverseMatrix();
        }

        public Vector ReverseTransformEx(Vector vector)
        {
            if (transform is null) throw new ArgumentNullException(nameof(transform));
            return vector * transform.GetInverseMatrix();
        }
    }
}
