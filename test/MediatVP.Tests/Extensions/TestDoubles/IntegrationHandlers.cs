using MediatVP.Abstractions;
using MediatVP.Tests.TestDoubles.Common;

namespace MediatVP.Tests.Extensions.TestDoubles;

public sealed class IntegrationPingHandler : IHandlerCommand<IntegrationPingCommand, string>
{
    private readonly InvocationTracker _tracker;

    public IntegrationPingHandler(InvocationTracker tracker)
    {
        _tracker = tracker;
    }

    public Task<string> HandleAsync(IntegrationPingCommand request, CancellationToken cancellationToken = default)
    {
        _tracker.Events.Add("handler:response");
        return Task.FromResult($"handled:{request.Message}");
    }
}

public sealed class IntegrationPingWithoutResponseHandler : IHandlerCommand<IntegrationPingWithoutResponseCommand>
{
    private readonly InvocationTracker _tracker;

    public IntegrationPingWithoutResponseHandler(InvocationTracker tracker)
    {
        _tracker = tracker;
    }

    public Task HandleAsync(IntegrationPingWithoutResponseCommand request, CancellationToken cancellationToken = default)
    {
        _tracker.Events.Add("handler:void");
        return Task.CompletedTask;
    }
}
