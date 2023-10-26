using FluentValidation;

namespace Reminders.BLL.CQS.Reminders.Commands.CreateReminder;

public class CreateReminderValidator : AbstractValidator<CreateReminderCommand>
{
    public CreateReminderValidator()
    {
        RuleFor(cmd => cmd.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(cmd => cmd.Date).Must(CheckIfDateInThePast).WithMessage("Cannot add a reminder for a past date.");
    }

    private static bool CheckIfDateInThePast(DateTime date)
    {
        return date >= DateTime.Now;
    }
}