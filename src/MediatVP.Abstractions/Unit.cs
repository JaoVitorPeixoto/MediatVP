namespace MediatVP.Abstractions;

/// <summary>
/// Represents a void-like response type used when no payload is returned.
/// </summary>
public readonly struct Unit
{
    /// <summary>
    /// Singleton value for <see cref="Unit"/>.
    /// </summary>
    public static readonly Unit Value = new();
}
