using Reminders.BLL.CQS.Reminders.Queries.GetReminderById;

namespace Reminders.UnitTests.CQS.Reminders.Queries.GetReminder;

public class GetReminderByIdHandlerTests : IClassFixture<ReminderHandlerFixture>
{
    private readonly ReminderHandlerFixture _fixture;
    private readonly GetReminderByIdHandler _getReminderByIdHandler;

    public GetReminderByIdHandlerTests(ReminderHandlerFixture fixture)
    {
        _fixture = fixture;
        _getReminderByIdHandler = new GetReminderByIdHandler(_fixture.MockUnitOfWork.Object, _fixture.MockMapper.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidQuery_ReminderDtoReturned()
    {
        // Arrange
        var reminder = new Reminder { UserId = 1, Id = 1 };
        var query = new GetReminderByIdQuery { UserId = 1, Id = 1 };
        var reminderDto = new ReminderDto();

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>())).ReturnsAsync(reminder);
        _fixture.MockMapper.Setup(mapper => mapper.Map<ReminderDto>(
            It.IsAny<Reminder>())).Returns(reminderDto);

        // Act
        var result = await _getReminderByIdHandler.HandleAsync(query);

        // Assert
        Assert.Equal(reminderDto, result);
    }

    [Fact]
    public async Task HandleAsync_UnauthorizedUser_UnauthorizedAccessExceptionThrown()
    {
        // Arrange
        var reminder = new Reminder { UserId = 2, Id = 1 };
        var query = new GetReminderByIdQuery { UserId = 1, Id = 1 };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>())).ReturnsAsync(reminder);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _getReminderByIdHandler.HandleAsync(query));
    }

    [Fact]
    public async Task HandleAsync_ReminderNotFound_EntityNotFoundExceptionThrown()
    {
        // Arrange
        Reminder? reminder = null;
        var query = new GetReminderByIdQuery { UserId = 1, Id = 1 };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAsync(
            It.IsAny<ReminderFilter>())).ReturnsAsync(reminder);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _getReminderByIdHandler.HandleAsync(query));
    }
}