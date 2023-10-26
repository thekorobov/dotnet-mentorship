namespace Reminders.UnitTests.Repositories;

public class ReminderRepositoryTests : IClassFixture<RepositoriesFixture>
{
    private readonly RepositoriesFixture _fixture;
    private readonly ApplicationDbContext _context;

    public ReminderRepositoryTests(RepositoriesFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.Context;
    }

    [Fact]
    public async Task CreateAsync_ValidReminder_ReminderCreated()
    {
        // Arrange
        var reminder = new Reminder { UserId = Constants.ValidUserId, Name = Constants.FirstValidReminderName, Date = DateTime.Now.AddMinutes(10) };

        // Act
        int createdId = await _fixture.ReminderRepository.CreateAsync(reminder);
        
        // Assert
        Assert.NotEqual(0, createdId);
        
        var savedReminder = _fixture.Context.Reminders.Find(createdId);
        Assert.NotNull(savedReminder);
        Assert.Equal(Constants.FirstValidReminderName, savedReminder.Name);
    }

    [Fact]
    public async Task CreateAsync_InvalidUserId_NoReminderCreated()
    {
        // Arrange
        var reminder = new Reminder { UserId = Constants.InvalidUserId, Name = Constants.FirstValidReminderName, Date = DateTime.Now.AddMinutes(10) };

        // Act
        int createdId = await _fixture.ReminderRepository.CreateAsync(reminder);

        // Assert
        Assert.Equal(0, createdId);
    }
    
    [Fact]
    public async Task UpdateAsync_ValidReminder_ReminderUpdated()
    {
        // Arrange
        var reminder = new Reminder { Id = Constants.ExistingReminderId2, UserId = Constants.ValidUserId, Name = Constants.FirstValidReminderName, Date = DateTime.Now.AddMinutes(10) };
        await _fixture.ReminderRepository.CreateAsync(reminder);

        // Act
        reminder.Name = Constants.SecondValidReminderName;
        await _fixture.ReminderRepository.UpdateAsync(reminder);

        // Assert
        var updatedReminder = await _fixture.Context.Reminders.FindAsync(Constants.ExistingReminderId2);
        Assert.Equal(Constants.SecondValidReminderName, updatedReminder.Name);
    }
    
    [Fact]
    public async Task UpdateAsync_InvalidUserId_NoUpdates()
    {
        // Arrange
        var reminder = new Reminder { Id = Constants.ExistingReminderId2, UserId = Constants.InvalidUserId, Name = Constants.FirstValidReminderName, Date = DateTime.Now.AddMinutes(10) };

        // Act
        await _fixture.ReminderRepository.UpdateAsync(reminder);

        // Assert
        var notUpdatedReminder = await _fixture.Context.Reminders.FindAsync(Constants.ExistingReminderId2);
        Assert.NotEqual(Constants.FirstValidReminderName, notUpdatedReminder.Name);
    }


    [Fact]
    public async Task GetAsync_ValidId_ReminderReturned()
    {
        // Arrange
        var reminder = new Reminder { Id = Constants.ExistingReminderId1, UserId = Constants.ValidUserId, Name = Constants.FirstValidReminderName, Date = DateTime.Now.AddMinutes(10) };
        await _fixture.ReminderRepository.CreateAsync(reminder);

        // Act
        var result = await _fixture.ReminderRepository.GetAsync(new ReminderFilter { Id = Constants.ExistingReminderId1 });

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Constants.ExistingReminderId1, result.Id);
    }

    [Fact]
    public async Task GetAsync_InvalidId_NoReminderReturned()
    {
        // Act
        var result = await _fixture.ReminderRepository.GetAsync(new ReminderFilter { Id = Constants.NonExistingReminderId });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_RemindersExist_RemindersReturned()
    {
        // Arrange
        // Act
        var allReminders = await _fixture.ReminderRepository.GetAllAsync();

        // Assert
        Assert.Equal(5, allReminders.Count());
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReminderDeleted()
    {
        // Arrange
        // Act
        await _fixture.ReminderRepository.DeleteAsync(1);

        // Assert
        var deletedReminder = await _fixture.Context.Reminders.FindAsync(1);
        Assert.Null(deletedReminder);
    }
    
    [Fact]
    public void GetQueryable_ReturnsExpectedQueryable()
    {
        // Arrange
        var repository = new ReminderRepository(_fixture.Context);

        // Act
        var actualReminders = repository.GetQueryable().ToList();

        // Assert
        Assert.Equal(5, actualReminders.Count);
    }
}