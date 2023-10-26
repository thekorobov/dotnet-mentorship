using FluentValidation;

namespace TicketFlow.Application.Mediatr.Venues.Queries.GetVenue;

public class GetVenueQueryValidator : AbstractValidator<GetVenueQuery>
{
    public GetVenueQueryValidator()
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