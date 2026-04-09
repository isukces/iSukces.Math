using System;
using Xunit;

namespace iSukces.Mathematics.Test.Compatibility;

public sealed class MatrixTests
{
    [Fact]
    public void T01_Identity_should_have_correct_properties()
    {
        var m = Matrix.Identity;

        Assert.True(m.IsIdentity);
        Assert.Equal(1, m.Determinant);
        Assert.True(m.HasInverse);
        Assert.Equal(1, m.M11);
        Assert.Equal(1, m.M22);
        Assert.Equal(0, m.OffsetX);
        Assert.Equal(0, m.OffsetY);
    }

    [Fact]
    public void T02_Multiply_operator_should_compose_transforms_correctly()
    {
        var scale = new Matrix(2, 0, 0, 3, 0, 0);
        var translation = Matrix.Identity.GetTranslated(5, 7);

        var composed = scale * translation;
        var got = composed.Transform(new Point(1, 1));

        Assert.Equal(new Point(7, 10), got);
    }

    [Fact]
    public void T03_GetTranslated_and_GetTranslatePrepend_should_have_different_order()
    {
        var m = new Matrix(2, 0, 0, 3, 0, 0);
        var p = new Point(1, 1);

        var appended = m.GetTranslated(5, 7).Transform(p);
        var prepended = m.GetTranslatePrepend(5, 7).Transform(p);

        Assert.Equal(new Point(7, 10), appended);
        Assert.Equal(new Point(12, 24), prepended);
    }

    [Fact]
    public void T04_Vector_transform_should_not_use_translation()
    {
        var m = new Matrix(1, 0, 0, 1, 10, 20);

        var point = m.Transform(new Point(3, 4));
        var vector = m.Transform(new Vector(3, 4));

        Assert.Equal(new Point(13, 24), point);
        Assert.Equal(new Vector(3, 4), vector);
    }

    [Fact]
    public void T05_GetRotated_should_rotate_by_90_degrees()
    {
        var m = Matrix.Identity.GetRotated(90);

        var p = m.Transform(new Point(2, 0));
        Assert.Equal(0, p.X, 12);
        Assert.Equal(2, p.Y, 12);
    }

    [Fact]
    public void T06_GetRotatedAt_should_keep_rotation_center()
    {
        var m   = Matrix.Identity.GetRotatedAt(90, 5, -3);
        var got = m.Transform(new Point(6, -3));
        Assert.Equal(5, got.X, 12);
        Assert.Equal(-2, got.Y, 12);
    }

    [Fact]
    public void T07_GetScaledAt_should_keep_scaling_center()
    {
        var m   = Matrix.Identity.GetScaledAt(2, 3, 2, 3);
        var got = m.Transform(new Point(4, 5));
        Assert.Equal(6, got.X, 12);
        Assert.Equal(9, got.Y, 12);
    }

    [Fact]
    public void T08_GetSkrewd_should_return_expected_result_for_vector()
    {
        var m = Matrix.Identity.GetSkrewd(45, 0);
        var v = m.Transform(new Vector(2, 3));

        Assert.Equal(5, v.X, 12);
        Assert.Equal(3, v.Y, 12);
    }

    [Fact]
    public void T09_GetInverted_should_work_for_translation()
    {
        var m = Matrix.Identity.GetTranslated(7, -4);
        var inverse = m.GetInverted();

        var source = new Point(3, 9);
        var transformed = m.Transform(source);
        var roundTrip = inverse.Transform(transformed);

        Assert.Equal(source.X, roundTrip.X, 12);
        Assert.Equal(source.Y, roundTrip.Y, 12);
    }

    [Fact]
    public void T10_GetInverted_should_throw_for_singular_matrix()
    {
        var m = new Matrix(1, 2, 2, 4, 0, 0);
        Assert.False(m.HasInverse);
        Assert.Throws<InvalidOperationException>(() => m.GetInverted());
    }

    [Fact]
    public void T11_Array_transform_should_work_and_handle_null()
    {
        var m = Matrix.Identity.GetTranslated(1, 2);

        Point[]? pointsNull = null;
        m.Transform(pointsNull);

        Vector[]? vectorsNull = null;
        m.Transform(vectorsNull);

        var points = new[] { new Point(0, 0), new Point(2, 3) };
        m.Transform(points);
        Assert.Equal(new Point(1, 2), points[0]);
        Assert.Equal(new Point(3, 5), points[1]);

        var vectors = new[] { new Vector(1, 2), new Vector(-4, 6) };
        m.Transform(vectors);
        Assert.Equal(new Vector(1, 2), vectors[0]);
        Assert.Equal(new Vector(-4, 6), vectors[1]);
    }

    [Fact]
    public void T12_WithOffset_should_change_only_offset()
    {
        var m = new Matrix(2, 1, -3, 4, 5, 6);

        var got = m.WithOffset(new Point(10, 20));

        Assert.Equal(m.M11, got.M11);
        Assert.Equal(m.M12, got.M12);
        Assert.Equal(m.M21, got.M21);
        Assert.Equal(m.M22, got.M22);
        Assert.Equal(10, got.OffsetX);
        Assert.Equal(20, got.OffsetY);
    }

    [Fact]
    public void T13_GetRotatedAtPrepend_should_prepend_rotation()
    {
        var m = new Matrix(2, 0, 0, 3, 0, 0);
        var p = new Point(1, 2);

        var got = m.GetRotatedAtPrepend(90, 0, 0).Transform(p);

        Assert.Equal(-4, got.X, 12);
        Assert.Equal(3, got.Y, 12);
    }

