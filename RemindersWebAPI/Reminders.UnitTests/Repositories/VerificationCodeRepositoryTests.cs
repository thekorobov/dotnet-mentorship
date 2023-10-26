namespace Reminders.UnitTests.Repositories;

public class VerificationCodeRepositoryTests : IClassFixture<RepositoriesFixture>
{
    private readonly RepositoriesFixture _fixture;
    private readonly ApplicationDbContext _context;
    
    public VerificationCodeRepositoryTests(RepositoriesFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.Context;
    }

    [Fact]
    public async Task CreateAsync_ValidEntity_EntityCreated()
    {
        // Arrange
        var verificationCode = new VerificationCode { UserId = 1, VerificationToken = Constants.ValidVerificationToken1 };

        // Act
        int createdId = await _fixture.VerificationCodeRepository.CreateAsync(verificationCode);

        // Assert
        Assert.NotEqual(0, createdId);

        var savedEntity = await _fixture.Context.VerificationCodes.FindAsync(createdId);
        Assert.NotNull(savedEntity);
        Assert.Equal(Constants.ValidVerificationToken1, savedEntity.VerificationToken);
    }
    
    [Fact]
    public async Task CreateAsync_NullEntity_ZeroReturned()
    {
        // Act
        int createdId = await _fixture.VerificationCodeRepository.CreateAsync(null);

        // Assert
        Assert.Equal(0, createdId);
    }

    [Fact]
    public async Task UpdateAsync_ValidEntity_EntityUpdated()
    {
        // Arrange
        var verificationCode = new VerificationCode { UserId = 2, VerificationToken = Constants.ValidVerificationToken1 };
        await _fixture.VerificationCodeRepository.CreateAsync(verificationCode);
        verificationCode.VerificationToken = Constants.ValidUpdateVerificationToken;

        // Act
        await _fixture.VerificationCodeRepository.UpdateAsync(verificationCode);

        // Assert
        var updatedEntity = await _context.VerificationCodes
            .FirstOrDefaultAsync(uv => uv.UserId == 2);
        Assert.Equal(Constants.ValidUpdateVerificationToken, updatedEntity.VerificationToken);
    }

    [Fact]
    public async Task UpdateAsync_NullEntity_NoUpdate()
    {
        // Arrange
        var verificationCode = new VerificationCode { UserId = Constants.InvalidUserId, VerificationToken = Constants.ValidVerificationToken1 };

        // Act
        await _fixture.VerificationCodeRepository.UpdateAsync(verificationCode);  

        // Assert
        var notUpdatedEntity = await _context.VerificationCodes.FirstOrDefaultAsync(uv => uv.UserId == Constants.InvalidUserId);
        Assert.Null(notUpdatedEntity);
    }
    
    [Fact]
    public async Task GetAsync_ByUserId_EntityExists_EntityReturned()
    {
        // Arrange
        var verificationCode = new VerificationCode { UserId = 3, VerificationToken = Constants.ValidVerificationToken1 };
        await _fixture.VerificationCodeRepository.CreateAsync(verificationCode);

        // Act
        var foundEntity = await _fixture.VerificationCodeRepository
            .GetAsync(new VerificationCodeFilter { UserId = 3 });

        // Assert
        Assert.NotNull(foundEntity);
        Assert.Equal(Constants.ValidVerificationToken1, foundEntity.VerificationToken);
    }
    
    [Fact]
    public async Task GetAsync_InvalidUserId_NullReturned()
    {
        // Act
        var foundEntity = await _fixture.VerificationCodeRepository.GetAsync(new VerificationCodeFilter { UserId = Constants.InvalidUserId });

        // Assert
        Assert.Null(foundEntity);
    }
    
    [Fact]
    public async Task GetAsync_ByVerificationCode_EntityExists_EntityReturned()
    {
        // Arrange
        var verificationCode = new VerificationCode { UserId = 4, VerificationToken = Constants.ValidVerificationToken2 };
        await _fixture.VerificationCodeRepository.CreateAsync(verificationCode);

        // Act
        var foundEntity = await _fixture.VerificationCodeRepository
            .GetAsync(new VerificationCodeFilter { VerificationToken = Constants.ValidVerificationToken2 });

        // Assert
        Assert.NotNull(foundEntity);
        Assert.Equal(4, foundEntity.UserId);
    }
    
    [Fact]
    public async Task GetAsync_InvalidVerificationCode_NullReturned()
    {
        // Act
        var foundEntity = await _fixture.VerificationCodeRepository.GetAsync(new VerificationCodeFilter { VerificationToken = "invalidcode" });

        // Assert
        Assert.Null(foundEntity);
    }
    
    [Fact]
    public async Task DeleteAsync_NotImplemented_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => _fixture.VerificationCodeRepository.DeleteAsync(1));
    }

    [Fact]
    public async Task GetAllAsync_VerificationCodesExist_VerificationCodesReturned()
    {
        // Arrange
        var verificationCode = new VerificationCode { UserId = 10, VerificationToken = Constants.ValidVerificationToken1 };

        // Act
        await _fixture.VerificationCodeRepository.CreateAsync(verificationCode);
        var allVerificationCodes = await _fixture.VerificationCodeRepository.GetAllAsync();

        // Assert
        Assert.NotNull(allVerificationCodes);
        Assert.Equal(2, allVerificationCodes.Count());
    }
    
    [Fact]
    public void GetQueryable_ReturnsExpectedQueryable()
    {
        // Arrange
        var repository = new VerificationCodeRepository(_fixture.Context);

        // Act
        var actualCodes = repository.GetQueryable().ToList();

        // Assert
        Assert.Equal(4, actualCodes.Count);
    }
}