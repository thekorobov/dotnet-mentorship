using FluentValidation;

namespace TicketFlow.Application.Mediatr.Seat.Commands.DeleteSeat;

public class DeleteSeatCommandValidator : AbstractValidator<DeleteSeatCommand>
{
    public DeleteSeatCommandValidator()
    {
        RuleFor(command => command.Id).NotEqual(String.Empty);
        RuleFor(command => command.UserId).NotEqual(String.Empty);
    }
}