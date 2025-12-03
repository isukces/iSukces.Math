namespace iSukces.Mathematics;

public static class CuttingSurfaceExtensions
{
    extension(ICuttingSurface? value)
    {
        public ICuttingSurface Coalesce()
        {
            return value ?? NullCuttingSurface.Instance;
        }

        public bool IsNullOrEmpty()
        {
            return value is null or NullCuttingSurface;
        }
    }
}