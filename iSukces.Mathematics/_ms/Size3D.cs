// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

public readonly struct Size3D : IEquatable<Size3D>
{
    public Size3D(double x, double y, double z)
    {
        if (x < 0 || y < 0 || z < 0)
            throw new ArgumentException(ua);
        X = x;
        Y = y;
        Z = z;
    }

    private Size3D(double x, double y, double z, bool dummy)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static bool operator ==(Size3D a, Size3D b)
    {
        return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    }

    public static bool operator !=(Size3D a, Size3D b)
    {
        return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    }

    public bool Equals(Size3D other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        return obj is Size3D other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = X.GetHashCode();
            hashCode = (hashCode * 397) ^ Y.GetHashCode();
            hashCode = (hashCode * 397) ^ Z.GetHashCode();
            return hashCode;
        }
    }

    public Size3D WithX(double x)
    {
        return new Size3D(x, Y, Z);
    }

    public Size3D WithY(double y)
    {
        return new Size3D(X, y, Z);
    }

    public Size3D WithZ(double z)
    {
        return new Size3D(X, Y, z);
    }

    public static Size3D Empty { get; } = new Size3D(
        double.NegativeInfinity,
        double.NegativeInfinity,
        double.NegativeInfinity,
        true);

    public bool IsEmpty => X < 0;

    public double X { get; }

    public double Y { get; }

    public double Z { get; }

    private const string ua = "ujemny argument";
    
    
    public override string ToString()
    {
        if (IsEmpty)
            return "Empty";
        var numericListSeparator = MsCompatibility.GetNumericListSeparator(null);
        return $"{X}{numericListSeparator}{Y}{numericListSeparator}{Z}";
    }
}
