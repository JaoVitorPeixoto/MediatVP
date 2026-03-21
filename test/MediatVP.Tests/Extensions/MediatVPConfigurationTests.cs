using MediatVP.Exceptions;
using MediatVP.Extensions;

namespace MediatVP.Tests.Extensions;

public class MediatVPConfigurationTests
{
    [Fact]
    public void RegisterServicesFromAssembly_WithNullAssembly_ShouldThrowArgumentNullException()
    {
        var config = new MediatVPConfiguration();

        Assert.Throws<ArgumentNullException>(() => config.RegisterServicesFromAssembly(null!));
    }

    [Fact]
    public void RegisterServicesFromAssemblies_WithNullAssemblies_ShouldThrowArgumentNullException()
    {
        var config = new MediatVPConfiguration();

        Assert.Throws<ArgumentNullException>(() => config.RegisterServicesFromAssemblies(null!));
    }

    [Fact]
    public void AddOpenBehavior_WithNullType_ShouldThrowArgumentNullException()
    {
        var config = new MediatVPConfiguration();

        Assert.Throws<ArgumentNullException>(() => config.AddOpenBehavior(null!));
    }

    [Fact]
    public void AddOpenBehavior_WithClosedGenericType_ShouldThrowClosedBehaviorException()
    {
        var config = new MediatVPConfiguration();

        Assert.Throws<ClosedBehaviorException>(() =>
            config.AddOpenBehavior(typeof(SampleClosedBehavior)));
    }

    [Fact]
    public void AddOpenBehavior_WithInvalidGenericArity_ShouldThrowArgumentException()
    {
        var config = new MediatVPConfiguration();

        var exception = Assert.Throws<ArgumentException>(() =>
            config.AddOpenBehavior(typeof(ThreeArgBehavior<,,>)));

        Assert.Equal("openBehaviorType", exception.ParamName);
    }

    [Fact]
    public void AddOpenBehavior_WithOpenGenericTypeWithTwoArgs_ShouldNotThrow()
    {
        var config = new MediatVPConfiguration();

        var exception = Record.Exception(() =>
            config.AddOpenBehavior(typeof(TwoArgBehavior<,>)));

        Assert.Null(exception);
    }

    private sealed class SampleClosedBehavior { }

    private sealed class TwoArgBehavior<TRequest, TResponse> { }

    private sealed class ThreeArgBehavior<T1, T2, T3> { }
}
