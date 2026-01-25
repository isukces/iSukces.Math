using System;
using System.Numerics;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class MethodOverrideMethodsTests
{
    [Theory]
    [MemberData(nameof(TypesToCheck))]
    public void T01_Should_override_ToString(Type type)
    {
        var method = type.GetMethod("ToString", []);
        Assert.NotNull(method);
        Assert.True(method.IsVirtual);
        Assert.Equal(type, method.DeclaringType);
    }

    [Theory]
    [MemberData(nameof(TypesToCheck))]
    public void T02_Should_override_equals_object(Type type)
    {
        var method = type.GetMethod("Equals", [typeof(object)]);
        Assert.NotNull(method);
        Assert.True(method.IsVirtual);
        Assert.Equal(type, method.DeclaringType);
    }


    [Theory]
    [MemberData(nameof(TypesToCheck))]
    public void T03_Should_override_equals(Type type)
    {
        var method = type.GetMethod("Equals", [type]);
        Assert.NotNull(method);
        Assert.True(method.IsVirtual);
        Assert.Equal(type, method.DeclaringType);
    }


    [Theory]
    [MemberData(nameof(TypesToCheck))]
    public void T04_Should_implement_IEquatable(Type type)
    {
        var expected = typeof(IEquatable<>).MakeGenericType(type);
        Assert.True(expected.IsAssignableFrom(type));
    }

    #region Properties

    public static TheoryData<Type>
        TypesToCheck { get; } = new()
    {
        typeof(Size),
        typeof(Size3D),
        typeof(Rect),
        typeof(Rect3D),
        typeof(Point),
        typeof(Point3D),
        typeof(Vector),
        typeof(Vector3),
        typeof(Matrix3x2),
        typeof(Matrix3D),
        typeof(Matrix4x4),
        typeof(ExMatrix3D)
    };

    #endregion
}
