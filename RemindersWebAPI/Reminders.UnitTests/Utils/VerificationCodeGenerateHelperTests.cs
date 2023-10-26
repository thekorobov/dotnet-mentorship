using Reminders.BLL.Utils;

namespace Reminders.UnitTests.Utils;

public class VerificationCodeGenerateHelperTests
{
    [Fact]
    public void GenerateVerificationCode_ReturnsHexString()
    {
        // Act
        var code = VerificationCodeGenerateHelper.GenerateVerificationCode();

        // Assert
        Assert.Matches("^[a-fA-F0-9]+$", code);
    }

    [Fact]
    public void GenerateVerificationCode_ReturnsCorrectLength()
    {
        // Act
        var code = VerificationCodeGenerateHelper.GenerateVerificationCode();

        // Assert
        Assert.Equal(32, code.Length);
    }

    [Fact]
    public void GenerateVerificationCode_ReturnsUniqueCodes()
    {
        // Act
        var code1 = VerificationCodeGenerateHelper.GenerateVerificationCode();
        var code2 = VerificationCodeGenerateHelper.GenerateVerificationCode();

        // Assert
        Assert.NotEqual(code1, code2);
    }
}