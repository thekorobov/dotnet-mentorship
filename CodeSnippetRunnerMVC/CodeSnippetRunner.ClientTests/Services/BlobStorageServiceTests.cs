namespace CodeSnippetRunner.ClientTests.Services;

public class BlobStorageServiceTests
{
    private readonly BlobStorageService _blobStorageService;

    public BlobStorageServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<BlobStorageServiceTests>()
            .Build();
        var connectionString = configuration["AzuriteConnectionString"];
        _blobStorageService = new BlobStorageService(connectionString!);
    }

    [Fact]
    public async Task UploadBlobAsync_ValidData_ShouldUploadBlob()
    {
        // Arrange
        var blobName = "testBlob";
        var content = "Test Content";

        // Act
        var blobClient = await _blobStorageService.UploadBlobAsync(blobName, content);

        // Assert
        Assert.NotNull(blobClient);
        Assert.Equal(blobName, blobClient.Name);

        await _blobStorageService.DeleteBlobAsync(blobName);
    }
    
    [Fact]
    public async Task DownloadBlobAsync_ExistingBlob_ShouldReturnData()
    {
        // Arrange
        var blobName = "testBlob";
        var content = "Test Content";
        await _blobStorageService.UploadBlobAsync(blobName, content);
        
        // Act
        var stream = await _blobStorageService.DownloadBlobAsync(blobName);
        
        // Assert
        Assert.NotNull(stream);
        
        await _blobStorageService.DeleteBlobAsync(blobName);
    }

    [Fact]
    public async Task DownloadBlobAsync_NonExistingBlob_ShouldReturnNull()
    {
        // Act & Assert
        var stream = await _blobStorageService.DownloadBlobAsync("nonExistingBlob");
        Assert.Null(stream);
    }

    [Fact]
    public async Task DeleteBlobAsync_ExistingBlob_ShouldDeleteBlob()
    {
        // Arrange
        var blobName = "testBlobDelete";
        var content = "Test Content";
        await _blobStorageService.UploadBlobAsync(blobName, content);
        
        // Act
        await _blobStorageService.DeleteBlobAsync(blobName);
        
        // Assert
        var stream = await _blobStorageService.DownloadBlobAsync("nonExistingBlob");
        Assert.Null(stream);
    }

    [Fact]
    public async Task GetListOfBlobNamesAsync_BlobsExist_ShouldReturnList()
    {
        // Arrange
        var blobName1 = "testBlob1";
        var blobName2 = "testBlob2";
        await _blobStorageService.UploadBlobAsync(blobName1, "Test Content");
        await _blobStorageService.UploadBlobAsync(blobName2, "Test Content");
        
        // Act
        var blobNames = await _blobStorageService.GetListOfBlobNamesAsync();
        
        // Assert
        Assert.Contains(blobName1, blobNames);
        Assert.Contains(blobName2, blobNames);
        blobNames.Count().ShouldBe(2);
        
        await _blobStorageService.DeleteBlobAsync(blobName1);
        await _blobStorageService.DeleteBlobAsync(blobName2);
    }

    [Fact]
    public async Task GenerateSasToken_ValidBlob_ShouldReturnSasToken()
    {
        // Arrange
        var blobName = "testBlob";
        await _blobStorageService.UploadBlobAsync(blobName, "Test Content");
        
        // Act
        var sasToken = _blobStorageService.GenerateSasToken(blobName);
        
        // Assert
        Assert.NotNull(sasToken);
        
        await _blobStorageService.DeleteBlobAsync(blobName);
    }
}