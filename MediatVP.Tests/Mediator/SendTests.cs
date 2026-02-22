using System.Net.NetworkInformation;
using MediatVP.Abstraction;
using MediatVP.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace MediatVP.Tests.Mediator;

public class SendTests
{
    private readonly ITestOutputHelper _console;


    public record PingCommand : IRequest<string>;

    public SendTests(ITestOutputHelper console)
    {
        this._console = console;
    }


    [Fact]
    public async Task SendAsync_GivenCommand_ShouldSendHandlerCommandAndReturnCorrectlyResponse()
    {
        // Arrange
        var excpectedResponse = "Pong";       

        var services = new ServiceCollection();

        var mockHandlerCommand = Substitute.For<IHandlerCommand<PingCommand, string>>();
        mockHandlerCommand.HandleAsync(Arg.Any<PingCommand>()).Returns(excpectedResponse);
        services.AddSingleton(mockHandlerCommand);

        var serviceProvider = services.BuildServiceProvider();

        var mediator = new MediatVP.Mediator(serviceProvider);


        // Act
        var returnedResponse = await mediator.SendAsync(new PingCommand());

        _console.WriteLine("Excpected: " + excpectedResponse);

        _console.WriteLine("Returned: " + returnedResponse);
        
        // Assert
        Assert.Equal(excpectedResponse, returnedResponse);
    }

    [Fact]
    public async Task SendAsync_GivenCommandwithoutHandler_ShouldThrowHandlerNotFoundException()
    {
        // Arrrange    
        var serviceProvider = Substitute.For<IServiceProvider>();

        var mediator = new MediatVP.Mediator(serviceProvider);
  
        // Act
        var actionSend = () => mediator.SendAsync(new PingCommand());
        
        // Assert
        await Assert.ThrowsAnyAsync<HandlerNotFoundException>(actionSend);
    }

}
