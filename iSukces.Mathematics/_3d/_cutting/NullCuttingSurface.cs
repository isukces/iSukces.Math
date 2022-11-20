using System.Collections.Generic;
using iSukces.Mathematics.Compatibility;

#if !WPFFEATURES
#else
using System.Windows.Media.Media3D;
#endif


namespace iSukces.Mathematics
{
    public sealed class NullCuttingSurface : ICuttingSurface
    {
        private NullCuttingSurface() { }


        /// <summary>
        ///     Oblicza wektor normalny we wskazanym punkcie
        /// </summary>
        /// <param name="x">współrzędna x</param>
        /// <param name="y">współrzędna y</param>
        /// <returns>wektor normalny</returns>
        public Vector3D? CalculateNormal(double x, double y) { return _normal; }

        public Point3D CalculatePoint(double x, double y) { return new Point3D(x, y, 0); }

        public Point3D CalculatePoint(double x, double y, double z) { return new Point3D(x, y, z); }

        /// <summary>
        ///     Oblicza wysokość obcięcia w punkcie x,y
        /// </summary>
        /// <param name="x">współrzędna x</param>
        /// <param name="y">współrzędna y</param>
        /// <returns>współrzędna z</returns>
        public double CalculateZ(double x, double y) { return 0; }


        public override bool Equals(object obj)
        {
            return obj is null || obj is NullCuttingSurface;
        }

        public string GetCompareString() { return nameof(NullCuttingSurface); }

        public override int GetHashCode() { return 0; }

        /// <summary>
        ///     Zwraca listę współrzędnych x, na których jest załamanie płaszczyzny
        /// </summary>
        /// <param name="xmin">początek zakresu</param>
        /// <param name="xmax">koniec zakresu zakresu</param>
        /// <returns>lista współrzędnych</returns>
        public IList<double> GetXEdges(double xmin, double xmax) { return null; }

        /// <summary>
        ///     Zwraca listę współrzędnych y, na których jest załamanie płaszczyzny
        /// </summary>
        /// <param name="ymin">początek zakresu</param>
        /// <param name="ymax">koniec zakresu zakresu</param>
        /// <returns>lista współrzędnych</returns>
        public IList<double> GetYEdges(double ymin, double ymax) { return null; }

        public override string ToString() { return nameof(NullCuttingSurface); }


        public bool HasSharpYEdges => false;


        public static NullCuttingSurface Instance => NullCuttingSurfaceHolder.SingleIstance;

        private readonly Vector3D _normal = new Vector3D(0, 0, 1);

        private static class NullCuttingSurfaceHolder
        {
            public static readonly NullCuttingSurface SingleIstance = new NullCuttingSurface();
        }
    }
}
