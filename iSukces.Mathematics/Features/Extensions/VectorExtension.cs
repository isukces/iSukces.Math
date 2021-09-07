using System;
#if COREFX
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics
{
    public static class VectorExtension
    {
        /// <summary>
        /// Zwraca wektor prostopadły
        /// </summary>
        /// <param name="vector">wektor źródłowy</param>
        /// <param name="leftHand">czy układ lewoskrętny (domyślnie tak)</param>
        /// <returns></returns>
        public static TheVector GetPrependicular(this TheVector vector, bool leftHand = true)
        {
            if (leftHand)
                return new TheVector(-vector.Y, vector.X);
            else
                return new TheVector(vector.Y, -vector.X);
        }

        public static TheVector GetReversedIf(this TheVector vector, bool condition)
        {
            return condition ? -vector : vector;
        }

        public static Vector3D GetReversedIf(this Vector3D vector, bool condition)
        {
            return condition ? -vector : vector;
        }
    }
}
