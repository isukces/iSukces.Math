// Copyright © Internet Sukces Piotr Stęclik 2017-2026. All rights reserved.
// Licensed under the MIT license.
//
// This file contains portions of code derived from the .NET Runtime (Microsoft Corporation).
// For more information, see the THIRD-PARTY-NOTICES.txt file in the project root.

using System;

namespace iSukces.Mathematics;

[Flags]
internal enum MatrixTypes
{
    TRANSFORM_IS_IDENTITY = 0,
    TRANSFORM_IS_TRANSLATION = 1,
    TRANSFORM_IS_SCALING = 2,
    TRANSFORM_IS_UNKNOWN = 4
}

internal enum MatrixTypes3
{
    IsIdentity = 0,
    IsAfiniteTranslation = 1,
    IsAfinite = 2,
    Projection = 3
}

internal enum AffinityMatrixTypes3
{
    IsIdentity = 0,
    IsAfiniteTranslation = 1,
    Other = 2
}
