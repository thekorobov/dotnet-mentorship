using FluentValidation;

namespace Reminders.BLL.CQS.Users.Commands.ResetUserEmail;

public class ResetUserEmailValidator : AbstractValidator<ResetUserEmailCommand>
{
    public ResetUserEmailValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}