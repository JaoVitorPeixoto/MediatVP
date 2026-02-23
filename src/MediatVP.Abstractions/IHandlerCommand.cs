using System;

namespace MediatVP.Abstractions;

public interface IHandlerCommand<in TRequest, TResponse> 
    where TRequest : IRequestCommand<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IHandlerCommand<in TRequest> 
    where TRequest : IRequestCommand
{
    public Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}