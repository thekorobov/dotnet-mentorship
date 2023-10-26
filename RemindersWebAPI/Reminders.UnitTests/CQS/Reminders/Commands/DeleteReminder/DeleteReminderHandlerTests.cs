using Reminders.BLL.CQS.Reminders.Commands.DeleteReminder;

namespace Reminders.UnitTests.CQS.Reminders.Commands.DeleteReminder;

public class DeleteReminderHandlerTests : IClassFixture<ReminderHandlerFixture>
{
    private readonly ReminderHandlerFixture _fixture;
    private readonly DeleteReminderHandler _deleteReminderHandler;

    public DeleteReminderHandlerTests(ReminderHandlerFixture fixture)
    {
        _fixture = fixture;
        _deleteReminderHandler = new DeleteReminderHandler(_fixture.MockUnitOfWork.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_ReminderDeleted()
    {
        // Arrange
        var reminder = new Reminder { UserId = 1, Id = 1 };
        var command = new DeleteReminderCommand { UserId = 1, Id = 1 };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>())).ReturnsAsync(reminder);

        // Act
        await _deleteReminderHandler.HandleAsync(command);

        // Assert
        _fixture.MockReminderRepository.Verify(repo => repo.DeleteAsync(
            It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_UnauthorizedUser_UnauthorizedAccessExceptionThrown()
    {
        // Arrange
        var reminder = new Reminder { UserId = 2, Id = 1 };
        var command = new DeleteReminderCommand { UserId = 1, Id = 1 };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>())).ReturnsAsync(reminder);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _deleteReminderHandler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_ReminderNotFound_EntityNotFoundExceptionThrown()
    {
        // Arrange
        Reminder? reminder = null;
        var command = new DeleteReminderCommand { UserId = 1, Id = 1 };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>()))!.ReturnsAsync(reminder);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _deleteReminderHandler.HandleAsync(command));
    }
}