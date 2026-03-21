using MediatVP.Abstractions;
using MediatVP.Extensions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddMediatVP(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var serviceProvider = services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IMediator>();

var created = await mediator.SendAsync(new CreateUserCommand("Vitor", "vitor@gmail.com"));
Console.WriteLine($"User created: {created}");

await mediator.SendAsync(new SendWelcomeEmailCommand("vitor@gmail.com"));

public record CreateUserCommand(string Name, string Email) : IRequestCommand<bool>;

public record SendWelcomeEmailCommand(string Email) : IRequestCommand;

public sealed class CreateUserCommandHandler : IHandlerCommand<CreateUserCommand, bool>
{
    public Task<bool> HandleAsync(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[Handler] Creating user: {request.Name} ({request.Email})");
        return Task.FromResult(true);
    }
}

public sealed class SendWelcomeEmailCommandHandler : IHandlerCommand<SendWelcomeEmailCommand>
{
    public Task HandleAsync(SendWelcomeEmailCommand request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[Handler] Sending welcome email to {request.Email}");
        return Task.CompletedTask;
    }
}

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        var requestName = request?.GetType().Name ?? typeof(TRequest).Name;
        Console.WriteLine($"[Pipeline] Starting {requestName}");
        var response = await next();
        Console.WriteLine($"[Pipeline] Finished {requestName}");
        return response;
    }
}