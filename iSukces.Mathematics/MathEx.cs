#if COREFX
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using ThePoint=System.Windows.Point;
using TheVector=System.Windows.Vector;
#endif
using System;
using System.Runtime.CompilerServices;


namespace iSukces.Mathematics
{
    public static class MathEx
    {
        public const double DoublePI = (double)DoublePI_m;
        public const double DEGTORAD = (double)DEGTORAD_m;
        public const double RADTODEG = (double)RADTODEG_m;
        
        
        
        
        public const decimal PI_m = 3.1415926535897932384626433832795028841971693993751058209749445923078164062862089m;
        public const decimal DoublePI_m = 6.2831853071795864769252867665590057683943387987502116419498891846156328125724179m;
        public const decimal PI_2_m = 1.5707963267948966192313216916397514420985846996875529104874722961539082031431044m;

        /// <summary>
        /// pi/180
        /// </summary>
        public const decimal DEGTORAD_m = 0.017453292519943295769236907684886127134428718885417254560971914401710091146034494m;
        /// <summary>
        /// 180/pi
        /// </summary>
        public const decimal RADTODEG_m = 57.295779513082320876798154814105170332405472466564321549160243861202847148321552m;


        public const decimal SQRT2_m = 1.4142135623730950488016887242096980785696718753769480731766797379907324784621070m;
        public const decimal SQRT2_2_m = 0.70710678118654752440084436210484903928483593768847403658833986899536623923105351m;
        public const decimal SQRT3_m = 1.7320508075688772935274463415058723669428052538103806280558069794519330169088000m;

        public const decimal LN_10_m = 2.3025850929940456840179914546843642076011014886287729760333279009675726096773524m;
        public const decimal LN_2_m = 0.69314718055994530941723212145817656807550013436025525412068000949339362196969471m;

        public static double TriangleArea(double a, double b, double c)
        {
            // wzór Herona http://pl.wikipedia.org/wiki/Wz%C3%B3r_Herona
            double p = (a + b + c) / 2.0;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }

        /// <summary>
        /// Wylicza cosinus na podstawie tan
        /// </summary>
        /// <param name="tan">tangens kąta</param>
        /// <returns>cos kąta</returns>
        public static double TanToCos(double tan)
        {
            return 1 / Math.Sqrt(1 + tan * tan);
        }
        /// <summary>
        /// Wylicza sinus na podstawie tan
        /// </summary>
        /// <param name="tan">tangens kąta</param>
        /// <returns>sin kąta</returns>
        public static double TanToSin(double tan)
        {
            return tan / Math.Sqrt(1 + tan * tan);
        }


        /// <summary>
        /// Oblicza tangens kąta w stopniach
        /// </summary>
        /// <param name="angleDeg"></param>
        /// <returns></returns>
        public static double TanDeg(double angleDeg)
        {
            return Math.Tan(angleDeg * DEGTORAD);
        }


        public static double Avg(double a, double b)
        {
            return (a + b) * 0.5;
        }


        public static double Atan2Deg(ThePoint p)
        {
            return Math.Atan2(p.Y, p.X) * RADTODEG;
        }
#if !SILVERLIGHT
        public static double Atan2Deg(TheVector p)
        {
            return Math.Atan2(p.Y, p.X) * RADTODEG;
        }
#endif

        public static double Atan2Deg(ThePoint p1, ThePoint p2)
        {
            return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * RADTODEG;
        }

        public static double Atan2Deg(double y, double x)
        {
            return Math.Atan2(y, x) * RADTODEG;
        }

        public static double AtanDeg(double t)
        {
            return Math.Atan(t) * RADTODEG;
        }

#if !SILVERLIGHT
        [Obsolete("Użyj GetCosSinV bez żadnej modyfikacji")]
        public static TheVector GetSinCos(double angleDeg)
        {
            double s, c;
            GetSinCos(angleDeg, out s, out c);
            return new TheVector(c, s);
        }
        public static TheVector GetSinCosV(double angleDeg)
        {
            double s, c;
            GetSinCos(angleDeg, out s, out c);
            return new TheVector(s, c);
        }
        public static TheVector GetCosSinV(double angleDeg)
        {
            double s, c;
            GetSinCos(angleDeg, out s, out c);
            return new TheVector(c, s);
        }
        public static TheVector GetCosSinV(double angleDeg, double length)
        {
            double s, c;
            GetSinCos(angleDeg, out s, out c);
            return new TheVector(c * length, s * length);
        }
#endif
        public static void GetSinCos(double angleDeg, out double sin, out double cos)
        {
            angleDeg = angleDeg * DEGTORAD;
            sin = Math.Sin(angleDeg);
            cos = Math.Cos(angleDeg);
            if (sin == -1 || sin == +1)
                cos = 0;
            if (cos == -1 || cos == +1)
                sin = 0;
        }
        public static double CosDeg(double angleDeg)
        {
            if (angleDeg == 90) return 0.0;
            return Math.Cos(angleDeg * DEGTORAD);
        }

        public static double SinDeg(double angleDeg)
        {
            if (angleDeg == 0) return 0.0;
            return Math.Sin(angleDeg * DEGTORAD);
        }


        /// <summary>
        /// Dzieli l przez m i wynik zaokrągla w górę
        /// </summary>
        /// <param name="l">licznik</param>
        /// <param name="m">mianownik</param>
        /// <returns></returns>
        public static int DivRoundUp(int l, int m)
        {
            return (l + m - 1) / m;
        }
        public static int Ceil(this double x)
        {
            return (int)Math.Ceiling(x);
        }

        public static double SuareEquationDelta(double a, double b, double c)
        {
            return b * b - 4 * a * c;
        }
        /// <summary>
        /// Oblicza długość przyprostokątnej
        /// </summary>
        /// <param name="c"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double PitagorasA(double c, double b)
        {
            return Math.Sqrt(c * c - b * b);
        }
        /// <summary>
        /// Oblicza długość przeciwprostokątnej 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double PitagorasC(double a, double b)
        {
            return Math.Sqrt(a * a + b * b);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double PitagorasCSquared(double a, double b)
        {
            return a * a + b * b;
        }
        public static double Sqr(double x)
        {
            return x * x;
        }



        public static ThePoint Rotate(ThePoint p, double s, double c)
        {
            return Rotate(p.X, p.Y, s, c);
        }
        public static ThePoint Rotate(double pX, double pY, double s, double c)
        {
            return new ThePoint(pX * c - pY * s, pY * c + pX * s);
        }

        public static ThePoint RotateRev(ThePoint p, double s, double c)
        {
            return RotateRev(p.X, p.Y, s, c);
        }



        public static ThePoint RotateRev(double pX, double pY, double s, double c)
        {
            return new ThePoint(pX * c + pY * s, pY * c - pX * s);
        }

        public static double MaxAbs(double a, double b)
        {
            return Math.Abs(a) > Math.Abs(b) ? a : b;
        }

        public static double MinAbs(double a, double b)
        {
            return Math.Abs(a) < Math.Abs(b) ? a : b;
        }       

        
        /// <summary>
        /// Zamienia na zakres ((0, 360)
        /// </summary>
        /// <param name="angleDeg">kąt w stopniach</param>
        /// <returns></returns>
        public static double NormalizeAngleDeg(double angleDeg)
        {
            var minus = (int)Math.Floor(angleDeg / 360);
            if (minus != 0)
                angleDeg -= minus * 360;
            return angleDeg;
        }
    }
}
