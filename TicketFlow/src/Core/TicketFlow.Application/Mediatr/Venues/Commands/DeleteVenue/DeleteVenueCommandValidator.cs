using FluentValidation;

namespace TicketFlow.Application.Mediatr.Venues.Commands.DeleteVenue;

public class DeleteVenueCommandValidator : AbstractValidator<DeleteVenueCommand>
{
    public DeleteVenueCommandValidator()
    {
        RuleFor(command => command.Id).NotEqual(String.Empty);
        RuleFor(command => command.UserId).NotEqual(String.Empty);
    }
}