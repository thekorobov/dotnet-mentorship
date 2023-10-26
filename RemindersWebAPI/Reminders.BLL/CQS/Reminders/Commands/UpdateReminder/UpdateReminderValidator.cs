using FluentValidation;

namespace Reminders.BLL.CQS.Reminders.Commands.UpdateReminder;

public class UpdateReminderValidator : AbstractValidator<UpdateReminderCommand>
{
    public UpdateReminderValidator()
    {
        RuleFor(cmd => cmd.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(cmd => cmd.Date).Must(CheckIfDateInThePast).WithMessage("Cannot add a reminder for a past date.");
    }

    private static bool CheckIfDateInThePast(DateTime date)
    {
        return date >= DateTime.Now;
    }
}