    [Fact]
    public void T14_GetRotatedPrepend_should_prepend_rotation()
    {
        var m = Matrix.Identity.GetTranslated(10, 20);
        var p = new Point(1, 2);

        var got = m.GetRotatedPrepend(90).Transform(p);

        Assert.Equal(8, got.X, 12);
        Assert.Equal(21, got.Y, 12);
    }

    [Fact]
    public void T15_GetScaled_and_GetScaledPrepend_should_have_different_order()
    {
        var m = Matrix.Identity.GetTranslated(1, 2);
        var p = new Point(3, 4);

        var appended = m.GetScaled(2, 3).Transform(p);
        var prepended = m.GetScaledPrepend(2, 3).Transform(p);

        Assert.Equal(new Point(8, 18), appended);
        Assert.Equal(new Point(7, 14), prepended);
    }

    [Fact]
    public void T16_GetScaledAtPrepend_should_prepend_scaling_at_center()
    {
        var m = Matrix.Identity.GetTranslated(10, 20);
        var p = new Point(2, 4);

        var got = m.GetScaledAtPrepend(2, 3, 1, 1).Transform(p);

        Assert.Equal(13, got.X, 12);
        Assert.Equal(30, got.Y, 12);
    }

    [Fact]
    public void T17_GetSkrewdPrepend_should_prepend_skew()
    {
        var m = new Matrix(2, 0, 0, 3, 0, 0);
        var v = new Vector(2, 3);

        var got = m.GetSkrewdPrepend(45, 0).Transform(v);

        Assert.Equal(10, got.X, 12);
        Assert.Equal(9, got.Y, 12);
    }

    [Fact]
    public void T18_Multiply_method_should_work_for_identity_case()
    {
        var m1 = Matrix.Identity;
        var m2 = Matrix.Identity.GetTranslated(5, 7);

        var got = Matrix.Multiply(m1, m2);

        Assert.Equal(m2.M11, got.M11);
        Assert.Equal(m2.M12, got.M12);
        Assert.Equal(m2.M21, got.M21);
        Assert.Equal(m2.M22, got.M22);
        Assert.Equal(m2.OffsetX, got.OffsetX);
        Assert.Equal(m2.OffsetY, got.OffsetY);
    }

    [Fact]
    public void T19_Multiply_method_should_match_manual_calculation_for_general_case()
    {
        var m1 = new Matrix(2, 3, 5, 7, 11, 13);
        var m2 = new Matrix(17, 19, 23, 29, 31, 37);

        var got = Matrix.Multiply(m1, m2);

        Assert.Equal(103, got.M11, 12);
        Assert.Equal(125, got.M12, 12);
        Assert.Equal(246, got.M21, 12);
        Assert.Equal(298, got.M22, 12);
        Assert.Equal(517, got.OffsetX, 12);
        Assert.Equal(623, got.OffsetY, 12);
    }

    [Fact]
    public void T20_GetInverted_for_scale_plus_translation_should_match_manual_result()
    {
        var m   = new Matrix(2, 0, 0, 4, 5, 7);
        var inv = m.GetInverted();
        Assert.Equal(0.5, inv.M11, 12);
        Assert.Equal(0, inv.M12, 12);
        Assert.Equal(0, inv.M21, 12);
        Assert.Equal(0.25, inv.M22, 12);
        Assert.Equal(-2.5, inv.OffsetX, 12);
        Assert.Equal(-1.75, inv.OffsetY, 12);
    }

    [Fact]
    public void T21_Multiply_should_match_hardcoded_general_case_result()
    {
        var m1 = new Matrix(2, 3, 5, 7, 11, 13);
        var m2 = new Matrix(17, 19, 23, 29, 31, 37);

        var got = Matrix.Multiply(m1, m2);

        Assert.Equal(103, got.M11, 12);
        Assert.Equal(125, got.M12, 12);
        Assert.Equal(246, got.M21, 12);
        Assert.Equal(298, got.M22, 12);
        Assert.Equal(517, got.OffsetX, 12);
        Assert.Equal(623, got.OffsetY, 12);
    }

    [Fact]
    public void T22_GetInverted_should_match_hardcoded_scaling_translation_result()
    {
        var m = new Matrix(2, 0, 0, 4, 5, 7);
        var got = m.GetInverted();
        Assert.Equal(0.5, got.M11, 12);
        Assert.Equal(0, got.M12, 12);
        Assert.Equal(0, got.M21, 12);
        Assert.Equal(0.25, got.M22, 12);
        Assert.Equal(-2.5, got.OffsetX, 12);
        Assert.Equal(-1.75, got.OffsetY, 12);
    }

    [Fact]
    public void T23_GetInverted_should_match_hardcoded_general_matrix_result()
    {
        var m = new Matrix(2, 3, 5, 7, 11, 13);
        var got = m.GetInverted();
        Assert.Equal(-7, got.M11, 12);
        Assert.Equal(3, got.M12, 12);
        Assert.Equal(5, got.M21, 12);
        Assert.Equal(-2, got.M22, 12);
        Assert.Equal(12, got.OffsetX, 12);
        Assert.Equal(-7, got.OffsetY, 12);
    }

    [Fact]
    public void T24_GetInverted_general_matrix_should_round_trip_point_and_vector()
    {
        var m = new Matrix(2, 3, 5, 7, 11, 13);
        var inv = m.GetInverted();

        var pSource = new Point(17, 19);
        var pRound = inv.Transform(m.Transform(pSource));
        Assert.Equal(pSource.X, pRound.X, 12);
        Assert.Equal(pSource.Y, pRound.Y, 12);

        var vSource = new Vector(23, 29);
        var vRound = inv.Transform(m.Transform(vSource));
        Assert.Equal(vSource.X, vRound.X, 12);
        Assert.Equal(vSource.Y, vRound.Y, 12);
    }
}
