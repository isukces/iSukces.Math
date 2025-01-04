#if WPFFEATURES
using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;

#else
using iSukces.Mathematics.TypeConverters;
using System.Windows;
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics.test
{
    public class TypeConverterTest
    {
        [Fact]
        public void T01_Should_Serialize_Range()
        {
            Action<Range, string> test = (range, s) =>
            {
                var json = JsonConvert.SerializeObject(range);
                Assert.Equal(Quote + s + Quote, json);
                var range2 = JsonConvert.DeserializeObject<Range>(json);
                Assert.Equal(range.Min, range2.Min);
                Assert.Equal(range.Max, range2.Max);
            };
            test(new Range(1, 3.3), "1,3.3");
            test(new Range(1, -3.3), "1,-3.3");
            test(Range.Empty, "Empty");
        }

        [Fact]
        public void T02_Should_Serialize_RangeI()
        {
            Action<RangeI, string> test = (range, s) =>
            {
                var json = JsonConvert.SerializeObject(range);
                Assert.Equal(Quote + s + Quote, json);
                var range2 = JsonConvert.DeserializeObject<RangeI>(json);
                Assert.Equal(range.Min, range2.Min);
                Assert.Equal(range.Max, range2.Max);
            };
            test(new RangeI(1, 3), "1,3");
            test(new RangeI(1, -3), "1,-3");
            test(RangeI.Empty, "Empty");
        }


        [Fact]
        public void T03_Should_Serialize_MinMax()
        {
            Action<MinMax, string> test = (range, s) =>
            {
                var json = JsonConvert.SerializeObject(range);
                Assert.Equal(Quote + s + Quote, json);
                var range2 = JsonConvert.DeserializeObject<MinMax>(json);
                Assert.Equal(range.Min, range2.Min);
                Assert.Equal(range.Max, range2.Max);
            };
            test(new MinMax(1, 3.3), "1,3.3");
            test(new MinMax(1, -3.3), "1,-3.3");
        }

        [Fact]
        public void T04_Should_Serialize_MinMaxI()
        {
            Action<MinMaxI, string> test = (range, s) =>
            {
                var json = JsonConvert.SerializeObject(range);
                Assert.Equal(Quote + s + Quote, json);
                var range2 = JsonConvert.DeserializeObject<MinMaxI>(json);
                Assert.Equal(range.Min, range2.Min);
                Assert.Equal(range.Max, range2.Max);
            };
            test(new MinMaxI(1, 3), "1,3");
            test(new MinMaxI(1, -3), "1,-3");

            var converted = new MinMaxITypeConverter()
                // ReSharper disable once AssignNullToNotNullAttribute
                .ConvertToString(null, Polish, new MinMaxI(1, 3));
            Assert.Equal("1;3", converted);
        }

        private static CultureInfo Polish => new CultureInfo("pl-pl");

        [Fact]
        public void T05_Should_Serialize_Coordinates3D()
        {
            Action<Coordinates3D, string> test = (range, s) =>
            {
                var json = JsonConvert.SerializeObject(range);
                Assert.Equal(Quote + s + Quote, json);
                var range2 = JsonConvert.DeserializeObject<Coordinates3D>(json);
                Assert.Equal(range.X, range2.X);
                var l = range2.Y - range.Y;
                Assert.True(l.Length < 1e-15);
                // Assert.Equal(range.Y, range2.Y);
                Assert.Equal(range.Origin, range2.Origin);
            };
            test(Coordinates3D.NORMAL, "Identity");
            test(new Coordinates3D(Coordinates3D.XVersor, Coordinates3D.ZVersor), "X,Z");
            test(new Coordinates3D(Coordinates3D.ZVersor, Coordinates3D.XVersor, new Point3D(1, 0, 0)), "Z,X,1,0,0");
            var expected1 = "0.7071067811865475,0.7071067811865475,0,-0.12309149097933272,0.12309149097933272,0.9847319278346618";
            test(new Coordinates3D(new Vector3D(1, 1, 0), new Vector3D(2, 3, 4)), expected1);
            var expected2 = "0.7071067811865475,0.7071067811865475,0,-0.12309149097933272,0.12309149097933272,0.9847319278346618,1,0,0";
            test(new Coordinates3D(new Vector3D(1, 1, 0), new Vector3D(2, 3, 4), new Point3D(1, 0, 0)), expected2);
        }

        [Fact]
        public void T06_CuluresWith()
        {
            RangeI i = RangeI.Empty;
            var seps = "";
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var culture in cultures)
            {
                var s = culture.TextInfo.ListSeparator;
                Assert.NotNull(s);
                Assert.Equal(1, s.Length);
                i = i.WithValue(culture.LCID);
                seps += s;
            }
            var seps2 = seps.Distinct().ToArray();
            //return list;
        }

        private const string Quote = "\"";

        public static void All()
        {
            new TypeConverterTest().T01_Should_Serialize_Range();
            new TypeConverterTest().T02_Should_Serialize_RangeI();
            new TypeConverterTest().T03_Should_Serialize_MinMax();
            new TypeConverterTest().T04_Should_Serialize_MinMaxI();
            new TypeConverterTest().T05_Should_Serialize_Coordinates3D();
            new TypeConverterTest().T06_CuluresWith();
        }
    }
}
#endif
