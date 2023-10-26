using FluentValidation;

namespace TicketFlow.Application.Mediatr.Users.Queries.GetUser;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(query => query)
            .Must(x => !string.IsNullOrEmpty(x.Id) ||
                       !string.IsNullOrEmpty(x.Email) ||
                       !string.IsNullOrEmpty(x.UserName))
            .WithMessage("At least one property must be not null.");
    }
}