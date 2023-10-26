using FluentValidation;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.VerifyVerificationCode;

public class VerifyVerificationCodeValidator : AbstractValidator<VerifyVerificationCodeCommand>
{
    public VerifyVerificationCodeValidator()
    {
        RuleFor(command => command.VerificationToken).NotEqual(String.Empty);
    }
}