using MediatVP.Abstractions;
using MediatVP.Tests.TestDoubles.Common;

namespace MediatVP.Tests.Extensions.TestDoubles;

public sealed class FirstBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly InvocationTracker _tracker;

    public FirstBehavior(InvocationTracker tracker)
    {
        _tracker = tracker;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        _tracker.Events.Add("first:before");
        var response = await next();
        _tracker.Events.Add("first:after");
        return response;
    }
}

public sealed class SecondBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly InvocationTracker _tracker;

    public SecondBehavior(InvocationTracker tracker)
    {
        _tracker = tracker;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        _tracker.Events.Add("second:before");
        var response = await next();
        _tracker.Events.Add("second:after");
        return response;
    }
}
