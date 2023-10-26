using Reminders.BLL.CQS.Users.Commands.CreateUser;
using Reminders.BLL.CQS.Users.Queries.GetAuthToken;
using Reminders.BLL.CQS.Users.Queries.GetUserById;

namespace Reminders.UnitTests.Services;

public class GoogleAuthServiceTests
{
    [Fact]
    public async Task HandleGoogleLoginAsync_UserExists_GenerateTokenCalled()
    {
        // Arrange
        var mockMediator = new Mock<IMediator>();
        mockMediator.Setup(m => m.SendQueryAsync<GetUserQuery, UserDto>(It.IsAny<GetUserQuery>()))
            .ReturnsAsync(new UserDto { Id = 1, Email = "test@test.com" });
        mockMediator.Setup(m => m.SendQueryAsync<GetAuthTokenQuery, string>(It.IsAny<GetAuthTokenQuery>()))
            .ReturnsAsync("some_token");

        var service = new GoogleAuthService(mockMediator.Object);

        // Act
        var token = await service.HandleGoogleLoginAsync("test@test.com", "Test User");

        // Assert
        mockMediator.Verify(m => m.SendCommandAsync<CreateUserCommand, int>(It.IsAny<CreateUserCommand>()), Times.Never);
        mockMediator.Verify(m => m.SendQueryAsync<GetAuthTokenQuery, string>(It.IsAny<GetAuthTokenQuery>()), Times.Once);
        Assert.Equal("some_token", token);
    }
}