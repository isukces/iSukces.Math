using System.Collections.Generic;
using iSukces.Mathematics.Compatibility;
using Xunit;
using Xunit.Abstractions;
#if !WPFFEATURES
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif


namespace iSukces.Mathematics.test;

class MyIsometricTranformator : IPointTransformator2D, IMatrixTransform2D
{
    private TheVector _x = new TheVector(1, 0);
    public ThePoint Origin { get; set; }

    public TheVector X
    {
        get { return _x; }
        set
        {
            if (value.LengthSquared > 0)
                value.Normalize();
            _x = value;
        }
    }

    public ThePoint Transform(ThePoint point)
    {
        var y = _x.GetPrependicular();
        return Origin + point.X * _x + point.Y * y;
    }


    public Matrix GetTransformMatrix()
    {
        var y = _x.GetPrependicular();
        return new Matrix(
            _x.X, _x.Y,
            y.X, y.Y,
            Origin.X, Origin.Y);
    }
}


public class IsometricTransformTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    public IsometricTransformTest(ITestOutputHelper testOutputHelper) { _testOutputHelper = testOutputHelper; }

    [Fact]
    public   void ShouldEqual()
    {
        IPointTransformator2D t = new MyIsometricTranformator();
        _testOutputHelper.WriteLine(string.Join("  ", GetTestPoints()));
        foreach (var p in GetTestPoints())
            Assert.Equal(p, t.Transform(p));
    }

    static IEnumerable<Point> GetTestPoints()
    {
        yield return new Point();
        for (var i = 0; i < 4; i++)
        {
            var xf = i == 0 || i == 2 ? 1 : -1;
            var yf = i < 2 ? 1 : -1;
            yield return new Point(7 * xf, 3 * yf);
            yield return new Point(1 * xf, 7 * yf);
        }
        for (var i = 0; i < 2; i++)
        {
            var f = i == 0 ? 1 : -1;
            foreach (var d in new[] { 1, 7, 13 })
            {
                yield return new Point(0, d * f);
                yield return new Point(d * f, 0);
            }
        }
    }

    [Fact]
    public static void ShouldMakeValidMatrix()
    {
        MyIsometricTranformator t = new MyIsometricTranformator
        {
            Origin = new Point(153, -17),
            X      = new Vector(4, 3)
        };
        var points = GetTestPoints();
        foreach (var p in points)
        {
            var p1 = t.Transform(p);
            var m  = t.GetTransformMatrix();
            var p2 = p * m;
            Assert.Equal(p1, p2);
        }
    }

}