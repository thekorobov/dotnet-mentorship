using Reminders.BLL.CQS.Reminders.Commands.UpdateReminder;

namespace Reminders.UnitTests.CQS.Reminders.Commands.UpdateReminder;

public class UpdateReminderValidatorTests
{
    private readonly UpdateReminderValidator _validator;

    public UpdateReminderValidatorTests()
    {
        _validator = new UpdateReminderValidator();
    }

    [Fact]
    public void Validate_EmptyName_ErrorGenerated()
    {
        var command = new UpdateReminderCommand { Id = 1, UserId = 1, Name = "", Date = DateTime.Now.AddDays(1) };
        var result = _validator.Validate(command);
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_PastDate_ErrorGenerated()
    {
        var command = new UpdateReminderCommand { Id = 1, UserId = 1, Name = "Test", Date = DateTime.Now.AddDays(-1) };
        var result = _validator.Validate(command);
        result.Errors.Should().Contain(e => e.PropertyName == "Date");
    }

    [Fact]
    public void Validate_ValidCommand_NoErrors()
    {
        var command = new UpdateReminderCommand { Id = 1, UserId = 1, Name = "Test", Date = DateTime.Now.AddDays(1) };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}