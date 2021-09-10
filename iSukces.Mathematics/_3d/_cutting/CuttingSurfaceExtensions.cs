namespace iSukces.Mathematics
{
    public static class CuttingSurfaceExtensions
    {
        public static ICuttingSurface Coalesce(this ICuttingSurface value)
        {
            return value ?? NullCuttingSurface.Instance;
        }

        public static bool IsNullOrEmpty(this ICuttingSurface x)
        {
            return x == null || x is NullCuttingSurface;
        }
    }
}
