#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace iSukces.Mathematics
{
    /// <summary>
    ///     Reprezentuje przecięcie dwóch boków trójkąta
    /// </summary>
    public sealed class TopologySideCross : TopologyBase
    {
        public static explicit operator Point(TopologySideCross src)
        {
            return src.CrossPoint;
        }

        public override string ToString()
        {
            return string.Format("{0} / {1} / {2} {3}", CrossPoint, First, Second, CrossConfig);
        }

        public string CrossConfig
        {
            get { return (IsCrossVertexOfFirst ? "*" : ".") + (IsCrossVertexOfSecond ? "*" : "."); }
        }

        public Point CrossPoint { get; set; }

        public TopologyTriangleLine First { get; set; }

        public bool IsCrossVertexOfFirst
        {
            get { return First.IsVertex(CrossPoint); }
        }

        public bool IsCrossVertexOfSecond
        {
            get { return Second.IsVertex(CrossPoint); }
        }

        public TopologyTriangleLine Second { get; set; }
    }
}