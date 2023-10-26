using FluentValidation;

namespace TicketFlow.Application.Mediatr.Halls.Queries.QueryHalls;

public class QueryHallsQueryValidator : AbstractValidator<QueryHallsQuery>
{
    public QueryHallsQueryValidator()
    {
        RuleFor(query => query)
            .Must(query => 
                !string.IsNullOrEmpty(query.Id) ||
                !string.IsNullOrEmpty(query.VenueId) ||
                !string.IsNullOrEmpty(query.Name) ||
                query.SeatingCapacity.HasValue ||
                query.IncludeSeats.HasValue)
            .WithMessage("At least one property must be set.");
    }
}