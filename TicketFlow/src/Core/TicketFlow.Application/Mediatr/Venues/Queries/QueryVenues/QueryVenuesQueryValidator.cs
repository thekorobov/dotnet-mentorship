using FluentValidation;
using TicketFlow.Application.Mediatr.Venues.Queries.GetVenue;

namespace TicketFlow.Application.Mediatr.Venues.Queries.QueryVenues;

public class QueryVenuesQueryValidator : AbstractValidator<GetVenueQuery>
{
    public QueryVenuesQueryValidator()
    {
        RuleFor(query => query)
            .Must(query => 
                !string.IsNullOrEmpty(query.UserId) ||
                !string.IsNullOrEmpty(query.Id) ||
                !string.IsNullOrEmpty(query.Address) ||
                !string.IsNullOrEmpty(query.Name) ||
                query.SeatingCapacity.HasValue ||
                query.IncludeHalls.HasValue ||
                query.IncludeSeats.HasValue)
            .WithMessage("At least one property must be set.");
    }
}