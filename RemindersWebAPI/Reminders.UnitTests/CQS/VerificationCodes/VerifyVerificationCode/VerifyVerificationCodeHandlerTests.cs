using Reminders.BLL.CQS.VerificationCodes.Commands.VerifyVerificationCode;

namespace Reminders.UnitTests.VerificationCodes.VerifyVerificationCode;

public class VerifyVerificationCodeHandlerTests : IClassFixture<VerificationCodeHandlerFixture>
{
    private readonly VerifyVerificationCodeHandler _verifyVerificationCodeHandler;
    private readonly VerificationCodeHandlerFixture _fixture;
    
    public VerifyVerificationCodeHandlerTests(VerificationCodeHandlerFixture fixture)
    {
        _fixture = fixture;
        _verifyVerificationCodeHandler = new VerifyVerificationCodeHandler(
            _fixture.MockUnitOfWork.Object, 
            _fixture.MockMapper.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCommand_UserVerified()
    {
        // Arrange
        var command = new VerifyVerificationCodeCommand() { VerificationToken = Constants.ValidVerificationToken1 };
        var existingVerificationCode = new VerificationCode { VerificationToken = Constants.ValidVerificationToken1 };
        
        _fixture.MockUserVerificationRepository.Setup(u => u.GetAsync(
                It.IsAny<VerificationCodeFilter>())).ReturnsAsync(existingVerificationCode);
        _fixture.MockMapper.Setup(m => m.Map<VerifyVerificationCodeCommand, VerificationCode>(
                It.IsAny<VerifyVerificationCodeCommand>())).Returns(new VerificationCode());

        // Act
        await _verifyVerificationCodeHandler.HandleAsync(command);

        // Assert
        _fixture.MockUserVerificationRepository.Verify(u => u.UpdateAsync(
            It.IsAny<VerificationCode>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_InvalidCode_EntityNotFoundExceptionThrown()
    {
        // Arrange
        var command = new VerifyVerificationCodeCommand { VerificationToken = String.Empty };

        // Setup your mock to return null
        _fixture.MockUserVerificationRepository.Setup(u => u.GetAsync(
                It.IsAny<VerificationCodeFilter>())).ReturnsAsync((VerificationCode)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _verifyVerificationCodeHandler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_AlreadyVerified_UserAlreadyVerifiedExceptionThrown()
    {
        // Arrange
        var command = new VerifyVerificationCodeCommand { VerificationToken = Constants.ValidVerificationToken1 };
        var existingVerificationCode = new VerificationCode 
            { VerificationToken = Constants.ValidVerificationToken1, VerifiedAt = DateTime.UtcNow };
        
        _fixture.MockUserVerificationRepository.Setup(u => u.GetAsync(
                It.IsAny<VerificationCodeFilter>())).ReturnsAsync(existingVerificationCode);

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyVerifiedException>(() => _verifyVerificationCodeHandler.HandleAsync(command));
    }
}