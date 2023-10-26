namespace TicketFlow.Persistence.UnitTests.Repositories;

public class UserRepositoryTests : IClassFixture<RepositoriesFixture>
{
    private readonly RepositoriesFixture _fixture;
    public UserRepositoryTests(RepositoriesFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateAsync_ValidUser_UserCreated()
    {
        // Arrange
        var user = new User
        {
            Id = UserConstants.User1Id,
            Role = UserConstants.User1Role,
            Email = UserConstants.User1Email,
            Forename = UserConstants.User1Forename,
            Surname = UserConstants.User1Surname,
            UserName = UserConstants.User1UserName,
            PasswordHash = UserConstants.User1PasswordHash,
            AuthProviderType = UserConstants.User1AuthProviderType
        };

        // Act
        var userId = await _fixture.UnitOfWork.Users.CreateAsync(user);

        // Assert
        Assert.NotNull(userId);
        Assert.Equal(user.Id, userId);
    }
    
    [Fact]
    public async Task CreateAsync_NullUser_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.UnitOfWork.Users.CreateAsync(null));
        Assert.Equal("user", exception.ParamName);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidUser_UserUpdated()
    {
        // Arrange
        var newEmail = "new@gmail.com";
        var user = new User
        {
            Id = UserConstants.User2Id,
            Role = UserConstants.User2Role,
            Email = UserConstants.User2Email,
            Forename = UserConstants.User2Forename,
            Surname = UserConstants.User2Surname,
            UserName = UserConstants.User2UserName,
            PasswordHash = UserConstants.User2PasswordHash,
            AuthProviderType = UserConstants.User2AuthProviderType
        };
        await _fixture.UnitOfWork.Users.CreateAsync(user);
        user.Email = newEmail;

        // Act
        await _fixture.UnitOfWork.Users.UpdateAsync(user);

        // Assert
        var updatedUser = await _fixture.Context.Users.FindAsync(user.Id);
        Assert.Equal(newEmail, updatedUser!.Email);
    }
    
    [Fact]
    public async Task UpdateAsync_NullUser_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>
            (() => _fixture.UnitOfWork.Users.UpdateAsync(null));
        Assert.Equal("user", exception.ParamName);
    }

    [Fact]
    public async Task UpdateAsync_UserNotFound_EntityNotFoundException()
    {
        // Arrange
        var nonExistingUserId = UserConstants.NonExistingUserId;
        var user = new User { Id = nonExistingUserId };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _fixture.UnitOfWork.Users.UpdateAsync(user));
        Assert.Equal($"Entity <User> ({nonExistingUserId}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task GetAsync_ById_UserExists_UserReturned()
    {
        // Arrange
        var user = new User
        {
            Id = UserConstants.User3Id,
            Role = UserConstants.User3Role,
            Email = UserConstants.User3Email,
            Forename = UserConstants.User3Forename,
            Surname = UserConstants.User3Surname,
            UserName = UserConstants.User3UserName,
            PasswordHash = UserConstants.User3PasswordHash,
            AuthProviderType = UserConstants.User3AuthProviderType
        };
        await _fixture.UnitOfWork.Users.CreateAsync(user);

        // Act
        var foundUser = await _fixture.UnitOfWork.Users.GetAsync(new UserFilter { Id = user.Id });

        // Assert
        Assert.NotNull(foundUser);
        Assert.Equal(user.Id, foundUser.Id);
    }
    
    [Fact]
    public async Task GetAsync_ByEmail_UserExists_UserReturned()
    {
        // Arrange
        var user = new User
        {
            Id = UserConstants.User4Id,
            Role = UserConstants.User4Role,
            Email = UserConstants.User4Email,
            Forename = UserConstants.User4Forename,
            Surname = UserConstants.User4Surname,
            UserName = UserConstants.User4UserName,
            PasswordHash = UserConstants.User4PasswordHash,
            AuthProviderType = UserConstants.User4AuthProviderType
        };
        await _fixture.UnitOfWork.Users.CreateAsync(user);

        // Act
        var foundUser = await _fixture.UnitOfWork.Users.GetAsync(new UserFilter { Email = user.Email });

        // Assert
        Assert.NotNull(foundUser);
        Assert.Equal(user.Email, foundUser.Email);
    }
    
    [Fact]
    public async Task GetAsync_InvalidId_NullReturned()
    {
        // Act
        var foundUser = await _fixture.UnitOfWork.Users.GetAsync(new UserFilter { Id = UserConstants.NonExistingUserId });

        // Assert
        Assert.Null(foundUser);
    }
    
    [Fact]
    public async Task GetAsync_InvalidEmail_NullReturned()
    {
        // Act
        var foundUser = await _fixture.UnitOfWork.Users.GetAsync(new UserFilter { Email = UserConstants.NonExistingEmail });

        // Assert
        Assert.Null(foundUser);
    }
    
    [Fact]
    public async Task GetAllAsync_UsersExist_UsersReturned()
    {
        // Arrange
        // Act
        var allUsers = await _fixture.UnitOfWork.Users.GetAllAsync(new UserFilter());

        // Assert
        Assert.NotNull(allUsers);
        Assert.Equal(8, allUsers.Count());
    }
    
    [Fact]
    public async Task DeleteAsync_UserExists_UserDeleted()
    {
        // Arrange
        var userId = UserConstants.User5Id;
        var user = new User
        {
            Id = userId,
            Role = UserConstants.User5Role,
            Email = UserConstants.User5Email,
            Forename = UserConstants.User5Forename,
            Surname = UserConstants.User5Surname,
            UserName = UserConstants.User5UserName,
            PasswordHash = UserConstants.User5PasswordHash,
            AuthProviderType = UserConstants.User5AuthProviderType
        };
        await _fixture.UnitOfWork.Users.CreateAsync(user);
        
        // Act
        await _fixture.UnitOfWork.Users.DeleteAsync(userId);

        // Assert
        var deletedUser = await _fixture.Context.Users.FindAsync(userId);
        Assert.Null(deletedUser);
    }
    
    [Fact]
    public async Task DeleteAsync_NonExistingUser_EntityNotFoundException()
    {
        // Arrange
        var nonExistingUserId = UserConstants.NonExistingUserId;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _fixture.UnitOfWork.Users.DeleteAsync(nonExistingUserId)
        );
        Assert.Equal($"Entity <User> ({nonExistingUserId}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task DeleteAsync_NullUserId_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _fixture.UnitOfWork.Users.DeleteAsync(null)
        );
        Assert.Equal("id", exception.ParamName);
    }
    
    [Fact]
    public void GetQueryable_ShouldReturnQueryable()
    {
        // Arrange
        // Act
        var queryable = _fixture.UnitOfWork.Users.GetQueryable();

        // Assert
        Assert.NotNull(queryable);
    }
}