using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSukces.Mathematics;
using Xunit;
#if !WPFFEATURES
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint=System.Windows.Point;
using TheVector=System.Windows.Vector;
#endif


namespace iSukces.Mathematics.test
{
    public class Program
    {
        [Fact(DisplayName = "COO")]
        public static void test1()
        {
            CircleOnPlane c = new CircleOnPlane(100, new ThePoint(100, 10));
            var p1 = new ThePoint(0, 50);
            var p2 = new ThePoint(10, 50);
            var g = c.CrossLine(p1, p2);
            Assert.Equal(g.Length, 2);
            Assert.Equal((g[0] - c.Center).Length, c.Radius, 10);
            Assert.Equal((g[1] - c.Center).Length, c.Radius, 10);
        }

        
        /*
        static void Main(string[] args)
        {
#if WPFFEATURES
            TypeConverterTest.All();
#endif

            IsometricTransformTest.ShouldEqual();
            IsometricTransformTest.ShouldMakeValidMatrix();
            test1();
            Console.ReadLine();
        } 
        */ 
    }
}
