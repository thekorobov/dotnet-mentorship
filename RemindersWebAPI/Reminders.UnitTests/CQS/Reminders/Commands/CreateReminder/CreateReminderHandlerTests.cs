using Reminders.BLL.CQS.Reminders.Commands.CreateReminder;

namespace Reminders.UnitTests.CQS.Reminders.Commands.CreateReminder;

public class CreateReminderHandlerTests : IClassFixture<ReminderHandlerFixture>
{
    private readonly ReminderHandlerFixture _fixture;
    private readonly CreateReminderHandler _createReminderHandler;
    
    public CreateReminderHandlerTests(ReminderHandlerFixture fixture)
    {
        _fixture = fixture;
        _createReminderHandler = new CreateReminderHandler(_fixture.MockUnitOfWork.Object, _fixture.MockMapper.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_ReminderCreated()
    {
        // Arrange
        var command = new CreateReminderCommand
        {
            UserId = 1,
            Name = Constants.FirstValidReminderName,
            Date = DateTime.Now.AddMinutes(10)
        };

        // Act
        await _createReminderHandler.HandleAsync(command);

        // Assert
        _fixture.MockReminderRepository.Verify(repo => repo.CreateAsync(
            It.IsAny<Reminder>()), Times.Once);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCommandWithDecorator_ReminderCreated()
    {
        // Arrange
        var command = new CreateReminderCommand();
        var commandResult = 42;
        var mockValidator = new Mock<IValidator<CreateReminderCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<CreateReminderCommand, int>>();

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        mockInnerHandler
            .Setup(h => h.HandleAsync(command))
            .ReturnsAsync(commandResult);

        var decorator = new CommandResultValidationDecorator<CreateReminderCommand, int>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act
        var result = await decorator.HandleAsync(command);

        // Assert
        mockInnerHandler.Verify(h => h.HandleAsync(command), Times.Once);
        Assert.Equal(commandResult, result);
    }
    
    [Fact]
    public async Task HandleAsync_EmptyNameWithDecorator_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateReminderCommand
        {
            UserId = 1,
            Name = "",
            Date = DateTime.Now.AddMinutes(10)
        };

        var mockValidator = new Mock<IValidator<CreateReminderCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<CreateReminderCommand, int>>();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required.")
        };

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var decorator = new CommandResultValidationDecorator<CreateReminderCommand, int>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => decorator.HandleAsync(command));
    }
    
    [Fact]
    public async Task HandleAsync_EmptyDateWithDecorator_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateReminderCommand
        {
            UserId = 1,
            Name = "Test",
            Date = DateTime.Now.AddMinutes(-10) 
        };

        var mockValidator = new Mock<IValidator<CreateReminderCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<CreateReminderCommand, int>>();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Date", "Cannot add a reminder for a past date.")
        };

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var decorator = new CommandResultValidationDecorator<CreateReminderCommand, int>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => decorator.HandleAsync(command));
    }
}