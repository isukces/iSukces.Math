namespace iSukces.Mathematics;
// http://www.efunda.com/math/areas/CircularSection.cfm
public enum CircleLocations
{
    /// <summary>
    ///     Bieżący okrąg jest we wnętrzu drugiego okręgu i nie mają punktów wspólnych
    /// </summary>
    InsideOtherCircle,

    /// <summary>
    ///     Bieżący okrąg jest we wnętrzu drugiego okręgu, 1 punkt styczny
    /// </summary>
    InsideOtherCircle1Point,


    /// <summary>
    ///     Drugi okrąg jest we wnętrzu i nie mają punktów wspólnych
    /// </summary>
    OtherCircleInside,

    /// <summary>
    ///     Drugi okrąg jest we wnętrzu, 1 punkt styczny
    /// </summary>
    OtherCircleInside1Point,


    /// <summary>
    ///     Okręgi są rozłączne
    /// </summary>
    NoCommonPoints,

    /// <summary>
    ///     Całkowicie się pokrywają
    /// </summary>
    Full,

    /// <summary>
    ///     Jeden punkt styczny, żaden z okręgów nie jest wewnątrz innego
    /// </summary>
    Outside1Point,

    /// <summary>
    ///     Pozostałe sytuacje
    /// </summary>
    Other
}
