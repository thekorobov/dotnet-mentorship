using FluentValidation.TestHelper;
using Reminders.BLL.CQS.Users.Commands.CreateUser;

namespace Reminders.UnitTests.CQS.Users.Commands.CreateUser;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator;

    public CreateUserValidatorTests()
    {
        _validator = new CreateUserValidator();
    }

    [Fact]
    public void Validate_EmailIsEmpty_HasValidationError()
    {
        var command = new CreateUserCommand { Email = string.Empty, Password = "Password123!" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Validate_InvalidEmail_HasValidationError()
    {
        var command = new CreateUserCommand { Email = "invalidEmail", Password = "Password123!" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Validate_PasswordIsEmpty_HasValidationError()
    {
        var command = new CreateUserCommand { Email = "test@example.com", Password = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_PasswordTooShort_HasValidationError()
    {
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Short" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Validate_ValidEmailAndPassword_NoValidationError()
    {
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Password123!" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}