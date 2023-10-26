using FluentValidation;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Commands.CreateVerificationCode;

public class CreateVerificationCodeValidator : AbstractValidator<CreateVerificationCodeCommand>
{
    public CreateVerificationCodeValidator()
    {
        RuleFor(command => command.UserId).NotEqual(String.Empty);
    }
}