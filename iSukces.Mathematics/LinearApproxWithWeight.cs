using System;

namespace iSukces.Mathematics
{
    public sealed class LinearApproxWithWeight
    {
        public void Add(double x, double y, double c)
        {
            _a1 += c * x * x;
            _b1 += c * x;
            _w1 += c * x * y;
            // a2 equals b1
            _b2 += c;
            _w2 += c * y;
            Count++;
            XMin = Math.Min(XMin, x);
            XMax = Math.Max(XMax, x);
        }

        public LinearFunc GetLine()
        {
            var s = new EquationSystem2
            {
                A1 = _a1,
                B1 = _b1,
                C1 = _w1,
                A2 = _b1,
                B2 = _b2,
                C2 = _w2
            };
            var d = s.Determinant;
            if (d == 0)
                return null;
            return new LinearFunc(-s.DeterminantX / d, -s.DeterminantY / d);
        }

        public override string ToString()
        {
            var a = GetLine();
            if (a == null)
                return "No solution";
            return a.ToString();
        }


        /// <summary>
        ///     ilość dodanych punktów
        /// </summary>
        public double Count { get; set; }

        /// <summary>
        ///     x min
        /// </summary>
        public double XMin { get; set; } = double.MaxValue;

        /// <summary>
        ///     x max
        /// </summary>
        public double XMax { get; set; } = double.MinValue;

        private double _a1;

        private double _b1;

        private double _b2;
        private double _w1;
        private double _w2;
    }
}
