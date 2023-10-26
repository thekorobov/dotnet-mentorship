using Reminders.BLL.CQS.VerificationCodes.Commands.CreateVerificationCode;

namespace Reminders.UnitTests.VerificationCodes.CreateVerificationCode;

public class CreateVerificationCodeHandlerTests : IClassFixture<VerificationCodeHandlerFixture>
{
    private readonly CreateVerificationCodeHandler _createVerificationCodeHandler;
    private readonly VerificationCodeHandlerFixture _fixture;
    
    public CreateVerificationCodeHandlerTests(VerificationCodeHandlerFixture fixture)
    {
        _fixture = fixture;
        _createVerificationCodeHandler = new CreateVerificationCodeHandler(
            _fixture.MockUnitOfWork.Object, 
            _fixture.MockMapper.Object, 
            _fixture.MockEmailService.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCommand_UserVerificationCreated()
    {
        // Arrange
        var command = new CreateVerificationCodeCommand() { UserId = 1 };
        var user = new User { Id = 1, AuthProviderType = AuthProviderType.SimpleAuth};
        var verificationCode = new VerificationCode { UserId = 1, VerifiedAt = null };  

        _fixture.MockUserRepository.Setup(u => u.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync(user);
        _fixture.MockUserVerificationRepository.Setup(u => u.GetAsync(
            It.IsAny<VerificationCodeFilter>())).ReturnsAsync(verificationCode);  
        _fixture.MockMapper.Setup(m => m.Map<CreateVerificationCodeCommand, VerificationCode>(
                It.IsAny<CreateVerificationCodeCommand>())).Returns(new VerificationCode());
        _fixture.MockEmailService.Setup(x => x.SendEmailAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
        
        // Act
        await _createVerificationCodeHandler.HandleAsync(command);

        // Assert
        _fixture.MockUserVerificationRepository.Verify(u => u.CreateAsync(
            It.IsAny<VerificationCode>()), Times.Once);
        _fixture.MockEmailService.Verify(x => x.SendEmailAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_EntityNotFoundExceptionThrown()
    {
        // Arrange
        var command = new CreateVerificationCodeCommand { UserId = 1 };
        
        // Act
        _fixture.MockUserRepository.Setup(u => u.GetAsync(
                It.Is<UserFilter>(f => f.Id == 1))).ReturnsAsync((User)null);
        
        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _createVerificationCodeHandler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_UserAlreadyVerified_UserAlreadyVerifiedExceptionThrown()
    {
        // Arrange
        var command = new CreateVerificationCodeCommand { UserId = 1 };
        var user = new User { Id = 1 };
        var verificationCode = new VerificationCode { UserId = 1, VerifiedAt = DateTime.UtcNow };

        _fixture.MockUserRepository.Setup(u => u.GetAsync(
            It.IsAny<UserFilter>())).ReturnsAsync(user);
        _fixture.MockUserVerificationRepository.Setup(u => u.GetAsync(
            It.IsAny<VerificationCodeFilter>())).ReturnsAsync(verificationCode);

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyVerifiedException>(() => _createVerificationCodeHandler.HandleAsync(command));
    }
}