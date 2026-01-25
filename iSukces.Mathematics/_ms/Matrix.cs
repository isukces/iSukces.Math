// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;
using System.Diagnostics;

namespace iSukces.Mathematics;

/// <summary>
///     Matrix
/// </summary>
public struct Matrix
{
    /// <summary>
    ///     Creates a matrix of the form
    ///     / m11, m12, 0 \
    ///     | m21, m22, 0 |
    ///     \ offsetX, offsetY, 1 /
    /// </summary>
    public Matrix(double m11, double m12,
        double m21, double m22,
        double offsetX, double offsetY)
    {
        _m11     = m11;
        _m12     = m12;
        _m21     = m21;
        _m22     = m22;
        _offsetX = offsetX;
        _offsetY = offsetY;
        _padding = 0;

        // We will detect EXACT identity, scale, translation or
        // scale+translation and use special case algorithms.

        // Now classify our matrix.
        if (!(_m21 == 0 && _m12 == 0))
        {
            _type = MatrixTypes.TRANSFORM_IS_UNKNOWN;
            return;
        }

        _type = 0;
        if (!(_m11 == 1 && _m22 == 1))
        {
            _type = MatrixTypes.TRANSFORM_IS_SCALING;
        }

        if (!(_offsetX == 0 && _offsetY == 0))
        {
            _type |= MatrixTypes.TRANSFORM_IS_TRANSLATION;
        }

        if (0 == (_type & (MatrixTypes.TRANSFORM_IS_TRANSLATION | MatrixTypes.TRANSFORM_IS_SCALING)))
        {
            // We have an identity matrix.
            _type = MatrixTypes.TRANSFORM_IS_IDENTITY;
            _m11  = 0;
            _m22  = 0;
        }
    }

    /// <summary>
    ///     Creates a matrix of the form
    ///     / m11, m12, 0 \
    ///     | m21, m22, 0 |
    ///     \ offsetX, offsetY, 1 /
    /// </summary>
    private Matrix(double m11, double m12,
        double m21, double m22,
        double offsetX, double offsetY, MatrixTypes type)
    {
        _m11     = m11;
        _m12     = m12;
        _m21     = m21;
        _m22     = m22;
        _offsetX = offsetX;
        _offsetY = offsetY;
        _padding = 0;
        _type    = type;
    }

    /// <summary>
    ///     Sets the transformation to the identity.
    /// </summary>
    private static Matrix CreateIdentity()
    {
        /*var matrix = new Matrix();
        matrix.SetMatrix(1, 0,
            0, 1,
            0, 0,
            MatrixTypes.TRANSFORM_IS_IDENTITY);
        return matrix;*/
        return default;
    }

    /// <summary>
    ///     Creates a rotation transformation about the given point
    /// </summary>
    /// <param name='angle'>The angle to rotate specified in radians</param>
    private static Matrix CreateRotationRadians(double angle)
    {
        return CreateRotationRadians(angle, centerX: 0, centerY: 0);
    }

    /// <summary>
    ///     Creates a rotation transformation about the given point
    /// </summary>
    /// <param name='angle'>The angle to rotate specified in radians</param>
    /// <param name='centerX'>The centerX of rotation</param>
    /// <param name='centerY'>The centerY of rotation</param>
    private static Matrix CreateRotationRadians(double angle, double centerX, double centerY)
    {
        // var matrix = new Matrix();

        var sin = Math.Sin(angle);
        var cos = Math.Cos(angle);
        var dx  = centerX * (1.0 - cos) + centerY * sin;
        var dy  = centerY * (1.0 - cos) - centerX * sin;

        return new Matrix(cos, sin,
            -sin, cos,
            dx, dy);
        //MatrixTypes.TRANSFORM_IS_UNKNOWN);

        // return matrix;
    }

    /// <summary>
    ///     Creates a scaling transform around the given point
    /// </summary>
    /// <param name='scaleX'>The scale factor in the x dimension</param>
    /// <param name='scaleY'>The scale factor in the y dimension</param>
    /// <param name='centerX'>The centerX of scaling</param>
    /// <param name='centerY'>The centerY of scaling</param>
    private static Matrix CreateScaling(double scaleX, double scaleY, double centerX, double centerY)
    {
        //var matrix = new Matrix();

        return new(scaleX, 0,
            0, scaleY,
            centerX - scaleX * centerX, centerY - scaleY * centerY
            //, MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION
        );
    }

