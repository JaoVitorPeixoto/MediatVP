using MediatVP.Abstractions;
using MediatVP.Exceptions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Xunit.Abstractions;

namespace MediatVP.Tests.Mediator;

public class SendTests
{
    private readonly ITestOutputHelper _console;


    public record PingCommand : IRequestCommand<string>;
    public record PingCommandVoidReturn  : IRequestCommand;

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
    public async Task SendAsync_GivenRequestCommandWithVoidResponse_ShouldReturnVoidResponse()
    {
        // Arrange
        var services = new ServiceCollection();

        var mockHandlerCommand = Substitute.For<IHandlerCommand<PingCommandVoidReturn>>();
        mockHandlerCommand.HandleAsync(Arg.Any<PingCommandVoidReturn>()).Returns(Task.CompletedTask);
        services.AddSingleton(mockHandlerCommand);

        var serviceProvider = services.BuildServiceProvider();

        var mediator = new MediatVP.Mediator(serviceProvider);

        // Act
        var sendTask = mediator.SendAsync(new PingCommandVoidReturn());

        // Assert
        await Assert.IsAssignableFrom<Task>(sendTask);
        await sendTask;
        await mockHandlerCommand.Received(1).HandleAsync(Arg.Any<PingCommandVoidReturn>());
    }

    [Fact]
    public async Task SendAsync_GivenCommandWithoutHandler_ShouldThrowHandlerNotFoundException()
    {
        // Arrrange    
        var serviceProvider = Substitute.For<IServiceProvider>();

        var mediator = new MediatVP.Mediator(serviceProvider);
  
        // Act
        var actionSend = () => mediator.SendAsync(new PingCommand());
        
        // Assert
        await Assert.ThrowsAnyAsync<HandlerNotFoundException>(actionSend);
    }

    [Fact]
    public async Task SendAsync_GivenRequestCommandWithVoidResponse_ShouldThrowHandlerNotFoundException()
    {
        // Arrrange    
        var serviceProvider = Substitute.For<IServiceProvider>();

        var mediator = new MediatVP.Mediator(serviceProvider);
  
        // Act
        var actionSend = () => mediator.SendAsync(new PingCommandVoidReturn());
        
        // Assert
        await Assert.ThrowsAnyAsync<HandlerNotFoundException>(actionSend);
    }

}
