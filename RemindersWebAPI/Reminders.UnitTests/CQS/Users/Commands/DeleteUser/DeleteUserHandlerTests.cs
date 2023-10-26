using Reminders.BLL.CQS.Users.Commands.DeleteUser;

namespace Reminders.UnitTests.CQS.Users.Commands.DeleteUser;

public class DeleteUserHandlerTests : IClassFixture<UserHandlerFixture>
{
    private readonly DeleteUserHandler _deleteUserHandler;
    private readonly UserHandlerFixture _fixture;

    public DeleteUserHandlerTests(UserHandlerFixture fixture)
    {
        _fixture = fixture;
        _deleteUserHandler = new DeleteUserHandler(_fixture.MockUnitOfWork.Object);
    }
    
    [Fact]
    public async Task HandleAsync_AdminAndUserExists_UserDeleted()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = 1, CurrentUserId = 2 };

        _fixture.MockUnitOfWork.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync(new User { Role = UserRoles.Admin });

        // Act
        await _deleteUserHandler.HandleAsync(command);

        // Assert
        _fixture.MockUnitOfWork.Verify(u => u.Users.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_UserDoesNotExist_EntityNotFoundExceptionThrown()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = 1, CurrentUserId = 2 };

        _fixture.MockUnitOfWork.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _deleteUserHandler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_NotAdmin_UnauthorizedAccessExceptionThrown()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = 1, CurrentUserId = 2 };

        _fixture.MockUnitOfWork.Setup(u => u.Users.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync(new User { Role = UserRoles.User });

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _deleteUserHandler.HandleAsync(command));
    }
}