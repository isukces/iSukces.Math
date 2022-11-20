using System.Collections.Generic;
using iSukces.Mathematics.Compatibility;
#if !WPFFEATURES
#else
using System.Windows;
#endif

namespace iSukces.Mathematics
{
    public sealed class RectTopology : object
    {
        public RectTopology()
        {
        }

        public RectTopology(IEnumerable<Rect> src)
        {
            Items.AddRange(src);
        }

        public bool HasAnyElement
        {
            get { return Items != null && Items.Count > 0; }
        }

        /// <summary>
        ///     Elementy prostokątne
        /// </summary>
        public List<Rect> Items { get; set; } = new List<Rect>();

        public static RectTopology operator -(RectTopology left, RectTopology right)
        {
            if (left == null) return null;
            return left.Substract(right);
        }

        public static explicit operator RectTopology(Rect src)
        {
            var r = new RectTopology();
            r.Items.Add(src);
            return r;
        }

        private static void ForceOrder(ref double a, ref double b)
        {
            if (a > b)
            {
                var tmp = a;
                a = b;
                b = tmp;
            }
        }

        private static IEnumerable<Point> GetRanges(double a1, double a2, double b1, double b2)
        {
            var p = new List<Point>();
            if (a1 < b1) p.Add(new Point(a1, System.Math.Min(a2, b1)));
            if (b2 < a2) p.Add(new Point(System.Math.Max(b2, a1), a2));
            return p;
        }

        private static void Substract(Rect a, Rect b, RectTopology output)
        {
            if (a.Left >= b.Right || b.Left >= a.Right || a.Top >= b.Bottom || b.Top >= a.Bottom)
            {
                output.Items.Add(a);
                return;
            }

            foreach (var p in GetRanges(a.Left, a.Right, b.Left, b.Right))
                output.Add(p.X, a.Top, p.Y, a.Bottom);
            var x1 = System.Math.Max(a.Left, b.Left);
            var x2 = System.Math.Min(a.Right, b.Right);
            foreach (var p in GetRanges(a.Top, a.Bottom, b.Top, b.Bottom))
                output.Add(x1, p.X, x2, p.Y);
        }

        public Rect Add(double x1, double y1, double x2, double y2)
        {
            ForceOrder(ref x1, ref x2);
            ForceOrder(ref y1, ref y2);
            var r = new Rect(x1, y1, x2 - x1, y2 - y1);
            Items.Add(r);
            return r;
        }

        public RectTopology Substract(Rect rectangle)
        {
            var output = new RectTopology();
            foreach (var x in Items)
                Substract(x, rectangle, output);
            return output;
        }

        public RectTopology Substract(RectTopology x)
        {
            if (x == null || !x.HasAnyElement) return this;

            var src = this;
            var r   = new RectTopology();
            foreach (var r2 in x.Items)
            {
                r   = src.Substract(r2);
                src = r;
            }

            return r;
        }
    }
}