using FluentValidation;

namespace TicketFlow.Application.Mediatr.Venues.Commands.CreateVenue;

public class CreateVenueCommandValidator : AbstractValidator<CreateVenueCommand>
{
    public CreateVenueCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(36).WithMessage("{PropertyName} must be 36 characters long.")
            .MaximumLength(36).WithMessage("{PropertyName} must be 36 characters long.");
        
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters long.");
        
        RuleFor(command => command.Address)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(5).WithMessage("{PropertyName} must be at least 5 characters long.");
        
        RuleFor(command => command.SeatingCapacity)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
            .LessThan(100_000).WithMessage("{PropertyName} must be less than 100 000.");
    }
}