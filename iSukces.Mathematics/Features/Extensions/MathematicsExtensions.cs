using System;
#if COREFX
using iSukces.Mathematics.Compatibility;
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif



namespace iSukces.Mathematics
{
    public static class MathematicsExtensions
    {
        /// <summary>
        /// Zaokrągla w dół do pełnej wielokrotności
        /// </summary>
        /// <param name="x">liczba zaokrąglana</param>
        /// <param name="round">zaokrąglenie</param>
        /// <returns></returns>
        public static int RoundDown(this double x, int round)
        {
            return round * (int)Math.Floor(x / round);
        }


        public static int DivUp(this int a, int b)
        {
            return (a + b - 1) / b;
        }

        public static double GetArea(this ThePoint[] p)
        {
            if (p == null || p.Length < 3) return 0;
            if (p.Length == 3)
            {
                var v1 = p[1] - p[0];
                var v2 = p[2] - p[1];
                return 0.5 * TheVector.CrossProduct(v1, v2);
            }
            var sum = 0.0;
            for (var i = 0; i < p.Length; i++)
            {
                var vector1 = i == 0 ? p[p.Length - 1] : p[i - 1];
                var vector2 = p[i];
                sum += vector1.X * vector2.Y - vector1.Y * vector2.X;
            }
            return 0.5 * sum;
        }
    }
}
