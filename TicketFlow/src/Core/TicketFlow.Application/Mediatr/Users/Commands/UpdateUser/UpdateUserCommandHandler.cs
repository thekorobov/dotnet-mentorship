using MediatR;
using Microsoft.AspNetCore.Identity;
using TicketFlow.Application.Common.Exceptions;
using TicketFlow.Application.Common.Exceptions.Users;
using TicketFlow.Application.Mediatr.VerificationCodes.Commands.ResetVerificationCode;
using TicketFlow.Application.Utils;
using TicketFlow.Application.Utils.Generators;
using TicketFlow.Domain.Entities;
using TicketFlow.Domain.Entities.Filters;
using TicketFlow.Domain.Repositories;

namespace TicketFlow.Application.Mediatr.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Id != command.CurrentUserId)
        {
            throw new PermissionDeniedException("You don't have permission to update this user.");
        }
        
        var existingUser = await _unitOfWork.Users.GetAsync
            (new UserFilter { Id = command.CurrentUserId }, cancellationToken);

        if (existingUser == null)
        {
            throw new EntityNotFoundException(nameof(User));
        }
        
        existingUser.UserName = NullSafeUpdateHelper.UpdateIfNotNullOrEmpty
            (existingUser.UserName!, UsernameGenerator.GenerateUsername(command.UserName));
        existingUser.Email = NullSafeUpdateHelper.UpdateIfNotNullOrEmpty
            (existingUser.Email!, command.Email);
        existingUser.Surname = NullSafeUpdateHelper.UpdateIfNotNullOrEmpty
            (existingUser.Surname, command.Surname);
        existingUser.Forename = NullSafeUpdateHelper.UpdateIfNotNullOrEmpty
            (existingUser.Forename, command.Forename);
        
        if (!string.IsNullOrEmpty(command.Password))
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result = await _userManager.ResetPasswordAsync(existingUser, resetToken, command.Password);

            if (!result.Succeeded)
            {
                throw new PasswordResetFailedException("Failed to reset password: " + string
                    .Join(", ", result.Errors.Select(x => x.Description)));
            }
        }

        await _unitOfWork.Users.UpdateAsync(existingUser, cancellationToken);
        
        if (!string.IsNullOrEmpty(command.Email))
        {
            var resetVerificationCodeCommand = new ResetVerificationCodeCommand()
            {
                UserId = command.Id,
                CurrentUserId = command.CurrentUserId
            };
            await _mediator.Send(resetVerificationCodeCommand, cancellationToken);   
        }
        
        return Unit.Value;
    }
}