    /// <summary>
    ///     Creates a scaling transform around the origin
    /// </summary>
    /// <param name='scaleX'>The scale factor in the x dimension</param>
    /// <param name='scaleY'>The scale factor in the y dimension</param>
    private static Matrix CreateScaling(double scaleX, double scaleY)
    {
        if (scaleX == 1 && scaleY == 1)
            return default;
        return new Matrix(scaleX, 0,
            0, scaleY,
            0, 0,
            MatrixTypes.TRANSFORM_IS_SCALING);
    }

    /// <summary>
    ///     Creates a skew transform
    /// </summary>
    /// <param name='skewX'>The skew angle in the x dimension in degrees</param>
    /// <param name='skewY'>The skew angle in the y dimension in degrees</param>
    private static Matrix CreateSkewRadians(double skewX, double skewY)
    {
        return new(
            1.0, Math.Tan(skewY),
            Math.Tan(skewX), 1.0,
            0.0, 0.0
            //,MatrixTypes.TRANSFORM_IS_UNKNOWN
        );
    }

    /// <summary>
    ///     Sets the transformation to the given translation specified by the offset vector.
    /// </summary>
    /// <param name='offsetX'>The offset in X</param>
    /// <param name='offsetY'>The offset in Y</param>
    private static Matrix CreateTranslation(double offsetX, double offsetY)
    {
        if (offsetX == 0 && offsetY == 0)
            return default;
        return new Matrix(
            1, 0,
            0, 1,
            offsetX, offsetY,
            MatrixTypes.TRANSFORM_IS_TRANSLATION);
    }

    /// <summary>
    ///     Multiply
    /// </summary>
    public static Matrix Multiply(Matrix trans1, Matrix trans2)
    {
        MatrixUtil.MultiplyMatrix(ref trans1, ref trans2);
        trans1.Debug_CheckType();
        return trans1;
    }


    /// <summary>
    ///     Multiplies two transformations.
    /// </summary>
    public static Matrix operator *(Matrix trans1, Matrix trans2)
    {
        MatrixUtil.MultiplyMatrix(ref trans1, ref trans2);
        trans1.Debug_CheckType();
        return trans1;
    }

    /*/// <summary>
    ///     Append - "this" becomes this * matrix, the same as this *= matrix.
    /// </summary>
    /// <param name="matrix"> The Matrix to append to this Matrix </param>
    public void Append(Matrix matrix)
    {
        this *= matrix;
    }*/

    /// <summary>
    ///     Asserts that the matrix tag is one of the valid options and
    ///     that coefficients are correct.
    /// </summary>
    [Conditional("DEBUG")]
    private void Debug_CheckType()
    {
        switch (_type)
        {
            case MatrixTypes.TRANSFORM_IS_IDENTITY:
                return;
            case MatrixTypes.TRANSFORM_IS_UNKNOWN:
                return;
            case MatrixTypes.TRANSFORM_IS_SCALING:
                Debug.Assert(_m21 == 0);
                Debug.Assert(_m12 == 0);
                Debug.Assert(_offsetX == 0);
                Debug.Assert(_offsetY == 0);
                return;
            case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                Debug.Assert(_m21 == 0);
                Debug.Assert(_m12 == 0);
                Debug.Assert(_m11 == 1);
                Debug.Assert(_m22 == 1);
                return;
            case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                Debug.Assert(_m21 == 0);
                Debug.Assert(_m12 == 0);
                return;
            default:
                Debug.Assert(false);
                return;
        }
    }


