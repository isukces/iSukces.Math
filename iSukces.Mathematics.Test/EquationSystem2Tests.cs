// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.

using Xunit;

namespace iSukces.Mathematics.Test;

/// <summary>
///     Tests for EquationSystem2 class
/// </summary>
public sealed class EquationSystem2Tests
{
    [Theory]
    [InlineData(1, 2, 3, 4, -2)]
    [InlineData(2, 3, 1, 4, 5)]
    [InlineData(5, 1, 2, 3, 13)]
    public void T01_Determinant_Should_Be_Calculated_Correctly(double a1, double b1, double a2, double b2,
        double expected)
    {
        // Arrange
        var system = new EquationSystem2(a1, b1, 0, a2, b2, 0);

        // Act & Assert: W = a1×b2 - a2×b1
        Assert.Equal(expected, system.Determinant, 12);
    }

    [Theory]
    [InlineData(1, 2, -3, 3, 4, -5, 2)]
    public void T02_DeterminantX_Should_Be_Calculated_Correctly(double a1, double b1, double c1, double a2,
        double b2, double c2, double expected)
    {
        // Arrange
        var system = new EquationSystem2(a1, b1, c1, a2, b2, c2);

        // Act & Assert: Wx = b1×c2 - b2×c1
        Assert.Equal(expected, system.DeterminantX, 12);
    }

    [Theory]
    [InlineData(1, 2, -3, 3, 4, -5, -4)]
    public void T03_DeterminantY_Should_Be_Calculated_Correctly(double a1, double b1, double c1, double a2,
        double b2, double c2, double expected)
    {
        // Arrange
        var system = new EquationSystem2(a1, b1, c1, a2, b2, c2);

        // Act & Assert: Wy = a2×c1 - a1×c2
        Assert.Equal(expected, system.DeterminantY, 12);
    }

    [Theory]
    [InlineData(1.37, -3.11, 1, 0, 0, 1)]
    [InlineData(1.37, -3.11, 0, 1, 1, 0)]
    [InlineData(1.37, -3.11, 1, 0, 4, 1)]
    [InlineData(1.37, -3.11, 4, 1, 1, 0)]
    [InlineData(-1.37, -3.11, 1, 0, 0, 1)]
    [InlineData(-1.37, -3.11, 0, 1, 1, 0)]
    [InlineData(-1.37, -3.11, 1, 0, 4, 1)]
    [InlineData(-1.37, -3.11, 4, 1, 1, 0)]
    public void T04_Solution_Should_Return_Correct_Point(double x, double y,
        double a1, double b1, double a2, double b2)
    {
        var c1 = -(x * a1 + y * b1);
        var c2 = -(x * a2 + y * b2);
        // Arrange
        var system = new EquationSystem2(a1, b1, c1, a2, b2, c2);

        // Act
        var solution = system.Solution;

        // Assert: x = Wx/W, y = Wy/W
        Assert.NotNull(solution);
        Assert.Equal(x, solution.Value.X, 12);
        Assert.Equal(y, solution.Value.Y, 12);
    }

    [Theory]
    [InlineData(1, 2, 3, 2, 4, 5)]
    [InlineData(1, 1, 1, 1, 1, 2)]
    public void T05_Solution_Should_Return_Null_When_Lines_Are_Parallel(double a1, double b1, double c1, double a2,
        double b2, double c2)
    {
        // Arrange
        var system = new EquationSystem2(a1, b1, c1, a2, b2, c2);

        // Act & Assert
        Assert.Null(system.Solution);
        Assert.Equal(0, system.Determinant, 12);
    }

    [Theory]
    [InlineData(1, 2, 3, 2, 4, 6)]
    [InlineData(1, 1, 1, 2, 2, 2)]
    public void T06_Solution_Should_Return_Null_When_Lines_Are_Coincident(double a1, double b1, double c1, double a2,
        double b2, double c2)
    {
        // Arrange
        var system = new EquationSystem2(a1, b1, c1, a2, b2, c2);

        // Act & Assert
        Assert.Null(system.Solution);
        Assert.Equal(0, system.Determinant, 12);
    }

