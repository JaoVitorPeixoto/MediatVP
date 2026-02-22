using System;

namespace MediatVP.Abstraction;

public interface IHandlerCommand<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
