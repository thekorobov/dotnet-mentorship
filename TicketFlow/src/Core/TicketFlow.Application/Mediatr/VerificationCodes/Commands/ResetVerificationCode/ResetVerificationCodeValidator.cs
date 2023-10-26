using FluentValidation;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.ResetVerificationCode;

public class ResetVerificationCodeValidator : AbstractValidator<ResetVerificationCodeCommand>
{
    public ResetVerificationCodeValidator()
    {
        RuleFor(command => command.UserId).NotEqual(String.Empty);
        RuleFor(command => command.CurrentUserId).NotEqual(String.Empty);
    }
}