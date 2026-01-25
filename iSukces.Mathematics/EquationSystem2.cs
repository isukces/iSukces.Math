namespace iSukces.Mathematics;

/// <summary>
/// Układ 2 współrzędnych liniowych
/// a1 x + b1 y + c1 = 0
/// a2 x + b2 y + c2 = 0
/// </summary>
public sealed class EquationSystem2
{
    public EquationSystem2()
    {

    }

    public EquationSystem2(double a1, double b1, double c1, double a2, double b2, double c2)
    {
        A1 = a1;
        B1 = b1;
        C1 = c1;

        A2 = a2;
        B2 = b2;
        C2 = c2;

    }

    /// <summary>
    /// Wspóczynnik A1
    /// </summary>
    public double A1 { get; set; }

    /// <summary>
    /// Wspóczynnik A2
    /// </summary>
    public double A2 { get; set; }

    /// <summary>
    /// Wspóczynnik B1
    /// </summary>
    public double B1 { get; set; }

    /// <summary>
    /// Wspóczynnik B2
    /// </summary>
    public double B2 { get; set; }

    /// <summary>
    /// Wspóczynnik C1
    /// </summary>
    public double C1 { get; set; }

    /// <summary>
    /// Wspóczynnik C2
    /// </summary>
    public double C2 { get; set; }

    public double Determinant
    {
        get { return A1 * B2 - A2 * B1; }
    }

    public double DeterminantX
    {
        get { return B1 * C2 - B2 * C1; }
    }

    public double DeterminantY
    {
        get { return A2 * C1 - A1 * C2; }
    }

    public Point? Solution
    {
        get
        {
            double w = Determinant;
            if (w == 0)
                return null;
            return new Point(DeterminantX / w, DeterminantY / w);
        }
    }
}