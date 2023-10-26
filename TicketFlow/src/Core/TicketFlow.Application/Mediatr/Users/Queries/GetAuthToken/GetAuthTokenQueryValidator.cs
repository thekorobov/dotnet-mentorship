using FluentValidation;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetAuthToken;

public class GetAuthTokenQueryValidator : AbstractValidator<GetAuthTokenQuery>
{
    public GetAuthTokenQueryValidator()
    {
        RuleFor(query => query.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("Invalid {PropertyName} format.");
        
        RuleFor(query => query.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters long.");
    }
}