using MediatVP.Abstractions;
using MediatVP.Tests.TestDoubles.Common;

namespace MediatVP.Tests.Mediator.TestDoubles;

public sealed class TrackingBehaviorForResponse : IPipelineBehavior<PingCommand, string>
{
    private readonly InvocationTracker _tracker;

    public TrackingBehaviorForResponse(InvocationTracker tracker)
    {
        _tracker = tracker;
    }

    public async Task<string> Handle(PingCommand request, RequestHandlerDelegate<string> next, CancellationToken cancellationToken = default)
    {
        _tracker.Events.Add("behavior:before:response");
        var response = await next();
        _tracker.Events.Add("behavior:after:response");
        return response;
    }
}

public sealed class TrackingBehaviorForUnit : IPipelineBehavior<PingWithoutResponseCommand, Unit>
{
    private readonly InvocationTracker _tracker;

    public TrackingBehaviorForUnit(InvocationTracker tracker)
    {
        _tracker = tracker;
    }

    public async Task<Unit> Handle(PingWithoutResponseCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken = default)
    {
        _tracker.Events.Add("behavior:before:unit");
        var response = await next();
        _tracker.Events.Add("behavior:after:unit");
        return response;
    }
}
