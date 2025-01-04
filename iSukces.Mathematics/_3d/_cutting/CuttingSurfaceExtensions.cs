#nullable disable
namespace iSukces.Mathematics;

public static class CuttingSurfaceExtensions
{
    public static ICuttingSurface Coalesce(this ICuttingSurface? value)
    {
        return value ?? NullCuttingSurface.Instance;
    }

    public static bool IsNullOrEmpty(this ICuttingSurface? x)
    {
        return x is null || x is NullCuttingSurface;
    }
}