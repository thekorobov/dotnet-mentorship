using CodeSnippetRunner.CodeExecutorMicroservice.Services;
using CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;

namespace CodeSnippetRunner.MicroserviceTests.ServicesTests
{
    public class BlobServiceTests
    {
        public IBlobStorageService BlobStorageService { get; set; }
        public BlobServiceTests()
        {
            BlobStorageService = new BlobStorageService();
        }

        [Fact]
        public async Task ReadCodeAsync_ValidInput_SuccessfulResult()
        {
            //Arrange
            string blobName = "test.txt";
            string sasToken = "http://127.0.0.1:10000/devstoreaccount1/snippet-container/test.txt";
            
            //Act
            var result = await BlobStorageService.ReadCodeAsync(blobName, sasToken);
            
            //Assert
            Assert.True(!string.IsNullOrWhiteSpace(result));
            Assert.Contains("Console.WriteLine(2+2);", result);
        }
        
        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("test.txt", "")]
        // string.Empty raises a compile error, so I use "" here
        public async Task ReadCodeAsync_EmptyOrNullInput_ThrowsArgumentException(string blobName, string sasToken)
        {
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                            await BlobStorageService.ReadCodeAsync(blobName, sasToken));
            Assert.IsType<ArgumentException>(exception);
            Assert.Contains("BlobFileName and SasToken are required.", exception.Message);
        }
        
        [Theory]
        [InlineData("snippet-container/test.txt")]    
        [InlineData("127.0.0.1:10000/devstoreaccount1/snippet-container/test.txt")]      
        public async Task ReadCodeAsync_WrongTokenInput_ThrowsArgumentException(string sasToken)
        {
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                            await BlobStorageService.ReadCodeAsync("test.txt", sasToken));
            Assert.IsType<ArgumentException>(exception);
            Assert.Contains("Invalid SasToken.", exception.Message);
        }

    }
}
