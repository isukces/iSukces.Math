
using System;
#if COREFX
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif


namespace iSukces.Mathematics
{
    public static class Extensions2D
    {
        public static Point Round(this Point src, int decimals)
        {
            return new Point(
                Math.Round(src.X, decimals),
                Math.Round(src.Y, decimals));
        }

        public static Rect Round(this Rect r, int decimals)
        {
            return new Rect(
                r.TopLeft.Round(decimals),
                r.BottomRight.Round(decimals));
        }

        /// <summary>
        ///     Zwraca tablicę 4 narożników
        /// </summary>
        /// <param name="rectangle">prostokąt</param>
        /// <returns></returns>
        public static Point[] GetCorners(this Rect rectangle)
        {
            var left   = rectangle.Left;
            var right  = rectangle.Right;
            var top    = rectangle.Top;
            var bottom = rectangle.Bottom;
            return new[]
            {
                new Point(left, bottom),
                new Point(right, bottom),
                new Point(right, top),
                new Point(left, top)
            };
        }
    }
}
