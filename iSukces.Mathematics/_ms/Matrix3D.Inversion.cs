// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;
using System.Diagnostics;

namespace iSukces.Mathematics;

public readonly partial record struct Matrix3D
{
    /// <summary>
    ///     Computes, and substitutes in-place, the inverse of a matrix.
    ///     The determinant of the matrix must be nonzero, otherwise the matrix is not invertible.
    ///     In this case it will throw InvalidOperationException exception.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     This will throw InvalidOperationException if the matrix is not invertible.
    /// </exception>
    public Matrix3D GetInverted()
    {
        if (!InvertCore(out var inverted))
        {
            throw new InvalidOperationException("Matrix3D_NotInvertible");
        }

        return inverted;
    }


    // RETURNS true if has inverse & invert was done.  Otherwise returns false & leaves matrix unchanged.
    private bool InvertCore(out Matrix3D inverted)
    {
        if (IsIdentity)
        {
            inverted = this;
            return true;
        }

        // NOTE: The beginning of this code is duplicated between
        //       GetNormalizedAffineDeterminant() and NormalizedAffineInvert()

        var z20 = _m12 * _m23 - _m22 * _m13;
        var z10 = _m32 * _m13 - _m12 * _m33;
        var z00 = _m22 * _m33 - _m32 * _m23;
        var det = _m31 * z20 + _m21 * z10 + _m11 * z00;

        // Fancy logic here avoids using equality with possible nan values.
        Debug.Assert(!(det < Determinant || det > Determinant),
            "Matrix3D.Inverse: Determinant property does not match value computed in Inverse.");

        if (DoubleUtil.IsZero(det))
        {
            inverted = default;
            return false;
        }

        // Compute 3x3 non-zero cofactors for the 2nd column
        var z21 = _m21 * _m13 - _m11 * _m23;
        var z11 = _m11 * _m33 - _m31 * _m13;
        var z01 = _m31 * _m23 - _m21 * _m33;

        // Compute all six 2x2 determinants of 1st two columns
        var y01 = _m11 * _m22 - _m21 * _m12;
        var y02 = _m11 * _m32 - _m31 * _m12;
        var y03 = _m11 * _offsetY - _offsetX * _m12;
        var y12 = _m21 * _m32 - _m31 * _m22;
        var y13 = _m21 * _offsetY - _offsetX * _m22;
        var y23 = _m31 * _offsetY - _offsetX * _m32;

        // Compute all non-zero and non-one 3x3 cofactors for 2nd
        // two columns
        var z23 = _m23 * y03 - _offsetZ * y01 - _m13 * y13;
        var z13 = _m13 * y23 - _m33 * y03 + _offsetZ * y02;
        var z03 = _m33 * y13 - _offsetZ * y12 - _m23 * y23;
        var z22 = y01;
        var z12 = -y02;
        var z02 = y12;

        var rcp = 1.0 / det;

        // Multiply all 3x3 cofactors by reciprocal & transpose
        inverted = new Matrix3D(
            z00 * rcp,
            z10 * rcp,
            z20 * rcp,
            z01 * rcp,
            z11 * rcp,
            z21 * rcp,
            z02 * rcp,
            z12 * rcp,
            z22 * rcp,
            z03 * rcp,
            z13 * rcp,
            z23 * rcp);

        return true;
    }

    /// <summary>
    ///     Matrix determinant.
    /// </summary>
    public double Determinant
    {
        get
        {
            if (_type != AffinityMatrixTypes3.Other)
                return 1.0;
            var z20 = _m12 * _m23 - _m22 * _m13;
            var z10 = _m32 * _m13 - _m12 * _m33;
            var z00 = _m22 * _m33 - _m32 * _m23;
            return _m31 * z20 + _m21 * z10 + _m11 * z00;
        }
    }

    /// <summary>
    ///     Whether the matrix has an inverse.
    /// </summary>
    public bool HasInverse => !DoubleUtil.IsZero(Determinant);

    // RET
}
