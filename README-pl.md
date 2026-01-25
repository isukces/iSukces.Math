# iSukces.Math

[English version](README.md)

Biblioteka .NET z pomocniczymi klasami do geometrii 2D/3D oraz narzedziami matematycznymi. Wspiera net6, net8, net9, net10 (takze warianty Windows) i jest wydana na licencji MIT.

## Informacje o wersji 2.x.x.x

Biblioteka została skonsolidowana do jednej wersji i zawiera własne, lekkie implementacje podstawowych typów matematycznych i geometrycznych, takich jak Point, Vector, Matrix, Rect oraz Size. Zmiana ta usuwa zależność od typów specyficznych dla systemu Windows i Windows Media, poprawiając przenośność oraz spójność między platformami. Część implementacji została oparta na otwartym kodzie źródłowym firmy Microsoft udostępnionym na licencji MIT.

Przy implementacji typów geometrycznych w większości zastosowano podejście niemutowalne, co może wymagać zmian w kodzie — na przykład zastąpienia 
`vector.Normalize()` przez `vector = vector.GetNormalized()`. W związku z tym migracja z wersji 1.x.x.x do 2.x.x.x wymaga istotnych zmian w istniejącym kodzie.


## Funkcje
- Uklady wspolrzednych 2D/3D z obrotami, przesunieciami i latwa inwersja (`Coordinates3D.Reversed`).
- Linie, plaszczyzny, okregi i trojkaty z funkcjami odleglosci oraz przeciec (`LineEquation`, `Plane3D` i pokrewne typy).
- Obsluga katow, zakresow i pomocnicze funkcje trygonometryczne (`AngleInfo`, `AngleRange`, `MathEx`, `SinusCosinus`).
- Zakresy min/max wraz z laczeniem i kompaktowaniem (`MinMax`, `MinMaxI`, `MinMaxGeneric`).
- Dodatkowe narzedzia, np. srednie wazone i iteratory liczbowe.
- Opcjonalne konwertery typow oraz warstwa zgodnosci z WPF (namespace `Compatibility`) przydatne przy serializacji i wiazaniach UI.

## Szybki start
- Instalacja z NuGet:
  - `dotnet add package iSukces.Math`
- Budowanie ze zrodel:
  - `dotnet build iSukces.Math.sln`
- Testy:
  - `dotnet test iSukces.Math.sln`

## Przyklad
```csharp
using iSukces.Mathematics;
using iSukces.Mathematics.Compatibility;

var linia = LineEquation.Horizontal(3);
var odleglosc = linia.DistanceNotNormalized(2, 10); // 0

var uklad = new Coordinates3D(
    new Vector3D(1, 0, 0),
    new Vector3D(0, 1, 0),
    new Point3D(0, 0, 0));
var odwrocony = uklad.Reversed;
```

## Repozytorium
- Licencja: MIT (`LICENSE`).
- Zrodlo: https://github.com/isukces/iSukces.Math
