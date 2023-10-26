using Reminders.BLL.CQS.VerificationCodes.Commands.ResetVerificationCode;

namespace Reminders.UnitTests.VerificationCodes.ResetVerificationCode;

public class ResetVerificationCodeHandlerTests : IClassFixture<VerificationCodeHandlerFixture>
{
    private readonly ResetVerificationCodeHandler _resetVerificationCodeHandler;
    private readonly VerificationCodeHandlerFixture _fixture;
    
    public ResetVerificationCodeHandlerTests(VerificationCodeHandlerFixture fixture)
    {
        _fixture = fixture;
        _resetVerificationCodeHandler = new ResetVerificationCodeHandler(
            _fixture.MockUnitOfWork.Object, 
            _fixture.MockMapper.Object, 
            _fixture.MockEmailService.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCommand_UserVerificationReset()
    {
        // Arrange
        var command = new ResetVerificationCodeCommand() { UserId = 1, CurrentUserId = 1 };
        var existingVerificationCode = new VerificationCode { UserId = 1 };
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Users.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync(new User { Id = 1, Email = Constants.ValidEmail });
        _fixture.MockUnitOfWork.Setup(uow => uow.VerificationCodes.GetAsync(
                It.IsAny<VerificationCodeFilter>())).ReturnsAsync(existingVerificationCode);
        _fixture.MockMapper.Setup(m => m.Map<ResetVerificationCodeCommand, VerificationCode>(
                It.IsAny<ResetVerificationCodeCommand>())).Returns(new VerificationCode());
        _fixture.MockEmailService.Setup(x => x.SendEmailAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        await _resetVerificationCodeHandler.HandleAsync(command);

        // Assert
        _fixture.MockUnitOfWork.Verify(uow => uow.VerificationCodes.UpdateAsync(
            It.IsAny<VerificationCode>()), Times.Once);
        _fixture.MockEmailService.Verify(x => x.SendEmailAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_MismatchedUserId_UnauthorizedAccessExceptionThrown()
    {
        // Arrange
        var command = new ResetVerificationCodeCommand { UserId = 1, CurrentUserId = 2 };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _resetVerificationCodeHandler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_UserVerificationNotFound_EntityNotFoundExceptionThrown()
    {
        // Arrange
        var command = new ResetVerificationCodeCommand { UserId = 1, CurrentUserId = 1 };
        VerificationCode existingVerificationCode = null;

        _fixture.MockUnitOfWork.Setup(uow => uow.VerificationCodes.GetAsync(
                It.IsAny<VerificationCodeFilter>())).ReturnsAsync(existingVerificationCode);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _resetVerificationCodeHandler.HandleAsync(command));
    }
}