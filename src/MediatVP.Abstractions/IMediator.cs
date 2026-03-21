using System;

namespace MediatVP.Abstractions;

/// <summary>
/// Defines the mediator contract responsible for dispatching requests to their handlers.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a request that expects a response.
    /// </summary>
    /// <typeparam name="TResponse">Type of the response produced by the handler.</typeparam>
    /// <param name="request">Request instance to dispatch.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that resolves to the handler response.</returns>
    public Task<TResponse> SendAsync<TResponse>(IRequestCommand<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request that does not produce a response payload.
    /// </summary>
    /// <param name="request">Request instance to dispatch.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the handler finishes execution.</returns>
    public Task SendAsync(IRequestCommand request, CancellationToken cancellationToken = default);
}
