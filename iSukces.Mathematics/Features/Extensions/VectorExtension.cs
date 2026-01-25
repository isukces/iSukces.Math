using System;

namespace iSukces.Mathematics;

public static class VectorExtension
{
    private const double MinusOne = -1d;

    /// <param name="vector">wektor źródłowy</param>
    extension(Vector vector)
    {
        /// <summary>
        ///     Zwraca wektor prostopadły
        /// </summary>
        /// <param name="leftHand">czy układ lewoskrętny (domyślnie tak)</param>
        /// <returns></returns>
        public Vector GetPrependicular(bool leftHand = true)
        {
            if (leftHand)
                return new Vector(-vector.Y, vector.X);
            return new Vector(vector.Y, -vector.X);
        }

        public Vector GetReversedIf(bool condition)
        {
            return condition ? -vector : vector;
        }

        [Obsolete("Use GetNormalized instead", true)]
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

        [Obsolete("Use GetNormalized instead")]
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
            vector = vector.GetNormalized();
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
