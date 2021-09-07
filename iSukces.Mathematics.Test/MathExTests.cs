using System;
using iSukces.Mathematics;
using Xunit;
using Xunit.Abstractions;

namespace iSukces.Mathematics.test
{
    public class MathExTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public MathExTests(ITestOutputHelper testOutputHelper) { _testOutputHelper = testOutputHelper; }

        [Fact]
        public void ShouldNormalizeAngle()
        {
            for (var i = -15; i <= 15; i++)
            {
                var plus = i * 360;

                void Test(double baseAngle)
                {
                    var got = MathEx.NormalizeAngleDeg(baseAngle + plus);
                    _testOutputHelper.WriteLine(baseAngle + plus + " => " + got);
                    Assert.Equal(baseAngle, got);
                }

                Test(0);
                // Test(1e-3);
                Test(90);
                Test(180);
                Test(270);
                // Test(360-1e-3);
            }
        }
    }
}