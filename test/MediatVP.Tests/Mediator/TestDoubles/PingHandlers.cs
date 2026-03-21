using MediatVP.Abstractions;
using MediatVP.Tests.TestDoubles.Common;

namespace MediatVP.Tests.Mediator.TestDoubles;

public sealed class PingHandler : IHandlerCommand<PingCommand, string>
{
    private readonly InvocationTracker _tracker;

    public PingHandler(InvocationTracker tracker)
    {
        _tracker = tracker;
    }

    public Task<string> HandleAsync(PingCommand request, CancellationToken cancellationToken = default)
    {
        _tracker.Events.Add($"handler:response:{request.Message}");
        return Task.FromResult($"pong:{request.Message}");
    }
}

public sealed class PingWithoutResponseHandler : IHandlerCommand<PingWithoutResponseCommand>
{
    private readonly InvocationTracker _tracker;

    public PingWithoutResponseHandler(InvocationTracker tracker)
    {
        _tracker = tracker;
    }

    public Task HandleAsync(PingWithoutResponseCommand request, CancellationToken cancellationToken = default)
    {
        _tracker.Events.Add($"handler:void:{request.Message}");
        return Task.CompletedTask;
    }
}
