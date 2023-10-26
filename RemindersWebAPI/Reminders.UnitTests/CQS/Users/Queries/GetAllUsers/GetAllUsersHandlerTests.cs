using Reminders.BLL.CQS.Users.Queries.GetAllUsers;

namespace Reminders.UnitTests.CQS.Users.Queries.GetAllUsers;

public class GetAllUsersHandlerTests : IClassFixture<UserHandlerFixture>
{
    private readonly UserHandlerFixture _fixture;
    private readonly GetAllUsersHandler _getAllUsersHandler;

    public GetAllUsersHandlerTests(UserHandlerFixture fixture)
    {
        _fixture = fixture;
        _getAllUsersHandler = new GetAllUsersHandler(_fixture.MockUnitOfWork.Object, _fixture.MockMapper.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidQuery_ReturnsUserDtoList()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1 },
            new User { Id = 2 }
        };
        var query = new GetAllUsersQuery();
        var userDtos = new List<UserDto>();

        _fixture.MockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
        _fixture.MockMapper.Setup(mapper => mapper.Map<List<User>, List<UserDto>>(It.IsAny<List<User>>()))
            .Returns(userDtos);

        // Act
        var result = await _getAllUsersHandler.HandleAsync(query);

        // Assert
        Assert.Equal(userDtos, result);
    }

    [Fact]
    public async Task HandleAsync_NoUsers_ReturnsEmptyList()
    {
        // Arrange
        var users = new List<User>();
        var query = new GetAllUsersQuery();

        _fixture.MockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _getAllUsersHandler.HandleAsync(query);

        // Assert
        Assert.Empty(result);
    }
}