    /// <summary>
    ///     Replaces matrix with the inverse of the transformation.  This will throw an InvalidOperationException
    ///     if !HasInverse
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     This will throw an InvalidOperationException if the matrix is non-invertable
    /// </exception>
    public Matrix GetInverted()
    {
        var determinant = Determinant;

        if (DoubleUtil.IsZero(determinant))
        {
            throw new InvalidOperationException("Matrix is not invertible");
        }

        // Inversion does not change the type of a matrix.
        switch (_type)
        {
            case MatrixTypes.TRANSFORM_IS_IDENTITY:
                return this;
            case MatrixTypes.TRANSFORM_IS_SCALING:
                return CreateScaling(1.0 / _m11, 1.0 / _m22);
            case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                return CreateTranslation(-_offsetX, -_offsetY);
            case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                var m11     = 1.0 / _m11;
                var m22     = 1.0 / _m22;
                var offsetX = -_offsetX * _m11;
                var offsetY = -_offsetY * _m22;
                return new Matrix(m11, 0, 0, m22, offsetX, offsetY);
            default:
                var invdet = 1.0 / determinant;
                return new Matrix(
                    _m22 * invdet,
                    -_m12 * invdet,
                    -_m21 * invdet,
                    _m11 * invdet,
                    (_m21 * _offsetY - _offsetX * _m22) * invdet,
                    (_offsetX * _m12 - _m11 * _offsetY) * invdet
                    //, MatrixTypes.TRANSFORM_IS_UNKNOWN
                );
        }
    }

    /*/// <summary>
    ///     Prepend - "this" becomes matrix * this, the same as this = matrix * this.
    /// </summary>
    /// <param name="matrix"> The Matrix to prepend to this Matrix </param>
    public void Prepend(Matrix matrix)
    {
        this = matrix * this;
    }*/

    /// <summary>
    ///     Rotates this matrix about the origin
    /// </summary>
    /// <param name='angle'>The angle to rotate specified in degrees</param>
    public Matrix GetRotated(double angle)
    {
        angle %= 360.0; // Doing the modulo before converting to radians reduces total error
        return this * CreateRotationRadians(angle * (Math.PI / 180.0));
    }

    /// <summary>
    ///     Rotates this matrix about the given point
    /// </summary>
    /// <param name='angle'>The angle to rotate specified in degrees</param>
    /// <param name='centerX'>The centerX of rotation</param>
    /// <param name='centerY'>The centerY of rotation</param>
    public Matrix GetRotatedAt(double angle, double centerX, double centerY)
    {
        angle %= 360.0; // Doing the modulo before converting to radians reduces total error
        return this * CreateRotationRadians(angle * (Math.PI / 180.0), centerX, centerY);
    }

    /// <summary>
    ///     Prepends a rotation about the given point to "this"
    /// </summary>
    /// <param name='angle'>The angle to rotate specified in degrees</param>
    /// <param name='centerX'>The centerX of rotation</param>
    /// <param name='centerY'>The centerY of rotation</param>
    public Matrix GetRotatedAtPrepend(double angle, double centerX, double centerY)
    {
        angle %= 360.0; // Doing the modulo before converting to radians reduces total error
        return CreateRotationRadians(angle * (Math.PI / 180.0), centerX, centerY) * this;
    }

    /// <summary>
    ///     Prepends a rotation about the origin to "this"
    /// </summary>
    /// <param name='angle'>The angle to rotate specified in degrees</param>
    public Matrix GetRotatedPrepend(double angle)
    {
        angle %= 360.0; // Doing the modulo before converting to radians reduces total error
        return CreateRotationRadians(angle * (Math.PI / 180.0)) * this;
    }

    /// <summary>
    ///     Scales this matrix around the origin
    /// </summary>
    /// <param name='scaleX'>The scale factor in the x dimension</param>
    /// <param name='scaleY'>The scale factor in the y dimension</param>
    public Matrix GetScaled(double scaleX, double scaleY)
    {
        return this * CreateScaling(scaleX, scaleY);
    }

    /// <summary>
    ///     Scales this matrix around the center provided
    /// </summary>
    /// <param name='scaleX'>The scale factor in the x dimension</param>
    /// <param name='scaleY'>The scale factor in the y dimension</param>
    /// <param name="centerX">The centerX about which to scale</param>
    /// <param name="centerY">The centerY about which to scale</param>
    public Matrix GetScaledAt(double scaleX, double scaleY, double centerX, double centerY)
    {
        return this * CreateScaling(scaleX, scaleY, centerX, centerY);
    }

    /// <summary>
    ///     Prepends a scale around the center provided to "this"
    /// </summary>
    /// <param name='scaleX'>The scale factor in the x dimension</param>
    /// <param name='scaleY'>The scale factor in the y dimension</param>
    /// <param name="centerX">The centerX about which to scale</param>
    /// <param name="centerY">The centerY about which to scale</param>
    public Matrix GetScaledAtPrepend(double scaleX, double scaleY, double centerX, double centerY)
    {
        return CreateScaling(scaleX, scaleY, centerX, centerY) * this;
    }

