using System;
#if !WPFFEATURES
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
    public struct SinusCosinus
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public SinusCosinus(double sin, double cos)
        {
            Sin = sin;
            Cos = cos;
        }


        public static SinusCosinus FromAngleDeg(double angle)
        {
            angle *= MathEx.DEGTORAD;
            return new SinusCosinus(Math.Sin(angle), Math.Cos(angle));
        }

        public static SinusCosinus FromAngleRad(double angle)
        {
            return new SinusCosinus(Math.Sin(angle), Math.Cos(angle));
        }

        /// <summary>
        ///     Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> containing a fully qualified type name.
        /// </returns>
        public override string ToString() { return string.Format("cos={0} sin={1}", Cos, Sin); }


        public TheVector ToVectorCosSin(double mx, double my) { return new TheVector(Cos * mx, Sin * my); }

        public TheVector ToVectorSinCos(double mx, double my) { return new TheVector(Sin * mx, Cos * my); }

        /// <summary>
        ///     Sinus
        /// </summary>
        public double Sin { get; }

        /// <summary>
        ///     Cosinus
        /// </summary>
        public double Cos { get; }

        /// <summary>
        ///     Tangent
        /// </summary>
        public double Tan => Sin / Cos;
    }
}
