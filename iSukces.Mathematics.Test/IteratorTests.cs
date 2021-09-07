using System;
using Xunit;

namespace iSukces.Mathematics.test
{
    public class IteratorTests
    {
        [Fact]
        public void T01_Should_solve()
        {
            var s = Iterator.Solve(x => 2 * x + 1, -10, 10,
                (iteration, result) => { return iteration > 20 || Math.Abs(result) < 1e-8; });

            Assert.Equal(-0.5, s.Value, 8);
        }

        [Fact]
        public void T02_Should_solve()
        {
            var s = Iterator.Solve(x => (x+10) * (x-3), -20, 0,
                (iteration, result) => { return iteration > 20 || Math.Abs(result) < 1e-8; });

            Assert.Equal(-10, s.Value, 5);
        }
        
        
        [Fact]
        public void T03_Should_solve()
        {
            var s = Iterator.Solve(x =>
                {
                    var xx        = 60 * MathEx.TanDeg(x);
                    var przekatna = 132 + xx;
                    return 125 - przekatna * MathEx.CosDeg(x);
                }, 0, 90,
                (iteration, result) => { return iteration > 100 || Math.Abs(result) < 1e-8; });

            Assert.Equal(54.891950867239046, s.Value, 5);
        }

        private const double epsilon = 1e-3;
        [Fact]
        public void T04_Should_solve()
        {
            TestTuple Calc(double angle)
            {
                const double plyta  = 125;
                var          tan    = MathEx.TanDeg(angle);
                var          cosDeg = MathEx.CosDeg(angle);
                var          b1     = plyta * tan;
                var          bok1   = (plyta - b1) / cosDeg;

                var skos   = plyta / cosDeg;
                var x      = skos - 132;
                var bok2   = x / tan;
                return new TestTuple(bok1, bok2);
                
            }
            var s = Iterator.Solve(angle =>
                {
                    var          tmp   = Calc(angle);
                    return tmp.B1 - 31;
                }, epsilon, 45-epsilon,
                (iteration, result) => { return iteration > 100 || Math.Abs(result) < 1e-8; });
            var oba = Calc(s.Value);
            Assert.Equal(38.903732523383034, s.Value, 5);
        }
        #if NOTREADY
        [Fact]
        public void T04_Should_solve()
        {
            var s = Iterator.FindMax(angle =>
                {
                    var          cos   = MathEx.CosDeg(angle);
                    var          b     = 31 * cos;
                    const double plyta = 125;

                    var b1   = plyta - b;
                    var skos = MathEx.PitagorasC(b1, plyta);
                    var x    = skos - 132;

                    var tan2 = b1 / plyta;
                    
                    return x / tan2;
                },
                0.1, 89.9, 10);

            Assert.Equal(54.891950867239046, s, 5);
        }
        #endif
    }
    
    public class TestTuple
    {
        public TestTuple(double b1, double b2)
        {
            B1 = b1;
            B2 = b2;
        }

        public override string ToString()
        {
            return $"B1={B1}, B2={B2}";
        }

        public double B1 { get; }

        public double B2 { get; }

    }

}