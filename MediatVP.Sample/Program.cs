// See https://aka.ms/new-console-template for more information
using MediatVP;
using MediatVP.Abstraction;
using MediatVP.Extensions;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection(); 
services.AddMediatVP(typeof(Program).Assembly);

var serviceProvider = services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IMediator>();

await mediator.SendAsync(new CreateUserCommand("Vitor", "vitor@gmail.com"));

public record CreateUserCommand(string Name, string Email) : IRequest<bool>;

public class CreateUserCommandHandler : IHandlerCommand<CreateUserCommand, bool>
{
    public Task<bool> HandleAsync(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Creating new User with name {request.Name} and e-mail {request.Email}");

        return Task.FromResult<bool>(true);
    }
}