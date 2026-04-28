using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
#if !WPFFEATURES
using ThePoint=iSukces.Mathematics.Point;
using TheVector=iSukces.Mathematics.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics;

/// <summary>
/// Reprezentuje płaszczyznę w przestrzeni trójwymiarowej opisaną równaniem
/// <c>Ax + By + Cz + D = 0</c>. Dostarcza metod do obliczania przecięć, rzutowania
/// punktów oraz transformacji przy użyciu współrzędnych 3‑D.
/// </summary>
public sealed class Plane3D : ICloneable
{
    /// <summary>
    /// Tworzy płaszczyznę przechodzącą przez punkt <paramref name="center"/> i
    /// o wektorze normalnym <paramref name="versor"/>.
    /// </summary>
    /// <param name="center">Punkt leżący na płaszczyźnie.</param>
    /// <param name="versor">Wektor normalny (nie musi być znormalizowany).</param>
    public Plane3D(Point3D center, Vector3D versor)
    {
        versor = versor.GetNormalized();
        A = versor.X;
        B = versor.Y;
        C = versor.Z;
        D = -(center.X * A + center.Y * B + center.Z * C);
    }


    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="a">A</param>
    ///     <param name="b">B</param>
    ///     <param name="c">C</param>
    ///     <param name="d">D</param>
    /// </summary>
    public Plane3D(double a, double b, double c, double d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    /// <summary>
    /// Oblicza prostą przecięcia dwóch płaszczyzn. Zwraca <c>null</c>,
    /// jeśli płaszczyzny są równoległe.
    /// </summary>
    /// <param name="a">Pierwsza płaszczyzna.</param>
    /// <param name="b">Druga płaszczyzna.</param>
    /// <returns>Prosta przecięcia lub <c>null</c>.</returns>
    public static Line3D? Cross(Plane3D a, Plane3D b)
    {
        var v1 = a.Normal;
        var v2 = b.Normal;

        var n = Vector3D.CrossProduct(v1, v2);
        n = n.GetNormalized();

        var     x = Math.Abs(n.X);
        var     y = Math.Abs(n.Y);
        var     z = Math.Abs(n.Z);
        Point3D p;
        if (z >= x && z >= y)
        {
            var l1         = new LineEquation(a.A, a.B, a.D);
            var l2         = new LineEquation(b.A, b.B, b.D);
            var ppNullable = LineEquation.Cross(l1, l2);
            if (!ppNullable.HasValue)
                return null;
            var pp = ppNullable.Value;
            p = new Point3D(pp.X, pp.Y, 0);
        }
        else if (y >= x)
        {
            var l1         = new LineEquation(a.A, a.C, a.D);
            var l2         = new LineEquation(b.A, b.C, b.D);
            var ppNullable = LineEquation.Cross(l1, l2);
            if (!ppNullable.HasValue)
                return null;
            var pp = ppNullable.Value;
            p = new Point3D(pp.X, 0, pp.Y);
        }
        else
        {
            var l1         = new LineEquation(a.B, a.C, a.D);
            var l2         = new LineEquation(b.B, b.C, b.D);
            var ppNullable = LineEquation.Cross(l1, l2);
            if (!ppNullable.HasValue)
                return null;
            var pp = ppNullable.Value;
            p = new Point3D(0, pp.X, pp.Y);
        }

        p = a.Throw(p);
        p = b.Throw(p);
        return new Line3D(p, n);
    }
    /// <summary>
    /// Dzieli płaszczyznę przez współrzędne (dzielenie przez transformację).
    /// Jeśli <paramref name="coordinates"/> jest <c>null</c>, zwraca kopię płaszczyzny.
    /// </summary>
    /// <param name="srcPlane">Płaszczyzna źródłowa.</param>
    /// <param name="coordinates">Transformacja współrzędnych.</param>
    /// <returns>Nowa płaszczyzna po podzieleniu.</returns>
    public static Plane3D operator /(Plane3D srcPlane, Coordinates3D? coordinates)
    {
        return coordinates is null
            ? (Plane3D)srcPlane.MemberwiseClone()
            : srcPlane * coordinates.Reversed;
    }

    /// <summary>
    /// Konwertuje współrzędne 3‑D na płaszczyznę, używając punktu początkowego jako środka
    /// i wektora <c>Z</c> jako wektora normalnego.
    /// </summary>
    /// <param name="src">Współrzędne 3‑D.</param>
    /// <returns>Nowa płaszczyzna.</returns>
    public static explicit operator Plane3D(Coordinates3D src)
    {
        return new Plane3D(src.Origin, src.Z);
    }

    /// <summary>
    /// Mnoży płaszczyznę przez transformację współrzędnych (skalowanie/rotacja/przesunięcie).
    /// Jeśli <paramref name="coordinates"/> jest <c>null</c>, zwraca kopię płaszczyzny.
    /// </summary>
    /// <param name="srcPlane">Płaszczyzna źródłowa.</param>
    /// <param name="coordinates">Transformacja współrzędnych.</param>
    /// <returns>Nowa płaszczyzna po przemnożeniu.</returns>
    public static Plane3D operator *(Plane3D srcPlane, Coordinates3D? coordinates)
    {
        if (coordinates is null)
            return (Plane3D)srcPlane.MemberwiseClone();
        var normal      = new Vector3D(srcPlane.A, srcPlane.B, srcPlane.C);
        var dNormalized = srcPlane.D / normal.Length;
        normal = normal.GetNormalized();
        var newNormal = normal * coordinates;

        // var center = new Point3D(-dNormalized * normal.X, -dNormalized * normal.Y, -dNormalized * normal.Z);
        // var newCenter = center * coordinates;
        //  (-dNormalized *  normal) * coordinates =  -dNormalized * newCenter -> trzeba tylko dodać origin bo tym się
        // różni mnożenie punktu od mnożenia wektora
        // normal * coordinates
        var newCenter2 = -dNormalized * newNormal + coordinates.Origin;
        return new Plane3D(newCenter2, newNormal);
    }

    /// <summary>
    /// Tworzy płytką kopię bieżącej płaszczyzny.
    /// </summary>
    /// <returns>Kopia płaszczyzny.</returns>
    public object Clone()
    {
        return MemberwiseClone();
    }

    /// <summary>
    /// Oblicza punkt przecięcia płaszczyzny z podaną prostą.
    /// </summary>
    /// <param name="l">Prosta, z którą ma być obliczone przecięcie.</param>
    /// <returns>Punkt przecięcia lub <c>null</c>, jeśli prosta jest równoległa.</returns>
    public Point3D? Cross(Line3D l)
    {
        return Cross(l.Point, l.Vector);
    }

    /// <summary>
    /// Oblicza punkt przecięcia płaszczyzny z prostą określoną punktem i wektorem.
    /// </summary>
    /// <param name="src">Punkt początkowy prostej.</param>
    /// <param name="v">Wektor kierunkowy prostej.</param>
    /// <returns>Punkt przecięcia lub <c>null</c>, jeśli prosta jest równoległa.</returns>
    public Point3D? Cross(Point3D src, Vector3D v)
    {
        var a = A * v.X + B * v.Y + C * v.Z;
        var b = A * src.X + B * src.Y + C * src.Z + D;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (a == 0)
            return null;
        var l  = -b / a;
        var p2 = src + v * l;
        return p2;
    }

    /// <summary>
    /// Zwraca nieznormalizowaną odległość (wartość równania płaszczyzny) od punktu <paramref name="p"/>.
    /// </summary>
    /// <param name="p">Punkt, dla którego obliczana jest odległość.</param>
    /// <returns>Wartość równania płaszczyzny w punkcie.</returns>
    public double DistanceNotNormalized(Point3D p)
    {
        return p.X * A + p.Y * B + p.Z * C + D;
    }

    /// <summary>
    /// Normalizuje współczynniki płaszczyzny tak, aby wektor normalny miał długość 1.
    /// </summary>
    public void Normalize()
    {
        var l = Math.Sqrt(A * A + B * B + C * C);
        A /= l;
        B /= l;
        C /= l;
        D /= l;
    }

    /// <summary>
    /// Rzuca punkt <paramref name="p"/> na płaszczyznę (projekcja ortogonalna).
    /// </summary>
    /// <param name="p">Punkt do rzutowania.</param>
    /// <returns>Punkt leżący na płaszczyźnie.</returns>
    public Point3D Throw(Point3D p)
    {
        var q = -(A * p.X + B * p.Y + C * p.Z + D) / (A * A + B * B + C * C);
        return new Point3D(p.X + A * q, p.Y + B * q, p.Z + C * q);
    }

    /// <summary>
    /// Płaszczyzna XY (Z = 0) – płaszczyzna równoległa do osi X i Y.
    /// </summary>
    public static Plane3D ZPlane
    {
        get { return new Plane3D(new Point3D(), new Vector3D(0, 0, 1)); }
    }

    /// <summary>
    /// Współczynnik <c>A</c> równania płaszczyzny <c>Ax + By + Cz + D = 0</c>.
    /// </summary>
    public double A { get; set; }

    /// <summary>
    /// Współczynnik <c>B</c> równania płaszczyzny <c>Ax + By + Cz + D = 0</c>.
    /// </summary>
    public double B { get; set; }

    /// <summary>
    /// Współczynnik <c>C</c> równania płaszczyzny <c>Ax + By + Cz + D = 0</c>.
    /// </summary>
    public double C { get; set; }

    /// <summary>
    /// Współczynnik <c>D</c> równania płaszczyzny <c>Ax + By + Cz + D = 0</c>.
    /// </summary>
    public double D { get; set; }

    /// <summary>
    /// Wektor normalny płaszczyzny, czyli <c>(A, B, C)</c>. Jest to właściwość tylko do odczytu.
    /// </summary>
    public Vector3D Normal
    {
        get { return new Vector3D(A, B, C); }
    }
}
