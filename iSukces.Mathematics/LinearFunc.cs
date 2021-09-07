using System;
#if COREFX
using ThePoint=iSukces.Mathematics.Compatibility.Point;

#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint=System.Windows.Point;
using TheVector=System.Windows.Vector;
#endif

namespace iSukces.Mathematics
{
    /// <summary>
    /// Funkcja liniowa y=ax+b
    /// </summary>
    public sealed class LinearFunc
    {
        /// <summary>
        /// Punkt zerowy - taki X dla którego Y=0
        /// </summary>
        public double ZeroPoint
        {
            get { return -B / A; }
        }

        public LinearFunc(double x1, double y1, double a)
        {
            A = a;
            B = y1 - a * x1;
        }
        public override string ToString()
        {
            return string.Format("Line {0} {1}", A, B);
        }

        public LinearFunc()
        {
        }
        public LinearFunc(double a, double b)
        {
            A = a;
            B = b;
        }

        public LinearFunc(ThePoint p1, ThePoint p2)
        {
            /*
                   p1y - p2y 
a1 ≔ ⎯⎯⎯⎯⎯⎯⎯⎯⎯
      p1x - p2x 
      p1x·p2y - p1y·p2x 
b1 ≔ ⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯
          p1x - p2x
             */
            double dx = p1.X - p2.X;
            if (dx == 0)
                throw new ArgumentException("Punkty nie mogą mieć tego samego X");
            A = (p1.Y - p2.Y) / dx;
            B = (p1.X * p2.Y - p1.Y * p2.X) / dx;
        }

        /// <summary>
        /// współczynnik A
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// współczynnik B
        /// </summary>
        public double B { get; set; }


        /// <summary>
        /// Miejsce zerowe
        /// </summary>
        public double X0
        {
            get { return -B / A; }
        }

        /// <summary>
        /// Kąt prostej względem osi OX
        /// </summary>
        public double AngleDeg
        {
            get { return MathEx.AtanDeg(A); }
        }

        public double Value(double x)
        {
            return A * x + B;
        }


        /// <summary>
        /// Punkt przecięcia z inną linią
        /// </summary>
        /// <param name="otherLine"></param>
        /// <returns></returns>
        public ThePoint? Cross(LinearFunc otherLine)
        {
            /*
⎡     b2 - b1         a1·b2 - a2·b1 ⎤
⎢x = ⎯⎯⎯⎯⎯⎯⎯ ∧ y = ⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎥
⎣     a1 - a2            a1 - a2    ⎦
             */
            double d = A - otherLine.A;
            if (d == 0) return null;
            return new ThePoint(
                (otherLine.B - B) / d,
                (A * otherLine.B - otherLine.A * B) / d
                );
        }
    }
}
