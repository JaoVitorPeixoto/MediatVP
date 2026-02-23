using System;

namespace MediatVP.Abstractions;

public interface IMediator
{
    public Task<TResponse> SendAsync<TResponse>(IRequestCommand<TResponse> request, CancellationToken cancellationToken = default);
    public Task SendAsync(IRequestCommand request, CancellationToken cancellationToken = default);
}
