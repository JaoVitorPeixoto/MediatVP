using System;

namespace MediatVP.Abstractions;

/// <summary>
/// Delegate that represents the next step in a request pipeline that returns a response.
/// </summary>
/// <typeparam name="TResponse">Response type produced by the pipeline chain.</typeparam>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
/// Delegate that represents the next step in a request pipeline with no response payload.
/// </summary>
public delegate Task RequestHandlerDelegate();

/// <summary>
/// Defines a pipeline behavior that can intercept request execution before and after the next handler.
/// </summary>
/// <typeparam name="TRequest">Request type intercepted by this behavior.</typeparam>
/// <typeparam name="TResponse">Response type flowing through the pipeline.</typeparam>
public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : notnull
{
    /// <summary>
    /// Handles cross-cutting logic for the current request and then invokes the next delegate.
    /// </summary>
    /// <param name="request">Current request instance.</param>
    /// <param name="next">Delegate that continues the pipeline chain.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task containing the final response.</returns>
    Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default
    );
}