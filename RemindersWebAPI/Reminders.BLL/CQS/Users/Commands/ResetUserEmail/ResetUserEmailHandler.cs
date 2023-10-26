using AutoMapper;
using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Utils;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Users.Commands.ResetUserEmail;

public class ResetUserEmailHandler : ICommandHandler<ResetUserEmailCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ResetUserEmailHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(ResetUserEmailCommand command)
    {
        if (command.Id != command.CurrentUserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this user.");
        }

        var existingUser = await _unitOfWork.Users.GetAsync(new UserFilter { Id = command.Id });

        if (existingUser == null)
        {
            throw new EntityNotFoundException(nameof(User));
        }
    
        existingUser.Email = command.Email;
        await _unitOfWork.Users.UpdateAsync(existingUser);
    }
}