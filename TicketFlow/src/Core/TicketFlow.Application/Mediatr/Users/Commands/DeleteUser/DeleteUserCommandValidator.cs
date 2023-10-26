using FluentValidation;

namespace TicketFlow.Application.Mediatr.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(command => command.Id).NotEqual(String.Empty);
        RuleFor(command => command.CurrentUserId).NotEqual(String.Empty);
    }
}