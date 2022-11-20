using System;
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
    /// <summary>
    ///     Prosty układ współrzędnych obsługujący jedynie przesunięcie i obrót
    /// </summary>
    public class SimpleCoordinateSystem2D : ICloneable
    {
        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        public SimpleCoordinateSystem2D()
        {
            Ax = 1;
            By = 1;
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        /// <param name="origin">początek układu współrzędnych</param>
        public SimpleCoordinateSystem2D(ThePoint origin) : this()
        {
            Ax = 1;
            By = 1;
            Dx = origin.X;
            Dy = origin.Y;
        }

        public static SimpleCoordinateSystem2D From2Points(ThePoint a, ThePoint b)
        {
            return FromPointAndVector(a, b - a);
        }

        public static SimpleCoordinateSystem2D FromPointAndVector(ThePoint a, TheVector v)
        {
            var c = new SimpleCoordinateSystem2D(a);

            v.Normalize();
            c.Ax = v.X;
            c.Bx = -v.Y;
            c.Ay = v.Y;
            c.By = v.X;
            return c;
        }

        public static SimpleCoordinateSystem2D FromRotateAndTranslate(double angleDeg, double x, double y)
        {
            var r = new SimpleCoordinateSystem2D();
            r.Rotate(angleDeg);
            r.Translate(x, y);
            return r;
        }

        public static SimpleCoordinateSystem2D FromRotateAndTranslate(double angleDeg, ThePoint point)
        {
            return FromRotateAndTranslate(angleDeg, point.X, point.Y);
        }

        /// <summary>
        ///     Implementuje operator sumowania układu współrzędnych i wektora - przesuwa początek układu współrzędnych o
        ///     wektor
        /// </summary>
        /// <param name="cs">układ współrzędnych</param>
        /// <param name="translateVector">wektor przesunięcia</param>
        /// <returns>przesunięty układ współrzędnych</returns>
        public static SimpleCoordinateSystem2D operator +(SimpleCoordinateSystem2D cs, TheVector translateVector)
        {
            var c = (SimpleCoordinateSystem2D)cs.Clone();
            c.Translate(translateVector);
            return c;
        }


        public static ThePoint operator *(ThePoint p, SimpleCoordinateSystem2D cs)
        {
            return cs.Transform(p);
        }


        public static ThePoint[] operator *(ThePoint[] a, SimpleCoordinateSystem2D b)
        {
            return b.Transform(a);
        }

        public static SimpleCoordinateSystem2D operator *(SimpleCoordinateSystem2D a, SimpleCoordinateSystem2D b)
        {
            // [[a_ax*b_ax+a_ay*b_bx,   a_ax*b_ay+a_ay*b_by,0],
            // [a_bx*b_ax+a_by*b_bx,    a_bx*b_ay+a_by*b_by,0],
            // [a_dx*b_ax+a_dy*b_bx+b_dx,    a_dx*b_ay+a_dy*b_by+b_dy,1]]

            var cs = new SimpleCoordinateSystem2D();
            cs.Ax = a.Ax * b.Ax + a.Ay * b.Bx;
            cs.Ay = a.Ax * b.Ay + a.Ay * b.By;

            cs.Bx = a.Bx * b.Ax + a.By * b.Bx;
            cs.By = a.Bx * b.Ay + a.By * b.By;

            cs.Dx = a.Dx * b.Ax + a.Dy * b.Bx + b.Dx;
            cs.Dy = a.Dx * b.Ay + a.Dy * b.By + b.Dy;
            return cs;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }


        public void CopyTo(SimpleCoordinateSystem2D cs)
        {
            cs.Ax = Ax;
            cs.Ay = Ay;
            cs.Bx = Bx;
            cs.By = By;
            cs.Dx = Dx;
            cs.Dy = Dy;
        }

        /// <summary>
        ///     Zmienia znak współrzędnej X
        /// </summary>
        public void FlipX()
        {
            Ax = -Ax;
            Bx = -Bx;
            Dx = -Dx;
        }

        /// <summary>
        ///     Zmienia znak współrzędnej Y
        /// </summary>
        public void FlipY()
        {
            Ay = -Ay;
            By = -By;
            Dy = -Dy;
        }

        public SimpleCoordinateSystem2D MultiplyOrigin(double factor)
        {
            return new SimpleCoordinateSystem2D
            {
                Ax = Ax,
                Ay = Ay,
                Bx = Bx,
                By = By,
                Dx = Dx * factor,
                Dy = Dy * factor
            };
        }

        public Matrix MultiplyOriginAsMatrix(double factor)
        {
            return new Matrix(
                Ax, Ay,
                Bx, By,
                Dx * factor, Dy * factor);
        }

        public SimpleCoordinateSystem2D Reverse()
        {
            var cs = new SimpleCoordinateSystem2D();
            var tmp = Ay * Bx - Ax * By;
            cs.Ax = -By / tmp;
            cs.Ay = Ay / tmp;
            cs.Bx = Bx / tmp;
            cs.By = -Ax / tmp;
            cs.Dx = (By * Dx - Bx * Dy) / tmp;
            cs.Dy = (Ax * Dy - Ay * Dx) / tmp;
            return cs;
        }

        /// <summary>
        ///     Obraca układ współrzędnych o wskazany kąt
        /// </summary>
        /// <param name="angleDeg">kąt w stopniach</param>
        public void Rotate(double angleDeg)
        {
            angleDeg *= Math.PI / 180.0;
            var s = Math.Sin(angleDeg);
            var c = Math.Cos(angleDeg);
            var cs = new SimpleCoordinateSystem2D
            {
                Ax = c,
                Ay = s,
                Bx = -s,
                By = c
            };

            cs = this * cs;
            cs.CopyTo(this);
        }

        public void Scale(double scale)
        {
            var cs = new SimpleCoordinateSystem2D();
            cs.Ax = scale;
            cs.By = scale;
            cs = this * cs;
            cs.CopyTo(this);
        }

        public override string ToString()
        {
            return string.Format("[ {0} {1} ] [ {2} {3} ] [ {4} {5} ]", Ax, Ay, Bx, By, Dx, Dy);
        }

        public Point Transform(Point src)
        {
            return new Point(
                Ax * src.X + Bx * src.Y + Dx,
                Ay * src.X + By * src.Y + Dy);
        }


        public Point[] Transform(Point[] src)
        {
#if USE_LINQ_IN_OLD_METHODS
            return src.Select(x => Transform(x)).ToArray();
#else
            var dst = new Point[src.Length];
            var i = 0;
            foreach (var p in src)
                dst[i++] = Transform(p);
            return dst;
#endif
        }

        public Point TransformRev(Point src)
        {
            var cs = Reverse();
            return cs.Transform(src);
        }

        public void Translate(double dx, double dy)
        {
            Dx += dx;
            Dy += dy;
        }

        /// <summary>
        ///     Przesuwa początek układu współrzędnych o wskazany wektor
        /// </summary>
        /// <param name="translateVector">wektor przesunięcia</param>
        public void Translate(Vector translateVector)
        {
            Origin += translateVector;
        }

        public static SimpleCoordinateSystem2D Rotated90
        {
            get
            {
                var r = new SimpleCoordinateSystem2D
                {
                    Ax = 0.0,
                    Bx = -1.0,
                    Ay = 1.0,
                    By = 0
                };

                return r;
            }
        }

        public static SimpleCoordinateSystem2D Rotated_90
        {
            get
            {
                var r = new SimpleCoordinateSystem2D
                {
                    Ax = 0.0,
                    Bx = 1.0,
                    Ay = -1.0,
                    By = 0
                };

                return r;
            }
        }

        public double Ax { get; private set; }

        public double Ay { get; private set; }

        public double Bx { get; private set; }

        public double By { get; private set; }

        public double Dx { get; private set; }

        public double Dy { get; private set; }

        public Matrix Matrix
        {
            get
            {
                return new Matrix(
                    Ax, Ay,
                    Bx, By,
                    Dx, Dy);
            }
        }

        public Point Origin
        {
            get { return new Point(Dx, Dy); }
            private set
            {
                Dx = value.X;
                Dy = value.Y;
            }
        }

        /// <summary>
        ///     Skala przez jaką zostanie przemnożona długość wersora OX przy transformacji
        /// </summary>
        public double XScale
        {
            get { return Math.Sqrt(Ax * Ax + Bx * Bx); }
        }
    }
}