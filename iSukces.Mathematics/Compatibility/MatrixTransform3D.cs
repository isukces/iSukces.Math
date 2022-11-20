#if !WPFFEATURES
namespace iSukces.Mathematics.Compatibility
{
    public sealed class MatrixTransform3D : Transform3D
    {
        public MatrixTransform3D(Matrix3D matrix)
        {
            Matrix = matrix;
        }

        public Matrix3D Matrix { get; }
    }
}
#endif