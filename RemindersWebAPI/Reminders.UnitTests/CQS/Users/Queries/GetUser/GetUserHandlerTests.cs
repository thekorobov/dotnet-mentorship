using Reminders.BLL.CQS.Users.Queries.GetUserById;

namespace Reminders.UnitTests.CQS.Users.Queries.GetUser;

public class GetUserHandlerTests : IClassFixture<UserHandlerFixture>
{
    private readonly UserHandlerFixture _fixture;
    private readonly GetUserHandler _getUserHandler;

    public GetUserHandlerTests(UserHandlerFixture fixture)
    {
        _fixture = fixture;
        _getUserHandler = new GetUserHandler(_fixture.MockUnitOfWork.Object, _fixture.MockMapper.Object);
    }

    [Fact]
    public async Task HandleAsync_UserFoundById_ReturnsUserDto()
    {
        // Arrange
        var query = new GetUserQuery { Id = Constants.ValidId };
        var user = new User { Id = Constants.ValidId };

        _fixture.MockUnitOfWork.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync(user);
        _fixture.MockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto());

        // Act
        var result = await _getUserHandler.HandleAsync(query);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task HandleAsync_UserFoundByEmail_ReturnsUserDto()
    {
        // Arrange
        var query = new GetUserQuery { Email = Constants.ValidEmail };
        var user = new User { Email = Constants.ValidEmail };

        _fixture.MockUnitOfWork.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync(user);
        _fixture.MockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto());

        // Act
        var result = await _getUserHandler.HandleAsync(query);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task HandleAsync_UserNotFoundById_EntityNotFoundExceptionThrown()
    {
        // Arrange
        var query = new GetUserQuery { Id = Constants.ValidId, AuthProviderType = Constants.ValidAuthProvider };

        _fixture.MockUnitOfWork.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _getUserHandler.HandleAsync(query));
    }

    [Fact]
    public async Task HandleAsync_UserNotFoundByEmail_EntityNotFoundExceptionThrown()
    {
        // Arrange
        var query = new GetUserQuery { Email = Constants.ValidEmail, AuthProviderType = Constants.ValidAuthProvider };

        _fixture.MockUnitOfWork.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>())).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _getUserHandler.HandleAsync(query));
    }
}