using Reminders.BLL.CQS.Users.Commands.UpdateUser;

namespace Reminders.UnitTests.Users.Commands.ResetUserPassword;

public class ResetUserPasswordValidatorTests
{
    private readonly ResetUserPasswordValidator _validator;

    public ResetUserPasswordValidatorTests()
    {
        _validator = new ResetUserPasswordValidator();
    }

    [Fact]
    public void Validate_PasswordIsEmpty_HasValidationError()
    {
        var command = new ResetUserPasswordCommand() { Password = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_InvalidPassword_HasValidationError()
    {
        var command = new ResetUserPasswordCommand { Password = "123" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }
}