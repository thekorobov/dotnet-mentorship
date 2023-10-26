using FluentValidation;

namespace TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetVerificationCode;

public class GetVerificationCodeValidator : AbstractValidator<GetVerificationCodeQuery>
{
    public GetVerificationCodeValidator()
    {
        RuleFor(query => query)
            .Must(query => !string.IsNullOrEmpty(query.UserId) ||
                       !string.IsNullOrEmpty(query.VerificationToken) ||
                       !string.IsNullOrEmpty(query.VerificationCodeId))
            .WithMessage("At least one property must be not null.");
    }
}