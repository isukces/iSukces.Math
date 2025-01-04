using System;

namespace iSukces.Mathematics;

public class Circle
{
    public Circle()
        : this(0)
    {
    }
    public Circle(double radius)
    {
        UpdateFromRadius(radius);
    }

    /// <summary>
    /// Promień
    /// </summary>
    public double Radius
    {
        get { return _radius; }
        set
        {
            if (_radius == value) return;
            UpdateFromRadius(value);
        }
    }
    protected double _radius;

    /// <summary>
    /// Średnica
    /// </summary>
    public double Diameter
    {
        get { return diameter; }
        set
        {
            if (diameter == value) return;
            UpdateFromDiameter(value);
        }
    }
    private double diameter;


    /// <summary>
    /// Pole powierzchni koła
    /// </summary>
    public double Area
    {
        get
        {
            return area;
        }
        set
        {
            if (area == value) return;
            UpdateFromArea(value);
        }
    }
    private double area;


    /// <summary>
    /// Obwód
    /// </summary>
    public double Perimeter
    {
        get
        {
            return perimeter;
        }
        set
        {
            if (perimeter == value) return;
            UpdateFromPerimeter(value);
        }
    }
    private double perimeter;


    /// <summary>
    /// Zwraca kod HASH obiektu
    /// </summary>
    /// <returns>kod HASH obiektu</returns>
    public override int GetHashCode()
    {
        return _radius.GetHashCode();
    }


    protected void UpdateFromRadius(double value)
    {
        _radius   = value;
        diameter  = value * 2;
        area      = Math.PI * value * value;
        perimeter = value * MathEx.DoublePI;
    }
    protected void UpdateFromDiameter(double value)
    {
        UpdateFromRadius(value / 2);
        diameter = value;
    }
    protected void UpdateFromArea(double value)
    {
        UpdateFromRadius(Math.Sqrt(value / Math.PI));
        area = value;
    }
    protected void UpdateFromPerimeter(double value)
    {
        UpdateFromRadius(value / MathEx.DoublePI);
        perimeter = value;
    }


    /// <summary>
    /// Sprawdza, czy wskazany obiekt jest równy bieżącemu
    /// </summary>
    /// <param name="obj">obiekt do porównania z obiektem bieżącym</param>
    /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
    public override bool Equals(object? obj)
    {
        if (obj is Circle) return (Circle)obj == this;
        return false;
    }

    /// <summary>
    /// Oblicza obwód na podstawie promienia
    /// </summary>
    /// <param name="radius">promień</param>
    /// <returns>średnica</returns>
    public static double ComputePerimeter(double radius)
    {
        return MathEx.DoublePI * radius;
    }

    public static double ComputeAreaFromDiameter(double diameter)
    {
        return Math.PI * diameter * diameter / 4;
    }

    /// <summary>
    /// Realizuje operator !=
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są różne</returns>
    public static bool operator !=(Circle? left, Circle? right)
    {
        var eq = left == right;
        return !eq;
    }

    /// <summary>
    /// Realizuje operator ==
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są równe</returns>
    public static bool operator ==(Circle? left, Circle? right)
    {
        if (left == (object?)null && right == (object?)null) return true;
        if (left == (object?)null || right == (object?)null) return false;
        return left._radius == right._radius;
    }
}