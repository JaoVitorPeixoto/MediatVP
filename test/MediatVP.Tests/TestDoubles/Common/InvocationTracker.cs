namespace MediatVP.Tests.TestDoubles.Common;

public sealed class InvocationTracker
{
    public List<string> Events { get; } = new();
}
