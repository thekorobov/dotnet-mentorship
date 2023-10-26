using Reminders.BLL.DTO.Exceptions;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Utils.Constants;
using Reminders.DAL.Entities;
using Reminders.DAL.Entities.Filtres;
using Reminders.DAL.Interfaces;

namespace Reminders.BLL.CQS.Users.Commands.DeleteUser;

public class DeleteUserHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(DeleteUserCommand command)
    {
        var filter = new UserFilter { Id = command.CurrentUserId };
        
        var user = await _unitOfWork.Users.GetAsync(filter);

        if (user == null)
        {
            throw new EntityNotFoundException(nameof(User), command.Id);
        }

        var userRole = user.Role;

        if (userRole == UserRoles.Admin)
        {
            if (user != null)
            {
                await _unitOfWork.Users.DeleteAsync(command.Id);
            }
            else
            {
                throw new EntityNotFoundException(nameof(User), command.Id);
            }   
        }
        else
        {
            throw new UnauthorizedAccessException("Access to the user is denied.");
        }
    }
}