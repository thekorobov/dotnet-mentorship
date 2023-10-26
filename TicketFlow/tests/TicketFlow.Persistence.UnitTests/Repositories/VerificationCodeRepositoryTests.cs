namespace TicketFlow.Persistence.UnitTests.Repositories;

public class VerificationCodeRepositoryTests : IClassFixture<RepositoriesFixture>
{
    private readonly RepositoriesFixture _fixture;
    public VerificationCodeRepositoryTests(RepositoriesFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task CreateAsync_ValidVerificationCode_VerificationCodeCreated()
    {
        // Arrange
        var verificationCodeId = VerificationCodeConstants.VerificationCodeId1;
        var verificationToken = VerificationCodeConstants.VerificationToken1;
        var verificationCode = new VerificationCode
        {
            Id = verificationCodeId,
            UserId = UserConstants.User6Id, 
            VerificationToken = verificationToken
        };

        // Act
        var createdId = await _fixture.UnitOfWork.VerificationCodes.CreateAsync(verificationCode);

        // Assert
        var savedEntity = await _fixture.Context.VerificationCodes.FindAsync(createdId);
        Assert.NotNull(savedEntity);
        Assert.Equal(verificationCodeId, savedEntity.Id);
        Assert.Equal(verificationToken, savedEntity.VerificationToken);
    }
    
    [Fact]
    public async Task CreateAsync_NullVerificationCode_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _fixture.UnitOfWork.VerificationCodes.CreateAsync(null));
        Assert.Equal("verificationCode", exception.ParamName);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidVerificationCode_VerificationCodeUpdated()
    {
        // Arrange
        var verificationCodeId = VerificationCodeConstants.VerificationCodeId2;
        var verificationToken = VerificationCodeConstants.VerificationToken2;
        var updatedVerificationToken = "newCode";
        
        var verificationCode = new VerificationCode
        {
            Id = verificationCodeId,
            UserId = UserConstants.User7Id, 
            VerificationToken = verificationToken
        };
        await _fixture.UnitOfWork.VerificationCodes.CreateAsync(verificationCode);
        verificationCode.VerificationToken = updatedVerificationToken;
        
        // Act
        await _fixture.UnitOfWork.VerificationCodes.UpdateAsync(verificationCode);

        // Assert
        var savedEntity = await _fixture.Context.VerificationCodes.FindAsync(verificationCodeId);
        Assert.NotNull(savedEntity);
        Assert.Equal(verificationCodeId, savedEntity.Id);
        Assert.Equal(updatedVerificationToken, savedEntity.VerificationToken);
    }
    
    [Fact]
    public async Task UpdateAsync_NullVerificationCode_ArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>
            (() => _fixture.UnitOfWork.VerificationCodes.UpdateAsync(null));
        Assert.Equal("verificationCode", exception.ParamName);
    }

    [Fact]
    public async Task UpdateAsync_VerificationCodeNotFound_EntityNotFoundException()
    {
        // Arrange
        var nonExistingVerificationCodeId = VerificationCodeConstants.NonExistingVerificationCodeId;
        var verificationCode = new VerificationCode { Id = nonExistingVerificationCodeId };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>
            (() => _fixture.UnitOfWork.VerificationCodes.UpdateAsync(verificationCode));
        Assert.Equal($"Entity <VerificationCode> ({nonExistingVerificationCodeId}) not found.", exception.Message);
    }
    
    [Fact]
    public async Task GetAsync_ByUserId_VerificationCodeExists()
    {
        // Arrange
        var verificationCodeId = VerificationCodeConstants.VerificationCodeId3;
        var verificationToken = VerificationCodeConstants.VerificationToken3;
        var userId = UserConstants.User8Id;
        
        var verificationCode = new VerificationCode
        {
            Id = verificationCodeId,
            UserId = userId, 
            VerificationToken = verificationToken
        };
        await _fixture.UnitOfWork.VerificationCodes.CreateAsync(verificationCode);

        // Act
        var foundVerificationCode = await _fixture.UnitOfWork.VerificationCodes.GetAsync(
            new VerificationCodeFilter { UserId = userId });

        // Assert
        Assert.NotNull(foundVerificationCode);
        Assert.Equal(verificationCodeId, foundVerificationCode.Id);
        Assert.Equal(userId, foundVerificationCode.UserId);
        Assert.Equal(verificationToken, foundVerificationCode.VerificationToken);
    }
    
    [Fact]
    public async Task GetAsync_InvalidId_NullReturned()
    {
        // Act
        var verificationCode = await _fixture.UnitOfWork.VerificationCodes.GetAsync(
            new VerificationCodeFilter { Id = VerificationCodeConstants.NonExistingVerificationCodeId });

        // Assert
        Assert.Null(verificationCode);
    }
    
    [Fact]
    public async Task GetAllAsync_UsersExist_UsersReturned()
    {
        // Arrange
        var userId = UserConstants.User9Id;
        var verificationCodeId = VerificationCodeConstants.VerificationCodeId4;
        var verificationToken = VerificationCodeConstants.VerificationToken4;
        var verificationCode = new VerificationCode
        {
            Id = verificationCodeId,
            UserId = userId, 
            VerificationToken = verificationToken
        };
        await _fixture.UnitOfWork.VerificationCodes.CreateAsync(verificationCode);
        
        // Act
        var allVerificationCodes = await _fixture.UnitOfWork.VerificationCodes
            .GetAllAsync(new VerificationCodeFilter());

        // Assert
        Assert.NotNull(allVerificationCodes);
        Assert.Single(allVerificationCodes);
    }
    
    [Fact]
    public async Task DeleteAsync_NotImplementedException()
    {
        // Arrange
        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotImplementedException>(
            () => _fixture.UnitOfWork.VerificationCodes.DeleteAsync("userId")
        );
    }
    
    [Fact]
    public void GetQueryable_NotImplementedException()
    {
        // Arrange
        // Act & Assert
        Assert.Throws<NotImplementedException>(() => _fixture.UnitOfWork.VerificationCodes.GetQueryable());
    }
}