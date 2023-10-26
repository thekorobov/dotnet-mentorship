using Reminders.BLL.CQS.Users.Queries.GetAuthToken;

namespace Reminders.UnitTests.CQS.Users.Queries.GetAuthToken;

public class GetAuthTokenHandlerTests : IClassFixture<UserHandlerFixture>
{
    private readonly UserHandlerFixture _fixture;
    private readonly GetAuthTokenHandler _getAuthTokenHandler;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    
    public GetAuthTokenHandlerTests(UserHandlerFixture fixture)
    {
        _fixture = fixture;
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _getAuthTokenHandler = new GetAuthTokenHandler(
            _fixture.MockUnitOfWork.Object, 
            _mockJwtTokenGenerator.Object,
            _fixture.MockUserManager.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidQuery_ReturnsToken()
    {
        // Arrange
        var query = new GetAuthTokenQuery
            { Email = "test@email.com", Password = "password", AuthProviderType = AuthProviderType.SimpleAuth };
        var user = new User { Id = 1, Email = query.Email };
        var token = "valid_token";

        _fixture.MockUserRepository.Setup(u => u.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync(user);
        _fixture.MockUserVerificationRepository.Setup(u => u.GetAsync(It.IsAny<VerificationCodeFilter>()))
            .ReturnsAsync(new VerificationCode { VerifiedAt = DateTime.Now });
        _fixture.MockUnitOfWork.Setup(uow => uow.Users)
            .Returns(_fixture.MockUserRepository.Object);
        _fixture.MockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), 
            It.IsAny<string>())).ReturnsAsync(true);
        _mockJwtTokenGenerator.Setup(jwt => jwt.GenerateToken(It.IsAny<int>(), It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(token);

        // Act
        var result = await _getAuthTokenHandler.HandleAsync(query);

        // Assert
        Assert.Equal(token, result);
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_UnauthorizedAccessExceptionThrown()
    {
        // Arrange
        var query = new GetAuthTokenQuery { Email = "test@email.com", Password = "password" };

        _fixture.MockUserRepository.Setup(u => u.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync((User)null);
        _fixture.MockUnitOfWork.Setup(uow => uow.Users)
            .Returns(_fixture.MockUserRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _getAuthTokenHandler.HandleAsync(query));
    }

    [Fact]
    public async Task HandleAsync_UserNotVerified_UserNotVerifiedExceptionThrown()
    {
        // Arrange
        var query = new GetAuthTokenQuery { Email = "test@email.com", Password = "password" };
        var user = new User { Id = 1, Email = query.Email };

        _fixture.MockUserRepository.Setup(u => u.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync(user);
        _fixture.MockUserVerificationRepository.Setup(u => u.GetAsync(It.IsAny<VerificationCodeFilter>()))
            .ReturnsAsync((VerificationCode)null);
        _fixture.MockUnitOfWork.Setup(uow => uow.Users)
            .Returns(_fixture.MockUserRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotVerifiedException>(() => _getAuthTokenHandler.HandleAsync(query));
    }

    [Fact]
    public async Task HandleAsync_InvalidPassword_UnauthorizedAccessExceptionThrown()
    {
        // Arrange
        var query = new GetAuthTokenQuery
            { Email = "test@email.com", Password = "password", AuthProviderType = AuthProviderType.SimpleAuth };
        var user = new User { Id = 1, Email = query.Email };

        _fixture.MockUserRepository.Setup(u => u.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync(user);
        _fixture.MockUserVerificationRepository.Setup(u => u.GetAsync(It.IsAny<VerificationCodeFilter>()))
            .ReturnsAsync(new VerificationCode { VerifiedAt = DateTime.Now });
        _fixture.MockUnitOfWork.Setup(uow => uow.Users)
            .Returns(_fixture.MockUserRepository.Object);
        _fixture.MockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), 
            It.IsAny<string>())).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _getAuthTokenHandler.HandleAsync(query));
    }
}