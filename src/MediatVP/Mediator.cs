using MediatVP.Exceptions;
using MediatVP.Abstractions;

namespace MediatVP;

internal class Mediator (IServiceProvider serviceProvider) : IMediator
{

    public async Task<TResponse> SendAsync<TResponse>(IRequestCommand<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerCommandType = request.GetType();

        var handlerType = typeof(IHandlerCommand<,>).MakeGenericType(handlerCommandType, typeof(TResponse));

        var handler = serviceProvider.GetService(handlerType);
        if (handler is null)
            throw new HandlerNotFoundException(handlerType);

        var method = handlerType.GetMethod("HandleAsync");
        if (method is null)
            throw new MethodNotFoundForHandlerException(handlerType);

        var result = method.Invoke(handler, [request, cancellationToken]);
        if (result is not Task<TResponse> task)
            throw new MethodReturnUnexpectedTypeException(result);

        return await task;
    }

    public async Task SendAsync(IRequestCommand request, CancellationToken cancellationToken = default)
    {
        var handlerCommandType = request.GetType();

        var handlerType = typeof(IHandlerCommand<>).MakeGenericType(handlerCommandType);

        var handler = serviceProvider.GetService(handlerType);
        if (handler is null)
            throw new HandlerNotFoundException(handlerType);

        var method = handlerType.GetMethod("HandleAsync");
        if (method is null)
            throw new MethodNotFoundForHandlerException(handlerType);

        var result = method.Invoke(handler, [request, cancellationToken]);
        if (result is not Task task)
            throw new MethodReturnUnexpectedTypeException(result);

        await task;
    }
}
