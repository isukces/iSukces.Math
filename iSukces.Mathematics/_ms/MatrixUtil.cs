// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

namespace iSukces.Mathematics;

internal static class MatrixUtil
{
    /// <summary>
    ///     Multiplies two transformations, where the behavior is matrix1 *= matrix2.
    ///     This code exists so that we can efficient combine matrices without copying
    ///     the data around, since each matrix is 52 bytes.
    ///     To reduce duplication and to ensure consistent behavior, this is the
    ///     method which is used to implement Matrix * Matrix as well.
    /// </summary>
    internal static Matrix MultiplyMatrix(ref Matrix matrix1, ref Matrix matrix2)
    {
        MatrixTypes type1 = matrix1._type;
        MatrixTypes type2 = matrix2._type;

        // Check for idents

        if (type2 == MatrixTypes.TRANSFORM_IS_IDENTITY)
            return matrix1;

        if (type1 == MatrixTypes.TRANSFORM_IS_IDENTITY)
            return matrix2;

        // Optimize for translate case, where the second is a translate
        if (type2 == MatrixTypes.TRANSFORM_IS_TRANSLATION)
        {
            return new Matrix(
                matrix1._m11,
                matrix1._m12,
                matrix1._m21,
                matrix1._m22,
                matrix1._offsetX + matrix2._offsetX,
                matrix1._offsetY + matrix2._offsetY);
        }

        // Check for the first value being a translate
        if (type1 == MatrixTypes.TRANSFORM_IS_TRANSLATION)
        {
            // Save off the old offsets
            var offsetX = matrix1._offsetX;
            var offsetY = matrix1._offsetY;

            // Copy the matrix
            matrix1 = matrix2;

            var newOffsetX = offsetX * matrix2._m11 + offsetY * matrix2._m21 + matrix2._offsetX;
            var newOffsetY = offsetX * matrix2._m12 + offsetY * matrix2._m22 + matrix2._offsetY;

            return new Matrix(
                matrix1._m11,
                matrix1._m12,
                matrix1._m21,
                matrix1._m22,
                newOffsetX,
                newOffsetY);
        }

        // The following code combines the type of the transformations so that the high nibble
        // is "this"'s type, and the low nibble is mat's type.  This allows for a switch rather
        // than nested switches.

        // trans1._type |  trans2._type
        //  7  6  5  4   |  3  2  1  0
        int combinedType = ((int)type1 << 4) | (int)type2;

        switch (combinedType)
        {
            case 34: // S * S
                // 2 multiplications
                return new Matrix(
                    matrix1._m11 * matrix2._m11,
                    matrix1._m12,
                    matrix1._m21,
                    matrix1._m22 * matrix2._m22,
                    matrix1._offsetX,
                    matrix1._offsetY);

            //matrix1._m11 *= matrix2._m11;
            //matrix1._m22 *= matrix2._m22;

            case 35: // S * S|T

                return new Matrix(
                    matrix1._m11 * matrix2._m11,
                    matrix1._m12,
                    matrix1._m21,
                    matrix1._m22 * matrix2._m22,
                    matrix2._offsetX,
                    matrix2._offsetY);
            /*matrix1._m11     *= matrix2._m11;
            matrix1._m22     *= matrix2._m22;
            matrix1._offsetX =  matrix2._offsetX;
            matrix1._offsetY =  matrix2._offsetY;

            // Transform set to Translate and Scale
            matrix1._type = MatrixTypes.TRANSFORM_IS_TRANSLATION | MatrixTypes.TRANSFORM_IS_SCALING;
            return;*/

            case 50: // S|T * S
                return new Matrix(
                    matrix1._m11 * matrix2._m11,
                    matrix1._m12,
                    matrix1._m21,
                    matrix1._m22 * matrix2._m22,
                    matrix1._offsetX * matrix2._m11,
                    matrix1._offsetY * matrix2._m22);
            /*matrix1._m11     *= matrix2._m11;
            matrix1._m22     *= matrix2._m22;
            matrix1._offsetX *= matrix2._m11;
            matrix1._offsetY *= matrix2._m22;*/

            case 51: // S|T * S|T
                return new Matrix(
                    matrix1._m11 * matrix2._m11,
                    matrix1._m12,
                    matrix1._m21,
                    matrix1._m22 * matrix2._m22,
                    matrix2._m11 * matrix1._offsetX + matrix2._offsetX,
                    matrix2._m22 * matrix1._offsetY + matrix2._offsetY);

            /*matrix1._m11     *= matrix2._m11;
            matrix1._m22     *= matrix2._m22;
            matrix1._offsetX =  matrix2._m11 * matrix1._offsetX + matrix2._offsetX;
            matrix1._offsetY =  matrix2._m22 * matrix1._offsetY + matrix2._offsetY;
            */

            /*case 36: // S * U
            case 52: // S|T * U
            case 66: // U * S
            case 67: // U * S|T
            case 68: // U * U*/
            default:
                return new Matrix(
                    matrix1._m11 * matrix2._m11 + matrix1._m12 * matrix2._m21,
                    matrix1._m11 * matrix2._m12 + matrix1._m12 * matrix2._m22,
                    matrix1._m21 * matrix2._m11 + matrix1._m22 * matrix2._m21,
                    matrix1._m21 * matrix2._m12 + matrix1._m22 * matrix2._m22,
                    matrix1._offsetX * matrix2._m11 + matrix1._offsetY * matrix2._m21 + matrix2._offsetX,
                    matrix1._offsetX * matrix2._m12 + matrix1._offsetY * matrix2._m22 + matrix2._offsetY);

#if DEBUGx
            default:
                Debug.Fail("Matrix multiply hit an invalid case: " + combinedType);
                break;
#endif
        }
    }
}
