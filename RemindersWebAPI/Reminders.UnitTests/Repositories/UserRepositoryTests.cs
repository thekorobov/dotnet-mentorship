namespace Reminders.UnitTests.Repositories;

public class UserRepositoryTests : IClassFixture<RepositoriesFixture>
{
    private readonly RepositoriesFixture _fixture;
    private readonly ApplicationDbContext _context;
    
    public UserRepositoryTests(RepositoriesFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.Context;
    }
    
    [Fact]
    public async Task CreateAsync_ValidUser_UserCreated()
    {
        // Arrange
        var user = new User { Id = 3, Email = Constants.ValidEmail, Role = "User" };

        // Act
        await _fixture.UserRepository.CreateAsync(user);

        // Assert
        var savedUser = await _fixture.Context.Users.FindAsync(3);
        Assert.NotNull(savedUser);
        Assert.Equal(Constants.ValidEmail, savedUser.Email);
    }
    
    [Fact]
    public async Task CreateAsync_NullUser_ZeroReturned()
    {
        // Act
        int createdId = await _fixture.UserRepository.CreateAsync(null);

        // Assert
        Assert.Equal(0, createdId);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidUser_UserUpdated()
    {
        // Arrange
        var user = new User { Id = 4, Email = Constants.ValidEmail, Role = "User" };
        await _fixture.UserRepository.CreateAsync(user);
        user.Email = "new@gmail.com";

        // Act
        await _fixture.UserRepository.UpdateAsync(user);

        // Assert
        var updatedUser = await _fixture.Context.Users.FindAsync(4);
        Assert.Equal("new@gmail.com", updatedUser.Email);
    }

    [Fact]
    public async Task UpdateAsync_NullUser_NoUpdate()
    {
        // Arrange
        var user = new User { Id = Constants.NonExistingUserId, Email = "doesntmatter@gmail.com", Role = "User" };

        // Act
        await _fixture.UserRepository.UpdateAsync(user);

        // Assert
        var notUpdatedUser = await _context.Users.FindAsync(Constants.NonExistingUserId);
        Assert.Null(notUpdatedUser);
    }
    
    [Fact]
    public async Task GetAsync_ById_UserExists_UserReturned()
    {
        // Arrange
        var user = new User { Id = 5, Email = Constants.ValidEmail, Role = "User" };
        await _fixture.UserRepository.CreateAsync(user);

        // Act
        var foundUser = await _fixture.UserRepository.GetAsync(new UserFilter { Id = 5 });

        // Assert
        Assert.NotNull(foundUser);
        Assert.Equal(Constants.ValidEmail, foundUser.Email);
    }
    
    [Fact]
    public async Task GetAsync_InvalidId_NullReturned()
    {
        // Act
        var foundUser = await _fixture.UserRepository.GetAsync(new UserFilter { Id = Constants.NonExistingUserId });

        // Assert
        Assert.Null(foundUser);
    }
    
    [Fact]
    public async Task GetAsync_ByEmail_UserExists_UserReturned()
    {
        // Arrange
        var user = new User { Id = 6, Email = Constants.ValidEmail, Role = "User" };
        await _fixture.UserRepository.CreateAsync(user);

        // Act
        var foundUser = await _fixture.UserRepository.GetAsync(new UserFilter { Email = Constants.ValidEmail });

        // Assert
        Assert.NotNull(foundUser);
        Assert.Equal(Constants.ValidEmail, foundUser.Email);
    }

    [Fact]
    public async Task GetAsync_InvalidEmail_NullReturned()
    {
        // Act
        var foundUser = await _fixture.UserRepository.GetAsync(new UserFilter { Email = "invalidemail@gmail.com" });

        // Assert
        Assert.Null(foundUser);
    }
    
    [Fact]
    public async Task GetAllAsync_UsersExist_UsersReturned()
    {
        // Arrange
        // Act
        var allUsers = await _fixture.UserRepository.GetAllAsync();

        // Assert
        Assert.NotNull(allUsers);
        Assert.Equal(4, allUsers.Count());
    }

    [Fact]
    public async Task DeleteAsync_UserExists_UserDeleted()
    {
        // Arrange
        // Act
        await _fixture.UserRepository.DeleteAsync(1);

        // Assert
        var deletedUser = await _fixture.Context.Users.FindAsync(1);
        Assert.Null(deletedUser);
    }
    
    [Fact]
    public async Task DeleteAsync_NonExistingUser_NoDeletion()
    {
        // Arrange
        // Act
        await _fixture.UserRepository.DeleteAsync(Constants.NonExistingUserId);

        // Assert
        var notDeletedUser = await _context.Users.FindAsync(Constants.NonExistingUserId);
        Assert.Null(notDeletedUser);
    }
}