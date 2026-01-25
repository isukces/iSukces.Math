// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;
using System.Runtime.CompilerServices;

namespace iSukces.Mathematics;

public readonly partial record struct Matrix3D : IFormattable
{
    /// <summary>
    ///     Główny konstruktor inicjalizujący wszystkie pola.
    /// </summary>
    public Matrix3D(
        double m11, double m12, double m13,
        double m21, double m22, double m23,
        double m31, double m32, double m33,
        double offsetX, double offsetY, double offsetZ)
    {
        _m11     = m11;
        _m12     = m12;
        _m13     = m13;
        _m21     = m21;
        _m22     = m22;
        _m23     = m23;
        _m31     = m31;
        _m32     = m32;
        _m33     = m33;
        _offsetX = offsetX;
        _offsetY = offsetY;
        _offsetZ = offsetZ;

        // ReSharper disable CompareOfFloatsByEqualityOperator
        var isXyzIdentity = m11 == 1.0 && m22 == 1.0 && m33 == 1.0 &&
                            m12 == 0.0 && m13 == 0.0 && m21 == 0.0 &&
                            m23 == 0.0 && m31 == 0.0 && m32 == 0.0;
        // ReSharper restore CompareOfFloatsByEqualityOperator

        if (isXyzIdentity)
        {
            if (offsetX == 0.0 && offsetY == 0.0 && offsetZ == 0.0)
            {
                _type = AffinityMatrixTypes3.IsIdentity;
                // Twoje bezpieczne zerowanie pól diagonalnych dla record struct
                _m11 = _m22 = _m33 = 0.0;
            }
            else
            {
                _type = AffinityMatrixTypes3.IsAfiniteTranslation;
            }
        }
        else
        {
            // Można tu jeszcze sprawdzić TRANSFORM_IS_SCALING (jeśli off-diagonal w 3x3 są 0)
            _type = AffinityMatrixTypes3.Other;
        }
    }


    /// <summary>
    ///     Matrix multiplication.
    /// </summary>
    /// <param name="matrix1">Matrix to multiply.</param>
    /// <param name="matrix2">Matrix by which the first matrix is multiplied.</param>
    /// <returns>Result of multiplication.</returns>
    public static Matrix3D operator *(Matrix3D matrix1, Matrix3D matrix2)
    {
        // Check if multiplying by identity.
        if (matrix1.IsIdentity)
            return matrix2;
        if (matrix2.IsIdentity)
            return matrix1;

        Matrix3D result = new Matrix3D(
            matrix1._m11 * matrix2._m11 + matrix1._m12 * matrix2._m21 +
            matrix1._m13 * matrix2._m31 + 0 * matrix2._offsetX,
            matrix1._m11 * matrix2._m12 + matrix1._m12 * matrix2._m22 +
            matrix1._m13 * matrix2._m32 + 0 * matrix2._offsetY,
            matrix1._m11 * matrix2._m13 + matrix1._m12 * matrix2._m23 +
            matrix1._m13 * matrix2._m33 + 0 * matrix2._offsetZ,
            matrix1._m21 * matrix2._m11 + matrix1._m22 * matrix2._m21 +
            matrix1._m23 * matrix2._m31 + 0 * matrix2._offsetX,
            matrix1._m21 * matrix2._m12 + matrix1._m22 * matrix2._m22 +
            matrix1._m23 * matrix2._m32 + 0 * matrix2._offsetY,
            matrix1._m21 * matrix2._m13 + matrix1._m22 * matrix2._m23 +
            matrix1._m23 * matrix2._m33 + 0 * matrix2._offsetZ,
            matrix1._m31 * matrix2._m11 + matrix1._m32 * matrix2._m21 +
            matrix1._m33 * matrix2._m31 + 0 * matrix2._offsetX,
            matrix1._m31 * matrix2._m12 + matrix1._m32 * matrix2._m22 +
            matrix1._m33 * matrix2._m32 + 0 * matrix2._offsetY,
            matrix1._m31 * matrix2._m13 + matrix1._m32 * matrix2._m23 +
            matrix1._m33 * matrix2._m33 + 0 * matrix2._offsetZ,
            matrix1._offsetX * matrix2._m11 + matrix1._offsetY * matrix2._m21 +
            matrix1._offsetZ * matrix2._m31 + 1d * matrix2._offsetX,
            matrix1._offsetX * matrix2._m12 + matrix1._offsetY * matrix2._m22 +
            matrix1._offsetZ * matrix2._m32 + 1d * matrix2._offsetY,
            matrix1._offsetX * matrix2._m13 + matrix1._offsetY * matrix2._m23 +
            matrix1._offsetZ * matrix2._m33 + 1d * matrix2._offsetZ);

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
    private string ConvertToString(string? format, IFormatProvider? provider)
    {
        if (IsIdentity)
        {
            return "Identity";
        }

        // Helper to get the numeric list separator for a given culture.
        char separator = MsCompatibility.GetNumericListSeparator(provider);
        return string.Format(provider,
            "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}{0}{5:" + format + "}{0}{6:" + format + "}{0}{7:" + format +
            "}{0}{8:" + format + "}{0}{9:" + format + "}{0}{10:" + format + "}{0}{11:" + format + "}{0}{12:" + format + "}",
            separator,
            _m11,
            _m12,
            _m13,
            _m21,
            _m22,
            _m23,
            _m31,
            _m32,
            _m33,
            _offsetX,
            _offsetY,
            _offsetZ);
    }


    /// <summary>
    ///     Silnie typowane porównanie z optymalizacją dla Identity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Matrix3D other)
    {
        if (_type != other._type)
            return false;
        if (_type == AffinityMatrixTypes3.IsIdentity)
            return true;

        // ReSharper disable CompareOfFloatsByEqualityOperator

        var translationEquals =
            _offsetX == other._offsetX
            && _offsetY == other._offsetY
            && _offsetZ == other._offsetZ;
        if (!translationEquals)
            return false;
        if (_type == AffinityMatrixTypes3.IsAfiniteTranslation)
            return true;

        var rotationEquals =
            _m11 == other._m11 && _m12 == other._m12 && _m13 == other._m13 &&
            _m21 == other._m21 && _m22 == other._m22 && _m23 == other._m23 &&
            _m31 == other._m31 && _m32 == other._m32 && _m33 == other._m33;
        return rotationEquals;
        // ReSharper restore CompareOfFloatsByEqualityOperator
    }

    /// <summary>
    ///     GetHashCode zoptymalizowany pod kątem flagi Identity.
    /// </summary>
    public override int GetHashCode()
    {
        if (_type == AffinityMatrixTypes3.IsIdentity)
            return 0;

        var hash = new HashCode();
        hash.Add(_type);
        if (_type != AffinityMatrixTypes3.IsAfiniteTranslation)
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
        if (_type == AffinityMatrixTypes3.IsIdentity)
            return;

        var x = point.X;
        var y = point.Y;
        var z = point.Z;

        if (_type == AffinityMatrixTypes3.IsAfiniteTranslation)
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
        point = new Point3D(outX, outY, outZ);
    }


    //  Multiplies the given Vector3D by this matrix.
    internal void MultiplyVector(ref Vector3D vector)
    {
        if (_type == AffinityMatrixTypes3.IsIdentity)
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

    public Matrix3D WithOffset(double x, double y, double z)
    {
        return new Matrix3D(
            M11, _m12, _m13,
            _m21, M22, _m23,
            _m31, _m32, M33,
            x, y, z
        );
    }

    public double M11 => _type == AffinityMatrixTypes3.IsIdentity ? 1.0 : _m11;
    public double M22 => _type == AffinityMatrixTypes3.IsIdentity ? 1.0 : _m22;
    public double M33 => _type == AffinityMatrixTypes3.IsIdentity ? 1.0 : _m33;


    public double M12 => _m12;
    public double M13 => _m13;

    public double M21 => _m21;
    public double M23 => _m23;

    public double M31 => _m31;
    public double M32 => _m32;

    public double OffsetX => _offsetX; // Translation X
    public double OffsetY => _offsetY; // Translation Y
    public double OffsetZ => _offsetZ; // Translation Z

    public bool IsIdentity => _type == AffinityMatrixTypes3.IsIdentity;

    public static Matrix3D Identity => new Matrix3D();

    private readonly double _m11, _m12, _m13;
    private readonly double _m21, _m22, _m23;
    private readonly double _m31, _m32, _m33;
    private readonly double _offsetX, _offsetY, _offsetZ;


    private readonly AffinityMatrixTypes3 _type;
}
