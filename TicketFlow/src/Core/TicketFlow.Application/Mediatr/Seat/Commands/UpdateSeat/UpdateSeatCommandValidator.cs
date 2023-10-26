using FluentValidation;

namespace TicketFlow.Application.Mediatr.Seat.Commands.UpdateSeat;

public class UpdateSeatCommandValidator : AbstractValidator<UpdateSeatCommand>
{
    public UpdateSeatCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(36).WithMessage("{PropertyName} must be 36 characters long.")
            .MaximumLength(36).WithMessage("{PropertyName} must be 36 characters long.");
        
        RuleFor(command => command.HallId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(36).WithMessage("{PropertyName} must be 36 characters long.")
            .MaximumLength(36).WithMessage("{PropertyName} must be 36 characters long.");
        
        RuleFor(command => command.Row)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
            .LessThan(1000).WithMessage("{PropertyName} must be less than 1000.");
        
        RuleFor(command => command.Number)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
            .LessThan(1000).WithMessage("{PropertyName} must be less than 1000.");
        
        RuleFor(command => command.Status)
            .IsInEnum().WithMessage("{PropertyName} must have a valid value.");
    }
}