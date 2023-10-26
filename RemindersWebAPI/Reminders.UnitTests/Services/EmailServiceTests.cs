using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Reminders.UnitTests.Services;

public class EmailServiceTests
{
    [Fact]
    public async Task SendEmailAsync_ValidParameters_EmailSentSuccessfully()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(c => c["Mailkit:EmailUsername"]).Returns("username");
        mockConfiguration.SetupGet(c => c["Mailkit:EmailHost"]).Returns("host");
        mockConfiguration.SetupGet(c => c["Mailkit:EmailPassword"]).Returns("password");

        var mockSmtpClientWrapper = new Mock<ISmtpClientWrapper>();
        mockSmtpClientWrapper
            .Setup(c => c.ExecuteEmailActionsAsync(
                It.IsAny<string>(), It.IsAny<int>(), 
                It.IsAny<SecureSocketOptions>(), It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<MimeMessage>()))
            .Returns(Task.CompletedTask);

        var emailService = new EmailService(mockConfiguration.Object, mockSmtpClientWrapper.Object);

        // Act
        await emailService.SendEmailAsync("recipient@example.com", "subject", "body");

        // Assert
        mockSmtpClientWrapper.Verify(c => c.ExecuteEmailActionsAsync(
            It.IsAny<string>(), It.IsAny<int>(), 
            It.IsAny<SecureSocketOptions>(), It.IsAny<string>(), 
            It.IsAny<string>(), It.IsAny<MimeMessage>()), Times.Once);
    }
    
    [Fact]
    public async Task SendEmailAsync_InvalidArguments_ThrowsArgumentException()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        var mockSmtpClientWrapper = new Mock<ISmtpClientWrapper>();
        var emailService = new EmailService(mockConfiguration.Object, mockSmtpClientWrapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => emailService
            .SendEmailAsync("", "subject", "body"));
        await Assert.ThrowsAsync<ArgumentException>(() => emailService
            .SendEmailAsync("recipient@example.com", "", "body"));
        await Assert.ThrowsAsync<ArgumentException>(() => emailService
            .SendEmailAsync("recipient@example.com", "subject", ""));
    }
}