using FluentValidation;

namespace TicketFlow.Application.Mediatr.Halls.Commands.CreateHall;

public class CreateHallCommandValidator : AbstractValidator<CreateHallCommand>
{
    public CreateHallCommandValidator()
    {
        RuleFor(command => command.VenueId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(36).WithMessage("{PropertyName} must be 36 characters long.")
            .MaximumLength(36).WithMessage("{PropertyName} must be 36 characters long.");
        
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters long.");
        
        RuleFor(command => command.SeatingCapacity)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
            .LessThan(1000).WithMessage("{PropertyName} must be less than 1000.");
        
        RuleFor(command => command.RowsCount)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
            .LessThan(1000).WithMessage("{PropertyName} must be less than 1000.");
        
        RuleFor(command => command.SeatsPerRow)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
            .LessThan(1000).WithMessage("{PropertyName} must be less than 1000.");
    }
}