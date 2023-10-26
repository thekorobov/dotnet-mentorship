using FluentValidation;

namespace TicketFlow.Application.Mediatr.Venues.Queries.GetAllVenues;

public class GetAllVenuesQueryValidator : AbstractValidator<GetAllVenuesQuery>
{
    public GetAllVenuesQueryValidator()
    {
        RuleFor(query => query)
            .Must(query => 
                !string.IsNullOrEmpty(query.UserId) ||
                query.IncludeHalls.HasValue ||
                query.IncludeSeats.HasValue)
            .WithMessage("At least one property must be set.");
    }
}