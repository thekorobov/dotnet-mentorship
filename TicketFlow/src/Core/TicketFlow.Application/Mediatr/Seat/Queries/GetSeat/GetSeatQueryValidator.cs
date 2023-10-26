using FluentValidation;

namespace TicketFlow.Application.Mediatr.Seat.Queries.GetSeat;

public class GetSeatQueryValidator : AbstractValidator<GetSeatQuery>
{
    public GetSeatQueryValidator()
    {
        RuleFor(query => query)
            .Must(query => 
                !string.IsNullOrEmpty(query.Id) ||
                !string.IsNullOrEmpty(query.Id) ||
                !string.IsNullOrEmpty(query.HallId) ||
                query.Row.HasValue ||
                query.Number.HasValue ||
                query.Status.HasValue) 
            .WithMessage("At least one property must be set.");
    }
}