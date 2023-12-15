using System.Collections.Generic;
using iSukces.Mathematics;
using Xunit;

namespace iSukces.Mathematics.test
{
    public class RangeITest
    {
        [Fact]
        public void T01_ShouldCutProperly()
        {
            var r010 = new RangeI(0, 10);
            var invalid = new RangeI(10, 0);
            var empty = RangeI.Empty;
            var result = empty.Cut(empty);
            Assert.Empty(result);

            result = r010.Cut(invalid);
            Assert.Single(result);
            Assert.Equal(r010, result[0]);

            result = r010.Cut(empty);
            Assert.Single(result);
            Assert.Equal(r010, result[0]);

            // wycięcie w środku
            result = r010.Cut(new RangeI(2, 5));
            Assert.Equal(2, result.Count);
            Assert.Equal(new RangeI(0, 2), result[0]);
            Assert.Equal(new RangeI(5, 10), result[1]);

            // obcięcie od dołu 2 przypadki
            result = r010.Cut(new RangeI(0, 5));
            Assert.Single(result);
            Assert.Equal(new RangeI(5, 10), result[0]);
            result = r010.Cut(new RangeI(-100, 5));
            Assert.Single(result);
            Assert.Equal(new RangeI(5, 10), result[0]);

            // obcięcie od góry 2 przypadki
            result = r010.Cut(new RangeI(5, 10));
            Assert.Single(result);
            Assert.Equal(new RangeI(0, 5), result[0]);
            result = r010.Cut(new RangeI(5, 99));
            Assert.Single(result);
            Assert.Equal(new RangeI(0, 5), result[0]);

            // całkowite wycięcie - 4 przypadki
            result = r010.Cut(new RangeI(0, 10));
            Assert.Empty(result);
            result = r010.Cut(new RangeI(-99, 10));
            Assert.Empty(result);
            result = r010.Cut(new RangeI(-99, 99));
            Assert.Empty(result);
            result = r010.Cut(new RangeI(0, 99));
            Assert.Empty(result);
        }

        [Fact]
        public void T02_ShouldHaveCommonArea()
        {
            var r010 = new RangeI(0, 10);
            var invalid = new RangeI(10, 0);
            var empty = RangeI.Empty;
            Assert.False(invalid.HasCommonAreaWith(invalid, true));
            Assert.False(invalid.HasCommonAreaWith(invalid, false));
            Assert.False(invalid.HasCommonAreaWith(empty, true));
            Assert.False(invalid.HasCommonAreaWith(empty, false));
            Assert.False(invalid.HasCommonAreaWith(r010, true));
            Assert.False(invalid.HasCommonAreaWith(r010, false));


            Assert.False(empty.HasCommonAreaWith(invalid, true));
            Assert.False(empty.HasCommonAreaWith(invalid, false));
            Assert.False(empty.HasCommonAreaWith(empty, true));
            Assert.False(empty.HasCommonAreaWith(empty, false));
            Assert.False(empty.HasCommonAreaWith(r010, true));
            Assert.False(empty.HasCommonAreaWith(r010, false));

            Assert.False(r010.HasCommonAreaWith(invalid, true));
            Assert.False(r010.HasCommonAreaWith(invalid, false));
            Assert.False(r010.HasCommonAreaWith(empty, true));
            Assert.False(r010.HasCommonAreaWith(empty, false));

            // jakieś przestrzenie
            var point = new RangeI(5, 5);
            Assert.False(point.IsEmpty);

            Assert.False(r010.HasCommonAreaWith(point, false));
            Assert.True(r010.HasCommonAreaWith(point, true));
        }

        [Fact]
        public void T03_ShouldCreate()
        {
            var point = new RangeI(5, 5);
            Assert.False(point.IsEmpty);
            Assert.True(point.IsZeroOnInvalid);
            Assert.True(point.IsZeroOnInvalid);
        }
    }
}