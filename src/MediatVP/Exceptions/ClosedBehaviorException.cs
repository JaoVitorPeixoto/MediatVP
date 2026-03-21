using System;

namespace MediatVP.Exceptions;

/// <summary>
/// Exception thrown when a pipeline behavior registration receives a closed type instead of an open generic type definition.
/// </summary>
public class ClosedBehaviorException : Exception
{
    /// <summary>
    /// Gets the behavior type that caused the exception.
    /// </summary>
    public Type Behavior { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ClosedBehaviorException"/>.
    /// </summary>
    /// <param name="behavior">Behavior type that is not open generic.</param>
    public ClosedBehaviorException(Type behavior)
    : base($"The behavior type '{behavior?.FullName ?? behavior?.Name ?? "<null>"}' must be an open generic type definition.")
    {
        Behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
    }
}
