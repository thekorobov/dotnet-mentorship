using Reminders.BLL.CQS.Users.Commands.UpdateUser;

namespace Reminders.UnitTests.Users.Commands.ResetUserPassword;

public class ResetUserPasswordHandlerTests : IClassFixture<UserHandlerFixture>
{
    private readonly ResetUserPasswordHandler _resetUserPasswordHandler;
    private readonly UserHandlerFixture _fixture;

    public ResetUserPasswordHandlerTests(UserHandlerFixture fixture)
    {
        _fixture = fixture;
        _resetUserPasswordHandler = new ResetUserPasswordHandler(
            _fixture.MockUnitOfWork.Object, 
            _fixture.MockUserManager.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_PasswordReset()
    {
        // Arrange
        var command = new ResetUserPasswordCommand { Id = 1, CurrentUserId = 1, Password = "NewPassword" };
        var user = new User { Id = 1 };

        _fixture.MockUnitOfWork.Setup(uow => uow.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync(user);
        _fixture.MockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("Token");
        _fixture.MockUserManager.Setup(um => um.ResetPasswordAsync(user, "Token", command.Password)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _resetUserPasswordHandler.HandleAsync(command);

        // Assert
        _fixture.MockUserManager.Verify(um => um.ResetPasswordAsync(user, "Token", command.Password), Times.Once);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCommandWithDecorator_PasswordReset()
    {
        // Arrange
        var command = new ResetUserPasswordCommand { Id = 1, CurrentUserId = 1, Password = "NewPassword" };

        var mockValidator = new Mock<IValidator<ResetUserPasswordCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<ResetUserPasswordCommand>>();

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var decorator = new CommandValidationDecorator<ResetUserPasswordCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act
        await decorator.HandleAsync(command);

        // Assert
        mockInnerHandler.Verify(h => h.HandleAsync(command), Times.Once);
    }
    
    [Fact]
    public async Task HandleAsync_EmptyPasswordWithDecorator_ValidationExceptionThrown()
    {
        // Arrange
        var command = new ResetUserPasswordCommand { Id = 1, CurrentUserId = 1, Password = String.Empty };

        var mockValidator = new Mock<IValidator<ResetUserPasswordCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<ResetUserPasswordCommand>>();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Password", "Password is required.")
        };

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var decorator = new CommandValidationDecorator<ResetUserPasswordCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => decorator.HandleAsync(command));
    }
    
    [Fact]
    public async Task HandleAsync_InvalidPasswordWithDecorator_ValidationExceptionThrown()
    {
        // Arrange
        var command = new ResetUserPasswordCommand { Id = 1, CurrentUserId = 1, Password = "123" };

        var mockValidator = new Mock<IValidator<ResetUserPasswordCommand>>();
        var mockInnerHandler = new Mock<ICommandHandler<ResetUserPasswordCommand>>();

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Password", "Password must be at least 8 characters long.")
        };

        mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var decorator = new CommandValidationDecorator<ResetUserPasswordCommand>(
            mockInnerHandler.Object, mockValidator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => decorator.HandleAsync(command));
    }
    
    [Fact]
    public async Task HandleAsync_PasswordResetFailed_PasswordResetFailedExceptionThrown()
    {
        // Arrange
        var command = new ResetUserPasswordCommand { Id = 1, CurrentUserId = 1, Password = "new_password" };
        var user = new User { Id = 1 };

        _fixture.MockUnitOfWork.Setup(uow => uow.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync(user);
        _fixture.MockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("Token");
        _fixture.MockUserManager.Setup(um => um.ResetPasswordAsync(user, "Token", "new_password")).ReturnsAsync(IdentityResult.Failed());

        // Act & Assert
        await Assert.ThrowsAsync<PasswordResetFailedException>(() => _resetUserPasswordHandler.HandleAsync(command));
    }
    
    [Fact]
    public async Task HandleAsync_MismatchedUserId_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var command = new ResetUserPasswordCommand { Id = 1, CurrentUserId = 2, Password = "NewPassword" };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _resetUserPasswordHandler.HandleAsync(command));
    }
}