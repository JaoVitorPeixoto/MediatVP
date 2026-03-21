using MediatVP.Exceptions;
using MediatVP.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace MediatVP;

internal class Mediator (IServiceProvider serviceProvider) : IMediator
{

    public async Task<TResponse> SendAsync<TResponse>(IRequestCommand<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var handlerCommandType = request.GetType();

        var handlerType = typeof(IHandlerCommand<,>).MakeGenericType(handlerCommandType, typeof(TResponse));

        var handlerInstance = serviceProvider.GetService(handlerType);
        if (handlerInstance is null)
            throw new HandlerNotFoundException(handlerType);

        RequestHandlerDelegate<TResponse> finalHandler = () =>
        {
            var method = handlerType.GetMethod("HandleAsync");
            if (method is null)
                throw new MethodNotFoundForHandlerException(handlerType);

            var result = method.Invoke(handlerInstance, [request, cancellationToken]);
            if (result is not Task<TResponse> task)
                throw new MethodReturnUnexpectedTypeException(result);

            return task;
        };

        var behaviorInterfaceType = typeof(IPipelineBehavior<,>).MakeGenericType(handlerCommandType, typeof(TResponse));
        var behaviors = ResolveServices(behaviorInterfaceType);

        var pipeline = BuildPipeline(request, cancellationToken, finalHandler, behaviors);
            
        return await pipeline();
    }

    public async Task SendAsync(IRequestCommand request, CancellationToken cancellationToken = default)
    {  
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var handlerCommandType = request.GetType();

        var handlerType = typeof(IHandlerCommand<>).MakeGenericType(handlerCommandType);

        var handlerInstance = serviceProvider.GetService(handlerType);
        if (handlerInstance is null)
            throw new HandlerNotFoundException(handlerType);

        RequestHandlerDelegate<Unit> finalHandler = async () =>
        {
            var method = handlerType.GetMethod("HandleAsync");
            if (method is null)
                throw new MethodNotFoundForHandlerException(handlerType);

            var result = method.Invoke(handlerInstance, [request, cancellationToken]);
            if (result is not Task task)
                throw new MethodReturnUnexpectedTypeException(result);

            await task;
            return Unit.Value;
        };

        var behaviorInterfaceType = typeof(IPipelineBehavior<,>).MakeGenericType(handlerCommandType, typeof(Unit));
        var behaviors = ResolveServices(behaviorInterfaceType);

        var pipeline = BuildPipeline(request, cancellationToken, finalHandler, behaviors);

        await pipeline();
    }

    private IEnumerable<object> ResolveServices(Type serviceType)
    {
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
        var resolved = serviceProvider.GetService(enumerableType) as IEnumerable;

        if (resolved is null)
            return Enumerable.Empty<object>();

        return resolved.Cast<object>();
    }

    private static RequestHandlerDelegate<TResponse> BuildPipeline<TResponse>(
        object request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> finalHandler,
        IEnumerable<object> behaviors)
    {
        var pipeline = finalHandler;

        foreach (var behavior in behaviors.Reverse())
        {
            var next = pipeline;
            pipeline = () => InvokeBehavior(behavior, request, next, cancellationToken);
        }

        return pipeline;
    }

    private static Task<TResponse> InvokeBehavior<TResponse>(
        object behavior,
        object request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var method = behavior.GetType().GetMethod("Handle");
        if (method is null)
            throw new MethodNotFoundForHandlerException(behavior.GetType());

        var result = method.Invoke(behavior, [request, next, cancellationToken]);
        if (result is not Task<TResponse> typedTask)
            throw new MethodReturnUnexpectedTypeException(result);

        return typedTask;
    }
}
