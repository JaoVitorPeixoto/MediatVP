using MediatVP.Abstractions;

namespace MediatVP.Tests.Extensions.TestDoubles;

public record IntegrationPingCommand(string Message) : IRequestCommand<string>;

public record IntegrationPingWithoutResponseCommand(string Message) : IRequestCommand;
