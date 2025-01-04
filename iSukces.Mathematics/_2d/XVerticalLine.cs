using System;

namespace iSukces.Mathematics;

/// <summary>
/// Vertical line on XZ or XY plane
/// </summary>
public readonly struct XVerticalLine : IEquatable<XVerticalLine>
{
    public bool Equals(XVerticalLine other)
    {
        return X.Equals(other.X);
    }

    public override string ToString()
    {
        return X.ToString();
    }

    public override bool Equals(object? obj)
    {
        return obj is XVerticalLine other && Equals(other);
    }

    public override int GetHashCode()
    {
        return X.GetHashCode();
    }

    public static bool operator ==(XVerticalLine left, XVerticalLine right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(XVerticalLine left, XVerticalLine right)
    {
        return !left.Equals(right);
    }

    public XVerticalLine(double x)
    {
        X = x;
    }

    public double X { get;  }
}

