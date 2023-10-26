using Reminders.BLL.CQS.Reminders.Commands.UpdateReminder;

namespace Reminders.UnitTests.CQS.Reminders.Commands.UpdateReminder;

public class UpdateReminderHandlerTests : IClassFixture<ReminderHandlerFixture>
{
    private readonly ReminderHandlerFixture _fixture;
    private readonly UpdateReminderHandler _updateReminderHandler;

    public UpdateReminderHandlerTests(ReminderHandlerFixture fixture)
    {
        _fixture = fixture;
        _updateReminderHandler = new UpdateReminderHandler(_fixture.MockUnitOfWork.Object, _fixture.MockMapper.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_ReminderUpdated()
    {
        // Arrange
        var reminder = new Reminder { UserId = 1, Id = 1 };
        var command = new UpdateReminderCommand
            { UserId = 1, Id = 1, Name = Constants.FirstValidReminderName, Date = DateTime.Now.AddMinutes(10) };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>())).ReturnsAsync(reminder);

        // Act
        await _updateReminderHandler.HandleAsync(command);

        // Assert
        _fixture.MockReminderRepository.Verify(repo => repo.UpdateAsync(
            It.IsAny<Reminder>()), Times.Once);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCommandWithDecorator_ReminderUpdated()
    {
        // Arrange
        var command = new UpdateReminderCommand
        {
            UserId = 1,
            Id = 1,
            Name = "Test",
            Date = DateTime.Now.AddMinutes(10)
        };

        var mockValidator = new Mock<IValidator<UpdateReminderCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<UpdateReminderCommand>>();

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var decorator = new CommandValidationDecorator<UpdateReminderCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act
        await decorator.HandleAsync(command);

        // Assert
        mockInnerHandler.Verify(h => h.HandleAsync(command), Times.Once);
    }
    
    [Fact]
    public async Task HandleAsync_EmptyNameWithDecorator_ValidationExceptionThrown()
    {
        // Arrange
        var command = new UpdateReminderCommand { UserId = 1, Id = 1, Name = "", Date = DateTime.Now.AddMinutes(10) };

        var mockValidator = new Mock<IValidator<UpdateReminderCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<UpdateReminderCommand>>();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required.")
        };

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var decorator = new CommandValidationDecorator<UpdateReminderCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => decorator.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_DateInPastWithDecorator_ValidationExceptionThrown()
    {
        // Arrange
        var command = new UpdateReminderCommand { UserId = 1, Id = 1, Name = "Test", Date = DateTime.Now.AddMinutes(-10) };

        var mockValidator = new Mock<IValidator<UpdateReminderCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<UpdateReminderCommand>>();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Date", "Cannot set a reminder for a past date.")
        };

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var decorator = new CommandValidationDecorator<UpdateReminderCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => decorator.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_UnauthorizedUser_UnauthorizedAccessExceptionThrown()
    {
        // Arrange
        var reminder = new Reminder { UserId = 2, Id = 1 };
        var command = new UpdateReminderCommand
            { UserId = 1, Id = 1, Name = Constants.FirstValidReminderName, Date = DateTime.Now.AddMinutes(10) };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>())).ReturnsAsync(reminder);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _updateReminderHandler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_ReminderNotFound_EntityNotFoundExceptionThrown()
    {
        // Arrange
        Reminder? reminder = null;
        var command = new UpdateReminderCommand
            { UserId = 1, Id = 1, Name = Constants.FirstValidReminderName, Date = DateTime.Now.AddMinutes(10) };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>())).ReturnsAsync(reminder);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _updateReminderHandler.HandleAsync(command));
    }
}