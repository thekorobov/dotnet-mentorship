using FluentValidation;

namespace TicketFlow.Application.Mediatr.Halls.Queries.GetAllHall;

public class GetAllHallsQueryValidator : AbstractValidator<GetAllHallsQuery>
{
    public GetAllHallsQueryValidator()
    {
        RuleFor(query => query)
            .Must(query => 
                !string.IsNullOrEmpty(query.VenueId) ||
                query.IncludeSeats.HasValue)
            .WithMessage("At least one property must be set.");
    }
}