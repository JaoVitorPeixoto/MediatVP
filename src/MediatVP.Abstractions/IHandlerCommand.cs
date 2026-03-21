using System;

namespace MediatVP.Abstractions;

/// <summary>
/// Handles a request and returns a response.
/// </summary>
/// <typeparam name="TRequest">Request type handled by this handler.</typeparam>
/// <typeparam name="TResponse">Response type produced by this handler.</typeparam>
public interface IHandlerCommand<in TRequest, TResponse> 
    where TRequest : IRequestCommand<TResponse>
{
    /// <summary>
    /// Executes handler logic for the given request.
    /// </summary>
    /// <param name="request">Request instance.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task containing the response.</returns>
    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Handles a request that does not return a response payload.
/// </summary>
/// <typeparam name="TRequest">Request type handled by this handler.</typeparam>
public interface IHandlerCommand<in TRequest> 
    where TRequest : IRequestCommand
{
    /// <summary>
    /// Executes handler logic for the given request.
    /// </summary>
    /// <param name="request">Request instance.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when processing is finished.</returns>
    public Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}