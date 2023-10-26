using Reminders.BLL.CQS.Users.Commands.ResetUserEmail;

namespace Reminders.UnitTests.Users.Commands.ResetUserEmail;

public class ResetUserEmailHandlerTests : IClassFixture<UserHandlerFixture>
{
    private readonly ResetUserEmailHandler _resetUserEmailHandler;
    private readonly UserHandlerFixture _fixture;

    public ResetUserEmailHandlerTests(UserHandlerFixture fixture)
    {
        _fixture = fixture;
        _resetUserEmailHandler = new ResetUserEmailHandler(_fixture.MockUnitOfWork.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_EmailReset()
    {
        // Arrange
        var command = new ResetUserEmailCommand { Id = 1, CurrentUserId = 1, Email = "new@example.com" };
        var user = new User { Id = 1 };

        _fixture.MockUnitOfWork.Setup(uow => uow.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync(user);

        // Act
        await _resetUserEmailHandler.HandleAsync(command);

        // Assert
        _fixture.MockUnitOfWork.Verify(uow => uow.Users.UpdateAsync(It.Is<User>(u => u.Email == "new@example.com")), Times.Once);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCommandWithDecorator_ReminderUpdated()
    {
        // Arrange
        var command = new ResetUserEmailCommand { Id = 1, CurrentUserId = 1, Email = "new@example.com" };

        var mockValidator = new Mock<IValidator<ResetUserEmailCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<ResetUserEmailCommand>>();

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var decorator = new CommandValidationDecorator<ResetUserEmailCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act
        await decorator.HandleAsync(command);

        // Assert
        mockInnerHandler.Verify(h => h.HandleAsync(command), Times.Once);
    }
    
    [Fact]
    public async Task HandleAsync_EmptyEmailWithDecorator_ValidationExceptionThrown()
    {
        // Arrange
        var command = new ResetUserEmailCommand { Id = 1, CurrentUserId = 1, Email = String.Empty };

        var mockValidator = new Mock<IValidator<ResetUserEmailCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<ResetUserEmailCommand>>();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Email is required.")
        };

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var decorator = new CommandValidationDecorator<ResetUserEmailCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => decorator.HandleAsync(command));
    }
    
    [Fact]
    public async Task HandleAsync_InvalidEmailWithDecorator_ValidationExceptionThrown()
    {
        // Arrange
        var command = new ResetUserEmailCommand { Id = 1, CurrentUserId = 1, Email = "invalid_email" };

        var mockValidator = new Mock<IValidator<ResetUserEmailCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<ResetUserEmailCommand>>();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Invalid email format.")
        };

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var decorator = new CommandValidationDecorator<ResetUserEmailCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => decorator.HandleAsync(command));
    }
    
    [Fact]
    public async Task HandleAsync_MismatchedUserId_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var command = new ResetUserEmailCommand { Id = 1, CurrentUserId = 2, Email = "new@example.com" };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _resetUserEmailHandler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var command = new ResetUserEmailCommand { Id = 1, CurrentUserId = 1, Email = "new@example.com" };
        User? user = null;

        _fixture.MockUnitOfWork.Setup(uow => uow.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _resetUserEmailHandler.HandleAsync(command));
    }
}