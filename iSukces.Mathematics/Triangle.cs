using System;

namespace iSukces.Helix;

/// <summary>
///     Trójkąt
/// </summary>
public sealed class Triangle
{
    /// <summary>
    ///     Oblicza przeciwprostokątną
    /// </summary>
    /// <param name="a">bok a</param>
    /// <param name="b">bok b</param>
    /// <returns></returns>
    public static double ComputeHypotenuse(double a, double b)
    {
        return Math.Sqrt(a * a + b * b);
    }


    public static double ComputeSide(double c, double a)
    {
        return Math.Sqrt(c * c - a * a);
    }

    /// <summary>
    ///     Realizuje operator ==
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są równe</returns>
    public static bool operator ==(Triangle? left, Triangle? right)
    {
        if (left == (object?)null && right == (object?)null) return true;
        if (left == (object?)null || right == (object?)null) return false;
        return left.A == right.A && left.B == right.B && left.C == right.C;
    }

    /// <summary>
    ///     Realizuje operator !=
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są różne</returns>
    public static bool operator !=(Triangle? left, Triangle? right)
    {
        var ln = ReferenceEquals(left, null);
        var rn = ReferenceEquals(right, null);
        if (ln && rn) return false;
        if (ln || rn) return true;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (left.A != right.A) return true;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (left.B != right.B) return true;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return left.C != right.C;
        /*
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return left.A == right.A
            || left.B == right.B || left.C == right.C;
        if (left != (object)null && right != (object)null) return false;
        if (left != (object)null || right != (object)null) return true;
        return left.A == right.A || left.B == right.B || left.C == right.C;
         */
    }

    /// <summary>
    ///     Pole trójkąta na podstawie długości boków
    /// </summary>
    /// <param name="a">długość boku a</param>
    /// <param name="b">długość boku b</param>
    /// <param name="c">długość boku c</param>
    /// <returns>pole trójkąta</returns>
    public static double TriangleArea(double a, double b, double c)
    {
        return Math.Sqrt(TriangleArea2(a, b, c));
    }


    /// <summary>
    ///     Kwadrat pola trójkąta na podstawie długości boków
    /// </summary>
    /// <param name="a">długość boku a</param>
    /// <param name="b">długość boku b</param>
    /// <param name="c">długość boku c</param>
    /// <returns>kwadrat pola trójkąta</returns>
    public static double TriangleArea2(double a, double b, double c)
    {
        // wzór Herona http://pl.wikipedia.org/wiki/Wz%C3%B3r_Herona
        var p = (a + b + c) / 2.0;
        return p * (p - a) * (p - b) * (p - c);
    }

    /// <summary>
    ///     Wykonuje głęboką kopię obiektu
    /// </summary>
    /// <returns>kopia obiektu</returns>
    public object Clone()
    {
        var result = new Triangle();
        result.CopyFrom(this);
        return result;
    }

    /// <summary>
    ///     Kopiuje własności z obiektu źródłowego
    /// </summary>
    /// <param name="source">obiekt źródłowy kopiowania</param>
    public void CopyFrom(Triangle source)
    {
        if (source == (object?)null)
            throw new ArgumentNullException("Source");
        A = source.A;
        B = source.B;
        C = source.C;
    }

    /// <summary>
    ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
    /// </summary>
    /// <param name="obj">obiekt do porównania z obiektem bieżącym</param>
    /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
    public override bool Equals(object? obj)
    {
        if (obj is Triangle) return (Triangle)obj == this;
        return false;
    }

    /// <summary>
    ///     Zwraca kod HASH obiektu
    /// </summary>
    /// <returns>kod HASH obiektu</returns>
    public override int GetHashCode()
    {
        return A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode();
    }


    private void Update()
    {
        _area = TriangleArea(_a, _b, _c);
    }

    /// <summary>
    ///     bok a
    /// </summary>
    public double A
    {
        get => _a;
        set
        {
            if (_a == value) return;
            _a = value;
            Update();
        }
    }

    /// <summary>
    ///     bok b
    /// </summary>
    public double B
    {
        get => _b;
        set
        {
            if (_b == value) return;
            _b = value;
            Update();
        }
    }

    /// <summary>
    ///     bok c
    /// </summary>
    public double C
    {
        get => _c;
        set
        {
            if (_c == value) return;
            _c = value;
            Update();
        }
    }

    /// <summary>
    ///     Powierzchnia
    /// </summary>
    public double Area => _area;

    /// <summary>
    ///     Wysokość na bok a
    /// </summary>
    public double HA => _area * 2 / _a;


    /// <summary>
    ///     Wysokość na bok b
    /// </summary>
    public double HB => _area * 2 / _b;

    /// <summary>
    ///     Wysokość na bok c
    /// </summary>
    public double HC => _area * 2 / _c;

    private double _a;

    private double _b;

    private double _c;
    
    private double _area;
}
