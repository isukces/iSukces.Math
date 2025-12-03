using System;
#if !WPFFEATURES
using TheVector = iSukces.Mathematics.Compatibility.Vector;
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics;

public static class VectorExtension
{
    private const double MinusOne = -1d;

    /// <param name="vector">wektor źródłowy</param>
    extension(TheVector vector)
    {
        /// <summary>
        ///     Zwraca wektor prostopadły
        /// </summary>
        /// <param name="leftHand">czy układ lewoskrętny (domyślnie tak)</param>
        /// <returns></returns>
        public TheVector GetPrependicular(bool leftHand = true)
        {
            if (leftHand)
                return new TheVector(-vector.Y, vector.X);
            return new TheVector(vector.Y, -vector.X);
        }

        public TheVector GetReversedIf(bool condition)
        {
            return condition ? -vector : vector;
        }

        public Vector NormalizeFast()
        {
            var x = vector.X;
            var y = vector.Y;

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
                            x       =  MinusOne;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            x       /= absY;
                            y       =  MinusOne;
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
                            y       =  MinusOne;
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
                            x       =  MinusOne;
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

        public Vector NormalizeFast(out double length)
        {
            var x = vector.X;
            var y = vector.Y;

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
                            x       =  MinusOne;
                            vLength =  Math.Sqrt(1 + y * y);
                        }
                        else
                        {
                            length  =  absY;
                            x       /= absY;
                            y       =  MinusOne;
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
                            y       =  MinusOne;
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
                            x       =  MinusOne;
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
    }

    extension(Vector3D vector)
    {
        public Vector3D GetReversedIf(bool condition)
        {
            return condition ? -vector : vector;
        }

        public Vector3D ToNormalized()
        {
            vector.Normalize();
            switch (vector.Y)
            {
                case 1d:
                    return new Vector3D(0, 1, 0);
                case -1d:
                    return new Vector3D(0, -1, 0);
            }

            switch (vector.X)
            {
                case 1d:
                    return new Vector3D(1, 0, 0);
                case -1d:
                    return new Vector3D(-1, 0, 0);
            }

            switch (vector.Z)
            {
                case 1d:
                    return new Vector3D(0, 0, 1);
                case -1d:
                    return new Vector3D(0, 0, -1);
                default:
                    return vector;
            }
        }
    }
}
