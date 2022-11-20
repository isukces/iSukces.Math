#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif

namespace iSukces.Mathematics
{

    public interface IMatrixTransform2D
    {
        Matrix GetTransformMatrix();
    }

    public interface IInvertibleMatrixTransform2D
    {
        Matrix GetInverseMatrix();
    }

    public interface IPointTransformator2D
    {
        Point Transform(Point point);
    }

    public interface IVectorTransformator2D 
    {      
        Vector Transform(Vector vector);
    }

    public interface IInvertiblePointTransformator2D
    {
        Point ReverseTransform(Point point);
    }

    public interface IInvertibleVectorTransformator2D
    {
        Vector ReverseTransform(Vector vector);
    }
}
