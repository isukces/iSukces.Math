
namespace iSukces.Mathematics;

public static class Size3DExtension
{
    extension(Size3D size)
    {
        public bool HasVolume()
        {
            return !size.IsEmpty && size.X > 0 && size.Y > 0 && size.Z > 0;
        }
    }

}