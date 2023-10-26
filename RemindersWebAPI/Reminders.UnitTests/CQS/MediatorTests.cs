using Microsoft.Extensions.DependencyInjection;
using Reminders.BLL.CQS.Reminders.Commands.CreateReminder;
using Reminders.BLL.CQS.Reminders.Queries.GetReminderById;

namespace Reminders.UnitTests;

public class MediatorTests
{
    [Fact]
    public async Task SendCommandAsync_ValidCommand_CallsHandlerOnce()
    {
        // Arrange
        var command = new CreateReminderCommand();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var commandHandlerMock = new Mock<ICommandHandler<CreateReminderCommand>>();

        serviceProviderMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);
        serviceScopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(serviceScopeMock.Object);
        serviceScopeMock
            .Setup(scope => scope.ServiceProvider.GetService(typeof(ICommandHandler<CreateReminderCommand>)))
            .Returns(commandHandlerMock.Object);

        var mediator = new Mediator(serviceProviderMock.Object);

        // Act
        await mediator.SendCommandAsync(command);

        // Assert
        commandHandlerMock.Verify(h => h.HandleAsync(command), Times.Once);
    }

    [Fact]
    public async Task SendCommandWithResultAsync_ValidCommand_CallsHandlerOnceAndReturnsResult()
    {
        // Arrange
        var command = new CreateReminderCommand();
        var commandResult = 42;  
        var serviceProviderMock = new Mock<IServiceProvider>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var commandHandlerMock = new Mock<ICommandHandler<CreateReminderCommand, int>>();

        commandHandlerMock.Setup(ch => ch.HandleAsync(command)).ReturnsAsync(commandResult);

        serviceProviderMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);
        serviceScopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(serviceScopeMock.Object);
        serviceScopeMock
            .Setup(scope => scope.ServiceProvider.GetService(typeof(ICommandHandler<CreateReminderCommand, int>)))
            .Returns(commandHandlerMock.Object);

        var mediator = new Mediator(serviceProviderMock.Object);

        // Act
        var result = await mediator.SendCommandAsync<CreateReminderCommand, int>(command);

        // Assert
        commandHandlerMock.Verify(h => h.HandleAsync(command), Times.Once);
        Assert.Equal(commandResult, result);
    }
    
    [Fact]
    public async Task SendQueryAsync_ValidQuery_CallsHandlerOnce()
    {
        // Arrange
        var query = new GetReminderByIdQuery();
        var queryResult = new ReminderDto(); 
        var serviceProviderMock = new Mock<IServiceProvider>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var queryHandlerMock = new Mock<IQueryHandler<GetReminderByIdQuery, ReminderDto>>();

        queryHandlerMock.Setup(q => q.HandleAsync(query)).ReturnsAsync(queryResult);

        serviceProviderMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);
        serviceScopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(serviceScopeMock.Object);
        serviceScopeMock
            .Setup(scope => scope.ServiceProvider.GetService(typeof(IQueryHandler<GetReminderByIdQuery, ReminderDto>)))
            .Returns(queryHandlerMock.Object);

        var mediator = new Mediator(serviceProviderMock.Object);

        // Act
        var result = await mediator.SendQueryAsync<GetReminderByIdQuery, ReminderDto>(query);

        // Assert
        queryHandlerMock.Verify(h => h.HandleAsync(query), Times.Once);
        Assert.Equal(queryResult, result);
    }
}