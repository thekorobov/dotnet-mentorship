using FluentValidation;

namespace TicketFlow.Application.Mediatr.Halls.Queries.GetHall;

public class GetHallQueryValidator : AbstractValidator<GetHallQuery>
{
    public GetHallQueryValidator()
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