using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSukces.Mathematics;
using Xunit;
#if !WPFFEATURES
using ThePoint=iSukces.Mathematics.Point;
using TheVector=iSukces.Mathematics.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics.test;

public class Test1
{

    [Fact]
    public static void Area()
    {
        var p = new ThePoint[]
        {
            new ThePoint(1,1),
            new ThePoint(3,1),
            new ThePoint(1,2)
        };
        var r = p.GetArea();
        Assert.Equal(r, 1.0, 6);

        p = new ThePoint[]
        {
            new ThePoint(1,1),
            new ThePoint(3,1),
            new ThePoint(3,2),
            new ThePoint(1,2)
        };
        r = p.GetArea();
        Assert.Equal(r, 2.0, 6);
    }


    static void t1(ThePoint p1, ThePoint p2, ThePoint p3, ThePoint p4, Action<ThePoint?> test)
    {
        var c = LineEquation.CrossLineSegment(p1, p2, p3, p4);
        test(c);
        c = LineEquation.CrossLineSegment(p2, p1, p3, p4);
        test(c);
        c = LineEquation.CrossLineSegment(p1, p2, p4, p3);
        test(c);
        c = LineEquation.CrossLineSegment(p2, p1, p4, p3);
        test(c);
    }

    [Fact]
    public static void TestCrossLineSegment()
    {
        t1(
            new ThePoint(0, 0), new ThePoint(100, 0),
            new ThePoint(0, 10), new ThePoint(100, 10),
            c => Assert.Null(c)
        );


        t1(
            new ThePoint(0, 10), new ThePoint(100, 10),
            new ThePoint(-1, 0), new ThePoint(-1, 20),
            c => Assert.Null(c)
        );

        t1(
            new ThePoint(5, 11), new ThePoint(5, 20),
            new ThePoint(0, 10), new ThePoint(100, 10),
            c => Assert.Null(c)
        );


        t1(
            new ThePoint(5, 9), new ThePoint(5, 20),
            new ThePoint(0, 10), new ThePoint(100, 10),
            c => Assert.Equal((c.Value - new ThePoint(5, 10)).Length, 0, 8)
        );

    }

    [Fact]
    public static void TestPlane()
    {
        var p  = new Plane3D(new Point3D(1, 3, 20), new Vector3D(1, -2, 2));
        var c  = Coordinates3D.FromXZO(new Vector3D(2, 1, 0), Coordinates3D.ZVersor);
        var p1 = p * c;
    }
}