using MediatVP.Abstraction;
using MediatVP.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatVP;

internal class Mediator (IServiceProvider serviceProvider) : IMediator
{

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
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
}
