using Reminders.BLL.CQS.Reminders.Queries.GetAllReminders;

namespace Reminders.UnitTests.CQS.Reminders.Queries.GetAllReminders;

public class GetAllRemindersHandlerTests : IClassFixture<ReminderHandlerFixture>
{
    private readonly ReminderHandlerFixture _fixture;
    private readonly GetAllRemindersHandler _getAllRemindersHandler;

    public GetAllRemindersHandlerTests(ReminderHandlerFixture fixture)
    {
        _fixture = fixture;
        _getAllRemindersHandler = new GetAllRemindersHandler(_fixture.MockUnitOfWork.Object, _fixture.MockMapper.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidQuery_ReminderDtoListReturned()
    {
        // Arrange
        var reminders = new List<Reminder>
        {
            new Reminder { UserId = 1, Id = 1 },
            new Reminder { UserId = 1, Id = 2 }
        };
        var query = new GetAllRemindersQuery { UserId = 1 };
        var reminderDtos = new List<ReminderDto>();

        _fixture.MockReminderRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(reminders);
        _fixture.MockMapper.Setup(mapper => mapper.Map<List<Reminder>, List<ReminderDto>>(
                It.IsAny<List<Reminder>>()))
            .Returns(new List<ReminderDto>());

        // Act
        var result = await _getAllRemindersHandler.HandleAsync(query);

        // Assert
        Assert.Equal(reminderDtos, result);
    }

    [Fact]
    public async Task HandleAsync_NoRemindersForUser_EmptyListReturned()
    {
        // Arrange
        var reminders = new List<Reminder>
        {
            new Reminder { UserId = 2, Id = 3 },
            new Reminder { UserId = 2, Id = 4 }
        };
        var query = new GetAllRemindersQuery { UserId = 1 };

        _fixture.MockReminderRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(reminders);

        // Act
        var result = await _getAllRemindersHandler.HandleAsync(query);

        // Assert
        Assert.Empty(result);
    }
}