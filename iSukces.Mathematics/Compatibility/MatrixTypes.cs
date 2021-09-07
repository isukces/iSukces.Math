#if !ALLFEATURES
using System;

namespace iSukces.Mathematics.Compatibility
{
    [Flags]
    internal enum MatrixTypes
    {
        Identity = 0,
        Translation = 1,
        Scaling = 2,
        Unknown = 4
    }
}
#endif