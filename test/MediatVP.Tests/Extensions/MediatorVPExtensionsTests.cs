using MediatVP.Abstractions;
using MediatVP.Tests.Extensions.TestDoubles;
using MediatVP.Tests.TestDoubles.Common;
using MediatVP.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatVP.Tests.Extensions;

public class MediatorVPExtensionsTests
{
    [Fact]
    public async Task AddMediatVP_ShouldRegisterHandlersMediatorAndOpenBehaviors()
    {
        var services = new ServiceCollection();
        services.AddSingleton<InvocationTracker>();

        services.AddMediatVP(config =>
        {
            config.RegisterServicesFromAssembly(typeof(IntegrationPingHandler).Assembly);
            config.AddOpenBehavior(typeof(FirstBehavior<,>));
            config.AddOpenBehavior(typeof(SecondBehavior<,>));
        });

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        var response = await mediator.SendAsync(new IntegrationPingCommand("hello"));

        Assert.Equal("handled:hello", response);

        var tracker = provider.GetRequiredService<InvocationTracker>();
        Assert.Equal(
            new[]
            {
                "first:before",
                "second:before",
                "handler:response",
                "second:after",
                "first:after"
            },
            tracker.Events);
    }

    [Fact]
    public async Task AddMediatVP_ShouldApplyOpenBehaviorToNoResponseRequestsUsingUnit()
    {
        var services = new ServiceCollection();
        services.AddSingleton<InvocationTracker>();

        services.AddMediatVP(config =>
        {
            config.RegisterServicesFromAssembly(typeof(IntegrationPingWithoutResponseHandler).Assembly);
            config.AddOpenBehavior(typeof(FirstBehavior<,>));
        });

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        await mediator.SendAsync(new IntegrationPingWithoutResponseCommand("hello"));

        var tracker = provider.GetRequiredService<InvocationTracker>();
        Assert.Equal(
            new[]
            {
                "first:before",
                "handler:void",
                "first:after"
            },
            tracker.Events);
    }
}
