using System;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;

#else
using System.Windows;
using System.Windows.Media.Media3D;
#endif



namespace iSukces.Mathematics;

public static class RectExtensions
{
    /// <param name="r">prostokąt</param>
    extension(Rect r)
    {
        public Rect Round(int decimals)
        {
            return new Rect(
                r.TopLeft.Round(decimals),
                r.BottomRight.Round(decimals));
        }
    
        /// <summary>
        ///     Zwraca tablicę 4 narożników
        /// </summary>
        /// <returns></returns>
        public Point[] GetCorners()
        {
            var left   = r.Left;
            var right  = r.Right;
            var top    = r.Top;
            var bottom = r.Bottom;
            return
            [
                new Point(left, bottom),
                new Point(right, bottom),
                new Point(right, top),
                new Point(left, top)
            ];
        }
    }
}
