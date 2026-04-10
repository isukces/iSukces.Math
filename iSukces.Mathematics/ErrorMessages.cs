// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.

using System;

namespace iSukces.Mathematics;

/// <summary>
///     Centralized error messages for the mathematics library.
///     Provides consistent, localizable error messages.
/// </summary>
public static class ErrorMessages
{
    /// <summary>
    ///     Error message for invalid vector (NaN values).
    /// </summary>
    public const string InvalidVector = "Invalid vector - contains NaN values";

    /// <summary>
    ///     Error message for points with same X coordinate in linear function.
    /// </summary>
    public const string PointsCannotHaveSameX = "Points cannot have the same X coordinate";

    /// <summary>
    ///     Error message for invalid point in topology operations.
    /// </summary>
    public const string InvalidPoint = "Invalid point";

    /// <summary>
    ///     Error message for negative size arguments.
    /// </summary>
    public const string NegativeSizeArgument = "Size argument must be non-negative";

    /// <summary>
    ///     Error message for non-invertible matrix.
    /// </summary>
    public const string MatrixNotInvertible = "Matrix is not invertible";

    /// <summary>
    ///     Error message for zero axis in quaternion operations.
    /// </summary>
    public const string ZeroAxisSpecified = "Cannot use (0,0,0) as rotation axis";
}

/// <summary>
///     Base exception for domain-specific errors in the mathematics library.
/// </summary>
public abstract class MathematicsException : Exception
{
    protected MathematicsException(string message) : base(message) { }
    protected MathematicsException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
///     Exception thrown when an invalid vector is encountered (NaN values).
/// </summary>
public sealed class InvalidVectorException : MathematicsException
{
    public InvalidVectorException() : base(ErrorMessages.InvalidVector) { }
    public InvalidVectorException(string message) : base(message) { }
    public InvalidVectorException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
///     Exception thrown when geometric operations fail due to invalid input.
/// </summary>
public sealed class GeometryException : MathematicsException
{
    public GeometryException() : base("Geometry operation failed") { }
    public GeometryException(string message) : base(message) { }
    public GeometryException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
///     Exception thrown when matrix operations fail (e.g., non-invertible matrix).
/// </summary>
public sealed class MatrixException : MathematicsException
{
    public MatrixException() : base("Matrix operation failed") { }
    public MatrixException(string message) : base(message) { }
    public MatrixException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
///     Exception thrown when size constraints are violated (negative values).
/// </summary>
public sealed class SizeException : MathematicsException
{
    public SizeException() : base(ErrorMessages.NegativeSizeArgument) { }
    public SizeException(string message) : base(message) { }
    public SizeException(string message, Exception innerException) : base(message, innerException) { }
}