    /// <summary>
    ///     Prepends a scale around the origin to "this"
    /// </summary>
    /// <param name='scaleX'>The scale factor in the x dimension</param>
    /// <param name='scaleY'>The scale factor in the y dimension</param>
    public Matrix GetScaledPrepend(double scaleX, double scaleY)
    {
        return CreateScaling(scaleX, scaleY) * this;
    }

    /// <summary>
    ///     Skews this matrix
    /// </summary>
    /// <param name='skewX'>The skew angle in the x dimension in degrees</param>
    /// <param name='skewY'>The skew angle in the y dimension in degrees</param>
    public Matrix GetSkrewd(double skewX, double skewY)
    {
        skewX %= 360;
        skewY %= 360;
        return this * CreateSkewRadians(skewX * (Math.PI / 180.0),
            skewY * (Math.PI / 180.0));
    }

    /// <summary>
    ///     Prepends a skew to this matrix
    /// </summary>
    /// <param name='skewX'>The skew angle in the x dimension in degrees</param>
    /// <param name='skewY'>The skew angle in the y dimension in degrees</param>
    public Matrix GetSkrewdPrepend(double skewX, double skewY)
    {
        skewX %= 360;
        skewY %= 360;
        return CreateSkewRadians(skewX * (Math.PI / 180.0),
            skewY * (Math.PI / 180.0)) * this;
    }

    /// <summary>
    ///     Translates this matrix
    /// </summary>
    /// <param name='offsetX'>The offset in the x dimension</param>
    /// <param name='offsetY'>The offset in the y dimension</param>
    public Matrix GetTranslated(double offsetX, double offsetY)
    {
        //
        // / a b 0 \   / 1 0 0 \    / a      b       0 \
        // | c d 0 | * | 0 1 0 | = |  c      d       0 |
        // \ e f 1 /   \ x y 1 /    \ e+x    f+y     1 /
        //
        // (where e = _offsetX and f == _offsetY)
        //

        if (_type == MatrixTypes.TRANSFORM_IS_IDENTITY)
        {
            return CreateTranslation(offsetX, offsetY);
        }

        return new Matrix(_m11, _m12, _m21, _m22,
            _offsetX + offsetX,
            _offsetY + offsetY);
    }

    /// <summary>
    ///     Prepends a translation to this matrix
    /// </summary>
    /// <param name='offsetX'>The offset in the x dimension</param>
    /// <param name='offsetY'>The offset in the y dimension</param>
    public Matrix GetTranslatePrepend(double offsetX, double offsetY)
    {
        return CreateTranslation(offsetX, offsetY) * this;
    }

    /// <summary>
    ///     MultiplyPoint
    /// </summary>
    private void MultiplyPoint(ref double x, ref double y)
    {
        switch (_type)
        {
            case MatrixTypes.TRANSFORM_IS_IDENTITY:
                return;
            case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                x += _offsetX;
                y += _offsetY;
                return;
            case MatrixTypes.TRANSFORM_IS_SCALING:
                x *= _m11;
                y *= _m22;
                return;
            case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                x *= _m11;
                x += _offsetX;
                y *= _m22;
                y += _offsetY;
                break;
            default:
                var xadd = y * _m21 + _offsetX;
                var yadd = x * _m12 + _offsetY;
                x *= _m11;
                x += xadd;
                y *= _m22;
                y += yadd;
                break;
        }
    }

    /// <summary>
    ///     MultiplyVector
    /// </summary>
    private void MultiplyVector(ref double x, ref double y)
    {
        switch (_type)
        {
            case MatrixTypes.TRANSFORM_IS_IDENTITY:
            case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                return;
            case MatrixTypes.TRANSFORM_IS_SCALING:
            case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                x *= _m11;
                y *= _m22;
                break;
            default:
                var xadd = y * _m21;
                var yadd = x * _m12;
                x *= _m11;
                x += xadd;
                y *= _m22;
                y += yadd;
                break;
        }
    }

