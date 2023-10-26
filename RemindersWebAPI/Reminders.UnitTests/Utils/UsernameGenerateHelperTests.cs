using Reminders.BLL.Utils;

namespace Reminders.UnitTests.Utils;

public class UsernameGenerateHelperTests
{
    [Fact]
    public void GenerateUsername_SpecialCharsInInput_CleansSpecialChars()
    {
        // Arrange
        var username = "user!@#";
            
        // Act
        var generatedUsername = UsernameGenerateHelper.GenerateUsername(username);
            
        // Assert
        Assert.True(generatedUsername.StartsWith("user_"));
    }

    [Fact]
    public void GenerateUsername_SimpleUsername_AppendsRandomString()
    {
        // Arrange
        var username = "user";
            
        // Act
        var generatedUsername = UsernameGenerateHelper.GenerateUsername(username);
            
        // Assert
        Assert.Equal(11, generatedUsername.Length); 
    }

    [Fact]
    public void GenerateUsername_SimpleUsername_OnlyAllowedChars()
    {
        // Arrange
        var username = "user";
            
        // Act
        var generatedUsername = UsernameGenerateHelper.GenerateUsername(username);
            
        // Assert
        Assert.True(generatedUsername.All(c => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_".Contains(c)));
    }
}