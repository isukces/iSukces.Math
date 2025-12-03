# iSukces.Math

[Polish version](README-pl.md)

Small .NET library with helpers for 2D/3D geometry, coordinate systems, and assorted math utilities. Targets net6, net8, net9, net10 (including Windows variants) and is distributed under the MIT license.

## Features
- 2D/3D coordinate systems with rotation, translation, and easy inversion (`Coordinates3D.Reversed`).
- Line, plane, circle, and triangle primitives with distance and intersection helpers (`LineEquation`, `Plane3D`, and related types).
- Angle helpers, ranges, and trigonometric utilities (`AngleInfo`, `AngleRange`, `MathEx`, `SinusCosinus`).
- Bounding ranges with merging/compaction helpers (`MinMax`, `MinMaxI`, `MinMaxGeneric`).
- Extra utilities such as weighted averages and numeric iterators.
- Optional type converters and WPF compatibility wrappers (`Compatibility` namespace) useful for serialization and UI bindings.

## Getting started
- Install from NuGet:
  - `dotnet add package iSukces.Math`
- Build from source:
  - `dotnet build iSukces.Math.sln`
- Run tests:
  - `dotnet test iSukces.Math.sln`

## Example
```csharp
using iSukces.Mathematics;
using iSukces.Mathematics.Compatibility;

var horizontal = LineEquation.Horizontal(3);
var distance = horizontal.DistanceNotNormalized(2, 10); // 0

var basis = new Coordinates3D(
    new Vector3D(1, 0, 0),
    new Vector3D(0, 1, 0),
    new Point3D(0, 0, 0));
var inverse = basis.Reversed;
```

## Repository
- License: MIT (see `LICENSE`).
- Source: https://github.com/isukces/iSukces.Math
