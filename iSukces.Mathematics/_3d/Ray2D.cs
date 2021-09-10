#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.Mathematics
{
    public sealed class Ray2D : IPoint12Mapper
    {
        public Ray2D()
        {
        }

        public Ray2D(Point _beginPoint, Point _endPoint)
        {
            BeginPoint = _beginPoint;
            axis       = _endPoint - _beginPoint;
            Distance   = axis.Length;
            axis.Normalize();
        }

        public Ray2D(Point _beginPoint, Vector _axis)
        {
            BeginPoint = _beginPoint;
            axis       = _axis;
            Distance   = axis.Length;
            axis.Normalize();
        }

        public static Point CrossLines(Ray2D ray1, Ray2D ray2)
        {
            var m = ray1.axis.Y * ray2.axis.X - ray1.axis.X * ray2.axis.Y;
            var k = (ray1.BeginPoint.X * ray2.axis.Y - ray1.BeginPoint.Y * ray2.axis.X -
                ray2.BeginPoint.X * ray2.axis.Y + ray2.BeginPoint.Y * ray2.axis.X) / m;
            // double l = (ray1.beginPoint.X * ray1.axis.Y - ray1.beginPoint.Y * ray1.axis.X + ray1.axis.X * ray2.beginPoint.Y - ray1.axis.Y * ray2.beginPoint.X) / m;
            return ray1.GetPoint(k);
        }

        /// <summary>
        ///     Iloczyn skalarny wektorów
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double DotProduct(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        ///     Rzutuje wektor v na układ współrzędnych, którego oś x wyznacza axis
        /// </summary>
        /// <param name="axis">oś X układu (zakładam długość 1)</param>
        /// <param name="v">rzutowany wektor</param>
        /// <returns></returns>
        public static Vector MapVector(Vector axis, Vector v)
        {
            return new Vector(
                v.X * axis.X + v.Y * axis.Y,
                -v.X * axis.Y + v.Y * axis.X);
        }

        public static Point operator *(Point p, Ray2D r)
        {
            //return beginPoint + p.X * axis + p.Y * (new Vector(-axis.Y, axis.X));
            return new Point(
                r.BeginPoint.X + p.X * r.axis.X - p.Y * r.axis.Y,
                r.BeginPoint.Y + p.X * r.axis.Y + p.Y * r.axis.X
            );
        }

        public double? Cross(Ray2D r)
        {
            var v = r.BeginPoint - BeginPoint;

            var a = MapVector(axis, v);
            var b = MapVector(axis, r.axis);

            try
            {
                return a.X + a.Y * (b.X / b.Y);
            }
            catch
            {
                return null;
            }
        }

        public Point GetPoint(double x)
        {
            return new Point(BeginPoint.X + x * axis.X,
                BeginPoint.Y + x * axis.Y);
        }

        public double Map(Point p)
        {
            return (p.X - BeginPoint.X) * axis.X + (p.Y - BeginPoint.Y) * axis.Y;
        }

        public double Map(double x, double y)
        {
            return (x - BeginPoint.X) * axis.X + (y - BeginPoint.Y) * axis.Y;
        }

        public override string ToString()
        {
            return string.Format("[{0}] => [{3}], axis={1}, distance={2}", BeginPoint, axis, Distance,
                BeginPoint + axis * Distance);
        }

        Point IPoint12Mapper.MapPoint12(double x)
        {
            return BeginPoint + x * axis;
        }

        /// <summary>
        ///     Punkt początkowy
        /// </summary>
        public Point BeginPoint { get; set; }

        /// <summary>
        ///     wektor
        /// </summary>
        public Vector Axis
        {
            get { return axis; }
            set
            {
                value.Normalize();
                if (value == axis) return;
                axis = value;
            }
        }

        /// <summary>
        ///     Odległość między punktami bazowymi, wartość pomocnicza
        /// </summary>
        public double Distance { get; set; }

        private Vector axis;
    }
}