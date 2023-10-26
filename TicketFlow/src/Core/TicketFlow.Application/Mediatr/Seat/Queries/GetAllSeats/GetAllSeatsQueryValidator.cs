using FluentValidation;

namespace TicketFlow.Application.Mediatr.Seat.Queries.GetAllSeats;

public class GetAllSeatsQueryValidator : AbstractValidator<GetAllSeatsQuery>
{
    public GetAllSeatsQueryValidator()
    {
        RuleFor(query => query)
            .Must(query => 
                !string.IsNullOrEmpty(query.VenueId) ||
                !string.IsNullOrEmpty(query.HallId) ||
                query.Status.HasValue)
            .WithMessage("At least one property must be set.");
    }
}