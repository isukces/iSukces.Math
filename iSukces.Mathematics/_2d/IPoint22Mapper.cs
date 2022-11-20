#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif
namespace iSukces.Mathematics
{
    public interface IPoint22Mapper
    {
        Point MapPoint22(Point x);
    }
}