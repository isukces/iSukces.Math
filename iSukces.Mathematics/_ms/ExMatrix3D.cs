// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;
using System.Runtime.CompilerServices;

namespace iSukces.Mathematics;

public readonly record struct ExMatrix3D : IFormattable
{
    /// <summary>
    ///     Główny konstruktor inicjalizujący wszystkie pola.
    /// </summary>
    public ExMatrix3D(
        double m11, double m12, double m13, double m14,
        double m21, double m22, double m23, double m24,
        double m31, double m32, double m33, double m34,
        double offsetX, double offsetY, double offsetZ, double m44)
    {
        _m11     = m11;
        _m12     = m12;
        _m13     = m13;
        _m14     = m14;
        _m21     = m21;
        _m22     = m22;
        _m23     = m23;
        _m24     = m24;
        _m31     = m31;
        _m32     = m32;
        _m33     = m33;
        _m34     = m34;
        _offsetX = offsetX;
        _offsetY = offsetY;
        _offsetZ = offsetZ;
        _m44     = m44;

        var isAffine = m14 == 0.0 && m24 == 0.0 && m34 == 0.0 && m44 == 1.0;

        if (isAffine)
        {
            // Tutaj wiemy, że m14, m24, m34 = 0 i m44 = 1.
            // Sprawdzamy tylko część 3x3 i translację.

            var isXyzIdentity = m11 == 1.0 && m22 == 1.0 && m33 == 1.0 &&
                                m12 == 0.0 && m13 == 0.0 && m21 == 0.0 &&
                                m23 == 0.0 && m31 == 0.0 && m32 == 0.0;

            if (isXyzIdentity)
            {
                if (offsetX == 0.0 && offsetY == 0.0 && offsetZ == 0.0)
                {
                    _type = MatrixTypes3.IsIdentity;
                    // Twoje bezpieczne zerowanie pól diagonalnych dla record struct
                    _m11 = _m22 = _m33 = _m44 = 0.0;
                }
                else
                {
                    _type = MatrixTypes3.IsAfiniteTranslation;
                }
            }
            else
            {
                // Można tu jeszcze sprawdzić TRANSFORM_IS_SCALING (jeśli off-diagonal w 3x3 są 0)
                _type = MatrixTypes3.IsAfinite;
            }
        }
        else
        {
            // Macierz z perspektywą lub m44 != 1
            _type = MatrixTypes3.Projection;
        }
    }


    /// <summary>
    ///     Matrix multiplication.
    /// </summary>
    /// <param name="matrix1">Matrix to multiply.</param>
    /// <param name="matrix2">Matrix by which the first matrix is multiplied.</param>
    /// <returns>Result of multiplication.</returns>
    public static ExMatrix3D operator *(ExMatrix3D matrix1, ExMatrix3D matrix2)
    {
        // Check if multiplying by identity.
        if (matrix1.IsIdentity)
            return matrix2;
        if (matrix2.IsIdentity)
            return matrix1;

        // Regular 4x4 matrix multiplication.
        var result = new ExMatrix3D(
            matrix1._m11 * matrix2._m11 + matrix1._m12 * matrix2._m21 +
            matrix1._m13 * matrix2._m31 + matrix1._m14 * matrix2._offsetX,
            matrix1._m11 * matrix2._m12 + matrix1._m12 * matrix2._m22 +
            matrix1._m13 * matrix2._m32 + matrix1._m14 * matrix2._offsetY,
            matrix1._m11 * matrix2._m13 + matrix1._m12 * matrix2._m23 +
            matrix1._m13 * matrix2._m33 + matrix1._m14 * matrix2._offsetZ,
            matrix1._m11 * matrix2._m14 + matrix1._m12 * matrix2._m24 +
            matrix1._m13 * matrix2._m34 + matrix1._m14 * matrix2._m44,
            matrix1._m21 * matrix2._m11 + matrix1._m22 * matrix2._m21 +
            matrix1._m23 * matrix2._m31 + matrix1._m24 * matrix2._offsetX,
            matrix1._m21 * matrix2._m12 + matrix1._m22 * matrix2._m22 +
            matrix1._m23 * matrix2._m32 + matrix1._m24 * matrix2._offsetY,
            matrix1._m21 * matrix2._m13 + matrix1._m22 * matrix2._m23 +
            matrix1._m23 * matrix2._m33 + matrix1._m24 * matrix2._offsetZ,
            matrix1._m21 * matrix2._m14 + matrix1._m22 * matrix2._m24 +
            matrix1._m23 * matrix2._m34 + matrix1._m24 * matrix2._m44,
            matrix1._m31 * matrix2._m11 + matrix1._m32 * matrix2._m21 +
            matrix1._m33 * matrix2._m31 + matrix1._m34 * matrix2._offsetX,
            matrix1._m31 * matrix2._m12 + matrix1._m32 * matrix2._m22 +
            matrix1._m33 * matrix2._m32 + matrix1._m34 * matrix2._offsetY,
            matrix1._m31 * matrix2._m13 + matrix1._m32 * matrix2._m23 +
            matrix1._m33 * matrix2._m33 + matrix1._m34 * matrix2._offsetZ,
            matrix1._m31 * matrix2._m14 + matrix1._m32 * matrix2._m24 +
            matrix1._m33 * matrix2._m34 + matrix1._m34 * matrix2._m44,
            matrix1._offsetX * matrix2._m11 + matrix1._offsetY * matrix2._m21 +
            matrix1._offsetZ * matrix2._m31 + matrix1._m44 * matrix2._offsetX,
            matrix1._offsetX * matrix2._m12 + matrix1._offsetY * matrix2._m22 +
            matrix1._offsetZ * matrix2._m32 + matrix1._m44 * matrix2._offsetY,
            matrix1._offsetX * matrix2._m13 + matrix1._offsetY * matrix2._m23 +
            matrix1._offsetZ * matrix2._m33 + matrix1._m44 * matrix2._offsetZ,
            matrix1._offsetX * matrix2._m14 + matrix1._offsetY * matrix2._m24 +
            matrix1._offsetZ * matrix2._m34 + matrix1._m44 * matrix2._m44);

        return result;
    }

    /// <summary>
    ///     Creates a string representation of this object based on the format string
    ///     and IFormatProvider passed in.
    ///     If the provider is null, the CurrentCulture is used.
    ///     See the documentation for IFormattable for more information.
    /// </summary>
    /// <returns>
    ///     A string representation of this object.
    /// </returns>
    internal string ConvertToString(string? format, IFormatProvider? provider)
    {
        if (IsIdentity)
        {
            return "Identity";
        }

        // Helper to get the numeric list separator for a given culture.
        char separator = MsCompatibility.GetNumericListSeparator(provider);
        return string.Format(provider,
            "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}{0}{5:" + format + "}{0}{6:" + format + "}{0}{7:" + format +
            "}{0}{8:" + format + "}{0}{9:" + format + "}{0}{10:" + format + "}{0}{11:" + format + "}{0}{12:" + format + "}{0}{13:" + format + "}{0}{14:" +
            format + "}{0}{15:" + format + "}{0}{16:" + format + "}",
            separator,
            _m11,
            _m12,
            _m13,
            _m14,
            _m21,
            _m22,
            _m23,
            _m24,
            _m31,
            _m32,
            _m33,
            _m34,
            _offsetX,
            _offsetY,
            _offsetZ,
            _m44);
    }


    /// <summary>
    ///     Silnie typowane porównanie z optymalizacją dla Identity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(ExMatrix3D other)
    {
        if (_type != other._type)
            return false;
        if (_type == MatrixTypes3.IsIdentity)
            return true;

        // ReSharper disable CompareOfFloatsByEqualityOperator

        var translationEquals =
            _offsetX == other._offsetX
            && _offsetY == other._offsetY
            && _offsetZ == other._offsetZ;
        if (!translationEquals)
            return false;
        if (_type == MatrixTypes3.IsAfiniteTranslation)
            return true;

        var rotationEquals =
            _m11 == other._m11 && _m12 == other._m12 && _m13 == other._m13 &&
            _m21 == other._m21 && _m22 == other._m22 && _m23 == other._m23 &&
            _m31 == other._m31 && _m32 == other._m32 && _m33 == other._m33;
        if (!rotationEquals)
            return false;
        if (_type == MatrixTypes3.IsAfinite)
            return true;
        return _m14 == other._m14 &&
               _m24 == other._m24 &&
               _m34 == other._m34 &&
               _m44 == other._m44;
        // ReSharper restore CompareOfFloatsByEqualityOperator
    }

    /// <summary>
    ///     GetHashCode zoptymalizowany pod kątem flagi Identity.
    /// </summary>
    public override int GetHashCode()
    {
        if (_type == MatrixTypes3.IsIdentity)
            return 0;

        var hash = new HashCode();
        hash.Add(_type);
        if (_type != MatrixTypes3.IsAfiniteTranslation)
        {
            hash.Add(_m11);
            hash.Add(_m22);
            hash.Add(_m33);
        }

        hash.Add(_offsetX);
        hash.Add(_offsetY);
        hash.Add(_offsetZ);
        return hash.ToHashCode();
    }

    //  Multiplies the given Point3D by this matrix, projecting the
    //  result back into the W=1 plane.
    //
    //  The point is modified in place for performance.
    //
    internal void MultiplyPoint(ref Point3D point)
    {
        if (_type == MatrixTypes3.IsIdentity)
            return;

        var x = point.X;
        var y = point.Y;
        var z = point.Z;

        if (_type == MatrixTypes3.IsAfiniteTranslation)
        {
            x     += _offsetX;
            y     += _offsetY;
            z     += _offsetZ;
            point =  new Point3D(x, y, z);
            return;
        }

        var outX = x * _m11 + y * _m21 + z * _m31 + _offsetX;
        var outY = x * _m12 + y * _m22 + z * _m32 + _offsetY;
        var outZ = x * _m13 + y * _m23 + z * _m33 + _offsetZ;

        if (_type == MatrixTypes3.Projection)
        {
            var w = x * _m14 + y * _m24 + z * _m34 + _m44;
            outX /= w;
            outY /= w;
            outZ /= w;
        }

        point = new Point3D(outX, outY, outZ);
    }


    //  Multiplies the given Vector3D by this matrix.
    //
    //  The vector is modified in place for performance.
    //
    internal void MultiplyVector(ref Vector3D vector)
    {
        if (_type == MatrixTypes3.IsIdentity)
            return;

        var x = vector.X;
        var y = vector.Y;
        var z = vector.Z;

        // Do not apply _offset to vectors.
        var vectorX = x * _m11 + y * _m21 + z * _m31;
        var vectorY = x * _m12 + y * _m22 + z * _m32;
        var vectorZ = x * _m13 + y * _m23 + z * _m33;
        vector = new Vector3D(vectorX, vectorY, vectorZ);
    }


    /// <summary>
    ///     Creates a string representation of this object based on the current culture.
    /// </summary>
    /// <returns>
    ///     A string representation of this object.
    /// </returns>
    public override string ToString()
    {
        // Delegate to the internal method which implements all ToString calls.
        return ConvertToString(null /* format string */, null /* format provider */);
    }

    /// <summary>
    ///     Creates a string representation of this object based on the IFormatProvider
    ///     passed in.  If the provider is null, the CurrentCulture is used.
    /// </summary>
    /// <returns>
    ///     A string representation of this object.
    /// </returns>
    public string ToString(IFormatProvider provider)
    {
        // Delegate to the internal method which implements all ToString calls.
        return ConvertToString(null /* format string */, provider);
    }

    /// <summary>
    ///     Creates a string representation of this object based on the format string
    ///     and IFormatProvider passed in.
    ///     If the provider is null, the CurrentCulture is used.
    ///     See the documentation for IFormattable for more information.
    /// </summary>
    /// <returns>
    ///     A string representation of this object.
    /// </returns>
    string IFormattable.ToString(string? format, IFormatProvider? provider)
    {
        // Delegate to the internal method which implements all ToString calls.
        return ConvertToString(format, provider);
    }


    /// <summary>
    ///     Transforms the given Point3D by this matrix, projecting the
    ///     result back into the W=1 plane.
    /// </summary>
    /// <param name="point">Point to transform.</param>
    /// <returns>Transformed point.</returns>
    public Point3D Transform(Point3D point)
    {
        MultiplyPoint(ref point);
        return point;
    }

    /// <summary>
    ///     Transforms the given vector by the current matrix.
    /// </summary>
    /// <param name="vector">Vector to transform.</param>
    /// <returns>Transformed vector.</returns>
    public Vector3D Transform(Vector3D vector)
    {
        MultiplyVector(ref vector);
        return vector;
    }

    public double M11 => _type == MatrixTypes3.IsIdentity ? 1.0 : _m11;
    public double M22 => _type == MatrixTypes3.IsIdentity ? 1.0 : _m22;
    public double M33 => _type == MatrixTypes3.IsIdentity ? 1.0 : _m33;
    public double M44 => _type == MatrixTypes3.IsIdentity ? 1.0 : _m44;


    public double M12 => _m12;
    public double M13 => _m13;
    public double M14 => _m14;

    public double M21 => _m21;
    public double M23 => _m23;
    public double M24 => _m24;

    public double M31 => _m31;
    public double M32 => _m32;
    public double M34 => _m34;

    public double OffsetX    => _offsetX; // Translation X
    public double OffsetY    => _offsetY; // Translation Y
    public double OffsetZ    => _offsetZ; // Translation Z
    public bool   IsIdentity => _type == MatrixTypes3.IsIdentity;

    private readonly double _m11, _m12, _m13, _m14;
    private readonly double _m21, _m22, _m23, _m24;
    private readonly double _m31, _m32, _m33, _m34;
    private readonly double _offsetX, _offsetY, _offsetZ, _m44;
    private readonly MatrixTypes3 _type;
}
