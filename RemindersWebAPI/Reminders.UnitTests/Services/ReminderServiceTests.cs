using Microsoft.Extensions.DependencyInjection;

namespace Reminders.UnitTests.Services;

public class ReminderServiceTests
{
    [Fact]
    public async Task CheckRemindersAndSendEmailsAsync_ValidScenario_EmailsSent1()
    {
        // Arrange
        var verificationCodes = new List<VerificationCode>
        {
            new VerificationCode { UserId = 1, VerifiedAt = DateTime.UtcNow },
            new VerificationCode { UserId = 2, VerifiedAt = DateTime.UtcNow }
        }.AsQueryable().BuildMock();

        var reminders = new List<Reminder>
        {
            new Reminder { UserId = 1, Date = DateTime.UtcNow },
            new Reminder { UserId = 2, Date = DateTime.UtcNow }
        }.AsQueryable().BuildMock();

        var users = new List<User>
        {
            new User { Id = 1, Email = "email1@test.com" },
            new User { Id = 2, Email = "email2@test.com" }
        };

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(u => u.GetAsync(It.IsAny<UserFilter>()))
            .Returns<UserFilter>(filter => Task.FromResult(users.FirstOrDefault(u => u.Id == filter.Id)));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.VerificationCodes.GetQueryable()).Returns(verificationCodes);
        mockUnitOfWork.Setup(u => u.Reminders.GetQueryable()).Returns(reminders);
        mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);

        var mockEmailService = new Mock<IEmailService>();

        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider.Setup(sp => sp.GetService(typeof(IUnitOfWork)))
            .Returns(mockUnitOfWork.Object);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(IEmailService)))
            .Returns(mockEmailService.Object);

        var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
        var mockServiceScope = new Mock<IServiceScope>();
        mockServiceScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);
        mockServiceScopeFactory.Setup(s => s.CreateScope()).Returns(mockServiceScope.Object);

        var reminderService = new ReminderService(mockUnitOfWork.Object, mockServiceScopeFactory.Object);

        // Act
        await reminderService.CheckRemindersAndSendEmailsAsync();

        // Assert
        mockEmailService.Verify(e => e.SendEmailAsync(It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task CheckRemindersAndSendEmailsAsync_UserNotFound_ThrowsException()
    {
        // Arrange
        var verificationCodes = new List<VerificationCode>
        {
            new VerificationCode { UserId = 1, VerifiedAt = DateTime.UtcNow }
        }.AsQueryable().BuildMock();

        var reminders = new List<Reminder>
        {
            new Reminder { UserId = 1, Date = DateTime.UtcNow }
        }.AsQueryable().BuildMock();

        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(u => u.GetAsync(It.IsAny<UserFilter>()))
            .ReturnsAsync((User)null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.VerificationCodes.GetQueryable())
            .Returns(verificationCodes);
        mockUnitOfWork.Setup(u => u.Reminders.GetQueryable()).Returns(reminders);
        mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);

        var mockEmailService = new Mock<IEmailService>();

        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider.Setup(sp => sp.GetService(typeof(IUnitOfWork)))
            .Returns(mockUnitOfWork.Object);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(IEmailService)))
            .Returns(mockEmailService.Object);

        var mockServiceScope = new Mock<IServiceScope>();
        mockServiceScope.Setup(s => s.ServiceProvider)
            .Returns(mockServiceProvider.Object);

        var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
        mockServiceScopeFactory.Setup(s => s.CreateScope())
            .Returns(mockServiceScope.Object);

        var reminderService = new ReminderService(mockUnitOfWork.Object, mockServiceScopeFactory.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => reminderService
            .CheckRemindersAndSendEmailsAsync());
    }
}