    [Theory]
    [InlineData(1, 2, -3, 3, 4, -5)]
    [InlineData(3, -2, -1, 3, 4, -5)]
    public void T07_Solution_Should_Satisfy_Both_Equations(double a1, double b1, double c1, double a2,
        double b2, double c2)
    {
        // Arrange
        var system = new EquationSystem2(a1, b1, c1, a2, b2, c2);

        // Act
        var solution = system.Solution;

        // Assert: a1×x + b1×y + c1 = 0 AND a2×x + b2×y + c2 = 0
        Assert.NotNull(solution);
        var x = solution.Value.X;
        var y = solution.Value.Y;

        // First equation: a1×x + b1×y + c1 = 0
        var eq1 = a1 * x + b1 * y + c1;
        Assert.Equal(0, eq1, 12);

        // Second equation: a2×x + b2×y + c2 = 0
        var eq2 = a2 * x + b2 * y + c2;
        Assert.Equal(0, eq2, 12);
    }

    [Fact]
    public void T08_Default_Constructor_Should_Create_Zero_System()
    {
        // Act
        var system = new EquationSystem2();

        // Assert: All coefficients = 0
        Assert.Equal(0, system.A1, 12);
        Assert.Equal(0, system.B1, 12);
        Assert.Equal(0, system.C1, 12);
        Assert.Equal(0, system.A2, 12);
        Assert.Equal(0, system.B2, 12);
        Assert.Equal(0, system.C2, 12);
    }

    [Fact]
    public void T09_Parameterized_Constructor_Should_Set_All_Coefficients()
    {
        // Arrange
        var a1 = 1.0;
        var b1 = 2.0;
        var c1 = 3.0;
        var a2 = 4.0;
        var b2 = 5.0;
        var c2 = 6.0;

        // Act
        var system = new EquationSystem2(a1, b1, c1, a2, b2, c2);

        // Assert
        Assert.Equal(a1, system.A1, 12);
        Assert.Equal(b1, system.B1, 12);
        Assert.Equal(c1, system.C1, 12);
        Assert.Equal(a2, system.A2, 12);
        Assert.Equal(b2, system.B2, 12);
        Assert.Equal(c2, system.C2, 12);
    }

    [Fact]
    public void T10_Setting_Coefficients_Should_Update_Determinants()
    {
        // Arrange
        var system = new EquationSystem2();

        // Act
        system.A1 = 1;
        system.B1 = 2;
        system.A2 = 3;
        system.B2 = 4;

        // Assert: W = a1×b2 - a2×b1 = 1×4 - 3×2 = -2
        Assert.Equal(-2, system.Determinant, 12);
    }


    [Theory]
    [InlineData(1.3, 2)]
    [InlineData(3, -2)]
    public void T11_Setting_Coefficients_Should_Update_Solution(double x, double y)
    {
        // Arrange
        var system = new EquationSystem2();

        // Act
        system.A1 = 1;
        system.B1 = 2;
        system.A2 = 3;
        system.B2 = 4;
        system.C1 = -(system.A1 * x + system.B1 * y);
        system.C2 = -(system.A2 * x + system.B2 * y);
        // Assert
        Assert.NotNull(system.Solution);
        Assert.Equal(x, system.Solution.Value.X, 12);
        Assert.Equal(y, system.Solution.Value.Y, 12);
    }

    [Theory]
    [InlineData(0, 1, 2, 1, 0, 3, -3, -2)]
    [InlineData(1, 0, 2, 0, 1, 3, -2, -3)]
    public void T12_Triangular_System_Should_Be_Solved_Correctly(double a1, double b1, double c1, double a2,
        double b2, double c2, double expectedX, double expectedY)
    {
        // Arrange
        var system = new EquationSystem2(a1, b1, c1, a2, b2, c2);

        // Act
        var solution = system.Solution;

        // Assert: Test special cases where one coefficient is zero
        Assert.NotNull(solution);
        Assert.Equal(expectedX, solution.Value.X, 12);
        Assert.Equal(expectedY, solution.Value.Y, 12);
    }
}