    /// <summary>
    ///     Transform - returns the result of transforming the point by this matrix
    /// </summary>
    /// <returns>
    ///     The transformed point
    /// </returns>
    /// <param name="point"> The Point to transform </param>
    public Point Transform(Point point)
    {
        var x = point.X;
        var y = point.Y;
        MultiplyPoint(ref x, ref y);
        return new Point(x, y);
    }

    /// <summary>
    ///     Transform - Transforms each point in the array by this matrix
    /// </summary>
    /// <param name="points"> The Point array to transform </param>
    public void Transform(Point[]? points)
    {
        if (points is null) return;
        for (var i = 0; i < points.Length; i++)
        {
            points[i] = Transform(points[i]);
        }
    }

    /// <summary>
    ///     Transform - returns the result of transforming the Vector by this matrix.
    /// </summary>
    /// <returns>
    ///     The transformed vector
    /// </returns>
    /// <param name="vector"> The Vector to transform </param>
    public Vector Transform(Vector vector)
    {
        var x = vector.X;
        var y = vector.Y;
        MultiplyVector(ref x, ref y);
        return new Vector(x, y);
    }

    /// <summary>
    ///     Transform - Transforms each Vector in the array by this matrix.
    /// </summary>
    /// <param name="vectors"> The Vector array to transform </param>
    public void Transform(Vector[]? vectors)
    {
        if (vectors is null) return;
        for (var i = 0; i < vectors.Length; i++)
        {
            vectors[i] = Transform(vectors[i]);
        }
    }

    public Matrix WithOffset(Point point)
    {
        return new Matrix(M11, M12, M21, M22, point.X, point.Y);
    }

    /// <summary>
    ///     Identity
    /// </summary>
    public static Matrix Identity => default;

    /// <summary>
    ///     Tests whether or not a given transform is an identity transform
    /// </summary>
    public bool IsIdentity => _type == MatrixTypes.TRANSFORM_IS_IDENTITY;

    /* ||
                              (_m11 == 1 && _m12 == 0 && _m21 == 0 && _m22 == 1 && _offsetX == 0 && _offsetY == 0);*/

    /// <summary>
    ///     The determinant of this matrix
    /// </summary>
    public double Determinant
    {
        get
        {
            switch (_type)
            {
                case MatrixTypes.TRANSFORM_IS_IDENTITY:
                case MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    return 1.0;
                case MatrixTypes.TRANSFORM_IS_SCALING:
                case MatrixTypes.TRANSFORM_IS_SCALING | MatrixTypes.TRANSFORM_IS_TRANSLATION:
                    return _m11 * _m22;
                default:
                    return _m11 * _m22 - _m12 * _m21;
            }
        }
    }

    /// <summary>
    ///     HasInverse Property - returns true if this matrix is invertable, false otherwise.
    /// </summary>
    public bool HasInverse => !DoubleUtil.IsZero(Determinant);

    /// <summary>
    ///     M11
    /// </summary>
    public double M11 => _type == MatrixTypes.TRANSFORM_IS_IDENTITY ? 1.0 : _m11;

    /// <summary>
    ///     M12
    /// </summary>
    public double M12 => _m12;

    /// <summary>
    ///     M22
    /// </summary>
    public double M21 => _m21;

    /// <summary>
    ///     M22
    /// </summary>
    public double M22 => _type == MatrixTypes.TRANSFORM_IS_IDENTITY ? 1.0 : _m22;

    /// <summary>
    ///     OffsetX
    /// </summary>
    public double OffsetX => _offsetX;

    /// <summary>
    ///     OffsetY
    /// </summary>
    public double OffsetY => _offsetY;

    // the transform is identity by default
    // Actually fill in the fields - some (private) code uses the fields directly for perf.
    // private static readonly Matrix s_identity = CreateIdentity();

    internal readonly double _m11;
    internal readonly double _m12;
    internal readonly double _m21;
    internal readonly double _m22;
    internal readonly double _offsetX;
    internal readonly double _offsetY;
    internal readonly MatrixTypes _type;

// This field is only used by unmanaged code which isn't detected by the compiler.
#pragma warning disable 0414
    // Matrix in blt'd to unmanaged code, so this is padding 
    // to align structure.
    //
    // Testing note: Validate that this blt will work on 64-bit
    //
    private int _padding;
#pragma warning restore 0414
}
