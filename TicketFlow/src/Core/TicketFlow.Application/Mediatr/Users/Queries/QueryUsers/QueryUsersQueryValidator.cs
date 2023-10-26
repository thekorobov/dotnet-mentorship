using FluentValidation;

namespace TicketFlow.Application.Mediatr.Users.Queries.QueryUsers;

public class QueryUsersQueryValidator : AbstractValidator<QueryUsersQuery>
{
    public QueryUsersQueryValidator()
    {
        RuleFor(query => query)
            .Must(query => !string.IsNullOrEmpty(query.Id) ||
                       !string.IsNullOrEmpty(query.Email) ||
                       !string.IsNullOrEmpty(query.UserName) ||
                       !string.IsNullOrEmpty(query.Forename) ||
                       !string.IsNullOrEmpty(query.Surname) ||
                       !string.IsNullOrEmpty(query.Role))
            .WithMessage("At least one property must be not null.");
    }
}