using MediatVP.Abstractions;

namespace MediatVP.Tests.Mediator.TestDoubles;

public record PingCommand(string Message) : IRequestCommand<string>;

public record PingWithoutResponseCommand(string Message) : IRequestCommand;
