using FluentValidation;

namespace TicketFlow.Application.Mediatr.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("Invalid {PropertyName} format.");
        
        RuleFor(command => command.UserName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(5).WithMessage("{PropertyName} must be at least 5 characters long.");
        
        RuleFor(command => command.Surname)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(3).WithMessage("{PropertyName} must be at least 5 characters long.");
        
        RuleFor(command => command.Forename)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(3).WithMessage("{PropertyName} must be at least 5 characters long.");
        
        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters long.");
    }
}