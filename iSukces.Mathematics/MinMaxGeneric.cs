using System;

namespace iSukces.Mathematics;

public class MinMaxGeneric<T> where T : IComparable<T>
{
    public MinMaxGeneric(T min, T max)
    {
        Min = min;
        Max = max;
    }

    /// <summary>
    /// Koniec zakresu
    /// </summary>
    public T Max { get; set; }


    /// <summary>
    /// Początek zakresu
    /// </summary>
    public T Min { get; set; }
}