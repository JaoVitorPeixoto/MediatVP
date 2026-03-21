using System;

namespace MediatVP.Abstractions;

/// <summary>
/// Represents a request message that returns a response.
/// </summary>
/// <typeparam name="TResponse">Response type expected for this request.</typeparam>
public interface IRequestCommand<out TResponse>;

/// <summary>
/// Represents a request message with no response payload.
/// Internally this maps to <see cref="Unit"/> for pipeline unification.
/// </summary>
public interface IRequestCommand : IRequestCommand<Unit>;
