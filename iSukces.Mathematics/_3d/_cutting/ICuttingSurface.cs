using System.Collections.Generic;

#if !WPFFEATURES
#else
using System.Windows.Media.Media3D;
#endif


namespace iSukces.Mathematics;

/// <summary>
///     Interfejs powierzchni obcinającej
/// </summary>
public interface ICuttingSurface
{
    /// <summary>
    ///     Oblicza wektor normalny we wskazanym punkcie
    /// </summary>
    /// <param name="x">współrzędna x</param>
    /// <param name="y">współrzędna y</param>
    /// <returns>wektor normalny</returns>
    Vector3D? CalculateNormal(double x, double y);

    Point3D CalculatePoint(double x, double y);
    Point3D CalculatePoint(double x, double y, double z);

    /// <summary>
    ///     Oblicza wysokość obcięcia w punkcie x,y
    /// </summary>
    /// <param name="x">współrzędna x</param>
    /// <param name="y">współrzędna y</param>
    /// <returns>współrzędna z</returns>
    double CalculateZ(double x, double y);

    string GetCompareString();

    /// <summary>
    ///     Zwraca listę współrzędnych x, na których jest załamanie płaszczyzny
    /// </summary>
    /// <param name="xmin">początek zakresu</param>
    /// <param name="xmax">koniec zakresu zakresu</param>
    /// <returns>lista współrzędnych</returns>
    IList<double>? GetXEdges(double xmin, double xmax);

    /// <summary>
    ///     Zwraca listę współrzędnych y, na których jest załamanie płaszczyzny
    /// </summary>
    /// <param name="xmin">początek zakresu</param>
    /// <param name="xmax">koniec zakresu zakresu</param>
    /// <returns>lista współrzędnych</returns>
    IList<double>? GetYEdges(double ymin, double ymax);

    /// <summary>
    ///     Czy posiada ostre krawędzie poziome (dzielące profil na fragmenty wg zakresów Y)
    /// </summary>
    bool HasSharpYEdges { get; }
}