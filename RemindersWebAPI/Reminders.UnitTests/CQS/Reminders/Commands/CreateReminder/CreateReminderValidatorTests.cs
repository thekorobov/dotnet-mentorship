using Reminders.BLL.CQS.Reminders.Commands.CreateReminder;

namespace Reminders.UnitTests.CQS.Reminders.Commands.CreateReminder;

public class CreateReminderValidatorTests
{
    private readonly CreateReminderValidator _validator;

    public CreateReminderValidatorTests()
    {
        _validator = new CreateReminderValidator();
    }

    [Fact]
    public void Validate_EmptyName_ErrorGenerated()
    {
        var command = new CreateReminderCommand { UserId = 1, Name = "", Date = DateTime.Now.AddDays(1) };
        var result = _validator.Validate(command);
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_PastDate_ErrorGenerated()
    {
        var command = new CreateReminderCommand { UserId = 1, Name = "Test", Date = DateTime.Now.AddDays(-1) };
        var result = _validator.Validate(command);
        result.Errors.Should().Contain(e => e.PropertyName == "Date");
    }

    [Fact]
    public void Validate_ValidCommand_NoErrors()
    {
        var command = new CreateReminderCommand { UserId = 1, Name = "Test", Date = DateTime.Now.AddDays(1) };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}