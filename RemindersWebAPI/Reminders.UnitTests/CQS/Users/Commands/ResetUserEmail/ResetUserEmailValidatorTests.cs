using Reminders.BLL.CQS.Users.Commands.ResetUserEmail;

namespace Reminders.UnitTests.Users.Commands.ResetUserEmail;

public class ResetUserEmailValidatorTests
{
    private readonly ResetUserEmailValidator _validator;

    public ResetUserEmailValidatorTests()
    {
        _validator = new ResetUserEmailValidator();
    }

    [Fact]
    public void Validate_EmailIsEmpty_HasValidationError()
    {
        var command = new ResetUserEmailCommand() { Email = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Validate_InvalidEmail_HasValidationError()
    {
        var command = new ResetUserEmailCommand { Email = "invalidEmail" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }
}