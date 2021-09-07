#if COREFX
using ThePoint=iSukces.Mathematics.Compatibility.Point;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint=System.Windows.Point;
using TheVector=System.Windows.Vector;
#endif
using System;


namespace iSukces.Mathematics
{
    public class CircleOnPlane : Circle, ICloneable
    {
        /// <summary>
        /// Tworzy instancję obiektu
        /// </summary>
        public CircleOnPlane()
        {
        }

        /// <summary>
        /// Tworzy instancję obiektu na podstawie promienia
        /// </summary>
        /// <param name="radius">promień</param>
        public CircleOnPlane(double radius)
            : base(radius)
        {
        }

        /// <summary>
        /// Tworzy instancję obiektu na podstawie promienia i współrzędnych środka
        /// </summary>
        /// <param name="radius">promień</param>
        /// <param name="center">środek</param>
        public CircleOnPlane(double radius, ThePoint center)
            : base(radius)
        {
            this.Center = center;
        }

        /// <summary>
        /// Tworzy instancję obiektu na podstawie środka i dowolnego punktu na obwodzie
        /// </summary>
        /// <param name="center">środek</param>
        /// <param name="anyPointOnPlane">dowolny punkt na obwodzie</param>
        public CircleOnPlane(ThePoint center, ThePoint anyPointOnPlane)
            : this((center - anyPointOnPlane).Length, center)
        {
        }


        /// <summary>
        /// Oblicza punkty przecięcia okręgu z linią (nieskończona długość) przechodzącą przez 2 punkty
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public ThePoint[] CrossLine(ThePoint a, ThePoint b)
        {
            ThePoint[] result;
            LineEquation le = LineEquation.FromPointAndDeltas(a.X - Center.X, a.Y - Center.Y, b.X - a.X, b.Y - a.Y);
            if (Math.Abs(le.A) > Math.Abs(le.B))
            {
                // pionowa
                SquareEquation se = new SquareEquation(
                  1 + MathEx.Sqr(le.B / le.A),
                  -2 * le.B * le.C / le.A,
                  MathEx.Sqr(le.C / le.A) - MathEx.Sqr(_radius)
                  );
                var tmp = se.Solutions;
                result = new ThePoint[tmp.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = new ThePoint(le.GetX(tmp[i]) + Center.X, tmp[i] + Center.Y);

            }
            else
            {
                // pozioma
                SquareEquation se = new SquareEquation(
                    1 + MathEx.Sqr(le.A / le.B),
                    -2 * le.A * le.C / le.B,
                    MathEx.Sqr(le.C / le.B) - MathEx.Sqr(_radius)
                    );
                var tmp = se.Solutions;
                result = new ThePoint[tmp.Length];
                for (int i = 0; i < result.Length; i++)
                    result[i] = new ThePoint(tmp[i] + Center.X, le.GetY(tmp[i]) + Center.Y);

            }
            return result;
        }


        /// <summary>
        /// Realizuje operator !=
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są różne</returns>
        public static bool operator !=(CircleOnPlane left, CircleOnPlane right)
        {
            if (left != (object)null && right != (object)null) return false;
            if (left != (object)null || right != (object)null) return true;
            return left.Center == right.Center || left._radius == right._radius;
        }

        /// <summary>
        /// Realizuje operator ==
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są równe</returns>
        public static bool operator ==(CircleOnPlane left, CircleOnPlane right)
        {
            if (left == (object)null && right == (object)null) return true;
            if (left == (object)null || right == (object)null) return false;
            return left.Center == right.Center && left._radius == right._radius;
        }


        /// <summary>
        /// Wykonuje kopię obiektu
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new CircleOnPlane(_radius, Center);
        }

        /// <summary>
        /// Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="obj">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public override bool Equals(object obj)
        {
            if (obj is CircleOnPlane) return (CircleOnPlane)obj == this;
            return false;
        }

        /// <summary>
        /// Zwraca kod HASH obiektu
        /// </summary>
        /// <returns>kod HASH obiektu</returns>
        public override int GetHashCode()
        {
            return Center.GetHashCode() ^ _radius.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Circle {0}, radius {1}", Center, Radius);
        }
        

        /// <summary>
        /// Środek koła
        /// </summary>
        public ThePoint Center { get; protected set; }
        
    }

    
}
