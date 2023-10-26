using Microsoft.AspNetCore.Identity;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Users.Commands.UpdateUser;

public class ResetUserPasswordHandler : ICommandHandler<ResetUserPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    
    public ResetUserPasswordHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task HandleAsync(ResetUserPasswordCommand passwordCommand)
    {
        if (passwordCommand.Id != passwordCommand.CurrentUserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this user.");
        }
        
        var userFilter  = new UserFilter { Id = passwordCommand.Id };
        var existingUser = await _unitOfWork.Users.GetAsync(userFilter );

        if (existingUser == null)
        {
            throw new EntityNotFoundException(nameof(User));
        }
    
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
        var result = await _userManager.ResetPasswordAsync(existingUser, resetToken, passwordCommand.Password);
    
        if (!result.Succeeded)
        {
            throw new PasswordResetFailedException("Failed to reset password: " + string.Join(", ", result.Errors.Select(x => x.Description)));
        }
    }
}