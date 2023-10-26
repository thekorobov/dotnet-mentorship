using CodeSnippetRunner.Client.Utils;

namespace CodeSnippetRunner.ClientTests.Utils;

public class FileNameHelperTests
{
    [Fact]
    public void ModifyFileName_EmptyString_ReturnsGuidWithTxtExtension()
    {
        // Arrange
        // Act
        var result = FileNameHelper.ModifyFileName();

        // Assert
        Assert.Matches(@"^[a-f0-9]{8}-[a-f0-9]{4}\.txt$", result);
    }

    [Fact]
    public void ModifyFileName_CsExtension_ReturnsGuidWithCsExtension()
    {
        // Arrange
        var originalName = "original.cs";

        // Act
        var result = FileNameHelper.ModifyFileName(originalName);

        // Assert
        Assert.Matches(@"^original_[a-zA-Z0-9]{3}\.cs$", result);
    }

    [Fact]
    public void ModifyFileName_TxtExtension_ReturnsGuidWithTxtExtension()
    {
        // Arrange
        var originalName = "original.txt";

        // Act
        var result = FileNameHelper.ModifyFileName(originalName);

        // Assert
        Assert.Matches(@"^original_[a-zA-Z0-9]{3}\.txt$", result);
    }

    [Fact]
    public void ModifyFileName_ShortFileName_ReturnsFileNameWithRandomChars()
    {
        // Arrange
        var originalName = "short.txt";

        // Act
        var result = FileNameHelper.ModifyFileName(originalName);

        // Assert
        Assert.Matches(@"^short_[a-zA-Z0-9]{3}\.txt$", result);
    }

    [Fact]
    public void ModifyFileName_NoExtension_ReturnsUndefinedBehavior()
    {
        // Arrange
        var originalName = "filename";

        // Act
        var result = FileNameHelper.ModifyFileName(originalName);

        // Assert
        Assert.Matches(@"^filename_[a-zA-Z0-9]{3}\.txt$", result);
    }
}