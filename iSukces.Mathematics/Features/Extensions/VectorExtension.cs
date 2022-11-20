using System;
#if !WPFFEATURES
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;
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
        ///     Zwraca wektor prostopadły
        /// </summary>
        /// <param name="vector">wektor źródłowy</param>
        /// <param name="leftHand">czy układ lewoskrętny (domyślnie tak)</param>
        /// <returns></returns>
        public static TheVector GetPrependicular(this TheVector vector, bool leftHand = true)
        {
            if (leftHand)
                return new TheVector(-vector.Y, vector.X);
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


        public static Vector NormalizeFast(this Vector src)
        {
            var x = src.X;
            var y = src.Y;

            {
                var    xMinus = x < 0;
                var    yMinus = y < 0;
                double vLength;

                if (yMinus)
                {
                    var absY = -y;

                    if (xMinus)
                    {
                        var absX = -x;

                        if (absX > absY)
                        {
                            y       /= absX;
                            x       =  minusOne;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            x       /= absY;
                            y       =  minusOne;
                            vLength =  Math.Sqrt(x * x + 1);
                        }
                    }
                    else
                    {
                        var absX = x;
                        if (absX > absY)
                        {
                            y       /= absX;
                            x       =  1;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            x       /= absY;
                            y       =  minusOne;
                            vLength =  Math.Sqrt(x * x + 1);
                        }
                    }
                }
                else
                {
                    var absY = y;

                    if (xMinus)
                    {
                        var absX = -x;

                        if (absX > absY)
                        {
                            y       /= absX;
                            x       =  minusOne;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            x       /= absY;
                            y       =  1;
                            vLength =  Math.Sqrt(x * x + 1);
                        }
                    }
                    else
                    {
                        var absX = x;

                        if (absX > absY)
                        {
                            y /= absX;
                            x =  1;
                            var a = y * y;
                            a++;
                            vLength = Math.Sqrt(a);
                        }
                        else
                        {
                            x /= absY;
                            y =  1;
                            var a = x * x;
                            a++;
                            vLength = Math.Sqrt(a);
                        }
                    }
                }

                x /= vLength;
                y /= vLength;
            }
            return new Vector(x, y);
        }

        public static Vector NormalizeFast(this Vector src, out double length)
        {
            var x = src.X;
            var y = src.Y;

            {
                var    xMinus = x < 0;
                var    yMinus = y < 0;
                double vLength;

                if (yMinus)
                {
                    var absY = -y;

                    if (xMinus)
                    {
                        var absX = -x;

                        if (absX > absY)
                        {
                            length  =  absX;
                            y       /= absX;
                            x       =  minusOne;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            length  =  absY;
                            x       /= absY;
                            y       =  minusOne;
                            vLength =  Math.Sqrt(x * x + 1);
                        }
                    }
                    else
                    {
                        var absX = x;
                        if (absX > absY)
                        {
                            length  =  absX;
                            y       /= absX;
                            x       =  1;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            length  =  absY;
                            x       /= absY;
                            y       =  minusOne;
                            vLength =  Math.Sqrt(x * x + 1);
                        }
                    }
                }
                else
                {
                    var absY = y;

                    if (xMinus)
                    {
                        var absX = -x;

                        if (absX > absY)
                        {
                            length  =  absX;
                            y       /= absX;
                            x       =  minusOne;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            length  =  absY;
                            x       /= absY;
                            y       =  1;
                            vLength =  Math.Sqrt(x * x + 1);
                        }
                    }
                    else
                    {
                        var absX = x;

                        if (absX > absY)
                        {
                            length =  absX;
                            y      /= absX;
                            x      =  1;
                            var a = y * y;
                            a++;
                            vLength = Math.Sqrt(a);
                        }
                        else
                        {
                            length =  absY;
                            x      /= absY;
                            y      =  1;
                            var a = x * x;
                            a++;
                            vLength = Math.Sqrt(a);
                        }
                    }
                }

                length *= vLength;
                x      /= vLength;
                y      /= vLength;
            }
            return new Vector(x, y);
        }

        const double minusOne = -1d;
    }
}
