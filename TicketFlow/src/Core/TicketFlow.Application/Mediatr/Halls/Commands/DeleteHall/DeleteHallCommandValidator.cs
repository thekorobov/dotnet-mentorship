using FluentValidation;

namespace TicketFlow.Application.Mediatr.Halls.Commands.DeleteHall;

public class DeleteHallCommandValidator : AbstractValidator<DeleteHallCommand>
{
    public DeleteHallCommandValidator()
    {
        RuleFor(command => command.Id).NotEqual(String.Empty);
        RuleFor(command => command.UserId).NotEqual(String.Empty);
    }
}