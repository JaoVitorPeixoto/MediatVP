using MediatVP.Abstractions;
using MediatVP.Exceptions;
using MediatVP.Tests.Mediator.TestDoubles;
using MediatVP.Tests.TestDoubles.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MediatVP.Tests.Mediator;

public class SendTests
{
    [Fact]
    public async Task SendAsync_WithResponseCommand_ShouldReturnHandlerResponse()
    {
        var services = new ServiceCollection();
        services.AddSingleton<InvocationTracker>();
        services.AddTransient<IHandlerCommand<PingCommand, string>, PingHandler>();

        var mediator = new MediatVP.Mediator(services.BuildServiceProvider());

        var response = await mediator.SendAsync(new PingCommand("ping"));

        Assert.Equal("pong:ping", response);
    }

    [Fact]
    public async Task SendAsync_WithoutResponseCommand_ShouldExecuteHandlerSuccessfully()
    {
        var services = new ServiceCollection();
        services.AddSingleton<InvocationTracker>();
        services.AddTransient<IHandlerCommand<PingWithoutResponseCommand>, PingWithoutResponseHandler>();

        var provider = services.BuildServiceProvider();
        var mediator = new MediatVP.Mediator(provider);

        await mediator.SendAsync(new PingWithoutResponseCommand("ping"));

        var tracker = provider.GetRequiredService<InvocationTracker>();
        Assert.Equal(new[] { "handler:void:ping" }, tracker.Events);
    }

    [Fact]
    public async Task SendAsync_WithResponseCommand_ShouldRunPipelineBehavior()
    {
        var services = new ServiceCollection();
        services.AddSingleton<InvocationTracker>();
        services.AddTransient<IHandlerCommand<PingCommand, string>, PingHandler>();
        services.AddTransient<IPipelineBehavior<PingCommand, string>, TrackingBehaviorForResponse>();

        var provider = services.BuildServiceProvider();
        var mediator = new MediatVP.Mediator(provider);

        var response = await mediator.SendAsync(new PingCommand("pipeline"));

        var tracker = provider.GetRequiredService<InvocationTracker>();
        Assert.Equal("pong:pipeline", response);
        Assert.Equal(
            new[] { "behavior:before:response", "handler:response:pipeline", "behavior:after:response" },
            tracker.Events);
    }

    [Fact]
    public async Task SendAsync_WithoutResponseCommand_ShouldRunPipelineBehaviorThroughUnit()
    {
        var services = new ServiceCollection();
        services.AddSingleton<InvocationTracker>();
        services.AddTransient<IHandlerCommand<PingWithoutResponseCommand>, PingWithoutResponseHandler>();
        services.AddTransient<IPipelineBehavior<PingWithoutResponseCommand, Unit>, TrackingBehaviorForUnit>();

        var provider = services.BuildServiceProvider();
        var mediator = new MediatVP.Mediator(provider);

        await mediator.SendAsync(new PingWithoutResponseCommand("pipeline"));

        var tracker = provider.GetRequiredService<InvocationTracker>();
        Assert.Equal(
            new[] { "behavior:before:unit", "handler:void:pipeline", "behavior:after:unit" },
            tracker.Events);
    }

    [Fact]
    public async Task SendAsync_WithoutRegisteredHandler_ShouldThrowHandlerNotFoundException()
    {
        var mediator = new MediatVP.Mediator(new ServiceCollection().BuildServiceProvider());

        await Assert.ThrowsAsync<HandlerNotFoundException>(() => mediator.SendAsync(new PingCommand("missing")));
        await Assert.ThrowsAsync<HandlerNotFoundException>(() => mediator.SendAsync(new PingWithoutResponseCommand("missing")));
    }
}
