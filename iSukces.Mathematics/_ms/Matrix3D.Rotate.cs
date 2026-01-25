// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

namespace iSukces.Mathematics;

public readonly partial record struct Matrix3D
{
    //  Creates a rotation matrix given a quaternion and center.
    //
    //  Quaternion and center are passed by reference for performance
    //  only and are not modified.
    //
    internal static Matrix3D CreateRotationMatrix(ref Quaternion quaternion, ref Point3D center)
    {
        var x2 = quaternion.X + quaternion.X;
        var y2 = quaternion.Y + quaternion.Y;
        var z2 = quaternion.Z + quaternion.Z;
        var xx = quaternion.X * x2;
        var xy = quaternion.X * y2;
        var xz = quaternion.X * z2;
        var yy = quaternion.Y * y2;
        var yz = quaternion.Y * z2;
        var zz = quaternion.Z * z2;
        var wx = quaternion.W * x2;
        var wy = quaternion.W * y2;
        var wz = quaternion.W * z2;

        var m11 = 1.0 - (yy + zz);
        var m12 = xy + wz;
        var m13 = xz - wy;
        var m21 = xy - wz;
        var m22 = 1.0 - (xx + zz);
        var m23 = yz + wx;
        var m31 = xz + wy;
        var m32 = yz - wx;
        var m33 = 1.0 - (xx + yy);

        if (center.X != 0 || center.Y != 0 || center.Z != 0)
        {
            var offsetX = -center.X * m11 - center.Y * m21 - center.Z * m31 + center.X;
            var offsetY = -center.X * m12 - center.Y * m22 - center.Z * m32 + center.Y;
            var offsetZ = -center.X * m13 - center.Y * m23 - center.Z * m33 + center.Z;

            return new Matrix3D(m11, m12, m13,
                m21, m22, m23,
                m31, m32, m33,
                offsetX, offsetY, offsetZ
            );
        }

        return new Matrix3D(m11, m12, m13,
            m21, m22, m23,
            m31, m32, m33,
            0, 0, 0
        );
    }

    /// <summary>
    ///     Appends rotation transform to the current matrix.
    /// </summary>
    /// <param name="quaternion">Quaternion representing rotation.</param>
    public Matrix3D GetRotated(Quaternion quaternion)
    {
        Point3D center = new Point3D();

        return this * CreateRotationMatrix(ref quaternion, ref center);
    }

    /// <summary>
    ///     Appends rotation transform to the current matrix.
    /// </summary>
    /// <param name="quaternion">Quaternion representing rotation.</param>
    /// <param name="center">Center to rotate around.</param>
    public Matrix3D GetRotatedAt(Quaternion quaternion, Point3D center)
    {
        return this * CreateRotationMatrix(ref quaternion, ref center);
    }

    /// <summary>
    ///     Prepends rotation transform to the current matrix.
    /// </summary>
    /// <param name="quaternion">Quaternion representing rotation.</param>
    /// <param name="center">Center to rotate around.</param>
    public Matrix3D GetRotatedAtPrepend(Quaternion quaternion, Point3D center)
    {
        return CreateRotationMatrix(ref quaternion, ref center) * this;
    }

    /// <summary>
    ///     Prepends rotation transform to the current matrix.
    /// </summary>
    /// <param name="quaternion">Quaternion representing rotation.</param>
    public Matrix3D GetRotatedPrepend(Quaternion quaternion)
    {
        Point3D center = new Point3D();
        return CreateRotationMatrix(ref quaternion, ref center) * this;
    }
}
