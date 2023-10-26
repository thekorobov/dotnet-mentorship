using System.Text;
using CodeSnippetRunner.Client.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CodeSnippetRunner.ClientTests.Services;

public class FileServiceTests
{
    private readonly IFileService _fileService;

    public FileServiceTests()
    {
        _fileService = new FileService();
    }

    [Fact]
    public void IsValidFile_NullFile_ShouldReturnFalse()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = _fileService.IsValidFile(file);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(10241, false)]
    [InlineData(1024, true)]
    public void IsValidFile_VariousFileSizes_ShouldReturnExpectedResult(long length, bool expected)
    {
        // Arrange
        var file = new FormFile(new MemoryStream(new byte[length]), 0, length, "data", "test.txt");

        // Act
        var result = _fileService.IsValidFile(file);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task ReadFileContents_ValidFile_ShouldReturnContent()
    {
        // Arrange
        var content = "Hello World!";
        var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(content)), 0, content.Length, "data",
            "dummy.txt");

        // Act
        var result = await _fileService.ReadFileContents(file);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(content, result.InputCode);
    }

    [Fact]
    public async Task ValidateFile_InvalidFile_ShouldReturnError()
    {
        // Arrange
        var file = new FormFile(new MemoryStream(new byte[0]), 0, 0, "data", "test.txt");

        // Act
        var result = await _fileService.ValidateFile(file);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Wrong file", result.ErrorMessage);
    }

    [Fact]
    public void WriteCodeToFile_ValidContent_ShouldReturnFileDownloadResult()
    {
        // Arrange
        var content = "Hello World!";

        // Act
        var result = _fileService.WriteCodeToFile(content);

        // Assert
        Assert.Equal(Encoding.UTF8.GetBytes(content), result.ContentBytes);
        Assert.Equal("text/plain", result.ContentType);
        Assert.Equal("code.txt", result.FileName);
    }
}